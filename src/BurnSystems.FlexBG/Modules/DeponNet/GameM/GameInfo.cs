using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM
{
    /// <summary>
    /// Entites that are persisted
    /// </summary>
    public class GameInfo
    {
        /// <summary>
        /// Stores the list of games
        /// </summary>
        private List<Game> games = new List<Game>();

        public List<Game> Games
        {
            get { return this.games; }
        }
    }
}
