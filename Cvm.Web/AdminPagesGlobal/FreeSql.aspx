<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdminPages_FreeSql" Codebehind="FreeSql.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Fri SQL eksekvering</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Panel runat="server" Visible="false">
    
        
        <asp:TextBox runat="server" ID="ConnectionStringTextBox" /><br />
        Database (hvis den sættes kaldes connection.ChangeDatabase(...) efter at den er forbundet.):
        <asp:TextBox runat="server" ID="DatabaseTextBox" /><br />
    </asp:Panel>
        Connection-string (vælg connection string):
        <asp:DropDownList runat="server" ID="ConnectionStringList" /><br />
    Initial sql (denne sql køres inden hoved-forespørgslen):
    <asp:TextBox runat="server" ID="InitSql"/><br />
    Run non-query (hvis du vil lave insert eller update eller delete):<asp:CheckBox ID="CheckBox1" runat="server" />
    <br />
    
    <asp:Label runat="server" ID="msgLabel" />
    <asp:TextBox runat="server" ID="InputSql" Rows="15" Columns="100" TextMode="MultiLine"/><br />
    <asp:Button runat="server" Text="Kør" OnClick="OnClickExecute" ID="ExecuteBtn" />
    <asp:GridView runat="server" ID="Grid" AutoGenerateColumns="true" AllowSorting="true" />
    </div>
    </form>
</body>
</html>
