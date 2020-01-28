<%@ Page Title="Receiving-Medical Supplies" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="MedicalSuppliesReceivingOrder.aspx.cs" Inherits="Inventory.MedicalSuppliesReceivingOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : MedicalSuppliesOrder.aspx 
' Type      : ASPX File 
' Description  :   To design the MedicalSuppliesReceivingOrder page for Add,Update and show the MedicalSuppliesReceivingOrder on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                                       Reason 
  02/21/2018           V.01              C.Dhanasekaran                              New
  05/Mar/2018          V.01              Vivekanand.S                                Multi Search
  18/May/2018          V.01              Vivekanand.S                                Multi click Generate Issue  
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

    <script type="text/javascript">

        function CorpDrop() {
            $('[id*=drpcorsearch]').change(function (event) {
                if ($(this).val().length > 1) {
                    var val = $(this).val() || [];
                    alert('Multiple selection are not allowed here. Use Multi select link for multiple selection.');
                    var $this = $(this);
                    //$this[0].sumo.unSelectAll();

                    //$.each(last_valid_selection, function (i, e) {
                    //    $this[0].sumo.selectItem($this.find('option[value="' + e + '"]').index());
                    //});

                } else {
                    last_valid_selection = $(this).val();
                }
            });

            $('[id*=drpfacilitysearch]').change(function (event) {

                if ($(this).val().length > 1) {
                    var val = $(this).val() || [];
                    alert('Multiple selection are not allowed here. Use Multi select link for multiple selection.');
                    var $this = $(this);
                    //$this[0].sumo.unSelectAll();

                    //$.each(last_valid_selection, function (i, e) {
                    //    $this[0].sumo.selectItem($this.find('option[value="' + e + '"]').index());
                    //});

                } else {
                    last_valid_selection = $(this).val();
                }
            });
        }

    </script>

    <script type="text/javascript">

        function jScript() {
            $('[id*=drpcorsearch]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: false,
                placeholder: 'Select Corporate'
            });

            $('[id*=drpfacilitysearch]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: false,
                placeholder: 'Select Facility'
            });

            $('[id*=drpvendorsearch]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Vendor'
            });

            $('[id*=drpStatussearch]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
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
        function Isvaliddate(dateString, msg) {
            //var dateString = $('[id*=txtReceivingDate]').val();
            var myDate = new Date(dateString.value);
            var today = new Date();
            if (myDate > today) {
                $('[id*=lblerrormsg]').html(msg);
                dateString.value = '';
            }
            else {
                $('[id*=lblerrormsg]').html("");
                return true;
            }
        }
        //function checkInvoiceDate() {

        //    var dateString = $('[id*=txtinvoicedate]').val();
        //    var dateString2 = $('[id*=txtreceiveddate]').val();
        //    var myDate1 = new Date(dateString);
        //    var myDate2 = new Date(dateString2);
        //    var today = new Date();
        //    if (myDate1 < myDate2) {
        //        $('[id*=lblerrormsg]').html("Invoice date should not be less than received date");
        //        $('[id*=txtinvoicedate]').val("");
        //    }
        //    else {
        //        $('[id*=lblerrormsg]').html("");
        //        return true;
        //    }
        //}

        function CheckIsValidSave() {
            var result = false;
            var message = '';
            var Usertype = $('[id*=hdnuserrole]').val();
            if (Page_ClientValidate()) {
                $("[id*=grdmsritemedit] [id*=txtreceivedoqty]").each(function () {
                    if ($(this).val() != '') {
                        if ($(this).val() != '0') {
                            result = true;
                        }
                    }
                });
                if (result == false) {
                    message = 'Please enter your receivedQty';
                }
                $("[id*=grdmsritemedit] tr").each(function () {
                    var txtreceivedqty = $(this).find("[id*=txtreceivedoqty]").val();
                    var lblOrderQuantity = $(this).find("[id*=lbloqty]").text();
                    var txtcomments = $(this).find("[id*=txtcomments]").val();

                    if (lblOrderQuantity == '')
                        lblOrderQuantity = 0;

                    if (txtreceivedqty != '') {
                        if (parseInt(txtreceivedqty) < parseInt(lblOrderQuantity)) {
                            if (txtcomments == '') {
                                result = false;
                                message = 'Please enter the comments';
                            }
                        }
                    }
                });
            }
            if (Usertype == '1') {
                result = true;
            }
            if (result == false) {
                $('[id*=lblerrormsg]').html(message);
            }
            else {
                $('[id*=lblerrormsg]').html("");
            }
            return result;
        }


        var shipval = 0;
        var taxval = 0;
        function GetTotal() {
            //console.log($('tr td#txtsipcost').text());
            $('#grdmsritemedit tfoot').each(function () {
                console.log($('th', this).text());
            });
        }

        function GetShipValue(val) {
            shipval = val.value;
            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            var taxVal = $("[id*=grdmsritemedit] .txttaxclass").val();
            var grandTotal = 0;
            $("[id*=lbltotprice]").each(function () {
                if (!isNaN(parseFloat($(this).html()))) {
                    grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                }
            });

            if (taxVal == '')
                taxVal = '0';

            if (val.value == '')
                val.value = '0';

            //console.log(shipval);            
            parseFloat(val.value);
            grandTotal = parseFloat(grandTotal) + parseFloat(taxVal) + parseFloat(val.value);
            $("[id*=txtTotalcost]").val(grandTotal.toFixed(2));
            //SetTotalCost(rowIndex);
        }

        function GetTaxVal(val) {
            taxval = val.value;

            var shipVal = $("[id*=grdmsritemedit] .txtshipcostclass").val();
            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            var grandTotal = 0;
            $("[id*=lbltotprice]").each(function () {
                if (!isNaN(parseFloat($(this).html()))) {
                    grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                }
            });

            if (val.value == '')
                val.value = '0';
            if (shipVal == '')
                shipVal = '0';


            grandTotal = parseFloat(grandTotal) + parseFloat(shipVal) + parseFloat(val.value);
            $("[id*=txtTotalcost]").val(grandTotal.toFixed(2));
            //SetTotalCost(rowIndex);
        }

        function GetadminShipValue(val) {
            shipval = val.value;
            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            var taxVal = $("[id*=grdmsrreviewdeatils] .txttaxclass").val();
            var grandTotal = 0;
            $("[id*=lbltotprice]").each(function () {
                if (!isNaN(parseFloat($(this).html()))) {
                    grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                }
            });

            if (taxVal == '')
                taxVal = '0';

            if (val.value == '')
                val.value = '0';

            //console.log(shipval);            
            parseFloat(val.value);
            grandTotal = parseFloat(grandTotal) + parseFloat(taxVal) + parseFloat(val.value);
            $("[id*=txtTotalcost]").val(grandTotal.toFixed(2));
            //SetTotalCost(rowIndex);
        }

        function GetadminTaxVal(val) {
            taxval = val.value;

            var shipVal = $("[id*=grdmsrreviewdeatils] .txtshipcostclass").val();
            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            var grandTotal = 0;
            $("[id*=lbltotprice]").each(function () {
                if (!isNaN(parseFloat($(this).html()))) {
                    grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                }
            });

            if (val.value == '')
                val.value = '0';
            if (shipVal == '')
                shipVal = '0';


            grandTotal = parseFloat(grandTotal) + parseFloat(shipVal) + parseFloat(val.value);
            $("[id*=txtTotalcost]").val(grandTotal.toFixed(2));
            //SetTotalCost(rowIndex);
        }


        $(function () {
            $(document).on("blur", "[id*=txtreceivedoqty]", function () {
                if (isNaN(parseInt($(this).val()))) {
                    // alert("Blurfunction");
                    //$(this).val('0');
                    var row = $(this).closest("tr");
                    if ($(this).val() == '') {
                        $("[id*=txtreceivedoqty]", row).html('');
                        $("[id*=lblbalanceqty]", row).html('');
                        $("[id*=lbltotprice]", row).html('');
                        // $("#dollorsymbol").hide();
                        var grandTotal = 0;
                        $("[id*=lbltotprice]").each(function () {
                            if (!isNaN(parseFloat($(this).html()))) {
                                grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                            }
                        });
                        $("[id*=txtTotalcost]").val(grandTotal.toFixed(2));
                    }
                } else {
                    $(this).val(parseInt($(this).val()).toString());
                }
            });
        });


        $(function () {
            $(document).on("keyup mouseup", "[id*=txtcomments]", function () {
                var row = $(this).closest("tr");
                if ($(this).val() == "") {
                    if ((parseInt($("[id*=txtreceivedoqty]", row).val()) < parseFloat($("[id*=lbloqty]", row).html()))) {
                        $(this).css({ "border": "Solid 1px red" });
                    } else {
                        $(this).css({ "border": "Solid 1px #a9a9a9" });
                    }

                } else {
                    $(this).css({ "border": "Solid 1px #a9a9a9" });
                }
            });
            // Calcualte received qty and balqty
            $(document).on("keyup change", "[id*=txtreceivedoqty]", function () {
                //    console.log('a');
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var txtcomments = $("[id*=txtcomments]", row);
                        var lblbalqty = parseFloat(parseFloat($("[id*=lbloqty]", row).html() - $(this).val()));
                        var lbltotprice = parseFloat(parseFloat($("[id*=lblPrice]", row).html() * $(this).val()));
                        if (parseInt($(this).val()) > parseFloat($("[id*=lbloqty]", row).html())) {
                            alert("Received quantity is less than orderquantity");
                            $(this).val('');
                        }
                        if (txtcomments != "") {
                            if ((parseInt($(this).val()) < parseFloat($("[id*=lbloqty]", row).html()))) {
                                if (jQuery.trim(txtcomments.val()) == '') {
                                    txtcomments.css({ "border": "Solid 1px red" });
                                }
                                else {
                                    txtcomments.css({ "border": "Solid 1px #a9a9a9" })
                                }
                            }
                            else {
                                txtcomments.css({ "border": "Solid 1px #a9a9a9" })
                            }
                        }
                        else {
                            txtcomments.css({ "border": "Solid 1px #a9a9a9" })
                        }

                        if (isNaN(lblbalqty) == false)
                            $("[id*=lblbalanceqty]", row).html(lblbalqty.toString());
                        else
                            $("[id*=lblbalanceqty]", row).html("");


                        $("[id*=lbltotprice]", row).html(lbltotprice.toString());

                        if ($(this).val() == '') {
                            $("[id*=lblbalanceqty]", row).html("");
                            var lbltotpriceold = parseFloat(parseFloat($("[id*=lbloqty]", row).html()) * parseFloat($("[id*=txtreceivedoqty]", row).html()));
                            $("[id*=lbltotprice]", row).html(lbltotpriceold);
                        }
                    }
                } else {
                    $(this).val('');
                    $("[id*=lblbalanceqty]", row).html("");
                    var lbltotpriceold = parseFloat(parseFloat($("[id*=lbloqty]", row).html()) * parseFloat($("[id*=txtreceivedoqty]", row).html()));
                    $("[id*=lbltotprice]", row).html(lbltotpriceold);
                }

                var grandTotal = 0;
                $("[id*=lbltotprice]").each(function () {
                    if (!isNaN(parseFloat($(this).html()))) {
                        grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                    }
                });
                $("[id*=txtTotalcost]").val(grandTotal.toFixed(2));
            });
        });

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


        function Isshowreason(res) {
            if (res.value == 'Other') {
                $('[id*=otherid]').css("display", "block");
            }
            else {
                $('[id*=otherid]').css("display", "none");
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
        function ShowwarningPopup(res) {
            $('[id*=lblwarning]').html(res);
            $("#modalWarning").modal("show");

        }
        function ShowwarningLookupPopup(res) {
            $('[id*=lblwarning]').html(res);
            $("#modalWarning").modal("show");
        }
        function ShowConfirmationPopup() {
            $("#modalConfirm").modal("show");
        }

        function HideConfirmationPopup() {
            $("#modalConfirm").modal("Hide");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Medical Supplies Receiving Order
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(Auditpopupshow);
                    Sys.Application.add_load(Remarkspopupshow);
                    Sys.Application.add_load(jScript);
                    Sys.Application.add_load(CorpDrop);
                    //Sys.Application.add_load(jscriptsearch);
                </script>
                <asp:HiddenField ID="HdnMSRDetailID" runat="server" />
                <asp:HiddenField ID="hdnuserrole" runat="server" />
                <asp:HiddenField ID="HddListCorpID" runat="server" />
                <asp:HiddenField ID="HddListFacID" runat="server" />
                <asp:HiddenField ID="HddGridCount" runat="server" Value="0" />
                <asp:HiddenField ID="RoHddgridcount" runat="server" Value="0" />
                <%-- Model PopUp For Multi 
                     and Facility --%>
                <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="DivMultiCorp">
                    <%--<asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnuploadfile" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                    <asp:Label ID="lblMultiCorp" runat="server" CssClass="page-header page-title" Text="Select Multiple Corporate"></asp:Label><br />
                     <asp:Label ID="lbrow" runat="server">No of records : <%=GrdMultiCorp.Rows.Count.ToString() %></asp:Label>
                    <div class="row" style="padding: 10px;">
                        <div class="col-lg-12 col-md-12 col-sm-12" style="overflow-y: scroll; height: 200px;">
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
                     <asp:Label ID="lbrowc" runat="server">No of records : <%=GrdMultiFac.Rows.Count.ToString() %></asp:Label>
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
                <div id="DivMedicalReceive" runat="server" class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4" align="left">
                            <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-lg-8 col-md-8 col-sm-8" align="right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyFieldSearch" OnClick="btnSearch_Click" />                          
                            <asp:Button ID="btnPrintAll" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrintAll_Click" />
                            <asp:Button ID="btnSearchcancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnSearchcancel_Click1" />
                        </div>
                    </div>
                    <div id="divMedReqSearch" runat="server" style="margin-top: 5px;">
                        <div id="divSearchContent" runat="server" class="well well-sm">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiCorp" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearCorp" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllCorp" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllCorp_Click"></asp:LinkButton>
                                        <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpcorsearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ListBox ID="drpcorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpcorsearch_SelectedIndexChanged"></asp:ListBox>   
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Facility</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiFac" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearFac" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllFac" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllFac_Click"></asp:LinkButton>
                                        <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpfacilitysearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacilitysearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ListBox ID="drpfacilitysearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpfacilitysearch_SelectedIndexChanged"></asp:ListBox>       
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Vendor</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendorsearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ListBox ID="drpvendorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>   
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Date From</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="rfvDateFrom" runat="server" ControlToValidate="txtDateFrom" ValidationGroup="EmptyFieldSearch"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revdatefrom" ControlToValidate="txtDateFrom"
                                            ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                            SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="EmptyFieldSearch" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY" MaxLength="10"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateFrom" runat="server" TargetControlID="txtDateFrom" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Date To</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="EmptyFieldSearch"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revdateto" ControlToValidate="txtDateTo"
                                            ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                            SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="EmptyFieldSearch" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY" MaxLength="10"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Status</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="" ID="ReqdrpStatus" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpStatussearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ListBox ID="drpStatussearch" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                        <%--<asp:DropDownList ID="drpStatussearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3" align="left">
                            <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                        </div>
                        <div class="col-lg-9" align="right">
                            <asp:Button ID="btnrefresh" runat="server" CssClass="btn btn-primary" Text="Refresh" OnClick="btnrefresh_Click" />
                        </div>
                    </div>
                    <asp:Label ID="lblrcount3" runat="server">No of records : <%=grdReciveSearch.Rows.Count.ToString() %></asp:Label>
                    <div id="divgrdMSRSearch" runat="server" style="margin-left: 1px; margin-top: 3px;" class="divMedReqSearchGrid MSRSearchgrid">
                        <asp:GridView ID="grdReciveSearch" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" OnRowDataBound="grdReciveSearch_RowDataBound" OnPageIndexChanging ="grdReciveSearch_PageIndexChanging" AllowPaging ="true" PageSize ="10">
                            <Columns>
                                <asp:TemplateField HeaderText="Summary" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgsummaryprint" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" ToolTip="Printsummary" OnClick="imgsummaryprint_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detail" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgdetailprint" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" ToolTip="PrintDetail" OnClick="imgdetailprint_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MedicalSuppliesReceivingMasterID" HeaderText="MedicalSuppliesReceivingMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <asp:BoundField DataField="PRMasterID" HeaderText="PRMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <asp:BoundField DataField="PONo" HeaderText="PONo" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <asp:BoundField DataField="MedicalSuppliesRequestOrderID" HeaderText="MedicalSuppliesRequestOrderID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <asp:BoundField DataField="CreatedOn" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="9%" ItemStyle-Width="9%" />
                                <asp:BoundField DataField="CorporateName" HeaderText="Corp" HeaderStyle-Width="7%" ItemStyle-Width="7%" />
                                <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" HeaderStyle-Width="7%" ItemStyle-Width="7%" />
                                <asp:BoundField DataField="VendorShortName" HeaderText="Vendor" HeaderStyle-Width="7%" ItemStyle-Width="7%" />
                                <asp:TemplateField HeaderText="PO No">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbtnpono" runat="server" Text=' <%# Eval("PONo")%>' OnClick="lkbtnpono_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RO No">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbtnrono" runat="server" Text=' <%# Eval("PRONo")%>' OnClick="lkbtnrono_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TotalPrice" HeaderText="Price($)" DataFormatString="$ {0:#,0.00}" HeaderStyle-Width="9%" />
                                <asp:BoundField DataField="FinalStatus" HeaderText="Status" />
                                <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblaudit" runat="server" Text='<%# Eval("Audit")%>'></asp:Label>--%>
                                        <asp:Image ID="imgreadmoreaudit" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-Wrap="true" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>--%>
                                        <asp:Image ID="imgreadmore" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Remarks")%>' data-toggle="popover" />
                                    </ItemTemplate>
                                </asp:TemplateField>
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
            <rsweb:ReportViewer ID="rvmedicalsupplyreportVoidPDF" runat="server"></rsweb:ReportViewer>
            <rsweb:ReportViewer ID="rvsummaryreport" runat="server"></rsweb:ReportViewer>
            <rsweb:ReportViewer ID="rvdetailreport" runat="server"></rsweb:ReportViewer>
            <rsweb:ReportViewer ID="rvMedicalSupplyReceivingOrderSummary" runat="server"></rsweb:ReportViewer>
        </div>
        <%-- Popup Notification--%>
        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">Medical Supplies Receiving
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


        <div id="modalWarning" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header btn-warning">
                        <h4 class="modal-title font-bold text-white">Medical Supplies Receiving
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
        <div id="modalDelete" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-red">
                        <h4 class="modal-title font-bold text-white">Medical Supplies Receiving
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
        <div id="modalConfirm" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-red">
                        <h4 class="modal-title font-bold text-white">Delete Confirmation</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <p>Do you want to delete this record <span id="spnreName"></span>?.</p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="removeyes" runat="server" Text="Yes" CssClass="btn btn-danger" />
                        <asp:Button ID="removeno" runat="server" Text="Close" class="btn btn-default ra-100" />
                    </div>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnmedreview" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpemsrreviewrecive" runat="server"
                    PopupControlID="mpemedsuprecreview" TargetControlID="btnmedreview"
                    BackgroundCssClass="modalBackground" BehaviorID="mpemedsuprecreview">
                </ajax:ModalPopupExtender>

                <div id="mpemedsuprecreview" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="btnrecclose" class="close" runat="server" Text="X" OnClick="btncancel_Click" />
                                <h4 class="modal-title" style="color: green; font-size: large">Medical Supplies Receiving Order</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="form-horizontal">
                                    <div class="row" style="margin-bottom: 2px;">
                                        <div class="col-lg-5" align="left">
                                            <span style="font-weight: 800;">Receiving order Number :- </span>
                                            <asp:Label ID="lblmprreview" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                        <div id="DivErrorMsg" class="col-lg-4" runat="server" align="left">
                                            <asp:Label ID="lblerrormsg" runat="server" ForeColor="Red" Style="font-weight: 800;" Text=""></asp:Label>
                                        </div>
                                        <div class="col-lg-3" align="right">
                                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="btn btn-success" ValidationGroup="EmptyFieldSave" OnClick="btnsave_Click" OnClientClick="return CheckIsValidSave()" />
                                            <asp:Button ID="btncancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btncancel_Click" />
                                        </div>
                                    </div>
                                    <div class="well well-sm" style="padding: 5px 15px 5px 15px;">
                                        <div id="nonadmin" runat="server" class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <span style="font-weight: 800;">Packing Silp No:</span>&nbsp;<span style="color: red">*</span>
                                                <asp:RequiredFieldValidator ID="Reqpacksilpno" runat="server" ControlToValidate="packingsilpno" ValidationGroup="EmptyFieldSave" Display="Dynamic"
                                                    ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="packingsilpno" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <span style="font-weight: 800;">Packing Silp Date:</span>&nbsp;<span style="color: red">*</span>
                                                <asp:RequiredFieldValidator ID="Reqpacking" runat="server" ControlToValidate="txtpackingslipdate" ValidationGroup="EmptyFieldSave" Display="Dynamic"
                                                    ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="regexpkslipdate" ControlToValidate="txtpackingslipdate"
                                                    ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                                    SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="EmptyFieldSave" Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                                <asp:TextBox ID="txtpackingslipdate" runat="server" CssClass="form-control" placeholder="--/--/----" onblur="Isvaliddate(this,'PackingSlip date can not be select future date')"></asp:TextBox>
                                                <ajax:CalendarExtender ID="calpacslip" runat="server" TargetControlID="txtpackingslipdate" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                <ajax:FilteredTextBoxExtender ID="filterpacking" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtpackingslipdate"></ajax:FilteredTextBoxExtender>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <span style="font-weight: 800;">Received Date:</span>&nbsp;<span style="color: red">*</span>
                                                <asp:RequiredFieldValidator ID="Reqrecedate" runat="server" ControlToValidate="txtreceiveddate" ValidationGroup="EmptyFieldSave" Display="Dynamic"
                                                    ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="regexrecdate" ControlToValidate="txtreceiveddate"
                                                    ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                                    SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="EmptyFieldSave" Display="Dynamic">
                                                </asp:RegularExpressionValidator>
                                                <asp:TextBox ID="txtreceiveddate" runat="server" CssClass="form-control" placeholder="--/--/----" onblur="Isvaliddate(this,'Received date can not be select future date')"></asp:TextBox>
                                                <ajax:CalendarExtender ID="calrecedate" runat="server" TargetControlID="txtreceiveddate" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                <ajax:FilteredTextBoxExtender ID="filterreceived" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtreceiveddate"></ajax:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div id="Divsupad" runat="server">
                                            <div class="row">
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <span style="font-weight: 800;">Invoice No: </span>&nbsp;<span style="color: red">*</span>
                                                    <asp:RequiredFieldValidator ID="Reqinvoiceno" runat="server" ControlToValidate="txtinvoiceno" ValidationGroup="EmptyFieldSave" Display="Dynamic"
                                                        ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtinvoiceno" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <span style="font-weight: 800;">Invoice Date:</span>&nbsp;<span style="color: red">*</span>
                                                    <asp:RequiredFieldValidator ID="Reqfldin" runat="server" ControlToValidate="txtinvoicedate" ValidationGroup="EmptyFieldSave" Display="Dynamic"
                                                        ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtinvoicedate"
                                                        ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                                        SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="EmptyFieldSave" Display="Dynamic">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:TextBox ID="txtinvoicedate" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="calinvoice" runat="server" TargetControlID="txtinvoicedate" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                    <ajax:FilteredTextBoxExtender ID="filterinvoice" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtinvoicedate"></ajax:FilteredTextBoxExtender>
                                                </div>
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <span style="font-weight: 800;">Receiving Action:</span>&nbsp;<span style="color: red">*</span>
                                                    <asp:RequiredFieldValidator InitialValue="0" ID="Reqdrprecaction" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                        ControlToValidate="ddlreceivingAct" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:DropDownList ID="ddlreceivingAct" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <span style="font-weight: 800;">Reason:</span>&nbsp;<span style="color: red">*</span>
                                                    <asp:RequiredFieldValidator InitialValue="0" ID="Reqreason" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                        ControlToValidate="ddlreason" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:DropDownList ID="ddlreason" runat="server" CssClass="form-control" onchange="Isshowreason(this)"></asp:DropDownList>
                                                </div>
                                                <div id="otherid" runat="server" style="display: none">
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                        <span id="other" style="font-weight: 800;">Other Reason:</span>
                                                        <asp:TextBox ID="txtreason" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblitems" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label>                                   
                                    <div class="row" style="margin-left: 1px">
                                        <asp:Label ID="lblrcount5" Visible="false" runat="server"></asp:Label>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <asp:Label ID="lblrcount4" runat="server"></asp:Label>
                                            <div class="SRReviewgrid">
                                                <asp:GridView ID="grdmsritemedit" runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false" CssClass="table table-responsive" ShowFooter="true" OnRowDataBound="grdmsritemedit_RowDataBound">
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="MedicalSuppliesReceivingDetailsID" HeaderText="MedicalSuppliesReceivingDetailsID" Visible="false" />--%>

                                                        <asp:TemplateField HeaderText="ItemID" HeaderStyle-Width="5%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemId" Text='<%#Eval("SNGItemID") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Group" HeaderStyle-Width="12%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCategoryName" Text='<%#Eval("CategoryName") %>' runat="server"></asp:Label>
                                                                <asp:Label ID="lblMedicalSuppliesReceivingDetailsID" runat="server" Text='<%#Eval("MedicalSuppliesReceivingDetailsID") %>' CssClass="HeaderHide"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Vendor Item ID" HeaderStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVendorItemCode" Text='<%#Eval("VendorItemID") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemDescription" Text='<%#Eval("ItemDescription") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUOM" Text='<%#Eval("UOM") %>' runat="server"></asp:Label>
                                                                <%-- <asp:Label ID="lbluomvalue" Text='<%#Eval("UomID") %>' runat="server" Style="display: none;"></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty/ Pack" HeaderStyle-Width="4%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQtyPack" Text='<%#Eval("QtyPack") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Price($)" HeaderStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <span id="dolloredititemprice">$</span>
                                                                <asp:Label ID="lblPrice" Text='<%#Eval("Price","{0:F2}") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Order Qty" HeaderStyle-Width="4%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbloqty" runat="server" Text='<%#Eval("OrderQty") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Balance Qty" HeaderStyle-Width="4%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblbalanceqty" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Received Qty" HeaderStyle-Width="9%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtreceivedoqty" runat="server" Text="" Width="80%"></asp:TextBox>
                                                                <ajax:FilteredTextBoxExtender ID="Filtertxtreceiveoqty" FilterType="Numbers" runat="server" TargetControlID="txtreceivedoqty"></ajax:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <span style="font-weight: 600; color: black">Shipping Cost</span>
                                                                <br />
                                                                <br />
                                                                <span style="font-weight: 600; color: black">Tax</span>
                                                                <br />
                                                                <br />
                                                                <span style="font-weight: 600; color: black">Total Cost</span>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total($)" HeaderStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <span id="dolloredititemtotal">$</span>
                                                                <asp:Label ID="lbltotprice" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <span style="color: black">$</span>
                                                                <asp:TextBox ID="txtshipcost" Text="0" onblur="GetShipValue(this)" Width="80%" onchange="GetShipValue(this)" runat="server" ForeColor="Black" CssClass="txtshipcostclass" Enabled="false"></asp:TextBox><br />
                                                                <ajax:FilteredTextBoxExtender ID="FTBtxtshipcost" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtshipcost"></ajax:FilteredTextBoxExtender>
                                                                <br />
                                                                <span style="color: black">$</span>
                                                                <asp:TextBox ID="txttax" Text="0" onblur="GetTaxVal(this)" onchange="GetTaxVal(this)" Width="80%" runat="server" ForeColor="Black" CssClass="txttaxclass" Enabled="false"></asp:TextBox>
                                                                <ajax:FilteredTextBoxExtender ID="Filteredtxttax" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txttax"></ajax:FilteredTextBoxExtender>
                                                                <br />
                                                                <br />
                                                                <span style="color: black">$</span>
                                                                <%--<asp:Label ID="txtTotalcost" runat="server" Text="" style="font-weight: 800; color:Black"></asp:Label>--%>
                                                                <asp:TextBox ID="txtTotalcost" runat="server" Style="font-weight: 800; color: Black" Width="90%" Enabled="false"></asp:TextBox>
                                                                <%-- <asp:TextBox ID="txtTotalcost" runat="server" CssClass="ToatalcostCLS" ForeColor="Black" disabled="true" BackColor="White"></asp:TextBox>
                                                                <asp:TextBox ID="txtTotalcost" runat="server" CssClass="ToatalcostLabelCLS HeaderHide"></asp:TextBox>--%>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Comments" HeaderStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtcomments" runat="server" Text=""></asp:TextBox>
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

                                                <asp:HiddenField ID="hdnreviewflag" runat="server" Value="0" />
                                                <asp:GridView ID="grdmsrreviewdeatils" runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false" CssClass="table table-responsive" OnRowDataBound="grdmsrreviewdeatils_RowDataBound" ShowFooter="true" Style="display: none">
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="MedicalSuppliesReceivingDetailsID" HeaderText="MedicalSuppliesReceivingDetailsID" Visible="false" />--%>

                                                        <asp:TemplateField HeaderText="ItemID" HeaderStyle-Width="5%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemId" Text='<%#Eval("SNGItemID") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Group" HeaderStyle-Width="12%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCategoryName" Text='<%#Eval("CategoryName") %>' runat="server"></asp:Label>
                                                                <asp:Label ID="lblMedicalSuppliesReceivingDetailsID" runat="server" Text='<%#Eval("MedicalSuppliesReceivingDetailsID") %>' CssClass="HeaderHide"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Vendor Item ID" HeaderStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVenItemCode" Text='<%#Eval("VendorItemID") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemDescription" Text='<%#Eval("ItemDescription") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUOM" Text='<%#Eval("UOM") %>' runat="server"></asp:Label>
                                                                <%-- <asp:Label ID="lbluomvalue" Text='<%#Eval("UomID") %>' runat="server" Style="display: none;"></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty/ Pack" HeaderStyle-Width="4%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQtyPack" Text='<%#Eval("QtyPack") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Price($)" HeaderStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <span id="dolloredititemprice">$</span>
                                                                <asp:Label ID="lblPrice" Text='<%#Eval("Price","{0:F2}") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Order Qty" HeaderStyle-Width="4%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbloqty" runat="server" Text='<%#Eval("OrderQty") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Balance Qty" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblbalanceqty" Text='<%#Eval("BalanceQty") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Received Qty" HeaderStyle-Width="9%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtreceivedoqty" Text='<%#Eval("ReceivedQty") %>' Enabled="false" runat="server" Width="80%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <span style="font-weight: 600; color: black">Shipping Cost</span>
                                                                <br />
                                                                <br />
                                                                <span style="font-weight: 600; color: black">Tax</span>
                                                                <br />
                                                                <br />
                                                                <span style="font-weight: 600; color: black">Total Cost</span>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total($)" HeaderStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <span id="dolloredititemtotal">$</span>
                                                                <asp:Label ID="lbltotprice" runat="server" Text='<%#Eval("TotalPrice","{0:F2}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <span style="color: black">$</span>
                                                                <asp:TextBox ID="txtshipcost" Text='<%#Eval("Shipping") %>' Width="80%" onblur="GetadminShipValue(this)" onchange="GetadminShipValue(this)" runat="server" ForeColor="Black" CssClass="txtshipcostclass"></asp:TextBox><br />
                                                                <ajax:FilteredTextBoxExtender ID="FTBtxtshipcost" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtshipcost"></ajax:FilteredTextBoxExtender>
                                                                <br />
                                                                <span style="color: black">$</span>
                                                                <asp:TextBox ID="txttax" Text='<%#Eval("Tax") %>' Width="80%" onblur="GetadminTaxVal(this)" onchange="GetadminTaxVal(this)" runat="server" ForeColor="Black" CssClass="txttaxclass"></asp:TextBox>
                                                                <ajax:FilteredTextBoxExtender ID="Filteredtxttax" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txttax"></ajax:FilteredTextBoxExtender>
                                                                <br />
                                                                <br />
                                                                <span style="color: black">$</span>
                                                                <asp:TextBox ID="txtTotalcost" runat="server" Width="90%" Text='<%#Eval("TotalCost") %>' Style="font-weight: 800; color: Black"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Comments" HeaderStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtcomments" runat="server" Text='<%#Eval("Comments") %>' Enabled="false"></asp:TextBox>
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
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
