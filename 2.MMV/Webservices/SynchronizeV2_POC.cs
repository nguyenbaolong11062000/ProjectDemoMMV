using L5sDmComm;
using Newtonsoft.Json;
using P5sCmm;
using P5sDmComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace MMV.Webservices
{
    public partial class SynchronizeV2 : System.Web.Services.WebService
    {

        //--- POC ----------------------------------------------------
        //-------- POCCheckPoint --------------

        #region getInfo: hàm lấy thông tin từ bảng  S_HH_PARAM để HH xử lý , tùy vào NVBH, CDS, AMS sẽ có 1 số thông tin param riêng
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string getPOCInfoParam(String imei, String obj, String type)
        {
            try
            {

                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");

                //if (!this.isValidDevice(imei))
                //    return this.encrypt("-1");

                if (obj.Trim().Length == 0)
                    return this.POCencrypt("-1");

                switch (type)
                {
                    case "1": // Sales
                        return this.POCencrypt(this.getPOCInfoSales(obj)); // dữ liệu đã mã hóa và dưới HH sẽ giải mã trở lại
                    case "2": // Sup
                        return this.POCencrypt(this.getPOCInfoSup(obj));
                    case "3": //ASM
                        return this.POCencrypt(this.getPOCInfoASM(obj));
                    default:
                        return this.POCencrypt("-1");
                }
            }
            catch (Exception)
            {
                return this.POCencrypt("-1");
            }

        }

        private string POCencrypt(String value)
        {
            return P5sDmComm.P5sSecurity.Encrypt(value);
        }

        //hàm lấy thông tin từ  S_HH_PARAM và 1 số thông tin khác cho NVBH để HH xử lý
        private String getPOCInfoSales(String salesCode)
        {
            try
            {
                DataTable dt = L5sSql.Query(@"SELECT HH_PARAM_KEY,HH_PARAM_VALUE FROM S_HH_PARAM WHERE ACTIVE = 1 
                                                  UNION ALL
                                                  SELECT 'TimeOfServer', FORMAT(GETDATE(),'yyyyMMdd.HHmmss') 
                                                  UNION ALL
                                                  SELECT 'TimeOfServerUTC', FORMAT(SYSUTCDATETIME(),'yyyyMMdd.HHmmss')  
                                             UNION ALL
                                             SELECT 'CustomerChainCode' ,STUFF((SELECT ',' + CUSTOMER_CHAIN_CODE 
                                                        FROM M_CUSTOMER_CHAIN
                                                        FOR XML PATH('')) ,1,1,'')
                                             UNION ALL								
											 SELECT 'Use_DMS', CONVERT(NVARCHAR,USE_DMS,103) FROM M_SALES WHERE SALES_CODE = @0
                                             UNION ALL
											SELECT 'AllowOutRangeOrder', CONVERT(NVARCHAR,ALLOW_OUT_RANGE_ORDER,103) FROM M_SALES WHERE SALES_CODE = @0
											 UNION ALL
											SELECT 'Use_5ptracking', CONVERT(NVARCHAR,USE_5PTRACKING,103) FROM M_SALES WHERE SALES_CODE = @0
                                             ", salesCode);

                DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", salesCode);

                //process clock get getocode
                if (dtSales.Rows.Count > 0)
                {
                    DataTable dtClock = dt.Clone();

                    //FlagAllowGetGeoCodeForEditCustomer = 0 : không cho phép cập nhật tọa độ với KH đã có tọa độ
                    //FlagAllowGetGeoCodeForEditCustomer = 1 : cho phép cập nhật tọa độ với KH đã có tọa độ
                    dtClock = L5sSql.Query(@" SELECT TOP 1 'FlagAllowGetGeoCodeForEditCustomer' AS HH_PARAM_KEY, '0' AS HH_PARAM_VALUE	    
                                                            FROM O_CLOCK_GET_GEOCODE clock INNER JOIN O_CLOCK_GET_GEOCODE_DETAIL det ON clock.CLOCK_CD = det.CLOCK_CD
                                                            WHERE   CONVERT (Date, GETDATE(), 103) between clock.DATE_BEGIN_CLOCK and clock.DATE_END_CLOCK
		                                                            AND clock.ACTIVE = 1 AND det.USER_CD = @0  AND det.ACTIVE = 1
                                                         ", dtSales.Rows[0]["SALES_CD"].ToString());
                    if (dtClock != null && dtClock.Rows.Count > 0)
                        dt.Merge(dtClock);
                    else
                    {   //allow get geocode
                        dtClock = L5sSql.Query(@" SELECT 'FlagAllowGetGeoCodeForEditCustomer' AS HH_PARAM_KEY, '1' AS HH_PARAM_VALUE");
                        dt.Merge(dtClock);
                    }
                }


                dt.Rows.Add(new object[] { "PasswordEncypt", P5sDmComm.P5sSecurity.GenerateKey() });
                dt.Rows.Add(new object[] { "PasswordDB", P5sDmComm.P5sSecurity.Encrypt(DateTime.Now.ToString("yyyy-MM-dd") + "GjkGjkJGHJghj5f678@&167126ZAAS655@") });

                return this.P5sConvertPOCSParamToJson(dt);
            }
            catch (Exception)
            {
                return "-1";
            }

        }

        private String getPOCInfoSup(String supCode)
        {
            try
            {
                DataTable dt = L5sSql.Query(@"SELECT HH_PARAM_KEY,HH_PARAM_VALUE FROM S_HH_PARAM WHERE ACTIVE = 1 
                                            UNION ALL
                                                SELECT 'TimeOfServer', FORMAT(GETDATE(),'yyyyMMdd.HHmmss') 
                                            UNION ALL
                                                SELECT 'TimeOfServerUTC', FORMAT(SYSUTCDATETIME(),'yyyyMMdd.HHmmss')
                                            UNION ALL
                                                SELECT 'CustomerChainCode' ,STUFF((SELECT ',' + CUSTOMER_CHAIN_CODE 
                                                FROM M_CUSTOMER_CHAIN
                                                FOR XML PATH('')) ,1,1,'') 
                                            UNION ALL								
											    SELECT 'Use_DMS', CONVERT(NVARCHAR,USE_DMS,103) FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0
                                            UNION ALL
											    SELECT 'AllowOutRangeOrder', CONVERT(NVARCHAR,ALLOW_OUT_RANGE_ORDER,103) FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0
											UNION ALL
											    SELECT 'Use_5ptracking', CONVERT(NVARCHAR,USE_5PTRACKING,103) FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0
                                             ", supCode);
                dt.Rows.Add(new object[] { "PasswordEncypt", P5sDmComm.P5sSecurity.GenerateKey() });
                dt.Rows.Add(new object[] { "PasswordDB", this.POCP5sGetPasswordDatabaseSqlite() });
                return this.P5sConvertPOCSParamToJson(dt);
            }
            catch (Exception)
            {

                return "-1";
            }

        }
        private String getPOCInfoASM(String asmCode)
        {
            try
            {
                DataTable dt = L5sSql.Query(@"SELECT HH_PARAM_KEY,HH_PARAM_VALUE FROM S_HH_PARAM WHERE ACTIVE = 1 
                                            UNION ALL
                                            SELECT 'TimeOfServer', FORMAT(GETDATE(),'yyyyMMdd.HHmmss')
                                            UNION ALL
                                            SELECT 'TimeOfServerUTC', FORMAT(SYSUTCDATETIME(),'yyyyMMdd.HHmmss') 
                                            UNION ALL
                                            SELECT 'CustomerChainCode' ,STUFF((SELECT ',' + CUSTOMER_CHAIN_CODE 
                                                        FROM M_CUSTOMER_CHAIN
                                                        FOR XML PATH('')) ,1,1,'') 
                                            UNION ALL								
											 SELECT 'Use_DMS', CONVERT(NVARCHAR,USE_DMS,103) FROM M_ASM WHERE ASM_CODE = @0
                                             UNION ALL
											SELECT 'AllowOutRangeOrder', CONVERT(NVARCHAR,ALLOW_OUT_RANGE_ORDER,103) FROM M_ASM WHERE ASM_CODE = @0
                                             ", asmCode);
                dt.Rows.Add(new object[] { "PasswordEncypt", P5sDmComm.P5sSecurity.GenerateKey() });
                dt.Rows.Add(new object[] { "PasswordDB", this.POCP5sGetPasswordDatabaseSqlite() });
                return this.P5sConvertPOCSParamToJson(dt);
            }
            catch (Exception)
            {

                return "-1";
            }

        }

        private String POCP5sGetPasswordDatabaseSqlite()
        {
            return P5sDmComm.P5sSecurity.Encrypt(DateTime.Now.ToString("yyyy-MM-dd") + "GjkGjkJGHJghj5f678@&167126ZAAS655@");
        }

        private String P5sConvertPOCSParamToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCCParam> p = new List<POCCParam>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String paramKey = dt.Rows[i]["HH_PARAM_KEY"].ToString();
                String paramValue = dt.Rows[i]["HH_PARAM_VALUE"].ToString();
                POCCParam param = new POCCParam(paramKey, paramValue);
                p.Add(param);
            }

            return oSerializer.Serialize(p);
        }
        public class POCCParam
        {
            public String ParamKey = "";
            public String ParamValue = "";
            public POCCParam(String ParamKey, String ParamValue)
            {
                this.ParamKey = ParamKey;
                this.ParamValue = ParamValue;
            }
        }

        #endregion

        #region synchronize Photo  Hàm lưu hình ảnh được gửi từ HH lên Server không theo TradeProgram hàm này hoạt động đối với các APK HH nhở hơn 6.00


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCPhoto(String imei, String obj, String type, String jsonData, byte[] image)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                if (jsonData.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCPhotoForSales(obj, type, jsonData, image);
                    case "2": // Sup
                        return this.synchronizePOCPhotoForSup(obj, type, jsonData, image);
                    case "3": //ASM
                        return this.synchronizePOCPhotoForASM(obj, type, jsonData, image);
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }


        }

        private String synchronizePOCPhotoForSales(String obj, String type, String jsonData, byte[] image)
        {
            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";

            String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
            String typeCD = type;
            try
            {

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                if (!dt.Columns.Contains("photoNote2")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote2 = new DataColumn("photoNote2");
                    photoNote2.DataType = typeof(String);
                    dt.Columns.Add(photoNote2);
                }

                if (!dt.Columns.Contains("photoNote3")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote3 = new DataColumn("photoNote3");
                    photoNote3.DataType = typeof(String);
                    dt.Columns.Add(photoNote3);
                }

                if (!dt.Columns.Contains("photoNote4")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote4 = new DataColumn("photoNote4");
                    photoNote4.DataType = typeof(String);
                    dt.Columns.Add(photoNote4);
                }

                if (!dt.Columns.Contains("photoNote5")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote5 = new DataColumn("photoNote5");
                    photoNote5.DataType = typeof(String);
                    dt.Columns.Add(photoNote5);
                }



                L5sSql.Execute(@"INSERT INTO [dbo].[O_CUSTOMER_PHOTO]
                                       (
                                        [CUSTOMER_CODE]
                                       ,[PHOTO_NAME]
                                       ,[PHOTO_PATH]
                                       ,[PHOTO_NOTES]
                                       ,[PHOTO_NOTES2]
                                       ,[PHOTO_NOTES3]
                                       ,[PHOTO_NOTES4]
                                       ,[PHOTO_NOTES5]
                                       ,[PHOTO_CREATED_DATE]
                                       ,[PHOTO_LATITUDE_LONGITUDE]
                                       ,[PHOTO_LATITUDE_LONGITUDE_ACCURACY],[SALES_CD],[TYPE_CD]
                                       )
                                 VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12)                                     
                                    "
                                        , dt.Rows[0]["photoCustomerCode"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoNote"].ToString()
                                        , dt.Rows[0]["photoNote2"].ToString()
                                        , dt.Rows[0]["photoNote3"].ToString()
                                        , dt.Rows[0]["photoNote4"].ToString()
                                        , dt.Rows[0]["photoNote5"].ToString()
                                        , dt.Rows[0]["dateTakesPhoto"].ToString()
                                        , dt.Rows[0]["geoCode"].ToString()
                                        , dt.Rows[0]["geoCodeAccuracy"].ToString()
                                        , salesCD
                                        , typeCD
                                      );

                L5sSql.Execute(@"UPDATE O_CUSTOMER_PHOTO SET CUSTOMER_CD = cust.CUSTOMER_CD
                                FROM O_CUSTOMER_PHOTO photo INNER JOIN M_CUSTOMER cust ON photo.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                WHERE   photo.CUSTOMER_CODE = @0 AND photo.CUSTOMER_CD IS NULL ", dt.Rows[0]["photoCustomerCode"].ToString());

                this.convertPOCByteImageToFile(image, dt.Rows[0]["photoName"].ToString());
            }
            catch (Exception EX)
            {
                return "-1";
            }


            return "1";

        }
        private string synchronizePOCPhotoForASM(String obj, String type, String jsonData, byte[] image)
        {

            DataTable dtASM = L5sSql.Query("SELECT  * FROM M_ASM WHERE ASM_CODE = @0", obj);
            if (dtASM.Rows.Count <= 0)
                return "-1";

            String asm = dtASM.Rows[0]["ASM_CD"].ToString();
            String typeCD = type;

            try
            {

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                if (!dt.Columns.Contains("photoNote2")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote2 = new DataColumn("photoNote2");
                    photoNote2.DataType = typeof(String);
                    dt.Columns.Add(photoNote2);
                }

                if (!dt.Columns.Contains("photoNote3")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote3 = new DataColumn("photoNote3");
                    photoNote3.DataType = typeof(String);
                    dt.Columns.Add(photoNote3);
                }

                if (!dt.Columns.Contains("photoNote4")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote4 = new DataColumn("photoNote4");
                    photoNote4.DataType = typeof(String);
                    dt.Columns.Add(photoNote4);
                }

                if (!dt.Columns.Contains("photoNote5")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote5 = new DataColumn("photoNote5");
                    photoNote5.DataType = typeof(String);
                    dt.Columns.Add(photoNote5);
                }



                L5sSql.Execute(@"INSERT INTO [dbo].[O_CUSTOMER_PHOTO]
                                       (
                                        [CUSTOMER_CODE]
                                       ,[PHOTO_NAME]
                                       ,[PHOTO_PATH]
                                       ,[PHOTO_NOTES]
                                       ,[PHOTO_NOTES2]
                                       ,[PHOTO_NOTES3]
                                       ,[PHOTO_NOTES4]
                                       ,[PHOTO_NOTES5]
                                       ,[PHOTO_CREATED_DATE]
                                       ,[PHOTO_LATITUDE_LONGITUDE]
                                       ,[PHOTO_LATITUDE_LONGITUDE_ACCURACY],[SALES_CD],[TYPE_CD]
                                       )
                                 VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12)                                     
                                    "
                                       , dt.Rows[0]["photoCustomerCode"].ToString()
                                       , dt.Rows[0]["photoName"].ToString()
                                       , dt.Rows[0]["photoName"].ToString()
                                       , dt.Rows[0]["photoNote"].ToString()
                                       , dt.Rows[0]["photoNote2"].ToString()
                                       , dt.Rows[0]["photoNote3"].ToString()
                                       , dt.Rows[0]["photoNote4"].ToString()
                                       , dt.Rows[0]["photoNote5"].ToString()
                                       , dt.Rows[0]["dateTakesPhoto"].ToString()
                                       , dt.Rows[0]["geoCode"].ToString()
                                       , dt.Rows[0]["geoCodeAccuracy"].ToString()
                                       , asm
                                       , typeCD
                                     );

                L5sSql.Execute(@"UPDATE O_CUSTOMER_PHOTO SET CUSTOMER_CD = cust.CUSTOMER_CD
                                FROM O_CUSTOMER_PHOTO photo INNER JOIN M_CUSTOMER cust ON photo.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                WHERE   photo.CUSTOMER_CODE = @0 AND photo.CUSTOMER_CD IS NULL ", dt.Rows[0]["photoCustomerCode"].ToString());

                this.convertPOCByteImageToFile(image, dt.Rows[0]["photoName"].ToString());
            }
            catch (Exception EX)
            {
                return "-1";
            }


            return "1";
        }
        private string synchronizePOCPhotoForSup(String obj, String type, String jsonData, byte[] image)
        {

            DataTable dtSup = L5sSql.Query("SELECT  * FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0", obj);
            if (dtSup.Rows.Count <= 0)
                return "-1";

            String supCD = dtSup.Rows[0]["SUPERVISOR_CD"].ToString();
            String typeCD = type;

            try
            {

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                if (!dt.Columns.Contains("photoNote2")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote2 = new DataColumn("photoNote2");
                    photoNote2.DataType = typeof(String);
                    dt.Columns.Add(photoNote2);
                }

                if (!dt.Columns.Contains("photoNote3")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote3 = new DataColumn("photoNote3");
                    photoNote3.DataType = typeof(String);
                    dt.Columns.Add(photoNote3);
                }

                if (!dt.Columns.Contains("photoNote4")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote4 = new DataColumn("photoNote4");
                    photoNote4.DataType = typeof(String);
                    dt.Columns.Add(photoNote4);
                }

                if (!dt.Columns.Contains("photoNote5")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote5 = new DataColumn("photoNote5");
                    photoNote5.DataType = typeof(String);
                    dt.Columns.Add(photoNote5);
                }



                L5sSql.Execute(@"INSERT INTO [dbo].[O_CUSTOMER_PHOTO]
                                       (
                                        [CUSTOMER_CODE]
                                       ,[PHOTO_NAME]
                                       ,[PHOTO_PATH]
                                       ,[PHOTO_NOTES]
                                       ,[PHOTO_NOTES2]
                                       ,[PHOTO_NOTES3]
                                       ,[PHOTO_NOTES4]
                                       ,[PHOTO_NOTES5]
                                       ,[PHOTO_CREATED_DATE]
                                       ,[PHOTO_LATITUDE_LONGITUDE]
                                       ,[PHOTO_LATITUDE_LONGITUDE_ACCURACY],[SALES_CD],[TYPE_CD]
                                       )
                                 VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12)                                     
                                    "
                                        , dt.Rows[0]["photoCustomerCode"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoNote"].ToString()
                                        , dt.Rows[0]["photoNote2"].ToString()
                                        , dt.Rows[0]["photoNote3"].ToString()
                                        , dt.Rows[0]["photoNote4"].ToString()
                                        , dt.Rows[0]["photoNote5"].ToString()
                                        , dt.Rows[0]["dateTakesPhoto"].ToString()
                                        , dt.Rows[0]["geoCode"].ToString()
                                        , dt.Rows[0]["geoCodeAccuracy"].ToString()
                                        , supCD
                                        , typeCD
                                      );

                L5sSql.Execute(@"UPDATE O_CUSTOMER_PHOTO SET CUSTOMER_CD = cust.CUSTOMER_CD
                                FROM O_CUSTOMER_PHOTO photo INNER JOIN M_CUSTOMER cust ON photo.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                WHERE   photo.CUSTOMER_CODE = @0 AND photo.CUSTOMER_CD IS NULL ", dt.Rows[0]["photoCustomerCode"].ToString());

                this.convertPOCByteImageToFile(image, dt.Rows[0]["photoName"].ToString());
            }
            catch (Exception EX)
            {
                return "-1";
            }


            return "1";
        }

        private void convertPOCByteImageToFile(byte[] strJSON, string fileName)
        {
            using (MemoryStream mStream = new MemoryStream(strJSON))
            {
                (Image.FromStream(mStream)).Save((Server.MapPath("~/FileUpload/Photo/") + fileName));
            }

        }

        private Boolean convertPOCByteImageToFileV2(byte[] strJSON, string fileName)
        {
            using (MemoryStream mStream = new MemoryStream(strJSON))
            {
                (Image.FromStream(mStream)).Save((Server.MapPath("~/FileUpload/TradeProgram/") + fileName));
            }
            if (!File.Exists((Server.MapPath("~/FileUpload/TradeProgram/") + fileName)))
            {
                return false;
            }
            return true;
        }


        #endregion
        #region synchronize get và upload ket qua quay thuong
        // Hàm get vòng xoay và các thông số cài đặt về số lần quay trong này
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetSpin(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";
                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");
                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGetSpinForSales(obj);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGetSpinForSales(string obj)
        {

            String url = HttpContext.Current.Request.Url.Authority + "/FileUpload/PrizeProgram/";
            if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
                url = "https://" + url;
            else
                url = "http://" + url;

            String StrGetSpin = string.Format(@"														SET XACT_ABORT ON
							BEGIN TRAN
							BEGIN TRY

							DECLARE @PROGRAM_PRIZE_CD BIGINT
							DECLARE @PRODUCT_CODE NVARCHAR(max)
							DECLARE @PRODUCT_NAME NVARCHAR(max)
							DECLARE @TMP_INFO TABLE
							(
								PROGRAM_PRIZE_CD BIGINT,
								PRODUCT_CODE NVARCHAR(max),
								PRODUCT_NAME NVARCHAR(max)
							)
							BEGIN
								DECLARE db_prize CURSOR FOR  
    							SELECT PROGRAM_PRIZE_CD,OP.PRODUCT_CODE,OP.PRODUCT_NAME
                                FROM O_PROGRAM_PRODUCT OPP
                                INNER JOIN O_PRODUCT OP ON OPP.PRODUCT_CD=OP.PRODUCT_CD 
								OPEN db_prize   
								FETCH NEXT FROM db_prize INTO @PROGRAM_PRIZE_CD,@PRODUCT_CODE,@PRODUCT_NAME  
								WHILE @@FETCH_STATUS = 0   
								BEGIN  
								DECLARE @CHECK BIGINT
								set @CHECK = (SELECT COUNT(*) FROM @TMP_INFO WHERE PROGRAM_PRIZE_CD IN (@PROGRAM_PRIZE_CD)) 
								IF(@CHECK<1)
									BEGIN 
										INSERT INTO @TMP_INFO (PROGRAM_PRIZE_CD,PRODUCT_CODE,PRODUCT_NAME)
										VALUES (@PROGRAM_PRIZE_CD,@PRODUCT_CODE,@PRODUCT_NAME)
									END 
								ELSE
									BEGIN
										DECLARE @CHECK_PRODUCT BIGINT
										SET @CHECK_PRODUCT = (SELECT COUNT(*) FROM @TMP_INFO WHERE @PRODUCT_CODE IN (PRODUCT_CODE)
																AND PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD)
										IF(@CHECK_PRODUCT<1)
											BEGIN
												UPDATE @TMP_INFO 
												SET PRODUCT_CODE = PRODUCT_CODE + '-' + @PRODUCT_CODE,
													PRODUCT_NAME=PRODUCT_NAME + '~' + @PRODUCT_NAME
												WHERE PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
											END 
									END
								FETCH NEXT FROM db_prize INTO @PROGRAM_PRIZE_CD,@PRODUCT_CODE,@PRODUCT_NAME   
								END   
								CLOSE db_prize   
								DEALLOCATE db_prize
                                --SELECT * FROM @TMP_INFO
								--DECLARE @NAMES NVARCHAR(4000) 
								DECLARE @SALE_CODE NVARCHAR(50)
								SET @SALE_CODE='{0}'
								DECLARE @URL NVARCHAR(150)
								SET @URL='{1}'
								SELECT  programprize.PROGRAM_PRIZE_CD, @URL + programprize.SPIN AS SPIN, @URL + programprize.BACKGROUND AS BACKGROUND,
										ISNULL(programprize.SALES_AMOUNT,0) AS SALES_AMOUNT,ISNULL(programprize.MAXROTATE,0) AS MAXROTATE,
										ISNULL(DAYSPIN,0) AS [MAXDAY],TMP.PRODUCT_CODE AS PRODUCT_CODE_LIST,TMP.PRODUCT_NAME AS PRODUCT_NAME_LIST,programprize.TYPE,programprize.PROGRAM_PRIZE_NAME
								FROM O_PROGRAM_PRIZE programprize WITH(NOLOCK, READUNCOMMITTED)
								LEFT JOIN @TMP_INFO TMP ON TMP.PROGRAM_PRIZE_CD=programprize.PROGRAM_PRIZE_CD 
								WHERE programprize.ACTIVE=1 
								AND programprize.CREATEDPRIZE=1
								AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)
								AND EXISTS ( SELECT * FROM  O_PROGRAM_DISTRIBUTOR programdistributor  WITH(NOLOCK, READUNCOMMITTED)
											INNER JOIN M_SALES sales WITH(NOLOCK, READUNCOMMITTED) ON programdistributor.DISTRIBUTOR_CD=sales.DISTRIBUTOR_CD AND sales.SALES_CODE=@SALE_CODE
											WHERE programdistributor.ACTIVE=1 AND programprize.PROGRAM_PRIZE_CD=programdistributor.PROGRAM_PRIZE_CD)

							END
                            COMMIT
							--SELECT * FROM @TMP_INFO
							END TRY
							BEGIN CATCH
								ROLLBACK
								SELECT -1
							END CATCH", obj, url);
            try
            {
                DataTable dt = P5sSql.Query(StrGetSpin);
                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }
        }
        // Hàm get giải thưởng
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetPrize(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGetPrizeForSales(obj);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGetPrizeForSales(String obj)
        {
            String StrGetPrize = @" DECLARE @REGION_CD BIGINT
                                    DECLARE @SALE_CODE NVARCHAR(50)
                                    DECLARE @SALE_CD BIGINT
                                    DECLARE @PROGRAM_PRIZE_CD BIGINT
                                    DECLARE @TBRESULT TABLE(
									PROGRAM_PRIZE_CD BIGINT,
									PRIZE_CD BIGINT,
									PRIZE_DEGREES INT,
									PRIZE_CODE NVARCHAR(50),
									PRIZE_DESC NVARCHAR(500),
									CUSTOMER_CODE NVARCHAR(50),
									STATUS INT,
									PRIZE_SALES DECIMAL(18,0),
									VALUE DECIMAL(18,0)
									)
                                    SET @SALE_CODE='{0}'
                                    -- LẤY THÔNG TIN SALE_CD VÀ REGION_CD ỨNG VỚI SALE
                                    SELECT TOP 1  @SALE_CD=sales.SALES_CD FROM M_SALES sales WITH(NOLOCK, READUNCOMMITTED)
                                    WHERE sales.SALES_CODE=@SALE_CODE
                                    -- LẤY CD CỦA CHƯƠNG TRÌNH QUAY THƯỞNG
                                    DECLARE db_cursor CURSOR FOR 
                                    SELECT programprize.PROGRAM_PRIZE_CD FROM O_PROGRAM_PRIZE programprize WITH(NOLOCK, READUNCOMMITTED)
                                    WHERE programprize.ACTIVE=1 
                                    AND programprize.CREATEDPRIZE=1
                                    AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)
                                    AND EXISTS ( SELECT 1 FROM  O_PROGRAM_DISTRIBUTOR programdistributor WITH(NOLOCK, READUNCOMMITTED) 
			                                    INNER JOIN M_SALES sales WITH(NOLOCK, READUNCOMMITTED) ON programdistributor.DISTRIBUTOR_CD=sales.DISTRIBUTOR_CD AND sales.SALES_CD=@SALE_CD
			                                    WHERE programdistributor.ACTIVE=1 AND programprize.PROGRAM_PRIZE_CD=programdistributor.PROGRAM_PRIZE_CD)
                                    OPEN db_cursor   
									FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD  

									WHILE @@FETCH_STATUS = 0   
									BEGIN
                                    -- DANH SÁCH CÁC GIẢI THƯỞNG ỨNG VỚI SALE ĐÃ ĐƯỢC KHAI BÁO Ở BẢNG O_PRIZE_SALES
                                    DECLARE @TBRotateSale TABLE
                                    (
	                                    ROTARE_CD BIGINT,
	                                    SALES_CD BIGINT,
	                                    PRIZE_CD BIGINT,
	                                    PRIZE_CUST_CD BIGINT
                                    )
                                    INSERT INTO @TBRotateSale(ROTARE_CD,SALES_CD,PRIZE_CD,PRIZE_CUST_CD)
                                    SELECT  rotate.ROTATE_CD,prizesales.SALES_CD,prize.PRIZE_CD,prizecust.PRIZE_CUST_CD
                                    FROM O_PRIZE_CUST prizecust WITH(NOLOCK, READUNCOMMITTED)
                                    INNER JOIN O_ROTATE rotate WITH(NOLOCK, READUNCOMMITTED) ON rotate.ROTATE_CD = prizecust.ROTATE_CD AND rotate.ACTIVE=1
                                    INNER JOIN O_PRIZE prize WITH(NOLOCK, READUNCOMMITTED) ON rotate.PRIZE_CD=prize.PRIZE_CD AND prize.ACTIVE=1 AND prize.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
                                    INNER JOIN O_PRIZE_SALES prizesales WITH(NOLOCK, READUNCOMMITTED) ON prize.PRIZE_CD=prizesales.PRIZE_CD AND prizesales.ACTIVE=1
                                    WHERE prizesales.SALES_CD =@SALE_CD AND prizecust.DSR_CD=@SALE_CD  AND prizecust.ACTIVE=1

                                    
									DECLARE @COUNTPRIZE BIGINT
									SELECT @COUNTPRIZE= COUNT(*) FROM O_PRIZE_CUST prizecust WITH(NOLOCK, READUNCOMMITTED)
									INNER JOIN O_ROTATE rotate WITH(NOLOCK, READUNCOMMITTED) ON prizecust.ROTATE_CD=rotate.ROTATE_CD
									WHERE prizecust.ACTIVE=1 AND prizecust.[STATUS]=1 AND prizecust.DSR_CD=@SALE_CD AND rotate.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
	                                    
									DECLARE @TBPRIZEFORSALE TABLE
	                                (
		                                PRIZE_CUST_CD BIGINT
	                                )
	                                INSERT INTO @TBPRIZEFORSALE(PRIZE_CUST_CD)
	                                SELECT TOP {1} TMP.PRIZE_CUST_CD FROM (
						                                --NHỮNG GIẢI THƯỞNG GIỚI HẠN(CHI TIẾT XEM Ở BẢNG O_PRIZE_SALES)
						                                SELECT prizecust.PRIZE_CUST_CD
						                                FROM O_PRIZE_CUST prizecust WITH(NOLOCK, READUNCOMMITTED)
						                                INNER JOIN O_ROTATE rotate WITH(NOLOCK, READUNCOMMITTED) ON prizecust.ROTATE_CD=rotate.ROTATE_CD AND rotate.ACTIVE=1
						                                INNER JOIN O_PRIZE prize WITH(NOLOCK, READUNCOMMITTED) ON rotate.PRIZE_CD=prize.PRIZE_CD AND prize.ACTIVE=1 AND prize.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
						                                WHERE EXISTS (SELECT PRIZE_CUST_CD FROM @TBRotateSale TMP WHERE TMP.PRIZE_CUST_CD=prizecust.PRIZE_CUST_CD) AND prizecust.[STATUS]=0 AND prizecust.ACTIVE=1
			                                ) TMP 
			                                ORDER BY NEWID()

											DECLARE @PrizeCustCD BIGINT
											DECLARE cursorPrizeCustCD CURSOR FOR
											SELECT PRIZE_CUST_CD FROM   @TBPRIZEFORSALE 
											OPEN cursorPrizeCustCD
											FETCH NEXT FROM cursorPrizeCustCD INTO @PrizeCustCD
											WHILE @@FETCH_STATUS = 0
											BEGIN
												IF @COUNTPRIZE < {1} OR @COUNTPRIZE IS NULL
												BEGIN
															-- MỖI LẦN SALE DOWNLOAD THÌ CẤP 600 GIẢ THƯỞNG
															UPDATE Prize_Cust SET Prize_Cust.SALE_CODE=@SALE_CODE,Prize_Cust.SALE_CD=@SALE_CD,Prize_Cust.[STATUS]=1,Prize_Cust.DATERANDOMFORSALE=GETDATE() 
															FROM O_PRIZE_CUST Prize_Cust 
															WHERE Prize_Cust.PRIZE_CUST_CD=@PrizeCustCD  AND Prize_Cust.[STATUS]=0 AND Prize_Cust.ACTIVE=1 AND Prize_Cust.DSR_CD=@SALE_CD

															SELECT @COUNTPRIZE= COUNT(*) FROM O_PRIZE_CUST prizecust
															INNER JOIN O_ROTATE rotate WITH(NOLOCK, READUNCOMMITTED) ON prizecust.ROTATE_CD=rotate.ROTATE_CD
															WHERE prizecust.ACTIVE=1 AND prizecust.[STATUS]=1 AND prizecust.SALE_CD=@SALE_CD AND rotate.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
	                                    
												END
											FETCH NEXT FROM cursorPrizeCustCD INTO @PrizeCustCD
											END
											CLOSE cursorPrizeCustCD
											DEALLOCATE cursorPrizeCustCD
                                           INSERT INTO @TBRESULT(PROGRAM_PRIZE_CD, PRIZE_CD, PRIZE_DEGREES, PRIZE_CODE, PRIZE_DESC, CUSTOMER_CODE, STATUS, PRIZE_SALES, VALUE)
								           SELECT @PROGRAM_PRIZE_CD, PRIZE_CD,PRIZE_DEGREES,PRIZE_CODE,PRIZE_DESC,CUSTOMER_CODE,[STATUS],PRIZE_SALES,VALUE FROM (
								           SELECT tmpprizecust.PRIZE_CUST_CD as PRIZE_CD ,tmpprizecust.PRIZE_DEGREES as PRIZE_DEGREES,prize.PRIZE_CODE as PRIZE_CODE ,prize.PRIZE_DES  as PRIZE_DESC, tmpprizecust.CUSTOMER_CODE,tmpprizecust.[STATUS], ISNULL( tmpprizecust.SALES,0) as PRIZE_SALES, prize.VALUE
							               FROM O_PRIZE_CUST tmpprizecust WITH(NOLOCK, READUNCOMMITTED) 
							               INNER JOIN O_ROTATE rotate WITH(NOLOCK, READUNCOMMITTED) ON rotate.ROTATE_CD = tmpprizecust.ROTATE_CD AND rotate.ACTIVE=1
							               INNER JOIN O_PRIZE prize WITH(NOLOCK, READUNCOMMITTED) ON rotate.PRIZE_CD=prize.PRIZE_CD AND prize.ACTIVE=1 AND prize.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
							               WHERE  tmpprizecust.DSR_CD=@SALE_CD and tmpprizecust.ACTIVE=1 AND tmpprizecust.[STATUS]=2  AND tmpprizecust.CUSTOMER_CODE IS NOT NULL AND CONVERT(DATE,tmpprizecust.PRIZE_DATE,103)=CONVERT(DATE,GETDATE(),103)
								           UNION ALL
							               SELECT tmpprizecust.PRIZE_CUST_CD as PRIZE_CD ,tmpprizecust.PRIZE_DEGREES as PRIZE_DEGREES,prize.PRIZE_CODE as PRIZE_CODE ,prize.PRIZE_DES  as PRIZE_DESC,tmpprizecust.CUSTOMER_CODE,tmpprizecust.[STATUS], ISNULL( tmpprizecust.SALES,0) as PRIZE_SALES, prize.VALUE
							               FROM O_PRIZE_CUST tmpprizecust WITH(NOLOCK, READUNCOMMITTED) 
							               INNER JOIN O_ROTATE rotate WITH(NOLOCK, READUNCOMMITTED) ON rotate.ROTATE_CD = tmpprizecust.ROTATE_CD AND rotate.ACTIVE=1
							               INNER JOIN O_PRIZE prize WITH(NOLOCK, READUNCOMMITTED) ON rotate.PRIZE_CD=prize.PRIZE_CD AND prize.ACTIVE=1 AND prize.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
							               WHERE  tmpprizecust.DSR_CD=@SALE_CD and tmpprizecust.ACTIVE=1 AND tmpprizecust.[STATUS]=1  AND tmpprizecust.CUSTOMER_CODE IS NULL
		                                   ) TMP ORDER BY [STATUS] DESC, NEWID()
                                   FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD 
                                   END
								   CLOSE db_cursor   
								   DEALLOCATE db_cursor
								   SELECT * FROM @TBRESULT";
            try
            {

                int top = 400;
                String sqlGetparam = "SELECT TOP 1 VALUE FROM S_PARAMS WHERE NAME='NUMBER_PRIZE_SYNC'";
                DataTable tb = L5sSql.Query(sqlGetparam);
                if (tb != null && tb.Rows.Count > 0)
                {
                    try
                    {
                        top = int.Parse(tb.Rows[0][0].ToString());
                    }
                    catch { }
                }

                DataTable dt = P5sSql.Query(string.Format(StrGetPrize, obj, top));

                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }

        }
        //Hàm HH up giải thưởng đã quay lên server
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCPrize(String imei, String obj, String type, String jsonData)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCSavePrizeCustForSales(obj, jsonData);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCSavePrizeCustForSales(String obj, String jsonData)
        {

            DataTable dtSales = L5sSql.Query("SELECT  SALES_CD FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1.0";

            String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
            try
            {
                String guid = obj.Trim() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                //add new column
                DataColumn columnGUID = new DataColumn("SESSION_CD");
                columnGUID.DataType = typeof(String);
                columnGUID.DefaultValue = guid;

                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(int);
                columnSalesCD.DefaultValue = salesCD;
                dt.Columns.Add(columnGUID);
                dt.Columns.Add(columnSalesCD);

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TMP_PRIZE_CUST";
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeCD", "PRIZE_CUST_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeCustomerCode", "CUSTOMER_CODE"));
                    //bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeRouteCode", "ROUTE_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeCreatedDate", "PRIZE_DATE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeReason", "REASON"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeType", "TYPE"));

                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        String query = @"DECLARE @SALES_CODE NVARCHAR(50)
                        DECLARE @SESSION_CD NVARCHAR(50)
                        DECLARE @PROGRAM_CD BIGINT
                        SET @SALES_CODE=@1
                        SET @SESSION_CD=@0
						DECLARE program_cursor CURSOR FOR  
                        SELECT programprize.PROGRAM_PRIZE_CD FROM O_PROGRAM_PRIZE programprize WITH(NOLOCK, READUNCOMMITTED)
                        WHERE programprize.ACTIVE=1 
                        AND programprize.CREATEDPRIZE=1
                        AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)
                        AND EXISTS ( SELECT * FROM  O_PROGRAM_DISTRIBUTOR programdistributor  WITH(NOLOCK, READUNCOMMITTED) 
			                        INNER JOIN M_SALES sales WITH(NOLOCK, READUNCOMMITTED) ON programdistributor.DISTRIBUTOR_CD=sales.DISTRIBUTOR_CD AND sales.SALES_CODE=@SALES_CODE
			                        WHERE programdistributor.ACTIVE=1 AND programprize.PROGRAM_PRIZE_CD=programdistributor.PROGRAM_PRIZE_CD)
						OPEN program_cursor   
						FETCH NEXT FROM program_cursor INTO @PROGRAM_CD
						WHILE @@FETCH_STATUS = 0   
						BEGIN 
								UPDATE OPrizeCust SET OPrizeCust.CUSTOMER_CODE=customer.CUSTOMER_CODE,OPrizeCust.CUSTOMER_CD= customer.CUSTOMER_CD,
								OPrizeCust.PRIZE_DATE= CONVERT(DATETIME ,tmpPrizeCust.PRIZE_DATE),OPrizeCust.[STATUS]=2, OPrizeCust.DATESYNC=GETDATE(),OPrizeCust.REASON=tmpPrizeCust.REASON,OPrizeCust.[TYPE]= tmpPrizeCust.[TYPE]
								FROM O_PRIZE_CUST OPrizeCust
								INNER JOIN TMP_PRIZE_CUST tmpPrizeCust ON OPrizeCust.PRIZE_CUST_CD=tmpPrizeCust.PRIZE_CUST_CD AND OPrizeCust.SALE_CD=tmpPrizeCust.SALES_CD AND tmpPrizeCust.SESSION_CD=@SESSION_CD
								INNER JOIN M_CUSTOMER customer ON tmpPrizeCust.CUSTOMER_CODE=customer.CUSTOMER_CODE
								LEFT JOIN M_ROUTE routesale ON tmpPrizeCust.ROUTE_CODE=tmpPrizeCust.ROUTE_CODE
								WHERE OPrizeCust.[STATUS]=1

								--DELETE FROM TMP_PRIZE_CUST WHERE SESSION_CD = @SESSION_CD OR CREATED_DATE < GETDATE()-2

								DECLARE @SALECD BIGINT
								DECLARE @PRIZECD BIGINT
								DECLARE @TRU BIGINT
								DECLARE @COUNT BIGINT
								DECLARE @CD BIGINT

								DECLARE db_cursor CURSOR FOR  
								SELECT ORPS.SALES_CD,ORPS.PRIZE_CD,ISNULL(ORPS.QUANTITY,0)- ISNULL(ORPS.NUM,0) AS TRU 
								FROM O_RETURN_PRIZE_SALES ORPS WITH(NOLOCK, READUNCOMMITTED)
								INNER JOIN O_PRIZE  OP WITH(NOLOCK, READUNCOMMITTED) ON ORPS.PRIZE_CD=OP.PRIZE_CD AND OP.ACTIVE=1
								INNER JOIN M_SALES  MS WITH(NOLOCK, READUNCOMMITTED) ON ORPS.SALES_CD=MS.SALES_CD AND SALES_CODE=@SALES_CODE
								WHERE ORPS.ACTIVE=1 AND OP.PROGRAM_PRIZE_CD=@PROGRAM_CD
								OPEN db_cursor   
								FETCH NEXT FROM db_cursor INTO @SALECD,@PRIZECD,@TRU
								WHILE @@FETCH_STATUS = 0   
								BEGIN   
									IF @TRU > 0
									BEGIN
									   SET @COUNT=1
									   WHILE(@COUNT<=@TRU)
									   BEGIN
										SET @CD=0;
										SELECT TOP 1 @CD=OPC.PRIZE_CUST_CD FROM O_PRIZE_CUST OPC WITH(NOLOCK, READUNCOMMITTED)
										INNER JOIN O_ROTATE rotate WITH(NOLOCK, READUNCOMMITTED) ON OPC.ROTATE_CD=rotate.ROTATE_CD AND rotate.ACTIVE=1
										WHERE rotate.PRIZE_CD=@PRIZECD AND OPC.[STATUS]=1 AND OPC.ACTIVE=1 AND OPC.DSR_CD=@SALECD
										ORDER BY NEWID()

										IF @CD IS NOT NULL AND @CD > 0
											BEGIN
												UPDATE O_PRIZE_CUST SET DSR_CD = NULL , SALE_CD = NULL, SALE_CODE= NULL ,[STATUS]=0 WHERE PRIZE_CUST_CD=@CD
                                
												UPDATE O_PRIZE_SALES SET QUANTITY = QUANTITY - 1 WHERE PRIZE_CD=@PRIZECD AND SALES_CD=@SALECD AND ACTIVE=1 AND QUANTITY > 0
                                
												UPDATE O_RETURN_PRIZE_SALES SET NUM= ISNULL(NUM,0)+1 WHERE PRIZE_CD=@PRIZECD AND SALES_CD=@SALECD AND ACTIVE=1
                                
												INSERT INTO H_PRIZE_SALES(SALES_CD,PRIZE_CD,QUANTITY)VALUES(@SALECD,@PRIZECD,-1)
												SET @COUNT=@COUNT+1;
											END
										ELSE
											BEGIN
												SET @COUNT=@TRU+1;
											END
									   END
									END
									  UPDATE O_PRIZE_SALES SET EXPECTED_QUANTITY=0 WHERE PRIZE_CD=@PRIZECD AND SALES_CD=@SALECD AND ACTIVE=1 AND QUANTITY > 0
									  UPDATE O_RETURN_PRIZE_SALES SET ACTIVE= 0 WHERE PRIZE_CD=@PRIZECD AND SALES_CD=@SALECD AND ACTIVE=1
									  FETCH NEXT FROM db_cursor INTO @SALECD,@PRIZECD,@TRU
                              
								END   

								CLOSE db_cursor   
								DEALLOCATE db_cursor
						FETCH NEXT FROM program_cursor INTO @PROGRAM_CD  
						END
						CLOSE program_cursor   
						DEALLOCATE program_cursor";
                        L5sSql.Query(query, guid, obj);
                        return "1";
                    }
                    catch (Exception ex)
                    {
                        return "-1.1" + ex.ToString();
                    }
                }
            }
            catch (Exception EX)
            {
                return "-1.2";
            }
            return "1";
        }


        //Hàm upload hình ảnh avatar được chụp trong quay giải
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCPrizeCustPhoto(String imei, String obj, String type, String jsonData, byte[] image)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                if (jsonData.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCPhotoPrizeCustForSales(obj, type, jsonData, image);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }


        }
        private string synchronizePOCPhotoPrizeCustForSales(string obj, string type, string jsonData, byte[] image)
        {
            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";

            String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
            String typeCD = type;
            try
            {

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                DataTable result = L5sSql.Query(@"DECLARE @PRIZE_CUST_CD BIGINT
                                DECLARE @SALES_CD BIGINT
                                DECLARE @PHOTO_NAME NVARCHAR(300)
                                DECLARE @CUSTOMER_CODE NVARCHAR(50)
                                SET @PRIZE_CUST_CD=@0
                                SET @SALES_CD=@1
                                SET @PHOTO_NAME=@2
                                SET @CUSTOMER_CODE=@3
                                IF EXISTS(SELECT 1 FROM O_PRIZE_CUST WITH(NOLOCK, READUNCOMMITTED) WHERE PRIZE_CUST_CD=@PRIZE_CUST_CD AND SALE_CD=@SALES_CD AND CUSTOMER_CODE=@CUSTOMER_CODE)
                                BEGIN
                                   INSERT INTO O_PRIZE_CUST_PHOTO(PRIZE_CUST_CD,PHOTO_NAME,CUSTOMER_CODE)VALUES (@PRIZE_CUST_CD,@PHOTO_NAME,@CUSTOMER_CODE)
                                   SELECT 1
                                END
                                ELSE
                                BEGIN
                                   SELECT 0
                                END", dt.Rows[0]["AvatarPrizeCD"].ToString(), salesCD, dt.Rows[0]["AvatarName"].ToString(), dt.Rows[0]["AvatarCustomerCode"].ToString());

                if (result.Rows[0][0].ToString() == "1")
                {
                    this.convertPOCByteImageToFileToFolderPrize(image, dt.Rows[0]["AvatarName"].ToString());
                }
                else
                {
                    return "-1";
                }
            }
            catch (Exception EX)
            {
                return EX.ToString() + "-1";
            }


            return "1";
        }
        private void convertPOCByteImageToFileToFolderPrize(byte[] strJSON, string fileName)
        {
            using (MemoryStream mStream = new MemoryStream(strJSON))
            {
                (Image.FromStream(mStream)).Save((Server.MapPath("~/FileUpload/PhotoPrize/") + fileName));
            }

        }
        // Hàm get thông tin khách hàng đã quay được bao nhiêu lần trong ngày
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetRotateTimes(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGetRotateTimes(obj);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGetRotateTimes(string obj)
        {
            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";
            String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
            try
            {
                String strGet = @"DECLARE @SALE_CODE NVARCHAR(50)
                                DECLARE @SALE_CD BIGINT
                                DECLARE @PROGRAM_PRIZE_CD BIGINT
								DECLARE @TBRESULT TABLE
								(
									PROGRAM_PRIZE_CD BIGINT,
									CUSTOMER_CODE NVARCHAR(50),
									PRIZE_TOTAL_ROTATE_TIME BIGINT,
									PRIZE_CURRENT_ROTATE_TIME BIGINT,
									DATE DATETIME,
									AMOUNT BIGINT,
									SALES DECIMAL(18,0),
                                    ORDER_CODE NVARCHAR(50)
								)
                                SET @SALE_CODE=@0
                                -- LẤY THÔNG TIN SALE_CD VÀ REGION_CD ỨNG VỚI SALE
                                SELECT TOP 1  @SALE_CD=sales.SALES_CD FROM M_SALES sales WITH(NOLOCK, READUNCOMMITTED)
                                WHERE sales.SALES_CODE=@SALE_CODE
                                -- LẤY CD CỦA CHƯƠNG TRÌNH QUAY THƯỞNG
								DECLARE db_cursor CURSOR FOR
                                SELECT programprize.PROGRAM_PRIZE_CD FROM O_PROGRAM_PRIZE programprize WITH(NOLOCK, READUNCOMMITTED)
                                WHERE programprize.ACTIVE=1 
                                AND programprize.CREATEDPRIZE=1
                                AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)
                                AND EXISTS ( SELECT programdistributor.PROGRAM_DISTRIBUTOR_CD FROM  O_PROGRAM_DISTRIBUTOR programdistributor WITH(NOLOCK, READUNCOMMITTED)
			                                INNER JOIN M_SALES sales WITH(NOLOCK, READUNCOMMITTED) ON programdistributor.DISTRIBUTOR_CD=sales.DISTRIBUTOR_CD AND sales.SALES_CD=@SALE_CD
			                                WHERE programdistributor.ACTIVE=1 AND programprize.PROGRAM_PRIZE_CD=programdistributor.PROGRAM_PRIZE_CD)
                            OPEN db_cursor   
							FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD
							WHILE @@FETCH_STATUS = 0   
							BEGIN    
								INSERT INTO @TBRESULT								 
								--GET THONG TIN KHÁCH HÀNG ĐÃ QUAY                              
								SELECT @PROGRAM_PRIZE_CD, CUSTOMER_CODE,PRIZE_TOTAL_ROTATE_TIME,PRIZE_CURRENT_ROTATE_TIME,[DATE],ISNULL( AMOUNT,0) AS AMOUNT,ISNULL( SALES,0) AS SALES, ISNULL(ORDER_CODE,'') AS ORDER_CODE  FROM (
								SELECT 
								CUSTOMER_CODE,PRIZE_TOTAL_ROTATE_TIME,PRIZE_CURRENT_ROTATE_TIME,
								CONVERT(VARCHAR, [DATE] , 120) AS DATE,ISNULL( AMOUNT,0) AMOUNT,ISNULL( SALES,0) SALES, ISNULL(ORDER_CODE,'') AS ORDER_CODE,
								ROW_NUMBER() OVER(PARTITION BY CUSTOMER_CODE ORDER BY PRIZE_ROTATE_TIMES_CD DESC) AS [Row]
								FROM O_PRIZE_ROTATE_TIMES 
								WHERE SALES_CD=@SALE_CD AND CONVERT(DATE,[DATE],103) = CONVERT(DATE,GETDATE(),103) AND PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
								) TMP WHERE TMP.[Row]= 1
							FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD  
							END
							CLOSE db_cursor   
							DEALLOCATE db_cursor
							SELECT * FROM @TBRESULT";

                DataTable dt = P5sSql.Query(strGet, obj);
                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
        // Hàm Save số lần KH đã quay trong ngày


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCRotateTimes(String imei, String obj, String type, String jsonData)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                //    if (obj.Trim().Length == 0)
                //        return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCSaveRotateTimes(obj, jsonData);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCSaveRotateTimes(String obj, String jsonData)
        {


            DataTable dtSales = L5sSql.Query("SELECT  SALES_CD FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";

            String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
            try
            {
                String guid = obj.Trim() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                //add new column
                DataColumn columnGUID = new DataColumn("SESSION_CD");
                columnGUID.DataType = typeof(String);
                columnGUID.DefaultValue = guid;

                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(int);
                columnSalesCD.DefaultValue = salesCD;
                dt.Columns.Add(columnGUID);
                dt.Columns.Add(columnSalesCD);

                try
                {
                    bool check = false;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].ColumnName.Equals("CUSTOMER_SALES"))
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        DataColumn columnCustSales = new DataColumn("CUSTOMER_SALES");
                        columnCustSales.DataType = typeof(decimal);
                        columnCustSales.DefaultValue = 0;
                        dt.Columns.Add(columnCustSales);
                    }
                }
                catch (Exception ex) { }

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TMP_PRIZE_ROTATE_TIMES";
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeRotateTimesCustomerCode", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeRotateTimes", "PRIZE_TOTAL_ROTATE_TIME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeCurrentRotateTimes", "PRIZE_CURRENT_ROTATE_TIME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeAmount", "AMOUNT"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CUSTOMER_SALES", "SALES"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeCreatedDate", "DATE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PrizeRotateProgramPrizeCD", "PROGRAM_PRIZE_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OrderCode", "ORDER_CODE"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        String query = @"DECLARE @SALE_CODE NVARCHAR(50)
                                        DECLARE @SESSION_CD NVARCHAR(50)
                                        DECLARE @SALE_CD BIGINT
                                        --DECLARE @PROGRAM_PRIZE_CD BIGINT
                                        SET @SALE_CODE=@0
                                        SET @SESSION_CD=@1
                                        -- LẤY THÔNG TIN SALE_CD VÀ REGION_CD ỨNG VỚI SALE
                                        SELECT TOP 1  @SALE_CD=sales.SALES_CD FROM M_SALES sales WITH(NOLOCK, READUNCOMMITTED)
                                        WHERE sales.SALES_CODE=@SALE_CODE                                      
												-- UPDATE CUSTOMER_CD TRONG BANG TMP
												UPDATE  TMP SET TMP.CUSTOMER_CD =MC.CUSTOMER_CD
												FROM TMP_PRIZE_ROTATE_TIMES TMP
												INNER JOIN M_CUSTOMER MC WITH(NOLOCK, READUNCOMMITTED) ON TMP.CUSTOMER_CODE=MC.CUSTOMER_CODE
												WHERE TMP.SESSION_CD=@SESSION_CD AND TMP.SALES_CD=@SALE_CD

												--NẾU CHƯA TỒN TẠI THÌ THÊM MỚI 
												INSERT INTO O_PRIZE_ROTATE_TIMES(CUSTOMER_CODE,CUSTOMER_CD,PRIZE_TOTAL_ROTATE_TIME,PRIZE_CURRENT_ROTATE_TIME,[DATE],SALES_CD,SALES_CODE,PROGRAM_PRIZE_CD,AMOUNT,SALES, ORDER_CODE)
												SELECT TMP.CUSTOMER_CODE,TMP.CUSTOMER_CD,TMP.PRIZE_TOTAL_ROTATE_TIME,TMP.PRIZE_CURRENT_ROTATE_TIME,TMP.[DATE],@SALE_CD,@SALE_CODE,TMP.PROGRAM_PRIZE_CD,TMP.AMOUNT,TMP.SALES, TMP.ORDER_CODE
												FROM TMP_PRIZE_ROTATE_TIMES TMP WITH(NOLOCK, READUNCOMMITTED)
												WHERE NOT EXISTS (
													SELECT 1 FROM O_PRIZE_ROTATE_TIMES OPRT  WITH(NOLOCK, READUNCOMMITTED)
													WHERE TMP.SALES_CD=OPRT.SALES_CD 
													AND  TMP.CUSTOMER_CD=OPRT.CUSTOMER_CD 
													AND OPRT.PROGRAM_PRIZE_CD=TMP.PROGRAM_PRIZE_CD
													AND CONVERT(DATE,OPRT.[DATE],103)=CONVERT(DATE,GETDATE(),103)
													AND OPRT.PRIZE_CURRENT_ROTATE_TIME=TMP.PRIZE_CURRENT_ROTATE_TIME
													AND OPRT.PRIZE_TOTAL_ROTATE_TIME=TMP.PRIZE_TOTAL_ROTATE_TIME
													AND OPRT.AMOUNT=TMP.AMOUNT
													AND OPRT.SALES=TMP.SALES
												) AND TMP.SESSION_CD=@SESSION_CD
                                        DELETE FROM  TMP_PRIZE_ROTATE_TIMES WHERE SESSION_CD = @SESSION_CD";

                        L5sSql.Query(query, obj, guid);
                        return "1";
                    }
                    catch (Exception ex)
                    {
                        return "-1";
                    }
                }
            }
            catch (Exception EX)
            {
                return "-1";
            }
            return "1";
        }

        // Hàm get số ngày mà khách hàng đã quay
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetCurrentDay(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGetCurrent(obj);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGetCurrent(String obj)
        {
            String sql = @"DECLARE @PROGRAM_PRIZE_CD BIGINT
                            DECLARE @SALES_CODE NVARCHAR(50)
                            DECLARE @SALES_CD BIGINT
							DECLARE @TBRESULT TABLE
							(
							PROGRAM_PRIZE_CD BIGINT,
							CUSTOMER_CODE NVARCHAR(50),
							TOTAL BIGINT
							)
                            SET @SALES_CODE=@0
                            SELECT @SALES_CD=SALES_CD FROM M_SALES WHERE SALES_CODE=@SALES_CODE
							DECLARE db_cursor CURSOR FOR 
                            SELECT programprize.PROGRAM_PRIZE_CD FROM O_PROGRAM_PRIZE programprize WITH(NOLOCK, READUNCOMMITTED)
                            WHERE programprize.ACTIVE=1 
                            AND programprize.CREATEDPRIZE=1
                            AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)
                            AND EXISTS ( SELECT 1 FROM  O_PROGRAM_DISTRIBUTOR programdistributor WITH(NOLOCK, READUNCOMMITTED) 
		                            INNER JOIN M_SALES sales WITH(NOLOCK, READUNCOMMITTED) ON programdistributor.DISTRIBUTOR_CD=sales.DISTRIBUTOR_CD AND sales.SALES_CD=@SALES_CD
		                            WHERE programdistributor.ACTIVE=1 AND programprize.PROGRAM_PRIZE_CD=programdistributor.PROGRAM_PRIZE_CD)
                            OPEN db_cursor   
							FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD   

									WHILE @@FETCH_STATUS = 0   
									BEGIN   
									DECLARE @TABLEDAYMAX TABLE
									(
									CUSTOMER_CODE NVARCHAR(50),
									[DATE] DATE
									)
									INSERT INTO @TABLEDAYMAX(CUSTOMER_CODE,[DATE])						
									SELECT DISTINCT  TMP.CUSTOMER_CODE,CONVERT(DATE,TMP.PRIZE_DATE ,103) AS DATE
									FROM O_PRIZE_CUST TMP WITH(NOLOCK, READUNCOMMITTED)
									INNER JOIN O_ROTATE rotate  WITH(NOLOCK, READUNCOMMITTED) ON TMP.ROTATE_CD=rotate.ROTATE_CD AND rotate.ACTIVE=1
									INNER JOIN O_PRIZE OP ON rotate.PRIZE_CD=OP.PRIZE_CD AND OP.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD AND OP.ACTIVE=1
									WHERE [STATUS]=2 AND DSR_CD=@SALES_CD AND TMP.ACTIVE=1
									INSERT INTO @TBRESULT
									SELECT @PROGRAM_PRIZE_CD, CUSTOMER_CODE,ISNULL(COUNT(*),0) AS TOTAL
									FROM @TABLEDAYMAX
									GROUP BY CUSTOMER_CODE
							 FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD   
							END
							CLOSE db_cursor   
							DEALLOCATE db_cursor
							SELECT * FROM @TBRESULT
                            ";
            try
            {
                DataTable dt = P5sSql.Query(sql, obj);
                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }
        }
        // Hàm get giải thưởng để quay thử
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetPrizeTrail(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGetPrizeTrailData(obj);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGetPrizeTrailData(String obj)
        {
            String sql = @"DECLARE @SALE_CODE NVARCHAR(50)
                            DECLARE @SALE_CD BIGINT
                            DECLARE @PROGRAM_PRIZE_CD BIGINT
							DECLARE @TBRESULT TABLE
							(
							PROGRAM_PRIZE_CD BIGINT,
							PRIZE_CD BIGINT,
							PRIZE_DEGREES INT,
							PRIZE_CODE NVARCHAR(50),
							PRIZE_DESC NVARCHAR(500),
							CUSTOMER_CODE NVARCHAR(50),
							STATUS INT,
							PRIZE_SALES DECIMAL(18,0) 
							)
                            SET @SALE_CODE=@0
                            -- LẤY THÔNG TIN SALE_CD VÀ REGION_CD ỨNG VỚI SALE
                            SELECT TOP 1  @SALE_CD=sales.SALES_CD FROM M_SALES sales WITH(NOLOCK, READUNCOMMITTED)
                            WHERE sales.SALES_CODE=@SALE_CODE
                            -- LẤY CD CỦA CHƯƠNG TRÌNH QUAY THƯỞNG
							DECLARE db_cursor CURSOR FOR 
                            SELECT programprize.PROGRAM_PRIZE_CD FROM O_PROGRAM_PRIZE programprize WITH(NOLOCK, READUNCOMMITTED)
                            WHERE programprize.ACTIVE=1 
                            AND programprize.CREATEDPRIZE=1
                            AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)
                            AND EXISTS ( SELECT 1 FROM  O_PROGRAM_DISTRIBUTOR programdistributor WITH(NOLOCK, READUNCOMMITTED) 
			                            INNER JOIN M_SALES sales WITH(NOLOCK, READUNCOMMITTED) ON programdistributor.DISTRIBUTOR_CD=sales.DISTRIBUTOR_CD AND sales.SALES_CD=@SALE_CD
			                            WHERE programdistributor.ACTIVE=1 AND programprize.PROGRAM_PRIZE_CD=programdistributor.PROGRAM_PRIZE_CD)
							OPEN db_cursor   
							FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD   

							WHILE @@FETCH_STATUS = 0   
							BEGIN  
										INSERT INTO @TBRESULT 
										SELECT @PROGRAM_PRIZE_CD, PRIZE_CD,PRIZE_DEGREES,PRIZE_CODE,PRIZE_DESC,CUSTOMER_CODE,[STATUS],PRIZE_SALES FROM 
										(
											SELECT tmpprizecust.PRIZE_CUST_CD as PRIZE_CD ,tmpprizecust.PRIZE_DEGREES as PRIZE_DEGREES,prize.PRIZE_CODE as PRIZE_CODE ,prize.PRIZE_DES  as PRIZE_DESC, tmpprizecust.CUSTOMER_CODE,tmpprizecust.[STATUS], ISNULL( tmpprizecust.SALES,0) as PRIZE_SALES,
											RN = ROW_NUMBER()OVER(PARTITION BY PRIZE_DEGREES ORDER BY PRIZE_DEGREES)
											FROM O_PRIZE_CUST tmpprizecust WITH(NOLOCK, READUNCOMMITTED) 
											INNER JOIN O_ROTATE rotate WITH(NOLOCK, READUNCOMMITTED) ON rotate.ROTATE_CD = tmpprizecust.ROTATE_CD AND rotate.ACTIVE=1
											INNER JOIN O_PRIZE prize WITH(NOLOCK, READUNCOMMITTED) ON rotate.PRIZE_CD=prize.PRIZE_CD AND prize.ACTIVE=1 AND prize.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
											WHERE  tmpprizecust.ACTIVE=1
										) TMP WHERE TMP.RN=1
							FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD   
							END
							CLOSE db_cursor   
							DEALLOCATE db_cursor
							SELECT * FROM @TBRESULT";
            try
            {
                DataTable dt = P5sSql.Query(sql, obj);
                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }
        }
        // Hàm trả về danh sách khách hàng bị block khong cho quay giải
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetCustomerBlock(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGetCustBlock(obj);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGetCustBlock(String obj)
        {
            String sql = @"DECLARE @SALE_CODE NVARCHAR(50)
                            DECLARE @SALE_CD BIGINT
                            DECLARE @PROGRAM_PRIZE_CD BIGINT
							DECLARE @TBRESULT TABLE
							(
							PROGRAM_PRIZE_CD BIGINT,
							CUSTOMER_CODE NVARCHAR(50),
							BLOCK_DATE NVARCHAR(120),
							STATUS INT
							)
                            SET @SALE_CODE=@0
                            -- LẤY THÔNG TIN SALE_CD VÀ REGION_CD ỨNG VỚI SALE
                            SELECT TOP 1  @SALE_CD=sales.SALES_CD FROM M_SALES sales WITH(NOLOCK, READUNCOMMITTED)
                            WHERE sales.SALES_CODE=@SALE_CODE
                            -- LẤY CD CỦA CHƯƠNG TRÌNH QUAY THƯỞNG
							DECLARE db_cursor CURSOR FOR
                            SELECT programprize.PROGRAM_PRIZE_CD FROM O_PROGRAM_PRIZE programprize WITH(NOLOCK, READUNCOMMITTED)
                            WHERE programprize.ACTIVE=1 
                            AND programprize.CREATEDPRIZE=1
                            AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)
                            AND EXISTS ( SELECT 1 FROM  O_PROGRAM_DISTRIBUTOR programdistributor WITH(NOLOCK, READUNCOMMITTED) 
			                            INNER JOIN M_SALES sales WITH(NOLOCK, READUNCOMMITTED) ON programdistributor.DISTRIBUTOR_CD=sales.DISTRIBUTOR_CD AND sales.SALES_CD=@SALE_CD
			                            WHERE programdistributor.ACTIVE=1 AND programprize.PROGRAM_PRIZE_CD=programdistributor.PROGRAM_PRIZE_CD)
										OPEN db_cursor   
							FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD
							WHILE @@FETCH_STATUS = 0   
							BEGIN 
							INSERT INTO @TBRESULT
                            SELECT @PROGRAM_PRIZE_CD, CUSTOMER_CODE , CONVERT(NVARCHAR,BLOCK_DATE,120) BLOCK_DATE, '2' AS [STATUS] FROM O_PROGRAM_PRIZE_CUST_BLOCK WHERE ACTIVE=1 AND SALES_CD=@SALE_CD AND PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
							FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD   
							END   
							CLOSE db_cursor   
							DEALLOCATE db_cursor
							SELECT * FROM @TBRESULT";
            try
            {
                DataTable dt = P5sSql.Query(sql, obj);
                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }
        }
        // Hàm save Khách hàng bị block 
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCSaveCustomerBlock(String imei, String obj, String type, String jsonData)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCSaveCustblock(obj, jsonData);
                    case "2": // Sup
                        return "1";
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCSaveCustblock(String obj, String jsonData)
        {

            DataTable dtSales = L5sSql.Query("SELECT  SALES_CD FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";

            String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
            try
            {
                String guid = obj.Trim() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                //add new column
                DataColumn columnGUID = new DataColumn("SESSION_CD");
                columnGUID.DataType = typeof(String);
                columnGUID.DefaultValue = guid;

                DataColumn columnsalescode = new DataColumn("SALES_CODE");
                columnsalescode.DataType = typeof(String);
                columnsalescode.DefaultValue = obj;

                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(int);
                columnSalesCD.DefaultValue = salesCD;

                dt.Columns.Add(columnGUID);
                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnsalescode);

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TMP_PROGRAM_PRIZE_CUST_BLOCK";
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Block_Customer_Code", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "SALES_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Block_Date", "BLOCK_DATE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("BlockProgramPrizeCD", "PROGRAM_PRIZE_CD"));

                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        String query = @"DECLARE @SALE_CODE NVARCHAR(50)
                                        DECLARE @SESSION_CD NVARCHAR(50)
                                        DECLARE @SALE_CD BIGINT
                                        DECLARE @PROGRAM_PRIZE_CD BIGINT
                                        SET @SALE_CODE=@0
                                        SET @SESSION_CD=@1
                                        -- LẤY THÔNG TIN SALE_CD VÀ REGION_CD ỨNG VỚI SALE
                                        SELECT TOP 1  @SALE_CD=sales.SALES_CD FROM M_SALES sales WITH(NOLOCK, READUNCOMMITTED)
                                        WHERE sales.SALES_CODE=@SALE_CODE
                                        -- LẤY CD CỦA CHƯƠNG TRÌNH QUAY THƯỞNG
										DECLARE db_cursor CURSOR FOR
                                        SELECT programprize.PROGRAM_PRIZE_CD FROM O_PROGRAM_PRIZE programprize WITH(NOLOCK, READUNCOMMITTED)
                                        WHERE programprize.ACTIVE=1 
                                        AND programprize.CREATEDPRIZE=1
                                        AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)
                                        AND EXISTS ( SELECT programdistributor.PROGRAM_DISTRIBUTOR_CD FROM  O_PROGRAM_DISTRIBUTOR programdistributor WITH(NOLOCK, READUNCOMMITTED) 
			                                        INNER JOIN M_SALES  sales WITH(NOLOCK, READUNCOMMITTED) ON programdistributor.DISTRIBUTOR_CD=sales.DISTRIBUTOR_CD AND sales.SALES_CD=@SALE_CD
			                                        WHERE programdistributor.ACTIVE=1 AND programprize.PROGRAM_PRIZE_CD=programdistributor.PROGRAM_PRIZE_CD)
														OPEN db_cursor   
										FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD
										WHILE @@FETCH_STATUS = 0   
										BEGIN 
                                        -- REMOVE DUPLICATE
                                        DELETE FROM TMP_PROGRAM_PRIZE_CUST_BLOCK WHERE SESSION_CD=@SESSION_CD AND CUST_BLOCK_CD IN (
	                                        SELECT CUST_BLOCK_CD FROM (
		                                        SELECT TMP.CUST_BLOCK_CD,TMP.CUSTOMER_CODE,
		                                        RN = ROW_NUMBER()OVER(PARTITION BY TMP.CUSTOMER_CODE,TMP.SALES_CODE ORDER BY TMP.CUST_BLOCK_CD)
		                                        FROM TMP_PROGRAM_PRIZE_CUST_BLOCK TMP
		                                        WHERE TMP.SESSION_CD=@SESSION_CD AND PROGRAM_PRIZE_CD = @PROGRAM_PRIZE_CD
                                           ) TMP WHERE TMP.RN>1
                                        )
                                        -- UPDATE CUSTOMER_CD TRONG BANG TMP
                                        UPDATE  TMP SET TMP.CUSTOMER_CD =MC.CUSTOMER_CD
                                        FROM TMP_PROGRAM_PRIZE_CUST_BLOCK TMP
                                        INNER JOIN M_CUSTOMER MC WITH(NOLOCK, READUNCOMMITTED) ON TMP.CUSTOMER_CODE=MC.CUSTOMER_CODE
                                        WHERE TMP.SESSION_CD=@SESSION_CD AND TMP.SALES_CD=@SALE_CD AND TMP.PROGRAM_PRIZE_CD = @PROGRAM_PRIZE_CD

                                        --NẾU CHƯA TỒN TẠI THÌ THÊM MỚI 
                                        INSERT INTO O_PROGRAM_PRIZE_CUST_BLOCK(CUSTOMER_CODE,CUSTOMER_CD,SALES_CD,SALES_CODE,PROGRAM_PRIZE_CD,BLOCK_DATE,ACTIVE,CREATED_DATE)
                                        SELECT TMP.CUSTOMER_CODE,TMP.CUSTOMER_CD,TMP.SALES_CD,TMP.SALES_CODE,@PROGRAM_PRIZE_CD,BLOCK_DATE,1,GETDATE()
                                        FROM TMP_PROGRAM_PRIZE_CUST_BLOCK TMP WITH(NOLOCK, READUNCOMMITTED)
                                        WHERE NOT EXISTS (
	                                        SELECT 1  FROM O_PROGRAM_PRIZE_CUST_BLOCK OPRT  WITH(NOLOCK, READUNCOMMITTED)
	                                        WHERE OPRT.ACTIVE=1 AND 
	                                        OPRT.SALES_CD=TMP.SALES_CD AND
	                                        OPRT.CUSTOMER_CD=TMP.CUSTOMER_CD AND
	                                        OPRT.PROGRAM_PRIZE_CD=@PROGRAM_PRIZE_CD
                                        ) AND TMP.SESSION_CD=@SESSION_CD AND TMP.PROGRAM_PRIZE_CD = @PROGRAM_PRIZE_CD									
										FETCH NEXT FROM db_cursor INTO @PROGRAM_PRIZE_CD   
										END   
										CLOSE db_cursor   
										DEALLOCATE db_cursor
										DELETE FROM TMP_PROGRAM_PRIZE_CUST_BLOCK WHERE SESSION_CD = @SESSION_CD OR CONVERT(DATE,CREATED_DATE+2,103)<CONVERT(DATE,GETDATE(),103)";

                        L5sSql.Query(query, obj, guid);
                        return "1";
                    }
                    catch (Exception ex)
                    {
                        return "-1";
                    }
                }
            }
            catch (Exception EX)
            {
                return "-1";
            }
            return "1";
        }
        #endregion
        #region synchronize Photo V2 Hàm lưu hình ảnh được gửi từ HH lên Server theo TradeProgram


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetTradeProgram(String imei, String obj, String type)
        {
            try
            {
                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                DataTable dt = new DataTable();

                switch (type.Trim())
                {
                    case "1":
                        dt = L5sSql.Query(@"SELECT OTP.TRADE_PROGRAM_CD,
	                                                 OTP.TRADE_PROGRAM_CODE,
	                                                 OTP.TRADE_PROGRAM_NAME,
	                                                 CONVERT(VARCHAR(10),OTP.[START_DATE],105) AS START_DATE,
	                                                 CONVERT(VARCHAR(10),OTP.END_DATE,105) AS END_DATE 
                                              FROM O_TRADE_PROGRAM OTP
                                              WHERE CONVERT(DATE, GETDATE()) BETWEEN START_DATE AND END_DATE
	                                                 AND OTP.ACTIVE = 1 AND OTP.[TYPE]=1  AND EXISTS (SELECT TRADE_PROGRAM_CD FROM O_TRADE_PROGRAM_USER OTPU
																				INNER JOIN M_SALES MS ON OTPU.USER_CD=MS.SALES_CD
																				WHERE OTPU.ACTIVE=1 AND OTPU.[TYPE]=OTP.[TYPE] AND MS.SALES_CODE LIKE @0 AND  OTP.TRADE_PROGRAM_CD=OTPU.TRADE_PROGRAM_CD)
                                            ", obj);
                        break;
                    case "2":
                        dt = L5sSql.Query(@"SELECT OTP.TRADE_PROGRAM_CD,
	                                                 OTP.TRADE_PROGRAM_CODE,
	                                                 OTP.TRADE_PROGRAM_NAME,
	                                                 CONVERT(VARCHAR(10),OTP.[START_DATE],105) AS START_DATE,
	                                                 CONVERT(VARCHAR(10),OTP.END_DATE,105) AS END_DATE 
                                              FROM O_TRADE_PROGRAM OTP
                                              WHERE CONVERT(DATE, GETDATE()) BETWEEN START_DATE AND END_DATE
	                                                 AND OTP.ACTIVE = 1 AND OTP.[TYPE]=2  AND EXISTS (SELECT TRADE_PROGRAM_CD FROM O_TRADE_PROGRAM_USER OTPU
																				INNER JOIN M_SUPERVISOR MS ON OTPU.USER_CD=MS.SUPERVISOR_CD
																				WHERE OTPU.ACTIVE=1 AND OTPU.[TYPE]=OTP.[TYPE] AND MS.SUPERVISOR_CODE LIKE @0 AND  OTP.TRADE_PROGRAM_CD=OTPU.TRADE_PROGRAM_CD)


																				", obj);
                        break;
                    case "3":
                        dt = L5sSql.Query(@"SELECT OTP.TRADE_PROGRAM_CD,
	                                                 OTP.TRADE_PROGRAM_CODE,
	                                                 OTP.TRADE_PROGRAM_NAME,
	                                                 CONVERT(VARCHAR(10),OTP.[START_DATE],105) AS START_DATE,
	                                                 CONVERT(VARCHAR(10),OTP.END_DATE,105) AS END_DATE 
                                              FROM O_TRADE_PROGRAM OTP
                                              WHERE CONVERT(DATE, GETDATE()) BETWEEN START_DATE AND END_DATE
	                                                 AND OTP.ACTIVE = 1 AND OTP.[TYPE]=3  AND EXISTS (SELECT TRADE_PROGRAM_CD FROM O_TRADE_PROGRAM_USER OTPU
																				INNER JOIN M_ASM MS ON OTPU.USER_CD=MS.ASM_CD
																				WHERE OTPU.ACTIVE=1 AND OTPU.[TYPE]=OTP.[TYPE] AND MS.ASM_CODE LIKE @0 AND  OTP.TRADE_PROGRAM_CD=OTPU.TRADE_PROGRAM_CD)


																				", obj);
                        break;

                }

                if (dt == null || dt.Rows.Count == 0)
                    return "1";

                return this.POCGetJSONString(dt);

            }
            catch (Exception)
            {
                return "-1";
            }
        }



        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCPhotoV2(String imei, String obj, String type, String jsonData, byte[] image)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                if (jsonData.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCPhotoForSalesV2(obj, type, jsonData, image);
                    case "2": // Sup
                        return this.synchronizePOCPhotoForSupV2(obj, type, jsonData, image);
                    case "3": //ASM
                        return this.synchronizePOCPhotoForASMV2(obj, type, jsonData, image);
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }


        }

        private String synchronizePOCPhotoForSalesV2(String obj, String type, String jsonData, byte[] image)
        {
            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";

            String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
            String typeCD = type;
            try
            {

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                if (!dt.Columns.Contains("photoNote2")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote2 = new DataColumn("photoNote2");
                    photoNote2.DataType = typeof(String);
                    dt.Columns.Add(photoNote2);
                }

                if (!dt.Columns.Contains("photoNote3")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote3 = new DataColumn("photoNote3");
                    photoNote3.DataType = typeof(String);
                    dt.Columns.Add(photoNote3);
                }

                if (!dt.Columns.Contains("photoNote4")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote4 = new DataColumn("photoNote4");
                    photoNote4.DataType = typeof(String);
                    dt.Columns.Add(photoNote4);
                }

                if (!dt.Columns.Contains("photoNote5")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote5 = new DataColumn("photoNote5");
                    photoNote5.DataType = typeof(String);
                    dt.Columns.Add(photoNote5);
                }


                if (!this.convertPOCByteImageToFileV2(image, dt.Rows[0]["photoName"].ToString()))
                    return "-1";




                L5sSql.Query(@"

                                --tìm KH đã tồn tại trong bảng  O_TRADE_PROGRAM_CUSTOMER
                                DECLARE @TRADE_PROGRAM_CUSTOMER_CD BIGINT
                                SET @TRADE_PROGRAM_CUSTOMER_CD = -1

                                DECLARE @TRADE_PROGRAM_CD BIGINT
                                DECLARE @CUSTOMER_CODE NVARCHAR(128)
                                SET @TRADE_PROGRAM_CD = @0
                                SET @CUSTOMER_CODE = @1

                                DECLARE @CUSTOMER_CD BIGINT
                                SET @CUSTOMER_CD = -1

                                SELECT @CUSTOMER_CD = CUSTOMER_CD 
                                FROM M_CUSTOMER WHERE CUSTOMER_CODE = @CUSTOMER_CODE

                                SELECT @TRADE_PROGRAM_CUSTOMER_CD = TRADE_PROGRAM_CUSTOMER_CD
                                FROM O_TRADE_PROGRAM_CUSTOMER
                                WHERE TRADE_PROGRAM_CD = @TRADE_PROGRAM_CD AND CUSTOMER_CD = @CUSTOMER_CD

                                IF @CUSTOMER_CD  = -1 --Không tìm thấy KH exit
                                BEGIN
                                     RETURN
                                END
                                
                                IF @TRADE_PROGRAM_CUSTOMER_CD  = -1
                                BEGIN
                                        INSERT INTO [dbo].[O_TRADE_PROGRAM_CUSTOMER]
                                                 ([CUSTOMER_CD],[CUSTOMER_CODE],[TRADE_PROGRAM_CD],[NUMBER_PHOTO])
                                                VALUES (@CUSTOMER_CD,@CUSTOMER_CODE,@TRADE_PROGRAM_CD,1)
                                     SELECT  TOP 1 @TRADE_PROGRAM_CUSTOMER_CD = SCOPE_IDENTITY() 

                                END      
                                ELSE
                                BEGIN
                                        UPDATE [O_TRADE_PROGRAM_CUSTOMER] SET [NUMBER_PHOTO] = [NUMBER_PHOTO] + 1
                                        WHERE TRADE_PROGRAM_CUSTOMER_CD = @TRADE_PROGRAM_CUSTOMER_CD                                 
                                END

                                IF @TRADE_PROGRAM_CUSTOMER_CD  = -1 -- vẫn không tìm được TradeProgramCD
                                BEGIN
                                    RETURN          
                                END
                                


                                INSERT INTO [dbo].[O_TRADE_PROGRAM_PHOTO]
                                           ([TRADE_PROGRAM_CUSTOMER_CD]
                                           ,[PHOTO_NAME]
                                           ,[PHOTO_PATH]                                         
                                           ,[PHOTO_NOTES_1]
                                           ,[PHOTO_NOTES_2]
                                           ,[PHOTO_NOTES_3]
                                           ,[PHOTO_NOTES_4]
                                           ,[PHOTO_NOTES_5]  
                                           ,[PHOTO_CREATED_DATE]
                                           ,[PHOTO_LATITUDE_LONGITUDE]
                                           ,[PHOTO_LATITUDE_LONGITUDE_ACCURACY]
                                           ,[SALES_CD]
                                           ,[TYPE_CD])
                              VALUES (@TRADE_PROGRAM_CUSTOMER_CD,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13)   
                                   
                                    ",
                                          dt.Rows[0]["photoTradeProgramCD"].ToString()
                                        , dt.Rows[0]["photoCustomerCode"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoNote"].ToString()
                                        , dt.Rows[0]["photoNote2"].ToString()
                                        , dt.Rows[0]["photoNote3"].ToString()
                                        , dt.Rows[0]["photoNote4"].ToString()
                                        , dt.Rows[0]["photoNote5"].ToString()
                                        , dt.Rows[0]["dateTakesPhoto"].ToString()
                                        , dt.Rows[0]["geoCode"].ToString()
                                        , dt.Rows[0]["geoCodeAccuracy"].ToString()
                                        , salesCD
                                        , typeCD
                                      );

            }
            catch (Exception EX)
            {
                return EX.Message;
            }


            return "1";

        }
        private string synchronizePOCPhotoForASMV2(String obj, String type, String jsonData, byte[] image)
        {

            DataTable dtASM = L5sSql.Query("SELECT  * FROM M_ASM WHERE ASM_CODE = @0", obj);
            if (dtASM.Rows.Count <= 0)
                return "-1";

            String asm = dtASM.Rows[0]["ASM_CD"].ToString();
            String typeCD = type;

            try
            {

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                if (!dt.Columns.Contains("photoNote2")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote2 = new DataColumn("photoNote2");
                    photoNote2.DataType = typeof(String);
                    dt.Columns.Add(photoNote2);
                }

                if (!dt.Columns.Contains("photoNote3")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote3 = new DataColumn("photoNote3");
                    photoNote3.DataType = typeof(String);
                    dt.Columns.Add(photoNote3);
                }

                if (!dt.Columns.Contains("photoNote4")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote4 = new DataColumn("photoNote4");
                    photoNote4.DataType = typeof(String);
                    dt.Columns.Add(photoNote4);
                }

                if (!dt.Columns.Contains("photoNote5")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote5 = new DataColumn("photoNote5");
                    photoNote5.DataType = typeof(String);
                    dt.Columns.Add(photoNote5);
                }


                if (!this.convertPOCByteImageToFileV2(image, dt.Rows[0]["photoName"].ToString()))
                    return "-1";


                L5sSql.Query(@"

                                --tìm KH đã tồn tại trong bảng  O_TRADE_PROGRAM_CUSTOMER
                                DECLARE @TRADE_PROGRAM_CUSTOMER_CD BIGINT
                                SET @TRADE_PROGRAM_CUSTOMER_CD = -1

                                DECLARE @TRADE_PROGRAM_CD BIGINT
                                DECLARE @CUSTOMER_CODE NVARCHAR(128)
                                SET @TRADE_PROGRAM_CD = @0
                                SET @CUSTOMER_CODE = @1

                                DECLARE @CUSTOMER_CD BIGINT
                                SET @CUSTOMER_CD = -1

                                SELECT @CUSTOMER_CD = CUSTOMER_CD 
                                FROM M_CUSTOMER WHERE CUSTOMER_CODE = @CUSTOMER_CODE

                                SELECT @TRADE_PROGRAM_CUSTOMER_CD = TRADE_PROGRAM_CUSTOMER_CD
                                FROM O_TRADE_PROGRAM_CUSTOMER
                                WHERE TRADE_PROGRAM_CD = @TRADE_PROGRAM_CD AND CUSTOMER_CD = @CUSTOMER_CD

                                IF @CUSTOMER_CD  = -1 --Không tìm thấy KH exit
                                BEGIN
                                     RETURN
                                END
                                
                                IF @TRADE_PROGRAM_CUSTOMER_CD  = -1
                                BEGIN
                                        INSERT INTO [dbo].[O_TRADE_PROGRAM_CUSTOMER]
                                                 ([CUSTOMER_CD],[CUSTOMER_CODE],[TRADE_PROGRAM_CD],[NUMBER_PHOTO])
                                                VALUES (@CUSTOMER_CD,@CUSTOMER_CODE,@TRADE_PROGRAM_CD,1)
                                     SELECT  TOP 1 @TRADE_PROGRAM_CUSTOMER_CD = SCOPE_IDENTITY() 

                                END         
                                ELSE
                                BEGIN
                                        UPDATE [O_TRADE_PROGRAM_CUSTOMER] SET [NUMBER_PHOTO] = [NUMBER_PHOTO] + 1
                                        WHERE TRADE_PROGRAM_CUSTOMER_CD = @TRADE_PROGRAM_CUSTOMER_CD                                 
                                END

                                IF @TRADE_PROGRAM_CUSTOMER_CD  = -1 -- vẫn không tìm được TradeProgramCD
                                BEGIN
                                    RETURN          
                                END
                                


                                INSERT INTO [dbo].[O_TRADE_PROGRAM_PHOTO]
                                           ([TRADE_PROGRAM_CUSTOMER_CD]
                                           ,[PHOTO_NAME]
                                           ,[PHOTO_PATH]                                         
                                           ,[PHOTO_NOTES_1]
                                           ,[PHOTO_NOTES_2]
                                           ,[PHOTO_NOTES_3]
                                           ,[PHOTO_NOTES_4]
                                           ,[PHOTO_NOTES_5]  
                                           ,[PHOTO_CREATED_DATE]
                                           ,[PHOTO_LATITUDE_LONGITUDE]
                                           ,[PHOTO_LATITUDE_LONGITUDE_ACCURACY]
                                           ,[SALES_CD]
                                           ,[TYPE_CD])
                              VALUES (@TRADE_PROGRAM_CUSTOMER_CD,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13)   
                                   
                                    ",
                                          dt.Rows[0]["photoTradeProgramCD"].ToString()
                                        , dt.Rows[0]["photoCustomerCode"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoNote"].ToString()
                                        , dt.Rows[0]["photoNote2"].ToString()
                                        , dt.Rows[0]["photoNote3"].ToString()
                                        , dt.Rows[0]["photoNote4"].ToString()
                                        , dt.Rows[0]["photoNote5"].ToString()
                                        , dt.Rows[0]["dateTakesPhoto"].ToString()
                                        , dt.Rows[0]["geoCode"].ToString()
                                        , dt.Rows[0]["geoCodeAccuracy"].ToString()
                                        , asm
                                        , typeCD
                                      );


            }
            catch (Exception EX)
            {
                return "-1";
            }


            return "1";
        }
        private string synchronizePOCPhotoForSupV2(String obj, String type, String jsonData, byte[] image)
        {

            DataTable dtSup = L5sSql.Query("SELECT  * FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0", obj);
            if (dtSup.Rows.Count <= 0)
                return "-1";

            String supCD = dtSup.Rows[0]["SUPERVISOR_CD"].ToString();
            String typeCD = type;

            try
            {

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                if (!dt.Columns.Contains("photoNote2")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote2 = new DataColumn("photoNote2");
                    photoNote2.DataType = typeof(String);
                    dt.Columns.Add(photoNote2);
                }

                if (!dt.Columns.Contains("photoNote3")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote3 = new DataColumn("photoNote3");
                    photoNote3.DataType = typeof(String);
                    dt.Columns.Add(photoNote3);
                }

                if (!dt.Columns.Contains("photoNote4")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote4 = new DataColumn("photoNote4");
                    photoNote4.DataType = typeof(String);
                    dt.Columns.Add(photoNote4);
                }

                if (!dt.Columns.Contains("photoNote5")) //version 220 cũ không có thông tin photoNote2
                {
                    DataColumn photoNote5 = new DataColumn("photoNote5");
                    photoNote5.DataType = typeof(String);
                    dt.Columns.Add(photoNote5);
                }

                if (!this.convertPOCByteImageToFileV2(image, dt.Rows[0]["photoName"].ToString()))
                    return "-1";

                L5sSql.Query(@"

                                --tìm KH đã tồn tại trong bảng  O_TRADE_PROGRAM_CUSTOMER
                                DECLARE @TRADE_PROGRAM_CUSTOMER_CD BIGINT
                                SET @TRADE_PROGRAM_CUSTOMER_CD = -1

                                DECLARE @TRADE_PROGRAM_CD BIGINT
                                DECLARE @CUSTOMER_CODE NVARCHAR(128)
                                SET @TRADE_PROGRAM_CD = @0
                                SET @CUSTOMER_CODE = @1

                                DECLARE @CUSTOMER_CD BIGINT
                                SET @CUSTOMER_CD = -1

                                SELECT @CUSTOMER_CD = CUSTOMER_CD 
                                FROM M_CUSTOMER WHERE CUSTOMER_CODE = @CUSTOMER_CODE

                                SELECT @TRADE_PROGRAM_CUSTOMER_CD = TRADE_PROGRAM_CUSTOMER_CD
                                FROM O_TRADE_PROGRAM_CUSTOMER
                                WHERE TRADE_PROGRAM_CD = @TRADE_PROGRAM_CD AND CUSTOMER_CD = @CUSTOMER_CD

                                IF @CUSTOMER_CD  = -1 --Không tìm thấy KH exit
                                BEGIN
                                     RETURN
                                END
                                
                                 IF @TRADE_PROGRAM_CUSTOMER_CD  = -1
                                BEGIN
                                        INSERT INTO [dbo].[O_TRADE_PROGRAM_CUSTOMER]
                                                 ([CUSTOMER_CD],[CUSTOMER_CODE],[TRADE_PROGRAM_CD],[NUMBER_PHOTO])
                                                VALUES (@CUSTOMER_CD,@CUSTOMER_CODE,@TRADE_PROGRAM_CD,1)
                                       SELECT  TOP 1 @TRADE_PROGRAM_CUSTOMER_CD = SCOPE_IDENTITY() 

                                END
                                ELSE
                                BEGIN
                                        UPDATE [O_TRADE_PROGRAM_CUSTOMER] SET [NUMBER_PHOTO] = [NUMBER_PHOTO] + 1
                                        WHERE TRADE_PROGRAM_CUSTOMER_CD = @TRADE_PROGRAM_CUSTOMER_CD                                 
                                END


                                IF @TRADE_PROGRAM_CUSTOMER_CD  = -1 -- vẫn không tìm được TradeProgramCD
                                BEGIN
                                    RETURN          
                                END
                                
                                


                                INSERT INTO [dbo].[O_TRADE_PROGRAM_PHOTO]
                                           ([TRADE_PROGRAM_CUSTOMER_CD]
                                           ,[PHOTO_NAME]
                                           ,[PHOTO_PATH]                                         
                                           ,[PHOTO_NOTES_1]
                                           ,[PHOTO_NOTES_2]
                                           ,[PHOTO_NOTES_3]
                                           ,[PHOTO_NOTES_4]
                                           ,[PHOTO_NOTES_5]  
                                           ,[PHOTO_CREATED_DATE]
                                           ,[PHOTO_LATITUDE_LONGITUDE]
                                           ,[PHOTO_LATITUDE_LONGITUDE_ACCURACY]
                                           ,[SALES_CD]
                                           ,[TYPE_CD])
                              VALUES (@TRADE_PROGRAM_CUSTOMER_CD,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13)   
                                   
                                    ",
                                          dt.Rows[0]["photoTradeProgramCD"].ToString()
                                        , dt.Rows[0]["photoCustomerCode"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoName"].ToString()
                                        , dt.Rows[0]["photoNote"].ToString()
                                        , dt.Rows[0]["photoNote2"].ToString()
                                        , dt.Rows[0]["photoNote3"].ToString()
                                        , dt.Rows[0]["photoNote4"].ToString()
                                        , dt.Rows[0]["photoNote5"].ToString()
                                        , dt.Rows[0]["dateTakesPhoto"].ToString()
                                        , dt.Rows[0]["geoCode"].ToString()
                                        , dt.Rows[0]["geoCodeAccuracy"].ToString()
                                        , supCD
                                        , typeCD
                                      );

            }
            catch (Exception EX)
            {
                return "-1";
            }


            return "1";
        }

        #endregion

        #region Get và upload 5P-tracking distribution
        //Hàm lấy thông tin tracking 5P của khách hàng dựa theo route code
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGet5PDistributionTracking(String imei, String obj, String routeCode, String type)
        {
            try
            {
                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                DataTable dt = new DataTable();
                routeCode = "'" + routeCode.Replace(",", "','") + "'";
                //string[] routeCodeList = routeCode.Split(',');
                //routeCode = string.Join("','", routeCodeList);
                switch (type.Trim())
                {
                    case "1":
                        String sql = String.Format(@"SELECT track.* FROM O_POC_DISTRIBUTOR_TRACKING track 
									INNER JOIN M_CUSTOMER cust ON track.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                    INNER JOIN O_CUSTOMER_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD
                                    INNER JOIN M_ROUTE rout ON rout.ROUTE_CD = custR.ROUTE_CD AND rout.ROUTE_CODE IN ({0})
                                            ", routeCode);
                        dt = L5sSql.Query(sql);
                        if (dt == null || dt.Rows.Count == 0)
                            return "1";
                        break;
                    case "2":
                        String sqlcds = String.Format(@"SELECT track.* FROM O_POC_DISTRIBUTOR_TRACKING track 
                                            INNER JOIN M_CUSTOMER cust ON track.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                            INNER JOIN O_CUSTOMER_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD
                                            INNER JOIN M_ROUTE rout ON rout.ROUTE_CD = custR.ROUTE_CD 
                                            INNER JOIN O_SUPERVISOR_DISTRIBUTOR OSD ON  rout.DISTRIBUTOR_CD=OSD.DISTRIBUTOR_CD
                                            INNER JOIN M_SUPERVISOR SUP ON OSD.SUPERVISOR_CD= SUP.SUPERVISOR_CD 
                                            WHERE SUP.SUPERVISOR_CODE='{0}' ", obj);
                        dt = L5sSql.Query(sqlcds);
                        if (dt == null || dt.Rows.Count == 0)
                            return "1";
                        break;

                }
                return this.POCGetJSONString(dt);

            }
            catch (Exception)
            {
                return "-1";
            }
        }


        //Hàm lưu thông tin tracking 5P của khách hàng
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCSave5PDistributionTracking(String imei, String obj, String jsonData)
        {
            try
            {
                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                DataTable dtSales = L5sSql.Query(@"SELECT  SALES_CD FROM M_SALES WHERE SALES_CODE = @0
                                                UNION ALL
                                                SELECT SUPERVISOR_CD FROM M_SUPERVISOR WHERE SUPERVISOR_CODE= @0", obj);
                if (dtSales.Rows.Count <= 0)
                    return "-1";


                try
                {
                    String guid = obj.Trim() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                    //add new column

                    DataColumn sale_code = new DataColumn("SALES_CODE");
                    sale_code.DataType = typeof(String);
                    sale_code.DefaultValue = obj;
                    dt.Columns.Add(sale_code);

                    DataColumn columnGUID = new DataColumn("SESSION_CD");
                    columnGUID.DataType = typeof(String);
                    columnGUID.DefaultValue = guid;
                    dt.Columns.Add(columnGUID);

                    using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                    {
                        connection.Open();
                        SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                        bulkcopy.DestinationTableName = "TMP_DISTRIBUTOR_TRACKING";
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Customer_Code", "CUSTOMER_CODE"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Product_Code", "PRODUCT_CODE"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Product_Name", "PRODUCT_NAME"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Distribution", "DISTRIBUTION"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Item_1", "ITEM_1"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Item_2", "ITEM_2"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Item_3", "ITEM_3"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ProgramCD", "PROGRAM_TRACKING_CD"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "USER_CODE"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Product_Image", "PRODUCT_IMG"));

                        try
                        {
                            bulkcopy.WriteToServer(dt);
                            String query = @"--UPDATE CÁC THÔNG TIN SẢN PHẨM ĐÃ TỒN TẠI CỦA KHÁCH HÀNG
                                            UPDATE TRACK SET TRACK.DISTRIBUTION = TMP.DISTRIBUTION , TRACK.ITEM_1 = TMP.ITEM_1, 
                                            TRACK.ITEM_2 = TMP.ITEM_2, TRACK.ITEM_3 = TMP.ITEM_3, TRACK.UPDATED_DATE = GETDATE(),TRACK.USER_CODE= TMP.USER_CODE,TRACK.PRODUCT_IMG= TMP.PRODUCT_IMG
                                            FROM  O_POC_DISTRIBUTOR_TRACKING TRACK
                                            INNER JOIN TMP_DISTRIBUTOR_TRACKING TMP ON TMP.CUSTOMER_CODE = TRACK.CUSTOMER_CODE
                                            AND TMP.PRODUCT_CODE = TRACK.PRODUCT_CODE 
											AND TMP.PROGRAM_TRACKING_CD = TRACK.PROGRAM_TRACKING_CD 
											AND TMP.SESSION_CD = @0
                                            --XÓA CÁC DÒNG DỮ LIỆU ĐÃ CÓ TRONG BẢNG TẠM
                                            DELETE TRACK FROM TMP_DISTRIBUTOR_TRACKING TMP
                                            INNER JOIN O_POC_DISTRIBUTOR_TRACKING TRACK ON TMP.CUSTOMER_CODE = TRACK.CUSTOMER_CODE
                                            AND TMP.PRODUCT_CODE = TRACK.PRODUCT_CODE 
											AND TMP.PROGRAM_TRACKING_CD = TRACK.PROGRAM_TRACKING_CD
											AND TMP.SESSION_CD = @0
                                            --THÊM MỚI CÁC DÒNG DỮ LIỆU
                                            INSERT INTO O_POC_DISTRIBUTOR_TRACKING(CUSTOMER_CODE,PRODUCT_CODE,PRODUCT_NAME,DISTRIBUTION,ITEM_1,ITEM_2,ITEM_3,USER_CODE,PROGRAM_TRACKING_CD,PRODUCT_IMG)
                                            SELECT CUSTOMER_CODE,PRODUCT_CODE,PRODUCT_NAME,DISTRIBUTION,ITEM_1,ITEM_2,ITEM_3,USER_CODE,PROGRAM_TRACKING_CD,PRODUCT_IMG 
											FROM TMP_DISTRIBUTOR_TRACKING
                                            WHERE SESSION_CD = @0
                                            --XÓA BẢNG TẠM THEO SESSION_CD
                                            DELETE TMP_DISTRIBUTOR_TRACKING WHERE SESSION_CD = @0";

                            L5sSql.Query(query, guid);
                            return "1";
                        }
                        catch (Exception ex)
                        {
                            return "-1";
                        }
                    }
                }
                catch (Exception EX)
                {
                    return "-1";
                }
                return "1";
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        #endregion
        #region synchronize shelf tracking : hàm đồng bộ dữ liệu shelf tracking

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCTrackingShelf(String imei, String obj, String jsonData)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            //if (!this.isValidDevice(imei, false))
            //    return this.encrypt("-1");

            DataTable dtSales = L5sSql.Query(@"SELECT  SALES_CD FROM M_SALES WHERE SALES_CODE = @0
                                                UNION ALL
                                                SELECT SUPERVISOR_CD FROM M_SUPERVISOR WHERE SUPERVISOR_CODE= @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";






            if (obj.Trim().Length == 0)
                return "-1";


            if (jsonData.Trim().Length == 0)
                return "-1";

            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);


                DataColumn sale_code = new DataColumn("SALES_CODE");
                sale_code.DataType = typeof(String);
                sale_code.DefaultValue = obj;
                dt.Columns.Add(sale_code);

                String sessionCD = System.Guid.NewGuid().ToString();
                DataColumn colSessionCD = new DataColumn("SESSION_CD");
                colSessionCD.DataType = typeof(String);
                colSessionCD.DefaultValue = sessionCD;

                dt.Columns.Add(colSessionCD);

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "TMP_SHELF_TRACKING";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCode", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("code", "CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("brandName", "PROGRAM_TRACKING_BRAND_CATEGORY_NAME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("shelfLength", "SHELF_LENGTH"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("displayOrder", "DISPLAY_ORDER"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("isActive", "PRODUCT_ACTIVE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "USER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("programCD", "PROGRAM_TRACKING_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("type", "TYPE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("group", "PROGRAM_TRACKING_BRAND_CATEGORY_CD"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);

                        String sql = @" 
                                        --UPDATE CÁC THÔNG TIN SẢN PHẨM ĐÃ TỒN TẠI CỦA KHÁCH HÀNG
                                        UPDATE O_POC_SHELF_TRACKING 
                                        SET SHELF_LENGTH= TMP.SHELF_LENGTH,UPDATED_DATE= GETDATE(),USER_CODE= TMP.USER_CODE
                                        FROM TMP_SHELF_TRACKING TMP
                                        INNER JOIN O_POC_SHELF_TRACKING OST ON TMP.CUSTOMER_CODE= OST.CUSTOMER_CODE 
	                                    AND TMP.CD= OST.PROGRAM_TRACKING_BRAND_CATEGORY_CD
	                                    AND TMP.PROGRAM_TRACKING_CD= OST.PROGRAM_TRACKING_CD
                                        WHERE TMP.SESSION_CD='{0}' AND TMP.TYPE=0

	                                    UPDATE O_POC_SHELF_BRAND_TRACKING 
                                        SET SHELF_LENGTH= TMP.SHELF_LENGTH,UPDATE_DATE= GETDATE()
                                        FROM TMP_SHELF_TRACKING TMP
                                        INNER JOIN O_POC_SHELF_BRAND_TRACKING OST ON TMP.CUSTOMER_CODE= OST.CUSTOMER_CODE 
	                                    AND TMP.PROGRAM_TRACKING_CD= OST.PROGRAM_TRACKING_CD
	                                    AND TMP.CD= OST.PROGRAM_TRACKING_BRAND_CD
                                        WHERE TMP.SESSION_CD='{0}' AND TMP.TYPE=1

                                      --XÓA CÁC DÒNG DỮ LIỆU ĐÃ UPDATE
                                        DELETE TMP FROM TMP_SHELF_TRACKING TMP
                                        INNER JOIN O_POC_SHELF_TRACKING OST ON TMP.CUSTOMER_CODE = OST.CUSTOMER_CODE
                                        AND TMP.PROGRAM_TRACKING_CD = OST.PROGRAM_TRACKING_CD 
	                                    AND TMP.SESSION_CD = '{0}'
                                        --THÊM MỚI CÁC DÒNG DỮ LIỆU
                                          INSERT INTO O_POC_SHELF_TRACKING (CUSTOMER_CODE,DISPLAY_ORDER,SHELF_LENGTH,PRODUCT_ACTIVE,USER_CODE,PROGRAM_TRACKING_CD,PROGRAM_TRACKING_BRAND_CATEGORY_CD,PRODUCT_CODE)
                                        SELECT CUSTOMER_CODE,PRODUCT_NAME,SHELF_LENGTH,PRODUCT_ACTIVE,USER_CODE,PROGRAM_TRACKING_CD,CD,DISPLAY_ORDER
                                        FROM TMP_SHELF_TRACKING
                                        WHERE SESSION_CD='{0}' AND TYPE =0

	                                    INSERT INTO O_POC_SHELF_BRAND_TRACKING (CUSTOMER_CODE,SHELF_LENGTH,PROGRAM_TRACKING_CD,PROGRAM_TRACKING_BRAND_CD)
                                        SELECT CUSTOMER_CODE,SHELF_LENGTH,PROGRAM_TRACKING_CD,CD
                                        FROM TMP_SHELF_TRACKING
                                        WHERE SESSION_CD='{0}' AND TYPE =1

                                        --XÓA BẢNG TẠM THEO SESSION_CD
                                        DELETE TMP_SHELF_TRACKING WHERE SESSION_CD = '{0}'";
                        sql = String.Format(sql, sessionCD);
                        L5sSql.Execute(sql);

                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }


        }


        //get message
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string getPOCP5TrackingShareOfShelf(String imei, String obj, String routeCode, String type)
        {
            try
            {
                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";
                DataTable dt = new DataTable();
                routeCode = "'" + routeCode.Replace(",", "','") + "'";

                switch (type.Trim())
                {
                    case "1":
                        String sql = String.Format(@"SELECT OSBT.PROGRAM_TRACKING_BRAND_CD AS PRODUCT_CODE,OSBT.CUSTOMER_CODE,B.PROGRAM_TRACKING_BRAND_NAME AS PRODUCT_NAME,
                                               OSBT.SHELF_LENGTH,0 AS DISPLAY_ORDER,1 AS TYPE,B.PROGRAM_TRACKING_BRAND_CATEGORY_CD,OSBT.PROGRAM_TRACKING_CD
                                            FROM  O_POC_SHELF_BRAND_TRACKING OSBT
                                            INNER JOIN O_POC_PROGRAM_TRACKING_BRAND B ON OSBT.PROGRAM_TRACKING_BRAND_CD= B.PROGRAM_TRACKING_BRAND_CD
                                            INNER JOIN M_CUSTOMER MC ON OSBT.CUSTOMER_CODE=MC.CUSTOMER_CODE
                                            INNER JOIN O_CUSTOMER_ROUTE OCR ON OCR.CUSTOMER_CD= MC.CUSTOMER_CD
                                            INNER JOIN M_ROUTE MR ON OCR.ROUTE_CD = MR.ROUTE_CD
                                            WHERE MR.ROUTE_CODE IN ({0})

                                            UNION ALL
                                              SELECT OST.PROGRAM_TRACKING_BRAND_CATEGORY_CD AS PRODUCT_CODE,OST.CUSTOMER_CODE,C.PROGRAM_TRACKING_BRAND_CATEGORY_NAME AS PRODUCT_NAME,
                                              OST.SHELF_LENGTH,0 AS DISPLAY_ORDER,0 AS TYPE,C.PROGRAM_TRACKING_BRAND_CATEGORY_CD ,OST.PROGRAM_TRACKING_CD

                                            FROM O_POC_SHELF_TRACKING OST

                                            INNER JOIN O_POC_PROGRAM_TRACKING_BRAND_CATEGORY C ON OST.PROGRAM_TRACKING_BRAND_CATEGORY_CD=C.PROGRAM_TRACKING_BRAND_CATEGORY_CD
                                            INNER JOIN M_CUSTOMER MC ON OST.CUSTOMER_CODE=MC.CUSTOMER_CODE
                                            INNER JOIN O_CUSTOMER_ROUTE OCR ON OCR.CUSTOMER_CD= MC.CUSTOMER_CD
                                            INNER JOIN M_ROUTE MR ON OCR.ROUTE_CD = MR.ROUTE_CD
                                                                                WHERE MR.ROUTE_CODE IN ({0})
                                            
                                            ", routeCode);
                        dt = L5sSql.Query(sql);
                        if (dt == null || dt.Rows.Count == 0)
                            return "1";

                        break;
                    case "2":
                        String sqlcds = String.Format(@"SELECT OSBT.PROGRAM_TRACKING_BRAND_CD AS PRODUCT_CODE,OSBT.CUSTOMER_CODE,B.PROGRAM_TRACKING_BRAND_NAME AS PRODUCT_NAME,
                                                OSBT.SHELF_LENGTH,0 AS DISPLAY_ORDER,1 AS TYPE,B.PROGRAM_TRACKING_BRAND_CATEGORY_CD,OSBT.PROGRAM_TRACKING_CD
                                            FROM  O_POC_SHELF_BRAND_TRACKING OSBT
                                            INNER JOIN O_POC_PROGRAM_TRACKING_BRAND B ON OSBT.PROGRAM_TRACKING_BRAND_CD= B.PROGRAM_TRACKING_BRAND_CD
                                            INNER JOIN M_CUSTOMER MC ON OSBT.CUSTOMER_CODE=MC.CUSTOMER_CODE
                                            INNER JOIN O_CUSTOMER_ROUTE OCR ON OCR.CUSTOMER_CD= MC.CUSTOMER_CD
                                            INNER JOIN M_ROUTE MR ON OCR.ROUTE_CD = MR.ROUTE_CD
                                            INNER JOIN O_SUPERVISOR_DISTRIBUTOR OSD ON  MR.DISTRIBUTOR_CD=OSD.DISTRIBUTOR_CD
                                            INNER JOIN M_SUPERVISOR SUP ON OSD.SUPERVISOR_CD= SUP.SUPERVISOR_CD 
                                            WHERE SUP.SUPERVISOR_CODE='{0}'

                                            UNION ALL
                                                SELECT OST.PROGRAM_TRACKING_BRAND_CATEGORY_CD AS PRODUCT_CODE,OST.CUSTOMER_CODE,C.PROGRAM_TRACKING_BRAND_CATEGORY_NAME AS PRODUCT_NAME,
                                                OST.SHELF_LENGTH,0 AS DISPLAY_ORDER,0 AS TYPE,C.PROGRAM_TRACKING_BRAND_CATEGORY_CD ,OST.PROGRAM_TRACKING_CD

                                            FROM O_POC_SHELF_TRACKING OST

                                            INNER JOIN O_POC_PROGRAM_TRACKING_BRAND_CATEGORY C ON OST.PROGRAM_TRACKING_BRAND_CATEGORY_CD=C.PROGRAM_TRACKING_BRAND_CATEGORY_CD
                                            INNER JOIN M_CUSTOMER MC ON OST.CUSTOMER_CODE=MC.CUSTOMER_CODE
                                            INNER JOIN O_CUSTOMER_ROUTE OCR ON OCR.CUSTOMER_CD= MC.CUSTOMER_CD
                                            INNER JOIN M_ROUTE MR ON OCR.ROUTE_CD = MR.ROUTE_CD
                                            INNER JOIN O_SUPERVISOR_DISTRIBUTOR OSD ON  MR.DISTRIBUTOR_CD=OSD.DISTRIBUTOR_CD
                                            INNER JOIN M_SUPERVISOR SUP ON OSD.SUPERVISOR_CD= SUP.SUPERVISOR_CD 
                                            WHERE SUP.SUPERVISOR_CODE='{0}' ", obj);
                        dt = L5sSql.Query(sqlcds);
                        if (dt == null || dt.Rows.Count == 0)
                            return "1";
                        break;


                }
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }

        }

        #endregion

        #region synchronize customer  hàm đồng bộ dữ liệu khách hàng từ HH lên Server. Nếu chưa tồn tại thì thêm mới



        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCCustomer(String imei, String obj, String type, String jsonData)
        {
            try
            {


                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";


                if (jsonData.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCCustomerForSales(obj, type, jsonData);
                    case "2": // Sup
                        return this.synchronizePOCCustomerForSup(obj, type, jsonData);
                    case "3": //ASM
                        return this.synchronizePOCCustomerForASM(obj, type, jsonData);
                    default:
                        return "-1";
                }


            }
            catch (Exception Ex)
            {
                // L5sMsg.Show(Ex.Message);
                return "-1";
            }

        }

        private string synchronizePOCCustomerForASM(string obj, string type, string jsonData)
        {
            try
            {
                //Đối với ASM thì khi đồng bộ khách hàng chỉ ghi nhận history geocode và cập nhật lại thôn tin cơ bản không thay đổi thông tin tuyến của KH
                DataTable dtASM = L5sSql.Query("SELECT  * FROM M_ASM WHERE ASM_CODE = @0", obj);
                if (dtASM.Rows.Count <= 0)
                    return "-1";



                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                String asmCd = dtASM.Rows[0]["ASM_CD"].ToString();
                //add new column
                DataColumn colAsmCD = new DataColumn("ASM_CD");
                colAsmCD.DataType = typeof(String);
                colAsmCD.DefaultValue = asmCd;


                //add new column
                String sessionCD = System.Guid.NewGuid().ToString();
                DataColumn colSessionCD = new DataColumn("SESSION_CD");
                colSessionCD.DataType = typeof(String);
                colSessionCD.DefaultValue = sessionCD;


                dt.Columns.Add(colAsmCD);
                dt.Columns.Add(colSessionCD);

                if (!dt.Columns.Contains("customerLocationAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("customerLocationAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }

                if (!dt.Columns.Contains("customerPhoneNumber")) //version cũ không có thông tin customerPhoneNumber
                {
                    DataColumn customerPhoneNumber = new DataColumn("customerPhoneNumber");
                    customerPhoneNumber.DataType = typeof(String);
                    dt.Columns.Add(customerPhoneNumber);
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["customerLocationAccuracy"].ToString() == "")
                    {
                        dt.Rows[i]["customerLocationAccuracy"] = 0.0;
                    }

                    try
                    {
                        dt.Rows[i]["customerCode"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerCode"].ToString());
                        dt.Rows[i]["customerName"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerName"].ToString());
                        dt.Rows[i]["customerAddress"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerAddress"].ToString());
                    }
                    catch (Exception)
                    {

                    }
                }

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TEMP_CUSTOMER";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ASM_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCode", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerName", "CUSTOMER_NAME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerAddress", "CUSTOMER_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocation", "CUSTOMER_LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAccuracy", "CUSTOMER_LATITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCategory", "CUSTOMER_CHAIN_CODE"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerActive", "CUSTOMER_ACTIVE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerRoute", "ROUTE_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAddress", "CUSTOMER_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerPhoneNumber", "PHONE_NUMBER"));


                    bulkcopy.WriteToServer(dt);

                    String sql = String.Format(@"


                                                    --Cập nhật CUSTOMER_CD lần 1
                                                    UPDATE T1 SET CUSTOMER_CD = T2.CUSTOMER_CD
                                                    FROM TEMP_CUSTOMER T1 INNER JOIN M_CUSTOMER T2 ON T1.CUSTOMER_CODE = T2.CUSTOMER_CODE 
                                                    WHERE T1.SESSION_CD = '{0}'  AND T1.CUSTOMER_CD IS NULL

                                                        
                                                                      
                                                    --INSERT HISTORY CUSTOMER
                                                     INSERT INTO [dbo].[H_CUSTOMER_LATITUDE_LONGITUDE]
                                                                       ([CUSTOMER_CD]
                                                                       ,[OLD_LATITUDE_LONGITUDE]
                                                                       ,[OLD_LATITUDE_LONGITUDE_ACCURACY]
                                                                       ,[NEW_LATITUDE_LONGITUDE]
                                                                       ,[NEW_LATITUDE_LONGITUDE_ACCURACY])
                                                    SELECT  tempCust.CUSTOMER_CD
		                                                    ,cust.LONGITUDE_LATITUDE
		                                                    ,cust.LONGITUDE_LATITUDE_ACCURACY
		                                                    ,tempCust.CUSTOMER_LONGITUDE_LATITUDE
		                                                    ,tempCust.CUSTOMER_LATITUDE_ACCURACY
                                                    FROM TEMP_CUSTOMER tempCust INNER JOIN M_CUSTOMER cust ON tempCust.CUSTOMER_CD = cust.CUSTOMER_CD
                                                    WHERE tempCust.CUSTOMER_LONGITUDE_LATITUDE != cust.LONGITUDE_LATITUDE 
		                                                    AND tempCust.CUSTOMER_CD IS NOT NULL
		                                                    AND SESSION_CD = '{0}'  
                                                     
      
                                                --luu lai thong tin goc
                                                INSERT INTO [dbo].[O_CUSTOMER_HISTORY_INFO]
			                                                ([CUSTOMER_CD],
			                                                [CUSTOMER_CODE],
			                                                CUSTOMER_NAME,
			                                                CUSTOMER_ADDRESS,
			                                                CUSTOMER_CHAIN_CODE,
                                                            DISTRIBUTOR_CD,PHONE_NUMBER)
                                                SELECT	T1.CUSTOMER_CD,
		                                                T1.CUSTOMER_CODE,
		                                                T1.CUSTOMER_NAME,  
		                                                T1.CUSTOMER_ADDRESS,
		                                                T1.CUSTOMER_CHAIN_CODE,
                                                        T1.DISTRIBUTOR_CD, T1.PHONE_NUMBER
                                                FROM M_CUSTOMER T1 INNER JOIN TEMP_CUSTOMER T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD
                                                WHERE (T2.SESSION_CD = '{0}' AND T2.CUSTOMER_CD IS NOT NULL) AND 
                                                        (
                                                               RTRIM(LTRIM(ISNULL(T1.CUSTOMER_NAME,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_NAME,'')))
                                                            OR RTRIM(LTRIM(ISNULL(T1.CUSTOMER_ADDRESS,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_ADDRESS,'')))
														    OR RTRIM(LTRIM(ISNULL(T1.CUSTOMER_CHAIN_CODE,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_CHAIN_CODE,'')))
                                                            OR RTRIM(LTRIM(ISNULL(T1.PHONE_NUMBER,''))) != RTRIM(LTRIM(ISNULL(T2.PHONE_NUMBER,'')))
                                                        )  


                                                --cập nhật info của khách hàng 
                                                UPDATE T1 SET 
                                                              CUSTOMER_ADDRESS = T2.CUSTOMER_ADDRESS,
                                                              CUSTOMER_NAME = T2.CUSTOMER_NAME,
                                                              LONGITUDE_LATITUDE = T2.CUSTOMER_LONGITUDE_LATITUDE,
                                                              LONGITUDE_LATITUDE_ACCURACY = T2.CUSTOMER_LATITUDE_ACCURACY,
                                                              CUSTOMER_CHAIN_CODE = T2.CUSTOMER_CHAIN_CODE,
                                                              ACTIVE = T2.CUSTOMER_ACTIVE, 
                                                              UPDATED_DATE = GETDATE(),
                                                              CUSTOMER_LOCATION_ADDRESS = T2.CUSTOMER_LOCATION_ADDRESS,
                                                              PHONE_NUMBER = T2.PHONE_NUMBER
                                                FROM M_CUSTOMER T1 INNER JOIN TEMP_CUSTOMER T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD
                                                WHERE T2.SESSION_CD = '{0}' AND T2.CUSTOMER_CD IS NOT NULL
                                                    
                                               
                                                --INACTIVE O_CUSTOMER_ASM_ROUTE nếu CUSTOMER INACTIVE
                                                UPDATE custR SET ACTIVE = 1, DEACTIVE_DATE = GETDATE()
                                                FROM O_CUSTOMER_ASM_ROUTE custR INNER JOIN  
								                                                TEMP_CUSTOMER tmpCust 
									                                                ON custR.CUSTOMER_CD = tmpCust.CUSTOMER_CD AND custR.ACTIVE = 1
                                                WHERE tmpCust.SESSION_CD = '{0}'  AND tmpCust.CUSTOMER_ACTIVE = 0           
                                                                                      

                                                --DELETE DATA
                                                DELETE  FROM TEMP_CUSTOMER
                                                WHERE  SESSION_CD = '{0}'     

                                            ", sessionCD);

                    L5sSql.Execute(sql);
                }

                return "1";
            }
            catch (Exception)
            {
                return "-1";
            }

        }

        private string synchronizePOCCustomerForSup(string obj, string type, string jsonData)
        {
            try
            {
                //Đối với Giám Sát thì khi đồng bộ khách hàng chỉ ghi nhận history geocode và cập nhật lại thôn tin cơ bản không thay đổi thông tin tuyến của KH
                DataTable dtSup = L5sSql.Query("SELECT  * FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0", obj);
                if (dtSup.Rows.Count <= 0)
                    return "-1";



                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                String supervisorCD = dtSup.Rows[0]["SUPERVISOR_CD"].ToString();

                //add new column
                DataColumn colSupervisorCD = new DataColumn("SUPERVISOR_CD");
                colSupervisorCD.DataType = typeof(String);
                colSupervisorCD.DefaultValue = supervisorCD;


                //add new column
                String sessionCD = System.Guid.NewGuid().ToString();
                DataColumn colSessionCD = new DataColumn("SESSION_CD");
                colSessionCD.DataType = typeof(String);
                colSessionCD.DefaultValue = sessionCD;

                dt.Columns.Add(colSupervisorCD);
                dt.Columns.Add(colSessionCD);

                if (!dt.Columns.Contains("customerLocationAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("customerLocationAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }

                if (!dt.Columns.Contains("customerPhoneNumber")) //version cũ không có thông tin customerPhoneNumber
                {
                    DataColumn customerPhoneNumber = new DataColumn("customerPhoneNumber");
                    customerPhoneNumber.DataType = typeof(String);
                    dt.Columns.Add(customerPhoneNumber);
                }


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["customerLocationAccuracy"].ToString() == "")
                    {
                        dt.Rows[i]["customerLocationAccuracy"] = 0.0;
                    }
                    try
                    {
                        dt.Rows[i]["customerCode"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerCode"].ToString());
                        dt.Rows[i]["customerName"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerName"].ToString());
                        dt.Rows[i]["customerAddress"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerAddress"].ToString());
                    }
                    catch (Exception)
                    {

                    }

                }

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TEMP_CUSTOMER";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SUPERVISOR_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCode", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerName", "CUSTOMER_NAME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerAddress", "CUSTOMER_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocation", "CUSTOMER_LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAccuracy", "CUSTOMER_LATITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCategory", "CUSTOMER_CHAIN_CODE"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerActive", "CUSTOMER_ACTIVE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerRoute", "ROUTE_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAddress", "CUSTOMER_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerPhoneNumber", "PHONE_NUMBER"));


                    bulkcopy.WriteToServer(dt);

                    String sql = String.Format(@"

                                                   --Cập nhật CUSTOMER_CD lần 1
                                                    UPDATE T1 SET CUSTOMER_CD = T2.CUSTOMER_CD
                                                    FROM TEMP_CUSTOMER T1 INNER JOIN M_CUSTOMER T2 ON T1.CUSTOMER_CODE = T2.CUSTOMER_CODE 
                                                    WHERE T1.SESSION_CD = '{0}'  AND T1.CUSTOMER_CD IS NULL

                                                        
                                                                      
                                                    --INSERT HISTORY CUSTOMER
                                                     INSERT INTO [dbo].[H_CUSTOMER_LATITUDE_LONGITUDE]
                                                                       ([CUSTOMER_CD]
                                                                       ,[OLD_LATITUDE_LONGITUDE]
                                                                       ,[OLD_LATITUDE_LONGITUDE_ACCURACY]
                                                                       ,[NEW_LATITUDE_LONGITUDE]
                                                                       ,[NEW_LATITUDE_LONGITUDE_ACCURACY])
                                                    SELECT  tempCust.CUSTOMER_CD
		                                                    ,cust.LONGITUDE_LATITUDE
		                                                    ,cust.LONGITUDE_LATITUDE_ACCURACY
		                                                    ,tempCust.CUSTOMER_LONGITUDE_LATITUDE
		                                                    ,tempCust.CUSTOMER_LATITUDE_ACCURACY
                                                    FROM TEMP_CUSTOMER tempCust INNER JOIN M_CUSTOMER cust ON tempCust.CUSTOMER_CD = cust.CUSTOMER_CD
                                                    WHERE tempCust.CUSTOMER_LONGITUDE_LATITUDE != cust.LONGITUDE_LATITUDE 
		                                                    AND tempCust.CUSTOMER_CD IS NOT NULL
		                                                    AND SESSION_CD = '{0}'  
                                                           
                                                --luu lai thong tin goc
                                                INSERT INTO [dbo].[O_CUSTOMER_HISTORY_INFO]
			                                                ([CUSTOMER_CD],
			                                                [CUSTOMER_CODE],
			                                                CUSTOMER_NAME,
			                                                CUSTOMER_ADDRESS,
			                                                CUSTOMER_CHAIN_CODE,
                                                            DISTRIBUTOR_CD,PHONE_NUMBER)
                                                SELECT	T1.CUSTOMER_CD,
		                                                T1.CUSTOMER_CODE,
		                                                T1.CUSTOMER_NAME,  
		                                                T1.CUSTOMER_ADDRESS,
		                                                T1.CUSTOMER_CHAIN_CODE,
                                                        T1.DISTRIBUTOR_CD, T1.PHONE_NUMBER
                                                FROM M_CUSTOMER T1 INNER JOIN TEMP_CUSTOMER T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD
                                                WHERE (T2.SESSION_CD = '{0}' AND T2.CUSTOMER_CD IS NOT NULL) AND 
                                                        (
                                                               RTRIM(LTRIM(ISNULL(T1.CUSTOMER_NAME,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_NAME,'')))
                                                            OR RTRIM(LTRIM(ISNULL(T1.CUSTOMER_ADDRESS,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_ADDRESS,'')))
														    OR RTRIM(LTRIM(ISNULL(T1.CUSTOMER_CHAIN_CODE,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_CHAIN_CODE,'')))
                                                            OR RTRIM(LTRIM(ISNULL(T1.PHONE_NUMBER,''))) != RTRIM(LTRIM(ISNULL(T2.PHONE_NUMBER,'')))
                                                        )  


                                                --cập nhật info của khách hàng 
                                                UPDATE T1 SET 
                                                              CUSTOMER_ADDRESS = T2.CUSTOMER_ADDRESS,
                                                              CUSTOMER_NAME = T2.CUSTOMER_NAME,
                                                              LONGITUDE_LATITUDE = T2.CUSTOMER_LONGITUDE_LATITUDE,
                                                              LONGITUDE_LATITUDE_ACCURACY = T2.CUSTOMER_LATITUDE_ACCURACY,
                                                              CUSTOMER_CHAIN_CODE = T2.CUSTOMER_CHAIN_CODE,
                                                              ACTIVE = T2.CUSTOMER_ACTIVE,
                                                              UPDATED_DATE = GETDATE(),
                                                              CUSTOMER_LOCATION_ADDRESS = T2.CUSTOMER_LOCATION_ADDRESS,
                                                              PHONE_NUMBER = T2.PHONE_NUMBER
                                                FROM M_CUSTOMER T1 INNER JOIN TEMP_CUSTOMER T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD
                                                WHERE T2.SESSION_CD = '{0}' AND T2.CUSTOMER_CD IS NOT NULL
                                                    
                                               
                                                --INACTIVE O_CUSTOMER_ASM_ROUTE nếu CUSTOMER INACTIVE
                                                UPDATE custR SET ACTIVE = 1, DEACTIVE_DATE = GETDATE()
                                                FROM O_CUSTOMER_SUPERVISOR_ROUTE custR INNER JOIN  
								                                                TEMP_CUSTOMER tmpCust 
									                                                ON custR.CUSTOMER_CD = tmpCust.CUSTOMER_CD AND custR.ACTIVE = 1
                                                WHERE tmpCust.SESSION_CD = '{0}'  AND tmpCust.CUSTOMER_ACTIVE = 0           
                                                                                      

                                                --DELETE DATA
                                                DELETE  FROM TEMP_CUSTOMER
                                                WHERE  SESSION_CD = '{0}'    

                                            ", sessionCD);

                    L5sSql.Execute(sql);
                }





                return "1";
            }
            catch (Exception Ex)
            {
                L5sMsg.Show(Ex.Message);
                return "-1";
            }

        }

        private string synchronizePOCCustomerForSales(string obj, string type, string jsonData)
        {

            try
            {

                DataTable dtSales = L5sSql.Query("SELECT  DISTRIBUTOR_CD,SALES_CD FROM M_SALES WHERE SALES_CODE = @0", obj);
                if (dtSales.Rows.Count <= 0)
                    return "-1";


                String distributorCD = dtSales.Rows[0]["DISTRIBUTOR_CD"].ToString();
                String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();

                //add new column
                DataColumn colSalesCD = new DataColumn("SALES_CD");
                colSalesCD.DataType = typeof(String);
                colSalesCD.DefaultValue = salesCD;

                //add new column
                DataColumn colDistributorCD = new DataColumn("DISTRIBUTOR_CD");
                colDistributorCD.DataType = typeof(String);
                colDistributorCD.DefaultValue = distributorCD;

                //add new column
                String sessionCD = System.Guid.NewGuid().ToString();
                DataColumn colSessionCD = new DataColumn("SESSION_CD");
                colSessionCD.DataType = typeof(String);
                colSessionCD.DefaultValue = sessionCD;

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                dt.Columns.Add(colSalesCD);
                dt.Columns.Add(colDistributorCD);
                dt.Columns.Add(colSessionCD);

                if (!dt.Columns.Contains("customerLocationAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("customerLocationAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }

                if (!dt.Columns.Contains("customerPhoneNumber")) //version cũ không có thông tin customerPhoneNumber
                {
                    DataColumn customerPhoneNumber = new DataColumn("customerPhoneNumber");
                    customerPhoneNumber.DataType = typeof(String);
                    dt.Columns.Add(customerPhoneNumber);
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["customerLocationAccuracy"].ToString() == "") //nếu như ở HH không ghi nhận dc Accuracy thì hệ thống sẽ thiết lập là 0 
                    {
                        dt.Rows[i]["customerLocationAccuracy"] = 0.0;
                    }

                    try
                    {
                        //chuẩn hóa dữ liệu để khi convert qua json không bị lỗi cú pháp do chứa các ký tự đặc biệt
                        dt.Rows[i]["customerCode"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerCode"].ToString());
                        dt.Rows[i]["customerName"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerName"].ToString());
                        dt.Rows[i]["customerAddress"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerAddress"].ToString());
                    }
                    catch (Exception)
                    {

                    }

                }



                //  dt.Rows.Add("TTP", "TRẦN TẤN PHƯỚC--XX", "18/5C KHU PHỐ 1 -XXX", "12.171606,109.184681", 0, "SGN", true, "2H11");


                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TEMP_CUSTOMER";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCode", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerName", "CUSTOMER_NAME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerAddress", "CUSTOMER_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocation", "CUSTOMER_LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAccuracy", "CUSTOMER_LATITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCategory", "CUSTOMER_CHAIN_CODE"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerActive", "CUSTOMER_ACTIVE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerRoute", "ROUTE_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAddress", "CUSTOMER_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerPhoneNumber", "PHONE_NUMBER"));


                    bulkcopy.WriteToServer(dt);

                    //đoạn code dưới phân dữ liệu thành 2 loại: KH mới (chưa có CD trên hệ thống) và KH cũ (đã CD trên hệ thống)

                    String sql = String.Format(@"

                                                    --Cập nhật CUSTOMER_CD lần 1 
                                                    --mục đích: xác định KH cũ trên hệ thống đã có CD rồi
                                                    UPDATE T1 SET CUSTOMER_CD = T2.CUSTOMER_CD
                                                    FROM TEMP_CUSTOMER T1 INNER JOIN M_CUSTOMER T2 ON T1.CUSTOMER_CODE = T2.CUSTOMER_CODE 
                                                    WHERE T1.SESSION_CD = '{0}'  AND T1.CUSTOMER_CD IS NULL

                                                    --Cập nhật ROUTE_CD lần 1
                                                    --Xác định xem tuyến của KH đã tồn tại trên hệ thống hay chưa
                                                    UPDATE T1 SET ROUTE_CD = T2.ROUTE_CD
                                                    FROM TEMP_CUSTOMER T1 INNER JOIN M_ROUTE T2 ON T1.ROUTE_CODE = T2.ROUTE_CODE 
                                                    WHERE T1.SESSION_CD = '{0}'  AND T1.ROUTE_CD IS NULL
                                                        
                                                    --Process new customer
                                                   INSERT INTO [dbo].[M_CUSTOMER]
                                                       ([CUSTOMER_CODE]
                                                       ,[CUSTOMER_NAME]
                                                       ,[CUSTOMER_ADDRESS]
                                                       ,[LONGITUDE_LATITUDE]
                                                       ,[LONGITUDE_LATITUDE_ACCURACY]
                                                       ,[CUSTOMER_CHAIN_CODE]
                                                       ,[ACTIVE]
                                                       ,[DISTRIBUTOR_CD]                                                   
                                                       ,[IS_PDA],[CUSTOMER_LOCATION_ADDRESS],PHONE_NUMBER)
                                                   SELECT CUSTOMER_CODE
                                                          ,CUSTOMER_NAME
                                                          ,CUSTOMER_ADDRESS
                                                          ,CUSTOMER_LONGITUDE_LATITUDE
                                                          ,CUSTOMER_LATITUDE_ACCURACY
                                                          ,CUSTOMER_CHAIN_CODE
                                                          ,CUSTOMER_ACTIVE
                                                          ,DISTRIBUTOR_CD
                                                          ,1,[CUSTOMER_LOCATION_ADDRESS],PHONE_NUMBER
                                                   FROM TEMP_CUSTOMER
                                                   WHERE CUSTOMER_CD IS NULL AND SESSION_CD = '{0}'
                             
                                                    
                                                --Cập nhật CUSTOMER_CD lần 2
                                                --mục đích  là cập nhật lại CD cho KH vừa thêm mới
                                                UPDATE T1 SET CUSTOMER_CD = T2.CUSTOMER_CD
                                                FROM TEMP_CUSTOMER T1 INNER JOIN M_CUSTOMER T2 ON T1.CUSTOMER_CODE = T2.CUSTOMER_CODE 
                                                WHERE T1.SESSION_CD = '{0}'  AND T1.CUSTOMER_CD IS NULL
                                                    

                                                --cập nhật lịch sử tọa độ của KH nếu như có thay đổi
                                                --INSERT HISTORY CUSTOMER
                                                 INSERT INTO [dbo].[H_CUSTOMER_LATITUDE_LONGITUDE]
                                                                   ([CUSTOMER_CD]
                                                                   ,[OLD_LATITUDE_LONGITUDE]
                                                                   ,[OLD_LATITUDE_LONGITUDE_ACCURACY]
                                                                   ,[NEW_LATITUDE_LONGITUDE]
                                                                   ,[NEW_LATITUDE_LONGITUDE_ACCURACY])
                                                SELECT  tempCust.CUSTOMER_CD
		                                                ,cust.LONGITUDE_LATITUDE
		                                                ,cust.LONGITUDE_LATITUDE_ACCURACY
		                                                ,tempCust.CUSTOMER_LONGITUDE_LATITUDE
		                                                ,tempCust.CUSTOMER_LATITUDE_ACCURACY
                                                FROM TEMP_CUSTOMER tempCust INNER JOIN M_CUSTOMER cust ON tempCust.CUSTOMER_CD = cust.CUSTOMER_CD
                                                WHERE tempCust.CUSTOMER_LONGITUDE_LATITUDE != cust.LONGITUDE_LATITUDE 
		                                                AND tempCust.CUSTOMER_CD IS NOT NULL
		                                                AND SESSION_CD = '{0}'  
                                                           
                                                --luu lai thong tin goc
                                                INSERT INTO [dbo].[O_CUSTOMER_HISTORY_INFO]
			                                                ([CUSTOMER_CD],
			                                                [CUSTOMER_CODE],
			                                                CUSTOMER_NAME,
			                                                CUSTOMER_ADDRESS,
			                                                CUSTOMER_CHAIN_CODE,
                                                            DISTRIBUTOR_CD,PHONE_NUMBER)
                                                SELECT	T1.CUSTOMER_CD,
		                                                T1.CUSTOMER_CODE,
		                                                T1.CUSTOMER_NAME,  
		                                                T1.CUSTOMER_ADDRESS,
		                                                T1.CUSTOMER_CHAIN_CODE,
                                                        T1.DISTRIBUTOR_CD,T1.PHONE_NUMBER
                                                FROM M_CUSTOMER T1 INNER JOIN TEMP_CUSTOMER T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD
                                                WHERE (T2.SESSION_CD = '{0}' AND T2.CUSTOMER_CD IS NOT NULL) AND 
                                                        (
                                                               RTRIM(LTRIM(ISNULL(T1.CUSTOMER_NAME,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_NAME,'')))
                                                            OR RTRIM(LTRIM(ISNULL(T1.CUSTOMER_ADDRESS,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_ADDRESS,'')))
														    OR RTRIM(LTRIM(ISNULL(T1.CUSTOMER_CHAIN_CODE,''))) != RTRIM(LTRIM(ISNULL(T2.CUSTOMER_CHAIN_CODE,'')))
                                                            OR RTRIM(LTRIM(ISNULL(T1.PHONE_NUMBER,''))) != RTRIM(LTRIM(ISNULL(T2.PHONE_NUMBER,'')))
                                                        )  


                                                --cập nhật info của khách hàng                                                
                                                UPDATE T1 SET DISTRIBUTOR_CD = T2.DISTRIBUTOR_CD,
                                                              CUSTOMER_ADDRESS = T2.CUSTOMER_ADDRESS,
                                                              CUSTOMER_NAME = T2.CUSTOMER_NAME,
                                                              LONGITUDE_LATITUDE = T2.CUSTOMER_LONGITUDE_LATITUDE,
                                                              LONGITUDE_LATITUDE_ACCURACY = T2.CUSTOMER_LATITUDE_ACCURACY,
                                                              CUSTOMER_CHAIN_CODE = T2.CUSTOMER_CHAIN_CODE,
                                                              ACTIVE = T2.CUSTOMER_ACTIVE,
                                                              UPDATED_DATE = GETDATE(),
                                                              CUSTOMER_LOCATION_ADDRESS = T2.CUSTOMER_LOCATION_ADDRESS,
                                                              PHONE_NUMBER = T2.PHONE_NUMBER
                                                FROM M_CUSTOMER T1 INNER JOIN TEMP_CUSTOMER T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD
                                                WHERE T2.SESSION_CD = '{0}' AND T2.CUSTOMER_CD IS NOT NULL

                                                                                                                                                        
                                                --INACTIVE O_CUSTOMER_ROUTE nếu CUSTOMER ĐANG THUỘC 1 TUYẾN KHÁC
                                                UPDATE custR SET ACTIVE = 0, DEACTIVE_DATE = GETDATE()
                                                FROM O_CUSTOMER_ROUTE custR INNER JOIN  
								                                                TEMP_CUSTOMER tmpCust 
									                                                ON custR.CUSTOMER_CD = tmpCust.CUSTOMER_CD AND custR.ACTIVE = 1
									                                                AND custR.ROUTE_CD != tmpCust.ROUTE_CD
                                                WHERE tmpCust.SESSION_CD = '{0}'                                      
    

                                                --INACTIVE CUSTOMER nếu như KH bị inactive ở HH
                                                UPDATE T1 SET ACTIVE = T2.CUSTOMER_ACTIVE
                                                FROM M_CUSTOMER T1 INNER JOIN TEMP_CUSTOMER T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD 
                                                WHERE T2.SESSION_CD = '{0}' AND T2.CUSTOMER_CD IS NOT NULL


                                                --INSERT CUSTOMER ROUTE
                                                INSERT INTO O_CUSTOMER_ROUTE (CUSTOMER_CD,ROUTE_CD)
                                                SELECT CUSTOMER_CD,ROUTE_CD 
                                                FROM TEMP_CUSTOMER tmpCust
                                                WHERE NOT  EXISTS (SELECT * FROM O_CUSTOMER_ROUTE custR WHERE tmpCust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1)
	                                                AND tmpCust.SESSION_CD = '{0}'
                                                
                                                --

                                                --INACTIVE O_CUSTOMER_ROUTE IF CUSTOMER INACTIVE
                                               UPDATE O_CUSTOMER_ROUTE SET ACTIVE = 0, DEACTIVE_DATE = GETDATE()
                                                FROM O_CUSTOMER_ROUTE custR INNER JOIN 
					                                                TEMP_CUSTOMER tmpCust ON custR.CUSTOMER_CD = tmpCust.CUSTOMER_CD AND custR.ACTIVE = 1 AND tmpCust.CUSTOMER_ACTIVE = 0
                                                WHERE tmpCust.SESSION_CD = '{0}'


                                           

                                                --DELETE DATA
                                                DELETE  FROM TEMP_CUSTOMER
                                                WHERE  SESSION_CD = '{0}'    

                                                --REMOVE DUPLICATE O_CUSTOMER_ROUTE
                                                DECLARE @TABLE_ROUTE AS TABLE
                                                (
	                                                CUSTOMER_ROUTE_CD BIGINT,
	                                                CUSTOMER_CD BIGINT,
	                                                ROUTE_CD BIGINT,
	                                                RN INT
                                                )

                                                INSERT INTO @TABLE_ROUTE
                                                SELECT 	CUSTOMER_ROUTE_CD,CUSTOMER_CD,ROUTE_CD,RN FROM
                                                 (
	                                                  SELECT 
		                                                CUSTOMER_ROUTE_CD,CUSTOMER_CD,ROUTE_CD,
		                                                RN = ROW_NUMBER()OVER(
					                                                PARTITION BY  CUSTOMER_CD,ROUTE_CD
			                                                ORDER BY 
					                                                CREATED_DATE)                                             
                                                                                                                                                    	               
	                                                FROM    O_CUSTOMER_ROUTE
	                                                WHERE ACTIVE = 1 	
                                                 ) AS T 
                                                 WHERE T.RN = 2

                                                DELETE FROM O_CUSTOMER_ROUTE 
                                                WHERE EXISTS (SELECT CUSTOMER_ROUTE_CD FROM @TABLE_ROUTE AS T WHERE  O_CUSTOMER_ROUTE.CUSTOMER_ROUTE_CD = T.CUSTOMER_ROUTE_CD )


                        
                                                --REMOVE DUPLICATE O_SALES_ROUTE
                                                DECLARE @TABLE_ROUTE_SALES AS TABLE
                                                (
                                                SALES_ROUTE_CD BIGINT,
                                                SALES_CD BIGINT,
                                                ROUTE_CD BIGINT,
                                                RN INT
                                                )

                                                INSERT INTO @TABLE_ROUTE_SALES
                                                SELECT 	SALES_ROUTE_CD,SALES_CD,ROUTE_CD,RN FROM
                                                (
	                                                SELECT 
	                                                SALES_ROUTE_CD,SALES_CD,ROUTE_CD,
	                                                RN = ROW_NUMBER()OVER(
				                                                PARTITION BY  SALES_CD,ROUTE_CD
		                                                ORDER BY 
				                                                CREATED_DATE)                                             
                                                                                                                                                                                                    	               
                                                FROM    O_SALES_ROUTE 
                                                WHERE ACTIVE = 1 	
                                                ) AS T 
                                                WHERE T.RN = 2

                                                DELETE FROM O_SALES_ROUTE 
                                                WHERE EXISTS (SELECT SALES_ROUTE_CD FROM @TABLE_ROUTE_SALES AS T WHERE  O_SALES_ROUTE.SALES_ROUTE_CD = T.SALES_ROUTE_CD )

                                                
                                                --remove duplicate customer
                                                DELETE FROM  M_CUSTOMER
                                                WHERE CUSTOMER_CD NOT IN 
                                                (   SELECT MIN(CUSTOMER_CD) 
	                                                FROM  M_CUSTOMER   
	                                                GROUP BY CUSTOMER_CD
                                                )


                                            ", sessionCD);

                    L5sSql.Execute(sql);
                }
                return "1";
            }
            catch (Exception ex)
            {

                return "-1";
            }

        }


        #endregion
        #region synchronize route Hàm cập nhật tuyến cho NVBH từ HH gửi lên.Nếu tuyến chưa tồn tại thì thêm mới


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCRoute(String imei, String obj, String type, String jsonData)
        {
            return "1"; //TKD Bo qua cap nhat tuyen cho NVBH

            try
            {

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";


                if (jsonData.Trim().Length == 0)
                    return "-1";
                jsonData = P5sDmComm.P5sSecurity.Decrypt(jsonData);

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCRouteForSales(obj, type, jsonData);
                    case "2": // Sup
                        return "-1";
                    case "3": //ASM
                        return "-1";
                    default:
                        return "-1";
                }


            }
            catch (Exception Ex)
            {
                // L5sMsg.Show(Ex.Message);
                return "-1";
            }

        }

        private string synchronizePOCRouteForSales(string obj, string type, string jsonData)
        {
            //Kiểm tra hệ thống này là của PH thì sẽ ko cho đồng bộ tuyến lên
            DataTable tbCountry = L5sSql.Query(@"SELECT NAME,VALUE FROM S_PARAMS WHERE NAME='COUNTRY' AND VALUE ='PH'");
            if (tbCountry != null && tbCountry.Rows.Count > 0)
                return "1";
            try
            {

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                DataTable dtDistributor = L5sSql.Query(@"SELECT DISTRIBUTOR_CD,SALES_CD FROM M_SALES WHERE SALES_CODE = @0", obj);
                if (dtDistributor == null || dtDistributor.Rows.Count == 0)
                    return "2";

                String distributorCD = dtDistributor.Rows[0]["DISTRIBUTOR_CD"].ToString();
                String salesCD = dtDistributor.Rows[0]["SALES_CD"].ToString();


                //add new column
                DataColumn colSalesCD = new DataColumn("SALES_CD");
                colSalesCD.DataType = typeof(String);
                colSalesCD.DefaultValue = salesCD;

                //add new column
                DataColumn colDistributorCD = new DataColumn("DISTRIBUTOR_CD");
                colDistributorCD.DataType = typeof(String);
                colDistributorCD.DefaultValue = distributorCD;

                //add new column
                String sessionCD = System.Guid.NewGuid().ToString();
                DataColumn colSessionCD = new DataColumn("SESSION_CD");
                colSessionCD.DataType = typeof(String);
                colSessionCD.DefaultValue = sessionCD;


                dt.Columns.Add(colSalesCD);
                dt.Columns.Add(colDistributorCD);
                dt.Columns.Add(colSessionCD);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        dt.Rows[i]["routeCode"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["routeCode"].ToString());
                        dt.Rows[i]["routeName"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["routeName"].ToString());
                    }
                    catch (Exception)
                    {

                    }

                }



                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TMP_ROUTE";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("routeCode", "ROUTE_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("routeName", "ROUTE_NAME"));

                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        //SQL PROCESS
                        String sql = String.Format(@"
                                                     
                                                    --Cập nhật routeCD lần 1
                                                    UPDATE T1 SET ROUTE_CD = T2.ROUTE_CD
                                                    FROM TMP_ROUTE T1 INNER JOIN M_ROUTE T2 ON T1.ROUTE_CODE = T2.ROUTE_CODE 
                                                    WHERE T1.SESSION_CD = '{0}'  AND T1.ROUTE_CD IS NULL
                                                    
                                                    --Process new route                                                    
                                        
		                                            INSERT INTO M_ROUTE  (ROUTE_CODE,ROUTE_NAME,DISTRIBUTOR_CD,ACTIVE)
		                                            SELECT ROUTE_CODE,ROUTE_NAME,DISTRIBUTOR_CD,1 FROM TMP_ROUTE
                                                    WHERE ROUTE_CD IS NULL AND SESSION_CD = '{0}' 
                                                    
                                                    --Cập nhật routeCD lần 2
                                            
                                                    UPDATE T1 SET ROUTE_CD = T2.ROUTE_CD
                                                    FROM TMP_ROUTE T1 INNER JOIN M_ROUTE T2 ON T1.ROUTE_CODE = T2.ROUTE_CODE 
                                                    WHERE T1.SESSION_CD = '{0}' AND T1.ROUTE_CD IS NULL
                                                    
                                                    --cập nhật DISTRIBUTOR_CD,ROUTE_NAME cho bảng M_ROUTE
                                                    UPDATE T1 SET DISTRIBUTOR_CD = T2.DISTRIBUTOR_CD, ROUTE_NAME = T2.ROUTE_NAME
                                                    FROM M_ROUTE T1 INNER JOIN TMP_ROUTE T2 ON T1.ROUTE_CD = T2.ROUTE_CD
                                                    WHERE T2.SESSION_CD = '{0}' AND T2.ROUTE_CD IS NOT NULL

                                        

                                                   -- UPDATE DISTRIBUTOR  CUSTOMER OF ROUTE
                                                    UPDATE cust SET DISTRIBUTOR_CD = tmpRoute.DISTRIBUTOR_CD
                                                    FROM M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
	                                                     INNER JOIN TMP_ROUTE tmpRoute ON custR.ROUTE_CD = tmpRoute.ROUTE_CD AND cust.DISTRIBUTOR_CD != tmpRoute.DISTRIBUTOR_CD
                                                    WHERE tmpRoute.SESSION_CD = '{0}'                                                  

                                                    --INACTIVE O_SALES_ROUTE nếu ROUTE ĐANG THUỘC 1 NVBH KHÁC
                                                    UPDATE slsR SET ACTIVE = 0, DEACTIVE_DATE = GETDATE()
                                                    FROM O_SALES_ROUTE slsR INNER JOIN  TMP_ROUTE tmpRoute 
								                                                    ON slsR.ROUTE_CD = tmpRoute.ROUTE_CD AND slsR.ACTIVE = 1
								                                                    AND slsR.SALES_CD != tmpRoute.SALES_CD
                                                    WHERE tmpRoute.SESSION_CD = '{0}'   

                                                    --INSER ROUTE FOR SALES
                                                    INSERT INTO O_SALES_ROUTE (ROUTE_CD,SALES_CD)
                                                    SELECT ROUTE_CD,SALES_CD FROM TMP_ROUTE tmpRoute
                                                    WHERE NOT  EXISTS (SELECT * FROM O_SALES_ROUTE slsR WHERE tmpRoute.ROUTE_CD = slsR.ROUTE_CD AND slsR.ACTIVE = 1)
	                                                      AND tmpRoute.SESSION_CD = '{0}'

                                                   DELETE  FROM TMP_ROUTE
                                                   WHERE  SESSION_CD = '{0}'
                                                ", sessionCD);


                        L5sSql.Execute(sql);

                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }

                }
                return "1";
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        #endregion
        #region synchronize time int out hàm ghi nhận thông tin timeinout từ HH gửi lên

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCTimeInOut(String imei, String obj, String type, String jsonData)
        {
            try
            {

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                if (jsonData.Trim().Length == 0)
                    return "-1";

                jsonData = P5sDmComm.P5sSecurity.Decrypt(jsonData);

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCTimeInOutForSales(obj, type, jsonData);
                    case "2": // Sup
                        return this.synchronizePOCTimeInOutForSup(obj, type, jsonData);
                    case "3": //ASM
                        return this.synchronizePOCTimeInOutForASM(obj, type, jsonData);
                    default:
                        return "-1";
                }

            }
            catch (Exception Ex)
            {
                return "-1";
            }

        }


        private string synchronizePOCTimeInOutForASM(string obj, string type, string jsonData)
        {
            DataTable dtASM = L5sSql.Query("SELECT  * FROM M_ASM WHERE ASM_CODE = @0", obj);
            if (dtASM.Rows.Count <= 0)
                return "-1";

            if (jsonData.Trim().Length == 0)
                return "-1";

            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String asmCD = dtASM.Rows[0]["ASM_CD"].ToString();
                String typeCD = type;

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = asmCD;

                DataColumn columnTypeCD = new DataColumn("TYPE_CD");
                columnTypeCD.DataType = typeof(String);
                columnTypeCD.DefaultValue = typeCD;

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnTypeCD);

                if (!dt.Columns.Contains("timeInLatitudeLongitudeAddress")) //version cũ không có thông tin timeInLatitudeLongitudeAddress
                {
                    DataColumn timeInLatitudeLongitudeAddress = new DataColumn("timeInLatitudeLongitudeAddress");
                    timeInLatitudeLongitudeAddress.DataType = typeof(String);
                    dt.Columns.Add(timeInLatitudeLongitudeAddress);
                }

                if (!dt.Columns.Contains("timeOutLatitudeLongitudeAddress")) //version cũ không có thông tin timeOutLatitudeLongitudeAddress
                {
                    DataColumn timeOutLatitudeLongitudeAddress = new DataColumn("timeOutLatitudeLongitudeAddress");
                    timeOutLatitudeLongitudeAddress.DataType = typeof(String);
                    dt.Columns.Add(timeOutLatitudeLongitudeAddress);
                }

                if (!dt.Columns.Contains("maxDateTimeTracking")) //version cũ không có thông tin maxDateTimeTracking
                {
                    DataColumn maxDateTimeTracking = new DataColumn("maxDateTimeTracking");
                    maxDateTimeTracking.DataType = typeof(String);
                    maxDateTimeTracking.DefaultValue = "";
                    dt.Columns.Add(maxDateTimeTracking);
                }

                if (!dt.Columns.Contains("isLocationNull")) //version cũ không có thông tin isLocationNull
                {
                    DataColumn isLocationNull = new DataColumn("isLocationNull");
                    isLocationNull.DataType = typeof(String);
                    isLocationNull.DefaultValue = false;
                    dt.Columns.Add(isLocationNull);
                }


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["timeInLatitudeLongitudeAccuracy"].ToString() == "")
                    {
                        dt.Rows[i]["timeInLatitudeLongitudeAccuracy"] = "0";
                    }

                    if (dt.Rows[i]["timeOutLatitudeLongitudeAccuracy"].ToString() == "")
                    {
                        dt.Rows[i]["timeOutLatitudeLongitudeAccuracy"] = "0";
                    }

                }





                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "O_TIME_IN_OUT";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInOutCustomerCode", "CUSTOMER_CODE"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitude", "TIME_IN_LATITUDE_LONGITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitudeAccuracy", "TIME_IN_LATITUDE_LONGITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInCreatedDate", "TIME_IN_CREATED_DATE"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitude", "TIME_OUT_LATITUDE_LONGITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitudeAccuracy", "TIME_OUT_LATITUDE_LONGITUDE_ACCURACY"));



                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutCreatedDate", "TIME_OUT_CREATED_DATE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitudeAddress", "TIME_IN_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitudeAddress", "TIME_OUT_LOCATION_ADDRESS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("maxDateTimeTracking", "MAX_DATETIME_TRACKING"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("isLocationNull", "LOCATION_IS_NULL"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        //code cập nhật CD của Khách hàng dựa vào mã KH
                        //code cập nhật CD tuyến của Khách hàng dựa vào mã Tuyến
                        L5sSql.Execute(@"   UPDATE timeInOut SET CUSTOMER_CD = cust.CUSTOMER_CD, DISTRIBUTOR_CD = cust.DISTRIBUTOR_CD
                                            FROM O_TIME_IN_OUT timeInOut INNER JOIN M_CUSTOMER cust ON timeInOut.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                            WHERE ( timeInOut.CUSTOMER_CD IS NULL OR  timeInOut.DISTRIBUTOR_CD IS NULL  ) AND TYPE_CD = 3 
                                                    AND SALES_CD = @0

                                            UPDATE timeInOut SET ROUTE_CD = custR.ASM_ROUTE_CD
                                            FROM O_TIME_IN_OUT timeInOut INNER JOIN M_CUSTOMER cust ON timeInOut.CUSTOMER_CD = cust.CUSTOMER_CD
	                                                INNER JOIN O_CUSTOMER_ASM_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
                                            WHERE timeInOut.ROUTE_CD IS NULL AND TYPE_CD = 3 AND SALES_CD = @0

                                    ", asmCD);

                        return "1";
                    }
                    catch (Exception ex)
                    {
                        //  L5sMsg.Show(ex.Message);
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }
        private string synchronizePOCTimeInOutForSup(string obj, string type, string jsonData)
        {

            DataTable dtSup = L5sSql.Query("SELECT  * FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0", obj);
            if (dtSup.Rows.Count <= 0)
                return "-1";

            if (jsonData.Trim().Length == 0)
                return "-1";

            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String supervisorCd = dtSup.Rows[0]["SUPERVISOR_CD"].ToString();
                String typeCD = type;

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = supervisorCd;

                DataColumn columnTypeCD = new DataColumn("TYPE_CD");
                columnTypeCD.DataType = typeof(String);
                columnTypeCD.DefaultValue = typeCD;

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnTypeCD);


                if (!dt.Columns.Contains("timeInLatitudeLongitudeAddress")) //version cũ không có thông tin timeInLatitudeLongitudeAddress
                {
                    DataColumn timeInLatitudeLongitudeAddress = new DataColumn("timeInLatitudeLongitudeAddress");
                    timeInLatitudeLongitudeAddress.DataType = typeof(String);
                    dt.Columns.Add(timeInLatitudeLongitudeAddress);
                }

                if (!dt.Columns.Contains("timeOutLatitudeLongitudeAddress")) //version cũ không có thông tin timeOutLatitudeLongitudeAddress
                {
                    DataColumn timeOutLatitudeLongitudeAddress = new DataColumn("timeOutLatitudeLongitudeAddress");
                    timeOutLatitudeLongitudeAddress.DataType = typeof(String);
                    dt.Columns.Add(timeOutLatitudeLongitudeAddress);
                }


                if (!dt.Columns.Contains("maxDateTimeTracking")) //version cũ không có thông tin maxDateTimeTracking
                {
                    DataColumn maxDateTimeTracking = new DataColumn("maxDateTimeTracking");
                    maxDateTimeTracking.DataType = typeof(String);
                    maxDateTimeTracking.DefaultValue = "";
                    dt.Columns.Add(maxDateTimeTracking);
                }

                if (!dt.Columns.Contains("isLocationNull")) //version cũ không có thông tin isLocationNull
                {
                    DataColumn isLocationNull = new DataColumn("isLocationNull");
                    isLocationNull.DataType = typeof(String);
                    isLocationNull.DefaultValue = false;
                    dt.Columns.Add(isLocationNull);
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["timeInLatitudeLongitudeAccuracy"].ToString() == "")
                    {
                        dt.Rows[i]["timeInLatitudeLongitudeAccuracy"] = "0";
                    }

                    if (dt.Rows[i]["timeOutLatitudeLongitudeAccuracy"].ToString() == "")
                    {
                        dt.Rows[i]["timeOutLatitudeLongitudeAccuracy"] = "0";
                    }

                }




                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "O_TIME_IN_OUT";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInOutCustomerCode", "CUSTOMER_CODE"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitude", "TIME_IN_LATITUDE_LONGITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitudeAccuracy", "TIME_IN_LATITUDE_LONGITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInCreatedDate", "TIME_IN_CREATED_DATE"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitude", "TIME_OUT_LATITUDE_LONGITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitudeAccuracy", "TIME_OUT_LATITUDE_LONGITUDE_ACCURACY"));



                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutCreatedDate", "TIME_OUT_CREATED_DATE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitudeAddress", "TIME_IN_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitudeAddress", "TIME_OUT_LOCATION_ADDRESS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("maxDateTimeTracking", "MAX_DATETIME_TRACKING"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("isLocationNull", "LOCATION_IS_NULL"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        //code cập nhật CD của Khách hàng dựa vào mã KH
                        //code cập nhật CD tuyến của Khách hàng dựa vào mã Tuyến

                        L5sSql.Execute(@"
                                            UPDATE timeInOut SET CUSTOMER_CD = cust.CUSTOMER_CD, DISTRIBUTOR_CD = cust.DISTRIBUTOR_CD
                                            FROM O_TIME_IN_OUT timeInOut INNER JOIN M_CUSTOMER cust ON timeInOut.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                            WHERE ( timeInOut.CUSTOMER_CD IS NULL OR timeInOut.DISTRIBUTOR_CD IS NULL )   AND SALES_CD = @0 AND TYPE_CD = 2

                                            UPDATE timeInOut SET ROUTE_CD = custR.SUPERVISOR_ROUTE_CD
                                            FROM O_TIME_IN_OUT timeInOut INNER JOIN M_CUSTOMER cust ON timeInOut.CUSTOMER_CD = cust.CUSTOMER_CD
	                                                INNER JOIN O_CUSTOMER_SUPERVISOR_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
                                            WHERE timeInOut.ROUTE_CD IS NULL  AND SALES_CD = @0 AND TYPE_CD = 2

                                            ", supervisorCd);




                        return "1";
                    }
                    catch (Exception ex)
                    {
                        // L5sMsg.Show(ex.Message);
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }
        private string synchronizePOCTimeInOutForSales(string obj, string type, string jsonData)
        {
            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";

            if (jsonData.Trim().Length == 0)
                return "-1";

            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
                String distributorCD = dtSales.Rows[0]["DISTRIBUTOR_CD"].ToString();

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = salesCD;

                DataColumn columnDistributorCD = new DataColumn("DISTRIBUTOR_CD");
                columnDistributorCD.DataType = typeof(String);
                columnDistributorCD.DefaultValue = distributorCD;

                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;


                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnDistributorCD);
                dt.Columns.Add(typeCD);

                if (!dt.Columns.Contains("timeInLatitudeLongitudeAddress")) //version cũ không có thông tin timeInLatitudeLongitudeAddress
                {
                    DataColumn timeInLatitudeLongitudeAddress = new DataColumn("timeInLatitudeLongitudeAddress");
                    timeInLatitudeLongitudeAddress.DataType = typeof(String);
                    dt.Columns.Add(timeInLatitudeLongitudeAddress);
                }

                if (!dt.Columns.Contains("timeOutLatitudeLongitudeAddress")) //version cũ không có thông tin timeOutLatitudeLongitudeAddress
                {
                    DataColumn timeOutLatitudeLongitudeAddress = new DataColumn("timeOutLatitudeLongitudeAddress");
                    timeOutLatitudeLongitudeAddress.DataType = typeof(String);
                    dt.Columns.Add(timeOutLatitudeLongitudeAddress);
                }

                if (!dt.Columns.Contains("maxDateTimeTracking")) //version cũ không có thông tin maxDateTimeTracking
                {
                    DataColumn maxDateTimeTracking = new DataColumn("maxDateTimeTracking");
                    maxDateTimeTracking.DataType = typeof(String);
                    maxDateTimeTracking.DefaultValue = "";
                    dt.Columns.Add(maxDateTimeTracking);
                }

                if (!dt.Columns.Contains("isLocationNull")) //version cũ không có thông tin isLocationNull
                {
                    DataColumn isLocationNull = new DataColumn("isLocationNull");
                    isLocationNull.DataType = typeof(String);
                    isLocationNull.DefaultValue = false;
                    dt.Columns.Add(isLocationNull);
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["timeInLatitudeLongitudeAccuracy"].ToString() == "")
                    {
                        dt.Rows[i]["timeInLatitudeLongitudeAccuracy"] = 0;
                    }

                    if (dt.Rows[i]["timeOutLatitudeLongitudeAccuracy"].ToString() == "")
                    {
                        dt.Rows[i]["timeOutLatitudeLongitudeAccuracy"] = 0;
                    }

                }



                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "O_TIME_IN_OUT";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInOutCustomerCode", "CUSTOMER_CODE"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitude", "TIME_IN_LATITUDE_LONGITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitudeAccuracy", "TIME_IN_LATITUDE_LONGITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInCreatedDate", "TIME_IN_CREATED_DATE"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitude", "TIME_OUT_LATITUDE_LONGITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitudeAccuracy", "TIME_OUT_LATITUDE_LONGITUDE_ACCURACY"));



                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutCreatedDate", "TIME_OUT_CREATED_DATE"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeInLatitudeLongitudeAddress", "TIME_IN_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("timeOutLatitudeLongitudeAddress", "TIME_OUT_LOCATION_ADDRESS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("maxDateTimeTracking", "MAX_DATETIME_TRACKING"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("isLocationNull", "LOCATION_IS_NULL"));



                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        //code cập nhật CD của Khách hàng dựa vào mã KH
                        //code cập nhật CD tuyến của Khách hàng dựa vào mã Tuyến

                        L5sSql.Execute(@"
                                            UPDATE timeInOut SET CUSTOMER_CD = cust.CUSTOMER_CD,DISTRIBUTOR_CD = cust.DISTRIBUTOR_CD
                                            FROM O_TIME_IN_OUT timeInOut INNER JOIN M_CUSTOMER cust ON timeInOut.CUSTOMER_CODE = cust.CUSTOMER_CODE
                                            WHERE ( timeInOut.CUSTOMER_CD IS NULL OR timeInOut.CUSTOMER_CD IS NULL )  AND SALES_CD = @0 AND TYPE_CD = 1
        

                                         UPDATE timeInOut SET ROUTE_CD = custR.ROUTE_CD
                                            FROM O_TIME_IN_OUT timeInOut INNER JOIN M_CUSTOMER cust ON timeInOut.CUSTOMER_CD = cust.CUSTOMER_CD
	                                             INNER JOIN O_CUSTOMER_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
                                            WHERE timeInOut.ROUTE_CD IS NULL    AND SALES_CD = @0 AND TYPE_CD = 1

                                            ", salesCD);

                        return "1";
                    }
                    catch (Exception ex)
                    {
                        // L5sMsg.Show(ex.Message);
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }

        #endregion
        #region synchronize tracking: hàm đồng bộ dữ liệu tracking của NVBH, CDS, ASM

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCTracking(String imei, String obj, String type, String jsonData)
        {
            try
            {

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";


                if (jsonData.Trim().Length == 0)
                    return "-1";


                jsonData = P5sDmComm.P5sSecurity.Decrypt(jsonData);

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCTrackingForSales(obj, type, jsonData);
                    case "2": // Sup
                        return this.synchronizePOCTrackingForSup(obj, type, jsonData);
                    case "3": //ASM
                        return this.synchronizePOCTrackingForASM(obj, type, jsonData);
                    default:
                        return "-1";
                }


            }
            catch (Exception Ex)
            {
                // L5sMsg.Show(Ex.Message);
                return "-1";
            }

        }

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCTrackingNotEncrypt(String imei, String obj, String type, String jsonData, String sK)
        {
            try
            {

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";


                if (jsonData.Trim().Length == 0)
                    return "-1";

                if (sK.Trim().Length == 0)
                    return "-1";

                if (!P5sDmComm.P5sSecurity.IsValidKey(sK))
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCTrackingForSales(obj, type, jsonData);
                    case "2": // Sup
                        return this.synchronizePOCTrackingForSup(obj, type, jsonData);
                    case "3": //ASM
                        return this.synchronizePOCTrackingForASM(obj, type, jsonData);
                    default:
                        return "-1";
                }


            }
            catch (Exception Ex)
            {
                // L5sMsg.Show(Ex.Message);
                return "-1";
            }

        }


        private string synchronizePOCTrackingForASM(string obj, string type, string jsonData)
        {

            DataTable dtASM = L5sSql.Query("SELECT  * FROM M_ASM WHERE ASM_CODE = @0", obj);
            if (dtASM.Rows.Count <= 0)
                return "-1";

            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String asmCD = dtASM.Rows[0]["ASM_CD"].ToString();
                String typeCD = type;// M_TYPE

                //add new column
                DataColumn columnSalesCD = new DataColumn("ASM_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = asmCD;

                DataColumn columnTypeCD = new DataColumn("TYPE_CD");
                columnTypeCD.DataType = typeof(String);
                columnTypeCD.DefaultValue = typeCD;

                DataColumn colYYMMDD = new DataColumn("YYMMDD");
                colYYMMDD.DataType = typeof(String);
                colYYMMDD.ReadOnly = false;


                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnTypeCD);
                dt.Columns.Add(colYYMMDD);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        //chuyển thông tin thời gian tracking thành định dạng YYMMDD mục đích xử dụng cho chốt số
                        dt.Rows[i]["YYMMDD"] = DateTime.Parse(dt.Rows[i]["trackingCreatedDate"].ToString()).ToString("yyMMdd");

                    }
                    catch (Exception)
                    {

                    }
                }

                //enable edit value
                dt.Columns["trackingDeviceStatus"].ReadOnly = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["trackingDeviceStatus"].ToString() != "")
                    {
                        dt.Rows[i]["trackingDeviceStatus"] = dt.Rows[i]["trackingDeviceStatus"] + String.Format(" ({0}) ", DateTime.Parse(dt.Rows[i]["trackingCreatedDate"].ToString()).ToString("hh:mm:ss tt"));
                    }

                }

                if (!dt.Columns.Contains("trackingAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("trackingAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "O_TRACKING_ASM";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ASM_CD", "ASM_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingLatitudeLongitude", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "TRACKING_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingDeviceStatus", "DEVICE_STATUS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAccuracy", "TRACKING_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingProvider", "TRACKING_PROVIDER"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "BEGIN_DATETIME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "END_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_START"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_END"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YYMMDD", "YYMMDD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAddress", "LOCATION_ADDRESS"));

                    try
                    {
                        bulkcopy.WriteToServer(dt);

                        //                            L5sSql.Execute(@" UPDATE O_TRACKING_ASM SET YYMMDD =  CONVERT ( nvarchar(6) , TRACKING_DATETIME, 12 )  
                        //                                            WHERE YYMMDD IS NULL AND ASM_CD = @0 ", asmCD);

                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }

        private string synchronizePOCTrackingForSup(string obj, string type, string jsonData)
        {
            DataTable dtSup = L5sSql.Query("SELECT  * FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0", obj);
            if (dtSup.Rows.Count <= 0)
                return "-1";

            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String supervisorCD = dtSup.Rows[0]["SUPERVISOR_CD"].ToString();
                String typeCD = type; // M_TYPE

                //add new column
                DataColumn columnSalesCD = new DataColumn("SUPERVISOR_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = supervisorCD;

                DataColumn columnTypeCD = new DataColumn("TYPE_CD");
                columnTypeCD.DataType = typeof(String);
                columnTypeCD.DefaultValue = typeCD;

                DataColumn colYYMMDD = new DataColumn("YYMMDD");
                colYYMMDD.DataType = typeof(String);
                colYYMMDD.ReadOnly = false;

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnTypeCD);
                dt.Columns.Add(colYYMMDD);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        //chuyển thông tin thời gian tracking thành định dạng YYMMDD mục đích xử dụng cho chốt số
                        dt.Rows[i]["YYMMDD"] = DateTime.Parse(dt.Rows[i]["trackingCreatedDate"].ToString()).ToString("yyMMdd");

                    }
                    catch (Exception)
                    {

                    }

                }

                //enable edit value
                dt.Columns["trackingDeviceStatus"].ReadOnly = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["trackingDeviceStatus"].ToString() != "")
                    {
                        dt.Rows[i]["trackingDeviceStatus"] = dt.Rows[i]["trackingDeviceStatus"] + String.Format(" ({0}) ", DateTime.Parse(dt.Rows[i]["trackingCreatedDate"].ToString()).ToString("hh:mm:ss tt"));
                    }

                }

                if (!dt.Columns.Contains("trackingAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("trackingAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }


                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "O_TRACKING_SUPERVISOR";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SUPERVISOR_CD", "SUPERVISOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingLatitudeLongitude", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "TRACKING_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingDeviceStatus", "DEVICE_STATUS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAccuracy", "TRACKING_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingProvider", "TRACKING_PROVIDER"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "BEGIN_DATETIME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "END_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_START"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_END"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YYMMDD", "YYMMDD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAddress", "LOCATION_ADDRESS"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);

                        //                            L5sSql.Execute(@" UPDATE O_TRACKING_SUPERVISOR SET YYMMDD = CONVERT ( nvarchar(6) , TRACKING_DATETIME, 12 ) 
                        //                                            WHERE YYMMDD IS NULL AND  SUPERVISOR_CD = @0 ", supervisorCD);

                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }

        private string synchronizePOCTrackingForSales(string obj, string type, string jsonData)
        {
            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";

            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
                String distributorCD = dtSales.Rows[0]["DISTRIBUTOR_CD"].ToString();

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = salesCD;

                DataColumn columnDistributorCD = new DataColumn("DISTRIBUTOR_CD");
                columnDistributorCD.DataType = typeof(String);
                columnDistributorCD.DefaultValue = distributorCD;

                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;

                DataColumn colYYMMDD = new DataColumn("YYMMDD");
                colYYMMDD.DataType = typeof(String);
                colYYMMDD.ReadOnly = false;

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnDistributorCD);
                dt.Columns.Add(typeCD);
                dt.Columns.Add(colYYMMDD);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        //chuyển thông tin thời gian tracking thành định dạng YYMMDD mục đích xử dụng cho chốt số
                        dt.Rows[i]["YYMMDD"] = DateTime.Parse(dt.Rows[i]["trackingCreatedDate"].ToString()).ToString("yyMMdd");
                    }
                    catch (Exception)
                    {

                    }
                }




                //enable edit value
                dt.Columns["trackingDeviceStatus"].ReadOnly = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["trackingDeviceStatus"].ToString() != "")
                    {
                        dt.Rows[i]["trackingDeviceStatus"] = dt.Rows[i]["trackingDeviceStatus"] + String.Format(" ({0}) ", DateTime.Parse(dt.Rows[i]["trackingCreatedDate"].ToString()).ToString("hh:mm:ss tt"));
                    }

                }


                if (!dt.Columns.Contains("trackingAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("trackingAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }


                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "O_TRACKING";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingLatitudeLongitude", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "TRACKING_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingDeviceStatus", "DEVICE_STATUS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAccuracy", "TRACKING_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingProvider", "TRACKING_PROVIDER"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "BEGIN_DATETIME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "END_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_START"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_END"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YYMMDD", "YYMMDD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAddress", "LOCATION_ADDRESS"));

                    try
                    {
                        bulkcopy.WriteToServer(dt);

                        //                            L5sSql.Execute(@" UPDATE O_TRACKING SET YYMMDD = CONVERT ( nvarchar(6) , TRACKING_DATETIME, 12 )  
                        //                                            WHERE YYMMDD IS NULL AND SALES_CD = @0 ", salesCD);

                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }


        #endregion
        #region synchronize tracking full: hàm đồng bộ dữ liệu tracking của NVBH, CDS, ASM full (ghi nhận tất cả ) hiện tại đã không còn xử dụng nữa

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCTrackingFull(String imei, String obj, String type, String jsonData)
        {
            try
            {

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";


                if (jsonData.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCTrackingFullForSales(obj, type, jsonData);
                    case "2": // Sup
                        return this.synchronizePOCTrackingFullForSup(obj, type, jsonData);
                    case "3": //ASM
                        return this.synchronizePOCTrackingFullForASM(obj, type, jsonData);
                    default:
                        return "-1";
                }


            }
            catch (Exception Ex)
            {
                // L5sMsg.Show(Ex.Message);
                return "-1";
            }

        }

        private string synchronizePOCTrackingFullForASM(string obj, string type, string jsonData)
        {
            DataTable dtASM = L5sSql.Query("SELECT  * FROM M_ASM WHERE ASM_CODE = @0", obj);
            if (dtASM.Rows.Count <= 0)
                return "-1";


            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String salesCD = dtASM.Rows[0]["ASM_CD"].ToString();
                String typeCD = type;

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = salesCD;

                DataColumn columnTypeCD = new DataColumn("TYPE_CD");
                columnTypeCD.DataType = typeof(String);
                columnTypeCD.DefaultValue = typeCD;

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnTypeCD);

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = String.Format("O_TRACKING_FULL");


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingLatitudeLongitude", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "TRACKING_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingDeviceStatus", "DEVICE_STATUS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAccuracy", "TRACKING_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingProvider", "TRACKING_PROVIDER"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "BEGIN_DATETIME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "END_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_START"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_END"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);

                        String sql = String.Format(@"  
                                            INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TRACKING_FULL]
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
                                            SELECT      [SALES_CD]
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
                                            FROM O_TRACKING_FULL
                                            WHERE SALES_CD = {0} AND TYPE_CD = {1}
        
                                            DELETE FROM O_TRACKING_FULL
                                            WHERE SALES_CD = {0} AND TYPE_CD = {1}

                             ", salesCD, typeCD);

                        L5sSql.Execute(sql);

                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }

        }

        private string synchronizePOCTrackingFullForSup(string obj, string type, string jsonData)
        {
            DataTable dtSupervisor = L5sSql.Query("SELECT  * FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0", obj);
            if (dtSupervisor.Rows.Count <= 0)
                return "-1";




            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String salesCD = dtSupervisor.Rows[0]["SUPERVISOR_CD"].ToString();
                String typeCD = type;

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = salesCD;

                DataColumn columnTypeCD = new DataColumn("TYPE_CD");
                columnTypeCD.DataType = typeof(String);
                columnTypeCD.DefaultValue = typeCD;

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnTypeCD);

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = String.Format("O_TRACKING_FULL");


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingLatitudeLongitude", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "TRACKING_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingDeviceStatus", "DEVICE_STATUS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAccuracy", "TRACKING_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingProvider", "TRACKING_PROVIDER"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "BEGIN_DATETIME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "END_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_START"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_END"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        String sql = String.Format(@"  
                                            INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TRACKING_FULL]
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
                                            SELECT      [SALES_CD]
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
                                            FROM O_TRACKING_FULL
                                            WHERE SALES_CD = {0} AND TYPE_CD = {1}
        
                                        --    UPDATE {0}.[O_TRACKING_FULL] SET YYMMDD = CONVERT ( nvarchar(6) , BEGIN_DATETIME, 12 ) 
                                        --    WHERE SALES_CD = {0} AND TYPE_CD = {1} AND YYMMDD IS NULL

                                            DELETE FROM O_TRACKING_FULL
                                            WHERE SALES_CD = {0} AND TYPE_CD = {1}

                             ", salesCD, typeCD);

                        L5sSql.Execute(sql);

                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }

        }

        private string synchronizePOCTrackingFullForSales(string obj, string type, string jsonData)
        {
            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";

            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
                String distributorCD = dtSales.Rows[0]["DISTRIBUTOR_CD"].ToString();

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = salesCD;

                DataColumn columnDistributorCD = new DataColumn("DISTRIBUTOR_CD");
                columnDistributorCD.DataType = typeof(String);
                columnDistributorCD.DefaultValue = distributorCD;


                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnDistributorCD);
                dt.Columns.Add(typeCD);

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "O_TRACKING_FULL";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));

                    //bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YYMMDD", "YYMMDD"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingLatitudeLongitude", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "TRACKING_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingDeviceStatus", "DEVICE_STATUS"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingAccuracy", "TRACKING_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingProvider", "TRACKING_PROVIDER"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "BEGIN_DATETIME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingCreatedDate", "END_DATETIME"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_START"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE_END"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("trackingBatteryPercentage", "BATTERY_PERCENTAGE"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        String sql = String.Format(@"  
                                            INSERT INTO [SQLSERVER_MMV].[MMV_CONSOLIDATE_SALES].[dbo].[O_TRACKING_FULL]
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
                                            SELECT      [SALES_CD]
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
                                            FROM O_TRACKING_FULL
                                            WHERE SALES_CD = {0} AND TYPE_CD = {1}
        
                                            DELETE FROM O_TRACKING_FULL
                                            WHERE SALES_CD = {0} AND TYPE_CD = {1}

                             ", salesCD, typeCD);

                        L5sSql.Execute(sql);

                        return "1";
                    }
                    catch (Exception ex)
                    {
                        // L5sMsg.Show(ex.Message);
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }


        #endregion

        #region get latitude and logitude exists on DB: hàm lấy các thông tin về tọa độ của KH và gửi xuống HH . Khi HH đồng bộ thì sẽ lấy tọa độ các Kh mà minh đang quản lý từ hệ thống
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetInfoCustomer(String imei, String obj, String type, String customerCodes)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            if (obj.Trim().Length == 0)
                return "-1";


            switch (type)
            {
                case "1": // Sales
                    return this.getPOCInfoCustomerBySales(obj, type, customerCodes);
                case "2": // Sup
                    return "-1";
                case "3": //ASM
                    return "-1";
                default:
                    return "-1";
            }

        }

        private string getPOCInfoCustomerBySales(String obj, string type, String customerCodes)
        {
            String sql = String.Format(@"
            SELECT * FROM
            (
                 SELECT DISTINCT CUSTOMER_CODE, CUSTOMER_NAME, CUSTOMER_ADDRESS, ISNULL(PHONE_NUMBER,'') AS PHONE_NUMBER, CUSTOMER_CHAIN_CODE, LONGITUDE_LATITUDE, ISNULL(LONGITUDE_LATITUDE_ACCURACY,0) AS LONGITUDE_LATITUDE_ACCURACY
                                                 ,cust.ACTIVE,rout.ROUTE_CODE
                                        FROM M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE custR ON cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
                                                      INNER JOIN O_SALES_ROUTE slsR ON custR.ROUTE_CD = slsR.ROUTE_CD AND slsR.ACTIVE = 1
			                                          INNER JOIN M_SALES sls ON slsR.SALES_CD = sls.SALES_CD
                                                      INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD 
                                        WHERE  LONGITUDE_LATITUDE != ''

            ) AS T
            WHERE CUSTOMER_CODE IN ('{0}')", customerCodes.Replace("@", "','"));

            DataTable dt = L5sSql.Query(sql);

            if (dt == null || dt.Rows.Count == 0)
                return "1";




            return this.POCP5sConvertCustomerToJson(dt);

        }
        private String POCP5sConvertCustomerToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            List<POCCCustomer> customers = new List<POCCCustomer>();
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
                String CustomerRoute = dt.Rows[i]["ROUTE_CODE"].ToString();
                String CustomerActive = dt.Rows[i]["ACTIVE"].ToString();


                POCCCustomer c = new POCCCustomer(CustomerCode, CustomerName, CustomerAddress, CustomerPhoneNumber, CustomerChainCode, CustomerLatitudeLongitude, CustomerLatitudeLongitudeAccuracy, CustomerActive, CustomerRoute);
                customers.Add(c);
            }

            return oSerializer.Serialize(customers);
        }
        public class POCCCustomer
        {
            public String CustomerCode = "";
            public String CustomerName = "";
            public String CustomerAddress = "";
            public String CustomerPhoneNumber = "";
            public String CustomerChainCode = "";
            public String CustomerLatitudeLongitude = "";
            public String CustomerLatitudeLongitudeAccuracy = "";
            public String CustomerActive = "";
            public String CustomerRoute = "";
            public string Cus_Add = "";
            public POCCCustomer(String CustomerCode, String CustomerName, String CustomerAddress, String CustomerPhoneNumber, String CustomerChainCode, String CustomerLatitudeLongitude, String CustomerLatitudeLongitudeAccuracy, String CustomerActive, String CustomerRoute)
            {
                this.CustomerCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerCode);
                this.CustomerName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerName);
                this.CustomerAddress = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerAddress);
                this.CustomerPhoneNumber = CustomerPhoneNumber;
                this.CustomerChainCode = CustomerChainCode;
                this.CustomerLatitudeLongitude = CustomerLatitudeLongitude;
                this.CustomerLatitudeLongitudeAccuracy = CustomerLatitudeLongitudeAccuracy;
                this.CustomerActive = CustomerActive;
                this.CustomerRoute = CustomerRoute;
            }
            public POCCCustomer(String CustomerCode, String Cus_Add, String CustomerName, String CustomerAddress, String CustomerChainCode)
            {
                this.CustomerCode = CustomerCode;
                this.Cus_Add = Cus_Add;
                this.CustomerName = CustomerName;
                this.CustomerAddress = CustomerAddress;
                this.CustomerChainCode = CustomerChainCode;
            }

        }



        #endregion
        #region  synchronize GPSStatus: đồng bộ trạng thái GPS từ HH lên Server mục đích dùng để lưu log quá trình bật tắt GPS của User.
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGPSStatus(String imei, String obj, String type, String jsonData)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            if (obj.Trim().Length == 0)
                return "-1";


            if (jsonData.Trim().Length == 0)
                return "-1";

            switch (type)
            {
                case "1": // Sales
                    return this.synchronizePOCGPSStatusBySales(obj, type, jsonData);
                case "2": // Sup
                    return this.synchronizePOCGPSStatusBySup(obj, type, jsonData);
                case "3": //ASM
                    return this.synchronizePOCGPSStatusByASM(obj, type, jsonData);
                default:
                    return "-1";
            }

        }
        private string synchronizePOCGPSStatusByASM(string obj, string type, string jsonData)
        {
            try
            {

                DataTable dtASM = L5sSql.Query("SELECT  * FROM M_ASM WHERE ASM_CODE = @0", obj);
                if (dtASM.Rows.Count <= 0)
                    return "-1";


                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                String supervisorCD = dtASM.Rows[0]["ASM_CD"].ToString();

                //add new column
                DataColumn columnASMervisorCD = new DataColumn("ASM_CD");
                columnASMervisorCD.DataType = typeof(String);
                columnASMervisorCD.DefaultValue = supervisorCD;


                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;

                dt.Columns.Add(columnASMervisorCD);
                dt.Columns.Add(typeCD);


                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "H_GPS_STATUS";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ASM_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gpsStatus", "GPS_STATUS_DESCRIPTION"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gpsCreatedDate", "GPS_STATUS_CREATED_DATE"));

                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }
        private string synchronizePOCGPSStatusBySup(string obj, string type, string jsonData)
        {
            try
            {
                DataTable dtSupervisor = L5sSql.Query("SELECT  * FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0", obj);
                if (dtSupervisor.Rows.Count <= 0)
                    return "-1";

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);



                String supervisorCD = dtSupervisor.Rows[0]["SUPERVISOR_CD"].ToString();

                //add new column
                DataColumn columnSupervisorCD = new DataColumn("SUPERVISOR_CD");
                columnSupervisorCD.DataType = typeof(String);
                columnSupervisorCD.DefaultValue = supervisorCD;


                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;

                dt.Columns.Add(columnSupervisorCD);
                dt.Columns.Add(typeCD);


                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "H_GPS_STATUS";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SUPERVISOR_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gpsStatus", "GPS_STATUS_DESCRIPTION"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gpsCreatedDate", "GPS_STATUS_CREATED_DATE"));

                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }
        }
        private string synchronizePOCGPSStatusBySales(string obj, string type, string jsonData)
        {
            try
            {
                DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
                if (dtSales.Rows.Count <= 0)
                    return "-1";

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
                String distributorCD = dtSales.Rows[0]["DISTRIBUTOR_CD"].ToString();

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = salesCD;

                DataColumn columnDistributorCD = new DataColumn("DISTRIBUTOR_CD");
                columnDistributorCD.DataType = typeof(String);
                columnDistributorCD.DefaultValue = distributorCD;

                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnDistributorCD);
                dt.Columns.Add(typeCD);


                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    //I assume you have created the table previously 
                    //Someone else here already showed how   
                    bulkcopy.DestinationTableName = "H_GPS_STATUS";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gpsStatus", "GPS_STATUS_DESCRIPTION"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("gpsCreatedDate", "GPS_STATUS_CREATED_DATE"));

                    try
                    {
                        bulkcopy.WriteToServer(dt);
                        return "1";
                    }
                    catch
                    {
                        return "-1";
                    }
                }

                return "-1";

            }
            catch (Exception)
            {
                return "-1";
            }

        }
        #endregion

        #region  synchronize Add new customer: hàm đồng bộ các KH mới từ HH gửi lên Server, các KH mới này được thêm trực tiếp từ ứng dụng FPIT, thông tin này sẽ được Admin xuất ra và import trở lại hệ thống


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCCustomerAddNew(String imei, String obj, String type, String jsonData)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            if (obj.Trim().Length == 0)
                return "-1";


            if (jsonData.Trim().Length == 0)
                return "-1";

            switch (type)
            {
                case "1": // Sales
                    return this.synchronizePOCAddnewCustomerBySales(obj, type, jsonData);
                case "2": // Sup
                    return this.synchronizePOCAddnewCustomerBySup(obj, type, jsonData);
                case "3": //ASM
                    return this.synchronizePOCAddnewCustomerASM(obj, type, jsonData);
                default:
                    return "-1";
            }

        }

        private string synchronizePOCAddnewCustomerASM(string obj, string type, string jsonData)
        {
            DataTable dtASMervisor = L5sSql.Query("SELECT  * FROM M_ASM WHERE ASM_CODE = @0", obj);
            if (dtASMervisor.Rows.Count <= 0)
                return "-1";


            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                //DataTable dt = new DataTable();

                String supervisorCD = dtASMervisor.Rows[0]["ASM_CD"].ToString();

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = supervisorCD;


                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;

                //add new column
                String sessionCD = System.Guid.NewGuid().ToString();
                DataColumn colSessionCD = new DataColumn("SESSION_CD");
                colSessionCD.DataType = typeof(String);
                colSessionCD.DefaultValue = sessionCD;

                //dt.Columns.Add("customerCode", typeof(String));
                //dt.Columns.Add("customerName", typeof(String));
                //dt.Columns.Add("customerAddress", typeof(String));
                //dt.Columns.Add("customerCategory", typeof(String));
                //dt.Columns.Add("customerRoute", typeof(String));
                //dt.Columns.Add("customerCreatedDate", typeof(DateTime));
                //dt.Columns.Add("customerLocation", typeof(String));
                //dt.Columns.Add("customerLocationAccuracy", typeof(float));
                //dt.Columns.Add("customerActive", typeof(Boolean));

                //dt.Rows.Add("TTP", "TRẦN TẤN PHƯỚ 11C", "18/5C KHU PHỐ 1 -XXX", "SGN", "NT001	Nha Trang", DateTime.Now, "2232", 0, true);

                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(typeCD);
                dt.Columns.Add(colSessionCD);




                //version add new column customerInfo1 -> customerInfo5
                if (!dt.Columns.Contains("customerInfo1")) //version cũ không có thông tin customerInfo1 -> customerInfo5
                //thêm column vào DataTable
                {
                    DataColumn customerInfo1 = new DataColumn("customerInfo1");
                    customerInfo1.DataType = typeof(String);

                    DataColumn customerInfo2 = new DataColumn("customerInfo2");
                    customerInfo2.DataType = typeof(String);

                    DataColumn customerInfo3 = new DataColumn("customerInfo3");
                    customerInfo3.DataType = typeof(String);

                    DataColumn customerInfo4 = new DataColumn("customerInfo4");
                    customerInfo4.DataType = typeof(String);

                    DataColumn customerInfo5 = new DataColumn("customerInfo5");
                    customerInfo5.DataType = typeof(String);

                    dt.Columns.Add(customerInfo1);
                    dt.Columns.Add(customerInfo2);
                    dt.Columns.Add(customerInfo3);
                    dt.Columns.Add(customerInfo4);
                    dt.Columns.Add(customerInfo5);
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        dt.Rows[i]["customerCode"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerCode"].ToString());
                        dt.Rows[i]["customerName"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerName"].ToString());
                        dt.Rows[i]["customerAddress"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerAddress"].ToString());
                    }
                    catch (Exception)
                    {

                    }

                }

                if (!dt.Columns.Contains("customerLocationAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("customerLocationAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }

                if (!dt.Columns.Contains("customerPhoneNumber")) //version cũ không có thông tin customerPhoneNumber
                {
                    DataColumn customerPhoneNumber = new DataColumn("customerPhoneNumber");
                    customerPhoneNumber.DataType = typeof(String);
                    dt.Columns.Add(customerPhoneNumber);
                }


                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TEMP_HH_CUSTOMER";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    //  bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCode", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerName", "CUSTOMER_NAME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerAddress", "CUSTOMER_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCategory", "CUSTOMER_CHAIN_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerRoute", "CUSTOMER_ROUTE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCreatedDate", "CUSTOMER_CREATED_DATE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocation", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAccuracy", "LONGITUDE_LATITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerActive", "ACTIVE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo1", "INFO_1"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo2", "INFO_2"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo3", "INFO_3"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo4", "INFO_4"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo5", "INFO_5"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAddress", "CUSTOMER_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerPhoneNumber", "PHONE_NUMBER"));

                    bulkcopy.WriteToServer(dt);
                    //SQL PROCESS
                    String sql = String.Format(@"
                                            
                                                --Cập nhật CUSTOMER_CD lần 
                                                    UPDATE T1 SET CUSTOMER_CD = T2.CUSTOMER_CD
                                                    FROM TEMP_HH_CUSTOMER T1 INNER JOIN M_HH_CUSTOMER T2 ON T1.CUSTOMER_CODE = T2.CUSTOMER_CODE  AND T2.TYPE_CD = T1.TYPE_CD  AND T1.SALES_CD = T2.SALES_CD 
                                                    WHERE T1.SESSION_CD = '{0}'  AND T1.CUSTOMER_CD IS NULL  
                                                    
                                                
                                                --GET DISTRIBUTOR BY ROUTE
                                            

                                                 UPDATE T1 SET ROUTE_CODE = SUBSTRING(CUSTOMER_ROUTE,0,CHARINDEX(CHAR(9),CUSTOMER_ROUTE,0))
                                                 FROM TEMP_HH_CUSTOMER T1
                                                 WHERE T1.SESSION_CD = '{0}'  AND T1.ROUTE_CODE IS NULL  

                                                 UPDATE T1 SET DISTRIBUTOR_CD = T2.DISTRIBUTOR_CD
                                                 FROM TEMP_HH_CUSTOMER T1 INNER JOIN M_ASM_ROUTE T2 ON T1.ROUTE_CODE = T2.ASM_ROUTE_CODE 
                                                 WHERE T1.SESSION_CD = '{0}'  AND T1.ROUTE_CODE IS NOT NULL  



                                               --THÊM MỚI CUSTOMER
                                                 INSERT INTO [dbo].[M_HH_CUSTOMER]
                                                       ([CUSTOMER_CODE]
                                                       ,[CUSTOMER_NAME]
                                                       ,[CUSTOMER_ADDRESS]
                                                       ,[PHONE_NUMBER]
                                                       ,[CUSTOMER_CHAIN_CODE]
                                                       ,[CUSTOMER_ROUTE]
                                                       ,[CUSTOMER_CREATED_DATE]
                                                       ,[LONGITUDE_LATITUDE]
                                                       ,[LONGITUDE_LATITUDE_ACCURACY]
                                                       ,[ACTIVE]
                                                       ,[SALES_CD]
                                                       ,[DISTRIBUTOR_CD]                                                   
                                                       ,[TYPE_CD],[INFO_1],[INFO_2],[INFO_3],[INFO_4],[INFO_5],[CUSTOMER_LOCATION_ADDRESS])
            
                                               SELECT [CUSTOMER_CODE]
                                                       ,[CUSTOMER_NAME]
                                                       ,[CUSTOMER_ADDRESS]
                                                       ,[PHONE_NUMBER]
                                                       ,[CUSTOMER_CHAIN_CODE]
                                                       ,[CUSTOMER_ROUTE]
                                                       ,[CUSTOMER_CREATED_DATE]
                                                       ,[LONGITUDE_LATITUDE]
                                                       ,[LONGITUDE_LATITUDE_ACCURACY]
                                                       ,[ACTIVE]
                                                       ,[SALES_CD]
                                                       ,[DISTRIBUTOR_CD]                                                   
                                                       ,[TYPE_CD],[INFO_1],[INFO_2],[INFO_3],[INFO_4],[INFO_5],[CUSTOMER_LOCATION_ADDRESS] 
                                               FROM TEMP_HH_CUSTOMER
                                               WHERE SESSION_CD = '{0}'  AND CUSTOMER_CD IS  NULL

                                             --UPDATE CUSTOMER
                                            
                                           UPDATE T1 SET CUSTOMER_NAME = T2.CUSTOMER_NAME, 
                                                        CUSTOMER_ADDRESS = T2.CUSTOMER_ADDRESS, 
                                                        PHONE_NUMBER = T2.PHONE_NUMBER, 
                                                        CUSTOMER_CHAIN_CODE = T2.CUSTOMER_CHAIN_CODE, 
                                                        CUSTOMER_ROUTE = T2.CUSTOMER_ROUTE, 
                                                        LONGITUDE_LATITUDE = T2.LONGITUDE_LATITUDE, 
                                                        LONGITUDE_LATITUDE_ACCURACY = T2.LONGITUDE_LATITUDE_ACCURACY, 
                                                        ACTIVE = T2.ACTIVE ,
                                                        INFO_1 = T2.INFO_1 ,
                                                        INFO_2 = T2.INFO_2 ,
                                                        INFO_3 = T2.INFO_3 ,
                                                        INFO_4 = T2.INFO_4 ,
                                                        INFO_5 = T2.INFO_5 ,
                                                        SALES_CD = T2.SALES_CD,
                                                        DISTRIBUTOR_CD = T2.DISTRIBUTOR_CD,
                                                        [CUSTOMER_LOCATION_ADDRESS] = T2.CUSTOMER_LOCATION_ADDRESS
                                         FROM M_HH_CUSTOMER  T1 INNER JOIN TEMP_HH_CUSTOMER  T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD AND T1.TYPE_CD = T2.TYPE_CD AND T1.SALES_CD = T2.SALES_CD 
                                         WHERE T2.CUSTOMER_CD IS NOT NULL AND  T2.SESSION_CD = '{0}'

            

                                        ", sessionCD);
                    L5sSql.Execute(sql);

                }

                return "1";
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        private string synchronizePOCAddnewCustomerBySup(string obj, string type, string jsonData)
        {
            DataTable dtSupervisor = L5sSql.Query("SELECT  * FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = @0", obj);
            if (dtSupervisor.Rows.Count <= 0)
                return "-1";




            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                //  DataTable dt = new DataTable();

                String supervisorCD = dtSupervisor.Rows[0]["SUPERVISOR_CD"].ToString();

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = supervisorCD;


                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;

                //add new column
                String sessionCD = System.Guid.NewGuid().ToString();
                DataColumn colSessionCD = new DataColumn("SESSION_CD");
                colSessionCD.DataType = typeof(String);
                colSessionCD.DefaultValue = sessionCD;




                //dt.Columns.Add("customerCode", typeof(String));
                //dt.Columns.Add("customerName", typeof(String));
                //dt.Columns.Add("customerAddress", typeof(String));
                //dt.Columns.Add("customerCategory", typeof(String));
                //dt.Columns.Add("customerRoute", typeof(String));
                //dt.Columns.Add("customerCreatedDate", typeof(DateTime));
                //dt.Columns.Add("customerLocation", typeof(String));
                //dt.Columns.Add("customerLocationAccuracy", typeof(float));
                //dt.Columns.Add("customerActive", typeof(Boolean));
                //dt.Rows.Add("TTP", "TRẦN TẤN PHƯỚC", "18/5C KHU PHỐ 1 -XXX", "SGN", "R0001	Full Week", DateTime.Now, "2232", 0, true);


                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(typeCD);
                dt.Columns.Add(colSessionCD);


                //version add new column customerInfo1 -> customerInfo5
                if (!dt.Columns.Contains("customerInfo1")) //version cũ không có thông tin customerInfo1 -> customerInfo5
                //thêm column vào DataTable
                {
                    DataColumn customerInfo1 = new DataColumn("customerInfo1");
                    customerInfo1.DataType = typeof(String);

                    DataColumn customerInfo2 = new DataColumn("customerInfo2");
                    customerInfo2.DataType = typeof(String);

                    DataColumn customerInfo3 = new DataColumn("customerInfo3");
                    customerInfo3.DataType = typeof(String);

                    DataColumn customerInfo4 = new DataColumn("customerInfo4");
                    customerInfo4.DataType = typeof(String);

                    DataColumn customerInfo5 = new DataColumn("customerInfo5");
                    customerInfo5.DataType = typeof(String);

                    dt.Columns.Add(customerInfo1);
                    dt.Columns.Add(customerInfo2);
                    dt.Columns.Add(customerInfo3);
                    dt.Columns.Add(customerInfo4);
                    dt.Columns.Add(customerInfo5);
                }

                if (!dt.Columns.Contains("customerLocationAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("customerLocationAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }

                if (!dt.Columns.Contains("customerPhoneNumber")) //version cũ không có thông tin customerPhoneNumber
                {
                    DataColumn customerPhoneNumber = new DataColumn("customerPhoneNumber");
                    customerPhoneNumber.DataType = typeof(String);
                    dt.Columns.Add(customerPhoneNumber);
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        dt.Rows[i]["customerCode"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerCode"].ToString());
                        dt.Rows[i]["customerName"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerName"].ToString());
                        dt.Rows[i]["customerAddress"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerAddress"].ToString());
                    }
                    catch (Exception)
                    {

                    }
                }

                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TEMP_HH_CUSTOMER";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    //  bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCode", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerName", "CUSTOMER_NAME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerAddress", "CUSTOMER_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCategory", "CUSTOMER_CHAIN_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerRoute", "CUSTOMER_ROUTE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCreatedDate", "CUSTOMER_CREATED_DATE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocation", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAccuracy", "LONGITUDE_LATITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerActive", "ACTIVE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo1", "INFO_1"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo2", "INFO_2"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo3", "INFO_3"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo4", "INFO_4"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo5", "INFO_5"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAddress", "CUSTOMER_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerPhoneNumber", "PHONE_NUMBER"));


                    bulkcopy.WriteToServer(dt);
                    //SQL PROCESS
                    String sql = String.Format(@"
                                            
                                                --Cập nhật CUSTOMER_CD lần 
                                                    UPDATE T1 SET CUSTOMER_CD = T2.CUSTOMER_CD
                                                    FROM TEMP_HH_CUSTOMER T1 INNER JOIN M_HH_CUSTOMER T2 ON T1.CUSTOMER_CODE = T2.CUSTOMER_CODE  AND T2.TYPE_CD = T1.TYPE_CD AND T1.SALES_CD = T2.SALES_CD
                                                    WHERE T1.SESSION_CD = '{0}'  AND T1.CUSTOMER_CD IS NULL  
                                                    
                                                
                                                --GET DISTRIBUTOR BY ROUTE
                                                        
                                                 UPDATE T1 SET ROUTE_CODE = SUBSTRING(CUSTOMER_ROUTE,0,CHARINDEX(CHAR(9),CUSTOMER_ROUTE,0))
                                                 FROM TEMP_HH_CUSTOMER T1
                                                 WHERE T1.SESSION_CD = '{0}'  AND T1.ROUTE_CODE IS NULL  

                                                 UPDATE T1 SET DISTRIBUTOR_CD = T2.DISTRIBUTOR_CD
                                                 FROM TEMP_HH_CUSTOMER T1 INNER JOIN M_SUPERVISOR_ROUTE T2 ON T1.ROUTE_CODE = T2.SUPERVISOR_ROUTE_CODE 
                                                 WHERE T1.SESSION_CD = '{0}'  AND T1.ROUTE_CODE IS NOT NULL  



                                               --THÊM MỚI CUSTOMER
                                                 INSERT INTO [dbo].[M_HH_CUSTOMER]
                                                       ([CUSTOMER_CODE]
                                                       ,[CUSTOMER_NAME]
                                                       ,[CUSTOMER_ADDRESS]
                                                       ,[PHONE_NUMBER]
                                                       ,[CUSTOMER_CHAIN_CODE]
                                                       ,[CUSTOMER_ROUTE]
                                                       ,[CUSTOMER_CREATED_DATE]
                                                       ,[LONGITUDE_LATITUDE]
                                                       ,[LONGITUDE_LATITUDE_ACCURACY]
                                                       ,[ACTIVE]
                                                       ,[SALES_CD]
                                                       ,[DISTRIBUTOR_CD]                                                   
                                                       ,[TYPE_CD],[INFO_1],[INFO_2],[INFO_3],[INFO_4],[INFO_5],[CUSTOMER_LOCATION_ADDRESS])
            
                                               SELECT [CUSTOMER_CODE]
                                                       ,[CUSTOMER_NAME]
                                                       ,[CUSTOMER_ADDRESS]
                                                       ,[PHONE_NUMBER]
                                                       ,[CUSTOMER_CHAIN_CODE]
                                                       ,[CUSTOMER_ROUTE]
                                                       ,[CUSTOMER_CREATED_DATE]
                                                       ,[LONGITUDE_LATITUDE]
                                                       ,[LONGITUDE_LATITUDE_ACCURACY]
                                                       ,[ACTIVE]
                                                       ,[SALES_CD]
                                                       ,[DISTRIBUTOR_CD]                                                   
                                                       ,[TYPE_CD],[INFO_1],[INFO_2],[INFO_3],[INFO_4],[INFO_5],[CUSTOMER_LOCATION_ADDRESS] 
                                               FROM TEMP_HH_CUSTOMER
                                               WHERE SESSION_CD = '{0}'  AND CUSTOMER_CD IS  NULL

                                             --UPDATE CUSTOMER
                                            
                                           UPDATE T1 SET CUSTOMER_NAME = T2.CUSTOMER_NAME, 
                                                        CUSTOMER_ADDRESS = T2.CUSTOMER_ADDRESS, 
                                                        PHONE_NUMBER = T2.PHONE_NUMBER, 
                                                        CUSTOMER_CHAIN_CODE = T2.CUSTOMER_CHAIN_CODE, 
                                                        CUSTOMER_ROUTE = T2.CUSTOMER_ROUTE, 
                                                        LONGITUDE_LATITUDE = T2.LONGITUDE_LATITUDE, 
                                                        LONGITUDE_LATITUDE_ACCURACY = T2.LONGITUDE_LATITUDE_ACCURACY, 
                                                        ACTIVE = T2.ACTIVE ,
                                                        INFO_1 = T2.INFO_1 ,
                                                        INFO_2 = T2.INFO_2 ,
                                                        INFO_3 = T2.INFO_3 ,
                                                        INFO_4 = T2.INFO_4 ,
                                                        INFO_5 = T2.INFO_5 ,
                                                        SALES_CD = T2.SALES_CD,
                                                        DISTRIBUTOR_CD = T2.DISTRIBUTOR_CD,
                                                        CUSTOMER_LOCATION_ADDRESS = T2.CUSTOMER_LOCATION_ADDRESS
                                         FROM M_HH_CUSTOMER  T1 INNER JOIN TEMP_HH_CUSTOMER  T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD AND T1.TYPE_CD = T2.TYPE_CD AND T1.SALES_CD = T2.SALES_CD
                                         WHERE T2.CUSTOMER_CD IS NOT NULL AND  T2.SESSION_CD = '{0}'

            

                                        ", sessionCD);
                    L5sSql.Execute(sql);

                }

                return "1";
            }
            catch (Exception eXX)
            {
                // L5sMsg.Show(eXX.Message);
                return "-1";
            }
        }

        private string synchronizePOCAddnewCustomerBySales(string obj, string type, string jsonData)
        {
            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales.Rows.Count <= 0)
                return "-1";


            try
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);


                //  DataTable dt = new DataTable();

                String salesCD = dtSales.Rows[0]["SALES_CD"].ToString();
                String distributorCD = dtSales.Rows[0]["DISTRIBUTOR_CD"].ToString();

                //add new column
                DataColumn columnSalesCD = new DataColumn("SALES_CD");
                columnSalesCD.DataType = typeof(String);
                columnSalesCD.DefaultValue = salesCD;

                DataColumn columnDistributorCD = new DataColumn("DISTRIBUTOR_CD");
                columnDistributorCD.DataType = typeof(String);
                columnDistributorCD.DefaultValue = distributorCD;

                DataColumn typeCD = new DataColumn("TYPE_CD");
                typeCD.DataType = typeof(String);
                typeCD.DefaultValue = type;

                //add new column
                String sessionCD = System.Guid.NewGuid().ToString();
                DataColumn colSessionCD = new DataColumn("SESSION_CD");
                colSessionCD.DataType = typeof(String);
                colSessionCD.DefaultValue = sessionCD;




                //dt.Columns.Add("customerCode", typeof(String));
                //dt.Columns.Add("customerName", typeof(String));
                //dt.Columns.Add("customerAddress", typeof(String));
                //dt.Columns.Add("customerCategory", typeof(String));
                //dt.Columns.Add("customerRoute", typeof(String));
                //dt.Columns.Add("customerCreatedDate", typeof(DateTime));
                //dt.Columns.Add("customerLocation", typeof(String));
                //dt.Columns.Add("customerLocationAccuracy", typeof(float));
                //dt.Columns.Add("customerActive", typeof(Boolean));



                dt.Columns.Add(columnSalesCD);
                dt.Columns.Add(columnDistributorCD);
                dt.Columns.Add(typeCD);
                dt.Columns.Add(colSessionCD);





                //   dt.Rows.Add("TTP", "TRẦN TẤN PHƯỚC", "18/5C KHU PHỐ 1 -XXX", "SGN", "3B38	Thu 3" , DateTime.Now, "2232",0 , true);


                //version add new column customerInfo1 -> customerInfo5
                if (!dt.Columns.Contains("customerInfo1")) //version cũ không có thông tin customerInfo1 -> customerInfo5
                //thêm column vào DataTable
                {
                    DataColumn customerInfo1 = new DataColumn("customerInfo1");
                    customerInfo1.DataType = typeof(String);

                    DataColumn customerInfo2 = new DataColumn("customerInfo2");
                    customerInfo2.DataType = typeof(String);

                    DataColumn customerInfo3 = new DataColumn("customerInfo3");
                    customerInfo3.DataType = typeof(String);

                    DataColumn customerInfo4 = new DataColumn("customerInfo4");
                    customerInfo4.DataType = typeof(String);

                    DataColumn customerInfo5 = new DataColumn("customerInfo5");
                    customerInfo5.DataType = typeof(String);

                    dt.Columns.Add(customerInfo1);
                    dt.Columns.Add(customerInfo2);
                    dt.Columns.Add(customerInfo3);
                    dt.Columns.Add(customerInfo4);
                    dt.Columns.Add(customerInfo5);
                }


                if (!dt.Columns.Contains("customerLocationAddress")) //version cũ không có thông tin customerLocationAddress
                {
                    DataColumn customerLocationAddress = new DataColumn("customerLocationAddress");
                    customerLocationAddress.DataType = typeof(String);
                    dt.Columns.Add(customerLocationAddress);
                }

                if (!dt.Columns.Contains("customerPhoneNumber")) //version cũ không có thông tin customerPhoneNumber
                {
                    DataColumn customerPhoneNumber = new DataColumn("customerPhoneNumber");
                    customerPhoneNumber.DataType = typeof(String);
                    dt.Columns.Add(customerPhoneNumber);
                }

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].ReadOnly = false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        dt.Rows[i]["customerCode"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerCode"].ToString());
                        dt.Rows[i]["customerName"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerName"].ToString());
                        dt.Rows[i]["customerAddress"] = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(dt.Rows[i]["customerAddress"].ToString());
                    }
                    catch (Exception)
                    {

                    }
                }


                using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                {
                    connection.Open();
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                    bulkcopy.DestinationTableName = "TEMP_HH_CUSTOMER";

                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CD", "SALES_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CD", "DISTRIBUTOR_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCode", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerName", "CUSTOMER_NAME"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerAddress", "CUSTOMER_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCategory", "CUSTOMER_CHAIN_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerRoute", "CUSTOMER_ROUTE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerCreatedDate", "CUSTOMER_CREATED_DATE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocation", "LONGITUDE_LATITUDE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAccuracy", "LONGITUDE_LATITUDE_ACCURACY"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerActive", "ACTIVE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo1", "INFO_1"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo2", "INFO_2"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo3", "INFO_3"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo4", "INFO_4"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerInfo5", "INFO_5"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE_CD", "TYPE_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerLocationAddress", "CUSTOMER_LOCATION_ADDRESS"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("customerPhoneNumber", "PHONE_NUMBER"));


                    bulkcopy.WriteToServer(dt);
                    //SQL PROCESS
                    String sql = String.Format(@"
                                            
                                                --Cập nhật CUSTOMER_CD lần 
                                                    UPDATE T1 SET CUSTOMER_CD = T2.CUSTOMER_CD
                                                    FROM TEMP_HH_CUSTOMER T1 INNER JOIN M_HH_CUSTOMER T2 ON T1.CUSTOMER_CODE = T2.CUSTOMER_CODE  AND T2.TYPE_CD = T1.TYPE_CD AND T1.SALES_CD = T2.SALES_CD
                                                    WHERE T1.SESSION_CD = '{0}'  AND T1.CUSTOMER_CD IS NULL  
                                                    
                                                
                                               --THÊM MỚI CUSTOMER
                                                 INSERT INTO [dbo].[M_HH_CUSTOMER]
                                                       ([CUSTOMER_CODE]
                                                       ,[CUSTOMER_NAME]
                                                       ,[CUSTOMER_ADDRESS]
                                                       ,[PHONE_NUMBER]
                                                       ,[CUSTOMER_CHAIN_CODE]
                                                       ,[CUSTOMER_ROUTE]
                                                       ,[CUSTOMER_CREATED_DATE]
                                                       ,[LONGITUDE_LATITUDE]
                                                       ,[LONGITUDE_LATITUDE_ACCURACY]
                                                       ,[ACTIVE]
                                                       ,[SALES_CD]
                                                       ,[DISTRIBUTOR_CD]                                                   
                                                       ,[TYPE_CD],[INFO_1],[INFO_2],[INFO_3],[INFO_4],[INFO_5],[CUSTOMER_LOCATION_ADDRESS])
            
                                               SELECT [CUSTOMER_CODE]
                                                       ,[CUSTOMER_NAME]
                                                       ,[CUSTOMER_ADDRESS]
                                                       ,[PHONE_NUMBER]
                                                       ,[CUSTOMER_CHAIN_CODE]
                                                       ,[CUSTOMER_ROUTE]
                                                       ,[CUSTOMER_CREATED_DATE]
                                                       ,[LONGITUDE_LATITUDE]
                                                       ,[LONGITUDE_LATITUDE_ACCURACY]
                                                       ,[ACTIVE]
                                                       ,[SALES_CD]
                                                       ,[DISTRIBUTOR_CD]                                                   
                                                       ,[TYPE_CD],[INFO_1],[INFO_2],[INFO_3],[INFO_4],[INFO_5],[CUSTOMER_LOCATION_ADDRESS] 
                                               FROM TEMP_HH_CUSTOMER
                                               WHERE SESSION_CD = '{0}'  AND CUSTOMER_CD IS  NULL

                                             --UPDATE CUSTOMER
                                            
                                           UPDATE T1 SET CUSTOMER_NAME = T2.CUSTOMER_NAME, 
                                                        CUSTOMER_ADDRESS = T2.CUSTOMER_ADDRESS, 
                                                        PHONE_NUMBER = T2.PHONE_NUMBER, 
                                                        CUSTOMER_CHAIN_CODE = T2.CUSTOMER_CHAIN_CODE, 
                                                        CUSTOMER_ROUTE = T2.CUSTOMER_ROUTE, 
                                                        LONGITUDE_LATITUDE = T2.LONGITUDE_LATITUDE, 
                                                        LONGITUDE_LATITUDE_ACCURACY = T2.LONGITUDE_LATITUDE_ACCURACY, 
                                                        ACTIVE = T2.ACTIVE ,
                                                        INFO_1 = T2.INFO_1 ,
                                                        INFO_2 = T2.INFO_2 ,
                                                        INFO_3 = T2.INFO_3 ,
                                                        INFO_4 = T2.INFO_4 ,
                                                        INFO_5 = T2.INFO_5 ,
                                                        SALES_CD = T2.SALES_CD,
                                                        DISTRIBUTOR_CD = T2.DISTRIBUTOR_CD,
                                                        CUSTOMER_LOCATION_ADDRESS = T2.CUSTOMER_LOCATION_ADDRESS
                                         FROM M_HH_CUSTOMER  T1 INNER JOIN TEMP_HH_CUSTOMER  T2 ON T1.CUSTOMER_CD = T2.CUSTOMER_CD AND T1.TYPE_CD = T2.TYPE_CD AND T1.SALES_CD = T2.SALES_CD
                                         WHERE T2.CUSTOMER_CD IS NOT NULL AND  T2.SESSION_CD = '{0}'

                                        ", sessionCD);
                    L5sSql.Execute(sql);

                }

                return "1";
            }
            catch (Exception ex)
            {
                // L5sMsg.Show(ex.Message);
                return "-1";
            }

        }




        #endregion


        #region synchronize Data CP from HH: tính năng cho phép đồng bộ đơn hàng từ HH lên Server
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCUploadDataFromHHToPC(String imei, String obj, String jsonData)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";
            //if (!isValidDevice(imei, false))
            //    return "-1";
            if (obj.Trim().Length == 0)
                return "-1";

            if (jsonData.Trim().Length == 0)
                return "-1";

            //lấy đường dẫn thư mục cần đồng bộ từ HH lên Server
            if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromHH"] == null)
                return "-1";

            String pathFolder = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromHH"].ToString();
            //String pathFolder = "C:\nietpub\wwww\CP_Sync\cphh\fromhh";
            //get folder valid to download

            String folderName = String.Format("{0}_{1}", obj, DateTime.Now.ToString("yyMMddHHmmssfff"));
            DirectoryInfo directory = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(pathFolder, folderName));

            //chuyển jsondata thành datatable  - mỗi 1 dòng là 1 file text sẽ được lưu vào folder đồng bộ
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                File.WriteAllText(Path.Combine(directory.FullName, dt.Rows[i]["FileName"].ToString()), dt.Rows[i]["Data"].ToString());
            }

            //tạo path file Product để xoá sau khi xử lý dữ liệu đơn hàng        
            string PathDeleteProduct = string.Format(pathFolder + directory + @"\" + "PRODUCT.txt");
            FileInfo myfileinf = new FileInfo(PathDeleteProduct);
            myfileinf.Delete();


            //tạo file đánh dấu f.txt ghi nhận là đã đồng bộ thành công
            File.WriteAllText(Path.Combine(directory.FullName, "f.txt"), "");
            return "1";
            // DataTable tbCountry = L5sSql.Query("SELECT NAME,VALUE FROM S_PARAMS WHERE NAME='COUNTRY' AND VALUE in ('TH','PH')");

            //// Kiểm tra nếu server của PH hoặc TL thì ko cần import dữ liệu lên bảng O_GROSS_SALE_ORDER_DMS

            // if (tbCountry != null && tbCountry.Rows.Count > 0)
            // {
            //     return "1";
            // }

            // else
            //     return importOrderAndGrossSales(imei, pathFolder, folderName, obj, DateTime.Now.ToString(), PathDeleteProduct);


        }
        private string importPOCOrderAndGrossSales(String imei, String pathFolder, String folderName, String SALES_CODE, String TIME_SYNC, String PathDeleteProduct)
        {
            //if (!this.isValidDevice(imei))
            //    return "-1";
            //if (!isValidDevice(imei, false))
            //    return "-1";
            String folder = pathFolder + folderName;
            //Tạo đường dẫn full của các file cần đọc
            String productFile = System.IO.Path.Combine(folder, "PRODUCT.txt");
            String orderFile = System.IO.Path.Combine(folder, "PALM_ORD_DET.txt");
            String customerFile = System.IO.Path.Combine(folder, "PALM_ORDER.txt");

            #region lay thong tin product
            //Đọc file, mỗi dòng là thông tin về 1 sản phẩm
            String[] products = File.ReadAllLines(productFile);
            DataTable dt = new DataTable();
            //Thêm các cột cho datatable
            for (int i = 0; i < 11; i++)
            {
                DataColumn columnGUID = new DataColumn("Column" + i);
                columnGUID.DataType = typeof(String);
                dt.Columns.Add(columnGUID);
            }
            dt.Columns[0].ColumnName = "PRODUCT_CODE";
            dt.Columns[1].ColumnName = "PRICE";
            dt.Columns[8].ColumnName = "PRODUCT_NAME";
            //Duyệt qua các dòng thông tin về sp, thêm dữ liệu vào dt, sau mỗi dấu tab là dữ liệu của các cột khác nhau
            foreach (String st in products)
            {

                String[] row = st.Split('\t');
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.Length; i++)
                {

                    dr[i] = row[i];//Gán dữ liệu vào datatable
                }
                dt.Rows.Add(dr);
            }
            //Tạo dictionary với key là PRODUCT_CODE và value là PRICE 
            Dictionary<String, String> PRODUCTCODE_PRICE_DIC = new Dictionary<String, String>();
            //Tạo dictionary với key là PRODUCT_CODE và value là NAME
            //Các dictionary được dùng để tham chiếu giá và tên ở DataTable của phần ORDER
            Dictionary<String, String> PRODUCTCODE_NAME_DIC = new Dictionary<String, String>();
            foreach (DataRow row in dt.Rows)
            {
                //Thêm dữ liệu vào các dictionary
                PRODUCTCODE_PRICE_DIC.Add(row["PRODUCT_CODE"].ToString(), row["PRICE"].ToString());
                PRODUCTCODE_NAME_DIC.Add(row["PRODUCT_CODE"].ToString(), row["PRODUCT_NAME"].ToString());
            }


            #endregion

            #region lay thong tin khach hang
            String[] customers = File.ReadAllLines(customerFile);
            DataTable customerTable = new DataTable();

            for (int i = 0; i < 10; i++)
            {
                DataColumn columnGUID = new DataColumn("Column" + i);
                columnGUID.DataType = typeof(String);
                customerTable.Columns.Add(columnGUID);
            }
            customerTable.Columns[0].ColumnName = "ORDER_CODE";
            customerTable.Columns[3].ColumnName = "CUSTOMER_CODE";
            customerTable.Columns[5].ColumnName = "ORDER_DATE";
            foreach (String st in customers)
            {

                String[] row = st.Split('\t');
                DataRow dr = customerTable.NewRow();
                for (int i = 0; i < row.Length; i++)
                {

                    dr[i] = row[i];
                }
                customerTable.Rows.Add(dr);
            }
            //Dictionary với Key là ORDER_CODE và VALUE là CUSTOMER_CODE
            Dictionary<String, String> ORDER_CUSTOMER_DIC = new Dictionary<String, String>();
            //Dictionary với Key là ORDER_CODE và VALUE là ngày của ORDER
            Dictionary<String, String> ORDER_DATE_DIC = new Dictionary<String, String>();

            foreach (DataRow row in customerTable.Rows)
            {
                ORDER_CUSTOMER_DIC.Add(row["ORDER_CODE"].ToString(), row["CUSTOMER_CODE"].ToString());
                ORDER_DATE_DIC.Add(row["ORDER_CODE"].ToString(), row["ORDER_DATE"].ToString());
            }

            #endregion

            #region lay thong tin order

            String[] orders = File.ReadAllLines(orderFile);
            DataTable orderTable = new DataTable();

            for (int i = 0; i < 6; i++)
            {
                DataColumn columnGUID = new DataColumn("Column" + i);
                columnGUID.DataType = typeof(String);
                orderTable.Columns.Add(columnGUID);
            }
            orderTable.Columns[0].ColumnName = "PRODUCT_CODE";
            orderTable.Columns[1].ColumnName = "ORDER_CODE";
            orderTable.Columns[2].ColumnName = "QUANTITY";
            orderTable.Columns["QUANTITY"].DataType = typeof(Int32);
            foreach (String st in orders)
            {

                String[] row = st.Split('\t');
                DataRow dr = orderTable.NewRow();
                for (int i = 0; i < row.Length; i++)
                {

                    dr[i] = row[i];
                }
                orderTable.Rows.Add(dr);
            }
            DataColumn priceColumn = new DataColumn("PRICE");
            priceColumn.DataType = typeof(Double);
            orderTable.Columns.Add(priceColumn);

            orderTable.Columns.Add("PRODUCT_NAME");
            //Thêm cột SALES_CODE
            DataColumn sale_code = new DataColumn("DSR_CODE");
            sale_code.DataType = typeof(String);
            sale_code.DefaultValue = SALES_CODE;
            orderTable.Columns.Add(sale_code);
            //Thêm cột TOTAL
            DataColumn totalColumn = new DataColumn("TOTAL");
            totalColumn.DataType = typeof(Double);
            orderTable.Columns.Add(totalColumn);
            //Thêm cột CUSTOMER_CODE
            orderTable.Columns.Add("CUSTOMER_CODE");
            //thêm cột DATE
            DataColumn dateColumn = new DataColumn("ORDER_DATE");
            dateColumn.DataType = typeof(DateTime);
            orderTable.Columns.Add(dateColumn);
            //Thêm cột TIME_SYNC
            DataColumn timeSync = new DataColumn("TIME_SYNC");
            timeSync.DataType = typeof(DateTime);
            timeSync.DefaultValue = Convert.ToDateTime(TIME_SYNC);
            orderTable.Columns.Add(timeSync);
            //Thêm cột SESSION_CD
            String session = DateTime.Now.ToString("ddMMyyyyHHmmssfff");
            DataColumn sessionCD = new DataColumn("SESSION_CD");
            sessionCD.DataType = typeof(String);
            sessionCD.DefaultValue = SALES_CODE + session;
            orderTable.Columns.Add(sessionCD);
            //Cập nhật giá trị của các cột PRICE, PRODUCT_NAME, TOTAL, CUSTOMER dựa vào ORDER_CODE
            //Các giá trị được lấy tương ứng từ các DICTIONNARY được tạo ở trên
            for (int i = 0; i < orderTable.Rows.Count; i++)
            {
                orderTable.Rows[i]["PRICE"] = Convert.ToInt32(PRODUCTCODE_PRICE_DIC[orderTable.Rows[i]["PRODUCT_CODE"].ToString()]);
                orderTable.Rows[i]["PRODUCT_NAME"] = PRODUCTCODE_NAME_DIC[orderTable.Rows[i]["PRODUCT_CODE"].ToString()];
                orderTable.Rows[i]["TOTAL"] = Convert.ToInt16(orderTable.Rows[i]["QUANTITY"].ToString()) * Convert.ToDouble(orderTable.Rows[i]["PRICE"].ToString());
                orderTable.Rows[i]["CUSTOMER_CODE"] = ORDER_CUSTOMER_DIC[orderTable.Rows[i]["ORDER_CODE"].ToString()];
                //Riêng cột DATE cần phải đưa chuỗi dạng yyyymmdd về dạng yyyy/mm/dd trước khi thêm vào datatable
                String date = ORDER_DATE_DIC[orderTable.Rows[i]["ORDER_CODE"].ToString()];
                date = date.Remove(4) + "-" + date.Remove(0, 4);// giá trị từ yyyymmdd --> yyyy-mmdd
                date = date.Remove(7) + "-" + date.Remove(0, 7);//giá trị từ yyyy/mmdd --> yyyy-mm-dd
                orderTable.Rows[i]["ORDER_DATE"] = Convert.ToDateTime(date);

            }
            //xoá các cột thường, bước này ko làm cũng ko quan trọng
            orderTable.Columns.Remove("Column3");
            orderTable.Columns.Remove("Column4");
            orderTable.Columns.Remove("Column5");

            #endregion

            #region insert orders to database

            using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
            {
                connection.Open();

                SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                bulkcopy.DestinationTableName = "TMP_GROSS_SALES_ORDER_DMS";
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PRODUCT_CODE", "PRODUCT_CODE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ORDER_CODE", "ORDER_CODE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("QUANTITY", "QUANTITY"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PRICE", "PRICE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PRODUCT_NAME", "PRODUCT_NAME"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DSR_CODE", "DSR_CODE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TOTAL", "TOTAL"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CUSTOMER_CODE", "CUSTOMER_CODE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ORDER_DATE", "ORDER_DATE"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TIME_SYNC", "TIME_SYNC"));
                bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SESSION_CD", "SESSION_CD"));
                bulkcopy.WriteToServer(orderTable);

                //FILL FULL DATA bang tam TMP_GROSS_SALES_ORDER_DMS AND INSERT NEW ORDER VAO O_GROSS_SALES_ORDER_DMS
                string Sql = string.Format(@"
                            SET XACT_ABORT ON
							BEGIN TRAN
							BEGIN TRY
							DECLARE @SESSION_CD NVARCHAR(50)
                            SET @SESSION_CD='{0}'
                            ----UPDATE TRONG BẢNG TẠM 
                            UPDATE TMP SET TMP.DSR_NAME = MSA.SALES_NAME,
								TMP.CUSTOMER_NAME = MCUS.CUSTOMER_NAME,
	                            TMP.DISTRIBUTOR_NAME = MDIS.DISTRIBUTOR_NAME,
								TMP.DISTRIBUTOR_CODE = MDIS.DISTRIBUTOR_CODE, 
	                            TMP.AREA_CODE = MAR.AREA_CODE, 
								TMP.AREA_CD = MAR.AREA_CD, 
								TMP.REGION_CODE = MRE.REGION_CODE, 
								TMP.REGION_CD = MRE.REGION_CD
	                            FROM TMP_GROSS_SALES_ORDER_DMS TMP
	                            LEFT JOIN M_SALES MSA ON MSA.SALES_CODE = TMP.DSR_CODE
	                            LEFT JOIN M_CUSTOMER MCUS ON MCUS.CUSTOMER_CODE = TMP.CUSTOMER_CODE
	                            LEFT JOIN [M_DISTRIBUTOR.] MDIS ON MDIS.DISTRIBUTOR_CD = MSA.DISTRIBUTOR_CD 
	                            LEFT JOIN M_COMMUNE MCOM ON MCOM.COMMUNE_CD = MDIS.COMMUNE_CD
	                            LEFT JOIN M_DISTRICT MDTRICT ON MDTRICT.DISTRICT_CD = MCOM.DISTRICT_CD
	                            LEFT JOIN M_PROVINCE MPRO ON MPRO.PROVINCE_CD = MDTRICT.PROVINCE_CD
	                            LEFT JOIN M_AREA_PROVINCE MAP ON MAP.PROVINCE_CD = MPRO.PROVINCE_CD
	                            LEFT JOIN M_AREA MAR ON MAR.AREA_CD = MAP.AREA_CD
	                            LEFT JOIN M_REGION MRE ON MRE.REGION_CD = MAR.REGION_CD
                            WHERE TMP.SESSION_CD = @SESSION_CD 
						--UPDATE CÁC ĐƠN HÀNG ĐÃ TỒN TẠI
						--DELCARE BẢNG CHỨA CÁC CD CẦN UPDATE
								DECLARE  @TABLEUPDATE TABLE(
											CROSSALE_ORDER_CD BIGINT
									)
								INSERT INTO @TABLEUPDATE
								SELECT TMP.CROSS_SALE_ORDER_DMS_CD  FROM TMP_GROSS_SALES_ORDER_DMS TMP
								INNER JOIN O_GROSS_SALES_ORDER_DMS O_GROSS ON TMP.PRODUCT_CODE = O_GROSS.PRODUCT_CODE
										AND TMP.ORDER_CODE=O_GROSS.ORDER_CODE AND TMP.SESSION_CD = @SESSION_CD AND TMP.CUSTOMER_CODE = O_GROSS.CUSTOMER_CODE
									  
							   UPDATE O_GROSS SET
										    O_GROSS.REGION_CD=TMP.REGION_CD,
										    O_GROSS.REGION_CODE=TMP.REGION_CODE,
											O_GROSS.AREA_CD=TMP.AREA_CD,
											O_GROSS.AREA_CODE=TMP.AREA_CODE,
											O_GROSS.DISTRIBUTOR_NAME=TMP.DISTRIBUTOR_NAME,
											O_GROSS.DISTRIBUTOR_CODE=TMP.DISTRIBUTOR_CODE,
											O_GROSS.CUSTOMER_NAME=TMP.CUSTOMER_NAME,
											O_GROSS.CUSTOMER_CODE=TMP.CUSTOMER_CODE,
											O_GROSS.DSR_CODE=TMP.DSR_CODE,
											O_GROSS.DSR_NAME=TMP.DSR_NAME,
											O_GROSS.TIME_SYNC=TMP.TIME_SYNC,
											O_GROSS.PRODUCT_CODE=TMP.PRODUCT_CODE,
											O_GROSS.PRODUCT_NAME=TMP.PRODUCT_NAME,
											O_GROSS.QUANTITY=TMP.QUANTITY,
											O_GROSS.PRICE=TMP.PRICE,
											O_GROSS.TOTAL=TMP.TOTAL,
											O_GROSS.ACTIVE=TMP.ACTIVE,
											O_GROSS.CREATED_DATE=TMP.CREATED_DATE,
                                            O_GROSS.ORDER_DATE= TMP.ORDER_DATE
									FROM O_GROSS_SALES_ORDER_DMS O_GROSS INNER JOIN TMP_GROSS_SALES_ORDER_DMS TMP
									ON O_GROSS.ORDER_CODE = TMP.ORDER_CODE
                                    WHERE   TMP.CROSS_SALE_ORDER_DMS_CD IN (SELECT CROSSALE_ORDER_CD FROM @TABLEUPDATE)
								--INSERT NHỮNG ĐƠN HÀNG MỚI
										INSERT INTO O_GROSS_SALES_ORDER_DMS 
										SELECT REGION_CD,REGION_CODE,
                                                AREA_CD,AREA_CODE,
                                                DISTRIBUTOR_NAME,DISTRIBUTOR_CODE,
                                                CUSTOMER_NAME,CUSTOMER_CODE,
                                                DSR_CODE,DSR_NAME,
                                                TIME_SYNC,
                                                ORDER_CODE,
                                                PRODUCT_CODE,PRODUCT_NAME,
                                                QUANTITY,PRICE,TOTAL,ORDER_DATE,ACTIVE,CREATED_DATE
										FROM TMP_GROSS_SALES_ORDER_DMS TMP
										WHERE TMP.CROSS_SALE_ORDER_DMS_CD NOT IN (SELECT CROSSALE_ORDER_CD FROM @TABLEUPDATE)
										AND TMP.SESSION_CD = @SESSION_CD
								DELETE FROM TMP_GROSS_SALES_ORDER_DMS
                                WHERE SESSION_CD=@SESSION_CD                                
                                SELECT 1							
                            COMMIT
							END TRY
							BEGIN CATCH
								ROLLBACK
								SELECT -1
							END CATCH", SALES_CODE + session);
                DataTable KQ = P5sCmmFns.SqlDatatableTimeout(Sql, 360000);
                FileInfo myfileinf = new FileInfo(PathDeleteProduct);
                myfileinf.Delete();
                if (KQ.Rows[0][0].ToString() == "1")
                {
                    return "1";
                }
                else
                {
                    return "-1";
                }
            }
            #endregion
        }


        #endregion

        #region synchronize Data CP from PC: hàm cho phép HH lấy thông tin về tuyến từ Server để bán hàng
        [WebMethod]
        [SoapHeader("ServiceCredentials")]

        public String synchronizePOCGetDataFromCP(String imei, String obj)
        {
            //if (!this.isValidDevice(imei, false))
            //    return "-1";
            if (!this.isPOCValidDevice(imei))
                return "-1";

            if (obj.Trim().Length == 0)
                return "-1";


            if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromPC"] == null)
                return "-1";

            try
            {
                String pathFolder = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromPC"].ToString();

                //Kiểm tra hệ thống này là của Thái Lan, Nếu là Thái lan thì tạo các file txt cho sale đồng bộ
                DataTable tbCountry = L5sSql.Query("SELECT NAME,VALUE FROM S_PARAMS WHERE NAME='COUNTRY' AND VALUE in ('TH','PH')");
                if (tbCountry != null && tbCountry.Rows.Count > 0)
                {
                    string path = pathFolder + "\\" + obj + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\\";
                    if (!File.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    DataTable dt = L5sSql.Query(@"DECLARE @SALES_CD BIGINT
                                SELECT @SALES_CD = SALES_CD FROM M_SALES
                                WHERE SALES_CODE = @0

                                SELECT  cust.CUSTOMER_CODE , cust.CUSTOMER_NAME, rout.ROUTE_CODE , cust.CUSTOMER_ADDRESS, '','','','',cust.CUSTOMER_CODE,'-','-','-','-','-'                
                                FROM 
	                                M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE cr ON cust.CUSTOMER_CD = cr.CUSTOMER_CD AND cr.ACTIVE = 1
	                                INNER JOIN O_SALES_ROUTE sr ON cr.ROUTE_CD = sr.ROUTE_CD AND sr.ACTIVE = 1
	                                INNER JOIN M_SALES sls ON sr.SALES_CD = sls.SALES_CD 	
	                                INNER JOIN M_ROUTE rout ON sr.ROUTE_CD = rout.ROUTE_CD
	                                INNER JOIN M_DISTRIBUTOR dis ON sls.DISTRIBUTOR_CD = dis.DISTRIBUTOR_CD
                                WHERE sls.SALES_CD = @SALES_CD", obj);
                    P5sCmmFns.P5sWriteFile(dt, path + "CUSTOMER.TXT");

                    DataTable dt2 = L5sSql.Query(@"DECLARE @SALES_CD BIGINT

                                SELECT @SALES_CD = SALES_CD FROM M_SALES
                                WHERE SALES_CODE = @0

                                SELECT ROUTE_CODE,ROUTE_NAME, 'D' FROM M_ROUTE
                                WHERE ROUTE_CD IN (SELECT ROUTE_CD FROM O_SALES_ROUTE WHERE SALES_CD = @SALES_CD)", obj);
                    P5sCmmFns.P5sWriteFile(dt2, path + "ROUTE.TXT");

                    DataTable dt3 = L5sSql.Query(@"SELECT SALES_CODE,SALES_NAME,'D' FROM M_SALES WHERE SALES_CODE=@0", obj);
                    P5sCmmFns.P5sWriteFile(dt3, path + "SALESMAN.TXT");

                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "BRAND.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "CATEGORY.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "CUSTBRD.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "CUSTSKU.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "f.txt");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "MTDSales.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "PALM_DOCNO.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "PRODUCT.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "REASON.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "SCHEME.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "STOCKIEST.TXT");
                    P5sCmmFns.P5sWriteFile(new DataTable(), path + "SUBBRAND.TXT");

                }

                //get folder valid to download
                DirectoryInfo d = new DirectoryInfo(pathFolder);
                DirectoryInfo[] listDirectory = d.GetDirectories(String.Format("{0}_*", obj), SearchOption.TopDirectoryOnly);
                if (listDirectory == null || listDirectory.Length == 0)
                    return "-1";

                //lấy folder cuối cùng để xử lý            
                FileInfo[] fileD = listDirectory[listDirectory.Length - 1].GetFiles("d.txt", SearchOption.TopDirectoryOnly);
                if (fileD.Length >= 1)
                    return "-1";

                FileInfo[] data = listDirectory[listDirectory.Length - 1].GetFiles("*.txt", SearchOption.TopDirectoryOnly);

                List<POCCTxtFromPC> result = new List<POCCTxtFromPC>();
                for (int i = 0; i < data.Length; i++)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(data[i].FullName);
                    result.Add(new POCCTxtFromPC(data[i].Name, System.Text.Encoding.UTF8.GetString(bytes).Replace("\"", "")));
                }

                List<POCCCustomer> customers = new List<POCCCustomer>();
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                             new System.Web.Script.Serialization.JavaScriptSerializer();
                oSerializer.MaxJsonLength = Int32.MaxValue;
                return oSerializer.Serialize(result);
            }
            catch (Exception)
            {

                return "-1";
            }

        }



        //Sau khi HH đồng bộ tuyến BH thành công thì sẽ gọi hàm này để tạo file d.txt đánh dấu đồng bộ thành công
        //đông thời duy chuyển folder này sang thư mục backup để làm nhẹ hệ thống
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public void uploadPOCResultAfterSynchronizeDataFromCP(String imei, String obj)
        {
            if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromPC"] == null)
                return;

            String pathFolder = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromPC"].ToString();
            //get folder valid to download
            DirectoryInfo d = new DirectoryInfo(pathFolder);
            DirectoryInfo[] listDirectory = d.GetDirectories(String.Format("{0}_*", obj), SearchOption.TopDirectoryOnly);
            if (listDirectory == null || listDirectory.Length == 0)
                return;


            //lấy folder cuối cùng để xử lý            
            FileInfo[] fileD = listDirectory[listDirectory.Length - 1].GetFiles("d.txt", SearchOption.TopDirectoryOnly);
            if (fileD.Length >= 1)
                return;

            File.WriteAllText(Path.Combine(listDirectory[listDirectory.Length - 1].FullName, "d.txt"), "");

            try
            {
                if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromPC_BK"] == null)
                    return;

                String pathFolderBK = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromPC_BK"].ToString();
                Directory.Move(listDirectory[listDirectory.Length - 1].FullName, pathFolderBK + "/" + listDirectory[listDirectory.Length - 1].Name);

            }
            catch (Exception)
            {

            }

        }

        public class POCCTxtFromPC
        {
            public String FileName = "";
            public String Data = "";
            public POCCTxtFromPC(String FileName, String Data)
            {
                this.FileName = FileName;
                this.Data = Data;
            }
        }



        #endregion

        #region synchronize Data PC to Server: hàm cho phép PC (ở NPP) đồng bộ tuyến BH lên Server để HH có thể download về
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCUploadFileFromPCToServer(String obj, String jsonData)
        {
            if (!this.isPOCValidAuthPC())
                return "-1";

            if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromPC"] == null)
                return "-1";

            String pathFolder = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromPC"].ToString();
            //get folder valid to download

            String folderDSP = obj + "_" + DateTime.Now.ToString("yyMMddHHmmssfff");


            //json
            DirectoryInfo directory = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(pathFolder, folderDSP));

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                File.WriteAllText(Path.Combine(directory.FullName, dt.Rows[i]["FileName"].ToString()), dt.Rows[i]["Data"].ToString());
            }
            //tạo file đánh dấu f.txt
            File.WriteAllText(Path.Combine(directory.FullName, "f.txt"), "");

            return "1";
        }





        #endregion

        #region synchronize Data SERVER to PC : hàm cho phép PC (NPP) lấy thông tin đơn hàng đã được HH gửi lên Server về máy PC
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCUploadFileFromServerToPC(String obj)
        {
            if (!this.isPOCValidAuthPC())
                return "-1";

            if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromHH"] == null)
                return "-1";

            String pathFolder = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromHH"].ToString();
            //get folder valid to download

            DirectoryInfo d = new DirectoryInfo(pathFolder);
            DirectoryInfo[] listDirectory = d.GetDirectories(String.Format("{0}_*", obj), SearchOption.TopDirectoryOnly);
            if (listDirectory == null || listDirectory.Length == 0)
                return "-1";

            FileInfo[] data = listDirectory[listDirectory.Length - 1].GetFiles("*.txt", SearchOption.TopDirectoryOnly);

            //lấy folder cuối cùng để xử lý            
            FileInfo[] fileF = listDirectory[listDirectory.Length - 1].GetFiles("d.txt", SearchOption.TopDirectoryOnly);
            if (fileF.Length >= 1)
                return "-1";

            List<POCCTxtFromPC> result = new List<POCCTxtFromPC>();
            for (int i = 0; i < data.Length; i++)
            {
                byte[] bytes = System.IO.File.ReadAllBytes(data[i].FullName);
                result.Add(new POCCTxtFromPC(data[i].Name, System.Text.Encoding.UTF8.GetString(bytes).Replace("\"", "")));
            }

            result.Add(new POCCTxtFromPC(listDirectory[listDirectory.Length - 1].Name, "DSPfolderName"));


            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                            new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;
            return oSerializer.Serialize(result);

        }

        #endregion
        #region synchronize Data SERVER to PC - upload result: hàm upload kết quả nếu như PC (NPP) đã lấy được thành công thông tin về ĐH trên Server, và đồng thời duy chuyển folder này sang folder backup để làm nhẹ hệ thống
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public void uploadResultPOCAfterSynchronizeDataFromServerToPC(String obj)
        {
            //if (!this.isValidAuthPC())
            //    return;

            if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromHH"] == null)
                return;

            String pathFolder = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromHH"].ToString();
            //get folder valid to download


            DirectoryInfo d = new DirectoryInfo(pathFolder);
            DirectoryInfo[] listDirectory = d.GetDirectories(String.Format("{0}_*", obj), SearchOption.TopDirectoryOnly);
            if (listDirectory == null || listDirectory.Length == 0)
                return;

            FileInfo[] data = listDirectory[listDirectory.Length - 1].GetFiles("*.txt", SearchOption.TopDirectoryOnly);

            //lấy folder cuối cùng để xử lý            
            FileInfo[] fileF = listDirectory[listDirectory.Length - 1].GetFiles("d.txt", SearchOption.TopDirectoryOnly);
            if (fileF.Length >= 1)
                return;

            File.WriteAllText(Path.Combine(listDirectory[listDirectory.Length - 1].FullName, "d.txt"), "");


            try
            {
                if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromHH_BK"] == null)
                    return;

                String pathFolderBK = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryFromHH_BK"].ToString();
                Directory.Move(listDirectory[listDirectory.Length - 1].FullName, pathFolderBK + "/" + listDirectory[listDirectory.Length - 1].Name);

            }
            catch (Exception)
            {

            }

        }





        #endregion


        #region hàm tải các file quảng cáo từ Server xuống HH.
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCDownloadFile(String imei, String obj, String type)
        {
            if (!this.isPOCValidDevice(imei))
                return this.POCencrypt("1");

            if (obj.Trim().Length == 0)
                return this.POCencrypt("1");

            if (System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryDownload"] == null)
                return this.POCencrypt("1");

            String pathFolder = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryDownload"].ToString();
            //get folder valid to download
            DirectoryInfo d = new DirectoryInfo(pathFolder);
            if (!d.Exists)
                return this.POCencrypt("1");

            FileInfo[] data = d.GetFiles();
            if (data == null || data.Length == 0)
                return this.POCencrypt("1");

            DataTable dt = new DataTable();
            dt.Columns.Add("FileName", typeof(String));
            dt.Columns.Add("PathDownLoad", typeof(String));

            String StrFileName = P5sCmmFns.P5sGetFileNameFromSalesCode(obj);

            if (StrFileName == "-1")
            {
                return this.POCencrypt("1");
            }

            string[] ArrFileName = StrFileName.Split('≡');
            for (int i = 0; i < data.Length; i++)
            {
                if (Array.IndexOf(ArrFileName, data[i].Name) != -1)
                {
                    String url = String.Format("{0}{1}", HttpContext.Current.Request.Url.Authority, String.Format("/Admin/DownloadFile.aspx?param={0}&param1={1}", data[i].Name.Replace(" ", P5sEnum.SPLIT_KEY), "SyncDirectoryDownload"));
                    if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
                        url = "https://" + url;
                    else
                        url = "http://" + url;

                    dt.Rows.Add(data[i].Name, url);
                }
            }
            if (dt != null || dt.Rows.Count > 0)
            {
                return this.POCencrypt(POCGetJSONString(dt));
            }
            else
            {
                return this.POCencrypt("1");
            }
        }

        #endregion


        #region get message : hàm lấy tin nhắn từ Server gửi xuống HH, tin nhắn được tạo từ trang: Administration | Tin nhắn (/Programs/Message.aspx)
        //get message
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetMessage(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1":
                        return this.synchronizePOCGetMessageForSales(obj, type);
                    case "2":
                        return this.synchronizePOCGetMessageForSup(obj, type);
                    case "3":
                        return this.synchronizePOCGetMessageForASM(obj, type);
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {
                return "-1";
            }

        }

        private string synchronizePOCGetMessageForASM(string obj, string type)
        {
            String objectCD = "";
            String sql = "";
            sql = String.Format("SELECT ASM_CD AS OBJECT_CD FROM M_ASM WHERE ASM_CODE = '{0}' ", obj);
            DataTable dt = L5sSql.Query(sql);
            if (dt == null || dt.Rows.Count == 0)
                return "-1";
            objectCD = dt.Rows[0]["OBJECT_CD"].ToString();

            dt = L5sSql.Query(@"SELECT msg.MESSAGE_CD,MESSAGE_CONTENT ,CONVERT (NVARCHAR(50),msg.CREATED_DATE, 103)  AS CREATED_DATE
                                    FROM O_MESSAGE msg INNER JOIN O_MESSAGE_DETAIL msgDetail ON msg.MESSAGE_CD = msgDetail.MESSAGE_CD 
                                    WHERE msgDetail.OBJECT_CD = @0 AND msgDetail.TYPE_CD = @1 
	                                      AND 
	                                      msg.ACTIVE = 1 and msgDetail.ACTIVE = 1
                                    ORDER BY CREATED_DATE                                         
                                         ", objectCD, type);
            if (dt == null || dt.Rows.Count == 0)
                return "1";

            return this.POCGetJSONString(dt);
        }

        private string synchronizePOCGetMessageForSup(string obj, string type)
        {
            String objectCD = "";
            String sql = "";
            sql = String.Format("SELECT SUPERVISOR_CD AS OBJECT_CD FROM M_SUPERVISOR WHERE SUPERVISOR_CODE = '{0}' ", obj);
            DataTable dt = L5sSql.Query(sql);
            if (dt == null || dt.Rows.Count == 0)
                return "-1";
            objectCD = dt.Rows[0]["OBJECT_CD"].ToString();

            dt = L5sSql.Query(@"SELECT msg.MESSAGE_CD,MESSAGE_CONTENT ,CONVERT (NVARCHAR(50),msg.CREATED_DATE, 103)  AS CREATED_DATE
                                            FROM O_MESSAGE msg INNER JOIN O_MESSAGE_DETAIL msgDetail ON msg.MESSAGE_CD = msgDetail.MESSAGE_CD 
                                            WHERE msgDetail.OBJECT_CD = @0 AND msgDetail.TYPE_CD = @1 
	                                              AND 
	                                              msg.ACTIVE = 1 and msgDetail.ACTIVE = 1
                                            ORDER BY CREATED_DATE                                           
                                         ", objectCD, type);
            if (dt == null || dt.Rows.Count == 0)
                return "1";

            return this.POCGetJSONString(dt);
        }

        private string synchronizePOCGetMessageForSales(string obj, string type)
        {
            String objectCD = "";
            String sql = "";
            sql = String.Format("SELECT SALES_CD AS OBJECT_CD FROM M_SALES WHERE SALES_CODE = '{0}' ", obj);
            DataTable dt = L5sSql.Query(sql);
            if (dt == null || dt.Rows.Count == 0)
                return "-1";
            objectCD = dt.Rows[0]["OBJECT_CD"].ToString();

            dt = L5sSql.Query(@"SELECT msg.MESSAGE_CD,MESSAGE_CONTENT ,CONVERT (NVARCHAR(50),msg.CREATED_DATE, 103)  AS CREATED_DATE
                                FROM O_MESSAGE msg INNER JOIN O_MESSAGE_DETAIL msgDetail ON msg.MESSAGE_CD = msgDetail.MESSAGE_CD 
                                WHERE msgDetail.OBJECT_CD = @0 AND msgDetail.TYPE_CD = @1 
	                                  AND 
	                                  msg.ACTIVE = 1 and msgDetail.ACTIVE = 1
                                ORDER BY CREATED_DATE                                        
                                         ", objectCD, type);
            if (dt == null || dt.Rows.Count == 0)
                return "1";

            return this.POCGetJSONString(dt);
        }



        private string POCGetJSONString(DataTable dt)
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

        #endregion
        #region get report for sales: hàm lấy thông tin report Sales Mtd cho NVBH
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string reportPOCSalesMtd(String imei, String obj, String yyyymm)
        {
            try
            {

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
                if (dtSales == null || dtSales.Rows.Count <= 0)
                    return "-1";

                if (yyyymm.Trim().Length == 0)
                    return "-1";

                String salesCd = dtSales.Rows[0]["SALES_CD"].ToString();
                POCCReportSalesMtd report = new POCCReportSalesMtd();
                report.YYYYMM = yyyymm;

                DateTime dtime = DateTime.ParseExact(yyyymm, "yyyyMM", null);
                String sqlGetSqlTimeInOut = P5sCmmFns.P5sGetDynamicSqlTimeInTimeOut(new DateTime(dtime.Year, dtime.Month, 1), new DateTime(dtime.Year, dtime.Month, DateTime.DaysInMonth(dtime.Year, dtime.Month)));

                #region process Target,Actual,%Achived,Total orders
                //get sales Target
                DataTable dt = L5sSql.Query(@"SELECT * FROM O_SALES_TARGET WHERE SALES_TARGET_YYYYMM = @0 AND SALES_CD = @1", yyyymm, salesCd);

                if (dt != null && dt.Rows.Count >= 1)
                {
                    report.TargetSales = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["TOTAL_SALES_TARGET"].ToString()));
                    report.TargetTP = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["TP_SALES_TARGET"].ToString()));
                    report.TargetTB = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["TB_SALES_TARGET"].ToString()));
                }

                //get actual 
                dt = L5sSql.Query(@"
                                SELECT 
		                                ISNULL(TOTAL_SALES_AMOUNT,0) AS TOTAL_SALES_AMOUNT,
		                                ISNULL(TOTAL_SALES_TP,0) AS TOTAL_SALES_TP,
		                                ISNULL(TOTAL_SALES_TB,0) AS TOTAL_SALES_TB,
		                                ISNULL(TOTAL_ORDERS,0) AS TOTAL_ORDERS,
                                        MonthToDateSales AS  MonthToDateSales
                                        
                                FROM
                                (                                                                
                                        SELECT 
		                                            SUM(SALES_AMOUNT) AS TOTAL_SALES_AMOUNT,
		                                            SUM(SALES_TP) AS TOTAL_SALES_TP,
		                                            SUM(SALES_TB) AS TOTAL_SALES_TB,
		                                            SUM(CUSTOMER_ORDERS) AS TOTAL_ORDERS,
                                                    MAX(SALES_AMOUNT_DATE)  AS MonthToDateSales
                                            FROM O_SALES_AMOUNT
                                            WHERE SALES_CD = @1 AND YEAR(SALES_AMOUNT_DATE)*100 + MONTH(SALES_AMOUNT_DATE) = @0
                                 ) AS T
                        ", yyyymm, salesCd);

                if (dt != null && dt.Rows.Count >= 1)
                {

                    try
                    {
                        report.MonthToDateSales = DateTime.Parse(dt.Rows[0]["MonthToDateSales"].ToString()).ToString("yyyy-MM-dd");
                    }
                    catch (Exception)
                    {
                        report.MonthToDateSales = "No data";
                    }

                    report.ActualSales = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["TOTAL_SALES_AMOUNT"].ToString()));
                    report.ActualTP = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["TOTAL_SALES_TP"].ToString()));
                    report.ActualTB = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["TOTAL_SALES_TB"].ToString()));

                    report.TotalOrders = String.Format("{0:n0}", Int32.Parse(dt.Rows[0]["TOTAL_ORDERS"].ToString()));

                    report.PercentageAchivedSales = Double.Parse(report.TargetSales) == 0 ? "0" : Math.Round((Double.Parse(report.ActualSales) / Double.Parse(report.TargetSales)) * 100, 0) + " %";
                    report.PercentageAchivedTP = Double.Parse(report.TargetTP) == 0 ? "0" : Math.Round((Double.Parse(report.ActualTP) / Double.Parse(report.TargetTP)) * 100, 0) + " %";
                    report.PercentageAchivedTB = Double.Parse(report.TargetTB) == 0 ? "0" : Math.Round((Double.Parse(report.ActualTB) / Double.Parse(report.TargetTB)) * 100, 0) + " %";
                }
                #endregion


                #region process RL Count,Visited,Stores sold,Not visited,%Visited,%ECC

                dt = L5sSql.Query(String.Format(@"DECLARE @yyymm int
                                    SET @yyymm = @0

                                    DECLARE @salesCd BIGINT
                                    SET @salesCd = @1


                                    DECLARE @TB_CustomerVisited TABLE 
                                    (
                                        CUSTOMER_CD BIGINT
                                        PRIMARY KEY (CUSTOMER_CD)
                                    )

                                    INSERT INTO @TB_CustomerVisited
                                    SELECT DISTINCT CUSTOMER_CD 
                                    FROM ({0}) otio
                                    WHERE  YEAR(TIME_IN_CREATED_DATE)*100 + MONTH(TIME_IN_CREATED_DATE)  = @yyymm				                																			
                                           AND otio.TYPE_CD = 1 --NVBH
                                           AND CUSTOMER_CD IS NOT NULL



                            

                                    DECLARE @TB_CustomerHaveSalesAmount TABLE 
                                    (
                                        CUSTOMER_CD BIGINT
                                        PRIMARY KEY (CUSTOMER_CD)
                                    )
                                    INSERT INTO @TB_CustomerHaveSalesAmount
                                    SELECT DISTINCT CUSTOMER_CD 
                                    FROM O_SALES_AMOUNT slsAmount
                                    WHERE   YEAR(slsAmount.SALES_AMOUNT_DATE)*100 + MONTH(slsAmount.SALES_AMOUNT_DATE) = @yyymm	


                                    DECLARE @TB_CustomerInformation TABLE 
                                    (
                                        
                                        SALES_CD BIGINT,
                                        CUSTOMER_CD BIGINT	
                                        PRIMARY KEY (CUSTOMER_CD)
                                    )
                                    INSERT INTO @TB_CustomerInformation
                                    SELECT DISTINCT sls.SALES_CD,custR.CUSTOMER_CD  
                                            FROM M_SALES sls INNER JOIN O_SALES_ROUTE slsR ON sls.SALES_CD = slsR.SALES_CD AND slsR.ACTIVE = 1
                                                  INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD AND rout.ACTIVE = 1
                                                  INNER JOIN O_CUSTOMER_ROUTE custR ON rout.ROUTE_CD = custR.ROUTE_CD AND custR.ACTIVE = 1	
                                                  INNER JOIN M_CUSTOMER cust ON custR.CUSTOMER_CD = cust.CUSTOMER_CD AND cust.ACTIVE = 1
                                    WHERE  sls.SALES_CD  = @salesCd




                                    SELECT 
		                                    T.SALES_CD,
		                                    COUNT(*) AS NoOfStore,
		                                    SUM(IS_VISIT) AS NoOfStoreVisited,
		                                    COUNT(*) - SUM(IS_VISIT) AS NoOfStoreNotVisited,
		                                    SUM (
				                                    CASE 
					                                    WHEN IS_VISIT = 1 AND HAVE_SALES_AMOUNT = 1 THEN 1
					                                    ELSE 0
				                                    END	
			                                    ) AS NoOfStoreVisitedAndSold,
                                    				
		                                    SUM (
				                                    CASE 
					                                    WHEN IS_VISIT = 1 AND HAVE_SALES_AMOUNT = 0 THEN 1
					                                    ELSE 0
				                                    END	
			                                    ) AS NoOfStoreVisitedAndNotSold,
                                		    SUM(HAVE_SALES_AMOUNT) AS NoOfStoreSold,
        
                                            SUM (
				                                    CASE 
					                                    WHEN IS_VISIT = 0 AND HAVE_SALES_AMOUNT =  1 THEN 1
					                                    ELSE 0
				                                    END	
			                                    ) AS NoOfStoreNotVisitedButSold																				
                                    														 
                                    FROM
                                    (
				                                    SELECT 
				                                    ms.SALES_CD,
				                                    ms.CUSTOMER_CD,
				                                    CASE
                                                          WHEN ISNULL(visited.CUSTOMER_CD,-1)  = -1 THEN 0
				                                          ELSE 1
				                                    END   AS IS_VISIT, 


				                                    CASE 
                                                         WHEN ISNULL(amount.CUSTOMER_CD,-1)  = -1 THEN 0
					                                     ELSE 1
				                                    END  AS HAVE_SALES_AMOUNT


				                                    FROM  @TB_CustomerInformation ms LEFT JOIN @TB_CustomerVisited visited ON ms.CUSTOMER_CD = visited.CUSTOMER_CD
                                                              LEFT JOIN @TB_CustomerHaveSalesAmount amount ON  ms.CUSTOMER_CD = amount.CUSTOMER_CD
				                                    
                                    											
                                    ) AS T
                                    GROUP BY T.SALES_CD", sqlGetSqlTimeInOut), yyyymm, salesCd);

                if (dt != null && dt.Rows.Count >= 1)
                {

                    report.RLCount = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["NoOfStore"].ToString()));
                    report.Visited = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["NoOfStoreVisited"].ToString()));
                    report.NotVisited = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["NoOfStoreNotVisited"].ToString()));
                    report.StoresVisitedAndSold = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["NoOfStoreVisitedAndSold"].ToString()));
                    report.StoresVisitedAndNotSold = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["NoOfStoreVisitedAndNotSold"].ToString()));
                    report.StoresNotVisitedButSold = String.Format("{0:n0}", Double.Parse(dt.Rows[0]["NoOfStoreNotVisitedButSold"].ToString()));


                    report.PercentageVisited = Double.Parse(report.RLCount) == 0 ? "0" : Math.Round((Double.Parse(report.Visited) / Double.Parse(report.RLCount)) * 100, 2) + " %";
                    report.PercentageECC = Double.Parse(report.RLCount) == 0 ? "0" : Math.Round(((Double.Parse(report.StoresVisitedAndSold) + Double.Parse(report.StoresNotVisitedButSold)) / Double.Parse(report.RLCount)) * 100, 2) + " %";

                }

                #endregion


                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                             new System.Web.Script.Serialization.JavaScriptSerializer();
                oSerializer.MaxJsonLength = Int32.MaxValue;
                return oSerializer.Serialize(report);

            }
            catch (Exception)
            {
                return "-1";
            }
            return "-1";

        }

        //khai báo class SalesMtd Report để lưu trữ các thông tin liên quan tới Report
        public class POCCReportSalesMtd
        {

            public String YYYYMM;

            public String MonthToDateSales;

            public String TargetSales;
            public String TargetTP;
            public String TargetTB;

            public String ActualSales;
            public String ActualTP;
            public String ActualTB;

            public String PercentageAchivedSales;
            public String PercentageAchivedTP;
            public String PercentageAchivedTB;

            public String TotalOrders;
            public String RLCount;
            public String Visited;
            public String StoresVisitedAndSold;
            public String StoresVisitedAndNotSold;
            public String StoresNotVisitedButSold;

            public String NotVisited;

            public String PercentageVisited;
            public String PercentageECC;

            public POCCReportSalesMtd()
            {
                this.TargetSales = "0";
                this.TargetTP = "0";
                this.TargetTB = "0";

                this.ActualSales = "0";
                this.ActualTP = "0";
                this.ActualTB = "0";

                this.PercentageAchivedSales = "0";
                this.PercentageAchivedTP = "0";
                this.PercentageAchivedTB = "0";

                this.TotalOrders = "0";
                this.RLCount = "0";
                this.Visited = "0";
                this.StoresVisitedAndSold = "0";
                this.StoresVisitedAndNotSold = "0";
                this.NotVisited = "0";


                this.PercentageVisited = "0";
                this.PercentageECC = "0";

            }

            public POCCReportSalesMtd(

                                String TargetSales, String ActualSales, String PercentageAchivedSales,
                                String TargetTP, String ActualTP, String PercentageAchivedTP,
                                String TargetTB, String ActualTB, String PercentageAchivedTB,

                                String TotalOrders, String RLCount, String Visited,
                                String StoresVisitedAndSold, String StoresVisitedAndNotSold, String NotVisited,
                                String PercentageVisited, String PercentageECC,

                                String MonthToDateSales, String YYYYMM, String StoresNotVisitedButSold
                            )
            {
                this.TargetSales = TargetSales;
                this.TargetTP = TargetTP;
                this.TargetTB = TargetTB;

                this.ActualSales = ActualSales;
                this.ActualTP = ActualTP;
                this.ActualTB = ActualTB;

                this.PercentageAchivedSales = PercentageAchivedSales;
                this.PercentageAchivedTP = PercentageAchivedTP;
                this.PercentageAchivedTB = PercentageAchivedTB;

                this.TotalOrders = TotalOrders;
                this.RLCount = RLCount;
                this.Visited = Visited;
                this.StoresVisitedAndSold = StoresVisitedAndSold;
                this.StoresVisitedAndNotSold = StoresVisitedAndNotSold;
                this.NotVisited = NotVisited;

                this.PercentageVisited = PercentageVisited;
                this.PercentageECC = PercentageECC;

                this.MonthToDateSales = MonthToDateSales;
                this.YYYYMM = YYYYMM;
                this.StoresNotVisitedButSold = StoresNotVisitedButSold;
            }
        }


        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string reportPOCTarget(String imei, String obj, String yyyymm)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            if (obj.Trim().Length == 0)
                return "-1";

            DataTable dtSales = L5sSql.Query("SELECT  * FROM M_SALES WHERE SALES_CODE = @0", obj);
            if (dtSales == null || dtSales.Rows.Count <= 0)
                return "-1";

            if (yyyymm.Trim().Length == 0)
                return "-1";

            String salesCd = dtSales.Rows[0]["SALES_CD"].ToString();
            Int32 yyyy = Int32.Parse(yyyymm) / 100;
            Int32 mm = Int32.Parse(yyyymm) % 100;

            String sql = @"SELECT OT.TARGET_CD,OT.TARGET_CODE,OT.TARGET_NAME,OTC.TARGET_CONDITION_CD,OTC.DATA_TYPE,OTC.CONDITION_NAME,OTR.VALUE 
                                FROM O_TARGET_RESULT OTR
                                INNER JOIN O_TARGET_CONDITION  OTC ON OTR.TARGET_CONDITION_CD=OTC.TARGET_CONDITION_CD
                                INNER JOIN O_TARGET OT ON OTC.TARGET_CD=OT.TARGET_CD
                                WHERE OTR.SALE_CD =@0 AND OTR.YEAR = @1 AND OTR.MONTH = @2
                                ORDER BY OTC.TARGET_CONDITION_CD
                            ";
            //ORDER BY OT.TARGET_CD
            DataTable dt = L5sSql.Query(sql, salesCd, yyyy, mm);
            if (dt == null || dt.Rows.Count == 0)
            {
                return "1";
            }
            else
            {
                return POCGetJSONString(dt);
            }
        }


        #endregion
        #region get customer for SUP,ASM:  khách hàng được lấy dựa vào tuyến được gán cho CDS, ASM
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetCustomer(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return "-1";
                    case "2": // Sup
                        return this.synchronizePOCGetCustomerForSup(obj, type);
                    case "3": //ASM
                        return this.synchronizePOCGetCustomerForASM(obj, type);
                    default:
                        return "-1";
                }

            }
            catch (Exception)
            {

                return "-1";
            }

        }

        //hàm lấy thông tin KH theo tuyến được gán theo tính năng: Master Data | Making working route for ASM 
        private string synchronizePOCGetCustomerForASM(string obj, string type)
        {

            DataTable dt = L5sSql.Query(@"SELECT cust.CUSTOMER_CODE, cust.CUSTOMER_NAME, msupR.ASM_ROUTE_CODE, 
	                                               cust.CUSTOMER_ADDRESS, cust.PHONE_NUMBER,cust.CUSTOMER_CHAIN_CODE,cust.LONGITUDE_LATITUDE
                                                 , cust.LONGITUDE_LATITUDE_ACCURACY,cust.ACTIVE
                                            FROM 
		                                            M_ASM sup INNER JOIN O_ASM_ASM_ROUTE supR ON sup.ASM_CD = supR.ASM_CD AND supR.ACTIVE = 1
		                                            INNER JOIN O_CUSTOMER_ASM_ROUTE custR ON supR.ASM_ROUTE_CD = custR.ASM_ROUTE_CD and custR.ACTIVE = 1
		                                            INNER JOIN M_CUSTOMER cust ON custR.CUSTOMER_CD = cust.CUSTOMER_CD
		                                            INNER JOIN M_ASM_ROUTE msupR ON supR.ASM_ROUTE_CD = msupR.ASM_ROUTE_CD
                                            WHERE sup.ASM_CODE = @0
                                         ", obj);



            return this.POCP5sConvertCustomerForASMToJson(dt);
        }


        //hàm lấy thông tin KH theo tuyến được gán theo tính năng: Master Data | Making working route for CDS 
        private string synchronizePOCGetCustomerForSup(string obj, string type)
        {

            DataTable dt = L5sSql.Query(@"SELECT cust.CUSTOMER_CODE, cust.CUSTOMER_NAME, msupR.SUPERVISOR_ROUTE_CODE, 
	                                               cust.CUSTOMER_ADDRESS,cust.PHONE_NUMBER, cust.CUSTOMER_CHAIN_CODE,cust.LONGITUDE_LATITUDE
                                                 , cust.LONGITUDE_LATITUDE_ACCURACY,cust.ACTIVE
                                            FROM 
		                                            M_SUPERVISOR sup INNER JOIN O_SUPERVISOR_SUPERVISOR_ROUTE supR ON sup.SUPERVISOR_CD = supR.SUPERVISOR_CD AND supR.ACTIVE = 1
		                                            INNER JOIN O_CUSTOMER_SUPERVISOR_ROUTE custR ON supR.SUPERVISOR_ROUTE_CD = custR.SUPERVISOR_ROUTE_CD and custR.ACTIVE = 1
		                                            INNER JOIN M_CUSTOMER cust ON custR.CUSTOMER_CD = cust.CUSTOMER_CD AND cust.ACTIVE = 1
		                                            INNER JOIN M_SUPERVISOR_ROUTE msupR ON supR.SUPERVISOR_ROUTE_CD = msupR.SUPERVISOR_ROUTE_CD AND msupR.ACTIVE = 1
                                            WHERE sup.SUPERVISOR_CODE =@0
                                         ", obj);



            return this.POCP5sConvertCustomerForSupToJson(dt);
        }



        private string POCP5sConvertCustomerForSupToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCCustomerForSupervisor> p = new List<POCCustomerForSupervisor>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String CustomerCode = dt.Rows[i]["CUSTOMER_CODE"].ToString();
                String CustomerName = dt.Rows[i]["CUSTOMER_NAME"].ToString();
                String CustomerRouteCode = dt.Rows[i]["SUPERVISOR_ROUTE_CODE"].ToString();
                String CustomerAddress = dt.Rows[i]["CUSTOMER_ADDRESS"].ToString();
                String CustomerPhoneNumber = dt.Rows[i]["PHONE_NUMBER"].ToString();
                String CustomerChainCode = dt.Rows[i]["CUSTOMER_CHAIN_CODE"].ToString();
                String CustomerLatitudeLongitude = dt.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                String CustomerLatitudeLongitudeAccuracy = dt.Rows[i]["LONGITUDE_LATITUDE_ACCURACY"].ToString();
                String CustomerActive = dt.Rows[i]["ACTIVE"].ToString();


                POCCustomerForSupervisor c = new POCCustomerForSupervisor(CustomerCode
                                                                     , CustomerName
                                                                     , CustomerRouteCode
                                                                     , CustomerAddress
                                                                     , CustomerPhoneNumber
                                                                     , CustomerChainCode
                                                                     , CustomerLatitudeLongitude
                                                                     , CustomerLatitudeLongitudeAccuracy
                                                                     , CustomerActive);
                p.Add(c);
            }

            return oSerializer.Serialize(p);
        }
        public class POCCustomerForSupervisor
        {
            public String CustomerCode = "";
            public String CustomerName = "";
            public String CustomerRouteCode = "";
            public String CustomerAddress = "";
            public String CustomerPhoneNumber = "";
            public String CustomerChainCode = "";
            public String CustomerLongLat = "";
            public String CustomerLatitudeLongitude = "";
            public String CustomerLatitudeLongitudeAccuracy = "";
            public String CustomerActive = "";
            public POCCustomerForSupervisor(String CustomerCode, String CustomerName, String CustomerRouteCode, String CustomerAddress, String CustomerPhoneNumber, String CustomerChainCode,
                            String CustomerLatitudeLongitude, String CustomerLatitudeLongitudeAccuracy, String CustomerActive)
            {
                this.CustomerCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerCode);
                this.CustomerName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerName);
                this.CustomerPhoneNumber = CustomerPhoneNumber;
                this.CustomerRouteCode = CustomerRouteCode;
                this.CustomerAddress = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerAddress);
                this.CustomerChainCode = CustomerChainCode;
                this.CustomerLatitudeLongitude = CustomerLatitudeLongitude;
                this.CustomerLatitudeLongitudeAccuracy = CustomerLatitudeLongitudeAccuracy;
                this.CustomerActive = CustomerActive;
            }

        }


        private string POCP5sConvertCustomerForASMToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCCustomerForASM> p = new List<POCCustomerForASM>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String CustomerCode = dt.Rows[i]["CUSTOMER_CODE"].ToString();
                String CustomerName = dt.Rows[i]["CUSTOMER_NAME"].ToString();
                String CustomerRouteCode = dt.Rows[i]["ASM_ROUTE_CODE"].ToString();
                String CustomerAddress = dt.Rows[i]["CUSTOMER_ADDRESS"].ToString();
                String CustomerChainCode = dt.Rows[i]["CUSTOMER_CHAIN_CODE"].ToString();
                String CustomerLatitudeLongitude = dt.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                String CustomerLatitudeLongitudeAccuracy = dt.Rows[i]["LONGITUDE_LATITUDE_ACCURACY"].ToString();
                String CustomerActive = dt.Rows[i]["ACTIVE"].ToString();


                POCCustomerForASM c = new POCCustomerForASM(CustomerCode
                                                                    , CustomerName
                                                                    , CustomerRouteCode
                                                                    , CustomerAddress
                                                                    , CustomerChainCode
                                                                    , CustomerLatitudeLongitude
                                                                    , CustomerLatitudeLongitudeAccuracy
                                                                    , CustomerActive);
                p.Add(c);
            }

            return oSerializer.Serialize(p);
        }

        public class POCCustomerForASM
        {
            public String CustomerCode = "";
            public String CustomerName = "";
            public String CustomerRouteCode = "";
            public String CustomerAddress = "";
            public String CustomerChainCode = "";
            public String CustomerLongLat = "";
            public String CustomerLatitudeLongitude = "";
            public String CustomerLatitudeLongitudeAccuracy = "";
            public String CustomerActive = "";
            public POCCustomerForASM(String CustomerCode, String CustomerName, String CustomerRouteCode, String CustomerAddress, String CustomerChainCode,
                            String CustomerLatitudeLongitude, String CustomerLatitudeLongitudeAccuracy, String CustomerActive)
            {
                this.CustomerCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerCode);
                this.CustomerName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerName);
                this.CustomerRouteCode = CustomerRouteCode;
                this.CustomerAddress = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(CustomerAddress);
                this.CustomerChainCode = CustomerChainCode;
                this.CustomerLatitudeLongitude = CustomerLatitudeLongitude;
                this.CustomerLatitudeLongitudeAccuracy = CustomerLatitudeLongitudeAccuracy;
                this.CustomerActive = CustomerActive;
            }

        }



        #endregion
        #region get route for asm,sup : lấy danh sách tuyến của ASM, CDS được gán
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetRoute(String imei, String obj, String type)
        {
            try
            {
                //if (!this.isValidDevice(imei))
                //    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return "-1";
                    case "2": // Sup
                        return this.synchronizePOCGetRouteForSup(obj, type);
                    case "3": //ASM
                        return this.synchronizePOCGetRouteForASM(obj, type);
                    default:
                        return "-1";
                }


            }
            catch (Exception)
            {

                return "-1";
            }

        }

        private string synchronizePOCGetRouteForASM(string obj, string type)
        {
            DataTable dt = L5sSql.Query(@"SELECT msupR.ASM_ROUTE_CODE, msupR.ASM_ROUTE_NAME
                                            FROM 
		                                            M_ASM sup INNER JOIN O_ASM_ASM_ROUTE supR ON sup.ASM_CD = supR.ASM_CD AND supR.ACTIVE = 1
		                                            INNER JOIN M_ASM_ROUTE msupR ON supR.ASM_ROUTE_CD = msupR.ASM_ROUTE_CD
                                            WHERE sup.ASM_CODE = @0
                                         ", obj);



            return this.POCP5sConvertRouteForASMToJson(dt);

        }

        private string synchronizePOCGetRouteForSup(string obj, string type)
        {
            DataTable dt = L5sSql.Query(@"SELECT msupR.SUPERVISOR_ROUTE_CODE, msupR.SUPERVISOR_ROUTE_NAME
                                            FROM 
		                                            M_SUPERVISOR sup INNER JOIN O_SUPERVISOR_SUPERVISOR_ROUTE supR ON sup.SUPERVISOR_CD = supR.SUPERVISOR_CD AND supR.ACTIVE = 1
		                                            INNER JOIN M_SUPERVISOR_ROUTE msupR ON supR.SUPERVISOR_ROUTE_CD = msupR.SUPERVISOR_ROUTE_CD
                                            WHERE sup.SUPERVISOR_CODE = @0
                                         ", obj);

            return this.POCP5sConvertRouteForSupToJson(dt);
        }

        private string POCP5sConvertRouteForSupToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCRouteForSupervisor> p = new List<POCRouteForSupervisor>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String RouteCode = dt.Rows[i]["SUPERVISOR_ROUTE_CODE"].ToString();
                String RouteName = dt.Rows[i]["SUPERVISOR_ROUTE_NAME"].ToString();
                POCRouteForSupervisor r = new POCRouteForSupervisor(RouteCode, RouteName);
                p.Add(r);
            }

            return oSerializer.Serialize(p);
        }

        public class POCRouteForSupervisor
        {
            public String RouteCode = "";
            public String RouteName = "";

            public POCRouteForSupervisor(String RouteCode, String RouteName)
            {
                this.RouteCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteCode);
                this.RouteName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteName);
            }
        }

        private string POCP5sConvertRouteForASMToJson(DataTable dt)
        {
            //nếu không có KH trong tuyến thì thống báo đồng bộ thành công
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCRouteForASMervisor> p = new List<POCRouteForASMervisor>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String RouteCode = dt.Rows[i]["ASM_ROUTE_CODE"].ToString();
                String RouteName = dt.Rows[i]["ASM_ROUTE_NAME"].ToString();
                POCRouteForASMervisor r = new POCRouteForASMervisor(RouteCode, RouteName);
                p.Add(r);
            }

            return oSerializer.Serialize(p);
        }

        public class POCRouteForASMervisor
        {
            public String RouteCode = "";
            public String RouteName = "";
            public String RouteCd = "";

            public POCRouteForASMervisor(String RouteCode, String RouteName)
            {
                this.RouteCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteCode);
                this.RouteName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteName);
            }
            public POCRouteForASMervisor(String RouteCode, String RouteName, String RouteCd)
            {
                this.RouteCode = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteCode);
                this.RouteName = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteName);
                this.RouteCd = P5sCmm.P5sCmmFns.P5sReplaceInVaildJson(RouteCd);
            }
        }


        #endregion
        #region synchronize HHVersion hàm ghi nhận version HH, hàm này không còn xử dụng nữa vì Verson HH đã được ghi nhận ở bên fsid.5stars.com.vn:9139

        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCHHVersion(String imei, String obj, String jsonData)
        {
            //tạm thời không xóa hàm để không bị ảnh hưởng việc đồng bộ từ HH
            return "1";
        }

        #endregion
        #region synchronizeLogTimeDevice hàm ghi nhận log thời gian HH và Server có bị sai lệch hay không
        [WebMethod]
        [SoapHeader("ServiceCredentials")]

        public string synchronizePOCLogTimeDevice(String imei, String obj, String jsonData, String type)
        {
            try
            {

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";


                if (jsonData.Trim().Length == 0)
                    return "-1";



                try
                {
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                    //add new column
                    DataColumn columnSalesCode = new DataColumn("SALES_CODE");
                    columnSalesCode.DataType = typeof(String);
                    columnSalesCode.DefaultValue = obj;
                    dt.Columns.Add(columnSalesCode);

                    //add new column
                    DataColumn columnType = new DataColumn("TYPE");
                    columnType.DataType = typeof(String);
                    columnType.DefaultValue = type;
                    dt.Columns.Add(columnType);



                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dt.Columns[i].ReadOnly = false;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["LogTimeDeviceTimePDA"].ToString().Equals(""))
                            dt.Rows[i]["LogTimeDeviceTimePDA"] = DBNull.Value;

                        if (dt.Rows[i]["LogTimeDeviceTimeServer"].ToString().Equals(""))
                            dt.Rows[i]["LogTimeDeviceTimeServer"] = DBNull.Value;

                        if (dt.Rows[i]["LogTimeDeviceCreatedDate"].ToString().Equals(""))
                            dt.Rows[i]["LogTimeDeviceCreatedDate"] = DBNull.Value;
                    }



                    using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                    {
                        connection.Open();
                        SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                        //I assume you have created the table previously 
                        //Someone else here already showed how   
                        bulkcopy.DestinationTableName = "L_TIME_DEVICE";

                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LogTimeDeviceTimePDA", "TIME_DEVICE_TIME_PDA"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LogTimeDeviceTimeServer", "TIME_DEVICE_TIME_SERVER"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LogTimeDeviceCreatedDate", "TIME_DEVICE_CREATED_DATE"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "SALES_CODE"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE", "TYPE_CD"));


                        try
                        {
                            bulkcopy.WriteToServer(dt);
                            return "1";
                        }
                        catch (Exception EX)
                        {
                            // L5sMsg.Show(EX.Message);
                            return "-1";
                        }
                    }

                    return "-1";

                }
                catch (Exception ex)
                {
                    // L5sMsg.Show(ex.Message);
                    return "-1";
                }

            }
            catch (Exception)
            {

                return "-1";
            }


        }
        #endregion
        #region synchronizeLogInternetDevice
        [WebMethod]
        [SoapHeader("ServiceCredentials")]

        public string synchronizePOCLogInternetDevice(String imei, String obj, String jsonData, String type)
        {
            try
            {

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";


                if (jsonData.Trim().Length == 0)
                    return "-1";



                try
                {
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonData);

                    //add new column
                    DataColumn columnSalesCode = new DataColumn("SALES_CODE");
                    columnSalesCode.DataType = typeof(String);
                    columnSalesCode.DefaultValue = obj;
                    dt.Columns.Add(columnSalesCode);

                    //add new column
                    DataColumn columnType = new DataColumn("TYPE");
                    columnType.DataType = typeof(String);
                    columnType.DefaultValue = type;
                    dt.Columns.Add(columnType);




                    using (SqlConnection connection = new SqlConnection(P5sCmm.P5sCmmFns.P5sGetConnectionString()))
                    {
                        connection.Open();
                        SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                        //I assume you have created the table previously 
                        //Someone else here already showed how   
                        bulkcopy.DestinationTableName = "L_INTERNET_DEVICE";

                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LogInternetDeviceStatus", "INTERNET_DEVICE_STATUS"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LogInternetDeviceCreatedDate", "INTERNET_DEVICE_CREATED_DATE"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "SALES_CODE"));
                        bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TYPE", "TYPE_CD"));


                        try
                        {
                            bulkcopy.WriteToServer(dt);
                            return "1";
                        }
                        catch (Exception EX)
                        {
                            // L5sMsg.Show(EX.Message);
                            return "-1";
                        }
                    }

                    return "-1";

                }
                catch (Exception ex)
                {
                    // L5sMsg.Show(ex.Message);
                    return "-1";
                }

            }
            catch (Exception)
            {

                return "-1";
            }


        }
        #endregion
        #region checkEmei & username & password
        //hàm chứng thực request từ HH lên Server có hợp lệ hay không
        //Các thông tin này sẽ giống với thông tin được lưu trữ trong dự án fsid.5stars.com.vn:9139 lấy từ bảng O_PROJECT_DETAIL
        private Boolean isPOCValidDevice(String imei, Boolean auth)
        {
            System.Web.UI.StateBag myViewState = new System.Web.UI.StateBag();
            L5sInitial.LoadForLoginPage();

            if (auth)
            {
                if (this.ServiceCredentials == null)
                    return false;

                if (this.ServiceCredentials.UserName == "5stars.com.vn-Nouser" && this.ServiceCredentials.Password == "#*&!@(*!@#&@#&@!6^@!@##@6382734") ;
                else
                    return false;
            }

            return true;
        }

        //hàm chứng thực request từ PC lên Server có hợp lệ hay không
        //Các thông tin này sẽ giống với thông tin được lưu trữ trong dự án fsid.5stars.com.vn:9139 lấy từ bảng O_PROJECT_DETAIL
        private Boolean isPOCValidAuthPC()
        {
            System.Web.UI.StateBag myViewState = new System.Web.UI.StateBag();
            L5sInitial.LoadForLoginPage();

            if (this.ServiceCredentials == null)
                return false;

            if (this.ServiceCredentials.UserName == "5stars.com.vn-Nouser" && this.ServiceCredentials.Password == "#*&!@(*!@#&@#&@!6^@!@##@6382734") ;
            else
                return false;

            return true;
        }


        private Boolean isPOCValidDevice(String imei)
        {
            return this.isPOCValidDevice(imei, true);
        }

        #endregion
        #region synchronize get chương trinh 5p tracking
        // Hàm get vòng xoay và các thông số cài đặt về số lần quay trong này
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGet5PParams(String imei, String obj, String type)
        {
            try
            {
                if (!this.isPOCValidDevice(imei))
                    return "-1";
                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");
                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGet5PParamsForAll();
                    case "2": // Sup
                        return this.synchronizePOCGet5PParamsForAll();
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGet5PParamsForAll()
        {

            //String url = HttpContext.Current.Request.Url.Authority + "/FileUpload/PrizeProgram/";
            //if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
            //    url = "https://" + url;
            //else
            //    url = "http://" + url;

            String StrGet5PTracking = string.Format(@"														
								SELECT PROGRAM_TRACKING_CD,PROGRAM_TRACKING_NAME,TYPE FROM O_POC_PROGRAM_TRACKING
	                            WHERE CREATEDTRACKING=1
	                            AND ACTIVE=1
	                            AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,BEGIN_DATE,103) AND CONVERT(DATE,END_DATE,103)");
            try
            {
                DataTable dt = P5sSql.Query(StrGet5PTracking);
                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }
        }
        // Hàm get product cho chương trình 5ptracking distribution
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetProduct5PTrackingDistribution(String imei, String obj, String type)
        {
            try
            {
                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");

                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGetProduct5PTrackingDistributionForAll();
                    case "2": // Sup
                        return this.synchronizePOCGetProduct5PTrackingDistributionForAll();
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGetProduct5PTrackingDistributionForAll()
        {
            String url = HttpContext.Current.Request.Url.Authority + "/FileUpload/Program5P/";
            if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
                url = "https://" + url;
            else
                url = "http://" + url;

            String StrGetPrize = @" DECLARE @URL NVARCHAR(200)
                                    SET @URL=@0
                                    SELECT OP.PRODUCT_SIZE_CD as PRODUCT_CD,OP.PRODUCT_SIZE_CODE as PRODUCT_CODE,OP.PRODUCT_SIZE_NAME as PRODUCT_NAME,@URL + OP.IMAGE AS IMAGE,OPT.PROGRAM_TRACKING_CD 
                                    FROM O_POC_PRODUCT_SIZE OP
                                    INNER JOIN O_POC_PROGRAM_DISTRIBUTION_TRACKING_PRODUCTSIZE OPTP ON OP.PRODUCT_SIZE_CD= OPTP.PRODUCT_SIZE_CD AND OPTP.ACTIVE=1
                                    INNER JOIN O_POC_PROGRAM_TRACKING OPT ON OPTP.PROGRAM_TRACKING_CD= OPT.PROGRAM_TRACKING_CD 
                                    WHERE OPT.ACTIVE=1 
                                    AND OPT.CREATEDTRACKING=1
                                    AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,OPT.BEGIN_DATE,103) AND CONVERT(DATE,OPT.END_DATE,103)";

            try
            {
                DataTable dt = P5sSql.Query(StrGetPrize, url);

                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }

        }
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public string synchronizePOCGetBrandCategoryShareOfShelf(String imei, String obj, String type)
        {
            try
            {
                //if (!this.isValidDevice(imei, false))
                //    return this.encrypt("-1");
                if (!this.isPOCValidDevice(imei))
                    return "-1";

                if (obj.Trim().Length == 0)
                    return "-1";

                switch (type)
                {
                    case "1": // Sales
                        return this.synchronizePOCGetBrandCategoryShareOfShelfForAll();
                    case "2": // Sup
                        return this.synchronizePOCGetBrandCategoryShareOfShelfForAll();
                    case "3": //ASM
                        return "1";
                    default:
                        return "-1";
                }
            }
            catch (Exception)
            {

                return "-1";
            }
        }
        private string synchronizePOCGetBrandCategoryShareOfShelfForAll()
        {
            String StrGetPrize = @"    SELECT OPTBC.PROGRAM_TRACKING_BRAND_CATEGORY_CD as CD,OPTBC.PROGRAM_TRACKING_BRAND_CATEGORY_NAME AS NAME,OPTBC.PROGRAM_TRACKING_BRAND_CATEGORY_CD,OPTBC.PROGRAM_TRACKING_CD, 0 AS TYPE 
                                        FROM O_POC_PROGRAM_TRACKING OPT
                                        INNER JOIN O_POC_PROGRAM_TRACKING_BRAND_CATEGORY OPTBC ON OPT.PROGRAM_TRACKING_CD= OPTBC.PROGRAM_TRACKING_CD AND OPTBC.ACTIVE=1
                                        WHERE OPT.ACTIVE=1 AND OPT.CREATEDTRACKING=1 
                                        AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,OPT.BEGIN_DATE,103) AND CONVERT(DATE,OPT.END_DATE,103)

                                        UNION ALL
                                        SELECT OPTB.PROGRAM_TRACKING_BRAND_CD as cd,OPTB.PROGRAM_TRACKING_BRAND_NAME AS NAME,OPTB.PROGRAM_TRACKING_BRAND_CATEGORY_CD,OPTBC.PROGRAM_TRACKING_CD ,1 AS TYPE
                                        FROM O_POC_PROGRAM_TRACKING_BRAND OPTB
                                        INNER JOIN O_POC_PROGRAM_TRACKING_BRAND_CATEGORY OPTBC ON OPTB.PROGRAM_TRACKING_BRAND_CATEGORY_CD= OPTBC.PROGRAM_TRACKING_BRAND_CATEGORY_CD AND OPTBC.ACTIVE=1
                                        INNER JOIN O_POC_PROGRAM_TRACKING OPT ON OPTBC.PROGRAM_TRACKING_CD = OPT.PROGRAM_TRACKING_CD
                                        WHERE OPT.ACTIVE=1 AND OPT.CREATEDTRACKING=1 AND OPTB.ACTIVE=1
                                        AND CONVERT(DATE,GETDATE(),103) BETWEEN CONVERT(DATE,OPT.BEGIN_DATE,103) AND CONVERT(DATE,OPT.END_DATE,103)

                                     ";

            try
            {
                DataTable dt = P5sSql.Query(StrGetPrize);

                if (dt == null || dt.Rows.Count == 0)
                    return "1";
                return this.POCGetJSONString(dt);
            }
            catch (Exception)
            {
                return "-1";
            }

        }



        #endregion

        # region this method is for downloading Route.txt
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCGetRouteListViaSalesmanCode(String imei, String salesmanCode)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            DataTable dt = L5sSql.Query(@" SELECT mRoute.ROUTE_CODE, mRoute.ROUTE_NAME, mRoute.ROUTE_CD
                                            from O_SALES_ROUTE osr 
                                            left join M_SALES mSales on osr.SALES_CD=mSales.SALES_CD
                                            left join M_ROUTE mRoute on mRoute.ROUTE_CD=osr.ROUTE_CD
                                            where mSales.SALES_CODE = @0", salesmanCode);
            return this.POCP5sConvertRouteListToJson(dt);
        }
        private String POCP5sConvertRouteListToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCRouteForASMervisor> p = new List<POCRouteForASMervisor>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String RouteCode = dt.Rows[i]["ROUTE_CODE"].ToString();
                String RouteName = dt.Rows[i]["ROUTE_NAME"].ToString();
                String RouteCd = dt.Rows[i]["ROUTE_CD"].ToString();
                POCRouteForASMervisor r = new POCRouteForASMervisor(RouteCode, RouteName, RouteCd);
                p.Add(r);
            }

            return oSerializer.Serialize(p);
        }
        #endregion
        #region this method is for downloading CUSTOMER.txt
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCGetCustomerViaRuoteCD(string imei, string _ArrRouteCd)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            string sql = string.Format(@"select mCustomer.CUSTOMER_CODE,concat(mCustomer.CUSTOMER_CODE,'*',mCustomer.CUSTOMER_NAME) AS CUS_ADD, mCustomer.CUSTOMER_NAME, mCustomer.CUSTOMER_ADDRESS , mCustomer.CUSTOMER_CHAIN_CODE
                                        from M_CUSTOMER mCustomer
                                        inner join O_CUSTOMER_ROUTE ocr on ocr.CUSTOMER_CD = mCustomer.CUSTOMER_CD
                                        inner join M_ROUTE mRoute on mRoute.ROUTE_CD = ocr.ROUTE_CD
                                        where mRoute.ROUTE_CD in ({0})", _ArrRouteCd);
            DataTable dt = L5sSql.Query(sql);
            return this.POCP5sConvertCustomer1ToJson(dt);
        }
        private String POCP5sConvertCustomer1ToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCCCustomer> cCustomers = new List<POCCCustomer>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String CUSTOMER_CODE = dt.Rows[i]["CUSTOMER_CODE"].ToString();
                String CUS_ADD = dt.Rows[i]["CUS_ADD"].ToString();
                String CUSTOMER_NAME = dt.Rows[i]["CUSTOMER_NAME"].ToString();
                String CUSTOMER_ADDRESS = dt.Rows[i]["CUSTOMER_ADDRESS"].ToString();
                String CUSTOMER_CHAIN_CODE = dt.Rows[i]["CUSTOMER_CHAIN_CODE"].ToString();

                POCCCustomer r = new POCCCustomer(CUSTOMER_CODE, CUS_ADD, CUSTOMER_NAME, CUSTOMER_ADDRESS, CUSTOMER_CHAIN_CODE);
                cCustomers.Add(r);
            }

            return oSerializer.Serialize(cCustomers);
        }
        #endregion
        #region this method is for downloading CUSTBRD.txt ,CUSTSKU.txt
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCGetCUSTBRD_CUSTSKUViaRuoteCD(string imei, string _ArrRouteCd)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            string sql = string.Format(@"select mCustomer.CUSTOMER_CODE
                                            from M_CUSTOMER mCustomer
                                            inner join O_CUSTOMER_ROUTE ocr on ocr.CUSTOMER_CD = mCustomer.CUSTOMER_CD
                                            inner join M_ROUTE mRoute on mRoute.ROUTE_CD = ocr.ROUTE_CD
                                            where mRoute.ROUTE_CD in ({0}) ", _ArrRouteCd);
            DataTable dt = L5sSql.Query(sql);
            return this.POCP5sConvert_CUSTBRD_CUSTSKU_ToJson(dt);
        }
        private String POCP5sConvert_CUSTBRD_CUSTSKU_ToJson(DataTable dt)
        {
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
        #endregion
        #region this method is for downloading SALESMAN.txt
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCGetSalesmanViaSalesmanCode(String imei, String salesmanCode)
        {

            if (!this.isPOCValidDevice(imei))
                return "-1";

            DataTable dt = L5sSql.Query(@"select mSales.SALES_CODE, mSales.SALES_NAME from M_SALES mSales where mSales.SALES_CODE = @0", salesmanCode);
            return this.POCP5sConvertSalesmanToJson(dt);
        }
        private String POCP5sConvertSalesmanToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCSalesmanInfo> CUSTBRDs = new List<POCSalesmanInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String SALES_CODE = dt.Rows[i]["SALES_CODE"].ToString();
                String SALES_NAME = dt.Rows[i]["SALES_NAME"].ToString();
                POCSalesmanInfo salesman = new POCSalesmanInfo(SALES_CODE, SALES_NAME);
                CUSTBRDs.Add(salesman);
            }

            return oSerializer.Serialize(CUSTBRDs);
        }
        public class POCSalesmanInfo
        {
            public String SalesCode;
            public String SalesName;
            public POCSalesmanInfo(String SalesCode, String SalesName)
            {
                this.SalesCode = SalesCode;
                this.SalesName = SalesName;
            }
        }

        public class POCStockiesInfo
        {
            public String DistributorCd;
            public String DistributorName;
            public POCStockiesInfo(String DistributorCd, String DistributorName)
            {
                this.DistributorCd = DistributorCd;
                this.DistributorName = DistributorName;
            }
        }
        #endregion
        #region this method is for downloading STOCKIEST.txt
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCGetSTOCKIESTViaSalesmanCode(String imei, String salesmanCode)
        {
            if (!this.isPOCValidDevice(imei))
                return "-1";

            DataTable dt = L5sSql.Query(@"select md.DISTRIBUTOR_CD, md.DISTRIBUTOR_NAME from M_DISTRIBUTOR md 
                                        inner join M_SALES mSales on mSales.DISTRIBUTOR_CD = md.DISTRIBUTOR_CD
                                        where mSales.SALES_CODE = @0", salesmanCode);
            return this.POCP5sConvertSTOCKIESTToJson(dt);
        }
        private String POCP5sConvertSTOCKIESTToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
                return "1";

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            List<POCStockiesInfo> STOCKIESTs = new List<POCStockiesInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String DISTRIBUTOR_CD = dt.Rows[i]["DISTRIBUTOR_CD"].ToString();
                String DISTRIBUTOR_NAME = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();
                POCStockiesInfo stockiesInfo = new POCStockiesInfo(DISTRIBUTOR_CD, DISTRIBUTOR_NAME);
                STOCKIESTs.Add(stockiesInfo);
            }

            return oSerializer.Serialize(STOCKIESTs);
        }
        #endregion
        #region this method is for downloading default data for HH BRAND.TXT, CATEGORY.TXT, PRODUCT.TXT, REASON.TXT, SALESMAN_PRODUCTIVITY.TXT, SCHEME.TXT, SUBBRAND.TXT
        /**
         * put the default file text to the folder : D:\MMV_Application\CP_Sync\cphh\frompc\data\
         * */
        [WebMethod]
        [SoapHeader("ServiceCredentials")]
        public String synchronizePOCGetConfigDataFromCP(String imei, String obj)
        {
            //if (!this.isValidDevice(imei, false))
            //    return "-1";
            //if (!this.isValidDevice(imei))
            //    return "-1";

            if (obj.Trim().Length == 0)
                return "-1";

            try
            {
                String pathFolder = System.Configuration.ConfigurationSettings.AppSettings["SyncDirectoryData"].ToString();
                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                    return "-1";
                }

                //get folder valid to download
                DirectoryInfo d = new DirectoryInfo(pathFolder);
                FileInfo[] data = d.GetFiles("*.txt", SearchOption.TopDirectoryOnly);

                List<POCCTxtFromPC> result = new List<POCCTxtFromPC>();
                for (int i = 0; i < data.Length; i++)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(data[i].FullName);
                    result.Add(new POCCTxtFromPC(data[i].Name, System.Text.Encoding.UTF8.GetString(bytes).Replace("\"", "")));
                }

                List<POCCCustomer> customers = new List<POCCCustomer>();
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                             new System.Web.Script.Serialization.JavaScriptSerializer();
                oSerializer.MaxJsonLength = Int32.MaxValue;
                return oSerializer.Serialize(result);
            }
            catch (Exception)
            {
                return "-1";
            }

        }
        #endregion

        //=== POC ========================================================
      

    }
}
