using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Meta;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Napp.Backend.Business.Meta;
using Napp.Backend.DataAccess;
using Napp.Backend.Hibernate;

namespace Cvm.Web.AdminPagesGlobal
{
    public partial class DataStatistics : System.Web.UI.Page
    {
        private StatisticsFacade.TableCountResult _result;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(false);
        }

        protected StatisticsFacade.TableCountResult TableCount
        {
            get
            {
                if (_result==null)
                {
                    var tables = TableMgr.instance().GetAllTables();
                    _result = CvmFacade.Statistics.GetTableCounts(tables);
                }
                return _result;
            }
        }
    }
}