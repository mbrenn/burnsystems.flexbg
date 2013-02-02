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
    /// Defines the lock scope
    /// </summary>
    class LockScope : IDisposable
    {
        /// <summary>
        /// Stores the sync object
        /// </summary>
        private object syncObject;

        /// <summary>
        /// Action to be executed on disposal
        /// </summary>
        private Action action;

        /// <summary>
        /// Initializes a new instnace of the LockScope class
        /// </summary>
        /// <param name="syncObject">Synchronizationobject to be used</param>
        public LockScope(object syncObject, Action action)
        {
            Ensure.That(syncObject != null);
            Ensure.That(action != null);

            this.syncObject = syncObject;
            this.action = action;
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            this.action();
            Monitor.Exit(this.syncObject);
        }
    }
}
