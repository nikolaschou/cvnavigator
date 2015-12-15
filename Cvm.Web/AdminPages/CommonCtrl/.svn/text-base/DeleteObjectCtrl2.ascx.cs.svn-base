using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Navigation;
using Napp.Backend.AutoDeleteForm;
using Napp.Backend.AutoDeleteForm.Annotation;
using Napp.Backend.Business.Common;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.VeryBasic.GenericDelegates;
using Napp.Web.Navigation;
using Napp.Web.Navigation.Script;
using Nshop.Backend.Business.GenericDelegates;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class DeleteObjectCtrl2 : System.Web.UI.UserControl
    {

        private IBusinessObject _obj;
        private DeletionData _delData;
        private ObjectGetter<Object> objectGetter;
        private List<DeletionData> _list;
        private const String Indent = "&nbsp;&nbsp;&nbsp;";

        protected DeletionData MyDeletionData
        {
            get
            {
                if (this._delData == null)
                {
                    object obj = objectGetter();
                    this._delData = DeletionData.CreateDeletionData(obj);
                }
                return _delData;
            }
        }

        protected List<DeletionData> MyDeletionDataList
        {
            get
            {
                if (this._list == null)
                {
                    this._list = AutoDeleteMgr.instance.BuildDeletionDataList(MyDeletionData);
                    this._list.RemoveAt(0);
                }
                return _list;
            }
        }

        /// <summary>
        /// The object to be deleted.
        /// </summary>
        protected IBusinessObject MyDeletionObject
        {
            get
            {
                if (this._obj == null)
                {
                    _obj = (IBusinessObject)objectGetter();
                }
                return _obj;
            }
        }

        /// <summary>
        /// Returns the objects making conflicts in the deletion.
        /// </summary>
        /// <returns></returns>
        public List<DeletionData> GetConflicts()
        {
            return _delData.GetRelatedObjects(DeletionBehaviorType.CancelDeletionIfExists);
        }

        /// <summary>
        /// Determines whether there are any conflicts.
        /// </summary>
        /// <returns></returns>
        private bool HasConflicts()
        {
            return _delData.HasAnyConflicts();
        }
        /// <summary>
        /// Assign the object to be deleted here.
        /// </summary>
        public ObjectGetter<Object> ObjectToDelete
        {
            set
            {
                this.objectGetter = value;
            }
        }

        public void Activate()
        {
            this.DataBind();
            bool hasConflict = MyDeletionData.HasAnyConflicts();
            this.ShowRefactor(hasConflict);
            if (hasConflict)
            {
                IList items = HibernateMgr.Current.GetAllByTypeCached(MyDeletionObject.GetObjectTypeReal());
                ArrayList list = new ArrayList(items);
                list.Sort(new MyComparer());
                this.RefactorDropDown2.DataSource = list;
                this.RefactorDropDown2.DataBind();
            }
        }


        protected void OnClickRefactorBtn(object sender, EventArgs e)
        {
            int counter = 0;
            IBusinessObject newVal = null;
            long? selectedId = this.RefactorDropDown2.GetSelectedInt();
            IBusinessObject oldVal = (IBusinessObject)this.objectGetter();
            foreach (DeletionData obj in this.MyDeletionData.GetConflicts())
            {
                IBusinessObject targetObject = (IBusinessObject)obj.GetWrappedObject();
                //At this point we must figure out which property we need to re-assign
                //We can find the property with the given type.
                if (newVal==null && selectedId != null) newVal = (IBusinessObject)HibernateMgr.Current.LoadById(oldVal.GetObjectType(), (long)selectedId);
                targetObject.Reassign(oldVal, newVal);
                counter++;
            }
            Utl.Msg.PostMessage("DeleteObjectCtrl.Reassigned ", counter, oldVal, newVal);
            this.Panel1.Visible = false;
            this.ShowRefactor(false);
            //As certain errors here will be db-errors, we want to find them immediately.
            HibernateMgr.Current.CommitAndReopenTransaction();
        }

        private void ShowRefactor(bool show)
        {
            this.RefactorPanel.Visible = show;
            this.DeleteButton.Visible = !show;
            this.DeleteLiteral.Visible = !show;
        }

        protected string GetIndent(int recurseDepth)
        {
            StringBuilder sb = new StringBuilder(recurseDepth * Indent.Length);
            for (int i = 0; i < recurseDepth; i++)
            {
                sb.Append(Indent);
            }
            return sb.ToString();
        }

        protected void OnClickDeleteBtn(object sender, EventArgs e)
        {
            DeleteMyObject();
        }

        private void DeleteMyObject()
        {
            //Pick up title before deleting.
            string title = MyDeletionObject.ExtendedObjectTitle;
            AutoDeleteMgr.instance.DeleteAll(MyDeletionData);
            Utl.Msg.PostMessage("DeleteObjectCtrl.DeletionDone", title);
            Inactivate();
            if (OnDeletionDone != null) OnDeletionDone();
        }

        private void Inactivate()
        {
            this.Rep1.Controls.Clear();
            this.RefactorDropDown2.Controls.Clear();
        }

        protected void OnClickCancelBtn(object sender, EventArgs e)
        {
            Inactivate();
            if (OnCancel != null) OnCancel();
        }

        /// <summary>
        /// Assign a return handler in case of cancel.
        /// </summary>
        public NoArgReturnHandler OnCancel;

        /// <summary>
        /// Assign a return handler in case of deletion done.
        /// </summary>
        public NoArgReturnHandler OnDeletionDone;
    }

    public class MyComparer : IComparer
    {
        public int Compare(object a, object b)
        {
            return (a as IBusinessObject).ExtendedObjectTitle.CompareTo((b as IBusinessObject).ExtendedObjectTitle);
        }
    }
}