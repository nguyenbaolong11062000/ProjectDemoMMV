<%@ Page Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" CodeBehind="InactiveCodeNotExistInFileMaster.aspx.cs" Inherits="MMV.Tool.InactiveCodeNotExistInFileMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="frmMain" runat="server" enctype="multipart/form-data">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server" AsyncPostBackTimeout="36000">
        </ajaxToolkit:ToolkitScriptManager>
        <!-------------Begin Search  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelSearch" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="background-color: #D0D0D0">
                    <tr>
                        <td colspan="6">
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%" class="sectionheader">
                            Search</td>
                        <td>
                            <asp:TextBox ID="P5sFilterTxt" runat="server" CssClass="textbox"></asp:TextBox>
                        </td>
                        <td style="width: 1%">
                            <asp:Panel ID="P5sFilterPanel" runat="server">
                            </asp:Panel>
                        </td>
                        <td style="width: 1px">
                            &nbsp;</td>
                        <td style="width: 1%">
                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sSearch)%>
                            <asp:LinkButton ID="P5sSearch" runat="server" CausesValidation="False" OnClick="P5sSearch_Click">Search</asp:LinkButton>
                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sSearch)%>
                        </td>
                        <td style="width: 78%">
                            &nbsp;
                        </td>
                        <td style="width: 1%">
                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnExportTemplate)%>
                            <asp:LinkButton ID="P5sLbtnExportTemplate" runat="server" CausesValidation="False"
                                OnClick="P5sLbtnExportTemplate_Click">Export Template Import</asp:LinkButton>
                            &nbsp;<%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnExportTemplate)%></td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp
                        </td>
                    </tr>
                </table>
                <span class="normaltxtNavi">
                    <%=HttpContext.Current.Session["P5s.Master.FormCaption"]%>
                </span>
                <br />
                <br />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="P5sLbtnExportTemplate" />
            </Triggers> 
        </asp:UpdatePanel>
        <!-------------End Search  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                    <tr>
                        <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                            Inactive List<span style="color: Red;">* </span>:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                        </td>
                        <td style="white-space: nowrap; vertical-align: top;">
                            <asp:FileUpload ID="P5sFileUpload" runat="server" />
                        </td>
                        <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnSaveFile)%>
                            <asp:LinkButton runat="server" ID="P5sLbtnSaveFile" CausesValidation="False" OnClientClick="Hide()"
                                OnClick="P5sLbtnSaveFile_Click">Save</asp:LinkButton>
                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnSaveFile)%>
                        </td>
                        <td style="white-space: nowrap; width: 99%; vertical-align: top;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td  colspan="4" style="white-space: nowrap; width: 1%; vertical-align: top;">
                            <span style="color: Red;">Note : when upload system will not update ward/commune if this information is empty.</span>
                        </td>
                      
                    </tr>
                </table>
                <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="P5sLbtnSaveFile" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>

