using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.FlexBG.Modules.IdGeneratorM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    public class BuildingManagement : IBuildingManagement
    {
        /// <summary>
        /// Gets or sets the game database
        /// </summary>
        [Inject(IsMandatory = true)]
        public LocalBuildingDatabase BuildingDb
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id generator
        /// </summary>
        [Inject()]
        public IIdGenerator IdGenerator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id generator
        /// </summary>
        [Inject()]
        public ITownManagement TownManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a building on a specific coordinate
        /// </summary>
        /// <param name="x">X-Coordinate of the building</param>
        /// <param name="y">Y-Coordinate of the building</param>
        /// <param name="buildingType">Type of the building</param>
        /// <param name="townId">Id of the town, where building shall be created</param>
        /// <returns>Id of the created building</returns>
        public long CreateBuilding(BuildingType buildingType, long townId, double x, double y)
        {
            var ownerTown = this.TownManagement.GetTown(townId);
            if (ownerTown == null)
            {
                throw new InvalidOperationException("Town with id " + townId + " does not exist");
            }

            var building = new Building();
            building.Id = this.IdGenerator.NextId(EntityType.Building);
            building.IsActive = true;
            building.Level = 1;
            building.PlayerId = ownerTown.OwnerId;
            building.Productivity = 1;
            building.TownId = townId;
            building.BuildingTypeId = buildingType.Id;

            lock (this.BuildingDb.BuildingsStore)
            {
                this.BuildingDb.BuildingsStore.Buildings.Add(building);
            }

            return building.Id;
        }

        /// <summary>
        /// Gets all buildings of the player
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        /// <returns>Enumeration of buildings</returns>
        public IEnumerable<Building> GetAllBuildingsOfPlayer(long playerId)
        {
            lock (this.BuildingDb.BuildingsStore)
            {
                return this.BuildingDb.BuildingsStore.Buildings.Where(x => x.PlayerId == playerId).ToList();
            }
        }

        /// <summary>
        /// Gets all buildings of the town
        /// </summary>
        /// <param name="townId">Id of the town</param>
        /// <returns>Enumeration of towns</returns>
        public IEnumerable<Building> GetAllBuildingsOfTown(long townId)
        {
            lock (this.BuildingDb.BuildingsStore)
            {
                return this.BuildingDb.BuildingsStore.Buildings.Where(x => x.TownId == townId).ToList();
            }
        }

        /// <summary>
        /// Gets all buildings in a certain region
        /// </summary>
        /// <param name="x1">Left X-Coordinate in the map</param>
        /// <param name="x2">Right X-Coordinate in the map</param>
        /// <param name="y1">Top Y-Coordinate in the map</param>
        /// <param name="y2">Bottom Y-Coordinate in the map</param>
        /// <returns>Enumeration of buildings</returns>
        public IEnumerable<Building> GetAllBuildingsInRegion(int x1, int x2, int y1, int y2)
        {
            lock (this.BuildingDb.BuildingsStore)
            {
                return this.BuildingDb.BuildingsStore.Buildings.Where(x =>
                    x.Position.X >= x1 &&
                    x.Position.X <= x2 &&
                    x.Position.Y >= y1 &&
                    x.Position.Y <= y2).ToList();
            }
        }
    }
}
