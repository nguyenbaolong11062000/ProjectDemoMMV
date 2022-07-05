<%@ Page Title="" Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" CodeBehind="MandatedAssortmentReport.aspx.cs" Inherits="MMV.fsmdls.StockCount.MandatedAssortmentReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
     <form id="frmMain" runat="server">
        <div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="0">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    &nbsp;&nbsp;<span class="normaltxtNavi"><%=HttpContext.Current.Session["P5s.Master.FormCaption"]%></span><br>
                    <br />
                    <asp:Panel ID="P5sDetailPanel" runat="server" Width="100%">
                        <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                        <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                            <tr>
                                <td colspan="2" style="font-style: italic; color: green;">
                                    Ô có dấu <span style="color: Red;">* </span>là bắt buộc nhập!<br />
                                    All fields marked with an asterisk (<span style="color: Red;">*</span>) are required<br />
                                    &nbsp
                                </td>
                                <td style="min-width:500px"></td>
                            </tr>
                            <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    Vùng/Region<span style="color: Red; white-space: nowrap;"> * </span>:
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtRegionCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActRegion.L5sShowAddAll("P5sTxtRegion_Add")%>
                                    <%=P5sActRegion.L5sShowRemoveAll("P5sTxtRegion_Remove")%>
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="P5sTxtRegionCD"
                                        ErrorMessage="Bắt buộc nhập!"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    Khu vực/Area<span style="color: Red; white-space: nowrap;"> *</span>:
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtAreaCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActArea.L5sShowAddAll("P5sTxtAreaCD_Add")%>
                                    <%=P5sActArea.L5sShowRemoveAll("P5sTxtAreaCD_Remove")%>
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="P5sTxtAreaCD"
                                        ErrorMessage="Bắt buộc nhập!"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    Nhà phân phối/Distributor<span style="color: Red; white-space: nowrap;"> * </span>
                                    :
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtDistributor" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActDistributor.L5sShowAddAll("P5sTxtDistributor_Add")%>
                                    <%=P5sActDistributor.L5sShowRemoveAll("P5sTxtDistributor_Remove")%>
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="P5sTxtAreaCD"
                                        ErrorMessage="Bắt buộc nhập!"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            


<%--                            <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    DSRs<span style="color: Red; white-space: nowrap;"> * </span>:
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtDSR" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActDSR.L5sShowAddAll("PP5sTxtDSR_Add")%>
                                    <%=P5sActDSR.L5sShowRemoveAll("P5sTxtDSR_Remove")%>
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="P5sTxtAreaCD"
                                        ErrorMessage="Bắt buộc nhập!"></asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>


                             <tr>
                                <td style="width: 1%; white-space: nowrap">
                                    Ngày/Date:
                                </td>
                                <td style="width: 1%">
                                    <table cellpadding="0" cellspacing="5px" border="0">
                                        <tr>
                                            <td style="padding: 0">
                                                Từ/From
                                                <asp:TextBox ID="P5sTxtFromDate" runat="server" CssClass="TextBox" Width="120px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="P5sTxtFromDate"
                                                    Format="dd-MM-yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                            </td>
                                            <td style="padding: 0">
                                                Đến/To
                                                <asp:TextBox ID="P5sTxtToDate" runat="server" CssClass="TextBox" Width="120px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="P5sTxtToDate"
                                                    Format="dd-MM-yyyy">
                                                </ajaxToolkit:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 98%; white-space: nowrap">
                                    &nbsp;
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
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="P5sLbtnExport" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</asp:Content>
