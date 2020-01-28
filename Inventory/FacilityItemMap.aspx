<%@ Page Title="" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="FacilityItemMap.aspx.cs" Inherits="Inventory.FacilityItemMap" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : FacilityItemMap.aspx 
' Type      : ASPX File 
' Description  :   To design the Facility page for add,Update and show the Facility list on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                          Reason 
  08/09/2017           V.01              Sairam.P                     New
'******************************************************************************************************/
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Common.css" rel="stylesheet" />
    <link href="Content/sumoselect.css" rel="stylesheet" />
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
    </style>

    <script>
        function Validatetxtbox() {
            var txtbxresult = false;
        <%--    var txtVendorID = document.getElementById('<%= txtVendorItemID.ClientID %>').value.trim();--%>

            //if (txtVendorID == "") {
            //    alert("Enter all mandatory Fields");
            //}
            //else {
            //    txtbxresult = true;
            //}
            return txtbxresult;
        }


        $(document).ready(function () {
            $(<%=drdItemID.ClientID%>).SumoSelect({
                selectAll: true,
                placeholder: 'Select Facility'
            });
        });

        function showfacility() {
            $('#div_SaveCancel').hide();
        }

        function editfacility() {

            $('#<%=div_AddContent.ClientID %>').show();
            $("#div_SaveCancel").show();
        }

        $(document).ready(function () {
            <%--$('#btnAdd').click(function () {
                $('#btnAdd').attr('disabled', 'disabled');
                $('#<%=div_AddContent.ClientID %>').slideToggle(200);
                $("#div_SaveCancel").slideToggle(200);
                $(<%=drdItemID.ClientID%>)[0].sumo.unSelectAll();
                document.getElementById('<%= drdItemID.ClientID %>').value = 0;
                document.getElementById(<%=hiFacilityItemID.ClientID %>).value = 0;
                clear();
            });--%>

            $('#btnCancel').click(function () {
               <%-- $('#btnAdd').removeAttr('disabled');
                $('#<%=div_AddContent.ClientID %>').slideToggle(200);
                $('#div_SaveCancel').slideToggle(200);--%>
                document.getElementById('<%= ddlFacilityID.ClientID %>').value = 0;
                document.getElementById('<%= drdItemID.ClientID %>').value = 0;
                clear();
                //document.getElementById('hdnfield').value = '0';
                //alert("--->  ");
                <%--  document.getElementById('<%= txtFacilityShortName.ClientID %>').value = ""
                document.getElementById('<%= txtFacilityName.ClientID %>').value = "";--%>

            });
        });

        function clear() {
            document.getElementById('<%= ddlFacilityID.ClientID %>').value = 0;
            document.getElementById('<%= drdItemID.ClientID %>').value = 0;
            //$('<%=drdItemID.ClientID%>')[0].sumo.unSelectItem();
            $(<%=drdItemID.ClientID%>)[0].sumo.unSelectAll();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <form runat="server">
        <ajax:ToolkitScriptManager ID="script11" runat="server" EnablePageMethods="true"></ajax:ToolkitScriptManager>

        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Facility Item Mapping
                </div>
            </div>
        </div>

        <div class="mypanel-body">
       <%--<div class="row">
            <div id="AddDiv" runat="server" class="col-sm-3 col-md-3 col-lg-3">
            <input id="btnAdd" type="button" class="btn btn-primary" value="AddNew"/>    
            </div>
           <div id="div_SaveCancel" style="display: none" class="col-sm-9 col-md-9 col-lg-9" align="right">
               <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
               <input id="btnCancel" type="button" class="btn btn-success" value="Cancel" />
           </div>
    </div>--%>
        <div id="div_AddContent" runat="server">
            <div class="row">
                <div class="col-sm-2 col-md-2 col-lg-2">
                     <span>Facility Name</span><span style="color: red">*</span>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3">
                    <div class="form-group">
                        <asp:DropDownList runat="server" ID="ddlFacilityID" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                 <div class="col-sm-2 col-md-2 col-lg-2">
                     <span>Item Name</span><span style="color: red">*</span>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3">
                    <div class="form-group">
                        <asp:ListBox ID="drdItemID" CssClass="form-control" runat="server" SelectionMode="Multiple"></asp:ListBox>
                    </div>
                </div>

                <div id="div_SaveCancel"  class="col-sm-2 col-md-2 col-lg-2" align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <input id="btnCancel" type="button" class="btn btn-success" value="Cancel" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hiFacilityItemID" runat="server" Value="0" />
            <asp:HiddenField ID="HddItemActive" runat="server" Value="" />
         <div class="row vendormastergrid" style="margin-left: 1px; margin-top: 3px;">
            <asp:GridView ID="gridItems" runat="server" AutoGenerateColumns="false"
                CssClass="table table-responsive" GridLines="None" ShowHeaderWhenEmpty="True"
                EmptyDataText="No Records Found">
                <Columns>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:ImageButton ID="lbedit" runat="server" Text="Edit" Height="20px" OnClick="lbedit_Click" ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:BoundField DataField="FacilityItemMapID" HeaderText="FacilityItemMap ID" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr" />
                    <asp:BoundField DataField="FacilityName" HeaderText="Facility Name" />
                    <asp:BoundField DataField="ItemID" HeaderText="Item Name" />
                    <asp:BoundField DataField="FacilityID" HeaderText="Facility ID" HeaderStyle-CssClass="headerstr" ItemStyle-CssClass="headerstr"  />
                    <asp:TemplateField HeaderText="Active">
                    <ItemTemplate>
                    <asp:CheckBox ID="chkactive" runat="server" Checked='<%#Convert.ToBoolean( Eval("IsActive").ToString() )%>' OnCheckedChanged="chkactive_CheckedChanged" AutoPostBack="true" />
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

           <div id="User_Permission_Message" runat="server" visible="false">
        <br />
        <br />
        <br />
        <h4>
            <center>This User doesn't have permission to view this screen</center>
        </h4>
    </div>
        
        

        <asp:Button ID="Button2" runat="server" Style="display: none" />
        <%--<ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                CancelControlID="btnNo" BackgroundCssClass="modalBackground">
            </ajax:ModalPopupExtender>--%>
        <asp:Panel ID="Panel2" runat="server" BackColor="White" Height="100px" Width="400px" Style="display: none;">
            <table width="100%" style="border: Solid 2px #605ca8; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                <tr style="background-color: #605ca8">
                    <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">Confirm Box</td>
                    <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right">
                        <a href="javascript:void(0)" onclick="closepopup()">
                            <img src="Images/Close.gif" style="border: 0px" align="right" /></a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                        <asp:Label ID="Label1" runat="server" Text="Are you sure you want to delete this Record?" />
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
        </asp:Panel>



        <asp:Panel ID="Panel1" runat="server" Style="display: none; height: auto" BackColor="#666699">
        </asp:Panel>
        <asp:Label ID="lblmsg" runat="server"></asp:Label>

        <asp:Button ID="btnmsg" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="mpemsg" runat="server" TargetControlID="btnmsg" PopupControlID="pnlmsg"
            CancelControlID="btnclose" BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlmsg" runat="server" BackColor="White" Height="100px" Width="200px" Style="display: none;">
            <table width="100%" style="border: Solid 2px #0271dd; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                <tr style="background-color: #0271dd">
                    <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">User Details</td>
                    <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right">
                        <a href="javascript:void(0)" onclick="closepopup()">
                            <img src="Images/Close.gif" style="border: 0px" align="right" /></a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                        <asp:Label ID="lblalert" runat="server" Text="Saved Successfully" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right" style="padding-right: 15px">

                        <asp:Button ID="btnclose" runat="server" Text="Ok" OnClientClick="return Validate();" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

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
                        <asp:ImageButton ID="btnYes1" OnClick="btnYes_Click" runat="server" ImageUrl="~/Images/btnyes.jpg" />
                        <asp:ImageButton ID="btnNo1" runat="server" ImageUrl="~/Images/btnNo.jpg" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

    </form>
</asp:Content>
