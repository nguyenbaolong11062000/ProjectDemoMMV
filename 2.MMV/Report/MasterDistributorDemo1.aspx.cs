using L5sDmComm;
using P5sCmm;
using P5sDmComm;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.Report
{
    public partial class MasterDistributorDemo1 : System.Web.UI.Page
    {
        public L5sAutocomplete P5sActRegion, P5sActArea;
        public L5sAutocomplete P5sActProvince;
        protected void Page_Load(object sender, EventArgs e)
        {
            //hàm hiển thị
            L5sInitial.Load(ViewState);
            this.P5sAutoCompleteInit();
        }
        private void P5sAutoCompleteInit()
        {
            //hàm đổ dữ liệu vào page hiển thị
            this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRegion(""), this.P5sTxtRegionCD.ClientID, 0, true, this.P5sTxtAreaCD.ClientID) : this.P5sActRegion;
            this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(P5sCmmFns.P5sGetArea("REGION_CD"), this.P5sTxtAreaCD.ClientID, 0, true, this.P5sTxtProvinceCD.ClientID) : this.P5sActArea;
            this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegionCD.ClientID);

            this.P5sActProvince = this.P5sActProvince == null ? new L5sAutocomplete(P5sCmmFns.P5sGetProvince("AREA_CD"), this.P5sTxtProvinceCD.ClientID, 0, true) : this.P5sActProvince;
            this.P5sActProvince.L5sChangeFilteringId(this.P5sTxtAreaCD.ClientID);
        }
        protected void P5sLbtnExport_Click(object sender, EventArgs e)
        {
            String provinceCDs = this.P5sTxtProvinceCD.Text;
            #region Sql
            String sql = String.Format(@"select reg.REGION_CODE
		                                            ,are.AREA_CODE
		                                            ,dist.DISTRIBUTOR_CODE
		                                            ,dist.DISTRIBUTOR_NAME
		                                            ,dist.DISTRIBUTOR_EMAIL
		                                            ,dt.DISTRICT_CODE
		                                            ,prv.PROVINCE_DISTRIBUTOR_CODE
		                                            ,cm.COMMUNE_CODE
		                                            ,cm.LONGITUDE_LATITUDE
		                                            ,cm.COMMUNE_NAME_EN
		                                            ,dt.DISTRICT_NAME_EN
		                                            ,prv.PROVINCE_NAME_EN
		                                            ,dist.DISTRIBUTOR_ADDRESS
		                                            ,mdt.DISTRIBUTOR_TYPE_CODE
		                                            ,CASE dist.ACTIVE WHEN 1 THEN 'Active' ELSE 'Inactive' END as ACTIVE
                                            FROM [M_DISTRIBUTOR.] dist
                                            join M_COMMUNE cm on cm.COMMUNE_CD = dist.COMMUNE_CD 
                                            join  M_DISTRICT dt on dt.DISTRICT_CD = cm.DISTRICT_CD
                                            join M_PROVINCE prv on prv.PROVINCE_CD = dt.PROVINCE_CD
                                            join M_AREA_PROVINCE map on map.PROVINCE_CD = prv.PROVINCE_CD and map.active = 1
                                            join M_AREA are on are.AREA_CD = map.AREA_CD
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD
                                            join [M_DISTRIBUTOR_TYPE] mdt on mdt.DISTRIBUTOR_TYPE_CD = dist.DISTRIBUTOR_TYPE_CD
                                            WHERE prv.PROVINCE_CD IN ({0})
                                            ORDER BY AREA_ORDER, PROVINCE_CODE", provinceCDs);
            #endregion
            DataTable tb = P5sCmmFns.SqlDatatableTimeout(sql, 36000);
            if (tb == null || tb.Rows.Count == 0)
            {
                L5sMsg.Show("No data.");
            }
            else
            {
                String sourcePath = Server.MapPath("~/Report/Templs/MasterDistributorDemo1.xlsx");
                String resultPath = Server.MapPath("~/Exports/MasterDistributorDemo1" + DateTime.Now.ToString("yyMMddhhmmssff") + ".xlsx");
                P5sEReport W5sRpt = new P5sEReport();
                W5sRpt.P5sAdd("list", tb);
                W5sRpt.P5sAdd("DateTime", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                W5sRpt.P5sCreateReport(sourcePath, resultPath);
                P5sEReport.P5sASPExportFileToClient(resultPath, "excel");
            }
        }
    }
}