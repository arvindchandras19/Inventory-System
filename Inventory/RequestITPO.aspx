<%@ Page Title="Generate/Approve-IT Request" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="RequestITPO.aspx.cs" Inherits="Inventory.RequestIT_PO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : RequestIT PO.aspx 
' Type      : ASPX File 
' Description  :   To design the Request IT PO page for add,Update and show the Request IT Parts on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version                 By                          Reason 
 12/26/2017              V.01                S.Mahalakshmi                    New
 05/Mar/2018          V.01                  Vivekanand.S                   Multi Search  
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

        function GetgrdOrderSearchIndexValue(orderid) {
            var row = orderid.parentNode.parentNode;
            grdMedReqsearchIndex = row.rowIndex - 1;
            document.getElementById('<%=HdnMSRDetailID.ClientID%>').value += (grdMedReqsearchIndex) + ',';
            console.log(grdMedReqsearchIndex);
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

        function Hide(lnk) {  
            var IsValidRemarks = true;
            $("[id*=grdreviewreqpo] tbody tr").each(function () {
                var txtremarks = $(this).find("[id*=lblRemarks]").val();
                if (txtremarks == '') {
                    IsValidRemarks = false;
                }
            });
            if (IsValidRemarks == true) {
                $('[id*=btnGenerateOrder]').hide();
            }
        }

    </script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
         <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                   IT Order
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
                <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                    <div class="row">
                        <div class="col-lg-4" align="left">
                            <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-lg-8" align="right">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" Visible="false" />
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyField" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnReview" Visible="false" runat="server" CssClass="btn btn-primary" ValidationGroup="EmptyFieldSave" Text="Review" />

                            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
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
                                        <asp:ListBox ID="drpfacilitysearch" runat="server" CssClass="form-control" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="drpfacilitysearch_SelectedIndexChanged"></asp:ListBox>
                                           <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpfacilitysearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpfacilitysearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span> 
                                        <asp:RequiredFieldValidator InitialValue="" ID="Reqdrpvendorsearch" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drpvendorsearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>   
                                        <asp:ListBox ID="drpvendorsearch" runat="server" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>                                    
                                        <%--<asp:DropDownList ID="drpvendorsearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date From</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="ReqDateFrom" runat="server" ControlToValidate="txtDateFrom" ValidationGroup="EmptyField"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateFrom" runat="server" TargetControlID="txtDateFrom" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilterDateFrom" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateFrom"></ajax:FilteredTextBoxExtender>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Date To</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator ID="ReqtxtDateTo" runat="server" ControlToValidate="txtDateTo" ValidationGroup="EmptyField"
                                            ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                        <ajax:FilteredTextBoxExtender ID="FilterDateTo" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateTo"></ajax:FilteredTextBoxExtender>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight: 800;">Status</span>&nbsp;<span style="color: red">*</span>
                                        <asp:RequiredFieldValidator InitialValue="" ID="ReqdrpStatus" runat="server" ForeColor="Red"
                                            ControlToValidate="drpStatussearch" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:ListBox ID="drpStatussearch" runat="server" CssClass="form-control" SelectionMode="Multiple" ></asp:ListBox> 
                                        <%--<asp:DropDownList ID="drpStatussearch" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <div style="margin-left: 1px; margin-top: 3px;" id="divsearch">
                            <div class="row">
                                <div class="col-lg-4" align="left">
                                    <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                </div>
                                <div class="col-lg-8" align="right">
                                    <asp:Button ID="btnrefresh" runat="server" Text="Refresh" CssClass="btn btn-primary" OnClick="btnrefresh_Click" />
                                    <asp:Button ID="btnapproveall" runat="server" CssClass="btn btn-primary" Text="Approve All" OnClick="btnapproveall_Click" />
                                    <asp:Button ID="btnorderall" runat="server" CssClass="btn btn-primary" ValidationGroup="EmptyFieldSave" Text="Order All" OnClick="btnorderall_Click" />
                                    <asp:Button ID="btnbtmreview" runat="server" Text="Review" CssClass="btn btn-primary" OnClick="btnbtmreview_Click" />
                                    <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                                </div>
                            </div>
                               <asp:Label ID="lblcount" runat="server">No of records : <%=grdITRequestPO.Rows.Count.ToString() %></asp:Label>
                            <div class="MPRSearchgrid">
                                <asp:GridView ID="grdITRequestPO" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="grdITRequestPO_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/edit.png" OnClick="imgbtnEdit_Click" ToolTip="Edit" />
                                                <asp:ImageButton ID="imgprint" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgprint_Click" ToolTip="Print" />
                                                <asp:ImageButton ID="imgresend" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/email_icon.png" ToolTip="Print" OnClick="imgresend_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RequestITMasterID" HeaderText="RequestITMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />                                     
                                        <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="Facility" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="CreatedOn" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="9%" ItemStyle-Width="9%"/>
                                        <asp:BoundField DataField="CorporateName" HeaderText="Corp" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="VendorShortName" HeaderText="Vendor" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                        <asp:TemplateField HeaderText="ITRNo">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbitrno" runat="server" Text='<%# Eval("ITRNo")%>' OnClick="lbitrno_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ITONO">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbitono" runat="server" Text='<%# Eval("ITNNo")%>' OnClick="lbitono_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TotalCost" HeaderText="TotalPrice($)" DataFormatString="$ {0:#,0.00}" />
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpostatus" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="drpaction" runat="server" onchange="GetgrdOrderSearchIndexValue(this)" CssClass="form-control">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblaudit" runat="server" Visible="false" Text=' <%# Eval("Audit")%>'></asp:Label>
                                                <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Visible="false" Text='<%# Eval("Remarks")%>'></asp:Label>
                                                <asp:Image ID="imgreadmore" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Remarks")%>' data-toggle="popover" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RequestITOrderID" HeaderText="RequestITOrderID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
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
                        <asp:HiddenField ID="HddListCorpID" runat="server" />
                        <asp:HiddenField ID="HddListFacID" runat="server" />
                    </div>
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
                                        <h4 class="modal-title" style="color: green; font-size: large">IT Order Review</h4>
                                    </div>
                                    <div class="modal-body" style="padding: 5px;">
                                        <div class="form-horizontal">
                                            <div class="row" style="margin-bottom: 2px;">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="col-lg-3"><span class="page-header page-title">Items</span></div>
                                                    <div class="col-lg-9" align="right">
                                                        <asp:Label ID="lblordermsg" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 col-md-6 col-sm-6" align="right">
                                                    <asp:Button ID="btnGenerateOrder" runat="server" CssClass="btn btn-success" Text="Generate/Approve Order" OnClientClick="Hide(this);"  OnClick="btnGenerateOrder_Click" ValidationGroup="Empty Field" />
                                                    <asp:Button ID="btnrevwcancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnrevwcancel_Click" />
                                                </div>
                                            </div>                                          
                                            <div id="DivMPRMasterNoreview" runat="server" style="display: none;">
                                                <span style="font-weight: 800;">Request IT PartsNo:- </span>
                                                <asp:Label ID="lblmprreview" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            </div>
                                            <div class="well well-sm" style="padding: 5px 15px 15px 25px; display: none;">

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

                                            </div>
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                       <asp:Label ID="lblrcount1" runat="server">No of records : <%=grdreviewreqpo.Rows.Count.ToString() %></asp:Label>
                                                    <div class="SRReviewgrid">
                                                        <asp:GridView ID="grdreviewreqpo" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive" OnRowDataBound="grdreviewreqpo_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Print" HeaderStyle-Width="4%" ItemStyle-Width="4%">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgrevprint" runat="server" Text="Print" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgrevprint_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                                <asp:BoundField DataField="Facility" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                                <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                                <asp:BoundField DataField="CreatedOn" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                                                <asp:BoundField DataField="CorporateName" HeaderText="Corp" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                                                <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                                                <asp:BoundField DataField="VendorShortName" HeaderText="Vendor" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                                                <asp:TemplateField HeaderText="ITRNo">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbrevitrno" runat="server" Text='<%# Eval("ITRNo")%>' OnClick="lbitrno_Click"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ITONO">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbrevitono" runat="server" Text='<%# Eval("ITNNo")%>' OnClick="lbitono_Click"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="TotalCost" HeaderText="TotalPrice($)" DataFormatString="{0:#,0.00}" />
                                                                <asp:TemplateField HeaderText="Status"  HeaderStyle-Width="9%" ItemStyle-Width="9%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblrevpostatus" runat="server" Text='<%# Eval("Status")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblaction" runat="server" Text='<%# Eval("Action")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                                                    <ItemTemplate>                                                                     
                                                                        <asp:Label ID="lblaudit" runat="server" Visible="false" Text=' <%# Eval("Audit")%>'></asp:Label>
                                                                        <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                                                    <ItemTemplate>                                                                     
                                                                        <asp:TextBox ID="lblRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvRemarks" ValidationGroup="Empty Field" ControlToValidate="lblRemarks" runat="server"
                                                                            ErrorMessage="Please Enter the Remark" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        <%--<asp:Image ID="imgreadmore" runat="server"  ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="30px" data-content='<%# Eval("Remarks")%>' data-toggle="popover" />--%>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="RequestITMasterID" HeaderText="RequestITMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                                                <asp:BoundField DataField="RequestITOrderID" HeaderText="RequestITOrderID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="upnlview" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnmpedemo" runat="server" Style="display: none" />
                        <ajax:ModalPopupExtender ID="mpeimgview" runat="server"
                            TargetControlID="btnmpedemo" PopupControlID="pnlimgview" CancelControlID="btnpopclose"
                            BackgroundCssClass="modalBackground">
                        </ajax:ModalPopupExtender>
                        <asp:Panel ID="pnlimgview" Style="display: none;" runat="server">
                            <div style="position: fixed; top: 0; right: 0; bottom: 0; left: 0; z-index: 1040; overflow: auto; overflow-y: scroll; margin-top: 1%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="height: 40px">
                                            <asp:Button ID="btnpopclose" runat="server" CssClass="close" Text="X" />
                                            <asp:Label runat="server" Text="Document Viewer" ID="lblimg" />
                                        </div>
                                        <div class="modal-body" style="padding: 2px;">
                                            <div class="container-fluid" runat="server" id="Div2">
                                                <iframe id="frame1" runat="server" style="height: 530px; width: 100%;" onscroll="auto"></iframe>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>



                <asp:Button ID="btnrequestreview" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeitrreview" runat="server"
                    PopupControlID="modalitrreview" TargetControlID="btnrequestreview"
                    BackgroundCssClass="modalBackground" BehaviorID="modalitrreview" CancelControlID="btnitrclose">
                </ajax:ModalPopupExtender>
                <div id="modalitrreview" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="btnitrclose" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: green; font-size: large">Request IT Parts Request  - Review</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px;">
                                <div class="form-horizontal">
                                    <div class="row" style="margin-bottom: 2px;">
                                        <div class="col-lg-3" align="left">
                                            <asp:Label ID="lblitrheader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>
                                        </div>
                                        </div>
                                        <div>
                                             <span style="font-weight: 800;">Request IT PartsNo:- </span>
                                        <asp:Label ID="lablitrno" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                    
                                    <div id="Div1" runat="server" style="display: none;">
                                        <span style="font-weight: 800;">Request IT PartsNo:- </span>
                                        <asp:Label ID="lblitrno" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </div>
                                    <div class="well well-sm" style="padding: 5px 15px 15px 25px;">

                                        <div class="row">
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Corporate</span>
                                                    <asp:Label ID="lblitrcor" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Facility:</span>
                                                    <asp:Label ID="lblitrfac" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Vendor:</span>
                                                    <asp:Label ID="lblitrven" runat="server"></asp:Label>
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

                                    <asp:Label ID="Label6" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label>
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
                                                                <asp:Label ID="lblequrecat" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Equipment List">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblequrelst" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Serial No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitrserno" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="User">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblitruser" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrevreason" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrevqty" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Price per Qty ($)" HeaderStyle-Width="11%">
                                                            <ItemTemplate>
                                                                <div class="Currency-group">
                                                                    <span>$</span>
                                                                    <asp:Label ID="lblrevppqty" runat="server"></asp:Label>
                                                                </div>
                                                            </ItemTemplate>
                                                               <FooterTemplate>
                                                                <span style="color: black">Shipping Cost</span>

                                                                <br />
                                                                <br />
                                                                <span style="color: black">Tax</span>

                                                                <br />
                                                                <br />
                                                                <span style="color: black">Total Cost</span>

                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Price ($)">
                                                            <ItemTemplate>
                                                                <span>$</span>
                                                                <asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <span style="color: black">$</span>
                                                                <asp:Label ID="lblshippingcost" runat="server" ForeColor="Black"></asp:Label>
                                                                <br />
                                                                <br />
                                                                <span style="color: black">$</span>
                                                                <asp:Label ID="lbltax" runat="server" ForeColor="Black"></asp:Label>
                                                                <br />
                                                                <br />
                                                                <span style="color: black">$</span>
                                                                <asp:Label ID="lbltotalcost" runat="server" ForeColor="Black"></asp:Label>
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
                    <rsweb:ReportViewer ID="rvITRequestPOreport" runat="server"></rsweb:ReportViewer>
                </div>
                <div style="display: none">
                    <rsweb:ReportViewer ID="rvitreqreviewrpt" runat="server"></rsweb:ReportViewer>
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
                                <h4 class="modal-title font-bold text-white">RequestIT Order
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
                  <asp:HiddenField ID="HdnMSRDetailID" runat="server" />
                  <asp:HiddenField ID="HdnITDetailID" runat="server" />
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
