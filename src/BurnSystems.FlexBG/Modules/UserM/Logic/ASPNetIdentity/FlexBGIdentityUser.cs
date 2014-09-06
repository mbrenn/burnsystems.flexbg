using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.ASPNetIdentity
{
    public class FlexBGIdentityUser : IdentityUser
    {
        private DateTime? premiumTill;

        private bool hasAgreedToTOS;

        /// <summary>
        /// Gets or sets a date until user is premium
        /// </summary>
        public virtual DateTime? PremiumTill
        {
            get { return this.premiumTill; }
            set { this.premiumTill = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user has agreed to the terms of service
        /// </summary>
        public virtual bool HasAgreedToTOS
        {
            get { return this.hasAgreedToTOS; }
            set { this.hasAgreedToTOS = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<FlexBGIdentityUser> manager)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            return userIdentity;
        }
    }
}
