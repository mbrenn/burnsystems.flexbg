using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.TownM;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapM.Controllers
{
    /// <summary>
    /// Returns all game objects in a certain region
    /// </summary>
    public class GameObjectsController : Controller
    {
        [Inject(ByName = DeponGamesController.CurrentGameName)]
        public Game CurrentGame
        {
            get;
            set;
        }

        [Inject(ByName = DeponPlayersController.CurrentPlayerName)]
        public Player CurrentPlayer
        {
            get;
            set;
        }

        [Inject]
        public IBuildingManagement BuildingManagement
        {
            get;
            set;
        }

        [Inject]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject]
        public ITownManagement TownManagement
        {
            get;
            set;
        }

        [Inject]
        public IBuildingTypeProvider BuildingTypeProvider
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the gmaeobjects on the map
        /// </summary>
        /// <param name="x1">Left-X-Coordinate of the map</param>
        /// <param name="x2">Right-X-Coordinate of the map</param>
        /// <param name="y1">Top-Y-Coordinate of the map</param>
        /// <param name="y2">Bottom-Y-Coordinate of the map</param>
        /// <returns>Enumeration of gameobjects</returns>
        [WebMethod]
        public IActionResult GetGameObjects(int x1, int x2, int y1, int y2)
        {   
            var gameObjects = new List<object>();

            var buildings = this.BuildingManagement.GetAllBuildingsInRegion(x1, x2, y1, y2);
            foreach (var building in buildings)
            {
                var player = this.PlayerManagement.GetPlayer(building.PlayerId) ?? new Player();
                var town = this.TownManagement.GetTown(building.TownId) ?? new Town();

                gameObjects.Add(
                    new
                    {
                        Id = building.Id,
                        BuildingTypeId = building.BuildingTypeId,
                        BuildingType = this.BuildingTypeProvider.Get(building.BuildingTypeId),
                        Level = building.Level,
                        TownId = building.TownId,
                        PlayerId = building.PlayerId,
                        Playername = player.Playername,
                        Townname = town.TownName,
                        GameObjectType = "building",
                        X = building.Position.X,
                        Y = building.Position.Y,
                        Z = building.Position.Z
                    });
            }
            

            var result = new 
            {
                success = true,
                gameObjects = gameObjects
            };

            return this.Json(result);
        }
    }
}
