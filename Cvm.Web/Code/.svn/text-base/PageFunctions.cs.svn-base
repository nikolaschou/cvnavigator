using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cvm.Backend.Business.Files;
using Cvm.Backend.Business.Resources;
using Napp.Web.AdminContentMgr;

namespace Cvm.Web.Code
{
    public class PageFunctions
    {
        /// <summary>
        /// Returns a link for the attached (that is, imported) CV document (PDF, WORD etc.) for the given resource.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static string GetCvLink(Resource res)
        {
            FileRef cvFile = res.RelatedCvFileRefObjWithFileCheck();
            if (cvFile != null)
            {
                string content = AdminContentMgr.instance.GetContentWithHelpTextAsHtml("EditCv.CvDocumentExists");
                return String.Format("<a class='stdLink' href='{0}' target='_blank'>{1}</a>", cvFile.GetAsPageLink().GetLinkAsHref(), content);
            }
            else return "";
        }
    }
}
