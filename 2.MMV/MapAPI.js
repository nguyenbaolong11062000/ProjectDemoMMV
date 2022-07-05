function VLatLng(j, a, h) {
    this.latitude = j;
    this.longitude = a;
    this.l1Ii = h;
    this.copy = b;
    this.lat = i;
    this.lng = k;
    this.latRadians = e;
    this.lngRadians = g;
    this.equals = d;
    this.distanceFrom = f;
    this.toUrlValue = m;
    this.toString = c;
    this.vType = l;

    function b() {
        return new VLatLng(this.latitude, this.longitude)
    }

    function i() {
        return this.latitude
    }

    function k() {
        return this.longitude
    }

    function e() { }

    function g() { }

    function d(n) {
        if (this.latitude == n.latitude && this.longitude == n.longitude) {
            return true
        } else {
            return false
        }
    }

    function f(n) { }

    function m() {
        return this.latitude.toString().substring(0, 10) + "," + this.longitude.toString().substring(0, 10)
    }

    function c() {
        return this.latitude.toString().substring(0, 10) + "," + this.longitude.toString().substring(0, 10)
    }

    function l() {
        return "VLatLng"
    }
}

function VSize(b, a) {
    this.width = b;
    this.height = a;
    this.equals = d;
    this.toString = e;
    this.vType = c;

    function d(f) {
        if (this.width == f.width && this.height == f.height) {
            return true
        } else {
            return false
        }
    }

    function e() {
        return (this.width.toString() + "," + this.height.toString())
    }

    function c() {
        return "VSize"
    }
}

function VControl() {
    this.initialize = c;
    this.redraw = g;
    this.remove = b;
    this.getDefaultPosition = a;
    this.hide = e;
    this.show = d;
    this.vType = f;

    function c(i, h) { }

    function g() { }

    function b() { }

    function a() { }

    function e() { }

    function d() { }

    function f() { }
}

function VControlPosition(a, b) {
    this.anchor = (a != undefined) ? a : V_ANCHOR_TOP_LEFT;
    this.offset = (b != undefined) ? b : new VSize(0, 0);
    this.vType = function () {
        return "VControlPosition"
    }
}

function VLatLngBounds(a, g) {
    this.nw = a;
    this.se = g;
    this.equals = e;
    this.contains = b;
    this.getNorthWest = f;
    this.getSouthEast = c;
    this.vType = d;

    function e(h) {
        if (this.nw.equals(h.nw) && this.se.equals(h.se)) {
            return true
        } else {
            return false
        }
    }

    function b(h) { }

    function f() {
        return a
    }

    function c() {
        return g
    }

    function d() {
        return "VLatLngBounds"
    }
}

function VDirectionsOptions(e, b, d, a, c) {
    this.getPolyline = e;
    this.jj1 = b;
    this.preserveViewport = d;
    this.avoidAlley = (a == undefined) ? true : a;
    this.avoidRoadConstruction = (c == undefined) ? false : c;
    this.vType = function () {
        return "VDirectionsOptions"
    }
}

function VInfoWindowOptions(f, j, k, h, a, b, e, g, d, i, c) {
    this.offset = (f != undefined) ? f : new VSize(0, 0);
    this.selectedTab = (j != undefined) ? j : 0;
    this.maxWidth = (k != undefined) ? k : 0;
    this.onOpenFn = (h != undefined) ? h : null;
    this.onCloseFn = (a != undefined) ? a : null;
    this.llll = (b != undefined) ? b : 0;
    this.maxContent = (e != undefined) ? e : "";
    this.maxTitle = (g != undefined) ? g : "";
    this.autoPan = (d != undefined) ? d : true;
    this.mapBlowup = (i != undefined) ? i : false;
    this.maximizeWhenOpen = (c != undefined) ? c : false;
    this.vType = function () {
        return "VInfoWindowOptions"
    }
}

function VInfoWindowTab(a, b) {
    this.label = (a != undefined) ? a : "";
    this.content = (b != undefined) ? b : "&nbsp;";
    this.vType = function () {
        return "VInfoWindowTab"
    }
}

function VCopyrightCollection(b) {
    this.prefix = b;
    this.vType = a;

    function a() {
        return "VCopyrightCollection"
    }
}

function VTileLayer(c, b, a, e) {
    this.copyrights = c;
    this.minResolution = b;
    this.maxResolution = a;
    this.opts = e;
    this.getTileUrl = f;
    this.vType = d;

    function f() { }

    function d() {
        return "VTileLayer"
    }
}

function VMapOptions(d, b, c, a) {
    this.skin = (d != undefined) ? d : "WhiteSkin";
    this.size = b;
    this.draggableCursor = (c != undefined) ? c : strResURL + "images/openhand.cur";
    this.draggingCursor = (a != undefined) ? a : strResURL + "images/closedhand.cur";
    this.vType = function () {
        return "VMapOptions"
    }
}

function VMapType(h, a, c, g) {
    this.layers = h;
    this.projection = a;
    this.name = c;
    this.opts = (g == undefined) ? {
        degree360: true
    } : g;
    this.getProjection = d;
    this.getTileLayers = e;
    this.setOptions = b;
    this.vType = f;

    function d() {
        return this.projection
    }

    function e() {
        return this.layers
    }

    function b(i) {
        this.opts = (i == undefined) ? {
            degree360: true
        } : i
    }

    function f() {
        return "VMapType"
    }
}

function VMappingLayer(b) {
    this.mappedTileLayer = (b != undefined) ? b : 0;
    this.arrEvents = new Array();
    this.arrHandlers = new Array();
    this.addListener = a;
    this.getMappingAreas = d;
    this.vType = c;

    function a(e, f) {
        this.arrEvents[this.arrEvents.length] = e;
        this.arrHandlers[this.arrHandlers.length] = f
    }

    function d() { }

    function c() {
        return "VMappingLayer"
    }
}

function VMappingArea(c, d, a) {
    this.coords = (c != undefined) ? c : "";
    this.title = (d != undefined) ? d : "";
    this.shape = (a != undefined) ? a : "poly";
    this.vType = b;

    function b() {
        return "VMappingArea"
    }
}

function VMarkerOptions(p, a, d, e, t, l, c, f, b, h, r) {
    this.icon = (p != undefined) ? p : new VIcon();
    var q = (a != undefined) ? a : "";
    var o = new Array();
    if (typeof q == "string") {
        o[0] = q
    } else {
        o = q
    }
    for (var n = 0; n < o.length; n++) {
        if (o[n] != "") {
            while (o[n].indexOf(" ") != -1) {
                o[n] = o[n].replace(" ", "&nbsp;")
            }
        }
    }
    this.jll = o;
    var g = (d != undefined) ? d : new VTextPosition();
    var m = new Array();
    if (g.length == undefined) {
        m[0] = g
    } else {
        m = g
    }
    this.textPosition = m;
    var j = (e != undefined) ? e : new VTextStyle();
    var s = new Array();
    if (j.length == undefined) {
        s[0] = j
    } else {
        s = j
    }
    this.textStyle = s;
    var k = (this.jll[0] == "") ? 0 : this.jll.length;
    if (this.textPosition.length < k) {
        k = this.textPosition.length
    }
    if (this.textStyle.length < k) {
        k = this.textStyle.length
    }
    this.nText = k;
    this.title = (t != undefined) ? t : "";
    this.clickable = (l != undefined) ? l : true;
    this.draggable = (c != undefined) ? c : false;
    this.offset = (f != undefined) ? f : null;
    this.autoPan = (b != undefined) ? b : true;
    this.zIndexProcess = (h != undefined) ? h : null;
    this.customMarker = r;
    this.vType = function () {
        return "VMarkerOptions"
    }
}

function VPolygonOptions(a, b) {
    this.clickable = (a == null) ? true : a;
    this.title = (b != undefined) ? b : "";
    this.vType = function () {
        return "VPolygonOptions"
    }
}

function VTextPosition(a, b) {
    this.anchor = (a != undefined) ? a : V_ANCHOR_BOTTOM_CENTER;
    this.offset = (b != undefined) ? b : new VSize(0, 0);
    this.vType = function () {
        return "VTextPosition"
    }
}

function VTextStyle(d, c, b, a,f) {
    this.fontSize = (d != undefined) ? d : 20;
    this.fontWeight = (c != undefined) ? c : "normal";
    this.color = (b != undefined) ? b : "#ff0000";
    this.fontFamily = (a != undefined) ? a : "Arial";
    this.paddingLeft = (f != undefined) ? f : "0px";
    this.vType = function () {
        return "VTextStyle"
    }
}

function VPolylineOptions(a, b, c, d) {
    this.clickable = (a == undefined) ? true : a;
    this.geodesic = (b == undefined) ? false : b;
    this.dashStyle = (c == undefined) ? "" : c;
    this.title = (d != undefined) ? d : "";
    this.vType = function () {
        return "VPolylineOptions"
    }
}

function VCircleOptions(a, c, d, b, e) {
    this.clickable = (a == undefined) ? true : a;
    this.geodesic = (c == undefined) ? false : c;
    this.dashStyle = (d == undefined) ? "" : d;
    this.showInLowLevel = (b == undefined) ? true : b;
    this.title = (e != undefined) ? e : "";
    this.vType = function () {
        return "VCircleOptions"
    }
}

function vcallbackFunction(c) {
    if (VUtil.isVBDWebsite()) {
        return
    }
    if (c.x != true || c.y != true) {
        jlj = false;
        isValidKey = false;
        jii.disableAll()
    } else {
        var b, a = false;
        for (b = 0; b < document.images.length; b++) {
            if (document.images[b].src == strResURL + "images/poweredby.png") {
                a = true;
                break
            }
        }
        if (!a && !window.opera) {
            jlj = false;
            isValidKey = false;
            jii.disableAll();
            return
        }
        jlj = true
    }
}
VUtil = {
    initVML: false,
    getBrowserName: function () {
        var b = "";
        var a = navigator.userAgent.toLowerCase();
        if (a.indexOf("iphone") != -1) {
            b = "iPhone"
        } else {
            if (a.indexOf("ipod") != -1) {
                b = "iPod"
            } else {
                if (a.indexOf("ipad") != -1) {
                    b = "iPad"
                } else {
                    if (a.indexOf("opera") != -1) {
                        b = "opera"
                    } else {
                        if (a.indexOf("msie") != -1) {
                            b = "msie"
                        } else {
                            if (a.indexOf("chrome") != -1) {
                                b = "chrome"
                            } else {
                                if (a.indexOf("safari") != -1) {
                                    b = "safari"
                                } else {
                                    if (a.indexOf("mozilla") != -1) {
                                        if (a.indexOf("firefox") != -1) {
                                            b = "firefox"
                                        } else {
                                            b = "mozilla"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return b
    },
    isIE6Browser: function () {
        return (/msie|MSIE 6/.test(navigator.userAgent))
    },
    getIEVersion: function () {
        var c = -1;
        if (navigator.appName == "Microsoft Internet Explorer") {
            var a = navigator.userAgent;
            var b = new RegExp("MSIE ([0-9]{1,}[.0-9]{0,})");
            if (b.exec(a) != null) {
                c = parseFloat(RegExp.$1)
            }
        }
        return c
    },
    getOperaVersion: function () {
        var b = 0;
        if (window.opera) {
            var a = window.opera.version();
            b = parseFloat(a)
        }
        return b
    },
    parseBorderWidth: function (b) {
        var a = 0;
        if (typeof (b) == "string" && b != null && b != "") {
            var c = b.indexOf("px");
            if (c >= 0) {
                a = parseInt(b.substring(0, c))
            } else {
                a = 1
            }
        }
        return a
    },
    getBorderWidth: function (c) {
        var b = new Object();
        b.left = 0;
        b.top = 0;
        b.right = 0;
        b.bottom = 0;
        if (window.getComputedStyle) {
            var a = window.getComputedStyle(c, null);
            b.left = parseInt(a.borderLeftWidth.slice(0, -2));
            b.top = parseInt(a.borderTopWidth.slice(0, -2));
            b.right = parseInt(a.borderRightWidth.slice(0, -2));
            b.bottom = parseInt(a.borderBottomWidth.slice(0, -2))
        } else {
            b.left = VUtil.parseBorderWidth(c.style.borderLeftWidth);
            b.top = VUtil.parseBorderWidth(c.style.borderTopWidth);
            b.right = VUtil.parseBorderWidth(c.style.borderRightWidth);
            b.bottom = VUtil.parseBorderWidth(c.style.borderBottomWidth)
        }
        return b
    },
    isVBDWebsite: function () {
        if (((window.location.href.indexOf("http://www.vietbando.com") != -1) || (window.location.href.indexOf("http://local.vietbando.com") != -1) || (window.location.href.indexOf("http://maps.vietbando.com") != -1) || (window.location.href.indexOf("http://image3.vietbando.com") != -1) || (window.location.href.indexOf("http://image4.vietbando.com") != -1) || (window.location.href.indexOf("http://ics.vietbando.com") != -1) || (window.location.href.indexOf("http://mapvn.vietbando.com") != -1) || (window.location.href.indexOf("http://tracking.vietbando.com") != -1) || (window.location.href.indexOf("http://forum.vietbando.com") != -1) || (window.location.href.indexOf("http://www.vietbando.net") != -1) || (window.location.href.indexOf("http://www.vietbando.com.vn") != -1) || (window.location.href.indexOf("http://www.vietbando.vn") != -1) || (window.location.href.indexOf("http://vietbando.com") != -1) || (window.location.href.indexOf("http://vietbando.net") != -1) || (window.location.href.indexOf("http://vietbando.com.vn") != -1) || (window.location.href.indexOf("http://quanlygps.com") != -1) || (window.location.href.indexOf("http://viendongmap.com") != -1) || (window.location.href.indexOf("http://vietbando.vn") != -1))) {
            return true
        }
    },
    addVMLStyle: function () {
        if (!this.initVML && document.readyState == "complete") {
            if (!document.namespaces.v) {
                document.namespaces.add("v", "urn:schemas-microsoft-com:vml")
            }
            var a = document.createStyleSheet();
            a.addRule("v\\:shape", "behavior:url(#default#VML); display:inline-block; antialias:true");
            a.addRule("v\\:shape", "behavior:url(#default#VML); display:inline-block; antialias:true");
            a.addRule("v\\:group", "behavior:url(#default#VML); display:inline-block; antialias:true");
            a.addRule("v\\:polyline", "behavior:url(#default#VML); display:inline-block; antialias:true");
            a.addRule("v\\:stroke", "behavior:url(#default#VML); display:inline-block; antialias:true");
            a.addRule("v\\:fill", "behavior:url(#default#VML); display:inline-block; antialias:true");
            a.addRule("v\\:rect", "behavior:url(#default#VML); display:inline-block; antialias:true");
            a.addRule("v\\:oval", "behavior:url(#default#VML); display:inline-block; antialias:true");
            this.initVML = true
        }
    },
    trim: function (a) {
        return a.replace(/^\s\s*/, "").replace(/\s\s*$/, "")
    },
    fixPNG: function () {
        if (VUtil.isIE6Browser()) {
            for (var g = 0; g < document.images.length; g++) {
                var d = document.images[g];
                var f = d.src.toUpperCase();
                if (f.substring(f.length - 3, f.length) == "PNG" && d.className == "transimg") {
                    var b = (d.id) ? "id='" + d.id + "' " : "";
                    var e = (d.className) ? "class='" + d.className + "' " : "";
                    var h = (d.title) ? "title='" + d.title + "' " : "title='" + d.alt + "' ";
                    var c = "display:inline-block;" + d.style.cssText;
                    if (d.align == "left") {
                        c = "float:left;" + c
                    }
                    if (d.align == "right") {
                        c = "float:right;" + c
                    }
                    if (d.parentElement.href) {
                        c = "cursor:hand;" + c
                    }
                    var a = "<span " + b + e + h + ' style="width:' + d.width + "px; height:" + d.height + "px;" + c + ";filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + d.src + "', sizingMethod='scale');\"></span>";
                    d.outerHTML = a;
                    g = g - 1
                }
            }
        }
    },
    l1Il: function (b) {
        var a = b.toString().toLowerCase();
        if (a.indexOf("<") != -1 || a.indexOf(">") != -1 || a.indexOf("script") != -1) {
            return false
        }
        return true
    },
    bubbleEventObject: function (c, b) {
        var a = (browserName != "msie") ? "HTML" : "BODY";
        var d = (browserName != "msie") ? c.target : event.srcElement;
        while (d.tagName != "path" && d.tagName != a && (d.className.indexOf != undefined && d.className.indexOf("dragruler") == -1 && d.className != "overviewmap" && d.className != "dragoverview" && d.className != "dragmarker" && d.className != "clickmarker" && d.className != "dragline" && d.className != "dragtext" && d.className.indexOf("clickcontrol") == -1 && d.className != "nomousemove")) {
            d = (browserName != "msie") ? d.parentNode : d.parentElement;
            if (d == null) {
                return false
            }
        }
        b[0] = (d.tagName != a && d.tagName != "svg");
        b[1] = d
    },
    isVObject: function (b) {
        var a = (browserName != "msie") ? "HTML" : "BODY";
        var c = (browserName != "msie") ? b.target : b.srcElement;
        if (c.className == undefined) {
            return false
        }
        while (c.tagName != a && (c.className.indexOf != undefined && c.className.indexOf("dragruler") == -1 && c.className != "overviewmap" && c.className != "dragoverview" && c.className != "dragmarker" && c.className.indexOf("clickcontrol") == -1 && c.className != "nomousemove")) {
            c = (browserName != "msie") ? c.parentNode : c.parentElement;
            if (c == null) {
                return false
            }
        }
        if (c.tagName != a && c.tagName != "svg") {
            return true
        }
        return false
    },
    isMapAreaObject: function (b) {
        var a = (browserName != "msie") ? "HTML" : "BODY";
        var c = (browserName != "msie") ? b.target : event.srcElement;
        if (c.className == undefined) {
            return false
        }
        while (c.tagName != a && c.className != "dragmaparea") {
            c = (browserName != "msie") ? c.parentNode : c.parentElement;
            if (c == null) {
                return false
            }
        }
        if (c.tagName != a && c.tagName != "svg") {
            return true
        }
        return false
    },
    createDivElement: function (a, c, g, b, h, f, d, i) {
        var e;
        if (a != "") {
            e = document.getElementById(a);
            if (e == null) {
                e = document.createElement("div");
                e.id = a
            }
        } else {
            e = document.createElement("div")
        } if (f != null) {
            e.style.position = f
        }
        if (d != null) {
            e.style.overflow = d
        }
        if (c != null && !isNaN(c) && c != "") {
            c += "px"
        }
        if (g != null && !isNaN(g) && g != "") {
            g += "px"
        }
        if (b != null && !isNaN(b) && b != "") {
            b += "px"
        }
        if (h != null && !isNaN(h) && h != "") {
            h += "px"
        }
        if (c != null) {
            e.style.left = c
        }
        if (g != null) {
            e.style.top = g
        }
        if (b != null) {
            e.style.width = b
        }
        if (h != null) {
            e.style.height = h
        }
        e.innerHTML = i;
        return e
    },
    removeDivElement: function (d, b) {
        try {
            var a = document.getElementById(d);
            if (a != null) {
                if (b == null) {
                    document.removeChild(a)
                } else {
                    document.getElementById(b).removeChild(a)
                }
            }
        } catch (c) { }
    },
    jl1: function (c, e, b, d) {
        if (c.indexOf("?") > -1) {
            c += "&jsonp="
        } else {
            c += "?jsonp="
        }
        c += b + "&";
        if (d) {
            c += encodeURIComponent(d) + "&"
        }
        c += new Date().getTime().toString();
        c += "&key=" + ll1i;
        c += "&location=" + window.location;
        var a = document.createElement("script");
        a.setAttribute("src", c);
        a.setAttribute("type", "text/javascript");
        document.body.appendChild(a)
    },
    removeLeftColumn: function (a) {
        var c, b = document.getElementById(a).rows;
        for (c = 0; c < b.length; c++) {
            if (b[c].cells.length > 1) {
                b[c].deleteCell(-1)
            }
        }
    },
    removeRightColumn: function (a) {
        var c, b = document.getElementById(a).rows;
        for (c = 0; c < b.length; c++) {
            if (b[c].cells.length > 1) {
                b[c].deleteCell(0)
            }
        }
    },
    OOP_ClipInside: function (p, f, e, n, a) {
        var c = new Array();
        var h, d, m, b, l;
        var o, g = p.length;
        var k = new OOP_ClipObj();
        var j = 0;
        while (j < g - 1) {
            d = p[j];
            m = p[j + 1];
            b = p[j + 2];
            l = p[j + 3];
            h = this.OOP_LineClip(d, m, b, l, f, e, n, a, k);
            if (h) {
                break
            }
            j += 2
        }
        if (!h) {
            return c
        }
        o = new Array();
        for (; j < g - 1; j += 2) {
            d = p[j];
            m = p[j + 1];
            b = p[j + 2];
            l = p[j + 3];
            h = this.OOP_LineClip(d, m, b, l, f, e, n, a, k);
            if (h) {
                o.push(k.x1);
                o.push(k.y1);
                if (this.OOP_IsInside(p[j + 2], p[j + 3], f, e, n, a)) {
                    continue
                } else {
                    o.push(k.x2);
                    o.push(k.y2);
                    c.push(o);
                    o = new Array()
                }
            }
        }
        if (this.OOP_IsInside(p[g - 2], p[g - 1], f, e, n, a)) {
            o.push(p[g - 2]);
            o.push(p[g - 1])
        }
        if (o.length != 0) {
            c.push(o)
        }
        return c
    },
    OOP_LineClip: function (d, k, b, j, h, f, l, a, i) {
        var e = 0,
            c = 1;
        var n = b - d,
            m;
        var g = new OOP_ClipObj();
        if (this.OOP_ClipTest(-n, d - h, e, c, g)) {
            if (this.OOP_ClipTest(n, l - d, g.x1, g.y1, g)) {
                m = j - k;
                if (this.OOP_ClipTest(-m, k - f, g.x1, g.y1, g)) {
                    if (this.OOP_ClipTest(m, a - k, g.x1, g.y1, g)) {
                        e = parseFloat(g.x1);
                        c = parseFloat(g.y1);
                        if (c < 1) {
                            b = parseInt(d) + Math.round(c * n);
                            j = parseInt(k) + Math.round(c * m)
                        }
                        if (e > 0) {
                            d = parseInt(d) + Math.round(e * n);
                            k = parseInt(k) + Math.round(e * m)
                        }
                        i.x1 = d;
                        i.x2 = b;
                        i.y1 = k;
                        i.y2 = j;
                        return true
                    }
                }
            }
        }
        return false
    },
    OOP_ClipTest: function (g, d, f, c, a) {
        var b, e = true;
        if (g < 0) {
            b = parseFloat(d / g);
            if (b > c) {
                e = false
            } else {
                if (b > f) {
                    f = b
                }
            }
        } else {
            if (g > 0) {
                b = parseFloat(d / g);
                if (b < f) {
                    e = false
                } else {
                    if (b < c) {
                        c = b
                    }
                }
            } else {
                if (d < 0) {
                    e = false
                }
            }
        }
        a.x1 = f;
        a.y1 = c;
        return e
    },
    OOP_IsInside: function (b, f, e, a, d, c) {
        if (b < e || b > d) {
            return false
        }
        if (f < a || f > c) {
            return false
        }
        return true
    },
    setVColorOver: function (a) {
        a.bgColor = "#e6f5fe"
    },
    setVColorOut: function (a) {
        a.bgColor = "#ffffff"
    },
    createZoomAnimation: function (h, a, g) {
        VUtil.removeZoomAnimation();
        var f = 0;
        if (h > 0) {
            f = 10
        } else {
            f = 46
        }
        var d = VUtil.createDivElement("vbdani_lefttop", a - f - 5, g - f - 5, 10, 8, "absolute", "hidden", "");
        d.style.border = 0;
        d.style.zIndex = TopZIndex;
        if (h > 0) {
            d.style.borderLeft = "2px red solid";
            d.style.borderTop = "2px red solid"
        } else {
            d.style.borderRight = "2px red solid";
            d.style.borderBottom = "2px red solid"
        }
        document.body.appendChild(d);
        var e = VUtil.createDivElement("vbdani_righttop", a + f - 5, g - f - 5, 10, 8, "absolute", "hidden", "");
        e.style.border = 0;
        e.style.zIndex = TopZIndex;
        if (h > 0) {
            e.style.borderRight = "2px red solid";
            e.style.borderTop = "2px red solid"
        } else {
            e.style.borderLeft = "2px red solid";
            e.style.borderBottom = "2px red solid"
        }
        document.body.appendChild(e);
        var b = VUtil.createDivElement("vbdani_leftbottom", a - f - 5, g + f - 5, 10, 8, "absolute", "hidden", "");
        b.style.border = 0;
        b.style.zIndex = TopZIndex;
        if (h > 0) {
            b.style.borderLeft = "2px red solid";
            b.style.borderBottom = "2px red solid"
        } else {
            b.style.borderRight = "2px red solid";
            b.style.borderTop = "2px red solid"
        }
        document.body.appendChild(b);
        var c = VUtil.createDivElement("vbdani_rightbottom", a + f - 5, g + f - 5, 10, 8, "absolute", "hidden", "");
        c.style.border = 0;
        c.style.zIndex = TopZIndex;
        if (h > 0) {
            c.style.borderRight = "2px red solid";
            c.style.borderBottom = "2px red solid"
        } else {
            c.style.borderLeft = "2px red solid";
            c.style.borderTop = "2px red solid"
        }
        document.body.appendChild(c)
    },
    removeZoomAnimation: function () {
        var c = document.getElementById("vbdani_lefttop");
        var d = document.getElementById("vbdani_righttop");
        var a = document.getElementById("vbdani_leftbottom");
        var b = document.getElementById("vbdani_rightbottom");
        if (c != null) {
            document.body.removeChild(c);
            document.body.removeChild(d);
            document.body.removeChild(a);
            document.body.removeChild(b)
        }
    },
    Distance: function (b, e, a, c) {
        var f = (a - b) * (a - b) + (c - e) * (c - e);
        if (f == 0) {
            return 0
        }
        return f
    },
    ProjectFactor: function (g, d, b, f, a, e) {
        if (g == b && d == f) {
            return 0
        }
        if (g == a && d == e) {
            return 1
        }
        var i, h, c;
        i = a - b;
        h = e - f;
        c = i * i + h * h;
        return ((g - b) * i + (d - f) * h) / c
    },
    ProjectPoint: function (a, h, c, e, b, d) {
        var g = new VLatLng(0, 0);
        var f;
        if (a == c && h == e) {
            g.longitude = a;
            g.latitude = h;
            return g
        }
        if (a == b && h == d) {
            g.longitude = a;
            g.latitude = h;
            return g
        }
        f = this.ProjectFactor(a, h, c, e, b, d);
        g.longitude = c + f * (b - c);
        g.latitude = e + f * (d - e);
        return g
    },
    ClosestPoint: function (k, h, e, j, c, i) {
        var d = new VLatLng(0, 0);
        var b, a, g = this.ProjectFactor(k, h, e, j, c, i);
        if (g > 0 && g < 1) {
            return this.ProjectPoint(k, h, e, j, c, i)
        }
        b = this.Distance(k, h, e, j);
        a = this.Distance(k, h, c, i);
        if (b < a) {
            d.longitude = e;
            d.latitude = j;
            return d
        }
        d.longitude = c;
        d.latitude = i;
        return d
    },
    ClosestPointPolyline: function (p, m, e, o) {
        var d = 0;
        var c, n, a, l;
        var k = new VLatLng(0, 0);
        var b = new VLatLng(0, 0);
        var j, g;
        var f, h = e.length;
        c = e[0].longitude;
        n = e[0].latitude;
        a = e[1].longitude;
        l = e[1].latitude;
        b = this.ClosestPoint(p, m, c, n, a, l);
        g = this.Distance(p, m, b.longitude, b.latitude);
        for (f = 1; f < h - 1; f++) {
            c = e[f].longitude;
            n = e[f].latitude;
            a = e[f + 1].longitude;
            l = e[f + 1].latitude;
            k = this.ClosestPoint(p, m, c, n, a, l);
            j = this.Distance(p, m, k.longitude, k.latitude);
            if (j < g) {
                g = j;
                b = k;
                d = f
            }
        }
        o[0] = b;
        o[1] = d
    },
    ClosestPointPolylines: function (l, h, j, k) {
        var e = 0,
            f = 0;
        var a = new VLatLng(0, 0);
        var g, m = null;
        var c, d, b;
        for (g = 0; g < j.length; g++) {
            c = j[g];
            d = new Array();
            VUtil.ClosestPointPolyline(l, h, c, d);
            if (m == null) {
                m = VUtil.Distance(l, h, d[0].x, d[0].y);
                a = d[0]
            } else {
                b = VUtil.Distance(l, h, d[0].x, d[0].y);
                if (m > b) {
                    m = b;
                    a = d[0];
                    e = f + d[1]
                }
            }
            f += j[g].length
        }
        k[0] = a;
        k[1] = e
    },
    isInsidePolygon: function (c, e) {
        var a = false;
        var b, d = e.length;
        for (b = 0; b < d - 1; b++) {
            if ((((e[b + 1].latitude <= c.latitude) && (c.latitude < e[b].latitude)) || ((e[b].latitude <= c.latitude) && (c.latitude < e[b + 1].latitude))) && (c.longitude < (e[b].longitude - e[b + 1].longitude) * (c.latitude - e[b + 1].latitude) / (e[b].latitude - e[b + 1].latitude) + e[b + 1].longitude)) {
                a = !a
            }
        }
        return a
    },
    getCenterPoint: function (f) {
        var e = f.length;
        var d = 0;
        var a;
        for (a = 0; a < e - 1; a++) {
            d += (f[a].latitude + f[a + 1].latitude) * (f[a].longitude - f[a + 1].longitude)
        }
        d *= 3;
        var b = 0;
        var c = 0;
        for (a = 0; a < e - 1; a++) {
            b += (f[a].longitude + f[a + 1].longitude) * (f[a].longitude * f[a + 1].latitude - f[a + 1].longitude * f[a].latitude);
            c += (f[a].latitude + f[a + 1].latitude) * (f[a].longitude * f[a + 1].latitude - f[a + 1].longitude * f[a].latitude)
        }
        b = b / d;
        c = c / d;
        return new VLatLng(c, b)
    },
    VLength2: function (a) {
        return a.dx * a.dx + a.dy * a.dy
    },
    SimplifyDP: function (q, n, s, r, x) {
        if (r <= s + 1) {
            return
        }
        var t, p = s;
        var a = 0;
        var h = q * q;
        var g = new Segment();
        var o = new Vector();
        var m = new Vector();
        var c = new Vector();
        var d = new VPoint();
        var y, f, e, l;
        g.p1 = n[s];
        g.p2 = n[r];
        o.dx = g.p2.x - g.p1.x;
        o.dy = g.p2.y - g.p1.y;
        f = o.dx * o.dx + o.dy * o.dy;
        for (t = s + 1; t < r; t++) {
            m.dx = n[t].x - g.p1.x;
            m.dy = n[t].y - g.p1.y;
            e = m.dx * o.dx + m.dy * o.dy;
            if (e <= 0) {
                c.dx = n[t].x - g.p1.x;
                c.dy = n[t].y - g.p1.y;
                l = this.VLength2(c)
            } else {
                if (f <= e) {
                    c.dx = n[t].x - g.p2.x;
                    c.dy = n[t].y - g.p2.y;
                    l = this.VLength2(c)
                } else {
                    y = e / f;
                    d.x = g.p1.x + y * o.dx;
                    d.y = g.p1.y + y * o.dy;
                    c.dx = n[t].x - d.x;
                    c.dy = n[t].y - d.y;
                    l = this.VLength2(c)
                }
            } if (l <= a) {
                continue
            }
            p = t;
            a = l
        }
        if (a > h) {
            x[p] = 1;
            this.SimplifyDP(q, n, s, p, x);
            this.SimplifyDP(q, n, p, r, x)
        }
    },
    Simplify: function (p, j, b, c) {
        var g, e, d, o;
        var a = p * p;
        var l = new Array();
        var h = new Vector();
        var f = new Array();
        for (g = 0; g < b; g++) {
            f[g] = 0
        }
        l[0] = j[0];
        e = 1;
        o = 0;
        for (g = 1; g < b; g++) {
            h.dx = j[g].x - j[o].x;
            h.dy = j[g].y - j[o].y;
            if (this.VLength2(h) < a) {
                continue
            }
            l[e++] = j[g];
            o = g
        }
        if (o < b - 1) {
            l[e++] = j[b - 1]
        }
        f[0] = f[e - 1] = 1;
        this.SimplifyDP(p, l, 0, e - 1, f);
        d = 0;
        for (g = 0; g < e; g++) {
            if (f[g]) {
                c[d++] = l[g]
            }
        }
        return d
    },
    Reduction: function (c, d, f) {
        var g = d.length;
        var b = new Array();
        var e, a;
        if (g == 2) {
            return
        }
        a = this.Simplify(c, d, g, b);
        for (e = 0; e < a; e++) {
            f[e] = b[e]
        }
    },
    includeCSS: function (a) {
        var b = document.createElement("link");
        b.rel = "stylesheet";
        b.type = "text/css";
        b.href = strResURL + a;
        document.getElementsByTagName("head")[0].appendChild(b)
    },
    vType: function () {
        return "VUtil"
    }
};

function OOP_ClipObj() {
    this.x1 = 0;
    this.y1 = 0;
    this.x2 = 0;
    this.y2 = 0
}

function Vector() {
    this.dx = 0;
    this.dy = 0
}

function Segment() {
    this.p1 = new VPoint();
    this.p2 = new VPoint()
}

function initializeTimer() {
    CountRequest++;
    stopTheClock();
    g_REtimerID = setTimeout(startTheTimer, g_REdelay)
}

function stopTheClock() {
    if (g_REtimerID != null) {
        clearTimeout(g_REtimerID);
        g_REtimerID = null
    }
}

function startTheTimer() {
    stopTheClock();
    VUtil.jl1(j1l + "?NumRequest=" + CountRequest, "", "", "");
    CountRequest = 0
}

function VOverlay() {
    this.getZIndex = d;
    this.initialize = b;
    this.remove = a;
    this.copy = f;
    this.redraw = e;

    function d(g) { }

    function b(g) { }

    function a() { }

    function f() { }

    function e(g) { }

    function c() { }
}
VEvent = {
    addListener: function (a, g, h) {
        switch (a.vType()) {
            case "VMap":
                var j = document.getElementById(a.id);
                if (g == "click" || g == "dblclick") {
                    if (j != undefined) {
                        var e = function (n) {
                            if (j.l1li == true && g == "click") {
                                j.l1li = false;
                                return
                            }
                            if (j.hasGestured) {
                                if (n.touches.length == 0) {
                                    j.hasGestured = false
                                }
                                return
                            }
                            if (j.Iij == true) {
                                j.Iij = false;
                                return
                            }
                            if (j.hasDraggedText == true) {
                                j.hasDraggedText = false;
                                return
                            }
                            var m = new VPoint(lI1 - a.Left, lIl - a.Top);
                            var k = a.fromContainerPixelToLatLng(m);
                            var l = new Array();
                            VUtil.bubbleEventObject(n, l);
                            var o = l[1];
                            if (l[0]) {
                                if (o.II != undefined) {
                                    h(o.II, k)
                                }
                                return
                            }
                            h(null, k)
                        };
                        if (browserName == "iPhone" || browserName == "iPod" || browserName == "iPad") {
                            return this.addDOMListener(j, "touchend", e)
                        }
                        return this.addDOMListener(j, g, e)
                    }
                } else {
                    if (g == "singlerightclick") {
                        g = "contextmenu";
                        if (j != undefined) {
                            return this.addDOMListener(j, g, function (n) {
                                var l = (browserName != "msie") ? n.target : event.srcElement;
                                var o = l;
                                var m = new VPoint(lI1 - a.Left, lIl - a.Top);
                                var k = setTimeout(function () {
                                    clearTimeout(k);
                                    if (j.dblRC == "true") {
                                        j.dblRC == "false"
                                    } else {
                                        var p = (browserName != "msie") ? "HTML" : "BODY";
                                        while (o.tagName != "path" && o.tagName != p && (o.className.indexOf != undefined && o.className.indexOf("dragruler") == -1 && o.className != "overviewmap" && o.className != "dragoverview" && o.className != "dragmarker" && o.className != "clickmarker" && o.className != "dragline" && o.className != "dragtext" && o.className.indexOf("clickcontrol") == -1 && o.className != "nomousemove")) {
                                            o = (browserName != "msie") ? o.parentNode : o.parentElement;
                                            if (o == null) {
                                                return false
                                            }
                                        }
                                        if (o.tagName != p && o.tagName != "svg") {
                                            if (o.II != undefined) {
                                                h(m, l, o.II)
                                            }
                                            return
                                        }
                                        h(m, l, null)
                                    }
                                }, 230)
                            })
                        }
                    } else {
                        if (g == "movestart") {
                            if (a != undefined) {
                                a.li1 = h
                            }
                        } else {
                            if (g == "moving") {
                                if (a != undefined) {
                                    a.jji = h
                                }
                            } else {
                                if (g == "moveend") {
                                    if (a != undefined) {
                                        if (a.lij == undefined) {
                                            a.lij = new Array()
                                        }
                                        a.lij[a.lij.length] = h
                                    }
                                } else {
                                    if (g == "zoomstart") {
                                        if (a != undefined) {
                                            a.zoomstartHandler = h
                                        }
                                    } else {
                                        if (g == "zoomend") {
                                            if (a != undefined) {
                                                a.j1I = h
                                            }
                                        } else {
                                            if (g == "infowindowbeforeclose") {
                                                if (a != undefined) {
                                                    a.iI1 = h
                                                }
                                            } else {
                                                if (g == "infowindowclose") {
                                                    if (a != undefined) {
                                                        a.iIl = h
                                                    }
                                                } else {
                                                    if (g == "infowindowclosebuttonclick") {
                                                        if (a != undefined) {
                                                            a.infoWindowCloseButtonClickHandler = h
                                                        }
                                                    } else {
                                                        if (g == "addoverlay") {
                                                            if (a != undefined) {
                                                                a.l1lj = h
                                                            }
                                                        } else {
                                                            if (g == "removeoverlay") {
                                                                if (a != undefined) {
                                                                    a.l11l = h
                                                                }
                                                            } else {
                                                                if (g == "clearoverlays") {
                                                                    if (a != undefined) {
                                                                        a.lliI = h
                                                                    }
                                                                } else {
                                                                    if (g == "mouseover" || g == "mouseout" || g == "mousemove") {
                                                                        if (j != undefined) {
                                                                            return this.addDOMListener(j, g, function (n) {
                                                                                var o = (browserName != "msie") ? n.target : event.srcElement;
                                                                                var l = (browserName != "msie") ? "HTML" : "BODY";
                                                                                while (o.parentNode.tagName != "svg" && o.tagName != l && (o.className.indexOf != undefined && o.className.indexOf("dragruler") == -1 && o.className != "overviewmap" && o.className != "dragoverview" && o.className != "dragmarker" && o.className != "dragline" && o.className.indexOf("clickcontrol") == -1 && o.className != "nomousemove")) {
                                                                                    o = (browserName != "msie") ? o.parentNode : o.parentElement;
                                                                                    if (o == null) {
                                                                                        return false
                                                                                    }
                                                                                }
                                                                                if (o.tagName != l && o.tagName != "svg") {
                                                                                    return
                                                                                }
                                                                                if (j.Iij == true) {
                                                                                    j.Iij = false;
                                                                                    return
                                                                                }
                                                                                var m = new VPoint(lI1 - a.Left, lIl - a.Top);
                                                                                var k = a.fromContainerPixelToLatLng(m);
                                                                                h(k)
                                                                            })
                                                                        }
                                                                    } else {
                                                                        if (g == "dragstart") {
                                                                            if (a != undefined) {
                                                                                a.lli1 = h
                                                                            }
                                                                        } else {
                                                                            if (g == "dragging") {
                                                                                if (a != undefined) {
                                                                                    a.iII = h
                                                                                }
                                                                            } else {
                                                                                if (g == "dragend") {
                                                                                    if (a != undefined) {
                                                                                        a.jjI = h
                                                                                    }
                                                                                } else {
                                                                                    if (g == "loaded") {
                                                                                        if (a != undefined) {
                                                                                            a.iIi = h
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "VTileLayer":
                if (g == "tileload") {
                    if (a != undefined) {
                        a.tileloadHandler = h
                    }
                }
                break;
            case "VMarker":
                var i = document.getElementById(a.id);
                var j = document.getElementById(a.ll.id);
                if (g == "click" || g == "dblclick" || g == "mousedown" || g == "mouseup" || g == "mouseover" || g == "mouseout" || g == "mousemove") {
                    if (i != undefined) {
                        if (a.sHandler == null || a.sHandler == undefined) {
                            a.sHandler = new Array();
                            a.svEvent = new Array()
                        }
                        a.sHandler[a.sHandler.length] = h;
                        a.svEvent[a.svEvent.length] = g;
                        var e = function (m) {
                            if (j.Iij == true) {
                                j.Iij = false;
                                return
                            }
                            var l = new VPoint(lI1 - j.ij.Left, lIl - j.ij.Top);
                            var k = j.ij.fromContainerPixelToLatLng(l);
                            h(a, k)
                        };
                        var b = i;
                        if (j.ij.i1I.isOpened()) {
                            if (VUtil.isInsidePolygon(a.latlngs, j.ij.i1I.mappingLLs)) {
                                b = a.mappingDiv
                            }
                        }
                        if (browserName == "iPhone" || browserName == "iPod" || browserName == "iPad") {
                            if (g == "click" || g == "dblclick") {
                                g = "touchend"
                            } else {
                                if (g == "mousedown" || g == "mouseover") {
                                    g = "touchstart"
                                } else {
                                    if (g == "mouseup" || g == "mouseout") {
                                        g = "touchend"
                                    } else {
                                        if (g == "mousemove") {
                                            g = "touchmove"
                                        }
                                    }
                                }
                            }
                            return this.addDOMListener(b, g, e)
                        }
                        return this.addDOMListener(b, g, e)
                    }
                } else {
                    if (g == "infowindowopen") {
                        i.iIj = h
                    } else {
                        if (g == "infowindowbeforeclose") {
                            i.iI1 = h
                        } else {
                            if (g == "infowindowclose") {
                                i.iIl = h
                            } else {
                                if (g == "remove") {
                                    i.il1 = h
                                } else {
                                    if (g == "dragstart") {
                                        i.dragStartHandler = function () {
                                            if (this.II.Il.draggable) {
                                                h(this.II)
                                            }
                                        }
                                    } else {
                                        if (g == "dragging") {
                                            i.iII = function () {
                                                if (this.II.Il.draggable) {
                                                    h(this.II)
                                                }
                                            }
                                        } else {
                                            if (g == "dragend") {
                                                i.dragEndHandler = function () {
                                                    if (this.II.Il.draggable) {
                                                        h(this.II)
                                                    }
                                                }
                                            } else {
                                                if (g == "visibilitychanged") {
                                                    i.I1l = h
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "VText":
                var c = a.lj;
                if (g == "dragend") {
                    c.dragEndHandler = function () {
                        if (this.II.textOptions.draggable) {
                            h(this.II)
                        }
                    }
                }
                break;
            case "VPolyline":
                var f = a.lj;
                if (g == "remove") {
                    a.il1 = h
                } else {
                    if (g == "visibilitychanged") {
                        a.I1l = h
                    } else {
                        if (g == "click") {
                            if (a.opts.clickable) {
                                a.llj = h;
                                a.addLineHandlers()
                            }
                        } else {
                            if (g == "mouseover") {
                                a.mouseoverHandler = h;
                                a.addLineHandlers()
                            } else {
                                if (g == "mousemove") {
                                    a.mousemoveHandler = h;
                                    a.addLineHandlers()
                                } else {
                                    if (g == "mouseout") {
                                        a.mouseoutHandler = h;
                                        a.addLineHandlers()
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "VPolygon":
                var d = a.lj;
                if (g == "remove") {
                    a.il1 = h
                } else {
                    if (g == "visibilitychanged") {
                        a.I1l = h
                    } else {
                        if (g == "click") {
                            if (a.opts.clickable) {
                                a.llj = h;
                                a.addLineHandlers()
                            }
                        } else {
                            if (g == "mouseover") {
                                a.mouseoverHandler = h;
                                a.addLineHandlers()
                            } else {
                                if (g == "mousemove") {
                                    a.mousemoveHandler = h;
                                    a.addLineHandlers()
                                } else {
                                    if (g == "mouseout") {
                                        a.mouseoutHandler = h;
                                        a.addLineHandlers()
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case "VDirections":
                if (g == "loaded") {
                    if (a != undefined) {
                        a.iIi = h
                    }
                }
                break;
            case "VInfoWindow":
                if (g == "maximizeclick") {
                    if (a != undefined) {
                        a.maximizeclickHandler = h
                    }
                } else {
                    if (g == "maximizeend") {
                        if (a != undefined) {
                            a.maximizeendHandler = h
                        }
                    } else {
                        if (g == "restoreclick") {
                            if (a != undefined) {
                                a.restoreclickHandler = h
                            }
                        } else {
                            if (g == "restoreend") {
                                if (a != undefined) {
                                    a.restoreendHandler = h
                                }
                            }
                        }
                    }
                }
                break
        }
        return new VEventListener(a, g, h, 0)
    },
    addDOMListener: function (c, a, b) {
        if (c != undefined) {
            if (browserName == "msie") {
                c.attachEvent("on" + a, b)
            } else {
                c.addEventListener(a, b, true)
            }
            return new VEventListener(c, a, b, 1)
        }
        return null
    },
    removeListener: function (f) {
        if (f.type == 1) {
            if (f.source != undefined) {
                if (browserName == "msie") {
                    f.source.detachEvent("on" + f.li, f.handler)
                } else {
                    f.source.removeEventListener(f.li, f.handler, true)
                }
            }
        } else {
            switch (f.source.vType()) {
                case "VMap":
                    var e = document.getElementById(f.source.id);
                    if (f.li == "movestart") {
                        if (f.source != undefined) {
                            delete f.source.li1
                        }
                    } else {
                        if (f.li == "moving") {
                            if (f.source != undefined) {
                                delete f.source.jji
                            }
                        } else {
                            if (f.li == "moveend") {
                                if (f.source != undefined) {
                                    if (f.source.lij != undefined) {
                                        for (var d = 0; d < f.source.lij.length; d++) {
                                            delete f.source.lij[d]
                                        }
                                        f.source.lij = new Array()
                                    }
                                }
                            } else {
                                if (f.li == "zoomend") {
                                    if (f.source != undefined) {
                                        delete f.source.j1I
                                    }
                                } else {
                                    if (f.li == "infowindowbeforeclose") {
                                        if (f.source != undefined) {
                                            delete f.source.iI1
                                        }
                                    } else {
                                        if (f.li == "infowindowclose") {
                                            if (f.source != undefined) {
                                                delete f.source.iIl
                                            }
                                        } else {
                                            if (f.li == "infowindowclosebuttonclick") {
                                                if (f.source != undefined) {
                                                    delete f.source.infoWindowCloseButtonClickHandler
                                                }
                                            } else {
                                                if (f.li == "addoverlay") {
                                                    if (f.source != undefined) {
                                                        delete f.source.l1lj
                                                    }
                                                } else {
                                                    if (f.li == "removeoverlay") {
                                                        if (f.source != undefined) {
                                                            delete f.source.l11l
                                                        }
                                                    } else {
                                                        if (f.li == "clearoverlays") {
                                                            if (f.source != undefined) {
                                                                delete f.source.lliI
                                                            }
                                                        } else {
                                                            if (f.li == "dragstart") {
                                                                if (f.source != undefined) {
                                                                    delete f.source.lli1
                                                                }
                                                            } else {
                                                                if (f.li == "dragging") {
                                                                    if (f.source != undefined) {
                                                                        delete f.source.iII
                                                                    }
                                                                } else {
                                                                    if (f.li == "dragend") {
                                                                        if (f.source != undefined) {
                                                                            delete f.source.jjI
                                                                        }
                                                                    } else {
                                                                        if (f.li == "loaded") {
                                                                            if (f.source != undefined) {
                                                                                delete f.source.iIi
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "VTileLayer":
                    if (f.li == "tileload") {
                        if (f.source != undefined) {
                            delete f.source.tileloadHandler
                        }
                    }
                    break;
                case "VMarker":
                    var g = document.getElementById(f.source.id);
                    var e = document.getElementById(f.source.ll.id);
                    if (f.li == "infowindowopen") {
                        g.iIj = null
                    } else {
                        if (f.li == "infowindowbeforeclose") {
                            g.iI1 = null
                        } else {
                            if (f.li == "infowindowclose") {
                                g.iIl = null
                            } else {
                                if (f.li == "remove") {
                                    g.il1 = null
                                } else {
                                    if (f.li == "dragstart") {
                                        if (f.source.Il.draggable) {
                                            g.dragStartHandler = null
                                        }
                                    } else {
                                        if (f.li == "dragging") {
                                            if (f.source.Il.draggable) {
                                                g.iII = null
                                            }
                                        } else {
                                            if (f.li == "dragend") {
                                                if (f.source.Il.draggable) {
                                                    g.dragEndHandler = null
                                                }
                                            } else {
                                                if (f.li == "visibilitychanged") {
                                                    g.I1l = null
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "VText":
                    var a = f.source.lj;
                    if (f.li == "dragend") {
                        if (f.source.textOptions.draggable) {
                            a.dragEndHandler = null
                        }
                    }
                    break;
                case "VPolyline":
                    var c = f.source.lj;
                    if (f.li == "remove") {
                        delete f.source.il1
                    } else {
                        if (f.li == "visibilitychanged") {
                            delete f.source.I1l
                        } else {
                            if (f.li == "click") {
                                delete f.source.llj;
                                f.source.removeLineHandlers()
                            } else {
                                if (f.li == "mouseover") {
                                    delete f.source.mouseoverHandler;
                                    f.source.removeLineHandlers()
                                } else {
                                    if (f.li == "mousemove") {
                                        delete f.source.mousemoveHandler;
                                        f.source.removeLineHandlers()
                                    } else {
                                        if (f.li == "mouseout") {
                                            delete f.source.mouseoutHandler;
                                            f.source.removeLineHandlers()
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "VPolygon":
                    var b = f.source.lj;
                    if (f.li == "remove") {
                        delete f.source.il1
                    } else {
                        if (f.li == "visibilitychanged") {
                            delete f.source.I1l
                        } else {
                            if (f.li == "click") {
                                delete f.source.llj;
                                f.source.removeLineHandlers()
                            } else {
                                if (f.li == "mouseover") {
                                    delete f.source.mouseoverHandler;
                                    f.source.removeLineHandlers()
                                } else {
                                    if (f.li == "mousemove") {
                                        delete f.source.mousemoveHandler;
                                        f.source.removeLineHandlers()
                                    } else {
                                        if (f.li == "mouseout") {
                                            delete f.source.mouseoutHandler;
                                            f.source.removeLineHandlers()
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "VDirections":
                    if (f.li == "loaded") {
                        if (f.source != undefined) {
                            delete f.source.iIi
                        }
                    }
                    break;
                case "VInfoWindow":
                    if (f.li == "maximizeclick") {
                        if (f.source != undefined) {
                            delete f.source.maximizeclickHandler
                        }
                    } else {
                        if (f.li == "maximizeend") {
                            if (f.source != undefined) {
                                delete f.source.maximizeendHandler
                            }
                        } else {
                            if (f.li == "restoreclick") {
                                if (f.source != undefined) {
                                    delete f.source.restoreclickHandler
                                }
                            } else {
                                if (f.li == "restoreend") {
                                    if (f.source != undefined) {
                                        delete f.source.restoreendHandler
                                    }
                                }
                            }
                        }
                    }
                    break
            }
        }
    },
    clearListeners: function (b, a) { },
    clearInstanceListeners: function (a) { },
    trigger: function (b, a) { },
    vType: function () {
        return "VEvent"
    }
};

function VEventListener(d, a, c, b) {
    this.source = d;
    this.li = a;
    this.handler = c;
    this.type = b;
    this.vType = function () {
        return "VEventListener"
    }
}

function VIcon(b, a) {
    if (b == undefined || b.image == undefined) {
        this.image = (b == undefined) ? V_DEFAULT_ICON : b;
        this.llii = "";
        this.iconSize = (a == undefined) ? null : a;
        this.l1II = null
    } else {
        this.image = b.image;
        this.llii = b.llii;
        this.iconSize = b.iconSize;
        this.l1II = b.l1II
    }
    this.vType = function () {
        return "VIcon"
    }
}

function VInfoWindow() {
    this.selectedTab = 0;
    this.tabs = null;
    this.point = null;
    this.size = null;
    this.offset = null;
    this.lIi = false;
    this.maximizable = true;
    this.sizeState = "normal";
    this.ll = null;
    this.mappingLLs = null;
    this.options = new VInfoWindowOptions();
    this.selectTab = s;
    this.hide = l;
    this.show = t;
    this.close = o;
    this.updateMaxContent = v;
    this.updateMaxTitle = f;
    this.createInfoWindow = w;
    this.redraw = p;
    this.getInfoWindowSize = n;
    this.getInfoWindowPan = m;
    this.isOpened = g;
    this.isHidden = e;
    this.reset = u;
    this.getPoint = b;
    this.getPixelOffset = k;
    this.getSelectedTab = h;
    this.getTabs = i;
    this.getContentContainers = c;
    this.enableMaximize = a;
    this.disableMaximize = r;
    this.maximize = j;
    this.restore = q;
    this.vType = d;

    function s(x) {
        if (document.getElementById(this.ll.id + "_InfoWindow") == null) {
            return
        }
        if (this.tabs.length == 1) {
            this.selectedTab = 0;
            document.getElementById("VIWContent0").style.display = "";
            return
        }
        document.getElementById("VIWTab" + this.selectedTab).style.zIndex = 5;
        document.getElementById("VIWTab" + this.selectedTab).childNodes[0].childNodes[0].style.left = "-201px";
        document.getElementById("VIWContent" + this.selectedTab).style.display = "none";
        this.selectedTab = x;
        document.getElementById("VIWTab" + this.selectedTab).style.zIndex = 6;
        document.getElementById("VIWTab" + this.selectedTab).childNodes[0].childNodes[0].style.left = "-98px";
        document.getElementById("VIWContent" + this.selectedTab).style.display = ""
    }

    function l() {
        if (document.getElementById(this.ll.id + "_InfoWindow") != null) {
            document.getElementById(this.ll.id + "_InfoWindow").style.display = "none"
        }
        this.lIi = true
    }

    function o() {
        if (this.ll.iI1 != undefined) {
            this.ll.iI1()
        }
        var z = document.getElementById(this.ll.id + "_path");
        var x = document.getElementById(this.ll.id + "_InfoWindow");
        if (x != null) {
            z.removeChild(x);
            if (this.ll.iIl != undefined) {
                this.ll.iIl()
            }
        }
        this.sizeState = "normal";
        this.ll.showControls();
        var y;
        for (y = 0; y < this.ll.Ill; y++) {
            if (this.ll.lli[y].vType() == "VMarker") {
                this.ll.lli[y].removeMappingMarker()
            }
        }
        if (this.openMk != null) {
            this.openMk.updateZIndex();
            this.openMk = null
        }
    }

    function t() {
        if (!this.lIi) {
            var x = document.getElementById(this.ll.id + "_InfoWindow");
            if (!this.options.maximizeWhenOpen) {
                this.sizeState = "normal";
                this.ll.showControls()
            } else {
                if (this.sizeState == "maximize") {
                    this.ll.hideControls()
                }
            } if (x == null) {
                var y = document.getElementById(this.ll.id + "_path");
                x = document.createElement("div");
                x.id = this.ll.id + "_InfoWindow";
                x.style.zIndex = InfoWindowShadeZIndex;
                x.style.position = "absolute";
                y.appendChild(x)
            }
            this.createInfoWindow()
        } else {
            if (document.getElementById(this.ll.id + "_InfoWindow") != null) {
                document.getElementById(this.ll.id + "_InfoWindow").style.display = ""
            }
            this.lIi = false
        }
    }

    function v(x) {
        if (document.getElementById(this.ll.id + "_InfoWindow") == null) {
            return
        }
        this.options.maxContent = x;
        document.getElementById("VIWC").innerHTML = x
    }

    function f(x) {
        if (document.getElementById(this.ll.id + "_InfoWindow") == null) {
            return
        }
        this.options.maxTitle = x;
        document.getElementById("VIWT").innerHTML = x
    }

    function w() {
        var L = document.getElementById(this.ll.id + "_InfoWindow");
        if (L == null) {
            return
        }
        var M = this.ll.fromLatLngToDivPixel(this.point);
        var aB = 32;
        var ap = -7;
        var an = 0;
        var O = this.ll.lI.offsetWidth - 50;
        var S = this.ll.lI.offsetHeight - 50;
        var ax = Math.min(this.size.width, O);
        var aq = Math.min(this.size.height, S);
        if (this.sizeState == "normal") {
            ax = Math.max(ax, 84 * this.tabs.length - 1 + 50)
        } else {
            ax = Math.min(658, O);
            aq = Math.min(474, S)
        }
        var A = ax + aB;
        var Q = aq + aB;
        if (this.sizeState == "normal") {
            A = Math.min(A, 690);
            Q = Math.min(Q, 506)
        }
        var aF = 97;
        var az = 96;
        var ar = (this.sizeState == "normal") ? Q + az : Q + 25;
        this.lll = A;
        this.Il1 = ar;
        var ak = A + 40,
            ab = parseInt(ar * 0.5);
        ak = Math.min(ak, 740);
        ab = Math.min(ab, 298);
        var K = 323;
        var U = 1033;
        var C = 30;
        var D = 60;
        var ag = 70;
        var J = 14;
        var B = 754;
        var at = 310;
        var ao = 345;
        var P = 140;
        var am = 259;
        var I = 119;
        var Z = new Array();
        var aE = strResURL + "images/infowindowshade.png";
        var al = ak - 2 * ag;
        var ai = 0;
        var G = 0;
        var W = I;
        var aj = ab - C - D;
        var x = K - aj;
        var z = ag;
        var ah = al - P + C;
        if (ah > 0) {
            if (ah >= 50) {
                ai = (ah - 50) / 2;
                G = ah - ai
            } else {
                ai = 0;
                G = ah
            }
        }
        if (aj >= P) {
            W = am
        }
        var ad = aj + C;
        var aA = 0;
        var aa = ad + ag;
        var H = aA;
        var E = M.x - ag - ai + ap + this.offset.width;
        var F = M.y - ab + an + this.offset.height;
        if (this.sizeState == "maximize") {
            F += 45
        }
        Z[Z.length] = "<div class='dragmap' style='z-index:" + InfoWindowShadeZIndex + "; position:absolute; left:" + E + "px; top:" + F + "px;'>";
        Z[Z.length] = "<div style='overflow:hidden; width:" + ag + "px; height:" + C + "px; z-index:1; position:absolute; left:" + ad + "px; top:" + aA + "px;'>";
        Z[Z.length] = "<img class='transimg' src='" + aE + "' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + K + "px; top:0px; width:1144px; height:370px; -moz-user-select:none;' />";
        Z[Z.length] = "</div>";
        if (al > 0) {
            Z[Z.length] = "<div style='overflow:hidden; position:absolute; left:" + aa + "px; top:0px; width:" + al + "px; height:30px;'>";
            Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + (K + ag) + "px; top:0px; width:1144px; height:370px; -moz-user-select:none;' src='" + aE + "'/>";
            Z[Z.length] = "</div>"
        }
        Z[Z.length] = "<div style='overflow:hidden; width:" + ag + "px; height:" + C + "px; z-index:1; position:absolute; left:" + (ad + ag + al) + "px; top:0px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + U + "px; top:0px; width:1144px; height:370px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        Z[Z.length] = "<div style='overflow:hidden; width:" + z + "px; height:" + aj + "px; bottom:-1px; position:absolute; left:" + (ad - aj) + "px; top:" + C + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + x + "px; top:-" + C + "px; -moz-user-select:none; width:1144px; height:370px;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        Z[Z.length] = "<div style='overflow:hidden; position:absolute; left:" + (ad + ag - aj) + "px; top:" + C + "px; width:" + al + "px; height:" + aj + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left: -" + (x + ag) + "px; top:-" + C + "px; width:1144px; height:370px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        Z[Z.length] = "<div style='overflow:hidden; width:" + (z + aj) + "px; height:" + aj + "px; bottom:-1px; position:absolute; left:" + (ad - aj + z + al) + "px; top:" + C + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + (U - aj) + "px; top:-" + C + "px; -moz-user-select:none; width:1144px; height:370px;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        Z[Z.length] = "<div style='overflow:hidden; width:" + ag + "px; height:" + D + "px; z-index:1; position:absolute; left:" + (ad - aj - C) + "px; top:" + (C + aj) + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + J + "px; top:-" + at + "px; width:1144px; height:370px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        if (ai > 0) {
            Z[Z.length] = "<div style='overflow:hidden; position:absolute; left:" + (ad - aj - C + ag) + "px; top:" + (C + aj) + "px; width:" + ai + "px; height:" + D + "px;'>";
            Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + ao + "px; top:-" + at + "px; width:1144px; height:370px; -moz-user-select:none;' src='" + aE + "'/>";
            Z[Z.length] = "</div>"
        }
        Z[Z.length] = "<div style='overflow:hidden; width:" + P + "px; height:" + D + "px; z-index:1; position:absolute; left:" + (ad - aj - C + ag + ai) + "px; top:" + (C + aj) + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + W + "px; top:-310px; width:1144px; height:370px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        if (G > 0) {
            Z[Z.length] = "<div style='overflow:hidden; position:absolute; left:" + (ad - aj - C + ag + P + ai) + "px; top:" + (C + aj) + "px; width:" + G + "px; height:" + D + "px;'>";
            Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + ao + "px; top:-310px; width:1144px; height:370px; -moz-user-select:none;' src='" + aE + "'/>";
            Z[Z.length] = "</div>"
        }
        Z[Z.length] = "<div style='overflow:hidden; width:" + ag + "px; height:" + D + "px; z-index:1; position:absolute; left:" + (ad - aj + z + al) + "px; top:" + (C + aj) + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:-" + B + "px; top:-" + at + "px; width:1144px; height:370px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        Z[Z.length] = "</div>";
        var R = new Array();
        R[R.length] = new VPoint(ad - aj - C + E, Q);
        R[R.length] = new VPoint(ad - aj - C + E, C + aj + D + F + 15);
        R[R.length] = new VPoint(ad - aj + z + al + ag + E, C + aj + D + F + 15);
        R[R.length] = new VPoint(ad - aj + z + al + ag + E, C + aj + F);
        R[R.length] = new VPoint(ad - aj + z + al + z + aj + E, C + aj + F);
        R[R.length] = new VPoint(ad - aj + z + al + z + aj + E, C + F);
        R[R.length] = new VPoint(ad + ag + al + ag + E, C + F);
        R[R.length] = new VPoint(ad + ag + al + ag + E, F);
        R[R.length] = new VPoint(A + E, F);
        K = 0;
        U = 665;
        C = 25;
        D = 25;
        ag = 25;
        J = 0;
        B = 665;
        at = 665;
        ao = 345;
        aF = 97;
        az = 96;
        am = 368;
        I = 0;
        aE = strResURL + "images/infowindow.png";
        E = parseInt(M.x + (aF - A) / 2) + ap + this.offset.width;
        F = parseInt(M.y - az - Q + 25) + an + this.offset.height;
        if (this.sizeState == "maximize") {
            F += 45
        }
        al = ak - 2 * ag;
        ai = 0;
        G = 0;
        W = I;
        aj = ab - C - D;
        x = K - aj;
        z = ag;
        ah = al - aF + C;
        if (ah > 0) {
            if (ah >= 50) {
                ai = (ah - 50) / 2;
                G = ah - ai
            } else {
                G = ai;
                ai = 0
            }
        }
        if (aj >= aF) {
            W = am
        }
        ad = aj + C;
        aA = 0;
        aa = ad + ag;
        H = aA;
        Z[Z.length] = "<div class='clickcontrol' style='z-index:" + InfoWindowZIndex + "; position:absolute; left:" + E + "px; top:" + F + "px;'>";
        Z[Z.length] = "<div style='cursor:default; position:relative; left:0px; top:0px; z-index:10; width:" + A + "px; height:" + Q + "px;'>";
        Z[Z.length] = "<img class='transimg' id='" + this.ll.id + "_VIWBtnClose' style='border:0px; margin:0px; padding:0px; position:absolute; left:" + (A - 23) + "px; top:7px; width:13px; height:13px; -moz-user-select: none; cursor:pointer; z-index:11;' src='" + strResURL + "images/iwclose.gif'/>";
        var ae = (this.sizeState == "normal") ? "iwmaximize.gif" : "iwrestore.gif";
        Z[Z.length] = "<img class='transimg' id='VIWBtnMR' style='" + (this.options.maxContent == "" ? "display:none;" : "") + "border:0px; margin:0px; padding:0px; position:absolute; left:" + (A - 40) + "px; top:7px; width:13px; height:13px; -moz-user-select: none; cursor:pointer; z-index:11;' src='" + strResURL + "images/" + ae + "' />";
        var aw;
        if (this.sizeState == "normal") {
            for (aw = 0; aw < this.tabs.length; aw++) {
                Z[Z.length] = "<div id='VIWContent" + aw + "' style='position:absolute; left:" + aB / 2 + "px; top:" + aB / 2 + "px; width:" + ax + "px; height:" + aq + "px; z-index:10; " + ((this.selectedTab != aw) ? "display:none;" : "") + "'><div>";
                Z[Z.length] = this.tabs[aw].content;
                Z[Z.length] = "</div></div>"
            }
        } else {
            Z[Z.length] = "<div id='VIWContent' style='position:absolute; left:" + aB / 2 + "px; top:" + aB / 2 + "px; width:" + ax + "px; height:" + aq + "px; z-index:10;'>";
            Z[Z.length] = "<div style='position:absolute; left:0px; top:-15px; width:" + (ax + 15) + "px; height:" + (aq + 23) + "px;' >";
            Z[Z.length] = "<div style='background:rgb(244, 244, 244) none repeat scroll 0% 0%; -moz-background-clip:-moz-initial; -moz-background-origin:-moz-initial; -moz-background-inline-policy:-moz-initial; height:" + ((browserName == "msie") ? 24 : 23) + "px; margin-right:7px; position:relative;'>";
            Z[Z.length] = "<div id='VIWT' style='overflow:hidden; width:" + (A - 51) + "px; text-align:center; position:absolute; top:2px;'>";
            Z[Z.length] = this.options.maxTitle;
            Z[Z.length] = "</div>";
            Z[Z.length] = "</div>";
            Z[Z.length] = "<div id='VIWC' style='background:white none repeat scroll 0% 0%; overflow:auto; -moz-background-clip:-moz-initial; -moz-background-origin:-moz-initial; -moz-background-inline-policy:-moz-initial; position:relative; outline-color:-moz-use-text-color; outline-style:none; outline-width:0px; width:" + (ax + 15) + "px; height:" + (aq - 1) + "px;'>";
            Z[Z.length] = this.options.maxContent;
            Z[Z.length] = "</div>";
            Z[Z.length] = "</div>";
            Z[Z.length] = "</div>"
        }
        Z[Z.length] = "</div>";
        var N = (this.sizeState == "normal") ? 0 : -517;
        var T = (this.sizeState == "normal") ? 0 : -691;
        Z[Z.length] = "<div style='overflow:hidden; width:" + ag + "px; height:" + C + "px; z-index:1; position:absolute; left:0px; top:0px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:" + N + "px; top:" + T + "px; width:690px; height:786px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        if (this.sizeState == "maximize") {
            Z[Z.length] = "<div style='border-top:1px solid rgb(171, 171, 171); overflow:hidden; width:" + A + "px; height:1px; z-index:12; position:absolute; left:0px; top:" + (C - 1) + "px;'></div>"
        }
        var aD = (this.sizeState == "normal") ? -665 : -542;
        var V = (this.sizeState == "normal") ? 0 : -691;
        Z[Z.length] = "<div style='overflow:hidden; width:" + ag + "px; height:" + C + "px; z-index:1; position:absolute; left:" + (A - 25) + "px; top:0px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:" + aD + "px; top:" + V + "px; width:690px; height:786px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        var aC = (this.sizeState == "normal") ? 0 : -517;
        var X = (this.sizeState == "normal") ? -665 : -716;
        Z[Z.length] = "<div style='overflow:hidden; width:" + ag + "px; height:" + D + "px; z-index:1; position:absolute; left:0px; top:" + (Q - 25) + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:" + aC + "px; top:" + X + "px; width:690px; height:786px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        var y = (this.sizeState == "normal") ? -665 : -542;
        var af = (this.sizeState == "normal") ? -665 : -716;
        Z[Z.length] = "<div style='overflow:hidden; width:" + ag + "px; height:" + D + "px; z-index:1; position:absolute; left:" + (A - 25) + "px; top:" + (Q - 25) + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:" + y + "px; top:" + af + "px; width:690px; height:786px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        Z[Z.length] = "<div style='z-index:1; background-color: white; position: absolute; width:" + (A - 50) + "px; height:" + D + "px; left:" + ag + "px; top:0px;' ></div><div style='z-index:1; border-top: 1px solid rgb(171, 171, 171); position: absolute; width:" + (A - 50) + "px; height:1px; left:" + ag + "px; top:0px;' ></div>";
        Z[Z.length] = "<div style='z-index:1; background-color: white; position: absolute; width:" + A + "px; height:" + (Q - 50) + "px; left:0px; top:" + ag + "px;' ></div><div style='z-index:1; border-left: 1px solid rgb(171, 171, 171); position:absolute; width:1px; height:" + (Q - 50) + "px; left:0px; top:" + ag + "px;' ></div><div style='z-index:1; border-left: 1px solid rgb(171, 171, 171); position:absolute; width:1px; height:" + (Q - 50) + "px; left:" + (A - 1) + "px; top:" + ag + "px;' ></div>";
        Z[Z.length] = "<div style='z-index:1; background-color: white; position: absolute; width:" + (A - 50) + "px; height:" + D + "px; left:" + ag + "px; top:" + (Q - 25) + "px;' ></div><div style='z-index:1; border-top: 1px solid rgb(171, 171, 171); position: absolute; width:" + (A - 50) + "px; height:1px; left:" + ag + "px; top:" + (Q - 1) + "px;' ></div>";
        var W = (this.sizeState == "normal") ? 0 : -368;
        Z[Z.length] = "<div class='dragmap' style='overflow:hidden; width:" + aF + "px; height:" + az + "px; z-index:1; position:absolute; left:" + ((A - aF) / 2) + "px; top:" + (Q - 25) + "px;'>";
        Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:" + W + "px; top:-691px; width:690px; height:786px; -moz-user-select:none;' src='" + aE + "'/>";
        Z[Z.length] = "</div>";
        if (this.tabs.length > 1 && this.sizeState == "normal") {
            Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:0px; top:-24px; width:11px; height:75px; -moz-user-select:none; z-index:1;' src='" + strResURL + "images/iwlefttop.png' />";
            for (aw = 0; aw < this.tabs.length; aw++) {
                Z[Z.length] = "<div id='VIWTab" + aw + "' style='position:absolute; left:" + (11 + aw * 84) + "px; top:-24px; z-index:" + ((this.selectedTab == aw) ? 6 : 5) + ";'>";
                Z[Z.length] = "<div style='overflow:hidden; position:absolute; left:0px; top:0px; width:103px; height:75px;'>";
                Z[Z.length] = "<img class='transimg' style='border:0px; margin:0px; padding:0px; position:absolute; left:" + ((this.selectedTab == aw) ? -98 : -201) + "px; top:-691px; -moz-user-select:none; width:690px; height:786px;' src='" + aE + "' />";
                Z[Z.length] = "</div>";
                Z[Z.length] = "<div style='position:absolute; left:0px; top:0px; width:103px; height:24px; font-family: Arial,sans-serif; font-size:12px; padding-top:5px; text-align:center; cursor:pointer;'>" + this.tabs[aw].label.substring(0, 10) + "</div>";
                Z[Z.length] = "</div>"
            }
        }
        Z[Z.length] = "</div>";
        L.innerHTML = Z.join("");
        L.style.visibility = "visible";
        VUtil.fixPNG();
        R[R.length - 1].x = A + E;
        R[R.length] = new VPoint(A + E, Q + F);
        R[0].y = Q + F;
        R[R.length] = new VPoint(R[0].x, R[0].y);
        var av = new Array();
        for (aw = 0; aw < R.length; aw++) {
            av[aw] = this.ll.fromDivPixelToLatLng(R[aw])
        }
        this.mappingLLs = av;
        for (aw = 0; aw < this.ll.Ill; aw++) {
            if (this.ll.lli[aw].vType() == "VMarker") {
                this.ll.lli[aw].createMappingMarker()
            }
        }
        var Y = this;
        var ac = document.getElementById(this.ll.id + "_VIWBtnClose");
        ac.onclick = function () {
            if (Y.ll.infoWindowCloseButtonClickHandler != undefined) {
                Y.ll.infoWindowCloseButtonClickHandler()
            }
            Y.close()
        };
        ac.onmouseover = function () {
            this.src = strResURL + "images/iwcloseover.gif"
        };
        ac.onmouseout = function () {
            this.src = strResURL + "images/iwclose.gif"
        };
        if (this.options.maxContent != "") {
            var au = document.getElementById("VIWBtnMR");
            au.onclick = function () {
                if (Y.sizeState == "normal") {
                    Y.maximize()
                } else {
                    Y.restore()
                }
            };
            au.onmouseover = function () {
                if (Y.sizeState == "normal") {
                    this.src = strResURL + "images/iwmaximizeover.gif"
                } else {
                    this.src = strResURL + "images/iwrestoreover.gif"
                }
            };
            au.onmouseout = function () {
                if (Y.sizeState == "normal") {
                    this.src = strResURL + "images/iwmaximize.gif"
                } else {
                    this.src = strResURL + "images/iwrestore.gif"
                }
            }
        }
        if (this.tabs.length > 1 && this.sizeState == "normal") {
            for (aw = 0; aw < this.tabs.length; aw++) {
                var ay = document.getElementById("VIWTab" + aw);
                ay.onclick = function () {
                    var aG = parseInt(this.id.substring(6, this.id.length));
                    Y.selectTab(aG)
                }
            }
        }
    }

    function p() {
        var K = document.getElementById(this.ll.id + "_InfoWindow");
        if (K == null) {
            return
        }
        var L = this.ll.fromLatLngToDivPixel(this.point);
        var ao = 32;
        var ag = -7;
        var ae = 0;
        var M = this.ll.lI.offsetWidth - 50;
        var Q = this.ll.lI.offsetHeight - 50;
        var al = Math.min(this.size.width, M);
        var ah = Math.min(this.size.height, Q);
        if (this.sizeState == "normal") {
            al = Math.max(al, 84 * this.tabs.length - 1 + 50)
        } else {
            al = Math.min(658, M);
            ah = Math.min(474, Q)
        }
        var z = al + ao;
        var O = ah + ao;
        if (this.sizeState == "normal") {
            z = Math.min(z, 690);
            O = Math.min(O, 506)
        }
        var aq = 97;
        var am = 96;
        var ai = (this.sizeState == "normal") ? O + am : O + 25;
        this.lll = z;
        this.Il1 = ai;
        var ab = z + 40,
            U = parseInt(ai * 0.5);
        ab = Math.min(ab, 740);
        U = Math.min(U, 298);
        var J = 323;
        var R = 1033;
        var B = 30;
        var C = 60;
        var X = 70;
        var I = 14;
        var A = 754;
        var aj = 310;
        var af = 345;
        var N = 140;
        var ad = 259;
        var H = 119;
        var ap = strResURL + "images/infowindowshade.png";
        var ac = ab - 2 * X;
        var Z = 0;
        var F = 0;
        var S = H;
        var aa = U - B - C;
        var x = J - aa;
        var y = X;
        var Y = ac - N + B;
        if (Y > 0) {
            if (Y >= 50) {
                Z = (Y - 50) / 2;
                F = Y - Z
            } else {
                Z = 0;
                F = Y
            }
        }
        if (aa >= N) {
            S = ad
        }
        var V = aa + B;
        var an = 0;
        var T = V + X;
        var G = an;
        var D = L.x - X - Z + ag + this.offset.width;
        var E = L.y - U + ae + this.offset.height;
        if (this.sizeState == "maximize") {
            E += 45
        }
        var W = K.childNodes[0];
        W.style.left = D + "px";
        W.style.top = E + "px";
        var ak = 0;
        W.childNodes[ak].style.left = V + "px";
        W.childNodes[ak].style.top = an + "px";
        ak++;
        if (ac > 0) {
            W.childNodes[ak].style.left = T + "px";
            W.childNodes[ak].style.top = "0px";
            ak++
        }
        W.childNodes[ak].style.left = (V + X + ac) + "px";
        W.childNodes[ak].style.top = "0px";
        ak++;
        W.childNodes[ak].style.left = (V - aa) + "px";
        W.childNodes[ak].style.top = B + "px";
        ak++;
        W.childNodes[ak].style.left = (V + X - aa) + "px";
        W.childNodes[ak].style.top = B + "px";
        ak++;
        W.childNodes[ak].style.left = (V - aa + y + ac) + "px";
        W.childNodes[ak].style.top = B + "px";
        ak++;
        W.childNodes[ak].style.left = (V - aa - B) + "px";
        W.childNodes[ak].style.top = (B + aa) + "px";
        ak++;
        if (Z > 0) {
            W.childNodes[ak].style.left = (V - aa - B + X) + "px";
            W.childNodes[ak].style.top = (B + aa) + "px";
            ak++
        }
        W.childNodes[ak].style.left = (V - aa - B + X + Z) + "px";
        W.childNodes[ak].style.top = (B + aa) + "px";
        ak++;
        if (F > 0) {
            W.childNodes[ak].style.left = (V - aa - B + X + N + Z) + "px";
            W.childNodes[ak].style.top = (B + aa) + "px";
            ak++
        }
        W.childNodes[ak].style.left = (V - aa + y + ac) + "px";
        W.childNodes[ak].style.top = (B + aa) + "px";
        ak++;
        J = 0;
        R = 665;
        B = 25;
        C = 25;
        X = 25;
        I = 0;
        A = 665;
        aj = 665;
        af = 345;
        aq = 97;
        am = 96;
        ad = 368;
        H = 0;
        ap = strResURL + "images/infowindow.png";
        D = parseInt(L.x + (aq - z) / 2) + ag + this.offset.width;
        E = parseInt(L.y - am - O + 25) + ae + this.offset.height;
        if (this.sizeState == "maximize") {
            E += 45
        }
        ac = ab - 2 * X;
        Z = 0;
        F = 0;
        S = H;
        aa = U - B - C;
        x = J - aa;
        y = X;
        Y = ac - aq + B;
        if (Y > 0) {
            if (Y >= 50) {
                Z = (Y - 50) / 2;
                F = Y - Z
            } else {
                F = Z;
                Z = 0
            }
        }
        if (aa >= aq) {
            S = ad
        }
        V = aa + B;
        an = 0;
        T = V + X;
        G = an;
        var P = K.childNodes[1];
        P.style.left = D + "px";
        P.style.top = E + "px"
    }

    function n(z) {
        var C = document.createElement("div");
        C.id = "divtemp";
        C.innerHTML = z;
        C.style.overflow = "visible";
        C.style.position = "absolute";
        document.body.appendChild(C);
        var D = C.offsetWidth;
        var B = C.offsetHeight;
        document.body.removeChild(C);
        var y = D,
            A = B;
        if (y < 180) {
            y = 180
        }
        if (A < 60) {
            A = 60
        }
        var x = new VSize(y, A);
        return x
    }

    function m() {
        var C = this.ll.lI.offsetWidth;
        var F = this.ll.lI.offsetHeight;
        var B = this.ll.fromLatLngToContainerPixel(this.point);
        var K = 0,
            I = 0,
            G = null;
        if (this.lll > C * 3 / 4 || this.Il1 > F * 3 / 4) {
            var z = this.ll.fromLatLngToDivPixel(this.ll.getCenter());
            B = this.ll.fromLatLngToDivPixel(this.point);
            var E = F / 2;
            K = B.x + 48 - z.x;
            I = B.y - z.y - ((this.sizeState == "maximize") ? 25 : 0);
            var x = Math.abs(F - this.Il1) / 2;
            if (E >= this.Il1) {
                I -= (x - (E - this.Il1))
            } else {
                I -= (x + (this.Il1 - E))
            }
            G = new VSize(K, I)
        } else {
            var J = B.x + 48.5 - this.lll / 2;
            var D = B.y - this.Il1;
            var H = J + this.lll;
            var L = D + this.Il1;
            var A = 20;
            if (document.getElementById(this.ll.id + "_VLargeMapControl") != null && D < 300) {
                A += 65
            }
            var y = 20;
            if (document.getElementById(this.ll.id + "_VOverviewMapControl") != null && L > F - 131) {
                y += 150
            }
            if (J < A) {
                K = J - A
            } else {
                if (H > C - y) {
                    K = H - C + y
                }
            } if (D < 25) {
                I = D - 25
            } else {
                if (L > F - 50) {
                    I = L - F + 50
                }
            }
            G = new VSize(K, I)
        }
        return G
    }

    function g() {
        return ((document.getElementById(this.ll.id + "_InfoWindow") != null) && (!this.lIi))
    }

    function e() {
        return this.lIi
    }

    function u(x, A, z, B, y) {
        if (x != null) {
            this.point = x
        }
        if (A != null) {
            this.tabs = A
        }
        if (z != null) {
            this.size = z
        }
        if (B != null) {
            this.offset = B
        }
        if (y != null) {
            this.selectedTab = y
        }
    }

    function b() {
        return this.point
    }

    function k() {
        return this.offset
    }

    function h() {
        return this.selectedTab
    }

    function i() {
        return this.tabs
    }

    function c() { }

    function a() {
        this.maximizable = true
    }

    function r() {
        this.maximizable = false
    }

    function j() {
        if (this.maximizable == false) {
            return
        }
        this.sizeState = "maximize";
        this.ll.hideControls();
        this.createInfoWindow();
        if (this.maximizeclickHandler != undefined) {
            this.maximizeclickHandler()
        }
        if (this.options.autoPan) {
            var y = this.getInfoWindowPan();
            this.ll.panBy(y);
            var x = this;
            setTimeout(function () {
                if (x.maximizeendHandler != undefined) {
                    x.maximizeendHandler()
                }
            }, 250)
        } else {
            if (this.maximizeendHandler != undefined) {
                this.maximizeendHandler()
            }
        }
    }

    function q() {
        if (this.maximizable == false) {
            return
        }
        this.sizeState = "normal";
        this.ll.showControls();
        this.createInfoWindow();
        if (this.restoreclickHandler != undefined) {
            this.restoreclickHandler()
        }
        if (this.restoreendHandler != undefined) {
            this.restoreendHandler()
        }
    }

    function d() {
        return "VInfoWindow"
    }
}

function VLargeMapControl(k, d) {
    this.printable = (k != undefined) ? k : false;
    this.II1 = (d != undefined) ? d : false;
    this.initialize = f;
    this.redraw = p;
    this.remove = e;
    this.getDefaultPosition = j;
    this.getIndicatorPos = a;
    this.getRulerMaxHeight = o;
    this.mouseMoveHandler = i;
    this.scrollWheelZoomHandler = n;
    this.llj = b;
    this.redrawRuler = c;
    this.updateLanguage = h;
    this.hide = g;
    this.show = m;
    this.vType = l;
    this.offsetY = 22;
    this.offsetTop = 79;
    this.rulerStep = 8;
    this.isMouseOver = false;

    function f(N, O) {
        if (isValidKey == false) {
            return
        }
        this.ll = N;
        var C = parseInt(N.lI.offsetWidth) / 2;
        var J = parseInt(N.lI.offsetHeight) / 2;
        var A = new Array();
        if (N.options.skin == "RedSkin") {
            A[A.length] = "<div class='move-control'>";
            A[A.length] = "<a class='clickcontrol top' id='" + N.id + "_btTop' title='B&#7855;c'></a>";
            A[A.length] = "<a class='clickcontrol right' id='" + N.id + "_btRight' title='&#272;&#244;ng'></a>";
            A[A.length] = "<a class='clickcontrol bot' id='" + N.id + "_btBottom' title='Nam'></a>";
            A[A.length] = "<a class='clickcontrol left' id='" + N.id + "_btLeft' title='T&#226;y'></a>";
            A[A.length] = "<a class='clickcontrol topright' id='" + N.id + "_btTopRight' title='&#272;&#244;ng B&#7855;c'></a>";
            A[A.length] = "<a class='clickcontrol botright' id='" + N.id + "_btBottomRight' title='&#272;&#244;ng Nam'></a>";
            A[A.length] = "<a class='clickcontrol botleft' id='" + N.id + "_btBottomLeft' title='T&#226;y Nam'></a>";
            A[A.length] = "<a class='clickcontrol topleft' id='" + N.id + "_btTopLeft' title='T&#226;y B&#7855;c'></a>";
            A[A.length] = "<a class='clickcontrol center' id='" + N.id + "_btFit' title='T&#226;m'></a>";
            A[A.length] = "</div>";
            A[A.length] = "<div class='zoom-control'>";
            A[A.length] = "<a class='clickcontrol zoom-in' id='" + N.id + "_btZoomIn' title='Ph&#243;ng to'></a>";
            A[A.length] = "<a class='clickcontrol center' id='" + N.id + "_rulerBar' style='height:" + ((N.maxLevel - N.minLevel) * 8) + "px;'><span></span></a>";
            A[A.length] = "<a class='clickcontrol zoom-out' id='" + N.id + "_btZoomOut' title='Thu nh&#7887;'></a>";
            A[A.length] = "<a class='dragruler drag' id='" + N.id + "_rulerDiv'></a>";
            A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barWard' style='left:25px; top:38px; display:none;'>";
            A[A.length] = "<div class='ZoomPinRight'>";
            A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "Ward" : "Ph&#432;&#7901;ng") + "</div>";
            A[A.length] = "</div>";
            A[A.length] = "</div>";
            A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barDistrict' style='left:25px; top:70px; display:none;'>";
            A[A.length] = "<div class='ZoomPinRight'>";
            A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "District" : "Qu&#7853;n") + "</div>";
            A[A.length] = "</div>";
            A[A.length] = "</div>";
            A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barCity' style='left:25px; top:102px; display:none;'>";
            A[A.length] = "<div class='ZoomPinRight'>";
            A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "City" : "Th&#224;nh ph&#7889;") + "</div>";
            A[A.length] = "</div>";
            A[A.length] = "</div>";
            A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barCountry' style='left:25px; top:134px; display:none;'>";
            A[A.length] = "<div class='ZoomPinRight'>";
            A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "Country" : "Qu&#7889;c gia") + "</div>";
            A[A.length] = "</div>";
            A[A.length] = "</div>";
            A[A.length] = "</div>"
        } else {
            if (N.options.skin == "TransSkin") {
                this.offsetY = 0;
                this.offsetTop = 95;
                this.rulerStep = 9;
                A[A.length] = "<div class='move-control'>";
                A[A.length] = "<a id='" + N.id + "_btTop' class='clickcontrol top'></a>";
                A[A.length] = "<a id='" + N.id + "_btRight' class='clickcontrol right'></a>";
                A[A.length] = "<a id='" + N.id + "_btBottom' class='clickcontrol bot'></a>";
                A[A.length] = "<a id='" + N.id + "_btLeft' class='clickcontrol left'></a>";
                A[A.length] = "<a id='" + N.id + "_btTopRight' class='clickcontrol topright'></a>";
                A[A.length] = "<a id='" + N.id + "_btBottomRight' class='clickcontrol botright'></a>";
                A[A.length] = "<a id='" + N.id + "_btBottomLeft' class='clickcontrol botleft'></a>";
                A[A.length] = "<a id='" + N.id + "_btTopLeft' class='clickcontrol topleft'></a>";
                A[A.length] = "<a id='" + N.id + "_btFit' class='clickcontrol center'></a>";
                A[A.length] = "</div>";
                A[A.length] = "<div class='zoom-control'>";
                A[A.length] = "<div class='zoombtns'>";
                A[A.length] = "<a id='" + N.id + "_btZoomIn' class='clickcontrol zoom-in btns'><span></span></a>";
                A[A.length] = "<a id='" + N.id + "_btZoomOut' class='clickcontrol zoom-out btns'><span></span></a>";
                A[A.length] = "</div>";
                A[A.length] = "<div id='" + N.id + "_rulerBar' class='clickcontrol zoomscroll' style='height:" + ((N.maxLevel - N.minLevel) * this.rulerStep + 8) + "px;'><a id='" + N.id + "_rulerDiv' class='dragruler drag'></a>";
                A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barWard' style='top:" + this.getIndicatorPos(17) + "px; display:none;'>";
                A[A.length] = "<div class='ZoomPinRight'>";
                A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "Ward" : "Ph&#432;&#7901;ng") + "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>";
                A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barDistrict' style='top:" + this.getIndicatorPos(13) + "px; display:none;'>";
                A[A.length] = "<div class='ZoomPinRight'>";
                A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "District" : "Qu&#7853;n") + "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>";
                A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barCity' style='top:" + this.getIndicatorPos(9) + "px; display:none;'>";
                A[A.length] = "<div class='ZoomPinRight'>";
                A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "City" : "Th&#224;nh ph&#7889;") + "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>";
                A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barCountry' style='top:" + this.getIndicatorPos(5) + "px; display:none;'>";
                A[A.length] = "<div class='ZoomPinRight'>";
                A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "Country" : "Qu&#7889;c gia") + "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>"
            } else {
                A[A.length] = "<div class='MapMove'>";
                A[A.length] = "<a class='clickcontrol MMTop' id='" + N.id + "_btTop' title='B&#7855;c'></a>";
                A[A.length] = "<a class='MMRight' id='" + N.id + "_btRight' title='&#272;&#244;ng'></a>";
                A[A.length] = "<a class='clickcontrol MMBot' id='" + N.id + "_btBottom' title='Nam'></a>";
                A[A.length] = "<a class='clickcontrol MMLeft' id='" + N.id + "_btLeft' title='T&#226;y'></a>";
                A[A.length] = "<a class='clickcontrol MMTopRight' id='" + N.id + "_btTopRight' title='&#272;&#244;ng B&#7855;c'></a>";
                A[A.length] = "<a class='clickcontrol MMBotRight' id='" + N.id + "_btBottomRight' title='&#272;&#244;ng Nam'></a>";
                A[A.length] = "<a class='clickcontrol MMBotLeft' id='" + N.id + "_btBottomLeft' title='T&#226;y Nam'></a>";
                A[A.length] = "<a class='clickcontrol MMTopLeft' id='" + N.id + "_btTopLeft' title='T&#226;y B&#7855;c'></a>";
                A[A.length] = "<a class='clickcontrol MMCenter' id='" + N.id + "_btFit' title='T&#226;m'></a>";
                A[A.length] = "</div>";
                A[A.length] = "<div class='MapZoom-top'>";
                A[A.length] = "<div class='MapZoom-bot'>";
                A[A.length] = "<div class='clickcontrol MapZoom-center' id='" + N.id + "_rulerBar' style='height:" + ((N.maxLevel - N.minLevel) * 8) + "px;'><span></span></div>";
                A[A.length] = "</div>";
                A[A.length] = "<a class='clickcontrol MZIn' id='" + N.id + "_btZoomIn' title='Ph&#243;ng to'></a>";
                A[A.length] = "<a class='clickcontrol MZOut' id='" + N.id + "_btZoomOut' title='Thu nh&#7887;'></a>";
                A[A.length] = "<div class='dragruler MZScroll' id='" + N.id + "_rulerDiv'></div>";
                A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barWard' style='left:25px; top:38px; display:none;'>";
                A[A.length] = "<div class='ZoomPinRight'>";
                A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "Ward" : "Ph&#432;&#7901;ng") + "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>";
                A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barDistrict' style='left:25px; top:70px; display:none;'>";
                A[A.length] = "<div class='ZoomPinRight'>";
                A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "District" : "Qu&#7853;n") + "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>";
                A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barCity' style='left:25px; top:102px; display:none;'>";
                A[A.length] = "<div class='ZoomPinRight'>";
                A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "City" : "Th&#224;nh ph&#7889;") + "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>";
                A[A.length] = "<div class='ZoomPinLeft' id='" + N.id + "_barCountry' style='left:25px; top:134px; display:none;'>";
                A[A.length] = "<div class='ZoomPinRight'>";
                A[A.length] = "<div class='clickcontrol ZoomPinCenter'>" + ((N.strLanguage == "English") ? "Country" : "Qu&#7889;c gia") + "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>";
                A[A.length] = "</div>"
            }
        }
        this.id = N.id + "_VLargeMapControl";
        var z = VUtil.createDivElement(N.id + "_VLargeMapControl", 0, 0, "60", "auto", "absolute", "", A.join(""));
        z.style.zIndex = ControlZIndex;
        z.style.MozUserSelect = "none";
        N.lI.appendChild(z);
        var v = function () {
            if (N.li1 != undefined) {
                N.li1()
            }
        };
        var u = document.getElementById(N.id + "_btZoomIn");
        var x = document.getElementById(N.id + "_btZoomOut");
        var M = document.getElementById(N.id + "_btFit");
        var G = document.getElementById(N.id + "_btLeft");
        var F = document.getElementById(N.id + "_btRight");
        var K = document.getElementById(N.id + "_btTop");
        var H = document.getElementById(N.id + "_btBottom");
        var B = document.getElementById(N.id + "_btTopRight");
        var t = document.getElementById(N.id + "_btBottomRight");
        var D = document.getElementById(N.id + "_btBottomLeft");
        var q = document.getElementById(N.id + "_btTopLeft");
        var s = document.getElementById(N.id + "_barWard");
        var E = document.getElementById(N.id + "_barDistrict");
        var L = document.getElementById(N.id + "_barCity");
        var y = document.getElementById(N.id + "_barCountry");
        var r = this;
        z.onmouseover = function () {
            if (N.minLevel <= 17 && 17 <= N.maxLevel) {
                s.style.display = ""
            }
            if (N.minLevel <= 13 && 13 <= N.maxLevel) {
                E.style.display = ""
            }
            if (N.minLevel <= 9 && 9 <= N.maxLevel) {
                L.style.display = ""
            }
            if (N.minLevel <= 5 && 5 <= N.maxLevel) {
                y.style.display = ""
            }
        };
        z.onmouseout = function () {
            s.style.display = "none";
            E.style.display = "none";
            L.style.display = "none";
            y.style.display = "none"
        };
        s.onclick = function (w) {
            N.zoomToLevel(17);
            N.stopEvent(w)
        };
        E.onclick = function (w) {
            N.zoomToLevel(13);
            N.stopEvent(w)
        };
        L.onclick = function (w) {
            N.zoomToLevel(9);
            N.stopEvent(w)
        };
        y.onclick = function (w) {
            N.zoomToLevel(5);
            N.stopEvent(w)
        };
        u.onclick = function () {
            N.zoomIn()
        };
        x.onclick = function () {
            N.zoomOut()
        };
        M.onclick = function () {
            N.fitOverlays()
        };
        G.onclick = function () {
            v();
            N.panBy(new VSize(-C, 0))
        };
        F.onclick = function () {
            v();
            N.panBy(new VSize(C, 0))
        };
        K.onclick = function () {
            v();
            N.panBy(new VSize(0, -J))
        };
        H.onclick = function () {
            v();
            N.panBy(new VSize(0, J))
        };
        B.onclick = function () {
            v();
            N.panBy(new VSize(C, -J))
        };
        t.onclick = function () {
            v();
            N.panBy(new VSize(C, J))
        };
        D.onclick = function () {
            v();
            N.panBy(new VSize(-C, J))
        };
        q.onclick = function () {
            v();
            N.panBy(new VSize(-C, -J))
        };
        document.getElementById(N.id + "_rulerBar").onclick = function (w) {
            r.llj(w)
        };
        this.lj = z;
        this.position = O;
        this.redraw()
    }

    function o() {
        return (this.ll.maxLevel - this.ll.minLevel) * this.rulerStep + this.offsetY
    }

    function a(q) {
        return this.getRulerMaxHeight() - ((q - this.ll.minLevel) * this.rulerStep + this.rulerStep / 2 - 1)
    }

    function i(r, q) {
        if (q < this.offsetY) {
            r.style.top = this.offsetY + "px"
        } else {
            if (q > (this.offsetY + (this.ll.maxLevel - this.ll.minLevel) * this.rulerStep)) {
                r.style.top = (this.offsetY + (this.ll.maxLevel - this.ll.minLevel) * this.rulerStep) + "px"
            } else {
                r.style.top = q + "px"
            }
        }
    }

    function n(u) {
        var q = document.getElementById(this.ll.id + "_rulerDiv");
        if (q != null) {
            var r = Math.ceil(this.ll.iZoomStep / 8);
            var t = (this.ll.iZoomStep % 8 == 0) ? 8 : this.ll.iZoomStep % 8;
            var s = 0;
            if (u > 0) {
                s = this.rulerStep * (this.ll.level - this.ll.minLevel + r - 1) + t * 1
            } else {
                s = this.rulerStep * (this.ll.level - this.ll.minLevel - r + 1) - t * 1
            }
            q.style.top = this.getRulerMaxHeight() - s + "px"
        }
    }

    function c() {
        var q = document.getElementById(this.ll.id + "_rulerDiv");
        if (q != null) {
            var r = this.rulerStep * (this.ll.level - this.ll.minLevel);
            q.style.top = this.getRulerMaxHeight() - r + "px"
        }
    }

    function h() {
        document.getElementById(map.id + "_barWard").childNodes[0].childNodes[0].innerHTML = (this.ll.strLanguage == "English") ? "Ward" : "Ph&#432;&#7901;ng";
        document.getElementById(map.id + "_barDistrict").childNodes[0].childNodes[0].innerHTML = (this.ll.strLanguage == "English") ? "District" : "Qu&#7853;n";
        document.getElementById(map.id + "_barCity").childNodes[0].childNodes[0].innerHTML = (this.ll.strLanguage == "English") ? "City" : "Th&#224;nh ph&#7889;";
        document.getElementById(map.id + "_barCountry").childNodes[0].childNodes[0].innerHTML = (this.ll.strLanguage == "English") ? "Country" : "Qu&#7889;c gia"
    }

    function b(s) {
        if (this.ll.zoomstartHandler != undefined) {
            this.ll.zoomstartHandler()
        }
        var u = lIl;
        var t = Math.round((this.getRulerMaxHeight() - (u - this.offsetTop - parseInt(this.ll.Top) - document.getElementById(this.ll.id + "_VLargeMapControl").offsetTop)) / this.rulerStep) + this.ll.minLevel;
        if (t < this.ll.minLevel) {
            t = this.ll.minLevel
        }
        if (t > this.ll.maxLevel) {
            t = maxLevel
        }
        var q = this.ll.getCenter();
        var r = this.ll.getInfoWindow();
        if (r.isOpened()) {
            q = this.ll.getCenterForPreserveInfoWindow(this.ll.level, t)
        }
        this.ll.setCenter(q, t);
        if (!document.all && this.ll.j1I == undefined && this.ll.llij) {
            this.ll.llij = false
        }
    }

    function p() {
        if (isValidKey == false) {
            return
        }
        if (this.position != undefined) {
            if (this.position.anchor == V_ANCHOR_TOP_RIGHT) {
                this.lj.style.left = parseInt(this.ll.lI.offsetWidth) - this.position.offset.width - 58 + "px";
                this.lj.style.top = this.position.offset.height + "px"
            } else {
                if (this.position.anchor == V_ANCHOR_TOP_LEFT) {
                    this.lj.style.left = this.position.offset.width + "px";
                    this.lj.style.top = this.position.offset.height + "px"
                } else {
                    if (this.position.anchor == V_ANCHOR_BOTTOM_RIGHT) {
                        this.lj.style.left = parseInt(this.ll.lI.offsetWidth) - this.position.offset.width - 58 + "px";
                        this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - this.position.offset.height - 258 + "px"
                    } else {
                        if (this.position.anchor == V_ANCHOR_BOTTOM_LEFT) {
                            this.lj.style.left = this.position.offset.width + "px";
                            this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - this.position.offset.height - 295 + "px"
                        }
                    }
                }
            }
        } else {
            this.lj.style.left = 10 + "px";
            this.lj.style.top = 10 + "px"
        }
    }

    function e() {
        VUtil.removeDivElement(this.id, this.ll.lI.id)
    }

    function j() {
        var q = new VControlPosition(V_ANCHOR_TOP_LEFT, new VSize(10, 10));
        return q
    }

    function g() {
        this.lj.style.visibility = "hidden"
    }

    function m() {
        this.lj.style.visibility = "visible"
    }

    function l() {
        return "VLargeMapControl"
    }
}
VLargeMapControl.prototype = new VControl();

function VClientGeocoder() {
    this.getLatLng = a;

    function a(b, c) {
        if (b == undefined || b == null || c == undefined || c == null) {
            return
        }
        
        if (VUtil.l1Il(b)) {
            VUtil.jl1(j1l + "?Address=" + b, "", c, "")
           
        }
    }
}
var vcur_direction;

function VDirections(s, l) {
    if (s != undefined) {
        s.i1i = this;
        this.ll = s;
        if (l != undefined) {
            l.ll = s
        }
    } else {
        this.ll = null;
        if (l != undefined) {
            l.ll = null
        }
    } if (l != undefined) {
        this.Panel = l
    } else {
        this.Panel = null
    }
    
    this.StartPoint = "";
    this.EndPoint = "";
    this.ObjMarkerClick = "";
    this.j11 = false;
    this.Distance = 0;
    this.Unit = "";
    this.l11j = true;
    this.l1I1 = true;
    this.PreserveViewportFlag = true;
    this.i11 = new Array();
    this.jli = new Array();
    this.Ijj = new Array();
    this.QueryOptions = new VDirectionsOptions(true, true, true);
    this.load = e;
    this.loadFromWayPoints = i;
    this.clear = p;
    this.getResult = f;
    this.getStatus = o;
    this.getBounds = c;
    this.getNumRoutes = k;
    this.getRoute = g;
    this.getNumGeocodes = j;
    this.getGeocode = r;
    this.getDistance = a;
    this.getDuration = t;
    this.getPolyline = n;
    this.getMarker = d;
    this.getSummaryHTML = m;
    this.showDirectionGuide = h;
    this.fitMap = q;
    this.vType = b;

    function b() {
        return "VDirections"
    }

    function e(x, u) {
        if (!VUtil.l1Il(x)) {
            return
        }
        if (this.j11 == true) {
            return
        }
        this.j11 = true;
        vcur_direction = this;
        this.QueryOptions = u;
        this.l11j = u.getPolyline;
        this.l1I1 = u.jj1;
        this.PreserveViewportFlag = u.preserveViewport;
        this.Ijj = x.split("To:");
        this.Ijj[0] = this.Ijj[0].substring(x.indexOf("From:") + 5);
        var v = (u.avoidAlley == true) ? 1 : 0;
        var w = (u.avoidRoadConstruction == true) ? 1 : 0;
        VUtil.jl1(j1l + "?option=0," + v + "," + w + "&Findstring=" + x, "", "vcur_direction.getResult", "")
    }

    function i(z, u) {
        if (this.j11 == true) {
            return
        }
        this.j11 = true;
        vcur_direction = this;
        this.QueryOptions = u;
        var v, y = "";
        for (v = 0; v < z.length; v++) {
            y += z[v].longitude + "," + z[v].latitude;
            this.Ijj.push(z[v].latitude + "," + z[v].longitude);
            if (v < z.length - 1) {
                y += ","
            }
        }
        this.l11j = u.getPolyline;
        this.l1I1 = u.jj1;
        this.PreserveViewportFlag = u.preserveViewport;
        var w = (u.avoidAlley == true) ? 1 : 0;
        var x = (u.avoidRoadConstruction == true) ? 1 : 0;
        VUtil.jl1(j1l + "?option=1," + w + "," + x + "&Findstring=" + y, "", "vcur_direction.getResult", "")
    }

    function p() {
        if (this.ll != null) {
            var v;
            for (v = 0; v < this.getNumRoutes(); v++) {
                this.ll.removeOverlay(this.i11[v].llII);
                this.ll.removeOverlay(this.jli[v])
            }
            if (v > 0) {
                this.ll.removeOverlay(this.jli[v])
            }
            this.ll.i1i = null
        }
        this.i11 = new Array();
        this.jli = new Array();
        this.Ijj = new Array();
        if (this.Panel != null) {
            this.Panel.innerHTML = ""
        }
        if (this.ll != null) {
            l1 = document.getElementById("PopUp");
            var u = document.getElementById(this.ll.id + "_path");
            if (l1 != null) {
                u.removeChild(l1)
            }
        }
    }

    function f(x) {
        var y = this.Ijj;
        this.clear();
        this.Ijj = y;
        this.Distance = x.ShortestPath.fulllength;
        var w = x.FullPath.split(",");
        var v = new Array();
        v[0] = new Array();
        var C, B, D = 0;
        for (C = 0; C < w.length; C += 2) {
            if (parseFloat(w[C]) <= DB_MIN) {
                D++;
                v[D] = new Array()
            } else {
                var u = new VLatLng(parseFloat(w[C + 1]), parseFloat(w[C]), true);
                v[D].push(u)
            }
        }
        this.Unit = x.ShortestPath.unit;
        var E, F, z, G, y, A;
        for (C = 0; C < x.ShortestPath.Section.length; C++) {
            z = new VRoute();
            z.Distance = x.ShortestPath.Section[C].Length.value;
            z.Unit = x.ShortestPath.Section[C].Length.unit;
            if (this.Panel != null || this.QueryOptions.jj1 == true) {
                for (B = 0; B < x.ShortestPath.Section[C].Segment.length; B++) {
                    y = x.ShortestPath.Section[C].Segment[B];
                    G = new VStep();
                    G.Distance = y.Length.value;
                    G.Unit = y.Length.unit;
                    G.Name = y.Name;
                    G.i1i = y.direction;
                    G.StartAt = new VLatLng(y.StartAt.Y, y.StartAt.X, true);
                    G.StartIndex = y.start;
                    G.EndIndex = y.end;
                    z.iil.push(G)
                }
            }
            A = new VPolyline(v[C], "green", 2, "green", 1, null);
            if (this.ll != null || this.QueryOptions.getPolyline == true) {
                z.llII = A
               if( this.ll != null) // chỉ add các polyline mà đối tượng map tồn tại
               { 
                    // Add Polyline to clear when search again
                    VPolylines.push(A);
                }
            }
            if (this.ll != null) {
                this.ll.addOverlay(A)
            }
            //Not display Icon 
//            E = new VIcon(strResURL + "images/place_map_" + C + ".gif");
//            E.iconSize = new VSize(26, 29);
//            F = new VMarker(v[C][0], E);
//            if (this.ll != null) {
//                this.ll.addOverlay(F)
//            }
//            this.jli.push(F);
            this.i11.push(z)
        }
        //Not display Icon 
//        E = new VIcon(strResURL + "images/place_map_" + C + ".gif");
//        E.iconSize = new VSize(26, 29);
//        F = new VMarker(v[C - 1][v[C - 1].length - 1], E);
//        if (this.ll != null) {
//            this.ll.addOverlay(F)
//        }
//        this.jli.push(F);
        if (!this.QueryOptions.preserveViewport && this.ll != null) {
            this.fitMap()
        }
        if (this.Panel != null) {
            this.showDirectionGuide()
        }
        this.j11 = false;
        if (this.iIi != undefined) {
            this.iIi()
        }
    }

    function o() {
        return this.j11
    }

    function c() {
        var u = 0,
            D = 0,
            x = 0,
            w = 0;
        var A, z, y, v, C, B;
        for (A = 0; A < this.i11.length; A++) {
            y = new Array();
            v = this.i11[A].llII;
            if (v != null) {
                y = v.latlngs;
                for (z = 0; z < y.length; z++) {
                    C = parseFloat(y[z].longitude.toString());
                    B = parseFloat(y[z].latitude.toString());
                    if (u == 0 && D == 0 && x == 0 && w == 0) {
                        u = C;
                        x = C;
                        D = B;
                        w = B
                    }
                    if (u < C) {
                        u = C
                    }
                    if (x > C) {
                        x = C
                    }
                    if (D < B) {
                        D = B
                    }
                    if (w > B) {
                        w = B
                    }
                }
            }
        }
        return new VLatLngBounds(new VLatLng(u, w), new VLatLng(x, D))
    }

    function q() {
        var E = this.getBounds();
        var u = E.getNorthWest().lat();
        var I = E.getSouthEast().lng();
        var z = E.getSouthEast().lat();
        var w = E.getNorthWest().lng();
        var v = new VLatLng(parseFloat((I + w) / 2), parseFloat((u + z) / 2));
        var y = VProjection.fromLatLngToPixel(new VLatLng(I, u), this.ll.maxLevel);
        var C = y.x;
        var A = y.y;
        y = VProjection.fromLatLngToPixel(new VLatLng(w, z), this.ll.maxLevel);
        var H = y.x;
        var G = y.y;
        var D = Math.abs((C - H) / this.ll.lI.offsetWidth) > Math.abs((A - G) / this.ll.lI.offsetHeight) ? Math.abs((C - H) / this.ll.lI.offsetWidth) : Math.abs((A - G) / this.ll.lI.offsetHeight);
        var x = 0;
        for (x = 0; x < this.ll.IlI - 1; x++) {
            if (Math.pow(2, x) > D) {
                break
            }
        }
        var F = this.ll.level;
        this.ll.level = this.ll.maxLevel - x;
        this.ll.setCenter(v, this.ll.level);
        if (this.ll.lij != undefined) {
            for (var B = 0; B < this.ll.lij.length; B++) {
                this.ll.lij[B]()
            }
            return
        }
        if (this.ll.jjI != undefined) {
            this.ll.jjI();
            return
        }
        if (this.ll.j1I != undefined) {
            this.ll.j1I(F, this.ll.level)
        }
    }

    function k() {
        return this.i11.length
    }

    function g(u) {
        return this.i11[u]
    }

    function j() { }

    function r(u) { }

    function a() {
        return (this.Distance + " " + this.Unit)
    }

    function t() { }

    function n() {
        return this
    }

    function d(u) {
        return this.jli[u]
    }

    function m() {
        return this.Panel.innerHTML
    }
    
    

    //P5s format display resut route planning 
    function h() {
        var x, w, z, v, y;
        var u = new Array();    
                          
            
    
  
        u[u.length] = '<div id="findpath_div"  class="bodyLeftPanel" style="width: {WIDTH_FINDPATH};{OVER_FLOW}position: relative;">';
        
       //set tổng chiều dài toàn route
        u[u.length] = '<br/>';
        u[u.length] = '<table style="font-family: Tahoma; font-size: 11px; text-align: left;"';
        u[u.length] = 'cellspacing="0" border="0" width="100%">';
        u[u.length] = '<tr style=" border-top: 1px solid #d9d9d9;';
        u[u.length] = 'border-bottom-width: thin;">';
        u[u.length] = '<td  style="white-space: nowrap; vertical-align: middle;width:1%">&nbsp'
        u[u.length] = '</td>';
        u[u.length] = '<td  style="white-space: nowrap; vertical-align: middle;text-align: left;width:1%">';        
        u[u.length] = ' </td> ';
        u[u.length] = '<td colspan="2" style="white-space: nowrap; vertical-align: middle;text-align: left;width:1%">';
        u[u.length] = '<b style="vertical-align: text-top;text-align: left;"> Tổng chiều dài :  P5sTotalDistanceRoute km </b> &nbsp;&nbsp;{PRINTER}';
        u[u.length] = '</td>  <td style="white-space: nowrap; vertical-align: middle;width:96%">  </td></tr></table>';


        var totalDistanceRoute = 0;
        for (x = 0; x < this.i11.length; x++) {
            
            var vIcon ;
            if( x == 0)
            {   
                if(markerStart != undefined)
                {
                    vIcon = markerStart.getIcon(); //set New ICon not use system ICon
                }
                else
                     if(arrPathImg != undefined)
                     { 
                        
                        vIcon = new VIcon();
                        vIcon.image = arrPathImg[0];
                     }
                
                z = this.i11[x];
            }
            else
            {
                 if(markerStart != undefined)
                 {
                   vIcon = markerPoints[x-1].getIcon();  //set New ICon not use system ICon
                 } 
                 else
                     if(arrPathImg != undefined)
                     {
                         vIcon = new VIcon();
                         vIcon.image = arrPathImg[x];
                     }
              
                z = this.i11[x];
            }
            
            
            u[u.length] = '<div id="listDirection"  style="width: {WIDTH_DIRECTION}">';
            u[u.length] = '<div id="direction'+ x + '" class="direction-item">';
            u[u.length] = '<table style="font-family: Tahoma; font-size: 11px; text-align: left;"';
            u[u.length] = 'cellspacing="0" border="0" width="100%">';
            u[u.length] = '<tr style=" border-top: 1px solid #d9d9d9;';
            u[u.length] = 'border-bottom-width: thin;">';
            u[u.length] = '<td  style="white-space: nowrap; vertical-align: middle;width:1%">&nbsp'
            u[u.length] = '</td>';
            u[u.length] = '<td  style="white-space: nowrap; vertical-align: middle;width:1%">';
            u[u.length] = '<img alt="" src="' + vIcon.image +'"/>';
            u[u.length] = ' </td> ';
            u[u.length] = '<td colspan="2" style="white-space: nowrap; vertical-align: middle">';
            u[u.length] = '<b style="vertical-align: text-top"> ' +  this.Ijj[x]  + ' </b>' ;
            u[u.length] = '</td> <td style="white-space: nowrap; vertical-align: middle;width:96%;text-align: left;"> <img alt="" src="/icon/bullet_blue_collapse.png" style="padding-right:40px;cursor:pointer;" onclick="CloseDirection(\'guide' + x  + '\')" />  </td> </tr></table>';             
            
            u[u.length] = '<div id="guide'+ x +'" class="direction-guide" >';  
            u[u.length] = '<ol class="stepsList" style="background:rgb(240,240,240);" >';

           
            for (w = 0; w < z.iil.length; w++) {
                v = z.iil[w];
                y = v.i1i;   
                
                v.Name = v.Name.replace("Vịng","Vòng");    
                y = y.replace("Head","Đi thẳng");
                y = y.replace("Turn left on","Rẽ trái vào");                
                y = y.replace("Turn right on","Rẽ phải vào");
                y = y.replace("Pass over","Qua");
                y = y.replace("Continue to follow","Đi tiếp");
                y = y.replace("Pass over","Qua");
                y = y.replace("Turn to","Rẽ vào");
                y = y.replace("At","Rẽ phải vào");
                         
                u[u.length] = '<li onclick="{showPathDetail(' + v.StartAt.latitude + "," + v.StartAt.longitude + ',\'' + y + '\',\'' + v.Name.replace("Đường Khơng ","Đường không") + '\',\'' + v.Distance +'\',\'' + v.Unit + '\');}" onmouseover="OverDirectionGuide(this)" onmouseout=OutDirectionGuide(this) class="">' ;
                u[u.length] = '<a>' + (w + 1) + '</a>';
                u[u.length] = '<span class="instruction">';
                
                u[u.length] = '<span> ' + y + '  <span class="instructionKeyword"> ' +  v.Name.replace("Đường Khơng ","Đường không")  +  '</span> </span> </span>   <span class="distance"> ' +  v.Distance  +' ' + v.Unit + '</span> </li>     ';

             
            }
            
            u[u.length] = '</ol>';  
            u[u.length] = '<div style="text-align:right;font-family: Tahoma; font-size: 12px; float: right;padding-right:30px;" >Chiều dài: <b> '+  eval(z.Distance).toFixed(2)  +'</b> ' + z.Unit + ' </div>';
            u[u.length] = '</div>';

            if (z.Unit == "m")
                totalDistanceRoute += eval(z.Distance)/1000;
            else
                totalDistanceRoute += eval(z.Distance);
        
        }
        
        var vIcon ; //set New ICon not use system ICon  
        if(markerEnd != undefined)
        {
            vIcon = markerEnd.getIcon();
        }
        else
             if(arrPathImg != undefined)
             { 
                 vIcon = new VIcon();
                 vIcon.image = arrPathImg[arrPathImg.length-1];
             }
                   
        var titleEnd = "";
        
         if(markerEnd != undefined)
        {
           titleEnd =   FormatStrBr(markerEnd.objectCommunes.TitleDirection);
        }
        else
             if(arrTitle != undefined)
             { 
                titleEnd = arrTitle[arrTitle.length -1];
             }
                     
        u[u.length] = '<div id="listDirection">';
        u[u.length] = '<div id="direction'+ x + '" class="direction-item">';
        u[u.length] = '<table style="font-family: Tahoma; font-size: 11px; text-align: left;"';
        u[u.length] = 'cellspacing="0" border="0" width="100%">';
        u[u.length] = '<tr style=" border-top: 1px solid #d9d9d9;';
        u[u.length] = 'border-bottom-width: thin;">';
        u[u.length] = '<td  style="white-space: nowrap; vertical-align: middle;width:1%">&nbsp'
        u[u.length] = '</td>';
        u[u.length] = '<td  style="white-space: nowrap; vertical-align: middle;text-align: left;width:1%">';
        u[u.length] = '<img alt="" src="' + vIcon.image +'"/>';
        u[u.length] = ' </td> ';
        u[u.length] = '<td colspan="2" style="white-space: nowrap; vertical-align: middle;text-align: left;width:1%">';
        u[u.length] = '<b style="vertical-align: text-top;text-align: left;"> ' +  titleEnd + ' </b>';
        u[u.length] = '</td>  <td style="white-space: nowrap; vertical-align: middle;width:96%"> &nbsp; </td></tr></table>';       
        u[u.length] = '</div>';
        
    
        var strPrinter = '<img alt="" src="icon/printer.png" style="cursor:pointer;" onclick="PrintResult()" />';
        var strOverFlow = 'overflow: auto;';
        var strWidthFindPath = '295px';
        var strWidthDirection = '270px';
        
        if( typeof arrTitle != 'undefined')
        {  
             strPrinter = "";
             strOverFlow = "";  
             strWidthFindPath = "100%";  
             strWidthDirection = "100%";
                 
        }
        
        this.Panel.innerHTML = u.join("").replace("P5sTotalDistanceRoute", totalDistanceRoute.toFixed(2)).replace("{OVER_FLOW}",strOverFlow).replace("{PRINTER}",strPrinter).replace("{WIDTH_FINDPATH}",strWidthFindPath).replace("{WIDTH_DIRECTION}",strWidthDirection);

        if(document.getElementById("findpath_div") != null &&  typeof arrPathImg == 'undefined' )
               document.getElementById("findpath_div").style.height = ( $(window).height()-80 - 330)  + "px";;
  
    }
}

//P5s
var currentElement = null;
function OverDirectionGuide(a) {
 
    if(currentElement != a)
    {
        $(a).addClass("StepList-hover");
         currentElement = a;
    }
}
function OutDirectionGuide(a) {
    $(a).removeClass("StepList-hover");
    currentElement = null;
}

function CloseDirection(id) {
    
    var e = document.getElementById(id);
    if(e.style.display == "none")   
      e.style.display = "";    
    else
      e.style.display = "none";  
 
}



//P5s Display marker when user click on route 
var mkOP = null;
function showPathDetail(f, a, e1,e2,e3,e4) {     
     
    var g = new VLatLng(f, a);
    var l = map.getLevel();
    var e = new VIcon("/icon/blink2.gif");
    e.iconSize = new VSize(11, 11);
    
    var c = new VMarkerOptions(e, null, new VSize(5, 5));    
    c.offset = new VSize(5, 5);
    
    if (mkOP != null) {
        map.removeOverlay(mkOP);
        mkOP = null
    }
    
    var result = e1 + " " + e2 + " - đi " + e3 + " " + e4;   
    
    mkOP = new VMarker(g, c);   
    map.addOverlay(mkOP);
    mkOP.openInfoWindow(result);
}


var ll1i = "";
var jlj = false;
var isValidKey = true;
var jii = null;
var DB_MIN = -1.7976931348623157;
var vbdIsdragging = false;
var vbdIsSelectInput = false;
var lI1 = -1,
    lIl = -1;
var browserName = VUtil.getBrowserName();
var __userAgent = navigator.userAgent;
var __isIE = navigator.appVersion.match(/MSIE/) != null;
var __IEVersion = VUtil.getIEVersion();
var __isIENew = __isIE && __IEVersion >= 8;
var __isIEOld = __isIE && !__isIENew;
var __isFireFox = __userAgent.match(/firefox/i) != null;
var __isFireFoxOld = __isFireFox && ((__userAgent.match(/firefox\/2./i) != null) || (__userAgent.match(/firefox\/1./i) != null));
var __isFireFoxNew = __isFireFox && !__isFireFoxOld;
var __isWebKit = navigator.appVersion.match(/WebKit/) != null;
var __isChrome = navigator.appVersion.match(/Chrome/) != null;
var __isOpera = window.opera != null;
var __operaVersion = VUtil.getOperaVersion();
var __isOperaOld = __isOpera && (__operaVersion < 10);
var LimitFolder = 10000;
var V_NORMAL_MAP = null;
var V_ANCHOR_TOP_RIGHT = "V_ANCHOR_TOP_RIGHT";
var V_ANCHOR_CENTER_RIGHT = "V_ANCHOR_CENTER_RIGHT";
var V_ANCHOR_BOTTOM_RIGHT = "V_ANCHOR_BOTTOM_RIGHT";
var V_ANCHOR_BOTTOM_CENTER = "V_ANCHOR_BOTTOM_CENTER";
var V_ANCHOR_BOTTOM_LEFT = "V_ANCHOR_BOTTOM_LEFT";
var V_ANCHOR_CENTER_LEFT = "V_ANCHOR_CENTER_LEFT";
var V_ANCHOR_TOP_LEFT = "V_ANCHOR_TOP_LEFT";
var V_ANCHOR_TOP_CENTER = "V_ANCHOR_TOP_CENTER";
var V_ANCHOR_CENTER_CENTER = "V_ANCHOR_CENTER_CENTER";
var MapAnimatedZIndex = 10;
var MapInnerZIndex = 20;
var MapLayerZIndex = 30;
var MapPathZIndex = 40;
var LineZIndex = 44;
var MarkerZIndex = 45;
var MarkerActiveZIndex = 46;
var InfoWindowShadeZIndex = 50;
var ObjBelowShadeZIndex = 51;
var InfoWindowZIndex = 52;
var ControlZIndex = 60;
var VBDLogoZIndex = 80;
var TopZIndex = 100;
var ll1j = (VUtil.isVBDWebsite()) ? "http://images.vietbando.com/mapimagesws/" : "http://images.vietbando.com/mapimagesws/";
var j1l = ll1j + "mapforipa%2Easpx";
var strResURL = ll1j + "API/Skins/WhiteSkin/";
var V_DEFAULT_ICON = strResURL + "images/point-1.gif";
var vbdtempimage = new Image();
vbdtempimage.src = strResURL + "images/whiteimage.png";
var vbdtransimage = new Image();
vbdtransimage.src = strResURL + "images/transparent.png";
var vbdiwimage = new Image();
vbdiwimage.src = strResURL + "images/infowindow.png";
var vbdiwsimage = new Image();
vbdiwsimage.src = strResURL + "images/infowindowshade.png";
var CountRequest = 0;
var g_REtimerID = null;
var g_REdelay = 30000;
var ScriptQuery = function (a) {
    this.scriptPath = a
};
ScriptQuery.prototype = {
    get: function () {
        var d = new RegExp(this.scriptPath.replace(".", "\\.") + "(\\?.*)?$");
        var a = document.getElementsByTagName("script");
        for (var c = 0; c < a.length; c++) {
            var b = a[c];
            if (b.src && b.src.match(d)) {
                var e = b.src.match(/\?([^#]*)(#.*)?/);
                return !e ? "" : e[1]
            }
        }
        return ""
    },
    parse: function () {
        var k = {};
        var g = this.get();
        var d = g.split("&");
        for (var c = 0; c < d.length; c++) {
            var b = new Array();
            b[0] = d[c].substring(0, d[c].indexOf("="));
            b[1] = d[c].substring(d[c].indexOf("=") + 1, d[c].length);
            var a = b[0],
                h = b[1];
            if (!k[a]) {
                k[a] = []
            }
            if (!h) {
                h = "true"
            } else {
                try {
                    h = decodeURIComponent(h)
                } catch (f) {
                    h = unescape(h)
                }
            }
            var j = k[a];
            j[j.length] = h
        }
        return k
    },
    flatten: function () {
        var b = this.parse();
        for (var a in b) {
            b[a] = b[a][0]
        }
        return b
    },
    toString: function () {
        return "ScriptQuery [path=" + this.scriptPath + "]"
    }
};

function VBrowserIsCompatible() {
    if (browserName == "msie" || browserName == "firefox" || browserName == "mozilla" || browserName == "opera" || browserName == "safari" || browserName == "iPhone" || browserName == "iPod" || browserName == "iPad" || browserName == "chrome") {
        return true
    }
    return false
}

function VMap(aP, bk) {
    jii = this;
    var aa = "";
    var F = document.getElementsByTagName("script");
    var aM = new ScriptQuery("VBDMapAPI.js").parse();
    for (var ar in aM) {
        aa += aM[ar]
    }
    while (aa.indexOf("+") > -1) {
        aa = aa.replace("+", "!?!")
    }
    ll1i = aa;
    if (ll1i.length < 1 || ll1i == "true") {
        return
    }
    if (jlj == false) {
        VUtil.jl1(j1l, "", "vcallbackFunction", "")
    }
    if (typeof aP == "string") {
        aP = document.getElementById(aP)
    }
    this.ijj = false;
    this.isZooming = false;
    this.lllj = false;
    this.ll1I = false;
    this.llIi = false;
    this.jjj = false;
    this.isMarkerAutoPanning = false;
    this.isTextDragging = false;
    this.isSelectWindowContent = false;
    this.Iij = false;
    this.hasDraggedText = false;
    this.llij = false;
    this.l1li = false;
    this.draggable = true;
    this.ll1l = true;
    this.jij = true;
    this.dblClickZoomable = true;
    this.llIj = false;
    this.bEnableInfoWindow = true;
    this.bMapActive = false;
    this.strLanguage = "Vietnamese";
    this.centerX = 109.649506;
    this.centerY = 14.102783;
    this.boundsRestrict = null;
    this.ptBdResNWV = null;
    this.ptBdResSEV = null;
    this.prevStartX = 0;
    this.llI = 0;
    this.jl = 0;
    this.divisorX = 0;
    this.divisorY = 0;
    this.rootGestureX = null;
    this.rootGestureY = null;
    this.curGestureX = null;
    this.curGestureY = null;
    this.level = 5;
    this.IlI = 20;
    this.minLevel = 0;
    this.maxLevel = 19;
    this.lIj = 1;
    this.liI = 0;
    this.l1j = 0;
    this.prevRightClickTime = 0;
    this.nInstalment = 0;
    this.arrMaxRowCol = new Array();
    this.arrMaxRowCol[0] = 1;
    for (var Y = 1; Y <= this.maxLevel; Y++) {
        this.arrMaxRowCol[Y] = 2 << (Y - 1)
    }
    if (bk != undefined) {
        this.options = bk;
        strResURL = ll1j + "API/Skins/" + this.options.skin + "/";
        V_DEFAULT_ICON = strResURL + "images/point-1.gif";
        this.options.draggableCursor = strResURL + "images/openhand.cur";
        this.options.draggingCursor = strResURL + "images/closedhand.cur";
        if (bk.size != undefined) {
            aP.style.width = bk.size.width + "px";
            aP.style.height = bk.size.height + "px"
        } else {
            this.options.size = new VSize(aP.offsetWidth, aP.offsetHeight)
        }
    } else {
        this.options = new VMapOptions("WhiteSkin", new VSize(aP.offsetWidth, aP.offsetHeight))
    } if (aP.offsetWidth > 2000 || aP.offsetHeight > 2000) {
        jlj = false;
        isValidKey = false;
        jii.disableAll();
        return
    }
    this.llIl = false;
    this.ij1 = 8;
    this.nZoomCount = 0;
    this.iZoomStep = 0;
    this.prevDelta = 0;
    this.curMappingLayer = null;
    this.nMapTypes = 0;
    this.arrMapTypes = new Array();
    this.jI1 = 0;
    this.lli = new Array();
    this.Ill = 0;
    this.arrControls = new Array();
    this.nControls = 0;
    this.llI1 = null;
    this.i1I = new VInfoWindow();
    this.i1I.ll = this;
    this.IiI = null;
    this.i1i = null;
    this.ImagesURL = ll1j + "mapimageservice.ashx";
    this.Left = 0;
    this.Top = 0;
    this.id = aP.id;
    this.lI = aP;
    aP.ij = this;
    aP.dblRC = "";
    if (aP.style.position == undefined || aP.style.position == "") {
        aP.style.position = "absolute"
    }
    VUtil.includeCSS("APIStyle.css");
    this.stopEvent = y;
    this.vType = Z;
    this.disableAll = ba;
    this.calculatePosition = m;
    this.initMap = bq;
    this.setLanguage = a0;
    this.getCurrentLayerColRows = be;
    this.updateMappingAreas = ax;
    this.resetSrcImgOverlay = X;
    this.getVbdMapsTileUrl = T;
    this.insertLeft = H;
    this.insertRight = a;
    this.insertTop = s;
    this.insertBottom = Q;
    this.overViewInsertLeft = ay;
    this.overViewInsertRight = am;
    this.overViewInsertTop = e;
    this.overViewInsertBottom = ah;
    this.setOverViewCenter = aS;
    this.fixOverView = b;
    this.updateCurrentXY = az;
    this.getGestureCenter = a9;
    this.getRealCenter = bo;
    this.mouseWheelHandler = ab;
    this.prepareScrollZoom = bl;
    this.scrollWheelZoom = aB;
    this.mouseDownHandler = j;
    this.mouseMoveHandler = bp;
    this.mouseUpHandler = D;
    this.gestureStartHandler = A;
    this.gestureChangeHandler = ac;
    this.gestureEndHandler = E;
    this.hideOverlays = x;
    this.showOverlays = aK;
    this.redrawLines = aN;
    this.redrawRuler = bn;
    this.redrawScaleCtrl = a6;
    this.redrawOverviewMap = aH;
    this.resetSVG = B;
    this.getCenterForPreserveInfoWindow = bu;
    this.hideControls = aQ;
    this.showControls = t;
    this.panByMarker = aD;
    this.enableDragging = r;
    this.disableDragging = ae;
    this.draggingEnabled = G;
    this.enableInfoWindow = I;
    this.disableInfoWindow = af;
    this.infoWindowEnabled = ak;
    this.enableDoubleClickZoom = g;
    this.disableDoubleClickZoom = N;
    this.doubleClickZoomEnabled = bt;
    this.enableContinuousZoom = aw;
    this.disableContinuousZoom = bf;
    this.continuousZoomEnabled = O;
    this.enableScrollWheelZoom = aW;
    this.disableScrollWheelZoom = bh;
    this.scrollWheelZoomEnabled = aL;
    this.addControl = K;
    this.removeControl = c;
    this.clearControls = ad;
    this.getContainer = an;
    this.addMapType = aV;
    this.removeMapType = ag;
    this.setMappingLayer = n;
    this.isLoaded = bj;
    this.getCurrentPointer = aY;
    this.getCenter = P;
    this.getBounds = a4;
    this.getDirectionBounds = l;
    this.getBoundsZoomLevel = aj;
    this.getBoundsAtCenterLevel = br;
    this.getSize = p;
    this.getLevel = a5;
    this.getDragObject = ao;
    this.refreshLayer = a2;
    this.setCenter = bm;
    this.panBy = av;
    this.processPanBy = u;
    this.panTo = aT;
    this.panDirection = k;
    this.restrictBounds = h;
    this.restrictZoom = aC;
    this.zoomIn = z;
    this.zoomOut = W;
    this.zoomFit = aF;
    this.zoomToLevel = aJ;
    this.saveCurrentPosition = bs;
    this.goToSavedPosition = a7;
    this.fitOverlays = aZ;
    this.fitMultiOverlays = aU;
    this.checkResize = aE;
    this.addOverlay = J;
    this.removeOverlay = bg;
    this.clearOverlays = d;
    this.getOverlayContainer = bc;
    this.openInfoWindow = f;
    this.openInfoWindowTabs = a3;
    this.showMapBlowup = v;
    this.updateInfoWindow = al;
    this.closeInfoWindow = au;
    this.getInfoWindow = U;
    this.fromLLsToDPxs = a8;
    this.fromLatLngToDivPixel = o;
    this.fromLatLngToContainerPixel = S;
    this.fromDivPixelToLatLng = aX;
    this.fromContainerPixelToLatLng = M;
    this.fromContainerPixelToDivPixel = a1;
    this.fromDivPixelToContainerPixel = w;
    this.fromHighLevelPixelToDivPixel = ai;
    if (aP == null) {
        alert("Parent Node is not exist");
        return
    }
    aP.style.overflow = "hidden";
    aP.style.cursor = "url('" + this.options.draggableCursor + "'),auto";
    aP.style.background = "#EDEAE2";
    this.calculatePosition();
    if (browserName == "msie") {
        VUtil.addVMLStyle()
    }
    var V = new VTileLayer(new VCopyrightCollection("Vietbando Inc"), this.minLevel, this.maxLevel);
    V.getTileUrl = function (i, bv) {
        if (bv < this.minLevel || bv > this.maxLevel) {
            return ""
        }
        if (V.tileloadHandler != undefined) {
            V.tileloadHandler(i, bv)
        }
        return aP.ij.getVbdMapsTileUrl(bv, i.x, i.y)
    };
    var aR = [V];
    V_NORMAL_MAP = new VMapType(aR, VProjection, "VIETBANDO NORMAL MAP");
    this.addMapType(V_NORMAL_MAP);
    this.curMapType = V_NORMAL_MAP;
    var aI = this;
    var q = function (i, bv, bw) {
        VEvent.addDOMListener(i, bv, function () {
            bw(i.II)
        })
    };
    VMappingLayer.prototype.setMappingAreas = function (bx, bC) {
        var bz = null;
        var bv = null;
        for (var bw = 0; bw < bC.length; bw++) {
            var bB = document.getElementById(bx[bw]).useMap;
            bz = document.getElementById(bB.replace("#", ""));
            bz.innerHTML = "";
            bv = null;
            if (typeof bC[bw] == "object") {
                for (var bA = 0; bA < bC[bw].length; bA++) {
                    bv = document.createElement("area");
                    bv.shape = bC[bw][bA].shape;
                    bv.coords = bC[bw][bA].coords;
                    bv.title = bC[bw][bA].title;
                    bv.style.cursor = "pointer";
                    bv.II = bC[bw][bA];
                    bz.appendChild(bv);
                    for (var by = 0; by < aI.curMappingLayer.arrEvents.length; by++) {
                        q(bv, aI.curMappingLayer.arrEvents[by], aI.curMappingLayer.arrHandlers[by])
                    }
                }
            }
        }
    };
    var R, bd, at, bi, C, L, aq, aG, bb;
    this.initMap(true);
    var aO;
    VEvent.addDOMListener(window, "unload", function () {
        if (CountRequest > 0) {
            VUtil.jl1(j1l + "?NumRequest=" + CountRequest, "", "", "");
            CountRequest = 0
        }
    });
    aP.ondblclick = function (bw) {
        if (!this.ij.dblClickZoomable) {
            return false
        }
        if (VUtil.isVObject((browserName == "msie") ? event : bw)) {
            return false
        }
        if (this.ij.level == this.ij.maxLevel) {
            var bv = new VPoint(lI1 - this.ij.Left, lIl - this.ij.Top);
            var i = this.ij.fromContainerPixelToLatLng(bv);
            this.ij.panTo(i)
        } else {
            this.ij.prepareScrollZoom(1)
        }
        return false
    };
    aP.oncontextmenu = function (by) {
        if (!this.ij.dblClickZoomable) {
            return false
        }
        if (VUtil.isVObject((browserName == "msie") ? event : by)) {
            return false
        }
        var bv = new Date();
        var bw = parseInt(bv.getHours()) * 60 * 60 * 1000 + parseInt(bv.getMinutes()) * 60 * 1000 + parseInt(bv.getSeconds()) * 1000 + parseInt(bv.getMilliseconds());
        if (bw - this.prevRightClickTime < 230) {
            this.dblRC = "true";
            if (this.ij.level == this.ij.minLevel) {
                var bx = new VPoint(lI1 - this.ij.Left, lIl - this.ij.Top);
                var i = this.ij.fromContainerPixelToLatLng(bx);
                this.ij.panTo(i)
            } else {
                this.ij.prepareScrollZoom(-1)
            }
        }
        this.prevRightClickTime = bw;
        return false
    };
    if (browserName == "msie" || browserName == "opera" || browserName == "chrome" || browserName == "safari") {
        aP.onmousewheel = function () {
            this.ij.mouseWheelHandler(event, event.wheelDelta / 120)
        }
    } else {
        aP.addEventListener("DOMMouseScroll", function (i) {
            this.ij.mouseWheelHandler(i, -i.detail)
        }, false)
    }

    function ab(bv, bw) {
        this.updateCurrentXY(bv);
        if (lI1 == -1 && lIl == -1) {
            return
        }
        this.calculatePosition();
        if (this.jij == false) {
            return false
        }
        var i = (browserName != "msie") ? "HTML" : "BODY";
        aO = (browserName != "msie") ? bv.target : bv.srcElement;
        while (aO.tagName != i && (aO.className.indexOf != undefined && aO.className.indexOf("dragruler") == -1 && aO.className != "overviewmap" && aO.className != "dragoverview" && aO.className.indexOf("clickcontrol") == -1 && aO.className != "nomousemove")) {
            aO = (browserName != "msie") ? aO.parentNode : aO.parentElement;
            if (aO == null) {
                return false
            }
        }
        if (aO.tagName != i && aO.tagName != "svg" && aO.tagName != "path" && aO.tagName != "circle") {
            return
        }
        this.stopEvent(bv);
        if ((bw > 0 && this.level == this.maxLevel) || (bw < 0 && this.level == this.minLevel)) {
            return false
        }
        this.prepareScrollZoom(bw)
    }
    var ap = aP;
    VEvent.addDOMListener(document, "mousedown", function (bw) {
        var i = (browserName != "msie") ? "HTML" : "BODY";
        var bv = (browserName != "msie") ? bw.target : event.srcElement;
        if (bv.tagName == "INPUT" || bv.tagName == "TEXTAREA") {
            return
        }
        while (bv.tagName != i && bv.id != ap.id) {
            bv = (browserName != "msie") ? bv.parentNode : bv.parentElement;
            if (bv == null) {
                return
            }
        }
        if (bv.tagName == i) {
            ap.ij.bMapActive = false
        } else {
            if (bv.id == ap.id) {
                ap.ij.bMapActive = true
            }
        }
    });

    function j(bv) {
        this.updateCurrentXY(bv);
        if (vbdIsdragging == false) {
            vbdIsdragging = true
        } else {
            return
        }
        var i = (browserName != "msie") ? "HTML" : "BODY";
        aO = (browserName != "msie") ? bv.target : bv.srcElement;
        if (aO.tagName == "INPUT" || aO.tagName == "TEXTAREA") {
            vbdIsSelectInput = true;
            return
        } else {
            vbdIsSelectInput = false
        }
        while (aO.tagName != "svg" && aO.tagName != "path" && aO.tagName != "polyline" && aO.tagName != "oval" && aO.tagName != i && (aO.className.indexOf != undefined && aO.className.indexOf("dragruler") == -1 && aO.className != "overviewmap" && aO.className != "dragoverview" && aO.className != "dragmarker" && aO.className != "clickmarker" && aO.className != "dragtext" && aO.className.indexOf("clickcontrol") == -1 && aO.className != "dragmap" && aO.className != "dragmaparea" && aO.className != "dragCustomOverlay" && aO.className != "nomousemove")) {
            aO = (browserName != "msie") ? aO.parentNode : aO.parentElement
        }
        aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto";
        if (aO.className.indexOf != undefined && aO.className.indexOf("dragruler") != -1) {
            this.lllj = true;
            aP.tLeft = parseInt(aO.style.left);
            aP.tTop = parseInt(aO.style.top);
            if (this.zoomstartHandler != undefined) {
                this.zoomstartHandler()
            }
        } else {
            if (aO.className == "overviewmap") {
                this.ll1I = true;
                aP.tLeft = parseInt(document.getElementById(this.id + "_VOverviewBox").style.left);
                aP.tTop = parseInt(document.getElementById(this.id + "_VOverviewBox").style.top);
                aP.l1Ij = parseInt(document.getElementById(this.id + "_VCenterRect").style.left);
                aP.l1il = parseInt(document.getElementById(this.id + "_VCenterRect").style.top)
            } else {
                if (aO.className == "dragoverview") {
                    this.llIi = true;
                    aP.tLeft = parseInt(aO.style.left);
                    aP.tTop = parseInt(aO.style.top)
                } else {
                    if (aO.className == "dragmarker") {
                        if (aO.draggable == 1) {
                            if (aO.dragStartHandler != undefined) {
                                aO.dragStartHandler()
                            }
                            this.closeInfoWindow();
                            this.jjj = true;
                            aP.tLeft = parseInt(aO.style.left);
                            aP.tTop = parseInt(aO.style.top);
                            aO.deltaX = parseFloat(aO.style.left) - (lI1 - this.Left - parseInt(C.style.left));
                            aO.deltaY = parseFloat(aO.style.top) - (lIl - this.Top - parseInt(C.style.top))
                        }
                    } else {
                        if (aO.className == "dragtext") {
                            if (aO.draggable == 1) {
                                this.closeInfoWindow();
                                this.isTextDragging = true;
                                aP.tLeft = parseInt(aO.style.left);
                                aP.tTop = parseInt(aO.style.top)
                            }
                        } else {
                            if (aO.className.indexOf != undefined && aO.className.indexOf("clickcontrol") != -1) {
                                aP.onselectstart = null;
                                aP.isSelectWindowContent = true;
                                return
                            } else {
                                if (aO.className == "dragmap" || aO.className == "dragmaparea" || aO.className == "dragCustomOverlay" || aO.className == "clickmarker" || aO.tagName == "path" || aO.tagName == "svg" || aO.tagName == "polyline" || aO.tagName == "circle" || aO.tagName == "oval" || (aO.className == "" && browserName == "safari")) {
                                    if (this.ijj) {
                                        return false
                                    }
                                    this.ijj = true;
                                    aP.tLeft = parseInt(bd.style.left);
                                    aP.tTop = parseInt(bd.style.top);
                                    aP.sLeft = parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left);
                                    aP.sTop = parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top);
                                    aP.stLeft = parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left);
                                    aP.stTop = parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top);
                                    aP.l1Ij = parseInt(C.style.left);
                                    aP.l1il = parseInt(C.style.top);
                                    aP.aLeft = parseInt(at.style.left);
                                    aP.aTop = parseInt(at.style.top)
                                } else {
                                    if (aO.className == "nomousemove" || aO.tagName != i) {
                                        return
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        aP.tX = bv.clientX;
        aP.tY = bv.clientY;
        return false
    }

    function bp(bJ) {
        this.updateCurrentXY(bJ);
        if (vbdIsSelectInput) {
            return
        }
        if (this.ijj && this.draggable) {
            if (!aP.llIl) {
                if (document.getElementById("VIWC") != null) {
                    document.getElementById("VIWC").style.overflow = "hidden"
                }
                if (this.li1 != undefined) {
                    this.li1()
                }
                if (this.lli1 != undefined) {
                    if (!VUtil.isVObject(bJ)) {
                        this.lli1()
                    }
                }
                aP.llIl = true;
                aP.l1li = true
            }
            if (this.jji != undefined) {
                this.jji()
            }
            if (this.iII != undefined) {
                this.iII()
            }
            var bM = document.getElementById(aP.id + "_VOverviewRect");
            var by = bJ.clientX - aP.tX + aP.tLeft;
            var bF = bJ.clientY - aP.tY + aP.tTop;
            var bA = 0,
                bz = 0;
            if (this.boundsRestrict != null) {
                if (by < this.ptBdResSEV.x) {
                    bA = this.ptBdResSEV.x - by;
                    by = this.ptBdResSEV.x
                } else {
                    if (by > this.ptBdResNWV.x) {
                        bA = this.ptBdResNWV.x - by;
                        by = this.ptBdResNWV.x
                    }
                } if (bF < this.ptBdResSEV.y) {
                    bz = this.ptBdResSEV.y - bF;
                    bF = this.ptBdResSEV.y
                } else {
                    if (bF > this.ptBdResNWV.y) {
                        bz = this.ptBdResNWV.y - bF;
                        bF = this.ptBdResNWV.y
                    }
                }
            }
            by += "px";
            bF += "px";
            var bN = this.curMapType.getTileLayers();
            for (var bG = 0; bG < bN.length; bG++) {
                bN[bG].lj.style.left = by;
                bN[bG].lj.style.top = bF
            }
            if (bi != undefined) {
                bi.style.left = by;
                bi.style.top = bF
            }
            aP.sLeft = (bJ.clientX - aP.tX + aP.stLeft) + bA;
            aP.sTop = (bJ.clientY - aP.tY + aP.stTop) + bz;
            C.style.left = (bJ.clientX - aP.tX + aP.l1Ij + bA) + "px";
            C.style.top = (bJ.clientY - aP.tY + aP.l1il + bz) + "px";
            at.style.left = (bJ.clientX - aP.tX + aP.aLeft + bA) + "px";
            at.style.top = (bJ.clientY - aP.tY + aP.aTop + bz) + "px";
            if (bM != null) {
                var bK = parseInt(document.getElementById(aP.id + "_VCenterRect").style.left);
                var bv = parseInt(document.getElementById(aP.id + "_VCenterRect").style.top);
                bM.style.left = (bK + Math.round((aP.tX - bJ.clientX - bA) / this.nInstalment)) + "px";
                bM.style.top = (bv + Math.round((aP.tY - bJ.clientY - bz) / this.nInstalment)) + "px"
            }
            if (aP.sTop < -256) {
                this.insertBottom();
                aP.stTop += 256;
                aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto"
            }
            if (aP.sLeft > 0) {
                this.insertLeft();
                aP.stLeft -= 256;
                aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto"
            } else {
                if (aP.sLeft < -256) {
                    this.insertRight();
                    aP.stLeft += 256;
                    aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto"
                }
            } if (aP.sTop > 0) {
                this.insertTop();
                aP.stTop -= 256;
                aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto"
            }
            return false
        } else {
            if (this.ll1I == true) {
                var bC = aP.tLeft + bJ.clientX - aP.tX;
                var bB = aP.tTop + bJ.clientY - aP.tY;
                if (bC > 0) {
                    this.overViewInsertLeft();
                    aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto";
                    bC = bC - 256;
                    aP.tLeft -= 256
                }
                if (bC < -256) {
                    this.overViewInsertRight();
                    aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto";
                    bC = bC + 256;
                    aP.tLeft += 256
                }
                if (bB > 0) {
                    this.overViewInsertTop();
                    aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto";
                    bB = bB - 256;
                    aP.tTop -= 256
                }
                if (bB < -256) {
                    this.overViewInsertBottom();
                    aP.style.cursor = "url('" + this.options.draggingCursor + "'),auto";
                    bB = bB + 256;
                    aP.tTop += 256
                }
                aO.style.left = bC + "px";
                aO.style.top = bB + "px";
                return false
            } else {
                if (this.llIi == true) {
                    var bC = aP.tLeft + bJ.clientX - aP.tX;
                    var bB = aP.tTop + bJ.clientY - aP.tY;
                    aO.style.left = bC + "px";
                    aO.style.top = bB + "px"
                } else {
                    if (this.lllj == true) {
                        var bB = aP.tTop + bJ.clientY - aP.tY;
                        for (var bH = 0; bH < this.nControls; bH++) {
                            if (this.arrControls[bH].mouseMoveHandler != undefined) {
                                this.arrControls[bH].mouseMoveHandler(aO, bB)
                            }
                        }
                        return false
                    } else {
                        if (this.jjj == true) {
                            if (!this.isMarkerAutoPanning) {
                                var bx = bJ.clientX;
                                var bw = bJ.clientY;
                                aO.style.left = aP.tLeft - aP.tX + bx + "px";
                                aO.style.top = aP.tTop - aP.tY + bw + "px"
                            } else {
                                aO.style.left = lI1 - this.Left - parseInt(C.style.left) + aO.deltaX + "px";
                                aO.style.top = lIl - this.Top - parseInt(C.style.top) + aO.deltaY + "px"
                            }
                            var bL, bI;
                            bL = parseFloat(aO.style.left) + aO.II.Il.offset.width;
                            bI = parseFloat(aO.style.top) + aO.II.Il.offset.height;
                            var bE = this.fromDivPixelToLatLng(new VPoint(bL, bI));
                            aO.II.latlngs.latitude = bE.latitude;
                            aO.II.latlngs.longitude = bE.longitude;
                            if (aO.II.Il.autoPan) {
                                var bD = new VPoint(lI1 - this.Left, lIl - this.Top);
                                aO.dx = 0;
                                aO.dy = 0;
                                if (bD.x <= 5) {
                                    aO.dx = -10
                                }
                                if (bD.x >= aP.offsetWidth - 5) {
                                    aO.dx = 10
                                }
                                if (bD.y <= 5) {
                                    aO.dy = -10
                                }
                                if (bD.y >= aP.offsetHeight - 5) {
                                    aO.dy = 10
                                }
                                if (aO.dx != 0 || aO.dy != 0) {
                                    if (!this.isMarkerAutoPanning) {
                                        this.isMarkerAutoPanning = true;
                                        this.panByMarker(aO)
                                    }
                                } else {
                                    this.isMarkerAutoPanning = false;
                                    aO.style.left = lI1 - this.Left - parseInt(C.style.left) + aO.deltaX + "px";
                                    aO.style.top = lIl - this.Top - parseInt(C.style.top) + aO.deltaY + "px";
                                    var bL, bI;
                                    bL = parseFloat(aO.style.left) + aO.II.Il.offset.width;
                                    bI = parseFloat(aO.style.top) + aO.II.Il.offset.height;
                                    var bE = this.fromDivPixelToLatLng(new VPoint(bL, bI));
                                    aO.II.latlngs.latitude = bE.latitude;
                                    aO.II.latlngs.longitude = bE.longitude
                                }
                            }
                            if (bx - aP.tX != 0 || bw - aP.tY != 0) {
                                aP.Iij = true;
                                aO.Iij = true;
                                if (aO.iII != undefined) {
                                    aO.iII()
                                }
                            }
                            return false
                        } else {
                            if (this.isTextDragging == true) {
                                var bx = bJ.clientX;
                                var bw = bJ.clientY;
                                aO.style.left = aP.tLeft - aP.tX + bx + "px";
                                aO.style.top = aP.tTop - aP.tY + bw + "px";
                                var bL, bI;
                                bL = parseFloat(aO.style.left) + aO.II.textOptions.offset.width;
                                bI = parseFloat(aO.style.top) + aO.II.textOptions.offset.height;
                                var bE = this.fromDivPixelToLatLng(new VPoint(bL, bI));
                                aO.II.latlngs.latitude = bE.latitude;
                                aO.II.latlngs.longitude = bE.longitude;
                                if (bx - aP.tX != 0 || bw - aP.tY != 0) {
                                    aP.hasDraggedText = true;
                                    aO.hasDraggedText = true
                                }
                                return false
                            } else {
                                if (this.isSelectWindowContent == true) {
                                    aP.onselectstart = null
                                } else {
                                    vbdIsdragging = false;
                                    return
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    function D(bK) {
        vbdIsdragging = false;
        aP.style.cursor = "url('" + this.options.draggableCursor + "'),auto";
        if (this.ijj == true) {
            this.ijj = false;
            if (aP.llIl == true) {
                this.getRealCenter();
                var bN = document.getElementById(aP.id + "_VOverviewRect");
                if (bN != null) {
                    var bL = parseInt(document.getElementById(aP.id + "_VCenterRect").style.left);
                    var bv = parseInt(document.getElementById(aP.id + "_VCenterRect").style.top);
                    aP.tX = parseInt(bN.style.left + 0) - bL;
                    aP.tY = parseInt(bN.style.top + 0) - bv;
                    this.setOverViewCenter()
                }
                this.redrawScaleCtrl();
                this.updateMappingAreas();
                if (this.lij != undefined) {
                    for (var bB = 0; bB < this.lij.length; bB++) {
                        this.lij[bB]()
                    }
                }
                if (!VUtil.isVObject(bK)) {
                    if (!VUtil.isVBDWebsite()) {
                        initializeTimer()
                    }
                    if (this.jjI != undefined) {
                        this.jjI()
                    }
                }
            }
            if (document.getElementById("VIWC") != null) {
                document.getElementById("VIWC").style.overflow = "auto"
            }
            aP.llIl = false;
            var by = this.arrMaxRowCol[this.level];
            if (this.curMapType.opts.degree360 && ((((this.llI % by) + by) % by) + this.iI >= by || (((this.prevStartX % by) + by) % by) + this.iI >= by)) {
                var bI;
                for (bI = 0; bI < this.Ill; bI++) {
                    this.lli[bI].redraw(false)
                }
            }
            this.redrawLines();
            this.prevStartX = this.llI
        } else {
            if (this.ll1I == true) {
                var bP = bK.clientX - aP.tX;
                var bO = bK.clientY - aP.tY;
                this.panBy(new VSize(-bP * this.nInstalment, -bO * this.nInstalment));
                this.ll1I = false
            } else {
                if (this.llIi == true) {
                    var bP = bK.clientX - aP.tX;
                    var bO = bK.clientY - aP.tY;
                    this.panBy(new VSize(bP * this.nInstalment, bO * this.nInstalment));
                    this.llIi = false
                } else {
                    if (this.lllj == true) {
                        for (var bI = 0; bI < aP.l1l; bI++) {
                            for (var bH = 0; bH < aP.iI; bH++) {
                                var bD = bd.childNodes[0].childNodes[bI * aP.iI + bH];
                                bD.src = "";
                                bD.src = strResURL + "images/whiteimage.png"
                            }
                        }
                        var bE = parseInt(aO.style.top);
                        var bG = (this.maxLevel - this.minLevel) * 8 + 22;
                        var bx = Math.round((bG - bE) / 8) + this.minLevel;
                        var bw = this.getCenter();
                        var bC = this.getInfoWindow();
                        if (bC.isOpened()) {
                            bw = this.getCenterForPreserveInfoWindow(this.level, bx)
                        }
                        this.setCenter(bw, bx);
                        this.lllj = false;
                        this.llij = true
                    } else {
                        if (this.jjj == true) {
                            this.jjj = false;
                            var bM, bJ;
                            bM = parseFloat(aO.style.left) + aO.II.Il.offset.width;
                            bJ = parseFloat(aO.style.top) + aO.II.Il.offset.height;
                            var bF = this.fromDivPixelToLatLng(new VPoint(bM, bJ));
                            aO.II.latlngs.latitude = bF.latitude;
                            aO.II.latlngs.longitude = bF.longitude;
                            this.isMarkerAutoPanning = false;
                            if (aO.dragEndHandler != undefined) {
                                var bA = bK.clientX;
                                var bz = bK.clientY;
                                if (bA - aP.tX != 0 || bz - aP.tY != 0) {
                                    aO.dragEndHandler()
                                }
                            }
                        } else {
                            if (this.isTextDragging == true) {
                                var bM, bJ;
                                bM = parseFloat(aO.style.left) + aO.II.textOptions.offset.width;
                                bJ = parseFloat(aO.style.top) + aO.II.textOptions.offset.height;
                                var bF = this.fromDivPixelToLatLng(new VPoint(bM, bJ));
                                aO.II.latlngs.latitude = bF.latitude;
                                aO.II.latlngs.longitude = bF.longitude;
                                this.isTextDragging = false;
                                if (aO.dragEndHandler != undefined) {
                                    aO.dragEndHandler()
                                }
                            } else {
                                if (this.isSelectWindowContent == true) {
                                    aP.isSelectWindowContent = false
                                } else {
                                    return false
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    function A(bA) {
        this.ijj = false;
        this.getRealCenter();
        if (this.isZooming) {
            return false
        }
        this.calculatePosition();
        if (this.curGestureX == null || this.curGestureY == null) {
            return false
        }
        if (this.jij == false) {
            return false
        }
        this.isZooming = true;
        bd.style.left = parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left) + "px";
        bd.style.top = parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top) + "px";
        at.style.left = bd.style.left;
        at.style.top = bd.style.top;
        var bx, bv, bz;
        for (bx = 0; bx < this.l1l; bx++) {
            for (bv = 0; bv < this.iI; bv++) {
                bz = at.childNodes[0].childNodes[bx * this.iI + bv];
                bz.style.width = "256px";
                bz.style.height = "256px";
                bz.src = strResURL + "images/transparent.png"
            }
        }
        var bw = at;
        at = bd;
        bd = bw;
        this.curMapType.getTileLayers()[0].lj = bd;
        var bB = this.curGestureX - parseInt(at.style.left) - this.Left;
        var by = this.curGestureY - parseInt(at.style.top) - this.Top;
        this.divisorX = bB / 256;
        this.divisorY = by / 256;
        this.aniRootX = parseInt(at.style.left);
        this.aniRootY = parseInt(at.style.top);
        this.hideOverlays();
        if (this.zoomstartHandler != undefined) {
            this.zoomstartHandler()
        }
    }

    function ac(bA) {
        if (!this.isZooming) {
            return false
        }
        VUtil.createZoomAnimation((bA.scale >= 1) ? 1 : -1, this.curGestureX, this.curGestureY);
        var bx, bw, bz, bv, by;
        bv = 256 * bA.scale;
        by = 256 * (bA.scale - 1);
        for (bx = 0; bx < this.l1l; bx++) {
            for (bw = 0; bw < this.iI; bw++) {
                bz = at.childNodes[0].childNodes[bx * this.iI + bw];
                bz.style.width = bv + "px";
                bz.style.height = bv + "px";
                bz.style.left = (bw * bv) + "px";
                bz.style.top = (bx * bv) + "px"
            }
        }
        at.style.left = this.aniRootX - Math.round(this.divisorX * by) + (this.curGestureX - this.rootGestureX) + "px";
        at.style.top = this.aniRootY - Math.round(this.divisorY * by) + (this.curGestureY - this.rootGestureY) + "px"
    }

    function E(bz) {
        if (!this.isZooming) {
            return false
        }
        this.isZooming = false;
        aP.hasGestured = true;
        VUtil.removeZoomAnimation();
        var bx;
        if (bz.scale == 1) {
            bx = this.level
        } else {
            if (bz.scale > 1) {
                bx = this.level + Math.round(bz.scale / 2)
            } else {
                bx = this.level - Math.round((1 / bz.scale) / 2)
            }
        } if (bx < this.minLevel) {
            bx = this.minLevel
        }
        if (bx > this.maxLevel) {
            bx = this.maxLevel
        }
        var bB = this.divisorX * 256;
        var by = this.divisorY * 256;
        var bA = VProjection.fromPixelToLatLng(new VPoint(bB + 256 * this.llI, by + 256 * this.jl), this.level);
        var bw = VProjection.fromLatLngToPixel(bA, bx);
        var bv = bw.x - this.curGestureX + this.Left + parseInt(this.lI.offsetWidth) / 2;
        var i = bw.y - this.curGestureY + this.Top + parseInt(this.lI.offsetHeight) / 2;
        bA = VProjection.fromPixelToLatLng(new VPoint(bv, i), bx);
        at.style.zIndex = MapAnimatedZIndex;
        bd.style.zIndex = MapInnerZIndex;
        this.setCenter(bA, bx, this.curMapType, false)
    }
    if (browserName == "iPhone" || browserName == "iPod" || browserName == "iPad") {
        aP.ontouchstart = function (bv) {
            try {
                if (bv.touches.length == 1) {
                    return aP.ij.mouseDownHandler(bv.touches[0])
                } else {
                    if (bv.touches.length == 2) {
                        aP.ij.getGestureCenter(bv.touches[0], bv.touches[1]);
                        return false
                    }
                }
            } catch (i) {
                console.log("touchstart - " + i)
            }
        };
        VEvent.addDOMListener(document, "touchmove", function (bv) {
            try {
                if (bv.touches.length == 1) {
                    return aP.ij.mouseMoveHandler(bv.touches[0])
                } else {
                    if (bv.touches.length == 2) {
                        aP.ij.getGestureCenter(bv.touches[0], bv.touches[1]);
                        return false
                    }
                }
            } catch (i) {
                console.log("touchmove - " + i)
            }
        });
        VEvent.addDOMListener(document, "touchend", function (bv) {
            try {
                if (bv.touches.length == 0) {
                    return aP.ij.mouseUpHandler(bv.changedTouches[0])
                } else {
                    if (bv.touches.length == 1) {
                        aP.ij.rootGestureX = null;
                        aP.ij.rootGestureY = null;
                        aP.ij.curGestureX = null;
                        aP.ij.curGestureY = null
                    }
                }
            } catch (i) {
                console.log("touchend - " + i)
            }
        });
        VEvent.addDOMListener(aP, "gesturestart", function (bv) {
            try {
                return aP.ij.gestureStartHandler(bv)
            } catch (i) {
                console.log("gesturestart - " + i)
            }
        });
        VEvent.addDOMListener(document, "gesturechange", function (bv) {
            try {
                return aP.ij.gestureChangeHandler(bv)
            } catch (i) {
                console.log("gesturechange - " + i)
            }
        });
        VEvent.addDOMListener(document, "gestureend", function (bv) {
            try {
                return aP.ij.gestureEndHandler(bv)
            } catch (i) {
                console.log("gestureend - " + i)
            }
        })
    } else {
        aP.onmousedown = function (i) {
            if ((browserName == "msie" && event.button != 1) || (browserName != "msie" && i.which != 1)) {
                return
            }
            return this.ij.mouseDownHandler(browserName == "msie" ? event : i)
        };
        VEvent.addDOMListener(document, "mousemove", function (i) {
            return aP.ij.mouseMoveHandler(browserName == "msie" ? event : i)
        });
        VEvent.addDOMListener(document, "mouseup", function (i) {
            return aP.ij.mouseUpHandler(browserName == "msie" ? event : i)
        })
    }

    function y(i) {
        if (browserName == "firefox") {
            if (i.stopPropagation) {
                i.stopPropagation();
                i.preventDefault()
            }
        } else {
            event.cancelBubble = true;
            event.returnValue = false
        }
        return false
    }

    function Z() {
        return "VMap"
    }

    function ba() {
        this.setCenter(this.getCenter(), this.getLevel());
        this.disableDragging();
        this.disableInfoWindow();
        this.disableDoubleClickZoom();
        this.disableContinuousZoom();
        this.disableScrollWheelZoom();
        this.clearOverlays();
        this.clearControls()
    }

    function m() {
        var bx = aP;
        var bC = new Object();
        bC.x = 0;
        bC.y = 0;
        if (bx !== null) {
            if (bx.getBoundingClientRect) {
                var bA = document.documentElement;
                var by = bx.getBoundingClientRect();
                var bw = bA.scrollLeft;
                var bv = bA.scrollTop;
                bC.x = by.left + bw;
                bC.y = by.top + bv
            } else {
                bC.x = bx.offsetLeft;
                bC.y = bx.offsetTop;
                var bz = bx.parentNode;
                var i = null;
                while (offsetParent != null) {
                    bC.x += offsetParent.offsetLeft;
                    bC.y += offsetParent.offsetTop;
                    var bB = offsetParent.tagName.toLowerCase();
                    if ((__isIEOld && bB != "table") || ((__isFireFoxNew || __isChrome) && bB == "td")) {
                        i = VUtil.getBorderWidth(offsetParent);
                        bC.x += i.left;
                        bC.y += i.top
                    }
                    if (offsetParent != document.body && offsetParent != document.documentElement) {
                        bC.x -= offsetParent.scrollLeft;
                        bC.y -= offsetParent.scrollTop
                    }
                    if (!__isIE && !__isOperaOld || __isIENew) {
                        while (offsetParent != bz && bz !== null) {
                            bC.x -= bz.scrollLeft;
                            bC.y -= bz.scrollTop;
                            if (__isFireFoxOld || __isWebKit) {
                                i = VUtil.getBorderWidth(bz);
                                bC.x += i.left;
                                bC.y += i.top
                            }
                            bz = bz.parentNode
                        }
                    }
                    bz = offsetParent.parentNode;
                    offsetParent = offsetParent.offsetParent
                }
            }
        }
        this.Left = bC.x;
        this.Top = bC.y
    }

    function bq(bI) {
        if (!bI && !VUtil.isVBDWebsite()) {
            aP.removeChild(aP.childNodes[0])
        }
        this.l1l = parseInt(this.options.size.height / 256) + 2;
        this.iI = parseInt(this.options.size.width / 256) + 2;
        var by = VProjection.fromLatLngToPixel(this.getCenter(), this.getLevel());
        var bE = this.options.size.width / 2;
        var bD = this.options.size.height / 2;
        var bK = by.x - bE;
        var bJ = by.y - bD;
        this.llI = parseInt(bK / 256);
        this.jl = parseInt(bJ / 256);
        var bv, bx;
        bv = -parseInt(bK % 256);
        bx = -parseInt(bJ % 256);
        if (by.x < bE) {
            this.llI--;
            bv -= 256
        }
        if (by.y < bD) {
            this.jl--;
            bx -= 256
        }
        var bB, bA, bz;
        if (!bI) {
            for (bB = 1; bB < 5; bB++) {
                aP.removeChild(document.getElementById(aP.id + "l" + bB));
                aP.removeChild(document.getElementById(aP.id + "t" + bB))
            }
        }
        var bF;
        for (bB = 1; bB < 5; bB++) {
            bF = VUtil.createDivElement(aP.id + "l" + bB, 0, 0, bB, aP.offsetHeight, "absolute", "hidden", "");
            bF.style.background = "#000000 none repeat scroll 0 0";
            bF.style.filter = "alpha(opacity = " + (10 - 2 * bB) + ")";
            bF.style.opacity = 0.1 - 0.02 * bB;
            bF.style.display = "block";
            bF.style.zIndex = 100;
            aP.appendChild(bF);
            bF = VUtil.createDivElement(aP.id + "t" + bB, 0, 0, aP.offsetWidth, bB, "absolute", "hidden", "");
            bF.style.background = "#000000 none repeat scroll 0 0";
            bF.style.filter = "alpha(opacity = " + (10 - 2 * bB) + ")";
            bF.style.opacity = 0.1 - 0.02 * bB;
            bF.style.display = "block";
            bF.style.zIndex = 100;
            aP.appendChild(bF)
        }
        if (bI) {
            R = VUtil.createDivElement(this.lI.id + "_map1", 0, 0, "100%", "100%", "absolute", "hidden", "")
        } else {
            R = document.getElementById(this.lI.id + "_map1");
            while (R.childNodes.length > 0) {
                R.removeChild(R.childNodes[0])
            }
        }
        var bH = new Array();
        if (this.curMappingLayer != null) {
            bH[bH.length] = "<div style='position:absolute; left:0px; top:0px;'>";
            for (bB = 0; bB < this.l1l; bB++) {
                for (bA = 0; bA < this.iI; bA++) {
                    bH[bH.length] = "<map id='usemapindex" + bB + "_" + bA + "' name='usemapindex" + (bB + "_" + bA) + "'></map>"
                }
            }
            bH[bH.length] = "</div>";
            bi = VUtil.createDivElement(this.lI.id + "_mapping", bv, bx, this.iI * 256, this.l1l * 256, "absolute", "", bH.join(""));
            R.appendChild(bi);
            bi.className = "dragmaparea";
            bi.style.zIndex = MapInnerZIndex
        }
        var bC = (this.curMappingLayer != null) ? this.curMappingLayer.mappedTileLayer : -1;
        for (bz = 0; bz < this.curMapType.getTileLayers().length; bz++) {
            bH = new Array();
            bH[bH.length] = "<div style='position:absolute; left:0px; top:0px;'>";
            for (bB = 0; bB < this.l1l; bB++) {
                for (bA = 0; bA < this.iI; bA++) {
                    if (bz != bC) {
                        bH[bH.length] = "<img style='position:absolute; -moz-user-select:none; width:256px; height:256px; left:" + (bA * 256) + "px; top:" + (bB * 256) + "px;' src='" + strResURL + "images/whiteimage.png' isMap />"
                    } else {
                        bH[bH.length] = "<img id='" + (this.llI + bB + "_" + this.jl + bA) + "' style='border:0px none; margin:0px; padding:0px; position:absolute; -moz-user-select:none; width:256px; height:256px; left:" + (bA * 256) + "px; top:" + (bB * 256) + "px;' src='" + strResURL + "images/whiteimage.png' isMap usemap='#usemapindex" + bB + "_" + bA + "' />"
                    }
                }
            }
            bH[bH.length] = "</div>";
            bd = VUtil.createDivElement((bz == 0) ? "" : this.lI.id + "_inner" + bz, bv, bx, this.iI * 256, this.l1l * 256, "absolute", "", bH.join(""));
            this.curMapType.getTileLayers()[bz].lj = bd;
            R.appendChild(bd);
            bd.className = "dragmap";
            bd.style.zIndex = (bz == 0) ? MapInnerZIndex : MapLayerZIndex
        }
        bd = this.curMapType.getTileLayers()[0].lj;
        bH = new Array();
        bH[bH.length] = "<div style='position:absolute; left:0px; top:0px;'>";
        for (bB = 0; bB < this.l1l; bB++) {
            for (bA = 0; bA < this.iI; bA++) {
                bH[bH.length] = "<img style='position:absolute; -moz-user-select:none; width:256px; height:256px; left:" + (bA * 256) + "px; top:" + (bB * 256) + "px;' src='" + strResURL + "images/transparent.png' isMap />"
            }
        }
        bH[bH.length] = "</div>";
        at = VUtil.createDivElement("", bv, bx, this.iI * 256, this.l1l * 256, "absolute", "", bH.join(""));
        R.appendChild(at);
        at.className = "dragmap";
        at.style.zIndex = MapAnimatedZIndex;
        if (bI) {
            aP.appendChild(R);
            C = VUtil.createDivElement(this.lI.id + "_path", 0, 0, "", "", "absolute", "", "");
            L = VUtil.createDivElement("", 0, 0, "", "", "absolute", "", "");
            aq = VUtil.createDivElement("", 0, 0, "", "", "absolute", "", "");
            aG = VUtil.createDivElement("", 0, 0, "", "", "absolute", "", "");
            bb = VUtil.createDivElement("", 0, 0, "", "", "absolute", "", "");
            C.appendChild(L);
            C.appendChild(aq);
            C.appendChild(aG);
            C.appendChild(bb);
            aP.appendChild(C);
            aG.className = "dragCustomOverlay";
            if (browserName != "msie") {
                var bG = document.createElementNS("http://www.w3.org/2000/svg", "svg");
                bG.setAttribute("overflow", "visible");
                bG.style.position = "absolute";
                aq.appendChild(bG)
            }
        }
        L.style.zIndex = MarkerZIndex;
        aq.style.zIndex = LineZIndex;
        C.style.zIndex = MapPathZIndex;
        bb.style.zIndex = ObjBelowShadeZIndex;
        bH = new Array();
        bH[bH.length] = "<img style='cursor:pointer;' onclick={window.open('http://www.vietbando.com/');} src='" + strResURL + "images/poweredby.png' />";
        if (!VUtil.isVBDWebsite()) {
            var bw = VUtil.createDivElement("", 5, this.options.size.height - 40, 40, 30, "absolute", "", bH.join(""));
            bw.style.zIndex = VBDLogoZIndex;
            bw.style.MozUserSelect = "none";
            aP.insertBefore(bw, aP.childNodes[0])
        }
    }

    function a0(bv) {
        this.strLanguage = bv;
        var bw;
        for (bw = 0; bw < this.nControls; bw++) {
            if (this.arrControls[bw].updateLanguage != undefined) {
                this.arrControls[bw].updateLanguage()
            }
        }
    }

    function be(bB) {
        var bv = this.curMapType.getTileLayers();
        var bD = bv[bB].lj;
        var bE = new Array();
        var bw = new Array();
        var bC = this.arrMaxRowCol[this.level];
        var bA = null;
        for (var bz = 0; bz < this.l1l; bz++) {
            for (var by = 0; by < this.iI; by++) {
                bA = bD.childNodes[0].childNodes[bz * this.iI + by];
                var bF = this.jl + bz;
                var bx = this.llI + by;
                if (this.curMapType.opts.degree360) {
                    bx = ((bx % bC) + bC) % bC
                }
                bE[bE.length] = bA.id;
                bw[bw.length] = new VPoint(bx, bF)
            }
        }
        return bw
    }

    function ax() {
        if (this.curMappingLayer == null) {
            return
        }
        var bv = this.curMapType.getTileLayers();
        var bB = (this.curMappingLayer != null) ? this.curMappingLayer.mappedTileLayer : -1;
        var bD = bv[bB].lj;
        var bE = new Array();
        var bw = new Array();
        var bC = this.arrMaxRowCol[this.level];
        var bA = null;
        for (var bz = 0; bz < this.l1l; bz++) {
            for (var by = 0; by < this.iI; by++) {
                bA = bD.childNodes[0].childNodes[bz * this.iI + by];
                var bF = this.jl + bz;
                var bx = this.llI + by;
                if (this.curMapType.opts.degree360) {
                    bx = ((bx % bC) + bC) % bC
                }
                bE[bE.length] = bA.id;
                bw[bw.length] = new VPoint(bx, bF)
            }
        }
        this.curMappingLayer.getMappingAreas(bE, bw, this.level)
    }

    function X() {
        var bB = (this.curMappingLayer != null) ? this.curMappingLayer.mappedTileLayer : -1;
        var bv = this.curMapType.getTileLayers();
        var bA = null;
        for (var bx = 0; bx < bv.length; bx++) {
            if (bx == bB) {
                var bC = bv[bx].lj;
                for (var bz = 0; bz < this.l1l; bz++) {
                    for (var by = 0; by < this.iI; by++) {
                        var bD = this.jl + bz;
                        var bw = this.llI + by;
                        bA = bC.childNodes[0].childNodes[bz * this.iI + by];
                        if (bA.id != null && bA.id != "") {
                            bA.id = bw + "_" + bD
                        }
                        bA.src = bv[bx].getTileUrl(new VPoint(bw, bD), this.level)
                    }
                }
            }
        }
    }

    function T(i, bw, by) {
        var bv = "";
        if (this.ImagesURL.indexOf("ashx") != -1) {
            bv = this.ImagesURL + "?Action=GetTiles&Level=" + i + "&X=" + bw + "&Y=" + by
        } else {
            var bx = parseInt((by * Math.pow(2, i) + bw) / LimitFolder);
            bv = this.ImagesURL + "mapimages/" + i + "/" + bx + "/" + by + "_" + bw + ".png"
        }
        return bv
    }

    function H() {
        this.llI--;
        var by = parseInt(bd.childNodes[0].childNodes[0].style.left) - 256;
        var bv = this.curMapType.getTileLayers();
        var bw = "";
        var bC = this.arrMaxRowCol[this.level];
        var bz, bA;
        for (bz = 0; bz < bv.length; bz++) {
            var bD = bv[bz].lj;
            for (bA = 0; bA < this.l1l; bA++) {
                var bB = bD.childNodes[0].childNodes[bA * this.iI + this.iI - 1];
                bD.childNodes[0].removeChild(bB);
                bB.src = "";
                bB.src = strResURL + "images/transparent.png";
                bB.style.left = by + "px";
                bB.style.width = "256px";
                bB.style.height = "256px";
                var bE = this.jl + bA;
                var bx = this.llI;
                if (this.curMapType.opts.degree360) {
                    bx = ((bx % bC) + bC) % bC
                }
                if (this.jl + bA >= 0 && this.jl + bA < bC) {
                    if (bB.id != null && bB.id != "") {
                        bB.id = bx + "_" + bE
                    }
                    bw = bv[bz].getTileUrl(new VPoint(bx, bE), this.level);
                    if (bw != null && bw != "") {
                        bB.src = bw
                    }
                }
                bD.childNodes[0].insertBefore(bB, bD.childNodes[0].childNodes[bA * this.iI])
            }
        }
    }

    function a() {
        this.llI++;
        var by = parseInt(bd.childNodes[0].childNodes[this.iI - 1].style.left) + 256;
        var bv = this.curMapType.getTileLayers();
        var bw = "";
        var bC = this.arrMaxRowCol[this.level];
        var bz, bA;
        for (bz = 0; bz < bv.length; bz++) {
            var bD = bv[bz].lj;
            for (bA = 0; bA < this.l1l; bA++) {
                var bB = bD.childNodes[0].childNodes[bA * this.iI];
                bD.childNodes[0].removeChild(bB);
                bB.src = "";
                bB.src = strResURL + "images/transparent.png";
                bB.style.left = by + "px";
                bB.style.width = "256px";
                bB.style.height = "256px";
                var bE = this.jl + bA;
                var bx = this.llI + this.iI - 1;
                if (this.curMapType.opts.degree360) {
                    bx = ((bx % bC) + bC) % bC
                }
                if (this.jl + bA >= 0 && this.jl + bA < bC) {
                    if (bB.id != null && bB.id != "") {
                        bB.id = bx + "_" + bE
                    }
                    bw = bv[bz].getTileUrl(new VPoint(bx, bE), this.level);
                    if (bw != null && bw != "") {
                        bB.src = bw
                    }
                }
                if (bA == this.l1l - 1) {
                    bD.childNodes[0].appendChild(bB)
                } else {
                    bD.childNodes[0].insertBefore(bB, bD.childNodes[0].childNodes[bA * this.iI + this.iI - 1])
                }
            }
        }
    }

    function s() {
        this.jl--;
        var bD = parseInt(bd.childNodes[0].childNodes[0].style.top) - 256;
        var bv = this.curMapType.getTileLayers();
        var bw = "";
        var bB = this.arrMaxRowCol[this.level];
        for (var by = 0; by < bv.length; by++) {
            var bC = bv[by].lj;
            for (var bz = 0; bz < this.iI; bz++) {
                var bA = bC.childNodes[0].childNodes[this.iI * (this.l1l - 1) + bz];
                bC.childNodes[0].removeChild(bA);
                bA.src = "";
                bA.src = strResURL + "images/transparent.png";
                bA.style.top = bD + "px";
                bA.style.width = "256px";
                bA.style.height = "256px";
                var bE = this.jl;
                var bx = this.llI + bz;
                if (this.curMapType.opts.degree360) {
                    bx = ((bx % bB) + bB) % bB
                }
                if (this.jl >= 0 && this.jl < bB) {
                    if (bA.id != null && bA.id != "") {
                        bA.id = bx + "_" + bE
                    }
                    bw = bv[by].getTileUrl(new VPoint(bx, bE), this.level);
                    if (bw != null && bw != "") {
                        bA.src = bw
                    }
                }
                bC.childNodes[0].insertBefore(bA, bC.childNodes[0].childNodes[bz])
            }
        }
    }

    function Q() {
        this.jl++;
        var bD = parseInt(bd.childNodes[0].childNodes[(this.l1l - 1) * this.iI].style.top) + 256;
        var bv = this.curMapType.getTileLayers();
        var bw = "";
        var bB = this.arrMaxRowCol[this.level];
        for (var by = 0; by < bv.length; by++) {
            var bC = bv[by].lj;
            for (var bz = 0; bz < this.iI; bz++) {
                var bA = bC.childNodes[0].childNodes[0];
                bC.childNodes[0].removeChild(bA);
                bA.src = "";
                bA.src = strResURL + "images/transparent.png";
                bA.style.top = bD + "px";
                bA.style.width = "256px";
                bA.style.height = "256px";
                var bE = this.jl + this.l1l - 1;
                var bx = this.llI + bz;
                if (this.curMapType.opts.degree360) {
                    bx = ((bx % bB) + bB) % bB
                }
                if (this.jl + this.l1l - 1 >= 0 && this.jl + this.l1l - 1 < bB) {
                    if (bA.id != null && bA.id != "") {
                        bA.id = bx + "_" + bE
                    }
                    bw = bv[by].getTileUrl(new VPoint(bx, bE), this.level);
                    if (bw != null && bw != "") {
                        bA.src = bw
                    }
                }
                bC.childNodes[0].appendChild(bA)
            }
        }
    }

    function ay() {
        var bE = document.getElementById(this.id + "_VOverviewTable").tHead;
        for (var bB = 0; bB < bE.rows.length; bB++) {
            var bD = document.createElement("th");
            bE.rows[bB].appendChild(bD)
        }
        VUtil.removeLeftColumn(this.id + "_VOverviewTable");
        for (var bA = 0; bA < 2; bA++) {
            for (var bz = 0; bz >= 0; bz--) {
                if (document.getElementById(this.id + "s_" + bA + "_" + bz) != null) {
                    document.getElementById(this.id + "s_" + bA + "_" + bz).id = this.id + "s_" + bA + "_" + (bz + 1)
                }
            }
        }
        var by = document.getElementById(this.id + "_VOverviewTable").tBodies[0];
        var bC = Math.pow(2, this.lIj);
        for (var bA = 0; bA < by.rows.length; bA++) {
            var bx = by.rows[bA].insertCell(0);
            bx.innerHTML = "<img id=" + this.id + "s_" + bA + "_0 src=" + strResURL + "images/transparent.png width=256px height=256px>";
            if ((this.l1j + bA) >= 0 && (this.l1j + bA) < bC) {
                var bw = parseInt(((this.l1j + bA) * bC + this.liI - 1) / LimitFolder);
                var bC = bC;
                var bF = this.l1j + bA;
                var bv = this.liI - 1;
                bv = ((bv % bC) + bC) % bC;
                document.getElementById(this.id + "s_" + bA + "_0").src = this.getVbdMapsTileUrl(this.lIj, bv, bF)
            }
        }
        this.liI--
    }

    function am() {
        var bE = document.getElementById(this.id + "_VOverviewTable").tHead;
        for (var bB = 0; bB < bE.rows.length; bB++) {
            var bD = document.createElement("th");
            bE.rows[bB].appendChild(bD)
        }
        VUtil.removeRightColumn(this.id + "_VOverviewTable");
        for (var bA = 0; bA < 2; bA++) {
            for (var bz = 1; bz <= 2 - 1; bz++) {
                if (document.getElementById(this.id + "s_" + bA + "_" + bz) != null) {
                    document.getElementById(this.id + "s_" + bA + "_" + bz).id = this.id + "s_" + bA + "_" + (bz - 1)
                }
            }
        }
        var by = document.getElementById(this.id + "_VOverviewTable").tBodies[0];
        var bC = Math.pow(2, this.lIj);
        for (var bA = 0; bA < by.rows.length; bA++) {
            var bx = by.rows[bA].insertCell(-1);
            bx.innerHTML = "<img id=" + this.id + "s_" + bA + "_" + (2 - 1) + " src=" + strResURL + "images/transparent.png width=256px height=256px>";
            if ((this.l1j + bA) >= 0 && (this.l1j + bA) < bC) {
                var bw = parseInt(((this.l1j + bA) * bC + this.liI + 2) / LimitFolder);
                var bC = bC;
                var bF = this.l1j + bA;
                var bv = this.liI + 2;
                bv = ((bv % bC) + bC) % bC;
                document.getElementById(this.id + "s_" + bA + "_" + (2 - 1)).src = this.getVbdMapsTileUrl(this.lIj, bv, bF)
            }
        }
        this.liI++
    }

    function e() {
        var bz = document.getElementById(this.id + "_VOverviewTable");
        bz.deleteRow(-1);
        for (var bA = 2 - 2; bA >= 0; bA--) {
            for (var by = 0; by < 2; by++) {
                if (document.getElementById(this.id + "s_" + bA + "_" + by) != null) {
                    document.getElementById(this.id + "s_" + bA + "_" + by).id = this.id + "s_" + (bA + 1) + "_" + by
                }
            }
        }
        var bC = bz.insertRow(1);
        var bB = Math.pow(2, this.lIj);
        for (var bA = 0; bA < 2; bA++) {
            var bv = bC.insertCell(bA);
            bv.innerHTML = "<img id=" + this.id + "s_0_" + bA + " src=" + strResURL + "images/transparent.png width=256px height=256px>";
            if ((this.l1j - 1) >= 0 && (this.l1j - 1) < bB) {
                var bx = parseInt(((this.l1j - 1) * bB + this.liI + bA) / LimitFolder);
                var bB = bB;
                var bD = this.l1j - 1;
                var bw = this.liI + bA;
                bw = ((bw % bB) + bB) % bB;
                document.getElementById(this.id + "s_0_" + bA).src = this.getVbdMapsTileUrl(this.lIj, bw, bD)
            }
        }
        this.l1j--
    }

    function ah() {
        var bz = document.getElementById(this.id + "_VOverviewTable");
        bz.deleteRow(1);
        for (var bA = 1; bA <= 2 - 1; bA++) {
            for (var by = 0; by < 2; by++) {
                if (document.getElementById(this.id + "s_" + bA + "_" + by) != null) {
                    document.getElementById(this.id + "s_" + bA + "_" + by).id = this.id + "s_" + (bA - 1) + "_" + by
                }
            }
        }
        var bC = bz.insertRow(-1);
        var bB = Math.pow(2, this.lIj);
        for (var bA = 0; bA < 2; bA++) {
            var bv = bC.insertCell(bA);
            bv.innerHTML = "<img id=" + this.id + "s_" + (2 - 1) + "_" + bA + " src=" + strResURL + "images/transparent.png width=256px height=256px>";
            if ((this.l1j + 2) >= 0 && (this.l1j + 2) < bB) {
                var bx = parseInt(((this.l1j + 2) * bB + this.liI + bA) / LimitFolder);
                var bB = bB;
                var bD = this.l1j + 2;
                var bw = this.liI + bA;
                bw = ((bw % bB) + bB) % bB;
                document.getElementById(this.id + "s_" + (2 - 1) + "_" + bA).src = this.getVbdMapsTileUrl(this.lIj, bw, bD)
            }
        }
        this.l1j++
    }

    function aS() {
        if (this.ij1 == 0) {
            this.ij1 = 8;
            document.getElementById(this.id + "_VOverviewRect").style.left = parseInt(document.getElementById(this.id + "_VOverviewBox").style.left) + "px";
            document.getElementById(this.id + "_VOverviewRect").style.top = parseInt(document.getElementById(this.id + "_VOverviewBox").style.top) + "px";
            this.fixOverView();
            return
        }
        this.ij1 -= 2;
        var bx = parseInt(document.getElementById(this.id + "_VOverviewBox").style.left);
        var bw = parseInt(document.getElementById(this.id + "_VOverviewBox").style.top);
        var bv = parseInt(bx) - Math.round(this.lI.tX / 4);
        var i = parseInt(bw) - Math.round(this.lI.tY / 4);
        if (bv >= 0) {
            this.overViewInsertLeft();
            bv -= 256
        }
        if (bv <= -256) {
            this.overViewInsertRight();
            bv += 256
        }
        if (i >= 0) {
            this.overViewInsertTop();
            i -= 256
        }
        if (i <= -256) {
            this.overViewInsertBottom();
            i += 256
        }
        document.getElementById(this.id + "_VOverviewBox").style.left = bv + "px";
        document.getElementById(this.id + "_VOverviewBox").style.top = i + "px";
        setTimeout(function () {
            aP.ij.setOverViewCenter()
        }, 10)
    }

    function b() {
        if (document.getElementById(this.id + "_VOverviewMapControl") != null) {
            this.lIj = this.level - 4;
            var bG = 0;
            var bD = 0;
            var bN = 0;
            var bv = 0;
            var bE = 142;
            var bx = 122;
            var bL = parseInt(this.lI.offsetWidth);
            var bC = parseInt(this.lI.offsetHeight);
            var bw;
            if (this.lIj < 0) {
                bw = this.arrMaxRowCol[this.level];
                if (this.lIj == -1) {
                    bG = Math.round(bL / bw);
                    bD = Math.round(bC / bw)
                }
                bN = Math.round((bE - bG) / 2);
                bv = Math.round((bx - bD) / 2);
                if (this.lIj < -1) {
                    document.getElementById(this.id + "_VOverviewRect").style.border = 0 + "px";
                    document.getElementById(this.id + "_VCenterRect").style.border = 0 + "px";
                    bG = 0;
                    bD = 0
                } else {
                    document.getElementById(this.id + "_VOverviewRect").style.border = "#0066cc 2px double";
                    document.getElementById(this.id + "_VCenterRect").style.border = "#0066cc 2px double"
                }
                document.getElementById(this.id + "_VOverviewRect").style.left = bN + "px";
                document.getElementById(this.id + "_VOverviewRect").style.top = bv + "px";
                document.getElementById(this.id + "_VCenterRect").style.left = bN + "px";
                document.getElementById(this.id + "_VCenterRect").style.top = bv + "px";
                document.getElementById(this.id + "_VOverviewRect").style.width = bG + "px";
                document.getElementById(this.id + "_VOverviewRect").style.height = bD + "px";
                document.getElementById(this.id + "_VCenterRect").style.width = bG + "px";
                document.getElementById(this.id + "_VCenterRect").style.height = bD + "px";
                this.nInstalment = bw;
                this.lIj = 0
            } else {
                bw = Math.pow(2, this.level - this.lIj);
                bG = Math.round(bL / bw);
                bD = Math.round(bC / bw);
                bN = Math.round((bE - bG) / 2);
                bv = Math.round((bx - bD) / 2);
                document.getElementById(this.id + "_VOverviewRect").style.border = "#0066cc 2px double";
                document.getElementById(this.id + "_VCenterRect").style.border = "#0066cc 2px double";
                document.getElementById(this.id + "_VOverviewRect").style.left = bN + "px";
                document.getElementById(this.id + "_VOverviewRect").style.top = bv + "px";
                document.getElementById(this.id + "_VCenterRect").style.left = bN + "px";
                document.getElementById(this.id + "_VCenterRect").style.top = bv + "px";
                document.getElementById(this.id + "_VOverviewRect").style.width = bG + "px";
                document.getElementById(this.id + "_VOverviewRect").style.height = bD + "px";
                document.getElementById(this.id + "_VCenterRect").style.width = bG + "px";
                document.getElementById(this.id + "_VCenterRect").style.height = bD + "px";
                this.nInstalment = bw
            }
            var bH = this.llI * 256 - parseInt(bd.style.left) - parseInt(bd.childNodes[0].childNodes[0].style.left) + bL / 2;
            var bI = this.jl * 256 - parseInt(bd.style.top) - parseInt(bd.childNodes[0].childNodes[0].style.top) + bC / 2;
            var bF = bH / this.nInstalment;
            var bz = bI / this.nInstalment;
            var bP = bF % 256;
            if (bP <= bE / 2) {
                this.liI = parseInt(bF / 256) - 1;
                var bM = bE / 2 - bP - 256;
                if (bM > 0) {
                    this.liI--;
                    bM -= 256
                }
                document.getElementById(this.id + "_VOverviewBox").style.left = bM + "px"
            } else {
                this.liI = parseInt(bF / 256);
                var bM = bE / 2 - bP;
                if (bM > 0) {
                    this.liI--;
                    bM -= 256
                }
            }
            document.getElementById(this.id + "_VOverviewBox").style.left = bM + "px";
            var bO = bz % 256;
            if (bO <= bx / 2) {
                this.l1j = parseInt(bz / 256) - 1;
                document.getElementById(this.id + "_VOverviewBox").style.top = bx / 2 - bO - 256 + "px"
            } else {
                this.l1j = parseInt(bz / 256);
                document.getElementById(this.id + "_VOverviewBox").style.top = bx / 2 - bO + "px"
            }
            bw = Math.pow(2, this.lIj);
            for (var bK = 0; bK < 2; bK++) {
                for (var bJ = 0; bJ < 2; bJ++) {
                    document.getElementById(this.id + "s_" + bK + "_" + bJ).src = "";
                    document.getElementById(this.id + "s_" + bK + "_" + bJ).src = strResURL + "images/whiteimage.png";
                    if ((this.l1j + bK) >= 0 && (this.l1j + bK) < bw) {
                        var bB = parseInt(((this.l1j + bK) * bw + this.liI + bJ) / LimitFolder);
                        var bA = this.l1j + bK;
                        var by = this.liI + bJ;
                        by = ((by % bw) + bw) % bw;
                        document.getElementById(this.id + "s_" + bK + "_" + bJ).src = this.getVbdMapsTileUrl(this.lIj, by, bA)
                    }
                }
            }
        }
    }

    function az(bw) {
        var bv = (document.body.scrollLeft == 0) ? document.documentElement.scrollLeft : document.body.scrollLeft;
        var i = (document.body.scrollTop == 0) ? document.documentElement.scrollTop : document.body.scrollTop;
        lI1 = (browserName != "msie") ? bw.pageX : bw.clientX + bv;
        lIl = (browserName != "msie") ? bw.pageY : bw.clientY + i
    }

    function a9(bx, bw) {
        var bv = Math.round((bx.pageX + bw.pageX) / 2);
        var i = Math.round((bx.pageY + bw.pageY) / 2);
        if (this.curGestureX == null || this.curGestureY == null) {
            this.rootGestureX = bv;
            this.rootGestureY = i
        } else {
            if (this.curGestureX == this.rootGestureX && this.curGestureY == this.rootGestureY) {
                this.rootGestureX = this.curGestureX;
                this.rootGestureY = this.curGestureY
            }
        }
        this.curGestureX = bv;
        this.curGestureY = i
    }

    function bo() {
        var bw = parseFloat(parseInt(this.lI.offsetWidth) / 2) - parseFloat(parseInt(bd.style.left)) - parseInt(bd.childNodes[0].childNodes[0].style.left) + 256 * this.llI;
        var i = parseFloat(parseInt(this.lI.offsetHeight) / 2) - parseFloat(parseInt(bd.style.top)) - parseInt(bd.childNodes[0].childNodes[0].style.top) + 256 * this.jl;
        var bv = VProjection.fromPixelToLatLng(new VPoint(bw, i), this.level);
        this.centerX = bv.longitude;
        this.centerY = bv.latitude
    }

    function bl(bB) {
        if (this.nZoomCount == 0) {
            this.nZoomCount = 8;
            this.iZoomStep = 0;
            this.prevDelta = bB
        } else {
            if (this.prevDelta * bB > 0) {
                this.nZoomCount += 8
            }
            return
        }
        aP.tX = 0;
        aP.tY = 0;
        bd.style.left = parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left) + "px";
        bd.style.top = parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top) + "px";
        at.style.left = bd.style.left;
        at.style.top = bd.style.top;
        var bx, bv, bz;
        for (bx = 0; bx < this.l1l; bx++) {
            for (bv = 0; bv < this.iI; bv++) {
                bz = at.childNodes[0].childNodes[bx * this.iI + bv];
                bz.style.width = "256px";
                bz.style.height = "256px";
                bz.src = strResURL + "images/transparent.png"
            }
        }
        var bw = at;
        at = bd;
        bd = bw;
        this.curMapType.getTileLayers()[0].lj = bd;
        var bA = lI1 - parseInt(at.style.left) - this.Left;
        var by = lIl - parseInt(at.style.top) - this.Top;
        this.divisorX = bA / 256;
        this.divisorY = by / 256;
        this.hideOverlays();
        VUtil.createZoomAnimation(bB, lI1, lIl);
        this.scrollWheelZoom(bB, lI1, lIl)
    }

    function aB(bR, bE, bD, bC) {
        if (this.zoomstartHandler != undefined) {
            this.zoomstartHandler()
        }
        if (!this.ll1l) {
            this.iZoomStep = this.nZoomCount
        }
        if ((bR > 0 && this.level + this.nZoomCount / 8 > this.maxLevel) || (bR < 0 && this.level - this.nZoomCount / 8 < this.minLevel)) {
            this.nZoomCount -= 8
        }
        if (this.prevDelta != bR) {
            this.prevDelta = bR
        }
        var bP = document.getElementById("vbdani_lefttop");
        var bQ = document.getElementById("vbdani_righttop");
        var by = document.getElementById("vbdani_leftbottom");
        var bA = document.getElementById("vbdani_rightbottom");
        if (this.iZoomStep == this.nZoomCount) {
            this.lI.dblRC = "false";
            var bO;
            if (bR > 0) {
                bO = this.level + this.nZoomCount / 8
            } else {
                bO = this.level - this.nZoomCount / 8
            } if (bO < 0) {
                bO = 0
            }
            var bJ = this.divisorX * 256;
            var bG = this.divisorY * 256;
            var bL = VProjection.fromPixelToLatLng(new VPoint(bJ + 256 * this.llI, bG + 256 * this.jl), this.level);
            var bK = VProjection.fromLatLngToPixel(bL, bO);
            var bI = bK.x - bE + this.Left + parseInt(this.lI.offsetWidth) / 2;
            var bF = bK.y - bD + this.Top + parseInt(this.lI.offsetHeight) / 2;
            bL = VProjection.fromPixelToLatLng(new VPoint(bI, bF), bO);
            this.nZoomCount = 0;
            this.iZoomStep = 0;
            this.prevDelta = 0;
            at.style.zIndex = MapAnimatedZIndex;
            bd.style.zIndex = MapInnerZIndex;
            if (bP != null) {
                document.body.removeChild(bP);
                document.body.removeChild(bQ);
                document.body.removeChild(by);
                document.body.removeChild(bA)
            }
            this.nZoomCount = 0;
            this.setCenter(bL, bO, this.curMapType, false);
            if (bC != null && bC != undefined) {
                bC()
            }
            return
        }
        this.iZoomStep++;
        var bx = Math.ceil(this.iZoomStep / 8);
        var bw = Math.pow(2, bx - 1);
        var bz = (this.iZoomStep % 8 == 0) ? 8 : this.iZoomStep % 8;
        var bN, bM;
        for (bN = 0; bN < this.nControls; bN++) {
            if (this.arrControls[bN].scrollWheelZoomHandler != undefined) {
                this.arrControls[bN].scrollWheelZoomHandler(bR)
            }
        }
        var bH, bB;
        if (bR > 0) {
            bB = 256 * bw + bz * 32 * bw
        } else {
            bB = 256 / bw - bz * 16 / bw
        }
        for (bN = 0; bN < this.l1l; bN++) {
            for (bM = 0; bM < this.iI; bM++) {
                bH = at.childNodes[0].childNodes[bN * this.iI + bM];
                bH.style.width = bB + "px";
                bH.style.height = bB + "px";
                bH.style.left = (bM * bB) + "px";
                bH.style.top = (bN * bB) + "px"
            }
        }
        var bv = 4;
        if (bR > 0) {
            at.style.left = parseInt(at.style.left) - Math.round(this.divisorX * 32 * bw) + "px";
            at.style.top = parseInt(at.style.top) - Math.round(this.divisorY * 32 * bw) + "px";
            if (bP != null) {
                bP.style.left = parseInt(bP.style.left) - bv + "px";
                bP.style.top = parseInt(bP.style.top) - bv + "px";
                bQ.style.left = parseInt(bQ.style.left) + bv + "px";
                bQ.style.top = parseInt(bQ.style.top) - bv + "px";
                by.style.left = parseInt(by.style.left) - bv + "px";
                by.style.top = parseInt(by.style.top) + bv + "px";
                bA.style.left = parseInt(bA.style.left) + bv + "px";
                bA.style.top = parseInt(bA.style.top) + bv + "px"
            }
        } else {
            at.style.left = parseInt(at.style.left) + Math.round(this.divisorX * 16 / bw) + "px";
            at.style.top = parseInt(at.style.top) + Math.round(this.divisorY * 16 / bw) + "px";
            if (bP != null) {
                bP.style.left = parseInt(bP.style.left) + bv + "px";
                bP.style.top = parseInt(bP.style.top) + bv + "px";
                bQ.style.left = parseInt(bQ.style.left) - bv + "px";
                bQ.style.top = parseInt(bQ.style.top) + bv + "px";
                by.style.left = parseInt(by.style.left) + bv + "px";
                by.style.top = parseInt(by.style.top) - bv + "px";
                bA.style.left = parseInt(bA.style.left) - bv + "px";
                bA.style.top = parseInt(bA.style.top) - bv + "px"
            }
        } if (this.iZoomStep > 8 && bP != null) {
            document.body.removeChild(bP);
            document.body.removeChild(bQ);
            document.body.removeChild(by);
            document.body.removeChild(bA)
        }
        setTimeout(function () {
            aP.ij.scrollWheelZoom(bR, bE, bD, bC)
        }, 2)
    }

    function x() {
        var bv = this.curMapType.getTileLayers();
        if (bv.length > 1) {
            for (var bw = 1; bw < bv.length; bw++) {
                for (var bz = 0; bz < this.l1l; bz++) {
                    for (var by = 0; by < this.iI; by++) {
                        var bx = bv[bw].lj.childNodes[0].childNodes[bz * this.iI + by];
                        bx.src = "";
                        bx.src = strResURL + "images/transparent.png"
                    }
                }
            }
        }
        for (var bz = 0; bz < this.Ill; bz++) {
            if (this.lli[bz].isHidden != undefined) {
                if (!this.lli[bz].isHidden()) {
                    this.lli[bz].hide();
                    this.lli[bz].hideWhenZoom = "hideWhenZoom"
                }
            }
        }
        var bA = this.getInfoWindow();
        if (bA.isOpened() && !bA.isHidden()) {
            bA.hide();
            bA.hideWhenZoom = "hideWhenZoom"
        }
    }

    function aK() {
        for (var bv = 0; bv < this.Ill; bv++) {
            if (this.lli[bv].isHidden != undefined) {
                if (this.lli[bv].hideWhenZoom != undefined && this.lli[bv].hideWhenZoom == "hideWhenZoom") {
                    this.lli[bv].show();
                    this.lli[bv].hideWhenZoom = ""
                }
            }
        }
        var bw = this.getInfoWindow();
        if (bw.hideWhenZoom != undefined && bw.hideWhenZoom == "hideWhenZoom") {
            bw.show();
            bw.hideWhenZoom = ""
        }
    }

    function aN() {
        for (var bv = 0; bv < this.Ill; bv++) {
            if (this.lli[bv].vType != undefined && this.lli[bv].vType() == "VPolyline" || this.lli[bv].vType() == "VPolygon") {
                this.lli[bv].redraw(true)
            }
        }
    }

    function bn() {
        for (var bv = 0; bv < this.nControls; bv++) {
            if (this.arrControls[bv].redrawRuler != undefined) {
                this.arrControls[bv].redrawRuler()
            }
        }
    }

    function a6() {
        var bA = document.getElementById(this.id + "_Units");
        if (bA != null) {
            var bz = [10, 5, 2];
            var by = -1;
            var i = 1000000;
            var bx = 0;
            var bw = 0;
            var bv = 0;
            do {
                by++;
                if (by > 2) {
                    by = 0;
                    i /= 10
                }
                bx = bz[by] * i;
                bw = VProjection.getGroundResolution(this.centerY, this.level);
                bv = bx / bw;
                if (i <= 0.1) {
                    break
                }
            } while (bv > 130);
            bA.innerHTML = (bx >= 1000) ? ((bx / 1000) + " km") : (bx + " m");
            document.getElementById(this.id + "_Ruler").style.width = bv + "px"
        }
    }

    function aH() {
        if (document.getElementById(this.id + "_VOverviewMapControl") != null) {
            this.lIj = this.level - 4;
            var bG = 0;
            var bD = 0;
            var bM = 0;
            var bv = 0;
            var bE = 142;
            var bx = 122;
            var bL = parseInt(this.lI.offsetWidth);
            var bC = parseInt(this.lI.offsetHeight);
            var bw;
            if (this.lIj < 0) {
                bw = this.arrMaxRowCol[this.level];
                if (this.lIj == -1) {
                    bG = Math.round(bL / bw);
                    bD = Math.round(bC / bw)
                }
                bM = Math.round((bE - bG) / 2);
                bv = Math.round((bx - bD) / 2);
                if (this.lIj < -1) {
                    document.getElementById(this.id + "_VOverviewRect").style.border = 0 + "px";
                    document.getElementById(this.id + "_VCenterRect").style.border = 0 + "px";
                    bG = 0;
                    bD = 0
                } else {
                    document.getElementById(this.id + "_VOverviewRect").style.border = "#0066cc 2px double";
                    document.getElementById(this.id + "_VCenterRect").style.border = "#0066cc 2px double"
                }
                document.getElementById(this.id + "_VOverviewRect").style.left = bM + "px";
                document.getElementById(this.id + "_VOverviewRect").style.top = bv + "px";
                document.getElementById(this.id + "_VCenterRect").style.left = bM + "px";
                document.getElementById(this.id + "_VCenterRect").style.top = bv + "px";
                document.getElementById(this.id + "_VOverviewRect").style.width = bG + "px";
                document.getElementById(this.id + "_VOverviewRect").style.height = bD + "px";
                document.getElementById(this.id + "_VCenterRect").style.width = bG + "px";
                document.getElementById(this.id + "_VCenterRect").style.height = bD + "px";
                this.nInstalment = bw;
                this.lIj = 0
            } else {
                bw = Math.pow(2, this.level - this.lIj);
                bG = Math.round(bL / bw);
                bD = Math.round(bC / bw);
                bM = Math.round((bE - bG) / 2);
                bv = Math.round((bx - bD) / 2);
                document.getElementById(this.id + "_VOverviewRect").style.border = "#0066cc 2px double";
                document.getElementById(this.id + "_VCenterRect").style.border = "#0066cc 2px double";
                document.getElementById(this.id + "_VOverviewRect").style.left = bM + "px";
                document.getElementById(this.id + "_VOverviewRect").style.top = bv + "px";
                document.getElementById(this.id + "_VCenterRect").style.left = bM + "px";
                document.getElementById(this.id + "_VCenterRect").style.top = bv + "px";
                document.getElementById(this.id + "_VOverviewRect").style.width = bG + "px";
                document.getElementById(this.id + "_VOverviewRect").style.height = bD + "px";
                document.getElementById(this.id + "_VCenterRect").style.width = bG + "px";
                document.getElementById(this.id + "_VCenterRect").style.height = bD + "px";
                this.nInstalment = bw
            }
            var bH = this.llI * 256 - parseInt(bd.style.left) - parseInt(bd.childNodes[0].childNodes[0].style.left) + bL / 2;
            var bI = this.jl * 256 - parseInt(bd.style.top) - parseInt(bd.childNodes[0].childNodes[0].style.top) + bC / 2;
            var bF = bH / this.nInstalment;
            var bz = bI / this.nInstalment;
            var bO = bF % 256;
            if (bO <= bE / 2) {
                this.liI = parseInt(bF / 256) - 1;
                document.getElementById(this.id + "_VOverviewBox").style.left = bE / 2 - bO - 256 + "px"
            } else {
                this.liI = parseInt(bF / 256);
                document.getElementById(this.id + "_VOverviewBox").style.left = bE / 2 - bO + "px"
            }
            var bN = bz % 256;
            if (bN <= bx / 2) {
                this.l1j = parseInt(bz / 256) - 1;
                document.getElementById(this.id + "_VOverviewBox").style.top = bx / 2 - bN - 256 + "px"
            } else {
                this.l1j = parseInt(bz / 256);
                document.getElementById(this.id + "_VOverviewBox").style.top = bx / 2 - bN + "px"
            }
            bw = Math.pow(2, this.lIj);
            for (var bK = 0; bK < 2; bK++) {
                for (var bJ = 0; bJ < 2; bJ++) {
                    document.getElementById(this.id + "s_" + bK + "_" + bJ).src = "";
                    document.getElementById(this.id + "s_" + bK + "_" + bJ).src = strResURL + "images/whiteimage.png";
                    if ((this.l1j + bK) >= 0 && (this.l1j + bK) < bw) {
                        var bB = parseInt(((this.l1j + bK) * bw + this.liI + bJ) / LimitFolder);
                        var bA = this.l1j + bK;
                        var by = this.liI + bJ;
                        by = ((by % bw) + bw) % bw;
                        document.getElementById(this.id + "s_" + bK + "_" + bJ).src = this.getVbdMapsTileUrl(this.lIj, by, bA)
                    }
                }
            }
        }
    }

    function B() {
        if (browserName != "msie") {
            var i = C.childNodes[1].childNodes[0];
            if (i.childNodes.length == 0) {
                i.setAttribute("width", "");
                i.setAttribute("height", "")
            }
        }
    }

    function bu(bB, bz) {
        var bC = this.getInfoWindow();
        var bA = VProjection.fromLatLngToPixel(bC.getPoint(), bB);
        var bv = VProjection.fromLatLngToPixel(this.getCenter(), bB);
        var by = bv.x - bA.x;
        var bx = bv.y - bA.y;
        var i = VProjection.fromLatLngToPixel(bC.getPoint(), bz);
        var bD = new VPoint(i.x + by, i.y + bx);
        var bw = VProjection.fromPixelToLatLng(bD, bz);
        return bw
    }

    function aQ() {
        for (var bv = 0; bv < this.nControls; bv++) {
            if (this.arrControls[bv].hide != undefined) {
                this.arrControls[bv].hide()
            }
        }
    }

    function t() {
        for (var bv = 0; bv < this.nControls; bv++) {
            if (this.arrControls[bv].show != undefined) {
                this.arrControls[bv].show()
            }
        }
    }

    function aD(bE) {
        if (!this.isMarkerAutoPanning) {
            return
        }
        var bD, bw;
        bD = parseFloat(bE.style.left) + bE.II.Il.offset.width + bE.dx;
        bw = parseFloat(bE.style.top) + bE.II.Il.offset.height + bE.dy;
        bE.style.left = parseFloat(bE.style.left) + bE.dx + "px";
        bE.style.top = parseFloat(bE.style.top) + bE.dy + "px";
        var by = this.fromDivPixelToLatLng(new VPoint(bD, bw));
        bE.II.latlngs.latitude = by.latitude;
        bE.II.latlngs.longitude = by.longitude;
        this.tLeft = parseInt(bd.style.left);
        this.tTop = parseInt(bd.style.top);
        this.sLeft = parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left);
        this.sTop = parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top);
        this.stLeft = parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left);
        this.stTop = parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top);
        var bB = parseInt(bd.style.left) - Math.round(bE.dx) + "px";
        var bz = parseInt(bd.style.top) - Math.round(bE.dy) + "px";
        var i = this.curMapType.getTileLayers();
        for (var bx = 0; bx < i.length; bx++) {
            i[bx].lj.style.left = bB;
            i[bx].lj.style.top = bz
        }
        if (bi != undefined) {
            bi.style.left = bB;
            bi.style.top = bz
        }
        this.stLeft = this.stLeft - Math.round(bE.dx);
        this.stTop = this.stTop - Math.round(bE.dy);
        this.sLeft = -Math.round(bE.dx) + this.stLeft;
        this.sTop = -Math.round(bE.dy) + this.stTop;
        C.style.left = parseInt(C.style.left) - Math.round(bE.dx) + "px";
        C.style.top = parseInt(C.style.top) - Math.round(bE.dy) + "px";
        at.style.left = parseInt(at.style.left) - Math.round(bE.dx) + "px";
        at.style.top = parseInt(at.style.top) - Math.round(bE.dy) + "px";
        if (this.sLeft > 0) {
            this.insertLeft();
            this.stLeft -= 256
        }
        if (this.sLeft < -256) {
            this.insertRight();
            this.stLeft += 256
        }
        if (this.sTop > 0) {
            this.insertTop();
            this.stTop -= 256
        }
        if (this.sTop < -256) {
            this.insertBottom();
            this.stTop += 256
        }
        var bC, bA;
        var bv = VProjection.fromLatLngToPixel(this.getCenter(), this.getLevel());
        bC = bv.x + bE.dx;
        bA = bv.y + bE.dy;
        var by = VProjection.fromPixelToLatLng(new VPoint(bC, bA), this.level);
        this.centerX = by.longitude;
        this.centerY = by.latitude;
        if (Math.abs(bE.dx) >= parseInt(this.lI.offsetWidth) || Math.abs(bE.dy) >= parseInt(this.lI.offsetHeight)) {
            this.setCenter(new VLatLng(this.centerY, this.centerX), this.level)
        }
        this.fixOverView();
        this.redrawLines();
        if (this.jjj && this.isMarkerAutoPanning) {
            setTimeout(function () {
                aP.ij.panByMarker(bE)
            }, 1)
        }
    }

    function r() {
        this.draggable = true
    }

    function ae() {
        this.draggable = false
    }

    function G() {
        return this.draggable
    }

    function I() {
        this.bEnableInfoWindow = true
    }

    function af() {
        this.bEnableInfoWindow = false
    }

    function ak() {
        return this.bEnableInfoWindow
    }

    function g() {
        this.dblClickZoomable = true
    }

    function N() {
        this.dblClickZoomable = false
    }

    function bt() {
        return this.dblClickZoomable
    }

    function aw() {
        this.ll1l = true
    }

    function bf() {
        this.ll1l = false
    }

    function O() {
        return this.ll1l
    }

    function aW() {
        this.jij = true
    }

    function bh() {
        this.jij = false
    }

    function aL() {
        return this.jij
    }

    function K(bw, i) {
        if (bw != undefined) {
            bw.initialize(this, i);
            bw.arrIndex = this.nControls;
            this.arrControls[this.nControls] = bw;
            this.nControls++;
            var bv = document.getElementById(bw.id);
            if (bv != null) {
                bv.style.zIndex = ControlZIndex;
                bv.className = "clickcontrol " + bv.className
            }
            if (bw.vType() == "VLargeMapControl") {
                this.redrawRuler()
            } else {
                if (bw.vType() == "VOverviewMapControl") {
                    this.redrawOverviewMap()
                }
            }
        }
    }

    function c(bw) {
        for (var bv = bw.arrIndex; bv < this.nControls - 1; bv++) {
            this.arrControls[bv] = this.arrControls[bv + 1];
            this.arrControls[bv].arrIndex = this.arrControls[bv].arrIndex - 1
        }
        this.nControls--;
        this.arrControls[this.nControls] = null;
        bw.remove()
    }

    function ad() {
        for (var bv = 0; bv < this.nControls; bv++) {
            this.arrControls[bv].remove()
        }
        this.nControls = 0
    }

    function an() {
        return this.lI
    }

    function aV(i) {
        i.arrIndex = this.nMapTypes;
        this.arrMapTypes[this.nMapTypes] = i;
        this.nMapTypes++
    }

    function ag() { }

    function n(i) {
        this.curMappingLayer = i;
        if (i != null) {
            this.initMap(false);
            this.setCenter(this.getCenter(), this.level, this.curMapType, true)
        }
    }

    function bj() {
        return this.llIj
    }

    function aY() {
        return new VPoint(lI1, lIl)
    }

    function P() {
        return new VLatLng(this.centerY, this.centerX)
    }

    function aA(i) {
        if (i.latitude < -89) {
            i.latitude = -85
        } else {
            if (i.latitude > 89) {
                i.latitude = 85
            }
        } if (i.longitude < -179) {
            i.longitude = -175
        } else {
            if (i.longitude > 179) {
                i.longitude = 175
            }
        }
        return i
    }

    function a4() {
        var i = this.fromContainerPixelToLatLng(new VPoint(0, 0));
        var bv = this.fromContainerPixelToLatLng(new VPoint(this.lI.offsetWidth, this.lI.offsetHeight));
        i = aA(i);
        bv = aA(bv);
        return new VLatLngBounds(i, bv)
    }

    function l() {
        var i = this.fromContainerPixelToLatLng(new VPoint(0, 0));
        var bw = this.fromContainerPixelToLatLng(new VPoint(this.lI.offsetWidth, this.lI.offsetHeight));
        var bx = Math.abs(bw.longitude - i.longitude);
        var bv = Math.abs(bw.latitude - i.latitude);
        i.longitude -= bx;
        bw.longitude += bx;
        i.latitude += bv;
        bw.latitude -= bv;
        i = aA(i);
        bw = aA(bw);
        return new VLatLngBounds(i, bw)
    }

    function aj(bA) {
        if (bA == null || bA == undefined) {
            return 0
        }
        var i = this.level;
        var bx = 0;
        this.level = this.maxLevel;
        var by = this.fromLatLngToContainerPixel(bA.nw);
        var bw = this.fromLatLngToContainerPixel(bA.se);
        var bv = parseInt(bw.x - by.x);
        var bz = parseInt(bw.y - by.y);
        if (bv / this.lI.offsetWidth > bz / this.lI.offsetHeight) {
            if (bv / this.lI.offsetWidth == 1) {
                bx = this.level
            } else {
                if (bv / this.lI.offsetWidth > 1) {
                    while (bv / this.lI.offsetWidth > 1) {
                        this.level--;
                        if (this.level < this.minLevel || this.level > this.maxLevel) {
                            break
                        }
                        by = this.fromLatLngToContainerPixel(bA.nw);
                        bw = this.fromLatLngToContainerPixel(bA.se);
                        bv = parseInt(bw.x - by.x)
                    }
                    bx = this.level
                }
            }
        } else {
            if (bz / this.lI.offsetHeight == 1) {
                bx = this.level
            } else {
                if (bz / this.lI.offsetHeight > 1) {
                    while (bz / this.lI.offsetHeight > 1) {
                        this.level--;
                        if (this.level < this.minLevel || this.level > this.maxLevel) {
                            break
                        }
                        by = this.fromLatLngToContainerPixel(bA.nw);
                        bw = this.fromLatLngToContainerPixel(bA.se);
                        bz = parseInt(bw.y - by.y)
                    }
                    bx = this.level
                }
            }
        }
        this.level = i;
        return bx
    }

    function br(bH, bv) {
        if (bv < this.minLevel || bv > this.maxLevel) {
            return null
        }
        var bx = VProjection.fromLatLngToPixel(bH, bv);
        var bE = bx.x;
        var bC = bx.y;
        var bw = parseInt((bC - parseFloat(parseInt(this.lI.offsetHeight) / 2)) / 256);
        var by = parseInt((bE - parseFloat(parseInt(this.lI.offsetWidth) / 2)) / 256);
        var bA = -parseInt((bE - parseFloat(parseInt(this.lI.offsetWidth) / 2)) % 256);
        var i = -parseInt((bC - parseFloat(parseInt(this.lI.offsetHeight) / 2)) % 256);
        if (bE < parseInt(this.lI.offsetWidth) / 2) {
            by--;
            bA -= 256
        }
        if (bC < parseInt(this.lI.offsetHeight) / 2) {
            bw--;
            i -= 256
        }
        var bG = -this.lI.offsetWidth * 2 + 256 * by - bA;
        var bD = -this.lI.offsetHeight * 2 + 256 * bw - i;
        var bB = VProjection.fromPixelToLatLng(new VPoint(bG, bD), bv);
        var bz = new VLatLng(bB.latitude, bB.longitude);
        bG = this.lI.offsetWidth * 3 + 256 * by - bA;
        bD = this.lI.offsetHeight * 3 + 256 * bw - i;
        bB = VProjection.fromPixelToLatLng(new VPoint(bG, bD), bv);
        var bF = new VLatLng(bB.latitude, bB.longitude);
        return new VLatLngBounds(bz, bF)
    }

    function p() {
        return new VSize(this.lI.offsetWidth, this.lI.offsetHeight)
    }

    function a5() {
        return this.level
    }

    function ao() { }

    function a2(bD) {
        var bC = (this.curMappingLayer != null) ? this.curMappingLayer.mappedTileLayer : -1;
        var bG = new Array();
        var bx = new Array();
        var bv = this.curMapType.getTileLayers();
        if (bD >= bv.length) {
            return
        }
        var bF = bv[bD].lj;
        var bE = this.arrMaxRowCol[this.level];
        var bB = null,
            bw = "";
        for (var bA = 0; bA < this.l1l; bA++) {
            for (var bz = 0; bz < this.iI; bz++) {
                bB = bF.childNodes[0].childNodes[bA * this.iI + bz];
                bB.src = "";
                bB.src = strResURL + "images/transparent.png";
                if ((this.jl + bA) >= 0 && (this.jl + bA) < bE) {
                    var bH = this.jl + bA;
                    var by = this.llI + bz;
                    if (this.curMapType.opts.degree360) {
                        by = ((by % bE) + bE) % bE
                    }
                    if (bB.id != null && bB.id != "") {
                        bB.id = by + "_" + bH
                    }
                    bw = bv[bD].getTileUrl(new VPoint(by, bH), this.level);
                    if (bw != null && bw != "") {
                        bB.src = bw
                    }
                    if (bD == bC) {
                        bG[bG.length] = bB.id;
                        bx[bx.length] = new VPoint(by, bH)
                    }
                } else {
                    if (bD == 0) {
                        bB.src = strResURL + "images/whiteimage.png"
                    } else {
                        bB.src = strResURL + "images/transparent.png"
                    }
                }
            }
        }
        if (bD == bC) {
            this.curMappingLayer.getMappingAreas(bG, bx, this.level)
        }
    }

    function bm(by, bv, bP, bG) {
        var bO, bN, bM;
        if (isValidKey == false) {
            for (bO = 0; bO < this.l1l; bO++) {
                for (bN = 0; bN < this.iI; bN++) {
                    bd.childNodes[0].childNodes[bO * this.iI + bN].src = strResURL + "images/invalidkey.jpg"
                }
            }
            return
        }
        if (bv < this.minLevel || bv > this.maxLevel) {
            return
        }
        if (this.boundsRestrict != null) {
            if (by.lng() < this.boundsRestrict.getNorthWest().lng() || by.lng() > this.boundsRestrict.getSouthEast().lng() || by.lat() < this.boundsRestrict.getSouthEast().lat() || by.lat() > this.boundsRestrict.getNorthWest().lat()) {
                this.setCenter(new VLatLng((this.boundsRestrict.getNorthWest().lat() + this.boundsRestrict.getSouthEast().lat()) / 2, (this.boundsRestrict.getNorthWest().lng() + this.boundsRestrict.getSouthEast().lng()) / 2), bv);
                return
            }
        }
        this.centerX = by.longitude;
        this.centerY = by.latitude;
        var bK = this.level;
        this.level = bv;
        this.redrawRuler();
        if (bP == undefined) {
            bP = this.curMapType
        }
        if (this.curMapType.arrIndex != bP.arrIndex) {
            this.curMapType = bP;
            this.initMap(false)
        }
        var bL = VProjection.fromLatLngToPixel(this.getCenter(), this.getLevel());
        var bI = bL.x - parseInt(this.lI.offsetWidth) / 2;
        var bH = bL.y - parseInt(this.lI.offsetHeight) / 2;
        this.llI = parseInt(bI / 256);
        this.jl = parseInt(bH / 256);
        var bF = -parseInt(bI % 256);
        var bB = -parseInt(bH % 256);
        if (bI < 0) {
            this.llI--;
            bF -= 256
        }
        if (bH < 0) {
            this.jl--;
            bB -= 256
        }
        var bS = (this.curMappingLayer != null) ? this.curMappingLayer.mappedTileLayer : -1;
        var bx = null,
            bJ = null;
        var bT = this.curMapType.getTileLayers();
        var bE, bU = null,
            bR = "";
        var bw = this.arrMaxRowCol[this.level];
        var bD, bA;
        if (bG != false) {
            for (bO = 0; bO < this.l1l; bO++) {
                for (bN = 0; bN < this.iI; bN++) {
                    at.childNodes[0].childNodes[bO * this.iI + bN].src = strResURL + "images/whiteimage.png"
                }
            }
        }
        for (bM = 0; bM < bT.length; bM++) {
            bE = bT[bM].lj;
            bE.style.left = bF + "px";
            bE.style.top = bB + "px";
            bx = new Array();
            bJ = new Array();
            for (bO = 0; bO < this.l1l; bO++) {
                for (bN = 0; bN < this.iI; bN++) {
                    bU = bE.childNodes[0].childNodes[bO * this.iI + bN];
                    if (bG != false) {
                        bU.src = strResURL + "images/whiteimage.png"
                    } else {
                        bU.src = strResURL + "images/transparent.png"
                    }
                    bU.style.left = (256 * bN) + "px";
                    bU.style.top = (256 * bO) + "px";
                    if ((this.jl + bO) >= 0 && (this.jl + bO) < bw) {
                        bD = this.jl + bO;
                        bA = this.llI + bN;
                        if (this.curMapType.opts.degree360) {
                            bA = ((bA % bw) + bw) % bw
                        }
                        if (bU.id != null && bU.id != "") {
                            bU.id = bA + "_" + bD
                        }
                        bR = bT[bM].getTileUrl(new VPoint(bA, bD), this.level);
                        if (bR != null && bR != "") {
                            bU.src = bR
                        }
                        if (bM == bS) {
                            bx[bx.length] = bU.id;
                            bJ[bJ.length] = new VPoint(bA, bD)
                        }
                    }
                }
            }
            if (bM == bS) {
                this.curMappingLayer.getMappingAreas(bx, bJ, this.level)
            }
        }
        if (bi != undefined) {
            bi.style.left = bF + "px";
            bi.style.top = bB + "px"
        }
        this.redrawScaleCtrl();
        this.showOverlays();
        for (bO = 0; bO < this.Ill; bO++) {
            this.lli[bO].redraw(false)
        }
        this.redrawOverviewMap();
        if (!this.llIj && this.iIi != undefined) {
            this.iIi()
        }
        this.llIj = true;
        if (this.i1I.isOpened()) {
            this.i1I.redraw()
        }
        if (this.lij != undefined && !bG) {
            for (var bz = 0; bz < this.lij.length; bz++) {
                this.lij[bz]()
            }
        }
        if (this.j1I != undefined && bK != bv) {
            this.j1I(bK, bv)
        }
        if (this.boundsRestrict != null) {
            var bC = VProjection.fromLatLngToPixel(this.boundsRestrict.getNorthWest(), this.level);
            var bQ = VProjection.fromLatLngToPixel(this.boundsRestrict.getSouthEast(), this.level);
            this.ptBdResNWV = new VPoint(256 * this.llI - bC.x, 256 * this.jl - bC.y);
            this.ptBdResSEV = new VPoint(256 * this.llI - bQ.x + this.lI.offsetWidth, 256 * this.jl - bQ.y + this.lI.offsetHeight)
        }
        if (!VUtil.isVBDWebsite()) {
            initializeTimer()
        }
    }

    function av(bx, bv) {
        if (this.boundsRestrict != null) {
            var bw = parseInt(bd.style.left) - bx.width;
            var i = parseInt(bd.style.top) - bx.height;
            if (bw < ap.ij.ptBdResSEV.x || bw > ap.ij.ptBdResNWV.x || i < ap.ij.ptBdResSEV.y || i > ap.ij.ptBdResNWV.y) {
                return
            }
        }
        this.processPanBy(bx, bv)
    }

    function u(bv, bx) {
        if (this.jji != undefined) {
            this.jji()
        }
        if (this.ij1 <= 0 || Math.abs(bv.width) >= parseInt(this.lI.offsetWidth) || Math.abs(bv.height) >= parseInt(this.lI.offsetHeight)) {
            this.lI.dblRC = "false";
            this.ij1 = 8;
            var bE, bD;
            var bw = VProjection.fromLatLngToPixel(this.getCenter(), this.getLevel());
            bE = bw.x + bv.width;
            bD = bw.y + bv.height;
            var bA = VProjection.fromPixelToLatLng(new VPoint(bE, bD), this.level);
            this.centerX = bA.longitude;
            this.centerY = bA.latitude;
            if (Math.abs(bv.width) >= parseInt(this.lI.offsetWidth) || Math.abs(bv.height) >= parseInt(this.lI.offsetHeight)) {
                this.setCenter(new VLatLng(this.centerY, this.centerX), this.level)
            } else {
                this.updateMappingAreas()
            }
            this.redrawScaleCtrl();
            this.fixOverView();
            this.redrawLines();
            if (this.lij != undefined) {
                for (var bz = 0; bz < this.lij.length; bz++) {
                    this.lij[bz]()
                }
            }
            if (document.getElementById("VIWC") != null) {
                document.getElementById("VIWC").style.overflow = "auto"
            }
            if (!VUtil.isVBDWebsite()) {
                initializeTimer()
            }
            return
        }
        if (this.ij1 == 8) {
            if (document.getElementById("VIWC") != null) {
                document.getElementById("VIWC").style.overflow = "hidden"
            }
            if (this.li1 != undefined && (bv.width > 0 || bv.height > 0)) {
                this.li1()
            }
            this.tLeft = parseInt(bd.style.left);
            this.tTop = parseInt(bd.style.top);
            this.sLeft = parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left);
            this.sTop = parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top);
            this.stLeft = parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left);
            this.stTop = parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top)
        }
        this.ij1--;
        var bC = parseInt(bd.style.left) - Math.round(bv.width / 8) + "px";
        var bB = parseInt(bd.style.top) - Math.round(bv.height / 8) + "px";
        var i = this.curMapType.getTileLayers();
        for (var by = 0; by < i.length; by++) {
            i[by].lj.style.left = bC;
            i[by].lj.style.top = bB
        }
        if (bi != undefined) {
            bi.style.left = bC;
            bi.style.top = bB
        }
        this.stLeft = this.stLeft - Math.round(bv.width / 8);
        this.stTop = this.stTop - Math.round(bv.height / 8);
        this.sLeft = -Math.round(bv.width / 8) + this.stLeft;
        this.sTop = -Math.round(bv.height / 8) + this.stTop;
        C.style.left = parseInt(C.style.left) - Math.round(bv.width / 8) + "px";
        C.style.top = parseInt(C.style.top) - Math.round(bv.height / 8) + "px";
        at.style.left = parseInt(at.style.left) - Math.round(bv.width / 8) + "px";
        at.style.top = parseInt(at.style.top) - Math.round(bv.height / 8) + "px";
        if (this.sLeft > 0) {
            this.insertLeft();
            this.stLeft -= 256
        }
        if (this.sLeft < -256) {
            this.insertRight();
            this.stLeft += 256
        }
        if (this.sTop > 0) {
            this.insertTop();
            this.stTop -= 256
        }
        if (this.sTop < -256) {
            this.insertBottom();
            this.stTop += 256
        }
        if ((bx == undefined) || (bx != undefined && bx == true)) {
            setTimeout(function () {
                aP.ij.processPanBy(new VSize(bv.width, bv.height))
            }, 30)
        }
    }

    function aT(bv) {
        var bx = this.fromLatLngToDivPixel(bv);
        var i = this.fromLatLngToDivPixel(this.getCenter());
        var by = bx.x - i.x;
        var bw = bx.y - i.y;
        this.panBy(new VSize(by, bw))
    }

    function k(bv, i) {
        if (bv == -1) {
            bv = -(this.lI.offsetWidth / 2)
        } else {
            if (bv == 0) {
                bv = 0
            } else {
                if (bv == 1) {
                    bv = this.lI.offsetWidth / 2
                }
            }
        } if (i == -1) {
            i = -(this.lI.offsetHeight / 2)
        } else {
            if (i == 0) {
                i = 0
            } else {
                if (i == 1) {
                    i = this.lI.offsetHeight / 2
                }
            }
        }
        this.panBy(new VSize(bv, i))
    }

    function h(bw) {
        this.boundsRestrict = bw;
        if (this.boundsRestrict != null) {
            var bz = 0;
            var i = this.IlI - 1;
            var bv = VProjection.fromLatLngToPixel(this.boundsRestrict.getNorthWest(), bz);
            var by = VProjection.fromLatLngToPixel(this.boundsRestrict.getSouthEast(), bz);
            if (by.x - bv.x - 5 <= this.lI.offsetWidth || by.y - bv.y - 5 <= this.lI.offsetHeight) {
                while (bz < this.IlI) {
                    bz++;
                    if (bz >= this.IlI) {
                        break
                    }
                    bv = VProjection.fromLatLngToPixel(this.boundsRestrict.getNorthWest(), bz);
                    by = VProjection.fromLatLngToPixel(this.boundsRestrict.getSouthEast(), bz);
                    if (by.x - bv.x - 5 > this.lI.offsetWidth && by.y - bv.y - 5 > this.lI.offsetHeight) {
                        break
                    }
                }
            }
            if (bz < this.IlI) {
                var bx = this.level;
                this.minLevel = (bz != null) ? bz : 0;
                this.maxLevel = (i != null) ? i : this.IlI - 1;
                if (this.level < this.minLevel) {
                    bx = this.minLevel
                }
                if (this.level > this.maxLevel) {
                    bx = this.maxLevel
                }
                if (document.getElementById(this.id + "_barCountry") != null) {
                    document.getElementById(this.id + "_barCountry").style.top = (22 + (this.maxLevel - 5) * 8) + "px";
                    document.getElementById(this.id + "_barCity").style.top = (22 + (this.maxLevel - 9) * 8) + "px";
                    document.getElementById(this.id + "_barDistrict").style.top = (22 + (this.maxLevel - 13) * 8) + "px";
                    document.getElementById(this.id + "_barWard").style.top = (22 + (this.maxLevel - 17) * 8) + "px";
                    document.getElementById(this.id + "_rulerBar").style.height = ((this.maxLevel - this.minLevel) * 8) + "px";
                    this.redrawRuler()
                }
                bv = VProjection.fromLatLngToPixel(this.boundsRestrict.getNorthWest(), this.level);
                by = VProjection.fromLatLngToPixel(this.boundsRestrict.getSouthEast(), this.level);
                this.ptBdResNWV = new VPoint(256 * this.llI - bv.x, 256 * this.jl - bv.y);
                this.ptBdResSEV = new VPoint(256 * this.llI - by.x + this.lI.offsetWidth, 256 * this.jl - by.y + this.lI.offsetHeight);
                if (this.centerX < this.boundsRestrict.getNorthWest().lng() || this.centerX > this.boundsRestrict.getSouthEast().lng() || this.centerY < this.boundsRestrict.getSouthEast().lat() || this.centerY > this.boundsRestrict.getNorthWest().lat()) {
                    this.setCenter(new VLatLng((this.boundsRestrict.getNorthWest().lat() + this.boundsRestrict.getSouthEast().lat()) / 2, (this.boundsRestrict.getNorthWest().lng() + this.boundsRestrict.getSouthEast().lng()) / 2), bx)
                } else {
                    this.setCenter(this.getCenter(), bx)
                }
                return true
            }
        }
        this.minLevel = 0;
        this.maxLevel = this.IlI - 1;
        if (document.getElementById(this.id + "_barCountry") != null) {
            document.getElementById(this.id + "_barCountry").style.top = (22 + (this.maxLevel - 5) * 8) + "px";
            document.getElementById(this.id + "_barCity").style.top = (22 + (this.maxLevel - 9) * 8) + "px";
            document.getElementById(this.id + "_barDistrict").style.top = (22 + (this.maxLevel - 13) * 8) + "px";
            document.getElementById(this.id + "_barWard").style.top = (22 + (this.maxLevel - 17) * 8) + "px";
            document.getElementById(this.id + "_rulerBar").style.height = ((this.maxLevel - this.minLevel) * 8) + "px";
            this.redrawRuler()
        }
        return false
    }

    function aC(bv, i) {
        if (bv > i) {
            return
        }
        this.minLevel = (bv != null) ? bv : 0;
        this.maxLevel = (i != null) ? i : this.IlI - 1;
        if (this.level < this.minLevel) {
            this.zoomToLevel(this.minLevel)
        }
        if (this.level > this.maxLevel) {
            this.zoomToLevel(this.maxLevel)
        }
        if (document.getElementById(this.id + "_barCountry") != null) {
            document.getElementById(this.id + "_barCountry").style.top = (22 + (this.maxLevel - 5) * 8) + "px";
            document.getElementById(this.id + "_barCity").style.top = (22 + (this.maxLevel - 9) * 8) + "px";
            document.getElementById(this.id + "_barDistrict").style.top = (22 + (this.maxLevel - 13) * 8) + "px";
            document.getElementById(this.id + "_barWard").style.top = (22 + (this.maxLevel - 17) * 8) + "px";
            document.getElementById(this.id + "_rulerBar").style.height = ((this.maxLevel - this.minLevel) * 8) + "px";
            this.redrawRuler()
        }
    }

    function z() {
        if (this.level == this.maxLevel) {
            return
        }
        if (this.zoomstartHandler != undefined) {
            this.zoomstartHandler()
        }
        var bv = this.curMapType.getTileLayers();
        for (var bx = 0; bx < bv.length; bx++) {
            for (var bA = 0; bA < this.l1l; bA++) {
                for (var bz = 0; bz < this.iI; bz++) {
                    var by = bv[bx].lj.childNodes[0].childNodes[bA * this.iI + bz];
                    by.src = "";
                    by.src = strResURL + "images/whiteimage.png"
                }
            }
        }
        var bC = this.level + 1;
        var bw = this.getCenter();
        var bB = this.getInfoWindow();
        if (bB.isOpened()) {
            bw = this.getCenterForPreserveInfoWindow(this.level, bC)
        }
        this.setCenter(bw, bC)
    }

    function W() {
        if (this.level == this.minLevel) {
            return
        }
        if (this.zoomstartHandler != undefined) {
            this.zoomstartHandler()
        }
        var bv = this.curMapType.getTileLayers();
        for (var bx = 0; bx < bv.length; bx++) {
            for (var bA = 0; bA < this.l1l; bA++) {
                for (var bz = 0; bz < this.iI; bz++) {
                    var by = bv[bx].lj.childNodes[0].childNodes[bA * this.iI + bz];
                    by.src = "";
                    by.src = strResURL + "images/whiteimage.png"
                }
            }
        }
        var bC = this.level - 1;
        var bw = this.getCenter();
        var bB = this.getInfoWindow();
        if (bB.isOpened()) {
            bw = this.getCenterForPreserveInfoWindow(this.level, bC)
        }
        this.setCenter(bw, bC)
    }

    function aF() {
        var bv = this.curMapType.getTileLayers();
        var bz, by, bw, bx;
        for (bw = 0; bw < bv.length; bw++) {
            for (bz = 0; bz < this.l1l; bz++) {
                for (by = 0; by < this.iI; by++) {
                    bx = bv[bw].lj.childNodes[0].childNodes[bz * this.iI + by];
                    bx.src = "";
                    bx.src = strResURL + "images/whiteimage.png"
                }
            }
        }
        if (this.i1i != null) {
            this.i1i.fitMap()
        } else {
            var bA = 5;
            if (bA < this.minLevel) {
                bA = this.minLevel
            }
            if (bA > this.maxLevel) {
                bA = this.maxLevel
            }
            this.setCenter(new VLatLng(14.102783, 109.649506), bA)
        }
    }

    function aJ(bA) {
        if (bA < this.minLevel || bA > this.maxLevel) {
            return
        }
        if (this.zoomstartHandler != undefined) {
            this.zoomstartHandler()
        }
        var bv = this.curMapType.getTileLayers();
        var bz, by, bw, bx;
        for (bw = 0; bw < bv.length; bw++) {
            for (bz = 0; bz < this.l1l; bz++) {
                for (by = 0; by < this.iI; by++) {
                    bx = bv[bw].lj.childNodes[0].childNodes[bz * this.iI + by];
                    bx.src = "";
                    bx.src = strResURL + "images/whiteimage.png"
                }
            }
        }
        this.setCenter(new VLatLng(this.centerY, this.centerX), bA)
    }

    function bs() {
        this.llI1 = {
            point: this.getCenter(),
            level: this.getLevel()
        }
    }

    function a7() {
        if (this.llI1 != null) {
            this.setCenter(this.llI1.point, this.llI1.level)
        }
    }

    function aZ() {
        if (this.zoomstartHandler != undefined) {
            this.zoomstartHandler()
        }
        if (this.Ill == 0) {
            this.zoomFit();
            return
        }
        var bF = 0;
        var bD = 0;
        var bA = 0;
        var bx = 0;
        for (var bN = 0; bN < this.Ill; bN++) {
            var bB = this.lli[bN].latlngs;
            var bI = new Array();
            if (bB.length == undefined) {
                if (this.lli[bN].vType() == "VCircle") {
                    var bO = this.lli[bN].getBounds();
                    bI.push(bO.getNorthWest());
                    bI.push(bO.getSouthEast())
                } else {
                    bI[0] = bB
                }
            } else {
                if ((this.lli[bN].vType() == "VPolyline" || this.lli[bN].vType() == "VPolygon" || this.lli[bN].vType() == "VArrow") && this.lli[bN].isDirectionObj) {
                    for (var bM = 0; bM < bB.length; bM++) {
                        for (var bK = 0; bK < bB[bM].length; bK++) {
                            bI.push(bB[bM][bK])
                        }
                    }
                } else {
                    bI = bB
                }
            }
            for (var bM = 0; bM < bI.length; bM++) {
                if (bF == 0 && bD == 0 && bA == 0 && bx == 0) {
                    bF = parseFloat(bI[bM].longitude.toString());
                    bA = parseFloat(bI[bM].longitude.toString());
                    bD = parseFloat(bI[bM].latitude.toString());
                    bx = parseFloat(bI[bM].latitude.toString())
                }
                if (bF < parseFloat(bI[bM].longitude.toString())) {
                    bF = parseFloat(bI[bM].longitude.toString())
                }
                if (bA > parseFloat(bI[bM].longitude.toString())) {
                    bA = parseFloat(bI[bM].longitude.toString())
                }
                if (bD < parseFloat(bI[bM].latitude.toString())) {
                    bD = parseFloat(bI[bM].latitude.toString())
                }
                if (bx > parseFloat(bI[bM].latitude.toString())) {
                    bx = parseFloat(bI[bM].latitude.toString())
                }
            }
        }
        var bL = new VLatLng(parseFloat((bD + bx) / 2), parseFloat((bF + bA) / 2));
        var bz = new Array();
        bz[0] = bF;
        bz[1] = bD;
        var bH = VProjection.fromLatLngToPixel(new VLatLng(bz[1], bz[0]), this.IlI - 1);
        var bw = bH.x;
        var bv = bH.y;
        bz = new Array();
        bz[0] = bA;
        bz[1] = bx;
        bH = VProjection.fromLatLngToPixel(new VLatLng(bz[1], bz[0]), this.IlI - 1);
        var bG = bH.x;
        var bE = bH.y;
        var bC = Math.abs((bw - bG) / this.lI.offsetWidth) > Math.abs((bv - bE) / this.lI.offsetHeight) ? Math.abs((bw - bG) / this.lI.offsetWidth) : Math.abs((bv - bE) / this.lI.offsetHeight);
        var by = 0;
        for (by = 0; by < this.IlI - 1; by++) {
            if (Math.pow(2, by) > bC) {
                break
            }
        }
        var bJ = this.IlI - 1 - by;
        if (bJ > 17) {
            bJ = 17
        }
        if (bJ > this.maxLevel) {
            bJ = this.maxLevel
        }
        if (bJ == this.level) {
            this.panTo(bL)
        } else {
            this.setCenter(bL, bJ)
        }
    }

    function aU(bH) {
        if (this.zoomstartHandler != undefined) {
            this.zoomstartHandler()
        }
        if (bH == null || bH == undefined) {
            return
        }
        if (bH.length == 0) {
            return
        }
        var bF = 0;
        var bD = 0;
        var bA = 0;
        var bx = 0;
        for (var bO = 0; bO < bH.length; bO++) {
            var bB = bH[bO].latlngs;
            var bJ = new Array();
            if (bB.length == undefined) {
                if (bH[bO].vType() == "VCircle") {
                    var bP = bH[bO].getBounds();
                    bJ.push(bP.getNorthWest());
                    bJ.push(bP.getSouthEast())
                } else {
                    bJ[0] = bB
                }
            } else {
                if ((bH[bO].vType() == "VPolyline" || bH[bO].vType() == "VPolygon" || bH[bO].vType() == "VArrow") && bH[bO].isDirectionObj) {
                    for (var bN = 0; bN < bB.length; bN++) {
                        for (var bL = 0; bL < bB[bN].length; bL++) {
                            bJ.push(bB[bN][bL])
                        }
                    }
                } else {
                    bJ = bB
                }
            }
            for (var bN = 0; bN < bJ.length; bN++) {
                if (bF == 0 && bD == 0 && bA == 0 && bx == 0) {
                    bF = parseFloat(bJ[bN].longitude.toString());
                    bA = parseFloat(bJ[bN].longitude.toString());
                    bD = parseFloat(bJ[bN].latitude.toString());
                    bx = parseFloat(bJ[bN].latitude.toString())
                }
                if (bF < parseFloat(bJ[bN].longitude.toString())) {
                    bF = parseFloat(bJ[bN].longitude.toString())
                }
                if (bA > parseFloat(bJ[bN].longitude.toString())) {
                    bA = parseFloat(bJ[bN].longitude.toString())
                }
                if (bD < parseFloat(bJ[bN].latitude.toString())) {
                    bD = parseFloat(bJ[bN].latitude.toString())
                }
                if (bx > parseFloat(bJ[bN].latitude.toString())) {
                    bx = parseFloat(bJ[bN].latitude.toString())
                }
            }
        }
        var bM = new VLatLng(parseFloat((bD + bx) / 2), parseFloat((bF + bA) / 2));
        var bz = new Array();
        bz[0] = bF;
        bz[1] = bD;
        var bI = VProjection.fromLatLngToPixel(new VLatLng(bz[1], bz[0]), this.IlI - 1);
        var bw = bI.x;
        var bv = bI.y;
        bz = new Array();
        bz[0] = bA;
        bz[1] = bx;
        bI = VProjection.fromLatLngToPixel(new VLatLng(bz[1], bz[0]), this.IlI - 1);
        var bG = bI.x;
        var bE = bI.y;
        var bC = Math.abs((bw - bG) / this.lI.offsetWidth) > Math.abs((bv - bE) / this.lI.offsetHeight) ? Math.abs((bw - bG) / this.lI.offsetWidth) : Math.abs((bv - bE) / this.lI.offsetHeight);
        var by = 0;
        for (by = 0; by < this.IlI - 1; by++) {
            if (Math.pow(2, by) > bC) {
                break
            }
        }
        var bK = this.IlI - 1 - by;
        if (bK > 17) {
            bK = 17
        }
        if (bK > this.maxLevel) {
            bK = this.maxLevel
        }
        if (bK == this.level) {
            this.panTo(bM)
        } else {
            this.setCenter(bM, bK)
        }
    }

    function aE(bx, bv) {
        if (aP.offsetWidth > 2000 || aP.offsetHeight > 2000) {
            jlj = false;
            isValidKey = false;
            for (var bA = 0; bA < this.l1l; bA++) {
                for (var by = 0; by < this.iI; by++) {
                    bd.childNodes[0].childNodes[bA * this.iI + by].src = strResURL + "images/invalidkey.jpg"
                }
            }
            jii.disableAll();
            return
        }
        if (bx == undefined) {
            bx = 0
        }
        if (bv == undefined) {
            bv = 0
        }
        this.calculatePosition();
        var bE, bD;
        if (bx == 1) {
            bE = (aP.offsetWidth - this.options.size.width) / 2
        } else {
            bE = (this.options.size.width - aP.offsetWidth) / 2
        } if (bv == 1) {
            bD = (aP.offsetHeight - this.options.size.height) / 2
        } else {
            bD = (this.options.size.height - aP.offsetHeight) / 2
        }
        var bC, bB;
        var bw = VProjection.fromLatLngToPixel(this.getCenter(), this.level);
        bC = bw.x + bE;
        bB = bw.y + bD;
        var bz = VProjection.fromPixelToLatLng(new VPoint(bC, bB), this.level);
        this.centerX = bz.longitude;
        this.centerY = bz.latitude;
        this.options.size = new VSize(aP.offsetWidth, aP.offsetHeight);
        this.initMap(false);
        this.setCenter(this.getCenter(), this.level, this.curMapType, true);
        for (var bA = 0; bA < this.nControls; bA++) {
            this.arrControls[bA].redraw()
        }
    }

    function J(bv) {
        bv.initialize(this);
        bv.arrIndex = this.Ill;
        this.lli[this.Ill] = bv;
        this.Ill++;
        if (this.l1lj != undefined) {
            this.l1lj(bv)
        }
        if (document.getElementById("MapBlowup") != undefined && this.IiI != null) {
            var i = bv.copy();
            if (i.vType() == "VMarker") {
                i.Il.clickable = false;
                i.Il.draggable = false;
                i.Il.bouncy = false
            }
            this.IiI.addOverlay(i)
        }
    }

    function bg(bv) {
        for (var bx = bv.arrIndex; bx < this.Ill - 1; bx++) {
            this.lli[bx] = this.lli[bx + 1];
            this.lli[bx].arrIndex = this.lli[bx].arrIndex - 1
        }
        this.Ill--;
        this.lli[this.Ill] = null;
        bv.remove();
        if (this.l11l != undefined) {
            this.l11l(bv)
        }
        if (document.getElementById("MapBlowup") != undefined && this.IiI != null) {
            var bw = document.getElementById("MapBlowup" + bv.id.substring(bv.id.indexOf("_")));
            this.IiI.removeOverlay(bw.II)
        }
    }

    function d() {
        this.jI1 = 0;
        for (var bv = 0; bv < this.Ill; bv++) {
            this.lli[bv].remove()
        }
        this.Ill = 0;
        if (this.lliI != undefined) {
            this.lliI()
        }
        if (document.getElementById("MapBlowup") != undefined && this.IiI != null) {
            this.IiI.clearOverlays()
        }
    }

    function bc() {
        return aG
    }

    function f(bv, bz, by, bB) {
        if (!this.bEnableInfoWindow) {
            this.closeInfoWindow();
            return
        }
        if (bv == undefined) {
            return
        }
        if (bz == undefined) {
            bz = ""
        }
        if (by == undefined) {
            by = new VInfoWindowOptions()
        }
        var bw = new Array();
        bw.push(new VInfoWindowTab("", bz));
        var bx = this.getInfoWindow();
        if (bB == undefined) {
            bx.openMk = null
        } else {
            bx.openMk = bB
        }
        var i = bx.getInfoWindowSize(bz);
        bx.reset(bv, bw, i, by.offset, 0);
        bx.options = (by != undefined) ? by : new VInfoWindowOptions();
        if (by.maximizeWhenOpen) {
            bx.sizeState = "maximize"
        }
        bx.show();
        if (by.autoPan) {
            var bA = bx.getInfoWindowPan();
            this.panBy(bA)
        }
    }

    function a3(bB, bA, bv) {
        if (!this.bEnableInfoWindow) {
            this.closeInfoWindow();
            return
        }
        if (bB == undefined) {
            return
        }
        if (bA == undefined) {
            bA = new VInfoWindowTab()
        }
        if (bv == undefined) {
            bv = new VInfoWindowOptions()
        }
        var bD = this.getInfoWindow();
        var bx = 0,
            bz = 0;
        for (var bw = 0; bw < bA.length; bw++) {
            if (bA[bw].content.length > bx) {
                bx = bA[bw].content.length;
                bz = bw
            }
        }
        var bC = bD.getInfoWindowSize(bA[bz].content);
        bD.reset(bB, bA, bC, bv.offset, bv.selectedTab);
        bD.options = bv;
        bD.show();
        if (bv.autoPan) {
            var by = bD.getInfoWindowPan();
            this.panBy(by)
        }
    }

    function v(bE, bw, bD) {
        if (!this.bEnableInfoWindow) {
            this.closeInfoWindow();
            return
        }
        if (bE == undefined) {
            return
        }
        if (bw == undefined) {
            bw = new VInfoWindowOptions()
        }
        bw.mapBlowup = true;
        if (bD == undefined || bD == "") {
            bD = "&nbsp;&nbsp;&nbsp;"
        }
        var bC = new Array();
        var bA = '<br/><div id=MapBlowup style="width:180px; height:150px; position:relative; "></div>';
        bC.push(new VInfoWindowTab(bD, bA));
        var bG = this.getInfoWindow();
        var bF = bG.getInfoWindowSize(bA);
        bG.reset(bE, bC, bF, bw.offset, 0);
        bG.options = bw;
        bG.show();
        var by = document.getElementById("MapBlowup");
        var bx = new VMap(by);
        bx.addControl(new VSmallZoomControl());
        for (var bz = 0; bz < this.Ill; bz++) {
            var bv = this.lli[bz].copy();
            if (bv.vType() == "VMarker") {
                bv.Il.clickable = false;
                bv.Il.draggable = false;
                bv.Il.bouncy = false
            }
            bx.addOverlay(bv)
        }
        bx.setCenter(bE, bw.llll);
        bx.disableScrollWheelZoom();
        bG.redraw();
        if (bw.autoPan) {
            var bB = bG.getInfoWindowPan();
            this.panBy(bB)
        }
        this.IiI = bx
    }

    function al(by, bA) {
        if (!this.bEnableInfoWindow) {
            this.closeInfoWindow();
            return
        }
        var bz = this.getInfoWindow();
        var bx = 0,
            bC = 0;
        for (var bw = 0; bw < by.length; bw++) {
            if (by[bw].content.length > bx) {
                bx = by[bw].content.length;
                bC = bw
            }
        }
        var bv;
        if (by[bC].label != "") {
            bv = bz.getInfoWindowSize(by[bC].content)
        } else {
            bv = bz.getInfoWindowSize(by[bC].content)
        }
        bz.reset(null, by, bv, null, null);
        bz.show();
        if (bz.options.autoPan) {
            var bB = bz.getInfoWindowPan();
            this.panBy(bB)
        }
        if (bA != undefined) {
            bA()
        }
    }

    function au() {
        var i = this.getInfoWindow();
        i.close()
    }

    function U() {
        return this.i1I
    }

    function a8(bw) {
        var bz = this.arrMaxRowCol[this.level];
        var by, bA = 0,
            bv = 0;
        var bC = new Array(),
            bx, bD, bB;
        for (by = 0; by < bw.length; by++) {
            bx = VProjection.fromLatLngToPixel(bw[by], this.getLevel());
            if (this.curMapType.opts.degree360) {
                bD = bx.x - 256 * (((this.llI % bz) + bz) % bz)
            } else {
                bD = bx.x - 256 * this.llI
            }
            bD += parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left);
            if (this.curMapType.opts.degree360) {
                if (bD < 0 && bD + 256 * bz < aP.offsetWidth) {
                    bA++
                } else {
                    bv++
                }
            }
            bD -= parseInt(C.style.left);
            bB = bx.y - 256 * this.jl + parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top) - parseInt(C.style.top);
            bC[by] = new VPoint(bD, bB)
        }
        if (bA > bv) {
            for (by = 0; by < bw.length; by++) {
                bC[by].x += 256 * bz
            }
        }
        return bC
    }

    function o(bz) {
        var bx = this.arrMaxRowCol[this.level];
        var bw = VProjection.fromLatLngToPixel(bz, this.getLevel());
        var i = bw.x - 256 * (((this.llI % bx) + bx) % bx) + parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left) - parseInt(C.style.left);
        var by = bw.y - 256 * this.jl + parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top) - parseInt(C.style.top);
        var bv = new VPoint(i, by);
        return bv
    }

    function ai(bz) {
        var bx = this.arrMaxRowCol[this.level];
        var bw = new VPoint(bz.longitude, bz.latitude);
        var i = bw.x - 256 * (((this.llI % bx) + bx) % bx) + parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left) - parseInt(C.style.left);
        var by = bw.y - 256 * this.jl + parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top) - parseInt(C.style.top);
        var bv = new VPoint(i, by);
        return bv
    }

    function S(bz) {
        var bx = this.arrMaxRowCol[this.level];
        var bw = VProjection.fromLatLngToPixel(bz, this.getLevel());
        var i = bw.x - 256 * (((this.llI % bx) + bx) % bx) + parseInt(bd.style.left) + parseInt(bd.childNodes[0].childNodes[0].style.left);
        var by = bw.y - 256 * this.jl + parseInt(bd.style.top) + parseInt(bd.childNodes[0].childNodes[0].style.top);
        var bv = new VPoint(i, by);
        return bv
    }

    function aX(bv) {
        var bw = this.arrMaxRowCol[this.level];
        var i = bv.x + 256 * (((this.llI % bw) + bw) % bw) - parseInt(bd.style.left) - parseInt(bd.childNodes[0].childNodes[0].style.left) + parseInt(C.style.left);
        var by = bv.y + 256 * this.jl - parseInt(bd.style.top) - parseInt(bd.childNodes[0].childNodes[0].style.top) + parseInt(C.style.top);
        var bx = VProjection.fromPixelToLatLng(new VPoint(i, by), this.level);
        return bx
    }

    function M(bv) {
        var bw = this.arrMaxRowCol[this.level];
        var i = bv.x + 256 * (((this.llI % bw) + bw) % bw) - parseInt(bd.style.left) - parseInt(bd.childNodes[0].childNodes[0].style.left);
        var by = bv.y + 256 * this.jl - parseInt(bd.style.top) - parseInt(bd.childNodes[0].childNodes[0].style.top);
        var bx = VProjection.fromPixelToLatLng(new VPoint(i, by), this.level);
        return bx
    }

    function a1(bv) {
        var i = bv.x - parseInt(C.style.left);
        var bx = bv.y - parseInt(C.style.top);
        var bw = new VPoint(i, bx);
        return bw
    }

    function w(bv) {
        var i = bv.x + parseInt(C.style.left);
        var bx = bv.y + parseInt(C.style.top);
        var bw = new VPoint(i, bx);
        return bw
    }
}

//P5s hàm khỏi tạo Marker được chỉnh lại để đáp ứng yêu cầu dự án
function VMarker(w, f, i,obj) {
    
    if (w == undefined) {
        return
    }
    if (f == undefined || f.llii != undefined) {
        this.latlngs = w;
        this.icon = (f == undefined) ? new VIcon() : f;
        this.inert = (i == undefined) ? true : i;
        this.Il = new VMarkerOptions();
        this.Il.icon = this.icon;
        if (this.inert == true) {
            this.Il.clickable = false;
            this.Il.draggable = false;
            this.Il.bouncy = false
        }
    } else {
        this.latlngs = w;
        this.Il = (f == undefined) ? new VMarkerOptions() : f;
        this.icon = this.Il.icon
    }
    
    this.objectCommunes = obj; //P5s đối tượng liên kết với Marker
    this.bOpenedInfoWindow = false;
    this.borderStyle = "";
    this.opacity = 0;
    this.mappingDiv = null;
    this.initialize = a;
    this.redraw = b;
    this.remove = p;
    this.copy = L;
    this.createMarker = c;
    this.createMappingMarker = K;
    this.removeMappingMarker = H;
    this.updateZIndex = d;
    this.openInfoWindow = n;
    this.closeInfoWindow = z;
    this.openInfoWindowTabs = m;
    this.bindInfoWindow = u;
    this.bindInfoWindowTabs = B;
    this.showMapBlowup = l;
    this.setCustomMarker = e;
    this.getIcon = A;  
    this.setIcon = C;
    this.getTitle = r;
    this.getPoint = t;
    this.getLatLng = v;
    this.setPoint = q;
    this.setLatLng = y;
    this.enableDragging = J;
    this.disableDragging = o;
    this.draggable = I;
    this.setImage = M;
    this.hide = F;
    this.show = x;
    this.hideText = D;
    this.showText = k;
    this.isHidden = g;
    this.active = G;
    this.unactive = h;
    this.isActive = E;
    this.vType = s;
    if (typeof this.latlngs == "string") {
        var j = this.latlngs.split(",");
        this.latlngs = new VLatLng(j[1], j[0])
    }
    if (this.latlngs.latitude > 180 || this.latlngs.longitude > 180) {
        this.latlngs = VProjection.fromPixelToLatLng(new VPoint(this.latlngs.longitude, this.latlngs.latitude), 12)
    }

    function a(N) {
        if (this.latlngs == null) {
            return
        }
        if (this.latlngs.latitude == undefined) {
            this.latlngs = VProjection.fromPixelToLatLng(this.latlngs, N.level)
        }
        this.createMarker(N, null)
    }

    function b() {
        var N = document.getElementById(this.id);
        if (N == undefined) {
            return
        }
        var O = this.ll.fromLLsToDPxs([this.latlngs])[0];
        N.style.left = O.x - this.Il.offset.width + "px";
        N.style.top = O.y - this.Il.offset.height + "px";
        this.createMappingMarker()
    }

    function p() {
        var N = document.getElementById(this.id);
        if (this.bOpenedInfoWindow) {
            this.closeInfoWindow()
        }
        if (N.il1 != null) {
            N.il1()
        }
        this.lj.parentNode.removeChild(this.lj);
        this.removeMappingMarker()
    }

    function L() {
        return new VMarker(this.latlngs, this.Il)
    }

    function c(N, O) {
        var Z = document.getElementById(N.id + "_path");
        var R;
        if (O == null) {
            R = document.getElementById(N.id + "_Marker_" + N.jI1)
        } else {
            R = document.getElementById(O)
        } if (R == null && O == null) {
            R = document.createElement("div");
            R.id = N.id + "_Marker_" + N.jI1;
            var Q = (this.Il.zIndexProcess != null) ? this.Il.zIndexProcess() : 1;
            R.style.zIndex = Q;
            R.style.MozUserSelect = "none";
            if (this.Il.draggable) {
                R.className = "dragmarker"
            } else {
                R.className = "clickmarker"
            }
            R.II = this;
            this.id = N.id + "_Marker_" + N.jI1;
            N.jI1++;
            Z.childNodes[0].appendChild(R);
            if (this.borderStyle != "") {
                R.style.border = this.borderStyle
            }
            if (this.opacity > 0) {
                R.style.filter = "alpha(opacity = " + this.opacity + ")";
                R.style.opacity = this.opacity / 100
            }
        }
        var S, V = (this.Il.clickable) ? "cursor:pointer;" : "";
        if (R.childNodes[0] == null || (this.Il.customMarker != undefined && this.Il.customMarker != "")) {
            var T = "";
            if (this.Il.customMarker == undefined || this.Il.customMarker == "") {
                if (this.icon.iconSize == null) {
                    T += "<img id='" + this.id + "_Img' style='" + V + "' src=" + this.icon.image + " title='" + this.Il.title + "' />"
                } else {
                    T += "<img id='" + this.id + "_Img' style='" + V + "' src=" + this.icon.image + " width=" + this.icon.iconSize.width + "px height=" + this.icon.iconSize.height + "px title='" + this.Il.title + "' />"
                }
            } else {
                T += this.Il.customMarker
            } if (this.Il.nText > 0) {
                for (S = 0; S < this.Il.nText; S++) {
                    T += "<table><tr><td id='" + this.id + "_Text" + S + "' nowrap style='position:absolute; " + V + " font-size:" + this.Il.textStyle[S].fontSize + "px; font-weight:" + this.Il.textStyle[S].fontWeight + "; color:" + this.Il.textStyle[S].color + "; font-family:" + this.Il.textStyle[S].fontFamily + ";'>" + this.Il.jll[S] + "</td></tr></table>"
                }
            }
            R.innerHTML = T;
            if (this.Il.customMarker != undefined || this.Il.customMarker != "") {
                var X = R.childNodes[0];
                X.id = this.id + "_Img";
                if (this.Il.clickable) {
                    X.style.cursor = "pointer"
                }
                X.title = this.Il.title
            }
        } else {
            var X = R.childNodes[0];
            if (this.icon.iconSize == null) {
                X.src = this.icon.image
            } else {
                X.src = this.icon.image;
                X.width = this.icon.iconSize.width;
                X.height = this.icon.iconSize.height
            }
        }
        R.style.position = "absolute";
        R.draggable = (this.Il.draggable == true) ? 1 : 0;
        R.autoPan = this.Il.autoPan;
        var P = N.fromLatLngToDivPixel(this.latlngs);
        if (this.icon.iconSize == null) {
            this.icon.iconSize = new VSize(R.offsetWidth, R.offsetHeight)
        }
        if (this.Il.offset == null) {
            this.Il.offset = new VSize(this.icon.iconSize.width / 2, this.icon.iconSize.height)
        }
        R.style.left = P.x - this.Il.offset.width + "px";
        R.style.top = P.y - this.Il.offset.height + "px";
        R.style.width = this.icon.iconSize.width + "px";
        R.style.height = this.icon.iconSize.height + "px";
        if (this.Il.nText > 0) {
            var X = document.getElementById(this.id + "_Img");
            var U, Y, W;
            for (S = 0; S < this.Il.nText; S++) {
                U = document.getElementById(this.id + "_Text" + S);
                if (this.Il.textPosition[S].anchor == V_ANCHOR_TOP_RIGHT) {
                    U.style.left = X.offsetWidth + this.Il.textPosition[S].offset.width + "px";
                    U.style.top = -U.offsetHeight - this.Il.textPosition[S].offset.height + "px"
                } else {
                    if (this.Il.textPosition[S].anchor == V_ANCHOR_CENTER_RIGHT) {
                        U.style.left = X.offsetWidth + this.Il.textPosition[S].offset.width + "px";
                        if (U.offsetHeight > X.offsetHeight) {
                            W = (U.offsetHeight - X.offsetHeight) / 2;
                            U.style.top = this.Il.textPosition[S].offset.height - W + "px"
                        } else {
                            if (U.offsetHeight < X.offsetHeight) {
                                W = (X.offsetHeight - U.offsetHeight) / 2;
                                U.style.top = this.Il.textPosition[S].offset.height + W + "px"
                            } else {
                                U.style.top = this.Il.textPosition[S].offset.height + "px"
                            }
                        }
                    } else {
                        if (this.Il.textPosition[S].anchor == V_ANCHOR_BOTTOM_RIGHT) {
                            U.style.left = X.offsetWidth + this.Il.textPosition[S].offset.width + "px";
                            U.style.top = X.offsetHeight + this.Il.textPosition[S].offset.height + "px"
                        } else {
                            if (this.Il.textPosition[S].anchor == V_ANCHOR_BOTTOM_CENTER) {
                                if (U.offsetWidth > X.offsetWidth) {
                                    Y = (U.offsetWidth - X.offsetWidth) / 2;
                                    U.style.left = this.Il.textPosition[S].offset.width - Y + "px"
                                } else {
                                    if (U.offsetWidth < X.offsetWidth) {
                                        Y = (X.offsetWidth - U.offsetWidth) / 2;
                                        U.style.left = this.Il.textPosition[S].offset.width + Y + "px"
                                    } else {
                                        U.style.left = this.Il.textPosition[S].offset.width + "px"
                                    }
                                }
                                U.style.top = X.offsetHeight + this.Il.textPosition[S].offset.height + "px"
                            } else {
                                if (this.Il.textPosition[S].anchor == V_ANCHOR_CENTER_CENTER) {
                                    if (U.offsetWidth > X.offsetWidth) {
                                        Y = (U.offsetWidth - X.offsetWidth) / 2;
                                        U.style.left = this.Il.textPosition[S].offset.width - Y + "px"
                                    } else {
                                        if (U.offsetWidth < X.offsetWidth) {
                                            Y = (X.offsetWidth - U.offsetWidth) / 2;
                                            U.style.left = this.Il.textPosition[S].offset.width + Y + "px"
                                        } else {
                                            U.style.left = this.Il.textPosition[S].offset.width + "px"
                                        }
                                    } if (U.offsetHeight > X.offsetHeight) {
                                        W = (U.offsetHeight - X.offsetHeight) / 2;
                                        U.style.top = this.Il.textPosition[S].offset.height - W + "px"
                                    } else {
                                        if (U.offsetHeight < X.offsetHeight) {
                                            W = (X.offsetHeight - U.offsetHeight) / 2;
                                            U.style.top = this.Il.textPosition[S].offset.height + W + "px"
                                        } else {
                                            U.style.top = this.Il.textPosition[S].offset.height + "px"
                                        }
                                    }
                                } else {
                                    if (this.Il.textPosition[S].anchor == V_ANCHOR_TOP_CENTER) {
                                        if (U.offsetWidth > X.offsetWidth) {
                                            Y = (U.offsetWidth - X.offsetWidth) / 2;
                                            U.style.left = this.Il.textPosition[S].offset.width - Y + "px"
                                        } else {
                                            if (U.offsetWidth < X.offsetWidth) {
                                                Y = (X.offsetWidth - U.offsetWidth) / 2;
                                                U.style.left = this.Il.textPosition[S].offset.width + Y + "px"
                                            } else {
                                                U.style.left = this.Il.textPosition[S].offset.width + "px"
                                            }
                                        }
                                        U.style.top = -U.offsetHeight - this.Il.textPosition[S].offset.height + "px"
                                    } else {
                                        if (this.Il.textPosition[S].anchor == V_ANCHOR_TOP_LEFT) {
                                            U.style.left = -U.offsetWidth - this.Il.textPosition[S].offset.width + "px";
                                            U.style.top = -U.offsetHeight - this.Il.textPosition[S].offset.height + "px"
                                        } else {
                                            if (this.Il.textPosition[S].anchor == V_ANCHOR_CENTER_LEFT) {
                                                U.style.left = -U.offsetWidth - this.Il.textPosition[S].offset.width + "px";
                                                if (U.offsetHeight > X.offsetHeight) {
                                                    W = (U.offsetHeight - X.offsetHeight) / 2;
                                                    U.style.top = this.Il.textPosition[S].offset.height - W + "px"
                                                } else {
                                                    if (U.offsetHeight < X.offsetHeight) {
                                                        W = (X.offsetHeight - U.offsetHeight) / 2;
                                                        U.style.top = this.Il.textPosition[S].offset.height + W + "px"
                                                    } else {
                                                        U.style.top = this.Il.textPosition[S].offset.height + "px"
                                                    }
                                                }
                                            } else {
                                                if (this.Il.textPosition[S].anchor == V_ANCHOR_BOTTOM_LEFT) {
                                                    U.style.left = -U.offsetWidth - this.Il.textPosition[S].offset.width + "px";
                                                    U.style.top = X.offsetHeight + this.Il.textPosition[S].offset.height + "px"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
//        //P5s set offet image hàm này bị lỗi khi hiển thị icon ở trang Tracking
//        if( this.icon.image.indexOf("_") >= 0 &&  typeof MAX_HEIGHT_OFFSET != 'undefined'  )        
//                 R.style.top = P.y - this.Il.offset.height - (MAX_HEIGHT_OFFSET-MIN_HEIGHT_OFFSET) + "px";
        
        this.ll = N;
        this.lj = R;
        if (O == null) {
            this.createMappingMarker()
        }
    }

    function K() {
        if (this.ll.i1I.isOpened()) {
            if (VUtil.isInsidePolygon(this.latlngs, this.ll.i1I.mappingLLs)) {
                var R = document.getElementById(this.ll.id + "_path");
                if (this.mappingDiv == null) {
                    this.mappingDiv = document.createElement("div");
                    this.mappingDiv.style.zIndex = ObjBelowShadeZIndex;
                    this.mappingDiv.style.MozUserSelect = "none";
                    this.mappingDiv.className = "dragmarker";
                    this.mappingDiv.II = this;
                    R.childNodes[3].appendChild(this.mappingDiv);
                    if (this.sHandler != null) {
                        var O, Q = this;
                        for (O = 0; O < this.sHandler.length; O++) {
                            VEvent.addDOMListener(this.mappingDiv, this.svEvent[O], function (W) {
                                var U = new VPoint(lI1 - Q.ll.Left, lIl - Q.map_Top);
                                var S = Q.ll.fromContainerPixelToLatLng(U);
                                var T = (browserName == "msie") ? event.type : W.type;
                                var V = 0;
                                while (V < Q.svEvent.length && Q.svEvent[V] != T) {
                                    V++
                                }
                                Q.sHandler[V](Q, S)
                            })
                        }
                    }
                }
                var N = "<img style='cursor:pointer' src='" + strResURL + "images/transparent.png' width=" + this.icon.iconSize.width + "px height=" + this.icon.iconSize.height + "px title='" + this.Il.title + "' />";
                this.mappingDiv.innerHTML = N;
                this.mappingDiv.style.position = "absolute";
                var P = this.ll.fromLatLngToDivPixel(this.latlngs);
                this.mappingDiv.style.left = P.x - this.Il.offset.width + "px";
                this.mappingDiv.style.top = P.y - this.Il.offset.height + "px";
                this.mappingDiv.style.width = this.icon.iconSize.width + "px";
                this.mappingDiv.style.height = this.icon.iconSize.height + "px"
            }
        }
    }

    function H() {
        var N = document.getElementById(this.ll.id + "_path");
        if (this.mappingDiv != null) {
            N.childNodes[3].removeChild(this.mappingDiv)
        }
        this.mappingDiv = null
    }

    function d() {
        var N = (this.Il.zIndexProcess != null) ? this.Il.zIndexProcess() : 1;
        this.lj.style.zIndex = N
    }

    function n(O, N) {
        
        this.bOpenedInfoWindow = true;
        var P = document.getElementById(this.id);
        if (P.iIj != null) {
            P.iIj()
        }
      
        if (this.ll.li1 != undefined) {
            this.ll.li1()
        }
        if (N == undefined) {
            N = new VInfoWindowOptions();
            N.offset = new VSize(0, -(this.icon.iconSize.height * 5 / 6))
        }
        this.updateZIndex();
        this.ll.openInfoWindow(this.latlngs, O, N, this)
    }

    function z() {
        this.bOpenedInfoWindow = false;
        var N = document.getElementById(this.id);
        if (N.iI1 != null) {
            N.iI1()
        }
        this.ll.closeInfoWindow();
        if (N.iIl != null) {
            N.iIl()
        }
    }

    function m(N, O) {
        this.bOpenedInfoWindow = true;
        var P = document.getElementById(this.id);
        if (P.iIj != null) {
            P.iIj()
        }
        if (O == undefined) {
            O = new VInfoWindowOptions();
            O.offset = new VSize(0, -(this.icon.iconSize.height * 5 / 6))
        }
        this.ll.openInfoWindowTabs(this.latlngs, N, O)
    }

    function u(P, O) {
        if (this.Il.clickable == false) {
            return
        }
        if (P == "") {
            this.closeInfoWindow();
            var N = document.getElementById(this.id);
            N.onclick = null
        } else {
            var N = document.getElementById(this.id);
            if (O == undefined) {
                O = new VInfoWindowOptions();
                O.offset = new VSize(0, -(this.icon.iconSize.height * 5 / 6))
            }
            var Q = this;
            N.onclick = function () {
                this.bOpenedInfoWindow = true;
                if (this.Iij == true) {
                    this.Iij = false;
                    return
                }
                Q.openInfoWindow(P, O)
            }
        }
    }

    function B(O, P) {
        if (this.Il.clickable == false) {
            return
        }
        if (O == undefined || O == null) {
            this.closeInfoWindow();
            var N = document.getElementById(this.id);
            N.onclick = null
        } else {
            var N = document.getElementById(this.id);
            if (P == undefined) {
                P = new VInfoWindowOptions();
                P.offset = new VSize(0, -(this.icon.iconSize.height * 5 / 6))
            }
            var Q = this;
            N.onclick = function () {
                this.bOpenedInfoWindow = true;
                if (this.Iij == true) {
                    this.Iij = false;
                    return
                }
                Q.openInfoWindowTabs(O, P)
            }
        }
    }

    function l(N) {
        this.bOpenedInfoWindow = true;
        var O = document.getElementById(this.id);
        if (O.iIj != null) {
            O.iIj()
        }
        if (this.ll.li1 != undefined) {
            this.ll.li1()
        }
        if (N == undefined) {
            N = new VInfoWindowOptions();
            N.offset = new VSize(0, -(this.icon.iconSize.height * 5 / 6))
        }
        this.ll.showMapBlowup(this.latlngs, N)
    }

    function e(N) {
        this.Il.customMarker = N;
        this.createMarker(this.ll, this.id)
    }

    function A() {
        return this.icon
    }

    function C(N) {
        this.icon = (N == undefined) ? new VIcon() : N;
        this.Il.icon = this.icon;
        this.createMarker(this.ll, this.id)
    }

    function r() {
        return this.Il.title
    }

    function t() {
        return this.latlngs
    }

    function v() {
        return this.latlngs
    }

    function q(N) {
        this.latlngs = N.copy();
        this.redraw()
    }

    function y(N) {
        this.latlngs = N.copy();
        this.redraw()
    }

    function J() {
        this.Il.draggable = true;
        if (this.lj != null && this.lj != undefined) {
            this.lj.className = "dragmarker"
        }
        if (document.getElementById(this.id) != undefined) {
            document.getElementById(this.id).draggable = 1
        }
    }

    function o() {
        this.Il.draggable = false;
        if (this.lj != null && this.lj != undefined) {
            this.lj.className = "clickmarker"
        }
        if (document.getElementById(this.id) != undefined) {
            document.getElementById(this.id).draggable = 0
        }
    }

    function I() {
        return this.Il.draggable
    }

    function M(N) { }

    function F() {
        var N = document.getElementById(this.id);
        if (N.I1l != null) {
            N.I1l(false)
        }
        this.lj.style.display = "none"
    }

    function x() {
        var N = document.getElementById(this.id);
        if (N.I1l != null) {
            N.I1l(true)
        }
        this.lj.style.display = ""
    }

    function D(O) {
        var N = document.getElementById(this.id + "_Text" + O);
        if (N != null) {
            N.style.display = "none"
        }
    }

    function k(O) {
        var N = document.getElementById(this.id + "_Text" + O);
        if (N != null) {
            N.style.display = ""
        }
    }

    function g() {
        if (this.lj.style.visibility == "hidden") {
            return true
        }
        return false
    }

    function G() {
        this.lj.style.zIndex = parseInt(this.lj.style.zIndex) + 1
    }

    function h() {
        this.lj.style.zIndex = parseInt(this.lj.style.zIndex) - 1
    }

    function E() {
        return (this.lj.style.zIndex == MarkerActiveZIndex)
    }

    function s() {
        return "VMarker"
    }
}
VMarker.prototype = new VOverlay();

function VOverviewMapControl(e) {
    this.objOpts = (e != undefined) ? e : {
        state: "open"
    };
    this.initialize = a;
    this.redraw = l;
    this.remove = b;
    this.getDefaultPosition = f;
    this.onOffOverviewMap = k;
    this.onMap = i;
    this.offMap = g;
    this.updateLanguage = d;
    this.hide = c;
    this.show = j;
    this.vType = h;
    this.buttonHeight = 27;
    this.buttonWidth = 27;

    function a(s, n) {
        if (isValidKey == false) {
            return
        }
        if (n == undefined) {
            n = new VControlPosition(V_ANCHOR_BOTTOM_RIGHT)
        }
        var p = new Array();
        p[p.length] = "<div id='" + s.id + "_VBtnOnOffOverviewMap' class='VBtnOnOffOverviewMap' title='" + ((s.strLanguage == "English") ? "Turn on/off the overview map" : "B&#7853;t/T&#7855;t b&#7843;n &#273;&#7891; nh&#7887;") + "'>";
        p[p.length] = "<div id='" + s.id + "_VImgOnOff' class='clickcontrol'>";
        p[p.length] = "<a class='btns'><span></span></a>";
        p[p.length] = "</div>";
        p[p.length] = "</div>";
        p[p.length] = "<div id='" + s.id + "_VOverviewBound' class='VOverviewBound'>";
        p[p.length] = "<div id='" + s.id + "_VOverviewBox' class='overviewmap' style='left: -256px; width: 568px; position: absolute; top: -256px; height: 568px; background-color: white; overflow:hidden;'>";
        p[p.length] = "<table id='" + s.id + "_VOverviewTable' height='512' cellspacing='0' cellpadding='0' width='512' border='0'>";
        p[p.length] = "<thead><tr><th></th><th></th></tr></thead>";
        p[p.length] = "<tbody>";
        p[p.length] = "<tr>";
        p[p.length] = "<td><img src='" + strResURL + "images/whiteimage.png' id='s_0_0' height='256' ismap width='256'/></td>";
        p[p.length] = "<td><img src='" + strResURL + "images/whiteimage.png' id='s_0_1' height='256' ismap width='256'/></td>";
        p[p.length] = "</tr>";
        p[p.length] = "<tr>";
        p[p.length] = "<td><img src='" + strResURL + "images/whiteimage.png' id='s_1_0' height='256' ismap width='256'/></td>";
        p[p.length] = "<td><img src='" + strResURL + "images/whiteimage.png' id='s_1_1' height='256' ismap width='256'/></td>";
        p[p.length] = "</tr>";
        p[p.length] = "</tbody>";
        p[p.length] = "</table>";
        p[p.length] = "</div>";
        p[p.length] = "<div id='" + s.id + "_VCenterRect' style='opacity: 0.5; border-right: #0066cc 2px double; border-top: #0066cc 2px double; filter: alpha(opacity = 50); left: 50px; background-image: url(\"" + strResURL + "images/sm_trans.gif\"); border-left: #0066cc 2px double; width: 50px; border-bottom: #0066cc 2px double; position: absolute; top: 50px; height: 30px; background-color: #d5f2ff; -moz-opacity: 0.5'></div>";
        p[p.length] = "<div class='dragoverview' id='" + s.id + "_VOverviewRect' style='opacity: 0.2; border-right: #0066cc 2px double; border-top: #0066cc 2px double; filter: alpha(opacity = 20); left: 50px; background-image: url(\"" + strResURL + "images/sm_trans.gif\"); border-left: #0066cc 2px double; width: 50px; border-bottom: #0066cc 2px double; position: absolute; top: 50px; height: 30px; background-color: transparent; -moz-opacity: 0.2'></div>";
        p[p.length] = "</div>";
        this.id = s.id + "_VOverviewMapControl";
        var r = VUtil.createDivElement(s.id + "_VOverviewMapControl", null, null, null, null, null, "hidden", p.join(""));
        r.position = n;
        r.style.MozUserSelect = "none";
        r.className = "VOverviewMapControl";
        s.lI.appendChild(r);
        var m = this;
        document.getElementById(s.id + "_VBtnOnOffOverviewMap").onclick = function () {
            m.onOffOverviewMap()
        };
        if (n.anchor == V_ANCHOR_TOP_RIGHT) {
            r.className += " MiniMapBorderTopRight"
        } else {
            if (n.anchor == V_ANCHOR_TOP_LEFT) {
                r.className += " MiniMapBorderTopLeft"
            } else {
                if (n.anchor == V_ANCHOR_BOTTOM_RIGHT) {
                    r.className += " MiniMapBorderBottomRight"
                } else {
                    if (n.anchor == V_ANCHOR_BOTTOM_LEFT) {
                        r.className += " MiniMapBorderBottomLeft"
                    }
                }
            }
        }
        this.OverviewMapFlag = 1;
        var q, o;
        for (q = 0; q < 2; q++) {
            for (o = 0; o < 2; o++) {
                document.getElementById("s_" + q + "_" + o).id = s.id + "s_" + q + "_" + o
            }
        }
        this.ll = s;
        this.lj = r;
        this.position = n;
        if (this.objOpts.state == "close") {
            this.onOffOverviewMap(false)
        }
    }

    function l() {
        if (isValidKey == false) {
            return
        }
    }

    function b() {
        VUtil.removeDivElement(this.id, this.ll.lI.id)
    }

    function f() {
        var m = new VControlPosition(V_ANCHOR_BOTTOM_RIGHT, new VSize(0, 0));
        return m
    }

    function k(m) {
        if (m == undefined) {
            m = true
        }
        if (this.OverviewMapFlag == 1) {
            this.offMap(this.id, this.ll.id, m)
        } else {
            this.onMap(this.id, this.ll.id, m)
        }
    }

    function g(o, n, m) {
        this.OverviewMapFlag = 0;
        document.getElementById(o).className = document.getElementById(o).className.replace("On", "");
        document.getElementById(n + "_VOverviewBox").style.display = "none"
    }

    function i(o, n, m) {
        this.OverviewMapFlag = 1;
        document.getElementById(o).className += " On";
        document.getElementById(n + "_VOverviewBox").style.display = ""
    }

    function d() {
        document.getElementById(this.ll.id + "_VBtnOnOffOverviewMap").title = (this.ll.strLanguage == "English") ? "Turn on/off the overview map" : "B&#7853;t/T&#7855;t b&#7843;n &#273;&#7891; nh&#7887;"
    }

    function c() {
        this.lj.style.display = "none"
    }

    function j() {
        this.lj.style.display = ""
    }

    function h() {
        return "VOverviewMapControl"
    }
}
VOverviewMapControl.prototype = new VControl();

function VPoint(a, e) {
    this.x = a;
    this.y = e;
    this.equals = c;
    this.toString = d;
    this.vType = b;

    function c(f) {
        if (this.x == f.x && this.y == f.y) {
            return true
        }
        return false
    }

    function d() {
        return (this.x.toString() + "," + this.y.toString())
    }

    function b() {
        return "VPoint"
    }
}

function VPolygon(p, K, s, O, w, A, v) {
    if (p == undefined) {
        return
    }
    this.latlngs = p;
    this.lineColor = (K == null) ? "red" : K;
    this.lineWidth =  1.2//(s == null) ? 3 : s;
    this.fillColor = (O == null) ? "green" : O;
    this.lineOpacity = (w == null) ? 0.5 : w;
    this.fillOpacity = (A == null) ? 0.3 : A;
    this.opts = (v == undefined) ? new VPolygonOptions() : v;
    this.arrPolygons = new Array();
    this.nPolygons = 0;
    this.lIi = false;
    this.isDirectionObj = false;
    this.initialize = e;
    this.drawLines = z;
    this.removeLines = D;
    this.addLineHandlers = x;
    this.removeLineHandlers = k;
    this.remove = u;
    this.copy = M;
    this.redraw = f;
    this.fromEncoded = H;
    this.calculateViewCoor = N;
    this.openInfoWindow = t;
    this.getCenterPoint = a;
    this.getVertexCount = q;
    this.getVertex = L;
    this.setVertex = b;
    this.insertPoint = h;
    this.deletePoint = r;
    this.getArea = d;
    this.getBounds = m;
    this.getLatLngs = B;
    this.setLatLngs = g;
    this.setStyle = E;
    this.hide = F;
    this.show = C;
    this.isHidden = n;
    this.supportsHide = c;
    this.vType = y;
    if (typeof this.latlngs == "object") {
        if (typeof this.latlngs[0] == "object" && this.latlngs[0].vType == undefined) {
            this.isDirectionObj = true;
            var J, I, G = new Array(),
                o;
            for (J = 0; J < this.latlngs.length; J++) {
                G[J] = new Array();
                for (I = 0; I < this.latlngs[J].length; I += 2) {
                    o = new VLatLng(this.latlngs[J][I + 1], this.latlngs[J][I]);
                    G[J].push(o)
                }
            }
            this.latlngs = G
        }
    } else {
        if (typeof this.latlngs == "string") {
            var l = this.latlngs.split(",");
            this.latlngs = new Array();
            var J, I = 0;
            for (J = 0; J < l.length; J += 2) {
                this.latlngs[I] = new VLatLng(parseFloat(l[J + 1]), parseFloat(l[J]));
                I++
            }
        }
    }
    for (var J = 0; J < this.latlngs.length; J++) {
        if (this.latlngs[J].latitude > 180 || this.latlngs[J].longitude > 180) {
            this.latlngs[J] = VProjection.fromPixelToLatLng(new VPoint(this.latlngs.longitude, this.latlngs.latitude), 12)
        }
    }

    function e(i) {
        this.ll = i;
        this.drawLines(i, true)
    }

    function z(R, T) {
        if (!this.lIi) {
            if (T) {
                this.calculateViewCoor()
            }
            var ae = document.getElementById(R.id + "_path");
            var af = R;
            var Q = new Array();
            if (this.isDirectionObj) {
                Q = this.l11
            } else {
                Q[0] = this.l11
            } if (Q.length == 0) {
                this.removeLines();
                return
            }
            var X;
            if (Q.length < this.nPolygons && Q.length > 0) {
                var ab = (browserName == "msie") ? ae.childNodes[1] : ae.childNodes[1].childNodes[0];
                for (X = Q.length; X < this.nPolygons; X++) {
                    ab.removeChild(this.arrPolygons[X]);
                    this.arrPolygons[X] = null
                }
                this.nPolygons = Q.length
            }
            for (X = 0; X < Q.length; X++) {
                if (browserName == "msie") {
                    if (this.arrPolygons[X] == null || this.arrPolygons[X] == undefined) {
                        var U = document.createElement("v:polyline");
                        this.arrPolygons[this.nPolygons] = U;
                        this.nPolygons++;
                        U.className = "dragline";
                        U.title = this.opts.title;
                        U.strokecolor = this.lineColor;
                        U.strokeweight = this.lineWidth;
                        U.filled = "t";
                        U.fillcolor = this.fillColor;
                        U.strokeopacity = this.lineOpacity;
                        U.fillopacity = this.fillOpacity;
                        var Y = document.createElement("v:stroke");
                        Y.fillopacity = this.lineOpacity;
                        Y.opacity = this.lineOpacity;
                        Y.endcap = "round";
                        U.appendChild(Y);
                        var S = document.createElement("v:fill");
                        S.fillopacity = this.fillOpacity;
                        S.opacity = this.fillOpacity;
                        U.appendChild(S);
                        ae.childNodes[1].appendChild(U);
                        U.style.cursor = "pointer";
                        U.llj = this.llj;
                        U.mouseoverHandler = this.mouseoverHandler;
                        U.mouseoutHandler = this.mouseoutHandler;
                        U.mousemoveHandler = this.mousemoveHandler;
                        U.II = this;
                        if (this.llj != undefined) {
                            U.onclick = function (aj) {
                                var ah = new VPoint(lI1 - af.Left, lIl - af.Top);
                                var i = af.fromContainerPixelToLatLng(ah);
                                var ak = this.II.latlngs;
                                var j = new Array();
                                if (this.II.isDirectionObj) {
                                    VUtil.ClosestPointPolylines(i.longitude, i.latitude, ak, j)
                                } else {
                                    VUtil.ClosestPointPolyline(i.longitude, i.latitude, ak, j)
                                }
                                var ag = j[0];
                                var ai = j[1];
                                this.llj(ag, ai, this.II)
                            }
                        }
                        if (this.mouseoverHandler != undefined) {
                            U.onmouseover = function () {
                                this.mouseoverHandler(this.II)
                            }
                        }
                        if (this.mouseoutHandler != undefined) {
                            U.onmouseout = function () {
                                this.mouseoutHandler(this.II)
                            }
                        }
                        if (this.mousemoveHandler != undefined) {
                            U.onmousemove = function (aj) {
                                var ah = new VPoint(lI1 - af.Left, lIl - af.Top);
                                var i = af.fromContainerPixelToLatLng(ah);
                                var ak = this.II.latlngs;
                                var j = new Array();
                                if (this.II.isDirectionObj) {
                                    VUtil.ClosestPointPolylines(i.longitude, i.latitude, ak, j)
                                } else {
                                    VUtil.ClosestPointPolyline(i.longitude, i.latitude, ak, j)
                                }
                                var ag = j[0];
                                var ai = j[1];
                                this.mousemoveHandler(ag, ai)
                            }
                        }
                    }
                    if (this.arrPolygons[X].points != undefined) {
                        this.arrPolygons[X].points.value = Q[X]
                    }
                } else {
                    var aa = ae.childNodes[1].childNodes[0];
                    var P = R.lI.offsetWidth * 3;
                    var ad = R.lI.offsetHeight * 3;
                    aa.setAttribute("width", P.toString() + "px");
                    aa.setAttribute("height", ad.toString() + "px");
                    var ac = R.fromContainerPixelToDivPixel(new VPoint(-R.lI.offsetWidth, -R.lI.offsetHeight));
                    aa.style.left = ac.x.toString() + "px";
                    aa.style.top = ac.y.toString() + "px";
                    aa.setAttribute("viewBox", ac.x.toString() + " " + ac.y.toString() + " " + P.toString() + " " + ad.toString());
                    var U;
                    if (this.arrPolygons[X] == null || this.arrPolygons[X] == undefined) {
                        U = document.createElementNS("http://www.w3.org/2000/svg", "path");
                        U.setAttribute("title", this.opts.title);
                        U.setAttribute("stroke", this.lineColor);
                        U.setAttribute("stroke-width", this.lineWidth + 1);
                        U.setAttribute("stroke-opacity", this.lineOpacity);
                        U.setAttribute("stroke-linecap", "round");
                        U.setAttribute("stroke-linejoin", "round");
                        U.setAttribute("fill-opacity", this.fillOpacity);
                        U.setAttribute("fill", this.fillColor);
                        aa.appendChild(U);
                        this.arrPolygons[this.nPolygons] = U;
                        this.nPolygons++;
                        U.style.cursor = "pointer";
                        U.llj = this.llj;
                        U.mouseoverHandler = this.mouseoverHandler;
                        U.mouseoutHandler = this.mouseoutHandler;
                        U.mousemoveHandler = this.mousemoveHandler;
                        U.II = this;
                        if (this.llj != undefined) {
                            U.onclick = function (aj) {
                                var ah = new VPoint(lI1 - af.Left, lIl - af.Top);
                                var i = af.fromContainerPixelToLatLng(ah);
                                var ak = this.II.latlngs;
                                var j = new Array();
                                if (this.II.isDirectionObj) {
                                    VUtil.ClosestPointPolylines(i.longitude, i.latitude, ak, j)
                                } else {
                                    VUtil.ClosestPointPolyline(i.longitude, i.latitude, ak, j)
                                }
                                var ag = j[0];
                                var ai = j[1];
                                this.llj(ag, ai, this.II)
                            }
                        }
                        if (this.mouseoverHandler != undefined) {
                            U.onmouseover = function () {
                                this.mouseoverHandler(this.II)
                            }
                        }
                        if (this.mouseoutHandler != undefined) {
                            U.onmouseout = function () {
                                this.mouseoutHandler(this.II)
                            }
                        }
                        if (this.mousemoveHandler != undefined) {
                            U.onmousemove = function (aj) {
                                var ah = new VPoint(lI1 - af.Left, lIl - af.Top);
                                var i = af.fromContainerPixelToLatLng(ah);
                                var ak = this.II.latlngs;
                                var j = new Array();
                                if (this.II.isDirectionObj) {
                                    VUtil.ClosestPointPolylines(i.longitude, i.latitude, ak, j)
                                } else {
                                    VUtil.ClosestPointPolyline(i.longitude, i.latitude, ak, j)
                                }
                                var ag = j[0];
                                var ai = j[1];
                                this.mousemoveHandler(ag, ai)
                            }
                        }
                    } else {
                        U = this.arrPolygons[X]
                    }
                    var V, W = new Array();
                    for (V = 0; V < Q[X].length; V += 2) {
                        W.push(Q[X][V]);
                        W.push(Q[X][V + 1])
                    }
                    var Z = new Array();
                    Z[Z.length] = "M";
                    Z[Z.length] = W[0];
                    Z[Z.length] = W[1];
                    for (V = 2; V < Q[X].length; V += 2) {
                        Z[Z.length] = "L";
                        Z[Z.length] = W[V];
                        Z[Z.length] = W[V + 1]
                    }
                    Z = VUtil.trim(Z.join(" "));
                    if (Z != "" && Z.indexOf("NaN") == -1) {
                        U.setAttribute("d", Z)
                    }
                }
            }
        }
    }

    function D() {
        var Q = document.getElementById(this.ll.id + "_path");
        var P = (browserName == "msie") ? Q.childNodes[1] : Q.childNodes[1].childNodes[0];
        if (this.nPolygons > 0) {
            var j;
            for (j = 0; j < this.nPolygons; j++) {
                P.removeChild(this.arrPolygons[j])
            }
            this.arrPolygons = new Array();
            this.nPolygons = 0
        }
    }

    function x() {
        var P, R = this.ll,
            j;
        for (P = 0; P < this.nPolygons; P++) {
            j = this.arrPolygons[P];
            j.llj = this.llj;
            j.mouseoverHandler = this.mouseoverHandler;
            j.mouseoutHandler = this.mouseoutHandler;
            j.mousemoveHandler = this.mousemoveHandler;
            if (this.llj != undefined) {
                var Q = function (W) {
                    var U = new VPoint(lI1 - R.Left, lIl - R.Top);
                    var i = R.fromContainerPixelToLatLng(U);
                    var X = this.II.latlngs;
                    var S = new Array();
                    if (this.II.isDirectionObj) {
                        VUtil.ClosestPointPolylines(i.longitude, i.latitude, X, S)
                    } else {
                        VUtil.ClosestPointPolyline(i.longitude, i.latitude, X, S)
                    }
                    var T = S[0];
                    var V = S[1];
                    this.llj(T, V, this.II)
                };
                if (browserName == "iPhone" || browserName == "iPod" || browserName == "iPad") {
                    j.ontouchend = Q
                } else {
                    j.onclick = Q
                }
            }
            if (this.mouseoverHandler != undefined) {
                j.onmouseover = function () {
                    this.mouseoverHandler(this.II)
                }
            }
            if (this.mouseoutHandler != undefined) {
                j.onmouseout = function () {
                    this.mouseoutHandler(this.II)
                }
            }
            if (this.mousemoveHandler != undefined) {
                j.onmousemove = function (W) {
                    var U = new VPoint(lI1 - R.Left, lIl - R.Top);
                    var i = R.fromContainerPixelToLatLng(U);
                    var X = this.II.latlngs;
                    var S = new Array();
                    if (this.II.isDirectionObj) {
                        VUtil.ClosestPointPolylines(i.longitude, i.latitude, X, S)
                    } else {
                        VUtil.ClosestPointPolyline(i.longitude, i.latitude, X, S)
                    }
                    var T = S[0];
                    var V = S[1];
                    this.mousemoveHandler(T, V)
                }
            }
        }
    }

    function k() {
        var P, j;
        for (P = 0; P < this.nPolygons; P++) {
            j = this.arrPolygons[P];
            j.llj = null;
            j.mouseoverHandler = null;
            j.mouseoutHandler = null;
            j.mousemoveHandler = null;
            j.onclick = null;
            j.onmouseover = null;
            j.onmouseout = null;
            j.onmousemove = null
        }
    }

    function u() {
        if (this.il1 != null) {
            this.il1()
        }
        this.removeLines();
        this.ll.resetSVG()
    }

    function M() {
        return new VPolygon(this.latlngs, this.lineColor, this.lineWidth, this.fillColor, this.lineOpacity, this.fillOpacity, this.opts)
    }

    function f(i) {
        this.drawLines(this.ll, !i)
    }

    function H(P, R, i, j, Q) { }

    function N() {
        if (this.isDirectionObj) {
            var Y = new VPoint(-this.ll.lI.offsetWidth * 2, -this.ll.lI.offsetHeight * 2);
            var W = new VPoint(this.ll.lI.offsetWidth * 3, this.ll.lI.offsetHeight * 3);
            var V = this.ll.fromContainerPixelToDivPixel(Y);
            var Q = this.ll.fromContainerPixelToDivPixel(W);
            var R, U, T, P = new Array(),
                S;
            for (U = 0; U < this.latlngs.length; U++) {
                P[U] = new Array();
                for (T = 0; T < this.latlngs[U].length; T++) {
                    S = this.ll.fromLatLngToDivPixel(this.latlngs[U][T]);
                    P[U].push(S.x);
                    P[U].push(S.y)
                }
                R = P[U].length;
                if (P[U][0] < P[U][R - 2]) {
                    if (V.x - P[U][0] > 1000) {
                        P[U][0] = V.x
                    }
                    if (P[U][R - 2] - Q.x > 1000) {
                        P[U][R - 2] = Q.x
                    }
                } else {
                    if (V.x - P[U][R - 2] > 1000) {
                        P[U][R - 2] = V.x
                    }
                    if (P[U][0] - Q.x > 1000) {
                        P[U][0] = Q.x
                    }
                } if (P[U][1] < P[U][R - 1]) {
                    if (V.y - P[U][1] > 1000) {
                        P[U][1] = V.y
                    }
                    if (P[U][R - 1] - Q.y > 1000) {
                        P[U][R - 1] = Q.y
                    }
                } else {
                    if (V.y - P[U][R - 1] > 1000) {
                        P[U][R - 1] = V.y
                    }
                    if (P[U][1] - Q.y > 1000) {
                        P[U][1] = Q.y
                    }
                }
            }
            this.l11 = P
        } else {
            var P = this.ll.fromLLsToDPxs(this.latlngs);
            var U, X = new Array;
            for (U = 0; U < P.length; U++) {
                X.push(P[U].x);
                X.push(P[U].y)
            }
            this.l11 = X
        }
    }

    function t(j, i) {
        if (this.ll.li1 != undefined) {
            this.ll.li1()
        }
        if (i == undefined) {
            i = new VInfoWindowOptions()
        }
        this.ll.openInfoWindow(this.getCenterPoint(), j, i)
    }

    function a() {
        if (this.isDirectionObj) {
            return null
        }
        var i = VUtil.getCenterPoint(this.latlngs);
        if (!VUtil.isInsidePolygon(i, this.latlngs)) {
            i.longitude = (this.latlngs[0].longitude + this.latlngs[1].longitude) / 2;
            i.latitude = (this.latlngs[0].latitude + this.latlngs[1].latitude) / 2
        }
        return i
    }

    function q() {
        if (this.latlngs == null) {
            return 0
        }
        if (this.isDirectionObj) {
            var j, P = 0;
            for (j = 0; j < this.latlngs.length; j++) {
                P += this.latlngs[j].length
            }
            return P
        }
        return this.latlngs.length
    }

    function L(j) {
        if (this.latlngs == null) {
            return
        }
        if (this.isDirectionObj) {
            var P = 0;
            while (j >= this.latlngs[P].length) {
                j -= this.latlngs[P].length;
                P++
            }
            return this.latlngs[P][j]
        }
        return this.latlngs[j]
    }

    function b(i, j) {
        if (this.isDirectionObj) {
            return
        }
        this.latlngs[i] = j.copy();
        if (i == 0) {
            this.latlngs[this.latlngs.length - 1] = j.copy()
        }
        this.drawLines(this.ll, true)
    }

    function h(P, S) {
        if (this.isDirectionObj) {
            return
        }
        if (P >= this.latlngs.length - 1) {
            return
        }
        var j = new Array();
        var Q, R = 0;
        for (Q = 0; Q < this.latlngs.length; Q++) {
            j[R] = this.latlngs[Q].copy();
            R++;
            if (Q == P) {
                j[R] = S.copy();
                R++
            }
        }
        this.latlngs = j;
        this.drawLines(this.ll, true)
    }

    function r(P) {
        if (this.isDirectionObj) {
            return
        }
        if (P >= this.latlngs.length - 1) {
            return
        }
        var j = new Array();
        var Q, R = 0;
        for (Q = 0; Q < this.latlngs.length; Q++) {
            if (Q != P) {
                j[R] = this.latlngs[Q].copy();
                R++
            }
        }
        if (P == 0) {
            j[R - 1] = j[0].copy()
        }
        this.latlngs = j;
        this.drawLines(this.ll, true)
    }

    function d() { }

    function m() { }

    function B() {
        return this.latlngs
    }

    function g(P) {
        this.latlngs = P;
        if (this.isDirectionObj) {
            var R, Q, T = new Array(),
                U;
            for (R = 0; R < this.latlngs.length; R++) {
                T[R] = new Array();
                for (Q = 0; Q < this.latlngs[R].length; Q += 2) {
                    U = new VLatLng(this.latlngs[R][Q + 1], this.latlngs[R][Q]);
                    T[R].push(U)
                }
            }
            this.latlngs = T
        }
        if (typeof this.latlngs == "string") {
            var S = this.latlngs.split(",");
            this.latlngs = new Array();
            var R, Q = 0;
            for (R = 0; R < S.length; R += 2) {
                this.latlngs[Q] = new VLatLng(parseFloat(S[R + 1]), parseFloat(S[R]));
                Q++
            }
        }
        this.drawLines(this.ll, true)
    }

    function E(Q, i, R, P, j) {
        this.lineColor = (Q == null) ? "red" : Q;
        this.lineWidth = (i == null) ? 1 : i;
        this.fillColor = (R == null) ? "red" : R;
        this.lineOpacity = (P == null) ? 0.5 : P;
        this.fillOpacity = (j == null) ? 0.5 : j;
        this.removeLines();
        this.drawLines(this.ll, false)
    }

    function F() {
        if (this.I1l != null) {
            this.I1l(false)
        }
        this.lIi = true;
        this.removeLines()
    }

    function C() {
        if (this.I1l != null) {
            this.I1l(true)
        }
        this.drawLines(this.ll, true);
        this.lIi = false
    }

    function n() {
        return this.lIi
    }

    function c() {
        return true
    }

    function y() {
        return "VPolygon"
    }
}
VPolygon.prototype = new VOverlay();

function VPolyline(o, J, r, N, H, u) {
    if (o == undefined) {
        return
    }
    this.latlngs = o;
    this.myObject;
    this.lineColor = (J == undefined) ? "red" : J;
    this.lineWidth = (r == undefined) ? 3 : r;
    this.fillColor = (N == undefined) ? "green" : N;
    this.opacity = (H == undefined) ? 0.5 : H;
    this.opts = (u == undefined) ? new VPolylineOptions() : u;
    this.arrPolylines = new Array();
    this.nPolylines = 0;
    this.lIi = false;
    this.isDirectionObj = false;
    this.initialize = d;
    this.drawLines = x;
    this.removeLines = A;
    this.addLineHandlers = v;
    this.removeLineHandlers = h;
    this.remove = t;
    this.copy = L;
    this.redraw = e;
    this.fromEncoded = E;
    this.calculateViewCoor = M;
    this.openInfoWindow = s;
    this.getCenterPoint = a;
    this.getVertexCount = p;
    this.getVertex = K;
    this.setVertex = b;
    this.insertPoint = g;
    this.deletePoint = q;
    this.getLength = I;
    this.getBounds = l;
    this.getLatLngs = y;
    this.setLatLngs = f;
    this.setStyle = B;
    this.hide = C;
    this.show = z;
    this.isHidden = m;
    this.supportsHide = c;
    this.vType = w;
    if (typeof this.latlngs == "object") {
        if (typeof this.latlngs[0] == "object" && this.latlngs[0].vType == undefined) {
            this.isDirectionObj = true;
            var G, F, n, D = new Array();
            for (G = 0; G < this.latlngs.length; G++) {
                D[G] = new Array();
                for (F = 0; F < this.latlngs[G].length; F += 2) {
                    n = new VLatLng(this.latlngs[G][F + 1], this.latlngs[G][F]);
                    D[G].push(n)
                }
            }
            this.latlngs = D
        }
    } else {
        if (typeof this.latlngs == "string") {
            var k = this.latlngs.split(",");
            this.latlngs = new Array();
            var G, F = 0;
            for (G = 0; G < k.length; G += 2) {
                this.latlngs[F] = new VLatLng(parseFloat(k[G + 1]), parseFloat(k[G]));
                F++
            }
        }
    }
    for (var G = 0; G < this.latlngs.length; G++) {
        if (this.latlngs[G].latitude > 180 || this.latlngs[G].longitude > 180) {
            this.latlngs[G] = VProjection.fromPixelToLatLng(new VPoint(this.latlngs.longitude, this.latlngs.latitude), 12)
        }
    }

    function d(i) {
        this.ll = i;
        this.drawLines(i, true)
    }

    function x(Q, S) {
        if (!this.lIi) {
            if (S) {
                this.calculateViewCoor()
            }
            var ad = document.getElementById(Q.id + "_path");
            var ae = Q;
            var P = new Array();
            if (this.isDirectionObj) {
                P = this.l11
            } else {
                P[0] = this.l11
            } if (P.length == 0) {
                this.removeLines();
                return
            }
            var W;
            if (P.length < this.nPolylines && P.length > 0) {
                var aa = (browserName == "msie") ? ad.childNodes[1] : ad.childNodes[1].childNodes[0];
                for (W = P.length; W < this.nPolylines; W++) {
                    aa.removeChild(this.arrPolylines[W]);
                    this.arrPolylines[W] = null
                }
                this.nPolylines = P.length
            }
            for (W = 0; W < P.length; W++) {
                if (browserName == "msie") {
                    if (this.arrPolylines[W] == null || this.arrPolylines[W] == undefined) {
                        var T = document.createElement("v:polyline");
                        this.arrPolylines[this.nPolylines] = T;
                        this.nPolylines++;
                        T.className = "dragline";
                        T.title = this.opts.title;
                        T.strokecolor = this.lineColor;
                        T.strokeweight = this.lineWidth;
                        T.filled = "f";
                        T.fillcolor = this.fillColor;
                        T.strokeopacity = this.opacity;
                        T.fillopacity = this.opacity;
                        var X = document.createElement("v:stroke");
                        X.fillopacity = this.opacity;
                        X.opacity = this.opacity;
                        X.endcap = "round";
                        if (this.opts.dashStyle == "Dot") {
                            X.dashstyle = "Dot"
                        }
                        T.appendChild(X);
                        var R = document.createElement("v:fill");
                        R.fillopacity = this.opacity;
                        R.opacity = this.opacity;
                        T.appendChild(R);
                        ad.childNodes[1].appendChild(T);
                        T.style.cursor = "pointer";
                        T.llj = this.llj;
                        T.mouseoverHandler = this.mouseoverHandler;
                        T.mouseoutHandler = this.mouseoutHandler;
                        T.mousemoveHandler = this.mousemoveHandler;
                        T.II = this;
                        if (this.llj != undefined) {
                            T.onclick = function (ai) {
                                var ag = new VPoint(lI1 - ae.Left, lIl - ae.Top);
                                var i = ae.fromContainerPixelToLatLng(ag);
                                var aj = this.II.latlngs;
                                var j = new Array();
                                if (this.II.isDirectionObj) {
                                    VUtil.ClosestPointPolylines(i.longitude, i.latitude, aj, j)
                                } else {
                                    VUtil.ClosestPointPolyline(i.longitude, i.latitude, aj, j)
                                }
                                var af = j[0];
                                var ah = j[1];
                                this.llj(af, ah, this.II)
                            }
                        }
                        if (this.mouseoverHandler != undefined) {
                            T.onmouseover = function () {
                                this.mouseoverHandler(this.II)
                            }
                        }
                        if (this.mouseoutHandler != undefined) {
                            T.onmouseout = function () {
                                this.mouseoutHandler(this.II)
                            }
                        }
                        if (this.mousemoveHandler != undefined) {
                            T.onmousemove = function (ai) {
                                var ag = new VPoint(lI1 - ae.Left, lIl - ae.Top);
                                var i = ae.fromContainerPixelToLatLng(ag);
                                var aj = this.II.latlngs;
                                var j = new Array();
                                if (this.II.isDirectionObj) {
                                    VUtil.ClosestPointPolylines(i.longitude, i.latitude, aj, j)
                                } else {
                                    VUtil.ClosestPointPolyline(i.longitude, i.latitude, aj, j)
                                }
                                var af = j[0];
                                var ah = j[1];
                                this.mousemoveHandler(af, ah, this.II)
                            }
                        }
                    }
                    if (this.arrPolylines[W].points != undefined) {
                        this.arrPolylines[W].points.value = P[W]
                    }
                } else {
                    var Z = ad.childNodes[1].childNodes[0];
                    var O = Q.lI.offsetWidth * 3;
                    var ac = Q.lI.offsetHeight * 3;
                    Z.setAttribute("width", O.toString() + "px");
                    Z.setAttribute("height", ac.toString() + "px");
                    var ab = Q.fromContainerPixelToDivPixel(new VPoint(-Q.lI.offsetWidth, -Q.lI.offsetHeight));
                    Z.style.left = ab.x.toString() + "px";
                    Z.style.top = ab.y.toString() + "px";
                    Z.setAttribute("viewBox", ab.x.toString() + " " + ab.y.toString() + " " + O.toString() + " " + ac.toString());
                    var T;
                    if (this.arrPolylines[W] == null || this.arrPolylines[W] == undefined) {
                        T = document.createElementNS("http://www.w3.org/2000/svg", "path");
                        T.setAttribute("title", this.opts.title);
                        T.setAttribute("stroke", this.lineColor);
                        T.setAttribute("stroke-width", this.lineWidth + 1);
                        T.setAttribute("stroke-opacity", this.opacity);
                        T.setAttribute("stroke-linecap", "round");
                        T.setAttribute("stroke-linejoin", "round");
                        if (this.opts.dashStyle == "Dot") {
                            T.setAttribute("stroke-dasharray", "0.5,15")
                        }
                        T.setAttribute("fill-opacity", this.opacity);
                        T.setAttribute("fill", "none");
                        Z.appendChild(T);
                        this.arrPolylines[this.nPolylines] = T;
                        this.nPolylines++;
                        T.style.cursor = "pointer";
                        T.llj = this.llj;
                        T.mouseoverHandler = this.mouseoverHandler;
                        T.mouseoutHandler = this.mouseoutHandler;
                        T.mousemoveHandler = this.mousemoveHandler;
                        T.II = this;
                        if (this.llj != undefined) {
                            T.onclick = function (ai) {
                                var ag = new VPoint(lI1 - ae.Left, lIl - ae.Top);
                                var i = ae.fromContainerPixelToLatLng(ag);
                                var aj = this.II.latlngs;
                                var j = new Array();
                                if (this.II.isDirectionObj) {
                                    VUtil.ClosestPointPolylines(i.longitude, i.latitude, aj, j)
                                } else {
                                    VUtil.ClosestPointPolyline(i.longitude, i.latitude, aj, j)
                                }
                                var af = j[0];
                                var ah = j[1];
                                this.llj(af, ah, this.II)
                            }
                        }
                        if (this.mouseoverHandler != undefined) {
                            T.onmouseover = function () {
                                this.mouseoverHandler(this.II)
                            }
                        }
                        if (this.mouseoutHandler != undefined) {
                            T.onmouseout = function () {
                                this.mouseoutHandler(this.II)
                            }
                        }
                        if (this.mousemoveHandler != undefined) {
                            T.onmousemove = function (ai) {
                                var ag = new VPoint(lI1 - ae.Left, lIl - ae.Top);
                                var i = ae.fromContainerPixelToLatLng(ag);
                                var aj = this.II.latlngs;
                                var j = new Array();
                                if (this.II.isDirectionObj) {
                                    VUtil.ClosestPointPolylines(i.longitude, i.latitude, aj, j)
                                } else {
                                    VUtil.ClosestPointPolyline(i.longitude, i.latitude, aj, j)
                                }
                                var af = j[0];
                                var ah = j[1];
                                this.mousemoveHandler(af, ah, this.II)
                            }
                        }
                    } else {
                        T = this.arrPolylines[W]
                    }
                    var U, V = new Array();
                    for (U = 0; U < P[W].length; U += 2) {
                        V.push(P[W][U]);
                        V.push(P[W][U + 1])
                    }
                    var Y = new Array();
                    Y[Y.length] = "M";
                    Y[Y.length] = V[0];
                    Y[Y.length] = V[1];
                    for (U = 2; U < P[W].length; U += 2) {
                        Y[Y.length] = "L";
                        Y[Y.length] = V[U];
                        Y[Y.length] = V[U + 1]
                    }
                    Y = VUtil.trim(Y.join(" "));
                    if (Y != "" && Y.indexOf("NaN") == -1) {
                        T.setAttribute("d", Y)
                    }
                }
            }
        }
    }

    function A() {
    //P5s comment to prevent error can't click marker when find route
        
        if(this.ll == null || this.ll.id == undefined )
            return;
    
        var P = document.getElementById(this.ll.id + "_path");
        var O = (browserName == "msie") ? P.childNodes[1] : P.childNodes[1].childNodes[0];
        if (this.nPolylines > 0) {
            var j;
            for (j = 0; j < this.nPolylines; j++) {
              
                for (K = 0; K < O.childNodes.length; K++) {
                   if(O.childNodes[K] ===  this.arrPolylines[j]) //P5sCheck exists node to remove
                   {
                      O.removeChild(this.arrPolylines[j])
                      break;
                
                   }
                }
                
            }
            this.arrPolylines = new Array();
            this.nPolylines = 0
        }
    }

    function v() {
        var O, j, Q = this.ll;
        for (O = 0; O < this.nPolylines; O++) {
            j = this.arrPolylines[O];
            j.llj = this.llj;
            j.mouseoverHandler = this.mouseoverHandler;
            j.mouseoutHandler = this.mouseoutHandler;
            j.mousemoveHandler = this.mousemoveHandler;
            if (this.llj != undefined) {
                var P = function (V) {
                    var T = new VPoint(lI1 - Q.Left, lIl - Q.Top);
                    var i = Q.fromContainerPixelToLatLng(T);
                    var W = this.II.latlngs;
                    var R = new Array();
                    if (this.II.isDirectionObj) {
                        VUtil.ClosestPointPolylines(i.longitude, i.latitude, W, R)
                    } else {
                        VUtil.ClosestPointPolyline(i.longitude, i.latitude, W, R)
                    }
                    var S = R[0];
                    var U = R[1];
                    this.llj(S, U, this.II)
                };
                if (browserName == "iPhone" || browserName == "iPod" || browserName == "iPad") {
                    j.ontouchend = P
                } else {
                    j.onclick = P
                }
            }
            if (this.mouseoverHandler != undefined) {
                j.onmouseover = function () {
                    this.mouseoverHandler(this.II)
                }
            }
            if (this.mouseoutHandler != undefined) {
                j.onmouseout = function () {
                    this.mouseoutHandler(this.II)
                }
            }
            if (this.mousemoveHandler != undefined) {
                j.onmousemove = function (V) {
                    var T = new VPoint(lI1 - Q.Left, lIl - Q.Top);
                    var i = Q.fromContainerPixelToLatLng(T);
                    var W = this.II.latlngs;
                    var R = new Array();
                    if (this.II.isDirectionObj) {
                        VUtil.ClosestPointPolylines(i.longitude, i.latitude, W, R)
                    } else {
                        VUtil.ClosestPointPolyline(i.longitude, i.latitude, W, R)
                    }
                    var S = R[0];
                    var U = R[1];
                    this.mousemoveHandler(S, U, this.II)
                }
            }
        }
    }

    function h() {
        var O, j;
        for (O = 0; O < this.nPolylines; O++) {
            j = this.arrPolylines[O];
            j.llj = null;
            j.mouseoverHandler = null;
            j.mouseoutHandler = null;
            j.mousemoveHandler = null;
            j.onclick = null;
            j.onmouseover = null;
            j.onmouseout = null;
            j.onmousemove = null
        }
    }

    function t() {
        if (this.il1 != null) {
            this.il1()
        }
        
        //P5s
        if(this.ll == null || this.ll.id == undefined )
            return;
        
        this.removeLines();
        this.ll.resetSVG()
    }

    function L() {
        return new VPolyline(this.latlngs, this.lineColor, this.lineWidth, this.fillColor, this.opacity, this.opts)
    }

    function e(i) {
        this.drawLines(this.ll, !i)
    }

    function E(i, S, j, Q, P, R, O) { }

    function M() {
        if (this.isDirectionObj) {
            var X = new VPoint(-this.ll.lI.offsetWidth * 2, -this.ll.lI.offsetHeight * 2);
            var V = new VPoint(this.ll.lI.offsetWidth * 3, this.ll.lI.offsetHeight * 3);
            var U = this.ll.fromContainerPixelToDivPixel(X);
            var P = this.ll.fromContainerPixelToDivPixel(V);
            var Q, T, S, O = new Array(),
                R;
            for (T = 0; T < this.latlngs.length; T++) {
                O[T] = new Array();
                for (S = 0; S < this.latlngs[T].length; S++) {
                    R = this.ll.fromLatLngToDivPixel(this.latlngs[T][S]);
                    O[T].push(R.x);
                    O[T].push(R.y)
                }
                Q = O[T].length;
                if (O[T][0] < O[T][Q - 2]) {
                    if (U.x - O[T][0] > 1000) {
                        O[T][0] = U.x
                    }
                    if (O[T][Q - 2] - P.x > 1000) {
                        O[T][Q - 2] = P.x
                    }
                } else {
                    if (U.x - O[T][Q - 2] > 1000) {
                        O[T][Q - 2] = U.x
                    }
                    if (O[T][0] - P.x > 1000) {
                        O[T][0] = P.x
                    }
                } if (O[T][1] < O[T][Q - 1]) {
                    if (U.y - O[T][1] > 1000) {
                        O[T][1] = U.y
                    }
                    if (O[T][Q - 1] - P.y > 1000) {
                        O[T][Q - 1] = P.y
                    }
                } else {
                    if (U.y - O[T][Q - 1] > 1000) {
                        O[T][Q - 1] = U.y
                    }
                    if (O[T][1] - P.y > 1000) {
                        O[T][1] = P.y
                    }
                }
            }
            this.l11 = O
        } else {
            var O = this.ll.fromLLsToDPxs(this.latlngs);
            var T, W = new Array;
            for (T = 0; T < O.length; T++) {
                W.push(O[T].x);
                W.push(O[T].y)
            }
            this.l11 = W
        }
    }

    function s(j, i) {
        if (this.ll.li1 != undefined) {
            this.ll.li1()
        }
        if (i == undefined) {
            i = new VInfoWindowOptions()
        }
        this.ll.openInfoWindow(this.getCenterPoint(), j, i)
    }

    function a() {
        if (this.isDirectionObj) {
            return null
        }
        var O, j, i;
        if (this.latlngs.length % 2 == 0) {
            i = parseInt(this.latlngs.length / 2) - 1;
            O = (this.latlngs[i].latitude + this.latlngs[i + 1].latitude) / 2;
            j = (this.latlngs[i].longitude + this.latlngs[i + 1].longitude) / 2
        } else {
            i = parseInt(this.latlngs.length / 2);
            O = this.latlngs[i].latitude;
            j = this.latlngs[i].longitude
        }
        return new VLatLng(O, j)
    }

    function p() {
        if (this.latlngs == null) {
            return 0
        }
        if (this.isDirectionObj) {
            var j, O = 0;
            for (j = 0; j < this.latlngs.length; j++) {
                O += this.latlngs[j].length
            }
            return O
        }
        return this.latlngs.length
    }

    function K(j) {
        if (this.latlngs == null) {
            return
        }
        if (this.isDirectionObj) {
            var O = 0;
            while (j >= this.latlngs[O].length) {
                j -= this.latlngs[O].length;
                O++
            }
            return this.latlngs[O][j]
        }
        return this.latlngs[j]
    }

    function b(i, j) {
        if (this.isDirectionObj) {
            return
        }
        this.latlngs[i] = j.copy();
        this.drawLines(this.ll, true)
    }

    function g(O, R) {
        if (this.isDirectionObj) {
            return
        }
        if (O >= this.latlngs.length) {
            return
        }
        var j = new Array();
        var P, Q = 0;
        for (P = 0; P < this.latlngs.length; P++) {
            j[Q] = this.latlngs[P].copy();
            Q++;
            if (P == O) {
                j[Q] = R.copy();
                Q++
            }
        }
        this.latlngs = j;
        this.drawLines(this.ll, true)
    }

    function q(O) {
        if (this.isDirectionObj) {
            return
        }
        if (O >= this.latlngs.length) {
            return
        }
        var j = new Array();
        var P, Q = 0;
        for (P = 0; P < this.latlngs.length; P++) {
            if (P != O) {
                j[Q] = this.latlngs[P].copy();
                Q++
            }
        }
        this.latlngs = j;
        this.drawLines(this.ll, true)
    }

    function I() {
        if (this.isDirectionObj) {
            return
        }
        var U, O = 0;
        var P, Q, S, V;
        var j, W, T, R;
        for (U = 0; U < this.latlngs.length - 1; U++) {
            j = this.latlngs[U].latitude * (Math.PI / 180);
            W = this.latlngs[U].longitude * (Math.PI / 180);
            T = this.latlngs[U + 1].latitude * (Math.PI / 180);
            R = this.latlngs[U + 1].longitude * (Math.PI / 180);
            P = this.latlngs[U].longitude - this.latlngs[U + 1].longitude;
            V = P * (Math.PI / 180);
            S = Math.sin(j) * Math.sin(T) + Math.cos(j) * Math.cos(T) * Math.cos(V);
            S = Math.acos(S);
            Q = S * 180 / Math.PI;
            Q = Q * 60 * 1.1515;
            Q = Q * 1.609344 * 1000;
            O += Q
        }
        return O
    }

    function l() { }

    function y() {
        return this.latlngs
    }

    function f(O) {
        this.latlngs = O;
        if (this.isDirectionObj) {
            var Q, P, S = new Array(),
                T;
            for (Q = 0; Q < this.latlngs.length; Q++) {
                S[Q] = new Array();
                for (P = 0; P < this.latlngs[Q].length; P += 2) {
                    T = new VLatLng(this.latlngs[Q][P + 1], this.latlngs[Q][P]);
                    S[Q].push(T)
                }
            }
            this.latlngs = S
        } else {
            if (typeof this.latlngs == "string") {
                var R = this.latlngs.split(",");
                this.latlngs = new Array();
                var Q, P = 0;
                for (Q = 0; Q < R.length; Q += 2) {
                    this.latlngs[P] = new VLatLng(parseFloat(R[Q + 1]), parseFloat(R[Q]));
                    P++
                }
            }
        }
        this.drawLines(this.ll, true)
    }

    function B(O, i, P, j) {
        this.lineColor = (O == undefined) ? "red" : O;
        this.lineWidth = (i == undefined) ? 1 : i;
        this.fillColor = (P == undefined) ? "red" : P;
        this.opacity = (j == undefined) ? 1 : j;
        this.removeLines();
        this.drawLines(this.ll, false)
    }

    function C() {
        if (this.I1l != null) {
            this.I1l(false)
        }
        this.lIi = true;
        this.removeLines()
    }

    function z() {
        if (this.I1l != null) {
            this.I1l(true)
        }
        this.drawLines(this.ll, true);
        this.lIi = false
    }

    function m() {
        return this.lIi
    }

    function c() {
        return true
    }

    function w() {
        return "VPolyline"
    }
}
VPolyline.prototype = new VOverlay();

function VCircle(b, h, d, a, l, u, s, q) {
    if (b == undefined) {
        return
    }
    this.latlngs = b;
    this.radius = (h == undefined) ? 100 : h;
    this.lineColor = (d == null) ? "red" : d;
    this.lineWidth = (a == null) ? 3 : a;
    this.fillColor = (l == null) ? "green" : l;
    this.lineOpacity = (u == null) ? 0.5 : u;
    this.fillOpacity = (s == null) ? 0.3 : s;
    this.opts = (q == undefined) ? new VCircleOptions() : q;
    this.circleObj = null;
    this.lIi = false;
    this.initialize = c;
    this.drawLines = g;
    this.removeLines = e;
    this.remove = y;
    this.copy = r;
    this.redraw = t;
    this.openInfoWindow = o;
    this.getBounds = k;
    this.getLatLngs = x;
    this.setLatLngs = n;
    this.getRadius = f;
    this.setRadius = v;
    this.hide = p;
    this.show = w;
    this.isHidden = j;
    this.supportsHide = m;
    this.vType = i;

    function c(z) {
        this.ll = z;
        this.drawLines(z, true)
    }

    function g(B, C) {
        if (!this.lIi) {
            if (C) {
                this.l11 = this.ll.fromLLsToDPxs([this.latlngs])[0]
            }
            var J = document.getElementById(B.id + "_path");
            if (browserName == "msie") {
                if (this.circleObj == null || this.circleObj == undefined) {
                    var E = document.createElement("v:oval");
                    this.circleObj = E;
                    E.className = "dragline";
                    E.title = this.opts.title;
                    E.strokecolor = this.lineColor;
                    E.filled = "t";
                    E.fillcolor = this.fillColor;
                    var F = document.createElement("v:stroke");
                    F.endcap = "round";
                    E.appendChild(F);
                    var D = document.createElement("v:fill");
                    E.appendChild(D);
                    J.childNodes[1].appendChild(E);
                    E.style.position = "absolute";
                    E.style.cursor = "pointer";
                    E.II = this
                }
                var A = this.radius / VProjection.getGroundResolution(this.latlngs.lat(), B.getLevel());
                this.circleObj.style.left = this.l11.x - A + "px";
                this.circleObj.style.top = this.l11.y - A + "px";
                this.circleObj.style.width = A * 2;
                this.circleObj.style.height = A * 2;
                if (A < 10) {
                    this.circleObj.strokeweight = 5;
                    this.circleObj.strokeopacity = 0.7;
                    this.circleObj.fillopacity = 0.7;
                    this.circleObj.childNodes[0].fillopacity = 0.7;
                    this.circleObj.childNodes[0].opacity = 0.7;
                    this.circleObj.childNodes[0].dashstyle = "";
                    this.circleObj.childNodes[1].fillopacity = 0.7;
                    this.circleObj.childNodes[1].opacity = 0.7
                } else {
                    this.circleObj.strokeweight = this.lineWidth;
                    this.circleObj.strokeopacity = this.lineOpacity;
                    this.circleObj.fillopacity = this.fillOpacity;
                    this.circleObj.childNodes[0].fillopacity = this.lineOpacity;
                    this.circleObj.childNodes[0].opacity = this.lineOpacity;
                    if (this.opts.dashStyle == "Dot") {
                        this.circleObj.childNodes[0].dashstyle = "Dot"
                    }
                    this.circleObj.childNodes[1].fillopacity = this.fillOpacity;
                    this.circleObj.childNodes[1].opacity = this.fillOpacity
                }
            } else {
                var G = J.childNodes[1].childNodes[0];
                var z = B.lI.offsetWidth * 3;
                var I = B.lI.offsetHeight * 3;
                G.setAttribute("width", z.toString() + "px");
                G.setAttribute("height", I.toString() + "px");
                var H = B.fromContainerPixelToDivPixel(new VPoint(-B.lI.offsetWidth, -B.lI.offsetHeight));
                G.style.left = H.x.toString() + "px";
                G.style.top = H.y.toString() + "px";
                G.setAttribute("viewBox", H.x.toString() + " " + H.y.toString() + " " + z.toString() + " " + I.toString());
                var E;
                if (this.circleObj == null || this.circleObj == undefined) {
                    E = document.createElementNS("http://www.w3.org/2000/svg", "circle");
                    E.setAttribute("title", this.opts.title);
                    E.setAttribute("stroke", this.lineColor);
                    E.setAttribute("stroke-linecap", "round");
                    E.setAttribute("stroke-linejoin", "round");
                    E.setAttribute("fill", this.fillColor);
                    G.appendChild(E);
                    this.circleObj = E;
                    E.style.cursor = "pointer";
                    E.II = this
                } else {
                    E = this.circleObj
                }
                E.setAttribute("cx", this.l11.x);
                E.setAttribute("cy", this.l11.y);
                var A = this.radius / VProjection.getGroundResolution(this.latlngs.lat(), B.getLevel());
                E.setAttribute("r", A);
                if (A < 10) {
                    E.setAttribute("stroke-width", 5);
                    E.setAttribute("stroke-opacity", 0.7);
                    E.setAttribute("stroke-dasharray", "");
                    E.setAttribute("fill-opacity", 0.7)
                } else {
                    E.setAttribute("stroke-width", this.lineWidth + 1);
                    E.setAttribute("stroke-opacity", this.lineOpacity);
                    if (this.opts.dashStyle == "Dot") {
                        E.setAttribute("stroke-dasharray", "1, 5")
                    }
                    E.setAttribute("fill-opacity", this.fillOpacity)
                }
            }
        }
    }

    function e() {
        var A = document.getElementById(this.ll.id + "_path");
        var z = (browserName == "msie") ? A.childNodes[1] : A.childNodes[1].childNodes[0];
        z.removeChild(this.circleObj);
        this.circleObj = null
    }

    function y() {
        this.removeLines()
    }

    function r() {
        return new VCircle(this.latlngs, this.radius, this.lineColor, this.lineWidth, this.fillColor, this.opacity, this.opts)
    }

    function t(z) {
        this.drawLines(this.ll, !z)
    }

    function o(A, z) {
        if (this.ll.li1 != undefined) {
            this.ll.li1()
        }
        if (z == undefined) {
            z = new VInfoWindowOptions()
        }
        this.ll.openInfoWindow(this.getCenterPoint(), A, z)
    }

    function k() {
        var A = this.radius / VProjection.getGroundResolution(this.latlngs.lat(), this.ll.getLevel());
        var B = this.ll.fromLatLngToContainerPixel(this.latlngs);
        var z = this.ll.fromContainerPixelToLatLng(new VPoint(B.x - A, B.y - A));
        var C = this.ll.fromContainerPixelToLatLng(new VPoint(B.x + A, B.y + A));
        return new VLatLngBounds(z, C)
    }

    function x() {
        return this.latlngs
    }

    function n(z) {
        this.latlngs = z;
        this.drawLines(this.ll, true)
    }

    function f() {
        return this.radius
    }

    function v(z) {
        this.radius = z;
        this.drawLines(this.ll, true)
    }

    function p() {
        this.lIi = true;
        this.removeLines()
    }

    function w() {
        this.drawLines(this.ll, true);
        this.lIi = false
    }

    function j() {
        return this.lIi
    }

    function m() {
        return true
    }

    function i() {
        return "VCircle"
    }
}
VCircle.prototype = new VOverlay();
VProjection = {
    EarthRadius: 6378137,
    MinLatitude: -85.05112878,
    MaxLatitude: 85.05112878,
    MinLongitude: -180,
    MaxLongitude: 180,
    clip: function (c, a, b) {
        return Math.min(Math.max(c, a), b)
    },
    getMapSize: function (a) {
        return 256 << a
    },
    getGroundResolution: function (b, a) {
        b = VProjection.clip(b, VProjection.MinLatitude, VProjection.MaxLatitude);
        return Math.cos(b * Math.PI / 180) * 2 * Math.PI * VProjection.EarthRadius / VProjection.getMapSize(a)
    },
    getMapScale: function (c, a, b) {
        return VProjection.getGroundResolution(c, a) * b / 0.0254
    },
    fromPixelXYToTileXY: function (a) {
        var b = new VPoint(a.x / 256, a.y / 256);
        return b
    },
    fromTileXYToPixelXY: function (b) {
        var a = new VPoint(b.x * 256, b.y * 256);
        return a
    },
    fromLatLngToPixel: function (b, c) {
        var g = VProjection.clip(b.latitude, VProjection.MinLatitude, VProjection.MaxLatitude);
        var a = VProjection.clip(b.longitude, VProjection.MinLongitude, VProjection.MaxLongitude);
        var i = (a + 180) / 360;
        var k = Math.sin(g * Math.PI / 180);
        var h = 0.5 - Math.log((1 + k) / (1 - k)) / (4 * Math.PI);
        var f = VProjection.getMapSize(c);
        var e = VProjection.clip(i * f + 0.5, 0, f - 1);
        var d = VProjection.clip(h * f + 0.5, 0, f - 1);
        var j = new VPoint(e, d);
        return j
    },
    fromPixelToLatLng: function (i, c) {
        var d = VProjection.getMapSize(c);
        i.x = ((i.x % d) + d) % d;
        var e = VProjection.getMapSize(c);
        var h = (VProjection.clip(i.x, 0, e - 1) / e) - 0.5;
        var f = 0.5 - (VProjection.clip(i.y, 0, e - 1) / e);
        var g = 90 - 360 * Math.atan(Math.exp(-f * 2 * Math.PI)) / Math.PI;
        var a = 360 * h;
        var b = new VLatLng(g, a);
        return b
    },
    TileXYToQuadKey: function (e, b) {
        var c = new Array();
        var d, f, a;
        for (d = b; d > 0; d--) {
            f = 0;
            a = 1 << (d - 1);
            if ((e.x & a) != 0) {
                f++
            }
            if ((e.y & a) != 0) {
                f++;
                f++
            }
            c[c.length] = f
        }
        return c.join("")
    },
    vType: function () {
        return "VProjection"
    }
};

function VRoute() {
    this.Distance = 0;
    this.Unit = "";
    this.iil = new Array();
    this.llII = null;
    this.getNumSteps = c;
    this.getStep = b;
    this.getEndLatLng = a;
    this.getSummaryHtml = f;
    this.getDistance = e;
    this.getUnit = h;
    this.getDuration = i;
    this.getPolyline = d;
    this.vType = g;

    function c() {
        return this.iil.length
    }

    function b(j) {
        return this.iil[j]
    }

    function a() {
        return this.iil[this.iil.length - 1].Latlng
    }

    function f() { }

    function d() {
        return this.llII
    }

    function e() {
        return this.Distance
    }

    function h() {
        return this.Unit
    }

    function i() { }

    function g() {
        return "VRoute"
    }
}

function VScaleControl(f, a) {
    this.printable = (f != undefined) ? f : false;
    this.II1 = (a != undefined) ? a : false;
    this.initialize = b;
    this.redraw = i;
    this.remove = c;
    this.getDefaultPosition = e;
    this.hide = d;
    this.show = h;
    this.vType = g;

    function b(m, j) {
        if (isValidKey == false) {
            return
        }
        var k = "";
        k += "<table height=20px cellSpacing=0px cellPadding=0px align=left>";
        k += "<tr vAlign=bottom height=10px>";
        k += "<td id='" + m.id + '_Units\' style="FONT-SIZE: 8pt; COLOR: black; FONT-FAMILY: Tahoma; HEIGHT: 1px;" vAlign=bottom align=left>20 km</td>';
        k += "</tr>";
        k += "<tr vAlign=top>";
        k += "<td vAlign=top align=left height=6px><IMG id='" + m.id + "_Ruler' height=8px src='" + strResURL + "images/RulerB.gif' width=200px align=top>";
        k += "</td>";
        k += "</tr>";
        k += "</table>";
        var l = VUtil.createDivElement(m.id + "_VScaleControl", 0, 0, "auto", "auto", "absolute", "", k);
        l.style.zIndex = ControlZIndex;
        l.style.MozUserSelect = "none";
        m.lI.appendChild(l);
        this.ll = m;
        this.lj = l;
        this.position = j;
        this.redraw()
    }

    function i() {
        if (isValidKey == false) {
            return
        }
        if (this.position != undefined) {
            if (this.position.anchor == V_ANCHOR_TOP_RIGHT) {
                this.lj.style.left = parseInt(this.ll.lI.offsetWidth) - this.position.offset.width - 163 + "px";
                this.lj.style.top = this.position.offset.height + "px"
            } else {
                if (this.position.anchor == V_ANCHOR_TOP_LEFT) {
                    this.lj.style.left = this.position.offset.width + "px";
                    this.lj.style.top = this.position.offset.height + "px"
                } else {
                    if (this.position.anchor == V_ANCHOR_BOTTOM_RIGHT) {
                        this.lj.style.left = parseInt(this.ll.lI.offsetWidth) - this.position.offset.width - 163 + "px";
                        this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - this.position.offset.height - 23 + "px"
                    } else {
                        if (this.position.anchor == V_ANCHOR_BOTTOM_LEFT) {
                            this.lj.style.left = this.position.offset.width + 90 + "px";
                            this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - this.position.offset.height - 23 + "px"
                        }
                    }
                }
            }
        } else {
            if (VUtil.isVBDWebsite()) {
                this.lj.style.left = 10 + "px";
                this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - 30 + "px"
            } else {
                this.lj.style.left = 125 + "px";
                this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - 32 + "px"
            }
        }
    }

    function c() {
        VUtil.removeDivElement(this.id, this.ll.lI.id)
    }

    function e() {
        var j = new VControlPosition(V_ANCHOR_BOTTOM_LEFT, new VSize(10, 7));
        return j
    }

    function d() {
        this.lj.style.display = "none"
    }

    function h() {
        this.lj.style.display = ""
    }

    function g() {
        return "VScaleControl"
    }
}
VScaleControl.prototype = new VControl();

function VSmallMapControl(f, a) {
    this.printable = (f != undefined) ? f : false;
    this.II1 = (a != undefined) ? a : false;
    this.initialize = b;
    this.redraw = i;
    this.remove = c;
    this.getDefaultPosition = e;
    this.hide = d;
    this.show = h;
    this.vType = g;

    function b(A, B) {
        if (isValidKey == false) {
            return
        }
        this.ll = A;
        var r = parseInt(A.lI.offsetWidth) / 2;
        var x = parseInt(A.lI.offsetHeight) / 2;
        var p = new Array();
        if (A.options.skin == "RedSkin") {
            p[p.length] = "<div class='move-control'>";
            p[p.length] = "<a class='clickcontrol top' id='" + A.id + "_btTop' title='B&#7855;c'></a>";
            p[p.length] = "<a class='clickcontrol right' id='" + A.id + "_btRight' title='&#272;&#244;ng'></a>";
            p[p.length] = "<a class='clickcontrol bot' id='" + A.id + "_btBottom' title='Nam'></a>";
            p[p.length] = "<a class='clickcontrol left' id='" + A.id + "_btLeft' title='T&#226;y'></a>";
            p[p.length] = "<a class='clickcontrol topright' id='" + A.id + "_btTopRight' title='&#272;&#244;ng B&#7855;c'></a>";
            p[p.length] = "<a class='clickcontrol botright' id='" + A.id + "_btBottomRight' title='&#272;&#244;ng Nam'></a>";
            p[p.length] = "<a class='clickcontrol botleft' id='" + A.id + "_btBottomLeft' title='T&#226;y Nam'></a>";
            p[p.length] = "<a class='clickcontrol topleft' id='" + A.id + "_btTopLeft' title='T&#226;y B&#7855;c'></a>";
            p[p.length] = "<a class='clickcontrol center' id='" + A.id + "_btFit' title='T&#226;m'></a>";
            p[p.length] = "</div>";
            p[p.length] = "<div class='zoom-control' style='height:0px;'>";
            p[p.length] = "<a class='clickcontrol zoom-in' id='" + A.id + "_btZoomIn' title='Ph&#243;ng to'></a>";
            p[p.length] = "<a class='clickcontrol zoom-out' id='" + A.id + "_btZoomOut' title='Thu nh&#7887;'></a>";
            p[p.length] = "</div>"
        } else {
            if (A.options.skin == "TransSkin") {
                p[p.length] = "<div class='move-control'>";
                p[p.length] = "<a id='" + A.id + "_btTop' class='clickcontrol top'></a>";
                p[p.length] = "<a id='" + A.id + "_btRight' class='clickcontrol right'></a>";
                p[p.length] = "<a id='" + A.id + "_btBottom' class='clickcontrol bot'></a>";
                p[p.length] = "<a id='" + A.id + "_btLeft' class='clickcontrol left'></a>";
                p[p.length] = "<a id='" + A.id + "_btTopRight' class='clickcontrol topright'></a>";
                p[p.length] = "<a id='" + A.id + "_btBottomRight' class='clickcontrol botright'></a>";
                p[p.length] = "<a id='" + A.id + "_btBottomLeft' class='clickcontrol botleft'></a>";
                p[p.length] = "<a id='" + A.id + "_btTopLeft' class='clickcontrol topleft'></a>";
                p[p.length] = "<a id='" + A.id + "_btFit' class='clickcontrol center'></a>";
                p[p.length] = "</div>";
                p[p.length] = "<div class='zoom-control'>";
                p[p.length] = "<div class='zoombtns'>";
                p[p.length] = "<a id='" + A.id + "_btZoomIn' class='clickcontrol zoom-in btns'><span></span></a>";
                p[p.length] = "<a id='" + A.id + "_btZoomOut' class='clickcontrol zoom-out btns'><span></span></a>";
                p[p.length] = "</div>";
                p[p.length] = "</div>"
            } else {
                p[p.length] = "<div class='MapMove'>";
                p[p.length] = "<a class='clickcontrol MMTop' id='" + A.id + "_btTop' title='B&#7855;c'></a>";
                p[p.length] = "<a class='MMRight' id='" + A.id + "_btRight' title='&#272;&#244;ng'></a>";
                p[p.length] = "<a class='clickcontrol MMBot' id='" + A.id + "_btBottom' title='Nam'></a>";
                p[p.length] = "<a class='clickcontrol MMLeft' id='" + A.id + "_btLeft' title='T&#226;y'></a>";
                p[p.length] = "<a class='clickcontrol MMTopRight' id='" + A.id + "_btTopRight' title='&#272;&#244;ng B&#7855;c'></a>";
                p[p.length] = "<a class='clickcontrol MMBotRight' id='" + A.id + "_btBottomRight' title='&#272;&#244;ng Nam'></a>";
                p[p.length] = "<a class='clickcontrol MMBotLeft' id='" + A.id + "_btBottomLeft' title='T&#226;y Nam'></a>";
                p[p.length] = "<a class='clickcontrol MMTopLeft' id='" + A.id + "_btTopLeft' title='T&#226;y B&#7855;c'></a>";
                p[p.length] = "<a class='clickcontrol MMCenter' id='" + A.id + "_btFit' title='T&#226;m'></a>";
                p[p.length] = "</div>";
                p[p.length] = "<div class='MapZoom-top'>";
                p[p.length] = "<div class='MapZoom-bot' style='height:61px;'>";
                p[p.length] = "</div>";
                p[p.length] = "<a class='clickcontrol MZIn' id='" + A.id + "_btZoomIn' title='Ph&#243;ng to'></a>";
                p[p.length] = "<a class='clickcontrol MZOut' id='" + A.id + "_btZoomOut' title='Thu nh&#7887;'></a>";
                p[p.length] = "</div>"
            }
        }
        this.id = A.id + "_VSmallMapControl";
        var o = VUtil.createDivElement(this.id, 0, 0, "60", "auto", "absolute", "", p.join(""));
        o.style.zIndex = ControlZIndex;
        o.style.MozUserSelect = "none";
        A.lI.appendChild(o);
        var m = function () {
            if (A.li1 != undefined) {
                A.li1()
            }
        };
        var l = document.getElementById(A.id + "_btZoomIn");
        var n = document.getElementById(A.id + "_btZoomOut");
        var z = document.getElementById(A.id + "_btFit");
        var u = document.getElementById(A.id + "_btLeft");
        var t = document.getElementById(A.id + "_btRight");
        var y = document.getElementById(A.id + "_btTop");
        var v = document.getElementById(A.id + "_btBottom");
        var q = document.getElementById(A.id + "_btTopRight");
        var k = document.getElementById(A.id + "_btBottomRight");
        var s = document.getElementById(A.id + "_btBottomLeft");
        var j = document.getElementById(A.id + "_btTopLeft");
        l.onclick = function () {
            A.zoomIn()
        };
        n.onclick = function () {
            A.zoomOut()
        };
        z.onclick = function () {
            A.fitOverlays()
        };
        u.onclick = function () {
            m();
            A.panBy(new VSize(-r, 0))
        };
        t.onclick = function () {
            m();
            A.panBy(new VSize(r, 0))
        };
        y.onclick = function () {
            m();
            A.panBy(new VSize(0, -x))
        };
        v.onclick = function () {
            m();
            A.panBy(new VSize(0, x))
        };
        q.onclick = function () {
            m();
            A.panBy(new VSize(r, -x))
        };
        k.onclick = function () {
            m();
            A.panBy(new VSize(r, x))
        };
        s.onclick = function () {
            m();
            A.panBy(new VSize(-r, x))
        };
        j.onclick = function () {
            m();
            A.panBy(new VSize(-r, -x))
        };
        this.lj = o;
        this.position = B;
        this.redraw()
    }

    function i() {
        if (isValidKey == false) {
            return
        }
        if (this.position != undefined) {
            if (this.position.anchor == V_ANCHOR_TOP_RIGHT) {
                this.lj.style.left = parseInt(this.ll.lI.offsetWidth) - this.position.offset.width - 58 + "px";
                this.lj.style.top = this.position.offset.height + "px"
            } else {
                if (this.position.anchor == V_ANCHOR_TOP_LEFT) {
                    this.lj.style.left = this.position.offset.width + "px";
                    this.lj.style.top = this.position.offset.height + "px"
                } else {
                    if (this.position.anchor == V_ANCHOR_BOTTOM_RIGHT) {
                        this.lj.style.left = parseInt(this.ll.lI.offsetWidth) - this.position.offset.width - 58 + "px";
                        this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - this.position.offset.height - 105 + "px"
                    } else {
                        if (this.position.anchor == V_ANCHOR_BOTTOM_LEFT) {
                            this.lj.style.left = this.position.offset.width + "px";
                            this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - this.position.offset.height - 142 + "px"
                        }
                    }
                }
            }
        } else {
            this.lj.style.left = 10 + "px";
            this.lj.style.top = 10 + "px"
        }
    }

    function c() {
        VUtil.removeDivElement(this.id, this.ll.lI.id)
    }

    function e() {
        var j = new VControlPosition(V_ANCHOR_TOP_LEFT, new VSize(10, 10));
        return j
    }

    function d() {
        this.lj.style.display = "none"
    }

    function h() {
        this.lj.style.display = ""
    }

    function g() {
        return "VSmallMapControl"
    }
}
VSmallMapControl.prototype = new VControl();

function VSmallZoomControl(f, a) {
    this.printable = (f != undefined) ? f : false;
    this.II1 = (a != undefined) ? a : false;
    this.initialize = b;
    this.redraw = i;
    this.remove = c;
    this.getDefaultPosition = e;
    this.hide = d;
    this.show = h;
    this.vType = g;

    function b(o, k) {
        if (isValidKey == false) {
            return
        }
        this.ll = o;
        var m = new Array();
        if (o.options.skin == "RedSkin") {
            m[m.length] = "<div class='zoom-control' style='margin:0px; height:0px;'>";
            m[m.length] = "<a class='clickcontrol zoom-in' id='" + o.id + "_btZoomIn' title='Ph&#243;ng to'></a>";
            m[m.length] = "<a class='clickcontrol zoom-out' id='" + o.id + "_btZoomOut' title='Thu nh&#7887;'></a>";
            m[m.length] = "</div>"
        } else {
            if (o.options.skin == "TransSkin") {
                m[m.length] = "<div class='zoom-control' style='margin:0px;'>";
                m[m.length] = "<div class='zoombtns'>";
                m[m.length] = "<a id='" + o.id + "_btZoomIn' class='clickcontrol zoom-in btns'><span></span></a>";
                m[m.length] = "<a id='" + o.id + "_btZoomOut' class='clickcontrol zoom-out btns'><span></span></a>";
                m[m.length] = "</div>";
                m[m.length] = "</div>"
            } else {
                m[m.length] = "<div class='MapZoom-top' style='left:0px; top:0px;'>";
                m[m.length] = "<div class='MapZoom-bot' style='height:61px;'>";
                m[m.length] = "</div>";
                m[m.length] = "<a class='clickcontrol MZIn hover' id='" + o.id + "_btZoomIn' title='Ph&#243;ng to'></a>";
                m[m.length] = "<a class='clickcontrol MZOut' id='" + o.id + "_btZoomOut' title='Thu nh&#7887;'></a>";
                m[m.length] = "</div>"
            }
        }
        this.id = o.id + "_VSmallZoomControl";
        var n = VUtil.createDivElement(this.id, 0, 0, "auto", "auto", "absolute", "", m.join(""));
        n.style.zIndex = ControlZIndex;
        n.style.MozUserSelect = "none";
        o.lI.appendChild(n);
        this.lj = n;
        this.position = k;
        var j = document.getElementById(o.id + "_btZoomIn");
        var l = document.getElementById(o.id + "_btZoomOut");
        j.onclick = function () {
            o.zoomIn()
        };
        l.onclick = function () {
            o.zoomOut()
        };
        this.redraw()
    }

    function i() {
        if (this.position != undefined) {
            if (this.position.anchor == V_ANCHOR_TOP_RIGHT) {
                this.lj.style.left = parseInt(this.ll.lI.offsetWidth) - this.position.offset.width - 18 + "px";
                this.lj.style.top = this.position.offset.height + "px"
            } else {
                if (this.position.anchor == V_ANCHOR_TOP_LEFT) {
                    this.lj.style.left = this.position.offset.width + "px";
                    this.lj.style.top = this.position.offset.height + "px"
                } else {
                    if (this.position.anchor == V_ANCHOR_BOTTOM_RIGHT) {
                        this.lj.style.left = parseInt(this.ll.lI.offsetWidth) - this.position.offset.width - 18 + "px";
                        this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - this.position.offset.height - 36 + "px"
                    } else {
                        if (this.position.anchor == V_ANCHOR_BOTTOM_LEFT) {
                            this.lj.style.left = this.position.offset.width + "px";
                            this.lj.style.top = parseInt(this.ll.lI.offsetHeight) - this.position.offset.height - 73 + "px"
                        }
                    }
                }
            }
        } else {
            this.lj.style.left = 10 + "px";
            this.lj.style.top = 10 + "px"
        }
    }

    function c() {
        VUtil.removeDivElement(this.id, this.ll.lI.id)
    }

    function e() {
        var j = new VControlPosition(V_ANCHOR_TOP_LEFT, new VSize(10, 10));
        return j
    }

    function d() {
        this.lj.style.display = "none"
    }

    function h() {
        this.lj.style.display = ""
    }

    function g() {
        return "VSmallZoomControl"
    }
}
VSmallZoomControl.prototype = new VControl();

function VStep() {
    this.StartIndex = -1;
    this.EndIndex = -1;
    this.Latlng = null;
    this.l1ij = "";
    this.Distance = 0;
    this.Unit = "";
    this.Name = "";
    this.i1i = "";
    this.StartAt = null;
    this.getLatLng = c;
    this.getPolylineIndex = a;
    this.getDescriptionHTML = g;
    this.getDistance = e;
    this.getUnit = h;
    this.getDuration = b;
    this.getDirection = d;
    this.vType = f;

    function c() {
        return this.StartAt
    }

    function a() {
        return this.StartIndex
    }

    function g() {
        return this.l1ij
    }

    function e() {
        return (this.Distance)
    }

    function h() {
        return (this.Unit)
    }

    function b() { }

    function d() {
        return this.i1i
    }

    function f() {
        return "VStep"
    }
}

function VText(a, k, e, g) {
    this.latlngs = a;
    this.myObject;
    this.text = (k != undefined) ? k : "vietbando";
    this.textStyle = (e != undefined) ? e : new VTextStyle();
    this.textOptions = (g != undefined) ? g : new VTextOptions();
    this.initialize = b;
    this.createText = q;
    this.remove = s;
    this.copy = n;
    this.redraw = o;
    this.openInfoWindow = h;
    this.enableDragging = f;
    this.disableDragging = j;
    this.getLatLng = m;
    this.setText = p;
    this.hide = i;
    this.show = r;
    this.isHidden = d;
    this.vType = c;
    if (typeof this.latlngs == "string") {
        var l = this.latlngs.split(",");
        this.latlngs = new VLatLng(parseFloat(l[1]), parseFloat(l[0]))
    }

    function b(t) {
        this.ll = t;
        this.createText(t, false)
    }

    function q(y, A) {
        var t, x = document.getElementById(y.id + "_path");
        if (!A) {
            t = document.createElement("div");
            this.id = y.id + "_VText";
            t.id = y.id + "_VText";
            t.style.overflow = "visible";
            t.style.position = "absolute";
            t.className = "dragtext";
            t.II = this;
            x.childNodes[0].appendChild(t);
            this.lj = t
        } else {
            t = this.lj
        }
        var u = "<a id='" + y.id + "_VTextCt' style='cursor:pointer; font-size:" + this.textStyle.fontSize + "px; font-weight:" + this.textStyle.fontWeight + "; padding-left:" + this.textStyle.paddingLeft +  "; color:" + this.textStyle.color + "; font-family:" + this.textStyle.fontFamily + ";'>" + this.text + "</a>";
        t.innerHTML = u;
        var w = document.createElement("div");
        w.id = "divtemp";
        w.innerHTML = u;
        w.style.overflow = "visible";
        w.style.position = "absolute";
        document.body.appendChild(w);
        var z = w.offsetWidth;
        var v = w.offsetHeight;
        document.body.removeChild(w);
        t.style.width = z + "px";
        t.style.height = v + "px";
        if (this.textOptions.offset == null) {
            this.textOptions.offset = new VSize(this.lj.offsetWidth / 2, this.lj.offsetHeight / 2)
        }
        this.redraw()
    }

    function s() {
        this.lj.parentNode.removeChild(this.lj)
    }

    function n() {
        return new VText(this.latlngs, this.text, this.textStyle)
    }

    function o() {
        var t = this.ll.fromLLsToDPxs([this.latlngs])[0];
        this.lj.style.left = (t.x - this.textOptions.offset.width) + "px";
        this.lj.style.top = (t.y - this.textOptions.offset.height) + "px"
    }

    function h(u, t) {
        this.bOpenedInfoWindow = true;
        if (this.ll.li1 != undefined) {
            this.ll.li1()
        }
        if (t == undefined) {
            t = new VInfoWindowOptions()
        }
        this.ll.openInfoWindow(this.latlngs, u, t)
    }

    function f() {
        this.textOptions.draggable = true;
        this.lj.draggable = 1
    }

    function j() {
        this.textOptions.draggable = false;
        this.lj.draggable = 0
    }

    function m() {
        return this.latlngs
    }

    function p(v, u, t) {
        this.text = (v != undefined) ? v : "vietbando";
        this.textStyle = (u != undefined) ? u : new VTextStyle();
        this.textOptions = (t != undefined) ? t : new VTextOptions();
        this.createText(this.ll, true)
    }

    function i() {
        this.lj.style.display = "none"
    }

    function r() {
        this.lj.style.display = ""
    }

    function d() {
        if (this.lj.style.visibility == "hidden") {
            return true
        }
        return false
    }

    function c() {
        return "VText"
    }
}
VText.prototype = new VOverlay();

function VTextOptions(a, b) {
    this.draggable = (a != undefined) ? a : false;
    this.offset = (b != undefined) ? b : null;
    this.vType = function () {
        return "VTextOptions"
    }
}

function VKeyboardHandler(a) {
    if (a == undefined) {
        return
    }
    this.ll = a;
    VEvent.addDOMListener(document, "keydown", function (i) {
        if (!a.bMapActive) {
            return
        }
        var b = (browserName != "msie") ? "HTML" : "BODY";
        var d = (browserName != "msie") ? i.target : event.srcElement;
        if (d.tagName == "INPUT" || d.tagName == "TEXTAREA") {
            return
        }
        var c = parseInt(a.lI.offsetWidth) / 2;
        var f = parseInt(a.lI.offsetHeight) / 2;
        var g = (window.event) ? event.keyCode : i.keyCode;
        switch (g) {
            case 37:
                a.panBy(new VSize(-20, 0));
                break;
            case 38:
                a.panBy(new VSize(0, -20));
                break;
            case 39:
                a.panBy(new VSize(20, 0));
                break;
            case 40:
                a.panBy(new VSize(0, 20));
                break;
            case 33:
                a.panBy(new VSize(0, -f));
                break;
            case 34:
                a.panBy(new VSize(0, f));
                break;
            case 36:
                a.panBy(new VSize(-c, 0));
                break;
            case 35:
                a.panBy(new VSize(c, 0));
                break;
            case 107:
                a.zoomIn();
                break;
            case 109:
                a.zoomOut();
                break;
            case 106:
                a.fitOverlays();
                break
        }
    });
    this.vType = function () {
        return "VKeyboardHandler"
    }
}

function VArrow(a, y, s, o, d) {
    if (a == undefined) {
        return
    }
    this.latlngs = a;
    this.position = (y == undefined) ? "end" : y;
    this.color = (s == undefined) ? "red" : s;
    this.width = (o == undefined) ? 2 : o;
    this.opacity = (d == undefined) ? 0.6 : d;
    this.arrArrows = new Array();
    this.nArrows = 0;
    this.lIi = false;
    this.isDirectionObj = false;
    this.initialize = b;
    this.drawLines = e;
    this.calculateViewCoor = w;
    this.drawArrow = k;
    this.removeLines = c;
    this.remove = z;
    this.copy = r;
    this.redraw = u;
    this.setLatLngs = m;
    this.hide = n;
    this.show = x;
    this.isHidden = g;
    this.supportsHide = h;
    this.vType = f;
    if (typeof this.latlngs == "object") {
        if (typeof this.latlngs[0] == "object" && this.latlngs[0].vType == undefined) {
            this.isDirectionObj = true;
            var v, t, l = new Array(),
                p;
            for (v = 0; v < this.latlngs.length; v++) {
                l[v] = new Array();
                for (t = 0; t < this.latlngs[v].length; t += 2) {
                    p = new VLatLng(this.latlngs[v][t + 1], this.latlngs[v][t]);
                    l[v].push(p)
                }
            }
            this.latlngs = l
        }
    } else {
        if (typeof this.latlngs == "string") {
            var q = this.latlngs.split(",");
            this.latlngs = new Array();
            var v, t = 0;
            for (v = 0; v < q.length; v += 2) {
                this.latlngs[t] = new VLatLng(parseFloat(q[v + 1]), parseFloat(q[v]));
                t++
            }
        }
    }

    function b(i) {
        this.ll = i;
        this.drawLines(i, true)
    }

    function e(V, E) {
        if (!this.lIi) {
            if (E) {
                this.calculateViewCoor()
            }
            if (!this.isDirectionObj) {
                this.l11 = [this.l11]
            }
            if (this.l11.length == 0) {
                this.removeLines();
                return
            }
            var R, Q, O;
            var M = document.getElementById(V.id + "_path");
            if (this.l11.length < this.nArrows && this.l11.length > 0) {
                var I = (browserName == "msie") ? M.childNodes[1] : M.childNodes[1].childNodes[0];
                for (R = this.l11.length; R < this.nArrows; R++) {
                    I.removeChild(this.arrArrows[R]);
                    this.arrArrows[R] = null
                }
                this.nArrows = this.l11.length
            }
            var U, C, T, B, G, F, D = true;
            for (O = 0; O < this.l11.length; O++) {
                for (R = 0; R < this.l11[O].length - 3; R += 2) {
                    D = true;
                    U = this.l11[O][R];
                    C = this.l11[O][R + 1];
                    T = this.l11[O][R + 2];
                    B = this.l11[O][R + 3];
                    if (this.position == "middle") {
                        T = (this.l11[O][R + 2] + this.l11[O][R]) / 2;
                        B = (this.l11[O][R + 3] + this.l11[O][R + 1]) / 2
                    }
                    if (R == 0) {
                        G = T;
                        F = B
                    } else {
                        if (VUtil.Distance(G, F, T, B) < 10000 || VUtil.Distance(U, C, T, B) < 600) {
                            D = false
                        } else {
                            G = T;
                            F = B
                        }
                    } if (browserName == "msie") {
                        if (this.arrArrows[R] == null || this.arrArrows[R] == undefined) {
                            var P = document.createElement("v:polyline");
                            this.arrArrows[this.nArrows] = P;
                            this.nArrows++;
                            P.className = "dragline";
                            P.strokecolor = this.color;
                            P.strokeweight = this.width;
                            P.filled = "t";
                            P.fillcolor = this.color;
                            P.strokeopacity = this.opacity;
                            P.fillopacity = this.opacity;
                            var S = document.createElement("v:stroke");
                            S.fillopacity = this.opacity;
                            S.opacity = this.opacity;
                            S.endcap = "square";
                            S.joinstyle = "miter";
                            P.appendChild(S);
                            var A = document.createElement("v:fill");
                            A.fillopacity = this.opacity;
                            A.opacity = this.opacity;
                            P.appendChild(A);
                            M.childNodes[1].appendChild(P);
                            P.style.cursor = "pointer"
                        }
                        if (D) {
                            this.arrArrows[R / 2].points.value = this.drawArrow(U, C, T, B)
                        } else {
                            this.arrArrows[R / 2].points.value = ""
                        }
                    } else {
                        var L = M.childNodes[1].childNodes[0];
                        var K = V.lI.offsetWidth * 3;
                        var J = V.lI.offsetHeight * 3;
                        L.setAttribute("width", K.toString() + "px");
                        L.setAttribute("height", J.toString() + "px");
                        var N = V.fromContainerPixelToDivPixel(new VPoint(-V.lI.offsetWidth, -V.lI.offsetHeight));
                        L.style.left = N.x.toString() + "px";
                        L.style.top = N.y.toString() + "px";
                        L.setAttribute("viewBox", N.x.toString() + " " + N.y.toString() + " " + K.toString() + " " + J.toString());
                        var P;
                        if (this.arrArrows[R] == null || this.arrArrows[R] == undefined) {
                            P = document.createElementNS("http://www.w3.org/2000/svg", "path");
                            P.setAttribute("stroke", this.color);
                            P.setAttribute("stroke-width", this.width + 1);
                            P.setAttribute("stroke-opacity", this.opacity);
                            P.setAttribute("stroke-linecap", "square");
                            P.setAttribute("stroke-linejoin", "miter");
                            P.setAttribute("fill-opacity", this.opacity);
                            P.setAttribute("fill", this.color);
                            L.appendChild(P);
                            this.arrArrows[this.nArrows] = P;
                            this.nArrows++;
                            P.style.cursor = "pointer"
                        } else {
                            P = this.arrArrows[R]
                        } if (D) {
                            var H = this.drawArrow(U, C, T, B);
                            var W = new Array();
                            W[W.length] = "M";
                            W[W.length] = H[0];
                            W[W.length] = H[1];
                            for (Q = 2; Q < H.length; Q += 2) {
                                W[W.length] = "L";
                                W[W.length] = H[Q];
                                W[W.length] = H[Q + 1]
                            }
                            W = VUtil.trim(W.join(" "));
                            if (W != "" && W.indexOf("NaN") == -1) {
                                P.setAttribute("d", W)
                            }
                        }
                    }
                }
            }
        }
    }

    function w() {
        if (this.isDirectionObj) {
            var J = new VPoint(-this.ll.lI.offsetWidth * 2, -this.ll.lI.offsetHeight * 2);
            var H = new VPoint(this.ll.lI.offsetWidth * 3, this.ll.lI.offsetHeight * 3);
            var G = this.ll.fromContainerPixelToDivPixel(J);
            var B = this.ll.fromContainerPixelToDivPixel(H);
            var C, F, E, A = new Array(),
                D;
            for (F = 0; F < this.latlngs.length; F++) {
                A[F] = new Array();
                for (E = 0; E < this.latlngs[F].length; E++) {
                    D = this.ll.fromLatLngToDivPixel(this.latlngs[F][E]);
                    A[F].push(D.x);
                    A[F].push(D.y)
                }
                C = A[F].length;
                if (A[F][0] < A[F][C - 2]) {
                    if (G.x - A[F][0] > 1000) {
                        A[F][0] = G.x
                    }
                    if (A[F][C - 2] - B.x > 1000) {
                        A[F][C - 2] = B.x
                    }
                } else {
                    if (G.x - A[F][C - 2] > 1000) {
                        A[F][C - 2] = G.x
                    }
                    if (A[F][0] - B.x > 1000) {
                        A[F][0] = B.x
                    }
                } if (A[F][1] < A[F][C - 1]) {
                    if (G.y - A[F][1] > 1000) {
                        A[F][1] = G.y
                    }
                    if (A[F][C - 1] - B.y > 1000) {
                        A[F][C - 1] = B.y
                    }
                } else {
                    if (G.y - A[F][C - 1] > 1000) {
                        A[F][C - 1] = G.y
                    }
                    if (A[F][1] - B.y > 1000) {
                        A[F][1] = B.y
                    }
                }
            }
            this.l11 = A
        } else {
            var F, D, I = new Array();
            for (F = 0; F < this.latlngs.length; F++) {
                D = this.ll.fromLatLngToDivPixel(this.latlngs[F]);
                I.push(D.x);
                I.push(D.y)
            }
            this.l11 = I
        }
    }

    function k(L, I, i, H) {
        var G = 10;
        var D = 0.823,
            J;
        var A = [];
        var K = [];
        var M = [];
        var j = [];
        var B, F, E, C;
        A[0] = i;
        K[0] = H;
        M[0] = A[0] - L;
        M[1] = K[0] - I;
        j[0] = -M[1];
        j[1] = M[0];
        J = Math.sqrt(M[0] * M[0] + M[1] * M[1]);
        B = G / (2 * J);
        F = G / (2 * (Math.tan(D) / 2) * J);
        E = A[0] - F * M[0];
        C = K[0] - F * M[1];
        A[1] = E + B * j[0];
        K[1] = C + B * j[1];
        A[2] = E - B * j[0];
        K[2] = C - B * j[1];
        return [E, C, A[1], K[1], A[0], K[0], A[2], K[2], E, C]
    }

    function c() {
        var B = document.getElementById(this.ll.id + "_path");
        var A = (browserName == "msie") ? B.childNodes[1] : B.childNodes[1].childNodes[0];
        if (this.nArrows > 0) {
            var j;
            for (j = 0; j < this.nArrows; j++) {
                A.removeChild(this.arrArrows[j])
            }
            this.arrArrows = new Array();
            this.nArrows = 0
        }
    }

    function z() {
        this.removeLines();
        this.ll.resetSVG()
    }

    function r() {
        return new VArrow(this.latlngs, this.position, this.color, this.width, this.opacity)
    }

    function u(i) {
        this.drawLines(this.ll, !i)
    }

    function m(A) {
        this.latlngs = A;
        if (this.isDirectionObj) {
            var C, B, E = new Array(),
                F;
            for (C = 0; C < this.latlngs.length; C++) {
                E[C] = new Array();
                for (B = 0; B < this.latlngs[C].length; B += 2) {
                    F = new VLatLng(this.latlngs[C][B + 1], this.latlngs[C][B]);
                    E[C].push(F)
                }
            }
            this.latlngs = E
        } else {
            if (typeof this.latlngs == "string") {
                var D = this.latlngs.split(",");
                this.latlngs = new Array();
                var C, B = 0;
                for (C = 0; C < D.length; C += 2) {
                    this.latlngs[B] = new VLatLng(parseFloat(D[C + 1]), parseFloat(D[C]));
                    B++
                }
            }
        }
        this.drawLines(this.ll, true)
    }

    function n() {
        this.lIi = true;
        this.removeLines()
    }

    function x() {
        this.drawLines(this.ll, true);
        this.lIi = false
    }

    function g() {
        return this.lIi
    }

    function h() {
        return true
    }

    function f() {
        return "VArrow"
    }
}
VArrow.prototype = new VOverlay();