<%@ Page Title="Receiving-Machine Parts Receive Order" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="MachinePartsReceiveOrder.aspx.cs" Inherits="Inventory.MachinePartsReceiveOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : MachinePartsReceivingOrder.aspx 
' Type      : ASPX File 
' Description  :   To design the Machine Parts Receiving Order page for add,Update and show the Machine Parts Receiving Order on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  02/13/2018            V.01              Sairam.P                      New
  05/Mar/2018          V.01              Sairam.P                   Multi Search 
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
            $('[id*=drpcor]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: false,
                placeholder: 'Select Corporate'
            });

            $('[id*=drpfacility]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: false,
                placeholder: 'Select Facility'
            });

            $('[id*=drpvendor]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Vendor'
            });

            $('[id*=drpStatus]').SumoSelect({
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


        var shipval = 0;
        var taxval = 0;
        function GetTotal() {
            //console.log($('tr td#txtsipcost').text());
            $('#grdMPRReview tfoot').each(function () {
                console.log($('th', this).text());
            });
        }
        function GetShipValue(val) {
            shipval = val.value;
            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            var taxVal = $("[id*=grdMPRReview] .txttax").val();
            var grandTotal = 0;
            $("[id*=lblTotalPrice]").each(function () {
                if (!isNaN(parseFloat($(this).html()))) {
                    grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                }
            });
            if (taxVal == '')
                taxVal = '0';
            if (val.value == '')
                val.value = '0';

            console.log(shipval);
            grandTotal = parseFloat(grandTotal) + parseFloat(taxVal) + parseFloat(val.value);
            $("[id*=txtTotalcost]").val(grandTotal.toFixed(2));
            //SetTotalCost(rowIndex);
        }
        function GetTaxVal(val) {
            taxval = val.value;

            var shipVal = $("[id*=grdMPRReview] .txtsipcost").val();
            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            var grandTotal = 0;
            $("[id*=lblTotalPrice]").each(function () {
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
        function SetTotalCost(rowIndex) {

            var table = $("[id*=grdMPRReview]");
            //var totalCost = table.find("tr").eq(rowIndex).find("td").eq(9).find("input.ToatalcostLabelCLS").val();
            var totalCost = table.find("tr").eq(rowIndex).find("td").eq(9).find("input.ToatalcostCLS").val();
            console.log(totalCost);
            console.log(taxval);
            if (taxval == null || taxval == '') {
                taxval = 0;
                console.log(taxval);
            }
            if (shipval == null || shipval == '') {
                shipval = 0;
                console.log(shipval);
            }
            console.log(shipval);
            var res = (parseFloat(taxval) + parseFloat(shipval) + parseFloat(totalCost)).toFixed(2);
            console.log('res' + res);
            if (isNaN(res) || res == "Infinity") {
                table.find("tr").eq(rowIndex).find("td").eq(9).find("input.ToatalcostCLS").val();
            } else {
                table.find("tr").eq(rowIndex).find("td").eq(9).find("input.ToatalcostCLS").val(res);

            }
        }

        $(function () {
            $(document).on("keyup mouseup", "[id*=txtComments]", function () {
                var row = $(this).closest("tr");
                if ($(this).val() == "") {
                    if ((parseInt($("[id*=txtreceivedqty]", row).val()) < parseFloat($("[id*=lblOrderQuantity]", row).html()))) {
                        $(this).css({ "border": "Solid 1px red" });
                    } else {
                        $(this).css({ "border": "Solid 1px #a9a9a9" });
                    }

                } else {
                    $(this).css({ "border": "Solid 1px #a9a9a9" });
                }
            });
            $(document).on("keyup mouseup", "[id*=txtreceivedqty]", function () {
                console.log('a');
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var txtcomments = $("[id*=txtComments]", row);
                        var lblbalqty = parseFloat(parseFloat($("[id*=lblOrderQuantity]", row).html() - $(this).val()));
                        var lbltotprice = parseFloat(parseFloat($("[id*=lblrevppqty]", row).html() * $(this).val()));
                        if (parseInt($(this).val()) > parseFloat($("[id*=lblOrderQuantity]", row).html())) {
                            alert("Received quantity should  be less than orderquantity");
                        }
                        if (txtcomments != "") {
                            if ((parseInt($(this).val()) < parseFloat($("[id*=lblOrderQuantity]", row).html()))) {
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
                            $("[id*=lblbalqty]", row).html(lblbalqty.toString());
                        else
                            $("[id*=lblbalqty]", row).html("");


                        $("[id*=lblTotalPrice]", row).html(lbltotprice.toString());

                        if ($(this).val() == '') {
                            //$("[id*=lblbalqty]", row).html("");
                            //var lbltotpriceold = parseFloat(parseFloat($("[id*=lblOrderQuantity]", row).html()) * parseFloat($("[id*=lblrevppqty]", row).html()));
                            //$("[id*=lblTotalPrice]", row).html(lbltotpriceold);
                            $("[id*=lblTotalPrice]", row).html('');
                        }
                    }
                } else {
                    $(this).val('');
                    //$("[id*=lblbalqty]", row).html("");
                    //var lbltotpriceold = parseFloat(parseFloat($("[id*=lblOrderQuantity]", row).html()) * parseFloat($("[id*=lblrevppqty]", row).html()));
                    //$("[id*=lblTotalPrice]", row).html(lbltotpriceold);
                    $("[id*=lblTotalPrice]", row).html('');
                    var txtcomments = $("[id*=txtComments]", row);
                    txtcomments.css({ "border": "Solid 1px #a9a9a9" })
                }

                var grandTotal = 0;
                $("[id*=lblTotalPrice]").each(function () {
                    if (!isNaN(parseFloat($(this).html()))) {
                        grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                    }
                });
                $("[id*=txtTotalcost]").val(grandTotal.toFixed(2));
            });
        });


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


            function CheckIsValidSave() {
                var result = false;
                var message = '';
                var Usertype = $('[id*=Hdnrole]').val();
                if (Page_ClientValidate()) {
                    $("[id*=grdMPRReview] [id*=txtreceivedqty]").each(function () {
                        if ($(this).val() != '') {
                            if ($(this).val() != '0') {
                                result = true;
                            }
                        }
                    });
                    if (result == false) {
                        message = 'Please enter your ReceivedQty';
                    }
                    $("[id*=grdMPRReview] tr").each(function () {
                        var txtreceivedqty = $(this).find("[id*=txtreceivedqty]").val();
                        var lblOrderQuantity = $(this).find("[id*=lblOrderQuantity]").text();
                        var txtcomments = $(this).find("[id*=txtComments]").val();

                        //if (txtreceivedqty == '')
                        //    txtreceivedqty = 0;
                        if (lblOrderQuantity == '')
                            lblOrderQuantity = 0;

                        if (txtreceivedqty != '') {
                            if (parseInt(txtreceivedqty) < parseInt(lblOrderQuantity)) {
                                if (txtcomments == '') {
                                    result = false;
                                    message = 'Please enter the Comments';
                                    //console.log('ssss');
                                }

                            }
                        }

                    });
                }
                if (Usertype == '1') {
                    result = true;
                }
                if (result == false) {
                    $('[id*=lblmsg]').html(message);
                }
                else {
                    $('[id*=lblmsg]').html("");
                }
                return result;
            }



            function Isshowreason(res) {
                if (res.value == 'Other') {
                    $('[id*=otherid]').show();
                }
                else {
                    $('[id*=otherid]').hide();
                }
            }


            function checkPackingSlipDate() {

                var dateString = $('[id*=txtPackingSlipDate]').val();
                var myDate = new Date(dateString);
                var today = new Date();
                if (myDate > today) {
                    $('[id*=lblmsg]').html("Packing Slip Date should not be less than Today");
                    $('[id*=txtPackingSlipDate]').val("");
                }
                else {
                    $('[id*=lblmsg]').html("");
                    return true;
                }

            }
            function checkReceivingDate() {

                var dateString = $('[id*=txtReceivingDate]').val();
                var myDate = new Date(dateString);
                var today = new Date();
                if (myDate > today) {
                    $('[id*=lblmsg]').html("Received Date should not be less than Today");
                    $('[id*=txtReceivingDate]').val("");
                }
                else {
                    $('[id*=lblmsg]').html("");
                    return true;
                }

            }
            function checkInvoiceDate() {

                var dateString = $('[id*=txtInvoiceDate]').val();
                var dateString2 = $('[id*=txtReceivingDate]').val();
                var myDate1 = new Date(dateString);
                var myDate2 = new Date(dateString2);
                var today = new Date();
                if (myDate1 < myDate2) {
                    $('[id*=lblmsg]').html("Invoice Date should not be less than Received Date");
                    $('[id*=txtInvoiceDate]').val("");
                }
                else {
                    $('[id*=lblmsg]').html("");
                    return true;
                }

            }


            function checkdate(txt) {
                var selectedDate = new Date(txt.value);
                var today = new Date();
                if (new Date(selectedDate) < new Date(today) && new Date(selectedDate) != new Date(today)) {
                    txt.value = '';
                    alert('You cannot select a Previous date!');

                }
            }

            function ShowPopup(res) {

                $('[id*=lblsave]').html(res);

                $("#modalSave").modal("show");

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

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Machine Parts Receive Order
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
                </script>
                <asp:HiddenField ID="HddListCorpID" runat="server" />
                <asp:HiddenField ID="HddListFacID" runat="server" />
                   <%-- Model PopUp For Multi Corporate and Facility --%>
                <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="DivMultiCorp">
                    <%--<asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnuploadfile" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                    <asp:Label ID="lblMultiCorp" runat="server" CssClass="page-header page-title" Text="Select Multiple Corporate"></asp:Label><br />
                     <asp:Label ID="lbrow" runat="server">No of records : <%=GrdMultiCorp.Rows.Count.ToString() %></asp:Label>
                    <div class="row" style="padding: 10px;">
                        <div class="col-lg-12 col-md-12 col-sm-12" style="overflow-y: scroll;height: 200px;">
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
                      <asp:Label ID="lbcount" runat="server">No of records : <%=GrdMultiFac.Rows.Count.ToString() %></asp:Label>
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
                <div class="mypanel-body" id="divMPReceiveOrder" runat="server" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-4" align="left">
                            <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-lg-8" align="right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" ValidationGroup="EmptyField" OnClick="btnSearch_Click" Text="Search" />
                            <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-primary" Text="Refresh" OnClick="btnRefresh_Click" />
                            <asp:Button ID="btnPrintAll" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrintAll_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                        </div>
                    </div>
                    <div id="divMPRMaster" runat="server" style="margin-top: 5px;">
                        <div id="divContent" class="well well-sm">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiCorp" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearCorp" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllCorp" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllCorp_Click"></asp:LinkButton> 
                                        <asp:ListBox ID="drpcor" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:ListBox>    
                                         <asp:RequiredFieldValidator InitialValue="" ID="ReqdrdCorporate" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcor" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                         <asp:LinkButton ID="lnkMultiFac" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearFac" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllFac" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllFac_Click"></asp:LinkButton>  
                                        <asp:ListBox ID="drpfacility" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="drpfacility_SelectedIndexChanged"></asp:ListBox>  
                                        <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpfacility" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacility" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>
                                        <asp:ListBox ID="drpvendor" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox> 
                                         <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendor" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendor" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date From</span>&nbsp;<span style="color: red">*</span>
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateFrom" runat="server" TargetControlID="txtDateFrom" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilterDateFrom" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateFrom"></ajax:FilteredTextBoxExtender>
                                         <asp:RequiredFieldValidator ID="rfvDateFrom" runat="server" ControlToValidate="txtDateFrom" ValidationGroup="EmptyField"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date To</span>&nbsp;<span style="color: red">*</span>
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilterDateTo" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateTo"></ajax:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="EmptyField"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Status</span>&nbsp;<span style="color: red">*</span>   
                                        <asp:ListBox ID="drpStatus" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                          <asp:RequiredFieldValidator InitialValue="" ID="rfvStatus" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpStatus" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
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
                        </div>
                        <asp:HiddenField ID="HdnMPRDetailsID" runat="server" />
                        <asp:HiddenField ID="HddMasterID" runat="server" />
                        <asp:HiddenField ID="HddGridCount" runat="server" Value="0" />
                        <div style="margin-left: 1px; margin-top: 3px;" id="divsearch">
                           <asp:Label ID="lblrcount" runat="server">No of records : <%=grdMPRMaster.Rows.Count.ToString() %></asp:Label>
                            <div class="MPRSearchgrid">
                                <asp:GridView ID="grdMPRMaster" runat="server" OnRowDataBound="grdMPRMaster_RowDataBound" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Summary" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgsummary" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" ToolTip="Summary" OnClick="imgsummary_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Details" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgdetails" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" ToolTip="Details" OnClick="imgdetails_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MPRMasterID" HeaderText="MPRMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="MachinePartsReceivingMasterID" HeaderText="MachinePartsReceivingMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CreatedOn" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="9%" />
                                        <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CorporateName" HeaderText="Corp" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="VendorShortName" HeaderText="Vendor" HeaderStyle-Width="8%" />
                                        <asp:TemplateField HeaderText="MPONo">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbMPONo" runat="server" Text=' <%# Eval("MPONo")%>' OnClick="lbMPONo_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MPRONo">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbMPRONo" runat="server" Text=' <%# Eval("MPRONo")%>' OnClick="lbMPRONo_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TotalCost" HeaderText="Price($)" DataFormatString="$ {0:#,0.00}" HeaderStyle-Width="10%" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblAudit" runat="server" Text=' <%# Eval("Audit")%>'></asp:Label>--%>
                                                <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblRemarks" runat="server" Text=' <%# Eval("Remarks")%>'></asp:Label>--%>
                                                <asp:Image ID="imgreadmore" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="right" Height="40px" data-content='<%# Eval("Remarks")%>' data-toggle="popover" />
                                                <%-- <asp:TextBox ID="txtremarks" runat="server" Visible="false"></asp:TextBox>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MachinePartsRequestOrderID" HeaderText="MachinePartsRequestOrderID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
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
                    <asp:HiddenField ID="HDDMPONo" Value="0" runat="server" />
                    <asp:HiddenField ID="HddMPRNONo" Value="0" runat="server" />
                    <asp:HiddenField ID="HddOrderID" Value="0" runat="server" /> 
                     <asp:HiddenField ID="Hdnrole" Value="0" runat="server" />   
                </div>
                <asp:UpdatePanel ID="upnrevieworeder" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="Button2" runat="server" Style="display: none" />
                        <ajax:ModalPopupExtender ID="mpereview" runat="server"
                            PopupControlID="modalreview" TargetControlID="Button2"
                            BackgroundCssClass="modalBackground" BehaviorID="modalreview" CancelControlID="btnreviewclose">
                        </ajax:ModalPopupExtender>
                        <div id="modalreview" style="display: none;">
                            <div class="modal-dialog-Review">
                                <div class="modal-content">
                                    <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                        <asp:Button ID="btnreviewclose" class="close" runat="server" Text="X" />
                                        <h4 class="modal-title" style="color: green; font-size: large">Machine Parts Receive Review</h4>
                                    </div>
                                    <div class="modal-body" style="padding: 5px;">
                                        <div class="form-horizontal">
                                            <div class="row" style="margin-bottom: 2px;">
                                                <div class="col-lg-4" align="left">
                                                    <asp:Label ID="lblmaorderNo" runat="server" Style="font-weight: 800;">Machine Parts Order Number :- </asp:Label>
                                                    <%--<asp:Label ID="lblmreceiveNo" runat="server" Visible="false" Style="font-weight: 800;">Machine Parts Receive Order Number :- </asp:Label>--%>
                                                    <asp:Label ID="lblMasterNo" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                </div>
                                                <div class="col-lg-4">
                                                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </div>
                                                <div class="col-lg-4" align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" ValidationGroup="EmptyFieldPop"  CausesValidation="true" OnClientClick="return CheckIsValidSave()"  Text="Save" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btncancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btncancel_Click" />
                                                </div>
                                            </div>
                                        </div>
                                      <%--   <asp:Label ID="lblMasterHeader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>--%>
                                        <div class="well well-sm" style="padding: 5px 15px 5px 15px;">
                                            <div id="divContentDetails" runat="server">
                                                <div class="row" id="divPack" runat="server">
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                            <span style="font-weight: 800;">Packing Slip No</span>&nbsp;<span style="color: red">*</span>
                                                            <asp:TextBox ID="txtPackingSlipNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="ReqtxtPackingSlipNo" ValidationGroup="EmptyFieldPop" runat="server" ForeColor="Red"
                                                                ControlToValidate="txtPackingSlipNo" ErrorMessage=""></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                            <span style="font-weight: 800;">Packing Slip Date</span>&nbsp;<span style="color: red">*</span>
                                                            <asp:TextBox ID="txtPackingSlipDate" runat="server" CssClass="form-control" onblur="checkPackingSlipDate()"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="reqPackingSlipDate" ValidationGroup="EmptyFieldPop" runat="server" ForeColor="Red"
                                                                ControlToValidate="txtPackingSlipDate" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            <ajax:CalendarExtender ID="CalPakslipDate" runat="server" TargetControlID="txtPackingSlipDate" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                            <ajax:FilteredTextBoxExtender ID="FilteredPackSlipDate" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtPackingSlipDate"></ajax:FilteredTextBoxExtender>
                                                      
                                                    </div>
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                            <span style="font-weight: 800;">Received Date</span>&nbsp;<span style="color: red">*</span>
                                                            <asp:TextBox ID="txtReceivingDate" runat="server" CssClass="form-control" onblur="checkReceivingDate()"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="ReqReceivedDate" ValidationGroup="EmptyFieldPop" runat="server" ForeColor="Red"
                                                                ControlToValidate="txtReceivingDate" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            <ajax:CalendarExtender ID="CalReceivingDate" runat="server" TargetControlID="txtReceivingDate" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                            <ajax:FilteredTextBoxExtender ID="FilteredReceivingDate" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtReceivingDate"></ajax:FilteredTextBoxExtender>
                                                       
                                                    </div>
                                                </div>
                                                <div class="row" id="divinvoice" runat="server" style="display: none">
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                            <span style="font-weight: 800;">Invoice No</span>&nbsp;<span style="color: red">*</span>
                                                             <asp:RequiredFieldValidator ID="ReqInvoiceNo" ValidationGroup="EmptyFieldPop" runat="server" ForeColor="Red"
                                                                ControlToValidate="txtInvoiceNo" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                            <span style="font-weight: 800;">Invoice Date</span>&nbsp;<span style="color: red">*</span>
                                                            <asp:RequiredFieldValidator ID="ReqInvoiceDate" ValidationGroup="EmptyFieldPop" runat="server" ForeColor="Red"
                                                                ControlToValidate="txtInvoiceDate" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" onblur="checkInvoiceDate()"></asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalInvoiceDate" runat="server" TargetControlID="txtInvoiceDate" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                            <ajax:FilteredTextBoxExtender ID="FilteredInvoiceDate" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtInvoiceDate"></ajax:FilteredTextBoxExtender>
                                                       
                                                    </div>
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                            <span style="font-weight: 800;">Receiving action</span>&nbsp;<span style="color: red">*</span>
                                                            <asp:RequiredFieldValidator ID="ReqddlReceivingAction" runat="server" InitialValue="0" Display="Dynamic" SetFocusOnError="true" Font-Size="0.9em" ControlToValidate="ddlReceivingAction" ValidationGroup="EmptyFieldPop"
                                                                ErrorMessage="" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ID="ddlReceivingAction" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>                                                   
                                                </div>
                                                <div class="row"> 
                                                    <div id="divreason" runat="server" style="display: none;">
                                                        <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                                                                <span style="font-weight: 800;">Reason</span>&nbsp;<span style="color: red">*</span>
                                                                <asp:RequiredFieldValidator ID="ReqddlReason" runat="server" InitialValue="0" ControlToValidate="ddlReason" ValidationGroup="EmptyFieldPop"
                                                                    ErrorMessage="" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control" Onchange="Isshowreason(this)"></asp:DropDownList>
                                                           
                                                        </div>
                                                    </div>
                                                    <div id="otherid" runat="server" style="display:none">
                                                        <div class="col-sm-4 col-md-4 col-lg-4">
                                                               <span id="other" style="font-weight: 800;">Other Reason:</span>
                                                             <%--   <asp:RequiredFieldValidator ID="ReqtxtOthers" runat="server" ControlToValidate="txtOthers" ValidationGroup=""
                                                                    ErrorMessage="" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                                <asp:TextBox ID="txtOthers" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                      </div>
                                                    </div>
                                                </div>
                                            </div>
                                          <asp:Label ID="lblitems" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label>
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="SRMPRReviewgrid">
                                                           <asp:Label ID="lblrcountpo" runat="server"></asp:Label>
                                                          <asp:Label ID="lblrcountro" runat="server"></asp:Label>
                                                        <asp:GridView ID="grdMPRReview" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="grdMPRReview_RowDataBound"
                                                            EmptyDataText="No Records Found" CssClass="table table-responsive" ShowFooter="true">
                                                            <Columns>
                                                                 <asp:TemplateField HeaderText="MachinePartsReceivingDetailsID" visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMachinePartsReceivingDetailsID" runat="server" Text='<%# Eval("MachinePartsReceivingDetailsID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                        <%--<asp:Label ID="lblMachinePartsReceivingDetailsID" runat="server" Text='<%# Eval("MachinePartsReceivingDetailsID") %>'></asp:Label>--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--  <asp:TemplateField HeaderText="Row Number" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Eval("RowNumber") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="ItemID" HeaderStyle-Width="8%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ItemID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Description">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("ItemDescription") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Order Qty" HeaderStyle-Width="4%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOrderQuantity" runat="server" Text='<%# Eval("OrderQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Balance Qty" HeaderStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblbalqty" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                  <asp:TemplateField HeaderText="Price/Unit ($)" HeaderStyle-Width="9%">
                                                                    <ItemTemplate>
                                                                        <span>$</span>
                                                                        <asp:Label ID="lblrevppqty" runat="server" Text='<%# Eval("PricePerQty","{0:F2}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Received Qty" HeaderStyle-Width="9%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtreceivedqty" runat="server" Width="80%"></asp:TextBox>
                                                                          <ajax:FilteredTextBoxExtender ID="Filtertxtreceive" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtreceivedqty"></ajax:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <span style="font-weight: 600; color: black">Shipping Cost</span>
                                                                        <br />
                                                                        <span style="font-weight: 600; color: black">Tax</span>
                                                                        <br />
                                                                        <br />
                                                                        <br />
                                                                        <span style="font-weight: 600; color: black">Total Cost</span>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total Price ($)" HeaderStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <span>$</span>
                                                                        <asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <span style="color: black">$</span>
                                                                        <asp:TextBox ID="txtsipcost" Text="0" Width="80%" onblur="GetShipValue(this)" onchange="GetShipValue(this)" CssClass="txtsipcost" runat="server" ForeColor="Black"></asp:TextBox><br />
                                                                        <ajax:FilteredTextBoxExtender ID="FTBtxtsipcost" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtsipcost"></ajax:FilteredTextBoxExtender>
                                                                        <span style="color: black">$</span>
                                                                        <asp:TextBox ID="txttax" Text="0" Width="80%" onblur="GetTaxVal(this)" onchange="GetTaxVal(this)" runat="server" CssClass="txttax" ForeColor="Black"></asp:TextBox>
                                                                        <ajax:FilteredTextBoxExtender ID="Filteredtxttax" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txttax"></ajax:FilteredTextBoxExtender>
                                                                        <br />
                                                                        <br />
                                                                        <span style="color: black">$</span>
                                                                        <asp:TextBox ID="txtTotalcost" Width="90%" runat="server" CssClass="ToatalcostCLS" ForeColor="Black" disabled="true" BackColor="White"></asp:TextBox>
                                                                        <asp:TextBox ID="lblToatalcost" runat="server" CssClass="ToatalcostLabelCLS HeaderHide"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Comments" HeaderStyle-Width="14%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtComments" runat="server"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="MPRMasterID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbMPRMasterID" CssClass="HeaderHide" runat="server" Text='<%# Eval("MPRMasterID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="MachinePartsRequestOrderID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbMPRDetailsID" CssClass="HeaderHide" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
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
                                <h4 class="modal-title font-bold text-white">Machine Parts Receive Order
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
                                <h4 class="modal-title font-bold text-white">Machine Parts Receive Order
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
                                <h4 class="modal-title font-bold text-white">Machine Parts Receive Order
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

         <div style="display: none">
                    <rsweb:ReportViewer ID="rvMachinePoreport" runat="server"></rsweb:ReportViewer>
                </div>
                <div style="display: none">
                    <rsweb:ReportViewer ID="rvMachineSummaryReport" runat="server"></rsweb:ReportViewer>
                </div>
                <div style="display: none">
                    <rsweb:ReportViewer ID="rvMachineDetailsReport" runat="server"></rsweb:ReportViewer>
                 </div>
               <div style="display: none">
                    <rsweb:ReportViewer ID="rvMachineSummaryPritnAll" runat="server"></rsweb:ReportViewer>
                </div>

        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center> This User don't have a Permission to View This Page...</center>
            </h4>
        </div>

</asp:Content>
