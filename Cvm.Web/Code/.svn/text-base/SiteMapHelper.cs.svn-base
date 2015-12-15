using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Cvm.Web.Code
{
    /// <summary>
    /// Resolves current node using a simple search mechanism allowing for pages in web.sitemap with up to two query parameters.
    /// It is required that the query parameters are put in a specific order and always at the beginning of the request URL.
    /// The principle of longest match is applied, hence if the URL including the first two query parameters matches then 
    /// it has higher precedence than if the URL including only the first query parameter matches which again has higher precedence
    /// than if the URL without query parameters matches.
    /// </summary>
    public class SiteMapHelper
    {
        private static Dictionary<String, SiteMapNode> map;
        private static String[] queryParms = { TypeKey, ModeKey };
        private static string ModeKey = "mode";
        private static string TypeKey = "type";

        public static SiteMapNode ResolveCurrentNode(object sender, SiteMapResolveEventArgs e)
        {
            String path = HttpContext.Current.Request.Path;
            var queryColl = HttpContext.Current.Request.QueryString;
            SiteMapNode result;
            
            //If we have both type and mode parameters...
            if (null != (result = TryParms(path, queryColl, TypeKey, ModeKey))) 
                return result;
            
            //If we have only type parameter
            if (null != (result = TryParms(path, queryColl, TypeKey))) 
                return result;
            
            //If we have only mode parameter
            if (null != (result = TryParms(path, queryColl, ModeKey))) 
                return result;
            
            //If we have no parameters
            if (null != (result = TryParms(path, queryColl))) 
                return result;
            
            return null;
        }

        private static SiteMapNode TryParms(string path, NameValueCollection queryColl, params string[] keys)
        {
            if (map == null)
            {
                map = new Dictionary<string, SiteMapNode>();
                BuildMap(map, SiteMap.RootNode.ChildNodes);
            }

            string key = path + (keys.Length > 0 ? "?" : "") + ConcatKeys(queryColl, keys);
            
            if (map.ContainsKey(key)) 
                return map[key];
            
            return null;
        }

        /// <summary>
        /// Creates query strings on the format a1=b1&a2=b2
        /// </summary>
        /// <param name="queryColl"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        private static string ConcatKeys(NameValueCollection queryColl, string[] keys)
        {
            //Primitive implementation based on up to two keys. Should be generalized if more keys are needed.
            if (keys.Length == 0) 
                return "";
            if (keys.Length == 1) 
                return keys[0] + "=" + queryColl[keys[0]];
            if (keys.Length == 2) 
                return keys[0] + "=" + queryColl[keys[0]] + "&" + keys[1] + "=" + queryColl[keys[1]];
            
            throw new InvalidProgramException("Expected length <= 2, found " + keys.Length);
        }


        private static void BuildMap(Dictionary<string, SiteMapNode> map, SiteMapNodeCollection childNodes)
        {
            foreach (SiteMapNode node in childNodes)
            {
                if (!String.IsNullOrEmpty(node.Url)) 
                    map[node.Url.Replace("~", "")] = node;
                
                if (node.HasChildNodes)
                {
                    //Recurse
                    BuildMap(map, node.ChildNodes);
                }
            }
        }
    }
}