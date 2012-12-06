using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator
{
    /// <summary>
    /// Replaces a fieldtype
    /// </summary>
    public class ReplaceFieldType
    {
        /// <summary>
        /// Gets or sets the fieldtype which shall be replaced
        /// </summary>
        public byte OldFieldType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the fieldtype being replaced
        /// </summary>
        public byte NewFieldType
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

        public ReplaceFieldType(IVoxelMap voxelMap, byte oldFieldType, byte newFieldType)
        {
            this.OldFieldType = oldFieldType;
            this.NewFieldType = newFieldType;
            this.VoxelMap = voxelMap;
        }

        /// <summary>
        /// Performs the replacement
        /// </summary>
        public void Execute()
        {
            var dx = this.VoxelMap.GetInfo().SizeX;
            var dy = this.VoxelMap.GetInfo().SizeY;

            for (var x = 0; x < dx; x++)
            {
                for (var y = 0; y < dy; y++)
                {
                    var column = this.VoxelMap.GetColumn(x, y);

                    var modified = false;
                    for (var n = 0; n < column.Count; n++)
                    {
                        if (column[n].FieldType == this.OldFieldType)
                        {
                            var change = column[n];
                            change.FieldType = this.NewFieldType;
                            column[n] = change;
                            modified = true;
                        }
                    }

                    if (modified)
                    {
                        this.VoxelMap.SetColumn(x, y, column);
                    }
                }
            }
        }

        public static void Execute(IVoxelMap voxelMap, byte oldFieldType, byte newFieldType)
        {
            var replace = new ReplaceFieldType(voxelMap, oldFieldType, newFieldType);
            replace.Execute();
        }
    }
}
