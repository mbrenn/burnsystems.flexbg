using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BurnSystems.FlexBG.Modules.ServerInfoProviderM
{
    public class ServerInfo
    {
        public string ServerVersion
        {
            get;
            set;
        }

        public DateTime ServerStartUp
        {
            get;
            set;
        }
    }
}
