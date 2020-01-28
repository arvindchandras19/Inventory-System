<%@ Page Title="Facilities" Language="C#" EnableEventValidation="true" AutoEventWireup="true" MasterPageFile="~/Inven.Master" CodeBehind="Facility.aspx.cs" Inherits="Inventory.Facility" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : Facility.aspx 
' Type      : ASPX File 
' Description  :   To design the Facility page for add,Update and show the Facility list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01              Sairam.P                     New
  08/11/2017           V.02              Sairam.P                     Validations for all fields and Create a mandatory
'******************************************************************************************************/
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Common.css" rel="stylesheet" />
    <link href="//cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="Content/sumoselect.css" rel="stylesheet" />
    <script src="Scripts/CDN.js/Cdn.js"></script>
    <style type="text/css">
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

        .color {
            color: red;
        }

        .radiostyle input[type="radio"] {
            margin-left: 10px;
            margin-right: 1px;
        }

        .gridviewPager {
            background-color: #fff;
            padding: 2px;
            margin: 2% auto;
        }

            .gridviewPager a {
                margin: auto 1%;
                border-radius: 50%;
                background-color: #545454;
                padding: 5px 10px 5px 10px;
                color: #fff;
                text-decoration: none;
                -o-box-shadow: 1px 1px 1px #111;
                -moz-box-shadow: 1px 1px 1px #111;
                -webkit-box-shadow: 1px 1px 1px #111;
                box-shadow: 1px 1px 1px #111;
            }

                .gridviewPager a:hover {
                    background-color: #337ab7;
                    color: #fff;
                }

            .gridviewPager span {
                background-color: #066091;
                color: #fff;
                -o-box-shadow: 1px 1px 1px #111;
                -moz-box-shadow: 1px 1px 1px #111;
                -webkit-box-shadow: 1px 1px 1px #111;
                box-shadow: 1px 1px 1px #111;
                border-radius: 50%;
                padding: 5px 10px 5px 10px;
            }
    </style>
    <script>
        //Show hide Content & Clear
        $(document).ready(function () {
            <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
            $('[id*=drpUserRole]').SumoSelect({
                selectAll: true,
                placeholder: 'Select UserRole'

            });
        });
        function jScript() {
            $('[id*=drpUserRole]').SumoSelect({
                selectAll: true,
                placeholder: 'Select UserRole'
            });
        }
        function jscriptsearch() {
            var config = {
                '.chosen-select': {},
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

            $(document).ready(function () {
                $(".input-mask").inputmask();
            });
        }
        $(document).ready(function () {

            <%--$("#btnAdd").click(function () {
                $('#btnAdd').attr('disabled', 'disabled');
                $('[id*=btnPrint]').hide();
                $('[id*=btnDetailsPrint]').hide();
                $('#savebtn').slideToggle(200);
                $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
                $('#div_Grid').hide();
                $('#FacilityUserRole').hide();
                clear();
            });--%>


            <%--$("#btnClose").click(function () {
                //$('#btnAdd').removeAttr('disabled');
                $('[id*=btnPrint]').show();
                $('[id*=btnPrint]').removeAttr('disabled');
                $('#savebtn').slideToggle(200);
                $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
                //$('#div_SearchDiv').slideToggle(200);
                $('#div_Grid').show();
                $('#FacilityUserRole').hide();
                validationclear();
                clear();
            });--%>
        });
        <%--function editfacility() {
            $('#div_Grid').hide();
            $('[id*=btnPrint]').hide();
            $('#<%=div_ADDContent.ClientID %>').show();
            $("#div_SearchDiv").show();
            $('#savebtn').slideToggle(200);
            //$('#btnAdd').attr('disabled', 'disabled');
            $('#FacilityUserRole').show();

        }--%>

        function ValidatefacilityFC() {
            //$('#btnAdd').attr('disabled', 'disabled');
            $('[id*=btnPrint]').hide();
            $('#savebtn').slideToggle(200);
            $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
            $('#div_Grid').hide();
            var FacilityID = document.getElementById('<%= hiddenFacilityID.ClientID %>').value;
            var FacilityCode = document.getElementById('<%= HddFacilityCode.ClientID%>').value;
            if (FacilityID != 0)
                $('#FacilityUserRole').show();
            else {
                $('#FacilityUserRole').hide();
                $('[id*=btnDetailsPrint]').hide();
            }
            ShowdelPopup("Facility code (" + FacilityCode + ") is already exists");
            $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
        }

        function ValidatefacilityFAC() {
            //$('#btnAdd').attr('disabled', 'disabled');
            $('[id*=btnPrint]').hide();
            $('#savebtn').slideToggle(200);
            $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
            $('#div_Grid').hide();
            var FacilityID = document.getElementById('<%= hiddenFacilityID.ClientID %>').value;
            var FacilityAcctCode = document.getElementById('<%= HddFacilityAcctCode.ClientID %>').value;
            if (FacilityID != 0)
                $('#FacilityUserRole').show();
            else {
                $('#FacilityUserRole').hide();
                $('[id*=btnDetailsPrint]').hide();
            }
            ShowdelPopup("Facility Accounting code (" + FacilityAcctCode + ") is already exists");
            $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
        }

        function ShowContent() {
            $('#div_Grid').show();
            $('#savebtn').show();
            $('#<%=div_ADDContent.ClientID %>').show();
            //$('#btnAdd').attr('disabled', 'disabled');
            $('#div_SearchDiv').slideToggle(200);
        }

        function ShowPopup(res) {

            $('[id*=lblsave]').html(res);

            $("#modalSave").modal("show");

        }
        function ShowdelPopup(res) {

            $('[id*=lbldelete]').html(res);

            $("#modalDelete").modal("show");

        }

        function ShowwarningPopup(res) {
            //alert('ggg');
            $('[id*=lblwarning]').html(res);

            $("#modalWarning").modal("show");

        }
        function ShowConfirmationPopup() {
            $("#modalConfirm").modal("show");
        }


        function clear() {
            document.getElementById('<%= txtFacilityShortName.ClientID %>').value = ""
            document.getElementById('<%= txtFacilityDescription.ClientID %>').value = "";
            document.getElementById('<%= ddlCorporate.ClientID %>').value = 0;
            document.getElementById('<%= ddlCopyFromExistingFacility.ClientID %>').value = 0;
            document.getElementById('<%= txtAddress1.ClientID %>').value = "";
            document.getElementById('<%= txtAddress2.ClientID %>').value = "";
            document.getElementById('<%= txtCity.ClientID %>').value = "";
            document.getElementById('<%= ddlState.ClientID %>').value = 0;
            document.getElementById('<%= txtPhone.ClientID %>').value = "";
            document.getElementById('<%= txtZipcode.ClientID %>').value = "";
            document.getElementById('<%= txtZipcode1.ClientID %>').value = "";
            document.getElementById('<%= txtBillAddress1.ClientID %>').value = "";
            document.getElementById('<%= txtBillAddress2.ClientID %>').value = "";
            document.getElementById('<%= txtBillCity.ClientID %>').value = "";
            document.getElementById('<%= txtBillPhone.ClientID %>').value = "";
            document.getElementById('<%= txtBillZip.ClientID %>').value = "";
            document.getElementById('<%= txtBillZip1.ClientID %>').value = "";
            document.getElementById('<%= txtBillFax.ClientID %>').value = "";
            document.getElementById('<%= ddlBillState.ClientID %>').value = 0;
            document.getElementById('<%= txtFax.ClientID %>').value = "";
            document.getElementById('<%= txtGPAccountCode.ClientID %>').value = "";
            document.getElementById('<%= txtEMRCode.ClientID %>').value = "";
            document.getElementById('<%= txtPatientCensus.ClientID %>').value = "";
            document.getElementById('<%= txtEmployeeCensus.ClientID %>').value = "";
            document.getElementById('<%= txttxperweek.ClientID %>').value = "";
            document.getElementById('<%= hdnfield.ClientID %>').value = 0;
            document.getElementById('<%= hiddenFacilityID.ClientID %>').value = 0;
            document.getElementById('<%= txtxtn.ClientID %>').value = "";
            document.getElementById('<%= txtxtnbill.ClientID %>').value = "";
               <%-- document.getElementById('<%= txtSearchFacility.ClientID %>').value = "";--%>

        }
        function Validatetxtbox(sender, args) {
            var Facshortname = document.getElementById('<%= txtFacilityShortName.ClientID %>').value;
            var FacDesc = document.getElementById('<%= txtFacilityDescription.ClientID %>').value;
            var corporate = document.getElementById('<%= ddlCorporate.ClientID %>').value;
            if (Facshortname != '' || FacDesc != '' || corporate != 0) {
                args.IsValid = true;
                //alert("Select one type");
                return true;
                //alert("Select one type");
            }
            else {
                //alert("not chk");
                args.IsValid = false;
            }

        }


        function validationclear() {
            $("#<%= reqfieldFacilityShortName.ClientID %>").css("display", "none");
            $("#<%= reqfieldFacilityDescription.ClientID %>").css("display", "none");
            $("#<%= Req_ID.ClientID %>").css("display", "none");
        }
        //function TestCheck() {
        //    return ValidationFacility();
        //}

        <%--  function ValidationFacility() {
            var txtbxresult = false;
            var facilityshortname = document.getElementById('<%= txtFacilityShortName.ClientID %>').value.trim();
            var facilityname = document.getElementById('<%= txtFacilityDescription.ClientID %>').value.trim();
            var techperson = document.getElementById('<%= txttechperson.ClientID %>').value.trim();
            var techemail = document.getElementById('<%= txttechemail.ClientID %>').value.trim();
            var ddlcorp = document.getElementById('<%= ddlCorporate.ClientID %>').value.trim();
            <%-- var phone = document.getElementById('<%= txtPhone.ClientID %>').value.trim();
            var fax = document.getElementById('<%= txtFax.ClientID %>').value.trim();--%>
        //    if (facilityname == "" && facilityshortname == "" && ddlcorp == 0) {
        //        alert("Enter all mandatory Fields");
        //    } else if (facilityname == "" && facilityshortname == "") {
        //        alert("Facility Name and Facility Description are mandatory Fields");
        //    } else if (facilityname == "" && ddlcorp == 0) {
        //        alert("Facility Description and Corporate are mandatory Fields");
        //    } else if (facilityshortname == "" && ddlcorp == 0) {
        //        alert("Facility Name and Corporate are mandatory Fields");
        //    } else if (facilityshortname == "") {
        //        alert("Facility Name is mandatory Field");
        //    } else if (facilityname == "") {
        //        alert("Facility Description is mandatory Field");
        //    } else if (ddlcorp == 0) {
        //        alert("Corporate is mandatory Field");
        //    } else {
        //        txtbxresult = true;
        //    }
        //    return txtbxresult;
        //}--%>

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="page-title-breadcrumb">
        <div class="page-header">
            <div class="page-header page-title">
                Facility
            </div>
        </div>
    </div>
    <script type="text/javascript">
        Sys.Application.add_load(jScript);
        Sys.Application.add_load(jscriptsearch);

    </script>
    <div id="MainBoby" runat="server" class="mypanel-body">
        <div class="row" id="pnlSearch">
            <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                <asp:Label ID="lblseroutHeader" runat="server" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
            </div>
            <div class="col-sm-4 col-md-4 col-lg-4">
            </div>
            <div id="div_SearchDiv" class="col-sm-4 col-md-4 col-lg-4" align="right">
                <asp:Button ID="btnSearchFacility" runat="server" Text="Search"
                    CssClass="btn btn-primary" OnClick="btnSearchFacility_Click" />
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="EmptyField"
                    OnClick="btnSave_Click" Visible="false" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary"
                    OnClick="btnPrint_Click" />
                <asp:Button ID="btnDetailsPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnDetailsPrint_Click" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btncancelsave" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btncancelsave_Click" Visible="false" />
            </div>
        </div>

        <div id="DivSearch" class="row" runat="server" style="margin: 5px 0px 0px 0px;">
            <div class="well well-sm">
                <div class="row">
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <div class="form-group">
                            <span style="font-weight: 800;">Facility Code <i class="fa fa-info-circle" title="Use % for wild card search"></i></span>
                            <%--  <asp:Label ID="Label4" runat="server" Text="Facility Code\Description"></asp:Label><span style="color: red; padding-right: 5px;">*</span>--%>
                            <asp:TextBox ID="txtFacility" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <div class="form-group">
                            <span style="font-weight: 800;">Facility Description <i class="fa fa-info-circle" title="Use % for wild card search"></i></span>
                            <asp:TextBox ID="txtfacilitydescr" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <div class="form-group">
                            <span style="font-weight: 800;">Status</span>
                            <asp:RadioButtonList ID="rdbstatus" runat="server" RepeatDirection="Horizontal" CssClass="radiostyle">
                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                                <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div id="div_ADDContent" runat="server" style="display: none; margin-right: 5px">
            <span style="margin-top: 1px; font-weight: bolder;">Facility</span>
            <div class="row" style="margin-left: 0px">
                <div class="col-sm-12 col-md-12 col-lg-12" style="border-style: solid; border-color: #b2b7bb; background-color: #f5f5f5; border-width: 2px; border-radius: 5px;">
                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:Label ID="lblFacilityShortName" runat="server" Text="Facility Code"></asp:Label><span style="color: red"> *</span>
                                <asp:TextBox ID="txtFacilityShortName" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                <%--<asp:CustomValidator ID="cusfacshortname" ErrorMessage="" SetFocusOnError="true" Display="Dynamic"
                                    ForeColor="Red" ClientValidationFunction="Validatetxtbox" ValidationGroup="A" runat="server" Font-Size="0.9em" />--%>
                                <asp:RequiredFieldValidator ID="reqfieldFacilityShortName" runat="server" ControlToValidate="txtFacilityShortName" ValidationGroup="EmptyField" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>

                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblFacilityName" runat="server" Text="Facility Description"></asp:Label><span style="color: red"> *</span>
                                <asp:TextBox ID="txtFacilityDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                <%--<asp:CustomValidator ID="cusvalfacdesc" ErrorMessage="" SetFocusOnError="true" Display="Dynamic"
                                    ForeColor="Red" ClientValidationFunction="Validatetxtbox" ValidationGroup="A" runat="server" Font-Size="0.9em" />--%>
                                <asp:RequiredFieldValidator ID="reqfieldFacilityDescription" runat="server" ControlToValidate="txtFacilityDescription" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                <%-- <ajax:ValidatorCalloutExtender ID="ValFacilityDescription" TargetControlID="reqfieldFacilityDescription" PopupPosition="BottomRight" runat="server" Width="160px"></ajax:ValidatorCalloutExtender>--%>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:Label ID="lblGPAccountCode" runat="server" Text="Facility Accounting Code"></asp:Label>
                                <asp:TextBox ID="txtGPAccountCode" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:Label ID="lblEMRCode" runat="server" Text="EMR Code"></asp:Label>
                                <asp:TextBox ID="txtEMRCode" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblCopyFromExistingFacility" Width="190px" runat="server" Text="Copy From Existing Facility"></asp:Label>
                                <asp:DropDownList ID="ddlCopyFromExistingFacility" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:Label ID="lbltxperweek" runat="server" Text="Tx per Week"></asp:Label>
                                <asp:TextBox ID="txttxperweek" runat="server" CssClass="form-control"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" TargetControlID="txttxperweek"></ajax:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblFCorporate" runat="server" Text="Corporate"></asp:Label><span style="color: red; padding-right: 5px;"> *</span>
                                <asp:DropDownList ID="ddlCorporate" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                                <%-- <asp:CustomValidator ID="cusvalcorporate" ErrorMessage="" SetFocusOnError="true" Display="Dynamic"
                                    ForeColor="Red" ClientValidationFunction="Validatetxtbox" ValidationGroup="A" runat="server" Font-Size="0.9em" />--%>
                                <asp:RequiredFieldValidator InitialValue="0" ID="Req_ID" Display="Dynamic" ValidationGroup="EmptyField" runat="server"
                                    ControlToValidate="ddlCorporate" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em"></asp:RequiredFieldValidator>
                                <%--<ajax:ValidatorCalloutExtender ID="ValddlCorporate" TargetControlID="Req_ID" PopupPosition="BottomRight" runat="server" Width="160px"></ajax:ValidatorCalloutExtender>--%>
                            </div>
                        </div>

                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:Label ID="lblPatientCensus" runat="server" Text="Patient Census"></asp:Label>
                                <asp:TextBox ID="txtPatientCensus" runat="server" CssClass="form-control"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" TargetControlID="txtPatientCensus"></ajax:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:Label ID="lblEmployeeCensus" runat="server" Text="Employee Census"></asp:Label>
                                <asp:TextBox ID="txtEmployeeCensus" runat="server" CssClass="form-control"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" TargetControlID="txtEmployeeCensus"></ajax:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:CheckBox ID="chkactive" runat="server" Text="Isactive" Checked="true" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6">
                    <span style="font-weight: bolder;">Shipping</span>

                    <div class="col-lg-12 col-md-12 col-sm-12" style="border-style: solid; border-color: #b2b7bb; background-color: #f5f5f5; border-width: 2px; border-radius: 5px; padding: 0px">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <div class="form-group">
                                <asp:Label ID="lblAddress1" runat="server" Text="Address 1"></asp:Label>
                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblAddress2" runat="server" Text="Address 2"></asp:Label>
                                <asp:TextBox ID="txtAddress2" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender FilterType="UppercaseLetters, LowercaseLetters" runat="server" TargetControlID="txtCity"></ajax:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="lblZipCode" runat="server" Text="Zip"></asp:Label>
                                </div>
                                <div class="col-sm-6" style="padding-right: 0px;">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtZipcode" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'99999'"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="Regzip" runat="server"
                                            ErrorMessage="Invalid format" ControlToValidate="txtZipcode" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                            ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-sm-1" style="padding-left: 4px; padding-right: 0px; padding-top: 6px">- </div>
                                <div class="col-sm-5" style="padding-left: 0px;">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtZipcode1" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'9999'"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="Regzip1" runat="server"
                                            ErrorMessage="Invalid format" ControlToValidate="txtZipcode1" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                            ValidationExpression="^\d{4}$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'(999) 999-9999'"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Regphone" runat="server"
                                    ErrorMessage="Invalid format" ControlToValidate="txtPhone" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblxtn" runat="server" Text="Xtn"></asp:Label>
                                <asp:TextBox ID="txtxtn" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtxtn"></ajax:FilteredTextBoxExtender>
                                <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ErrorMessage="Invalid format" ControlToValidate="txtxtn" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                        ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>--%>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label>
                                <%--<ajax:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" TargetControlID="txtFax" ValidChars="-"></ajax:FilteredTextBoxExtender>--%>
                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'(999) 999-9999'"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Regfax" runat="server"
                                    ErrorMessage="Invalid format" ControlToValidate="txtFax" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6" style="padding: 0px 0px 0px 0px">
                    <span style="font-weight: bolder;">Billing</span>

                    <div class="col-lg-12" style="border-style: solid; border-color: #b2b7bb; border-width: 2px; background-color: #f5f5f5; border-radius: 5px; padding: 0px; padding-right: 5px">
                        <div class="col-sm-3 col-md-3 col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblBillAdrress1" runat="server" Text="Address 1"></asp:Label>
                                <asp:TextBox ID="txtBillAddress1" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="lblBillAdrress2" runat="server" Text="Address 2"></asp:Label>
                                <asp:TextBox ID="txtBillAddress2" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblBillCity" runat="server" Text="City"></asp:Label>
                                <asp:TextBox ID="txtBillCity" runat="server" CssClass="form-control"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender FilterType="UppercaseLetters, LowercaseLetters" runat="server" TargetControlID="txtBillCity"></ajax:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblBillState" runat="server" Text="State"></asp:Label>
                                <asp:DropDownList ID="ddlBillState" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="lblBillZip" runat="server" Text="Zip"></asp:Label>
                                </div>
                                <div class="col-sm-6" style="padding-right: 0px;">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtBillZip" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'99999'"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="Regzipcode" runat="server"
                                            ErrorMessage="Invalid format" ControlToValidate="txtBillZip" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                            ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-sm-1" style="padding-left: 4px; padding-right: 0px; padding-top: 6px">- </div>
                                <div class="col-sm-5" style="padding-left: 0px;">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtBillZip1" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'9999'"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="Regzipcode1" runat="server"
                                            ErrorMessage="Invalid format" ControlToValidate="txtBillZip1" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                            ValidationExpression="^\d{4}$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblBillPhone" runat="server" Text="Phone"></asp:Label>
                                <asp:TextBox ID="txtBillPhone" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'(999) 999-9999'"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Regbill" runat="server"
                                    ErrorMessage="Invalid format" ControlToValidate="txtBillPhone" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblxtnbill" runat="server" Text="Xtn"></asp:Label>
                                <asp:TextBox ID="txtxtnbill" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtxtnbill"></ajax:FilteredTextBoxExtender>
                                <%-- <asp:RegularExpressionValidator ID="Regxtn" runat="server"
                                        ErrorMessage="Invalid format" ControlToValidate="txtxtnbill" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                        ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>--%>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-4">
                            <div class="form-group">
                                <asp:Label ID="lblBillFax" runat="server" Text="Fax"></asp:Label>
                                <%--<ajax:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" TargetControlID="txtBillFax" ValidChars="-"></ajax:FilteredTextBoxExtender>--%>
                                <asp:TextBox ID="txtBillFax" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'(999) 999-9999'"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Regbillfax" runat="server"
                                    ErrorMessage="Invalid format" ControlToValidate="txtBillFax" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div id="FacilityUserRole" runat="server" style="display: none">
                <div class="row">
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <div class="form-group">
                            <span style="font-weight: 800;">User Role</span>
                            <asp:ListBox ID="drpUserRole" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpUserRole_SelectedIndexChanged"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Label ID="lblcountrole" runat="server">No of records : <%=grdFacilityUserRole.Rows.Count.ToString() %></asp:Label>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12" style="padding-right: 0px; height: 100%; width: 100%; overflow: auto; z-index: 1;">
                        <asp:GridView ID="grdFacilityUserRole" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                            CssClass="table table-responsive" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="UserRole" HeaderText="User Role" />
                                <asp:BoundField DataField="UserName" HeaderText="Name" />
                                <asp:BoundField DataField="PhoneNo" HeaderText="Phone" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
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
        <div id="div_Grid" runat="server">
            <div class="row">
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <asp:Label ID="lblresult" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                </div>
            </div>
            <div>
                <asp:Label ID="lblrowcount" runat="server">No of records : <%=grdFacility.Rows.Count.ToString() %></asp:Label>
            </div>
            <div class="row vendormastergrid" style="margin-top: 0px;">
                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12" style="padding-right: 0px; height: 100%; width: 100%; overflow: auto; z-index: 1;">
                    <asp:GridView ID="grdFacility" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                        CssClass="table table-responsive" OnRowDataBound="grdFacility_RowDataBound" OnPageIndexChanging ="grdFacility_PageIndexChanging" AllowPaging ="true" PageSize ="15">
                        <Columns>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ToolTip="Edit" OnClick="btnEdit_Click" ImageUrl="~/Images/edit.png" />
                                    <asp:ImageButton ID="lbldelete" runat="server" Text="delete" Height="20px" ToolTip="Delete" OnClick="lbldelete_Click" ImageUrl="~/Images/Delete.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--           <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnPrint" runat="server" Text="Delete" Height="20px" OnClick="imgbtnPrint_Click" ImageUrl="~/Images/Print.png" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                            <asp:BoundField DataField="FacilityID" HeaderText="Facility ID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="FacilityShortName" HeaderText="Facility Code" HeaderStyle-Width="9%" />
                            <asp:BoundField DataField="FacilityDescription" HeaderText="Facility Description" />
                            <asp:BoundField DataField="Address1" HeaderText="Address 1" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="Address2" HeaderText="Address 2" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="City" HeaderText="City" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="State" HeaderText="State" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="Zipcode" HeaderText="Zipcode" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="Fax" HeaderText="Fax" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="BillAddress1" HeaderText="Bill Address 1" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="BillAddress2" HeaderText="Bill Address 2" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="BillCity" HeaderText="Bill City" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="BillState" HeaderText="Bill State" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="BillZipcode" HeaderText="Bill Zipcode" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="BillPhone" HeaderText="Bill Phone" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="BillFax" HeaderText="Bill Fax" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="Corporate" HeaderText="Corporate" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="GPAccountCode" HeaderText="Facility Acct Code" HeaderStyle-Width="9%" />
                            <asp:BoundField DataField="EMRCode" HeaderText="EMR Code" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="TechPerson" HeaderText="Contact Name" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="TechPhone" HeaderText="Tech Phone" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="TechEmail" HeaderText="E-Mail" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="AdminPerson" HeaderText="Admin Person" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="AdminPhone" HeaderText="Admin Phone" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="AdminEmail" HeaderText="Admin Email" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="PatientCensus" HeaderText="Patient Census" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="TxWeek" HeaderText="Tx per Week" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="Xtn" HeaderText="Xtn" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="BillXtn" HeaderText="Bill Xtn" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:TemplateField HeaderText="Active" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" Text='<%# ((string)Eval("IsActive").ToString() == "True" ? "Yes" : "No")  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EmployeeCensus" HeaderText="Patient Census" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                        </Columns>
                        <HeaderStyle CssClass="Headerstyle" />
                        <FooterStyle CssClass="gridfooter" />
                        <PagerStyle CssClass="gridviewPager" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle CssClass="gridselectedrow" />
                        <EditRowStyle CssClass="grideditrow" />
                        <AlternatingRowStyle CssClass="gridalterrow" />
                        <RowStyle CssClass="gridrow" />
                    </asp:GridView>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hiddenFacilityID" Value="0"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hdnfield" Value="0"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="HddFacilityCode" Value="0"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="HddFacilityAcctCode" Value="0"></asp:HiddenField>
        </div>

    </div>
    <div id="modalSave" class="modal fade" style="position: center">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <div class="modal-header bg-green">
                    <h4 class="modal-title font-bold text-white">Facility
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
                    <h4 class="modal-title font-bold text-white">Facility
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
                    <h4 class="modal-title font-bold text-white">Facility
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
    <div id="modalConfirm" class="modal fade" style="position: center">
        <div class="modal-dialog modal-sm">
            <div class="modal-content ">
                <div class="modal-header bg-red">
                    <h4 class="modal-title font-bold text-white">Delete Confirmation</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                </div>
                <div class="modal-body">
                    <p>Do you want to delete this record <span id="spnreName"></span>?</p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btndeletepop" runat="server" Text="Yes" OnClick="btndeletepop_Click" CssClass="btn btn-danger" />
                    <%--<asp:Button ID="btncancelpop" runat="server" OnClick="btncancelpop_Click" Text="Close" class="btn btn-default ra-100" />--%>
                    <button type="button" class="btn btn-default ra-100" data-dismiss="modal" cssclass="btn btn-danger">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div id="User_Permission_Message" runat="server" visible="false">
        <br />
        <br />
        <br />
        <h4>
            <center> This User doesn't have permission to view this screen</center>
        </h4>
    </div>



    <asp:Button ID="Button2" runat="server" Style="display: none" />

    <asp:Panel ID="Panel2" runat="server" BackColor="White" Height="100px" Width="400px" Style="display: none;">
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
                    <asp:Label ID="Label1" runat="server" Text="Are you sure you want to delete this Record?" />
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
    <asp:Panel ID="Panel1" runat="server" Style="display: none; height: auto" BackColor="#666699">
    </asp:Panel>
    <asp:Label ID="lblmsg" runat="server"></asp:Label>

    <asp:Button ID="btnmsg" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="mpemsg" runat="server" TargetControlID="btnmsg" PopupControlID="pnlmsg"
        CancelControlID="btnclose" BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlmsg" runat="server" BackColor="White" Height="100px" Width="200px" Style="display: none;">
        <table width="100%" style="border: Solid 2px #0271dd; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
            <tr style="background-color: #0271dd">
                <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">User Details</td>
                <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right">
                    <a href="javascript:void(0)" onclick="closepopup()">
                        <img src="Images/Close.gif" style="border: 0px" align="right" /></a>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                    <asp:Label ID="lblalert" runat="server" Text="Saved Successfully" />
                </td>
            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td></td>
                <td align="right" style="padding-right: 15px">

                    <asp:Button ID="btnclose" runat="server" Text="Ok" OnClientClick="return Validate();" />
                </td>
            </tr>
        </table>
    </asp:Panel>

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
                    <asp:ImageButton ID="btnYes1" OnClick="btnYes_Click" runat="server" ImageUrl="~/Images/btnyes.jpg" />
                    <asp:ImageButton ID="btnNo1" runat="server" ImageUrl="~/Images/btnNo.jpg" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div style="display: none">
        <rsweb:ReportViewer ID="rvFacilityreport" runat="server"></rsweb:ReportViewer>
    </div>
    <div style="display: none">
        <rsweb:ReportViewer ID="rvFacilityDetaislReport" runat="server"></rsweb:ReportViewer>
    </div>

</asp:Content>

