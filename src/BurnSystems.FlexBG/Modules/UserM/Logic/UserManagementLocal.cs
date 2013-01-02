using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.GameInfoM;
using BurnSystems.FlexBG.Modules.MailSenderM;
using BurnSystems.FlexBG.Modules.UserM.Data;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.FlexBG.Modules.UserQueryM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Defines the usermanagement
    /// </summary>
    public class UserManagementLocal : IUserManagement, IFlexBgRuntimeModule
    {
        /// <summary>
        /// Stores the logger instance for this class
        /// </summary>
        private ILog classLogger = new ClassLogger(typeof(UserManagementLocal));

        /// <summary>
        /// Gets the database storing the users. 
        /// </summary>
        [Inject(IsMandatory = true)]
        public UserDatabase UserDb
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IGameInfoProvider GameInfoProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration of the usermanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public UserManagementConfig Configuration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user query
        /// </summary>
        [Inject]
        public IUserQuery UserQuery
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a user to database
        /// </summary>
        /// <param name="user">Information of user to be added</param>
        public void AddUser(User user)
        {
            if (this.IsUsernameExisting(user.Username))
            {
                throw new UserManagementException(
                    UserManagementExceptionReason.UsernameExisting,
                    "Username existing");
            }

            if (user.HasAgreedToTOS == false)
            {
                throw new UserManagementException(
                    UserManagementExceptionReason.NoAcceptTos,
                    "The Terms of Services have not been accepted");
            }

            if (string.IsNullOrEmpty(user.Username))
            {
                throw new UserManagementException(
                    UserManagementExceptionReason.NoUsername,
                    "The username is empty");
            }

            if (string.IsNullOrEmpty(user.EMail))
            {
                throw new UserManagementException(
                    UserManagementExceptionReason.NoEmail,
                    "The email is empty");
            }

            if (!user.IsEmailValid)
            {
                throw new UserManagementException(
                    UserManagementExceptionReason.InvalidEmail,
                    "Invalid email address");
            }

            if (string.IsNullOrEmpty(user.ActivationKey))
            {
                user.ActivationKey = StringManipulation.SecureRandomString(32);
            }

            if (string.IsNullOrEmpty(user.APIKey))
            {
                user.APIKey = StringManipulation.SecureRandomString(32);
            }

            user.Id = this.UserDb.Data.GetNextUserId();

            this.UserDb.Data.Users.Add(user);
            this.UserDb.Data.SaveChanges();
        }

        /// <summary>
        /// Gets a certain user by id
        /// </summary>
        /// <param name="userId">Id of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public User GetUser(long userId)
        {
            return this.UserDb.Data.Users.Where(x => x.Id == userId).FirstOrDefault();
        }

        /// <summary>
        /// Gets a certain user by username
        /// </summary>
        /// <param name="username">Name of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public User GetUser(string username)
        {
            return this.UserDb.Data.Users.Where(x => x.Username == username).FirstOrDefault();
        }

        /// <summary>
        /// Gets a certain user by username
        /// </summary>
        /// <param name="username">Name of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public User GetUser(string username, string password)
        {
            var user = this.GetUser(username);
            if (user == null)
            {
                return null;
            }

            if (this.IsPasswordCorrect(user, password))
            {
                return user;
            }

            // Password is not correct
            return null;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            return this.UserDb.Data.Users.ToList();
        }

        /// <summary>
        /// Adds group to usermanagement
        /// </summary>
        /// <param name="group">Group to be added</param>
        public void AddGroup(Group group)
        {
            Ensure.IsNotNull(group);

            if (string.IsNullOrEmpty(group.Title))
            {
                throw new UserManagementException(
                    UserManagementExceptionReason.NoGroupTitle,
                    "No group title");
            }

            group.Id = this.UserDb.Data.GetNextGroupId();

            this.UserDb.Data.Groups.Add(group);
            this.UserDb.Data.SaveChanges();
        }

        /// <summary>
        /// Removes a group from database
        /// </summary>
        /// <param name="group">Group to be removed</param>
        public void RemoveGroup(Group group)
        {
            this.UserDb.Data.Groups.Remove(group);

            foreach (var found in this.UserDb.Data.Memberships.Where(x => x.GroupId == group.Id).ToList())
            {
                this.UserDb.Data.Memberships.Remove(found);
            }
        }

        /// <summary>
        /// Remvoes user from database
        /// </summary>
        /// <param name="user">User to be removed</param>
        public void RemoveUser(User user)
        {
            this.UserDb.Data.Users.Remove(user);

            foreach (var found in this.UserDb.Data.Memberships.Where(x => x.UserId == user.Id).ToList())
            {
                this.UserDb.Data.Memberships.Remove(found);
            }
        }

        /// <summary>
        /// Adds membership
        /// </summary>
        /// <param name="group">Group to be associated</param>
        /// <param name="user">User to be associated</param>
        public void AddToGroup(Group group, User user)
        {
            this.RemoveFromGroup(group, user);

            var membership = new Membership()
            {
                GroupId = group.Id,
                UserId = user.Id
            };

            this.UserDb.Data.Memberships.Add(membership);
        }

        /// <summary>
        /// Removes membership
        /// </summary>
        /// <param name="group">Group of membership to be removed</param>
        /// <param name="user">User of membership to be removed</param>
        public void RemoveFromGroup(Group group, User user)
        {
            foreach (var found in this.UserDb.Data.Memberships.Where(x => x.GroupId == group.Id && x.UserId == user.Id).ToList())
            {
                this.UserDb.Data.Memberships.Remove(found);
            }
        }

        /// <summary>
        /// Checks if user is in group
        /// </summary>
        /// <param name="group">Group to be checked</param>
        /// <param name="user">User to be checked</param>
        public bool IsInGroup(Group group, User user)
        {
            return this.UserDb.Data.Memberships.Any(x => x.GroupId == group.Id && x.UserId == user.Id);
        }

        /// <summary>
        /// Gets all groups where user is member
        /// </summary>
        /// <param name="user">User to be checked</param>
        /// <returns>Enumeration of users</returns>
        public IEnumerable<Group> GetGroupsOfUser(User user)
        {
            return this.UserDb.Data.Memberships
                .Where(x => x.UserId == user.Id)
                .Select(x => this.UserDb.Data.Groups.Where(y => y.Id == x.GroupId).FirstOrDefault())
                .Where(x => x != null)
                .ToList();
        }

        /// <summary>
        /// Saves the changes
        /// </summary>
        public void SaveChanges()
        {
            this.UserDb.Data.SaveChanges();
        }

        /// <summary>
        /// Checks, if username is existing
        /// </summary>
        /// <param name="username">Username to be checked</param>
        /// <returns>true, if username is existing</returns>
        public bool IsUsernameExisting(string username)
        {
            var userCount = this.UserDb.Data.Users.Where(x => x.Username == username).Count();

            return userCount > 0;
        }

        /// <summary>
        /// Encrypts the password for a certain user. 
        /// It is important that the username does not get changed after encryption
        /// </summary>
        /// <param name="user">User whose password gets encrypted</param>
        /// <param name="password">Password to be encrypted</param>
        public void SetPassword(User user, string password)
        {
            var completePassword = user.Username + password + this.GameInfoProvider.GameInfo.PasswordSalt;
            user.EncryptedPassword = completePassword.Sha1();
        }

        /// <summary>
        /// Checks, if the given password is correct for a certain user
        /// </summary>
        /// <param name="user">User to be checked</param>
        /// <param name="password">Password to be checked</param>
        /// <returns>true, if password is correct</returns>
        public bool IsPasswordCorrect(User user, string password)
        {
            var completePassword = user.Username + password + this.GameInfoProvider.GameInfo.PasswordSalt;
            return user.EncryptedPassword == completePassword.Sha1();
        }

        /// <summary>
        /// Starts the plugin
        /// </summary>
        public void Start()
        {
            if (!this.IsUsernameExisting(AdminName))
            {
                if (this.UserQuery != null &&
                    this.UserQuery.Ask(
                        "No Administrator found. Shall an administrator be created?",
                        new[] { "y", "n" },
                        "y") == "y")
                {
                    classLogger.LogEntry(LogLevel.Message, "Administrator is initialized");
                    this.InitAdmin();
                }
            }
        }

        /// <summary>
        /// Performs a shutdown
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// Defines the name of the Administrator
        /// </summary>
        public string AdminName = "Admin";

        /// <summary>
        /// Initializes the admin, if necessary
        /// </summary>
        public void InitAdmin()
        {
            if (this.IsUsernameExisting(AdminName))
            {
                return;
            }

            var user = new User();
            user.Username = "Admin";
            user.EMail = this.GameInfoProvider.GameInfo.AdminEMail;
            user.HasAgreedToTOS = true;
            user.HasNoCredentials = false;
            user.IsActive = true;

            this.SetPassword(user, this.GameInfoProvider.GameInfo.PasswordSalt);

            this.AddUser(user);
        }
    }
}
