﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Configuration;
using System.Globalization;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Controllers
{
    public class VoxelMapController : Controller
    {
        [Inject]
        public IVoxelMap VoxelMap
        {
            get;
            set;
        }

        [Inject]
        public IVoxelMapConfiguration VoxelMapConfiguration
        {
            get;
            set;
        }

        public IActionResult MapInfo(int i)
        {
            var info = this.VoxelMap.GetInfo(i);
            return this.Json(info);
        }

        /// <summary>
        /// Gets the surface for the map
        /// </summary>
        /// <param name="i">Instance of the map</param>
        /// <param name="x1">Left-X-Coordinate of the map</param>
        /// <param name="x2">Right-X-Coordinate of the map</param>
        /// <param name="y1">Top-Y-Coordinate of the map</param>
        /// <param name="y2">Bottom-Y-Coordinate of the map</param>
        /// <returns></returns>
        public IActionResult Surface(int i, int x1, int x2, int y1, int y2)
        {
            if (x2 < x1 || y2 < y1)
            {
                throw new ArgumentException("x2 <= x1 || y2 <= y1");
            }

            var fields = (x2 - x1) * (y2 - y1);
            if (fields > 10000)
            {
                throw new ArgumentException("(x2 - x1) * (y2 - y1) > 10000");
            }

            var surface = this.VoxelMap.GetSurfaceInfo(i, x1, y1, x2, y2);

            var result = new List<object>();

            var tx = 0;
            for (var x = x1; x <= x2; x++)
            {
                var ty = 0;
                for (var y = y1; y <= y2; y++)
                {
                    result.Add(
                        new
                        {
                            x = x, 
                            y = y,
                            h = surface[tx][ty].ChangeHeight,
                            t = surface[tx][ty].FieldType
                        });

                    ty++;
                }


                tx++;
            }

            return this.Json(result);
        }

        /// <summary>
        /// Returns an actionresult containing the textures
        /// </summary>
        public IActionResult Textures()
        {
            var result = new Dictionary<string, string>();

            foreach (var texture in this.VoxelMapConfiguration.MapInfo.Textures)
            {
                result[texture.FieldType.ToString(CultureInfo.InvariantCulture)] = texture.TexturePath;
            }

            return this.Json(result);
        }
    }
}
