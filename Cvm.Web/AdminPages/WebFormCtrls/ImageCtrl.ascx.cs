using System;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Files;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Napp.Web.Auto.Annotations;

namespace Cvm.Web.AdminPages.WebFormCtrls
{
    public partial class ImageCtrl : System.Web.UI.UserControl, ISimpleWebControl
    {
        private string _fieldContentId;

        public object ValueOfControl
        {
            get
            {
                if (!this.FileUpload1.HasFile)
                    return this.FileRefId;
                else
                {
                    //New file is uploaded, save as a new file reference,
                    //and return reference to new object
                    FileRef f = CvmFacade.File.CreateImageFileRef(this.FileUpload1.FileName);
                    string path = f.GetPathForFileSaving();
                    this.FileUpload1.SaveAs(path);
                    //For now we just the simplest possible scaling rule, everything must fit within 200x200
                    ImageUtility.ResizeImage(path, path, 200, 200, true);
                    return f.FileRefId;
                }
            }
            set
            {
                FileRefId = value as long?;
                
                if (FileRefId != null)
                {
                    FileRef file = QueryMgr.instance.GetFileRefById((long)FileRefId);
                    this.Image1.ImageUrl = file.GetAsUrl();
                    this.Image1.Visible = true;
                } 
                else
                {
                    this.Image1.Visible = false;
                }
            }
        }

        private long? FileRefId
        {
            get
            {
                return ViewState["refId"] as long?;
            }
            set
            {
                ViewState["refId"] = value;
            }
        }

        public string FieldContentId
        {
            get { return _fieldContentId; }
            set { this._fieldContentId = value; }
        }

        protected void OnClickClearBtn(object sender, EventArgs e)
        {
            this.FileRefId = null;
            this.Image1.ImageUrl = null;
            this.Image1.Visible = false;
        }
    }
}