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
    public partial class InactiveCodeNotExistInFileMaster : System.Web.UI.Page
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

                ViewState["TMP_INACTIVE_CUSTOMER_SALES_ROUTE"] = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();

                ////Change Column Name

                dt.Columns[0].ColumnName = "CUSTOMER_CODE";
                dt.Columns[1].ColumnName = "SALES_CODE";
                dt.Columns[2].ColumnName = "ROUTE_CODE";
  


                //add new column
                DataColumn objDataColumn = new DataColumn();
                objDataColumn.DefaultValue = ViewState["TMP_INACTIVE_CUSTOMER_SALES_ROUTE"].ToString();
                objDataColumn.ColumnName = "TMP_INACTIVE_CUSTOMER_SALES_ROUTE";
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
                    bulkcopy.DestinationTableName = "TMP_INACTIVE_CUSTOMER_SALES_ROUTE";
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CUSTOMER_CODE", "CUSTOMER_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SALES_CODE", "SALES_CODE"));
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ROUTE_CODE", "ROUTE_CODE"));
                    
                    try
                    {
                        bulkcopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        P5sCmmFns.P5sWriteHistory("ImportCustomer", false, fileName);
                        L5sMsg.Show(ex.Message);
                        return;
                    }

                }
                String tmpViewState = ViewState["TMP_INACTIVE_CUSTOMER_SALES_ROUTE"].ToString();

                String sql = String.Format(@"
                                                          ---inactive customer ---
                                    update [M_CUSTOMER]
                                    set ACTIVE = 0
                                    where CUSTOMER_CODE not in
	                                    (
		                                    select t1.CUSTOMER_CODE
		                                    from (SELECT distinct tmp.CUSTOMER_CODE
		                                    FROM [M_CUSTOMER] cust
		                                    inner join [TMP_INACTIVE_CUSTOMER_SALES_ROUTE] tmp on cust.CUSTOMER_CODE = tmp.CUSTOMER_CODE
	                                    )t1
                                     )
                                    and CUSTOMER_CODE not in
                                     (
	                                    select cuspoc.CUSTOMER_CODE
	                                    from (select cust.*
		                                     from M_SALES sls
			                                     INNER JOIN [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
			                                     inner join O_SALES_ROUTE osr on osr.SALES_CD = sls.SALES_CD
			                                     inner join M_ROUTE mrout on mrout.ROUTE_CD = osr.ROUTE_CD
			                                     inner join O_CUSTOMER_ROUTE ocus on ocus.ROUTE_CD = mrout.ROUTE_CD
			                                     inner join M_CUSTOMER cust on cust.CUSTOMER_CD = ocus.CUSTOMER_CD
		                                     where dis.DISTRIBUTOR_CODE in ('109001','109002','900001')
		                                     )cuspoc
                                       )
                                    ---inactive sales---
                                    update M_SALES
                                    set ACTIVE = 0
                                    where SALES_CODE not in
	                                    (
		                                    select t1.SALES_CODE
		                                    from ( select distinct tmp.SALES_CODE
		                                    from M_SALES sls
		                                    inner join [TMP_INACTIVE_CUSTOMER_SALES_ROUTE] tmp on sls.SALES_CODE = tmp.SALES_CODE
		                                    ) t1
	                                    )
	                                    and SALES_CODE not in
	                                    (
		                                    select slspoc.SALES_CODE
		                                    from (select sls.*
		                                    from M_SALES sls
			                                     INNER JOIN [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
		                                     where dis.DISTRIBUTOR_CODE in ('109001','109002','900001')) slspoc
	                                    )


                                    --inactive route---
                                    update M_ROUTE
                                    set ACTIVE = 0
                                    where ROUTE_CODE not in
	                                    (
		                                    select t1.ROUTE_CODE
		                                    from (select distinct tmp.ROUTE_CODE
		                                    from M_ROUTE rout
		                                    inner join [TMP_INACTIVE_CUSTOMER_SALES_ROUTE] tmp on rout.ROUTE_CODE = tmp.ROUTE_CODE
		                                    ) t1
	                                    )and ROUTE_CODE not in
	                                    (
	                                    select routpoc.ROUTE_CODE
	                                    from (select mrout.*
                                         from M_SALES sls
		                                     INNER JOIN [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
		                                     inner join O_SALES_ROUTE osr on osr.SALES_CD = sls.SALES_CD
		                                     inner join M_ROUTE mrout on mrout.ROUTE_CD = osr.ROUTE_CD
	                                     where dis.DISTRIBUTOR_CODE in ('109001','109002','900001')) routpoc
	                                    )

                                    ---inactive O_CUSTOMER_ROUTE---
                                    update O_CUSTOMER_ROUTE
                                    set ACTIVE = 0
                                    from M_CUSTOMER cust
                                    inner join O_CUSTOMER_ROUTE ocr on cust.CUSTOMER_CD = ocr.CUSTOMER_CD
                                    where cust.ACTIVE = 0

                                    ----Inactive O_sales_route---
                                    update O_SALES_ROUTE
                                    set ACTIVE = 0
                                    from M_ROUTE rout
                                    inner join O_SALES_ROUTE osr on rout.ROUTE_CD = osr.ROUTE_CD
                                    where rout.ACTIVE = 0
                            ", tmpViewState);
                try
                {
                    P5sCmm.P5sCmmFns.SqlDatatableTimeout(sql, 36000);
                    P5sCmm.P5sCmmFns.P5sUpdateValueForMap();
                    P5sCmmFns.P5sWriteHistory("ImportCustomer", true, fileName);
                    L5sMsg.Show("Inactive successfull!");
                }
                catch (Exception ex)
                {
                    L5sMsg.Show("Inactive successfull!");

                }
                finally
                {
                    this.RollbackFinish(tmpViewState);

                }
            }

        }
        private void RollbackFinish(String tempViewState)
        {
            L5sSql.Execute(@"
                                DELETE TMP_INACTIVE_CUSTOMER_SALES_ROUTE  

                            ", tempViewState);

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
            String templatePath = Server.MapPath("~/Tool/Templs/TemplateInactiveCode.xlsx");
            P5sEReport.P5sASPExportFileToClient(templatePath, "excel");
        }
    }
}