using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface
{
    public interface IBuildingManagement
    {
        /// <summary>
        /// Creates a building on a specific coordinate
        /// </summary>
        /// <param name="x">X-Coordinate of the building</param>
        /// <param name="y">Y-Coordinate of the building</param>
        /// <param name="buildingType">Type of the building</param>
        /// <param name="townId">Id of the town, where building shall be created</param>
        /// <returns>Id of the created building</returns>
        long CreateBuilding(
            BuildingType buildingType,
            long townId,
            double x,
            double y);

        /// <summary>
        /// Gets all buildings of the player
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        /// <returns>Enumeration of buildings</returns>
        IEnumerable<Building> GetAllBuildingsOfPlayer(long playerId);

        /// <summary>
        /// Gets all buildings of the town
        /// </summary>
        /// <param name="townId">Id of the town</param>
        /// <returns>Enumeration of towns</returns>
        IEnumerable<Building> GetAllBuildingsOfTown(long townId);

        /// <summary>
        /// Gets all buildings in a certain region
        /// </summary>
        /// <param name="x1">Left X-Coordinate in the map</param>
        /// <param name="x2">Right X-Coordinate in the map</param>
        /// <param name="y1">Top Y-Coordinate in the map</param>
        /// <param name="y2">Bottom Y-Coordinate in the map</param>
        /// <returns>Enumeration of buildings</returns>
        IEnumerable<Building> GetAllBuildingsInRegion(int x1, int x2, int y1, int y2);
    }
}
