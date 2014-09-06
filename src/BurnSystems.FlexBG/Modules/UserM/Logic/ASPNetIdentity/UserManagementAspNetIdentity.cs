using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.ASPNetIdentity
{
    public class UserManagementAspNetIdentity : UserManagementFramework, IUserManagement
    {
        private UserStore<IdentityUser> store = new UserStore<IdentityUser>();

        public override Models.User GetUserById(string userId)
        {
            var user = this.store.FindByIdAsync(userId).Result;

            return ConvertToModel(user);
        }

        public override Models.User GetUser(string username)
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


        public void RemoveUser(Models.User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.User> GetAllUsers()
        {
            return this.store.Users.ToList().Select(x => this.ConvertToModel(x));
        }

        public void SetUserData(Models.User user, string key, object value)
        {
            throw new NotImplementedException();
        }

        public T GetUserData<T>(Models.User user, string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveGroup(Models.Group group)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Group> GetAllGroups()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromGroup(Models.Group group, Models.User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Group> GetGroupsOfUser(Models.User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the IdentityUser to the FlexBG Model
        /// </summary>
        /// <param name="user">User to be converted</param>
        /// <returns>The converted model</returns>
        private Models.User ConvertToModel(IdentityUser user)
        {
            var result = new Models.User()
            {
                EMail = user.Email,
                Id = user.Id,
                IsActive = user.EmailConfirmed,
                Username = user.UserName
            };

            return result;
        }
    }
}
