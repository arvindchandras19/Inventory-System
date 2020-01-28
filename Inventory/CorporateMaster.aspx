<%@ Page Title="Corporate" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="CorporateMaster.aspx.cs" Inherits="Inventory.CorporateMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : CorporateMaster.aspx 
' Type      : ASPX File 
' Description  :   To design the CorporateMaster page for add,Update,Read,Delete and show the record on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                                         Reason 
  04/10/2018             V.01              Sairam. P                                    New
  
'******************************************************************************************************/
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Common.css" rel="stylesheet" />
    <script src="Scripts/CDN.js/Cdn.js"></script>
    <style type="text/css">
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

        .headerstr {
            display: none;
        }

        .color {
            color: red;
        }

        .radiostyle input[type="radio"] {
            margin-left: 10px;
            margin-right: 1px;
        }
    </style>
    <script src="Scripts/notification.js"></script>
    <script type="text/javascript">


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
        function ShowwarningPopup(res) {
            $('[id*=lblwarning]').html(res);

            $("#modalWarning").modal("show");
        }

        function ShowConfirmationPopup() {

            $("#modalConfirm").modal("show");

        }
        function ShowRestorePopup() {

            $("#modalrestore").modal("show");

        }
        function ShowpanelPopup() {

            $("#panelconfirm").modal("show");

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Corporate Master
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <div class="mypanel-body">
                    <asp:HiddenField runat="server" ID="HiddenCorporateMasterID" Value="0"></asp:HiddenField>
                    <asp:HiddenField ID="HiddenCorporateName" runat="server" Value="0" />
                    <asp:HiddenField runat="server" ID="HdnStatus" Value="0"></asp:HiddenField>
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                            <asp:Label ID="lblseroutHeader" runat="server" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                        </div>
                        <div class="col-lg-6" style="margin-bottom: 5px;" align="right">
                            <div align="right" id="deletebtn" style="display: block;" runat="server">
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnClear_Click" />
                            </div>
                            <div id="savebtn" runat="server" style="display: none; margin-left: 10px; float: right;" align="right">
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="ErrorFieldSave" OnClick="btnsave_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                            </div>
                        </div>
                    </div>
                    <div runat="server">
                        <div class="well well-sm">
                            <div id="DivSearch" runat="server" class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                      <div class="form-group">
                                    <span style="font-weight:800;">Corporate Code <i class="fa fa-info-circle" title="Use % for wild card search"></i></span>
                                    <asp:TextBox ID="txtCorporateSearch" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ReqtxtCorporateSearch" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                        ControlToValidate="txtCorporateSearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                     </div>
                                 <div class="col-sm-4 col-md-4 col-lg-4">
                                      <div class="form-group">
                                    <span style="font-weight:800;">Corporate Description <i class="fa fa-info-circle" title="Use % for wild card search"></i> </span>
                                    <asp:TextBox ID="txtCorporatedescription" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ReqtxtCorporatedesc" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                        ControlToValidate="txtCorporatedesc" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                     </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <span style="font-weight:800;">Status</span>
                                        <asp:RadioButtonList ID="rdbstatus" runat="server" RepeatDirection="Horizontal" CssClass="radiostyle">
                                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>

                            <div id="div_ADDContent" runat="server" class="row" style="display: none">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblCorporateName" runat="server" Text="Corporate Code"></asp:Label><span style="color: red">*</span>
                                        <asp:TextBox ID="txtCorporateFullName" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ReqCorporateFullName" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                            ControlToValidate="txtCorporateFullName" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblCorporateDesc" runat="server" Text="Corporate Description"></asp:Label><span style="color: red">*</span>
                                        <asp:TextBox ID="txtCorporateDesc" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ReqCorporateDesc" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                            ControlToValidate="txtCorporateDesc" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblPOEmail" runat="server" Text="PO Email"></asp:Label><span style="color: red">*</span>
                                        <asp:TextBox ID="txtPOEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ReqtxtPOEmail" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                            ControlToValidate="txtPOEmail" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revtxtPOEmail" runat="server"
                                            ErrorMessage="Please enter valid email" ControlToValidate="txtPOEmail"
                                            SetFocusOnError="true"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ValidationGroup="ErrorFieldSave" ForeColor="" CssClass="color" Display="Dynamic" Font-Size="0.9em"></asp:RegularExpressionValidator>
                                    </div>
                                </div>

                              <%--  <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblstatus" runat="server" Text="Restore" Visible="false"></asp:Label>
                                        <asp:CheckBox ID="chkrestore" runat="server" Text="" Visible="false" />
                                    </div>
                                </div>--%>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <asp:CheckBox ID="chkstatus" runat="server" Text="Isactive" Checked="true" Visible="false" />
                                </div>
                        </div>
                            </div>

                        </div>
                    </div>
                    <div class="row" style="margin-left: 1px; margin-top: 0px;" id="divCorporate" runat="server">
                        <div class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                                <asp:Label ID="lblhead" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>                                
                            </div>
                        </div>
                        <div >
                            <asp:Label ID="lblrowcount" runat="server">No of records : <%=gvCorporate.Rows.Count.ToString() %></asp:Label>
                        </div>
                        <div class="MSRReviewgrid">
                        <asp:GridView ID="gvCorporate" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" OnRowDataBound="gvCorporate_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="8%" ItemStyle-Width="8%" >
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" runat="server" Text="Edit" Height="20px" ToolTip="Edit Corporate Master" OnClick="imgbtnEdit_Click" ImageUrl="~/Images/edit.png" />
                                        <asp:ImageButton ID="btndeleteimg" runat="server" Height="20px" ToolTip="Delete Corporate Master" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClick="btndeleteimg_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CorporateID" HeaderText="CorporateID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                                <asp:BoundField DataField="CorporateName" HeaderText="Corporate Code" />
                                <asp:BoundField DataField="CorporateDescription" HeaderText="Corporate Description" />
                                <asp:BoundField DataField="POEmail" HeaderText="PO Email" />
                                <%--<asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkactive" runat="server" Checked='<%#Convert.ToBoolean(Eval("IsActive").ToString() )%>' OnCheckedChanged="chkactive_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" Text=' <%# Eval("Active")%>'></asp:Label>
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
                <div id="modalSave" class="modal fade" style="position: center">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content ">
                            <div class="modal-header bg-green">
                                <h4 class="modal-title font-bold text-white">Corporate Master
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
                                <h4 class="modal-title font-bold text-white">Corporate Master
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
                                <h4 class="modal-title font-bold text-white">Corporate Master
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
                                <asp:ImageButton ID="ImageRemoveYes" runat="server" CssClass="btn btn-danger" AlternateText="Yes" OnClick="ImageRemoveYes_Click" />
                                <button type="button" class="btn btn-default ra-100" data-dismiss="modal">Close</button>
                                <%--<asp:ImageButton ID="ImageButtonNo" runat="server" ImageUrl="~/Images/btnNo.jpg"/>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="modalrestore" class="modal fade" style="position: center">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content ">
                            <div class="modal-header bg-red">
                                <h4 class="modal-title font-bold text-white">Restore Confirmation</h4>
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            </div>
                            <div class="modal-body">
                                <p>Do you want to restore this record <span id="spnName"></span>?.</p>
                            </div>
                            <div class="modal-footer">
                                <asp:ImageButton ID="ImgRestore" runat="server" CssClass="btn btn-danger" AlternateText="Yes"/>
                                <asp:ImageButton ID="ImageButton7" runat="server" CssClass="btn btn-default ra-100" AlternateText="Close" />
                            </div>
                        </div>
                    </div>
                </div>
                <div style="display: none">
                    <rsweb:ReportViewer ID="rvCorporatereport" runat="server"></rsweb:ReportViewer>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="User_Permission_Message" runat="server" visible="false">

            <br />
            <br />
            <br />
            <h4>
                <center> This User doesn't have permission to view this screen</center>
            </h4>
        </div>
   
</asp:Content>
