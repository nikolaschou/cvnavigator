using System;
using System.Collections.Generic;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.VeryBasic.GenericDelegates;
using Napp.Web.AdminContentMgr;
using Napp.Web.Auto;
using Napp.Web.Navigation;

namespace Nshop.Web.AdminPages.CommonPages
{
    public partial class EditObject : System.Web.UI.Page
    {
//        private EditAllFacade facade = new EditAllFacade();
        private QueryStringHelperCvm query = QueryStringHelperCvm.Instance;
        private IBusinessObject bussObj;
        private string DefaultFilterText = Utl.Content("EditObject.FilterDefault");

        /// <summary>
        /// Returns the list filter currently entered or empty string if nothing has been entered.
        /// </summary>
        protected String ListFilter
        {
            get
            {
                bool isDefault = this.DefaultFilterText.Equals(this.FilterTextBox.Text);

                if (isDefault)
                {
                    //Check query string
                    return QueryStringHelper.Instance.GetParmOrNull(QueryParmCvm.filter) ?? "";
                } 
                else
                {
                    return this.FilterTextBox.Text;
                }
            }
        }

        protected String GetContent(string s)
        {
            return AdminContentMgr.instance.GetContent("EditObject." + query.GetTypeOrFail()+ s);
        }

        protected String GetLink(IBusinessObject o)
        {
            return PageNavigation.GetCurrentLink().IncludeParm(QueryParmCvm.type).SetParm(QueryParmCvm.id, o.Idfr.IdfrAsLong()).GetLinkOnServer();
        }

        protected override void OnPreInit(EventArgs e)
        {
            String type = query.GetTypeOrFail();

            bool filterSysId = !type.Equals("SysRoot");
            MasterPageHelper.Instance.OnPageInit(filterSysId);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int? id = query.GetIdOrNull();

            if (!IsPostBack)
            {
                if (id != null) EditObjCtrl.PopulateFront();
                this.ShowDeleteCtrl(false);
                if (QueryStringHelper.Instance.HasParm(QueryParmCvm.filter.ToString())) 
                    this.FilterTextBox.Text = QueryStringHelper.Instance.GetParmOrFail(QueryParmCvm.filter);
                else 
                    this.FilterTextBox.Text = DefaultFilterText;
            }
            
            if (id == null) 
                this.DeleteLinkBtn.Visible = false;

            PopulateList();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            this.EditObjCtrl.ObjectGetter = new ObjectSource(GetObject);
            string type = query.GetTypeOrFail();
            this.EditObjCtrl.ObjType = type;
            this.EditObjCtrl.OnSaveHandler = new GenericEventHandler(OnSave);
            this.EditObjCtrl.OnCreateHandler = new GenericEventHandler(OnCreate);
            CreateLink.PageLink = PageNavigation.GetCurrentLink().IncludeParm(QueryParmCvm.type).SetMode(PageMode.New);
            
            int? id = query.GetIdOrNull();
            
            if (id != null || IsNewMode())
            {
                EditObjCtrl.BuildForm();
            }
            
            //Set source in case deletion is started.
            this.DeleteObjCtrl.ObjectToDelete = GetObject;
            this.DeleteObjCtrl.OnCancel = OnDeletionCancelled;
            this.DeleteObjCtrl.OnDeletionDone = OnDeletionDone;

            this.CreateLink.Text = Utl.Content("EditObject.CreateNew",Utl.Content("ObjectTypes." + type));
        }

        private bool IsNewMode()
        {
            return query.GetMode() == PageMode.New;
        }

        private void OnDeletionDone()
        {
            PageNavigation.GetCurrentLink().IncludeParm(QueryParmCvm.type).IncludeParm(QueryParmCvm.filter).Redirect();
        }

        private void OnDeletionCancelled()
        {
            this.ShowDeleteCtrl(false);
        }

        private void OnCreate()
        {
            PageNavigation.GetCurrentLink().IncludeParm(QueryParmCvm.type).SetParm(QueryParmCvm.id, GetObject().Idfr.IdfrAsLong()).Redirect();
        }

        private void OnSave()
        {
            PopulateList();
        }

        private void PopulateList()
        {
            Type type = TypeParser.ParseType(query.GetTypeOrFail());
            List<IBusinessObject> all = HibernateMgr.Current.GetAllByTypeCached<IBusinessObject>(type);
            string listFilter = ListFilter.ToLower();
            if (!String.IsNullOrEmpty(listFilter)) 
                all.RemoveAll(a => a.ExtendedObjectTitle.ToLower().IndexOf(listFilter) == -1);
            
            all.Sort(delegate(IBusinessObject a, IBusinessObject b)
                { return String.Compare(a.ExtendedObjectTitle, b.ExtendedObjectTitle); });
            
            this.rep.DataSource = all;
            this.rep.DataBind();
        }

        private IBusinessObject GetObject()
        {
            if (this.bussObj == null)
            {
                string type = query.GetTypeOrFail();
                Type typeObj = TypeParser.ParseType(type);
                if (query.GetMode() == PageMode.New)
                {
                    bussObj = (IBusinessObject)typeObj.GetConstructor(new Type[0]).Invoke(new object[0]);
                }
                else
                {
                    bussObj = (IBusinessObject)HibernateMgr.Current.LoadById(typeObj, query.GetIdOrFail());
                }
            }

            return bussObj;
        }

        protected void OnClickShowUsages(object sender, EventArgs e)
        {
            Utl.Msg.PostMessage("Standard.NotImplementedYet");
        }

        protected void OnClickDeleteObject(object sender, EventArgs e)
        {
            if (ConfigMgr.instance.AllowObjectDeletionInGeneral || Utl.HasGlobalRole(GlobalRoleEnum.GlobalAdmin))
            {
                this.DeleteObjCtrl.Activate();
                ShowDeleteCtrl(true);
            } 
            else
            {
                Utl.Msg.PostMessage("EditObject.NoPermissionToDeleteObject");
            }
        }

        private void ShowDeleteCtrl(bool show)
        {
            this.DeleteLinkBtn.Visible = !show;
            this.EditObjCtrl.Visible = !show;
            this.DeleteObjCtrl.Visible = show;

            //We cannot delete if we are not global admin or deletion is enabled for the current site
            if (!Utl.HasGlobalRole(GlobalRoleEnum.GlobalAdmin) || ConfigMgr.instance.AllowObjectDeletionInGeneral) 
                this.DeleteLinkBtn.Visible = false;
        }

        protected void OnChangeFilterText(object sender, EventArgs e)
        {
            this.rep.Controls.Clear();
            this.PopulateList();
        }
    }
}