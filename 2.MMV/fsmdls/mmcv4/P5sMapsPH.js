function CPolygon(myMap, StrObject, lineColor, lineWidth, fillColor, fillOpacity) {
    this.myMap = myMap;
    this.Polygons = b(myMap, StrObject, lineColor, lineWidth, fillColor, fillOpacity);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;
    CTracking.prototype.mInfowindow = new Array();

    function b(myMap, v, lineColor, lineWidth, fillColor) {
        var object = $.parseJSON(v);
        var pWS = new Array();
        for (var i = 0; i < object.length; i++) {

            var arr = object[i].Polygon.split("-");
            for (var j = 0; j < arr.length; j++) {
                var polygon;
                if (fillOpacity != undefined) {
                    //polygon = new VPolygon(eval(arr[j]), lineColor, lineWidth, fillColor, 1, fillOpacity);
                    polygon = new google.maps.Polygon({
                        paths: eval(arr[j]),
                        draggable: false,
                        editable: false,
                        strokeColor: lineColor,
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillColor: fillColor,
                        fillOpacity: 0.35
                    });
                }
                else {

                    //polygon = new VPolygon(eval(arr[j]), lineColor, lineWidth, fillColor, 1);
                    polygon = new google.maps.Polygon({
                        paths: eval(arr[j]),
                        draggable: false,
                        editable: false,
                        strokeColor: lineColor,
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillColor: fillColor,
                        fillOpacity: 0.35
                    });
                }
                //myMap.addOverlay(polygon);
                polygon.setMap(myMap);
                pWS.push(polygon);
            }
        }
        return pWS;
    }


    function c() {
        for (var i = 0; i < this.Polygons.length; i++) {
            //this.Polygons[i].show();
            this.Polygons[i].setMap(this.myMap);
        }

        this.IsMarkerShow = true;
        //this.myMap.panDirection(0, 0);
    }

    function d() {
  
        for (var i = 0; i < this.Polygons.length; i++) {
            //this.Polygons[i].hide();
            this.Polygons[i].setMap(null);
        }
        this.IsMarkerShow = false;
    }
}

function CDistributorNetworkPolygon(Polygon, Type, DistributorCD, AreaCD, Color, isRemove) {
    this.Polygon = Polygon;
    this.Type = Type;
    this.DistributorCD = DistributorCD;
    this.AreaCD = AreaCD;
    this.Color = Color;
    this.IsRemove = (isRemove != undefined) ? isRemove : false;
}

function CPOIS(myMap, StrObject, UrlImg) {
    this.myMap = myMap;
    this.VMarkers = b(myMap, StrObject, UrlImg);
    this.UrlImg = UrlImg;
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function b(m, v, UrlImg) {
        var object = $.parseJSON(v);
        var markers = new Array();
        for (var i = 0; i < object.length; i++) {
            var icon = UrlImg;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);
            //myMap.addOverlay(mar);
            var marker = CreateMarker(pt, object[i], icon, m);
            markers.push(marker);
        }
        return markers;
    }

    function CreateMarker(pt, Object, icon, m)
    {
        var mar = new google.maps.Marker({
            position: pt,
            icon: icon
        });

        mar.setMap(m);
        var obj = Object;
        mar.addListener('click', function () {
            obj = Object;
            var str = '';
            str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            <h5> {0} </h5>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
            str += '                           <img src="/icon/address.png" />  {1}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';




            var POIS_NAME = obj.NAME;
            var POIS_ADDRESS = obj.ADDRESS;
            var POIS_PHONE = obj.PHONE;
            var POIS_DESCRIPTION = obj.DESCRIPTION;


            if (POIS_PHONE != "") {
                str += '                    <tr>';
                str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
                str += '                           <img src="/icon/phone.png" />  {2}';
                str += '                        </td>';
                str += '                    </tr>';
            }

            if (POIS_DESCRIPTION != "") {
                str += '                    <tr>';
                str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
                str += '                         <b> Ghi chú: </b> {3}';
                str += '                        </td>';
                str += '                    </tr>';
            }

            str += '                </table>';


            str = str.replace("{0}", POIS_NAME);
            str = str.replace("{1}", POIS_ADDRESS);
            str = str.replace("{2}", POIS_PHONE);
            str = str.replace("{3}", POIS_DESCRIPTION);

            // obj.openInfoWindow(str);
            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);

        });
    }


    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].marker.setMap(this.myMap);
        }

        this.IsMarkerShow = true;

    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].marker.setMap(null);
        }
        this.IsMarkerShow = false;
    }
}

function ClassMarkerCoveraged(myMap, pathImage, StrObject) {

    this.myMap = myMap;
    this.pathImage = pathImage;
    this.VMarkers = b(StrObject, this.pathImage, this.myMap);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].marker.setMap(this.myMap);
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {

            this.VMarkers[i].marker.setMap(null);
        }
        this.IsMarkerShow = false;
    }

    function b(v, p, m) {

        var markers = new Array();
        var object = $.parseJSON(v);
        for (var i = 0; i < object.length; i++) {
            var icon = p;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);
            var marker = CreateMarker(pt, object[i], icon, m);
            var MarkerObj = new Object();
            MarkerObj.marker = marker;
            MarkerObj.obj = object[i];
            markers.push(MarkerObj);
            BOUNDS.extend(pt);
        }
        return markers;
    }

    function CreateMarker(pt, Object, icon, m)
    {
        var mar = new google.maps.Marker({
            position: pt,
            icon: icon
        });
        mar.setMap(m);

        //m.addOverlay(mar);
        var obj = Object;
        mar.addListener('click', function () {
            markerTmp = Object;
            obj = Object;
            var str = '';
            str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            <h3> {0} </h3>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
            str += '                           <img src="/icon/address.png" />  {1}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                   ';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                            Population :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {2}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 190px; vertical-align: top; padding-left:15px\">';
            str += '                            Name of nearest distributor :';
            str += '                        </td>';
            str += '                        <td colspan=\"3\" style=\"white-space: nowrap; width: 154px; vertical-align: top\">';
            str += '                           {3}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 190px; vertical-align: top; padding-left:15px\">';
            str += '                            Road distance from:';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 1%; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            - Nearest town covered :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                             <span id="NearestTownCoveredName">{23} </span>';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 12px;\">';
            str += '                             <span id="NearestTownCoveredDistance">{24} </span>';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 1%; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            - Nearest ward covered :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                             <span id="NearestWardCoveredName">{25} </span>';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 12px;\">';
            str += '                            <span id="NearestWardCoveredDistance">{26} </span> ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 1%; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            - Nearest commune covered :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                           <span id="NearestCommuneCoveredName">{21} </span> ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 12px;\">';
            str += '                          <span id="NearestCommuneCoveredDistance">{22} </span> ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            - Distributor :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                           ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 12px;\">';
            str += '                            <span id="RoadDistanceDistributor"> {20}</span>  ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                     <tr>';
            str += '                         <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap\">';
            str += '                             Number of Customers :</td>';
            str += '                         <td style=\"vertical-align: top; width: 154px; white-space: nowrap\">';
            str += '                             {9} </td>';
            str += '                         <td style=\"vertical-align: top; width: 50px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                         <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                     </tr>';
            str += '                     <tr>';
            str += '                         <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap\">';
            str += '                             Wholesales:</td>';
            str += '                         <td style=\"vertical-align: top; width: 154px; white-space: nowrap\">';
            str += '                             {10} </td>';
            str += '                         <td style=\"vertical-align: top; width: 50px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                         <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                     </tr>';
            str += '                     <tr>';
            str += '                         <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap\">';
            str += '                             AMS in VNĐ:</td>';
            str += '                         <td style=\"vertical-align: top; width: 154px; white-space: nowrap\">';
            str += '                            {11}</td>';
            str += '                         <td style=\"vertical-align: top; width: 50px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                         <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                     </tr>';
            str += '     <tr>';
            str += '                        <td colspan=\"4\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
            str += '                            <hr />';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap;';
            str += '                            color: Blue\">';
            str += '                            <a id=\"StarHere\" onclick=\"StartHere()\">';
            str += '                                <img id=\"StartImg\" alt=\"\" {100} />';
            str += '                                Start here </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
            str += '                            <a id=\"SelectPoint\" onclick=\"SelectPoint()\">';
            str += '                                <img id=\"ImgSelect\" alt=\"\" {101} />';
            str += '                                Select Point </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
            str += '                            <a id=\"EndHere\" onclick=\"EndHere()\">';
            str += '                                <img id=\"EndImg\" alt=\"\" {102} >';
            str += '                                End here </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                </table>';




            var arrowOption = 'src=\"icon/arrowOption.png\"';
            var arrowDownOption = 'src=\"icon/arrowOptionDown.png\"';

            //Start Point
            if (markerStart == undefined || markerStart.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{100}", arrowOption);
            } else
                if (markerStart.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{100}", arrowDownOption);
                }


            //End Point
            if (markerEnd == undefined || markerEnd.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{102}", arrowOption);
            } else
                if (markerEnd.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{102}", arrowDownOption);
                }

            //Select Point

            var selectPoint = false;
            for (var i = 0; i < markerPoints.length; i++) {
                if (markerPoints[i].getPoint() == markerTmp.getPoint()) {
                    selectPoint = true;
                    break;
                }
            }

            if (selectPoint)
                str = str.replace("{101}", arrowDownOption);
            else
                str = str.replace("{101}", arrowOption);



            var NAME = obj.NAME;
            var FULL_ADDRESS = obj.FULL_ADDRESS;
            var POPULATION = obj.POPULATION;

            var DISTRIBUTOR_NAME = obj.DISTRIBUTOR_NAME;
            var DISTRIBUTOR_DISTANCE = obj.DISTRIBUTOR_DISTANCE;

            var TOTAL_CUSTOMER = obj.TOTAL_CUSTOMER;
            var AMS = obj.AMS;
            var WHOLE_SALES = obj.WHOLE_SALES;



            str = str.replace("{0}", NAME);
            str = str.replace("{1}", FULL_ADDRESS);
            str = str.replace("{2}", POPULATION);
            str = str.replace("{3}", DISTRIBUTOR_NAME);
            str = str.replace("{9}", TOTAL_CUSTOMER);
            str = str.replace("{10}", WHOLE_SALES);
            str = str.replace("{11}", AMS);




            if (DISTRIBUTOR_DISTANCE == 0) // get khoảng cách nếu là lần đầu tiên lick vào marker
            {
                str = str.replace("{20}", "");
                GetRoadDistanceFromDistributor(obj, obj.LONGITUDE_LATITUDE, obj.LONGITUDE_LATITUDE_DISTRIBUTOR);

            }
            else {
                // set distance 
                //document.getElementById("RoadDistanceDistributor").innerText = DISTRIBUTOR_DISTANCE + " Km";
                str = str.replace("{20}", DISTRIBUTOR_DISTANCE + " Km");
            }
            str = str.replace("{21}", obj.COMMUNE_COVERED_NAME);
            str = str.replace("{22}", obj.COMMUNE_COVERED_DISTANCE + " km");
            str = str.replace("{23}", obj.TOWN_COVERED_NAME);
            str = str.replace("{24}", obj.TOWN_COVERED_DISTANCE + " km");
            str = str.replace("{25}", obj.WARD_COVERED_NAME);
            str = str.replace("{26}", obj.WARD_COVERED_DISTANCE + " km");
            // set distance commune nearest
            //document.getElementById("NearestCommuneCoveredName").innerText = obj.COMMUNE_COVERED_NAME;
            //document.getElementById("NearestCommuneCoveredDistance").innerText = obj.COMMUNE_COVERED_DISTANCE + " km";

            // set distance town nearest
            //document.getElementById("NearestTownCoveredName").innerText = obj.TOWN_COVERED_NAME;
            //document.getElementById("NearestTownCoveredDistance").innerText = obj.TOWN_COVERED_DISTANCE + " km";


            // set distance ward nearest
            //document.getElementById("NearestWardCoveredName").innerText = obj.WARD_COVERED_NAME;
            //document.getElementById("NearestWardCoveredDistance").innerText = obj.WARD_COVERED_DISTANCE + " km";

            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);
        });
        return mar;
    }

}

function ClassMarkerLongTude(myMap, pathImage, StrObject) {

    this.myMap = myMap;
    this.pathImage = pathImage;
    this.VMarkers = b(StrObject, this.pathImage, this.myMap);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].show();
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {

            this.VMarkers[i].hide();
        }
        this.IsMarkerShow = false;
    }

    function b(v, p, m) {

        var markers = new Array();
        var object = $.parseJSON(v);
        alert(object.length + "8");
        for (var i = 0; i < object.length; i++) {
            var icon = p;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);


            var mar = new google.maps.Marker({
                position: pt,
                icon: icon
            });
            mar.setMap(m);

            //m.addOverlay(mar);
            var obj = object[i];
            mar.addListener('click', function () {
                markerMoveTmp = obj;
                markerViewDistance = obj;

                var str = '';
                str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
                str += '                    <tr>';
                str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
                str += '                            <h3> {0} </h3>';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    {Info}';
                str += '                    {DistributorName}';

                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top;  padding-left:15px\">';
                str += '                            Population covered :';
                str += '                        </td>';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; \">';
                str += '                             {2}';
                str += '                       </td>';


                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; \">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px\">';
                str += '                            Area covered</td>';
                str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top\">';
                str += '                            {4} Km2</td>';
                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
                str += '                            Number of Customers :</td>';
                str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
                str += '                            {5} </td>';
                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
                str += '                             Wholesales</td>';
                str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
                str += '                            {6}</td>';
                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
                str += '                            DSP:</td>';
                str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
                str += '                            {7}</td>';
                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                     <tr>';
                str += '                         <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap\">';
                str += '                             AMS in VNĐ:</td>';
                str += '                         <td style=\"vertical-align: top; width: 154px; white-space: nowrap\">';
                str += '                             {8}</td>';
                str += '                         <td style=\"vertical-align: top; width: 34px; white-space: nowrap\">';
                str += '                         </td>';
                str += '                         <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
                str += '                         </td>';
                str += '                     </tr>';
                str += '     <tr>';
                str += '                        <td colspan=\"4\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
                str += '                            <hr />';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap;';
                str += '                            color: Blue\">';
                str += '                            <a id=\"SelectMove\" onclick=\"fnsDistributorNetworkSelectPoints()\">';
                str += '                                <img id=\"SelectMoveImg\" alt=\"\" {100} />';
                str += '                                 <span id=\"TitleSelect\" > {Select} </span>   </a> ';
                str += '                        </td> ';
                str += '                        {ViewDistance} ';


                //                    str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
                //                    str += '                            <a id=\"EndHere\" onclick=\"EndHere()\">';
                //                    str += '                                <img id=\"EndImg\" alt=\"\" {102} >';
                //                    str += '                                End here </a>';
                //                    str += '                        </td>';                
                //                    str += '                        <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
                //                    str += '                        </td>';
                //                    str += '                    </tr>';                
                str += '                </table>';




                var arrowOption = 'src=\"icon/arrowOption.png\"';
                var arrowDownOption = 'src=\"icon/arrowOptionDown.png\"';

                if (obj.getIcon().image == "/icon/longTude.png") {
                    str = str.replace("{100}", arrowOption);
                    str = str.replace("{Select}", "Select");
                }
                else {
                    str = str.replace("{100}", arrowDownOption);
                    str = str.replace("{Select}", "Deselect");
                }

                var NAME = obj.Name;
                var POPULATION = obj.Population;
                var AREA_COVERED = obj.Area;
                var CUSTOMERS = obj.Customer;
                var WHOLE_SALES = obj.Wholesales;
                var AMS = obj.Ams;
                var Dsp = obj.Dsp;

                if (obj.Info == "")
                    str = str.replace("{Info}", "");
                else {
                    var strTemp = "";
                    strTemp += '                    <tr>';
                    strTemp += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
                    strTemp += '                           <img src="/icon/address.png" />  {1}';
                    strTemp += '                        </td>';
                    strTemp += '                    </tr>';

                    strTemp = strTemp.replace("{1}", obj.Info);
                    str = str.replace("{Info}", strTemp);

                }


                if (obj.DistributorName == "")
                    str = str.replace("{DistributorName}", "");
                else {
                    var strTemp = "";
                    strTemp += '                    <tr>';
                    strTemp += '                        <td colspan=\"4\"  style=\"white-space: nowrap;width: 375px;vertical-align: top;\">';
                    strTemp += '                          &nbsp;&nbsp;&nbsp;&nbsp;  Distributor: <br/>';
                    strTemp += '                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{1}';
                    strTemp += '                        </td>';
                    strTemp += '                    </tr>';

                    strTemp = strTemp.replace("{1}", obj.DistributorName.replace(",", "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
                    str = str.replace("{DistributorName}", strTemp);

                }


                if ((pointCDs.length == 1 && centralPoints.length == 0) || (pointCDs.length == 0 && centralPoints.length == 1)) {
                    markerStart = pointCDs[0];
                    var strTemp = "";
                    strTemp += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
                    strTemp += '                            <a id=\"ViewDistance\" onclick=\"fnsViewDistance()\">';
                    strTemp += '                                View distance </a>   <span id="ResultViewDistance"> </span> ';
                    strTemp += '                        </td>';

                    str = str.replace("{ViewDistance}", strTemp);
                }
                else {
                    str = str.replace("{ViewDistance}", "");
                }

                str = str.replace("{0}", NAME);
                str = str.replace("{2}", POPULATION);
                str = str.replace("{4}", AREA_COVERED);
                str = str.replace("{5}", CUSTOMERS);
                str = str.replace("{6}", WHOLE_SALES);
                str = str.replace("{7}", Dsp);
                str = str.replace("{8}", AMS);

                var infowindow = new google.maps.InfoWindow({
                    content: str
                });
                for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                    CTracking.prototype.mInfowindow[i].close();
                }
                this.mInfowindow = null;
                infowindow.open(myMap, mar);
                CTracking.prototype.mInfowindow.push(infowindow);


                //                            
                //                    
                //                         if(distributorDirect.VMarkers.length  <= 1 )
                //                            return;
                //                                                     
                //                                                           
                //                         if(obj.getIcon().image == "/icon/longTude.png")
                //                         {
                //                            var icon = new VIcon();
                //                            pointCDs.push(obj);
                //                            icon.image = "/icon/LongTudeSelect.png";
                //                            obj.setIcon(icon);
                //                            LongTudeSelect();
                //                            
                //                         }
                //                         else
                //                         {                            
                //                            var icon = new VIcon();
                //                            icon.image = "/icon/longTude.png";
                //                            obj.setIcon(icon);   
                //                            for(var j = 0; j < pointCDs.length; j++){
                //                                if(pointCDs[j].CD == obj.CD)
                //                                { 
                //                                    pointCDs.splice(j,1);
                //                                    break;
                //                                }                                
                //                            }
                //                            LongTudeUnSelect(obj);
                //                            
                //                          }  
                //                          


            });

            markers.push(mar);

        }
        return markers;
    };



}

