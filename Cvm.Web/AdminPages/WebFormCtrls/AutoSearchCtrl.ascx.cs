using System;
using System.Collections;
using Cvm.Backend.Business.Meta;
using Napp.Backend.Business.Common;
using Napp.Backend.BusinessObject;
using Napp.Backend.Context.Providers;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.Web.AdminContentMgr;
using Napp.Web.Auto;
using Napp.Web.Navigation;

namespace Nshop.Web.AdminPages.WebFormCtrls
{
    public partial class AutoSearchCtrl : System.Web.UI.UserControl, IAutoSearchCtrl
    {

        private QueryStringHelper helper = QueryStringHelper.Instance;
        
        public event ObjectFoundEventHandler OnObjectIdFound;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }



        protected void OnClickSearchBtn(object sender, EventArgs e)
        {
            object searchObj = this.autoSearchCtrl.GetSearchObject();
            IList res = HibernateMgr.Current.SearchByExample(searchObj);
            this.SearchGrid.DataKeyNames = new string[] { IBusinessObject.IdFieldName };
            this.SearchGrid.DataSource = res;
            this.SearchGrid.DataBind();

        }
        protected void SearchListSelectChanged(object sender, EventArgs e)
        {
            long objectId = (long) SearchGrid.SelectedValue;
            this.ObjectIdFoundUtil(objectId);
        }

        public Type ObjectTypeToSearch
        {
            set
            {
                this.autoSearchCtrl.ObjectType = value;
                ContainerCtrl1.ContentId =
                    AdminContentMgr.instance.GetContent("AutoSearchCtrl.SearchObject",
                                            AdminContentMgr.instance.GetContent("Type."+value.Name));
            }
            get
            {
                return this.autoSearchCtrl.ObjectType;
            }
        }

        public void StartSearch()
        {
            this.autoSearchCtrl.BuildForm();
        }



        protected void OnClickLookupById(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.ObjectIdInput.Text))
            {
                MessageManager.Current.PostMessage("AutoSearchCtrl.LookupIdMissing");
            }else
            {
                int id = int.Parse(this.ObjectIdInput.Text);
                this.ObjectIdFoundUtil(id);
            }
        }

        /// <summary>
        /// Internal helper method called 
        /// </summary>
        /// <param name="id"></param>
        private void ObjectIdFoundUtil(long id)
        {
            this.OnObjectIdFound(this.autoSearchCtrl.ObjectType,id);
        }


    }
}