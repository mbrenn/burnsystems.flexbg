using BurnSystems.FlexBG.Modules.UserM.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BurnSystems.FlexBG.Modules.UserM.Data
{
    /// <summary>
    /// Stores the data in memory and writes it to file system if necessary
    /// </summary>
    [Serializable]
    public class UserDatabaseLocal
    {
        private long lastUserId = 0;
        /// <summary>
        /// Stores a list of users
        /// </summary>
        private List<User> users = new List<User>();

        /// <summary>
        /// Gets a list of users
        /// </summary>
        public List<User> Users
        {
            get { return this.users; }
        }

        public long LastUserId
        {
            get { return this.lastUserId; }
            set { this.lastUserId = value; }
        }

        /// <summary>
        /// Gets the next user id
        /// </summary>
        /// <returns></returns>
        public long GetNextUserId()
        {
            return Interlocked.Increment(ref lastUserId);
        }

        /// <summary>
        /// Nothing to do here
        /// </summary>
        public void SaveChanges()
        {
        }
    }
}

