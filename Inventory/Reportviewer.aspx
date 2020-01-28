<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Inven.Master" CodeBehind="Reportviewer.aspx.cs" Inherits="Inventory.Reportviewer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Common.css" rel="stylesheet" />
    <link href="Content/sumoselect.css" rel="stylesheet" />
    <script src="Scripts/CDN.js/Cdn.js"></script>
    <script src="Scripts/datepicker.js" type="text/javascript"></script>
    <link href="Content/datepicker.css" rel="stylesheet" />
    <link rel="shortcut icon" href="#" />


    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="https://cdn.datatables.net/1.10.19/js/dataTables.bootstrap.min.js" type="text/javascript"></script>
    <link href="https://cdn.datatables.net/1.10.19/css/dataTables.bootstrap.min.css" rel="stylesheet" />

    <link href="https://cdn.datatables.net/buttons/1.5.2/css/buttons.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/buttons/1.5.2/js/dataTables.buttons.min.js" type="text/javascript"></script>

    <script src="https://cdn.datatables.net/buttons/1.5.2/js/buttons.flash.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js" type="text/javascript"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.36/pdfmake.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.36/vfs_fonts.js" type="text/javascript"></script>

    <script src="https://cdn.datatables.net/buttons/1.5.2/js/buttons.html5.min.js" type="text/javascript"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.2/js/buttons.print.min.js" type="text/javascript"></script>

    <link href="https://cdn.datatables.net/rowgroup/1.0.4/css/rowGroup.bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/rowgroup/1.0.4/js/dataTables.rowGroup.min.js" type="text/javascript"></script>

    <script src="https://cdn.datatables.net/fixedcolumns/3.2.6/js/dataTables.fixedColumns.min.js" type="text/javascript"></script>
    <link href="https://cdn.datatables.net/fixedcolumns/3.2.6/css/fixedColumns.dataTables.min.css" rel="stylesheet" />

    <script src="Scripts/Reports.js/Reportviewer.js"></script>
    <script src="Scripts/Reports.js/MonthlyUsage.js"></script>
    <script src="Scripts/Reports.js/MonthlyPurchase.js"></script>
    <script src="Scripts/Reports.js/MonthlyEndingInventory.js"></script>
    <script src="Scripts/Reports.js/CumulativebyItemCategory.js"></script>
    <script src="Scripts/Reports.js/CumulativebyItemDesc.js"></script>
    <script src="Scripts/Reports.js/MonthlyInventory.js"></script>
    <script src="Scripts/Reports.js/Costpertx.js"></script>
    <script src="Scripts/Reports.js/MonthlyDrugssupplies.js"></script>

    <style type="text/css">
        /*table.dataTable.no-footer {
            border: none;
        }*/

        /*table.dataTable thead th {
            padding: 5px;
        }*/

        table.dataTable thead th {
            background: #0271dd;
            color: #fff;
        }

        table.dataTable {
            margin: 0px !important;
        }

        /*table.dataTable tbody td {
            padding: 0px;
            text-align: left;
        }*/

        /*th, td { white-space: nowrap; }
    div.dataTables_wrapper {
        width: 100%;
        margin: 0 auto;
    }*/

        th, td {
            white-space: nowrap;
        }

        div.dataTables_wrapper {
            width: 100%;
        }

        table.dataTable thead .sorting:after {
            display: none;
        }

        table.dataTable thead .sorting_asc:after, table.dataTable thead .sorting_desc:after {
            display: none;
        }

        .DTFC_LeftBodyLiner {
            /*overflow-y: hidden !important;*/
            /*width:auto !important;*/
            /*margin-top:-1px !important;*/
            /*height:auto !important;
                max-height:none !important;*/
        }

        .DTFC_LeftWrapper {
            position: absolute !important;
            top: 0 !important;
            right: 0 !important;
        }

        button.dt-button {
            color: #fff;
            background-color: #0271dd;
            background-image: -webkit-linear-gradient(top, #0271dd 0%, #0271dd 100%);
            border-color: #122b40;
        }

            button.dt-button:hover:not(.disabled), div.dt-button:hover:not(.disabled), a.dt-button:hover:not(.disabled) {
                background-color: #0271dd;
                background-image: -webkit-linear-gradient(top, #0271dd 0%, #0271dd 100%);
                border-color: #122b40;
            }

        /*button.dt-button{
                 color:#fff;
                 background-color:#0271dd;
             }*/
        /*.th tr{
                     background: #0271dd;
                     color: #fff;
             }*/

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
            align-items: center;
            align vertical;
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
        }


        .panel-title > a:before {
            float: right !important;
            font-family: FontAwesome;
            content: "\f068";
            padding-right: 5px;
        }

        .panel-title > a.collapsed:before {
            float: right !important;
            content: "\f067";
        }

        .panel-title > a:hover,
        .panel-title > a:active,
        .panel-title > a:focus {
            text-decoration: none;
        }

        div.dt-buttons {
            float: right;
        }

        .panel-default > .panel-heading {
            color: #fff;
            background-color: #0271dd;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="page-title-breadcrumb">
        <div class="page-header">
            <div class="page-header page-title">
                Reports
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="updmain" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(jScript);
                Sys.Application.add_load(jscriptsearch);
                //Sys.Application.add_load(JDataTable);
                Sys.Application.add_load(ShowControls);
                Sys.Application.add_load(DropdownChange);
                Sys.Application.add_load(DropdownsubChange);
            </script>

            <div class="container" style="width: 100%">
                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-default" style="margin-right: 0px;">
                        <div class="panel-heading" role="tab" id="headingOne">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Report Header
                                </a>
                            </h4>
                        </div>
                        <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <div id="divMPRMaster" runat="server" style="margin-top: 5px;">
                                    <div id="divContent" class="well well-sm">
                                        <div class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Report Type</span>&nbsp;<span style="color: red">*</span>
                                                    <%--       <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpreport" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                            ControlToValidate="drpreport" ErrorMessage="This information is required."></asp:RequiredFieldValidator>--%>
                                                    <asp:DropDownList ID="drpreport" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Report Sub Type</span>&nbsp;<span style="color: red">*</span>
                                                    <%--<asp:RequiredFieldValidator InitialValue="" ID="Reqdrpsubreport" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                            ControlToValidate="drpsubreport" ErrorMessage="This information is required."></asp:RequiredFieldValidator>--%>
                                                    <asp:DropDownList ID="drpsubreport" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4" align="right">
                                                <%--<input type="button" id="btnSearch" value="Search" class="btn btn-primary" onclick="validatereport();" />--%>
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClientClick="validatereport();"/>
                                                <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" />

                                                <%--<asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClientClick="ShowCurrentTime();" />--%>
                                                <%--<asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />--%>
                                                <%--<input type="button" id="" value="Cancel" class="btn btn-primary" />--%>
                                                <%--   <asp:Button ID="btnreset" runat="server" Text="Reset" CssClass="btn btn-primary" />--%>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Corporate</span>
                                                    <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpcorsearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                        ControlToValidate="drpcorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:ListBox ID="drpcorsearch" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Facility</span>
                                                    <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpfacilitysearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                        ControlToValidate="drpfacilitysearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:ListBox ID="drpfacilitysearch" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Vendor</span>
                                                    <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendorsearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                        ControlToValidate="drpvendorsearch" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                    <asp:ListBox ID="drpvendorsearch" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Order Type</span>
                                                    <asp:RequiredFieldValidator InitialValue="" ID="Reqordertype" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                        ControlToValidate="drpordertype" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                    <asp:DropDownList ID="drpordertype" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Category</span>
                                                    <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpcategory" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                        ControlToValidate="drpcategory" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                    <asp:ListBox ID="drpcategory" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Description</span>
                                                    <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpitem" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                        ControlToValidate="drpitem" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                    <asp:ListBox ID="drpitem" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Date Range</span>&nbsp;<span style="color: red">*</span>
                                                    <%--  <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpdate" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                            ControlToValidate="drpitem" ErrorMessage="This information is required."></asp:RequiredFieldValidator>--%>
                                                    <asp:DropDownList ID="drpdaterange" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">Monthly/Year</asp:ListItem>
                                                        <asp:ListItem Value="2">From/To</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div id="divdate" style="display: none">
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <div class="form-group">
                                                        <span style="font-weight: 800;">Date From</span>&nbsp;<span style="color: red">*</span>
                                                        <%-- <asp:RequiredFieldValidator ID="rfvDateFrom" runat="server" ControlToValidate="txtDateFrom" ValidationGroup="Emptyfields"
                                                                ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
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
                                                        <span style="font-weight: 800;">Date To</span>&nbsp;<span style="color: red">*</span>
                                                        <%-- <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="Emptyfields" Font-Size=".9em"
                                                                ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                                        <asp:RegularExpressionValidator ID="revdateto" ControlToValidate="txtDateTo"
                                                            ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$" runat="server" ErrorMessage="Invalid Format(eg.MM/DD/YYYY)"
                                                            SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="Empty" Font-Size=".9em" Display="Dynamic">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY" MaxLength="10"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="divmonth" style="display: none">
                                                <div class="col-sm-4 col-md-4 col-lg-4">
                                                    <div class="form-group">
                                                        <div class="form-group">
                                                            <span style="font-weight: 800;">Month/Year</span>&nbsp;<span style="color: red">*</span>
                                                            <%--  <asp:RequiredFieldValidator ID="Reqtxtmonth" runat="server" ControlToValidate="txtmonth" ValidationGroup="Emptyfields"
                                                                    ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                                            <asp:RegularExpressionValidator ID="Regtxtmonth" ControlToValidate="txtmonth"
                                                                runat="server" ErrorMessage=""
                                                                SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="Empty" Font-Size=".9em" Display="Dynamic">
                                                            </asp:RegularExpressionValidator>
                                                            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control datepicker_monthyear" placeholder="Month-Year"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading" role="tab" id="headingTwo">
                            <h4 class="panel-title">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="true" aria-controls="collapseTwo">Report Viewer
                                    </a>
                                </h4>
                        </div>
                        <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo">
                            <div class="panel-body">
                                <div id="divdtable">
                                    <div id="divtitle" class="row well well-sm" style="display: none">
                                        <div class="col-sm-8 col-md-8 col-lg-8">
                                            <span style="font-weight: 900;">Report Title:</span>
                                            <asp:Label ID="lblreporttype" runat="server"></asp:Label>
                                            <asp:Label ID="lblsubreport" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4" align="right">
                                            <span style="font-weight: 900;">Period:</span>
                                            <asp:Label ID="lblmonth" runat="server"></asp:Label>
                                            <asp:Label ID="lbldatefrom" runat="server"></asp:Label>
                                            <asp:Label ID="lbldateto" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div id="divbndtable">
                                        <div id="divmonthusage" style="display: none">
                                            <table id="example" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                        </div>
                                        <div id="divmonthpurchase" style="display: none">
                                            <table id="tblmonthlypurchase" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                        </div>
                                        <div id="divmonthEndingInventory" style="display: none">
                                            <table id="tblmonthlyendinginventory" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                        </div>
                                        <div id="divCumulativebyCategory" style="display: none">
                                            <table id="tblCumulativebyCategory" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                        </div>
                                        <div id="divCumulativebyItemDesc" style="display: none">
                                            <table id="tblCumulativebyItemDesc" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                        </div>
                                        <div id="divmonthlyInventory" style="display: none">
                                            <table id="tblmonthlyinventory" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                        </div>
                                        <div id="divcostpertx" style="display: none">
                                            <table id="tblcostpertx" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                        </div>
                                        <div id="divMonthlydrugsupplies" style="display: none">
                                            <table id="tblmonthlydrugs" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>



            <div id="modalWarning" class="modal fade" style="position: center">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content ">
                        <div class="modal-header btn-warning">
                            <h4 class="modal-title font-bold text-white">Reports
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

</asp:Content>
