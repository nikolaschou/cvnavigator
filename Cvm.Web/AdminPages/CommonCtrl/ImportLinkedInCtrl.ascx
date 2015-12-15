<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportLinkedInCtrl.ascx.cs" Inherits="Cvm.Web.AdminPages.CommonCtrl.ImportLinkedInCtrl" %>
<%@ Import Namespace="Cvm.Backend.Business.Config" %>
<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Napp.Web.Auto" %>
  <script runat="server"></script>
  <div>
  <div id="linkedInDiv" style="display:block">
  <%= @"
  <script type='text/javascript' src='http://platform.linkedin.com/in.js'>
    api_key: "+WebConfigMgr.LinkedInApiKey+@"
    authorize: false
  </script>
  " %>
  </div>
  <script type="text/javascript">
      var gu;
      // Once we have an authorization, fetch the user's profile via API
      function onLinkedInAuth() {
          IN.API.Profile("me").fields("id", "firstName", "lastName", "headline", "pictureUrl", "educations", "skills", "certifications","positions", "location", "connections", "languages")
        .result(setProfile)
        .error(function (e) { alert("something broke " + e); });
      }

      // Display basic profile information inside the page
      function setProfile(result) {
          $('#linkedInImportDiv input').val($.toJSON(result)).change();
          //Call this callback if it is defined.
          if (linkedInCallback) linkedInCallback(result);
      }

  </script>

   <script type="in/login" data-onAuth="onLinkedInAuth"></script>
  <div id="linkedInImportDiv" style="display:none">
  <asp:TextBox runat="server" ID="LinkedInImportTxBox" OnTextChanged="OnChangeLinkedInImport" AutoPostBack="true" CausesValidation="false" />
  </div>

  </div>