<%@ Page Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" CodeBehind="TrackingPH.aspx.cs"
    Inherits="MMV.fsmdls.mmcv4.TrackingPH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">

    <script src="P5sMapsPH.js"></script>

    <script src="../../libs/KeyGoogleMap.js"></script>

    <script src="../../libs/markerwithlabel.js"></script>

    <link href="CSS/fscstyles_181103.css" rel="stylesheet" />

    <style type="text/css">
        .labels {
            color: red;
            font-family: "Lucida Grande", "Arial", sans-serif;
            font-size: 12px;
            font-weight: bold;
            text-align: left;
            width: 150px;
        }
    </style>

    <script type="text/javascript">

        function pageLoad(sender, args) {
            $(document).ready(function () {
                ShowOption();
                $("#ImgBackForward").click(function () {


                    var src = $(this).attr('src');

                    if (src.indexOf("images/back.png") >= 0) {
                        $(this).attr('src', "images/forward.png");
                        $("#TbLefPanel").animate({
                            left: "-280px"
                        });

                    }
                    else {
                        $(this).attr('src', "images/back.png");
                        $("#TbLefPanel").animate({
                            left: "10px"
                        });

                    }

                });
            });

        }



    </script>

    <script type="text/javascript">
        var myMap;

        var CONTAINER_ID = 'container';
        var ACCORDION_CLIENT = "<%=P5sMyAccordion.ClientID%>";
        var PATH_ARROW_IMG = "icon/arrowOption.png";
        var PATH_ARROW_DOWN_IMG = "icon/arrowOptionDown.png";
        var PATH_ICON_OUTLET = "icon/Outlet.png";
        var PATH_ICON_OUTLET_VISITED = "icon/Visited_Outlet.png";

        var myTracking = null;
        var P5sSpeedValue = 0;

        //hàm khỏi tạo bản đồ - hàm này phải được gọi đầu tiên khi muốn xử dụng bản đồ
        function LoadMap() {
            //clear variable
            myMap = null;
            myTracking = null;
            durationDisplayStop = 0;
            var windowWidth = $(window).width(); //-330;
            var windowHeight = $(window).height() - 10;
            $('#container').css({ 'width': windowWidth - 10, 'height': windowHeight });
            var LocationCountry = document.getElementById('<%= P5sLocaltionCountry.ClientID%>').value;
            var split = LocationCountry.split(',');
            var cairo = { lat: parseFloat(split[0]), lng: parseFloat(split[1]) };
            myMap = new google.maps.Map(document.getElementById('container'), {
                scaleControl: true,
                center: cairo,
                zoom: 6
            });
            $("#P5sDivSpeedRange").slider({
                step: 0.1,
                min: 0.0,
                max: 1.0,
                value: 0.5,
                change: function (event, ui) {
                    var value = $("#P5sDivSpeedRange").slider("option", "value");
                    value = 1 - value;
                    //document.getElementById("P5sDivSpeedValue").innerHTML= value + "s";
                    P5sSpeedValue = value;
                }
            });
            $("#P5sDivSpeedRange").css('background', 'rgb(113, 73, 4)');
            $("#P5sDivSpeedRange").css('height', '0.2em');
            $("#P5sDivSpeedRange .ui-state-default").css('height', '0.7em');
            $("#P5sDivSpeedRange .ui-state-default").css('width', '0.6em');
            $("#P5sDivSpeedRange .ui-state-default").css('background', 'rgb(219, 142, 10)');

            initControl(myMap);

            myMap.addListener('dragend', function () {
                if (myTracking != undefined) {
                    if (myTracking.StatusRePlay == false) {
                        //xem code P5sMaps.js để biết thêm chi tiết
                        //                            myTracking.ShowStart();    
                        //                            myTracking.ShowPoint();   
                        //                            myTracking.ShowRouteText();  
                        //                            myTracking.ShowRouteTextFS(); 
                        //                            myTracking.ShowRouteLine();  
                        //                            myTracking.ShowShops(); 
                        //                            myTracking.ShowStop(); 
                        //                            myTracking.ShowRePlayPoints();

                    }
                    else {
                        //nếu user đang xử dụng tính năng replay thì hiển thị 1 số thông tin như bên dưới 
                        //                            myTracking.ShowStart();      
                        //                            myTracking.ShowRouteTextFS(); 
                        //                            myTracking.ShowRouteLine();  
                        //                            myTracking.ShowShops(); 
                        //                            myTracking.ShowStop(); 

                    }
                }

            });


            //                    myMap.addListener('dragend', function () 
            //                    { 
            //                        if(myTracking != undefined && myTracking.StatusRePlay == true)
            //                        {
            //                            myTracking.P5sShowRePlay(myTracking.SalCD);
            //                        }  
            //                    });
        }



        //hàm chính để khỏi tạo thông tin tracking để hiển thị lên bản đồ   
        //objectTrackingOfSales: thông tin về tuyến đường đi
        //objectShop: thông tin về cửa hàng (khách hàng)
        function CreatedCTracking(objectTrackingOfSales, objectShop) {
            $(function () {
                myTracking = new CTracking(myMap, objectTrackingOfSales, objectShop);
                fitOverlays(myMap, CTracking.prototype.mEnd);
            });
        }
        function initControl(myMap) {
            ////
            this.id = 'divContainer';
            var div_container = document.createElement("div");
            div_container.id = 'divContainer';
            div_container.style.position = 'relative';
            div_container.style.float = "right";
            div_container.style.margin = "50px";
            div_container.style.marginTop = $(window).height() - 60 + "px";
            //////       
            var div_range = document.createElement("input");
            div_range.style.color = "#20b526";
            div_range.id = 'divRange';
            div_range.type = "range";
            div_range.step = 1;
            div_range.min = 0;
            div_range.value = 0;
            div_range.max = 60;
            div_range.style.width = "200px";
            div_range.style.cursor = 'pointer';
            div_range.style.float = "left";
            div_container.appendChild(div_range);
            /////
            //////       
            var div_value_range = document.createElement("b");
            div_value_range.style.color = "#ff0200";
            div_value_range.id = 'divValueRange';
            div_value_range.style.cursor = 'pointer';
            div_value_range.style.float = "left";
            div_value_range.style.marginTop = "2px";
            div_value_range.style.marginLeft = "3px";
            div_container.appendChild(div_value_range);
            div_value_range.appendChild(document.createTextNode("Time 0'"));
            div_range.addEventListener('change', function () {
                var value = document.getElementById("divRange").value;
                document.getElementById("divValueRange").innerHTML = "Time " + value + "'";
                durationDisplayStop = value;
                if (myTracking != null)
                    myTracking.ShowStop();
            });
            myMap.controls[google.maps.ControlPosition.TOP_RIGHT].push(div_container);
        }

        var StatusRePlay = false;

        //hàm khỏi tạo control Time: control này để use chọn lưa việc hiển thị các điểm đừng theo thời gian chọn
        //vd: nếu user chọn 10' thì hệ thống sẽ chỉ hiển thị các điểm dừng có thời gian dừng lớn hơn hoặc bằng 10'



        //hàm này được xử dụng để ẩn hiện các pannel chọn xem thông tin theo các loại khác nhau
        // trang này cho phép xem thông tin tracking theo ASM, SUP, NVBH
        // mỗi 1 loại sẽ có cách thức filter khác nhau 
        // tùy vào loại chọn mà sẽ hiển thị thông tin tương 
        function ShowOption() {
            var Region = document.getElementById("P5sTrRegion");
            var Area = document.getElementById("P5sTrArea");
            var TrDistributor = document.getElementById("P5sTrDistributor");
            var TrSales = document.getElementById("P5sTrSales");
            var Supervisor = document.getElementById("P5sTrSupervisor");
            var ASM = document.getElementById("P5sTrASM");

            var P5DdlViewOption = document.getElementById('<%=P5sDdlViewOption.ClientID%>');
                var ValueP5DdlViewOption = P5DdlViewOption.options[P5DdlViewOption.selectedIndex].value;
                if (ValueP5DdlViewOption == 1) {
                    TrDistributor.style.display = "block";
                    TrSales.style.display = "block";
                    ASM.style.display = "none";
                    Supervisor.style.display = "none";
                }
                else if (ValueP5DdlViewOption == 2) {
                    TrDistributor.style.display = "none";
                    TrSales.style.display = "none";
                    ASM.style.display = "none";
                    Supervisor.style.display = "block";
                }
                else if (ValueP5DdlViewOption == 3) {
                    Region.style.display = "none";
                    Area.style.display = "none";
                    TrDistributor.style.display = "none";
                    Supervisor.style.display = "none";
                    TrSales.style.display = "none";
                    ASM.style.display = "block";
                }
            }


    </script>

    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" AsyncPostBackTimeout="0"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:HiddenField ID="P5sLocaltionCountry" runat="server" />
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPnlMain" runat="server">
                    <div id="container" style="white-space: nowrap; display: block; position: absolute;">
                    </div>
                    <table id="TbLefPanel" cellpadding="0" cellspacing="0" border="0" width="300px" height="100%"
                        style="z-index: 200; position: absolute; background-color: White">
                        <tr>
                            <td style="width: 1%; white-space: nowrap; vertical-align: top">
                                <div style="padding-left: 280px">
                                    <img alt="" id="ImgBackForward" src="images/back.png" style="cursor: pointer" />
                                </div>
                                <ajaxToolkit:Accordion ID="P5sMyAccordion" runat="Server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                    FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" AutoSize="Fill"
                                    Width="300px" Height="100%" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                                    <Panes>
                                        <ajaxToolkit:AccordionPane ID="P5sAccPnlTracking" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <div style="font-size: medium; font-style: italic; color: Green; cursor: pointer;">
                                                    Working Itinerary
                                                </div>
                                            </Header>
                                            <Content>
                                                <table cellpadding="0" cellspacing="2" border="0" width="300px">
                                                    <tr hidden="hidden">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">By
                                                                        <br />
                                                                        <asp:DropDownList ID="P5sDdlViewOption" runat="server" AutoPostBack="true" OnSelectedIndexChanged="P5sDdlViewOption_SelectedIndexChanged">
                                                                            <asp:ListItem Text="DSRs - NVBH" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="CDSs - Giám sát" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="ASMs - Quản lý khu vực" Value="3"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <br />
                                                            Date <span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="5px" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:TextBox ID="P5sTxtDay" Width="80px" CssClass="input-sm" runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="P5sTxtDay"
                                                                            Format="dd-MM-yyyy">
                                                                        </ajaxToolkit:CalendarExtender>
                                                                    </td>
                                                                    <td>
                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td style="padding: 0">From:
                                                                                    <asp:DropDownList CssClass="input-sm" ID="P5sDdlFromHH" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:DropDownList CssClass="input-sm" ID="P5sDdlFromMM" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <table cellpadding="5px" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td style="padding: 0">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                                        &nbsp; &nbsp; 
                                                                    </td>
                                                                    <td>
                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td style="padding: 0">To:
                                                                                    <asp:DropDownList CssClass="input-sm" ID="P5sDdlToHH" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:DropDownList CssClass="input-sm" ID="P5sDdlToMM" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrRegion">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <br />
                                                            Region <span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:DropDownList Width="40%" CssClass="input-sm" ID="P5sDDLRegion" AutoPostBack="true" OnSelectedIndexChanged="P5sDDLRegion_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                                        <asp:TextBox Visible="false" ID="P5sTxtRegion" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrArea">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <br />
                                                            Area<span style="color: Red; white-space: nowrap;"> * </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:DropDownList Width="40%" CssClass="input-sm" ID="P5sDDLArea" AutoPostBack="true" OnSelectedIndexChanged="P5sDDLArea_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                                        <asp:TextBox Visible="false" ID="P5sTxtArea" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrDistributor">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <br />
                                                            Distributor<span style="color: Red; white-space: nowrap;"> * </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <div style="width: 290px; min-width: 290px;">
                                                                            <asp:DropDownList Width="95%" CssClass="input-sm" ID="P5sDDLDistributor" AutoPostBack="true" OnSelectedIndexChanged="P5sDDLDistributor_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                                            <asp:TextBox Visible="false" ID="P5sTxtDistributor" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrSales">

                                                        <td style="height: 150px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <br />
                                                            DSR - NVBH<span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <select class="input-sm" style="width: 95%; height: 150px;" id="selDSR" runat="server" multiple="true">
                                                                        </select>
                                                                        <asp:TextBox Visible="false" ID="P5sTxtSales" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                        <%-- <%=P5sActSales.L5sShowAddAll("P5sTxtSales_Add")%>
                                                                        <%=P5sActSales.L5sShowRemoveAll("P5sTxtSales_Remove")%>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr hidden="hidden" id="P5sTrASM">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">ASM<span style="color: Red; white-space: nowrap;"> * </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <div style="width: 290px; min-width: 290px;">
                                                                            <asp:TextBox ID="P5sTxtASM" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                            <%--  <%=P5sActASM.L5sShowAddAll("P5sTxtASM_Add")%>
                                                                            <%=P5sActASM.L5sShowRemoveAll("P5sTxtASM_Remove")%>--%>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr hidden="hidden" id="P5sTrSupervisor">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">CDSs - Giám sát <span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:TextBox ID="P5sTxtSupervisor" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                        <%--<%=P5sActSupervisor.L5sShowAddAll("P5sTxtSupervisor_Add")%>
                                                                        <%=P5sActSupervisor.L5sShowRemoveAll("P5sTxtSupervisor_Remove")%>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="text-align:center;">
                                                                        <br />
                                                                        <asp:Button ID="P5sLbtnLoadTracking" CausesValidation="true" OnClick="P5sLbtnLoadTracking_Click" Text="Display"
                                                                            runat="server"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" style="padding-left: 1px; vertical-align: top; color: Blue">
                                                            <a id="showPoint" onclick="myTracking.ProcessShowHidePoint(myTracking.SalCD);">
                                                                <img id="imgShowHidePoint2" alt="" />
                                                                <span id="spanShowHidePoint2"></span></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" style="padding-left: 1px; vertical-align: top; color: Blue">
                                                            <a id="idShowStop" onclick="myTracking.ProcessShowHideStop(myTracking.SalCD);">
                                                                <img id="imgShowHideStop2" alt="" />
                                                                <span id="spanShowHideStop2"></span></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" style="padding-left: 1px; vertical-align: top; color: Blue">
                                                            <a id="idShowRoute" onclick="myTracking.ProcessShowHideRoute(myTracking.SalCD)">
                                                                <img id="imgShowHideRoute2" alt="" />
                                                                <span id="spanShowHideRoute2"></span></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" style="padding-left: 1px; vertical-align: top; color: Blue">
                                                            <a id="showShop" onclick="myTracking.ProcessShowHideShop(myTracking.SalCD);">
                                                                <img id="imgShowHideShop2" alt="">
                                                                <span id="spanShowHideShop2"></span></a>
                                                             </tr>
                                                    <tr>
                                                        <td colspan="5" style="padding-left: 1px; vertical-align: top; color: Blue">
                                                            <div style="float: left">
                                                                <a id="idRePlay" onclick="myTracking.P5sShowRePlay(myTracking.SalCD);">
                                                                    <img id="imgShowRePlay2" alt="">
                                                                    <span id="spanShowRePlay2"></span></a>
                                                                 </div>
                                                            <div style="display: none; float: left; margin-left: 5px" id="P5sDivSpeed">
                                                                <%--Speed(s)<input type="number" min="0.1" max ="2" step ="0.1" id="P5sTxtSecond" style="width: 40px" value="0.5">--%>
                                                                <div style="float: left">
                                                                    slow
                                                                </div>
                                                                <div style="float: left; width: 140px; margin-left: 15px; margin-top: 8px" id="P5sDivSpeedRange">
                                                                </div>
                                                                <%-- <div style="float: left; margin-left: 20px" id="P5sDivSpeedValue">
                                                                        0.5s</div>--%>
                                                                <div style="float: left; margin-left: 5px">
                                                                    fast
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <%--<ajaxToolkit:AccordionPane ID="AccordionPaneTrackingVisit" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <hr />
                                                <span style="font-size: medium; font-style: italic; color: Green; cursor: pointer;
                                                    text-decoration: none" onclick="javascript:window.location.href = '/TrackingVisitUnVisitPH.aspx';">
                                                    Visualizing Coverage on Map</span>
                                            </Header>
                                            <Content>
                                                <br />
                                                <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                    <tr>
                                                        <td style="vertical-align: top; white-space: nowrap;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; padding-left: 140px; display: none;">
                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnTrackingVisit)%>
                                                            <asp:LinkButton ID="P5sLbtnTrackingVisit" OnClick="P5sLbtnTrackingVisit_Click" runat="server">Go to page</asp:LinkButton>
                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnTrackingVisit)%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>--%>
                                        <%--<ajaxToolkit:AccordionPane ID="P5sAccPnlDCoverageProgressSettingAndReports" runat="server"
                                            HeaderCssClass="accordionHeader" ContentCssClass="accordionContent">
                                            <Header>
                                                <hr />
                                                <span style="font-size: medium; font-style: italic; color: Green; cursor: pointer;
                                                    text-decoration: none" onclick="javascript:window.location.href = '/Report/CustomerList.aspx';">
                                                    Setting and Reports </span>
                                            </Header>
                                            <Content>
                                                <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                    <tr>
                                                        <td style="vertical-align: top; white-space: nowrap;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; padding-left: 140px; display: none;">
                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sSettingAndReports)%>
                                                            <asp:LinkButton ID="P5sSettingAndReports" OnClick="P5sSettingAndReports_Click" runat="server">Setting and Reports</asp:LinkButton>
                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sSettingAndReports)%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>--%>
                                    </Panes>
                                </ajaxToolkit:Accordion>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
