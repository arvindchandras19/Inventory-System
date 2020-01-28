<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Inventory.Login" %>
<!DOCTYPE html>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : Login.aspx 
' Type      : ASPX File 
' Description  :   To design the Login Page.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01              Dhanasekran.C                     New
  08/18/2017           V.01              Dhanasekar                         Added  Scripts/jquery-1.9.1.min.js reference
'******************************************************************************************************/
--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Form</title>
    <%--<link rel="stylesheet" href="css/style.css" />--%>
    <link href="Dashboard/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="Dashboard/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <style type="text/css">
        body, html {
            height: 100%;
            background-repeat: no-repeat;
            /*background-image: linear-gradient(rgb(104, 145, 162), rgb(12, 97, 33));*/
            background-image: url(Images/inven.jpg);
        }

        .card-container.card {
            max-width: 350px;
            padding: 40px 40px;
        }

        .btn {
            font-weight: 700;
            height: 36px;
            -moz-user-select: none;
            -webkit-user-select: none;
            user-select: none;
            cursor: default;
        }

        /*
 * Card component
 */
        .card {
            background-color: #F7F7F7;
            /* just in case there no content*/
            padding: 20px 25px 30px;
            margin: 0 auto 25px;
            margin-top: 50px;
            /* shadows and rounded borders */
            -moz-border-radius: 2px;
            -webkit-border-radius: 2px;
            border-radius: 2px;
            -moz-box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
            box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
        }

        .profile-img-card {
            width: 96px;
            height: 96px;
            margin: 0 auto 10px;
            display: block;
            -moz-border-radius: 50%;
            -webkit-border-radius: 50%;
            border-radius: 50%;
        }

        /*
 * Form styles
 */
        .profile-name-card {
            font-size: 16px;
            font-weight: bold;
            text-align: center;
            margin: 10px 0 0;
            min-height: 1em;
        }

        .reauth-email {
            display: block;
            color: #404040;
            line-height: 2;
            margin-bottom: 10px;
            font-size: 14px;
            text-align: center;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
        }

        .form-signin #inputEmail,
        .form-signin #inputPassword {
            direction: ltr;
            height: 44px;
            font-size: 16px;
        }

        .form-signin input[type=email],
        .form-signin input[type=password],
        .form-signin input[type=text],
        .form-signin button {
            width: 100%;
            display: block;
            margin-bottom: 10px;
            z-index: 1;
            position: relative;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
        }

        .form-signin .form-control:focus {
            border-color: rgb(104, 145, 162);
            outline: 0;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgb(104, 145, 162);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgb(104, 145, 162);
        }

        .btn.btn-signin {
            /*background-color: #4d90fe; */
            background-color: rgb(104, 145, 162);
            /* background-color: linear-gradient(rgb(104, 145, 162), rgb(12, 97, 33));*/
            padding: 0px;
            font-weight: 700;
            font-size: 14px;
            height: 36px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            border: none;
            -o-transition: all 0.218s;
            -moz-transition: all 0.218s;
            -webkit-transition: all 0.218s;
            transition: all 0.218s;
        }

            .btn.btn-signin:hover,
            .btn.btn-signin:active,
            .btn.btn-signin:focus {
                background-color: rgb(12, 97, 33);
            }

        .forgot-password {
            color: rgb(104, 145, 162);
        }

            .forgot-password:hover,
            .forgot-password:active,
            .forgot-password:focus {
                color: rgb(12, 97, 33);
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            // DOM ready

            // Test data
            /*
             * To test the script you should discomment the function
             * testLocalStorageData and refresh the page. The function
             * will load some test data and the loadProfile
             * will do the changes in the UI
             */
            // testLocalStorageData();
            // Load profile if it exits
            loadProfile();
        });

        /**
         * Function that gets the data of the profile in case
         * thar it has already saved in localstorage. Only the
         * UI will be update in case that all data is available
         *
         * A not existing key in localstorage return null
         *
         */
        function getLocalProfile(callback) {
            var profileImgSrc = localStorage.getItem("PROFILE_IMG_SRC");
            var profileName = localStorage.getItem("PROFILE_NAME");
            var profileReAuthEmail = localStorage.getItem("PROFILE_REAUTH_EMAIL");

            if (profileName !== null
                    && profileReAuthEmail !== null
                    && profileImgSrc !== null) {
                callback(profileImgSrc, profileName, profileReAuthEmail);
            }
        }

        /**
         * Main function that load the profile if exists
         * in localstorage
         */
        function loadProfile() {
            if (!supportsHTML5Storage()) { return false; }
            // we have to provide to the callback the basic
            // information to set the profile
            getLocalProfile(function (profileImgSrc, profileName, profileReAuthEmail) {
                //changes in the UI
                $("#profile-img").attr("src", profileImgSrc);
                $("#profile-name").html(profileName);
                $("#reauth-email").html(profileReAuthEmail);
                $("#inputEmail").hide();
                $("#remember").hide();
            });
        }

        /**
         * function that checks if the browser supports HTML5
         * local storage
         *
         * @returns {boolean}
         */
        function supportsHTML5Storage() {
            try {
                return 'localStorage' in window && window['localStorage'] !== null;
            } catch (e) {
                return false;
            }
        }

        /**
         * Test data. This data will be safe by the web app
         * in the first successful login of a auth user.
         * To Test the scripts, delete the localstorage data
         * and comment this call.
         *
         * @returns {boolean}
         */
        function testLocalStorageData() {
            if (!supportsHTML5Storage()) { return false; }
            localStorage.setItem("PROFILE_IMG_SRC", "//lh3.googleusercontent.com/-6V8xOA6M7BA/AAAAAAAAAAI/AAAAAAAAAAA/rzlHcD0KYwo/photo.jpg?sz=120");
            localStorage.setItem("PROFILE_NAME", "César Izquierdo Tello");
            localStorage.setItem("PROFILE_REAUTH_EMAIL", "oneaccount@gmail.com");
        }

        function Validatetxtbox() {
            var txtbxresult = false;
            var UserName = document.getElementById('<%= txtemail.ClientID %>').value.trim();
            var Pwd = document.getElementById('<%= txtpwd.ClientID %>').value.trim();
            var Facility = document.getElementById('<%= drpfacility.ClientID %>').value.trim();
            if (UserName == "" && Pwd == "" && Facility == "0") {
                alert("Enter All Login Credentials");
            } else if (UserName == "" && Pwd == "") {
                alert("Enter UserName and Pawword");
            } else if (UserName == "" && Facility == "0") {
                alert("Enter UserName and Facility");
            } else if (Pwd == "" && Facility == "0") {
                alert("Enter Password and Facility");
            } else if (UserName == "") {
                alert("Enter UserName");
            } else if (Pwd == "") {
                alert("Enter Password");
            } else if (Facility == "0") {
                alert("Enter Facility");
            }
            else {
                txtbxresult = true;
            }
            return txtbxresult;
        }
    </script>
