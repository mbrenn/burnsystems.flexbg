using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.LockMasterM
{
    /// <summary>
    /// Defines some extension methods for LockMaster
    /// </summary>
    public static class Extensions
    {
        public static IDisposable AcquireReadLock(this ILockMaster lockMaster, params LockEntity[] entities)
        {
            return lockMaster.AcquireReadLock(entities);
        }

        public static IDisposable AcquireWriteLock(this ILockMaster lockMaster, params LockEntity[] entities)
        {
            return lockMaster.AcquireWriteLock(entities);
        }

        public static IDisposable AcquireReadLock(this ILockMaster lockMaster, int entityType, int entityId)
        {
            return lockMaster.AcquireReadLock(new LockEntity(entityType, entityId));
        }

        public static IDisposable AcquireWriteLock(this ILockMaster lockMaster, int entityType, int entityId)
        {
            return lockMaster.AcquireWriteLock(new LockEntity(entityType, entityId));
        }
    }
}
