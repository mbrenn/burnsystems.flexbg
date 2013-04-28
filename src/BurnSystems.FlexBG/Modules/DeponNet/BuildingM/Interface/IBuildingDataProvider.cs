﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface
{
    /// <summary>
    /// Game-specific game provider
    /// </summary>
    public interface IBuildingDataProvider
    {        
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
