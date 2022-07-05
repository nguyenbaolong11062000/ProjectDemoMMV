using System;
using System.Data;
using System.Configuration;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using L5sDmComm;
using P5sCmm;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using P5sDmComm;

namespace MMV.fsmdls.mmcv4
{
    partial class TrackingPH : System.Web.UI.Page
    {

        public L5sAutocomplete P5sActDistributor;
        public L5sAutocomplete P5sActSupervisor;
        public L5sAutocomplete P5sActSales;
        public L5sAutocomplete P5sActRegion, P5sActArea, P5sActASM;

        Dictionary<String, String> menuReports = new Dictionary<String, String>();


        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);

            //nếu use xem bằng HH thì redirect sang trang được thiết kế cho HH
            //về cơ chế thì 2 trang này giống nhau cách xử lý còn giao diện thì được thiết kế để tương thích với HH
            String userAgent = Request.UserAgent;
            if (userAgent.Contains("BlackBerry") || (userAgent.Contains("iPhone") || (userAgent.Contains("Android"))))
            {
                L5sTransferrer.L5sGoto(null, "./TrackingForHH.aspx");
                return;
            }

            DataTable dtLocationCountry = L5sSql.Query("SELECT NAME, VALUE FROM S_PARAMS WHERE NAME = 'LOCATION_COUNTRY'");
            if (dtLocationCountry != null && dtLocationCountry.Rows.Count > 0)
            {
                this.P5sLocaltionCountry.Value = dtLocationCountry.Rows[0]["VALUE"].ToString();
            }

            if (!IsPostBack)
            {

                //khỏi tạo tham số cho dropdownlist
                P5sCmm.P5sCmmFns.P5sInitHHMM(this.P5sDdlFromHH, this.P5sDdlFromMM);  // Gọi hàm khỏi tạo control HH, MM
                P5sCmm.P5sCmmFns.P5sInitHHMM(this.P5sDdlToHH, this.P5sDdlToMM);  // Gọi hàm khỏi tạo control HH, MM


                DataTable dtStartWorking = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_TIME_START_WORKING' AND ACTIVE = 1 ");
                DataTable dtEndWorking = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_TIME_END_WORKING' AND ACTIVE = 1 ");

                String[] timeStartWorking = dtStartWorking.Rows[0][0].ToString().Split(new Char[] { ':' });
                String[] timeEndWorking = dtEndWorking.Rows[0][0].ToString().Split(new Char[] { ':' });

                this.P5sDdlFromHH.SelectedValue = timeStartWorking[0];
                this.P5sDdlFromMM.SelectedValue = timeStartWorking[1];


                this.P5sDdlToHH.SelectedValue = timeEndWorking[0];
                this.P5sDdlToMM.SelectedValue = timeEndWorking[1];

                int HSS = int.Parse(timeStartWorking[0]);
                int MMS = int.Parse(timeStartWorking[1]);

                int HHE = int.Parse(timeEndWorking[0]);
                int MME = int.Parse(timeEndWorking[1]);

                for (int i = this.P5sDdlFromHH.Items.Count -1 ; i >= 0  ; i--)
                {
                    ListItem li;
                    li = this.P5sDdlFromHH.Items[i];
                    if (int.Parse(li.Value) < HSS || int.Parse(li.Value) > HHE)
                    {
                        this.P5sDdlFromHH.Items.Remove(P5sDdlFromHH.Items.FindByValue(li.Value));
                        this.P5sDdlToHH.Items.Remove(P5sDdlToHH.Items.FindByValue(li.Value));
                    }
                }
            }


            if (!IsPostBack)
            {
                this.P5sInit();
                this.P5sTxtDay.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }

            //gọi hàm javascript để khỏi tạo bản đồ
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "Javascript", "$(document).ready(function(){LoadMap();});", true);

        }

        private void P5sInit()
        {

            DateTime date = DateTime.Now;

            DataTable dtRegion = P5sCmmFns.P5sGetRegion("");

            P5sDDLRegion.DataSource = dtRegion;
            P5sDDLRegion.DataTextField = "REGION_CODE";
            P5sDDLRegion.DataValueField = "REGION_CD";
            P5sDDLRegion.DataBind();
            P5sDDLRegion.SelectedIndex = 0;

            DataTable dtArea = P5sGetArea(P5sDDLRegion.SelectedValue.ToString());
            P5sDDLArea.DataSource = dtArea;
            P5sDDLArea.DataTextField = "AREA_CODE";
            P5sDDLArea.DataValueField = "AREA_CD";
            P5sDDLArea.DataBind();
            P5sDDLArea.SelectedIndex = 0;

            DataTable dtDistri = P5sGetDistributor(P5sDDLArea.SelectedValue.ToString());
            P5sDDLDistributor.DataSource = dtDistri;
            P5sDDLDistributor.DataTextField = "DISTRI_NAME";
            P5sDDLDistributor.DataValueField = "DISTRIBUTOR_CD";
            P5sDDLDistributor.DataBind();
            DataRow[] rows = dtDistri.Select("DISTRI_NAME like '%100568%'");
            int n = rows.Length;
            int SelectedIndex = 0;
            if (rows.Length > 0)
            {
                SelectedIndex = dtDistri.Rows.IndexOf(rows[0]);
            }
            string kq = dtDistri.Rows[SelectedIndex]["DISTRIBUTOR_CD"].ToString();
            P5sDDLDistributor.SelectedValue = kq;

            DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            selDSR.DataSource = dtSale;
            selDSR.DataTextField = "NAME_SALE";
            selDSR.DataValueField = "SALES_CD";
            selDSR.DataBind();
            //String[] dSR = new String[selDSR.Items.Count];
            //for (int i = 0; i < selDSR.Items.Count; i++)
            //{
            //    selDSR.Items[i].Selected = true;
            //}

            // if(selDSR.Items[1].Selected == true)
            try
            {
                if (this.P5sTxtDay.Text != "")
                    date = DateTime.ParseExact(this.P5sTxtDay.Text, "dd-MM-yyyy", null);
                else
                    date = DateTime.ParseExact(Request[this.P5sTxtDay.UniqueID], "dd-MM-yyyy", null);

            }
            catch (Exception)
            {

            }
            this.P5sAutoCompleteInit();
        }

        private void P5sAutoCompleteInit()
        {
            //hàm khỏi tạo autocomplete tùy vào loại mà user chọn: ASM, CDS,DSR
            switch (P5sDdlViewOption.SelectedValue)
            {
                case "1": //sales
                    {
                        this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRegion(""), this.P5sTxtRegion.ClientID, 1, true, this.P5sTxtArea.ClientID) : this.P5sActRegion;
                        this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(P5sCmmFns.P5sGetArea("REGION_CD"), this.P5sTxtArea.ClientID, 1, true, this.P5sTxtDistributor.ClientID) : this.P5sActArea;
                        this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegion.ClientID);

                        this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistributor("AREA_CD"), this.P5sTxtDistributor.ClientID, 1, true, this.P5sTxtSales.ClientID) : this.P5sActDistributor;
                        this.P5sActDistributor.L5sChangeFilteringId(this.P5sTxtArea.ClientID);

                        this.P5sActSales = this.P5sActSales == null ? new L5sAutocomplete(P5sCmmFns.P5sGetSales("DISTRIBUTOR_CD"), this.P5sTxtSales.ClientID, 0, true) : this.P5sActSales;
                        this.P5sActSales.L5sChangeFilteringId(this.P5sTxtDistributor.ClientID);

                        //thiết lập giá trị null cho autocomplete, nếu ko thiết lập thì sẽ bị lỗi autocomplete vì ko dc khỏi tạo
                        this.P5sActSupervisor = this.P5sActSupervisor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSupervisor.ClientID, 0, true) : this.P5sActSupervisor;

                        //thiết lập giá trị null cho autocomplete, nếu ko thiết lập thì sẽ bị lỗi autocomplete vì ko dc khỏi tạo
                        this.P5sActASM = this.P5sActASM == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtASM.ClientID, 0, true) : this.P5sActASM;

                        break;
                    }
                case "2": //supervisor
                    {
                        this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRegion(""), this.P5sTxtRegion.ClientID, 1, true, this.P5sTxtArea.ClientID) : this.P5sActRegion;
                        this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(P5sCmmFns.P5sGetArea("REGION_CD"), this.P5sTxtArea.ClientID, 1, true, this.P5sTxtSupervisor.ClientID) : this.P5sActArea;
                        this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegion.ClientID);

                        this.P5sActSupervisor = this.P5sActSupervisor == null ? new L5sAutocomplete(P5sCmmFns.P5sGetSupervisorByArea("AREA_CD"), this.P5sTxtSupervisor.ClientID, 0, true) : this.P5sActSupervisor;
                        this.P5sActSupervisor.L5sChangeFilteringId(this.P5sTxtArea.ClientID);

                        //thiết lập giá trị null cho autocomplete, nếu ko thiết lập thì sẽ bị lỗi autocomplete vì ko dc khỏi tạo
                        this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtDistributor.ClientID, 1, true) : this.P5sActDistributor;
                        this.P5sActSales = this.P5sActSales == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSales.ClientID, 0, true) : this.P5sActSales;

                        //thiết lập giá trị null cho autocomplete, nếu ko thiết lập thì sẽ bị lỗi autocomplete vì ko dc khỏi tạo
                        this.P5sActASM = this.P5sActASM == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtASM.ClientID, 0, true) : this.P5sActASM;

                        break;
                    }
                case "3": //ASM
                    {
                        this.P5sActASM = this.P5sActASM == null ? new L5sAutocomplete(P5sCmmFns.P5sGetASM(""), this.P5sTxtASM.ClientID, 0, true) : this.P5sActASM;

                        //thiết lập giá trị null cho autocomplete, nếu ko thiết lập thì sẽ bị lỗi autocomplete vì ko dc khỏi tạo
                        this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtDistributor.ClientID, 1, true) : this.P5sActDistributor;
                        this.P5sActSales = this.P5sActSales == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSales.ClientID, 0, true) : this.P5sActSales;
                        this.P5sActSupervisor = this.P5sActSupervisor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSupervisor.ClientID, 0, true) : this.P5sActSupervisor;
                        this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtRegion.ClientID, 1, true) : this.P5sActRegion;
                        this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtArea.ClientID, 1, true) : this.P5sActArea;
                        break;
                    }
            }
        }


        protected void P5sLbtnTrackingVisit_Click(object sender, EventArgs e)
        {
            Response.Redirect("./TrackingVisitUnVisitPH.aspx");
        }

        protected void P5sSettingAndReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Report/CustomerList.aspx");
        }

        protected void P5sLbtnLoadTracking_Click(object sender, EventArgs e)
        {
            L5sJS.L5sRun("LoadMap()"); // load map first

            String[] dSR = new String[selDSR.Items.Count];
            int f = 0;
            for (int i = 0; i < selDSR.Items.Count; i++)
            {

                if (selDSR.Items[i].Selected == true)
                {
                    dSR[f] = selDSR.Items[i].Value;
                    f++;
                }
            }
            String dsR = "";
            for (int i = 0; i < f; i++)
            {
                dsR += dSR[i];
                if (i < f - 1)
                    dsR += ",";

            }

            String salesCDs = dsR;
            //String supervisors = this.P5sTxtSupervisor.Text;
            //String asms = this.P5sTxtASM.Text;
            //String distributors = this.P5sTxtDistributor.Text;

            DateTime date = DateTime.Now;
            if (Request[this.P5sTxtDay.UniqueID].ToString() != "")
            {
                try
                {
                    date = DateTime.ParseExact(Request[this.P5sTxtDay.UniqueID], "dd-MM-yyyy", null);
                }
                catch (Exception)
                {
                    this.P5sTxtDay.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    L5sMsg.Show("Date invalid.");
                    this.P5sInit();
                    return;
                }
            }

            String yymmdd = date.ToString("yyMMdd");

            String timeFrom = String.Format("{0}:{1}:00", this.P5sDdlFromHH.SelectedValue, this.P5sDdlFromMM.SelectedValue);
            String timeTo = String.Format("{0}:{1}:59", this.P5sDdlToHH.SelectedValue, this.P5sDdlToMM.SelectedValue);


            if (salesCDs.Trim().Length > 0)
            {
                //load dữ liệu tracking
                DataTable dt = this.P5sGetTrackingOfSale(salesCDs, yymmdd, timeFrom, timeTo);
                if (dt.Rows.Count <= 0)
                {
                    L5sMsg.Show("No data.");
                    return;
                }
                //load dữ liệu Khách hàng
                DataTable dtCustomer = this.P5sGetCustomer(salesCDs, date.ToString("yyMMdd"));

                //convert  datatable sang json để xử dụng ở javascript
                String objectCustomer = this.P5sConvertDataTableCustomerJson(dtCustomer);
                String objectTracking = this.P5sConvertDataTableTrackingJson(dt);
                L5sJS.L5sRun("CreatedCTracking('" + objectTracking + "','" + objectCustomer + "')");
            }
            else
            {
                salesCDs = string.Format(@"SELECT sls.SALES_CD
                                            FROM  M_SALES sls INNER JOIN M_DISTRIBUTOR dist ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
                                            where dist.DISTRIBUTOR_CD in ({0}) and sls.ACTIVE = 1", this.P5sDDLDistributor.SelectedValue);
                DataTable dt = this.P5sGetTrackingOfSale(salesCDs, yymmdd, timeFrom, timeTo);
                if (dt.Rows.Count <= 0)
                {
                    L5sMsg.Show("No data.");
                    return;
                }
                //load dữ liệu Khách hàng
                DataTable dtCustomer = this.P5sGetCustomer(salesCDs, date.ToString("yyMMdd"));

                //convert  datatable sang json để xử dụng ở javascript
                String objectCustomer = this.P5sConvertDataTableCustomerJson(dtCustomer);
                String objectTracking = this.P5sConvertDataTableTrackingJson(dt);
                L5sJS.L5sRun("CreatedCTracking('" + objectTracking + "','" + objectCustomer + "')");
            }
            // Comment code 190301
            //switch (P5sDdlViewOption.SelectedValue)
            //{
            //    case "1": // DSR
            //        {
            //            if (salesCDs.Trim().Length > 0)
            //            {
            //                //load dữ liệu tracking
            //                DataTable dt = this.P5sGetTrackingOfSale(salesCDs, yymmdd, timeFrom, timeTo);
            //                if (dt.Rows.Count <= 0)
            //                {
            //                    L5sMsg.Show("No data.");
            //                    return;
            //                }
            //                //load dữ liệu Khách hàng
            //                DataTable dtCustomer = this.P5sGetCustomer(salesCDs, date.ToString("yyMMdd"));

            //                //convert  datatable sang json để xử dụng ở javascript
            //                String objectCustomer = this.P5sConvertDataTableCustomerJson(dtCustomer);
            //                String objectTracking = this.P5sConvertDataTableTrackingJson(dt);
            //                L5sJS.L5sRun("CreatedCTracking('" + objectTracking + "','" + objectCustomer + "')");
            //            }
            //            break;
            //        }
            //    case "2": //CDS
            //        {
            //            if (supervisors.Trim().Length > 0)
            //            {
            //                //load dữ liệu tracking
            //                DataTable dt = this.P5sGetTrackingOfSupervisor(supervisors, yymmdd, timeFrom, timeTo);
            //                if (dt.Rows.Count <= 0)
            //                {
            //                    L5sMsg.Show("No data.");
            //                    return;
            //                }

            //                //load dữ liệu Khách hàng
            //                DataTable dtCustomer = this.P5sGetCustomerBySupervisor(supervisors, date.ToString("yyMMdd"));

            //                //convert  datatable sang json để xử dụng ở javascript
            //                String objectCustomer = this.P5sConvertDataTableCustomerJson(dtCustomer);
            //                String objectTracking = this.P5sConvertDataTableTrackingJson(dt);
            //                L5sJS.L5sRun("CreatedCTracking('" + objectTracking + "','" + objectCustomer + "')");
            //            }

            //            break;
            //        }
            //    case "3": // ASM

            //        if (asms.Trim().Length > 0)
            //        {
            //            //load dữ liệu tracking
            //            DataTable dt = this.P5sGetTrackingOfASM(asms, yymmdd, timeFrom, timeTo);
            //            if (dt.Rows.Count <= 0)
            //            {
            //                L5sMsg.Show("No data.");
            //                return;
            //            }


            //            //load dữ liệu Khách hàng
            //            DataTable dtCustomer = this.P5sGetCustomerByASM(asms, date.ToString("yyMMdd"));

            //            //convert  datatable sang json để xử dụng ở javascript
            //            String objectCustomer = this.P5sConvertDataTableCustomerJson(dtCustomer);
            //            String objectTracking = this.P5sConvertDataTableTrackingJson(dt);
            //            L5sJS.L5sRun("CreatedCTracking('" + objectTracking + "','" + objectCustomer + "')");
            //        }
            //        break;

            //}



        }

        private DataTable P5sGetCustomer(string salesCDs, String yymmdd)
        {
            //hàm lấy danh sách khách hàng theo NVBH bao gồm các thông tin  cơ bản về KH, doanh số, và xác định KH có đc viếng thăm hay không
            //lưu ý: KH đc xác định là viếng thăm nếu như NVBH này ghé thăm (CDS,ASM ghé thăm cũng ko tính)

            DateTime dtime = DateTime.ParseExact(yymmdd, "yyMMdd", null);

            // lệnh này rất qua trọng vì hệ thống đang chốt số theo tuần nền cần phải tạo câu sql động với tham số truyền vào là thời gian để 
            //có thể lấy đúng dữ liệu và đúng bảng

            String sqlGetSqlTimeInOut = P5sCmmFns.P5sGetDynamicSqlTimeInTimeOut(dtime, dtime);
            String sql = String.Format(@"

                                                 DECLARE @TB_CustomerInformation TABLE 
                                                (
	                                                DISTRIBUTOR_CD BIGINT,
                                                    SALES_CD BIGINT,
	                                                SALES_CODE NVARCHAR(128),
	                                                SALES_NAME NVARCHAR(512),
                                                	

	                                                ROUTE_CD BIGINT,
	                                                ROUTE_CODE NVARCHAR(128),
	                                                ROUTE_NAME NVARCHAR(512),

	                                                CUSTOMER_CD BIGINT,
	                                                CUSTOMER_CODE NVARCHAR(128),
	                                                CUSTOMER_NAME NVARCHAR(512),
	                                                CUSTOMER_CHAIN_CODE NVARCHAR(512),
	                                                CUSTOMER_ADDRESS NVARCHAR(512),
                                                    LONGITUDE_LATITUDE NVARCHAR(512),
                                                	
	                                                PRIMARY KEY (CUSTOMER_CD)
                                                )
                                                INSERT INTO @TB_CustomerInformation
                                                SELECT sls.DISTRIBUTOR_CD,sls.SALES_CD,sls.SALES_CODE, sls.SALES_NAME,
		                                                   rout.ROUTE_CD, rout.ROUTE_CODE, rout.ROUTE_NAME,
		                                                   custR.CUSTOMER_CD, cust.CUSTOMER_CODE,cust.CUSTOMER_NAME,  cust.CUSTOMER_CHAIN_CODE, cust.CUSTOMER_ADDRESS ,cust.LONGITUDE_LATITUDE  
		                                                FROM M_SALES sls INNER JOIN O_SALES_ROUTE slsR ON sls.SALES_CD = slsR.SALES_CD AND slsR.ACTIVE = 1 AND sls.ACTIVE = 1
			                                                  INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD AND rout.ACTIVE = 1
			                                                  INNER JOIN O_CUSTOMER_ROUTE custR ON rout.ROUTE_CD = custR.ROUTE_CD AND custR.ACTIVE = 1	
			                                                  INNER JOIN M_CUSTOMER cust ON custR.CUSTOMER_CD = cust.CUSTOMER_CD AND cust.ACTIVE = 1
                                                WHERE  sls.SALES_CD IN ({0}) AND  cust.LONGITUDE_LATITUDE != ''  

                                                
        
                                                 DECLARE @TB_CustomerAmount TABLE 
                                                (
	                                               

	                                                CUSTOMER_CD BIGINT,
	                                                SALES_AMOUNT MONEY,
                                                    SALES_TP MONEY,
                                                    SALES_TB MONEY,
                                                    CUSTOMER_ORDERS INT                                                	
	                                                PRIMARY KEY (CUSTOMER_CD)
                                                )
                                                INSERT INTO @TB_CustomerAmount
                                                SELECT CUSTOMER_CD,SUM(SALES_AMOUNT) AS SALES_AMOUNT,
	                                                SUM(SALES_TP) AS SALES_TP,
	                                                SUM(SALES_TB) AS SALES_TB,
	                                                SUM(CUSTOMER_ORDERS) AS CUSTOMER_ORDERS
                                                FROM O_SALES_AMOUNT
                                                WHERE CAST( CONVERT ( nvarchar(6) , SALES_AMOUNT_DATE, 12 ) AS INT) = {1}   
                                                GROUP BY CUSTOMER_CD

                                               DECLARE @TB_TimeInOut AS TABLE
									           (
											        CUSTOMER_CD BIGINT,
											        TimeIn nvarchar(30),
											        TimeOut nvarchar(30),
											        PRIMARY KEY (CUSTOMER_CD)
									           )


                                                  INSERT INTO @TB_TimeInOut
						                           SELECT  CUSTOMER_CD, CONVERT(varchar, TIME_IN_CREATED_DATE, 108 ) ,  CONVERT(varchar ,TIME_OUT_CREATED_DATE,108) 
                                                   FROM
						                           (
								                           SELECT  CUSTOMER_CD, TIME_IN_CREATED_DATE,TIME_OUT_CREATED_DATE,TIME_IN_LATITUDE_LONGITUDE,TIME_OUT_LATITUDE_LONGITUDE
									                        ,ROW_NUMBER() OVER(PARTITION BY CUSTOMER_CD 	ORDER BY TIME_IN_OUT_CD DESC) AS RN
								                           FROM ({2}) otio
								                           WHERE  CAST( CONVERT ( nvarchar(6) , otio.TIME_IN_CREATED_DATE, 12 ) AS INT) = {1}  
										                           AND otio.TYPE_CD = 1
                                                                   AND CUSTOMER_CD IS NOT NULL
						                           ) AS T
						                           WHERE T.RN = 1



                                        SELECT * 	                                        
	                                        , IS_VISIT =    CASE 
							                                        WHEN ISNULL(T.TimeIn,'') = '' THEN '0' 
							                                        ELSE '1'
					                                        END  
                                                                                    
                                        FROM
                                        (	
                                              SELECT cust.CUSTOMER_CD, cust.CUSTOMER_CODE, cust.CUSTOMER_NAME,  cust.CUSTOMER_ADDRESS , cust.CUSTOMER_CHAIN_CODE,cust.LONGITUDE_LATITUDE,
		                                                    cust.SALES_CD,cust.SALES_CODE, cust.SALES_NAME, cust.SALES_CODE + '-'+ cust.SALES_NAME AS SALES_DESC ,
		                                                    cust.ROUTE_CD,cust.ROUTE_CODE, cust.ROUTE_NAME , cust.ROUTE_CODE + '-' + cust.ROUTE_CODE AS ROUTE_DESC,
		                                                    dist.DISTRIBUTOR_CD, dist.DISTRIBUTOR_CODE, dist.DISTRIBUTOR_NAME  , inout.TimeIn, inout.TimeOut  ,
                                                            FORMAT(CAST( ISNULL(customerAmount.SALES_AMOUNT,0) AS INT) , '##,##0')   AS SALES_AMOUNT,
                                                            FORMAT(CAST( ISNULL(customerAmount.SALES_TP,0) AS INT) , '##,##0')   AS SALES_TP,
                                                            FORMAT(CAST( ISNULL(customerAmount.SALES_TB,0) AS INT) , '##,##0')   AS SALES_TB,
                                                            FORMAT(CAST( ISNULL(customerAmount.CUSTOMER_ORDERS,0) AS INT) , '##,##0')    AS CUSTOMER_ORDERS                                                  
                                                                                          
                                                    FROM @TB_CustomerInformation cust 
                                                         INNER JOIN M_DISTRIBUTOR dist ON cust.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
	                                                     LEFT JOIN @TB_TimeInOut inout ON cust.CUSTOMER_CD = inout.CUSTOMER_CD
                                                         LEFT JOIN @TB_CustomerAmount customerAmount ON cust.CUSTOMER_CD = customerAmount.CUSTOMER_CD
                                                        
                                            
			                                ) AS T", salesCDs, yymmdd, sqlGetSqlTimeInOut);
            return L5sSql.Query(sql);
        }

        private DataTable P5sGetCustomerBySupervisor(string supervisorCDs, String yymmdd)
        {
            //hàm lấy danh sách khách hàng theo NVBH bao gồm các thông tin  cơ bản về KH, doanh số, và xác định KH có đc viếng thăm hay không
            //lưu ý: KH đc xác định là viếng thăm nếu như giám sát này ghé thăm (NVBH,ASM ghé thăm cũng ko tính)
            DateTime dtime = DateTime.ParseExact(yymmdd, "yyMMdd", null);
            // lệnh này rất qua trọng vì hệ thống đang chốt số theo tuần nền cần phải tạo câu sql động với tham số truyền vào là thời gian để 
            //có thể lấy đúng dữ liệu và đúng bảng
            String sqlGetSqlTimeInOut = P5sCmmFns.P5sGetDynamicSqlTimeInTimeOut(dtime, dtime);
            String sql = String.Format(@"



                                                DECLARE @TB_CustomerInformation TABLE 
                                                (
	                                                DISTRIBUTOR_CD BIGINT,
                                                    RSM_CD BIGINT,
	                                                RSM_CODE NVARCHAR(128),
	                                                RSM_NAME NVARCHAR(512),                                                	

	                                                ASM_CD BIGINT,
	                                                ASM_CODE NVARCHAR(128),
	                                                ASM_NAME NVARCHAR(512),

                                                    SUPERVISOR_CD BIGINT,
	                                                SUPERVISOR_CODE NVARCHAR(128),
	                                                SUPERVISOR_NAME NVARCHAR(512),

	                                                CUSTOMER_CD BIGINT,
	                                                CUSTOMER_CODE NVARCHAR(128),
	                                                CUSTOMER_NAME NVARCHAR(512),
	                                                CUSTOMER_CHAIN_CODE NVARCHAR(512),
	                                                CUSTOMER_ADDRESS NVARCHAR(512),

													SUPERVISOR_ROUTE_CD BIGINT,
	                                                SUPERVISOR_ROUTE_CODE NVARCHAR(128),
	                                                SUPERVISOR_ROUTE_NAME NVARCHAR(512),
	                                                LONGITUDE_LATITUDE NVARCHAR(512),
                                                	
	                                                PRIMARY KEY (CUSTOMER_CD)
                                                )
                                                INSERT INTO @TB_CustomerInformation
                                                SELECT   rout.DISTRIBUTOR_CD,
                                                         rsm.RSM_CD,rsm.RSM_CODE, rsm.RSM_NAME,
                                                         asm.ASM_CD,asm.ASM_CODE, asm.ASM_NAME,
                                                         sup.SUPERVISOR_CD,sup.SUPERVISOR_CODE, sup.SUPERVISOR_NAME,
                                                         custR.CUSTOMER_CD, cust.CUSTOMER_CODE,cust.CUSTOMER_NAME,  cust.CUSTOMER_CHAIN_CODE, cust.CUSTOMER_ADDRESS   ,
														 rout.SUPERVISOR_ROUTE_CD,rout.SUPERVISOR_ROUTE_CODE,rout.SUPERVISOR_ROUTE_NAME ,cust.LONGITUDE_LATITUDE

		                                                FROM  M_SUPERVISOR sup INNER JOIN O_SUPERVISOR_SUPERVISOR_ROUTE supR ON sup.SUPERVISOR_CD = supR.SUPERVISOR_CD AND supR.ACTIVE = 1
			                                                 INNER JOIN M_SUPERVISOR_ROUTE rout ON supR.SUPERVISOR_ROUTE_CD = rout.SUPERVISOR_ROUTE_CD AND rout.ACTIVE = 1
				                                             INNER JOIN O_CUSTOMER_SUPERVISOR_ROUTE custR ON rout.SUPERVISOR_ROUTE_CD = custR.SUPERVISOR_ROUTE_CD AND custR.ACTIVE = 1	
				                                             INNER JOIN M_CUSTOMER cust ON custR.CUSTOMER_CD = cust.CUSTOMER_CD AND cust.ACTIVE = 1
				                                             INNER JOIN M_ASM asm ON sup.ASM_CD = asm.ASM_CD
				                                             INNER JOIN M_RSM rsm ON asm.RSM_CD = rsm.RSM_CD
                                               WHERE sup.SUPERVISOR_CD IN ({0}) AND  cust.LONGITUDE_LATITUDE != '' 



                                                

                                               DECLARE @TB_TimeInOut AS TABLE
									           (
											        CUSTOMER_CD BIGINT,
											        TimeIn nvarchar(30),
											        TimeOut nvarchar(30),
											        PRIMARY KEY (CUSTOMER_CD)
									           )


                                                    DECLARE @DT datetime
	                                                SET @DT = EOMONTH (DATEADD(MONTH,-2,GETDATE()))
	                                                DECLARE @SysYYMMDD int
	                                                DECLARE @SysYYYYMM int
	                                                SET @SysYYMMDD = CAST( CONVERT ( nvarchar(6) , @DT, 12 )   AS INT)
	                                                SET @SysYYYYMM = YEAR(@DT)*100 + MONTH(@DT)

                                                    
                                                   INSERT INTO @TB_TimeInOut
						                           SELECT  CUSTOMER_CD, CONVERT(varchar, TIME_IN_CREATED_DATE, 108 ) ,  CONVERT(varchar ,TIME_OUT_CREATED_DATE,108) 
                                                   FROM
						                           (
								                           SELECT  CUSTOMER_CD, TIME_IN_CREATED_DATE,TIME_OUT_CREATED_DATE,TIME_IN_LATITUDE_LONGITUDE,TIME_OUT_LATITUDE_LONGITUDE
									                        ,ROW_NUMBER() OVER(PARTITION BY CUSTOMER_CD 	ORDER BY TIME_IN_OUT_CD DESC) AS RN
								                           FROM ({2}) otio
								                           WHERE  CAST( CONVERT ( nvarchar(6) , otio.TIME_IN_CREATED_DATE, 12 ) AS INT) = {1}  
										                           AND otio.TYPE_CD = 2
                                                                   AND CUSTOMER_CD IS NOT NULL
						                           ) AS T
						                           WHERE T.RN = 1
                                                





                                                SELECT * 
	                                                , CUSTOMER_SALES_AMOUNT = 
					                                                CASE 
							                                                WHEN ISNULL(T.TimeIn,'') = '' THEN '0' 
							                                                ELSE FORMAT( CAST(RAND(CHECKSUM(NEWID())) * 10000000 AS INT) , 'N1') 
					                                                END  -- DOANH SỐ THEO NGÀY CỦA CUSTOMER TẠM THỜI RANDOME GIÁ TRỊ
	                                                , IS_VISIT =    CASE 
							                                                WHEN ISNULL(T.TimeIn,'') = '' THEN '0' 
							                                                ELSE '1'
					                                                END 
                                                 ,
                                                  SALES_AMOUNT = 0,
                                                  SALES_TP = 0,
                                                  SALES_TB = 0,
                                                  CUSTOMER_ORDERS = 0

                                                                                                                                    
                                                FROM
                                                (	
                                                SELECT info.CUSTOMER_CD, info.CUSTOMER_CODE, info.CUSTOMER_NAME,  info.CUSTOMER_ADDRESS , info.CUSTOMER_CHAIN_CODE,info.LONGITUDE_LATITUDE,
		                                               info.SUPERVISOR_CD AS SALES_CD,
				                                       info.SUPERVISOR_CODE AS  SALES_CODE, 
				                                       info.SUPERVISOR_NAME AS SALES_NAME,
				                                       info.SUPERVISOR_NAME AS SALES_DESC ,
		                                               info.SUPERVISOR_ROUTE_CD AS ROUTE_CD,
				                                       info.SUPERVISOR_ROUTE_CODE AS ROUTE_CODE, 
				                                       info.SUPERVISOR_ROUTE_NAME AS ROUTE_NAME ,
				                                       info.SUPERVISOR_ROUTE_CODE + '-' + info.SUPERVISOR_ROUTE_NAME AS ROUTE_DESC,
		                                               dist.DISTRIBUTOR_CD, dist.DISTRIBUTOR_CODE, dist.DISTRIBUTOR_NAME,inout.TimeIn ,inout.TimeOut                                                               
                                                                                                                              
                                                FROM  @TB_CustomerInformation info INNER JOIN M_DISTRIBUTOR dist ON info.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
                                                       LEFT JOIN @TB_TimeInOut inout ON info.CUSTOMER_CD = inout.CUSTOMER_CD
                                                        
                                       
			) AS T", supervisorCDs, yymmdd, sqlGetSqlTimeInOut);
            return L5sSql.Query(sql);
        }



        private DataTable P5sGetCustomerByASM(string ASMCDs, String yymmdd)
        {
            //hàm lấy danh sách khách hàng theo NVBH bao gồm các thông tin  cơ bản về KH, doanh số, và xác định KH có đc viếng thăm hay không
            //lưu ý: KH đc xác định là viếng thăm nếu như ASM này ghé thăm (NVBH,CDS ghé thăm cũng ko tính)
            DateTime dtime = DateTime.ParseExact(yymmdd, "yyMMdd", null);

            // lệnh này rất qua trọng vì hệ thống đang chốt số theo tuần nền cần phải tạo câu sql động với tham số truyền vào là thời gian để 
            //có thể lấy đúng dữ liệu và đúng bảng            
            String sqlGetSqlTimeInOut = P5sCmmFns.P5sGetDynamicSqlTimeInTimeOut(dtime, dtime);
            String sql = String.Format(@"


                                                DECLARE @TB_CustomerInformation TABLE 
                                                (
	                                                DISTRIBUTOR_CD BIGINT,
                                                    RSM_CD BIGINT,
	                                                RSM_CODE NVARCHAR(128),
	                                                RSM_NAME NVARCHAR(512),                                                	
													
                                                    ASM_CD BIGINT,
	                                                ASM_CODE NVARCHAR(128),
	                                                ASM_NAME NVARCHAR(512),

	                                                CUSTOMER_CD BIGINT,
	                                                CUSTOMER_CODE NVARCHAR(128),
	                                                CUSTOMER_NAME NVARCHAR(512),
	                                                CUSTOMER_CHAIN_CODE NVARCHAR(512),
	                                                CUSTOMER_ADDRESS NVARCHAR(512),

													ASM_ROUTE_CD BIGINT,
	                                                ASM_ROUTE_CODE NVARCHAR(128),
	                                                ASM_ROUTE_NAME NVARCHAR(512),
	                                                LONGITUDE_LATITUDE NVARCHAR(512),
                                                	
	                                                PRIMARY KEY (CUSTOMER_CD)
                                                )
                                                INSERT INTO @TB_CustomerInformation
                                                SELECT   rout.DISTRIBUTOR_CD,
                                                         rsm.RSM_CD,rsm.RSM_CODE, rsm.RSM_NAME,
                                                         sup.ASM_CD,sup.ASM_CODE, sup.ASM_NAME,
                                                         custR.CUSTOMER_CD, cust.CUSTOMER_CODE,cust.CUSTOMER_NAME,  cust.CUSTOMER_CHAIN_CODE, cust.CUSTOMER_ADDRESS   ,
														 rout.ASM_ROUTE_CD,rout.ASM_ROUTE_CODE,rout.ASM_ROUTE_NAME ,cust.LONGITUDE_LATITUDE

		                                                FROM  M_ASM sup INNER JOIN O_ASM_ASM_ROUTE supR ON sup.ASM_CD = supR.ASM_CD AND supR.ACTIVE = 1
			                                                 INNER JOIN M_ASM_ROUTE rout ON supR.ASM_ROUTE_CD = rout.ASM_ROUTE_CD AND rout.ACTIVE = 1
				                                             INNER JOIN O_CUSTOMER_ASM_ROUTE custR ON rout.ASM_ROUTE_CD = custR.ASM_ROUTE_CD AND custR.ACTIVE = 1	
				                                             INNER JOIN M_CUSTOMER cust ON custR.CUSTOMER_CD = cust.CUSTOMER_CD AND cust.ACTIVE = 1
				                                             INNER JOIN M_ASM asm ON sup.ASM_CD = asm.ASM_CD
				                                             INNER JOIN M_RSM rsm ON asm.RSM_CD = rsm.RSM_CD
                                              WHERE sup.ASM_CD IN ({0}) AND  cust.LONGITUDE_LATITUDE != '' 


                                                

                                               DECLARE @TB_TimeInOut AS TABLE
									           (
											        CUSTOMER_CD BIGINT,
											        TimeIn nvarchar(30),
											        TimeOut nvarchar(30),
											        PRIMARY KEY (CUSTOMER_CD)
									           )


                                                    DECLARE @DT datetime
	                                                SET @DT = EOMONTH (DATEADD(MONTH,-2,GETDATE()))
	                                                DECLARE @SysYYMMDD int
	                                                DECLARE @SysYYYYMM int
	                                                SET @SysYYMMDD = CAST( CONVERT ( nvarchar(6) , @DT, 12 )   AS INT)
	                                                SET @SysYYYYMM = YEAR(@DT)*100 + MONTH(@DT)

                                                    
                                                     INSERT INTO @TB_TimeInOut
							                           SELECT  CUSTOMER_CD, CONVERT(varchar, TIME_IN_CREATED_DATE, 108 ) ,  CONVERT(varchar ,TIME_OUT_CREATED_DATE,108) 
                                                       FROM
							                           (
									                           SELECT  CUSTOMER_CD, TIME_IN_CREATED_DATE,TIME_OUT_CREATED_DATE,TIME_IN_LATITUDE_LONGITUDE,TIME_OUT_LATITUDE_LONGITUDE
										                        ,ROW_NUMBER() OVER(PARTITION BY CUSTOMER_CD 	ORDER BY TIME_IN_OUT_CD DESC) AS RN
									                           FROM ({2}) otio
									                           WHERE  CAST( CONVERT ( nvarchar(6) , otio.TIME_IN_CREATED_DATE, 12 ) AS INT) = {1}  
											                           AND otio.TYPE_CD = 3
                                                                       AND CUSTOMER_CD IS NOT NULL
							                           ) AS T
							                           WHERE T.RN = 1
                                                





                                             SELECT *
	                                                , IS_VISIT =    CASE 
							                                                WHEN ISNULL(T.TimeIn,'') = '' THEN '0' 
							                                                ELSE '1'
					                                                END  
                                                  ,
                                                  SALES_AMOUNT = 0,
                                                  SALES_TP = 0,
                                                  SALES_TB = 0,
                                                  CUSTOMER_ORDERS = 0

                                                                                                                                    
                                                FROM
                                                (	
                                                SELECT info.CUSTOMER_CD, info.CUSTOMER_CODE, info.CUSTOMER_NAME,  info.CUSTOMER_ADDRESS , info.CUSTOMER_CHAIN_CODE,info.LONGITUDE_LATITUDE,
		                                               info.ASM_CD AS SALES_CD,
				                                       info.ASM_CODE AS  SALES_CODE, 
				                                       info.ASM_NAME AS SALES_NAME,
				                                       info.ASM_NAME AS SALES_DESC ,
		                                               info.ASM_ROUTE_CD AS ROUTE_CD,
				                                       info.ASM_ROUTE_CODE AS ROUTE_CODE, 
				                                       info.ASM_ROUTE_NAME AS ROUTE_NAME ,
				                                       info.ASM_ROUTE_CODE + '-' + info.ASM_ROUTE_NAME AS ROUTE_DESC,
		                                               dist.DISTRIBUTOR_CD, dist.DISTRIBUTOR_CODE, dist.DISTRIBUTOR_NAME,inout.TimeIn ,inout.TimeOut                                                               
                                                                                                                              
                                                FROM  @TB_CustomerInformation info INNER JOIN M_DISTRIBUTOR dist ON info.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
                                                       LEFT JOIN @TB_TimeInOut inout ON info.CUSTOMER_CD = inout.CUSTOMER_CD


			                                        ) AS T", ASMCDs, yymmdd, sqlGetSqlTimeInOut);
            return L5sSql.Query(sql);
        }



        #region GET TRACKING OF SALES

        private DataTable P5sGetTrackingOfSale(String salesCD, String yymmdd, String timeFrom, String timeEnd)
        {
            // lệnh này rất qua trọng vì hệ thống đang chốt số theo tuần nền cần phải tạo câu sql động với tham số truyền vào là thời gian để 
            //có thể lấy đúng dữ liệu và đúng bảng
            String sqlGetTrackingOfSales = P5sCmmFns.P5sGetDynamicSqlTrackingOfSales(DateTime.ParseExact(yymmdd, "yyMMdd", null), DateTime.ParseExact(yymmdd, "yyMMdd", null));
            if (sqlGetTrackingOfSales.Equals("-1"))
            {
                return null;
            }


            String sql = String.Format(@"
                                            ---------------------------------------------GET TRACKING OF SALES------------------------------------------------
												
												-- < Khai bao Timezone cho khu vuc philippin thi cong them 1 hour--

												DECLARE @TIMEZONE BIGINT

												select @TIMEZONE = ISNULL(value,0)
												from S_PARAMS
												where NAME = 'TIME_ZONE'
													
												--Khai bao Timezone cho khu vuc philippin thi cong them 1 hour />--	

												----------------------------------------------------------------------------------------------

                                            DECLARE @TB_Tracking  AS TABLE
                                            (
	                                            [YYMMDD] [int] NULL,
	                                            [SALES_CD] [bigint] NULL,
	                                            [DISTRIBUTOR_CD] [bigint] NULL,
	                                            [LONGITUDE_LATITUDE] [nvarchar](255) NULL,
	                                            [DEVICE_STATUS] [nvarchar](max) NULL,
	                                            [NO_REPEAT] [int] NULL,
	                                            [BEGIN_DATETIME] [datetime] NULL,
	                                            [END_DATETIME] [datetime] NULL,
	                                            [BATTERY_PERCENTAGE] [int] NULL,
	                                            [TYPE_TRACKING] [nvarchar](50) NULL,
	                                            [POINT_RADIUS] [nvarchar](50) NULL,
	                                            [ANGEL] [float] NULL,
	                                            [TRACKING_ACCURACY] [float] NULL,
	                                            [BATTERY_PERCENTAGE_VALUE] [nvarchar](max) NULL,
	                                            [BATTERY_PERCENTAGE_DATETIME] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER_VALUE] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER_DATETIME] [nvarchar](max) NULL,
	                                            [CREATED_DATE] [datetime] NULL,
	                                            [LOCATION_ADDRESS] [nvarchar](512) NULL,
                                                [LAST_UPDATE] [datetime] NULL
                                            )

                                            DECLARE @TB_Tracking_Max_END_DATETIME  AS TABLE
                                            (	                                          
	                                            [SALES_CD] [bigint] ,	                                            
                                                [END_DATETIME] [datetime] NULL,
                                                PRIMARY KEY (SALES_CD)
                                            )

                                            DECLARE @TB_Tracking_Max_CREATED_DATE  AS TABLE
                                            (	                                          
	                                            [SALES_CD] [bigint] ,	                                            
                                                [CREATED_DATE] [datetime] NULL,
                                                PRIMARY KEY (SALES_CD)
                                            )

                                     
                                            INSERT INTO @TB_Tracking
                                            SELECT [YYMMDD]
                                                  ,[SALES_CD]
                                                  ,[DISTRIBUTOR_CD]
                                                  ,[LONGITUDE_LATITUDE]
                                                  ,[DEVICE_STATUS]
                                                  ,[NO_REPEAT]
                                                  ,[BEGIN_DATETIME]
                                                  ,[END_DATETIME]
                                                  ,[BATTERY_PERCENTAGE]
                                                  ,[TYPE_TRACKING]
                                                  ,[POINT_RADIUS]
                                                  ,[ANGEL]
                                                  ,[TRACKING_ACCURACY]
                                                  ,[BATTERY_PERCENTAGE_VALUE]
                                                  ,[BATTERY_PERCENTAGE_DATETIME]
                                                  ,[TRACKING_PROVIDER]
                                                  ,[TRACKING_PROVIDER_VALUE]
                                                  ,[TRACKING_PROVIDER_DATETIME]
                                                  ,DATEADD(HOUR,@TIMEZONE,CREATED_DATE) as [CREATED_DATE]
												  ,[LOCATION_ADDRESS]
												  ,DATEADD(HOUR,@TIMEZONE,CREATED_DATE) as [LAST_UPDATE]                                  
                                            FROM ({4}) AS T 
                                            WHERE   SALES_CD IN ({0})  AND  YYMMDD = {1} 
														AND
                                                        (
                                                            ( CONVERT(TIME, END_DATETIME) >= '{2}' 
                                                               AND CONVERT(TIME, END_DATETIME) <= '{3}'
                                                             ) 

                                                            OR

                                                            ( CONVERT(TIME, BEGIN_DATETIME) >= '{2}'
                                                               AND CONVERT(TIME, BEGIN_DATETIME) <= '{3}'
                                                            ) 
                                                        )

                                           
                                            INSERT INTO @TB_Tracking_Max_END_DATETIME
                                            SELECT SALES_CD,MAX(END_DATETIME)  FROM @TB_Tracking
                                            GROUP BY SALES_CD

                                            INSERT INTO @TB_Tracking_Max_CREATED_DATE
                                            SELECT SALES_CD,MAX(CREATED_DATE)  FROM @TB_Tracking
                                            GROUP BY SALES_CD
                                                 
                                            --cập nhật thời gian lần cập nhật cuối cùng để hiển thị lên bản đồ
                                            UPDATE T SET  LAST_UPDATE = 
                                                                         CASE 
                                                                               WHEN T1.END_DATETIME > T2.CREATED_DATE THEN  T2.CREATED_DATE
                                                                               ELSE T1.END_DATETIME
                                                                         END
                                                                    
                                            FROM @TB_Tracking AS T INNER JOIN @TB_Tracking_Max_END_DATETIME AS T1 ON T.SALES_CD = T1.SALES_CD
                                                                  INNER JOIN @TB_Tracking_Max_CREATED_DATE AS T2 ON T1.SALES_CD = T2.SALES_CD
                                
                                            

                                                                               
                                    

                                            SELECT     sls.SALES_CD, sls.SALES_CODE, 'DSR: '+ sls.SALES_NAME as SALES_NAME,	                                       
                                                       dis.DISTRIBUTOR_CD, dis.DISTRIBUTOR_CODE, dis.DISTRIBUTOR_NAME,
                                                       tracking.LONGITUDE_LATITUDE, 
                                                       tracking.DEVICE_STATUS, 
                                                       tracking.NO_REPEAT, 
                                                       tracking.BEGIN_DATETIME, 
                                                       tracking.END_DATETIME, 
                                                       tracking.BATTERY_PERCENTAGE, 
                                                       --tracking.TYPE_TRACKING, 
                                                       tracking.POINT_RADIUS, 
                                                       tracking.TRACKING_ACCURACY, 
                                                       tracking.ANGEL, 
                                                       tracking.BATTERY_PERCENTAGE_VALUE, 
                                                       tracking.BATTERY_PERCENTAGE_DATETIME, 
                                                       tracking.TRACKING_PROVIDER, 
                                                       tracking.TRACKING_PROVIDER_VALUE, 
                                                       tracking.TRACKING_PROVIDER_DATETIME,
                                                       tracking.LAST_UPDATE,	
                                                       CASE
															WHEN tracking.BEGIN_DATETIME = (SELECT MIN(A.BEGIN_DATETIME) FROM @TB_Tracking A WHERE A.SALES_CD=tracking.SALES_CD) 
															THEN 'S'
															WHEN tracking.BEGIN_DATETIME = (SELECT MAX(A.BEGIN_DATETIME) FROM @TB_Tracking A WHERE A.SALES_CD=tracking.SALES_CD) 
															THEN 'E'
															WHEN tracking.BEGIN_DATETIME != (SELECT MAX(A.BEGIN_DATETIME) FROM @TB_Tracking A WHERE A.SALES_CD=tracking.SALES_CD) AND tracking.BEGIN_DATETIME != (SELECT MIN(A.BEGIN_DATETIME) FROM @TB_Tracking A 
															WHERE A.SALES_CD=tracking.SALES_CD) THEN 'P'
													   END AS TYPE_TRACKING
                                	               
                                            FROM 
                                                    @TB_Tracking tracking INNER JOIN  M_SALES sls ON tracking.SALES_CD = sls.SALES_CD
                                                                          INNER JOIN M_DISTRIBUTOR dis ON sls.DISTRIBUTOR_CD = dis.DISTRIBUTOR_CD  
                                             ORDER BY sls.SALES_CD,tracking.BEGIN_DATETIME
                                            
                                        ", salesCD, yymmdd, timeFrom, timeEnd, sqlGetTrackingOfSales);

            return L5sSql.Query(sql);

        }

        #endregion

        #region GET TRACKING OF SUPERVISOR

        private DataTable P5sGetTrackingOfSupervisor(String supervisor, String yymmdd, String timeFrom, String timeEnd)
        {
            // lệnh này rất qua trọng vì hệ thống đang chốt số theo tuần nền cần phải tạo câu sql động với tham số truyền vào là thời gian để 
            //có thể lấy đúng dữ liệu và đúng bảng
            String sqlGetTrackingOfSupervisor = P5sCmmFns.P5sGetDynamicSqlTrackingOfSupervisor(DateTime.ParseExact(yymmdd, "yyMMdd", null), DateTime.ParseExact(yymmdd, "yyMMdd", null));
            if (sqlGetTrackingOfSupervisor.Equals("-1"))
            {
                return null;
            }


            String sql = String.Format(@"
                                                --------------------------------------------GET TRACKING OF SUPERVISOR-------------------------------------------------
												
												-- < Khai bao Timezone cho khu vuc philippin thi cong them 1 hour--

												DECLARE @TIMEZONE BIGINT

												select @TIMEZONE = ISNULL(value,0)
												from S_PARAMS
												where NAME = 'TIME_ZONE'
													
												--Khai bao Timezone cho khu vuc philippin thi cong them 1 hour />--	

												----------------------------------------------------------------------------------------------   
                                               DECLARE @TB_Tracking  AS TABLE
                                            (
	                                            [YYMMDD] [int] NULL,
	                                            [SUPERVISOR_CD] [bigint] NULL,
	                                            [DISTRIBUTOR_CD] [bigint] NULL,
	                                            [LONGITUDE_LATITUDE] [nvarchar](255) NULL,
	                                            [DEVICE_STATUS] [nvarchar](max) NULL,
	                                            [NO_REPEAT] [int] NULL,
	                                            [BEGIN_DATETIME] [datetime] NULL,
	                                            [END_DATETIME] [datetime] NULL,
	                                            [BATTERY_PERCENTAGE] [int] NULL,
	                                            [TYPE_TRACKING] [nvarchar](50) NULL,
	                                            [POINT_RADIUS] [nvarchar](50) NULL,
	                                            [ANGEL] [float] NULL,
	                                            [TRACKING_ACCURACY] [float] NULL,
	                                            [BATTERY_PERCENTAGE_VALUE] [nvarchar](max) NULL,
	                                            [BATTERY_PERCENTAGE_DATETIME] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER_VALUE] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER_DATETIME] [nvarchar](max) NULL,
	                                            [CREATED_DATE] [datetime] NULL,
	                                            [LOCATION_ADDRESS] [nvarchar](512) NULL,
                                                [LAST_UPDATE] [datetime] NULL
                                            )

                                            DECLARE @TB_Tracking_Max_END_DATETIME  AS TABLE
                                            (	                                          
	                                            [SUPERVISOR_CD] [bigint] ,	                                            
                                                [END_DATETIME] [datetime] NULL,
                                                PRIMARY KEY (SUPERVISOR_CD)
                                            )

                                            DECLARE @TB_Tracking_Max_CREATED_DATE  AS TABLE
                                            (	                                          
	                                            [SUPERVISOR_CD] [bigint] ,	                                            
                                                [CREATED_DATE] [datetime] NULL,
                                                PRIMARY KEY (SUPERVISOR_CD)
                                            )

                                             INSERT INTO @TB_Tracking
                                                SELECT [YYMMDD]
                                                      ,[SUPERVISOR_CD]
                                                      ,[DISTRIBUTOR_CD]
                                                      ,[LONGITUDE_LATITUDE]
                                                      ,[DEVICE_STATUS]
                                                      ,[NO_REPEAT]
                                                      ,[BEGIN_DATETIME]
                                                      ,[END_DATETIME]
                                                      ,[BATTERY_PERCENTAGE]
                                                      ,[TYPE_TRACKING]
                                                      ,[POINT_RADIUS]
                                                      ,[ANGEL]
                                                      ,[TRACKING_ACCURACY]
                                                      ,[BATTERY_PERCENTAGE_VALUE]
                                                      ,[BATTERY_PERCENTAGE_DATETIME]
                                                      ,[TRACKING_PROVIDER]
                                                      ,[TRACKING_PROVIDER_VALUE]
                                                      ,[TRACKING_PROVIDER_DATETIME]
                                                      ,DATEADD(HOUR,@TIMEZONE,CREATED_DATE) as [CREATED_DATE]
												      ,[LOCATION_ADDRESS]
												      ,DATEADD(HOUR,@TIMEZONE,CREATED_DATE) as [LAST_UPDATE]                                  
                                                FROM   ({4}) AS T
                                                WHERE   SUPERVISOR_CD IN ({0})  AND  YYMMDD = {1} 
														AND
                                                        (
                                                            ( CONVERT(TIME, END_DATETIME) >= '{2}' 
                                                               AND CONVERT(TIME, END_DATETIME) <= '{3}'
                                                             ) 

                                                            OR

                                                            ( CONVERT(TIME, BEGIN_DATETIME) >= '{2}'
                                                               AND CONVERT(TIME, BEGIN_DATETIME) <= '{3}'
                                                            ) 
                                                        )


                                            INSERT INTO @TB_Tracking_Max_END_DATETIME
                                            SELECT SUPERVISOR_CD,MAX(END_DATETIME)  FROM @TB_Tracking
                                            GROUP BY SUPERVISOR_CD

                                            INSERT INTO @TB_Tracking_Max_CREATED_DATE
                                            SELECT SUPERVISOR_CD,MAX(CREATED_DATE)  FROM @TB_Tracking
                                            GROUP BY SUPERVISOR_CD


                                            UPDATE T SET  LAST_UPDATE = 
                                                                         CASE 
                                                                               WHEN T1.END_DATETIME > T2.CREATED_DATE THEN  T2.CREATED_DATE
                                                                               ELSE T1.END_DATETIME
                                                                         END
                                                                    
                                            FROM @TB_Tracking AS T INNER JOIN @TB_Tracking_Max_END_DATETIME AS T1 ON T.SUPERVISOR_CD = T1.SUPERVISOR_CD
                                                                  INNER JOIN @TB_Tracking_Max_CREATED_DATE AS T2 ON T1.SUPERVISOR_CD = T2.SUPERVISOR_CD
                                
                                            



                                        SELECT    sup.SUPERVISOR_CD AS SALES_CD -- set name là SALES_CD để không phải chỉnh sửa code ở json và javascript
                                                    ,sup.SUPERVISOR_CODE AS SALES_CODE,
                                                    'CDS: '+ sup.SUPERVISOR_NAME AS SALES_NAME,
	                                                '' AS DISTRIBUTOR_CD,'RSM[' + rsm.RSM_NAME  +']'  AS DISTRIBUTOR_CODE, 'ASM[' + asm.ASM_NAME  +']'   AS DISTRIBUTOR_NAME,
                                                       tracking.LONGITUDE_LATITUDE, 
                                                       tracking.DEVICE_STATUS, 
                                                       tracking.NO_REPEAT, 
                                                       tracking.BEGIN_DATETIME, 
                                                       tracking.END_DATETIME, 
                                                       tracking.BATTERY_PERCENTAGE, 
                                                       tracking.TYPE_TRACKING, 
                                                       tracking.POINT_RADIUS, 
                                                       tracking.TRACKING_ACCURACY, 
                                                       tracking.ANGEL, 
                                                       tracking.BATTERY_PERCENTAGE_VALUE, 
                                                       tracking.BATTERY_PERCENTAGE_DATETIME, 
                                                       tracking.TRACKING_PROVIDER, 
                                                       tracking.TRACKING_PROVIDER_VALUE, 
                                                       tracking.TRACKING_PROVIDER_DATETIME,
                                                       tracking.LAST_UPDATE,
													CASE
															WHEN tracking.BEGIN_DATETIME = (SELECT MIN(A.BEGIN_DATETIME) FROM @TB_Tracking A WHERE A.[SUPERVISOR_CD]=tracking.[SUPERVISOR_CD]) 
															THEN 'S'
															WHEN tracking.BEGIN_DATETIME = (SELECT MAX(A.BEGIN_DATETIME) FROM @TB_Tracking A WHERE A.[SUPERVISOR_CD]=tracking.[SUPERVISOR_CD]) 
															THEN 'E'
															WHEN tracking.BEGIN_DATETIME != (SELECT MAX(A.BEGIN_DATETIME) FROM @TB_Tracking A WHERE A.[SUPERVISOR_CD]=tracking.[SUPERVISOR_CD]) AND tracking.BEGIN_DATETIME != (SELECT MIN(A.BEGIN_DATETIME) FROM @TB_Tracking A 
															WHERE A.[SUPERVISOR_CD]=tracking.[SUPERVISOR_CD]) THEN 'P'
													   END AS TYPE_TRACKING

                                	               
                                            FROM @TB_Tracking tracking INNER JOIN M_SUPERVISOR sup ON   sup.SUPERVISOR_CD = tracking.SUPERVISOR_CD   
                                                INNER JOIN M_ASM asm ON sup.ASM_CD = asm.ASM_CD 
                                                INNER JOIN M_RSM rsm ON asm.RSM_CD = rsm.RSM_CD                                                   
                                             ORDER BY sup.SUPERVISOR_CODE,tracking.BEGIN_DATETIME

                                        ", supervisor, yymmdd, timeFrom, timeEnd, sqlGetTrackingOfSupervisor);



            return L5sSql.Query(sql);

        }


        #endregion

        #region GET TRACKING OF ASM

        private DataTable P5sGetTrackingOfASM(String ASM, String yymmdd, String timeFrom, String timeEnd)
        {
            // lệnh này rất qua trọng vì hệ thống đang chốt số theo tuần nền cần phải tạo câu sql động với tham số truyền vào là thời gian để 
            //có thể lấy đúng dữ liệu và đúng bảng
            String sqlGetTrackingOfASM = P5sCmmFns.P5sGetDynamicSqlTrackingOfASM(DateTime.ParseExact(yymmdd, "yyMMdd", null), DateTime.ParseExact(yymmdd, "yyMMdd", null));
            if (sqlGetTrackingOfASM.Equals("-1"))
            {
                return null;
            }

            String sql = String.Format(@"
                                            ----------------------------------------------GET TRACKING OF ASM-----------------------------------------------
												
												-- < Khai bao Timezone cho khu vuc philippin thi cong them 1 hour--

												DECLARE @TIMEZONE BIGINT

												select @TIMEZONE = ISNULL(value,0)
												from S_PARAMS
												where NAME = 'TIME_ZONE'
													
												--Khai bao Timezone cho khu vuc philippin thi cong them 1 hour />--	

												----------------------------------------------------------------------------------------------
                                        
                                            DECLARE @TB_Tracking  AS TABLE
                                            (
	                                            [YYMMDD] [int] NULL,
	                                            [ASM_CD] [bigint] NULL,
	                                            [DISTRIBUTOR_CD] [bigint] NULL,
	                                            [LONGITUDE_LATITUDE] [nvarchar](255) NULL,
	                                            [DEVICE_STATUS] [nvarchar](max) NULL,
	                                            [NO_REPEAT] [int] NULL,
	                                            [BEGIN_DATETIME] [datetime] NULL,
	                                            [END_DATETIME] [datetime] NULL,
	                                            [BATTERY_PERCENTAGE] [int] NULL,
	                                            [TYPE_TRACKING] [nvarchar](50) NULL,
	                                            [POINT_RADIUS] [nvarchar](50) NULL,
	                                            [ANGEL] [float] NULL,
	                                            [TRACKING_ACCURACY] [float] NULL,
	                                            [BATTERY_PERCENTAGE_VALUE] [nvarchar](max) NULL,
	                                            [BATTERY_PERCENTAGE_DATETIME] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER_VALUE] [nvarchar](max) NULL,
	                                            [TRACKING_PROVIDER_DATETIME] [nvarchar](max) NULL,
	                                            [CREATED_DATE] [datetime] NULL,
	                                            [LOCATION_ADDRESS] [nvarchar](512) NULL,
                                                [LAST_UPDATE] [datetime] NULL
                                            )


                                            DECLARE @TB_Tracking_Max_END_DATETIME  AS TABLE
                                            (	                                          
	                                            [ASM_CD] [bigint] ,	                                            
                                                [END_DATETIME] [datetime] NULL,
                                                PRIMARY KEY (ASM_CD)
                                            )

                                            DECLARE @TB_Tracking_Max_CREATED_DATE  AS TABLE
                                            (	                                          
	                                            [ASM_CD] [bigint] ,	                                            
                                                [CREATED_DATE] [datetime] NULL,
                                                PRIMARY KEY (ASM_CD)
                                            )


                                               INSERT INTO @TB_Tracking
                                                SELECT [YYMMDD]
                                                      ,[ASM_CD]
                                                      ,[DISTRIBUTOR_CD]
                                                      ,[LONGITUDE_LATITUDE]
                                                      ,[DEVICE_STATUS]
                                                      ,[NO_REPEAT]
                                                      ,[BEGIN_DATETIME]
                                                      ,[END_DATETIME]
                                                      ,[BATTERY_PERCENTAGE]
                                                      ,[TYPE_TRACKING]
                                                      ,[POINT_RADIUS]
                                                      ,[ANGEL]
                                                      ,[TRACKING_ACCURACY]
                                                      ,[BATTERY_PERCENTAGE_VALUE]
                                                      ,[BATTERY_PERCENTAGE_DATETIME]
                                                      ,[TRACKING_PROVIDER]
                                                      ,[TRACKING_PROVIDER_VALUE]
                                                      ,[TRACKING_PROVIDER_DATETIME]
                                                      ,DATEADD(HOUR,@TIMEZONE,CREATED_DATE) as [CREATED_DATE]
												      ,[LOCATION_ADDRESS]
												      ,DATEADD(HOUR,@TIMEZONE,CREATED_DATE) as [LAST_UPDATE]                                  
                                                FROM   ({4}) AS T
                                                WHERE   ASM_CD IN ({0})  AND  YYMMDD = {1} 


                                            INSERT INTO @TB_Tracking_Max_END_DATETIME
                                            SELECT ASM_CD,MAX(END_DATETIME)  FROM @TB_Tracking
                                            GROUP BY ASM_CD

                                            INSERT INTO @TB_Tracking_Max_CREATED_DATE
                                            SELECT ASM_CD,MAX(CREATED_DATE)  FROM @TB_Tracking
                                            GROUP BY ASM_CD

                                            UPDATE T SET  LAST_UPDATE = 
                                                                         CASE 
                                                                               WHEN T1.END_DATETIME > T2.CREATED_DATE THEN  T2.CREATED_DATE
                                                                               ELSE T1.END_DATETIME
                                                                         END
                                                                    
                                            FROM @TB_Tracking AS T INNER JOIN @TB_Tracking_Max_END_DATETIME AS T1 ON T.ASM_CD = T1.ASM_CD
                                                                  INNER JOIN @TB_Tracking_Max_CREATED_DATE AS T2 ON T1.ASM_CD = T2.ASM_CD
                                
                                            




                                 SELECT  asm.ASM_CD AS SALES_CD -- set name là SALES_CD để không phải chỉnh sửa code ở json và javascript
                                                    ,asm.ASM_CODE AS SALES_CODE,
                                                    'ASM: '+ asm.ASM_NAME AS SALES_NAME,
	                                                '' AS DISTRIBUTOR_CD,'RSM[' + rsm.RSM_NAME  +']'  AS DISTRIBUTOR_CODE, 'ASM[' + asm.ASM_NAME  +']'   AS DISTRIBUTOR_NAME,

					                               tracking.LONGITUDE_LATITUDE, 
                                                   tracking.DEVICE_STATUS, 
                                                   tracking.NO_REPEAT, 
                                                   tracking.BEGIN_DATETIME, 
                                                   tracking.END_DATETIME, 
                                                   tracking.BATTERY_PERCENTAGE, 
                                                   tracking.TYPE_TRACKING, 
                                                   tracking.POINT_RADIUS, 
                                                   tracking.TRACKING_ACCURACY, 
                                                   tracking.ANGEL, 
                                                   tracking.BATTERY_PERCENTAGE_VALUE, 
                                                   tracking.BATTERY_PERCENTAGE_DATETIME, 
                                                   tracking.TRACKING_PROVIDER, 
                                                   tracking.TRACKING_PROVIDER_VALUE, 
                                                   tracking.TRACKING_PROVIDER_DATETIME,
                                                   tracking.LAST_UPDATE
                                                

                                	               
                                        FROM @TB_Tracking tracking INNER JOIN  M_ASM asm ON asm.ASM_CD = tracking.ASM_CD   
                                                    INNER JOIN M_RSM rsm ON asm.RSM_CD = rsm.RSM_CD
                                        ORDER BY asm.ASM_CODE,tracking.BEGIN_DATETIME
                                            
                                            
                                        ", ASM, yymmdd, timeFrom, timeEnd, sqlGetTrackingOfASM);



            return L5sSql.Query(sql);

        }


        #endregion


        private string P5sConvertDataTableCustomerJson(DataTable dtCustomer)
        {
            DataTable dt = dtCustomer;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            List<CCustomer> customers = new List<CCustomer>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String customerCD = dt.Rows[i]["CUSTOMER_CD"].ToString();
                String customerCode = dt.Rows[i]["CUSTOMER_CODE"].ToString();
                String customerName = dt.Rows[i]["CUSTOMER_NAME"].ToString();
                String customerAddress = dt.Rows[i]["CUSTOMER_ADDRESS"].ToString();
                String customerChainCode = dt.Rows[i]["CUSTOMER_CHAIN_CODE"].ToString();

                String distributorCD = dt.Rows[i]["DISTRIBUTOR_CD"].ToString();
                String distributorCode = dt.Rows[i]["DISTRIBUTOR_CODE"].ToString();
                String distributorName = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();

                String salesCD = dt.Rows[i]["SALES_CD"].ToString();
                String saleCode = dt.Rows[i]["SALES_CODE"].ToString();
                String salesName = dt.Rows[i]["SALES_NAME"].ToString();


                String routeCD = dt.Rows[i]["ROUTE_CD"].ToString();
                String routeCode = dt.Rows[i]["ROUTE_CODE"].ToString();
                String routeName = dt.Rows[i]["ROUTE_NAME"].ToString();

                String latLngs = dt.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                String routeDesc = dt.Rows[i]["ROUTE_NAME"].ToString();

                String customerIsVisit = dt.Rows[i]["IS_VISIT"].ToString();

                String customerTimeIn = dt.Rows[i]["TimeIn"].ToString();
                String customerTimeOut = dt.Rows[i]["TimeOut"].ToString();

                String customerSalesAmount = dt.Rows[i]["SALES_AMOUNT"].ToString();
                String customerSalesTP = dt.Rows[i]["SALES_TP"].ToString();
                String customerSalesTB = dt.Rows[i]["SALES_TB"].ToString();
                String customerOrders = dt.Rows[i]["CUSTOMER_ORDERS"].ToString();


                if (latLngs != "")
                {
                    CCustomer c = new CCustomer(customerCD,
                                                customerCode,
                                                customerName,
                                                customerAddress,
                                                customerChainCode,
                                                latLngs,
                                                 distributorCD,
                                                 distributorCode,
                                                 distributorName,
                                                 salesCD,
                                                 saleCode,
                                                 salesName,
                                                 routeCD,
                                                 routeCode,
                                                 routeName,
                                                 customerIsVisit,
                                                 customerTimeIn,
                                                 customerTimeOut,
                                                 customerSalesAmount,
                                                 customerSalesTP,
                                                 customerSalesTB,
                                                 customerOrders);

                    customers.Add(c);
                }
            }
            return oSerializer.Serialize(customers);
        }
        private string P5sConvertDataTableTrackingJson(DataTable dtable)
        {
            DataTable dt = dtable;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            List<CTracking> trackings = new List<CTracking>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;


            String[] arrColor = new String[] { "#0000ee", "#458b00", "#cd8c95", "#008b8b", "#eead0e", "#006400", "#8b4500", "#00bfff", "#cd5555", "#7cfc00", "#eedc82", "#20b2aa", "#8470ff", "#ffc1c1", "#436eee", "#d02090", "#8b8b00", "#9acd32", "#87ceff", "#ffff00" };

            DataTable dtImage = L5sSql.Query("SELECT IMAGE_CD, IMAGE_URL FROM M_IMAGE");


            Double pointDuration = Double.Parse(L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_DURATION' ").Rows[0]["VALUE"].ToString());

            //process Image display each sales

            Dictionary<String, String> dicSalesColor = new Dictionary<String, String>();
            DataTable dtSales = new DataView(dt).ToTable(true, "SALES_CD");

            String[] arrIcons = new String[] {   "icon/marker_geo_1.png", 
                                                 "icon/marker_geo_2.png", 
                                                 "icon/marker_geo_3.png", 
                                                 "icon/marker_geo_4.png", 
                                                 "icon/marker_geo_5.png", 
                                                 "icon/marker_geo_6.png", 
                                                 "icon/marker_geo_7.png", 
                                                 "icon/marker_geo_8.png",
                                                 "icon/marker_geo_9.png",
                                                 "icon/marker_geo_10.png"
                                              };

            //tính năng show multi icon là mục đích là hiển thị nhiều dữ liệu tracking của nhiều NVBH lên cùng 1 lúc
            //việc này sẽ rất phức tạp khi xử lý nên tạm thời đã bị khóa
            //nếu có xử dụng thì phải kiểm tra kĩ lại 
            //TRACKING_SHOW_MULTI_ICON
            Boolean showMultiIcon = Boolean.Parse(L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_SHOW_MULTI_ICON' ").Rows[0]["VALUE"].ToString());

            if (showMultiIcon)
            {
                int indexIcon = 0;
                for (int i = 0; i < dtSales.Rows.Count; i++)
                {
                    if (indexIcon == arrIcons.Length - 1)
                    {
                        indexIcon = 0;
                    }
                    dicSalesColor.Add(dtSales.Rows[i]["SALES_CD"].ToString(), arrIcons[indexIcon].ToString());
                    indexIcon++;
                }
            }


            int indexColor = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                String salesCD = dt.Rows[i]["SALES_CD"].ToString();
                String salesCode = dt.Rows[i]["SALES_CODE"].ToString();
                String salesName = dt.Rows[i]["SALES_NAME"].ToString();

                String distributorCD = dt.Rows[i]["DISTRIBUTOR_CD"].ToString();
                String distributorCode = dt.Rows[i]["DISTRIBUTOR_CODE"].ToString();
                String distributorName = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();

                String latLngs = dt.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                String deviceStatus = "";

                String noRepeat = dt.Rows[i]["NO_REPEAT"].ToString();

                String beginDatetime = dt.Rows[i]["BEGIN_DATETIME"].ToString();
                String endDatetime = dt.Rows[i]["END_DATETIME"].ToString();

                String batteryPercentage = "";
                String lastUpdate = dt.Rows[i]["LAST_UPDATE"].ToString();


                //process device status
                //định dạng lại các hiển thị trong trường hợp dữ liệu quá dài thì cần phai xuống dòng để có thể hiển thị 
                // thông tin 1 cách dễ nhìn
                if (dt.Rows[i]["DEVICE_STATUS"].ToString().Trim().Length > 0)
                {
                    String[] arrayDeviceStatus = dt.Rows[i]["DEVICE_STATUS"].ToString().Split(new String[] { "->" }, StringSplitOptions.None);
                    if (arrayDeviceStatus.Length > 1)
                    {
                        deviceStatus = String.Format("{0}-> ", arrayDeviceStatus[0]);
                        for (int k = 1; k < arrayDeviceStatus.Length; k++)
                        {
                            if (k % 2 == 0)
                            {
                                deviceStatus += String.Format("{0}->", arrayDeviceStatus[k]);
                            }
                            else
                            {
                                deviceStatus += String.Format("{0}->  </br>", arrayDeviceStatus[k]);
                            }
                        }
                    }
                    else
                        deviceStatus = String.Format("{0}", arrayDeviceStatus[0]);

                }



                //xử lý BATTERY_PERCENTAGE để cho thấy sự thay đổi lên xuống của % BATTERY_PERCENTAGE
                String[] arrayBatteryPecentage = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"].ToString().Split(new char[] { ',' });
                String[] arrayBatteryPecentageDateTime = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"].ToString().Split(new char[] { ',' });

                List<String> percentages = new List<String>();
                List<String> percentagesDateTime = new List<String>();
                //thuật toán vẽ lại biểu đồ tình trạng pin theo hình Sin
                for (int k = 0; k < arrayBatteryPecentage.Length; k++)
                {
                    if (k == 0)
                    {
                        percentages.Add(arrayBatteryPecentage[k]);
                        percentagesDateTime.Add(arrayBatteryPecentageDateTime[k]);
                    }
                    else
                        if (k == arrayBatteryPecentage.Length - 1 && percentages[percentages.Count - 1] != arrayBatteryPecentage[k])
                        {
                            percentages.Add(arrayBatteryPecentage[k]);
                            percentagesDateTime.Add(arrayBatteryPecentageDateTime[k]);
                        }
                        else
                            if (int.Parse(percentages[percentages.Count - 1]) < int.Parse(arrayBatteryPecentage[k])
                                  && int.Parse(arrayBatteryPecentage[k]) > int.Parse(arrayBatteryPecentage[k + 1]))
                            {
                                percentages.Add(arrayBatteryPecentage[k]);
                                percentagesDateTime.Add(arrayBatteryPecentageDateTime[k]);
                            }
                            else
                                if (int.Parse(percentages[percentages.Count - 1]) > int.Parse(arrayBatteryPecentage[k])
                                 && int.Parse(arrayBatteryPecentage[k]) < int.Parse(arrayBatteryPecentage[k + 1]))
                                {
                                    percentages.Add(arrayBatteryPecentage[k]);
                                    percentagesDateTime.Add(arrayBatteryPecentageDateTime[k]);
                                }

                }

                //add thêm thời gian lúc ghi nhận battery percentage
                for (int k = 0; k < percentages.Count; k++)
                {
                    try
                    {
                        if (k == 0)
                            batteryPercentage = percentages[k] + String.Format("% ({0}) ", DateTime.Parse(percentagesDateTime[k]).ToString("hh:mm:ss tt"));
                        else
                            if (k % 2 == 0)
                                batteryPercentage += " -> </br> " + percentages[k] + String.Format("% ({0}) ", DateTime.Parse(percentagesDateTime[k]).ToString("hh:mm:ss tt"));
                            else
                                batteryPercentage += " -> " + percentages[k] + String.Format("% ({0}) ", DateTime.Parse(percentagesDateTime[k]).ToString("hh:mm:ss tt"));

                    }
                    catch (Exception)
                    {

                    }

                }





                //xử lý TRACKING_PROVIDER để cho thấy sự thay đổi lên tính hiệu
                String[] arrayProvider = dt.Rows[i]["TRACKING_PROVIDER_VALUE"].ToString().Split(new char[] { ',' });
                String[] arrayProviderDateTime = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"].ToString().Split(new char[] { ',' });

                //thuật toán vẽ lại biểu đồ tình trạng provider

                String providers = "";
                String currentProvider = "";

                if (arrayProvider.Length == 1)
                {
                    providers = String.Format("{0} [{1} -> {1}] ", arrayProvider[0], DateTime.Parse(arrayProviderDateTime[0]).ToString("hh:mm:ss tt"));
                }
                else
                {
                    for (int k = 0; k < arrayProvider.Length; k++)
                    {
                        if (k == 0)
                        {
                            currentProvider = arrayProvider[k];
                            providers = String.Format("{0} [{1}", arrayProvider[k], DateTime.Parse(arrayProviderDateTime[k]).ToString("hh:mm:ss tt"));
                        }
                        else
                            if (k == arrayProvider.Length - 1)
                            {
                                providers += String.Format(" -> {0}] ", DateTime.Parse(arrayProviderDateTime[k]).ToString("hh:mm:ss tt"));
                            }
                            else
                            {
                                if (arrayProvider[k] != currentProvider)
                                {
                                    currentProvider = arrayProvider[k];
                                    providers += String.Format(" -> {0}] </br> ", DateTime.Parse(arrayProviderDateTime[k - 1]).ToString("hh:mm:ss tt"));
                                    providers += String.Format("{0} [{1}", arrayProvider[k], DateTime.Parse(arrayProviderDateTime[k]).ToString("hh:mm:ss tt"));
                                }
                            }
                    }
                }

                //value to test layout
                //providers = "network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] </br> network[09:09:33 AM -> 09:50:49 AM] ";
                //batteryPercentage = "92% (08:15:31 AM) -> 100% (09:50:49 AM)  </br> -> 92% (08:15:31 AM) -> 100% (09:50:49 AM)  </br> ->92% (08:15:31 AM) -> 100% (09:50:49 AM)  </br> -> 92% (08:15:31 AM) -> 100% (09:50:49 AM)  </br> -> 92% (08:15:31 AM) -> 100% (09:50:49 AM)";

                String typeTracking = dt.Rows[i]["TYPE_TRACKING"].ToString();
                String pointRadius = dt.Rows[i]["POINT_RADIUS"].ToString();

                Double angle = 0;
                try
                {
                    angle = Double.Parse(dt.Rows[i]["ANGEL"].ToString());
                }
                catch (Exception)
                {

                }

                //thiết lập màu sắc cho từng NVBH
                //nếu chế độ hiển thị multi icon đc bật thì mỗi NVBH sẽ phải có 1 màu sắc hiển thị khác nhau để phân biệt
                String color = arrColor[++indexColor];
                if (indexColor == arrColor.Length - 1)
                    indexColor = 0;

                CTracking d = new CTracking(
                                                salesCD, salesCode, salesName,
                                                distributorCD, distributorCode, distributorName,

                                                latLngs, deviceStatus, noRepeat, beginDatetime, endDatetime,
                                                batteryPercentage, typeTracking, pointRadius, dtImage, pointDuration, dicSalesColor, angle,
                                                providers, lastUpdate
                                            );
                trackings.Add(d);
            }
            return oSerializer.Serialize(trackings);
        }



        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {
                if (this.P5sActRegion != null)
                    this.P5sActRegion.L5sSetDefaultValues(this.P5sTxtRegion.Text);

                if (this.P5sActArea != null)
                    this.P5sActArea.L5sSetDefaultValues(this.P5sTxtArea.Text);

                if (this.P5sActSupervisor != null)
                    this.P5sActSupervisor.L5sSetDefaultValues(this.P5sTxtSupervisor.Text);

                if (this.P5sActASM != null)
                    this.P5sActASM.L5sSetDefaultValues(this.P5sTxtASM.Text);

                if (this.P5sActDistributor != null)
                    this.P5sActDistributor.L5sSetDefaultValues(this.P5sTxtDistributor.Text);

                if (this.P5sActSales != null)
                    this.P5sActSales.L5sSetDefaultValues(this.P5sTxtSales.Text);

            }
            catch (Exception)
            {


            }

        }

       

        protected void P5sTxtDay_TextChanged(object sender, EventArgs e)
        {
            this.P5sTxtRegion.Text = this.P5sTxtArea.Text = this.P5sTxtDistributor.Text = this.P5sTxtSales.Text = this.P5sTxtSupervisor.Text = "";
            L5sJS.L5sRun("LoadMap()"); // load map first
            this.P5sInit();
        }

        

        protected void P5sDdlViewOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.P5sAutoCompleteInit();
            this.P5sTxtRegion.Text = "";
            this.P5sTxtArea.Text = "";
            this.P5sTxtDistributor.Text = "";
            this.P5sTxtSales.Text = "";
            this.P5sTxtSupervisor.Text = "";
            this.P5sTxtASM.Text = "";
        }

        //Code Thêm mới 190301

        protected void P5sDDLRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            String index = P5sDDLRegion.SelectedValue.ToString();
            DataTable dt = P5sGetArea(index);
            P5sDDLArea.DataSource = dt;
            P5sDDLArea.DataTextField = "AREA_CODE";
            P5sDDLArea.DataValueField = "AREA_CD";
            P5sDDLArea.DataBind();
            P5sDDLArea.SelectedIndex = 0;

            String index1 = P5sDDLArea.SelectedValue.ToString();
            DataTable dtdis = P5sGetDistributor(index1);
            P5sDDLDistributor.DataSource = dtdis;
            P5sDDLDistributor.DataTextField = "DISTRI_NAME";
            P5sDDLDistributor.DataValueField = "DISTRIBUTOR_CD";
            P5sDDLDistributor.DataBind();
            P5sDDLDistributor.SelectedIndex = 0;

            DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            selDSR.DataSource = dtSale;
            selDSR.DataTextField = "NAME_SALE";
            selDSR.DataValueField = "SALES_CD";
            selDSR.DataBind();
        }
        protected void P5sDDLDistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            selDSR.DataSource = dtSale;
            selDSR.DataTextField = "NAME_SALE";
            selDSR.DataValueField = "SALES_CD";
            selDSR.DataBind();
        }

        protected void P5sDDLArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            String index = P5sDDLArea.SelectedValue.ToString();
            DataTable dt = P5sGetDistributor(index);
            P5sDDLDistributor.DataSource = dt;
            P5sDDLDistributor.DataTextField = "DISTRI_NAME";
            P5sDDLDistributor.DataValueField = "DISTRIBUTOR_CD";
            P5sDDLDistributor.DataBind();
            P5sDDLDistributor.SelectedIndex = 0;

            DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            selDSR.DataSource = dtSale;
            selDSR.DataTextField = "NAME_SALE";
            selDSR.DataValueField = "SALES_CD";
            selDSR.DataBind();

        }
        protected  DataTable P5sGetArea(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@" 
                                            SELECT  AREA_CD,AREA_CODE, '', ACTIVE 
                                            FROM  M_AREA 
                                            WHERE  AREA_CD IN (  SELECT are.AREA_CD
                                                                        FROM  M_AREA are  INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                                                        WHERE are.ACTIVE = 1  AND EXISTS ( SELECT * FROM
										                                                                            M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										                                            INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										                                            INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
								                                            WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1 and
								                                            are.REGION_CD = {0}
                                                                                
								                                            ) 
					                                            )

                                            ORDER BY AREA_ORDER", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        protected DataTable P5sGetDistributor(string p)
        {
            p = p == "" ? @"' '" : p;


            String P5sSqlSelect = String.Format(@"SELECT DISTINCT dis.DISTRIBUTOR_CD,dis.DISTRIBUTOR_CODE + '-'+ dis.DISTRIBUTOR_NAME as DISTRI_NAME, '', dis.ACTIVE 
                                                FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
                                                INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
                                                INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
                                                INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                AND dis.DISTRIBUTOR_TYPE_CD = 1
                                                JOIN M_AREA are on are.AREA_CD = arePro.AREA_CD
                                                where are.AREA_CD = {0} and dis.ACTIVE = 1 ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        protected DataTable P5sGetSales(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT sls.SALES_CD,sls.SALES_CODE + '-'+ sls.SALES_NAME as NAME_SALE, '', sls.ACTIVE 
                                                FROM  M_SALES sls INNER JOIN M_DISTRIBUTOR dist ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
                                                where dist.DISTRIBUTOR_CD in ({0}) and sls.ACTIVE = 1
                                                ORDER BY SALES_CODE
                                                 ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        
    }
}