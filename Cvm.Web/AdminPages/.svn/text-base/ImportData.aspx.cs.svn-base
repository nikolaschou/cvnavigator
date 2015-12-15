using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Cvm.Backend.FileStore;
using Cvm.Web.Backend.FileStore;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Napp.Common.MessageManager;

namespace Cvm.Web.AdminPages
{
    public partial class ImportData : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ImportDropDown.DataSource = GetImportFiles();
                this.ImportDropDown.DataBind();
            }
        }

        private String[] GetImportFiles()
        {
            string[] files = Directory.GetFileSystemEntries(HttpContext.Current.Server.MapPath(FolderStructure.ImportDataVirtual),"*.xls");
            string[] fileNames=new string[files.Length];
            int i = 0;
            foreach (String f in files)
            {
                fileNames[i++] = Path.GetFileName(f);
            }
            return fileNames;
        }

        protected void OnClickImport(object sender, EventArgs e)
        {
            string dataFile = ImportDropDown.SelectedItem.Text;

            CvmFacade.ImportData.ImportData(ContextObjectHelper.CurrentSysIdValOrFail,dataFile);
            MessageManager.Current.PostMessage("ImportData.ImportDone",dataFile);
        }
    }
}
