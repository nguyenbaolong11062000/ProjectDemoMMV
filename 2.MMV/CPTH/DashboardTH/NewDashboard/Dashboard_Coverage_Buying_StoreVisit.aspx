<%@ Page Title="" Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" CodeBehind="Dashboard_Coverage_Buying_StoreVisit.aspx.cs" Inherits="MMV.CPTH.DashboardTH.NewDashboard.Dashboard_Coverage_Buying_StoreVisit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <link href="/fsmdls/mmcv4/CSS/fscstyles_181103.css" rel="stylesheet" />
    <link href="../CSS/dashboard.css" rel="stylesheet" />
    <script src="/Dashboard/loader.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Dashboard/jquery-3.3.1.min.js"></script>
    <script src="/CPTH/DashboardTH/LoadChartCPTH.js" type="text/javascript"></script>
    <script type="text/javascript">

        google.load("visualization", "1", { packages: ["corechart"] });
        // Set a callback to run when the Google Visualization API is loaded.
        var erros = "No data.";
        var filterRE = "";
        $().ready(function () {
            google.charts.setOnLoadCallback(
                function () {
                    $(".loader-wrapper").fadeOut('Slow');
                });
        
        })
        function fadeOut() {

            $(".loader-wrapper").fadeIn('fast').fadeOut('slow');
        }
        function formatNumber(num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }

        //REGION
        function drawRegionChart() {
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
                            fontSize: 12
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
                        hAxis: {
                            scaleType: 'log'
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
                        let erros = "No data Region";
                        document.getElementById("Region").innerHTML = "<span style='color:red'> " + erros + "</span>";
                    }
                },
                error: function (result, status, error) {
                    alert("a");
                },
                async: false
            });
        }
        //Distributor
        function drawDistributorChart() {
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
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 13,
                        width: '60%',
                        height: (gglData.length+2) * 70,
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
                        hAxis: {
                            scaleType: 'log'
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
                        let erros = "No data Distributor";
                        document.getElementById("Distributor").innerHTML = "<span style='color:red'> " + erros + "</span>";
                    }
                },
                error: function (result, status, error) {
                    alert("a");
                },
                async: false
            });
        }
        //DSR
        function drawDSRChart() {
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
                            fontSize: 12
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
                        hAxis: {
                            scaleType: 'log'
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
                        let erros = "No data DSR";
                        document.getElementById("DSR").innerHTML = "<span style='color:red'> " + erros + "</span>";
                    }
                },
                error: function (result, status, error) {
                    alert("a");
                },
                async: false
            });
        }
        function clearFilter() {
            //Region
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getCoverageBuyingStoreVisit",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'REGION', colSum: 'COVERAGE',colSum2:'BUYING',colSum3:'STORE_VISIT', orderBy: 'COVERAGE DESC', table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var gglData = dataArrayCPTH(result.d);
                    console.log(gglData);
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 13,
                        width: '60%',
                        height: gglData.length * 45,
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
                        hAxis: {
                            scaleType: 'log'
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
            //distributor
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
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 13,
                        width: '60%',
                        height: gglData.length * 45,
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
                        hAxis: {
                            scaleType: 'log'
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
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'SALES_NAME', colSum: 'COVERAGE',colSum2:'BUYING',colSum3:'STORE_VISIT', orderBy: 'COVERAGE DESC', table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var gglData = dataArrayCPTH(result.d);
                    console.log(gglData);
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 13,
                        width: '60%',
                        height: gglData.length * 45,
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
                        hAxis: {
                            scaleType: 'log'
                        },
                        colors: ['#DB9F81', '#00B0F0', '#92D050'],
                        legend: { position: 'right' },
                        bar: { groupWidth: "95%" },
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
        function setLabelStoreVisited() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getECC_StoreVisited_CoverageBuyingStoreVisit",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'STORE_VISIT', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var obj = JSON.parse(result.d);
                    if (obj.length <= 0) {
                        document.getElementById('lblStoreVisited').innerHTML =  "0%";
                    } else {
                        obj.forEach(function (item) {
                            if (item.Store_visited) {
                                document.getElementById('lblStoreVisited').innerHTML = item.Store_visited + "%";
                            } else {
                                document.getElementById('lblStoreVisited').innerHTML =  "0%";
                            }
                           
                        });
                    }
                    
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
                    if (obj.length <= 0) {
                        document.getElementById('lblECC').innerHTML ="0%";
                    } else {
                        obj.forEach(function (item) {
                            if (item.Store_visited) {
                                document.getElementById('lblECC').innerHTML = item.Store_visited + "%";
                            } else {
                                document.getElementById('lblECC').innerHTML ="0%";
                            }

                        });
                    }
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        };

        function drawBuyingChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDonutChart",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'Buying', colSum2: 'COVERAGE',orderBy: 'Store_visited DESC',table: 'D_COVERAGE_BUYING_STORE_VISIT'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var gglData = dataArrayCPTH(result.d);
                    console.log(result.d)
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
                    //check values null or empty
                    if (arrayItem[0]) {
                        document.getElementById('Buying_data').innerHTML = formatNumber(arrayItem[0]) + " (" + percentage + "%)";
                    } else {
                        document.getElementById('Buying_Customer').style.display = "none";
                        let erros = "No data Buying_Customer";
                        document.getElementById("Buying_Customer_errors").innerHTML = "<span style='color:red'> " + erros + "</span>";

                    }
                    if (arrayRL[0]) {
                        document.getElementsByClassName('data_coverage')[0].innerHTML = formatNumber(arrayRL[0]);
                    } else {
                        document.getElementById('Buying_Customer').style.display = "none";
                        let erros = "No data Buying_Customer";
                        document.getElementById("Buying_Customer_errors").innerHTML = "<span style='color:red'> " + erros + "</span>";
                    }
                   
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
                        tooltip: { textStyle: { fontSize: 12 } },
                        chartArea: {
                            width: '50%',
                            top: 20,
                            left: 95,
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
                    //check values null or empty
                    if (arrayItem[0]) {
                        document.getElementById('store_visited_data').innerHTML = formatNumber(arrayItem[0]) + " (" + percentage + "%)";
                    } else {
                        document.getElementById('Store_visited').style.display = "none";
                        let erros = "No data Store_visited";
                        document.getElementById("Store_visited_erros").innerHTML = "<span style='color:red'> " + erros + "</span>";

                    }
                    if (arrayRL[0]) {
                        document.getElementsByClassName('data_coverage')[1].innerHTML = formatNumber(arrayRL[0]);
                    } else {
                        document.getElementById('Store_visited').style.display = "none";
                        let erros = "No data Store_visited";
                        document.getElementById("Store_visited_erros").innerHTML = "<span style='color:red'> " + erros + "</span>";
                       
                    }
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
                        tooltip: { textStyle: { fontSize: 12 } },
                        chartArea: {
                            width: '50%',
                            top: 20,
                            left: 95,
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
        

    </script>
    <style>
        * {
            box-sizing: border-box;
        }

        option {
            padding: 10px;
            font-size: 14px;
        }

        #Province {
            overflow-x: hidden;
            overflow-y: scroll;
            width: 95%;
            height: 80%;
        }

        #CDS {
            overflow: auto;
            width: 90%;
        }

        #Region {
            width: 95%;
            height: 80%;
        }

        .dhb-body_sub-3 {
            width: 32%;
            height: 100%;
            margin-right: 1%;
            float: left;
        }

        .dhb-body_sub-3_1 {
            height: 100%;
        }
        .centerLabel
            {
                position: absolute;
                left: 2px;
                top: 2px;
                width: 416px;
                line-height: 149px;
                text-align: center;
                font-family: Arial, Helvetica, sans-serif;
                font-size: 13px;
                color: maroon;
            }
        .textZero{
            position: absolute;
                left: 2px;
                top: 2px;
                width: 309px;
                line-height: 245px;
                text-align: center;
                font-family: Arial, Helvetica, sans-serif;
                font-size: 13px;
                color: maroon;
        }
        .coverage{
                position: absolute;
                left: 2px;
                top: 2px;
                width: 416px;
                line-height: 302px;
                text-align: center;
                font-family: Arial, Helvetica, sans-serif;
                font-size: 13px;
                color: maroon;
        }
    </style>
    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:HiddenField ID="P5sLocaltionCountry" runat="server" />
        <div class="loader-wrapper">
            <img src="../images/loading.gif" alt="" width="150px" />
        </div>
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPnlMain" runat="server">
                    <div style="width: 98%; padding: 10px; background-color: #808080;text-align:left;">
                        <asp:Button ID="P5sLbtnRemoveSession" Style="width: 10%;" runat="server" CausesValidation="false" Text="Clear Filter" OnClick="P5sLbtnRemoveSession_Click" />
                    </div>
                    <div class="dhb-header">
                        <h4 style="color: #808080">Coverage/Buying/Visit</h4>
                    </div>
                    <div class="dhb-body">

                        <div class="dhb-body_sub-3">
                            <div class="dhb-body_sub-3_1 box-shadow">
                                <div class="div_padding">
                                    <div style="height: 200px;position:relative" >
                                        <div style="bottom: 0;" id="Buying_Customer">
                                            <div id="Buying" style="width: 400px; margin-left: 15px"></div>
                                            <div class="centerLabel" id="Buying_data"></div>
                                            <span class="centerLabel" style="margin-top:15px;font-size:11px !important">Buying Customer</span>
                                            <span class="coverage data_coverage"></span>
                                        </div>
                                        <span id="Buying_Customer_errors"></span>
                                    </div>
                                    <div style="height: 200px; position:relative" >

                                        <div style="bottom: 0;"id="Store_visited">
                                            <div id="StoreVisited" style="width: 400px; margin-left: 15px"></div>
                                            <div class="centerLabel" id="store_visited_data"></div>
                                            <div class="centerLabel" style="margin-top:15px;font-size:11px !important">Store visited</div>
                                            <span class="coverage data_coverage"></span>
                                        </div>
                                        <span id="Store_visited_erros"></span>
                                    </div>
                                    <div style="height: 200px;">
                                        <div style="text-align: center">
                                            <p>%<%=MMV.L5sMaster.L5sLangs["Store visited compliance"]%> </p>
                                            <div id="lblStoreVisited" style="color: #808080;font-size:22px;font-weight:bold;"></div>
                                        </div>
                                        <div style="text-align: center">
                                            <p>%ECC </p>
                                            <div id="lblECC"style="color: #808080;font-size:22px;font-weight:bold;">lblECC</div>
                                        </div>


                                    </div>

                                </div>
                            </div>

                        </div>
                        <div class="dhb-body_sub-3">
                            <div class="dhb-body_sub-2_1 box-shadow">
                                <div class="div_padding">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Region"]%></div>
                                    <div style="overflow: auto; height: 90%; bottom: 0;">
                                        <div id="Region"></div>
                                    </div>

                                </div>
                            </div>
                            <div class="dhb-body_sub-2_2 box-shadow">
                                <div class="div_padding">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Distributor"]%></div>
                                    <div style="overflow: auto; height: 90%; bottom: 0;">
                                        <div id="Distributor"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="dhb-body_sub-3">
                            <div class="dhb-body_sub-3_1 box-shadow">
                                <div class="div_padding">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["DSR"]%></div>
                                    <div style="overflow: auto; height: 90%; bottom: 0;">
                                        <div id="DSR"></div>
                                    </div>

                                </div>
                            </div>

                        </div>

                    </div>
                  
                </asp:Panel>
                <asp:HiddenField ID="P5sHfRange" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
