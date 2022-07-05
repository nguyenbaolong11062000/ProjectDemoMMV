using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using P5sCmm;
using P5sDmComm;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using OfficeOpenXml;
using System.Globalization;

namespace MMV.Tool
{
    public class UploadFile
    {

        //Check File Extenstion
        public Boolean P5sValidExtenstion(String extension)
        {
            switch (extension)
            {
                case ".txt":
                    return true;
                case ".xlsx":
                    return false;
                default:
                    return false;
            }
        }

        //Check duplicate by sql
        //input: sql check dupl, column name get data show alert
        public bool P5sCheckDuplicateOrDataExists(string sql, string cols, out string result)
        {
            result = "";


            DataTable dt = L5sSql.Query(sql);

            if (dt.Rows.Count <= 0)
                return true;
            else
            {
                result = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dt, cols);
                return false;
            }
        }

        //Movefile from path to path2
        //input: path, path2, filename
        public void Move_File(string path, string path2, string filename)
        {
            string date = DateTime.Today.ToString("yyyyMMdd");
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(path2 + date);
                if (!File.Exists(path + filename))
                {
                    // This statement ensures that the file is created,
                    // but the handle is not kept.

                    using (FileStream fs = File.Create(path + filename)) { }
                }

                // Ensure that the target does not exist.
                if (File.Exists(path2 + "\\" + date + "\\" + filename))
                    File.Delete(path2 + "\\" + date + "\\" + filename);

                // Move the file.
                File.Move(path + filename, path2 + "\\" + date + "\\" + filename);

            }
            catch (Exception ex)
            {
                Console.WriteLine("The process failed: {0}", ex.ToString());
            }
        }

        //Delete table
        //input: table name
        public void RollbackFinish(String tbName)
        {
            L5sSql.Execute(1, string.Format("DELETE {0}", tbName));
        }

        //Read excels, import temp table
        public void ReadExcel(string fileName, string filePath, string TempTable, string sheetName, int rowBegin, string[] arr, string active)
        {
            DataTable dt = P5sCmm.P5sCmmFns.readDataFromExcelFile(filePath, sheetName, rowBegin);

            if (dt == null)
            {

                L5sMsg.Show("File uploads don't exported from system.");
            }
            if (dt.Rows.Count == 0)
            {
                L5sMsg.Show(string.Format("File {0} is no data.", fileName));
                return;
            }

            //ViewState["IMPORT_CD"] = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();

            //string tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();
            //add new column
            DataColumn objDataColumn = new DataColumn();
            // objDataColumn.DefaultValue = tmpViewState;
            // objDataColumn.ColumnName = colsCD;
            //dt.Columns.Add(objDataColumn);

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
                bulkcopy.DestinationTableName = TempTable;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string tp = dt.Columns[j].ToString();
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(tp, arr[j].ToString()));
                }
                if (active != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][active].ToString().Equals("1"))
                            dt.Rows[i][active] = Boolean.TrueString;
                        else if (dt.Rows[i][active].ToString().Equals("0"))
                            dt.Rows[i][active] = Boolean.FalseString;
                        else
                            dt.Rows[i][active] = null;
                    }
                }

                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    //ghi log vao table H_UPLOAD_FILE
                    //P5sCmmFns.P5sWriteHistory("ImportCR19_DOC_LISTING", false, fileNameTransaction);
                    L5sMsg.Show(ex.Message);
                    return;
                }
            }
        }


        public DataTable ConvertToDataTable(string filePath, int numberOfColumns, int columndr)
        {
            DataTable tbl = new DataTable();

            for (int col = 0; col < numberOfColumns; col++)
                tbl.Columns.Add(new DataColumn("Column" + (col + 1).ToString()));

            if (!File.Exists(filePath))
            {
                return tbl;
            }

            string[] lines = System.IO.File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                //cắt chuổi line thành từng cột dựa theo khoảng \t
                string[] cols = line.Split('\t');
                DataRow dr = tbl.NewRow();
                for (int cIndex = 0; cIndex < columndr; cIndex++)
                {
                    try
                    {
                        dr[cIndex] = cols[cIndex];
                    }
                    catch (Exception ex)
                    {
                        P5sCmmFns.P5sWriteHistory("Autoupload", false, "", ex.Message);
                        return null;
                    }
                }

                tbl.Rows.Add(dr);

            }
            //xóa header
            tbl.Rows.Remove(tbl.Rows[0]);
            return tbl;
        }

        public DataTable ConvertOrderSalesToDataTable(string filePath, int numberOfColumns, int columndr)
        {
            DataTable tbl = new DataTable();

            for (int col = 0; col < numberOfColumns; col++)
                tbl.Columns.Add(new DataColumn("Column" + (col + 1).ToString()));

            if (!File.Exists(filePath))
            {
                return tbl;
            }

            string[] lines = System.IO.File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                //cắt chuổi line thành từng cột dựa theo khoảng \t
                string[] cols = line.Split('\t');
                DataRow dr = tbl.NewRow();
                for (int cIndex = 0; cIndex < columndr; cIndex++)
                {
                    try
                    {
                        dr[cIndex] = cols[cIndex];
                    }
                    catch (Exception ex)
                    {
                        P5sCmmFns.P5sWriteHistory("Autoupload", false, "", ex.Message);
                        return null;
                    }
                }
                tbl.Rows.Add(dr);
            }
            //xóa header
            tbl.Rows.Remove(tbl.Rows[0]);
            return tbl;
        }
        public string ReadTXTFromFolder(string control, string name, string path, string TempTable, int numberofcolumn, string[] arr, string active, string colsCD, out String tmpViewState, int columndr, string Dist_cols)
        {
            string fileName = name;
            string extension = ".txt";

            fileName = fileName.Replace(extension, "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + extension);
            string filePath = path;

            //read file txt
            DataTable dt = ConvertToDataTable(filePath, numberofcolumn, columndr);
            if (dt == null)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File uploads don't exported from system.");
                tmpViewState = "";
                return "-1";
            }


            if (dt.Rows.Count == 0)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File is no data.");
                tmpViewState = "";
                return "-1";
            }

            tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();
            //add new column
            DataColumn objDataColumn = new DataColumn();
            objDataColumn.DefaultValue = tmpViewState;
            objDataColumn.ColumnName = colsCD;
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
                bulkcopy.DestinationTableName = TempTable;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string tp = dt.Columns[j].ToString();
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(tp, arr[j].ToString()));
                }
                if (active != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][active].ToString().Equals("1"))
                            dt.Rows[i][active] = Boolean.TrueString;
                        else if (dt.Rows[i][active].ToString().Equals("0"))
                            dt.Rows[i][active] = Boolean.FalseString;
                        else
                            dt.Rows[i][active] = null;
                    }
                }
                if (Dist_cols != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][Int32.Parse(Dist_cols)] = dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Substring(dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Length - 6, 6);
                    }
                }
                try
                {
                    bulkcopy.WriteToServer(dt);

                }
                catch (Exception ex)
                {

                    //ghi log vao table H_UPLOAD_FILE
                    P5sCmmFns.P5sWriteHistory(control, false, name, ex.ToString());
                    return "-1";
                }

            }
            return "1";

        }

        public string ReadSalesHDataFromFolder(string control, string name, string path, string TempTable, int numberofcolumn, string[] arr, string active, string colsCD, out String tmpViewState, int columndr)
        {
            string fileName = name;
            string extension = ".txt";

            fileName = fileName.Replace(extension, "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + extension);
            string filePath = path;

            //read file txt
            DataTable dt = ConvertOrderSalesToDataTable(filePath, numberofcolumn, columndr);


            //Định dạng lại dữ liệu tiền tệ của thái lan (đem dấu trừ '-' phía sau dữ liệu số ra phía trước)
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int[] num = { 13, 14, 15, 16, 17, 18 };// Các cột cần kiểm tra
                foreach (int n in num)
                {
                    string str = dt.Rows[i][n].ToString().Trim();
                    if (str.IndexOf('-') > 0)
                    {
                        dt.Rows[i][n] = '-' + str.Replace("-", string.Empty);
                    }else if(str.IndexOf('-')==0 && str.Length==1)
                    {
                        dt.Rows[i][n] = "";
                    }
                }
                try
                {
                    dt.Rows[i][10] = DateTime.ParseExact(dt.Rows[i][10].ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy/MM/dd HH:mm:ss");
                }
                catch
                {
                    dt.Rows[i][10] = "";
                }
                try
                {
                    dt.Rows[i][11] = DateTime.ParseExact(dt.Rows[i][11].ToString(), "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy/MM/dd HH:mm:ss");
                }
                catch
                {
                    dt.Rows[i][11] = "";
                }
                
                
            }
            if (dt == null)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File uploads don't exported from system.");
                tmpViewState = "";
                return "-1";
            }


            if (dt.Rows.Count == 0)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File is no data.");
                tmpViewState = "";
                return "-1";
            }

            tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();
            //add new column
            DataColumn objDataColumn = new DataColumn();
            objDataColumn.DefaultValue = tmpViewState;
            objDataColumn.ColumnName = colsCD;
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
                bulkcopy.DestinationTableName = TempTable;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string tp = dt.Columns[j].ToString();
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(tp, arr[j].ToString()));
                }
                if (active != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][active].ToString().Equals("1"))
                            dt.Rows[i][active] = Boolean.TrueString;
                        else if (dt.Rows[i][active].ToString().Equals("0"))
                            dt.Rows[i][active] = Boolean.FalseString;
                        else
                            dt.Rows[i][active] = null;
                    }
                }

                //if (Dist_cols != "")
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        dt.Rows[i][Int32.Parse(Dist_cols)] = dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Substring(dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Length - 6, 6);
                //    }
                //}
                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {

                    //ghi log vao table H_UPLOAD_FILE
                    P5sCmmFns.P5sWriteHistory(control, false, name, ex.ToString());
                    return "-1";
                }

            }
            return "1";

        }

        public string ReadSalesIDataFromFolder(string control, string name, string path, string TempTable, int numberofcolumn, string[] arr, string active, string colsCD, out String tmpViewState, int columndr)
        {
            string fileName = name;
            string extension = ".txt";

            fileName = fileName.Replace(extension, "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + extension);
            string filePath = path;

            //read file txt
            DataTable dt = ConvertOrderSalesToDataTable(filePath, numberofcolumn, columndr);
            //Định dạng lại dữ liệu tiền tệ của thái lan (đem dấu trừ phía sau dữ liệu số ra phía trước)
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int[] num = { 6, 7, 10, 11, 13, 14 };// Các cột cần kiểm tra
                foreach (int n in num)
                {
                    string str = dt.Rows[i][n].ToString().Trim();
                    if (str.IndexOf('-') > 0)
                    {
                        dt.Rows[i][n] = '-' + str.Replace("-", string.Empty);
                    }
                }
            }

            if (dt == null)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File uploads don't exported from system.");
                tmpViewState = "";
                return "-1";
            }

            if (dt.Rows.Count == 0)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File is no data.");
                tmpViewState = "";
                return "-1";
            }

            tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();
            //add new column
            DataColumn objDataColumn = new DataColumn();
            objDataColumn.DefaultValue = tmpViewState;
            objDataColumn.ColumnName = colsCD;
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
                bulkcopy.DestinationTableName = TempTable;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string tp = dt.Columns[j].ToString();
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(tp, arr[j].ToString()));
                }
                if (active != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][active].ToString().Equals("1"))
                            dt.Rows[i][active] = Boolean.TrueString;
                        else if (dt.Rows[i][active].ToString().Equals("0"))
                            dt.Rows[i][active] = Boolean.FalseString;
                        else
                            dt.Rows[i][active] = null;
                    }
                }

                //if (Dist_cols != "")
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        dt.Rows[i][Int32.Parse(Dist_cols)] = dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Substring(dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Length - 6, 6);
                //    }
                //}
                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {

                    //ghi log vao table H_UPLOAD_FILE
                    P5sCmmFns.P5sWriteHistory(control, false, name, ex.ToString());
                    return "-1";
                }

            }
            return "1";

        }

        public string ReadExcelFromFolderPerformance(string control, string name, string path, string TempTable, int namelocation, string[] arr, string active, string colsCD, out String tmpViewState, string Dist_cols)
        {
            string fileName = name;
            string extension = ".txt";

            fileName = fileName.Replace(extension, "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + extension);
            string filePath = path;

            DataTable dt = ConvertToDataTable(filePath, namelocation, namelocation);
            if (dt == null)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File uploads don't exported from system.");
                tmpViewState = "";
                return "-1";
            }


            if (dt.Rows.Count == 0)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File is no data.");
                tmpViewState = "";
                return "-1";
            }

            tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();
            //add new column
            DataColumn objDataColumn = new DataColumn();
            objDataColumn.DefaultValue = tmpViewState;
            objDataColumn.ColumnName = colsCD;
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
                bulkcopy.DestinationTableName = TempTable;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string tp = dt.Columns[j].ToString();
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(tp, arr[j].ToString()));
                }
                if (Dist_cols != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][Int32.Parse(Dist_cols)] = dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Substring(dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Length - 6, 6);
                    }
                }
                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    //ghi log vao table H_UPLOAD_FILE
                    P5sCmmFns.P5sWriteHistory(control, false, name, ex.ToString());
                    return "-1";
                }
            }
            return "1";

        }

        public static DataTable readDataFromExcelFilePerformance(String path, String sheetName, int firstRow)
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
                        ws.Cells[rowNum, 15].Style.Numberformat.Format = "0.00";
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

        public string ReadTXTFromFolderCustomerCPTH(string control, string name, string path, string TempTable, int numberofcolumn, string[] arr, string active, string colsCD, out String tmpViewState, int columndr, string Dist_cols)
        {
            string fileName = name;
            string extension = ".txt";

            fileName = fileName.Replace(extension, "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + extension);
            string filePath = path;

            //read file txt
            DataTable dt = ConvertToDataTable(filePath, numberofcolumn, columndr);
            if (dt == null)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File uploads don't exported from system.");
                tmpViewState = "";
                return "-1";
            }

            if (dt.Rows.Count == 0)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File is no data.");
                tmpViewState = "";
                return "-1";
            }

            tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();
            //add new column
            DataColumn objDataColumn = new DataColumn();
            objDataColumn.DefaultValue = tmpViewState;
            objDataColumn.ColumnName = colsCD;
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
                bulkcopy.DestinationTableName = TempTable;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string tp = dt.Columns[j].ToString();
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(tp, arr[j].ToString()));
                }
                if (active != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][active].ToString().Equals("1"))
                            dt.Rows[i][active] = Boolean.TrueString;
                        else if (dt.Rows[i][active].ToString().Equals("0"))
                            dt.Rows[i][active] = Boolean.FalseString;
                        else
                            dt.Rows[i][active] = null;
                    }
                }
                if (Dist_cols != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string latutide = dt.Rows[i][19].ToString().Trim();
                        string longtide = dt.Rows[i][20].ToString().Trim();
                        dt.Rows[i][19] = latutide + ',' + longtide;
                        // dt.Rows[i][Int32.Parse(Dist_cols)] = dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Substring(dt.Rows[i][Int32.Parse(Dist_cols)].ToString().Length - 6, 6);
                    }
                }
                try
                {
                    bulkcopy.WriteToServer(dt);

                }
                catch (Exception ex)
                {

                    //ghi log vao table H_UPLOAD_FILE
                    P5sCmmFns.P5sWriteHistory(control, false, name, ex.ToString());
                    return "-1";
                }

            }
            return "1";

        }
        public string ReadDataFromTextToDB(string control, string name, string path, string TempTable, int numberofcolumn, string[] arr, string active, string colsCD, out String tmpViewState, int columndr)
        {
            string fileName = name;
            string extension = ".txt";

            fileName = fileName.Replace(extension, "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + extension);
            string filePath = path;

            //read file txt
            DataTable dt = ConvertToDataTable(filePath, numberofcolumn, columndr);
                  
            if (dt == null)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File uploads don't exported from system.");
                tmpViewState = "";
                return "-1";
            }


            if (dt.Rows.Count == 0)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File is no data.");
                tmpViewState = "";
                return "-1";
            }

            tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();
            //add new column
            DataColumn objDataColumn = new DataColumn();
            objDataColumn.DefaultValue = tmpViewState;
            objDataColumn.ColumnName = colsCD;
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
                bulkcopy.DestinationTableName = TempTable;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string tp = dt.Columns[j].ToString();
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(tp, arr[j].ToString()));
                }
                if (active != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][active].ToString().Equals("1"))
                            dt.Rows[i][active] = Boolean.TrueString;
                        else if (dt.Rows[i][active].ToString().Equals("0"))
                            dt.Rows[i][active] = Boolean.FalseString;
                        else
                            dt.Rows[i][active] = null;
                    }
                }
                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {

                    //ghi log vao table H_UPLOAD_FILE
                    P5sCmmFns.P5sWriteHistory(control, false, name, ex.ToString());
                    return "-1";
                }

            }
            return "1";
        }

        public DataTable ConvertPrimaryDataToDataTable_TH(string filePath, int numberOfColumns, int columndr)
        {
            DataTable tbl = new DataTable();

            for (int col = 0; col < numberOfColumns; col++)
                tbl.Columns.Add(new DataColumn("Column" + (col + 1).ToString()));

            if (!File.Exists(filePath))
            {
                return tbl;
            }

            string[] lines = System.IO.File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                //cắt chuổi line thành từng cột dựa theo ký tự '|', chỉ riêng ở file này
                string[] cols = line.Split('|');
                DataRow dr = tbl.NewRow();
                for (int cIndex = 0; cIndex < columndr; cIndex++)
                {
                    try
                    {
                        dr[cIndex] = cols[cIndex].Trim();
                    }
                    catch (Exception ex)
                    {
                        P5sCmmFns.P5sWriteHistory("Autoupload", false, "", ex.Message);
                        return null;
                    }
                }
                tbl.Rows.Add(dr);
            }
            //xóa header , chỉ dành cho file có header
            //tbl.Rows.Remove(tbl.Rows[0]);
            return tbl;
        }

        public string ReadDataPrimaryFromFolder_TH(string control, string name, string path, string TempTable, int numberofcolumn, string[] arr, string active, string colsCD, out String tmpViewState, int columndr)
        {
            string fileName = name;
            string extension = ".txt";

            fileName = fileName.Replace(extension, "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssffffff") + extension);
            string filePath = path;

            //read file txt
            DataTable dt = ConvertPrimaryDataToDataTable_TH(filePath, numberofcolumn, columndr);
            //Định dạng lại dữ liệu tiền tệ của thái lan (đem dấu trừ phía sau dữ liệu số ra phía trước)
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int[] num = { 6, 7, 10, 11, 13, 14 };// Các cột cần kiểm tra
                foreach (int n in num)
                {
                    string str = dt.Rows[i][n].ToString().Trim();
                    if (str.IndexOf('-') > 0)
                    {
                        dt.Rows[i][n] = '-' + str.Replace("-", string.Empty);
                    }
                }
            }

            if (dt == null)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File uploads don't exported from system.");
                tmpViewState = "";
                return "-1";
            }

            if (dt.Rows.Count == 0)
            {
                P5sCmmFns.P5sWriteHistory(control, false, name, "File is no data.");
                tmpViewState = "";
                return "-1";
            }

            tmpViewState = P5sCmm.P5sCmmFns.P5sCreateViewStateTemp();
            //add new column
            DataColumn objDataColumn = new DataColumn();
            objDataColumn.DefaultValue = tmpViewState;
            objDataColumn.ColumnName = colsCD;
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
                bulkcopy.DestinationTableName = TempTable;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string tp = dt.Columns[j].ToString();
                    bulkcopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(tp, arr[j].ToString()));
                }

                if (active != "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][active].ToString().Equals("1"))
                            dt.Rows[i][active] = Boolean.TrueString;
                        else if (dt.Rows[i][active].ToString().Equals("0"))
                            dt.Rows[i][active] = Boolean.FalseString;
                        else
                            dt.Rows[i][active] = null;
                    }
                }
                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    //ghi log vao table H_UPLOAD_FILE
                    P5sCmmFns.P5sWriteHistory(control, false, name, ex.ToString());
                    return "-1";
                }
            }
            return "1";
        }
    }
}