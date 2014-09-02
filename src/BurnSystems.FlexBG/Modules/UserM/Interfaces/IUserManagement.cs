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

        /// <summary>
        /// Sets the user data for a certain user
        /// </summary>
        /// <param name="user">User to be updated</param>
        /// <param name="key">Key of the userdata</param>
        /// <param name="value">Value of the userdata</param>
        void SetUserData(User user, string key, object value);

        /// <summary>
        /// Gets userdata of a certain type
        /// </summary>
        /// <typeparam name="T">Type of the userdata</typeparam>
        /// <param name="user">User to be used</param>
        /// <param name="key">Key of the userdata</param>
        /// <returns>Returned data or null, if not correct type or not existing</returns>
        T GetUserData<T>(User user, string key);

        long AddGroup(Group group);

        void RemoveGroup(Group group);

        IEnumerable<Group> GetAllGroups();

        Group GetGroup(long groupId);

        Group GetGroup(string groupName);

        void AddToGroup(Group group, User user);

        void RemoveFromGroup(Group group, User user);

        IEnumerable<Group> GetGroupsOfUser(User user);

        void InitAdmin();

        void UpdateUser(User user);
    }
}
