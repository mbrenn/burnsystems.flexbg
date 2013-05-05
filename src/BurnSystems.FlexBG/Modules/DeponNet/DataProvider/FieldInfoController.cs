using BurnSystems.FlexBG.Modules.DeponNet.BuildingM;
using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.MapM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Controllers;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
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
    /// Gives the information about fields
    /// </summary>
    public class FieldInfoController : Controller
    {
        /// <summary>
        /// Gets or sets the current game
        /// </summary>
        [Inject(ByName = DeponGamesController.CurrentGameName)]
        public Game CurrentGame
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current player
        /// </summary>
        [Inject(ByName = DeponPlayersController.CurrentPlayerName)]
        public Player CurrentPlayer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the voxelmap
        /// </summary>
        [Inject(IsMandatory=true)]
        public IVoxelMap VoxelMap
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IFieldTypeProvider FieldTypeProvider
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IBuildingManagement BuildingManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IBuildingDataProvider BuildingDataProvider
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IBuildingTypeProvider BuildingTypeProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets detailled information about a certain field
        /// </summary>
        /// <param name="x">Requested X-Coordinate</param>
        /// <param name="y">Requested Y-Coordinate</param>
        /// <returns>Result of the field</returns>
        [WebMethod]
        public IActionResult GetDetailFieldInfo(int x, int y)
        {
            // Gets the field information
            var field = this.VoxelMap.GetColumn(this.CurrentGame.Id, x, y);
            var data = new List<InfoContent>();
            data.Add(
                new InfoContent()
                {
                    content = field.FirstOrDefault().FieldType.ToString(),
                    token = "fieldinfo",
                    data = new
                    {
                        x = x,
                        y = y,
                        fieldTypeTitle = this.FieldTypeProvider.Get(field.Last().FieldType).Token
                    }
                });

            // Gets the building information
            var buildings = this.BuildingDataProvider.GetBuildingsOnField(x, y);
            foreach (var building in buildings)
            {
                data.Add(
                    new InfoContent()
                    {
                        token = "buildinginfo",
                        data = new
                        {
                            id = building.Id, 
                            productivity = building.Productivity, 
                            buildingTypeId = building.BuildingTypeId,
                            isActive = building.IsActive,
                            buildingType = this.BuildingTypeProvider.Get ( building.BuildingTypeId).Token,
                            x = building.Position.X,
                            y = building.Position.Y,
                            townId = building.TownId,
                            playerId = building.PlayerId
                        }
                    });
            }

            var result = new
            {
                data = data,
                success = true
            };

            return this.Json(result);
        }

        [Serializable]
        private class InfoContent
        {
            public string token
            {
                get;
                set;
            }

            public string content
            {
                get;
                set;
            }

            public object data
            {
                get;
                set;
            }
        }
    }
}
