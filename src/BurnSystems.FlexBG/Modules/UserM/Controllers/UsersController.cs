using BurnSystems.Extensions;
using BurnSystems.FlexBG.Modules.GameInfoM;
using BurnSystems.FlexBG.Modules.MailSenderM;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Logic;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.MVC;
using BurnSystems.WebServer.Modules.UserManagement;
using BurnSystems.WebServer.Parser;

namespace BurnSystems.FlexBG.Modules.UserM.Controllers
{
    /// <summary>
    /// Defines the usercontroller offering Login, Register, etc
    /// </summary>
    public class UsersController : BurnSystems.WebServer.Modules.MVC.Controller
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

        [Inject(IsMandatory = true)]
        public IAuthentication Authentication
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mailsender
        /// </summary>
        [Inject(IsMandatory = true)]
        public ITemplateParser TemplateParser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration of the usermanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public UserManagementConfig Configuration
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
            var user = this.UserManagement.GetUser(model.Username, model.Password);

            var isLoggedIn = false;
            if (user != null)
            {
                // Check, if user has agreed to tos and check, if user is active
                if (!user.IsActive)
                {
                    throw new MVCProcessException("login_usernotactive", "User is not active");
                }

                if (!user.HasAgreedToTOS)
                {
                    throw new MVCProcessException("login_noaccepttos", "User has not accepted TOS");
                }

                // Everything ok
                isLoggedIn = true;

                // Logged in! 
                this.Authentication.LoginUser(model.Username, model.Password);

                if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith("/"))
                {
                    return new RedirectActionResult(returnUrl);
                }
                else
                {
                    var result = new
                    {
                        success = isLoggedIn,
                        username = model.Username
                    };

                    return this.TemplateOrJson(result);
                }
            }
            else
            {
                // Error in Login
                throw new MVCProcessException(
                    "login_unknowncredentials",
                    "Unknown credentials of user");
            }
        }

        
        [WebMethod]
        public IActionResult Logout()
        {
            this.Authentication.LogoutUser();

            return this.Json(
                new { success = true });
        }

        [WebMethod]
        public IActionResult Register([PostModel] RegisterModel model)
        {
            var hasSuccess = false;

            // Checks, if username already exists
            if (this.UserManagement.IsUsernameExisting(model.Username))
            {
                throw new MVCProcessException(
                    "register_usernameexisting",
                    "The username is already existing");
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new MVCProcessException(
                    "register_nopassword",
                    "The password is empty");
            }

            if (model.Password != model.Password2)
            {
                throw new MVCProcessException(
                    "register_passwordnotequal",
                    "The given passwords are not equal");
            }

            try
            {
                // Everything seems ok, create and add user
                var user = new UserM.Models.User();
                user.HasAgreedToTOS = true;
                user.Username = model.Username;
                this.UserManagement.SetPassword(user, model.Password);
                user.EMail = model.EMail;

                this.UserManagement.AddUser(user);

                // Send mail to user
                var authLink = string.Format(
                    "{0}auth.bspx?u={1}&a={2}",
                    this.GameInfo.GameInfo.Url,
                    user.Id,
                    user.ActivationKey);

                var subject = this.Configuration.RegisterDoneMailSubject;
                var template = this.TemplateParser.Parse(
                    this.Configuration.RegisterDoneMailTemplate,
                    user,
                    new System.Collections.Generic.Dictionary<string, object>()
                        .With("AuthLink", authLink));

                this.MailSender.SendMail(
                    user.EMail,
                    subject,
                    template);

                hasSuccess = true;
            }
            catch (UserManagementException exc)
            {
                switch (exc.Reason)
                {
                    default:
                        throw new MVCProcessException("register_" + exc.Reason.ToString().ToLower(), exc.Message);
                }
            }

            var result = new
            {
                success = hasSuccess
            };

            return this.TemplateOrJson(result);
        }

        [WebMethod]
        public IActionResult GetLoginStatus([Inject(ByName = "CurrentUser")] User currentUser)
        {
            if (currentUser == null)
            {
                return this.TemplateOrJson(
                    new
                    {
                        success = true,
                        isloggedin = false
                    });
            }
            else
            {
                return this.TemplateOrJson(
                    new
                    {
                        success= true,
                        isloggedin = true,
                        username = currentUser.Username,
                        id = currentUser.Id
                    });
            }
        }

        /// <summary>
        /// Activates the user
        /// </summary>
        /// <param name="u">Id of the user to be activated</param>
        /// <param name="a">Authentication key</param>
        [WebMethod]
        public IActionResult Activate(long u, string a)
        {
            var success = false;

            var user = this.UserManagement.GetUser(u);
            if (user != null)
            {
                if (user.IsActive)
                {
                    // Already activated... nothing to do here
                    success = true;
                }
                else if (!string.IsNullOrEmpty(a) && user.ActivationKey == a)
                {
                    user.IsActive = true;
                    user.ActivationKey = string.Empty;
                    this.UserManagement.SaveChanges();
                    success = true;
                }
            }

            // Creates model and return
            var result = new
            {
                success = success
            };

            return this.TemplateOrJson(result);
        }

        [WebMethod]
        public IActionResult PasswordForgotten([PostModel] ForgotPasswordModel model)
        {
            Ensure.That(this.MailSender != null, "No Mailsender available");

            var user = this.UserManagement.GetUser(model.Username);
            if (user == null)
            {
                throw new MVCProcessException("forgot_unknownusername", "User is not known");
            }

            if (!user.IsActive)
            {
                throw new MVCProcessException("forgot_usernotactive", "User is not active");
            }

            // Ok, we have user, create new activation key and send out mail
            user.ActivationKey = StringManipulation.SecureRandomString(16);
            this.UserManagement.SaveChanges();

            // Creates mail to be send
            var templateContent =
                this.TemplateParser.Parse(
                    this.Configuration.ForgotPwdMailTemplate,
                    user,
                    new System.Collections.Generic.Dictionary<string, object>()
                        .With("ForgotLink", this.GameInfo.GameInfo.Url + "newpassword.bspx?u=" + user.Id.ToString() + "&a=" + user.ActivationKey));
            this.MailSender.SendMail(
                user.EMail,
                this.Configuration.ForgotPwdMailSubject,
                templateContent);

            var result = new
            {
                User = user,
                success = true
            };

            return this.TemplateOrJson(result);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="u">Id of the user</param>
        /// <param name="a">Activationkey of the user</param>
        /// <returns>Changed password</returns>
        [WebMethod]
        public IActionResult CreateNewPassword(long u, string a)
        {
            // Gets user
            var user = this.UserManagement.GetUser(u);

            if (user == null || !user.IsActive || user.ActivationKey != a)
            {
                throw new MVCProcessException("invalidactivationkey", "Unknown activationkey");
            }

            var newPassword = StringManipulation.SecureRandomString(8);
            this.UserManagement.SetPassword(user, newPassword);
            this.UserManagement.SaveChanges();

            var model = new
            {
                username = user.Username,
                newPassword = newPassword,
                success = true
            };

            return this.TemplateOrJson(model);
        }

        [WebMethod]
        public IActionResult ChangePassword([PostModel] ChangePasswordModel model, [Inject(ByName = "CurrentUser", IsMandatory=true)] User currentUser)
        {
            if (!this.UserManagement.IsPasswordCorrect(currentUser, model.OldPassword))
            {
                throw new MVCProcessException("changepassword_wrongpassword", "Old password is not correct");
            }

            this.UserManagement.SetPassword(currentUser, model.NewPassword);
            this.UserManagement.SaveChanges();

            var result = new
            {
                success = true
            };
            return this.TemplateOrJson(result);
        }
    }
}