using BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
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

        [Inject(IsMandatory = true)]
        public IGameClockManager GameClockManager
        {
            get;
            set;
        }

        [Inject(IsMandatory = true, ByName = DeponGamesController.CurrentGameName)]
        public Game CurrentGame
        {
            get;
            set;
        }

        public ResourceSetBag GetResourceSet(int entityType, long entityId)
        {
            var resourceBag = this.Database.Find(entityType, entityId);

            this.UpdateResources(resourceBag);


            return resourceBag;
        }

        public void SetAvailable(int entityType, long entityId, ResourceSet resources)
        {
            var resourceBag = this.Database.Find(entityType, entityId);

            // No update is necessary. Amont is directly set
            resourceBag.Available.Set(resources);
        }

        /// <summary>
        /// Updates the available resources by adding the change to the maximum. 
        /// </summary>
        /// <param name="resourceBag">Resource Bag which shall be updated</param>
        private void UpdateResources(ResourceSetBag resourceBag)
        {
            throw new NotImplementedException();
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