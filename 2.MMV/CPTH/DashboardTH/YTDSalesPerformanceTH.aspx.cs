using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.DashboardTH
{
    public partial class YTDSalesPerformanceTH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            string parameter = Request["__EVENTARGUMENT"]; // parameter
            if (!IsPostBack)
            {
                Session["Where"] = "(1=1)";
                LoadSelectMonth();
                L5sJS.L5sRun(@"google.charts.setOnLoadCallback(drawRegionChart);
                            google.charts.setOnLoadCallback(drawSubCatgChart);
                            google.charts.setOnLoadCallback(drawYearChart);
                            google.charts.setOnLoadCallback(drawDistributorChartV2);
                            google.charts.setOnLoadCallback(drawBrandChart);setLabelGrowth(); setLabelYTD();");
            }
            else
            {

            }
            Session["Where"] = "(1=1)";
            Session.Remove("REGION");
            Session.Remove("SUB_CATG");
            Session.Remove("BRAND");
            Session.Remove("YEAR");
            Session.Remove("MONTH");
            Session.Remove("NAME");
            L5sJS.L5sRun("drawRegionChart(); drawSubCatgChart(); drawYearChart(); drawBrandChart(); setLabelGrowth(); setLabelYTD();drawDistributorChartV2();");
            P5sLbtnRemoveSession.Text = L5sMaster.L5sLangs["Clear Filter"];
            //year for Growth
            //year for Growth
            string checkyear = DateTime.Now.ToString("MM");
            if (checkyear == "1" || checkyear == "01")
            {
                string year = DateTime.Now.ToString("yyyy");
                int yearafter = Int32.Parse(year) - 1;
                int yearbefore = Int32.Parse(year) - 2;
                YEAR.Text = yearafter + " vs " + yearbefore;
                YEAR_DISTRIBUTOR.Text = yearafter + " & " + yearbefore;
            }
            else
            {
                string year = DateTime.Now.ToString("yyyy");
                int yearbefore = Int32.Parse(year) - 1;
                YEAR.Text = year + " vs " + yearbefore;
                YEAR_DISTRIBUTOR.Text = year + " & " + yearbefore;
            }
            //
       
         
        }

        private void LoadSelectMonth()
        {
            if (IsPostBack)
                return;
            slMonth.DataSource = L5sSql.Query(@"	SELECT 
	                                                distinct 
													month AS VALUE
													, 
	                                                RIGHT(MONTH,3) as MONTH,
													CASE
														WHEN MONTH LIKE '%Jan%' or MONTH = '1' or  MONTH = '01'  THEN '01'	 
														WHEN MONTH LIKE '%Feb%' or MONTH = '2' or  MONTH = '02' THEN '02'	
														WHEN MONTH LIKE '%Mar%' or MONTH = '3' or  MONTH = '03' THEN '03'	
														WHEN MONTH LIKE '%Apr%' or MONTH = '4' or  MONTH = '04' THEN '04'	

														WHEN MONTH LIKE '%May%' or MONTH = '5' or  MONTH = '05' THEN '05'	
														WHEN MONTH LIKE '%Jun%' or MONTH = '6' or  MONTH = '06' THEN '06'	
														WHEN MONTH LIKE '%Jul%' or MONTH = '7' or  MONTH = '07' THEN '07'	
														WHEN MONTH LIKE '%Aug%' or MONTH = '8' or  MONTH = '08' THEN '08'	
														WHEN MONTH LIKE '%Sep%' or MONTH = '9' or  MONTH = '09' THEN '09'	
														WHEN MONTH LIKE '%Oct%' or MONTH = '10' or  MONTH = '10' THEN '10'
														WHEN MONTH LIKE '%Nov%' or MONTH = '11' or  MONTH = '11' THEN '11'
														WHEN MONTH LIKE '%Dec%' or MONTH = '12' or  MONTH = '12' THEN '12'
													END  as Va_Month 
                                                FROM D_YTD_SALES_DATA_SEC
                                                order by Va_Month");
            slMonth.DataValueField = "VALUE";
            slMonth.DataTextField = "MONTH";
            slMonth.DataBind();

        }

        protected void P5sLbtnRemoveSession_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>SaveWithParameter();result = [];</script>", false);
            Session["Where"] = "(1=1)";
            Session.Remove("REGION");
            Session.Remove("SUB_CATG");
            Session.Remove("BRAND");
            Session.Remove("YEAR");
            Session.Remove("MONTH");
            L5sJS.L5sRun("drawRegionChart(); drawSubCatgChart(); drawYearChart(); drawBrandChart(); setLabelGrowth(); setLabelYTD();fadeOut();drawDistributorChartV2");
            slMonth.DataSource = L5sSql.Query(@"SELECT 
	                                                distinct 
													month AS VALUE
													, 
	                                                RIGHT(MONTH,3) as MONTH,
													CASE
														WHEN MONTH LIKE '%Jan%' or MONTH = '1' or  MONTH = '01'  THEN '01'	 
														WHEN MONTH LIKE '%Feb%' or MONTH = '2' or  MONTH = '02' THEN '02'	
														WHEN MONTH LIKE '%Mar%' or MONTH = '3' or  MONTH = '03' THEN '03'	
														WHEN MONTH LIKE '%Apr%' or MONTH = '4' or  MONTH = '04' THEN '04'	

														WHEN MONTH LIKE '%May%' or MONTH = '5' or  MONTH = '05' THEN '05'	
														WHEN MONTH LIKE '%Jun%' or MONTH = '6' or  MONTH = '06' THEN '06'	
														WHEN MONTH LIKE '%Jul%' or MONTH = '7' or  MONTH = '07' THEN '07'	
														WHEN MONTH LIKE '%Aug%' or MONTH = '8' or  MONTH = '08' THEN '08'	
														WHEN MONTH LIKE '%Sep%' or MONTH = '9' or  MONTH = '09' THEN '09'	
														WHEN MONTH LIKE '%Oct%' or MONTH = '10' or  MONTH = '10' THEN '10'
														WHEN MONTH LIKE '%Nov%' or MONTH = '11' or  MONTH = '11' THEN '11'
														WHEN MONTH LIKE '%Dec%' or MONTH = '12' or  MONTH = '12' THEN '12'
													END  as Va_Month 
                                                FROM D_YTD_SALES_DATA_SEC
                                                order by Va_Month");
            slMonth.DataValueField = "VALUE";
            slMonth.DataTextField = "MONTH";
            slMonth.DataBind();
            // Response.Redirect(Request.RawUrl);
        }

        protected void slMonth_ServerChange(object sender, EventArgs e)
        {
            String[] month = new String[slMonth.Items.Count];
            int f = 0;
            for (int i = 0; i < slMonth.Items.Count; i++)
            {

                if (slMonth.Items[i].Selected == true)
                {
                    month[f] = slMonth.Items[i].Value;
                    f++;
                }
            }
            String multiMonth = "";
            for (int i = 0; i < f; i++)
            {
                multiMonth += month[i];
                if (i < f - 1)
                    multiMonth += ",";
            }
            L5sJS.L5sRun("slMonthChanged('" + multiMonth + "');");
        }
    }
}