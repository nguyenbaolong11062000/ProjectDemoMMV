using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.CPTH.DashboardTH.NewDashboard
{
    public partial class MTDActAndTarget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            string parameter = Request["__EVENTARGUMENT"]; // parameter
            if (!IsPostBack)
            {
                Session["Where2"] = "(2=2)";
                P5sLbtnRemoveSession.Text = L5sMaster.L5sLangs["Clear Filter"].ToString();
                L5sJS.L5sRun(@"google.charts.setOnLoadCallback();
                            google.charts.setOnLoadCallback(drawRegionChart);
                            google.charts.setOnLoadCallback(drawDistributorChart);
                            google.charts.setOnLoadCallback(drawDSRChart);
                            google.charts.setOnLoadCallback(drawMTDActualChart);
                                    setLabelTarget();");
            }
            Session["Where2"] = "(2=2)";
            Session.Remove("DSR");
            Session.Remove("DISTRIBUTOR");
            Session.Remove("MONTH");
            Session.Remove("REGION");
        }

        protected void P5sLbtnRemoveSession_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>SaveWithParameter();result = [];</script>", false);
            Session["Where2"] = "(2=2)";
            Session.Remove("DSR");
            Session.Remove("DISTRIBUTOR");
            Session.Remove("MONTH");
            Session.Remove("REGION");
            L5sJS.L5sRun("drawRegionChart();drawDistributorChart();drawDSRChart();drawMTDActualChart();setLabelTarget()");
        }
    }
}