<%@ Page Title="" Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" CodeBehind="DistributorDemo1.aspx.cs" Inherits="MMV.MasterData.DistributorDemo1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <!-------------Begin Search  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelSearch" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="background-color: #999999">
                    <tr>
                        <td colspan="6">
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%" class="sectionheader">
                            <%=MMV.L5sMaster.L5sLangs["Search"]   %>
                        </td>
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
                            <asp:LinkButton ID="P5sSearch" runat="server" CausesValidation="False" OnClick="P5sSearch_Click"><%=MMV.L5sMaster.L5sLangs["Search"]   %></asp:LinkButton>
                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sSearch)%>
                        </td>
                        <td style="width: 78%">
                            &nbsp;
                        </td>
                        <td style="width: 1%">
                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnNew)%>
                            <asp:LinkButton ID="P5sLbtnNew" runat="server" CausesValidation="false" OnClick="P5sLbtnNew_Click"><%=MMV.L5sMaster.L5sLangs["Add New"]   %></asp:LinkButton>
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
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td style="width: 100%">
                            <asp:Panel ID="P5sDetailPanel" runat="server" Width="100%" Visible="false">
                                <p style="font-weight: bold; margin-left: 9px">
                                    </p>
                                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                                <table cellpadding="0" cellspacing="5" border="0" width="100%">
                                    <tr>
                                        <td colspan="3" style="font-style: italic; color: green;">
                                            <%=MMV.L5sMaster.L5sLangs["All fields marked with an asterisk (<b style='color: Red; font-size: large'>*</b>)are required."]   %><br />
                                            <br />
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Distributor Code"]   %><span style="color: Red;"> * </span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:TextBox ID="P5sTxtDISTRIBUTOR_CODE" runat="server" CssClass="TextBox" Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="P5sTxtDISTRIBUTOR_CODE" ForeColor="Red"
                                                ></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Distributor Name"]   %><span style="color: Red;"> * </span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:TextBox ID="P5sTxtDISTRIBUTOR_NAME" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="P5sTxtDISTRIBUTOR_NAME" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Type"]   %><span style="color: Red;"> * </span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:DropDownList ID="P5sDdlDistributorType" Width="150px" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="P5sDdlDistributorType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <%--test visible true--%>
                                    <asp:Panel ID="P5sPnlUnderDistributor" runat="server" Visible="false">
                                        <tr>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                                <%=MMV.L5sMaster.L5sLangs["Under Distributor"]   %><span style="color: Red;"> * </span>
                                            </td>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                                <table cellpadding="0" cellspacing="0" border="0" width="350px">
                                                    <tr>
                                                        <td style="padding: 0; width: 98%; white-space: nowrap">
                                                            <asp:TextBox ID="P5sTxtDISTRIBUTOR_CD" runat="server" CssClass="TextBox" Width="300px">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td style="padding: 0; width: 1%; white-space: nowrap">
                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sConfirm)%>
                                                            <asp:LinkButton ID="P5sConfirm" runat="server" CausesValidation="false"><%=MMV.L5sMaster.L5sLangs["Confirm"]   %></asp:LinkButton>
                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sConfirm)%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            </td>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            </td>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            </td>
                                            <td style="white-space: nowrap; width: 96%; vertical-align: top">
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Region"]   %><span style="color: Red;"> * </span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:DropDownList ID="P5sDdlREGION_CD" Width="100px" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="P5sDdlREGION_CD_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Area"]   %><span style="color: Red;"> * </span>
                                        </td>
                                         
                                        <td style="white-space: nowrap; vertical-align: top">
                                            <asp:DropDownList ID="P5sDdlAREA_CD" Width="100px" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="P5sDdlAREA_CD_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="P5sHdfAREA_CD" runat="server" Visible="false" />
                                        </td>
                                        
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Province"]   %><span style="color: Red;">&nbsp;</span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <table cellpadding="0" cellspacing="0" border="0" width="300px">
                                                <tr>
                                                    <td style="padding: 0">
                                                        <asp:TextBox ID="P5sTxtPROVINCE_CD" runat="server" CssClass="TextBox" MaxLength="50" 
                                                            Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            &nbsp;</td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["District"]   %><span style="color: Red;">&nbsp; </span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <table cellpadding="0" cellspacing="0" border="0" width="300px">
                                                <tr>
                                                    <td style="padding: 0">
                                                        <asp:TextBox ID="P5sTxtDISTRICT_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                            Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            &nbsp;</td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Ward"]   %><span style="color: Red;"><asp:Label ID="lbWard" runat="server"> * </asp:Label>  </span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <table cellpadding="0" cellspacing="0" border="0" width="300px">
                                                <tr>
                                                    <td style="padding: 0">
                                                        <asp:TextBox ID="P5sTxtCOMMUNE_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                            Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="P5sTxtCOMMUNE_CD" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Address"]%> <span style="color: Red;">*</span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:TextBox ID="P5sTxtDISTRIBUTOR_ADDRESS" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="P5sTxtDISTRIBUTOR_ADDRESS" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <%=MMV.L5sMaster.L5sLangs["Geocode"]   %>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:TextBox ID="P5sTxtLongTude" runat="server" CssClass="TextBox" Width="150px"></asp:TextBox>
                                            <span style="color: Green;">Ex: 12.7007825,108.054674 </span>
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="P5sTxtLongTude" ForeColor="Red" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:CheckBox ID="P5sACTIVE" runat="server" Checked="True" />
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
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
                                                        <asp:LinkButton ID="P5sLbtnInsert" runat="server" OnClick="P5sLbtnInsert_Click"><%=MMV.L5sMaster.L5sLangs["Save"]   %></asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnInsert)%>
                                                    </td>
                                                    <td style="width: 5px">
                                                        &nbsp;</td>
                                                    <td style="width: 27px">
                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnUpdate)%>
                                                        <asp:LinkButton ID="P5sLbtnUpdate" runat="server" OnClick="P5sLbtnUpdate_Click"><%=MMV.L5sMaster.L5sLangs["Update"]   %></asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnUpdate)%>
                                                    </td>
                                                    <td style="width: 5px">
                                                        &nbsp;</td>
                                                    <td>
                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnDelete)%>
                                                        <asp:LinkButton ID="P5sLbtnDelete" runat="server" OnClick="P5sLbtnDelete_Click"><%=MMV.L5sMaster.L5sLangs["Delete"]   %></asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnDelete)%>
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td>
                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnCancel)%>
                                                        <asp:LinkButton ID="P5sLbtnCancel" runat="server" OnClick="P5sLbtnCancel_Click" CausesValidation="False"><%=MMV.L5sMaster.L5sLangs["Cancel"]   %></asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnCancel)%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="white-space: nowrap; width: 97%; vertical-align: top">
                                            &nbsp;
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
                <asp:Panel ID="P5sPanelMainGV" runat="server">
                    <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                    <asp:DropDownList ID="P5sDdlMainPaging" runat="server" CssClass="dropdown" AutoPostBack="True">
                    </asp:DropDownList>
                    <br />
                    <asp:GridView ID="P5sMainGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="DISTRIBUTOR_CD"
                        CssClass="gridview" OnRowDataBound="P5sMainGridView_RowDataBound" OnRowEditing="P5sMainGridView_RowEditing">
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="P5sEdit" runat="server" CommandName="Edit" Text='<%# Eval("DISTRIBUTOR_CODE") %>'
                                        CommandArgument="<%# Container.DataItemIndex %>" CausesValidation="False">Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Label ID="lb1" runat="server" Text='<%# Eval("DISTRIBUTOR_NAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                               <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Label ID="lb2" runat="server" Text='<%# Eval("DISTRIBUTOR_TYPE_CODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                               <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Label ID="lb3" runat="server" Text='<%# Eval("DISTRIBUTOR_ADDRESS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                    <%--        <asp:BoundField DataField="DISTRIBUTOR_NAME" />
                            <asp:BoundField DataField="DISTRIBUTOR_TYPE_CODE" />
                            <asp:BoundField DataField="DISTRIBUTOR_ADDRESS" />
                            <asp:BoundField DataField="DISTRIBUTOR_ADDRESS" Visible="false" />--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="P5sCheck" Checked='<%# Eval("ACTIVE")  %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                         <%--   <asp:BoundField DataField="DISTRIBUTOR_CD" HeaderText="#" />
                            <asp:BoundField DataField="DISTRIBUTOR_CODE" HeaderText="#" />
                            <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="#" />
                            <asp:BoundField DataField="REGION_CD" HeaderText="#" />
                            <asp:BoundField DataField="REGION_CD" HeaderText="#" />
                            <asp:BoundField DataField="AREA_CD" HeaderText="#" />
                            <asp:BoundField DataField="AREA_CD" HeaderText="#" />
                            <asp:BoundField DataField="PROVINCE_CD" HeaderText="#" />
                            <asp:BoundField DataField="PROVINCE_CD" HeaderText="#" />
                            <asp:BoundField DataField="DISTRICT_CD" HeaderText="#" />
                            <asp:BoundField DataField="COMMUNE_CD" HeaderText="#" />
                            <asp:BoundField DataField="DISTRIBUTOR_ADDRESS" HeaderText="#" />
                            <asp:BoundField DataField="LONGITUDE_LATITUDE" HeaderText="#" />
                            <asp:BoundField DataField="ACTIVE" HeaderText="#" />
                            <asp:BoundField DataField="DISTRIBUTOR_TYPE_CD" HeaderText="#" />
                            <asp:BoundField DataField="DISTRIBUTOR_PARENT_CD" HeaderText="#" />--%>
                        </Columns>
                    </asp:GridView>
                    <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-------------End Main GV  ----------------------->
        </form>
</asp:Content>
