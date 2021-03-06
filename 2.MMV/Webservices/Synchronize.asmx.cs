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

namespace MMV.Webservices
{
    /// <summary>
    /// Summary description for Synchronize
    /// </summary>
    [WebService(Namespace = "http://5stars.com.vn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Synchronize : System.Web.Services.WebService
    {
        #region webservice đồng bộ dữ liệu từ Server CP về MMV - 
        //thông tin đồng bộ về là SalesAmount cách thức hoạt động của hàm này giống như trang MasterData/ImportSalesAmount.aspx



        private DataTable GetDataSourceFromFile(String path)
        {

            DataTable dt = new DataTable();
            string[] columns = null;

            String[] lines = File.ReadAllLines(path);

            // assuming the first row contains the columns information
            if (lines.Length > 0)
            {
                columns = lines[0].Split(new char[] { '	' });
                foreach (String column in columns)
                    dt.Columns.Add(column);
            }

            // reading rest of the data
            for (int i = 1; i < lines.Length; i++)
            {
                DataRow dr = dt.NewRow();
                String[] values = lines[i].Split(new char[] { '	' });

                for (int j = 0; j < values.Length && j < columns.Length; j++)
                    dr[j] = values[j];

                dt.Rows.Add(dr);
            }
            return dt;


        }

        [WebMethod]
        public string synchronizeDataFromCP(String userName, String password, String jsonData)
        {
            System.Web.UI.StateBag myViewState = new System.Web.UI.StateBag();
            L5sInitial.LoadForLoginPage();
            String userLogin = P5sCmm.P5sCmmFns.convertToMD5("Uploader");
            String passwordLogin = P5sCmm.P5sCmmFns.convertToMD5("Nopass#");

            if( !userLogin.Equals(userName) || !passwordLogin.Equals(password) )
                 return "100";

            String filePath = Server.MapPath("~/FileUpload/" + String.Format("synchronizeDataFromCP_{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss") ) );


            File.WriteAllText(filePath,jsonData );

            //FileUpload
            try
            {
                  DataTable dt = new DataTable();
                  String guiCd = System.Guid.NewGuid().ToString();
                  //HÀM IMPORT NÀY GIỐNG NHƯ HÀM TRONG TRANG ImportSalesAmount.aspx 
                  dt = this.GetDataSourceFromFile(filePath); // JsonConvert.DeserializeObject<DataTable>(jsonData);
                 
                 

                  ////Change Column Name
                  dt.Columns[0].ColumnName = "DISTRIBUTOR_CODE";
                  dt.Columns[1].ColumnName = "SALES_CODE";
                  dt.Columns[2].ColumnName = "CUSTOMER_CODE";
                  dt.Columns[3].ColumnName = "DAY";
                  dt.Columns[4].ColumnName = "MONTH";
                  dt.Columns[5].ColumnName = "YEAR";
                  dt.Columns[6].ColumnName = "TOTAL_SALES";
                  dt.Columns[7].ColumnName = "SALES_TP";
                  dt.Columns[8].ColumnName = "SALES_TB";
                  dt.Columns[9].ColumnName = "ORDER_NUMBER";
                    




                  //add new column
                  DataColumn objDataColumn = new DataColumn();
                  objDataColumn.DefaultValue = guiCd;
                  objDataColumn.ColumnName = "IMPORT_CUSTOMER_TEMP";
                  dt.Columns.Add(objDataColumn);


                  SqlConnection Conn = (SqlConnection)null;

                  string connectionString = ConfigurationSettings.AppSettings["ConStr"];
                  if (ConfigurationSettings.AppSettings["sa"] == "1" && HttpContext.Current.Session["L5sSAU"] != null)
                      connectionString = connectionString.Substring(0, connectionString.ToLower().IndexOf("user id")) +
                                         (object)"User ID=fs" + (string)HttpContext.Current.Session["L5sSAU"] + "; " +
                                         connectionString.Substring(connectionString.ToLower().IndexOf("password"));


                  using (SqlConnection connection = new SqlConnection(connectionString))
                  {
                      connection.Open();
                      SqlBulkCopy bulkcopy = new SqlBulkCopy(connection);
                      //I assume you have created the table previously 
                      //Someone else here already showed how   
                      bulkcopy.DestinationTableName = "TMP_IMPORT_SALES_AMOUNT";
                      
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IMPORT_CUSTOMER_TEMP", "IMPORT_SALES_AMOUNT_CD"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CODE", "DISTRIBUTOR_CODE"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CUSTOMER_CODE", "CUSTOMER_CODE"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "SALES_CODE"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DAY", "DAY"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("MONTH", "MONTH"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YEAR", "YEAR"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TOTAL_SALES", "TOTAL_SALES"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_TP", "SALES_TP"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_TB", "SALES_TB"));
                      bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ORDER_NUMBER", "NUMBER_OF_ORDER"));

                      try
                      {
                          bulkcopy.WriteToServer(dt);
                      }
                      catch (Exception ex)
                      {
                          //L5sMsg.Show(ex.Message);
                          return ex.Message;
                      }
                  }

                  //check data valid
                  String tmpViewState = guiCd;
                  //INSERT DATA 
                  //chỉ cập nhật doanh số cho các KH đã tồn tại trên hệ thống
                  P5sCmm.P5sCmmFns.P5sInsertSalesAmount(tmpViewState);

                  return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;    
            }
        }
        #endregion


    }
}
