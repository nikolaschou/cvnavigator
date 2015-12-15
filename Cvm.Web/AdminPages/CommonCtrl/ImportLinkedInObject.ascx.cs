using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.Import;
using Cvm.Backend.Business.Resources;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Web.AdmControl;
using Napp.Web.Auto;
using Napp.Web.Navigation;
using NHibernate.Linq;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class ImportLinkedInObject : System.Web.UI.UserControl
    {

        protected bool HasAnyImportedObject = false;

        private string _importType;
        //Assign the type of import such as "Project". Used to parametrize content strings.
        public string ImportType
        {
            get { return _importType; }
            set { 
                _importType = value;
                this.AdmButton1.ContentId = "ImportLinkedInObject.ImportSelected" + value;
            }
        }
        protected IEnumerable<IImportedObject> _updatedObjects;
        protected IEnumerable<IImportedObject> _newObjects;

        protected override void OnLoad(EventArgs e)
        {
            this.ContainerCtrl1.ContentId = "ImportLinkedInObject.Headline" + ImportType;
        }

        public IEnumerable<IImportedObject> UpdatedObjects
        {
            set
            {
                if (value != null)
                {
                    this._updatedObjects = value.ToList();
                    this.UpdatedLinkedInObjectsRep.DataSource = this._updatedObjects;
                    this.UpdatedLinkedInObjectsRep.DataBind();
                }
            }
        }
        public IEnumerable<IImportedObject> NewObjects
        {
            set
            {
                if (value != null)
                {
                    this._newObjects = value.ToList();
                    this.NewLinkedInObjectsRep.DataSource = this._newObjects;
                    this.NewLinkedInObjectsRep.DataBind();
                }
            }
        }



        /// <summary>
        /// Sets a text by content ID within a control.
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="ctrl"></param>
        public void SetControlText(string contentId)
        {
            this.ContainerCtrl1.Controls.Add(new LiteralControl(Utl.Content(contentId)));
        }

        protected override void OnPreRender(EventArgs e)
        {
            var missingData = this._updatedObjects==null && this._newObjects==null;
            
            if (missingData)
            {
                    this.SetControlText("ImportLinkedInObject.LinkedInIncludesNoData"+this.ImportType);
                this.mainPanel.Visible = false;
            }
            else
            {
                bool anyUpdated = this._updatedObjects != null ? this._updatedObjects.Any() : false;
                bool anyNew = this._newObjects != null ? this._newObjects.Any() : false;
                if (!anyNew && !anyUpdated)
                {
                    this.mainPanel.Visible = false;
                    this.SetControlText("ImportLinkedInObject.LinkedInDataIsFullySynched" + this.ImportType);
                } else
                {
                    this.mainPanel.Visible = true;
                }
            }
        }



        protected void OnClickImportObjects(object sender, EventArgs e)
        {
            if (this._updatedObjects != null && this._updatedObjects.Any())
            {
                int count = HandleImports(this._updatedObjects, this.UpdatedLinkedInObjectsRep, false);
                if (count > 0) Utl.Msg.PostMessage("ImportCvDataCtrl.LinkedInSynched" + this.ImportType, count);
            }

            if (this._newObjects != null && this._newObjects.Any())
            {
                int count = HandleImports(this._newObjects, this.NewLinkedInObjectsRep, true);
                if (count > 0) Utl.Msg.PostMessage("ImportCvDataCtrl.LinkedInImported" + this.ImportType, count);
            }
            PageNavigation.GotoCurrentPageAgainWithParms();
        }


        private int HandleImports(IEnumerable<IImportedObject> ps, Repeater rep, bool areNew)
        {
            IImportedObject[] objects = ps.ToArray();
            var selectedIndexes = GetSelectedCheckBoxes(rep);
            selectedIndexes.ForEach(i => objects[i].DoImport());
            return selectedIndexes.Count();
        }

        /// <summary>
        /// Returns the indexes of the checkboxes selected.
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        private IEnumerable<int> GetSelectedCheckBoxes(Repeater rep)
        {
            int i = 0;
            foreach (var c in FindControlsOfType<CheckBox>(rep))
            {
                if (c.Checked) yield return i;
                i++;
            }

        }

        private IEnumerable<T> FindControlsOfType<T>(Control parent) where T : Control
        {
            foreach (Control child in parent.Controls)
            {
                if (child is T)
                {
                    yield return (T)child;
                }
                else if (child.Controls.Count > 0)
                {
                    foreach (T grandChild in FindControlsOfType<T>(child))
                    {
                        yield return grandChild;
                    }
                }
            }
        }


    }
}