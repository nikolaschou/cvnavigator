using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Napp.Common.MessageManager;

namespace Cvm.Web.AdminPages.CommonCtrl
{
    public partial class MessageValidationCtrl : System.Web.UI.UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            List<string> messages = MessageManager.Current.GetAndClearMessages();
            if (messages != null && messages.Count > 0)
            {
                this.MessageRep.DataSource = messages;
                this.MessageRep.DataBind();
            }
        }
    }
}