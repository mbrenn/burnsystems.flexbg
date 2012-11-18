using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.UserM.Models
{
    /// <summary>
    /// Stores the userinformation
    /// </summary>
    public class User
    {
        public User()
        {
            this.PremiumTill = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public virtual long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public virtual string Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the encrypted password
        /// </summary>
        public virtual string EncryptedPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public virtual string EMail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the activation key
        /// </summary>
        public virtual string ActivationKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the API key
        /// </summary>
        public virtual string APIKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the user is a premium user
        /// </summary>
        public virtual bool IsPremium
        {
            get { return DateTime.Now < this.PremiumTill; }
        }

        /// <summary>
        /// Gets or sets a date until user is premium
        /// </summary>
        public virtual DateTime PremiumTill
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active
        /// </summary>
        public virtual bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user has agreed to the terms of service
        /// </summary>
        public virtual bool HasAgreedToTOS
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user has no credentials. 
        /// This can happen by Facebook-Login
        /// </summary>
        public virtual bool HasNoCredentials
        {
            get;
            set;
        }
    }
}
