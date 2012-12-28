using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResearchM
{
    public class Research
    {
        public long Id
        {
            get;
            set;
        }

        public long PlayerId
        {
            get;
            set;
        }

        public long ResearchTypeId
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }
    }
}
