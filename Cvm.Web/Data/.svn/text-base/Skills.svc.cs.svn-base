using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Cvm.Web.Facade;
using Napp.Backend.Hibernate;

namespace Cvm.Web.Data
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehaviorAttribute(IncludeExceptionDetailInFaults = true)]
    public class Skills
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public String[] Like(String startWith)
        {
            HibernateMgr.InitializeForRequestScope();

            HibernateMgr.Current.BeginSessionAndTransaction();
            string[] skillsBySearch = CvmFacade.SysProfile.GetSkillsBySearch(startWith);

            HibernateMgr.Current.CommitAndCloseTransactionAndSession();
            return skillsBySearch;
        }

        [OperationContract]
        [System.ServiceModel.Web.WebInvoke(
Method = "POST",
RequestFormat = System.ServiceModel.Web.WebMessageFormat.Json,
ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json)]

        public String[] Like2(String startWith)
        {
            return new String[]
            {
                "a",
                "b",
                    startWith
            }
            ;
        }

        [OperationContract]
        public String Test()
        {
            return "Hello";
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
