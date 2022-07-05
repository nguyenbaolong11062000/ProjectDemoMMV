
//function fitOverlays(myMap, marker_list) {
//    var bounds = new google.maps.LatLngBounds();
//    for (var i = 0; i < marker_list.length; i++) {
//        bounds.extend(marker_list[i].marker.getPosition());
//    }
//    try {
//        for (var i = 0; i < CTracking.prototype.mEnd.length; i++) {
           
//            bounds.extend(CTracking.prototype.mEnd[i].marker.getPosition());
//        }
//    }
//    catch (err) {
//    }
//    myMap.fitBounds(bounds);
//}

function fitOverlaysRoute(myMap, obj) {
    var bounds = new google.maps.LatLngBounds();
    var points = obj.getPath().getArray();
    for (var i = 0; i < points.length; i++) {
        bounds.extend(points[i]);
    }
    myMap.fitBounds(bounds);
}

function calcDistance(p1, p2) {

    return (google.maps.geometry.spherical.computeDistanceBetween(p1, p2)).toFixed(0);
}
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
        if (this.mRouteLine == undefined || this.SalesCD_Route == -1)
            return;
        var check = false;
        for (var k = 0; k < CTracking.prototype.mRouteLine.length; k++) {
            if (CTracking.prototype.mRouteLine[k].SalesCD == this.SalesCD_Route) {
                check = true;
                break;

            }
        }
        if (check == false) {
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
                if (object[i].TypeTracking == "P") {
                    var icon = object[i].ImageUrl;
                    var marker = CreateTrackingPoint(eval("new google.maps.LatLng(" + object[i].LatLngs + ")"), object[i], icon, this.myMap, indexRoute++);
                    if (object[i].SalesCD == this.SalesCD_Point) {
                        marker.setVisible(true);
                    }
                    else {
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
                    if (myTracking.IsStopReplay == true) {
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
                            for (var j = 0; j < CTracking.prototype.mStopReplay.length; j++) {
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
        if (this.IsPointCreated == false) {
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
            var customerAddess = obj.CustomerAddress;
            var distributorDesc = obj.DistributorCode + '-' + obj.DistributorName;
            var salesDesc = obj.SalesName; // obj.SalesCode  + '-' +  obj.SalesName;                         
            var routeDesc = obj.RouteCode + '-' + obj.RouteName;
            var customerChainCode = obj.CustomerChainCode;

            var timeIn = obj.TimeIn;
            var timeOut = obj.TimeOut;
            var duration = obj.Duration;

            var customerAmount = obj.CustomerSalesAmount;
            var customerNoOrders = obj.CustomerOrders;

            //replace chuổi thành các giá trị tương ứng             
            str = str.replace("{0}", customerDesc);
            str = str.replace("{1}", customerAddess);
            str = str.replace("{2}", distributorDesc);
            str = str.replace("{3}", salesDesc);
            str = str.replace("{4}", customerChainCode);
            str = str.replace("{5}", routeDesc);

            str = str.replace("{6}", timeIn);
            str = str.replace("{7}", timeOut);
            str = str.replace("{8}", duration + ' (hh:mm:ss) ');
            str = str.replace("{9}", customerAmount);
            str = str.replace("{10}", customerNoOrders);


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

function LoadCustomerRouteOnMap(myMap, jsonCust) {

    this.myMap = myMap;
    this.jsonCustomer = jsonCust;
    this.IsMarkerShow = true;
    this.show = c;

    this.CustomerMarkers = cmcust(myMap, jsonCust);

    init();

    function init() {
        CTracking.prototype.mInfowindow = new Array();
        var object2 = new Object();
    }

    function c() {
        for (var i = 0; i < this.CustomerMarkers.length; i++) {
            this.CustomerMarkers[i].marker.setVisible(true);
        }
        this.IsMarkerShow = true;
    }


    function cmcust(myMap, jsonCust) {


        var object = $.parseJSON(jsonCust);
        object2 = object;

        var markers = new Array();
        console.log(object);
        // get icon for customer on route TMT
        for (var i = 0; i < object.length; i++) {


            var icon;
            var salesAmount = object[i].CustomerSalesAmount;
            //set image store
           
            if (object[i].status == 0)
                icon = "icon/ic_store_red.png";
            else 
                icon = "icon/ic_store_blue.png";

            var custMarker = new google.maps.MarkerImage(
                icon,
                new google.maps.Size(30, 30), //size
                null, //origin
                null, //anchor
                new google.maps.Size(30, 30) //scale
            );

            var marker = CreateMarkerShops_RouteOnMap(eval("new google.maps.LatLng(" + object[i].LatLng + ")"), object[i], custMarker, myMap, "");
            var MMarket = new Object();
            MMarket.marker = marker;
            MMarket.obj = object[i];
            markers.push(MMarket);
        }
        return markers;
    }

    

    function CreateMarkerShops_RouteOnMap(LatLng, Object, icon, vmap, num) {

        var mar = new google.maps.Marker({
            position: LatLng,
            icon: icon
        });
        mar.setMap(vmap);
        arrMarker.push(mar);
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

            //----Set cứng các dữ liệu mới demo
            str += '                    <tr>';
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Mtd: ';
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
            str += '                           2M: ';
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
            str += '                           3M: ';
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
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Last visit: ';
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
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Last order value: ';
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
            str += '                        <td style=\"white-space: nowrap; width: 166px; vertical-align: top; height: 19px; padding-left:15px\">';
            str += '                           Last order date: ';
            str += '                        </td>';
            str += '                      ';
            str += '                         <td style=\"white-space: nowrap; width: 154px; vertical-align: top; height: 19px;\">';
            str += '                             {11}';
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

            var customerMTD = obj.customerMTD;
            var customer2M = obj.customer2M;
            var customer3M = obj.customer3M;
            var lastVistDate = obj.lastVistDate;
            var lastOrderdate = obj.lastOrderdate;
            var lastOrderValue = obj.lastOrderValue;


            str = str.replace("{0}", customerDesc);
            str = str.replace("{1}", customerAddess);
            str = str.replace("{2}", distributorDesc);
            str = str.replace("{3}", salesDesc);
            str = str.replace("{4}", customerChainCode);
            str = str.replace("{5}", routeDesc);

            str = str.replace("{6}", customerMTD);
            str = str.replace("{7}", customer2M);
            str = str.replace("{8}", customer3M);
            str = str.replace("{9}", lastVistDate);
            str = str.replace("{10}", lastOrderValue);
            str = str.replace("{11}", lastOrderdate);
           




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

function GetPosition(myMap, LongLat) {
    var geocode = LongLat.split(",");
    var Loglat = new google.maps.LatLng(geocode[0], geocode[1])
    var mar = new google.maps.Marker({
        position: LongLat,
    });
}