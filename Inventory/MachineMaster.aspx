<%@ Page Title="Machine Master" Language="C#" MasterPageFile="~/Inven.Master" AutoEventWireup="true" CodeBehind="MachineMaster.aspx.cs" Inherits="Inventory.MachineMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--/* 
'*******************************************************************************************************
' Itrope Technologies All rights reserved. 
' Copyright (C) 2017. Itrope Technologies. 
' Name      : MachineMaster.aspx 
' Type      : ASPX File 
' Description  :   To design the MachineMaster page for add,Update,Read,Delete and show the record on Grid.
' Modification History : 
'------------------------------------------------------------------------------------------------------'
   Date		            Version             By                                         Reason 
  08/21/2017             V.01               Mahalakshmi.S,Murali.M,Dhanasekar.C           New
  
'******************************************************************************************************/
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="Content/themes/themes.css" rel="stylesheet" />--%>
    <script src="Scripts/CDN.js/Cdn.js"></script>
    <link href="Content/Common.css" rel="stylesheet" />
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
    <script src="Scripts/notification.js"></script>
    <script type="text/javascript">
        function OpenEquipPopup(lnk) {
            document.getElementById("<%=txtEquipmentcategory.ClientID %>").style.borderColor = "#d2d6de";
            document.getElementById("<%=txtEquipmentcategory.ClientID %>").value = '';
            var ddlEquipmentCat = document.getElementById("<%=drpEquipment.ClientID %>");

            if (ddlEquipmentCat.value != '0' && lnk.id != 'body_Ibaddequip') {
                var selectedText = ddlEquipmentCat.options[ddlEquipmentCat.selectedIndex].innerHTML;
                $find("modalAddequip").show();
            }
            else if ((lnk.id != 'body_Ibaddequip' && ddlEquipmentCat.value == '0')) {
                ShowwarningPopup('Please Select Equipement Category');
            }
            else {
                $find("modalAddequip").show();
                document.getElementById("<%=drpEquipment.ClientID %>").value = "0";
            }
        var myHidden = document.getElementById('<%= hdnequipcat.ClientID %>');
            if (lnk.id == 'body_Ibdeleqip') {
                document.getElementById("<%=txtEquipmentcategory.ClientID %>").value = selectedText;
                document.getElementById('<%=btnaddequipment.ClientID %>').value = "Delete";
                myHidden.value = '1';
            }
            else if (lnk.id == 'body_Ibaddequip') {
                document.getElementById('<%=btnaddequipment.ClientID %>').value = "Save";
                myHidden.value = '0';
            }
            else {
                document.getElementById('<%=btnaddequipment.ClientID %>').value = "Save";
                document.getElementById("<%=txtEquipmentcategory.ClientID %>").value = selectedText;
                myHidden.value = '0';
            }
        return false
    }

    function OpenEquipmentSubCatPopup(img) {
        document.getElementById("<%=txtequiSubCat.ClientID %>").style.borderColor = "#d2d6de";
        document.getElementById("<%=txtequiSubCat.ClientID %>").value = '';
        var ddlEquipmentSubCat = document.getElementById("<%=drpEquipmentSubCategory.ClientID %>");
        var ddlcategory = document.getElementById("<%=drpEquipment.ClientID %>");
        var selectedText = '';
        if (ddlcategory.value != '0') {
            if (ddlEquipmentSubCat.value != '0' && img.id != 'body_imgeequipSubadd') {
                selectedText = ddlEquipmentSubCat.options[ddlEquipmentSubCat.selectedIndex].innerHTML;
                $find("modalSubCategory").show();
            }
            else if (img.id != 'body_imgeequipSubadd') {
                // alert("Please Select EquipementList");
                ShowwarningPopup('Please Select Equipement Sub Category');
            }
            else {
                $find("modalSubCategory").show();
                document.getElementById("<%=drpEquipmentSubCategory.ClientID %>").value = "0";
            }
    }
    else {
            //alert("Please Select EquipementCategory");
        ShowwarningPopup('Please Select Equipement Category');
    }
    var myhdn = document.getElementById('<%= hdnequipSubCat.ClientID %>');
        if (img.id == 'body_imgequiplstSubdelete') {
            document.getElementById("<%=txtequiSubCat.ClientID %>").value = selectedText;
            document.getElementById('<%=btnequipSubCatsave.ClientID %>').value = "Delete";
            myhdn.value = '1';
        }
        else if (img.id == 'body_imgeequipSubadd') {
            document.getElementById('<%=btnequipSubCatsave.ClientID %>').value = "Save";
                myhdn.value = '0';
            }
            else {
                document.getElementById('<%=btnequipSubCatsave.ClientID %>').value = "Save";
                document.getElementById("<%=txtequiSubCat.ClientID %>").value = selectedText;
                myhdn.value = '0';
            }
        return false;
    }

    function OpenEquiplistPopup(img) {
        document.getElementById("<%=txtequilist.ClientID %>").style.borderColor = "#d2d6de";
        document.getElementById("<%=txtequilist.ClientID %>").value = '';
        var ddlEquipmentList = document.getElementById("<%=drpEquipmentlist.ClientID %>");
        var ddlEquipmentSubCat = document.getElementById("<%=drpEquipmentSubCategory.ClientID %>");
        var selectedText = '';
        if (ddlEquipmentSubCat.value != '0') {
            if (ddlEquipmentList.value != '0' && img.id != 'body_imgeequipadd') {
                selectedText = ddlEquipmentList.options[ddlEquipmentList.selectedIndex].innerHTML;
                $find("modallist").show();
            }
            else if (img.id != 'body_imgeequipadd') {
                // alert("Please Select EquipementList");
                ShowwarningPopup('Please Select EquipementList');
            }
            else {
                $find("modallist").show();
                document.getElementById("<%=drpEquipmentlist.ClientID %>").value = "0";
            }
    }
    else {
            //alert("Please Select EquipementCategory");
        ShowwarningPopup('Please Select Equipement Sub Category');
    }
    var myhdn = document.getElementById('<%= hdnequiplist.ClientID %>');
        if (img.id == 'body_imgequiplstdelete') {
            document.getElementById("<%=txtequilist.ClientID %>").value = selectedText;
            document.getElementById('<%=btnequiplistsave.ClientID %>').value = "Delete";
            myhdn.value = '1';
        }
        else if (img.id == 'body_imgeequipadd') {
            document.getElementById('<%=btnequiplistsave.ClientID %>').value = "Save";
                myhdn.value = '0';
            }
            else {
                document.getElementById('<%=btnequiplistsave.ClientID %>').value = "Save";
                document.getElementById("<%=txtequilist.ClientID %>").value = selectedText;
                myhdn.value = '0';
            }
        return false;
    }

   <%-- function Del() {
        $('#<%=deletebtn.ClientID %>').hide();
            $('#<%=savebtn.ClientID %>').show();
            var myHidden = document.getElementById('<%= hdndelete.ClientID %>');
            myHidden.value = '1';
        }--%>
        function CloseEquiPopup() {
            $('#<%=div_ADDContent.ClientID %>').show();
            $find("modalAddequip").hide();
        }
        function CloseEquiSubCatPopup() {
            $('#<%=div_ADDContent.ClientID %>').show();
            $find("modalSubCategory").hide();
        }
        function CloseEquilistPopup() {
            $('#<%=div_ADDContent.ClientID %>').show();
            $find("modallist").hide();
        }
        function Open(lnk) {
            var row = lnk.parentNode.parentNode;
            grid_rowindex = row.rowIndex - 1;
            var a = document.getElementById('<%=hdndelmachineID.ClientID%>').value += (row.rowIndex - 1) + ',';
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
        function validationclear() {

            $("#<%= reqequip.ClientID %>").css("display", "none");
            $("#<%= reqmanufacturer.ClientID %>").css("display", "none");
            $("#<%= reqequplst.ClientID %>").css("display", "none");
            $("#<%= reqmanyear.ClientID %>").css("display", "none");
            $("#<%= reqmodel.ClientID %>").css("display", "none");
            $("#<%= reqserialno.ClientID %>").css("display", "none");

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
        
        <div class="page-title-breadcrumb">
            <div class="page-header">
                <div class="page-header page-title">
                    Machine Master
                </div>
            </div>
        </div>

        <div class="mypanel-body" style="padding: 5px 15px 15px 15px;">
            <asp:HiddenField runat="server" ID="hdnfield" Value="0"></asp:HiddenField>
            <asp:HiddenField ID="hiddenMachineID" Value="0" runat="server" />
            <asp:HiddenField ID="hdnequipcat" Value="0" runat="server" />
            <asp:HiddenField ID="hdnequiplist" Value="0" runat="server" />
            <asp:HiddenField ID="hdnequipSubCat" Value="0" runat="server" />
            <asp:HiddenField ID="hdndelete" Value="0" runat="server" />
            <asp:HiddenField ID="hdndelmachineID" runat="server" />
            <asp:HiddenField ID="hdnroleID" runat="server" />
            <asp:HiddenField ID="hdnEditPermission" runat="server" />
            <asp:HiddenField ID="hdnViewPermission" runat="server" />
            <asp:HiddenField ID="hdnShowpanel" runat="server" Value="0" />
            <asp:HiddenField ID="hdnmachinename" runat="server" Value="" />

            <div class="row">
                <div class="col-sm-4 col-md-4 col-lg-4">
                    <asp:Label ID="lblseroutHeader" runat="server" CssClass="page-header page-title" Text="Search Criteria"></asp:Label>
                </div>
                <div class="col-sm-8 col-md-8 col-lg-8" style="margin-bottom: 5px;">
                    <div align="right" id="deletebtn" style="display: block;" runat="server">
                        <asp:Button ID="Search" runat="server" CssClass="btn btn-primary" Text="Search" ValidationGroup="EmptyFieldAdd" OnClick="Search_Click" />
                     <%--   <input id="btnAdd" type="button" class="btn btn-primary" value="Add" />--%>
                        <asp:Button ID="btnadd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnadd_Click" />
                        <asp:Button ID="btnsearchprint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnsearchprint_Click" />
                        <asp:Button ID="btncancelsch" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btncancelsch_Click" />
                        <%--<input id="btndelete" type="button" class="btn btn-primary" value="Delete" onclick="Del();" />--%>
                        <%--<asp:Button ID="btndelete" runat="server" CssClass="btn btn-primary" Text="Delete" OnClick="btndelete_Click" />--%>
                    </div>
                    <div id="savebtn" runat="server" style="display: none; margin-left: 10px; float: right;" align="right">
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-success" ValidationGroup="ErrorFieldSave" OnClick="btnsave_Click" />
                        <asp:Button ID="btnprint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnprint_Click" />
                        <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="btn btn-primary"  OnClick="btnClose_Click" />
                    </div>
                </div>
            </div>
            <div class="well well-sm" id="searchdiv" runat="server">
                <div class="row">
                    <div class="col-lg-4">
                        <div class="form-group">
                            <span style="font-weight: 800;">Corporate</span>
                            <asp:DropDownList ID="drpCorporate" runat="server" CssClass="form-control" placeholder="--Select Corporate--" AutoPostBack="true" OnSelectedIndexChanged="drpcor_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqddlCorporateadd" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                ControlToValidate="drpCorporate" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="form-group">
                            <span style="font-weight: 800;">Facility</span>
                            <asp:DropDownList ID="drpFacility" runat="server" CssClass="form-control" placeholder="--Select Facility--" OnSelectedIndexChanged="drpFacility_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:RequiredFieldValidator InitialValue="0" ID="reqfacility" ValidationGroup="EmptyFieldAdd" runat="server" ForeColor="Red"
                                ControlToValidate="drpFacility" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
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
       
            <div id="div_ADDContent" runat="server" style="display: none">
                <div class="well well-sm">
                    <div class="row">

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblFacility" runat="server" Text="Facility"></asp:Label><span></span><br />
                                <asp:Label ID="lblsubfacility" runat="server" CssClass="form-control"></asp:Label>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblEquipment" runat="server" Text="Equipment Category"></asp:Label><span style="color: red">*</span>
                                <asp:ImageButton ID="Ibaddequip" runat="server" Height="17px" ToolTip="Add Equipment Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Add1.png" OnClientClick="return OpenEquipPopup(this)" />
                                <asp:ImageButton ID="lbeditequip" runat="server" Height="17px" ToolTip="Edit Equipment Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/edit.png" OnClientClick="return OpenEquipPopup(this)" />
                                <asp:ImageButton ID="Ibdeleqip" runat="server" Height="17px" ToolTip="Delete Equipment Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClientClick="return OpenEquipPopup(this)" />
                                <asp:UpdatePanel ID="upnlequip" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="drpEquipment" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpEquipment_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="reqequip" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                            ControlToValidate="drpEquipment" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="drpEquipment" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblEquipmentSubCategory" runat="server" Text="Equipment Sub Category"></asp:Label><span style="color: red">*</span>
                                <asp:ImageButton ID="imgeequipSubadd" runat="server" Height="17px" ToolTip="Add Equipment Sub Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Add.png" OnClientClick="return OpenEquipmentSubCatPopup(this)" />
                                <asp:ImageButton ID="imgequlstSubedit" runat="server" Height="17px" ToolTip="Edit Equipment Sub Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/edit.png" OnClientClick="return OpenEquipmentSubCatPopup(this)" />
                                <asp:ImageButton ID="imgequiplstSubdelete" runat="server" Height="17px" ToolTip="Delete Equipment Sub Category" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClientClick="return OpenEquipmentSubCatPopup(this)" />
                                <asp:UpdatePanel ID="upnlequSub" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="drpEquipmentSubCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpEquipmentSubCategory_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="reqequpSub" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                            ControlToValidate="drpEquipmentSubCategory" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="drpEquipmentSubCategory" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblEquipmentlist" runat="server" Text="Equipment List"></asp:Label><span style="color: red">*</span>
                                <asp:ImageButton ID="imgeequipadd" runat="server" Height="17px" ToolTip="Add Equipment List" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Add.png" OnClientClick="return OpenEquiplistPopup(this)" />
                                <asp:ImageButton ID="imgequlstedit" runat="server" Height="17px" ToolTip="Edit Equipment List" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/edit.png" OnClientClick="return OpenEquiplistPopup(this)" />
                                <asp:ImageButton ID="imgequiplstdelete" runat="server" Height="17px" ToolTip="Delete Equipment List" Style="margin-bottom: 0px; margin-left: 5px;" ImageUrl="~/Images/Delete.png" OnClientClick="return OpenEquiplistPopup(this)" />
                                <asp:UpdatePanel ID="upnlequlist" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="drpEquipmentlist" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="reqequplst" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                            ControlToValidate="drpEquipmentlist" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>
                    <div class="row">

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblWarrenty" runat="server" Text="Warranty"></asp:Label><%--<span style="color: red">*</span>--%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtwarrenty"
                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-]\d{2}$"
                                    runat="server" ErrorMessage="Invalid Format(eg.MM/YY)" SetFocusOnError="true" ForeColor="Red" Style="margin-left: 4px;" ValidationGroup="ErrorFieldSave">
                                </asp:RegularExpressionValidator>
                                <ajax:FilteredTextBoxExtender ID="Filteredtxtwarrenty" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789/-." runat="server" TargetControlID="txtwarrenty">
                                </ajax:FilteredTextBoxExtender>
                                <asp:TextBox ID="txtwarrenty" runat="server" CssClass="form-control" MaxLength="5" placeholder="MM/YY"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblmanufacturer" runat="server" Text="Manufacturer"></asp:Label><span style="color: red">*</span>
                                <asp:TextBox ID="txtmanufacturer" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqmanufacturer" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                    ControlToValidate="txtmanufacturer" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lablegpcode" runat="server" Text="GPBilling Code"></asp:Label><%--<span style="color: red">*</span>--%>
                                <asp:TextBox ID="txtgpcode" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblmanufactureryear" runat="server" Text="Manufacturer Year"></asp:Label><span style="color: red">*</span>
                                <asp:RangeValidator ID="rvYear" runat="server" ControlToValidate="txtyear" MaximumValue="3000" MinimumValue="1000" ErrorMessage="Invalid Year" ForeColor="Red" Type="Integer" ValidationGroup="ErrorFieldSave"></asp:RangeValidator>
                                <asp:TextBox ID="txtyear" runat="server" CssClass="form-control" MaxLength="4" placeholder="YYYY"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="fteExtender" FilterType="Numbers" FilterMode="ValidChars" ValidChars="0123456789" runat="server" TargetControlID="txtyear">
                                </ajax:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="reqmanyear" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                    ControlToValidate="txtyear" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                                <%--<ajax:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txtyear" runat="server"></ajax:FilteredTextBoxExtender>--%>
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblmodel" runat="server" Text="Model #"></asp:Label><span style="color: red">*</span>
                                <asp:TextBox ID="txtmodel" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqmodel" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                    ControlToValidate="txtmodel" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblhours" runat="server" Text="Hours on the Machine"></asp:Label><%--<span style="color: red">*</span>--%>
                                <asp:UpdatePanel ID="upnlhours" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txthours" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender FilterType="Numbers" TargetControlID="txthours" runat="server"></ajax:FilteredTextBoxExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="lblserial" runat="server" Text="Serial No"></asp:Label><span style="color: red">*</span>
                                <asp:TextBox ID="txtserial" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqserialno" ValidationGroup="ErrorFieldSave" runat="server" ForeColor="Red"
                                    ControlToValidate="txtserial" ErrorMessage="" SetFocusOnError="true" Font-Size=".9em" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <asp:CheckBox ID="chkactive" runat="server" Text="Isactive" Checked="true" Visible="false" />
                                <asp:Label ID="lblstatus" runat="server" Text="Restore" Visible="false"></asp:Label>
                                <%-- <asp:Button ID="btnrestore" runat="server" Text="Restore" Visible="false" CssClass="btn btn-primary" OnClick="btnrestore_Click" />--%>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div id="divmachine" runat="server">
                <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <asp:Label ID="lblschrsult" runat="server" Visible="true" CssClass="page-header page-title" Text="Search Result"></asp:Label>
                    </div>
                </div>
                <div>
                    <asp:Label ID="lblrowcount" runat="server">No of records : <%=gvmachinemaster.Rows.Count.ToString() %></asp:Label>
                </div>
                <div class="row Itemcategrid" style="margin-left: 1px;">
                    <%--<div class="row" style="margin-left: 1px; margin-top: 0px;" id="divmachine" runat="server">--%>
                    <asp:GridView ID="gvmachinemaster" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnRowDataBound="gvmachinemaster_RowDataBound" overflow-y="scroll" EmptyDataText="No Records Found" CssClass="table table-responsive">
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="lbedit" runat="server" Text="Edit" OnClick="lbedit_Click"></asp:LinkButton>--%>
                                    <asp:ImageButton ID="lbedit" runat="server" Text="Edit" Height="20px" ToolTip="Edit" ImageUrl="~/Images/edit.png" OnClick="lbedit_Click" />
                                    <asp:ImageButton ID="lbldelete" runat="server" Text="Edit" Height="20px" ToolTip="Delete" OnClick="lbldelete_Click" ImageUrl="~/Images/Delete.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MachineID" HeaderText="MachineID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="EquipmentCategory" HeaderText="Equipment Category" />
                            <asp:BoundField DataField="EquipmentSubCategory" HeaderText="Equipment Sub Category" />
                            <asp:BoundField DataField="EquipementList" HeaderText="Equipment" />
                            <asp:BoundField DataField="Manufacturer" HeaderText="Manufacturer" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
                            <asp:BoundField DataField="Manufacturedyear" HeaderText="Make" />
                            <asp:BoundField DataField="Model" HeaderText="Model" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="SerialNo" HeaderText="Sl.No" />
                            <asp:BoundField DataField="Hoursonthemachine" HeaderText="Hours" />
                            <asp:BoundField DataField="Warranty" HeaderText="Warranty" />
                            <asp:BoundField DataField="GpAccountCode" HeaderText="GpAccountCode" HeaderStyle-CssClass="HeaderHide" ItemStyle-CssClass="HeaderHide" />
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
        <div id="User_Permission_Message" runat="server" visible="false">

            <br />
            <br />
            <br />
            <h4>
                <center> This User doesn't have permission to view this screen</center>
            </h4>
        </div>

        <div id="modalSave" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-green">
                        <h4 class="modal-title font-bold text-white">Machine Master
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
                        <h4 class="modal-title font-bold text-white">Machine Master
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
                        <h4 class="modal-title font-bold text-white">Machine Master
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
                        <asp:ImageButton ID="ImageButton3" runat="server" CssClass="btn btn-danger" AlternateText="Yes" OnClick="Imageremoveyes_Click" />
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
                        <asp:ImageButton ID="ImageButton4" runat="server" CssClass="btn btn-danger" AlternateText="Yes" OnClick="imgreyes_Click" />
                        <asp:ImageButton ID="ImageButton7" runat="server" CssClass="btn btn-default ra-100" AlternateText="Close" OnClick="imgreno_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div id="panelconfirm" class="modal fade" style="position: center">
            <div class="modal-dialog modal-sm">
                <div class="modal-content ">
                    <div class="modal-header bg-red">
                        <h4 class="modal-title font-bold text-white">Delete Confirmation</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <p>Do you want to delete this record <span id="spnName2"></span>?.</p>
                    </div>
                    <div class="modal-footer">
                        <asp:ImageButton ID="ImageButton5" runat="server" CssClass="btn btn-danger" AlternateText="Yes" OnClick="Imageremoveyes_Click" />
                        <asp:ImageButton ID="ImageButton6" runat="server" CssClass="btn btn-default ra-100" AlternateText="Close" OnClick="imgdiagremove_Click" />
                    </div>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="updmain" runat="server">
            <ContentTemplate>
                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeequipment" runat="server"
                    PopupControlID="modalAddequip" TargetControlID="Button1"
                    BackgroundCssClass="modalBackground" BehaviorID="modalAddequip">
                </ajax:ModalPopupExtender>
                <div id="modalAddequip" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <%--   <asp:Button ID="btnpopupclose" class="close" runat="server" Text="X" OnClientClick="CloseEquiPopup();" />--%>
                                <h4 class="modal-title" style="color: green; font-size: large">Equipment Category</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Panel runat="server" ID="Panel1" DefaultButton="btnaddequipment">
                                    <div style="height: 40px">
                                        <div class="form-horizontal">
                                            <div class="col-md-6 col-sm-6">
                                                <span>Equipment Category </span>
                                                <asp:TextBox ID="txtEquipmentcategory" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnaddequipment" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnaddequipment_Click" />
                                <asp:Button runat="server" Text="Close" CssClass="btn btn-success" OnClientClick="CloseEquiPopup();" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnaddequipment" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpnlsubCat" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="Button3" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeequipSubCat" runat="server"
                    PopupControlID="modalSubCategory" TargetControlID="Button3"
                    BackgroundCssClass="modalBackground" BehaviorID="modalSubCategory">
                </ajax:ModalPopupExtender>
                <div id="modalSubCategory" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <%-- <asp:Button ID="btneqlst" class="close" runat="server" Text="X" OnClientClick="CloseEquilistPopup()"  />--%>
                                <h4 class="modal-title" style="color: green; font-size: large">Equipment Sub Category</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Panel runat="server" ID="pnlepuipkSub" DefaultButton="btnequipSubCatsave">
                                    <div style="height: 40px">
                                        <div class="form-horizontal">
                                            <div class="col-md-6 col-sm-6">
                                                <span>Equipment Sub Category</span>
                                                <asp:TextBox ID="txtequiSubCat" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnequipSubCatsave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnequipSubCatsave_Click" />
                                <%--  <asp:Button runat="server" Text="Close" CssClass="btn btn-success" OnClientClick="CloseEquilistPopup()" />--%>
                                <input type="button" value="Close" class="btn btn-success" onclick="CloseEquiSubCatPopup()" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="Upnlsub" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="Button2" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpeequiplist" runat="server"
                    PopupControlID="modallist" TargetControlID="Button2"
                    BackgroundCssClass="modalBackground" BehaviorID="modallist">
                </ajax:ModalPopupExtender>
                <div id="modallist" style="display: none;">
                    <div class="modal-dialog" style="width: 350px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <%-- <asp:Button ID="btneqlst" class="close" runat="server" Text="X" OnClientClick="CloseEquilistPopup()"  />--%>
                                <h4 class="modal-title" style="color: green; font-size: large">Equipment List</h4>
                            </div>
                            <div class="modal-body">
                                <asp:Panel runat="server" ID="pnlepuipk" DefaultButton="btnequiplistsave">
                                    <div style="height: 40px">
                                        <div class="form-horizontal">
                                            <div class="col-md-6 col-sm-6">
                                                <span>Equipment List </span>
                                                <asp:TextBox ID="txtequilist" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnequiplistsave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnequiplistsave_Click" />
                                <%--  <asp:Button runat="server" Text="Close" CssClass="btn btn-success" OnClientClick="CloseEquilistPopup()" />--%>
                                <input type="button" value="Close" class="btn btn-success" onclick="CloseEquilistPopup()" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>



        <asp:Button ID="btnconfirm" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="mpeconfirm"
            runat="server" TargetControlID="btnconfirm" PopupControlID="pnlconfirm"
            BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlconfirm" runat="server" BackColor="White" CssClass="panel"
            Style="display: none;">
            <table style="border: Solid 2px #f0ad4e; width: 100%; height: 100%"
                cellpadding="0" cellspacing="0">
                <tr style="background-color: #f0ad4e">
                    <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">Confirm </td>
                    <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right">
                        <%--<a href="javascript:void(0)"
                            onclick="closepopup()">
                            <img src="Images/Close.gif"
                                style="border: 0px" align="right" id="imgdelclose" /></a>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                        <asp:Label ID="Label7" runat="server" Text="Do you want to Delete this Record?" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right" style="padding-right: 15px">
                        <asp:ImageButton ID="Imageremoveyes"
                            OnClick="Imageremoveyes_Click" runat="server" ImageUrl="~/Images/btnyes.jpg" />
                        <asp:ImageButton ID="imgdiagremove"
                            runat="server" ImageUrl="~/Images/btnNo.jpg" OnClick="imgdiagremove_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
       
        <asp:Button ID="btncnfre" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="mperestore"
            runat="server" TargetControlID="btncnfre" PopupControlID="pnlrestore"
            BackgroundCssClass="modalBackground" Enabled="True">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlrestore" runat="server" BackColor="White" CssClass="panel"
            Style="display: none;">
            <table style="border: Solid 2px #f0ad4e; width: 100%; height: 100%"
                cellpadding="0" cellspacing="0">
                <tr style="background-color: #f0ad4e">
                    <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">Confirm </td>
                    <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right">
                        <%--<a href="javascript:void(0)"
                            onclick="closepopup()">
                            <img src="Images/Close.gif"
                                style="border: 0px" align="right" id="imgre" /></a>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                        <asp:Label ID="Label1" runat="server" Text="Do you want to Restore this Record?" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right" style="padding-right: 15px">
                        <asp:ImageButton ID="imgreyes" runat="server" ImageUrl="~/Images/btnyes.jpg" OnClick="imgreyes_Click" />
                        <asp:ImageButton ID="imgreno" runat="server" ImageUrl="~/Images/btnNo.jpg" OnClick="imgreno_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:UpdatePanel ID="upnldelcat" runat="server">
            <ContentTemplate>
                <asp:Button ID="btncatde" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="mpecatdel"
                    runat="server" TargetControlID="btncatde" PopupControlID="pnlcardel"
                    BackgroundCssClass="modalBackground" BehaviorID="pnlcardel" Enabled="true">
                </ajax:ModalPopupExtender>
                <asp:Panel ID="pnlcardel" runat="server" BackColor="White" CssClass="panel" Style="display: none;">
                    <table style="border: Solid 2px #f0ad4e; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                        <tr style="background-color: #f0ad4e">
                            <td style="height: 10%; color: White; font-weight: bold; padding: 3px; font-size: larger; font-family: Calibri; color: white;" align="Left">Confirm </td>
                            <td style="color: White; font-weight: bold; padding: 3px; font-size: larger" align="Right">
                                <%-- <a href="javascript:void(0)"
                                    onclick="closepopup()">
                                    <img src="Images/Close.gif" style="border: 0px" align="right" id="imgcardel" /></a>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" style="padding: 5px; font-family: Calibri">
                                <asp:Label ID="lbldelmessage" runat="server" Text="Do you want to Delete this Record?" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right" style="padding-right: 15px">
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/btnyes.jpg" OnClick="Imageremoveyes_Click" />
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/btnNo.jpg" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div style="display: none">
                    <rsweb:ReportViewer ID="rvmachinesummaryreport" runat="server"></rsweb:ReportViewer>
                </div>
                <div style="display: none">
                    <rsweb:ReportViewer ID="rvmachineDetailreport" runat="server"></rsweb:ReportViewer>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    
</asp:Content>
