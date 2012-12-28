using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.TownM
{
    /// <summary>
    /// Defines the town
    /// </summary>
    public class Town
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id of the owner
        /// </summary>
        public long OwnerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the town name
        /// </summary>
        public string TownName
        {
            get;
            set;
        }
    }
}
