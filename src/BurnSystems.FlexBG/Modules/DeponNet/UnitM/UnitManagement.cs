using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
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
