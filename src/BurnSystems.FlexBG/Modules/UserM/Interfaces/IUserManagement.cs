using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.UserM.Models;

namespace BurnSystems.FlexBG.Modules.UserM.Interfaces
{
    public interface IUserManagement
    {
        long AddUser(User user);

        void RemoveUser(User user);

        User GetUser(long userId);

        User GetUser(string username);

        User GetUser(string username, string password);

        IEnumerable<User> GetAllUsers();

        bool IsUsernameExisting(string username);

        void SetPassword(User user, string password);

        bool IsPasswordCorrect(User user, string password);

        long AddGroup(Group group);

        void RemoveGroup(Group group);

        IEnumerable<Group> GetAllGroups();

        Group GetGroup(long groupId);

        Group GetGroup(string groupName);

        void AddToGroup(Group group, User user);

        void RemoveFromGroup(Group group, User user);

        IEnumerable<Group> GetGroupsOfUser(User user);

        void InitAdmin();

        void SaveChanges();

        void UpdateUser(User user);
    }
}
