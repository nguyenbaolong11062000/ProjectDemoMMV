var colorRegion = '';
sessionStorage.setItem('flatCorlor', 1);
function dataArrayCPTH(jsonData) {
    var object = JSON.parse(jsonData);
    var gglData = [];
    if (object.length > 0) {
        // load column headings
        var colHead = [];
        Object.keys(object[0]).forEach(function (key) {
            colHead.push(key);
        });
        gglData.push(colHead);
        //console.log(gglData);
        // load data rows
        object.forEach(function (row) {
            var gglRow = [];
            Object.keys(row).forEach(function (key) {
                gglRow.push(row[key]);
            });
            gglData.push(gglRow);
        });
    }
    //console.log(gglData);
    return gglData;
}

function classChangeCPTH(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var color = '';
                for (var i = 1; i < gglData.length; i++) {
                    colorRegion = gglData[i][0];
                    switch (colorRegion) {
                        case 'NORTHEAST':
                            color += '#006266' + ',';
                            break;
                        case 'BANGKOK':
                            color += '#DC3912' + ',';
                            break;
                        case 'SOUTH':
                            color += '#FF9900' + ',';
                            break;
                        case 'CENTRAL':
                            color += '#9966ff' + ',';
                            break;
                        case 'NORTH':
                            color += '#990099' + ',';
                            break;
                        default:
                            color += '#4075ae' + ',';
                    }
                }
                var listColor = color.split(',');
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    animation: {
                        duration: 500,
                        startup: true
                    },
                    chartArea: {
                        width: '80%',
                        height: '80%',
                        bottom: 10,
                        left: 10,
                    },
                    colors: listColor,
                };

                GetdataChartCPTH(result.d, options, "Region", "Pie");
                if (Array.isArray(gglData) && gglData.length) {

                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'SUB_CATG', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
                    width: gglData.length * 65,
                    height: '100%',
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        style: 'line',
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        left: 20
                    },
                    colors: ['#4075ae']
                };
                GetdataChartCPTH(result.d, options, "SubCatg", "Column");
                if (Array.isArray(gglData) && gglData.length) {

                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'YEAR', colSum: 'GROSS_SALES_MLI', orderBy: 'YEAR', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        style: 'line',
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 20,
                            color: '#808080'
                        },

                    },
                    colors: ['#f154d4']
                };
                GetdataChartCPTH(result.d, options, "Year", "Column");
                if (Array.isArray(gglData) && gglData.length) {

                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'YEAR', colSum: 'GROSS_SALES_MLI', orderBy: 'YEAR', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    title: 'Sales Perfomance',
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        style: 'line',
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 20,
                            color: '#808080'
                        },

                    },
                    colors: ['#f154d4'],
                    seriesType: 'bars',
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { type: 'line', targetAxisIndex: 1, color: 'red' }
                    },
                    vAxes: {
                        // Adds titles to each axis.
                        0: { title: 'Gross sales' },
                        1: { title: 'Percentage' }
                    },
                    hAxis: {
                        title: 'Year',
                        fontSize: 30
                    }

                };
                // console.log(result.d);
                //var data = new google.visualization.arrayToDataTable(gglData);
                //chart = new google.visualization.ComboChart(document.getElementById("Year"));
                // alert(result.d);
                //chart.draw(data, options);
                if (Array.isArray(gglData) && gglData.length) {
                    GetdataChartCPTH(result.d, options, "Year", "Combo");
                } else {
                    // erros = "No data Year, ";

                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });


        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'BRAND', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
                    width: gglData.length * 65,
                    height: '100%',
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        style: 'line',
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        left: 20
                    },
                    colors: ['#4075ae']
                };
                GetdataChartCPTH(result.d, options, "Brand", "Column");
                if (Array.isArray(gglData) && gglData.length) {

                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        //YTD Distribution
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getGrowthV2",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "',Cols: 'NAME',colSum: 'GROSS_SALES_MLI', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
                    width: (gglData.length + 2) * 70,
                    height: '500',
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 120,
                        right: 100,
                        width: '600',
                    },
                    colors: ['#4075ae'],

                    seriesType: 'bars',
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { type: 'line', targetAxisIndex: 1, color: 'red' }
                    },
                    //curveType: 'function',
                    pointSize: 4,
                };
                GetdataChartCPTH(result.d, options, "Distributor", "Combo");
                if (Array.isArray(gglData) && gglData.length) {

                } else {
                    //erros = erros + "No data Distributor, ";
                    // document.getElementById("errors").innerText="asdasd";
                }

            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });


        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getGrowth",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'GROSS_SALES_MLI', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var obj = JSON.parse(result.d);
                var gglData = dataArrayCPTH(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblPerValue').innerHTML = item.GROWTH;
                });
                if (Array.isArray(gglData) && gglData.length) {

                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getYTD",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'GROSS_SALES_MLI', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                var gglData = dataArrayCPTH(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblYTDValue').innerHTML = item.YTD + " ฿";
                });
                if (Array.isArray(gglData) && gglData.length) {


                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}

function GetdataChartCPTH(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.
    //Convert Json to array
    var gglData = dataArrayCPTH(json);
    if (Array.isArray(gglData) && gglData.length) {

    }

    var data = new google.visualization.arrayToDataTable(gglData);

    drawChartCPTH(data, options, div, chartType);
    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.

    // Create the data table

}

