using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Models
{
    /// <summary>
    /// Stores the memberships
    /// </summary>
    [Serializable]
    public class Membership
    {
        private string userId;
        private string groupId;

        /// <summary>
        /// Id of the user
        /// </summary>
        public string UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        /// <summary>
        /// Id of the group
        /// </summary>
        public string GroupId
        {
            get { return this.groupId; }
            set { this.groupId = value; }
        }
    }
}
