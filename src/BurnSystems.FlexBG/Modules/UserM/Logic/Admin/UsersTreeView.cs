using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.Admin
{
    /// <summary>
    /// Gets the users treeview
    /// </summary>
    public class UsersTreeView : BaseTreeViewItem
    {
        public override string Title
        {
            get { return "Users"; }
        }

        public override IEnumerable<ITreeViewItem> GetChildren(IActivates activates)
        {
            var userManagement = activates.Get<IUserManagement>();
            Ensure.IsNotNull(userManagement, "No IUserManagement bound");
            var users = userManagement.GetAllUsers();

            return users.Select(x =>
                new GenericTreeViewItem()
            {
                Id = x.Id,
                Title = x.Username,
                Entity = x
            });
        }
    }
}