function drawChartCPTH(data, options, div, chartType) {

    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
        //console.log(view);
        chart = new google.visualization.ColumnChart(document.getElementById(div));
        var formatterBath = new google.visualization.NumberFormat(
            { suffix: ' ฿', pattern: '#,###' });
        formatterBath.format(data, 1); // Apply formatter to one column
    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Combo") {

        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }]);
        var formatterBath = new google.visualization.NumberFormat(
            { suffix: ' ฿', pattern: '#,###' });
        var formatterPercent = new google.visualization.NumberFormat(
            { suffix: ' %', pattern: '#' });
        formatterBath.format(data, 1); // Apply formatter to one column
        formatterPercent.format(data, 2); // Apply formatter to second column
        chart = new google.visualization.ComboChart(document.getElementById(div));
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 30);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        classChangeCPTH(filter, colHeader);
    }

}

function GetdataPriChart(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.

    //Convert Json to array
    var gglData = dataArrayCPTH(json);

    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.

    // Create the data table
    var data = new google.visualization.arrayToDataTable(gglData);

    drawPriChart(data, options, div, chartType);
}

function drawPriChart(data, options, div, chartType) {
    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
        //console.log(view);
        chart = new google.visualization.ColumnChart(document.getElementById(div));

    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 100);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        classChangePri(filter, colHeader);
    }
}

function classChangePri(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'INVOICE_SALES', orderBy: 'INVOICE_SALES DESC', table: 'D_YTD_SALES_DATA_PRI'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var options = {
                    'title': 'Region',
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    animation: {
                        duration: 500,
                        startup: true
                    },
                    colors: ['#4075ae']
                };
                GetdataChartCPTH(result.d, options, "Region", "Pie");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'SUB_CATG', colSum: 'INVOICE_SALES', orderBy: 'INVOICE_SALES DESC', table: 'D_YTD_SALES_DATA_PRI'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
                    width: gglData.length * 65,
                    height: '100%',
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        style: 'line',
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        left: 20
                    },
                    colors: ['#4075ae']
                };
                GetdataChartCPTH(result.d, options, "SubCatg", "Column");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'YEAR', colSum: 'INVOICE_SALES', orderBy: 'YEAR', table: 'D_YTD_SALES_DATA_PRI'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var options = {
                    'title': 'Year',
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        style: 'line',
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 20,
                            color: '#808080'
                        },
                    },
                    colors: ['#f154d4']
                };
                GetdataChartCPTH(result.d, options, "Year", "Column");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'BRAND', colSum: 'INVOICE_SALES', orderBy: 'INVOICE_SALES DESC', table: 'D_YTD_SALES_DATA_PRI'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
                    width: gglData.length * 65,
                    height: '100%',
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        style: 'line',
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        left: 20
                    },
                    colors: ['#4075ae']
                };
                GetdataChartCPTH(result.d, options, "Brand", "Column");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getGrowth",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'INVOICE_SALES', table: 'D_YTD_SALES_DATA_PRI'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblPerValue').innerHTML = item.GROWTH;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getYTD",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'INVOICE_SALES', table: 'D_YTD_SALES_DATA_PRI'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblYTDValue').innerHTML = item.YTD;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}


function GetdataSecondaryChartCPTH(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.

    //Convert Json to array
    var gglData = dataArrayCPTH(json);

    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.

    // Create the data table
    var data = new google.visualization.arrayToDataTable(gglData);

    drawSecondaryChartCPTH(data, options, div, chartType);
}

function drawSecondaryChartCPTH(data, options, div, chartType) {

    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
        chart = new google.visualization.ColumnChart(document.getElementById(div));
    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Bar") {
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
        chart = new google.visualization.BarChart(document.getElementById(div));
        var formatterBath = new google.visualization.NumberFormat(
            { suffix: ' ฿', pattern: '#,###' });
        formatterBath.format(data, 1); // Apply formatter to one column


    }
    else if (chartType === "Combo") {
        chart = new google.visualization.ComboChart(document.getElementById(div));
        var view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }]);
        var formatterBath = new google.visualization.NumberFormat(
            { suffix: ' ฿', pattern: '#,###' });
        var formatterPercent = new google.visualization.NumberFormat(
            { suffix: ' %', pattern: '#' });
        formatterBath.format(data, 1); // Apply formatter to one column
        formatterPercent.format(data, 2); // Apply formatter to second column
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 30);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        classChangeSecondaryCPTH(filter, colHeader);
    }
}

