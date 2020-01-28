var table;
var table_monthdrugsusage;
var ddsubtype;
var ddtype;
var ortype;
var ObjID = [];
CorpID = '';
FacilityID = '';
VendorID = '';
CategoryID = '';
ItemID = '';
Month = '';
DateFrom = '';
DateTo = '';
ddtype = '';
var mondate = '';
JDataTableMonthdrugsusage();

function ShowMonthdrugsUsage() {
    GetDropID();
    $.ajax({
        type: "POST",
        url: "Reportviewer.aspx/BindMonthlyDrugsUsage",
        data: '{CorpID: "' + CorpID + '",FacilityID: "' + FacilityID + '" ,VendorID: "' + VendorID + '" ,CategoryID: "' + CategoryID + '", Month:"' + Month + '", DateFrom: "' + DateFrom + '", DateTo: "' + DateTo + '", ItemID: "' + ItemID + '", ortype: "' + ortype + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OndrugSuccesss,
        failure: function (response) {
            alert(response.d);
        },
        error: function (jqXHR, status, err) {
            alert(jqXHR.responseText);
        },
    });


    if (Month != "") {
        mondate = 'Period:' + Month;
    }
    else {
        var dfrmto = DateFrom + " -" + DateTo;
        mondate = 'Period:' + dfrmto;
    }

    CorpID = "";
    FacilityID = "";
    VendorID = "";
    CategoryID = "";
    DateFrom = "",
    DateTo = "",
    Month = ""
}


var dynamic_data8 = [];
var dynamic_columns8 = [];
var column_length8 = 0;
dynamic_columns8 = [
            { 'title': 'Item Description', data: 'ItemDescription' },
            { 'title': 'Qty/Pack', data: 'QtyPack' },
            { 'title': 'Uom', data: 'UomName' },
            { 'title': 'Each Price', data: 'EachPrice' },
            { 'title': 'Category Name', data: 'CategoryName' }
];

function OndrugSuccesss(response) {
    var columns = [];
    columns = response.d;
    if (columns.List01.length > 0) {        
        Loggedinby = columns.List01[0].PrintedBy;
    } else { Loggedinby = ""; }
    if (ddsubtype == "MonthlyDrugsSuppliesUsage") {
        dynamic_data8 = columns.List01;
        dynamic_columns8 = columns.List02;
        column_length8 = columns.List02.length;
        table_monthdrugsusage.destroy();
        $('#tblmonthlydrugs').empty();
        JDataTableMonthdrugsusage();
    }
}

