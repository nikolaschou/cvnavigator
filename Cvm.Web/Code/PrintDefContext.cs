using System;
using Cvm.Backend.Business.Print;
using Cvm.Backend.Business.Users;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Web.Navigation;
using Napp.Web.Session;

namespace Cvm.Web.Code
{
    public class PrintDefContext : RequestObject<PrintDefinition>
    {
        public PrintDefContext() : base(QueryParmCvm.printoptions, CreatePrintDefinitionObject)
        {
        }

        private static PrintDefinition CreatePrintDefinitionObject(string[] args)
        {
            PrintDefinition def = new PrintDefinition();

            if (Utl.HasSysRole(SysRoleEnum.Client))
            {
                def.CvPrintFlagsEnum = CvPrintFlagEnum.IncludeBackground | CvPrintFlagEnum.IncludeProjects | CvPrintFlagEnum.IncludeSkills |
                                       (ContextObjectHelper.CurrentSysUserObjOrFail.AnonymousPrint ? CvPrintFlagEnum.Anonymous : ~CvPrintFlagEnum.Anonymous);
            }
            else
            {
            }

            if (args != null)
            {
                long? printOptions = ParseLong(args[0]);
                long? profildeIds = ParseLong(args[1]);
                long? customerId = ParseLong(args[2]);
                if (printOptions != null) def.CvPrintFlags = (long)printOptions;
                if (profildeIds != null) def.ProfileTypeIds = (long)profildeIds;
                if (customerId != null) def.CustomerId = customerId;
            }

            return def;
        }

        private static long? ParseLong(string s)
        {
            if (String.IsNullOrEmpty(s)) 
                return null;
            else 
                return long.Parse(s);
        }

        private static string[] CreateStringListParam(PrintDefinition def)
        {
            String[] args = new string[3];
            args[0] = "" + def.CvPrintFlags;
            args[1] = "" + def.ProfileTypeIds;
            args[2] = "" + def.CustomerId;

            return args;
        }

        public void AddRequestParmsToLink(PageLink link)
        {
            link.SetParm(this.queryParmName, CreateStringListParam(this.Current));
        }
    }
}