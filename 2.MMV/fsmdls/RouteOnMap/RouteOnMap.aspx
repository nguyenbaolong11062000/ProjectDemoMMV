<%@ Page Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" Codebehind="RouteOnMap.aspx.cs"
    Inherits="MMV.fsmdls.RouteOnMap.RouteOnMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
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
       #divGPS
      {
          margin-top:10px;
          margin-left:10px;
      }    
       .ddlRoute {
           height:30px;
           font-size:14px;
       }
    </style>

    <script type="text/javascript" src="http://maps.google.com/maps/api/js"></script>

    <script src="../../libs/KeyGoogleMap.js"></script>

    <script src="P5sMapsLoadCustomer.js" type="text/javascript"></script>

    <script type="text/javascript">
        function pageLoad(sender, args) {
              $(document).ready(function(){
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
            
            var PATH_ARROW_IMG= "icon/arrowOption.png";
            var PATH_ARROW_DOWN_IMG= "icon/arrowOptionDown.png";              
               
            var myCustomer = null;
        var shopVisited = null;

        // load map default
            
            function LoadMap(lat, long, salecode)
            {
                //clear variable
                 myMap = null;    
                 myCustomer = null; 
                shopVisited = null;
                var myLatlng = {lat: parseFloat(lat) , lng:  parseFloat(long)};
                        //set Height
                       //width: 860px;
                    myMap = null;    
                    durationDisplayStop = 0;
                    var windowWidth = $(window).width() ; //-330;
                    var windowHeight =$(window).height()- 10;
                    $('#container').css({'width':windowWidth - 10 ,'height':windowHeight });  
                    var LocationCountry = document.getElementById('<%= P5sLocaltionCountry.ClientID%>').value;
                    var split = LocationCountry.split(',');
                    var cairo = {lat: parseFloat(split[0]) , lng:  parseFloat(split[1])};
                    myMap = new google.maps.Map(document.getElementById('container'), {
                        scaleControl: true,
                        center: myLatlng,
                        zoom: 18
                    });                       
                              
                       myMap.addListener('dragend', function () {                   
                               if (myCustomer != undefined  && myCustomer.IsVisitedShow == false) {
                                     myCustomer.hideVisited();
                               }
                               
                               if (myCustomer != undefined  && myCustomer.IsSalesShow == false) {
                                     myCustomer.hideSales();
                               }
                                
                               if (myCustomer != undefined  && myCustomer.IsVisitedSalesShow == false) {
                                     myCustomer.hideVisitedSales();
                               }
                               
                                if (myCustomer != undefined  && myCustomer.IsNoVisitedShow == false) {
                                     myCustomer.hideNoVisited();
                               }
                });
                

                    google.maps.event.addListener(myMap.getStreetView(),'visible_changed',function(){
                        if(this.getVisible()){ 
                            action = function () {
                                var RangeValue = document.getElementById('<%= P5sHfRange.ClientID%>').value;
                                var CustNameList = "<h2>LIST OF ALL STORES IN RANGE OF " +RangeValue+ "M</h2>";
                                var CurrentPoint = "";
                                var ArrayCust = new Array();
                                CurrentPoint =  myMap.getStreetView().getPosition().toString();
                              
                                for(var i = 0; i < object2.length; i++)
                                {
                                    var distance = calcDistance(eval("new google.maps.LatLng" + CurrentPoint + ""),
                                                        eval("new google.maps.LatLng(" + object2[i].LatLng + ")"));
                                    if(parseInt(distance) <= parseInt(RangeValue))
                                    {
                                        var Cust = new Object();
                                        Cust.range = distance;
                                        Cust.name = object2[i].CustomerName;
                                        ArrayCust.push(Cust);
                                        //CustNameList += object2[i].CustomerName + "<br />";
                                    }
                                }
                                
                                ArrayCust.sort(function(a, b) {
                                    return parseFloat(a.range) - parseFloat(b.range);
                                });
                                for(var i = 0; i < ArrayCust.length; i++)
                                {
                                    CustNameList += ArrayCust[i].name + "<br />";
                                }
                                document.getElementById("textcontant").innerHTML= CustNameList; 
                            };
                            setTimeout(action, 2000);
                        }
                        else
                        document.getElementById("textcontant").innerHTML = "";
                });
                var zInd = 0;
                var maxZindex = google.maps.Marker.MAX_ZINDEX;               
                    zInd = (maxZindex + 1);
                var addMarker;
                addMarker = new google.maps.Marker({
                    draggable: false,
                    position: myLatlng,
                    map: myMap,
                    zIndex: zInd,
                    icon: saleMarker,
                    title: 'Saleman: ' + salecode
                });

                google.maps.event.addListener(myMap, 'zoom_changed', function() {

                    var pixelSizeAtZoom0 = 4; //the size of the icon at zoom level 0
                    var maxPixelSize = 80; //restricts the maximum size of the icon, otherwise the browser will choke at higher zoom levels trying to scale an image to millions of pixels

                    var zoom = myMap.getZoom();
                    var relativePixelSize = Math.round(pixelSizeAtZoom0*(zoom/2)); // use 2 to the power of current zoom to calculate relative pixel size.  Base of exponent is 2 because relative size should double every time you zoom in
                    
                    if(relativePixelSize > maxPixelSize) //restrict the maximum size of the icon
                        relativePixelSize = maxPixelSize;

                    //change the size of the icon
                    addMarker.setIcon(
                        new google.maps.MarkerImage(
                            addMarker.getIcon().url, //marker's same icon graphic
                            null,//size
                            null,//origin
                            null, //anchor
                            new google.maps.Size(relativePixelSize, relativePixelSize) //changes the scale
                        )
                    );
                    console.log(arrMarker);

                    for (var i = 0; i < arrMarker.length; i++) {
                        arrMarker[i].setIcon(
                            new google.maps.MarkerImage(
                            arrMarker[i].getIcon().url, //marker's same icon graphic
                            null,//size
                            null,//origin
                            null, //anchor  
                                new google.maps.Size(relativePixelSize - 5, relativePixelSize - 5) //changes the scale
                            )
                        );
                    }
                });
                myMap.controls[google.maps.ControlPosition.LEFT_TOP].push(divGPS);
                
            }
        var arrMarker = new Array();
        var saleMarker = new google.maps.MarkerImage(
            'icon/saleman.gif',
            new google.maps.Size(35,35), //size
            null, //origin
            null, //anchor
            new google.maps.Size(35,35) //scale
        );
                         
        function showShop(jsonShop)
        {        
              $(function(){
                  
                  myCustomer = new LoadCustomerRouteOnMap(myMap, jsonShop); 
                     //myMap.fitOverlays();          
              });
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
                    <div id="divGPS" class="floating-panel">
                        <asp:DropDownList runat="server" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged" 
                            CssClass="ddlRoute"
                            AutoPostBack="true"  ID="ddlRoute"></asp:DropDownList>
                    </div>                             
                </asp:Panel>
                <asp:HiddenField ID="P5sHfRange" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>
