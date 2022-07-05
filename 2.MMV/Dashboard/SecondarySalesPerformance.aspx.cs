using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.Dashboard
{
    public partial class SecondarySalesPerformance : System.Web.UI.Page
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
                            google.charts.setOnLoadCallback(drawDistributorChart);
                            google.charts.setOnLoadCallback(drawCDSChart);
                            google.charts.setOnLoadCallback(drawDSRChart);setLabelTimeGone(); setLabelECC(); setLabelVisit(); setLabelPercent(); setLabelMTD(); setLabelTarget();");
            }
            Session["Where"] = "(1=1)";
            Session.Remove("REGION");
            Session.Remove("PROVINCE");
            Session.Remove("AREA");
            Session.Remove("DIST");
            Session.Remove("CDS");
            Session.Remove("NAME");
            Session.Remove("DSR_NAME");
            L5sJS.L5sRun("drawRegionChart(); drawProvinceChart(); drawDistributorChart(); drawCDSChart(); setLabelTimeGone(); setLabelPercent(); setLabelMTD(); setLabelTarget();");
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
                                                FROM  D_MTD_SECONDATY_SALES_UPDATE
                                                GROUP BY AREA
                                                order by AREA asc");

                slArea.DataValueField = "AREA";
                slArea.DataTextField = "AREA";
                slArea.DataBind();
            }
            else
            {
                slArea.DataSource = L5sSql.Query(@" select AREA_CODE
                                                from(
	                                                SELECT distinct  are.AREA_CODE,are.AREA_ORDER
	                                                FROM 					
	                                                [M_DISTRIBUTOR] dis 
	                                                join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
	                                                join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
	                                                join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
	                                                join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
	                                                join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
	                                                join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                           
                                                ) t
                                                ORDER BY AREA_ORDER asc");

                slArea.DataValueField = "AREA_CODE";
                slArea.DataTextField = "AREA_CODE";
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
            Session.Remove("DSR_NAME");
            Session.Remove("NAME");
           
            L5sJS.L5sRun("drawRegionChart(); drawProvinceChart(); drawDistributorChart(); drawCDSChart(); drawDSRChart(); setLabelTimeGone(); setLabelECC(); setLabelVisit(); setLabelPercent(); setLabelMTD(); setLabelTarget();");
            slArea.DataSource = L5sSql.Query(@"select AREA_CODE
                                            from(
	                                            SELECT distinct  are.AREA_CODE,are.AREA_ORDER
	                                            FROM 					
	                                            [M_DISTRIBUTOR] dis 
	                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
	                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
	                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
	                                            join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
	                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
	                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                           
                                            ) t
                                            ORDER BY AREA_ORDER asc");

            slArea.DataValueField = "AREA_CODE";
            slArea.DataTextField = "AREA_CODE";
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