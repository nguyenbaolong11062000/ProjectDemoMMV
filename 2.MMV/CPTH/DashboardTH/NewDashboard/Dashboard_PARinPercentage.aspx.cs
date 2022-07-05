using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.CPTH.DashboardTH.NewDashboard
{
    public partial class Dashboard_PARinPercentage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            P5sLbtnRemoveSession.Text = L5sMaster.L5sLangs["Clear Filter"];
            L5sJS.L5sRun(@"google.charts.setOnLoadCallback(drawRegionChart);
                           google.charts.setOnLoadCallback(drawDistributorChart);
                           google.charts.setOnLoadCallback(drawDSRChart);
                           setLabelParSec();setLabelTime_gone();changeColorParSec();
                            ");
            Session["Where"] = "(1=1)";
            Session.Remove("REGION");
            Session.Remove("DISTRIBUTOR_NAME");
            Session.Remove("DSR");
        }
        protected void P5sLbtnRemoveSession_Click(object sender, EventArgs e)
        {
            Session["Where"] = "(1=1)";
            Session.Remove("REGION");
            Session.Remove("DISTRIBUTOR_NAME");
            Session.Remove("DSR");
            L5sJS.L5sRun("fadeOut();drawRegionChart();drawDistributorChart();drawDSRChart();setLabelParSec();setLabelTime_gone();changeColorParSec();");

        }
    }
}