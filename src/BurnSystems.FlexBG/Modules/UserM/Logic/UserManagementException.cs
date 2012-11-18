using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    public enum UserManagementExceptionReason
    {
        UsernameExisting
    }

    public class UserManagementException : ApplicationException
    {
        public UserManagementExceptionReason Reason
        {
            get;
            private set;
        }

        public UserManagementException(UserManagementExceptionReason reason, string message)
            : base(message)
        {
            this.Reason = reason;
        }
    }
}
