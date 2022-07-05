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
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Net;
using System.Xml;
using System.Security.Cryptography;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using System.Threading;
using P5sDmComm;
using System.Globalization;

namespace P5sCmm
{
    public class P5sCmmFns
    {

        public static void P5sInsertSalesAmount(String tmpViewState)
        {
            //UPDATE COLUMN SALES_AMOUNT_DATE
            L5sSql.Execute(@"  UPDATE TMP_IMPORT_SALES_AMOUNT SET SALES_AMOUNT_DATE = CAST ([YEAR] AS nvarchar) + '-' +  CAST ([MONTH] AS nvarchar)  + '-' +  CAST ([DAY] AS nvarchar) 
                                     WHERE IMPORT_SALES_AMOUNT_CD = @0  ;", tmpViewState);

            //UPDATE COLUMN SALES_CD
            L5sSql.Execute(@"  UPDATE slsAmount SET SALES_CD = sls.SALES_CD
                                    FROM TMP_IMPORT_SALES_AMOUNT slsAmount INNER JOIN M_SALES sls ON  slsAmount.SALES_CODE = sls.SALES_CODE                                    
                                    WHERE IMPORT_SALES_AMOUNT_CD = @0  ;", tmpViewState);

            //UPDATE COLUMN CUSTOMER_CD
            L5sSql.Execute(@"--CUSTOMER OF DISTRIBUTOR    
                           UPDATE tmp SET CUSTOMER_CD = cust.CUSTOMER_CD
                           FROM TMP_IMPORT_SALES_AMOUNT tmp INNER JOIN M_CUSTOMER cust ON tmp.CUSTOMER_CODE = cust.CUSTOMER_CODE
                           WHERE tmp.IMPORT_SALES_AMOUNT_CD = @0 ", tmpViewState);



            //DELETE SALES AMOUNT EXISTS

            L5sSql.Execute(@"  DELETE   FROM O_SALES_AMOUNT 
                                    WHERE EXISTS
                                    (
	                                    SELECT * FROM  TMP_IMPORT_SALES_AMOUNT tmp WHERE tmp.CUSTOMER_CD = O_SALES_AMOUNT.CUSTOMER_CD AND tmp.SALES_AMOUNT_DATE = O_SALES_AMOUNT.SALES_AMOUNT_DATE
	                                    AND tmp.IMPORT_SALES_AMOUNT_CD = @0 
                                    ) ;", tmpViewState);


            L5sSql.Execute(@"  INSERT INTO [dbo].[O_SALES_AMOUNT]
                                       ([CUSTOMER_CD]
                                       ,[CUSTOMER_CODE]
                                       ,[CUSTOMER_ORDERS]
                                       ,[SALES_AMOUNT]
                                       ,[SALES_TP]
                                       ,[SALES_TB]
                                       ,[SALES_AMOUNT_DATE],[SALES_CODE],[SALES_CD]
		                            )
                            SELECT [CUSTOMER_CD], [CUSTOMER_CODE],[NUMBER_OF_ORDER],[TOTAL_SALES] ,[SALES_TP],[SALES_TB],[SALES_AMOUNT_DATE],[SALES_CODE],[SALES_CD]
                            FROM TMP_IMPORT_SALES_AMOUNT
                            WHERE IMPORT_SALES_AMOUNT_CD = @0 AND CUSTOMER_CD IS NOT NULL  AND SALES_CD IS NOT NULL

     



						 --CHỐT SỐ DỮ LIỆU THEO YÊU CẦU ISSUE: 0011038
                            DECLARE @TB_CUSTOMER_CHANGE AS TABLE
                            (
	                            [CUSTOMER_CD]   BIGINT,
                                [YRMTH] INT
	                            PRIMARY KEY ([CUSTOMER_CD],[YRMTH])
                            )
							INSERT INTO @TB_CUSTOMER_CHANGE
						    SELECT CUSTOMER_CD, YEAR(SALES_AMOUNT_DATE)*100 + MONTH(SALES_AMOUNT_DATE)	  AS [YRMTH]                              
                            FROM O_SALES_AMOUNT
                            WHERE CONVERT(DATE, CREATED_DATE , 103) = CONVERT(DATE, GETDATE() , 103)
							GROUP BY CUSTOMER_CD, YEAR(SALES_AMOUNT_DATE), MONTH(SALES_AMOUNT_DATE)
			

                            --CHỐT SỐ DỮ LIỆU THEO YÊU CẦU ISSUE: 0011038
                            DECLARE @TB AS TABLE
                            (
	                            [CUSTOMER_CD]   BIGINT,
	                            [TOTAL_ORDER] INT,
                                [SALES_AMOUNT] money,
                                [SALES_TP] money,
                                [SALES_TB] money,
                                [SALES_AMOUNT_DATE] DATE,
                                [YRMTH] INT
	                            PRIMARY KEY ([CUSTOMER_CD],[YRMTH])
                            )
                            INSERT INTO @TB		   
                            SELECT CUSTOMER_CD,
	                               SUM(CUSTOMER_ORDERS) AS TOTAL_ORDER,
	                               SUM(SALES_AMOUNT) AS SALES_AMOUNT,
	                               SUM(SALES_TP) AS SALES_TP,
	                               SUM(SALES_TB) AS SALES_TB,	  
	                               CAST(CAST(YEAR(SALES_AMOUNT_DATE) AS varchar) + '-' +
	                               CAST(MONTH(SALES_AMOUNT_DATE) AS varchar) + '-' + 
	                               CAST(1 AS varchar) AS DATE) AS SALES_AMOUNT_DATE,
	                               YEAR(SALES_AMOUNT_DATE)*100 + MONTH(SALES_AMOUNT_DATE) AS YRMTH
                            FROM O_SALES_AMOUNT T
                            WHERE EXISTS (
											SELECT * FROM @TB_CUSTOMER_CHANGE T1 
											WHERE T1.CUSTOMER_CD = T.CUSTOMER_CD AND T1.[YRMTH] = YEAR(T.SALES_AMOUNT_DATE)*100 + MONTH(T.SALES_AMOUNT_DATE)
										)
                            GROUP BY CUSTOMER_CD,YEAR(SALES_AMOUNT_DATE), MONTH(SALES_AMOUNT_DATE)

                            --DELETE DATA [dbo].[ME_SALES_AMOUNT]

                            DELETE T
                            FROM [dbo].[ME_SALES_AMOUNT] AS T
                            WHERE EXISTS ( SELECT * FROM  @TB AS T1 WHERE T.[CUSTOMER_CD] = T1.[CUSTOMER_CD] AND T.[YRMTH] = T1.[YRMTH] ) 

                            -- INSERT NEW DATA
                            INSERT INTO [dbo].[ME_SALES_AMOUNT]
                                       ([CUSTOMER_CD]
                                       ,[TOTAL_ORDER]
                                       ,[SALES_AMOUNT]
                                       ,[SALES_TP]
                                       ,[SALES_TB]
                                       ,[SALES_AMOUNT_DATE]
                                       ,[YRMTH])

                            SELECT   [CUSTOMER_CD]
		                            ,[TOTAL_ORDER]
		                            ,[SALES_AMOUNT]
		                            ,[SALES_TP]
		                            ,[SALES_TB]
		                            ,[SALES_AMOUNT_DATE]
		                            ,[YRMTH]
                            FROM @TB 


                            DELETE FROM TMP_IMPORT_SALES_AMOUNT
                            WHERE IMPORT_SALES_AMOUNT_CD = @0 
                        
                             ;", tmpViewState);
        }

        public static void P5sRemoveAllFileAndDirectory()
        {
            try
            {
                String[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Exports/"));
                foreach (String file in files)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(file);
                        if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                            fi.Delete();
                    }
                    catch (Exception)
                    {

                    }

                }

                String[] directorys = Directory.GetDirectories(HttpContext.Current.Server.MapPath("~/Exports/"));
                foreach (String directory in directorys)
                {
                    try
                    {
                        DirectoryInfo dir = new DirectoryInfo(directory);
                        if (dir.LastAccessTime < DateTime.Now.AddDays(-1))
                            dir.Delete(true);
                    }
                    catch (Exception)
                    {
                    }

                }
            }
            catch (Exception)
            {

            }

        }

        public static int P5sGetWeekNumber(DateTime dtime)
        {
            String sql = String.Format(@" SELECT DATEPART(WEEK,'{0}') ", dtime.ToString("yyyy-MM-dd"));
            return int.Parse(L5sSql.Query(sql).Rows[0][0].ToString());
        }

        public static String P5sGetDynamicSqlTrackingOfSales(DateTime begin, DateTime end)
        {
            int beginYear = begin.Year;
            int beginWeek = P5sCmmFns.P5sGetWeekNumber(begin);

            int endYear = end.Year;
            int endWeek = P5sCmmFns.P5sGetWeekNumber(end);

            String sql = @" SELECT * 
                            FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE]
                            WHERE YEAR*100 + WEEK BETWEEN @0 AND @1 AND TYPE_CD = 1
                          ";
            DataTable dt = L5sSql.Query(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);

            if (dt == null || dt.Rows.Count <= 0)
            {
                return String.Format(@"
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[DE_TRACKING_OF_SALES_2014_01]                           
                      ");
            }
            String result = @"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    result += String.Format(@"
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]                           
                      ", dt.Rows[i]["TABLE_NAME_2"].ToString());
                }
                else
                {
                    result += String.Format(@"
                             
                              UNION ALL
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] 
                            
                      ", dt.Rows[i]["TABLE_NAME_2"].ToString());
                }
            }

            return result;
        }

        public static String P5sGetDynamicSqlTrackingOfStopOfSales(DateTime begin, DateTime end)
        {
            int beginYear = begin.Year;
            int beginWeek = P5sCmmFns.P5sGetWeekNumber(begin);

            int endYear = end.Year;
            int endWeek = P5sCmmFns.P5sGetWeekNumber(end);

            String sql = @" SELECT * 
                            FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE]
                            WHERE YEAR*100 + WEEK BETWEEN @0 AND @1 AND TYPE_CD = 1
                          ";
            DataTable dt = L5sSql.Query(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);

            if (dt == null || dt.Rows.Count <= 0)
            {
                return String.Format(@"
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[DE_TRACKING_STOP_OF_SALES_2014_01]                           
                      ");

            }
            String result = @"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    result += String.Format(@"
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]                           
                      ", dt.Rows[i]["TABLE_NAME_3"].ToString());
                }
                else
                {
                    result += String.Format(@"
                             
                              UNION ALL
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] 
                            
                      ", dt.Rows[i]["TABLE_NAME_3"].ToString());
                }
            }

