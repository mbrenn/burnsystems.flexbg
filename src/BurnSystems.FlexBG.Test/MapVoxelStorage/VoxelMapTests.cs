using System;
using System.IO;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using NUnit.Framework;

namespace BurnSystems.FlexBG.Test.MapVoxelStorage
{
    [TestFixture]
    public class VoxelMapTests
    {
        [Test]
        public void TestMapCreation()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 1000,
                SizeY = 2000
            };

            var database = new PartitionLoader(
                info,
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database, 5);

            var voxelMap = new VoxelMap(info);
            voxelMap.Loader = cache;

            voxelMap.CreateMap();

            for (var x = 0; x < 1000; x += 10)
            {
                Assert.That(voxelMap.GetFieldType(x, x / 2, 100), Is.EqualTo(0));
            }
        }

        [Test]
        public void TestMapRetrieval()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 1000,
                SizeY = 1000
            };

            var database = new PartitionLoader(
                info,
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database);
            var voxelMap = new VoxelMap(info);
            voxelMap.Loader = cache;
            voxelMap.CreateMap();

            for (var x = 0; x < info.SizeX; x ++)
            {
                for (var y = 0; y < info.SizeY; y++)
                {
                    voxelMap.SetFieldType(x, y, 2, ((float)x) * ((float)y) + 1, 0);
                }
            }

            for (var x = 0; x < info.SizeX; x++)
            {
                for (var y = 0; y < info.SizeY; y++)
                {
                    Assert.That(voxelMap.GetFieldType(x, y, (((float)x) * ((float)y) + 1) / 2), Is.EqualTo(2));
                    Assert.That(voxelMap.GetFieldType(x, y, (((float)x) * ((float)y) + 1) * 2), Is.EqualTo(0));
                }
            }
        }

        [Test]
        public void TestMapSurface()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 1000,
                SizeY = 1000
            };

            var database = new PartitionLoader(
                info,
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database);
            var voxelMap = new VoxelMap(info);
            voxelMap.Loader = cache;
            voxelMap.CreateMap();

            for (var x = 0; x < info.SizeX; x++)
            {
                for (var y = 0; y < info.SizeY; y++)
                {
                    voxelMap.SetFieldType(x, y, 2, ((float)x) * ((float)y) + 1, 0);
                }
            }

            var surfaceInfo = voxelMap.GetSurfaceInfo(0, 0, 200, 200);

            for (var x = 0; x < 200; x++)
            {
                for (var y = 0; y < 200; y++)
                {
                    Assert.That(surfaceInfo[x][y].ChangeHeight, Is.EqualTo(x * y + 1));
                    Assert.That(surfaceInfo[x][y].FieldType, Is.EqualTo(2));
                }
            }

            var surfaceInfo2 = voxelMap.GetSurfaceInfo(200, 200, 400, 400);

            for (var x = 0; x < 200; x++)
            {
                for (var y = 0; y < 200; y++)
                {
                    Assert.That(surfaceInfo2[x][y].ChangeHeight, Is.EqualTo((200 + x) * (200 + y) + 1));
                    Assert.That(surfaceInfo2[x][y].FieldType, Is.EqualTo(2));
                }
            }
        }

        [Test]
        public void TestCreateBigMap()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 250,
                SizeX = 2000,
                SizeY = 2000
            };

            var database = new PartitionLoader(
                info,
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database);
            var voxelMap = new VoxelMap(info);
            voxelMap.Loader = cache;
            voxelMap.CreateMap();
        }
    }
}
