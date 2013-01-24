using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface
{
    public interface IPlayerManagement
    {
        /// <summary>
        /// Creates a new player into the database
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="gameId">Id of the game</param>
        /// <param name="playerName">Name of the player</param>
        /// <param name="empireName">Name of the empire</param>
        /// <returns>Id of the new player</returns>
        long CreatePlayer(long userId, long gameId, string playerName, string empireName);

        /// <summary>
        /// Removes the player
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        void RemovePlayer(long playerId);

        IEnumerable<Player> GetAllPlayers();

        IEnumerable<Player> GetPlayerOfUser(long userId);

        Player GetPlayer(long playerId);
    }
}
