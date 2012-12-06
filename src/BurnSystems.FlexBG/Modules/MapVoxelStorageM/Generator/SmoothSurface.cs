using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator
{
    /// <summary>
    /// Smoothes the surface being specified by a certain field type
    /// </summary>
    public class SmoothSurface
    {
        /// <summary>
        /// Gets the voxel map
        /// </summary>
        public IVoxelMap VoxelMap
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the fieldtype being smoothed
        /// </summary>
        public byte FieldType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the distance used to smooth the surface
        /// </summary>
        public int SmoothRadius
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the smooth surface class
        /// </summary>
        /// <param name="voxelMap">Voxelmap, where filter shall be applied</param>
        /// <param name="fieldType">Type of the field</param>
        /// <param name="smoothRadius">Distance where filter shall be applied</param>
        public SmoothSurface(IVoxelMap voxelMap, byte fieldType, int smoothRadius)
        {
            this.FieldType = fieldType;
            this.SmoothRadius = smoothRadius;
            this.VoxelMap = voxelMap;
        }

        /// <summary>
        /// Executes the smoothing
        /// </summary>
        public void Execute()
        {
            var dx = this.VoxelMap.GetInfo().SizeX;
            var dy = this.VoxelMap.GetInfo().SizeY;

            for (var x = 0; x < dx; x++)
            {
                for (var y = 0; y < dy; y++)
                {
                    this.Smooth(x, y);
                }
            }
        }

        /// <summary>
        /// Smoothes a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column to be smoothed</param>
        /// <param name="y">Y-Coordinate of the column to be smoothed</param>
        private void Smooth(int x, int y)
        {
            double total = 0;
            var totalCount = 0;

            // Calculates the average of all columns within the smoothradius
            for (var dx = -this.SmoothRadius; dx <= this.SmoothRadius; dx++)
            {
                for (var dy = -this.SmoothRadius; dy <= this.SmoothRadius; dy++)
                {
                    var absX = x + dx;
                    var absY = y + dy;
                    if (absX < 0 || absX >= this.VoxelMap.GetInfo().SizeX
                        || absY < 0 || absY >= this.VoxelMap.GetInfo().SizeY)
                    {
                        // Out Of Map
                        continue;
                    }

                    // Get height of type
                    var column = this.VoxelMap.GetColumn(absX, absY);
                    var heights = column.GetHeightsOfFieldType(this.FieldType).ToList();

                    if (heights.Count == 0 || heights[0] == float.MinValue)
                    {
                        // No height, no cry
                        continue;
                    }

                    total += heights.First();
                    totalCount++;
                }
            }

            // Ok, hope, we got something
            if (totalCount == 0)
            {
                // No information caught, should never occur
                return;
            }

            var newHeight = (float)(total / totalCount);

            // Get currentheight of the changed block within our column
            var relevantColumn = this.VoxelMap.GetColumn(x, y);
            var hit = false;
            var startingHeight = float.MinValue;
            var endingHeight = float.MinValue;
            var lastFieldType = (byte)0;
            for (var n = 0; n < relevantColumn.Count; n++)
            {
                var change = relevantColumn[n];
                if (hit)
                {
                    // 3. We need to know how big we are, so ask for end of change height. 
                    endingHeight = change.ChangeHeight;
                    break;
                }

                if (change.FieldType == this.FieldType)
                {
                    // 2. Argh, we are hit. 
                    // 2. Found our requested field type
                    hit = true;
                    startingHeight = change.ChangeHeight;
                }
                else
                {
                    // 1. Store current field type, if we have not been hit or if this is not the requested field type
                    lastFieldType = change.FieldType;
                }
            }

            if (startingHeight == float.MinValue)
            {
                // No change, if we did not have such a field type in this column
                return;
            }

            if (endingHeight > newHeight)
            {
                // This block will disappear, because new height moved below the border of this fieldtype
                relevantColumn.SetFieldType(lastFieldType, startingHeight, endingHeight);
            }
            else if (startingHeight > newHeight)
            {
                // We will get reduced, so set last field type
                relevantColumn.SetFieldType(lastFieldType, startingHeight, newHeight);
            }
            else if (startingHeight < newHeight)
            {
                // We will are growing. Set new fieldtype
                relevantColumn.SetFieldType(this.FieldType, startingHeight, newHeight);
            }

            // Hope everything went well
            this.VoxelMap.SetColumn(x, y, relevantColumn);
        }
    }
}
