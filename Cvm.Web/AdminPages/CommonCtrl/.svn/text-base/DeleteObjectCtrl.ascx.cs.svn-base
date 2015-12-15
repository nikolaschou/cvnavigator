using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Backend.AutoDeleteForm;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Web.Navigation;
using Napp.Web.Navigation.Script;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class DeleteObjectCtrl : System.Web.UI.UserControl
    {

        private object _obj;


        protected void Page_Load(object sender, EventArgs e)
        {

        }


        ///<summary>
        ///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
        ///</summary>
        ///
        ///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
             CvmFacade.Security.CanDeleteVerify(ObjectGetter());
            this.AutoDeleteCtrl.ObjectToDelete = ObjectGetter;
            this.AutoDeleteCtrl.OnCancel = CloseWindow;
            this.AutoDeleteCtrl.OnDeletionDone = NotifyParentAndCloseWindow;
            
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.AutoDeleteCtrl.BuildControl();
            if (this.AutoDeleteCtrl.HasConflicts())
            {
                this.RefactorPanel.Visible = true;
                IEntity objectGetter = (IEntity)ObjectGetter();
                this.RefactorDropDown.EntityName = objectGetter.GetObjectType();
                this.RefactorDropDown.Activate();
            }
            this.AutoDeleteCtrl.BuildControl();
        }


        private void NotifyParentAndCloseWindow()
        {
            PopupWindowJs.instance.NotifyParentAndCloseWindow(this.Page, this.OnDeleteClientSideIDToCollapse);
        }


        private void CloseWindow()
        {
            PopupWindowJs.instance.CloseWindow(this.Page);
        }

        /// <summary>
        /// A client side javascript method defined in the parent window
        /// that will be called if the object is deleted.
        /// </summary>
        private string OnDeleteClientSideIDToCollapse
        {
            get
            {
                return QueryStringHelper.Instance.GetParmOrFail(QueryParmCvm.clientId);
            }
        }

        private Object ObjectGetter()
        {
            if (this._obj == null)
            {
                Type objectType = GetObjectType();
                int id = QueryStringHelper.Instance.GetParmIntOrFail(QueryParmCvm.id);
                this._obj = HibernateMgr.Current.LoadById(objectType, id);

            }
            return _obj;
        }

        private Type GetObjectType()
        {
            String type = QueryStringHelper.Instance.GetParmOrNull(QueryParmCvm.type);
            return TypeParser.ParseType(type);
        }

        protected void OnClickRefactorBtn(object sender, EventArgs e)
        {
            foreach (DeletionData obj in this.AutoDeleteCtrl.GetConflicts())
            {
                IBusinessObject targetObject = (IBusinessObject) obj.GetWrappedObject();
                //At this point we must figure out which property we need to re-assign
                //We can find the property with the given type.
                IBusinessObject oldVal = (IBusinessObject) this.ObjectGetter();
                long? selectedId = this.RefactorDropDown.SelectedLong;
                IBusinessObject newVal=null;
                if (selectedId!=null) newVal = (IBusinessObject) HibernateMgr.Current.LoadById(oldVal.GetObjectType(), (long)selectedId);
                targetObject.Reassign(oldVal, newVal);
                Utl.Msg.PostMessage("DeleteObjectCtrl.Reassigned ",oldVal,newVal);
            }
            PageNavigation.GotoCurrentPageAgainWithParms();
        }
    }
}