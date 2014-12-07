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
        public UserManagementAspNetIdentity()
        {
            var dbContext = new FlexBgUserDbContext();
            this.userStore = new UserStore<
                FlexBgIdentityUser,FlexBgIdentityRole,string,IdentityUserLogin,IdentityUserRole,IdentityUserClaim>
                (dbContext);
            this.roleStore = new RoleStore<FlexBgIdentityRole>(dbContext );
        }

        private UserStore<FlexBgIdentityUser, FlexBgIdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim> userStore;

        private RoleStore<FlexBgIdentityRole> roleStore;

        public override Models.User GetUserById(string userId)
        {
            var user = this.userStore.FindByIdAsync(userId).Result;

            return ConvertToModel(user);
        }

        public override Models.User GetUser(string username)
        {
            var user = this.userStore.FindByNameAsync(username).Result;

            return ConvertToModel(user);
        }

        public override void UpdateUser(Models.User user)
        {
            Ensure.That(user != null);
            var foundUser = this.userStore.FindByIdAsync(user.Id).Result;
            if (foundUser != null)
            {
                // Transfer the necessary stuff
                foundUser.APIKey = user.APIKey;
                foundUser.HasAgreedToTOS = user.HasAgreedToTOS;
                foundUser.PremiumTill = user.PremiumTill;

                // Is doing the update
                this.userStore.UpdateAsync(foundUser).Wait();
            }           
        }

        public override void AddUserToDb(Models.User user)
        {
            var newUser = new FlexBgIdentityUser();
            newUser.APIKey = user.APIKey;
            newUser.Email = user.EMail;
            newUser.EmailConfirmed = user.IsActive;
            newUser.HasAgreedToTOS = user.HasAgreedToTOS;
            //newUser.Id = user.Id;
            newUser.PremiumTill = user.PremiumTill;
            newUser.UserName = user.Username;
            this.userStore.CreateAsync(newUser).Wait();
        }

        public override void AddGroupToDb(Models.Group user)
        {
            var role = new FlexBgIdentityRole();
            role.Id = user.Id;
            role.Name = user.Name;
            role.TokenId = user.TokenId.ToString();
            this.roleStore.CreateAsync(role).Wait();
        }

        public override bool IsUsernameExisting(string username)
        {
            return this.userStore.FindByNameAsync(username).Result != null;
        }

        public override bool IsGroupExisting(string groupName)
        {
            return this.roleStore.FindByNameAsync(groupName).Result != null;
        }

        public override void AddToGroup(Models.Group group, Models.User user)
        {
            var foundUser = this.userStore.FindByIdAsync(user.Id).Result;
            if (foundUser != null)
            {
                this.userStore.AddToRoleAsync(foundUser, group.Name).Wait();
            }
        }

        public override Models.Group GetGroupById(string groupId)
        {
            return ConvertToModel(this.roleStore.FindByIdAsync(groupId).Result);
        }

        public override Models.Group GetGroup(string groupName)
        {
            return ConvertToModel(this.roleStore.FindByNameAsync(groupName).Result);
        }

        public void RemoveUser(Models.User user)
        {
            var foundUser = this.userStore.FindByIdAsync(user.Id).Result;

            // Checks, if we have found the user, if yes, return it
            if (foundUser != null)
            {
                this.userStore.DeleteAsync(foundUser).Wait();
            }
        }

        public IEnumerable<Models.User> GetAllUsers()
        {
            return this.userStore.Users.ToList().Select(x => ConvertToModel(x));
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
            var foundGroup = this.roleStore.FindByIdAsync(group.Id).Result;
            if (foundGroup != null)
            {
                this.roleStore.DeleteAsync(foundGroup).Wait();
            }
        }

        public IEnumerable<Models.Group> GetAllGroups()
        {
            return this.roleStore.Roles.ToList().Select(x => ConvertToModel(x));
        }

        public void RemoveFromGroup(Models.Group group, Models.User user)
        {
            var foundUser = this.userStore.FindByIdAsync(user.Id).Result;
            if (foundUser != null)
            {
                this.userStore.RemoveFromRoleAsync(foundUser, group.Name);
            } 
        }

        public IEnumerable<Models.Group> GetGroupsOfUser(Models.User user)
        {
            var foundUser = this.userStore.FindByIdAsync(user.Id).Result;
            if (foundUser != null)
            {
                return foundUser.Roles.Select(x => this.GetGroupById(x.RoleId));
            }

            return null;
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

        /// <summary>
        /// Converts the IdentityRole to the FlexBG Model
        /// </summary>
        /// <param name="role">User to be converted</param>
        /// <returns>The converted model</returns>
        private static Models.Group ConvertToModel(FlexBgIdentityRole role)
        {
            if (role == null)
            {
                return null;
            }

            var result = new Models.Group()
            {
                Id = role.Id,
                Name = role.Name,
                TokenId = new Guid(role.TokenId)
            };

            return result;
        }
    }
}
