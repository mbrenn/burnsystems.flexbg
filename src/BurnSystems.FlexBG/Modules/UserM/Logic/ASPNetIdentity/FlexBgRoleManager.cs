using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.ASPNetIdentity
{
    public class FlexBgRoleManager : RoleManager<FlexBgIdentityRole>
    {        
        public FlexBgRoleManager(FlexBgUserDbContext dbContext)
            : base(new RoleStore<FlexBgIdentityRole>(dbContext))
        {
        }
    }
}
