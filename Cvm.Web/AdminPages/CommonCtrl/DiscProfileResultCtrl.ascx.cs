using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Resources;
using Cvm.Web.Code;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class DiscProfileResultCtrl : System.Web.UI.UserControl
    {
        private Resource _resource;
        public Resource MyResource 
        {
            set
            {
                this._resource = value; 

                
            }
            get { return _resource; }
        }

       
    }
}