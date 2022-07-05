<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributorDailySummary.aspx.cs" Inherits="MMV.CPTH.DashboardTH.NewDashboard.DistributorDailySummary" MasterPageFile="~/P5s.Master" %>


<asp:Content runat="server" ContentPlaceHolderID="BodyContentPlaceHolder">
    <link href="/fsmdls/mmcv4/CSS/fscstyles_181103.css" rel="stylesheet" />
    <link href="../CSS/dashboard.css" rel="stylesheet" />
    <script src="/Dashboard/loader.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Dashboard/jquery-3.3.1.min.js"></script>
    <script src="/CPTH/DashboardTH/LoadChartV2.js" type="text/javascript"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        $().ready(function () {
            google.charts.setOnLoadCallback(
                function () {
                    $(".loader-wrapper").fadeOut('Slow');
                });
        })

        function drawYearChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharV3",
                data: "{ filter: '' , colFilter: '', Cols: 'YEAR', colSum: 'ACTUAL', orderBy: 'YEAR ASC', table: 'D_MTD_ACT_TARGET'}",
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
                            bottom: 20,
                            width: '80%',
                            height: '100%',
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
        }

        function drawQuarterChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharV3",
                data: "{ filter: '' , colFilter: '', Cols: 'QUARTER', colSum: 'ACTUAL', orderBy: 'QUARTER ASC', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result, status, xhr) {
                    var gglData = dataArrayCPTH(result.d);
                    var options = {
                        height: '80%',
                        width: '100%',
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
                        bar: { groupWidth: "80%" },
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
        }

        function drawMonthChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharV3",
                data: "{ filter: '' , colFilter: '', Cols: 'MONTH', colSum: 'ACTUAL', orderBy: 'MONTH_NUMBER ASC', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result, status, xhr) {
                    var gglData = dataArrayCPTH(result.d);
                    var options = {
                        height: '100%',
                        width: '80%',
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
                        bar: { groupWidth: "80%" },
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
        }

        function drawMTDActualChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharV4",
                data: "{ filter: '' , colFilter: '', Cols: 'MTD', colSum: 'ACTUAL', orderBy: '', table: 'D_MTD_ACT_TARGET'}",
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
                            top: 10,
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
        };

        function setLabelTarget() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getTarget",
                data: "{ filter: '' , colFilter: '', Cols: 'MTD', colSum: 'ACTUAL', orderBy: '', table: 'D_MTD_ACT_TARGET'}",
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
        }

        function drawRegionChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharV4",
                data: "{ filter: '' , colFilter: '', Cols: 'REGION', colSum: 'ACTUAL', orderBy: 'REGION', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result, status, xhr) {
                    var gglData = dataArrayCPTH(result.d);
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
                            top: 10,
                            left: 100,
                            right: 100,
                        },
                        legend: { position: 'right' },
                        bar: { groupWidth: "90%" },
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
            });
        }

        function drawDistributorChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharV4",
                data: "{ filter: '' , colFilter: '', Cols: 'DISTRIBUTOR', colSum: 'ACTUAL', orderBy: 'DISTRIBUTOR', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result, status, xhr) {
                    var gglData = dataArrayCPTH(result.d);
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
                            top: 10,
                            left: 100,
                            right: 100,
                        },
                        legend: { position: 'right' },
                        bar: { groupWidth: "90%" },
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
        }

        function drawDSRChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharV4",
                data: "{ filter: '' , colFilter: '', Cols: 'DSR', colSum: 'ACTUAL', orderBy: 'DSR_CODE', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result, status, xhr) {
                    var gglData = dataArrayCPTH(result.d);
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 13
                        },
                        fontName: 'Tahoma',
                        fontSize: 13,
                        width: (gglData.length + 2) * 100,
                        height: '80%',
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
                            left: 100,
                            right: 100,
                            bottom: 40,
                        },
                        legend: { position: 'right' },
                        bar: { groupWidth: "70%" },
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
            });
        }

        function drawBuyingChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDonutChart",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'Buying', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
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
        };

        function drawStoreVisitedChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDonutChart",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'STORE_VISIT', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
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
        };

        function setLabelStoreVisited() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getECC_StoreVisited_CoverageBuyingStoreVisit",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'STORE_VISIT', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
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
        };

        function setLabelECC() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getECC_StoreVisited_CoverageBuyingStoreVisit",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'BUYING', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
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
        };

        function drawRegionChartCoverage() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getCoverageBuyingStoreVisit",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'REGION', colSum: 'COVERAGE',colSum2:'BUYING',colSum3:'STORE_VISIT', orderBy: 'COVERAGE DESC', table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
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
        }

        function drawDistributorChartCoverage() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getCoverageBuyingStoreVisit",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'DISTRIBUTOR_NAME', colSum: 'COVERAGE',colSum2:'BUYING',colSum3:'STORE_VISIT', orderBy: 'COVERAGE DESC', table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
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
                            top: 10,
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
        }

        function drawDSRChartCoverage() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getCoverageBuyingStoreVisit",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'SALES_NAME', colSum: 'COVERAGE',colSum2:'BUYING',colSum3:'STORE_VISIT', orderBy: 'COVERAGE DESC', table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
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
        }
        //D_MSS_DISTRIBUTION
        function clearSession() {
            sessionStorage.removeItem("filter");
            sessionStorage.removeItem("colHeader");
        }
        function drawRegionChartMSS() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharCPTH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'REGION', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION'}",
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
                            top: 20,
                            left: 100,
                        },
                        colors: ['#0000FF'],
                        legend: { position: 'none' },
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
        }

        function drawDistributorChartMSS() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharCPTH",
                data: "{ filter: '' , colFilter: 'DISTRIBUTOR_NAME', Cols: 'DISTRIBUTOR_NAME', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION'}",
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
                            left: 200,
                            bottom: 30,
                        },
                        colors: ['#0000FF'],
                        legend: { position: 'none' },
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
        }

        function setLabelTotalPointRL() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getTotalPointRL",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'Point_RL', orderBy: 'Point_RL DESC',table: 'D_MSS_DISTRIBUTION'}",
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
        };
        function setLabelTimeGone() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getTimeGone",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'TIME_GONE', orderBy: 'TIME_GONE DESC',table: 'D_MSS_DISTRIBUTION'}",
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
        };

        var filterRE = "";

        function getFilterRe() {
            let list = document.getElementById('<%=rblMeasurementSystem.ClientID%>');
            let inputs = list.getElementsByTagName("input");
            let filter;
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    filter = inputs[i];
                    break;
                }
            }
            sessionStorage.setItem("filterRE", filter.value);
            filterRE = sessionStorage.getItem("filterRE");

        }
        function filterPointRL() {
            $(() => {
                //event change select in radio
                $('#<%=rblMeasurementSystem.ClientID%>').change(() => {
                    let filter = sessionStorage.getItem("filter");
                    let colHeader = sessionStorage.getItem("colHeader");
                    let list = document.getElementById('<%=rblMeasurementSystem.ClientID%>');
                    let inputs = list.getElementsByTagName("input");
                    let filter1;
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].checked) {
                            filter1 = inputs[i];
                            break;
                        }
                    }
                    sessionStorage.setItem("filterRE", filter1.value);
                    filterRE = sessionStorage.getItem("filterRE");
                    if ((filter != null && filter != "") || (colHeader != null && colHeader != "")) {
                        //alert("filter");
                        //alert(filter);
                        //alert(colHeader);
                        //alert(filterRE)
                        //region
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
                                    height: gglData.length * 55,
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
                                    legend: { position: 'none' },
                                };

                                if (Array.isArray(gglData) && gglData.length) {
                                    GetdataMSSDistributorChartCPTH(result.d, options, "RegionMSS", "Bar");
                                } else {
                                    let erros = "No data Region";
                                    document.getElementById("RegionMSS").innerHTML = "<span style='color:red'> " + erros + "</span>";
                                }
                            },
                            error: function (result, status, error) {
                                alert("a");
                            },
                            async: false
                        });
                        //distributor
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
                                    height: gglData.length * 55,
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
                                    legend: { position: 'none' },
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
                                    height: gglData.length * 55,
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
                                    legend: { position: 'none' },
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
                                    height: gglData.length * 55,
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
                                        // width: '50%',
                                        top: 20,
                                        //left: 200,
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
                                    height: gglData.length * 55,
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
                                    document.getElementById('lblTotalPointRL').innerHTML = formatNumber(item.Point_RL);
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
                                console.log(result.d)
                                var obj = JSON.parse(result.d);
                                obj.forEach(function (item) {

                                    document.getElementById('lblTimeGone').innerHTML = item.TIME_GONE + "%";
                                });
                            },
                            error: function (result, status, error) {
                                var err = eval("(" + result.responseText + ")");
                                alert(err.Message);
                            },
                            async: false
                        });
                    } else {
                        //alert("normal");
                        //alert(filter);
                        //alert(colHeader);
                        //alert(filterRE)
                        let list = document.getElementById('<%=rblMeasurementSystem.ClientID%>');
                        let inputs = list.getElementsByTagName("input");
                        let filter;
                        for (var i = 0; i < inputs.length; i++) {
                            if (inputs[i].checked) {
                                filter = inputs[i];
                                break;
                            }
                        }
                        sessionStorage.setItem("filterRE", filter.value);
                        filterRE = sessionStorage.getItem("filterRE");
                        //MP
                        $.ajax({
                            type: "POST",
                            url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
                            data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'503'}",
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
                                    height: gglData.length * 55,
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
                                    legend: { position: 'none' },
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
                            data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'602'}",
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
                                    height: gglData.length * 55,
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
                            data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'502'}",
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
                                    height: gglData.length * 55,
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
                            data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'504'}",
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
                                    height: gglData.length * 55,
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
                    }

                })//end event change of radio

            })
        }

        function drawMPChart() {

            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'503'}",
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
                        legend: { position: 'none' },
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
        }

        function drawWSChart() {

            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'602'}",
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
        }

        function drawSPMChart() {

            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'502'}",
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
        }

        function drawMNMChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'504'}",
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
        }

        $(() => {
            //event change select in radio

            $('#<%=rblMeasurementSystem.ClientID%>').change(() => {
                let filter = sessionStorage.getItem("filter");
                let colHeader = sessionStorage.getItem("colHeader");
                let list = document.getElementById('<%=rblMeasurementSystem.ClientID%>');
                let inputs = list.getElementsByTagName("input");
                let filter1;
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        filter1 = inputs[i];
                        break;
                    }
                }
                sessionStorage.setItem("filterRE", filter1.value);
                filterRE = sessionStorage.getItem("filterRE");
                if ((filter != null && filter != "") || (colHeader != null && colHeader != "")) {
                    //alert("filter");
                    //alert(filter);
                    //alert(colHeader);
                    //alert(filterRE)
                    //region
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
                                legend: { position: 'none' },
                            };

                            if (Array.isArray(gglData) && gglData.length) {
                                GetdataMSSDistributorChartCPTH(result.d, options, "RegionMSS", "Bar");
                            } else {
                                let erros = "No data Region";
                                document.getElementById("RegionMSS").innerHTML = "<span style='color:red'> " + erros + "</span>";
                            }
                        },
                        error: function (result, status, error) {
                            alert("a");
                        },
                        async: false
                    });
                    //distributor
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
                                legend: { position: 'none' },
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
                                legend: { position: 'none' },
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
                                    // width: '50%',
                                    top: 20,
                                    //left: 200,
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
                            console.log(result.d)
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
                } else {
                    //alert("normal");
                    //alert(filter);
                    //alert(colHeader);
                    //alert(filterRE)
                    let list = document.getElementById('<%=rblMeasurementSystem.ClientID%>');
                    let inputs = list.getElementsByTagName("input");
                    let filter;
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].checked) {
                            filter = inputs[i];
                            break;
                        }
                    }
                    sessionStorage.setItem("filterRE", filter.value);
                    filterRE = sessionStorage.getItem("filterRE");
                    //MP
                    $.ajax({
                        type: "POST",
                        url: "/Dashboard/ChartService.asmx/OnClickCharRECPTH",
                        data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'503'}",
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
                                legend: { position: 'none' },
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
                        data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'602'}",
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
                        data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'502'}",
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
                        data: "{ filter: '' , colFilter: 'REGION', Cols: '" + filterRE + "', colSum: 'Point_RL', orderBy: 'Point_RL DESC', table: 'D_MSS_DISTRIBUTION',re_code:'504'}",
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
                }

            })
        })
    </script>
    <style>
        * {
            box-sizing: border-box;
        }

        .text-value {
            font-family: Arial sans-serif;
            color: #777;
        }

        .text-value--main {
        }

        .text-value--sub {
            margin-top: 5px;
            font-weight: bold;
        }

        .centerLabel {
            position: absolute;
            left: -30px;
            top: 2px;
            width: 416px;
            line-height: 149px;
            text-align: center;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: maroon;
        }

        .textZero {
            position: absolute;
            left: -35px;
            top: 2px;
            width: 309px;
            line-height: 245px;
            text-align: center;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: maroon;
        }

        .coverage {
            position: absolute;
            left: -30px;
            top: 2px;
            width: 416px;
            line-height: 302px;
            text-align: center;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: maroon;
        }

        .dhb-body2 {
            height: 50%;
            width: 100%;
            margin: 0;
            padding: 0;
            padding-bottom: 20px;
            box-sizing: border-box;
        }

        .dhb-body2_header {
            height: auto;
            width: 100%;
            padding: 0;
            height: 49%;
        }

        .div_padding2 {
            width: 100%;
            height: 100%;
            padding: 5px;
            float: left;
            box-sizing: border-box;
        }

        .dhb-body_sub-2_2_2 {
            height: 95%;
        }

        .radioWithProperWrap tr {
            float: left;
            margin-right: 30px;
        }

        .table-total th, .td-total {
            padding: 8px;
            text-align: left;
        }

        .table-total {
            border-radius: 1em;
            overflow: hidden;
        }

        .left {
            float: left;
            width: 40%;
            margin-right: 1%;
        }
    </style>
    </style>
    <form runat="server" id="frmMain">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPnlMain" runat="server">
                    <div class="loader-wrapper">
                        <img src="../images/loading.gif" alt="" width="150px" />
                    </div>
                    <div style="width: 100%; margin-bottom: 10px; padding: 10px; background-color: #808080; text-align: left;">
                        <asp:Button ID="P5sLbtnRemoveSession" Style="width: 10%;" runat="server" CausesValidation="false" Text="Clear Filter" OnClientClick="return SaveWithParameter();" OnClick="P5sLbtnRemoveSession_Click" />
                    </div>
                    <div class="dhb-header">
                        <h4 style="color: #808080">Distributor Daily Summary</h4>
                        <p id="errors" style="color: red; text-align: left; text-wrap: normal"></p>
                    </div>
                    <div class="dhb-body" style="margin: auto !important; max-height: 600px; width: 2500px !important;">
                        <div class="box-shadow" style="width: 49%; height: 100%; float: left; margin: .5%; padding: 10px;">
                            <div style="width: 30%; height: 50%; float: left;">
                                <div class="title-chart" style="text-align: left;"><%=MMV.L5sMaster.L5sLangs["Year"]%></div>
                                <div id="dbYear"></div>
                            </div>
                            <div style="width: 70%; height: 50%; float: left;">
                                <div class="title-chart" style="text-align: left;">Quarter</div>
                                <div id="dbQuarter"></div>
                            </div>
                            <div style="width: 100%; height: 50%; float: left;">
                                <div class="title-chart" style="text-align: left;">Month</div>
                                <div id="dbMonth"></div>
                            </div>
                        </div>
                        <div class="box-shadow" style="width: 49%; height: 100%; float: left; margin: .5%; padding: 10px;">
                            <div style="width: 27%; height: 50%; float: left; position: relative; text-align: center;">
                                <div class="title-chart" style="text-align: left;">MTD Actual</div>
                                <div id="dbActual" style="margin: auto; text-align: center"></div>
                                <div class="text-value" style="position: absolute; margin: auto; left: 0; right: 0; top: 30%;">
                                    <p class="text-value--main" id="Actual-value">10000</p>
                                    <p class="text-value--sub">MTD Actual</p>
                                </div>
                                <div class="text-value" style="position: center !important; margin-top: 10px;">
                                    <p class="text-value--main" id="Target-value" title="Target">0</p>
                                </div>
                            </div>
                            <div style="width: 36%; height: 50%; float: left;">
                                <div class="title-chart" style="text-align: left;"><%=MMV.L5sMaster.L5sLangs["Region"]%></div>
                                <div style="overflow-y: scroll; overflow-x: hidden; height: 90%; margin-top: 5px;">
                                    <div id="dbRegion"></div>
                                </div>
                            </div>
                            <div style="width: 36%; height: 50%; float: left; margin-left: 1%;">
                                <div class="title-chart" style="text-align: left;"><%=MMV.L5sMaster.L5sLangs["Distributor"]%>(M&P)</div>
                                <div style="overflow-y: scroll; overflow-x: hidden; height: 90%; margin-top: 5px;">
                                    <div id="dbDistributor"></div>
                                </div>
                            </div>
                            <div style="width: 100%; height: 50%; float: left; padding-top: 10px;">
                                <div class="title-chart" style="text-align: left;">MTD Act vs Sales target</div>
                                <div style="overflow-y: hidden; overflow-x: scroll; height: 90%; margin-top: 5px;">
                                    <div id="dbSales"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="dhb-body" style="margin: auto !important; max-height: 600px; width: 2500px !important;">
                        <div class="box-shadow" style="width: 49%; height: 100%; float: left; margin: .5%; padding: 1rem;">
                            <div style="width: 25%; height: 100%; float: left;">
                                <div style="height: 200px; position: relative">
                                    <div style="bottom: 0;">
                                        <div id="Buying" style="width: 345px; margin: auto"></div>
                                        <div class="centerLabel" id="Buying_data"></div>
                                        <span class="centerLabel" style="margin-top: 15px;">Buying Customer</span>
                                        <span class="coverage data_coverage"></span>
                                      
                                    </div>
                                </div>
                                <div style="height: 200px; position: relative">
                                    <div style="bottom: 0;">
                                        <div id="StoreVisited" style="width: 345px; margin: auto;"></div>
                                        <div class="centerLabel" id="store_visited_data"></div>
                                        <div class="centerLabel" style="margin-top: 15px;">Store visited</div>
                                        <span class="coverage data_coverage"></span>
                                       
                                    </div>
                                </div>
                                <div style="height: 200px;">
                                    <div style="text-align: center">
                                        <p>%Store visited compliance </p>
                                        <div id="lblStoreVisited" style="color: #808080; font-size: 22px"></div>
                                    </div>
                                    <div style="text-align: center">
                                        <p>%ECC </p>
                                        <div id="lblECC" style="color: #808080; font-size: 22px"></div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 37%; height: 100%; float: left;">
                                <div style="width: 100%; height: 50%;">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Region"]%></div>
                                    <div style="overflow-x: hidden; overflow-y: auto; height: 90%; bottom: 0;">
                                        <div id="Region"></div>
                                    </div>
                                </div>
                                <div style="width: 100%; height: 50%; position: relative;">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Distributor"]%></div>
                                    <div style="overflow-x: hidden; overflow-y: auto; height: 95%;">
                                        <div id="Distributor"></div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 38%; height: 100%; float: left;">
                                <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["DSR"]%></div>
                                <div style="overflow-x: hidden; overflow-y: auto; height: 95%; bottom: 0;">
                                    <div id="DSR"></div>
                                </div>
                            </div>
                        </div>
                        <div class="box-shadow" style="width: 49%; height: 100%; float: left; margin: .5%; padding: 10px;">
                            <div class="dhb-body2_header">
                                <div style="float: left; width: 18%; height: 95%;">
                                    <h4 class="title-chart" style="margin: 0;">MSS Distribution
                                    </h4>
                                    <table style="width: 90%; margin: auto; margin-top: 20px; border: 2px solid #808080" class="table-total">
                                        <tr>
                                            <td style="text-align: center; border-bottom: 2px dashed #808080">
                                                <h4 style="color: #808080;">
                                                    <div id="lblTotalPointRL"></div>
                                                </h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <h4 style="color: #808080;">
                                                    <div id="lblTimeGone" style="display: inline"></div>
                                                    <div style="display: inline">%</div>
                                                </h4>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="dhb-body_sub-2_2_2 left">
                                    <div>
                                        <div class="title-chart">
                                            <%=MMV.L5sMaster.L5sLangs["Region"]%>(M&P)
                                        </div>
                                        <div style="overflow: auto; height: 90%; bottom: 0;">
                                            <div id="RegionMSS"></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="dhb-body_sub-2_2_2 left">
                                    <div style="height: 100%">
                                        <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Distributor"]%>(M&P)</div>
                                        <div style="overflow-x: hidden; overflow-y: auto; height: 100%; bottom: 0;">
                                            <div id="DistributorMSS"></div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div>
                                <asp:RadioButtonList ID="rblMeasurementSystem" Style="margin-top: 5px;" runat="server" CssClass="radioWithProperWrap" OnSelectedIndexChanged="rblMeasurementSystem_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Region" Value="REGION" />
                                    <asp:ListItem Text="Distributor" Value="DISTRIBUTOR_NAME" />
                                    <asp:ListItem Text="DSR" Value="SALES_NAME" />
                                </asp:RadioButtonList>
                            </div>
                            <div class="dhb-body2">
                                <div class="dhb-body_sub-2">
                                    <div class="dhb-body_sub-2_2_2">
                                        <div class="div_padding2">
                                            <div class="title-chart">M&P</div>
                                            <div style="overflow-x: hidden; overflow-y: auto; height: 90%; bottom: 0;">
                                                <div id="MP"></div>
                                            </div>


                                        </div>
                                    </div>
                                </div>
                                <div class="dhb-body_sub-2">
                                    <div class="dhb-body_sub-2_2_2">
                                        <div class="div_padding2">
                                            <div class="title-chart">WS</div>
                                            <div style="overflow-x: hidden; overflow-y: auto; height: 90%; bottom: 0;">
                                                <div id="WS"></div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="dhb-body_sub-2">
                                    <div class="dhb-body_sub-2_2_2 ">
                                        <div class="div_padding2">
                                            <div class="title-chart">SPM</div>
                                            <div style="overflow-x: hidden; overflow-y: auto; height: 90%; bottom: 0;">
                                                <div id="SPM"></div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="dhb-body_sub-2 ">

                                    <div class="dhb-body_sub-2_2_2 ">
                                        <div class="div_padding2">
                                            <div class="title-chart">MNM</div>
                                            <div style="overflow-x: hidden; overflow-y: auto; height: 90%; bottom: 0;">
                                                <div id="MNM"></div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
