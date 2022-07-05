<%@ Page Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" Codebehind="COVERAGEV3.aspx.cs"
    Inherits="MMV.fsmdls.mmcv4.COVERAGEV3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">

    <script src="P5sMapsPHV3.js" type="text/javascript"></script>

    <script src="../../libs/KeyGoogleMap.js" type="text/javascript"></script>
    
    <script src="../../libs/markerwithlabel.js" type="text/javascript"></script>

    <link href="CSS/fscstyles_181103.css" rel="stylesheet" />

    <script type="text/javascript">
        
        function pageLoad(sender, args) {
              $(document).ready(function(){
                  $("#ImgBackForward").click(function(){    
                
                        
                     var src = $(this).attr('src');
                                
                     if(src.indexOf("images/back.png") >= 0)
                     {  
                        $(this).attr('src',"images/forward.png");
                        $("#TbLefPanel").animate({
                            left: "-320px"
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
        var MAX_HEIGHT_OFFSET = 43;
        var MIN_HEIGHT_OFFSET = 16;
        var END_IMG = "EndImg";
        var START_IMG = "StartImg";
        var SELECT_IMG  = "ImgSelect";
        var PATH_ARROW_IMG= "icon/arrowOption.png";
        var PATH_ARROW_DOWN_IMG= "icon/arrowOptionDown.png";   
        var ACCORDION_CLIENT =  "<%=P5sMyAccordion.ClientID%>";
        var CONTAINER_ID = 'container';
        var PATH_DISTRIBUTOR_INDIRECT = "/icon/SD.png";
        var PATH_DISTRIBUTOR_DIRECT = "/icon/ND.png";
        var PATH_DISTRIBUTOR_DIRECT_SDN = "/icon/SDN.png";
        var PATH_CWS_IMG = "/icon/CoveredWS.png";
        var PATH_WWS_IMG = "/icon/CoveredWithoutWS.png";
        var PATH_UC_IMG = "/icon/Uncovered.png";
        var BOUNDS = new google.maps.LatLngBounds();
         
        
        
        var map;
        var coverageWS,coverageWithouWS, uncoverage,distributorDirect,distributorInDirect,distributorSD;
        var markerTmp,markerMoveTmp , markerStart, markerEnd, distributorSelectMove;    
        var banking, atm,hospital,postOffice,school,market;
        var VPolylines = new Array();
        var markerPoints = new Array();            
        var pointCDs = new Array();  
        var centralPoints = new Array();  
        var distributorNetworkPoint;
        var centralPoint;
        var polygonWS,polygonWWS,polygonUnCover,polygonProvince,polygonNoSatisfy, polygonDistributorNetworkUncover;         
        var arrDistributorNetwork = new Array();
        var markerViewDistance, markerStartViewDistance;
       
         var letters = new Array();
         letters[letters.length] = "_A";
         letters[letters.length] = "_B";
         letters[letters.length] = "_C";
         letters[letters.length] = "_D";
         letters[letters.length] = "_E";
         letters[letters.length] = "_F";
         letters[letters.length] = "_G";
         letters[letters.length] = "_H";
         letters[letters.length] = "_I";
         letters[letters.length] = "_J";
         letters[letters.length] = "_K";
         letters[letters.length] = "_L";
         letters[letters.length] = "_M";
      
        
        function loadMap()
        {
             //clear variable
            map = null;            
            coverageWS  = coverageWithouWS =  uncoverage = distributorDirect = distributorInDirect = distributorSD = null;
            markerTmp = markerStart = markerEnd  = distributorSelectMove = null;    
            banking  = atm  = hospital = postOffice = school = null;
            markerViewDistance = markerStartViewDistance = null;
            VPolylines = new Array();
            markerPoints = new Array();            
            pointCDs = new Array();  
            centralPoints = new Array();  
            distributorNetworkPoint = null;
            centralPoint = null;
            polygonWS = polygonWWS = polygonUnCover = polygonProvince  = polygonNoSatisfy = null; 
            polygonDistributorNetworkUncover = null;        
            arrDistributorNetwork = new Array();
                 
            
           //set Height
           //width: 860px;
           var windowWidth = $(window).width() ; //-330;
           var windowHeight =$(window).height()-80;
           $('#container').css({'width':windowWidth - 20 ,'height':windowHeight });            
           document.getElementById(ACCORDION_CLIENT).style.height= (windowHeight - 25)+ "px";
            
           document.getElementById("MainDivFindPath").style.height  = (windowHeight - 245) + "px";
             
           document.getElementById("<%=PanelScroll.ClientID%>").setAttribute("height", (windowHeight - 330) + "px");            
         

                                    
//               map = new VMap(document.getElementById(CONTAINER_ID));
//               var pt = new VLatLng(16.2568673,108.610839);
//               map.setCenter(pt,6);
//               map.addControl(new VLargeMapControl(), new VControlPosition(V_ANCHOR_TOP_RIGHT, new VSize(50, 20)));
//               map.addControl(new VScaleControl());
//               map.addControl(new VOverviewMapControl());
//               markerPoints = new Array();
            var LocationCountry = document.getElementById('<%= P5sLocaltionCountry.ClientID%>').value;
            var split = LocationCountry.split(',');
            var cairo = {lat: parseFloat(split[0]) , lng:  parseFloat(split[1])};
            map = new google.maps.Map(document.getElementById(CONTAINER_ID), {
                scaleControl: true,
                center: cairo,
                zoom: 6
            }); 
       
           
               map.addListener('dragend', function () {
              
//                    HideShowMarket(coverageWS);
//                    HideShowMarket(coverageWithouWS);
//                    HideShowMarket(uncoverage);
//                    HideShowMarket(distributorDirect);
//                    HideShowMarket(distributorInDirect);
//                    HideShowMarket(distributorSD);
//                    HideShowMarket(polygonWS);
//                    HideShowMarket(polygonWWS);
//                    HideShowMarket(polygonUnCover);
//                    HideShowMarket(polygonNoSatisfy);
//                    HideShowMarket(distributorNetworkPoint);
//                    HideShowMarket(centralPoint);

            });
            
        }
        
        function HideShowMarket(object)
        {
                if ( object != undefined &&  object.IsMarkerShow == false) {
                          object.hide();    
                }   
        }
        
        /* B
           Polygon: WS, WWS, C
        */
       
   
        
        function AddPWS(v)
        {        
            polygonWS = new CPolygon(map,v,'#7977F4',0,'#CD2D24');                     
        }
        
       
        function AddPWWS(v)
        {
            polygonWWS = new CPolygon(map,v,'#7977F4',0,'#00FF00');  
        }
        
        function AddPUnCover(v)
        {        
            polygonUnCover = new CPolygon(map,v,'#7977F4',0,'#F2AB08');  
        }
        
        
        function AddNoSatisfy(v)
        {        
            polygonNoSatisfy = new CPolygon(map,v,'#7977F4',0,'#0099FF');              
        }
        
        function AddPolygonDistributorNetworkUncover(v)
        {        
            polygonDistributorNetworkUncover = new CPolygon(map,v,'#7977F4',1,'#ffffff',0.6);              
        }
        
        
        /* E
           Polygon: WS, WWS, C
        */
        
        
        
        /*B DISTRIBUTOR NETWORK
        */
        
          
        function AddDistributorNetworkPoint(v)
        {           
           distributorNetworkPoint = new ClassMarkerLongTude(map,"/icon/longTude.png",v );
           distributorNetworkPoint.hide();    
        }
        
        function AddCentralPoint(v)
        {           
           centralPoint = new ClassMarkerCentralPoint(map,"/icon/centralPoint.png",v );
           centralPoint.hide();    
        }
        
        
        function AddDistributorNetwork(v,type,distributorCD,areaCD,color)
        {   

             var arr = v.split("-");
             var flag = false;
             
            //arrDistributorNetwork    
            //Ki?m tra n?u trong 1 vùng mà có nhiêu NPP ???c add                                   
              for(var k = 0; k < arrDistributorNetwork.length ; k++)
              {
                   if(arrDistributorNetwork[k].AreaCD == areaCD)
                   {
                      flag = true;
                      break;
                   }
              }
               
         
             for(var j = 0; j < arr.length; j++){ 
                   
                   //var polygon = new VPolygon(eval(arr[j]),'#7977F4',0,color, 1);  
                   var polygon = new google.maps.Polygon({
                        paths: eval(arr[j]),
                        draggable: false,
                        editable: false,
                        strokeColor: '#7977F4',
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillColor: color,
                        fillOpacity: 0.35
                    });    
                   var cDistributorNetworkPolygon; 
                   if(flag == true) //?? t?n t?i NPP khác cùng vùng v?i NPP ?ang add => ch? add qu?n l? không add Overlay
                       cDistributorNetworkPolygon = new  CDistributorNetworkPolygon(polygon,type,distributorCD,areaCD,color,false);                       
                   else
                   {
                   polygon.setMap(map);
                      // map.addOverlay(polygon);   
                       cDistributorNetworkPolygon  = new  CDistributorNetworkPolygon(polygon,type,distributorCD,areaCD,color,true); 
                   }
                 arrDistributorNetwork.push(cDistributorNetworkPolygon);    
             }                     
             
        }
        
        function AddDistributorNetworkDirect(str)
        {
             distributorDirect = new ClassMarkerDistributorNetworkDirect(map, PATH_DISTRIBUTOR_DIRECT, str);
            
        }      
        
        function AddDistributorNetworkInDirect(str)
        {
             distributorInDirect = new ClassMarkerDistributorNetworkInDirect(map, PATH_DISTRIBUTOR_INDIRECT, str);
            
        }      
                
        function AddPolyline(v)
        {            
             var arr = v.split("-");
             for(var j = 0; j < arr.length; j++){ 
                //var polyline = new VPolyline(eval(arr[j]), '#7977F4', 2, '#7977F4', 1);     
                var polyline = new google.maps.Polygon({
                    paths: eval(arr[j]),
                    draggable: false,
                    editable: false,
                    strokeColor: '#7977F4',
                    strokeOpacity: 0.8,
                    strokeWeight: 2,
                    fillColor: '#7977F4',
                    fillOpacity: 0.35
                });   
                map.addOverlay(polyline); 
             }       
        }
        
        
         
        /*E DISTRIBUTOR NETWORK
        */
        
        
        
        /* B
           POIs
        */
            function AddBanking(str)
            {           
                banking = new CPOIS(map,str ,"/icon/Bank.png");            
                 
            }
            
            function AddATM(str)
            {           
                atm = new CPOIS(map,str ,"/icon/ATM.png");            
                 
            }
            
            function AddHospital(str)
            {           
                hospital = new CPOIS(map,str ,"/icon/Hospital.png");            
                 
            }
                
                 
            function AddPostOffice(str)
            {           
                postOffice = new CPOIS(map,str ,"/icon/postoffice.png");            
                 
            }
            
                   
            function AddSchool(str)
            {           
                school = new CPOIS(map,str ,"/icon/school.png");            
                 
            }
            
            function AddMarket(str)
            {           
                market = new CPOIS(map,str ,"/icon/Market.png");            
                 
            }
            
         /* E
            POIs
        */
        
        
        function AddCoverageWS(str)
        {
       
            coverageWS = new ClassMarkerCoveraged(map, PATH_CWS_IMG, str);             
           
        }
        
        function AddCoverageWithoutWS(str)
        {
             coverageWithouWS = new ClassMarkerCoveraged(map, PATH_WWS_IMG, str);
        }
        
        function AddUnCoverage(str)
        {
             uncoverage = new ClassMarkerUnCoveraged(map, PATH_UC_IMG, str);
        }
        
        
        function AddDistributorDirect(str)
        {
             distributorDirect = new ClassMarkerDistributor(map, PATH_DISTRIBUTOR_DIRECT, str);
        }
        
        
        function AddDistributorInDirect(str)
        {
             distributorInDirect = new ClassMarkerDistributorInDirect(map, PATH_DISTRIBUTOR_INDIRECT, str);
        }
        
        function FitOverlays()
        {
            map.fitBounds(BOUNDS);
        }
        
        
        
        
        function FitOverlays2(latLng){  
        
        
//        for(var i = 0; i<latLngList.length; i++){
//        alert("Ph?n t? th?" +i + ":"+ latLngList[i]);
//        }
//       // alert(latLngList);
        var latLng2 = latLng.split(",");        
  
        var myLatLng = new google.maps.LatLng(latLng2[0], latLng2[1]);
        
        var bounds = new google.maps.LatLngBounds();
        try{
                 bounds.extend(myLatLng);
     
        }catch(err){
             alert("Cannot bound Latitude_Longitude!!");
        }
             map.fitBounds(bounds); 
             map.setCenter(myLatLng);
             map.setZoom(10);
        }
      
        
        function AddDistributorSD(str)//hinh nhu ham nay ko xai toi
        {
             distributorSD = new ClassMarkerDistributor(map, PATH_DISTRIBUTOR_INDIRECT, str);            
        }
        
         function HideShowCoverageWS() {
                  
            if (coverageWS.IsMarkerShow == true) 
            {   
                if(polygonWS != null)
                    polygonWS.hide();
                if(coverageWS != null)
                    coverageWS.hide();                
                document.getElementById("<%=P5sImgCoveredWS.ClientID%>").src  = "icon/CoveredWS_.png";
            }
            else           
            {   
                if(polygonWS != null)
                    polygonWS.show();
                    
                if(coverageWS != null)
                    coverageWS.show();  
                                 
                document.getElementById("<%=P5sImgCoveredWS.ClientID%>").src  = "icon/CoveredWS.png";
            }
                
             return false;                      
        }
        
         function HideShowCoverageWithoutWS() {
                  
            if (coverageWithouWS.IsMarkerShow == true) 
            {    
                 if(polygonWWS != null)
                     polygonWWS.hide(); 
                         
                 if(coverageWithouWS != null)       
                    coverageWithouWS.hide(); 
                     
                 document.getElementById("<%=P5sImgCoveredWithoutWS.ClientID%>").src  = "icon/CoveredWithoutWS_.png";
            }
            else           
            {
              if(polygonWWS != null)
                polygonWWS.show();
                
              if(coverageWithouWS != null)
               coverageWithouWS.show();
               document.getElementById("<%=P5sImgCoveredWithoutWS.ClientID%>").src  = "icon/CoveredWithoutWS.png";
            }
          
             return false;                      
        }
        
        function HideShowUncoverage() {
                
            if (uncoverage.IsMarkerShow == true) 
            { 
              if(polygonUnCover != null)
                polygonUnCover.hide();
                
              if(uncoverage != null)
                uncoverage.hide();  
                document.getElementById("<%=P5sImgUncovered.ClientID%>").src  = "icon/Uncovered_.png";
       
            }
            else           
            {
             if(polygonUnCover != null)
                polygonUnCover.show();
                
             if(uncoverage != null)  
                uncoverage.show();
                
                document.getElementById("<%=P5sImgUncovered.ClientID%>").src  = "icon/Uncovered.png";
      
            }
            
             return false;                      
        }
            
            
            
            
            
         function ClearPolyLinesRoute()
         {
             //Clear All VPolylines
            for(var i = 0; i < VPolylines.length; i++)
            {
                map.removeOverlay(VPolylines[i]);
            }
            VPolylines = new Array();
            document.getElementById("<%=PanelScroll.ClientID%>").style.display = 'none'; 
//            document.getElementById("DistanceNearestTownCommune").removeAttribute("checked");
//            document.getElementById("DistanceFromDistributor").removeAttribute("checked");
         }
           
            
        /* Thu?t toán permute - Begin */
          var count=0;
          var arrayResultPermute = new Array();
          function permute(pre,cur){            
                var len=cur.length;                
                for(var i=0;i<len;i++){
                    var p = clone(pre);
                    var c = clone(cur);
                    p.push(cur[i]);
                    remove(c,cur[i]);
                    if(len>1){
                        permute(p,c);
                    }else{
                        arrayResultPermute.push(p);
                        count++;
                    }
                }
         }
         
         
        function print(arr){
            var len=arr.length;
            for(var i=0;i<len;i++){
                document.write(arr[i].objectCommunes.TitleDirection+" --> ");
            }
            document.write("<br />");
        }
    
        function remove(arr,item){
            if(contains(arr,item)){
                var len=arr.length;
                for(var i = len-1; i >= 0; i--){ // STEP 1
                    if(arr[i] == item){             // STEP 2
                        arr.splice(i,1);              // STEP 3
                    }
                }
            }
        }
        
        function contains(arr,value){
            for(var i=0;i<arr.length;i++){
                if(arr[i]==value){
                    return true;
                }
            }
            return false;
        }
        
        function clone(arr){
            var a=new Array();
            var len=arr.length;
            for(var i=0;i<len;i++){
                a.push(arr[i]);
            }
            return a;
        }

         /* Thu?t toán permute - End */
         
         
          var finalArrPoints = new Array();
          var findMinRoute = false;
          
          
          function FindRouteMinMultiPoints(start, arrPoints ,end) {
                                  
            var dirTemp = new VDirections(map);
            dirTemp.EndPoint = arrPoints; //gán arrary point vào direction object  
            var diroptions = new VDirectionsOptions(true, false, false);
            var waypoints = new Array();                                    
            //Start Point
            waypoints.push(start.getPoint());   
            
           for (var i = 0; i < arrPoints.length; i++) {
               waypoints.push(arrPoints[i].getPoint());
           }  
                                          
            //End point
            waypoints.push(end.getPoint());                                                                        
            dirTemp.loadFromWayPoints(waypoints, diroptions);

            VEvent.addListener(dirTemp, 'loaded', function () {
       
                var v;
                if (dirTemp.Unit == "km")
                    v = eval(dirTemp.Distance);
                else
                    v = eval(dirTemp.Distance / 1000);

                ClearPolyLinesRoute();
                if (FINAL_MIN_DISTANCE > v) {
                    FINAL_MIN_DISTANCE = v;
                    finalArrPoints = dirTemp.EndPoint;
                }
                if (LENGTH_TEMP_MARKER == 0) {
               
                    arrayResultPermute = new Array();               
                    FINAL_MIN_DISTANCE = 100000000000;
                    markerPoints  = finalArrPoints;                    
                    ChangeIconSelectPoint(); // set icon                     
                    FindMap();//find get route
                    
                }
                else {
                    LENGTH_TEMP_MARKER = LENGTH_TEMP_MARKER - 1;                    
                    FindRouteMinMultiPoints(markerStart, arrayResultPermute[LENGTH_TEMP_MARKER],markerEnd);
                }

            });
              
        }
        
/*  BEGIN: REGION process get auto information of Marker */
       function GetRoadDistanceUncoverToDistributor(obj)
       {   
          var points = new Array();  
          for(var k = 0; k < 3 ;k++)
          {
              var minDistance = 999999;      
              var found = 0;  
              var tempDistributor = ""; 
              for(var i = 0; i < distributorDirect.VMarkers.length; i++)
              {
                    if(obj.LONGITUDE_LATITUDE != distributorDirect.VMarkers[i].obj.LONGITUDE_LATITUDE)
                    {
                       var tempDistance = CalculateDistance(obj.LONGITUDE_LATITUDE,distributorDirect.VMarkers[i].obj.LONGITUDE_LATITUDE); 
                     alert(tempDistance);
                       if( tempDistance != 0 && tempDistance < minDistance  ) 
                       {              
                            found = P5sIndexOf( points,distributorDirect.VMarkers[i]);
                            if( found == 0 && minDistance > tempDistance)
                            {
                                minDistance = tempDistance;   
                                tempDistributor = distributorDirect.VMarkers[i];
                                tempDistance = minDistance;
                                points.push(tempDistributor);
                            }
                       }                                      
                    }                    
               }                  
               
              for(var i = 0; i < distributorInDirect.VMarkers.length; i++)
              {
                    if(obj.LONGITUDE_LATITUDE != distributorInDirect.VMarkers[i].obj.LONGITUDE_LATITUDE)
                    {
                       var tempDistance = CalculateDistance(obj.LONGITUDE_LATITUDE,distributorInDirect.VMarkers[i].obj.LONGITUDE_LATITUDE); 
                     
                       if( tempDistance != 0 && tempDistance < minDistance  ) 
                       {              
                            found = P5sIndexOf( points,distributorInDirect.VMarkers[i]);
                            if( found == 0 && minDistance > tempDistance)
                            {
                                minDistance = tempDistance;   
                                tempDistributor = distributorInDirect.VMarkers[i];
                                tempDistance = minDistance;
                                points.push(tempDistributor);
                            }
                                    
                                              
                       }                                      
                    }                    
               }      
               
               
             if(tempDistributor == "")
                 break;            
          }
          
          
          if(points.length == 0)
          {
              //get thông tin commune nearest
               GetRoadDistanceByCommune(obj);
          }
          else
          {            
              pointsDistributor = points;
              lengthPointsDistributor = points.length - 1;              
              GetDistance5PointsByDistributor(obj, pointsDistributor[lengthPointsDistributor]);
          }
         
        
       
          
       }
       
       
        var minDistanceDistributor = 999999;           
        var tempDistributor;
        var pointsDistributor = new Array();  
        var lengthPointsDistributor = 0;
        
        function GetDistance5PointsByDistributor(start, end) {
                    var dirDistributor = new VDirections();     
                    dirDistributor.EndPoint = end; 
                    dirDistributor.StartPoint = start;
                                      
                    var diroptions = new VDirectionsOptions(true, false, false);
                    var waypoints = new Array();                                    
                    //Start Point
                    waypoints.push(start.getPoint());                                 
                    //End point
                    waypoints.push(end.getPoint());                                                                        
                    dirDistributor.loadFromWayPoints(waypoints, diroptions);
                             
                      
                    VEvent.addListener(dirDistributor, 'loaded', function () {                       
                       
                        var v = 999999;
                        if (dirDistributor.Unit == "km")
                            v = eval(dirDistributor.Distance);
                        else
                            v = eval(dirDistributor.Distance / 1000);                           

                        if (minDistanceDistributor >= v) {                        
                            minDistanceDistributor = v;        
                            tempDistributor  = dirDistributor.EndPoint;  
                        }
                                                
                        
                        if (lengthPointsDistributor == 0) {
                            
                            dirDistributor.StartPoint.objectCommunes.DISTRIBUTOR_NAME = tempDistributor.objectCommunes.NAME;
                            dirDistributor.StartPoint.objectCommunes.DISTRIBUTOR_DISTANCE = minDistanceDistributor;    
                            
                            document.getElementById("NameOfNearestDistributor").innerText = tempDistributor.objectCommunes.NAME;
                            document.getElementById("RoadDistanceDistributor").innerText = minDistanceDistributor + " km";                          
                            
                            //get thông tin commune nearest
                            GetRoadDistanceByCommune(dirDistributor.StartPoint);
                            minDistanceDistributor = 999999;           
                            tempDistributor = null;
                                                       
                        }
                        else {
                            lengthPointsDistributor = lengthPointsDistributor - 1;                     
                            GetDistance5PointsByDistributor(dirDistributor.StartPoint, pointsDistributor[lengthPointsDistributor]);
                        }

                    });         
                   return false;
         } 
        
        
        
        
            
         
        function GetRoadDistanceFromDistributor(obj,begin,end)
        {
                           //calculate distance
                var rad = function(x) {
                  return x * Math.PI / 180;
                };

                  var getDistance = function(p1, p2) {
                  var R = 6378137; // Earth’s mean radius in meter
                  var dLat = rad(p2.lat() - p1.lat());
                  var dLong = rad(p2.lng() - p1.lng());
                  var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                  Math.cos(rad(p1.lat())) * Math.cos(rad(p2.lat())) *
                  Math.sin(dLong / 2) * Math.sin(dLong / 2);
                  var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
                  var d = R * c;
                  return Math.round(d/10)/100; // returns the distance in kilometer
                };
                
                var latlng = begin.split(/, ?/);
                var startLatLng = new google.maps.LatLng(parseFloat(latlng[0]), parseFloat(latlng[1]));
                var latlng2 = end.split(/, ?/);
                var endLatLng = new google.maps.LatLng(parseFloat(latlng2[0]), parseFloat(latlng2[1]));
                var distance = getDistance(startLatLng,endLatLng);
                obj.DISTRIBUTOR_DISTANCE = distance;              
        }
        
        
        //region: process distance by commune
        function GetRoadDistanceByCommune(obj)
        {
          var points = new Array();  
          for(var k = 0; k < 3 ;k++)
          {
              var minDistance = 999999;      
              var found = 0;  
              
              var tempFoundMarkerWS = "";
              var tempFoundMarkerWWS = "";
             // console.log(coverageWS.VMarkers);
              for(var i in coverageWS.VMarkers)
              {
                    if(
                      coverageWS.VMarkers[i].obj.DistrictCD == obj.DistrictCD &&
                      coverageWS.VMarkers[i].obj.communeType ==  3 && 
                      obj.LONGITUDE_LATITUDE != coverageWS.VMarkers[i].obj.LONGITUDE_LATITUDE
                     )
                    {
                       var tempDistance = CalculateDistance(obj.LONGITUDE_LATITUDE,coverageWS.VMarkers[i].obj.LONGITUDE_LATITUDE); 
                     
                       if( tempDistance != 0 && tempDistance < minDistance  ) 
                       {              
                            found = P5sIndexOf( points,coverageWS.VMarkers[i]);
                            if( found == 0)
                            {
                                minDistance = tempDistance;   
                                tempFoundMarkerWS = coverageWS.VMarkers[i];
                            }               
                       }                                      
                    }                    
               } 
            
              
              
              for(var i in coverageWithouWS.VMarkers)
              {
                    if( coverageWithouWS.VMarkers[i].obj.DistrictCD == obj.DistrictCD && 
                        coverageWithouWS.VMarkers[i].obj.communeType == 3 &&  
                        obj.LONGITUDE_LATITUDE != coverageWithouWS.VMarkers[i].obj.LONGITUDE_LATITUDE                  
                    )
                    {
                       var tempDistance = CalculateDistance(obj.LONGITUDE_LATITUDE,coverageWithouWS.VMarkers[i].obj.LONGITUDE_LATITUDE); 
                       if( tempDistance != 0 && tempDistance < minDistance  ) 
                       {   
                            found = P5sIndexOf( points,coverageWithouWS.VMarkers[i]);
                            if( found == 0)
                            {
                                minDistance = tempDistance;
                                tempFoundMarkerWWS = coverageWithouWS.VMarkers[i];
                            }                  
                       }                                      
                    }                    
               }  
              
                                      
               
             if(tempFoundMarkerWS == "" && tempFoundMarkerWWS == "")
                 break; 
                       alert(minDistance);     
               
              //t?m kho?ng cách ng?n nh?t gi?a WS & WWS
              var distance1 = 999999; 
              var distance2 = 999999; 
              
              if(tempFoundMarkerWS != "" && tempFoundMarkerWWS == "")
              {
                 points.push(tempFoundMarkerWS); 
              }
              else
                if(tempFoundMarkerWWS != "" && tempFoundMarkerWS == "")
                {
                    points.push(tempFoundMarkerWWS); 
                }
                else
                {
                    distance1 = CalculateDistance(obj.LONGITUDE_LATITUDE,tempFoundMarkerWS.obj.LONGITUDE_LATITUDE); 
                    distance2 = CalculateDistance(obj.LONGITUDE_LATITUDE,tempFoundMarkerWWS.obj.LONGITUDE_LATITUDE); 
                    if(distance1 > distance2 )
                    {  
                        points.push(tempFoundMarkerWWS);                         
                    }
                    else
                         points.push(tempFoundMarkerWS);   
                    
                }                   
               
          }
          console.log(points);
          if(points.length == 0)
          {
            obj.COMMUNE_COVERED_NAME = "-";
            obj.COMMUNE_COVERED_DISTANCE = "-";
            // document.getElementById("NearestCommuneCoveredName").innerText = obj.COMMUNE_COVERED_NAME;
            //document.getElementById("NearestCommuneCoveredDistance").innerText = obj.COMMUNE_COVERED_DISTANCE + " km";
            $("#NearestCommuneCoveredName").text(obj.COMMUNE_COVERED_NAME);
            $("#NearestCommuneCoveredDistance").text(obj.COMMUNE_COVERED_DISTANCE + " km");
            //get thông tin Town nearest
            GetRoadDistanceByTown(obj);
           
          }
          else
          {                    
               pointsCommune = points;
               lengthPointsCommune = points.length - 1;              
               GetDistance5PointsByCommune(obj, pointsCommune[lengthPointsCommune]);
          }                      
        }
        
        var minDistanceCommune = 999999;           
        var tempCommune;
        var pointsCommune = new Array();  
        var lengthPointsCommune = 0;
        
        function GetDistance5PointsByCommune(start, end) {
                    var dirCommune = new VDirections();     
                    dirCommune.EndPoint = end; 
                    dirCommune.StartPoint = start;
                  
                    var diroptions = new VDirectionsOptions(true, false, false);
                    var waypoints = new Array();                                    
                    //Start Point
                    waypoints.push(start.getPoint());                                 
                    //End point
                    waypoints.push(end.getPoint());                                                                        
                    dirCommune.loadFromWayPoints(waypoints, diroptions);
                             
                      
                    VEvent.addListener(dirCommune, 'loaded', function () {                       
                                              
                        var v;
                        if (dirCommune.Unit == "km")
                            v = eval(dirCommune.Distance);
                        else
                            v = eval(dirCommune.Distance / 1000);

                        if (minDistanceCommune > v) {                        
                            minDistanceCommune = v;        
                            tempCommune  = dirCommune.EndPoint;                            
                        }
                        
                        if (lengthPointsCommune == 0) {
                         
                            dirCommune.StartPoint.objectCommunes.COMMUNE_COVERED_NAME = tempCommune.objectCommunes.NAME;
                            dirCommune.StartPoint.objectCommunes.COMMUNE_COVERED_DISTANCE = minDistanceCommune;    
                            
                            document.getElementById("NearestCommuneCoveredName").innerText = tempCommune.objectCommunes.NAME;
                            document.getElementById("NearestCommuneCoveredDistance").innerText = minDistanceCommune + " km";                          
                            //get thông tin Town nearest
                            GetRoadDistanceByTown(dirCommune.StartPoint);    
                            minDistanceCommune = 999999;                                       
                            tempCommune = null; 
                                                  
                        }
                        else {
                            lengthPointsCommune = lengthPointsCommune - 1;                     
                            GetDistance5PointsByCommune(dirCommune.StartPoint, pointsCommune[lengthPointsCommune]);
                        }

                    });         
                   return false;
         } 
         //End region: process distance by commune
           
           
           
           
           
         
         
         
        //region: process distance by town
        function GetRoadDistanceByTown(obj)
        {
        
        
          var points = new Array();  
          for(var k = 0; k < 3 ;k++)
          {
              var minDistance = 999999;      
              var found = 0;  
              
              var tempFoundMarkerWS = "";
              var tempFoundMarkerWWS = "";
              
              for(var i = 0; i < coverageWS.VMarkers.length; i++)
              {
                    if(
                        coverageWS.VMarkers[i].obj.DistrictCD == obj.DistrictCD &&
                        coverageWS.VMarkers[i].obj.communeType ==  2 &&  
                        obj.LONGITUDE_LATITUDE != coverageWS.VMarkers[i].obj.LONGITUDE_LATITUDE
                   
                    )
                    {
                       var tempDistance = CalculateDistance(obj.LONGITUDE_LATITUDE,coverageWS.VMarkers[i].obj.LONGITUDE_LATITUDE); 
                    
                       if( tempDistance != 0 && tempDistance < minDistance  ) 
                       {              
                            found = P5sIndexOf( points,coverageWS.VMarkers[i]);
                            if( found == 0)
                            {
                                minDistance = tempDistance;   
                                tempFoundMarkerWS = coverageWS.VMarkers[i];
                            }
                                    
                                              
                       }                                      
                    }                    
               } 
               
              
              
              for(var i = 0; i < coverageWithouWS.VMarkers.length; i++)
              {
                    if(
                        coverageWithouWS.VMarkers[i].obj.DistrictCD == obj.DistrictCD &&
                        coverageWithouWS.VMarkers[i].obj.communeType == 2 &&  
                        obj.LONGITUDE_LATITUDE != coverageWithouWS.VMarkers[i].obj.LONGITUDE_LATITUDE
                   
                    )
                    {
                       var tempDistance = CalculateDistance(obj.LONGITUDE_LATITUDE,coverageWithouWS.VMarkers[i].obj.LONGITUDE_LATITUDE); 
                       if( tempDistance != 0 && tempDistance < minDistance  ) 
                       {   
                            found = P5sIndexOf( points,coverageWithouWS.VMarkers[i]);
                            if( found == 0)
                            {
                                minDistance = tempDistance;
                                tempFoundMarkerWWS = coverageWithouWS.VMarkers[i];
                            }                  
                       }                                      
                    }                    
               }  
               
              //t?m kho?ng cách ng?n nh?t gi?a WS & WWS
              var distance1 = 999999; 
              var distance2 = 999999; 
              
              if(tempFoundMarkerWS == "" && tempFoundMarkerWWS == "")
                    break;
              
              if(tempFoundMarkerWS != "" && tempFoundMarkerWWS == "")
              {
                 points.push(tempFoundMarkerWS); 
              }
              else
                if(tempFoundMarkerWWS != "" && tempFoundMarkerWS == "")
                {
                    points.push(tempFoundMarkerWWS); 
                }
                else
                {
                    distance1 = CalculateDistance(obj.LONGITUDE_LATITUDE,tempFoundMarkerWS.obj.LONGITUDE_LATITUDE); 
                    distance2 = CalculateDistance(obj.LONGITUDE_LATITUDE,tempFoundMarkerWWS.obj.LONGITUDE_LATITUDE); 
                    if(distance1 > distance2 )
                    {  
                        points.push(tempFoundMarkerWWS);                         
                    }
                    else
                         points.push(tempFoundMarkerWS);   
                    
                }                   
               
          }
          
          
          if(points.length == 0)
          {
            obj.TOWN_COVERED_NAME = "-";
            obj.TOWN_COVERED_DISTANCE = "-";
            //document.getElementById("NearestTownCoveredName").innerText = obj.TOWN_COVERED_NAME;
            //document.getElementById("NearestTownCoveredDistance").innerText = obj.TOWN_COVERED_DISTANCE + " km";
            $("#NearestTownCoveredName").text(obj.TOWN_COVERED_NAME);
            $("#NearestTownCoveredDistance").text(obj.TOWN_COVERED_DISTANCE + " km");
             //get thông tin Ward nearest
             GetRoadDistanceByWard(obj);

          }
          else
          {                    
               pointsTown = points;
               lengthPointsTown = points.length - 1;
               GetDistance5PointsByTown(obj, pointsTown[lengthPointsTown]);
              
          
          }                      
        }
        
        var minDistanceTown = 999999;           
        var tempTown;
        var pointsTown = new Array();  
        var lengthPointsTown = 0;
        
        function GetDistance5PointsByTown(start, end) {
                    var dirTown = new VDirections();     
                    dirTown.EndPoint = end; 
                    dirTown.StartPoint = start;
                  
                    var diroptions = new VDirectionsOptions(true, false, false);
                    var waypoints = new Array();                                    
                    //Start Point
                    waypoints.push(start.getPoint());                                 
                    //End point
                    waypoints.push(end.getPoint());                                                                        
                    dirTown.loadFromWayPoints(waypoints, diroptions);
                             
                 
                      
                    VEvent.addListener(dirTown, 'loaded', function () {
                    
                        var v;
                        if (dirTown.Unit == "km")
                            v = eval(dirTown.Distance);
                        else
                            v = eval(dirTown.Distance / 1000);

                        if (minDistanceTown > v) {
                        
                            minDistanceTown = v;        
                            tempTown  = dirTown.EndPoint;                            
                        }                        
                        if (lengthPointsTown == 0) {
                           
                                                
                            dirTown.StartPoint.objectCommunes.TOWN_COVERED_NAME = tempTown.objectCommunes.NAME;
                            dirTown.StartPoint.objectCommunes.TOWN_COVERED_DISTANCE = minDistanceTown;    
                            
                            document.getElementById("NearestTownCoveredName").innerText = tempTown.objectCommunes.NAME;
                            document.getElementById("NearestTownCoveredDistance").innerText = minDistanceTown + " km";                            
                            //get thông tin Ward nearest
                            GetRoadDistanceByWard(dirTown.StartPoint);  
                            minDistanceTown = 999999;    
                            tempTown = null;

                        }
                        else {
                            lengthPointsTown = lengthPointsTown - 1;                     
                            GetDistance5PointsByTown(dirTown.StartPoint, pointsTown[lengthPointsTown]);
                        }

                    });         
                   return false;
         } 
         //End region: process distance by town
         
         
           
           
           
        //region: process distance by ward
        function GetRoadDistanceByWard(obj)
        {
                    
          var points = new Array();  
          for(var k = 0; k < 3 ;k++)
          {
              var minDistance = 999999;      
              var found = 0;  
              
              var tempFoundMarkerWS = "";
              var tempFoundMarkerWWS = "";
              
              for(var i = 0; i < coverageWS.VMarkers.length; i++)
              {
                    if(
                      coverageWS.VMarkers[i].obj.DistrictCD == obj.DistrictCD &&
                      coverageWS.VMarkers[i].obj.communeType ==  1 && 
                      obj.LONGITUDE_LATITUDE != coverageWS.VMarkers[i].obj.LONGITUDE_LATITUDE
                      
                    )
                    {
                       var tempDistance = CalculateDistance(obj.LONGITUDE_LATITUDE,coverageWS.VMarkers[i].obj.LONGITUDE_LATITUDE); 
                     
                       
                       if( tempDistance != 0 && tempDistance < minDistance  ) 
                       {              
                            found = P5sIndexOf( points,coverageWS.VMarkers[i]);
                            if( found == 0)
                            {
                                minDistance = tempDistance;   
                                tempFoundMarkerWS = coverageWS.VMarkers[i];
                            }
                                    
                                              
                       }                                      
                    }                    
               } 
               
              
              
              for(var i = 0; i < coverageWithouWS.VMarkers.length; i++)
              {
                    if(
                        coverageWithouWS.VMarkers[i].obj.DistrictCD == obj.DistrictCD &&
                        coverageWithouWS.VMarkers[i].obj.communeType == 1 && 
                        obj.LONGITUDE_LATITUDE != coverageWithouWS.VMarkers[i].obj.LONGITUDE_LATITUDE
                   
                    )
                    {
                       var tempDistance = CalculateDistance(obj.LONGITUDE_LATITUDE,coverageWithouWS.VMarkers[i].obj.LONGITUDE_LATITUDE); 
                       if( tempDistance != 0 && tempDistance < minDistance  ) 
                       {   
                            found = P5sIndexOf( points,coverageWithouWS.VMarkers[i]);
                            if( found == 0)
                            {
                                minDistance = tempDistance;
                                tempFoundMarkerWWS = coverageWithouWS.VMarkers[i];
                            }                  
                       }                                      
                    }                    
               }  
               
              //t?m kho?ng cách ng?n nh?t gi?a WS & WWS
              var distance1 = 999999; 
              var distance2 = 999999; 
              
              if(tempFoundMarkerWS == "" && tempFoundMarkerWWS == "")
                    break;
              
              if(tempFoundMarkerWS != "" && tempFoundMarkerWWS == "")
              {
                 points.push(tempFoundMarkerWS); 
              }
              else
                if(tempFoundMarkerWWS != "" && tempFoundMarkerWS == "")
                {
                    points.push(tempFoundMarkerWWS); 
                }
                else
                {
                    distance1 = CalculateDistance(obj.LONGITUDE_LATITUDE,tempFoundMarkerWS.obj.LONGITUDE_LATITUDE); 
                    distance2 = CalculateDistance(obj.LONGITUDE_LATITUDE,tempFoundMarkerWWS.obj.LONGITUDE_LATITUDE); 
                    if(distance1 > distance2 )
                    {  
                        points.push(tempFoundMarkerWWS);                         
                    }
                    else
                        points.push(tempFoundMarkerWS);   
                    
                }                   
               
          }
          
          if(points.length == 0)
          {
            obj.WARD_COVERED_NAME = "-";
            obj.WARD_COVERED_DISTANCE = "-";
            //document.getElementById("NearestWardCoveredName").innerText = obj.WARD_COVERED_NAME;
            //document.getElementById("NearestWardCoveredDistance").innerText = obj.WARD_COVERED_DISTANCE + " km";
            $("#NearestWardCoveredName").text(obj.WARD_COVERED_NAME);
            $("#NearestWardCoveredDistance").text(obj.WARD_COVERED_DISTANCE + " km");
          }
          else
          {                    
               pointsWard = points;
               lengthPointsWard = points.length - 1;
               GetDistance5PointsByWard(obj, pointsWard[lengthPointsWard]);  
                     
          }   
         
                             
        }
        
        var minDistanceWard = 999999;           
        var tempWard;
        var pointsWard = new Array();  
        var lengthPointsWard = 0;
        
        function GetDistance5PointsByWard(start, end) {
                    var dirWard = new VDirections();     
                    dirWard.EndPoint = end; 
                    dirWard.StartPoint = start;
                  
                    var diroptions = new VDirectionsOptions(true, false, false);
                    var waypoints = new Array();                                    
                    //Start Point
                    waypoints.push(start.getPoint());                                 
                    //End point
                    waypoints.push(end.getPoint());                                                                        
                    dirWard.loadFromWayPoints(waypoints, diroptions);
                      
                    VEvent.addListener(dirWard, 'loaded', function () {
                     
                         
                        var v;
                        if (dirWard.Unit == "km")
                            v = eval(dirWard.Distance);
                        else
                            v = eval(dirWard.Distance / 1000);

                        if (minDistanceWard > v) {
                        
                            minDistanceWard = v;        
                            tempWard  = dirWard.EndPoint;                            
                        }                        
                        
                        if (lengthPointsWard == 0) {
                            
                            dirWard.StartPoint.objectCommunes.WARD_COVERED_NAME = tempWard.objectCommunes.NAME;
                            dirWard.StartPoint.objectCommunes.WARD_COVERED_DISTANCE = minDistanceWard;    
                            
                            document.getElementById("NearestWardCoveredName").innerText = tempWard.objectCommunes.NAME;
                            document.getElementById("NearestWardCoveredDistance").innerText = minDistanceWard + " km";                            
                            minDistanceWard = 999999; 
                            tempWard = null;
                            
                        }
                        else {
                            lengthPointsWard = lengthPointsWard - 1;                     
                            GetDistance5PointsByWard(dirWard.StartPoint, pointsWard[lengthPointsWard]);
                        }

                    });         
                   return false;
         } 
         //End region: process distance by ward
           
/*  End: REGION process get auto information of Marker */
           
           
            
         function FindMap() {
       
//            if(  document.getElementById("<%=P5sLbtnStartRoutePlanning.ClientID%>").getAttribute("disabled") == true)
//                 return;    

            //Start Point
           //console.log(markerStart);
            if(markerStart == null)   
            {
                 alert("Chọn điểm bắt đầu thực hiện tính năng này.");
                return false;
            }
          
          
           //Start Point
            if(markerEnd == null)   
            {
                 alert("Chọn điểm kết thúc thực hiện tính năng này.");
                return false;
            } 
             
            var autoOrManualDirection = document.getElementById("P5sChkAutoOrManual").checked;
            if(autoOrManualDirection == true && findMinRoute == false && markerPoints != null && markerPoints.length >= 2 && markerPoints.length <= 4)
            {         
                 var tempMarkerPoints = new Array();
                 findMinRoute = true;
                 permute(tempMarkerPoints,markerPoints);  // t?m t?t c? các TH hoán v?
                 LENGTH_TEMP_MARKER = arrayResultPermute.length -1; //get length 
                 FindRouteMinMultiPoints(markerStart,arrayResultPermute[LENGTH_TEMP_MARKER],markerEnd);             
            }
            else
            {
            
                ClearPolyLinesRoute();
                var directionsService = new google.maps.DirectionsService;
                                 var directionsDisplay = new google.maps.DirectionsRenderer({
                                      map: map
                                    })
                                    
                      //var direction = document.getElementById("<%=PanelScroll.ClientID%>");
                 var latlng = markerStart.LONGITUDE_LATITUDE.split(/, ?/);
                 var startPoint = new google.maps.LatLng(parseFloat(latlng[0]), parseFloat(latlng[1]));
                 //alert("Start: " + start.LONGITUDE_LATITUDE);
                 var latlng2 = markerEnd.LONGITUDE_LATITUDE.split(/, ?/);
                 var endPoint = new google.maps.LatLng(parseFloat(latlng2[0]), parseFloat(latlng2[1]));
                // alert("End: " + end.obj.LONGITUDE_LATITUDE);
                 var markerStart = new google.maps.Marker({
                              position: startPoint,
                              title: "Bắt đầu",
                              label: "Start",
                              map: map
                            });
                 var markerEnd = new google.maps.Marker({
                              position: endPoint,
                              title: "Kết thúc",
                              label: "End",
                              map: map
                            });
               calculateAndDisplayRoute(directionsService, directionsDisplay, startPoint, endPoint);
                    
                //PanelScroll
                //T?m ???ng ch? ho?t ??ng d?a vào t?a ??
              //  var direction = document.getElementById("<%=PanelScroll.ClientID%>");
                //P5sPnlDirection
              //  var  dir = new VDirections(map, direction);
              //  var diroptions = new VDirectionsOptions(true, false, false);
             //   var waypoints = new Array();
            
            
                
             //   findMinRoute = false;
                
             //   waypoints.push(markerStart.getPoint());       // start
              //  for (var i = 0; i < markerPoints.length; i++) {
               //     waypoints.push(markerPoints[i].getPoint()); // point select
              //  }  
              //  waypoints.push(markerEnd.getPoint());      // end
               //   dir.loadFromWayPoints(waypoints, diroptions);   
                
                //   VEvent.addListener(dir, 'loaded', function () {
                         
                                     
                     //   var str = dir.getSummaryHTML();    
                     //   str = str.replace(markerStart.getPoint(), FormatStrBr(markerStart.objectCommunes.TitleDirection) ); 
                    //    str = str.replace(markerEnd.getPoint(), FormatStrBr(markerEnd.objectCommunes.TitleDirection) ); 
                              
                        //Select Point
                    //    for (var i = 0; i < markerPoints.length; i++) {
                     //      str = str.replace(markerPoints[i].getPoint(),FormatStrBr(markerPoints[i].objectCommunes.TitleDirection )  );                    
                     //   }
                            
                        
                     //  document.getElementById("<%=PanelScroll.ClientID%>").innerHTML = str;
                     //  document.getElementById("<%=PanelScroll.ClientID%>").style.display = 'block';  
                     
                   //});
                          
            }
            return false;                      
        }
        
      
        
        
       function PrintResult() {   
            var str = markerStart.LONGITUDE_LATITUDE+ "-" + markerStart.getIcon().image + "-" +  markerStart.obj.TitleDirection;         
           
              //Select Point
            for (var i = 0; i < markerPoints.length; i++) {       
                 str += "|" + markerPoints[i].getPoint()+ "-" + markerPoints[i].getIcon().image + "-" +  markerPoints[i].objectCommunes.TitleDirection;
            }
             str += "|" +  markerEnd.getPoint()+ "-" + markerEnd.getIcon().image + "-" +  markerEnd.objectCommunes.TitleDirection;
             
            window.open("/PrintMapResult.aspx?p="+str)   
       }
    </script>

    <script type="text/javascript">
    
                                 function CalculateDistance(i, k) {
                                        var tmp_i = new Array(); 
                                        var tmp_k = new Array();
										tmp_i = i.split(",");
										tmp_k = k.split(",");
                                        var n, l, h;
                                        var j;
                                        var c;
                                        var e;
                                        var a;
                                        var m;
                                        var d = tmp_i[0];
                                        var b = tmp_i[1];
                                        var g = tmp_i[0];
                                        var f = tmp_i[1];
                                        j = d * (Math.PI / 180);
                                        c = b * (Math.PI / 180);
                                        e = g * (Math.PI / 180);
                                        a = f * (Math.PI / 180);
                                        n = b - f;
                                        m = n * (Math.PI / 180);
                                        h = Math.sin(j) * Math.sin(e) + Math.cos(j) * Math.cos(e) * Math.cos(m);
                                        h = Math.acos(h);
                                        l = h * 180 / Math.PI;
                                        l = l * 60 * 1.1515;
                                        l = l * 1.609344 * 1000;
                                        return Math.round(l)
                                 }
                                
                                
                                 
                                
                                
                                
                                
                                
                                function P5sIndexOf(arr,value)
                                {
                                     for(var i = 0; i < arr.length; i++)
                                     {
                                        if(arr[i] == value)
                                            return 1;
                                     }
                                     return 0;
                                }
                                     
                                  
                                 //tính n?ng t?m kho?ng cách th?c d?a vào 5 ?i?m g?n nh?t ???ng chim bay
                                 var FINAL_MIN_DISTANCE = 100000000000;           
                                 var TEMP_MARKER_END;
                                 var LENGTH_TEMP_MARKER;
                                 var ARR_5POINTS = new Array();  

                                 function FindRoute5Points(start, end) {
                                                          
                                    var dir = new VDirections();
                                    dir.EndPoint = end; 
                                    var diroptions = new VDirectionsOptions(true, false, false);
                                    var waypoints = new Array();                                    
                                    //Start Point
                                    waypoints.push(start.getPoint());                                 
                                    //End point
                                    waypoints.push(end.getPoint());                                                                        
                                    dir.loadFromWayPoints(waypoints, diroptions);


                                    VEvent.addListener(dir, 'loaded', function () {
                                        var v;
                                        if (dir.Unit == "km")
                                            v = eval(dir.Distance);
                                        else
                                            v = eval(dir.Distance / 1000);


                                        if (FINAL_MIN_DISTANCE > v) {
                                            FINAL_MIN_DISTANCE = v;
                                            markerEnd = dir.EndPoint;
                                        }
                                        if (LENGTH_TEMP_MARKER == 0) {

                                            ChangeIconEndPoint();
                                            //Clear All Value
                                            VPolylines = new Array(); // Clear Polylines
                                            ARR_5POINTS = new Array();
                                            FINAL_MIN_DISTANCE = 100000000000;
                                            TEMP_MARKER_END = null;
                                            FindRouteFromTwoPoint(markerStart, markerEnd);
                                        }
                                        else {
                                            LENGTH_TEMP_MARKER = LENGTH_TEMP_MARKER - 1;
                                            FindRoute5Points(markerStart, ARR_5POINTS[LENGTH_TEMP_MARKER]);
                                        }

                                    });
                                      
                                    return false;
                                }

                                                                     
                                function GetDistanceNearest()
                                {   
                                    if(markerStart  != undefined )
                                    {

                                       ARR_5POINTS = new Array();        
                                       
 
                                                                          
                                       for(var k = 0; k < 5 ;k++)
                                       {
                                              var minDistance = 100000000000;
                                              var tmpMinMarker = "";
                                              var found = 0;
                                             
                                            
                                              
                                               for(var i = 0; i < coverageWS.VMarkers.length; i++)
                                               {
                                                    //console.log(coverageWS.VMarkers[i]);
                                                    var tempDistance = CalculateDistance(markerStart.LONGITUDE_LATITUDE,coverageWS.VMarkers[i].obj.LONGITUDE_LATITUDE );                                                  
                                                    if( tempDistance != 0 && tempDistance < minDistance && markerStart.LONGITUDE_LATITUDE != coverageWS.obj.LONGITUDE_LATITUDE )
                                                     { 
                                                        found = P5sIndexOf( ARR_5POINTS,coverageWS.VMarkers[i]);
                                                        if( found == 0)
                                                        {
                                                          minDistance = tempDistance;
                                                          // markerEnd = coverageWS.VMarkers[i]; 
                                                           tmpMinMarker = coverageWS.VMarkers[i]; 
                                                        }
                                                     }
                                               }                                       
                                      
                                               
                                               for(var i = 0; i < coverageWithouWS.VMarkers.length; i++)
                                               {
                                                    
                                                    var tempDistance = CalculateDistance(markerStart.LONGITUDE_LATITUDE ,coverageWithouWS.VMarkers[i].obj.LONGITUDE_LATITUDE );
                                                    if( tempDistance != 0 &&  tempDistance < minDistance  &&  markerStart.LONGITUDE_LATITUDE != coverageWithouWS.VMarkers[i].obj.LONGITUDE_LATITUDE  )
                                                    { 
                                                         found = P5sIndexOf( ARR_5POINTS,coverageWithouWS.VMarkers[i]  );
                                                         if( found == 0)
                                                         {
                                                             minDistance = tempDistance;
                                                            // markerEnd = coverageWithouWS.VMarkers[i];      
                                                            tmpMinMarker = coverageWithouWS.VMarkers[i]; 
                                                         }
                                                        
                                                                                                                                                       
                                                    }
                                               }      
                                            
                                             if(tmpMinMarker != "" )
                                                 ARR_5POINTS.push(tmpMinMarker);                                       
                                       }
                              
                              
                                         //Find min route in ARR_5POINTS
                                         LENGTH_TEMP_MARKER = ARR_5POINTS.length;
                                         if (LENGTH_TEMP_MARKER >= 1) {
                                             LENGTH_TEMP_MARKER = LENGTH_TEMP_MARKER - 1;
                                             FindRoute5Points(markerStart, ARR_5POINTS[LENGTH_TEMP_MARKER]);
                                         }

                                    }
                                    
                                }
                                
                                function ShowHideDistributorNetworkPoint()
                                {
                                   
                                    var btn =  document.getElementById("<%=P5sLbtnShowHideDistributorNetworkPoint.ClientID%>");
                                    if(btn.innerText == "Show Points")
                                    {
                                        btn.innerText = "Hide Points";
                                        if ( distributorNetworkPoint != undefined &&  distributorNetworkPoint.IsMarkerShow == false) {
                                            distributorNetworkPoint.show();    
                                        }   
                                    }
                                    else
                                    {
                                        btn.innerText = "Show Points";
                                        if ( distributorNetworkPoint != undefined &&  distributorNetworkPoint.IsMarkerShow != false) {
                                            distributorNetworkPoint.hide();    
                                        } 
                                    }
                                    return false;                                                                 
                                }
                                
                                function ShowHideCentralPoint()
                                {
                              
                                    var btn =  document.getElementById("<%=P5sLbtnShowHideCentralPoint.ClientID%>");
                                    if(btn.innerText == "Show Central Points")
                                    {
                                        btn.innerText = "Hide Central Points";
                                        if ( centralPoint != undefined &&  centralPoint.IsMarkerShow == false) {

                                            centralPoint.show();    
                                        }   
                                    }
                                    else
                                    {
                                        btn.innerText = "Show Central Points";
                                        if ( centralPoint != undefined &&  centralPoint.IsMarkerShow != false) {
                                            centralPoint.hide();    
                                        } 
                                    }               
                                    
                                    
                                    return false;                                                                 
                                }
                                
                                
                                function ClearAllIconSelect()
                                {
                                     ClearPolyLinesRoute();
                                     //Change default icon select
                                     for(var i = markerPoints.length- 1; i >= 0; i--)
                                     {
                                        markerTmp = markerPoints[i];                                       
                                        ChangeDefaultIconSelect();
                                     }
                                     markerPoints = new Array();                                                                      
                                     ChangeDefaultIconEnd();                    
                                     ChangeDefaultIconStart();      
                                     
                                     markerStart = undefined;
                                     markerEnd = undefined;
                                  //   document.getElementById("<%=P5sLbtnStartRoutePlanning.ClientID%>").disabled = true;
                                     RadioButtonCheckDisable();                                      
                                     
                                     return false;
                                }
                                
                                function FindRouteNearest()
                                {                                                                                                              
                                    //Change default icon select
                                     for(var i = markerPoints.length- 1; i >= 0; i--)
                                     {
                                        markerTmp = markerPoints[i];                                       
                                        ChangeDefaultIconSelect();
                                     }
                                     markerPoints = new Array();                                                                      
                                     ChangeDefaultIconEnd();              
                                     
                                     if(markerStart == undefined)
                                     {
                                        alert("Chọn điểm bắt đầu thực hiện tính năng này .");
                                        return false;
                                     }
                                 
                                      
                                    var DistanceFromDistributor = document.getElementById("DistanceFromDistributor").checked;
                                    if(DistanceFromDistributor == true)                               
                                        GetDistanceFromDistributor();  
                                    else
                                        GetDistanceNearest();
                                      
                                
                                    
                                    return false;   
                                }
                                                             
                                function GetDistanceFromDistributor()
                                {
                                  // console.log(markerStart);
                                    if(markerStart != undefined &&  markerStart.LONGITUDE_LATITUDE_DISTRIBUTOR != "" )
                                    {                          
                                       var found = false;
                                       var lat = markerStart.LONGITUDE_LATITUDE_DISTRIBUTOR .split(",");
                                       var pt = new google.maps.LatLng(lat[0], lat[1]);
                                       
                                       //console.log(distributorDirect);
                                       //find DistributorDirect
                                       for(var i = 0; i < distributorDirect.VMarkers.length; i++)
                                       {            
                                            if(distributorDirect.VMarkers[i].obj.ID == markerStart.DISTRIBUTOR_CD)
                                            { 
                                                markerEnd = distributorDirect.VMarkers[i];                                              
                                                found = true;
                                                break;
                                            }
                                       }   
                                       
                                    
                                       
                                       
                                       if(found == true)
                                       {
                                            
                                            ChangeIconEndPoint();
                                            FindRouteFromTwoPoint(markerStart,markerEnd);                                       
                                       }                                              
                                    }
                                    else
                                    {
                                        alert("Tính năng này chưa hoạt động với các khu vực không Cover .");
                                    }                      
                                }
                                
                                      
                             function FindRouteFromTwoPoint(start,end) {
                                          
                                                             
                                ClearPolyLinesRoute();
                               // console.log(end);
                                //T?m ???ng ch? ho?t ??ng d?a vào t?a ??
                                 var directionsService = new google.maps.DirectionsService;
                                 var directionsDisplay = new google.maps.DirectionsRenderer({
                                      map: map
                                    })
                                 //var direction = document.getElementById("<%=PanelScroll.ClientID%>");
                                  var latlng = start.LONGITUDE_LATITUDE.split(/, ?/);
                                 var startPoint = new google.maps.LatLng(parseFloat(latlng[0]), parseFloat(latlng[1]));
                                 //alert("Start: " + start.LONGITUDE_LATITUDE);
                                 var latlng2 = end.obj.LONGITUDE_LATITUDE.split(/, ?/);
                                 var endPoint = new google.maps.LatLng(parseFloat(latlng2[0]), parseFloat(latlng2[1]));
                                // alert("End: " + end.obj.LONGITUDE_LATITUDE);
                                 var markerStart = new google.maps.Marker({
                                              position: startPoint,
                                              title: "Bắt đầu",
                                              label: "Start",
                                              map: map
                                            });
                                 var markerEnd = new google.maps.Marker({
                                              position: endPoint,
                                              title: "Kết thúc",
                                              label: "End",
                                              map: map
                                            });
                                 calculateAndDisplayRoute(directionsService, directionsDisplay, startPoint, endPoint);
                                //P5sPnlDirection
                                //var  dir = new VDirections(map, direction);
                              //  var diroptions = new VDirectionsOptions(true, false, false);
//                                var waypoints = new Array();
//                                
//                               console.log(start); 
//                                //Start Point
//                                waypoints.push(start.getPoint());
//                             
//                                //End point
//                                waypoints.push(end.getPoint());
//                                
//                                dir.loadFromWayPoints(waypoints, diroptions);          
//                                
//                                    
//                               VEvent.addListener(dir, 'loaded', function () {
//                                    var str = dir.getSummaryHTML();                                         
//                                    str = str.replace(start.getPoint(), FormatStrBr(start.objectCommunes.TitleDirection) ); 
//                                    str = str.replace(end.getPoint(), FormatStrBr(end.objectCommunes.TitleDirection) );                                     
//                                                                                                           
//                                    document.getElementById("<%=PanelScroll.ClientID%>").innerHTML = str;
//                                    document.getElementById("<%=PanelScroll.ClientID%>").style.display = 'block';  
//                                 
//                               });
                                  
                                return false;                      
                            }
                                
                           function calculateAndDisplayRoute(directionsService, directionsDisplay, pointA, pointB) {
                                
                                  directionsService.route({
                                    origin: pointA,
                                    destination: pointB,
                                    travelMode: google.maps.TravelMode.DRIVING
                                  }, function(response, status) {
                                    if (status == google.maps.DirectionsStatus.OK) {
                                      directionsDisplay.setDirections(response);
                                    } else {
                                      alert('Directions request failed due to ' + status);
                                    }
                                  });
                                }
    
                                
                                function RadioButtonCheckDisable()
                                {
                                
                                    if(markerStart == undefined && markerEnd == undefined && markerPoints.length <= 0 )
                                    {
//                                       document.getElementById("DistanceFromDistributor").setAttribute("disabled","disabled");
//                                       document.getElementById("DistanceNearestTownCommune").setAttribute("disabled","disabled");
//                                       document.getElementById("<%=P5sLbtnFindRouteDistance.ClientID%>").setAttribute("disabled","disabled");
                                    }
                                    else
                                    {
//                                       document.getElementById("DistanceNearestTownCommune").removeAttribute("disabled");
//                                       document.getElementById("DistanceFromDistributor").removeAttribute("disabled");
//                                       document.getElementById("<%=P5sLbtnFindRouteDistance.ClientID%>").removeAttribute("disabled","disabled");
                                    }                                   
                                
                                }

                                
                                function ChangeDefaultIconStart()
                                {
                                     if(markerStart == null)
                                        return;
                                   //  console.log(markerStart);  
                                     var imagePath = markerStart.DistributorTypeImage;
                                     if (imagePath==null)
                                     imagePath = "/icon/ND.png";
                                     var icon = new Image();
                                  
                                     
                                     for(var i = 0 ; i < letters.length ; i++)
                                     {
                                        imagePath = imagePath.replace("/markerImages/","/icon/");
                                        imagePath = imagePath.replace( letters[i] ,"");                                        
                                     }
                                     
                                     imagePath = imagePath.replace("_" ,"");      
                                     
                                     icon.image = imagePath;                                   
                                     markerStart.setIcon= icon.image; 
                                }
                                                           
                    
                                
                                function ChangeDefaultIconEnd()
                                {
                                    if(markerEnd == null)
                                        return;
                                        //  alert(imagePath);
                                     var imagePath = markerEnd.DistributorTypeImage;
                                     if(imagePath==null)
                                     imagePath = markerEnd.setIcon;
                                     var icon = new Image();                                                                     
                                     for(var i = 0 ; i < letters.length ; i++)
                                     {
                                        imagePath = imagePath.replace("/markerImages/","/icon/");
                                        imagePath = imagePath.replace( letters[i] ,"");                                        
                                     }
                                     imagePath = imagePath.replace("_" ,"");       
                                     
                                     icon.image = imagePath;
                                     markerEnd.setIcon= icon.image;
                                }
                                
                                function ChangeDefaultIconSelect()
                                {
                                    if(markerTmp == null)
                                        return;
                                         
                                     var imagePath = markerTmp.DistributorTypeImage;
                                     var icon = new Image();                                                                      
                                     for(var i = 0 ; i < letters.length ; i++)
                                     {
                                        imagePath = imagePath.replace("/markerImages/","/icon/");
                                        imagePath = imagePath.replace( letters[i] ,"");                                        
                                     }
                                     icon.image = imagePath;
                                     markerTmp.setIcon= icon.image;;
                                }
                                
                                
                                function ChangeIconEndPoint() 
                                {
                                       if(markerEnd == null)
                                           return;
                                           
                                       var imagePath = markerEnd.DistributorTypeImage;
                                       //console.log(imagePath);
                                       if(typeof imagePath == 'undefined')
                                       imagePath = markerEnd.obj.DistributorTypeImage;
                                       //console.log('2 ' + imagePath);
                                       var image = imagePath;                                    
                                   // console.log(markerEnd);
                                    
                                       var numberPoint = 0;
                                       if( markerStart != null )
                                         numberPoint =  numberPoint + 1;
                                       numberPoint =  numberPoint + markerPoints.length;
                                       
                                       switch(imagePath)
                                        {
                                            case PATH_CWS_IMG:  
                                               image = "/markerImages/CoveredWS" + letters[numberPoint]+ ".png";                                          
                                               break;
                                            case PATH_WWS_IMG:                                        
                                               image = "/markerImages/CoveredWithoutWS" +  letters[numberPoint]+".png";  
                                               break;
                                            case PATH_UC_IMG:                      
                                              image = "/markerImages/Uncovered"+ letters[numberPoint]+".png";                    
                                              break;  
                                             case null:                      
                                              image = "/markerImages/Uncovered"+ letters[numberPoint]+".png";                    
                                              break;                                            
                                            case PATH_DISTRIBUTOR_DIRECT:                             
                                              image = "/markerImages/ND"+ letters[numberPoint]+".png";             
                                              break;
                                            case PATH_DISTRIBUTOR_INDIRECT:                             
                                              image = "/markerImages/SD"+ letters[numberPoint]+".png";             
                                              break;      
                                            case PATH_DISTRIBUTOR_DIRECT_SDN:                             
                                              image = "/markerImages/SDN"+ letters[numberPoint]+".png";             
                                              break; 
                                                  
                                             default:                                             
                                                 image = imagePath;
                                                 //console.log(image);
                                                 //alert(icon.image);
                                                 for(var i = 0 ; i < letters.length ; i++)
                                                 {                                                    
                                                    image =   image.replace(letters[i],letters[numberPoint]);                                                         
                                                                                                                      
                                                 }
                                               break;                                
                                        }                                                   
                                      markerEnd.setIcon = image;  
                                                                        
                                }
                                
                                 
                                 function ChangeIconSelectPoint()
                                 {                                  
                                      
                                       var icon = new Image();
                                                                             
                                       var numberPoint = 0;
                                       if( markerStart != null )
                                         numberPoint =  numberPoint + 1;
                                           
                                         for(var i = 0; i < markerPoints.length ; i++)
                                         {                     
                                           var imagePath = markerPoints[i].DistributorTypeImage;
                                           var icon = new Image();                       
                                            switch(imagePath)
                                            {
                                                case PATH_CWS_IMG:  
                                                  icon.image = "/markerImages/CoveredWS"+ letters[numberPoint]+".png";                                          
                                                  break;
                                                case PATH_WWS_IMG:                                        
                                                  icon.image = "/markerImages/CoveredWithoutWS"+ letters[numberPoint]+".png";  
                                                  break;
                                                case PATH_UC_IMG:                      
                                                  icon.image = "/markerImages/Uncovered"+ letters[numberPoint]+".png";                    
                                                  break;                                              
                                                case PATH_DISTRIBUTOR_DIRECT:                             
                                                  icon.image = "/markerImages/ND"+ letters[numberPoint]+".png";             
                                                  break;
                                                case PATH_DISTRIBUTOR_INDIRECT:                             
                                                  icon.image = "/markerImages/SD"+ letters[numberPoint]+".png";             
                                                  break;    
                                                  
                                                case PATH_DISTRIBUTOR_DIRECT_SDN:                             
                                                  icon.image = "/markerImages/SDN"+ letters[numberPoint]+".png";             
                                                  break;    
                                                                                                
                                                default:
                                                 for(var j = 0 ; j < letters.length ; j++)
                                                 {
                                                   imagePath =   imagePath.replace(letters[j],letters[numberPoint]);              
                                                 }
                                                 icon.image = imagePath;
                                               break;    
                                           }                                                                                                        
                                            numberPoint = numberPoint + 1;    
                                            markerPoints[i].setIcon= icon.image;                                                  
                                         }
                                       
                                       
                                       ChangeIconEndPoint();
                                                                
                                }
                                
                                
                                 function ChangeIconStartPoint()
                                 {                                
                                       if(markerStart == null)  
                                           return;
                                           //console.log(markerStart);
                                      // alert(markerStart.icon); 
                                       var imagePath = markerStart.DistributorTypeImage; 
                                          //  alert(imagePath);      
                                          if(imagePath==null)
                                          imagePath = markerStart.setIcon;             
                                       for(var i = 0 ; i < letters.length ; i++)
                                       {                                                    
                                           imagePath  =  imagePath.replace(letters[i],"");         
                                          // imagePath  =  imagePath.replace("markerImages","icon");         
                                       }
                                                 
                                       var icon = new Image();                                 
                                       switch(imagePath)
                                        {
                                            case PATH_CWS_IMG:  
                                              icon.image = "/markerImages/CoveredWS_A.png";                                          
                                              break;
                                            case PATH_WWS_IMG:                                        
                                              icon.image = "/markerImages/CoveredWithoutWS_A.png";  
                                              break;
                                            case PATH_UC_IMG:                      
                                              icon.image = "/markerImages/Uncovered_A.png";                    
                                              break;                                              
                                            case PATH_DISTRIBUTOR_DIRECT:                             
                                              icon.image = "/markerImages/ND_A.png";             
                                              break;
                                            case PATH_DISTRIBUTOR_INDIRECT:                             
                                               icon.image = "/markerImages/SD_A.png";  
                                               break;
                                               
                                             case PATH_DISTRIBUTOR_DIRECT_SDN:                             
                                              icon.image = "/markerImages/SDN_A.png";                                                           
                                              break;
                                            
                                        }     
                                                                                                           
                                      markerStart.setIcon= icon.image;                                     
                                }
                                                               
                               
                                
                                
                    
                                function StartHere()
                                {                                     
                                    ClearPolyLinesRoute();
                                    if(document.getElementById(START_IMG).getAttribute("src").indexOf(PATH_ARROW_IMG) != -1 )
                                    {                                                                                                                    
                                       ChangeDefaultIconStart(); //Clear Icon Start if exists                                     
                                       markerStart = markerTmp ;           
                                                               
//                                       if(markerStart != undefined && markerEnd != undefined )
//                                           document.getElementById("<%=P5sLbtnStartRoutePlanning.ClientID%>").disabled = false;  
                                                                                    
                                       document.getElementById(START_IMG).setAttribute("src",PATH_ARROW_DOWN_IMG);
                                       
                                       
                                       //Check Start Point the like Select Point
                                        for(var i = markerPoints.length- 1; i >= 0; i--)
                                        {
                                           if(markerPoints[i] == markerTmp )
                                           {
                                              document.getElementById(SELECT_IMG).setAttribute("src",PATH_ARROW_IMG);                                              
                                              markerPoints.splice(i,1);                                              
                                              break;
                                           }
                                        }
                                                                   
                                       //Check Start Point the like End Point
                                       if(markerStart != undefined && markerEnd != undefined  &&  markerStart == markerEnd)
                                       {                                      
                                          //Change Icon Start & End  
                                           var imagePath = markerStart.DistributorTypeImage;
                                          // alert(imagePath);
                                           var icon = new Image();                                          
                                           
                                           for(var j = 0 ; j < letters.length ; j++)
                                           {
                                                imagePath = imagePath.replace("/markerImages/","/icon/");
                                                imagePath = imagePath.replace( letters[j] ,"");                                        
                                           }                
                                           imagePath = imagePath.replace("_" ,"");  
                                                                                                    
                                           switch(imagePath)
                                            {
                                                case PATH_CWS_IMG:  
                                                  icon.image = "/markerImages/CoveredWS_.png";                                          
                                                  break;
                                                case PATH_WWS_IMG:                                        
                                                  icon.image = "/markerImages/CoveredWithoutWS_.png";  
                                                  break;
                                                case PATH_UC_IMG:                      
                                                  icon.image = "/markerImages/Uncovered_.png";                    
                                                  break;                                              
                                                case PATH_DISTRIBUTOR_DIRECT:                             
                                                  icon.image = "/markerImages/ND_.png";             
                                                  break;   
                                                case PATH_DISTRIBUTOR_INDIRECT:                             
                                                  icon.image = "/markerImages/SD_.png";             
                                                  break;                                      
                                                  
                                               case PATH_DISTRIBUTOR_DIRECT_SDN:                             
                                                  icon.image = "/markerImages/SDN_.png";             
                                                  break;                                      
                                                      
                                            }                                                                            
                                          markerStart.setIcon=icon.image;      
                                          
                                                                                                                      
                                       }
                                       else
                                       {
                                          ChangeIconStartPoint(); //Set Icon Start 
                                       }
                                      
                                    }
                                    else
                                    {
                                        document.getElementById(START_IMG).setAttribute("src",PATH_ARROW_IMG);
                                        ChangeDefaultIconStart();
                                        markerStart = null ;
                                        ChangeIconEndPoint(); // set new Icon  
                                    }
                                     
                                    ChangeIconSelectPoint();
                                    if(markerStart != undefined && markerEnd != undefined  &&  markerStart != markerEnd)
                                       ChangeIconEndPoint(); // Change Icon start
                                    
                                    RadioButtonCheckDisable();
                                  
                                }     
                               
                               
                               function EndHere()
                               {                 
                                    ClearPolyLinesRoute();                   
                                    if(document.getElementById(END_IMG).getAttribute("src").indexOf(PATH_ARROW_IMG) != -1 )
                                    {
                                                                           
                                       ChangeDefaultIconEnd(); // Change icon if exists                                      
                                       markerEnd = markerTmp ;
                                        
                                        
//                                       if(markerStart != undefined && markerEnd != undefined )
//                                         document.getElementById("<%=P5sLbtnStartRoutePlanning.ClientID%>").disabled = false;
                                       document.getElementById(END_IMG).setAttribute("src",PATH_ARROW_DOWN_IMG);   
                                      
                                       //Check End Point the like Select Point
                                        for(var i = markerPoints.length- 1; i >= 0; i--)
                                        {
                                           if(markerPoints == markerTmp )
                                           {
                                              document.getElementById(SELECT_IMG).setAttribute("src",PATH_ARROW_IMG);
                                              markerPoints.splice(i,1);                                              
                                              break;
                                           }
                                        }
                                        
                                                                                
                                        //Check End Point the like Start Point
                                       if(markerStart != undefined && markerEnd != undefined  &&  markerStart == markerEnd)
                                       {
                                          //Change Icon Start & End  
                                           var imagePath = markerEnd.DistributorTypeImage;
                                           var icon = new Image;                                          
                                           
                                           for(var j = 0 ; j < letters.length ; j++)
                                           {
                                                imagePath = imagePath.replace("/markerImages/","/icon/");
                                                imagePath = imagePath.replace( letters[j] ,"");                                        
                                           }                
                                           
                                           imagePath = imagePath.replace("_" ,"");       
                                                                                                    
                                           switch(imagePath)
                                            {
                                                case PATH_CWS_IMG:  
                                                  icon.image = "/markerImages/CoveredWS_.png";                                          
                                                  break;
                                                case PATH_WWS_IMG:                                        
                                                  icon.image = "/markerImages/CoveredWithoutWS_.png";  
                                                  break;
                                                case PATH_UC_IMG:                      
                                                  icon.image = "/markerImages/Uncovered_.png";                    
                                                  break;                                              
                                                case PATH_DISTRIBUTOR_DIRECT:                             
                                                  icon.image = "/markerImages/ND_.png";             
                                                  break;   
                                               case PATH_DISTRIBUTOR_INDIRECT:                             
                                                  icon.image = "/markerImages/SD_.png";    
                                                   break;
                                                case PATH_DISTRIBUTOR_DIRECT_SDN:                             
                                                  icon.image = "/markerImages/SDN_.png";             
                                                  break;                                          
                                            }                                                                            
                                          markerEnd.setIcon= icon.image;                                             
                                         
                                       }
                                       else
                                            ChangeIconEndPoint(); // set new Icon  
                                   
                              
                                    }
                                    else
                                    {
                                       ChangeDefaultIconEnd();
                                       markerEnd = null;
                                       ChangeIconStartPoint();
                                       document.getElementById(END_IMG).setAttribute("src",PATH_ARROW_IMG);
                                    }
                                    ChangeIconSelectPoint();
                                    RadioButtonCheckDisable();
                                }    
                                
                                
                                function SelectPoint()
                                {
                                     ClearPolyLinesRoute();
                                    
                                    if(document.getElementById(SELECT_IMG).getAttribute("src").indexOf(PATH_ARROW_IMG) != -1 )
                                    {
                                       document.getElementById(SELECT_IMG).setAttribute("src",PATH_ARROW_DOWN_IMG);
                                       markerPoints.push(markerTmp);    
                                       
                                       
                                       //Check Select Point like Start Point     
                                       if(markerStart != undefined && markerStart == markerTmp)
                                       {
                                       
                                          ChangeDefaultIconStart();
                                          markerStart = null;
                                          document.getElementById(START_IMG).setAttribute("src",PATH_ARROW_IMG);                             
                                       }
                                       
                                       //Check Select Point like End Point     
                                       if(markerEnd != undefined && markerEnd== markerTmp)
                                       {
                                           ChangeDefaultIconEnd();
                                           markerEnd = null;
                                           document.getElementById(END_IMG).setAttribute("src",PATH_ARROW_IMG);                    
                                       }
                                                                       
                                    }
                                    else
                                    {
                                        document.getElementById(SELECT_IMG).setAttribute("src",PATH_ARROW_IMG);                                       
                                        for(var i = markerPoints.length- 1; i >= 0; i--)
                                        {
                                           if(markerPoints[i] == markerTmp )
                                           {
                                              ChangeDefaultIconSelect();
                                              markerPoints.splice(i,1);
                                              break;
                                           }
                                        }
                                    }
                                    ChangeIconSelectPoint();
                                    RadioButtonCheckDisable();
                                }     
                                
                                
                               
                              
                              function  LongTudeSelect()
                              {
                                 if(distributorSelectMove == null)
                                 {
                                    return;
                                 }
                                 
                                   //Remove point thu?c NPP moved
                                   for(var j = pointCDs.length -1; j >=0 ; j--){
                                        
                                        if(distributorSelectMove.objectCommunes.ID == pointCDs[j].objectCommunes.DistributorCD)
                                        { 
                                            //Call fns remove color
                                            LongTudeUnSelect(pointCDs[j]);
                                            var icon = new VIcon();                                               
                                            icon.image = "/icon/longTude.png";
                                            pointCDs[j].setIcon(icon);                           
                                            pointCDs.splice(j,1);                                                
                                        }                                
                                   }
                                       
                                        
                                  //arrDistributorNetwork                                      
                                  for(var i = 0; i < arrDistributorNetwork.length ; i++)
                                  {
                                  
                                     if(arrDistributorNetwork[i].DistributorCD == distributorSelectMove.objectCommunes.ID)
                                     {
                                             //duyet point select move
                                              for(var j = 0; j < pointCDs.length; j++)
                                              {                                                                                  
                                                    //duyet list DistributorNetworkPolygon to find Polygon select to change color                                                                                                                
                                                     for(var k = 0; k < arrDistributorNetwork.length; k++)
                                                     {
                                                        //set color Polygon n?u nó ?? ???c add vào OverLay 
                                                        if(arrDistributorNetwork[k].AreaCD == pointCDs[j].objectCommunes.CD && arrDistributorNetwork[k].IsRemove == true)
                                                        {
                                                              //set color and redraw
                                                              arrDistributorNetwork[k].Polygon.fillColor = arrDistributorNetwork[i].Color;
                                                              //arrDistributorNetwork[k].Polygon.removeLines(); 
                                                              arrDistributorNetwork[k].Polygon.setMap(null);    
                                                              arrDistributorNetwork[k].Polygon.setMap(map);                                                                                                                   
                                                           //   arrDistributorNetwork[k].Polygon.drawLines(arrDistributorNetwork[k].Polygon.ll, false);                                                                                                                       
                                                        }
                                                     }
                                               }  
                                                                                         
                                            break;
                                       }
                                  }
                                  
                                  ViewInfoAfterMove();
                              }
                              
                               
                              function  LongTudeUnSelect(obj)
                              {                                  
                                 if(distributorSelectMove == null)
                                 {
                                    return;
                                 }
                                 
                                 //set color base if user not select distributor
                                   for(var k = 0; k < arrDistributorNetwork.length; k++)
                                   {
                                        if(arrDistributorNetwork[k].IsRemove == true && 
                                        arrDistributorNetwork[k].AreaCD == obj.objectCommunes.CD) //Remove Polygon n?u nó ?? ???c add vào OverLay
                                        {
                                      
                                             //set color and redraw
                                             arrDistributorNetwork[k].Polygon.fillColor = arrDistributorNetwork[k].Color;
                                             //arrDistributorNetwork[k].Polygon.removeLines();  
                                             arrDistributorNetwork[k].Polygon.setMap(null);             
                                             arrDistributorNetwork[k].Polygon.setMap(map);                                                                                                         
                                          //   arrDistributorNetwork[k].Polygon.drawLines(arrDistributorNetwork[k].Polygon.ll, false);   
                                                                                                                                                              
                                        }
                                   } 
                                       
                                        
                                    
                                     
                                 if(pointCDs.length <= 0)
                                 {  
                                 
                                
                                    //clear select icon &img, hide btn Accept
                                    markerStart = distributorSelectMove;
                                    ChangeDefaultIconStart(); //Clear Icon Start if exists 
                                    document.getElementById(START_IMG).setAttribute("src",PATH_ARROW_IMG);   
                                    distributorSelectMove = null;   
                                    //PnlAccept
                                    document.getElementById("PnlAccept").style.display = "none";
                                 }
                                 
                                  ViewInfoAfterMove();
                              
                              }
                                                             
                                 
                                 
                               function ViewInfoAfterMove()
                               {
                              
                                    if(pointCDs.length == 0 || distributorSelectMove == null)
                                    {
                                        //reset value
                                        document.getElementById("idTitleCurrent").innerHTML = ""; 
                                        document.getElementById("idTitleMoved").innerHTML = "";  
                                        document.getElementById("idRadiusMoved").innerHTML = "";  
                                        document.getElementById("idAreadMoved").innerHTML = "";
                                        document.getElementById("idNumberOfCustomerMoved").innerHTML = "";  
                                        document.getElementById("idWholesalesMoved").innerHTML = "";  
                                        document.getElementById("idPopulationMoved").innerHTML = ""; 
                                        document.getElementById("idAMSMoved").innerHTML = "";                                       
                                        return;
                                    }
                               
                                    if( markerTmp == distributorSelectMove)
                                    {
                                     //get current information of distributor select
                                      var wholesales = distributorSelectMove.WHOLE_SALES.replace(/,/gi,"");
                                      var customer = distributorSelectMove.CUSTOMERS.replace(/,/gi,"");
                                      var population = distributorSelectMove.POPULATION.replace(/,/gi,"");
                                      var area = distributorSelectMove.AREA_COVERED.replace(/,/gi,"");
                                      var ams =  distributorSelectMove.AMS.replace(/,/gi,"");
                                     
                                      var maxDistance = eval(distributorSelectMove.RADIUS.replace(/,/gi,""))*1000;
                                    
                                      
                                      for(var i = 0; i < distributorNetworkPoint.VMarkers.length ; i++)
                                      {
                                           if(distributorNetworkPoint.VMarkers[i].getIcon().image == "/icon/LongTudeSelect.png")
                                           {
                                               wholesales = eval(wholesales) + eval(distributorNetworkPoint.VMarkers[i].Wholesales.replace(/,/gi,""));
                                               customer = eval(customer) + eval(distributorNetworkPoint.VMarkers[i].Customer.replace(/,/gi,""));
                                               population = eval(population) + eval(distributorNetworkPoint.VMarkers[i].Population.replace(/,/gi,""));
                                               area = eval(area) + eval(distributorNetworkPoint.VMarkers[i].Area.replace(/,/gi,""));                                                 
                                               ams =  eval(ams) + eval(distributorNetworkPoint.VMarkers[i].Ams.replace(/,/gi,""));     
                                              
                                               var tmpDistance =  CalculateDistance(distributorSelectMove.LONGITUDE_LATITUDE, distributorNetworkPoint.VMarkers[i].LONGITUDE_LATITUDE );
                                         
                                               if(tmpDistance > maxDistance)
                                               {   
                                                  maxDistance = tmpDistance;
                                               }   
                                           } 
                                      }
                                       
                                      //Cal Radius                                       
                                      //CalculateDistance
                                      
                                     //set value after move
                                      document.getElementById("idTitleCurrent").innerHTML = "Before"; 
                                      document.getElementById("idTitleMoved").innerHTML = "After";  
                                      document.getElementById("idAreadMoved").innerHTML = eval(area).toFixed(2) + " km2";  
                                      document.getElementById("idRadiusMoved").innerHTML = eval(maxDistance/1000).toFixed(2) + " km";  
                                      document.getElementById("idNumberOfCustomerMoved").innerHTML = Comma(customer);  
                                      document.getElementById("idWholesalesMoved").innerHTML = Comma(wholesales);  
                                      document.getElementById("idPopulationMoved").innerHTML = Comma(population); 
                                      document.getElementById("idAMSMoved").innerHTML = Comma(ams); 
                                    
                                    }  
                                    else
                                    {   
                                    
                                     //get current information of distributor temp
                                      var wholesales = markerTmp.WHOLE_SALES.replace(/,/gi,"");
                                      var customer = markerTmp.CUSTOMERS.replace(/,/gi,"");
                                      var population = markerTmp.POPULATION.replace(/,/gi,"");
                                      var area = markerTmp.AREA_COVERED.replace(/,/gi,"");
                                      var ams =  markerTmp.AMS.replace(/,/gi,"");
                                      var maxDistance = eval(markerTmp.RADIUS.replace(/,/gi,""))*1000;
                                      var flag = false;
                                    
                                    
                                      for(var i = 0; i < distributorNetworkPoint.VMarkers.length ; i++)
                                      { 
                                    
                                           if( distributorNetworkPoint.VMarkers[i].DistributorCD ==  markerTmp.ID 
                                           &&  distributorNetworkPoint.VMarkers[i].getIcon().image == "/icon/LongTudeSelect.png")
                                           {               
                                                                                                                                  
                                               wholesales = eval(wholesales) - eval(distributorNetworkPoint.VMarkers[i].Wholesales.replace(/,/gi,""));
                                               customer = eval(customer) - eval(distributorNetworkPoint.VMarkers[i].Customer.replace(/,/gi,""));
                                               population = eval(population) - eval(distributorNetworkPoint.VMarkers[i].Population.replace(/,/gi,""));
                                               area = eval(area) - eval(distributorNetworkPoint.VMarkers[i].Area.replace(/,/gi,""));                                                 
                                               ams =  eval(ams) - eval(distributorNetworkPoint.VMarkers[i].Ams.replace(/,/gi,""));   
                                               var tmpDistance =  CalculateDistance(markerTmp.LONGITUDE_LATITUDE,distributorNetworkPoint.VMarkers[i].LONGITUDE_LATITUDE );
                                               flag = true;
                                               if(tmpDistance > maxDistance)
                                               {   
                                                  maxDistance = tmpDistance;
                                               }   
                                           } 
                                      }
                                      
                                      maxDistance = 0;
                                      for(var i = 0; i < distributorNetworkPoint.VMarkers.length ; i++)
                                      { 
                                    
                                           if( distributorNetworkPoint.VMarkers[i].DistributorCD ==  markerTmp.ID 
                                                &&  distributorNetworkPoint.VMarkers[i].getIcon().image == "/icon/longTude.png")
                                           {             
                                               var tmpDistance =  CalculateDistance(markerTmp.LONGITUDE_LATITUDE, distributorNetworkPoint.VMarkers[i].LONGITUDE_LATITUDE );                                      
                                               if(tmpDistance > maxDistance)
                                               {   
                                                  maxDistance = tmpDistance;
                                               }   
                                           } 
                                      }
                                    
                                    
                                    
                                    
                                    
                                       if(flag)
                                       {
                                           if( eval(area) < 0)
                                           {
                                              area = 0;
                                              maxDistance = 0;
                                           }
                                           
                                           if( eval(population) < 0)
                                           {
                                              population = 0;
                                           }
                                           
                                         //set value after move
                                          document.getElementById("idTitleCurrent").innerHTML = "Before"; 
                                          document.getElementById("idTitleMoved").innerHTML = "After";  
                                          document.getElementById("idAreadMoved").innerHTML = eval(area).toFixed(2) + " km2";  
                                          document.getElementById("idRadiusMoved").innerHTML = eval(maxDistance/1000).toFixed(2) + " km";  
                                          document.getElementById("idNumberOfCustomerMoved").innerHTML = Comma(customer);  
                                          document.getElementById("idWholesalesMoved").innerHTML = Comma(wholesales);  
                                          document.getElementById("idPopulationMoved").innerHTML = Comma(population); 
                                          document.getElementById("idAMSMoved").innerHTML = Comma(ams); 
                                       }
                                       
                                    }
                                    
                                    
                                                                     
                               }
                                
                                
                                 
                                 
                               function MoveHere()
                               {     
                                  //console.log(pointCDs);
                                    markerStart = distributorSelectMove;
                                    ChangeDefaultIconStart(); //Clear Icon Start if exists 
                                    if(document.getElementById(START_IMG).getAttribute("src").indexOf(PATH_ARROW_IMG) != -1 )
                                    {    
                                    
                                    
                                                                                
                                      //Remove point thu?c NPP moved
                                       var flag = false;
                                       for(var j = pointCDs.length -1; j >=0 ; j--){
                                            //console.log(pointCDs[j]);
                                            if(markerTmp.ID == pointCDs[j].DistributorCD)
                                            { 
                                                //Call fns remove color
                                                LongTudeUnSelect(pointCDs[j]);
                                                var icon;                                              
                                                icon = "/icon/longTude.png";
                                                pointCDs[j].icon = icon;                           
                                                pointCDs.splice(j,1);        
                                                flag = true;                                        
                                            }                                
                                       }
                                       
                                       
                                         //Check m?ng point select
                                        if(pointCDs.length <= 0 && flag == true)
                                        {
                                            alert("Khu vực sát nhập thuộc về nhà phân phối này !.");
                                            return;
                                        }
                                        
                                       
                                         //Check m?ng point select
                                        if(pointCDs.length <= 0)
                                        {
                                        
                                            alert("Cần chọn khu vực sát nhập.");
                                            return;
                                        }
                                    
                                    
//                                        //Check m?ng point select
//                                        if(pointCDs.length <= 0)
//                                        {
//                                            alert("C?n ch?n khu v?c ?? sát nh?p !.");
//                                            return;
//                                        }
                                      // alert(document.getElementById(START_IMG).getAttribute("src"));                    
                                       distributorSelectMove = markerTmp ; 
                                       document.getElementById(START_IMG).setAttribute("src",PATH_ARROW_DOWN_IMG);                                      
                                       
                                       var imagePath = distributorSelectMove.icon;
                                       var icon;
                                       
                                       switch(imagePath)
                                       {                                                                                     
                                            case PATH_DISTRIBUTOR_DIRECT:                             
                                              icon = "/markerImages/ND_A.png";             
                                              break;
                                            case PATH_DISTRIBUTOR_INDIRECT:                             
                                              icon = "/markerImages/SD_A.png";             
                                              break;
                                              
                                             case PATH_DISTRIBUTOR_DIRECT_SDN:                             
                                              icon = "/markerImages/SDN_A.png";             
                                              break;
                                        }                                                                            
                                       distributorSelectMove.icon = icon;
                                       //PnlAccept
                                       document.getElementById("PnlAccept").style.display = "block";
                                       
                               
                                      
                                      //arrDistributorNetwork                                      
                                      for(var i = 0; i < arrDistributorNetwork.length ; i++)
                                      {
                                      
                                        //console.log(distributorSelectMove);
                                         if(arrDistributorNetwork[i].DistributorCD == distributorSelectMove.ID)
                                         {
                                                 //duyet point select move
                                                  for(var j = 0; j < pointCDs.length; j++)
                                                  {                                                                                  
                                                        //duyet list DistributorNetworkPolygon to find Polygon select to change color                                                                                                                
                                                         for(var k = 0; k < arrDistributorNetwork.length; k++)
                                                         {
                                                            //set color Polygon n?u nó ?? ???c add vào OverLay 
                                                         //   console.log(pointCDs[j]);
                                                            if(arrDistributorNetwork[k].AreaCD == pointCDs[j].CD && arrDistributorNetwork[k].IsRemove == true)
                                                            {
                                                                  //set color and redraw
                                                                
                                                            
                                                                  arrDistributorNetwork[k].Polygon.fillColor = arrDistributorNetwork[i].Color;                                                         
                                                                 // arrDistributorNetwork[k].Polygon.removeLines();
                                                                 arrDistributorNetwork[k].Polygon.setMap(null);    
                                                                  arrDistributorNetwork[k].Polygon.setMap(map);                                                                                                                     
                                                                 // arrDistributorNetwork[k].Polygon.drawLines(arrDistributorNetwork[k].Polygon.ll, false);                                                                                                                       
                                                            }
                                                         }
                                                   }  
                                                                                             
                                                break;
                                           }
                                      }
                                        
                                        ViewInfoAfterMove();
                                      
                                    }
                                    else
                                    {
                                      
                                  
                                       document.getElementById(START_IMG).setAttribute("src",PATH_ARROW_IMG);   
                                       distributorSelectMove = null;   
                                       //PnlAccept
                                       document.getElementById("PnlAccept").style.display = "none";
                                       
                                       //set color base if user not select distributor
                                       for(var k = 0; k < arrDistributorNetwork.length; k++)
                                       {
                                            if(arrDistributorNetwork[k].IsRemove == true) //Remove Polygon n?u nó ?? ???c add vào OverLay
                                            {
                                                 //set color and redraw
                                                 arrDistributorNetwork[k].Polygon.fillColor = arrDistributorNetwork[k].Color;
                                                // arrDistributorNetwork[k].Polygon.removeLines();  
                                                arrDistributorNetwork[k].Polygon.setMap(null);   
                                                arrDistributorNetwork[k].Polygon.setMap(map);                                                                                                                   
                                               //  arrDistributorNetwork[k].Polygon.drawLines(arrDistributorNetwork[k].Polygon.ll, false);                                                                                                                       
                                            }
                                       } 
                                       
                                       distributorSelectMove = null;
                                       ViewInfoAfterMove();
                                                        
                                       
                                    }                   
                                    
                                    
                                     
                                }     
                                
                                
    </script>

    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:HiddenField ID="P5sLocaltionCountry" runat="server" />
        <asp:UpdatePanel ID="P5sUpanelMainTop" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="2" border="0" width="100%">
                    <tr>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            <table cellpadding="0" cellspacing="0" border="0" width="255px">
                                <tr>
                                    <td style="padding: 0">
                                        <asp:TextBox ID="P5sTxtCountry" runat="server" CssClass="TextBox" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            <asp:DropDownList ID="P5sDdlREGION_CD" CssClass="input-sm" Height="28px" Width="120px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="P5sDdlREGION_CD_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            <asp:DropDownList ID="P5sDdlAREA_CD" CssClass="input-sm" Width="120px" Height="28px" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="P5sDdlAREA_CD_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                            <table cellpadding="0" cellspacing="0" border="0" width="250px">
                                <tr>
                                    <td style="padding: 0">
                                        <asp:DropDownList ID="P5sDdlPROVINCE_CD" CssClass="input-sm" Width="120px" Height="28px" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="P5sDdlPROVINCE_CD_SelectedIndexChanged">
                                        </asp:DropDownList>
                                       <%-- <asp:TextBox ID="P5sTxtPROVINCE_CD" runat="server" CssClass="TextBox" MaxLength="50"
                                            Width="250px"></asp:TextBox>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="height: 26px; white-space: nowrap; width: 94%; vertical-align: top">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="P5sUpanelMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="P5sPnlMain" runat="server">
                    <div id="container" style="white-space: nowrap; display: block; position: absolute;">
                    </div>
                    <table id="TbLefPanel" cellpadding="0" cellspacing="0" border="0" width="300px" height="100%"
                        style="z-index: 200; position: absolute; background-color: White">
                        <tr>
                            <td style="width: 1%; white-space: nowrap; vertical-align: top">
                                <div style="padding-left: 325px">
                                    <img alt="" id="ImgBackForward" src="images/back.png" style="cursor: pointer" />
                                </div>
                                <ajaxToolkit:Accordion ID="P5sMyAccordion" runat="Server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                    FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" AutoSize="Fill"
                                    Width="350px" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                                    <Panes>
                                        <ajaxToolkit:AccordionPane ID="P5sAccPnlCoverage" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <div style="font-size: medium; font-style: italic; color: Green; cursor: pointer;">
                                                    Coverage</div>
                                            </Header>
                                            <Content>
                                                <table cellpadding="0" cellspacing="2" border="0" width="275">
                                                    <tr>
                                                        <td style="height: 100%; white-space: nowrap; width: 100%; vertical-align: top">
                                                            <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                                <tr  style=" display:none;">
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        Distributor Type:
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:DropDownList ID="P5sDdlDistributorType" Width="130px" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr  style=" display:none;">
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        Point of Interest:
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="padding-left: 5px;  display:none;">
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkBank" Text="Bank" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkSchool" Text="School" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="padding-left: 5px;  display:none;">
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkATM" Text="ATM" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkBSBF" Text="BSBF" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="padding-left: 5px; display:none;">
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkPostOffice" Text="Post Office" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkStateHighway" Text="Province Highway" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="padding-left: 5px; display:none;">
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkHospital" Text="Hospital" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkNationlHighway" Text="National Highway" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="padding-left: 5px; display:none;">
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        <asp:CheckBox ID="P5sChkMarket" Text="Market" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr  style=" display:none;">
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        Population:
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="padding-left: 5px; display:none;">
                                                                    <td colspan="3" style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        min
                                                                        <asp:TextBox ID="P5sTxtPopulationMin" onkeypress="return isNumberKey(this)" onkeyup="javascript:this.value=Comma(this.value);"
                                                                            runat="server" CssClass="TextBox" MaxLength="9" Width="60px"></asp:TextBox>
                                                                        to max:
                                                                        <asp:TextBox ID="P5sTxtPopulationMax" onkeypress="return isNumberKey(this);" onkeyup="javascript:this.value=Comma(this.value);"
                                                                            runat="server" CssClass="TextBox" MaxLength="9" Width="60px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style=" display:none;">
                                                                    <td colspan="3" style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        By: &nbsp;&nbsp;
                                                                        <asp:CheckBox ID="P5sChkWard" Text="Ward" runat="server" />
                                                                        <asp:CheckBox ID="P5sChkTown" Text="Town" runat="server" />
                                                                        <asp:CheckBox ID="P5sChkCommune" Text="Commune" runat="server" />
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                     <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                                        <asp:Button ID="P5sLbtnSearch" runat="server" OnClick="P5sLbtnSearch_Click" Text="Search"/>
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 1%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 100%; white-space: nowrap; width: 96%; vertical-align: top">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="vertical-align: top; white-space: nowrap; text-align: center">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="P5sPnlSearchCoverage" Visible="false" runat="server">
                                                                            <table cellpadding="0" cellspacing="2" border="1" width="275px" style="border-collapse: collapse;
                                                                                border: 1px solid black;">
                                                                                <tr style="border-collapse: collapse; border: 1px solid black;">
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%;">
                                                                                        <asp:Label ID="LabP5sLblTitleWard" Font-Size="Small" Font-Bold="true" runat="server"
                                                                                            Text="Wards"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%;">
                                                                                        <asp:Label ID="P5sLblTitleTowns" Font-Size="Small" Font-Bold="true" runat="server"
                                                                                            Text="Towns"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%;">
                                                                                        <asp:Label ID="P5sLblTitleCommunes" Font-Size="Small" Font-Bold="true" runat="server"
                                                                                            Text="Comm"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="border-collapse: collapse; border: 1px solid black;">
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 160px;
                                                                                        text-align: left">
                                                                                        Total
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblTotalWard" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblTotalTowns" runat="server"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblTotalCommunes" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <br />
                                                                                <tr>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">
                                                                                        <asp:Image ID="P5sImgCoveredWS" ImageUrl="~/icon/CoveredWS.png" runat="server" />
                                                                                        <asp:LinkButton ID="P5sLbtnCoveredWS" runat="server" OnClientClick="return HideShowCoverageWS()">Covered with WS</asp:LinkButton>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblCoveredWardWS" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblCoveredTownsWS" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblCoveredCommunesWS" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">
                                                                                        <asp:Image ID="P5sImgCoveredWithoutWS" ImageUrl="~/icon/CoveredWithoutWS.png" runat="server" />
                                                                                        <asp:LinkButton ID="P5sLbtnCoveredWithoutWS" runat="server" OnClientClick="return HideShowCoverageWithoutWS()">Covered W/o WS</asp:LinkButton>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblCoveredWardWithoutWS" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblCoveredTownsWithoutWS" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5slblCoveredCommunesWithoutWS" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: left">
                                                                                        <asp:Image ID="P5sImgUncovered" ImageUrl="~/icon/Uncovered.png" runat="server" />
                                                                                        <asp:LinkButton ID="P5sLbtnUnCovered" runat="server" OnClientClick="return HideShowUncoverage()"> Uncovered </asp:LinkButton>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5sLbtnUncoveredWards" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5sLbtnUncoveredTowns" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 1%; text-align: right;">
                                                                                        <asp:Label ID="P5sLbtnUncoveredCommunes" runat="server" Text="0"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <br />
                                                                            <asp:Panel ID="P5sPnlNumberOfDistributor" runat="server" Width="275px">
                                                                                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100%">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left">
                                                                                            <span style="font-size: medium; font-weight: bold">Number of Distributor :</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left;">
                                                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                                <tr>
                                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left;">
                                                                                                        <asp:Image ID="P5sImgDistributorDirect" ImageUrl="~/icon/ND.png" runat="server" />
                                                                                                        <asp:Label ID="P5sLblDistributorDirect" Font-Size="Medium" Font-Bold="true" runat="server"
                                                                                                            Text="Number of Distributor [41]"></asp:Label>
                                                                                                    </td>
                                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left;">
                                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <%--                                                                                                <tr>
                                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left;">
                                                                                                        <asp:Image ID="P5sImgDistributorInDirectSDW" ImageUrl="~/icon/SD.png" runat="server" />
                                                                                                        <asp:Label ID="P5sLblDistributorInDirectSDW" Font-Size="Medium" Font-Bold="true"
                                                                                                            runat="server" Text="Number of Distributor [41]"></asp:Label>
                                                                                                    </td>
                                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left;">
                                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    </td>
                                                                                                </tr>--%>
                                                                                                <tr>
                                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left;">
                                                                                                        <asp:Image ID="P5sImgDistributorInDirectSDN" ImageUrl="~/icon/SDN.png" runat="server" />
                                                                                                        <asp:Label ID="P5sLblDistributorInDirectSDN" Font-Size="Medium" Font-Bold="true"
                                                                                                            runat="server" Text="Number of Distributor [41]"></asp:Label>
                                                                                                    </td>
                                                                                                    <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: left;">
                                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100%">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                                                                            </asp:Panel>
                                                                            <br />
                                                                            <asp:Panel ID="P5sPnlParentPOIS" runat="server" Visible="false" Width="275px">
                                                                                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100%">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; text-align: center">
                                                                                            <asp:Label ID="P5sLblTotalPOIs" Font-Size="Medium" Font-Bold="true" runat="server"
                                                                                                Text="Total POIs [41]:"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100%">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                                                                                <br />
                                                                                <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                                                    <asp:Panel ID="P5sPnlBank" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="P5sImgBank" ImageUrl="~/icon/Bank.png" runat="server" />
                                                                                                Bank
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblTotalBank" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="P5sPnlATM" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="P5sImgATM" ImageUrl="~/icon/ATM.png" runat="server" />
                                                                                                ATM
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblTotalATM" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="P5sPnlStateHighway" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="P5sImgHighway" ImageUrl="~/icon/StateHighway.png" runat="server" />
                                                                                                Province Highway
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblTotalStateHighway" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="P5sPnlSchool" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="P5sImgSchool" ImageUrl="~/icon/school.png" runat="server" />
                                                                                                School
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblTotalSchool" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="P5sPnlPostOffice" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="P5sImgPostOffice" ImageUrl="~/icon/postoffice.png" runat="server" />
                                                                                                Post Office
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblPostOffice" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="P5sPnlNationlHighway" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="P5sImgNationlHighway" ImageUrl="~/icon/nationalhighway.png" runat="server" />
                                                                                                National Highway
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblNationlHighway" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="P5sPnlBSBF" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="P5sImgBSBF" ImageUrl="~/icon/BSBF.png" runat="server" />
                                                                                                BSBF
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblBSBF" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="P5sPnlHospital" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="P5sImgHospital" ImageUrl="~/icon/Hospital.png" runat="server" />
                                                                                                Hospital
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblHospital" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                    <asp:Panel ID="P5sPnlMarket" runat="server">
                                                                                        <tr>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 200px;
                                                                                                text-align: left">
                                                                                                <asp:Image ID="Image2" ImageUrl="~/icon/Market.png" runat="server" />
                                                                                                Market
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                <asp:Label ID="P5sLblTotalMarket" runat="server" Text="4"></asp:Label>
                                                                                            </td>
                                                                                            <td style="vertical-align: top; white-space: nowrap; height: 100%; width: 100px">
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </asp:Panel>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <span style="font-size: medium; font-style: italic; color: Green; cursor: pointer;
                                                    text-decoration: none; display:none;" onclick="javascript:window.location.href = '/COVERAGE_NEW_V2.aspx';">
                                                    Coverage by Sales </span>
                                            </Header>
                                            <Content>
                                                <table cellpadding="0" cellspacing="2" border="0" width="100%" style="display:none;">
                                                    <tr>
                                                        <td style="vertical-align: top; white-space: nowrap;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; padding-left: 140px; display: none;">
                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sSettingAndReports)%>
                                                            <asp:LinkButton ID="LinkButton1" OnClick="P5sCoverageBySales_Click" runat="server">Coverage by Sales</asp:LinkButton>
                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sSettingAndReports)%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <ajaxToolkit:AccordionPane ID="P5sAccPnlRoutePlanning" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <div style="font-size: medium; font-style: italic; color: Green; cursor: pointer; display:none;">
                                                    Route Planning
                                                </div>
                                            </Header>
                                            <Content>
                                                <div id="MainDivFindPath" style="vertical-align: top; white-space: nowrap; padding-left: 0px;
                                                    overflow: hidden; display:none;" >
                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td style="vertical-align: top; white-space: nowrap;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="vertical-align: top; white-space: nowrap; padding-left: 130px">
                                                                <input id="P5sChkAutoOrManual" type="checkbox" />
                                                                Auto
                                                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnStartRoutePlanning)%>
                                                                <asp:LinkButton ID="P5sLbtnStartRoutePlanning" OnClientClick="return FindMap()" runat="server">Start Route Planning</asp:LinkButton>
                                                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnStartRoutePlanning)%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="vertical-align: top; white-space: nowrap; text-align: right">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="vertical-align: top; white-space: nowrap;">
                                                                <input type="radio" name="Distance" id="DistanceFromDistributor" checked="checked"
                                                                    value="0">Distance from Distributor<br>
                                                                <input type="radio" name="Distance" id="DistanceNearestTownCommune" value="1">Distance
                                                                from nearest town/commune
                                                                <br />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;covered
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="vertical-align: top; white-space: nowrap; padding-left: 5px">
                                                                <table cellpadding="0" cellspacing="5px" border="0" width="270px">
                                                                    <tr>
                                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top; text-align: left">
                                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnFindRouteDistance)%>
                                                                            <asp:LinkButton ID="P5sLbtnFindRouteDistance" OnClientClick="return FindRouteNearest()"
                                                                                runat="server">Find route</asp:LinkButton>
                                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnFindRouteDistance)%>
                                                                        </td>
                                                                        <td style="height: 26px; white-space: nowrap; width: 98%; vertical-align: top; text-align: left">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top; text-align: left">
                                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnClearAllICon)%>
                                                                            <asp:LinkButton ID="P5sLbtnClearAllICon" OnClientClick="return ClearAllIconSelect()"
                                                                                runat="server">Clear</asp:LinkButton>
                                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnClearAllICon)%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="vertical-align: top; white-space: nowrap; padding-left: 0px;">
                                                                <asp:Panel ID="PanelScroll" runat="server" Style="display: none; font-family: Tahoma;">
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <ajaxToolkit:AccordionPane ID="P5sAccPnlDistributorNetwork" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <div style="font-size: medium; font-style: italic; color: Green; cursor: pointer; display:none;">
                                                    Distributor Network
                                                </div>
                                            </Header>
                                            <Content>
                                                <table cellpadding="0" cellspacing="2" border="0" width="200px;">
                                                    <tr style="display: none;">
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                            <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                                <tr>
                                                                    <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnShowHideCentralPoint)%>
                                                                        <asp:LinkButton ID="P5sLbtnShowHideCentralPoint" runat="server" Enabled="false" OnClientClick="return ShowHideCentralPoint()">Show Central Points</asp:LinkButton>
                                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnShowHideCentralPoint)%>
                                                                    </td>
                                                                    <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnShowHideDistributorNetworkPoint)%>
                                                                        <asp:LinkButton ID="P5sLbtnShowHideDistributorNetworkPoint" runat="server" Enabled="false"
                                                                            OnClientClick="return ShowHideDistributorNetworkPoint()">Show Points</asp:LinkButton>
                                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnShowHideDistributorNetworkPoint)%>
                                                                    </td>
                                                                    <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 98%">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 97%">
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                            &nbsp; &nbsp; By:
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%; display:none;">
                                                            <asp:RadioButtonList ID="P5sRdBtnlist" AutoPostBack="false" runat="server" OnSelectedIndexChanged="P5sRdBtnlist_SelectedIndexChanged">
                                                                <asp:ListItem Value="Province" Selected="True" Text="Province"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 97%">
                                                            &nbsp;
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                            &nbsp; &nbsp; &nbsp; &nbsp;
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%;
                                                            padding-left: 10px">
                                                            <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                                <tr>
                                                                    <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%; display:none;">
                                                                        <asp:Button ID="P5sLbtnNetWorkDistributorSearch" runat="server" OnClick="P5sLbtnNetWorkDistributorSearch_Click" Text="Search"/>
                                                                    </td>
                                                                    <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                                        <div id="PnlAccept" style="display: none;">
                                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnAccept)%>
                                                                            <asp:LinkButton ID="P5sLbtnAccept" OnClick="P5sLbtnAccept_Click" OnClientClick="if(!confirm('Chuy?n nh?ng khu v?c ?? ch?n ?')) return false;"
                                                                                runat="server">Accept</asp:LinkButton>
                                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnAccept)%>
                                                                        </div>
                                                                    </td>
                                                                    <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 98%">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 1%">
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; text-align: left; width: 97%">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <div style="font-size: medium; font-style: italic; color: Green; cursor: pointer; display:none;">
                                                    Load Distributor
                                                </div>
                                            </Header>
                                            <Content>
                                                <br />
                                                <table cellpadding="0" cellspacing="2" border="0" width="300px; display:none;">
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            Vùng<span style="color: Red; white-space: nowrap;"> * </span>:
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; vertical-align: top; width: 200px">
                                                            <asp:TextBox ID="P5sTxtRegionCD" runat="server" CssClass="TextBox" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            Khu vực<span style="color: Red; white-space: nowrap;"> * </span>:
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="P5sTxtAreaCD" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                                            <%=P5sActArea.L5sShowAddAll("P5sTxtAreaCD_Add")%>
                                                            <%=P5sActArea.L5sShowRemoveAll("P5sTxtAreaCD_Remove")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 26px; white-space: nowrap; width: 1%; vertical-align: top">
                                                            Nhà phân phối<span style="color: Red; white-space: nowrap;"> * </span>:
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="P5sTxtDistributor" runat="server" CssClass="TextBox" Width="300px"></asp:TextBox>
                                                            <%=P5sActDistributor.L5sShowAddAll("P5sTxtDistributor_Add")%>
                                                            <%=P5sActDistributor.L5sShowRemoveAll("P5sTxtDistributor_Remove")%>
                                                        </td>
                                                    </tr>
                                                    <td style="vertical-align: top; white-space: nowrap; padding-left: 50px">
                                                        <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnLoadDistributor)%>
                                                        <asp:LinkButton ID="P5sLbtnLoadDistributor" OnClick="P5sLbtnLoadDistributor_Click"
                                                            runat="server">Load Distributor</asp:LinkButton>
                                                        <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnLoadDistributor)%>
                                                    </td>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <ajaxToolkit:AccordionPane ID="AccordionPaneMapping" runat="server" HeaderCssClass="accordionHeader"
                                            ContentCssClass="accordionContent">
                                            <Header>
                                                <span style="font-size: medium; font-style: italic; color: Green; cursor: pointer;
                                                    text-decoration: none; display:none;" onclick="javascript:window.location.href = '/MAPPING_HIERARCHY.aspx';">
                                                    Data Mapping </span>
                                                <div style="font-size: medium; font-style: italic; color: Green; cursor: pointer;">
                                                </div>
                                            </Header>
                                            <Content>
                                                <br />
                                                <table cellpadding="0" cellspacing="2" border="0" width="100%; display:none;">
                                                    <tr>
                                                        <td style="vertical-align: top; white-space: nowrap;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="vertical-align: top; white-space: nowrap; padding-left: 140px; display: none;">
                                                            <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sRemapRouteList)%>
                                                            <asp:LinkButton ID="P5sRemapRouteList" OnClick="P5sRemapRouteList_Click" runat="server">Remap route list</asp:LinkButton>
                                                            <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sRemapRouteList)%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                        <ajaxToolkit:AccordionPane ID="P5sAccPnlDCoverageProgressSettingAndReports" runat="server"
                                            HeaderCssClass="accordionHeader" ContentCssClass="accordionContent">
                                            <Header>
                                                <span style="font-size: medium; font-style: italic; color: Green; cursor: pointer;
                                                    text-decoration: none; display:none;" onclick="javascript:window.location.href = '/Report/CustomerList.aspx';">
                                                    Setting and Reports </span>
                                            </Header>
                                            <Content>
                                                <table cellpadding="0" cellspacing="2" border="0" width="100%; display:none;">
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
