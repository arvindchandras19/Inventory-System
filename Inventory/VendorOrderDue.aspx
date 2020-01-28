<%@ Page Title="Vendor Order Due" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="VendorOrderDue.aspx.cs" Inherits="Inventory.VendorOverDue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : Vendororderdue.aspx 
' Type      : ASPX File 
' Description  :   To design the VendorOrderdue page for add,Update and show the Vendororderdue list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01              Dhanasekaran.C                    New
  10/24/2017           V.02              Sairam.P                      Model Popup for notifications
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

    </style>

     <script type="text/javascript">
         function CorpDrop() {
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

         function LoadSumo() {
             $('.drpfacility').SumoSelect({
                 
                 selectAll: false,
                 placeholder: 'Select Facility'
             });
             $('.drpVendorsearch').SumoSelect({

                 selectAll: true,
                 placeholder: 'Select Vendor'
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

        var old_date;
        var old_time_diff;
        var Is_Lessthan_Orderdate;
        var Is_Grater_time_diff;
        var grid_rowindex;
        function updateValue(theGrid, rowIdx) {
            var total = 0;
            var cellCount = theGrid.rows[rowIdx].cells.length - 2;
            for (var i = 1; i < cellCount; i++) {
                if (theGrid.rows[rowIdx].cells[i].children[0].children[0].value != "")
                    total += (theGrid.rows[rowIdx].cells[i].children[0].children[0].value - 0);
            }
            theGrid.rows[rowIdx].cells[8].children[0].children[0].value = total;
        }
        function Validate() {
            var RB1 = document.getElementById("<%=rbordertype.ClientID%>");
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
       
        <%--function Validatetxtbox() {
            var txtbxresult = false;
            var Corporate = document.getElementById('<%= drpcor.ClientID %>').value.trim();
            var Facility = document.getElementById('<%= drpfacility.ClientID %>').value.trim();
            var Vendor = document.getElementById('<%= drpvendor.ClientID %>').value.trim();
            var OrderType = Validate();
            var OrderDueDate = document.getElementById('<%= txtorderduedate.ClientID %>').value.trim();
            var drpdeliwin = document.getElementById('<%= drpdeliwin.ClientID %>').value.trim();
            var Daysnotify = document.getElementById('<%= txtdaysnotify.ClientID %>').value.trim();
            var Checkhidden = document.getElementById('<%=hdnvendororderID.ClientID%>').value;

            if (Checkhidden == "0" || Checkhidden == "") {
                if (Corporate == "0" || Facility == "" || Vendor == "0" || OrderType == false || OrderDueDate == "" || drpdeliwin == "0" || Daysnotify == "") {
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

        function GetHiddenValue(lnk, txt) {
            var row = lnk.parentNode.parentNode;
            grid_rowindex = row.rowIndex - 1;
            var txtoderDuedate = $("input[id*=txtoderDuedate]")
            var txtDeliveryDate = $("input[id*=txtDeliveryDate]")
            var txtToNotify = $("input[id*=txtToNotify]")
            var Olddeldate = txtDeliveryDate[row.rowIndex - 1].defaultValue;

            old_date = Olddeldate;
            var oldnotify = txtToNotify[row.rowIndex - 1].defaultValue;
            old_time_diff = oldnotify;
            var OrderDate = new Date(txtoderDuedate[row.rowIndex - 1].value);
            var Deliverydate = new Date(txtDeliveryDate[row.rowIndex - 1].value);

            var timeDiff = Math.abs(Deliverydate.getTime() - OrderDate.getTime());
            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

            if (txt == 3) {
                if (diffDays < txtToNotify[row.rowIndex - 1].value) {
                    Is_Grater_time_diff = true;

                    //Olddeldate = txtDeliveryDate[row.rowIndex - 1].defaultValue;
                    ShowwarningPopup("DaysToNotify does not exits delivery period " + diffDays + "days");
                }
                else {
                    Is_Grater_time_diff = false;
                }
            } else if (txt == 2) {
                if (OrderDate > Deliverydate) {
                    ShowwarningPopup("DeliveryDate should not be less than orderDate ");
                    Is_Lessthan_Orderdate = true;
                    //txtDeliveryDate[row.rowIndex - 1].defaultValue.value = test;
                }
                else {
                    Is_Lessthan_Orderdate = false;
                }
            }
            
            document.getElementById('<%=hdnvendororderID.ClientID%>').value += (row.rowIndex - 1) + ',';
        }


        function CallOnblurDeliveryDate() {
            if (Is_Lessthan_Orderdate == true) {
                var dayToNotify = $("input[id*=txtDeliveryDate]")
                dayToNotify[grid_rowindex].value = old_date;
            }

        }
        function CallOnblurtxtNotify() {
            if (Is_Grater_time_diff == true) {
                var txtToNotify = $("input[id*=txtToNotify]");
                txtToNotify[grid_rowindex].value = old_time_diff;
            }
        }
        function DaysToNotifyValidation() {
            var DeliveryWindow = document.getElementById('<%= drpdeliwin.ClientID %>').value.trim();
            var DaysTonotify = document.getElementById('<%= txtdaysnotify.ClientID %>').value.trim();
            var OrderType = test();
            var a = parseInt(DeliveryWindow);
            var b = parseInt(DaysTonotify);

            if (DeliveryWindow != 0 && OrderType != 0) {
                if (a < b) {
                    ShowwarningPopup("DaysToNotify should be less than deliverywindow");
                    document.getElementById('<%= txtdaysnotify.ClientID %>').value = "";
               <%-- document.getElementById('<%= drpdeliwin.ClientID %>').value = 0;--%>
                }
                else {

                }
            }
            else if (DeliveryWindow == 0 && OrderType != 0) {
                ShowwarningPopup("Please select delivery window");
                document.getElementById('<%= txtdaysnotify.ClientID %>').value = "";
            }
            else if (OrderType == 0) {
                ShowwarningPopup("Please select ordertype");
                document.getElementById('<%= txtdaysnotify.ClientID %>').value = "";
        }
        else {

        }
}
function test() {
    var rad = document.getElementById('<%=rbordertype.ClientID %>');
    var radio = rad.getElementsByTagName("input");
    var selectedvalue = 0;
    for (var i = 0; i < radio.length; i++) {
        if (radio[i].checked) {
            selectedvalue = radio[i].value;
        }
    }
    return selectedvalue;
}


function CheckIsValidSave() {
    var result = true;
    var message = '';

    $("[id*=grdaddvendor] tbody tr").each(function () {
        if ($(this).find("[id*=txtDeliveryDate]").val() == '') {
            result = false;
            message = 'Delivery Date is should not be empty';
        }
        if ($(this).find("[id*=txtToNotify]").val() == '') {
            result = false;
            message = 'Days To Notify is should not be empty';
        }
    });
    if (result == false) {
    }

    if (result == false) {
        ShowwarningPopup(message);
    }
    else {
    }
    return result;
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
    $('[id*=lblwarning]').html(res);
    $("#modalWarning").modal("show");
}
function ShowwarningOrderLookupPopup(res) {
    $('[id*=lblwarningorder]').html(res);
    $("#modalorderWarning").modal("show");
}
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   

        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Vendor Order Due
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                 <script type="text/javascript">
                     Sys.Application.add_load(LoadSumo);
                     Sys.Application.add_load(CorpDrop);
                     Sys.Application.add_load(jscriptsearch);
                </script>

                  <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="DivFacCorp">
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
                <asp:HiddenField ID="hdnvendororderID" runat="server" />
                 <asp:HiddenField ID="hdneditororder" runat="server" />
                <asp:HiddenField ID="hdSearchDetail" runat="server" />
                  <asp:HiddenField ID="hdndeleteid" runat="server" />
                 <asp:HiddenField ID="HddListFacID" runat="server" />
                <div class="mypanel-body" id="divvendordue" runat="server" style="padding: 5px 15px 15px 15px;">
                    <div class="row" id="divsearchbtn" runat="server">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-sm-8 col-md-8 col-lg-8" align="right">
                             <asp:Button ID="btnsearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="Emptyfields" OnClick="btnsearch_Click"  />
                            <asp:Button ID="btnadd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnadd_Click" />
                             <asp:Button ID="btnprint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnprint_Click" />
                            <asp:Button ID="btnclear" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnclear_Click" />
                        </div>
                    </div>
                    <div id="divorderdue" style="margin-top: 5px;" runat="server">
                        <div id="divContent" class="well well-sm">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Corporate</span><span style="color: red">*</span>
                                        <asp:DropDownList ID="drpcor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqfielddrpcor" ValidationGroup="Emptyfields" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcor" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiFac" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearFac" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearFac_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllFac" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllFac_Click"></asp:LinkButton>       
                                        <asp:ListBox ID="drpfacilitysearch" runat="server" CssClass="form-control drpfacility" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpfacility_SelectedIndexChanged"></asp:ListBox>
                                         <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpfacility" ValidationGroup="Emptyfields" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacilitysearch" ErrorMessage="" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                          <span>Vendor</span><span style="color: red">*</span>
                                        <%--<asp:DropDownList ID="drpfacility" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpfacility_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                                        <asp:ListBox ID="drpVendorsearch" runat="server" CssClass="form-control drpVendorsearch" multiple="multiple" SelectionMode="Multiple"></asp:ListBox>
                                         <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendorsearch" ValidationGroup="Emptyfields" runat="server" ForeColor="Red"
                                            ControlToValidate="drpVendorsearch" ErrorMessage="" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Date From</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="rfvDateFrom" runat="server" ControlToValidate="txtDateFrom" ValidationGroup="Emptyfields"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revdatefrom" ControlToValidate="txtDateFrom"
                                            ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                            SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="Empty" Font-Size=".9em" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY" MaxLength="10"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateFrom" runat="server" TargetControlID="txtDateFrom" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Date To</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="Emptyfields" Font-Size=".9em"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revdateto" ControlToValidate="txtDateTo"
                                            ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                            SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="Empty" Font-Size=".9em" Display="Dynamic">
                                        </asp:RegularExpressionValidator>
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY" MaxLength="10"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divresult" runat="server" style="display: none;">
                            <div class="row">
                                <div class="col-sm-6 col-md-6 col-lg-6">
                                    <asp:Label ID="Label1" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-md-6 col-lg-6">
                                    <asp:Label ID="lblrowcount" runat="server">No of records : <%=grdvendororderue.Rows.Count.ToString() %></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="row vendororderduegrid" style="margin-left: 1px; margin-top: 3px;" id="divvendor" runat="server">
                            <asp:GridView ID="grdvendororderue" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found">
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/edit.png" OnClick="imgbtnEdit_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="VenOrderDueID" HeaderText="VenOrderDueID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" />
                                    <asp:BoundField DataField="VendorShortName" HeaderText="Vendor"  />
                                    <asp:BoundField DataField="OrderdueDate" HeaderText="Orderdue Date" DataFormatString="{0:MM/dd/yyyy}"  />
                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:MM/dd/yyyy}"  />
                                    <asp:BoundField DataField="OrderType" HeaderText="Order Type" />
                                    <asp:BoundField DataField="DaysToNotify" HeaderText="Days To Notify" />
                                    <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="DeliveryWindow" HeaderText="DeliveryWindow" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />

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

                    <div id="adddiv" runat="server" style="display: none">
                        <div class="row">
                            <div class="col-sm-8 col-md-8 col-lg-8"></div>
                            <div class="col-sm-4 col-md-4 col-lg-4" align="right">
                                <asp:Button ID="btnaddnew" runat="server" CssClass="btn btn-primary" Text="ShowGrid" ValidationGroup="EmptyField" OnClick="btnaddnew_Click" />
                                <asp:Button ID="btnreview" runat="server" CssClass="btn btn-primary" Text="Review" ValidationGroup="EmptyField" OnClick="btnreview_Click" OnClientClick="return CheckIsValidSave()" />
                                <asp:Button ID="btncancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btncancel_Click" />
                            </div>
                        </div>
                        <div class="well well-sm" style="margin-top: 5px;">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Corporate</span><span style="color: red">*</span>
                                        <asp:DropDownList ID="ddrpcorporate" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddrpcorporate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="reqcroporate" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="ddrpcorporate" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Facility</span><span style="color: red">*</span>
                                        <asp:DropDownList ID="ddrpfacility" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddrpfacility_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="reqfacility" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="ddrpfacility" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Vendor</span><span style="color: red">*</span>
                                        <asp:DropDownList ID="ddrpvendor" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="reqvendoradd" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="ddrpvendor" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Order Type</span><span style="color: red">*</span>
                                        <asp:RadioButtonList ID="rbordertype" runat="Server" RepeatDirection="Horizontal" CssClass="rbl" OnSelectedIndexChanged="rbordertype_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Weekly" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Bi-Monthly" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Monthly" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Ad-hoc" Value="4"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RequiredFieldValidator runat="server" ID="radRfv" ControlToValidate="rbordertype" ForeColor="Red" ValidationGroup="EmptyField" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage=""></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div id ="divadhoc" runat="server">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Order Due Date</span><span style="color: red">*</span>
                                        <asp:TextBox ID="txtorderduedate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqfieldorderduedate" runat="server" ControlToValidate="txtorderduedate" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtorderduedate"></ajax:CalendarExtender>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Delivery Window</span><span style="color: red">*</span><br />
                                        <asp:DropDownList ID="drpdeliwin" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFielddrpdeliwin" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpdeliwin" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <span>Days To Notify</span><span style="color: red">*</span>
                                        <asp:TextBox ID="txtdaysnotify" runat="server" CssClass="form-control" onchange="DaysToNotifyValidation()"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqfieldtxtdaysnotify" runat="server" ControlToValidate="txtdaysnotify" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <ajax:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtdaysnotify" runat="server"></ajax:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>
                          </div>
                        </div>
                    </div>
                    <div id="divaddgrid" runat="server" style="display: none">
                         <asp:Label ID="Label3" runat="server">No of records : <%=grdaddvendor.Rows.Count.ToString() %></asp:Label>
                        <div class="vendororderduegrid">
                        <asp:GridView ID="grdaddvendor" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive">
                            <Columns>
                                <asp:BoundField DataField="VenOrderDueID" HeaderText="VenOrderDueID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <%-- <asp:BoundField DataField="FacilityDescription" HeaderText="Facility" />--%>
                                <asp:TemplateField HeaderText="Facility">
                                    <ItemTemplate>
                                        <asp:Label ID="lbladdFacility" runat="server" Text='<%# Eval("FacilityShortName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="VendorDescription" HeaderText="Vendor" />--%>
                                <asp:TemplateField HeaderText="Vendor">
                                    <ItemTemplate>
                                        <asp:Label ID="lbladdVendor" runat="server" Text='<%# Eval("VendorShortName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Orderdue Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtoderDuedate" runat="server" Text='<%# Eval("OrderdueDate","{0:M/d/yyyy}") %>' onchange="GetHiddenValue(this,1)" Enabled="false" />
                                        <ajax:CalendarExtender ID="ceduedate" runat="server" TargetControlID="txtoderDuedate" PopupPosition="BottomRight" Enabled="True"></ajax:CalendarExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivery Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDeliveryDate" runat="server" Text='<%# Eval("DeliveryDate","{0:M/d/yyyy}") %>' onblur="CallOnblurDeliveryDate()" onchange="GetHiddenValue(this,2)" />
                                        <ajax:CalendarExtender ID="cedelidate" runat="server" TargetControlID="txtDeliveryDate" PopupPosition="BottomRight" Enabled="True"></ajax:CalendarExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="OrderType" HeaderText="Order Type" />--%>
                                <asp:TemplateField HeaderText="Order Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lbladdOrderType" runat="server" Text='<%# Eval("OrderType")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Days To Notify">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtToNotify" runat="server" Text='<%# Eval("DaysToNotify") %>' onblur="CallOnblurtxtNotify()" onchange="GetHiddenValue(this,3)" />
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
            <rsweb:ReportViewer ID="rvVendorOrderDuereport" runat="server"></rsweb:ReportViewer>
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
                        <h4 class="modal-title" style="color: green; font-size: large">VendorOrder Due - Review</h4>
                    </div>
                    <div class="modal-body" style="padding: 5px;">
                        <div class="form-horizontal">
                            <div class="row" style="margin-bottom: 2px;">
                                <div class="col-lg-3" align="left">
                                    <asp:Label ID="lblpopupheader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>
                                </div>
                                <div class="col-lg-9" align="right">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" ValidationGroup="EmptyField" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnclosepop" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnclosepop_Click" />
                                </div>
                            </div>
                            <div class="well well-sm" style="padding: 5px 15px 15px 25px;">
                                <div class="row">
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Corporate</span>
                                            <asp:Label ID="lblCorp" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Facility:</span>
                                            <asp:Label ID="lblFac" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Vendor:</span>
                                            <asp:Label ID="lblVen" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Order Type: </span>
                                            <asp:Label ID="lblordertype" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">OrderDue Date:</span>
                                            <asp:Label ID="lblduedate" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Delivery Window:</span>
                                            <asp:Label ID="lbldelivery" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Days To Notify:</span>
                                            <asp:Label ID="lblnotifydate" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <asp:Label ID="lblpopupitems" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="SRReviewgrid">
                                        <asp:GridView ID="grdreview" runat="server" ShowHeaderWhenEmpty="true" ShowHeader="true" AutoGenerateColumns="false"
                                            EmptyDataText="No Records Found" CssClass="table table-responsive">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Facility">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfaility" runat="server" Text='<%# Eval("FacilityShortName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vendor">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendor" runat="server" Text='<%# Eval("VendorShortName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OrderDue Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblorderdate" runat="server" Text='<%# Eval("OrderdueDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delivery Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Eval("DeliveryDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Order Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderType" runat="server" Text='<%# Eval("OrderType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Days To Notify">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDaysToNotify" runat="server" Text='<%# Eval("DaysToNotify") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="Headerstyle" />
                                          <%--  <FooterStyle CssClass="gridfooter" />--%>
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
            <rsweb:ReportViewer ID="rvCapitalItemsreport" runat="server"></rsweb:ReportViewer>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">Vendor Order Due
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
                        <h4 class="modal-title font-bold text-white">Vendor Order Due
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
                        <h4 class="modal-title font-bold text-white">Vendor Order Due
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
     
         <div id="modalorderWarning" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header btn-warning">
                        <h4 class="modal-title font-bold text-white">Vendor Order Due
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblwarningorder" runat="server"></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnorderreview" runat="server" CssClass="btn btn-primary" Text="Yes" OnClick="btnorderreview_Click" />
                       <button type="button" class="btn btn-default ra-100" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
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
