using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    public class Building
    {
        /// <summary>
        /// Gets or sets the id of the building
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id of the town, where building is associated
        /// </summary>
        public long TownId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id of the player, who is owner of the building
        /// </summary>
        public long PlayerId
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the building type
        /// </summary>
        public long BuildingTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets level of building
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets information whether the building is currently active
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the productivity of the building. 
        /// Can be a number between 0.0 and 1.0
        /// </summary>
        public double Productivity
        {
            get;
            set;
        }
    }
}
