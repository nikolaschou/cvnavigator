using System;
using System.Collections.Generic;
using System.Linq;
using Cvm.Backend.Business.Assignments;
using Cvm.Backend.Business.Resources;
using Cvm.Backend.Business.Users;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;

namespace Cvm.Web.Facade
{
    public class AssignmentFacade
    {
        private AssignmentFacade() { }
        public static AssignmentFacade Instance=new AssignmentFacade();

        public IEnumerable<Assignment> GetAssigmentsForCurrentUser()
        {
            if (!ContextObjectHelper.CurrentUserHasSysRole(SysRoleEnum.Client))
            {
                throw new NotImplementedException("Only implemented for Client role at this point.");
            }

            var a = ContextObjectHelper.CurrentSysUserObjOrFail.Assignments;

           
            return a.Where(b=>b.IsPending());
        }

        /// <summary>
        /// Validates all assigned resources for the following:
        /// 1. If the CV is Inactive, a warning will be given and the resource will be removed from the list
        /// 2. If at least one CV is Draft, a warning will be given
        /// 3. If at least one resource is unavailable when the assignment starts, a warning will be given
        /// </summary>
        /// <param name="ars"></param>
        public void ValidateResources(List<AssignmentResource> ars)
        {
            bool hasUnavailableResource = false;
            bool hasUnknownAvailability = false;
            bool hasDraftCv = false;

            foreach (var r in ars)
            {
                if (!hasDraftCv && r.RelatedResourceObj.ProfileStatusIdEnum == ProfileStatusEnum.UnderUpdate)
                {
                    hasDraftCv = true;
                }
                
                if (!hasUnknownAvailability && r.RelatedResourceObj.AvailableBy==null)
                {
                    hasUnknownAvailability = true;
                }
                
                if (!hasUnavailableResource && r.RelatedResourceObj.AvailableBy > r.RelatedAssignmentObj.ContractStartBy)
                {
                    hasUnavailableResource = true;
                }
            }
            
            if (hasDraftCv)
            {
                MessageManager.Current.PostMessage("AssignmentFacade.WarningHasDraft");
            }

            if (hasUnavailableResource)
            {
                MessageManager.Current.PostMessage("AssignmentFacade.WarningHasUnavail");
            }

            if (hasUnknownAvailability)
            {
                MessageManager.Current.PostMessage("AssignmentFacade.WarningHasUnknownAvail");
            }

            if (hasDraftCv || hasUnavailableResource || hasUnknownAvailability)
            {
                MessageManager.Current.PostMessage("AssignmentFacade.Warning");
            }
            
            //Now handle inactiveCV's
            ars.ForEach(a=>
                             {
                                 if (a.RelatedResourceObj.ProfileStatusIdEnum==ProfileStatusEnum.Inactive)
                                 {
                                     MessageManager.Current.PostMessage("AssignmentFacade.InactiveCv", a.RelatedResourceObj.FullName);
                                     HibernateMgr.Current.Delete(a);
                                 }
                             });
            ars.RemoveAll(a => (a.RelatedResourceObj.ProfileStatusIdEnum == ProfileStatusEnum.Inactive));
        }
    }
}