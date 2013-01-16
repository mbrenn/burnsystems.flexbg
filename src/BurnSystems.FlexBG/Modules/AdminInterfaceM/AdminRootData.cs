using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.AdminInterfaceM
{
    /// <summary>
    /// Root object for data
    /// </summary>
    public class AdminRootData : ITreeViewItem
    {
        /// <summary>
        /// Stores the name 
        /// </summary>
        public const string Name = "AdminRootData";

        public long Id
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "FlexBG"; }
        }

        public string ImageUrl
        {
            get { return string.Empty; }
        }

        public bool IsExpandable
        {
            get { return true; }
        }

        public IEnumerable<ITreeViewItem> GetChildren(IActivates activates)
        {
            return null;
        }

        public override string ToString()
        {
            return "FlexBG Interface";
        }
    }
}
