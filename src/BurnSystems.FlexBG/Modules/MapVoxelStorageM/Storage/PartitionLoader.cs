using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System.IO;
using System.Xml.Serialization;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// This is the database for reading and writing of partitions.
    /// The methods within this database are not thread-safe
    /// </summary>
    public class PartitionLoader : IPartitionLoader
    {
        /// <summary>
        /// Defines the logger for the partition
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(PartitionLoader));

        /// <summary>
        /// Gets the voxelmap info
        /// </summary>
        [Inject]
        public VoxelMapInfo VoxelMapInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path to database
        /// </summary>
        public string DatabasePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the PartitionLoader class
        /// </summary>
        /// <param name="info">Voxelmap info</param>
        [Inject]
        public PartitionLoader(VoxelMapInfo info)
            : this(info, "data/map/")
        {
            this.VoxelMapInfo = info;
        }

        /// <summary>
        /// Initializes a new instance of the PartitionLoader class
        /// </summary>
        /// <param name="info">Voxelmap info</param>
        /// <param name="databasePath">Path of database</param>
        public PartitionLoader(VoxelMapInfo info, string databasePath)
        {
            this.VoxelMapInfo = info;
            this.DatabasePath = databasePath;

            if (!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(databasePath);
            }
        }

        /// <summary>
        /// Removes all database files
        /// </summary>
        public void Clear()
        {
            var mapInfoPath = Path.Combine(this.DatabasePath, "mapinfo.xml");
            if (File.Exists(mapInfoPath))
            {
                File.Delete(mapInfoPath);
            }

            foreach (var file in Directory.GetFiles(this.DatabasePath, "*.vox"))
            {
                File.Delete(Path.Combine(this.DatabasePath, file));
            }
        }

        /// <summary>
        /// Gets the path and filename for a certain partition
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Path to partition</returns>
        public string GetPathForPartition(int x, int y)
        {
            return Path.Combine(
                this.DatabasePath,
                string.Format("{0}-{1}.vox", x, y));
        }

        /// <summary>
        /// Stores the info data
        /// </summary>
        public void StoreInfoData()
        {
            var serializer = new XmlSerializer(typeof(VoxelMapInfo));

            using (var fileStream = new FileStream(Path.Combine(this.DatabasePath, "mapinfo.xml"), FileMode.Create))
            {
                serializer.Serialize(fileStream, this.VoxelMapInfo);
            }
        }

        /// <summary>
        /// Loads the info data from generic file
        /// </summary>
        /// <returns></returns>
        public VoxelMapInfo LoadInfoData()
        {
            var path = Path.Combine(this.DatabasePath, "mapinfo.xml");
            if (File.Exists(path))
            {
                this.VoxelMapInfo = LoadInfoData(path);
                return this.VoxelMapInfo;
            }

            return null;
        }

        /// <summary>
        /// Loads the info data from generic file
        /// </summary>
        /// <param name="filePath">Path, where mapinfo.xml is stored</param>
        /// <returns>Found voxel map</returns>
        public static VoxelMapInfo LoadInfoData(string filePath)
        {
            var serializer = new XmlSerializer(typeof(VoxelMapInfo));

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                return serializer.Deserialize(fileStream) as VoxelMapInfo;
            }
        }

        /// <summary>
        /// Loads a partition from drive
        /// </summary>
        /// <param name="x">X-Coordinate of the partition</param>
        /// <param name="y">Y-Coordinate of the partition</param>
        /// <returns>Loaded partition</returns>
        public Partition LoadPartition(int x, int y)
        {
            Ensure.That(
                x >= 0 && x < this.VoxelMapInfo.SizeX / this.VoxelMapInfo.PartitionLength &&
                y >= 0 && y < this.VoxelMapInfo.SizeY / this.VoxelMapInfo.PartitionLength);

            var partition = new Partition(x, y, this.VoxelMapInfo.PartitionLength);

            var filePath = this.GetPathForPartition(x, y);
            if (File.Exists(filePath))
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    partition.Load(fileStream);
                } 
                
                logger.LogEntry(LogLevel.Verbose, "Loaded Partition: " + x + ", " + y);
            }
            else
            {
                // Default
                partition.InitFields();
                logger.LogEntry(LogLevel.Verbose, "Initialized Partition: " + x + ", " + y);
            }

            return partition;
        }

        /// <summary>
        /// Stores a certain partition on drive
        /// </summary>
        /// <param name="partition">Partition to be stored</param>
        public void StorePartition(Partition partition)
        {
            var filePath = this.GetPathForPartition(partition.PartitionX, partition.PartitionY);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                partition.Store(fileStream);

                logger.LogEntry(LogLevel.Verbose, "Stored Partition: " + partition.PartitionX + ", " + partition.PartitionY);
            }
        }
    }
}
