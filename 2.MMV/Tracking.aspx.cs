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

namespace MMV
{
    partial class Tracking : System.Web.UI.Page
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

                for (int i = this.P5sDdlFromHH.Items.Count - 1; i >= 0; i--)
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

        protected void listItem()
        {
            P5sDdlViewOption.Items.Add(new ListItem(L5sMaster.L5sLangs["DSRs - NVBH"], "1"));
            P5sDdlViewOption.Items.Add(new ListItem(L5sMaster.L5sLangs["CDSs - Giám sát"], "2"));
            P5sDdlViewOption.Items.Add(new ListItem(L5sMaster.L5sLangs["ASMs - Quản lý khu vực"], "3"));


        }
        private void P5sInit()
        {
            this.listItem();
            DateTime date = DateTime.Now;
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

        private void P5sAutoCompleteInit( )
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
            Response.Redirect("./TrackingVisitUnVisit.aspx");
        }

        protected void P5sSettingAndReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Report/CustomerList.aspx");
        }

        protected void P5sLbtnLoadTracking_Click(object sender, EventArgs e)
        {
            L5sJS.L5sRun("LoadMap()"); // load map first
            String salesCDs = this.P5sTxtSales.Text;
            String supervisors = this.P5sTxtSupervisor.Text;
            String asms = this.P5sTxtASM.Text;
            String distributors = this.P5sTxtDistributor.Text;

            DateTime date = DateTime.Now;
            if (Request[this.P5sTxtDay.UniqueID].ToString() != "")
            {
                try
                {
                    date = DateTime.ParseExact(Request[this.P5sTxtDay.UniqueID], "dd-MM-yyyy", null);
                    this.P5sInit();
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
            switch (P5sDdlViewOption.SelectedValue)
            {
                case "1": // DSR
                    {
                        if (salesCDs.Trim().Length > 0)
                        {
                            //load dữ liệu tracking
                            DataTable dt = this.P5sGetTrackingOfSale(salesCDs, yymmdd, timeFrom, timeTo);
                            if (dt.Rows.Count <= 0)
                            {
                                L5sMsg.Show("No data !.");
                                return;
                            }
                            //load dữ liệu Khách hàng
                            DataTable dtCustomer = this.P5sGetCustomer(salesCDs, date.ToString("yyMMdd"));

                            //convert  datatable sang json để xử dụng ở javascript
                            String objectCustomer = this.P5sConvertDataTableCustomerJson(dtCustomer);
                            String objectTracking = this.P5sConvertDataTableTrackingJson(dt);
                            L5sJS.L5sRun("CreatedCTracking('" + objectTracking + "','" + objectCustomer + "')");
                        }
                        break;
                    }
                case "2": //CDS
                    {
                        if (supervisors.Trim().Length > 0)
                        {
                            //load dữ liệu tracking
                            DataTable dt = this.P5sGetTrackingOfSupervisor(supervisors, yymmdd, timeFrom, timeTo);
                            if (dt.Rows.Count <= 0)
                            {
                                L5sMsg.Show("No data !.");
                                return;
                            }

                            //load dữ liệu Khách hàng
                            DataTable dtCustomer = this.P5sGetCustomerBySupervisor(supervisors, date.ToString("yyMMdd"));

                            //convert  datatable sang json để xử dụng ở javascript
                            String objectCustomer = this.P5sConvertDataTableCustomerJson(dtCustomer);
                            String objectTracking = this.P5sConvertDataTableTrackingJson(dt);
                            L5sJS.L5sRun("CreatedCTracking('" + objectTracking + "','" + objectCustomer + "')");
                        }

                        break;
                    }
                case "3": // ASM

                    if (asms.Trim().Length > 0)
                    {
                        //load dữ liệu tracking
                        DataTable dt = this.P5sGetTrackingOfASM(asms, yymmdd, timeFrom, timeTo);
                        if (dt.Rows.Count <= 0)
                        {
                            L5sMsg.Show("No data !.");
                            return;
                        }


                        //load dữ liệu Khách hàng
                        DataTable dtCustomer = this.P5sGetCustomerByASM(asms, date.ToString("yyMMdd"));

                        //convert  datatable sang json để xử dụng ở javascript
                        String objectCustomer = this.P5sConvertDataTableCustomerJson(dtCustomer);
                        String objectTracking = this.P5sConvertDataTableTrackingJson(dt);
                        L5sJS.L5sRun("CreatedCTracking('" + objectTracking + "','" + objectCustomer + "')");
                    }
                    break;

            }



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
                                                        
                                            
			                                ) AS T", salesCDs, yymmdd,sqlGetSqlTimeInOut);
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
                                                        
                                       
			) AS T", supervisorCDs, yymmdd,sqlGetSqlTimeInOut);
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


			                                        ) AS T", ASMCDs, yymmdd,sqlGetSqlTimeInOut);
            return L5sSql.Query(sql);
        }



        #region get tracking of sales

        private DataTable P5sGetTrackingOfSale(String salesCD, String yymmdd, String timeFrom, String timeEnd)
        {
            // lệnh này rất qua trọng vì hệ thống đang chốt số theo tuần nền cần phải tạo câu sql động với tham số truyền vào là thời gian để 
            //có thể lấy đúng dữ liệu và đúng bảng
            String sqlGetTrackingOfSales = P5sCmmFns.P5sGetDynamicSqlTrackingOfSales(DateTime.ParseExact(yymmdd, "yyMMdd", null), DateTime.ParseExact(yymmdd, "yyMMdd", null) );
            if (sqlGetTrackingOfSales.Equals("-1"))
            {                
                return null;
            }

            
            String sql = String.Format(@"

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
                                                  ,[CREATED_DATE]
                                                  ,[LOCATION_ADDRESS], 
                                                   LAST_UPDATE = CREATED_DATE                            
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

        #region get tracking of supervisor

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
                                                      ,[CREATED_DATE]
                                                      ,[LOCATION_ADDRESS], LAST_UPDATE = CREATED_DATE
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

                                        ", supervisor, yymmdd, timeFrom, timeEnd,sqlGetTrackingOfSupervisor);



            return L5sSql.Query(sql);

        }


        #endregion

        #region get tracking of ASM

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
                                                      ,[CREATED_DATE]
                                                      ,[LOCATION_ADDRESS], LAST_UPDATE = CREATED_DATE
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
                                            
                                            
                                        ", ASM, yymmdd, timeFrom, timeEnd,sqlGetTrackingOfASM);



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


    }



    public class CTracking
    {
        public String SalesCD;
        public String SalesCode;
        public String SalesName;

        public String DistributorCD;
        public String DistributorCode;
        public String DistributorName;

        public String LatLngs;
        public String DeviceStatus;
        public String NoRepeat;


        public String TimeIn;
        public String TimeOut;
        public String Duration;
        private Double DurationByMinute;

        public String BatteryPercentage;

        public String TypeTracking;

        public String MainPoint;


        public String ImageUrl;

        private Double Angle;
        public String Providers;


        private Double SystemPointDuration;
        private DataTable DtImage;
        public String SystemPointRadius;

        public int NumberOfStop;

        public String LastUpdate;

        private String YYMMDD = ""; //value verify icon

        Dictionary<String, String> dicSalesColor = new Dictionary<String, String>();

        public CTracking(String SalesCD, String SalesCode, String SalesName,
                         String DistributorCD, String DistributorCode, String DistributorName,

                         String LatLngs, String DeviceStatus, String NoRepeat, String BeginDatetime,
                         String EndDatetime, String batteryPercentage,

                         String TypeTracking, String SystemPointRadius, DataTable dtImage, Double SystemPointDuration,

                         Dictionary<String, String> dicSalesColor, Double angle, String providers, String LastUpdate

                         )
        {
            this.SalesCD = SalesCD;
            this.SalesCode = SalesCode;
            this.SalesName = SalesName;

            this.DistributorCD = DistributorCD;
            this.DistributorCode = DistributorCode;
            this.DistributorName = DistributorName;

            this.LatLngs = LatLngs;
            this.DeviceStatus = DeviceStatus;
            this.NoRepeat = NoRepeat;

            this.TimeIn = BeginDatetime;
            this.TimeOut = EndDatetime;



            this.BatteryPercentage = batteryPercentage;

            this.TypeTracking = TypeTracking;
            this.SystemPointRadius = SystemPointRadius;
            this.DtImage = dtImage;
            this.Duration = "";
            this.SystemPointDuration = SystemPointDuration;

            this.MainPoint = "";



            this.dicSalesColor = dicSalesColor;
            this.Angle = angle;

            this.Providers = providers;

            DateTime dtTimeIn = new DateTime();
            DateTime dtTimeOut = new DateTime();

            this.NumberOfStop = 0;

            try
            {
                DateTime dt = DateTime.Parse(LastUpdate);
                this.LastUpdate = dt.ToString("dd/MM/yyyy hh:mm:ss tt"); //định dạng cách hiển thị thời gian
            }
            catch (Exception)
            {
                
            }
        

            try
            {
                if (this.TimeIn != "")
                {
                    dtTimeIn = DateTime.Parse(this.TimeIn);
                    this.TimeIn = dtTimeIn.ToString("hh:mm:ss tt"); //định dạng cách hiển thị thời gian

                    this.YYMMDD = dtTimeIn.ToString("yyMMdd");
                }

            }
            catch (Exception)
            {

            }

            try
            {
                if (this.TimeOut != "")
                {
                    dtTimeOut = DateTime.Parse(this.TimeOut);
                    this.TimeOut = dtTimeOut.ToString("hh:mm:ss tt"); //định dạng cách hiển thị thời gian
                }
            }
            catch (Exception)
            {

            }

            //nếu thời gian bắt đầu và kết thúc ko rỗng thì sẽ tính toán đc khoảng thời gian dừng (duration)
            if (this.TimeIn != "" && this.TimeOut != "")
            {
                TimeSpan span = dtTimeOut.Subtract(dtTimeIn);
                this.Duration = span.Hours.ToString() + ":" + span.Minutes.ToString() + ":" + span.Seconds.ToString();
                this.DurationByMinute = span.TotalMinutes; 
            }
            else
            {
                this.Duration = "";
                this.DurationByMinute = 0;
            }

            //gọi hàm xử lý việc hiển thị image: Start, Stop, End, StartEnd
            this.ProcessImageUrl();

        }

        private void ProcessImageUrl()
        {

            // thông số hình ảnh được lưu trong M_IMAGE
            if (this.TypeTracking == P5sEnum.TrackingStart) // điểm bắt đầu
            {

                this.ImageUrl = this.DtImage.Select("IMAGE_CD = 1")[0]["IMAGE_URL"].ToString();
                this.MainPoint = "Start";
                return;
            }

            if (this.TypeTracking == P5sEnum.TrackingEnd) // điểm kết thúc
            {
                if (this.YYMMDD != DateTime.Now.ToString("yyMMdd"))
                    this.ImageUrl = this.DtImage.Select("IMAGE_CD = 2")[0]["IMAGE_URL"].ToString();
                else
                    this.ImageUrl = this.DtImage.Select("IMAGE_CD = 4")[0]["IMAGE_URL"].ToString();

                this.MainPoint = "End";
                return;
            }

            if (this.TypeTracking == P5sEnum.TrackingStartEnd) // điểm bắt đầu và kết thúc trùng nhau
            {
                if (this.YYMMDD != DateTime.Now.ToString("yyMMdd"))
                    this.ImageUrl = this.DtImage.Select("IMAGE_CD = 2")[0]["IMAGE_URL"].ToString();
                else
                    this.ImageUrl = this.DtImage.Select("IMAGE_CD = 4")[0]["IMAGE_URL"].ToString();


                this.MainPoint = "End";
                return;
            }

            if (this.TypeTracking == P5sEnum.TrackingPoint) // điểm dừng bình thường
            {
                if (this.SystemPointDuration <= this.DurationByMinute)
                    //nếu duration lớn hơn thời gian dừng hệ thống => ghi nhận nó là 1 stoppint
                    //ngược lại thì nó chỉ là 1 point bình thường
                {
                    this.ImageUrl = this.DtImage.Select("IMAGE_CD = 5")[0]["IMAGE_URL"].ToString();
                    this.MainPoint = "Stop";
                    this.NumberOfStop = 1;
                }
                else 
                {


                    if (this.dicSalesColor.Count != 0)
                        this.ImageUrl = this.dicSalesColor[this.SalesCD].ToString();
                    else
                        this.ImageUrl = this.ImageUrl = this.getMoveIcon();

                    this.MainPoint = "Point";
                }
                return;
            }

        }

        //hàm quan trọng để xác định hướng đi giữa 2 điểm
        private String getMoveIcon()
        {
            Double angle = Math.Round(this.Angle, 0);
            String s = "";

            int mod = (int)angle % 10;

            if (mod != 0)
            {
                if (mod <= 5)
                    angle = angle - mod;
                else
                    angle = angle - mod + 10;
            }

            switch (angle.ToString())
            {
                case "0":
                    s = "icon/angle/move0.gif";
                    break;
                case "10":
                    s = "icon/angle/move10.gif";
                    break;
                case "20":
                    s = "icon/angle/move20.gif";
                    break;
                case "30":
                    s = "icon/angle/move30.gif";
                    break;
                case "40":
                    s = "icon/angle/move40.gif";
                    break;
                case "50":
                    s = "icon/angle/move50.gif";
                    break;
                case "60":
                    s = "icon/angle/move60.gif";
                    break;
                case "70":
                    s = "icon/angle/move70.gif";
                    break;
                case "80":
                    s = "icon/angle/move80.gif";
                    break;
                case "90":
                    s = "icon/angle/move90.gif";
                    break;
                case "100":
                    s = "icon/angle/move100.gif";
                    break;
                case "110":
                    s = "icon/angle/move110.gif";
                    break;
                case "120":
                    s = "icon/angle/move120.gif";
                    break;
                case "130":
                    s = "icon/angle/move130.gif";
                    break;
                case "140":
                    s = "icon/angle/move140.gif";
                    break;
                case "150":
                    s = "icon/angle/move150.gif";
                    break;
                case "160":
                    s = "icon/angle/move160.gif";
                    break;
                case "170":
                    s = "icon/angle/move170.gif";
                    break;
                case "180":
                    s = "icon/angle/move180.gif";
                    break;
                case "190":
                    s = "icon/angle/move190.gif";
                    break;
                case "200":
                    s = "icon/angle/move200.gif";
                    break;
                case "210":
                    s = "icon/angle/move210.gif";
                    break;
                case "220":
                    s = "icon/angle/move220.gif";
                    break;
                case "230":
                    s = "icon/angle/move230.gif";
                    break;
                case "240":
                    s = "icon/angle/move240.gif";
                    break;
                case "250":
                    s = "icon/angle/move250.gif";
                    break;
                case "260":
                    s = "icon/angle/move260.gif";
                    break;
                case "270":
                    s = "icon/angle/move270.gif";
                    break;
                case "280":
                    s = "icon/angle/move280.gif";
                    break;
                case "290":
                    s = "icon/angle/move290.gif";
                    break;
                case "300":
                    s = "icon/angle/move300.gif";
                    break;
                case "310":
                    s = "icon/angle/move310.gif";
                    break;
                case "320":
                    s = "icon/angle/move320.gif";
                    break;
                case "330":
                    s = "icon/angle/move330.gif";
                    break;
                case "340":
                    s = "icon/angle/move340.gif";
                    break;
                case "350":
                    s = "icon/angle/move350.gif";
                    break;
                case "360":
                    s = "icon/angle/move360.gif";
                    break;
                default:
                    s = "icon/angle/move0.gif";
                    break;
            }

            return s;
        }

    }
    public class CCustomer
    {
        public String CustomerCD;
        public String CustomerCode;
        public String CustomerName;
        public String CustomerAddress;
        public String CustomerChainCode;

        public String DistributorCD;
        public String DistributorCode;
        public String DistributorName;
        public String SalesCD;
        public String SalesCode;
        public String SalesName;
        public String LatLng;
        public String RouteDesc;

        public String TimeIn;
        public String TimeOut;
        public String Duration;

        private DateTime dtTimeIn;
        private DateTime dtTimeOut;

        public String RouteCD;
        public String RouteCode;
        public String RouteName;
        public String status;


        public String CustomerIsVisit;
        public String CustomerSalesAmount;
        public String CustomerOrders;
        public String CustomerSalesTP;
        public String CustomerSalesTB;
        


        public CCustomer(String CustomerCD, String CustomerCode, String CustomerName,
                         String CustomerAddress, String CustomerChainCode,
                         String CustomerLatLng,
                         String DistributorCD, String DistributorCode, String DistributorName,
                         String SalesCD, String SalesCode, String SalesName,
                         String RouteCD, String RouteCode, String RouteName,
                         String CustomerIsVisit,
                         String TimeIn, String TimeOut, 
                         String CustomerSalesAmount,  String CustomerSalesTP,
                         String CustomerSalesTB,String CustomerOrders
                         )
        {
            this.CustomerCD = CustomerCD;
            this.CustomerCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerCode);
            this.CustomerName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerName);

            this.DistributorCD = DistributorCD;
            this.DistributorCode = DistributorCode;
            this.DistributorName = DistributorName;


            this.SalesCD = SalesCD;
            this.SalesCode = SalesCode;
            this.SalesName = SalesName;

            this.RouteCD = RouteCD;
            this.RouteCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteCode);
            this.RouteName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteName);

            this.LatLng = CustomerLatLng;

            this.CustomerAddress = CustomerAddress;

            this.CustomerChainCode = CustomerChainCode;

            this.CustomerIsVisit = CustomerIsVisit;
            this.processTimeInOut(TimeIn, TimeOut);         
            this.CustomerSalesAmount = CustomerSalesAmount;
            this.CustomerSalesTP = CustomerSalesTP;
            this.CustomerSalesTB = CustomerSalesTB;
            this.CustomerOrders = CustomerOrders;

        }


        //tính toán duration (khoảng thời gian) khi bắt đầu ghé thăm(timein) và kết thúc ghé thăm (timeout)
        private void processTimeInOut(String TimeIn, String TimeOut)
        {
            this.TimeIn = TimeIn;
            this.TimeOut = TimeOut;


            try
            {
                if (this.TimeIn != "")
                {
                    dtTimeIn = DateTime.Parse(this.TimeIn);
                    this.TimeIn = dtTimeIn.ToString("hh:mm:ss tt"); //định dạng cách hiển thị
                }

            }
            catch (Exception)
            {

            }

            try
            {
                if (this.TimeOut != "")
                {
                    dtTimeOut = DateTime.Parse(this.TimeOut);
                    this.TimeOut = dtTimeOut.ToString("hh:mm:ss tt");  //định dạng cách hiển thị
                }
            }
            catch (Exception)
            {

            }

            if (this.TimeIn != "" && this.TimeOut != "")
            {
                TimeSpan span = dtTimeOut.Subtract(dtTimeIn);
                this.Duration = span.Hours.ToString() + ":" + span.Minutes.ToString() + ":" + span.Seconds.ToString();  //định dạng cách hiển thị
            }
            else
                this.Duration = "";


        }


    }

    public class MCustomer
    {
        public String CustomerCD;
        public String CustomerCode;
        public String CustomerName;
        public String CustomerAddress;
        public String CustomerChainCode;

        public String DistributorCD;
        public String DistributorCode;
        public String DistributorName;
        public String SalesCD;
        public String SalesCode;
        public String SalesName;
        public String LatLng;
        public String RouteDesc;

        public String TimeIn;
        public String TimeOut;
        public String Duration;

        private DateTime dtTimeIn;
        private DateTime dtTimeOut;

        public String RouteCD;
        public String RouteCode;
        public String RouteName;
        public int status;


        public String CustomerIsVisit;
        public String CustomerSalesAmount;
        public String CustomerOrders;
        public String CustomerSalesTP;
        public String CustomerSalesTB;

        public String customerMTD;
        public String customer2M;
        public String customer3M;
        public String lastVistDate;
        public String lastOrderdate;
        public String lastOrderValue;


        public MCustomer(String CustomerCD, String CustomerCode, String CustomerName,
                         String CustomerAddress, String CustomerChainCode,
                         String CustomerLatLng,
                         String DistributorCD, String DistributorCode, String DistributorName,
                         String SalesCD, String SalesCode, String SalesName,
                         String RouteCD, String RouteCode, String RouteName,
                         String CustomerIsVisit,
                         String TimeIn, String TimeOut,
                         String CustomerSalesAmount, String CustomerSalesTP,
                         String CustomerSalesTB, String CustomerOrders, int status,
                         String customerMTD, String customer2M,
                         String customer3M, String lastVistDate,
                         String lastOrderdate, String lastOrderValue
                         )
        {
            this.CustomerCD = CustomerCD;
            this.CustomerCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerCode);
            this.CustomerName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerName);

            this.DistributorCD = DistributorCD;
            this.DistributorCode = DistributorCode;
            this.DistributorName = DistributorName;

            this.SalesCD = SalesCD;
            this.SalesCode = SalesCode;
            this.SalesName = SalesName;

            this.RouteCD = RouteCD;
            this.RouteCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteCode);
            this.RouteName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteName);

            this.LatLng = CustomerLatLng;
            this.status = status;

            this.CustomerAddress = CustomerAddress;

            this.CustomerChainCode = CustomerChainCode;

            this.CustomerIsVisit = CustomerIsVisit;
            this.processTimeInOut(TimeIn, TimeOut);
            this.CustomerSalesAmount = CustomerSalesAmount;
            this.CustomerSalesTP = CustomerSalesTP;
            this.CustomerSalesTB = CustomerSalesTB;
            this.CustomerOrders = CustomerOrders;

            this.customerMTD = customerMTD;
            this.customer2M = customer2M;
            this.customer3M = customer3M;
            this.lastVistDate = lastVistDate;
            this.lastOrderdate = lastOrderdate;
            this.lastOrderValue = lastOrderValue;

    }


        //tính toán duration (khoảng thời gian) khi bắt đầu ghé thăm(timein) và kết thúc ghé thăm (timeout)
        private void processTimeInOut(String TimeIn, String TimeOut)
        {
            this.TimeIn = TimeIn;
            this.TimeOut = TimeOut;


            try
            {
                if (this.TimeIn != "")
                {
                    dtTimeIn = DateTime.Parse(this.TimeIn);
                    this.TimeIn = dtTimeIn.ToString("hh:mm:ss tt"); //định dạng cách hiển thị
                }

            }
            catch (Exception)
            {

            }

            try
            {
                if (this.TimeOut != "")
                {
                    dtTimeOut = DateTime.Parse(this.TimeOut);
                    this.TimeOut = dtTimeOut.ToString("hh:mm:ss tt");  //định dạng cách hiển thị
                }
            }
            catch (Exception)
            {

            }

            if (this.TimeIn != "" && this.TimeOut != "")
            {
                TimeSpan span = dtTimeOut.Subtract(dtTimeIn);
                this.Duration = span.Hours.ToString() + ":" + span.Minutes.ToString() + ":" + span.Seconds.ToString();  //định dạng cách hiển thị
            }
            else
                this.Duration = "";


        }


    }
}






