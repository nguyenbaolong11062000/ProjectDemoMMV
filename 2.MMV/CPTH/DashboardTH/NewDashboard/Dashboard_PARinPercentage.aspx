<%@ Page Title="" Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" CodeBehind="Dashboard_PARinPercentage.aspx.cs" Inherits="MMV.CPTH.DashboardTH.NewDashboard.Dashboard_PARinPercentage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
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
        function fadeOut() {
            $(".loader-wrapper").fadeIn('fast').fadeOut('slow');
        }
        //Time gone
        function setLabelTime_gone() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getParPARinPercentage",
                data: "{ filter: '' , colFilter: 'Time_gone', colSum: 'TRGET', colSum2: 'ACTUAL',month:'MONTH',table: 'D_MTD_PRIMARY_UPDATE'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var obj = JSON.parse(result.d);

                    obj.forEach(function (item) {
                        document.getElementsByClassName('time_gone')[0].innerHTML = item.TIME_GONE + "%";
                        //document.getElementsByClassName('time_gone')[1].innerHTML = item.TIME_GONE + "%";
                        sessionStorage.setItem("Time_gone", item.TIME_GONE);

                    });
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        };
        function drawRegionChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getPARinPercentage",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'REGION', table: 'D_MTD_PRIMARY_UPDATE', table2: 'D_MTD_ACT_TARGET'}",
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
                        fontSize: 12,
                        width: '470',
                        height: (gglData.length + 2) * 55,
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
            });
        }

        function drawDistributorChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getPARinPercentage",
                data: "{ filter: '' , colFilter: 'DISTRIBUTOR_NAME', Cols: 'DISTRIBUTOR_NAME', table: 'D_MTD_PRIMARY_UPDATE', table2: 'D_MTD_ACT_TARGET'}",
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
                        fontSize: 12,
                        width: '470',
                        height: (gglData.length + 2) * 55,
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
                        //series: {
                        //    0: { targetAxisIndex: 0 },
                        //    1: { targetAxisIndex: 1 }
                        //},
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
        }
        function drawDSRChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDSRPARinPercentage",
                data: "{ filter: '' , colFilter: 'DSR', Cols: 'DSR', table: 'D_DISTRIBUTOR_PAR_PRI', table2: 'D_MTD_ACT_TARGET'}",
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
                        fontSize: 12,
                        width: (gglData.length + 2) * 55,
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
        }

        function formatNumber(num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }

        function setLabelParSec() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getParPARinPercentage",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'TARGET', colSum2: 'ACTUAL',month:'MONTH_NUMBER',table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var obj = JSON.parse(result.d);
                    var gglData = dataArrayCPTH(result.d);

                    obj.forEach(function (item) {
                        if (item.Par) {
                            document.getElementById('ach-secondary').innerHTML = item.Par + "%";
                        } else {
                            document.getElementById('ach-secondary').innerHTML = "0%";
                        }


                    });
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        };

        function setLabelParpRI() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getParPARinPercentage",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'TRGET', colSum2: 'ACTUAL',month:'MONTH',table: 'D_MTD_PRIMARY_UPDATE'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var obj = JSON.parse(result.d);

                    obj.forEach(function (item) {
                        if (item.Par) {
                            document.getElementById('ach-primary').innerHTML = item.Par + "%";
                        } else {
                            document.getElementById('ach-primary').innerHTML = "0%";
                        }


                    });
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        };

        function changeColorParSec() {
            var sec, pri;
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getParPARinPercentage",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'TARGET', colSum2: 'ACTUAL',month:'MONTH_NUMBER',table: 'D_MTD_ACT_TARGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var obj = JSON.parse(result.d);
                    obj.forEach(function (item) {
                        sec = item.Par;
                    });
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });

            let time_gone = sessionStorage.getItem("Time_gone");
            let summary = sec - time_gone;
            if (summary <= -10) {
                document.getElementById('ach-secondary').style.color = '#FE0000';
            } else if (summary <= -5) {
                document.getElementById('ach-secondary').style.color = '#FFC000';
            } else if (summary <= 5) {
                document.getElementById('ach-secondary').style.color = '#00B050';
            }
            else {
                document.getElementById('ach-secondary').style.color = '#00FF00';
            }
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

        .dashboard-bot_right {
            padding: 10px;
            width: 10%;
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
                    <div style="width: 98vw; margin: auto; margin-bottom: 10px; padding: 10px; background-color: #808080; text-align: left;">
                        <asp:Button ID="P5sLbtnRemoveSession" Style="width: 10%;" runat="server" CausesValidation="false" Text="Clear Filter" OnClick="P5sLbtnRemoveSession_Click" />
                    </div>
                    <div class="dhb-header">
                        <h4 style="color: #808080">PAR in Percentage</h4>
                        <p id="errors" style="color: red; text-align: left; text-wrap: normal"></p>
                    </div>
                    <div class="dashboard">

                        <div class="dashboard-top-1 box-shadow" style="position: relative; text-align: center;">
                            <div class="title-chart" style="text-align: left;">%Par</div>
                            <div style="position: absolute; left: 50%; top: 50%; padding: 20px; transform: translate(-50%, -50%)">
                                <p style="color: #808080; text-align: center; font-size: 23px">Ex-Dist</p>
                                <div id="ach-secondary" style="color: #16B75F; font-size: 30px; margin-top: 5%px">19.23%</div>
                                <p style="color: #FFD85F; text-align: center; position: absolute; top: 45%; left: 84%; font-size: 21px" class="time_gone">19.20%</p>
                                <p style="position: absolute; font-size: 12px; top: 67%; left: 84%;">Par</p>
                                <%-- <p style="color: #808080; text-align: center">Ex-CP</p>
                                <div id="ach-primary" style="color: #16B75F; font-size: 22px">15.10%</div>
                                <p style="color: #FFD85F; text-align: center; position: absolute; top: 66%; left: 82%; font-size: 11px" class="time_gone">19.20%</p>
                                <p style="position: absolute; font-size: 8px; top: 73%; left: 81%;">Par</p>--%>
                            </div>
                        </div>
                        <div class="dashboard-top-2 box-shadow">
                            <div class="title-chart" style="text-align: left;">Region</div>
                            <div style="overflow-y: scroll; overflow-x: hidden; height: 90%; margin-top: 5px;">
                                <div id="dbRegion"></div>
                            </div>
                        </div>
                        <div class="dashboard-top-3 box-shadow">
                            <div class="title-chart" style="text-align: left;">Distributor</div>
                            <div style="overflow-y: scroll; overflow-x: hidden; height: 90%; margin-top: 5px;">
                                <div id="dbDistributor"></div>
                            </div>
                        </div>
                        <div class="dashboard-bot box-shadow">
                            <div class="title-chart" style="text-align: left;">DSR</div>
                            <div style="overflow-y: hidden; overflow-x: scroll; height: 90%; margin-top: 5px; width: 80%">
                                <div id="dbSales"></div>

                            </div>
                            <div style="line-height: 185%; width: 19%; position: absolute; left: 79%; top: 62%">
                                <p style="text-align: center">&nbsp</p>
                                <p style="border: 2px solid #5A1919; background-color: #FE0000; color: #715500">10% Below PAR : High Risk</p>
                                <p style="text-align: center">&nbsp</p>
                                <p style="border: 2px solid #5A1919; background-color: #FFC000; color: #715500">5% Below PAR : Medium Risk</p>
                                <p style="text-align: center">&nbsp</p>
                                <p style="border: 2px solid #5A1919; background-color: #00B050; color: #80D7A8">-4% - +5%: Good</p>
                                <p style="text-align: center">&nbsp</p>
                                <p style="border: 2px solid #5A1919; background-color: #00FF00; color: #715500">5% Above PAR: excellence</p>
                            </div>


                        </div>

                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
