using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.ASPNetIdentity
{
    /// <summary>
    /// Defines the class for the role
    /// </summary>
    public class FlexBgIdentityRole : IdentityRole
    {
        /// <summary>
        /// Gets or sets the id of the token
        /// </summary>
        public string TokenId
        {
            get;
            set;
        }
    }
}
