using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.Test;
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
        private UserStore<FlexBgIdentityUser> store = new UserStore<FlexBgIdentityUser>(
            new FlexBgUserDbContext());      
 

        public override Models.User GetUserById(string userId)
        {
            var user = this.store.FindByIdAsync(userId).Result;

            return ConvertToModel(user);
        }

        public override Models.User GetUser(string username)
        {
            var user = this.store.FindByNameAsync(username).Result;

            return ConvertToModel(user);
        }

        public override void UpdateUser(Models.User user)
        {
            Ensure.That(user != null);
            var foundUser = this.store.FindByIdAsync(user.Id).Result;
            if (foundUser != null)
            {
                // Transfer the necessary stuff
                foundUser.APIKey = user.APIKey;
                foundUser.HasAgreedToTOS = user.HasAgreedToTOS;
                foundUser.PremiumTill = user.PremiumTill;

                // Is doing the update
                this.store.UpdateAsync(foundUser);
            }           
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
            return this.store.FindByNameAsync(username).Result != null;
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
            var foundUser = this.store.FindByIdAsync(user.Id).Result;

            // Checks, if we have found the user, if yes, return it
            if (foundUser != null)
            {
                this.store.DeleteAsync(foundUser);
            }
        }

        public IEnumerable<Models.User> GetAllUsers()
        {
            return this.store.Users.ToList().Select(x => ConvertToModel(x));
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
        private static Models.User ConvertToModel(FlexBgIdentityUser user)
        {
            if (user == null)
            {
                return null;
            }

            var result = new Models.User()
            {
                EMail = user.Email,
                Id = user.Id,
                IsActive = user.EmailConfirmed,
                Username = user.UserName,
                HasAgreedToTOS = user.HasAgreedToTOS,
                PremiumTill = user.PremiumTill ?? DateTime.MinValue
            };

            return result;
        }
    }
}
