using BurnSystems.FlexBG.Modules.DeponNet.MapM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM.Interface;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    public class ResourceManagement : IResourceManagement
    {
        private  static ILog logger = new ClassLogger(typeof(ResourceManagement));

        /// <summary>
        /// Gets or sets the database
        /// </summary>
        [Inject(IsMandatory = true)]
        public LocalResourceDatabase Database
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IFieldTypeProvider FieldTypeProvider
        {
            get;
            set;
        }

        public ResourceSetBag GetResourceSet(int entityType, long entityId)
        {
            return this.Database.Find(entityType, entityId);
        }

        public void SetResourceSet(int entityType, long entityId, ResourceSetBag resources)
        {
            this.Database.Find(entityType, entityId).Set(resources);
        }

        /// <summary>
        /// Converts a resource set as json 
        /// </summary>
        /// <returns></returns>
        public object AsJson(ResourceSet resourceSet)
        {
            var result = new Dictionary<string, double>();

            foreach (var pair in resourceSet.Resources)
            {
                var fieldType = this.FieldTypeProvider.Get(pair.Key);
                if (fieldType != null)
                {
                    result[fieldType.Token] = pair.Value;
                }
                else
                {
                    logger.Message(
                        string.Format(
                            "Unknown fieldtype with number: {0}",
                            pair.Key));
                }
            }

            return result;
        }
    }
}