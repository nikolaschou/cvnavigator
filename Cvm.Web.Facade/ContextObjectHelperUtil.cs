using System;
using System.Text.RegularExpressions;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Meta;

namespace Cvm.Web.Facade
{
    public static class ContextObjectHelperUtil
    {

        public static SysId FindSysIdFromSubDomain(string host)
        {
            string subdomain = GetPrefix(host);
            if (subdomain == null) return null;
            SysRoot root = QueryMgr.instance.GetSysRootBySysCodeOrNull(subdomain);
            if (root != null) return root.SysIdObj;
            else return null;
        }

        /// <summary>
        /// Returns the subdomain of the given host name
        /// If host = xxx.cvnav.dk then
        /// parts = [xxx , cvnav , dk]
        /// and subdomain is xxx.
        /// If host is an ip-address, null is returned.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static String GetPrefix(string host)
        {
            if (new Regex(@"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+").IsMatch(host))
            {
                //It's an ip-address, has no prefix
                return null;
            }
            String[] parts = host.Split('.');
            if (parts.Length < 3) return null;
            else return parts[0];
        }
    }
}