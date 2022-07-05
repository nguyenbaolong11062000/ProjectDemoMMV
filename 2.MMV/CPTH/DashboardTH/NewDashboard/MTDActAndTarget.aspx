<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MTDActAndTarget.aspx.cs" Inherits="MMV.CPTH.DashboardTH.NewDashboard.MTDActAndTarget" MasterPageFile="~/P5s.Master" %>

<asp:Content ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <link href="/fsmdls/mmcv4/CSS/fscstyles_181103.css" rel="stylesheet" />
    <link href="../CSS/dashboard.css" rel="stylesheet" />
    <script src="/Dashboard/loader.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Dashboard/jquery-3.3.1.min.js"></script>
    <script src="/CPTH/DashboardTH/LoadChartCPTH.js" type="text/javascript"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        $().ready(function () {
            google.charts.setOnLoadCallback(
                function () {
                    $(".loader-wrapper").fadeOut('Slow');
                });
        })
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
                            fontSize: 14
                        },
                        fontName: 'Tahoma',
                        fontSize: 13,
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
                            bottom: 50,
                            right: 120,
                        },
                        series: {
                            0: { targetAxisIndex: 0 },
                            1: { targetAxisIndex: 0 }
                        },
                        colors: ['#4477aa', '#cc6677'],
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetDataMTDandTargetChart(result.d, options, "dbRegion", "Bar");
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
                            fontSize: 14
                        },
                        fontName: 'Tahoma',
                        fontSize: 13,
                        height: (gglData.length + 2) * 85,
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
                            left: 120,
                            right: 120,
                            bottom: 50
                        },
                        series: {
                            0: { targetAxisIndex: 0 },
                            1: { targetAxisIndex: 0 }
                        },
                        colors: ['#4477aa', '#cc6677'],
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetDataMTDandTargetChart(result.d, options, "dbDistributor", "Bar");
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
                            fontSize: 14
                        },
                        fontName: 'Tahoma',
                        fontSize: 13,
                        width: (gglData.length + 2) * 120,
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
                            right: 120,
                            bottom: 50
                        },
                        series: {
                            0: { targetAxisIndex: 0 },
                            1: { targetAxisIndex: 0 }
                        },
                        colors: ['#4477aa', '#cc6677'],
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetDataMTDandTargetChart(result.d, options, "dbSales", "Column");
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

        function formatNumber(num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
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
                            0: {},
                            1: { color: '#F0F5F7' }
                        },
                        pieHole: 0.8,
                        pieStartAngle: 0,
                        pieSliceText: 'none',
                        legend: { position: 'none' },
                        tooltip: { trigger: 'selection' },
                        tooltip: { textStyle: { fontSize: 12 } },
                        chartArea: {
                            top: 20,
                            bottom: 0,
                        },
                        colors: ['#4477aa'],
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetDataMTDandTargetChart(result.d, options, "dbActual", "Pie");
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
    </script>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        .dashboard {
            width: 98vw;
            height: 90vh;
            margin: auto;
        }

        .dashboard-top-1 {
            padding: 10px;
            width: 24%;
            float: left;
            height: 48%;
            margin: .5%;
            text-align: center;
        }

        .dashboard-top-2 {
            padding: 10px;
            width: 37%;
            float: left;
            margin: .5%;
            height: 48%;
            text-align: center;
        }

        .dashboard-top-3 {
            padding: 10px;
            width: 36%;
            float: left;
            margin: .5%;
            height: 48%;
            text-align: center;
        }

        .dashboard-bot {
            padding: 10px;
            width: 99%;
            float: left;
            height: 49%;
            margin: .5%;
            text-align: center;
        }

        #dbQuarter {
            width: 100%;
            height: 60%;
        }

        #pcQuarter {
            width: 100%;
            height: 25%;
            padding-top: 30px;
        }

        #dbDistributor {
            width: 95%;
        }

        #dbSales {
            height: 90%;
        }

        #dbRegion {
            width: 95%;
        }

        #dbActual {
            width: 90%;
            height: 90%;
        }

        table {
            width: 100%;
        }

            table tr td {
                text-align: center;
            }

        .value-chart {
            font-family: Arial,sans-serif;
            color: #777;
            margin: 10px;
        }

        .text-value {
            font-family: Arial sans-serif;
            color: #777;
        }

        .text-value--main {
            font-size:14px;
        }

        .text-value--sub {
            margin-top: 5px;
            font-weight: bold;
        }
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
                    <div style="width: 99vw; margin: auto; margin-bottom: 10px; padding: 10px; background-color: #808080; text-align: left">
                        <asp:Button ID="P5sLbtnRemoveSession" Style="width: 10%;" runat="server" CausesValidation="false" Text="Clear Filter" OnClientClick="return SaveWithParameter();" OnClick="P5sLbtnRemoveSession_Click" />
                    </div>
                    <div class="dhb-header">
                        <h4 style="color: #808080">MTD Act vs Target</h4>
                        <p id="errors" style="color: red; text-align: left; text-wrap: normal"></p>
                    </div>
                    <div class="dashboard">
                        <div class="dashboard-top-1 box-shadow" style="position: relative; text-align: center;">
                            <div id="dbActual" style="margin: auto; text-align: center"></div>
                            <div class="text-value" style="position: absolute; margin: auto; left: 0; right: 0; top: 45%;">
                                <p class="text-value--main" id="Actual-value">10000</p>
                                <p class="text-value--sub">MTD Actual</p>
                            </div>
                            <div class="text-value" style="position: center !important; margin-top: 10px;">
                                <p class="text-value--main" id="Target-value" title="Target">0</p>
                            </div>
                        </div>
                        <div class="dashboard-top-2 box-shadow">
                            <div class="title-chart" style="text-align: left;"><%= MMV.L5sMaster.L5sLangs["Region"] %></div>
                            <div style="overflow-y: scroll; overflow-x: hidden; height: 90%; margin-top: 5px;">
                                <div id="dbRegion"></div>
                            </div>
                        </div>
                        <div class="dashboard-top-3 box-shadow">
                            <div class="title-chart" style="text-align: left;"><%= MMV.L5sMaster.L5sLangs["Distributor"] %></div>
                            <div style="overflow-y: scroll; overflow-x: auto; height: 90%; margin-top: 5px; text-align: center">
                                <div id="dbDistributor"></div>
                            </div>
                        </div>
                        <div class="dashboard-bot box-shadow">
                            <div class="title-chart" style="text-align: left;">MTD Act vs Sales target</div>
                            <div style="overflow-y: hidden; overflow-x: scroll; height: 90%; margin-top: 5px;">
                                <div id="dbSales"></div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
