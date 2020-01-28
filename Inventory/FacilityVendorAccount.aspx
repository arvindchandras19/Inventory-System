<%@ Page Title="Facility Vendor Acct" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="FacilityVendorAccount.aspx.cs" Inherits="Inventory.FacilityVendorAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : FacilityVendorAccount.aspx 
' Type      : ASPX File 
' Description  :   To design the Facility page for add,Update and show the Facility Vendor Account list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01              Sairam.P                     New
  10/24/2017           V.02              Sairam.P                     Model Popup Added for Notifications
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
                $("#savecancel").slideToggle(200);
                document.getElementById('<%=hdnFVAccountID.ClientID%>').value = 0;
            })--%>;

            $("#btnClose").click(function () {
              <%--  $('#addnew').removeAttr('disabled');
                $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
                $("#savecancel").slideToggle(200);--%>
                document.getElementById('<%= drdFacilityName.ClientID %>').value = 0;
                document.getElementById('<%= drdVendorName.ClientID %>').value = 0;
                document.getElementById('<%= txtBillAccount.ClientID %>').value = "";
                document.getElementById('<%= txtShipAccount.ClientID %>').value = "";
                document.getElementById('<%=hdnFVAccountID.ClientID%>').value = 0;
                validationclear();
            });
        });

        function validationclear() {

            $("#<%= ReqfielddrdFacilityName.ClientID %>").css("display", "none");
            $("#<%= ReqfielddrdVendorName.ClientID %>").css("display", "none");
            $("#<%= reqfieldtxtShipAccount.ClientID %>").css("display", "none");
            $("#<%= reqfieldtxtBillAccount.ClientID %>").css("display", "none");
        }

        function Page_ClientValidateReset() {
            $("#<%= ReqfielddrdFacilityName.ClientID %>").css("display", "block");
            $("#<%= ReqfielddrdVendorName.ClientID %>").css("display", "block");
            $("#<%= reqfieldtxtShipAccount.ClientID %>").css("display", "block");
            $("#<%= reqfieldtxtBillAccount.ClientID %>").css("display", "block");

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
        <%--function editfacility() {
            $('#<%=div_ADDContent.ClientID %>').show();
            $("#savecancel").show();
            $('#addnew').attr('disabled', 'disabled');
        }--%>

        <%--        function Validatetxtbox() {
            var txtbxresult = false;
            var txtBillAccount = document.getElementById('<%= txtBillAccount.ClientID %>').value.trim();
            var txtShipAccount = document.getElementById('<%= txtShipAccount.ClientID %>').value.trim();
            var drdFacility = document.getElementById('<%= drdFacilityName.ClientID %>').value.trim();
            var drdVendor = document.getElementById('<%= drdVendorName.ClientID %>').value.trim();


            if (drdVendor == '0' || drdFacility == "0" || txtShipAccount == "" || txtBillAccount == "") {
                alert("Enter all mandatory Fields");
            }
            else {
                txtbxresult = true;
            }
            return txtbxresult;
        }--%>

        function jScript() {
            $('[id*=drpFacilitySearch]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Facility'
            });

            $('[id*=drpVendorSearch]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Vendor'
            });
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

  
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Facility Vendor Account
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(jScript);
                </script>
                <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
                    <asp:Panel ID="pnlSearch" DefaultButton="btnSearchItems" runat="server">
                        <div class="row">
                            <div id="savecancel" runat="server">
                                <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                                    <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                                </div>
                                <div style="float: right; padding: 0px 0px 0px 3px; margin-right: 15px;">
                                    <asp:Button ID="btnsave" runat="server" Text="Save" OnClientClick="Page_ClientValidateReset()" CssClass="btn btn-success" ValidationGroup="EmptyField"
                                        OnClick="btnsave_Click" Visible="false" />
                                    <%--<input id="btnClose" type="button" class="btn btn-success" value="Cancel" />--%>
                                    <asp:Button ID="btnclose" runat="server" CssClass="btn btn-primary" Text="Cancel" Visible="false" OnClick="btnclose_Click" />
                                </div>
                            </div>
                            <div id="div_SearchDiv" runat="server" style="padding: 0px; margin-right: 15px;">
                                <div class="display_inline" style="float: right; text-align: center">
                                    <asp:Button ID="btnSearchItems" runat="server" Text="Search"
                                        CssClass="btn btn-primary" OnClick="btnSearchFacility_Click" />
                                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                    <asp:Button ID="buttonPrint" runat="server" Text="Print"
                                        CssClass="btn btn-primary" OnClick="buttonPrint_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div style="margin-bottom: 5px;"></div>
                    <asp:HiddenField ID="hdnFVAccountID" Value="0" runat="server" />


                    <div id="div_ADDContent" runat="server" class="well well-sm">

                        <div id="DivSearchControl" runat="server" class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4">
                                <div class="form-group">
                                    <span style="font-weight: 800;">Facility Description</span>
                                    <asp:ListBox ID="drpFacilitySearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple"></asp:ListBox>
                                    <%--<asp:DropDownList runat="server" ID="drpFacilitySearch" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrpVendorSearch" runat="server" ForeColor="Red"
                                ControlToValidate="drpVendorSearch" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-4 col-lg-4">
                                <div class="form-group">
                                    <span style="font-weight: 800;">Vendor Description</span>
                                    <asp:ListBox ID="drpVendorSearch" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple"></asp:ListBox>
                                    <%--<asp:DropDownList runat="server" ID="drpVendorSearch" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>--%>
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

                        <div class="row" id="DivAdd" runat="server" style="display: none;">
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <span>Facility Description<span style="color: red">*</span></span>
                                    <asp:DropDownList ID="drdFacilityName" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrdFacilityName" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                        ControlToValidate="drdFacilityName" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <span>Vendor Description<span style="color: red">*</span></span>
                                    <asp:DropDownList runat="server" ID="drdVendorName" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrdVendorName" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                        ControlToValidate="drdVendorName" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <span>Ship to Account<span style="color: red">*</span></span>
                                    <asp:TextBox runat="server" ID="txtShipAccount" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqfieldtxtShipAccount" runat="server" ControlToValidate="txtShipAccount" ValidationGroup="EmptyField"
                                        ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group">
                                    <span>Bill to Account<span style="color: red">*</span></span>
                                    <asp:TextBox runat="server" ID="txtBillAccount" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqfieldtxtBillAccount" runat="server" ControlToValidate="txtBillAccount" ValidationGroup="EmptyField"
                                        ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3">
                                <div class="form-group" id="DivCheckBox" runat="server">
                                    <asp:CheckBox ID="chkactive" runat="server" Text="Isactive" Checked="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="HddFacilityVendor" runat="server" Value="" />
                    <div id="DivSearch" runat="server">
                        <div class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4" align="left">
                                <asp:Label ID="lblhead" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="lblrowcount" runat="server">No of records : <%=gridFacilityVendorAcc.Rows.Count.ToString() %></asp:Label>
                        </div>
                        <div class="row VendorItemgrid" style="margin-left: 1px; margin-top: 3px;">
                            <asp:GridView ID="gridFacilityVendorAcc" runat="server" AutoGenerateColumns="false" OnRowDataBound="gridFacilityVendorAcc_RowDataBound"
                                CssClass="table table-responsive" ShowHeaderWhenEmpty="True"
                                EmptyDataText="No Records Found">
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lbedit" runat="server" Text="Edit" Height="20px" ToolTip="Edit Facility Vendor Account" Style="margin-bottom: 0px; margin-left: 5px;" OnClick="lbedit_Click" ImageUrl="~/Images/edit.png" />
                                            <asp:ImageButton ID="btndeleteimg" runat="server" Height="20px" ToolTip="Delete Facility Vendor Account" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClick="btndeleteimg_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FacilityVendorAccID" HeaderText="Facility Vendor Acc ID" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
                                    <asp:BoundField DataField="FacilityDescription" HeaderText="Facility Description" />
                                    <asp:BoundField DataField="VendorDescription" HeaderText="Vendor Description" />
                                    <asp:BoundField DataField="ShipAccount" HeaderText="Ship to Account" />
                                    <asp:BoundField DataField="BillAccount" HeaderText="Bill to Account" />
                                    <%--<asp:TemplateField HeaderText="Active">
                                <ItemTemplate>                                     
                                    <asp:CheckBox runat="server" ID="chkActive"
                                        Checked='<%#Convert.ToBoolean(Eval("IsActive").ToString())%>' AutoPostBack="true" OnCheckedChanged="chkActive_CheckedChanged" />
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


                    <div id="modalSave" class="modal fade" style="position: center">
                        <div class="modal-dialog modal-sm">
                            <div class="modal-content ">
                                <div class="modal-header bg-green">
                                    <h4 class="modal-title font-bold text-white">Facility Vendor Account
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
                                    <h4 class="modal-title font-bold text-white">Facility Vendor Account
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
                                    <h4 class="modal-title font-bold text-white">Facility Vendor Account
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
                            <center> This User doesn't have permission to view this screen</center>
                        </h4>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

        <div style="display: none">
            <rsweb:ReportViewer ID="rvFacilityVendorAccreport" runat="server"></rsweb:ReportViewer>
        </div>
   
</asp:Content>
