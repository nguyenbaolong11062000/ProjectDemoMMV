<%@ Page Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" Codebehind="SalesRoute.aspx.cs"
    Inherits="MMV.MasterData.SalesRoute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="frmMain" runat="server">
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
                    </tr>
                    <tr>
                        <td style="width: 20%" class="sectionheader">
                            Search</td>
                        <td>
                            <asp:TextBox ID="P5sFilterTxt" AutoPostBack="true" runat="server" CssClass="textbox"></asp:TextBox>
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
                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnNew)%>
                            <asp:LinkButton ID="P5sLbtnNew" runat="server" Visible="false" CausesValidation="false">Add new</asp:LinkButton>
                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnNew)%>
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
        <!-------------Begin Detail  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelDetailMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPanelDetailMain" runat="server">
                    <p style="font-weight: bold; margin-left: 10px">
                       </p>
                    <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                    <table cellpadding="0px" cellspacing="5px" border="0" width="100%">
                        <tr>
                            <td style="width: 1%; white-space: nowrap">
                                <b>DSR Code :</b>
                            </td>
                            <td style="">
                                <asp:Label ID="P5sLblSALES_CODE" runat="server" CssClass="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 1%; white-space: nowrap">
                                <b>DSR Name: </b>
                            </td>
                            <td style="">
                                <asp:Label ID="P5sLblSALES_NAME" runat="server" CssClass="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 1%; white-space: nowrap">
                                <b>Distributor: </b>
                            </td>
                            <td style="">
                                <asp:Label ID="P5sLblDISTRIBUTOR_DESC" runat="server" CssClass="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                </asp:Panel>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-------------End Detail  ----------------------->
        <!-------------Begin Detail  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelAddRoute" runat="server">
            <ContentTemplate>
                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                    <tr>
                        <td colspan="2" style="white-space: nowrap; width: 1%; vertical-align: top">
                            <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                                <tr>
                                    <td colspan="2" style="font-style: italic; color: green;">
                                        </span> Add route<br />
                                        &nbsp</td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                        Route<span style="color: Red;"> * </span>
                                    </td>
                                    <td style="">
                                        <table cellpadding="0" cellspacing="0" border="0" width="305px">
                                            <tr>
                                                <td style="padding: 0">
                                                    <asp:TextBox ID="P5sTxtROUTE_CD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                                    <%=P5sActROUTE.L5sShowAddAll("P5sTxtROUTE_CD_Add")%>
                                                    <%=P5sActROUTE.L5sShowRemoveAll("P5sTxtROUTE_CD_Remove")%>
                                                </td>
                                                <td style="vertical-align: top">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap;">
                                        &nbsp&nbsp
                                    </td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" width="1%">
                                            <tr>
                                                <td style="height: 19px">
                                                    &nbsp;</td>
                                                <td style="height: 19px">
                                                    <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnInsert)%>
                                                    <asp:LinkButton ID="P5sLbtnInsert" OnClientClick="if(!confirm('Are you sure?')) return false;"
                                                        runat="server" OnClick="P5sLbtnInsert_Click">Add</asp:LinkButton>
                                                    <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnInsert)%>
                                                </td>
                                                <td style="height: 19px">
                                                    &nbsp;</td>
                                                <td style="height: 19px">
                                                    <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnCancel)%>
                                                    <asp:LinkButton ID="P5sLbtnCancel" runat="server" CausesValidation="False" OnClick="P5sLbtnCancel_Click">Cancel</asp:LinkButton>
                                                    <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnCancel)%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <br />
                </table>
                <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-------------End Detail  ----------------------->
        <br />
        <!-------------Begin Main GV  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelMainGV" runat="server">
            <ContentTemplate>
                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                <asp:Label ID="P5sLblEmptyRow" Font-Italic="true" ForeColor="green" runat="server"
                    Text="No data"></asp:Label>
                <asp:Panel ID="P5sPanelMainGV" runat="server">
                    <br />
                    <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                        <tr>
                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                <asp:DropDownList ID="P5sDdlMainPaging" runat="server" CssClass="dropdown" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="white-space: nowrap; vertical-align: top">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(W5sLbtnRemoveRoute)%>
                                <asp:LinkButton ID="W5sLbtnRemoveRoute" runat="server" OnClientClick="if(!confirm('Are you sure?')) return false;"
                                    CausesValidation="False" OnClick="W5sLbtnRemoveRoute_Click">Remove route</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(W5sLbtnRemoveRoute)%>
                            </td>
                            <td style="white-space: nowrap; vertical-align: top">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnMoveRoute)%>
                                <asp:LinkButton ID="P5sLbtnMoveRoute" runat="server" CausesValidation="False" OnClick="P5sLbtnMoveRoute_Click">Transfer route</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnMoveRoute)%>
                            </td>
                            <td style="width: 99%">
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="P5sMainGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="SALES_ROUTE_CD"
                        CssClass="gridview" OnRowDataBound="P5sMainGridView_RowDataBound" >
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="CheckAll" runat="server" onClick="CheckAll(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="RouteSelector" runat="server" onClick="Check_Click(this);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ROWNUMBER" HeaderText="No." />
                            <asp:TemplateField HeaderText="&nbsp; Route Code&nbsp;" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="P5sEdit" runat="server" CommandName="Edit" Text='<%# Eval("ROUTE_CODE") %>'
                                        CommandArgument="<%# Container.DataItemIndex %>" CausesValidation="False">Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ROUTE_NAME" HeaderText=" &nbsp;Route Name &nbsp;" />
                            <asp:BoundField DataField="DISTRIBUTOR_DESC" HeaderText=" &nbsp;Distributor&nbsp;" />
                            <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="ROUTE_FREQ" HeaderText="Frequency" />
                            <asp:BoundField DataField="ROWNUMBER" HeaderText="#" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    &nbsp; Active &nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="P5sCheck" Checked='<%# Eval("ACTIVE")  %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="SALES_ROUTE_CD" HeaderText="" />
                            <asp:BoundField DataField="ROUTE_CODE" HeaderText="" />
                            <asp:BoundField DataField="ROUTE_NAME" HeaderText="" />
                            <asp:BoundField DataField="ROUTE_FREQ" HeaderText="" />
                            <asp:BoundField DataField="SEQUENCE" HeaderText="" />
                            <asp:BoundField DataField="ACTIVE" HeaderText="" />
                            <asp:BoundField DataField="ROUTE_CD" HeaderText="" />
                            <asp:BoundField DataField="SALES_CD" HeaderText="" />
                            <asp:BoundField DataField="ROUTE_CD" HeaderText="" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                <asp:HiddenField ID="P5sHdfPopup" runat="server" />
                <asp:Panel ID="P5sPnlTabControl" runat="server" BackColor="#DCDCDC" Width="450px"
                    Height="90px" Style="padding: 0px; display: none;" Visible="true" CssClass="PanelPopup"
                    orderStyle="solid" BorderWidth="2px">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr style="width: 100%; text-align: center; line-height: 30px; height: 30px;">
                            <td style="width: 450px">
                                <asp:Panel ID="Panel2" runat="server" BackColor="#87CEFA" Height="30px">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="white-space: nowrap; width: 99%">
                                                <asp:Label ID="Label3" runat="server" Text="Selete new DSR"></asp:Label></td>
                                            <td style="white-space: nowrap; padding-right: 0px; width: 1%">
                                                <asp:ImageButton ID="P5sImgCanncelConfirmPopup" runat="server" ImageUrl="~/images/delete.png"
                                                    Width="20px" Height="20px" /></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0">
                        <tr style="width: 100%">
                            <td style="width: 1%; height: 26px; white-space: nowrap">
                                DSR:<span style="color: Red;"> * </span>:
                            </td>
                            <td style="width: 1%; height: 26px; white-space: nowrap">
                                <table cellpadding="0" cellspacing="0" border="0" width="295px">
                                    <tr>
                                        <td style="padding: 0">
                                            <asp:TextBox ID="P5sTxtSELECT_NEW_SALE" runat="server" Style="width: 250px; padding: 0"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 98%; height: 26px; white-space: nowrap">
                                &nbsp;
                            </td>
                        </tr>
                        <tr style="width: 100%;">
                            <td colspan="3" style="width: 100%; height: 19px;" align="center">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnConfirmMoveRoute)%>
                                <asp:LinkButton ID="P5sLbtnConfirmMoveRoute" runat="server" OnClick="P5sLbtnConfirmMoveRoute_Click"
                                    OnClientClick="if(!confirm('Are you sure?')) return false;">
                                Confirm</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnConfirmMoveRoute)%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="P5sHdfPopup"
                    PopupControlID="P5sPnlTabControl" CancelControlID="P5sImgCanncelConfirmPopup"
                    BackgroundCssClass="ModalPopupBG">
                </ajaxToolkit:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-------------End Main GV  ----------------------->
    </form>
</asp:Content>
