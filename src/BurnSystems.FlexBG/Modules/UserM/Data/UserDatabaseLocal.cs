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
        /// <summary>
        /// Stores the id of the last group
        /// </summary>
        private long lastGroupId = 0;

        /// <summary>
        /// Stores the id of the last user
        /// </summary>
        private long lastUserId = 0;

        /// <summary>
        /// Stores a list of users
        /// </summary>
        private List<User> users = new List<User>();

        /// <summary>
        /// Stores the groups
        /// </summary>
        private List<Group> groups = new List<Group>();

        /// <summary>
        /// Stores the memberships between user and group
        /// </summary>
        private List<Membership> memberships = new List<Membership>();

        /// <summary>
        /// Gets a list of users
        /// </summary>
        public List<User> Users
        {
            get { return this.users; }
        }

        /// <summary>
        /// Gets a list of users
        /// </summary>
        public List<Group> Groups
        {
            get { return this.groups; }
        }

        /// <summary>
        /// Gets a list of users
        /// </summary>
        public List<Membership> Memberships
        {
            get { return this.memberships; }
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
        /// Gets the next user id
        /// </summary>
        /// <returns></returns>
        public long GetNextGroupId()
        {
            return Interlocked.Increment(ref lastGroupId);
        }

        /// <summary>
        /// Nothing to do here
        /// </summary>
        public void SaveChanges()
        {
        }
    }
}

