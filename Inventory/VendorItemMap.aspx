<%@ Page Title="Vendor Item Mapping" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="VendorItemMap.aspx.cs" Inherits="Inventory.VendorItemMap" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : VendorItemMap.aspx 
' Type      : ASPX File 
' Description  :   To design the VendorItemMap page for add,Update and show the VendorItem list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/17/2017           V.01               Murali.M                       New
  24/10/2017           V.02               Sairam.P                       Model popup added for notifications
'******************************************************************************************************/
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/sumoselect.css" rel="stylesheet" />
    <link href="Content/Common.css" rel="stylesheet" />
    <script src="Scripts/CDN.js/Cdn.js"></script>
    <style type="text/css">
        .modalBackground {
            position: fixed;
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

        .radiostyle input[type="radio"] {
            margin-left: 10px;
            margin-right: 1px;
        }
    </style>

    <script>
        $(document).ready(function () {

            <%--$("#addnew").click(function () {
                $('#addnew').attr('disabled', 'disabled');
                $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
                $("#saveoredit").slideToggle(200);
                document.getElementById('<%=hdnItemMapID.ClientID%>').value = 0;
            });--%>

            //$("#btnCancel").click(function () {
            <%--  $('#addnew').removeAttr('disabled');
                $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
                $("#saveoredit").slideToggle(200);--%>
                <%--document.getElementById('<%= drdItemCategory.ClientID %>').value = 0;
                document.getElementById('<%= drdVendorID.ClientID %>').value = 0;
                document.getElementById('<%= drdItemID.ClientID %>').value = 0;
                document.getElementById('<%= txtVendorItemID.ClientID %>').value = "";
                document.getElementById('<%=hdnItemMapID.ClientID%>').value = 0;
                validationclear();

            });--%>
        });

        function validationclear() {
            $("#<%= ReqfielddrdItemCategory.ClientID %>").css("display", "none");
            $("#<%= ReqfielddrdItemName.ClientID %>").css("display", "none");
            $("#<%= ReqfielddrdVendorID.ClientID %>").css("display", "none");
            $("#<%= reqfieldtxtVendorItemID.ClientID %>").css("display", "none");
        }

        function Page_ClientValidateReset() {
            $("#<%= ReqfielddrdItemCategory.ClientID %>").css("display", "block");
            $("#<%= ReqfielddrdItemName.ClientID %>").css("display", "block");
            $("#<%= ReqfielddrdVendorID.ClientID %>").css("display", "block");
            $("#<%= reqfieldtxtVendorItemID.ClientID %>").css("display", "block");

        }
        function editfacility() {
            $('#<%=div_ADDContent.ClientID %>').show();
            $("#saveoredit").show();
            $('#addnew').attr('disabled', 'disabled');
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

        function ShowConfirmationPopup() {

            $("#modalConfirm").modal("show");

        }

        function jScript() {
            $('[id*=drpItemCategorySearch]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Item Category'
            });

            $('[id*=drpVendorSearch]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Vendor'
            });
        }
    </script>
    <%--        function Validatetxtbox() {
            var txtbxresult = false;
            var txtVendorID = document.getElementById('<%= txtVendorItemID.ClientID %>').value.trim();
            var drdVendorID = document.getElementById('<%= drdVendorID.ClientID %>').value.trim();
            var drdItemID = document.getElementById('<%= drdItemID.ClientID %>').value.trim();
            var drdItemCategory = document.getElementById('<%= drdItemCategory.ClientID %>').value.trim();

            if (txtVendorID == "" || drdVendorID == 0 || drdItemID == 0 || drdItemCategory == 0) {
                alert("Enter all mandatory Fields");
            }
            else {
                txtbxresult = true;
            }
            return txtbxresult;
        }--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   

        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Vendor Item Mapping
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(jScript);
                </script>
                <div class="mypanel-body">
                    <div class="row" id="saveoredit" runat="server">
                        <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                            <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                        </div>
                        <div runat="server" class="col-sm-8 col-md-8 col-lg-8" align="right">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                            <%--<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" ValidationGroup="SearchInput" />--%>
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success" Visible="false" ValidationGroup="EmptyField" OnClientClick="Page_ClientValidateReset()" OnClick="btnsave_Click" />
                            <%--  <input type="button" id="btnCancel" class="btn btn-success" value="Clear" />--%>
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_Click" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="btn btn-primary" Visible="false" Text="Cancel" OnClick="btnclose_Click" />
                        </div>
                    </div>
                    <div style="margin-bottom: 5px;"></div>
                    <asp:HiddenField ID="hdnItemMapID" Value="0" runat="server" />
                    <asp:HiddenField ID="HddVendorItemCategory" Value="" runat="server" />

                    <div id="div_ADDContent" runat="server" class="well well-sm">

                        <div id="DivSearchControl" runat="server" class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4">
                                <div class="form-group">
                                    <span style="font-weight: 800;">Item Category</span>
                                    <asp:ListBox ID="drpItemCategorySearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple"></asp:ListBox>
                                    <%--<asp:DropDownList runat="server" ID="drpVendorSearch" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpVendorSearch" runat="server" ForeColor="Red"
                                ControlToValidate="drpVendorSearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-4 col-lg-4">
                                <div class="form-group">
                                    <span style="font-weight: 800;">Vendor</span>
                                    <asp:ListBox ID="drpVendorSearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple"></asp:ListBox>
                                    <%--<asp:DropDownList runat="server" ID="drpItemCategorySearch" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpItemCategorySearch" runat="server" ForeColor="Red"
                                ControlToValidate="drpItemCategorySearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
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
                        <div id="DivAdd" runat="server" class="row" style="display: none;">
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <span>Item Category<span style="color: red;">*</span></span>
                                    <br />
                                    <asp:DropDownList runat="server" ID="drdItemCategory"
                                        OnSelectedIndexChanged="drdItemCategory_SelectedIndexChanged"
                                        CssClass="form-control" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrdItemCategory" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                        ControlToValidate="drdItemCategory" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <span>Item Description<span style="color: red">*</span></span>
                                    <asp:DropDownList runat="server" ID="drdItemID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrdItemName" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                        ControlToValidate="drdItemID" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <span>Vendor Description<span style="color: red">*</span></span>
                                    <br />
                                    <asp:DropDownList runat="server" ID="drdVendorID" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrdVendorID" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                        ControlToValidate="drdVendorID" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <span>Vendor Item ID<span style="color: red">*</span></span>
                                    <asp:TextBox runat="server" ID="txtVendorItemID" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqfieldtxtVendorItemID" runat="server" ControlToValidate="txtVendorItemID" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group" id="DivCheckBox" runat="server">
                                    <asp:CheckBox ID="chkactive" runat="server" Text="Isactive" Checked="true" Visible="false" />
                                </div>
                            </div>
                        </div>

                    </div>

                    <div id="DivSearch" runat="server">
                        <div class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                                <asp:Label ID="lblhead" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="lblrowcount" runat="server">No of records : <%=gridItems.Rows.Count.ToString() %></asp:Label>
                        </div>
                        <div id="div_Grid" class="row VendorItemgrid" style="margin-top: 3px;">
                            <div class="col-sm-12 col-md-12 col-lg-12" style="padding-right: 0px">
                                <asp:GridView ID="gridItems" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-responsive" ShowHeaderWhenEmpty="True" OnRowDataBound="gridItems_RowDataBound"
                                    EmptyDataText="No Records Found">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lbedit" runat="server" Text="Edit" Height="20px" ToolTip="Edit Vendor Item Map" OnClick="lbedit_Click" ImageUrl="~/Images/edit.png" />
                                                <asp:ImageButton ID="btndeleteimg" runat="server" Height="20px" ToolTip="Delete Vendor Item Map" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClick="btndeleteimg_Click" />
                                                <%--<asp:ImageButton ID="imgbtnview" runat="server" Text="View" Height="20px" OnClick="imgbtnview_Click" ImageUrl="~/Images/View.png" />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Delete" Visible="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="lbdelete" runat="server" Text="Delete" OnClick="lbdelete_Click" Height="20px" ImageUrl="~/Images/close.png" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                                        <asp:BoundField DataField="ItemMapID" HeaderText="ItemMapID" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
                                        <asp:BoundField HeaderText="Item Category" DataField="CategoryName" />
                                        <asp:BoundField HeaderText="Item Code" DataField="ItemID" />
                                        <asp:BoundField HeaderText="Item Description" DataField="ItemDescription" />
                                        <asp:BoundField HeaderText="Vendor Description" DataField="VendorDescriptionWithCode" />
                                        <asp:BoundField HeaderText="Vendor Item ID" DataField="VendorItemID" />
                                        <%--<asp:TemplateField HeaderText="Active">
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
                                        <asp:BoundField HeaderText="Item Description" DataField="ItemDescriptionWithCode" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
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
                        <h4 class="modal-title font-bold text-white">Vendor ItemMap
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
                        <h4 class="modal-title font-bold text-white">Vendor ItemMap
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
                        <h4 class="modal-title font-bold text-white">Vendor ItemMap
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

        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center>This User doesn't have permission to view this screen</center>
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
                        <asp:Label ID="lblmsg" runat="server" Text="Are you sure you want to delete this Record?" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right" style="padding-right: 15px">
                        <asp:ImageButton ID="btnYes" OnClick="btnYes_Click" runat="server" ImageUrl="~/Images/btnyes.jpg" />
                        <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/Images/btnNo.jpg" />
                    </td>
                </tr>
            </table>
            <div style="display: none">
                <rsweb:ReportViewer ID="rvVendorItemMapreport" runat="server"></rsweb:ReportViewer>
            </div>
        </asp:Panel>
</asp:Content>