function classChangeSecondaryCPTH(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var color = '';
                for (var i = 1; i < gglData.length; i++) {
                    colorRegion = gglData[i][0];
                    switch (colorRegion) {
                        case 'NORTHEAST':
                            color += '#006266' + ',';
                            break;
                        case 'BANGKOK':
                            color += '#DC3912' + ',';
                            break;
                        case 'SOUTH':
                            color += '#FF9900' + ',';
                            break;
                        case 'CENTRAL':
                            color += '#9966ff' + ',';
                            break;
                        case 'NORTH':
                            color += '#990099' + ',';
                            break;
                        default:
                            color += '#4075ae' + ',';
                    }
                }
                var listColor = color.split(',');
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    animation: {
                        duration: 500,
                        startup: true
                    },
                    chartArea: {
                        bottom: 10,
                        left: 10,
                        width: '80%',
                        height: '80%'
                    },
                    colors: listColor,
                    fontName: 'Tahoma',
                    fontSize: 11,
                };

                GetdataSecondaryChartCPTH(result.d, options, "Region", "Pie");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'PROVINCE', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 11,
                    width: '100%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20
                    },
                    colors: ['#4075ae'],
                    hAxis: { viewWindow: { min: 0 } },
                };
                GetdataSecondaryChartCPTH(result.d, options, "Province", "Bar");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV2",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_SECONDATY_SALES_UPDATE',ACTUAL: 'ACTUAL',TARGET :'TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 11,
                    width: gglData.length * 75,
                    height: '200',
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 80,
                        right: 10,
                    },
                    colors: ['#4075ae'],
                    seriesType: 'bars',
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { type: 'line', targetAxisIndex: 1, color: 'red' }
                    },
                    pointSize: 4,
                };
                GetdataSecondaryChartCPTH(result.d, options, "Distributor", "Combo");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'CDS', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 11,
                    width: '100%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20
                    },
                    colors: ['#4075ae']
                };
                GetdataSecondaryChartCPTH(result.d, options, "CDS", "Bar");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV2",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'DSR_NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_SECONDATY_SALES_UPDATE',ACTUAL: 'ACTUAL',TARGET :'TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 11,
                    width: gglData.length * 85,
                    height: '200',
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 100,
                    },
                    colors: ['#4075ae'],
                    seriesType: 'bars',
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { type: 'line', targetAxisIndex: 1, color: 'red' }
                    },
                    pointSize: 4,
                };
                GetdataSecondaryChartCPTH(result.d, options, "DSR", "Combo");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataOrtherPercent",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'TIME_GONE', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblTimeGoneValue').innerText = item.ORTHERPERCENT;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataOrtherPercent",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'ECC', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblECCValue').innerHTML = item.ORTHERPERCENT;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataOrtherPercent",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'VISITED', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblVisitValue').innerHTML = item.ORTHERPERCENT;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataPercent",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'ACTUAL', colSum2: 'TARGET', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblPercentValue').innerHTML = item.PERCENTTAG;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataMTD",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'ACTUAL', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblMTDValue').innerHTML = item.MTD;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataMTD",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'TARGET', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblTargetValue').innerHTML = item.MTD;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}

function GetdataPriMTDChart(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.

    //Convert Json to array
    var gglData = dataArrayCPTH(json);

    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.

    // Create the data table
    var data = new google.visualization.arrayToDataTable(gglData);

    drawPriMTDChart(data, options, div, chartType);

}

function drawPriMTDChart(data, options, div, chartType) {
    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        chart = new google.visualization.ColumnChart(document.getElementById(div));
        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Bar") {
        chart = new google.visualization.BarChart(document.getElementById(div));
        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 30);
    }
    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        $(".loader-wrapper").fadeIn('fast');
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        classChangePriMTD(filter, colHeader);
    }
    google.charts.setOnLoadCallback(
        function () {
            setTimeout(function () {
                $(".loader-wrapper").fadeOut('Slow');
            }, 500)
        });
}

function classChangePriMTD(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var options = {
                    'title': 'Region',
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    animation: {
                        duration: 500,
                        startup: true
                    },
                    colors: ['#4075ae']
                };
                GetdataPriMTDChart(result.d, options, "Region", "Pie");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'PROVINCE', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
                    width: '60%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20
                    },
                    colors: ['#4075ae']
                };
                GetdataPriMTDChart(result.d, options, "Province", "Bar");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
                    width: '60%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20
                    },
                    colors: ['#4075ae']
                };
                GetdataPriMTDChart(result.d, options, "Distributor", "Bar");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'CDS', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
                    width: '60%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 10,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20
                    },
                    colors: ['#4075ae']
                };
                GetdataPriMTDChart(result.d, options, "CDS", "Bar");
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataOrtherPercent",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'TIME_GONE', table: 'D_MTD_PRIMARY_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblTimeGoneValue').innerText = item.ORTHERPERCENT;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataPercent",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'ACTUAL', colSum2: 'TRGET', table: 'D_MTD_PRIMARY_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblPercentValue').innerHTML = item.PERCENTTAG;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataMTD",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'ACTUAL', table: 'D_MTD_PRIMARY_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblMTDValue').innerHTML = item.MTD;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDataMTD",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'TRGET', table: 'D_MTD_PRIMARY_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblTargetValue').innerHTML = item.MTD;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}

//-------------------------Code for mtd vs target chart--------------------------------------

function GetDataMTDandTargetChartV2(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.
    //Convert Json to array
    var gglData = dataArrayCPTH(json);
    var data = new google.visualization.arrayToDataTable(gglData);
    drawMTDandTargetChartV2(data, options, div, chartType);
    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.
}

function drawMTDandTargetChartV2(data, options, div, chartType) {
    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }]);
        chart = new google.visualization.ColumnChart(document.getElementById(div));
    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Bar") {
        chart = new google.visualization.BarChart(document.getElementById(div));
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" },]);
    }
    else if (chartType === "Donut") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 100);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        if (colHeader == 'Coverage')
            return;
        classChangeMTDandTargetV2(filter, colHeader);
    }
    google.charts.setOnLoadCallback(
        function () {
            setTimeout(function () {
                $(".loader-wrapper").fadeOut('Slow');
            }, 500)
        }
    );
}

