<%@ Page Title="Medical Supplies" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="MedicalSupplies.aspx.cs" Inherits="Inventory.MedicalSupplies" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : MedicalSuplies.aspx 
' Type      : ASPX File 
' Description  :   To design the MedicalSupplies page for add,Update and show the Item list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01               Murali.M                       New
  08/16/2017           V.01               Vivekanand.S                 Added ClearContent function while ADD Button click Event
  10/25/2017           V.02               Sairam.P                     Model Popup Added for Notifications
'******************************************************************************************************/
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/CDN.js/Cdn.js"></script>
    <link href="Content/Common.css" rel="stylesheet" />
    <link href="Content/sumoselect.css" rel="stylesheet" />
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

        .rblv input[type="radio"] {
            margin-left: 10px;
            margin-right: 1px;
            margin-bottom: 24px;
        }


         .gridviewPager {
           background-color: #fff;
           padding: 2px;
           margin: 2% auto;
       }

           .gridviewPager a {
               margin: auto 1%;
               border-radius: 50%;
               background-color: #545454;
               padding: 5px 10px 5px 10px;
               color: #fff;
               text-decoration: none;
               -o-box-shadow: 1px 1px 1px #111;
               -moz-box-shadow: 1px 1px 1px #111;
               -webkit-box-shadow: 1px 1px 1px #111;
               box-shadow: 1px 1px 1px #111;
           }

               .gridviewPager a:hover {
                   background-color: #337ab7;
                   color: #fff;
               }

           .gridviewPager span {
               background-color: #066091;
               color: #fff;
               -o-box-shadow: 1px 1px 1px #111;
               -moz-box-shadow: 1px 1px 1px #111;
               -webkit-box-shadow: 1px 1px 1px #111;
               box-shadow: 1px 1px 1px #111;
               border-radius: 50%;
               padding: 5px 10px 5px 10px;
           }

       


    </style>

    <script type="text/javascript">

        function jScript() {
            $('[id*=drpitemcategory]').SumoSelect({
                <%-- $(<%=drpFacilitys.ClientID%>).SumoSelect({--%>
                selectAll: true,
                placeholder: 'Select Category'
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

        function GetEachPrice() {
            var unitprice = document.getElementById('<%=txtunitvalue.ClientID%>').value;
            var qty = document.getElementById('<%=txtQty.ClientID%>').value;
            var eachprice = (unitprice / qty);
            if (isNaN(eachprice) || eachprice == "Infinity") {
                document.getElementById('<%=txtEachprice.ClientID%>').value = "";
            } else {
                document.getElementById('<%=txtEachprice.ClientID%>').value = eachprice.toFixed(2);
            }
        }

        function LoaddrdUnitpriceType() {
            var defaultunitprice = $('[id*=drdUnitpriceType]').find('option[text="$"]').val();
            $("[id*=drdUnitpriceType]").val(defaultunitprice);
        }

        function OpenBudgetPopup() {
            $find("modalAddbedget").show();
            document.getElementById("<%=txtCurrency.ClientID %>").style.borderColor = "gray";
            return false;
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
            $('[id*=lblwarning]').html(res);
            $("#modalWarning").modal("show");
        }
        function ShowConfirmationPopup() {
            $("#modalConfirm").modal("show");
        }
        function OnSelectRditemid(res) {
            var selectedval = $('#<%= rditemid.ClientID %> input:checked').val();

            if (selectedval == "1") {
                //console.log(selectedval);
                $("[id*=txtSearchItem]").attr('disabled', false);
                $("[id*=textsearchdesc]").attr('disabled', true);
                document.getElementById('<%=textsearchdesc.ClientID%>').value = "";
            }
            else if (selectedval == "2") {
                $("[id*=txtSearchItem]").attr('disabled', true);
                $("[id*=textsearchdesc]").attr('disabled', false);
                document.getElementById('<%=txtSearchItem.ClientID%>').value = "";
            }
    }

    function OnSelectrditem(res) {

        var isChecked = $(res).attr('checked') ? true : false;

        if (isChecked == false) {
            $("[id*=rdsearcate]").attr('checked', false);
            $("[id*=drpitemcategory]").attr('disabled', true);
            $("[id*=drpitemcategory]")[0].sumo.unSelectAll();
            $("[id*=rditemid]").attr('disabled', false);
            $("[id*=txtSearchItem]").attr('disabled', true);
            $("[id*=textsearchdesc]").attr('disabled', true);
            document.getElementById('<%=textsearchdesc.ClientID%>').value = "";
                document.getElementById('<%=txtSearchItem.ClientID%>').value = "";
                document.getElementById('<%=drpitemcategory.ClientID %>').value = "";
                 <%--document.getElementById('<%=rdsearcate.ClientID %>').disabled = true;
                 document.getElementById('<%= drpitemcategory.ClientID %>').value = -1;
                 document.getElementById('<%= drpitemcategory.ClientID %>').disabled = true;
                 $("[id*=drpitemcategory]").attr('disabled', true);
                 document.getElementById('<%=rditemid.ClientID %>').disabled = false;
                 document.getElementById("1").disabled = true;
                 document.getElementById("2").disabled = true;--%>
            }
        }

        function onselectrdsearcate(res) {
            var isChecked = $(res).attr('checked') ? true : false;
            if (isChecked == false) {
                $("[id*=rditem]").attr('checked', false);
                $("[id*=drpitemcategory]").attr('disabled', false);
                $("[id*=drpitemcategory]")[0].sumo.selectAll();
                $("[id*=rditemid]").attr('disabled', true);
                $("[id*=txtSearchItem]").attr('disabled', true);
                $("[id*=textsearchdesc]").attr('disabled', true);
                document.getElementById('<%=textsearchdesc.ClientID%>').value = "";
                document.getElementById('<%=txtSearchItem.ClientID%>').value = "";
            }

        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Medical Supplies
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(jScript);
                    Sys.Application.add_load(jscriptsearch);
                </script>
                <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
                    <div id="searchdiv" runat="server">
                        <div class="row">
                            <div class="col-sm-4 col-md-4 col-lg-4">
                                <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                            </div>
                            <div id="saveoredit" class="col-sm-8 col-md-8 col-lg-8" align="right">
                                <asp:Button ID="btnSearchItems" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearchItems_Click" />
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnprintsummary" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnprintsummary_Click" />
                                <asp:Button ID="btnCancel" runat="server" class="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                        <div id="divsearch" runat="server" class="well well-sm" style="margin-top: 5px;">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:RadioButton ID="rdsearcate" runat="server" RepeatDirection="Horizontal" CssClass="rbl" onclick="onselectrdsearcate(this);"></asp:RadioButton><span style="font-weight: 800;">Item Category</span>
                                        <%--<asp:RadioButton ID="rdsearcate" runat="server" RepeatDirection="Horizontal" CssClass="rbl" OnCheckedChanged="rdsearcate_CheckedChanged" AutoPostBack="true"></asp:RadioButton><span style="font-weight: 800;">Item Category</span>--%>
                                        <%--<asp:DropDownList ID="drpitemcategory" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                        <asp:ListBox ID="drpitemcategory" runat="server" CssClass="form-control" SelectionMode="Multiple" multiple="multiple" Enabled="true"></asp:ListBox>
                                    </div>
                                </div>
                                <div class="col-lg-9 col-md-9 col-sm-9">
                                    <div class="col-lg-2 col-md-1 col-sm-1">
                                        <asp:RadioButton ID="rditem" runat="server" RepeatDirection="Horizontal" CssClass="rbl" onclick="OnSelectrditem(this);"></asp:RadioButton><span style="font-weight: 800;">Item</span>
                                        <%--<asp:RadioButton ID="rditem" runat="server" RepeatDirection="Horizontal" CssClass="rbl" OnCheckedChanged="rditem_CheckedChanged" AutoPostBack="true"></asp:RadioButton><span style="font-weight: 800;">Item</span>--%>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <div class="form-group">
                                            <asp:RadioButtonList ID="rditemid" runat="server" onchange="OnSelectRditemid(this)" RepeatDirection="Vertical" CssClass="rblv" Enabled="false">
                                                <asp:ListItem Text="ItemID" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Item Description" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <div class="form-group">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSearchItem" Enabled="false"></asp:TextBox>&nbsp;
                                            <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtSearchItem"></ajax:FilteredTextBoxExtender>
                                            <asp:TextBox runat="server" CssClass="form-control" ID="textsearchdesc" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <div class="form-group">
                                            <span style="font-weight: 800;">Status</span>
                                            <asp:RadioButtonList ID="reactive" runat="server" RepeatDirection="Horizontal" CssClass="rbl">
                                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <%--<asp:CheckBox ID="chkall" runat="server" Text="All" CssClass="chk" OnCheckedChanged="chkall_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                    <asp:CheckBox ID="chkact" runat="server" Text="Active" CssClass="chk" OnCheckedChanged="Chkact_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                    <asp:CheckBox ID="chkinact" runat="server" Text="Inactive" CssClass="chk" OnCheckedChanged="Chkinact_CheckedChanged" AutoPostBack="true"></asp:CheckBox>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="div_Grid" style="margin-top: 0px;">
                            <div class="row">
                                <div class="col-sm-6 col-md-6 col-lg-6">
                                    <asp:Label ID="lblresult" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="lblrowcount" runat="server">No of records : <%=grditem.Rows.Count.ToString() %></asp:Label>
                            </div>
                            <div class="row vendormastergrid" style="margin-left: 1px;">
                                <asp:GridView ID="grditem" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="grditem_RowDataBound" AutoGenerateColumns="false" overflow-y="scroll" EmptyDataText="No Records Found" CssClass="table table-responsive" OnSelectedIndexChanged="grditem_SelectedIndexChanged" OnPageIndexChanging="grditem_PageIndexChanging" AllowPaging="true" PageSize="15">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lbedit" runat="server" Text="Edit" OnClick="lbedit_Click"></asp:LinkButton>--%>
                                                <asp:ImageButton ID="lbedit" runat="server" Text="Edit" Height="20px" ToolTip="Edit Medical Supply" OnClick="lbedit_Click" ImageUrl="~/Images/edit.png" />
                                                <asp:ImageButton ID="lblview" runat="server" Text="View" Height="20px" ToolTip="View Medical Supply" OnClick="lblview_Click" ImageUrl="~/Images/Eyeview.png" />
                                                <asp:ImageButton ID="lbldelete" runat="server" Text="Edit" Height="20px" ToolTip="Delete Medical Supply" OnClick="lbldelete_Click" ImageUrl="~/Images/Delete.png" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemID" HeaderText="Item Code" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                                        <asp:BoundField DataField="VendorDescription" HeaderText="Vendor Description" />
                                        <asp:BoundField DataField="VendorItemID" HeaderText="Vendor ItemID" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                        <asp:BoundField DataField="CategoryName" HeaderText="Item Category" HeaderStyle-Width="13%" ItemStyle-Width="13%" />
                                        <%-- <asp:BoundField DataField="VendorName" HeaderText="Vendor" />--%>
                                        <asp:TemplateField HeaderText="Unit Price" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("UnitPriceCurrency") %>'></asp:Label>
                                                <asp:Label ID="lblValue" runat="server" Text='<%# Eval("UnitPricevalue", "{0:F2}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UomName" HeaderText="UOM" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                                        <asp:BoundField DataField="QtyPack" HeaderText="Qty Pack" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
                                        <%-- <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkActive"
                                            Checked='<%#Convert.ToBoolean( Eval("IsActive").ToString() )%>' AutoPostBack="true" OnCheckedChanged="chkActive_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Active" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActive" runat="server" Text='<%# Eval("Active") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EachPrice" HeaderText="EachPrice" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
                                        <asp:BoundField DataField="NDC" HeaderText="NDC" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
                                        <asp:BoundField DataField="Standard" HeaderText="Standard" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
                                        <asp:BoundField DataField="NonStandard" HeaderText="NonStandard" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
                                        <asp:BoundField DataField="GPBillingCode" HeaderText="GPBillingCode" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
                                    </Columns>
                                    <HeaderStyle CssClass="Headerstyle" />
                                    <FooterStyle CssClass="gridfooter" />
                                    <PagerStyle CssClass="gridviewPager" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle CssClass="gridselectedrow" />
                                    <EditRowStyle CssClass="grideditrow" />
                                    <AlternatingRowStyle CssClass="gridalterrow" />
                                    <RowStyle CssClass="gridrow" />

                                </asp:GridView>
                            </div>

                        </div>
                    </div>

                    <div id="div_ADDContent" runat="server" style="display: none">
                        <div class="row" align="right" style="margin-right: 1px;">
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="EmptyField" OnClick="btnsave_Click" />
                            <asp:Button ID="btnprintdetail" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnprintdetail_Click" />
                            <asp:Button ID="btncancelsave" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btncancelsave_Click" />
                        </div>
                        <div class="well well-sm" style="margin-top: 5px;">
                            <div class="row">
                                <div class="col-sm-2 col-md-2 col-lg-2">
                                    <div class="form-group">
                                        <asp:Label ID="lblItemId" runat="server" Text="Item Code"></asp:Label>
                                        <asp:TextBox ID="txtitemid" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="reqfielditemid" runat="server" ControlToValidate="txtitemid" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="col-sm-1 col-md-1 col-lg-1"></div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lbldesc" runat="server" Text="Item Description"></asp:Label><span style="color: red">*</span>
                                        <asp:TextBox ID="txtItemDesc" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqfielditemDesc" runat="server" ControlToValidate="txtItemDesc" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblItemCategory" runat="server" Text="Item Category"></asp:Label><span style="color: red">*</span>
                                        <asp:DropDownList ID="drdItemCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="ReqfielddrdItemCategory" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drdItemCategory" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblQtyPack" runat="server" Text="Qty\Pack"></asp:Label><span style="color: red">*</span>
                                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ReqfieldQty" runat="server" ControlToValidate="txtQty" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <ajax:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtQty"></ajax:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblUoM" runat="server" Text="UOM"></asp:Label><span style="color: red">*</span>
                                        <asp:DropDownList ID="ddlUOM" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldddlUOM" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="ddlUOM" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="col-lg-6" style="padding-right: 5px; padding-left: 0px;">
                                        <span>Currency</span><span style="color: red">*</span> <span>
                                            <asp:LinkButton ID="lnkPrice" runat="server" Text="Add" OnClientClick="return OpenBudgetPopup()"></asp:LinkButton></span>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="drdUnitpriceType" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldrdUnitpriceType" ValidationGroup="EmptyField" runat="server" ForeColor="Red"
                                            ControlToValidate="drdUnitpriceType" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-lg-6" style="padding-right: 0px; padding-left: 0px; padding-top: 0px;">
                                        <span>Unit Price</span><span style="color: red">*</span>
                                        <asp:TextBox ID="txtunitvalue" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldunitvalue" runat="server" ControlToValidate="txtunitvalue" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <ajax:FilteredTextBoxExtender FilterType="Numbers,Custom" runat="server" TargetControlID="txtunitvalue" ValidChars="."></ajax:FilteredTextBoxExtender>
                                        <asp:RegularExpressionValidator ID="Regunit" runat="server"
                                            ErrorMessage="Unit Price cannot have more than 4 decimal point value" ControlToValidate="txtunitvalue" ForeColor="Red" Display="Dynamic" Font-Size="0.9em" ValidationGroup="EmptyField"
                                            ValidationExpression="^\d+(?:\.\d{0,4})?$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblEachprice" runat="server" Text="Each Price"></asp:Label><br />
                                        <asp:TextBox ID="txtEachprice" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblNDC" runat="server" Text="NDC"></asp:Label>
                                        <asp:TextBox ID="txtNDC" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <%--<div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblusage" runat="server" Text="Usage"></asp:Label><br />
                                <asp:CheckBox ID="chkstan" runat="server" Text="Standard" />
                                &nbsp;
                    <asp:CheckBox ID="chknonstan" runat="server" Text="Non Standard" />
                            </div>
                        </div>--%>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:Label ID="lblgp" runat="server" Text="Item Accounting Code"></asp:Label>
                                        <%--<asp:DropDownList ID="ddlgpbill" runat="server" CssClass="form-control"> </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtgpbill" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="form-group">
                                        <asp:CheckBox ID="chkactive" runat="server" Text="Isactive" Checked="true" Visible="false" />
                                    </div>
                                </div>
                            </div>
                            <asp:HiddenField runat="server" ID="ItemID" Value="0"></asp:HiddenField>
                            <asp:HiddenField runat="server" ID="hdnitemname" Value=""></asp:HiddenField>
                        </div>
                    </div>

                    <div id="itemmap" runat="server" style="display: none">
                        <div class="row vendormastergrid" style="margin-left: 1px;">
                            <asp:GridView ID="grditemmap" runat="server" AutoGenerateColumns="false" EmptyDataText="No Records" ShowHeaderWhenEmpty="true" CssClass="table table-responsive" GridLines="None">
                                <Columns>
                                    <%--<asp:BoundField DataField="ItemID" HeaderText="Item Code" />--%>
                                    <asp:BoundField DataField="VendorShortName" HeaderText="Vendor Short Description" />
                                    <asp:BoundField DataField="VendorItemID" HeaderText="Vendor Item ID" />
                                    <%--<asp:TemplateField HeaderText="Active">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkActive"
                                        Checked='<%#Convert.ToBoolean( Eval("IsActive").ToString() )%>' />
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
                </div>

                <asp:HiddenField ID="hdnitemid" runat="server" />
                <div id="modalSave" class="modal fade" style="position: center">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content ">
                            <div class="modal-header bg-green">
                                <h4 class="modal-title font-bold text-white">Medical Supplies
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
                                <h4 class="modal-title font-bold text-white">Medical Supplies
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
                                <h4 class="modal-title font-bold text-white">Medical Supplies
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
        <div style="display: none">
            <rsweb:ReportViewer ID="rvitemsummaryreport" runat="server"></rsweb:ReportViewer>
        </div>
        <div style="display: none">
            <rsweb:ReportViewer ID="rvitemdetailreport" runat="server"></rsweb:ReportViewer>
        </div>

        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpebudget" runat="server"
                    PopupControlID="modalAddbedget" TargetControlID="Button1"
                    BackgroundCssClass="modalBackground" BehaviorID="modalAddbedget">
                </ajax:ModalPopupExtender>
                <div id="modalAddbedget" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="btneditclose" class="close" runat="server" Text="X" />
                                <h4 class="modal-title" style="color: green; font-size: large">Currency</h4>
                            </div>
                            <div class="modal-body">
                                <div style="height: 40px">
                                    <div class="form-horizontal">
                                        <div class="col-md-6 col-sm-6">
                                            <span>Currency Name</span>
                                            <asp:TextBox ID="txtCurrency" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="Save" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="Save_Click" />
                                <asp:Button runat="server" Text="Close" CssClass="btn btn-success" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Save" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
</asp:Content>
