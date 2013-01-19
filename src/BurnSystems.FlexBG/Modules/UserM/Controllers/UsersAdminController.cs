using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;

namespace BurnSystems.FlexBG.Modules.UserM.Controllers
{
    /// <summary>
    /// Controller for user admin
    /// </summary>
    public class UsersAdminController : BurnSystems.WebServer.Modules.MVC.Controller
    {
        [Inject(IsMandatory = true)]
        public IUserManagement UserManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the password of the user
        /// </summary>
        /// <param name="model">Model to be used</param>
        [WebMethod]
        public IActionResult SetPassword([PostModel] AdminSetPasswordModel model)
        {
            var user = this.UserManagement.GetUser(model.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User with " + model.Id + " not found");
            }

            this.UserManagement.SetPassword(user, model.NewPassword);

            return this.Json(
                new
                {
                    success = true
                });
        }
    }
}
