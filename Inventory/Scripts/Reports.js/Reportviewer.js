function jScript() {
    $('[id*=drpcorsearch]').SumoSelect({
        selectAll: true,
        placeholder: 'Select Corporate'
    });

    $('[id*=drpfacilitysearch]').SumoSelect({
        selectAll: true,
        placeholder: 'Select Facility'
    });

    $('[id*=drpvendorsearch]').SumoSelect({
        selectAll: true,
        placeholder: 'Select Vendor'
    });

    $('[id*=drpcategory]').SumoSelect({
        selectAll: true,
        placeholder: 'Select Status'
    });

    $('[id*=drpitem]').SumoSelect({
        selectAll: true,
        placeholder: 'Select Status'
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


function ShowwarningPopup(res) {
    $('[id*=lblwarning]').html(res);
    $("#modalWarning").modal("show");

}

var table;
var ddsubtype;
var ddtype;
var ortype;
var ObjID = [];
var CID = [];
var IID = [];
var FID = [];
var corp = [];
var VID = [];
CorpID = '';
FacilityID = '';
VendorID = '';
CategoryID = '';
ItemID = '';
Month = '';
DateFrom = '';
DateTo = '';
ddtype = '';
col = '';

function GetDropID() {

    ObjID = [];
    CID = [];
    IID = [];
    FID = [];
    corp = [];
    VID = [];
    CorpID = "";

  
    $('[id*=drpcorsearch] option:selected').each(function (i) {
        ObjID.push($(this).val());
        corp.push($(this).val());
        //objName.push($(this).text());
    });

    
    var $this = $(this);
    for (var i = 0; i < ObjID.length; i++) {
        if (i == ObjID.length - 1)
            CorpID += ObjID[i]
        else
            CorpID += ObjID[i] + ','
    };


    ObjID = [];
    FacilityID = '';
    $('[id*=drpfacilitysearch] option:selected').each(function (i) {
        ObjID.push($(this).val());
        FID.push($(this).val());
        //objName.push($(this).text());
    });

    var $this = $(this);
    for (var i = 0; i < ObjID.length; i++) {
        if (i == ObjID.length - 1)
            FacilityID += ObjID[i]
        else
            FacilityID += ObjID[i] + ','
    };


    ObjID = [];
    VendorID = '';
    $('[id*=drpvendorsearch] option:selected').each(function (i) {
        ObjID.push($(this).val());
        VID.push($(this).val());
        //objName.push($(this).text());
    });

    var $this = $(this);
    for (var i = 0; i < ObjID.length; i++) {
        if (i == ObjID.length - 1)
            VendorID += ObjID[i]
        else
            VendorID += ObjID[i] + ','
    };

    CategoryID = '';
    ObjID = [];
    $('[id*=drpcategory] option:selected').each(function (i) {
        ObjID.push($(this).val());
        //objName.push($(this).text());
        CID.push($(this).val());
    });

    var $this = $(this);
    for (var i = 0; i < ObjID.length; i++) {
        if (i == ObjID.length - 1)
            CategoryID += ObjID[i]
        else
            CategoryID += ObjID[i] + ','
    };


    ObjID = [];
    ItemID = '';
    $('[id*=drpitem] option:selected').each(function (i) {
        ObjID.push($(this).val());
        IID.push($(this).val());
        //objName.push($(this).text());
    });

    var $this = $(this);
    for (var i = 0; i < ObjID.length; i++) {
        if (i == ObjID.length - 1)
            ItemID += ObjID[i]
        else
            ItemID += ObjID[i] + ','
    };


    $(document).ready(function () {
        Month = $('[id*=txtMonth]').val();
        DateFrom = $('[id*=txtDateFrom]').val();
        DateTo = $('[id*=txtDateTo]').val();
       
    });

    ddtype = $('option:selected', '[id*=drpreport]').val();
    ddsubtype = $('option:selected', '[id*=drpsubreport]').val();
    ortype = $('option:selected', '[id*=drpordertype]').val();

}


$(document).ready(function () {
    $("[id*=drpdaterange]").change(function () {
        var res = $(this).val();
        if (res == '1') {
            $('[id*=txtMonth]').val('');
            $('[id*=txtDateFrom]').val('');
            $('[id*=txtDateTo]').val('');
            $('[id*=divmonth]').css("display", "block");
        }
        else {
            $('[id*=divmonth]').css("display", "none");
        }
        if (res == '2') {
            $('[id*=txtMonth]').val('');
            $('[id*=txtDateFrom]').val('');
            $('[id*=txtDateTo]').val('');
            $('[id*=divdate]').css("display", "block");
        }
        else {
            $('[id*=divdate]').css("display", "none");
        }

        $('[id*=divdtable]').css("display", "none");
    });
});


function ShowControls() {
    $(document).ready(function () {
        var today = new Date();
        $(".datepicker_monthyear").bsdatepicker({
            format: "MMM-yyyy",
            viewMode: "months",
            minViewMode: "months",
            endDate: "today",
            maxDate: today,
        })
    });
}

function datetitle() {

    GetDropID();

    var dfrm = DateFrom;
    var dto = DateTo;
    var dmth = Month;
    var dtype = ddtype;

    if (Month != "") {
        $('[id*=lbldatefrom]').text("");
        $('[id*=lblmonth]').text(Month);
        $('[id*=lblmonth]').css({"font-weight": "bold"});
    }
    else {
        $('[id*=lblmonth]').text("");
        var dfrmto = dfrm + " -" + dto;
        $('[id*=lbldatefrom]').text(dfrmto);
        $('[id*=lbldatefrom]').css({"font-weight": "bold"});
    }
    if (ddsubtype == 0) {
        $('[id*=lblsubreport]').text("");
        $('[id*=lblreporttype]').text("");
        $('[id*=lblreporttype]').text(dtype);
        $('[id*=lblreporttype]').css({"font-weight": "bold" });
    }
    else {
        $('[id*=lblreporttype]').text("");
        $('[id*=lblsubreport]').text("");
        var contype = dtype + " ( " + ddsubtype + " )";
        $('[id*=lblreporttype]').text(contype);
        $('[id*=lblreporttype]').css({"font-weight": "bold"});
    }

}

var d1 = '';
var d2 = '';

function validatereport() {
    $('[id*=divdtable]').css("display", "none");
    ddtype = "";
    ddsubtype = "";
    Monthdate = "";
    fromdate = "";
    Todate = "";
    var date1 = "";
    var date2 = "";
    ddtype = $('option:selected', '[id*=drpreport]').val();
    ddsubtype = $('option:selected', '[id*=drpsubreport]').val();
    Monthdate = $('[id*=txtMonth]').val();
    fromdate = $('[id*=txtDateFrom]').val();
    Todate = $('[id*=txtDateTo]').val();
    GetDropID();
    datetitle();
  
    if (fromdate != "" && Todate != "") {
        d1 = new Date($('[id*=txtDateFrom]').val());
        d2 = new Date($('[id*=txtDateTo]').val());
        date1 = d1.getMonth() + 1;
        date2 = d2.getMonth() + 1;
    }
    var errormsg = "";
    var error = "";

    // validate conditions on click of search
    if ((ddtype == 0 || Monthdate== "") && (ddtype == 0 || fromdate== "" || Todate=="")){
        ShowwarningPopup("Please fill the required fields");
    }
    else if ((ddtype == "Cumulative Usage" || ddtype == "Usage") && (ddsubtype == 0)) {
        ShowwarningPopup("Please fill the required fields");
    }
    else if (corp == "" || FID == "" || VID == "" ) {
        if (corp == "") { errormsg = errormsg + "Corporate should not be empty,"; }
        if (FID == "") { errormsg = errormsg + "Facility should not be empty,"; }
        if (VID == "") { errormsg = errormsg + "Vendor should not be empty,"; }
        error = errormsg.substring(0, errormsg.length - 1)
        ShowwarningPopup(error);
    }
    else{
        var reporttype = "";
        if(ddtype !="" || ddsubtype !=""){
            if (ddsubtype == "Monthly Usage") {
                reporttype = 1;
            }
            if (ddtype == "Monthly Purchase") {
                reporttype = 2;
            }
            if (ddtype == "Ending Inventory") {
                reporttype = 3;
            }
            if (ddsubtype == "CumulativeUsage by itemcategory") {
                reporttype = 4;
            }
            if (ddsubtype == "CumulativeUsage by singleitem") {
                reporttype = 5;
            }
            if (ddtype == "Inventory") {
                reporttype = 6;
            }
            if (ddtype == "Cost per Tx") {
                reporttype = 7;
            }
            if (ddsubtype == "MonthlyDrugsSuppliesUsage") {
                reporttype = 8;
            }
        }
        switch(reporttype){
            case 1:
                if (ddsubtype == "Monthly Usage" && (date1) != (date2)) {
                    ShowwarningPopup("Date range should be same month ");
                } else {
                    $('[id*=collapseOne]').collapse('toggle');
                    $('[id*=divtitle]').css("display", "block");
                    $('[id*=divdtable]').css("display", "block");
                    $('[id*=divmonthusage]').css("display", "block");
                    $('[id*=divmonthlyInventory]').css("display", "none");
                    $('[id*=divCumulativebyItemDesc]').css("display", "none");
                    $('[id*=divCumulativebyCategory]').css("display", "none");
                    $('[id*=divmonthEndingInventory]').css("display", "none");
                    $('[id*=divmonthpurchase]').css("display", "none");
                    $('[id*=divcostpertx]').css("display", "none");
                    $('[id*=divMonthlydrugsupplies]').css("display", "none");
                    ShowMonthUsage();
                }
                break;
            case 2:
                if (ddtype == "Monthly Purchase" && (date1) != (date2)) {
                    ShowwarningPopup("Date range should be same month ");}
                else {
                    $('[id*=collapseOne]').collapse('toggle');
                    $('[id*=divtitle]').css("display", "block");
                    $('[id*=divdtable]').css("display", "block");
                    $('[id*=divmonthpurchase]').css("display", "block");
                    $('[id*=divmonthlyInventory]').css("display", "none");
                    $('[id*=divCumulativebyItemDesc]').css("display", "none");
                    $('[id*=divCumulativebyCategory]').css("display", "none");
                    $('[id*=divmonthEndingInventory]').css("display", "none");
                    $('[id*=divmonthusage]').css("display", "none");
                    $('[id*=divcostpertx]').css("display", "none");
                    $('[id*=divMonthlydrugsupplies]').css("display", "none");
                    ShowMonthlypurchase();
                }
                break;
            case 3:
                if (ddtype == "Ending Inventory" && (date1) != (date2)) {
                    ShowwarningPopup("Date range should be same month ");}
                else {
                    $('[id*=collapseOne]').collapse('toggle');
                    $('[id*=divtitle]').css("display", "block");
                    $('[id*=divdtable]').css("display", "block");
                    $('[id*=divmonthEndingInventory]').css("display", "block");
                    $('[id*=divmonthlyInventory]').css("display", "none");
                    $('[id*=divCumulativebyItemDesc]').css("display", "none");
                    $('[id*=divCumulativebyCategory]').css("display", "none");
                    $('[id*=divmonthpurchase]').css("display", "none");
                    $('[id*=divmonthusage]').css("display", "none");
                    $('[id*=divcostpertx]').css("display", "none");
                    $('[id*=divMonthlydrugsupplies]').css("display", "none");
                    ShowMonthlyEndingInventory();
                }
                break;
            case 4:
                if (ddsubtype == "CumulativeUsage by itemcategory" && CID.length > 1 || CID == "") {
                    if (CID == "") { errormsg = "Category should not be empty,"; }
                    if (CID.length > 1) { errormsg = errormsg + "Category  multi select is not applicable for this report,"; }
                    error = errormsg.substring(0, errormsg.length - 1)
                    ShowwarningPopup(error);
                    CID = [];
                }
                else {
                    $('[id*=collapseOne]').collapse('toggle');
                    $('[id*=divtitle]').css("display", "block");
                    $('[id*=divdtable]').css("display", "block");
                    $('[id*=divCumulativebyCategory]').css("display", "block");
                    $('[id*=divmonthlyInventory]').css("display", "none");
                    $('[id*=divCumulativebyItemDesc]').css("display", "none");
                    $('[id*=divmonthEndingInventory]').css("display", "none");
                    $('[id*=divmonthpurchase]').css("display", "none");
                    $('[id*=divmonthusage]').css("display", "none");
                    $('[id*=divcostpertx]').css("display", "none");
                    $('[id*=divMonthlydrugsupplies]').css("display", "none");
                    ShowCumulativebyItemCategory();
                }
                break;
            case 5:
                if (ddsubtype == "CumulativeUsage by singleitem" && IID.length > 1 || IID == "") {
                    if (IID == "") { errormsg ="Description should not be empty,"; }
                    if (IID.length > 1) { errormsg = errormsg + "Description  multi select is not applicable for this report,"; }
                    error = errormsg.substring(0, errormsg.length - 1)
                    ShowwarningPopup(error);
                    IID = [];
                }
                else {
                    $('[id*=collapseOne]').collapse('toggle');
                    $('[id*=divtitle]').css("display", "block");
                    $('[id*=divdtable]').css("display", "block");
                    $('[id*=divCumulativebyItemDesc]').css("display", "block");
                    $('[id*=divmonthlyInventory]').css("display", "none");
                    $('[id*=divCumulativebyCategory]').css("display", "none");
                    $('[id*=divmonthEndingInventory]').css("display", "none");
                    $('[id*=divmonthpurchase]').css("display", "none");
                    $('[id*=divmonthusage]').css("display", "none");
                    $('[id*=divcostpertx]').css("display", "none");
                    $('[id*=divMonthlydrugsupplies]').css("display", "none");
                    ShowCumulativebyItemDesc();
                }
                break;
            case 6:
                if ((ddtype == "Inventory" && FID.length > 1) || (ddtype == "Inventory" && fromdate != "" && Todate != "")) {
                    if (FID.length > 1) { errormsg = "Facility  multi select is not applicable for this report,"; }
                    if (fromdate != "" && Todate != "") { errormsg = errormsg + "Date range From/To is not applicable for this report,"; }
                    FID = [];
                    fromdate = "";
                    Todate = "";
                    error = errormsg.substring(0, errormsg.length - 1)
                    ShowwarningPopup(error);
                }
                else {
                    $('[id*=collapseOne]').collapse('toggle');
                    $('[id*=divtitle]').css("display", "block");
                    $('[id*=divdtable]').css("display", "block");
                    $('[id*=divmonthlyInventory]').css("display", "block");
                    $('[id*=divCumulativebyItemDesc]').css("display", "none");
                    $('[id*=divCumulativebyCategory]').css("display", "none");
                    $('[id*=divmonthEndingInventory]').css("display", "none");
                    $('[id*=divmonthpurchase]').css("display", "none");
                    $('[id*=divmonthusage]').css("display", "none");
                    $('[id*=divcostpertx]').css("display", "none");
                    $('[id*=divMonthlydrugsupplies]').css("display", "none");
                    ShowMonthlyInventory();
                }
                break;
            case 7:
                if ((ddtype == "Cost per Tx" && FID.length > 1) || (ddtype == "Cost per Tx" && fromdate != "" && Todate != "")) {
                    if (FID.length > 1) { errormsg = "Facility  multi select is not applicable for this report,"; }
                    if (fromdate != "" && Todate != "") { errormsg = errormsg + "Date range From/To is not applicable for this report,"; }
                    FID = [];
                    fromdate = "";
                    Todate = "";
                    error = errormsg.substring(0, errormsg.length - 1)
                    ShowwarningPopup(error);
                }
              else {
                    $('[id*=collapseOne]').collapse('toggle');
                    $('[id*=divtitle]').css("display", "block");
                    $('[id*=divdtable]').css("display", "block");
                    $('[id*=divcostpertx]').css("display", "block");
                    $('[id*=divmonthEndingInventory]').css("display", "none");
                    $('[id*=divmonthlyInventory]').css("display", "none");
                    $('[id*=divCumulativebyItemDesc]').css("display", "none");
                    $('[id*=divCumulativebyCategory]').css("display", "none");
                    $('[id*=divmonthpurchase]').css("display", "none");
                    $('[id*=divmonthusage]').css("display", "none");
                    $('[id*=divMonthlydrugsupplies]').css("display", "none");
                    ShowCostPerTx();
                }
                break;
            case 8:
                if (ddsubtype == "MonthlyDrugsSuppliesUsage" && (date1) != (date2)) {
                    ShowwarningPopup("Date range should be same month ");
                } else {
                    $('[id*=collapseOne]').collapse('toggle');
                    $('[id*=divtitle]').css("display", "block");
                    $('[id*=divdtable]').css("display", "block");
                    $('[id*=divMonthlydrugsupplies]').css("display", "block");
                    $('[id*=divmonthusage]').css("display", "none");
                    $('[id*=divmonthlyInventory]').css("display", "none");
                    $('[id*=divCumulativebyItemDesc]').css("display", "none");
                    $('[id*=divCumulativebyCategory]').css("display", "none");
                    $('[id*=divmonthEndingInventory]').css("display", "none");
                    $('[id*=divmonthpurchase]').css("display", "none");
                    $('[id*=divcostpertx]').css("display", "none");
                    ShowMonthdrugsUsage();
                }
                break;

            default:
                $('[id*=divtitle]').css("display", "none");
                $('[id*=divdtable]').css("display", "none");
                $('[id*=divmonthlyInventory]').css("display", "none");
                $('[id*=divCumulativebyItemDesc]').css("display", "none");
                $('[id*=divCumulativebyCategory]').css("display", "none");
                $('[id*=divmonthEndingInventory]').css("display", "none");
                $('[id*=divmonthpurchase]').css("display", "none");
                $('[id*=divmonthusage]').css("display", "none");
                $('[id*=divcostpertx]').css("display", "none");
                $('[id*=divMonthlydrugsupplies]').css("display", "none");
              break;
        }
    }
}
     

//Get Report Type and Report type based enable controls

function DropdownChange() {
    $('[id*=drpreport]').change(function () {
        DropdownReport();
    });
}

function DropdownReport() {

    var dropdowntext = $('option:selected', '[id*=drpreport]').val();
    $.ajax({
        type: "POST",
        url: "Reportviewer.aspx/BindSubReport",
        data: '{drpreporttext: "' + dropdowntext + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: Onsucc,
        failure: function (r) {
            alert(response.d);
        }
    });

    var ddordertype = $("[id*=drpordertype]");
    var dditem = $("[id*=drpitem]");
    var ddlsubreport = $("[id*=drpsubreport]");
    var drpvendorsearch = $("[id*=drpsubreport]");
    var drpcategory = $("[id*=drpsubreport]");

    if (dropdowntext == "Monthly Purchase") {
        ddordertype.attr('disabled', true);
        dditem.attr('disabled', true);
        drpvendorsearch.removeAttr('disabled');
        drpcategory.removeAttr('disabled');
    }
    else if (dropdowntext == "Ending Inventory") {
        ddordertype.attr('disabled', true);
        dditem.attr('disabled', true);
        drpvendorsearch.removeAttr('disabled');
        drpcategory.removeAttr('disabled');
    }
    else if (dropdowntext == "Inventory") {
        dditem.attr('disabled', true);
        ddordertype.attr('disabled', true);
        drpvendorsearch.removeAttr('disabled');
        drpcategory.removeAttr('disabled');
    }
    else if (dropdowntext == "Cost per Tx")
    {
        ddordertype.attr('disabled', true);
        drpvendorsearch.removeAttr('disabled');
        drpcategory.removeAttr('disabled');
        dditem.removeAttr('disabled');
    }

}

function Onsucc(response) {
    var ddlreport = $("[id*=drpsubreport]");
    ddlreport.empty().append('<option selected="selected" value="0">Please select</option>');

    $.each(response.d, function () {
        ddlreport.append($("<option></option>").val(this['InvenValue']).html(this['InvenValue']));
    });
   
    if ($('[id*=drpsubreport] option').length == 1) {
        ddlreport.attr('disabled', true);
    } else
        ddlreport.attr('disabled', false);
}



function DropdownsubChange() {
    $('[id*=drpsubreport]').change(function () {
        var drpsubreport = $('option:selected', '[id*=drpsubreport]').val();
        var ddordertype = $("[id*=drpordertype]");
        var dditem = $("[id*=drpitem]");
        var drpvendorsearch = $("[id*=drpvendorsearch]");
        var drpcategory = $("[id*=drpcategory]");

        if (drpsubreport == "Monthly Usage") {
            ddordertype.attr('disabled', true);
            dditem.attr('disabled', true);
            drpvendorsearch.removeAttr('disabled');
            drpcategory.removeAttr('disabled');
        }
        else if (drpsubreport == "MonthlyDrugsSuppliesUsage") {
            ddordertype.attr('disabled', true);
            dditem.attr('disabled', true);
            drpvendorsearch.removeAttr('disabled');
            drpcategory.removeAttr('disabled');
        }
        else if (drpsubreport == "CumulativeUsage by itemcategory") {
            ddordertype.attr('disabled', true);
            drpvendorsearch.attr('disabled', true);
            dditem.attr('disabled', 'true');
            drpcategory.removeAttr('disabled');
        }
        else if (drpsubreport == "CumulativeUsage by singleitem") {
            ddordertype.attr('disabled', true);
            drpvendorsearch.attr('disabled', true);
            drpcategory.attr('disabled', true);
            dditem.removeAttr('disabled');
        }
    });
}


// Clear all the fields
$(document).ready(function () {
    $("#btnClose").click(function () {
        location.reload();
        //var ddordertype = $("[id*=drpordertype]");
        //var dditem = $("[id*=drpitem]");
        //var drpcategory = $("[id*=drpcategory]");
        //var drpvendorsearch = $("[id*=drpvendorsearch]");
       
        //ddordertype.removeAttr('disabled');
        //dditem.removeAttr('disabled');
        //drpvendorsearch.removeAttr('disabled');
        //drpcategory.removeAttr('disabled');
     
        //$('[id*=drpreport]').val("");
        //$('[id*=drpsubreport]').val("");
        //$('[id*=drpdaterange]').val("");
        //$('[id*=txtDateFrom]').val("");
        //$('[id*=txtDateTo]').val("");
        //$('[id*=txtMonth]').val("");
        //$('[id*=divdate]').css("display", "none");
        //$('[id*=divmonth]').css("display", "none");

        $('[id*=divdtable]').css("display", "none");
    });
});



