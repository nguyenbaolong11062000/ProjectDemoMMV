<%@ Page Title="" Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" CodeBehind="PrimaryInvoiceSalesPerformanceMTD.aspx.cs" Inherits="MMV.Dashboard.PrimaryInvoiceSalesPerformanceMTD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <link href="/fsmdls/mmcv4/CSS/fscstyles_181103.css" rel="stylesheet" />
    <script src="/Dashboard/loader.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Dashboard/jquery-3.3.1.min.js"></script>
    <script src="/Dashboard/LoadChart.js" type="text/javascript"></script>
    <script type="text/javascript">
        var erros = "No data.";
        google.load("visualization", "1", { packages: ["corechart"] });
        // Set a callback to run when the Google Visualization API is loaded.
        $().ready(function () {
            google.charts.setOnLoadCallback(
                function () {
                    $(".loader-wrapper").fadeOut('Slow');
                });
        })

        function drawRegionChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'REGION', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
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
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataPriMTDChart(result.d, options, "Region", "Pie");
                    } else {
                        //erros = "No data Region, ";
                       
                    }
                   
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        }

        function drawProvinceChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'PROVINCE', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
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
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataPriMTDChart(result.d, options, "Province", "Bar");
                    } else {
                        //erros = "No data Province, ";

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
                url: "/Dashboard/ChartService.asmx/OnClickChar",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'NAME', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
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
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataPriMTDChart(result.d, options, "Distributor", "Bar");
                    } else {
                        //erros = "No data Distributor, ";

                    }
                    
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        }

        function drawCDSChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'CDS', colSum: 'ACTUAL', orderBy: 'ACTUAL DESC', table: 'D_MTD_PRIMARY_UPDATE'}",
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
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataPriMTDChart(result.d, options, "CDS", "Bar");
                    } else {
                        //erros = "No data CDS ";
                        document.getElementById("errors").innerText = erros;

                    }
                    
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        }

        function setLabelTimeGone() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataOrtherPercent",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'TIME_GONE', table: 'D_MTD_PRIMARY_UPDATE'}",
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
        };

        function setLabelPercent() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataPercent",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'ACTUAL', colSum2: 'TRGET', table: 'D_MTD_PRIMARY_UPDATE'}",
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
        };

        function setLabelMTD() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataMTD",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'ACTUAL', table: 'D_MTD_PRIMARY_UPDATE'}",
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
        };

        function setLabelTarget() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getDataMTD",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'TRGET', table: 'D_MTD_PRIMARY_UPDATE'}",
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
        };

        function slAreaChanged(value) {
            google.setOnLoadCallback(function () { classChangePriMTD(value, "AREA") });
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
            slAreaChanged(temp)
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
            width: 90%;
        }

        #CDS {
            overflow-x: hidden;
            overflow-y: scroll;
            width: 90%;
        }

        #Distributor {
            overflow-x: hidden;
            overflow-y: scroll;
            width: 90%;
        }

        .loader-wrapper {
            width: 100%;
            height: 100%;
            position: fixed;
            top: 0;
            overflow: hidden;
            left: 0;
            z-index: 999;
            background-color: #FFF;
            display: flex;
            justify-content: center;
            align-items: center;
            opacity:0.9;
        }
    </style>
    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:HiddenField ID="P5sLocaltionCountry" runat="server" />
        <div class="loader-wrapper">
           <img src="images/loading.gif" alt="" width="150px"/>
        </div>
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPnlMain" runat="server">
                    <div style="width: 98%; padding: 10px; background-color: #808080;">
                        <asp:Button ID="P5sLbtnRemoveSession" Style="width: 10%;" runat="server" CausesValidation="false" Text="Clear Filter" OnClick="P5sLbtnRemoveSession_Click" />
                    </div>
                    <h4 style="color: #808080"><%=MMV.L5sMaster.L5sLangs["Primary Sale Perfomance - MTD - Current month (Mil)"]%></h4>
                    <p id="errors" style="color:red;text-align:left;text-wrap:normal"> </p>
                    <table style="width: 100%; height: 800px;">
                        <tr>
                            <td rowspan="3" style="width: 10%; height: 100%; text-align: center; vertical-align: top;">
                                <div style="width: 100%;">
                                    <asp:Label Style="color: #808080; font-size: 14px;" runat="server"><%=MMV.L5sMaster.L5sLangs["Area"]%></asp:Label>
                                </div>
                                <br />
                                <div style="width: 118px;">
                                    <select class="input-sm" style="width: 100%; height: 480px;" id="slArea" runat="server" multiple="true">
                                    </select>
                                </div>
                            </td>
                            <td style="width: 45%; height: 35%; padding: 0;">
                                <div style="height: 100%" id="Region"></div>
                            </td>
                            <td rowspan="2" style="width: 45%; padding: 0;">
                                <div style="width: 70%; float: left; display: inline-block;">
                                    <div>
                                        <h4 style="color: #808080"><%=MMV.L5sMaster.L5sLangs["MTD"]%></h4>
                                    </div>
                                    <div style="text-align: right; margin-right: 15%;">
                                        <div class="lblMTD01" id="lblMTDValue"></div>
                                    </div>
                                    <div style="text-align: right;">
                                        <div class="lblMTD02" id="lblPercentValue"></div>
                                    </div>
                                    <div style="text-align: right; margin-right: 15%;">
                                        <p style="font-size: 20px; color: #808080;"><%=MMV.L5sMaster.L5sLangs["Target"]%></p>
                                    </div>
                                    <div style="text-align: right; margin-right: 15%;">
                                        <div class="lblMTD01" id="lblTargetValue"></div>
                                    </div>
                                </div>
                                <div style="width: 30%; float: left; display: inline-block;">
                                    <div style="text-align: center;">
                                        <p style="font-size: 15px; color: #808080;">% <%=MMV.L5sMaster.L5sLangs["Time Gone"]%></p>
                                    </div>
                                    <div style="text-align: center;">
                                        <div id="lblTimeGoneValue" style="font-size: 20px; color: red; padding: 0;"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                                <h4 style="margin-left: 1%; color: #808080;"><%=MMV.L5sMaster.L5sLangs["Distributor"]%></h4>
                                <div style="width: 100%; height: 30%;" id="Distributor">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 45%; padding: 0;">
                                <h4 style="margin-left: 1%; color: #808080;"><%=MMV.L5sMaster.L5sLangs["Province"]%></h4>
                                <div style="height: 70%" id="Province">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 35%; padding: 0;">
                                <h4 style="margin-left: 1%; color: #808080;"><%=MMV.L5sMaster.L5sLangs["CDS"]%></h4>
                                <div style="height: 70%" id="CDS">
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:HiddenField ID="P5sHfRange" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
