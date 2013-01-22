using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM
{
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class LocalGameDatabase : IFlexBgRuntimeModule
    {
        public GameInfo GameStore
        {
            get;
            set;
        }

        /// <summary>
        /// Called, when started
        /// </summary>
        public void Start()
        {
            this.GameStore = SerializedFile.LoadFromFile<GameInfo>("games.db", () => new GameInfo());
        }

        /// <summary>
        /// Called when shutdowned
        /// </summary>
        public void Shutdown()
        {
            SerializedFile.StoreToFile("games.db", this.GameStore);
        }
    }
}