using System;
using System.Collections.Generic;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.Print;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Search;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Users;
using Cvm.Backend.Business.Util;
using Cvm.Web.Code;
using Cvm.Web.Facade;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class PrintCvCtrl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Must be assigned when initializing this control
        /// </summary>
        public Resource MyResource;

        private string lastSkillTypeName;

        /// <summary>
        /// Must be assigned when initializing this control
        /// </summary>
        public PrintDefinition MyPrintDefinition;

        /// <summary>
        /// The widht of the page in pixels.
        /// </summary>
        protected int PageWidth = 620;

        protected int FirstColWidth = 200;
        private List<Project> customerProjects;

        private int DateComparison(Project p1, Project p2)
        {
            if (!p1.StartedBy.HasValue) 
                return 1;
            if (!p2.StartedBy.HasValue) 
                return -1;
            
            return -p1.StartedBy.Value.CompareTo(p2.StartedBy.Value);
        }

        public override void DataBind()
        {
            MyResource.KeepAnonymous = MyPrintDefinition.HasPrintOptions(CvPrintFlagEnum.Anonymous);
            ContextObjectHelper.CurrentBusinessObject = MyResource;

            base.DataBind();

            SysOwner owner = ContextObjectHelper.CurrentSysOwnerOrNull;
            if (owner != null && owner.RelatedLogoFileRefObj != null)
            {
                this.LogoImage.ImageUrl = owner.RelatedLogoFileRefObj.GetAsUrl();
                this.LogoImage.Visible = true;
            }
        }

        protected IEnumerable<Project> GetRemainingProjects()
        {
            Iesi.Collections.Generic.ISet<Project> ps1 = MyResource.Projects;
            List<Project> ps2 = new List<Project>(ps1);
            ps2.RemoveAll(p => p.CustomerId == this.MyPrintDefinition.CustomerId);
            ps2.Sort(DateComparison);

            return ps2;
        }

        protected List<Project> GetCustomerProjects()
        {
            if (this.customerProjects == null)
            {
                this.customerProjects = new List<Project>();
                foreach (Project p in MyResource.Projects)
                {
                    if (p.CustomerId == MyPrintDefinition.CustomerId) customerProjects.Add(p);
                }

                this.customerProjects.Sort(DateComparison);
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

        protected String FormatDate(DateTime? from, DateTime? to)
        {
            return DateHelper.Instance.FormatDuration(from, to);
        }

        public IList<ISearchResultWithDate> GetResourceQualifications()
        {
            SearchResultWithDateTypeEnum types =
                SearchResultWithDateTypeEnum.Certification
                | SearchResultWithDateTypeEnum.Education
                | SearchResultWithDateTypeEnum.Merit;
            List<ISearchResultWithDate> qualifications = MyResource.GetQualifications(types);
            qualifications.Sort(delegate(ISearchResultWithDate a, ISearchResultWithDate b) { return -1 * DateHelper.Compare(a.ResultPeriodDate, b.ResultPeriodDate); });
            return qualifications;
        }

        protected String MakeAnonymName(Resource r)
        {
            if (this.MyPrintDefinition.HasPrintOptionsOne(CvPrintFlagEnum.Anonymous))
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

        protected String IsContentContentEmpty(Resource r, string s)
        {
            string returnValue = null;

            switch (s)
            {
                case "PrintCv.Address1":
                    returnValue = r.Address1;
                    break;

                case "PrintCv.Address2":
                    returnValue = r.Address2;
                    break;

                case "PrintCv.PostalCode":
                    returnValue = r.PostalCode;
                    break;

                case "PrintCv.City":
                    returnValue = r.CityName;
                    break;

                case "PrintCv.PhoneNumber1":
                    returnValue = r.Phonenumber1;
                    break;

                case "PrintCv.PhoneNumber2":
                    returnValue = r.Phonenumber2;
                    break;

                case "PrintCv.AIMInfo":
                    returnValue = r.AIMInfo;
                    break;
            }

            return returnValue;
        }

        protected String GetAddressContent(Resource r, string s)
        {
            string returnValue = null;

            switch (s)
            {
                case "PrintCv.Address1":
                case "PrintCv.Address2":
                case "PrintCv.PostalCode":
                case "PrintCv.City":
                case "PrintCv.PhoneNumber1":
                case "PrintCv.PhoneNumber2":
                case "PrintCv.AIMInfo":
                    if (!this.MyPrintDefinition.HasPrintOptionsOne(CvPrintFlagEnum.Anonymous))
                    {
                        if (this.MyPrintDefinition.HasPrintOptionsOne(CvPrintFlagEnum.IncludeContactInfo))
                        {
                            if (!string.IsNullOrEmpty(IsContentContentEmpty(r,s)))
                                returnValue = PrintHelper.Instance.GetContent(s);
                        }
                    }
                    break;

                default:
                    returnValue = PrintHelper.Instance.GetContent(s);
                    break;
            }

            return returnValue;
        }

        protected string GetContactInfo(Resource r, string s)
        {
            string returnValue = null;

            if (!this.MyPrintDefinition.HasPrintOptionsOne(CvPrintFlagEnum.Anonymous))
            {
                if (this.MyPrintDefinition.HasPrintOptionsOne(CvPrintFlagEnum.IncludeContactInfo))
                {
                    returnValue = IsContentContentEmpty(r, s);
                }
            }

            return returnValue;
        }
        
        protected bool HasCustomerProjects()
        {
            return this.GetCustomerProjects().Count > 0;
        }

    }
}