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
using Cvm.Web.Facade;
using log4net;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Code
{
    public class HttpModuleSessionHandler : IHttpModule
    {
        private static ILog log = LogManager.GetLogger(typeof (HttpModuleSessionHandler));
        public void Init(HttpApplication context)
        {
            context.PostRequestHandlerExecute += OnPostRequestHandler;
            context.PreRequestHandlerExecute += OnPreRequest;
            context.PostRequestHandlerExecute += OnPostRequest;
        }

        private void OnPostRequest(object sender, EventArgs e)
        {
            if (isAspx()) log.Info("*********************** REQUEST ENDED "+HttpContext.Current.Request.Path+" ********************");
        }

        private bool isAspx()
        {
            return HttpContext.Current.Request.Path.ToLower().EndsWith("aspx");
        }


        private void OnPreRequest(object sender, EventArgs e)
        {
            if (isAspx()) log.Info("*********************** REQUEST START " + HttpContext.Current.Request.Path + " ********************");
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Determines which resources to handle.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool HandleRequest(HttpRequest request)
        {
            return request.FilePath.ToLower().EndsWith("aspx");
        }
         
        private void OnPostRequestHandler(object sender, EventArgs e)
        {

            //ContextObjectHelper.ContextSysId.PersistSession();
        }

    }
}
