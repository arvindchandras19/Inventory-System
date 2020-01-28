<%@ Page Title="User Roles" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="UserPermission.aspx.cs" Inherits="Inventory.UserPermission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : UserPermission.aspx 
' Type      : ASPX File 
' Description  :   To design the UserPermission page for add,Update and show the Item list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01              Dhanasekran.C                  New
  10/24/2017           V.02              Sairam.P                       Model Popup is added for Notifications
  12/27/2017           V.02              Vivekanand.S                   Model Popup is added for Notifications
  03/20/2018           V.03              Sairam.P                       Add Email Notification
'******************************************************************************************************/
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Common.css" rel="stylesheet" />
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

        .outPopUp {
            position: absolute;
            width: 900px;
            max-height: 300px;
            z-index: 15;
            top: 30%;
            left: 20%;
            margin: -50px 0 0 -150px;
            background: #f1f1f1;
            box-shadow: 10px 10px 10px rgba(0, 0, 0, 0.5);
        }

        .Upopacity {
            opacity: 0.3;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            -o-background-size: cover;
            background-size: cover;
        }
    </style>
    <script type="text/javascript">

        function Clear() {
            var MainGridView = $('#GrdUserPermissionMaster');
            MainGridView.find("input[type='checkbox']").prop('checked', false);
            document.getElementById('<%=drpMainmenu.ClientID%>').value = 0;
            document.getElementById('<%=drpsubmenu.ClientID%>').value = 0;
            document.getElementById('<%=hdnfield.ClientID%>').value = "0";
            validationclear();
        }

        function OpenRolePopup() {
            $find("modalAddbedget").show();
            document.getElementById("<%=txtRole.ClientID %>").style.borderColor = "#d2d6de";
            document.getElementById("<%=txtRole.ClientID %>").value = '';
            $('[id*=DivErrorList]').hide();
            return false;
        }
        function validationclear() {

            $("#<%= ReqfielddrpMainmenu.ClientID %>").css("display", "none");
            $("#<%= Reqfielddrpsubmenu.ClientID %>").css("display", "none");

        }

        function Page_ClientValidateReset() {
            $("#<%= ReqfielddrpMainmenu.ClientID %>").css("display", "block");
            $("#<%= Reqfielddrpsubmenu.ClientID %>").css("display", "block");


        }
        function ShowPopup(res) {

            $('[id*=lblsave]').html(res);

            $("#modalSave").modal("show");

        }
        function ShowdelPopup(res) {

            $('[id*=lbldelete]').html(res);

            $("#modalDelete").modal("show");

        }

