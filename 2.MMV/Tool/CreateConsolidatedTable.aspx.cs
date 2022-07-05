using L5sDmComm;
using P5sCmm;
using P5sDmComm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Windows.Forms;

namespace MMV.Tool
{
    public partial class CreateConsolidatedTable : System.Web.UI.Page
    {
        private int weeksForYear = 53;
        public L5sAutocomplete P5sActTableName;
        String[] P5sControlNameInitAutoComplete = new String[] { "P5sConfirm", "P5sDdlDistributorType", "P5sEdit", "P5sLbtnNew", "P5sLbtnUpdate", "P5sLbtnInsert", "P5sDdlREGION_CD", "P5sDdlAREA_CD" };
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            if (!IsPostBack)
            {
                P5sTxtYear.Text = (DateTime.Now.Year + 1).ToString();
                P5sTxtDatabaseName.Text = "MMV_CONSOLIDATE_SALES";

            }
            P5sCheckValid();
            P5sInit();
        }
        private void P5sInit()
        {
            if (Array.IndexOf(P5sControlNameInitAutoComplete, P5sCmm.P5sCmmFns.P5sGetPostBackControlId(this)) != -1)
            {

            }

            this.P5sAutoCompleteInit();
        }
        private void P5sAutoCompleteInit()
        {
            DataTable dt = getTableNameToCreate();
            this.P5sActTableName = this.P5sActTableName == null ? new L5sAutocomplete(dt, this.P5sTxtTableName.ClientID, 0, true) : this.P5sActTableName;
        }

        private DataTable getTableNameToCreate()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CD");
            dt.Columns.Add("Name");
            dt.Columns.Add("Parent");
            dt.Columns.Add("Active");

