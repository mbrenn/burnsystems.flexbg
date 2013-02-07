﻿using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.MVC;
using BurnSystems.WebServer.Modules.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers
{
    /// <summary>
    /// Controller for games
    /// </summary>
    public class DeponGamesController : Controller
    {
        [Inject(IsMandatory=true)]
        public IGameManagement GameManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IUserManagement UserManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true, ByName="CurrentUser")]
        public User CurrentUser
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IPlayerRules PlayerRules
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public Session Session
        {
            get;
            set;
        }

        /// <summary>
        /// Session variable for current game
        /// </summary>
        public const string CurrentGameName = "FlexBG.CurrentGame";

        /// <summary>
        /// Lists all games
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public IActionResult GetGames()
        {
            var games = this.GameManagement.GetAll();
            var players = this.PlayerManagement.GetPlayersOfUser(this.CurrentUser.Id);

            var result = new
            {
                success = true,
                games = games.Select(x => new
                {
                    id = x.Id,
                    isPaused = x.IsPaused,
                    title = x.Title,
                    playerCount = this.PlayerManagement.GetPlayersOfGame(x.Id).Count(),
                    isInGame = players.Any(y => y.GameId == x.Id)
                })
            };

            return this.Json(result);
        }

        [WebMethod]
        public IActionResult JoinGame([PostModel] JoinGameModel model)
        {
            var playerId = this.PlayerRules.CreatePlayer(
                new PlayerCreationParams()
                {
                    Playername = model.Playername,
                    Empirename = model.Empirename,
                    GameId = model.GameId,
                    UserId = this.CurrentUser.Id
                });
                    
            var result = new
            {
                success = true, 
                playerId = playerId
            };

            return this.Json(result);
        }

        [WebMethod]
        public IActionResult ContinueGame([PostModel] ContinueGameModel model)
        {
            if (!this.PlayerRules.CanUserContinueGame(this.CurrentUser.Id, model.GameId))
            {
                throw new MVCProcessException("continuegame_playernotingame", "Player cannot join game.");
            }
            else
            {
                // Ok, we are in game, now add a cookie for game (Cookies are for games, mjam)
                this.Session["FlexBG.CurrentGame"] = model.GameId;

                var result = new
                {
                    success = true
                };

                return this.Json(result);
            }
        }

        /// <summary>
        /// Static helper method which retrieves the current game out of the session variables
        /// which have been associated to the user
        /// </summary>
        /// <param name="activates">Activation container</param>
        /// <returns>Found game or null, if user has not continued a game</returns>
        public static Game GetGameOfWebRequest(IActivates activates)
        {
            var session = activates.Get<Session>();
            var gameManagement = activates.Get<IGameManagement>();
            Ensure.That(session != null, "Binding to Session has not been done");
            Ensure.That(session != null, "Binding to IGameManagement has not been done");

            var gameIdObj = session["FlexBG.CurrentGame"];
            if (gameIdObj == null || !(gameIdObj is long))
            {
                // Nothing found
                return null;
            }

            return gameManagement.Get((long)gameIdObj);
        }
    }
}
