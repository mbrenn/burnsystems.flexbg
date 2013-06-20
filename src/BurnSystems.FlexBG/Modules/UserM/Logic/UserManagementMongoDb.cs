﻿using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.Database.MongoDb;
using BurnSystems.FlexBG.Modules.MailSenderM;
using BurnSystems.FlexBG.Modules.ServerInfoM;
using BurnSystems.FlexBG.Modules.UserM.Data;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.FlexBG.Modules.UserQueryM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Parser;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
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
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class UserManagementMongoDb : IUserManagement, IFlexBgRuntimeModule
    {
        /// <summary>
        /// Just a simple, global synchronization object
        /// </summary>
        private static object syncObject = new object();

        /// <summary>
        /// Stores the logger instance for this class
        /// </summary>
        private ILog classLogger = new ClassLogger(typeof(UserManagementLocal));

        /// <summary>
        /// Gets the database storing the users. 
        /// </summary>
        [Inject(IsMandatory = true)]
        public MongoDbConnection Db
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IServerInfoProvider GameInfoProvider
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
        public long AddUser(User user)
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

            user.Id = this.GetNextUserId();
            this.UserCollection.Insert(user);

            return user.Id;
        }

        /// <summary>
        /// Gets a certain user by id
        /// </summary>
        /// <param name="userId">Id of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public User GetUser(long userId)
        {
            return this.UserCollection.AsQueryable().Where(x => x.Id == userId).FirstOrDefault();
        }

        /// <summary>
        /// Gets a certain user by username
        /// </summary>
        /// <param name="username">Name of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public User GetUser(string username)
        {
            return this.UserCollection.AsQueryable().Where(x => x.Username == username).FirstOrDefault();
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
        /// Remvoes user from database
        /// </summary>
        /// <param name="user">User to be removed</param>
        public void RemoveUser(User user)
        {
            this.UserCollection.Remove(Query.EQ("Id", user.Id));

            this.MembershipCollection.Remove(Query.EQ("UserId", user.Id));
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            return this.UserCollection.AsQueryable().ToList();
        }

        /// <summary>
        /// Adds group to usermanagement
        /// </summary>
        /// <param name="group">Group to be added</param>
        public long AddGroup(Group group)
        {
            Ensure.IsNotNull(group);

            if (string.IsNullOrEmpty(group.Name))
            {
                throw new UserManagementException(
                    UserManagementExceptionReason.NoGroupTitle,
                    "No group title");
            }

            group.Id = this.GetNextGroupId();

            this.GroupCollection.Insert(group);

            return group.Id;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Group> GetAllGroups()
        {
            return this.GroupCollection.AsQueryable().ToList();
        }

        /// <summary>
        /// Gets the group
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        public Group GetGroup(long groupId)
        {
            return this.GroupCollection.AsQueryable().FirstOrDefault(x => x.Id == groupId);
        }

        /// <summary>
        /// Gets the group
        /// </summary>
        /// <param name="groupName">NAme of the group</param>
        public Group GetGroup(string groupName)
        {
            return this.GroupCollection.AsQueryable().FirstOrDefault(x => x.Name == groupName);
        }

        /// <summary>
        /// Removes a group from database
        /// </summary>
        /// <param name="group">Group to be removed</param>
        public void RemoveGroup(Group group)
        {
            this.GroupCollection.Remove(Query.EQ("Id", group.Id));

            this.MembershipCollection.Remove(Query.EQ("GroupId", group.Id));
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

            this.MembershipCollection.Insert(membership);
        }

        /// <summary>
        /// Sets the user data for a certain user
        /// </summary>
        /// <param name="user">User to be updated</param>
        /// <param name="key">Key of the userdata</param>
        /// <param name="value">Value of the userdata</param>
        public void SetUserData(User user, string key, object value)
        {
            user.UserData[key] = value;
            this.UpdateUser(user);
        }

        /// <summary>
        /// Gets userdata of a certain type
        /// </summary>
        /// <typeparam name="T">Type of the userdata</typeparam>
        /// <param name="user">User to be used</param>
        /// <param name="key">Key of the userdata</param>
        /// <returns>Returned data or null, if not correct type or not existing</returns>
        public T GetUserData<T>(User user, string key)
        {
            object result;
            if (user.UserData.TryGetValue(key, out result))
            {
                if (result is T)
                {
                    return (T)result;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Stores the user into database
        /// </summary>
        /// <param name="user">User to be updated</param>
        public void UpdateUser(User user)
        {
            this.UserCollection.Save(user);
        }

        /// <summary>
        /// Removes membership
        /// </summary>
        /// <param name="group">Group of membership to be removed</param>
        /// <param name="user">User of membership to be removed</param>
        public void RemoveFromGroup(Group group, User user)
        {
            this.MembershipCollection.Remove(
                Query.And(Query.EQ("GroupId", group.Id), Query.EQ("UserId", user.Id)));
        }

        /// <summary>
        /// Checks if user is in group
        /// </summary>
        /// <param name="group">Group to be checked</param>
        /// <param name="user">User to be checked</param>
        public bool IsInGroup(Group group, User user)
        {
            return this.MembershipCollection.AsQueryable().Any(x => x.GroupId == group.Id && x.UserId == user.Id);
        }

        /// <summary>
        /// Gets all groups where user is member
        /// </summary>
        /// <param name="user">User to be checked</param>
        /// <returns>Enumeration of users</returns>
        public IEnumerable<Group> GetGroupsOfUser(User user)
        {
            return this.MembershipCollection.AsQueryable()
                .Where(x => x.UserId == user.Id)
                .Select(x => this.GroupCollection.AsQueryable().Where(y => y.Id == x.GroupId).FirstOrDefault())
                .ToList();
        }

        /// <summary>
        /// Saves the changes
        /// </summary>
        public void SaveChanges()
        {
        }

        /// <summary>
        /// Checks, if username is existing
        /// </summary>
        /// <param name="username">Username to be checked</param>
        /// <returns>true, if username is existing</returns>
        public bool IsUsernameExisting(string username)
        {
            return this.UserCollection.AsQueryable().Any(x => x.Username == username);
        }

        /// <summary>
        /// Checks, if group is existing
        /// </summary>
        /// <param name="groupName">Name of the group</param>
        /// <returns>True, if group is existing</returns>
        public bool IsGroupExisting(string groupName)
        {
            return this.GroupCollection.AsQueryable().Any(x => x.Name == groupName);
        }

        /// <summary>
        /// Encrypts the password for a certain user. 
        /// It is important that the username does not get changed after encryption
        /// </summary>
        /// <param name="user">User whose password gets encrypted</param>
        /// <param name="password">Password to be encrypted</param>
        public void SetPassword(User user, string password)
        {
            Ensure.That(user != null);
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("password is empty");
            }

            var completePassword = user.Username + password + this.GameInfoProvider.ServerInfo.PasswordSalt;
            user.EncryptedPassword = completePassword.Sha1();

            this.UpdateUser(user);
        }

        /// <summary>
        /// Checks, if the given password is correct for a certain user
        /// </summary>
        /// <param name="user">User to be checked</param>
        /// <param name="password">Password to be checked</param>
        /// <returns>true, if password is correct</returns>
        public bool IsPasswordCorrect(User user, string password)
        {
            var completePassword = user.Username + password + this.GameInfoProvider.ServerInfo.PasswordSalt;
            return user.EncryptedPassword == completePassword.Sha1();
        }

        /// <summary>
        /// Stores the value whether the class already had been initialized. 
        /// If not, MongoDB will get contact with our Entities
        /// </summary>
        private static bool alreadyInitialized = false;

        /// <summary>
        /// Starts the plugin
        /// </summary>
        public void Start()
        {
            // Initializes MongoDB
            if (!alreadyInitialized)
            {
                BsonClassMap.RegisterClassMap<UserDatabaseInfo>();
                BsonClassMap.RegisterClassMap<User>();
                BsonClassMap.RegisterClassMap<Group>();
                BsonClassMap.RegisterClassMap<Membership>();
                alreadyInitialized = true;
            }

            if (!this.IsUsernameExisting(AdminName) || !this.IsGroupExisting(GroupAdminName))
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
        public const string AdminName = "Admin";

        /// <summary>
        /// Defines the name of the Administrator
        /// </summary>
        public const string GroupAdminName = "Administrators";

        /// <summary>
        /// Initializes the admin, if necessary
        /// </summary>
        public void InitAdmin()
        {
            if (!this.IsUsernameExisting(AdminName))
            {
                classLogger.Message("Creating user with name: " + AdminName);
                var password = this.UserQuery.Ask(
                    "Give password: ",
                    null,
                    this.GameInfoProvider.ServerInfo.PasswordSalt);


                var user = new User();
                user.Username = AdminName;
                user.EMail = this.GameInfoProvider.ServerInfo.AdminEMail;
                user.HasAgreedToTOS = true;
                user.HasNoCredentials = false;
                user.IsActive = true;

                this.AddUser(user);

                this.SetPassword(user, password);
            }

            if (!this.IsGroupExisting(GroupAdminName))
            {
                var group = new Group();
                group.Name = GroupAdminName;
                group.TokenId = Group.AdministratorsToken;

                this.AddGroup(group);
            }

            var adminUser = this.GetUser(AdminName);
            var adminGroup = this.GetGroup(GroupAdminName);

            this.AddToGroup(adminGroup, adminUser);
        }

        /// <summary>
        /// Gets the next user id
        /// </summary>
        /// <returns></returns>
        public long GetNextUserId()
        {
            lock (syncObject)
            {
                var collection = this.Db.Database.GetCollection<UserDatabaseInfo>("UserdatabaseInfo");
                var info = collection.AsQueryable().SingleOrDefault();

                if (info == null)
                {
                    info = new UserDatabaseInfo();
                    info.LastUserId = 1;
                }
                else
                {
                    info.LastUserId++;
                }

                collection.Save(info);

                return info.LastUserId;
            }
        }

        /// <summary>
        /// Gets a specific user by token id
        /// </summary>
        /// <param name="id">Id of the user</param>
        public User GetUserByToken(Guid id)
        {
            lock (syncObject)
            {
                return this.UserCollection.AsQueryable().Where(x => x.TokenId == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the next user id
        /// </summary>
        /// <returns></returns>
        public long GetNextGroupId()
        {
            lock (syncObject)
            {
                var collection = this.Db.Database.GetCollection<UserDatabaseInfo>("UserdatabaseInfo");
                var info = collection.AsQueryable().SingleOrDefault();

                if (info == null)
                {
                    info = new UserDatabaseInfo();
                    info.LastGroupId = 1;
                }
                else
                {
                    info.LastGroupId++;
                }

                collection.Save(info);

                return info.LastGroupId;
            }
        }

        public void SetPersistantCookie(long userId, string series, string token)
        {
            lock (syncObject)
            {
                var user = this.GetUser(userId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                user.PersistantTokens[series] = token;
                this.UpdateUser(user);
            }
        }

        public bool CheckPersistantCookie(long userId, string series, string token)
        {
            lock (syncObject)
            {
                var user = this.GetUser(userId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                string tokenCheck;

                if (user.PersistantTokens.TryGetValue(series, out tokenCheck))
                {
                    if (token == tokenCheck)
                    {
                        user.PersistantTokens.Remove(series);
                        this.UpdateUser(user);
                        return true;
                    }
                    else
                    {
                        classLogger.Fail("Perhabs Security breach of cookie for user " + userId);

                        // Clears all cookies
                        user.PersistantTokens.Clear();
                        this.UpdateUser(user);
                    }
                }

                return false;
            }
        }

        public void DeletePersistantCookie(long userId, string series)
        {
            lock (syncObject)
            {
                var user = this.GetUser(userId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                user.PersistantTokens.Remove(series);
                this.UpdateUser(user);
            }
        }

        public MongoCollection<User> UserCollection
        {
            get { return this.Db.Database.GetCollection<User>("Users"); }
        }

        public MongoCollection<Group> GroupCollection
        {
            get { return this.Db.Database.GetCollection<Group>("Groups"); }
        }

        public MongoCollection<Membership> MembershipCollection
        {
            get { return this.Db.Database.GetCollection<Membership>("Memberships"); }
        }
    }
}
