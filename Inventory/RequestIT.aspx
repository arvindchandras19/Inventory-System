<%@ Page Title="Request-IT Request" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="RequestIT.aspx.cs" Inherits="Inventory.RequestIT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : RequestIT.aspx 
' Type      : ASPX File 
' Description  :   To design the Request IT Parts page for add,Update and show the Request IT Parts on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
 12/15/2017           V.01             S.Mahalakshmi,C.Dhanasekaran         New
 01/08/2017           V.01                S.Mahalakshmi                 Addnew row while edit functionality,order functionality
  05/Mar/2018          V.01              Vivekanand.S                   Multi Search  
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
                    //ShowwarningPopup('Multiple selection are not allowed here. Use Multi select link for multiple selection.');
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
                    //ShowwarningPopup('Multiple selection are not allowed here. Use Multi select link for multiple selection.');
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
        var taxval = 0; +
        function GetTotal() {
            console.log($('tr td#txtsipcost').text());
            $('#grdreview tfoot').each(function () {
                console.log($('th', this).text());
            });
        }
        function GetShipValue(val) {
            shipval = val.value;
            //alert(shipval);
            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            SetTotalCost(rowIndex);
        }
        function GetTaxVal(val) {
            taxval = val.value;

            var rowData = val.parentNode.parentNode;
            var rowIndex = rowData.rowIndex;

            SetTotalCost(rowIndex);
        }
        function SetTotalCost(rowIndex) {

            var table = $("[id*=grdreview]");
            var totalCost = table.find("tr").eq(rowIndex).find("td").eq(8).find("input.ToatalcostLabelCLS").val();
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
                table.find("tr").eq(rowIndex).find("td").eq(8).find("input.ToatalcostCLS").val();
            } else {
                table.find("tr").eq(rowIndex).find("td").eq(8).find("input.ToatalcostCLS").val(res);

            }
        }

        $(function () {
            $(document).on("keyup mouseup", "[id*=txtqty]", function () {
                console.log('a');
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var OrderQuanttityval = parseFloat($(this).val()) * parseFloat($("[id*=txtppq]", row).val());
                        if (isNaN(OrderQuanttityval) == false)
                            $("[id*=txttotprice]", row).val(OrderQuanttityval.toString());
                        else
                            $("[id*=txttotprice]", row).val("");
                    }
                } else {
                    $(this).val('');
                    $("[id*=txttotprice]", row).val("");
                }
            });

            $(document).on("keyup mouseup", "[id*=txtppq]", function () {
                console.log('a');
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var OrderQuanttityval = parseFloat($(this).val()) * parseFloat($("[id*=txtqty]", row).val());
                        if (isNaN(OrderQuanttityval) == false)
                            $("[id*=txttotprice]", row).val(OrderQuanttityval.toString());
                        else
                            $("[id*=txttotprice]", row).val("");
                    }
                } else {
                    $(this).val('');
                    $("[id*=txttotprice]", row).val("");
                }
            });
        });

        $(function () {
            $(document).on("keyup mouseup", "[id*=txteqty]", function () {
                console.log('a');
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var OrderQuanttityval = parseFloat($(this).val()) * parseFloat($("[id*=txtePricePerUnit]", row).val());
                        if (isNaN(OrderQuanttityval) == false)
                            $("[id*=txteTotalPrice]", row).val(OrderQuanttityval.toString());
                        else
                            $("[id*=txteTotalPrice]", row).val("");
                        console.log(OrderQuanttityval);
                    }
                }
                else {
                    $(this).val('');
                    $("[id*=txteTotalPrice]", row).val("");
                }
            });
        });

        $(document).on("keyup mouseup", "[id*=txtePricePerUnit]", function () {
            console.log('a');
            var row = $(this).closest("tr");
            if (!jQuery.trim($(this).val()) == '') {
                if (!isNaN(parseFloat($(this).val()))) {
                    var row = $(this).closest("tr");
                    var OrderQuanttityval = parseFloat($(this).val()) * parseFloat($("[id*=txteqty]", row).val());
                    if (isNaN(OrderQuanttityval) == false)
                        $("[id*=txteTotalPrice]", row).val(OrderQuanttityval.toString());
                    else
                        $("[id*=txteTotalPrice]", row).val("");
                }
            } else {
                $(this).val('');
                $("[id*=txteTotalPrice]", row).val("");
            }
        });      


        $(function () {
            $(document).on("keyup mouseup", "[id*=FooteOrderQuantity]", function () {
                console.log('a');
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var OrderQuanttityval = parseFloat($(this).val()) * parseFloat($("[id*=FootePricePerUnit]", row).val());
                        if (isNaN(OrderQuanttityval) == false)
                            $("[id*=FooteTotalPrice]", row).val(OrderQuanttityval.toString());
                        else
                            $("[id*=FooteTotalPrice]", row).val("");
                        console.log(OrderQuanttityval);
                    }
                }
                else {
                    $(this).val('');
                    $("[id*=FooteTotalPrice]", row).val("");
                }
            });
        });

        $(document).on("keyup mouseup", "[id*=FootePricePerUnit]", function () {
            console.log('a');
            var row = $(this).closest("tr");
            if (!jQuery.trim($(this).val()) == '') {
                if (!isNaN(parseFloat($(this).val()))) {
                    var row = $(this).closest("tr");
                    var OrderQuanttityval = parseFloat($(this).val()) * parseFloat($("[id*=FooteOrderQuantity]", row).val());
                    if (isNaN(OrderQuanttityval) == false)
                        $("[id*=FooteTotalPrice]", row).val(OrderQuanttityval.toString());
                    else
                        $("[id*=FooteTotalPrice]", row).val("");
                }
            } else {
                $(this).val('');
                $("[id*=FooteTotalPrice]", row).val("");
            }
        });


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


        $(function () {
            $(document).on("keyup mouseup", "[id*=gvSearchMRPDetails] [id*=txteqty]", function () {
                console.log('Qty');
                var row = $(this).closest("tr");
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        var OrderQuanttityval = parseFloat($(this).val()) * parseFloat($("[id*=txtePricePerUnit]", row).html());
                        console.log($("[id*=txtePricePerUnit]", row).html());
                        var txtreason = $("[id*=txtereason]", row);

                        var lblqty = $("[id*=lblqty]", row).html();
                        if (lblqty != '') {
                            if ($(this).val() != lblqty) {

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

                        if (isNaN(OrderQuanttityval) == false)
                            $("[id*=txteTotalPrice]", row).html(OrderQuanttityval.toString());
                        else
                            $("[id*=txteTotalPrice]", row).html("");
                        console.log('Total' + OrderQuanttityval);
                    }
                }
                else {
                    $(this).val('');
                    $("[id*=txteTotalPrice]", row).html("");
                }
            });
        });

        $(function () {
            $(document).on("keyup mouseup", "[id*=gvSearchMRPDetails] [id*=txtereason]", function () {
                console.log('Qty');
                var row = $(this).closest("tr");
                var txtreason = $(this);
                var lblqty = $("[id*=lblqty]", row).html();
                var txtqty = $("[id*=txteqty]", row).val();
                if (lblqty != '') {
                    if (txtqty != lblqty) {
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

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Request IT Parts 
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
                <div id="divITrequest" class="mypanel-body" runat="server" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-4" align="left">
                            <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-lg-8" align="right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyField" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                            <asp:Button ID="btnReview" Visible="false" runat="server" CssClass="btn btn-primary" ValidationGroup="EmptyFieldSave" Text="Review" OnClick="btnReview_Click" />
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                        </div>
                    </div>
                    <div id="divMPRMaster" runat="server" style="margin-top: 5px;">
                        <div id="divContent" class="well well-sm" runat="server">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiCorp" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearCorp" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllCorp" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllCorp_Click"></asp:LinkButton>  
                                        <asp:ListBox ID="drpcorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpcorsearch_SelectedIndexChanged"></asp:ListBox> 
                                        <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpcorsearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>                                           
                                        <%--<asp:DropDownList ID="drpcorsearch" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpcorsearch_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>  
                                        <asp:LinkButton ID="lnkMultiFac" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearFac" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllFac" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllFac_Click"></asp:LinkButton> 
                                        <asp:ListBox ID="drpfacilitysearch" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="drpfacilitysearch_SelectedIndexChanged" ></asp:ListBox>
                                         <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpfacilitysearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacilitysearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <%--<asp:DropDownList ID="drpfacilitysearch" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpfacilitysearch_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                    </div>

                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>
                                        <asp:ListBox ID="drpvendorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple" ></asp:ListBox>
                                         <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendorsearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <%--<asp:DropDownList ID="drpvendorsearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
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
                                        <asp:RequiredFieldValidator ID="ReqDateFrom" runat="server" ControlToValidate="txtDateFrom" ValidationGroup="EmptyField"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date To</span>&nbsp;<span style="color: red">*</span>   
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilterDateTo" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateTo"></ajax:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="ReqtxtDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="EmptyField"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Status</span>&nbsp;<span style="color: red">*</span>
                                        <asp:ListBox ID="drpStatussearch" runat="server" CssClass="form-control" SelectionMode="Multiple" ></asp:ListBox>
                                          <asp:RequiredFieldValidator InitialValue="" ID="ReqdrpStatus" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpStatussearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator> 
                                        <%--<asp:DropDownList ID="drpStatussearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    </div>
                                </div>
                              </div>
                            <div class ="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Request Type</span>&nbsp;<span style="color: red">*</span>
                                         <asp:RadioButtonList ID="rdorequesttypesearch" runat="server" RepeatDirection="Horizontal" CssClass="rbl">
                                            <asp:ListItem Text="ALL" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="New" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Replacement" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="margin-left: 1px; margin-top: 3px;" id="divsearch">
                            <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            <div class="row" style="margin-left: 1px; margin-top: 3px;">
                            <asp:Label ID="lblcount" runat="server">No of records : <%=grdITMastersearch.Rows.Count.ToString() %></asp:Label>
                             </div>
                            <div class="ITSearchgrid">
                                <asp:GridView ID="grdITMastersearch" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="grdITMastersearch_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/edit.png" OnClick="imgbtnEdit_Click" />
                                                <asp:ImageButton ID="imgprint" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgprint_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RequestITMasterID" HeaderText="RequestITMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />   
                                        <asp:BoundField DataField="CreatedOn" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="10%"/>                                     
                                        <asp:BoundField DataField="Corporate" HeaderText="Corp" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="Facility" HeaderText="Facility" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="Vendor" HeaderText="Vendor" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="ITRNO" HeaderText="ITR NO" />
                                        <asp:BoundField DataField="Reequestype" HeaderText="Type" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="TotalCost" HeaderText="TotalPrice($)" DataFormatString="$ {0:#,0.00}" HeaderStyle-Width="10%" />
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
                                        <asp:BoundField DataField="Shipping" HeaderText="Shipping" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <%--<asp:BoundField DataField="User" HeaderText="User" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" /> --%>
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
                        <asp:HiddenField ID="hdncheckfield" Value="0" runat="server" />
                    </div>

                    <div id="divMPRDetails" runat="server" style="margin-top: 5px; display: none">
                        <div runat="server" style="margin-left: 1px; margin-top: 3px; display: none;" id="divEdit">
                           <asp:Label ID="lblrcount" runat="server">No of records : <%=GvTempEdit.Rows.Count.ToString() %></asp:Label>
                            <div class="MPREditgridup">
                            <asp:GridView ID="GvTempEdit" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="GvTempEdit_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/edit.png" OnClick="imgbtnEdit_Click" />
                                            <asp:ImageButton ID="imgprint" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgprint_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RequestITMasterID" HeaderText="RequestITMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="10%" />
                                    <%--<asp:BoundField DataField="EquipmentCategoryID" HeaderText="EquipmentCategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="EquipementListID" HeaderText="EquipementListID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />--%>
                                    <asp:BoundField DataField="Corporate" HeaderText="Corp" HeaderStyle-Width="8%" />
                                    <asp:BoundField DataField="Facility" HeaderText="Facility" HeaderStyle-Width="8%" />
                                    <asp:BoundField DataField="Vendor" HeaderText="Vendor"  HeaderStyle-Width="8%"/>
                                    <asp:BoundField DataField="ITRNO" HeaderText="ITR NO" />
                                    <asp:BoundField DataField="Reequestype" HeaderText="Type" HeaderStyle-Width="8%" />
                                    <asp:BoundField DataField="TotalCost" HeaderText="TotalPrice($)" DataFormatString="$ {0:#,0.00}" HeaderStyle-Width="10%" />
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
                                    <asp:BoundField DataField="Shipping" HeaderText="Shipping" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide"/>
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
                        <div>
                            <asp:Label ID="lblMasterHeader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>
                            <div id="DivMPRMasterNo" runat="server" style="display: none;">
                                <span style="font-weight: 800;">Request IT PartsNo:- </span>
                                <asp:Label ID="lblMasterNo" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </div>
                            <div id="divContentDetails" runat="server" class="well well-sm">
                                <div class="row">
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldValidator5" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                ControlToValidate="ddlCorporate" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="ddlCorporate" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCorporate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqdrdddlFacility" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                ControlToValidate="ddlFacility" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="ddlFacility" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqdrdddlVendor" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                ControlToValidate="ddlVendor" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Shipping</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqdrdddlShipping" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                ControlToValidate="ddlShipping" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="ddlShipping" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Request Type</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator InitialValue="" ID="reqaddrequesttype" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                ControlToValidate="rdorequesttype" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            <asp:RadioButtonList ID="rdorequesttype" runat="server" RepeatDirection="Horizontal" CssClass="rbl">
                                                <asp:ListItem Value="0">New</asp:ListItem>
                                                <asp:ListItem Value="1">Replacement</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>                                    
                                </div>
                            </div>
                        </div>

                        <asp:HiddenField ID="HddMasterID" runat="server" />
                        <asp:HiddenField ID="HddDetailsID" runat="server" />
                        <asp:HiddenField ID="HddDetailRowID" runat="server" />
                        <asp:HiddenField ID="HddUserID" runat="server" />   
                        <asp:HiddenField ID="HddUpdateLockinEdit" runat="server" />
                        <asp:HiddenField ID="HddListCorpID" runat="server" />
                        <asp:HiddenField ID="HddListFacID" runat="server" />
                        <div runat="server" style="margin-left: 1px; margin-top: 3px; display: none;" id="divSearchMachine">
                            <%--<asp:Label ID="lblItemHeader" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label><br />--%>
                            <asp:Button runat="server" Text="Add New Row" CssClass="btn btn-primary" ID="btnSearchNewRow" OnClick="btnSearchNewRow_Click" /><br />
                             <asp:Label ID="lblrcount1" runat="server">No of records : <%=gvSearchMRPDetails.Rows.Count.ToString() %></asp:Label>
                            <div class="MPREditgrid">
                                <asp:GridView ID="gvSearchMRPDetails" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnSearchDelete" AlternateText="Delete" runat="server" Height="20px" ImageUrl="~/Images/Delete.png" ToolTip="Delete" OnClick="btnSearchDelete_Click" />
                                                <asp:Label ID="lbITRMasterID" runat="server" Text='<%# Eval("ITRequestMasterID") %>' CssClass="HeaderHide"></asp:Label>
                                                <asp:Label ID="lbITRDetailsID" runat="server" Text='<%# Eval("ITRequestDetailID") %>' CssClass="HeaderHide"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="btnSaveRow" runat="server" Width="18px" Height="20px" ImageUrl="~/Images/save.png" OnClick="btnSaveRow_Click" />
                                                <asp:ImageButton ID="btnRemoveRow" runat="server" Width="18px" Height="20px" ImageUrl="~/Images/Delete.png" OnClick="btnRemoveRow_Click" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Equipment Sub Category">
                                            <ItemTemplate>
                                                <%--<asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control input-sm" AppendDataBoundItems="true"></asp:DropDownList>--%>
                                                <asp:Label ID="lbleequipcat" runat="server" Text='<%# Eval("EquipmentSubCategory") %>' ></asp:Label>
                                                <asp:Label ID="lbleequipsubcatID" runat="server" Text='<%# Eval("EquipementSubCategoryID") %>' Visible="false"></asp:Label>
                                                <%-- <asp:DropDownList ID="ddleequipcat" runat="server" CssClass="form-control input-sm" AppendDataBoundItems="true"></asp:DropDownList>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlFooteequipcat" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlFooteequipcat_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Equipment List">
                                            <ItemTemplate>
                                                <%--<asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control input-sm" AppendDataBoundItems="true"></asp:DropDownList>--%>
                                                <asp:Label ID="lbleequiplst" runat="server" Text='<%# Eval("EquipmentList") %>' ></asp:Label>
                                                <asp:Label ID="lbleequiplistID" runat="server" Text='<%# Eval("EquipementListID") %>' Visible="false"></asp:Label>
                                                <%-- <asp:DropDownList ID="ddleequiplst" runat="server" CssClass="form-control input-sm" AppendDataBoundItems="true"></asp:DropDownList>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlFooteequiplst" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlFooteequiplst_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleserialNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblfootserialno" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <%-- <asp:TextBox ID="txtsearuser" runat="server" Text='<%# Eval("User") %>'></asp:TextBox>--%>
                                                <asp:Label ID="txtsearuser" runat="server" Text='<%# Eval("User") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtfootuser" runat="server" Width="75%" ></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <%--<asp:TextBox ID="OrderQuantity" runat="server" Text='<%# Eval("OrderQuantity") %>' AutoPostBack="true" OnTextChanged="PricePerUnit_TextChanged" />--%>
                                                <asp:TextBox ID="txteqty" runat="server" MaxLength="10" Width="75%"  Text='<%# Eval("OrderQuantity") %>'/>
                                                <asp:Label ID="lblqty" runat="server" Text='<%# Eval("OrderQuantity") %>' CssClass="hidden"/>
                                                <ajax:FilteredTextBoxExtender ID="FilterOrderQuantity" FilterType="Numbers" runat="server" TargetControlID="txteqty"></ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%--<asp:TextBox ID="FootOrderQuantity" runat="server" AutoPostBack="true" OnTextChanged="FootOrderQuantity_TextChanged" />--%>
                                                <asp:TextBox ID="FooteOrderQuantity" runat="server" Width="75%" />
                                                 <asp:Label ID="lblqty" runat="server" Text='<%# Eval("OrderQuantity") %>' CssClass="hidden"/>
                                                <ajax:FilteredTextBoxExtender ID="FilterFootOrderQuantity" FilterType="Numbers" runat="server" TargetControlID="FooteOrderQuantity"></ajax:FilteredTextBoxExtender>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price/Unit ($)" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <div class="Currency-group">
                                                    <span class="Currency-group-addon">
                                                        <i class="fa fa-dollar"></i>
                                                    </span>
                                                    <asp:TextBox ID="txtePricePerUnit" runat="server" Text='<%# Eval("PriceperUnit", "{0:F2}") %>' Width="75%"  Enabled="false" />

                                                </div>
                                                <ajax:FilteredTextBoxExtender ID="FilterPricePerUnit" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtePricePerUnit"></ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div class="Currency-group">
                                                    <span class="Currency-group-addon">
                                                        <i class="fa fa-dollar"></i>
                                                    </span>
                                                    <asp:TextBox ID="FootePricePerUnit" runat="server" Width="75%"  />
                                                </div>
                                                <ajax:FilteredTextBoxExtender ID="FilterFootPricePerUnit" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="FootePricePerUnit"></ajax:FilteredTextBoxExtender>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Total Price ($)" HeaderStyle-Width="14%">
                                            <ItemTemplate>
                                                <div class="Currency-group">
                                                    <span class="Currency-group-addon">
                                                        <i class="fa fa-dollar"></i>
                                                    </span>
                                                    <asp:TextBox ID="txteTotalPrice" runat="server" Width="90%"  Text='<%# Eval("TotalPrice", "{0:F2}") %>' disabled="true" BackColor="White" />
                                                </div>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div class="Currency-group">
                                                    <span class="Currency-group-addon">
                                                        <i class="fa fa-dollar"></i>
                                                    </span>
                                                    <asp:TextBox ID="FooteTotalPrice" runat="server" Width="90%" disabled="true" BackColor="White" />
                                                </div>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtereason" runat="server" Text='<%# Eval("Reason") %>' Width="90%" ></asp:TextBox>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                  <asp:TextBox ID="txtfootreason" runat="server" Width="90%"></asp:TextBox>
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

                        <div runat="server" style="margin-left: 1px; margin-top: 3px; display: none;" id="divAddMachine">                           
                            <asp:Label ID="lblAddItemHeader" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label><br />
                            <asp:Button runat="server" Text="Add New Row" CssClass="btn btn-primary" ID="btn_New" OnClick="btn_New_Click" /><br />
                             <asp:Label ID="lblrcount2" runat="server">No of records : <%=gvAddITRDetails.Rows.Count.ToString() %></asp:Label>
                            <div class="MPRAddgrid">
                                <asp:GridView ID="gvAddITRDetails" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" overflow-y="scroll" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="gvAddITRDetails_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false" />
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnAddDelete" runat="server" Text="Delete" Height="20px" ImageUrl="~/Images/Delete.png" OnClick="btnAddDelete_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SINo" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Equipment Sub Category">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpequipsubcat" runat="server" OnSelectedIndexChanged="drpequipsubcat_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Equipment List">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpequiplst" runat="server" OnSelectedIndexChanged="drpequiplst_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblserialNo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtuser" runat="server" MaxLength="10" CssClass="form-control" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtqty" runat="server" MaxLength="10" CssClass="form-control" />
                                                <ajax:FilteredTextBoxExtender ID="Filterqty" FilterType="Numbers,Custom" ValidChars=".$" runat="server" TargetControlID="txtqty"></ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price per Qty" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <div class="Currency-group">
                                                    <span class="Currency-group-addon">
                                                        <i class="fa fa-dollar"></i>
                                                    </span>
                                                    <asp:TextBox ID="txtppq" runat="server" CssClass="form-control" />
                                                </div>
                                                <ajax:FilteredTextBoxExtender ID="FilterPricePerqty" FilterType="Numbers,Custom" ValidChars=".$" runat="server" TargetControlID="txtppq"></ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Price($)">
                                            <ItemTemplate>
                                                <div class="Currency-group">
                                                    <span class="Currency-group-addon">
                                                        <i class="fa fa-dollar"></i>
                                                    </span>
                                                    <asp:TextBox ID="txttotprice" runat="server" disabled="true" BackColor="White" CssClass="form-control" />
                                                </div>
                                                <ajax:FilteredTextBoxExtender ID="FilterPricePerUnit" FilterType="Numbers,Custom" ValidChars=".$" runat="server" TargetControlID="txttotprice"></ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtreason" runat="server" CssClass="form-control" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle Width="10px" />
                                    <HeaderStyle CssClass="Headerstyle" />
                                    <FooterStyle CssClass="gridfooter" />
                                    <PagerStyle CssClass="pagerstyle" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle CssClass="gridselectedrow" />
                                    <EditRowStyle CssClass="gr  ideditrow" />
                                    <AlternatingRowStyle CssClass="gridalterrow" />
                                    <RowStyle CssClass="gridrow" />
                                </asp:GridView>
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
                                <asp:ImageButton ID="btnImgDeletePopUp" runat="server" CssClass="btn btn-danger" AlternateText="Yes" OnClick="btnImgDeletePopUp_Click" />
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
                                <h4 class="modal-title font-bold text-white">IT Parts Request
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="lblsave" runat="server"></asp:Label><asp:LinkButton ID="lbpopprint" runat="server" Text="Print" OnClick="btnPrint_Click"></asp:LinkButton>
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
                                <h4 class="modal-title font-bold text-white">Request IT Parts
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
                                <h4 class="modal-title font-bold text-white">Request IT Parts
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

        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
            CancelControlID="btnNo" BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="100px" Width="400px" Style="display: none;">
            <table width="100%" style="border: Solid 2px #0271dd; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                <tr style="background-color: #0271dd">
                    <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">Confirm Box</td>
                    <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right">
                       
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                        <asp:Label ID="Label2" runat="server" Text="Do you want to delete this record ?" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right" style="padding-right: 15px">
                        <asp:ImageButton ID="btnYes" runat="server" ImageUrl="~/Images/btnyes.jpg" />
                        <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/Images/btnNo.jpg" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

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
                                <h4 class="modal-title" style="color: green; font-size: large">Request IT Parts Request - Review</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="form-horizontal">
                                    <div class="row" style="margin-bottom: 2px;">
                                        <div class="col-lg-3" align="left">
                                            <asp:Label ID="lblpopupheader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>
                                        </div>
                                        <div class="col-lg-9" align="right">
                                            <asp:Button ID="btnSave" Visible="false" runat="server" CssClass="btn btn-success" ValidationGroup="EmptyFieldSave" Text="Save" OnClick="btnSave_Click" />
                                            <asp:Button ID="btncancel" runat="server" CssClass="btn btn-primary" Text="Cancel" />
                                        </div>
                                    </div>
                                    <div id="DivMPRMasterNoreview" runat="server" style="display: none;">
                                        <span style="font-weight: 800;">Request IT PartsNo:- </span>
                                        <asp:Label ID="lblmprreview" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </div>
                                    <div class="well well-sm" style="padding: 5px 15px 15px 25px;">

                                        <div class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Corporate</span>
                                                    <asp:Label ID="lblCorp" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Facility:</span>
                                                    <asp:Label ID="lblFac" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Vendor:</span>
                                                    <asp:Label ID="lblVen" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Shipping: </span>
                                                    <asp:Label ID="lblship" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Request Type:</span>
                                                    <asp:Label ID="lblreqtype" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Request Date:</span>
                                                    <asp:Label ID="lblreqdate" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                           
                                        </div>
                                    </div>

                                    <asp:Label ID="lblpopupitems" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label>
                                    <div class="row">
                                        <div class="col-lg-12">
                                             <asp:Label ID="lblrcount3" runat="server">No of records : <%=grdreview.Rows.Count.ToString() %></asp:Label>
                                            <div class="SRReviewgrid">
                                                <asp:GridView ID="grdreview" runat="server" ShowHeaderWhenEmpty="true" ShowHeader="true" AutoGenerateColumns="false"
                                                    EmptyDataText="No Records Found" CssClass="table table-responsive" ShowFooter="true">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Equipment Sub Category">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblequrecat" runat="server" Text='<%# Eval("Equipcat") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Equipment List">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblequrelst" runat="server" Text='<%# Eval("Equiplst") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="SerialNo"/>
                                                        <asp:BoundField DataField="User" HeaderText="User" />
                                                         <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrevreason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrevqty" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Price per Qty ($)" HeaderStyle-Width="13%" ItemStyle-Width="13%">
                                                            <ItemTemplate>
                                                                <%--<div class="Currency-group">
                                                                    <span>$</span>--%>
                                                                    <asp:Label ID="lblrevppqty" runat="server" Text='<%# Eval("Priceperqty","$ {0:#,0.00}") %>'></asp:Label>
                                                               <%-- </div>--%>

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
                                                      
                                                        <asp:TemplateField HeaderText="Total Price ($)" HeaderStyle-Width="18%" ItemStyle-Width="18%">
                                                            <ItemTemplate>
                                                                <span>$</span>
                                                                <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalPrice", "{0:F2}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <span style="color: black">$</span>
                                                                <asp:TextBox ID="txtsipcost" Text="0" onblur="GetShipValue(this)" onchange="GetShipValue(this)" runat="server" ForeColor="Black"></asp:TextBox><br />
                                                                <ajax:FilteredTextBoxExtender ID="FTBtxtsipcost" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txtsipcost"></ajax:FilteredTextBoxExtender>
                                                                <span style="color: black">$</span>
                                                                <asp:TextBox ID="txttax" Text="0" onblur="GetTaxVal(this)" onchange="GetTaxVal(this)" runat="server" ForeColor="Black"></asp:TextBox>
                                                                <ajax:FilteredTextBoxExtender ID="Filteredtxttax" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="txttax"></ajax:FilteredTextBoxExtender>
                                                                <br />
                                                                <br />
                                                                <span style="color: black">$</span>
                                                                <asp:TextBox ID="txtTotalcost" runat="server" CssClass="ToatalcostCLS" ForeColor="Black" disabled="true" BackColor="White"></asp:TextBox>
                                                                <asp:TextBox ID="lblToatalcost" runat="server" CssClass="ToatalcostLabelCLS HeaderHide"></asp:TextBox>
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
                <div style="display: none">
                    <rsweb:ReportViewer ID="rvITRequestreport" runat="server"></rsweb:ReportViewer>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
