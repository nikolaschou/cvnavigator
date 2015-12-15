using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Web.Code;
using Napp.Common.MessageManager;
using Napp.Web.DialogCtrl;

namespace Cvm.Web.AdminPages
{
    public partial class TestingPage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckValues();

            //            AdminPages_AdminMasterPage.HideValidationMessages();
        }

        private void CheckValues()
        {
            String text=this.TextBox3.Text;
            bool b1 = radio1.Checked;
            bool b2 = this.CheckBox1.Checked;
        }


  
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.IsInvokingDialog)
            {
                MyTextBox dialog = Helper.SetupDialog(new DialogInvokerViewState(this.ViewState));
            }

            CheckValues();
        }


        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            CheckValues();
        }

        private bool IsInvokingDialog
        {
            get
            {
                return ((this.ViewState["IsInvokingDialog"] as bool?) ?? false);
            }
            set
            {
                this.ViewState["IsInvokingDialog"] = value;
            }
        }

        protected void OnClick_TestBtn(object sender, EventArgs e)
        {
            this.IsInvokingDialog = true;
            MyTextBox dialog = Helper.SetupDialog(new DialogInvokerViewState(this.ViewState));
            this.TestTxtInput.Text += "X";
        }


        protected void OnClickSwitchBtn(object sender, EventArgs e)
        {
            this.CheckBox1.Enabled = !this.CheckBox1.Enabled;
            this.checkBoxPanel.Visible = !this.checkBoxPanel.Visible;
        }

        protected void OnCheckBoxChange(object sender, EventArgs e)
        {
            //Do nothing
        }
    }


    internal class Helper
    {
        public static MyTextBox SetupDialog(DialogInvokerViewState invokerViewState)
        {
            MyTextBox ctrl = new MyTextBox();
            ctrl.AutoPostBack = true;
            ctrl.ID = "MyTextBox1";
            ctrl.DialogConcluded += OnDialogConcluded;
            DialogController.RequestInstance.InvokeDialog(ctrl,invokerViewState);
            return ctrl;
        }

        private static void OnDialogConcluded(Object concl)
        {
            MessageManager.Current.PostMessage("TestingPage.FoundConclusion", (concl as MyDialogConlusion).Text);
        }


    }
    internal class MyTextBox : TextBox, IDialogCtrl
    {
        public static int counter = 0;
        public event DialogConcludedHandler DialogConcluded;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            if (this.IsInvokingDialog)
            {
                Helper.SetupDialog(new DialogInvokerViewState(this.ViewState));
            }
        }
        public string DialogTitle
        {
            get { return "Subdialog " + (counter); }
        }
        private bool IsInvokingDialog
        {
            get
            {
                return ((this.ViewState["IsInvokingDialog"] as bool?) ?? false);
            }
            set
            {
                this.ViewState["IsInvokingDialog"] = value;
            }
        }


        public MyTextBox()
        {
            this.TextChanged += MyTextChanged;
        }

        private void MyTextChanged(object sender, EventArgs e)
        {
            if (this.Text.Equals("xxx"+counter))
            {
                DialogConcluded(new MyDialogConlusion(this.Text));
            }
            else if (this.Text.Equals("yyy" + counter))
            {
                counter++;
                IsInvokingDialog = true;
                Helper.SetupDialog(new DialogInvokerViewState(this.ViewState));
            }
            else
            {
                MessageManager.Current.PostMessage("TestingPage.NotXxx"+counter);
            }
        }
    }

    internal class MyDialogConlusion : IDialogConclusion
    {
        public string Text;

        public MyDialogConlusion(string text)
        {
            this.Text = text;
        }
    }
}
