using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using L5sDmComm;
using P5sCmm;
using P5sDmComm;
using OfficeOpenXml;
using System.IO;

namespace MMV.Tool
{
    public partial class CustomersMonthlyVisitReportAll : System.Web.UI.Page
    {
        //DSR
        public L5sAutocomplete P5sActRegion, P5sActArea;
        public L5sAutocomplete P5sActDistributor;
        public L5sAutocomplete P5sActDSR;
        public L5sAutocomplete P5sActRoute;


        //CDS & ASM
        public L5sAutocomplete P5sActRSM, P5sActASM;
        public L5sAutocomplete P5sActCDS;
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);

            if (!IsPostBack)
            {
                this.P5sTxtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                this.P5sTxtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //Double DefaultDuration = Double.Parse(L5sSql.Query("SELECT  HH_PARAM_VALUE  FROM  S_HH_PARAM WHERE HH_PARAM_KEY = 'MaximumTimeVerifyTimeInOutValid'").Rows[0]["HH_PARAM_VALUE"].ToString());
                //this.P5sTxtMinute.Text = DefaultDuration.ToString();
                this.P5sTxtMinute.Text = "0";
                this.P5sTxtSeconds.Text = "1";
            }

        }

        protected void P5sLbtnExport_Click(object sender, EventArgs e)
        {
            if (!this.P5sIsValid())
                return;

            if (string.IsNullOrEmpty(this.P5sTxtMinute.Text))
            {
                this.P5sTxtMinute.Text = "0";
            }
            if (string.IsNullOrEmpty(this.P5sTxtSeconds.Text))
            {
                this.P5sTxtSeconds.Text = "0";
            }
            DateTime dateExportFrom = DateTime.ParseExact(Request[P5sTxtFromDate.UniqueID], "dd-MM-yyyy", null);
            DateTime dateExportTo = DateTime.ParseExact(Request[P5sTxtToDate.UniqueID], "dd-MM-yyyy", null);
            String sqlGetSqlTimeInOut = P5sCmmFns.P5sGetDynamicSqlTimeInTimeOut(dateExportFrom, dateExportTo);

            String sql = String.Format(@"

                                              DECLARE @BEGIN_DAYS DATE
									          DECLARE @END_DAYS DATE
									          SET @BEGIN_DAYS = @0
									          SET @END_DAYS = @1
                                              
	                                           DECLARE @TB_TimeInOut AS TABLE
									           (
											        CUSTOMER_CD BIGINT,
											        TIME_IN_CREATED_DATE DATETIME,
											        TIME_OUT_CREATED_DATE DATETIME,
											        TIME_IN_LATITUDE_LONGITUDE NVARCHAR(128),
											        TIME_OUT_LATITUDE_LONGITUDE NVARCHAR(128),
											        PRIMARY KEY (CUSTOMER_CD, TIME_IN_CREATED_DATE, TIME_OUT_CREATED_DATE)
									           )


                                            DECLARE @DT datetime
	                                        SET @DT = EOMONTH (DATEADD(MONTH,-2,GETDATE()))
	                                        DECLARE @SysYYMMDD int
	                                        DECLARE @SysYYYYMM int
	                                        SET @SysYYMMDD = CAST( CONVERT ( nvarchar(6) , @DT, 12 )   AS INT)
	                                        SET @SysYYYYMM = YEAR(@DT)*100 + MONTH(@DT)

                                              INSERT INTO @TB_TimeInOut
							                   SELECT  CUSTOMER_CD, TIME_IN_CREATED_DATE,TIME_OUT_CREATED_DATE,TIME_IN_LATITUDE_LONGITUDE,TIME_OUT_LATITUDE_LONGITUDE FROM
							                   (
									                   SELECT  CUSTOMER_CD, TIME_IN_CREATED_DATE,TIME_OUT_CREATED_DATE,TIME_IN_LATITUDE_LONGITUDE,TIME_OUT_LATITUDE_LONGITUDE
										                ,ROW_NUMBER() OVER(PARTITION BY CUSTOMER_CD, TIME_IN_CREATED_DATE,TIME_OUT_CREATED_DATE	ORDER BY CUSTOMER_CD DESC) AS RN
									                   FROM ({2}) otio
									                   WHERE  CONVERT(Date, otio.TIME_IN_CREATED_DATE , 103) BETWEEN @BEGIN_DAYS  AND @END_DAYS
											                   AND otio.TYPE_CD = 1
                                                               AND DATEDIFF(second, otio.TIME_IN_CREATED_DATE,otio.TIME_OUT_CREATED_DATE) > = {1}
                                                               AND CUSTOMER_CD IS NOT NULL
							                   ) AS T
							                   WHERE T.RN = 1

									          

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
	                                                CUSTOMER_ADDRESS NVARCHAR(512)
                                                	
	                                               -- PRIMARY KEY (CUSTOMER_CD)
                                                )
                                                INSERT INTO @TB_CustomerInformation
                                                SELECT sls.DISTRIBUTOR_CD,sls.SALES_CD,sls.SALES_CODE, sls.SALES_NAME,
		                                                   rout.ROUTE_CD, rout.ROUTE_CODE, rout.ROUTE_NAME,
		                                                   custR.CUSTOMER_CD, cust.CUSTOMER_CODE,cust.CUSTOMER_NAME,  cust.CUSTOMER_CHAIN_CODE, cust.CUSTOMER_ADDRESS   
		                                                FROM M_SALES sls left JOIN O_SALES_ROUTE slsR ON sls.SALES_CD = slsR.SALES_CD AND slsR.ACTIVE = 1 AND sls.ACTIVE = 1
			                                                  left JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD AND rout.ACTIVE = 1
			                                                  left JOIN O_CUSTOMER_ROUTE custR ON rout.ROUTE_CD = custR.ROUTE_CD AND custR.ACTIVE = 1	
			                                                  left JOIN M_CUSTOMER cust ON custR.CUSTOMER_CD = cust.CUSTOMER_CD AND cust.ACTIVE = 1
                                                --WHERE   rout.ROUTE_CD IN ({0})                                        
            
                                    
	                                          SELECT 
                                                        dist.REGION_CODE, dist.REGION_ORDER,
                                                        dist.AREA_CODE,dist.AREA_ORDER,
                                                        dist.DISTRIBUTOR_CODE, 
                                                        dist.DISTRIBUTOR_NAME, 
                                                        cust.CUSTOMER_CD , 
                                                        cust.CUSTOMER_CODE , 
                                                        cust.CUSTOMER_NAME , 
                                                        cust.CUSTOMER_CHAIN_CODE, 
                                                        cust.SALES_CODE, 
                                                        cust.SALES_NAME, 
                                                        cust.SALES_CODE +'-' + cust.SALES_NAME AS SALES_DESC, 
                                                        cust.ROUTE_CD, 
                                                        cust.ROUTE_CODE, 
                                                        cust.ROUTE_NAME, 
                                                        otio.TIME_IN_LATITUDE_LONGITUDE ,
                                                        otio.TIME_OUT_LATITUDE_LONGITUDE ,
                                                        CONVERT(nvarchar, otio.TIME_IN_CREATED_DATE  ,108)  AS TIME_IN_CREATED_DATE , 
                                                        CONVERT(nvarchar, otio.TIME_OUT_CREATED_DATE  ,108) AS TIME_OUT_CREATED_DATE ,
                                                        DATEADD(ss,datediff(ss,TIME_IN_CREATED_DATE, TIME_OUT_CREATED_DATE),CAST('00:00:00' AS TIME))  AS DURATION,
											            CONVERT( DATE,otio.TIME_IN_CREATED_DATE,103 ) AS CREATED_DATE
                                                FROM  
                                                    @TB_TimeInOut otio   INNER JOIN   @TB_CustomerInformation cust  ON cust.CUSTOMER_CD = otio.CUSTOMER_CD                                                                                                                         
                                                    INNER JOIN dbo.ufnGetDistributorInformation() dist ON cust.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD                                               
                                                ORDER BY CREATED_DATE,REGION_ORDER,AREA_ORDER,DISTRIBUTOR_CODE,SALES_CODE,ROUTE_CODE, CUSTOMER_CODE 
                                                   
                                   

                                ", "", (Convert.ToInt16(this.P5sTxtMinute.Text) * 60 + Double.Parse(this.P5sTxtSeconds.Text)), sqlGetSqlTimeInOut);
          





            DataTable dt = L5sSql.Query(sql,
                                        dateExportFrom.ToString("yyyy-MM-dd"),
                                        dateExportTo.ToString("yyyy-MM-dd")
                                        );

            if (dt == null || dt.Rows.Count == 0)
            {
                L5sMsg.Show("No data.");
                return;
            }

            //dt



            P5sEReport P5sRpt = new P5sEReport();
            P5sRpt.P5sAdd("list", dt);
            P5sRpt.P5sAdd("fromdate", dateExportFrom.ToString("dd/MM/yyyy"));
            P5sRpt.P5sAdd("todate", dateExportTo.ToString("dd/MM/yyyy"));
            P5sRpt.P5sAdd("date", DateTime.Now.ToString("dd/MM/yyyy hh:MM"));
            P5sRpt.P5sAdd("MinuteStop", this.P5sTxtMinute.Text + ":" + this.P5sTxtSeconds.Text);

            string fileName = "CustomersMonthlyVisitReport_";
            String sourcePath = Server.MapPath("Templs/CustomersMonthlyVisitReport.xlsx");
            String resultPath = Server.MapPath("~/Exports/" + fileName + DateTime.Now.ToString("yyyyMMddhhmmssffffff") + ".xlsx");

            P5sRpt.P5sCreateReport(sourcePath, resultPath, new int[] { 1 });
            //P5sRpt.P5sAddPivot(1, 2, true);
            //P5sRpt.P5sCreatePivot();
            P5sEReport.P5sASPExportFileToClient(resultPath, "excel");


        }
        private String processTimeInOut(String TimeIn, String TimeOut)
        {
            DateTime dtTimeIn = new DateTime();
            DateTime dtTimeOut = new DateTime();
            try
            {
                if (TimeIn != "")
                {
                    dtTimeIn = DateTime.Parse(TimeIn);

                }

            }
            catch (Exception)
            {

            }

            try
            {
                if (TimeOut != "")
                {
                    dtTimeOut = DateTime.Parse(TimeOut);
                }
            }
            catch (Exception)
            {

            }

            if (TimeIn != "" && TimeOut != "")
            {
                TimeSpan span = dtTimeOut.Subtract(dtTimeIn);
                return span.Hours.ToString() + ":" + span.Minutes.ToString() + ":" + span.Seconds.ToString();
            }
            else
                return "";
        }

        
        private Boolean P5sIsValid()
        {
           // String route = this.P5sTxtROUTE_CD.Text;

            //if (route == "")
            //{
            //    L5sMsg.Show("All fields marked with an asterisk (*) are required");
            //    return false;
            //}

            return true;
        }
        protected void P5sDdlViewOption_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}