function classChangeMTDandTargetV2(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV4",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'MTD', colSum: 'ACTUAL', orderBy: '', table: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var obj = JSON.parse(result.d);
                var arrayItem = [];
                obj.forEach(function (item) {
                    arrayItem.push(item.TotalCoverage);
                });
                var actual = arrayItem[0];
                var summary = arrayItem[1];
                document.getElementById('Actual-value').innerHTML = formatNumber(actual) + ' (' + ((actual / (actual + summary)) * 100).toFixed(1) + '%)';
                var options = {
                    slices: {
                        o: {},
                        1: { color: '#F0F5F7' }
                    },
                    pieHole: 0.8,
                    pieStartAngle: 0,
                    pieSliceText: 'none',
                    legend: { position: 'none' },
                    tooltip: { trigger: 'selection' },
                    tooltip: { textStyle: { fontSize: 13 } },
                    chartArea: {
                        top: 20,
                        bottom: 0,
                    },
                    colors: ['#4477aa'],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataMTDandTargetChartV2(result.d, options, "dbActual", "Pie");
                } else {
                    // erros = "No data Region, ";
                    // document.getElementById("errors").innerText="asdasd";
                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getTarget",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'MTD', colSum: 'ACTUAL', orderBy: '', table: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var obj = JSON.parse(result.d);
                var arrayItem = [];
                obj.forEach(function (item) {
                    arrayItem.push(item.TotalCoverage);
                });
                document.getElementById('Target-value').innerHTML = formatNumber(arrayItem[0]);
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV4",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'ACTUAL', orderBy: 'REGION', table: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 14
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '470',
                    height: (gglData.length + 2) * 70,
                    bar: { groupWidth: "80%" },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'right' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 100,
                        width: '50%',
                        bottom: 50
                    },
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { targetAxisIndex: 0 },
                    },
                    hAxis: { viewWindow: { min: 0 } },
                    colors: ['#4477aa', '#cc6677'],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataMTDandTargetChartV2(result.d, options, "dbRegion", "Bar");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        })

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV4",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'DISTRIBUTOR', colSum: 'ACTUAL', orderBy: 'DISTRIBUTOR', table: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 14
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '450',
                    height: (gglData.length + 2) * 70,
                    bar: { groupWidth: "80%" },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'right' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 130,
                        width: '200',
                        bottom: 50
                    },
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { targetAxisIndex: 0 }
                    },
                    hAxis: { viewWindow: { min: 0 } },
                    colors: ['#4477aa', '#cc6677'],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataMTDandTargetChartV2(result.d, options, "dbDistributor", "Bar");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        })

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV4",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'DSR', colSum: 'ACTUAL', orderBy: 'DSR_CODE', table: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 14
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: (gglData.length + 2) * 120,
                    height: 220,
                    bar: { groupWidth: "80%" },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'right' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 130,
                        right: 150,
                        bottom: 50
                    },
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { targetAxisIndex: 0 }
                    },
                    hAxis: { viewWindow: { min: 0 } },
                    colors: ['#4477aa', '#cc6677'],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataMTDandTargetChartV2(result.d, options, "dbSales", "Column");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        })
    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}

function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
}

//------------------------- Code for distributor daily summary -------------------------


function GetDataDistributorDailySummaryV2(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.
    //Convert Json to array
    var gglData = dataArrayCPTH(json);
    var data = new google.visualization.arrayToDataTable(gglData);
    drawDistributorDailySummaryV2(data, options, div, chartType);
    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.
}

function drawDistributorDailySummaryV2(data, options, div, chartType) {
    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }]);
        chart = new google.visualization.ColumnChart(document.getElementById(div));
    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Bar") {
        chart = new google.visualization.BarChart(document.getElementById(div));
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }]);
    }
    else if (chartType === "Donut") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 100);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        if (colHeader == 'Coverage')
            return;
        classChangeDistributorDailySummaryV2(filter, colHeader);
    }
    google.charts.setOnLoadCallback(
        function () {
            setTimeout(function () {
                $(".loader-wrapper").fadeOut('Slow');
            }, 500)
        }
    );
}

