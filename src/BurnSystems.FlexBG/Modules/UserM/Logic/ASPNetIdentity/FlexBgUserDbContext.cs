using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.ASPNetIdentity
{
    public class FlexBgUserDbContext : IdentityDbContext<FlexBgIdentityUser, FlexBgIdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public FlexBgUserDbContext()
            : base("DefaultConnection")
        {            
        }

        public static FlexBgUserDbContext Create()
        {
            return new FlexBgUserDbContext();
        }
    }
}
