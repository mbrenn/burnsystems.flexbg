using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM
{
    /// <summary>
    /// Defines the interface for the player rules
    /// </summary>
    public interface IPlayerRules
    {
        /// <summary>
        /// Creates a new player
        /// </summary>
        /// <param name="param">Parameters for the new player</param>
        /// <returns>Id of the new player</returns>
        long CreatePlayer(PlayerCreationParams param);

        /// <summary>
        /// Drops the player
        /// </summary>
        /// <param name="playerId">Id of the player to be dropped</param>
        void DropPlayer(long playerId);
    }
}
