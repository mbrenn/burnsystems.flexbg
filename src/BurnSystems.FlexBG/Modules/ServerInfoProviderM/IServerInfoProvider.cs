using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.ServerInfoProviderM
{
    public interface IServerInfoProvider
    {
        /// <summary>
        /// Gets the serverinfo
        /// </summary>
        /// <returns></returns>
        ServerInfo GetServerInfo();
    }
}
