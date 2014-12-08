using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.ASPNetIdentity
{
    /// <summary>
    /// Defines the user manager for FlexBG
    /// </summary>
    public class FlexBgUserManager : UserManager<FlexBgIdentityUser, string>
    {
        /// <summary>
        /// Initializes a new instance of the FlexBgUserManager class
        /// </summary>
        /// <param name="dbContext">Database context to be used</param>
        /// [Inject]
        public FlexBgUserManager(FlexBgUserDbContext dbContext)
            : base(new UserStore<FlexBgIdentityUser, FlexBgIdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(dbContext))
        {            
            // Konfigurieren der Überprüfungslogik für Benutzernamen.
            this.UserValidator = new UserValidator<FlexBgIdentityUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Konfigurieren der Überprüfungslogik für Kennwörter.
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                /*RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,*/
            };

            // Standardeinstellungen für Benutzersperren konfigurieren
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registrieren von Anbietern für zweistufige Authentifizierung. Diese Anwendung verwendet telefonische und E-Mail-Nachrichten zum Empfangen eines Codes zum Überprüfen des Benutzers.
            // Sie können Ihren eigenen Anbieter erstellen und hier einfügen.
            this.RegisterTwoFactorProvider("Telefoncode", new PhoneNumberTokenProvider<FlexBgIdentityUser>
            {
                MessageFormat = "Ihr Sicherheitscode lautet {0}"
            });
            this.RegisterTwoFactorProvider("E-Mail-Code", new EmailTokenProvider<FlexBgIdentityUser>
            {
                Subject = "Sicherheitscode",
                BodyFormat = "Ihr Sicherheitscode lautet {0}"
            });
            // this.EmailService = new EmailService();
            // this.SmsService = new SmsService();
        }
    }
}
