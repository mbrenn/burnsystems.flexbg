using BurnSystems.FlexBG.Modules.DeponNet.BuildingM;
using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.MapM;
using BurnSystems.FlexBG.Modules.DeponNet.MapM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet
{
    /// <summary>
    /// Defines the game configuration
    /// </summary>
    public static class GameConfig
    {
        /// <summary>
        /// Defines the buildings configurations
        /// </summary>
        public static class Buildings
        {
            public static BuildingType Temple;

            public static BuildingType Townhall;

            public static BuildingType Coppermine;

            public static BuildingType ForestersPlace;

            public static BuildingType SawingPlace;

            public static BuildingType SmeltingPlace;

            public static BuildingType LivingHouse;
        }

        public static class Fields
        {
            /// <summary>
            /// Stores the air
            /// </summary>
            public static FieldType Air;

            /// <summary>
            /// Defines the grass field type as 1
            /// </summary>
            public static FieldType Grass;

            /// <summary>
            /// Defines the grass field type as 2
            /// </summary>
            public static FieldType DarkGrass;
        }

        /// <summary>
        /// Initializes the game configuration
        /// </summary>
        public static void Init(IActivates container)
        {
            InitBuildingTypes(container);
            InitFieldTypes(container);
        }

        private static void InitBuildingTypes(IActivates container)
        {
            var buildingTypeProvider = container.Get<IBuildingTypeProvider>();
            Ensure.That(buildingTypeProvider != null, "No IBuildingTypeProvider");

            var id = 1;
            Buildings.Temple = new BuildingType()
            {
                Id = id++,
                Token = "temple",
                SizeX = 2,
                SizeY = 2
            };
            buildingTypeProvider.Add(Buildings.Temple);

            Buildings.Townhall = new BuildingType()
            {
                Id = id++,
                Token = "townhall",
                SizeX = 2,
                SizeY = 2
            };
            buildingTypeProvider.Add(Buildings.Townhall);

            Buildings.Coppermine = new BuildingType()
            {
                Id = id++,
                Token = "coppermine",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.Coppermine);

            Buildings.ForestersPlace = new BuildingType()
            {
                Id = id++,
                Token = "forestersplace",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.ForestersPlace);

            Buildings.SawingPlace = new BuildingType()
            {
                Id = id++,
                Token = "sawingplace",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.SawingPlace);

            Buildings.SmeltingPlace = new BuildingType()
            {
                Id = id++,
                Token = "smeltingplace",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.SmeltingPlace);

            Buildings.LivingHouse = new BuildingType()
            {
                Id = id++,
                Token = "livinghouse",
                SizeX = 1,
                SizeY = 1
            };
            buildingTypeProvider.Add(Buildings.LivingHouse);
        }

        /// <summary>
        /// Initializes the field types
        /// </summary>
        /// <param name="container">Container to be used</param>
        private static void InitFieldTypes(IActivates container)
        {
            var fieldTypeProvider = container.Get<IFieldTypeProvider>();
            
            /// <summary>
            /// Stores the air
            /// </summary>
            Fields.Air = new FieldType()
            {
                Id = 0,
                Title = "Air"
            };

            /// <summary>
            /// Defines the grass field type as 1
            /// </summary>
            Fields.Grass = new FieldType()
            {
                Id = 1,
                Title = "Grass"
            };

            /// <summary>
            /// Defines the grass field type as 2
            /// </summary>
            Fields.DarkGrass = new FieldType()
            {
                Id = 2,
                Title = "DarkGrass"
            };
        }
    }
}
