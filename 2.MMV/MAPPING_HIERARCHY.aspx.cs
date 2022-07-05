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
using System.Diagnostics;
using System.IO;
using P5sCmm;
using P5sDmComm;
using System.Drawing;
using iTextSharp.text;

namespace MMV
{
    public partial class MAPPING_HIERARCHY : System.Web.UI.Page
    {

        public L5sForm P5sUnMapFrm;
        public L5sPaging P5sUnMapPaging;
        public L5sTextboxFilter P5sTxtFtrUnMap;

        public L5sForm P5sMapFrm;
        public L5sPaging P5sMapPaging;
        public L5sTextboxFilter P5sTxtFtrMap;

        String P5sSqlSelectUnMap = "";
        String P5sSqlSelectMap = "";
        
        public L5sAutocomplete P5sActREGION;
        public L5sAutocomplete P5sActAREA;
        public L5sAutocomplete P5sActDISTRIBUTOR;
        public L5sAutocomplete P5sActSALES;
        public L5sAutocomplete P5sActROUTE;

        public L5sAutocomplete P5sActPROVINCE;
        public L5sAutocomplete P5sActDISTRICT;
        public L5sAutocomplete P5sActCOMMUNE;

        String P5sSqlInsert = "";
        String P5sSqlUpdate = "";
        String P5sSqlDelete = "";
        String P5sSqlOrderBy = " cust.CUSTOMER_CODE,cust.CUSTOMER_NAME, cust.ACTIVE ";
        
        String[] P5sControlNameInitNotInitGridView = new String[] { "P5sLbtnUnMap","P5sLbtnMap" ,"P5sLbtnEditAddress" ,"P5sLbtnConfirmPopup" ,"P5sLbtnExport" ,"P5sLbtnFindToHierarchy" ,"P5sLbtnFindToMap"};

        String[] P5sControlNameResetGridView = new String[] { "P5sDdlREGION_CD", "P5sDdlAREA_CD" };

        String[] P5sControlNameNotInitAutComplete = new String[] { "P5sDdlREGION_CD", "P5sDdlAREA_CD" };

         Stopwatch st = new Stopwatch();
             
        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
           // st.Start();

            if (!IsPostBack)
            {
                this.P5sLoadDropDownListREGION();
                this.P5sLoadDropDownListAREA(this.P5sDdlREGION_CD.SelectedValue);
            }
            String postbackControlID = P5sCmmFns.P5sGetPostBackControlId(this);

            //clear textbox filter
            if (postbackControlID == "P5sLbtnFindToMap" )
            {
                this.P5sFilterTxt.Text = "";
            }
            else
                if (postbackControlID == "P5sLbtnFindToHierarchy")
                {
                    this.P5sFilterTxtHierarchyVN.Text = "";
                }

            if (Array.IndexOf(P5sControlNameInitNotInitGridView, postbackControlID) == -1)
            {
                String sales_CD = this.P5sTxtSALES_CD.Text;
                String route_CD = this.P5sTxtROUTE_CD.Text;

                String province_CD = this.P5sTxtPROVINCE_CD.Text;
                String district_CD = this.P5sTxtDISTRICT_CD.Text;
                String commune_CD = this.P5sTxtCOMMUNE_CD.Text;

             
                if (Array.IndexOf(P5sControlNameResetGridView, postbackControlID) != -1)
                {
                    sales_CD = route_CD = province_CD = district_CD = commune_CD = "-1";
                }
                
                this.P5sUnMapGridViewInit(sales_CD, route_CD);
                this.P5sMapGridViewInit(province_CD, district_CD, commune_CD);     
            }

            if (Array.IndexOf(P5sControlNameNotInitAutComplete, postbackControlID) == -1)                         
                 this.P5sInit();

            this.P5sLbtnMap.Attributes.Add("onclick", "return P5sCountGridViewMapRowsCheck('" + this.P5sUnMapGridView.ClientID + "','" + this.P5sTxtCOMMUNE_CD.ClientID +"')");
            this.P5sLbtnUnMap.Attributes.Add("onclick", "return P5sCountGridViewUnMapRowsCheck('" + this.P5sMapGridView.ClientID + "','" + this.P5sTxtCOMMUNE_CD.ClientID + "')");
        }

      
        private void P5sInit()
        {
            this.P5sAutoCompleteInit();
        }

        private void P5sLoadDropDownListREGION()
        {
            String sqlLoadDropDownListRegion = @"SELECT -1 AS REGION_CD ,'--Select Region--' AS REGION_CODE  
                                UNION ALL
                               SELECT DISTINCT reg.REGION_CD, reg.REGION_CODE 
                                FROM M_REGION reg INNER JOIN M_AREA are ON reg.REGION_CD  = are.REGION_CD AND are.ACTIVE = 1
	                                 INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                WHERE reg.ACTIVE = 1 AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										  INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1
								 )  ORDER BY REGION_CD ";

