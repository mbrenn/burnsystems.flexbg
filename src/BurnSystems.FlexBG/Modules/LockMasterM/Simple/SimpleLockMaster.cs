using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.LockMasterM.Simple
{
    /// <summary>
    /// A very simple lock master that just locks the complete game, but checks whether
    /// we have the right order for entity types. 
    /// </summary>
    public class SimpleLockMaster : ILockMaster
    {
        public IDisposable AcquireReadLock(int entityType, long entityId)
        {
            throw new NotImplementedException();
        }

        public IDisposable AcquireWriteLock(int entityType, long entityId)
        {
            throw new NotImplementedException();
        }

        public void AddRelationShip(int ownerEntityType, int childEntityType)
        {
            throw new NotImplementedException();
        }
    }
}
