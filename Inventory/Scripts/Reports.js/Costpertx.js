var table;
var table_costpertx;
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
var totalNoVist = '';
var mondate = '';
JDataTableCostpertx();

function ShowCostPerTx() {
    GetDropID();
    $.ajax({
        type: "POST",
        url: "Reportviewer.aspx/BindCostpertx",
        data: '{CorpID: "' + CorpID + '",FacilityID: "' + FacilityID + '",VendorID: "' + VendorID + '",CategoryID: "' + CategoryID + '",Month:"' + Month + '",DateFrom: "' + DateFrom + '",DateTo: "' + DateTo + '", ItemID: "' + ItemID + '", ortype: "' + ortype + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OncostSuccess,
        failure: function (response) {
            alert(response.d);
        },
        error: function (jqXHR, status, err) {
            alert(jqXHR.responseText);
        },
    });

    if (Month != "") {
        mondate = 'Cost per Treatment Period:' + Month;
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


var dynamic_data7 = [];
var dynamic_columns7 = [];
var column_length7 = 0;
dynamic_columns7 = [
            { 'title': 'Item Description', data: 'ItemDescription' },
            { 'title': 'Qty/Pack', data: 'QtyPack' },
            { 'title': 'Uom', data: 'UomName' },
];

function OncostSuccess(response) {
    var columns = [];
    columns = response.d;
    if (columns.List01.length > 0) {
        totalNoVist = columns.List01[1].Noofvisit;
        Loggedinby = columns.List01[1].PrintedBy;
    }
    else {
        totalNoVist = "";
        Loggedinby = "";
    }
    if (ddtype == "Cost per Tx") {
        dynamic_data7 = columns.List01;
        dynamic_columns7 = columns.List02;
        column_length7 = columns.List02.length;
        table_costpertx.destroy();
        $('#tblcostpertx').empty();
        JDataTableCostpertx();
    }
}




function JDataTableCostpertx() {
    var Facilityname = $('option:selected', '[id*=drpfacilitysearch]').html();
    table_costpertx = $('#tblcostpertx').DataTable({
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
     {
        "targets": [3],
        "visible": false,
     },
     {
        "targets": [4],
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
        data: dynamic_data7,
        "columns": dynamic_columns7,
        dom: '<"toolbar">Bfrtip',
        buttons: [
            {
                extend: 'excel',
                title: 'CostperTx',
                messageTop: 'Facility Name:' + Facilityname + '     ' + mondate,
                messageBottom: 'Total No. of Treatments:' + totalNoVist,
                exportOptions: {
                    columns: ':visible'
                },
                customize: function (xlsx) {
                    $(xlsx.xl["styles.xml"]).find('numFmt[numFmtId="164"]').attr('formatCode', '[$$-45C] #,##0.00_-');
                    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                    // Loop over the cells in column `C`
                    $('row c[r^="A"]', sheet).each(function () {
                        if ($('is t', this).text().indexOf("[") != -1) {
                            $(this).attr('s', '2');
                            $(this).siblings().attr('s', '2');
                        }
                        if ($('is t', this).text().indexOf("Total No. of Treatments:") != -1) {
                            $(this).attr('s', '50');
                        }
                    });
                    $('row c[r^="M"]', sheet).each(function () {
                        if ($('is t', this).text().indexOf("Total Price") != -1) {
                            $(this).attr('s', '2');
                            //  $(this).siblings().attr('s', '2');
                        }
                        if ($('is t', this).text().indexOf("Grand Total") != -1) {
                            $(this).attr('s', '2');
                            //$(this).siblings().attr('s', '2');
                        }

                    });

                }
            },

                {
                    extend: 'pdfHtml5',
                    pageSize: 'LEGAL',
                    orientation: 'portrait',
                    filename: 'CostperTx',
                    title: 'Facility Name:' + Facilityname + '\n' + 'Total No. of Treatments:' + totalNoVist + '  ' + mondate,
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

                        //$('#tblcostpertx').find('tbody tr:first-child td').each(function () {
                        //    count++;
                        //});
                        //if (count < 12) {
                        //    doc.pageOrientation = 'portrait';
                        //    doc.pageSize = 'LEGAL';
                        //}
                        //else if (count > 12 && count < 24) {
                        //    doc.pageOrientation = 'landscape';
                        //    doc.pageSize = 'LEGAL';
                        //}
                        //else {
                        //    doc.pageOrientation = 'landscape';
                        //    doc.pageSize = 'A0';
                        //}

                        doc.defaultStyle.fontSize = 7;
                        doc.styles.tableHeader.fontSize = 7;

                        $('#tblcostpertx').find('tr').each(function (ix, row) {
                            var index = ix;
                            var rowElt = row;
                        
                            var grandtotal = $(this).find('td').eq(8).find('strong').html();
                            var subtitle = $(this).find('td').eq(0).find('strong').find('font').html();

                            $(row).find('td').each(function (ind, elt) {

                                if (grandtotal != undefined) {
                                    if (grandtotal == 'Total Price = ') {
                                        tblBody[index][ind].fillColor = '#FFFDE7';
                                    }
                                    if (grandtotal == 'Grand Total : ') {
                                        tblBody[index][ind].fillColor = '#f1f3f4';
                                    }
                                }
                                if (subtitle != undefined) {
                                    tblBody[index][ind].fillColor = '#c2c3c9';
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
                        doc.styles.title = {
                            fontSize: '9',
                            alignment: 'center'
                        }
                        doc['header'] = (function () {
                            return {
                                columns: [
                                    {
                                        alignment: 'center',
                                        text: [
                                        { text: 'Cost Per Tx' }
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
    $("div.toolbar").html('<center><font size=4><strong>Facility :' + Facilityname + '</strong></font><br/><strong>Total No. of Treatments:' + totalNoVist + '</strong></center>');
}

