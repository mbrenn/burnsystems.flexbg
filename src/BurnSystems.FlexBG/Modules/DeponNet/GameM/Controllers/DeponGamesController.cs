using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
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
    }
}
