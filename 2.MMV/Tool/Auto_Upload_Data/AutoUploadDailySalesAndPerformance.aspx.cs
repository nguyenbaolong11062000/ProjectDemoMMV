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
using System.IO;
using System.Data.SqlClient;
using P5sDmComm;
using P5sCmm;
using MMV.Report;

namespace MMV.Tool.Auto_Upload_Data
{
    public partial class AutoUploadDailySalesAndPerformance : System.Web.UI.Page
    {
        UploadFile upload = new UploadFile();

        #region Khai báo dailysales
        string dailysales = "";
        string linksave = "";
        string linkMoveto = "";
        string TBTemp = "TMP_IMPORT_SALES_AMOUNT";
        int namelocation = 12;
        string[] arr = { "DISTRIBUTOR_CODE", "DMS_SALES_CODE", "IDMS_CUST_CODE", "DAY", "MONTH", "YEAR", "TOTAL_SALES", "SALES_TP", "SALES_TB", "NUMBER_OF_ORDER", "SALES_CODE", "CUSTOMER_CODE", "IMPORT_SALES_AMOUNT_CD" };
        string cosCD = "IMPORT_SALES_AMOUNT_CD";

        #endregion 

        #region Khai báo dailyPerformance
        string dailyPerformance = "";
        string linksave1 = "";
        string linkMoveto1 = "";
        string TBTemp1 = "TMP_IMPORT_DSR_PERPORMANCE";
        int namelocation1 = 15;
        string[] arr1 = { "REGION_CD", "AREA_CD", "PROVINCE_CD", "STOCKIST_CODE", "STOCKIST_NAME", "SAP_SALES_MAN_CODE", "DMS_SALES_MAN_CODE", "SALES_MAN_NAME", "CDS", "AM", "RM", "DMS_DATE", "DELAY_DATE", "C_HEADER", "C_VALUES", "IMPORT_DSR_PERPORMANCE_CD" };

        string cosCD1 = "IMPORT_DSR_PERPORMANCE_CD";
        #endregion

        #region Khai báo CustomerList
        string dailyCustomerList = "";
        string linksave2 = "";
        string linkMoveto2 = "";
        string TBTemp2 = "TMP_IMPORT_CUSTOMER";
        int namelocation2 = 12;
        string[] arr2 = { "DISTRIBUTOR_CODE", "CUSTOMER_CODE", "IDMS_CUSTOMER_CODE", "CUSTOMER_NAME", "CUSTOMER_ADDRESS", "GLOBAL_RE", "CUSTOMER_CHAIN_CODE", "SALES_CODE", "IDMS_SALES_CODE", "SALES_NAME", "ROUTE_CODE", "ROUTE_DESC", "IMPORT_CUSTOMER_CD" };

        string cosCD2 = "IMPORT_CUSTOMER_CD";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.LoadForLoginPage();
            UploadCustomer();
            UploadDaiLySales();
            UploadPerformance();
            this.Dashboard_MTD_Primary();//consolidate MTD PRIMARY
        }

