<%@ Page Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" Codebehind="Tracking.aspx.cs"
    Inherits="MMV.Tracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">

    <script src="libs/KeyVietBanDoApi.js" type="text/javascript"></script>

    <script src="MapAPI.js" type="text/javascript"></script>

    <script src="P5sMaps.js" type="text/javascript"></script>

    <script type="text/javascript">
        
        function pageLoad(sender, args) {
              $(document).ready(function(){
               ShowOption();
                  $("#ImgBackForward").click(function(){    
                
                        
                     var src = $(this).attr('src');
                                
                     if(src.indexOf("images/back.png") >= 0)
                     {  
                        $(this).attr('src',"images/forward.png");
                        $("#TbLefPanel").animate({
                            left: "-280px"
                        });                          
                     
                     }
                     else
                     {
                         $(this).attr('src',"images/back.png");
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
            var ACCORDION_CLIENT =  "<%=P5sMyAccordion.ClientID%>";
            var PATH_ARROW_IMG= "icon/arrowOption.png";
            var PATH_ARROW_DOWN_IMG= "icon/arrowOptionDown.png";              
            var PATH_ICON_OUTLET = "icon/Outlet.png";  
            var PATH_ICON_OUTLET_VISITED = "icon/Visited_Outlet.png";  
            
            var myTracking = null;
            var P5sSpeedValue = 0;
    
            //hàm khỏi tạo bản đồ - hàm này phải được gọi đầu tiên khi muốn xử dụng bản đồ
            function LoadMap()
            {
                //clear variable
                 myMap = null;    
                 myTracking = null; 
                 durationDisplayStop = 0;
                 
                 if (VBrowserIsCompatible())
                 {
                        //set Height
                       //width: 860px;
                       var windowWidth = $(window).width() ; //-330;
                       var windowHeight =$(window).height()- 20;
                       $('#container').css({'width':windowWidth - 20 ,'height':windowHeight });            
                       document.getElementById(ACCORDION_CLIENT).style.height= (windowHeight - 25)+ "px";           
                       myMap = new VMap(document.getElementById(CONTAINER_ID)); //khỏi tạo bản đồ truyền vào ID div chứa nơi hiển thị bản đồ
                       var pt = new VLatLng(16.2568673,108.610839); //khỏi tạo point với kinh độ và vĩ độ
                       myMap.setCenter(pt,6); //thiết lập vị trí trung tâm khi tạo bản đồ
                       //tạo 1 số control cho bản đồ tham khảo link sau: http://www.vietbando.com/maps/api/Reference.aspx
                       myMap.addControl(new VLargeMapControl(), new VControlPosition(V_ANCHOR_TOP_RIGHT, new VSize(50, 20)));
                       myMap.addControl(new MyControl());
                       myMap.addControl(new VScaleControl());
                       myMap.addControl(new VOverviewMapControl());    
                      
                       //đoan code khỏi tạo tính năng replay lại đường đi của NVBH
                       $( "#divRange" ).slider({
                                step: 1,
                                min: 0,
                                max: 60,
                                change: function( event, ui )                                {
                                    var value = $( "#divRange" ).slider( "option", "value" );
                                    document.getElementById("divValueRange").innerHTML= "Time "  + value+ "'";
                                    durationDisplayStop = value;
                                    if(myTracking != null )
                                        myTracking.ShowStop();
                                }
                        });
                        
                        $( "#P5sDivSpeedRange" ).slider({
                                step: 0.1,
                                min: 0.0,
                                max: 1.0,
                                value: 0.1,
                                change: function( event, ui )                                {
                                    var value = $( "#P5sDivSpeedRange" ).slider( "option", "value" );
                                    value = 1 - value;
                                    //document.getElementById("P5sDivSpeedValue").innerHTML= value + "s";
                                    P5sSpeedValue = value;
                          }
                        });
                        $( "#P5sDivSpeedRange" ).css('background', 'rgb(113, 73, 4)');
                        $( "#P5sDivSpeedRange" ).css('height', '0.2em');    
                        $( "#P5sDivSpeedRange .ui-state-default" ).css('height', '0.7em');    
                        $( "#P5sDivSpeedRange .ui-state-default" ).css('width', '0.6em'); 
                        $( "#P5sDivSpeedRange .ui-state-default" ).css('background', 'rgb(219, 142, 10)');   
                          //  .ui-slider-horizontal  .ui-state-default  {background:#edea07;  height: 0.8em;}
                          
                    //hàm này sẽ thực thi khi người dùng kết thúc về duy chuyển bản đồ qua lại, lên xuống hoặc phóng to thu nhỏ
                    //mục đích là khi người dùng duy chuyển thì toàn bộ icon trên bản đồ sẽ hiển thị ra xử dụng hàm này để ẩn hiện icon 1 cách logic
                    //
                          
                    VEvent.addListener(myMap, 'moveend', function () {
                 
                    if ( myTracking != undefined )
                    {
                         if(myTracking.StatusRePlay == false)
                        {
                            //xem code P5sMaps.js để biết thêm chi tiết
                            myTracking.ShowStart();    
                            myTracking.ShowPoint();   
                            myTracking.ShowRouteText();  
                            myTracking.ShowRouteTextFS(); 
                            myTracking.ShowRouteLine();  
                            myTracking.ShowShops(); 
                            myTracking.ShowStop(); 
                            myTracking.ShowRePlayPoints();
                            
                        }  
                        else 
                        {
                            //nếu user đang xử dụng tính năng replay thì hiển thị 1 số thông tin như bên dưới 
                            myTracking.ShowStart();      
                            myTracking.ShowRouteTextFS(); 
                            myTracking.ShowRouteLine();  
                            myTracking.ShowShops(); 
                            myTracking.ShowStop(); 
                            
                        }              
                    }
                   
                });
                
                
                VEvent.addListener(myMap, 'zoomend', function () 
                {
                    if(myTracking != undefined && myTracking.StatusRePlay == true)
                    {
                        myTracking.P5sShowRePlay(myTracking.SalCD);
                    }  
                });
            }            
        }
            
                
         //hàm chính để khỏi tạo thông tin tracking để hiển thị lên bản đồ   
         //objectTrackingOfSales: thông tin về tuyến đường đi
         //objectShop: thông tin về cửa hàng (khách hàng)
        function CreatedCTracking(objectTrackingOfSales,objectShop)
        {        
              $(function(){
               // alert(objectShop);
                     myTracking = new CTracking(myMap,objectTrackingOfSales, objectShop);         
                     myMap.fitOverlays();          
              });
        }
        
   var StatusRePlay = false;  
   
   //hàm khỏi tạo control Time: control này để use chọn lưa việc hiển thị các điểm đừng theo thời gian chọn
   //vd: nếu user chọn 10' thì hệ thống sẽ chỉ hiển thị các điểm dừng có thời gian dừng lớn hơn hoặc bằng 10'
   function MyControl()
    {
        this.initialize = initialize;
        this.getDefaultPosition = getDefaultPosition;
        this.vType = vType;

          function initialize(map)
            {
                ////
                this.id = 'divContainer';
                var div_container = document.createElement("div");
                div_container.id = 'divContainer';
                div_container.style.position = 'relative';
                div_container.style.float = "right";  
                div_container.style.margin = "20px";  
                div_container.style.marginTop =  $(window).height()- 60 + "px";
                //////       
                var div_range = document.createElement("div");
                div_range.id = 'divRange';
                div_range.style.width = "300px";  
                div_range.style.cursor = 'pointer';
                div_range.style.float = "left";     
                div_container.appendChild(div_range);
                /////
                //////       
                var div_value_range = document.createElement("b");
                div_value_range.style.color = "#1a9b1f";     
                div_value_range.id = 'divValueRange';
                div_value_range.style.cursor = 'pointer';
                div_value_range.style.float = "left";    
                div_value_range.style.marginTop = "-7px";
                div_value_range.style.marginLeft = "18px";
                div_container.appendChild(div_value_range);
                div_value_range.appendChild(document.createTextNode("Time 0'"));
            
                
                map.getContainer().appendChild(div_container);
            }
        


        function getDefaultPosition()
        {
            var defaultpos = new VControlPosition(V_ANCHOR_TOP_RIGHT, new VSize(0, 0));
            return defaultpos;
        }

        function vType()
        {
            return 'MyControl';
        }
    }
    MyControl.prototype = new VControl();
    
        //hàm này được xử dụng để ẩn hiện các pannel chọn xem thông tin theo các loại khác nhau
        // trang này cho phép xem thông tin tracking theo ASM, SUP, NVBH
        // mỗi 1 loại sẽ có cách thức filter khác nhau 
        // tùy vào loại chọn mà sẽ hiển thị thông tin tương 
        function ShowOption()
        {
            var Region = document.getElementById("P5sTrRegion");
            var Area = document.getElementById("P5sTrArea");
            var TrDistributor = document.getElementById("P5sTrDistributor");  
            var TrSales = document.getElementById("P5sTrSales");
            var Supervisor = document.getElementById("P5sTrSupervisor");
            var ASM = document.getElementById("P5sTrASM");

            var P5DdlViewOption = document.getElementById('<%=P5sDdlViewOption.ClientID%>');
            var ValueP5DdlViewOption = P5DdlViewOption.options[P5DdlViewOption.selectedIndex].value;
            if(ValueP5DdlViewOption == 1)
            {
                TrDistributor.style.display = "block";
                TrSales.style.display = "block";
                ASM.style.display = "none";
                Supervisor.style.display = "none";
            }
            else if(ValueP5DdlViewOption == 2)
            {
                TrDistributor.style.display = "none";
                TrSales.style.display = "none";
                ASM.style.display = "none";
                Supervisor.style.display = "block";
            }
            else if(ValueP5DdlViewOption == 3)
            {
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
                    <div id="container" style="white-space: nowrap; display: block; position: fixed;">
                    </div>
                    <table id="TbLefPanel" cellpadding="0" cellspacing="0" border="0" width="300px" style="z-index: 200;
                        position: absolute; background-color: White">
                        <tr>
                            <td style="width: 1%; white-space: nowrap; vertical-align: top">
                                <div style="padding-left: 280px">
                                    <img alt="" id="ImgBackForward" src="images/back.png" style="cursor: pointer" />
                                </div>
                                <ajaxToolkit:Accordion ID="P5sMyAccordion"  runat="Server" SelectedIndex="-1"
                                    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                                    ContentCssClass="accordionContent" FadeTransitions="true" TransitionDuration="250"
                                    FramesPerSecond="40" AutoSize="Fill" Width="300px" RequireOpenedPane="false"
                                    SuppressHeaderPostbacks="true">
                                    <Panes>
                                        <ajaxToolkit:AccordionPane ID="P5sAccPnlTracking" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <div style="font-size: medium; font-style: italic; color: Green; cursor: pointer;">
                                                    <%=MMV.L5sMaster.L5sLangs["Search"]%>
                                                </div>
                                            </Header>
                                            <Content>
                                                <table cellpadding="0" cellspacing="2" border="0" width="300px">
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <%=MMV.L5sMaster.L5sLangs["By"]%>
                                                                        <br />
                                                                        <asp:DropDownList ID="P5sDdlViewOption" runat="server" AutoPostBack="true" OnSelectedIndexChanged="P5sDdlViewOption_SelectedIndexChanged">
                                                                            <%--<asp:ListItem Text="DSRs - NVBH" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="CDSs - Giám sát" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="ASMs - Quản lý khu vực" Value="3"></asp:ListItem>--%>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <%=MMV.L5sMaster.L5sLangs["Date"]%> <span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="5px" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:TextBox ID="P5sTxtDay" Width="80px" CssClass="textbox" runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="P5sTxtDay"
                                                                            Format="dd-MM-yyyy">
                                                                        </ajaxToolkit:CalendarExtender>
                                                                    </td>
                                                                    <td>
                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td style="padding: 0">
                                                                                    <%=MMV.L5sMaster.L5sLangs["From"]%>:
                                                                                    <asp:DropDownList ID="P5sDdlFromHH" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:DropDownList ID="P5sDdlFromMM" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <table cellpadding="5px" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                                            <tr>
                                                                                <td style="padding: 0">
                                                                                    <%=MMV.L5sMaster.L5sLangs["To"]%>:
                                                                                    <asp:DropDownList ID="P5sDdlToHH" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:DropDownList ID="P5sDdlToMM" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrRegion">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <%=MMV.L5sMaster.L5sLangs["Region"]%> <span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:TextBox ID="P5sTxtRegion" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrArea">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <%=MMV.L5sMaster.L5sLangs["Area"]%><span style="color: Red; white-space: nowrap;"> * </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:TextBox ID="P5sTxtArea" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrDistributor">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <%=MMV.L5sMaster.L5sLangs["Distributor"]%><span style="color: Red; white-space: nowrap;"> * </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <div style="width: 290px; min-width: 290px;">
                                                                            <asp:TextBox ID="P5sTxtDistributor" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrSales">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <%=MMV.L5sMaster.L5sLangs["DSR - NVBH"]%><span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:TextBox ID="P5sTxtSales" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                        <%=P5sActSales.L5sShowAddAll("P5sTxtSales_Add")%>
                                                                        <%=P5sActSales.L5sShowRemoveAll("P5sTxtSales_Remove")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="P5sTrASM">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <%=MMV.L5sMaster.L5sLangs["ASM"]%><span style="color: Red; white-space: nowrap;"> * </span>:
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
                                                    <tr id="P5sTrSupervisor">
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            <%=MMV.L5sMaster.L5sLangs["CDSs - Giám sát"]%> <span style="color: Red; white-space: nowrap;">* </span>:
                                                            <table cellpadding="0" cellspacing="0" border="0" width="290px">
                                                                <tr>
                                                                    <td style="padding: 0">
                                                                        <asp:TextBox ID="P5sTxtSupervisor" runat="server" CssClass="TextBox" Width="290px"></asp:TextBox>
                                                                        <%=P5sActSupervisor.L5sShowAddAll("P5sTxtSupervisor_Add")%>
                                                                        <%=P5sActSupervisor.L5sShowRemoveAll("P5sTxtSupervisor_Remove")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">
                                                            &nbsp;
                                                        </td>
                                                        <tr>
                                                            <td style="vertical-align: top; white-space: nowrap; padding-left: 50px">
                                                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnLoadTracking)%>
                                                                <asp:LinkButton ID="P5sLbtnLoadTracking" CausesValidation="true" OnClick="P5sLbtnLoadTracking_Click"
                                                                    runat="server"><%=MMV.L5sMaster.L5sLangs["Display"]%></asp:LinkButton>
                                                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnLoadTracking)%>
                                                            </td>
                                                            <td style="height: 26px; white-space: nowrap; width: 99%; vertical-align: top">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                       
                                                        <tr>
                                                            <td colspan="2">
                                                                <hr />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                            </td>
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
                                                            </td>
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
                                                                          <%=MMV.L5sMaster.L5sLangs["slow"]%>  </div>
                                                                        <div style="float: left; width: 140px; margin-left: 15px;margin-top:8px" id="P5sDivSpeedRange">
                                                                        </div>
                                                                        <%-- <div style="float: left; margin-left: 20px" id="P5sDivSpeedValue">
                                                                        0.5s</div>--%>
                                                                        <div style="float: left; margin-left: 5px">
                                                                           <%=MMV.L5sMaster.L5sLangs["fast"]%> </div>
                                           
                                                                </div>
                                                            </td>
                                                        </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <ajaxToolkit:AccordionPane ID="AccordionPaneTrackingVisit" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <hr />
                                                <span style="font-size: medium; font-style: italic; color: Green; cursor: pointer;
                                                    text-decoration: none" onclick="javascript:window.location.href = '/TrackingVisitUnVisit.aspx';">
                                                    <%=MMV.L5sMaster.L5sLangs["Visualizing Coverage on Map"]%> </span>
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
                                                            <asp:LinkButton ID="P5sLbtnTrackingVisit" OnClick="P5sLbtnTrackingVisit_Click" runat="server"><%=MMV.L5sMaster.L5sLangs["Go to page"]%></asp:LinkButton>
                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnTrackingVisit)%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <ajaxToolkit:AccordionPane ID="P5sAccPnlDCoverageProgressSettingAndReports" runat="server"
                                            HeaderCssClass="accordionHeader" ContentCssClass="accordionContent">
                                            <Header>
                                                <hr />
                                                <span style="font-size: medium; font-style: italic; color: Green; cursor: pointer;
                                                    text-decoration: none" onclick="javascript:window.location.href = '/Report/CustomerList.aspx';">
                                                    <%=MMV.L5sMaster.L5sLangs["Setting and Reports"]%> </span>
                                            </Header>
                                            <Content>
                                                <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                    <tr>
                                                        <td style="vertical-align: top; white-space: nowrap;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; padding-left: 140px; display: none;">
                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sSettingAndReports)%>
                                                            <asp:LinkButton ID="P5sSettingAndReports" OnClick="P5sSettingAndReports_Click" runat="server"><%=MMV.L5sMaster.L5sLangs["Setting and Reports"]%></asp:LinkButton>
                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sSettingAndReports)%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
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
