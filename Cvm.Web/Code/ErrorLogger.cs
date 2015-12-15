using System;
using log4net;

namespace Cvm.Web.Code
{
    internal class ErrorLogger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (ErrorLogger));
        private const string ERROR_ID = "Error-ID";

        /// <summary>
        /// Logs an error and returns the error ID.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string LogError(Exception e)
        {
            string s = "ERR" + GetErrorGuid(e);
            log.Error(s+ "\n" + e.ToString());
            return s;
        }

        public static String GetErrorGuid(Exception e)
        {
            if (e.Data[ERROR_ID]==null)
            {
                e.Data[ERROR_ID] = Guid.NewGuid().ToString();
            }
            return (string) e.Data[ERROR_ID];
        }
    }
}