function JDataTableMonthdrugsusage() {
    //$('#example').append('<caption style="caption-side: top"><strong>Monthly Usage</strong></caption>');
    table_monthdrugsusage = $('#tblmonthlydrugs').DataTable({
        scrollY: "500px",
        scrollX: true,
        "searching": false,
        "paging": false,
        "ordering": false,
        scrollCollapse: true,
        fixedColumns: {
            leftColumns: 1,
            rightColumns: 0
        },
        "columnDefs": [
     {
         "targets": [0,1],
         "visible": false,
     },
	 //{
	 //    "targets": [1],
	 //    "visible": false,
	 //},
        ],
        //order: [[1, 'asc']],
        "rowCallback": function (row, data, index) {
            //alert(data);
            if (data["RowType"] == '2') {
                $('td', row).css('background-color', '#BDD7EE');
            }           
        },
        data: dynamic_data8,
        "columns": dynamic_columns8,
        dom: 'Bfrtip',
        buttons: [
             {
                 extend: 'excel',
                 title: 'Monthly Drugs Supplies Usage',
                 messageTop: mondate,
                 exportOptions: {
                     columns: ':visible'
                 },
                 customize: function (xlsx) {
                     $(xlsx.xl["styles.xml"]).find('numFmt[numFmtId="164"]').attr('formatCode', '[$$-45C] #,##0.00_-');
                     var sheet = xlsx.xl.worksheets['sheet1.xml'];
                 //customize: function (xlsx) {
                 //    $(xlsx.xl["styles.xml"]).find('numFmt[numFmtId="164"]').attr('formatCode', '[$$-45C] #,##0.00_-');
                 //    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                 //    // Loop over the cells in column `C`
                 //    $('row c[r^="A"]', sheet).each(function () {
                 //        //alert($(this).text());
                 //        // Get the value
                 //        //alert($('is t', this).text());
                 //        if ($('is t', this).text().indexOf("[") != -1) {
                 //            $(this).attr('s', '2');
                 //            $(this).siblings().attr('s', '2');
                 //        }
                 //        if ($('is t', this).text().indexOf("Group Total Cost") != -1) {
                 //            $(this).attr('s', '2');
                 //        }

                 //    });
                    
                 }
             },

               {
                   extend: 'pdfHtml5',
                   pageSize: 'LEGAL',
                   orientation: 'landscape',
                   filename: 'Monthly Drugs Supplies',
                   footer: true,
                   exportOptions: {
                       columns: ':visible',
                   },
                   customize: function (doc) {
                       var tblBody = doc.content[1].table.body;
                       doc.content[1].layout = {
                           hLineWidth: function (i, node) {
                               return (i === 0 || i === node.table.body.length) ? 1 : 1;
                           },
                           vLineWidth: function (i, node) {
                               return (i === 0 || i === node.table.widths.length) ? 1 : 1;
                           },
                           hLineColor: function (i, node) {
                               return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                           },
                           vLineColor: function (i, node) {
                               return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                           }
                       };

                       //var count = 0;
                       var currentdate = new Date();
                       var datetime = moment(currentdate).format('MM/DD/YYYY HH:mm:ss');

                       //$('#tblmonthlydrugs').find('tbody tr:first-child td').each(function () {
                       //    count++;
                       //});
                       //if (count > 12) {
                       //    doc.pageOrientation = 'landscape';
                       //} else {
                       //    doc.pageOrientation = 'portrait';
                       //}

                       doc.defaultStyle.fontSize = 8;
                       doc.styles.tableHeader.fontSize = 8;

                       $('#tblmonthlydrugs').find('tr').each(function (ix, row) {
                           var index = ix;
                           var rowElt = row;

                           var grandtotal = $(this).find('td').eq(0).find('strong').find('font').html();
                           
                           console.log(grandtotal);


                           $(row).find('td').each(function (ind, elt) {

                               if (grandtotal != undefined) {
                                   if (grandtotal == 'Grand Total') {
                                       tblBody[index][ind].fillColor = '#FFFDE7';
                                   }
                               }
                               if (grandtotal == 'Grand Total') {
                                   tblBody[index][ind].fillColor = '#FFFDE7';
                               }
                               if (grandtotal == '') {
                                   tblBody[index][ind].fillColor = '#FFFDE7';
                               }
                           });

                       });

                       doc['styles'] = {
                           tableHeader: {
                               bold: !0,
                               color: 'black',
                               fillColor: '#add8e6'
                           }
                       };

                       doc['header'] = (function () {
                           return {
                               columns: [
                                   {
                                       alignment: 'center',
                                       text: [
                                       { text: 'Monthly Drugs Supplies' + '\n' + mondate }
                                       ],
                                       fontSize: 11,
                                   },
                               ],
                               margin: [0, 10]
                           }
                       });

                       doc['footer'] = (function (page, pages) {
                           return {
                               columns: [
                                {
                                    alignment: 'left',
                                    text: [
                                    { text: 'Page No:' + page.toString() + '        ' },
                                    ]
                                },

                                 {
                                     text: [
                                      { text: 'Printed By:' + Loggedinby + '         ' },
                                     ]
                                 },

                                 {
                                     text: [
                                      { text: 'Printed On:' + datetime }
                                     ]
                                 },

                               {
                                   alignment: 'right',
                                   text: [
                                   { text: 'Page:' + page.toString() },
                                   ' of ',
                                   { text: pages.toString() }
                                   ]
                               },

                               ],

                               fontSize: 7,
                               margin: [10, 0]
                           }
                       });

                   }
               },
        ]
    });
}






