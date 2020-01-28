<%@ Page Title="Receiving-Major Items" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="CapitalReceivingOrder.aspx.cs" Inherits="Inventory.CapitalReceivingOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : CapitalReceivingOrder.aspx 
' Type      : ASPX File 
' Description  :  Capital orders are Received for all corp/facility are done in this screen.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  02/20/2018             V.01               Murali M                     New
  06/Mar/2018          V.01                 Murali.M                   Multi Search 
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

        var shipval = 0;
        var taxval = 0; 
        function GetTotal() {
            //console.log($('tr td#txtsipcost').text());
            $('#grdreview tfoot').each(function () {
                console.log($('th', this).text());
            });
        }
        function GetShipValue(val) {
            shipval = val.value;
            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            var taxVal = $("[id*=grdreview] .txttax").val();
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
            
            var shipVal=$("[id*=grdreview] .txtsipcost").val();
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

            var table = $("[id*=grdreview]");
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
            $(document).on("keyup mouseup", "[id*=txtcomments]", function () {
                var row = $(this).closest("tr");
                if ($(this).val() == "") {
                    if ((parseInt($("[id*=txtreceivedqty]",row).val()) < parseFloat($("[id*=lblOrderQuantity]", row).html()))) {
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
                        var txtcomments = $("[id*=txtcomments]", row);
                        var lblbalqty = parseFloat(parseFloat($("[id*=lblOrderQuantity]", row).html() - $(this).val()));
                        var lbltotprice = parseFloat(parseFloat($("[id*=lblrevppqty]", row).html() * $(this).val()));
                        if (parseInt($(this).val()) > parseFloat($("[id*=lblOrderQuantity]", row).html())) {
                            alert("Received Qty is less than OrderQty");
                            $(this).val('');
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
                            $("[id*=lblbalqty]", row).html("");
                        }
                    }
                        if ($(this).val() != '') {
                            if ($(this).val() != '0') {
                                $('[id*=lblerror]').html("");
                            }
                        }
       
                } else {
                    $(this).val('');
                    $("[id*=lblbalqty]", row).html("");
                    //var lbltotpriceold = parseFloat(parseFloat($("[id*=lblOrderQuantity]", row).html()) * parseFloat($("[id*=lblrevppqty]", row).html()));
                    //$("[id*=lblTotalPrice]", row).html(lbltotpriceold);
                    $("[id*=lblTotalPrice]", row).html('');
                    var txtcomments = $("[id*=txtcomments]", row);
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


        function CheckIsValidSave() {
            var result = false;
            var message='';
            var Usertype = $('[id*=Hdnrole]').val();
            if (Page_ClientValidate()) {
                $("[id*=grdreview] [id*=txtreceivedqty]").each(function () {
                    if ($(this).val() != '') {
                        if ($(this).val() != '0') {
                            result = true;
                        }
                    }                    
                });
                if (result == false) {
                    message = 'Please enter your ReceivedQty';
                }
                $("[id*=grdreview] tr").each(function () {
                    var txtreceivedqty = $(this).find("[id*=txtreceivedqty]").val();
                    var lblOrderQuantity = $(this).find("[id*=lblOrderQuantity]").text();
                    var txtcomments = $(this).find("[id*=txtcomments]").val();
                    
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
            if (Usertype == '1')
            {
                result = true;
            }
            if (result == false)
            {
                $('[id*=lblerror]').html(message);
            }
            else {
                $('[id*=lblerror]').html("");
            }
            return result;
        }


      function Isshowreason(res)
        {
            if(res.value == 'Other')
            {
                $('[id*=otherid]').show();
            }
            else {
                $('[id*=otherid]').hide();
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

        function checkDate1(txt) {
            //var dateString = $('[id*=txtpackingslip]').val();
            var dateString = txt.value;
            //console.log(dateString);
            var myDate = new Date(dateString);
            var today = new Date();
            if (myDate > today) {
                //console.log('asdfas');
                $('[id*=lblerror]').html("Please select within today date");
                txt.value = '';
            }
            else {
                console.log(myDate);
              $('[id*=lblerror]').html("");
                return true;
            }
          

        }
        function checkDate2(txt) {
            //var dateString = $('[id*=txtreceived]').val();
            var dateString = txt.value;
            var myDate = new Date(dateString);
            var today = new Date();
            if (myDate > today) {
                $('[id*=lblerror]').html("Please select within today date");
                txt.value = '';
            }
            else {
                $('[id*=lblerror]').html("");
                return true;
            }

        }
        function checkDate3(txt) {
            //var dateString = $('[id*=txtinvoicedate]').val();
            var dateString = txt.value;
            var dateString2 = $('.clstxtreceived').val();
            var myDate1 = new Date(dateString);
            var myDate2 = new Date(dateString2);
            var today = new Date();
            if (myDate1 < myDate2) {
                $('[id*=lblerror]').html("Invoice Date should not be less than Received Date");
                txt.value = '';
            }
            else {
                $('[id*=lblerror]').html("");
                return true;
            }
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


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Major Item Receiving Order
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">                   
                    Sys.Application.add_load(jScript);
                    Sys.Application.add_load(jscriptsearch);
                    Sys.Application.add_load(Auditpopupshow);
                    Sys.Application.add_load(Remarkspopupshow);
                    Sys.Application.add_load(CorpDrop);
                </script>
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
                <div id="divMPOrder" runat="server" class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-4" align="left">
                            <asp:Label ID="lblseroutHeader" runat="server" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-lg-8" align="right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="Empty" OnClick="btnSearch_Click" />
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
                                        <asp:ListBox ID="drpcorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpcorsearch_SelectedIndexChanged"></asp:ListBox> 
                                         <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpcorsearch" ValidationGroup="Empty" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>                                           
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiFac" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearFac" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllFac" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllFac_Click"></asp:LinkButton>        
                                        <asp:ListBox ID="drpfacilitysearch" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="drpfacilitysearch_SelectedIndexChanged" ></asp:ListBox>
                                          <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpfacilitysearch" ValidationGroup="Empty" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacilitysearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>          
                                    </div>

                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>    
                                        <asp:ListBox ID="drpvendorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple" ></asp:ListBox>
                                        <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendorsearch" ValidationGroup="Empty" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>   
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date From</span>&nbsp;<span style="color: red">*</span>  
                                         <asp:RegularExpressionValidator ID="revdatefrom" ControlToValidate="txtDateFrom"
                                            ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                            SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="Empty" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateFrom" runat="server" TargetControlID="txtDateFrom" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilterDateFrom" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateFrom"></ajax:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="ReqDateFrom" runat="server" ControlToValidate="txtDateFrom" ValidationGroup="Empty"
                                            ErrorMessage="This information is required" ForeColor="Red"  SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date To</span>&nbsp;<span style="color: red">*</span>  
                                         <asp:RegularExpressionValidator ID="revdateto" ControlToValidate="txtDateTo"
                                            ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                            SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="Empty" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilterDateTo" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateTo"></ajax:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="ReqtxtDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="Empty"
                                            ErrorMessage="This information is required" ForeColor="Red"  SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Status</span>&nbsp;<span style="color: red">*</span>
                                        <asp:ListBox ID="drpStatussearch" runat="server" CssClass="form-control" SelectionMode="Multiple" ></asp:ListBox>   
                                          <asp:RequiredFieldValidator InitialValue="" ID="ReqdrpStatus" runat="server" ForeColor="Red" ValidationGroup="Empty"
                                            ControlToValidate="drpStatussearch" ErrorMessage="This information is required."  SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>             
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div style="margin-left: 1px; margin-top: 3px;" id="divsearch">
                            <div class="row">
                                <div class="col-lg-3" align="left">
                                    <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                </div>
                                <div class="col-lg-9" align="right">
                                    <asp:Button ID="btnrefresh" runat="server" Text="Refresh" CssClass="btn btn-primary" OnClick="btnrefresh_Click" />
                                </div>
                            </div>
                              <asp:Label ID="lblrcount" runat="server">No of records : <%=grdCRReceivingPO.Rows.Count.ToString() %></asp:Label>
                            <div class="MPRSearchgrid">
                                <asp:GridView ID="grdCRReceivingPO" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="grdCRReceivingPO_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Summary" HeaderStyle-Width="5%" >
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgprint" runat="server" Text="Summary" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgprint_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Detail" HeaderStyle-Width="5%" >
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgprintedit" runat="server" Text="Detail" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgprintedit_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CapitalReceivingMasterID" HeaderText="CapitalReceivingMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CapitalItemMasterID" HeaderText="CapitalItemMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CreatedOn" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CorporateName" HeaderText="Corp" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="VendorShortName" HeaderText="Vendor" HeaderStyle-Width="8%" />
                                        <asp:TemplateField HeaderText="CPONo">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbcrono" runat="server" Text='<%# Eval("CRONo")%>' OnClick="lbcrono_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CPRNo">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbCRrno" runat="server" Text='<%# Eval("CPRONo")%>' OnClick="lbCRrno_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Price" HeaderText="Price($)" DataFormatString="$ {0:#,0.00}" HeaderStyle-Width="9%" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblaudit" runat="server" Text=' <%# Eval("Audit")%>'></asp:Label>--%>
                                                <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>--%>
                                                <asp:Image ID="imgreadmore" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Remarks")%>' data-toggle="popover" />
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                          <asp:BoundField DataField="CapitalOrderID" HeaderText="CapitalOrderID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
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
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:HiddenField ID="HdnMasterID" runat="server" />
          <asp:HiddenField ID="HdnMReqID" runat="server" />
         <asp:HiddenField ID="Hdncpo" runat="server" />
          <asp:HiddenField ID="Hdnrole" runat="server" />
          <asp:HiddenField ID="Hdnorderid" runat="server" />
        
        <asp:UpdatePanel ID="updaterequestreview" runat="server">
            <ContentTemplate>
                <asp:Button ID="Button4" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="modalreviewreq" runat="server"
                    PopupControlID="modalreviewCR" TargetControlID="Button4"
                    BackgroundCssClass="modalBackground" BehaviorID="modalreviewCR" CancelControlID="btncrreviewclose">
                </ajax:ModalPopupExtender>
                <div id="modalreviewCR" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="btncrreviewclose" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: green; font-size: large">Major Item Order</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="form-horizontal">
                                    <div class="row" style="margin-bottom: 2px;">
                                        <div class="col-lg-4" align="left">
                                            <span style="font-weight: 800;">Major Item Order Number :- </span>
                                            <asp:Label ID="lblmprreview" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                        <div class="col-lg-4">
                                            <asp:Label ID="lblerror" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                        <div class="col-lg-4" align="right">
                                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="btn btn-success" CausesValidation="true" OnClientClick="return CheckIsValidSave()" ValidationGroup="EmptySave" OnClick="btnsave_Click" />
                                            <asp:Button ID="btncancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btncancel_Click" />
                                        </div>
                                    </div>
                                    <div class="well well-sm" style="padding: 5px 15px 5px 15px;">
                                        <div class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <span style="font-weight: 800;">Packing Silp No:</span>
                                                  <asp:RequiredFieldValidator ID="Reqpackingsilpno" runat="server" ControlToValidate="packingsilpno" ValidationGroup="EmptySave"
                                            ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="packingsilpno" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <span style="font-weight: 800;">Packing Silp Date:</span>
                                               <asp:RequiredFieldValidator ID="Reqtxtpackingslip" runat="server" ControlToValidate="txtpackingslip" ValidationGroup="EmptySave"
                                           ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtpackingslip" runat="server" CssClass="form-control" onchange="checkDate1(this)" placeholder="--/--/----"></asp:TextBox>
                                                <ajax:CalendarExtender ID="calpacslip" runat="server" TargetControlID="txtpackingslip" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                <ajax:FilteredTextBoxExtender ID="filterpacking" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtpackingslip"></ajax:FilteredTextBoxExtender>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <span style="font-weight: 800;">Received Date:</span>
                                                 <asp:RequiredFieldValidator ID="Reqtxtreceived" runat="server" ControlToValidate="txtreceived" ValidationGroup="EmptySave"
                                           ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtreceived" runat="server" CssClass="form-control clstxtreceived aspNetDisabled" onchange="checkDate2(this)"></asp:TextBox>
                                                <ajax:CalendarExtender ID="calrecedate" runat="server" TargetControlID="txtreceived" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                <ajax:FilteredTextBoxExtender ID="filterreceived" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtreceived"></ajax:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div id="Divsupad" runat="server">
                                            <div class="row">
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <span style="font-weight: 800;">Invoice No: </span>
                                                     <asp:RequiredFieldValidator ID="Reqtxtinvoiceno" runat="server" ControlToValidate="txtinvoiceno" ValidationGroup="EmptySave"
                                            ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtinvoiceno" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <span style="font-weight: 800;">Invoice Date:</span>
                                                   <asp:RequiredFieldValidator ID="Reqtxtinvoicedate" runat="server" ControlToValidate="txtinvoicedate" ValidationGroup="EmptySave"
                                          ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtinvoicedate" runat="server" CssClass="form-control" onchange="checkDate3(this)" placeholder="--/--/----"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="calinvoice" runat="server" TargetControlID="txtinvoicedate" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                    <ajax:FilteredTextBoxExtender ID="filterinvoice" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtinvoicedate"></ajax:FilteredTextBoxExtender>
                                                </div>
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <span style="font-weight: 800;">Receiving Action:</span>
                                                     <asp:RequiredFieldValidator ID="ReqddlreceivingAct" runat="server" InitialValue="0" ControlToValidate="ddlreceivingAct" ValidationGroup="EmptySave"
                                           ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                                    <asp:DropDownList ID="ddlreceivingAct" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <span style="font-weight: 800;">Reason:</span>
                                                      <asp:RequiredFieldValidator ID="Reqddlreason" runat="server" InitialValue="0" ControlToValidate="ddlreason" ValidationGroup="EmptySave"
                                            ErrorMessage="This information is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                                    <asp:DropDownList ID="ddlreason" runat="server" CssClass="form-control" onchange="Isshowreason(this);"></asp:DropDownList>
                                                </div>

                                                <div id="otherid" runat="server" style="display:none">
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                        <span id="other" style="font-weight: 800;">Other Reason:</span>
                                                          <%--<asp:RequiredFieldValidator ID="Reqtxtreason" runat="server" ControlToValidate="txtreason" 
                                                       ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                        <asp:TextBox ID="txtreason" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hdnmaster" runat="server" />
                                     <asp:HiddenField ID="HddGridCount" runat="server" Value="0" />
                                    <asp:Label ID="lblitems" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label>
                                    <div class="row">
                                        <div class="col-lg-12">
                                                <asp:Label ID="lblrcountpo" runat="server"></asp:Label>
                                                <asp:Label ID="lblrcountro" runat="server" Visible="false"></asp:Label>
                                            <div class="SRReviewgrid">
                                                <asp:GridView ID="grdreview" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                     EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="grdreview_RowDataBound" ShowFooter="true">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="CapitalReceivingDetailsID" visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbCRMasterID" runat="server" Text='<%# Eval("CapitalReceivingDetailsID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <%-- <asp:TemplateField HeaderText="CapitalItemDetailsID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbCRDetailsID" runat="server" Text='<%# Eval("CapitalItemDetailsID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <%-- <asp:TemplateField HeaderText="CapitalReceivingMasterID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbCReceivingID" runat="server" Text='<%# Eval("CapitalReceivingMasterID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Equipment Category">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblequrecat" runat="server" Text='<%# Eval("EquimentSubCategory") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Equipment List">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblequrelst" runat="server" Text='<%# Eval("EquipmentList") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SerialNo">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblser" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <%-- <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblreason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Price per Qty ($)" HeaderStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <span>$</span>
                                                                <asp:Label ID="lblrevppqty" runat="server" Text='<%# Eval("PricePerQty","{0:F2}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Order Qty" HeaderStyle-Width="4%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderQuantity" runat="server" Text='<%# Eval("OrderQty") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Balance Qty" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblbalqty" runat="server" ></asp:Label>
                                                            </ItemTemplate>
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
                                                                <span style="font-weight: 600; color: black">Total Cost</span>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Price ($)" HeaderStyle-Width="10%" ItemStyle-Width="10%">
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
                                                                <%--<asp:Label ID="lbltotalcost" runat="server"></asp:Label>--%>
                                                                <asp:TextBox ID="txtTotalcost" Width="90%" runat="server" CssClass="ToatalcostCLS" ForeColor="Black" disabled="true" BackColor="White"></asp:TextBox>
                                                                <%--<asp:TextBox ID="lblToatalcost" runat="server" CssClass="ToatalcostLabelCLS HeaderHide"></asp:TextBox>--%>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Comments" HeaderStyle-Width="13%" ItemStyle-Width="13%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtcomments" runat="server"></asp:TextBox>
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
                            </div>
                        </div>
                    </div>
                </div>

                <div id="modalSave" class="modal fade" style="position: center">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content ">
                            <div class="modal-header bg-green">
                                <h4 class="modal-title font-bold text-white">Major Item Receiving
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
                                <h4 class="modal-title font-bold text-white">Major Item Receiving
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
                                <h4 class="modal-title font-bold text-white">Major Item Receiving
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
         <rsweb:ReportViewer ID="rvCapitalPodetails" runat="server"></rsweb:ReportViewer>
         </div>
         <div style="display: none">
             <rsweb:ReportViewer ID="rvCapitalPoreportReview" runat="server"></rsweb:ReportViewer>
        </div>
         <div style="display: none">
            <rsweb:ReportViewer ID="rvCapitalPoSummary" runat="server"></rsweb:ReportViewer>
        </div>
          <div style="display: none">
            <rsweb:ReportViewer ID="rvCapitalPoPrintAll" runat="server"></rsweb:ReportViewer>
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
