<%@ Page Title="" Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" CodeBehind="MasterDistributorDemo1.aspx.cs" Inherits="MMV.Report.MasterDistributorDemo1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="frmMain" runat="server">
        <div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"  AsyncPostBackTimeOut="0" >
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="P5sDetailPanel" runat="server" Width="100%">
                        <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                        <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                 
                            <tr>
                                <td colspan="2" style="font-style: italic; color: green;">
                                    <%=MMV.L5sMaster.L5sLangs["All fields marked with an asterisk (<b style='color: Red; font-size: large'>*</b>)are required."]   %><br />
                                    &nbsp
                                </td>
                                <td style="min-width:600px"></td>
                            </tr>
                            <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <%=MMV.L5sMaster.L5sLangs["Region"]%><span style="color: Red; white-space: nowrap;">  </span>:
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtRegionCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActRegion.L5sShowAddAll("P5sTxtRegion_Add")%>
                                    <%=P5sActRegion.L5sShowRemoveAll("P5sTxtRegion_Remove")%>
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                   
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <%=MMV.L5sMaster.L5sLangs["Area"]%><span style="color: Red; white-space: nowrap;">  </span>:
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtAreaCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActArea.L5sShowAddAll("P5sTxtAreaCD_Add")%>
                                    <%=P5sActArea.L5sShowRemoveAll("P5sTxtAreaCD_Remove")%>
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                   
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                     <%=MMV.L5sMaster.L5sLangs["Province"]%><span style="color: Red; white-space: nowrap;"> * </span>:
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtProvinceCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActProvince.L5sShowAddAll("P5sTxtProvinceCD_Add")%>
                                    <%=P5sActProvince.L5sShowRemoveAll("P5sTxtProvinceCD_Remove")%>
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="P5sTxtProvinceCD"
                                        ErrorMessage="Bắt buộc nhập!"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                             <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <%=MMV.L5sMaster.L5sLangs["District"]%><span style="color: Red; white-space: nowrap;"> * </span>:
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtDistrictCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActDistrict.L5sShowAddAll("P5sTxtDistrict_Add")%>
                                    <%=P5sActDistrict.L5sShowRemoveAll("P5sTxtDistrict_Remove")%>
                                    <%--2 hàm có sẵn để dùng--%>
                                </td>
                                 <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="P5sTxtDistrictCD"
                                        ErrorMessage="Bắt buộc nhập!"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <%=MMV.L5sMaster.L5sLangs["Ward"]%><span style="color: Red; white-space: nowrap;"> * </span>:
                                </td>
                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                    <asp:TextBox ID="P5sTxtWardCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                    <%=P5sActWard.L5sShowAddAll("P5sTxtWard_Add")%>
                                    <%=P5sActWard.L5sShowRemoveAll("P5sTxtWard_Remove")%>
                                    <%--2 hàm có sẵn để dùng--%>
                                </td>
                                 <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="P5sTxtWardCD"
                                        ErrorMessage="Bắt buộc nhập!"></asp:RequiredFieldValidator>
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
                                                <asp:LinkButton ID="P5sLbtnExport" runat="server" Visible="true" OnClick="P5sLbtnExport_Click"><%=MMV.L5sMaster.L5sLangs["Export"]%></asp:LinkButton>
                                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnExport)%>
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
