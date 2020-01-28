<%@ Page Title="Vendor" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="Vendor.aspx.cs" Inherits="Inventory.Vendor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : Vendor.aspx 
' Type      : ASPX File 
' Description  :   To design the Vendor page for add,Update and show the Vendor list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01              Mahalakshmi.S                     New
  08/16/2017           V.01               Vivekanand.S                 Added ClearContent function while ADD Button click Event
  10/24/2017           V.02               Sairam.P                     Notifications Model Popup added.
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

        .color {
            color: red;
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
    <script>
        function Validatetxtbox(sender, args) {
            var chkall = document.getElementById('<%= chkf.ClientID %>');
           var serorder = document.getElementById('<%= chks.ClientID %>');
           var regsup = document.getElementById('<%= chkh.ClientID %>');
           var macpart = document.getElementById('<%= chkm.ClientID %>');
           var BM = document.getElementById('<%= chkBM.ClientID %>');
           var VendorName = document.getElementById('<%= txtvendorID.ClientID %>').value;
           var VendorDesc = document.getElementById('<%= txtvendorname.ClientID %>').value;
           if (chkall.checked == true || serorder.checked == true || regsup.checked == true || macpart.checked == true || BM.checked == true) {
               args.IsValid = true;
               //alert("Select one type");
               return true;
               //alert("Select one type");
           }
           else {
               //alert("not chk");
               args.IsValid = false;
           }

       }

       //Show Hide Content & Clear
       <%-- $(document).ready(function () {

            $("#btnAdd").click(function () {
                $('#btnAdd').attr('disabled', 'disabled');
                //$('[id*=btnprint]').attr('disabled', 'disabled');
                $('[id*=btnprint]').hide();
                $('[id*=btnprintdt]').hide();
                $('#savebtn').slideToggle(200);
                $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
                $('#divvendor').hide();
                ClearData();
                //$('#div_SearchDiv').slideToggle(200);
            });

            $("#btnClose").click(function () {
                $('#btnAdd').removeAttr('disabled');
                //$('[id*=btnprint]').removeAttr('disabled');
                $('[id*=btnprint]').show();
                $('[id*=btnprintdt]').hide();
                $('#savebtn').slideToggle(200);
                $('#<%=div_ADDContent.ClientID %>').slideToggle(200);
                //$('#div_SearchDiv').slideToggle(200);
                $('#divvendor').show();
                ClearData();
                validationclear();
            });
        });

       function HideGrid() {
            $('#divvendor').hide();

       }     
        function ShowContent() {
            $('#savebtn').show();
            $('[id*=btnprint]').hide();
            //$('[id*=btnprint]').attr('disabled', 'disabled');
            $('[id*=btnprintdt]').show();
            $('#<%=div_ADDContent.ClientID %>').show();
            $('#btnAdd').attr('disabled', 'disabled');
            $('#divvendor').hide();
            //$('#div_SearchDiv').slideToggle(200);
        }--%>

        function checkall() {
            var chkall = document.getElementById('<%= chkf.ClientID %>');
            var serorder = document.getElementById('<%= chks.ClientID %>');
            var regsup = document.getElementById('<%= chkh.ClientID %>');
            var macpart = document.getElementById('<%= chkm.ClientID %>');
            var BM = document.getElementById('<%= chkBM.ClientID %>');
            var IT = document.getElementById('<%= chkIT.ClientID %>');

            if (chkall.checked) {

                serorder.checked = true;
                regsup.checked = true;
                macpart.checked = true;
                BM.checked = true;
                IT.checked = true;
            }
            else {
                serorder.checked = false;
                regsup.checked = false;
                macpart.checked = false;
                BM.checked = false;
                IT.checked = false;
            }
            Page_ClientValidate('reset-all');
        }

        function chkother() {
            var chkall = document.getElementById('<%= chkf.ClientID %>');
            var serorder = document.getElementById('<%= chks.ClientID %>');
            var regsup = document.getElementById('<%= chkh.ClientID %>');
            var macpart = document.getElementById('<%= chkm.ClientID %>');
            var BM = document.getElementById('<%= chkBM.ClientID %>');
            var IT = document.getElementById('<%= chkIT.ClientID %>');
            if (serorder.checked == false || regsup.checked == false || macpart.checked == false || BM.checked == false || IT.checked == false) {
                chkall.checked = false;
            }
            else {
                chkall.checked = true;
            }
            Page_ClientValidate('reset-all');
        }

        <%--     function ClearData() {
            document.getElementById('<%= txtvendorname.ClientID %>').value = "";
            document.getElementById('<%= txtcontactperson.ClientID %>').value = "";
            document.getElementById('<%= txtcontactph.ClientID %>').value = "";
            document.getElementById('<%= txtcontactemail.ClientID %>').value = "";
            document.getElementById('<%= txtpoemail.ClientID %>').value = "";
            document.getElementById('<%= txtvendorID.ClientID %>').value = "";
            document.getElementById('<%= txtaddress1.ClientID %>').value = "";
            document.getElementById('<%= txtaddress2.ClientID %>').value = "";
            document.getElementById('<%= txtcity.ClientID %>').value = "";
            document.getElementById('<%= txtzipcode.ClientID %>').value = "";
            document.getElementById('<%= ddlState.ClientID %>').value = "0";
            document.getElementById('<%= txtphone.ClientID %>').value = "";
             document.getElementById('<%= txtxtn.ClientID %>').value = "";
            document.getElementById('<%= txtaltenateemail.ClientID %>').value = "";
            document.getElementById('<%= txtcountry.ClientID %>').value = "";
            document.getElementById('<%= txtfax.ClientID %>').value = "";

            document.getElementById('<%=hiddenVendorID.ClientID%>').value = 0;
            $("#<%=chkf.ClientID %>").prop('checked', false);
            $("#<%=chkh.ClientID %>").prop('checked', false);
            $("#<%=chkm.ClientID %>").prop('checked', false);
            $("#<%=chks.ClientID %>").prop('checked', false);
            $("#<%=chkBM.ClientID %>").prop('checked', false);
             $("#<%=chkIT.ClientID %>").prop('checked', false);

        }
        function validationclear() {
            <%-- $("#<%= RequiredFieldtxtvendorID.ClientID %>").css("display", "none");--%>
        <%-- $("#<%= RequiredFieldtxtvendorname.ClientID %>").css("display", "none");--%>
        <%-- $("#<%= revEmail.ClientID %>").css("display", "none");
            $("#<%= regaltmail.ClientID %>").css("display", "none");
            $("#<%= Reqvendorcode.ClientID %>").css("display", "none");
            $("#<%= Reqvendordesc.ClientID %>").css("display", "none");
            $("#<%= regpomail.ClientID %>").css("display", "none");
            $("#<%= reqContactEmail.ClientID %>").css("display", "none");
            $("#<%= cusvalvendortype.ClientID %>").css("display", "none"); --%>
        <%--   //$("#<%= cusvalvendorname.ClientID %>").css("display", "none");--%>
        <%--    $("#<%= cusvalvendordesc.ClientID %>").css("display", "none");--%>
        //}--%>

        function ShowPopup(res) {

            $('[id*=lblsave]').html(res);

            $("#modalSave").modal("show");

        }
        function ShowdelPopup(res) {

            $('[id*=lbldelete]').html(res);

            $("#modalDelete").modal("show");
        }

        function ShowConfirmationPopup() {
            $("#modalConfirm").modal("show");
        }
        function jScript()
        {
            $(document).ready(function () {
                $(".input-mask").inputmask();
            });
        }
     

        //function VendorcodeError() {
        //    ShowContent()
        //}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   

        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Vendor
                </div>
            </div>
        </div>
      <script type="text/javascript">
                    Sys.Application.add_load(jScript);          
        </script>
        <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
            <asp:HiddenField ID="hiddenVendorID" Value="0" runat="server" />
            <asp:HiddenField ID="hdnvendor" Value="" runat="server" />
            <div id="div_searchContent" runat="server">
                <div class="row">
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                    </div>
                    <div id="saveoredit" class="col-sm-8 col-md-8 col-lg-8" align="right">
                        <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnsearch_Click" />
                        <asp:Button ID="btnadd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnadd_Click" />
                        <asp:Button ID="btnprint" runat="server" Text="Print" class="btn btn-primary" OnClick="btnprint_Click" />
                        <asp:Button ID="btnCancel" runat="server" class="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </div>
                <div id="divsearch" runat="server" class="well well-sm" style="margin-top: 2px;">
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <span style="font-weight: 800;">Vendor Code <i class="fa fa-info-circle" title="Use % for wild card search"></i></span>
                                <asp:TextBox ID="txtSearchVendor" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <span style="font-weight: 800;">Vendor Description <i class="fa fa-info-circle" title="Use % for wild card search"></i></span>
                                <asp:TextBox ID="txtSearchdesc" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md4 col-sm-4">
                            <div class="form-group">
                                <span style="font-weight: 800;">Status </span>
                                <asp:RadioButtonList ID="reactive" runat="server" RepeatDirection="Horizontal" CssClass="rbl">
                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divsearchgrid" runat="server">
                <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <asp:Label ID="Label1" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                    </div>
                </div>
                <div>
                    <asp:Label ID="lblrowcount" runat="server">No of records : <%=gvvendor.Rows.Count.ToString() %></asp:Label>
                </div>
                <div class="row vendormastergrid" style="margin-left: 1px; margin-top: 0px;" id="divvendor">
                    <asp:GridView ID="gvvendor" runat="server" AutoGenerateColumns="false" CssClass="table table-responsive" ShowHeaderWhenEmpty="true" OnRowDataBound="gvvendor_RowDataBound" EmptyDataText="No Records Found">
                        <Columns>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="7%" ItemStyle-Width="7%">
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="lbedit" runat="server" Text="Edit" OnClick="lbedit_Click"></asp:LinkButton>--%>
                                    <asp:ImageButton ID="lbedit" runat="server" Text="Edit" Height="20px" OnClick="lbedit_Click" ImageUrl="~/Images/edit.png" />
                                    <asp:ImageButton ID="lbdelete" runat="server" Text="Delete" OnClick="lbdelete_Click" Height="20px" ImageUrl="~/Images/Delete.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" Visible="false">
                                <ItemTemplate>
                                    <%-- <asp:LinkButton ID="lbdelete" runat="server" Text="Delete" OnClick="lbdelete_Click"></asp:LinkButton>--%>
                                    <%--<asp:ImageButton ID="lbdelete" runat="server" Text="Delete" OnClick="lbdelete_Click" Height="20px" ImageUrl="~/Images/close.png" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="VendorID" HeaderText="VendorID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="VendorShortName" HeaderText="Vendor Code" />
                            <asp:BoundField DataField="VendorDescription" HeaderText="Vendor Description" />
                            <%--<asp:BoundField DataField="Street" HeaderText="Street" ItemStyle-CssClass="HeaderHide" HeaderStyle-CssClass="HeaderHide" />--%>
                            <asp:BoundField DataField="Address1" HeaderText="Address1" ItemStyle-CssClass="HeaderHide" HeaderStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="Address2" HeaderText="Address2" ItemStyle-CssClass="HeaderHide" HeaderStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="City" HeaderText="City" ItemStyle-CssClass="HeaderHide" HeaderStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="State" HeaderText="State" ItemStyle-CssClass="HeaderHide" HeaderStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="Zip" HeaderText="Zip" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <%-- <asp:BoundField DataField="Country" HeaderText="Country" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />--%>
                            <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="ContactName" HeaderText="Contact Name" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                            <asp:BoundField DataField="ContactPhone" HeaderText="Contact Phone" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="Fax" HeaderText="Fax" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="ContactEmail" HeaderText="Contact Email" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="POEmail" HeaderText="PO Email" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="AlternateEmail" HeaderText="Alternate Email" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="vendortype" HeaderText="Vendor Type" HeaderStyle-Width="18%" ItemStyle-Width="18%" />
                            <asp:TemplateField HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                <ItemTemplate>
                                    <asp:Label ID="lblall" runat="server" Text='<%#Eval("All") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                <ItemTemplate>
                                    <asp:Label ID="lblregularsuplies" runat="server" Text='<%#Eval("RegularSupplies") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                <ItemTemplate>
                                    <asp:Label ID="lblMachineparts" runat="server" Text='<%#Eval("MachineParts") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                <ItemTemplate>
                                    <asp:Label ID="lblserviceorder" runat="server" Text='<%#Eval("ServiceOrder") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                <ItemTemplate>
                                    <asp:Label ID="lblBuildingMaintenance" runat="server" Text='<%#Eval("BuildingMaintenance") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                <ItemTemplate>
                                    <asp:Label ID="lblIT" runat="server" Text='<%#Eval("IT") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkactive" runat="server" Checked='<%#Convert.ToBoolean( Eval("IsActive").ToString() )%>' OnCheckedChanged="chkactive_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Active" HeaderStyle-Width="4%" ItemStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" Text='<%#Eval("Active") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide">
                                <ItemTemplate>
                                    <asp:Label ID="lblXtn" runat="server" Text='<%#Eval("Xtn") %>' />
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

            <div id="div_ADDContent" runat="server" style="display: none;">
                <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6"></div>
                    <div class="col-sm-6 col-md-6 col-lg-6" align="right">
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="EmptyValue" OnClick="btnsave_Click" />
                        <asp:Button ID="btnprintdt" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnprintdt_Click" />
                        <asp:Button ID="btncanceladd" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btncanceladd_Click" />
                    </div>
                </div>
                <div class="well well-sm" style="margin-top: 3px;">
                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:Label ID="lblstreet" runat="server" Text="Vendor Code"></asp:Label><span style="color: red">*</span>
                                <div class="input-icon right i">
                                    <i class="fa fa-user"></i>
                                    <asp:TextBox ID="txtvendorID" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="Reqvendorcode" runat="server" ControlToValidate="txtvendorID"
                                        ValidationGroup="EmptyValue" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-1 col-md-1 col-lg-1"></div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblvendorName" runat="server" Text="Vendor Description"></asp:Label><span style="color: red">*</span>
                                <div class="input-icon right i">
                                    <i class="fa fa-user"></i>
                                    <asp:TextBox ID="txtvendorname" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="Reqvendordesc" runat="server" ControlToValidate="txtvendorname"
                                        ValidationGroup="EmptyValue" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lbladdress1" runat="server" Text="Address 1"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-street-view"></i>
                                    <asp:TextBox ID="txtaddress1" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lbladdress2" runat="server" Text="Address 2"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-street-view"></i>
                                    <asp:TextBox ID="txtaddress2" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblcity" runat="server" Text="City"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-street-view"></i>
                                    <asp:TextBox ID="txtcity" runat="server" CssClass="form-control"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender FilterType="UppercaseLetters, LowercaseLetters" runat="server" TargetControlID="txtcity"></ajax:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblstate" runat="server" Text="State"></asp:Label>
                                <div class="input-icon right i">
                                    <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3" style="display: none;">
                            <div class="form-group">
                                <asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-street-view"></i>
                                    <asp:TextBox ID="txtcountry" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="lblzipcode" runat="server" Text="Zip Code"></asp:Label>
                                </div>
                                <div class="col-sm-6" style="padding-right: 0px;">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtzipcode" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'99999'"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="Regzip" runat="server"
                                            ErrorMessage="Invalid format" ControlToValidate="txtzipcode" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyValue"
                                            ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-sm-1" style="padding-left: 9px; padding-right: 0px; padding-top: 6px">- </div>
                                <div class="col-sm-5" style="padding-left: 0px;">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtzipcode1" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'9999'"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="Regzipcode1" runat="server"
                                            ErrorMessage="Invalid format" ControlToValidate="txtzipcode1" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyValue"
                                            ValidationExpression="^\d{4}$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblphone" runat="server" Text="Phone"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-phone"></i>
                                    <asp:TextBox ID="txtphone" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'(999) 999-9999'"></asp:TextBox>
                                    <%--<ajax:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtphone" runat="server"></ajax:FilteredTextBoxExtender>--%>
                                    <asp:RegularExpressionValidator ID="Regphone" runat="server"
                                        ErrorMessage="Invalid format" ControlToValidate="txtphone" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyValue"
                                        ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblxtn" runat="server" Text="Xtn"></asp:Label>
                                <div class="input-icon right">
                                    <i class="fa fa-phone"></i>
                                    <asp:TextBox ID="txtxtn" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtxtn"></ajax:FilteredTextBoxExtender>
                                    <%--  <asp:RegularExpressionValidator ID="Regxtn" runat="server" 
                                            ErrorMessage="Invalid format" ControlToValidate="txtxtn" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyValue"
                                            ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>--%>
                                    <%--<ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtxtn"></ajax:FilteredTextBoxExtender>--%>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblfax" runat="server" Text="Fax"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-fax"></i>
                                    <asp:TextBox ID="txtfax" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'(999) 999-9999'"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="Regfax" runat="server"
                                        ErrorMessage="Invalid format" ControlToValidate="txtfax" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyValue"
                                        ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblcontactperson" runat="server" Text="Contact Person"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-user"></i>
                                    <asp:TextBox ID="txtcontactperson" runat="server" CssClass="form-control"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender FilterType="LowercaseLetters,UppercaseLetters,Custom" runat="server" TargetControlID="txtcontactperson"></ajax:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblcontactph" runat="server" Text="Contact Phone"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-phone"></i>
                                    <asp:TextBox ID="txtcontactph" runat="server" CssClass="form-control input-mask" data-inputmask="'mask':'(999) 999-9999'"></asp:TextBox>
                                    <%--<ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtcontactph"></ajax:FilteredTextBoxExtender>--%>
                                    <asp:RegularExpressionValidator ID="Regcont" runat="server"
                                        ErrorMessage="Invalid format" ControlToValidate="txtcontactph" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyValue"
                                        ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblcontactemail" runat="server" Text="Contact Email"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-envelope"></i>
                                    <asp:TextBox ID="txtcontactemail" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="reqContactEmail" runat="server" ControlToValidate="txtcontactemail" ValidationGroup="EmptyValue" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                        ErrorMessage="" ControlToValidate="txtcontactemail"
                                        SetFocusOnError="true"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="EmptyValue" ForeColor="" CssClass="color" Display="Dynamic" Font-Size="0.9em"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblalternateemail" runat="server" Text="Alternate Email"></asp:Label>
                                <div class="input-icon right i">
                                    <i class="fa fa-envelope"></i>
                                    <asp:TextBox ID="txtaltenateemail" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regaltmail" runat="server"
                                        ErrorMessage="Please enter the valid Email" ControlToValidate="txtaltenateemail"
                                        SetFocusOnError="true"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="EmptyValue" ForeColor="" CssClass="color" Display="Dynamic" Font-Size="0.9em"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblpoemail" runat="server" Text="PO Email"></asp:Label><span style="color: red">*</span>
                                <div class="input-icon right i">
                                    <i class="fa fa-envelope"></i>
                                    <asp:TextBox ID="txtpoemail" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqContactEmail" runat="server" ControlToValidate="txtpoemail" ValidationGroup="EmptyValue" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regpomail" runat="server"
                                        ErrorMessage="Please enter the valid Email" ControlToValidate="txtpoemail"
                                        SetFocusOnError="true"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="EmptyValue" ForeColor="" CssClass="color" Display="Dynamic" Font-Size="0.9em"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <asp:Label ID="lblvendortype" runat="server" Text="Vendor Type :" Font-Bold="true"></asp:Label><span style="color: red">*</span><br />
                            <div class="form-group">
                                <asp:CheckBox ID="chkf" runat="server" Text="All" onclick="checkall()" />
                                <asp:CheckBox ID="chks" runat="server" Text="Service Order" onclick="chkother()" />
                                <asp:CheckBox ID="chkh" runat="server" Text="Regular Supplies" onclick="chkother()" />
                                <asp:CheckBox ID="chkm" runat="server" Text="Machine Parts" onclick="chkother()" />
                                <asp:CheckBox ID="chkBM" runat="server" Text="Building Maintenance" onclick="chkother()" />
                                <asp:CheckBox ID="chkIT" runat="server" Text="IT" onclick="chkother()" /><br />
                                <br />
                                <asp:CustomValidator ID="cusvalvendortype" ErrorMessage="" SetFocusOnError="true" Display="Dynamic"
                                    ForeColor="Red" ClientValidationFunction="Validatetxtbox" ValidationGroup="EmptyValue" runat="server" Font-Size="0.9em" />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpoemail" ValidationGroup="EmptyValue" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:CheckBox ID="chkactive" runat="server" Text="Isactive" Checked="true" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--   <div class="mypanel-body" id="DivBodyClass" runat="server">
            <asp:Panel runat="server" ID="pnlSearchBlock" DefaultButton="btnSearchVendor">
                <div class="row">
                    <div id="AddDiv" runat="server" class="col-sm-3 col-md-3 col-lg-3 col-xs-5">
                        <input id="btnAdd" type="button" class="btn btn-primary" value="Add" />
                    </div>
                    <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                         <div style="float: right; margin-left: 5px;">
                             <asp:Button ID="btnprint" runat="server" Text="Print"  class="btn btn-primary" OnClick="btnprint_Click" />
                             </div>
                        <div id="savebtn" style="display: none; margin-left: 10px; float: right;" align="right">
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="EmptyValue"
                                OnClick="btnsave_Click" />
                            <input id="btnClose" type="button" class="btn btn-success" value="Cancel" />
                             <asp:Button ID="btnprintdt" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnprintdt_Click" />
                        </div>
                        <div id="div_SearchDiv" runat="server" style="padding: 0px 0px 0px 0px;">
                            <div style="float: right; margin-left: 5px; text-align: center;">
                                <asp:Button ID="btnSearchVendor" runat="server" Text="Search"
                                    CssClass="btn btn-primary" OnClick="btnSearchVendor_Click" />
                            </div>
                            <div class="col-sm-9 col-md-7 col-lg-4 col-xs-12 input-icon right i" style="padding: 0px 0px 0px 0px; float: right">
                                <i class="fa fa-search"></i>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtSearchVendor" placeholder="Search By Vendor"></asp:TextBox>
                            </div>

                        </div>
                    </div>

                </div>
            </asp:Panel>--%>

        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center> This User doesn't have permission to view this screen</center>
            </h4>
        </div>

        <div style="display: none">
            <rsweb:ReportViewer ID="rvvendorreport" runat="server"></rsweb:ReportViewer>
        </div>

        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">Vendor
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
                        <h4 class="modal-title font-bold text-white">Vendor
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
                        <h4 class="modal-title font-bold text-white">Vendor
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
                        <asp:Button ID="btndeletepop" runat="server" Text="Yes" OnClick="btndeletepop_Click" CssClass="btn btn-danger" />
                        <%--<asp:Button ID="btncancelpop" runat="server" OnClick="btncancelpop_Click" Text="Close" class="btn btn-default ra-100" />--%>
                        <button type="button" class="btn btn-default ra-100" data-dismiss="modal" cssclass="btn btn-danger">Close</button>
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
                        <asp:Label ID="Label2" runat="server" Text="Are you sure you want to delete this Record?" />
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
