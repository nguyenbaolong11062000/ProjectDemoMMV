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

namespace MMV
{
    public partial class ImportSalesTarget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
        }

        protected void P5sSearch_Click(object sender, EventArgs e)
        {
    
        }


        protected void P5sLbtnSaveFile_Click(object sender, EventArgs e)
        {
            string fileName = Path.GetFileName(this.P5sFileUpload.PostedFile.FileName);
            string extension = Path.GetExtension(this.P5sFileUpload.PostedFile.FileName);
            if (P5sFileUpload.HasFile)
            {
                
               

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
                
                //if (!dt.Columns[0].ColumnName.Equals("Template upload Monthly Target for DSRs"))
                //{
                //    L5sMsg.Show("File upload không phải file được xuất từ hệ thống .");
                //    return;
                //}

              
                //remove row 0
               // dt.Rows.RemoveAt(0);//row header


                for(int row=0; row<dt.Rows.Count;row++){
                    for (int col = 4; col < dt.Columns.Count; col++ )
                    {
                        if (dt.Rows[row][col].ToString().Equals(""))
                        {
                            dt.Rows[row][col] = 0;                            
                        }
                    }
                }
                ViewState["IMPORT_SALES_TARGET_TEMP"] = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();

                ////Change Column Name
                dt.Columns[0].ColumnName = "DISTRIBUTOR_CODE";
                dt.Columns[1].ColumnName = "SALES_CODE";
                dt.Columns[2].ColumnName = "MONTH";
                dt.Columns[3].ColumnName = "YEAR";
                dt.Columns[4].ColumnName = "TOTAL_SALES_TARGET";
                dt.Columns[5].ColumnName = "TP_SALES_TARGET";
                dt.Columns[6].ColumnName = "TB_SALES_TARGET";

                //add new column
                DataColumn objDataColumn = new DataColumn();
                objDataColumn.DefaultValue = ViewState["IMPORT_SALES_TARGET_TEMP"].ToString();
                objDataColumn.ColumnName = "IMPORT_SALES_TARGET_TEMP";
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
                    bulkcopy.DestinationTableName = "TMP_IMPORT_SALES_TARGET";


                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IMPORT_SALES_TARGET_TEMP", "IMPORT_SALES_TARGET_CD"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DISTRIBUTOR_CODE", "DISTRIBUTOR_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "SALES_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("MONTH", "MONTH"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("YEAR", "YEAR"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TOTAL_SALES_TARGET", "TOTAL_SALES_TARGET"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TP_SALES_TARGET", "TP_SALES_TARGET"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("TB_SALES_TARGET", "TB_SALES_TARGET"));


                    try
                    {
                        bulkcopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        //for(int row = 0; row<dt.Rows.Count; row++){
                        //    for(int col=0; column<dt.Columns.Count; col++){

                        //    }
                        //}
                        P5sCmmFns.P5sWriteHistory("ImportSalesTarget", false, fileName);
                        L5sMsg.Show(ex.Message);
                        return;
                    }
                }



            
                //check data valid
                String tmpViewState = ViewState["IMPORT_SALES_TARGET_TEMP"].ToString();


                //UPDATE COLUMN SALES_TARGET_DATE
                L5sSql.Execute(@"  UPDATE TMP_IMPORT_SALES_TARGET SET SALES_TARGET_YYYYMM = [YEAR]*100 + MONTH
                                     WHERE IMPORT_SALES_TARGET_CD = @0  ;", tmpViewState);
        
                //CHECK VALID VALUE
                String result = "";
                if (!this.P5sCheckSales(tmpViewState, out result))
                {
                    L5sMsg.Show(String.Format("Import fail, list of DSRs not exists on system: {0}", result));
                    this.RollbackFinish(tmpViewState);
                    return;
                }


                //DELETE SALES TARGET EXISTS
                L5sSql.Execute(@"  DELETE   FROM O_SALES_TARGET 
                                    WHERE EXISTS
                                    (
	                                    SELECT * FROM  TMP_IMPORT_SALES_TARGET tmp WHERE tmp.SALES_CD = O_SALES_TARGET.SALES_CD AND tmp.SALES_TARGET_YYYYMM = O_SALES_TARGET.SALES_TARGET_YYYYMM
	                                    AND tmp.IMPORT_SALES_TARGET_CD = @0
                                    ) ;", tmpViewState);

                //INSERT DATA 

                L5sSql.Execute(@"  INSERT INTO [dbo].[O_SALES_TARGET]
                                               ([SALES_CODE]
                                               ,[SALES_CD]
                                               ,[TOTAL_SALES_TARGET]
                                               ,[TP_SALES_TARGET]
                                               ,[TB_SALES_TARGET]
                                               ,[SALES_TARGET_YYYYMM]
                                               )
                                    SELECT [SALES_CODE],[SALES_CD],ISNULL([TOTAL_SALES_TARGET],0),ISNULL([TP_SALES_TARGET],0),ISNULL([TB_SALES_TARGET],0),[SALES_TARGET_YYYYMM] 
                                    FROM TMP_IMPORT_SALES_TARGET
                                    WHERE IMPORT_SALES_TARGET_CD = @0 AND SALES_CD IS NOT NULL

                                

                             ;", tmpViewState);


                this.RollbackFinish(tmpViewState);
                P5sCmmFns.P5sWriteHistory("ImportSalesTarget", true, fileName);
                L5sMsg.Show("Import successfull!.");

             }

            else
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (!this.P5sValidExtenstion(extension))
                    {
                        L5sMsg.Show("Extension of file is not valid!.");
                        return;
                    }
                    else
                    {
                        L5sMsg.Show("File no data!.");
                        return;
                    }
                }
                else
                {
                    L5sMsg.Show("Please select file!.");
                    return;
                }
            }
        }

        private bool P5sCheckSales(string tmpViewState, out string result)
        {
            result = "";
            String sql = @"
                         --SALES_TARGET OF DISTRIBUTOR    
                           UPDATE tmp SET SALES_CD = sls.SALES_CD
                           FROM TMP_IMPORT_SALES_TARGET tmp INNER JOIN M_SALES sls ON tmp.SALES_CODE = sls.SALES_CODE
                           WHERE tmp.IMPORT_SALES_TARGET_CD = @0  
                        ";
            L5sSql.Execute(sql, tmpViewState);

            return true;//   //không check TH NVBH không ton tai trên hệ thóng theo y/c của KH

            sql = @"SELECT DISTINCT SALES_CODE AS SALES_CODE 
                    FROM TMP_IMPORT_SALES_TARGET tmp
                    WHERE IMPORT_SALES_TARGET_CD = @0 AND SALES_CD IS NULL 
                    ";

            DataTable dt = L5sSql.Query(sql, tmpViewState);


            if (dt.Rows.Count <= 0)
                return true;
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, "SALES_CODE");
                return false;
            }
        }

        private void RollbackFinish(string tmpViewState)
        {
            L5sSql.Execute(@"
                                DELETE TMP_IMPORT_SALES_TARGET WHERE IMPORT_SALES_TARGET_CD = @0 OR CREATED_DATE < GETDATE() - 1 

                            ", tmpViewState);
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

        
        
       protected void P5sLbtnExportTemplate_Click(object sender, EventArgs e)
       {
           String templatePath = Server.MapPath("~/Report/Templs/TemplateImportSalesTarget.xlsx");
           P5sEReport.P5sASPExportFileToClient(templatePath, "excel");
       }   

     
    }
}
