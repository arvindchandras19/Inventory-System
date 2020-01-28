<%@ Page Title="Vendor Order Due Remainder" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="VendorOrderDueRemainder.aspx.cs" Inherits="Inventory.VendorOrderDueRemainder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : Vendororderdueremainder.aspx 
' Type      : ASPX File 
' Description  :   To design the VendorOrderdueRemainder page for an print a report  list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  06/01/2018            V.01              Sairam.P                      New
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
    </style>

    <script type="text/javascript">

        //function jScript() {
        //    $('[id*=drpcor]').SumoSelect({                
        //         selectAll: true,
        //         placeholder: 'Select Corporate'
        //     });

        //    $('[id*=drpfacility]').SumoSelect({
        //         selectAll: true,
        //         placeholder: 'Select Facility'
        //     });

        //    $('[id*=drpvendor]').SumoSelect({
        //         selectAll: true,
        //         placeholder: 'Select Vendor'
        //     });
        // }

        //function jscriptsearch() {
        //    var config = {
        //        '.chosen-select': {},
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  

        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Vendor Order Due Remainder
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <%-- <script type="text/javascript">                   
                     Sys.Application.add_load(jScript);
                     Sys.Application.add_load(jscriptsearch);
                </script>--%>
                <asp:HiddenField ID="hdnvendororderID" runat="server" />
                <asp:HiddenField ID="hdSearchDetail" runat="server" />
                <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-12" align="right">
                            <asp:Button ID="btnsearch" runat="server" CssClass="btn btn-primary" ValidationGroup="EmptyField" Text="Search" OnClick="btnsearch_Click" />
                            <asp:Button ID="btnSendmail" runat="server" CssClass="btn btn-primary" ValidationGroup="EmptyField" Text="Send Email" OnClick="btnSendmail_Click" />
                            <asp:Button ID="btnpreview" runat="server" CssClass="btn btn-primary" ValidationGroup="EmptyField" Text="Preview Email" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                    <div id="divorderdue" style="margin-top: 5px;">
                        <div id="divContent" class="well well-sm">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Corporate</span><span style="color: red">*</span>
                                        <asp:DropDownList ID="drpcor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:DropDownList>
                                        <%--<asp:ListBox ID="drpcor" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:ListBox>--%>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqfielddrpcor" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpcor" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Facility</span><span style="color: red">*</span>
                                        <asp:DropDownList ID="drpfacility" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <%--<asp:ListBox ID="drpfacility" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="drpfacility_SelectedIndexChanged" ></asp:ListBox>--%>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Reqdrpfacility" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacility" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <%-- <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span>Vendor</span><span style="color: red">*</span>
                                        <asp:DropDownList ID="drpvendor" runat="server"  CssClass="form-control"></asp:DropDownList>
                                        <asp:ListBox ID="drpvendor" runat="server" CssClass="form-control" SelectionMode="Multiple" ></asp:ListBox>
                                         <asp:RequiredFieldValidator InitialValue="0" ID="ReqdrpVendor" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendor" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>                         
                                    </div>
                                </div>--%>
                                <div class="col-sm-4 col-md-4 col-lg-4" style="display:none;">
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
                            </div>
                            <div class="row" style="display:none;">
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
                            </div>
                        </div>
                          <asp:Label ID="lblcount" Visible="false" runat="server">No of records : <%=grdvendororderueremainder.Rows.Count.ToString() %></asp:Label>
                        <div class="row vendororderduegrid" style="margin-left: 1px; margin-top: 3px;" id="divvendor">
                            <asp:GridView ID="grdvendororderueremainder" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive">
                                <Columns>
                                    <asp:BoundField DataField="VenOrderDueID" HeaderText="VenOrderDueID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                    <asp:BoundField DataField="CorporateName" HeaderText="Corporate" />
                                    <asp:BoundField DataField="FacilityDescription" HeaderText="Facility" />
                                    <asp:BoundField DataField="VendorWithDate" HeaderText="Vendor OrderDue Summary" />
                                    <asp:BoundField DataField="ToEmailID" HeaderText="To Email" />
                                    <asp:BoundField DataField="OrderType" HeaderText="Order Type" />
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

        <asp:UpdatePanel ID="UpdateOrderdue" runat="server">
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
                                <h4 class="modal-title" style="color: green; font-size: large">Vendor OrderDue Remainder</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="form-horizontal">
                                    <div id="DivBodyContent" runat="server">                                       
                                        
                                        
                                    </div>
                                    <div id="DivBodyContent2" runat="server">
                                        
                                    </div>
                                    <div>
                                        <div id="DivRegards" runat="server">
                                            <br /> Regards<%--<br />Super Admin1--%>
                                        </div>                                        
                                        <div id="DivBodyContent3" runat="server">

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





        <div style="display: none">
            <rsweb:ReportViewer ID="rvvendorremainderreport" runat="server"></rsweb:ReportViewer>
        </div>

</asp:Content>
