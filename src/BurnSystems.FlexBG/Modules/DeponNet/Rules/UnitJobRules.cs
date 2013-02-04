using BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using BurnSystems.FlexBG.Modules.LockMasterM;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.WayPointCalculationM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules
{
    /// <summary>
    /// Executes the jobs for the units
    /// </summary>
    public class UnitJobRules : IUnitJobRules
    {
        /// <summary>
        /// Gets or sets the usermanagement to be used
        /// </summary>
        [Inject(IsMandatory = true)]
        public IUnitManagement UnitManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IGameClockManager GameClockManager
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public ILockMaster LockMaster
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IWayPointCalculation WayPointCalculation
        {
            get;
            set;
        }

        [Inject(ByName = "CurrentGame", IsMandatory = true)]
        public long CurrentGameId
        {
            get;
            set;
        }

        public void AddJob(long unitId, UnitM.UnitJobs.IJob job, int position)
        {
            throw new NotImplementedException();
        }

        public void CancelCurrentJob(long unitId)
        {
            throw new NotImplementedException();
        }

        public void ExecuteJob(long unitId)
        {
            var unit = this.UnitManagement.GetUnit(unitId);
            if (unit == null)
            {
                return;
            }

            this.ExecuteJobInternal(unit);
        }

        /// <summary>
        /// Executes the job for all units
        /// </summary>
        public void ExecuteJobForAllUnits()
        {
            using (this.LockMaster.AcquireWriteLock(EntityType.Game, this.CurrentGameId))
            {
                var units = this.UnitManagement.GetAllUnits();

                foreach (var unit in units)
                {
                    this.ExecuteJobInternal(unit);
                }
            }
        }

        /// <summary>
        /// Executes the job internally
        /// </summary>
        /// <param name="unit">Unit, whose job shall be exeucted</param>
        private void ExecuteJobInternal(Unit unit)
        {
            // Check, if we need to activate a job
            var currentJobIndex = unit.IndexCurrentJob;
            var hasActiveJob = false;
            if (currentJobIndex != -1)
            {
                hasActiveJob = true;
            }

            if (!hasActiveJob)
            {
                this.ActivateNextJob(unit);
            }

            // Gets current job
            if (unit.IndexCurrentJob < 0 || unit.IndexCurrentJob >= unit.Jobs.Count)
            {
                // Nothing to do
                return;
            }

            // Gets the current job
            var currentJob = unit.Jobs[unit.IndexCurrentJob];

            if (currentJob is DoNothingJob)
            {
                // Nothing to do...
                return;
            }

            if (currentJob is MoveJob)
            {
                if (this.ExecuteMoveJob(unit, currentJob as MoveJob))
                {
                    this.ActivateNextJob(unit);
                }
            }
        }

        /// <summary>
        /// Executes the move job for the given unit
        /// </summary>
        /// <param name="unit">Unit, whose job shall be executed</param>
        /// <param name="moveJob">The movejob</param>
        private bool ExecuteMoveJob(Unit unit, MoveJob moveJob)
        {
            var tick = 1;
            var finished = false;
            var distance = moveJob.Velocity * tick;

            var currentPosition = unit.Position;
            var targetPosition = moveJob.TargetPosition;

            var wayTo = targetPosition - currentPosition;
            if (wayTo.Length <= distance)
            {
                // We have reached target position, finish!
                distance = wayTo.Length;
                finished = true;
            }

            wayTo.Normalize();
            wayTo *= distance;

            this.UnitManagement.UpdatePosition(unit.Id, currentPosition + wayTo);

            return finished;
        }

        /// <summary>
        /// Activates the next job for the unit. 
        /// </summary>
        /// <param name="unit">Unit, whose job shall be activated</param>
        /// <param name="expand">Flag, indicating whether the job shall be expanded</param>
        private void ActivateNextJob(Unit unit)
        {
            // We need to clean up!
            var value = unit.IndexCurrentJob;
            if (value < 0)
            {
                value = 0;
            }
            else
            {
                value++;
            }

            // Maximum + 1
            if (value > unit.Jobs.Count)
            {
                value = unit.Jobs.Count;
            }

            this.UnitManagement.SetCurrentJob(unit.Id, value);

            // And expand if necessary
            if (value < 0 || value >= unit.Jobs.Count)
            {
                // Nothing to expand
                return;
            }

            this.ExpandCurrentJob(unit, value);
        }

        /// <summary>
        /// Expands the current job
        /// </summary>
        /// <param name="unit">Unit, whose job shall be expanded</param>
        /// <param name="value">Value of the unit, whose job shall be expanded</param>
        private void ExpandCurrentJob(Unit unit, int value)
        {
            // Check, if we are currently on a user defined job, if no, nothing has to occur
            var removalRequired = true;
            if (unit.IndexCurrentJob >= 0 && unit.IndexCurrentJob < unit.Jobs.Count)
            {
                // If we have user defined job, we may remove all non-necessary items
                removalRequired = unit.Jobs[unit.IndexCurrentJob].IsUserDefined;
            }

            if (removalRequired)
            {
                var currentJobIndex = unit.IndexCurrentJob;

                // Step 1, remove all non-userdefined jobs
                for (var n = 0; n < unit.Jobs.Count; n++)
                {
                    var iterateJob = unit.Jobs[n];
                    if (!iterateJob.IsUserDefined)
                    {
                        this.UnitManagement.RemoveJob(unit.Id, n);
                        n--;

                        if (n < currentJobIndex)
                        {
                            currentJobIndex--;
                        }
                    }
                }

                this.UnitManagement.SetCurrentJob(unit.Id, currentJobIndex);

                // Step 2, do we still have a valid job? 
                unit = this.UnitManagement.GetUnit(unit.Id);
                if (unit.IndexCurrentJob < 0 || unit.IndexCurrentJob >= unit.Jobs.Count)
                {
                    // No
                    return;
                }

                // Step 3, expand current job
                var currentJob = unit.Jobs[unit.IndexCurrentJob];
                if (currentJob.IsUserDefined)
                {
                    // Only user defined jobs are expanded
                    if (currentJob is MoveJob)
                    {
                        this.ExpandMoveJob(unit, currentJob as MoveJob);
                    }
                }
            }
        }

        /// <summary>
        /// Expands the move job in a way that a move job will be generated for each field
        /// </summary>
        /// <param name="unit">Unit who needs new jobs</param>
        /// <param name="moveJob">Movejob to be generated</param>
        private void ExpandMoveJob(Unit unit, MoveJob moveJob)
        {
            var currentJobPosition = unit.IndexCurrentJob;
            var wayPoints = this.WayPointCalculation.CalculateWaypoints(
                    unit.Position,
                    moveJob.TargetPosition,
                    null)
                .ToList();

            // First waypoint and last waypoint are not required, because they give the start and endposition
            foreach (var point in wayPoints.Skip(1).Take(wayPoints.Count - 2))
            {
                var newMoveJob = new MoveJob()
                {
                    IsUserDefined = false,
                    TargetPosition = point,
                    Velocity = moveJob.Velocity
                };

                this.UnitManagement.InsertJob(unit.Id, newMoveJob, currentJobPosition);

                currentJobPosition++;
            }
        }
    }
}