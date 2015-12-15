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

    /// <summary>
    /// Contains various extension methods for easy formating
    /// </summary>
    public static class FormatExtensions
    {
        private const String MORE = " [...]";
        /// <summary>
        /// Formats a string to be shown on the format
        /// sdfklæj fsdæklfj sdælkfj sdælk sdjf [...]
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string FormatMore(this string str, int length)
        {
            if (str == null) return "";
            if (str.Length < length) return str;
            else
            {
                return "<span class='more' title='" + str + "'>" + str.Substring(0, length - MORE.Length) + MORE + "</span>";
            }
        }

        /// <summary>
        /// Substitutes comma with html-breaks
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String Comma2LineBreak(this string s)
        {
            if (s == null) return null;
            return s.Replace(",", "<br/>");
        }
        

        public static String FormatDate(this DateTime? d)
        {
            if (d == null) return "";
            else return ((DateTime)d).ToShortDateString();
        }
    }
