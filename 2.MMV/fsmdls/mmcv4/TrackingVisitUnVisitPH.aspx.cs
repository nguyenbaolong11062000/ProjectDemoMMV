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
    partial class TrackingVisitUnVisitPH : System.Web.UI.Page
    {


        public L5sAutocomplete P5sActDistributor;
        public L5sAutocomplete P5sActSales;
        public L5sAutocomplete P5sActRegion, P5sActArea, P5sActRoute, P5sActSupervisor, P5sActASM;

        Dictionary<String, String> menuReports = new Dictionary<String, String>();


        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            this.P5sPnlSearch.Visible = false;

            DataTable dtLocationCountry = L5sSql.Query("SELECT NAME, VALUE FROM S_PARAMS WHERE NAME = 'LOCATION_COUNTRY'");
            if (dtLocationCountry != null && dtLocationCountry.Rows.Count > 0)
            {
                this.P5sLocaltionCountry.Value = dtLocationCountry.Rows[0]["VALUE"].ToString();
            }

            if (!IsPostBack)
            {
                this.P5sTxtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MM-yyyy");
                this.P5sTxtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                this.P5sInit();
            }
            
            // ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "$(document).ready(function(){LoadMap();});", true);
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "Javascript", "$(document).ready(function(){LoadMap();});", true);
            this.P5sAutoCompleteInit();

        }

        private void P5sAutoCompleteInit()
        {
            switch (P5sDdlViewOption.SelectedValue)
            {
                case "0": //sales
                    {
                        //select null
                        this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtRegionCD.ClientID, 0, true) : this.P5sActRegion;

                        this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtAreaCD.ClientID, 0, true) : this.P5sActArea;

                        this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtDistributorCD.ClientID, 0, true) : this.P5sActDistributor;

                        this.P5sActSales = this.P5sActSales == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSalesCD.ClientID, 0, true) : this.P5sActSales;

                        this.P5sActRoute = this.P5sActRoute == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtRouteCD.ClientID, 0, true) : this.P5sActRoute;

                        this.P5sActSupervisor = this.P5sActSupervisor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSupervisorCD.ClientID, 0, true) : this.P5sActSupervisor;



                        this.P5sActASM = this.P5sActASM == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtASM.ClientID, 1, true) : this.P5sActASM;



                        break;
                    }
                case "1": //sales
                    {
                        this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRegion(""), this.P5sTxtRegionCD.ClientID, 1, true, this.P5sTxtAreaCD.ClientID) : this.P5sActRegion;
                        this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(P5sCmmFns.P5sGetArea("REGION_CD"), this.P5sTxtAreaCD.ClientID, 1, true, this.P5sTxtDistributorCD.ClientID) : this.P5sActArea;
                        this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegionCD.ClientID);

                        this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistributor("AREA_CD"), this.P5sTxtDistributorCD.ClientID, 1, true, this.P5sTxtSalesCD.ClientID) : this.P5sActDistributor;
                        this.P5sActDistributor.L5sChangeFilteringId(this.P5sTxtAreaCD.ClientID);

                        this.P5sActSales = this.P5sActSales == null ? new L5sAutocomplete(P5sCmmFns.P5sGetSales("DISTRIBUTOR_CD"), this.P5sTxtSalesCD.ClientID, 0, true, this.P5sTxtRouteCD.ClientID) : this.P5sActSales;
                        this.P5sActSales.L5sChangeFilteringId(this.P5sTxtDistributorCD.ClientID);

                        this.P5sActRoute = this.P5sActRoute == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRoute("SALES_CD"), this.P5sTxtRouteCD.ClientID, 0, true) : this.P5sActRoute;
                        this.P5sActRoute.L5sChangeFilteringId(this.P5sTxtSalesCD.ClientID);

                        //select null
                        this.P5sActSupervisor = this.P5sActSupervisor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSupervisorCD.ClientID, 0, true) : this.P5sActSupervisor;



                        this.P5sActASM = this.P5sActASM == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtASM.ClientID, 1, true) : this.P5sActASM;



                        break;
                    }
                case "2": //supervisor
                    {
                        this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRegion(""), this.P5sTxtRegionCD.ClientID, 1, true, this.P5sTxtAreaCD.ClientID) : this.P5sActRegion;
                        this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(P5sCmmFns.P5sGetArea("REGION_CD"), this.P5sTxtAreaCD.ClientID, 1, true, this.P5sTxtDistributorCD.ClientID) : this.P5sActArea;
                        this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegionCD.ClientID);

                        this.P5sActSupervisor = this.P5sActSupervisor == null ? new L5sAutocomplete(P5sCmmFns.P5sGetSupervisorByArea("AREA_CD"), this.P5sTxtSupervisorCD.ClientID, 0, true, this.P5sTxtRouteCD.ClientID) : this.P5sActSupervisor;
                        this.P5sActSupervisor.L5sChangeFilteringId(this.P5sTxtAreaCD.ClientID);

                        this.P5sActRoute = this.P5sActRoute == null ? new L5sAutocomplete(P5sCmmFns.P5sGetSupervisorRoute("SUPERVISOR_CD"), this.P5sTxtRouteCD.ClientID, 0, true) : this.P5sActRoute;
                        this.P5sActRoute.L5sChangeFilteringId(this.P5sTxtSupervisorCD.ClientID);


                        //select null
                        this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtDistributorCD.ClientID, 0, true) : this.P5sActDistributor;
                        this.P5sActSales = this.P5sActSales == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSalesCD.ClientID, 0, true) : this.P5sActSales;


                        this.P5sActASM = this.P5sActASM == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtASM.ClientID, 1, true) : this.P5sActASM;

                        break;
                    }
                case "3": //asm
                    {


                        this.P5sActASM = this.P5sActASM == null ? new L5sAutocomplete(P5sCmmFns.P5sGetASM(""), this.P5sTxtASM.ClientID, 0, true) : this.P5sActASM;

                        this.P5sActRoute = this.P5sActRoute == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRoutesByASM("ASM_CD"), this.P5sTxtRouteCD.ClientID, 0, true) : this.P5sActRoute;
                        this.P5sActRoute.L5sChangeFilteringId(this.P5sTxtASM.ClientID);


                        //select null
                        this.P5sActSales = this.P5sActSales == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSalesCD.ClientID, 0, true) : this.P5sActSales;

                        this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtRegionCD.ClientID, 1, true, this.P5sTxtAreaCD.ClientID) : this.P5sActRegion;
                        this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtAreaCD.ClientID, 1, true, this.P5sTxtDistributorCD.ClientID) : this.P5sActArea;

                        this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtDistributorCD.ClientID, 1, true, this.P5sTxtSalesCD.ClientID) : this.P5sActDistributor;

                        this.P5sActSupervisor = this.P5sActSupervisor == null ? new L5sAutocomplete(L5sSql.Query(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 "), this.P5sTxtSupervisorCD.ClientID, 0, true) : this.P5sActSupervisor;
                        break;

                    }
            }
        }

        protected void P5sLbtnLoad_Click(object sender, EventArgs e)
        {
            L5sJS.L5sRun("LoadMap()"); // load map first
            String sql = null;
            String[] lsRoute = new String[selRoute.Items.Count];
            int f = 0;
            for (int i = 0; i < selRoute.Items.Count; i++)
            {

                if (selRoute.Items[i].Selected == true)
                {
                    lsRoute[f] = selRoute.Items[i].Value;
                    f++;
                }
            }
            String rout = "";
            for (int i = 0; i < f; i++)
            {
                rout += lsRoute[i];
                if (i < f - 1)
                    rout += ",";

            }
            String route = rout;
            DateTime dateExportFrom = DateTime.ParseExact(Request[P5sTxtFromDate.UniqueID], "dd-MM-yyyy", null);
            DateTime dateExportTo = DateTime.ParseExact(Request[P5sTxtToDate.UniqueID], "dd-MM-yyyy", null);
            String sqlGetSqlTimeInOut = P5sCmmFns.P5sGetDynamicSqlTimeInTimeOut(dateExportFrom, dateExportTo);

            switch ("1")
            {
                case "0":
                    {
                        L5sMsg.Show("Vui lòng chọn thông tin chức vụ.");
                        return;
                    }

                case "1":
                    {
                        //if (P5sTxtRegionCD.Text == "" || P5sTxtAreaCD.Text == "" || P5sTxtDistributorCD.Text == "" || P5sTxtSalesCD.Text == "" || P5sTxtRouteCD.Text == "")
                        //{
                        //    L5sMsg.Show("Vui lòng điền đầy đủ thông tin.");
                        //    return;
                        //}
                        //String[] dSR = new String[selDSR.Items.Count];
                        //int t = 0;
                        //for (int i = 0; i < selDSR.Items.Count; i++)
                        //{

                        //    if (selDSR.Items[i].Selected == true)
                        //    {
                        //        dSR[t] = selDSR.Items[i].Value;
                        //        t++;
                        //    }
                        //}
                        //String dsR = "";
                        //for (int i = 0; i < t; i++)
                        //{
                        //    dsR += dSR[i];
                        //    if (i < t - 1)
                        //        dsR += ",";

                        //}

                        //String sales = dsR;
                        String sales = this.P5sDDLSales.SelectedValue;
                        if (string.IsNullOrEmpty(sales))
                        {
                            sales = string.Format(@"SELECT sls.SALES_CD
                                            FROM  M_SALES sls INNER JOIN M_DISTRIBUTOR dist ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
                                            where dist.DISTRIBUTOR_CD in ({0}) and sls.ACTIVE = 1", this.P5sDDLDistributor.SelectedValue);
                        }

                        if (string.IsNullOrEmpty(route))
                        {
                            route = string.Format("select ROUTE_CD from O_SALES_ROUTE where SALES_CD in ({0})", sales);
                        }

                        #region Sql
                        sql = String.Format(@"
                                                
                                            	                                            
                                                DECLARE @TB_CustomerVisited TABLE 
                                                (
                                                    CUSTOMER_CD BIGINT
                                                    PRIMARY KEY (CUSTOMER_CD)
                                                )
                                              
                                             
                                                INSERT INTO @TB_CustomerVisited
                                                SELECT DISTINCT CUSTOMER_CD 
                                                FROM ({2}) otio
                                                WHERE CONVERT(Date, otio.TIME_IN_CREATED_DATE , 103) BETWEEN @0 AND @1						                																			
                                                       AND otio.TYPE_CD = 1 --NVBH
                                                       AND CUSTOMER_CD IS NOT NULL
             

                                                

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
                                                WHERE  CONVERT(Date, SALES_AMOUNT_DATE , 103) BETWEEN @0 AND @1	 
                                                GROUP BY CUSTOMER_CD




                                                SELECT 
                                                    md.DISTRIBUTOR_CD,
                                                    md.DISTRIBUTOR_CODE, 
                                                    md.DISTRIBUTOR_NAME, 
		                                            mc.CUSTOMER_CD , 
                                                    mc.CUSTOMER_CODE , 
                                                    mc.CUSTOMER_NAME , 
                                                    mc.CUSTOMER_CHAIN_CODE,
                                                    mc.CUSTOMER_ADDRESS,  
                                                    mc.LONGITUDE_LATITUDE,    

                                                    MS.SALES_CD, 
                                                    MS.SALES_CODE, 
                                                    'DSR: '+ MS.SALES_NAME as SALES_NAME, 
                                                    MS.SALES_CODE +'-' + MS.SALES_NAME AS SALES_DESC, 

                                                    mr.ROUTE_CD, 
                                                    mr.ROUTE_CODE, 
                                                    mr.ROUTE_NAME, 

                                                    '' AS TIME_IN_LATITUDE_LONGITUDE ,
                                                    '' AS TIME_OUT_LATITUDE_LONGITUDE ,
                                                    '' AS TIME_IN_CREATED_DATE , 
                                                    '' AS TIME_OUT_CREATED_DATE ,
                                                    '' AS Distance,
                                                    '' AS NoOfOrder,
                                                    '' AS VISIT_DURATION,
													 
													CASE 
                                                        WHEN ISNULL(visited.CUSTOMER_CD,-1) = -1 THEN 0
														ELSE 1 
													END  AS IS_VISIT, 


                                                     FORMAT(CAST( ISNULL(customerAmount.SALES_AMOUNT,0) AS INT) , '##,##0')   AS SALES_AMOUNT,
                                                     FORMAT(CAST( ISNULL(customerAmount.SALES_TP,0) AS INT) , '##,##0')   AS SALES_TP,
                                                     FORMAT(CAST( ISNULL(customerAmount.SALES_TB,0) AS INT) , '##,##0')   AS SALES_TB,
                                                     FORMAT(CAST( ISNULL(customerAmount.CUSTOMER_ORDERS,0) AS INT) , '##,##0')    AS CUSTOMER_ORDERS    

                                                    FROM  
                                                    M_CUSTOMER mc 
                                                    INNER JOIN O_CUSTOMER_ROUTE ocr ON mc.CUSTOMER_CD=ocr.CUSTOMER_CD AND ocr.ACTIVE=1
                                                    INNER JOIN M_ROUTE mr ON ocr.ROUTE_CD=mr.ROUTE_CD 
                                                    INNER JOIN O_SALES_ROUTE osr ON ocr.ROUTE_CD = osr.ROUTE_CD AND osr.ACTIVE=1
                                                    INNER JOIN M_SALES ms ON osr.SALES_CD = ms.SALES_CD  
                                                    INNER JOIN [M_DISTRIBUTOR] md ON ms.DISTRIBUTOR_CD=md.DISTRIBUTOR_CD
                                                    LEFT JOIN @TB_CustomerVisited visited ON mc.CUSTOMER_CD = visited.CUSTOMER_CD
                                                    LEFT JOIN @TB_CustomerAmount customerAmount ON customerAmount.CUSTOMER_CD = mc.CUSTOMER_CD  
		                                            WHERE  
                                                           ms.SALES_CD IN ({0}) 
                                                           AND osr.ROUTE_CD IN({1})
														   AND ( mc.LONGITUDE_LATITUDE != '' 
														          OR mc.LONGITUDE_LATITUDE IS NOT NULL )

                                 ", sales, route, sqlGetSqlTimeInOut);
                        #endregion
                        break;
                    }
                case "2":
                    {
                        if (P5sTxtRegionCD.Text == "" || P5sTxtAreaCD.Text == "" || P5sTxtSupervisorCD.Text == "" || P5sTxtRouteCD.Text == "")
                        {
                            L5sMsg.Show("Vui lòng điền đầy đủ thông tin.");
                            return;
                        }
                        String sup = this.P5sTxtSupervisorCD.Text;
                        #region Sql
                        sql = String.Format(@"
                                                DECLARE @TB_CustomerVisited TABLE 
                                                (
                                                    CUSTOMER_CD BIGINT
                                                    PRIMARY KEY (CUSTOMER_CD)
                                                )
                                              
	                                   
                                               INSERT INTO @TB_CustomerVisited
                                                SELECT DISTINCT CUSTOMER_CD 
                                                FROM ({2}) otio
                                                WHERE CONVERT(Date, otio.TIME_IN_CREATED_DATE , 103) BETWEEN @0 AND @1						                																			
                                                       AND otio.TYPE_CD = 2 --supervisor
                                                       AND CUSTOMER_CD IS NOT NULL


                                                SELECT 
                                                    md.DISTRIBUTOR_CD,
                                                    md.DISTRIBUTOR_CODE, 
                                                    md.DISTRIBUTOR_NAME, 
		                                            mc.CUSTOMER_CD , 
                                                    mc.CUSTOMER_CODE , 
                                                    mc.CUSTOMER_NAME , 
                                                    mc.CUSTOMER_CHAIN_CODE,
                                                    mc.CUSTOMER_ADDRESS,  
                                                    mc.LONGITUDE_LATITUDE,    

                                                    MS.SUPERVISOR_CD AS SALES_CD, 
                                                    MS.SUPERVISOR_CODE AS SALES_CODE, 
                                                    'CDS: '+MS.SUPERVISOR_NAME AS SALES_NAME, 
                                                    MS.SUPERVISOR_CODE +'-' + MS.SUPERVISOR_NAME AS SALES_DESC, 

                                                    route.SUPERVISOR_ROUTE_CD AS  ROUTE_CD, 
                                                    route.SUPERVISOR_ROUTE_CODE AS ROUTE_CODE, 
                                                     route.SUPERVISOR_ROUTE_NAME AS ROUTE_NAME, 

                                                    '' AS TIME_IN_LATITUDE_LONGITUDE ,
                                                    '' AS TIME_OUT_LATITUDE_LONGITUDE ,
                                                    '' AS TIME_IN_CREATED_DATE , 
                                                    '' AS TIME_OUT_CREATED_DATE ,
                                                    '' AS Distance,
                                                    '' AS NoOfOrder,
                                                    '' AS VISIT_DURATION,
													 
													CASE 
                                                        WHEN ISNULL(visited.CUSTOMER_CD,-1) = -1 THEN 0
														ELSE 1 
													END  AS IS_VISIT, 

                                                    SALES_AMOUNT = 0,
                                                    SALES_TP = 0,
                                                    SALES_TB = 0,
                                                    CUSTOMER_ORDERS = 0             

                                                    FROM  
                                                    M_CUSTOMER mc 
                                                    INNER JOIN O_CUSTOMER_SUPERVISOR_ROUTE ocr ON mc.CUSTOMER_CD=ocr.CUSTOMER_CD AND ocr.ACTIVE=1
                                                    INNER JOIN O_SUPERVISOR_SUPERVISOR_ROUTE mr ON ocr.SUPERVISOR_ROUTE_CD =mr.SUPERVISOR_ROUTE_CD  AND mr.ACTIVE=1
                                                    INNER JOIN M_SUPERVISOR ms ON mr.SUPERVISOR_CD = ms.SUPERVISOR_CD  
                                                    INNER JOIN M_SUPERVISOR_ROUTE route ON mr.SUPERVISOR_ROUTE_CD = route.SUPERVISOR_ROUTE_CD
													INNER JOIN [M_DISTRIBUTOR] md ON mc.DISTRIBUTOR_CD = md.DISTRIBUTOR_CD
                                                    LEFT JOIN @TB_CustomerVisited visited ON mc.CUSTOMER_CD = visited.CUSTOMER_CD
		                                            WHERE  
                                                           ms.SUPERVISOR_CD IN ({0}) 
                                                           AND mr.SUPERVISOR_ROUTE_CD IN({1})
														   AND mc.LONGITUDE_LATITUDE != '' 
														   AND mc.LONGITUDE_LATITUDE IS NOT NULL
                                 ", sup, route, sqlGetSqlTimeInOut);
                        #endregion
                        break;
                    }
                case "3":
                    {
                        if (P5sTxtASM.Text == "" || P5sTxtRouteCD.Text == "")
                        {
                            L5sMsg.Show("Vui lòng điền đầy đủ thông tin.");
                            return;
                        }
                        String asm = this.P5sTxtASM.Text;
                        #region Sql
                        sql = String.Format(@"

                                                DECLARE @TB_CustomerVisited TABLE 
                                                (
                                                    CUSTOMER_CD BIGINT
                                                    PRIMARY KEY (CUSTOMER_CD)
                                                )

                                                 INSERT INTO @TB_CustomerVisited
                                                SELECT DISTINCT CUSTOMER_CD 
                                                FROM ({2}) otio
                                                WHERE CONVERT(Date, otio.TIME_IN_CREATED_DATE , 103) BETWEEN @0 AND @1						                																			
                                                       AND otio.TYPE_CD = 3 --asm
                                                       AND CUSTOMER_CD IS NOT NULL

                                                SELECT 
                                                    md.DISTRIBUTOR_CD,
                                                    md.DISTRIBUTOR_CODE, 
                                                    md.DISTRIBUTOR_NAME, 
		                                            mc.CUSTOMER_CD , 
                                                    mc.CUSTOMER_CODE , 
                                                    mc.CUSTOMER_NAME , 
                                                    mc.CUSTOMER_CHAIN_CODE,
                                                    mc.CUSTOMER_ADDRESS,  
                                                    mc.LONGITUDE_LATITUDE,    

                                                    MS.ASM_CD AS SALES_CD, 
                                                    MS.ASM_CODE AS SALES_CODE, 
                                                    'ASM: '+ MS.ASM_NAME AS SALES_NAME, 
                                                    MS.ASM_CODE +'-' + MS.ASM_NAME AS SALES_DESC, 

                                                    route.ASM_ROUTE_CD AS  ROUTE_CD, 
                                                    route.ASM_ROUTE_CODE AS ROUTE_CODE, 
                                                     route.ASM_ROUTE_NAME AS ROUTE_NAME, 

                                                    '' AS TIME_IN_LATITUDE_LONGITUDE ,
                                                    '' AS TIME_OUT_LATITUDE_LONGITUDE ,
                                                    '' AS TIME_IN_CREATED_DATE , 
                                                    '' AS TIME_OUT_CREATED_DATE ,
                                                    '' AS Distance,
                                                    '' AS NoOfOrder,
                                                    '' AS VISIT_DURATION,
													 														 
													CASE 
                                                        WHEN ISNULL(visited.CUSTOMER_CD,-1) = -1 THEN 0
														ELSE 1 
													END  AS IS_VISIT, 

                                                    SALES_AMOUNT = 0,
                                                    SALES_TP = 0,
                                                    SALES_TB = 0,
                                                    CUSTOMER_ORDERS = 0                       

                                                    FROM  
                                                    M_CUSTOMER mc 
                                                    INNER JOIN O_CUSTOMER_ASM_ROUTE ocr ON mc.CUSTOMER_CD=ocr.CUSTOMER_CD AND ocr.ACTIVE=1
                                                    INNER JOIN O_ASM_ASM_ROUTE mr ON ocr.ASM_ROUTE_CD =mr.ASM_ROUTE_CD  AND mr.ACTIVE=1
                                                    INNER JOIN M_ASM ms ON mr.ASM_CD = ms.ASM_CD  
                                                    INNER JOIN M_ASM_ROUTE route ON mr.ASM_ROUTE_CD = route.ASM_ROUTE_CD
													INNER JOIN [M_DISTRIBUTOR] md ON mc.DISTRIBUTOR_CD = md.DISTRIBUTOR_CD
                                                    LEFT JOIN @TB_CustomerVisited visited ON mc.CUSTOMER_CD = visited.CUSTOMER_CD
		                                            WHERE  
                                                           ms.ASM_CD IN ({0}) 
                                                           AND mr.ASM_ROUTE_CD IN({1})
														   AND mc.LONGITUDE_LATITUDE != '' 
														   AND mc.LONGITUDE_LATITUDE IS NOT NULL
                                 ", asm, route, sqlGetSqlTimeInOut);
                        #endregion
                        break;
                    }
            }




            DataTable dt = L5sSql.Query(sql,
                                        dateExportFrom.ToString("yyyy-MM-dd"),
                                        dateExportTo.ToString("yyyy-MM-dd")
                                        );

            if (dt == null || dt.Rows.Count == 0)
            {
                L5sMsg.Show("Không có dữ liệu!");
                return;
            }

            int totalCustomer = dt.Rows.Count;

            DataView dv = new DataView(dt);
            dv.RowFilter = "IS_VISIT = '1'";
            int totalCustomerVisit = dv.ToTable().Rows.Count;

            this.P5sLblNoOfCustomer.Text = totalCustomer + "";
            this.P5sLblNoOfCustomerVisit.Text = totalCustomerVisit + "";

            this.P5sLblEffectivityCalls.Text = Math.Round(100 * ((float)totalCustomerVisit / (float)totalCustomer), 0) + " %";


            //display number of stop visit, sales, visit sales

            int visited = 0;
            int visitedSales = 0;
            int havingSales = 0;
            int noVisited = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["CUSTOMER_CODE"].ToString() == "4A0145")
                {
                    string a = "n";
                }
                if (dt.Rows[i]["IS_VISIT"].ToString().Equals("1") && !dt.Rows[i]["SALES_AMOUNT"].ToString().Equals("0"))
                    visitedSales++;
                else
                     if (dt.Rows[i]["IS_VISIT"].ToString().Equals("1"))
                    visited++;
                else
                        if (!dt.Rows[i]["SALES_AMOUNT"].ToString().Equals("0"))
                    havingSales++;
                else
                    noVisited++;
            }

            this.P5sLblVisited.Text = visited.ToString();
            this.P5sLblSales.Text = havingSales.ToString();
            this.P5sLblVisitedSales.Text = visitedSales.ToString();
            this.P5sLblNoVisit.Text = noVisited.ToString();

            DataTable dtRangeValue = L5sSql.Query("SELECT NAME, VALUE FROM S_PARAMS WHERE NAME = 'RANGE_VALUE'");
            if (dtRangeValue != null && dtRangeValue.Rows.Count > 0)
            {
                this.P5sHfRange.Value = dtRangeValue.Rows[0]["VALUE"].ToString();
            }

            String jsonData = this.P5sConvertDataTableCustomerJson(dt);
            L5sJS.L5sRun("showShop('" + jsonData + "')");
            this.P5sPnlSearch.Visible = true;

        }

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

                String customerTimeIn = "";  // dt.Rows[i]["TimeIn"].ToString();
                String customerTimeOut = ""; // dt.Rows[i]["TimeOut"].ToString();  


                String customerSalesAmount = dt.Rows[i]["SALES_AMOUNT"].ToString();
                String customerSalesTP = dt.Rows[i]["SALES_TP"].ToString();
                String customerSalesTB = dt.Rows[i]["SALES_TB"].ToString();
                String customerOrders = dt.Rows[i]["CUSTOMER_ORDERS"].ToString();

                if (latLngs != "")
                {
                    CCustomer c = new CCustomer(customerCD, customerCode, customerName, customerAddress, customerChainCode, latLngs,
                                             distributorCD, distributorCode, distributorName, salesCD, saleCode, salesName,
                                             routeCD, routeCode, routeName, customerIsVisit,
                                             customerTimeIn, customerTimeOut,
                                             customerSalesAmount, customerSalesTP, customerSalesTB, customerOrders);

                    customers.Add(c);
                }
            }
            return oSerializer.Serialize(customers);
        }


        protected void P5sLbtnTracking_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Tracking.aspx");
        }

        protected void P5sSettingAndReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Report/CustomerList.aspx");
        }

        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {
                if (this.P5sActRegion != null)
                    this.P5sActRegion.L5sSetDefaultValues(this.P5sTxtRegionCD.Text);

                if (this.P5sActArea != null)
                    this.P5sActArea.L5sSetDefaultValues(this.P5sTxtAreaCD.Text);

                if (this.P5sActDistributor != null)
                    this.P5sActDistributor.L5sSetDefaultValues(this.P5sTxtDistributorCD.Text);

                if (this.P5sActSales != null)
                    this.P5sActSales.L5sSetDefaultValues(this.P5sTxtSalesCD.Text);

                if (this.P5sActSupervisor != null)
                    this.P5sActSupervisor.L5sSetDefaultValues(this.P5sTxtSupervisorCD.Text);

                if (this.P5sActASM != null)
                    this.P5sActASM.L5sSetDefaultValues(this.P5sTxtASM.Text);


                if (this.P5sActRoute != null)
                    this.P5sActRoute.L5sSetDefaultValues(this.P5sTxtRouteCD.Text);





                this.P5sTxtFromDate.Text = Request[this.P5sTxtFromDate.UniqueID].ToString();
                this.P5sTxtToDate.Text = Request[this.P5sTxtToDate.UniqueID].ToString();



            }
            catch (Exception)
            {


            }

        }
        protected void P5sDdlViewOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.P5sTxtRegionCD.Text = "";
            this.P5sTxtAreaCD.Text = "";
            this.P5sTxtDistributorCD.Text = "";
            this.P5sTxtSalesCD.Text = "";
            this.P5sTxtSupervisorCD.Text = "";
            this.P5sTxtRouteCD.Text = "";
        }

        //Code Thêm mới 190301
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
            int SelectedIndex = 0;
            if (rows.Length > 0)
            {
                SelectedIndex = dtDistri.Rows.IndexOf(rows[0]);
            }
            string kq = dtDistri.Rows[SelectedIndex]["DISTRIBUTOR_CD"].ToString();
            P5sDDLDistributor.SelectedValue = kq;

            DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            P5sDDLSales.DataSource = dtSale;
            P5sDDLSales.DataTextField = "NAME_SALE";
            P5sDDLSales.DataValueField = "SALES_CD";
            P5sDDLSales.DataBind();
            DataRow[] rowsSale = dtSale.Select("NAME_SALE like '%VNS0005%'");
            SelectedIndex = 0;
            if (rowsSale.Length > 0)
            {
                SelectedIndex = dtSale.Rows.IndexOf(rowsSale[0]);
            }
            kq = dtSale.Rows[SelectedIndex]["SALES_CD"].ToString();
            P5sDDLSales.SelectedValue = kq;

            //DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            //selDSR.DataSource = dtSale;
            //selDSR.DataTextField = "NAME_SALE";
            //selDSR.DataValueField = "SALES_CD";
            //selDSR.DataBind();

            this.P5sAutoCompleteInit();
        }
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
            P5sDDLSales.DataSource = dtSale;
            P5sDDLSales.DataTextField = "NAME_SALE";
            P5sDDLSales.DataValueField = "SALES_CD";
            P5sDDLSales.DataBind();

            //DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            //selDSR.DataSource = dtSale;
            //selDSR.DataTextField = "NAME_SALE";
            //selDSR.DataValueField = "SALES_CD";
            //selDSR.DataBind();
        }
        protected void P5sDDLDistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            P5sDDLSales.DataSource = dtSale;
            P5sDDLSales.DataTextField = "NAME_SALE";
            P5sDDLSales.DataValueField = "SALES_CD";
            P5sDDLSales.DataBind();

            //DataTable dtSale = P5sGetSales(P5sDDLDistributor.SelectedValue.ToString());
            //selDSR.DataSource = dtSale;
            //selDSR.DataTextField = "NAME_SALE";
            //selDSR.DataValueField = "SALES_CD";
            //selDSR.DataBind();
        }

        protected void selDSR_ServerChange(object sender, EventArgs e)
        {
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
            for(int i = 0;i < f;i++)
            {
                dsR += dSR[i];
                if (i < f -1)
                    dsR += ",";
                
            }

            DataTable dtRout = P5sGetRoute(dsR);
            selRoute.DataSource = dtRout;
            selRoute.DataTextField = "Route_Name";
            selRoute.DataValueField = "ROUTE_CD";
            selRoute.DataBind();
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
        protected DataTable P5sGetArea(string p)
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

        protected DataTable P5sGetRoute(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"
                                                SELECT r.ROUTE_CD, r.ROUTE_CODE + '-' +r.ROUTE_NAME as Route_Name,'',r.ACTIVE 
                                                FROM O_SALES_ROUTE sr 
                                                INNER JOIN M_ROUTE r ON sr.ROUTE_CD = r.ROUTE_CD AND sr.ACTIVE = 1 
                                                WHERE sr.ACTIVE = 1 and sr.SALES_CD in ({0})
                                                ORDER BY ROUTE_CODE
                                                ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;

        }

    }
}






