using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Defines the webuser management
    /// </summary>
    public class WebUserManagementView : IWebUserManagement
    {
        private IUserManagement usermanagement;

        [Inject]
        public WebUserManagementView(IUserManagement local)
        {
            Ensure.That(local != null, "IUserManagement is null");

            this.usermanagement = local;
        }
    
        public IWebUser GetUser(string username)
        {
            var user = this.usermanagement.GetUser(username);
            if (user == null)
            {
                return null;
            }

            return new WebUserView(this.usermanagement, user);
        }

        public IWebUser GetUser(string username, string password)
        {
            var user = this.usermanagement.GetUser(username, password);
            if (user == null)
            {
                return null;
            }

            return new WebUserView(this.usermanagement, user);
        }
    }
}
