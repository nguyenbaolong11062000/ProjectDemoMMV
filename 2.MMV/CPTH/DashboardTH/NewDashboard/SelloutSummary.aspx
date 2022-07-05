<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelloutSummary.aspx.cs" Inherits="MMV.CPTH.DashboardTH.NewDashboard.SelloutSummary" MasterPageFile="~/P5s.Master" %>

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
                            bottom:50,
                            width: '80%',
                            height: '100%',
                        },
                        series: {
                            0: { targetAxisIndex: 0 },
                            1: { targetAxisIndex: 0 }
                        },
                        hAxis: {
                            scaleType: 'log'
                        },
                        vAxis: {
                            scaleType: 'log'
                        },
                        colors: ['#4477aa','#cc6677'],
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetDataSelloutChart(result.d, options, "dbYear", "Bar");
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
                        width:'100%',
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
                        hAxis: {
                            scaleType: 'log'
                        },
                        vAxis: {
                            scaleType: 'log'
                        },
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetDataSelloutChart(result.d, options, "dbQuarter", "Column");
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
                        width:'80%',
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
                        hAxis: {
                            scaleType: 'log'
                        },
                        vAxis: {
                            scaleType: 'log'
                        },
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetDataSelloutChart(result.d, options, "dbMonth", "Column");
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

        function setLabelQ1() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataQuarter",
                data: "{ filter: 'Q1' , colFilter: 'QUARTER', colSum: 'ACTUAL', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    document.getElementById('lblQ1').innerHTML = result.d;
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        };

        function setLabelQ2() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataQuarter",
                data: "{ filter: 'Q2' , colFilter: 'QUARTER', colSum: 'ACTUAL', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    document.getElementById('lblQ2').innerHTML = result.d;
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        };

        function setLabelQ3() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataQuarter",
                data: "{ filter: 'Q3' , colFilter: 'QUARTER', colSum: 'ACTUAL', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    document.getElementById('lblQ3').innerHTML = result.d;
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        };

        function setLabelQ4() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataQuarter",
                data: "{ filter: 'Q4' , colFilter: 'QUARTER', colSum: 'ACTUAL', table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    document.getElementById('lblQ4').innerHTML = result.d;
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
            width: 34%;
            float: left;
            height: 48%;
            margin: .5%;
            text-align: center;
        }

        .dashboard-top-2 {
            padding: 10px;
            width: 64%;
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

        #dbYear{
            width: 100%;
            height: 80%;
        }

        #dbMonth{
            width: 100%;
            height: 70%;
            margin-top:2%;
        }
        #pcQuarter {
            width: 100%;
            height: 25%;
            padding-top: 30px;
        }

        table {
            width: 100%;
        }

            table tr td {
                text-align: center;
            }

        .value-chart {
            font-family: Arial,sans-serif;
            color: #cc6677;
            margin: 10px;
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
                    <div style="width: 98vw; margin: auto; margin-bottom: 10px; padding: 10px; background-color: #808080; text-align: left;">
                        <asp:Button ID="P5sLbtnRemoveSession" Style="width: 10%;" runat="server" CausesValidation="false" Text="Clear Filter" OnClientClick="return SaveWithParameter();" OnClick="P5sLbtnRemoveSession_Click" />
                    </div>
                    <div class="dhb-header">
                        <h4 style="color: #808080">Sellout Summary</h4>
                        <p id="errors" style="color: red; text-align: left; text-wrap: normal"></p>
                    </div>
                    <div class="dashboard">
                        <div class="dashboard-top-1 box-shadow">
                            <div class="title-chart" style="text-align: left;"><%=MMV.L5sMaster.L5sLangs["Year"]%></div>
                            <div id="dbYear"></div>
                        </div>
                        <div class="dashboard-top-2 box-shadow">
                            <div class="title-chart" style="text-align: left;"><%=MMV.L5sMaster.L5sLangs["Quarter"]%></div>
                            <div id="dbQuarter"></div>
                            <div id="pcQuarter">
                                <table>
                                    <tr>
                                        <td class="title-chart" style="width: 25%">Q1</td>
                                        <td class="title-chart" style="width: 25%">Q2</td>
                                        <td class="title-chart" style="width: 25%">Q3</td>
                                        <td class="title-chart" style="width: 25%">Q4</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="value-chart" id="lblQ1"></div>
                                        </td>
                                        <td>
                                            <div class="value-chart" id="lblQ2"></div>
                                        </td>
                                        <td>
                                            <div class="value-chart" id="lblQ3"></div>
                                        </td>
                                        <td>
                                            <div class="value-chart" id="lblQ4"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="dashboard-bot box-shadow">
                            <div class="title-chart" style="text-align: left;"><%=MMV.L5sMaster.L5sLangs["Month"]%></div>
                            <div id="dbMonth"></div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
