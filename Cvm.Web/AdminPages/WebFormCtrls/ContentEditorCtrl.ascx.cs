using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Contents;
using Napp.Backend.Contents;
using Napp.Web.Auto;
using Napp.Web.Auto.Annotations;

namespace Nshop.Web.AdminPages.WebFormCtrls
{
    public partial class ContentEditorCtrl : System.Web.UI.UserControl, ICompositeWebForm
    {
        private ObjectSource objectSource;
        private bool didBuildControls = false;

        protected IContentField MyContentField
        {
            get
            {
                return objectSource() as IContentField;
            }
        }

        protected int GetNoColumnsForField()
        {
            return 50;
        }

        protected int GetNoRowsForField()
        {
            return 2;
        }

        protected TextBoxMode GetTextMode()
        {
            return TextBoxMode.MultiLine;
        }

        public ObjectSource ObjectSource
        {
            set { objectSource = value; }
        }

        public void BuildForm()
        {
            if (!didBuildControls)
            {
                this.Rep1.Controls.Clear();
                ILanguage[] langs = MyContentField.GetLanguages();
                Array.Sort(langs);
                Rep1.DataSource = langs;
                Rep1.DataBind();
                didBuildControls = true;                
            } else
            {
                throw new Exception("The control tree was built twice.");
            }
        }

        public void PopulateBack()
        {
            ControlCollection ctrlColl = Rep1.Controls;
            PopulateBackRecurseUtil(ctrlColl);
        }

        private void PopulateBackRecurseUtil(ControlCollection ctrlColl)
        {
            foreach (Control ctrl in ctrlColl)
            {
                if (ctrl is ContentFieldTextBox)
                {
                    ((ContentFieldTextBox)ctrl).PopulateBack();
                }
                if (ctrl.Controls!=null && ctrl.Controls.Count>0)
                {
                    PopulateBackRecurseUtil(ctrl.Controls);
                }
            }
        }

        public void PopulateFront()
        {
            //For now this control will implicitly populate the front as part of building the form.
        }
    }
}