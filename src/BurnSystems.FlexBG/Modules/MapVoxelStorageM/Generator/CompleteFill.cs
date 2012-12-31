using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator
{
    /// <summary>
    /// Performs a complete fill of the map
    /// </summary>
    public class CompleteFill
    {
        /// <summary>
        /// Gets or sets the fieldtype for the complete fill
        /// </summary>
        public byte FieldType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the voxelmap
        /// </summary>
        public IVoxelMap VoxelMap
        {
            get;
            private set;
        }

        public CompleteFill(IVoxelMap voxelMap, byte fieldType)
        {
            this.FieldType = fieldType;
            this.VoxelMap = voxelMap;
        }

        /// <summary>
        /// Executes the fill
        /// </summary>
        public void Execute(int instanceId)
        {
            var dx = this.VoxelMap.GetInfo(instanceId).SizeX;
            var dy = this.VoxelMap.GetInfo(instanceId).SizeY;

            for (var x = 0; x < dx; x++)
            {
                for (var y = 0; y < dy; y++)
                {
                    var column = this.VoxelMap.GetColumn(instanceId, x, y);
                    column.SetFieldType(this.FieldType, float.MaxValue, float.MinValue);
                    this.VoxelMap.SetColumn(instanceId, x, y, column);
                }
            }
        }

        public static void Execute(IVoxelMap voxelMap, int instanceId, byte fieldType)
        {
            var fill = new CompleteFill(voxelMap, fieldType);
            fill.Execute(instanceId);
        }
    }
}
