var table;
var table_CumulativebyItemDesc;
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
JDataTableCumulativebyItemDesc();

function ShowCumulativebyItemDesc() {
    GetDropID();
    $.ajax({
        type: "POST",
        url: "Reportviewer.aspx/BindCumulativebyItemDesc",
        data: '{CorpID: "' + CorpID + '",FacilityID: "' + FacilityID + '" ,VendorID: "' + VendorID + '",CategoryID: "' + CategoryID + '", Month:"' + Month + '", DateFrom: "' + DateFrom + '", DateTo: "' + DateTo + '",  ItemID: "' + ItemID + '", ortype: "' + ortype + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnCumulativebyItemDesc,
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
    ItemID = "";
    DateFrom = "",
    DateTo = "",
    Month = ""
}


var dynamic_data5 = [];
var dynamic_columns5 = [];
var column_length5 = 0;
dynamic_columns5 = [
           { 'title': 'Facility Description', data: 'FacilityDescription' },
            { 'title': 'Facility Code', data: 'FacilityCode' },
            { 'title': 'Item Description', data: 'ItemDescription' }
];

function OnCumulativebyItemDesc(response) {
    var columns = [];
    columns = response.d;
    if (columns.List01.length > 0) {
        Loggedinby = columns.List01[1].PrintedBy;
    } else { Loggedinby = ""; }
    if (ddsubtype == "CumulativeUsage by singleitem") {
        dynamic_data5 = columns.List01;
        dynamic_columns5 = columns.List02;
        column_length5 = columns.List02.length;
        table_CumulativebyItemDesc.destroy();
        $('#tblCumulativebyItemDesc').empty();
        JDataTableCumulativebyItemDesc();
    }
}

function JDataTableCumulativebyItemDesc() {
    //$('#tblCumulativebyItemDesc').append('<caption style="caption-side: top"><strong>Cumutlatve Usage by Single Item</strong></caption>');
    table_CumulativebyItemDesc = $('#tblCumulativebyItemDesc').DataTable({
        scrollY: "500px",
        scrollX: true,
        "searching": false,
        "paging": false,
        "ordering": false,
        scrollCollapse: true,
        fixedColumns: {
            leftColumns: 5,
            rightColumns: 0
        },
        "columnDefs": [
     {
         "targets": [0],
         "visible": false,
     },
	 {
	     "targets": [1],
	     "visible": false,
	 },
     {
         "targets": [2],
         "visible": false,
     },
        ],
        order: [[1, 'asc']],
        "rowCallback": function (row, data, index) {
            //alert(data);
            if (data["RowType"] == '1') {
                $('td', row).css('background-color', '#BDD7EE');
            }
            if (data["RowType"] == '3') {
                $('td', row).css('background-color', '#D6DCE4');
            }
        },
        data: dynamic_data5,
        "columns": dynamic_columns5,
        dom: 'Bfrtip',
        buttons: [
              {
                  extend: 'excel',
                  title: 'Cumulative Usage by Single Item',
                  messageTop: mondate,
                  exportOptions: {
                      columns: ':visible'
                  },
                  customize: function (xlsx) {
                      var sheet = xlsx.xl.worksheets['sheet1.xml'];
                      // Loop over the cells in column `C`
                      $('row c[r^="A"]', sheet).each(function () {
                          if ($('is t', this).text().indexOf("[") != -1) {
                              $(this).attr('s', '2');
                              $(this).siblings().attr('s', '2');
                          }
                          if ($('is t', this).text().indexOf("Grant Total") != -1) {
                              $(this).attr('s', '2');
                          }

                      });
                  }
              },

                 {
                     extend: 'pdfHtml5',
                     //pageSize: 'LEGAL',
                     //orientation: 'landscape',
                     filename: 'Cumulative Usage by Single Item',
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
                   
                       
                         var count = 0;
                         var currentdate = new Date();
                         var datetime = moment(currentdate).format('MM/DD/YYYY HH:mm:ss');

                         $('#tblCumulativebyItemDesc').find('tbody tr:first-child td').each(function () {
                             count++;
                         });

                         if (count < 12) {
                             doc.pageOrientation = 'portrait';
                             doc.pageSize = 'LEGAL';
                         }
                         else if (count >= 12 && count < 24) {
                             doc.pageOrientation = 'landscape';
                             doc.pageSize = 'LEGAL';
                         }
                         else {
                             doc.pageOrientation = 'landscape';
                             doc.pageSize = 'A0';
                         }

                         doc.defaultStyle.fontSize = 8;
                         doc.styles.tableHeader.fontSize = 8;

                         $('#tblCumulativebyItemDesc').find('tr').each(function (ix, row) {
                             var index = ix;
                             var rowElt = row;
                          
                             var subtitle = $(this).find('td').eq(0).find('strong').find('font').html();

                             $(row).find('td').each(function (ind, elt) {
                                 if (subtitle != undefined) {
                                     if (subtitle == 'Grant Total') {
                                         tblBody[index][ind].fillColor = '#FFFDE7';
                                     } else {
                                         tblBody[index][ind].fillColor = '#c2c3c9';
                                     }
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
                                         { text: 'Cumulative Usage by Single Item' + '\n' + mondate }
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
                                        { text: 'Printed By:' + Loggedinby + '          ' },
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

