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
        private ResourceSet resources = new ResourceSet();

        public ResourceSet Resources
        {
            get { return this.resources; }
        }

        /// <summary>
        /// Sets the resource set bag
        /// </summary>
        /// <param name="other">Other resource set bag</param>
        public void Set(ResourceSetBag other)
        {
            this.Resources.Set(other.Resources);
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
