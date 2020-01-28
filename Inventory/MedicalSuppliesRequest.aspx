<%@ Page Title="Request-Medical Supplies" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="MedicalSuppliesRequest.aspx.cs" Inherits="Inventory.MedicalSuppliesRequest" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : MedicalSuppliesRequest.aspx 
' Type      : ASPX File 
' Description  :   To design the MedicalSuppliesRequest page for Add,Update and show the MedicalSuppliesRequest on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                                       Reason 
  09/14/2017           V.01              C.Dhanasekaran,S.Mahalakshmi,M.Murali         New
  10/25/2017           V.01              Vivekanand.S                               Locked the record.
  05/Mar/2018          V.01              Vivekanand.S                               Multi Search 
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

        var grdMedReqaddIndex = '';
        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;

            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }
        function SelectheaderCheckboxes(headerchk) {
            debugger
            var gvcheck = document.getElementById('<%= grdeditnewitemadd.ClientID %>');
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
                //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function GetgrdMedReqaddIndexValue(lynk) {
            var row = lynk.parentNode.parentNode;
            grdMedReqaddIndex = row.rowIndex - 1;
            document.getElementById('<%=HdnMSRDetailID.ClientID%>').value += (grdMedReqaddIndex) + ',';
        }
        $(function () {
            $(document).on("blur", "[id*=txtqihand]", function () {
                if (isNaN(parseInt($(this).val()))) {
                    //$(this).val('0');
                    var row = $(this).closest("tr");
                    if ($(this).val() == '') {
                        $("[id*=lbloqty]", row).html('');
                        $("[id*=lbltotprice]", row).html('');
                        // $("#dollorsymbol").hide();
                        var grandTotal = 0;
                        $("[id*=lbltotprice]").each(function () {
                            if (!isNaN(parseFloat($(this).html()))) {
                                grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                            }
                        });
                        $("[id*=lbladdfoottotal]").html(grandTotal.toFixed(2));
                    }
                } else {
                    $(this).val(parseInt($(this).val()).toString());
                }
            });
        });
        $(function () {
            $(document).on("change", "[id*=txtqihand]", function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        //alert($("[id*=lblUOM]", row).html());
                        if ($("[id*=lblUOM]", row).html() == 'Each') {
                            // if (parseFloat($(this).val()) == 'Each') {                                                       
                            var OrderQuanttity = parseFloat($("[id*=lblParlevel]", row).html()) - parseFloat($(this).val());
                            if (parseInt(OrderQuanttity) > 0) {
                                OrderQuanttity = Math.round(parseFloat(OrderQuanttity));
                                $("[id*=lbloqty]", row).html(OrderQuanttity.toString());
                            } else {
                                ShowwarningPopup("The Order Quantity should not be less than Zero");
                                $(this).val('');
                            }
                            var Subtotal = parseFloat($("[id*=lblPrice]", row).html()) * parseFloat($("[id*=lbloqty]", row).html());
                            $("[id*=lbltotprice]", row).html(Subtotal.toFixed(2));
                            //  $("#dollorsymbol").hide();

                        } else {
                            var row = $(this).closest("tr");
                            var Qty = parseInt($(this).val()) / parseInt($("[id*=lblQtyPack]", row).html());
                            var OrderquantityDecimal = parseInt($("[id*=lblParlevel]", row).html()) - Qty;
                            if (parseFloat(OrderquantityDecimal) > 0) {
                                OrderquantityDecimal = Math.round(parseFloat(OrderquantityDecimal));
                                $("[id*=lbloqty]", row).html(OrderquantityDecimal.toString());
                                if (OrderquantityDecimal.toString() == '0') {
                                    ShowwarningPopup("The Order Quantity should not be less than Zero");
                                    $(this).val('');
                                }
                            } else {
                                ShowwarningPopup("The Order Quantity should not be less than Zero");
                                $(this).val('');
                                //alert("The Order Quantity should not be less than Zero");
                            }
                            var Subtotal = parseFloat($("[id*=lblPrice]", row).html()) * parseFloat($("[id*=lbloqty]", row).html());
                            $("[id*=lbltotprice]", row).html(Subtotal.toFixed(2));
                            //$("#dollorsymbol").hide();
                        }
                    }
                }
                else {
                    $(this).val('');
                }

                var myHidden = document.getElementById('<%= HdnMSRDetailID.ClientID %>');
                var grandTotal = 0;
                $("[id*=lbltotprice]").each(function () {
                    if (!isNaN(parseFloat($(this).html()))) {
                        grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                    }
                });
                $("[id*=lbladdfoottotal]").html(grandTotal.toFixed(2));
            });
        });

        // Edit Item Grid 
        $(function () {
            $(document).on("blur", "[id*=txteqihand]", function () {
                if (isNaN(parseInt($(this).val()))) {
                    var row = $(this).closest("tr");
                    if ($(this).val() == '') {
                        $("[id*=lbleoqty]", row).html('');
                        $("[id*=lbltotprice]", row).html('');
                        var grandTotal = 0;
                        $("[id*=lbltotprice]").each(function () {
                            if (!isNaN(parseFloat($(this).html()))) {
                                grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                            }
                        });
                        $("[id*=lbleditfoottotal]").html(grandTotal.toFixed(2));
                    }
                } else {
                    $(this).val(parseInt($(this).val()).toString());
                }
            });
        });
        $(function () {
            $(document).on("change", "[id*=txteqihand]", function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        var row = $(this).closest("tr");
                        if ($("[id*=lbleUOM]", row).html() == 'Each') {
                            var OrderQuanttity = parseFloat($("[id*=lbleParlevel]", row).html()) - parseFloat($(this).val());
                            if (parseInt(OrderQuanttity) > 0) {
                                OrderQuanttity = Math.round(parseFloat(OrderQuanttity));
                                $("[id*=lbleoqty]", row).html(OrderQuanttity.toString());
                            } else {
                                ShowwarningPopup("The Order Quantity should not be less than Zero");
                                $(this).val('');
                            }
                            var Subtotal = parseFloat($("[id*=lblPrice]", row).html()) * parseFloat($("[id*=lbleoqty]", row).html());
                            $("[id*=lbltotprice]", row).html(Subtotal.toFixed(2));

                        } else {
                            var row = $(this).closest("tr");
                            var Qty = parseInt($(this).val()) / parseInt($("[id*=lbleQtyPack]", row).html());
                            var OrderquantityDecimal = parseInt($("[id*=lbleParlevel]", row).html()) - Qty;
                            if (parseFloat(OrderquantityDecimal) > 0) {
                                OrderquantityDecimal = Math.round(parseFloat(OrderquantityDecimal));
                                $("[id*=lbleoqty]", row).html(OrderquantityDecimal.toString());
                                if (OrderquantityDecimal.toString() == '0') {
                                    ShowwarningPopup("The Order Quantity should not be less than Zero");
                                    $(this).val('');
                                }
                            } else {
                                ShowwarningPopup("The Order Quantity should not be less than Zero");
                                $(this).val('');
                            }
                            var Subtotal = parseFloat($("[id*=lblPrice]", row).html()) * parseFloat($("[id*=lbleoqty]", row).html());
                            $("[id*=lbltotprice]", row).html(Subtotal.toFixed(2));

                        }
                    }
                }
                else {
                    $(this).val('');
                }
                document.getElementById('<%=HdnMSRDetailID.ClientID%>').value += (grdMedReqaddIndex) + ',';
                var myHidden = document.getElementById('<%= HdnMSRDetailID.ClientID %>');
                var grandTotal = 0;
                $("[id*=lbltotprice]").each(function () {
                    if (!isNaN(parseFloat($(this).html()))) {
                        grandTotal = parseFloat(grandTotal) + parseFloat($(this).html());
                    }

                });
                $("[id*=lbleditfoottotal]").html(grandTotal.toFixed(2));
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


        function CheckDateEalier(sender, args) {
            var toDate = new Date();
            var selectedDate = sender._selectedDate;
            toDate.setMinutes(0);
            toDate.setSeconds(0);
            toDate.setHours(0);
            toDate.setMilliseconds(0);

            selectedDate.setMinutes(0);
            selectedDate.setSeconds(0);
            selectedDate.setHours(0);
            selectedDate.setMilliseconds(0);
            if (selectedDate < toDate) {
                //alert("You cannot select a day earlier than today!");
                ShowwarningPopup("You cannot select a day earlier than today!");
                sender._selectedDate = toDate; //set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format));
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Medical Supplies Request
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
                </script>
                <%--Hidden fields--%>
                <asp:HiddenField ID="hdnMPRMasterID" runat="server" />
                <asp:HiddenField ID="HddMasterID" runat="server" />           
                <asp:HiddenField ID="HddUserID" runat="server" />
                <asp:HiddenField ID="HdnMSRDetailID" runat="server" />
                <asp:HiddenField ID="HddUpdateLockinEdit" runat="server" />
                <asp:HiddenField ID="HdnQueryStringID" runat="server" />
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
                      <asp:Label ID="lblrow" runat="server">No of records : <%=GrdMultiCorp.Rows.Count.ToString() %></asp:Label>
                    <div class="row" style="padding: 10px;">
                        <div class="col-lg-12 col-md-12 col-sm-12" style="overflow-y: scroll; height:200px;">
                            <asp:GridView ID="GrdMultiCorp" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="ChkAllCorp" runat="server"  AutoPostBack="true" OnCheckedChanged="ChkAllCorp_CheckedChanged" />
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

                <div id="DivMedicalRequest" runat="server" class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4" align="left">
                            <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-lg-8 col-md-8 col-sm-8" align="right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyFieldSearch" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                            <asp:Button ID="btnReview" runat="server" CssClass="btn btn-primary" Text="Review" OnClick="btnReview_Click" Visible="false" ValidationGroup="EmptyFieldAdd" />
                            <asp:Button ID="btnreviewprint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnreviewprint_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
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
                                        <%--<asp:DropDownList ID="drpcorsearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpcorsearch_SelectedIndexChanged"></asp:DropDownList>--%>
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
                                        <%--<asp:DropDownList ID="drpfacilitysearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpfacilitysearch_SelectedIndexChanged"></asp:DropDownList>--%>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Vendor</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendorsearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ListBox ID="drpvendorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                        <%--<asp:DropDownList ID="drpvendorsearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
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
                                            ControlToValidate="drpStatus" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ListBox ID="drpStatus" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                        <%--<asp:DropDownList ID="drpStatus" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                          <div id ="divcount" runat="server" style="margin-left: 14px; margin-top: 3px;">
                           <div class=" row" align="left">
                                 <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title"  Text="Search Result"></asp:Label>
                            </div>
                           <div class="row" align="left">
                                <asp:Label ID="lblrowcount" runat="server">No of records : <%=grdMedReqSearch.Rows.Count.ToString() %></asp:Label>
                           </div>
                        </div>
                        <div id="divgrdMSRSearch" runat="server" style="margin-left: 1px; margin-top: 3px;" class="divMedReqSearchGrid MSRSearchgrid">
                            <asp:GridView ID="grdMedReqSearch" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" OnRowDataBound="grdMedReqSearch_RowDataBound" EmptyDataText="No Records Found ">
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/edit.png" OnClick="imgbtnEdit_Click" />
                                            <asp:ImageButton ID="imgprint" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgprint_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MedicalSuppliesID" HeaderText="MedicalSuppliesID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="10%" />
                                    <asp:BoundField DataField="Corporate" HeaderText="Corp" HeaderStyle-Width="8%" ItemStyle-Width="8%"/>
                                    <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                    <asp:BoundField DataField="VendorShortName" HeaderText="Vendor"  HeaderStyle-Width="8%" ItemStyle-Width="8%"/>
                                    <asp:BoundField DataField="PRNo" HeaderText="PR No" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="19%" />
                                    <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="6%">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblaudit" runat="server" Text="Click here to read more"></asp:Label>--%>
                                            <asp:Image ID="imgreadmoreaudit" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="6%">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>--%>
                                            <asp:Image ID="imgreadmore" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px"  data-content='<%# Eval("Remarks")%>' data-toggle="popover" />
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
                            <asp:HiddenField ID="hdncheckfield" runat="server" Value="0" />
                        </div>
                    </div>

                    <div id="divMedReqAddHeader" runat="server" style="margin-top: 5px;">
                        <%-- <div class="row" style="display: none;" id="divpr" runat="server">
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <span class="page-header page-title">Header</span>
                            </div>
                        </div>--%>
                        <div id="divPRNo" runat="server" style="display: none;">
                            <asp:Label ID="spnprno" runat="server" Style="font-weight: 800;">Purchase Number :- </asp:Label>
                            <asp:Label ID="lblprno" runat="server" ForeColor="Red" CssClass="page-title lable-align" Text=""></asp:Label>
                        </div>
                        <div id="divMSRContentAddHeader" class="well well-sm" runat="server" style="display: none;">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqddlCorporateadd" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                            ControlToValidate="ddlCorporateadd" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="ddlCorporateadd" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCorporateadd_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Facility</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqddlFacilityadd" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                            ControlToValidate="ddlFacilityadd" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="ddlFacilityadd" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFacilityadd_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Vendor</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqddlVendoradd" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                            ControlToValidate="ddlVendoradd" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="ddlVendoradd" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlVendoradd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Order Type</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="Reqrdovendortype" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                            ControlToValidate="rdovendortype" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RadioButtonList ID="rdovendortype" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdovendortype_SelectedIndexChanged" AutoPostBack="true" CssClass="rbl">
                                            <asp:ListItem Text="Weekly" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Bi-Monthly" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Monthly" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Ad-Hoc" Value="4"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3" id="divorderperiod" runat="server">
                                    <div class="form-group">
                                        <span>Order Period</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqdrporderperiod" runat="server" ForeColor="Red"
                                            ControlToValidate="drporderperiod" ErrorMessage="This information is required." Visible="false" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="drporderperiod" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3" id="divOrderAdhoc" runat="server" style="display: none;">
                                    <div class="form-group">
                                        <span>Order Period</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="ReqdrporderperiodAdhoc" runat="server" ForeColor="Red"
                                            ControlToValidate="txtadhocorder" ErrorMessage="This information is required." Visible="false" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtadhocorder" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY"></asp:TextBox>
                                        <ajax:CalendarExtender ID="Ceadhoc" runat="server" TargetControlID="txtadhocorder" Enabled="True" OnClientDateSelectionChanged="CheckDateEalier"></ajax:CalendarExtender>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3" id="divshipping" runat="server" style="display: block;">
                                    <div class="form-group">
                                        <span>Shipping</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqdrpshipping" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                            ControlToValidate="drpshipping" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ImageButton ID="imgeshipadd" runat="server" Height="17px" ToolTip="Add Shipping" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Add.png" OnClick="imgeshipadd_Click" />
                                        <asp:ImageButton ID="imgeshipedit" runat="server" Height="17px" ToolTip="Edit Shipping" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/edit.png" OnClick="imgeshipedit_Click" />
                                        <asp:ImageButton ID="imgeshipdelete" runat="server" Height="17px" ToolTip="Delete Shipping" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClick="imgeshipdelete_Click" />
                                        <asp:DropDownList ID="drpshipping" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpshipping_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3" id="divtimedelivery" runat="server" style="display: none;">
                                    <div class="form-group">
                                        <span>Time Delivery</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqdrptimedelivery" runat="server" ForeColor="Red"
                                            ControlToValidate="drptimedelivery" ErrorMessage="This information is required." Visible="false" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ImageButton ID="imgtimedeladd" runat="server" Height="17px" ToolTip="Add TimeDelivery" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Add.png" OnClick="imgtimedeladd_Click" />
                                        <asp:ImageButton ID="imgtimedeledit" runat="server" Height="17px" ToolTip="Edit TimeDelivery" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/edit.png" OnClick="imgtimedeledit_Click" />
                                        <asp:ImageButton ID="imgtimedldel" runat="server" Height="17px" ToolTip="Delete TimeDelivery" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClick="imgtimedldel_Click" />
                                        <asp:DropDownList ID="drptimedelivery" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="divContentgrdAdd" runat="server" style="display: none;">
                            <asp:Label ID="lblItemHeader" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label><br />
                             <asp:Label ID="lblcount" runat="server">No of records : <%=grdMedReqadd.Rows.Count.ToString() %></asp:Label>
                            <div class="MSRAddgrid">
                                <asp:GridView ID="grdMedReqadd" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ItemID" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemId" Text='<%#Eval("ItemID") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Item Id" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblvendorItemId" Text='<%#Eval("VendorItemID") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Group" HeaderStyle-Width="9%" ItemStyle-Width="9%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryName" Text='<%#Eval("CategoryName") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemDescription" Text='<%#Eval("ItemDescription") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" Text='<%#Eval("UOM") %>' runat="server"></asp:Label>
                                                <asp:Label ID="lbluomvalue" Text='<%#Eval("UomID") %>' runat="server" Style="display: none;"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty Pack" HeaderStyle-Width="4%" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyPack" Text='<%#Eval("QtyPack") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price($)" HeaderStyle-Width="9%" ItemStyle-Width="9%">
                                            <ItemTemplate>
                                                <span id="dolloradditemprice">$</span>
                                                <asp:Label ID="lblPrice" Text='<%#Eval("UnitPriceValue","{0:F2}") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Par level (UOM)" HeaderStyle-Width="4%" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParlevel" Text='<%#Eval("Parlevel") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty In Hand(Each)" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtqihand" runat="server" Width="90%" onkeyup="GetgrdMedReqaddIndexValue(this)" onblur="GetgrdMedReqaddIndexValue(this)" onchange="GetgrdMedReqaddIndexValue(this)"></asp:TextBox>
                                                <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtqihand" BehaviorID=""></ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Qty" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbloqty" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total($)" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <span id="dolloradditemtotalprice">$</span>
                                                <asp:Label ID="lbltotprice" runat="server"></asp:Label>
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
                            <div class="row" align="right">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <span style="font-weight: 800;">Grand Total =</span>&nbsp&nbsp
                                    <span id="dolloraddtotalprice" style="font-weight: 800;">$</span>
                                    <asp:Label ID="lbladdfoottotal" runat="server" Text="0" Style="font-weight: 800;"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div id="divContentgrdEdit" runat="server" style="display: none;">
                            <asp:Button ID="btnEdititemAdd" runat="server" Text="Add New Item" OnClick="btnEdititemAdd_Click" CssClass="btn btn-primary" /><br />
                              <asp:Label ID="lbcount" runat="server">No of records : <%=grdmedsupitemedit.Rows.Count.ToString() %></asp:Label>
                            <div class="MSREditgrid">
                                <asp:GridView ID="grdmedsupitemedit" runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" AutoGenerateColumns="false" CssClass="table table-responsive">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lbdelete" runat="server" Text="Delete" Height="20px" ImageUrl="~/Images/Delete.png" OnClick="lbdelete_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MedicalsuppliesItemID" HeaderText="MedicalsuppliesItemID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:TemplateField HeaderText="ItemID" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleItemId" Text='<%#Eval("ItemID") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Item Id" HeaderStyle-Width="7%" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblevendorItemId" Text='<%#Eval("VendorItemID") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Group" HeaderStyle-Width="9%" ItemStyle-Width="9%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleCategoryName" Text='<%#Eval("CategoryName") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleItemDescription" Text='<%#Eval("ItemDescription") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleUOM" Text='<%#Eval("UOM") %>' runat="server"></asp:Label>
                                                <asp:Label ID="lbluomvalue" Text='<%#Eval("UomID") %>' runat="server" Style="display: none;"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty/ Pack" HeaderStyle-Width="4%" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleQtyPack" Text='<%#Eval("QtyPack") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price($)" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <span id="dolloredititemprice">$</span>
                                                <asp:Label ID="lblPrice" Text='<%#Eval("Price","{0:F2}") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Par level (UOM)" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleParlevel" Text='<%#Eval("Parlevel") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty In Hand(Each)" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txteqihand" Width="90%" runat="server" Text='<%#Eval("QtyInHand") %>' onkeyup="GetgrdMedReqaddIndexValue(this)" onblur="GetgrdMedReqaddIndexValue(this)" onchange="GetgrdMedReqaddIndexValue(this)"></asp:TextBox>
                                                <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txteqihand" BehaviorID=""></ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Qty" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbleoqty" runat="server" Text='<%#Eval("OrderQty") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RowNumber" HeaderText="RowNumber" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" NullDisplayText=" " />
                                        <asp:BoundField DataField="ItemID" HeaderText="ItemKey" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:TemplateField HeaderText="Total($)" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <span id="dolloredititemtotal">$</span>
                                                <asp:Label ID="lbltotprice" runat="server" Text='<%#Eval("TotalPrice","{0:F2}") %>'></asp:Label>
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
                            <div class="row" align="right">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <span style="font-weight: 800;">Grand Total =</span>&nbsp&nbsp
                                    <span id="dolloredittotalprice" style="font-weight: 800;">$</span>
                                    <asp:Label ID="lbleditfoottotal" runat="server" Text="" Style="font-weight: 800;"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div id="modalSave" class="modal fade" style="position: center">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content ">
                            <div class="modal-header bg-green">
                                <h4 class="modal-title font-bold text-white">Medical Supplies Request
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="lblsave" runat="server"></asp:Label><asp:LinkButton ID="lbpopprint" runat="server" Text="Print" OnClick="btnreviewprint_Click" Visible="false"></asp:LinkButton>
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
                                <h4 class="modal-title font-bold text-white">Medical Supplies Request
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
                                <h4 class="modal-title font-bold text-white">Medical Supplies Request
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
                                <p>Do you want to delete this record <span id="spnreName"></span>?.</p>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="removeyes" runat="server" OnClick="removeyes_Click" Text="Yes" CssClass="btn btn-danger" />
                                <asp:Button ID="removeno" runat="server" OnClick="removeno_Click" Text="Close" class="btn btn-default ra-100" />
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
        <asp:UpdatePanel ID="upnlship" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnship" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeshipping" runat="server"
                    PopupControlID="modalAddship" TargetControlID="btnship"
                    BackgroundCssClass="modalBackground" BehaviorID="modalAddequip" CancelControlID="btnpopupclose">
                </ajax:ModalPopupExtender>
                <div id="modalAddship" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="btnpopupclose" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: green; font-size: large">Shipping</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Panel runat="server" ID="Panel1" DefaultButton="btnaddshipping">
                                    <div style="height: 40px">
                                        <div class="form-horizontal">
                                            <div class="col-md-6 col-sm-6">
                                                <span>Shipping</span>
                                                <asp:TextBox ID="txtshipping" CssClass="form-control" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtshipping" runat="server" ControlToValidate="txtshipping" ValidationGroup="EmptyFieldshipping"
                                                    ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnaddshipping" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="EmptyFieldshipping" OnClick="btnaddshipping_Click" />
                                <asp:Button ID="btnshipclose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnshipclose_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnaddshipping" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upnltimedelivery" runat="server">
            <ContentTemplate>
                <asp:Button ID="btntimedel" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpetimedel" runat="server"
                    PopupControlID="modalAddtimedel" TargetControlID="btntimedel"
                    BackgroundCssClass="modalBackground" BehaviorID="modalAddtimedel" CancelControlID="btnpoptimeclose">
                </ajax:ModalPopupExtender>
                <div id="modalAddtimedel" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="btnpoptimeclose" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: green; font-size: large">Time Delivery</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Panel runat="server" ID="pnltimedel" DefaultButton="btntimedelsave">
                                    <div style="height: 40px">
                                        <div class="form-horizontal">
                                            <div class="col-md-6 col-sm-6">
                                                <span>Time Delivery</span>
                                                <asp:TextBox ID="txttimedel" CssClass="form-control" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxttimedel" runat="server" ControlToValidate="txttimedel" ValidationGroup="EmptyFieldtimedeli"
                                                    ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btntimedelsave" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="EmptyFieldtimedeli" OnClick="btntimedelsave_Click" />
                                <asp:Button ID="btntimedelclose" runat="server" Text="Close" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btntimedelsave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upnAddNewItem" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnaddnewitem" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeeditnewitem" runat="server"
                    PopupControlID="modaleditaddnewitem" TargetControlID="btnaddnewitem"
                    BackgroundCssClass="modalBackground" BehaviorID="modaleditaddnewitem">
                </ajax:ModalPopupExtender>
                <div id="modaleditaddnewitem" style="display: none;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="btneditnewitemclose" class="close" runat="server" Text="X" OnClick="btneditnewitemclose_Click" />
                                <h4 class="modal-title" style="color: green; font-size: large">New Item</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px 15px 15px 15px;">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="col-lg-4">
                                        <asp:Label ID="lcount" runat="server">No of records : <%=grdeditnewitemadd.Rows.Count.ToString() %></asp:Label>
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:Label ID="lblerrormsgselectitem" runat="server" Text="Please Select at Least One Item Record" Style="display: none" ForeColor="Red"></asp:Label>
                                        </div>
                                        <div class="col-lg-2" align="right">
                                            <asp:Button ID="addnewItem" runat="server" Text="Add To Grid" OnClick="addnewItem_Click" CssClass="btn btn-primary" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 MSRNewItempopupgrid" style="margin-top: 2px;">
                                            <asp:GridView ID="grdeditnewitemadd" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select Item" HeaderStyle-Width="6%">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="ckboxSelectAllNewItem" runat="server" onclick="SelectheaderCheckboxes(this);"
                                                                Style="font-size: 14px; line-height: 1.42857143; font-weight: bold; color: #ffffff;" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ckboxselectitem" runat="server" onclick="Check_Click(this);" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="MedicalSupplyItemID" HeaderText="MedicalsuppliesItemID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <asp:BoundField DataField="ItemID" HeaderText="ItemID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <asp:BoundField DataField="VendorItemID" HeaderText="Vendor Item Id" HeaderStyle-Width="14%" />
                                                    <asp:BoundField DataField="CategoryName" HeaderText="CategoryName" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                                    <asp:BoundField DataField="UomID" HeaderText="UomID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                    <asp:BoundField DataField="QtyPack" HeaderText="QtyPack" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <asp:BoundField DataField="Price" HeaderText="Price" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <asp:BoundField DataField="Parlevel" HeaderText="Parlevel" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <asp:BoundField DataField="QtyInHand" HeaderText="QtyInHand" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" NullDisplayText=" " />
                                                    <asp:BoundField DataField="OrderQty" HeaderText="OrderQty" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <asp:BoundField DataField="RowNumber" HeaderText="RowNumber" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                    <%--<asp:BoundField DataField="Price" HeaderText="TotalPrice"/>--%>
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnconfirm" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="mpeconfirm"
            runat="server" TargetControlID="btnconfirm" PopupControlID="pnlconfirm"
            BackgroundCssClass="modalBackground" Enabled="True">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlconfirm" runat="server" BackColor="White" CssClass="panel"
            Style="display: none;">
            <table style="border: Solid 2px #f0ad4e; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                <tr style="background-color: #f0ad4e">
                    <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">Confirm </td>
                    <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                        <asp:Label ID="Label7" runat="server" Text="Do you want to Delete this Record?" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
            </table>
        </asp:Panel>
        <asp:HiddenField ID="hdnupdate" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnmedreview" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpemedsupplyReview" runat="server"
                    PopupControlID="mpemedsupreview" TargetControlID="btnmedreview"
                    BackgroundCssClass="modalBackground" BehaviorID="mpemedsupplyReview" CancelControlID="btnrevclose">
                </ajax:ModalPopupExtender>
                <div id="mpemedsupreview" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="btnrevclose" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: black; font-size: large">Medical Supplies Review</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px 15px 15px 15px;">
                                <div class="form-horizontal">
                                    <div class="row" style="margin-bottom: 2px;">
                                        <div class="col-sm-6 col-md-6 col-lg-6">
                                            <span class="page-header page-title">Header</span>
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-6" align="right">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click" Visible="false" />
                                            <asp:Button ID="btnreviewcancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnreviewcancel_Click" />
                                        </div>
                                    </div>
                                    <div runat="server" id="diveviewprno">
                                        <span style="font-weight: 800;">Purchase Number :- </span>
                                        <asp:Label ID="lblrwprno" runat="server" ForeColor="Red" CssClass="page-title lable-align" Text=""></asp:Label>
                                    </div>
                                    <div class="well well-sm">
                                        <div class="row">
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <span style="font-size: 14px; font-weight: bold;">Corporate</span>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group" style="font-weight: 600;">
                                                    <asp:Label ID="lblreviewcorporate" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <span style="font-size: 14px; font-weight: bold;">Facility</span>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group" style="font-weight: 600;">
                                                    <asp:Label ID="lblreviewfacility" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <span style="font-size: 14px; font-weight: bold;">Vendor</span>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group" style="font-weight: 600;">
                                                    <asp:Label ID="lblvendor" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <span style="font-size: 14px; font-weight: bold;">Order Period</span>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group" style="font-weight: 600;">
                                                    <asp:Label ID="lblorderperiod" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <span style="font-size: 14px; font-weight: bold;">Shipping</span>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group" style="font-weight: 600;">
                                                    <asp:Label ID="lblshipping" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <span style="font-size: 14px; font-weight: bold;">Order Type</span>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group" style="font-weight: 600;">
                                                    <asp:Label ID="lblordertype" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <span style="font-size: 14px; font-weight: bold;">Time Delivery</span>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group" style="font-weight: 600;">
                                                    <asp:Label ID="lbltimedelivery" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                      <asp:Label ID="lheader" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label>
                                    <div class="row">
                                      <div class="col-lg-12">
                                             <asp:Label ID="lblrcount" runat="server">No of records : <%=gvmedreview.Rows.Count.ToString() %></asp:Label>
                                        <div class="MSRReviewgrid">
                                            <asp:GridView ID="gvmedreview" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                                <Columns>
                                                    <asp:BoundField DataField="ItemID" HeaderText="ItemID" HeaderStyle-Width="5%" />
                                                    <asp:BoundField DataField="VendorItemID" HeaderText="Vendor Item Id" HeaderStyle-Width="8%" />
                                                    <asp:BoundField DataField="CategoryName" HeaderText="Item Group" HeaderStyle-Width="8%" />
                                                    <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                                    <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-Width="4%" />
                                                    <asp:BoundField DataField="QtyPack" HeaderText="Qty/ Pack" HeaderStyle-Width="5%" />
                                                    <asp:BoundField DataField="Price" HeaderText="Price($)" DataFormatString="$ {0:#,0.00}" HeaderStyle-Width="9%" />
                                                    <asp:BoundField DataField="Parlevel" HeaderText="Par level (UOM)" HeaderStyle-Width="5%" />
                                                    <asp:BoundField DataField="QtyInHand" HeaderText="Qty In Hand (Each)" HeaderStyle-Width="5%" />
                                                    <asp:BoundField DataField="OrderQty" HeaderText="Order Qty" HeaderStyle-Width="5%" />
                                                    <asp:BoundField DataField="TotalPrice" HeaderText="TotalPrice($)" DataFormatString="$ {0:#,0.00}" HeaderStyle-Width="10%" />
                                                    <%-- <asp:BoundField DataField="" HeaderText="Total" />--%>
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
                                    <div class="row" align="right">
                                        <div class="col-sm-12 col-md-12 col-lg-12">
                                            <span style="font-weight: 800;">Grand Total</span>&nbsp 
                                             <span id="dollorreviewtotalprice" style="font-weight: 800;">$</span>
                                            <asp:Label ID="lblrwgrandtotal" runat="server" Text="0" Style="font-weight: 800;"></asp:Label>
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
        <div style="display: none">
            <rsweb:ReportViewer ID="rvmedicalsupplyreport" runat="server"></rsweb:ReportViewer>
        </div>
</asp:Content>