function ClassMarkerCentralPoint(myMap, pathImage, StrObject) {

    this.myMap = myMap;
    this.pathImage = pathImage;
    this.VMarkers = b(StrObject, this.pathImage, this.myMap);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].show();
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {

            this.VMarkers[i].hide();
        }
        this.IsMarkerShow = false;
    }

    function b(v, p, m) {

        var markers = new Array();
        var object = $.parseJSON(v);
        alert(object.length + "7");
        for (var i = 0; i < object.length; i++) {
            var icon = p;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);


            var mar = new google.maps.Marker({
                position: pt,
                icon: icon
            });
            mar.setMap(m);

            //m.addOverlay(mar);
            var obj = object[i];
            mar.addListener('click', function () {
                markerMoveTmp = obj;
                markerViewDistance = obj;

                var str = '';
                str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
                str += '                    <tr>';
                str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
                str += '                            <h3> {0} </h3>';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    {Info}';
                str += '                    {DistributorName}';

                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top;  padding-left:15px\">';
                str += '                            Population covered :';
                str += '                        </td>';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; \">';
                str += '                             {2}';
                str += '                       </td>';


                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; \">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px\">';
                str += '                            Area covered</td>';
                str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top\">';
                str += '                            {4} Km2</td>';
                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
                str += '                            Number of Customers :</td>';
                str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
                str += '                            {5} </td>';
                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
                str += '                             Wholesales</td>';
                str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
                str += '                            {6}</td>';
                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
                str += '                            DSP:</td>';
                str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
                str += '                            {7}</td>';
                str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                     <tr>';
                str += '                         <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap\">';
                str += '                             AMS in VNĐ:</td>';
                str += '                         <td style=\"vertical-align: top; width: 154px; white-space: nowrap\">';
                str += '                             {8}</td>';
                str += '                         <td style=\"vertical-align: top; width: 34px; white-space: nowrap\">';
                str += '                         </td>';
                str += '                         <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
                str += '                         </td>';
                str += '                     </tr>';
                str += '     <tr>';
                str += '                        <td colspan=\"4\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
                str += '                            <hr />';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap;';
                str += '                            color: Blue\">';
                str += '                            <a id=\"SelectMove\" onclick=\"fnsCentralPointSelect()\">';
                str += '                                <img id=\"SelectMoveImg\" alt=\"\" {100} />';
                str += '                                 <span id=\"TitleSelect\" > {Select} </span>   </a> ';
                str += '                        </td> ';
                str += '                        {ViewDistance} ';

                //                    str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
                //                    str += '                            <a id=\"EndHere\" onclick=\"EndHere()\">';
                //                    str += '                                <img id=\"EndImg\" alt=\"\" {102} >';
                //                    str += '                                End here </a>';
                //                    str += '                        </td>';                
                //                    str += '                        <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
                //                    str += '                        </td>';
                //                    str += '                    </tr>';                
                str += '                </table>';




                var arrowOption = 'src=\"icon/arrowOption.png\"';
                var arrowDownOption = 'src=\"icon/arrowOptionDown.png\"';

                if (obj.getIcon().image == "/icon/centralPoint.png") {
                    str = str.replace("{100}", arrowOption);
                    str = str.replace("{Select}", "Select");
                }
                else {
                    str = str.replace("{100}", arrowDownOption);
                    str = str.replace("{Select}", "Deselect");
                }

                var NAME = obj.Name;
                var POPULATION = obj.Population;
                var AREA_COVERED = obj.Area;
                var CUSTOMERS = obj.Customer;
                var WHOLE_SALES = obj.Wholesales;
                var AMS = obj.Ams;
                var Dsp = obj.Dsp;

                if (obj.Info == "")
                    str = str.replace("{Info}", "");
                else {
                    var strTemp = "";
                    strTemp += '                    <tr>';
                    strTemp += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
                    strTemp += '                           <img src="/icon/address.png" />  {1}';
                    strTemp += '                        </td>';
                    strTemp += '                    </tr>';

                    strTemp = strTemp.replace("{1}", obj.Info);
                    str = str.replace("{Info}", strTemp);

                }


                if (obj.DistributorName == "")
                    str = str.replace("{DistributorName}", "");
                else {
                    var strTemp = "";
                    strTemp += '                    <tr>';
                    strTemp += '                        <td colspan=\"4\"  style=\"white-space: nowrap;width: 375px;vertical-align: top;\">';
                    strTemp += '                          &nbsp;&nbsp;&nbsp;&nbsp;  Distributor: <br/>';
                    strTemp += '                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{1}';
                    strTemp += '                        </td>';
                    strTemp += '                    </tr>';

                    strTemp = strTemp.replace("{1}", obj.DistributorName.replace(",", "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
                    str = str.replace("{DistributorName}", strTemp);

                }


                if ((pointCDs.length == 1 && centralPoints.length == 0) || (pointCDs.length == 0 && centralPoints.length == 1)) {
                    markerStart = pointCDs[0];
                    var strTemp = "";
                    strTemp += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
                    strTemp += '                            <a id=\"ViewDistance\" onclick=\"fnsViewDistance()\">';
                    strTemp += '                                View distance </a>   <span id="ResultViewDistance"> </span> ';
                    strTemp += '                        </td>';

                    str = str.replace("{ViewDistance}", strTemp);
                }
                else {
                    str = str.replace("{ViewDistance}", "");
                }

                str = str.replace("{0}", NAME);
                str = str.replace("{2}", POPULATION);
                str = str.replace("{4}", AREA_COVERED);
                str = str.replace("{5}", CUSTOMERS);
                str = str.replace("{6}", WHOLE_SALES);
                str = str.replace("{7}", Dsp);
                str = str.replace("{8}", AMS);

                var infowindow = new google.maps.InfoWindow({
                    content: str
                });
                for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                    CTracking.prototype.mInfowindow[i].close();
                }
                this.mInfowindow = null;
                infowindow.open(myMap, mar);
                CTracking.prototype.mInfowindow.push(infowindow);

                //                            
                //                    
                //                         if(distributorDirect.VMarkers.length  <= 1 )
                //                            return;
                //                                                     
                //                                                           
                //                         if(obj.getIcon().image == "/icon/longTude.png")
                //                         {
                //                            var icon = new VIcon();
                //                            pointCDs.push(obj);
                //                            icon.image = "/icon/LongTudeSelect.png";
                //                            obj.setIcon(icon);
                //                            LongTudeSelect();
                //                            
                //                         }
                //                         else
                //                         {                            
                //                            var icon = new VIcon();
                //                            icon.image = "/icon/longTude.png";
                //                            obj.setIcon(icon);   
                //                            for(var j = 0; j < pointCDs.length; j++){
                //                                if(pointCDs[j].CD == obj.CD)
                //                                { 
                //                                    pointCDs.splice(j,1);
                //                                    break;
                //                                }                                
                //                            }
                //                            LongTudeUnSelect(obj);
                //                            
                //                          }  
                //                          


            });

            markers.push(mar);

        }
        return markers;
    };



}

function fnsViewDistance() {
    return;
    var dirTemp = new VDirections();
    var diroptions = new VDirectionsOptions(true, false, false);
    var waypoints = new Array();
    //Start Point            


    waypoints.push(markerStartViewDistance.getPoint());
    waypoints.push(markerViewDistance.getPoint());

    dirTemp.loadFromWayPoints(waypoints, diroptions);
    VEvent.addListener(dirTemp, 'loaded', function () {

        var v;
        if (dirTemp.Unit == "km")
            v = eval(dirTemp.Distance);
        else
            v = eval(dirTemp.Distance / 1000);
        document.getElementById("ResultViewDistance").innerText = "==> " + v + " Km";

    });


}

function fnsCentralPointSelect() {

    if (document.getElementById("SelectMoveImg").getAttribute("src").indexOf(PATH_ARROW_IMG) != -1) {
        document.getElementById("SelectMoveImg").setAttribute("src", PATH_ARROW_DOWN_IMG);
        var icon;
        centralPoints.push(markerMoveTmp);
        icon = "/icon/centralPointSelect.png";
        markerMoveTmp.setIcon(icon);
        document.getElementById("TitleSelect").innerHTML = "Deselect";
        if (pointCDs.length == 0 && centralPoints.length == 1) {
            markerStartViewDistance = markerMoveTmp;
        }
    }
    else {
        document.getElementById("SelectMoveImg").setAttribute("src", PATH_ARROW_IMG);
        var icon = "/icon/centralPoint.png";
        markerMoveTmp.setIcon(icon);
        document.getElementById("TitleSelect").innerHTML = "Select";
        if (pointCDs.length == 0 && centralPoints.length == 1) {
            markerStartViewDistance = centralPoints[0];
        }
    }


}

function fnsDistributorDirectSelect() {

    if (document.getElementById("SelectMoveImg").getAttribute("src").indexOf(PATH_ARROW_IMG) != -1) {
        document.getElementById("SelectMoveImg").setAttribute("src", PATH_ARROW_DOWN_IMG);
        var icon;
        centralPoints.push(markerMoveTmp);
        icon = "/icon/ND_.png";
        markerMoveTmp.setIcon(icon);
        document.getElementById("TitleSelect").innerHTML = "Deselect";
        if (pointCDs.length == 0 && centralPoints.length == 1) {
            markerStartViewDistance = markerMoveTmp;
        }
    }
    else {
        document.getElementById("SelectMoveImg").setAttribute("src", PATH_ARROW_IMG);
        var icon = "/icon/ND.png";
        markerMoveTmp.setIcon(icon);
        document.getElementById("TitleSelect").innerHTML = "Select";
        if (pointCDs.length == 0 && centralPoints.length == 1) {
            markerStartViewDistance = centralPoints[0];
        }
    }


}

function fnsDistributorInDirectSelect() {

    if (document.getElementById("SelectMoveImg").getAttribute("src").indexOf(PATH_ARROW_IMG) != -1) {
        document.getElementById("SelectMoveImg").setAttribute("src", PATH_ARROW_DOWN_IMG);
        var icon;
        centralPoints.push(markerMoveTmp);
        icon = "/icon/SD_.png";
        markerMoveTmp.setIcon(icon);
        document.getElementById("TitleSelect").innerHTML = "Deselect";
        if (pointCDs.length == 0 && centralPoints.length == 1) {
            markerStartViewDistance = markerMoveTmp;
        }
    }
    else {
        document.getElementById("SelectMoveImg").setAttribute("src", PATH_ARROW_IMG);
        var icon = "/icon/SD.png";
        markerMoveTmp.setIcon(icon);
        document.getElementById("TitleSelect").innerHTML = "Select";
        if (pointCDs.length == 0 && centralPoints.length == 1) {
            markerStartViewDistance = centralPoints[0];
        }
    }


}

function fnsDistributorNetworkSelectPoints() {
    if (distributorDirect.VMarkers.length + distributorInDirect.VMarkers.length <= 1) {
        markerStart = null;
        return;
    }

    if (document.getElementById("SelectMoveImg").getAttribute("src").indexOf(PATH_ARROW_IMG) != -1) {
        document.getElementById("SelectMoveImg").setAttribute("src", PATH_ARROW_DOWN_IMG);
        var icon;
        pointCDs.push(markerMoveTmp);
        icon = "/icon/LongTudeSelect.png";
        markerMoveTmp.setIcon(icon);
        LongTudeSelect();
        document.getElementById("TitleSelect").innerHTML = "Deselect";

        if (pointCDs.length == 1 && centralPoints.length == 0) {
            markerStartViewDistance = markerMoveTmp;
        }
    }
    else {
        document.getElementById("SelectMoveImg").setAttribute("src", PATH_ARROW_IMG);
        var icon;
        icon = "/icon/longTude.png";
        markerMoveTmp.setIcon(icon);
        for (var j = 0; j < pointCDs.length; j++) {
            if (pointCDs[j].CD == markerMoveTmp.CD) {
                pointCDs.splice(j, 1);
                break;
            }
        }
        LongTudeUnSelect(markerMoveTmp);
        document.getElementById("TitleSelect").innerHTML = "Select";

        if (pointCDs.length == 1 && centralPoints.length == 0) {
            markerStartViewDistance = pointCDs[0];
        }
    }


}

