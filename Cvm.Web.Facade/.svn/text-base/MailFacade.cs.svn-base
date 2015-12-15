using System;
using System.Configuration;
using System.Net.Mail;
using System.Web.Security;
using Cvm.Backend.Business.Externals;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Users;
using Cvm.Web.Navigation;
using log4net;
using Napp.Common.MessageManager;
using Napp.Web.AdminContentMgr;

namespace Cvm.Web.Facade
{
    public class MailFacade
    {
        private static ILog log = LogManager.GetLogger(typeof(MailFacade));
        private static SmtpClient _smtp;

        private void SendMailUtil(MailMessage msg)
        {
            if (!String.IsNullOrEmpty(GetSmtpHost()))
            {
                Smtp.Send(msg);
                MessageManager.Current.PostMessage("MailFacade.MailSent");
            }
            else
            {
                MessageManager.Current.PostMessage("MailFacade.MailNotSent", msg.To.ToString(), "<xmp>" + msg.Body + "</xmp>");
                log.Info("Mail not sent to " + msg.To.ToString() + ":");
                log.Info(msg.Subject);
                log.Info(msg.Body);
            }
        }

        private void SendMailUtil(string emailReceiver, string subject, string body)
        {
            if (!String.IsNullOrEmpty(GetSmtpHost()))
            {
                Smtp.Send(ConfigurationManager.AppSettings["smtpFrom"], emailReceiver,
                          subject,
                          body);
                MessageManager.Current.PostMessage("MailFacade.MailSent");
                log.Info("Mail sent to" + emailReceiver + ":");
                log.Info(subject);
                log.Info(body);
            }
            else
            {
                MessageManager.Current.PostMessage("MailFacade.MailNotSent", emailReceiver, "<xmp>" + body + "</xmp>");
                log.Info("Mail not sent to " + emailReceiver + ":");
                log.Info(subject);
                log.Info(body);
            }
        }

        private SmtpClient Smtp
        {
            get
            {
                if (_smtp == null)
                {
                    _smtp = new SmtpClient();
                    var smtpHost = GetSmtpHost();
                    _smtp.Host = smtpHost;
                }
                return _smtp;
            }
        }

        private string GetSmtpHost()
        {
            return ConfigurationManager.AppSettings["smtpHost"];
        }

        public void SendInvitationMail(UserObj mySysUser)
        {
            MembershipUser user = Membership.GetUser(mySysUser.UserName);
            string password = user.GetPassword();
            SendInvitationMail(user.UserName, user.Email, password);
        }

        public void SendInvitationMail(string userName, string emailReceiver, string password)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(emailReceiver);
            msg.Bcc.Add("invitationmail@cvnav.dk");
            msg.Subject = AdminContentMgr.instance.GetContent("MailFacade.InvitationMailSubject");
            msg.Body = AdminContentMgr.instance.GetContent("MailFacade.InvitationMailBody", userName, password);

            SendMailUtil(msg);
        }

        public void SendPrintResource(string emailReceiver, Resource relatedResourceObj, ExternalLink relatedExternalLinkObj)
        {
            string subject = AdminContentMgr.instance.GetContent("MailFacade.PrintResourceSubject");
            string body = AdminContentMgr.instance.GetContent("MailFacade.PrintResourceBody", relatedResourceObj.FullName, CvmPages.ExternalPrintLink(relatedExternalLinkObj.LinkGuid).GetLinkAsHref());
            SendMailUtil(emailReceiver, subject, body);
        }

        public void SignupSendToFriend(string email, string message, string rawUrl)
        {
            string subject = AdminContentMgr.instance.GetContent("MailFacade.SignupSendToFriendSubject");
            if (subject == null) subject = "From CVNavigator";
            else subject = subject.Trim();
            string body = AdminContentMgr.instance.GetContent("MailFacade.SignupSendToFriendBody", message, rawUrl);
            SendMailUtil(email, subject, body);
        }

        /// <summary>
        /// Sends an email to the given sysEmail saying that the contactEmail wants a meeting setup regarding a contract on the given resource
        /// </summary>
        /// <param name="sysEmail"></param>
        /// <param name="contactEmail"></param>
        /// <param name="resourceId"></param>
        /// <param name="fullName"></param>
        public void SendContactRequestedMail(string sysEmail, string contactEmail, long resourceId, string fullName)
        {
            SendMailUtil(sysEmail, "Meeting requested for " + fullName, "Meeting requested by " + contactEmail);
        }

        /// <summary>
        /// Sends an email to the contactEmail confirming that a meeting will be set up
        /// </summary>
        /// <param name="contactEmail"></param>
        /// <param name="profileTitle"></param>
        /// <param name="getInitials"></param>
        public void SendContactRequesteConfirmationEmail(string contactEmail, string profileTitle, string getInitials)
        {
            SendMailUtil(contactEmail, "Meeting confirmation on '" + profileTitle + "'",
                         "Thank you for requesting this resource. You will be contacted soon.");
        }
    }
}