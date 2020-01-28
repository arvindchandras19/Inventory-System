<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="Inventory.LoginPage" %>

<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/HorizontalInven.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="JS/DropDown.js"></script>
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons" />
    <link rel="stylesheet" href="https://code.getmdl.io/1.3.0/material.indigo-pink.min.css" />
    <script src="https://code.getmdl.io/1.3.0/material.min.js"></script>
    <script>
        function hide() {
            $('#mpeaddnew').modal('hide');
        }
    </script>
</head>
<body style="background: linear-gradient(to top, rgba(255,255,255,1), rgba(116,154,187,1), rgba(116,154,187,1)); background-repeat: no-repeat;">
    <form runat="server">
        <div class="mdl-layout mdl-js-layout mdl-color--grey-100">
            <main class="mdl-layout__content">
                <img src="img/inventry_Logo.png" style="width:60%;height:15%;margin-left:15%;margin-bottom:2%;"/>
		<div class="mdl-card mdl-shadow--6dp" style="width:100%">
			<div class="mdl-card__title mdl-color--primary mdl-color-text--white">
				<div class="mdl-card__title-text">
                    <%--<!-- Right aligned menu below button -->
<button id="demo-menu-lower-right" class="mdl-button mdl-js-button mdl-button--icon"> <i class="material-icons">more_vert</i></button>

<ul class="mdl-menu mdl-menu--bottom-right mdl-js-menu mdl-js-ripple-effect" for="demo-menu-lower-right">
  <li class="mdl-menu__item">Some Action</li>
  <li class="mdl-menu__item">Another Action</li>
  <li aria-disabled="true" class="mdl-menu__item">Disabled Action</li>
  <li class="mdl-menu__item">Yet Another Action</li>
</ul>--%>
				</div>
			</div>
	  	<div class="mdl-card__supporting-text">
				<div runat="server" action="#">
					<div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">						
                        <asp:TextBox ID="txtuser" runat="server" CssClass="mdl-textfield__input"></asp:TextBox>
                        <asp:Label runat="server" class="mdl-textfield__label" for="txtuser" Text="Username"></asp:Label>						
					</div>
					<div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">						
                        <asp:TextBox ID="txtpswd" runat="server" TextMode="password" CssClass="mdl-textfield__input"></asp:TextBox>
                        <asp:Label runat="server" class="mdl-textfield__label" for="txtpswd" Text="Password"></asp:Label>						
					</div>
				</div>
			</div>
			<div class="mdl-card__actions mdl-card--border">
                <asp:Button ID="btnlogin" runat="server" Text="Log in" CssClass="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect" OnClick="btnlogin_Click"/>
				<asp:Button ID="btnforgot" runat="server" Text="Forgot password?" CssClass="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect"/>                		
		</div>
            </div>
	</main>
        </div>

        <%--<div class="modal modal-wide fade" id="mpeaddnew" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header" style="height: 50px">
                        <span class="mdl-chip mdl-chip--contact mdl-chip--deletable" >
                            <img class="mdl-chip__contact" src="Images/img.jpg"/>
                            <span class="mdl-chip__text">Add User Details</span>                            
                        </span>
                        <button type="button" class="close mdl-button mdl-js-button mdl-button--icon mdl-button--colored" data-dismiss="modal" style="font-weight: 900" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal" runat="server" id="addmodal" method="post">
                            <div runat="server" action="#">
                                <div class="col-md-12">
                                    <asp:Label ID="lblfnerror" runat="server" ClientIDMode="Static"></asp:Label>
                                    <asp:Label ID="lbllnerror" runat="server" ClientIDMode="Static"></asp:Label>
                                    <asp:Label ID="lblunerror" runat="server" ClientIDMode="Static"></asp:Label>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                            <asp:Label ID="lblFirstname" runat="server" CssClass="control-label mdl-textfield__label" for="txtFirstName" Text="First Name"></asp:Label>
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="mdl-textfield__input" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                            <asp:Label ID="lblLastname" runat="server" CssClass="control-label mdl-textfield__label" for="txtLastName" Text="Last Name"></asp:Label>
                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="mdl-textfield__input" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblemailerror" runat="server" ClientIDMode="Static"></asp:Label>
                                    <asp:Label ID="lblpwderror" runat="server" ClientIDMode="Static"></asp:Label>
                                    <asp:Label ID="lblrepwderror" runat="server" ClientIDMode="Static"></asp:Label>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                            <asp:Label ID="lblUsername" runat="server" CssClass="control-label mdl-textfield__label" for="txtUserName" Text="User Name"></asp:Label>
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="mdl-textfield__input" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                            <asp:Label ID="lblEmail" runat="server" CssClass="control-label mdl-textfield__label" for="txtEmail" Text="Email"></asp:Label>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="mdl-textfield__input" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                            <asp:Label ID="lblPassword" runat="server" CssClass="control-label mdl-textfield__label" for="txtPassword" Text="Password"></asp:Label>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="password" CssClass="mdl-textfield__input" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                            <asp:Label ID="lblRePassword" runat="server" CssClass="control-label mdl-textfield__label" for="txtReenter" Text="Re Enter Password"></asp:Label>
                                            <asp:TextBox ID="txtReenter" runat="server" TextMode="password" CssClass="mdl-textfield__input" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                            <asp:Label ID="lblOfficePhone" runat="server" CssClass="control-label mdl-textfield__label" for="txtOfficePhone" Text="Office Phone"></asp:Label>
                                            <asp:TextBox ID="txtOfficePhone" runat="server" CssClass="mdl-textfield__input"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                                            <asp:Label ID="lblMobilePhone" runat="server" CssClass="control-label mdl-textfield__label" for="txtMobilePhone" Text="Mobile Phone"></asp:Label>
                                            <asp:TextBox ID="txtMobilePhone" runat="server" CssClass="mdl-textfield__input"></asp:TextBox>                                            
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="display: none;">
                                    <div class="col-md-6" runat="server" action="#">
                                        <div class="mdl-select mdl-js-select mdl-select--floating-label">
                                            <asp:Label ID="lblspeciality" runat="server" Visible="false" CssClass="mdl-select__label" Text="Speciality" for="drpspeciality"></asp:Label>
                                            <asp:DropDownList ID="drpspeciality" runat="server" CssClass="mdl-select__input">
                                                <asp:ListItem>XXX</asp:ListItem>
                                                <asp:ListItem>YYY</asp:ListItem>
                                                <asp:ListItem>ZZZ</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4" style="margin-top: 5%">
                                        <div class="filters__container">
                                            <asp:CheckBox ID="chkIsAdministration" Text="Is Administration" runat="server" CssClass="filter__input mdl-checkbox__input" />
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField runat="server" ID="hdnfield" Value="0"></asp:HiddenField>
                            </div>
                            <asp:Label ID="lblmsg" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSave" runat="server" CssClass="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--accent" Text="Save" />
                        <asp:Button ID="btnclear" runat="server" CssClass="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--accent" Text="Cancel" OnClientClick="Validate();return clear();" />
                    </div>
                </div>
            </div>
        </div>--%>
    </form>
</body>

