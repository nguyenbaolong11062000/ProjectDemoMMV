using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.CPTH.DashboardTH.NewDashboard
{
    public partial class DistributorDailySummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            string parameter = Request["__EVENTARGUMENT"]; // parameter
            if (!IsPostBack)
            {
                Session["Where1"] = "(1=1)";
                Session["Where2"] = "(2=2)";
                P5sLbtnRemoveSession.Text = L5sMaster.L5sLangs["Clear Filter"].ToString();
                L5sJS.L5sRun(@"clearSession();getFilterRe();
                            google.charts.setOnLoadCallback();
                            google.charts.setOnLoadCallback(drawYearChart);
                            google.charts.setOnLoadCallback(drawQuarterChart);
                            google.charts.setOnLoadCallback(drawMonthChart);
                            google.charts.setOnLoadCallback(drawMTDActualChart);
                            google.charts.setOnLoadCallback(drawRegionChart);
                            google.charts.setOnLoadCallback(drawDistributorChart);
                            google.charts.setOnLoadCallback(drawDSRChart);
                            google.charts.setOnLoadCallback(drawBuyingChart);
                            google.charts.setOnLoadCallback(drawStoreVisitedChart);
                            google.charts.setOnLoadCallback(setLabelStoreVisited);
                            google.charts.setOnLoadCallback(setLabelECC);
                            google.charts.setOnLoadCallback(drawRegionChartCoverage);
                            google.charts.setOnLoadCallback(drawDistributorChartCoverage);
                            google.charts.setOnLoadCallback(drawDSRChartCoverage);google.charts.setOnLoadCallback(drawRegionChartMSS);
                            google.charts.setOnLoadCallback(drawDistributorChartMSS);                  
                            setLabelTotalPointRL(); 
                            google.charts.setOnLoadCallback(drawMPChart);
                            google.charts.setOnLoadCallback(drawWSChart);
                            google.charts.setOnLoadCallback(drawSPMChart);
                            google.charts.setOnLoadCallback(drawMNMChart)
                            getFilterRe();setLabelTimeGone();
                            google.charts.setOnLoadCallback(drawDSRChartCoverage);
                            google.charts.setOnLoadCallback(drawDistributorChartCoverage);
                            google.charts.setOnLoadCallback(drawRegionChartCoverage); 
                            google.charts.setOnLoadCallback(drawBuyingChart);
                            google.charts.setOnLoadCallback(drawStoreVisitedChart);
                            setLabelECC();
                            setLabelStoreVisited();
                            drawStoreVisitedChart();
                            setLabelTarget();");
            }
            Session["Where1"] = "(1=1)";
            Session["Where2"] = "(2=2)";
            Session.Remove("QUARTER");
            Session.Remove("YEAR");
            Session.Remove("MONTH");
            Session.Remove("NAME");
            Session.Remove("DSR");
            Session.Remove("DISTRIBUTOR");
            Session.Remove("DISTRIBUTOR_NAME");
            Session.Remove("SALES_NAME");
            Session.Remove("MONTH");
            Session.Remove("REGION");
            Session.Remove("MTD");
             ///MTD
            Session.Remove("REGIONMSS");
            Session.Remove("DISTRIBUTOR_NAMEMSS");
            Session.Remove("SALES_NAMEMSS");
            Session["Where3"] = "(3=3)";
            //CBS
            Session["Where4"] = "(4=4)";
            Session.Remove("REGIONCBS");
            Session.Remove("DISTRIBUTOR_NAMECBS");
            Session.Remove("SALES_NAMECBS");
        }

        protected void P5sLbtnRemoveSession_Click(object sender, EventArgs e)
        {
            rblMeasurementSystem.SelectedValue = "REGION";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>SaveWithParameter();result = [];</script>", false);
            Session["Where"] = "(1=1)";
            Session["Where1"] = "(1=1)";
            Session["Where2"] = "(2=2)";
            Session.Remove("QUARTER");
            Session.Remove("YEAR");
            Session.Remove("MONTH");
            Session.Remove("NAME");
            Session.Remove("DSR");
            Session.Remove("DISTRIBUTOR");
            Session.Remove("DISTRIBUTOR_NAME");
            Session.Remove("SALES_NAME");
            Session.Remove("MONTH");
            Session.Remove("REGION");
            Session.Remove("MTD");
            Session.Remove("REGIONMSS");
            Session.Remove("DISTRIBUTOR_NAMEMSS");
            Session.Remove("SALES_NAMEMSS");
            Session["Where3"] = "(3=3)";
            //CBS
            Session["Where4"] = "(4=4)";
            Session.Remove("REGIONCBS");
            Session.Remove("DISTRIBUTOR_NAMECBS");
            Session.Remove("SALES_NAMECBS");
            L5sJS.L5sRun(@"clearSession();getFilterRe(); drawYearChart();drawQuarterChart();drawMonthChart();drawMTDActualChart();
                            drawRegionChart();drawDistributorChart();drawDSRChart();drawBuyingChart();
                            drawStoreVisitedChart();setLabelStoreVisited();setLabelECC();
                            drawRegionChartCoverage();drawDistributorChartCoverage();
                            drawDSRChartCoverage();drawRegionChartMSS();
                            drawDistributorChartMSS();setLabelTotalPointRL();setLabelTimeGone();drawMPChart();
                            drawWSChart();drawSPMChart();drawMNMChart();filterPointRL();setLabelTarget();");
        }

        protected void rblMeasurementSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            L5sJS.L5sRun("drawRegionChartMSS(); drawDistributorChartMSS();drawMPChart();drawWSChart();drawSPMChart();drawMNMChart();getFilterRe();");
        }
    }
}