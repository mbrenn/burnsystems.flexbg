using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.AutoMap
{
    /// <summary>
    /// Creates a map, if necessary
    /// </summary>
    public class AutoMapModule : IFlexBgRuntimeModule
    {
        private ILog logger = new ClassLogger(typeof(AutoMapModule));
        
        [Inject]
        public IVoxelMap VoxelMap
        {
            get;
            set;
        }

        public void Start()
        {
            if (this.VoxelMap.IsMapCreated())
            {
                logger.LogEntry(LogLevel.Notify, "Map is already created");
            }
            else
            {
                logger.LogEntry(LogLevel.Notify, "Map will be created");
                this.VoxelMap.CreateMap();

                var noiseLayer = new AddNoiseLayer(
                    this.VoxelMap,
                    1,
                    () => 0,
                    () => float.MinValue);
                noiseLayer.Execute();
            }
        }

        public void Shutdown()
        {
        }
    }
}
