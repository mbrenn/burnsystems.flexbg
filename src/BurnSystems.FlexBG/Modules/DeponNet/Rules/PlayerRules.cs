﻿using BurnSystems.FlexBG.Modules.DeponNet.PlayerM;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules
{
    /// <summary>
    /// Stores the rules, which are for the player
    /// </summary>
    public class PlayerRules : IPlayerRules
    {
        /// <summary>
        /// Gets or sets the playermanagement
        /// </summary>
        [Inject]
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
            var result = this.PlayerManagement.CreatePlayer(
                param.UserId, 
                param.GameId, 
                param.Playername, 
                param.Empirename);

            return result;
        }

        /// <summary>
        /// Drops the player
        /// </summary>
        /// <param name="playerId">Id of the player to be dropped</param>
        public void DropPlayer(long playerId)
        {
            throw new NotImplementedException();
        }
    }
}
