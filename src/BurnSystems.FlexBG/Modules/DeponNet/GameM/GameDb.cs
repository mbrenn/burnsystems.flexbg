using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM
{
    public class GameDb : IFlexBgRuntimeModule
    {
        public GameInfo Games
        {
            get;
            set;
        }

        /// <summary>
        /// Called, when started
        /// </summary>
        public void Start()
        {
            this.Games = SerializedFile.LoadFromFile<GameInfo>("games.db");

            if (this.Games == null)
            {
                this.Games = new GameInfo();
            }
        }

        /// <summary>
        /// Called when shutdowned
        /// </summary>
        public void Shutdown()
        {
            SerializedFile.StoreToFile("ids.db", this.Games);
        }

    }
}
