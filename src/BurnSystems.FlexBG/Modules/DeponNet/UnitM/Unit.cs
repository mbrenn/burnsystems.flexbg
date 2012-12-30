using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    public class Unit
    {
        /// <summary>
        /// Gets or sets the unit id
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the playerid
        /// </summary>
        public long PlayerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field id
        /// </summary>
        public long FieldId
        {
            get;
            set;
        }

        public int UnitTypeId
        {
            get;
            set;
        }

        public long Amount
        {
            get;
            set;
        }
    }
}
