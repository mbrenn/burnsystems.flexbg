using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameClockM
{
    /// <summary>
    /// Defines the game clock for one game
    /// </summary>
    public class GameClock
    {
        /// <summary>
        /// Gets or sets the id of the game clock
        /// </summary>
        public long GameId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time since start
        /// </summary>
        public long Time
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date time, when the clock had been created
        /// </summary>
        public DateTime Created
        {
            get;
            set;
        }
    }
}
