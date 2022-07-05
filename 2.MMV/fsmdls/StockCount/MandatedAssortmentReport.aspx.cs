using L5sDmComm;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using P5sCmm;
using P5sDmComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMV.fsmdls.StockCount
{
    public partial class MandatedAssortmentReport : System.Web.UI.Page
    {
        public L5sAutocomplete P5sActRegion, P5sActArea;
        public L5sAutocomplete P5sActDistributor;
        public L5sAutocomplete P5sActDSR;
        public L5sAutocomplete P5sActSupervisor;
        public L5sForm P5sMainFrm;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.P5sTxtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            this.P5sTxtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            L5sInitial.Load(ViewState);
            this.P5sAutoCompleteInit();
        }
        private void P5sAutoCompleteInit()
        {
            this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRegion(""), this.P5sTxtRegionCD.ClientID, 0, true, this.P5sTxtAreaCD.ClientID) : this.P5sActRegion;
            this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(P5sCmmFns.P5sGetArea("REGION_CD"), this.P5sTxtAreaCD.ClientID, 0, true, this.P5sTxtDistributor.ClientID) : this.P5sActArea;
            this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegionCD.ClientID);

            this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistributor("AREA_CD"), this.P5sTxtDistributor.ClientID, 0, true) : this.P5sActDistributor;
            this.P5sActDistributor.L5sChangeFilteringId(this.P5sTxtAreaCD.ClientID);
        }
		private Boolean P5sIsValid()
		{

			String dis = this.P5sTxtDistributor.Text;


			if (dis == "")
			{
				L5sMsg.Show("All fields marked with an asterisk (*) are required.");
				return false;
			}
			return true;

		}

		protected void P5sLbtnExport_Click(object sender, EventArgs e)
		{
			DateTime dateFrom = DateTime.ParseExact(Request[P5sTxtFromDate.UniqueID], "dd-MM-yyyy", null);
			DateTime dateTo = DateTime.ParseExact(Request[P5sTxtToDate.UniqueID], "dd-MM-yyyy", null);

			
			String Dfrom = dateFrom.ToString("MM/dd/yyyy");
			String Dto = dateTo.ToString("MM/dd/yyyy");





			if (!this.P5sIsValid())
				return;
			String distributorCD = this.P5sTxtDistributor.Text;
			String sql = String.Format(@"
                select * , (Case when Convert(float,Proposed)  = 0 then 0 else (Convert(float,Counted)  /Convert(float,Proposed))  end ) as PERCENTPROPOSED,
		                 (CASE when Convert(float,Proposed)  = 0 then '6. <20%' ELSE
						                (case when  Convert(float,Counted)  /Convert(float,Proposed) >=1     then '1. 100%'
							                  when   Convert(float,Counted)  /Convert(float,Proposed) >=0.8  then '2. 80%-99%'  
							                  when   Convert(float,Counted)  /Convert(float,Proposed) >=0.6  then '3. 60%-79%'  
							                  when   Convert(float,Counted)  /Convert(float,Proposed) >=0.4  then '4. 40%-59%'  
							                  when   Convert(float,Counted)  /Convert(float,Proposed) >=0.2  then '5. 20%-39%'  

							                else '6. <20%'
						                 end)
			                END)   as PERCENTGROUP
                from(
	                SELECT
		                region.REGION_CODE AS REGION_CODE
		                ,area.AREA_CODE AS AREA_CODE
		                ,DISTRIBUTOR_CODE
		                ,DISTRIBUTOR_NAME
		                ,CUST.CUSTOMER_CODE
		                ,CUST.CUSTOMER_NAME
		                ,CUST.CUSTOMER_ADDRESS
		                ,CUST.GLOBAL_RE AS GLOBAL_RE
		                ,CASE WHEN GLRE.GLOBAL_DESC is null THEN CUST.GLOBAL_RE ELSE GLRE.GLOBAL_DESC END AS GLOBAL_DESC
		                ,SALES_CODE AS DSR_CODE
		                ,SALES_NAME AS DSR_NAME
		                ,(SELECT COUNT(MCUST.CUSTOMER_CODE)
			                FROM M_CUSTOMER MCUST
			                INNER JOIN O_CUSTOMER_ROUTE OCR ON OCR.CUSTOMER_CD = MCUST.CUSTOMER_CD
			                INNER JOIN O_SALES_ROUTE OSR ON OSR.ROUTE_CD = OCR.ROUTE_CD
			                WHERE MCUST.GLOBAL_RE = CUST.GLOBAL_RE AND OSR.SALES_CD =SL.SALES_CD
			                and MCUST.ACTIVE = 1
		                ) AS RL
		                ,CAST((
			                select count(t.product_size_cd)
			                from
			                (select Max(PRODUCT_REQUIRED_DATE) as PRODUCT_REQUIRED_DATE, product_size_cd
			                from O_PRODUCT_REQUIRED_STOCK
			                where GLOBAL_RE = CUST.GLOBAL_RE and ACTIVE = 1
			                group by product_size_cd
			                ) t 
		                ) as FLOAT ) AS Proposed
		                ,CAST((
			                select COUNT(PRODUCT_SIZE_CD)
			                from(
				                select max(PRODUCT_COUNT_DATE) as PRODUCT_COUNT_DATE,PRODUCT_SIZE_CD
				                from O_PRODUCT_COUNT_BY_CUSTOMER
				                where PRODUCT_VARIOUS > 0 and CUSTOMER_CODE = CUST.CUSTOMER_CODE and ACTIVE=1 and   convert (nvarchar,PRODUCT_COUNT_DATE,101 ) between '{1}' and '{2}'
				                group by PRODUCT_SIZE_CD
			                ) t
		                )AS FLOAT  )AS Counted
		                --,'' AS PER_Proposed
		                --,'' AS PER_GROUP
	                FROM M_SALES SL
	                INNER JOIN O_SALES_ROUTE OSR ON OSR.SALES_CD = SL.SALES_CD AND OSR.ACTIVE =1
	                INNER JOIN O_CUSTOMER_ROUTE OCR ON OCR.ROUTE_CD = OSR.ROUTE_CD AND OCR.ACTIVE = 1
	                INNER JOIN M_CUSTOMER CUST ON CUST.CUSTOMER_CD = OCR.CUSTOMER_CD AND CUST.ACTIVE = 1
	                LEFT JOIN M_GLOBAL_RE GLRE ON GLRE.GLOBAL_RE = CUST.GLOBAL_RE AND GLRE.ACTIVE = 1
	                JOIN [M_DISTRIBUTOR.] dis on SL.DISTRIBUTOR_CD = dis.DISTRIBUTOR_CD and dis.ACTIVE = 1
	                Join M_COMMUNE com on dis.COMMUNE_CD = com.COMMUNE_CD and com.ACTIVE =1
	                join M_DISTRICT dist on dist.DISTRICT_CD = com.DISTRICT_CD and dist.ACTIVE =1
	                join M_PROVINCE pro on pro.PROVINCE_CD = dist.PROVINCE_CD and pro.ACTIVE =1
	                join M_AREA_PROVINCE map on map.PROVINCE_CD = pro.PROVINCE_CD and map.ACTIVE =1
	                Join M_AREA area on area.AREA_CD = map.AREA_CD and area.ACTIVE =1
	                join M_REGION region on region.REGION_CD = area.REGION_CD and region.ACTIVE =1
		
		                where dis.DISTRIBUTOR_CD in ({0})
                )T
                ", distributorCD, Dfrom, Dto);

			DataTable dt = L5sSql.Query(sql);
			if (dt == null || dt.Rows.Count == 0)
			{
				L5sMsg.Show("No data.");
				return;
			}
			String nameFile = "MandatedAssortmentReport_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
			String sourcePath = Server.MapPath("Templs/TemplMandatedAssortmentReport.xlsx");
			String sourcePathTemp = Server.MapPath("~/Exports/"+nameFile);
			String resultPath = Server.MapPath("~/Exports/"+nameFile);


			P5sEReport P5sRpt = new P5sEReport();
			P5sRpt.P5sAdd("list", dt);
            //String nameFile = "TemplMandatedAssortmentReport_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            P5sRpt.P5sAdd("PrintDate", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
			P5sRpt.P5sAdd("from", dateFrom.ToString("dd-MM-yyyy"));
			P5sRpt.P5sAdd("to", dateTo.ToString("dd-MM-yyyy"));
			P5sRpt.P5sCreateReport(sourcePath, resultPath,new int[] {2});
			//P5sEReport.P5sASPExportFileToClient(resultPath, "excel");
            P5sCmmFns.P5sASPExportFileToClient(resultPath, nameFile);// xuat cai ten ngan







        }
	}
}