function ClassMarkerUnCoveraged(myMap, pathImage, StrObject) {

    this.myMap = myMap;
    this.pathImage = pathImage;
    this.VMarkers = b(StrObject, this.pathImage, this.myMap);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].marker.setMap(this.myMap);
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].marker.setMap(null);
        }
        this.IsMarkerShow = false;
    }

    function b(v, p, m) {
        var markers = new Array();
        var object = $.parseJSON(v);
        for (var i = 0; i < object.length; i++) {
            var icon;
            icon = p;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);
            var marker = CreateMarker(pt, object[i], icon, m);
            var MarkerObj = new Object();
            MarkerObj.marker = marker;
            MarkerObj.obj = object[i];
            markers.push(MarkerObj);
            BOUNDS.extend(pt);
        }
        return markers;
    };

    function CreateMarker(pt, Object, icon, m)
    {

        var mar = new google.maps.Marker({
            position: pt,
            icon: icon
        });
        mar.setMap(m);

        //m.addOverlay(mar);

        var obj = Object;
        mar.addListener('click', function () {
            obj = Object
            markerTmp = Object;


            var str = '';
            str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            <h3> {0} </h3>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
            str += '                            <img src="/icon/address.png" /> {1}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                   ';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                            Population :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {2}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td  style=\"white-space: nowrap; width: 190px; vertical-align: top; padding-left:15px\">';
            str += '                            Name of nearest distributor :';
            str += '                        </td>';
            str += '                        <td colspan=\"3\" style=\"white-space: nowrap; width: 200px; vertical-align: top;text-align:left\">';
            str += '                           <span id="NameOfNearestDistributor">{4}</span>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px\">';
            str += '                            Road distance from:';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 200px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 1%; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            - Nearest town covered :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 200px; vertical-align: top; height: 12px;\">';
            str += '                             <span id="NearestTownCoveredName"> </span>';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 12px;\">';
            str += '                             <span id="NearestTownCoveredDistance"> {9}</span>';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 1%; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            - Nearest ward covered :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 200px; vertical-align: top; height: 12px;\">';
            str += '                             <span id="NearestWardCoveredName">{10} </span>';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 12px;\">';
            str += '                            <span id="NearestWardCoveredDistance">{11} </span> ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 1%; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            - Nearest commune covered :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 200px; vertical-align: top; height: 12px;\">';
            str += '                           <span id="NearestCommuneCoveredName">{6} </span> ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 12px;\">';
            str += '                          <span id="NearestCommuneCoveredDistance">{7} </span> ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            - Distributor :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 200px; vertical-align: top; height: 12px;\">';
            str += '                           ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 12px;\">';
            str += '                            <span id="RoadDistanceDistributor">{5} </span>  ';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
            str += '                            ';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
            str += '                            <hr />';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap;';
            str += '                            color: Blue\">';
            str += '                            <a id=\"StarHere\" onclick=\"StartHere()\">';
            str += '                                <img id=\"StartImg\" alt=\"\" {100} />';
            str += '                                Start here </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 200px; white-space: nowrap; color: Blue\">';
            str += '                            <a id=\"SelectPoint\" onclick=\"SelectPoint()\">';
            str += '                                <img id=\"ImgSelect\" alt=\"\" {101} />';
            str += '                                Select Point </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
            str += '                            <a id=\"EndHere\" onclick=\"EndHere()\">';
            str += '                                <img id=\"EndImg\" alt=\"\" {102} >';
            str += '                                End here </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
            str += '                           &nbsp; ';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                </table>';




            var arrowOption = 'src=\"icon/arrowOption.png\"';
            var arrowDownOption = 'src=\"icon/arrowOptionDown.png\"';

            //Start Point
            if (markerStart == undefined || markerStart.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{100}", arrowOption);
            } else
                if (markerStart.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{100}", arrowDownOption);
                }


            //End Point
            if (markerEnd == undefined || markerEnd.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{102}", arrowOption);
            } else
                if (markerEnd.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{102}", arrowDownOption);
                }

            //Select Point

            var selectPoint = false;
            for (var i = 0; i < markerPoints.length; i++) {
                if (markerPoints[i].getPoint() == markerTmp.getPoint()) {
                    selectPoint = true;
                    break;
                }
            }

            if (selectPoint)
                str = str.replace("{101}", arrowDownOption);
            else
                str = str.replace("{101}", arrowOption);



            var NAME = obj.NAME;
            var FULL_ADDRESS = obj.FULL_ADDRESS;
            var POPULATION = obj.POPULATION;
            var DISTRIBUTOR_NAME = obj.DISTRIBUTOR_NAME;


            str = str.replace("{0}", NAME);
            str = str.replace("{1}", FULL_ADDRESS);
            str = str.replace("{2}", POPULATION);
            str = str.replace("{3}", DISTRIBUTOR_NAME);

            str = str.replace("{4}", obj.DISTRIBUTOR_NAME);
            str = str.replace("{5}", obj.DISTRIBUTOR_DISTANCE + " Km");
            str = str.replace("{6}", obj.COMMUNE_COVERED_NAME);
            str = str.replace("{7}", obj.COMMUNE_COVERED_DISTANCE + " Km");
            str = str.replace("{8}", obj.TOWN_COVERED_NAME);
            str = str.replace("{9}", obj.TOWN_COVERED_DISTANCE + " Km");
            str = str.replace("{10}", obj.WARD_COVERED_NAME);
            str = str.replace("{11}", obj.WARD_COVERED_DISTANCE + " Km");

            if (obj.DISTRIBUTOR_DISTANCE == 0) // get khoảng cách nếu là lần đầu tiên lick vào marker
            {
                //GetRoadDistanceUncoverToDistributor(obj);
                //GetRoadDistanceFromDistributor(obj, obj.LONGITUDE_LATITUDE, obj.LONGITUDE_LATITUDE_DISTRIBUTOR);
            }



            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);



            // set distance 
            //document.getElementById("NameOfNearestDistributor").innerText = obj.DISTRIBUTOR_NAME;
            //document.getElementById("RoadDistanceDistributor").innerText = obj.DISTRIBUTOR_DISTANCE + " Km";

   

            // set distance commune nearest
            //document.getElementById("NearestCommuneCoveredName").innerText = obj.COMMUNE_COVERED_NAME;
            //document.getElementById("NearestCommuneCoveredDistance").innerText = obj.COMMUNE_COVERED_DISTANCE + " km";

            // set distance town nearest
            //document.getElementById("NearestTownCoveredName").innerText = obj.TOWN_COVERED_NAME;
            //document.getElementById("NearestTownCoveredDistance").innerText = obj.TOWN_COVERED_DISTANCE + " km";


            // set distance ward nearest
            //document.getElementById("NearestWardCoveredName").innerText = obj.WARD_COVERED_NAME;
            //document.getElementById("NearestWardCoveredDistance").innerText = obj.WARD_COVERED_DISTANCE + " km";


        });
        return mar;
    }



}

function ClassMarkerDistributor(myMap, pathImage, StrObject) {

    this.myMap = myMap;
    this.pathImage = pathImage;
    this.VMarkers = b(StrObject, this.pathImage, this.myMap);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].show();
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {

            this.VMarkers[i].hide();
        }
        this.IsMarkerShow = false;
    }

    function b(v, p, m) {
        var markers = new Array();
        var object = $.parseJSON(v);
        for (var i = 0; i < object.length; i++) {
            var icon = object[i].DistributorTypeImage;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);

            var marker = CreateMarker(pt, object[i], icon, m);
            var MarkerObj = new Object();
            MarkerObj.marker = marker;
            MarkerObj.obj = object[i];
            markers.push(MarkerObj);
        }

        return markers;
    }

    function CreateMarker(pt, Object, icon, m)
    {
        var mar = new google.maps.Marker({
            position: pt,
            icon: icon
        });
        mar.setMap(m);
        var obj = Object;
        mar.addListener('click', function () {
            markerTmp = Object;
            obj = Object;

            var str = '';
            str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            <h3> {0} </h3>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
            str += '                          <img src="/icon/address.png" />  {1}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                            Population covered :';
            str += '                        </td>';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {2}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            Radius :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                            {3} Km</td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px\">';
            str += '                            Area covered</td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top\">';
            str += '                            {4} Km2</td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            Number of Customers :</td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                            {5} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                             Wholesales</td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                            {6}</td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            DSP:</td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                            {7}</td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                     <tr>';
            str += '                         <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap\">';
            str += '                             AMS in VNĐ:</td>';
            str += '                         <td style=\"vertical-align: top; width: 154px; white-space: nowrap\">';
            str += '                             {8}</td>';
            str += '                         <td style=\"vertical-align: top; width: 34px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                         <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                     </tr>';
            str += '     <tr>';
            str += '                        <td colspan=\"4\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
            str += '                            <hr />';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap;';
            str += '                            color: Blue\">';
            str += '                            <a id=\"StarHere\" onclick=\"StartHere()\">';
            str += '                                <img id=\"StartImg\" alt=\"\" {100} />';
            str += '                                Start here </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
            str += '                            <a id=\"SelectPoint\" onclick=\"SelectPoint()\">';
            str += '                                <img id=\"ImgSelect\" alt=\"\" {101} />';
            str += '                                Select Point </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
            str += '                            <a id=\"EndHere\" onclick=\"EndHere()\">';
            str += '                                <img id=\"EndImg\" alt=\"\" {102} >';
            str += '                                End here </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                </table>';




            var arrowOption = 'src=\"icon/arrowOption.png\"';
            var arrowDownOption = 'src=\"icon/arrowOptionDown.png\"';

            //Start Point
            if (markerStart == undefined || markerStart.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{100}", arrowOption);
            } else
                if (markerStart.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{100}", arrowDownOption);
                }


            //End Point
            if (markerEnd == undefined || markerEnd.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{102}", arrowOption);
            } else
                if (markerEnd.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{102}", arrowDownOption);
                }

            //Select Point

            var selectPoint = false;
            for (var i = 0; i < markerPoints.length; i++) {
                if (markerPoints[i].getPoint() == markerTmp.getPoint()) {
                    selectPoint = true;
                    break;
                }
            }

            if (selectPoint)
                str = str.replace("{101}", arrowDownOption);
            else
                str = str.replace("{101}", arrowOption);




            var NAME = obj.NAME;
            var FULL_ADDRESS = obj.FULL_ADDRESS;
            var POPULATION = obj.POPULATION;
            var RADIUS = obj.RADIUS;
            var AREA_COVERED = obj.AREA_COVERED;
            var CUSTOMERS = obj.CUSTOMERS;
            var WHOLE_SALES = obj.WHOLE_SALES;
            var DSP = obj.DSP;
            var AMS = obj.AMS;


            str = str.replace("{0}", NAME);
            str = str.replace("{1}", FULL_ADDRESS);
            str = str.replace("{2}", POPULATION);
            str = str.replace("{3}", RADIUS);
            str = str.replace("{4}", AREA_COVERED);
            str = str.replace("{5}", CUSTOMERS);
            str = str.replace("{6}", WHOLE_SALES);
            str = str.replace("{7}", DSP);
            str = str.replace("{8}", AMS);


            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);



        });
    }

}

function ClassMarkerDistributorInDirect(myMap, pathImage, StrObject) {

    this.myMap = myMap;
    this.pathImage = pathImage;
    this.VMarkers = b(StrObject, this.pathImage, this.myMap);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].show();
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {

            this.VMarkers[i].hide();
        }
        this.IsMarkerShow = false;
    }

    function b(v, p, m) {
        var markers = new Array();
        var object = $.parseJSON(v);
        for (var i = 0; i < object.length; i++) {
            var icon = object[i].DistributorTypeImage;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);
            var marker = CreateMarker(pt, object[i], icon, m);
            markers.push(marker);
        }
   
        return markers;
    }

    function CreateMarker(pt, Object, icon, m)
    {

        var mar = new google.maps.Marker({
            position: pt,
            icon: icon
        });
        mar.setMap(m);
        var obj = Object;

        mar.addListener('click', function () {
            markerTmp = Object;
            obj = Object;
            var str = '';
            str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            <h3> {0} </h3>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
            str += '                          <img src="/icon/address.png" />  {1}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Under distributor: ';
            str += '                        </td>';
            str += '                         <td colspan=\"3\"style=\"white-space: nowrap; vertical-align: top; height: 19px;\">';
            str += '                             {150}';
            str += '                        </td>';

            str += '                    </tr>';


            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                            Population covered :';
            str += '                        </td>';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {2}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            Radius :';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                            {3} Km</td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px\">';
            str += '                            Area covered</td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top\">';
            str += '                            {4} Km2</td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            Number of Customers :</td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                            {5} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                             Wholesales</td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                            {6}</td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                            DSP:</td>';
            str += '                        <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 12px;\">';
            str += '                            {7}</td>';
            str += '                        <td style=\"white-space: nowrap; width: 34px; vertical-align: top; height: 12px;\">';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                     <tr>';
            str += '                         <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap\">';
            str += '                             AMS in VNĐ:</td>';
            str += '                         <td style=\"vertical-align: top; width: 154px; white-space: nowrap\">';
            str += '                             {8}</td>';
            str += '                         <td style=\"vertical-align: top; width: 34px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                         <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                         </td>';
            str += '                     </tr>';
            str += '     <tr>';
            str += '                        <td colspan=\"4\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
            str += '                            <hr />';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap;';
            str += '                            color: Blue\">';
            str += '                            <a id=\"StarHere\" onclick=\"StartHere()\">';
            str += '                                <img id=\"StartImg\" alt=\"\" {100} />';
            str += '                                Start here </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
            str += '                            <a id=\"SelectPoint\" onclick=\"SelectPoint()\">';
            str += '                                <img id=\"ImgSelect\" alt=\"\" {101} />';
            str += '                                Select Point </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
            str += '                            <a id=\"EndHere\" onclick=\"EndHere()\">';
            str += '                                <img id=\"EndImg\" alt=\"\" {102} >';
            str += '                                End here </a>';
            str += '                        </td>';
            str += '                        <td style=\"vertical-align: top; width: 1px; white-space: nowrap\">';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                </table>';




            var arrowOption = 'src=\"icon/arrowOption.png\"';
            var arrowDownOption = 'src=\"icon/arrowOptionDown.png\"';

            //Start Point
            if (markerStart == undefined || markerStart.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{100}", arrowOption);
            } else
                if (markerStart.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{100}", arrowDownOption);
                }


            //End Point
            if (markerEnd == undefined || markerEnd.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{102}", arrowOption);
            } else
                if (markerEnd.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{102}", arrowDownOption);
                }

            //Select Point

            var selectPoint = false;
            for (var i = 0; i < markerPoints.length; i++) {
                if (markerPoints[i].getPoint() == markerTmp.getPoint()) {
                    selectPoint = true;
                    break;
                }
            }

            if (selectPoint)
                str = str.replace("{101}", arrowDownOption);
            else
                str = str.replace("{101}", arrowOption);




            var NAME = obj.NAME;
            var FULL_ADDRESS = obj.FULL_ADDRESS;
            var POPULATION = obj.POPULATION;
            var RADIUS = obj.RADIUS;
            var AREA_COVERED = obj.AREA_COVERED;
            var CUSTOMERS = obj.CUSTOMERS;
            var WHOLE_SALES = obj.WHOLE_SALES;
            var DSP = obj.DSP;
            var AMS = obj.AMS;
            var parent = obj.ParentName;

            str = str.replace("{0}", NAME);
            str = str.replace("{1}", FULL_ADDRESS);
            str = str.replace("{2}", POPULATION);
            str = str.replace("{3}", RADIUS);
            str = str.replace("{4}", AREA_COVERED);
            str = str.replace("{5}", CUSTOMERS);
            str = str.replace("{6}", WHOLE_SALES);
            str = str.replace("{7}", DSP);
            str = str.replace("{8}", AMS);
            str = str.replace("{150}", parent);

            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);



        });
        return mar;
    }



}