function classChangeDistributorDailySummaryV2(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        //Sellout - Year
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV3",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'YEAR', colSum: 'ACTUAL', orderBy: 'YEAR ASC', table: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 14
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '70%',
                    height: '100%',
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'right' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 50,
                        left: 80,
                        right: 100,
                        width: '80%',
                    },
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { targetAxisIndex: 0 }
                    },
                    colors: ['#4477aa', '#cc6677'],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataDistributorDailySummaryV2(result.d, options, "dbYear", "Bar");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
        //Sellout - Quarter
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV3",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'QUARTER', colSum: 'ACTUAL', orderBy: 'QUARTER ASC', table: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    height: '80%',
                    fontName: 'Tahoma',
                    fontSize: 13,
                    legend: { position: 'right' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    bar: { groupWidth: "35%" },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    chartArea: {
                        top: 30,
                        right: 100,
                        bottom: 20,
                        left: 100,
                    },
                    colors: ['#4477aa', '#cc6677'],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataDistributorDailySummaryV2(result.d, options, "dbQuarter", "Column");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        //Sellout - Month
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharV3",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'MONTH', colSum: 'ACTUAL', orderBy: 'MONTH_NUMBER ASC', table: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    width: '80%',
                    height: '100%',
                    fontName: 'Tahoma',
                    fontSize: 13,
                    legend: { position: 'right' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    bar: { groupWidth: "30%" },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    chartArea: {
                        top: 50,
                        right: 100,
                        width: '85%',
                    },
                    colors: ['#4477aa', '#cc6677'],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataDistributorDailySummaryV2(result.d, options, "dbMonth", "Column");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}


//---------------------------CPTH D_MSS_DISTRIBUTION LOAD CHART------------------------
function GetdataMSSDistributorChartCPTH(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.

    //Convert Json to array
    var gglData = dataArrayCPTH(json);

    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.

    // Create the data table
    var data = new google.visualization.arrayToDataTable(gglData);

    drawMSSDistributorChartCPTH(data, options, div, chartType);
}

function drawMSSDistributorChartCPTH(data, options, div, chartType) {

    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
        chart = new google.visualization.ColumnChart(document.getElementById(div));
    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Bar") {
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
        chart = new google.visualization.BarChart(document.getElementById(div));
        //var formatterBath = new google.visualization.NumberFormat(
        //    { suffix: '', pattern: '#,###.0' });
        //formatterBath.format(data, 1); // Apply formatter to one column


    }
    else if (chartType === "Combo") {
        chart = new google.visualization.ComboChart(document.getElementById(div));
        var view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }]);
        var formatterBath = new google.visualization.NumberFormat(
            { suffix: ' ฿', pattern: '#,###' });
        var formatterPercent = new google.visualization.NumberFormat(
            { suffix: ' %', pattern: '#' });
        formatterBath.format(data, 1); // Apply formatter to one column
        formatterPercent.format(data, 2); // Apply formatter to second column
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 30);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        classChangeMSSDistributorCPTH(filter, colHeader);
    }
}
function classChangeMSSDistributorCPTH(filter, colHeader) {
    //let list = document.getElementById('<%=rblMeasurementSystem.ClientID%>');
    //let inputs = list.getElementsByTagName("input");


    let filterRE = sessionStorage.getItem("filterRE");

    sessionStorage.setItem("filter", filter);
    sessionStorage.setItem("colHeader", colHeader);
    //alert(filterRE);
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharCPTH",
            data: "{  filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);

                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 13
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '80%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 100,
                    },
                    colors: ['#0000FF'],

                };

                if (Array.isArray(gglData) && gglData.length) {
                    GetdataMSSDistributorChartCPTH(result.d, options, "RegionMSS", "Bar");
                } else {
                    // erros = "No data Region, ";
                    // document.getElementById("errors").innerText="asdasd";
                }
            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharCPTH",
            data: "{  filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'DISTRIBUTOR_NAME', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '60%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 200
                    },
                    colors: ['#0000FF'],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetdataMSSDistributorChartCPTH(result.d, options, "DistributorMSS", "Bar");
                } else {
                    let erros = "No data Distributor";
                    document.getElementById("DistributorMSS").innerHTML = "<span style='color:red'> " + erros + "</span>";
                }

            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });

        //MP
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'503'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var stringjob = JSON.parse(result.d);
                //console.log(result.d);
                delete stringjob["SALES_NAME"];
                var data = JSON.stringify(stringjob);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 13
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '80%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 80,
                    },
                    colors: ['#FFFF00'],
                };

                if (Array.isArray(gglData) && gglData.length) {
                    GetdataMSSDistributorChartCPTH(data, options, "MP", "Bar");
                } else {
                    let erros = "No data MP";
                    document.getElementById("MP").innerHTML = "<span style='color:red'> " + erros + "</span>";
                }
            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });
        //WS
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'602'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var stringjob = JSON.parse(result.d);
                //console.log(result.d);
                delete stringjob["SALES_NAME"];
                var data = JSON.stringify(stringjob);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 13
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '80%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 80,
                    },
                    colors: ['#A7FFA7'],
                    legend: { position: 'none' },
                };

                if (Array.isArray(gglData) && gglData.length) {
                    GetdataMSSDistributorChartCPTH(data, options, "WS", "Bar");
                } else {
                    let erros = "No data WS";
                    document.getElementById("WS").innerHTML = "<span style='color:red'> " + erros + "</span>";
                }
            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });
        //SPM
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'502'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var stringjob = JSON.parse(result.d);
                //console.log(result.d);
                delete stringjob["SALES_NAME"];
                var data = JSON.stringify(stringjob);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 13
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '80%',
                    height: gglData.length * 50,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 80,
                    },
                    colors: ['#FF99FF'],
                    legend: { position: 'none' },
                };

                if (Array.isArray(gglData) && gglData.length) {
                    GetdataMSSDistributorChartCPTH(data, options, "SPM", "Bar");
                } else {
                    let erros = "No data SPM";
                    document.getElementById("SPM").innerHTML = "<span style='color:red'> " + erros + "</span>";
                }
            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });
        //MNM
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'504'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                var stringjob = JSON.parse(result.d);
                //console.log(result.d);
                delete stringjob["SALES_NAME"];
                var data = JSON.stringify(stringjob);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 13
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '80%',
                    height: gglData.length * 45,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 80,
                    },
                    colors: ['#00B0F0'],
                    legend: { position: 'none' },
                };

                if (Array.isArray(gglData) && gglData.length) {
                    GetdataMSSDistributorChartCPTH(data, options, "MNM", "Bar");
                } else {
                    let erros = "No data MNM";
                    document.getElementById("MNM").innerHTML = "<span style='color:red'> " + erros + "</span>";
                }
            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });
        //getdata total PointRL
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getTotalPointRL",
            data: "{  filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC',table: 'D_MSS_DISTRIBUTION'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblTotalPointRL').innerHTML = item.Point_RL;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getTimeGone",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'TIME_GONE', orderBy: 'TIME_GONE DESC',table: 'D_MSS_DISTRIBUTION'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                //console.log(result.d)
                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {

                    document.getElementById('lblTimeGone').innerHTML = "Time Gone:" + item.TIME_GONE;
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}
//---------------------------END MTD_DISTRIBTION---------------------------------------

