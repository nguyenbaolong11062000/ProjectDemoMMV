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
using MMV.Tool;
using System.Globalization;

namespace MMV.InterfaceTH
{
    public partial class AutoUploadDailyCustomer : System.Web.UI.Page
    {
        #region Khai báo Upload Customer
        UploadFile upload = new UploadFile();
        string distributor_not_exist = "";
        string route_sale_multiple = "";
        string dailyCustomerList = "";
        string linksave2 = "";
        string linkMoveto2 = "";
        string TBTemp2 = "TMP_IMPORT_CUSTOMER";
        int namelocation2 = 24;
        string[] arr2 = { "CUSTOMER_CODE", "DISTRIBUTOR_CODE", "CUSTOMER_NAME","NAME2", "CUSTOMER_ADDRESS","STREET2"
                            ,"STREET3","STREET4","CITY","ROUTE_CODE","MOBILEPHONE","IDMS_CUSTOMER_CODE","GLOBAL_RE","CUSTOMER_CHAIN_CODE","INDIA_SUB_RE"
                            ,"SALES_CODE","SALES_DISTRICT","SALES_REGION","REGION","LONGITUDE_LATITUDE","LONGTITIDE","CREATE_DATE_TH","CREATE_BY","ACTIVE_Y_N","IMPORT_CUSTOMER_CD"};
        string cosCD2 = "IMPORT_CUSTOMER_CD";
        #endregion

        #region Khai báo ordersalesh
        string salesh = "";
        string linksave = "";
        string linkMoveto = "";
        string TBTemp = "TMP_TH_SALESH_DATA";
        int namelocation = 20;
        string[] arr = { "DISTRIBUTOR_CODE"
                        , "SITE_CODE"
                        , "ORDER_NO"
                        , "INVOICE_NO"
                        ,"REFERENCE_NO"
                        ,"BILL_TYPE"
                        ,"RE"
                        ,"SUB_RE"
                        ,"SAP_CUST_CODE"
                        ,"IDMS_CUST_CODE"
                        ,"ORDER_DATE_TIME"
                        ,"BILL_DATE_TIME"
                        ,"SAP_SALESMAN_CODE"
                        ,"MANUAL_DISC"
                        ,"MANUAL_DISC_VAL"
                        ,"LIST_PRICE"
                        ,"NET_VALUE"
                        ,"SCHEME_AMOUNT"
                        ,"PO_NUMBER"
                        ,"SAP_ID"
                        ,"TH_SALESH_DATA_CD"
                        };
        string cosCD = "TH_SALESH_DATA_CD";

        #endregion

        #region Khai báo ordersalesi
        string salesi = "";
        string linksavei = "";
        string linkMovetoi = "";
        string TBTempi = "TMP_TH_SALESI_DATA";
        int namelocationi = 15;
        string[] arri = {   "DISTRIBUTOR_CODE"
                            ,"ORDER_NO"
                            ,"INVOICE_NO"
                            ,"REFERENCE_NO"
                            ,"BILL_TYPE"
                            ,"PRODUCT_CODE"
                            ,"SALES_IN_CS"
                            ,"SALES_IN_A_UNIT"
                            ,"A_UNIT"
                            ,"FREE_QTY_QC"
                            ,"KZWI1"
                            ,"GROSS_SALES"
                            ,"SCHEME_AMOUNT"
                            ,"MANUAL_DISC"
                            ,"NET_VALUES"
                            ,"TH_SALESI_DATA_CD"
                        };
        string cosCDi = "TH_SALESI_DATA_CD";

        #endregion

        #region Khai báo Month to date Primary
        // Auto Upload cho Month To Date Primary Thái Lan
        string _mtdp = "";
        string _patchSave = "";
        string _patchBackUp = "";
        string _TBTempMTDP = "TMP_MTD_PRIMARY_UPDATE_TH";
        string _CDTempMTDP = "TMP_MTD_PRIMARY_UPDATE_CD";
        int _numberMTDP = 36;
        string[] _arrMTDP = {   "STK_CD"
                            ,"INV_NO"
                            ,"INV_DATE"
                            ,"SKU_CD"
                            ,"SKU_DESC"
                            ,"LINE_NO"
                            ,"DASH_CODE"
                            ,"INV_RATE"
                            ,"NET_INV_VAL"
                            ,"TAX"
                            ,"PURCH_VAL"
                            ,"INV_QTY"
                            ,"UOM_REC"
                            ,"INVOICE_VALUE"
                            ,"NET_INV_VAL_1"
                            ,"INVOICE_TYPE"
                            ,"UOM_REC_NO_CPK"
                            ,"CASE_FACTOR_PC"
                            ,"MRP_DOZ"
                            ,"NET_WEIGHT"
                            ,"FIELD21"
                            ,"FIELD22"
                            ,"FIELD23"
                            ,"CATEGORY"
                            ,"CATEGORY_DES"
                            ,"SUB_CATE"
                            ,"SUB_CATE_DES"
                            ,"SIZE"
                            ,"SUB_BRAND"
                            ,"SUB_BRAND_DES"
                            ,"VARIANT"
                            ,"VARIANT_DES"
                            ,"FIELD33"
                            ,"FIELD34"
                            ,"FIELD35"
                            ,"FIELD36"
                            ,"TMP_MTD_PRIMARY_UPDATE_CD"
                        };

        #endregion

        #region Khai báo route
        string route = "";
        string linksaveRoute = "";
        string linkMovetoRoute = "";
        string TBTempRoute = "TMP_IMPORT_ROUTE";
        int namelocationRoute = 13; //Số trường cần đọc trong file txt
        string[] arrRoute = {  "ROUTE_CODE"
                              ,"ROUTE_DESC"
                              ,"SITE"
                              ,"DIST_CUST_NO"
                              ,"VISIT_FRE"
                              ,"SUNDAY"
                              ,"MONDAY"
                              ,"TUESDAY"
                              ,"WEDNESDAY"
                              ,"THURSDAY"
                              ,"FRIDAY"
                              ,"SATURDAY"
                              ,"IDMS_ROUTE"
                              ,"TMP_ROUTE_CD"
                            };
        string cosCDRoute = "TMP_ROUTE_CD";
        #endregion

        #region Khai báo Sales
        String Product_Code = "";
        string sales = "";
        string linksaveSales = "";
        string linkMovetoSales = "";
        string TBTempSales = "TMP_IMPORT_SALES";
        int namelocationSales = 12; //Số trường cần đọc trong file txt
        string[] arrSales = {  "SALES_CODE"
                              ,"SALES_NAME"
                              ,"SITE"
                              ,"SAP_CODE"
                              ,"JOINING_DATE"
                              ,"GENDER"
                              ,"MOBILE"
                              ,"DATE_OF_BIRTH"
                              ,"OLD_CODE"
                              ,"CREATE_DATE"
                              ,"CREATE_BY"
                              ,"ACTIVE"
                              ,"TMP_SALES_CD"
    };
        string cosCDSales = "TMP_SALES_CD";
        #endregion

