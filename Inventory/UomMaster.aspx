<%@ Page Title="Unit Of Measure" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="UomMaster.aspx.cs" Inherits="Inventory.UomMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : UomMaster.aspx 
' Type      : ASPX File 
' Description  :   To design the MedicalSupplies page for add,Update and show the Item list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/11/2017           V.01               Murali.M                     New
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
    </style>
    <script>
        

        $(document).ready(function () {

            $("#addnew").click(function () {
                <%--$('#addnew').attr('disabled', 'disabled');
                $('#<%=div_AddContent.ClientID %>').slideToggle(200);
                $("#saveoredit").slideToggle(200);--%>
                document.getElementById('<%= txtUomName.ClientID %>').value = "";
                document.getElementById('<%= hdnfield.ClientID %>').value = 0;
            });

            $("#btnCancel").click(function () {
               <%-- $('#addnew').removeAttr('disabled');
                $('#<%=div_AddContent.ClientID %>').slideToggle(200);
                $("#saveoredit").slideToggle(200);--%>
                document.getElementById('<%= txtUomName.ClientID %>').value = "";
                document.getElementById('<%= hdnfield.ClientID %>').value = 0;
                validationclear();
            });
        });

       <%-- function validationclear() {

            $("#<%= reqfieldUomName.ClientID %>").css("display", "none");
         
        }

        function Page_ClientValidateReset() {
            $("#<%= reqfieldUomName.ClientID %>").css("display", "block");
           

        }--%>
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

        <%--function editfacility() {
            $('#<%=div_AddContent.ClientID %>').show();
            $("#saveoredit").show();
            $('#addnew').attr('disabled', 'disabled');
        }--%>


        <%--        function Validatetxtbox() {
            var txtbxresult = false;
            var UOMName = document.getElementById('<%= txtUomName.ClientID %>').value.trim();
            if (UOMName == "") {
                alert("Please Enter the Unit Of Measure Name");
            }
            else {
                txtbxresult = true;
            }

            return txtbxresult;
        }--%>

        <%--  function Validatetxtbox() {
            var txtbxresult = false;
            var UomName = document.getElementById('<%= txtUomName.ClientID %>').value.trim();
            return txtbxresult;
        }--%>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    UOM Master
                </div>
            </div>
        </div>
     <asp:UpdatePanel ID="updmain" runat="server">
        <ContentTemplate>
             <script type="text/javascript">
                    Sys.Application.add_load(jScript);
             </script>
        <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
            <asp:HiddenField runat="server" ID="hdnfield" Value="0"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hdnuomname" Value=""></asp:HiddenField>
            <div id="div_ADDContent" runat="server">
                <div class="row">
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <asp:Label ID="lblseroutHeader" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                    </div>
                    <div class="col-sm-8 col-md-8 col-lg-8" align="right">
                        <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnsearch_Click" />
                        <asp:Button ID="btnadd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnadd_Click" />
                        <asp:Button ID="btnprint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnprint_Click" />
                        <asp:Button ID="btnCancelsearch" runat="server" class="btn btn-primary" Text="Cancel" OnClick="btnCancelsearch_Click" />
                    </div>
                </div>
                <div id="divsearch" runat="server" class="well well-sm" style="margin-top: 2px;">
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <span style="font-weight: 800;">Unit Of Measurement  <i class="fa fa-info-circle" title="Use % for wild card search"></i></span>
                                <asp:TextBox ID="txtUomNamesearch" runat="server" CssClass="form-control"></asp:TextBox>
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
                    <asp:Label ID="lblrowcount" runat="server">No of records : <%=grdUom.Rows.Count.ToString() %></asp:Label>
                </div>
                <div class="row Itemcategrid" style="margin-top: 0px;">
                    <div class="col-sm-12 col-md-12 col-lg-12" style="padding-right: 0px">
                        <asp:GridView ID="grdUom" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="grdUom_RowDataBound" overflow-y="scroll" EmptyDataText="No Records Found" CssClass="table table-responsive">
                            <Columns>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lbedit" runat="server" Text="Edit" Height="20px" OnClick="lbedit_Click" ImageUrl="~/Images/edit.png" />
                                        <asp:ImageButton ID="lbldelete" runat="server" Text="Edit" Height="20px" OnClick="lbldelete_Click" ImageUrl="~/Images/Delete.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UomID" HeaderText="UomID" HeaderStyle-Width="8%" ItemStyle-Width="8%" />
                                <asp:BoundField DataField="UomName" HeaderText="UOM Name" />
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
            <div id="divItemadd" runat="server" style="display: none">
                <div class="row">
                    <div class="col-sm-8 col-md-8 col-lg-8"></div>
                    <div class="col-sm-4 col-md-4 col-lg-4" align="right">
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="EmptyField" OnClick="btnsave_Click" />
                        <asp:Button ID="btncanceladd" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btncanceladd_Click" />
                    </div>
                </div>
                <div id="divContent" class="well well-sm" style="margin-top: 3px;">
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <span style="font-weight: 800;">Unit Of Measurement </span>&nbsp;<span style="color: red">*</span>
                                <asp:TextBox ID="txtUomName" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqfieldUomName" runat="server" ControlToValidate="txtUomName"
                                    ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
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





            <%-- <div id="div_AddContent" runat="server" class="">
                <div class="row">
                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <asp:Label ID="lblUomName" runat="server" Text="Unit Of Measurement"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-3">
                        <asp:TextBox ID="txtUomName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqfieldUomName" runat="server" ControlToValidate="txtUomName" CssClass="Validation-Msg" ValidationGroup="EmptyField" ErrorMessage="" ForeColor="Red" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div id="saveoredit" class="col-sm-7 col-md-7 col-lg-7" align="right">
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="EmptyField" OnClick="btnsave_Click"/>
                        <input type="button" id="btnCancel" class="btn btn-success" value="Clear" onclick="Page_ClientValidateReset()" />
                        <asp:Button ID="btnprint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnprint_Click" />
                    </div>
                </div>
            </div>--%>
        </div>

        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">UOM
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
                        <h4 class="modal-title font-bold text-white">UOM
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
                        <h4 class="modal-title font-bold text-white">UOM
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
        <div style="display: none">
            <rsweb:ReportViewer ID="rvuomreport" runat="server"></rsweb:ReportViewer>
        </div>
</ContentTemplate>
         </asp:UpdatePanel>
        <div id="User_Permission_Message" runat="server" visible="false">
            <br />
            <br />
            <br />
            <h4>
                <center>This User doesn't have permission to view this screen</center>
            </h4>
        </div>
</asp:Content>
