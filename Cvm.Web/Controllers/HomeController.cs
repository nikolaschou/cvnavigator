using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.Meta;
using Napp.Backend.Hibernate;

namespace MvcApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }
        public ActionResult Index2()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            try
            {

                HibernatePluginConfig.BusinessAssembly = "Cvm.Backend.Business";
                HibernatePluginConfig.TypeParser = new TypeParserImpl();
                HibernateMgr.InitializeForRequestScope();
                HibernateMgr.Current.BeginSessionAndTransaction();
                var q = from p in Hiber.Q<Project>() where p.ProjectName.Contains("s") select p;

                return View(new AboutModel() {Projects = q.ToList()});
            }finally
            {

                HibernateMgr.Current.CommitAndCloseTransactionAndSession();
            }
        }
    }

    public class AboutModel : IView
    {
        public void Render(ViewContext viewContext, TextWriter writer)
        {
        }

        public IEnumerable<Project> Projects;
    }
}
