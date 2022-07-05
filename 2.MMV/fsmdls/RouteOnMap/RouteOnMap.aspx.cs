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

namespace MMV.fsmdls.RouteOnMap
{
    partial class RouteOnMap : System.Web.UI.Page
    {
       
        Dictionary<String, String> menuReports = new Dictionary<String, String>();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
          
           DataTable dtLocationCountry = L5sSql.Query("SELECT NAME, VALUE FROM S_PARAMS WHERE NAME = 'LOCATION_COUNTRY'");
            if (dtLocationCountry != null && dtLocationCountry.Rows.Count > 0)
            {
                this.P5sLocaltionCountry.Value = dtLocationCountry.Rows[0]["VALUE"].ToString();
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["SaleCode"] != null && Request.QueryString["RouteCD"] != null && Request.QueryString["LatLog"] != null)
                {
                    String saleCode = checkUserProfile(Request.QueryString["SaleCode"].ToString().Trim());
                    String RouteCD = Request.QueryString["RouteCD"].ToString().Trim();
                    String saleLatLog = Request.QueryString["LatLog"].ToString().Trim();
                    string[] arrRoute = RouteCD.Split(',');

                    if (Change_routeCode_to_RouteCD(RouteCD) != "0")
                    {
                        addDropdownlist(Change_routeCode_to_RouteCD(RouteCD));

                        arrRoute = Change_routeCode_to_RouteCD(RouteCD).Split(',');
                        CustomerOnMap(saleCode, arrRoute[0], saleLatLog);
                    }else
                    {
                        addDropdownlist(RouteCD);
                        CustomerOnMap(saleCode, arrRoute[0], saleLatLog);
                    }
                   
                }
                else
                {
                    L5sJS.L5sRun("alert('No data')");
                }
            }
            
           
            
