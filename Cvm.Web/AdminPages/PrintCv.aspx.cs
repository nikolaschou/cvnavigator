using System;
using System.Collections.Generic;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.Print;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Search;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Util;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.AdminPages
{
    public partial class PrintCv : System.Web.UI.Page
    {
        protected readonly BusinessRequestObject<Resource> req = new BusinessRequestObject<Resource>(QueryParmCvm.id);
        private string lastSkillTypeName;
        private readonly PrintDefContext printDefContext=new PrintDefContext();
        /// <summary>
        /// The widht of the page in pixels.
        /// </summary>
        protected int PageWidth = 620;
        protected int FirstColWidth = 150;
        private List<Project> customerProjects;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CvmFacade.Security.VerifyAccess(req.Current, AccessMode.read);
            req.Current.KeepAnonymous = printDefContext.Current.HasPrintOptions(CvPrintFlagEnum.Anonymous);
            ContextObjectHelper.CurrentBusinessObject = req.Current;
            this.PrintCvCtrl1.MyResource = this.MyResource;
            this.PrintCvCtrl1.MyPrintDefinition = this.printDefContext.Current;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();
            /*SysOwner owner = ContextObjectHelper.CurrentSysOwnerOrNull;
            if (owner!=null && owner.RelatedLogoFileRefObj!=null)
            {
                this.LogoImage.ImageUrl = owner.RelatedLogoFileRefObj.GetAsUrl();
                this.LogoImage.Visible = true;
            }*/
        }

        protected PrintDefinition PrintDef
        {
            get 
            { 
                return printDefContext.Current; 
            }
        }

        public Resource MyResource
        {
            get
            {
                return req.Current;
            }
        }

        protected void OnClick_ChangeOptions(object sender, EventArgs e)
        {
            
        }

        protected IEnumerable<Project> GetRemainingProjects()
        {
            foreach (Project p in MyResource.Projects)
            {
                if (p.CustomerId != this.PrintDef.CustomerId) 
                    yield return p;
            }
        }

        protected List<Project> GetCustomerProjects()
        {
            if (this.customerProjects == null)
            {
                this.customerProjects = new List<Project>();
                
                foreach (Project p in MyResource.Projects)
                {
                    if (p.CustomerId == PrintDef.CustomerId) 
                        customerProjects.Add(p);
                }
            }

            return customerProjects;
        }

        protected String GetSkillLevels()
        {
            return SkillMgr.GetAllSkillLevelsAsString();
        }

        /// <summary>
        /// Determines whether the skill-type has changed since last.
        /// Used to obtain simple grouping logic in the skill-grid.
        /// </summary>
        /// <param name="currentSkillTypeName"></param>
        /// <returns></returns>
        protected bool IsNewSkillType(string currentSkillTypeName)
        {
            if (this.lastSkillTypeName != currentSkillTypeName)
            {
                this.lastSkillTypeName = currentSkillTypeName;
                return true;
            }
            
            return false;
        }

        protected Iesi.Collections.Generic.ISet<Project> GetProjectsSorted()
        {
            return this.MyResource.Projects;
        }

       protected String FormatDate(DateTime? from, DateTime? to)
       {
           return DateHelper.Instance.FormatDuration(from, to);
       }

       public IList<ISearchResultWithDate> GetResourceQualifications()
       {
           SearchResultWithDateTypeEnum types = SearchResultWithDateTypeEnum.Certification | SearchResultWithDateTypeEnum.Education | SearchResultWithDateTypeEnum.Merit;
           
           List<ISearchResultWithDate> qualifications = MyResource.GetQualifications(types);
           qualifications.Sort(delegate(ISearchResultWithDate a, ISearchResultWithDate b) {return -1 * DateHelper.Compare(a.ResultPeriodDate, b.ResultPeriodDate);});
           return qualifications;
       }

        protected String MakeAnonymName(Resource r)
        {
            if (this.PrintDef.HasPrintOptionsOne(CvPrintFlagEnum.Anonymous))
            {
                return r.GetInitials();
            } 
            else
            {
                return r.FullName;
            }
        }

        protected String GetContent(string s)
        {
            return PrintHelper.Instance.GetContent(s);
        }

        protected bool HasCustomerProjects()
        {
            return this.GetCustomerProjects().Count > 0;
        }

        protected IEnumerable<ResourceSkill> GetResourceSkills()
        {
            if (ContextObjectHelper.CurrentSysIdIsSpecified())
            {
                return MyResource.GetResourceSkillsFiltered(this.PrintDef.ProfileTypeIds);
            } 
            else
            {
                return MyResource.ResourceSkills;
            }
        }
    }
}