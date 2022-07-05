using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using L5sDmComm;
using P5sCmm;
using P5sDmComm;
using System.Text;
using System.IO;
using System.Data;

namespace MMV.fsmdls.TransferPage
{
    public partial class TransferPage : System.Web.UI.Page
    {
        string url_mess = "";
   
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            //A Đức xét setting lại theo session USER_NAME giúp em
            string Session_USER_NAME = "";
            //star
            String pagename = "TranferPassCode.aspx";
            string sqllog = @"INSERT INTO S_LOG_TRANFERPASSCODE(PAGE_NAME,USER_CD,DESCRIPTION) VALUES(@0,@1,@2)";
            string sql_URL = @"SELECT HH_PARAM_VALUE FROM S_HH_PARAM where HH_PARAM_KEY='UrlMessenger'";
            L5sSql.Execute(sqllog, pagename, Session_USER_NAME, @"Start");
            try
            {
                //lây URL 
                //Get url
                DataTable dturlmess = L5sSql.Query(sql_URL);
                L5sSql.Execute(sqllog, pagename, Session_USER_NAME, @"Get url");
                if (dturlmess.Rows.Count > 0)
                {
                    //gan url
                    url_mess = dturlmess.Rows[0]["HH_PARAM_VALUE"].ToString();
                    //L5sSql.Execute(sqllog, pagename, Session_USER_NAME, @"Gan url");
                    //random 10 số.
                    // Random rnd = new Random();
                    //Get passcode
                    String Passcode_random = DateTime.Now.ToString("hhmmssffffff");
                    L5sSql.Execute(sqllog, pagename, Session_USER_NAME, @"Get Random Passcode");
                    //insert passcode
                    string sqlrandom = string.Format(@"insert into O_ESN_TRANFERS_PASSCODE(USER_CODE,PASS_CODE) values ('{0}','{1}')", Session_USER_NAME, Passcode_random);
                    long a = L5sSql.Execute(sqlrandom);
                    L5sSql.Execute(sqllog, pagename, Session_USER_NAME, @"Inserted passcode");
                    //tạo pass code thành công, chuyen trang
                    if (a > 0)
                    {
                        //Page.ClientScript.RegisterStartupScript(
                        //        this.GetType(), "OpenWindow", "window.open('"+ url_mess+ "','_newtab');", true);
                        //chuyen trang
                        L5sSql.Execute(sqllog, pagename, Session_USER_NAME, "Tranfer page successfully");
                        Response.Redirect(url_mess + "?USER_CODE=" + Passcode_random, false);

                        //return ;
                        //Response.Redirect(url_mess + "?USER_CODE=" + aaa,false);

                    }
                    else
                    {
                        //faile execute sql
                        L5sSql.Execute(sqllog, pagename, Session_USER_NAME, @"Fail execute sql");
                        L5sMsg.Show("Page transfer failed!");
                    }
                }
                else
                {
                    //fail get url
                    L5sSql.Execute(sqllog, pagename, Session_USER_NAME, @"Fail get url");
                    L5sMsg.Show("Transfer page fail!");

                }
            }
            catch (Exception ex)
            {
                //fail try catch
                L5sSql.Execute(sqllog, pagename, Session_USER_NAME, @"Fail try catch");
                L5sMsg.Show("ERROR: " + ex.Message);
            }

        }

        public string MaNgauNhien_So(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                if (temp != -1 && temp == t)
                {
                    return MaNgauNhien_So(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

    }
}