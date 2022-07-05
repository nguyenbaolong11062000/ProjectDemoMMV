<%@ Page Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" Codebehind="MapGeocodeToHierarchy.aspx.cs"
    Inherits="MMV.MapGeocodeToHierarchy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="frmMain" runat="server" enctype="multipart/form-data">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <!-------------Begin Search  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelSearch" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="background-color: #D0D0D0">
                    <tr>
                        <td colspan="6">
                            &nbsp
                        </td>
                        <td style="min-width:600px"></td>
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
                        </td>
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-------------End Search  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                    <tr>
                        <td colspan="3" style="font-style: italic; color: green;">
                           All fields marked with an asterisk ( <span style="color: Red;">* </span>) are required!<br />                    
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            Region<span style="color: Red; white-space: nowrap;">*</span>:
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            <asp:TextBox ID="P5sTxtRegionCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <%=P5sActRegion.L5sShowAddAll("P5sTxtRegion_Add")%>
                            <%=P5sActRegion.L5sShowRemoveAll("P5sTxtRegion_Remove")%>
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="P5sTxtRegionCD"
                                ErrorMessage="The Region field is required!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            Area<span style="color: Red; white-space: nowrap;">*</span>:
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            <asp:TextBox ID="P5sTxtAreaCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <%=P5sActArea.L5sShowAddAll("P5sTxtAreaCD_Add")%>
                            <%=P5sActArea.L5sShowRemoveAll("P5sTxtAreaCD_Remove")%>
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="P5sTxtAreaCD"
                                ErrorMessage="The Area field is required!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            Distributor<span style="color: Red; white-space: nowrap;">*</span>:
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            <asp:TextBox ID="P5sTxtDistributor" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <%=P5sActDistributor.L5sShowAddAll("P5sTxtDistributor_Add")%>
                            <%=P5sActDistributor.L5sShowRemoveAll("P5sTxtDistributor_Remove")%>
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="P5sTxtDistributor"
                                ErrorMessage="The Distributor field is required!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            DSRs<span style="color: Red; white-space: nowrap;"> * </span>:
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            <asp:TextBox ID="P5sTxtDSR" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                            <%=P5sActDSR.L5sShowAddAll("PP5sTxtDSR_Add")%>
                            <%=P5sActDSR.L5sShowRemoveAll("P5sTxtDSR_Remove")%>
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="P5sTxtDSR"
                                ErrorMessage="The DSR field is required!"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1%; white-space: nowrap">
                            &nbsp;
                        </td>
                        <td style="width: 1%">
                            <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                                <tr>
                                    <td style="width: 1%; white-space: nowrap">
                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnExport)%>
                                        <asp:LinkButton ID="P5sLbtnExport" runat="server" Visible="true" OnClick="P5sLbtnExport_Click">Export</asp:LinkButton>
                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnExport)%>
                                    </td>
                                    <td style="width: 1%; white-space: nowrap">
                                    </td>
                                    <td style="width: 98%; white-space: nowrap">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 98%; white-space: nowrap">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="P5sLbtnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>