function ClassMarkerDistributorNetworkDirect(myMap, pathImage, StrObject) {

    this.myMap = myMap;
    this.pathImage = pathImage;
    this.VMarkers = b(StrObject, this.pathImage, this.myMap);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].show();
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {

            this.VMarkers[i].hide();
        }
        this.IsMarkerShow = false;
    }

    function b(v, p, m) {
        var markers = new Array();
        var object = $.parseJSON(v);
        for (var i = 0; i < object.length; i++) {
            var icon = p;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);
            var marker = CreateMarker(pt, object[i], icon, m);
            var MarkerObj = new Object();
            MarkerObj.marker = marker;
            MarkerObj.obj = object[i];
            markers.push(MarkerObj);

        }


        return markers;
    }

    function CreateMarker(pt, Object, icon, m) {
        var mar = new google.maps.Marker({
            position: pt,
            icon: icon
        });
        mar.setMap(map);

        //m.addOverlay(mar);
        var obj = Object;
        mar.addListener('click', function () {
            markerTmp = obj;
            markerViewDistance = obj;
            markerMoveTmp = obj;
            obj = Object;


            var str = '';
            str += '';
            str += '                 <table style=\"font-family:Tahoma; font-size:11px;width: 401px; text-align:left\" ';
            str += '                     cellpadding=\"0\" cellspacing=\"2\" border=\"0\">';
            str += '                                          <tr>';
            str += '                                              <td colspan=\"4\" style=\"white-space: nowrap; width: 375px vertical-align: top\">';
            str += '                                                  <h3>  {0}  </h3>';
            str += '                                              </td>';
            str += '                                          </tr>';
            str += '                                          <tr>';
            str += '                                              <td colspan=\"4\" style=\"white-space: nowrap; width: 375px vertical-align: top\">';
            str += '                                                <img src=\"/icon/address.png\" />  {1}';
            str += '                                              </td>';
            str += '                                          </tr>';
            str += '                                          <tr>';
            str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top height: 19px padding-left:15px\" ';
            str += '                                                  class=\"style1\">';
            str += '                                                  &nbsp;</td>';
            str += '                                               <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 19px; font-weight: 700;\">';
            str += '                                                 <span id="idTitleCurrent"></span>    </td>';
            str += '                                              <td style=\"white-space: nowrap; width: 34px vertical-align: top height: 19px; font-weight: 700;\">';
            str += '                                                <span id="idTitleMoved"></span>   </td>';
            str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 19px\" ';
            str += '                                                  class=\"style2\">';
            str += '                                                  &nbsp;</td>';
            str += '                                          </tr>';
            str += '                                          <tr>';
            str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top height: 19px padding-left:15px\" ';
            str += '                                                  class=\"style1\">';
            str += '                                                  Population covered :';
            str += '                                              </td>';
            str += '                                               <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 19px\">';
            str += '                                                   {2}';
            str += '                                              </td>';
            str += '                                              <td style=\"white-space: nowrap; width: 34px vertical-align: top height: 19px\">';
            str += '                                                   <span id="idPopulationMoved"></span> </td>';
            str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 19px\" ';
            str += '                                                  class=\"style2\">';
            str += '                                                  &nbsp;';
            str += '                                              </td>';
            str += '                                          </tr>';
            str += '                                          <tr>';
            str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px height: 12px\" ';
            str += '                                                  class=\"style1\">';
            str += '                                                  Radius :';
            str += '                                              </td>';
            str += '                                              <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 12px\">';
            str += '                                                  {3} Km</td>';
            str += '                                              <td  style=\"white-space: nowrap; width: 34px vertical-align: top height: 12px\">';
            str += '                                                   <span id="idRadiusMoved"></span> </td>';
            str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 12px\" ';
            str += '                                                  class=\"style2\">';
            str += '                                                  &nbsp;';
            str += '                                              </td>';
            str += '                                          </tr>';
            str += '                                          <tr>';
            str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px\" ';
            str += '                                                  class=\"style1\">';
            str += '                                                  Area covered</td>';
            str += '                                              <td style=\"white-space: nowrap; width: 154px vertical-align: top\">';
            str += '                                                  {4} Km2</td>';
            str += '                                              <td style=\"white-space: nowrap; width: 34px vertical-align: top\">';
            str += '                                                  <span id="idAreadMoved"></span></td>';
            str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top\" class=\"style2\">';
            str += '                                                  &nbsp;';
            str += '                                              </td>';
            str += '                                          </tr>';
            str += '                                          <tr>';
            str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px height: 12px\" ';
            str += '                                                  class=\"style1\">';
            str += '                                                  Number of Customers :</td>';
            str += '                                              <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 12px\">';
            str += '                                                  {5} </td>';
            str += '                                              <td  style=\"white-space: nowrap; width: 34px vertical-align: top height: 12px\">';
            str += '                                                 <span id="idNumberOfCustomerMoved"> </span> </td>';
            str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 12px\" ';
            str += '                                                  class=\"style2\">';
            str += '                                                  &nbsp;';
            str += '                                              </td>';
            str += '                                          </tr>';
            str += '                                          <tr>';
            str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px height: 12px\" ';
            str += '                                                  class=\"style1\">';
            str += '                                                   Wholesales</td>';
            str += '                                              <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 12px\">';
            str += '                                                  {6}</td>';
            str += '                                              <td style=\"white-space: nowrap; width: 34px vertical-align: top height: 12px\">';
            str += '                                                   <span id="idWholesalesMoved"></span>   </td>';
            str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 12px\" ';
            str += '                                                  class=\"style2\">';
            str += '                                                  &nbsp;';
            str += '                                             </td>';
            str += '                                         </tr>';
            str += '                                         <tr>';
            str += '                                             <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px height: 12px\" ';
            str += '                                                 class=\"style1\">';
            str += '                                                 DSP:</td>';
            str += '                                             <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 12px\">';
            str += '                                                 {7}</td>';
            str += '                                             <td style=\"white-space: nowrap; width: 34px vertical-align: top height: 12px\">';
            str += '                                                 </td>';
            str += '                                             <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 12px\" ';
            str += '                                                 class=\"style2\">';
            str += '                                                 &nbsp;';
            str += '                                             </td>';
            str += '                                         </tr>';
            str += '                                          <tr>';
            str += '                                              <td style=\"padding-left: 15px vertical-align: top width: 166px white-space: nowrap\" ';
            str += '                                                  class=\"style1\">';
            str += '                                                  AMS in VNĐ:</td>';
            str += '                                              <td style=\"vertical-align: top width: 154px white-space: nowrap\">';
            str += '                                                  {8}</td>';
            str += '                                              <td style=\"vertical-align: top width: 34px white-space: nowrap\">';
            str += '                                                   <span id="idAMSMoved"></span>   </td>';
            str += '                                              <td style=\"vertical-align: top width: 1px white-space: nowrap\" class=\"style2\">';
            str += '                                              </td>';
            str += '                                          </tr>';
            str += '                                         <tr>';
            str += '                                             <td colspan=\"4\" style=\"padding-left: 15px vertical-align: top white-space: nowrap\">';
            str += '                                                 <hr />';
            str += '                                             </td>';
            str += '                                         </tr>';

            str += '                                         <tr>';
            str += '                                             <td style=\"padding-left: 15px vertical-align: top width: 166px white-space: nowrap';
            str += '                                                 color: Blue\" class=\"style1\">';
            str += '                                                 <a id=\"MoveHere\" onclick=\"MoveHere()\">';
            str += '                                                     <img id=\"StartImg\" alt=\"\" {100} />';
            str += '                                                    Move Here </a>';
            str += '                                         </td>';


            str += '                        <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap;';
            str += '                            color: Blue\">';
            str += '                            <a id=\"SelectMove\" onclick=\"fnsDistributorDirectSelect()\">';
            str += '                                <img id=\"SelectMoveImg\" alt=\"\" {105} />';
            str += '                                 <span id=\"TitleSelect\" > {Select} </span>   </a> ';
            str += '                        </td> ';

            str += '                        {ViewDistance} ';
            str += '                        </tr>                ';

            str += '                                     </table>';
            str += '';




            var arrowOption = 'src=\"icon/arrowOption.png\"';
            var arrowDownOption = 'src=\"icon/arrowOptionDown.png\"';

            //Start Point
            if (distributorSelectMove == undefined || distributorSelectMove.getPoint() != markerTmp.getPoint()) {
                str = str.replace("{100}", arrowOption);
            } else
                if (distributorSelectMove.getPoint() == markerTmp.getPoint()) {
                    str = str.replace("{100}", arrowDownOption);
                }

            //Select NPP Select                    
            if (obj.getIcon().image == "/icon/ND.png") {
                str = str.replace("{105}", arrowOption);
                str = str.replace("{Select}", "Select");
            }
            else {
                str = str.replace("{105}", arrowDownOption);
                str = str.replace("{Select}", "Deselect");

            }



            if ((pointCDs.length == 1 && centralPoints.length == 0) || (pointCDs.length == 0 && centralPoints.length == 1)) {
                var strTemp = "";
                strTemp += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
                strTemp += '                            <a id=\"ViewDistance\" onclick=\"fnsViewDistance()\">';
                strTemp += '                                View distance </a>   <span id="ResultViewDistance"> </span> ';
                strTemp += '                        </td>';

                str = str.replace("{ViewDistance}", strTemp);
            }
            else {
                str = str.replace("{ViewDistance}", "");
            }


            var NAME = obj.NAME;
            var FULL_ADDRESS = obj.FULL_ADDRESS;
            var POPULATION = obj.POPULATION;
            var RADIUS = obj.RADIUS;
            var AREA_COVERED = obj.AREA_COVERED;
            var CUSTOMERS = obj.CUSTOMERS;
            var WHOLE_SALES = obj.WHOLE_SALES;
            var DSP = obj.DSP;
            var AMS = obj.AMS;


            str = str.replace("{0}", NAME);
            str = str.replace("{1}", FULL_ADDRESS);
            str = str.replace("{2}", POPULATION);
            str = str.replace("{3}", RADIUS);
            str = str.replace("{4}", AREA_COVERED);
            str = str.replace("{5}", CUSTOMERS);
            str = str.replace("{6}", WHOLE_SALES);
            str = str.replace("{7}", DSP);
            str = str.replace("{8}", AMS);


            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);
            ViewInfoAfterMove();
        });
        return mar;
    }
}

function ClassMarkerDistributorNetworkInDirect(myMap, pathImage, StrObject) {

    this.myMap = myMap;
    this.pathImage = pathImage;
    this.VMarkers = b(StrObject, this.pathImage, this.myMap);
    this.show = c;
    this.hide = d;
    this.IsMarkerShow = true;

    function c() {
        for (var i = 0; i < this.VMarkers.length; i++) {
            this.VMarkers[i].show();
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.VMarkers.length; i++) {

            this.VMarkers[i].hide();
        }
        this.IsMarkerShow = false;
    }

    function b(v, p, m) {
        var markers = new Array();
        var object = $.parseJSON(v);
        for (var i = 0; i < object.length; i++) {
            var icon = p;
            var lat = object[i].LONGITUDE_LATITUDE.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);

            var mar = new google.maps.Marker({
                position: pt,
                icon: icon
            });
            mar.setMap(m);

            //m.addOverlay(mar);
            var obj = object[i];
            mar.addListener('click', function () {
                markerTmp = obj;
                markerViewDistance = obj;
                markerMoveTmp = obj;


                var str = '';
                str += '';
                str += '                 <table style=\"font-family:Tahoma; font-size:11px;width: 401px; text-align:left\" ';
                str += '                     cellpadding=\"0\" cellspacing=\"2\" border=\"0\">';
                str += '                                          <tr>';
                str += '                                              <td colspan=\"4\" style=\"white-space: nowrap; width: 375px vertical-align: top\">';
                str += '                                                  <h3>  {0}  </h3>';
                str += '                                              </td>';
                str += '                                          </tr>';
                str += '                                          <tr>';
                str += '                                              <td colspan=\"4\" style=\"white-space: nowrap; width: 375px vertical-align: top\">';
                str += '                                                <img src=\"/icon/address.png\" />  {1}';
                str += '                                              </td>';
                str += '                                          </tr>';

                str += '                                          <tr>';
                str += '                                              <td colspan=\"4\" style=\"white-space: nowrap; width: 375px vertical-align: top\">';
                str += '                                            Under Distributor:   {150}';
                str += '                                              </td>';
                str += '                                          </tr>';




                str += '                                          <tr>';
                str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top height: 19px padding-left:15px\" ';
                str += '                                                  class=\"style1\">';
                str += '                                                  &nbsp;</td>';
                str += '                                               <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 19px; font-weight: 700;\">';
                str += '                                                 <span id="idTitleCurrent"></span>    </td>';
                str += '                                              <td style=\"white-space: nowrap; width: 34px vertical-align: top height: 19px; font-weight: 700;\">';
                str += '                                                <span id="idTitleMoved"></span>   </td>';
                str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 19px\" ';
                str += '                                                  class=\"style2\">';
                str += '                                                  &nbsp;</td>';
                str += '                                          </tr>';
                str += '                                          <tr>';
                str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top height: 19px padding-left:15px\" ';
                str += '                                                  class=\"style1\">';
                str += '                                                  Population covered :';
                str += '                                              </td>';
                str += '                                               <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 19px\">';
                str += '                                                   {2}';
                str += '                                              </td>';
                str += '                                              <td style=\"white-space: nowrap; width: 34px vertical-align: top height: 19px\">';
                str += '                                                   <span id="idPopulationMoved"></span> </td>';
                str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 19px\" ';
                str += '                                                  class=\"style2\">';
                str += '                                                  &nbsp;';
                str += '                                              </td>';
                str += '                                          </tr>';
                str += '                                          <tr>';
                str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px height: 12px\" ';
                str += '                                                  class=\"style1\">';
                str += '                                                  Radius :';
                str += '                                              </td>';
                str += '                                              <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 12px\">';
                str += '                                                  {3} Km</td>';
                str += '                                              <td  style=\"white-space: nowrap; width: 34px vertical-align: top height: 12px\">';
                str += '                                                   <span id="idRadiusMoved"></span> </td>';
                str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 12px\" ';
                str += '                                                  class=\"style2\">';
                str += '                                                  &nbsp;';
                str += '                                              </td>';
                str += '                                          </tr>';
                str += '                                          <tr>';
                str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px\" ';
                str += '                                                  class=\"style1\">';
                str += '                                                  Area covered</td>';
                str += '                                              <td style=\"white-space: nowrap; width: 154px vertical-align: top\">';
                str += '                                                  {4} Km2</td>';
                str += '                                              <td style=\"white-space: nowrap; width: 34px vertical-align: top\">';
                str += '                                                  <span id="idAreadMoved"></span></td>';
                str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top\" class=\"style2\">';
                str += '                                                  &nbsp;';
                str += '                                              </td>';
                str += '                                          </tr>';
                str += '                                          <tr>';
                str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px height: 12px\" ';
                str += '                                                  class=\"style1\">';
                str += '                                                  Number of Customers :</td>';
                str += '                                              <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 12px\">';
                str += '                                                  {5} </td>';
                str += '                                              <td  style=\"white-space: nowrap; width: 34px vertical-align: top height: 12px\">';
                str += '                                                 <span id="idNumberOfCustomerMoved"> </span> </td>';
                str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 12px\" ';
                str += '                                                  class=\"style2\">';
                str += '                                                  &nbsp;';
                str += '                                              </td>';
                str += '                                          </tr>';
                str += '                                          <tr>';
                str += '                                              <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px height: 12px\" ';
                str += '                                                  class=\"style1\">';
                str += '                                                   Wholesales</td>';
                str += '                                              <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 12px\">';
                str += '                                                  {6}</td>';
                str += '                                              <td style=\"white-space: nowrap; width: 34px vertical-align: top height: 12px\">';
                str += '                                                   <span id="idWholesalesMoved"></span>   </td>';
                str += '                                              <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 12px\" ';
                str += '                                                  class=\"style2\">';
                str += '                                                  &nbsp;';
                str += '                                             </td>';
                str += '                                         </tr>';
                str += '                                         <tr>';
                str += '                                             <td style=\"white-space: nowrap; width: 166px vertical-align: top padding-left:15px height: 12px\" ';
                str += '                                                 class=\"style1\">';
                str += '                                                 DSP:</td>';
                str += '                                             <td style=\"white-space: nowrap; width: 154px vertical-align: top height: 12px\">';
                str += '                                                 {7}</td>';
                str += '                                             <td style=\"white-space: nowrap; width: 34px vertical-align: top height: 12px\">';
                str += '                                                 </td>';
                str += '                                             <td style=\"white-space: nowrap; width: 1px vertical-align: top height: 12px\" ';
                str += '                                                 class=\"style2\">';
                str += '                                                 &nbsp;';
                str += '                                             </td>';
                str += '                                         </tr>';
                str += '                                          <tr>';
                str += '                                              <td style=\"padding-left: 15px vertical-align: top width: 166px ;white-space: nowrap\" ';
                str += '                                                  class=\"style1\">';
                str += '                                                  AMS in VNĐ:</td>';
                str += '                                              <td style=\"vertical-align: top width: 154px; white-space: nowrap\">';
                str += '                                                  {8}</td>';
                str += '                                              <td style=\"vertical-align: top width: 34px; white-space: nowrap\">';
                str += '                                                   <span id="idAMSMoved"></span>   </td>';
                str += '                                              <td style=\"vertical-align: top width: 1px ;white-space: nowrap\" class=\"style2\">';
                str += '                                              </td>';
                str += '                                          </tr>';
                str += '                                         <tr>';
                str += '                                             <td colspan=\"4\" style=\"padding-left: 15px vertical-align: top ;white-space: nowrap\">';
                str += '                                                 <hr />';
                str += '                                             </td>';
                str += '                                         </tr>';

                str += '                                         <tr>';
                str += '                                             <td style=\"padding-left: 15px vertical-align: top width: 166px white-space: nowrap';
                str += '                                                 color: Blue\" class=\"style1\">';
                str += '                                                 <a id=\"MoveHere\" onclick=\"MoveHere()\">';
                str += '                                                     <img id=\"StartImg\" alt=\"\" {100} />';
                str += '                                                    Move Here </a>';
                str += '                                         </td>';


                str += '                        <td style=\"padding-left: 15px; vertical-align: top; width: 166px; white-space: nowrap;';
                str += '                            color: Blue\">';
                str += '                            <a id=\"SelectMove\" onclick=\"fnsDistributorInDirectSelect()\">';
                str += '                                <img id=\"SelectMoveImg\" alt=\"\" {105} />';
                str += '                                 <span id=\"TitleSelect\" > {Select} </span>   </a> ';
                str += '                        </td> ';

                str += '                        {ViewDistance} ';
                str += '                        </tr>                ';

                str += '                                     </table>';
                str += '';




                var arrowOption = 'src=\"icon/arrowOption.png\"';
                var arrowDownOption = 'src=\"icon/arrowOptionDown.png\"';

                //Start Point
                if (distributorSelectMove == undefined || distributorSelectMove.getPoint() != markerTmp.getPoint()) {
                    str = str.replace("{100}", arrowOption);
                } else
                    if (distributorSelectMove.getPoint() == markerTmp.getPoint()) {
                        str = str.replace("{100}", arrowDownOption);
                    }

                //Select NPP Select                    
                if (obj.getIcon().image == "/icon/SD.png") {
                    str = str.replace("{105}", arrowOption);
                    str = str.replace("{Select}", "Select");
                }
                else {
                    str = str.replace("{105}", arrowDownOption);
                    str = str.replace("{Select}", "Deselect");

                }



                if ((pointCDs.length == 1 && centralPoints.length == 0) || (pointCDs.length == 0 && centralPoints.length == 1)) {
                    var strTemp = "";
                    strTemp += '                        <td style=\"vertical-align: top; width: 154px; white-space: nowrap; color: Blue\">';
                    strTemp += '                            <a id=\"ViewDistance\" onclick=\"fnsViewDistance()\">';
                    strTemp += '                                View distance </a>   <span id="ResultViewDistance"> </span> ';
                    strTemp += '                        </td>';

                    str = str.replace("{ViewDistance}", strTemp);
                }
                else {
                    str = str.replace("{ViewDistance}", "");
                }


                var NAME = obj.NAME;
                var FULL_ADDRESS = obj.FULL_ADDRESS;
                var POPULATION = obj.POPULATION;
                var RADIUS = obj.RADIUS;
                var AREA_COVERED = obj.AREA_COVERED;
                var CUSTOMERS = obj.CUSTOMERS;
                var WHOLE_SALES = obj.WHOLE_SALES;
                var DSP = obj.DSP;
                var AMS = obj.AMS;
                var parentName = obj.ParentName;

                str = str.replace("{0}", NAME);
                str = str.replace("{1}", FULL_ADDRESS);
                str = str.replace("{2}", POPULATION);
                str = str.replace("{3}", RADIUS);
                str = str.replace("{4}", AREA_COVERED);
                str = str.replace("{5}", CUSTOMERS);
                str = str.replace("{6}", WHOLE_SALES);
                str = str.replace("{7}", DSP);
                str = str.replace("{8}", AMS);

                if (parentName.length >= 2) {
                    str = str.replace("{150}", parentName);
                }
                else
                    str = str.replace("{150}", "");

                var infowindow = new google.maps.InfoWindow({
                    content: str
                });
                for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                    CTracking.prototype.mInfowindow[i].close();
                }
                this.mInfowindow = null;
                infowindow.open(myMap, mar);
                CTracking.prototype.mInfowindow.push(infowindow);



                ViewInfoAfterMove();

            });
            markers.push(mar);

        }


        return markers;
    };


};

