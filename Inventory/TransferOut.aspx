<%@ Page Title="TransferOut" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="TransferOut.aspx.cs" Inherits="Inventory.TransferOut" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : TransferOut.aspx 
' Type      : ASPX File 
' Description  :   To design the Transfer Out page for add,Update and show the  Transfer Out page on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  03/13/2018             V.01              Sairam.P                       New
  06/04/2018             V.02              Sairam.P                      Transfer Qty(In Each)is added
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
    </style>

  
    <script type="text/javascript"> 
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


        $(document).on("keyup mouseup", "[id*=txtTransferQty]", function () {
        
            var row = $(this).closest("tr");
            if (!jQuery.trim($(this).val()) == '') {
                if (!isNaN(parseFloat($(this).val()))) {
                    var row = $(this).closest("tr");    
                    var lbltotprice = parseFloat(parseFloat(($("[id*=lblprice]", row).html().replace("$", "").replace (/,/g, "").trim()) * $(this).val()));
                    if (isNaN(lbltotprice))
                        $("[id*=lblTotalPrice]", row).html('');
                    else
                        $("[id*=lblTotalPrice]", row).html('$' + parseFloat(lbltotprice.toString()).toFixed(2));

                    if ($(this).val() == '') {
                        $("[id*=lblTotalPrice]", row).html('');
                        $(this).val("");
                    }
                }
            } else {
                $(this).val('');
                $("[id*=lblTotalPrice]", row).html('');
            }

        });

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
            $("#modalDelete").modal("show");
        }

        function HideConfirmationPopup() {
            $("#modalDelete").modal("Hide");
        }

        function checkTransferDate() {    
            var dateString = $('[id*=txtTransferDate]').val();
            var myDate = new Date(dateString);
            var datePreviousMonth = new Date();
            datePreviousMonth.setMonth(datePreviousMonth.getMonth() - 1);
            var futuremonth=new Date();
            if (myDate < datePreviousMonth)
            {
                ShowwarningPopup("Transfer date should not be less than 30days from current date");
                $('[id*=txtTransferDate]').val("");
            }
            if (myDate > futuremonth)
            {
                ShowwarningPopup("Transfer date should not be future date");
                $('[id*=txtTransferDate]').val("");
            }

        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Transfer Out
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(Auditpopupshow);
                    Sys.Application.add_load(Remarkspopupshow);
                    //Sys.Application.add_load(laodDatepicker);
                </script>
                <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-6" align="left">
                            <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-lg-6" align="right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyField" OnClick="btnSearch_Click"/>
                            <asp:Button ID="btnReview" runat="server" CssClass="btn btn-primary" Text="Review" ValidationGroup="EmptyField" OnClick="btnReview_Click"/>
                             <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click"/>
                             <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                        </div>
                    </div>
                    <div id="divMPRMaster" runat="server" style="margin-top: 5px;">
                        <div id="divContent" class="well well-sm">
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Enter Transfer Date</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="ReqTransferDate" runat="server" ControlToValidate="txtTransferDate" ValidationGroup="EmptyField"
                                            ErrorMessage="" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtTransferDate" runat="server" CssClass="form-control" onblur="checkTransferDate()" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalTransferDate" runat="server" TargetControlID="txtTransferDate" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilTransferDate" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtTransferDate"></ajax:FilteredTextBoxExtender>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqdrpCorporate" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpCorporate" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="drpCorporate" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpCorporate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Item Category</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqItemCategory" runat="server" ControlToValidate="drpItemCategory" ValidationGroup="EmptyField"
                                            ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="drpItemCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Transfer From</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqTrasnsferfrom" runat="server" ControlToValidate="drpTransFrom" ValidationGroup="EmptyField"
                                            ErrorMessage="This information is required" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="drpTransFrom" runat="server" CssClass="form-control"  OnSelectedIndexChanged="drpTransFrom_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Transfer To</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqTrasnsferTo" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpTransTo" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="drpTransTo" runat="server" CssClass="form-control"></asp:DropDownList>
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
                            <asp:HiddenField ID="HdnMPRDetailsID" runat="server" />
                            <asp:HiddenField ID="HddMasterID" runat="server" />
                           <asp:Label ID="lblrcount" runat="server">No of records : <%=grdTransfer.Rows.Count.ToString() %></asp:Label>
                            <div style="margin-left: 1px; margin-top: 3px;" id="divsearch">
                                <div class="MPRSearchgrid">
                                    <asp:GridView ID="grdTransfer" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                        <Columns>
                                            <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="CategoryID" HeaderText="CategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="UOMID" HeaderText="UOMID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="ItemID" HeaderText="ItemID" HeaderStyle-Width="5%"/>
                                            <asp:BoundField DataField="ItemCategory" HeaderText="ItemCategory" HeaderStyle-Width="8%"/>
                                            <asp:BoundField DataField="ItemDescription" HeaderText="ItemDescription"/>
                                            <asp:BoundField DataField="UomName" HeaderText="UOM" HeaderStyle-Width="5%"/>
                                            <asp:TemplateField HeaderText="QtyPack" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQtyPack" runat="server" Text='<%# Eval("QtyPack") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price ($)" HeaderStyle-Width="9%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblprice" runat="server" Text='<%# Eval("Price","$ {0:#,0.00}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TransferQty(In Each)" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTransferQty" runat="server"></asp:TextBox>
                                                    <ajax:FilteredTextBoxExtender ID="FiltertxtTransferQty" FilterType="Numbers" runat="server" TargetControlID="txtTransferQty"></ajax:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TotalPrice ($)" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPrice" runat="server" DataFormatString="$ {0:#,0.00}"></asp:Label>
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
                        <asp:HiddenField ID="hdncheckfield" Value="0" runat="server" />
                        <asp:HiddenField ID="HdnTransferNo" Value="0" runat="server" />
                        <asp:HiddenField ID="HdnTransferOutID" Value="0" runat="server" />
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
                                            <h4 class="modal-title" style="color: green; font-size: large">TransferOut Review</h4>
                                        </div>
                                        <div class="modal-body" style="padding: 5px;">
                                            <div class="form-horizontal">
                                                <div class="row" style="margin-bottom: 2px;" align="right">
                                                    <div class="col-lg-4" align="left">
                                                 <asp:Label ID="lblcount1" runat="server">No of records : <%=grdTRReview.Rows.Count.ToString() %></asp:Label>
                                                        </div>
                                                    <div class="col-lg-8" align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click"  />
                                                        <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-primary" Text="Go Back" OnClick="btnGoBack_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="MSRReviewgrid">
                                                        <asp:GridView ID="grdTRReview" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" 
                                                            EmptyDataText="No Records Found" CssClass="table table-responsive">
                                                            <Columns>
                                                                <asp:BoundField DataField="TransferNo" HeaderText="Transfer ID" HeaderStyle-Width="8%" />
                                                                <asp:BoundField DataField="TransferDate" HeaderText="Transferred Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="9%" />
                                                                <asp:BoundField DataField="TransferTo" HeaderText="TransferTo" HeaderStyle-Width="8%"/>
                                                               <asp:TemplateField HeaderText="UOMID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="UOMID" runat="server" Text='<%# Eval("UOMID") %>' HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="ItemID" HeaderStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="ItemID" runat="server" Text='<%# Eval("ItemID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Item Description">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="ItemDescription" runat="server" Text='<%# Eval("ItemDescription") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="4%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="UomName" runat="server" Text='<%# Eval("UomName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                               <asp:TemplateField HeaderText="Qty Pack" HeaderStyle-Width="4%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQtyPack" runat="server" Text='<%# Eval("QtyPack") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Price ($)" HeaderStyle-Width="9%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblprice" runat="server" Text='<%# Eval("Price","$ {0:#,0.00}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TransferQty (In Each)" HeaderStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtTransferQty" runat="server" Enabled="false" Text='<%# Eval("TransferQty") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TotalPrice ($)" HeaderStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalPrice","$ {0:#,0.00}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="Headerstyle" />
                                                            <%--<FooterStyle CssClass="gridfooter" />--%>
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
                                    <h4 class="modal-title font-bold text-white">Transfer Out
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
                                    <h4 class="modal-title font-bold text-white">Transfer Out
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
                                    <h4 class="modal-title font-bold text-white">Transfer Out
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
            <rsweb:ReportViewer ID="rvTransferOutReport" runat="server"></rsweb:ReportViewer>
        </div>
        <%--<div style="display: none">
            <rsweb:ReportViewer ID="rvMachinePoreportReview" runat="server"></rsweb:ReportViewer>
        </div>--%>
        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center> This User don't have a Permission to View This Page...</center>
            </h4>
        </div>

</asp:Content>
