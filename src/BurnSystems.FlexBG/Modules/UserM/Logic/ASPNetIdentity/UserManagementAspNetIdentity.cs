using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.ASPNetIdentity
{
    public class UserManagementAspNetIdentity : UserManagementFramework
    {
        public override Models.User GetUserById(string userId)
        {
            var store = new UserStore<IdentityUser>();

            throw new NotImplementedException();
        }

        public override Models.User GetUser(string usernasme)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(Models.User user)
        {
            throw new NotImplementedException();
        }

        public override void AddUserToDb(Models.User user)
        {
            throw new NotImplementedException();
        }

        public override void AddGroupToDb(Models.Group user)
        {
            throw new NotImplementedException();
        }

        public override bool IsUsernameExisting(string username)
        {
            throw new NotImplementedException();
        }

        public override bool IsGroupExisting(string groupName)
        {
            throw new NotImplementedException();
        }

        public override void AddToGroup(Models.Group group, Models.User user)
        {
            throw new NotImplementedException();
        }

        public override Models.Group GetGroupById(string groupId)
        {
            throw new NotImplementedException();
        }

        public override Models.Group GetGroup(string groupName)
        {
            throw new NotImplementedException();
        }
    }
}
