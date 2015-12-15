using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.Localization;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.ObjectNavigation;
using log4net.Config;
using Napp.Backend.AutoDeleteForm;
using Napp.Backend.Business;
using Napp.Backend.Business.Common;
using Napp.Backend.BusinessObject;
using Napp.Backend.Hibernate;
using Napp.Backend.Hibernate.DataFetcher;
using Napp.Common.MessageManager;
using Napp.VeryBasic.GenericDelegates;
using Napp.Web.AdminContentMgr;
using Napp.Web.AutoFormDialog;
using Napp.Web.ContentManagerExprBuilder;
using Napp.Web.Navigation;
using Napp.Web.WebForm;

namespace Cvm.Web
{
    public class Global : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //It doesn't work properly
            OnError(sender, e);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("");
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //Set up log4n
            FileInfo log4nFile = new FileInfo(this.Server.MapPath("~/log4j.config"));
            if (!log4nFile.Exists)
            {
                throw new NappGenericException("Config-file for log4net was not found:" + log4nFile);
            }

            XmlConfigurator.Configure(log4nFile);

            AdminContentMgrPluginConfig.DefaultLanguage = SiteLanguageConst.DefaultLanguage;
            AdminContentMgrPluginConfig.LanguageGetter = Utl.GetLanguage;
            AdminContentMgrPluginConfig.EditContentAspxUrl = "../AdminPages/Content/EditActiveCms.aspx";
            
            AutoPluginConfig.ContentManager = AdminContentMgr.instance;
            AutoPluginConfig.EntityFetcher = new HibernateDataFetcher();
            AutoPluginConfig.ObjectTitleGetter = BusinessObjectHelper.Instance.GetObjectTitle;
            AutoPluginConfig.ConfigMgr = ConfigMgrImpl.Instance;
            
            //Configure ExtControls plugin
            Napp.Web.ExtControls.Services.ExtControlMaster.Content = AdminContentMgr.instance;
            Napp.Web.ExtControls.Services.ExtControlMaster.HELP_ICON = "../images/master/help.gif";

            //Configure AdmControls plugin
            Napp.Web.AdmControl.AdmControlPluginConfig.ContentManager = AdminContentMgr.instance;
            Napp.Web.AdmControl.AdmControlPluginConfig.entityFetcher = new HibernateDataFetcher();

            //Configure Navigation plugin
            NavigationPluginConfig.ContentMgr = AdminContentMgr.instance;
            NappBackendBusinessPluginConfig.instance.ContentMgr = AdminContentMgr.instance;

            //Configure ContentManagerExprBuilderPlugin
            ContentManagerExprBuilderPluginConfig.ContentManager = AdminContentMgr.instance;

            MessageManager.InitMessageManager(MyContentManagerFactory);

            //Configure AutoFormDialog control
            AutoFormDialoPluginConfig.ContentMgr = AdminContentMgr.instance;

            //Configure backend code
            Cvm.Backend.Business.ModuleConfig.MakePopupLinkDelegate = ObjectLinkHelper.Instance.MakePopupLinkDelegate;
            Cvm.Backend.Business.ModuleConfig.MakeSearchLinkDelegate = ObjectLinkHelper.Instance.MakeSearchLinkDelegate;

            InitializeAutoDeleteCtrl();

            //Configure plugins Hibernate module
            Cvm.Backend.Business.ModuleConfig.SetupPersistenceLayerBasics(ContextObjectHelper.GetCurrentSysIdValOrUndefined);
            AutoPluginConfig.SysIdGetter = ContextObjectHelper.GetCurrentSysIdValOrUndefined;

            //Initialize tables

            Tables.RegisterAll();

            //Handle SiteMap 
            SiteMap.SiteMapResolve += SiteMapHelper.ResolveCurrentNode;
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private void OnError(object sender, EventArgs e2)
        {
            //OnErrorOld();
            /*var e = Server.GetLastError();
            ErrorLogger.LogError(e);
            Server.ClearError();
            var stack = e.StackTrace;
            var inner = e.InnerException;
            while(inner!=null)
            {
                stack += inner.StackTrace;
                inner = inner.InnerException;
            }
            Response.Write("<div class='dialog open' title=''><xmp>"+stack+"</xmp></div>");*/
        }

        private void OnErrorOld()
        {
            try
            {
                Response.Clear();
                Server.Transfer("~/Public/Error.aspx", false);
                ErrorLogger.LogError(Server.GetLastError());
                Server.ClearError();
            }
            catch (Exception e2)
            {
                string deployInfo = MasterPageHelper.Instance.GetDeployInfo();
                if (!String.IsNullOrEmpty(deployInfo))
                {
                    HttpContext.Current.Response.Write(AdminContentMgr.instance.GetContent("Global.ErrorHappenedDuringDeploy") +
                                                       "\n\n");
                    HttpContext.Current.Response.Write(deployInfo);
                }
                HttpContext.Current.Response.Clear();

                HttpContext.Current.Response.Write("<html><body>");
                Exception e3 = Server.GetLastError();

                ErrorLogger.LogError(e3);
                ErrorLogger.LogError(e2);

                Server.ClearError();
                HttpContext.Current.Response.Flush();
            }
        }

        private void InitializeAutoDeleteCtrl()
        {
            AutoDeleteFormContext.instance.ContentMgr = AdminContentMgr.instance;
            //            AutoDeleteFormContext.instance.MessageMgr = MessageManager.Current;
            AutoDeleteFormContext.instance.ObjectDeleteDelegate = new SimpleListHandler<object>(ObjectDeleteMethod);
            AutoDeleteFormContext.instance.TitleMakerDelegate = BusinessObjectUtil.MakeTitleWithContext;
        }

        private void ObjectDeleteMethod(IList<object> objs)
        {
            List<object> objs2 = new List<object>(objs);
            objs2.Sort(new EntityTopologicalComparer());
            
            foreach (Object obj in objs2)
            {
                HibernateMgr.Current.Delete(obj);
            }

            HibernateSessionFactory.Instance.ClearSecondLevelCache();
        }


        private IMessageManager MyContentManagerFactory()
        {
            return new ContentBasedMessageManager(AdminContentMgr.instance.GetContent);
        }

        //        private static ILanguage[] GetAllLanguages()
        //        {
        //            IList<Language> lgs = QueryMgr.instance.GetAllLanguage().ListOrNull(HibernateMgr.Current.Session);
        //            ILanguage[] arr=new ILanguage[lgs.Count];
        //            int i = 0;
        //            foreach(Language lg in lgs)
        //            {
        //                arr[i++] = lg;
        //            }
        //            return arr;
        //        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }

    internal class EntityTopologicalComparer : IComparer<object>
    {
        public int Compare(object x, object y)
        {
            IEntity x1 = x as IEntity;
            IEntity y1 = y as IEntity;
            if (x1 == null || y1 == null) return 0;
            else return y1.GetTopologicalOrderIndex() - x1.GetTopologicalOrderIndex();
        }
    }
}