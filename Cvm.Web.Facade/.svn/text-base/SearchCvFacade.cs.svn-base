using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Cvm.Backend.Business.Customers;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Search;
using Cvm.Backend.Business.Skills;
using Cvm.Backend.Business.Util;
using Napp.Backend.Business.Common;
using Napp.Backend.Hibernate;
using Napp.VeryBasic;

namespace Cvm.Web.Facade
{
    public class SearchCvFacade
    {

        public static SearchCvFacade Instance = new SearchCvFacade();
        private QueryMgrDynamicHql query = QueryMgrDynamicHql.Instance;

        private SearchCvFacade()
        {
        }

        private IList<Resource> Map(IList<SysResource> res)
        {
            List<Resource> res2 = new List<Resource>(res.Count);
            foreach (var r in res)
            {
                res2.Add(r.RelatedResourceObj);
            }
            return res2;
        }
        public IList<Resource> SearchCvAndMap(ProfileStatusEnum status, BitPatternEnum profileType, EmployeeTypeEnum empType, long[] customerIds, long[] resourceIds, int? availInNoDays)
        {
            IList<SysResource> result = SearchCv(status, profileType, empType, customerIds, resourceIds, availInNoDays);
            return Map(result);
        }

        public IList<SysResource> SearchCv(ProfileStatusEnum status, BitPatternEnum profileType, EmployeeTypeEnum empType, long[] customerIds, long[] resourceIds, int? availInNoDays)
        {
            string hql = "select res from SysResource res left join fetch res.RelatedResource res2 where not (res2.ProfileStatusId & :status)=0 and (not (res.ProfileTypeId & :profileType)=0 or res.ProfileTypeId=0 or res.ProfileTypeId=-1) "
                //+"and (not (res.EmployeeTypeId & :empType)=0 or res.EmployeeTypeId IS NULL or res.EmployeeTypeId=0 or res.EmployeeTypeId=-1)"
                ;

            if (customerIds!=null && customerIds.Length>0)
            {
                hql +=
                    " and exists (select p from Project p where p.RelatedResource.ResourceId=res2.ResourceId and p.CustomerId in (" +
                    customerIds.ConcatToString(",")
                    +"))";
            }

            if (resourceIds != null && resourceIds.Length > 0)
            {
                hql += " and res.ResourceId in (" + resourceIds.ConcatToString()
                    +")";
            }
            if (availInNoDays!=null)
            {
                hql += " and (res2.AvailableBy is null or res2.AvailableBy < :availableBy)";
            }
            hql += " order by res2.ProfileStatusId desc, res2.FirstName, res2.LastName";
            HiberQuery<SysResource> q = HiberQuery<SysResource>.CreateBySql(hql)
                .SetParm("status", (long)status)
                .SetParm("profileType",(long)profileType)
                //.SetParm("empType", (long)empType)
                ;
            if (availInNoDays!=null)
            {
                //Add the parameter now
                DateTime availBy = DateTime.Now.AddDays((int)availInNoDays);
                q.SetParm("availableBy", availBy);
            }
            IList<SysResource> sysResources = q.ListOrNull(HibernateMgr.Current.Session);
            ResourceMgr.Instance.PrepareContext(sysResources);
            return sysResources;
        }

        public IList<ISearchResult> SearchCvByString(string searchString)
        {
            IList<ISearchResult> result = new List<ISearchResult>();
            //MoveToList(query.SearchCustomersBySearchString(searchString), result);
            MoveToList(query.SearchProjectsBySearchString(searchString), result);
            MoveToList(query.SearchResourcesBySearchString(searchString), result);
            MoveToList(query.SearchSkillBySearchString(searchString), result);
            MoveToList(SearchImportTextBySearchString(searchString), result);
            return result;
        }

        public IList<ResourceImport> SearchImportTextBySearchString(string searchString)
        {
            IList<ResourceImport> list = query.SearchImportTextBySearchStringUtil(searchString);
            foreach(var item in list)
            {
                item.SearchString = searchString;
            }
            return list;
        }

        private void MoveToList(IEnumerable from, IList<ISearchResult> to)
        {
            foreach (Object x in from)
            {
                to.Add((ISearchResult) x);
            }
        }

        /// <summary>
        /// Finds all resources having at least some experience with at least
        /// one of the skillIds.
        /// </summary>
        /// <param name="skillIds"></param>
        /// <param name="omitResources"></param>
        /// <returns></returns>
        public IList<SysResource> SearchCvBySkills(IdfrStringList skillIds, IdfrStringList omitResources)
        {
            return query.SearchCvBySkills(skillIds, omitResources);
        }

        public IList<Resource> GetResourcesByIds(IdfrStringList chosenResourceIds)
        {
            return query.GetResourcesByIds(chosenResourceIds);
        }

        public IList<Resource> SearchResourcesGloballyBySearchString(string searchStr)
        {
            return query.SearchResourcesGloballyBySearchString(searchStr,ProfileStatusEnum.UnderUpdate|ProfileStatusEnum.Done);
        }
    }
}