//---------------------------CPTH D_Coverage_Buying_StoreVisit-------------------------
function GetdataChangeCoverageBuyingStoreVisit(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.

    //Convert Json to array
    var gglData = dataArrayCPTH(json);

    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.

    // Create the data table
    var data = null;
    if (gglData != null || gglData != undefined) {
        data = new google.visualization.arrayToDataTable(gglData);
    }
    drawChangeCoverageBuyingStoreVisit(data, options, div, chartType);
}

function drawChangeCoverageBuyingStoreVisit(data, options, div, chartType) {
    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
        chart = new google.visualization.ColumnChart(document.getElementById(div));
    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Bar") {
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }, 3, { calc: "stringify", sourceColumn: 3, type: "string", role: "annotation" }], 1, 2, 3);
        chart = new google.visualization.BarChart(document.getElementById(div));
        //var formatterBath = new google.visualization.NumberFormat(
        //    { suffix: '', pattern: '#,###.0' });
        //formatterBath.format(data, 1); // Apply formatter to one column


    }
    else if (chartType === "Combo") {
        chart = new google.visualization.ComboChart(document.getElementById(div));
        var view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }]);
        var formatterBath = new google.visualization.NumberFormat(
            { suffix: ' ฿', pattern: '#,###' });
        var formatterPercent = new google.visualization.NumberFormat(
            { suffix: ' %', pattern: '#' });
        formatterBath.format(data, 1); // Apply formatter to one column
        formatterPercent.format(data, 2); // Apply formatter to second column
    } else if (chartType === "Donut") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 30);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        classChangeCoverageBuyingStoreVisit(filter, colHeader);
    }
}
function classChangeCoverageBuyingStoreVisit(filter, colHeader) {
    //let list = document.getElementById('<%=rblMeasurementSystem.ClientID%>');
    //let inputs = list.getElementsByTagName("input");

    //alert(filterRE);
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        //Region
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getCoverageBuyingStoreVisit",
            data: "{  filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'COVERAGE',colSum2:'BUYING',colSum3:'STORE_VISIT', orderBy: 'COVERAGE DESC', table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                console.log(gglData);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 13
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '60%',
                    height: (gglData.length + 2) * 70,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 100,
                    },
                    colors: ['#DB9F81', '#00B0F0', '#92D050'],
                    legend: { position: 'right' },
                    bar: { groupWidth: "95%" },
                };
                //var data = new google.visualization.arrayToDataTable(gglData);
                // Console.log(data);

                if (Array.isArray(gglData) && gglData.length) {
                    GetdataChangeCoverageBuyingStoreVisit(result.d, options, "Region", "Bar");
                } else {
                    // erros = "No data Region, ";
                    // document.getElementById("errors").innerText="asdasd";
                }
            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });
        //Distributor

        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getCoverageBuyingStoreVisit",
            data: "{  filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'DISTRIBUTOR_NAME', colSum: 'COVERAGE',colSum2:'BUYING',colSum3:'STORE_VISIT', orderBy: 'COVERAGE DESC', table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                //console.log(gglData);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 13
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '60%',
                    height: (gglData.length + 2) * 70,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 100,
                    },
                    colors: ['#DB9F81', '#00B0F0', '#92D050'],
                    legend: { position: 'right' },
                    bar: { groupWidth: "95%" },
                };
                //var data = new google.visualization.arrayToDataTable(gglData);
                // Console.log(data);

                if (Array.isArray(gglData) && gglData.length) {
                    GetdataChangeCoverageBuyingStoreVisit(result.d, options, "Distributor", "Bar");
                } else {
                    // erros = "No data Region, ";
                    // document.getElementById("errors").innerText="asdasd";
                }
            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });

        //DSR
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getCoverageBuyingStoreVisit",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'SALES_NAME', colSum: 'COVERAGE', colSum2:'BUYING', colSum3:'STORE_VISIT', orderBy: 'COVERAGE DESC', table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArrayCPTH(result.d);
                console.log(gglData);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 13
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '60%',
                    height: (gglData.length + 2) * 70,
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 100,
                    },
                    colors: ['#DB9F81', '#00B0F0', '#92D050'],
                    legend: { position: 'right' },
                    bar: { groupWidth: "90%" },
                };
                //var data = new google.visualization.arrayToDataTable(gglData);
                // Console.log(data);

                if (Array.isArray(gglData) && gglData.length) {
                    GetdataChangeCoverageBuyingStoreVisit(result.d, options, "DSR", "Bar");
                } else {
                    // erros = "No data Region, ";
                    // document.getElementById("errors").innerText="asdasd";
                }
            },
            error: function (result, status, error) {
                alert("a");
            },
            async: false
        });
        //StoreVisit
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getECC_StoreVisited_CoverageBuyingStoreVisit",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'STORE_VISIT', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblStoreVisited').innerHTML = item.Store_visited + "%";
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
        //Buying
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getECC_StoreVisited_CoverageBuyingStoreVisit",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "',colSum: 'Buying', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var obj = JSON.parse(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblECC').innerHTML = item.Store_visited + "%";
                });
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        //Donut Buying
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDonutChart",
            data: "{filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'Buying', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {


                var gglData = dataArrayCPTH(result.d);
                var obj = JSON.parse(result.d);
                var arrayItem = [];
                var arrayRL = [];
                obj.forEach(function (item) {
                    arrayItem.push(item.TotalCoverage);
                    arrayRL.push(item.RL);
                });
                let buying = arrayItem[0];
                let RL = arrayRL[0]
                let percentage;
                if (buying == 0 || RL == 0) {
                    percentage = 0;
                } else {
                    percentage = ((buying / RL) * 100).toFixed(1);
                }
                document.getElementById('Buying_data').innerHTML = formatNumber(arrayItem[0]) + " (" + percentage + "%)";
                document.getElementsByClassName('data_coverage')[0].innerHTML = formatNumber(arrayRL[0]);


                var options = {
                    slices: {
                        0: { color: '#00B0F0' },
                        1: { color: '#F7DFD2' }
                    },
                    //title: titler,
                    pieHole: 0.8,
                    pieStartAngle: 0,
                    pieSliceText: 'none',
                    legend: { position: 'none' },
                    tooltip: { trigger: 'selection' },
                    tooltip: { textStyle: { fontSize: 13 } },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 90,
                    },
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetdataChangeCoverageBuyingStoreVisit(result.d, options, "Buying", "Donut");
                } else {
                    // erros = "No data Region, ";
                    // document.getElementById("errors").innerText="asdasd";
                }

            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        //Donut StoreVisited
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDonutChart",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'STORE_VISIT', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var gglData = dataArrayCPTH(result.d);
                var obj = JSON.parse(result.d);
                var arrayItem = [];
                var arrayRL = [];

                obj.forEach(function (item) {
                    arrayItem.push(item.TotalCoverage);
                    arrayRL.push(item.RL);
                });
                let visited = arrayItem[0];
                let RL = arrayRL[0];
                let percentage;
                if (visited == 0 || RL == 0) {
                    percentage = 0;
                } else {
                    percentage = ((visited / RL) * 100).toFixed(1);
                }
                document.getElementById('store_visited_data').innerHTML = formatNumber(arrayItem[0]) + " (" + percentage + "%)";;
                document.getElementsByClassName('data_coverage')[1].innerHTML = formatNumber(arrayRL[0]);
                var options = {
                    slices: {
                        0: { color: '#92D050' },
                        1: { color: '#F7DFD2' }
                    },
                    //title: titler,
                    pieHole: 0.8,
                    pieStartAngle: 0,
                    //pieSliceText: 'none',
                    legend: { position: 'none' },
                    tooltip: { trigger: 'selection' },
                    tooltip: { textStyle: { fontSize: 13 } },
                    chartArea: {
                        width: '50%',
                        top: 20,
                        left: 90,
                    },
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetdataChangeCoverageBuyingStoreVisit(result.d, options, "StoreVisited", "Donut");
                } else {
                    // erros = "No data Region, ";
                    // document.getElementById("errors").innerText="asdasd";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });
        //

    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}
