<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAuthGoogle.aspx.cs" Inherits="MMV.fsmdls.GoogleSheet.CreateAuthGoogle" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <table>
            <tbody>
                <tr>
                    <td></td>
                    <td style="font-weight: bold;">Hướng dẫn setup Auth cho Google sheet</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Bước 1</td>
                    <td>Vào drive, tạo một folder dùng để upload file, để folder ở chế độ share, lấy key của folder nhập vào textbox dưới (để trống nếu key đã lưu trên db)</td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:TextBox ID="txtFolderKey" runat="server"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Bước 2</td>
                    <td>Chọn một file ngẩu nhiên để upload</td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:FileUpload ID="fileUpload" runat="server" />
                        <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" Text="Upload" Style="box-shadow: 0 0 10px rgba(0,0,0,.5); border: none" />
                    </td>
                    <td></td>
                </tr>

                <tr>
                    <td>Bước 3</td>
                    <td>Đăng nhập tài khoản google và cấp quyền để upload</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Bước 4</td>
                    <td>Tài khoản sau khi cấp quyền là có thể upload file, reload trang để tiến hành upload</td>
                    <td></td>
                </tr>
                <tr>
                    <td>Link file</td>
                    <td>
                        <a id="lbtnRunPage" runat="server"></a>
                    </td>
                    <td></td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
