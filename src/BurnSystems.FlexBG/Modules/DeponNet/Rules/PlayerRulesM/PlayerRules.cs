using BurnSystems.FlexBG.Modules.DeponNet.PlayerM;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.LockMasterM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM
{
    /// <summary>
    /// Stores the rules, which are for the player
    /// </summary>
    public class PlayerRules : IPlayerRules
    {
        [Inject(IsMandatory=true)]
        public ILockMaster LockMaster
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the playermanagement
        /// </summary>
        [Inject(IsMandatory=true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new player
        /// </summary>
        /// <param name="param">Parameters for the new player</param>
        /// <returns>Id of the new player</returns>
        public long CreatePlayer(PlayerCreationParams param)
        {
            using (this.LockMaster.AcquireWriteLock(EntityType.Game, param.GameId))
            {
                var result = this.PlayerManagement.CreatePlayer(
                    param.UserId,
                    param.GameId,
                    param.Playername,
                    param.Empirename);

                return result;
            }
        }

        /// <summary>
        /// Drops the player
        /// </summary>
        /// <param name="playerId">Id of the player to be dropped</param>
        public void DropPlayer(long playerId)
        {
            var player = this.PlayerManagement.GetPlayer(playerId);
            using (this.LockMaster.AcquireWriteLock(EntityType.Game, player.GameId))
            {
                this.PlayerManagement.RemovePlayer(playerId);
            }
        }

        /// <summary>
        /// Gets a value whether the user may join the game
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="gameId">Id of the game</param>
        /// <returns>true, if user can join the game</returns>
        public bool CanUserContinueGame(long userId, long gameId)
        {
            return this.PlayerManagement.GetPlayersOfUser(userId).Any(x => x.GameId == gameId);
        }
    }
}