//---------------------------EndCPTH D_Coverage_Buying_StoreVisit----------------------


//---------------------------D_PAR in Percenage----------------------------------------

function GetDataMTDPARinPercentage(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.
    //Convert Json to array
    var gglData = dataArrayCPTH(json);
    var data = new google.visualization.arrayToDataTable(gglData);
    drawMTDandPARinPercentage(data, options, div, chartType);
    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.
}

function drawMTDandPARinPercentage(data, options, div, chartType) {
    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, { calc: "stringify", sourceColumn: 2, type: "string", role: "style" }]);
        chart = new google.visualization.ColumnChart(document.getElementById(div));
    }
    else if (chartType === "Line") {
        chart = new google.visualization.LineChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Bar") {
        chart = new google.visualization.BarChart(document.getElementById(div));
        view = new google.visualization.DataView(data);
        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }, { calc: "stringify", sourceColumn: 4, type: "string", role: "style" }, 2, { calc: "stringify", sourceColumn: 2, type: "string", role: "annotation" }, { calc: "stringify", sourceColumn: 3, type: "string", role: "style" }]);
    }
    else if (chartType === "Donut") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    chart.draw(view, options);

    if (chartType === "Pie") {
        var val1 = view.getValue(0, 1);
        var val2 = view.getValue(1, 1);
        var total = val1 + val2;
        var percent = val1 % 10;
        var tmp = Math.floor(val1 / 10);
        // start the animation loop
        var handler = setInterval(function () {
            // values increment
            percent += tmp;
            // apply new values
            view.setValue(0, 1, percent);
            view.setValue(1, 1, total - percent);
            // update the pie
            chart.draw(view, options);
            // check if we have reached the desired value
            if (percent >= val1)
                // stop the loop
                clearInterval(handler);
        }, 100);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        if (colHeader == 'Coverage')
            return;
        classChangeMTDPARinPercentage(filter, colHeader);
    }
    google.charts.setOnLoadCallback(
        function () {
            setTimeout(function () {
                $(".loader-wrapper").fadeOut('Slow');
            }, 500)
        }
    );
}