        #region Upload Daily sales
        public void UploadDaiLySales()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_FILE],[PATH_BACKUP_FILE] FROM [S_PATH_UPLOAD] where PATH_CODE = 'DAILYSALES'");
            linksave = dt.Rows[0][0].ToString();
            linkMoveto = dt.Rows[0][1].ToString();
            
            //Upload daily sales
            DataTable dtdailysales = L5sSql.Query("SELECT PATH_NAME FROM[S_PATH_UPLOAD] where PATH_CODE = 'DAILYSALES'");
            dailysales = dtdailysales.Rows[0][0].ToString();
            string filenamedailysales = dailysales + ".txt";
            string filepathdailysales = dt.Rows[0][0].ToString() + filenamedailysales;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailysales);
            string extension = Path.GetExtension(filenamedailysales);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }

            string fl = upload.ReadTXTFromFolder("AutoUpload", filenamedailysales, filepathdailysales, TBTemp, namelocation, arr, "", cosCD, out tmpViewState, namelocation, "0");

            if (!string.IsNullOrEmpty(tmpViewState))
            {
                if (fl == "1")
                {
                    P5sCmm.P5sCmmFns.P5sInsertSalesAmount(tmpViewState);
                    //Consolidate data for map MMV - 190114 DTP
                    P5sCmm.P5sCmmFns.P5sUpdateValueForMap();
                    upload.RollbackFinish(TBTemp);
                    P5sCmmFns.P5sWriteHistory("ImportSalesAmount", true, filenamedailysales, "Upload successful");
                    upload.Move_File(linksave, linkMoveto, filenamedailysales);
                    long check = L5sSql.Execute(@"exec [CONSOLIDATE_DASHBOARD_MTD]");
                    if(check > 0)
                    {
                        P5sCmmFns.P5sWriteHistory("CONSOLIDATE_DASHBOARD_MTD", true, "CONSOLIDATE_DASHBOARD_MTD", "Upload successful");
                    }
                    else
                    {
                        P5sCmmFns.P5sWriteHistory("CONSOLIDATE_DASHBOARD_MTD", false, "CONSOLIDATE_DASHBOARD_MTD", "Upload fail!");

                    }
                }
            }
        }
        #endregion

        #region Upload Performance
        public void UploadPerformance()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_FILE],PATH_BACKUP_FILE FROM [S_PATH_UPLOAD] where PATH_CODE = 'PERFORMANCE'");
            linksave1 = dt.Rows[0][0].ToString();
            linkMoveto1 = dt.Rows[0][1].ToString();


            //Upload daily Performance
            DataTable dtdailyPerformance = L5sSql.Query(@"select [PATH_NAME] FROM [S_PATH_UPLOAD] where PATH_CODE = 'PERFORMANCE'");
            dailyPerformance = dtdailyPerformance.Rows[0][0].ToString();
            string filenamedailyPerformance = dailyPerformance + ".txt";
            string filepathdailyPerformance = dt.Rows[0][0].ToString() + filenamedailyPerformance;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailyPerformance);
            string extension = Path.GetExtension(filenamedailyPerformance);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }

            string fl = upload.ReadExcelFromFolderPerformance("AutoUpload", filenamedailyPerformance, filepathdailyPerformance, TBTemp1, namelocation1, arr1, "", cosCD1, out tmpViewState, "3");

            if (!string.IsNullOrEmpty(tmpViewState))
            {
                if (fl == "1")
                {
                    UploadDataReport.P5sInsertDSRPerformance(tmpViewState);
                    upload.RollbackFinish(TBTemp);
                    P5sCmmFns.P5sWriteHistory("ImportDailyPerformance", true, filenamedailyPerformance, "Upload successful");
                    upload.Move_File(linksave1, linkMoveto1, filenamedailyPerformance);

                }
            }
        }
        #endregion

        #region Upload customer
        public void UploadCustomer()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_FILE],PATH_BACKUP_FILE FROM [S_PATH_UPLOAD] where PATH_CODE = 'CUSTOMERLIST'");
            linksave2 = dt.Rows[0][0].ToString();
            linkMoveto2 = dt.Rows[0][1].ToString();


            //Upload daily CustomerList
            DataTable dtdailyCustomerList = L5sSql.Query(@"select [PATH_NAME] FROM [S_PATH_UPLOAD] where PATH_CODE = 'CUSTOMERLIST'");
            dailyCustomerList = dtdailyCustomerList.Rows[0][0].ToString();
            string filenamedailyCustomerList = dailyCustomerList + ".txt";
            string filepathdailyCustomerList = dt.Rows[0][0].ToString() + filenamedailyCustomerList;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailyCustomerList);
            string extension = Path.GetExtension(filepathdailyCustomerList);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }

            string fl = upload.ReadTXTFromFolder("AutoUpload", filenamedailyCustomerList, filepathdailyCustomerList, TBTemp2, namelocation2, arr2, "", cosCD2, out tmpViewState, namelocation2, "0");

            try
            {
                String result = "";
                if (!this.P5sCheckDistributor(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName,(string.Format("Distributor code not exists: {0}", result)));
                    this.RollbackFinish(tmpViewState);
                    return;
                }


                if (!this.P5sCheckCommune(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("Commune code not exists: {0}", result)));
                    this.RollbackFinish(tmpViewState);
                    return;
                }


                if (!this.P5sCheckRoute(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("Route code existed in orther distributor: {0}", result)));
                    this.RollbackFinish(tmpViewState);
                    return;
                }


                if (!this.P5sCheckCustomer(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("Customer existed in orther distributor : {0}", result)));
                    this.RollbackFinish(tmpViewState);
                    return;
                }

                if (!this.P5sCheckCustomerDuplicate(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("List of customer were duplicate: {0}", result)));
                    this.RollbackFinish(tmpViewState);
                    return;
                }

                if (!this.P5sCheckSales(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("DSR existed in orther distributor: {0}", result)));
                    this.RollbackFinish(tmpViewState);
                    return;
                }


                if (!this.P5sCheckRouteMultiSales(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("Route managed by many DSR: {0}", result)));
                    this.RollbackFinish(tmpViewState);

                    return;
                }

                if (!this.P5sCheckSalesMultiDistributor(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("DSR managed by many distributor: {0}", result)));
                    this.RollbackFinish(tmpViewState);
                    return;
                }
                //update data for customer exists DB

                #region data sử lý
                String sql = String.Format(@"

                                        UPDATE TMP_IMPORT_CUSTOMER SET LONGITUDE_LATITUDE = NULL
                                        WHERE IMPORT_CUSTOMER_CD = '{0}' AND LEN(LONGITUDE_LATITUDE) <= 10

                                        --UPDATE GHI NHẬN NVBH MỚI
                                        UPDATE TMP_IMPORT_CUSTOMER SET IS_NEW_SALES = 1
                                        WHERE SALES_CD IS NULL  AND IMPORT_CUSTOMER_CD = '{0}'

                                        INSERT INTO [dbo].[M_SALES]
                                                   ([SALES_CODE]
                                                   ,[SALES_NAME]
                                                   ,[DISTRIBUTOR_CD]
                                                   ,[IDMS_SALES_CODE]
                                                  )
                                        SELECT DISTINCT SALES_CODE,SALES_NAME,DISTRIBUTOR_CD,IDMS_SALES_CODE FROM TMP_IMPORT_CUSTOMER
                                        WHERE SALES_CD IS NULL  AND IMPORT_CUSTOMER_CD = '{0}'



                                        --UPDATE GHI NHẬN TUYẾN BH MỚI
                                        UPDATE TMP_IMPORT_CUSTOMER SET IS_NEW_ROUTE = 1
                                        WHERE ROUTE_CD IS NULL  AND IMPORT_CUSTOMER_CD = '{0}'

                                        INSERT INTO [dbo].[M_ROUTE]
                                                   ([ROUTE_CODE]
                                                   ,[ROUTE_NAME]
                                                   ,[DISTRIBUTOR_CD]
                                                    ,[IDMS_ROUTE_CODE]
        )
                                        SELECT DISTINCT ROUTE_CODE,ROUTE_DESC,DISTRIBUTOR_CD,IDMS_ROUTE_CODE FROM TMP_IMPORT_CUSTOMER
                                        WHERE ROUTE_CD IS NULL  AND IMPORT_CUSTOMER_CD = '{0}'


                                        --UPDATE GHI NHẬN KHÁCH HÀNG MỚI
                                        UPDATE TMP_IMPORT_CUSTOMER SET IS_NEW_CUSTOMER = 1
                                        WHERE CUSTOMER_CD IS NULL AND IMPORT_CUSTOMER_CD = '{0}'


                                        INSERT INTO [dbo].[M_CUSTOMER]
        			                                 ( [CUSTOMER_CODE]
        			                                  ,[CUSTOMER_NAME]
        			                                  ,[CUSTOMER_ADDRESS]
        			                                  ,[CUSTOMER_CHAIN_CODE]   
        			                                  ,[DISTRIBUTOR_CD] 
        			                                  ,[COMMUNE_CD] 
                                                      ,[LONGITUDE_LATITUDE] 
                                                      ,[CUSTOMER_DISPLAY] 
                                                      ,[IDMS_CUSTOMER_CODE]
                                                      ,[GLOBAL_RE]
        			                                  ) 
                                        SELECT DISTINCT CUSTOMER_CODE,CUSTOMER_NAME,CUSTOMER_ADDRESS,CUSTOMER_CHAIN_CODE,
        				                                DISTRIBUTOR_CD,COMMUNE_CD,LONGITUDE_LATITUDE,CUSTOMER_DISPLAY,IDMS_CUSTOMER_CODE,GLOBAL_RE
                                        FROM TMP_IMPORT_CUSTOMER
                                        WHERE CUSTOMER_CD IS NULL AND IMPORT_CUSTOMER_CD = '{0}'

                                        --UPDATE VALUE ROUTE,SALES

                                                --ROUTE OF DISTRIBUTOR    
                                                 UPDATE TMP_IMPORT_CUSTOMER SET ROUTE_CD = rout.ROUTE_CD
                                                 FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_ROUTE rout ON tmp.ROUTE_CODE = rout.ROUTE_CODE
                                                 WHERE IMPORT_CUSTOMER_CD = '{0}'  AND rout.DISTRIBUTOR_CD = tmp.DISTRIBUTOR_CD AND tmp.ROUTE_CD IS NULL


                                                 --CUSTOMER OF DISTRIBUTOR    
                                                 UPDATE TMP_IMPORT_CUSTOMER SET CUSTOMER_CD = cust.CUSTOMER_CD
                                                 FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_CUSTOMER cust ON tmp.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                                 WHERE IMPORT_CUSTOMER_CD = '{0}'  AND cust.DISTRIBUTOR_CD = tmp.DISTRIBUTOR_CD AND tmp.CUSTOMER_CD IS NULL

                                                 --SALES OF DISTRIBUTOR    
                                                 UPDATE TMP_IMPORT_CUSTOMER SET SALES_CD = sls.SALES_CD
                                                 FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_SALES sls ON tmp.SALES_CODE = sls.SALES_CODE
                                                 WHERE IMPORT_CUSTOMER_CD = '{0}'  AND sls.DISTRIBUTOR_CD = tmp.DISTRIBUTOR_CD AND tmp.SALES_CD IS NULL



                                     --update thông tin NVBH
                                      UPDATE sls SET 
                                        SALES_NAME = CASE 
        			                                        WHEN tmp.SALES_NAME != '' THEN tmp.SALES_NAME 
        			                                        ELSE sls.SALES_NAME   
                                                       END, 
                                        ACTIVE = 1,
                                        DISTRIBUTOR_CD = tmp.DISTRIBUTOR_CD,
                                        IDMS_SALES_CODE = tmp.IDMS_SALES_CODE

                                        FROM M_SALES sls INNER JOIN 
                                        ( SELECT DISTINCT SALES_CD,SALES_CODE,SALES_NAME,DISTRIBUTOR_CD,IDMS_SALES_CODE FROM TMP_IMPORT_CUSTOMER WHERE DISTRIBUTOR_CD IS NOT NULL )  tmp ON  sls.SALES_CD = tmp.SALES_CD


                                     --update thông tin Tuyến BH
        			                UPDATE rout SET 
                                    ROUTE_NAME = CASE 
        				                                        WHEN tmp.ROUTE_DESC != '' THEN tmp.ROUTE_DESC 
        				                                        ELSE ROUTE_NAME   
                                                           END, 
                                    ACTIVE = 1,
                                    DISTRIBUTOR_CD = tmp.DISTRIBUTOR_CD,
                                    IDMS_ROUTE_CODE = tmp.IDMS_ROUTE_CODE
                                    FROM M_ROUTE rout INNER JOIN 
        				                 ( SELECT DISTINCT ROUTE_CD,ROUTE_CODE,ROUTE_DESC,DISTRIBUTOR_CD,IDMS_ROUTE_CODE FROM TMP_IMPORT_CUSTOMER WHERE DISTRIBUTOR_CD IS NOT NULL  )  tmp ON  rout.ROUTE_CD = tmp.ROUTE_CD


                                      --Cập nhật lại dữ liệu về NPP của CUSTOMER, province, district, Active lại KH
                                        UPDATE cust SET
                                        --cust.ACTIVE = tmp.ACTIVE, 

                                        cust.ACTIVE = CASE 
        				                                        WHEN tmp.ACTIVE IS NULL THEN cust.ACTIVE 
        				                                        ELSE tmp.ACTIVE   
                                                           END, 
                                        cust.COMMUNE_CD =
                                                           CASE 
                                                                WHEN ISNULL(tmp.COMMUNE_CD,0) = 0 THEN cust.COMMUNE_CD
                                                                ELSE
                                                                     tmp.COMMUNE_CD
                                                           END,
                                        cust.PROVINCE_CD = CASE 
                                                                WHEN ISNULL(district.PROVINCE_CD,0) = 0 THEN cust.PROVINCE_CD
                                                                ELSE
                                                                     district.PROVINCE_CD
                                                           END, 

                                        cust.DISTRICT_CD = CASE 
                                                                WHEN ISNULL(district.DISTRICT_CD,0) = 0 THEN cust.DISTRICT_CD 
                                                                ELSE
                                                                     district.DISTRICT_CD
                                                           END,

                                        cust.CUSTOMER_NAME = CASE 
        				                                        WHEN tmp.CUSTOMER_NAME != '' THEN tmp.CUSTOMER_NAME 
        				                                        ELSE cust.CUSTOMER_NAME   
                                                           END, 
                                        cust.CUSTOMER_ADDRESS = CASE 
        				                                        WHEN tmp.CUSTOMER_ADDRESS != '' THEN tmp.CUSTOMER_ADDRESS 
        				                                        ELSE cust.CUSTOMER_ADDRESS   
                                                           END, 
                                        cust.CUSTOMER_CHAIN_CODE = CASE 
        				                                        WHEN tmp.CUSTOMER_CHAIN_CODE != '' THEN tmp.CUSTOMER_CHAIN_CODE 
        				                                        ELSE cust.CUSTOMER_CHAIN_CODE   
                                                           END, 
                                        cust.DISTRIBUTOR_CD = CASE 
        				                                        WHEN tmp.DISTRIBUTOR_CD != '' THEN tmp.DISTRIBUTOR_CD 
        				                                        ELSE cust.DISTRIBUTOR_CD   
                                                           END, 
                                        cust.LONGITUDE_LATITUDE   = 
        		                                CASE 
        				                                WHEN tmp.LONGITUDE_LATITUDE != '' THEN tmp.LONGITUDE_LATITUDE 
        				                                ELSE cust.LONGITUDE_LATITUDE   
        		                                END,
                                        cust.CUSTOMER_DISPLAY = CASE 
        				                                        WHEN tmp.CUSTOMER_DISPLAY IS NULL THEN cust.CUSTOMER_DISPLAY 
        				                                        ELSE tmp.CUSTOMER_DISPLAY   
                                                           END,

                                       cust.IDMS_CUSTOMER_CODE = CASE
                                                                  WHEN tmp.IDMS_CUSTOMER_CODE != '' THEN tmp.IDMS_CUSTOMER_CODE
                                                                  ELSE cust.IDMS_CUSTOMER_CODE
        													END,
                                        cust.GLOBAL_RE = CASE
                                                                  WHEN tmp.GLOBAL_RE != '' THEN tmp.GLOBAL_RE
                                                                  ELSE cust.GLOBAL_RE
        													END

                                        FROM  M_CUSTOMER cust   INNER JOIN TMP_IMPORT_CUSTOMER tmp ON tmp.CUSTOMER_CD = cust.CUSTOMER_CD
        						                                LEFT JOIN M_COMMUNE cmm ON cust.COMMUNE_CD = cust.COMMUNE_CD
        						                                LEFT JOIN M_DISTRICT district ON cmm.DISTRICT_CD = district.DISTRICT_CD
                                        WHERE IMPORT_CUSTOMER_CD = '{0}'


                                        UPDATE M_CUSTOMER SET PROVINCE_CD = dist.PROVINCE_CD , DISTRICT_CD = cmm.DISTRICT_CD
                                        FROM M_CUSTOMER cust INNER JOIN M_COMMUNE cmm ON cust.COMMUNE_CD = cmm.COMMUNE_CD
                                             INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD

                                        UPDATE M_CUSTOMER SET PROVINCE_CD = NULL, DISTRICT_CD = NULL
                                        WHERE COMMUNE_CD IS NULL


                                        --deactive O_CUSTOMER_ROUTE
                                        UPDATE O_CUSTOMER_ROUTE SET ACTIVE = 0 , DEACTIVE_DATE = GETDATE()
                                        WHERE CUSTOMER_CD IN (SELECT CUSTOMER_CD FROM TMP_IMPORT_CUSTOMER  WHERE IMPORT_CUSTOMER_CD = '{0}' AND SALES_CD != '' AND ROUTE_CD != '')

                                        --INSERT O_CUSTOMER_ROUTE
                                        INSERT INTO [dbo].[O_CUSTOMER_ROUTE]
                                                           ([ROUTE_CD]
                                                           ,[CUSTOMER_CD])            
                                        SELECT DISTINCT ROUTE_CD,CUSTOMER_CD
                                        FROM TMP_IMPORT_CUSTOMER  
                                        WHERE IMPORT_CUSTOMER_CD = '{0}' AND SALES_CD != '' AND ROUTE_CD != ''



                                        -- DEACTIVE O_SALES_ROUTE
                                        UPDATE O_SALES_ROUTE SET ACTIVE = 0 , DEACTIVE_DATE = GETDATE()
                                        WHERE ROUTE_CD IN  (
                                                             SELECT DISTINCT tmp.ROUTE_CD 
                                                             FROM TMP_IMPORT_CUSTOMER   tmp
                                                             WHERE IMPORT_CUSTOMER_CD = '{0}' AND tmp.SALES_CD != '' AND tmp.ROUTE_CD != ''
                                                            ) 


                                        -- insert sales route



        	                            INSERT INTO [dbo].[O_SALES_ROUTE]
                                               ([SALES_CD]
                                               ,[ROUTE_CD])
                                        SELECT T.SALES_CD,T.ROUTE_CD FROM
                                        (
                                             SELECT DISTINCT tmp.SALES_CD,tmp.ROUTE_CD 
                                             FROM TMP_IMPORT_CUSTOMER   tmp
                                             WHERE IMPORT_CUSTOMER_CD = '{0}' AND tmp.SALES_CD != '' AND tmp.ROUTE_CD != ''
                                         ) AS T
                                         WHERE NOT EXISTS (SELECT * FROM O_SALES_ROUTE slsR 
                                                        WHERE slsR.SALES_CD = T.SALES_CD AND
                                                              slsR.ROUTE_CD = T.ROUTE_CD AND slsR.ACTIVE = 1)


                                        --update distributor
                                        UPDATE cust SET DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
                                        FROM M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
        	                                 INNER JOIN O_SALES_ROUTE slsR ON custR.ROUTE_CD = slsR.ROUTE_CD AND slsR.ACTIVE = 1
        	                                 INNER JOIN M_SALES sls ON slsR.SALES_CD = sls.SALES_CD


                                        --remove duplicate customer
                                        DELETE FROM  M_CUSTOMER
                                        WHERE CUSTOMER_CD NOT IN 
                                        (   SELECT MIN(CUSTOMER_CD) 
        	                                FROM  M_CUSTOMER   
        	                                GROUP BY CUSTOMER_CD
                                        )
                                        SELECT 1


                                    ", tmpViewState);

                #endregion

                try
                {
                    if (!string.IsNullOrEmpty(tmpViewState))
                    {
                        if (fl == "1")
                        {
                            P5sCmm.P5sCmmFns.SqlDatatableTimeout(sql, 36000);
                            Inactive_Customer();//hàm đang sử dụng câu store ../ScriptSQL/Scipt_Store_Inactive_customer.sql version: 2208, ../ScriptSQL/Scipt_inactive_sales.sql,../ScriptSQL/Script_inactive_Route.sql version 2211
                            P5sCmm.P5sCmmFns.P5sUpdateValueForMap();
                            P5sCmmFns.P5sWriteHistory("ImportCustomer", true, fileName, "Upload successful");
                            upload.Move_File(linksave2, linkMoveto2, filenamedailyCustomerList);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.RollbackError(tmpViewState);

                }
                finally
                {
                    this.RollbackFinish(tmpViewState);

                }
            }
            catch
            {
                this.RollbackError(tmpViewState);
            }


        }

        private void RollbackError(String tempViewState)
        {
            L5sSql.Execute(@"
                                        --REMOVE ROUTE ADD NEW
                                        DELETE M_ROUTE WHERE ROUTE_CD IN (SELECT ROUTE_CD FROM TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0 AND  IS_NEW_ROUTE = 1)

                                      --delete o_customer_route
                                        DELETE O_CUSTOMER_ROUTE
                                        WHERE ROUTE_CD IN (SELECT ROUTE_CD FROM TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0 AND  IS_NEW_ROUTE = 1)




                                        --REMOVE NVBH ADD NEW
                                        DELETE M_SALES WHERE SALES_CD IN (SELECT SALES_CD FROM TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0 AND  IS_NEW_SALES = 1)


                                          --delete O_SALES_ROUTE
                                        DELETE O_SALES_ROUTE
                                        WHERE SALES_CD IN (SELECT SALES_CD FROM TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0 AND  IS_NEW_SALES = 1)




                                      --INACTIVE CUSTOMER
                                        UPDATE M_CUSTOMER SET ACTIVE = 0
                                        WHERE EXISTS (SELECT SALES_CD FROM TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0 AND  CUSTOMER_ACTIVE = 0 AND M_CUSTOMER.CUSTOMER_CD = TMP_IMPORT_CUSTOMER.CUSTOMER_CD )                             



                                        DELETE TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0 OR CREATED_DATE < GETDATE() - 1 

                                    ", tempViewState);

        }

        private void RollbackFinish(String tempViewState)
        {
            L5sSql.Execute(@"
                                        DELETE TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0 OR CREATED_DATE < GETDATE() - 1 

                                    ", tempViewState);

        }

        #region check dupl
        private bool P5sCheckCustomerDuplicate(string tmpViewState, out string result)
        {
            result = "";
            String sql = @"
                                   SELECT CUSTOMER_CODE 
                                   FROM  TMP_IMPORT_CUSTOMER
                                   WHERE IMPORT_CUSTOMER_CD = @0                             
                                   GROUP BY CUSTOMER_CODE
                                   HAVING COUNT(*) >= 2  
                                ";


            DataTable dt = L5sSql.Query(sql, tmpViewState);
            if (dt.Rows.Count <= 0)
            {
                return true;
            }
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "CUSTOMER_CODE");
                return false;
            }


        }
        private Boolean P5sCheckRoute(String tmpViewState, out String result)
        {
            result = "";

            //Cập nhật ROUTE_CD từ ROUTE_CODE
            String sql = @"
                                 --ROUTE OF DISTRIBUTOR    
                                UPDATE tmp SET ROUTE_CD = rout.ROUTE_CD
                                FROM TMP_IMPORT_CUSTOMER tmp 
                                INNER JOIN (
        				                        SELECT dist.DISTRIBUTOR_CD,dist.DISTRIBUTOR_CODE,rout.ROUTE_CD,rout.ROUTE_CODE,rout.ACTIVE
        				                        FROM [M_DISTRIBUTOR.] dist INNER JOIN M_SALES sls ON dist.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
        							                          INNER JOIN O_SALES_ROUTE slsR ON slsR.SALES_CD = sls.SALES_CD AND slsr.ACTIVE = 1
        							                          INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD

        			                         ) rout ON tmp.ROUTE_CODE = rout.ROUTE_CODE AND   rout.DISTRIBUTOR_CD = tmp.DISTRIBUTOR_CD
                                WHERE IMPORT_CUSTOMER_CD = '{0}'  

                                 --ROUTE INACTIVE   
                                 UPDATE TMP_IMPORT_CUSTOMER SET ROUTE_CD = rout.ROUTE_CD
                                 FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_ROUTE rout ON tmp.ROUTE_CD IS NULL AND tmp.ROUTE_CODE = rout.ROUTE_CODE
                                 WHERE IMPORT_CUSTOMER_CD = '{0}'  --AND rout.ACTIVE  = 0      


                                ";
            // L5sSql.Query(sql, tmpViewState);

            sql = string.Format(sql, tmpViewState);
            try
            {
                L5sSql.Execute(sql);

            }
            catch (Exception e)
            {
                string erore = e.Message;
                string erore2 = e.ToString();

            }
            sql = @"SELECT DISTINCT ROUTE_CODE AS ROUTE_CODE FROM TMP_IMPORT_CUSTOMER tmp
                            WHERE IMPORT_CUSTOMER_CD = @0 AND ROUTE_CD IS NULL 
                             AND EXISTS (SELECT * FROM M_ROUTE rout WHERE tmp.ROUTE_CODE = rout.ROUTE_CODE  )
                            ";

            DataTable dt = L5sSql.Query(sql, tmpViewState);

            if (dt.Rows.Count <= 0)
                return true;
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "ROUTE_CODE");
                return false;
            }


        }
        private Boolean P5sCheckCustomer(String tmpViewState, out String result)
        {
            result = "";

            String sql = @"
                                 --CUSTOMER OF DISTRIBUTOR    
                                   UPDATE TMP_IMPORT_CUSTOMER SET ROUTE_CD = rout.ROUTE_CD
                                    FROM TMP_IMPORT_CUSTOMER tmp 
                                    INNER JOIN (
        				                            SELECT dist.DISTRIBUTOR_CD,dist.DISTRIBUTOR_CODE,rout.ROUTE_CD,rout.ROUTE_CODE,rout.ACTIVE
        				                            FROM [M_DISTRIBUTOR.] dist INNER JOIN M_SALES sls ON dist.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
        							                                INNER JOIN O_SALES_ROUTE slsR ON slsR.SALES_CD = sls.SALES_CD AND slsr.ACTIVE = 1
        							                                INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD			
        			                                ) rout ON tmp.ROUTE_CODE = rout.ROUTE_CODE AND tmp.DISTRIBUTOR_CD = rout.DISTRIBUTOR_CD
                                    WHERE IMPORT_CUSTOMER_CD = '{0}'  

                                 --CUSTOMER INACTIVE   
                                 UPDATE TMP_IMPORT_CUSTOMER SET CUSTOMER_CD = cust.CUSTOMER_CD
                                 FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_CUSTOMER cust ON tmp.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                 WHERE IMPORT_CUSTOMER_CD = '{0}'  --AND cust.ACTIVE  = 0      


                                ";
            // L5sSql.Query(sql, tmpViewState);
            sql = string.Format(sql, tmpViewState);
            L5sSql.Execute(sql);

            sql = @"SELECT DISTINCT CUSTOMER_CODE AS CUSTOMER_CODE FROM TMP_IMPORT_CUSTOMER tmp
                            WHERE IMPORT_CUSTOMER_CD = @0 AND CUSTOMER_CD IS NULL 
                             AND EXISTS (SELECT * FROM M_CUSTOMER cust WHERE tmp.CUSTOMER_CODE = cust.CUSTOMER_CODE  )
                            ";

            DataTable dt = L5sSql.Query(sql, tmpViewState);

            if (dt.Rows.Count <= 0)
                return true;
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "CUSTOMER_CODE");
                return false;
            }
        }


        private bool P5sCheckSalesMultiDistributor(string tmpViewState, out string result)
        {
            result = "";
            String sql = @"
                                   SELECT SALES_CODE 
                                   FROM 
                                     (
                                          SELECT DISTINCT DISTRIBUTOR_CODE,SALES_CODE
                                                FROM TMP_IMPORT_CUSTOMER
                                            WHERE IMPORT_CUSTOMER_CD = @0
                                      )     AS T
                                    GROUP BY SALES_CODE
                                    HAVING COUNT(*) >= 2                        


                                ";


            DataTable dt = L5sSql.Query(sql, tmpViewState);
            if (dt.Rows.Count <= 0)
            {
                return true;
            }
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "SALES_CODE");
                return false;
            }
        }



        private Boolean P5sCheckRouteMultiSales(String tmpViewState, out String result)
        {
            result = "";
            String sql = @"
                                   SELECT ROUTE_CODE 
                                   FROM 
                                     (
                                          SELECT DISTINCT ROUTE_CODE,SALES_CODE
                                                FROM TMP_IMPORT_CUSTOMER
                                            WHERE IMPORT_CUSTOMER_CD = @0
                                      )    AS T
                                    GROUP BY ROUTE_CODE
                                    HAVING COUNT(*) >= 2                        


                                ";


            DataTable dt = L5sSql.Query(sql, tmpViewState);
            if (dt.Rows.Count <= 0)
            {
                return true;
            }
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "ROUTE_CODE");
                return false;
            }
        }

        private Boolean P5sCheckSales(String tmpViewState, out String result)
        {
            result = "";
            String sql = @"
                                 --SALES OF DISTRIBUTOR    
                                 UPDATE TMP_IMPORT_CUSTOMER SET SALES_CD = sls.SALES_CD
                                 FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_SALES sls ON tmp.SALES_CODE = sls.SALES_CODE
                                 WHERE IMPORT_CUSTOMER_CD = @0  AND sls.DISTRIBUTOR_CD = tmp.DISTRIBUTOR_CD

                                 --SALES INACTIVE   
                                 UPDATE TMP_IMPORT_CUSTOMER SET SALES_CD = sls.SALES_CD
                                 FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_SALES sls ON tmp.SALES_CODE = sls.SALES_CODE
                                 WHERE IMPORT_CUSTOMER_CD = @0 -- AND sls.ACTIVE  = 0     


                                ";
            L5sSql.Execute(sql, tmpViewState);


            sql = @"SELECT DISTINCT SALES_CODE AS SALES_CODE FROM TMP_IMPORT_CUSTOMER tmp
                            WHERE IMPORT_CUSTOMER_CD = @0 AND SALES_CD IS NULL 
                             AND EXISTS (SELECT * FROM M_SALES sls WHERE tmp.SALES_CODE = sls.SALES_CODE  )                 

                            ";

            DataTable dt = L5sSql.Query(sql, tmpViewState);

            if (dt.Rows.Count <= 0)
            {
                return true;
                //                //check duplicate
                //                sql = @"SELECT ROUTE_CODE,SALES_CODE,DISTRIBUTOR_CODE FROM TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0
                //                        GROUP BY ROUTE_CODE,SALES_CODE,DISTRIBUTOR_CODE
                //                        HAVING COUNT(*) >= 2
                //                ";

                //                dt = L5sSql.Query(sql, tmpViewState);
                //                if (dt.Rows.Count <= 0)
                //                    return true;
                //                else
                //                {
                //                    result = "Duplicate ROUTE_CODE,SALES_CODE,DISTRIBUTOR_CODE ";
                //                    return false;
                //                }
            }
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "SALES_CODE");
                return false;
            }

        }

        private Boolean P5sCheckCommune(String tmpViewState, out String result)
        {
            result = "";

            //Get COMMUNE_CD từ COMMUNE_CODE
            String sql = @"UPDATE TMP_IMPORT_CUSTOMER SET COMMUNE_CD = cmm.COMMUNE_CD
                                FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_COMMUNE cmm ON tmp.COMMUNE_CODE = cmm.COMMUNE_CODE
                                WHERE IMPORT_CUSTOMER_CD = @0  ";
            L5sSql.Execute(sql, tmpViewState);



            sql = @"SELECT * FROM
                            ( 
                                SELECT DISTINCT COMMUNE_CODE AS COMMUNE_CODE FROM TMP_IMPORT_CUSTOMER
                                WHERE IMPORT_CUSTOMER_CD = @0  AND  COMMUNE_CODE = '' AND COMMUNE_CD IS NULL 
                            ) AS T WHERE T.COMMUNE_CODE != '' ";

            DataTable dt = L5sSql.Query(sql, tmpViewState);

            if (dt.Rows.Count <= 0)
                return true;
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "COMMUNE_CODE");
                return false;
            }

        }



        private Boolean P5sCheckDistributor(String tmpViewState, out String result)
        {
            result = "";
            //get DISTRIBUTOR_CD from DISTRIBUTOR_CODE
            String sql = @"UPDATE TMP_IMPORT_CUSTOMER SET DISTRIBUTOR_CD = dis.DISTRIBUTOR_CD, DISTRIBUTOR_ACTIVE = dis.ACTIVE
                                FROM TMP_IMPORT_CUSTOMER tmp INNER JOIN M_DISTRIBUTOR dis ON tmp.DISTRIBUTOR_CODE = dis.DISTRIBUTOR_CODE
                                WHERE IMPORT_CUSTOMER_CD = @0";
            L5sSql.Execute(sql, tmpViewState);



            sql = @"SELECT DISTINCT DISTRIBUTOR_CODE AS DISTRIBUTOR_CODE FROM TMP_IMPORT_CUSTOMER
                            WHERE IMPORT_CUSTOMER_CD = @0 AND  DISTRIBUTOR_CD IS NULL  ";

            DataTable dt = L5sSql.Query(sql, tmpViewState);

            if (dt.Rows.Count <= 0)
                return true;
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "DISTRIBUTOR_CODE");
                return false;
            }

        }
        #endregion
        #endregion


        public void Inactive_Customer()
        {
            try
            {
                string sql = @"select max(REPLACE(CUSTOMER_CODE,'VNC','')) as CUSTOMER_CODE  from TMP_IMPORT_CUSTOMER";
                DataTable dt = L5sSql.Query(sql);
                string _Codemax = dt.Rows[0][0].ToString();
                L5sSql.Execute(string.Format(@"exec INACTIVE_CUSTOMER 'update','{0}'
											   exec INACTIVE_ROUTE 'update'
                                               exec INACTIVE_SALES 'update'", _Codemax));
            }
            catch(Exception ex)
            {
                P5sCmmFns.P5sWriteHistory("InactiveCustomer", true, "InactiveCustomer", ex.ToString());
            }
           
        }
        public void Dashboard_MTD_Primary()
        {
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout("exec CONSOLIDATE_DASHBOARD_MTD_PRIMARY", 36000);
                P5sCmmFns.P5sWriteHistory("CONSOLIDATE_DASHBOARD_MTD_PRIMARY", true, "CONSOLIDATE_DASHBOARD_MTD_PRIMARY","Update Successfull!");
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("CONSOLIDATE_DASHBOARD_MTD_PRIMARY", false, "CONSOLIDATE_DASHBOARD_MTD_PRIMARY", "Update fail!");
            }
        }


    }
}