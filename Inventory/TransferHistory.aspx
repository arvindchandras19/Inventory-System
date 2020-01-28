<%@ Page Title="TransferHistory" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="TransferHistory.aspx.cs" Inherits="Inventory.TransferHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : TransferHistory.aspx 
' Type      : ASPX File 
' Description  :   To design the TransferHistory page for add,Update and show the  TransferHistory page on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                                           Reason 
  03/17/2018             V.01              Sairam.P ,Dhanasekaran.C                      New
  04/06/2018             V.02              Sairam.P                                      Added(In Each)in the grid
'******************************************************************************************************/
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Common.css" rel="stylesheet" />
    <link href="Content/sumoselect.css" rel="stylesheet" />
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

        .HeaderStyleLable {
            color: #000;
            font-family: 'Trocchi', serif;
            font-size: 20px;
            font-weight: bold;
            line-height: 18px;
            margin: 0;
        }

        .box {
            height: 100%;
            display: flex;
            align-items: center; /* align vertical */
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
          .LeftPadding {
            padding-left: 15px;
        }
        
        .outPopUp {
            position: absolute;
            width: 900px;
            max-height: 400px;
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

        function CorpDrop() {
            $('[id*=drpcorsearch]').change(function (event) {
                if ($(this).val().length > 1) {
                    var val = $(this).val() || [];     
                    alert('Multiple selection are not allowed here. Use Multi select link for multiple selection.');
                    var $this = $(this);   

                } else {
                    last_valid_selection = $(this).val();

                }
            });

            $('[id*=drpfacilitysearch]').change(function (event) {

                if ($(this).val().length > 1) {
                    var val = $(this).val() || [];   
                    alert('Multiple selection are not allowed here. Use Multi select link for multiple selection.');
                    var $this = $(this);

                } else {
                    last_valid_selection = $(this).val();
                }
            });
        }

    </script>
    <script type="text/javascript">
        function jScript() {
            $('[id*=drpcorsearch]').SumoSelect({
                selectAll: false,
                placeholder: 'Select Corporate'
            });

            $('[id*=drpfacilitysearch]').SumoSelect({
                selectAll: false,
                placeholder: 'Select Facility'
            });

            $('[id*=drpItemCategorysearch]').SumoSelect({
                selectAll: true,
                placeholder: 'Select Category'
            });

            $('[id*=drpStatussearch]').SumoSelect({
                selectAll: true,
                placeholder: 'Select Status'
            });
        }

        function jscriptsearch() {
            var config = {
                '.chosen-select': {},
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }

        function GetgrdOrderSearchIndexValue(orderid) {
            var row = orderid.parentNode.parentNode;
            grdMPOIndex = row.rowIndex - 1;
            document.getElementById('<%=HdnMPRDetailsID.ClientID%>').value += (grdMPOIndex) + ',';
        }
        function Remarkspopupshow() {
            $(document).ready(function () {
                $('[id*=imgreadmore]').on('mouseover', function () {
                    var a = "Click here to read more";
                    $('[id*=imgreadmore]').attr('title', a);
                })
                $('[data-toggle="popover"]').popover({
                    placement: 'top',
                    html: true,
                    title: 'Remarks <a href="#" class="tooltipclose" data-dismiss="alert">&times;</a>'
                });

                $(document).on("click", ".popover .tooltipclose", function () {
                    $(this).parents(".popover").popover('hide');
                });

                $('body').on('click', function (e) {
                    $('[data-toggle=popover]').each(function () {
                        if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                            $(this).popover('hide');
                        }
                    });
                });
            });
        }

        function Auditpopupshow() {
            $(document).ready(function () {
                $('[id*=imgreadmore]').on('mouseover', function () {
                    var a = "Click here to read more";
                    $('[id*=imgreadmore]').attr('title', a);
                })
                $('[data-toggle1="popover"]').popover({
                    placement: 'top',
                    html: true,
                    title: 'Audit Trail <a href="#" class="tooltipclose" data-dismiss="alert">&times;</a>'
                });

                $(document).on("click", ".popover .tooltipclose", function () {
                    $(this).parents(".popover").popover('hide');
                });

                $('body').on('click', function (e) {
                    $('[data-toggle1=popover]').each(function () {
                        if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                            $(this).popover('hide');
                        }
                    });
                });
            });
        }


        $(document).on("keyup mouseup", "[id*=txtTransferqty]", function () {
            var row = $(this).closest("tr");
            if (!jQuery.trim($(this).val()) == '') {
                if (!isNaN(parseFloat($(this).val()))) {
                    var row = $(this).closest("tr");
                    //console.log(($("[id*=lblPrice]", row).html().replace("$", "")));
                    var lbltotprice = parseFloat(parseFloat(($("[id*=lblPrice]", row).html().replace("$", "").trim()) * $(this).val()));
                    if (isNaN(lbltotprice))
                        $("[id*=lblTotalPrice]", row).html('');
                    else
                        $("[id*=lblTotalPrice]", row).html('$' + parseFloat(lbltotprice.toString()).toFixed(2));


                    var txtreason = $("[id*=txtRemarks]", row);
                    var lbltransferqty = $("[id*=lbltransferqty]", row).html();
                    if (lbltransferqty != '') {
                        if ($(this).val() != lbltransferqty) {

                            if (jQuery.trim(txtreason.val()) == '') {
                                txtreason.css({ "border": "Solid 1px red" });
                            }
                            else {
                                txtreason.css({ "border": "Solid 1px #a9a9a9" })
                            }

                        } else {
                            txtreason.css({ "border": "Solid 1px #a9a9a9" })

                        }
                    }

                    if ($(this).val() == '') {
                        $("[id*=lblTotalPrice]", row).html('');
                        $(this).val("");
                    }
                }
            } else {
                $(this).val('');
                $("[id*=lblTotalPrice]", row).html('');
            }

        });


        $(function () {
            $(document).on("keyup mouseup", "[id*=grdTransferOut] [id*=txtRemarks]", function () {
                var row = $(this).closest("tr");
                var txtreason = $(this);
                var lbltransferqty = $("[id*=lbltransferqty]", row).html();
                var txtTransferqty = $("[id*=txtTransferqty]", row).val();
                if (lbltransferqty != '') {
                    if (txtTransferqty != lbltransferqty) {
                        if (jQuery.trim(txtreason.val()) == '') {
                            txtreason.css({ "border": "Solid 1px red" });
                        }
                        else {
                            txtreason.css({ "border": "Solid 1px #a9a9a9" })
                        }

                    } else {
                        txtreason.css({ "border": "Solid 1px #a9a9a9" })
                    }
                }

            });
        });



        function ShowPopup(res) {

            $('[id*=lblsave]').html(res);

            $("#modalSave").modal("show");

        }
        function ShowdelPopup(res) {

            $('[id*=lbldelete]').html(res);

            $("#modalDelete").modal("show");

        }
        function ShowwarningPopup(res) {
            $('[id*=lblwarning]').html(res);
            $("#modalWarning").modal("show");

        }
        function ShowwarningLookupPopup(res) {
            $('[id*=lblwarning]').html(res);
            $("#modalWarning").modal("show");
        }
        function ShowConfirmationPopup() {
            $("#modalDelete").modal("show");
        }

        function HideConfirmationPopup() {
            $("#modalDelete").modal("Hide");
        }

//$(".datepicker_monthyear").bsdatepicker({
//                    format: "MMM-yyyy",
//                    viewMode: "months",
//                    minViewMode: "months",
//                    endDate: "today",
//                    maxDate: today,
//                }).on('changeDate', function (ev) {
//                    $(this).bsdatepicker('hide');
//                });
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Transfer History
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(Auditpopupshow);
                    Sys.Application.add_load(Remarkspopupshow);
                    Sys.Application.add_load(jScript);
                    Sys.Application.add_load(jscriptsearch);
                    Sys.Application.add_load(CorpDrop);
                    //Sys.Application.add_load(LoadDatePicker);
                </script>
                  <asp:HiddenField ID="HddListCorpID" Value="0" runat="server" />
                  <asp:HiddenField ID="HddListFacID" Value="0" runat="server" />
                    <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="DivMultiCorp">
                    <%--<asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnuploadfile" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                    <asp:Label ID="lblMultiCorp" runat="server" CssClass="page-header page-title" Text="Select Multiple Corporate"></asp:Label><br />
                      <asp:Label ID="lblrow" runat="server">No of records : <%=GrdMultiCorp.Rows.Count.ToString() %></asp:Label>
                    <div class="row" style="padding: 10px;">
                        <div class="col-lg-12 col-md-12 col-sm-12" style="overflow-y: scroll;height:200px;">
                            <asp:GridView ID="GrdMultiCorp" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="ChkAllCorp" runat="server" AutoPostBack="true" OnCheckedChanged="ChkAllCorp_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkmultiCorp" runat="server" />
                                            <asp:Label ID="lblCorpID" runat="server" Text=' <%# Eval("CorporateID")%>' CssClass="HeaderHide"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Corporate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCorpname" runat="server" Text=' <%# Eval("CorporateName")%>'></asp:Label>
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

                    <div class="row" style="margin-top: 5px;">
                        <div class="col-lg-6 col-md-6 col-sm-6 form-group">
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 " align="right">
                            <br />
                            <asp:Button ID="btnMultiCorpselect" runat="server" Text="Select" CssClass="btn btn-primary" OnClick="btnMultiCorpselect_Click" />
                            <asp:Button ID="btnMultiCorpClose" runat="server" Text="Close" CssClass="btn btn-success" OnClick="btnMultiCorpClose_Click" />
                        </div>
                    </div>
                </div>

                <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="DivFacCorp">
                    <%--<asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnuploadfile" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                    <asp:Label ID="lblMultiFac" runat="server" CssClass="page-header page-title" Text="Select Multiple Facility"></asp:Label><br />
                      <asp:Label ID="lbrow" runat="server">No of records : <%=GrdMultiFac.Rows.Count.ToString() %></asp:Label>
                    <div class="row" style="padding: 10px;">
                        <div class="col-lg-12 col-md-12 col-sm-12" style="overflow-y: scroll; height: 200px;">
                            <asp:GridView ID="GrdMultiFac" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="ChkAllFac" runat="server" AutoPostBack="true" OnCheckedChanged="ChkAllFac_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkmultiFac" runat="server" />
                                            <asp:Label ID="lblFacID" runat="server" Text=' <%# Eval("FacilityID")%>' CssClass="HeaderHide"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Facility">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFacname" runat="server" Text=' <%# Eval("FacilityDescription")%>'></asp:Label>
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

                    <div class="row" style="margin-top: 5px;">
                        <div class="col-lg-6 col-md-6 col-sm-6 form-group">
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 " align="right">
                            <br />
                            <asp:Button ID="btnMultiFacselect" runat="server" Text="Select" CssClass="btn btn-primary" OnClick="btnMultiFacselect_Click" />
                            <asp:Button ID="btnMultiFacClose" runat="server" Text="Close" CssClass="btn btn-success" OnClick="btnMultiFacClose_Click" />
                        </div>
                    </div>
                </div>

                <div id="divTransdferHistory" class="mypanel-body" runat="server" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-4" align="left">
                            <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-lg-8" align="right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" ValidationGroup="EmptyFieldSearch" />
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                        </div>
                    </div>
                    <div id="divMPRMaster" runat="server" style="margin-top: 5px;">
                        <div id="divContent" class="well well-sm">
                            <div class="row" align="center">
                                <asp:RadioButton ID="rdbtransferin" runat="server" Text="Tranfer IN" AutoPostBack="true" OnCheckedChanged="rdbtransferin_CheckedChanged" Checked="true" />
                                &nbsp&nbsp
                                <asp:RadioButton ID="rdbtransferout" runat="server" Text="Tranfer Out" AutoPostBack="true" OnCheckedChanged="rdbtransferout_CheckedChanged" />
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiCorp" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearCorp" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllCorp" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllCorp_Click"></asp:LinkButton>  
                                        <asp:ListBox ID="drpcorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpcorsearch_SelectedIndexChanged"></asp:ListBox>
                                         <asp:RequiredFieldValidator InitialValue="" ID="ReqdrpCorporate" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcorsearch" ErrorMessage="This information is required."  SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <%-- <asp:DropDownList ID="drpcorsearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiFac" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearFac" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllFac" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllFac_Click"></asp:LinkButton>    
                                        <asp:ListBox ID="drpfacilitysearch" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="drpfacilitysearch_SelectedIndexChanged"></asp:ListBox>
                                          <asp:RequiredFieldValidator InitialValue="" ID="ReqdrpFacility" runat="server" ControlToValidate="drpfacilitysearch"  ValidationGroup="EmptyFieldSearch"
                                            ErrorMessage="This information is required." ForeColor="Red"  SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <%-- <asp:DropDownList ID="drpfacilitysearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Item Category</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="" ID="ReqItemCategory" runat="server" ControlToValidate="drpItemCategorysearch" ValidationGroup="EmptyFieldSearch"
                                            ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:ListBox ID="drpItemCategorysearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple"></asp:ListBox>
                                        <%--  <asp:DropDownList ID="drpItemCategorysearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Status</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="" ID="ReqdrpStatus" runat="server" ControlToValidate="drpStatussearch" ValidationGroup="EmptyFieldSearch"
                                            ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                        <%--  <asp:DropDownList ID="drpStatussearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                        <asp:ListBox ID="drpStatussearch" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date From</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="ReqDateFrom" runat="server" ControlToValidate="txtDateFrom" ValidationGroup="EmptyFieldSearch"
                                            ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateFrom" runat="server" TargetControlID="txtDateFrom" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilDateFrom" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateFrom"></ajax:FilteredTextBoxExtender>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date To</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="ReqDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="EmptyFieldSearch"
                                            ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CaltxtDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>            
                                        <ajax:FilteredTextBoxExtender ID="FilDateTo" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateTo"></ajax:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="margin-left: 1px; margin-top: 3px;" id="divsearchResult">
                            <div class="row">
                                <div class="col-lg-3" align="left">
                                    <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                </div>
                            </div>
                            <asp:HiddenField ID="HdnMPRDetailsID" runat="server" />
                            <asp:HiddenField ID="HddMasterID" runat="server" />
                            <div style="margin-left: 1px; margin-top: 3px;" id="divsearch" runat="server">
                                <asp:Label ID="lblrcount" runat="server">No of records : <%=grdTransferin.Rows.Count.ToString() %></asp:Label>
                                <div class="MPRSearchgrid">
                                    <asp:GridView ID="grdTransferin" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found">
                                        <Columns>
                                            <asp:TemplateField HeaderText="TransferOutID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferOutID" runat="server" Text='<%# Eval("TransferOutID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transfer ID" ItemStyle-Width="9%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferNo" runat="server" Text='<%# Eval("TransferNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transfer Date" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferDate" runat="server" Text='<%# Eval("TransferDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CorporateIDFrom" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCorporateFromID" runat="server" Text='<%# Eval("CorporateIDFrom")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CorporateIDTo" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCorporateToID" runat="server" Text='<%# Eval("CorporateIDTo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FacilityIDFrom" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFacilityIDFrom" runat="server" Text='<%# Eval("FacilityIDFrom")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FacilityIDTo" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFacilityIDTo" runat="server" Text='<%# Eval("FacilityIDTo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transferred From" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFacilityNameFrom" runat="server" Text='<%# Eval("FacilityFrom")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemID" HeaderStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ItemID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Category" HeaderStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryID")%>' CssClass="HeaderHide"></asp:Label>
                                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("CategoryName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("ItemDescription")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uom" HeaderStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluomID" runat="server" Text='<%# Eval("UOM")%>' CssClass="HeaderHide"></asp:Label>
                                                    <asp:Label ID="lbluomName" runat="server" Text='<%# Eval("UomName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty Pack" HeaderStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQtyPack" runat="server" Text='<%# Eval("QtyPack")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price ($)" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price","$ {0:#,0.00}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Received Qty(In Each)" ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferqty" runat="server" Text='<%# Eval("Transferqty")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Price ($)" HeaderStyle-Width="9%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalPrice","$ {0:#,0.00}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblaudit" runat="server" Text='<%# Eval("Audit")%>'></asp:Label>--%>
                                                     <asp:Image ID="imgreadmoreaudit" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
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
                        <asp:HiddenField ID="hdncheckfield" Value="0" runat="server" />
                        <asp:HiddenField ID="HdnTransferNo" Value="0" runat="server" />
                    </div>
                    <div style="margin-left: 1px; margin-top: 3px; display:none;" id="divsearchOut" runat="server">
                             <asp:Label ID="lblcount1" runat="server">No of records : <%=grdTransferOut.Rows.Count.ToString() %></asp:Label>
                        <div class="MPRSearchgrid">
                            <asp:GridView ID="grdTransferOut" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="grdTransferOut_RowDataBound">
                                        <Columns>
                                       <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                               <ItemTemplate>
                                                  <asp:ImageButton ID="imgEdit" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/edit.png" OnClick="imgEdit_Click"/>
                                                  <asp:ImageButton ID="imgVoid" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/void.png" OnClick="imgVoid_Click"/>
                                                  <asp:ImageButton ID="imgSave" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/save.png" OnClick="imgSave_Click" Visible="false"/>
                                                   <asp:Label ID="lblTransferOutID" runat="server" Text='<%# Eval("TransferOutID")%>' CssClass="HeaderHide"></asp:Label>
                                              </ItemTemplate>
                                        </asp:TemplateField>                                             
                                            <asp:TemplateField HeaderText="Transfer ID" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferNo" runat="server" Text='<%# Eval("TransferNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TransferDate" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransferDate" runat="server" Text='<%# Eval("TransferDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CorporateIDFrom" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCorporateFromID" runat="server" Text='<%# Eval("CorporateIDFrom")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CorporateIDTo" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCorporateToID" runat="server" Text='<%# Eval("CorporateIDTo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transferredfrom" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFacilityIDFrom" runat="server" Text='<%# Eval("FacilityIDFrom")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transferredto" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFacilityIDTo" runat="server" Text='<%# Eval("FacilityIDTo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transferred To" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFacilityNameTo" runat="server" Text='<%# Eval("FacilityTo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemID" HeaderStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ItemID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Category" HeaderStyle-Width="7%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryID")%>' CssClass="HeaderHide"></asp:Label>
                                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("CategoryName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("ItemDescription")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uom" HeaderStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluomID" runat="server" Text='<%# Eval("UOM")%>' CssClass="HeaderHide"></asp:Label>
                                                    <asp:Label ID="lbluomName" runat="server" Text='<%# Eval("UomName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty Pack" HeaderStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQtyPack" runat="server" Text='<%# Eval("QtyPack")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price ($)" HeaderStyle-Width="9%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price","$ {0:#,0.00}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transfer Qty(In Each)" HeaderStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTransferqty" runat="server" Enabled="false" Text='<%# Eval("Transferqty")%>' Width="100%"></asp:TextBox>
                                                    <ajax:FilteredTextBoxExtender ID="FiltertxtTransferqty" FilterType="Numbers" runat="server" TargetControlID="txtTransferqty"></ajax:FilteredTextBoxExtender>
                                                     <asp:Label ID="lbltransferqty" runat="server" Text='<%# Eval("Transferqty") %>' CssClass="hidden"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TotalPrice($)" HeaderStyle-Width="9%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalPrice","$ {0:#,0.00}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reason" HeaderStyle-Width="3%" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                     <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                     <%--<asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>--%> 
                                                    <asp:Image ID="imgreadremarks" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Remarks") %>' data-toggle="popover" />     
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="5%">
                                              <ItemTemplate>
                                                    <%--<asp:Label ID="lblaudit" runat="server" Text='<%# Eval("Audit")%>'></asp:Label>--%>
                                                     <asp:Image ID="imgreadaudit" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                <HeaderStyle CssClass="Headerstyle" />
                                <FooterStyle CssClass="gridfooter" />
                                <PagerStyle CssClass="pagerstyle" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle CssClass="gridselectedrow" />
                                <EditRowStyle CssClass="grideditrow" />
                                <AlternatingRowStyle CssClass="gridalterrow" HorizontalAlign="left" />
                                <RowStyle CssClass="gridrow" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="HiddenField1" Value="0" runat="server" />
                <asp:HiddenField ID="HiddenField2" Value="0" runat="server" />
                 <asp:HiddenField ID="HiddenIsVoid" runat="server" />
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
                                    <asp:ImageButton ID="btnImgDeletePopUp" runat="server" CssClass="btn btn-danger" AlternateText="Yes" />
                                    <button type="button" class="btn btn-default ra-100" data-dismiss="modal">Close</button>
                                    <%--<asp:ImageButton ID="ImageButtonNo" runat="server" ImageUrl="~/Images/btnNo.jpg"/>--%>
                                </div>
                            </div>
                        </div>
                    </div>

                <div id="modalSave" class="modal fade" style="position: center">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content ">
                            <div class="modal-header bg-green">
                                <h4 class="modal-title font-bold text-white">Transfer History
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="lblsave" runat="server"></asp:Label><asp:LinkButton ID="lbpopprint" runat="server" Text="Print" Visible="false"></asp:LinkButton>
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
                                <h4 class="modal-title font-bold text-white">Transfer History
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
                                <h4 class="modal-title font-bold text-white">Transfer History
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center> This User don't have a Permission to View This Page...</center>
            </h4>
        </div>
        <div style="display: none">
            <rsweb:ReportViewer ID="rvtransferinReport" runat="server"></rsweb:ReportViewer>
        </div>
        <div style="display: none">
            <rsweb:ReportViewer ID="rvtransferoutreport" runat="server"></rsweb:ReportViewer>
        </div>

</asp:Content>
