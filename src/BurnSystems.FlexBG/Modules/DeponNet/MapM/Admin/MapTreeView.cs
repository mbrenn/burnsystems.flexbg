using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.DetailView;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapM.Admin
{
    /// <summary>
    /// Defines the map tree view item
    /// </summary>
    public class MapTreeView : DetailView
    {
        /// <summary>
        /// Performs the necessary dispatch
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <param name="context">Context to be used</param>
        public override void Dispatch(IActivates container, WebServer.Dispatcher.ContextDispatchInformation context)
        {
            this.AddScript(
                "js/lib/viewtypes/flexbg.viewtypes.voxelmapview.js");
            this.Title = this.Item.ToString();
            this.ViewTypeToken = "BurnSystems.FlsxBG.DetailView.VoxelMapView";
            this.UserData = null;
        }
    }
}
