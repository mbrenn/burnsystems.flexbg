using BurnSystems.FlexBG.Modules.DeponNet.GameM.Admin;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers
{
    public class DeponGamesAdminController : Controller
    {
        /// <summary>
        /// Gets or sets the game management
        /// </summary>
        [Inject(IsMandatory = true)]
        public IGameManagement GameManagement
        {
            get;
            set;
        }

        [WebMethod]
        public void Create([PostModel] DeponCreateGameModel model)
        {
            this.GameManagement.Create(model.Title, model.Description, model.MaxPlayers);
        }
    }
}
