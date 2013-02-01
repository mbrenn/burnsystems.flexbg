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
        public const int Region = 6;

        /// <summary>
        /// Stores the entity type id of research
        /// </summary>
        public const int Research = 7;

        /// <summary>
        /// Stores the entity type id of unit groups
        /// </summary>
        public const int UnitGroup = 8;
    }
}