            //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "Javascript", "$(document).ready(function(){LoadMap();});", true);        
          
        }

        private string checkUserProfile(string salesCode)
        {
            string sql = "  SELECT * FROM O_HH_PROFILE WHERE USER_CODE= @0 AND ACTIVE=1";
            DataTable dt = L5sSql.Query(sql, salesCode);
            if (dt == null || dt.Rows.Count == 0)
                return salesCode;
            return dt.Rows[0]["PROFILE_CODE"].ToString();
        }

        private void addDropdownlist(String routeCD)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = String.Format(@"select ROUTE_CODE + ' - ' + ROUTE_NAME as NAME_ROUTE, ROUTE_CD from M_ROUTE where ROUTE_CD in ({0})
                            Order by ROUTE_CD ASC", routeCD);
                dt = L5sSql.Query(sql);
                ddlRoute.DataSource = dt;
                ddlRoute.DataTextField = "NAME_ROUTE";
                ddlRoute.DataValueField = "ROUTE_CD";
                ddlRoute.DataBind();
            }
            catch {
                
            }
 
        }


        private void CustomerOnMap(String saleCode,String RouteCD,String saleLatLog )
        {
             // load map first
            String sql = null;
           

            // Get Table O_TIME_IN_OUT MMVCONSOLIDATE by date
            //DateTime dateNow = DateTime.Parse(DateTime.Now.ToString(), "dd-MM-yyyy", null);
            DateTime date = DateTime.Now;
            int dateNow = date.Day;


            String sqlGetSqlTimeInOut = P5sCmmFns.P5sGetDynamicSqlTimeInTimeOut(DateTime.Now, DateTime.Now);
            String SqlTime = sqlGetSqlTimeInOut + String.Format(@"
                    where DAY(TIME_IN_CREATED_DATE) = {0} and ROUTE_CD IN({1})
                    ", dateNow, RouteCD);
            #region Get table O_TIME_IN_OUT MMV
            String sqlTITO = String.Format(@"
                              SELECT
				                   [ROUTE_CD]
                                  ,[SALES_CD]
                                  ,[DISTRIBUTOR_CD]
                                  ,[CUSTOMER_CD]
                                  ,[CUSTOMER_CODE]
                                  ,[TIME_IN_LATITUDE_LONGITUDE]
                                  ,[TIME_IN_LATITUDE_LONGITUDE_ACCURACY]
                                  ,[TIME_OUT_LATITUDE_LONGITUDE]
                                  ,[TIME_OUT_LATITUDE_LONGITUDE_ACCURACY]
                                  ,[TIME_IN_CREATED_DATE]
                                  ,[TIME_OUT_CREATED_DATE]
                                  ,[CREATED_DATE]
                                  ,[TYPE_CD]
                                  ,[TIME_IN_LOCATION_ADDRESS]
                                  ,[TIME_OUT_LOCATION_ADDRESS]
                                  ,[MAX_DATETIME_TRACKING]
                                  ,[LOCATION_IS_NULL]
                                  ,[TIME_IN_OUT_CD]
					              FROM [O_TIME_IN_OUT] WITH(NOLOCK, READUNCOMMITTED)                                                     
                       where DAY(TIME_IN_CREATED_DATE) = {0} and ROUTE_CD IN({1})
                    ", dateNow, RouteCD);
            #endregion


            DataTable dtTITO1 = L5sSql.Query(SqlTime);
            DataTable dtTITO2 = L5sSql.Query(sqlTITO);
            dtTITO1.Merge(dtTITO2);

            string[] arrListStr = saleLatLog.Split(',');
            L5sJS.L5sRun("LoadMap('" + arrListStr[0] + "','" + arrListStr[1] + "','" + saleCode + "');");
            //L5sJS.L5sRun("addListing('"+ saleLatLog + "');");


            #region get LongLat Customer
            sql = String.Format(@"
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
	                                    mr.ROUTE_NAME

                                        ,Case when omd.CUSTOMER_MTD is null then 0 else convert(int,omd.CUSTOMER_MTD) end as CUSTOMER_MTD
                                        ,Case when omd.CUSTOMER_2M is null then 0 else convert(int,omd.CUSTOMER_2M) end as CUSTOMER_2M
                                        ,Case when omd.CUSTOMER_3M is null then 0 else convert(int,omd.CUSTOMER_3M) end as CUSTOMER_3M
                                        ,Case when omd.LAST_VISIT_DATE is null then '' else convert(nvarchar,omd.LAST_VISIT_DATE) end as LAST_VISIT_DATE
                                        ,Case when omd.LAST_ORDER_DATE is null then '' else convert(nvarchar,omd.LAST_ORDER_DATE) end as LAST_ORDER_DATE
                                        ,Case when omd.LAST_ORDER_VALUE is null then 0 else convert(int,omd.LAST_ORDER_VALUE) end as LAST_ORDER_VALUE


                                    FROM  
                                    M_CUSTOMER mc 
                                    INNER JOIN O_CUSTOMER_ROUTE ocr ON mc.CUSTOMER_CD=ocr.CUSTOMER_CD AND ocr.ACTIVE=1
                                    INNER JOIN M_ROUTE mr ON ocr.ROUTE_CD=mr.ROUTE_CD 
                                    INNER JOIN O_SALES_ROUTE osr ON ocr.ROUTE_CD = osr.ROUTE_CD AND osr.ACTIVE=1
                                    INNER JOIN M_SALES ms ON osr.SALES_CD = ms.SALES_CD  
                                    INNER JOIN [M_DISTRIBUTOR] md ON ms.DISTRIBUTOR_CD=md.DISTRIBUTOR_CD
                                    left join O_CUSTOMER_MTD omd on omd.CUSTOMER_CD = mc.CUSTOMER_CD
                                    WHERE  mc.ACTIVE = 1 and
                                            ms.SALES_CD = (Select SALES_CD from M_SALES where SALES_CODE = '{0}') --- TMT tìm sale cd dựa trên sale code
                                            AND osr.ROUTE_CD IN ({1})
		                                    AND ( mc.LONGITUDE_LATITUDE != '' 
				                                    OR mc.LONGITUDE_LATITUDE IS NOT NULL)

                                 ", saleCode, RouteCD);
            #endregion
            DataTable dt = L5sSql.Query(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                L5sMsg.Show("Không có dữ liệu!");
                return;
            }

            int totalCustomer = dt.Rows.Count;

            DataView dv = new DataView(dt);

            int totalCustomerVisit = dv.ToTable().Rows.Count;

            dt.Columns.Add("status", typeof(Int32));
            dt.Columns["status"].DefaultValue = 0;

            //display number of stop visit, sales, visit sales

            int visited = 0;
            int visitedSales = 0;
            int havingSales = 0;
            int noVisited = 0;

            DataTable dtRangeValue = L5sSql.Query("SELECT NAME, VALUE FROM S_PARAMS WHERE NAME = 'RANGE_VALUE'");
            if (dtRangeValue != null && dtRangeValue.Rows.Count > 0)
            {
                this.P5sHfRange.Value = dtRangeValue.Rows[0]["VALUE"].ToString();
            }
            // Thay đổi màu Marker những KH đã TITO
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["status"] = 0;
                for (int j = 0; j < dtTITO1.Rows.Count; j++)
                {
                    if (dt.Rows[i]["CUSTOMER_CODE"].ToString() == dtTITO1.Rows[j]["CUSTOMER_CODE"].ToString())
                    {
                        dt.Rows[i]["status"] = 1;
                        break;
                    }
                }
            }
            String jsonData = this.P5sConvertDataTableCustomerJson(dt);
            // đổ json data qua javascript TMT 181127
            L5sJS.L5sRun("showShop('" + jsonData + "');");
            
        }

        public string formatDecimal(string s)
        {
            string stmp = s;
            int amount;
            amount = (int)(s.Length / 3);
            if (s.Length % 3 == 0)
                amount--;
            for (int i = 1; i <= amount; i++)
            {
                stmp = stmp.Insert(stmp.Length - 4 * i + 1, ",");
            }
            return stmp;
        }

        //  TMT 181127        
        private string P5sConvertDataTableCustomerJson(DataTable dtCustomer)
        {
            DataTable dt = dtCustomer;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            List<MCustomer> customers = new List<MCustomer>();
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

                int status = Int32.Parse(dt.Rows[i]["status"].ToString());

                String customerIsVisit = "";

                String customerTimeIn = "";  // dt.Rows[i]["TimeIn"].ToString();
                String customerTimeOut = ""; // dt.Rows[i]["TimeOut"].ToString();  


                String customerSalesAmount = "";
                String customerSalesTP = "";
                String customerSalesTB = "";
                String customerOrders = "";

               String customerMTD = formatDecimal(dt.Rows[i]["CUSTOMER_MTD"].ToString());
               String customer2M = formatDecimal(dt.Rows[i]["CUSTOMER_2M"].ToString());
               String customer3M = formatDecimal(dt.Rows[i]["CUSTOMER_3M"].ToString());
               String lastVistDate = dt.Rows[i]["LAST_VISIT_DATE"].ToString();
               String lastOrderdate = dt.Rows[i]["LAST_ORDER_DATE"].ToString();
               String lastOrderValue = formatDecimal(dt.Rows[i]["LAST_ORDER_VALUE"].ToString());

                if (latLngs != "")
                {
                    MCustomer c = new MCustomer(customerCD, customerCode, customerName, customerAddress, customerChainCode, latLngs,
                                             distributorCD, distributorCode, distributorName, salesCD, saleCode, salesName,
                                             routeCD, routeCode, routeName, customerIsVisit,
                                             customerTimeIn, customerTimeOut,
                                             customerSalesAmount, customerSalesTP, customerSalesTB, customerOrders,status,
                                             customerMTD, customer2M, customer3M, lastVistDate, lastOrderdate, lastOrderValue);

                    customers.Add(c);
                }
            }
            return oSerializer.Serialize(customers);
        }

       

        protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            String saleCode = checkUserProfile(Request.QueryString["SaleCode"].ToString().Trim());
            String saleLatLog = Request.QueryString["LatLog"].ToString().Trim();

            string routCD = ddlRoute.SelectedValue;

            CustomerOnMap(saleCode, routCD, saleLatLog);
        }

        protected string Change_routeCode_to_RouteCD(string RouteCD)
        {
            try
            {
                string sql_change = String.Format( @"SELECT STUFF((
                                            SELECT ',' + Convert(nvarchar, ROUTE_CD)
                                            FROM M_ROUTE
                                            WHERE ROUTE_CODE in ('{0}')
                                            FOR XML PATH('')
                                            ), 1, 1, '') as ROUTE_CD", RouteCD.Replace("," , "','" ));

                DataTable dt = L5sSql.Query(sql_change);
                    if (dt != null && dt.Rows[0]["ROUTE_CD"].ToString().Trim() !="")
                        return dt.Rows[0]["ROUTE_CD"].ToString().Trim();
                    else
                        return "0";
            }
            catch {
                return "0";
            }

        }
    }
 }