            DataTable dtLoadRegion = L5sSql.Query(sqlLoadDropDownListRegion);
            this.P5sDdlREGION_CD.DataSource = dtLoadRegion;
            this.P5sDdlREGION_CD.DataValueField = "REGION_CD";
            this.P5sDdlREGION_CD.DataTextField = "REGION_CODE";           
            this.P5sDdlREGION_CD.DataBind();
            if (dtLoadRegion.Rows.Count == 2)
            {      
                this.P5sDdlREGION_CD.SelectedIndex = 1;
            }
        }

        private void P5sLoadDropDownListAREA(String RegionCD)
        {
            RegionCD = RegionCD == "" ? "-1" : RegionCD;
            String sql = @"SELECT -1 AS AREA_CD ,'--Select Area--' AS AREA_CODE , 0 AS AREA_ORDER 
                          UNION ALL 
                        
                          SELECT DISTINCT are.AREA_CD, are.AREA_CODE   ,are.AREA_ORDER
                                FROM  M_AREA are  INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                WHERE are.ACTIVE = 1 AND are.REGION_CD = @0 AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										  INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1
								 )  
								 ORDER BY AREA_CD ASC";
            DataTable dt = L5sSql.Query(sql, RegionCD);
            this.P5sDdlAREA_CD.DataSource = dt;
            this.P5sDdlAREA_CD.DataValueField = "AREA_CD";
            this.P5sDdlAREA_CD.DataTextField = "AREA_CODE";          
            this.P5sDdlAREA_CD.DataBind();
            if (dt.Rows.Count == 2)
            {
                this.P5sDdlAREA_CD.SelectedIndex = 1;
            }

        }

        private void P5sAutoCompleteInit()
        {
            //Load DISTRIBUTOR list to texbox P5sTxtDISTRIBUTOR_CD 
            String regionCD = this.P5sDdlREGION_CD.SelectedValue;
            String areaCD = this.P5sDdlAREA_CD.SelectedValue;
            this.P5sActDISTRIBUTOR = this.P5sActDISTRIBUTOR == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistributorAllType("AREA_CD", areaCD), this.P5sTxtDISTRIBUTOR_CD.ClientID, 1, true, this.P5sTxtSALES_CD.ClientID) : this.P5sActDISTRIBUTOR;
            
            //Load Sales list to texbox P5sTxtSALES_CD  
            areaCD = areaCD == "" ? "-1" : areaCD;
            String sqlDistributor = String.Format(@"SELECT DISTINCT DISTRIBUTOR_CD 
                                                    FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                                                     INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
	                                                     INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                                                     INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                    WHERE arePro.AREA_CD = {0} AND dis.ACTIVE = 1", areaCD);

            String distributorCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlDistributor);
            this.P5sActSALES = this.P5sActSALES == null ? new L5sAutocomplete(P5sCmmFns.P5sGetSales("DISTRIBUTOR_CD", distributorCDs), this.P5sTxtSALES_CD.ClientID, 1, true, this.P5sTxtROUTE_CD.ClientID) : this.P5sActSALES;
            this.P5sActSALES.L5sChangeFilteringId(this.P5sTxtDISTRIBUTOR_CD.ClientID);
            
            //Load Route list to texbox P5sTxtRoute_CD 
            String sqlSales = String.Format("SELECT SALES_CD FROM M_SALES WHERE DISTRIBUTOR_CD IN ({0}) AND ACTIVE = 1 ", distributorCDs);
            String salesCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlSales);          
            this.P5sActROUTE = this.P5sActROUTE == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRoute("SALES_CD", salesCDs), this.P5sTxtROUTE_CD.ClientID, 0, true) : this.P5sActROUTE;
            this.P5sActROUTE.L5sChangeFilteringId(this.P5sTxtSALES_CD.ClientID);

            // Lấy Database Mapping cả nước
            string sqlAllowMapping = @"SELECT * FROM S_PARAMS where NAME ='REGION_CD_ALLOW_MAPPING'";
            DataTable dtAllowMapping = L5sSql.Query(sqlAllowMapping);
            string abc = dtAllowMapping.Rows[0]["VALUE"].ToString();
            string[] arrRegionCD = abc.Split(',');
            string value = Array.Find(arrRegionCD, element => element == regionCD);
            
            if (value == null )
            // Region chỉ thể mapping theo khu vực (area)
            {
                this.P5sActPROVINCE = this.P5sActPROVINCE == null ? new L5sAutocomplete(P5sCmmFns.P5sGetProvinceByArea(areaCD), this.P5sTxtPROVINCE_CD.ClientID, 1, true, this.P5sTxtDISTRICT_CD.ClientID) : this.P5sActPROVINCE;

                //Load district 
                String sqlProvince = String.Format(@"SELECT p.PROVINCE_CD 
                                                FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD AND a.ACTIVE = 1
                                                WHERE a.AREA_CD IN ({0})  AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										                      INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										                      INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									                    WHERE pro.PROVINCE_CD = a.PROVINCE_CD AND dis.ACTIVE = 1 )
								             ", areaCD);
                String provinceCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlProvince);
                this.P5sActDISTRICT = this.P5sActDISTRICT == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistrict("PROVINCE_CD", provinceCDs), this.P5sTxtDISTRICT_CD.ClientID, 0, true, this.P5sTxtCOMMUNE_CD.ClientID) : this.P5sActDISTRICT;
                this.P5sActDISTRICT.L5sChangeFilteringId(this.P5sTxtPROVINCE_CD.ClientID);

                //Load COMMUNE 
                String sqlDistrict = String.Format(@"SELECT DISTRICT_CD FROM  M_DISTRICT WHERE PROVINCE_CD IN ({0}) ", provinceCDs);
                String districtCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlDistrict);
                this.P5sActCOMMUNE = this.P5sActCOMMUNE == null ? new L5sAutocomplete(P5sCmmFns.P5sGetCommune("DISTRICT_CD", districtCDs), this.P5sTxtCOMMUNE_CD.ClientID, 0, true) : this.P5sActCOMMUNE;
                this.P5sActCOMMUNE.L5sChangeFilteringId(this.P5sTxtDISTRICT_CD.ClientID);
            }
            else
            // Region có thể mapping cả nước
            {
                this.P5sActPROVINCE = this.P5sActPROVINCE == null ? new L5sAutocomplete(P5sCmmFns.P5sGetProvinceByNotArea(""), this.P5sTxtPROVINCE_CD.ClientID, 1, true, this.P5sTxtDISTRICT_CD.ClientID) : this.P5sActPROVINCE;

                //Load district 
                String sqlProvince = String.Format(@"SELECT p.PROVINCE_CD 
                                                FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD AND a.ACTIVE = 1
                                                WHERE NOT a.AREA_CD IN ({0})  AND EXISTS ( SELECT * FROM
                                            M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
                                INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
                                INNER JOIN [M_DISTRIBUTOR.] dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
                             WHERE pro.PROVINCE_CD = a.PROVINCE_CD AND dis.ACTIVE = 1 )", areaCD);
                String provinceCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlProvince);
                this.P5sActDISTRICT = this.P5sActDISTRICT == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistrict("PROVINCE_CD", provinceCDs), this.P5sTxtDISTRICT_CD.ClientID, 1, true, this.P5sTxtCOMMUNE_CD.ClientID) : this.P5sActDISTRICT;
                this.P5sActDISTRICT.L5sChangeFilteringId(this.P5sTxtPROVINCE_CD.ClientID);
                //Load COMMUNE 
                String sqlDistrict = String.Format(@"SELECT DISTRICT_CD FROM  M_DISTRICT WHERE PROVINCE_CD IN ({0}) ", provinceCDs);
                String districtCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlDistrict);
                this.P5sActCOMMUNE = this.P5sActCOMMUNE == null ? new L5sAutocomplete(P5sCmmFns.P5sGetCommune("DISTRICT_CD", districtCDs), this.P5sTxtCOMMUNE_CD.ClientID, 1, true) : this.P5sActCOMMUNE;
                this.P5sActCOMMUNE.L5sChangeFilteringId(this.P5sTxtDISTRICT_CD.ClientID);
            }
        }

        public void P5sShowFormOnStatus(Int32 P5sFormStatus, Boolean P5sIsLoaded)
        {
            switch (P5sFormStatus)
            {
                case 0://read only  
                    break;
                case 1: //new  
                    break;
                case 2: //Edit 
                    break;
                default:
                    break;
            }
        }

        private void P5sUnMapGridViewInit(String sales_CD, String route_CD)
        {

            if (sales_CD.Trim().Length <= 0)
                sales_CD = "-1";

            if (route_CD.Trim().Length <= 0)
                route_CD = "-1";

            this.P5sSqlSelectUnMap = String.Format(@"SELECT cust.CUSTOMER_CD,cust.CUSTOMER_CODE,cust.CUSTOMER_NAME, cust.CUSTOMER_ADDRESS, cust.ACTIVE ,
            d.DISTRIBUTOR_CODE + '-' + d.DISTRIBUTOR_NAME AS DISTRIBUTOR_DESC,
            sls.SALES_CODE + '-' + sls.SALES_NAME AS SALES_DESC,
            rout.ROUTE_CODE  + '-' + rout.ROUTE_NAME ROUTE_NAME,
            cust.CUSTOMER_CHAIN_CODE, cust.DISTRIBUTOR_CD,sls.SALES_CD, rout.ROUTE_CD
            FROM M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE custR on cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
            INNER JOIN O_SALES_ROUTE slsR ON custR.ROUTE_CD = slsR.ROUTE_CD AND slsR.ACTIVE = 1
            INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD
            INNER JOIN M_SALES sls ON slsR.SALES_CD = sls.SALES_CD
            INNER JOIN M_DISTRIBUTOR d on sls.DISTRIBUTOR_CD = d.DISTRIBUTOR_CD AND d.ACTIVE = 1
            WHERE   cust.ACTIVE = 1 AND rout.ROUTE_CD IN ({0})   and (cust.COMMUNE_CD IS NULL or cust.COMMUNE_CD = 0) ", route_CD);


            this.P5sUnMapFrm = new L5sForm(this.P5sUnMapGridView, false, "CUSTOMER_CD", new Panel(), "P5sShowFormOnStatus",
                          this.P5sSqlInsert,
                          this.P5sSqlUpdate,
                          this.P5sSqlDelete,
                          new String[] { },
                          new Int32[]  { });


           
            this.P5sUnMapPaging = new L5sPaging(this.P5sUnMapGridView, 1000, this.P5sDdlUnMapPaging);
            this.P5sTxtFtrUnMap = new L5sTextboxFilter(new Panel(), this.P5sUnMapGridView,
                                        new String[] { "cust.CUSTOMER_ADDRESS" }, this.P5sFilterTxt);


            if (this.P5sTxtFtrUnMap.L5sIsClicked() && this.P5sFilterTxt.Text != "")
                this.P5sTxtFtrUnMap.L5sLoadFilter(this.P5sUnMapPaging, this.P5sSqlSelectUnMap, this.P5sSqlOrderBy, "");
            else
                this.P5sUnMapFrm.L5sLoadPage(this.P5sUnMapPaging.L5sParseSql(this.P5sSqlSelectUnMap, this.P5sSqlOrderBy));

            this.P5sUnMapPaging.L5sLoadPaging();

            //Hide P5sUpanelMainGV if this empty
            this.P5sPnlUnMapGV.Visible = this.P5sUnMapGridView.Rows.Count > 0 ? true : false;

     
        }


        private void P5sMapGridViewInit( String province_CD, String district_CD, String commune_CD)
        {
            if (province_CD.Trim().Length <= 0)
                province_CD = "-1";

            if (district_CD.Trim().Length <= 0)
                district_CD = "-1";

            if (commune_CD.Trim().Length <= 0)
                commune_CD = "-1";


            P5sSqlSelectMap = String.Format(@"SELECT  cust.CUSTOMER_CD,cust.CUSTOMER_CODE,cust.CUSTOMER_NAME, cust.CUSTOMER_ADDRESS, cust.ACTIVE ,d.DISTRIBUTOR_CODE + '-' + d.DISTRIBUTOR_NAME AS DISTRIBUTOR_DESC,
                                                sls.SALES_CODE + '-' + sls.SALES_NAME AS SALES_DESC,
                                                rout.ROUTE_CODE  + '-' + rout.ROUTE_NAME ROUTE_NAME,
                                                cust.CUSTOMER_CHAIN_CODE
                                                , cust.DISTRIBUTOR_CD,sls.SALES_CD, rout.ROUTE_CD
                                                FROM M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE custR on cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
                                                INNER JOIN O_SALES_ROUTE slsR ON custR.ROUTE_CD = slsR.ROUTE_CD AND slsR.ACTIVE = 1
                                                INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD
                                                INNER JOIN M_SALES sls ON slsR.SALES_CD = sls.SALES_CD
                                                INNER JOIN M_DISTRIBUTOR d on sls.DISTRIBUTOR_CD = d.DISTRIBUTOR_CD AND d.ACTIVE = 1
                                                WHERE   cust.ACTIVE = 1 AND cust.COMMUNE_CD in({0})", commune_CD);

            


            this.P5sMapFrm = new L5sForm(this.P5sMapGridView, false, "CUSTOMER_CD", new Panel(), "P5sShowFormOnStatus",
                          this.P5sSqlInsert,
                          this.P5sSqlUpdate,
                          this.P5sSqlDelete,
                          new String[] { },
                          new Int32[] { });

            
            this.P5sMapPaging = new L5sPaging(this.P5sMapGridView, 1000, this.P5sDdlMapPaging);


            this.P5sTxtFtrMap = new L5sTextboxFilter(new Panel(), this.P5sMapGridView,
                                        new String[] { "cust.CUSTOMER_ADDRESS" }, this.P5sFilterTxtHierarchyVN);

            if (this.P5sTxtFtrMap.L5sIsClicked() && this.P5sFilterTxtHierarchyVN.Text != "")
                this.P5sTxtFtrMap.L5sLoadFilter(this.P5sMapPaging, this.P5sSqlSelectMap, this.P5sSqlOrderBy, "");
            else
                this.P5sMapFrm.L5sLoadPage(this.P5sMapPaging.L5sParseSql(this.P5sSqlSelectMap, this.P5sSqlOrderBy));

            this.P5sMapPaging.L5sLoadPaging();

            //Hide P5sUpanelMainGV if this empty
            this.P5sPnlMapGV.Visible = this.P5sMapGridView.Rows.Count > 0 ? true : false;

        
        }

        protected void P5sSearch_Click(object sender, EventArgs e)
        {

            this.P5sFilterTxt.Text = this.P5sFilterTxt.Text.Trim();
            //this.P5sTxtFtrMap.L5sFilterButton_Click(this.P5sMapPaging, this.P5sSqlSelectMap, this.P5sSqlOrderBy, "");
            //this.P5sMapPaging.L5sLoadPaging();

            this.P5sTxtFtrUnMap.L5sFilterButton_Click(this.P5sUnMapPaging, this.P5sSqlSelectUnMap, this.P5sSqlOrderBy, "");
            this.P5sUnMapPaging.L5sLoadPaging();
            
        }

        protected void P5sLbtnNew_Click(object sender, EventArgs e)
        {

        }

        public Boolean P5sIsValidVNHierarchy()
        {

            String province_CD = this.P5sTxtPROVINCE_CD.Text.ToString();
            String district_CD = this.P5sTxtDISTRICT_CD.Text.ToString();
            String commune_CD = this.P5sTxtCOMMUNE_CD.Text.ToString();

            if (province_CD.Trim().Length <= 0)
            {
                L5sMsg.Show("Tỉnh / Thành Phố bắt buộc chọn !.");
                return false;
            }

            if (district_CD.Trim().Length <= 0)
            {
                L5sMsg.Show(" Quận/ Thị Xã/ Huyện bắt buộc chọn !.");
                return false;
            }

            if (commune_CD.Trim().Length <= 0)
            {
                L5sMsg.Show(" Phường / Xã/ Thị Trấn Huyện bắt buộc chọn !.");
                return false;
            }

            return true;
        }

        public Boolean P5sIsValidSalesHierarchy()
        {

            String region_CD = this.P5sDdlREGION_CD.SelectedValue.ToString();
            String area_CD = this.P5sDdlREGION_CD.SelectedValue.ToString();
            String distributor_CD = this.P5sTxtDISTRIBUTOR_CD.Text.ToString();
            String sales_CD = this.P5sTxtSALES_CD.Text.ToString();
            String route_CD = this.P5sTxtROUTE_CD.Text.ToString();




            if (region_CD.Trim() == "-1" )
            {
                L5sMsg.Show("Vùng bắt buộc chọn !.");
                return false;
            }

            if (area_CD.Trim() == "-1" )
            {
                L5sMsg.Show("Khu vực bắt buộc chọn !.");
                return false;
            }
            
            if (distributor_CD.Trim().Length <= 0)
            {
                L5sMsg.Show("Nhà phân phối bắt buộc chọn !.");
                return false;
            }
             
            if (sales_CD.Trim().Length <= 0)
            {
                L5sMsg.Show("DSP bắt buộc chọn !.");
                return false;
            }

             if (route_CD.Trim().Length <= 0)
            {
                L5sMsg.Show("Tuyến BH bắt buộc chọn !.");
                return false;
            }

           
            return true;
        }

        protected void P5sLbtnFindToMap_Click(object sender, EventArgs e)
        {

            if (!this.P5sIsValidSalesHierarchy())
                return;


            String sales_CD = this.P5sTxtSALES_CD.Text.ToString();
            String route_CD = this.P5sTxtROUTE_CD.Text.ToString();
            this.P5sUnMapGridViewInit(sales_CD, route_CD);

            }

        protected void P5sLbtnMap_Click(object sender, EventArgs e)
        {

            if (!this.P5sIsValidSalesHierarchy())
                return;


            if (!this.P5sIsValidVNHierarchy())
                return;


            if (!this.P5sCheckSelect(this.P5sUnMapGridView))
            {
                L5sMsg.Show("Không có khách hàng nào được chọn !.");
                return;
            }


            String region = this.P5sDdlREGION_CD.SelectedValue;
            String area = this.P5sDdlAREA_CD.SelectedValue;



            String sales_CD = this.P5sTxtSALES_CD.Text.ToString();
            String route_CD = this.P5sTxtROUTE_CD.Text.ToString();

            String province_CD = this.P5sTxtPROVINCE_CD.Text.ToString();
            String district_CD = this.P5sTxtDISTRICT_CD.Text.ToString();
            String commune_CD = this.P5sTxtCOMMUNE_CD.Text.ToString();
            

            String sqlUpdate = "UPDATE M_CUSTOMER SET PROVINCE_CD = @1, DISTRICT_CD = @2, COMMUNE_CD =@3 WHERE CUSTOMER_CD = @0 AND PROVINCE_CD IS NULL AND DISTRICT_CD IS NULL AND COMMUNE_CD IS NULL";

            foreach (GridViewRow row in this.P5sUnMapGridView.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("P5sChk");
                if (cb != null && cb.Checked)
                { 
                    String customer_CD = this.P5sUnMapGridView.DataKeys[row.RowIndex].Value.ToString();
                    String distributor = row.Cells[8].Text;
                    String dsp = row.Cells[9].Text;                   
                    String route = row.Cells[10].Text;
                    L5sSql.Execute(sqlUpdate, customer_CD, province_CD, district_CD, commune_CD);                
                }
            }
            this.P5sFilterTxt.Text = "";
            this.P5sUnMapGridViewInit(sales_CD, route_CD);
            this.P5sMapGridViewInit(province_CD, district_CD, commune_CD);
            this.P5sUpdateDataBase(); // update lại thông tin sau khi map
        }

        private void P5sUpdateDataBase()
        {
            P5sCmm.P5sCmmFns.P5sUpdateValueForMap();
            P5sCmm.P5sCmmFns.P5sUpdateRadius(this.P5sTxtCOMMUNE_CD.Text.ToString());
        }

     


        private Boolean P5sCheckSelect(GridView gridView)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("P5sChk");
                if (cb != null && cb.Checked)              
                    return true;                
            }
            return false;
        }

        protected void P5sLbtnUnMap_Click(object sender, EventArgs e)
        {
            
            if (!this.P5sIsValidVNHierarchy())
                return;


            if (!this.P5sCheckSelect(this.P5sMapGridView))
            {
                L5sMsg.Show("Không có khách hàng nào được chọn !.");
                return;
            }


            String sales_CD = this.P5sTxtSALES_CD.Text.ToString();
            String route_CD = this.P5sTxtROUTE_CD.Text.ToString();

            String province_CD = this.P5sTxtPROVINCE_CD.Text.ToString();
            String district_CD = this.P5sTxtDISTRICT_CD.Text.ToString();
            String commune_CD = this.P5sTxtCOMMUNE_CD.Text.ToString();


            String sqlUpdate = "UPDATE M_CUSTOMER SET PROVINCE_CD = NULL, DISTRICT_CD = NULL, COMMUNE_CD = NULL WHERE CUSTOMER_CD = @0 ";
            foreach (GridViewRow row in this.P5sMapGridView.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("P5sChk");
                if (cb != null && cb.Checked)
                {
                    String customer_CD = this.P5sMapGridView.DataKeys[row.RowIndex].Value.ToString();                  
                    L5sSql.Execute(sqlUpdate, customer_CD);
                }
            }
            this.P5sFilterTxtHierarchyVN.Text = "";
            this.P5sUnMapGridViewInit(sales_CD, route_CD);
            this.P5sMapGridViewInit( province_CD, district_CD, commune_CD);
            this.P5sUpdateDataBase(); // update lại thông tin sau khi unmap
        }

       

        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {

            try
            {

                this.P5sLblMapTitle.Text = "Danh sách khách hàng .Tổng số : " + this.P5sMapGridView.Rows.Count.ToString() + " /Trang";
                this.P5sLblUnMapTitle.Text = "DSKH chưa chọn Phường/Xã .Tổng số : " + this.P5sUnMapGridView.Rows.Count.ToString();

                this.P5sLblEmptyRowUnMapGV.Visible = !this.P5sUnMapGridView.Visible;
                this.P5sLblEmptyRowMapGV.Visible = !this.P5sMapGridView.Visible;

                this.P5sActDISTRIBUTOR.L5sSetDefaultValues(this.P5sTxtDISTRIBUTOR_CD.Text);
                this.P5sActSALES.L5sSetDefaultValues(this.P5sTxtSALES_CD.Text);
                this.P5sActROUTE.L5sSetDefaultValues(this.P5sTxtROUTE_CD.Text);
                this.P5sActPROVINCE.L5sSetDefaultValues(this.P5sTxtPROVINCE_CD.Text);
                this.P5sActDISTRICT.L5sSetDefaultValues(this.P5sTxtDISTRICT_CD.Text);
                this.P5sActCOMMUNE.L5sSetDefaultValues(this.P5sTxtCOMMUNE_CD.Text);

                this.P5sLbtnMap.Visible = this.P5sUnMapGridView.Rows.Count > 0 ? true : false;
                this.P5sLbtnUnMap.Visible = this.P5sMapGridView.Rows.Count > 0 ? true : false;
               
            }
            catch (Exception)
            {

            }

            //st.Stop();

            //String sql = "INSERT INTO TMP_FOR_TEST (ROOT,CHILD,ELAPSED,ELAPSED_MILISECONDS) VALUES (@0,@1,@2,@3)";
            //L5sSql.Execute(sql, this.P5sDdlREGION_CD.Items[this.P5sDdlREGION_CD.SelectedIndex].Text, this.P5sDdlAREA_CD.Items[this.P5sDdlAREA_CD.SelectedIndex].Text, st.Elapsed.ToString(), st.ElapsedMilliseconds.ToString());


            //using (StreamWriter _testData = new StreamWriter(Server.MapPath("~/data.txt"), true))
            //{
            //    _testData.WriteLine(this.P5sDdlREGION_CD.Items[this.P5sDdlREGION_CD.SelectedIndex].Text + "-->" + this.P5sDdlAREA_CD.Items[this.P5sDdlAREA_CD.SelectedIndex].Text + ":" + st.Elapsed.ToString() + " ==  " + st.ElapsedMilliseconds.ToString()); // Write the file.
            //}         

        }

        protected void P5sUnMapGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow ||
           e.Row.RowType == DataControlRowType.Header ||
           e.Row.RowType == DataControlRowType.Footer)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 5px;");

                }
                e.Row.Cells[0].Width = Unit.Percentage(1);
                e.Row.Cells[1].Width = Unit.Percentage(1);
                e.Row.Cells[2].Width = Unit.Percentage(1);
                e.Row.Cells[3].Width = Unit.Percentage(1);
                e.Row.Cells[4].Width = Unit.Percentage(1);
                e.Row.Cells[7].Width = Unit.Percentage(1);


                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
              
            }
        }

        protected void P5sMapGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow ||
           e.Row.RowType == DataControlRowType.Header ||
           e.Row.RowType == DataControlRowType.Footer)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 10px;");

                }
                e.Row.Cells[0].Width = Unit.Percentage(1);
                e.Row.Cells[1].Width = Unit.Percentage(1);
                e.Row.Cells[2].Width = Unit.Percentage(1);
                e.Row.Cells[3].Width = Unit.Percentage(1);
                e.Row.Cells[4].Width = Unit.Percentage(1);
                e.Row.Cells[6].Width = Unit.Percentage(1);
               
            }

        }

        protected void P5sDdlREGION_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            String regionCD = this.P5sDdlREGION_CD.SelectedValue;
            this.P5sLoadDropDownListAREA(regionCD);
            this.P5sInit();
            this.P5sFilterTxt.Text = this.P5sFilterTxtHierarchyVN.Text = "";
        }

        protected void P5sDdlAREA_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.P5sInit();
            this.P5sFilterTxt.Text = this.P5sFilterTxtHierarchyVN.Text = "";
        }

        protected void P5sLbtnFindToHierarchy_Click(object sender, EventArgs e)
        {
            if (!this.P5sIsValidVNHierarchy())
                return;


            String province_CD = this.P5sTxtPROVINCE_CD.Text.ToString();
            String district_CD = this.P5sTxtDISTRICT_CD.Text.ToString();
            String commune_CD = this.P5sTxtCOMMUNE_CD.Text.ToString();
            this.P5sMapGridViewInit(province_CD, district_CD, commune_CD);

        }

        protected void P5sUnMapGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "EditAddress":

                    try
                    {
                        ViewState["CUSTOMER_CD"] = ((GridView)sender).DataKeys[Int32.Parse(e.CommandArgument.ToString())].Value;

                        Panel pnl = ((GridView)sender).Rows[Int32.Parse(e.CommandArgument.ToString())].Cells[7].FindControl("P5sPnlTabControl") as Panel;
                        Label customerCODE = pnl.Controls[2].FindControl("P5sLblCUSTOMER_CODE") as Label;
                        customerCODE.Text = ((GridView)sender).Rows[Int32.Parse(e.CommandArgument.ToString())].Cells[1].Text.ToString();

                        Label customerNAME = pnl.Controls[2].FindControl("P5sLblCUSTOMER_NAME") as Label;
                        customerNAME.Text = ((GridView)sender).Rows[Int32.Parse(e.CommandArgument.ToString())].Cells[2].Text.ToString();

                        Label routeDESC = pnl.Controls[2].FindControl("P5sLblROUTE_NAME") as Label;
                        routeDESC.Text = ((GridView)sender).Rows[Int32.Parse(e.CommandArgument.ToString())].Cells[3].Text.ToString();


                        TextBox customerAddress = pnl.Controls[2].FindControl("P5sTxtCUSTOMER_ADDRESS") as TextBox;
                        customerAddress.Text = ((GridView)sender).Rows[Int32.Parse(e.CommandArgument.ToString())].Cells[5].Text.ToString();


                        AjaxControlToolkit.ModalPopupExtender modal = ((GridView)sender).Rows[Int32.Parse(e.CommandArgument.ToString())].Cells[7].FindControl("P5sModelPopupTabControl") as AjaxControlToolkit.ModalPopupExtender;
                        modal.Show();  
                    }
                    catch (Exception)
                    {
                        
                    }
                    

                    break;
                default:
                    break;
            }
          

        }

        protected void P5sUnMapGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
                      
        }

        protected void P5sLbtnConfirmPopup_Click(object sender, EventArgs e)
        {
            if (ViewState["CUSTOMER_CD"] == null)
                return;
            
            Panel parent = ((LinkButton)sender).Parent as Panel;

            TextBox customerAddress = parent.Controls[2].FindControl("P5sTxtCUSTOMER_ADDRESS") as TextBox;


            if (customerAddress.Text.Trim() == "")
            {
                L5sMsg.Show("Địa chỉ bắt buộc nhập !.");
                AjaxControlToolkit.ModalPopupExtender modal = parent.Parent.FindControl("P5sModelPopupTabControl") as AjaxControlToolkit.ModalPopupExtender;
                modal.Show();
                return;
            }


            String sql = "UPDATE M_CUSTOMER SET CUSTOMER_ADDRESS = @1 WHERE CUSTOMER_CD = @0";
            L5sSql.Execute(sql, ViewState["CUSTOMER_CD"].ToString(), customerAddress.Text.Trim());
            L5sMsg.Show("Cập nhật địa chỉ thành công !.");


            String sales_CD = this.P5sTxtSALES_CD.Text.ToString();
            String route_CD = this.P5sTxtROUTE_CD.Text.ToString();

            String province_CD = this.P5sTxtPROVINCE_CD.Text.ToString();
            String district_CD = this.P5sTxtDISTRICT_CD.Text.ToString();
            String commune_CD = this.P5sTxtCOMMUNE_CD.Text.ToString();

            this.P5sUnMapGridViewInit(sales_CD, route_CD);
            this.P5sMapGridViewInit(province_CD, district_CD, commune_CD);

        }

        protected void P5sLbtnExport_Click(object sender, EventArgs e)
        {
            String sql = @"DECLARE @TMP TABLE (
	PROVINCE_CD BIGINT,
	AREA_CD BIGINT
)

