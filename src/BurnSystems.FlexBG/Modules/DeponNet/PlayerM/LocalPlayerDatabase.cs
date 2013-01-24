using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM
{
    [BindAlsoTo(typeof ( IFlexBgRuntimeModule) )]
    public class LocalPlayerDatabase : IFlexBgRuntimeModule
    {
        public PlayersData PlayersStore
        {
            get;
            set;
        }

        /// <summary>
        /// Called, when started
        /// </summary>
        public void Start()
        {
            this.PlayersStore = SerializedFile.LoadFromFile<PlayersData>("players.db", () => new PlayersData());
        }

        /// <summary>
        /// Called when shutdowned
        /// </summary>
        public void Shutdown()
        {
            SerializedFile.StoreToFile("players.db", this.PlayersStore);
        }
    }
}
