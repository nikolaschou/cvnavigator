using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Public
{
    public partial class Profiles : System.Web.UI.Page
    {
        

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
            
        }


        protected void OnClickBookMeeting(object sender, EventArgs e)
        {
            string id=((Button) sender).CommandArgument;
            int idInt = int.Parse(id);
            var result = (from res in Hiber.Q<Resource>() where res.ResourceId==idInt select new {FirstName=res.FirstName, LastName=res.LastName}).First();
            string fullName = result.FirstName+" "+result.LastName;
            CvmFacade.Mail.SendContactRequestedMail(this.EmailInput.Text, this.EmailInput.Text,idInt,fullName);
            Utl.Msg.PostMessage("Profiles.InvitationSentByMail",fullName);
        }

        protected IEnumerable<Resource> GetResources()
        {
            
            IOrderedQueryable<Resource> q = from res in Hiber.Q<Resource>() 
                                            where (res.FirstName.Contains(this.SearchInput.Text) || res.LastName.Contains(this.SearchInput.Text))
                                            orderby res.FirstName , res.LastName select res;
            return q;
        }

        protected void OnChangeResourceNameSearch(object sender, EventArgs e)
        {
            this.DataBind();
        }

        protected string GetImageUrl(Resource resource)
        {

            return resource.RelatedPhotoFileRefObj!=null?resource.RelatedPhotoFileRefObj.GetAsUrl():"";
        }

    }
}