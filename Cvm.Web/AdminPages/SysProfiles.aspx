<%@ Page Title="" Language="C#" MasterPageFile="~/AdminPages/AdminMasterPage.master"
    AutoEventWireup="true" CodeBehind="SysProfiles.aspx.cs" Inherits="Cvm.Web.AdminPages.SysProfiles" %>

<%@ Import Namespace="Cvm.Backend.Business.Resources" %>
<%@ Import Namespace="Cvm.Backend.Business.Skills" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="addDiv">
        <fieldset>
        <div class="subformDiv">
            <table class="paddedTable">
                <tr>
                    <td>
                        <%=Utl.ContentHlp("SysProfiles.AddSkill") %>
                    </td>
                    <td>
                        <ad:AdmTextBox runat="server" ID="AddSkillTextBox" OnTextChanged="OnChangeAddSkill"
                            CausesValidation="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Utl.ContentHlp("SysProfiles.AddProfile") %>
                    </td>
                    <td>
                        <ad:AdmTextBox runat="server" ID="AddProfileTextBox" OnTextChanged="OnChangeAddProfile"
                            CausesValidation="false" />
                    </td>
                </tr>
            </table>
            </div>
            <br />
        </fieldset>
    </div>
    <br />
    <% if (MyProfileTypes.Count == 0 && MySysSkills.Count == 0)
       {%>
    <%=Utl.ContentBread("SysProfiles.NoSkillOrProfile")%>
    <%
        }
       else
       {%>
    <%=Utl.ContentHlp("SysProfile.MatrixIntro") %><br />
    <fieldset>
        <legend>
            <%=Utl.Content("SysProfiles.SkillMatrixLegend")%></legend>
        <table class="paddedTable">
            <tr>
                <td>
                    <table class="matrix" id="profileMatrixHeader">
                        <thead>
                            <tr>
                                <td>
                                </td>
                                <%
                                    foreach (ProfileType p in this.MyProfileTypes)
                                    {%>
                                <td title='<%=p.ProfileTypeName%>'>
                                    <%=p.GetNameAbbreviation()%>
                                </td>
                                <%
                                    }%>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </thead>
                    </table>
                    <div id="profileMatrixDiv">
                        <table class="matrix" id="profileMatrix">
                            <tbody>
                                <%
                                    foreach (SysSkill ss in this.MySysSkills)
                                    {%>
                                <tr>
                                    <td>
                                        <%=ss.SkillName%>
                                    </td>
                                    <%
                                        foreach (ProfileType p in this.MyProfileTypes)
                                        {%>
                                    <td class='selector <%=ss.IsAssignedToProfile(p) ? "marked" : ""%>'>
                                        &nbsp;
                                    </td>
                                    <%
                                        }%>
                                    <td>
                                        <img title="<%=Utl.Content("SysProfiles.RemoveSkill")%>" src="../images/master/Delete-24.png"
                                            alt="delete" skillid="<%=ss.SkillId%>" />
                                    </td>
                                </tr>
                                <%
                                    }%>
                            </tbody>
                        </table>
                    </div>
                    <br />
                    <div style='<%=MyProfileTypes.Count==0 || this.MySysSkills.Count<15 ? "display:none": ""%>'>
                        <ad:AdmButton ID="AdmButton3" runat="server" OnClick="OnClickSaveBtn" ContentId="Standard.Save" />
                        <ad:AdmButton ID="AdmButton4" runat="server" OnClick="OnClickCancelBtn" ContentId="Standard.Cancel" />
                    </div>
                </td>
                <td valign="top" style="padding-left: 30px">
                    <%
                        if (this.MyProfileTypes.Count > 0)
                        {%>
                    <%=Utl.ContentHlp("SysProfiles.Abbreviations")%>
                    <br />
                    <asp:Repeater runat="server" ID="ProfileRep">
                        <ItemTemplate>
                            <nobr>
                <asp:ImageButton title="<%$Content:SysProfiles.DeleteProfile %>" AlternateText="<%$Content:SysProfiles.DeleteProfile %>" ImageUrl="../images/master/Delete-16.png" OnClick="OnClickDeleteProfile" CommandArgument="<%#((ProfileType)Container.DataItem).ProfileTypeId %>" runat="server"></asp:ImageButton>
                &nbsp;&nbsp;<b><%#((ProfileType)Container.DataItem).GetNameAbbreviation()%></b>: 
        <%#((ProfileType)Container.DataItem).ProfileTypeName %>

    </nobr>
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>
                    <%
           
                        }
                    %>
                </td>
            </tr>
        </table>
        <div class="buttons">
        <div style='<%=MyProfileTypes.Count==0 || this.MySysSkills.Count==0 ? "display:none": ""%>'>
            <ad:AdmButton ID="AdmButton1" runat="server" CssClass="saveBtn" OnClick="OnClickSaveBtn" ContentId="Standard.Save" />
            <ad:AdmButton ID="AdmButton2" runat="server" CssClass="cancelBtn" OnClick="OnClickCancelBtn" ContentId="Standard.Cancel" />
        </div>
        </div>
    </fieldset>
    <%
        }%>
    <div style="display: none" id="hiddenStateDiv">
        <asp:TextBox runat="server" ID="StateTextBx" />
    </div>
    <script src="../Js/SysProfiles.js" type="text/javascript">
    </script>
</asp:Content>
