using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using L5sDmComm;
using System.Collections.Generic;
using System.Reflection;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using System.Configuration;
using P5sCmm;
using Org.BouncyCastle.Utilities.Encoders;
using P5sDmComm;
using System.Text;

namespace MMV.Webservices
{
    public partial class SynchronizeV2 : System.Web.Services.WebService
    {
        //Tạo các class đổi tượng
        public class SalesmanInfo
        {
            public String SalesCode;
            public String SalesName;
            public SalesmanInfo(String SalesCode, String SalesName)
            {
                this.SalesCode = SalesCode;
                this.SalesName = SalesName;
            }
        }

        public class StockiesInfo
        {
            public String DistributorCd;
            public String DistributorName;
            public StockiesInfo(String DistributorCd, String DistributorName)
            {
                this.DistributorCd = DistributorCd;
                this.DistributorName = DistributorName;
            }
        }
        public class RpKPI
        {
            public String C_HEADER;
            public String TOTAL_VALUES;
            public RpKPI(String C_HEADER, String TOTAL_VALUES)
            {
                this.C_HEADER = C_HEADER;
                this.TOTAL_VALUES = TOTAL_VALUES;
            }
        }


        public class RpSalary
        {
            public String C_HEADER;
            public String C_VALUES;
            public RpSalary(String C_HEADER, String C_VALUES)
            {
                this.C_HEADER = C_HEADER;
                this.C_VALUES = C_VALUES;
            }
        }


        #region Hàm này dùng cho APK mới FPITv7_5890_180725
        //this method is for downloading STOCKIEST.txt
        [WebMethod]
        public String synchronizeGetSTOCKIESTViaSalesmanCode(String imei, String salesmanCode)
        {
            L5sInitial.LoadForLoginPage();
            //if (!this.isValidDevice(imei))
            //    return "-1";
            DataTable dt = L5sSql.Query(@"select md.DISTRIBUTOR_CD, md.DISTRIBUTOR_NAME from M_DISTRIBUTOR md 
                                        inner join M_SALES mSales on mSales.DISTRIBUTOR_CD = md.DISTRIBUTOR_CD
                                        where mSales.SALES_CODE = @0", salesmanCode);
            return this.P5sConvertSTOCKIESTToJson(dt);
        }

        private String P5sConvertSTOCKIESTToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<StockiesInfo> STOCKIESTs = new List<StockiesInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String DISTRIBUTOR_CD = dt.Rows[i]["DISTRIBUTOR_CD"].ToString();
                String DISTRIBUTOR_NAME = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();
                StockiesInfo stockiesInfo = new StockiesInfo(DISTRIBUTOR_CD, DISTRIBUTOR_NAME);
                STOCKIESTs.Add(stockiesInfo);
            }

            return oSerializer.Serialize(STOCKIESTs);
        }

        //this method is for downloading SALESMAN.txt
        [WebMethod]
        public String synchronizeGetSalesmanViaSalesmanCode(String imei, String salesmanCode)
        {
            L5sInitial.LoadForLoginPage();
            //if (!this.isValidDevice(imei))
            //    return "-1";
            DataTable dt = L5sSql.Query(@"select mSales.SALES_CODE, mSales.SALES_NAME from M_SALES mSales where mSales.SALES_CODE = @0", salesmanCode);
            return this.P5sConvertSalesmanToJson(dt);
        }

