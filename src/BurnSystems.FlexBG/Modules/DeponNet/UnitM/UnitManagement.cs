using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    public class UnitManagement : IUnitManagement
    {
        public long CreateUnit(long ownerId, long unitTypeId, System.Windows.Media.Media3D.Vector3D position)
        {
            throw new NotImplementedException();
        }

        public void DissolveUnit(long unitId)
        {
            throw new NotImplementedException();
        }

        public void UpdatePosition(long unitId, System.Windows.Media.Media3D.Vector3D newPosition)
        {
            throw new NotImplementedException();
        }

        public Unit GetUnit(long unitId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Unit> GetUnitsOfPlayer(long ownerId)
        {
            throw new NotImplementedException();
        }
    }
}
