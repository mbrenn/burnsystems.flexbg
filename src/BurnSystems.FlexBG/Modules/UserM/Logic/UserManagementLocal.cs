using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.GameInfoM;
using BurnSystems.FlexBG.Modules.UserM.Data;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.FlexBG.Modules.UserQueryM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
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
        /// Stores the database
        /// </summary>
        private UserDatabaseLocal userDb = new UserDatabaseLocal();

        /// <summary>
        /// Gets the database storing the users. 
        /// </summary>
        public UserDatabaseLocal UserDb
        {
            get { return this.userDb; }
        }

        [Inject(IsMandatory = true)]
        public IGameInfoProvider GameInfoProvider
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

            if (string.IsNullOrEmpty(user.ActivationKey))
            {
                user.ActivationKey = StringManipulation.SecureRandomString(32);
            }

            if (string.IsNullOrEmpty(user.APIKey))
            {
                user.APIKey = StringManipulation.SecureRandomString(32);
            }

            this.UserDb.Users.Add(user);
            this.UserDb.SaveChanges();
        }

        /// <summary>
        /// Gets a certain user by id
        /// </summary>
        /// <param name="userId">Id of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public User GetUser(long userId)
        {
            return this.UserDb.Users.Where(x => x.Id == userId).FirstOrDefault();
        }

        /// <summary>
        /// Gets a certain user by username
        /// </summary>
        /// <param name="username">Name of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public User GetUser(string username)
        {
            return this.UserDb.Users.Where(x => x.Username == username).FirstOrDefault();
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
            return this.UserDb.Users.ToList();
        }

        /// <summary>
        /// Saves the changes
        /// </summary>
        public void SaveChanges()
        {
            this.UserDb.SaveChanges();
        }

        /// <summary>
        /// Checks, if username is existing
        /// </summary>
        /// <param name="username">Username to be checked</param>
        /// <returns>true, if username is existing</returns>
        public bool IsUsernameExisting(string username)
        {
            var userCount = this.UserDb.Users.Where(x => x.Username == username).Count();

            return userCount > 0;
        }

        /// <summary>
        /// Encrypts the password for a certain user. 
        /// It is important that the username does not change after encryption
        /// </summary>
        /// <param name="user">User whose password gets encrypted</param>
        /// <param name="password">Password to be encrypted</param>
        public void EncryptPassword(User user, string password)
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

            this.EncryptPassword(user, this.GameInfoProvider.GameInfo.PasswordSalt);

            this.AddUser(user);
        }

        /// <summary>
        /// Stores the filepath to user data
        /// </summary>
        string filePath = "data/users.data";

        /// <summary>
        /// Loads database from file
        /// </summary>
        private void LoadFromFile()
        {
            if (!File.Exists(filePath))
            {
                classLogger.LogEntry(LogLevel.Message, "No file for UserManagementLocal existing, creating empty database");
                return;
            }

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    this.userDb = formatter.Deserialize(stream) as UserDatabaseLocal;
                }
            }
            catch (Exception exc)
            {
                classLogger.LogEntry(LogLevel.Fatal, "Loading for userdatabase failed: " + exc.Message);

                if (this.UserQuery == null || this.UserQuery.Ask(
                        "Shall a new database be created?",
                        new[] { "y", "n" },
                        "n") == "n")
                {
                    throw;
                }
                else
                {
                    classLogger.LogEntry(LogLevel.Message, "New database will be created");
                }
            }
        }

        /// <summary>
        /// Stores database to file
        /// </summary>
        private void StoreToFile()
        {
            if (!Directory.Exists("data"))
            {
                Directory.CreateDirectory("data");
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this.UserDb);
            }
        }

        /// <summary>
        /// Starts the usermanagement
        /// </summary>
        public void Start()
        {
            this.LoadFromFile();

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
        /// Stops the usermanagement
        /// </summary>
        public void Shutdown()
        {
            this.StoreToFile();
        }
    }
}
