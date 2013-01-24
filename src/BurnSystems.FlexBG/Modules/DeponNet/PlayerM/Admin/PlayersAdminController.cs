using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Admin
{
    public class PlayersAdminController : Controller
    {
        [Inject]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }
    }
}
