using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.fsmdls.GoogleSheet
{
    public partial class CreateAuthGoogle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            string key = txtFolderKey.Text;
            string sql;
            if (key.Trim() != "")
            {
                sql = @" DECLARE @KEY nvarchar(50)
                        SET @KEY = @0

                        IF EXISTS(select VALUE from S_PARAMS where NAME = 'DRIVE_FOLDER_KEY')
                        BEGIN
                            UPDATE S_PARAMS SET VALUE = @KEY where NAME = 'DRIVE_FOLDER_KEY'
                        END
                        ELSE
                        BEGIN
                            INSERT INTO S_PARAMS(NAME, VALUE, PARAM_DESCRIPTION)

                            VALUES('DRIVE_FOLDER_KEY', @KEY, N'Folder key trong google drive để upload file cho google sheet')
                        END
                        SELECT VALUE from S_PARAMS where NAME='DRIVE_FOLDER_KEY'";
            }
            else
            {
                sql= @"SELECT VALUE from S_PARAMS where NAME='DRIVE_FOLDER_KEY'";
            }
            DataTable dt = L5sDmComm.L5sSql.Query(sql, key);
            string primaryKey = dt.Rows[0]["VALUE"].ToString();
            string id= GoogleDriveAPI.UploadFileOnDrive(fileUpload.PostedFile,primaryKey);
            //System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/" + id);
            lbtnRunPage.HRef= "https://docs.google.com/spreadsheets/d/" + id;
            lbtnRunPage.InnerHtml= "https://docs.google.com/spreadsheets/d/" + id;
        }
    }
}