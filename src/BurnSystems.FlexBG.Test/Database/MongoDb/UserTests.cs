using BurnSystems.FlexBG.Modules.Database.MongoDb;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using BurnSystems.FlexBG.Modules.UserM.Logic;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.ServerInfoM;

namespace BurnSystems.FlexBG.Test.Database.MongoDb
{
    /// <summary>
    /// Tests the user addition in Mongodatabase
    /// </summary>
    [TestFixture]
    public class UserTests
    {
        public static IActivates Init()
        {
            var result = MongoDbTests.Init() as ActivationContainer;

            result.Bind<IUserManagement>().To<UserManagementMongoDb>();
            result.Bind<IServerInfoProvider>().ToConstant(
                new ServerInfoProvider(null)).AsSingleton();
            return result;
        }

        [Test]
        public void TestAddAndRetrieval()
        {
            var container = Init();
            var dbConnection = container.Get<MongoDbConnection>();
            var database = dbConnection.Database;

            var users = database.GetCollection<User>("users");
            users.Drop();

            var newUser  = new User();
            newUser.Username = "Testusername";
            newUser.TokenId = Guid.NewGuid();
            newUser.PremiumTill = DateTime.UtcNow.AddDays(2);
            newUser.IsActive = true;
            newUser.Id = 12;
            newUser.EMail = "brenn@depon.net";
            users.Insert(newUser);

            var foundUsers = users.Count();
            Assert.That(foundUsers, Is.EqualTo(1));

            var foundUser = users.AsQueryable<User>().First();
            Assert.That(foundUser != null);

            // Let's see
            Assert.That(foundUser.Username, Is.EqualTo(newUser.Username));
            Assert.That(foundUser.TokenId, Is.EqualTo(newUser.TokenId));
            Assert.That((foundUser.PremiumTill - newUser.PremiumTill).TotalSeconds < 1);
            Assert.That(foundUser.IsActive, Is.EqualTo(newUser.IsActive));
            Assert.That(foundUser.Id, Is.EqualTo(newUser.Id));
            Assert.That(foundUser.EMail, Is.EqualTo(newUser.EMail));
        }

        [Test]
        public void TestUserManagement()
        {
        }
    }
}
