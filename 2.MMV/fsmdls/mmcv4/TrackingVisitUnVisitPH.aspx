<%@ Page Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" CodeBehind="TrackingVisitUnVisitPH.aspx.cs"
    Inherits="MMV.fsmdls.mmcv4.TrackingVisitUnVisitPH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <link href="CSS/fscstyles_181103.css" rel="stylesheet" />
    <style>
        #floating-panel {
            position: absolute;
            right: 5px;
            z-index: 5;
            color: white;
            text-shadow: -2px 0 black, 0 2px black, 2px 0 black, 0 -2px black;
            padding: 5px;
            text-align: right;
            font-family: 'Roboto','sans-serif';
            line-height: 30px;
            padding-left: 10px;
        }
    </style>

    <script type="text/javascript" src="http://maps.google.com/maps/api/js"></script>

    <script src="../../libs/KeyGoogleMap.js"></script>

    <script src="P5sMapsPH.js" type="text/javascript"></script>

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

            var myCustomer = null;
            var shopVisited = null;

            function LoadMap() {
                //clear variable
                myMap = null;
                myCustomer = null;
                shopVisited = null;


                //set Height
                //width: 860px;
                myMap = null;
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


                    myMap.addListener('dragend', function () {
                        if (myCustomer != undefined && myCustomer.IsVisitedShow == false) {
                            myCustomer.hideVisited();
                        }

                        if (myCustomer != undefined && myCustomer.IsSalesShow == false) {
                            myCustomer.hideSales();
                        }

                        if (myCustomer != undefined && myCustomer.IsVisitedSalesShow == false) {
                            myCustomer.hideVisitedSales();
                        }

                        if (myCustomer != undefined && myCustomer.IsNoVisitedShow == false) {
                            myCustomer.hideNoVisited();
                        }
                    });
                    google.maps.event.addListener(myMap.getStreetView(), 'visible_changed', function () {
                        if (this.getVisible()) {
                            action = function () {
                                var RangeValue = document.getElementById('<%= P5sHfRange.ClientID%>').value;
                                var CustNameList = "<h2>LIST OF ALL STORES IN RANGE OF " + RangeValue + "M</h2>";
                                var CurrentPoint = "";
                                var ArrayCust = new Array();
                                CurrentPoint = myMap.getStreetView().getPosition().toString();

                                for (var i = 0; i < object2.length; i++) {
                                    var distance = calcDistance(eval("new google.maps.LatLng" + CurrentPoint + ""),
                                                        eval("new google.maps.LatLng(" + object2[i].LatLng + ")"));
                                    if (parseInt(distance) <= parseInt(RangeValue)) {
                                        var Cust = new Object();
                                        Cust.range = distance;
                                        Cust.name = object2[i].CustomerName;
                                        ArrayCust.push(Cust);
                                        //CustNameList += object2[i].CustomerName + "<br />";
                                    }
                                }

                                ArrayCust.sort(function (a, b) {
                                    return parseFloat(a.range) - parseFloat(b.range);
                                });
                                for (var i = 0; i < ArrayCust.length; i++) {
                                    CustNameList += ArrayCust[i].name + "<br />";
                                }
                                document.getElementById("textcontant").innerHTML = CustNameList;
                            };
                            setTimeout(action, 2000);
                        }
                        else
                            document.getElementById("textcontant").innerHTML = "";
                    });

                }


                function showShop(jsonShop) {
                    $(function () {

                        myCustomer = new LoadCustomerVisitUnVisit(myMap, jsonShop);
                        //myMap.fitOverlays();          
                    });
                }
                function ShowOption() {
                    var Region = document.getElementById("P5sTrRegion");
                    var Area = document.getElementById("P5sTrArea");
                    var TrDistributor = document.getElementById("P5sTrDistributor");
                    var TrSales = document.getElementById("P5sTrSales");
                    var Supervisor = document.getElementById("P5sTrSupervisor");
                    var WorkingRoutes = document.getElementById("P5sTrWorkingRoutes");
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



        function HideShowVisited() {

            if (myCustomer == null)
                return false;

            if (document.getElementById("<%=P5sImgVisited.ClientID%>").src.indexOf("icon/Visited_Outlet.png") >= 0) //set hide
            {
                document.getElementById("<%=P5sImgVisited.ClientID%>").src = "icon/UnSelect_Visited_Outlet.png";
                myCustomer.hideVisited();
            }
            else   // set show        
            {
                myCustomer.showVisited();
                document.getElementById("<%=P5sImgVisited.ClientID%>").src = "icon/Visited_Outlet.png";
            }

            return false;
        }

        function HideShowSales() {

            if (myCustomer == null)
                return false;

            if (document.getElementById("<%=P5sImgSales.ClientID%>").src.indexOf("icon/SalesAmount_Outlet.png") >= 0) //set hide
            {
                document.getElementById("<%=P5sImgSales.ClientID%>").src = "icon/UnSelect_SalesAmount_Outlet.png";
                myCustomer.hideSales();
            }
            else   // set show        
            {
                myCustomer.showSales();
                document.getElementById("<%=P5sImgSales.ClientID%>").src = "icon/SalesAmount_Outlet.png";
            }

            return false;
        }

        function HideShowVisitedSales() {

            if (myCustomer == null)
                return false;

            if (document.getElementById("<%=P5sImgVisitedSales.ClientID%>").src.indexOf("icon/SalesAmount_Visit_Outlet.png") >= 0) //set hide
            {
                document.getElementById("<%=P5sImgVisitedSales.ClientID%>").src = "icon/UnSelect_SalesAmount_Visit_Outlet.png";
                myCustomer.hideVisitedSales();
            }
            else   // set show        
            {
                myCustomer.showVisitedSales();
                document.getElementById("<%=P5sImgVisitedSales.ClientID%>").src = "icon/SalesAmount_Visit_Outlet.png";
            }

            return false;
        }

        function HideShowNoVisited() {

            if (myCustomer == null)
                return false;

            if (document.getElementById("<%=P5sImgNoVisited.ClientID%>").src.indexOf("icon/Outlet.png") >= 0) //set hide
            {
                document.getElementById("<%=P5sImgNoVisited.ClientID%>").src = "icon/UnSelect_Outlet.png";
                myCustomer.hideNoVisited();
            }
            else   // set show        
            {
                myCustomer.showNoVisited();
                document.getElementById("<%=P5sImgNoVisited.ClientID%>").src = "icon/Outlet.png";
            }

            return false;
        }


    </script>

    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:HiddenField ID="P5sLocaltionCountry" runat="server" />
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPnlMain" runat="server">
                    <div id="container" style="white-space: nowrap; display: block; position: absolute;">
                    </div>
                    <div id="floating-panel">
                        <div id="textcontant">
                        </div>
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
                                    Width="300px" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                                    <Panes>
                                        <ajaxToolkit:AccordionPane ID="P5sAccPnlCoverage" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <div style="font-size: medium; font-style: italic; color: Green; cursor: pointer;">
                                                    Visualizing Coverage on Map
                                                </div>
                                            </Header>
                                            <Content>
                                                <br />
                                                <table cellpadding="0" cellspacing="2" border="0" width="300px">
                                                    <tr hidden="hidden">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">By
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
                                                    <tr hidden="hidden">
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr id="P5sTrRegion">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            Region <span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:DropDownList Width="40%" CssClass="input-sm" ID="P5sDDLRegion" AutoPostBack="true" OnSelectedIndexChanged="P5sDDLRegion_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                                        <asp:TextBox ID="P5sTxtRegionCD" runat="server" CssClass="TextBox" Width="290px" Visible="false"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrArea">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <br />Area<span style="color: Red; white-space: nowrap;"> * </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:DropDownList Width="40%" CssClass="input-sm" ID="P5sDDLArea" AutoPostBack="true" OnSelectedIndexChanged="P5sDDLArea_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                                        <asp:TextBox ID="P5sTxtAreaCD" runat="server" CssClass="TextBox" Width="290px" Visible="false"></asp:TextBox>
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
                                                                            <asp:TextBox ID="P5sTxtDistributorCD" runat="server" CssClass="TextBox" Width="290px" Visible="false"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrSales">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <br />
                                                            DSR - NVBH<span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:DropDownList Width="95%" CssClass="input-sm" ID="P5sDDLSales" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                                        <select class="input-sm" style="width: 95%;" id="selDSR" Visible="false" runat="server" multiple="true" onserverchange="selDSR_ServerChange" onclick="__doPostBack()">
                                                                        </select>
                                                                        <asp:TextBox ID="P5sTxtSalesCD" runat="server" AutoPostBack="true" CssClass="TextBox" Width="290px" Visible="false"></asp:TextBox>
                                                                        <%--   <%=P5sActSales.L5sShowAddAll("P5sTxtSalesCD_Add")%>
                                                                        <%=P5sActSales.L5sShowRemoveAll("P5sTxtSalesCD_Remove")%>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrASM" hidden="hidden">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">ASM<span style="color: Red; white-space: nowrap;"> * </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <div style="width: 290px; min-width: 290px;">
                                                                            <asp:TextBox ID="P5sTxtASM" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                            <%=P5sActASM.L5sShowAddAll("P5sTxtASM_Add")%>
                                                                            <%=P5sActASM.L5sShowRemoveAll("P5sTxtASM_Remove")%>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrSupervisor" hidden="hidden">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">CDSs - Giám sát <span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:TextBox ID="P5sTxtSupervisorCD" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                        <%=P5sActSupervisor.L5sShowAddAll("P5sTxtSupervisorCD_Add")%>
                                                                        <%=P5sActSupervisor.L5sShowRemoveAll("P5sTxtSupervisorCD_Remove")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrWorkingRoutes" hidden="hidden">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top;">
                                                            Working date - Routes<span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <select class="input-sm" style="width: 95%;" id="selRoute" runat="server" multiple="true">
                                                                        </select>
                                                                        <asp:TextBox ID="P5sTxtRouteCD" AutoPostBack="true" runat="server" CssClass="TextBox" Width="290px" Visible="false"></asp:TextBox>
                                                                        <%-- <%=P5sActRoute.L5sShowAddAll("P5sTxtRouteCD_Add")%>
                                                                        <%=P5sActRoute.L5sShowRemoveAll("P5sTxtRouteCD_Remove")%>--%>
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
                                                                    <br />
                                                                    <td style="padding: 0">From date:
                                                                        <asp:TextBox ID="P5sTxtFromDate" runat="server" CssClass="input-sm" Width="70px" ReadOnly="True"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="P5sTxtFromDate"
                                                                            Format="dd-MM-yyyy">
                                                                        </ajaxToolkit:CalendarExtender>
                                                                        to date:
                                                                        <asp:TextBox ID="P5sTxtToDate" runat="server" CssClass="input-sm" Width="70px" ReadOnly="True"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="P5sTxtToDate"
                                                                            Format="dd-MM-yyyy">
                                                                        </ajaxToolkit:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <br />
                                                            <asp:Button ID="P5sLbtnLoad" CausesValidation="true" OnClick="P5sLbtnLoad_Click" Text="Display"
                                                                runat="server"></asp:Button>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <asp:Panel ID="P5sPnlSearch" Visible="false" runat="server">
                                                        <tr>
                                                            <td colspan="2">
                                                                <table cellpadding="0" cellspacing="2" border="1" width="275px" style="border-collapse: collapse; border: 1px solid black;">
                                                                    <tr>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">
                                                                            <asp:Image ID="P5sImgVisited" ImageUrl="~/icon/Visited_Outlet.png" runat="server" />
                                                                            <asp:LinkButton ID="P5sLbtnVisited" runat="server" OnClientClick="return HideShowVisited()">Visited but not sold</asp:LinkButton>
                                                                        </td>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                            <asp:Label ID="P5sLblVisited" runat="server" Text="0"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">
                                                                            <asp:Image ID="P5sImgSales" ImageUrl="~/icon/SalesAmount_Outlet.png" runat="server" />
                                                                            <asp:LinkButton ID="P5sLbtnSales" runat="server" OnClientClick="return HideShowSales()">Not visited but sold </asp:LinkButton>
                                                                        </td>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                            <asp:Label ID="P5sLblSales" runat="server" Text="0"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">
                                                                            <asp:Image ID="P5sImgVisitedSales" ImageUrl="~/icon/SalesAmount_Visit_Outlet.png"
                                                                                runat="server" />
                                                                            <asp:LinkButton ID="P5sLbtnVisitedSales" runat="server" OnClientClick="return HideShowVisitedSales()">Visited and sold </asp:LinkButton>
                                                                        </td>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                            <asp:Label ID="P5sLblVisitedSales" runat="server" Text="0"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">
                                                                            <asp:Image ID="P5sImgNoVisited" ImageUrl="~/icon/Outlet.png" runat="server" />
                                                                            <asp:LinkButton ID="P5sLbtnNoVisit" runat="server" OnClientClick="return HideShowNoVisited()">Not visited and not sold</asp:LinkButton>
                                                                        </td>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                            <asp:Label ID="P5sLblNoVisit" runat="server" Text="0"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <hr />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                    <tr>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left">
                                                                            <span style="font-size: medium; font-weight: bold">Summary :</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left;">
                                                                            <table cellpadding="0" cellspacing="2" border="1" width="275px" style="border-collapse: collapse; border: 1px solid black;">
                                                                                <tr>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">No of stores in Routes
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5sLblNoOfCustomer" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">Actual visited in period
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5sLblNoOfCustomerVisit" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">% Visited to total
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5sLblEffectivityCalls" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </asp:Panel>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                    </Panes>
                                </ajaxToolkit:Accordion>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:HiddenField ID="P5sHfRange" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
