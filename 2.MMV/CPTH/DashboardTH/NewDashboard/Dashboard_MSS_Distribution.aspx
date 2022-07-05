<%@ Page Title="" Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" CodeBehind="Dashboard_MSS_Distribution.aspx.cs" Inherits="MMV.CPTH.DashboardTH.NewDashboard.Dashboard_MSS_Distribution" %>

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
        //get filter RE
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
        function clearSession() {
            sessionStorage.removeItem("filter");
            sessionStorage.removeItem("colHeader");
        }
 
       

        function drawRegionChart() {
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
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 10,
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
                        GetdataMSSDistributorChartCPTH(result.d, options, "Region", "Bar");
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


        function drawDistributorChart() {
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
                        fontSize: 10,
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
                            left: 200
                        },
                        colors: ['#0000FF'],
                        legend: { position: 'none' },
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataMSSDistributorChartCPTH(result.d, options, "Distributor", "Bar");
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
                    console.log(stringjob);
                    var data = JSON.stringify(stringjob);
                   
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 10,
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
                    delete stringjob["SALES_NAME"];
                    var data = JSON.stringify(stringjob);
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 10,
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
                    delete stringjob["SALES_NAME"];
                    var data = JSON.stringify(stringjob);
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 10,
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
                   // console.log(stringjob);
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 12
                        },
                        fontName: 'Tahoma',
                        fontSize: 10,
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
        function setLabelTotalPointRL() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getTotalPointRL",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'Point_RL', orderBy: 'Point_RL DESC',table: 'D_MSS_DISTRIBUTION'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var obj = JSON.parse(result.d);
                    if (obj.length <= 0) {
                        document.getElementById('lblTotalPointRL').innerHTML = 0;
                    } else {
                        obj.forEach(function (item) {
                            if (item.Point_RL) {
                                document.getElementById('lblTotalPointRL').innerHTML = formatNumber(item.Point_RL);
                            } else {
                                document.getElementById('lblTotalPointRL').innerHTML = 0;
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
                    
                    if (obj.length <= 0) {
                        document.getElementById('lblTimeGone').innerHTML = "0 %";
                    } else {
                        obj.forEach(function (item) {
                            document.getElementById('lblTimeGone').innerHTML = item.TIME_GONE + "%";

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


        //reset event when click filter
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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
                                    GetdataMSSDistributorChartCPTH(result.d, options, "Region", "Bar");
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
                                    fontSize: 10,
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
                                    GetdataMSSDistributorChartCPTH(result.d, options, "Distributor", "Bar");
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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

                                    document.getElementById('lblTimeGone').innerHTML = item.TIME_GONE +"%";
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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
                                        fontSize: 12
                                    },
                                    fontName: 'Tahoma',
                                    fontSize: 10,
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

        ///jquery
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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
                                GetdataMSSDistributorChartCPTH(result.d, options, "Region", "Bar");
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
                                fontSize: 10,
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
                                GetdataMSSDistributorChartCPTH(result.d, options, "Distributor", "Bar");
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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

                                document.getElementById('lblTimeGone').innerHTML = item.TIME_GONE +"%";
                            });
                        },
                        error: function (result, status, error) {
                            var err = eval("(" + result.responseText + ")");
                            alert(err.Message);
                        },
                        async: false
                    });
                }
                else {
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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
                                    fontSize: 12
                                },
                                fontName: 'Tahoma',
                                fontSize: 10,
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

            })


           



        })


        function fadeOut() {

            $(".loader-wrapper").fadeIn('fast').fadeOut('slow');
        }

    </script>
    <style>
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

        .hidden {
            visibility: hidden;
        }

        .dhb-body2 {
            height: auto;
            width: 97vw;
            margin: 0;
            padding: 0;
            height: 330px;
            padding-bottom: 20px;
            box-sizing: border-box;
        }

        .dhb-body2_header {
            height: auto;
            width: 97vw;
            margin-bottom: 1%;
            padding: 0;
            height: 200px;
            padding-bottom: 20px;
        }

        .div_padding2 {
            width: 100%;
            height: 100%;
            padding: 5px;
            float: left;
            box-sizing: border-box;
        }

        .left {
            float: left;
            width: 49%;
            margin-right: 1%;
        }

        .dhb-body_sub-2_2_2 {
            margin-top: 1%;
            height: 95%;
        }

        .radioWithProperWrap tr {
            float: left;
            margin-right: 30px;
        }

        .table-total th,.td-total:first-child {
            padding: 8px;
            text-align: left;
            border-right: 1px solid #ddd;
        }

        .table-total {
            
            border-radius: 1em;
            overflow: hidden;
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
                        <h4 style="color: #808080">MSS Distribution</h4>
                    </div>

                    <table style="width:49%;" class="table-total box-shadow">

                        <tr style="height: 20px;">
                            <td style="text-align:center;padding: 0; font-weight: bold;width:50%" class="td-total";>
                                <div class="title-chart" style="text-align:left">&nbsp&nbsp MSS Total (M&P)</div>
                                <h4 style="color: #0000FF;"><div id="lblTotalPointRL"></h4>
                                
                                </div>

                            </td>
                            <td style="text-align:center;padding: 0; font-weight: bold;" class="td-total">
                                <div class="title-chart" style="text-align:left">Time Gone</div>
                                <h4 style="color: #808080;"><div id="lblTimeGone">
                                </div></h4>
                                
                            </td>

                        </tr>

                    </table>
                    <div class="dhb-body2_header">
                        <div class="dhb-body_sub-2_2_2 box-shadow left">
                            <div class="div_padding2">
                                <div class="title-chart">
                                    <%=MMV.L5sMaster.L5sLangs["Region"]%>(M&P)
                                    <%--<span style="position: absolute; left: 400px;"><label id="lblTotalPointRL"></label></span>
                                    <span style="position: absolute; left: 600px;"><label id="lblTimeGone"></label></span>--%>
                                </div>
                                <div style="overflow: auto; height: 90%; bottom: 0;">
                                    <div id="Region"></div>
                                </div>

                            </div>
                        </div>

                        <div class="dhb-body_sub-2_2_2 box-shadow left">
                            <div class="div_padding2">
                                <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Distributor"]%>(M&P)</div>
                                <div style="overflow: auto; height: 90%; bottom: 0;">
                                    <div id="Distributor"></div>
                                </div>


                            </div>
                        </div>

                    </div>
                    <div>
                        <asp:RadioButtonList ID="rblMeasurementSystem" runat="server" CssClass="radioWithProperWrap" OnSelectedIndexChanged="rblMeasurementSystem_SelectedIndexChanged">
                            <asp:ListItem   Text="Region" Value="REGION" />
                            <asp:ListItem Text="Distributor" Value="DISTRIBUTOR_NAME" />
                            <asp:ListItem  Selected="True" Text="DSR" Value="SALES_NAME" />
                        </asp:RadioButtonList>
                    </div>
                    <div class="dhb-body2">


                        <div class="dhb-body_sub-2">

                            <div class="dhb-body_sub-2_2_2 box-shadow">
                                <div class="div_padding2">
                                    <div class="title-chart">M&P</div>


                                    <div style="overflow: auto; height: 90%; bottom: 0;">
                                        <div id="MP"></div>
                                    </div>


                                </div>
                            </div>
                        </div>
                        <div class="dhb-body_sub-2">

                            <div class="dhb-body_sub-2_2_2 box-shadow">
                                <div class="div_padding2">
                                    <div class="title-chart">WS</div>
                                    <div style="overflow: auto; height: 90%; bottom: 0;">
                                        <div id="WS"></div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="dhb-body_sub-2">

                            <div class="dhb-body_sub-2_2_2 box-shadow">
                                <div class="div_padding2">
                                    <div class="title-chart">SPM</div>
                                    <div style="overflow: auto; height: 90%; bottom: 0;">
                                        <div id="SPM"></div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="dhb-body_sub-2 ">

                            <div class="dhb-body_sub-2_2_2 box-shadow">
                                <div class="div_padding2">
                                    <div class="title-chart">MNM</div>
                                    <div style="overflow: auto; height: 90%; bottom: 0;">
                                        <div id="MNM"></div>
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
