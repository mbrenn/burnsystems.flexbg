﻿using BurnSystems.FlexBG.Modules.GameInfoM;
using BurnSystems.FlexBG.Modules.MailSenderM;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;

namespace BurnSystems.FlexBG.Modules.UserM.Controllers
{
    /// <summary>
    /// Defines the usercontroller offering Login, Register, etc
    /// </summary>
    public class UsersController : Controller
    {
        private ILog logger = new ClassLogger(typeof(UsersController));

        /// <summary>
        /// Gets or sets the usermanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public IUserManagement UserManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Sends the mail
        /// </summary>
        [Inject]        
        public IMailSender MailSender
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the game info
        /// </summary>
        [Inject]
        public IGameInfoProvider GameInfo
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult Login([PostModel] LoginModel model, string returnUrl)
        {
            // Check, if we are ok
            //var user = this.UserManagement.GetUser(model.Username, model.Password);

            var isLoggedIn = false;
            /*if (user != null)
            {
                isLoggedIn = true;
                // Logged in! 
                //FormsAuthentication.SetAuthCookie(user.Username, model.IsPersistant);

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Error in Login
                //this.ModelState.AddModelError("", "Der angegebene Benutzername oder das angegebene Kennwort ist ungültig.");
            }*/

            var result = new
            {
                success = isLoggedIn
            };

            return this.TemplateOrJson(result);
        }

        /*
        [WebMethod]
        public void Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }*/

        [WebMethod]
        public IActionResult Register([PostModel] RegisterModel model)
        {
            var hasSuccess = false;

            // Checks, if username already exists
            if (this.UserManagement.IsUsernameExisting(model.Username))
            {
                throw new MVCProcessException(
                    "usernameexisting",
                    "The username is already existing");
            }

            if (model.AcceptTOS == false)
            {
                throw new MVCProcessException(
                    "noaccepttos",
                    "The Terms of Services have not been accepted");
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                throw new MVCProcessException(
                    "nousername",
                    "The username is empty");
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new MVCProcessException(
                    "nopassword",
                    "The password is empty");
            }

            if (string.IsNullOrEmpty(model.EMail))
            {
                throw new MVCProcessException(
                    "noemail",
                    "The email is empty");
            }

            if (model.Password != model.Password2)
            {
                throw new MVCProcessException(
                    "passwordnotequal",
                    "The given passwords are not equal");
            }

            //if (this.ModelState.IsValid)
            {
                // Everything seems ok, create and add user
                var user = new UserM.Models.User();
                user.HasAgreedToTOS = true;
                user.Username = model.Username;
                this.UserManagement.EncryptPassword(user, model.Password);
                user.EMail = model.EMail;

                this.UserManagement.AddUser(user);
                hasSuccess = true;
            }

            var result = new
            {
                success = hasSuccess
            };

            return this.TemplateOrJson(result);
        }

        /*
        [WebMethod]
        public void PasswordForgotten([PostModel] ForgotPasswordModel model)
        {
            if (this.MailSender == null)
            {
                logger.Error("No Mailsender available");
            }

            Assert.That(this.MailSender, Is.Not.Null, "Keine Mailversende-Engine konfiguriert");

            var user = this.UserManagement.GetUser(model.Username);
            if (user == null)
            {
                this.ModelState["Username"].Errors.Add("Dieser Nutzer ist uns nicht bekannt. Wer bist du?");
            }

            if (this.ModelState.IsValid)
            {
                // Ok, we have user, create new activation key and send out mail
                user.ActivationKey = StringManipulation.SecureRandomString(16);
                this.UserManagement.SaveChanges();

                var mailMessage = new MailMessage(
                    this.GameInfo.GameInfo.AdminEMail,
                    user.EMail,
                    "Kennwort vergessen",
                    this.GameInfo.GameInfo.Url + "Users/CreateNewPassword?u=" + user.Id.ToString() + "&a=" + user.ActivationKey);
                this.MailSender.SendMail(mailMessage);

                return this.View("PasswordForgottenSuccess");
            }

            return View();
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="u">Id of the user</param>
        /// <param name="a">Activationkey of the user</param>
        /// <returns>Changed password</returns>
        [WebMethod]
        public void CreateNewPassword(long u, string a)
        {
            // Gets user
            var user = this.UserManagement.GetUser(u);
            if (user == null || user.ActivationKey != a)
            {
                this.ModelState.AddModelError(string.Empty, "Irgendwie erscheint es uns komisch. Der Link schien nicht korrekt zu sein und wir konnten kein neues Kennwort erzeugen.");
            }

            if (this.ModelState.IsValid)
            {
                var newPassword = StringManipulation.SecureRandomString(8);
                this.UserManagement.EncryptPassword(user, newPassword);
                this.UserManagement.SaveChanges();

                var model = new CreateNewPasswordModel();
                model.Username = user.Username;
                model.NewPassword = newPassword;

                return this.View(model);
            }
            else
            {
                return this.View("CreateNewPasswordFailed");
            }
        }

        [WebMethod]
        public ActionResult ChangePassword([PostModel] ChangePasswordModel model)
        {
            var user = this.UserManagement.GetUser(HttpContext.User.Identity.Name);
            Assert.That(user, Is.Not.Null);
            if (!this.UserManagement.IsPasswordCorrect(user, model.OldPassword))
            {
                this.ModelState["OldPassword"].Errors.Add("Dieses Kennwort scheint nicht korrekt zu sein");
            }

            if (this.ModelState.IsValid)
            {
                this.UserManagement.EncryptPassword(user, model.NewPassword);
                this.UserManagement.SaveChanges();
                return this.View("ChangePasswordSuccess");
            }

            return this.View();
        }
         * */
    }
}