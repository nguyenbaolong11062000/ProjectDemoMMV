<%@ Page Title="" Language="C#" MasterPageFile="~/P5s2.Master" AutoEventWireup="true" CodeBehind="TransferPage.aspx.cs" Inherits="MMV.fsmdls.TransferPage.TransferPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ToolkitScriptManager" runat="server" AsyncPostBackTimeout="0" />
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <%--<ContentTemplate>
                    <asp:LinkButton ID="btnTranfer" runat="server" Text="btuton click" OnClick="TranferPassCodeclick" />
                </ContentTemplate>
                <Triggers>
                     <asp:PostBackTrigger ControlID="btnTranfer" />
                </Triggers>--%>
            </asp:UpdatePanel>
            
        </div>
    </form>
     
</asp:Content>
