<%@ Page Title="Request-Work/Service" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="ServiceRequest.aspx.cs" Inherits="Inventory.ServiceRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : ServiceRequest.aspx 
' Type      : ASPX File 
' Description  :   To design the Service Request page for add,Update and show the Service Request on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  09/26/2017           V.01              Vivekanand.S, Sairam.P          New
  10/25/2017           V.01              Vivekanand.S                  Locked the record.
  02/08/2018           V.01              Vivekanand.S                  Service Request Attachemnt Mandatory while inserting the service.

'******************************************************************************************************/
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/sumoselect.css" rel="stylesheet" />
    <link href="Content/Common.css" rel="stylesheet" />
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

        .Textbox_Size {
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 12pt;
            border: 4px solid #67CFF5;
            width: 260px;
            height: 60px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }

        .close {
            margin-top: -2px;
        }

        .lable-align {
            font-weight: 600;
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

        .radiostyle input[type="radio"] {
            margin-left: 10px;
            margin-right: 1px;
        }
    </style>
    <script type="text/javascript">

        function CorpDrop() {
            $('[id*=drpcor]').change(function (event) {
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
    <script>

        function jScript() {
            $('[id*=drpcor]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: false,
                placeholder: 'Select Corporate'
            });

            $('[id*=drpfacility]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Facility'
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


        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }

        function ShowError(errors) {
            var notify = $.notify('<strong>Saving</strong> Do not close this page...', { allow_dismiss: true }, { delay: '500' }
                );

            notify.update({ type: 'error', message: '' + errors + '', progress: 20, });


        }
        function ShowSuccess(Success) {
            var notify = $.notify('<strong>Saving</strong> Do not close this page...', { allow_dismiss: true }, { delay: '500' }
                );

            notify.update({ type: 'success', message: '' + Success + '', progress: 20, });

        }
        function ShowWarning(Warning) {
            var notify = $.notify('<strong>Saving</strong> Do not close this page...', { allow_dismiss: true }, { delay: '500' }
                );

            notify.update({ type: 'warning', message: '' + Warning + '', progress: 20, });

        }


        $(function () {
            $('[id*=imgreadmore]').on('mouseover', function () {
                var a = "Click here to read more";
                $('[id*=imgreadmore]').attr('title', a);
            })
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
            //alert('ggg');
            $('[id*=lblwarning]').html(res);

            $("#modalWarning").modal("show");

        }

        function ShowConfirmationPopup() {

            $("#modalConfirm").modal("show");

        }

        <%--var SetTime;

        function TimeOut() {
          SetTime =  setTimeout(
                function ShowCurrentTime() {
                    $.ajax({
                        type: "POST",
                        url: "ServiceRequest.aspx/AutoUpdateLockedOut",
                        data: '{ ServiceMasterId: "' + $("#<%=HddMasterID.ClientID%>")[0].value + '", UserID:"' + $("#<%=HddUserID.ClientID%>")[0].value + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        ShowwarningLookupPopup(response.d);
                    }
                });
            },
            20000);
        }

        function OnSuccess(response) {            
            ShowwarningLookupPopup(response.d);            
        }

        function StopTimer() {
            clearTimeout(SetTime);            
        }--%>



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Service Request
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
                <asp:HiddenField ID="hdnSRMasterID" runat="server" />
                <asp:HiddenField ID="hdnServicecat" Value="0" runat="server" />
                <asp:HiddenField ID="hdnServicelist" Value="0" runat="server" />
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
                <div id="UploadOpacity" runat="server">
                    <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                        <div class="row">
                            <div class="col-lg-4" align="left">
                                <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Header"></asp:Label>
                                <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                            </div>
                            <div class="col-lg-8" align="right">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyField" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnSave" Visible="false" runat="server" CssClass="btn btn-primary" ValidationGroup="EmptyFieldSave" Text="Review" OnClick="btnSave_Click" />
                                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                            </div>
                        </div>
                        <div id="divSRMaster" runat="server" style="margin-top: 5px;">
                            <div id="divContent" class="well well-sm">
                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                        <asp:LinkButton ID="lnkMultiCorp" runat="server" Text="Multi Select" CssClass="LeftPadding" OnClick="lnkMultiCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearCorp" runat="server" Text="Select All" CssClass="LeftPadding" OnClick="lnkClearCorp_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkClearAllCorp" runat="server" Text="Clear All" CssClass="LeftPadding" OnClick="lnkClearAllCorp_Click"></asp:LinkButton>
                                            <asp:ListBox ID="drpcor" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:ListBox>   
                                             <asp:RequiredFieldValidator InitialValue="" ID="ReqdrdddlCorporate" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                ControlToValidate="drpcor" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic">></asp:RequiredFieldValidator>                                         
                                            <%--<asp:DropDownList ID="drpcor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:DropDownList>--%>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                            <asp:ListBox ID="drpfacility" runat="server" CssClass="form-control" SelectionMode="Multiple" OnSelectedIndexChanged="drpfacility_SelectedIndexChanged"></asp:ListBox> 
                                             <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator1" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                ControlToValidate="drpfacility" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <%--<asp:DropDownList ID="drpfacility" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpfacility_SelectedIndexChanged"></asp:DropDownList>--%>
                                        </div>

                                    </div>
                                    <%--<div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldValidator2" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                ControlToValidate="drpvendor" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="drpvendor" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>--%>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Status</span>&nbsp;<span style="color: red">*</span>     
                                            <asp:ListBox ID="drpStatus" runat="server" CssClass="form-control" SelectionMode="Multiple" ></asp:ListBox> 
                                              <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator3" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                ControlToValidate="drpStatus" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic">></asp:RequiredFieldValidator>
                                            <%--<asp:DropDownList ID="drpStatus" runat="server" CssClass="form-control"></asp:DropDownList>--%>
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
                                                ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic">></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Date To</span>&nbsp;<span style="color: red">*</span>
                                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" placeholder="--/--/----"></asp:TextBox>
                                            <ajax:CalendarExtender ID="CalDateTo" runat="server" TargetControlID="txtDateTo" Format="MM/dd/yyyy"></ajax:CalendarExtender>
                                            <ajax:FilteredTextBoxExtender ID="FilterDateTo" FilterType="Numbers,Custom" ValidChars="/,-" runat="server" TargetControlID="txtDateTo"></ajax:FilteredTextBoxExtender>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDateTo" ValidationGroup="EmptyField"
                                                ErrorMessage="This information is required" ForeColor="Red" SetFocusOnError="true" Display="Dynamic">></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="SRMasterGrid" style="margin-left: 1px; margin-top: 3px;" id="divvendor">
                                <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title" Text="Search Result"></asp:Label><br />
                                 <asp:Label ID="lblrcount" runat="server">No of records : <%=grdSRMaster.Rows.Count.ToString() %></asp:Label>
                                <div class="SRSearchgrid">
                                    <asp:GridView ID="grdSRMaster" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnRowDataBound="grdSRMaster_RowDataBound" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/edit.png" OnClick="imgbtnEdit_Click" />
                                                    <asp:ImageButton ID="imgprint" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgprint_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:BoundField DataField="CreatedOn" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="10%" />
                                            <asp:BoundField DataField="CorporateName" HeaderText="Corp" HeaderStyle-Width="8%"/>
                                            <asp:BoundField DataField="FacilityShortName" HeaderText="Facility" HeaderStyle-Width="8%" />
                                            <asp:BoundField DataField="SRNo" HeaderText="SR NO" />
                                            <%--<asp:BoundField DataField="VendorDescription" HeaderText="Vendor" />--%>
                                            <asp:BoundField DataField="Service" HeaderText="Service" />
                                            <asp:BoundField DataField="Status" HeaderText="Status"/>
                                            <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <%--<asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />--%>
                                            <asp:BoundField DataField="ServiceCategoryID" HeaderText="ServiceCategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="ServiceListID" HeaderText="ServiceListID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="ServiceRequestMasterID" HeaderText="ServiceRequestMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="ServiceType" HeaderText="ServiceType" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="EquipmentCategoryID" HeaderText="ServiceCategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="EquipementSubCategoryID" HeaderText="EquipementSubCategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="EquipementListID" HeaderText="ServiceListID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblCreatedBy" runat="server" Text=' <%# Eval("CreatedBy")%>'></asp:Label>--%>
                                                    <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="6%">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblRemarks" runat="server" Text=' <%# Eval("Remarks")%>'></asp:Label>--%>
                                                    <asp:Image ID="imgreadmore" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Remarks")%>' data-toggle="popover" />
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
                            <asp:HiddenField ID="hdncheckfield" Value="0" runat="server" />
                        </div>

                        <div id="divSRDetails" runat="server" style="margin-top: 5px; display: none">
                            <div runat="server" style="margin-left: 1px; margin-top: 3px; display: none;" id="divEdit">
                                 <asp:Label ID="lblrcount1" runat="server">No of records : <%=GvTempEdit.Rows.Count.ToString() %></asp:Label>
                                <div class="SREditgrid">
                                <asp:GridView ID="GvTempEdit" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" OnRowDataBound="grdSRMaster_RowDataBound" CssClass="table table-responsive">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Edit"  HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/edit.png" OnClick="imgbtnEdit_Click" />
                                                <asp:ImageButton ID="imgprint" runat="server" Text="Edit" Height="20px" ImageUrl="~/Images/Print.png" OnClick="imgprint_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreatedOn" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="10%" />
                                        <asp:BoundField DataField="CorporateName" HeaderText="Corp"  HeaderStyle-Width="8%"/>
                                        <asp:BoundField DataField="FacilityShortName" HeaderText="Facility"  HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="SRNo" HeaderText="SR NO" />
                                        <%--<asp:BoundField DataField="VendorDescription" HeaderText="Vendor" />--%>
                                        <asp:BoundField DataField="Service" HeaderText="Service" />
                                        <asp:BoundField DataField="Status" HeaderText="Status"/>
                                        <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <%--<asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />--%>
                                        <asp:BoundField DataField="ServiceCategoryID" HeaderText="ServiceCategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="ServiceListID" HeaderText="ServiceListID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="ServiceRequestMasterID" HeaderText="ServiceRequestMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="ServiceType" HeaderText="ServiceType" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="EquipmentCategoryID" HeaderText="ServiceCategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="EquipementSubCategoryID" HeaderText="EquipementSubCategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:BoundField DataField="EquipementListID" HeaderText="ServiceListID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                        <asp:TemplateField HeaderText="Audit Trail" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblCreatedBy" runat="server" Text=' <%# Eval("CreatedBy")%>'></asp:Label>--%>
                                                <asp:Image ID="imgreadmore1" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Audit") %>' data-toggle1="popover" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblRemarks" runat="server" Text=' <%# Eval("Remarks")%>'></asp:Label>--%>
                                                <asp:Image ID="imgreadmore" runat="server" ImageUrl="~/Images/Readmore.png" ImageAlign="Right" Height="40px" data-content='<%# Eval("Remarks")%>' data-toggle="popover" />
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
                            <div>
                                <asp:Label ID="lblMasterHeader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>
                                <div id="DivServiceMasterNo" runat="server" style="display: none;">
                                    <span style="font-weight: 800;">Service Request Number :- </span>
                                    <asp:Label ID="lblServiceMasterNo" runat="server" ForeColor="Red" CssClass="page-title lable-align" Text=""></asp:Label>
                                </div>
                                <div id="divContentDetails" class="well well-sm">
                                    <div class="row">
                                        <div class="col-sm-4 col-md-4 col-lg-4">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                    ControlToValidate="ddlCorporate" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlCorporate" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCorporate_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Facility</span>&nbsp;<span style="color: red">*</span>
                                                <asp:RequiredFieldValidator InitialValue="0" ID="ReqdrdddlFacility" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                    ControlToValidate="ddlFacility" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlFacility" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged"></asp:DropDownList>
                                            </div>

                                        </div>
                                        <%--<div class="col-sm-3 col-md-3 col-lg-3">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Vendor</span>&nbsp;<span style="color: red">*</span>
                                                <asp:RequiredFieldValidator InitialValue="0" ID="ReqdrdddlVendor" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                    ControlToValidate="ddlVendor" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>--%>
                                        <div class="col-sm-4 col-md-4 col-lg-4">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Service Type</span>&nbsp;<span style="color: red;">*</span>
                                                <asp:RequiredFieldValidator ID="ReqrdoServicetype" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                    ControlToValidate="rdbServiceType" ErrorMessage="This information is required." SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:RadioButtonList ID="rdbServiceType" runat="server" RepeatDirection="Horizontal" CssClass="radiostyle" AutoPostBack="true" OnSelectedIndexChanged="rdbServiceType_SelectedIndexChanged">
                                                    <asp:ListItem Text="Building" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Equipment" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-4 col-md-4 col-lg-4">
                                            <div id="DivServiceCategory" runat="server" class="form-group" style="display: none;">
                                                <span style="font-weight: 800;">Service Category</span>&nbsp;<span style="color: red">*</span>
                                                <asp:ImageButton ID="imgeservicecatadd" runat="server" Height="17px" ToolTip="Add Service Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Add.png" OnClick="imgeservicecatadd_Click" />
                                                <asp:ImageButton ID="imgeservicecatedit" runat="server" Height="17px" ToolTip="Edit Service Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/edit.png" OnClick="imgeservicecatedit_Click" />
                                                <asp:ImageButton ID="imgeservicecatdelete" runat="server" Height="17px" ToolTip="Delete Service Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClick="imgeservicecatdelete_Click" />
                                                <asp:DropDownList ID="ddlServiceCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlServiceCategory_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="ReqdrdddlServiceCategory" InitialValue="0" runat="server" ForeColor="Red" Visible="false" ValidationGroup="EmptyFieldSave"
                                                    ControlToValidate="ddlServiceCategory" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            </div>
                                            <div id="DivEquipmentCategory" runat="server" class="form-group" style="display: none;">
                                                <span style="font-weight: 800;">Equipment Category</span>&nbsp;<span style="color: red">*</span>
                                                <asp:DropDownList ID="ddlEquipmentCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEquipmentCategory_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="ReqdrdddlEquipmentCategory" InitialValue="0" runat="server" ForeColor="Red" Visible="false" ValidationGroup="EmptyFieldSave"
                                                    ControlToValidate="ddlEquipmentCategory" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4">
                                            <div id="DivServiceList" runat="server" class="form-group" style="display: none;">
                                                <span style="font-weight: 800;">Service List</span>&nbsp;<span style="color: red">*</span>
                                                <asp:ImageButton ID="imgeservicelistadd" runat="server" Height="17px" ToolTip="Add ServiceList" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Add.png" OnClick="imgeservicelistadd_Click" />
                                                <asp:ImageButton ID="imgeservicelistedit" runat="server" Height="17px" ToolTip="Edit ServiceList" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/edit.png" OnClick="imgeservicelistedit_Click" />
                                                <asp:ImageButton ID="imgeservicelistdelete" runat="server" Height="17px" ToolTip="Delete ServiceList" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClick="imgeservicelistdelete_Click" />
                                                <asp:DropDownList ID="ddlServiceList" runat="server" CssClass="form-control"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="ReqdrdddlServiceList" InitialValue="0" runat="server" ForeColor="Red" Visible="false" ValidationGroup="EmptyFieldSave"
                                                    ControlToValidate="ddlServiceList" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            </div>
                                            <div id="DivEquipmentSubCat" runat="server" class="form-group" style="display: none;">
                                                <span style="font-weight: 800;">Equipment Sub Category</span>&nbsp;<span style="color: red">*</span>
                                                <asp:DropDownList ID="ddlEquipmentSubCat" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEquipmentSubCat_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="ReqdrdddlEquipmentSubCat" InitialValue="0" runat="server" ForeColor="Red" Visible="false" ValidationGroup="EmptyFieldSave"
                                                    ControlToValidate="ddlEquipmentSubCat" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4">
                                            <div id="DivEquipmentList" runat="server" class="form-group" style="display: none;">
                                                <span style="font-weight: 800;">Equipment List</span>&nbsp;<span style="color: red">*</span>
                                                <asp:DropDownList ID="ddlEquipmentList" runat="server" CssClass="form-control"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="ReqdrdddlEquipmentList" InitialValue="0" runat="server" ForeColor="Red" Visible="false" ValidationGroup="EmptyFieldSave"
                                                    ControlToValidate="ddlEquipmentList" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField ID="HddMasterID" runat="server" />
                            <asp:HiddenField ID="HddDetailsID" runat="server" />
                            <asp:HiddenField ID="HddUserID" runat="server" />
                            <asp:HiddenField ID="HddUpdateLockinEdit" runat="server" />
                            <asp:HiddenField ID="HddFacilityID" runat="server" />
                            <asp:HiddenField ID="HddRowIndex" runat="server" />
                             <asp:HiddenField ID="HddListCorpID" runat="server" />                     
                            <div runat="server" style="margin-left: 1px; margin-top: 3px; display: none;" id="divSearchMachine">
                                <%--<asp:Label ID="lblItemHeader" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label><br />--%>
                                <asp:Button runat="server" Text="Add New Row" CssClass="btn btn-primary" ID="btnSearchNewRow" OnClick="btnSearchNewRow_Click" /><br />
                               <asp:Label ID="lblrcount2" runat="server">No of records : <%=gvSearchSRDetails.Rows.Count.ToString() %></asp:Label>
                                <div id="SREditQuerygrid" runat="server" class="SREditgrid">
                                    <asp:GridView ID="gvSearchSRDetails" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnSearchDelete" runat="server" Text="Delete" Height="20px" ImageUrl="~/Images/Delete.png" OnClick="btnSearchDelete_Click" />
                                                    <asp:Label ID="lbSRMasterID" runat="server" Text='<%# Eval("ServiceRequestMasterID") %>' CssClass="HeaderHide"></asp:Label>
                                                    <asp:Label ID="lbSRDetailsID" runat="server" Text='<%# Eval("ServiceRequestDetailsID") %>' CssClass="HeaderHide"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="btnSaveRow" runat="server" Width="18px" Height="20px" ImageUrl="~/Images/save.png" OnClick="btnSaveRow_Click" />
                                                    <asp:ImageButton ID="btnRemoveRow" runat="server" Width="18px" Height="20px" ImageUrl="~/Images/Delete.png" OnClick="btnRemoveRow_Click" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vendor">
                                                <ItemTemplate>
                                                    <%--<asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control input-sm" AppendDataBoundItems="true"></asp:DropDownList>--%>
                                                    <asp:Label ID="lblVendor" runat="server" Text='<%# Eval("VendorName") %>' CssClass="form-control"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlFootVendor" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Service" runat="server" Text='<%# Eval("Service") %>' CssClass="Textbox_Size" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="FootService" runat="server" CssClass="Textbox_Size" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Unit" runat="server" Text='<%# Eval("Unit") %>' CssClass="Textbox_Size" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="FootUnit" runat="server" CssClass="Textbox_Size" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price($)">
                                                <ItemTemplate>
                                                    <div class="Currency-group">
                                                        <span class="Currency-group-addon">
                                                            <i class="fa fa-dollar"></i>
                                                        </span>
                                                        <asp:TextBox ID="Price" runat="server" Text='<%# Eval("Price", "{0:F2}") %>' CssClass="Textbox_Size" />
                                                        <ajax:FilteredTextBoxExtender ID="FTBPrice" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="Price"></ajax:FilteredTextBoxExtender>
                                                    </div>

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div class="Currency-group">
                                                        <span class="Currency-group-addon">
                                                            <i class="fa fa-dollar"></i>
                                                        </span>
                                                        <asp:TextBox ID="FootPrice" runat="server" CssClass="Textbox_Size" DataFormatString="{0:F2}" />
                                                        <ajax:FilteredTextBoxExtender ID="FTBFootPrice" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="FootPrice"></ajax:FilteredTextBoxExtender>
                                                    </div>

                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="hideID">
                                        <ItemTemplate>
                                           
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Quote" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnImgUploadQuote" runat="server" Height="20px" ImageUrl="~/Images/Attachment.png" OnClick="btnImgUploadQuote_Click" />
                                                    <asp:Label ID="lbServiceRequestMasterID" runat="server" Text='<%# Eval("ServiceRequestMasterID") %>' CssClass="HeaderHide"></asp:Label>
                                                    <asp:Label ID="lbServiceRequestDetailsID" runat="server" Text='<%# Eval("ServiceRequestDetailsID") %>' CssClass="HeaderHide"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="btnImgUploadQuote" runat="server" Height="20px" ImageUrl="~/Images/Attachment.png" OnClick="btnImgUploadQuote_Click" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="ServiceRequestMasterID" HeaderText="ServiceRequestMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide"/>
                                    <asp:BoundField DataField="ServiceRequestDetailsID" HeaderText="ServiceRequestDetailsID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide"/>  --%>
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

                            <div class="row" runat="server" style="margin-left: 1px; margin-top: 3px; display: none;" id="divAddMachine">
                                <%--<asp:Button runat="server" Text="Check" CssClass="btn-primary" ID="btn_Check" OnClick="btn_Check_Click" />--%>
                                <asp:Label ID="lblAddItemHeader" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label><br />
                                <asp:Button runat="server" Text="Add New Row" CssClass="btn btn-primary" ID="btn_New" OnClick="btn_New_Click" /><br />
                                   <asp:Label ID="lblrcount3" runat="server">No of records : <%=gvAddSRDetails.Rows.Count.ToString() %></asp:Label>
                                <div class="SRAddgrid">
                                    <asp:GridView ID="gvAddSRDetails" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                                        <Columns>
                                            <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false" />
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnAddDelete" runat="server" Text="Delete" Height="20px" ImageUrl="~/Images/Delete.png" OnClick="btnAddDelete_Click" />
                                                    <asp:Label ID="lblSINo" runat="server" Text='<%# Container.DataItemIndex + 1 %>' CssClass="HeaderHide"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="SINo">
                                        <ItemTemplate>
                                            <asp:Label ID="SINo" runat="server" CssClass="Textbox_Size" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Vendor">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlVendor" runat="server" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Service" runat="server" CssClass="Textbox_Size" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="Unit" runat="server" CssClass="Textbox_Size" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price($)">
                                                <ItemTemplate>
                                                    <div class="Currency-group">
                                                        <span class="Currency-group-addon">
                                                            <i class="fa fa-dollar"></i>
                                                        </span>
                                                        <asp:TextBox ID="Price" runat="server" CssClass="Textbox_Size" DataFormatString="{0:F2}" />
                                                        <ajax:FilteredTextBoxExtender ID="FTBPrice" FilterType="Numbers,Custom" ValidChars="." runat="server" TargetControlID="Price"></ajax:FilteredTextBoxExtender>
                                                    </div>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quote">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnImgUploadQuote" runat="server" Height="20px" ImageUrl="~/Images/Upload.png" OnClick="btnImgUploadQuote_Click" />
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
                                <h4 class="modal-title font-bold text-white">Service Request
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="lblsave" runat="server"></asp:Label><asp:LinkButton ID="lbpopprint" runat="server" Text="Print" OnClick="btnPrint_Click" Visible="false"></asp:LinkButton>
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
                                <h4 class="modal-title font-bold text-white">Service Request
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
                                <h4 class="modal-title font-bold text-white">Service Request
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

                <div class="loading" style="padding-left: 10px;">
                    <img src="Images/LoadingCircle.gif" alt="" style="height: 50px;" />
                    Loading..Please wait...                                                                                
                </div>
                <div class="outPopUp" runat="server" style="margin-left: 1px; margin-top: 3px; display: none; padding: 5px 5px 5px 10px;" id="divUploadFile">
                    <asp:UpdatePanel runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnuploadfile" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:Label ID="lblUPServicefile" runat="server" CssClass="page-header page-title" Text="Quote"></asp:Label>
                    <asp:HiddenField ID="hdnattachment" runat="server" />
                    <asp:GridView ID="GrdUploadFile" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="table table-responsive">
                        <Columns>
                            <asp:BoundField DataField="ServiceUploadID" HeaderText="ServiceUploadID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="ServiceRequestDetailsID" HeaderText="ServiceRequestDetailsID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="LocationOfTheFile" HeaderText="Location Of The File" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="FileName" HeaderText="FileName" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="UploadedDateTime" HeaderText="Uploaded DateTime" />
                            <asp:BoundField DataField="UploadedByName" HeaderText="Uploaded By" />

                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnImgView" runat="server" Width="18px" Height="20px" ImageUrl="~/Images/View.png" OnClick="btnImgView_Click" />
                                    <asp:ImageButton ID="btnImgDeleteQuote" runat="server" Width="18px" Height="20px" ImageUrl="~/Images/Delete.png" OnClick="btnImgDeleteQuote_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="ServiceRequestMasterID" HeaderText="ServiceRequestMasterID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide"/>
                                    <asp:BoundField DataField="ServiceRequestDetailsID" HeaderText="ServiceRequestDetailsID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide"/>  --%>
                        </Columns>
                        <HeaderStyle CssClass="Headerstyle" />
                        <FooterStyle CssClass="gridfooter" />
                        <PagerStyle CssClass="pagerstyle" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle CssClass="gridselectedrow" />
                        <EditRowStyle CssClass="grideditrow" />
                        <AlternatingRowStyle CssClass="gridalterrow" />
                        <RowStyle CssClass="gridrow" />
                    </asp:GridView>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <asp:FileUpload ID="Fileuploadfile" runat="server" />
                        </div>
                    </div>
                    <div class="row" style="margin-top: 5px;">
                        <div class="col-lg-6 col-md-6 col-sm-6 form-group">
                            <asp:Label ID="lblDescrip" runat="server" Text="Description" Font-Bold="true" CssClass="page-title"></asp:Label><br />
                            <asp:TextBox ID="txtDescrip" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 " align="right">
                            <br />
                            <asp:Button ID="btnuploadfile" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnuploadfile_Click" OnClientClick="ShowProgress();" />
                            <asp:Button ID="btnupclose" runat="server" Text="Close" CssClass="btn btn-success" OnClick="btnupclose_Click" />
                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>




        <asp:UpdatePanel ID="upnlServiceCat" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnServiceCat" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeServiceCat" runat="server"
                    PopupControlID="modalAddServiceCat" TargetControlID="btnServiceCat"
                    BackgroundCssClass="modalBackground" BehaviorID="modalAddServiceCat" CancelControlID="btnpopupclose">
                </ajax:ModalPopupExtender>
                <div id="modalAddServiceCat" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="btnpopupclose" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: green; font-size: large">Service Category</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Panel runat="server" ID="btnPanelSC" DefaultButton="btnaddservicecategory">
                                    <div style="height: 40px">
                                        <div class="form-horizontal">
                                            <div class="col-md-6 col-sm-6">
                                                <span>Service Category</span>
                                                <asp:TextBox ID="txtServiceCategory" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnaddservicecategory" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnaddservicecategory_Click" />
                                <asp:Button ID="btnservicecategoryclose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnservicecategoryclose_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnaddservicecategory" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upnlServiceList" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnServiceList" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeServiceList" runat="server"
                    PopupControlID="modalAddServiceList" TargetControlID="btnServiceList"
                    BackgroundCssClass="modalBackground" BehaviorID="modalAddServiceList" CancelControlID="btnpopupclose1">
                </ajax:ModalPopupExtender>
                <div id="modalAddServiceList" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="btnpopupclose1" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: green; font-size: large">Service List</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Panel runat="server" ID="btnPanelSL" DefaultButton="btnaddserviceList">
                                    <div style="height: 40px">
                                        <div class="form-horizontal">
                                            <div class="col-md-6 col-sm-6">
                                                <span>Service List</span>
                                                <asp:TextBox ID="txtServiceList" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnaddserviceList" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnaddserviceList_Click" />
                                <asp:Button ID="btnservicelistclose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnservicelistclose_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnaddserviceList" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upnlview" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnmpedemo" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeimgview" runat="server"
                    TargetControlID="btnmpedemo" PopupControlID="pnlimgview"
                    BackgroundCssClass="modalBackground">
                </ajax:ModalPopupExtender>
                <%--   <div id="pnlimgview" style="display: none;">--%>
                <asp:Panel ID="pnlimgview" Style="display: none;" runat="server">
                    <div style="position: fixed; top: 0; right: 0; bottom: 0; left: 0; z-index: 1040; overflow: auto; overflow-y: scroll; margin-top: 1%;">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header" style="height: 40px">
                                    <asp:Button ID="btnpopclose" runat="server" CssClass="close" Text="X" OnClick="btnpopclose_Click" />
                                    <asp:Label runat="server" Text="Document Viewer" ID="lblimg" />
                                </div>
                                <div class="modal-body" style="padding: 2px;">
                                    <div class="container-fluid" runat="server" id="Div2">
                                        <%-- <asp:Image ID="vwimg" runat="server" CssClass="img-responsive" />--%>
                                        <iframe id="frame1" runat="server" style="height: 530px; width: 100%;" onscroll="auto"></iframe>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--</div>--%>
                </asp:Panel>

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
                        <a href="javascript:void(0)" onclick="closepopup()">
                            <img src="Images/Close.gif" style="border: 0px" align="right" /></a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                        <asp:Label ID="Label2" runat="server" Text="Do you want To delete this record ?" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right" style="padding-right: 15px">
                        <asp:ImageButton ID="btnYes" runat="server" ImageUrl="~/Images/btnyes.jpg" OnClick="btnYes_Click" />
                        <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/Images/btnNo.jpg" />
                    </td>
                </tr>
            </table>
        </asp:Panel>


        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnSerReqreview" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeSerRequReview" runat="server"
                    PopupControlID="mpeSerReqreview" TargetControlID="btnSerReqreview"
                    BackgroundCssClass="modalBackground" BehaviorID="mpeSerRequReview">
                </ajax:ModalPopupExtender>
                <div id="mpeSerReqreview" style="display: none;">
                    <div class="modal-dialog-Review">
                        <div class="modal-content">
                            <div class="modal-header" runat="server" style="padding: 8px 5px 0px 5px;">
                                <asp:Button ID="btnrevclose" runat="server" CssClass="close" Text="X" OnClick="btnreviewcancel_Click" />
                                <h4 class="modal-title" style="color: black; font-size: large">Service Request Review</h4>
                            </div>
                            <div class="modal-body" style="padding: 5px 15px 15px 15px;">
                                <div class="form-horizontal">
                                    <div class="row" style="margin-bottom: 2px;">
                                        <div class="col-sm-9 col-md-9 col-lg-9">
                                            <asp:Label ID="lblpopupheader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>
                                        </div>
                                        <div class="col-sm-3 col-md-3 col-lg-3" id="div16" runat="server" align="right">
                                            <asp:Button ID="btnSaveReview" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSaveReview_Click" OnClientClick="StopTimer();" />
                                            <asp:Button ID="btnreviewcancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnreviewcancel_Click" />
                                        </div>
                                    </div>
                                    <div runat="server" id="DivPopupSRNo">
                                        <span style="font-weight: 800;">Service Request Number :- </span>
                                        <asp:Label ID="lblhead" runat="server" ForeColor="Red" CssClass="page-title lable-align" Text=""></asp:Label>
                                    </div>
                                    <div class="well well-sm" style="padding: 5px 15px 15px 25px;">
                                        <div class="row">
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div1" runat="server">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Corporate</span>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div3" runat="server">
                                                <div class="form-group">
                                                    <asp:Label ID="lblreviewcorporate" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div4" runat="server">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Facility</span>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div5" runat="server">
                                                <div class="form-group">
                                                    <asp:Label ID="lblreviewfacility" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div6" runat="server">
                                                <div class="form-group">
                                                    <span style="font-weight: 800;">Service Type</span>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div7" runat="server">
                                                <div class="form-group">
                                                    <asp:Label ID="lblServiceType" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div8" runat="server">
                                                <div id="DivreviewEC" runat="server" class="form-group" style="display: none;">
                                                    <span style="font-weight: 800;">Equipment Category</span>
                                                </div>
                                                <div id="DivreviewSC" runat="server" class="form-group" style="display: none;">
                                                    <span style="font-weight: 800;">Service Category</span>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div9" runat="server">
                                                <div id="DivreviewLEC" runat="server" class="form-group" style="display: none;">
                                                    <asp:Label ID="lblEquipmentCategory" runat="server"></asp:Label>
                                                </div>
                                                <div id="DivreviewLSC" runat="server" class="form-group" style="display: none;">
                                                    <asp:Label ID="lblServiceCategory" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div10" runat="server">
                                                <div id="DivreviewESC" runat="server" class="form-group" style="display: none;">
                                                    <span style="font-weight: 800;">Equipement Sub Category</span>
                                                </div>
                                                <div id="DivreviewSL" runat="server" class="form-group" style="display: none;">
                                                    <span style="font-weight: 800;">Service List</span>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div11" runat="server">
                                                <div id="DivreviewLESC" runat="server" class="form-group" style="display: none;">
                                                    <asp:Label ID="lblEquipementEquipSubCat" runat="server"></asp:Label>
                                                </div>
                                                <div id="DivreviewLSL" runat="server" class="form-group" style="display: none;">
                                                    <asp:Label ID="lblServiceList" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div12" runat="server">
                                                <div id="DivreviewEL" runat="server" class="form-group" style="display: none;">
                                                    <span style="font-weight: 800;">Equipement List</span>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3" id="div13" runat="server">
                                                <div id="DivreviewLEL" runat="server" class="form-group" style="display: none;">
                                                    <asp:Label ID="lblEquipementList" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="HddAutoServiceMasterNo" runat="server" Value="" />
                                    <asp:Label ID="lblpopupitems" runat="server" CssClass="page-header page-title" Text="Items"></asp:Label>
                                    <div class="row">
                                        <div class="col-lg-12">
                                              <asp:Label ID="Label6" runat="server">No of records : <%=GvServiceRequestReview.Rows.Count.ToString() %></asp:Label>
                                            <div class=" SRReviewgrid">
                                                <asp:GridView ID="GvServiceRequestReview" runat="server" CssClass="table table-responsive" AutoGenerateColumns="false" ShowHeader="true"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Column1" HeaderText="Vendor" />
                                                        <asp:BoundField DataField="Column2" HeaderText="Service" />
                                                        <asp:BoundField DataField="Column3" HeaderText="Unit" />
                                                        <asp:BoundField DataField="Column4" HeaderText="Price($)" DataFormatString="$ {0:F2}" />
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

        <div style="display: none">
            <rsweb:ReportViewer ID="rvservicerequestreport" runat="server"></rsweb:ReportViewer>
        </div>
</asp:Content>