            //----------------------------------
            DataRow dr = dt.NewRow();
            dr["CD"] = "DE_TRACKING_OF_SUPERVISOR";
            dr["Name"] = "DE_TRACKING_OF_SUPERVISOR";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "DE_TRACKING_OF_ASM";
            dr["Name"] = "DE_TRACKING_OF_ASM";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "DE_TRACKING_OF_SALE";
            dr["Name"] = "DE_TRACKING_OF_SALE";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "DE_TRACKING_STOP_OF_SUPERVISOR";
            dr["Name"] = "DE_TRACKING_STOP_OF_SUPERVISOR";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "DE_TRACKING_STOP_OF_ASM";
            dr["Name"] = "DE_TRACKING_STOP_OF_ASM";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "DE_TRACKING_STOP_OF_SALE";
            dr["Name"] = "DE_TRACKING_STOP_OF_SALE";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "O_TRACKING_SUPERVISOR_RAW_DATA";
            dr["Name"] = "O_TRACKING_SUPERVISOR_RAW_DATA";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "O_TRACKING_ASM_RAW_DATA";
            dr["Name"] = "O_TRACKING_ASM_RAW_DATA";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "O_TIME_IN_OUT";
            dr["Name"] = "O_TIME_IN_OUT";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);

            //----------------------------------
            dr = dt.NewRow();
            dr["CD"] = "O_TRACKING";
            dr["Name"] = "O_TRACKING";
            dr["Parent"] = "";
            dr["Active"] = "TRUE";
            dt.Rows.Add(dr);
            return dt;
        }

        protected void P5sLbtnCreate_Click(object sender, EventArgs e)
        {
            if (!CheckExistsDatabase(P5sTxtDatabaseName.Text))
            {
                L5sMsg.Show("Database isn't exists. Please change the name and try again!");
                return;
            }
            string databaseName = P5sTxtDatabaseName.Text;
            string year = P5sTxtYear.Text;
            string log = string.Empty, message = string.Empty;
            //DE_TRACKING_OF_SUPERVISOR
            if (P5sTxtTableName.Text.IndexOf("DE_TRACKING_OF_SUPERVISOR") >= 0)
            {
                if (CreateTableTrackingOfPosition(databaseName, "DE_TRACKING_OF_SUPERVISOR", year, weeksForYear, out log))
                {
                    message += "--------- Create table DE_TRACKING_OF_SUPERVISOR ---------" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //DE_TRACKING_OF_ASM
            if (P5sTxtTableName.Text.IndexOf("DE_TRACKING_OF_ASM") >= 0)
            {
                if (CreateTableTrackingOfPosition(databaseName, "DE_TRACKING_OF_ASM", year, weeksForYear, out log))
                {
                    message += "------------ Create table DE_TRACKING_OF_ASM ------------" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //DE_TRACKING_OF_SALES
            if (P5sTxtTableName.Text.IndexOf("DE_TRACKING_OF_SALES") >= 0)
            {
                if (CreateTableTrackingOfPosition(databaseName, "DE_TRACKING_OF_SALES", year, weeksForYear, out log))
                {
                    message += "---------- Create table DE_TRACKING_OF_SALES ----------" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //DE_TRACKING_STOP_OF_SUPERVISOR
            if (P5sTxtTableName.Text.IndexOf("DE_TRACKING_STOP_OF_SUPERVISOR") >= 0)
            {
                if (CreateTableTrackingStopOfPosition(databaseName, "DE_TRACKING_STOP_OF_SUPERVISOR", year, weeksForYear, out log))
                {
                    message += "---- Create table DE_TRACKING_STOP_OF_SUPERVISOR ----" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //DE_TRACKING_STOP_OF_ASM
            if (P5sTxtTableName.Text.IndexOf("DE_TRACKING_STOP_OF_ASM") >= 0)
            {
                if (CreateTableTrackingStopOfPosition(databaseName, "DE_TRACKING_STOP_OF_ASM", year, weeksForYear, out log))
                {
                    message += "------- Create table DE_TRACKING_STOP_OF_ASM -------" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //DE_TRACKING_STOP_OF_SALES
            if (P5sTxtTableName.Text.IndexOf("DE_TRACKING_STOP_OF_SALES") >= 0)
            {
                if (CreateTableTrackingStopOfPosition(databaseName, "DE_TRACKING_STOP_OF_SALES", year, weeksForYear, out log))
                {
                    message += "----- Create table DE_TRACKING_STOP_OF_SALES ------" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //O_TRACKING_SUPERVISOR_RAW_DATA
            if (P5sTxtTableName.Text.IndexOf("O_TRACKING_SUPERVISOR_RAW_DATA") >= 0)
            {
                if (CreateTableTrackingRawDataPosition(databaseName, "O_TRACKING_SUPERVISOR_RAW_DATA", year, weeksForYear, out log))
                {
                    message += "---- Create table O_TRACKING_SUPERVISOR_RAW_DATA ----" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //O_TRACKING_ASM_RAW_DATA
            if (P5sTxtTableName.Text.IndexOf("O_TRACKING_ASM_RAW_DATA") >= 0)
            {
                if (CreateTableTrackingRawDataPosition(databaseName, "O_TRACKING_ASM_RAW_DATA", year, weeksForYear, out log))
                {
                    message += "------- Create table O_TRACKING_ASM_RAW_DATA -------" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //O_TIME_IN_OUT
            if (P5sTxtTableName.Text.IndexOf("O_TIME_IN_OUT") >= 0)
            {
                if (CreateTableTITO(databaseName, year, weeksForYear, out log))
                {
                    message += "----------- Create table O_TIME_IN_OUT -----------" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //O_TRACKING
            if (P5sTxtTableName.Text.IndexOf("O_TRACKING") >= 0)
            {
                if (CreateTableTracking(databaseName, year, weeksForYear, out log))
                {
                    message += "------------ Create table O_TRACKING ------------" + Environment.NewLine + " Success!";
                }
                else
                {
                    message += log;
                }
                message += Environment.NewLine;
            }

            //Insert data to M_TABLE
            if (this.ckbInsertDataToMTABLE.Checked)
            {
                insertDataIntoMTable(databaseName, year, weeksForYear, out log);
                message += log;
            }
            if (message.Trim() == "")
                message = "Please chose table to create!";
            P5sTxtLog.Text = message;
        }
        //private void WriteLog(string log)
        //{
        //    string oldtext = divShowLog.InnerText;
        //    oldtext += log;
        //    divShowLog.InnerText = oldtext;
        //}
        private void P5sCheckValid()
        {
            if (CheckExistsDatabase(P5sTxtDatabaseName.Text))
            {
                P5sLblCheckExistsDatabase.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "green");
                P5sLblCheckExistsDatabase.Text = "Database exists!";
            }
            else
            {
                P5sLblCheckExistsDatabase.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "red");
                P5sLblCheckExistsDatabase.Text = "Database isn't exists!";
            }
        }
        private bool CheckExistsDatabase(string dbName)
        {
            String sql = String.Format(@"
                                    IF DB_ID('{0}') IS NOT NULL
                                        SELECT 1 AS [STATUS]
                                    ELSE
                                        SELECT 0 AS [STATUS]
                        ", dbName);
            DataTable dt = new DataTable();
            try
            {
                L5sInitial.LoadForLoginPage();
                dt = P5sSql.Query(sql);
                string status = dt.Rows[0]["STATUS"].ToString();
                if (status == "0")
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        private bool checkExistsTable(string dbName, string tableName)
        {
            string sql = string.Format("SELECT CASE WHEN OBJECT_ID('[{0}].dbo.[{1}]', 'U') IS NOT NULL THEN 1 ELSE 0 END AS STATUS", dbName, tableName);
            DataTable table = L5sSql.Query(sql);
            return table.Rows[0]["STATUS"].ToString() == "1" ? true : false;
        }
        private void insertDataIntoMTable(string dbName, string year, int weeks, out string log)
        {
            if(!checkExistsTable(dbName,"M_TABLE"))
            {
               log = Environment.NewLine + "---------- Insert data into[M_TABLE] ----------" + Environment.NewLine + "=> Table M_TABLE not exists in database " + dbName;
                return;
            }
            log = Environment.NewLine + "---------- Insert data into[M_TABLE] ----------" + Environment.NewLine+"Key value exists in [M_TABLE]";
            bool flag = true;
            string tableName1, tableName2, tableName3, sql;
            for (int i = 1; i <= weeks; i++)
            {
                //Type CD=1
                tableName1 = "O_TRACKING_" + year + "_" + i.ToString("00");
                tableName2 = "DE_TRACKING_OF_SALES_" + year + "_" + i.ToString("00");
                tableName3 = "DE_TRACKING_STOP_OF_SALES_" + year + "_" + i.ToString("00");
                if (checkPrimaryKey(dbName, "M_TABLE", "TABLE_NAME_1", tableName1))
                {
                    sql = string.Format("INSERT INTO [{0}].dbo.M_TABLE ([TABLE_NAME_1],[TABLE_NAME_2],[TABLE_NAME_3],[WEEK],[YEAR],[TYPE_CD]) VALUES ('{1}','{2}','{3}',{4},{5},1)", dbName, tableName1, tableName2, tableName3, i, year);
                    L5sSql.Execute(sql);
                }
                else
                {
                    flag = false;
                    log += Environment.NewLine + " -> " + tableName1;
                }

                //Type CD=2
                tableName1 = "O_TRACKING_SUPERVISOR_RAW_DATA_" + year + "_" + i.ToString("00");
                tableName2 = "DE_TRACKING_OF_SUPERVISOR_" + year + "_" + i.ToString("00");
                tableName3 = "DE_TRACKING_STOP_OF_SUPERVISOR_" + year + "_" + i.ToString("00");
                if (checkPrimaryKey(dbName, "M_TABLE", "TABLE_NAME_1", tableName1))
                {
                    sql = string.Format("INSERT INTO [{0}].dbo.M_TABLE ([TABLE_NAME_1],[TABLE_NAME_2],[TABLE_NAME_3],[WEEK],[YEAR],[TYPE_CD]) VALUES ('{1}','{2}','{3}',{4},{5},2)", dbName, tableName1, tableName2, tableName3, i, year);
                    L5sSql.Execute(sql);
                }
                else
                {
                    flag = false;
                    log += Environment.NewLine + " -> " + tableName1 ;
                }



                //Type CD=3
                tableName1 = "O_TIME_IN_OUT_" + year + "_" + i.ToString("00");
                tableName2 = "";
                tableName3 = "";
                if (checkPrimaryKey(dbName, "M_TABLE", "TABLE_NAME_1", tableName1))
                {
                    sql = string.Format("INSERT INTO [{0}].dbo.M_TABLE ([TABLE_NAME_1],[TABLE_NAME_2],[TABLE_NAME_3],[WEEK],[YEAR],[TYPE_CD]) VALUES ('{1}','{2}','{3}',{4},{5},3)", dbName, tableName1, tableName2, tableName3, i, year);
                    L5sSql.Execute(sql);
                }
                else
                {
                    log += Environment.NewLine + " -> " + tableName1 ;
                }


                //Type CD=4
                tableName1 = "O_TRACKING_ASM_RAW_DATA_" + year + "_" + i.ToString("00");
                tableName2 = "DE_TRACKING_OF_ASM_" + year + "_" + i.ToString("00");
                tableName3 = "DE_TRACKING_STOP_OF_ASM_" + year + "_" + i.ToString("00");
                if (checkPrimaryKey(dbName, "M_TABLE", "TABLE_NAME_1", tableName1))
                {
                    sql = string.Format("INSERT INTO [{0}].dbo.M_TABLE ([TABLE_NAME_1],[TABLE_NAME_2],[TABLE_NAME_3],[WEEK],[YEAR],[TYPE_CD]) VALUES ('{1}','{2}','{3}',{4},{5},4)", dbName, tableName1, tableName2, tableName3, i, year);
                    L5sSql.Execute(sql);
                }
                else
                {
                    flag = false;
                    log += Environment.NewLine + " -> " + tableName1;
                }
            }
            if (flag)
            {
                log += Environment.NewLine + "=> Success!";
            }
        }
        private bool checkPrimaryKey(string dbName, string tableName, string keyName, string value)//return true if key value is not exists
        {
            string sql = string.Format(@"SELECT * FROM [{0}].dbo.[{1}] WHERE [{2}]='{3}'", dbName, tableName, keyName, value);
            DataTable dt = L5sSql.Query(sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return true;
            }
            return false;
        }

        #region function create table

        //Time in Time Out
        private bool CreateTableTITO(string dbName, string year, int weeks, out string log)
        {
            log = Environment.NewLine + "#Create table Time In Time Out" + Environment.NewLine + " Table is exists: ";
            bool Flag = true;
            for (int i = 1; i <= weeks; i++)
            {
                string tableName = "O_TIME_IN_OUT" + '_' + year + '_' + i.ToString("00");
                if (checkExistsTable(dbName, tableName))
                {
                    log += Environment.NewLine + " -> " + tableName;
                    Flag = false;
                }
                else
                {
                    string sql = string.Format(@" 
                            CREATE TABLE [{0}].[dbo].[{1}](
	                            [ROUTE_CD] [bigint] NULL,
	                            [SALES_CD] [bigint] NULL,
	                            [DISTRIBUTOR_CD] [bigint] NULL,
	                            [CUSTOMER_CD] [bigint] NULL,
	                            [CUSTOMER_CODE] [nvarchar](50) NULL,
	                            [TIME_IN_LATITUDE_LONGITUDE] [nvarchar](50) NULL,
	                            [TIME_IN_LATITUDE_LONGITUDE_ACCURACY] [float] NULL,
	                            [TIME_OUT_LATITUDE_LONGITUDE] [nvarchar](50) NULL,
	                            [TIME_OUT_LATITUDE_LONGITUDE_ACCURACY] [float] NULL,
	                            [TIME_IN_CREATED_DATE] [datetime] NULL,
	                            [TIME_OUT_CREATED_DATE] [datetime] NULL,
	                            [CREATED_DATE] [datetime] NULL,
	                            [TYPE_CD] [bigint] NULL,
	                            [TIME_IN_LOCATION_ADDRESS] [nvarchar](512) NULL,
	                            [TIME_OUT_LOCATION_ADDRESS] [nvarchar](512) NULL,
	                            [MAX_DATETIME_TRACKING] [nvarchar](50) NULL,
	                            [LOCATION_IS_NULL] [bit] NULL,
	                            [TIME_IN_OUT_CD] [bigint] IDENTITY(1,1) NOT NULL
                            ) ON [PRIMARY]", dbName, tableName);
                    L5sSql.Execute(sql);
                }
            }
            return Flag;
        }

        //O_TRACKING
        private bool CreateTableTracking(string dbName, string year, int weeks, out string log)
        {
            log = Environment.NewLine + "------------ Create table Tracking ------------" + Environment.NewLine + " Table is exists:";
            bool Flag = true;
            for (int i = 1; i <= weeks; i++)
            {
                string tableName = "O_TRACKING" + '_' + year + '_' + i.ToString("00");
                if (checkExistsTable(dbName, tableName))
                {
                    log += Environment.NewLine + " -> " + tableName;
                    Flag = false;
                }
                else
                {
                    string sql = string.Format(@" 
                            CREATE TABLE [{0}].[dbo].[{1}](
	                            [TRACKING_CD] [bigint] IDENTITY(1,1) NOT NULL,
	                            [SALES_CD] [bigint] NULL,
	                            [DISTRIBUTOR_CD] [bigint] NULL,
	                            [YYMMDD] [int] NULL,
	                            [LONGITUDE_LATITUDE] [nvarchar](255) NULL,
	                            [TRACKING_DATETIME] [datetime] NULL,
	                            [BATTERY_PERCENTAGE] [float] NULL,
	                            [DEVICE_STATUS] [nvarchar](max) NULL,
	                            [NO_REPEAT] [int] NULL DEFAULT ((1)),
	                            [NO_ORDER] [int] NULL DEFAULT ((0)),
	                            [CREATED_DATE] [datetime] NULL DEFAULT (getdate()),
	                            [BEGIN_DATETIME] [datetime] NULL,
	                            [END_DATETIME] [datetime] NULL,
	                            [TRACKING_ACCURACY] [float] NULL DEFAULT ((0)),
	                            [TRACKING_PROVIDER] [nvarchar](50) NULL,
	                            [DURATION] [nvarchar](50) NULL,
	                            [ACTIVE] [bit] NULL DEFAULT ((1)),
	                            [BATTERY_PERCENTAGE_START] [float] NULL DEFAULT ((0)),
	                            [BATTERY_PERCENTAGE_END] [float] NULL DEFAULT ((0)),
	                            [TYPE_CD] [bigint] NULL DEFAULT ((1)),
	                            [IS_SYNC] [bit] NULL DEFAULT ((1)),
	                            [LOCATION_ADDRESS] [nvarchar](512) NULL DEFAULT ('')
                            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]", dbName, tableName);
                    L5sSql.Execute(sql);
                }

            }
            return Flag;
        }

        //DE_TRACKING_OF_ASM/SALES/SUPERVISOR
        private bool CreateTableTrackingOfPosition(string dbName, string title, string year, int weeks, out string log)
        {
            log = Environment.NewLine + "--------- Create table " + title + " ---------" + Environment.NewLine + " Table is exists:";
            bool Flag = true;
            for (int i = 1; i <= weeks; i++)
            {
                string tableName = title + '_' + year + '_' + i.ToString("00");
                if (checkExistsTable(dbName, tableName))
                {
                    log += Environment.NewLine + "-> " + tableName;
                    Flag = false;
                }
                else
                {
                    string sql = string.Format(@" 
                            CREATE TABLE [{0}].[dbo].[{1}](
	                            [YYMMDD] [int] NULL,
	                            [TRACKING_CD] [int] IDENTITY(1,1) NOT NULL,
	                            [ASM_CD] [bigint] NULL,
	                            [DISTRIBUTOR_CD] [bigint] NULL,
	                            [LONGITUDE_LATITUDE] [nvarchar](255) NULL,
	                            [DEVICE_STATUS] [nvarchar](max) NULL,
	                            [NO_REPEAT] [int] NULL,
	                            [BEGIN_DATETIME] [datetime] NULL,
	                            [END_DATETIME] [datetime] NULL,
	                            [BATTERY_PERCENTAGE] [int] NULL,
	                            [TYPE_TRACKING] [nvarchar](50) NULL,
	                            [POINT_RADIUS] [nvarchar](50) NULL,
	                            [ANGEL] [float] NULL,
	                            [TRACKING_ACCURACY] [float] NULL,
	                            [BATTERY_PERCENTAGE_VALUE] [nvarchar](max) NULL,
	                            [BATTERY_PERCENTAGE_DATETIME] [nvarchar](max) NULL,
	                            [TRACKING_PROVIDER] [nvarchar](max) NULL,
	                            [TRACKING_PROVIDER_VALUE] [nvarchar](max) NULL,
	                            [TRACKING_PROVIDER_DATETIME] [nvarchar](max) NULL,
	                            [CREATED_DATE] [datetime] NULL,
	                            [SYS_CREATED_DATE] [datetime] NULL,
	                            [LOCATION_ADDRESS] [nvarchar](512) NULL
                            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]", dbName, tableName);
                    L5sSql.Execute(sql);
                }

            }
            return Flag;
        }

        //DE_TRACKING_STOP_OF_ASM/SALES/SUPERVISOR
        private bool CreateTableTrackingStopOfPosition(string dbName, string title, string year, int weeks, out string log)
        {
            log = Environment.NewLine + "#Create table " + title + Environment.NewLine + " Table is exists:";
            bool Flag = true;
            for (int i = 1; i <= weeks; i++)
            {
                string tableName = title + '_' + year + '_' + i.ToString("00");
                if (checkExistsTable(dbName, tableName))
                {
                    log += Environment.NewLine + " -> " + tableName;
                    Flag = false;
                }
                else
                {
                    string sql = string.Format(@" 
                            CREATE TABLE [{0}].[dbo].[{1}](
	                            [YYMMDD] [int] NULL,
	                            [TRACKING_CD] [int] IDENTITY(1,1) NOT NULL,
	                            [ASM_CD] [bigint] NULL,
	                            [DISTRIBUTOR_CD] [bigint] NULL,
	                            [LONGITUDE_LATITUDE] [nvarchar](255) NULL,
	                            [DEVICE_STATUS] [nvarchar](max) NULL,
	                            [NO_REPEAT] [int] NULL,
	                            [BEGIN_DATETIME] [datetime] NULL,
	                            [END_DATETIME] [datetime] NULL,
	                            [BATTERY_PERCENTAGE] [int] NULL,
	                            [TYPE_TRACKING] [nvarchar](50) NULL,
	                            [POINT_RADIUS] [nvarchar](50) NULL,
	                            [ANGEL] [float] NULL,
	                            [TRACKING_ACCURACY] [float] NULL,
	                            [BATTERY_PERCENTAGE_VALUE] [nvarchar](max) NULL,
	                            [BATTERY_PERCENTAGE_DATETIME] [nvarchar](max) NULL,
	                            [TRACKING_PROVIDER] [nvarchar](max) NULL,
	                            [TRACKING_PROVIDER_VALUE] [nvarchar](max) NULL,
	                            [TRACKING_PROVIDER_DATETIME] [nvarchar](max) NULL,
	                            [CREATED_DATE] [datetime] NULL,
	                            [SYS_CREATED_DATE] [datetime] NULL,
	                            [LOCATION_ADDRESS] [nvarchar](512) NULL
                            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((0)) FOR [TRACKING_ACCURACY]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT (getdate()) FOR [SYS_CREATED_DATE]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ('') FOR [LOCATION_ADDRESS]", dbName, tableName);
                    L5sSql.Execute(sql);
                }

            }
            return Flag;
        }

        //DE_TRACKING_STOP_OF_ASM/SALES/SUPERVISOR
        private bool CreateTableTrackingRawDataPosition(string dbName, string title, string year, int weeks, out string log)
        {
            log = Environment.NewLine + "---- Create table " + title + " ----" + Environment.NewLine + " Table is exists:";
            bool Flag = true;
            for (int i = 1; i <= weeks; i++)
            {
                string tableName = title + '_' + year + '_' + i.ToString("00");
                if (checkExistsTable(dbName, tableName))
                {
                    log += Environment.NewLine + " -> " + tableName;
                    Flag = false;
                }
                else
                {
                    string sql = string.Format(@" 
                            CREATE TABLE [{0}].[dbo].[{1}](
	                            [TRACKING_CD] [bigint] IDENTITY(1,1) NOT NULL,
	                            [SUPERVISOR_CD] [bigint] NULL,
	                            [DISTRIBUTOR_CD] [bigint] NULL,
	                            [YYMMDD] [int] NULL,
	                            [LONGITUDE_LATITUDE] [nvarchar](255) NULL,
	                            [TRACKING_DATETIME] [datetime] NULL,
	                            [BATTERY_PERCENTAGE] [float] NULL,
	                            [DEVICE_STATUS] [nvarchar](max) NULL,
	                            [NO_REPEAT] [int] NULL,
	                            [NO_ORDER] [int] NULL,
	                            [CREATED_DATE] [datetime] NULL,
	                            [BEGIN_DATETIME] [datetime] NULL,
	                            [END_DATETIME] [datetime] NULL,
	                            [TRACKING_ACCURACY] [float] NULL,
	                            [TRACKING_PROVIDER] [nvarchar](50) NULL,
	                            [DURATION] [nvarchar](50) NULL,
	                            [ACTIVE] [bit] NULL,
	                            [BATTERY_PERCENTAGE_START] [float] NULL,
	                            [BATTERY_PERCENTAGE_END] [float] NULL,
	                            [TYPE_CD] [bigint] NULL,
	                            [IS_SYNC] [bit] NULL,
	                            [LOCATION_ADDRESS] [nvarchar](512) NULL
                            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((1)) FOR [NO_REPEAT]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((0)) FOR [NO_ORDER]                            
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]                           
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((0)) FOR [TRACKING_ACCURACY]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((1)) FOR [ACTIVE]                           
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((0)) FOR [BATTERY_PERCENTAGE_START]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((0)) FOR [BATTERY_PERCENTAGE_END]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((1)) FOR [TYPE_CD]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ((1)) FOR [IS_SYNC]
                            ALTER TABLE [{0}].[dbo].[{1}] ADD  DEFAULT ('') FOR [LOCATION_ADDRESS]
                            ", dbName, tableName);
                    L5sSql.Execute(sql);
                }

            }
            return Flag;
        }


        #endregion 
    }
}