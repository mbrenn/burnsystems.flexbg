using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class LocalUnitDatabase : IFlexBgRuntimeModule
    {
        public UnitsData UnitsStore
        {
            get;
            set;
        }

        /// <summary>
        /// Called, when started
        /// </summary>
        public void Start()
        {
            this.UnitsStore = SerializedFile.LoadFromFile<UnitsData>("units.db", () => new UnitsData());
        }

        /// <summary>
        /// Called when shutdowned
        /// </summary>
        public void Shutdown()
        {
            SerializedFile.StoreToFile("units.db", this.UnitsStore);
        }
    }
}
