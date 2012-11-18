using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    /// <summary>
    /// Defines the mail sending engine
    /// </summary>
    public interface IMailSender
    {
        /// <summary>
        /// Sends a mail
        /// </summary>
        /// <param name="mailMessage">Mail message to be sent</param>
        void SendMail(MailMessage mailMessage);
    }
}
