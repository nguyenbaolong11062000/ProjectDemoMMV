<%@ Page Title="" Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" CodeBehind="RunScriptDataSQL.aspx.cs" Inherits="MMV.CPTH.DashboardTH.NewDashboard.RunScriptDataSQL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
      <form id="frmMain" runat="server">
      <div style="border-style: solid; border-color: darkgray; border-width: 1px;padding-left:15px;padding-bottom:20px;">
        <asp:Label ID="lbHeader" runat="server"><h2>Run Script</h2></asp:Label>
            <tr>
                <td class="labeltxt" style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">Choose Script <span style="color: Red; white-space: nowrap;">*</span></td>
                <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top;">
                    <asp:DropDownList runat="server" ID="P5stxtScript" Height="35px" Width="224px">
                        <asp:ListItem Value="1" Text="Create table dashboard" />
                        <asp:ListItem Value="2" Text="Create store procedure 'Consolidate_D_Coverage_Buying_StoreVisit'" />
                        <asp:ListItem Value="3" Text="Create store procedure 'Consolidate_D_MSS_DISTRIBUTION'" />
                        <asp:ListItem Value="4" Text="Create store procedure 'Consolidate_MTD_TO_D_MTD_ACT_TARGET'" />
                        <asp:ListItem Value="5" Text="Create temp table reports MSS" />
                        <asp:ListItem Value="6" Text="Create store procedure 'CONSOLIDATE_REPORTS_MSS'" />
                    </asp:DropDownList>
                    <asp:Button ID="P5stxtChoose" runat="server" Text="Create" style="color: #FFF;text-shadow: 0 -1px 0 rgba(0,0,0,.25);background-image: none; border: 5px solid #428BCA; transition: background-color .15s,border-color .15s,opacity .15s;cursor: pointer;vertical-align: middle; box-shadow: none!important;font-size: 13px;padding: 4px 9px;line-height: 1.38;background-color: #428BCA; border-radius: 6px;display: inline-block; margin-left: 23px;" OnClick="P5stxtChoose_Click" Height="35px" Width="99px"/>
                </td>
                <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top;"></td>
            </tr>
          <br/><br/>
            <tr>
                <td>
                     <asp:TextBox id="_TxtLog" TextMode="multiline" Columns="70" Rows="12" runat="server" Height="45px" />
                </td>
            </tr>
       </div>
     </form>
</asp:Content>
