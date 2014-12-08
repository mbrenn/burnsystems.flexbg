using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.Test;
using Microsoft.AspNet.Identity;
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
        /// <summary>
        /// Stores the usermanager
        /// </summary>
        private FlexBgUserManager userManager;

        private RoleManager<FlexBgIdentityRole> roleManager;

        public UserManagementAspNetIdentity()
        {
            var dbContext = new FlexBgUserDbContext();

            this.roleManager = new FlexBgRoleManager(dbContext);
            this.userManager = new FlexBgUserManager(dbContext);
        }

        public override Models.User GetUserById(string userId)
        {
            var user = this.userManager.FindById(userId);

            return ConvertToModel(user);
        }

        public override Models.User GetUser(string username)
        {
            var user = this.userManager.FindByName(username);

            return ConvertToModel(user);
        }

        public override void UpdateUser(Models.User user)
        {
            Ensure.That(user != null);
            var foundUser = this.userManager.FindById(user.Id);
            if (foundUser != null)
            {
                // Transfer the necessary stuff
                foundUser.APIKey = user.APIKey;
                foundUser.HasAgreedToTOS = user.HasAgreedToTOS;
                foundUser.PremiumTill = user.PremiumTill;

                // Is doing the update
                this.userManager.Update(foundUser);
            }           
        }

        public override void AddUserToDb(Models.User user)
        {
            var newUser = new FlexBgIdentityUser();
            newUser.APIKey = user.APIKey;
            newUser.Email = user.EMail;
            newUser.EmailConfirmed = user.IsActive;
            newUser.HasAgreedToTOS = user.HasAgreedToTOS;
            newUser.PremiumTill = user.PremiumTill;
            newUser.UserName = user.Username;
            this.userManager.Create(newUser);

            user.Id = newUser.Id;
        }

        public override void AddGroupToDb(Models.Group group)
        {
            var role = new FlexBgIdentityRole();
            role.Name = group.Name;
            role.TokenId = group.TokenId.ToString();
            this.roleManager.CreateAsync(role).Wait();
            
            group.Id = role.Id;
        }

        public override void SetPassword(Models.User user, string password)
        {
            this.userManager.RemovePassword(user.Id);
            var result = this.userManager.AddPassword(user.Id, password);
            if (!result.Succeeded)
            {                
                throw new InvalidOperationException(result.Errors.First());
            }
        }

        public override bool IsPasswordCorrect(Models.User user, string password)
        {
            var foundUser = this.userManager.FindById(user.Id);
            if (foundUser != null)
            {
                return this.userManager.CheckPassword(foundUser, password);
            }

            return false;
        }

        public override bool IsUsernameExisting(string username)
        {
            return this.userManager.FindByName(username) != null;
        }

        public override bool IsGroupExisting(string groupName)
        {
            return this.roleManager.FindByNameAsync(groupName).Result != null;
        }

        public override void AddToGroup(Models.Group group, Models.User user)
        {
            this.userManager.AddToRole(user.Id, group.Name);
        }

        public override Models.Group GetGroupById(string groupId)
        {
            return ConvertToModel(this.roleManager.FindByIdAsync(groupId).Result);
        }

        public override Models.Group GetGroup(string groupName)
        {
            return ConvertToModel(this.roleManager.FindByNameAsync(groupName).Result);
        }

        public void RemoveUser(Models.User user)
        {
            var foundUser = this.userManager.FindById(user.Id);

            // Checks, if we have found the user, if yes, return it
            if (foundUser != null)
            {
                this.userManager.Delete(foundUser);
            }
        }

        public IEnumerable<Models.User> GetAllUsers()
        {

            return this.userManager.Users.ToList().Select(x => ConvertToModel(x));
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
            var foundGroup = this.roleManager.FindByIdAsync(group.Id).Result;
            if (foundGroup != null)
            {
                this.roleManager.DeleteAsync(foundGroup).Wait();
            }
        }

        public IEnumerable<Models.Group> GetAllGroups()
        {
            return this.roleManager.Roles.ToList().Select(x => ConvertToModel(x));
        }

        public void RemoveFromGroup(Models.Group group, Models.User user)
        {
            this.userManager.RemoveFromRole(user.Id, group.Id);
        }

        public IEnumerable<Models.Group> GetGroupsOfUser(Models.User user)
        {
            var roles = this.userManager.GetRoles(user.Id);
            if (roles != null)
            {
                return roles.Select(x => this.GetGroup(x));
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