INSERT INTO @TMP select distinct PROVINCE_CD,AREA_CD from M_AREA_PROVINCE 
				where (select COUNT(*) from M_AREA_PROVINCE A where A.PROVINCE_CD = M_AREA_PROVINCE.PROVINCE_CD AND A.ACTIVE = 1) >= 2



SELECT T.REGION_CODE, T.AREA_CODE, T.PROVINCE_CODE, T.PROVINCE_NAME_EN,T.DISTRICT_CODE, T.DISTRICT_NAME_EN, T.COMMUNE_CODE,T.COMMUNE_NAME_EN  , SUM(T.FLAG) AS TongSoKH ,T.TYPE,T.ACN
		FROM 
			(
			SELECT r.REGION_CODE, a.AREA_CODE, p.PROVINCE_CODE, p.PROVINCE_NAME_EN,d.DISTRICT_CODE, d.DISTRICT_NAME_EN, m.COMMUNE_CODE,m.COMMUNE_NAME_EN ,  CASE WHEN c.CUSTOMER_CD  IS NOT NULL THEN  1 ELSE 0  END  FLAG 
						,case
								when m.COMMUNE_TYPE = 1 then 'Ward'
								when m.COMMUNE_TYPE = 2 then 'Town'
								when m.COMMUNE_TYPE = 3 then 'Commune'
						 end as TYPE 
						 ,m.ACN
			FROM  
					M_REGION r INNER JOIN M_AREA a ON r.REGION_CD = a.REGION_CD
					INNER JOIN M_AREA_PROVINCE ap ON a.AREA_CD = ap.AREA_CD
					INNER JOIN (SELECT * FROM M_PROVINCE pr WHERE pr.PROVINCE_CD NOT IN (SELECT PROVINCE_CD FROM @TMP) ) p ON ap.PROVINCE_CD = p.PROVINCE_CD
					INNER JOIN M_DISTRICT d ON ap.PROVINCE_CD = d.PROVINCE_CD
					INNER JOIN M_COMMUNE m ON d.DISTRICT_CD = m.DISTRICT_CD
					LEFT JOIN M_CUSTOMER  c on m.COMMUNE_CD = c.COMMUNE_CD And C.ACTIVE = 1 
			) AS T
