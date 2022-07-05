using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using L5sDmComm;
using System.IO;
using System.Data.SqlClient;
using P5sCmm;
using System.Threading;
using P5sDmComm;

namespace MMV
{
    public partial class MapGeocodeToHierarchy : System.Web.UI.Page
    {

        public L5sAutocomplete P5sActRegion, P5sActArea;
        public L5sAutocomplete P5sActDistributor;
        public L5sAutocomplete P5sActDSR;


        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            this.P5sAutoCompleteInit();

          
        }

        private void P5sAutoCompleteInit()
        {
            this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRegion(""), this.P5sTxtRegionCD.ClientID, 0, true, this.P5sTxtAreaCD.ClientID) : this.P5sActRegion;
            this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(P5sCmmFns.P5sGetArea("REGION_CD"), this.P5sTxtAreaCD.ClientID, 0, true, this.P5sTxtDistributor.ClientID) : this.P5sActArea;
            this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegionCD.ClientID);

            this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistributor("AREA_CD"), this.P5sTxtDistributor.ClientID, 0, true, this.P5sTxtDSR.ClientID) : this.P5sActDistributor;
            this.P5sActDistributor.L5sChangeFilteringId(this.P5sTxtAreaCD.ClientID);

            this.P5sActDSR = this.P5sActDSR == null ? new L5sAutocomplete(P5sCmmFns.P5sGetSales("DISTRIBUTOR_CD"), this.P5sTxtDSR.ClientID, 0, true) : this.P5sActDSR;
            this.P5sActDSR.L5sChangeFilteringId(this.P5sTxtDistributor.ClientID);
        }



        private Boolean P5sIsValid()
        {

            String dsr = this.P5sTxtDSR.Text;


            if (dsr == "")
            {
                L5sMsg.Show("DSR bắt buộc chọn!");
                return false;
            }

            return true;
        }



        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {
            try
            {
                if (this.P5sActRegion != null)
                    this.P5sActRegion.L5sSetDefaultValues(this.P5sTxtRegionCD.Text);


                if (this.P5sActArea != null)
                    this.P5sActArea.L5sSetDefaultValues(this.P5sTxtAreaCD.Text);

                if (this.P5sActDistributor != null)
                    this.P5sActDistributor.L5sSetDefaultValues(this.P5sTxtDistributor.Text);

                if (this.P5sActDSR != null)
                    this.P5sActDSR.L5sSetDefaultValues(this.P5sTxtDSR.Text);
            }
            catch (Exception)
            {

            }
        }

        protected void P5sSearch_Click(object sender, EventArgs e)
        {
    
        }
        public static DataTable dtHierarchy;
        public static DataTable dtGeoCode;
        public static DataTable dtProxyIP;
        public static int numberGeoCode = 0, numberProxy = 0;
        protected void P5sLbtnExport_Click(object sender, EventArgs e)
        {

            if (!P5sIsValid())
                return;
            String dsr = this.P5sTxtDSR.Text;
            String sql = String.Format(@"SELECT dis.DISTRIBUTOR_CODE, dis.DISTRIBUTOR_NAME, 
                           cust.CUSTOMER_CODE, cust.CUSTOMER_NAME,cust.CUSTOMER_ADDRESS,cust.LONGITUDE_LATITUDE,cust.LONGITUDE_LATITUDE_ACCURACY,
	                       cust.CUSTOMER_CHAIN_CODE,sls.SALES_CODE, sls.SALES_NAME, rout.ROUTE_CODE, rout.ROUTE_NAME AS ROUTE_DESC,
	                       ISNULL(pro.PROVINCE_CODE,'') AS PROVINCE_CODE, 
	                       ISNULL(pro.PROVINCE_NAME_EN,'') AS PROVINCE_NAME_EN,
	                       ISNULL(dist.DISTRICT_CODE,'') AS DISTRICT_CODE, 
	                       ISNULL(dist.DISTRICT_NAME_EN,'') AS DISTRICT_NAME_EN ,
	                       ISNULL(cmm.COMMUNE_CODE,'') AS COMMUNE_CODE,
	                       ISNULL(cmm.COMMUNE_NAME_EN,'') AS  COMMUNE_NAME_EN,
                           CASE WHEN pro.PROVINCE_CODE IS NULL AND dist.DISTRICT_CODE IS NULL AND cmm.COMMUNE_CODE IS NULL THEN N'Phân giải tọa độ thất bại.' END AS  NOTE
                    FROM 
		                    M_DISTRIBUTOR dis 
                            INNER JOIN M_SALES sls ON dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
							INNER JOIN O_SALES_ROUTE slsR ON sls.SALES_CD = slsR.SALES_CD AND slsR.ACTIVE = 1
							INNER JOIN O_CUSTOMER_ROUTE custR ON slsR.ROUTE_CD = custR.ROUTE_CD AND custR.ACTIVE = 1
							INNER JOIN M_CUSTOMER cust ON custR.CUSTOMER_CD = cust.CUSTOMER_CD
		                    INNER JOIN M_ROUTE rout ON custR.ROUTE_CD = rout.ROUTE_CD
							LEFT JOIN O_MAPGEOCODE omgc ON omgc.LONGITUDE_LATITUDE=cust.LONGITUDE_LATITUDE
		                    LEFT JOIN M_COMMUNE cmm ON omgc.COMMUNE_CD = cmm.COMMUNE_CD
		                    LEFT JOIN M_DISTRICT dist ON omgc.DISTRICT_CD = dist.DISTRICT_CD
		                    LEFT JOIN M_PROVINCE pro	ON omgc.PROVINCE_CD = pro.PROVINCE_CD
                    WHERE sls.SALES_CD IN ({0}) AND cust.LONGITUDE_LATITUDE IS NOT NULL AND ( cust.COMMUNE_CD IS NULL  OR cust.COMMUNE_CD = 0 )", dsr);

            dtGeoCode = L5sSql.Query(sql);

            if (dtGeoCode == null || dtGeoCode.Rows.Count == 0)
            {
                L5sMsg.Show("Không có dữ liệu!");
                return;
            }
            P5sEReport P5sRpt = new P5sEReport();
            P5sRpt.P5sAdd("list", dtGeoCode);
            P5sRpt.P5sAdd("DateTimeNow", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

            string fileName = "MapGeocodeToHierarchy_";
            String sourcePath = Server.MapPath("~/Report/Templs/MapGeocodeToHierarchy.xlsx");
            String resultPath = Server.MapPath("~/Exports/" + fileName + DateTime.Now.ToString("yyyyMMddhhmmssffffff") + ".xlsx");

            P5sRpt.P5sCreateReport(sourcePath, resultPath);
            P5sEReport.P5sASPExportFileToClient(resultPath, "excel");


                

        }


   

     
    }
}
