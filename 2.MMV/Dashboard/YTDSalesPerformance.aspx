<%@ Page Title="" Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" CodeBehind="YTDSalesPerformance.aspx.cs" Inherits="MMV.Dashboard.YTDSalesPerformance" %>

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
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'REGION', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result, status, xhr) {
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
                    console.log(status);
                    console.log(result.d);
                    console.log(xhr.responseJSON.d);
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataChart(result.d, options, "Region", "Pie");
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

        function drawSubCatgChart() {
            $.ajax({
                type: "POST",
                url: "/Dashboard/ChartService.asmx/OnClickChar",
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'SUB_CATG', colSum: 'GROSS_SALES_MLI', orderBy: 'GROSS_SALES_MLI DESC', table: 'D_YTD_SALES_DATA_SEC'}",
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
                        GetdataChart(result.d, options, "SubCatg", "Column");
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
                data: "{ filter: '' , colFilter: 'REGION', Cols: 'YEAR', colSum: 'GROSS_SALES_MLI', orderBy: 'YEAR', table: 'D_YTD_SALES_DATA_SEC'}",
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
                    // console.log(result.d);
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataChart(result.d, options, "Year", "Column");
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
                    //console.log(result.d);
                    if (Array.isArray(gglData) && gglData.length) {
                        GetdataChart(result.d, options, "Brand", "Column");
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
                        document.getElementById('lblYTDValue').innerHTML = item.YTD;
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
            google.setOnLoadCallback(function () { classChange(value, "MONTH") });
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
                $("option").on('mousedown touchstart',function (e) {
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
                }).on('mousemove touchmove',function (e) { e.preventDefault(); });

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
                $("option").on('mousedown touchstart',function (e) {
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
                }).on('mousemove touchmove',function (e) { e.preventDefault(); });



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
            $("option").on('mousedown touchstart',function (e) {
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
            }).on('mousemove touchmove',function (e) { e.preventDefault(); });





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
                    <h4 style="color: #808080"><%=MMV.L5sMaster.L5sLangs["YTD Sales Perfomance - YTD (Mil)"] %></h4>
                    <p id="errors" style="color: red; text-align: left; text-wrap: normal"></p>
                    <table style="width: 100%; height: 500px;">
                        <tr style="height: 200px;">
                            <td rowspan="3" style="width: 10%; text-align: center; vertical-align: top;">
                                <div style="width: 100%;">
                                    <asp:Label Style="color: #808080; font-size: 14px;" runat="server"><%=MMV.L5sMaster.L5sLangs["Month"]%></asp:Label>
                                </div>
                                <br />
                                <div style="width: 118px;">
                                    <select class="input-sm" style="width: 100%; height: 480px;" id="slMonth" onclick="Javascript:SaveWithParameter()" runat="server" multiple="true">
                                    </select>
                                </div>
                            </td>
                            <td style="width: 30%; padding: 0;">
                                <div style="height: 100%;" id="Region"></div>
                            </td>
                            <td rowspan="2" style="width: 60%; padding: 0;">
                                <div style="width: 80%; height: 100%; float: left;" id="Year">
                                </div>
                                <div style="width: 20%; float: left; text-align: center; margin-top: 5%;">
                                    <asp:Label ID="lblPerGrowth" Style="color: #808080; font-size: 12px;" runat="server"><span>%Growth <asp:Literal ID="YEAR" runat="server"></asp:Literal></span></asp:Label>
                                    <div class="lblPerGrowth" id="lblPerValue"></div>
                                    <br />
                                    <asp:Label ID="lblYTD" Style="color: #808080; font-size: 12px;" runat="server"><%=MMV.L5sMaster.L5sLangs["YTD"]%></asp:Label>
                                    <div class="lblPerYTD" id="lblYTDValue"></div>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 100px;">
                            <td rowspan="2" style="width: 30%; padding: 0;">
                                <h4 style="margin-left: 5%; color: #808080;"><%=MMV.L5sMaster.L5sLangs["SubCatg"]%></h4>
                                <div style="height: 80%" id="SubCatg">
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 200px;">
                            <td style="width: 60%; padding: 0;">
                                <h4 style="margin-left: 1%; color: #808080;"><%=MMV.L5sMaster.L5sLangs["Brand"]%></h4>
                                <div style="height: 80%" id="Brand">
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
