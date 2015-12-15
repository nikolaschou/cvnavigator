using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Cvm.Backend.Business.Meta;
using Cvm.Backend.Business.Users;
using Cvm.Backend.FileStore;
using Cvm.Web.Code;
using Cvm.Web.Facade;
using Cvm.Web.Navigation;
using Napp.Web.AdminContentMgr;
using Napp.Web.DialogCtrl;
using Napp.Web.Navigation;

public partial class AdminPages_AdminMasterPage : CommonMaster
{
    private SiteMapDataSource siteMap = new SiteMapDataSource();

    protected Panel ClosePanel;
    protected Panel SearchPanel;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //Push this master on the page-context so other pages 
        //can manipulate it.
        ThisMaster = this;
        PanelBasedDialogController panelcontroller = new PanelBasedDialogController(this.MainPanel, this.DialogPanel);
        DialogController.InitializeOnRequestStart(panelcontroller.PushDialog, panelcontroller.PopDialog);

        this.HistoryRep.DataSource = ContextObjectHelper.History.GetObject().GetHistoryItems();
        this.HistoryRep.DataBind();
    }

    public static bool IsLoginPage
    {
        set
        {
            HttpContext.Current.Items["AdminMasterPage.IsLoginPage"] = value;
        }
        get
        {
            bool? b = HttpContext.Current.Items["AdminMasterPage.IsLoginPage"] as bool?;
            return b ?? false;
        }
    }

    public bool ShowMenu
    {
        set
        {
            this.MenuPanel.Visible = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "code", "CLOSE_WINDOW_WARNING='" + AdminContentMgr.instance.GetContent("AdminMaster.CloseWindowWarning") + "';", true);

        if (!IsPostBack)
        {
            SysRoot sysRoot = ContextObjectHelper.CurrentSysRoot;
            if (sysRoot != null)
            {
                FilePath imageFile = FolderStructure.Instance.GetLogoFile(sysRoot.SysCodeObj);

                //this.LogoImage.ImageUrl = imageFile.RelativePathDotDot;
                //this.LogoImage.Visible = true;
            }
        }
    }

    private static bool? IsPopupProp
    {
        get
        {
            if (QueryStringHelper.Instance.GetParmIntOrNull(QueryParmCvm.isPopup) == 1) 
                return true;
            
            return HttpContext.Current.Items["isPopup"] as bool?;
        }
        set
        {
            HttpContext.Current.Items["isPopup"] = value;
        }
    }
    
    private static AdminPages_AdminMasterPage ThisMaster
    {
        get
        {
            return HttpContext.Current.Items["thisMaster"] as AdminPages_AdminMasterPage;
        }
        set
        {
            HttpContext.Current.Items["thisMaster"] = value;
        }
    }

    /// <summary>
    /// Determines whether this window should be considered to be a popup-window.
    /// This is the case if either it was explicitly set to true from an outside call or 
    /// if isPopup=1 is set on the query string.
    /// </summary>
    public static bool IsPopup
    {
        get
        {
            if (IsPopupProp != null) 
                return (bool)IsPopupProp;
            else
            {
                return QueryStringHelper.Instance.GetParmOrDefault(QueryParmCommon.isPopup, 0) == 1;
            }
        }
        set
        {
            IsPopupProp = value;
        }
    }
    
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        
        this.MenuPanel.Visible = !IsPopup && !IsLoginPage;
        this.SearchPanel.Visible = Utl.HasSysRole(RoleSet.SalesMgrAtLeast) && !IsPopup && !IsLoginPage;
        this.ClosePanel.Visible = IsPopup;

        // TODO: This has to be changed as this is a shity hack.
        if (ContextObjectHelper.CurrentSysUserObjOrNull != null && ContextObjectHelper.CurrentSysUserObjOrNull.RelatedCompanyObj != null &&!string.IsNullOrEmpty((ContextObjectHelper.CurrentSysUserObjOrNull.RelatedCompanyObj).Name))
            this.Cnt.Title = (ContextObjectHelper.CurrentSysUserObjOrNull.RelatedCompanyObj).Name;
        else
            this.Cnt.Title = MakeTitleBreadCrum(MasterPageHelper.Instance.GetPageTitle(), DialogController.RequestInstance.GetDialogTitles(), MasterPageHelper.Instance.GetSubTitles());            
    }

    private string MakeTitleBreadCrum(string mainTitle, StringCollection subTitles, StringCollection subTitles2)
    {
        if (subTitles == null || subTitles.Count == 0) 
            return mainTitle;
        
        StringBuilder sb = new StringBuilder();
        sb.Append(mainTitle);
        MakeTitleBreadCrumUtil(subTitles, sb);
        MakeTitleBreadCrumUtil(subTitles2, sb);
        
        return sb.ToString();
    }

    private void MakeTitleBreadCrumUtil(StringCollection subTitles, StringBuilder sb)
    {
        foreach (String subTitle in subTitles)
        {
            sb.Append(AdminContentMgr.instance.GetContent("Standard.BreadCrumArrow")).Append(subTitle);
        }
    }
}
