using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Backend.BusinessObject;
using Napp.Web.AdmControl;
using Napp.Web.AdminContentMgr;
using Napp.Web.Auto;
using Napp.Web.AutoFormExt;
using Napp.Web.Navigation;
using Napp.Web.WebForm;

namespace Cvm.Web.AdminPages.GenericCtrls
{
    public class EditListCtrl<T> : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl
        where T : IBusinessObject
    {
        /// <summary>
        /// Must be a source returning a list of objects, each being 
        /// suited to be viewed by AutoFormExt-controls.
        /// </summary>
        private ObjectSource _objectSource;

        private readonly AdmHyperLink AddNewBtn = new AdmHyperLink();
        private readonly String guid = new Guid().ToString();
        private IList<T> _list;
        public string OmitProperties;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }

        private IList<T> List
        {
            get
            {
                if (_list==null)
                {
                    _list = (IList<T>) _objectSource();
                }
                return _list;
            }
        }
        public void BuildForm()
        {
            PageLink link = PageNavigation.GetCurrentLink().IncludeExistingParms().SetMode(PageMode.AddListItem);
            AddNewBtn.PageLink = link;
            AddNewBtn.ContentId = "EditListCtrl.AddNew";
            AddNewBtn.SetupLink();

            
            this.Controls.Add(AddNewBtn);
            foreach (T item in List)
            {
                this.Controls.Add(new HtmlGenericControl("br"));
                this.Controls.Add(new HtmlGenericControl("hr"));
                HtmlControl div = new HtmlGenericControl("div");
                div.Attributes["id"] = guid;
                this.Controls.Add(div);
                AutoFormExt2 form = new AutoFormExt2();
                form.OmitProperties = this.OmitProperties;
                form.OmitProperties = "LastModified";
                div.Controls.Add(form);
                form.EditMode = AutoFormEditMode.View;
                form.IncludeDeleteLink = true;
                form.IncludeEditLink = true;

                //Must declare this variable in this inner scope
                //to make this delegate work.
                T itemCapture = item;
                form.ObjectSource = delegate { return itemCapture; };
                form.BuildForm();
            }
        }

        public void PopulateFront()
        {
            foreach (Control ctrl in this.Controls)
            {
                AutoFormExt2 form = ctrl as AutoFormExt2;
                if (form!=null)
                {
                    form.PopulateFront();
                }
            }
        }

        public void PopulateBack()
        {
            foreach (Control ctrl in this.Controls)
            {
                AutoFormExt2 form = ctrl as AutoFormExt2;
                if (form != null)
                {
                    form.PopulateBack();
                }
            }
        }

        public ObjectSource ObjectSource
        {
            get { return _objectSource; }
            set { _objectSource = value; }
        }


    }
}
