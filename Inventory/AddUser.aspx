<%@ Page Title="Users" Language="C#" AutoEventWireup="true" MasterPageFile="~/Inven.Master" CodeBehind="AddUser.aspx.cs" Inherits="Inventory.AddUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Common.css" rel="stylesheet" />
    <link href="//cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="Content/sumoselect.css" rel="stylesheet" />
    <link href="Content/chosen.css" rel="stylesheet" />
    <script src="Scripts/CDN.js/Cdn.js"></script>
    <style type="text/css">
        .page-title-breadcrumb {
            padding: 3px 20px;
            background: #ffffff;
            -webkit-box-shadow: 0 2px 2px rgba(0, 0, 0, 0.05), 0 1px 0 rgba(0, 0, 0, 0.05);
            box-shadow: 0 2px 2px rgba(0, 0, 0, 0.05), 0 1px 0 rgba(0, 0, 0, 0.05);
            clear: both;
            border-bottom: 5px solid #e5e5e5 !important;
            box-shadow: none !important;
        }

        .pull-left {
            float: left !important;
        }

        .page-title-breadcrumb .page-header .page-title {
            font-size: 25px;
            font-weight: 300;
            display: inline-block;
        }

        .page-title-breadcrumb .breadcrumb {
            margin-bottom: 0;
            padding-left: 0;
            padding-right: 0;
            border-radius: 0;
            background: transparent;
        }

        .color {
            color: red;
        }

        .breadcrumb {
            padding: 8px 15px;
            margin-bottom: 20px;
            list-style: none;
            background-color: #f5f5f5;
            border-radius: 4px;
        }

        .page-header {
            margin: 0;
            padding: 0;
            border-bottom: 0;
        }

            .page-header .page-title {
                font-size: 25px;
                font-weight: 300;
                display: inline-block;
            }

        .pull-left {
            float: left !important;
        }

        .modalBackground {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1030;
            background-color: #000;
            opacity: 0.3;
        }

        .ShoworHide {
            display: none;
        }

        .radiostyle input[type="radio"] {
            margin-left: 10px;
            margin-right: 1px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>

            $('[id*=drpCorpSearch]').SumoSelect({
                selectAll: true,
                placeholder: 'Select Facility'
            });

            $('[id*=drpFacilitySearch]').SumoSelect({
                selectAll: true,
                placeholder: 'Select Facility'
            });

            $('[id*=drpRoleSearch]').SumoSelect({
                selectAll: true,
                placeholder: 'Select Facility'
            });

            $('[id*=drpFacilitys]').SumoSelect({
                selectAll: true,
                placeholder: 'Select Facility'
            });
        });

        function ViewPassword(lnk) {
            if ($(lnk).is(":checked")) {
                $('[id*=txtPwd]').attr("Type", "text");
                $('[id*=txtComfirmPwd]').attr("Type", "text");
            } else {
                $('[id*=txtPwd]').attr("Type", "Password");
                $('[id*=txtComfirmPwd]').attr("Type", "Password");
            }
        }



        function jScript() {
            //--$(document).ready(function () {   
            $('[id*=drpCorpSearch]').SumoSelect({
           <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Corporate'
            });

            $('[id*=drpFacilitySearch]').SumoSelect({
           <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Facility'
            });

            $('[id*=drpRoleSearch]').SumoSelect({
           <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Role'
            });

            $('[id*=drpFacilitys]').SumoSelect({
           <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Facility'
            });



            $(document).ready(function () {
                $(".input-mask").inputmask();
            });
            //});
        }
        function jscriptsearch() {
            var config = {
                '.chosen-select': {},
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }

        function ShowPassword() {
            var HddRoleId = $(<%=HddRoleID.ClientID%>).val();

            if (HddRoleId != 1) {
                $('.Show').addClass("ShoworHide");
                $('Show').addClass("ShoworHide");
            }
        }

        function ShowPopup(res) {

            $('[id*=lblsave]').html(res);

            $("#modalSave").modal("show");

        }
        function ShowdelPopup(res) {

            $('[id*=lbldelete]').html(res);

            $("#modalDelete").modal("show");

        }
        function ShowwarningLookupPopup(res) {
            //alert('ggg');
            $('[id*=lblwarning]').html(res);

            $("#modalWarning").modal("show");

        }
        function ShowConfirmationPopup() {

            $("#modalConfirm").modal("show");

        }

        $(document).ready(function () {

            $('body').on('keyup', 'input.phone-input', function () {
                if ($(this).val().length === this.size) {
                    var inputs = $('input.phone-input');
                    inputs.eq(inputs.index(this) + 1).focus();
                }
            });

        });



    </script>


    <script>
       <%-- function Validatetxtbox() {
            var txtbxresult = false;
            var Firstname = document.getElementById('<%= txtFirstname.ClientID %>').value.trim();
            var Lastname = document.getElementById('<%= txtLastname.ClientID %>').value.trim();
            var UserName = document.getElementById('<%= txtUsername.ClientID %>').value.trim();
            var Pwd = document.getElementById('<%= txtPwd.ClientID %>').value.trim();
            var RePwd = document.getElementById('<%= txtComfirmPwd.ClientID %>').value.trim();
            var Phone = document.getElementById('<%= txtPhone.ClientID %>').value.trim();
            var Email = document.getElementById('<%= txtEmail.ClientID %>').value.trim();
           // var Facility = document.getElementById('<%= drpFacilitys.ClientID %>').value.trim();
          //  var Role = document.getElementById('<%= drpUserrole.ClientID %>').value.trim();
          //  var Corporate = document.getElementById('<%= drpcorp.ClientID %>').value.trim();
            // var Budget = document.getElementById('<%= drpcurrency.ClientID %>').value.trim();
            if (Firstname == "" || Lastname == "" || UserName == "" || Pwd == "" || Phone == "" || Email == "" || RePwd == "") {
                alert("Enter all mandatory Fields");
            }
            else {
                txtbxresult = true;
            }
            return txtbxresult;
        }--%>
        function OpenBudgetPopup() {
            $find("modalAddbedget").show();
            document.getElementById("<%=txtbudget.ClientID %>").style.borderColor = "gray";
            return false;
        }
        <%--function drpfun() {
            var DollarDropDown = document.getElementById('<%= drpcurrency.ClientID %>').value;

            if (DollarDropDown == 0) {
                $('#<%= txtbudgetvalue.ClientID %>').prop('disabled', true);
            }
            else {
                $('#<%= txtbudgetvalue.ClientID %>').prop('disabled', false);
            }

        }--%>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    
        <asp:HiddenField runat="server" ID="hdnfield" Value="0"></asp:HiddenField>
        <asp:HiddenField runat="server" ID="HddRoleID" Value="0"></asp:HiddenField>        
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    User Details
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(jScript);
                    Sys.Application.add_load(jscriptsearch);
                </script>
                <div runat="server">
                    <div class="mypanel-body">
                        <asp:HiddenField runat="server" ID="HddAddCancel" Value="0"></asp:HiddenField>
                        <asp:HiddenField runat="server" ID="HddUserID" Value="0" />
                        <div class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                                <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                            </div>
                            <div class="col-sm-4 col-md-4 col-lg-4">
                            </div>
                            <%--<div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="">
                                    <span>Select User</span>
                                    <asp:DropDownList ID="drpexistuser" runat="server" CssClass="form-control chosen-select chosen-container chosen-search"
                                        OnSelectedIndexChanged="drpexistuser_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>--%>
                            <%--     <div class="col-sm-1 col-md-1 col-lg-1">
                                      <asp:Label ID="lblactivedd" runat="server" Text="Active"></asp:Label>
                                    <div class="input-icon right">
                                        <asp:CheckBox ID="chkact" runat="server" Checked="true" />
                                    </div>
                                </div>--%>
                           <%-- <div class="col-sm-3 col-md-3 col-lg-3">
                                <asp:Label ID="lblinactivedd" runat="server" Text="All Users[Active & InActive]"></asp:Label>
                                <div class="input-icon right">
                                    <asp:CheckBox ID="chkinact" runat="server" OnCheckedChanged="chkinact_CheckedChanged" AutoPostBack="true" />
                                </div>
                            </div>--%>
                            <div class="col-sm-4 col-md-4 col-lg-4" align="right">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click"/>
                                <asp:Button ID="btnSave" runat="server" Visible="false" CssClass="btn btn-success" Text="Save" ValidationGroup="EmptyField" OnClick="btnSave_Click" />
                                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                                <asp:Button ID="btnclear" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnclear_Click" />
                            </div>
                        </div>
                        <div style="margin-bottom: 5px;"></div>
                        <div id="DivSearch" runat="server">
                            <div id="divContent" class="well well-sm">
                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                <div class="form-group">
                                    <span style="font-weight:800;">Corporate</span>
                                    <asp:ListBox ID="drpCorpSearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpCorpSearch_SelectedIndexChanged"></asp:ListBox>
                                    <%--<asp:DropDownList runat="server" ID="drpFacilitySearch" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpVendorSearch" runat="server" ForeColor="Red"
                                ControlToValidate="drpVendorSearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>      
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                <div class="form-group">
                                    <span style="font-weight:800;">Facility</span>
                                    <asp:ListBox ID="drpFacilitySearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple"></asp:ListBox>
                                    <%--<asp:DropDownList runat="server" ID="drpFacilitySearch" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpVendorSearch" runat="server" ForeColor="Red"
                                ControlToValidate="drpVendorSearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>      
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight:800;">Status</span>
                                            <asp:RadioButtonList ID="rdbstatus" runat="server" RepeatDirection="Horizontal" CssClass="radiostyle">
                                                <asp:ListItem Text="All" Value="" ></asp:ListItem>
                                                <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                <div class="form-group">
                                    <span style="font-weight:800;">Role</span>
                                    <asp:ListBox ID="drpRoleSearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple"></asp:ListBox>
                                    <%--<asp:DropDownList runat="server" ID="drpFacilitySearch" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpVendorSearch" runat="server" ForeColor="Red"
                                ControlToValidate="drpVendorSearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                         <div class="form-group">
                                             <span style="font-weight:800;">User Name</span>
                                             <asp:TextBox ID="txtSerchUserName" runat="server" CssClass="form-control"></asp:TextBox>
                                             </div>
                                    </div>
                                    
                                </div>                               
                            </div>
                             <div id="DivGrid" runat="server">
                                 <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <asp:Label ID="lblresult" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>                   </div>
                </div>
                                  <div>
                                    <asp:Label ID="lblrowcount" runat="server">No. of Records: <%= GrdUserDetails.Rows.Count.ToString()  %></asp:Label>
                                </div>
                     <asp:HiddenField ID="HddMainDelete" runat="server" Value="" />                                             
                    <div class="row" style="margin-top: 0px;">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12" style="padding-right: 0px; height: 100%; width: 100%; overflow: auto; z-index: 1;">
                        <div class="MSRReviewgrid">
                        <asp:GridView ID="GrdUserDetails" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                            CssClass="table table-responsive" OnRowDataBound="GrdUserDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ToolTip="Edit User" OnClick="imgbtnEdit_Click" ImageUrl="~/Images/edit.png" />
                                        <asp:ImageButton ID="Imgdelete" runat="server" Text="Delete" Height="20px" ToolTip="Delete User" ImageUrl="~/Images/Delete.png" OnClick="Imgdelete_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>                                       
                                <asp:BoundField DataField="CorporateName" HeaderText="Corporate" HeaderStyle-Width="7%" />
                                <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" HeaderStyle-Width="7%" />
                                <asp:BoundField DataField="UserRole" HeaderText="Role"  HeaderStyle-Width="10%" />
                                <asp:BoundField DataField="UserID" HeaderText="User ID" HeaderStyle-Width="5%" />
                                <asp:BoundField DataField="FirstName" HeaderText="First Name"/>
                                <asp:BoundField DataField="LastName" HeaderText="Last Name"/>
                                <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-Width="18%" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" HeaderStyle-Width="9%" />
                                <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo"  HeaderStyle-Width="10%"/>
                                <%--<asp:BoundField DataField="Active" HeaderText="Active" />--%>
                              <%--  <asp:BoundField DataField="BillAddress1" HeaderText="Bill Address 1" />
                                <asp:BoundField DataField="BillAddress2" HeaderText="Bill Address 2" />--%>                                
                                <asp:TemplateField HeaderText="Active" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" Text='<%# ((string)Eval("IsActive").ToString() == "True" ? "Yes" : "No")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                            <HeaderStyle CssClass="Headerstyle" />
                            <FooterStyle CssClass="gridfooter" />
                            <PagerStyle CssClass="pagerstyle" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="gridselectedrow" />
                            <EditRowStyle CssClass="grideditrow" />
                            <AlternatingRowStyle CssClass="gridalterrow" />
                            <RowStyle CssClass="gridrow" />
                        </asp:GridView>
                 </div>
                    </div>
                </div>
                                 
                                </div>
                        </div>

                        <div id="DivAdd" runat="server" style="display:none;">
                               <div style="margin-top: 3px;">
                            <span style="font-size: 14px; font-weight: bolder;">Users</span>
                        </div>

                        <div style="margin-left: 2px; margin-right: 2px; border-style: solid; border-color: #e5e5e5; border-width: 2px; border-radius: 5px; background-color: #f5f5f5;">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblFirstname" runat="server" Text="First Name"></asp:Label><span style="color: red; padding-right: 5px;">*</span>
                                        <div class="input-icon right i">
                                            <i class="fa fa-user"></i>
                                            <asp:TextBox ID="txtFirstname" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqfieldFirstname" runat="server" ControlToValidate="txtFirstname" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblLastname" runat="server" Text="Last Name"></asp:Label><span style="color: red; padding-right: 5px;">*</span>
                                        <div class="input-icon right">
                                            <i class="fa fa-user"></i>
                                            <asp:TextBox ID="txtLastname" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqfieldLastname" runat="server" ControlToValidate="txtLastname" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label><span style="color: red; padding-right: 5px;">*</span>
                                        <div class="input-icon right i">
                                            <i class="fa fa-envelope"></i>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqfieldEmail" runat="server" ControlToValidate="txtEmail" CssClass="color" ValidationGroup="EmptyField" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                ErrorMessage="Please enter valid email" ControlToValidate="txtEmail"
                                                SetFocusOnError="true"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ValidationGroup="EmptyField" ForeColor="" Display="Dynamic" Font-Size="0.9em" CssClass="color">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblUsername" runat="server" Text="User Name"></asp:Label><span style="color: red; padding-right: 5px;">*</span>
                                        <div class="input-icon right i">
                                            <i class="fa fa-user-md"></i>
                                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqfieldUsername" runat="server" ControlToValidate="txtUsername" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblPwd" runat="server" Text="Password"></asp:Label><span style="color: red; padding-right: 15px;">*</span>
                                        <input type="checkbox" id="ChkShowPassword"/ onchange="ViewPassword(this);" class="Show"><span id="lblshow" class="Show">  Show</span>
                                        <div class="input-icon right">
                                            <i class="fa fa-lock"></i>
                                            <%--<i id="IconPassword" runat="server" class="fa fa-eye"></i>--%>
                                            <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqfieldPwd" runat="server" ControlToValidate="txtPwd" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblConfirmpwd" runat="server" Text="Re Password"></asp:Label><span style="color: red">*</span>
                                        <div class="input-icon right i">
                                            <i class="fa fa-lock"></i>
                                            <asp:TextBox ID="txtComfirmPwd" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="Reqfieldcgpwd" runat="server" ControlToValidate="txtComfirmPwd" CssClass="color" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator runat="server" ID="cmpPwd" ControlToValidate="txtComfirmPwd" ErrorMessage="" CssClass="color" ControlToCompare="txtPwd" Operator="Equal" ValidationGroup="EmptyField" Type="String" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label><span style="color: red">*</span>
                                        <div class="input-icon right">
                                            <i class="fa fa-phone"></i>
                                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'(999) 999-9999'"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="Reqfieldcophone" runat="server" ControlToValidate="txtPhone" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <%--<asp:CompareValidator runat="server" ID="compphone" Operator="DataTypeCheck" Type="Integer" CssClass="color" ValidationGroup="EmptyField"  SetFocusOnError="true" ForeColor="Red" Font-Size=".9em" Display="Dynamic" ControlToValidate="txtPhone" ErrorMessage="" />--%>
                                            <%--<ajax:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" TargetControlID="txtPhone" ValidChars="-"></ajax:FilteredTextBoxExtender>--%>
                                            <asp:RegularExpressionValidator ID="Regphone" runat="server"
                                                ErrorMessage="Invalid format" ControlToValidate="txtPhone" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                                ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2">
                                    <div class="form-group">
                                        <asp:Label ID="lblxtn" runat="server" Text="Xtn"></asp:Label>
                                        <div class="input-icon right">
                                            <i class="fa fa-phone"></i>
                                            <asp:TextBox ID="txtxtn" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                             <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtxtn"></ajax:FilteredTextBoxExtender>
                                           <%-- <asp:RegularExpressionValidator ID="Regxtn" runat="server"
                                                ErrorMessage="Invalid format" ControlToValidate="txtxtn" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                                ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>--%>
                                            <%--<ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtxtn"></ajax:FilteredTextBoxExtender>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-1 col-md-1 col-lg-1">
                                    <div id="DivCheckBox" runat="server">
                                          <asp:Label ID="lblactive" runat="server" Text="Active"></asp:Label>
                                    <div class="input-icon right">
                                        <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" />
                                    </div>
                                    </div>
                                </div>
                            </div>
                            <%-- <div class="row"> --%>

                            <%--</div>--%>
                        </div>


                        <div style="margin-top: 3px;">
                            <span style="font-size: 14px; font-weight: bolder;">Assign Roles</span>
                        </div>
                        <div style="margin-left: 2px; margin-right: 2px; border-style: solid; border-color: #e5e5e5; border-width: 2px; border-spacing: 5px; border-radius: 5px; background-color: #f5f5f5;">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <span>Corporate</span>
                                    <asp:DropDownList ID="drpcorp" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="drpcorp_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <span>Facility</span>

                                    <asp:ListBox ID="drpFacilitys" CssClass="form-control" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <span>User Role</span>
                                    <asp:DropDownList ID="drpUserrole" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <%--<div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="col-lg-6" style="padding-right: 5px; padding-left: 0px;">
                                    <span>Budget Limit</span><span><asp:LinkButton ID="lblbbudget" runat="server" Text="  Add" OnClientClick="return OpenBudgetPopup()" ></asp:LinkButton></span>
                                    <asp:DropDownList ID="drpcurrency" runat="server" CssClass="form-control" onchange="drpfun();">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-6" style="padding-right: 0px; padding-left: 0px;">
                                    <br />
                                  <asp:TextBox ID="txtbudgetvalue" runat="server" CssClass="form-control" disabled="true"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtbudgetvalue"></ajax:FilteredTextBoxExtender>
                                </div>
                            </div>--%>
                                <%--<div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="col-lg-6" style="padding-right: 5px; padding-left: 0px;">
                                </div>
                            </div>--%>
                            </div>
                        </div>
                        <div class="row usermaster" style="margin-top: 5px;">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <asp:GridView ID="grduserrole" runat="server" CssClass=" table table-responsive" AutoGenerateColumns="false" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Remove" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>

                                            <asp:ImageButton ID="lbremove" runat="server" Text="Remove" Height="20px" OnClick="lbremove_Click" ImageUrl="~/Images/close.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserFacilityRoleID" HeaderText="UserFacilityRoleID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="CorporateName" HeaderText="Corporate" />
                                    <asp:BoundField DataField="RoleID" HeaderText="RoleID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="FacilityDescription" HeaderText="Facility" />
                                    <asp:BoundField DataField="UserRole" HeaderText="User Role" />
                                    <%-- <asp:BoundField DataField="Budget" HeaderText="Budget Limit" />--%>
                                </Columns>
                                <HeaderStyle CssClass="Headerstyle" />
                                <FooterStyle CssClass="gridfooter" />
                                <PagerStyle CssClass="pagerstyle" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle CssClass="gridselectedrow" />
                                <EditRowStyle CssClass="grideditrow" />
                                <AlternatingRowStyle CssClass="gridalterrow" />
                                <RowStyle CssClass="gridrow" />
                            </asp:GridView>
                        </div>
                    </div>

                    </div>

                    
                        </div>
                     

                    <asp:Button ID="Button1" runat="server" Style="display: none" />
                    <ajax:ModalPopupExtender ID="mpebudget" runat="server"
                        PopupControlID="modalAddbedget" TargetControlID="Button1"
                        BackgroundCssClass="modalBackground" BehaviorID="modalAddbedget">
                    </ajax:ModalPopupExtender>
                    <div id="modalAddbedget" style="display: none;">
                        <div class="modal-dialog" style="width: 350px">

                            <div class="modal-content">
                                <div class="modal-header">
                                    <asp:Button ID="btneditclose" class="close" runat="server" Text="X" />
                                    <h4 class="modal-title" style="color: green; font-size: large">Budget Currency</h4>
                                </div>
                                <div class="modal-body">
                                    <div style="height: 40px">
                                        <div class="form-horizontal">
                                            <div class="col-md-6 col-sm-6">
                                                <span>Budget Currency </span>
                                                <asp:TextBox ID="txtbudget" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnbudgetvalue" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnbudgetvalue_Click" />
                                    <asp:Button runat="server" Text="Close" CssClass="btn btn-success" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                    CancelControlID="btnNo" BackgroundCssClass="modalBackground">
                </ajax:ModalPopupExtender>
                <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="100px" Width="400px" Style="display: none;">
                    <table width="100%" style="border: Solid 2px #0271dd; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                        <tr style="background-color: #0271dd">
                            <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">Confirm Box</td>
                            <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right">
                                <a href="javascript:void(0)" onclick="closepopup()">
                                    <img src="Images/Close.gif" style="border: 0px" align="right" /></a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                                <asp:Label ID="Label2" runat="server" Text="Are you sure you want to delete this Record?" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right" style="padding-right: 15px">
                                <asp:ImageButton ID="btnYes" OnClick="btnYes_Click" runat="server" ImageUrl="~/Images/btnyes.jpg" />
                                <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/Images/btnNo.jpg" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                </div>

      <div id="modalConfirm" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-red">
                        <h4 class="modal-title font-bold text-white">Delete Confirmation</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <p>Do you want to delete this record <span id="spnName"></span>?.</p>
                    </div>
                    <div class="modal-footer">
                        <asp:ImageButton ID="ImageButton3" runat="server" CssClass="btn btn-danger" AlternateText="Yes" OnClick="btnYes_Click" />
                        <button type="button" class="btn btn-default ra-100" data-dismiss="modal">Close</button>
                        <%--<asp:ImageButton ID="ImageButtonNo" runat="server" ImageUrl="~/Images/btnNo.jpg"/>--%>
                    </div>
                </div>
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">Add User
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblsave" runat="server"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default ra-100" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalDelete" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-red">
                        <h4 class="modal-title font-bold text-white">Add User
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lbldelete" runat="server"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default ra-100" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalWarning" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header btn-warning">
                        <h4 class="modal-title font-bold text-white">Add User
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblwarning" runat="server"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default ra-100" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

          <div style="display: none">
                    <rsweb:ReportViewer ID="rvUserSummaryreport" runat="server"></rsweb:ReportViewer>
           </div>
        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center> This User doesn't have permission to view this screen</center>
            </h4>
        </div>
       
</asp:Content>


