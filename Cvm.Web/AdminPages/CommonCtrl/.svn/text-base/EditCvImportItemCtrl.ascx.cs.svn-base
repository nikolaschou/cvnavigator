using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.CvImport;
using Napp.Web.AdminContentMgr;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class EditCvImportItemCtrl : System.Web.UI.UserControl
    {
        private const int MinusOne = -1;
        private CvImportItem item;
        /// <summary>
        /// Assign an import-item to be edited by this control.
        /// </summary>
        public CvImportItem ImportItem
        {
            set
            {
                item = value;
            }
            get
            {
                return item;
            }
        }

        public bool HideIfCompleted
        {
            get; set;
        }

        private void EnableControls(bool b)
        {
            this.DoImportCheckBox.Enabled = b;
            this.FirstNameTextBox.Enabled = b;
            this.LastNameTextBox.Enabled = b;
            this.EmailTextBox.Enabled = b;
            
        }

        /// <summary>
        /// To be called when import is completed for this item.
        /// </summary>
        private void ShowErrorMessage(String errMsg)
        {
            ShowMessage(errMsg, true);
        }

        private void ShowMessage(String msg, bool isError)
        {
            this.MessageLiteral.Text += String.Format("<span colspan='10' style='color:{0}'>{1} </span>",(isError?"red":"green"),msg);
        }

        private void ClearMessagesAndHideDropDown()
        {
            this.MessageLiteral.Text = "";
            this.ResourceMessage.Text = "";
            this.ResourceDropDown.Visible = false;
        }

        public void PopulateFront()
        {
            this.DoImportCheckBox.Checked = item.DoImport;
            this.FirstNameTextBox.Text = item.ResourceFirstName;
            this.LastNameTextBox.Text = item.ResourceLastName;
            this.EmailTextBox.Text = item.ResourceEmail;
            this.FileNameLabel.Text = item.OriginalFileName;
        }

        /// <summary>
        /// Saves user-input back to the underlying CvImportItem object.
        /// </summary>
        public void PopulateBack()
        {
            item.DoImport=this.DoImportCheckBox.Checked;
            item.ResourceFirstName=this.FirstNameTextBox.Text;
            item.ResourceLastName=this.LastNameTextBox.Text;
            item.ResourceEmail=this.EmailTextBox.Text;
            
            if (this.ResourceDropDown.Items.Count>0)
            {
                int selectedId = int.Parse(this.ResourceDropDown.SelectedValue);
                if (selectedId==MinusOne)
                {
                    item.CreateNewResourceForced = true;
                    item.ImportIntoResourceId = null;
                } else
                {
                    item.CreateNewResourceForced = false;
                    item.ImportIntoResourceId = selectedId;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool DoImport
        {
            set
            {
                this.DoImportCheckBox.Checked = value;
            }
            get
            {
                return this.DoImportCheckBox.Checked;
            }
        }

        public void SetStatus(CvImportItemStatus status)
        {
            this.ClearMessagesAndHideDropDown();
            
            
            if (status.HasErrors())
            {
                this.StatusLabel.Text = AdminContentMgr.instance.GetContent("EditCvImportItemCtrl.NotOk");
                this.StatusLabel.CssClass = "failureTxt";
                int counter = 1;
                foreach(String errMsg in status.GetErrorMessages())
                {
                    this.ShowErrorMessage(counter+". "+errMsg);
                    counter++;
                }
            } else
            {
                List<Resource> resources = status.GetPotentialResources();
                if (resources.Count > 0)
                {
                    String message = AdminContentMgr.instance.GetContent("EditCvImportItemCtrl.ExistingResources");
                    this.ResourceMessage.Text = message;
                    this.ResourceDropDown.DataSource = resources;
                    this.ResourceDropDown.DataBind();
                    String createNewText = AdminContentMgr.instance.GetContent("EditCvImportItemCtrl.CreateNew");
                    this.ResourceDropDown.Items.Add(new ListItem(createNewText,MinusOne.ToString()));
                    this.ResourceDropDown.Visible = true;
                    this.DoImportCheckBox.Checked = false;
                } else
                {
                    this.StatusLabel.Text = AdminContentMgr.instance.GetContent("EditCvImportItemCtrl.SuccessStatus");
                    this.StatusLabel.CssClass = "successTxt";
                    foreach(String s in status.GetCompletionMessages()) this.ShowMessage(s,false); 
                    EnableControls(false);
                    this.Visible = this.HideIfCompleted;
                    IsCompleted = true;
                }
            }
        }


        public bool IsCompleted
        {
            get
            {
                bool? b = ViewState["IsCmpl"] as bool?;
                return b ?? false;
            }
            set
            {
                ViewState["IsCmpl"] = value;
            }
        }
    }
}