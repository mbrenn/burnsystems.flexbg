using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.UserM.Models;

namespace BurnSystems.FlexBG.Modules.UserM.Interfaces
{
    public interface IUserManagement
    {
        void AddUser(User user);

        User GetUser(long userId);

        User GetUser(string username);

        User GetUser(string username, string password);

        IEnumerable<User> GetAllUsers();

        bool IsUsernameExisting(string username);

        void EncryptPassword(User user, string password);

        bool IsPasswordCorrect(User user, string password);
        
        void InitAdmin();
    }
}
