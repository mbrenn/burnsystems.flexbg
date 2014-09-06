using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.ASPNetIdentity
{
    public class FlexBgUserDbContext : IdentityDbContext<FlexBGIdentityUser>
    {
        public FlexBgUserDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static FlexBgUserDbContext Create()
        {
            return new FlexBgUserDbContext();
        }
    }
}
