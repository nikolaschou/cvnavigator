using System;
using System.Web.UI;

public partial class AdminPages_AdminMasterPage2 : System.Web.UI.MasterPage
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        bool mustAddTitle = false;
        foreach (Control c in this.RightPane.Controls)
        {
            if (c.Visible)
            {
                mustAddTitle = true;
                break;
            }
        }

        if (mustAddTitle) 
            this.RightPane.Controls.AddAt(0, new LiteralControl("<h2>" + Utl.ContentHlp("AdminMasterPage2.Actions") + "</h2>"));

    }
}