////// code bên dưới được xử dụng cho tracking.aspx và trackingforhh.aspx
function CTracking(myMap, myObjectTrackingOfSales, objectShop) {
    this.myMap = myMap;      //đối tượng bản đồ
    this.myObjectTrackingOfSales = myObjectTrackingOfSales;  //json về tracking (danh sách các điểm)    
    this.objectShop = objectShop;     // json về khách hàng (các thông tin cơ bản về khách hàng sẽ được hiển thị khi use click vào)
    this.SalesCD_Shops = -1; //khỏi tạo giá trị mặt định của NVBH,ASM,CDS đc chọn để xem
    this.SalesCD_Point = -1; //khỏi tạo giá trị mặt định của NVBH,ASM,CDS đc chọn để xem
    this.SalesCD_Stop = -1; //khỏi tạo giá trị mặt định của NVBH,ASM,CDS đc chọn để xem
    this.SalesCD_Route = -1; //khỏi tạo giá trị mặt định của NVBH,ASM,CDS đc chọn để xem
    this.SalCD = -1; //khỏi tạo giá trị mặt định của NVBH,ASM,CDS đc chọn để xem
    this.arrowOption = -1;
    this.arrowDownOption = -1;

    //khỏi tạo marker lộ trình đi    
    initMarker(this.myObjectTrackingOfSales, this.myMap);

    //khỏi tạo marker cửa hàng     
    this.CreateShops = cmsh;
    function cmsh() {
        //nếu marker cửa hàng đã tạo rồi thì exit
        if (this.IsShopsCreated == true)
            return;


        var object = $.parseJSON(this.objectShop);  //parse JSON thành mảng các đối tượng để xử lý


        // duyệt for
        for (var i = 0; i < object.length; i++) {

            var icon; //tạo mới đối tượng icon - dùng để hiển thị hình ảnh trên bản đồ

            if (object[i].CustomerIsVisit == "1") {
                icon = PATH_ICON_OUTLET_VISITED;   //gán icon đã visit (xem trang Tracking.aspx)
            }
            else {
                icon = PATH_ICON_OUTLET;    //gán icon chưa visit  (xem trang Tracking.aspx)        
            }

            var marker = CreateMarkerShops(eval("new google.maps.LatLng(" + object[i].LatLng + ")"), object[i], icon, this.myMap, "");
            marker.setVisible(false);
            var MMarket = new Object();
            MMarket.marker = marker;
            MMarket.obj = object[i];
            CTracking.prototype.mShops.push(MMarket);
        }

        this.IsShopsCreated = true;
    }

    //khỏi tạo hàm ẩn hiển marker shop (khách hàng) - khi gọi hàm này thì sẽ ảnh hiển marker shop (nếu đang ẩn thì hiển thị ra, ngược lại thì ẩn)
    this.ProcessShowHideShop = pshshop; //hàm xử lý ẩn hiện icon và text
    this.ShowShops = sshop; //hàm ẩn hiển marker shop (hàm xử lý ẩn hiển marker shop)
    this.IsShopsShow = false;   //khỏi tạo giá trị mặt định shop chưa đc hiển thị lên bản đồ
    this.IsShopsCreated = false;   //khỏi tạo giá trị mặt định shop đã được khỏi tạo

    function pshshop(salesCD) {

        this.SalesCD_Shops = salesCD; //gán giá trị ghi nhận NVBH đc chọn để hiển thị thông tin   

        if (this.IsShopsCreated == true) // nếu shop đã đc khỏi tạo
        {
            //if (this.IsShopsShow == true) //shop đang hiển thì thiết lặp lại SalesCD_Shops để không hiển thị marker shop lên
            //    this.SalesCD_Shops = -1;

            if (this.IsShopsShow == true) //nếu shop đang hiển thị trên bản đồ thì sẽ cập nhật lại image và text
            {
                if (document.getElementById("imgShowHideShop") != null) {
                    document.getElementById("imgShowHideShop").setAttribute("src", PATH_ARROW_IMG);
                }
                if (document.getElementById("imgShowHideShop2") != null) {
                    document.getElementById("imgShowHideShop2").setAttribute("src", PATH_ARROW_IMG);
                }

                if (document.getElementById("spanShowHideShop") != null) {
                    document.getElementById("spanShowHideShop").innerText = "Show store";
                }
                if (document.getElementById("spanShowHideShop2") != null) {
                    document.getElementById("spanShowHideShop2").innerText = "Show store";
                }
                this.SalesCD_Shops = -1;
            }
            else {
                //nếu shop chưa hiển thị trên bản đồ thì sẽ cập nhật lại image và text 
                if (document.getElementById("imgShowHideShop") != null) {
                    document.getElementById("imgShowHideShop").setAttribute("src", PATH_ARROW_DOWN_IMG);
                }
                if (document.getElementById("imgShowHideShop2") != null) {
                    document.getElementById("imgShowHideShop2").setAttribute("src", PATH_ARROW_DOWN_IMG);
                }
                if (document.getElementById("spanShowHideShop") != null) {
                    document.getElementById("spanShowHideShop").innerText = "Hide store";
                }
                if (document.getElementById("spanShowHideShop2") != null) {
                    document.getElementById("spanShowHideShop2").innerText = "Hide store";
                }
            }
        }
        else {   //shop chưa được khỏi tạo thì sẽ phải khỏi tạo 
            this.CreateShops();
            if (document.getElementById("imgShowHideShop") != null) {
                document.getElementById("imgShowHideShop").setAttribute("src", PATH_ARROW_DOWN_IMG);
            }
            if (document.getElementById("imgShowHideShop2") != null) {
                document.getElementById("imgShowHideShop2").setAttribute("src", PATH_ARROW_DOWN_IMG);
            }
            if (document.getElementById("spanShowHideShop") != null) {
                document.getElementById("spanShowHideShop").innerText = "Hide store";
            }
            if (document.getElementById("spanShowHideShop2") != null) {
                document.getElementById("spanShowHideShop2").innerText = "Hide store";
            }
        }
        //gọi hàm showShop để ẩn hiện shop tùy vào  this.SalesCD_Shops (nếu -1 thì ẩn ngược lại hiên thị theo SalesCD)
        this.ShowShops();
        if (bol != true) {
            myMap.fitOverlays();
        }
    }

    function sshop() {
        //nếu mảng mShop null thì exit   
        if (this.mShops == undefined)
            return;

        this.IsShopsShow = false;// gán lại giá trị ghi nhận shop chưa được hiển thị 
        for (var i = 0; i < this.mShops.length; i++) {
            if (this.mShops[i].obj.SalesCD == this.SalesCD_Shops) //duyệt mảng hiển thị các marker tương ứng với NVBH chọn
            {
                this.mShops[i].marker.setVisible(true);
                this.IsShopsShow = true;
            }
            else //khác NVBH chọn thì ẩn đi
            {
                this.mShops[i].marker.setVisible(false);
            }
        }
        if (this.IsShopsShow == true) {
            fitOverlays(this.myMap, CTracking.prototype.mShops);
        }
    }

    //process polyline - hàm này để vẽ 1 đường line dựa vào danh sách các tọa độ và hiển thị trên bản đồ
    
     //cách thức hoạt động về code giống như showShop 
    this.ProcessShowHideRoute = pshr;
    function pshr(salesCD) {
        this.SalesCD_Route = salesCD;
            
            if (this.IsRoutelineShow == true) {
                if (document.getElementById("imgShowHideRoute") != null) {
                    document.getElementById("imgShowHideRoute").setAttribute("src", PATH_ARROW_IMG);
                }
                if (document.getElementById("imgShowHideRoute2") != null) {
                    document.getElementById("imgShowHideRoute2").setAttribute("src", PATH_ARROW_IMG);
                }

                if (document.getElementById("spanShowHideRoute") != null) {
                    document.getElementById("spanShowHideRoute").innerText = "Show route";
                }
                if (document.getElementById("spanShowHideRoute2") != null) {
                    document.getElementById("spanShowHideRoute2").innerText = "Show route";
                }
            }
            else {
                if (document.getElementById("imgShowHideRoute") != null) {
                    document.getElementById("imgShowHideRoute").setAttribute("src", PATH_ARROW_DOWN_IMG);
                }
                if (document.getElementById("imgShowHideRoute2") != null) {
                    document.getElementById("imgShowHideRoute2").setAttribute("src", PATH_ARROW_DOWN_IMG);
                }
                if (document.getElementById("spanShowHideRoute") != null) {
                    document.getElementById("spanShowHideRoute").innerText = "Hide route";
                }
                if (document.getElementById("spanShowHideRoute2") != null) {
                    document.getElementById("spanShowHideRoute2").innerText = "Hide route";
                }
            }
            this.ShowRouteLine();
        if (bol != true) {
            myMap.fitOverlays();
        }
    }
    this.ShowRouteLine = srline;
    this.IsRoutelineShow = false;
    //hàm ẩn hiển polyline theo NVBH đã chọn
    function srline() {
        if (this.mRouteLine == undefined || this.SalesCD_Route==-1)
            return;
        var check = false;
            for (var k = 0; k < CTracking.prototype.mRouteLine.length; k++) {
                if (CTracking.prototype.mRouteLine[k].SalesCD == this.SalesCD_Route) {
                    check = true;
                    break;
                    
                }
            }
            if (check == false)
            {
                var object = $.parseJSON(this.myObjectTrackingOfSales);
                var strLatLng = ""; //khai báo chuổi chứa danh sách các tọa độ VLatLng
                var split = "";
                for (var i = 0; i < object.length; i++) {
                    if (object[i].SalesCD == this.SalesCD_Route) {

                        split = object[i].LatLngs.split(',');
                        //object[i].LatLngs tọa độ điểm
                        if (object[i].TypeTracking == "S") {
                            strLatLng = "[{lat: " + split[0] + ", lng: " + split[1] + "}"; //nếu là điểm bắt đàu thì sẽ có riêng kí tự [
                        }
                        else
                            if (object[i].TypeTracking == "E") //nếu là điểm kết thúc thì có ký tự ] và tiến hành tạo polyline
                            {
                                strLatLng += ",{lat: " + split[0] + ", lng: " + split[1] + "}]";
                            }
                            else
                                if (object[i].TypeTracking == "P") //nếu là điểm dừng bình thường thì tiến hành nối chuổi các tọa độ
                                {
                                    strLatLng += ",{lat: " + split[0] + ", lng: " + split[1] + "}";
                                }
                    }
                }
                var flightPath = new google.maps.Polyline({
                    geodesic: true,
                    path: eval(strLatLng),
                    strokeColor: '#FF0000',
                    strokeOpacity: 1.0,
                    strokeWeight: 1
                });
                var Route = new Object();
                Route.SalesCD = this.SalesCD_Route;
                Route.flightPath = flightPath;
                Route.show = false;
                CTracking.prototype.mRouteLine.push(Route);

            }
                

            for (var k = 0; k < CTracking.prototype.mRouteLine.length; k++) {
                if (CTracking.prototype.mRouteLine[k].SalesCD == this.SalesCD_Route) {
                    
                    if (CTracking.prototype.mRouteLine[k].show == true) {
                        CTracking.prototype.mRouteLine[k].flightPath.setMap(null);
                        CTracking.prototype.mRouteLine[k].show = false;
                        this.IsRoutelineShow = false;
                    } else if (CTracking.prototype.mRouteLine[k].show == false) {
                        CTracking.prototype.mRouteLine[k].flightPath.setMap(myMap);
                        CTracking.prototype.mRouteLine[k].show = true;
                        this.IsRoutelineShow = true
                        fitOverlaysRoute(myMap, CTracking.prototype.mRouteLine[k].flightPath)
                    }

                }
            }
        
    }



    //hàm khỏi tạo point (các điểm Start, Stop, Point, End)
    
    this.ProcessShowHidePoint = pshp;

    function pshp(salesCD) {
        this.NumOfPointText = 0;
        this.NumOfPoint = 0;
        this.SalesCD_Point = salesCD;
        this.StatusRePlay = false;
        this.IsStopReplay = true;

        if (this.IsStartCreated == true) {
            //  alert(this.IsStartShow);
            //if (this.IsStartShow == true)
            //    this.SalesCD_Point = -1;

            myTracking.ShowOrHidePoint();//

            if (this.PointStatus == false) {
                if (document.getElementById("imgShowHidePoint2") != null) {
                    document.getElementById("imgShowHidePoint2").setAttribute("src", PATH_ARROW_IMG);
                }
                if (document.getElementById("imgShowHidePoint") != null) {
                    document.getElementById("imgShowHidePoint").setAttribute("src", PATH_ARROW_IMG);
                    //document.getElementById("P5sTxtSecond").value = "0.5"; 
                }
                if (document.getElementById("spanShowHidePoint2") != null) {
                    document.getElementById("spanShowHidePoint2").innerText = "Show point";
                }
                if (document.getElementById("spanShowHidePoint") != null) {
                    document.getElementById("spanShowHidePoint").innerText = "Show point";
                }
                if (document.getElementById("spanShowRePlay") != null) {
                    document.getElementById("spanShowRePlay").innerText = "Replay";
                }

                //hiển thị tính năng Replay
                if (document.getElementById("spanShowRePlay2") != null) {
                    document.getElementById("spanShowRePlay2").innerText = "Replay";
                    document.getElementById("P5sDivSpeed").style.display = "block";
                }
                if (document.getElementById("imgShowRePlay2") != null) {
                    document.getElementById("imgShowRePlay2").setAttribute("src", "icon/replay.png");
                }
            }
            else {
                if (document.getElementById("imgShowHidePoint") != null) {
                    document.getElementById("imgShowHidePoint").setAttribute("src", PATH_ARROW_DOWN_IMG);
                    document.getElementById("imgShowRePlay").setAttribute("src", "icon/replay.png");
                }
                if (document.getElementById("imgShowHidePoint2") != null) {
                    document.getElementById("imgShowHidePoint2").setAttribute("src", PATH_ARROW_DOWN_IMG);
                }
                if (document.getElementById("spanShowHidePoint2") != null) {
                    document.getElementById("spanShowHidePoint2").innerText = "Hide point";
                }
                if (document.getElementById("spanShowHidePoint") != null) {
                    document.getElementById("spanShowHidePoint").innerText = "Hide point";
                }
                if (document.getElementById("spanShowRePlay") != null) {
                    document.getElementById("spanShowRePlay").innerText = "Replay";
                }

                //hiển thị tính năng Replay
                if (document.getElementById("spanShowRePlay2") != null) {
                    document.getElementById("spanShowRePlay2").innerText = "Replay";
                    document.getElementById("P5sDivSpeed").style.display = "block";
                }
                if (document.getElementById("imgShowRePlay2") != null) {
                    document.getElementById("imgShowRePlay2").setAttribute("src", "icon/replay.png");
                }
            }
        }
        else {

            this.CreateStart(); // khỏi tạo điểm bắt đầu
            this.CreatePoint(); //khỏi tạo các điểm dừng
            // this.CreateRouteText(); //khỏi tạo các text đánh số thứ tự của các điểm
            if (document.getElementById("imgShowHidePoint2") != null) {
                document.getElementById("imgShowHidePoint2").setAttribute("src", PATH_ARROW_DOWN_IMG);
            }

            if (document.getElementById("imgShowHidePoint") != null) {
                document.getElementById("imgShowHidePoint").setAttribute("src", PATH_ARROW_DOWN_IMG);
            }
            if (document.getElementById("spanShowHidePoint2") != null) {
                document.getElementById("spanShowHidePoint2").innerText = "Hide point";
            }
            if (document.getElementById("spanShowHidePoint") != null) {
                document.getElementById("spanShowHidePoint").innerText = "Hide point";
            }
            //hiển thị tính năng Replay
            if (document.getElementById("spanShowRePlay2") != null) {
                document.getElementById("spanShowRePlay2").innerText = "Replay";
                document.getElementById("P5sDivSpeed").style.display = "block";
            }
        }

        //if (bol != true) {
        //    myMap.fitOverlays();
        //}
    }

    //process start point
    this.CreateStart = cs;
    function cs() {
        if (this.IsStartCreated == true)
            return;

        var object = $.parseJSON(this.myObjectTrackingOfSales);

        for (var i = 0; i < object.length; i++) {
            if (object[i].MainPoint == "Start")  //khỏi tạo marker bắt đầu nếu như loại MainPoint = Start
            {
                var icon;
                icon = object[i].ImageUrl;
                var marker = CreateTrackingPoint(eval("new google.maps.LatLng(" + object[i].LatLngs + ")"), object[i], icon, this.myMap, "");

                if (object[i].SalesCD == this.SalesCD_Point) {
                    marker.setVisible(true);
                } else {
                    marker.setVisible(false);
                }

                var Marker = new Object();
                Marker.marker = marker;
                Marker.obj = object[i];
                CTracking.prototype.mStart.push(Marker);
            }
        }
        this.IsStartCreated = true;
    }
    this.ShowStart = ss;
    //this.IsStartShow = false;
    this.IsStartCreated = false;
    this.PointStatus = false;

    //cách thức ẩn hiển giống như showShop
    function ss() {
        if (this.mStart == undefined)
            return;

        this.IsStartShow = false;
        for (var i = 0; i < this.mStart.length; i++) {
            if (this.mStart[i].SalesCD == this.SalesCD_Point) {
                this.mStart[i].setVisible(true);
                this.IsStartShow = true;
            }
            // else
            //this.mStart[i].hide();
        }
    }



    //process route number
    this.CreateRouteText = crt;
    function crt() {
        if (this.IsRouteTextCreated == true)
            return;

        var object = $.parseJSON(this.myObjectTrackingOfSales);

        var indexRoute = 1;
        for (var i = 0; i < object.length; i++) {
            if (object[i].TypeTracking == "S") {

            }
            else
                if (object[i].TypeTracking == "E") //nếu là điểm kết thúc thì reset lại việc đánh số thứ tự
                {
                    indexRoute = 1; //reset index route
                }
                else
                    if (object[i].TypeTracking == "P") {
                        //tạo đối tượng VText để hiển thị thứ tự đánh số giữa các point
                        //var myText = new VText(eval(" new google.maps.LatLng(" + object[i].LatLngs + ")"),indexRoute++, new VTextStyle(14,"bold","#ff0000","Times New Roman","-50") );                            
                        //myText.myObject = object[i];
                        //this.myMap.addOverlay(myText);    
                        //myText.hide();                         
                        //CTracking.prototype.mRouteText.push(myText);                              
                    }
        }
        this.IsRouteTextCreated = true;
    }
   // this.ShowRouteText = srt;
    this.IsRouteTextShow = false;
    this.IsRouteTextCreated = false;

    //cách thức ẩn hiển giống như show shop
    //function srt() {
    //    if (this.mRouteText == undefined)
    //        return;

    //    this.IsRouteTextShow = false;
    //    for (var i = 0; i < this.mRouteText.length; i++) {
    //        if (this.mRouteText[i].myObject.SalesCD == this.SalesCD_Point) {
    //            this.mRouteText[i].show();
    //            this.IsRouteTextShow = true;
    //        }
    //        else
    //            this.mRouteText[i].hide();
    //    }
    //}

    //process point
    this.CreatePoint = cp;
    function cp() {
        if (this.IsPointCreated == true)
            return;

        var object = $.parseJSON(this.myObjectTrackingOfSales);
        var indexRoute = 1;
        for (var i = 0; i < object.length; i++) {

            if (object[i].TypeTracking == "E") //nếu là điểm kết thúc thì reset lại việc đánh số thứ tự
            {
                indexRoute = 1; //reset index route
            }
            if (object[i].MainPoint == "Point")   //hàm khỏi tạo marker point dùng để hiển thị trên bản đồ
            {
                if (object[i].TypeTracking == "P")
                {
                        var icon = object[i].ImageUrl;
                        var marker = CreateTrackingPoint(eval("new google.maps.LatLng(" + object[i].LatLngs + ")"), object[i], icon, this.myMap, indexRoute++);
                        if (object[i].SalesCD == this.SalesCD_Point)
                        {
                            marker.setVisible(true);
                        }
                        else
                        {
                            marker.setVisible(false);
                        }
                        var Marker = new Object();
                        Marker.marker = marker;
                        Marker.obj = object[i];
                        CTracking.prototype.mPoint.push(Marker);
                    }
            }


            //hàm khỏi tạo marker point dùng để thực hiện tính năng replay trên bản đồ
            if (object[i].MainPoint == "Point" || object[i].MainPoint == "Stop") {
                 var icon = object[i].ImageUrl;

                 var marker = CreateMarkerReplay(eval("new google.maps.LatLng(" + object[i].LatLngs + ")"), object[i], icon, this.myMap);
                 marker.setVisible(false);
                //marker.hide();             
                 //CTracking.prototype.mPointRePlay.push(marker);
                 var Marker1 = new Object();
                 Marker1.marker = marker;
                 Marker1.obj = object[i];
                 CTracking.prototype.mPointRePlay.push(Marker1);
                //add listpoint selemain
                icon = 'icon/saleman2.png' 
                marker = CreateMarkerReplay(eval("new google.maps.LatLng(" + object[i].LatLngs + ")"), object[i], icon, this.myMap);
                marker.setVisible(false);
                //marker.hide();                                 
                // CTracking.prototype.mPointSaleManMoving.push(marker);
                var Marker2 = new Object();
                Marker2.marker = marker;
                Marker2.obj = object[i];
                CTracking.prototype.mPointSaleManMoving.push(Marker2);
                //CTracking.prototype.mLatLngsList.push(object[i].LatLngs);
                //add listpoint selemain   
                if (object[i].MainPoint == "Stop") {
                    var Marker3 = new Object();
                    Marker3.marker = marker;
                    Marker3.obj = object[i];
                    CTracking.prototype.mStopReplay.push(Marker3);
                }
             }
        }
        fitOverlays(this.myMap, CTracking.prototype.mPoint);
        this.IsPointCreated = true;
        this.PointStatus = true;
    }

    this.IsStopReplay = false; // true dang stop, false dang replay
    this.ShowPoint = sp;
    this.IsPointShow = false;
    this.IsPointCreated = false;
    this.ShowRePlayPoints = srpp;
    this.NumOfPoint = 0;
    function srpp() {

        this.IsPointShow = false;
        this.IsStopReplay = false;
        this.NumOfPoint = this.mPointRePlay.length;
        (function () {//pointreplay speed 180
            var sleep = P5sSpeedValue;
            var i = 0,
                action = function () {
                    if (myTracking.IsStopReplay == true)
                    {
                        HideReplay();
                        return;
                    }
              
                    if (CTracking.prototype.mPointRePlay[i].obj.SalesCD == myTracking.SaleCDRePlay) {
                        CTracking.prototype.mPointSaleManMoving[i].marker.setVisible(true);
                        if (i > 1) {
                            CTracking.prototype.mPointSaleManMoving[i - 1].marker.setVisible(false);
                            CTracking.prototype.mPointRePlay[i - 1].marker.setVisible(true);
                            sleep = P5sSpeedValue * 1000;
                            //move map when replay
                            //if (sleep != 0 && i < myTracking.NumOfPoint - 1) {
                            if (sleep != 0 && i < 4) {
                                var LatLng = CTracking.prototype.mPointSaleManMoving[i + 1].obj.LatLngs.split(",");
                                if (i == 0 || i % 3 == 0) {
                                    myMap.panTo(new google.maps.LatLng(LatLng[0], LatLng[1]));
                                }
                            }
                            //move map when replay

                            // resplay pause when stop point
                            for (var j = 0 ; j < CTracking.prototype.mStopReplay.length ; j++) {
                                if (CTracking.prototype.mPointRePlay[i].obj.LatLngs == CTracking.prototype.mStopReplay[j]) {
                                    sleep = sleep * 5;
                                }
                            }
                            // resplay pause when stop point

                        }
                        if (i == 0) {
                            CTracking.prototype.mPointSaleManMoving[i].marker.setVisible(false);
                        }
                    }
                    else {
                        sleep = 0;
                    }

                    i++;
                    if (i < myTracking.NumOfPoint) {
                        setTimeout(action, sleep);
                    }
                };
            setTimeout(action, sleep);
        })();

    }

    function sp() {

        if (this.mPoint == undefined) {
            return;
        }
        this.IsPointShow = false;
        for (var i = 0; i < this.mPoint.length; i++) {
            if (this.mPoint[i].obj.SalesCD == this.SalesCD_Point) {
                this.mPoint[i].marker.setVisible(true);
                this.IsPointShow = true;
            }
            else {
                this.mPoint[i].marker.setVisible(false);
            }
        }
    }

    this.StatusRePlay = false;
    this.SaleCDRePlay = -1;
    this.P5sShowRePlay = srp;
    this.ShowOrHidePoint = shp;
    function srp(SalesCD) {
        this.SaleCDRePlay = SalesCD;
        this.NumOfPointText = 0;
        this.NumOfPoint = 0;
        if (this.IsPointCreated == false)
        {
            this.ProcessShowHidePoint();
        }
        this.StatusRePlay = true;
        setTimeout(HideShowPointAndPointText, (P5sSpeedValue * 110 / 100) * 1000);//(document.getElementById("P5sTxtSecond").value) * 1000);
    }

    function shp() {
        if (this.PointStatus == true) {

            for (var i = 0; i < myTracking.mPoint.length; i++) {
                if (CTracking.prototype.mPoint[i].obj.SalesCD == this.SalesCD_Point) {
                    CTracking.prototype.mPoint[i].marker.setVisible(false);
                }
            }
            for (var i = 0; i < myTracking.mStart.length; i++) {
                if (CTracking.prototype.mStart[i].obj.SalesCD == this.SalesCD_Point) {
                    CTracking.prototype.mStart[i].marker.setVisible(false);
                }
            }
            this.PointStatus = false;
        }
        else if (this.PointStatus == false) {
            for (var i = 0; i < myTracking.mPoint.length; i++) {
                if (CTracking.prototype.mPoint[i].obj.SalesCD == this.SalesCD_Point) {
                    CTracking.prototype.mPoint[i].marker.setVisible(true);
                }
            }
            for (var i = 0; i < myTracking.mStart.length; i++) {
                if (CTracking.prototype.mStart[i].obj.SalesCD == this.SalesCD_Point) {
                    CTracking.prototype.mStart[i].marker.setVisible(true);
                }
            }
            fitOverlays(this.myMap, CTracking.prototype.mPoint);
            this.PointStatus = true;
        }
        HideReplay();
    }

    function HideReplay() {
        for (var i = 0; i < myTracking.mPointRePlay.length; i++) {
            CTracking.prototype.mPointRePlay[i].marker.setVisible(false);
        }
        for (var i = 0; i < myTracking.mPointSaleManMoving.length; i++) {
            CTracking.prototype.mPointSaleManMoving[i].marker.setVisible(false);
        }
    }

    function HideShowPointAndPointText() {
        //for (var i = 0; i < myTracking.mRouteText.length; i++) {
        //    CTracking.prototype.mRouteText[i].Visible(false);
        //}

        for (var i = 0; i < myTracking.mPointRePlay.length; i++) {
            CTracking.prototype.mPointRePlay[i].marker.setVisible(false);
            CTracking.prototype.mPointSaleManMoving[i].marker.setVisible(false);
        }

        for (var i = 0; i < myTracking.mPoint.length; i++) {
            // hide stop
            CTracking.prototype.mPoint[i].marker.setVisible(false);
        }

        //myTracking.ShowRePlayText();
        myTracking.ShowRePlayPoints();
    }



    //code xử lý chỉ hiển thị các điểm stoppoint 
    //cách thức hoạt động giống như showshop   
    this.ProcessShowHideStop = pshts;
    this.ShowStop = sts;
    this.StopStatus = false;
    this.IsStopCreated = false;



    function pshts(salesCD) {

        this.SalesCD_Stop = salesCD;



        if (this.IsStopCreated == true) {

            myTracking.ShowOrHideStop();

            //
            //if (this.StopStatus == true)
            //    this.SalesCD_Stop = -1;
            // alert(this.StopStatus);
            if (this.StopStatus == false) {
                if (document.getElementById("imgShowHideStop") != null) {
                    document.getElementById("imgShowHideStop").setAttribute("src", PATH_ARROW_IMG);
                }
                if (document.getElementById("imgShowHideStop2") != null) {
                    document.getElementById("imgShowHideStop2").setAttribute("src", PATH_ARROW_IMG);
                }

                if (document.getElementById("spanShowHideStop") != null) {
                    document.getElementById("spanShowHideStop").innerText = "Show stop";
                }

                if (document.getElementById("spanShowHideStop2") != null) {
                    document.getElementById("spanShowHideStop2").innerText = "Show stop";
                }
            }
            else {
                if (document.getElementById("imgShowHideStop") != null) {
                    document.getElementById("imgShowHideStop").setAttribute("src", PATH_ARROW_DOWN_IMG);
                }
                if (document.getElementById("imgShowHideStop2") != null) {
                    document.getElementById("imgShowHideStop2").setAttribute("src", PATH_ARROW_DOWN_IMG);
                }
                if (document.getElementById("spanShowHideStop") != null) {
                    document.getElementById("spanShowHideStop").innerText = "Hide stop";
                }
                if (document.getElementById("spanShowHideStop2") != null) {
                    document.getElementById("spanShowHideStop2").innerText = "Hide stop";
                }
            }
        }
        else {
            this.CreateRouteTextFS();
            this.CreateStop();

            if (document.getElementById("imgShowHideStop") != null) {
                document.getElementById("imgShowHideStop").setAttribute("src", PATH_ARROW_DOWN_IMG);
            }
            if (document.getElementById("imgShowHideStop2") != null) {
                document.getElementById("imgShowHideStop2").setAttribute("src", PATH_ARROW_DOWN_IMG);
            }

            if (document.getElementById("spanShowHideStop") != null) {
                document.getElementById("spanShowHideStop").innerText = "Hide stop";
            }
            if (document.getElementById("spanShowHideStop2") != null) {
                document.getElementById("spanShowHideStop2").innerText = "Hide stop";
            }
        }
        if (bol != true) {
            myMap.fitOverlays();
        }
    }


    this.CreateStop = cts;
    function cts() {
        if (this.IsStopCreated == true)
            return;

        var object = $.parseJSON(this.myObjectTrackingOfSales);

        for (var i = 0; i < object.length; i++) {
            if (object[i].MainPoint == "Stop") {
                var icon = object[i].ImageUrl;
                var marker = CreateTrackingPoint(eval("new google.maps.LatLng(" + object[i].LatLngs + ")"), object[i], icon, this.myMap, "");

                if (object[i].SalesCD == this.SalesCD_Stop) {
                    marker.setVisible(true);

                } else {
                    marker.setVisible(false);
                }

                var Marker = new Object();
                Marker.marker = marker;
                Marker.obj = object[i];
                CTracking.prototype.mStops.push(Marker);

            }
        }
        fitOverlays(this.myMap, CTracking.prototype.mStops);
        this.IsStopCreated = true;
        this.StopStatus = true;
    }


    function sts() {
        if (this.mStops == undefined)
            return;

        this.StopStatus = false;
        for (var i = 0; i < this.mStops.length; i++) {
            if (this.mStops[i].obj.SalesCD == this.SalesCD_Stop) {
                var duration = this.mStops[i].obj.Duration.split(":");

                var second = parseInt(duration[0] * 60 * 60) + parseInt(duration[1] * 60) + parseInt(duration[2]);

                if (second >= parseInt(durationDisplayStop * 60)) {
                    this.mStops[i].marker.setVisible(true);
                    this.StopStatus = true;
                }
                else
                    this.mStops[i].marker.setVisible(false);
            }
            else
                this.mStops[i].marker.setVisible(false);
        }
    }

    this.ShowOrHideStop = shst;

    function shst() {
        if (this.StopStatus == true) {
            for (var i = 0; i < myTracking.mStops.length; i++) {
                if (this.mStops[i].obj.SalesCD == this.SalesCD_Stop) {
                    CTracking.prototype.mStops[i].marker.setVisible(false);
                }
            }
            this.StopStatus = false;
        }
        else if (this.StopStatus == false) {
                for (var i = 0; i < myTracking.mStops.length; i++) {
                    if (this.mStops[i].obj.SalesCD == this.SalesCD_Stop) {
                        CTracking.prototype.mStops[i].marker.setVisible(true);
                    }
                }
                this.StopStatus = true;
                fitOverlays(this.myMap, CTracking.prototype.mStops);
            }
    }



    //process route number stop
    this.CreateRouteTextFS = crtfs;
    function crtfs() {
        if (this.IsRouteTextCreatedFS == true)
            return;

        var object = $.parseJSON(this.myObjectTrackingOfSales);

        var indexRoute = 1;
        for (var i = 0; i < object.length; i++) {
            if (object[i].TypeTracking == "S") {

            }
            else
                if (object[i].TypeTracking == "E") {
                    indexRoute = 1; //reset index route
                }
                else
                    if (object[i].MainPoint == "Stop") {
                        //var myText = new VText(eval(" new google.maps.LatLng(" + object[i].LatLngs + ")"),indexRoute++, new VTextStyle(14,"bold","#ff0000","Times New Roman","-50") );                            
                        //myText.myObject = object[i];
                        //this.myMap.addOverlay(myText);    
                        //myText.hide();    
                        //CTracking.prototype.mRouteTextFS.push(myText);                              
                    }
        }
        this.IsRouteTextCreatedFS = true;
    }
    this.ShowRouteTextFS = srts;
    this.IsRouteTextShowFS = false;
    this.IsRouteTextCreatedFS = false;


    function srts() {

        if (this.mRouteTextFS == undefined)
            return;

        this.IsRouteTextShowFS = false;


        for (var i = 0; i < this.mRouteTextFS.length; i++) {
            if (this.mRouteTextFS[i].myObject.SalesCD == this.SalesCD_Stop) {
                this.mRouteTextFS[i].show();
                this.IsRouteTextShowFS = true;
            }
            else
                this.mRouteTextFS[i].hide();
        }
    }


    //process end point     
    this.ShowEnd = se;
    this.HideEnd = he;
    this.IsEndShow = true;


    function se() {

        if (this.mEnd == undefined)
            return;

        for (var i = 0; i < this.mEnd.length; i++) {
            this.mEnd[i].show();
        }
        this.IsEndShow = true;
    }

    function he() {

        if (this.mEnd == undefined)
            return;

        for (var i = 0; i < this.mEnd.length; i++) {
            this.mEnd[i].hide();
        }
        this.IsEndShow = false;
    }




    var bol = true;
    function CreateTrackingPoint(LatLng, Object, icon, vmap, num) {
        //var mar = new google.maps.Marker({
        //    position: LatLng,
        //    icon: icon,
        //    label: type+""
        //});
        //mar.setMap(vmap);
        var mar = new MarkerWithLabel({
            position: LatLng,
            draggable: false,
            raiseOnDrag: true,
            map: vmap,
            labelContent: num,
            labelAnchor: new google.maps.Point(22, 0),
            labelClass: "labels", // the CSS class for the label
            icon: icon
        });

        var obj = Object;
        mar.addListener('click', function (obj) {
            obj = Object;
            //if(bol == true)//khởi tạo ham ProcessShowHideStop 1 lần fix issue 0009607. khởi tạo show/hide left panel issue 0009741
            // {alert();
            //    myTracking.ProcessShowHideStop(-1);
            //    myTracking.ProcessShowHidePoint(-1);
            //    myTracking.ProcessShowHideShop(-1);
            //    myTracking.ProcessShowHideRoute(-1);
            //    bol = false;
            // }  //khởi tạo ham ProcessShowHideStop 1 lần fix issue 0009607. khởi tạo show/hide left panel issue 0009741

            var str = '';
            str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left;\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
            str += '                    <tr>';
            str += '                        <td colspan=\"3\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            <h3> {0} </h3>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"3\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
            str += '                          <img src="/icon/address.png" />  {1}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"3\" style=\"white-space: nowrap; width: 375px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap;  vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                           TimeIn: </td>';
            str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                            {2} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                         TimeOut:   </td>';
            str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                            {3} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap;  vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                          Duration:   </td>';
            str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                            {4} (h:m:s) </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';


            //str += '                    <tr>';
            //str += '                        <td style=\"white-space: nowrap;  vertical-align: top; padding-left:15px; height: 12px;\">';
            //str += '                          Number of records:   </td>';
            //str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            //str += '                            {5} </td>';
            //str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            //str += '                            &nbsp;';
            //str += '                        </td>';
            //str += '                    </tr>';


            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap;  vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                         Battery status: </td>';
            str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                            {6} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                         PDA status : </td>';
            str += '                        <td  style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                        {7} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            //               
            //                 
            //                 str += '                    <tr>';
            //                 str += '                        <td style=\"white-space: nowrap;  vertical-align: top; padding-left:15px; height: 12px;\">';
            //                 str += '                        GPS Provider: </td>';
            //                 str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            //                 str += '                       {11}   </td>';
            //                 str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            //                 str += '                            &nbsp;';
            //                 str += '                        </td>';
            //                 str += '                    </tr>';
            //                 

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                         Radius : </td>';
            str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                            {8} (m) </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                         Number of stops : </td>';
            str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                            {9} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                         Number of store visits : </td>';
            str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                            {12} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    ';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; vertical-align: top; padding-left:15px; height: 12px;\">';
            str += '                         Total store in route list : </td>';
            str += '                        <td style=\"white-space: nowrap; width: 250px; vertical-align: top; height: 12px;\">';
            str += '                            {10} </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 12px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    {9999}';




            str += '                </table>';

            var salesDesc = obj.SalesName;  //obj.SalesCode + '-' + obj.SalesName;  
            var distributorDesc = obj.DistributorCode + '-' + obj.DistributorName;
            var timeIn = obj.TimeIn;
            var timeOut = obj.TimeOut;

            var noRepeat = obj.NoRepeat;


            var batteryPercentage = obj.BatteryPercentage;


            var radius = obj.SystemPointRadius;
            var deviceStatus = obj.DeviceStatus;
            var duration = obj.Duration;
            var provider = obj.Providers;
            var lastUpdate = obj.LastUpdate;


            str = str.replace("{0}", salesDesc);
            str = str.replace("{1}", distributorDesc);

            str = str.replace("{2}", timeIn);
            str = str.replace("{3}", timeOut);
            str = str.replace("{4}", duration);

            str = str.replace("{5}", noRepeat);
            str = str.replace("{6}", batteryPercentage);
            str = str.replace("{7}", deviceStatus);

            str = str.replace("{8}", radius);
            str = str.replace("{11}", provider);

            var numberOfStop = 0;
            var numberOfShop = 0;
            var numberOfShopVisit = 0;

            var myObject = $.parseJSON(myTracking.myObjectTrackingOfSales);

            for (var i = 0; i < myObject.length; i++) {
                if (myObject[i].SalesCD == obj.SalesCD && myObject[i].NumberOfStop == 1)
                    numberOfStop++;

            }

            myObject = $.parseJSON(myTracking.objectShop);
            for (var i = 0; i < myObject.length; i++) {
                if (myObject[i].SalesCD == obj.SalesCD)
                    numberOfShop++;

                if (myObject[i].SalesCD == obj.SalesCD && myObject[i].CustomerIsVisit == "1")
                    numberOfShopVisit++;
            }


            str = str.replace("{9}", numberOfStop + " (Last update: " + lastUpdate + " )");
            str = str.replace("{10}", numberOfShop);
            str = str.replace("{12}", numberOfShopVisit);

            if (obj.MainPoint == "End") {
                //reset lại trạng thái khi người dùng lick vào một điêm dừng khác
                if (document.getElementById("spanShowHideRoute2") != null)
                    document.getElementById("spanShowHideRoute2").innerText = "Show route";

                if (document.getElementById("imgShowHideRoute2") != null)
                    document.getElementById("imgShowHideRoute2").setAttribute("src", PATH_ARROW_IMG);

                if (document.getElementById("spanShowHideShop2") != null)
                    document.getElementById("spanShowHideShop2").innerText = "Show store";

                if (document.getElementById("imgShowHideShop2") != null)
                    document.getElementById("imgShowHideShop2").setAttribute("src", PATH_ARROW_IMG);

                if (document.getElementById("spanShowHidePoint2") != null)
                    document.getElementById("spanShowHidePoint2").innerText = "Show point";

                if (document.getElementById("imgShowHidePoint2") != null)
                    document.getElementById("imgShowHidePoint2").setAttribute("src", PATH_ARROW_IMG);

                if (document.getElementById("imgShowRePlay2") != null) {
                    document.getElementById("imgShowRePlay2").setAttribute("src", "icon/replay.png");
                    document.getElementById("spanShowRePlay2").innerText = "Replay";
                    document.getElementById("P5sDivSpeed").style.display = "block";
                }

                if (document.getElementById("spanShowHideStop2") != null)
                    document.getElementById("spanShowHideStop2").innerText = "Show stop";

                if (document.getElementById("imgShowHideStop2") != null)
                    document.getElementById("imgShowHideStop2").setAttribute("src", PATH_ARROW_IMG);
                //reset lại trạng thái khi người dùng lick vào một điêm dừng khác
                var tagHTML = '                      <tr>';
                tagHTML += '                        <td colspan=\"3\" style=\"padding-left: 15px; vertical-align: top; white-space: nowrap\">';
                tagHTML += '                            <hr />';
                tagHTML += '                        </td>';
                tagHTML += '                    </tr>';
                tagHTML += '                    <tr>';
                tagHTML += '                        <td colspan=\"3\" style=\"padding-left: 15px; vertical-align: top;';
                tagHTML += '                            color: Blue\">';
                tagHTML += '                            <a id=\"showPoint\" onclick=\"myTracking.ProcessShowHidePoint(' + obj.SalesCD + ');\">';
                tagHTML += '                              <img id=\"imgShowHidePoint\" alt=\"\" {100} />';
                tagHTML += '                              <span id="spanShowHidePoint">  {101} </span> &nbsp;&nbsp;&nbsp;&nbsp; </a>';

                tagHTML += '                            <a id=\"idShowStop\" onclick=\"myTracking.ProcessShowHideStop(' + obj.SalesCD + ');\">';
                tagHTML += '                              <img id=\"imgShowHideStop\" alt=\"\" {106} />';
                tagHTML += '                              <span id="spanShowHideStop">  {107} </span> &nbsp;&nbsp;&nbsp;&nbsp;  </a>';

                tagHTML += '                            <a id=\"idShowRoute\" onclick=\"myTracking.ProcessShowHideRoute(' + obj.SalesCD + ');\">';
                tagHTML += '                               <img id=\"imgShowHideRoute\" alt=\"\" {102} />';
                tagHTML += '                               <span id="spanShowHideRoute">  {103} </span> &nbsp;&nbsp;&nbsp;&nbsp;  </a>';

                tagHTML += '                            <a id=\"showShop\" onclick=\"myTracking.ProcessShowHideShop(' + obj.SalesCD + ');\">';
                tagHTML += '                               <img id=\"imgShowHideShop\" alt=\"\" {104} >';
                tagHTML += '                               <span id="spanShowHideShop">  {105} </span> &nbsp;&nbsp;&nbsp;&nbsp;  </a>';

                tagHTML += '                            <a id=\"showRePlay\" onclick=\"myTracking.P5sShowRePlay(' + obj.SalesCD + ');\">';
                tagHTML += '                               <img id=\"imgShowRePlay\" alt=\"\" {111} >';
                tagHTML += '                               <span id="spanShowRePlay">  {112} </span>  </a>';

                tagHTML += '                        </td>';
                tagHTML += '                    </tr>';

                var arrowOption = 'src="' + PATH_ARROW_IMG + '"';
                var arrowDownOption = 'src="' + PATH_ARROW_DOWN_IMG + '"\"';
                myTracking.arrowOption = arrowOption;
                myTracking.arrowDownOption = arrowDownOption;
                myTracking.SalCD = obj.SalesCD;

                //hide all data route line
                if (myTracking.SalesCD_Point != obj.SalesCD) {
                    myTracking.SalesCD_Point = -1;
                    myTracking.ShowStart();
                    myTracking.ShowPoint();
                }


                if (myTracking.IsStartCreated == true && myTracking.IsStartShow == true) {
                    tagHTML = tagHTML.replace("{100}", arrowDownOption);
                    tagHTML = tagHTML.replace("{101}", "Hide point");
                    tagHTML = tagHTML.replace("{111}", "src=\"icon/replay.png\"");
                    tagHTML = tagHTML.replace("{112}", "Replay");

                }
                else {
                    tagHTML = tagHTML.replace("{100}", arrowOption);
                    tagHTML = tagHTML.replace("{101}", "Show point");
                    tagHTML = tagHTML.replace("{111}", "src=\"icon/replay.png\"");
                    tagHTML = tagHTML.replace("{112}", "Replay");

                }



                //hide all data route line
                if (myTracking.SalesCD_Route != obj.SalesCD) {
                    myTracking.SalesCD_Route = -1;
                    myTracking.ShowRouteLine();
                }

                if (myTracking.IsPolylineCreated == true && myTracking.IsPolylineShow == true) {
                    tagHTML = tagHTML.replace("{102}", arrowDownOption);
                    tagHTML = tagHTML.replace("{103}", "Hide route");
                }
                else {
                    tagHTML = tagHTML.replace("{102}", arrowOption);
                    tagHTML = tagHTML.replace("{103}", "Show route");
                }





                //hide all data shop
                if (myTracking.SalesCD_Shops != obj.SalesCD) {
                    myTracking.SalesCD_Shops = -1;
                    myTracking.ShowShops();
                }

                if (myTracking.IsShopsCreated == true && myTracking.IsShopsShow == true) {
                    tagHTML = tagHTML.replace("{104}", arrowDownOption);
                    tagHTML = tagHTML.replace("{105}", "Hide store");
                }
                else {
                    tagHTML = tagHTML.replace("{104}", arrowOption);
                    tagHTML = tagHTML.replace("{105}", "Show store");
                }


                //hide all data stop
                if (myTracking.SalesCD_Stop != obj.SalesCD) {
                    myTracking.SalesCD_Stop = -1;
                    myTracking.ShowStop();
                }

                if (myTracking.IsStopCreated == true && myTracking.StopStatus == true) {
                    tagHTML = tagHTML.replace("{106}", arrowDownOption);
                    tagHTML = tagHTML.replace("{107}", "Hide stop");
                }
                else {
                    tagHTML = tagHTML.replace("{106}", arrowOption);
                    tagHTML = tagHTML.replace("{107}", "Show stop");
                }


                str = str.replace("{9999}", tagHTML);
            }
            else {
                str = str.replace("{9999}", "");
            }

            //var infowindow = new google.maps.InfoWindow({
            //    content: str
            //});
            //infowindow.open(vmap, mar);

            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);

        });

        return mar;
    }

    function CreateMarkerShops(LatLng, Object, icon, vmap, num) {

        //var mar = new google.maps.Marker({
        //    position: LatLng,
        //    map: vmap,
        //    icon: icon
        //});
        //mar.setMap(vmap);

        var mar = new MarkerWithLabel({
            position: LatLng,
            draggable: true,
            raiseOnDrag: true,
            map: vmap,
            labelContent: num,
            labelAnchor: new google.maps.Point(22, 0),
            labelClass: "labels", // the CSS class for the label
            icon: icon
        });

        var obj = Object;
        mar.addListener('click', function (obj) {
            obj = Object;
            //Lưu ý: {0},{1} v.v.v mục đích là để replace chuổi và thay đổi các thông tin cầ thiết 
            var str = '';
            str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            <h3> {0} </h3>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
            str += '                           <img src="/icon/address.png" />  {1}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                   ';
                     
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                            Distributor :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {2}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
                  
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           DSR :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {3}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
                        
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Cust type :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {4}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
                     
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Route :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {5}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           TimeIn :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {6}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           TimeOut :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {7}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
                        
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Duration :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {8}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
                          
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Sale :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {9}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
                           
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Number of Order :';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {10}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';
                     
                            
            str += '                </table>';
            
                    
            //lấy các thông tin về Khách hàng (Lưu ý để lấy thông tin thì phải gõ chính xác keywork: vd: CustomerCode (Đúng) -> customerCode (sai)
            //Các keywork này dựa vào các property khi khai báo đối tượng
                     
            var customerDesc = 'Store: ' + obj.CustomerName;// obj.CustomerCode + '-' + obj.CustomerName;  
            var customerAddess = obj.CustomerAddress ;
            var distributorDesc = obj.DistributorCode  + '-' +  obj.DistributorName;
            var salesDesc = obj.SalesName; // obj.SalesCode  + '-' +  obj.SalesName;                         
            var routeDesc = obj.RouteCode + '-' +  obj.RouteName;
            var customerChainCode = obj.CustomerChainCode ;
                     
            var timeIn = obj.TimeIn ;                           
            var timeOut = obj.TimeOut ;
            var duration = obj.Duration ;
                     
            var customerAmount = obj.CustomerSalesAmount ;
            var customerNoOrders = obj.CustomerOrders ;
                                  
            //replace chuổi thành các giá trị tương ứng             
            str = str.replace("{0}",customerDesc );
            str = str.replace("{1}",customerAddess ); 
            str = str.replace("{2}",distributorDesc ); 
            str = str.replace("{3}",salesDesc ); 
            str = str.replace("{4}",customerChainCode ); 
            str = str.replace("{5}",routeDesc ); 
                     
            str = str.replace("{6}",timeIn ); 
            str = str.replace("{7}",timeOut );
            str = str.replace("{8}",duration + ' (hh:mm:ss) ' ); 
            str = str.replace("{9}",customerAmount );
            str = str.replace("{10}",customerNoOrders ); 
                       
                       
            //var infowindow = new google.maps.InfoWindow({
            //    content: str
            //});
            //infowindow.open(vmap, mar);
            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);
        });
        return mar;
    }

    function CreateMarkerReplay(LatLng, Object, icon, vmap) {

        var mar = new google.maps.Marker({
            position: LatLng,
            map: vmap,
            icon: icon
        });
        mar.setMap(vmap);

        //var mar = new MarkerWithLabel({
        //    position: LatLng,
        //    draggable: true,
        //    raiseOnDrag: true,
        //    map: vmap,
        //    labelContent: num,
        //    labelAnchor: new google.maps.Point(22, 0),
        //    labelClass: "labels", // the CSS class for the label
        //    icon: icon
        //});
        return mar;
    }


    function initMarker(v, m) {
        //init array
        CTracking.prototype.mShops = new Array();
        CTracking.prototype.mRouteText = new Array();
        CTracking.prototype.mRouteTextFS = new Array();
        CTracking.prototype.mStops = new Array();
        CTracking.prototype.mStopReplay = new Array();
        CTracking.prototype.mStart = new Array();
        CTracking.prototype.mEnd = new Array();
        CTracking.prototype.mPoint = new Array();
        CTracking.prototype.mPointRePlay = new Array();
        CTracking.prototype.mPointSaleManMoving = new Array();
        CTracking.prototype.mLatLngsList = new Array();
        CTracking.prototype.mRouteLine = new Array();
        CTracking.prototype.mInfowindow = new Array();

        var object = $.parseJSON(v);
        var strLatLng = "";
        var indexRoute = 1;

        for (var i = 0; i < object.length; i++) {
            if (object[i].MainPoint == "End")  //khi khỏi tạo bản đồ mặc định hiển thị các stoppoint lên để use có thể chọn các tính năng và xử dụng 
            {

                var icon = object[i].ImageUrl;
                var marker = CreateTrackingPoint(eval("new google.maps.LatLng(" + object[i].LatLngs + ")"), object[i], icon, m, object[i].SalesName);
                //CTracking.prototype.mEnd.push(marker);
                var Marker = new Object();
                Marker.marker = marker;
                Marker.obj = object[i];
                CTracking.prototype.mEnd.push(Marker);
                //                    
                //                    var salesDesc = "[ " + object[i].SalesName + " ] " ;
                //            
                //                    var myText = new VText(eval(" new google.maps.LatLng(" + object[i].LatLngs + ")"),salesDesc, new VTextStyle(15,"bold","#ff00ff","Times New Roman","0px") );                            
                //                    myText.myObject = object[i];
                //                    this.myMap.addOverlay(myText);

            }
        }

    }

}

