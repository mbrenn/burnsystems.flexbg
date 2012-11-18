using BurnSystems.FlexBG.Modules.UserM.Models;
using System;
using System.Collections.Generic;

namespace BurnSystems.FlexBG.Modules.UserM.Data
{
    /// <summary>
    /// Stores the data in memory and writes it to file system if necessary
    /// </summary>
    [Serializable]
    public class UserDatabaseLocal
    {
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

        /// <summary>
        /// Nothing to do here
        /// </summary>
        public void SaveChanges()
        {
        }
    }
}

