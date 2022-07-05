<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/P5s.Master" CodeBehind="PrimarySalesPerformanceTH.aspx.cs" Inherits="MMV.CPTH.DashboardTH.PrimarySalesPerformanceTH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <link href="/fsmdls/mmcv4/CSS/fscstyles_181103.css" rel="stylesheet" />
    <link href="CSS/dashboard.css" rel="stylesheet" />
    <script src="/Dashboard/loader.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Dashboard/jquery-3.3.1.min.js"></script>
    <script src="/CPTH/DashboardTH/LoadChartCPTH.js" type="text/javascript"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        // Set a callback to run when the Google Visualization API is loaded.
        var erros = "No data.";

        $().ready(function () {
            google.charts.setOnLoadCallback(
                function () {
                    $(".loader-wrapper").fadeOut('Slow');
                });
        })
        function drawRegionChart_TH() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar_PRI_MON_TH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'REGION', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var gglData = dataArrayCPTH(result.d);
                    var options = {
                        //'title': 'Region',
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
                        colors: ['#006266', '#DC3912', '#FF9900', '#9966ff', '#990099', '#1B1464'],
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataSecondaryChartCPTH_PRI_MON(result.d, options, "Region", "Pie");
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

        function drawProvinceChart_TH() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar_PRI_MON_TH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'PROVINCE', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
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
                            top: 20,
                            left: 100,
                        },
                        colors: ['#4075ae'],
                        hAxis: { viewWindow: { min: 0 }},
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataSecondaryChartCPTH_PRI_MON(result.d, options, "Province", "Bar");
                    } else {
                        // erros = erros +"No data Province, ";
                        // document.getElementById("errors").innerText="asdasd";
                    }
                },
                error: function (result, status, error) {
                    alert("a");
                },
                async: false
            });
        }

        //function drawDistributorChart_TH() {
        //    $.ajax({
        //        type: "POST",
        //        url: "/Dashboard/ChartService.asmx/OnClickChar_PRI_TH",
        //        data: "{ filter: '' , colFilter: 'REGION', Cols: 'NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (result) {
        //            var gglData = dataArrayCPTH(result.d);
        //            var options = {
        //                titleTextStyle: {
        //                    color: '#808080',
        //                    fontSize: 16
        //                },
        //                fontName: 'Tahoma',
        //                fontSize: 12,
        //                width: '60%',
        //                height: gglData.length * 45,
        //                animation: {
        //                    duration: 500,
        //                    startup: true //This is the new option
        //                },
        //                legend: { position: 'none' },
        //                annotations: {
        //                    textStyle: {
        //                        fontName: 'Tahoma',
        //                        fontSize: 10,
        //                        color: '#808080'
        //                    },
        //                },
        //                chartArea: {
        //                    width: '50%',
        //                    top: 20
        //                },
        //                colors: ['#4075ae']
        //            };
        //            if (Array.isArray(gglData) && gglData.length) {
        //                GetdataSecondaryChartCPTH_PRI_MON(result.d, options, "Distributor", "Bar");
        //            } else {
        //                //erros = erros + "No data Distributor, ";
        //                // document.getElementById("errors").innerText="asdasd";
        //            }
        //        },
        //        error: function (result, status, error) {
        //            alert("a");
        //        },
        //        async: false
        //    });
        //}

        function drawDistributorChartV2_TH() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickCharV2_PRI_MON_TH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE',ACTUAL: 'ACTUAL',TARGET :'TRGET'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var gglData = dataArrayCPTH(result.d);
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 11
                        },
                        fontName: 'Tahoma',
                        fontSize: 11,
                        width: (gglData.length+1.5)  * 70,
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
                            left: 120,
                            top: 20,
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
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataSecondaryChartCPTH_PRI_MON(result.d, options, "Distributor", "Combo");
                    } else {
                        //erros = erros + "No data Distributor, ";
                        // document.getElementById("errors").innerText="asdasd";
                    }
                },
                error: function (result, status, error) {
                    console.log("fdsafdas:", result);
                    alert("a");
                },
                async: false
            });
        }

        function drawCDSChart_TH() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar_PRI_MON_TH",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'CDS', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var gglData = dataArrayCPTH(result.d);
                    var options = {
                        titleTextStyle: {
                            color: '#808080',
                            fontSize: 14
                        },
                        fontName: 'Tahoma',
                        fontSize: 12,
                        width: '60%',
                        height: (gglData.length + 2) * 50,
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
                            top: 20,
                            left:50,
                        },
                        colors: ['#4075ae'],
                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataSecondaryChartCPTH_PRI_MON(result.d, options, "CDS", "Bar");
                    } else {
                        //erros = erros + "No data CDS, ";
                        // document.getElementById("errors").innerText="asdasd";
                    }
                },
                error: function (result, status, error) {
                    alert("a");
                },
                async: false
            });
        }

        //function drawDSRChart() {
        //    $.ajax({
        //        type: "POST",
        //        url: "/Dashboard/ChartService.asmx/OnClickChar_PRI_TH",
        //        data: "{ filter: '' , colFilter: 'REGION', Cols: 'DSR_NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (result) {
        //            var gglData = dataArrayCPTH(result.d);
        //            var options = {
        //                titleTextStyle: {
        //                    color: '#808080',
        //                    fontSize: 16
        //                },
        //                fontName: 'Tahoma',
        //                fontSize: 11,
        //                width: gglData.length * 85,
        //                height: '200',
        //                animation: {
        //                    duration: 500,
        //                    startup: true //This is the new option
        //                },
        //                legend: { position: 'none' },
        //                annotations: {
        //                    textStyle: {
        //                        fontName: 'Tahoma',
        //                        fontSize: 10,
        //                        color: '#808080'
        //                    },
        //                },
        //                chartArea: {
        //                    top: 20,
        //                    left: 100,
        //                },
        //                colors: ['#4075ae'],
        //                seriesType: 'bars',
        //                series: {
        //                    0: { targetAxisIndex: 0 },
        //                    1: { type: 'line', targetAxisIndex: 1, color: 'red' }
        //                },
        //                pointSize: 4,  
        //            };
        //            if (Array.isArray(gglData) && gglData.length) {
        //                GetdataSecondaryChartCPTH_PRI_MON(result.d, options, "DSR", "Bar");
        //            } else {
        //                //erros = erros + "No data DSR ";
        //                // document.getElementById("errors").innerText="asdasd";
        //                document.getElementById("errors").innerText = erros;
        //            }
        //        },
        //        error: function (result, status, error) {
        //            alert("a");
        //        },
        //        async: false
        //    });
        //}

        //function drawDSRChartV2() {
        //    $.ajax({
        //        type: "POST",
        //        url: "/Dashboard/ChartService.asmx/OnClickCharV2_PRI_TH",
        //        data: "{ filter: '' , colFilter: 'REGION', Cols: 'DSR_NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE',ACTUAL: 'ACTUAL',TARGET :'TRGET'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (result) {
        //            var gglData = dataArrayCPTH(result.d);
        //            var options = {
        //                titleTextStyle: {
        //                    color: '#808080',
        //                    fontSize: 16
        //                },
        //                fontName: 'Tahoma',
        //                fontSize: 11,
        //                width: (gglData.length +2) * 70,
        //                height: '200',
        //                animation: {
        //                    duration: 500,
        //                    startup: true //This is the new option
        //                },
        //                legend: { position: 'none' },
        //                annotations: {
        //                    textStyle: {
        //                        fontName: 'Tahoma',
        //                        fontSize: 10,
        //                        color: '#808080'
        //                    },
        //                },
        //                chartArea: {
        //                    top: 20,
        //                    left: 100,
        //                    right: 100,
        //                },
        //                seriesType: 'bars',
        //                series: {
        //                    0: { targetAxisIndex: 0 },
        //                    1: { type: 'line', targetAxisIndex: 1, color: 'red' }
        //                },
        //                pointSize: 4,
        //                colors: ['#4075ae'],
        //                hAxis: { viewWindow: { min: 0 } },
        //            };
        //            if (Array.isArray(gglData) && gglData.length) {
        //                GetdataSecondaryChartCPTH_PRI_MON(result.d, options, "DSR", "Combo");
        //            } else {
        //                //erros = erros + "No data DSR ";
        //                // document.getElementById("errors").innerText="asdasd";
        //                document.getElementById("errors").innerText = erros;
        //            }
        //        },
        //        error: function (result, status, error) {
        //            alert("a");
        //        },
        //        async: false
        //    });
        //}

        function setLabelTimeGone_TH() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataOrtherPercent",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'TIME_GONE', table: 'D_MTD_PRIMARY_UPDATE'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    console.log("TimeGone",result);
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
        };

        //function setLabelECC() {
        //    $.ajax({
        //        type: "POST",
        //        url: "/Dashboard/ChartService.asmx/getDataOrtherPercent",
        //        data: "{ filter: '' , colFilter: 'REGION', colSum: 'ECC', table: 'D_MTD_PRIMARY_UPDATE'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (result) {
        //            //console.log(result.d);
        //            var obj = JSON.parse(result.d);
        //            obj.forEach(function (item) {
        //                document.getElementById('lblECCValue').innerHTML = item.ORTHERPERCENT;
        //            });
        //        },
        //        error: function (result, status, error) {
        //            var err = eval("(" + result.responseText + ")");
        //            alert(err.Message);
        //        },
        //        async: false
        //    });
        //};

        //function setLabelVisit() {
        //    $.ajax({
        //        type: "POST",
        //        url: "/Dashboard/ChartService.asmx/getDataOrtherPercent",
        //        data: "{ filter: '' , colFilter: 'REGION', colSum: 'VISITED', table: 'D_MTD_PRIMARY_UPDATE'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (result) {
        //            //console.log(result.d);
        //            var obj = JSON.parse(result.d);
        //            obj.forEach(function (item) {
        //                document.getElementById('lblVisitValue').innerHTML = item.ORTHERPERCENT;
        //            });
        //        },
        //        error: function (result, status, error) {
        //            var err = eval("(" + result.responseText + ")");
        //            alert(err.Message);
        //        },
        //        async: false
        //    });
        //};

        function setLabelPercent_TH() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataPercent",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'ACTUAL', colSum2: 'TRGET', table: 'D_MTD_PRIMARY_UPDATE'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //console.log(result.d);
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
        };

        function setLabelMTD_TH() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataMTD",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'ACTUAL', table: 'D_MTD_PRIMARY_UPDATE'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //console.log(result.d);
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
        };

        function setLabelTarget_TH() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataMTD",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'TRGET', table: 'D_MTD_PRIMARY_UPDATE'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //console.log(result.d);
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
        };

        function slAreaChanged(value) {
            google.setOnLoadCallback(function () { classChangeSecondaryCPTH_PRI_MON(value, "AREA") });
        }

        function SaveWithParameter() {
            var selector = document.getElementById('<%=slArea.ClientID %>');
            var result = [];
            var options = selector && selector.options;
            var opt;
            for (var i = 0, iLen = options.length; i < iLen; i++) {
                opt = options[i];

                if (opt.selected) {
                    result.push(opt.value);
                }
            }
            var temp = result.toString();
            slAreaChanged(temp);
        }

        $(() => {
            var result = [];
            $("option").mousedown(function (e) {
                e.preventDefault();
                var select = this;
                var scroll = select.scrollTop;
                var value = select.value;
                e.target.selected = !e.target.selected;

                setTimeout(function () { select.scrollTop = scroll; }, 0);

                $(select).focus();
                if (e.target.selected) {
                    result.push(select.value);
                } else {
                    for (var i = 0; i < result.length; i++) {
                        if (result[i] === value) {
                            result.splice(i, 1);
                        }
                    }
                }
                slAreaChanged(result);
                //console.log(select.value);
                //console.log(e.target.selected);
                //console.log(result);
                //console.log(e.target.selected);
            }).mousemove(function (e) { e.preventDefault(); });

        });
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
    </style>
    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:HiddenField ID="P5sLocaltionCountry" runat="server" />
        <div class="loader-wrapper">
            <img src="images/loading.gif" alt="" width="150px" />
        </div>
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPnlMain" runat="server">
                    <div style="width: 98%; padding: 10px; background-color: #808080;">
                        <asp:Button ID="P5sLbtnRemoveSession" Style="width: 10%;" runat="server" CausesValidation="false" Text="Clear Filter" OnClick="P5sLbtnRemoveSession_Click" />
                    </div>
                    <div class="dhb-header">
                        <h4 style="color: #808080"><%=MMV.L5sMaster.L5sLangs["Primary Sale Perfomance - MTD - TH"]%></h4>
                    </div>
                    <div class="dhb-body">
                        <div class="dhb-body_sub-1 box-shadow">
                            <div class="div_padding">
                                <asp:Label class="title-chart" runat="server"><%=MMV.L5sMaster.L5sLangs["Area"]%></asp:Label>
                                <select class="input-sm" style="width: 90%; height: 95%;" id="slArea" runat="server" multiple="true">
                                </select>
                            </div>
                        </div>
                        <div class="dhb-body_sub-2">
                            <div class="dhb-body_sub-2_1 box-shadow">
                                <div class="div_padding">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Region"]%></div>
                                    <div id="Region"></div>
                                </div>
                            </div>
                            <div class="dhb-body_sub-2_2 box-shadow">
                                <div class="div_padding">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["CDM"]%></div>
                                    <div style="height: 90%" id="Province"></div>
                                </div>
                            </div>
                        </div>
                        <div class="dhb-body_sub-3">
                            <div class="dhb-body_sub-3_1 box-shadow">
                                <table class="table-details">
                                    <tr>
                                        <td style="text-align: left; padding: 5px; width: 21%; border-right: 1px solid">
                                            <p class="title-chart" style="margin: 0;"><%=MMV.L5sMaster.L5sLangs["MTD"]%></p>
                                        </td>
                                        <td style="text-align: left; padding: 5px; width: 21%; border-right: 1px solid">
                                            <p class="title-chart" style="margin: 0;"><%=MMV.L5sMaster.L5sLangs["Target"]%></p>
                                        </td>
                                        <td style="text-align: left; padding: 5px; width: 15%; border-right: 1px solid">
                                            <p class="title-chart" style="margin: 0;">% <%=MMV.L5sMaster.L5sLangs["ACH"]%></p>
                                        </td>
                                        <td style="text-align: left; padding: 5px; width: 15%; border-right: 1px solid">
                                            <p class="title-chart" style="margin: 0;">% <%=MMV.L5sMaster.L5sLangs["ECC"]%></p>
                                        </td>
                                        <td style="text-align: left; padding: 5px; width: 15%; border-right: 1px solid">
                                            <p class="title-chart" style="margin: 0;">% <%=MMV.L5sMaster.L5sLangs["Visited"]%></p>
                                        </td>
                                        <td style="text-align: left; padding: 5px; width: 12%">
                                            <p class="title-chart" style="margin: 0;">% <%=MMV.L5sMaster.L5sLangs["Time Gone"]%></p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; border-right: 1px solid">
                                            <div class="value-chart" title='<%=MMV.L5sMaster.L5sLangs["MTD"]%>' style="color: green" id="lblMTDValue"></div>
                                        </td>
                                        <td style="text-align: center; border-right: 1px solid">
                                            <div class="value-chart" title='<%=MMV.L5sMaster.L5sLangs["Target"]%>' style="color: green" id="lblTargetValue"></div>
                                        </td>
                                        <td style="text-align: center; border-right: 1px solid">
                                            <div class="value-chart" title='% <%=MMV.L5sMaster.L5sLangs["ACH"]%>' style="color: green" id="lblPercentValue"></div>
                                        </td>
                                        <td style="text-align: center; border-right: 1px solid">
                                            <div class="value-chart" title='% <%=MMV.L5sMaster.L5sLangs["ECC"]%>' style="color: orange" id="lblECCValue"></div>
                                        </td>
                                        <td style="text-align: center; border-right: 1px solid">
                                            <div class="value-chart" title='% <%=MMV.L5sMaster.L5sLangs["Visited"]%>' style="color: red" id="lblVisitValue"></div>
                                        </td>
                                        <td style="text-align: center">
                                            <div class="value-chart" title='% <%=MMV.L5sMaster.L5sLangs["Time Gone"]%>' style="color: orange" id="lblTimeGoneValue"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="dhb-body_sub-3_2 box-shadow">
                                <div class="div_padding" style="width:97% !important;">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Distributor"]%></div>
                                    <div style="overflow: auto; height:90%; bottom: 0;">
                                        <div id="Distributor"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="dhb-body_sub-3_3 box-shadow">
                                <div class="div_padding"  style="width:97% !important;">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["DSR"]%></div>
                                    <div style="overflow: auto; height:90%; bottom: 0;">
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
