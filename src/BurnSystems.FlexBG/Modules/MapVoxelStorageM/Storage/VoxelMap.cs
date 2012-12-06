using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System.Collections.Generic;
using System.Threading;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Defines the voxelmap class, which gives the access to voxels for the game
    /// </summary>
    public class VoxelMap : IVoxelMap
    {
        private static ILog logger = new ClassLogger(typeof(VoxelMap));

        /// <summary>
        /// Defines the loader for the partitions
        /// </summary>
        [Inject]
        public IPartitionLoader Loader
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the information about the voxelmap
        /// </summary>
        [Inject]
        public VoxelMapInfo Info
        {
            get;
            private set;
        }

        public VoxelMapInfo GetInfo()
        {
            return this.Info;
        }

        /// <summary>
        /// Gets the info, if map has been created
        /// </summary>
        public bool IsMapCreated()
        {
            return this.Loader.LoadInfoData() != null;
        }

        /// <summary>
        /// Initializes a new instance of the voxelmap
        /// </summary>
        /// <param name="info">Information about voxelmap</param>
        [Inject]
        public VoxelMap(VoxelMapInfo info)
        {
            Ensure.That(info != null, "info is null");
            this.Info = info;
        }

        /// <summary>
        /// Syncronisation for locking
        /// </summary>
        private ReaderWriterLockSlim sync = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Calculates the number of partition and the relative coordinates for a certain field
        /// </summary>
        /// <param name="columnX">X-Coordinate of queried column</param>
        /// <param name="columnY">Y-Coordinate of queried column</param>
        /// <param name="partitionX">X Coordinate of the partition itself</param>
        /// <param name="partitionY">Y Coordinate of the partition itself</param>
        /// <param name="relX">Relative X coordinate of the column on the partition</param>
        /// <param name="relY">Relative Y coordinate of the column on the partition</param>
        public void CalculatePartitionCoordinates(int columnX, int columnY, out int partitionX, out int partitionY, out int relX, out int relY)
        {
            partitionX = columnX / this.Info.PartitionLength;
            partitionY = columnY / this.Info.PartitionLength;
            relX = columnX % this.Info.PartitionLength;
            relY = columnY % this.Info.PartitionLength;
        }


        /// <summary>
        /// Calculates the absolute field position from relative coordinates on a partition
        /// </summary>
        /// <param name="partitionX">X Coordinate of the partition itself</param>
        /// <param name="partitionY">Y Coordinate of the partition itself</param>
        /// <param name="relX">Relative X coordinate of the column on the partition</param>
        /// <param name="relY">Relative Y coordinate of the column on the partition</param>
        /// <param name="columnX">X-Coordinate of queried column</param>
        /// <param name="columnY">Y-Coordinate of queried column</param>
        public void CalculateFieldCoordinates(int partitionX, int partitionY, int relX, int relY, out int columnX, out int columnY)
        {
            columnX = this.Info.PartitionLength * partitionX + relX;
            columnY = this.Info.PartitionLength * partitionY + relY;
        }

        /// <summary>
        /// Creates a simple mal just containing air
        /// </summary>
        public void CreateMap()
        {
            // Creates the map by creating partition and storing them
            var partitionCountX = this.Info.SizeX / this.Info.PartitionLength;
            var partitionCountY = this.Info.SizeY / this.Info.PartitionLength;

            for (var x = 0; x < partitionCountX; x++)
            {
                for (var y = 0; y < partitionCountY; y++)
                {
                    var partition = this.Loader.LoadPartition(x, y);
                    partition.InitFields();
                    this.Loader.StorePartition(partition);
                }
            }

            this.Loader.StoreInfoData();
        }

        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="column">Column of the fieldtype</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        public void SetFieldType(int x, int y, byte fieldType, float startingHeight, float endingHeight)
        {
            // 1: Convert partition
            int relX, relY;
            var partition = this.GetPartitionFor(x, y, out relX, out relY);

            // 3. Change partition
            partition.SetFieldType(relX, relY, fieldType, startingHeight, endingHeight);

            // 4. Store partition
            this.Loader.StorePartition(partition);
        }

        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="column">Column of the fieldtype</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        public byte GetFieldType(int x, int y, float height)
        {
            int relX;
            int relY;
            var partition = GetPartitionFor(x, y, out relX, out relY);

            // 3. Change partition
            return partition.GetFieldType(relX, relY, height);
        }

        /// <summary>
        /// Gets the surface information
        /// </summary>
        /// <param name="x1">Upper left X-Coordinate</param>
        /// <param name="y1">Upper left Y-Coordinate</param>
        /// <param name="x2">Lower right X-Coordinate</param>
        /// <param name="y2">Lower right Y-Coordinate</param>
        /// <returns>Information about the fieldtypes. This array is starting with 0 relative to x1/y1.First coordinate is x-coordinate</returns>
        public FieldTypeChangeInfo[][] GetSurfaceInfo(int x1, int y1, int x2, int y2)
        {
            Ensure.That(x1 <= x2);
            Ensure.That(y1 <= y2);

            var result = new FieldTypeChangeInfo[x2 - x1 + 1][];

            for (var x = 0; x <= x2 - x1; x++)
            {
                result[x] = new FieldTypeChangeInfo[y2 - y1 + 1];
            }

            for (var x = x1; x <= x2; x++)
            {
                for (var y = y1; y <= y2; y++)
                {
                    int partitionX, partitionY, relX, relY;
                    this.CalculatePartitionCoordinates(x, y, out partitionX, out partitionY, out relX, out relY);

                    var partition = this.Loader.LoadPartition(partitionX, partitionY);
                    var column = partition.GetColumn(relX, relY);

                    if (column.Count == 1)
                    {
                        result[x - x1][y - y1].ChangeHeight = float.MinValue;
                        result[x - x1][y - y1].FieldType = 0;
                    }
                    else
                    {
                        result[x - x1][y - y1].ChangeHeight = column[1].ChangeHeight;
                        result[x - x1][y - y1].FieldType = column[1].FieldType;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a complete column of a certain position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<FieldTypeChangeInfo> GetColumn(int x, int y)
        {
            int relX, relY;
            var partition = GetPartitionFor(x, y, out relX, out relY);

            // 3. Change partition
            return partition.GetColumn(relX, relY);
        }

        /// <summary>
        /// Gets a complete column of a certain position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public void SetColumn(int x, int y, List<FieldTypeChangeInfo> column)
        {
            int relX, relY;
            var partition = GetPartitionFor(x, y, out relX, out relY);

            // 3. Change partition
            partition.SetColumn(relX, relY, column);
        }

        /// <summary>
        /// Gets a partition for a certain position
        /// </summary>
        /// <param name="x">Absolute X-Coordinate</param>
        /// <param name="y">Absolute Y-Coordinate</param>
        /// <param name="relX">Relative X-Coordinate</param>
        /// <param name="relY">Relative Y-Coordinate</param>
        /// <returns>Loaded partition</returns>
        private Partition GetPartitionFor(int x, int y, out int relX, out int relY)
        {
            // 1: Convert partition
            int partitionX;
            int partitionY;

            this.CalculatePartitionCoordinates(x, y, out partitionX, out partitionY, out relX, out relY);

            // 2. Get Partition
            var partition = this.Loader.LoadPartition(partitionX, partitionY);
            Ensure.That(partition != null);

            return partition;
        }

        /// <summary>
        /// Acquires readlock for a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        public void AcquireReadLock(int x1, int y1, int x2, int y2)
        {
            sync.EnterReadLock();
        }

        /// <summary>
        /// Acquires writelock for a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        public void AcquireWriteLock(int x1, int y1, int x2, int y2)
        {
            sync.EnterWriteLock();
        }

        /// <summary>
        /// Releases readlock for a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        public void ReleaseReadLock(int x1, int y1, int x2, int y2)
        {
            sync.ExitReadLock();
        }

        /// <summary>
        /// Releases writelock for a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        public void ReleaseWriteLock(int x1, int y1, int x2, int y2)
        {
            sync.ExitWriteLock();
        }
    }
}
