<%@ Page Title="Item Category" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="ItemCategory.aspx.cs" Inherits="Inventory.ItemCategory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : ItemCategory.aspx 
' Type      : ASPX File 
' Description  :   To design the ItemCategory page for add,Update and show the Item list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
   08/12/2017           V.01               Murali.M                     New
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

        .Hider {
            display: none;
        }

        .headerstr {
            display: none;
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
    <script>
        function ShowPopup(res) {

            $('[id*=lblsave]').html(res);

            $("#modalSave").modal("show");

        }
        function ShowdelPopup(res) {

            $('[id*=lbldeletepop]').html(res);

            $("#modalDelete").modal("show");

        }
        function ShowwarningPopup(res) {
            //alert('ggg');
            $('[id*=lblwarning]').html(res);

            $("#modalWarning").modal("show");
        }

        function ShowConfirmationPopup() {
            $("#modalConfirm").modal("show");
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Item Category
                </div>
            </div>
        </div>
           <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
        <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
            <asp:HiddenField runat="server" ID="hdnfield" Value="0"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hdnitemname" Value=""></asp:HiddenField>
            <div id="div_ADDContent" runat="server">
                <div class="row">
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                    </div>
                    <div id="saveoredit" class="col-sm-8 col-md-8 col-lg-8" align="right">
                        <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnsearch_Click" />
                        <asp:Button ID="btnadd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnadd_Click" />
                        <asp:Button ID="btnprint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnprint_Click" />
                        <asp:Button ID="btnCancel" runat="server" class="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </div>
                <div id="divsearch" runat="server" class="well well-sm" style="margin-top: 5px;">
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <span style="font-weight: 800;">Item Category <i class="fa fa-info-circle" title="Use % for wild card search"></i></span>
                                <%--<asp:ImageButton ID="btninfo" runat="server" Height="20px" ImageUrl="~/Images/Info.png" ToolTip="Search" />--%>
                                <asp:TextBox ID="txtitemcategory" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                        </div>
                        <div class="col-lg-4 col-md4 col-sm-4">
                            <div class="form-group">
                                <span style="font-weight: 800;">Status</span>
                                <asp:RadioButtonList ID="reactive" runat="server" RepeatDirection="Horizontal" CssClass="rbl">
                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                                <%-- <asp:CheckBox ID="chkall" runat="server" Text="All" CssClass="chk" OnCheckedChanged="chkall_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                <asp:CheckBox ID="chkact" runat="server" Text="Active" CssClass="chk" OnCheckedChanged="Chkact_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                <asp:CheckBox ID="chkinact" runat="server" Text="Inactive" CssClass="chk" OnCheckedChanged="Chkinact_CheckedChanged" AutoPostBack="true"></asp:CheckBox>--%>
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
                    <asp:Label ID="lblrowcount" runat="server">No of records : <%=grdpgroup.Rows.Count.ToString() %></asp:Label>
                </div>
                <div class="row Itemcategrid" style="margin-left: 1px;">
                    <asp:GridView ID="grdpgroup" runat="server" ShowHeaderWhenEmpty="true" OnRowDataBound="grdpgroup_RowDataBound" AutoGenerateColumns="false" overflow-y="scroll" EmptyDataText="No Records Found" CssClass="table table-responsive" OnPageIndexChanging ="grdFacility_PageIndexChanging" AllowPaging ="true" PageSize ="15">
                        <Columns>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="lbedit" runat="server" Text="Edit" OnClick="lbedit_Click"></asp:LinkButton>--%>
                                    <asp:ImageButton ID="lbedit" runat="server" Text="Edit" Height="20px" ToolTip="Edit Item Category" OnClick="lbedit_Click" ImageUrl="~/Images/edit.png" />
                                    <asp:ImageButton ID="lbldelete" runat="server" Text="Edit" Height="20px" ToolTip="Delete Item Category" OnClick="lbldelete_Click" ImageUrl="~/Images/Delete.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CategoryID" HeaderText="Item Category ID" HeaderStyle-Width="12%" ItemStyle-Width="12%" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Item Category" />
                            <%--<asp:BoundField DataField="UsageID" HeaderText="Usage" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr"/>--%>
                            <asp:BoundField DataField="Usage" HeaderText="Usage" />
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


            <div id="divItemadd" runat="server" style="display: none">
                <div class="row">
                    <div class="col-sm-8 col-md-8 col-lg-8"></div>
                    <div class="col-sm-4 col-md-4 col-lg-4" align="right">
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="EmptyField" OnClick="btnsave_Click" />
                        <asp:Button ID="btncanceladd" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btncanceladd_Click" />
                    </div>
                </div>
                <div id="divContent" class="well well-sm" style="margin-top: 5px;">
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <span style="font-weight: 800;">Item Category</span>&nbsp;<span style="color: red">*</span>
                                <asp:TextBox ID="txtGroupName" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Reqtxtgroupname" runat="server" ControlToValidate="txtGroupName"
                                    ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <span style="font-weight: 800;">Usage</span><br />
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <asp:CheckBox ID="chkstan" runat="server" Text="Standard" OnCheckedChanged="chkstan_CheckedChanged" AutoPostBack="true" />
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <asp:CheckBox ID="chknonstan" runat="server" Text="Non Standard" OnCheckedChanged="chknonstan_CheckedChanged" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div id="DivCheckbox" runat="server" class="form-group">
                                <asp:CheckBox ID="chkactive" runat="server" Text="Isactive" Checked="true" Visible="false" />
                            </div>
                        </div>
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
            <rsweb:ReportViewer ID="rvitemcategoryreport" runat="server"></rsweb:ReportViewer>
        </div>

        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">Item Category
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
                        <h4 class="modal-title font-bold text-white">Item Category
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lbldeletepop" runat="server"></asp:Label>
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
                        <h4 class="modal-title font-bold text-white">Item Category
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
                        <asp:Label ID="lblmsg" runat="server" Text="Are you sure you want to delete this Record?" />
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
