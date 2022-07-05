function dataArray(jsonData) {
    var object = JSON.parse(jsonData);
    var gglData = [];
    if (object.length > 0) {
        // load column headings
        var colHead = [];
        Object.keys(object[0]).forEach(function (key) {
            colHead.push(key);
        });
        gglData.push(colHead);
        console.log(gglData);
        // load data rows
        object.forEach(function (row) {
            var gglRow = [];
            Object.keys(row).forEach(function (key) {
                gglRow.push(row[key]);
            });
            gglData.push(gglRow);
        });
    }
    console.log(gglData);
    return gglData;
}

function classChange(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArray(result.d);
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
                console.log(result.d);
                GetdataChart(result.d, options, "Region", "Pie");
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
                var gglData = dataArray(result.d);
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
                GetdataChart(result.d, options, "SubCatg", "Column");
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
                var gglData = dataArray(result.d);
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
                GetdataChart(result.d, options, "Year", "Column");
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
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'BRAND', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArray(result.d);
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
                GetdataChart(result.d, options, "Brand", "Column");
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
            url: "/Dashboard/ChartService.asmx/getGrowth",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', colSum: 'GROSS_SALES_MLI', table: 'D_YTD_SALES_DATA_SEC'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var obj = JSON.parse(result.d);
                var gglData = dataArray(result.d);
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
                console.log(result.d);
                var obj = JSON.parse(result.d);
                var gglData = dataArray(result.d);
                obj.forEach(function (item) {
                    document.getElementById('lblYTDValue').innerHTML = item.YTD;
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
    },300);
    $(".loader-wrapper").fadeOut('slow');
}

function GetdataChart(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.
    //Convert Json to array
    var gglData = dataArray(json);
    if (Array.isArray(gglData) && gglData.length) {
      
    }
   
    var data = new google.visualization.arrayToDataTable(gglData);

    drawChart(data, options, div, chartType);
    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.

    // Create the data table
    
}

function drawChart(data, options, div, chartType) {

    var chart = [];
    var view;
    if (chartType === "Pie") {
        chart = new google.visualization.PieChart(document.getElementById(div));
        view = data;
    }
    else if (chartType === "Column") {
        view = new google.visualization.DataView(data);

        view.setColumns([0, 1, { calc: "stringify", sourceColumn: 1, type: "string", role: "annotation" }], 2);
        console.log(view);
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
        }, 30);
    }

    google.visualization.events.addListener(chart, 'select', ClickHandler);
    function ClickHandler() {
        var colHeader = view.getColumnLabel(0);
        selectedData = chart.getSelection();
        row = selectedData[0].row;
        var filter = view.getValue(row, 0);
        classChange(filter, colHeader);
    }

}

function GetdataPriChart(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.

    //Convert Json to array
    var gglData = dataArray(json);

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
        console.log(view);
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
                GetdataPriChart(result.d, options, "Region", "Pie");
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
                var gglData = dataArray(result.d);
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
                GetdataPriChart(result.d, options, "SubCatg", "Column");
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
                GetdataPriChart(result.d, options, "Year", "Column");
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
                var gglData = dataArray(result.d);
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
                GetdataPriChart(result.d, options, "Brand", "Column");
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
                console.log(result.d);
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
                console.log(result.d);
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


function GetdataSecondaryChart(json, options, div, chartType) {
    // Load the Visualization API and the corechart package.
    // Set a callback to run when the Google Visualization API is loaded.

    //Convert Json to array
    var gglData = dataArray(json);

    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.

    // Create the data table
    var data = new google.visualization.arrayToDataTable(gglData);

    drawSecondaryChart(data, options, div, chartType);
}

function drawSecondaryChart(data, options, div, chartType) {

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
        classChangeSecondary(filter, colHeader);
    }
}

function classChangeSecondary(filter, colHeader) {
    $(".loader-wrapper").fadeIn('fast');
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "/Dashboard/ChartService.asmx/OnClickChar",
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'REGION', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
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
                GetdataSecondaryChart(result.d, options, "Region", "Pie");
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
                var gglData = dataArray(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
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
                GetdataSecondaryChart(result.d, options, "Province", "Bar");
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
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArray(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
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
                GetdataSecondaryChart(result.d, options, "Distributor", "Bar");
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
                var gglData = dataArray(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
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
                GetdataSecondaryChart(result.d, options, "CDS", "Bar");
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
            data: "{ filter: '" + filter + "' , colFilter: '" + colHeader + "', Cols: 'DSR_NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_SECONDATY_SALES_UPDATE'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var gglData = dataArray(result.d);
                var options = {
                    titleTextStyle: {
                        color: '#808080',
                        fontSize: 16
                    },
                    fontName: 'Tahoma',
                    fontSize: 12,
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
                GetdataSecondaryChart(result.d, options, "DSR", "Bar");
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
                console.log(result.d);
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
                console.log(result.d);
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
                console.log(result.d);
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
                console.log(result.d);
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
                console.log(result.d);
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
                console.log(result.d);
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
    var gglData = dataArray(json);

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
                var gglData = dataArray(result.d);
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
                var gglData = dataArray(result.d);
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
                var gglData = dataArray(result.d);
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
                console.log(result.d);
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
                console.log(result.d);
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
                console.log(result.d);
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
                console.log(result.d);
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