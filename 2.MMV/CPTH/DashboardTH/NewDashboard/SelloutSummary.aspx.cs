using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.CPTH.DashboardTH.NewDashboard
{
    public partial class SelloutSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            string parameter = Request["__EVENTARGUMENT"]; // parameter
            if (!IsPostBack)
            {
                Session["Where1"] = "(1=1)";
                P5sLbtnRemoveSession.Text = L5sMaster.L5sLangs["Clear Filter"].ToString();
                L5sJS.L5sRun(@"google.charts.setOnLoadCallback();
                            google.charts.setOnLoadCallback(drawYearChart);
                            google.charts.setOnLoadCallback(drawQuarterChart);
                            google.charts.setOnLoadCallback(drawMonthChart);
                            google.charts.setOnLoadCallback(setLabelQ1);
                            google.charts.setOnLoadCallback(setLabelQ2);
                            google.charts.setOnLoadCallback(setLabelQ3);
                            google.charts.setOnLoadCallback(setLabelQ4);");
            }
            Session["Where1"] = "(1=1)";
            Session.Remove("QUARTER");
            Session.Remove("YEAR");
            Session.Remove("MONTH");
            Session.Remove("NAME");
        }

        protected void P5sLbtnRemoveSession_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>SaveWithParameter();result = [];</script>", false);
            Session["Where1"] = "(1=1)";
            Session.Remove("QUARTER");
            Session.Remove("YEAR");
            Session.Remove("MONTH");
            Session.Remove("NAME");
            L5sJS.L5sRun("drawYearChart();drawQuarterChart();drawMonthChart();setLabelQ1();setLabelQ2();setLabelQ3();setLabelQ4();");
        }
    }
}