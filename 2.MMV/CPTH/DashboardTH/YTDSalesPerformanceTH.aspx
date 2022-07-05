<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/P5s.Master" CodeBehind="YTDSalesPerformanceTH.aspx.cs" Inherits="MMV.DashboardTH.YTDSalesPerformanceTH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <link href="/fsmdls/mmcv4/CSS/fscstyles_181103.css" rel="stylesheet" />
    <link href="CSS/dashboard.css" rel="stylesheet" />
    <script src="/Dashboard/loader.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Dashboard/jquery-3.3.1.min.js"></script>
    <script src="/CPTH/DashboardTH/LoadChartCPTH.js" type="text/javascript"></script>
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
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'REGION', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result, status, xhr) {
                    var gglData = dataArrayCPTH(result.d);
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
                        colors: ['#006266', '#DC3912', '#FF9900', '#9966ff', '#990099', '#1B1464'],
                    };
                    //console.log(status);
                    //console.log(result.d);
                    //console.log(xhr.responseJSON.d);
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataChartCPTH(result.d, options, "Region", "Pie");
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

        function drawDistributorChartV2() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getGrowthV2",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'NAME', colSum: 'GROSS_SALES_MLI', table: 'D_YTD_SALES_DATA_SEC'}",
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
                                fontSize: 12,
                                color: '#808080'
                            },
                        },
                        chartArea: {
                            top: 20,
                            left: 120,
                            right: 100
                        },
                        colors: ['#4075ae'],

                        seriesType: 'bars',
                        series: {
                            0: { targetAxisIndex: 0 },
                            1: { type: 'line', targetAxisIndex: 1, color: 'red' }
                        },
                        //curveType: 'function',
                        pointSize: 4,
                        vAxes: {
                            // Adds titles to each axis.
                            0: { title: 'Gross sales' },
                            1: { title: 'Percentage' }
                        },
                        hAxis: {
                            title: 'Distribution',
                            fontSize: 30
                        },


                    };
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataChartCPTH(result.d, options, "Distributor", "Combo");
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
        }

        function drawSubCatgChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'SUB_CATG', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
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
                        width: gglData.length * 45,
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
                        colors: ['#4075ae']
                    };
                    // console.log(result.d);
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataChartCPTH(result.d, options, "SubCatg", "Column");
                    } else {
                        //erros = "No data SubCatg, ";

                    }

                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        }

        function drawYearChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'YEAR', colSum: 'GROSS_SALES_MLI', orderBy: 'YEAR asc', table: 'D_YTD_SALES_DATA_SEC'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    let object = JSON.parse(result.d);
                    let dateTime = new Date();
                    let year = dateTime.getFullYear() ;
                    for (let val in object) {
                        if ((year - 1) > object[val]["YEAR"] ) {
                            object.splice(val,1);
                        } 
                    }
                    console.log(object);
                    var data = JSON.stringify(object);
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
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataChartCPTH(data, options, "Year", "Column");
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
        }


        function drawBrandChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'BRAND', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
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
                    //console.log(result.d);
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataChartCPTH(result.d, options, "Brand", "Column");
                    } else {
                        // erros = "No data Brand ";
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

        function setLabelGrowth() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getGrowth",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'GROSS_SALES_MLI', table: 'D_YTD_SALES_DATA_SEC'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //console.log(result.d);
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
        };

        function setLabelYTD() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getYTD",
                data: "{ filter: '' , colFilter: 'REGION', colSum: 'GROSS_SALES_MLI', table: 'D_YTD_SALES_DATA_SEC'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    // console.log(result.d);
                    var obj = JSON.parse(result.d);
                    obj.forEach(function (item) {
                        document.getElementById('lblYTDValue').innerHTML = item.YTD + " ฿";
                    });
                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });
        };

        function slMonthChanged(value) {
            google.setOnLoadCallback(function () { classChangeCPTH(value, "MONTH") });
        }

        function SaveWithParameter1() {
            var selector = document.getElementById('<%=slMonth.ClientID %>');
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
            slMonthChanged(temp)
        }

        function fadeOut() {

            $(".loader-wrapper").fadeIn('fast').fadeOut('slow');
        }

        var result = [];
        var d = new Date();
        var flat = 0;
        var flat_load = 0;

        function SaveWithParameter() {
            flat_load = 1;

            $(() => {
                $("option").on('mousedown touchstart', function (e) {
                    e.preventDefault();
                    flat_load = 1;
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
                    slMonthChanged(result);
                    //console.log(select.value);
                    //console.log(e.target.selected);
                    console.log(result);
                    //console.log(e.target.selected);
                }).on('mousemove touchmove', function (e) { e.preventDefault(); });

            });

        }


        $(() => {
            var values = "";
            //GET MONTH
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/getMonth",
                data: "{ table:'D_YTD_SALES_DATA_SEC'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    // console.log(result.d);
                    var obj = JSON.parse(result.d);
                    for (var prop of obj) {
                        values = values + "," + prop.MONTH + "*" + prop.Va_Month;
                    }
                    values = values.slice(1, values.length);

                },
                error: function (result, status, error) {
                    var err = eval("(" + result.responseText + ")");
                    alert(err.Message);
                },
                async: false
            });




            //clear result
            $("#<%=P5sLbtnRemoveSession.ClientID%>").click(() => {

                result = [];
                $("option").on('mousedown touchstart', function (e) {
                    e.preventDefault();
                    flat_load = 1;
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
                    slMonthChanged(result);
                    //console.log(select.value);
                    //console.log(e.target.selected);
                    console.log(result);
                    //console.log(e.target.selected);
                }).on('mousemove touchmove', function (e) { e.preventDefault(); });



            })

            //select month now - 1
            var month = d.getMonth() + 1 - 1;
            //var values = "T01 - Jan,T02 - Feb,T03 - Mar,T04 - Apr,T05 - May,T06 - Jun,T07 - Jul,T08 - Aug,T09 - Sep,T10 - Oct,T11 - Nov,T12 - Dec";
            if (flat_load == 0) {
                $.each(values.split(","), function (i, e) {
                    var numberMonth = parseInt(e.slice(e.indexOf("*") + 1, e.length));
                    if (month >= numberMonth) {

                        $("option[value='" + e.slice(0, e.indexOf("*")) + "']").prop("selected", true);
                        result.push(e.slice(0, e.indexOf("*")));
                    }

                });
                slMonthChanged(result);
            }



            //select option not used press "CTRL"
            $("option").on('mousedown touchstart', function (e) {
                e.preventDefault();
                flat_load = 1;
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
                slMonthChanged(result);
                //console.log(select.value);
                //console.log(e.target.selected);
                console.log(result);
                //console.log(e.target.selected);
            }).on('mousemove touchmove', function (e) { e.preventDefault(); });





        });
    </script>
    <style>
        option {
            padding: 10px;
            font-size: 14px;
        }

        #SubCatg {
            overflow-x: scroll;
            overflow-y: hidden;
            width: 300px;
        }

        #Brand {
            overflow-x: scroll;
            overflow-y: hidden;
            width: 700px;
        }

        #Region {
            width: 95%;
            height: 80%;
        }

        #Year {
            overflow-x:auto;
            overflow-y:hidden;
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
            opacity: 0.9;
        }
    </style>
    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="loader-wrapper">
            <img src="images/loading.gif" alt="" width="150px" />
        </div>
        <asp:HiddenField ID="P5sLocaltionCountry" runat="server" />
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPnlMain" runat="server">
                    <div style="width: 98%; padding: 10px; background-color: #808080;">
                        <asp:Button ID="P5sLbtnRemoveSession" Style="width: 10%;" runat="server" CausesValidation="false" Text="Clear Filter" OnClientClick="return SaveWithParameter();" OnClick="P5sLbtnRemoveSession_Click" />
                    </div>
                    <div class="dhb-header">
                        <h4 style="color: #808080"><%=MMV.L5sMaster.L5sLangs["YTD Sales Perfomance - YTD (Mil)"]%></h4>
                        <p id="errors" style="color: red; text-align: left; text-wrap: normal"></p>
                    </div>
                    <div class="dhb-body">
                        <div class="dhb-body_sub-1 box-shadow">
                            <div class="div_padding">
                                <asp:Label class="title-chart" runat="server"><%=MMV.L5sMaster.L5sLangs["Month"]%></asp:Label>
                                <select id="slMonth" style="width: 90%; height: 95%;" onclick="Javascript:SaveWithParameter()" runat="server" multiple="true"></select>
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
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Year"]%></div>
                                    <div style="height: 90%;" id="Year">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="dhb-body_sub-3">
                            <div class="dhb-body_sub-3_1 box-shadow">
                                <table class="table-details">
                                    <tr>
                                        <td style="text-align: left; padding: 5px; width: 50%; border-right: 1px solid">
                                            <p class="title-chart" style="margin: 0;"><span>%Growth <asp:Literal ID="YEAR" runat="server"></asp:Literal></span> </p>
                                        </td>
                                        <td style="text-align: left; padding: 5px; width: 50%;">
                                            <p class="title-chart" style="margin: 0;"><%=MMV.L5sMaster.L5sLangs["YTD"]%></p>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="text-align: center; border-right: 1px solid">
                                            <div class="value-chart" title='%Growth 2020 vs 2019' style="color: #4075ae" id="lblPerValue"></div>
                                        </td>
                                        <td style="text-align: center;">
                                            <div class="value-chart" title='<%=MMV.L5sMaster.L5sLangs["YTD"]%>' style="color: green" id="lblYTDValue"></div>
                                        </td>

                                    </tr>
                                </table>
                            </div>
                            <div class="dhb-body_sub-3_2 box-shadow" style="height:86% !important">
                                <div class="div_padding" style="width: 97% !important;">
                                    <div class="title-chart"><%=MMV.L5sMaster.L5sLangs["Distributor"]%> (<asp:Literal ID="YEAR_DISTRIBUTOR" runat="server"></asp:Literal>)</div>
                                    <div style="overflow-x: auto;overflow-y:hidden; height: 90%; bottom: 0;">
                                        <div id="Distributor"></div>
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