GROUP BY T.REGION_CODE, T.AREA_CODE, T.PROVINCE_CODE, T.PROVINCE_NAME_EN,T.DISTRICT_CODE, T.DISTRICT_NAME_EN, T.COMMUNE_CODE,T.COMMUNE_NAME_EN ,T.TYPE,T.ACN

UNION ALL

SELECT T.REGION_CODE, '' AS  AREA_CODE, T.PROVINCE_CODE, T.PROVINCE_NAME_EN,T.DISTRICT_CODE, T.DISTRICT_NAME_EN, T.COMMUNE_CODE,T.COMMUNE_NAME_EN  , SUM(T.FLAG) AS TongSoKH,T.TYPE ,T.ACN
		FROM 
			(
			SELECT r.REGION_CODE, a.AREA_CODE, p.PROVINCE_CODE, p.PROVINCE_NAME_EN,d.DISTRICT_CODE, d.DISTRICT_NAME_EN, m.COMMUNE_CODE,m.COMMUNE_NAME_EN ,  CASE WHEN c.CUSTOMER_CD  IS NOT NULL THEN  1 ELSE 0  END  FLAG 
					     ,case
								when m.COMMUNE_TYPE = 1 then 'Ward'
								when m.COMMUNE_TYPE = 2 then 'Town'
								when m.COMMUNE_TYPE = 3 then 'Commune'
						 end as TYPE ,

						 m.ACN
			FROM  
					M_REGION r INNER JOIN M_AREA a ON r.REGION_CD = a.REGION_CD
					INNER JOIN M_AREA_PROVINCE ap ON a.AREA_CD = ap.AREA_CD
					INNER JOIN (SELECT * FROM M_PROVINCE pr WHERE pr.PROVINCE_CD  IN (SELECT PROVINCE_CD FROM @TMP) ) p ON ap.PROVINCE_CD = p.PROVINCE_CD
					INNER JOIN M_DISTRICT d ON ap.PROVINCE_CD = d.PROVINCE_CD
					INNER JOIN M_COMMUNE m ON d.DISTRICT_CD = m.DISTRICT_CD
					LEFT JOIN M_CUSTOMER  c on m.COMMUNE_CD = c.COMMUNE_CD  And C.ACTIVE = 1             
           
			) AS T
