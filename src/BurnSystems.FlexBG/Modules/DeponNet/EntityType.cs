using BurnSystems.FlexBG.Modules.LockMasterM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet
{
    /// <summary>
    /// Enumeration of possible entity types
    /// </summary>
    public static class EntityType
    {
        /// <summary>
        /// Stores the entity type id of the player
        /// </summary>
        public const int Player = 1;

        /// <summary>
        /// Stores the entity type id of the town
        /// </summary>
        public const int Town = 2;

        /// <summary>
        /// Stores the entity type id of the unit
        /// </summary>
        public const int Unit = 3;

        /// <summary>
        /// Stores the entity type id of game
        /// </summary>
        public const int Game = 4;

        /// <summary>
        /// Stores the entity type id of building
        /// </summary>
        public const int Building = 5;

        /// <summary>
        /// Stores the entity type id of region
        /// </summary>
        public const int Map = 6;

        /// <summary>
        /// Stores the entity type id of region
        /// </summary>
        public const int Region = 7;

        /// <summary>
        /// Stores the entity type id of research
        /// </summary>
        public const int Research = 8;

        /// <summary>
        /// Stores the entity type id of unit groups
        /// </summary>
        public const int UnitGroup = 9;

        /// <summary>
        /// Stores the entity type for server
        /// </summary>
        public const int Server = 100;

        /// <summary>
        /// Apply the relationships between the entity types to the lockmaster
        /// </summary>
        /// <param name="lockMaster">Lock master to be used</param>
        public static void Apply(ILockMaster lockMaster)
        {
            lockMaster.AddRelationShip(Server, Game);
            lockMaster.AddRelationShip(Game, Player);
            lockMaster.AddRelationShip(Player, Building);
            lockMaster.AddRelationShip(Player, Research);
            lockMaster.AddRelationShip(Player, UnitGroup);
            lockMaster.AddRelationShip(Player, Unit);
            lockMaster.AddRelationShip(Player, Town);
            lockMaster.AddRelationShip(Game, Region);
            lockMaster.AddRelationShip(Game, Map);
        }
    }
}
