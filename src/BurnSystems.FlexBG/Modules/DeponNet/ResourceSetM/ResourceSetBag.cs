using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    /// <summary>
    /// Defines the associated properties of one ResourceSet
    /// One ResourceSet may be associated to a certain entity and entity type
    /// </summary>
    [Serializable]
    public class ResourceSetBag
    {
        /// <summary>
        /// Stores the resource set
        /// </summary>
        private ResourceSet available = new ResourceSet();

        /// <summary>
        /// Stores the change per tick
        /// </summary>
        private ResourceSet change = new ResourceSet();

        /// <summary>
        /// Stores the allowed resources
        /// </summary>
        private ResourceSet maximum = new ResourceSet();

        /// <summary>
        /// Stores the last update
        /// </summary>
        private long lastUpdate = -1;

        public ResourceSet Available
        {
            get
            {
                if (this.available == null)
                {
                    this.available = new ResourceSet();
                }

                return this.available;
            }
        }

        public ResourceSet Change
        {
            get
            {
                if (this.change == null)
                {
                    this.change = new ResourceSet();
                }

                return this.change;
            }
        }

        public ResourceSet Maximum
        {
            get
            {
                if (this.maximum == null)
                {
                    this.maximum = new ResourceSet();
                }

                return this.maximum;
            }
        }

        /// <summary>
        /// Gets or sets the time when the 
        /// </summary>
        public long LastUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id of the entity
        /// </summary>
        public long EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entitytype
        /// </summary>
        public int EntityType
        {
            get;
            set;
        }
    }
}
