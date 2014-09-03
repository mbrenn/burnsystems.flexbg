using BurnSystems.FlexBG.Modules.UserM.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BurnSystems.FlexBG.Modules.UserM.Local.Data
{
    /// <summary>
    /// Stores the data in memory and writes it to file system if necessary
    /// </summary>
    [Serializable]
    public class UserDatabaseLocal : UserDatabaseInfo
    {
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
            get
            {
                if (this.users == null)
                {
                    this.users = new List<User>();
                }

                return this.users;
            }
        }

        /// <summary>
        /// Gets a list of users
        /// </summary>
        public List<Group> Groups
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new List<Group>();
                }

                return this.groups;
            }
        }

        /// <summary>
        /// Gets a list of users
        /// </summary>
        public List<Membership> Memberships
        {
            get
            {
                if (this.memberships == null)
                {
                    this.memberships = new List<Membership>();
                }

                return this.memberships;
            }
        }

        /// <summary>
        /// Nothing to do here
        /// </summary>
        public void SaveChanges()
        {
        }

        /// <summary>
        /// Gets the next user id
        /// </summary>
        /// <returns></returns>
        public string GetNextUserId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets the next user id
        /// </summary>
        /// <returns></returns>
        public string GetNextGroupId()
        {
            return Guid.NewGuid().ToString();
        }

    }
}

