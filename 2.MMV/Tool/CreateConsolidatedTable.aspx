<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateConsolidatedTable.aspx.cs" MasterPageFile="~/P5s2.Master" Inherits="MMV.Tool.CreateConsolidatedTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="frmMain" runat="server">
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
                            <td colspan="3" style="font-style: italic; color: green;">Ô có dấu <span style="color: Red;">* </span>là bắt buộc nhập!<br />
                                All fields marked with an asterisk (<span style="color: Red;">*</span>) are required<br />
                                &nbsp
                            </td>
                           
                        </tr>
                        <tr>
                            <td style="width: 1%; white-space: nowrap">Database name:
                            </td>
                            <td style="width: 1%; padding-left: 15px">
                                <asp:TextBox ID="P5sTxtDatabaseName" runat="server" CssClass="TextBox" Width="200px" AutoPostBack="true">
                                </asp:TextBox>
                                <asp:Label ID="P5sLblCheckExistsDatabase" runat="server" Style="color: red; display: inline"></asp:Label>
                            </td>
                            <td rowspan="4" style="width: 98%; white-space: nowrap; text-align: center">
                              <asp:TextBox ID="P5sTxtLog" TextMode="MultiLine" ReadOnly="true" style="height:100px;width:60%;" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 1%; white-space: nowrap">Year:
                            </td>
                            <td style="width: 1%; padding-left: 15px">
                                <asp:TextBox ID="P5sTxtYear" runat="server" type="number" min="2010" CssClass="TextBox" Width="70px">
                                </asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="width: 1%; white-space: nowrap">Insert data to [M_TABLE]
                            </td>
                            <td style="width: 1%; padding-left: 15px">
                                <asp:CheckBox ID="ckbInsertDataToMTABLE" runat="server" Checked="false" />
                            </td>
                           
                        </tr>
                        <tr>
                           <td style="width: 1%; white-space: nowrap; vertical-align: text-top; padding-top: 11px;">Create table:
                            </td>
                            <td style="width: 1%; padding-left: 15px">
                                <asp:TextBox ID="P5sTxtTableName" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                <%=P5sActTableName.L5sShowAddAll("P5sTxtTableName_Add")%>
                                <%=P5sActTableName.L5sShowRemoveAll("P5sTxtTableName_Remove")%>
                            </td>
                           
                        </tr>
                        <tr>
                            <td style="width: 1%; white-space: nowrap"></td>
                            <td style="width: 1%">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnCreate)%>
                                <asp:LinkButton ID="P5sLbtnCreate" runat="server" Visible="true" OnClick="P5sLbtnCreate_Click">Create</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnCreate)%>
                            </td>
                           
                        </tr>
                    </table>
                    <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="P5sLbtnCreate" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>
