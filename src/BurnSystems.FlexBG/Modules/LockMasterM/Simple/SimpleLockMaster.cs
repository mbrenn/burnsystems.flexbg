using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.LockMasterM.Simple
{
    /// <summary>
    /// A very simple lock master that just locks the complete game, but checks whether
    /// we have the right order for entity types. 
    /// </summary>
    public class SimpleLockMaster : ILockMaster
    {
        /// <summary>
        /// Stores the relationships, where the child type is the index and the parent type the value
        /// </summary>
        private List<int> parentRelationShips = new List<int>();

        /// <summary>
        /// Used for synchronization
        /// </summary>
        private object syncObject = new object();

        /// <summary>
        /// Stores the locking status
        /// </summary>
        private ThreadLocal<LockingStatus> status = new ThreadLocal<LockingStatus>(() => new LockingStatus());

        public IDisposable AcquireReadLock(IEnumerable<LockEntity> entities)
        {
            // Check for possible dead locks
            this.CheckForDeadLock(entities);

            // Adds to status
            this.AddToStatus(entities);

            // Perform locking
            Monitor.Enter(this.syncObject);
            return new LockScope(
                this.syncObject,
                () =>
                {
                    this.RemoveFromStatus(entities);
                });
        }

        public IDisposable AcquireWriteLock(IEnumerable<LockEntity> entities)
        {
            // Check for possible dead locks
            this.CheckForDeadLock(entities);

            // Adds to status
            this.AddToStatus(entities);

            // Perform lock
            Monitor.Enter(this.syncObject);
            return new LockScope(
                this.syncObject,
                () =>
                {
                    this.RemoveFromStatus(entities);
                });
        }

        /// <summary>
        /// Adds the lock to the status, so locking can be tracked
        /// </summary>
        /// <param name="entities">Entities to be added</param>
        private void AddToStatus(IEnumerable<LockEntity> entities)
        {
            foreach (var entity in entities)
            {
                // Checks, if entity type is known
                if (entity.EntityType >= this.parentRelationShips.Count)
                {
                    throw new InvalidOperationException("Entity Type is not known: " + entity.EntityType);
                }

                // Performs the addition itself
                this.status.Value.Add(entity);
                var currentEntityType = entity.EntityType;

                while (currentEntityType != 0)
                {
                    this.status.Value.SetEntityType(currentEntityType);

                    currentEntityType = this.parentRelationShips[currentEntityType];
                }
            }
        }

        /// <summary>
        /// Removes the lock to the status, so locking can be tracked
        /// </summary>
        /// <param name="entities">Entities to be removed</param>
        private void RemoveFromStatus(IEnumerable<LockEntity> entities)
        {
            foreach (var entity in entities)
            {
                // Checks, if entity type is known
                if (entity.EntityType >= this.parentRelationShips.Count)
                {
                    throw new InvalidOperationException("Entity Type is not known: " + entity.EntityType);
                }

                // Performs the removal itself
                this.status.Value.Remove(entity);
                var currentEntityType = entity.EntityType;

                while (currentEntityType != 0)
                {
                    this.status.Value.ResetEntityType(currentEntityType);

                    currentEntityType = this.parentRelationShips[currentEntityType];
                }
            }
        }

        /// <summary>
        /// Checks if dead lock may occur. Some rules apply
        /// </summary>
        /// <param name="entities">Entities to be added</param>
        private void CheckForDeadLock(IEnumerable<LockEntity> entities)
        {
            // Entity itself shall not be locked! 
            foreach (var entity in entities)
            {
                this.CheckForDeadLock(entity);
            }
        }

        /// <summary>
        /// Checks for possible deadlock, if this item has been locked
        /// </summary>
        /// <param name="entity">Entity to be checked</param>
        private void CheckForDeadLock(LockEntity entity)
        {
            var currentEntityId = entity.EntityType;

            if (this.status.Value.IsEntityTypeSet(currentEntityId))
            {
                throw new InvalidOperationException("Entity type is alread locked: " + currentEntityId);
            }
        }

        /// <summary>
        /// Adds a relationship between two entity types
        /// </summary>
        /// <param name="parentEntityType">Type id of the owner</param>
        /// <param name="childEntityType">Type if of the parent</param>
        public void AddRelationShip(int parentEntityType, int childEntityType)
        {
            Ensure.That(parentEntityType > 0, "parentEntityType is <= 0");
            Ensure.That(childEntityType > 0, "parentEntityType is <= 0");

            // Resize array
            var max = Math.Max(parentEntityType, childEntityType);
            while (this.parentRelationShips.Count <= max)
            {
                this.parentRelationShips.Add(0);
            }

            // Assign values
            this.parentRelationShips[childEntityType] = parentEntityType;
        }
    }
}