</head>
<body>
    <%-- <div class="container">  
                <div class="row">  
                    <div class="col-lg-3"></div>  
                    <div class="col-lg-6">  
                        <form class="form-signin" runat="server">  
                            <h2 class="form-signin-heading">Please sign in</h2>  
                            <label for="inputEmail" class="sr-only">Email address</label>  
                          <asp:TextBox ID="txtemail" runat="server" placeholder="Email address" CssClass="form-control"></asp:TextBox>
                            <br />
                                <label for="inputPassword" class="sr-only">Password</label>  
                                <asp:TextBox ID="txtpwd" runat="server" placeholder="Password" CssClass="form-control"></asp:TextBox>
                                    <div class="checkbox">  
                                     
                                            <asp:CheckBox ID="chkrem" Text="Remember me" runat="server" />   
                                                   
                                       
                                    </div>  
                            <asp:Button ID="btnlogin" Text="Login" CssClass="btn btn-primary" runat="server" Width="50%"  OnClick="btnlogin_Click"/>
                            
                        </form>  
                    </div>  
                    <div class="col-lg-3"></div>  
                </div>  
            </div>--%>
    <div class="container">
        <div class="card card-container">
            <!-- <img class="profile-img-card" src="//lh3.googleusercontent.com/-6V8xOA6M7BA/AAAAAAAAAAI/AAAAAAAAAAA/rzlHcD0KYwo/photo.jpg?sz=120" alt="" /> -->
            <div class="mypanel-body">
                <div class="row">
                    <div class="col-lg-12">
                        <img id="profile-img" class="profile-img-card" src="//ssl.gstatic.com/accounts/ui/avatar_2x.png" />
                        <p id="profile-name" class="profile-name-card"></p>
                    </div>
                </div>

                <form class="form-signin" runat="server">
                    <div class="row">
                        <div class="col-lg-12">
                            <span id="reauth-cor" class="reauth-email"></span>
                            <asp:DropDownList ID="drpcorp" runat="server" CssClass="form-control" Width="98%" OnSelectedIndexChanged="drpcorp_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <span id="reauth-email" class="reauth-email"></span>
                            <asp:DropDownList ID="drpfacility" runat="server" CssClass="form-control" Width="98%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="height: 10px;"></div>
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:TextBox ID="txtemail" runat="server" placeholder="User Name" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:TextBox ID="txtpwd" runat="server" placeholder="Password" CssClass="form-control" TextMode="Password"></asp:TextBox>

                        </div>
                    </div>
                    <%-- <div id="remember" class="checkbox">
                   <label>  <asp:CheckBox ID="chkrem" Text="Remember me" runat="server" /></label>
                </div>--%>
                    <%-- <button class="btn btn-lg btn-primary btn-block btn-signin" type="submit">Sign in</button>--%>
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:Button CssClass="btn btn-lg btn-primary btn-block btn-signin" Text="Sign in" runat="server" OnClick="btnlogin_Click" OnClientClick="return Validatetxtbox();" /></div>
                    </div>
                </form>
                <!-- /form -->
              <%--  <div class="row">
                    <div class="col-lg-9"><a href="#" class="forgot-password">Forgot the password? </a></div>
                </div>--%>
                <div class="row">
                    <div class="col-lg-12">
                        <asp:Label ID="lblerror" runat="server" Style="color: red;" Visible="false"></asp:Label>
                    </div>
                </div>

            </div>
            <!-- /card-container -->
        </div>
    </div>
</body>
</html>
