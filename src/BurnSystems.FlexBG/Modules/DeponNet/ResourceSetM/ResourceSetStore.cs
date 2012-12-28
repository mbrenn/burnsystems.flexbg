using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    public class ResourceSetStore
    {
        /// <summary>
        /// Stores the resource set
        /// </summary>
        private ResourceSet resources = new ResourceSet();

        public ResourceSet Resources
        {
            get { return this.resources; }
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
