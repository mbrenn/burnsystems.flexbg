using BurnSystems.FlexBG.Modules.DeponNet.PlayerM;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.DataProvider
{
    /// <summary>
    /// Gets the information for a complete player, including resources
    /// </summary>
    public class PlayerInfoController : Controller
    {
        [Inject(IsMandatory = true)]
        public IResourceManagement ResourceManagement
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

        [Inject(ByName = DeponPlayersController.CurrentPlayerName, IsMandatory = true)]
        public Player CurrentPlayer
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult GetCurrentPlayer()
        {
            var playerData = this.CurrentPlayer.AsJson();
            var resourcesOfPlayer = this.ResourceManagement.GetResources(EntityType.Player, this.CurrentPlayer.Id);
            var convertedResources = this.ResourceManagement.AsJson(resourcesOfPlayer);

            return this.Json(
                new
                {
                    playerResources = convertedResources,
                    player = playerData,
                    success = true
                });
        }
    }
}
