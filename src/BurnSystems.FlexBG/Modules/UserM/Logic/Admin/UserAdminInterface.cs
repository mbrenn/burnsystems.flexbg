using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.AdminInterfaceM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic.Admin
{
    /// <summary>
    /// Initializes a new instance of the admin interface
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class UserAdminInterface : IFlexBgRuntimeModule
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
        /// Initializes a new instance of the UserAdminInterface
        /// </summary>
        /// <param name="activationContext">Activationcontext to be used</param>
        public UserAdminInterface()
        {
        }

        /// <summary>
        /// Starts the user admin interface
        /// </summary>
        public void Start()
        {
            this.Data.Children.Add(
                new UsersTreeView()
                {
                    Id = this.Data.GetNextChildrenId()
                });
            this.Data.Children.Add(
                new GroupsTreeView()
                {
                    Id = this.Data.GetNextChildrenId()
                });
        }

        /// <summary>
        /// Stops the user admin interface
        /// </summary>
        public void Shutdown()
        {
        }
    }
}
