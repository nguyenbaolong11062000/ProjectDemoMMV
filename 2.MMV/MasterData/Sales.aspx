<%@ Page Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" Codebehind="Sales.aspx.cs"
    Inherits="MMV.MasterData.Sales" %>

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
                            <asp:LinkButton ID="P5sLbtnNew" runat="server" CausesValidation="false" OnClick="P5sLbtnNew_Click">Add New</asp:LinkButton>
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
                <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                    <tr>
                        <td style="width: 100%">
                            <asp:Panel ID="P5sDetailPanel" runat="server" Width="100%" Visible="False">
                                <p style="font-weight: bold; margin-left: 9px">
                                </p>
                                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                                <table cellpadding="0" cellspacing="5" border="0" width="100%">
                                    <tr>
                                        <td colspan="4" style="font-style: italic; color: green;">
                                            All fields marked with an asterisk (<span style="color: Red;">*</span>) are required<br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            DSR Code<span style="color: Red;"> * </span>:
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: top; width: 1%">
                                            <asp:TextBox ID="P5sTxtSALES_CODE" runat="server" CssClass="TextBox" Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="white-space: nowrap; width: 99%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="P5sTxtSALES_CODE"
                                                ErrorMessage="The DSR code field is required!"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            DSR Name <span style="color: Red;">* </span>:
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: top; width: 1%">
                                            <asp:TextBox ID="P5sTxtSALES_NAME" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                        </td>
                                        <td style="white-space: nowrap; width: 99%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="P5sTxtSALES_NAME"
                                                ErrorMessage="The DSR name field is required!"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            Distributor<span style="color: Red;"> * </span>:
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: top; width: 1%">
                                            <table cellpadding="0" cellspacing="0" border="0" width="305px">
                                                <tr>
                                                    <td style="padding: 0">
                                                        <asp:TextBox ID="P5sTxtDISTRIBUTOR_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                            Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="white-space: nowrap; width: 99%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="P5sTxtDISTRIBUTOR_CD"
                                                ErrorMessage="The Distributor field is required!"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            Type<span style="color: Red;"> * </span>:
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: top; width: 1%">
                                            <table cellpadding="0" cellspacing="0" border="0" width="305px">
                                                <tr>
                                                    <td style="padding: 0">
                                                        <asp:TextBox ID="P5sTxtSALES_TYPE_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                            Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="white-space: nowrap; width: 99%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="P5sTxtSALES_TYPE_CD"
                                                ErrorMessage="The type field is required!"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            &nbsp
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: top; width: 1%">
                                            <asp:CheckBox ID="P5sACTIVE" runat="server" Text="Active" Checked="True" />
                                        </td>
                                        <td style="white-space: nowrap; width: 99%; vertical-align: top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                        </td>
                                        <td style="white-space: nowrap; vertical-align: top">
                                            <table cellpadding="0" cellspacing="0" width="1%">
                                                <tr>
                                                    <td>
                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnInsert)%>
                                                        <asp:LinkButton ID="P5sLbtnInsert" runat="server" OnClick="P5sLbtnInsert_Click">Save</asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnInsert)%>
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td>
                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnUpdate)%>
                                                        <asp:LinkButton ID="P5sLbtnUpdate" runat="server" OnClick="P5sLbtnUpdate_Click">Update</asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnUpdate)%>
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td>
                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnCancel)%>
                                                        <asp:LinkButton ID="P5sLbtnCancel" runat="server" OnClick="P5sLbtnCancel_Click" CausesValidation="False">Cancel</asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnCancel)%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-------------End Detail  ----------------------->
        <br />
        <!-------------Begin Main GV  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelMainGV" runat="server">
            <ContentTemplate>
            <asp:Label ID="P5sLblEmptyRow" Font-Italic="true" ForeColor="green" runat="server"
                    Text=""></asp:Label>
                <asp:Panel ID="P5sPanelMainGV" runat="server">
                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                    <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                        <tr>
                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                <asp:DropDownList ID="P5sDdlMainPaging" runat="server" CssClass="dropdown" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="white-space: nowrap; width: 2%; vertical-align: top">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnChangeUseDMS)%>
                                <asp:LinkButton ID="P5sLbtnChangeUseDMS" runat="server" OnClientClick="return confirm('Are you sure to change these selected Sales?');"
                                    CausesValidation="False" OnClick="P5sLbtnChangeUseDMS_Click">Active/Deactive DMS Use</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnChangeUseDMS)%>
                            </td>
                            <td style="white-space: nowrap; width: 2%; vertical-align: top">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnChangeOutRangeOrder)%>
                                <asp:LinkButton ID="P5sLbtnChangeOutRangeOrder" runat="server" OnClientClick="return confirm('Are you sure to change these selected Sales?');"
                                    CausesValidation="False" OnClick="P5sLbtnChangeOutRangeOrder_Click">Active/Deactive Out Range Order</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnChangeOutRangeOrder)%>
                            </td>
                             <td style="white-space: nowrap; width: 2%; vertical-align: top">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnChange5PTracking)%>
                                <asp:LinkButton ID="P5sLbtnChange5PTracking" runat="server" OnClientClick="return confirm('Are you sure to change these selected Sales?');"
                                    CausesValidation="False" OnClick="P5sLbtnChange5PTracking_Click">Active/Deactive 5P Tracking</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnChange5PTracking)%>
                            </td>
                            <td style="white-space: nowrap; width: 2%; vertical-align: top">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnChangeSurvey)%>
                                <asp:LinkButton ID="P5sLbtnChangeSurvey" runat="server" OnClientClick="return confirm('Are you sure to change these selected Sales?');"
                                    CausesValidation="False" OnClick="P5sLbtnChangeSurvey_Click">Active/Deactive Survey</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnChangeSurvey)%>
                            </td>
                            <td style="white-space: nowrap; width: 93%; vertical-align: top">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnChangeEditInfor)%>
                                <asp:LinkButton ID="P5sLbtnChangeEditInfor" runat="server" OnClientClick="return confirm('Are you sure to change these selected Sales?');"
                                    CausesValidation="False" OnClick="P5sLbtnChangeEditInfor_Click">Active/Deactive EditInfor</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnChangeEditInfor)%>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="P5sMainGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="SALES_CD"
                        CssClass="gridview" OnRowDataBound="P5sMainGridView_RowDataBound" OnRowEditing="P5sMainGridView_RowEditing"
                        OnRowCommand="P5sMainGridView_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="CheckAll" runat="server" onClick="CheckAll(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="SalesSelector" runat="server" onClick="Check_Click(this);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="&nbsp;DSR Code&nbsp;" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="P5sEdit" runat="server" CommandName="Edit" Text='<%# Eval("SALES_CODE") %>'
                                        CommandArgument="<%# Container.DataItemIndex %>" CausesValidation="False">Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText=" &nbsp;DSR Name &nbsp;">
                                <ItemTemplate>
                                    <asp:Label ID="LB1" runat="server" text='<%# Eval("SALES_NAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText=" &nbsp;Distributor &nbsp;">
                                <ItemTemplate>
                                    <asp:Label ID="LB2" runat="server" text='<%# Eval("DISTRIBUTOR_DESC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:BoundField DataField="SALES_NAME" HeaderText=" &nbsp;DSR Name &nbsp;" />
                            <asp:BoundField DataField="DISTRIBUTOR_DESC" HeaderText=" &nbsp;Distributor &nbsp;" />--%>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    &nbsp; Active &nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="P5sCheck" Checked='<%# Eval("ACTIVE")  %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    &nbsp; DMS &nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="P5sUSE_DMSCheck" Checked='<%# Eval("USE_DMS")  %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    &nbsp; Out range &nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="P5sALLOW_OUT_RANGE_ORDERCheck" Checked='<%# Eval("ALLOW_OUT_RANGE_ORDER")  %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    &nbsp; 5P Tracking &nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="P5sUSE_5PTRACKINGCheck" Checked='<%# Eval("USE_5PTRACKING")  %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    &nbsp; Survey &nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="P5sUSE_SURVEYCheck" Checked='<%# Eval("USE_SURVEY")  %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    &nbsp; EditInfor &nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="P5sUSE_EditInforCheck" Checked='<%# Eval("EDIT_INFO")  %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <%--Begin Image button them tuyến--%>
                            <asp:TemplateField HeaderText="Add route">
                                <ItemTemplate>
                                    <asp:LinkButton ID="P5sLbtnAddRoute" CommandName="AddRoute" CommandArgument='<%# Container.DataItemIndex %>'
                                        runat="server" ToolTip="Add route" CausesValidation="False">
                                        <asp:Image ID="P5sImgLbtnAddRoute" Width="20px" Height="15px" ImageUrl="~/images/dtranfer1.png"
                                            runat="server" ImageAlign="Middle" BorderStyle="None" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--End Image button them tuyến--%>
                            <asp:BoundField DataField="SALES_CD" HeaderText="#" />
                            <asp:BoundField DataField="SALES_CODE" HeaderText="#" />
                            <asp:BoundField DataField="SALES_NAME" HeaderText="#" />
                            <asp:BoundField DataField="DISTRIBUTOR_CD" HeaderText="#" />
                            <asp:BoundField DataField="ACTIVE" HeaderText="#" />
                           
                            <asp:BoundField DataField="SALES_TYPE_CD" HeaderText="#" />
                        </Columns>
                    </asp:GridView>
                    <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-------------End Main GV  ----------------------->
    </form>
</asp:Content>