GROUP BY T.REGION_CODE,  T.PROVINCE_CODE, T.PROVINCE_NAME_EN,T.DISTRICT_CODE, T.DISTRICT_NAME_EN, T.COMMUNE_CODE,T.COMMUNE_NAME_EN, T.TYPE,T.ACN
ORDER BY T.REGION_CODE,T.AREA_CODE,T.PROVINCE_CODE,T.DISTRICT_CODE, T.COMMUNE_CODE
  ";
            DataTable dt = L5sSql.Query(sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                L5sMsg.Show("Không có dữ liệu!");
                return;
            }
            P5sEReport P5sRpt = new P5sEReport();
            P5sRpt.P5sAdd("list", dt);

            string fileName = "MAPPING_SUMMARY_";
            String sourcePath = Server.MapPath("~/Template/MAPPING_SUMMARY_140311.xlsx");
            String resultPath = Server.MapPath("~/Exports/" + fileName + DateTime.Now.ToString("yyyyMMddhhmmssffffff") + ".xlsx");

            P5sRpt.P5sCreateReport(sourcePath, resultPath);
            P5sEReport.P5sASPExportFileToClient(resultPath, "excel");
        }

        protected void P5sBtnSearchHierarchyVN_Click(object sender, EventArgs e)
        {
            this.P5sFilterTxtHierarchyVN.Text = this.P5sFilterTxtHierarchyVN.Text.Trim();
            this.P5sTxtFtrMap.L5sFilterButton_Click(this.P5sMapPaging, this.P5sSqlSelectMap, this.P5sSqlOrderBy, "");
            this.P5sMapPaging.L5sLoadPaging();
        }

       




        protected void P5sLbtnExportCP_Click(object sender, EventArgs e)
        {

            String sql = @"SELECT TMP.REGION_CODE, TMP.AREA_CODE,TMP.DISTRIBUTOR_CODE,TMP.DISTRIBUTOR_NAME, TMP.DSP_CODE, TMP.DSP_NAME, TMP.ROUTE_CODE,
	                               COUNT(*) AS TOTAL_CUSTOMER, SUM(TMP.MAP) AS TOTAL_CUSTOMER_MAP,  COUNT(*) -  SUM(TMP.MAP) AS TOTAL_CUSTOMER_UNMAP

                            FROM
                            (
		                            SELECT REG.REGION_CODE, ARE.AREA_CODE,d.DISTRIBUTOR_CODE,d.DISTRIBUTOR_NAME, SLS.SALES_CODE AS DSP_CODE , SLS.SALES_NAME AS DSP_NAME , rout.ROUTE_CODE, CASE ISNULL(cust.COMMUNE_CD,'') WHEN '' THEN 0 ELSE 1  END  AS 'MAP'

		                             FROM 
                            		 
		                             M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE custR on cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
													                              INNER JOIN O_SALES_ROUTE slsR ON custR.ROUTE_CD = slsR.ROUTE_CD AND slsR.ACTIVE = 1
													                              INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD
													                              INNER JOIN M_SALES sls ON slsR.SALES_CD = sls.SALES_CD
                                                                                  INNER JOIN M_DISTRIBUTOR d on sls.DISTRIBUTOR_CD = d.DISTRIBUTOR_CD AND d.ACTIVE = 1
													                              INNER JOIN M_COMMUNE cmm ON d.COMMUNE_CD = cmm.COMMUNE_CD
													                              INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
													                              INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
													                              INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
													                              INNER JOIN M_AREA ARE ON arePro.AREA_CD = ARE.AREA_CD
													                              INNER JOIN M_REGION REG ON ARE.REGION_CD = REG.REGION_CD
                                    WHERE d.ACTIVE = 1 AND cust.ACTIVE = 1
                             ) AS TMP

                            GROUP BY TMP.REGION_CODE, TMP.AREA_CODE,TMP.DISTRIBUTOR_CODE,TMP.DISTRIBUTOR_NAME, TMP.DSP_CODE, TMP.DSP_NAME, TMP.ROUTE_CODE  
                            ORDER BY TMP.REGION_CODE, TMP.AREA_CODE,TMP.DISTRIBUTOR_CODE,TMP.DSP_CODE,  TMP.ROUTE_CODE    ";
                       
            DataTable dt = L5sSql.Query(sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                L5sMsg.Show("Không có dữ liệu!");
                return;
            }
            P5sEReport P5sRpt = new P5sEReport();
            P5sRpt.P5sAdd("list", dt);

            string fileName = "CP_MAPPING_SUMMARY_";
            String sourcePath = Server.MapPath("~/Template/CP_MAPPING_SUMMARY_140319.xlsx");
            String resultPath = Server.MapPath("~/Exports/" + fileName + DateTime.Now.ToString("yyyyMMddhhmmssffffff") + ".xlsx");

            P5sRpt.P5sCreateReport(sourcePath, resultPath);
            P5sEReport.P5sASPExportFileToClient(resultPath, "excel");

        }

        protected void P5sAddNewCustomer_Click(object sender, EventArgs e)
        {
            Response.Redirect("./MasterData/Customer.aspx");
        }


     
    }
}
