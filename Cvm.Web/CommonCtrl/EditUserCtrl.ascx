<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditUserCtrl.ascx.cs" Inherits="Cvm.Web.CommonCtrl.EditUserCtrl" %>

<%@ Import Namespace="Napp.Web.AdminContentMgr" %>

<div>



<asp:DetailsView AutoGenerateRows="False" DataSourceID="MemberData"
  ID="UserInfo" runat="server" OnItemUpdating="UserInfo_ItemUpdating"
  >
  
<Fields>
	<asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
	</asp:BoundField>
	<asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:BoundField DataField="Comment" HeaderText="Comment" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:CheckBoxField DataField="IsApproved" HeaderText="Active User" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem" />
	<asp:CheckBoxField DataField="IsLockedOut" HeaderText="Is Locked Out" ReadOnly="true" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem" />
	
	<asp:CheckBoxField DataField="IsOnline" HeaderText="Is Online" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem" />
	<asp:BoundField DataField="CreationDate" HeaderText="CreationDate" ReadOnly="True"
	 HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:BoundField DataField="LastActivityDate" HeaderText="LastActivityDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
	</asp:BoundField>
	<asp:BoundField DataField="LastLoginDate" HeaderText="LastLoginDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
	</asp:BoundField>
	<asp:BoundField DataField="LastLockoutDate" HeaderText="LastLockoutDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:BoundField DataField="LastPasswordChangedDate" HeaderText="LastPasswordChangedDate"
	ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem"></asp:BoundField>
	<asp:CommandField ButtonType="button" ShowEditButton="true" EditText="<%$Content:EditUserCtrl.Edit %>" />
</Fields>
</asp:DetailsView>



<div style="text-align: right; width: 100%; margin: 20px 0px;">
<asp:Button ID="Button1" runat="server" Text="Unlock User" OnClick="UnlockUser" OnClientClick="return confirm('Click OK to unlock this user.')" />
&nbsp;&nbsp;&nbsp;
<asp:Button ID="Button2" runat="server" Text="Delete User" OnClick="DeleteUser" OnClientClick="return confirm('Are Your Sure?')" />
</div>




<td valign="top">
<asp:DropDownList ID="UserRoles" runat="server" />

</td>

</div>
<asp:ObjectDataSource ID="MemberData" runat="server" DataObjectTypeName="System.Web.Security.MembershipUser" SelectMethod="GetUser" UpdateMethod="UpdateUser" TypeName="System.Web.Security.Membership">
	<SelectParameters>
		<asp:QueryStringParameter Name="username" QueryStringField="userId" />
	</SelectParameters>
</asp:ObjectDataSource> 