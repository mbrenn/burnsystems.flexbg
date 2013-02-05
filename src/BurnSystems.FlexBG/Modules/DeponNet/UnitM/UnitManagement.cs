﻿using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Strategies;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using BurnSystems.FlexBG.Modules.IdGeneratorM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    public class UnitManagement : IUnitManagement
    {
        [Inject(IsMandatory = true)]
        public LocalUnitDatabase Data
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IIdGenerator IdGenerator
        {
            get;
            set;
        }

        public long CreateUnit(long ownerId, int unitTypeId, Vector3D position)
        {
            var unit = new Unit();
            unit.PlayerId = ownerId;
            unit.UnitTypeId= unitTypeId;
            unit.Position = position;
            unit.Id = this.IdGenerator.NextId(EntityType.Unit);

            lock (this.Data.SyncObject)
            {
                this.Data.UnitsStore.Units.Add(unit);
            }

            return unit.Id;
        }

        public void DissolveUnit(long unitId)
        {
            throw new NotImplementedException();
        }

        public void UpdatePosition(long unitId, Vector3D newPosition)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.GetUnit(unitId);
                if (unit != null)
                {
                    unit.Position = newPosition;
                }                
            }            
        }

        public Unit GetUnit(long unitId)
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
            }
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.UnitsStore.Units.ToList();
            }
        }

        public IEnumerable<Unit> GetUnitsOfPlayer(long ownerId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a job for the unit. 
        /// This is just done on data level, no automatic update of current unit task will be performed
        /// </summary>
        /// <param name="unitId">Id of the unit </param>
        /// <param name="job">Job to be added</param>
        /// <param name="position">Position where unit will be found or max value if it shall be inserted at the end</param>
        /// <returns>Id of the ne inserted job</returns>
        public int InsertJob(long unitId, IJob job, int position)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
                if (unit == null)
                {
                    // Nothing to do here
                    return -1;
                }

                // Alignment of position
                var count = unit.Jobs.Count;
                if (position < 0)
                {
                    position = 0;
                }

                if (position > count)
                {
                    position = count;
                }

                // Performs the insertion
                unit.Jobs.Insert(position, job);

                return position;
            }
        }

        /// <summary>
        /// Removes the job from the unit. 
        /// No automatic update of unit task wil be performed
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="position">Position of job that shall be removed</param>
        public void RemoveJob(long unitId, int position)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
                if (unit == null)
                {
                    // Nothing to do here
                    return;
                }

                unit.Jobs.RemoveAt(position);
            }
        }

        /// <summary>
        /// Sets the index of the current job
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="currentJobIndex">Index of the next current job</param>
        public void SetCurrentJob(long unitId, int currentJobIndex)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
                if (unit == null)
                {
                    // Nothing to do here
                    return;
                }

                unit.IndexCurrentJob = currentJobIndex;
            }
        }

        /// <summary>
        /// Sets the unit strategy
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="strategy">Strategy to be implemented</param>
        public void SetUnitStrategy(long unitId, UnitStrategy strategy)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
                if (unit == null)
                {
                    // Nothing to do here
                    return;
                }

                unit.Strategy = strategy;
            }
        }
    }
}
