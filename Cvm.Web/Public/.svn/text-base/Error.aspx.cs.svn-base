using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cvm.Backend.Business.Users;
using Cvm.ErrorHandling;
using Cvm.Web.Code;
using Cvm.Web.Navigation;
using log4net;
using Napp.Web.AdminContentMgr;
using Napp.Web.Navigation;

namespace Cvm.Web.AdminPages
{
    public partial class Error : System.Web.UI.Page
    {
        private ILog log = LogManager.GetLogger(typeof (Error));
        private const string _errorInErrorHandling = "<Error in error handling>";
        private List<Exception> exceptionChain;
        protected String errorCode = "E00"+new Random().Next()+"0";
        /*protected override void OnPreInit(EventArgs e)
        {
            try
            {
                MasterPageHelper.Instance.OnPageInit(false);
            } catch(Exception)
            {
                //Ignore
            }
        }*/
        protected void Page_Load(object sender, EventArgs _e)
        {
            try {
                bool hasErrors = GetExceptionChain().Count>0;
                if (hasErrors)
                {
                    log.Error("Error happened: " + GetErrorCode(GetExceptionChain()));
                    foreach (var e in GetExceptionChain())
                    {
                        log.Error(GetExplanation(e));
                        log.Error(e);
                    }
                    this.DataBind();
                }
                //this.ErrorPanel.Visible = hasErrors;
            }
            catch (Exception)
            {
                //Ignore
            }
        }

        /// <summary>
        /// Should be changed to return a unique code for each kind of error.
        /// Now simply return a random value.
        /// </summary>
        /// <param name="getExceptionChain"></param>
        /// <returns></returns>
        protected string GetErrorCode(IList<Exception> getExceptionChain)
        {
            return errorCode;
        }

        protected Exception GetFirstException()
        {
            IList<Exception> chain = GetExceptionChain();
            if (chain!=null && chain.Count>0) return chain[0];
            else return new Exception("Unknown error");
        }

        protected IList<Exception> GetExceptionChain()
        {
            try
            {
                if (this.exceptionChain == null)
                {
                    List<Exception> list = new List<Exception>();
                    Exception error = Server.GetLastError();
                    if (error != null) GetExceptionChainRecurse(list, error);
                    this.exceptionChain = list;
                }
                return this.exceptionChain;

            }
            catch (Exception)
            {
                //Ignore
                return exceptionChain;
            }
        }



        private void GetExceptionChainRecurse(List<Exception> list, Exception exception)
        {
            //Deepest first
            if (exception.InnerException!=null) GetExceptionChainRecurse(list,exception.InnerException);
            list.Add(exception);
        }

        protected string GetExplanation(Exception _error)
        {
            try
            {
                if (_error.HasCode())
                {
                    ErrorCode code = _error.GetCode();
                    String[] parms = _error.GetParameters();
                    String contentId = typeof (ErrorCode).Name + "." + code.ToString();
                    return GetExplanationContent(contentId, parms);
                }
                else return GetExplanationContent("ExceptionReason." + _error.GetType().Name);
            } catch(Exception)
            {
                //Ignore
                return _errorInErrorHandling;
            }
        }

        private string GetExplanationContent(string contentId, params string[] parms)
        {
            string exp = AdminContentMgr.instance.GetContent(contentId, parms);
            if (exp!=null && !String.IsNullOrEmpty(exp.Trim())) return exp;
            else return "An unexpected error happened.";
        }

        protected String GetStackTrace(Exception exception)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                MakeStackTrace(exception, sb);
                return sb.ToString();
            } catch(Exception )
            {
                return _errorInErrorHandling;
            }
        }

        private void MakeStackTrace(Exception exception, StringBuilder sb)
        {
            if (exception.InnerException != null) MakeStackTrace(exception.InnerException, sb);
            sb.AppendLine(exception.Message).AppendLine(exception.StackTrace);
            
        }


        protected string GetErrorCodeLink()
        {
            try
            {
                if (Utl.HasSysRole(SysRoleEnum.SysAdmin) || Utl.HasGlobalRole(GlobalRoleEnum.GlobalAdmin))
                {
                    return "<a href='" + CvmPages.ErrorDetailLink(this.errorCode).GetLinkAsHref() + "'>" + this.errorCode + "</a>";
                }
                else return errorCode;
            } catch(Exception )
            {
                return errorCode;
            }
        }
    }
}
