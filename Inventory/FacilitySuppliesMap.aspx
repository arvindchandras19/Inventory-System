<%@ Page Title="Facility Supplies Map" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="FacilitySuppliesMap.aspx.cs" Inherits="Inventory.FacilitySuppliesMap" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : FacilitySuppliesMap.aspx 
' Type      : ASPX File 
' Description  :   To design the Facility Medical Supply page for add,Update and show the Facility Supply Map on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01              Vivekanand.S                     New
  10/24/2017           V.02              Sairam.P                     Model Popup added for Notifications
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

        .Upopacity {
            opacity: 0.3;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            -o-background-size: cover;
            background-size: cover;
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

        .radiostyle input[type="radio"] {
            margin-left: 10px;
            margin-right: 1px;
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

        /*.radiostyle input[type="radio"] {
            margin-left: 10px;
            margin-right: 1px;
        }*/
    </style>

    <script type="text/javascript">

        function CorpDrop() {
            $('[id*=drpvendorcodeSearch]').change(function (event) {

                if ($(this).val().length > 1) {
                    var val = $(this).val() || [];
                    alert('Multiple selection are not allowed here. Use Multi select link for multiple selection.');
                    var $this = $(this);

                } else {
                    last_valid_selection = $(this).val();
                }
            });
        }


        function Validate() {
            var RB1 = document.getElementById("<%=rbCensus.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            var isChecked = false;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    isChecked = true;
                    break;
                }
            }
            if (!isChecked) {

            }

            return isChecked;
        }

        $(function () {
            $(document).on("keyup mouseup", "[id*=grdFacilitySupply] [id*=txtCensus]", function () {
                var row = $(this).closest("tr");
                var txtreason = $(this);
                if (txtreason != '') {
                    txtreason.css({ "border": "Solid 1px #a9a9a9" })
                } else {
                    txtreason.css({ "border": "Solid 1px red" })
                }

            });



        });


        function ValidateCensus() {
          
           
            var IsValid = false;
            if (Page_ClientValidate()) {
                IsValid = true;
            } else {
                IsValid = false;
            }

            $("[id*=grdFacilitySupply] tbody").find('tr').each(function (i, el) {

                var td = $(this).find("td");
                var txtCensus = td.find("[id*=txtCensus]");

                if (txtCensus.val() != '') {
                    txtCensus.css({ "border": "Solid 1px #a9a9a9" })
                    IsValid = true;
                } else {
                    txtCensus.css({ "border": "Solid 1px red" })
                }
                if (txtCensus.val() == "") {
                    ShowwarningLookupPopup("Census Value in Grid Not Entered");
                    IsValid = false;
                } else {
                    if (Page_ClientValidate()) {
                        IsValid = true;
                    } else {
                        IsValid = false;
                    }
                }
            });

            return IsValid;
        }

        function loadcensushide() {
            $(document).ready(function () {
                //var isClicked = $("[id*=HdnLoad]").val();
                //if (isClicked == '1') {
                    $("[id*=grdFacilitySupply] tbody tr").each(function () {
                        $(this).find(".clsparlevel").text('');
                    });

                    $("[id*=grdFacilitySupply]").find("input[type=text][id*=clsparlevel]").val("");
                //}
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
        function ShowwarningLookupPopup(res) {
            //alert('ggg');
            $('[id*=lblwarning]').html(res);

            $("#modalWarning").modal("show");

        }

        function editfacility() {
            $('[id*=btnSubmit]').show();
        }

        function addfacility() {
            $('[id*=btnSubmit]').show();
        }

    </script>

    <script type="text/javascript">
        function jScript() {

            $('[id*=drpvendorcodeSearch]').SumoSelect({
                selectAll: false,
                placeholder: 'Select Vendor'
            });

            $('[id*=drpItemcategorySearch]').SumoSelect({
                selectAll: true,
                placeholder: 'Select Status'
            });

           
            //$('[id*=rbCensus]').click(function () {
            //    //alert($('[id*=rbCensus] input:checked').val());
            //    if ($('[id*=rbCensus] input:checked').val() == '4') {
            //        $('[id*=txtotherCensus]').val("");
            //        $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val("");
            //        $('[id*=txtotherCensus]').removeAttr("disabled");
            //        $('[id*=txtotherCensus]').attr("enabled", "enabled");
            //    } else {
            //        $('[id*=txtotherCensus]').val("");
            //        $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val("");
            //        $('[id*=txtotherCensus]').removeAttr("enabled");
            //        $('[id*=txtotherCensus]').attr("disabled", "disabled");
            //        if ($('[id*=rbCensus] input:checked').val() == '1') {
            //            $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val($('[id*=HddEmployeeCensus]').val());
            //        } else if ($('[id*=rbCensus] input:checked').val() == '2') {
            //            $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val($('[id*=HddPatientCensus]').val());
            //        } else if ($('[id*=rbCensus] input:checked').val() == '3') {
            //            $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val($('[id*=HddBothCensus]').val());
            //        }
            //    }
            //});

        }

         function LoadCensusChange() {
            $('#<%=rbCensus.ClientID %>').click(function () {

                if ($('[id*=rbCensus] input:checked').val() == '4') {

                    $('[id*=txtotherCensus]').attr("disabled", false);
                    //$("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val($('[id*=txtotherCensus]').val());
                } else {
                    $('[id*=txtotherCensus]').attr("disabled", true);
                }
                $('[id*=txtotherCensus]').val('');
                $("[id*=HdnLoad]").val('');
            });
        }


        function Populate() {
           
      
            $("[id*=HdnLoad]").val('1');
            $("[id*=grdFacilitySupply] tbody tr").each(function () {
                $(this).find(".clsparlevel").text('');
            });


            $("[id*=grdFacilitySupply]").find("input[type=text][id*=clsparlevel]").val("");


            if ($('[id*=rbCensus] input:checked').val() == '4') {
                $("[id*=grdFacilitySupply] tbody tr").each(function () {
                    $(this).find(".txtCensus").val($("[id*=txtotherCensus]").val());
                });

            } else {
                $('[id*=txtotherCensus]').val("");
                $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val("");
                if ($('[id*=rbCensus] input:checked').val() == '1') {
                    $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val($('[id*=HddEmployeeCensus]').val());
                } else if ($('[id*=rbCensus] input:checked').val() == '2') {
                    $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val($('[id*=HddPatientCensus]').val());
                } else if ($('[id*=rbCensus] input:checked').val() == '3') {
                    $("[id*=grdFacilitySupply]").find("input[type=text][id*=txtCensus]").val($('[id*=HddBothCensus]').val());
                }
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Facility Supplies Map
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(jScript);
                    Sys.Application.add_load(CorpDrop); 
                    Sys.Application.add_load(LoadCensusChange);
                    
                </script>
                <asp:HiddenField ID="hdnEdit" runat="server" />
                <asp:HiddenField ID="hdnFacilitySupplyID" runat="server" />
                <asp:HiddenField ID="HddCuurentStatus" runat="server" Value="Search" />
                <asp:HiddenField ID="HddEmployeeCensus" runat="server" Value="" />
                <asp:HiddenField ID="HddPatientCensus" runat="server" Value="" />
                <asp:HiddenField ID="HddBothCensus" runat="server" Value="" />
                <asp:HiddenField ID="HddListVendorID" runat="server" Value="" />
                <asp:HiddenField ID="Hdncheckcensus" runat="server" Value="" />
                    <asp:HiddenField ID="HdnLoad" runat="server" Value="0" />

                <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="DivMultiVendor">
                    <%--<asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnuploadfile" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                    <asp:Label ID="lblMultiVendor" runat="server" CssClass="page-header page-title" Text="Select Multiple Vendor"></asp:Label><br />
                    <asp:Label ID="lbrow" runat="server">No of records : <%=GrdMultiVendor.Rows.Count.ToString() %></asp:Label>
                    <div class="row" style="padding: 10px;">
                        <div class="col-lg-12 col-md-12 col-sm-12" style="overflow-y: scroll; height: 200px;">
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


                <div class="mypanel-body" id="UploadOpacity" runat="server" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-12" align="right">
                            <asp:Button ID="btnsearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyFieldSearch" OnClick="btnsearch_Click" />
                            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />
                            <asp:Button ID="btnSave" runat="server" Visible="false" CssClass="btn btn-success" Text="Review" ValidationGroup="AddEmptyField" OnClientClick="return ValidateCensus()" OnClick="btnSave_Click" />
                            <asp:Button ID="btnprint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnprint_Click" />
                            <asp:Button ID="btnclear" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnclear_Click" />
                        </div>
                    </div>
                    <div id="DivSearch" runat="server" style="margin-top: 5px;">
                        <div id="divFacilitySuppMapSearch" class="well well-sm">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:DropDownList ID="drpcopSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpcopSearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcor" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                        <asp:DropDownList ID="drpfacilitySearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpfacility_SelectedIndexChanged"></asp:DropDownList>
                                        <%-- <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpfacilitySearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacility" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                    </div>

                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiVendor" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiVendor_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearVendor" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearVendor_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllVendor" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllVendor_Click"></asp:LinkButton>
                                        <asp:ListBox ID="drpvendorcodeSearch" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="drpvendorcodeSearch_SelectedIndexChanged"></asp:ListBox>
                                        <%--<asp:DropDownList ID="drpvendorSearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpvendor_SelectedIndexChanged"></asp:DropDownList>--%>
                                        <%--<asp:RequiredFieldValidator InitialValue="" ID="ReqfielddrpvendorSearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendorcodeSearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Item Category</span>&nbsp;<span style="color: red">*</span>
                                        <asp:ListBox ID="drpItemcategorySearch" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                                        <%--<asp:DropDownList ID="drpItemcategorySearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                        <%--<asp:DropDownList ID="drpItemcategorySearch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpItemcategory_SelectedIndexChanged"></asp:DropDownList>--%>
                                        <%-- <asp:RequiredFieldValidator InitialValue="" ID="ReqfielddrpItemcategorySearch" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpItemcategorySearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                 <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Parlevel</span>&nbsp;<span style="color: red">*</span>
                                        <asp:DropDownList ID="drpParlevel" runat="server" CssClass="form-control" AutoPostBack="true">
                                            <asp:ListItem Text="--Select Parlevel--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Parlevel > 0" Value="1"  Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Parlevel <= 0" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Both" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Requireddrporderdate" ValidationGroup="EmptyFieldSearch" runat="server" ForeColor="Red"
                                            ControlToValidate="drpParlevel" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row vendororderduegrid" runat="server" style="margin-left: 1px; margin-top: 3px; display: none" id="DivSearchGrid">
                            <span style="font-weight: bold">Hint: Parlevel = (Factor x census[emp/patient] x shipping[weekly/bi-weekly/monthly]x Tx per week) / Qty per pack of the item</span>
                            <div>
                                <asp:Label ID="lblrowcount" runat="server">No of records : <%=grdFacilitySupplySearch.Rows.Count.ToString() %></asp:Label>
                            </div>
                            <asp:GridView ID="grdFacilitySupplySearch" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive">
                                <Columns>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="3%" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgbtnEdit" runat="server" Text="Edit" Height="20px" ToolTip="Edit" ImageUrl="~/Images/edit.png" OnClick="ImgbtnEdit_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FacilitySupplyID" HeaderText="FacilitySupplyID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="CategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="VendorShortName" HeaderText="Vendor Code" HeaderStyle-Width="100px" />
                                    <asp:BoundField DataField="CategoryName" HeaderText="Item Group" HeaderStyle-Width="100px" />
                                    <asp:BoundField DataField="VendorItemCode" HeaderText="Vendor Item Code" HeaderStyle-Width="100px" />
                                    <asp:BoundField DataField="ItemID" HeaderText="Item Code" HeaderStyle-Width="60px" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" HeaderStyle-Width="100px" DataFormatString="{0:F}" />
                                    <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-Width="60px" />
                                    <asp:BoundField DataField="QtyPack" HeaderText="Qty Pack" HeaderStyle-Width="60px" />
                                    <asp:BoundField DataField="Factor" HeaderText="Factor" HeaderStyle-Width="60px" />
                                    <asp:BoundField DataField="Census" HeaderText="Census" HeaderStyle-Width="60px" />
                                    <asp:BoundField DataField="Parlevel" HeaderText="Parlevel" HeaderStyle-Width="80px" />
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
                    <div id="DivAdd" runat="server" style="margin-top: 5px; display: none">
                        <div id="DivContent" class="well well-sm">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:DropDownList ID="drpcor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqfielddrpcorp" ValidationGroup="AddEmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcor" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                        <asp:DropDownList ID="drpfacility" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpfacility_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqfielddrpfacility" ValidationGroup="AddEmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacility" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>
                                        <asp:DropDownList ID="drpvendor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpvendor_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqfielddrpvendor" ValidationGroup="AddEmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendor" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Item Category</span>&nbsp;<span style="color: red">*</span>
                                        <asp:DropDownList ID="drpItemcategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpItemcategory_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpItemcategory" ValidationGroup="AddEmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpItemcategory" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div id="DivFactor" runat="server" class="col-sm-4 col-md-4 col-lg-4">
                                    <%--<div class="col-sm-7 col-md-7 col-lg-7 form-group" style="padding-left: 0px; padding-right: 0px;">
                                        <span style="font-weight: 800;">Factor</span>&nbsp;<span style="color: red">*</span>
                                        <asp:TextBox ID="txtFactor" runat="server" CssClass="form-control"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="filterFactortext" runat="server" FilterType="Numbers,Custom" TargetControlID="txtFactor" ValidChars="."></ajax:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="reqfieldtxtFactor" runat="server" ControlToValidate="txtFactor" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>--%>
                                    <div id="DivCensus" class="col-sm-7 col-md-7 col-lg-7 form-group" style="padding-right: 0px;">
                                        <span style="font-weight: 800;">Census</span>&nbsp;<span style="color: red;">*</span>
                                        <asp:RadioButtonList ID="rbCensus" runat="Server" RepeatDirection="Horizontal" CssClass="radiostyle">
                                            <asp:ListItem Text="Emp" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Patient" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Both" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Other" Value="4"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator ID="RequiredrbCensus" runat="server" ControlToValidate="rbCensus" ValidationGroup="AddEmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 form-group" style="padding-top: 15px" align="left">
                                        <asp:TextBox ID="txtotherCensus" runat="server" CssClass="form-control" Width="50px" Enabled="false"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="FilterOtherCensus" FilterType="Numbers,Custom" runat="server" TargetControlID="txtotherCensus"></ajax:FilteredTextBoxExtender>
                                    </div>
                                    <div class="col-sm-2 col-md-2 col-lg-2 form-group" style="padding-top: 15px" align="left">
                                        <input type="button" value="Load grid" class="btn btn-primary" id="btnSubmit" onclick="Populate();" />
                                    </div>
                                </div>
                                <%--<div class="col-sm-2 col-md-2 col-lg-2 form-group">
                                    <br />
                                        <input type="button" value="Submit" class="btn btn-primary" id="btnSubmit" onclick="Populate();" />
                                    </div>--%>
                                <%-- <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Select Order Date</span>&nbsp;<span style="color: red">*</span>
                                        <asp:DropDownList ID="drporderdate" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drporderdate_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Requireddrporderdate" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drporderdate" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>--%>
                            </div>
                        </div>
                        <div class="row" runat="server" style="margin-left: 1px; margin-top: 3px;" id="divvendor">
                            <span style="font-weight: bold">Hint: Parlevel = (Factor x census[emp/patient] x shipping[weekly/bi-weekly/monthly]x Tx per week) / Qty per pack of the item</span>
                            <div>
                                   <asp:Label ID="Lblgfsupply" runat="server">No of records : <%=grdFacilitySupply.Rows.Count.ToString() %></asp:Label>
                            </div>
                            <div class="vendororderduegrid">
                                <asp:GridView ID="grdFacilitySupply" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive">
                                    <Columns>
                                        <asp:BoundField DataField="FacilitySupplyID" HeaderText="FacilitySupplyID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="FacilityShortName" HeaderText="Facility Code" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="VendorShortName" HeaderText="Vendor Code" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="CategoryName" HeaderText="Item Group" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="VendorItemCode" HeaderText="Vendor Item Code" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="ItemID" HeaderText="Item Code" HeaderStyle-Width="60px" />
                                        <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" HeaderStyle-Width="100px" DataFormatString="{0:F}" />
                                        <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-Width="60px" />
                                        <asp:BoundField DataField="QtyPack" HeaderText="Qty Pack" HeaderStyle-Width="60px" />
                                        <asp:TemplateField HeaderText="Factor" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFactor" runat="server" Width="50px" Text='<%# Eval("Factor") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Census" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCensus" runat="server" CssClass="txtCensus" Width="50px" Text='<%# Eval("Census") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VendorOrderDate" HeaderText="Vendor Order Date" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                         <asp:BoundField DataField="Parlevel" ItemStyle-CssClass="clsparlevel" HeaderText="Parlevel" HeaderStyle-Width="80px" />
                                        <%-- <asp:TemplateField HeaderText="Parlevel">
                                             <ItemTemplate>
                                                
                                                 <asp:Label ID ="parlevelcal" runat="server" Text='<%# Eval("Parlevel") %>'></asp:Label> 
                                             </ItemTemplate>
                                        </asp:TemplateField>--%>
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
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">Facility Supplies Map
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
                        <h4 class="modal-title font-bold text-white">Facility Supplies Map
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
                        <h4 class="modal-title font-bold text-white">Facility Supplies Map
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


        <asp:UpdatePanel ID="UpdatFacilitySupply" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnFacilSupplreview" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeFacilSupplyReview" runat="server"
                    PopupControlID="mpeFacilSupplreview" TargetControlID="btnFacilSupplreview"
                    BackgroundCssClass="modalBackground" BehaviorID="mpeFacilSupplyReview">
                </ajax:ModalPopupExtender>

                <div id="mpeFacilSupplreview" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" runat="server" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="btnrevclose" runat="server" CssClass="close" Text="X" OnClick="btnrevclose_Click"/>
                                <h4 class="modal-title" style="color: black; font-size: large">Facility Supplies Map Review</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px 15px 15px 15px;">
                                <div class="form-horizontal">
                                    <div class="row" style="margin-bottom: 2px;">
                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                            <asp:Label ID="lblpopupheader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>
                                        </div>
                                        <div class="col-sm-3 col-md-3 col-lg-3" id="div16" runat="server" align="right">
                                            <asp:Button ID="btnSaveReview" runat="server" CssClass="btn btn-success" Text="Save" CausesValidation="false" OnClick="btnSaveReview_Click" />
                                            <asp:Button ID="btnreviewcancel" runat="server" Text="Cancel" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnreviewcancel_Click" />
                                        </div>
                                    </div>
                                    <div runat="server" id="DivPopup">
                                        <span style="font-weight: 800;">Facility Supplies Map </span>
                                        <asp:Label ID="lblhead" runat="server" ForeColor="Red" CssClass="page-title lable-align" Text=""></asp:Label>
                                    </div>
                                    <div class="well well-sm" style="padding: 5px 15px 15px 25px;">
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
                                                <span style="font-size: 14px; font-weight: bold;">Item Category</span>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group" style="font-weight: 600;">
                                                    <asp:Label ID="lblItemCat" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="col-lg-12">
                                            <asp:Label ID="Label6" runat="server">No of records : <%=GrdFacilSupplReview.Rows.Count.ToString() %></asp:Label>
                                            <div class=" SRReviewgrid">
                                                <asp:GridView ID="GrdFacilSupplReview" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" OnRowDataBound="GrdFacilSupplReview_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="FacilitySupplyID" HeaderText="FacilitySupplyID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                        <asp:BoundField DataField="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                        <asp:BoundField DataField="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                        <asp:BoundField DataField="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                        <asp:BoundField DataField="CategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                        <asp:BoundField DataField="VendorShortName" HeaderText="Vendor Code" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="CategoryName" HeaderText="Item Group" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="VendorItemCode" HeaderText="Vendor Item Code" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="ItemID" HeaderText="Item Code" HeaderStyle-Width="60px" />
                                                        <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" HeaderStyle-Width="100px" DataFormatString="{0:F}" />
                                                        <asp:BoundField DataField="UOM" HeaderText="UOM" HeaderStyle-Width="60px" />
                                                        <asp:BoundField DataField="QtyPack" HeaderText="Qty Pack" HeaderStyle-Width="60px" />
                                                        <asp:BoundField DataField="Factor" HeaderText="Factor" HeaderStyle-Width="60px" />
                                                        <asp:BoundField DataField="Census" HeaderText="Census" HeaderStyle-Width="60px" />
                                                        <asp:BoundField DataField="Parlevel" HeaderText="Parlevel" HeaderStyle-Width="60px" />
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

        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center> This User don't have a Permission to View This Page...</center>
            </h4>
        </div>

        <div style="display: none">
            <rsweb:ReportViewer ID="rvFacilitySuppliesMapreport" runat="server"></rsweb:ReportViewer>
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
                        <asp:Label ID="Label2" runat="server" Text="" />
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



</asp:Content>
