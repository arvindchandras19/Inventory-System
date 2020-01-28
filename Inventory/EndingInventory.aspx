<%@ Page Title="Ending Inventory" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="EndingInventory.aspx.cs" Inherits="Inventory.EndingInventory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : EndingInventory.aspx 
' Type      : ASPX File 
' Description  :   To design the Ending Inventory page for add,Update and show the Ending Inventory on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version                 By                          Reason 
 08/Mar/2018              V.01                S.Vivekanand                    New 
 04/JUNE/2018             V.02                Sairam.P                       Added (In Each) in all the grid.
'******************************************************************************************************/
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Common.css" rel="stylesheet" />
    <link href="Content/sumoselect.css" rel="stylesheet" />
    <link href="Content/datepicker.css" rel="stylesheet" />
    <script src="Scripts/datepicker.js" type="text/javascript"></script>
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

        .loading {
            font-family: Arial;
            font-size: 12pt;
            border: 4px solid #67CFF5;
            width: 260px;
            height: 60px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
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
            margin: -100px 0 0 -150px;
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

        .ErrorBorder {
            border: 1px solid red;
        }
    </style>

    <script type="text/javascript">
        function CorpDrop() {
            $('[id*=drpvendorsearch]').change(function (event) {

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

            $('[id*=drpvendorsearch]').SumoSelect({
                placeholder: 'Select Vendor'
            });

            $('[id*=drpItemCategory]').SumoSelect({
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



        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }


        function Remarkspopupshow() {
            $(document).ready(function () {
                $(function () {
                    $('[id*=imgreadmore]').on('mouseover', function () {
                        var a = "Click here to read more";
                        $('[id*=imgreadmore]').attr('title', a);
                    })
                });
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
                $(function () {
                    $('[id*=imgreadmore]').on('mouseover', function () {
                        var a = "Click here to read more";
                        $('[id*=imgreadmore]').attr('title', a);
                    })
                });
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

        function Remarkspopupshow() {
            $(document).ready(function () {
                $(function () {
                    $('[id*=imgreadmore]').on('mouseover', function () {
                        var a = "Click here to read more";
                        $('[id*=imgreadmore]').attr('title', a);
                    })
                });
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
                $(function () {
                    $('[id*=imgreadmore]').on('mouseover', function () {
                        var a = "Click here to read more";
                        $('[id*=imgreadmore]').attr('title', a);
                    })
                });
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
        function ShowwarningLookupPopup(res) {
            //alert('ggg');
            $('[id*=lblwarning]').html(res);

            $("#modalWarning").modal("show");

        }

        function ShowConfirmationPopup() {

            $("#modalConfirm").modal("show");

        }




        // Valiadtion 
        function ShowControls() {
            $(document).ready(function () {
                var today = new Date();
                $(".datepicker_monthyear").bsdatepicker({
                    format: "MMM-yyyy",
                    viewMode: "months",
                    minViewMode: "months",
                    endDate: "today",
                    maxDate: today,
                }).on('changeDate', function (ev) {
                    $(this).bsdatepicker('hide');
                    $("[id*=btnSave]").prop('disabled', true);
                    $("[id*=DIVgrdEndingInven]").css('display', 'none');
                    
                    var seldate = ev.date;
                    $('[id*=btnSearch]').prop('disabled', false);
                    if (seldate > today) {
                        ShowwarningPopup("Month/Year should not be more than system date.");
                        $('[id*=btnSearch]').prop('disabled', true);
                    }
                    
                });

                              
                $('[id*=drpItemCategory]').on("change", function () {
                    $("[id*=btnSave]").prop('disabled', true);
                    $("[id*=DIVgrdEndingInven]").css('display', 'none');
                });

                //$('.datepicker_monthyear').keyup(function () {
                //    if (this.value.match(/[^0-9]/g)) {
                //        this.value = this.value.replace(/[^0-9^-]/g, '');
                //    }
                //});


                //$(".datepicker_year").datepicker({
                //    format: "mm-yyyy",
                //    viewMode: "years",
                //    minViewMode: "years",
                //    autoclose: true,

                //}).on('changeDate', function (ev) {
                //    $(this).datepicker('hide');
                //});

            });            
        }

        
        //function MothCalculation(){
        $(document).ready(function () {
            $(document).on("keyup mouseup", "[id*=txtbeginv]", function () {
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var RecInven = $("[id*=lblReceiveingInven]", row).text();
                        var TransferIn = $("[id*=lblTransferIn]", row).text();
                        var TransferOut = $("[id*=lnkTransferOut]", row).text();
                        var expiredmeds = $("[id*=txtexpiredmeds]", row).val();
                        var endinv = $("[id*=txtendinv]", row).val();

                        //alert(parseInt($(this).val()) + parseInt(RecInven) + parseInt(TransferIn));
                        //alert((parseInt(TransferOut) + parseInt(expiredmeds) + parseInt(endinv)));
                        //alert(TransferIn + '-' + RecInven + '-' + TransferOut + '-' + expiredmeds + '-' + endinv);
                        var MonthlyUsage = (parseInt($(this).val()) + parseInt(RecInven) + parseInt(TransferIn)) - (parseInt(TransferOut)
                                            + parseInt(expiredmeds) + parseInt(endinv));

                        if (isNaN(MonthlyUsage) == false) {
                            if (parseInt(MonthlyUsage) >= 0) {
                                $("[id*=DivGrdErrorMessage]").css('display', 'none');
                                $("[id*=lblMonthlyUsage]", row).removeClass("ErrorBorder");
                                $("[id*=lblMonthlyUsage]", row).css("border", "");
                                $("[id*=lblMonthlyUsage]", row).val(MonthlyUsage.toString());
                            }
                            else {
                                event.preventDefault();
                                $("[id*=DivGrdErrorMessage]").css('display', 'block');
                                $("[id*=lblMonthlyUsage]", row).addClass("ErrorBorder");
                                $("[id*=lblMonthlyUsage]", row).val(MonthlyUsage.toString());
                            }
                        }
                        else {
                            $("[id*=DivGrdErrorMessage]").css('display', 'none');
                            $("[id*=lblMonthlyUsage]", row).removeClass("ErrorBorder");
                            $("[id*=lblMonthlyUsage]", row).css("border", "");
                            $("[id*=lblMonthlyUsage]", row).val("");
                        }
                    }
                } else {
                    $(this).val('');
                    $("[id*=DivGrdErrorMessage]").css('display', 'none');
                    $("[id*=lblMonthlyUsage]", row).css("border", "");
                    $("[id*=lblMonthlyUsage]", row).val("");
                }
            });

            $(document).on("keyup mouseup", "[id*=txtexpiredmeds]", function () {
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var beginv = $("[id*=txtbeginv]", row).val();
                        var RecInven = $("[id*=lblReceiveingInven]", row).text();
                        var TransferIn = $("[id*=lblTransferIn]", row).text();
                        var TransferOut = $("[id*=lnkTransferOut]", row).text();
                        var endinv = $("[id*=txtendinv]", row).val();

                        var MonthlyUsage = parseInt(beginv) + parseInt(RecInven) + parseInt(TransferIn) - parseInt(TransferOut)
                                            - parseInt($(this).val()) - parseInt(endinv);

                        if (isNaN(MonthlyUsage) == false) {
                            if (parseInt(MonthlyUsage) >= 0) {
                                $("[id*=DivGrdErrorMessage]").css('display', 'none');
                                $("[id*=lblMonthlyUsage]", row).removeClass("ErrorBorder");
                                $("[id*=lblMonthlyUsage]", row).css("border", "");
                                $("[id*=lblMonthlyUsage]", row).val(MonthlyUsage.toString());
                            }
                            else {
                                event.preventDefault();
                                $("[id*=DivGrdErrorMessage]").css('display', 'block');
                                $("[id*=lblMonthlyUsage]", row).addClass("ErrorBorder");
                                $("[id*=lblMonthlyUsage]", row).val(MonthlyUsage.toString());
                            }
                        }
                        else {
                            $("[id*=DivGrdErrorMessage]").css('display', 'none');
                            $("[id*=lblMonthlyUsage]", row).removeClass("ErrorBorder");
                            $("[id*=lblMonthlyUsage]", row).css("border", "");
                            $("[id*=lblMonthlyUsage]", row).val("");
                        }
                    }
                } else {
                    $(this).val('');
                    $("[id*=DivGrdErrorMessage]").css('display', 'none');
                    $("[id*=lblMonthlyUsage]", row).removeClass("ErrorBorder");
                    $("[id*=lblMonthlyUsage]", row).css("border", "");
                    $("[id*=lblMonthlyUsage]", row).val("");
                }
            });

            $(document).on("keyup mouseup", "[id*=txtendinv]", function () {
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var beginv = $("[id*=txtbeginv]", row).val();
                        var RecInven = $("[id*=lblReceiveingInven]", row).text();
                        var TransferIn = $("[id*=lblTransferIn]", row).text();
                        var TransferOut = $("[id*=lnkTransferOut]", row).text();
                        var expiredmeds = $("[id*=txtexpiredmeds]", row).val();

                        var MonthlyUsage = parseInt(beginv) + parseInt(RecInven) + parseInt(TransferIn) - parseInt(TransferOut)
                                            - parseInt(expiredmeds) - parseInt($(this).val());

                        if (isNaN(MonthlyUsage) == false) {
                            if (parseInt(MonthlyUsage) >= 0) {
                                $("[id*=DivGrdErrorMessage]").css('display', 'none');
                                $("[id*=lblMonthlyUsage]", row).removeClass("ErrorBorder");
                                $("[id*=lblMonthlyUsage]", row).css("border", "");
                                $("[id*=lblMonthlyUsage]", row).val(MonthlyUsage.toString());
                            }
                            else {
                                event.preventDefault();
                                $("[id*=DivGrdErrorMessage]").css('display', 'block');
                                $("[id*=lblMonthlyUsage]", row).addClass("ErrorBorder");
                                $("[id*=lblMonthlyUsage]", row).val(MonthlyUsage.toString());
                            }
                        }
                        else {
                            $("[id*=DivGrdErrorMessage]").css('display', 'none');
                            $("[id*=lblMonthlyUsage]", row).removeClass("ErrorBorder");
                            $("[id*=lblMonthlyUsage]", row).css("border", "");
                            $("[id*=lblMonthlyUsage]", row).val("");
                        }
                    }
                } else {
                    $(this).val('');
                    $("[id*=DivGrdErrorMessage]").css('display', 'none');
                    $("[id*=lblMonthlyUsage]", row).removeClass("ErrorBorder");
                    $("[id*=lblMonthlyUsage]", row).css("border", "");
                    $("[id*=lblMonthlyUsage]", row).val("");
                }
            });

        });
        //}

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Ending Inventory
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(Auditpopupshow);
                    Sys.Application.add_load(Remarkspopupshow);
                    Sys.Application.add_load(ShowControls);
                    Sys.Application.add_load(jScript);
                    Sys.Application.add_load(CorpDrop);
                    //Sys.Application.add_load(jscriptsearch);
                </script>

                <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="DivMultiVendor">
                    <%--<asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnuploadfile" />el
                        </Triggers>
                    </asp:UpdatePanel>--%>
                    <asp:Label ID="lblMultiVendor" runat="server" CssClass="page-header page-title" Text="Select Multiple Vendor"></asp:Label><br />
                    <asp:Label ID="lbrow" runat="server">No of records : <%=GrdMultiVendor.Rows.Count.ToString() %></asp:Label>
                    <div class="row" style="padding: 10px;">
                        <div class="col-lg-12 col-md-12 col-sm-12" style="overflow-y: scroll;">
                            <asp:GridView ID="GrdMultiVendor" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="ChkAllVendor" runat="server" AutoPostBack="true" OnCheckedChanged="ChkAllVendor_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkmultiVendor" runat="server" />
                                            <asp:Label ID="lblVendorID" runat="server" Text=' <%# Eval("VendorID")%>' CssClass="HeaderHide"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendorname" runat="server" Text=' <%# Eval("VendorDescription")%>'></asp:Label>
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
                            <asp:Button ID="btnMultiVendorselect" runat="server" Text="Select" CssClass="btn btn-primary" OnClick="btnMultiVendorselect_Click" />
                            <asp:Button ID="btnMultiVendorClose" runat="server" Text="Close" CssClass="btn btn-success" OnClick="btnMultiVendorClose_Click" />
                        </div>
                    </div>
                </div>

                <div id="UploadOpacity" runat="server">
                    <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                        <div class="row">
                            <div class="col-lg-4" align="left">
                                <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                                <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                            </div>
                            <div class="col-lg-8" align="right">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyField" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click"  ValidationGroup="Empty"/>
                                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                                <%--<asp:Button ID="btnAdd" runat="server" Text="Cancel" CssClass="btn btn-primary" />--%>
                            </div>
                        </div>



                        <div id="divMPRMaster" runat="server" style="margin-top: 5px;">
                            <div id="divContent" class="well well-sm">
                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                            <%-- <asp:RequiredFieldValidator InitialValue="-1" ID="Reqdrpcorsearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcorsearch" ErrorMessage="This information is required."></asp:RequiredFieldValidator>--%>
                                            <asp:DropDownList ID="drpcorsearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpcorsearch_SelectedIndexChanged"></asp:DropDownList>
                                            <%--<asp:ListBox ID="drpcorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpcorsearch_SelectedIndexChanged"></asp:ListBox>--%>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                            <%-- <asp:RequiredFieldValidator InitialValue="-1" ID="Reqdrpfacilitysearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacilitysearch" ErrorMessage="This information is required."></asp:RequiredFieldValidator>--%>
                                            <asp:DropDownList ID="drpfacilitysearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpfacilitysearch_SelectedIndexChanged"></asp:DropDownList>
                                            <%--<asp:ListBox ID="drpfacilitysearch" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>--%>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>
                                            <asp:LinkButton ID="lnkMultiVendor" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiVendor_Click"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkClearVendor" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearVendor_Click"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkClearAllVendor" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllVendor_Click"></asp:LinkButton>
                                            <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendorsearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                                ControlToValidate="drpvendorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:ListBox ID="drpvendorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="drpvendorsearch_SelectedIndexChanged"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Item Group</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator InitialValue="" ID="ReqdrpItemCategory" runat="server" ForeColor="Red" ValidationGroup="EmptyField"
                                                ControlToValidate="drpItemCategory" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            <%--<asp:DropDownList ID="drpStatussearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                            <asp:ListBox ID="drpItemCategory" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Month/Year</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="ReqMonth" runat="server" ControlToValidate="txtMonth" ValidationGroup="EmptyField"
                                                ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control datepicker_monthyear" placeholder="Month-Year"></asp:TextBox>
                                            <%--<ajax:CalendarExtender ID="CalMonth" runat="server" TargetControlID="txtMonth" Format="MM/yyyy" DefaultView="Months"></ajax:CalendarExtender>--%>
                                            <ajax:FilteredTextBoxExtender ID="FilterCalMonth" FilterType="Numbers,LowercaseLetters,UppercaseLetters,Custom" ValidChars="/,-" runat="server" TargetControlID="txtMonth"></ajax:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                    <%--<div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Year</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="ReqtxtYear" runat="server" ControlToValidate="txtYear" ValidationGroup="EmptyField"
                                                ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control datepicker_year" placeholder="Year"></asp:TextBox>
                                            <%--<ajax:CalendarExtender ID="CalYear" runat="server" TargetControlID="txtYear" Format="yyyy/MM" DefaultView="Years"></ajax:CalendarExtender>--%>
                                    <%--<ajax:FilteredTextBoxExtender ID="FilterYear" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtYear"></ajax:FilteredTextBoxExtender>
                                        </div>
                                    </div>--%>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div>
                                            <div class="form-group" style="padding-top: 25px;">
                                                <asp:CheckBox ID="chkNewFaciltiy" runat="server" Text="New Facility" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="DIVgrdEndingInven" runat="server">
                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <asp:Button ID="btnaddnewitem" runat="server" Text="Add New Item" CssClass="btn btn-primary" OnClick="btnaddnewitem_Click" />
                                    </div>
                                    <div id="DivGrdErrorMessage" runat="server" class="col-sm-4 col-md-4 col-lg-4" align="Left" style="padding-top: 10px; color: red; display: none;">
                                        <asp:Label ID="lblGrdErrorMessage" runat="server" Text="Month Usage calculation should not be negative"></asp:Label>
                                    </div>

                                </div>


                                <div style="margin-left: 1px; margin-top: 3px;" id="divsearch">
                                    <div class="row">
                                        <div class="col-sm-2 col-md-2 col-lg-2" align="left">
                                            <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4">
                                            <span style="font-weight: 800;">No. of Visit/Procedures/TX</span>&nbsp;<span style="color: red">*</span>
                                            <asp:TextBox ID="txtnoofvisit" runat="server"></asp:TextBox>
                                         <asp:RequiredFieldValidator InitialValue="" ID="reqtxtnoofvisit" ValidationGroup="Empty" runat="server" ForeColor="Red"
                                                ControlToValidate="txtnoofvisit" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <ajax:FilteredTextBoxExtender ID="filternoofvisit" FilterType="Numbers,Custom" runat="server" TargetControlID="txtnoofvisit"></ajax:FilteredTextBoxExtender>
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4" align="right">
                                            <%--<asp:Button ID="btnrefresh" runat="server" Text="Refresh" CssClass="btn btn-primary" OnClick="btnrefresh_Click" />--%>
                                            <%--<asp:Button ID="btnbtmreview" runat="server" Text="Review" CssClass="btn btn-primary" OnClick="btnReview_Click" />--%>
                                            <%--<asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />--%>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblrcount" runat="server">No of records : <%=grdEndingInven.Rows.Count.ToString() %></asp:Label>
                                    <div class="usermaster">
                                        <asp:GridView ID="grdEndingInven" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="grdEndingInven_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                        <asp:Label ID="lblEndingInvenID" runat="server" Text='<%# Eval("EndingInvenID") %>' CssClass="HeaderHide"></asp:Label>
                                                        <asp:Label ID="lblCorporateID" runat="server" Text='<%# Eval("CorporateID") %>' CssClass="HeaderHide"></asp:Label>
                                                        <asp:Label ID="lblFacilityID" runat="server" Text='<%# Eval("FacilityID") %>' CssClass="HeaderHide"></asp:Label>
                                                        <asp:Label ID="lblCatagoryID" runat="server" Text='<%# Eval("CategoryID") %>' CssClass="HeaderHide"></asp:Label>
                                                        <asp:Label ID="lblNewFacility" runat="server" Text='<%# Eval("NewFacility") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemID" HeaderText="Item ID" HeaderStyle-Width="5%" />
                                                <asp:TemplateField HeaderText="Vendor Item ID" HeaderStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorItemID" runat="server" Text='<%# Eval("VendorItemID") %>' ReadOnly="true"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemDescription" HeaderText="Description" ItemStyle-Width="" />
                                                <asp:BoundField DataField="QtyPack" HeaderText="Qty/ Pack" HeaderStyle-Width="5%" />
                                                <asp:BoundField DataField="UomID" HeaderText="UOM" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                <asp:TemplateField HeaderText="Beg Inv(In Each)" HeaderStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtbeginv" runat="server" Text='<%# Eval("BeginingInvQty") %>' Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Recv Qty(In Each)" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblReceiveingInven" runat="server" Text='<%# Eval("ReceiveingOrderInvQty") %>' OnClick="lblReceiveingInven_Click"></asp:LinkButton>
                                                        <asp:Label ID="lblReceiveDate" runat="server" Text='<%# Eval("ReceiveDate") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Trans In(In Each)" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblTransferIn" runat="server" Text='<%# Eval("TransferInQty") %>' OnClick="lblTransferIn_Click"></asp:LinkButton>
                                                        <asp:Label ID="lblTransferINDate" runat="server" Text='<%# Eval("TransferINDate") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Trans Out(In Each)" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkTransferOut" runat="server" Text='<%# Eval("TransferOutQty") %>' OnClick="lnkTransferOut_Click"></asp:LinkButton>
                                                        <asp:Label ID="lblTransferOutDate" runat="server" Text='<%# Eval("TransferOutDate") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expired Meds(In Each)" HeaderStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtexpiredmeds" runat="server" Text='<%# Eval("ExpiredMeds") %>' Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="End Inv(In Each)" HeaderStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtendinv" runat="server" Text='<%# Eval("EndingInvQty") %>' Width="80%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mon.Usage (In Each)" HeaderStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblMonthlyUsage" runat="server" Text='<%# Eval("MonthlyUsage") %>' Width="80%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- <asp:TemplateField HeaderText="Audit Trail">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreatedBy" runat="server" Text=' <%# Eval("CreatedBy")%>'></asp:Label>
                                                    <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="30px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                                    <asp:Image ID="imgreadmore" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="30px" Visible="false" data-content='<%# Eval("Remarks")%>' data-toggle="popover" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
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

                            <asp:HiddenField ID="hdncheckfield" Value="0" runat="server" />
                            <asp:HiddenField ID="HddServiceMasterID" runat="server" />
                            <asp:HiddenField ID="HddServiceDetailsID" runat="server" />
                            <asp:HiddenField ID="HddServiceMasterNo" runat="server" />
                            <asp:HiddenField ID="HddSelectedActionIndex" runat="server" />
                            <asp:HiddenField ID="HddListofSRMasterID" runat="server" />
                            <asp:HiddenField ID="HddReviewPOPCheck" runat="server" />
                            <asp:HiddenField ID="HddGeneratePOPCheck" runat="server" />
                            <asp:HiddenField ID="HddUserRoleID" runat="server" />
                            <asp:HiddenField ID="HddListVendorID" runat="server" />
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
                                <h4 class="modal-title font-bold text-white">Ending Inventory  
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
                                <h4 class="modal-title font-bold text-white">Ending Inventory 
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
                                <h4 class="modal-title font-bold text-white">Ending Inventory 
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

        <asp:UpdatePanel ID="UpdAddnewItem" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnpopAddnewItem" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="MPAddnewItem" runat="server"
                    PopupControlID="ModalAddnewItem" TargetControlID="btnpopAddnewItem"
                    BackgroundCssClass="modalBackground" BehaviorID="ModalAddnewItem">
                </ajax:ModalPopupExtender>
                <div id="ModalAddnewItem" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="Button2" class="close" runat="server" Text="X" OnClick="btncancel_Click" />
                                <h4 class="modal-title" style="color: green; font-size: large">Add New Item</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="well well-sm">
                                    <div class="row">
                                        <div class="col-sm-1 col-md-1 col-lg-1" align="left">
                                            <asp:Button ID="btnAddtogrid" runat="server" CssClass="btn btn-primary" Text="Add to Grid" OnClick="btnAddtogrid_Click" />
                                        </div>
                                    </div>
                                    <asp:Label ID="lblrcount1" runat="server">No of records : <%=GrdAddnewItem.Rows.Count.ToString() %></asp:Label>
                                    <div id="DevGrdAddnewItem" class="SRPOSearchgrid">
                                        <asp:GridView ID="GrdAddnewItem" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                        <asp:Label ID="lblEndingInvenID" runat="server" Text='<%# Eval("EndingInvenID") %>' CssClass="HeaderHide"></asp:Label>
                                                        <asp:Label ID="lblCorporateID" runat="server" Text='<%# Eval("CorporateID") %>' CssClass="HeaderHide"></asp:Label>
                                                        <asp:Label ID="lblFacilityID" runat="server" Text='<%# Eval("FacilityID") %>' CssClass="HeaderHide"></asp:Label>
                                                        <asp:Label ID="lblCatagoryID" runat="server" Text='<%# Eval("CategoryID") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemID" HeaderText="Item ID" />
                                                <asp:TemplateField HeaderText="Vendor Item ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorItemID" runat="server" Text='<%# Eval("VendorItemID") %>' ReadOnly="true"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemDescription" HeaderText="Description" />
                                                <asp:BoundField DataField="QtyPack" HeaderText="Qty/Pack" />
                                                <asp:BoundField DataField="UomID" HeaderText="UOM" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                <asp:BoundField DataField="CategoryName" HeaderText="Item Group" />
                                                <%--<asp:TemplateField HeaderText="Beg Inv">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtbeginv" runat="server" Text='<%# Eval("BeginingInvQty") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Recv Qty(In Each)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceiveingInven" runat="server" Text='<%# Eval("ReceiveingOrderInvQty") %>'></asp:Label>
                                                        <asp:Label ID="lblReceiveDate" runat="server" Text='<%# Eval("ReceiveDate") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Trans In(In Each)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTransferIn" runat="server" Text='<%# Eval("TransferInQty") %>'></asp:Label>
                                                        <asp:Label ID="lblTransferINDate" runat="server" Text='<%# Eval("TransferINDate") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Trans Out(In Each)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTransferOut" runat="server" Text='<%# Eval("TransferOutQty") %>'></asp:Label>
                                                        <asp:Label ID="lblTransferOutDate" runat="server" Text='<%# Eval("TransferOutDate") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Expired Meds">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtexpiredmeds" runat="server" Text='<%# Eval("ExpiredMeds") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <%--<asp:TemplateField HeaderText="End Inv">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtendinv" runat="server" Text='<%# Eval("EndingInvQty") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Mon.Usage(In Each)">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMonthlyUsage" runat="server" Text='<%# Eval("MonthlyUsage") %>'></asp:Label>
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
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:UpdatePanel ID="uppanReceivedQty" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnReceivedQty" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="MPReceivedQty" runat="server"
                    PopupControlID="ModalReceivedQty" TargetControlID="btnReceivedQty"
                    BackgroundCssClass="modalBackground" BehaviorID="ModalReceivedQty">
                </ajax:ModalPopupExtender>
                <div id="ModalReceivedQty" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="btnactionclose" class="close" runat="server" Text="X" OnClick="btncancel_Click" />
                                <h4 class="modal-title" style="color: green; font-size: large">Received Quantity</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="well well-sm">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Item ID:-</span>
                                                <asp:Label ID="lblItemID" runat="server" CssClass="form-control"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Item Description:-</span>
                                                <asp:Label ID="lblItemDescript" runat="server" CssClass="form-control"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="DevGrdReceiveqty">
                                        <asp:GridView ID="GrdReceiveqty" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                        <asp:Label ID="lblEndingInvenID" runat="server" Text='<%# Eval("ItemID") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" />
                                                <asp:BoundField DataField="PRONo" HeaderText="RO Number" />
                                                <asp:BoundField DataField="VendorDescription" HeaderText="Vendor" />
                                                <asp:BoundField DataField="VendorItemID" HeaderText="Vendor Item ID" />
                                                <asp:BoundField DataField="UomName" HeaderText="UOM" />
                                                <asp:BoundField DataField="QtyPack" HeaderText="Oty/Pack" />
                                                <asp:BoundField DataField="ReceivedQty" HeaderText="Received Qty(In Each)" />
                                                <asp:BoundField DataField="Price" DataFormatString="{0:F2}" HeaderText="Cost" />
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
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:UpdatePanel ID="UplTransferIN" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnTransferIN" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="MPTransferIN" runat="server"
                    PopupControlID="ModalTransferIN" TargetControlID="btnTransferIN"
                    BackgroundCssClass="modalBackground" BehaviorID="ModalTransferIN">
                </ajax:ModalPopupExtender>
                <div id="ModalTransferIN" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="Button3" class="close" runat="server" Text="X" OnClick="btncancel_Click" />
                                <h4 class="modal-title" style="color: green; font-size: large">Transfer In</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="well well-sm">
                                    <div id="DevGrdTransferIN">
                                        <asp:GridView ID="GrdTransferIN" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="GrdTransferOutQty_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                        <asp:Label ID="lblTransferInItemID" runat="server" Text='<%# Eval("ItemID") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TransferNo" HeaderText="Transfer No" />
                                                <asp:BoundField DataField="TransferInDate" HeaderText="Transfer Date" />
                                                <asp:BoundField DataField="FacilityDescription" HeaderText="Transferred From" />
                                                <asp:BoundField DataField="ItemID" HeaderText="Item ID" />
                                                <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                                <asp:BoundField DataField="CategoryName" HeaderText="Item Category" />
                                                <asp:BoundField DataField="UomName" HeaderText="UOM" />
                                                <asp:BoundField DataField="QtyPack" HeaderText="Oty/Pack" />
                                                <asp:BoundField DataField="Transferqty" HeaderText="Transfer Qty(In Each)" />
                                                <asp:BoundField DataField="TotalPrice" DataFormatString="{0:F2}" HeaderText="Total Price" />
                                                <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="17%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreatedBy" runat="server" Text=' <%# Eval("Audit")%>'></asp:Label>
                                                        <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="30px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
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
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>



        <asp:UpdatePanel ID="UplTransferOutQty" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnTransferOutQty" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="MPTransferOutQty" runat="server"
                    PopupControlID="ModalTransferOutQty" TargetControlID="btnTransferOutQty"
                    BackgroundCssClass="modalBackground" BehaviorID="ModalTransferOutQty">
                </ajax:ModalPopupExtender>
                <div id="ModalTransferOutQty" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="Button4" class="close" runat="server" Text="X" OnClick="btncancel_Click" />
                                <h4 class="modal-title" style="color: green; font-size: large">Transfer Out Info</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="well well-sm">
                                    <div id="DevGrdTransferOutQty">
                                        <asp:GridView ID="GrdTransferOutQty" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="GrdTransferOutQty_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                        <asp:Label ID="lblTransferoutItemID" runat="server" Text='<%# Eval("ItemID") %>' CssClass="HeaderHide"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TransferNo" HeaderText="Transfer No" />
                                                <asp:BoundField DataField="TransferDate" HeaderText="Transfer Date" />
                                                <asp:BoundField DataField="FacilityDescription" HeaderText="Transferred From" />
                                                <asp:BoundField DataField="ItemID" HeaderText="Item ID" />
                                                <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                                <asp:BoundField DataField="CategoryName" HeaderText="Item Category" />
                                                <asp:BoundField DataField="UomName" HeaderText="UOM" />
                                                <asp:BoundField DataField="QtyPack" HeaderText="Oty/Pack" />
                                                <asp:BoundField DataField="Transferqty" HeaderText="Transfer Qty(In Each)" />
                                                <asp:BoundField DataField="TotalPrice" DataFormatString="{0:F2}" HeaderText="Total Price" />
                                                <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="17%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreatedBy" runat="server" Text=' <%# Eval("Audit")%>'></asp:Label>
                                                        <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="30px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
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
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>



        <asp:UpdatePanel ID="upnlview" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnmpedemo" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeimgview" runat="server"
                    TargetControlID="btnmpedemo" PopupControlID="pnlimgview" CancelControlID="btnpopclose"
                    BackgroundCssClass="modalBackground">
                </ajax:ModalPopupExtender>
                <asp:Panel ID="pnlimgview" Style="display: none;" runat="server">
                    <div style="position: fixed; top: 0; right: 0; bottom: 0; left: 0; z-index: 1040; overflow: auto; overflow-y: scroll; margin-top: 1%;">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header" style="height: 40px">
                                    <asp:Button ID="btnpopclose" runat="server" CssClass="close" Text="X" />
                                    <asp:Label runat="server" Text="Document Viewer" ID="lblimg" />
                                </div>
                                <div class="modal-body" style="padding: 2px;">
                                    <div class="container-fluid" runat="server" id="Div2">
                                        <iframe id="frame1" runat="server" style="height: 530px; width: 100%;" onscroll="auto"></iframe>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>


        <div style="display: none">
            <rsweb:ReportViewer ID="rvEndingInventoryreport" runat="server"></rsweb:ReportViewer>
        </div>

</asp:Content>
