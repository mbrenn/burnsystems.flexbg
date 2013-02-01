using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces
{
    public interface IUnitManagement
    {
        /// <summary>
        /// Creates a unit
        /// </summary>
        /// <param name="ownerId">Id of the owner</param>
        /// <param name="unitTypeId">Id of the unit type</param>
        /// <param name="position">Position of the unit</param>
        /// <returns>Id of the new unit</returns>
        long CreateUnit(long ownerId, long unitTypeId, Vector3D position);

        /// <summary>
        /// Dissolves the unit
        /// </summary>
        /// <param name="unitId"></param>
        void DissolveUnit(long unitId);

        void UpdatePosition(long unitId, Vector3D newPosition);

        Unit GetUnit(long unitId);

        IEnumerable<Unit> GetAllUnits();

        IEnumerable<Unit> GetUnitsOfPlayer(long ownerId);
    }
}
