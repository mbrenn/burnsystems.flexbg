using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class LocalResourceDatabase : IFlexBgRuntimeModule
    {
        public ResourcesData ResourceStore
        {
            get;
            set;
        }

        /// <summary>
        /// Called, when started
        /// </summary>
        public void Start()
        {
            this.ResourceStore = SerializedFile.LoadFromFile<ResourcesData>("resources.db", () => new ResourcesData());
        }

        /// <summary>
        /// Called when shutdowned
        /// </summary>
        public void Shutdown()
        {
            SerializedFile.StoreToFile("resources.db", this.ResourceStore);
        }
    }
}