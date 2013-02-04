using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces
{
    /// <summary>
    /// Interface for unit management, all methods are just performed on data level, no logic update 
    /// will be performed
    /// </summary>
    public interface IUnitManagement
    {
        /// <summary>
        /// Creates a unit
        /// </summary>
        /// <param name="ownerId">Id of the owner</param>
        /// <param name="unitTypeId">Id of the unit type</param>
        /// <param name="position">Position of the unit</param>
        /// <returns>Id of the new unit</returns>
        long CreateUnit(long ownerId, int unitTypeId, Vector3D position);

        /// <summary>
        /// Dissolves the unit
        /// </summary>
        /// <param name="unitId"></param>
        void DissolveUnit(long unitId);

        void UpdatePosition(long unitId, Vector3D newPosition);

        Unit GetUnit(long unitId);

        IEnumerable<Unit> GetAllUnits();

        IEnumerable<Unit> GetUnitsOfPlayer(long ownerId);
        
        /// <summary>
        /// Inserts a job for the unit. 
        /// This is just done on data level, no automatic update of current unit task will be performed
        /// </summary>
        /// <param name="unitId">Id of the unit </param>
        /// <param name="job">Job to be added</param>
        /// <param name="position">Position where unit will be found or max value if it shall be inserted at the end</param>
        /// <returns>Id of the ne inserted job</returns>
        int InsertJob(long unitId, IJob job, int position);

        /// <summary>
        /// Removes the job from the unit. 
        /// No automatic update of unit task wil be performed
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="position">Position of job that shall be removed</param>
        void RemoveJob(long unitId, int position);

        /// <summary>
        /// Sets the index of the current job
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="currentJobIndex">Index of the next current job</param>
        void SetCurrentJob(long unitId, int currentJobIndex);
    }
}
