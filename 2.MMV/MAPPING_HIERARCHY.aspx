<%@ Page Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" Codebehind="MAPPING_HIERARCHY.aspx.cs"
    Inherits="MMV.MAPPING_HIERARCHY" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="P5sUpanelSelectHierarchy" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td style="white-space: nowrap; width: 50%; vertical-align: top;">
                            <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                            <table cellpadding="0" cellspacing="1px" border="0" width="100%">
                                <tr>
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        <asp:Label ID="P5sLblPhanVungTheoCP" Font-Bold="true" runat="server" Text="Phân vùng theo CP"></asp:Label>
                                    </td>
                                    <td style="white-space: nowrap; width: 98%; vertical-align: top;">
                                        &nbsp</td>
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sAddNewCustomer)%>
                                        <asp:LinkButton ID="P5sAddNewCustomer" runat="server" OnClick="P5sAddNewCustomer_Click" >Thêm mới Khách Hàng</asp:LinkButton>
                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sAddNewCustomer)%>
                                    </td>
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                       <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnExportCP)%>
                                        <asp:LinkButton ID="P5sLbtnExportCP" runat="server" OnClick="P5sLbtnExportCP_Click">Báo cáo tình trạng cập nhật</asp:LinkButton>
                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnExportCP)%>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="1px" border="0" width="100%">
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        Vùng(Region)
                                    </td>
                                    <td style="white-space: nowrap; vertical-align: top; height: 25px; width: 400px">
                                        <table cellpadding="0" cellspacing="1px" border="0" width="400px">
                                            <tr style="height: 25px">
                                                <td style="white-space: nowrap; width: 1%; vertical-align: top; height: 25px">
                                                    <asp:DropDownList ID="P5sDdlREGION_CD" Width="100px" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="P5sDdlREGION_CD_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="white-space: nowrap; width: 97%; vertical-align: top; height: 19px;">
                                                </td>
                                                <td style="white-space: nowrap; width: 1%; vertical-align: top; height: 19px;">
                                                    Khu vực(Area)
                                                </td>
                                                <td style="white-space: nowrap; width: 1%; vertical-align: top; height: 25px">
                                                    <asp:DropDownList ID="P5sDdlAREA_CD" Width="100px" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="P5sDdlAREA_CD_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        Nhà phân phối(Distributor)
                                    </td>
                                    <td style="white-space: nowrap; vertical-align: top;">
                                        <table cellpadding="0" cellspacing="0" border="0" width="400px">
                                            <tr>
                                                <td style="padding: 0">
                                                    <asp:TextBox ID="P5sTxtDISTRIBUTOR_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                        Width="400px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        DSP
                                    </td>
                                    <td style="white-space: nowrap; vertical-align: top;">
                                        <table cellpadding="0" cellspacing="0" border="0" width="400px">
                                            <tr>
                                                <td style="padding: 0">
                                                    <asp:TextBox ID="P5sTxtSALES_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                        Width="400px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        Tuyến BH(Route list)
                                    </td>
                                    <td style="white-space: nowrap; vertical-align: top; width: 400px;">
                                        <table cellpadding="0" cellspacing="0" border="0" width="400px">
                                            <tr>
                                                <td style="padding: 0">
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td style="white-space: nowrap; vertical-align: top;">
                                                                <asp:TextBox ID="P5sTxtROUTE_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                                    Width="400px"></asp:TextBox>
                                                                      <%=P5sActROUTE.L5sShowAddAll("P5sTxtROUTE_CD_Add")%>
                                                                <%=P5sActROUTE.L5sShowRemoveAll("P5sTxtROUTE_CD_Remove")%>
                                                            </td>
                                                            <td style="white-space: nowrap; vertical-align: top;">
                                                              
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%;">
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <table cellpadding="0" cellspacing="0" width="1%">
                                            <tr>
                                                <td style="white-space: nowrap; width: 1%; height: 19px;">
                                                    <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnFindToMap)%>
                                                    <asp:LinkButton ID="P5sLbtnFindToMap" runat="server" OnClick="P5sLbtnFindToMap_Click"> Hiển thị </asp:LinkButton>
                                                    <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnFindToMap)%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="P5sUpanelUnMapGV" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="P5sLblUnMapTitle" Font-Italic="true" runat="server" Text="DSKH chưa chọn Phường/Xã"></asp:Label>
                                    <asp:Panel ID="P5sPnlRootUnMapGV" runat="server">
                                        <%--  <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>--%>
                                        <asp:Label ID="P5sLblEmptyRowUnMapGV" Font-Italic="true" ForeColor="green" runat="server"
                                            Text="Không có dữ liệu"></asp:Label>
                                        <asp:Panel ID="P5sPnlUnMapGV" runat="server">
                                            <table cellpadding="0" cellspacing="1px" border="0" width="100%">
                                                <tr>
                                                    <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                        <asp:DropDownList ID="P5sDdlUnMapPaging" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                            Visible="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top; text-align: right;">
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%" style="background-color: #D0D0D0">
                                                            <tr>
                                                                <td style="white-space: nowrap; width: 1%" class="sectionheader">
                                                                    Tìm kiếm</td>
                                                                <td>
                                                                    <td style="white-space: nowrap; width: 1%">
                                                                        <asp:TextBox ID="P5sFilterTxt" runat="server" CssClass="textbox"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 1%">
                                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sSearch)%>
                                                                        <asp:LinkButton ID="P5sSearch" runat="server" CausesValidation="False" OnClick="P5sSearch_Click">Tìm kiếm</asp:LinkButton>
                                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sSearch)%>
                                                                    </td>
                                                                    <td style="white-space: nowrap; width: 97%">
                                                                        &nbsp</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top;">
                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnMap)%>
                                                        <asp:LinkButton ID="P5sLbtnMap" runat="server" OnClick="P5sLbtnMap_Click">Chọn vào</asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnMap)%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                        <asp:GridView ID="P5sUnMapGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="CUSTOMER_CD"
                                                            CssClass="gridview" OnRowDataBound="P5sUnMapGridView_RowDataBound" OnRowCommand="P5sUnMapGridView_RowCommand"
                                                            OnRowEditing="P5sUnMapGridView_RowEditing">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="P5sChkAll" runat="server" onClick="CheckAll(this);" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="P5sChk" runat="server" onClick="Check_Click(this);" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CUSTOMER_CODE" HeaderText="&nbsp; Mã KH " HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="&nbsp; Tên KH" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="ROUTE_NAME" HeaderText="&nbsp; Mã Tuyến" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="CUSTOMER_CHAIN_CODE" HeaderText="Loại KH" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="CUSTOMER_ADDRESS" HeaderText="&nbsp; Địa chỉ &nbsp;" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="SALES_DESC" Visible="False" HeaderText="&nbsp; DSP &nbsp;"
                                                                    HeaderStyle-HorizontalAlign="Center" />
                                                                <%--Begin Image button Edit--%>
                                                                <asp:TemplateField HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="P5sLbtnEditAddress" CommandName="EditAddress" CommandArgument='<%# Container.DataItemIndex %>'
                                                                            runat="server" CausesValidation="False">
                                                                            <asp:Image ID="P5sImgLbtnEditAddress" Width="20px" Height="15px" ImageUrl="~/images/edit.png"
                                                                                runat="server" ImageAlign="Middle" BorderStyle="None" />
                                                                        </asp:LinkButton>
                                                                        <asp:HiddenField ID="P5sHdfTabControl" runat="server" />
                                                                        <asp:Panel ID="P5sPnlTabControl" runat="server" BackColor="#DCDCDC" Width="450px"
                                                                            Height="80px" Style="padding: 0px; display: none;" Visible="true">
                                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr style="width: 100%; text-align: center; line-height: 30px; height: 30px;">
                                                                                    <td>
                                                                                        <asp:Panel ID="Panel2" runat="server" BackColor="#87CEFA" Height="30px">
                                                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                                                <tr>
                                                                                                    <td style="white-space: nowrap; width: 99%">
                                                                                                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="THÔNG TIN KHÁCH HÀNG"></asp:Label></td>
                                                                                                    <td style="white-space: nowrap; padding-right: 0px; width: 1%">
                                                                                                        <asp:ImageButton ID="P5sImgCanncelConfirmPopup" runat="server" ImageUrl="~/images/delete.png"
                                                                                                            Width="20px" Height="20px" /></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <table cellpadding="5" cellspacing="0" style="padding: 0 20px;">
                                                                                <tr style="width: 100%; height: 26px;">
                                                                                    <td style="width: 1%; white-space: nowrap; text-align: left;">
                                                                                        Mã KH :<span style="color: Red;"> </span>
                                                                                    </td>
                                                                                    <td style="width: 1%; white-space: nowrap; text-align: left;">
                                                                                        <asp:Label ID="P5sLblCUSTOMER_CODE" runat="server" Text=""></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 98%; white-space: nowrap">
                                                                                        &nbsp</td>
                                                                                </tr>
                                                                                <tr style="width: 100%; height: 26px; text-align: left;">
                                                                                    <td style="width: 1%; white-space: nowrap">
                                                                                        Tên KH :<span style="color: Red;"> </span>
                                                                                    </td>
                                                                                    <td style="width: 1%; white-space: nowrap; text-align: left;">
                                                                                        <asp:Label ID="P5sLblCUSTOMER_NAME" runat="server" Text=""></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 98%; white-space: nowrap">
                                                                                        &nbsp</td>
                                                                                </tr>
                                                                                <tr style="width: 100%; height: 26px; text-align: left;">
                                                                                    <td style="width: 1%; white-space: nowrap">
                                                                                        Tuyến BH :<span style="color: Red;"> </span>
                                                                                    </td>
                                                                                    <td style="width: 1%; white-space: nowrap; text-align: left;">
                                                                                        <asp:Label ID="P5sLblROUTE_NAME" runat="server" Text=""></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 98%; white-space: nowrap">
                                                                                        &nbsp</td>
                                                                                </tr>
                                                                                <tr style="width: 100%; height: 26px; text-align: left;">
                                                                                    <td style="width: 1%; white-space: nowrap">
                                                                                        Địa chỉ :<span style="color: Red;"> * </span>
                                                                                    </td>
                                                                                    <td style="width: 1%; white-space: nowrap; text-align: left;">
                                                                                        <asp:TextBox ID="P5sTxtCUSTOMER_ADDRESS" EnableViewState="true" TextMode="MultiLine"
                                                                                            Width="400px" runat="server"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 98%; white-space: nowrap">
                                                                                        &nbsp</td>
                                                                                </tr>
                                                                                <tr style="width: 100%; height: 26px;">
                                                                                    <td colspan="3" style="width: 100%" align="center">
                                                                                        <asp:LinkButton ID="P5sLbtnConfirmPopup" OnClick="P5sLbtnConfirmPopup_Click" runat="server">Đồng Ý</asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                                                                        </asp:Panel>
                                                                        <ajaxToolkit:ModalPopupExtender ID="P5sModelPopupTabControl" runat="server" TargetControlID="P5sHdfTabControl"
                                                                            PopupControlID="P5sPnlTabControl" BackgroundCssClass="ModalPopupBG" CancelControlID="P5sImgCanncelConfirmPopup">
                                                                        </ajaxToolkit:ModalPopupExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--End Image button Edit--%>
                                                                <asp:BoundField DataField="DISTRIBUTOR_CD" HeaderText="#" />
                                                                <asp:BoundField DataField="SALES_CD" HeaderText="#" />
                                                                <asp:BoundField DataField="ROUTE_CD" HeaderText="#" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <%-- <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>--%>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                        </td>
                        <td style="white-space: nowrap; width: 50%; vertical-align: top;">
                            <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                            <table cellpadding="0" cellspacing="1px" border="0" width="100%">
                                <tr>
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        <asp:Label ID="P5sLblPhanVungTheoHanhChinhVN" Font-Bold="true" runat="server" Text="Phân vùng theo Hành chính VN"></asp:Label>
                                    </td>
                                    <td style="white-space: nowrap; width: 98%; vertical-align: top;">
                                        &nbsp</td>
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnExport)%>
                                        <asp:LinkButton ID="P5sLbtnExport" runat="server" OnClick="P5sLbtnExport_Click">Báo cáo tình trạng cập nhật</asp:LinkButton>
                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnExport)%>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="1px" border="0" width="100%">
                                <tr style="height: 25px">
                                    <td colspan="2" style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        Tỉnh/ Thành Phố(Province)</td>
                                    <td style="white-space: nowrap; vertical-align: top;">
                                        <table cellpadding="0" cellspacing="0" border="0" width="400px">
                                            <tr>
                                                <td style="padding: 0">
                                                    <asp:TextBox ID="P5sTxtPROVINCE_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                        Width="400px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        Quận/ Thị Xã/ Huyện(District)</td>
                                    <td style="white-space: nowrap; vertical-align: top;">
                                        <table cellpadding="0" cellspacing="0" border="0" width="400px">
                                            <tr>
                                                <td style="padding: 0">
                                                    <asp:TextBox ID="P5sTxtDISTRICT_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                        Width="400px"></asp:TextBox>
                                                            <%=P5sActDISTRICT.L5sShowAddAll("P5sTxtDISTRICT_CD_Add")%>
                                                             <%=P5sActDISTRICT.L5sShowRemoveAll("P5sTxtDISTRICT_CD_Remove")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%; vertical-align: top;">
                                        Phường / Xã/ Thị Trấn(Ward)</td>
                                    <td style="white-space: nowrap; vertical-align: top;">
                                        <table cellpadding="0" cellspacing="0" border="0" width="400px">
                                            <tr>
                                                <td style="padding: 0">
                                                    <asp:TextBox ID="P5sTxtCOMMUNE_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                                        Width="400px"></asp:TextBox>
                                                         <%=P5sActCOMMUNE.L5sShowAddAll("P5sTxtCOMMUNE_CD_Add")%>
                                                             <%=P5sActCOMMUNE.L5sShowRemoveAll("P5sTxtCOMMUNE_CD_Remove")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td style="white-space: nowrap; width: 1%;">
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <table cellpadding="0" cellspacing="0" width="1%">
                                            <tr>
                                                <td style="white-space: nowrap; width: 1%; height: 19px;">
                                                    <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnFindToHierarchy)%>
                                                    <asp:LinkButton ID="P5sLbtnFindToHierarchy" runat="server" OnClick="P5sLbtnFindToHierarchy_Click"> Hiển thị </asp:LinkButton>
                                                    <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnFindToHierarchy)%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                        <asp:UpdatePanel ID="P5sUpanelMapGV" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="P5sLblMapTitle" Font-Italic="true" runat="server" Text="Danh sách khách hàng"></asp:Label>
                                                <asp:Panel ID="P5sPnlRootMapGV" runat="server">
                                                    <%-- <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>--%>
                                                    <asp:Label ID="P5sLblEmptyRowMapGV" Font-Italic="true" ForeColor="green" runat="server"
                                                        Text="Không có dữ liệu"></asp:Label>
                                                    <asp:Panel ID="P5sPnlMapGV" runat="server">
                                                        <table cellpadding="0" cellspacing="1px" border="0" width="100%">
                                                            <tr>
                                                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    <asp:DropDownList ID="P5sDdlMapPaging" Visible="true" runat="server" CssClass="dropdown"
                                                                        AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top;">
                                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%" style="background-color: #D0D0D0">
                                                                        <tr>
                                                                            <td style="white-space: nowrap; width: 1%" class="sectionheader">
                                                                                Tìm kiếm</td>
                                                                            <td>
                                                                                <td style="white-space: nowrap; width: 1%">
                                                                                    <asp:TextBox ID="P5sFilterTxtHierarchyVN" runat="server" CssClass="textbox"></asp:TextBox>
                                                                                </td>
                                                                                <td style="width: 1%">
                                                                                    <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sBtnSearchHierarchyVN)%>
                                                                                    <asp:LinkButton ID="P5sBtnSearchHierarchyVN" runat="server" CausesValidation="False"
                                                                                        OnClick="P5sBtnSearchHierarchyVN_Click">Tìm kiếm</asp:LinkButton>
                                                                                    <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sBtnSearchHierarchyVN)%>
                                                                                </td>
                                                                                <td style="white-space: nowrap; width: 97%">
                                                                                    &nbsp</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top;">
                                                                    <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnUnMap)%>
                                                                    <asp:LinkButton ID="P5sLbtnUnMap" runat="server" OnClick="P5sLbtnUnMap_Click">Bỏ ra</asp:LinkButton>
                                                                    <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnUnMap)%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:GridView ID="P5sMapGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="CUSTOMER_CD"
                                                            CssClass="gridview" OnRowDataBound="P5sMapGridView_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="P5sChkAll" runat="server" onClick="CheckAll(this);" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="P5sChk" runat="server" onClick="Check_Click(this);" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CUSTOMER_CODE" HeaderText="&nbsp; Mã KH " HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="&nbsp; Tên KH" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="ROUTE_NAME" HeaderText="Mã Tuyến" />
                                                                <asp:BoundField DataField="CUSTOMER_CHAIN_CODE" HeaderText="Loại KH" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="CUSTOMER_ADDRESS" HeaderText="&nbsp; Địa chỉ &nbsp;" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="SALES_DESC" Visible="False" HeaderText="&nbsp; DSP &nbsp;"
                                                                    HeaderStyle-HorizontalAlign="Center" />
                                                                <%--Begin Image button Edit--%>
                                                                <asp:TemplateField HeaderText="" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="P5sLbtnEditAddress" CommandName="EditAddress" CommandArgument='<%# Container.DataItemIndex %>'
                                                                            runat="server" CausesValidation="False">
                                                                            <asp:Image ID="P5sImgLbtnEditAddress" Width="20px" Height="15px" ImageUrl="~/images/edit.png"
                                                                                runat="server" ImageAlign="Middle" BorderStyle="None" />
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--End Image button Edit--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <%-- <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>--%>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="P5sLbtnExport" />
                <asp:PostBackTrigger ControlID="P5sLbtnExportCP" />
                <asp:PostBackTrigger ControlID="P5sBtnSearchHierarchyVN" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>
