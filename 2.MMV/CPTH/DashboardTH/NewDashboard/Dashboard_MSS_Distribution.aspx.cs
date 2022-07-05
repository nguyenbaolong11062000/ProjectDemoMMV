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
    public partial class Dashboard_MSS_Distribution : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            P5sLbtnRemoveSession.Text = L5sMaster.L5sLangs["Clear Filter"];
            Session["Where3"] = "(3=3)";

            L5sJS.L5sRun(@"getFilterRe();
                         setLabelTotalPointRL();
                         setLabelTimeGone();
                         google.charts.setOnLoadCallback(drawRegionChart);
                         google.charts.setOnLoadCallback(drawDistributorChart);
                         google.charts.setOnLoadCallback(drawMPChart);
                         google.charts.setOnLoadCallback(drawWSChart);
                         google.charts.setOnLoadCallback(drawSPMChart);
                         google.charts.setOnLoadCallback(drawMNMChart);
                          
                        ");
            Session.Remove("REGIONMSS");
            Session.Remove("DISTRIBUTOR_NAMEMSS");
            Session.Remove("SALES_NAMEMSS");
        }
        protected void P5sLbtnRemoveSession_Click(object sender, EventArgs e)
        {
            rblMeasurementSystem.SelectedValue = "REGION";
    
            Session["Where3"] = "(3=3)";
            Session.Remove("REGIONMSS");
            Session.Remove("DISTRIBUTOR_NAMEMSS");
            Session.Remove("SALES_NAMEMSS");

            L5sJS.L5sRun("clearSession();fadeOut();drawRegionChart(); drawDistributorChart();drawMPChart();drawWSChart();drawSPMChart();drawMNMChart();filterPointRL();");
            //slArea.DataSource = L5sSql.Query(@"SELECT distinct AREA
            //                                    FROM D_MTD_PRIMARY_UPDATE");

            //slArea.DataValueField = "AREA";
            //slArea.DataTextField = "AREA";
            //slArea.DataBind();
            // Response.Redirect(Request.RawUrl);
        }

        protected void rblMeasurementSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            L5sJS.L5sRun("drawRegionChart(); drawDistributorChart();drawMPChart();drawWSChart();drawSPMChart();drawMNMChart();getFilterRe();");
        }
    }
}