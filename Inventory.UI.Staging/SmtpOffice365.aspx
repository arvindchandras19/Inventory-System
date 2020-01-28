<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmtpOffice365.aspx.cs" Inherits="Inventory.UI.Staging.SmtpOffice365" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Testing SMTP OFFICE 365</h2>
    <span>Click the button to send mail    --->  </span>
        <br />
        <asp:Button ID="btnsend" runat="server" Text="Send Mail" OnClick="btnsend_Click" />
       <br />
        <asp:Button ID="btnsendAttach" runat="server" Text="Send Mail with Attachment" OnClick="btnsendAttach_Click" />
    </div>
        <div>
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
