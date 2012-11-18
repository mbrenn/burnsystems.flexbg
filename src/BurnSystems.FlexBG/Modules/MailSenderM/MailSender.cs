using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using System.Net.Mail;
using System.Xml.Serialization;
using System.Net;
using BurnSystems.Logging;

namespace BurnSystems.FlexBG.Modules.MailSenderM
{
    /// <summary>
    /// Implements the mailsender
    /// </summary>
    public class MailSender : IMailSender
    {
        private ILog logger = new ClassLogger(typeof(MailSender));

        private Settings settings;

        public MailSender(IConfigurationStorage storage)
        {
            var xmlSettings = storage.Documents
                .Elements("FlexBG")
                .Elements("MailSender")
                .LastOrDefault();

            if (xmlSettings == null)
            {
                logger.LogEntry(LogLevel.Critical, "No Settings found within configuration '/FlexBG/MailSender'");
                throw new InvalidOperationException("No Settings found within configuration '/FlexBG/MailSender'");
            }
            else
            {
                this.settings = (Settings)
                    new XmlSerializer(typeof(Settings)).Deserialize(xmlSettings.CreateReader());
            }
        }

        /// <summary>
        /// Sends a mail
        /// </summary>
        /// <param name="mailMessage">Mail message to be sent</param>
        public void SendMail(MailMessage mailMessage)
        {
            var smtpClient = new System.Net.Mail.SmtpClient(this.settings.Host, this.settings.Port);
            smtpClient.Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);

            mailMessage.Subject = this.settings.SubjectPrefix + mailMessage.Subject;

            // Sends mail
            smtpClient.SendAsync(mailMessage, null);
            smtpClient.SendCompleted += (x, y) =>
                {
                    logger.LogEntry(LogLevel.Message, "Mail " + mailMessage.Subject + " to " + mailMessage.To + " has been sent.");
                };
        }
    }
}
