using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM
{
    public class Player
    {
        /// <summary>
        /// Gets or sets the id of the player
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owner id
        /// </summary>
        public long OwnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the playername
        /// </summary>
        public string Playername
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the empirename
        /// </summary>
        public string Empirename
        {
            get;
            set;
        }
    }
}
