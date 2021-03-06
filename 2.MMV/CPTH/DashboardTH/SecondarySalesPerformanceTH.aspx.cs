using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.CPTH.DashboardTH
{
    public partial class SecondarySalesPerformanceTH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            P5sLbtnRemoveSession.Text = L5sMaster.L5sLangs["Clear Filter"];
            if (!IsPostBack)
            {
                Session["Where"] = "(1=1)";
                LoadSelectArea();
                L5sJS.L5sRun(@"google.charts.setOnLoadCallback(drawRegionChart);
                            google.charts.setOnLoadCallback(drawProvinceChart);
                            google.charts.setOnLoadCallback(drawDistributorChartV2);
                            google.charts.setOnLoadCallback(drawCDSChart);
                            google.charts.setOnLoadCallback(drawDSRChartV2);
                            setLabelTimeGone(); 
                            setLabelECC(); 
                            setLabelVisit();
                            setLabelPercent();
                            setLabelMTD(); 
                            setLabelTarget();");
            }
            
                Session["Where"] = "(1=1)";
                Session.Remove("REGION");
                Session.Remove("PROVINCE");
                Session.Remove("AREA");
                Session.Remove("DIST");
                Session.Remove("CDS");
                Session.Remove("NAME");
            Session.Remove("DSR_NAME");
            L5sJS.L5sRun(@"drawRegionChart(); drawProvinceChart();drawDistributorChartV2(); drawCDSChart(); setLabelTimeGone();" +
                    " setLabelPercent(); setLabelMTD(); setLabelTarget();drawDSRChartV2();setLabelECC();setLabelVisit();");
            
        }

        private void LoadSelectArea()
        {
            if (IsPostBack)
                return;
            DataTable dt = L5sSql.Query(@"SELECT [COUNTRY_NAME] FROM [M_COUNTRY]");
            string _Spr = dt.Rows[0]["COUNTRY_NAME"].ToString();
            if (_Spr == "TH")
            {
                slArea.DataSource = L5sSql.Query(@" SELECT AREA
                                                FROM  D_MTD_SECONDATY_SALES_UPDATE _data
                                                JOIN M_DISTRIBUTOR _dist ON _dist.DISTRIBUTOR_CODE=_data.DIST
                                                GROUP BY AREA
                                                order by AREA asc");

                slArea.DataValueField = "AREA";
                slArea.DataTextField = "AREA";
                slArea.DataBind();
            }
            else
            {
                slArea.DataSource = L5sSql.Query(@" SELECT AREA
                                                FROM  D_MTD_SECONDATY_SALES_UPDATE _data
                                                JOIN M_DISTRIBUTOR _dist ON _dist.DISTRIBUTOR_CODE=_data.DIST
                                                GROUP BY AREA
                                                ORDER BY CONVERT(float,REPLACE(REPLACE(REPLACE(REPLACE(AREA,'S','9'),'A',''),'M',''),'-','.')) ASC");

                slArea.DataValueField = "AREA";
                slArea.DataTextField = "AREA";
                slArea.DataBind();
            }

        }

        protected void P5sLbtnRemoveSession_Click(object sender, EventArgs e)
        {
            Session["Where"] = "(1=1)";
            Session.Remove("REGION");
            Session.Remove("PROVINCE");
            Session.Remove("AREA");
            Session.Remove("DIST");
            Session.Remove("CDS");
            Session.Remove("NAME");
            Session.Remove("DSR_NAME"); 
            L5sJS.L5sRun(@"drawRegionChart(); drawProvinceChart();drawDistributorChartV2(); drawCDSChart(); setLabelTimeGone();" +
                " setLabelPercent(); setLabelMTD(); setLabelTarget();drawDSRChartV2();setLabelECC();setLabelVisit();");
            slArea.DataSource = L5sSql.Query(@"SELECT distinct AREA
                                                FROM D_MTD_SECONDATY_SALES_UPDATE");

            slArea.DataValueField = "AREA";
            slArea.DataTextField = "AREA";
            slArea.DataBind();
            Response.Redirect(Request.RawUrl);
        }

        protected void slArea_ServerChange(object sender, EventArgs e)
        {
            String[] area = new String[slArea.Items.Count];
            int f = 0;
            for (int i = 0; i < slArea.Items.Count; i++)
            {
                if (slArea.Items[i].Selected == true)
                {
                    area[f] = slArea.Items[i].Value;
                    f++;
                }
            }
            String multiArea = "";
            for (int i = 0; i < f; i++)
            {
                multiArea += area[i];
                if (i < f - 1)
                    multiArea += ",";
            }
            L5sJS.L5sRun("slAreaChanged('" + multiArea + "');");
        }

    }
}