        #region Khai báo DSR

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.LoadForLoginPage();
            try
            {
                this.UploadSalesMan();
                this.UploadRoute();
                this.UploadCustomer();

            }
            catch (Exception ex)
            {
                P5sCmmFns.P5sWriteHistory("Run UploadSalesMan,UploadRoute,UploadCustomer errors", false, ex.ToString());
            }
            try
            {
                this.UploadSalesHData();
            }
            catch (Exception)
            {

            }
            try
            {
                this.UploadSalesIData();
                this.UploadDaiLySales();
                this.AutoUploadMTD_YTD();//update consolidate MTD rồi đến YTD
                this.AutoUploadMTD_NewDashboard();//update 5 dashboard moi cua ThaiLand
                this.Upload_MTD_PRIMARY_DATA_TH();
            }
            catch(Exception ex)
            {
                P5sCmmFns.P5sWriteHistory("Run UploadSalesIData,UploadDaiLySales,AutoUploadMTD_YTD errors", false, ex.ToString());
            } 
        }

        #region Upload Customer
        private void UploadCustomer()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_FILE],PATH_BACKUP_FILE,[PATH_NAME] FROM [S_PATH_UPLOAD] where PATH_CODE = 'THCUSTOMERMASTER'");
            linksave2 = dt.Rows[0][0].ToString();
            linkMoveto2 = dt.Rows[0][1].ToString();
            string dailyCustomerList = dt.Rows[0][2].ToString();
            //Upload daily CustomerList

            string filenamedailyCustomerList = dailyCustomerList + ".txt";
            string filepathdailyCustomerList = linksave2 + filenamedailyCustomerList;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailyCustomerList);
            string extension = Path.GetExtension(filepathdailyCustomerList);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }
            string fl = upload.ReadTXTFromFolderCustomerCPTH("AutoUpload", filenamedailyCustomerList, filepathdailyCustomerList, TBTemp2, namelocation2, arr2, "", cosCD2, out tmpViewState, namelocation2, "0");
            if (fl == "1") // Nếu đọc file customer thành công thì update DSR Name và Route Desc vào bảng TMP_IMPORT_CUSTOMER
            {
                this.UpdateDataToCustomer();
            }
            else if (fl == "-1")
            {
                return;
            }

            #region
            try
            {
                DeleteCustInactive();
                String result = "";
                L5sSql.Execute(String.Format(@"delete N_CUSTOMER_LIST where IMPORT_CUSTOMER_CD not in ('{0}') ", tmpViewState));
                if (!this.P5sCheckDistributor(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("Distributor code not exists: {0}", result)));
                    result = result.Replace(",", "','");
                    result = "'" + result + "'";
                    this.distributor_not_exist = result;
                    String sqlDistributor = String.Format(@"

                    insert into N_CUSTOMER_LIST(IMPORT_CUSTOMER_CD
                                            ,[DESCRIPTION]
                                            ,[DISTRIBUTOR_CODE]
					                        ,[DISTRIBUTOR_NAME]
					                        ,[CUSTOMER_CODE]
					                        ,[CUSTOMER_NAME]
					                        ,[CUSTOMER_ADDRESS]
					                        ,[CUSTOMER_CHAIN_CODE]
					                        ,[CUSTOMER_DISPLAY]
					                        ,[SALES_CODE]
					                        ,[SALES_NAME]
					                        ,[ROUTE_CODE]
					                        ,[ROUTE_DESC]
					                        ,[COMMUNE_CODE]
					                        ,[CREATED_DATE]
					                        ,[DISTRIBUTOR_CD]
					                        ,[DISTRIBUTOR_ACTIVE]
					                        ,[CUSTOMER_CD]
					                        ,[SALES_CD]
					                        ,[ROUTE_CD]
					                        ,[COMMUNE_CD]
					                        ,[LONGITUDE_LATITUDE]
					                        ,[IS_NEW_SALES]
					                        ,[IS_NEW_ROUTE]
					                        ,[IS_NEW_CUSTOMER]
					                        ,[CUSTOMER_ACTIVE]
					                        ,[ACTIVE]
					                        ,[IDMS_CUSTOMER_CODE]
					                        ,[IDMS_SALES_CODE]
					                        ,[IDMS_ROUTE_CODE]
					                        ,[GLOBAL_RE]
					                        ,[NAME2]
					                        ,[STREET2]
					                        ,[STREET3]
					                        ,[STREET4]
					                        ,[CITY]
					                        ,[MOBILEPHONE]
					                        ,[INDIA_RE]
					                        ,[INDIA_SUB_RE]
					                        ,[SALES_DISTRICT]
					                        ,[SALES_REGION]
					                        ,[REGION]
					                        ,[LATITIDE]
					                        ,[LONGTITIDE]
					                        ,[CREATE_DATE_TH]
					                        ,[CREATE_BY]
					                        ,[ACTIVE_Y_N])

					                        select IMPORT_CUSTOMER_CD
                                            ,'Distributor code not exists'
                                            ,[DISTRIBUTOR_CODE]
					                        ,[DISTRIBUTOR_NAME]
					                        ,[CUSTOMER_CODE]
					                        ,[CUSTOMER_NAME]
					                        ,[CUSTOMER_ADDRESS]
					                        ,[CUSTOMER_CHAIN_CODE]
					                        ,[CUSTOMER_DISPLAY]
					                        ,[SALES_CODE]
					                        ,[SALES_NAME]
					                        ,[ROUTE_CODE]
					                        ,[ROUTE_DESC]
					                        ,[COMMUNE_CODE]
					                        ,[CREATED_DATE]
					                        ,[DISTRIBUTOR_CD]
					                        ,[DISTRIBUTOR_ACTIVE]
					                        ,[CUSTOMER_CD]
					                        ,[SALES_CD]
					                        ,[ROUTE_CD]
					                        ,[COMMUNE_CD]
					                        ,[LONGITUDE_LATITUDE]
					                        ,[IS_NEW_SALES]
					                        ,[IS_NEW_ROUTE]
					                        ,[IS_NEW_CUSTOMER]
					                        ,[CUSTOMER_ACTIVE]
					                        ,[ACTIVE]
					                        ,[IDMS_CUSTOMER_CODE]
					                        ,[IDMS_SALES_CODE]
					                        ,[IDMS_ROUTE_CODE]
					                        ,[GLOBAL_RE]
					                        ,[NAME2]
					                        ,[STREET2]
					                        ,[STREET3]
					                        ,[STREET4]
					                        ,[CITY]
					                        ,[MOBILEPHONE]
					                        ,[INDIA_RE]
					                        ,[INDIA_SUB_RE]
					                        ,[SALES_DISTRICT]
					                        ,[SALES_REGION]
					                        ,[REGION]
					                        ,[LATITIDE]
					                        ,[LONGTITIDE]
					                        ,[CREATE_DATE_TH]
					                        ,[CREATE_BY]
					                        ,[ACTIVE_Y_N]
					                        from TMP_IMPORT_CUSTOMER 
                                            where DISTRIBUTOR_CODE in ({0})", result);
                    L5sSql.Execute(sqlDistributor);


                    //this.RollbackFinish(tmpViewState);

                    // return;
                }

                if (!this.P5sCheckRouteMultiSales(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("Route managed by many DSR: {0}", result)));

                    result = result.Replace(",", "','");
                    result = "'" + result + "'";
                    this.route_sale_multiple = result;//

                    String sqlSaleMultiplte = String.Format(@"

                    insert into N_CUSTOMER_LIST(IMPORT_CUSTOMER_CD
                                            ,[DESCRIPTION]
                                            ,[DISTRIBUTOR_CODE]
					                        ,[DISTRIBUTOR_NAME]
					                        ,[CUSTOMER_CODE]
					                        ,[CUSTOMER_NAME]
					                        ,[CUSTOMER_ADDRESS]
					                        ,[CUSTOMER_CHAIN_CODE]
					                        ,[CUSTOMER_DISPLAY]
					                        ,[SALES_CODE]
					                        ,[SALES_NAME]
					                        ,[ROUTE_CODE]
					                        ,[ROUTE_DESC]
					                        ,[COMMUNE_CODE]
					                        ,[CREATED_DATE]
					                        ,[DISTRIBUTOR_CD]
					                        ,[DISTRIBUTOR_ACTIVE]
					                        ,[CUSTOMER_CD]
					                        ,[SALES_CD]
					                        ,[ROUTE_CD]
					                        ,[COMMUNE_CD]
					                        ,[LONGITUDE_LATITUDE]
					                        ,[IS_NEW_SALES]
					                        ,[IS_NEW_ROUTE]
					                        ,[IS_NEW_CUSTOMER]
					                        ,[CUSTOMER_ACTIVE]
					                        ,[ACTIVE]
					                        ,[IDMS_CUSTOMER_CODE]
					                        ,[IDMS_SALES_CODE]
					                        ,[IDMS_ROUTE_CODE]
					                        ,[GLOBAL_RE]
					                        ,[NAME2]
					                        ,[STREET2]
					                        ,[STREET3]
					                        ,[STREET4]
					                        ,[CITY]
					                        ,[MOBILEPHONE]
					                        ,[INDIA_RE]
					                        ,[INDIA_SUB_RE]
					                        ,[SALES_DISTRICT]
					                        ,[SALES_REGION]
					                        ,[REGION]
					                        ,[LATITIDE]
					                        ,[LONGTITIDE]
					                        ,[CREATE_DATE_TH]
					                        ,[CREATE_BY]
					                        ,[ACTIVE_Y_N])

					                        select IMPORT_CUSTOMER_CD 
                                            ,'Route managed by many DSR'
                                            ,[DISTRIBUTOR_CODE]
					                        ,[DISTRIBUTOR_NAME]
					                        ,[CUSTOMER_CODE]
					                        ,[CUSTOMER_NAME]
					                        ,[CUSTOMER_ADDRESS]
					                        ,[CUSTOMER_CHAIN_CODE]
					                        ,[CUSTOMER_DISPLAY]
					                        ,[SALES_CODE]
					                        ,[SALES_NAME]
					                        ,[ROUTE_CODE]
					                        ,[ROUTE_DESC]
					                        ,[COMMUNE_CODE]
					                        ,[CREATED_DATE]
					                        ,[DISTRIBUTOR_CD]
					                        ,[DISTRIBUTOR_ACTIVE]
					                        ,[CUSTOMER_CD]
					                        ,[SALES_CD]
					                        ,[ROUTE_CD]
					                        ,[COMMUNE_CD]
					                        ,[LONGITUDE_LATITUDE]
					                        ,[IS_NEW_SALES]
					                        ,[IS_NEW_ROUTE]
					                        ,[IS_NEW_CUSTOMER]
					                        ,[CUSTOMER_ACTIVE]
					                        ,[ACTIVE]
					                        ,[IDMS_CUSTOMER_CODE]
					                        ,[IDMS_SALES_CODE]
					                        ,[IDMS_ROUTE_CODE]
					                        ,[GLOBAL_RE]
					                        ,[NAME2]
					                        ,[STREET2]
					                        ,[STREET3]
					                        ,[STREET4]
					                        ,[CITY]
					                        ,[MOBILEPHONE]
					                        ,[INDIA_RE]
					                        ,[INDIA_SUB_RE]
					                        ,[SALES_DISTRICT]
					                        ,[SALES_REGION]
					                        ,[REGION]
					                        ,[LATITIDE]
					                        ,[LONGTITIDE]
					                        ,[CREATE_DATE_TH]
					                        ,[CREATE_BY]
					                        ,[ACTIVE_Y_N]
					                        from TMP_IMPORT_CUSTOMER 
                                            where ROUTE_CODE in ({0}) ", result);
                    L5sSql.Execute(sqlSaleMultiplte);

                    //this.RollbackFinish(tmpViewState);

                    // return;
                }
                //check distributor_not_exist and sale_multiple not null or Empty
                if (!String.IsNullOrEmpty(this.distributor_not_exist) || !String.IsNullOrEmpty(this.route_sale_multiple))
                {

                    this.Delete_CUSTOMER(this.distributor_not_exist, this.route_sale_multiple);
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




                if (!this.P5sCheckSalesMultiDistributor(tmpViewState, out result))
                {
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName, (string.Format("DSR managed by many distributor: {0}", result)));
                    this.RollbackFinish(tmpViewState);
                    return;
                }
               
                //update data for customer exists DB

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
                             //L5sSql.Execute(sql);
                            InactiveSale_Customer_Route();
                            // P5sCmm.P5sCmmFns.P5sUpdateValueForMap();
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
            #endregion

        }
        private void InactiveSale_Customer_Route()
        {
            string sql = @"update M_CUSTOMER SET ACTIVE = 0
                WHERE CUSTOMER_CODE NOT IN (select CUSTOMER_CODE from TMP_IMPORT_CUSTOMER)

                update M_ROUTE SET ACTIVE = 0
                WHERE ROUTE_CODE NOT IN (select ROUTE_CODE from TMP_IMPORT_CUSTOMER)

                update M_SALES SET ACTIVE = 0
                WHERE SALES_CODE NOT IN (select SALES_CODE From TMP_IMPORT_CUSTOMER)


                UPDATE O_CUSTOMER_ROUTE
                SET ACTIVE = 0
                WHERE CUSTOMER_CD NOT IN (SELECT CUSTOMER_CD FROM M_CUSTOMER WHERE ACTIVE = 1)


                UPDATE O_SALES_ROUTE
                SET ACTIVE = 0
                WHERE ROUTE_CD NOT IN (SELECT ROUTE_CD FROM M_ROUTE WHERE ACTIVE = 1)
                select 1";
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout(sql, 36000);
                P5sCmmFns.P5sWriteHistory("ImportCustomer", true, "Inactive Customer Sales Route", "Upload successful!");
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("ImportCustomer", false, "Inactive Customer Sales Route", "Upload fail!");
            }
           
            

        }
        private void UploadRoute()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_NAME],[PATH_FILE],[PATH_BACKUP_FILE] FROM [S_PATH_UPLOAD] where PATH_CODE = 'THROUTE'");
            route = dt.Rows[0][0].ToString();
            linksaveRoute = dt.Rows[0][1].ToString();
            linkMovetoRoute = dt.Rows[0][2].ToString();

            //Upload salesh data
            string filenamedailysales = route + ".txt";
            string filepathdailysales = linksaveRoute + filenamedailysales;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailysales);
            string extension = Path.GetExtension(filenamedailysales);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }
            string fl = upload.ReadDataFromTextToDB("Import Route Data", filenamedailysales, filepathdailysales, TBTempRoute, namelocationRoute, arrRoute, "", cosCDRoute, out tmpViewState, namelocationRoute);

            if (!string.IsNullOrEmpty(tmpViewState))
            {
                if (fl == "1")
                {
                    upload.Move_File(linksaveRoute, linkMovetoRoute, filenamedailysales);
                    P5sCmmFns.P5sWriteHistory("Import Route", true, fileName, "Upload successful");
                }
            }
        }
        private void UploadSalesMan()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_NAME],[PATH_FILE],[PATH_BACKUP_FILE] FROM [S_PATH_UPLOAD] where PATH_CODE = 'THSALESMAN'");
            sales = dt.Rows[0][0].ToString();
            linksaveSales = dt.Rows[0][1].ToString();
            linkMovetoSales = dt.Rows[0][2].ToString();

            //Upload salesh data
            string filenamedailysales = sales + ".txt";
            string filepathdailysales = linksaveSales + filenamedailysales;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailysales);
            string extension = Path.GetExtension(filenamedailysales);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }
            string fl = upload.ReadDataFromTextToDB("Import Sales Data", filenamedailysales, filepathdailysales, TBTempSales, namelocationSales, arrSales, "", cosCDSales, out tmpViewState, namelocationSales);

            if (!string.IsNullOrEmpty(tmpViewState))
            {
                if (fl == "1")
                {
                    upload.Move_File(linksaveSales, linkMovetoSales, filenamedailysales);
                    P5sCmmFns.P5sWriteHistory("Import Sales", true, fileName, "Upload successful");
                }
            }
        }

        private void UpdateDataToCustomer()
        {
            L5sSql.Execute(@"  update TMP_IMPORT_CUSTOMER set ROUTE_DESC=rt.ROUTE_DESC, SALES_NAME=sale.SALES_NAME
                                from TMP_IMPORT_CUSTOMER cus
                                left join TMP_IMPORT_ROUTE rt on cus.ROUTE_CODE = rt.ROUTE_CODE
                                left join TMP_IMPORT_SALES sale on cus.SALES_CODE = sale.SALES_CODE

                                TRUNCATE TABLE TMP_IMPORT_ROUTE
                                TRUNCATE TABLE TMP_IMPORT_SALES");
        }
        #endregion

        #region Check data trước khi import customer vào

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
            L5sSql.Execute(@"DELETE TMP_IMPORT_CUSTOMER WHERE IMPORT_CUSTOMER_CD = @0 OR CREATED_DATE < GETDATE() - 1", tempViewState);
        }

        private bool P5sCheckCustomerDuplicate(string tmpViewState, out string result)
        {
            result = "";
            String sql = @"     SELECT CUSTOMER_CODE 
                                FROM  TMP_IMPORT_CUSTOMER
                                WHERE IMPORT_CUSTOMER_CD = @0                             
                                GROUP BY CUSTOMER_CODE
                                HAVING COUNT(*) >= 2";
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

        public void Delete_CUSTOMER(string distributor, string route_sale)
        {
            String sqlDeleteCustomer = String.Format(@"delete TMP_IMPORT_CUSTOMER 
                                                        where [DISTRIBUTOR_CODE] in ({0}) or [ROUTE_CODE] in ({1})", distributor, route_sale);
            L5sSql.Execute(sqlDeleteCustomer);
        }

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
            catch (Exception ex)
            {
                P5sCmmFns.P5sWriteHistory("InactiveCustomer", true, "InactiveCustomer", ex.ToString());
            }

        }

        #endregion

        #region Upload SalesI
        private void UploadSalesIData()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_NAME],[PATH_FILE],[PATH_BACKUP_FILE] FROM [S_PATH_UPLOAD] where PATH_CODE = 'THSALESI'");
            salesi = dt.Rows[0][0].ToString();
            linksavei = dt.Rows[0][1].ToString();
            linkMovetoi = dt.Rows[0][2].ToString();

            //Upload salesh data
            string filenamedailysales = salesi + ".txt";
            string filepathdailysales = linksavei + filenamedailysales;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailysales);
            string extension = Path.GetExtension(filenamedailysales);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }
            string fl = upload.ReadSalesIDataFromFolder("Import SALES_I Data", filenamedailysales, filepathdailysales, TBTempi, namelocationi, arri, "", cosCDi, out tmpViewState, namelocationi);

            if (fl == "1")
            {
                try
                {
                    P5sCmm.P5sCmmFns.SqlDatatableTimeout("EXEC IMPORT_DATA_SALESI_TO_SAP_DMS",36000);
                    P5sCmm.P5sCmmFns.P5sWriteHistory("ImportDataSaleIFromMMC_CPTHToSAP_DMS", true, fileName, "Import successful");

                }
                catch (Exception ex)
                {
                    P5sCmm.P5sCmmFns.P5sWriteHistory("ImportDataSaleIFromMMC_CPTHToSAP_DMS", false, fileName, ex.ToString());

                }
            }
            try
            {
                if (!string.IsNullOrEmpty(tmpViewState))
                {
                    if (fl == "1")
                    {
                        if (checkedMasterSalesIData(tmpViewState, filenamedailysales))
                        {
                            P5sInsertSalesIData(tmpViewState);
                            //Consolidate data for map MMV - 190114 DTP
                            //upload.RollbackFinish(TBTempi);
                            P5sCmmFns.P5sWriteHistory("Import Sales I Data", true, filenamedailysales, "Upload successful");
                            upload.Move_File(linksavei, linkMovetoi, filenamedailysales);
                        }
                        else
                        {
                            P5sCmmFns.P5sWriteHistory("Import SalesI Data", false, filenamedailysales, "Upload fail!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                P5sCmmFns.P5sWriteHistory("Import SalesI Data", false, filenamedailysales, ex.ToString());
            }
            finally
            {
                L5sSql.Execute("TRUNCATE TABLE TMP_TH_SALESI_DATA");
            }

        }
        #endregion

        #region Upload SalesH
        private void UploadSalesHData()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_NAME],[PATH_FILE],[PATH_BACKUP_FILE] FROM [S_PATH_UPLOAD] where PATH_CODE = 'THSALESH'");
            salesh = dt.Rows[0][0].ToString();
            linksave = dt.Rows[0][1].ToString();
            linkMoveto = dt.Rows[0][2].ToString();

            //Upload salesh data
            string filenamedailysales = salesh + ".txt";
            string filepathdailysales = linksave + filenamedailysales;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailysales);
            string extension = Path.GetExtension(filenamedailysales);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }

            string fl = upload.ReadSalesHDataFromFolder("Import SALES_H Data", filenamedailysales, filepathdailysales, TBTemp, namelocation, arr, "", cosCD, out tmpViewState, namelocation);

            try
            {
                L5sSql.Execute("EXEC IMPORT_DATA_SALESH_TO_SAP_DMS");
                P5sCmm.P5sCmmFns.P5sWriteHistory("ImportDataSaleHFromMMC_CPTHToSAP_DMS", true, fileName, "Import successful");

            }
            catch (Exception ex)
            {
                P5sCmm.P5sCmmFns.P5sWriteHistory("ImportDataSaleHFromMMC_CPTHToSAP_DMS", false, fileName, ex.ToString());

            }

            try
            {
                if (!string.IsNullOrEmpty(tmpViewState))
                {
                    if (fl == "1")
                    {
                        if (checkedMasterSalesHData(tmpViewState, filenamedailysales))
                        {
                            P5sInsertSalesHData(tmpViewState);
                            //Consolidate data for map MMV - 190114 DTP
                            //upload.RollbackFinish(TBTemp);
                            P5sCmmFns.P5sWriteHistory("Import SalesH Data", true, filenamedailysales, "Upload successful");
                            upload.Move_File(linksave, linkMoveto, filenamedailysales);
                        }
                        else
                        {
                            P5sCmmFns.P5sWriteHistory("Import SalesH Data", false, filenamedailysales, "Upload fail!");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                P5sCmmFns.P5sWriteHistory("Import SalesH Data", false, filenamedailysales, ex.ToString());
            }
            finally
            {
                L5sSql.Execute("TRUNCATE TABLE TMP_TH_SALESH_DATA");

            }

        }

        private bool checkedMasterSalesIData(string viewState, string filenamedailysales)
        {
            L5sSql.Execute("delete N_TH_SALESI_DATA where PRODUCT_CODE  in (select PRODUCT_CODE from O_PRODUCT where ACTIVE = 1)");
            bool flag = true;
            string sqlPro = @"SELECT PRODUCT_CODE
                            FROM TMP_TH_SALESI_DATA _tsi
                            WHERE 	NOT EXISTS (
	                            SELECT * 
	                            FROM O_PRODUCT 
	                            WHERE PRODUCT_CODE=_tsi.PRODUCT_CODE
                            )  AND TH_SALESI_DATA_CD=@0 
                            GROUP BY PRODUCT_CODE";
            DataTable dtPro = L5sSql.Query(sqlPro, viewState);
            if (dtPro.Rows.Count > 0)
            {
                string strP = "PRODUCT_CODE IS NOT EXISTS: " + dtPro.Rows[0][0].ToString();
                for (int i = 0; i < dtPro.Rows.Count; i++)
                {
                    strP += "," + dtPro.Rows[i][0];
                }
                //P5sCmmFns.P5sWriteHistory("Import SalesI Data", true, filenamedailysales, strP);
                flag = false;
                String product_code = "";
                strP = strP.Replace(",", "','");
                product_code = "'" + strP + "'";
                
                string sql_product = String.Format(@"          
                            insert into N_TH_SALESI_DATA(
                            [DISTRIBUTOR_CODE]
                                ,[ORDER_NO]
                                ,[INVOICE_NO]
                                ,[REFERENCE_NO]
                                ,[BILL_TYPE]
                                ,[PRODUCT_CODE]
                                ,[SALES_IN_CS]
                                ,[SALES_IN_A_UNIT]
                                ,[A_UNIT]
                                ,[FREE_QTY_QC]
                                ,[KZWI1]
                                ,[GROSS_SALES]
                                ,[SCHEME_AMOUNT]
                                ,[MANUAL_DISC]
                                ,[NET_VALUES]
                                ,[TH_SALESI_DATA_CD]
                            )

                            SELECT 
                                [DISTRIBUTOR_CODE]
                                ,[ORDER_NO]
                                ,[INVOICE_NO]
                                ,[REFERENCE_NO]
                                ,[BILL_TYPE]
                                ,[PRODUCT_CODE]
                                ,[SALES_IN_CS]
                                ,[SALES_IN_A_UNIT]
                                ,[A_UNIT]
                                ,[FREE_QTY_QC]
                                ,[KZWI1]
                                ,[GROSS_SALES]
                                ,[SCHEME_AMOUNT]
                                ,[MANUAL_DISC]
                                ,[NET_VALUES]
                                ,[TH_SALESI_DATA_CD]
                            FROM [TMP_TH_SALESI_DATA]
                            where PRODUCT_CODE IN ({0})
                                ", product_code);
                L5sSql.Execute(sql_product);
            }


            if (flag)
                return true;
            //L5sSql.Execute("TRUNCATE TABLE TMP_TH_SALESI_DATA");
           // L5sMsg.Show("Import SALESI fail! Check H_FILE_UPLOAD to show detail.");
            return true;
        }

        private bool checkedMasterSalesHData(string viewState, string filenamedailysales)
        {
            bool flag = true;
            string sqlCus = @"SELECT SAP_CUST_CODE 
                            FROM TMP_TH_SALESH_DATA _tsh 
                            WHERE 	NOT EXISTS (
	                            SELECT * 
	                            FROM M_CUSTOMER 
	                            WHERE CUSTOMER_CODE=_tsh.SAP_CUST_CODE
                            )  AND TH_SALESH_DATA_CD=@0 
                            GROUP BY SAP_CUST_CODE";
            DataTable dtCus = L5sSql.Query(sqlCus, viewState);
            if (dtCus.Rows.Count > 0)
            {
                string strC = "CUSTOMER_CODE IS NOT EXISTS: " + dtCus.Rows[0][0].ToString();
                for (int i = 0; i < dtCus.Rows.Count; i++)
                {
                    strC += "," + dtCus.Rows[i][0];
                }
                //P5sCmmFns.P5sWriteHistory("ImportSalesHData", true, filenamedailysales, strC);
                flag = false;
            }

            string sqlSale = @"SELECT SAP_SALESMAN_CODE 
                                 FROM TMP_TH_SALESH_DATA _tsh 
                                 WHERE 	NOT EXISTS (
	                                SELECT * 
	                                FROM M_SALES 
	                                WHERE SALES_CODE=_tsh.SAP_SALESMAN_CODE
                                 )	  AND TH_SALESH_DATA_CD=  @0
                                 GROUP BY SAP_SALESMAN_CODE";
            DataTable dtSale = L5sSql.Query(sqlSale, viewState);
            if (dtSale.Rows.Count > 0)
            {
                string strS = "SALE_CODE IS NOT EXISTS: " + dtSale.Rows[0][0].ToString();
                for (int i = 0; i < dtSale.Rows.Count; i++)
                {
                    strS += "," + dtSale.Rows[i][0];
                }
                //P5sCmmFns.P5sWriteHistory("ImportSalesHData", true, filenamedailysales, strS);
                flag = false;
            }
            if (flag)
                return true;
            //L5sSql.Execute("TRUNCATE TABLE TMP_TH_SALESH_DATA");
            //L5sMsg.Show("Import SALESH fail! Check H_FILE_UPLOAD to show detail.");
            return true;
        }
        #endregion

        #region SQL insert SalesI, SalesH
        private void P5sInsertSalesHData(String tmpViewState)
        {
            //Update TH_SALESH_DATA nếu ORDER_NO trong đã tồn tại
           
            //timeout 3600
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout(String.Format(@"UPDATE TH_SALESH_DATA
                                SET SITE_CODE=TPSHL.SITE_CODE,
	                                REFERENCE_NO=TPSHL.REFERENCE_NO,
	                                BILL_TYPE=TPSHL.BILL_TYPE,
	                                RE=TPSHL.RE,
	                                SUB_RE=TPSHL.SUB_RE,
	                                SAP_CUST_CODE=TPSHL.SAP_CUST_CODE,
	                                IDMS_CUST_CODE=TPSHL.IDMS_CUST_CODE,
	                                ORDER_DATE_TIME=CASE TPSHL.ORDER_DATE_TIME WHEN '' THEN NULL ELSE TPSHL.ORDER_DATE_TIME END,
	                                BILL_DATE_TIME=CASE TPSHL.BILL_DATE_TIME WHEN '' THEN NULL ELSE TPSHL.BILL_DATE_TIME END,
	                                SAP_SALESMAN_CODE=TPSHL.SAP_SALESMAN_CODE,
	                                MANUAL_DISC=TPSHL.MANUAL_DISC,
	                                MANUAL_DISC_VAL=TPSHL.MANUAL_DISC_VAL,
	                                LIST_PRICE=TPSHL.LIST_PRICE,
	                                NET_VALUE=TPSHL.NET_VALUE,
	                                SCHEME_AMOUNT=TPSHL.SCHEME_AMOUNT,
	                                PO_NUMBER=TPSHL.PO_NUMBER,
	                                SAP_ID=TPSHL.SAP_ID,
	                                DISTRIBUTOR_CODE_CD=dis.DISTRIBUTOR_CD,
	                                SUB_RE_CD = RE.MSS_RE_CD,
	                                SAP_SALESMAN_CODE_CD=SALE.SALES_CD
                                FROM TH_SALESH_DATA SLH
                                JOIN TMP_TH_SALESH_DATA TPSHL ON SLH.INVOICE_NO=TPSHL.INVOICE_NO AND SLH.DISTRIBUTOR_CODE=TPSHL.DISTRIBUTOR_CODE
                                LEFT JOIN M_DISTRIBUTOR DIS ON SLH.DISTRIBUTOR_CODE=DIS.DISTRIBUTOR_CODE AND DIS.ACTIVE=1
                                LEFT JOIN M_MSS_RE RE ON SLH.SUB_RE=RE.RE_CODE AND RE.ACTIVE=1
                                LEFT JOIN M_SALES SALE ON SLH.SAP_SALESMAN_CODE=SALE.SALES_CODE AND SALE.ACTIVE=1
                                WHERE TPSHL.TH_SALESH_DATA_CD='{0}'
                                 select 1
                                ", tmpViewState), 36000);
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("UPDATE TH_SALESH_DATA", false, "UPDATE TH_SALESH_DATA", "Upload fail!");
            }
            //Insert data từ bảng tạm vào TH_SALESH_DATA
            
            ///Timeout 3600
            ///
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout(String.Format(@"INSERT INTO TH_SALESH_DATA
                                ([DISTRIBUTOR_CODE],[DISTRIBUTOR_CODE_CD],[SITE_CODE],[ORDER_NO],[INVOICE_NO],[REFERENCE_NO],[BILL_TYPE]
	                            ,[RE],[SUB_RE],[SUB_RE_CD],[SAP_CUST_CODE],[IDMS_CUST_CODE],[ORDER_DATE_TIME],[BILL_DATE_TIME]
	                            ,[SAP_SALESMAN_CODE],[SAP_SALESMAN_CODE_CD],[MANUAL_DISC],[MANUAL_DISC_VAL],[LIST_PRICE],[NET_VALUE],[SCHEME_AMOUNT]
	                            ,[PO_NUMBER],[SAP_ID],[CREATE_DATE],[ACTIVE])
                            SELECT   SH.[DISTRIBUTOR_CODE],DIS.[DISTRIBUTOR_CD],[SITE_CODE],[ORDER_NO],[INVOICE_NO],[REFERENCE_NO],[BILL_TYPE]
	                            ,[RE],[SUB_RE],RE.[MSS_RE_CD],[SAP_CUST_CODE],[IDMS_CUST_CODE]
	                            ,CASE [ORDER_DATE_TIME] WHEN '' THEN NULL ELSE [ORDER_DATE_TIME] END AS [ORDER_DATE_TIME]
	                            ,CASE [BILL_DATE_TIME]  WHEN '' THEN NULL ELSE [BILL_DATE_TIME] END AS [BILL_DATE_TIME]
	                            ,[SAP_SALESMAN_CODE],SALE.[SALES_CD],[MANUAL_DISC],[MANUAL_DISC_VAL],[LIST_PRICE],[NET_VALUE],[SCHEME_AMOUNT]
	                            ,[PO_NUMBER],[SAP_ID],GETDATE() AS [CREATE_DATE],1 AS [ACTIVE]
                            FROM TMP_TH_SALESH_DATA SH
                            LEFT JOIN M_DISTRIBUTOR DIS ON SH.DISTRIBUTOR_CODE=DIS.DISTRIBUTOR_CODE AND DIS.ACTIVE=1
                            LEFT JOIN M_MSS_RE RE ON SH.SUB_RE=RE.RE_CODE AND RE.ACTIVE=1
                            LEFT JOIN M_SALES SALE ON SH.SAP_SALESMAN_CODE=SALE.SALES_CODE AND SALE.ACTIVE=1
                            WHERE TH_SALESH_DATA_CD ='{0}' AND  INVOICE_NO NOT IN (SELECT INVOICE_NO FROM TH_SALESH_DATA)

                            --TRUNCATE TABLE TMP_TH_SALESH_DATA
                            
                            --Update Customer CD trong SALESH DATA
                            UPDATE TH_SALESH_DATA 
                            SET SAP_CUST_CODE_CD=CUSTOMER_CD
                            FROM TH_SALESH_DATA	_sh
                            JOIN M_CUSTOMER _cus ON _sh.SAP_CUST_CODE=_cus.CUSTOMER_CODE 
                            WHERE SAP_CUST_CODE_CD IS NULL
                            
                            --Update Sales CD trong salesh data
                            UPDATE TH_SALESH_DATA 
                            SET SAP_SALESMAN_CODE_CD=_sale.SALES_CD
                            FROM TH_SALESH_DATA	_sh
                            JOIN M_SALES _sale ON _sale.SALES_CODE=_sh.SAP_SALESMAN_CODE
                            WHERE SAP_SALESMAN_CODE_CD IS NULL
                             select 1
                            ", tmpViewState), 36000);
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("INSERT INTO TH_SALESH_DATA", false, "INSERT INTO TH_SALESH_DATA", "Upload fail!");
            }
        }

        private void P5sInsertSalesIData(String tmpViewState)
        {
            //P5sCmm.P5sCmmFns.SqlDatatableTimeout("exec Consolidate_YTD_SEC_CPTH_V2", 36000);
            //Update TH_SALESI_DATA nếu ORDER_NO đã tồn tại
            
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout(String.Format(@"UPDATE TH_SALESI_DATA
                            SET
                                    [REFERENCE_NO]=tsli.[REFERENCE_NO]
                                    ,[BILL_TYPE]=tsli.[BILL_TYPE]
                                    ,[SALES_IN_CS]=tsli.[SALES_IN_CS]
                                    ,[SALES_IN_A_UNIT]=tsli.[SALES_IN_A_UNIT]
                                    ,[A_UNIT]=tsli.[A_UNIT]
                                    ,[FREE_QTY_QC]=tsli.[FREE_QTY_QC]
                                    ,[KZWI1]=tsli.[KZWI1]
                                    ,[GROSS_SALES]=tsli.[GROSS_SALES]
                                    ,[SCHEME_AMOUNT]=tsli.[SCHEME_AMOUNT]
                                    ,[MANUAL_DISC]=tsli.[MANUAL_DISC]
                                    ,[NET_VALUES]=tsli.[NET_VALUES]
                            FROM TH_SALESI_DATA sli
                            INNER JOIN [TMP_TH_SALESI_DATA] tsli on sli.INVOICE_NO=tsli.INVOICE_NO and sli.DISTRIBUTOR_CODE=tsli.DISTRIBUTOR_CODE and sli.PRODUCT_CODE=tsli.PRODUCT_CODE
                            WHERE  [TH_SALESI_DATA_CD]='{0}' 
                            select 1", tmpViewState), 36000);
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("UPDATE TH_SALESI_DATA", false, "UPDATE TH_SALESI_DATA", "Upload fail!");
            }
            //Insert data từ bảng tạm vào TH_SALESI_DATA
          
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout(String.Format(@"INSERT INTO TH_SALESI_DATA ([DISTRIBUTOR_CODE],[ORDER_NO],[INVOICE_NO]
                                    ,[REFERENCE_NO],[BILL_TYPE],[PRODUCT_CODE],[SALES_IN_CS]
                                    ,[SALES_IN_A_UNIT],[A_UNIT],[FREE_QTY_QC],[KZWI1]
                                    ,[GROSS_SALES],[SCHEME_AMOUNT],[MANUAL_DISC]
                                    ,[NET_VALUES],[CREATE_DATE],[ACTIVE])
                            SELECT [DISTRIBUTOR_CODE]
                                    ,[ORDER_NO]
                                    ,[INVOICE_NO]
                                    ,[REFERENCE_NO]
                                    ,[BILL_TYPE]
                                    ,[PRODUCT_CODE]
                                    ,[SALES_IN_CS]
                                    ,[SALES_IN_A_UNIT]
                                    ,[A_UNIT]
                                    ,[FREE_QTY_QC]
                                    ,[KZWI1]
                                    ,[GROSS_SALES]
                                    ,[SCHEME_AMOUNT]
                                    ,[MANUAL_DISC]
                                    ,[NET_VALUES]
	                                ,GETDATE() AS [CREATE_DATE],'1' AS [ACTIVE]
                            FROM [TMP_TH_SALESI_DATA] TMP
                            WHERE NOT EXISTS (
	                            SELECT	[DISTRIBUTOR_CODE],[ORDER_NO],[INVOICE_NO]
                                    ,[REFERENCE_NO],[BILL_TYPE],[PRODUCT_CODE],[SALES_IN_CS]
                                    ,[SALES_IN_A_UNIT],[A_UNIT],[FREE_QTY_QC],[KZWI1]
                                    ,[GROSS_SALES],[SCHEME_AMOUNT],[MANUAL_DISC]
                                    ,[NET_VALUES],[CREATE_DATE],[ACTIVE]
	                            FROM TH_SALESI_DATA sli
	                            WHERE SLI.DISTRIBUTOR_CODE=TMP.DISTRIBUTOR_CODE AND SLI.INVOICE_NO=TMP.INVOICE_NO AND SLI.PRODUCT_CODE=TMP.PRODUCT_CODE 
                            ) AND TMP.TH_SALESI_DATA_CD = '{0}'
                            
                            TRUNCATE TABLE TMP_TH_SALESI_DATA
                            
                            select 1
                        ", tmpViewState), 36000);
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("INSERT INTO TH_SALESI_DATA", false, "INSERT INTO TH_SALESI_DATA", "Upload fail!");
            }
        }

        #endregion

        public void DeleteCustInactive()
        {
            L5sSql.Execute("DELETE TMP_IMPORT_CUSTOMER WHERE ACTIVE_Y_N = 'N'");
        }

        public void UploadDaiLySales()
        {

            string tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();


            try
            {
                string sql_Sale_amount = String.Format(@"update [TMP_IMPORT_SALES_AMOUNT] set [IMPORT_SALES_AMOUNT_CD] = '{0}' select 1", tmpViewState);
                P5sCmm.P5sCmmFns.SqlDatatableTimeout(sql_Sale_amount, 36000);
                P5sCmmFns.P5sWriteHistory("ImportSalesAmount", true, "TMP_SALES_AMOUNT", "Upload successful");
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("ImportSalesAmount", false, "TMP_SALES_AMOUNT", "Upload fail!");

            }
            
            if (!string.IsNullOrEmpty(tmpViewState))
            {
                
                    P5sCmm.P5sCmmFns.P5sInsertSalesAmount(tmpViewState);

                  

                    try
                    {
                        P5sCmm.P5sCmmFns.SqlDatatableTimeout("exec Consolidate_Report_Performance_CPTH_V7", 36000);
                        P5sCmmFns.P5sWriteHistory("RunPerformance_CPTH_V7", true, "RunPerformance_CPTH_V7", "Upload successful");

                    }
                    catch
                    {
                        P5sCmmFns.P5sWriteHistory("RunPerformance_CPTH_V7", false, "RunPerformance_CPTH_V7", "Upload fail!");

                    }
                   
                
            }
        }
        //Consolidate_Dashboard_CPTH (daily update)
        public void AutoUploadMTD_YTD()
        {
            //Consolidate_Dashboard_MTD_SEC
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout("exec Consolidate_MTD_SEC_DASHBOARD_CPTH", 36000);
                P5sCmmFns.P5sWriteHistory("Consolidate_MTD_SEC_DASHBOARD_CPTH", true, "Consolidate_MTD_SEC_DASHBOARD_CPTH", "Upload successful");
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("Consolidate_MTD_SEC_DASHBOARD_CPTH", false, "Consolidate_MTD_SEC_DASHBOARD_CPTH", "Upload fail!");
            }

            //Consolidate_Dashboard_YTD_SEC
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout("exec Consolidate_YTD_SEC_CPTH_V2", 36000);
                P5sCmmFns.P5sWriteHistory("Consolidate_YTD_SEC_CPTH_V2", true, "Consolidate_YTD_SEC_CPTH_V2", "Upload successful");

            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("Consolidate_YTD_SEC_CPTH_V2", false, "Consolidate_YTD_SEC_CPTH_V2", "Upload fail!");

            }


            
        }

        //Consolidate Dashboard (daily update)
        public void AutoUploadMTD_NewDashboard()
        {
            //Consolidate_MTD_TO_D_MTD_ACT_TARGET
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout("exec Consolidate_MTD_TO_D_MTD_ACT_TARGET", 36000);
                P5sCmmFns.P5sWriteHistory("Consolidate_MTD_TO_D_MTD_ACT_TARGET", true, "Consolidate_MTD_TO_D_MTD_ACT_TARGET", "Upload successful");
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("Consolidate_MTD_TO_D_MTD_ACT_TARGET", false, "Consolidate_MTD_TO_D_MTD_ACT_TARGET", "Upload fail!");
            }
            //Consolidate_D_Coverage_Buying_StoreVisit
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout("exec Consolidate_D_Coverage_Buying_StoreVisit", 36000);
                P5sCmmFns.P5sWriteHistory("Consolidate_D_Coverage_Buying_StoreVisit", true, "Consolidate_D_Coverage_Buying_StoreVisit", "Upload successful");
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("Consolidate_D_Coverage_Buying_StoreVisit", false, "Consolidate_D_Coverage_Buying_StoreVisit", "Upload fail!");
            }
            //Consolidate_D_MSS_DISTRIBUTION
            try
            {
                P5sCmm.P5sCmmFns.SqlDatatableTimeout("exec Consolidate_D_MSS_DISTRIBUTION", 36000);
                P5sCmmFns.P5sWriteHistory("Consolidate_D_MSS_DISTRIBUTION", true, "Consolidate_D_MSS_DISTRIBUTION", "Upload successful");
            }
            catch
            {
                P5sCmmFns.P5sWriteHistory("Consolidate_D_MSS_DISTRIBUTION", false, "Consolidate_D_MSS_DISTRIBUTION", "Upload fail!");
            }
        }

        // Auto upload Month to date Primary Thái Lan
        // 07/10/2020
        private bool checkedMasterMTD_Primary_Data(string viewState, string filenamedailysales)
        {
            // Check Distributor Code trên file Data/ TMP_MTD_PRIMARY_UPDATE_TH có tồn tại trên hệ thống
            // Nếu không có thì ghi log
            //bool flag = true;
            string sqlPro = @"  SELECT STK_CD
                                FROM TMP_MTD_PRIMARY_UPDATE_TH _tMTP
                                WHERE NOT EXISTS (
	                                SELECT * 
	                                FROM [M_DISTRIBUTOR.] 
	                                WHERE DISTRIBUTOR_CODE = _tMTP.STK_CD
                                )  AND TMP_MTD_PRIMARY_UPDATE_CD = @0
                                GROUP BY STK_CD";
            DataTable dtPro = L5sSql.Query(sqlPro, viewState);
            if (dtPro.Rows.Count > 0)
            {
                string strP = "DISTRIBUTOR CODE IS NOT EXISTS: "; //+ dtPro.Rows[0][0].ToString();
                for (int i = 0; i < dtPro.Rows.Count; i++)
                {
                    if(i < (dtPro.Rows.Count - 1))
                    {
                        strP += dtPro.Rows[i][0] + " - ";
                    }else
                    {
                        strP += dtPro.Rows[i][0];
                    }   
                }
                P5sCmmFns.P5sWriteHistory("IMPORT_DAILY_MTD_PRIMARY_TH", true, filenamedailysales, strP);
            }
            return true;
        }
        private void Upload_MTD_PRIMARY_DATA_TH()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            DataTable dt = L5sSql.Query(@"SELECT [PATH_NAME],[PATH_FILE],[PATH_BACKUP_FILE] FROM [S_PATH_UPLOAD] where PATH_CODE = 'MTD_PRIMARY_TH'");
            _mtdp = dt.Rows[0][0].ToString();
            _patchSave = dt.Rows[0][1].ToString();
            _patchBackUp = dt.Rows[0][2].ToString();

            //Upload salesh data
            string filenamedailysales = _mtdp + ".txt";
            string filepathdailysales = _patchSave + filenamedailysales;
            String tmpViewState = "";
            string fileName = Path.GetFileName(filenamedailysales);
            string extension = Path.GetExtension(filenamedailysales);
            if (!upload.P5sValidExtenstion(extension))
            {
                return;
            }
            string fl = upload.ReadDataPrimaryFromFolder_TH("IMPORT_DAILY_MTD_PRIMARY_TH", filenamedailysales, filepathdailysales, _TBTempMTDP, _numberMTDP, _arrMTDP, "", _CDTempMTDP, out tmpViewState, _numberMTDP);

            try
            {
                if (!string.IsNullOrEmpty(tmpViewState))
                {
                    if (fl == "1")
                    {
                        if (checkedMasterMTD_Primary_Data(tmpViewState, filenamedailysales))
                        {
                            P5sCmm.P5sCmmFns.SqlDatatableTimeout(String.Format(@"EXEC IMPORT_DAILY_MTD_PRIMARY_TH '{0}'", tmpViewState), 36000);
                            //InsertMTD_Primary_Data(tmpViewState);
                            P5sCmmFns.P5sWriteHistory("IMPORT_DAILY_MTD_PRIMARY_TH", true, filenamedailysales, "Upload successful");
                            upload.Move_File(_patchSave, _patchBackUp, filenamedailysales);
                        }
                        else
                        {
                            P5sCmmFns.P5sWriteHistory("IMPORT_DAILY_MTD_PRIMARY_TH", false, filenamedailysales, "Upload fail!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                P5sCmmFns.P5sWriteHistory("IMPORT_DAILY_MTD_PRIMARY_TH", false, filenamedailysales, ex.ToString());
            }
            finally
            {
                L5sSql.Execute("TRUNCATE TABLE TMP_MTD_PRIMARY_UPDATE_TH");
            }
        }
    }
}