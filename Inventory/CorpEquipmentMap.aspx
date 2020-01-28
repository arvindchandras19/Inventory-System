<%@ Page Title="Corp Equipment Map" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="CorpEquipmentMap.aspx.cs" Inherits="Inventory.CorpEquipmentMap" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : CorpEquipmentMap.aspx 
' Type      : ASPX File 
' Description  :   To design the Corporate Equipment Map page for add,Update and show the Corporate Equipment on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  12/12/2017           V.01              Vivekanand.S                   New

'******************************************************************************************************/
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        .outPopUp {
            position: absolute;
            width: 900px;
            max-height: 300px;
            z-index: 15;
            top: 30%;
            left: 20%;
            margin: -100px 0 0 -150px;
            background: #f1f1f1;
            /*border: inset;
            border-style: solid;*/
            /*-webkit-transform: rotate(8deg);
            -moz-transform: rotate(8deg);
            -o-transform: rotate(8deg);
            -ms-transform: rotate(8deg);
            transform: rotate(8deg);
            right: 5px;
            left: auto;*/
            /*box-shadow: 10px 10px 10px #000;*/
            box-shadow: 10px 10px 10px rgba(0, 0, 0, 0.5);
            /*-moz-box-shadow: 5px 5px 5px rgba(0,0,0,0.3);
            -webkit-box-shadow: 5px 5px 5px rgba(0,0,0,0.3);
            box-shadow: 5px 5px 5px rgba(0,0,0,0.3);*/
            /*Opacity*/
            /*-khtml-opacity: .50;
            -moz-opacity: .50;
            filter: alpha(opacity=50);
            filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0.5);
            opacity: .50;*/
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

    <script>
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


        $(document).ready(function () {
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


        $(document).ready(function () {
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
    <%--<form runat="server">--%>
        <%--<ajax:ToolkitScriptManager ID="script11" runat="server" EnablePageMethods="true"></ajax:ToolkitScriptManager>--%>
        
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Corp Equipment Map
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hdnSRMasterID" runat="server" />
                <asp:HiddenField ID="hdnServicecat" Value="0" runat="server" />
                <asp:HiddenField ID="hdnServicelist" Value="0" runat="server" />
                <div id="UploadOpacity" runat="server">
                    <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                        <div class="row">
                            <div class="col-lg-4" align="left">
                                <asp:Label ID="lblEditHeader" runat="server" Visible="false" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                <asp:Label ID="lblUpdateHeader" runat="server" Visible="false" CssClass="page-header page-title" Text=""></asp:Label>
                                <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                            </div>
                            <div class="col-lg-8" align="right">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnSave" Visible="false" runat="server" CssClass="btn btn-success" ValidationGroup="EmptyFieldSave" Text="Save" OnClick="btnSave_Click" />
                                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                            </div>
                        </div>
                        <div id="divSRMaster" runat="server" style="margin-top: 5px;">
                            <div id="divContent" class="well well-sm">
                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Corporate</span>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqdrdddlCorporate" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                ControlToValidate="drpcor" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                            <asp:DropDownList ID="drpcor" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Equipment Category/Sub Category</span>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                                ControlToValidate="txtEquipmentSearch" ErrorMessage="This information is required."></asp:RequiredFieldValidator>--%>
                                            <%-- <div class="input-group">--%>
                                            <asp:TextBox ID="txtEquipmentSearch" runat="server" CssClass="form-control" placeholder="Equipment Search ...."></asp:TextBox>
                                            <%--<span class="input-group-addon">
                                                    <i class="fa fa-search"></i>
                                                </span>--%>
                                            <%--</div>--%>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Status</span>
                                            <asp:RadioButtonList ID="rdbstatus" runat="server" RepeatDirection="Horizontal" CssClass="radiostyle">
                                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="SRMasterGrid" style="margin-left: 1px; margin-top: 3px;" id="divvendor">
                                <asp:Label ID="btnSearchHeader" runat="server" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                <div>
                                    <asp:Label ID="lblrowcount" runat="server">No of records : <%=GrdEquipmentSearch.Rows.Count.ToString() %></asp:Label>
                                </div>
                                <div class="Itemcategrid">
                                    <asp:HiddenField ID="HddEquipmentCategory" runat="server" />
                                    <asp:HiddenField ID="HddEquipmentCategoryID" runat="server" />
                                    <asp:HiddenField ID="HddEquiSubCatList" runat="server" />
                                    <asp:HiddenField ID="HddEquiCancel" runat="server" Value="0" />
                                    <asp:GridView ID="GrdEquipmentSearch" runat="server" AutoGenerateColumns="false" EmptyDataText="No Records" ShowHeaderWhenEmpty="true" CssClass="table table-responsive" OnRowDataBound="GrdEquipmentSearch_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lbEquipmentSearchedit" runat="server" Text="Edit" Height="20px" OnClick="lbEquipmentSearchedit_Click" ImageUrl="~/Images/edit.png" />
                                                    <asp:ImageButton ID="btndeleteimg" runat="server" Height="20px" ToolTip="Delete" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClick="btndeleteimg_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EquipementSubCategoryID" HeaderText="EquipementListID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="EquipmentCategoryID" HeaderText="EquipmentCategoryID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                            <asp:BoundField DataField="CorporateName" HeaderText="Corporate" />
                                            <asp:BoundField DataField="EquipmentCatDescription" HeaderText="Equipment Category" />
                                            <asp:BoundField DataField="EquipmentSubCategoryDescription" HeaderText="Equipment Sub Category" />

                                            <%-- <asp:TemplateField HeaderText="Active">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkActive"
                                                        Checked='<%#Convert.ToBoolean( Eval("IsActive").ToString() )%>' AutoPostBack="true" OnCheckedChanged="chkActive_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Active">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActive" runat="server" Text='<%# ((string)Eval("IsActive").ToString() == "True" ? "Yes" : "No")  %>'></asp:Label>
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
                            <div class="SREditgrid" runat="server" style="margin-left: 1px; margin-top: 3px; display: none;" id="divEdit">
                            </div>
                            <div>
                                <asp:Label ID="lblMasterHeader" runat="server" CssClass="page-header page-title" Text="Header"></asp:Label>
                                <div id="divContentDetails" class="well well-sm">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Corporate</span>&nbsp;<span style="color: red">*</span>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" InitialValue="0" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                    ControlToValidate="ddlCorporate" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlCorporate" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Equipment Category</span>&nbsp;<span style="color: red">*</span>
                                                <asp:RequiredFieldValidator ID="ReqdrdddlFacility" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                    ControlToValidate="txtEquipmentCategory" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtEquipmentCategory" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-sm-5 col-md-5 col-lg-5" style="padding-right: 0px">
                                            <div class="form-group">
                                                <span style="font-weight: 800;">Equipment Sub Category</span>&nbsp;<span style="color: red;">*</span>
                                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                    ControlToValidate="txtEquipmentSubCategory" ErrorMessage="This information is required." Visible="false" ></asp:RequiredFieldValidator>--%>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtEquipmentSubCategory" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <i class="fa fa-floppy-o"></i>
                                                    </span>
                                                </div>
                                                <%--<div class="form-group">
                                                <span style="font-weight: 800;">Equipment Sub Category</span>&nbsp;<span style="color: red;">*</span>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="EmptyFieldSave" runat="server" ForeColor="Red"
                                                    ControlToValidate="txtEquipmentSubCategory" ErrorMessage="This information is required."></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtEquipmentSubCategory" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>--%>
                                            </div>
                                        </div>
                                        <div class="col-sm-1 col-md-1 col-lg-1" style="padding-left: 0px">
                                            <div class="form-group">
                                                <br />
                                                <asp:Button ID="btnAddSave" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAddSave_Click" />
                                            </div>
                                        </div>

                                        <div class="col-sm-3 col-md-3 col-lg-3">
                                            <div class="form-group">
                                                <asp:CheckBox ID="chkactive" runat="server" Text="Isactive" Visible="false" />
                                            </div>
                                        </div>
                                    </div>

                                    <asp:HiddenField ID="HddEquipmentSubCat" runat="server" />
                                    <div class="row" style="padding-top: 0px;">
                                        <div class="col-sm-6 col-md-6 col-lg-6">
                                            <div id="DivAddEquipmentCategory" runat="server" class="form-group" style="display: none;">
                                                <asp:GridView ID="GrdAddEquipmentsub" runat="server" AutoGenerateColumns="false" ShowHeader="false" OnRowEditing="GrdAddEquipmentsub_RowEditing"
                                                    ShowHeaderWhenEmpty="false" CssClass="table table-responsive" GridLines="None" BackColor="White">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IbAddEquipmentsubedit" runat="server" Text="EditSub" Height="20px" CommandName="edit" ImageUrl="~/Images/edit.png" />
                                                                <asp:ImageButton ID="IbAddEquipmentsubdelete" runat="server" Text="Delete" Height="20px" ImageUrl="~/Images/Delete.png" OnClick="IbAddEquipmentsubdelete_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEquipmentSubCat" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="Headerstyle" />
                                                    <FooterStyle CssClass="gridfooter" />
                                                    <%-- <PagerStyle CssClass="pagerstyle" ForeColor="White" HorizontalAlign="Center" />--%>
                                                    <%-- <SelectedRowStyle CssClass="gridselectedrow" />
                                                    <EditRowStyle CssClass="grideditrow" />
                                                    <AlternatingRowStyle CssClass="gridalterrow" />
                                                    <RowStyle CssClass="gridrow" />--%>
                                                </asp:GridView>
                                            </div>

                                            <div id="DivEditEquipmentCategory" runat="server" class="form-group" style="display: none;">
                                                <asp:HiddenField ID="HddEquipementSubCategoryID" runat="server" />
                                                <asp:HiddenField ID="HddGridIndex" runat="server" />
                                                <asp:HiddenField ID="HddAddorEdit" runat="server" />
                                                <asp:GridView ID="GrdEditEquipmentsub" runat="server" AutoGenerateColumns="false" ShowHeader="false" OnRowEditing="GrdEditEquipmentsub_RowEditing"
                                                    OnRowDeleting="GrdEditEquipmentsub_RowDeleting" ShowHeaderWhenEmpty="false" CssClass="table table-responsive" GridLines="None" BackColor="White">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IbEditEquipmentsubedit" runat="server" Text="Edit" Height="20px" CommandName="Edit" ImageUrl="~/Images/edit.png" />
                                                                <asp:ImageButton ID="IbEditEdit" runat="server" Text="Delete" Height="20px" CommandName="delete" ImageUrl="~/Images/Delete.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEquipmentSubCat" runat="server" Text='<%# Eval("EquipmentSubCategoryDescription") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEquipmentSubCatID" runat="server" Text='<%# Eval("EquipementSubCategoryID") %>' CssClass="HeaderHide"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <HeaderStyle CssClass="Headerstyle" />
                                                    <FooterStyle CssClass="gridfooter" />
                                                    <%-- <PagerStyle CssClass="pagerstyle" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle CssClass="gridselectedrow" />
                                                    <EditRowStyle CssClass="grideditrow" />
                                                    <AlternatingRowStyle CssClass="gridalterrow" />
                                                    <RowStyle CssClass="gridrow" />--%>
                                                </asp:GridView>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                            <asp:HiddenField ID="HddUserID" runat="server" />
                            <asp:HiddenField ID="HddUpdateLockinEdit" runat="server" />
                            <asp:HiddenField ID="HddCorpID" runat="server" />
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
                            </div>
                        </div>
                    </div>
                </div>

                <div id="modalSave" class="modal fade" style="position: center">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                            <div class="modal-header bg-green">
                                <h4 class="modal-title font-bold text-white">Corporate Equipment Map
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
                                <h4 class="modal-title font-bold text-white">Corporate Equipment Map
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
                        <div class="modal-content">
                            <div class="modal-header btn-warning">
                                <h4 class="modal-title font-bold text-white">Corporate Equipment Map
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



        <div style="display: none">
            <rsweb:ReportViewer ID="rvservicerequestreport" runat="server"></rsweb:ReportViewer>
        </div>
    <%--</form>--%>
</asp:Content>
