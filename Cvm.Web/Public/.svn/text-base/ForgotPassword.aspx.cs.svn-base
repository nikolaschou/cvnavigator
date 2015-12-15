using System;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;

namespace Cvm.Web.Public
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected void ForgotPassword_SendingMail(object sender, MailMessageEventArgs e)
        {
            UserObj user = QueryMgr.instance.GetUserObjByUserNameOrNullCached(((PasswordRecovery)(sender)).UserName);

            e.Message.Body = e.Message.Body.Replace("<%firstName%>", user.FirstName);
            e.Message.Body = e.Message.Body.Replace("<%lastName%>", user.LastName);

            MailAddress bcc = new MailAddress("forgotpassword@cvnav.dk");
            e.Message.Bcc.Add(bcc);
        }
    }
}
