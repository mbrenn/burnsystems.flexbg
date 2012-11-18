using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BurnSystems.FlexBG.Interfaces;

namespace BurnSystems.FlexBG.Modules.ServerInfoProviderM
{
    public class ServerInfoProvider : IServerInfoProvider
    {
        public ServerInfo GetServerInfo()
        {
            var serverInfo = new ServerInfo();

            serverInfo.ServerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            serverInfo.ServerStartUp = DateTime.Now;

            return serverInfo;
        }
    }
}