<%--        function Validatetxtbox() {
            var txtbxresult = false;
            var Mainmenu = document.getElementById('<%= drpMainmenu.ClientID %>').value.trim();
            var Submenu = document.getElementById('<%= drpsubmenu.ClientID %>').value.trim();
            var Checkhidden = document.getElementById('<%=hdnfield.ClientID%>').value;


            if (Checkhidden == "0") {
                if (Mainmenu == "0" || Submenu == "0") {
                    alert("Enter all mandatory Fields");
                }
                else {
                    txtbxresult = true;
                }
            } else {
                txtbxresult = true;
            }
            return txtbxresult;
        }--%>


        // Choosen Dropdown

        //$(document).ready(function () {
        //    "use strict";

        //    $(".chosen-select").chosen();

        //    $(".chosen-search").append('<i class="glyph-icon icon-search"></i>');

        //    $(".chosen-single div").html('<i class="glyph-icon icon-caret-down"></i>');


        //});      

        function ChosenjScript() {
            $(".chosen-select").chosen();

            $(".chosen-search").append('<i class="glyph-icon icon-search"></i>');

            $(".chosen-single div").html('<i class="glyph-icon icon-caret-down"></i>');
        }


    </script>
    <script type="text/javascript">
        function CheckOrUnCheckAll(obj) {
            var MainGridView = $('#GrdUserPermissionMaster');
            MainGridView.find("input[type='checkbox']").prop('checked', false);
            document.getElementById('<%=drpMainmenu.ClientID%>').value = 0;
            document.getElementById('<%=drpsubmenu.ClientID%>').value = 0;
            document.getElementById('<%=hdnfield.ClientID%>').value = "0";
            var valid = false;
            var chkselectcount = 0;
            var gridview = document.getElementById('<%= GrdUserPermissionMaster.ClientID %>');
            for (var i = 0; i < gridview.getElementsByTagName("input").length; i++) {
                var node = gridview.getElementsByTagName("input")[i];
                if (node != null && node.type == "checkbox") {
                    node.checked = obj;
                }
            }
            validationclear();
            return false;
        }


        function ShowwarningPopup(res) {
            $('[id*=lblwarning]').html(res);
            $("#modalWarning").modal("show");

        }


        function ValidateRange(row) {
            var rowData = row.parentNode.parentNode.parentNode;
            var rowIndex = rowData.rowIndex - 2;

            var txtApprovalFrom = $("input[id*= txtApprovalFrom]")
            var txtApprovalTo = $("input[id*= txtApprovalTo]")

            var LowerLimit = txtApprovalFrom[rowIndex].value;
            var UpperLimit = txtApprovalTo[rowIndex].value;



            if (parseFloat(LowerLimit) > parseFloat(UpperLimit) && UpperLimit != "" && LowerLimit != "") {
                ShowwarningPopup("Lower limt value should be less than upper limit value");
            }

        }


        function ValidateFootRange(row) {
            var rowData = row.parentNode.parentNode.parentNode;
            var rowIndex = rowData.rowIndex - 2;

            var txtApprovalFrom = $("input[id*= foottxtApprovalFrom]")
            var txtApprovalTo = $("input[id*= foottxtApprovalTo]")

            var LowerLimit = txtApprovalFrom[rowIndex].value;
            var UpperLimit = txtApprovalTo[rowIndex].value;


            if (parseFloat(LowerLimit) > parseFloat(UpperLimit) && UpperLimit != "" && LowerLimit != "") {
                ShowwarningPopup("Lower limt value should be less than upper limit value");
            }

        }


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Role Menu Permission
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(ChosenjScript);
                </script>
                <asp:HiddenField runat="server" ID="hdnfield" Value="0"></asp:HiddenField>
                <div id="permission" runat="server" class="mypanel-body">
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                        </div>
                        <div class="col-sm-9 col-md-9 col-lg-9" align="right">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" ValidationGroup="EmptyField" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                            <%--<asp:Button ID="btnclear" runat="server" CssClass="btn btn-success" Text="Clear" OnClientClick="javascript:return CheckOrUnCheckAll(false);return false;" />--%>
                            <asp:Button ID="btnclear" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnclear_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <span>Main Menu</span><span style="color: red">*</span>
                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpMainmenu" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                ControlToValidate="drpMainmenu" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="drpMainmenu" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpMainmenu_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <span>Sub Menu</span><span style="color: red">*</span>
                            <asp:RequiredFieldValidator InitialValue="0" ID="Reqfielddrpsubmenu" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                ControlToValidate="drpsubmenu" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="drpsubmenu" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpsubmenu_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3" style="margin-top: 1%;">
                            <asp:LinkButton ID="lkbtnAdd" runat="server" Text="Add New" OnClientClick="return OpenRolePopup()"></asp:LinkButton>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3" id="DivAddApproveLink" runat="server" style="margin-top: 1%; display: none;">
                            <asp:LinkButton ID="lkbtnAddAcitve" runat="server" Text="Add New Approval" OnClick="lkbtnAddAcitve_Click"></asp:LinkButton>
                        </div>
                    </div>

                    <div class="row userrolemaster" runat="server" id="DivUserPermission">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <asp:GridView ID="GrdUserPermissionMaster" AutoGenerateColumns="false" runat="server" CssClass="table table-responsive" OnRowDataBound="GrdUserPermissionMaster_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="UserRoleID" HeaderText="UserRoleID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="UserRole" HeaderText="Role Name" />
                                    <asp:TemplateField HeaderText="Is Edit">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="IsEdit" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Is View">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="IsViewOnly" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email Notification">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="IsEmailNotification" runat="server" />
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

                    <div class="row userrolemaster" runat="server" id="DivUserPermissionApprove" style="display: none;">
                        <asp:GridView ID="GrdUserRoleApprove" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" OnRowCreated="GrdUserRoleApprove_RowCreated" OnRowDataBound="GrdUserRoleApprove_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Role Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserRoleName" runat="server"></asp:Label>
                                        <asp:Label ID="lblUserRoleId" runat="server" CssClass="HeaderHide"></asp:Label>
                                        <asp:Label ID="lblPermissionID" runat="server" CssClass="HeaderHide"></asp:Label>
                                        <%--<asp:DropDownList ID="drpUserRole" runat="server"></asp:DropDownList>--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="footdrpUserRole" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Edit">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="IsEdit" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="footIsEdit" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="IsView" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="footIsView" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Approve">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="IsApprove" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="footIsApprove" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Order">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="IsOrder" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="footIsOrder" runat="server" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Deny">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="IsDeny" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="footIsDeny" runat="server" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lower Limit($)">
                                    <ItemTemplate>
                                        <div class="Currency-group">
                                            <span class="Currency-group-addon">
                                                <i class="fa fa-dollar"></i>
                                            </span>
                                            <asp:TextBox ID="txtApprovalFrom" runat="server" MaxLength="9" DataFormatString="${0:#,0.00}"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="FTBApprovalFrom" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtApprovalFrom"></ajax:FilteredTextBoxExtender>
                                        </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="Currency-group">
                                            <span class="Currency-group-addon">
                                                <i class="fa fa-dollar"></i>
                                            </span>
                                            <asp:TextBox ID="foottxtApprovalFrom" runat="server" MaxLength="9" DataFormatString="${0:#,0.00}"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="FTBfootApprovalFrom" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="foottxtApprovalFrom"></ajax:FilteredTextBoxExtender>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upper Limit($)">
                                    <ItemTemplate>
                                        <div class="Currency-group">
                                            <span class="Currency-group-addon">
                                                <i class="fa fa-dollar"></i>
                                            </span>
                                            <asp:TextBox ID="txtApprovalTo" runat="server" onchange="ValidateRange(this);" MaxLength="9" DataFormatString="$ {0:#,0.00}"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="FTBApprovalTo" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtApprovalTo"></ajax:FilteredTextBoxExtender>
                                        </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="Currency-group">
                                            <span class="Currency-group-addon">
                                                <i class="fa fa-dollar"></i>
                                            </span>
                                            <asp:TextBox ID="foottxtApprovalTo" runat="server" onchange="ValidateFootRange(this);" MaxLength="9" DataFormatString="$ {0:#,0.00}"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="FTBfootApprovalTo" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="foottxtApprovalTo"></ajax:FilteredTextBoxExtender>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Multiple Roles">
                                    <ItemTemplate>
                                        <%--<asp:ListBox ID="drpchoosenUserRole" runat="server" SelectionMode="Multiple" multiple="multiple" class="form-control chosen-select" Width="300px"></asp:ListBox>--%>
                                        <asp:LinkButton ID="lkbmultiapprove" runat="server" Text="Assign multiple roles" OnClick="lkbmultiapprove_Click"></asp:LinkButton>
                                        <asp:Label ID="lblhddListUserID" runat="server" CssClass="HeaderHide"></asp:Label>
                                        <%--<asp:DropDownList ID="drpchoosenUserRole" runat="server" SelectionMode="Multiple" multiple = "multiple" class = "form-control chosen-select" Width="300px"></asp:DropDownList>--%>
                                    </ItemTemplate>
                                    <%--<FooterTemplate>
                                        <asp:ListBox ID="footdrpchoosenUserRole" runat="server" SelectionMode="Multiple" multiple="multiple" class="form-control chosen-select" Width="300px"></asp:ListBox>
                                    </FooterTemplate>--%>
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

                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeRole" runat="server"
                    PopupControlID="modalAddRole" TargetControlID="Button1"
                    BackgroundCssClass="modalBackground" BehaviorID="modalAddbedget">
                </ajax:ModalPopupExtender>
                <div id="modalAddRole" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="btneditclose" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: green; font-size: large">Add Role</h4>
                            </div>
                            <div class="modal-body">
                                <div id="DivErrorList" runat="server" style="display:none;">
                                    <asp:Label ID="lblErrorList" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                                <div style="height: 40px">
                                    <div class="form-horizontal">
                                        <div class="col-md-6 col-sm-6">
                                            <span>Role Name </span>
                                            <asp:TextBox ID="txtRole" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                <asp:Button runat="server" Text="Close" CssClass="btn btn-success" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <div id="modalWarning" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header btn-warning">
                        <h4 class="modal-title font-bold text-white">User Permission
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

        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">User Permission
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
                        <h4 class="modal-title font-bold text-white">User Permission
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

        <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="DivMultiroleApprove">
            <%--<asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnuploadfile" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
            <asp:Label ID="lblMultiroleApprove" runat="server" CssClass="page-header page-title" Text="Assign Multiple Roles"></asp:Label>
            <asp:HiddenField ID="hdnattachment" runat="server" />
            <%--<asp:HiddenField ID="HddListofUserID" runat="server" />--%>
            <asp:GridView ID="GrdMultiroleApprove" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                <Columns>
                    <asp:TemplateField HeaderText="IsAssign">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkmultirole" runat="server" />
                            <asp:Label ID="lblMultiPermissionID" runat="server" CssClass="HeaderHide"></asp:Label>
                            <asp:Label ID="lblPageMasterPermissionMultiRoleID" runat="server" CssClass="HeaderHide"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Role Name">
                        <ItemTemplate>
                            <asp:Label ID="lblroleid" runat="server" CssClass="HeaderHide"></asp:Label>
                            <asp:Label ID="lblrole" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action Order">
                        <ItemTemplate>
                            <asp:TextBox ID="txtactionorder" runat="server"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="Filteractionorder" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtactionorder"></ajax:FilteredTextBoxExtender>
                            <asp:Label ID="lblApprovefrom" runat="server" CssClass="HeaderHide"></asp:Label>
                            <asp:Label ID="lblApproveto" runat="server" CssClass="HeaderHide"></asp:Label>
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
            <div class="row" style="margin-top: 5px;">
                <div class="col-lg-6 col-md-6 col-sm-6 form-group">
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 " align="right">
                    <br />
                    <asp:Button ID="btnMultiroleApproveAssign" runat="server" Text="Assign" CssClass="btn btn-primary" OnClick="btnMultiroleApproveAssign_Click" />
                    <asp:Button ID="btnMultiroleApproveClose" runat="server" Text="Close" CssClass="btn btn-success" OnClick="btnMultiroleApproveClose_Click" />
                </div>
            </div>
        </div>

        <%-- <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>--%>
        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center>You Don't have a Permission to View This Page...</center>
            </h4>
        </div>

        <div style="display: none">
            <rsweb:ReportViewer ID="rvUserPermissionreport" runat="server"></rsweb:ReportViewer>
        </div>
   

</asp:Content>