            return result;
        }

        public static String P5sGetDynamicSqlTrackingOfStopOfSupervisor(DateTime begin, DateTime end)
        {
            int beginYear = begin.Year;
            int beginWeek = P5sCmmFns.P5sGetWeekNumber(begin);

            int endYear = end.Year;
            int endWeek = P5sCmmFns.P5sGetWeekNumber(end);

            String sql = @" SELECT * 
                            FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE]
                            WHERE YEAR*100 + WEEK BETWEEN @0 AND @1 AND TYPE_CD = 2
                          ";
            DataTable dt = L5sSql.Query(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);

            if (dt == null || dt.Rows.Count <= 0)
            {
                return String.Format(@"
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[DE_TRACKING_STOP_OF_SUPERVISOR_2014_01]                           
                      ");
            }
            String result = @"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    result += String.Format(@"
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]                           
                      ", dt.Rows[i]["TABLE_NAME_3"].ToString());
                }
                else
                {
                    result += String.Format(@"
                             
                              UNION ALL
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] 
                            
                      ", dt.Rows[i]["TABLE_NAME_3"].ToString());
                }
            }

            return result;
        }

        public static String P5sGetDynamicSqlTrackingOfSupervisor(DateTime begin, DateTime end)
        {
            int beginYear = begin.Year;
            int beginWeek = P5sCmmFns.P5sGetWeekNumber(begin);

            int endYear = end.Year;
            int endWeek = P5sCmmFns.P5sGetWeekNumber(end);

            String sql = @" SELECT * 
                            FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE]
                            WHERE YEAR*100 + WEEK BETWEEN @0 AND @1 AND TYPE_CD = 2
                          ";
            DataTable dt = L5sSql.Query(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);

            if (dt == null || dt.Rows.Count <= 0)
            {
                return String.Format(@"
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[DE_TRACKING_OF_SUPERVISOR_2014_01]                           
                      ");
            }
            String result = @"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    result += String.Format(@"
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]                           
                      ", dt.Rows[i]["TABLE_NAME_2"].ToString());
                }
                else
                {
                    result += String.Format(@"
                             
                              UNION ALL
                              SELECT
				                   [YYMMDD]
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
					              ,[LOCATION_ADDRESS] 
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] 
                            
                      ", dt.Rows[i]["TABLE_NAME_2"].ToString());
                }
            }

            return result;
        }

        public static String P5sGetDynamicSqlTimeInTimeOut(DateTime begin, DateTime end)
        {
            int beginYear = begin.Year;
            int beginWeek = P5sCmmFns.P5sGetWeekNumber(begin);


            int endYear = end.Year;
            int endWeek = P5sCmmFns.P5sGetWeekNumber(end);

            String sql = @" SELECT * 
                            FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] WITH(NOLOCK, READUNCOMMITTED) 
                            WHERE YEAR*100 + WEEK BETWEEN '{0}' AND '{1}' AND TYPE_CD = 3
                          ";
            sql = String.Format(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);
            // DataTable dt = L5sSql.Query(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);
            DataTable dt = L5sSql.Query(sql);

            if (dt == null || dt.Rows.Count <= 0)
            {

                return String.Format(@"
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
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TIME_IN_OUT_2014_01] WITH(NOLOCK, READUNCOMMITTED)                           
                      ");

            }
            String result = @"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    result += String.Format(@"
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
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] WITH(NOLOCK, READUNCOMMITTED)                                                     
                      ", dt.Rows[i]["TABLE_NAME_1"].ToString());
                }
                else
                {
                    result += String.Format(@"
                             
                              UNION ALL
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
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] WITH(NOLOCK, READUNCOMMITTED) 
                            
                      ", dt.Rows[i]["TABLE_NAME_1"].ToString());
                }
            }

            return result;
        }


        public static String P5sGetDynamicSqlTrackingOfStopOfASM(DateTime begin, DateTime end)
        {
            int beginYear = begin.Year;
            int beginWeek = P5sCmmFns.P5sGetWeekNumber(begin);

            int endYear = end.Year;
            int endWeek = P5sCmmFns.P5sGetWeekNumber(end);

            String sql = @" SELECT * 
                            FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE]
                            WHERE YEAR*100 + WEEK BETWEEN @0 AND @1 AND TYPE_CD = 4
                          ";
            DataTable dt = L5sSql.Query(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);

            if (dt == null || dt.Rows.Count <= 0)
            {
                return String.Format(@"
                              SELECT
				                  [YYMMDD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[DE_TRACKING_STOP_OF_SALES_2014_01]                           
                      ");
            }
            String result = @"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    result += String.Format(@"
                              SELECT
				                  [YYMMDD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]                           
                      ", dt.Rows[i]["TABLE_NAME_3"].ToString());
                }
                else
                {
                    result += String.Format(@"
                             
                              UNION ALL
                              SELECT
				                  [YYMMDD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] 
                            
                      ", dt.Rows[i]["TABLE_NAME_3"].ToString());
                }
            }

            return result;
        }

        public static String P5sGetDynamicSqlTrackingOfASM(DateTime begin, DateTime end)
        {
            int beginYear = begin.Year;
            int beginWeek = P5sCmmFns.P5sGetWeekNumber(begin);

            int endYear = end.Year;
            int endWeek = P5sCmmFns.P5sGetWeekNumber(end);

            String sql = @" SELECT * 
                            FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE]
                            WHERE YEAR*100 + WEEK BETWEEN @0 AND @1 AND TYPE_CD = 4
                          ";
            DataTable dt = L5sSql.Query(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);

            if (dt == null || dt.Rows.Count <= 0)
            {
                return String.Format(@"
                              SELECT
				                   [YYMMDD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[DE_TRACKING_OF_SUPERVISOR_2014_01]                           
                      ");
            }
            String result = @"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    result += String.Format(@"
                              SELECT
				                   [YYMMDD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]                           
                      ", dt.Rows[i]["TABLE_NAME_2"].ToString());
                }
                else
                {
                    result += String.Format(@"
                             
                              UNION ALL
                              SELECT
				                   [YYMMDD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] 
                            
                      ", dt.Rows[i]["TABLE_NAME_2"].ToString());
                }
            }

            return result;
        }

        public static String P5sGetDynamicSqlTrackingOfRSM(DateTime begin, DateTime end)
        {
            int beginYear = begin.Year;
            int beginWeek = P5sCmmFns.P5sGetWeekNumber(begin);

            int endYear = end.Year;
            int endWeek = P5sCmmFns.P5sGetWeekNumber(end);

            String sql = @" SELECT * 
                            FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE]
                            WHERE YEAR*100 + WEEK BETWEEN @0 AND @1 AND TYPE_CD = 5
                          ";
            DataTable dt = L5sSql.Query(sql, beginYear * 100 + beginWeek, endYear * 100 + endWeek);

            if (dt == null || dt.Rows.Count <= 0)
            {
                return String.Format(@"
                              SELECT
				                   [YYMMDD]
                                  ,[RSM_CD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[DE_TRACKING_OF_SUPERVISOR_2014_01]                           
                      ");
            }
            String result = @"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    result += String.Format(@"
                              SELECT
				                   [YYMMDD]
                                  ,[RSM_CD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]                           
                      ", dt.Rows[i]["TABLE_NAME_2"].ToString());
                }
                else
                {
                    result += String.Format(@"
                             
                              UNION ALL
                              SELECT
				                   [YYMMDD]
                                  ,[RSM_CD]
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
                                  ,[LOCATION_ADDRESS]
					              FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}] 
                            
                      ", dt.Rows[i]["TABLE_NAME_2"].ToString());
                }
            }

            return result;
        }

        public static String P5sReplaceInVaildJson(String str)
        {
            return str.Replace("\b", "").Replace("\f", "").Replace("\n", "").Replace("\r", "").Replace("\"", "").Replace("\t", "").Replace("\\", "");
        }

        public static string P5sConvertDataTableToJSONString(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        public static void P5sRemoveTrackingError()
        {
            String sql = String.Format(@" 
                                             DECLARE @TIMEZONE BIGINT

                                            SELECT @TIMEZONE = ISNULL(VALUE,0)
                                            FROM S_PARAMS
                                            WHERE NAME = 'TIME_ZONE'
                                        
                                            --INSERT VALUE ERROR    -- SALES                                            
                                            INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[ERR_TRACKING]
                                                               ([SALES_CD]
                                                               ,[DISTRIBUTOR_CD]
                                                               ,[YYMMDD]
                                                               ,[LONGITUDE_LATITUDE]
                                                               ,[TRACKING_DATETIME]
                                                               ,[BATTERY_PERCENTAGE]
                                                               ,[DEVICE_STATUS]
                                                               ,[NO_REPEAT]
                                                               ,[NO_ORDER]
                                                               ,[CREATED_DATE]
                                                               ,[BEGIN_DATETIME]
                                                               ,[END_DATETIME]
                                                               ,[TRACKING_ACCURACY]
                                                               ,[TRACKING_PROVIDER]
                                                               ,[DURATION]
                                                               ,[ACTIVE]
                                                               ,[BATTERY_PERCENTAGE_START]
                                                               ,[BATTERY_PERCENTAGE_END]
                                                               ,[TYPE_CD])                                            
                                            SELECT 
                                                   [SALES_CD]
                                                  ,[DISTRIBUTOR_CD]
                                                  ,[YYMMDD]
                                                  ,[LONGITUDE_LATITUDE]
                                                  ,[TRACKING_DATETIME]
                                                  ,[BATTERY_PERCENTAGE]
                                                  ,[DEVICE_STATUS]
                                                  ,[NO_REPEAT]
                                                  ,[NO_ORDER]
                                                  ,[CREATED_DATE]
                                                  ,[BEGIN_DATETIME]
                                                  ,[END_DATETIME]
                                                  ,[TRACKING_ACCURACY]
                                                  ,[TRACKING_PROVIDER]
                                                  ,[DURATION]
                                                  ,[ACTIVE]
                                                  ,[BATTERY_PERCENTAGE_START]
                                                  ,[BATTERY_PERCENTAGE_END]
                                                  ,[TYPE_CD]
                                            FROM O_TRACKING
                                            WHERE   YYMMDD > 500000 OR YYMMDD >  CONVERT ( nvarchar(6) ,GETDATE()    , 12 ) OR TRACKING_DATETIME > DATEADD(MINUTE,30,DATEADD(HOUR, @TIMEZONE, CREATED_DATE) ) -- năm 2050 lứn hơn có nhgia thoi gian pda bi sai, hoặc lớn hơn ngày hiện tại
                                                    OR YYMMDD < 150101 OR BEGIN_DATETIME IS NULL
                                                     OR  LONGITUDE_LATITUDE IS NULL OR YYMMDD IS NULL
                                            DELETE
                                            FROM O_TRACKING
                                            WHERE   YYMMDD > 500000 OR YYMMDD >  CONVERT ( nvarchar(6) , GETDATE()   , 12 ) OR TRACKING_DATETIME > DATEADD(MINUTE,30,DATEADD(HOUR, @TIMEZONE, CREATED_DATE) )   -- năm 2050 lứn hơn có nhgia thoi gian pda bi sai , hoặc lớn hơn ngày hiện tại
                                                  OR YYMMDD < 150101 OR BEGIN_DATETIME IS NULL OR  LONGITUDE_LATITUDE IS NULL OR YYMMDD IS NULL

                                        --INSERT VALUE ERROR    -- SUP                                            
                                            INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[ERR_TRACKING]
                                                               ([SALES_CD]
                                                               ,[DISTRIBUTOR_CD]
                                                               ,[YYMMDD]
                                                               ,[LONGITUDE_LATITUDE]
                                                               ,[TRACKING_DATETIME]
                                                               ,[BATTERY_PERCENTAGE]
                                                               ,[DEVICE_STATUS]
                                                               ,[NO_REPEAT]
                                                               ,[NO_ORDER]
                                                               ,[CREATED_DATE]
                                                               ,[BEGIN_DATETIME]
                                                               ,[END_DATETIME]
                                                               ,[TRACKING_ACCURACY]
                                                               ,[TRACKING_PROVIDER]
                                                               ,[DURATION]
                                                               ,[ACTIVE]
                                                               ,[BATTERY_PERCENTAGE_START]
                                                               ,[BATTERY_PERCENTAGE_END]
                                                               ,[TYPE_CD])                                            
                                            SELECT 
                                                   [SUPERVISOR_CD]
                                                  ,[DISTRIBUTOR_CD]
                                                  ,[YYMMDD]
                                                  ,[LONGITUDE_LATITUDE]
                                                  ,[TRACKING_DATETIME]
                                                  ,[BATTERY_PERCENTAGE]
                                                  ,[DEVICE_STATUS]
                                                  ,[NO_REPEAT]
                                                  ,[NO_ORDER]
                                                  ,[CREATED_DATE]
                                                  ,[BEGIN_DATETIME]
                                                  ,[END_DATETIME]
                                                  ,[TRACKING_ACCURACY]
                                                  ,[TRACKING_PROVIDER]
                                                  ,[DURATION]
                                                  ,[ACTIVE]
                                                  ,[BATTERY_PERCENTAGE_START]
                                                  ,[BATTERY_PERCENTAGE_END]
                                                  ,[TYPE_CD]
                                            FROM [dbo].[O_TRACKING_SUPERVISOR]
                                            WHERE   YYMMDD > 500000 OR YYMMDD >  CONVERT ( nvarchar(6) , GETDATE()    , 12 ) OR TRACKING_DATETIME > DATEADD(MINUTE,30,DATEADD(HOUR, @TIMEZONE, CREATED_DATE) ) -- năm 2050 lứn hơn có nhgia thoi gian pda bi sai
                                                     OR YYMMDD < 150101  OR BEGIN_DATETIME IS NULL OR  LONGITUDE_LATITUDE IS NULL
                                                      OR YYMMDD IS NULL
                                            DELETE
                                            FROM [dbo].[O_TRACKING_SUPERVISOR]
                                            WHERE   YYMMDD > 500000 OR YYMMDD >  CONVERT ( nvarchar(6) ,  GETDATE()   , 12 ) OR TRACKING_DATETIME > DATEADD(MINUTE,30,DATEADD(HOUR, @TIMEZONE, CREATED_DATE) ) -- năm 2050 lứn hơn có nhgia thoi gian pda bi sai
                                                    OR YYMMDD < 150101 OR BEGIN_DATETIME IS NULL OR  LONGITUDE_LATITUDE IS NULL
                                                                    OR YYMMDD IS NULL


                                        --INSERT VALUE ERROR    -- ASM                                            
                                            INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[ERR_TRACKING]
                                                               ([SALES_CD]
                                                               ,[DISTRIBUTOR_CD]
                                                               ,[YYMMDD]
                                                               ,[LONGITUDE_LATITUDE]
                                                               ,[TRACKING_DATETIME]
                                                               ,[BATTERY_PERCENTAGE]
                                                               ,[DEVICE_STATUS]
                                                               ,[NO_REPEAT]
                                                               ,[NO_ORDER]
                                                               ,[CREATED_DATE]
                                                               ,[BEGIN_DATETIME]
                                                               ,[END_DATETIME]
                                                               ,[TRACKING_ACCURACY]
                                                               ,[TRACKING_PROVIDER]
                                                               ,[DURATION]
                                                               ,[ACTIVE]
                                                               ,[BATTERY_PERCENTAGE_START]
                                                               ,[BATTERY_PERCENTAGE_END]
                                                               ,[TYPE_CD])                                            
                                            SELECT 
                                                   [ASM_CD]
                                                  ,[DISTRIBUTOR_CD]
                                                  ,[YYMMDD]
                                                  ,[LONGITUDE_LATITUDE]
                                                  ,[TRACKING_DATETIME]
                                                  ,[BATTERY_PERCENTAGE]
                                                  ,[DEVICE_STATUS]
                                                  ,[NO_REPEAT]
                                                  ,[NO_ORDER]
                                                  ,[CREATED_DATE]
                                                  ,[BEGIN_DATETIME]
                                                  ,[END_DATETIME]
                                                  ,[TRACKING_ACCURACY]
                                                  ,[TRACKING_PROVIDER]
                                                  ,[DURATION]
                                                  ,[ACTIVE]
                                                  ,[BATTERY_PERCENTAGE_START]
                                                  ,[BATTERY_PERCENTAGE_END]
                                                  ,[TYPE_CD]
                                            FROM [dbo].[O_TRACKING_ASM]
                                            WHERE   YYMMDD > 500000 OR YYMMDD >  CONVERT ( nvarchar(6) ,  GETDATE()  , 12 ) OR TRACKING_DATETIME > DATEADD(MINUTE,30,DATEADD(HOUR, @TIMEZONE, CREATED_DATE) ) -- năm 2050 lứn hơn có nhgia thoi gian pda bi sai
                                                    OR YYMMDD < 150101 OR BEGIN_DATETIME IS NULL OR  LONGITUDE_LATITUDE IS NULL OR YYMMDD IS NULL
                                            DELETE
                                            FROM [dbo].[O_TRACKING_ASM]
                                            WHERE   YYMMDD > 500000 OR YYMMDD >  CONVERT ( nvarchar(6) , GETDATE()  , 12 ) OR TRACKING_DATETIME > DATEADD(MINUTE,30,DATEADD(HOUR, @TIMEZONE, CREATED_DATE) ) -- năm 2050 lứn hơn có nhgia thoi gian pda bi sai
                                                OR YYMMDD < 150101 OR BEGIN_DATETIME IS NULL OR  LONGITUDE_LATITUDE IS NULL OR YYMMDD IS NULL
                                ");
            L5sSql.Execute(sql);

        }

        public static void P5sConsolidate()
        {

            //chạy script remove các dữ liệu bị inactive của các bảng O_CUSTOMER_ROUTE,O_SALES_ROUTE qua bảng history
            //mục đích là tăng tốc độ hệ thống khi truy vấn dữ liệu
            P5sCmm.P5sCmmFns.P5sRemoveDataInactive(); // move các dữ liệu O_CUSTOMER_ROUTE,O_SALES_ROUTE


            //xóa các dữ liệu không hợp lệ về thời gian trước khi thực hiện chốt số
            P5sCmm.P5sCmmFns.P5sRemoveTrackingError(); // remove dữ liệu bị lỗi do sai thời gian

            //xóa các dữ liệu bị duplicate do lỗi Server
            P5sCmm.P5sCmmFns.P5sRemoveDuplicateTracking(); //remove duplicate data


            //update dữ liệu cho giám sát, NVBH
            //Mục đích: để tăng tốc độ chốt số hệ thống thì chôt số sẽ xử dụng dữ liệu ở bảng O_TRACKING_SYNC
            //nếu NVBH,ASM,CDS chưa tồn tại thì thêm mới
            //Nếu thông tin nào có trạng thái Active = 1 thì sẽ tiến hành chạy lại chốt số cho NVBH đó theo ngày

            L5sSql.Execute(@"  


                            DECLARE @TB AS TABLE
                            (
                                SALES_CD BIGINT,
                                YYMMDD INT,
                                TYPE_CD  INT        
                            )

                            INSERT INTO @TB                            
                            SELECT DISTINCT SALES_CD,YYMMDD,1 AS  [TYPE_CD]
                            FROM O_TRACKING
                            UNION ALL
                            SELECT DISTINCT SUPERVISOR_CD,YYMMDD,2  AS  [TYPE_CD]
                            FROM O_TRACKING_SUPERVISOR
                            UNION ALL
                            SELECT DISTINCT ASM_CD,YYMMDD,4  AS  [TYPE_CD]
                            FROM O_TRACKING_ASM
                            UNION ALL
                            SELECT DISTINCT RSM_CD,YYMMDD,5  AS  [TYPE_CD]
                            FROM O_TRACKING_RSM
                           
                           
                            INSERT INTO [dbo].[O_TRACKING_SYNC]
                                       ([OBJECT_CD]  ,[YYMMDD] ,[TYPE_CD]     )
                            SELECT      
                                        SALES_CD
                                       ,[YYMMDD]
                                       ,[TYPE_CD]
                            FROM @TB T
                            WHERE NOT EXISTS 
                                    (   SELECT * 
                                        FROM [dbo].[O_TRACKING_SYNC] sync 
                                        WHERE sync.OBJECT_CD = T.SALES_CD AND T.YYMMDD = sync.YYMMDD AND T.TYPE_CD = sync.TYPE_CD
									
                                    )

                            
                            UPDATE sync  SET ACTIVE = 1
                            FROM  [dbo].[O_TRACKING_SYNC] AS sync
                            WHERE  EXISTS 
                                    (   SELECT * 
                                        FROM @TB T 
                                        WHERE sync.OBJECT_CD = T.SALES_CD AND T.YYMMDD = sync.YYMMDD AND T.TYPE_CD = sync.TYPE_CD
										AND sync.ACTIVE = 0
                                    )
                    
                            
                            --cập nhật lại DEVICE_STATUS - ghi nhận thông tin chưa đúng ở HH
                            UPDATE O_TRACKING SET DEVICE_STATUS =  REPLACE(REPLACE(DEVICE_STATUS,'GPS turn off,',''),'GPS turn on,','') 
                        
                            UPDATE O_TRACKING_SUPERVISOR SET DEVICE_STATUS =  REPLACE(REPLACE(DEVICE_STATUS,'GPS turn off,',''),'GPS turn on,','') 
                         
                            UPDATE O_TRACKING_ASM SET DEVICE_STATUS =  REPLACE(REPLACE(DEVICE_STATUS,'GPS turn off,',''),'GPS turn on,','') 

                            UPDATE O_TRACKING_RSM SET DEVICE_STATUS =  REPLACE(REPLACE(DEVICE_STATUS,'GPS turn off,',''),'GPS turn on,','') 

                                
                            --xóa dữ liệu trong các bảng tạm để tăng tốc độ hệ thống
                            DECLARE @CURRENT_DATE DATE
                            SET @CURRENT_DATE = GETDATE()

                            DELETE  FROM TMP_DE_TRACKING_OF_SALES
                            WHERE CREATED_DATE <  @CURRENT_DATE

                    
                            DELETE  FROM TMP_DE_TRACKING_OF_SUPERVISOR
                            WHERE CREATED_DATE <  @CURRENT_DATE

                            DELETE  FROM TMP_DE_TRACKING_OF_ASM
                            WHERE CREATED_DATE <  @CURRENT_DATE

                            DELETE  FROM TMP_DE_TRACKING_OF_RSM
                            WHERE CREATED_DATE <  @CURRENT_DATE
                        ");


            //thực hiện chốt số cho NVBH (cần xem chi tiết)
            P5sCmm.P5sCmmFns.P5sConsolicateTrackingSales();

            //thực hiện chốt số cho Giám sát (giống NVBH nên sẽ không giải thích code)
            P5sCmm.P5sCmmFns.P5sConsolicateTrackingSupervisor();

            //thực hiện chốt số cho ASM (giống NVBH nên sẽ không giải thích code)
            P5sCmm.P5sCmmFns.P5sConsolicateTrackingASM();

            //200103 - Thêm hàm chốt số cho RSM
            P5sCmm.P5sCmmFns.P5sConsolicateTrackingRSM();

            //thực hiện chốt số cho dữ liệu Timeinout
            P5sCmm.P5sCmmFns.P5sConsolicateTimeIntOut();

            //cập nhật doanh số mtd cho customer
            P5sCmm.P5sCmmFns.P5sConsolidateCustomerMTD();

        }

        private static void P5sConsolicateTrackingRSM()
        {
            #region script move data
            String sql = @"         
                        DECLARE @TIMEZONE BIGINT

                        SELECT @TIMEZONE = ISNULL(VALUE,0)
                        FROM S_PARAMS
                        WHERE NAME = 'TIME_ZONE' 
                 
                            DECLARE @YYMMDD INT
                            DECLARE @TB_YYMMDD AS TABLE
                            (
	                            YYMMDD BIGINT
                            )

                            INSERT INTO @TB_YYMMDD
                            SELECT DISTINCT YYMMDD 
                            FROM O_TRACKING_RSM


                            DECLARE myCursor CURSOR FOR  
                            SELECT * 
                            FROM @TB_YYMMDD
                            WHERE YYMMDD IS NOT NULL AND YYMMDD >= 160101
                            ORDER BY YYMMDD

                            OPEN myCursor   
                            FETCH NEXT FROM myCursor INTO @YYMMDD   

                            WHILE @@FETCH_STATUS = 0   
                            BEGIN   
	                              --MOVE DATA
                            	
	                              DECLARE @DATE DATE
	                              DECLARE @WEEK INT
	                              DECLARE @Str NVARCHAR(MAX)


	                              SET @DATE =   CONVERT(DATE, CAST( @YYMMDD AS NVARCHAR) ) 	           
	                              SET @WEEK = DATEPART(WEEK, @DATE) 
	 

	           
	  SET @str = '
					BEGIN TRY 
					INSERT INTO  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TRACKING_RSM_RAW_DATA_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +']
									(   [RSM_CD]
									   ,[DISTRIBUTOR_CD]
									   ,[YYMMDD]
									   ,[LONGITUDE_LATITUDE]
									   ,[TRACKING_DATETIME]
									   ,[BATTERY_PERCENTAGE]
									   ,[DEVICE_STATUS]
									   ,[NO_REPEAT]
									   ,[NO_ORDER]
									   ,[CREATED_DATE]
									   ,[BEGIN_DATETIME]
									   ,[END_DATETIME]
									   ,[TRACKING_ACCURACY]
									   ,[TRACKING_PROVIDER]
									   ,[DURATION]
									   ,[ACTIVE]
									   ,[BATTERY_PERCENTAGE_START]
									   ,[BATTERY_PERCENTAGE_END]
									   ,[TYPE_CD]
									   ,[IS_SYNC]
									   ,[LOCATION_ADDRESS])

							  SELECT 
									   [RSM_CD]
									   ,[DISTRIBUTOR_CD]
									   ,[YYMMDD]
									   ,[LONGITUDE_LATITUDE]
									   ,[TRACKING_DATETIME]
									   ,[BATTERY_PERCENTAGE]
									   ,[DEVICE_STATUS]
									   ,[NO_REPEAT]
									   ,[NO_ORDER]
									   ,[CREATED_DATE]
									   ,[BEGIN_DATETIME]
									   ,[END_DATETIME]
									   ,[TRACKING_ACCURACY]
									   ,[TRACKING_PROVIDER]
									   ,[DURATION]
									   ,[ACTIVE]
									   ,[BATTERY_PERCENTAGE_START]
									   ,[BATTERY_PERCENTAGE_END]
									   ,[TYPE_CD]
									   ,[IS_SYNC] = 1
									   ,[LOCATION_ADDRESS]
						     FROM O_TRACKING_RSM
						     WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TRACKING_DATETIME <= DATEADD(MINUTE,30,DATEADD(HOUR,'+CONVERT(NVARCHAR, @TIMEZONE)+', CREATED_DATE) )  AND BEGIN_DATETIME IS NOT NULL
                                                     AND LONGITUDE_LATITUDE IS NOT NULL 
							  					
							 UPDATE [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] SET IS_SYNC = 1
							 WHERE TABLE_NAME_1 = ''O_TRACKING_RSM_RAW_DATA_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +'''
							
							 --REMOVE ALL DATA 
	 					     DELETE FROM O_TRACKING_RSM
						     WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TRACKING_DATETIME <= DATEADD(MINUTE,30,DATEADD(HOUR,'+CONVERT(NVARCHAR, @TIMEZONE)+', CREATED_DATE) )  AND BEGIN_DATETIME IS NOT NULL
                                                     AND LONGITUDE_LATITUDE IS NOT NULL 
					END TRY  

					BEGIN CATCH  
     
					END CATCH  
			 
			      '
                  EXECUTE(@str)

                  FETCH NEXT FROM myCursor INTO @YYMMDD   
            END   

            CLOSE myCursor   
            DEALLOCATE myCursor

                        ";


            L5sSql.Execute(sql);
            #endregion



            //consolidate tracking of sales
            DataTable dt = L5sSql.Query(@"
                                              SELECT DISTINCT T1.OBJECT_CD AS RSM_CD , T1.YYMMDD,T2.TABLE_NAME_1,T2.TABLE_NAME_2,T2.TABLE_NAME_3
                                              FROM [dbo].[O_TRACKING_SYNC] T1 INNER JOIN [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] T2 ON
		                                                DATEPART(WEEK, CONVERT(DATE, CAST( T1.YYMMDD AS NVARCHAR) ))  = T2.WEEK AND
		                                                DATEPART(YEAR, CONVERT(DATE, CAST( T1.YYMMDD AS NVARCHAR) ))  = T2.YEAR AND
		                                              T1.TYPE_CD = T2.TYPE_CD
                                               WHERE T1.TYPE_CD = 5 AND T1.ACTIVE = 1
                                                ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable row = dt.Clone();
                row.ImportRow(dt.Rows[i]);
                P5sCmm.P5sCmmFns.P5sConsolidateTrackingOfRSM(row, dt.Rows[i]["RSM_CD"].ToString(), dt.Rows[i]["YYMMDD"].ToString());
                L5sSql.Execute(@"UPDATE [dbo].[O_TRACKING_SYNC] SET ACTIVE = 0 WHERE OBJECT_CD = @0 AND YYMMDD = @1 AND TYPE_CD = 5", dt.Rows[i]["RSM_CD"].ToString(), dt.Rows[i]["YYMMDD"].ToString());

            }
        }

        private static void P5sConsolidateCustomerMTD()
        {
            String sql = @"DECLARE @Date_Start datetime
		                    DECLARE @Date_End datetime
		                    DECLARE @Date_Start_3M datetime
		
		                    set @Date_Start = DATEADD(MONTH,-2,GETDATE())
		                    set @Date_Start = DATEADD(DAY,-(DAY(@Date_Start)-1),@Date_Start)

		                    set @Date_End = DATEADD(MONTH,2,@Date_Start)
		                    set @Date_End = DATEADD(Day,-1,@Date_End)

		                    set @Date_Start_3M =  DATEADD(MONTH,-1,@Date_Start)


                            Create TABLE  #TB  
                            (
                                _CustomerCD BIGINT,
                                _CustomerCode nvarchar(50),
                                _MTD  money,
			                    _Customer_2M money,
			                    _Customer_3M money        
                            )
		                    --B1: Đẩy M_Customer vào @TB, Lưu ý bỏ qua nhưng Customer bị trùng Code
		                    insert into #TB(_CustomerCD,_CustomerCode)
		                    select  MIN(CUSTOMER_CD)as CUSTOMER_CD ,CUSTOMER_CODE 
		                    from M_CUSTOMER 
		                    where  active = 1
		                    group by CUSTOMER_CODE
		                    having COUNT(*) = 1

		                    --B2: Update MTD vào @TB
		                    Update tb
		                    set tb._MTD = mtd.MTD		 
		                    from  
		                    (
			                    select  cust.CUSTOMER_CD,cust.CUSTOMER_CODE, Convert(money,(CASE WHEN SUM(SALES_AMOUNT) is null then 0 else SUM(SALES_AMOUNT)  end)) as MTD
			                    from  M_CUSTOMER cust
			                    join O_SALES_AMOUNT osa  on osa.CUSTOMER_CD = cust.CUSTOMER_CD and YEAR(SALES_AMOUNT_DATE) = Year(getdate()) and MONTH(SALES_AMOUNT_DATE) = Month(getdate())
			                    where cust.ACTIVE = 1
			                    group by cust.CUSTOMER_CD,cust.CUSTOMER_CODE 
		                    ) mtd join #TB tb on mtd.CUSTOMER_CD = tb._CustomerCD
		
	  
		                    --Update Customer 2M: doanh số trung bình 2 tháng trước (ko tính tháng hiện tại)
		                    update  tb
			                    set tb._Customer_2M = TB_2M.Customer_2M
		                    from (
				                    select  cust.CUSTOMER_CD,cust.CUSTOMER_CODE, Convert(money,(CASE WHEN SUM(SALES_AMOUNT) is null then 0 else SUM(SALES_AMOUNT)/2  end)) as Customer_2M
				                    from  M_CUSTOMER cust
				                    join O_SALES_AMOUNT osa  on osa.CUSTOMER_CD = cust.CUSTOMER_CD 
				                    where cust.ACTIVE = 1
				                    and  SALES_AMOUNT_DATE between @Date_Start and @Date_End 	 --VD tháng hiện tại là T6 thì tính T4 và T5
				                    group by cust.CUSTOMER_CD,cust.CUSTOMER_CODE 
		                    ) TB_2M  join #TB tb  on tb._CustomerCD = TB_2M.CUSTOMER_CD 
			
	
		
		                    --Update Customer 3M: doanh số trung bình 3 tháng trước (ko tính tháng hiện tại)
		                    update  tb
			                    set tb._Customer_3M = TB_3M.Customer_3M
		                    from (
				                    select  cust.CUSTOMER_CD,cust.CUSTOMER_CODE,  Convert(money,(CASE WHEN SUM(SALES_AMOUNT) is null then 0 else SUM(SALES_AMOUNT)/3  end)) as Customer_3M
				                    from  M_CUSTOMER cust
				                    left join O_SALES_AMOUNT osa  on osa.CUSTOMER_CODE = cust.CUSTOMER_CODE 
				                    where cust.ACTIVE = 1
					                    and  SALES_AMOUNT_DATE between @Date_Start_3M and @Date_End --VD tháng hiện tại là T6 thì tính  T3,T4 và T5 
				                    group by cust.CUSTOMER_CD,cust.CUSTOMER_CODE 
		                    ) TB_3M  join #TB tb on tb._CustomerCode = TB_3M.CUSTOMER_CODE
		
	

		                    --insert MTD
		                    insert into [O_CUSTOMER_MTD] (  
				                    [CUSTOMER_CD]
				                    ,[CUSTOMER_CODE]
				                    ,[CUSTOMER_MTD]
				                    ,[CUSTOMER_2M]
				                    ,[CUSTOMER_3M]
				                    ) 
		                    select _CustomerCD,_CustomerCode,_MTD,_Customer_2M,_Customer_3M  
		                    from #TB
		                    where NOT EXISTS(
			                    select _CustomerCD,_CustomerCode from [O_CUSTOMER_MTD] mtd
			                    join #TB tb on tb._CustomerCD = mtd.CUSTOMER_CD
		                    )
		
		                    --update MTD 
		                    update mtd	
		                    set mtd.[CUSTOMER_MTD] = tb._MTD
		                    ,mtd.CUSTOMER_2M = tb._Customer_2M
		                    ,mtd.CUSTOMER_3M = tb._Customer_3M
		                    , mtd.UPDATE_DATE = getdate()
		                    from [O_CUSTOMER_MTD] mtd
		                    join #TB tb on tb._CustomerCD = mtd.CUSTOMER_CD
		                    --print 'Update MTD,2M,3M'

		                    --Update Last visit customer
		                    Declare @Tuan nvarchar(max)
		                    Declare @sql nvarchar(max)

		                    select @Tuan = YEAR(GETDATE())
		                   select @Tuan = @Tuan + '_' + RIGHT('0' + CAST(DATEPART(week,getdate()) AS VARCHAR(2)),2)
	
		                    set @sql = '
		                    update ocm
		                    set ocm.LAST_VISIT_DATE = LV.Max_date
		                    from O_CUSTOMER_MTD ocm
		                    join (select CUSTOMER_CODE, MAX(TIME_IN_CREATED_DATE) as Max_date from MMV_CONSOLIDATE_SALES.dbo.O_TIME_IN_OUT_'+ @Tuan+' 
		                    where CUSTOMER_CODE in (
		                    select CUSTOMER_CODE from M_CUSTOMER
		                    where ACTIVE = 1)
		                    group by CUSTOMER_CODE) LV  on LV.CUSTOMER_CODE = ocm.CUSTOMER_CODE'

		                    exec sp_executesql @sql
		                    --print 'Update Last visit'

		                    --Update Last order date/value
		                    -- select  amount.CUSTOMER_CODE, SALES_AMOUNT,amount.SALES_AMOUNT_DATE,mtd.LAST_ORDER_DATE,LAST_ORDER_VALUE
		                    Update mtd
		                    set mtd.LAST_ORDER_VALUE = amount.SALES_AMOUNT,
			                    mtd.LAST_ORDER_DATE = amount.SALES_AMOUNT_DATE
		                    from [O_SALES_AMOUNT] amount
		                    join O_CUSTOMER_MTD mtd on mtd.CUSTOMER_CODE = amount.CUSTOMER_CODE
		                    join 
  
		                    (SELECT a.[CUSTOMER_CODE] ,MAX([SALES_AMOUNT_DATE]) as Last_Date
		                    FROM [O_SALES_AMOUNT] a
		                    join M_CUSTOMER cust on cust.CUSTOMER_CODE = a.CUSTOMER_CODE and cust.ACTIVE = 1
		                    group by  a.[CUSTOMER_CODE]) T 
		                    on T.CUSTOMER_CODE= amount.CUSTOMER_CODE and T.Last_Date = amount.SALES_AMOUNT_DATE
		                    ---print 'Update Last order date/value'";
            L5sSql.Execute(sql);
        }


        private static void P5sRemoveDataInactive()
        {
            String sql = @"        
                        DECLARE @TIMEZONE BIGINT

                        SELECT @TIMEZONE = ISNULL(VALUE,0)
                        FROM S_PARAMS
                        WHERE NAME = 'TIME_ZONE' 
                  
                    	 INSERT INTO [dbo].[H_SALES_ROUTE]
                                   ([SALES_CD]
                                   ,[ROUTE_CD]
                                   ,[ACTIVE]
                                   ,[SEQUENCE]
                                   ,[DEACTIVE_DATE]
                                   ,[CREATED_DATE])
                        SELECT [SALES_CD]
                                   ,[ROUTE_CD]
                                   ,[ACTIVE]
                                   ,[SEQUENCE]
                                   ,[DEACTIVE_DATE]
                                   ,[CREATED_DATE]
                        FROM [O_SALES_ROUTE]
                        WHERE ACTIVE = 0 AND DEACTIVE_DATE < DATEADD(HOUR, -1 ,DATEADD(HOUR, @TIMEZONE, GETDATE()) )

                        DELETE FROM [O_SALES_ROUTE]
                        WHERE ACTIVE = 0 AND DEACTIVE_DATE < DATEADD(HOUR, -1 ,DATEADD(HOUR, @TIMEZONE, GETDATE()) )

                        INSERT INTO [dbo].[H_CUSTOMER_ROUTE]
                                   ([ROUTE_CD]
                                   ,[CUSTOMER_CD]
                                   ,[CUSTOMER_STT]
                                   ,[ACTIVE],[CREATED_DATE],[DEACTIVE_DATE])
                        SELECT [ROUTE_CD]
                                   ,[CUSTOMER_CD]
                                   ,[CUSTOMER_STT]
                                   ,[ACTIVE], [CREATED_DATE],[DEACTIVE_DATE]
                        FROM O_CUSTOMER_ROUTE
                        WHERE ACTIVE = 0  AND DEACTIVE_DATE < DATEADD(HOUR, -1 ,DATEADD(HOUR, @TIMEZONE, GETDATE()) )


                        DELETE FROM O_CUSTOMER_ROUTE
                        WHERE ACTIVE = 0 AND DEACTIVE_DATE < DATEADD(HOUR, -1 ,DATEADD(HOUR, @TIMEZONE, GETDATE()) )

                    ";

            L5sSql.Query(sql);


        }

        public static void P5sConsolicateTrackingSales()
        {



            // Move dữ liệu từ O_TRACKING sang các bảng O_TRACKING_ tương ứng theo tuần
            //chỉ thực hiện chốt số cho dữ liệu từ ngày 1/1/2016 trở đi
            //Bảng [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] là bảng quan trọng để xác định dữ liệu tuần nào sẽ được chốt số
            //Nếu Active = 1 thì cần chốt số lại tuần đó

            #region script move data
            String sql = @"                          
                        DECLARE @TIMEZONE BIGINT

                        SELECT @TIMEZONE = ISNULL(VALUE,0)
                        FROM S_PARAMS
                        WHERE NAME = 'TIME_ZONE'

                            DECLARE @YYMMDD INT
                            DECLARE @TB_YYMMDD AS TABLE
                            (
	                            YYMMDD BIGINT
                            )

                            INSERT INTO @TB_YYMMDD
                            SELECT DISTINCT YYMMDD 
                            FROM O_TRACKING 


                            DECLARE myCursor CURSOR FOR  
                            SELECT * 
                            FROM @TB_YYMMDD
                            WHERE YYMMDD IS NOT NULL  AND YYMMDD >= 160101
                            ORDER BY YYMMDD

                            OPEN myCursor   
                            FETCH NEXT FROM myCursor INTO @YYMMDD   

                            WHILE @@FETCH_STATUS = 0   
                            BEGIN   
	                              --MOVE DATA
                            	
	                              DECLARE @DATE DATE
	                              DECLARE @WEEK INT
	                              DECLARE @Str NVARCHAR(MAX)


	                              SET @DATE =   CONVERT(DATE, CAST( @YYMMDD AS NVARCHAR) ) 	           
	                              SET @WEEK = DATEPART(WEEK, @DATE) 
	 

	               SET @str = '
					BEGIN TRY 
					            INSERT INTO  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TRACKING_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +']
									            (  [SALES_CD]
									               ,[DISTRIBUTOR_CD]
									               ,[YYMMDD]
									               ,[LONGITUDE_LATITUDE]
									               ,[TRACKING_DATETIME]
									               ,[BATTERY_PERCENTAGE]
									               ,[DEVICE_STATUS]
									               ,[NO_REPEAT]
									               ,[NO_ORDER]
									               ,[CREATED_DATE]
									               ,[BEGIN_DATETIME]
									               ,[END_DATETIME]
									               ,[TRACKING_ACCURACY]
									               ,[TRACKING_PROVIDER]
									               ,[DURATION]
									               ,[ACTIVE]
									               ,[BATTERY_PERCENTAGE_START]
									               ,[BATTERY_PERCENTAGE_END]
									               ,[TYPE_CD]
									               ,[IS_SYNC]
									               ,[LOCATION_ADDRESS])

							              SELECT 
									               [SALES_CD]
									               ,[DISTRIBUTOR_CD]
									               ,[YYMMDD]
									               ,[LONGITUDE_LATITUDE]
									               ,[TRACKING_DATETIME]
									               ,[BATTERY_PERCENTAGE]
									               ,[DEVICE_STATUS]
									               ,[NO_REPEAT]
									               ,[NO_ORDER]
									               ,[CREATED_DATE]
									               ,[BEGIN_DATETIME]
									               ,[END_DATETIME]
									               ,[TRACKING_ACCURACY]
									               ,[TRACKING_PROVIDER]
									               ,[DURATION]
									               ,[ACTIVE]
									               ,[BATTERY_PERCENTAGE_START]
									               ,[BATTERY_PERCENTAGE_END]
									               ,[TYPE_CD]
									               ,[IS_SYNC] = 1
									               ,[LOCATION_ADDRESS]
						                 FROM O_TRACKING
						                 WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TRACKING_DATETIME <= DATEADD(MINUTE,30,DATEADD(HOUR, '+ CONVERT(NVARCHAR, @TIMEZONE)+', CREATED_DATE) )  AND BEGIN_DATETIME IS NOT NULL
                                                     AND LONGITUDE_LATITUDE IS NOT NULL 
            							  					
							             UPDATE [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] SET IS_SYNC = 1
							             WHERE TABLE_NAME_1 = ''O_TRACKING_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +'''
            							
							             --REMOVE ALL DATA 
	 					                 DELETE FROM O_TRACKING
						                 WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TRACKING_DATETIME <= DATEADD(MINUTE,30,DATEADD(HOUR,'+ CONVERT(NVARCHAR,@TIMEZONE)+', CREATED_DATE) )  AND BEGIN_DATETIME IS NOT NULL
                                                     AND LONGITUDE_LATITUDE IS NOT NULL 
					            END TRY  

					            BEGIN CATCH  
                 
					            END CATCH  
            			 
			                  '
                  EXECUTE(@str)
                  FETCH NEXT FROM myCursor INTO @YYMMDD   
            END   

            CLOSE myCursor   
            DEALLOCATE myCursor

                        ";


            L5sSql.Execute(sql);
            #endregion



            //lấy danh sách các NVBH cần chốt số - theo tuần
            //consolidate tracking of sales
            DataTable dtSales = L5sSql.Query(@"
                                              SELECT DISTINCT T1.OBJECT_CD AS SALES_CD , T1.YYMMDD,T2.TABLE_NAME_1,T2.TABLE_NAME_2,T2.TABLE_NAME_3
                                              FROM [dbo].[O_TRACKING_SYNC] T1 INNER JOIN [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] T2 ON
		                                                DATEPART(WEEK, CONVERT(DATE, CAST( T1.YYMMDD AS NVARCHAR) ))  = T2.WEEK AND
		                                                DATEPART(YEAR, CONVERT(DATE, CAST( T1.YYMMDD AS NVARCHAR) ))  = T2.YEAR AND
		                                              T1.TYPE_CD = T2.TYPE_CD
                                               WHERE T1.TYPE_CD = 1 AND T1.ACTIVE = 1
                                                ");

            for (int i = 0; i < dtSales.Rows.Count; i++)
            {
                DataTable row = dtSales.Clone();
                row.ImportRow(dtSales.Rows[i]);

                //hàm chính sẽ thực hiện chốt số dữ liệu
                P5sCmm.P5sCmmFns.P5sConsolidateTrackingOfSales(row, dtSales.Rows[i]["SALES_CD"].ToString(), dtSales.Rows[i]["YYMMDD"].ToString());

                //cập nhật lại dữ liệu nếu đã chốt số rồi thì lần sau sẽ không phải chốt số nữa trừ khi có sự thay đổi về dữ liệu trong ngày đó
                L5sSql.Execute(@"UPDATE [dbo].[O_TRACKING_SYNC] SET ACTIVE = 0 WHERE OBJECT_CD = @0 AND YYMMDD = @1 AND TYPE_CD = 1", dtSales.Rows[i]["SALES_CD"].ToString(), dtSales.Rows[i]["YYMMDD"].ToString());
            }









        }

        public static void P5sConsolicateTrackingSupervisor()
        {

            // Move dữ liệu từ O_TRACKING_SUPERVISOR sang các bảng tương ứng
            #region script move data
            String sql = @"  
 DECLARE @TIMEZONE BIGINT

                        SELECT @TIMEZONE = ISNULL(VALUE,0)
                        FROM S_PARAMS
                        WHERE NAME = 'TIME_ZONE'                        
    DECLARE @YYMMDD INT
                            DECLARE @TB_YYMMDD AS TABLE
                            (
	                            YYMMDD BIGINT
                            )

                            INSERT INTO @TB_YYMMDD
                            SELECT DISTINCT YYMMDD 
                            FROM O_TRACKING_SUPERVISOR 


                            DECLARE myCursor CURSOR FOR  
                            SELECT * 
                            FROM @TB_YYMMDD
                            WHERE YYMMDD IS NOT NULL AND YYMMDD >= 160101
                            ORDER BY YYMMDD

                            OPEN myCursor   
                            FETCH NEXT FROM myCursor INTO @YYMMDD   

                            WHILE @@FETCH_STATUS = 0   
                            BEGIN   
	                              --MOVE DATA
                            	
	                              DECLARE @DATE DATE
	                              DECLARE @WEEK INT
	                              DECLARE @Str NVARCHAR(MAX)


	                              SET @DATE =   CONVERT(DATE, CAST( @YYMMDD AS NVARCHAR) ) 	           
	                              SET @WEEK = DATEPART(WEEK, @DATE) 
	 

	               SET @str = '
					BEGIN TRY 
					            INSERT INTO  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TRACKING_SUPERVISOR_RAW_DATA_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +']
									            (   [SUPERVISOR_CD]
									               ,[DISTRIBUTOR_CD]
									               ,[YYMMDD]
									               ,[LONGITUDE_LATITUDE]
									               ,[TRACKING_DATETIME]
									               ,[BATTERY_PERCENTAGE]
									               ,[DEVICE_STATUS]
									               ,[NO_REPEAT]
									               ,[NO_ORDER]
									               ,[CREATED_DATE]
									               ,[BEGIN_DATETIME]
									               ,[END_DATETIME]
									               ,[TRACKING_ACCURACY]
									               ,[TRACKING_PROVIDER]
									               ,[DURATION]
									               ,[ACTIVE]
									               ,[BATTERY_PERCENTAGE_START]
									               ,[BATTERY_PERCENTAGE_END]
									               ,[TYPE_CD]
									               ,[IS_SYNC]
									               ,[LOCATION_ADDRESS])

							              SELECT 
									                [SUPERVISOR_CD]
									               ,[DISTRIBUTOR_CD]
									               ,[YYMMDD]
									               ,[LONGITUDE_LATITUDE]
									               ,[TRACKING_DATETIME]
									               ,[BATTERY_PERCENTAGE]
									               ,[DEVICE_STATUS]
									               ,[NO_REPEAT]
									               ,[NO_ORDER]
									               ,[CREATED_DATE]
									               ,[BEGIN_DATETIME]
									               ,[END_DATETIME]
									               ,[TRACKING_ACCURACY]
									               ,[TRACKING_PROVIDER]
									               ,[DURATION]
									               ,[ACTIVE]
									               ,[BATTERY_PERCENTAGE_START]
									               ,[BATTERY_PERCENTAGE_END]
									               ,[TYPE_CD]
									               ,[IS_SYNC] = 1
									               ,[LOCATION_ADDRESS]
						                 FROM O_TRACKING_SUPERVISOR
						                 WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TRACKING_DATETIME <= DATEADD(MINUTE,30,DATEADD(HOUR,'+ CONVERT(NVARCHAR, @TIMEZONE)+', CREATED_DATE) )  AND BEGIN_DATETIME IS NOT NULL
                                                     AND LONGITUDE_LATITUDE IS NOT NULL 
            							  					
							             UPDATE [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] SET IS_SYNC = 1
							             WHERE TABLE_NAME_1 = ''O_TRACKING_SUPERVISOR_RAW_DATA_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +'''
            							
							             --REMOVE ALL DATA 
	 					                 DELETE FROM O_TRACKING_SUPERVISOR
						                 WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TRACKING_DATETIME <= DATEADD(MINUTE,30,DATEADD(HOUR,'+ CONVERT(NVARCHAR,@TIMEZONE)+', CREATED_DATE) )  AND BEGIN_DATETIME IS NOT NULL
                                                     AND LONGITUDE_LATITUDE IS NOT NULL 
					            END TRY  

					            BEGIN CATCH  
                 
					            END CATCH  
            			 
			                  '
                  EXECUTE(@str)
                  FETCH NEXT FROM myCursor INTO @YYMMDD   
            END   

            CLOSE myCursor   
            DEALLOCATE myCursor


                        ";


            L5sSql.Execute(sql);
            #endregion




            //consolidate tracking of sales
            DataTable dt = L5sSql.Query(@"
                                              SELECT DISTINCT T1.OBJECT_CD AS SUPERVISOR_CD , T1.YYMMDD,T2.TABLE_NAME_1,T2.TABLE_NAME_2,T2.TABLE_NAME_3
                                              FROM [dbo].[O_TRACKING_SYNC] T1 INNER JOIN [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] T2 ON
		                                                DATEPART(WEEK, CONVERT(DATE, CAST( T1.YYMMDD AS NVARCHAR) ))  = T2.WEEK AND
		                                                DATEPART(YEAR, CONVERT(DATE, CAST( T1.YYMMDD AS NVARCHAR) ))  = T2.YEAR AND
		                                              T1.TYPE_CD = T2.TYPE_CD
                                               WHERE T1.TYPE_CD = 2 AND T1.ACTIVE = 1
                                                ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable row = dt.Clone();
                row.ImportRow(dt.Rows[i]);
                P5sCmm.P5sCmmFns.P5sConsolidateTrackingOfSupervisor(row, dt.Rows[i]["SUPERVISOR_CD"].ToString(), dt.Rows[i]["YYMMDD"].ToString());
                L5sSql.Execute(@"UPDATE [dbo].[O_TRACKING_SYNC] SET ACTIVE = 0 WHERE OBJECT_CD = @0 AND YYMMDD = @1 AND TYPE_CD = 2", dt.Rows[i]["SUPERVISOR_CD"].ToString(), dt.Rows[i]["YYMMDD"].ToString());

            }



        }

        public static void P5sConsolicateTrackingASM()
        {
            // Move dữ liệu từ O_TRACKING_SUPERVISOR sang các bảng tương ứng
            #region script move data
            String sql = @"         
                        DECLARE @TIMEZONE BIGINT

                        SELECT @TIMEZONE = ISNULL(VALUE,0)
                        FROM S_PARAMS
                        WHERE NAME = 'TIME_ZONE' 
                 
                            DECLARE @YYMMDD INT
                            DECLARE @TB_YYMMDD AS TABLE
                            (
	                            YYMMDD BIGINT
                            )

                            INSERT INTO @TB_YYMMDD
                            SELECT DISTINCT YYMMDD 
                            FROM O_TRACKING_ASM 


                            DECLARE myCursor CURSOR FOR  
                            SELECT * 
                            FROM @TB_YYMMDD
                            WHERE YYMMDD IS NOT NULL AND YYMMDD >= 160101
                            ORDER BY YYMMDD

                            OPEN myCursor   
                            FETCH NEXT FROM myCursor INTO @YYMMDD   

                            WHILE @@FETCH_STATUS = 0   
                            BEGIN   
	                              --MOVE DATA
                            	
	                              DECLARE @DATE DATE
	                              DECLARE @WEEK INT
	                              DECLARE @Str NVARCHAR(MAX)


	                              SET @DATE =   CONVERT(DATE, CAST( @YYMMDD AS NVARCHAR) ) 	           
	                              SET @WEEK = DATEPART(WEEK, @DATE) 
	 

	           
	  SET @str = '
					BEGIN TRY 
					INSERT INTO  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TRACKING_ASM_RAW_DATA_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +']
									(   [ASM_CD]
									   ,[DISTRIBUTOR_CD]
									   ,[YYMMDD]
									   ,[LONGITUDE_LATITUDE]
									   ,[TRACKING_DATETIME]
									   ,[BATTERY_PERCENTAGE]
									   ,[DEVICE_STATUS]
									   ,[NO_REPEAT]
									   ,[NO_ORDER]
									   ,[CREATED_DATE]
									   ,[BEGIN_DATETIME]
									   ,[END_DATETIME]
									   ,[TRACKING_ACCURACY]
									   ,[TRACKING_PROVIDER]
									   ,[DURATION]
									   ,[ACTIVE]
									   ,[BATTERY_PERCENTAGE_START]
									   ,[BATTERY_PERCENTAGE_END]
									   ,[TYPE_CD]
									   ,[IS_SYNC]
									   ,[LOCATION_ADDRESS])

							  SELECT 
									   [ASM_CD]
									   ,[DISTRIBUTOR_CD]
									   ,[YYMMDD]
									   ,[LONGITUDE_LATITUDE]
									   ,[TRACKING_DATETIME]
									   ,[BATTERY_PERCENTAGE]
									   ,[DEVICE_STATUS]
									   ,[NO_REPEAT]
									   ,[NO_ORDER]
									   ,[CREATED_DATE]
									   ,[BEGIN_DATETIME]
									   ,[END_DATETIME]
									   ,[TRACKING_ACCURACY]
									   ,[TRACKING_PROVIDER]
									   ,[DURATION]
									   ,[ACTIVE]
									   ,[BATTERY_PERCENTAGE_START]
									   ,[BATTERY_PERCENTAGE_END]
									   ,[TYPE_CD]
									   ,[IS_SYNC] = 1
									   ,[LOCATION_ADDRESS]
						     FROM O_TRACKING_ASM
						     WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TRACKING_DATETIME <= DATEADD(MINUTE,30,DATEADD(HOUR,'+CONVERT(NVARCHAR, @TIMEZONE)+', CREATED_DATE) )  AND BEGIN_DATETIME IS NOT NULL
                                                     AND LONGITUDE_LATITUDE IS NOT NULL 
							  					
							 UPDATE [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] SET IS_SYNC = 1
							 WHERE TABLE_NAME_1 = ''O_TRACKING_ASM_RAW_DATA_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +'''
							
							 --REMOVE ALL DATA 
	 					     DELETE FROM O_TRACKING_ASM
						     WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TRACKING_DATETIME <= DATEADD(MINUTE,30,DATEADD(HOUR,'+CONVERT(NVARCHAR, @TIMEZONE)+', CREATED_DATE) )  AND BEGIN_DATETIME IS NOT NULL
                                                     AND LONGITUDE_LATITUDE IS NOT NULL 
					END TRY  

					BEGIN CATCH  
     
					END CATCH  
			 
			      '
                  EXECUTE(@str)

                  FETCH NEXT FROM myCursor INTO @YYMMDD   
            END   

            CLOSE myCursor   
            DEALLOCATE myCursor

                        ";


            L5sSql.Execute(sql);
            #endregion



            //consolidate tracking of sales
            DataTable dt = L5sSql.Query(@"
                                              SELECT DISTINCT T1.OBJECT_CD AS ASM_CD , T1.YYMMDD,T2.TABLE_NAME_1,T2.TABLE_NAME_2,T2.TABLE_NAME_3
                                              FROM [dbo].[O_TRACKING_SYNC] T1 INNER JOIN [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] T2 ON
		                                                DATEPART(WEEK, CONVERT(DATE, CAST( T1.YYMMDD AS NVARCHAR) ))  = T2.WEEK AND
		                                                DATEPART(YEAR, CONVERT(DATE, CAST( T1.YYMMDD AS NVARCHAR) ))  = T2.YEAR AND
		                                              T1.TYPE_CD = T2.TYPE_CD
                                               WHERE T1.TYPE_CD = 4 AND T1.ACTIVE = 1
                                                ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable row = dt.Clone();
                row.ImportRow(dt.Rows[i]);
                P5sCmm.P5sCmmFns.P5sConsolidateTrackingOfASM(row, dt.Rows[i]["ASM_CD"].ToString(), dt.Rows[i]["YYMMDD"].ToString());
                L5sSql.Execute(@"UPDATE [dbo].[O_TRACKING_SYNC] SET ACTIVE = 0 WHERE OBJECT_CD = @0 AND YYMMDD = @1 AND TYPE_CD = 3", dt.Rows[i]["ASM_CD"].ToString(), dt.Rows[i]["YYMMDD"].ToString());

            }




        }

        public static String P5sGetConnectionString()
        {
            SqlConnection Conn = (SqlConnection)null;

            string connectionString = ConfigurationSettings.AppSettings["ConStr"];
            if (ConfigurationSettings.AppSettings["sa"] == "1" && HttpContext.Current.Session["L5sSAU"] != null)
                connectionString = connectionString.Substring(0, connectionString.ToLower().IndexOf("user id")) +
                                   (object)"User ID=fs" + (string)HttpContext.Current.Session["L5sSAU"] + "; " +
                                   connectionString.Substring(connectionString.ToLower().IndexOf("password"));
            return connectionString;

        }

        internal static string P5sRandomeStr(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        internal static DataTable P5sGetCommune(string p)
        {
            String W5sSqlSelectCommune = String.Format(@"SELECT COMMUNE_CD, COMMUNE_NAME_VI, {0},ACTIVE FROM  M_COMMUNE", p);
            DataTable dtableCommune = L5sSql.Query(W5sSqlSelectCommune);
            return dtableCommune;
        }

        internal static DataTable P5sGetCountry()
        {
            String W5sSqlSelectCountry = String.Format(@"SELECT COUNTRY_CD, COUNTRY_NAME, '' ,ACTIVE FROM  M_COUNTRY");
            DataTable dtableCountry = L5sSql.Query(W5sSqlSelectCountry);
            return dtableCountry;
        }

        internal static DataTable P5sGetPSR(string p)
        {
            String W5sSqlSelectPSR = String.Format(@"SELECT 1, 'Select PSR ', '' ,'true'");
            DataTable dtablePRS = L5sSql.Query(W5sSqlSelectPSR);
            return dtablePRS;
        }

        internal static DataTable P5sGetRegion(string p)
        {
            p = p == "" ? @"' '" : "reg." + p;
            String P5sSqlSelect = String.Format(@"
                                        SELECT DISTINCT reg.REGION_CD,reg.REGION_CODE, {0}, reg.ACTIVE  
                                         FROM M_REGION reg INNER JOIN M_AREA are ON reg.REGION_CD  = are.REGION_CD AND are.ACTIVE = 1
	                                 INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                WHERE reg.ACTIVE = 1 AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										  INNER JOIN [M_DISTRIBUTOR.] dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1
								 )  ORDER BY REGION_CD", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;

        }

        internal static DataTable P5sGetChanel(string p)
        {
            p = p == "" ? @"' '" : "reg." + p;
            String P5sSqlSelect = String.Format(@"
                                        SELECT CUSTOMER_CHAIN_CD, CUSTOMER_CHAIN_NAME , '', ACTIVE
                                        from M_CUSTOMER_CHAIN
                                        WHERE ACTIVE = 1
								 ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;

        }

        internal static DataTable P5sGetArea(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@" 
                                             SELECT  AREA_CD,AREA_CODE, {0}, ACTIVE 
                                             FROM  M_AREA 
                                             WHERE  AREA_CD IN (  SELECT are.AREA_CD
                                                                            FROM  M_AREA are  INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                                                            WHERE are.ACTIVE = 1  AND EXISTS ( SELECT * FROM
										                                                                              M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										                                              INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										                                              INNER JOIN [M_DISTRIBUTOR.] dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									                                            WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1
								                                             ) 
					                                             )

                                    ORDER BY AREA_ORDER", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetDistributor(string p, string areaCD)
        {
            p = p == "" ? @"' '" : "arePro." + p;
            areaCD = areaCD == "" ? "-1" : areaCD;

            String P5sSqlSelect = String.Format(@"SELECT DISTINCT dis.DISTRIBUTOR_CD,dis.DISTRIBUTOR_CODE + '-'+ dis.DISTRIBUTOR_NAME, {0}, dis.ACTIVE 
                                                    FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                                                     INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
	                                                     INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                                                     INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                    WHERE arePro.AREA_CD = {1} AND dis.DISTRIBUTOR_TYPE_CD = 1 ", p, areaCD);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetDistributorAllType(string p, string areaCD)
        {
            p = p == "" ? @"' '" : "arePro." + p;
            areaCD = areaCD == "" ? "-1" : areaCD;

            String P5sSqlSelect = String.Format(@"SELECT DISTINCT dis.DISTRIBUTOR_CD,dis.DISTRIBUTOR_CODE + '-'+ dis.DISTRIBUTOR_NAME, {0}, dis.ACTIVE 
                                                    FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                                                     INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
	                                                     INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                                                     INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                    WHERE arePro.AREA_CD = {1}  ", p, areaCD);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetDistributor(string p)
        {
            p = p == "" ? @"' '" : "arePro." + p;


            String P5sSqlSelect = String.Format(@"SELECT DISTINCT dis.DISTRIBUTOR_CD,dis.DISTRIBUTOR_CODE + '-'+ dis.DISTRIBUTOR_NAME, {0}, dis.ACTIVE 
                                                    FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                                                     INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
	                                                     INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                                                     INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                       AND dis.DISTRIBUTOR_TYPE_CD = 1 ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetDistributorBySupervisor(string param, String supervisorCD)
        {
            param = param == "" ? @"' '" : "arePro." + param;


            String P5sSqlSelect = String.Format(@"SELECT DISTINCT dis.DISTRIBUTOR_CD,dis.DISTRIBUTOR_CODE + '-'+ dis.DISTRIBUTOR_NAME, {0}, dis.ACTIVE 
                                                    FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                                                     INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
	                                                     INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                                                     INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1                                                      
                                                 WHERE  not exists 
                                                                ( 
                                                                    Select * from O_SUPERVISOR_DISTRIBUTOR supDist 
                                                                    WHERE dis.DISTRIBUTOR_CD = supDist.DISTRIBUTOR_CD AND supDist.DEACTIVE_DATE IS NULL 
                                                                           AND supDist.SUPERVISOR_CD = {1}
                                                                )
                                                ", param, supervisorCD);

            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetAllDistributor(string p)
        {
            p = p == "" ? @"' '" : "arePro." + p;


            String P5sSqlSelect = String.Format(@"SELECT DISTINCT dis.DISTRIBUTOR_CD,dis.DISTRIBUTOR_CODE + '-'+ dis.DISTRIBUTOR_NAME, {0}, dis.ACTIVE 
                                                    FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                                                     INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
	                                                     INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                                                     INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                      ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetSales(string p, String distributorCDs)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT SALES_CD,SALES_CODE + '-'+ SALES_NAME, {0}, ACTIVE FROM  M_SALES WHERE DISTRIBUTOR_CD IN ({1})   ORDER BY SALES_CODE ", p, distributorCDs);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetSales(string p)
        {
            p = p == "" ? @"' '" : "sls." + p;
            String P5sSqlSelect = String.Format(@"SELECT sls.SALES_CD,sls.SALES_CODE + '-'+ sls.SALES_NAME, {0}, sls.ACTIVE 
                                                 FROM  M_SALES sls INNER JOIN M_DISTRIBUTOR dist ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
                                                    ORDER BY SALES_CODE ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }





        internal static DataTable P5sGetSalesByMessage(string p, String messageCD)
        {
            p = p == "" ? @"' '" : "sls." + p;
            String P5sSqlSelect = String.Format(@"SELECT sls.SALES_CD,sls.SALES_CODE + '-'+ sls.SALES_NAME, {0}, sls.ACTIVE 
                                                 FROM  M_SALES sls INNER JOIN M_DISTRIBUTOR dist ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
                                                 WHERE NOT EXISTS (SELECT * FROM O_MESSAGE_DETAIL msgDetail WHERE msgDetail.OBJECT_CD = sls.SALES_CD AND  msgDetail.MESSAGE_CD = {1} AND msgDetail.ACTIVE = 1)
                                                    ORDER BY SALES_CODE ", p, messageCD);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetSalesBySales(string p, string salesCd)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT SALES_CD, SALES_CODE+ '-'+ SALES_NAME, {0}, ACTIVE FROM  M_SALES WHERE SALES_CD !={1} AND DISTRIBUTOR_CD=(select DISTRIBUTOR_CD FROM M_SALES WHERE SALES_CD={1})", p, salesCd);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetSupervisorByDistributor(string p, string salesCd)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT SALES_CD, SALES_CODE+ '-'+ SALES_NAME, {0}, ACTIVE 
                                                FROM  M_SALES WHERE SALES_CD !={1} AND DISTRIBUTOR_CD=(select DISTRIBUTOR_CD FROM M_SALES WHERE SALES_CD={1})", p, salesCd);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetRoute(string p)
        {
            p = p == "" ? @"' '" : "sr." + p;
            String P5sSqlSelect = String.Format(@"SELECT r.ROUTE_CD, r.ROUTE_CODE + '-' +r.ROUTE_NAME ,{0} ,r.ACTIVE FROM O_SALES_ROUTE sr INNER JOIN M_ROUTE r ON sr.ROUTE_CD = r.ROUTE_CD AND sr.ACTIVE = 1 WHERE r.ACTIVE = 1 ORDER BY ROUTE_CODE", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;

        }

        internal static DataTable P5sGetSupervisorRoute(string p)
        {
            p = p == "" ? @"' '" : "ssRout." + p;
            String P5sSqlSelect = String.Format(@"SELECT supRout.SUPERVISOR_ROUTE_CD, supRout.SUPERVISOR_ROUTE_CODE +'-'+ supRout.SUPERVISOR_ROUTE_NAME,{0}  ,supRout.ACTIVE
                                                          FROM M_SUPERVISOR_ROUTE supRout INNER JOIN O_SUPERVISOR_SUPERVISOR_ROUTE ssRout  
						                                	ON supRout.SUPERVISOR_ROUTE_CD = ssRout.SUPERVISOR_ROUTE_CD AND ssRout.ACTIVE = 1", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;

        }

        internal static DataTable P5sGetRoute(string p, String salesCDs)
        {
            p = p == "" ? @"' '" : "sr." + p;
            String P5sSqlSelect = String.Format(@"SELECT r.ROUTE_CD, r.ROUTE_CODE + '-' +r.ROUTE_NAME ,{0} ,r.ACTIVE FROM O_SALES_ROUTE sr INNER JOIN M_ROUTE r ON sr.ROUTE_CD = r.ROUTE_CD AND sr.ACTIVE = 1 WHERE sr.SALES_CD IN ({1}) ORDER BY ROUTE_CODE", p, salesCDs);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;

        }

        internal static DataTable P5sGetCustomerChainCode(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT CUSTOMER_CHAIN_CODE,CUSTOMER_CHAIN_CODE,{0}, ACTIVE
                                                    FROM [M_CUSTOMER_CHAIN]", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;

        }

        internal static DataTable P5sGetProvinceByNotArea(String p)
        {
            p = p == "" ? @"' '" : "reg." + p;
            String P5sSqlSelect = String.Format(@"SELECT DISTINCT p.PROVINCE_CD, p.PROVINCE_CODE + '-' + p.PROVINCE_NAME_EN AS TINH_THANH,
                                                '', p.ACTIVE 
                                                FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD
                                                WHERE p.ACTIVE = 1 and not a.AREA_CD IN ({0})",p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetProvinceByArea(String areaCDs)
        {
            String P5sSqlSelect = String.Format(@"SELECT DISTINCT p.PROVINCE_CD, p.PROVINCE_CODE + '-' + p.PROVINCE_NAME_EN ,'', p.ACTIVE 
                                                FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD
                                                WHERE a.AREA_CD IN ({0})  AND a.ACTIVE = 1
                                                  AND EXISTS ( SELECT * FROM
                                            M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
                                INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
                                INNER JOIN [M_DISTRIBUTOR.] dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
                             WHERE pro.PROVINCE_CD = a.PROVINCE_CD AND dis.ACTIVE = 1 )", areaCDs);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetProvince(String p)
        {
            p = p == "" ? @"' '" : "a." + p;
            String P5sSqlSelect = String.Format(@"SELECT distinct p.PROVINCE_CD, p.PROVINCE_CODE + '-' + p.PROVINCE_NAME_EN ,{0}, p.ACTIVE 
                                                 FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD AND a.ACTIVE = 1
                                                 WHERE  p.PROVINCE_CD IN  (SELECT pro.PROVINCE_CD FROM
	                                                                M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
	                                                                INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
	                                                                INNER JOIN [M_DISTRIBUTOR.] dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
                                                     WHERE  dis.ACTIVE = 1 )
                                                 ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetProvinceUnMap(String p)
        {
            p = p == "" ? @"' '" : "p." + p;
            String P5sSqlSelect = String.Format(@"SELECT  p.PROVINCE_CD, p.PROVINCE_CODE + '-' + p.PROVINCE_NAME_EN ,{0}, p.ACTIVE 
                                                    FROM  M_PROVINCE p 
                                                    WHERE  p.PROVINCE_CD IN 

                                                    (SELECT pro.PROVINCE_CD FROM
                                                    M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
                                                    INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
                                                    INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD

                                                    WHERE  dis.ACTIVE = 1 )
                                                    AND p.PROVINCE_CD NOT IN (SELECT arePro.PROVINCE_CD FROM M_AREA_PROVINCE arePro WHERE arePro.ACTIVE = 1)
                                                                                            ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetDistrict(String p, String provinceCDs)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT DISTRICT_CD,DISTRICT_CODE + '-'+ DISTRICT_NAME_VI,{0}, ACTIVE FROM  M_DISTRICT WHERE PROVINCE_CD IN ({1}) ORDER BY DISTRICT_CODE", p, provinceCDs);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetDistrict(String p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT DISTRICT_CD,DISTRICT_CODE + '-'+ DISTRICT_NAME_VI,{0}, ACTIVE FROM  M_DISTRICT  ORDER BY DISTRICT_CODE", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetCommune(string p, String districtCDs)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT COMMUNE_CD,COMMUNE_CODE + '-'+ COMMUNE_NAME_VI, {0}, ACTIVE FROM  M_COMMUNE  WHERE DISTRICT_CD IN ({1}) ORDER BY COMMUNE_CODE", p, districtCDs);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static String P5sConvertDataTableToListStr(String sql)
        {
            DataTable dt = L5sSql.Query(sql);
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            String result = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result += dt.Rows[i][0].ToString() + ",";
            }
            return result.Remove(result.Length - 1);
        }

        internal static String P5sConvertDataTableToListStr(DataTable dtable, String columnName)
        {
            DataTable dt = dtable;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            String result = "";
            List<String> arr = new List<String>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][columnName].ToString() != "" && Array.IndexOf(arr.ToArray(), dt.Rows[i][columnName].ToString()) == -1)
                {
                    result += dt.Rows[i][columnName].ToString() + ",";
                    arr.Add(dt.Rows[i][columnName].ToString());
                }
            }
            return result.Remove(result.Length - 1);
        }

        internal static String P5sConvertDataTableToListStrWithSeperate(DataTable dtable, String seperate)
        {
            DataTable dt = dtable;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            String result = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() != "")
                    result += dt.Rows[i][0].ToString() + seperate;
            }
            return result.Remove(result.Length - seperate.Length);
        }

        //Get postback control ID
        public static string P5sGetPostBackControlId(Page page)
        {
            if (!page.IsPostBack)
                return string.Empty;

            Control control = null;
            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.
            string controlName = page.Request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                control = page.FindControl(controlName);
            }
            else
            {
                // if __EVENTTARGET is null, the control is a button type and we need to
                // iterate over the form collection to find it

                // ReSharper disable TooWideLocalVariableScope
                string controlId;
                Control foundControl;
                // ReSharper restore TooWideLocalVariableScope

                foreach (string ctl in page.Request.Form)
                {
                    if (ctl == null)
                    {
                        return "";
                    }
                    // handle ImageButton they having an additional "quasi-property" 
                    // in their Id which identifies mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlId = ctl.Substring(0, ctl.Length - 2);
                        foundControl = page.FindControl(controlId);
                    }
                    else
                    {
                        foundControl = page.FindControl(ctl);
                    }

                    if (!(foundControl is Button || foundControl is ImageButton)) continue;

                    control = foundControl;
                    break;
                }
            }

            return control == null ? String.Empty : control.ID;
        }

        internal static DataTable P5sGetASM(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"select ASM_CD,ASM_CODE + '-'+ ASM_NAME, {0}, ACTIVE from  M_ASM order by ASM_CODE", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetUser(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"select USER_CD,USER_CODE + '-'+ USER_NAME, {0}, ACTIVE from  M_USER order by USER_CODE", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetCountry(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"select COUNTRY_CD,COUNTRY_NAME, {0}, ACTIVE from  M_COUNTRY where COUNTRY_NAME = 'VIET NAM' order by COUNTRY_CODE", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetUserPosition(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"select PST_CD,PST_CD +'-' + PST_DESCRIPT, {0}, 1 from  [S_PST]  order by PST_CD", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetUserManagement(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"select USER_CD,USER_CODE +'-'+USER_NAME, {0}, ACTIVE from  M_USER where POSITION_CD in (1,2,3)  order by USER_CODE desc", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetRSM(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"select RSM_CD,RSM_CODE +'-'+RSM_NAME, {0}, ACTIVE from  M_RSM order by RSM_CODE ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetAM(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"select ASM_CD,ASM_CODE +'-'+ASM_NAME, {0}, ACTIVE from  M_ASM  order by ASM_CODE ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetCDS(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"select SUPERVISOR_CD,SUPERVISOR_CODE +'-'+SUPERVISOR_NAME, {0}, ACTIVE from  M_SUPERVISOR  order by SUPERVISOR_CODE ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable ReadExcelContents(string fileName, String sheetName)
        {
            try
            {
                OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
                String strExtendedProperties = String.Empty;
                sbConnection.DataSource = fileName;


                List<DataTable> dts = new List<DataTable>();


                if (Path.GetExtension(fileName).Equals(".xls"))//for 97-03 Excel file
                {
                    sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                    strExtendedProperties = "Excel 8.0;HDR=NO;IMEX=1;";//HDR=ColumnHeader,IMEX=InterMixed
                }
                else if (Path.GetExtension(fileName).Equals(".xlsx"))  //for 2007 Excel file
                {
                    sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                    strExtendedProperties = "Excel 12.0;HDR=NO;IMEX=1";
                }

                sbConnection.Add("Extended Properties", strExtendedProperties);
                OleDbConnection connection = new OleDbConnection(sbConnection.ToString());

                connection.Open();



                string excelQuery = String.Format(@"Select * FROM [{0}]", sheetName);


                OleDbCommand cmd = new OleDbCommand(excelQuery, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                connection.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public static DataTable readDataFromExcelFile(String path, String sheetName)
        {


            if (File.Exists(path))
            {
                try
                {
                    FileInfo file = new FileInfo(path);
                    ExcelPackage package = new ExcelPackage(file);
                    ExcelWorkbook workBook = package.Workbook;

                    ExcelWorksheet ws;
                    if (sheetName == "")
                    {
                        ws = workBook.Worksheets[1];
                    }
                    else
                        ws = workBook.Worksheets[sheetName];

                    DataTable tbl = new DataTable();



                    //remove row merge
                    int beginRow = 1;
                    //process header column
                    String[] header = new String[ws.Dimension.End.Column];
                    for (int i = 1; i <= ws.Dimension.End.Column; i++)
                    {

                        String columnName = ws.Cells[beginRow, i].Text;
                        if (String.IsNullOrEmpty(columnName))
                            columnName = String.Format("Column {0}", i);
                        else
                        {
                            if (Array.IndexOf(header, columnName) == -1)
                                header[i - 1] = columnName;
                            else
                                columnName = String.Format("{0} {1}", columnName, i);
                        }

                        tbl.Columns.Add(columnName);
                    }




                    beginRow = beginRow + 1; //d?c giá tr? cho dòng ti?p theo


                    for (int rowNum = beginRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        ExcelRange wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.NewRow();
                        foreach (ExcelRangeBase cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                        tbl.Rows.Add(row);
                    }
                    return tbl;

                }
                catch (Exception ex)
                {
                    return null;
                }

            }

            return null;


        }
        public static DataTable readDataFromExcelFile(String path, String sheetName, int firstRow)
        {


            if (File.Exists(path))
            {
                try
                {
                    FileInfo file = new FileInfo(path);
                    ExcelPackage package = new ExcelPackage(file);
                    ExcelWorkbook workBook = package.Workbook;

                    ExcelWorksheet ws;
                    if (sheetName == "")
                    {
                        ws = workBook.Worksheets[1];
                    }
                    else
                        ws = workBook.Worksheets[sheetName];

                    DataTable tbl = new DataTable();



                    //remove row merge
                    int beginRow = firstRow;
                    //process header column
                    String[] header = new String[ws.Dimension.End.Column];
                    for (int i = 1; i <= ws.Dimension.End.Column; i++)
                    {

                        String columnName = ws.Cells[beginRow, i].Text;
                        if (String.IsNullOrEmpty(columnName))
                            columnName = String.Format("Column {0}", i);
                        else
                        {
                            if (Array.IndexOf(header, columnName) == -1)
                                header[i - 1] = columnName;
                            else
                                columnName = String.Format("{0} {1}", columnName, i);
                        }

                        tbl.Columns.Add(columnName);
                    }




                    beginRow = beginRow + 1; //d?c giá tr? cho dòng ti?p theo


                    for (int rowNum = beginRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        ExcelRange wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.NewRow();
                        foreach (ExcelRangeBase cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                        tbl.Rows.Add(row);
                    }
                    return tbl;

                }
                catch (Exception ex)
                {
                    return null;
                }

            }

            return null;


        }
        internal static DataTable ReadExcelContents(string fileName, String sheetName, bool showHDR)
        {
            try
            {
                OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
                String strExtendedProperties = String.Empty;
                sbConnection.DataSource = fileName;


                List<DataTable> dts = new List<DataTable>();


                if (Path.GetExtension(fileName).Equals(".xls"))//for 97-03 Excel file
                {
                    sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                    strExtendedProperties = "Excel 8.0;HDR=YES;IMEX=1;HDR=ColumnHeader,IMEX=InterMixed";
                }
                else if (Path.GetExtension(fileName).Equals(".xlsx"))  //for 2007 Excel file
                {
                    sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                    strExtendedProperties = "Excel 12.0;HDR=YES;IMEX=1";
                }

                sbConnection.Add("Extended Properties", strExtendedProperties);
                OleDbConnection connection = new OleDbConnection(sbConnection.ToString());

                connection.Open();



                string excelQuery = String.Format(@"Select * FROM [{0}]", sheetName);


                OleDbCommand cmd = new OleDbCommand(excelQuery, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                connection.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        internal static String P5sCreateViewStateTemp()
        {
            return System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + P5sCmm.P5sCmmFns.P5sRandomeStr(50);
        }

        internal static void P5sUpdateValueForMap()
        {
            #region Update DB 
            String P5sUpdate = @";with CTE  as
                                (
	                                select d.DISTRIBUTOR_CD, SUM(cm.[POPULATION]) as [POPULATION]     
	                                from [M_DISTRIBUTOR.] d inner join M_CUSTOMER c on d.DISTRIBUTOR_CD = c.DISTRIBUTOR_CD AND c.ACTIVE = 1
                                                         inner join M_COMMUNE cm on c.COMMUNE_CD = cm.COMMUNE_CD
	                                where c.COMMUNE_CD is not null 
	                                group by d.DISTRIBUTOR_CD
                                )
                                update [M_DISTRIBUTOR.] set POPULATION_COVERED = c.[POPULATION]
                                from [M_DISTRIBUTOR.] d inner join CTE c on d.DISTRIBUTOR_CD = c.DISTRIBUTOR_CD;
                                --Update AMS of Customer 
                               ;with CTE_ASM_CUSTOMER as 
                                (
	                                 select cust.CUSTOMER_CD, Sum(OS.SALES_AMOUNT) as SALES_AMOUNT
                                    from [M_CUSTOMER] cust
                                    Join O_SALES_AMOUNT OS on cust.CUSTOMER_CD = OS.CUSTOMER_CD
                                    where MONTH(OS.SALES_AMOUNT_DATE) = MONTH(Getdate()) -- thay đổi tháng hiện tại
                                    and YEAR(OS.SALES_AMOUNT_DATE) = YEAR(GETDATE()) -- thay đổi năm hiện tại
                                    group by cust.CUSTOMER_CD
                                )
                                Update [M_CUSTOMER]  
                                Set AMS = AMSC.SALES_AMOUNT
                                from [M_CUSTOMER] cust join CTE_ASM_CUSTOMER AMSC on cust.CUSTOMER_CD = AMSC.CUSTOMER_CD
                
                                --Update DSP
                                ;with CTE_1  as
                                (
	                                select d.DISTRIBUTOR_CD , COUNT(*) as DSP
	                                from [M_DISTRIBUTOR.] d inner join M_SALES s on d.DISTRIBUTOR_CD = s.DISTRIBUTOR_CD AND s.ACTIVE = 1
	                                group by d.DISTRIBUTOR_CD
                                )
                                update [M_DISTRIBUTOR.] set DSP = c.DSP
                                from [M_DISTRIBUTOR.] d inner join CTE_1 c on d.DISTRIBUTOR_CD = c.DISTRIBUTOR_CD;                                

                                -- Update Customer Coveraged
                                ;with CTE_2  as
                                (
	                                select d.DISTRIBUTOR_CD , 
                                           SUM( CASE C.CUSTOMER_CHAIN_CODE WHEN 'WSN' THEN 1 ELSE 0 END) as WHOLE_SALES 
                                           ,COUNT(*) as  CUSTOMER_COVERED        
	                                from [M_DISTRIBUTOR.] d inner join M_CUSTOMER c on d.DISTRIBUTOR_CD = c.DISTRIBUTOR_CD	AND c.ACTIVE = 1
	                                group by d.DISTRIBUTOR_CD
                                )
                                update [M_DISTRIBUTOR.] set CUSTOMER_COVERED = c.CUSTOMER_COVERED, WHOLE_SALES = c.WHOLE_SALES
                                from [M_DISTRIBUTOR.] d inner join CTE_2 c on d.DISTRIBUTOR_CD = c.DISTRIBUTOR_CD;   
                                     
                                 --Update AMS of DISTRIBUTOR
                    		     ;with CTE_3  as
                                (
	                                select c.DISTRIBUTOR_CD, SUM(c.AMS) as AMS from M_CUSTOMER c inner join M_COMMUNE  cm on c.COMMUNE_CD = cm.COMMUNE_CD
									where c.AMS is not null AND c.ACTIVE = 1
	                                group by c.DISTRIBUTOR_CD
                                )
                                update [M_DISTRIBUTOR.] set AMS = c.AMS
                                from [M_DISTRIBUTOR.] d inner join CTE_3 c on d.DISTRIBUTOR_CD = c.DISTRIBUTOR_CD

                                --Update Population Covered

						        ;with CTE_4  as
                                (
										select TMP.DISTRIBUTOR_CD, SUM(Tmp.POPULATION) as Population
										from (
														select distinct cus.DISTRIBUTOR_CD,com.POPULATION,com.COMMUNE_CD 
														from M_COMMUNE com inner join M_CUSTOMER cus on com.COMMUNE_CD = cus.COMMUNE_CD AND cus.ACTIVE = 1) as Tmp
										group by Tmp.DISTRIBUTOR_CD
                                )
                                update [M_DISTRIBUTOR.] set POPULATION_COVERED = c.Population
                                from [M_DISTRIBUTOR.] d left join CTE_4 c on d.DISTRIBUTOR_CD = c.DISTRIBUTOR_CD


                               --Update distance Commune
                                update M_COMMUNE set DISTRIBUTOR_DISTANCE = T.KM
                                from M_COMMUNE com inner join 
		                                (select distinct com.COMMUNE_CD,comdis.KM,comdis.DISTRIBUTOR_CD
		                                 from M_COMMUNE com inner join M_CUSTOMER cus on com.COMMUNE_CD = cus.COMMUNE_CD  AND cus.ACTIVE = 1
                                           inner join M_COMMUNE_DISTRIBUTOR comdis on com.COMMUNE_CD = comdis.COMMUNE_CD and cus.DISTRIBUTOR_CD = comdis.DISTRIBUTOR_CD	
		                                ) T on com.COMMUNE_CD = t.COMMUNE_CD
                                
                              --Update distance commune uncovered
                                update M_COMMUNE set DISTRIBUTOR_DISTANCE = T.KM, DISTRIBUTOR_NAME_NEAREST = T.DISTRIBUTOR_NAME
                                from M_COMMUNE com inner join 
		                                (
			                                select comdis.COMMUNE_CD,comdis.COMMUNE_DISTRIBUTOR_CD, dis.DISTRIBUTOR_NAME,T.KM from M_COMMUNE_DISTRIBUTOR comdis inner join (

			                                select comdis.COMMUNE_CD ,min(comdis.KM) as KM From M_COMMUNE com inner join M_COMMUNE_DISTRIBUTOR comdis on com.COMMUNE_CD = comdis.COMMUNE_CD
			                                where not exists (select * from M_CUSTOMER cus where cus.COMMUNE_CD = com.COMMUNE_CD and cus.COMMUNE_CD is not null AND cus.ACTIVE = 1)
		                                     group by comdis.COMMUNE_CD
			                                ) as T on comdis.COMMUNE_CD = T.COMMUNE_CD and comdis.KM = T.KM
		                                    inner join [M_DISTRIBUTOR.] dis on comdis.DISTRIBUTOR_CD = dis.DISTRIBUTOR_CD

		                                ) T on com.COMMUNE_CD = t.COMMUNE_CD

						      --Update area Covered
						            ;with CTE_6  as
                                    (
										    select TMP.DISTRIBUTOR_CD, SUM(Tmp.AREA) as Area
										    from (
														    select distinct cus.DISTRIBUTOR_CD,ISNULL(com.AREA,0) as AREA,com.COMMUNE_CD 
														    from M_COMMUNE com inner join M_CUSTOMER cus on com.COMMUNE_CD = cus.COMMUNE_CD AND cus.ACTIVE = 1 ) as Tmp
										    group by Tmp.DISTRIBUTOR_CD
                                    )
                                    update [M_DISTRIBUTOR.] set AREA_COVERED = c.Area
                                    from [M_DISTRIBUTOR.] d left join CTE_6 c on d.DISTRIBUTOR_CD = c.DISTRIBUTOR_CD
                                                        
							        --Update AMS of Commune
                    		           ;with CTE_ASM_COMMUNE  as
                                        (
	                                        select  SUM(c.AMS) as AMS,c.COMMUNE_CD
									        from M_CUSTOMER c 
									        where c.AMS is not null AND c.ACTIVE = 1 AND c.COMMUNE_CD IS NOT NULL
	                                        group by c.COMMUNE_CD
                                        )
                                        update M_COMMUNE set AMS = cte.AMS
                                        from M_COMMUNE cmm inner join CTE_ASM_COMMUNE cte on cmm.COMMUNE_CD = cte.COMMUNE_CD";
            #endregion
            //update Population
            L5sSql.Execute(P5sUpdate);
        }

        internal static void P5sUpdateRadius(String communeCD)
        {
            String sql = @"select DISTINCT dis.DISTRIBUTOR_CD,dis.LONGITUDE_LATITUDE
                        from M_CUSTOMER cust INNER JOIN M_DISTRIBUTOR dis ON cust.DISTRIBUTOR_CD = dis.DISTRIBUTOR_CD
                        WHERE cust.COMMUNE_CD = @0";
            DataTable dt = L5sSql.Query(sql, communeCD);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql = @"SELECT LONGITUDE_LATITUDE FROM M_COMMUNE cmm
                        WHERE EXISTS (SELECT * FROM M_CUSTOMER cust WHERE cmm.COMMUNE_CD = cust.COMMUNE_CD AND cust.DISTRIBUTOR_CD = @0) ";
                DataTable dtCommune = L5sSql.Query(sql, dt.Rows[i]["DISTRIBUTOR_CD"].ToString());

                double maxDistance = 0;
                for (int j = 0; j < dtCommune.Rows.Count; j++)
                {
                    P5sLocation start = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                    P5sLocation end = new P5sLocation(dtCommune.Rows[j]["LONGITUDE_LATITUDE"].ToString());
                    double tempDistance = P5sTrackingHelper.CalculateDistance(start, end);
                    if (tempDistance > maxDistance)
                        maxDistance = tempDistance;
                }
                String sqlUpdate = @"UPDATE [M_DISTRIBUTOR.] SET RADIUS = @1 WHERE DISTRIBUTOR_CD = @0";
                L5sSql.Execute(sqlUpdate, dt.Rows[i]["DISTRIBUTOR_CD"].ToString(), maxDistance);
            }
        }

        internal static DataTable P5sGetDistributorDirect()
        {
            String P5sSqlSelect = String.Format(@"SELECT DISTINCT dis.DISTRIBUTOR_CD,dis.DISTRIBUTOR_CODE + '-'+ dis.DISTRIBUTOR_NAME, ' ', dis.ACTIVE 
                                                    FROM [M_DISTRIBUTOR.] dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                                                     INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
	                                                     INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                                                     INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                WHERE dis.DISTRIBUTOR_TYPE_CD = 1");
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        //        internal static void P5sCreateFnsGetLongTudeOfSalesman()
        //        {
        //            String sql = "IF NOT  EXISTS (SELECT * FROM dbo.sysobjects WHERE [id] = OBJECT_ID(N'[dbo].[fnsGetLongTudeOfSalesman]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) SELECT 'NOT EXISTS' as 'VALUE' ";
        //            if (L5sSql.Query(sql).Rows.Count > 0 && L5sSql.Query(sql).Rows[0][0].ToString() == "NOT EXISTS")
        //            {

        //                String sqlCreateFunction = @"   
        //                                                        CREATE FUNCTION [dbo].[fnsGetLongTudeOfSalesman](@SalesCD BIGINT,@DistributorCD BIGINT,@YYMMDD INT) 
        //	                                        RETURNS @TableReturn TABLE 
        //                                            (    	
        //		                                        VlatLng nvarchar(max),
        //		                                        LatLngs  nvarchar(max),
        //		                                        TrackingDateTime  nvarchar(max)
        //	                                        )
        //	                                        AS
        //
        //                                        BEGIN
        //		                                        DECLARE @VlatLng nvarchar(max)
        //		                                        DECLARE @LatLngs nvarchar(max)
        //		                                        DECLARE @TrackingDateTime nvarchar(max)
        //                                        		
        //                                        		
        //		                                        SELECT @VlatLng = COALESCE(@VlatLng + ',',' ') + ' ' +  CONVERT(NVARCHAR(MAX), 'new VLatLng (' +  LONGITUDE_LATITUDE + ')' ),
        //			                                           @LatLngs = COALESCE(@LatLngs + '@',' ') + '' +  CONVERT(NVARCHAR(MAX),   LONGITUDE_LATITUDE   ) ,
        //			                                           @TrackingDateTime = COALESCE(@TrackingDateTime + '@',' ') + '' + CAST(TRACKING_DATETIME AS nvarchar) 
        //                                        			   		   	
        //		                                        FROM  O_TRACKING 
        //		                                        WHERE SALES_CD = @SalesCD AND DISTRIBUTOR_CD = @DistributorCD AND YYMMDD = @YYMMDD
        //		                                        ORDER BY TRACKING_DATETIME
        //
        //
        //		                                        SET @VlatLng = '[' + @VlatLng + ']'		
        //
        //		                                        INSERT INTO @TableReturn 
        //		                                        SELECT @VlatLng,@LatLngs,ISNULL(@TrackingDateTime,'') AS TrackingDateTime
        //
        //		                                        RETURN
        //                                        END;  
        //
        //                                        --DROP FUNCTION [fnsGetLongTudeOfSalesman]
        //
        //                                        ";

        //                L5sSql.Execute(sqlCreateFunction);
        //            }

        //        }

        public static double CalculateAngle(P5sLocation location1, P5sLocation location2)
        {
            double a = location1.Latitude * Math.PI / 180;
            double b = location1.Longitude * Math.PI / 180;
            double c = location2.Latitude * Math.PI / 180;
            double d = location2.Longitude * Math.PI / 180;

            if (Math.Cos(c) * Math.Sin(d - b) == 0)
                if (c > a)
                    return 0;
                else
                    return 180;
            else
            {
                double angle = Math.Atan2(Math.Cos(c) * Math.Sin(d - b), Math.Sin(c) * Math.Cos(a) - Math.Sin(a) * Math.Cos(c) * Math.Cos(d - b));
                return (angle * 180 / Math.PI + 360) % 360;

            }
        }

        internal static void P5sInitHHMM(DropDownList HH, DropDownList MM)
        {
            P5sCmm.P5sCmmFns.P5sSetHH(HH);
            P5sCmm.P5sCmmFns.P5sSetMM(MM);

        }

        private static void P5sSetMM(DropDownList MM)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MM_CD");
            dt.Columns.Add("MM_NAME");


            for (int i = 0; i <= 59; i++)
            {
                if (i <= 9)
                    dt.Rows.Add("0" + i, "0" + i);
                else
                    dt.Rows.Add(i, i);
            }
            MM.DataSource = dt;
            MM.DataValueField = "MM_CD";
            MM.DataTextField = "MM_NAME";
            MM.DataBind();
        }

        private static void P5sSetHH(DropDownList HH)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("HH_CD");
            dt.Columns.Add("HH_NAME");


            for (int i = 1; i <= 23; i++)
            {
                dt.Rows.Add(i, i);
            }
            HH.DataSource = dt;
            HH.DataValueField = "HH_CD";
            HH.DataTextField = "HH_NAME";
            HH.DataBind();
        }

        internal static DataTable P5sDtableRouteByDistributor(string p, string distributor)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT r.ROUTE_CD,r.ROUTE_CODE + '-'+ r.ROUTE_NAME, {0}, r.ACTIVE FROM  M_ROUTE r WHERE r.DISTRIBUTOR_CD =@0 and not exists ( select * from O_SALES_ROUTE sr WHERE r.ROUTE_CD = sr.ROUTE_CD AND sr.DEACTIVE_DATE IS NULL  ) ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect, distributor);
            return dtable;
        }

        internal static DataTable P5sGetCustomerByDistributor(string p, string distributor)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@" SELECT c.CUSTOMER_CD, c.CUSTOMER_CODE + '-'+ c.CUSTOMER_NAME, {0}, 
                                             c.ACTIVE
                                             FROM  M_CUSTOMER c
                                             WHERE c.DISTRIBUTOR_CD =@1     and not exists 
                                                                                           (   Select * 
	                                                                                           from O_CUSTOMER_ROUTE cr
	                                                                                           WHERE c.CUSTOMER_CD = cr.CUSTOMER_CD 
	                                                                                           AND cr.DEACTIVE_DATE IS NULL 
                                                                                           )", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect, p, distributor);
            return dtable;
        }
        internal static DataTable P5sGetCustomersBySale(string p, string sale)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@" 	select MC.CUSTOMER_CD,MC.CUSTOMER_CODE+ '-'+MC.CUSTOMER_NAME,{0},MC.ACTIVE
                                                    from M_CUSTOMER MC
                                                    INNER JOIN O_CUSTOMER_ROUTE OCR ON MC.CUSTOMER_CD=OCR.CUSTOMER_CD AND OCR.ACTIVE=1
                                                    INNER JOIN M_ROUTE MR ON OCR.ROUTE_CD= MR.ROUTE_CD AND MR.ACTIVE=1
                                                    INNER JOIN O_SALES_ROUTE OSR ON OSR.ROUTE_CD= MR.ROUTE_CD AND OSR.ACTIVE=1
                                                    INNER JOIN M_SALES MS ON OSR.SALES_CD=MS.SALES_CD AND MS.ACTIVE=1 AND MS.SALES_CD='{1}'
                                                    WHERE MC.ACTIVE=1 ", p, sale);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }
        internal static DataTable P5sGetCustomerByDistributorForSupervisor(string p, string distributor)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@" SELECT c.CUSTOMER_CD, c.CUSTOMER_CODE + '-'+ c.CUSTOMER_NAME, {0}, 
                                             c.ACTIVE
                                             FROM  M_CUSTOMER c
                                             WHERE c.DISTRIBUTOR_CD =@1     and not exists 
                                                                                           (   Select * 
	                                                                                           from O_CUSTOMER_SUPERVISOR_ROUTE cr
	                                                                                           WHERE c.CUSTOMER_CD = cr.CUSTOMER_CD 
	                                                                                           AND cr.DEACTIVE_DATE IS NULL 
                                                                                           )", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect, p, distributor);
            return dtable;
        }

        internal static DataTable P5sDtableRouteByDistributorForSupervisor(string p, string distributor)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT r.SUPERVISOR_ROUTE_CD,r.SUPERVISOR_ROUTE_CODE + '-'+ r.SUPERVISOR_ROUTE_NAME, {0}, r.ACTIVE FROM  M_SUPERVISOR_ROUTE r WHERE r.DISTRIBUTOR_CD IN ({1}) and not exists ( select * from O_SUPERVISOR_SUPERVISOR_ROUTE sr WHERE r.SUPERVISOR_ROUTE_CD = sr.SUPERVISOR_ROUTE_CD AND sr.DEACTIVE_DATE IS NULL  ) ", p, distributor);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sDtableRouteByDistributorForASM(string p, string distributor)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT r.ASM_ROUTE_CD,r.ASM_ROUTE_CODE + '-'+ r.ASM_ROUTE_NAME, {0}, r.ACTIVE FROM  M_ASM_ROUTE r WHERE r.DISTRIBUTOR_CD IN ({1}) and not exists ( select * from O_ASM_ASM_ROUTE sr WHERE r.ASM_ROUTE_CD = sr.ASM_ROUTE_CD AND sr.DEACTIVE_DATE IS NULL  ) ", p, distributor);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetRoutes(string p, string routeCD)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT ROUTE_CD, ROUTE_CODE+ '-'+ ROUTE_NAME, {0}, ACTIVE FROM  M_ROUTE WHERE ROUTE_CD !={1} AND DISTRIBUTOR_CD=(select DISTRIBUTOR_CD FROM M_ROUTE WHERE ROUTE_CD={1})", p, routeCD);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetSupervisorRoutes(string p, string routeCD)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT SUPERVISOR_ROUTE_CD, SUPERVISOR_ROUTE_CODE+ '-'+ SUPERVISOR_ROUTE_NAME, {0}, ACTIVE FROM  M_SUPERVISOR_ROUTE WHERE SUPERVISOR_ROUTE_CD !={1} AND DISTRIBUTOR_CD=(select DISTRIBUTOR_CD FROM M_SUPERVISOR_ROUTE WHERE SUPERVISOR_ROUTE_CD={1})", p, routeCD);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetASMRoutes(string p, string routeCD)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT ASM_ROUTE_CD, ASM_ROUTE_CODE+ '-'+ ASM_ROUTE_NAME, {0}, ACTIVE FROM  M_ASM_ROUTE WHERE ASM_ROUTE_CD !={1} AND DISTRIBUTOR_CD=(select DISTRIBUTOR_CD FROM M_ASM_ROUTE WHERE ASM_ROUTE_CD={1})", p, routeCD);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetCustomerByRouteChainCode(string routeCd, string chainCd)
        {
            String sql = String.Format(@"SELECT DISTINCT cust.CUSTOMER_CD, cust.CUSTOMER_CODE +'-'+ cust.CUSTOMER_NAME,'',cust.ACTIVE 
                        FROM M_CUSTOMER cust INNER JOIN M_CUSTOMER_CHAIN CC ON     cust.CUSTOMER_CHAIN_CODE = CC.CUSTOMER_CHAIN_CODE
                             INNER JOIN O_CUSTOMER_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
                             INNER JOIN M_ROUTE rout ON custR.ROUTE_CD = rout.ROUTE_CD 
                        WHERE CC.CUSTOMER_CHAIN_CD in ({0}) AND rout.ROUTE_CD IN ({1})

                        AND  not exists  
                        (
                             SELECT CUSTOMER_CD FROM O_CUSTOMER_SUPERVISOR_ROUTE custR
                             WHERE custR.CUSTOMER_CD = cust.CUSTOMER_CD AND  custR.DEACTIVE_DATE IS NULL 
                         )
                    
                
                        ", chainCd, routeCd);
            DataTable dtable = L5sSql.Query(sql);
            return dtable;
        }

        internal static DataTable P5sGetCustomerByRouteChainCodeForASM(string routeCd, string chainCd)
        {
            String sql = String.Format(@"SELECT DISTINCT cust.CUSTOMER_CD, cust.CUSTOMER_CODE +'-'+ cust.CUSTOMER_NAME,'',cust.ACTIVE 
                        FROM M_CUSTOMER cust INNER JOIN M_CUSTOMER_CHAIN CC ON     cust.CUSTOMER_CHAIN_CODE = CC.CUSTOMER_CHAIN_CODE
                             INNER JOIN O_CUSTOMER_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
                             INNER JOIN M_ROUTE rout ON custR.ROUTE_CD = rout.ROUTE_CD 
                        WHERE CC.CUSTOMER_CHAIN_CD in ({0}) AND rout.ROUTE_CD IN ({1})

                        AND  not exists  
                        (
                             SELECT CUSTOMER_CD FROM O_CUSTOMER_ASM_ROUTE custR
                             WHERE custR.CUSTOMER_CD = cust.CUSTOMER_CD AND  custR.DEACTIVE_DATE IS NULL 
                         )
                    
                
                        ", chainCd, routeCd);
            DataTable dtable = L5sSql.Query(sql);
            return dtable;
        }

        internal static DataTable P5sGetRouteByDistributor(string p, String distributorCd)
        {
            p = p == "" ? @"' '" : "sr." + p;
            String P5sSqlSelect = String.Format(@"SELECT * FROM ( 
                    SELECT DISTINCT  r.ROUTE_CD, r.ROUTE_CODE + '-' +r.ROUTE_NAME AS  ROUTE_CODE ,sr.SALES_CD ,r.ACTIVE 
                                        FROM O_SALES_ROUTE sr INNER JOIN M_ROUTE r ON sr.ROUTE_CD = r.ROUTE_CD AND sr.ACTIVE = 1
                                            INNER JOIN M_SALES sls ON sr.SALES_CD = sls.SALES_CD 
                                        WHERE r.ACTIVE = 1  AND sls.DISTRIBUTOR_CD = @0 										
                    )AS T ORDER BY T.ROUTE_CODE", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect, distributorCd);
            return dtable;

        }

        internal static DataTable P5sGetChainCode(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT CUSTOMER_CHAIN_CD,CUSTOMER_CHAIN_CODE,{0}, ACTIVE
                                                    FROM [M_CUSTOMER_CHAIN]", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;

        }

        internal static DataTable P5sGetCustomerByRouteCD(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT DISTINCT cr.CUSTOMER_CD, cp.CUSTOMER_CODE ,{0} ,  ACTIVE FROM O_CUSTOMER_ROUTE cr INNER JOIN O_CUSTOMER_PHOTO cp ON 
                                                cr.CUSTOMER_CD = cp.CUSTOMER_CD
                                                WHERE ACTIVE = 1", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetSalesGeoCode(string p, string clockCd)
        {
            p = p == "" ? @"' '" : "sls." + p;
            String P5sSqlSelect = String.Format(@"SELECT sls.SALES_CD,sls.SALES_CODE + '-'+ sls.SALES_NAME, {0}, sls.ACTIVE 
                                                 FROM  M_SALES sls INNER JOIN M_DISTRIBUTOR dist ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
												 WHERE sls.SALES_CD  NOT IN  (SELECT USER_CD FROM O_CLOCK_GET_GEOCODE_DETAIL WHERE ACTIVE = 1 AND CLOCK_CD = {1})
                                                    ORDER BY SALES_CODE ", p, clockCd);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sGetRoutesByASM(string p)
        {
            p = p == "" ? @"' '" : "oAAr." + p;
            String P5sSqlSelect = String.Format(@"SELECT mAr.ASM_ROUTE_CD, mAr.ASM_ROUTE_CODE +'-'+ mAr.ASM_ROUTE_NAME,{0}, mAr.ACTIVE FROM O_ASM_ASM_ROUTE oAAr 
                                                    INNER JOIN M_ASM mA ON oAAr.ASM_CD = mA.ASM_CD
                                                    INNER JOIN M_ASM_ROUTE mAr ON mAr.ASM_ROUTE_CD = oAAr.ASM_ROUTE_CD", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }
        internal static DataTable P5sGetProduct(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT PRODUCT_CD, PRODUCT_CODE +'-'+ PRODUCT_NAME,{0}, ACTIVE
                                                FROM O_PRODUCT", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static void P5sConsolicateTimeIntOut()
        {
            //các dữ liệu liệu về timeinout bị lỗi có thời gian không hợp lệ, ngày tạo bị null => sẽ bị xóa trước khi chốt số
            #region remove timeinout error
            String sql = @"
 DECLARE @TIMEZONE BIGINT

                        SELECT @TIMEZONE = ISNULL(VALUE,0)
                        FROM S_PARAMS
                        WHERE NAME = 'TIME_ZONE'
INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[ERR_TIME_IN_OUT]
           ([ROUTE_CD]
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
           ,[YYMMDD])
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
		  ,[YYMMDD]
  FROM [dbo].[O_TIME_IN_OUT]
  WHERE  YYMMDD > 500000 OR YYMMDD >  CONVERT ( nvarchar(6) , GETDATE()    , 12 ) OR TIME_IN_CREATED_DATE > DATEADD(MINUTE,30,DATEADD(HOUR, @TIMEZONE, CREATED_DATE) ) -- năm 2050 lứn hơn có nhgia thoi gian pda bi sai, hoặc lớn hơn ngày hiện tại
         OR YYMMDD < 150101 OR TIME_IN_CREATED_DATE IS NULL

DELETE	   FROM [dbo].[O_TIME_IN_OUT]
  WHERE  YYMMDD > 500000 OR YYMMDD >  CONVERT ( nvarchar(6) ,  GETDATE()    , 12 ) OR TIME_IN_CREATED_DATE > DATEADD(MINUTE,30,DATEADD(HOUR, @TIMEZONE, CREATED_DATE) )  -- năm 2050 lứn hơn có nhgia thoi gian pda bi sai, hoặc lớn hơn ngày hiện tại
         OR YYMMDD < 150101 OR TIME_IN_CREATED_DATE IS NULL ";
            #endregion
            L5sSql.Query(sql);

            // Move dữ liệu từ O_TRACKING sang các bảng tương ứng
            //Chốt số TIME_IN_OUT khá đơn giản chỉ là duy chuyển dữ liệu từ bảng gốc sang các bảng tương ứng với DB  MMV_CONSOLIDATE_SALES
            //sau khi duy chuyển dữ liệu xong thì sẽ xóa các dữ liệu bị duplicate
            #region script move data
            sql = @"                          
 DECLARE @TIMEZONE BIGINT

                        SELECT @TIMEZONE = ISNULL(VALUE,0)
                        FROM S_PARAMS
                        WHERE NAME = 'TIME_ZONE'
                            DECLARE @YYMMDD INT
                            DECLARE @TB_YYMMDD AS TABLE
                            (
	                            YYMMDD BIGINT
                            )

                             UPDATE O_TIME_IN_OUT SET YYMMDD = CONVERT ( nvarchar(6) , TIME_IN_CREATED_DATE, 12 )  
                                                                    WHERE YYMMDD IS NULL

                        
                            INSERT INTO @TB_YYMMDD
                            SELECT DISTINCT YYMMDD 
                            FROM O_TIME_IN_OUT 


                            DECLARE myCursor CURSOR FOR  
                            SELECT * 
                            FROM @TB_YYMMDD
                            WHERE YYMMDD IS NOT NULL AND YYMMDD >= 160101
                            ORDER BY YYMMDD

                            OPEN myCursor   
                            FETCH NEXT FROM myCursor INTO @YYMMDD   

                            WHILE @@FETCH_STATUS = 0   
                            BEGIN   
                                  --MOVE DATA
                            	
                                  DECLARE @DATE DATE
                                  DECLARE @WEEK INT
                                  DECLARE @Str NVARCHAR(MAX)


                                  SET @DATE =   CONVERT(DATE, CAST( @YYMMDD AS NVARCHAR) ) 	           
                                  SET @WEEK = DATEPART(WEEK, @DATE) 
	 

	               SET @str = '
					BEGIN TRY 
					            INSERT INTO  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TIME_IN_OUT_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +']
									             ( [ROUTE_CD]
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
							                       ,[LOCATION_IS_NULL]) 							          
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
						                      FROM O_TIME_IN_OUT
						                 WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TIME_IN_CREATED_DATE <= DATEADD(MINUTE,30,DATEADD(HOUR,'+CONVERT(NVARCHAR, @TIMEZONE)+', CREATED_DATE) )
            							  				
							         
							             --REMOVE ALL DATA 
	 					                 DELETE FROM O_TIME_IN_OUT
						                 WHERE  YYMMDD  = ' +  CAST(@YYMMDD AS nvarchar)   + ' AND TIME_IN_CREATED_DATE <= DATEADD(MINUTE,30,DATEADD(HOUR,'+CONVERT(NVARCHAR, @TIMEZONE)+', CREATED_DATE) )
                                          
                                         --xóa các dữ liệu bị duplicate nếu có
                                         DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TIME_IN_OUT_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +']
					                     WHERE TIME_IN_OUT_CD NOT IN (SELECT MIN(TIME_IN_OUT_CD) 
					                     FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TIME_IN_OUT_' + CAST(YEAR(@DATE) AS nvarchar) + '_' +  CAST(FORMAT(@WEEK,'00')   AS nvarchar)  +']
					                     GROUP BY ROUTE_CD,SALES_CD,DISTRIBUTOR_CD,CUSTOMER_CD, CUSTOMER_CODE, TIME_IN_LATITUDE_LONGITUDE, TIME_IN_LATITUDE_LONGITUDE_ACCURACY
							                    ,  TIME_OUT_LATITUDE_LONGITUDE,TIME_OUT_LATITUDE_LONGITUDE_ACCURACY,
							                    TIME_IN_CREATED_DATE, TIME_OUT_CREATED_DATE)
      

					            END TRY 
					            BEGIN CATCH  
					            END CATCH  
			                  '
                  EXECUTE(@str)
                  FETCH NEXT FROM myCursor INTO @YYMMDD   
            END   

            CLOSE myCursor   
            DEALLOCATE myCursor


            

                        ";


            L5sSql.Execute(sql);





            #endregion

        }

        internal static DataTable P5sGetSupervisorByArea(string p)
        {
            p = p == "" ? @"' '" : "arePro." + p;
            String P5sSqlSelect = String.Format(@"SELECT DISTINCT sup.SUPERVISOR_CD,sup.SUPERVISOR_CODE + '-'+ sup.SUPERVISOR_NAME, {0}, sup.ACTIVE 
                                            FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                                                INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
	                                                INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                                                INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                    INNER JOIN O_SUPERVISOR_DISTRIBUTOR supDist ON supDist.DISTRIBUTOR_CD = dis.DISTRIBUTOR_CD and supDist.ACTIVE = 1
                                                    INNER JOIN M_SUPERVISOR sup ON sup.SUPERVISOR_CD = supDist.SUPERVISOR_CD ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static DataTable P5sAutoCompleteNull()
        {

            String P5sSqlSelect = String.Format(@"SELECT TOP 1 CUSTOMER_CD, CUSTOMER_CODE +'-'+ CUSTOMER_NAME,'', ACTIVE 
                                                                                                        FROM M_CUSTOMER WHERE CUSTOMER_CD = -1 ");
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        public static bool P5sCheckGeocodeValid(string xy)
        {
            try
            {
                Regex regex = new Regex("^[0-9]*$");
                string[] ArrXY = xy.Split(',');
                string[] ArrX = ArrXY[0].Split('.');
                string[] ArrY = ArrXY[1].Split('.');
                if (ArrXY.Length > 2 || ArrX.Length > 2 || ArrY.Length > 2)
                {
                    return false;
                }
                if (!regex.IsMatch(ArrX[0]) || !regex.IsMatch(ArrX[1]) || !regex.IsMatch(ArrY[0]) || !regex.IsMatch(ArrY[1]))
                {
                    return false;
                }
                if (Convert.ToInt64(ArrX[0]) <= 0 ||
                    Convert.ToInt64(ArrX[1]) <= 0 ||
                    Convert.ToInt64(ArrY[0]) <= 0 ||
                    Convert.ToInt64(ArrY[1]) <= 0 ||
                    Convert.ToInt64(ArrX[0]) > Convert.ToInt16(ArrY[0]))
                {
                    return false;
                }
                if (ArrXY[0].Remove(1, ArrXY[0].Length - 1) == "0" || ArrXY[1].Remove(1, ArrXY[1].Length - 1) == "0")
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetAddressFormGeoCode(string xy, string proxy)
        {
            if (!P5sCheckGeocodeValid(xy))
            {
                return "-1";
            }
            try
            {
                WebProxy proxyObject = new WebProxy(proxy, true);
                string url = "http://maps.google.com/maps/api/geocode/xml?latlng=" + xy + "&sensor=false";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Proxy = proxyObject;
                request.Timeout = 5000;
                request.Method = "POST";
                string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseFromServer);
                string a = "-1";
                XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("/GeocodeResponse");
                foreach (XmlNode node in nodes)
                {
                    a = node.SelectSingleNode("status").InnerText;
                }
                if (a.Equals("OK"))
                {
                    XmlNodeList nodes1 = xmlDoc.DocumentElement.SelectNodes("/GeocodeResponse/result");

                    if (nodes1.Item(0) != null)
                    {
                        string[] list = nodes1.Item(0).SelectSingleNode("formatted_address").InnerText.ToString().Split(',');
                        if (list.Length >= 5)
                        {
                            return list[list.Length - 4] + ", " + list[list.Length - 3] + ", " + list[list.Length - 2] + ", " + list[list.Length - 1];
                        }
                        else
                        {
                            if (nodes1.Item(1) != null)
                            {
                                list = nodes1.Item(1).SelectSingleNode("formatted_address").InnerText.ToString().Split(',');
                                if (list.Length == 4)
                                    return nodes1.Item(1).SelectSingleNode("formatted_address").InnerText.ToString();
                            }
                            return nodes1.Item(0).SelectSingleNode("formatted_address").InnerText;
                        }
                    }
                    else
                    {
                        return "0"; // Tọa độ hợp lệ nhưng không phân giải đc địa chỉ
                    }

                }
                else
                {
                    return "-1"; // tọa độ không hợp lệ
                }
            }
            catch
            {
                return "1";
            }
        }

        public static void GetAddressFormGeoCodeThroughProxy()
        {
            string GeoCode = "", proxy = "", address = "";

            if (MMV.MapGeocodeToHierarchy.numberGeoCode < MMV.MapGeocodeToHierarchy.dtGeoCode.Rows.Count &&
                MMV.MapGeocodeToHierarchy.numberProxy < MMV.MapGeocodeToHierarchy.dtProxyIP.Rows.Count - 1
                )
            {
                proxy = MMV.MapGeocodeToHierarchy.dtProxyIP.Rows[MMV.MapGeocodeToHierarchy.numberProxy++]["PROXY_IP"].ToString();
            }
            while (MMV.MapGeocodeToHierarchy.numberGeoCode < MMV.MapGeocodeToHierarchy.dtGeoCode.Rows.Count)
            {
                int i = MMV.MapGeocodeToHierarchy.numberGeoCode++;
                GeoCode = MMV.MapGeocodeToHierarchy.dtGeoCode.Rows[i]["LONGITUDE_LATITUDE"].ToString();
            RECONNECT:
                if (!P5sCheckGeocodeValid(GeoCode))
                {
                    continue;
                }
                try
                {
                    WebProxy proxyObject = new WebProxy(proxy, true);
                    string url = "http://maps.google.com/maps/api/geocode/xml?latlng=" + GeoCode + "&sensor=false";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Proxy = proxyObject;
                    request.Timeout = 8000;
                    request.Method = "POST";
                    string postData = "This is a test that posts this string to a Web server.";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseFromServer);
                    string a = "-1";
                    XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("/GeocodeResponse");
                    foreach (XmlNode node in nodes)
                    {
                        a = node.SelectSingleNode("status").InnerText;
                    }
                    if (a.Equals("OK"))
                    {
                        XmlNodeList nodes1 = xmlDoc.DocumentElement.SelectNodes("/GeocodeResponse/result");

                        if (nodes1.Item(0) != null)
                        {
                            string[] list = nodes1.Item(0).SelectSingleNode("formatted_address").InnerText.ToString().Split(',');
                            if (list.Length >= 5)
                            {
                                address = list[list.Length - 4] + ", " + list[list.Length - 3] + ", " + list[list.Length - 2] + ", " + list[list.Length - 1];
                            }
                            else
                            {
                                if (nodes1.Item(1) != null)
                                {
                                    list = nodes1.Item(1).SelectSingleNode("formatted_address").InnerText.ToString().Split(',');
                                    if (list.Length == 4)
                                        address = nodes1.Item(1).SelectSingleNode("formatted_address").InnerText.ToString();
                                }
                                address = nodes1.Item(0).SelectSingleNode("formatted_address").InnerText;
                            }
                        }
                    }
                    else if (a.Equals("OVER_QUERY_LIMIT"))//ip has limit.
                    {
                        if (MMV.MapGeocodeToHierarchy.numberProxy >= MMV.MapGeocodeToHierarchy.dtProxyIP.Rows.Count)
                        {
                            return;
                        }
                        else
                        {
                            proxy = MMV.MapGeocodeToHierarchy.dtProxyIP.Rows[MMV.MapGeocodeToHierarchy.numberProxy++]["PROXY_IP"].ToString();
                        }
                        goto RECONNECT;
                    }
                    String[] add = address.Split(new String[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                    if (add.Length >= 4)
                    {
                        DataRow[] results = MMV.MapGeocodeToHierarchy.dtHierarchy.Select(String.Format("COMMUNE_NAME_VI LIKE '%{0}%' AND DISTRICT_NAME_VI LIKE '%{1}%' AND PROVINCE_NAME_VI  LIKE '%{2}%' ", add[0].Trim(), add[1].Trim(), add[2].Trim()));
                        if (results != null && results.Length != 0)
                        {
                            MMV.MapGeocodeToHierarchy.dtGeoCode.Rows[i]["COMMUNE_CODE"] = results[0].ItemArray[0];
                            MMV.MapGeocodeToHierarchy.dtGeoCode.Rows[i]["COMMUNE_NAME_EN"] = results[0].ItemArray[1];

                            MMV.MapGeocodeToHierarchy.dtGeoCode.Rows[i]["DISTRICT_CODE"] = results[0].ItemArray[2];
                            MMV.MapGeocodeToHierarchy.dtGeoCode.Rows[i]["DISTRICT_NAME_EN"] = results[0].ItemArray[3];

                            MMV.MapGeocodeToHierarchy.dtGeoCode.Rows[i]["PROVINCE_CODE"] = results[0].ItemArray[4];
                            MMV.MapGeocodeToHierarchy.dtGeoCode.Rows[i]["PROVINCE_NAME_EN"] = results[0].ItemArray[5];
                        }
                        else
                        {
                            MMV.MapGeocodeToHierarchy.dtGeoCode.Rows[i]["NOTE"] = "Phân giải tọa độ thất bại";
                        }
                    }

                    if (MMV.MapGeocodeToHierarchy.numberGeoCode == MMV.MapGeocodeToHierarchy.dtGeoCode.Rows.Count)
                    {
                        return;
                    }
                }
                catch
                {
                    if (MMV.MapGeocodeToHierarchy.numberProxy >= MMV.MapGeocodeToHierarchy.dtProxyIP.Rows.Count)
                    {
                        return;
                    }
                    else
                    {
                        proxy = MMV.MapGeocodeToHierarchy.dtProxyIP.Rows[MMV.MapGeocodeToHierarchy.numberProxy++]["PROXY_IP"].ToString();
                    }
                    goto RECONNECT;
                    // return "-1";
                }

            }
        }

        public static String convertToMD5(String str)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(str);
            encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes).Replace("-", "");
        }

        public static void P5sRemoveDuplicateTracking()
        {
            String sql = String.Format(@"
                     DELETE FROM  O_TRACKING
					 WHERE TRACKING_CD NOT IN 
                        (   SELECT MIN(TRACKING_CD) 
					        FROM  O_TRACKING                            
					        GROUP BY SALES_CD, LONGITUDE_LATITUDE, NO_REPEAT,BEGIN_DATETIME,END_DATETIME,BATTERY_PERCENTAGE
                        )
                    
                     DELETE FROM  O_TRACKING_SUPERVISOR
					 WHERE TRACKING_CD NOT IN 
                        (   SELECT MIN(TRACKING_CD) 
					        FROM  O_TRACKING_SUPERVISOR                            
					        GROUP BY SUPERVISOR_CD, LONGITUDE_LATITUDE, NO_REPEAT,BEGIN_DATETIME,END_DATETIME,BATTERY_PERCENTAGE
                        )

                    DELETE FROM  O_TRACKING_ASM
					 WHERE TRACKING_CD NOT IN 
                        (   SELECT MIN(TRACKING_CD) 
					        FROM  O_TRACKING_ASM                            
					        GROUP BY ASM_CD, LONGITUDE_LATITUDE, NO_REPEAT,BEGIN_DATETIME,END_DATETIME,BATTERY_PERCENTAGE
                        )
                           

                     DELETE FROM [dbo].[O_TIME_IN_OUT]
                     WHERE TIME_IN_OUT_CD NOT IN (SELECT MIN(TIME_IN_OUT_CD) 
                     FROM [dbo].[O_TIME_IN_OUT]
                     GROUP BY ROUTE_CD,SALES_CD,DISTRIBUTOR_CD,CUSTOMER_CD, CUSTOMER_CODE, TIME_IN_LATITUDE_LONGITUDE, TIME_IN_LATITUDE_LONGITUDE_ACCURACY
		                    ,  TIME_OUT_LATITUDE_LONGITUDE,TIME_OUT_LATITUDE_LONGITUDE_ACCURACY,
		                    TIME_IN_CREATED_DATE, TIME_OUT_CREATED_DATE)
            
                    --remove duplicate photo
                    DELETE FROM  O_TRADE_PROGRAM_PHOTO
                    WHERE TRADE_PROGRAM_PHOTO_CD NOT IN 
                    (   SELECT MIN(TRADE_PROGRAM_PHOTO_CD) 
	                    FROM  O_TRADE_PROGRAM_PHOTO                            
	                    GROUP BY PHOTO_NAME,PHOTO_LATITUDE_LONGITUDE,PHOTO_CREATED_DATE
                    )

                    --update lai [NUMBER_PHOTO] sau khi remove duplicate photo
                    DECLARE @TMP AS TABLE
                    (
	                    TRADE_PROGRAM_CUSTOMER_CD BIGINT,
	                    [CURRENT_PHOTO] BIGINT,
	                    [NUMBER_PHOTO] BIGINT,
                        PRIMARY KEY (TRADE_PROGRAM_CUSTOMER_CD)
                    )
                    INSERT INTO @TMP

                    SELECT * FROM (SELECT t1.TRADE_PROGRAM_CUSTOMER_CD, Count(PHOTO_NAME) as 'CURRENT_PHOTO', NUMBER_PHOTO FROM O_TRADE_PROGRAM_PHOTO t1 inner join O_TRADE_PROGRAM_CUSTOMER t2
                    on t1.TRADE_PROGRAM_CUSTOMER_CD = t2.TRADE_PROGRAM_CUSTOMER_CD group by t1.TRADE_PROGRAM_CUSTOMER_CD, NUMBER_PHOTO )t WHERE CURRENT_PHOTO != NUMBER_PHOTO

                    UPDATE T1 SET NUMBER_PHOTO = T2.CURRENT_PHOTO FROM O_TRADE_PROGRAM_CUSTOMER T1 INNER JOIN @TMP T2 ON T1.TRADE_PROGRAM_CUSTOMER_CD = T2.TRADE_PROGRAM_CUSTOMER_CD
                    SELECT * FROM (select t1.TRADE_PROGRAM_CUSTOMER_CD, Count(PHOTO_NAME) as 'CURRENT PHOTO', NUMBER_PHOTO from O_TRADE_PROGRAM_PHOTO t1 inner join O_TRADE_PROGRAM_CUSTOMER t2
                    on t1.TRADE_PROGRAM_CUSTOMER_CD = t2.TRADE_PROGRAM_CUSTOMER_CD group by t1.TRADE_PROGRAM_CUSTOMER_CD, NUMBER_PHOTO )t WHERE [CURRENT PHOTO] = NUMBER_PHOTO

                
                    --remove duplicate customer
                    DELETE FROM  M_CUSTOMER
                    WHERE CUSTOMER_CD NOT IN 
                    (   SELECT MIN(CUSTOMER_CD) 
	                    FROM  M_CUSTOMER   
	                    GROUP BY CUSTOMER_CD
                    )

                
                ");

            L5sSql.Execute(sql);


        }

        //        private static void P5sRemoveDuplicateTrackingOfSupervisor(String supervisorCD,String tableName )
        //        {
        //            String sql = String.Format(String.Format(@" 
        //                     DELETE FROM  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]
        //					 WHERE TRACKING_CD NOT IN (SELECT MIN(TRACKING_CD) 
        //					 FROM  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]
        //                     WHERE  SUPERVISOR_CD =  {1}                    
        //					 GROUP BY SUPERVISOR_CD, LONGITUDE_LATITUDE, NO_REPEAT,BEGIN_DATETIME,END_DATETIME,BATTERY_PERCENTAGE) AND  SUPERVISOR_CD =  {1}", tableName, supervisorCD));

        //           L5sSql.Execute(sql);

        //        }

        //        private static void P5sRemoveDuplicateTrackingOfASM(String salesCD,String tableName)
        //        {
        //            String sql = String.Format(String.Format(@" 
        //                     DELETE FROM  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]
        //					 WHERE 
        //                            TRACKING_CD NOT IN (
        //                                                SELECT MIN(TRACKING_CD) 
        //					                            FROM  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[{0}]
        //                                                WHERE ASM_CD = {1}
        //					                            GROUP BY ASM_CD, LONGITUDE_LATITUDE, NO_REPEAT,BEGIN_DATETIME,END_DATETIME,BATTERY_PERCENTAGE
        //                                                ) 
        //                            AND ASM_CD = {1}
        //                    ", tableName, salesCD));

        //            L5sSql.Execute(sql);

        //        }
        public static void P5sConsolidateTrackingOfSales(DataTable row, String salesCD, String yymmdd)
        {

            //danh sách các bảng cần phải chốt số
            String tableNameTracking = row.Rows[0]["TABLE_NAME_1"].ToString(); //bảng chứ dữ liệu gốc cần chốt số
            String tableNameTrackingOfSales = row.Rows[0]["TABLE_NAME_2"].ToString(); // bảng chứa kết quả chốt số
            String tableNameTrackingOfStopOfSales = row.Rows[0]["TABLE_NAME_3"].ToString(); //bảng chứ stoppint (điểm dừng) - dùng cho 1 số báo cáo Stoppoint


            //script lấy các dữ liệu cần chốt số theo NVBH và ngày 
            String sql = String.Format(@"
                                            DECLARE @SALES_CD BIGINT = {0}
                                            DECLARE @YYMMDD INT = {1}
                                           
                           
                                            SELECT 
	                                            YYMMDD = @YYMMDD,SALES_CD,DISTRIBUTOR_CD,
	                                            LONGITUDE_LATITUDE,
	                                            DEVICE_STATUS, 
	                                            NO_REPEAT,
	                                            BEGIN_DATETIME, 
	                                            END_DATETIME,
	                                            CAST( BATTERY_PERCENTAGE AS INT) AS BATTERY_PERCENTAGE,
	                                            TYPE_TRACKING = '', 
	                                            POINT_RADIUS = '' ,
	                                            ANGEL = '0', 
	                                            TRACKING_ACCURACY,                                                    
	                                            BATTERY_PERCENTAGE_VALUE = '',
	                                            BATTERY_PERCENTAGE_DATETIME = '',
	                                            TRACKING_PROVIDER,
	                                            TRACKING_PROVIDER_VALUE =  '',
	                                            TRACKING_PROVIDER_DATETIME =  '',
                                                LOCATION_ADDRESS                                             
                                                                            	               
                                            FROM   [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{2}
                                            WHERE   TYPE_CD  = 1 AND LONGITUDE_LATITUDE != '' AND SALES_CD = @SALES_CD  AND YYMMDD = @YYMMDD   
                                            ORDER BY BEGIN_DATETIME  

                                        ", salesCD, yymmdd, tableNameTracking);

            DataTable dt = L5sSql.Query(sql);

            if (dt == null || dt.Rows.Count <= 0)
                return;


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ReadOnly = false;
            }

            //set maxlength for colum: mục đích là để có thể cập nhật lại giá trị cho datatable mà không bị lỗi 
            dt.Columns["TYPE_TRACKING"].MaxLength = int.MaxValue;
            dt.Columns["BATTERY_PERCENTAGE_VALUE"].MaxLength = int.MaxValue;
            dt.Columns["BATTERY_PERCENTAGE_DATETIME"].MaxLength = int.MaxValue;
            dt.Columns["TRACKING_PROVIDER_VALUE"].MaxLength = int.MaxValue;
            dt.Columns["TRACKING_PROVIDER_DATETIME"].MaxLength = int.MaxValue;


            //BATTERY_PERCENTAGE_VALUE và BATTERY_PERCENTAGE_DATETIME cho dòng đầu tiên và sẽ được xử dụng ở bên dưới
            if (dt.Rows.Count == 1)
            {
                //set default BATTERY_PERCENTAGE
                if (dt.Rows[0]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[0]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[0]["BATTERY_PERCENTAGE"];
                    dt.Rows[0]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[0]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[0]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[0]["TRACKING_PROVIDER_VALUE"] = dt.Rows[0]["TRACKING_PROVIDER"];
                    dt.Rows[0]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[0]["BEGIN_DATETIME"];
                }


            }

            //gôm các điểm theo Accuracy(độ sai lệ) - nếu như từ điểm A -> B nhở hơn độ sai lệch max của A hoặc B thì A và B cùng là 1 điểm
            for (int i = 0; i < dt.Rows.Count - 1;)
            {
                P5sLocation location1 = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                P5sLocation location2 = new P5sLocation(dt.Rows[i + 1]["LONGITUDE_LATITUDE"].ToString());
                double tempDistance = P5sTrackingHelper.CalculateDistance(location1, location2) * 1000; //tính khoảng cách giữa 2 điểm

                //set default BATTERY_PERCENTAGE
                if (dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE"];
                    dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[i]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER"];
                    dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["BEGIN_DATETIME"];
                }


                try
                {
                    Double accuracy1 = Double.Parse(dt.Rows[i]["TRACKING_ACCURACY"].ToString());
                    Double accuracy2 = Double.Parse(dt.Rows[i + 1]["TRACKING_ACCURACY"].ToString());
                    Double maxAccuracy = accuracy1 > accuracy2 ? accuracy1 : accuracy2; //tìm độ sai lệch lớn nhất giữa 2 điểm

                    //nếu như KC từ A -> B mà nhở hơn hoặc bằng độ sai lệch thì sẽ tiến hành gồm điểm A -> B thành 1 điểm
                    if (tempDistance <= maxAccuracy)
                    {
                        dt.Rows[i]["TRACKING_ACCURACY"] = maxAccuracy.ToString(); //gán lại giá trị Accuracy max sau khi gôm A và B thành 1 điểm


                        //DEVICE_STATUS : thông tin về trạng thái máy như: Reboot, Shutdown, turnoff GPS, turnon GPS khi user HH thực hiện
                        //sau khi gôm A và B lại thì tiến hành gôm DEVICE_STATUS lại thành 1 chuổi duy nhất
                        if (dt.Rows[i + 1]["DEVICE_STATUS"].ToString() != "")
                            if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i + 1]["DEVICE_STATUS"].ToString();
                            else
                                dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[i + 1]["DEVICE_STATUS"].ToString();

                        //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                        dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[i + 1]["BATTERY_PERCENTAGE"].ToString();
                        dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[i + 1]["BEGIN_DATETIME"].ToString();


                        //TRACKING_PROVIDER_VALUE và TRACKING_PROVIDER_DATETIME sẽ được xử lý ở phía sau
                        //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                        dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[i + 1]["TRACKING_PROVIDER"].ToString();
                        dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[i + 1]["BEGIN_DATETIME"].ToString();


                        //NO_REPEAT: ghi nhận số điểm dừng 
                        dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[i + 1]["NO_REPEAT"].ToString());

                        //END_DATETIME: thời gian kết thúc điểm sau khi gôm A và B lại sẽ là thời gian kết thúc của điểm B
                        dt.Rows[i]["END_DATETIME"] = dt.Rows[i + 1]["END_DATETIME"].ToString();

                        //gôm cập nhật lai địa chỉ
                        //LOCATION_ADDRESS: địa chỉ của tọa độ sau khi gôm A và B lại
                        if (dt.Rows[i + 1]["LOCATION_ADDRESS"].ToString().Length > 0)
                            dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[i + 1]["LOCATION_ADDRESS"].ToString();

                        dt.Rows.RemoveAt(i + 1); // xóa điểm B sau khi gôm

                    }
                    else
                        i++;

                }
                catch (Exception)
                {
                    i++;
                }

            }


            //BATTERY_PERCENTAGE_VALUE và BATTERY_PERCENTAGE_DATETIME cho dòng cuối cùng và sẽ được xử dụng ở bên dưới
            if (dt != null && dt.Rows.Count >= 1)
            {
                //set default BATTERY_PERCENTAGE
                if (dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE"];
                    dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[dt.Rows.Count - 1]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_VALUE"] = dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER"];
                    dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[dt.Rows.Count - 1]["BEGIN_DATETIME"];
                }


            }




            dt.Columns["TYPE_TRACKING"].MaxLength = 1024;
            dt.Columns["POINT_RADIUS"].MaxLength = 1024;
            dt.Columns["ANGEL"].MaxLength = 1024;

            //lấy thông tin về bán kính hệ thống
            DataTable dtParam = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_RADIUS' AND ACTIVE = 1 ");

            //lấy thông tin về thời gian giữa các điểm hiện tại là 20' 
            // mục đích là: nếu A và B không trùng nhau và khoảng thời gian giữa A và B nhở hơn 0.2 minutes ~= 12s thì sẽ gôm thành 1 điểm
            DataTable dtTimePoint = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_TIME_POINT' AND ACTIVE = 1 ");
            DataTable dtResult = new DataTable();
            dtResult = dt.Clone();
            dtResult.Clear();



            if (dtParam != null && dtParam.Rows.Count == 1)
            {
                Double pointRadius = Double.Parse(dtParam.Rows[0]["VALUE"].ToString());
                Double pointTime = Double.Parse(dtTimePoint.Rows[0]["VALUE"].ToString());

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dtResult.Columns.Count; i++)
                {
                    dtResult.Columns[i].ReadOnly = false;
                }


                //process show icon 

                //nếu dữ liệu chỉ có 1 điểm duy nhất thì không cần phải gôm các điểm lại theo bán kính
                if (dt.Rows.Count == 1)
                {

                    dtResult = dt;
                    dtResult.Rows[0]["POINT_RADIUS"] = pointRadius.ToString(); //thiết lập bán kính hệ thống từ tham số
                }
                else
                {

                    for (int i = 0; i < dt.Rows.Count;)
                    {
                        //nếu như tọa độ không hợp lệ thì bỏ qua
                        if (!dt.Rows[i]["LONGITUDE_LATITUDE"].ToString().Contains(","))
                            continue;

                        dt.Rows[i]["POINT_RADIUS"] = pointRadius.ToString();
                        P5sLocation location1 = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());

                        int index = i + 1;
                        for (int j = index; j < dt.Rows.Count; j++)
                        {
                            //nếu như tọa độ không hợp lệ thì bỏ qua
                            if (!dt.Rows[j]["LONGITUDE_LATITUDE"].ToString().Contains(","))
                                continue;
                            P5sLocation location2 = new P5sLocation(dt.Rows[j]["LONGITUDE_LATITUDE"].ToString());
                            double tempDistance = P5sTrackingHelper.CalculateDistance(location1, location2) * 1000; //tính toán khoảng cách giữa 2 điểm

                            //tieensh hành gom điểm nếu như khoảng giữa A và B nhỏ hơn bán kính hệ thống
                            if (tempDistance <= pointRadius)
                            {

                                dt.Rows[i]["END_DATETIME"] = dt.Rows[j]["END_DATETIME"];

                                //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                                dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[j]["BATTERY_PERCENTAGE"];
                                dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"];

                                //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                                dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[j]["TRACKING_PROVIDER"].ToString();
                                dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"].ToString();


                                if (dt.Rows[j]["DEVICE_STATUS"].ToString() != "")
                                    if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                        dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[j]["DEVICE_STATUS"].ToString();
                                    else
                                        dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[j]["DEVICE_STATUS"].ToString();

                                dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[j]["NO_REPEAT"].ToString());

                                //gôm cập nhật lai địa chỉ
                                if (dt.Rows[j]["LOCATION_ADDRESS"].ToString().Length > 0)
                                    dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[j]["LOCATION_ADDRESS"].ToString();


                                index = j + 1;

                            }
                            else
                            {
                                //compare time two point
                                DateTime dtime1 = DateTime.Parse(dt.Rows[i]["BEGIN_DATETIME"].ToString());
                                DateTime dtime2 = DateTime.Parse(dt.Rows[j]["BEGIN_DATETIME"].ToString());
                                TimeSpan diffResult = dtime2.Subtract(dtime1);
                                //nếu thời gian giữa A và B nhở hơn 0.2 phút ~= 12s thì sẽ tính thành 1 điểm
                                if (diffResult.TotalMinutes <= pointTime)
                                {
                                    //cập nhật thời gian két thúc của A = thời gian kết thúc của B
                                    dt.Rows[i]["END_DATETIME"] = dt.Rows[j]["END_DATETIME"];

                                    //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                                    dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[j]["BATTERY_PERCENTAGE"];
                                    dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"];

                                    //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                                    dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[j]["TRACKING_PROVIDER"].ToString();
                                    dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"].ToString();


                                    if (dt.Rows[j]["DEVICE_STATUS"].ToString() != "")
                                        if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                            dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[j]["DEVICE_STATUS"].ToString();
                                        else
                                            dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[j]["DEVICE_STATUS"].ToString();

                                    //cập nhật lại số điểm (point) sau khi gôm điểm
                                    dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[j]["NO_REPEAT"].ToString());

                                    //gôm cập nhật lai địa chỉ
                                    if (dt.Rows[j]["LOCATION_ADDRESS"].ToString().Length > 0)
                                        dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[j]["LOCATION_ADDRESS"].ToString();


                                    index = j + 1;

                                }
                                else
                                    break;
                            }
                        }
                        //sau khi xử lý xong thì đưa vào datatable Result để lưu kết quả
                        dtResult.Rows.Add(
                                           dt.Rows[i]["YYMMDD"],
                                           dt.Rows[i]["SALES_CD"],
                                           dt.Rows[i]["DISTRIBUTOR_CD"],
                                           dt.Rows[i]["LONGITUDE_LATITUDE"],
                                           dt.Rows[i]["DEVICE_STATUS"],
                                           dt.Rows[i]["NO_REPEAT"],
                                           dt.Rows[i]["BEGIN_DATETIME"],
                                           dt.Rows[i]["END_DATETIME"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE"],
                                           dt.Rows[i]["TYPE_TRACKING"],
                                           dt.Rows[i]["POINT_RADIUS"],
                                           dt.Rows[i]["TRACKING_ACCURACY"],
                                           dt.Rows[i]["ANGEL"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"],
                                           dt.Rows[i]["TRACKING_PROVIDER"],
                                           dt.Rows[i]["TRACKING_PROVIDER_VALUE"],
                                           dt.Rows[i]["TRACKING_PROVIDER_DATETIME"],
                                           dt.Rows[i]["LOCATION_ADDRESS"]
                                        );
                        i = index;
                    }

                }
            }
            else
                dtResult = dt;




            //verify start point & end point
            //lấy thông số thời gian nghỉ giữa 1 điểm
            dtParam = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_BREAK_TIME' AND ACTIVE = 1 ");
            if (dtParam != null && dtParam.Rows.Count == 1)
            {
                Double breakingTime = Double.Parse(dtParam.Rows[0]["VALUE"].ToString());

                //hàm xử lý dữ liệu cho biết đâu là điểm bắt đầu, điểm kết thúc, stoppoint
                for (int i = 0; i < dtResult.Rows.Count;)
                {
                    // điểm đầu tiên mặc định là điểm start
                    if (i == 0)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingStart;
                        i++;
                        continue;
                    }


                    // điểm kết thúc mặc định là điểm end
                    if (i == dtResult.Rows.Count - 1)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingEnd;
                        i++;
                        continue;
                    }

                    DateTime dtime1 = DateTime.Parse(dtResult.Rows[i]["END_DATETIME"].ToString());
                    DateTime dtime2 = DateTime.Parse(dtResult.Rows[i + 1]["BEGIN_DATETIME"].ToString());

                    TimeSpan diffResult = dtime2.Subtract(dtime1);
                    //nếu giữa 2 điểm A và B có khoảng thời gian lớn hơn thời gian breaking time thì sẽ ghi nhận A là Start, B là End
                    if (diffResult.TotalMinutes >= breakingTime)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingEnd;
                        dtResult.Rows[i + 1]["TYPE_TRACKING"] = P5sEnum.TrackingStart;
                        i = i + 2;
                        continue;
                    }
                    else //ngược lại thì đây là 1 điểm bình thường
                    {

                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingPoint;
                        i++;
                        continue;
                    }

                }
            }

            //Điểm đầu và điểm cuối trùng nhau thì ghi nhận điểm này là StartEnd
            if (dtResult.Rows.Count > 0 && dtResult.Rows[dtResult.Rows.Count - 1]["TYPE_TRACKING"].ToString() == P5sEnum.TrackingStart)
            {
                dtResult.Rows[dtResult.Rows.Count - 1]["TYPE_TRACKING"] = P5sEnum.TrackingStartEnd;

            }

            // tính tọa độ gốc để hiển thị mũi lên đường đi trên bảng đồ
            //Calculate Angle
            for (int i = 0; i < dtResult.Rows.Count - 1; i++)
            {
                P5sLocation location1 = new P5sLocation(dtResult.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                P5sLocation location2 = new P5sLocation(dtResult.Rows[i + 1]["LONGITUDE_LATITUDE"].ToString());
                dtResult.Rows[i]["ANGEL"] = P5sCmmFns.CalculateAngle(location1, location2);
            }

            //exit nếu như dữ liệu chốt số không  có 
            if (dtResult == null || dtResult.Rows.Count == 0)
                return;


            //tạo cột GUID 
            //add new column
            String sessionCd = System.Guid.NewGuid().ToString();
            DataColumn colSessionCd = new DataColumn("SESSION_CD");
            colSessionCd.DataType = typeof(String);
            colSessionCd.DefaultValue = sessionCd;
            dtResult.Columns.Add(colSessionCd);


            //xử dụng buildcopy để đưa dữ liệu vào bảng tạm
            using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
            {
                connection.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                //I assume you have created the table previously 
                //Someone else here already showed how   
                bulkcopy.DestinationTableName = "TMP_DE_TRACKING_OF_SALES";


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YYMMDD", "YYMMDD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LONGITUDE_LATITUDE", "LONGITUDE_LATITUDE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DEVICE_STATUS", "DEVICE_STATUS"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NO_REPEAT", "NO_REPEAT"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BEGIN_DATETIME", "BEGIN_DATETIME"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("END_DATETIME", "END_DATETIME"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE", "BATTERY_PERCENTAGE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_TRACKING", "TYPE_TRACKING"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("POINT_RADIUS", "POINT_RADIUS"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ANGEL", "ANGEL"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_ACCURACY", "TRACKING_ACCURACY"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE_VALUE", "BATTERY_PERCENTAGE_VALUE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE_DATETIME", "BATTERY_PERCENTAGE_DATETIME"));


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER", "TRACKING_PROVIDER"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER_VALUE", "TRACKING_PROVIDER_VALUE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER_DATETIME", "TRACKING_PROVIDER_DATETIME"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LOCATION_ADDRESS", "LOCATION_ADDRESS"));

                try
                {

                    //DELETE DATA BEFORE INSERT
                    //xóa các dữ liệu đã được chốt số trước đó
                    L5sSql.Execute(String.Format(@"
                                    DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{2} WHERE YYMMDD = {0} AND SALES_CD = {1} 
                                    DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{3} WHERE YYMMDD = {0} AND SALES_CD = {1} 
                                    ", yymmdd, salesCD
                                     , tableNameTrackingOfSales
                                     , tableNameTrackingOfStopOfSales));


                    bulkcopy.WriteToServer(dtResult);

                    //thêm dữ liệu vào bảng tương ứng                    
                    //INSERT DATA DE_TRACKING_STOP_OF_SALES -DÙNG CHO CÁC BÁO CÁO STOP POINT để tối ưu hệ thống khi xuất báo cáo
                    sql = String.Format(@"
                                         DECLARE @TRACKING_POINT_DURATION INT 
                                         SET @TRACKING_POINT_DURATION = 2
                                         SELECT  @TRACKING_POINT_DURATION = CAST( VALUE AS INT) FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_DURATION' 
                                

                                       INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{4} -- [dbo].[DE_TRACKING_OF_SALES]
                                                       ([YYMMDD]
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
                                                       ,[CREATED_DATE],[LOCATION_ADDRESS])
                                             SELECT   
		                                              [YYMMDD]
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
		                                              ,[CREATED_DATE],[LOCATION_ADDRESS]
                                              FROM [dbo].[TMP_DE_TRACKING_OF_SALES]
                                              WHERE SALES_CD = {0} AND YYMMDD = {1} 
                                                    AND SESSION_CD = '{3}'

                                        
                                       INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{5}  --[dbo].[DE_TRACKING_STOP_OF_SALES]
                                                       ([YYMMDD]
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
                                                       ,[CREATED_DATE],[LOCATION_ADDRESS])
                                             SELECT   
		                                              [YYMMDD]
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
		                                              ,[CREATED_DATE],[LOCATION_ADDRESS]
                                              FROM [dbo].[TMP_DE_TRACKING_OF_SALES]
                                              WHERE SALES_CD = {0} AND YYMMDD = {1} AND TYPE_TRACKING = '{2}' 
                                                    AND DATEDIFF(SECOND,BEGIN_DATETIME,  END_DATETIME ) >= @TRACKING_POINT_DURATION*60
                                                    AND SESSION_CD = '{3}'

                                              DELETE  
                                              FROM [dbo].[TMP_DE_TRACKING_OF_SALES]
                                              WHERE SALES_CD = {0} AND YYMMDD = {1}
                                                    AND SESSION_CD = '{3}' 
 

                                                    ", salesCD
                                                     , yymmdd
                                                     , P5sEnum.TrackingPoint, sessionCd
                                                     , tableNameTrackingOfSales
                                                     , tableNameTrackingOfStopOfSales);

                    L5sSql.Execute(sql);


                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void P5sConsolidateTrackingOfSupervisor(DataTable row, String supervisorCD, String yymmdd)
        {
            String tableNameTracking = row.Rows[0]["TABLE_NAME_1"].ToString();
            String tableNameTrackingOfSales = row.Rows[0]["TABLE_NAME_2"].ToString();
            String tableNameTrackingOfStopOfSales = row.Rows[0]["TABLE_NAME_3"].ToString();


            String sql = String.Format(@"
                                            DECLARE @SUPERVISOR_CD BIGINT = {0}
                                            DECLARE @YYMMDD INT = {1}

                                        
                                            SELECT 
	                                            YYMMDD = @YYMMDD,SUPERVISOR_CD,DISTRIBUTOR_CD,
	                                            LONGITUDE_LATITUDE,
	                                            DEVICE_STATUS, 
	                                            NO_REPEAT,
	                                            BEGIN_DATETIME, 
	                                            END_DATETIME,
	                                            CAST( BATTERY_PERCENTAGE AS INT) AS BATTERY_PERCENTAGE,
	                                            TYPE_TRACKING = '', 
	                                            POINT_RADIUS = '' ,
	                                            ANGEL = '0', 
	                                            TRACKING_ACCURACY,                                                    
	                                            BATTERY_PERCENTAGE_VALUE = '',
	                                            BATTERY_PERCENTAGE_DATETIME = '',
	                                            TRACKING_PROVIDER,
	                                            TRACKING_PROVIDER_VALUE =  '',
	                                            TRACKING_PROVIDER_DATETIME =  '', 
                                                [LOCATION_ADDRESS]                                            
                                                                            	               
                                            FROM    [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{2} 
                                            WHERE   LONGITUDE_LATITUDE != '' AND SUPERVISOR_CD = @SUPERVISOR_CD  AND YYMMDD = @YYMMDD   
                                            ORDER BY BEGIN_DATETIME  


                                        ", supervisorCD, yymmdd, tableNameTracking);

            DataTable dt = L5sSql.Query(sql);


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ReadOnly = false;
            }

            //set maxlength for colum
            dt.Columns["TYPE_TRACKING"].MaxLength = int.MaxValue;
            dt.Columns["BATTERY_PERCENTAGE_VALUE"].MaxLength = int.MaxValue;
            dt.Columns["BATTERY_PERCENTAGE_DATETIME"].MaxLength = int.MaxValue;
            dt.Columns["TRACKING_PROVIDER_VALUE"].MaxLength = int.MaxValue;
            dt.Columns["TRACKING_PROVIDER_DATETIME"].MaxLength = int.MaxValue;



            if (dt.Rows.Count == 1)
            {
                //set default BATTERY_PERCENTAGE
                if (dt.Rows[0]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[0]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[0]["BATTERY_PERCENTAGE"];
                    dt.Rows[0]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[0]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[0]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[0]["TRACKING_PROVIDER_VALUE"] = dt.Rows[0]["TRACKING_PROVIDER"];
                    dt.Rows[0]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[0]["BEGIN_DATETIME"];
                }


            }

            //gôm các điểm theo Accuracy
            for (int i = 0; i < dt.Rows.Count - 1;)
            {
                P5sLocation location1 = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                P5sLocation location2 = new P5sLocation(dt.Rows[i + 1]["LONGITUDE_LATITUDE"].ToString());
                double tempDistance = P5sTrackingHelper.CalculateDistance(location1, location2) * 1000;

                //set default BATTERY_PERCENTAGE
                if (dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE"];
                    dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[i]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER"];
                    dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["BEGIN_DATETIME"];
                }


                try
                {
                    Double accuracy1 = Double.Parse(dt.Rows[i]["TRACKING_ACCURACY"].ToString());
                    Double accuracy2 = Double.Parse(dt.Rows[i + 1]["TRACKING_ACCURACY"].ToString());
                    Double maxAccuracy = accuracy1 > accuracy2 ? accuracy1 : accuracy2;

                    if (tempDistance <= maxAccuracy)
                    {
                        dt.Rows[i]["TRACKING_ACCURACY"] = maxAccuracy.ToString();


                        if (dt.Rows[i + 1]["DEVICE_STATUS"].ToString() != "")
                            if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i + 1]["DEVICE_STATUS"].ToString();
                            else
                                dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[i + 1]["DEVICE_STATUS"].ToString();

                        //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                        dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[i + 1]["BATTERY_PERCENTAGE"].ToString();
                        dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[i + 1]["BEGIN_DATETIME"].ToString();


                        //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                        dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[i + 1]["TRACKING_PROVIDER"].ToString();
                        dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[i + 1]["BEGIN_DATETIME"].ToString();


                        dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[i + 1]["NO_REPEAT"].ToString());
                        dt.Rows[i]["END_DATETIME"] = dt.Rows[i + 1]["END_DATETIME"].ToString();


                        //gôm cập nhật lai địa chỉ
                        if (dt.Rows[i + 1]["LOCATION_ADDRESS"].ToString().Length > 0)
                            dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[+1]["LOCATION_ADDRESS"].ToString();


                        dt.Rows.RemoveAt(i + 1);

                    }
                    else
                        i++;

                }
                catch (Exception)
                {
                    i++;
                }

            }



            if (dt != null && dt.Rows.Count >= 1)
            {
                //set default BATTERY_PERCENTAGE
                if (dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE"];
                    dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[dt.Rows.Count - 1]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_VALUE"] = dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER"];
                    dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[dt.Rows.Count - 1]["BEGIN_DATETIME"];
                }


            }




            dt.Columns["TYPE_TRACKING"].MaxLength = 1024;
            dt.Columns["POINT_RADIUS"].MaxLength = 1024;
            dt.Columns["ANGEL"].MaxLength = 1024;
            //radius
            DataTable dtParam = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_RADIUS' AND ACTIVE = 1 ");
            DataTable dtTimePoint = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_TIME_POINT' AND ACTIVE = 1 ");
            DataTable dtResult = new DataTable();
            dtResult = dt.Clone();
            dtResult.Clear();



            if (dtParam != null && dtParam.Rows.Count == 1)
            {
                Double pointRadius = Double.Parse(dtParam.Rows[0]["VALUE"].ToString());
                Double pointTime = Double.Parse(dtTimePoint.Rows[0]["VALUE"].ToString());

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dtResult.Columns.Count; i++)
                {
                    dtResult.Columns[i].ReadOnly = false;
                }


                //process show icon 

                if (dt.Rows.Count == 1)
                {

                    dtResult = dt;
                    dtResult.Rows[0]["POINT_RADIUS"] = pointRadius.ToString();
                }
                else
                {

                    for (int i = 0; i < dt.Rows.Count;)
                    {
                        if (!dt.Rows[i]["LONGITUDE_LATITUDE"].ToString().Contains(","))
                            continue;

                        dt.Rows[i]["POINT_RADIUS"] = pointRadius.ToString();
                        P5sLocation location1 = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());

                        int index = i + 1;
                        for (int j = index; j < dt.Rows.Count; j++)
                        {
                            if (!dt.Rows[j]["LONGITUDE_LATITUDE"].ToString().Contains(","))
                                continue;
                            P5sLocation location2 = new P5sLocation(dt.Rows[j]["LONGITUDE_LATITUDE"].ToString());
                            double tempDistance = P5sTrackingHelper.CalculateDistance(location1, location2) * 1000;

                            if (tempDistance <= pointRadius)
                            {

                                dt.Rows[i]["END_DATETIME"] = dt.Rows[j]["END_DATETIME"];

                                //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                                dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[j]["BATTERY_PERCENTAGE"];
                                dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"];

                                //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                                dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[j]["TRACKING_PROVIDER"].ToString();
                                dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"].ToString();


                                if (dt.Rows[j]["DEVICE_STATUS"].ToString() != "")
                                    if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                        dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[j]["DEVICE_STATUS"].ToString();
                                    else
                                        dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[j]["DEVICE_STATUS"].ToString();

                                dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[j]["NO_REPEAT"].ToString());

                                //gôm cập nhật lai địa chỉ
                                if (dt.Rows[j]["LOCATION_ADDRESS"].ToString().Length > 0)
                                    dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[j]["LOCATION_ADDRESS"].ToString();


                                index = j + 1;

                            }
                            else
                            {
                                //compare time two point
                                DateTime dtime1 = DateTime.Parse(dt.Rows[i]["BEGIN_DATETIME"].ToString());
                                DateTime dtime2 = DateTime.Parse(dt.Rows[j]["BEGIN_DATETIME"].ToString());
                                TimeSpan diffResult = dtime2.Subtract(dtime1);
                                if (diffResult.TotalMinutes <= pointTime)
                                {

                                    dt.Rows[i]["END_DATETIME"] = dt.Rows[j]["END_DATETIME"];

                                    //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                                    dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[j]["BATTERY_PERCENTAGE"];
                                    dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"];

                                    //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                                    dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[j]["TRACKING_PROVIDER"].ToString();
                                    dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"].ToString();


                                    if (dt.Rows[j]["DEVICE_STATUS"].ToString() != "")
                                        if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                            dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[j]["DEVICE_STATUS"].ToString();
                                        else
                                            dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[j]["DEVICE_STATUS"].ToString();

                                    dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[j]["NO_REPEAT"].ToString());

                                    //gôm cập nhật lai địa chỉ
                                    if (dt.Rows[j]["LOCATION_ADDRESS"].ToString().Length > 0)
                                        dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[j]["LOCATION_ADDRESS"].ToString();


                                    index = j + 1;

                                }
                                else
                                    break;
                            }
                        }
                        dtResult.Rows.Add(
                                           dt.Rows[i]["YYMMDD"],
                                           dt.Rows[i]["SUPERVISOR_CD"],
                                           dt.Rows[i]["DISTRIBUTOR_CD"],
                                           dt.Rows[i]["LONGITUDE_LATITUDE"],
                                           dt.Rows[i]["DEVICE_STATUS"],
                                           dt.Rows[i]["NO_REPEAT"],
                                           dt.Rows[i]["BEGIN_DATETIME"],
                                           dt.Rows[i]["END_DATETIME"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE"],
                                           dt.Rows[i]["TYPE_TRACKING"],
                                           dt.Rows[i]["POINT_RADIUS"],
                                           dt.Rows[i]["TRACKING_ACCURACY"],
                                           dt.Rows[i]["ANGEL"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"],
                                           dt.Rows[i]["TRACKING_PROVIDER"],
                                           dt.Rows[i]["TRACKING_PROVIDER_VALUE"],
                                           dt.Rows[i]["TRACKING_PROVIDER_DATETIME"],
                                           dt.Rows[i]["LOCATION_ADDRESS"]
                                        );
                        i = index;
                    }

                }
            }
            else
                dtResult = dt;




            //verify start point & end point
            dtParam = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_BREAK_TIME' AND ACTIVE = 1 ");
            if (dtParam != null && dtParam.Rows.Count == 1)
            {
                Double breakingTime = Double.Parse(dtParam.Rows[0]["VALUE"].ToString());
                for (int i = 0; i < dtResult.Rows.Count;)
                {

                    if (i == 0)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingStart;
                        i++;
                        continue;
                    }

                    if (i == dtResult.Rows.Count - 1)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingEnd;
                        i++;
                        continue;
                    }

                    DateTime dtime1 = DateTime.Parse(dtResult.Rows[i]["END_DATETIME"].ToString());
                    DateTime dtime2 = DateTime.Parse(dtResult.Rows[i + 1]["BEGIN_DATETIME"].ToString());

                    TimeSpan diffResult = dtime2.Subtract(dtime1);
                    if (diffResult.TotalMinutes >= breakingTime)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingEnd;
                        dtResult.Rows[i + 1]["TYPE_TRACKING"] = P5sEnum.TrackingStart;
                        i = i + 2;
                        continue;
                    }
                    else
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingPoint;
                        i++;
                        continue;
                    }

                }
            }

            //Điểm đầu và điểm cuối trùng nhau
            if (dtResult.Rows.Count > 0 && dtResult.Rows[dtResult.Rows.Count - 1]["TYPE_TRACKING"].ToString() == P5sEnum.TrackingStart)
            {
                dtResult.Rows[dtResult.Rows.Count - 1]["TYPE_TRACKING"] = P5sEnum.TrackingStartEnd;

            }


            //Calculate Angle
            for (int i = 0; i < dtResult.Rows.Count - 1; i++)
            {
                P5sLocation location1 = new P5sLocation(dtResult.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                P5sLocation location2 = new P5sLocation(dtResult.Rows[i + 1]["LONGITUDE_LATITUDE"].ToString());
                dtResult.Rows[i]["ANGEL"] = P5sCmmFns.CalculateAngle(location1, location2);
            }

            if (dtResult == null || dtResult.Rows.Count == 0)
                return;

            //add new column
            String sessionCd = System.Guid.NewGuid().ToString();
            DataColumn colSessionCd = new DataColumn("SESSION_CD");
            colSessionCd.DataType = typeof(String);
            colSessionCd.DefaultValue = sessionCd;
            dtResult.Columns.Add(colSessionCd);

            using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
            {
                connection.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                //I assume you have created the table previously 
                //Someone else here already showed how   
                bulkcopy.DestinationTableName = "TMP_DE_TRACKING_OF_SUPERVISOR";



                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SUPERVISOR_CD", "SUPERVISOR_CD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YYMMDD", "YYMMDD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LONGITUDE_LATITUDE", "LONGITUDE_LATITUDE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DEVICE_STATUS", "DEVICE_STATUS"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NO_REPEAT", "NO_REPEAT"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BEGIN_DATETIME", "BEGIN_DATETIME"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("END_DATETIME", "END_DATETIME"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE", "BATTERY_PERCENTAGE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_TRACKING", "TYPE_TRACKING"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("POINT_RADIUS", "POINT_RADIUS"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ANGEL", "ANGEL"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_ACCURACY", "TRACKING_ACCURACY"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE_VALUE", "BATTERY_PERCENTAGE_VALUE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE_DATETIME", "BATTERY_PERCENTAGE_DATETIME"));


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER", "TRACKING_PROVIDER"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER_VALUE", "TRACKING_PROVIDER_VALUE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER_DATETIME", "TRACKING_PROVIDER_DATETIME"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LOCATION_ADDRESS", "LOCATION_ADDRESS"));

                try
                {

                    //DELETE DATA BEFORE INSERT
                    sql = String.Format(@"
                                    DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{2} WHERE YYMMDD = {0} AND SUPERVISOR_CD = {1}
                                    DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{3} WHERE YYMMDD = {0} AND SUPERVISOR_CD = {1}
                                    ", yymmdd, supervisorCD, tableNameTrackingOfSales
                                     , tableNameTrackingOfStopOfSales);

                    L5sSql.Execute(sql);

                    bulkcopy.WriteToServer(dtResult);

                    //INSERT DATA DE_TRACKING_STOP_OF_SALES -DÙNG CHO CÁC BÁO CÁO STOP POINT

                    sql = String.Format(@"
                                     DECLARE @TRACKING_POINT_DURATION INT 
                                     SET @TRACKING_POINT_DURATION = 2
                                     SELECT  @TRACKING_POINT_DURATION = CAST( VALUE AS INT) FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_DURATION' 
                            

                                            INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{3}
                                                       ([YYMMDD]
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
                                                       ,[CREATED_DATE],[LOCATION_ADDRESS])
                                             SELECT   
		                                              [YYMMDD]
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
		                                              ,[CREATED_DATE],[LOCATION_ADDRESS]
                                              FROM [dbo].[TMP_DE_TRACKING_OF_SUPERVISOR]
                                              WHERE SUPERVISOR_CD = {0} AND YYMMDD = {1}
                                                    AND SESSION_CD = '{2}'

                                       INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{4} 
                                                       ([YYMMDD]
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
                                                       ,[CREATED_DATE],[LOCATION_ADDRESS])
                                             SELECT   
		                                              [YYMMDD]
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
		                                              ,[CREATED_DATE],[LOCATION_ADDRESS]
                                              FROM [dbo].[TMP_DE_TRACKING_OF_SUPERVISOR]
                                              WHERE SUPERVISOR_CD = {0} AND YYMMDD = {1} AND TYPE_TRACKING = 'P'
                                                    AND DATEDIFF(SECOND,BEGIN_DATETIME,  END_DATETIME ) >= @TRACKING_POINT_DURATION*60
                                                    AND SESSION_CD = '{2}'
                                                                    
                                              DELETE  
                                              FROM [dbo].[TMP_DE_TRACKING_OF_SUPERVISOR]
                                              WHERE SUPERVISOR_CD = {0} AND YYMMDD = {1} 
                                                    AND SESSION_CD = '{2}'                     

                                         
                                                    
                                    ", supervisorCD, yymmdd, sessionCd, tableNameTrackingOfSales, tableNameTrackingOfStopOfSales);

                    L5sSql.Execute(sql);


                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void P5sConsolidateTrackingOfASM(DataTable row, String asmCD, String yymmdd)
        {
            String tableNameTracking = row.Rows[0]["TABLE_NAME_1"].ToString();
            String tableNameTrackingOfSales = row.Rows[0]["TABLE_NAME_2"].ToString();
            String tableNameTrackingOfStopOfSales = row.Rows[0]["TABLE_NAME_3"].ToString();

            String sql = String.Format(@"
                                            DECLARE @ASM_CD BIGINT = {0}
                                            DECLARE @YYMMDD INT = {1}

                              


                                            SELECT 
	                                            YYMMDD = @YYMMDD,ASM_CD,DISTRIBUTOR_CD,
	                                            LONGITUDE_LATITUDE,
	                                            DEVICE_STATUS, 
	                                            NO_REPEAT,
	                                            BEGIN_DATETIME, 
	                                            END_DATETIME,
	                                            CAST( BATTERY_PERCENTAGE AS INT) AS BATTERY_PERCENTAGE,
	                                            TYPE_TRACKING = '', 
	                                            POINT_RADIUS = '' ,
	                                            ANGEL = '0', 
	                                            TRACKING_ACCURACY,                                                    
	                                            BATTERY_PERCENTAGE_VALUE = '',
	                                            BATTERY_PERCENTAGE_DATETIME = '',
	                                            TRACKING_PROVIDER,
	                                            TRACKING_PROVIDER_VALUE =  '',
	                                            TRACKING_PROVIDER_DATETIME =  '',
                                                [LOCATION_ADDRESS]                                            
                                                                            	               
                                            FROM    [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{2}
                                            WHERE   LONGITUDE_LATITUDE != '' AND ASM_CD = @ASM_CD  AND YYMMDD = @YYMMDD   
                                            ORDER BY BEGIN_DATETIME        

                                            
                                        ", asmCD, yymmdd, tableNameTracking);

            DataTable dt = L5sSql.Query(sql);


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ReadOnly = false;
            }

            //set maxlength for colum
            dt.Columns["TYPE_TRACKING"].MaxLength = int.MaxValue;
            dt.Columns["BATTERY_PERCENTAGE_VALUE"].MaxLength = int.MaxValue;
            dt.Columns["BATTERY_PERCENTAGE_DATETIME"].MaxLength = int.MaxValue;
            dt.Columns["TRACKING_PROVIDER_VALUE"].MaxLength = int.MaxValue;
            dt.Columns["TRACKING_PROVIDER_DATETIME"].MaxLength = int.MaxValue;



            if (dt.Rows.Count == 1)
            {
                //set default BATTERY_PERCENTAGE
                if (dt.Rows[0]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[0]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[0]["BATTERY_PERCENTAGE"];
                    dt.Rows[0]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[0]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[0]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[0]["TRACKING_PROVIDER_VALUE"] = dt.Rows[0]["TRACKING_PROVIDER"];
                    dt.Rows[0]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[0]["BEGIN_DATETIME"];
                }


            }

            //gôm các điểm theo Accuracy
            for (int i = 0; i < dt.Rows.Count - 1;)
            {
                P5sLocation location1 = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                P5sLocation location2 = new P5sLocation(dt.Rows[i + 1]["LONGITUDE_LATITUDE"].ToString());
                double tempDistance = P5sTrackingHelper.CalculateDistance(location1, location2) * 1000;

                //set default BATTERY_PERCENTAGE
                if (dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE"];
                    dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[i]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER"];
                    dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["BEGIN_DATETIME"];
                }


                try
                {
                    Double accuracy1 = Double.Parse(dt.Rows[i]["TRACKING_ACCURACY"].ToString());
                    Double accuracy2 = Double.Parse(dt.Rows[i + 1]["TRACKING_ACCURACY"].ToString());
                    Double maxAccuracy = accuracy1 > accuracy2 ? accuracy1 : accuracy2;

                    if (tempDistance <= maxAccuracy)
                    {
                        dt.Rows[i]["TRACKING_ACCURACY"] = maxAccuracy.ToString();


                        if (dt.Rows[i + 1]["DEVICE_STATUS"].ToString() != "")
                            if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i + 1]["DEVICE_STATUS"].ToString();
                            else
                                dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[i + 1]["DEVICE_STATUS"].ToString();

                        //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                        dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[i + 1]["BATTERY_PERCENTAGE"].ToString();
                        dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[i + 1]["BEGIN_DATETIME"].ToString();


                        //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                        dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[i + 1]["TRACKING_PROVIDER"].ToString();
                        dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[i + 1]["BEGIN_DATETIME"].ToString();


                        dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[i + 1]["NO_REPEAT"].ToString());
                        dt.Rows[i]["END_DATETIME"] = dt.Rows[i + 1]["END_DATETIME"].ToString();


                        //gôm cập nhật lai địa chỉ
                        if (dt.Rows[i + 1]["LOCATION_ADDRESS"].ToString().Length > 0)
                            dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[i + 1]["LOCATION_ADDRESS"].ToString();



                        dt.Rows.RemoveAt(i + 1);

                    }
                    else
                        i++;

                }
                catch (Exception)
                {
                    i++;
                }

            }



            if (dt != null && dt.Rows.Count >= 1)
            {
                //set default BATTERY_PERCENTAGE
                if (dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE"];
                    dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[dt.Rows.Count - 1]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_VALUE"] = dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER"];
                    dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[dt.Rows.Count - 1]["BEGIN_DATETIME"];
                }


            }




            dt.Columns["TYPE_TRACKING"].MaxLength = 1024;
            dt.Columns["POINT_RADIUS"].MaxLength = 1024;
            dt.Columns["ANGEL"].MaxLength = 1024;
            //radius
            DataTable dtParam = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_RADIUS' AND ACTIVE = 1 ");
            DataTable dtTimePoint = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_TIME_POINT' AND ACTIVE = 1 ");
            DataTable dtResult = new DataTable();
            dtResult = dt.Clone();
            dtResult.Clear();



            if (dtParam != null && dtParam.Rows.Count == 1)
            {
                Double pointRadius = Double.Parse(dtParam.Rows[0]["VALUE"].ToString());
                Double pointTime = Double.Parse(dtTimePoint.Rows[0]["VALUE"].ToString());

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dtResult.Columns.Count; i++)
                {
                    dtResult.Columns[i].ReadOnly = false;
                }


                //process show icon 

                if (dt.Rows.Count == 1)
                {

                    dtResult = dt;
                    dtResult.Rows[0]["POINT_RADIUS"] = pointRadius.ToString();
                }
                else
                {

                    for (int i = 0; i < dt.Rows.Count;)
                    {
                        if (!dt.Rows[i]["LONGITUDE_LATITUDE"].ToString().Contains(","))
                            continue;

                        dt.Rows[i]["POINT_RADIUS"] = pointRadius.ToString();
                        P5sLocation location1 = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());

                        int index = i + 1;
                        for (int j = index; j < dt.Rows.Count; j++)
                        {
                            if (!dt.Rows[j]["LONGITUDE_LATITUDE"].ToString().Contains(","))
                                continue;
                            P5sLocation location2 = new P5sLocation(dt.Rows[j]["LONGITUDE_LATITUDE"].ToString());
                            double tempDistance = P5sTrackingHelper.CalculateDistance(location1, location2) * 1000;

                            if (tempDistance <= pointRadius)
                            {

                                dt.Rows[i]["END_DATETIME"] = dt.Rows[j]["END_DATETIME"];

                                //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                                dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[j]["BATTERY_PERCENTAGE"];
                                dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"];

                                //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                                dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[j]["TRACKING_PROVIDER"].ToString();
                                dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"].ToString();


                                if (dt.Rows[j]["DEVICE_STATUS"].ToString() != "")
                                    if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                        dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[j]["DEVICE_STATUS"].ToString();
                                    else
                                        dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[j]["DEVICE_STATUS"].ToString();

                                dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[j]["NO_REPEAT"].ToString());

                                //gôm cập nhật lai địa chỉ
                                if (dt.Rows[j]["LOCATION_ADDRESS"].ToString().Length > 0)
                                    dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[j]["LOCATION_ADDRESS"].ToString();



                                index = j + 1;

                            }
                            else
                            {
                                //compare time two point
                                DateTime dtime1 = DateTime.Parse(dt.Rows[i]["BEGIN_DATETIME"].ToString());
                                DateTime dtime2 = DateTime.Parse(dt.Rows[j]["BEGIN_DATETIME"].ToString());
                                TimeSpan diffResult = dtime2.Subtract(dtime1);
                                if (diffResult.TotalMinutes <= pointTime)
                                {

                                    dt.Rows[i]["END_DATETIME"] = dt.Rows[j]["END_DATETIME"];

                                    //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                                    dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[j]["BATTERY_PERCENTAGE"];
                                    dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"];

                                    //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                                    dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[j]["TRACKING_PROVIDER"].ToString();
                                    dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"].ToString();


                                    if (dt.Rows[j]["DEVICE_STATUS"].ToString() != "")
                                        if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                            dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[j]["DEVICE_STATUS"].ToString();
                                        else
                                            dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[j]["DEVICE_STATUS"].ToString();

                                    dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[j]["NO_REPEAT"].ToString());

                                    //gôm cập nhật lai địa chỉ
                                    if (dt.Rows[j]["LOCATION_ADDRESS"].ToString().Length > 0)
                                        dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[j]["LOCATION_ADDRESS"].ToString();


                                    index = j + 1;

                                }
                                else
                                    break;
                            }
                        }
                        dtResult.Rows.Add(
                                           dt.Rows[i]["YYMMDD"],
                                           dt.Rows[i]["ASM_CD"],
                                           dt.Rows[i]["DISTRIBUTOR_CD"],
                                           dt.Rows[i]["LONGITUDE_LATITUDE"],
                                           dt.Rows[i]["DEVICE_STATUS"],
                                           dt.Rows[i]["NO_REPEAT"],
                                           dt.Rows[i]["BEGIN_DATETIME"],
                                           dt.Rows[i]["END_DATETIME"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE"],
                                           dt.Rows[i]["TYPE_TRACKING"],
                                           dt.Rows[i]["POINT_RADIUS"],
                                           dt.Rows[i]["TRACKING_ACCURACY"],
                                           dt.Rows[i]["ANGEL"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"],
                                           dt.Rows[i]["TRACKING_PROVIDER"],
                                           dt.Rows[i]["TRACKING_PROVIDER_VALUE"],
                                           dt.Rows[i]["TRACKING_PROVIDER_DATETIME"],
                                           dt.Rows[i]["LOCATION_ADDRESS"]
                                        );
                        i = index;
                    }

                }
            }
            else
                dtResult = dt;




            //verify start point & end point
            dtParam = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_BREAK_TIME' AND ACTIVE = 1 ");
            if (dtParam != null && dtParam.Rows.Count == 1)
            {
                Double breakingTime = Double.Parse(dtParam.Rows[0]["VALUE"].ToString());
                for (int i = 0; i < dtResult.Rows.Count;)
                {

                    if (i == 0)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingStart;
                        i++;
                        continue;
                    }

                    if (i == dtResult.Rows.Count - 1)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingEnd;
                        i++;
                        continue;
                    }

                    DateTime dtime1 = DateTime.Parse(dtResult.Rows[i]["END_DATETIME"].ToString());
                    DateTime dtime2 = DateTime.Parse(dtResult.Rows[i + 1]["BEGIN_DATETIME"].ToString());

                    TimeSpan diffResult = dtime2.Subtract(dtime1);
                    if (diffResult.TotalMinutes >= breakingTime)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingEnd;
                        dtResult.Rows[i + 1]["TYPE_TRACKING"] = P5sEnum.TrackingStart;
                        i = i + 2;
                        continue;
                    }
                    else
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingPoint;
                        i++;
                        continue;
                    }

                }
            }

            //Điểm đầu và điểm cuối trùng nhau
            if (dtResult.Rows.Count > 0 && dtResult.Rows[dtResult.Rows.Count - 1]["TYPE_TRACKING"].ToString() == P5sEnum.TrackingStart)
            {
                dtResult.Rows[dtResult.Rows.Count - 1]["TYPE_TRACKING"] = P5sEnum.TrackingStartEnd;
            }


            //Calculate Angle
            for (int i = 0; i < dtResult.Rows.Count - 1; i++)
            {
                P5sLocation location1 = new P5sLocation(dtResult.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                P5sLocation location2 = new P5sLocation(dtResult.Rows[i + 1]["LONGITUDE_LATITUDE"].ToString());
                dtResult.Rows[i]["ANGEL"] = P5sCmmFns.CalculateAngle(location1, location2);
            }

            if (dtResult == null || dtResult.Rows.Count == 0)
                return;

            //add new column
            String sessionCd = System.Guid.NewGuid().ToString();
            DataColumn colSessionCd = new DataColumn("SESSION_CD");
            colSessionCd.DataType = typeof(String);
            colSessionCd.DefaultValue = sessionCd;
            dtResult.Columns.Add(colSessionCd);


            using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
            {
                connection.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                //I assume you have created the table previously 
                //Someone else here already showed how   
                bulkcopy.DestinationTableName = "TMP_DE_TRACKING_OF_ASM";

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ASM_CD", "ASM_CD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YYMMDD", "YYMMDD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LONGITUDE_LATITUDE", "LONGITUDE_LATITUDE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DEVICE_STATUS", "DEVICE_STATUS"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NO_REPEAT", "NO_REPEAT"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BEGIN_DATETIME", "BEGIN_DATETIME"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("END_DATETIME", "END_DATETIME"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE", "BATTERY_PERCENTAGE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_TRACKING", "TYPE_TRACKING"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("POINT_RADIUS", "POINT_RADIUS"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ANGEL", "ANGEL"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_ACCURACY", "TRACKING_ACCURACY"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE_VALUE", "BATTERY_PERCENTAGE_VALUE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE_DATETIME", "BATTERY_PERCENTAGE_DATETIME"));


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER", "TRACKING_PROVIDER"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER_VALUE", "TRACKING_PROVIDER_VALUE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER_DATETIME", "TRACKING_PROVIDER_DATETIME"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LOCATION_ADDRESS", "LOCATION_ADDRESS"));

                try
                {

                    //DELETE DATA BEFORE INSERT
                    sql = String.Format(@"  
                                           DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{2} WHERE YYMMDD = {0} AND ASM_CD = {1}
                                           DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{3} WHERE YYMMDD = {0} AND ASM_CD = {1} ",
                                           yymmdd, asmCD, tableNameTrackingOfSales, tableNameTrackingOfStopOfSales);
                    L5sSql.Execute(sql);
                    bulkcopy.WriteToServer(dtResult);

                    sql = String.Format(@"  DECLARE @TRACKING_POINT_DURATION INT 
                                     SET @TRACKING_POINT_DURATION = 2
                                     SELECT  @TRACKING_POINT_DURATION = CAST( VALUE AS INT) FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_DURATION' 
                            
                                        INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{3}
                                                       ([YYMMDD]
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
                                                       ,[CREATED_DATE]  ,[LOCATION_ADDRESS])
                                             SELECT   
		                                              [YYMMDD]
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
		                                              ,[CREATED_DATE]  ,[LOCATION_ADDRESS]
                                              FROM [dbo].[TMP_DE_TRACKING_OF_ASM]
                                              WHERE ASM_CD = {0} AND YYMMDD = {1} 
                                                    AND SESSION_CD = '{2}'
                            
                                       INSERT INTO  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{4} 
                                                       ([YYMMDD]
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
                                                       ,[CREATED_DATE]  ,[LOCATION_ADDRESS])
                                             SELECT   
		                                              [YYMMDD]
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
		                                              ,[CREATED_DATE]  ,[LOCATION_ADDRESS]
                                              FROM [dbo].[TMP_DE_TRACKING_OF_ASM]
                                              WHERE ASM_CD = {0} AND YYMMDD = {1} AND TYPE_TRACKING = 'P'
                                                    AND DATEDIFF(SECOND,BEGIN_DATETIME,  END_DATETIME ) >= @TRACKING_POINT_DURATION*60
                                                    AND SESSION_CD = '{2}'

                                            DELETE  
                                              FROM [dbo].[TMP_DE_TRACKING_OF_ASM]
                                              WHERE ASM_CD = {0} AND YYMMDD = {1} 
                                                    AND SESSION_CD = '{2}'   


                                    ", asmCD, yymmdd, sessionCd, tableNameTrackingOfSales, tableNameTrackingOfStopOfSales);

                    //INSERT DATA DE_TRACKING_STOP_OF_SALES -DÙNG CHO CÁC BÁO CÁO STOP POINT
                    L5sSql.Execute(sql);

                }
                catch (Exception ex)
                {
                }
            }
        }

        //201003 - Thêm RSM consolidate - copy từ P5sConsolidateTrackingOfASM
        public static void P5sConsolidateTrackingOfRSM(DataTable row, String RSMCD, String yymmdd)
        {
            String tableNameTracking = row.Rows[0]["TABLE_NAME_1"].ToString();
            String tableNameTrackingOfSales = row.Rows[0]["TABLE_NAME_2"].ToString();
            String tableNameTrackingOfStopOfSales = row.Rows[0]["TABLE_NAME_3"].ToString();

            String sql = String.Format(@"
                                            DECLARE @RSM_CD BIGINT = {0}
                                            DECLARE @YYMMDD INT = {1}

                              


                                            SELECT 
	                                            YYMMDD = @YYMMDD,RSM_CD,DISTRIBUTOR_CD,
	                                            LONGITUDE_LATITUDE,
	                                            DEVICE_STATUS, 
	                                            NO_REPEAT,
	                                            BEGIN_DATETIME, 
	                                            END_DATETIME,
	                                            CAST( BATTERY_PERCENTAGE AS INT) AS BATTERY_PERCENTAGE,
	                                            TYPE_TRACKING = '', 
	                                            POINT_RADIUS = '' ,
	                                            ANGEL = '0', 
	                                            TRACKING_ACCURACY,                                                    
	                                            BATTERY_PERCENTAGE_VALUE = '',
	                                            BATTERY_PERCENTAGE_DATETIME = '',
	                                            TRACKING_PROVIDER,
	                                            TRACKING_PROVIDER_VALUE =  '',
	                                            TRACKING_PROVIDER_DATETIME =  '',
                                                [LOCATION_ADDRESS]                                            
                                                                            	               
                                            FROM    [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{2}
                                            WHERE   LONGITUDE_LATITUDE != '' AND RSM_CD = @RSM_CD  AND YYMMDD = @YYMMDD   
                                            ORDER BY BEGIN_DATETIME        

                                            
                                        ", RSMCD, yymmdd, tableNameTracking);

            DataTable dt = L5sSql.Query(sql);


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ReadOnly = false;
            }

            //set maxlength for colum
            dt.Columns["TYPE_TRACKING"].MaxLength = int.MaxValue;
            dt.Columns["BATTERY_PERCENTAGE_VALUE"].MaxLength = int.MaxValue;
            dt.Columns["BATTERY_PERCENTAGE_DATETIME"].MaxLength = int.MaxValue;
            dt.Columns["TRACKING_PROVIDER_VALUE"].MaxLength = int.MaxValue;
            dt.Columns["TRACKING_PROVIDER_DATETIME"].MaxLength = int.MaxValue;



            if (dt.Rows.Count == 1)
            {
                //set default BATTERY_PERCENTAGE
                if (dt.Rows[0]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[0]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[0]["BATTERY_PERCENTAGE"];
                    dt.Rows[0]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[0]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[0]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[0]["TRACKING_PROVIDER_VALUE"] = dt.Rows[0]["TRACKING_PROVIDER"];
                    dt.Rows[0]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[0]["BEGIN_DATETIME"];
                }


            }

            //gôm các điểm theo Accuracy
            for (int i = 0; i < dt.Rows.Count - 1;)
            {
                P5sLocation location1 = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                P5sLocation location2 = new P5sLocation(dt.Rows[i + 1]["LONGITUDE_LATITUDE"].ToString());
                double tempDistance = P5sTrackingHelper.CalculateDistance(location1, location2) * 1000;

                //set default BATTERY_PERCENTAGE
                if (dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE"];
                    dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[i]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER"];
                    dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["BEGIN_DATETIME"];
                }


                try
                {
                    Double accuracy1 = Double.Parse(dt.Rows[i]["TRACKING_ACCURACY"].ToString());
                    Double accuracy2 = Double.Parse(dt.Rows[i + 1]["TRACKING_ACCURACY"].ToString());
                    Double maxAccuracy = accuracy1 > accuracy2 ? accuracy1 : accuracy2;

                    if (tempDistance <= maxAccuracy)
                    {
                        dt.Rows[i]["TRACKING_ACCURACY"] = maxAccuracy.ToString();


                        if (dt.Rows[i + 1]["DEVICE_STATUS"].ToString() != "")
                            if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i + 1]["DEVICE_STATUS"].ToString();
                            else
                                dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[i + 1]["DEVICE_STATUS"].ToString();

                        //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                        dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[i + 1]["BATTERY_PERCENTAGE"].ToString();
                        dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[i + 1]["BEGIN_DATETIME"].ToString();


                        //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                        dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[i + 1]["TRACKING_PROVIDER"].ToString();
                        dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[i + 1]["BEGIN_DATETIME"].ToString();


                        dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[i + 1]["NO_REPEAT"].ToString());
                        dt.Rows[i]["END_DATETIME"] = dt.Rows[i + 1]["END_DATETIME"].ToString();


                        //gôm cập nhật lai địa chỉ
                        if (dt.Rows[i + 1]["LOCATION_ADDRESS"].ToString().Length > 0)
                            dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[i + 1]["LOCATION_ADDRESS"].ToString();



                        dt.Rows.RemoveAt(i + 1);

                    }
                    else
                        i++;

                }
                catch (Exception)
                {
                    i++;
                }

            }



            if (dt != null && dt.Rows.Count >= 1)
            {
                //set default BATTERY_PERCENTAGE
                if (dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_VALUE"].ToString() == "")
                {
                    dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE"];
                    dt.Rows[dt.Rows.Count - 1]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[dt.Rows.Count - 1]["BEGIN_DATETIME"];
                }

                //set default TRACKING_PROVIDER_VALUE
                if (dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_VALUE"].ToString() == "")
                {
                    dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_VALUE"] = dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER"];
                    dt.Rows[dt.Rows.Count - 1]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[dt.Rows.Count - 1]["BEGIN_DATETIME"];
                }


            }




            dt.Columns["TYPE_TRACKING"].MaxLength = 1024;
            dt.Columns["POINT_RADIUS"].MaxLength = 1024;
            dt.Columns["ANGEL"].MaxLength = 1024;
            //radius
            DataTable dtParam = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_RADIUS' AND ACTIVE = 1 ");
            DataTable dtTimePoint = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_TIME_POINT' AND ACTIVE = 1 ");
            DataTable dtResult = new DataTable();
            dtResult = dt.Clone();
            dtResult.Clear();



            if (dtParam != null && dtParam.Rows.Count == 1)
            {
                Double pointRadius = Double.Parse(dtParam.Rows[0]["VALUE"].ToString());
                Double pointTime = Double.Parse(dtTimePoint.Rows[0]["VALUE"].ToString());

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dtResult.Columns.Count; i++)
                {
                    dtResult.Columns[i].ReadOnly = false;
                }


                //process show icon 

                if (dt.Rows.Count == 1)
                {

                    dtResult = dt;
                    dtResult.Rows[0]["POINT_RADIUS"] = pointRadius.ToString();
                }
                else
                {

                    for (int i = 0; i < dt.Rows.Count;)
                    {
                        if (!dt.Rows[i]["LONGITUDE_LATITUDE"].ToString().Contains(","))
                            continue;

                        dt.Rows[i]["POINT_RADIUS"] = pointRadius.ToString();
                        P5sLocation location1 = new P5sLocation(dt.Rows[i]["LONGITUDE_LATITUDE"].ToString());

                        int index = i + 1;
                        for (int j = index; j < dt.Rows.Count; j++)
                        {
                            if (!dt.Rows[j]["LONGITUDE_LATITUDE"].ToString().Contains(","))
                                continue;
                            P5sLocation location2 = new P5sLocation(dt.Rows[j]["LONGITUDE_LATITUDE"].ToString());
                            double tempDistance = P5sTrackingHelper.CalculateDistance(location1, location2) * 1000;

                            if (tempDistance <= pointRadius)
                            {

                                dt.Rows[i]["END_DATETIME"] = dt.Rows[j]["END_DATETIME"];

                                //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                                dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[j]["BATTERY_PERCENTAGE"];
                                dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"];

                                //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                                dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[j]["TRACKING_PROVIDER"].ToString();
                                dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"].ToString();


                                if (dt.Rows[j]["DEVICE_STATUS"].ToString() != "")
                                    if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                        dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[j]["DEVICE_STATUS"].ToString();
                                    else
                                        dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[j]["DEVICE_STATUS"].ToString();

                                dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[j]["NO_REPEAT"].ToString());

                                //gôm cập nhật lai địa chỉ
                                if (dt.Rows[j]["LOCATION_ADDRESS"].ToString().Length > 0)
                                    dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[j]["LOCATION_ADDRESS"].ToString();



                                index = j + 1;

                            }
                            else
                            {
                                //compare time two point
                                DateTime dtime1 = DateTime.Parse(dt.Rows[i]["BEGIN_DATETIME"].ToString());
                                DateTime dtime2 = DateTime.Parse(dt.Rows[j]["BEGIN_DATETIME"].ToString());
                                TimeSpan diffResult = dtime2.Subtract(dtime1);
                                if (diffResult.TotalMinutes <= pointTime)
                                {

                                    dt.Rows[i]["END_DATETIME"] = dt.Rows[j]["END_DATETIME"];

                                    //gôm BATTERY_PERCENTAGE thành 1 chổi duy nhất để xử lý sau
                                    dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] = dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"] + "," + dt.Rows[j]["BATTERY_PERCENTAGE"];
                                    dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] = dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"];

                                    //gôm TRACKING_PROVIDER thành 1 chổi duy nhất để xử lý sau
                                    dt.Rows[i]["TRACKING_PROVIDER_VALUE"] = dt.Rows[i]["TRACKING_PROVIDER_VALUE"] + "," + dt.Rows[j]["TRACKING_PROVIDER"].ToString();
                                    dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] = dt.Rows[i]["TRACKING_PROVIDER_DATETIME"] + "," + dt.Rows[j]["BEGIN_DATETIME"].ToString();


                                    if (dt.Rows[j]["DEVICE_STATUS"].ToString() != "")
                                        if (dt.Rows[i]["DEVICE_STATUS"].ToString() == "")
                                            dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[j]["DEVICE_STATUS"].ToString();
                                        else
                                            dt.Rows[i]["DEVICE_STATUS"] = dt.Rows[i]["DEVICE_STATUS"].ToString() + "-> " + dt.Rows[j]["DEVICE_STATUS"].ToString();

                                    dt.Rows[i]["NO_REPEAT"] = int.Parse(dt.Rows[i]["NO_REPEAT"].ToString()) + int.Parse(dt.Rows[j]["NO_REPEAT"].ToString());

                                    //gôm cập nhật lai địa chỉ
                                    if (dt.Rows[j]["LOCATION_ADDRESS"].ToString().Length > 0)
                                        dt.Rows[i]["LOCATION_ADDRESS"] = dt.Rows[j]["LOCATION_ADDRESS"].ToString();


                                    index = j + 1;

                                }
                                else
                                    break;
                            }
                        }
                        dtResult.Rows.Add(
                                           dt.Rows[i]["YYMMDD"],
                                           dt.Rows[i]["RSM_CD"],
                                           dt.Rows[i]["DISTRIBUTOR_CD"],
                                           dt.Rows[i]["LONGITUDE_LATITUDE"],
                                           dt.Rows[i]["DEVICE_STATUS"],
                                           dt.Rows[i]["NO_REPEAT"],
                                           dt.Rows[i]["BEGIN_DATETIME"],
                                           dt.Rows[i]["END_DATETIME"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE"],
                                           dt.Rows[i]["TYPE_TRACKING"],
                                           dt.Rows[i]["POINT_RADIUS"],
                                           dt.Rows[i]["TRACKING_ACCURACY"],
                                           dt.Rows[i]["ANGEL"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE_VALUE"],
                                           dt.Rows[i]["BATTERY_PERCENTAGE_DATETIME"],
                                           dt.Rows[i]["TRACKING_PROVIDER"],
                                           dt.Rows[i]["TRACKING_PROVIDER_VALUE"],
                                           dt.Rows[i]["TRACKING_PROVIDER_DATETIME"],
                                           dt.Rows[i]["LOCATION_ADDRESS"]
                                        );
                        i = index;
                    }

                }
            }
            else
                dtResult = dt;




            //verify start point & end point
            dtParam = L5sSql.Query("SELECT VALUE FROM S_PARAMS WHERE NAME = 'TRACKING_BREAK_TIME' AND ACTIVE = 1 ");
            if (dtParam != null && dtParam.Rows.Count == 1)
            {
                Double breakingTime = Double.Parse(dtParam.Rows[0]["VALUE"].ToString());
                for (int i = 0; i < dtResult.Rows.Count;)
                {

                    if (i == 0)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingStart;
                        i++;
                        continue;
                    }

                    if (i == dtResult.Rows.Count - 1)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingEnd;
                        i++;
                        continue;
                    }

                    DateTime dtime1 = DateTime.Parse(dtResult.Rows[i]["END_DATETIME"].ToString());
                    DateTime dtime2 = DateTime.Parse(dtResult.Rows[i + 1]["BEGIN_DATETIME"].ToString());

                    TimeSpan diffResult = dtime2.Subtract(dtime1);
                    if (diffResult.TotalMinutes >= breakingTime)
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingEnd;
                        dtResult.Rows[i + 1]["TYPE_TRACKING"] = P5sEnum.TrackingStart;
                        i = i + 2;
                        continue;
                    }
                    else
                    {
                        dtResult.Rows[i]["TYPE_TRACKING"] = P5sEnum.TrackingPoint;
                        i++;
                        continue;
                    }

                }
            }

            //Điểm đầu và điểm cuối trùng nhau
            if (dtResult.Rows.Count > 0 && dtResult.Rows[dtResult.Rows.Count - 1]["TYPE_TRACKING"].ToString() == P5sEnum.TrackingStart)
            {
                dtResult.Rows[dtResult.Rows.Count - 1]["TYPE_TRACKING"] = P5sEnum.TrackingStartEnd;
            }


            //Calculate Angle
            for (int i = 0; i < dtResult.Rows.Count - 1; i++)
            {
                P5sLocation location1 = new P5sLocation(dtResult.Rows[i]["LONGITUDE_LATITUDE"].ToString());
                P5sLocation location2 = new P5sLocation(dtResult.Rows[i + 1]["LONGITUDE_LATITUDE"].ToString());
                dtResult.Rows[i]["ANGEL"] = P5sCmmFns.CalculateAngle(location1, location2);
            }

            if (dtResult == null || dtResult.Rows.Count == 0)
                return;

            //add new column
            String sessionCd = System.Guid.NewGuid().ToString();
            DataColumn colSessionCd = new DataColumn("SESSION_CD");
            colSessionCd.DataType = typeof(String);
            colSessionCd.DefaultValue = sessionCd;
            dtResult.Columns.Add(colSessionCd);


            using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
            {
                connection.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                //I assume you have created the table previously 
                //Someone else here already showed how   
                bulkcopy.DestinationTableName = "TMP_DE_TRACKING_OF_RSM";

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("RSM_CD", "RSM_CD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YYMMDD", "YYMMDD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LONGITUDE_LATITUDE", "LONGITUDE_LATITUDE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DEVICE_STATUS", "DEVICE_STATUS"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NO_REPEAT", "NO_REPEAT"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BEGIN_DATETIME", "BEGIN_DATETIME"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("END_DATETIME", "END_DATETIME"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE", "BATTERY_PERCENTAGE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_TRACKING", "TYPE_TRACKING"));

                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("POINT_RADIUS", "POINT_RADIUS"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ANGEL", "ANGEL"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_ACCURACY", "TRACKING_ACCURACY"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE_VALUE", "BATTERY_PERCENTAGE_VALUE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BATTERY_PERCENTAGE_DATETIME", "BATTERY_PERCENTAGE_DATETIME"));


                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER", "TRACKING_PROVIDER"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER_VALUE", "TRACKING_PROVIDER_VALUE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TRACKING_PROVIDER_DATETIME", "TRACKING_PROVIDER_DATETIME"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LOCATION_ADDRESS", "LOCATION_ADDRESS"));

                try
                {

                    //DELETE DATA BEFORE INSERT
                    sql = String.Format(@"  
                                           DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{2} WHERE YYMMDD = {0} AND RSM_CD = {1}
                                           DELETE FROM [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{3} WHERE YYMMDD = {0} AND RSM_CD = {1} ",
                                           yymmdd, RSMCD, tableNameTrackingOfSales, tableNameTrackingOfStopOfSales);
                    L5sSql.Execute(sql);
                    bulkcopy.WriteToServer(dtResult);

                    sql = String.Format(@"  DECLARE @TRACKING_POINT_DURATION INT 
                                     SET @TRACKING_POINT_DURATION = 2
                                     SELECT  @TRACKING_POINT_DURATION = CAST( VALUE AS INT) FROM S_PARAMS WHERE NAME = 'TRACKING_POINT_DURATION' 
                            
                                        INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{3}
                                                       ([YYMMDD]
                                                       ,[RSM_CD]
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
                                                       ,[CREATED_DATE]  ,[LOCATION_ADDRESS])
                                             SELECT   
		                                              [YYMMDD]
		                                              ,[RSM_CD]
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
		                                              ,[CREATED_DATE]  ,[LOCATION_ADDRESS]
                                              FROM [dbo].[TMP_DE_TRACKING_OF_RSM]
                                              WHERE RSM_CD = {0} AND YYMMDD = {1} 
                                                    AND SESSION_CD = '{2}'
                            
                                       INSERT INTO  [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].{4} 
                                                       ([YYMMDD]
                                                       ,[RSM_CD]
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
                                                       ,[CREATED_DATE]  ,[LOCATION_ADDRESS])
                                             SELECT   
		                                              [YYMMDD]
		                                              ,[RSM_CD]
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
		                                              ,[CREATED_DATE]  ,[LOCATION_ADDRESS]
                                              FROM [dbo].[TMP_DE_TRACKING_OF_RSM]
                                              WHERE RSM_CD = {0} AND YYMMDD = {1} AND TYPE_TRACKING = 'P'
                                                    AND DATEDIFF(SECOND,BEGIN_DATETIME,  END_DATETIME ) >= @TRACKING_POINT_DURATION*60
                                                    AND SESSION_CD = '{2}'

                                            DELETE  
                                              FROM [dbo].[TMP_DE_TRACKING_OF_RSM]
                                              WHERE RSM_CD = {0} AND YYMMDD = {1} 
                                                    AND SESSION_CD = '{2}'   


                                    ", RSMCD, yymmdd, sessionCd, tableNameTrackingOfSales, tableNameTrackingOfStopOfSales);

                    //INSERT DATA DE_TRACKING_STOP_OF_SALES -DÙNG CHO CÁC BÁO CÁO STOP POINT
                    L5sSql.Execute(sql);

                }
                catch (Exception ex)
                {
                }
            }
        }

        internal static DataTable P5sGetTarget(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT DISTINCT TARGET_CD, TARGET_CODE+' - '+TARGET_NAME ,{0} ,ACTIVE 
                                                FROM O_TARGET ", p);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }

        internal static bool GenerateCrystalReports(DataTable dataTable, string RptFile)
        {
            try
            {

                if (dataTable.Rows.Count == 0 || dataTable.Rows.Count <= 0)
                    return false;
                L5sCReport.cryRpt.Load(RptFile);
                try
                {
                    L5sCReport.cryRpt.SetDataSource(dataTable);

                }
                catch (Exception ex)
                {

                    L5sMsg.ShowError(ex);
                }
                return true;
            }
            catch (Exception ex)
            {

                L5sMsg.ShowError(ex);
            }
            return false;
        }

        internal static String getIpAddress(Page page)
        {

            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if(string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            return ip;

        }

        public static string ConvertStr(string text)
        {
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            //text = text.Replace(" ", "-").Replace("--", "-").Replace("--", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');//.ToLower();
        }

        public static void GetMapGeoCodeProxy(DataTable tbGeoCode, DataTable tbproxy, int Timeout)
        {

            //lấy danh sách phường/xã/thị trấn từ hệ thống để chút nữa map tọa độ sau khi phân giải với phường xã thị trấn
            DataTable dtHierarchy = L5sSql.Query(@"SELECT cmm.COMMUNE_CD,
                                                    dist.DISTRICT_CD,
                                                    pro.PROVINCE_CD ,
                                                     replace(replace(replace(replace([dbo].GetUnsignString(cmm.COMMUNE_NAME_VI),' ',''),'-',''),'y','i'),'Y','I') AS COMMUNE_NAME_VI, 
                                                     replace(replace(replace(replace([dbo].GetUnsignString(dist.DISTRICT_NAME_VI),' ',''),'-',''),'y','i'),'Y','I') AS DISTRICT_NAME_VI, 
                                                     replace(replace(replace(replace([dbo].GetUnsignString(PROVINCE_NAME_VI),' ',''),'-',''),'y','i'),'Y','I') AS PROVINCE_NAME_VI  
                                                    FROM   M_COMMUNE cmm INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
                                                    INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD");


            for (int i = 0; i < dtHierarchy.Columns.Count; i++)
            {
                dtHierarchy.Columns[i].ReadOnly = false;
            }







            string GeoCode = "", proxy = "", address = "";
            int proxyIndex = 0;
            for (int i = 0; i < tbGeoCode.Rows.Count; i++)
            {
                address = "";
                try
                {

                    GeoCode = tbGeoCode.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                    if (!P5sCheckGeocodeValid(GeoCode)) //kiểm tra xe tọa độ có hợp lệ hay không
                    {
                        continue;
                    }

                    //duyệt danh sách proxy
                    for (int j = proxyIndex; j < tbproxy.Rows.Count; j++)
                    {
                        proxyIndex = j;
                        proxy = tbproxy.Rows[j]["PROXY_IP"].ToString();
                        try
                        {
                            //post tọa độ cần phân giải kèm với proxy IP
                            WebProxy proxyObject = new WebProxy(proxy, true);
                            string url = "http://maps.google.com/maps/api/geocode/xml?latlng=" + GeoCode + "&sensor=false";
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                            request.Proxy = proxyObject;
                            request.Timeout = Timeout;
                            request.Method = "POST";
                            string postData = "This is a test that posts this string to a Web server.";
                            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.ContentLength = byteArray.Length;
                            Stream dataStream = request.GetRequestStream();
                            dataStream.Write(byteArray, 0, byteArray.Length);
                            dataStream.Close();
                            System.Net.ServicePointManager.Expect100Continue = false;
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                            dataStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(dataStream);
                            string responseFromServer = reader.ReadToEnd();
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseFromServer);
                            string a = "-1";
                            XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("/GeocodeResponse");
                            foreach (XmlNode node in nodes)
                            {
                                a = node.SelectSingleNode("status").InnerText;
                            }

                            //kết quả trả về thất bại thì sẽ exit
                            if (a.Equals("ZERO_RESULTS"))
                            {
                                break;
                            }

                            //kết quả trả về Ok thì xem như phân giải thành công
                            //code bên dưới là lưu dữ liệu vào DB                           
                            if (a.Equals("OK"))
                            {
                                XmlNodeList nodes1 = xmlDoc.DocumentElement.SelectNodes("/GeocodeResponse/result");

                                if (nodes1.Item(0) != null)
                                {

                                    string[] list = nodes1.Item(0).SelectSingleNode("formatted_address").InnerText.ToString().Split(',');
                                    if (list.Length >= 5)
                                    {
                                        address = nodes1.Item(0).SelectSingleNode("formatted_address").InnerText;
                                    }
                                    else
                                    {
                                        if (nodes1.Item(1) != null)
                                        {
                                            list = nodes1.Item(1).SelectSingleNode("formatted_address").InnerText.ToString().Split(',');
                                            if (list.Length == 4)
                                            {
                                                address = nodes1.Item(1).SelectSingleNode("formatted_address").InnerText.ToString();
                                            }
                                            else
                                            {
                                                address = nodes1.Item(0).SelectSingleNode("formatted_address").InnerText;
                                            }
                                        }
                                        else
                                        {
                                            address = nodes1.Item(0).SelectSingleNode("formatted_address").InnerText;
                                        }
                                    }
                                    string LONGITUDE_LATITUDE = GeoCode, fullAddress = "", street = "", ward = "", district = "", province = "", country = "";
                                    string strSql = @"INSERT INTO O_MAPGEOCODE(LONGITUDE_LATITUDE,FULLADDRESS,STREET,WARD,DISTRICT,PROVINCE,COUNTRY,COMMUNE_CD,DISTRICT_CD,PROVINCE_CD)
                                            VALUES(@0,@1,@2,@3,@4,@5,@6,@7,@8,@9)";
                                    fullAddress = address;
                                    list = address.Split(','); //cắt danh sách kết quả trả về để xử lý
                                    if (list.Length == 5) // length = 5 thì sẽ lấy được đầy đủ thông tin về dường, phường, xã, thị trấn, thành phố v.v.
                                    {
                                        street = list[0].ToString();
                                        ward = list[1].ToString();
                                        district = list[2].ToString();
                                        province = list[3].ToString();
                                        country = list[4].ToString();
                                    }
                                    else
                                    {
                                        if (list.Length > 5) //nếu > 5 thì tọa độ này có nhiều thông tin về đường nên sẽ cần phải gôm các thông tin về đường lại
                                        {
                                            ward = list[list.Length - 4].ToString();
                                            district = list[list.Length - 3].ToString();
                                            province = list[list.Length - 2].ToString();
                                            country = list[list.Length - 1].ToString();
                                            for (int m = list.Length - 5; m >= 0; m--)
                                            {
                                                street = list[m].ToString() + ", " + street;
                                            }
                                            if (!string.IsNullOrEmpty(street))
                                            {
                                                street = street.Substring(0, street.Length - 2);
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                if (list[list.Length - 4] != null) //kết quả trả về có thông tin về phường
                                                {
                                                    ward = list[list.Length - 4].ToString();
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                ward = "";
                                            }

                                            try
                                            {
                                                if (list[list.Length - 3] != null) //kết quả trả về có thông tin về quận
                                                {
                                                    district = list[list.Length - 3].ToString();
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                district = "";
                                            }

                                            try
                                            {
                                                if (list[list.Length - 2] != null) //kết quả trả về có thông tin về tỉnh/thành phố
                                                {
                                                    province = list[list.Length - 2].ToString();
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                province = "";
                                            }

                                            try
                                            {
                                                if (list[list.Length - 1] != null) //kết quả trả về có thông tin quốc gia
                                                {
                                                    country = list[list.Length - 1].ToString();
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                country = "";
                                            }


                                        }
                                    }


                                    //chuẩn hóa dữ liêu lại, bản thân google lưu thông tin không được thống nhất cho nên cần chuẩn hóa trước để có thể so sánh với dữ liệu trong DB
                                    ward = ward.Replace("tp.", "").Replace("tt.", "").Replace("tx.", "").Replace("x.", "").Replace("h.", "").Replace("Q.", "").Trim();
                                    district = district.Replace("tp.", "").Replace("tt.", "").Replace("tx.", "").Replace("x.", "").Replace("h.", "").Replace("Q.", "").Trim();
                                    province = province.Replace("tp.", "").Replace("tt.", "").Replace("tx.", "").Replace("x.", "").Replace("h.", "").Replace("Q.", "").Trim();

                                    String provinceRemoveNumber = ReplaceStr(ConvertStr(province.Trim()));
                                    provinceRemoveNumber = Regex.Replace(provinceRemoveNumber, @"[\d-]", string.Empty);

                                    //với các thông tin từ phường/xã/thị trấn được trả về từ tọa độ có thể map với phường/xã/thị trấn trong DB hay không 
                                    DataRow[] results = dtHierarchy.Select(String.Format("COMMUNE_NAME_VI LIKE '%{0}%' AND DISTRICT_NAME_VI LIKE '%{1}%' AND PROVINCE_NAME_VI  LIKE '%{2}%' ", ReplaceStr(ConvertStr(ward.Trim())), ReplaceStr(ConvertStr(district.Trim())), provinceRemoveNumber));


                                    if (results.Length == 0)
                                    {
                                        L5sSql.Query(strSql, LONGITUDE_LATITUDE, fullAddress, street, ward, district, province, country, DBNull.Value, DBNull.Value, DBNull.Value);
                                    }
                                    else
                                    {
                                        L5sSql.Query(strSql, LONGITUDE_LATITUDE, fullAddress, street, ward, district, province, country, results[0].ItemArray[0], results[0].ItemArray[1], results[0].ItemArray[2]);
                                    }

                                    proxyIndex = j;
                                    break;
                                }
                            }
                            else
                                //nếu IP bị google khóa do vượt quá số lần post => cập nhật để loại bỏ proxy này và tiếp tục xử dụng proxy tiếp theo     
                                if (a.Equals("OVER_QUERY_LIMIT"))//ip has limit.
                            {

                                string SQL = "UPDATE S_PROXY SET ACTIVE=0 WHERE PROXY_CD=@0";
                                L5sSql.Execute(SQL, tbproxy.Rows[j]["PROXY_CD"].ToString());

                                proxyIndex = j++;
                                if (proxyIndex >= tbproxy.Rows.Count)
                                {
                                    return;
                                }
                                continue;
                            }
                        }
                        catch
                        {
                            string SQL = "UPDATE S_PROXY SET ACTIVE=0 WHERE PROXY_CD=@0";
                            L5sSql.Execute(SQL, tbproxy.Rows[j]["PROXY_CD"].ToString());

                            proxyIndex = j++;
                            if (proxyIndex >= tbproxy.Rows.Count)
                            {
                                return;
                            }
                            continue;
                        }

                        if (proxyIndex >= tbproxy.Rows.Count)
                        {
                            return;
                        }
                    }
                }
                catch (Exception)
                {

                }

            }
        }

        public static string ReplaceStr(string str)
        {
            str = str.Replace("tp.", "");
            str = str.Replace("tt.", "");
            str = str.Replace("tx.", "");
            str = str.Replace("x.", "");
            str = str.Replace(" ", "");
            str = str.Replace("-", "");
            str = str.Replace("y", "i");
            str = str.Replace("Y", "I");
            str = str.Replace("h.", "");
            str = str.Replace("Q.", "");
            str = str.Replace("Province", "");
            str = str.Replace("Viet Nam", "");
            return str;
        }

        internal static DataTable P5sGetSalesType(string p)
        {
            p = p == "" ? @"' '" : p;
            String sql = String.Format(@"SELECT SALES_TYPE_CD,SALES_TYPE_CODE + '-'+ SALES_TYPE_NAME, {0}, ACTIVE FROM  M_SALES_TYPE ORDER BY SALES_TYPE_CODE ", p);
            DataTable dt = L5sSql.Query(sql);
            return dt;
        }

        internal static bool GenerateReportTable(String sql, string RptFile)
        {
            try
            {
                DataTable dt = L5sSql.Query(sql);

                if (dt.Rows.Count == 0 || dt.Rows.Count <= 0)
                    return false;
                L5sCReport.cryRpt.Load(RptFile);
                try
                {
                    L5sCReport.cryRpt.SetDataSource(dt);

                }
                catch (Exception ex)
                {

                    L5sMsg.ShowError(ex);
                }
                return true;
            }
            catch (Exception ex)
            {

                L5sMsg.ShowError(ex);
            }
            return false;
        }

        internal static DataTable P5sGetTradeProgram(DateTime from, DateTime to, int type)
        {
            String sql = String.Format(@"SELECT TRADE_PROGRAM_CD,TRADE_PROGRAM_CODE + '-'+ TRADE_PROGRAM_NAME,'',1 
                                        FROM  O_TRADE_PROGRAM 
                                        WHERE  CONVERT(Date, START_DATE , 103) >= '{0}'
                                               AND CONVERT(Date, END_DATE , 103) <= '{1}' AND TYPE = {2}
                                        ORDER BY TRADE_PROGRAM_CODE , START_DATE DESC ",
                                       from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), type);
            DataTable dt = L5sSql.Query(sql);
            return dt;

        }

        internal static DataTable P5sGetTradeProgram(DateTime from, DateTime to)
        {
            String sql = String.Format(@"SELECT TRADE_PROGRAM_CD,TRADE_PROGRAM_CODE + '-'+ TRADE_PROGRAM_NAME,'',1 
                                        FROM  O_TRADE_PROGRAM 
                                        WHERE  CONVERT(Date, START_DATE , 103) >= '{0}'
                                               AND CONVERT(Date, END_DATE , 103) <= '{1}'
                                        ORDER BY TRADE_PROGRAM_CODE , START_DATE DESC ",
                                       from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"));
            DataTable dt = L5sSql.Query(sql);
            return dt;

        }

        internal static void P5sWriteHistory(String Feature, bool Status, String FileName)
        {
            try
            {
                String sql = String.Format(@"INSERT INTO H_FILE_UPLOAD (FILE_UPLOAD_NAME,FILE_UPLOAD_STATTUS,FEATURE,USER_UPLOAD) 
                                        VALUES(@0,@1,@2,@3)");
                L5sSql.Execute(sql, FileName, Status, Feature, HttpContext.Current.Session["F5sUsrCD"].ToString());
            }
            catch (Exception ex)
            {

            }
        }

        internal static void P5sWriteHistory(String Feature, bool Status, String FileName, string error)
        {
            try
            {
                String sql = String.Format(@"INSERT INTO H_FILE_UPLOAD (FILE_UPLOAD_NAME,FILE_UPLOAD_STATTUS,FEATURE,ERROR) 
                                        VALUES(@0,@1,@2,@3)");
                L5sSql.Execute(sql, FileName, Status, Feature, error);
            }
            catch (Exception ex)
            {

            }
        }

        internal static String StandardFileNameUpload(String fileName)
        {
            String Tmp = Regex.Replace(fileName, "[^a-zA-Z0-9_.]+", "_", RegexOptions.Compiled);
            Tmp = Regex.Replace(Tmp, @"\.(?=.*\.)", "");
            return Tmp.Replace(".", "_" + DateTime.Now.ToString("yyMMddHHmmssffff") + ".").Replace("__", "_");
        }

        internal static long P5sInsertFileUploadInfo(String Description, String FileName, bool Active)
        {
            long P5sResult = L5sSql.Execute("INSERT INTO O_FILE_UPLOAD_INFO (DESCRIPTION, FILE_UPLOAD_NAME, ACTIVE) VALUES(@0,@1,@2)", Description, FileName, Active);
            return P5sResult;
        }

        internal static long P5sUpdateFileUploadInfo(String FileUploadCD, String Description, bool Active)
        {
            long P5sResult = L5sSql.Execute("UPDATE O_FILE_UPLOAD_INFO SET DESCRIPTION = @1 ,ACTIVE = @2 WHERE FILE_UPLOAD_CD = @0", FileUploadCD, Description, Active);
            return P5sResult;
        }

        internal static bool P5sCheckFileExist(String P5sFileName, String Path)
        {
            //String P5sPath = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/FileUpload/"), P5sFileName);
            string P5sPath = Path + P5sFileName;
            if (File.Exists(P5sPath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static String P5sGetFileNameFromSalesCode(String SalesCode)
        {
            DataTable dt = L5sSql.Query(@"SELECT FILE_UPLOAD_NAME 
                                        FROM O_FILE_UPLOAD_INFO T1 INNER JOIN O_FILEUPLOAD_DISTRIBUTR T2 ON T1.FILE_UPLOAD_CD = T2.FILE_UPLOAD_CD
                                        INNER JOIN M_SALES T3 ON T2.DISTRIBUTOR_CD = T3.DISTRIBUTOR_CD
                                        WHERE T1.ACTIVE = 1 AND T2.ACTIVE =1 AND T3.SALES_CODE = @0", SalesCode);

            if (dt != null && dt.Rows.Count > 0)
            {
                return P5sDtToString(dt);
            }
            return "-1";
        }

        public static String P5sDtToString(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                String str = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str += "" + dt.Rows[i][0].ToString() + "≡";
                }
                return str.Remove(str.LastIndexOf("≡"));
            }
            else
            {
                return "-1";
            }
        }

        public static DataTable SqlDatatableTimeout(string selectsql, int timeout)
        {
            SqlConnection Conn = (SqlConnection)null;
            string connectionString = ConfigurationSettings.AppSettings["ConStr"];
            if (ConfigurationSettings.AppSettings["sa"] == "1" && HttpContext.Current.Session["L5sSAU"] != null)
                connectionString = connectionString.Substring(0, connectionString.ToLower().IndexOf("user id")) +
                                   (object)"User ID=fs" + (string)HttpContext.Current.Session["L5sSAU"] + "; " +
                                   connectionString.Substring(connectionString.ToLower().IndexOf("password"));
            Conn = new SqlConnection(connectionString);
            DataSet dataset = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = new SqlCommand(selectsql, Conn);
            adapter.SelectCommand.CommandTimeout = timeout;
            Conn.Open();
            adapter.Fill(dataset);
            DataTable table = dataset.Tables[0];
            if (Conn != null)
                Conn.Close();
            return table;
        }

        public static void P5sWriteFile(DataTable dt, String Path)
        {
            StreamWriter swExtLogFile = new StreamWriter(Path, true);
            int i;
            foreach (DataRow row in dt.Rows)
            {
                object[] array = row.ItemArray;
                for (i = 0; i < array.Length - 1; i++)
                {
                    swExtLogFile.Write(array[i].ToString() + "\t");
                }
                swExtLogFile.WriteLine(array[i].ToString());
            }
            swExtLogFile.Flush();
            swExtLogFile.Close();
        }


        public static void P5sASPExportFileToClient(String resultPath, String FileName)
        {
            HttpContext.Current.Response.ContentType = "application/excel";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
            HttpContext.Current.Response.WriteFile(resultPath);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static string Passcode_Random(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                if (temp != -1 && temp == t)
                {
                    return Passcode_Random(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

    }
}
