using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.AdminInterfaceM;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Umbra.Views.DetailView;
using BurnSystems.WebServer.Umbra.Views.DetailView.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Admin
{
    /// <summary>
    /// Defines the game admin interface
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class DeponGameAdminInterface : IFlexBgRuntimeModule
    {
        /// <summary>
        /// Stores the admin root data
        /// </summary>
        [Inject(ByName = AdminRootData.Name)]
        public AdminRootData Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the view resolver
        /// </summary>
        [Inject(IsMandatory = true)]
        public BasicDetailViewResolver ViewResolver
        {
            get;
            set;
        }

        public void Start()
        {
            this.Data.Children.Add(
                new GamesTreeView()
                {
                    Id = this.Data.GetNextChildrenId()
                });

            // Creates Entity View for Users
            this.ViewResolver.Add(
                (x) => x is GamesTreeView,
                (x) => new EntityView(
                    new EntityViewConfig(
                        new EntityViewListTable<GamesTreeView>(
                            "Games",
                            EntityViewElementProperty.Create().Labelled("Id").For("Id").AsInteger(),
                            EntityViewElementProperty.Create().Labelled("Title").For("Title").AsString(),
                            EntityViewElementProperty.Create().Labelled("Paused").For("IsPaused").AsBoolean(),
                            EntityViewElementProperty.Create().Labelled("Max Players").For("MaxPlayers").AsInteger())
                        .SetSelector((a, y) => y.GetChildren(a).Select(z => z.Entity)),
                        new EntityViewDetailTable(
                            "CreateGame",
                            EntityViewElementProperty.Create().Labelled("Title").For("Title").AsString(),
                            EntityViewElementProperty.Create().Labelled("Description").For("Description").AsString(),
                            EntityViewElementProperty.Create().Labelled("MaxPlayers").For("MaxPlayers").AsInteger(),
                            EntityViewElementProperty.Create().Labelled("Width of Map").For("MapWidth").AsInteger(),
                            EntityViewElementProperty.Create().Labelled("Height of Map").For("MapHeight").AsInteger())
                            .WithOverrideUrl("games/Create"))));
        }

        public void Shutdown()
        {
        }
    }
}
