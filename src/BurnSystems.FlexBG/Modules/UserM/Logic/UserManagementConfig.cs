using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Stores the configuration for the usermanagement
    /// </summary>
    public class UserManagementConfig
    {
        /// <summary>
        /// Gets or sets the subject for the register done
        /// </summary>
        public string RegisterDoneMailSubject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the template for the register done
        /// </summary>
        public string RegisterDoneMailTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subject for the register done
        /// </summary>
        public string ForgotPwdMailSubject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the template for the register done
        /// </summary>
        public string ForgotPwdMailTemplate
        {
            get;
            set;
        }
    }
}