//MapCustomer.aspx
function LoadCustomerOnMap(myMap, jsonCustomer) {

    this.myMap = myMap;
    this.jsonCustomer = jsonCustomer;
    this.IsMarkerShow = true;
    this.show = c;
    this.hide = d;
    this.markers = new Array();

    this.CustomerMarkers = cmcust(myMap, jsonCustomer);

    function c() {

        for (var i = 0; i < this.CustomerMarkers.length; i++) {
            this.CustomerMarkers[i].show();
        }
        this.IsMarkerShow = true;
    }

    function d() {

        for (var i = 0; i < this.CustomerMarkers.length; i++) {
            this.CustomerMarkers[i].hide();
        }
        this.IsMarkerShow = false;
    }

    function cmcust(myMap, jsonCustomer) {

        var object = $.parseJSON(jsonCustomer);

        //create route line
        //var strLatLng = "";
        //for (var i = 0; i < object.length; i++) {
        //    if (i == 0)
        //        strLatLng = "[new google.maps.LatLng(" + object[i].LatLng + ")";
        //    else
        //        if (i == object.length - 1)
        //            strLatLng += ", new google.maps.LatLng(" + object[i].LatLng + ") ]";
        //        else
        //            strLatLng += ", new google.maps.LatLng(" + object[i].LatLng + ") ";
        //}


        //if (strLatLng != "") {
        //    var myPolyline = new VPolyline(eval(strLatLng), "#0066FF", 1, 'red', 0.6);
        //    myMap.addOverlay(myPolyline);
        //}

        for (var i = 0; i < object.length; i++) {


            var icon;

            if (object[i].CustomerIsVisit == "1") {
                icon = PATH_ICON_OUTLET_VISITED;
            }
            else {
                icon = PATH_ICON_OUTLET_NON_VISITED;
            }




            var lat = object[i].LatLng.split(",");
            var pt = new google.maps.LatLng(lat[0], lat[1]);
            var mar = new VMarker(pt, icon, undefined, object[i]);
            myMap.addOverlay(mar);

            //tạo stt for route
            //var myText = new VText(pt,i+1, new VTextStyle(14,"bold","#0000ff","Times New Roman","-50") );  
            // myMap.addOverlay(myText); 

            VEvent.addListener(mar, 'click', function (obj, latlng) {
                var opt = new VInfoWindowOptions(new VSize(9, 10), undefined, 0, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined);

                var str = '';
                str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
                str += '                    <tr>';
                str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
                str += '                            <h3>Store: {0} </h3>';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
                str += '                           <img src="/icon/address.png" />  {1}';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                    <tr>';
                str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                    </tr>';
                str += '                   ';

                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                            Distributor: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {2}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';
                str += '                    <tr>';


                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           DSR: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {3}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';
                str += '                    <tr>';


                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           Cust type: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {4}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';
                str += '                    <tr>';


                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           Route: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {5}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';


                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           Status: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {6}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';


                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           Sale: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {7}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';


                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           Numer of Order: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {20}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';


                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           TimeIn: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {8}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';
                str += '                    <tr>';

                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           TimeOut: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {9}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';
                str += '                    <tr>';


                str += '                    <tr>';
                str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
                str += '                           Duration: ';
                str += '                        </td>';
                str += '                      ';
                str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
                str += '                             {10}';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
                str += '                            &nbsp;';
                str += '                        </td>';
                str += '                      ';
                str += '                    </tr>';
                str += '                    <tr>';


                str += '                </table>';


                var customerDesc = obj.CustomerCode + '-' + obj.CustomerName;
                var customerAddess = obj.CustomerAddress;
                var distributorDesc = obj.DistributorCode + '-' + obj.DistributorName;
                var salesDesc = obj.SalesCode + '-' + obj.SalesName;
                var routeDesc = obj.RouteCode + '-' + obj.RouteName;
                var customerChainCode = obj.CustomerChainCode;

                var customerIsVisit = obj.CustomerIsVisit;
                var customerSalesAmount = obj.CustomerSalesAmount;
                var customerOrders = obj.CustomerOrders;

                var customerTimeIn = obj.TimeIn;
                var customerTimeOut = obj.TimeOut;
                var customerDuration = obj.Duration;


                str = str.replace("{0}", customerDesc);
                str = str.replace("{1}", customerAddess);
                str = str.replace("{2}", distributorDesc);
                str = str.replace("{3}", salesDesc);
                str = str.replace("{4}", customerChainCode);
                str = str.replace("{5}", routeDesc);

                if (customerIsVisit == "1")
                    str = str.replace("{6}", "Visited");
                else
                    str = str.replace("{6}", "No visit");


                str = str.replace("{7}", customerSalesAmount);
                str = str.replace("{20}", customerOrders);
                str = str.replace("{8}", customerTimeIn);
                str = str.replace("{9}", customerTimeOut);
                str = str.replace("{10}", customerDuration + ' (hh:mm:ss) ');


                if (__IEVersion != null && __IEVersion > 0) {
                    obj.openInfoWindow(str);
                }

                else {
                    var infowindow = new google.maps.InfoWindow({
                        content: str
                    });
                    for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                        CTracking.prototype.mInfowindow[i].close();
                    }
                    this.mInfowindow = null;
                    infowindow.open(myMap, mar);
                    CTracking.prototype.mInfowindow.push(infowindow);
                }
            });
            markers.push(mar);

        }
        return markers;
    }


}