function classChangeMTDPARinPercentage(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getPARinPercentage",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "',Cols: 'REGION', table: 'D_MTD_PRIMARY_UPDATE', table2: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var stringjob = JSON.parse(result.d);
                let time_gone = sessionStorage.getItem("Time_gone");
                for (let key in stringjob) {
                    let Sec = stringjob[key]["Sec"];
                    let Pri = stringjob[key]["Pri"];
                    let Sec_timegon = (Sec - time_gone).toFixed(0);
                    let Pri_timegon = (Pri - time_gone).toFixed(0);

                    //PRI

                    if (Pri_timegon < - 10) {
                        stringjob[key]["STYLE_PRI"] = "color:#FE0000";

                    } else if (-10 <= Pri_timegon && Pri_timegon <= -5) {
                        stringjob[key]["STYLE_PRI"] = "color:#FFC000";
                    } else if (-4 <= Pri_timegon && Pri_timegon <= 5) {
                        stringjob[key]["STYLE_PRI"] = "color:#00B050";
                    } else if (Pri_timegon > 5) {
                        stringjob[key]["STYLE_PRI"] = "color:#00FF00";
                    }

                    //SEC
                    if (Sec_timegon < - 10) {
                        stringjob[key]["STYLE_SEC"] = "color:#FE0000";

                    } else if (-10 <= Sec_timegon && Sec_timegon <= -5) {
                        stringjob[key]["STYLE_SEC"] = "color:#FFC000";
                    } else if (-4 <= Sec_timegon && Sec_timegon <= 5) {
                        stringjob[key]["STYLE_SEC"] = "color:#00B050";
                    } else if (Sec_timegon > 5) {
                        stringjob[key]["STYLE_SEC"] = "color:#00FF00";
                    }
                }
                var data = JSON.stringify(stringjob);
                var gglData = dataArrayCPTH(data);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 14
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '470',
                    height: (gglData.length + 2) * 70,
                    bar: { groupWidth: "50%" },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 100,
                        width: '50%',
                        bottom: 50
                    },
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { targetAxisIndex: 1 }
                    },
                    hAxis: { viewWindow: { min: 0 } },
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataMTDPARinPercentage(data, options, "dbRegion", "Bar");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        })
        //Distributor
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getPARinPercentage",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'DISTRIBUTOR_NAME', table: 'D_MTD_PRIMARY_UPDATE', table2: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var stringjob = JSON.parse(result.d);
                let time_gone = sessionStorage.getItem("Time_gone");
                for (let key in stringjob) {
                    let Sec = stringjob[key]["Sec"];
                    let Pri = stringjob[key]["Pri"];
                    let Sec_timegon = (Sec - time_gone).toFixed(0);
                    let Pri_timegon = (Pri - time_gone).toFixed(0);

                    if (Pri_timegon < - 10) {
                        stringjob[key]["STYLE_PRI"] = "color:#FE0000";

                    } else if (-10 <= Pri_timegon && Pri_timegon <= -5) {
                        stringjob[key]["STYLE_PRI"] = "color:#FFC000";
                    } else if (-4 <= Pri_timegon && Pri_timegon <= 5) {
                        stringjob[key]["STYLE_PRI"] = "color:#00B050";
                    } else if (Pri_timegon > 5) {
                        stringjob[key]["STYLE_PRI"] = "color:#00FF00";
                    }

                    //SEC
                    if (Sec_timegon < - 10) {
                        stringjob[key]["STYLE_SEC"] = "color:#FE0000";

                    } else if (-10 <= Sec_timegon && Sec_timegon <= -5) {
                        stringjob[key]["STYLE_SEC"] = "color:#FFC000";
                    } else if (-4 <= Sec_timegon && Sec_timegon <= 5) {
                        stringjob[key]["STYLE_SEC"] = "color:#00B050";
                    } else if (Sec_timegon > 5) {
                        stringjob[key]["STYLE_SEC"] = "color:#00FF00";
                    }
                }
                var data = JSON.stringify(stringjob);
                var gglData = dataArrayCPTH(data);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 14
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: '470',
                    height: (gglData.length + 2) * 70,
                    bar: { groupWidth: "85%" },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 100,
                        width: '50%',
                        bottom: 50
                    },
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { targetAxisIndex: 1 }
                    },
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataMTDPARinPercentage(data, options, "dbDistributor", "Bar");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

        //DSR
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/getDSRPARinPercentage",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'DSR', table: 'D_DISTRIBUTOR_PAR_PRI', table2: 'D_MTD_ACT_TARGET'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result, status, xhr) {
                var stringjob = JSON.parse(result.d);
                let time_gone = sessionStorage.getItem("Time_gone");
                for (let key in stringjob) {
                    let Sec = stringjob[key]["Sec"];
                    let Sec_timegon = (Sec - time_gone).toFixed(0);
                    //SEC
                    if (Sec_timegon < - 10) {
                        stringjob[key]["STYLE_SEC"] = "color:#FE0000";

                    } else if (-10 <= Sec_timegon && Sec_timegon <= -5) {
                        stringjob[key]["STYLE_SEC"] = "color:#FFC000";
                    } else if (-4 <= Sec_timegon && Sec_timegon <= 5) {
                        stringjob[key]["STYLE_SEC"] = "color:#00B050";
                    } else if (Sec_timegon > 5) {
                        stringjob[key]["STYLE_SEC"] = "color:#00FF00";
                    }
                }

                var data = JSON.stringify(stringjob);
                var gglData = dataArrayCPTH(data);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 14
                    },
                    fontName: 'Tahoma',
                    fontSize: 13,
                    width: (gglData.length + 2) * 90,
                    height: 220,
                    bar: { groupWidth: "85%" },
                    animation: {
                        duration: 500,
                        startup: true //This is the new option
                    },
                    legend: { position: 'none' },
                    annotations: {
                        textStyle: {
                            fontName: 'Tahoma',
                            fontSize: 13,
                            color: '#808080'
                        },
                    },
                    chartArea: {
                        top: 20,
                        left: 130,
                        right: 150,
                        bottom: 50
                    },

                    colors: ["#DC3912"],
                };
                if (Array.isArray(gglData) && gglData.length) {
                    GetDataMTDPARinPercentage(data, options, "dbSales", "Column");
                } else {
                    // erros = "No data Region, ";
                }
            },
            error: function (result, status, error) {
                var err = eval("(" + result.responseText + ")");
                alert(err.Message);
            },
            async: false
        });

    }, 300);
    $(".loader-wrapper").fadeOut('slow');
}

//---------------------------End D_PAR in Percenage------------------------------------