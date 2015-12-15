using System;
using System.Web;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using CVnavNewsLetterWrapper;

namespace Cvm.Web.Public
{
    public partial class AcceptConditions : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            /*This won't work as we enter infinite loop
             * if (ContextObjectHelper.IsSimulatingUser())
            {
                DoRedirect();
            }*/
            if (ContextObjectHelper.CurrentUser.AcceptedConditions)
            {
                this.Panel1.Visible = false;
                Utl.Msg.PostMessage("AcceptConditions.AlreadyAccepted");
                this.ContinueBtn.Visible = true;
            }
        }

        protected void OnClickContinueBtn(object sender, EventArgs e)
        {
            if (!AcceptedCheckbox.Checked)
            {
                Modalpop.Show();

                if (ChkBoxReceiveNewsLetter.Checked)
                {
                    MailChimp cm = new MailChimp();
                    cm.SubscribeUser(ContextObjectHelper.CurrentUser.Email, ContextObjectHelper.CurrentUser.FirstName, ContextObjectHelper.CurrentUser.LastName);

                    // Update database
                }

            }
            else
            {
                ContextObjectHelper.CurrentUser.AcceptedConditions = true;
                DoRedirect();
            }
        }

        private void DoRedirect()
        {
            if (QueryStringHelperCvm.Instance.HasParm(QueryParmCvm.ReturnUrl))
            {
                String url = HttpUtility.HtmlDecode(QueryStringHelperCvm.Instance.GetParmOrFail(QueryParmCvm.ReturnUrl));
                HttpContext.Current.Response.Redirect(url, true);
            }
            else
            {
                CvmPages.StartPage.Redirect();
            }
        }

        protected void OnClickPassThroughBtn(object sender, EventArgs e)
        {
            DoRedirect();
        }
    }
}