//TrackingVisitUnVisit.aspx
function LoadCustomerVisitUnVisit(myMap, jsonCustomer) {

    this.myMap = myMap;
    this.jsonCustomer = jsonCustomer;
    this.IsMarkerShow = true;
    this.show = c;
    this.hide = d;

    this.CustomerMarkers = cmcust(myMap, jsonCustomer);

    init();

    function init()
    {
        CTracking.prototype.mInfowindow = new Array();
        var object2 = new Object();
    }

    function c() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
            this.CustomerMarkers[i].marker.setVisible(true);
        }
        this.IsMarkerShow = true;
    }

    function d() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
            this.CustomerMarkers[i].marker.setVisible(false);
        }
        this.IsMarkerShow = false;
    }


    //for visited
    this.hideVisited = hideVisited;
    this.showVisited = showVisited;
    this.IsVisitedShow = true;

    function hideVisited() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {

            if (this.CustomerMarkers[i].obj.CustomerIsVisit == "1" && this.CustomerMarkers[i].obj.CustomerSalesAmount == "0")
                this.CustomerMarkers[i].marker.setVisible(false);
        }
        this.IsVisitedShow = false;
    }

    function showVisited() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
            if (this.CustomerMarkers[i].obj.CustomerIsVisit == "1" && this.CustomerMarkers[i].obj.CustomerSalesAmount == "0")
                this.CustomerMarkers[i].marker.setVisible(true);
        }
        this.IsVisitedShow = true;
    }

    //for sales
    this.hideSales = hideSales;
    this.showSales = showSales;
    this.IsSalesShow = true;

    function hideSales() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
            if (this.CustomerMarkers[i].obj.CustomerIsVisit == "0" && this.CustomerMarkers[i].obj.CustomerSalesAmount != "0")
                this.CustomerMarkers[i].marker.setVisible(false);
        }
        this.IsSalesShow = false;
    }

    function showSales() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
        
            if (this.CustomerMarkers[i].obj.CustomerIsVisit == "0" && this.CustomerMarkers[i].obj.CustomerSalesAmount != "0")
                this.CustomerMarkers[i].marker.setVisible(true);
        }
        this.IsSalesShow = true;
    }

    //for visited & sales
    this.hideVisitedSales = hideVisitedSales;
    this.showVisitedSales = showVisitedSales;
    this.IsVisitedSalesShow = true;

    function hideVisitedSales() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
      
            if (this.CustomerMarkers[i].obj.CustomerIsVisit == "1" && this.CustomerMarkers[i].obj.CustomerSalesAmount != "0")
                this.CustomerMarkers[i].marker.setVisible(false);
        }
        this.IsVisitedSalesShow = false;
    }

    function showVisitedSales() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
       
            if (this.CustomerMarkers[i].obj.CustomerIsVisit == "1" && this.CustomerMarkers[i].obj.CustomerSalesAmount != "0")
                this.CustomerMarkers[i].marker.setVisible(true);
        }
        this.IsVisitedSalesShow = true;
    }


    //for no visited
    this.hideNoVisited = hideNoVisited;
    this.showNoVisited = showNoVisited;
    this.IsNoVisitedShow = true;

    function hideNoVisited() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
   
            if (this.CustomerMarkers[i].obj.CustomerIsVisit == "0" && this.CustomerMarkers[i].obj.CustomerSalesAmount == "0")
                this.CustomerMarkers[i].marker.setVisible(false);
        }
        this.IsNoVisitedShow = false;
    }

    function showNoVisited() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
          
            if (this.CustomerMarkers[i].obj.CustomerIsVisit == "0" && this.CustomerMarkers[i].obj.CustomerSalesAmount == "0")
                this.CustomerMarkers[i].marker.setVisible(true);
        }
        this.IsNoVisitedShow = true;
    }





    function cmcust(myMap, jsonCustomer) {


        var object = $.parseJSON(jsonCustomer);
        object2 = object;

        var markers = new Array();

        // thay đổi phần icon isvisit TMT
        for (var i = 0; i < object.length; i++) {


            var icon;
            var salesAmount = object[i].CustomerSalesAmount;

            if (object[i].CustomerIsVisit == "1" && salesAmount != "0")
                icon = "icon/SalesAmount_Visit_Outlet.png";
            else
                if (object[i].CustomerIsVisit == "1")
                    icon = "icon/Visited_Outlet.png";
                else
                    if (salesAmount != "0")
                        icon = "icon/SalesAmount_Outlet.png";
                    else
                        icon = "icon/Outlet.png";

            //var lat = object[i].LatLng.split(",");
            //var pt = new google.maps.LatLng(lat[0], lat[1]);
            //var mar = new VMarker(pt, icon, undefined, object[i]);
            //myMap.addOverlay(mar);
            var marker = CreateMarkerShops_VisitUnVisit(eval("new google.maps.LatLng(" + object[i].LatLng + ")"), object[i], icon, myMap, "");
            var MMarket = new Object();
            MMarket.marker = marker;
            MMarket.obj = object[i];
            markers.push(MMarket);
        }
        fitOverlays(myMap, markers)
        return markers;
    }

    function CreateMarkerShops_VisitUnVisit(LatLng, Object, icon, vmap, num) {

        var mar = new google.maps.Marker({
            position: LatLng,
            icon: icon
        });
        mar.setMap(vmap);

        //var mar = new MarkerWithLabel({
        //    position: LatLng,
        //    draggable: true,
        //    raiseOnDrag: true,
        //    map: vmap,
        //    labelContent: num,
        //    labelAnchor: new google.maps.Point(22, 0),
        //    labelClass: "labels", // the CSS class for the label
        //    icon: icon
        //});

        var obj = Object;
        mar.addListener('click', function (obj) {
            obj = Object;
            var str = '';
            str += '       <table style=\"font-family:Tahoma; font-size:11px;width: 375px;text-align:left\" cellpadding=\"0\" cellspacing=\"2\" border=\"0\" width=\"100%\">';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            <h3>Store: {0} </h3>';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top;\">';
            str += '                           <img src="/icon/address.png" />  {1}';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                    <tr>';
            str += '                        <td colspan=\"4\" style=\"white-space: nowrap; width: 375px; vertical-align: top\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                    </tr>';
            str += '                   ';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                            Distributor: ';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {2}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';


            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           DSR: ';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {3}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';


            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Cust type: ';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {4}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';
            str += '                    <tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Route: ';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {5}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';

            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Status: ';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {6}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';


            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                          Sale: ';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {7}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';


            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                          Number of Order: ';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {8}';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 50px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                        <td style=\"white-space: nowrap; width: 1px; vertical-align: top; height: 19px;\">';
            str += '                            &nbsp;';
            str += '                        </td>';
            str += '                      ';
            str += '                    </tr>';

            str += '                </table>';


            var customerDesc = obj.CustomerCode + '-' + obj.CustomerName;
            var customerAddess = obj.CustomerAddress;
            var distributorDesc = obj.DistributorCode + '-' + obj.DistributorName;
            var salesDesc = obj.SalesCode + '-' + obj.SalesName;
            var routeDesc = obj.RouteCode + '-' + obj.RouteName;
            var customerChainCode = obj.CustomerChainCode;
            var customerIsVisit = obj.CustomerIsVisit;


            var salesAmount = obj.CustomerSalesAmount;
            var salesOrders = obj.CustomerOrders;


            str = str.replace("{0}", customerDesc);
            str = str.replace("{1}", customerAddess);
            str = str.replace("{2}", distributorDesc);
            str = str.replace("{3}", salesDesc);
            str = str.replace("{4}", customerChainCode);
            str = str.replace("{5}", routeDesc);
            str = str.replace("{7}", salesAmount);
            str = str.replace("{8}", salesOrders);

            if (customerIsVisit == "1")
                str = str.replace("{6}", "Visited");
            else
                str = str.replace("{6}", "No visit");


            //var infowindow = new google.maps.InfoWindow({
            //    content: str
            //});
            //infowindow.open(vmap, mar);
            var infowindow = new google.maps.InfoWindow({
                content: str
            });
            for (var i = 0; i < CTracking.prototype.mInfowindow.length; i++) {
                CTracking.prototype.mInfowindow[i].close();
            }
            this.mInfowindow = null;
            infowindow.open(myMap, mar);
            CTracking.prototype.mInfowindow.push(infowindow);
        });
        return mar;
    }



}

function fitOverlays(myMap,marker_list)
{
    var bounds = new google.maps.LatLngBounds();
    for (var i = 0; i < marker_list.length; i++) {
        bounds.extend(marker_list[i].marker.getPosition());
    }
    try {
        for (var i = 0; i < CTracking.prototype.mEnd.length; i++) {
            // alert(SalesCD_Point);
            //  if (CTracking.prototype.mEnd[i].obj.SalesCD == myTracking.SalesCD_Point)
            bounds.extend(CTracking.prototype.mEnd[i].marker.getPosition());
        }
    }
    catch (err) {

    }

    myMap.fitBounds(bounds);
}

function fitOverlaysRoute(myMap, obj) {
    var bounds = new google.maps.LatLngBounds();
    var points = obj.getPath().getArray();
    for (var i = 0; i < points.length ; i++) {
        bounds.extend(points[i]);
    }
    myMap.fitBounds(bounds);
}

function calcDistance(p1, p2) {

    return (google.maps.geometry.spherical.computeDistanceBetween(p1, p2)).toFixed(0);
}