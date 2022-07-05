<%@ Page Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" CodeBehind="CustomersMonthlyVisitReportAll.aspx.cs" Inherits="MMV.Tool.CustomersMonthlyVisitReportAll" %>

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
                                <td colspan="3" style="font-style: italic; color: green;">
                                    Ô có dấu <span style="color: Red;">* </span>là bắt buộc nhập!<br />
                                    All fields marked with an asterisk (<span style="color: Red;">*</span>) are required<br />
                                    &nbsp
                                </td>
                            </tr>
                          
                            <tr>
                                <td style="width: 1%; white-space: nowrap">
                                    Từ ngày/From:
                                </td>
                                <td style="width: 1%">
                                    <asp:TextBox ID="P5sTxtFromDate" runat="server" CssClass="TextBox" Width="120px"
                                        ReadOnly="True"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="P5sTxtFromDate"
                                        Format="dd-MM-yyyy">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                                <td style="width: 98%; white-space: nowrap">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1%; white-space: nowrap">
                                    Đến ngày/To:
                                </td>
                                <td style="width: 1%">
                                    <asp:TextBox ID="P5sTxtToDate" runat="server" CssClass="TextBox" Width="120px" ReadOnly="True"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="P5sTxtToDate"
                                        Format="dd-MM-yyyy">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                                <td style="width: 98%; white-space: nowrap">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1%; white-space: nowrap">
                                    Số phút dừng/Duration:
                                </td>
                                <td style="width: 98%; white-space: nowrap">
                                    <table cellpadding="0" cellspacing="1px" border="0">
                                        <tr>
                                            <td style="padding: 0">
                                                phút/minute&nbsp;<asp:TextBox ID="P5sTxtMinute" MaxLength="2" runat="server" CssClass="TextBox"
                                                    Width="30px">
                                                </asp:TextBox>
                                                &nbsp;&nbsp; giây/seconds&nbsp;<asp:TextBox ID="P5sTxtSeconds" MaxLength="2" runat="server"
                                                    Text="0" CssClass="TextBox" Width="30px">
                                                </asp:TextBox>
                                                <asp:RangeValidator ID="Rangevalidator1" Type="Integer" ErrorMessage="Please enter value between 0-60."
                                                    ForeColor="Red" ControlToValidate="P5sTxtSeconds" MinimumValue="0" MaximumValue="60"
                                                    runat="server">
                                                </asp:RangeValidator>
                                                <asp:RangeValidator ID="Rangevalidator2" Type="Integer" ErrorMessage="Please enter value between 0-60."
                                                    ForeColor="Red" ControlToValidate="P5sTxtMinute" MinimumValue="0" MaximumValue="60"
                                                    runat="server">
                                                </asp:RangeValidator>
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