        private String P5sConvertSalesmanToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<SalesmanInfo> CUSTBRDs = new List<SalesmanInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String SALES_CODE = dt.Rows[i]["SALES_CODE"].ToString();
                String SALES_NAME = dt.Rows[i]["SALES_NAME"].ToString();
                SalesmanInfo salesman = new SalesmanInfo(SALES_CODE, SALES_NAME);
                CUSTBRDs.Add(salesman);
            }

            return oSerializer.Serialize(CUSTBRDs);
        }
        //this method is for downloading Route.txt
        [WebMethod]
        public String synchronizeGetRouteListViaSalesmanCode(String imei, String salesmanCode)
        {
            L5sInitial.LoadForLoginPage();
            //if (!this.isValidDevice(imei))
            //    return "-1";

            //DTP - 191219 download tuyến theo profile
            DataTable dt = L5sSql.Query(@"
                    IF EXISTS(Select * from [O_HH_PROFILE] where USER_CODE = @0 and ACTIVE = 1)
                    begin 
		                    SELECT mRoute.ROUTE_CODE, mRoute.ROUTE_NAME, mRoute.ROUTE_CD
		                    from O_SALES_ROUTE osr 
		                    left join M_SALES mSales on osr.SALES_CD=mSales.SALES_CD
		                    left join M_ROUTE mRoute on mRoute.ROUTE_CD=osr.ROUTE_CD
		                    where mSales.SALES_CODE = (Select Top 1 PROFILE_CODE from [O_HH_PROFILE] where USER_CODE = @0 and ACTIVE = 1)
		                    and mRoute.ACTIVE =1  and osr.ACTIVE = 1
                            order by ROUTE_CODE ASC
                    end
                    ELSE
                    begin
		                    SELECT mRoute.ROUTE_CODE, mRoute.ROUTE_NAME, mRoute.ROUTE_CD
		                    from O_SALES_ROUTE osr 
		                    left join M_SALES mSales on osr.SALES_CD=mSales.SALES_CD
		                    left join M_ROUTE mRoute on mRoute.ROUTE_CD=osr.ROUTE_CD
		                    where mSales.SALES_CODE = @0 and mRoute.ACTIVE =1  and osr.ACTIVE = 1
                            order by ROUTE_CODE ASC
                    end", salesmanCode);
            return this.P5sConvertRouteListToJson(dt);
        }
        private String P5sConvertRouteListToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<RouteForASMervisor> p = new List<RouteForASMervisor>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String RouteCode = dt.Rows[i]["ROUTE_CODE"].ToString();
                String RouteName = dt.Rows[i]["ROUTE_NAME"].ToString();
                String RouteCd = dt.Rows[i]["ROUTE_CD"].ToString();
                RouteForASMervisor r = new RouteForASMervisor(RouteCode, RouteName, RouteCd);
                p.Add(r);
            }

            return oSerializer.Serialize(p);
        }

        //this method is for downloading CUSTOMER.txt
        [WebMethod]
        public String synchronizeGetCustomerViaRuoteCD(string imei, string _ArrRouteCd)
        {
            L5sInitial.LoadForLoginPage();
            //if (!this.isValidDevice(imei))
            //    return "-1";
            string sql = string.Format(@"select mCustomer.CUSTOMER_CODE,
                                        mCustomer.CUSTOMER_NAME AS CUS_ADD,
                                        mCustomer.CUSTOMER_NAME,
                                        mCustomer.CUSTOMER_ADDRESS,
                                        mCustomer.CUSTOMER_CHAIN_CODE
                                        from M_CUSTOMER mCustomer
                                        inner join O_CUSTOMER_ROUTE ocr on ocr.CUSTOMER_CD = mCustomer.CUSTOMER_CD
                                        inner join M_ROUTE mRoute on mRoute.ROUTE_CD = ocr.ROUTE_CD
                                        where  mCustomer.ACTIVE = 1 and mRoute.ROUTE_CD in ({0}) and ocr.ACTIVE = 1", _ArrRouteCd);
            DataTable dt = L5sSql.Query(sql);
            return this.P5sConvertCustomer1ToJson(dt);
        }
        private String P5sConvertCustomer1ToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<CCustomer> cCustomers = new List<CCustomer>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String CUSTOMER_CODE = dt.Rows[i]["CUSTOMER_CODE"].ToString();
                String CUS_ADD = dt.Rows[i]["CUS_ADD"].ToString();
                String CUSTOMER_NAME = dt.Rows[i]["CUSTOMER_NAME"].ToString();
                String CUSTOMER_ADDRESS = dt.Rows[i]["CUSTOMER_ADDRESS"].ToString();
                String CUSTOMER_CHAIN_CODE = dt.Rows[i]["CUSTOMER_CHAIN_CODE"].ToString();

                CCustomer r = new CCustomer(CUSTOMER_CODE, CUS_ADD, CUSTOMER_NAME, CUSTOMER_ADDRESS, CUSTOMER_CHAIN_CODE);
                cCustomers.Add(r);
            }

            return oSerializer.Serialize(cCustomers);
        }

        //this method is for downloading CUSTBRD.txt ,CUSTSKU.txt
        [WebMethod]
        public String synchronizeGetCUSTBRD_CUSTSKUViaRuoteCD(string imei, string _ArrRouteCd)
        {
            L5sInitial.LoadForLoginPage();
            //if (!this.isValidDevice(imei))
            //    return "-1";
            string sql = string.Format(@"select mCustomer.CUSTOMER_CODE
                                            from M_CUSTOMER mCustomer
                                            inner join O_CUSTOMER_ROUTE ocr on ocr.CUSTOMER_CD = mCustomer.CUSTOMER_CD
                                            inner join M_ROUTE mRoute on mRoute.ROUTE_CD = ocr.ROUTE_CD
                                            where mRoute.ROUTE_CD in ({0}) ", _ArrRouteCd);
            DataTable dt = L5sSql.Query(sql);
            return this.P5sConvert_CUSTBRD_CUSTSKU_ToJson(dt);
        }
        private String P5sConvert_CUSTBRD_CUSTSKU_ToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<String> CUSTBRDs = new List<String>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string CUSTOMER_CODE = dt.Rows[i]["CUSTOMER_CODE"].ToString();
                CUSTBRDs.Add(CUSTOMER_CODE);
            }

            return oSerializer.Serialize(CUSTBRDs);
        }


        ////Gọi qua server 198 để active mã SALE & IMEI
        //[WebMethod]
        //public String ActiveSaleCode(string IDMS_SALES_CODE,string SAP_SALE_CODE)
        //{
        //    L5sInitial.LoadForLoginPage();
        //    try
        //    {
        //        string query = String.Format(@" UPDATE [FSID].[dbo].[O_DEV]
        //                              SET DEV_SALES_CODE = '{1}'
        //                              where DEV_SALES_CODE = '{0}'  and ACTIVE = 1", IDMS_SALES_CODE, SAP_SALE_CODE);

        //kết nối qua connect String thứ 2
        //        long kq = L5sSql.Execute(1,query);
        //        if (kq > 0) return "1";

        //        return "-1";
        //    }
        //    catch
        //    { 
        //        return "-1";
        //    }
        //}


        [WebMethod]
        public String synchronizeGetReportKPI(String month, String year, String saleCode)
        {
            L5sInitial.LoadForLoginPage();
            //if (!this.isValidDevice(imei))
            //    return "-1";
            DataTable dt = L5sSql.Query(@"
                                        IF EXISTS(select * from [O_HH_PROFILE] where [USER_CODE] = @2 and ACTIVE =1)
                                        BEGIN
		                                        select 
			                                        [C_HEADER]
			                                        ,replace(convert(varchar,convert(Money, [C_VALUES]),1),'.00','') as TOTAL_VALUES 
		                                        from R_DSR_PERPORMANCE
		                                        where DSR_PERPORMANCE_CD in 
		                                        (
			                                        select 
				                                        max(DSR_PERPORMANCE_CD)
			                                        from R_DSR_PERPORMANCE
			                                        where  SAP_SALES_MAN_CODE = (Select Top 1 PROFILE_CODE from [O_HH_PROFILE] where [USER_CODE] = @2 and ACTIVE =1) 
			                                        and ACTIVE = 1
			                                        and year(DMS_Date) = @1 
			                                        and month(DMS_Date) = @0
			                                        group by [C_HEADER]
		                                        )
		                                        order by C_HEADER asc
                                        END
                                        ELSE 
                                        BEGIN
		                                        select 
			                                        [C_HEADER]
			                                        ,replace(convert(varchar,convert(Money, [C_VALUES]),1),'.00','') as TOTAL_VALUES 
		                                        from R_DSR_PERPORMANCE
		                                        where DSR_PERPORMANCE_CD in 
		                                        (
			                                        select 
				                                        max(DSR_PERPORMANCE_CD)
			                                        from R_DSR_PERPORMANCE
			                                        where  SAP_SALES_MAN_CODE = @2
			                                        and ACTIVE = 1
			                                        and year(DMS_Date) = @1 
			                                        and month(DMS_Date) = @0
			                                        group by [C_HEADER]
		                                        )
		                                        order by C_HEADER asc
                                        END
                                     ", month, year, saleCode);

            return this.P5sConvertReportKPIToJson(dt);
        }

        private String P5sConvertReportKPIToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<RpKPI> KPIrp = new List<RpKPI>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                String C_HEADER = dt.Rows[i]["C_HEADER"].ToString();
                String TOTAL_VALUES = dt.Rows[i]["TOTAL_VALUES"].ToString();

                RpKPI r = new RpKPI(C_HEADER, TOTAL_VALUES);
                KPIrp.Add(r);
            }

            return oSerializer.Serialize(KPIrp);
        }
        [WebMethod]
        public String synchronizeGetReportSalary(String month, String year, String saleCode)
        {
            L5sInitial.LoadForLoginPage();
            //if (!this.isValidDevice(imei))
            //    return "-1";
            DataTable dt = L5sSql.Query(@"  SELECT _SLR.C_HEADER, replace(convert(varchar,convert(Money, [C_VALUES]),1),'.00','') as C_VALUES 
                                            FROM R_DSR_SALARY _SLR
                                            LEFT JOIN R_DSR_SALARY_ORDER _order ON _SLR.C_HEADER=_order.C_HEADER
                                            WHERE DSR_SALARY_CD IN (
                                                select 
	                                                max(DSR_SALARY_CD)
                                                from R_DSR_SALARY
                                                where  left(MONTH,4) = @1 and RIGHT(MONTH,2) = @0	 and DSR_CODE=@2
                                                group by [C_HEADER]
                                            )
                                            ORDER BY _order.ORDER_C_HEADER
                                           ", month, year, saleCode);

            return this.P5sConvertReportSalaryToJson(dt);
        }

        private String P5sConvertReportSalaryToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<RpSalary> Salaryrp = new List<RpSalary>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                String C_HEADER = dt.Rows[i]["C_HEADER"].ToString();
                String C_VALUES = dt.Rows[i]["C_VALUES"].ToString();

                RpSalary r = new RpSalary(C_HEADER, C_VALUES);
                Salaryrp.Add(r);
            }

            return oSerializer.Serialize(Salaryrp);
        }

        [WebMethod]
        public String synchronizeGetCustNotBought(String month, String saleCode)
        {
            L5sInitial.LoadForLoginPage();
            //if (!this.isValidDevice(imei))
            //    return "-1";
            DataTable dt = L5sSql.Query(@"
                                        IF EXISTS(select * from [O_HH_PROFILE] where [USER_CODE] = @1 and ACTIVE =1)
                                        BEGIN
                                                SELECT cust.*,rout.ROUTE_CODE as ROUTECODE
                                                FROM M_CUSTOMER cust
                                                JOIN O_CUSTOMER_ROUTE ocr on ocr.CUSTOMER_CD = cust.CUSTOMER_CD
                                                JOIN M_ROUTE rout on rout.ROUTE_CD = ocr.ROUTE_CD
                                                JOIN O_SALES_ROUTE osr on osr.ROUTE_CD = rout.ROUTE_CD
                                                JOIN M_SALES sale on osr.SALES_CD = sale.SALES_CD
                                                where  cust.ACTIVE = 1 and sale.SALES_CODE = (Select Top 1 PROFILE_CODE from [O_HH_PROFILE] where [USER_CODE] = @1 and ACTIVE =1) -- lấy theo profile
                                                --- get cust have order in 1 or 2 or 3 month
                                                and cust.CUSTOMER_CD not in 
                                                (                                                
                                                        SELECT  osa.CUSTOMER_CD
                                                        FROM [MMV].[dbo].[O_SALES_AMOUNT] osa
                                                        where osa.SALES_CODE = (Select Top 1 PROFILE_CODE from [O_HH_PROFILE] where [USER_CODE] = @1 and ACTIVE =1) -- lấy theo profile
                                                        and  (osa.SALES_AMOUNT_DATE  BETWEEN  GETDATE() - 30*@0 and GETDATE() )
                                                )
                                        END
                                        ELSE
                                        BEGIN
                                                SELECT cust.*,rout.ROUTE_CODE as ROUTECODE
                                                FROM M_CUSTOMER cust
                                                JOIN O_CUSTOMER_ROUTE ocr on ocr.CUSTOMER_CD = cust.CUSTOMER_CD
                                                JOIN M_ROUTE rout on rout.ROUTE_CD = ocr.ROUTE_CD
                                                JOIN O_SALES_ROUTE osr on osr.ROUTE_CD = rout.ROUTE_CD
                                                JOIN M_SALES sale on osr.SALES_CD = sale.SALES_CD
                                                where  cust.ACTIVE = 1 and sale.SALES_CODE =@1
                                                --- get cust have order in 1 or 2 or 3 month
                                                and cust.CUSTOMER_CD not in 
                                                (                                                
                                                        SELECT  osa.CUSTOMER_CD
                                                        FROM [MMV].[dbo].[O_SALES_AMOUNT] osa
                                                        where osa.SALES_CODE = @1
                                                        and  (osa.SALES_AMOUNT_DATE  BETWEEN  GETDATE() - 30*@0 and GETDATE() )
                                                )
                                        END
                                           ", month, saleCode);

            return this.P5sConvertCustToJson(dt);
        }
        private String P5sConvertCustToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            List<CCustomer> customers = new List<CCustomer>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String CustomerCode = dt.Rows[i]["CUSTOMER_CODE"].ToString();
                String CustomerName = dt.Rows[i]["CUSTOMER_NAME"].ToString();
                String CustomerAddress = dt.Rows[i]["CUSTOMER_ADDRESS"].ToString();
                String CustomerPhoneNumber = dt.Rows[i]["PHONE_NUMBER"].ToString();
                String CustomerChainCode = dt.Rows[i]["CUSTOMER_CHAIN_CODE"].ToString();
                String CustomerLatitudeLongitude = dt.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                String CustomerLatitudeLongitudeAccuracy = dt.Rows[i]["LONGITUDE_LATITUDE_ACCURACY"].ToString();
                String CustomerRoute = dt.Rows[i]["ROUTECODE"].ToString();
                String CustomerActive = dt.Rows[i]["ACTIVE"].ToString();

                CCustomer c = new CCustomer(CustomerCode, CustomerName, CustomerAddress, CustomerPhoneNumber, CustomerChainCode, CustomerLatitudeLongitude, CustomerLatitudeLongitudeAccuracy, CustomerActive, CustomerRoute);
                customers.Add(c);
            }

            return oSerializer.Serialize(customers);
        }

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizeGetSurveyProgram(String imei)
        {

            String sql = @"SELECT *
                            FROM [MMV].[dbo].[O_SURVEY_PROGRAM]
                            Where  ACTIVE=1
                            AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)";
            DataTable dt = new DataTable();
            try
            {
                L5sInitial.LoadForLoginPage();
                dt = P5sSql.Query(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.GetJSONString(dt);
            }
            catch (Exception)
            {

                return "-1";
            }
        }

        //hàm đồng bộ số lượng tồn kho của khách hàng theo tuyến.
        [WebMethod]
        public String synchronizeGetRequiredStock(String _ArrRouteCd)
        {

            String sql = String.Format(@"
                         select mcust.CUSTOMER_CODE
                                ,mcust.GLOBAL_RE 
                                ,ops.PRODUCT_SIZE_CODE
                                ,ops.PRODUCT_SIZE_NAME
                                ,stock.PRODUCT_REQUIRED
                                ,CASE  WHEN  opc.PRODUCT_COUNT is null THEN 0 ELSE opc.PRODUCT_COUNT END as PRODUCT_COUNT
                                ,CASE  WHEN  opc.PRODUCT_VARIOUS is null THEN 0 ELSE opc.PRODUCT_VARIOUS END as PRODUCT_VARIOUS
                                ,CASE  WHEN  opc.PRODUCT_COUNT_DATE is null THEN NULL ELSE opc.PRODUCT_COUNT_DATE END as CREATED_DATE
                                from O_CUSTOMER_ROUTE ocr
                                inner join M_CUSTOMER mcust on mcust.CUSTOMER_CD = ocr.CUSTOMER_CD
                                join (
			                                select A.PRODUCT_SIZE_CD,A.GLOBAL_RE,PRODUCT_REQUIRED,A.PRODUCT_REQUIRED_DATE
			                                from O_PRODUCT_REQUIRED_STOCK A
			                                join (
				                                select PRODUCT_SIZE_CD,GLOBAL_RE,MAX(PRODUCT_REQUIRED_DATE) as PRODUCT_REQUIRED_DATE  
				                                from O_PRODUCT_REQUIRED_STOCK
				                                where ACTIVE = 1
				                                group by PRODUCT_SIZE_CD,GLOBAL_RE
			                                ) B
			                                on A.PRODUCT_SIZE_CD = B.PRODUCT_SIZE_CD and A.GLOBAL_RE = B.GLOBAL_RE and A.PRODUCT_REQUIRED_DATE = B.PRODUCT_REQUIRED_DATE

                                ) stock on stock.GLOBAL_RE = mcust.GLOBAL_RE
                                join O_PRODUCT_SIZE ops on ops.PRODUCT_SIZE_CD = stock.PRODUCT_SIZE_CD	
                                left join(
	                                SELECT  a.* 
	                                FROM [MMV].[dbo].[O_PRODUCT_COUNT_BY_CUSTOMER] a
	                                join (
			                                select max(procus.PRODUCT_COUNT_BY_CUSTOMER_CD) as PRODUCT_COUNT_BY_CUSTOMER_CD
				                                ,procus.CUSTOMER_CODE
				                                ,PRODUCT_SIZE_CD
			                                from O_PRODUCT_COUNT_BY_CUSTOMER procus
			                                join M_CUSTOMER cust on cust.CUSTOMER_CODE = procus.CUSTOMER_CODE
			                                join O_CUSTOMER_ROUTE ocrs on ocrs.CUSTOMER_CD = cust.CUSTOMER_CD
			                                where ocrs.ROUTE_CD in ({0}) -- RouteCD
			                                group by procus.CUSTOMER_CODE,PRODUCT_SIZE_CD

	                                ) b on a.PRODUCT_COUNT_BY_CUSTOMER_CD = b.PRODUCT_COUNT_BY_CUSTOMER_CD
                                )opc on opc.CUSTOMER_CODE = mcust.CUSTOMER_CODE and opc.PRODUCT_SIZE_CD = ops.PRODUCT_SIZE_CD
                                where ocr.ACTIVE = 1 and mcust.ACTIVE =1 and ocr.ROUTE_CD in ({0}) -- RouteCD"
                        , _ArrRouteCd);
            DataTable dt = new DataTable();
            try
            {
                L5sInitial.LoadForLoginPage();
                dt = P5sSql.Query(sql);
                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.GetJSONString(dt);
            }
            catch (Exception)
            {

                return "-1";
            }
        }

        //Hàm đồng bộ từ HH về server, cập nhật số lượng tồn kho theo từng khách hang & SKU.
        [WebMethod]
        public String synchronizeRequiredStockToServer(String JsonData)
        {
            L5sInitial.LoadForLoginPage();
            DataTable dtJson = JsonConvert.DeserializeObject<DataTable>(JsonData);
            #region SQL Query
            string SQL = @"
                BEGIN
	                    Declare @ProductCode nvarchar(50)
	                    Declare @CustomerCode nvarchar(50)
	                    Declare @StockCount bigint
		                Declare @Create_Date datetime

	                    Set @ProductCode = @0
	                    Set @CustomerCode = @1
	                    Set @StockCount = @2
		                Set @Create_Date = @3
		
	                BEGIN
				                insert into [MMV].[dbo].[O_PRODUCT_COUNT_BY_CUSTOMER]
				                (
					                [PRODUCT_SIZE_CD]
					                ,[CUSTOMER_CODE]
					                ,[PRODUCT_COUNT]
					                ,[PRODUCT_VARIOUS]
					                ,[PRODUCT_COUNT_DATE]
				                )
			                Select Top 1 prod.PRODUCT_SIZE_CD,cust.CUSTOMER_CODE 
				                ,@StockCount as PRODUCT_COUNT
				                ,(req.PRODUCT_REQUIRED - @StockCount) as  PRODUCT_VARIOUS
				                ,@Create_Date as PRODUCT_COUNT_DATE
			                from M_CUSTOMER cust
			                join [O_PRODUCT_REQUIRED_STOCK] req on req.GLOBAL_RE = cust.GLOBAL_RE 
			                join O_PRODUCT_SIZE prod on prod.PRODUCT_SIZE_CD = req.PRODUCT_SIZE_CD
			                where CUSTOMER_CODE = @CustomerCode and prod.PRODUCT_SIZE_CODE = @ProductCode
			                order by req.CREATED_DATE DESC

			                --print 'insert'
	                END
                END";
            #endregion
            try
            {
                string ProductCode = String.Empty;
                string CustomerCode = String.Empty;
                int StockCount = 0;
                String createDate = String.Empty;
                foreach (DataRow row in dtJson.Rows)
                {
                    ProductCode = row["PRODUCT_CODE"].ToString();
                    CustomerCode = row["CUSTOMER_CODE"].ToString();
                    StockCount = Int32.Parse(row["PRODUCT_COUNT"].ToString());
                    createDate = row["CREATED_DATE"].ToString();
                    L5sSql.Execute(SQL, ProductCode, CustomerCode, StockCount, createDate);

                }
            }
            catch
            {
                return "-1";
            }

            return "1";
        }


        #endregion

        private Boolean isValidDevice2()
        {
            System.Web.UI.StateBag myViewState = new System.Web.UI.StateBag();
            L5sInitial.LoadForLoginPage();
            Boolean auth = true;
            if (auth)
            {
                if (this.ServiceCredentials == null)
                    return false;

                if (this.ServiceCredentials.UserName == "5stars.com.vn-Nouser" && this.ServiceCredentials.Password == "#*&!@(*!@#&@#&@!6^@!@##@6382734")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        [WebMethod]
        public String FuncDecrypt(String JsonData)
        {
            String kq = "";
            try
            {
                L5sInitial.LoadForLoginPage();
                return kq = P5sDmComm.P5sSecurity.Decrypt(JsonData);
            }
            catch
            {
                return kq = "Error";
            }

        }

        //DTP 191219 - Làm tính năng động bộ danh sách DSR của CDS, load trên profile của FPIT. Dùng cho version từ 2913 trở đi.
        [WebMethod]
        public String synchonizeGetDSRListbyCDS(String cdm_Code)
        {


            String sql = String.Format(@"
                            IF EXISTS( select * from M_SUPERVISOR where SUPERVISOR_CODE = '{0}'  )
                                select  SUP.SUPERVISOR_CODE,sale.SALES_CODE,sale.SALES_NAME
                                from M_SUPERVISOR SUP
                                join O_SUPERVISOR_DISTRIBUTOR OS on OS.SUPERVISOR_CD = sup.SUPERVISOR_CD
                                join M_SALES sale on sale.DISTRIBUTOR_CD = OS.DISTRIBUTOR_CD
                                where sale.ACTIVE = 1 and sup.SUPERVISOR_CODE ='{0}'
                        ", cdm_Code);
            DataTable dt = new DataTable();
            try
            {
                L5sInitial.LoadForLoginPage();
                dt = P5sSql.Query(sql);

                if (dt == null || dt.Rows.Count == 0)
                    return "0";

                return this.GetJSONString(dt);
            }
            catch (Exception)
            {

                return "-1";
            }
        }

        //DTP 191219 - Lưu PROFILE từ FPIT lên server. Dùng cho version từ 2913 trở đi.
        [WebMethod]
        public String synchonizeSetProfile(String User_Code, String Profile_Code)
        {

            String sql = (@"IF EXISTS(select * from [O_HH_PROFILE] where [USER_CODE] = @0)
                            begin
                                UPDATE[O_HH_PROFILE]
                                Set PROFILE_CODE = @1,
                                    UPDATE_DATE = GETDATE()
                                where USER_CODE = @0 and ACTIVE = 1
                            end
                            ELSE
                            begin
	                            IF EXISTS( select * from M_SALES where SALES_CODE = @0 and ACTIVE =1) --USER IS DSR then type = 1
	                            begin
		                            Insert into [O_HH_PROFILE] (USER_CODE, PROFILE_CODE,PROFILE_TYPE)
		                            values(@0, @1,1)
	                            end
	                            ELSE
	                            IF EXISTS( select * from M_SUPERVISOR where SUPERVISOR_CODE = @0 and ACTIVE =1) --USER IS CDS then type = 2
	                            begin
		                            Insert into [O_HH_PROFILE] (USER_CODE, PROFILE_CODE,PROFILE_TYPE)
		                            values(@0, @1,2)
	                            end
	                            ELSE
	                            IF EXISTS( select * from M_ASM where ASM_CODE = @0 and ACTIVE =1)  --USER IS ASM then type = 4
	                            begin
		                            Insert into [O_HH_PROFILE] (USER_CODE, PROFILE_CODE,PROFILE_TYPE) 
		                            values(@0, @1,4)
	                            end
                                ELSE
	                            IF EXISTS( select * from M_RSM where RSM_CODE = @0 and ACTIVE =1)  --USER IS RSM then type = 5
	                            begin
		                            Insert into [O_HH_PROFILE] (USER_CODE, PROFILE_CODE,PROFILE_TYPE) 
		                            values(@0, @1,5)
	                            end
                            end ");
            DataTable dt = new DataTable();
            try
            {
                L5sInitial.LoadForLoginPage();
                long KQ = P5sSql.Execute(sql, User_Code, Profile_Code);

                if (KQ > 0)
                    return "1";
                else
                    return "0";

                // return this.GetJSONString(dt);
            }
            catch (Exception)
            {

                return "-1";
            }
        }

        #region  Hàm dùng cho profile trên HH từ version 2921
        [WebMethod]
        public String synchonizeGetInfoByUserID(String user_id)
        {
            String sql = String.Format(@"
                                        DECLARE @user_id nvarchar(250)
                                        SET @user_id=N'{0}' 

                                        IF EXISTS( select * from M_SUPERVISOR where SUPERVISOR_CODE = @user_id)
                                            BEGIN
	                                            SELECT (SELECT TOP 1 COUNTRY_NAME from M_COUNTRY) AS COUNTRY, _region.REGION_DESC,_are.AREA_DESC,_dist.DISTRIBUTOR_NAME,@user_id AS [USER_ID],'SUPERVISOR' AS POSITION,SALES_CODE,SALES_NAME
	                                            FROM M_SALES _sale
	                                            JOIN [M_DISTRIBUTOR.] _dist ON _dist.DISTRIBUTOR_CD=_sale.DISTRIBUTOR_CD and _dist.ACTIVE=1
	                                            JOIN M_COMMUNE _commune ON _dist.COMMUNE_CD=_commune.COMMUNE_CD and _commune.ACTIVE=1
	                                            JOIN M_DISTRICT _district ON _district.DISTRICT_CD=_commune.DISTRICT_CD and _district.ACTIVE=1
	                                            JOIN M_PROVINCE _province ON _province.PROVINCE_CD=_district.PROVINCE_CD and _province.ACTIVE=1
	                                            JOIN M_AREA_PROVINCE _area_province ON _area_province.PROVINCE_CD=_province.PROVINCE_CD and _area_province.ACTIVE=1
	                                            JOIN M_AREA _are ON _are.AREA_CD=_area_province.AREA_CD and _are.ACTIVE=1
	                                            JOIN M_REGION _region ON _region.REGION_CD=_are.REGION_CD and _region.ACTIVE=1
	                                            JOIN O_SUPERVISOR_DISTRIBUTOR _sup_dist ON _sup_dist.DISTRIBUTOR_CD=_dist.DISTRIBUTOR_CD and _sup_dist.ACTIVE=1
	                                            JOIN M_SUPERVISOR _sup ON _sup.SUPERVISOR_CD =_sup_dist.SUPERVISOR_CD and _sup.ACTIVE=1
	                                            WHERE SUPERVISOR_CODE=@user_id	and _sale.ACTIVE=1
                                                order by _sale.SALES_CODE ASC
                                            END
                                            ELSE IF EXISTS (select * from M_ASM where ASM_CODE= @user_id)
                                            BEGIN
	                                            SELECT (SELECT TOP 1 COUNTRY_NAME from M_COUNTRY) AS COUNTRY, _region.REGION_DESC,_are.AREA_DESC,_dist.DISTRIBUTOR_NAME,@user_id AS [USER_ID],'ASM' AS POSITION,SALES_CODE,SALES_NAME
	                                            FROM M_SALES _sale
	                                            JOIN [M_DISTRIBUTOR.] _dist ON _dist.DISTRIBUTOR_CD=_sale.DISTRIBUTOR_CD and _dist.ACTIVE=1
	                                            JOIN M_COMMUNE _commune ON _dist.COMMUNE_CD=_commune.COMMUNE_CD and _commune.ACTIVE=1
	                                            JOIN M_DISTRICT _district ON _district.DISTRICT_CD=_commune.DISTRICT_CD and _district.ACTIVE=1
	                                            JOIN M_PROVINCE _province ON _province.PROVINCE_CD=_district.PROVINCE_CD and _province.ACTIVE=1
	                                            JOIN M_AREA_PROVINCE _area_province ON _area_province.PROVINCE_CD=_province.PROVINCE_CD and _area_province.ACTIVE=1
	                                            JOIN M_AREA _are ON _are.AREA_CD=_area_province.AREA_CD and _are.ACTIVE=1
	                                            JOIN M_REGION _region ON _region.REGION_CD=_are.REGION_CD and _region.ACTIVE=1
	                                            JOIN O_SUPERVISOR_DISTRIBUTOR _sup_dist ON _sup_dist.DISTRIBUTOR_CD=_dist.DISTRIBUTOR_CD and _sup_dist.ACTIVE=1
	                                            JOIN M_SUPERVISOR _sup ON _sup.SUPERVISOR_CD =_sup_dist.SUPERVISOR_CD and _sup.ACTIVE=1
	                                            JOIN M_ASM _asm ON _sup.ASM_CD=_asm.ASM_CD and _asm.ACTIVE=1
	                                            WHERE _asm.ASM_CODE=@user_id   and _sale.ACTIVE=1
                                                order by _sale.SALES_CODE ASC
                                            END
                                            ELSE IF EXISTS (select * from M_RSM where RSM_CODE= @user_id)
                                            BEGIN
		                                        Declare @checkAdmin	int
		                                        Set @checkAdmin = (select REGION_CD from M_RSM where RSM_CODE= @user_id )
	
		                                        IF @checkAdmin <> 0 or @checkAdmin is null
		                                        begin
			                                        SELECT (SELECT TOP 1 COUNTRY_NAME from M_COUNTRY) AS COUNTRY, _region.REGION_DESC,_are.AREA_DESC,_dist.DISTRIBUTOR_NAME,@user_id AS [USER_ID],'RSM' AS POSITION,SALES_CODE,SALES_NAME
			                                        FROM M_SALES _sale
			                                        JOIN [M_DISTRIBUTOR.] _dist ON _dist.DISTRIBUTOR_CD=_sale.DISTRIBUTOR_CD and _dist.ACTIVE=1
			                                        JOIN M_COMMUNE _commune ON _dist.COMMUNE_CD=_commune.COMMUNE_CD and _commune.ACTIVE=1
			                                        JOIN M_DISTRICT _district ON _district.DISTRICT_CD=_commune.DISTRICT_CD and _district.ACTIVE=1
			                                        JOIN M_PROVINCE _province ON _province.PROVINCE_CD=_district.PROVINCE_CD and _province.ACTIVE=1
			                                        JOIN M_AREA_PROVINCE _area_province ON _area_province.PROVINCE_CD=_province.PROVINCE_CD and _area_province.ACTIVE=1
			                                        JOIN M_AREA _are ON _are.AREA_CD=_area_province.AREA_CD and _are.ACTIVE=1
			                                        JOIN M_REGION _region ON _region.REGION_CD=_are.REGION_CD and _region.ACTIVE=1
			                                        JOIN O_SUPERVISOR_DISTRIBUTOR _sup_dist ON _sup_dist.DISTRIBUTOR_CD=_dist.DISTRIBUTOR_CD and _sup_dist.ACTIVE=1
			                                        JOIN M_SUPERVISOR _sup ON _sup.SUPERVISOR_CD =_sup_dist.SUPERVISOR_CD and _sup.ACTIVE=1
			                                        JOIN M_ASM _asm ON _sup.ASM_CD=_asm.ASM_CD and _asm.ACTIVE=1
			                                        JOIN M_RSM _rsm ON _rsm.RSM_CD=_asm.RSM_CD and _rsm.ACTIVE=1
			                                        WHERE _rsm.RSM_CODE=@user_id  and _sale.ACTIVE=1
                                                    order by _sale.SALES_CODE ASC
		                                        end
		                                        else
		                                        begin
			                                        SELECT (SELECT TOP 1 COUNTRY_NAME from M_COUNTRY) AS COUNTRY, _region.REGION_DESC,_are.AREA_DESC,_dist.DISTRIBUTOR_NAME,@user_id AS [USER_ID],'RSM' AS POSITION,SALES_CODE,SALES_NAME
			                                        FROM M_SALES _sale
			                                        JOIN [M_DISTRIBUTOR.] _dist ON _dist.DISTRIBUTOR_CD=_sale.DISTRIBUTOR_CD and _dist.ACTIVE=1
			                                        JOIN M_COMMUNE _commune ON _dist.COMMUNE_CD=_commune.COMMUNE_CD and _commune.ACTIVE=1
			                                        JOIN M_DISTRICT _district ON _district.DISTRICT_CD=_commune.DISTRICT_CD and _district.ACTIVE=1
			                                        JOIN M_PROVINCE _province ON _province.PROVINCE_CD=_district.PROVINCE_CD and _province.ACTIVE=1
			                                        JOIN M_AREA_PROVINCE _area_province ON _area_province.PROVINCE_CD=_province.PROVINCE_CD and _area_province.ACTIVE=1
			                                        JOIN M_AREA _are ON _are.AREA_CD=_area_province.AREA_CD and _are.ACTIVE=1
			                                        JOIN M_REGION _region ON _region.REGION_CD=_are.REGION_CD and _region.ACTIVE=1
			                                        JOIN O_SUPERVISOR_DISTRIBUTOR _sup_dist ON _sup_dist.DISTRIBUTOR_CD=_dist.DISTRIBUTOR_CD and _sup_dist.ACTIVE=1
			                                        JOIN M_SUPERVISOR _sup ON _sup.SUPERVISOR_CD =_sup_dist.SUPERVISOR_CD and _sup.ACTIVE=1
			                                        JOIN M_ASM _asm ON _sup.ASM_CD=_asm.ASM_CD and _asm.ACTIVE=1
			                                        JOIN M_RSM _rsm ON _rsm.RSM_CD=_asm.RSM_CD and _rsm.ACTIVE=1
			                                        WHERE  _sale.ACTIVE=1
                                                    order by _sale.SALES_CODE ASC
		                                        end

	   
                                            END

                        ", user_id);
            DataTable dt = new DataTable();
            try
            {
                L5sInitial.LoadForLoginPage();
                dt = P5sSql.Query(sql);

                if (dt == null || dt.Rows.Count == 0)
                    return "0";

                return this.GetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }
        }
        #endregion
        [WebMethod]
        public string P5sSaveTokenID(String USER_CODE, String TokenID, String serial, String model)
        {
            L5sInitial.LoadForLoginPage();
            try
            {
                if (USER_CODE.Length <= 0)
                    return "-1";

                if (TokenID.Length <= 0)
                    return "-1";

                String sql = @"
                             Declare @USER_CD nvarchar(50)
                            select @USER_CD = USER_CD from M_USER where USER_CODE =@1

                            IF EXISTS (SELECT * FROM  M_FCM WHERE FCM_TOKEN_ID = @0)
                                        BEGIN
                                            UPDATE M_FCM  SET  USER_CD = @USER_CD WHERE FCM_TOKEN_ID = @0  
                                        END
                                        ELSE
                                        BEGIN
                                                DELETE FROM M_FCM WHERE FCM_SERIAL = @2
                                                INSERT INTO M_FCM(FCM_TOKEN_ID,USER_CD,FCM_SERIAL,FCM_MODEL) VALUES(@0,@USER_CD,@2,@3) 
                                        END                                    
                             ";
                L5sSql.Query(sql, TokenID, USER_CODE, serial, model);
                return "1";
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        [WebMethod]
        public string RequestPasscodeforMessenger(String USER_CODE)
        {
            L5sInitial.LoadForLoginPage();
            String pagename = "SynchronizeV3.cs";
            string sqllog = @"INSERT INTO S_LOG_TRANFERPASSCODE(PAGE_NAME,USER_CD,DESCRIPTION) VALUES(@0,@1,@2)";
            try
            {

                L5sSql.Execute(sqllog, pagename, USER_CODE, @"Start");
                if (USER_CODE.Length <= 0)
                    return "-1";
                //String Passcode_random = P5sCmmFns.Passcode_Random(8);
                L5sSql.Execute(sqllog, pagename, USER_CODE, @"Get Passcode");
                String Passcode_random = DateTime.Now.ToString("hhmmssffffff");
                L5sSql.Execute(sqllog, pagename, USER_CODE, @"Insert Passcode");
                string sqlrandom = string.Format(@"insert into O_ESN_TRANFERS_PASSCODE(USER_CODE,PASS_CODE) values ('{0}','{1}')", USER_CODE, Passcode_random);
                long a = L5sSql.Execute(sqlrandom);
                if (a > 0)
                {
                    L5sSql.Execute(sqllog, pagename, USER_CODE, @"Return Passcode");
                    return Passcode_random;
                }

                return "-2";
            }
            catch (Exception ex)
            {
                L5sSql.Execute(sqllog, pagename, USER_CODE, @"Error: " + ex.Message);
                return "ERR: " + ex.Message;
            }
        }
    }

}