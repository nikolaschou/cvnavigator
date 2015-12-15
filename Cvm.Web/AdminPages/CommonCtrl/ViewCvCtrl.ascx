<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewCvCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.ViewCvCtrl" %>
<%@ Register Assembly="Napp.Web.AutoFormExt" Namespace="Napp.Web.AutoFormExt" TagPrefix="cc2" %>

<%@ Register Assembly="Napp.Web.AutoForm" Namespace="Napp.Web.AutoForm" TagPrefix="cc3" %>

<div>
<cc2:AutoFormExt2 EditMode="View" AutoFull="true" ObjectSourceInstance="<%#MyResource %>" runat="server" ID="AutoForm1"  />
<cc3:AutoList RenderMode="Table" runat="server" EditMode="View" ObjectSourceInstance="<%#MyResource.GetResourceSkillsFetched() %>" AutoFull="true" ID="AutoForm2"  />

</div>