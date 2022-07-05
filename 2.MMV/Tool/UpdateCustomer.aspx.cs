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

namespace MMV.Tool
{
    public partial class UpdateCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);

        }

        protected void P5sLbtnSaveFile_Click(object sender, EventArgs e)
        {
            if (P5sFileUpload.HasFile)
            {
                string fileName = Path.GetFileName(this.P5sFileUpload.PostedFile.FileName);
                string extension = Path.GetExtension(this.P5sFileUpload.PostedFile.FileName);


                if (!this.P5sValidExtenstion(extension))
                {
                    L5sMsg.Show("Extension of file is not valid!.");
                    return;
                }

                fileName = fileName.Replace(extension, "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + extension);
                string filePath = Server.MapPath("~/FileUpload/" + fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                P5sFileUpload.SaveAs(filePath);
                DataTable dt = P5sCmm.P5sCmmFns.readDataFromExcelFile(filePath, "MMV-FPIT", 2);
                if (dt == null)
                {
                    L5sMsg.Show("File uploads don't exported from system.");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    L5sMsg.Show("File is no data.");
                    return;
                }
                ViewState["UPDATE_CUSTOMER_TEMP"] = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();

                ////Change Column Name
               
                dt.Columns[0].ColumnName = "CUSTOMER_CODE";
                dt.Columns[1].ColumnName = "IDMS_CUSTOMER_CODE";
                dt.Columns[2].ColumnName = "SALES_CODE";
                dt.Columns[3].ColumnName = "IDMS_SALES_CODE";
                dt.Columns[4].ColumnName = "ROUTE_CODE";
                dt.Columns[5].ColumnName = "IDMS_ROUTE_CODE";


           

                DataColumn objDataColumn = new DataColumn();
                objDataColumn.DefaultValue = ViewState["UPDATE_CUSTOMER_TEMP"].ToString();
                objDataColumn.ColumnName = "UPDATE_CUSTOMER_TEMP";
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
                    bulkcopy.DestinationTableName = "TMP_UPDATE_CUSTOMER_SALES_ROUTE";


                    //bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UPDATE_CUSTOMER_TEMP", "UPDATE_CUSTOMER_TEMP"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CUSTOMER_CODE", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IDMS_CUSTOMER_CODE", "IDMS_CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "SALES_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IDMS_SALES_CODE", "IDMS_SALES_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ROUTE_CODE", "ROUTE_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IDMS_ROUTE_CODE", "IDMS_ROUTE_CODE"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        P5sCmmFns.P5sWriteHistory("UpdateCustomer", false, fileName);
                        L5sMsg.Show(ex.Message);
                        return;
                    }
                }
                String tmpViewState = ViewState["UPDATE_CUSTOMER_TEMP"].ToString();

             

                String sql = (@" --update code customer cũ thành code customer mới
                                              Update [M_CUSTOMER]
                                                set [CUSTOMER_CODE] = t1.[IDMS_CUSTOMER_CODE],
                                                    [IDMS_CUSTOMER_CODE] = t1.[CUSTOMER_CODE]
                                                FROM [TMP_UPDATE_CUSTOMER_SALES_ROUTE] t1
                                                Inner join [M_CUSTOMER] t2 on t1.[CUSTOMER_CODE] = t2.CUSTOMER_CODE
                                                and t1.[IDMS_CUSTOMER_CODE] = t2.IDMS_CUSTOMER_CODE
                                                 


                                               --update code sales cũ thành code sales mới
                                               Update [M_SALES]
                                                set [SALES_CODE] = t1.[IDMS_SALES_CODE],
                                                    [IDMS_SALES_CODE] = t1.[SALES_CODE]
                                                FROM [TMP_UPDATE_CUSTOMER_SALES_ROUTE] t1
                                                Inner join [M_SALES] t2 on t1.[SALES_CODE] = t2.[SALES_CODE]
                                                and t1.[IDMS_SALES_CODE] = t2.[IDMS_SALES_CODE]


                                                 --update code route cũ thành code route mới
                                                Update [M_ROUTE]
                                                set [ROUTE_CODE] = t1.[IDMS_ROUTE_CODE],
                                                    [IDMS_ROUTE_CODE] = t1.[ROUTE_CODE]
                                                FROM [TMP_UPDATE_CUSTOMER_SALES_ROUTE] t1
                                                Inner join [M_ROUTE] t2 on t1.[ROUTE_CODE] = t2.[ROUTE_CODE]
                                                and t1.[IDMS_ROUTE_CODE] = t2.[IDMS_ROUTE_CODE]



                                                        ");
                try
                {

                    P5sCmm.P5sCmmFns.SqlDatatableTimeout(sql, 36000);
                    P5sCmm.P5sCmmFns.P5sUpdateValueForMap();
                    P5sCmmFns.P5sWriteHistory("UpdateCustomer", true, fileName);
                    L5sMsg.Show("Update successfull!");
                }
                catch (Exception ex)
                {
                    L5sMsg.Show("Update successfull");

                }
                finally
                {
                    this.RollbackFinish(tmpViewState);

                }
            }
        }


        protected Boolean P5sValidExtenstion(String extension)
        {
            switch (extension)
            {
                case ".xls":
                    return false;
                case ".xlsx":
                    return true;
                default:
                    return false;
            }
        }

        protected void P5sSearch_Click(object sender, EventArgs e)
        {

        }

        protected void P5sLbtnExportTemplate_Click(object sender, EventArgs e)
        {
            String templatePath = Server.MapPath("~/Tool/Templs/TemplateUpdateDSKH.xlsx");
            P5sEReport.P5sASPExportFileToClient(templatePath, "excel");
        }
        private void RollbackFinish(String tempViewState)
        {
            L5sSql.Execute(@"
                                DELETE TMP_UPDATE_CUSTOMER_SALES_ROUTE

                            ", tempViewState);

        }

    }
}