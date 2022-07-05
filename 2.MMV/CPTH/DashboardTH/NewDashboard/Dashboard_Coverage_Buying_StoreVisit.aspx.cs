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
    public partial class Dashboard_Coverage_Buying_StoreVisit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            Session["Where4"] = "(4=4)";
            P5sLbtnRemoveSession.Text = L5sMaster.L5sLangs["Clear Filter"];
            L5sJS.L5sRun(@"google.charts.setOnLoadCallback(drawRegionChart);
                            google.charts.setOnLoadCallback(drawDSRChart);
                            google.charts.setOnLoadCallback(drawDistributorChart);
                            google.charts.setOnLoadCallback(drawBuyingChart);
                            google.charts.setOnLoadCallback(drawStoreVisitedChart);

                            setLabelStoreVisited();
                            setLabelECC();

                            "); 
            
            Session.Remove("REGIONCBS");
            Session.Remove("DISTRIBUTOR_NAMECBS");
            Session.Remove("SALES_NAMECBS");

        }
        protected void P5sLbtnRemoveSession_Click(object sender, EventArgs e)
        {
            //rblMeasurementSystem.SelectedValue = "REGION";
            Session["Where4"] = "(4=4)";
            Session.Remove("REGIONCBS");
            Session.Remove("DISTRIBUTOR_NAMECBS");
            Session.Remove("SALES_NAMECBS");

            L5sJS.L5sRun("drawRegionChart(); drawDistributorChart();drawDSRChart();clearFilter();fadeOut();");
            //slArea.DataSource = L5sSql.Query(@"SELECT distinct AREA
            //                                    FROM D_MTD_PRIMARY_UPDATE");

            //slArea.DataValueField = "AREA";
            //slArea.DataTextField = "AREA";
            //slArea.DataBind();
            // Response.Redirect(Request.RawUrl);
        }
    }
}