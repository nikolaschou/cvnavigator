using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Web.Auto;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class DiscProfileCtrl : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl
    {

        private Resource MyResource { get { return (Resource) this.ObjectSource(); } }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            this.LanguageDropDown.EnumType = typeof(DiscLanguageEnum);
        }

        public void PopulateFront()
        {
            if (MyResource.HasDiscProfile())
            {
                this.DiscProfileResultCtrl1.MyResource = this.MyResource;
                this.DiscProfileResultCtrl1.DataBind();
                this.Panel1.Visible = false;
                this.Panel2.Visible = true;
            }
        }



        public void PopulateBack()
        {
        }

        public ObjectSource ObjectSource { get; set; }

        public void BuildForm()
        {

        }


        protected void OnClickStartWizard(object sender, EventArgs e)
        {
            string url = GetUrlStartWizard();
            this.MyFrame.Attributes["src"] = url;
            this.MyFrame.Attributes["style"] = "display:block";
            this.LanguageDropDown.Visible = false;
            this.StartWizardBtn.Visible = false;
            this.CancelWizardBtn.Visible = true;

            //Example: http://www.adfaerdsprofil.dk/CVNav/CvNav_NyProfil.asp?P1=614142b5-ef37-4b59-bbe8-ff6a213a32e7&P2=Flemming&P3=jaller@mail.dk&P4=M&P5=48301244&P6=en
        }

        protected string GetUrlStartWizard()
        {
            var url = WebConfigMgr.EstimateUrlStartPage;
            var resId = HttpUtility.UrlEncode(MyResource.ResourceIdEncoded);
            var name = HttpUtility.UrlEncode(MyResource.FullName);
            var gender = MyResource.GenderIdEnum==GenderEnum.Male?"M":"K";
            String lang = TranslateCode((DiscLanguageEnum) this.LanguageDropDown.GetSelectedInt());
            var phone = HttpUtility.UrlEncode(MyResource.RelatedUserObjObj.Phone);
            var email = HttpUtility.UrlEncode(MyResource.Email);
            string replyUrl = HttpUtility.UrlEncode(CvmPages.DoneHtml.GetLinkAsFullHref());
            string overrideCss = HttpUtility.UrlEncode(CvmPages.OverrideEstimateCss.GetLinkAsFullHref());
            return String.Format("{0}?P1={1}&P2={2}&P3={3}&P4={4}&P5={5}&P6={6}&replyUrl={7}&overrideCss={8}", 
                                 url, resId,
                                 name, email, gender,
                                 phone,lang,replyUrl,overrideCss);
        }

        private String TranslateCode(DiscLanguageEnum lang)
        {
            switch(lang)
            {
                case DiscLanguageEnum.Danish:
                    return "da";
                case DiscLanguageEnum.English:
                    return "en";
                case DiscLanguageEnum.Norwish:
                    return "no";
                case DiscLanguageEnum.Sweedish:
                    return "sv";
                default:
                    throw new Exception("Not exptected.");
            }
        }

        protected void OnChangeDiscProfileResult(object sender, EventArgs e)
        {
            string discResult = this.DiscProfileResult.Value;
            discResult = discResult.TrimStart('?').TrimEnd('&');
            MyResource.SetDiscResult(discResult);
            PopulateFront();
        }

        protected void OnClickCancelWizard(object sender, EventArgs e)
        {
            PageNavigation.GotoCurrentPageAgainWithParms();
        }

        protected void OnClickRemoveDiscProfile(object sender, EventArgs e)
        {
            MyResource.SetDiscResult(null);
            PageNavigation.GotoCurrentPageAgainWithParms();
        }
    }

    public enum DiscLanguageEnum
    {
        English=1,
        Danish=2,
        Sweedish=4,
        Norwish=8
    }
}