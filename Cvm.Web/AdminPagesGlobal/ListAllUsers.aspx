<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master" AutoEventWireup="true" CodeBehind="ListAllUsers.aspx.cs" Inherits="Cvm.Web.AdminPagesGlobal.ListAllUsers" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Cvm.Backend.Business.DataAccess" %>
<%@ Import Namespace="Cvm.Backend.Business.Meta" %>
<%@ Import Namespace="Cvm.Backend.Business.Users" %>
<%@ Import Namespace="Cvm.Web.Facade" %>
<%@ Import Namespace="Napp.Backend.Business.Meta" %>
<%@ Import Namespace="Napp.Backend.BusinessObject" %>
<%@ Import Namespace="Napp.Backend.DataAccess" %>
<%@ Import Namespace="Napp.Backend.Hibernate" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        PopulateFront();
    }

    protected void PopulateFront()
    {
        this.Grid1.Controls.Clear();
        this.Grid1.DataSource = GetUsers();
        this.Grid1.DataBind();
    }

    private DataTable GetUsers()
    {
        if (table == null)
        {
            string selectCommand =
                @"
select u.userId as [User ID], 
u.userName as [User name], 
u.email as [Email], 
su.sysId as [Sys ID], 
sr.sysName as [Site name], 
sr.sysCode as [Site short code], 
srole.roleName as [Role name] 
from UserObj u left join SysUserObj su on u.userId=su.userId
left join SysResource sres on sres.resourceId=u.userId
left join SysRoot sr on (su.sysId=sr.sysId or sres.sysId=sr.sysId)
left join SysOwner so on sr.sysId=so.sysId
left join SysRole srole on srole.roleId=su.roleId";
            SqlDataAdapter adt = new SqlDataAdapter(selectCommand,
                                                    (SqlConnection) HibernateMgr.Current.GetDirectSqlConnection());
            table = new DataTable();
            adt.Fill(table);
        }
        return table;
    }


    protected void OnClickUserGrid(object sender, GridViewCommandEventArgs e)
    {
        int index = int.Parse(e.CommandArgument.ToString());
        var row = GetUsers().Rows[index];
        object userId = row["User ID"];
        String userName = row["User name"]+"";
        long userIdL = long.Parse(userId+"");
        if (e.CommandName.Equals("Simulate"))
        {
            CvmFacade.Security.SimulateUser(QueryMgr.instance.GetUserObjById(userIdL));            
        } else if (e.CommandName.Equals("ShowPassword"))
        {
            String connStr=ConfigurationManager.ConnectionStrings["aspnetConnectionString"].ConnectionString;
            SqlDataAdapter adt=new SqlDataAdapter("select [password] from aspnet_users u join aspnet_membership m on u.UserId=m.UserId where userName='"+userName+"'",connStr);
            DataTable tbl=new DataTable();
            adt.Fill(tbl);
            if (tbl.Rows.Count == 0)
            {
                Response.Write("User not found:"+userName);
            }
            else
            {
                Response.Write(userName+" / "+tbl.Rows[0][0]);

            }
        } else if (e.CommandName.Equals("Delete"))
        {
            CvmFacade.UserAdmin.DeleteUser(userIdL, userName);
            PageNavigation.GetCurrentLink().Redirect();
        }
    }

    </script>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ad:AdmGridView runat="server" ID="Grid1" AutoGenerateColumns="true" OnRowCommand="OnClickUserGrid">
<Columns>
<asp:ButtonField Text="[Simulate]"  CommandName="Simulate"/>
<asp:ButtonField Text="[Show_PW]" CommandName="ShowPassword"/>
<asp:ButtonField Text="[Delete]" CommandName="Delete"/>
</Columns>
</ad:AdmGridView>
<script type="text/javascript" >
    $(function () {
        $('a:contains(Delete)').click(function() { return confirm('Are you sure you want to delete this user? It cannot be undone.'); });
    });
</script>
</asp:Content>

