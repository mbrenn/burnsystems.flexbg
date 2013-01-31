using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.MapM;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.FlexBG.Modules.WayPointCalculationM;
using BurnSystems.ObjectActivation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Test.WayPointCalculationM
{
    [TestFixture]
    public class TestOnlyHeights
    {
        /// <summary>
        ///  Initialies the activation container
        /// </summary>
        /// <returns></returns>
        private IActivates Init()
        {
            var container = new ActivationContainer("Test");

            var configurationStorage = new ConfigurationStorage();
            container.Bind<IConfigurationStorage>().ToConstant(configurationStorage);

            // VoxelMap
            BurnSystems.FlexBG.Modules.MapVoxelStorageM.Module.Load(container);

            container.Bind<IWayPointCalculation>().To<BurnSystems.FlexBG.Modules.WayPointCalculationM.OnlyHeight.WayPointCalculation>();

            var game = new Game();
            game.Id = 1;
            container.BindToName("CurrentGame").ToConstant(game);

            // Starts up FlexBG
            var runtime = new Runtime(container, "config");

            runtime.StartUpCore();

            // Create map
            var info = new VoxelMapInfo();
            info.SizeX = 100;
            info.SizeY = 100;
            info.PartitionLength = 100;

            var voxelMap = container.Get<IVoxelMap>();
            voxelMap.CreateMap(game.Id, info);
            CompleteFill.Execute(voxelMap, game.Id, FieldTypes.Air);
            new AddNoiseLayer(voxelMap, FieldTypes.Grass, () => 0, () => float.MinValue).Execute(game.Id);

            return container;
        }
        
        [Test]
        public void TestSetup()
        {
            var container = this.Init();
            var wayPointCalculation = container.Get<IWayPointCalculation>();

            var wayPoints = wayPointCalculation.CalculateWaypoints(
                new System.Windows.Media.Media3D.Vector3D(0, 0, 0),
                new System.Windows.Media.Media3D.Vector3D(50, 50, 50), 
                null);

            Assert.That(wayPoints, Is.Not.Null);
            Assert.That(wayPoints.Count() > 0);
        }
    }
}
