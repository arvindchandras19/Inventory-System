var table;
var table_monthpurchase;
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
JDataTableMonthPurchase();

function ShowMonthlypurchase() {
    GetDropID();
    $.ajax({
        type: "POST",
        url: "Reportviewer.aspx/BindMonthlyPurchase",
        data: '{CorpID: "' + CorpID + '",FacilityID: "' + FacilityID + '" ,VendorID: "' + VendorID + '" ,CategoryID: "' + CategoryID + '", Month:"' + Month + '", DateFrom: "' + DateFrom + '", DateTo: "' + DateTo + '", ItemID: "' + ItemID + '", ortype: "' + ortype + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
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


var dynamic_data2 = [];
var dynamic_columns2 = [];
var column_length2 = 0;
dynamic_columns2 = [
            { 'title': 'Item Description', data: 'ItemDescription' },
            { 'title': 'Qty/Pack', data: 'QtyPack' },
            { 'title': 'Uom', data: 'UomName' },
            { 'title': 'Each Price', data: 'EachPrice' },
            { 'title': 'Category Name', data: 'CategoryName' }
];

function OnSuccess(response) {
    var columns = [];
    columns = response.d;
    if (columns.List01.length > 0) {
        Loggedinby = columns.List01[1].PrintedBy;
    } else { Loggedinby = ""; }
    if (ddtype == "Monthly Purchase") {
        dynamic_data2 = columns.List01;
        dynamic_columns2 = columns.List02;
        column_length2 = columns.List02.length;
        table_monthpurchase.destroy();
        $('#tblmonthlypurchase').empty();
        JDataTableMonthPurchase();
    }
}



function JDataTableMonthPurchase() {
    //$('#tblmonthlypurchase').append('<caption style="caption-side: top"><strong>Monthly Purchase</strong></caption>');
    table_monthpurchase = $('#tblmonthlypurchase').DataTable({
        scrollY: "500px",
        scrollX: true,
        "searching": false,
        "paging": false,
        "ordering": false,
        scrollCollapse: true,
        fixedColumns: {
            leftColumns: 6,
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
        data: dynamic_data2,
        "columns": dynamic_columns2,
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'excel',
                title: 'Monthly Purchase',
                messageTop: mondate,
                exportOptions: {
                    columns: ':visible'
                },
                customize: function (xlsx) {
                    $(xlsx.xl["styles.xml"]).find('numFmt[numFmtId="164"]').attr('formatCode', '[$$-45C] #,##0.00_-');
                    var sheet = xlsx.xl.worksheets['sheet1.xml'];

                    // jQuery selector to add a border
                    //$('row:first c', sheet).attr('s', '32');
                    // Loop over the cells in column `C`
                    $('row c[r^="A"]', sheet).each(function () {
                        //alert($(this).text());
                        // Get the value
                        //alert($('is t', this).text());
                        if ($('is t', this).text().indexOf("[") != -1) {
                            $(this).attr('s', '2');
                            $(this).siblings().attr('s', '2');
                        }
                        if ($('is t', this).text().indexOf("Group Total") != -1) {
                            $(this).attr('s', '2');
                        }

                    });
                    //$('row c[r^="M"]', sheet).each(function () {
                    //    //alert($(this).text());
                    //    // Get the value
                    //    //alert($('is t', this).text());
                    //    if ($('is t', this).text().indexOf("Total Price") != -1) {
                    //        $(this).attr('s', '2');
                    //      //  $(this).siblings().attr('s', '2');
                    //    }
                    //    if ($('is t', this).text().indexOf("Grand Total") != -1) {
                    //        $(this).attr('s', '2');
                    //        //$(this).siblings().attr('s', '2');
                    //    }

                    //});

                }
            },
             
              {
                  extend: 'pdfHtml5',
                  //pageSize: 'LEGAL',
                  //orientation: 'landscape',
                  filename: 'Monthly Purchase',
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

                      $('#tblmonthlypurchase').find('tbody tr:first-child td').each(function () {
                          count++;
                      });
                      if (count < 10) {
                          doc.pageOrientation = 'portrait';
                          doc.pageSize = 'LEGAL';
                      }
                      else if (count >= 10 && count < 18) {
                          doc.pageOrientation = 'landscape';
                          doc.pageSize = 'LEGAL';
                      }
                      else {
                          doc.pageOrientation = 'landscape';
                          doc.pageSize = 'A0';
                      }
                      doc.defaultStyle.fontSize = 8;
                      doc.styles.tableHeader.fontSize = 8;
                    
                      $('#tblmonthlypurchase').find('tr').each(function (ix, row) {
                          var index = ix;
                          var rowElt = row;

                          var grandtotal = $(this).find('td').eq(0).find('strong').html();
                          var subtitle = $(this).find('td').eq(0).find('strong').find('font').html();

                          $(row).find('td').each(function (ind, elt) {

                              if (grandtotal != undefined) {
                                  if (grandtotal == 'Group Total') {
                                      tblBody[index][ind].fillColor = '#FFFDE7';
                                  }
                              }
                              if (subtitle != undefined) {
                                  tblBody[index][ind].fillColor = '#c2c3c9';
                              }
                          });
                      });
                          
                         
                          //if (count > 1) {
                          //    $(row).find('td').each(function (ind, elt) {
                          //        console.log(ind)
                          //        tblBody[index][ind].border
                          //        if (tblBody[index][1].text == '') {
                          //            delete tblBody[index][ind].style;
                          //            tblBody[index][ind].fillColor = '#FFFDE7';
                          //        }
                          //        if (tblBody[index][7].text == '') {
                          //            delete tblBody[index][ind].style;
                          //            tblBody[index][ind].fillColor = '#c2c3c9';
                          //        }
                          //    });
                          //}
                     

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
                                      { text: 'Monthly Purchase' + '\n' + mondate }
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

