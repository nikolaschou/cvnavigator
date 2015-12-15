using System;
using System.Collections.Generic;
using System.Web.UI;
using Cvm.Backend.Business.Resources;
using Napp.Web.Auto;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class ViewCvCtrl : System.Web.UI.UserControl, IAutoBuildPopulateWithSourceCtrl
    {
        private ObjectSource objectSource;
        private Resource source;
        private Dictionary<string, Control> ids = new Dictionary<string, Control>();
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            CheckControlIds(this.Controls);
            base.OnPreRender(e);
        }

        private void CheckControlIds(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                string id = ctrl.ClientID;
                if (id != null)
                {
                    if (ids.ContainsKey(id)) throw new Exception("Already contains " + id + " for " + ids[id]);
                    ids.Add(id, ctrl);
                }
                CheckControlIds(ctrl.Controls);
            }
        }

        public void PopulateFront()
        {

        }

        public void PopulateBack()
        {
            //Will never be called on a view-ctrl.
        }

        public ObjectSource ObjectSource
        {
            get 
            { 
                return objectSource; 
            }
            set 
            { 
                objectSource = value; 
            }
        }

        protected Resource MyResource
        {
            get
            {
                if (source == null) 
                    source = (Resource) objectSource();
                return source;
            }
        }

        public void BuildForm()
        {
//            this.AutoForm1.ObjectSourceInstance = MyResource;
//            this.AutoForm2.ObjectSourceInstance = MyResource.GetResourceSkillsFetched();
            this.DataBind();
        }
    }
}