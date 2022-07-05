using System;
using System.Data;
using System.Configuration;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using L5sDmComm;
using P5sCmm;
using System.Collections.Generic;
using System.Drawing;
using P5sDmComm;
using System.Web.Script.Serialization;

namespace MMV.fsmdls.mmcv4
{
    public partial class DistributorNetwork : System.Web.UI.Page
    {
        L5sAutocomplete P5sActCountry;

        public L5sAutocomplete P5sActRegion, P5sActArea;
        public L5sAutocomplete P5sActDistributor;


        String[] P5sControlNameNotInitAutComplete = new String[] { "P5sDdlREGION_CD", "P5sDdlAREA_CD" };

        Dictionary<String, String> menuReports = new Dictionary<String, String>();

        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);

            DataTable dtLocationCountry = L5sSql.Query("SELECT NAME, VALUE FROM S_PARAMS WHERE NAME = 'LOCATION_COUNTRY'");
            if (dtLocationCountry != null && dtLocationCountry.Rows.Count > 0)
            {
                this.P5sLocaltionCountry.Value = dtLocationCountry.Rows[0]["VALUE"].ToString();
            }

            if (!IsPostBack)
            {
                this.P5sLoadDropDownListREGION();
                this.P5sLoadDropDownListAREA(this.P5sDdlREGION_CD.SelectedValue);
                this.P5sLoadDropDownListPROVINCE(this.P5sDdlAREA_CD.SelectedValue);
                this.P5sLoaDdlDistributorType();

            }


            String postbackControlID = P5sCmmFns.P5sGetPostBackControlId(this);

            if (Array.IndexOf(P5sControlNameNotInitAutComplete, postbackControlID) == -1)
                this.P5sInit();

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "$(document).ready(function(){loadMap();});", true);

        }

        private void P5sLoaDdlDistributorType()
        {
            String sql = @" SELECT 0 AS DISTRIBUTOR_TYPE_CD ,'All' AS DISTRIBUTOR_TYPE_CODE 
                            UNION ALL 
                            SELECT DISTRIBUTOR_TYPE_CD,DISTRIBUTOR_TYPE_CODE
                            FROM M_DISTRIBUTOR_TYPE WHERE ACTIVE = 1";

            DataTable dt = L5sSql.Query(sql);
            this.P5sDdlDistributorType.DataSource = dt;
            this.P5sDdlDistributorType.DataValueField = "DISTRIBUTOR_TYPE_CD";
            this.P5sDdlDistributorType.DataTextField = "DISTRIBUTOR_TYPE_CODE";
            this.P5sDdlDistributorType.SelectedIndex = 1;
            this.P5sDdlDistributorType.DataBind();

        }

        private void P5sLoadDropDownListREGION()
        {

            String sql = @"SELECT -1 AS REGION_CD ,'--Select Region--' AS REGION_CODE  
                                UNION ALL
                               SELECT  DISTINCT  reg.REGION_CD, reg.REGION_CODE 
                                FROM M_REGION reg INNER JOIN M_AREA are ON reg.REGION_CD  = are.REGION_CD AND are.ACTIVE = 1
	                                 INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                WHERE reg.ACTIVE = 1 AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										  INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1
								 )  ORDER BY REGION_CD ";

            DataTable dt = L5sSql.Query(sql);

            this.P5sDdlREGION_CD.DataSource = dt;
            this.P5sDdlREGION_CD.DataValueField = "REGION_CD";
            this.P5sDdlREGION_CD.DataTextField = "REGION_CODE";
            this.P5sDdlREGION_CD.DataBind();
            if (dt.Rows.Count == 2)
            {
                this.P5sDdlREGION_CD.SelectedIndex = 1;
                this.P5sDdlREGION_CD.Enabled = true;
                this.P5sLbtnSearch.Visible = true;
                if (this.P5sRdBtnlist.Items.Count == 2)
                    this.P5sRdBtnlist.Items.Add(new ListItem("Commune", "Commune"));

            }


        }

        private void P5sLoadDropDownListAREA(String RegionCD)
        {
            RegionCD = RegionCD == "" ? "-1" : RegionCD;
            String sql = @"SELECT -1 AS AREA_CD ,'--Select Area--' AS AREA_CODE , 0 AS AREA_ORDER 
                          UNION ALL 
                        
                          SELECT DISTINCT  are.AREA_CD, are.AREA_CODE   ,are.AREA_ORDER
                                FROM  M_AREA are  INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                WHERE are.ACTIVE = 1 AND are.REGION_CD = @0 AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										  INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1 
								 )  
								 ORDER BY AREA_CD  ";

            DataTable dt = L5sSql.Query(sql, RegionCD);
            this.P5sDdlAREA_CD.DataSource = dt;
            this.P5sDdlAREA_CD.DataValueField = "AREA_CD";
            this.P5sDdlAREA_CD.DataTextField = "AREA_CODE";
            this.P5sDdlAREA_CD.DataBind();
            if (dt.Rows.Count == 2)
            {
                this.P5sDdlAREA_CD.SelectedIndex = 1;
                this.P5sDdlAREA_CD.Enabled = true;
                this.P5sLbtnSearch.Visible = true;
            }
        }

        private void P5sLoadDropDownListPROVINCE(String areaCDs)
        {
            areaCDs = areaCDs == "" ? "-1" : areaCDs;
            String sql = String.Format(@"
                                                SELECT -1 AS PROVINCE_CD ,'--Select Province--' AS PROVINCE_CODE  
                                                UNION ALL
                                                SELECT DISTINCT p.PROVINCE_CD, p.PROVINCE_CODE + '-' + p.PROVINCE_NAME_EN as PROVINCE_CODE
                                                FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD
                                                WHERE a.AREA_CD IN ({0})  AND a.ACTIVE = 1
                                                  AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										                      INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										                      INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									                    WHERE pro.PROVINCE_CD = a.PROVINCE_CD AND dis.ACTIVE = 1 )
                                                ORDER BY PROVINCE_CD
								         ", areaCDs);

            //   String sql = @"SELECT -1 AS REGION_CD ,'--Select Region--' AS REGION_CODE  
            //                       UNION ALL
            //                      SELECT  DISTINCT  reg.REGION_CD, reg.REGION_CODE 
            //                       FROM M_REGION reg INNER JOIN M_AREA are ON reg.REGION_CD  = are.REGION_CD AND are.ACTIVE = 1
            //                         INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
            //                       WHERE reg.ACTIVE = 1 AND EXISTS ( SELECT * FROM
            //	                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
            //	  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
            //	  INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
            //WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1
            //)  ORDER BY REGION_CD ";

            DataTable dt = L5sSql.Query(sql);

            this.P5sDdlPROVINCE_CD.DataSource = dt;
            this.P5sDdlPROVINCE_CD.DataValueField = "PROVINCE_CD";
            this.P5sDdlPROVINCE_CD.DataTextField = "PROVINCE_CODE";
            this.P5sDdlPROVINCE_CD.DataBind();
            if (dt.Rows.Count == 2)
            {
                this.P5sDdlPROVINCE_CD.SelectedIndex = 1;
                this.P5sDdlPROVINCE_CD.Enabled = true;
                this.P5sDdlPROVINCE_CD.Visible = true;
            }
        }
        private void P5sInit()
        {
            this.P5sAutoCompleteInit();
        }

        private void P5sAutoCompleteInit()
        {
            DataTable dtableCountry = P5sCmmFns.P5sGetCountry("");
            this.P5sActCountry = this.P5sActCountry == null ? new L5sAutocomplete(dtableCountry, this.P5sTxtCountry.ClientID, 1, true) : this.P5sActCountry;
            this.P5sActCountry.L5sSetDefaultValues(dtableCountry.Rows[0][0].ToString());

            String areaCD = this.P5sDdlAREA_CD.SelectedValue;
            areaCD = areaCD == "" ? "-1" : areaCD;

            ////Load province 
            //this.P5sActPROVINCE = this.P5sActPROVINCE == null ? new L5sAutocomplete(P5sCmmFns.P5sGetProvinceByArea(areaCD), this.P5sTxtPROVINCE_CD.ClientID, 1, true) : this.P5sActPROVINCE;


            ////Load district 
            //String sqlProvince = String.Format(@"SELECT p.PROVINCE_CD 
            //                                    FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD AND a.ACTIVE = 1
            //                                    WHERE a.AREA_CD IN ({0})  
            //                                    AND EXISTS ( SELECT * FROM
            //                                M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
            //                    INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
            //                    INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
            //                 WHERE pro.PROVINCE_CD = a.PROVINCE_CD AND dis.ACTIVE = 1
            //         )  ", areaCD);

            //String provinceCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlProvince);

            this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(P5sCmmFns.P5sGetRegion(""), this.P5sTxtRegionCD.ClientID, 1, true, this.P5sTxtAreaCD.ClientID) : this.P5sActRegion;
            this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(P5sCmmFns.P5sGetArea("REGION_CD"), this.P5sTxtAreaCD.ClientID, 0, true, this.P5sTxtDistributor.ClientID) : this.P5sActArea;
            this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegionCD.ClientID);

            this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(P5sCmmFns.P5sGetAllDistributor("AREA_CD"), this.P5sTxtDistributor.ClientID, 0, true) : this.P5sActDistributor;
            this.P5sActDistributor.L5sChangeFilteringId(this.P5sTxtAreaCD.ClientID);

        }

        protected void P5sDdlREGION_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.P5sDdlREGION_CD.SelectedIndex == 0)
            {
                this.P5sRdBtnlist.Items.RemoveAt(2);
                this.P5sLbtnSearch.Visible = false;
            }
            else
                if (this.P5sRdBtnlist.Items.Count == 2)
            {
                this.P5sRdBtnlist.Items.Add(new ListItem("Commune", "Commune"));
                this.P5sLbtnSearch.Visible = true;
            }

            String regionCD = this.P5sDdlREGION_CD.SelectedValue;
            this.P5sLoadDropDownListAREA(regionCD);
            string areaCD = this.P5sDdlAREA_CD.SelectedValue;
            this.P5sLoadDropDownListPROVINCE(areaCD);
            this.P5sInit();
            L5sJS.L5sRun("loadMap()");
            this.P5sPnlSearchCoverage.Visible = false;


        }

        protected void P5sDdlAREA_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.P5sInit();
            L5sJS.L5sRun("loadMap()");
            string areaCD = this.P5sDdlAREA_CD.SelectedValue;
            this.P5sLoadDropDownListPROVINCE(areaCD);
            this.P5sPnlSearchCoverage.Visible = false;
        }

        #region Population Method
        private Boolean P5sCheckPopulation(out Int64 pmin, out Int64 pmax)
        {
            Int64 populationMin = 0;
            Int64 populationMax = 0;
            if (this.P5sTxtPopulationMin.Text.Trim().Length <= 0 && this.P5sTxtPopulationMax.Text.Trim().Length <= 0)
            {
                pmin = pmax = 0;
                return true;
            }

            String min = this.P5sTxtPopulationMin.Text.Replace(",", "");
            String max = this.P5sTxtPopulationMax.Text.Replace(",", "");


            if (this.P5sTxtPopulationMin.Text.Trim().Length <= 0)
                min = "0";

            if (this.P5sTxtPopulationMax.Text.Trim().Length <= 0)
                max = "0";

            bool isNum = Int64.TryParse(min, out populationMin);
            if (!isNum)
            {
                L5sJS.L5sRun("loadMap()");
                L5sMsg.Show("Population min chỉ chấp nhận kiểu số.");
                pmin = pmax = 0;
                return false;
            }

            isNum = Int64.TryParse(max, out populationMax);
            if (!isNum)
            {
                L5sJS.L5sRun("loadMap()");
                L5sMsg.Show("Population max chỉ chấp nhận kiểu số.");
                pmin = pmax = 0;
                return false;
            }

            if (populationMax < -1)
            {
                L5sJS.L5sRun("loadMap()");
                L5sMsg.Show("Population max không chấp nhận số âm.");
                pmin = pmax = 0;
                return false;
            }

            if (populationMin < -1)
            {
                L5sJS.L5sRun("loadMap()");
                L5sMsg.Show("Population min không chấp nhận số âm.");
                pmin = pmax = 0;
                return false;
            }

            if (populationMin > populationMax)
            {
                L5sJS.L5sRun("loadMap()");
                L5sMsg.Show("Population max không được nhỏ hơn Population min .");
                pmin = pmax = 0;
                return false;
            }

            //get value
            pmin = populationMin;
            pmax = populationMax;
            return true;

        }
        #endregion
        #region WS Utility
        protected DataTable P5sGetWS(Int64 populationMin, Int64 populationMax)
        {
            Boolean chkWard = this.P5sChkWard.Checked;
            Boolean chkTown = this.P5sChkTown.Checked;
            Boolean chkCommune = this.P5sChkCommune.Checked;
            String sql = String.Format(@"    select COM.COMMUNE_CD,COM.COMMUNE_CODE,COM.COMMUNE_NAME_EN,COM.COMMUNE_TYPE,COM.LONGITUDE_LATITUDE AS  COM_LONGITUDE_LATITUDE
                                                                                               ,DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam ' as FULL_ADDRESS
	                                                                                           ,COM.[POPULATION] as [POPULATION]
	                                                                                           ,DIST.DISTRIBUTOR_NAME as DISTRIBUTOR_NAME
                                                                                               ,DISTRIBUTOR_DISTANCE
	                                                                                           ,COM.TOWN_COVERED_NAME as TOWN_COVERED_NAME
	                                                                                           ,COM.TOWN_COVERED_DISTANCE as TOWN_COVERED_DISTANCE
	                                                                                           ,COM.COMMUNE_COVERED_NAME as COMMUNE_COVERED_NAME
	                                                                                           ,COM.COMMUNE_COVERED_DISTANCE as COMMUNE_COVERED_DISTANCE
	                                                                                           ,COUNT(*) as TOTAL_CUSTOMER
	                                                                                           ,COM.AMS as AMS
                                                                                                ,SUM( CASE CUS.CUSTOMER_CHAIN_CODE WHEN 'WSN' THEN 1 ELSE 0 END) as WHOLE_SALES
                                                                                                ,COM.COMMUNE_NAME_EN + ' , ' + DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN as TITLE_DIRECTION
                                                                                                ,DIST.LONGITUDE_LATITUDE as LONGITUDE_LATITUDE_DISTRIBUTOR
                                                                                                ,DIS.DISTRICT_CD,DIST.DISTRIBUTOR_CD 
                                                            INTO #TMP_TABLE
                                                            from M_COMMUNE COM INNER JOIN M_DISTRICT DIS on COM.DISTRICT_CD = DIS.DISTRICT_CD
                                                                                            INNER JOIN M_PROVINCE PRO on DIS.PROVINCE_CD = PRO.PROVINCE_CD
				                                                                            INNER JOIN M_CUSTOMER CUS on COM.COMMUNE_CD = CUS.COMMUNE_CD
				                                                                            INNER JOIN M_AREA_PROVINCE AREP on AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
				                                                                            INNER JOIN M_AREA ARE on ARE.AREA_CD = arep.AREA_CD
				                                                                            INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
				                                                                            INNER JOIN M_DISTRIBUTOR DIST on CUS.DISTRIBUTOR_CD = DIST.DISTRIBUTOR_CD 
                                                                                            
                                                                            where CUS.COMMUNE_CD is not null and (1=1)   and (2=2)  and (3=3) AND DIST.ACTIVE = 1 AND CUS.ACTIVE = 1
                                                                                        AND (5=5)
                                                                            group by 
	                                                                           DIS.DISTRICT_CD,COM.DISTRIBUTOR_DISTANCE,  COM.COMMUNE_CD,COM.COMMUNE_CODE,COM.COMMUNE_NAME_EN,COM.COMMUNE_TYPE,COM.LONGITUDE_LATITUDE 
		                                                                               ,DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam '
		                                                                               ,COM.[POPULATION]  ,DIST.DISTRIBUTOR_NAME  ,COM.TOWN_COVERED_NAME   ,COM.TOWN_COVERED_DISTANCE   ,COM.COMMUNE_COVERED_NAME    ,COM.COMMUNE_COVERED_DISTANCE    ,COM.AMS   
                                                                                       ,COM.COMMUNE_NAME_EN + ' , ' + DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN 
                                                                                       ,DIST.LONGITUDE_LATITUDE ,DIST.DISTRIBUTOR_CD, COM.COMMUNE_TYPE
                                                                             having SUM( CASE CUS.CUSTOMER_CHAIN_CODE WHEN 'WSN' THEN 1 ELSE 0 END) >  0

                                                         SELECT * FROM #TMP_TABLE  AS T
						                                 WHERE  (4=4)

                                                         DROP TABLE #TMP_TABLE
                                                              ");

            if (this.P5sDdlPROVINCE_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" PRO.PROVINCE_CD in ({0})", this.P5sDdlPROVINCE_CD.SelectedValue));
            else
                if (this.P5sDdlAREA_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" ARE.AREA_CD = {0} ", this.P5sDdlAREA_CD.SelectedValue));
            else
                    if (this.P5sDdlREGION_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" REG.REGION_CD = {0}", this.P5sDdlREGION_CD.SelectedValue));


            if (this.P5sDdlDistributorType.SelectedValue != "0")
                sql = sql.Replace("(5=5)", String.Format(" DIST.DISTRIBUTOR_TYPE_CD = {0}", this.P5sDdlDistributorType.SelectedValue));




            if (populationMin != 0 || populationMax != 0)
                sql = sql.Replace("(2=2)", String.Format("COM.[POPULATION] between {0} and {1}", populationMin, populationMax));

            String strType = "";
            if (chkTown || chkWard || chkCommune)
            {
                strType = chkWard == true ? "1" : strType;
                strType = chkTown == true ? strType == "" ? "2" : strType + ",2" : strType;
                strType = chkCommune == true ? strType == "" ? "3" : strType + ",3" : strType;
                sql = sql.Replace("(1=1)", String.Format("COM.COMMUNE_TYPE in ({0})", strType));

            }

            if (this.P5sChkBank.Checked || this.P5sChkATM.Checked || this.P5sChkHospital.Checked
           || this.P5sChkPostOffice.Checked || this.P5sChkSchool.Checked || this.P5sChkStateHighway.Checked
           || this.P5sChkNationlHighway.Checked || this.P5sChkBSBF.Checked || this.P5sChkMarket.Checked)
            {
                String strTypePOI = "";
                strTypePOI = this.P5sChkBank.Checked == true ? "1" : strTypePOI; //BANK
                strTypePOI = this.P5sChkATM.Checked == true ? strTypePOI == "" ? "2" : strTypePOI + ",2" : strTypePOI; //ATM
                strTypePOI = this.P5sChkHospital.Checked == true ? strTypePOI == "" ? "3" : strTypePOI + ",3" : strTypePOI; // HOSPITAL
                strTypePOI = this.P5sChkPostOffice.Checked == true ? strTypePOI == "" ? "4" : strTypePOI + ",4" : strTypePOI; // PostOffice
                strTypePOI = this.P5sChkSchool.Checked == true ? strTypePOI == "" ? "5" : strTypePOI + ",5" : strTypePOI; // School

                strTypePOI = this.P5sChkStateHighway.Checked == true ? strTypePOI == "" ? "6" : strTypePOI + ",6" : strTypePOI; // StateHighway
                strTypePOI = this.P5sChkNationlHighway.Checked == true ? strTypePOI == "" ? "7" : strTypePOI + ",7" : strTypePOI; // NationlHighway
                strTypePOI = this.P5sChkBSBF.Checked == true ? strTypePOI == "" ? "8" : strTypePOI + ",8" : strTypePOI; // P5sLblBSBF

                strTypePOI = this.P5sChkMarket.Checked == true ? strTypePOI == "" ? "9" : strTypePOI + ",9" : strTypePOI; // P5sLblMarket


                sql = sql.Replace("(4=4)", String.Format(" exists (select * from M_POIS p where p.COMMUNE_CD = T.COMMUNE_CD and  p.POIS_TYPE in ({0}) )", strTypePOI));


            }

            return L5sSql.Query(sql);

        }


        protected DataTable P5sGetCommuneNotExsits(String communes)
        {
            String sql = String.Format(@"select COM.COMMUNE_CD,COM.DISTRICT_CD
                                     from M_COMMUNE COM INNER JOIN M_DISTRICT DIS on COM.DISTRICT_CD = DIS.DISTRICT_CD
                                         INNER JOIN M_PROVINCE PRO on DIS.PROVINCE_CD = PRO.PROVINCE_CD
	                                     INNER JOIN M_AREA_PROVINCE AREP on AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
	                                     INNER JOIN M_AREA ARE on ARE.AREA_CD = arep.AREA_CD
	                                     INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
                            where  (1=1) and  COM.COMMUNE_CD not in ({0}) ", communes);

            if (this.P5sDdlPROVINCE_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" PRO.PROVINCE_CD in ({0})", this.P5sDdlPROVINCE_CD.SelectedValue));
            else
                if (this.P5sDdlAREA_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" ARE.AREA_CD = {0}", this.P5sDdlAREA_CD.SelectedValue));
            else
                    if (this.P5sDdlREGION_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" REG.REGION_CD = {0}", this.P5sDdlREGION_CD.SelectedValue));

            return L5sSql.Query(sql);
        }

        protected DataTable P5sGetDistrictNotExsits(String districtCds)
        {
            String sql = String.Format(@"select distinct COM.DISTRICT_CD
                                     from M_COMMUNE COM INNER JOIN M_DISTRICT DIS on COM.DISTRICT_CD = DIS.DISTRICT_CD
                                         INNER JOIN M_PROVINCE PRO on DIS.PROVINCE_CD = PRO.PROVINCE_CD
	                                     INNER JOIN M_AREA_PROVINCE AREP on AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
	                                     INNER JOIN M_AREA ARE on ARE.AREA_CD = arep.AREA_CD
	                                     INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
                            where  (1=1) and  DIS.DISTRICT_CD not in ({0}) ", districtCds);

            if (this.P5sDdlPROVINCE_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" PRO.PROVINCE_CD in ({0})", this.P5sDdlPROVINCE_CD.SelectedValue));
            else
                if (this.P5sDdlAREA_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" ARE.AREA_CD = {0}", this.P5sDdlAREA_CD.SelectedValue));
            else
                    if (this.P5sDdlREGION_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" REG.REGION_CD = {0}", this.P5sDdlREGION_CD.SelectedValue));

            return L5sSql.Query(sql);
        }


        #endregion
        #region WWS Utility
        protected DataTable P5sGetWWS(Int64 populationMin, Int64 populationMax)
        {
            Boolean chkWard = this.P5sChkWard.Checked;
            Boolean chkTown = this.P5sChkTown.Checked;
            Boolean chkCommune = this.P5sChkCommune.Checked;
            String sql = String.Format(@"  
                                        select COM.COMMUNE_CD,COM.COMMUNE_CODE,COM.COMMUNE_NAME_EN,COM.COMMUNE_TYPE,COM.LONGITUDE_LATITUDE AS  COM_LONGITUDE_LATITUDE
                                                                                   ,DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam ' as FULL_ADDRESS
	                                                                               ,COM.[POPULATION] as [POPULATION]
	                                                                               ,DIST.DISTRIBUTOR_NAME as DISTRIBUTOR_NAME
                                                                                   ,DISTRIBUTOR_DISTANCE
	                                                                               ,COM.TOWN_COVERED_NAME as TOWN_COVERED_NAME
	                                                                               ,COM.TOWN_COVERED_DISTANCE as TOWN_COVERED_DISTANCE
	                                                                               ,COM.COMMUNE_COVERED_NAME as COMMUNE_COVERED_NAME
	                                                                               ,COM.COMMUNE_COVERED_DISTANCE as COMMUNE_COVERED_DISTANCE
	                                                                               ,COUNT(*) as TOTAL_CUSTOMER
	                                                                               ,COM.AMS as AMS
                                                                                    ,'' as WHOLE_SALES
                                                                                    ,COM.COMMUNE_NAME_EN + ' , ' + DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN  as TITLE_DIRECTION
                                                                                    ,DIST.LONGITUDE_LATITUDE as LONGITUDE_LATITUDE_DISTRIBUTOR
                                                                                   ,DIS.DISTRICT_CD,DIST.DISTRIBUTOR_CD
                                                    INTO #TMP_TABLE              
                                                    from M_COMMUNE COM INNER JOIN M_DISTRICT DIS on COM.DISTRICT_CD = DIS.DISTRICT_CD
                                                                                            INNER JOIN M_PROVINCE PRO on DIS.PROVINCE_CD = PRO.PROVINCE_CD
				                                                                            INNER JOIN M_CUSTOMER CUS on COM.COMMUNE_CD = CUS.COMMUNE_CD 
				                                                                            INNER JOIN M_AREA_PROVINCE AREP on AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
				                                                                            INNER JOIN M_AREA ARE on ARE.AREA_CD = arep.AREA_CD
				                                                                            INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
				                                                                            INNER JOIN M_DISTRIBUTOR DIST on CUS.DISTRIBUTOR_CD = DIST.DISTRIBUTOR_CD 
                                                                            where CUS.COMMUNE_CD is not null and (1=1)   and (2=2)  and (3=3) AND DIST.ACTIVE = 1 AND CUS.ACTIVE = 1
                                                                                               AND (5=5)
                                                                            group by 
	                                                                           DIS.DISTRICT_CD,COM.DISTRIBUTOR_DISTANCE,  COM.COMMUNE_CD,COM.COMMUNE_CODE,COM.COMMUNE_NAME_EN,COM.COMMUNE_TYPE,COM.LONGITUDE_LATITUDE 
		                                                                               ,DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam '
		                                                                               ,COM.[POPULATION]  ,DIST.DISTRIBUTOR_NAME  ,COM.TOWN_COVERED_NAME   ,COM.TOWN_COVERED_DISTANCE   ,COM.COMMUNE_COVERED_NAME    ,COM.COMMUNE_COVERED_DISTANCE    ,COM.AMS   
                                                                                       ,COM.COMMUNE_NAME_EN + ' , ' + DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN 
                                                                                       ,DIST.LONGITUDE_LATITUDE ,DIST.DISTRIBUTOR_CD, COM.COMMUNE_TYPE
                                                                             having SUM( CASE CUS.CUSTOMER_CHAIN_CODE WHEN 'WSN' THEN 1 ELSE 0 END) = 0
                                                    SELECT * FROM #TMP_TABLE  AS T
						                            WHERE  (4=4)

                                                    DROP TABLE #TMP_TABLE
                                        
                                                              ");

            if (this.P5sDdlPROVINCE_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" PRO.PROVINCE_CD in ({0})", this.P5sDdlPROVINCE_CD.SelectedValue));
            else
                if (this.P5sDdlAREA_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" ARE.AREA_CD = {0} ", this.P5sDdlAREA_CD.SelectedValue));
            else
                    if (this.P5sDdlREGION_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" REG.REGION_CD =  {0}", this.P5sDdlREGION_CD.SelectedValue));

            if (populationMin != 0 || populationMax != 0)
                sql = sql.Replace("(2=2)", String.Format("COM.[POPULATION] between {0} and {1}", populationMin, populationMax));

            if (this.P5sDdlDistributorType.SelectedValue != "0")
                sql = sql.Replace("(5=5)", String.Format(" DIST.DISTRIBUTOR_TYPE_CD = {0}", this.P5sDdlDistributorType.SelectedValue));


            String strType = "";
            if (chkTown || chkWard || chkCommune)
            {
                strType = chkWard == true ? "1" : strType;
                strType = chkTown == true ? strType == "" ? "2" : strType + ",2" : strType;
                strType = chkCommune == true ? strType == "" ? "3" : strType + ",3" : strType;
                sql = sql.Replace("(1=1)", String.Format("COM.COMMUNE_TYPE in ({0})", strType));

            }

            if (this.P5sChkBank.Checked || this.P5sChkATM.Checked || this.P5sChkHospital.Checked
          || this.P5sChkPostOffice.Checked || this.P5sChkSchool.Checked || this.P5sChkStateHighway.Checked
          || this.P5sChkNationlHighway.Checked || this.P5sChkBSBF.Checked || this.P5sChkMarket.Checked)
            {
                String strTypePOI = "";
                strTypePOI = this.P5sChkBank.Checked == true ? "1" : strTypePOI; //BANK
                strTypePOI = this.P5sChkATM.Checked == true ? strTypePOI == "" ? "2" : strTypePOI + ",2" : strTypePOI; //ATM
                strTypePOI = this.P5sChkHospital.Checked == true ? strTypePOI == "" ? "3" : strTypePOI + ",3" : strTypePOI; // HOSPITAL
                strTypePOI = this.P5sChkPostOffice.Checked == true ? strTypePOI == "" ? "4" : strTypePOI + ",4" : strTypePOI; // PostOffice
                strTypePOI = this.P5sChkSchool.Checked == true ? strTypePOI == "" ? "5" : strTypePOI + ",5" : strTypePOI; // School

                strTypePOI = this.P5sChkStateHighway.Checked == true ? strTypePOI == "" ? "6" : strTypePOI + ",6" : strTypePOI; // StateHighway
                strTypePOI = this.P5sChkNationlHighway.Checked == true ? strTypePOI == "" ? "7" : strTypePOI + ",7" : strTypePOI; // NationlHighway
                strTypePOI = this.P5sChkBSBF.Checked == true ? strTypePOI == "" ? "8" : strTypePOI + ",8" : strTypePOI; // P5sLblBSBF

                strTypePOI = this.P5sChkMarket.Checked == true ? strTypePOI == "" ? "9" : strTypePOI + ",9" : strTypePOI; // P5sLblMarket


                sql = sql.Replace("(4=4)", String.Format(" exists (select * from M_POIS p where p.COMMUNE_CD = T.COMMUNE_CD and  p.POIS_TYPE in ({0}) )", strTypePOI));

            }



            return L5sSql.Query(sql);

        }
        #endregion
        #region UnCover Utility
        protected DataTable P5sGetUnCoveraged(Int64 populationMin, Int64 populationMax)
        {
            Boolean chkWard = this.P5sChkWard.Checked;
            Boolean chkTown = this.P5sChkTown.Checked;
            Boolean chkCommune = this.P5sChkCommune.Checked;




            String sql = String.Format(@"

                    
                        select COM.COMMUNE_CD,COM.COMMUNE_CODE,COM.COMMUNE_NAME_EN,COM.COMMUNE_TYPE,COM.LONGITUDE_LATITUDE AS  COM_LONGITUDE_LATITUDE
                                        ,DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam ' as FULL_ADDRESS
	                                    ,COM.[POPULATION] as [POPULATION]
	                                    ,DISTRIBUTOR_NAME_NEAREST as DISTRIBUTOR_NAME
                                        ,COM.DISTRIBUTOR_DISTANCE as DISTRIBUTOR_DISTANCE
	                                    ,COM.TOWN_COVERED_NAME as TOWN_COVERED_NAME
	                                    ,COM.TOWN_COVERED_DISTANCE as TOWN_COVERED_DISTANCE
	                                    ,COM.COMMUNE_COVERED_NAME as COMMUNE_COVERED_NAME
	                                    ,COM.COMMUNE_COVERED_DISTANCE as COMMUNE_COVERED_DISTANCE
                                        ,COM.COMMUNE_NAME_EN + ' , ' + DIS.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN as TITLE_DIRECTION
                                       ,DIS.DISTRICT_CD
                         						
						INTO #TMP_TABLE      
                        
                        from M_COMMUNE COM INNER JOIN M_DISTRICT DIS on COM.DISTRICT_CD = DIS.DISTRICT_CD
                                         INNER JOIN M_PROVINCE PRO on DIS.PROVINCE_CD = PRO.PROVINCE_CD
	                                     INNER JOIN M_AREA_PROVINCE AREP on AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
	                                     INNER JOIN M_AREA ARE on ARE.AREA_CD = arep.AREA_CD
	                                     INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
                            where  not exists (select * from M_CUSTOMER CUS INNER JOIN M_DISTRIBUTOR DIST on CUS.DISTRIBUTOR_CD = DIST.DISTRIBUTOR_CD where  COM.COMMUNE_CD = CUS.COMMUNE_CD AND CUS.ACTIVE = 1 AND DIST.ACTIVE = 1 ) 
                                   and (1=1) and (2=2) and (3=3) 


                	    SELECT * FROM #TMP_TABLE  AS T
						WHERE  (4=4)

                        DROP TABLE #TMP_TABLE ");

            if (this.P5sDdlPROVINCE_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" PRO.PROVINCE_CD in ({0})", this.P5sDdlPROVINCE_CD.SelectedValue));
            else
                if (this.P5sDdlAREA_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" ARE.AREA_CD = {0}", this.P5sDdlAREA_CD.SelectedValue));
            else
                    if (this.P5sDdlREGION_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" REG.REGION_CD = {0}", this.P5sDdlREGION_CD.SelectedValue));

            if (populationMin != 0 || populationMax != 0)
                sql = sql.Replace("(2=2)", String.Format("COM.[POPULATION] between {0} and {1}", populationMin, populationMax));



            String strType = "";
            if (chkTown || chkWard || chkCommune)
            {
                strType = chkWard == true ? "1" : strType;
                strType = chkTown == true ? strType == "" ? "2" : strType + ",2" : strType;
                strType = chkCommune == true ? strType == "" ? "3" : strType + ",3" : strType;
                sql = sql.Replace("(1=1)", String.Format("COM.COMMUNE_TYPE in ({0})", strType));

            }


            if (this.P5sChkBank.Checked || this.P5sChkATM.Checked || this.P5sChkHospital.Checked
            || this.P5sChkPostOffice.Checked || this.P5sChkSchool.Checked || this.P5sChkStateHighway.Checked
            || this.P5sChkNationlHighway.Checked || this.P5sChkBSBF.Checked || this.P5sChkMarket.Checked)
            {
                String strTypePOI = "";
                strTypePOI = this.P5sChkBank.Checked == true ? "1" : strTypePOI; //BANK
                strTypePOI = this.P5sChkATM.Checked == true ? strTypePOI == "" ? "2" : strTypePOI + ",2" : strTypePOI; //ATM
                strTypePOI = this.P5sChkHospital.Checked == true ? strTypePOI == "" ? "3" : strTypePOI + ",3" : strTypePOI; // HOSPITAL
                strTypePOI = this.P5sChkPostOffice.Checked == true ? strTypePOI == "" ? "4" : strTypePOI + ",4" : strTypePOI; // PostOffice
                strTypePOI = this.P5sChkSchool.Checked == true ? strTypePOI == "" ? "5" : strTypePOI + ",5" : strTypePOI; // School

                strTypePOI = this.P5sChkStateHighway.Checked == true ? strTypePOI == "" ? "6" : strTypePOI + ",6" : strTypePOI; // StateHighway
                strTypePOI = this.P5sChkNationlHighway.Checked == true ? strTypePOI == "" ? "7" : strTypePOI + ",7" : strTypePOI; // NationlHighway
                strTypePOI = this.P5sChkBSBF.Checked == true ? strTypePOI == "" ? "8" : strTypePOI + ",8" : strTypePOI; // P5sLblBSBF
                strTypePOI = this.P5sChkMarket.Checked == true ? strTypePOI == "" ? "9" : strTypePOI + ",9" : strTypePOI; // P5sLblMarket


                sql = sql.Replace("(4=4)", String.Format(" exists (select * from M_POIS p where p.COMMUNE_CD = T.COMMUNE_CD and  p.POIS_TYPE in ({0}) )", strTypePOI));

            }


            return L5sSql.Query(sql);

        }
        #endregion
        #region Distributor Utility



        private DataTable P5sGetDistributorDirectByDistrict(String districtCDs)
        {

            String sql = String.Format(@"
                                         select DIS.DISTRIBUTOR_CD, DIS.DISTRIBUTOR_CODE, DIS.DISTRIBUTOR_NAME, DIS.DISTRIBUTOR_TYPE,DIS.LONGITUDE_LATITUDE 
		                                            ,DIST.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam ' as FULL_ADDRESS
	                                                ,DIS.POPULATION_COVERED 
		                                            ,DIS.RADIUS
		                                            ,DIS.AREA_COVERED
		                                            ,DIS.CUSTOMER_COVERED AS CUSTOMER
		                                            ,DIS.AMS
		                                            ,ISNULL(DIS.WHOLE_SALES,0) as WHOLE_SALES    
                                                    ,DIS.DSP   ,
                                                     DIS.DISTRIBUTOR_TYPE_CD, 
                                                     parent.DISTRIBUTOR_CODE + '-' + parent.DISTRIBUTOR_NAME AS DISTRIBUTOR_PARENT,
                                                     DisType.DISTRIBUTOR_TYPE_IMAGE                                                                                
                                           from M_DISTRIBUTOR DIS INNER JOIN M_COMMUNE COM on DIS.COMMUNE_CD = COM.COMMUNE_CD AND DIS.ACTIVE = 1
                                                                INNER JOIN M_DISTRICT DIST	on COM.DISTRICT_CD = DIST.DISTRICT_CD
					                                            INNER JOIN M_PROVINCE PRO on DIST.PROVINCE_CD = PRO.PROVINCE_CD
																INNER JOIN M_AREA_PROVINCE arePro on PRO.PROVINCE_CD = arePro.PROVINCE_CD and arePro.ACTIVE = 1
					                                            INNER JOIN M_AREA ARE on arePro.AREA_CD = ARE.AREA_CD
					                                            INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
                                                                LEFT JOIN M_DISTRIBUTOR parent ON DIS.DISTRIBUTOR_PARENT_CD = parent.DISTRIBUTOR_CD                                                             
                                                                INNER JOIN M_DISTRIBUTOR_TYPE DisType ON DisType.DISTRIBUTOR_TYPE_CD = parent.DISTRIBUTOR_TYPE_CD                                                    
                                       where  DIS.DISTRIBUTOR_TYPE_CD  = 1 AND exists (select * from M_CUSTOMER c where c.ACTIVE = 1 AND c.DISTRICT_CD in ({0}) and c.DISTRIBUTOR_CD = DIS.DISTRIBUTOR_CD )", districtCDs);


            return L5sSql.Query(sql);
        }




        private DataTable P5sGetDistributorInDirectByDistrict(String districtCDs)
        {

            String sql = String.Format(@"
                                         select DIS.DISTRIBUTOR_CD, DIS.DISTRIBUTOR_CODE, DIS.DISTRIBUTOR_NAME, DIS.DISTRIBUTOR_TYPE,DIS.LONGITUDE_LATITUDE 
		                                            ,DIST.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam ' as FULL_ADDRESS
	                                                ,DIS.POPULATION_COVERED 
		                                            ,DIS.RADIUS
		                                            ,DIS.AREA_COVERED
		                                            ,DIS.CUSTOMER_COVERED AS CUSTOMER
		                                            ,DIS.AMS
		                                            ,ISNULL(DIS.WHOLE_SALES,0) as WHOLE_SALES    
                                                    ,DIS.DSP   ,
                                                     DIS.DISTRIBUTOR_TYPE_CD, 
                                                     parent.DISTRIBUTOR_CODE + '-' + parent.DISTRIBUTOR_NAME AS DISTRIBUTOR_PARENT                                                                              
                                           from M_DISTRIBUTOR DIS INNER JOIN M_COMMUNE COM on DIS.COMMUNE_CD = COM.COMMUNE_CD AND DIS.ACTIVE = 1
                                                                INNER JOIN M_DISTRICT DIST	on COM.DISTRICT_CD = DIST.DISTRICT_CD
					                                            INNER JOIN M_PROVINCE PRO on DIST.PROVINCE_CD = PRO.PROVINCE_CD
																INNER JOIN M_AREA_PROVINCE arePro on PRO.PROVINCE_CD = arePro.PROVINCE_CD and arePro.ACTIVE = 1
					                                            INNER JOIN M_AREA ARE on arePro.AREA_CD = ARE.AREA_CD
					                                            INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
                                                                left join M_DISTRIBUTOR parent ON DIS.DISTRIBUTOR_PARENT_CD = parent.DISTRIBUTOR_CD                                                             
                                       where  DIS.DISTRIBUTOR_TYPE_CD  = 2 AND exists (select * from M_CUSTOMER c where c.ACTIVE = 1 AND c.DISTRICT_CD in ({0}) and c.DISTRIBUTOR_CD = DIS.DISTRIBUTOR_CD )", districtCDs);


            return L5sSql.Query(sql);
        }


        private DataTable P5sGetDistributorDirect(String distributorCDs)
        {

            String sql = String.Format(@"
                                         select DIS.DISTRIBUTOR_CD, DIS.DISTRIBUTOR_CODE, DIS.DISTRIBUTOR_CODE + '-' +  DIS.DISTRIBUTOR_NAME AS DISTRIBUTOR_NAME, DIS.DISTRIBUTOR_TYPE,DIS.LONGITUDE_LATITUDE 
		                                            ,DIST.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam ' as FULL_ADDRESS
	                                                ,DIS.POPULATION_COVERED 
		                                            ,DIS.RADIUS
		                                            ,DIS.AREA_COVERED
		                                            ,DIS.CUSTOMER_COVERED AS CUSTOMER
		                                            ,DIS.AMS
		                                            ,ISNULL(DIS.WHOLE_SALES,0) as WHOLE_SALES    
                                                    ,DIS.DSP ,
                                                     DIS.DISTRIBUTOR_TYPE_CD, 
                                                     '' AS DISTRIBUTOR_PARENT , distType.DISTRIBUTOR_TYPE_IMAGE
                                                                                
                                           from M_DISTRIBUTOR DIS INNER JOIN M_COMMUNE COM on DIS.COMMUNE_CD = COM.COMMUNE_CD 
                                                                INNER JOIN M_DISTRICT DIST	on COM.DISTRICT_CD = DIST.DISTRICT_CD
					                                            INNER JOIN M_PROVINCE PRO on DIST.PROVINCE_CD = PRO.PROVINCE_CD
																INNER JOIN M_AREA_PROVINCE arePro ON PRO.PROVINCE_CD =  arePro.PROVINCE_CD AND arePro.ACTIVE = 1
					                                            INNER JOIN M_AREA ARE on arePro.AREA_CD = ARE.AREA_CD
					                                            INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD  
                                                                INNER JOIN M_DISTRIBUTOR_TYPE distType ON DIS.DISTRIBUTOR_TYPE_CD = distType.DISTRIBUTOR_TYPE_CD
                                                              
                                       where DIS.DISTRIBUTOR_CD in ({0}) AND DIS.ACTIVE = 1 AND DIS.DISTRIBUTOR_TYPE_CD = 1 ", distributorCDs);
            return L5sSql.Query(sql);
        }

        private DataTable P5sGetDistributorInDirect(String distributorCDs)
        {

            String sql = String.Format(@"
                                         select DIS.DISTRIBUTOR_CD, DIS.DISTRIBUTOR_CODE, DIS.DISTRIBUTOR_CODE + '-' +  DIS.DISTRIBUTOR_NAME AS DISTRIBUTOR_NAME, DIS.DISTRIBUTOR_TYPE,DIS.LONGITUDE_LATITUDE 
		                                            ,DIST.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam ' as FULL_ADDRESS
	                                                ,DIS.POPULATION_COVERED 
		                                            ,DIS.RADIUS
		                                            ,DIS.AREA_COVERED
		                                            ,DIS.CUSTOMER_COVERED AS CUSTOMER
		                                            ,DIS.AMS
		                                            ,ISNULL(DIS.WHOLE_SALES,0) as WHOLE_SALES    
                                                    ,DIS.DSP ,
                                                     DIS.DISTRIBUTOR_TYPE_CD, 
                                                     parent.DISTRIBUTOR_CODE + '-' + parent.DISTRIBUTOR_NAME AS DISTRIBUTOR_PARENT
                                                    , distType.DISTRIBUTOR_TYPE_IMAGE
                                                                                
                                           from M_DISTRIBUTOR DIS INNER JOIN M_COMMUNE COM on DIS.COMMUNE_CD = COM.COMMUNE_CD 
                                                                INNER JOIN M_DISTRICT DIST	on COM.DISTRICT_CD = DIST.DISTRICT_CD
					                                            INNER JOIN M_PROVINCE PRO on DIST.PROVINCE_CD = PRO.PROVINCE_CD
																INNER JOIN M_AREA_PROVINCE arePro ON PRO.PROVINCE_CD =  arePro.PROVINCE_CD AND arePro.ACTIVE = 1
					                                            INNER JOIN M_AREA ARE on arePro.AREA_CD = ARE.AREA_CD
					                                            INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD   
                                                                INNER JOIN M_DISTRIBUTOR parent ON DIS.DISTRIBUTOR_PARENT_CD = parent.DISTRIBUTOR_CD
                                                                INNER JOIN M_DISTRIBUTOR_TYPE distType ON DIS.DISTRIBUTOR_TYPE_CD = distType.DISTRIBUTOR_TYPE_CD                                                             
                                       where DIS.DISTRIBUTOR_CD in ({0}) AND DIS.ACTIVE = 1 AND DIS.DISTRIBUTOR_TYPE_CD IN (2,3) ", distributorCDs);
            return L5sSql.Query(sql);
        }

        private DataTable P5sGetDistributorInDirectNoCustomer(String distributorCDs)
        {
            String sql = String.Format(@"
                                         select DIS.DISTRIBUTOR_CD, DIS.DISTRIBUTOR_CODE, DIS.DISTRIBUTOR_CODE + '-' +  DIS.DISTRIBUTOR_NAME AS DISTRIBUTOR_NAME, DIS.DISTRIBUTOR_TYPE,DIS.LONGITUDE_LATITUDE 
		                                            ,DIST.DISTRICT_NAME_EN + ' , ' + PRO.PROVINCE_NAME_EN + ' , ' + ARE.AREA_DESC + ' , ' + REG.REGION_DESC + ' , Viet Nam ' as FULL_ADDRESS
	                                                ,DIS.POPULATION_COVERED 
		                                            ,DIS.RADIUS
		                                            ,DIS.AREA_COVERED
		                                            ,DIS.CUSTOMER_COVERED AS CUSTOMER
		                                            ,DIS.AMS
		                                            ,ISNULL(DIS.WHOLE_SALES,0) as WHOLE_SALES    
                                                    ,DIS.DSP ,
                                                     DIS.DISTRIBUTOR_TYPE_CD, 
                                                     parent.DISTRIBUTOR_CODE + '-' + parent.DISTRIBUTOR_NAME AS DISTRIBUTOR_PARENT
                                                     , distType.DISTRIBUTOR_TYPE_IMAGE                     
                                           from M_DISTRIBUTOR DIS INNER JOIN M_COMMUNE COM on DIS.COMMUNE_CD = COM.COMMUNE_CD 
                                                                INNER JOIN M_DISTRICT DIST	on COM.DISTRICT_CD = DIST.DISTRICT_CD
					                                            INNER JOIN M_PROVINCE PRO on DIST.PROVINCE_CD = PRO.PROVINCE_CD
																INNER JOIN M_AREA_PROVINCE arePro ON PRO.PROVINCE_CD =  arePro.PROVINCE_CD AND arePro.ACTIVE = 1
					                                            INNER JOIN M_AREA ARE on arePro.AREA_CD = ARE.AREA_CD
					                                            INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD   
                                                                INNER JOIN M_DISTRIBUTOR parent ON DIS.DISTRIBUTOR_PARENT_CD = parent.DISTRIBUTOR_CD    
                                                                INNER JOIN M_DISTRIBUTOR_TYPE distType ON DIS.DISTRIBUTOR_TYPE_CD = distType.DISTRIBUTOR_TYPE_CD                                                                                                                      
                                       where DIS.DISTRIBUTOR_CD NOT IN ({0}) AND DIS.ACTIVE = 1 AND DIS.DISTRIBUTOR_TYPE_CD IN (2,3) AND (3=3) ", distributorCDs);

            if (this.P5sDdlPROVINCE_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" PRO.PROVINCE_CD in ({0})", this.P5sDdlPROVINCE_CD.SelectedValue));
            else
                if (this.P5sDdlAREA_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" ARE.AREA_CD = {0} ", this.P5sDdlAREA_CD.SelectedValue));
            else
                    if (this.P5sDdlREGION_CD.SelectedValue != "-1")
                sql = sql.Replace("(3=3)", String.Format(" REG.REGION_CD = {0}", this.P5sDdlREGION_CD.SelectedValue));


            return L5sSql.Query(sql);

        }

        private DataTable P5sGetDistributorNetworkByProvince(String provinceByArea)
        {
            String provinceCDs = this.P5sDdlPROVINCE_CD.SelectedValue;

            String sql = String.Format(@"                                       

                                    ; WITH CTE AS
                                    (

			                                    Select  DIS.DISTRIBUTOR_CD,PRO.POLYGON as  POLYGON,PRO.PROVINCE_CD as CD,PRO.LONGITUDE_LATITUDE  as LONGITUDE_LATITUDE ,      
					                                    PRO.PROVINCE_NAME_EN AS NAME ,dis.POPULATION_COVERED AS Population,dis.AREA_COVERED AS AREA,
								                                         dis.DSP AS SALES,dis.WHOLE_SALES AS WHOLE_SALES , dis.CUSTOMER_COVERED AS CUSTOMER 
									                                    ,dis.AMS AS AMS, PRO.CENTRAL_POINT
			                                    from M_DISTRIBUTOR DIS INNER JOIN M_COMMUNE COM on DIS.COMMUNE_CD = COM.COMMUNE_CD 
								                                    INNER JOIN M_DISTRICT DIST	on COM.DISTRICT_CD = DIST.DISTRICT_CD
								                                    INNER JOIN M_PROVINCE PRO on DIST.PROVINCE_CD = PRO.PROVINCE_CD
								                                    INNER JOIN M_AREA_PROVINCE AREP on AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
								                                    INNER JOIN M_AREA ARE on ARE.AREA_CD = arep.AREA_CD
								                                    INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
								                                    INNER JOIN M_CUSTOMER C ON DIS.DISTRIBUTOR_CD = C.DISTRIBUTOR_CD
			                                    where  PRO.PROVINCE_CD IN ({0}) and (1=1) AND DIS.ACTIVE = 1 AND C.ACTIVE = 1
			                                    GROUP BY   DIS.DISTRIBUTOR_CD,PRO.POLYGON ,PRO.PROVINCE_CD ,PRO.LONGITUDE_LATITUDE,PRO.PROVINCE_NAME_EN 
					                                      ,dis.POPULATION_COVERED ,dis.AREA_COVERED , dis.DSP ,dis.WHOLE_SALES , dis.CUSTOMER_COVERED
					                                      ,dis.AMS, PRO.CENTRAL_POINT
                                       ) 
                                       SELECT T1.DISTRIBUTOR_CD,T1.POLYGON,T1.CD,T1.LONGITUDE_LATITUDE,T1.NAME,T2.*, '' AS DESCRIPTION , '' AS DISTRIBUTOR_NAME,
                                                T1.CENTRAL_POINT
                                        FROM CTE AS T1 INNER JOIN 
			                                    (
				                                    SELECT CD, SUM(Population) AS Population, SUM(AREA) AS AREA,
						                                    SUM(WHOLE_SALES) AS WHOLE_SALES , SUM(CUSTOMER) AS CUSTOMER, SUM(AMS ) AS AMS  
                                                            ,SUM(SALES) AS SALES   
				                                    FROM CTE
				                                    GROUP BY CD
			                                    ) AS T2 ON T1.CD = T2.CD                                    
                                                                        		 
                                        ", provinceByArea);

            String distributorCDs = "-1";
            if (provinceCDs == "") ;

            if (distributorCDs != "-1")
                sql = sql.Replace("(1=1)", String.Format(" DIS.DISTRIBUTOR_CD in ({0})", distributorCDs));
            return L5sSql.Query(sql);
        }


        private void W5sCreateFunctionDistributorNameByDistrict()
        {
            String sql = "IF NOT  EXISTS (SELECT * FROM dbo.sysobjects WHERE [id] = OBJECT_ID(N'[dbo].[fnsGetDistributorNameByDistrict]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) SELECT 'NOT EXISTS' as 'VALUE' ";
            if (L5sSql.Query(sql).Rows.Count > 0 && L5sSql.Query(sql).Rows[0][0].ToString() == "NOT EXISTS")
            {

                String sqlCreateFunction = @"CREATE FUNCTION [dbo].[fnsGetDistributorNameByDistrict](@DistrictCD BIGINT) 
												RETURNS NVARCHAR(MAX)
											AS
											BEGIN
													DECLARE @RESULT_String nvarchar(max);
													SELECT @RESULT_String = COALESCE(@RESULT_String + ',',' ') + ' ' +  convert(NVARCHAR(max),DIS.DISTRIBUTOR_CODE + '-' +  DIS.DISTRIBUTOR_NAME) + ''

													FROM M_DISTRIBUTOR DIS   
																			INNER JOIN M_CUSTOMER C ON DIS.DISTRIBUTOR_CD = C.DISTRIBUTOR_CD
																			INNER JOIN M_COMMUNE COM ON C.COMMUNE_CD = COM.COMMUNE_CD
																			INNER JOIN M_DISTRICT DIST	ON COM.DISTRICT_CD = DIST.DISTRICT_CD
													WHERE DIST.DISTRICT_CD = @DistrictCD AND DIS.ACTIVE = 1 AND C.ACTIVE = 1
													GROUP BY DIS.DISTRIBUTOR_CODE + '-' +  DIS.DISTRIBUTOR_NAME                                                			
													RETURN @RESULT_String;
											END;  ";

                L5sSql.Execute(sqlCreateFunction);
            }
        }

        private void W5sCreateFunctionDistributorNameByCommune()
        {
            String sql = "IF NOT  EXISTS (SELECT * FROM dbo.sysobjects WHERE [id] = OBJECT_ID(N'[dbo].[fnsGetDistributorNameByCommune]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) SELECT 'NOT EXISTS' as 'VALUE' ";
            if (L5sSql.Query(sql).Rows.Count > 0 && L5sSql.Query(sql).Rows[0][0].ToString() == "NOT EXISTS")
            {

                String sqlCreateFunction = @"	CREATE FUNCTION [dbo].[fnsGetDistributorNameByCommune](@CommuneCD BIGINT) 
																		RETURNS NVARCHAR(MAX)
																AS
														BEGIN
																DECLARE @RESULT_String nvarchar(max);
																SELECT @RESULT_String = COALESCE(@RESULT_String + ',',' ') + ' ' +  convert(NVARCHAR(max),DIS.DISTRIBUTOR_CODE + '-' +  DIS.DISTRIBUTOR_NAME) + ''
   
																FROM M_DISTRIBUTOR DIS   
																						INNER JOIN M_CUSTOMER C ON DIS.DISTRIBUTOR_CD = C.DISTRIBUTOR_CD
																						INNER JOIN M_COMMUNE COM ON C.COMMUNE_CD = COM.COMMUNE_CD
																WHERE COM.COMMUNE_CD = @CommuneCD AND DIS.ACTIVE = 1 AND C.ACTIVE = 1
																GROUP BY DIS.DISTRIBUTOR_CODE + '-' +  DIS.DISTRIBUTOR_NAME                                                			
																RETURN @RESULT_String;
														END;    ";

                L5sSql.Execute(sqlCreateFunction);
            }
        }

        private DataTable P5sGetDistributorNetworkByDistributor(String distributorCDs)
        {


            this.W5sCreateFunctionDistributorNameByDistrict(); // create funstion get distributor name

            String sql = String.Format(@"SELECT T1.*, T2.Population,ISNULL(T2.AREA,0) AS AREA, T3.SALES 
	                                    ,[dbo].[fnsGetDistributorNameByDistrict](T1.CD) AS DISTRIBUTOR_NAME
                                        FROM (
                                            SELECT DIS.DISTRIBUTOR_CD,DIST.POLYGON AS  POLYGON,DIST.DISTRICT_CD AS CD,DIST.LONGITUDE_LATITUDE AS LONGITUDE_LATITUDE   
                                            ,SUM( CASE c.CUSTOMER_CHAIN_CODE WHEN 'WSN' THEN 1 ELSE 0 END) AS WHOLE_SALES ,  
                                            COUNT( *) AS CUSTOMER  , SUM(C.AMS) AS AMS  ,DIST.DISTRICT_NAME_EN AS NAME,
                                            PRO.PROVINCE_NAME_EN AS DESCRIPTION    ,     DIST.CENTRAL_POINT     
                                            FROM M_DISTRIBUTOR DIS INNER JOIN M_CUSTOMER C ON DIS.DISTRIBUTOR_CD = C.DISTRIBUTOR_CD
                                                                INNER JOIN M_COMMUNE COM ON C.COMMUNE_CD = COM.COMMUNE_CD
                                                                INNER JOIN M_DISTRICT DIST	ON COM.DISTRICT_CD = DIST.DISTRICT_CD
					                                            INNER JOIN M_PROVINCE PRO ON DIST.PROVINCE_CD = PRO.PROVINCE_CD
                                                                INNER JOIN M_AREA_PROVINCE AREP ON AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
				                                                INNER JOIN M_AREA ARE ON ARE.AREA_CD = arep.AREA_CD
				                                                INNER JOIN M_REGION REG ON ARE.REGION_CD = REG.REGION_CD
                                            WHERE (1=1)  AND C.COMMUNE_CD IS NOT NULL   AND DIS.ACTIVE = 1  AND C.ACTIVE = 1                                                                   
                                            GROUP BY   DIS.DISTRIBUTOR_CD,DIST.POLYGON   ,DIST.DISTRICT_CD ,DIST.LONGITUDE_LATITUDE,DIST.DISTRICT_NAME_EN 
                                                       ,PRO.PROVINCE_NAME_EN,DIST.CENTRAL_POINT  
                                            ) AS T1 INNER JOIN 
                                              (
												SELECT com.DISTRICT_CD,SUM( com.POPULATION) AS Population,SUM(com.AREA) AS AREA
	                                            FROM M_COMMUNE com 
	                                            WHERE  exists (SELECT * FROM M_CUSTOMER  cus WHERE cus.COMMUNE_CD = com.COMMUNE_CD AND cus.ACTIVE = 1)
	                                            Group by com.DISTRICT_CD  
											   ) AS T2 ON T1.CD = T2.DISTRICT_CD  INNER JOIN (

																							   SELECT DISTRICT_CD, COUNT(DISTINCT sls.SALES_CD) AS SALES 
																							   FROM 
																									  M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE custR on cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
																									  INNER JOIN O_SALES_ROUTE slsR ON custR.ROUTE_CD = slsR.ROUTE_CD AND slsR.ACTIVE = 1
																									  INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD
																									  INNER JOIN M_SALES sls ON slsR.SALES_CD = sls.SALES_CD

																								WHERE DISTRICT_CD IS NOT NULL AND cust.ACTIVE = 1
																								GROUP BY DISTRICT_CD
																							   ) AS T3 ON T1.CD = T3.DISTRICT_CD ");


            sql = sql.Replace("(1=1)", String.Format(" DIS.DISTRIBUTOR_CD in ({0}) ", distributorCDs));


            return L5sSql.Query(sql);
        }


        private DataTable P5sGetDistributorNetworkByDistrict(String provinceByArea)
        {

            String provinceCDs = this.P5sDdlPROVINCE_CD.SelectedValue;

            this.W5sCreateFunctionDistributorNameByDistrict(); // create funstion get distributor name

            String sql = String.Format(@"SELECT T1.*, T2.Population,ISNULL(T2.AREA,0) AS AREA, T3.SALES 
	                                    ,[dbo].[fnsGetDistributorNameByDistrict](T1.CD) AS DISTRIBUTOR_NAME
                                        FROM (
                                            SELECT DIS.DISTRIBUTOR_CD,DIST.POLYGON AS  POLYGON,DIST.DISTRICT_CD AS CD,DIST.LONGITUDE_LATITUDE AS LONGITUDE_LATITUDE   
                                            ,SUM( CASE c.CUSTOMER_CHAIN_CODE WHEN 'WSN' THEN 1 ELSE 0 END) AS WHOLE_SALES ,  
                                            COUNT( *) AS CUSTOMER  , SUM(C.AMS) AS AMS  ,DIST.DISTRICT_NAME_EN AS NAME,
                                            PRO.PROVINCE_NAME_EN AS DESCRIPTION    ,     DIST.CENTRAL_POINT     
                                            FROM M_DISTRIBUTOR DIS INNER JOIN M_CUSTOMER C ON DIS.DISTRIBUTOR_CD = C.DISTRIBUTOR_CD
                                                                INNER JOIN M_COMMUNE COM ON C.COMMUNE_CD = COM.COMMUNE_CD
                                                                INNER JOIN M_DISTRICT DIST	ON COM.DISTRICT_CD = DIST.DISTRICT_CD
					                                            INNER JOIN M_PROVINCE PRO ON DIST.PROVINCE_CD = PRO.PROVINCE_CD
                                                                INNER JOIN M_AREA_PROVINCE AREP ON AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
				                                                INNER JOIN M_AREA ARE ON ARE.AREA_CD = arep.AREA_CD
				                                                INNER JOIN M_REGION REG ON ARE.REGION_CD = REG.REGION_CD
                                            WHERE (1=1)  AND C.COMMUNE_CD IS NOT NULL   AND DIS.ACTIVE = 1  AND C.ACTIVE = 1                                                                   
                                            GROUP BY   DIS.DISTRIBUTOR_CD,DIST.POLYGON   ,DIST.DISTRICT_CD ,DIST.LONGITUDE_LATITUDE,DIST.DISTRICT_NAME_EN 
                                                       ,PRO.PROVINCE_NAME_EN,DIST.CENTRAL_POINT  
                                            ) AS T1 INNER JOIN 
                                              (
												SELECT com.DISTRICT_CD,SUM( com.POPULATION) AS Population,SUM(com.AREA) AS AREA
	                                            FROM M_COMMUNE com 
	                                            WHERE  exists (SELECT * FROM M_CUSTOMER  cus WHERE cus.COMMUNE_CD = com.COMMUNE_CD AND cus.ACTIVE = 1)
	                                            Group by com.DISTRICT_CD  
											   ) AS T2 ON T1.CD = T2.DISTRICT_CD  INNER JOIN (
																							   SELECT cust.DISTRICT_CD, COUNT(DISTINCT sls.SALES_CD) AS SALES 
																							   FROM 
																									  M_CUSTOMER cust INNER JOIN O_CUSTOMER_ROUTE custR on cust.CUSTOMER_CD = custR.CUSTOMER_CD AND custR.ACTIVE = 1
																									  INNER JOIN O_SALES_ROUTE slsR ON custR.ROUTE_CD = slsR.ROUTE_CD AND slsR.ACTIVE = 1
																									  INNER JOIN M_ROUTE rout ON slsR.ROUTE_CD = rout.ROUTE_CD
																									  INNER JOIN M_SALES sls ON slsR.SALES_CD = sls.SALES_CD

																								WHERE cust.DISTRICT_CD IS NOT NULL AND cust.ACTIVE = 1
																								GROUP BY cust.DISTRICT_CD
																							   ) AS T3 ON T1.CD = T3.DISTRICT_CD  ");

            if (this.P5sDdlPROVINCE_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" PRO.PROVINCE_CD in ({0})", this.P5sDdlPROVINCE_CD.SelectedValue));
            else
                if (this.P5sDdlAREA_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" ARE.AREA_CD = {0} ", this.P5sDdlAREA_CD.SelectedValue));
            else
                    if (this.P5sDdlREGION_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" REG.REGION_CD =  {0}", this.P5sDdlREGION_CD.SelectedValue));

            return L5sSql.Query(sql);
        }


        private DataTable P5sGetDistributorNetworkByCommune(String provinceByArea)
        {
            String provinceCDs = this.P5sDdlPROVINCE_CD.SelectedValue;

            this.W5sCreateFunctionDistributorNameByCommune();// create funstion get distributor name

            String sql = String.Format(@"SELECT DIS.DISTRIBUTOR_CD,COM.POLYGON AS  POLYGON,COM.COMMUNE_CD  AS CD,COM.LONGITUDE_LATITUDE   AS LONGITUDE_LATITUDE   
		                                    ,SUM( CASE c.CUSTOMER_CHAIN_CODE WHEN 'WSN' THEN 1 ELSE 0 END) AS WHOLE_SALES,
		                                    COUNT( *) AS CUSTOMER,com.POPULATION ,ISNULL(com.AREA,0) AREA  , SUM(C.AMS) AS AMS   ,COM.COMMUNE_NAME_EN AS NAME , COUNT(DISTINCT sls.SALES_CD) AS SALES                                                                          
                                            ,DIST.DISTRICT_NAME_EN + ',' + PRO.PROVINCE_NAME_EN AS DESCRIPTION ,
                                            dbo.[fnsGetDistributorNameByCommune](COM.COMMUNE_CD) AS DISTRIBUTOR_NAME,
                                                COM.CENTRAL_POINT  
                                    FROM M_DISTRIBUTOR DIS  INNER JOIN M_SALES sls ON DIS.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
									     INNER JOIN O_SALES_ROUTE slsR ON slsR.SALES_CD = sls.SALES_CD AND slsR.ACTIVE = 1
										 INNER JOIN O_CUSTOMER_ROUTE custR on custR.ROUTE_CD = slsR.ROUTE_CD AND custR.ACTIVE = 1									
										 INNER JOIN M_CUSTOMER C ON custR.CUSTOMER_CD = C.CUSTOMER_CD
                                        INNER JOIN M_COMMUNE COM ON C.COMMUNE_CD = COM.COMMUNE_CD
                                        INNER JOIN M_DISTRICT DIST	ON COM.DISTRICT_CD = DIST.DISTRICT_CD
					                    INNER JOIN M_PROVINCE PRO ON DIST.PROVINCE_CD = PRO.PROVINCE_CD
						                INNER JOIN M_AREA_PROVINCE AREP ON AREP.PROVINCE_CD = PRO.PROVINCE_CD AND AREP.ACTIVE = 1
				                        INNER JOIN M_AREA ARE ON ARE.AREA_CD = arep.AREA_CD
				                        INNER JOIN M_REGION REG ON ARE.REGION_CD = REG.REGION_CD    
                                    WHERE (1=1)  AND DIS.ACTIVE = 1    AND C.ACTIVE = 1                                       
                                    GROUP BY DIS.DISTRIBUTOR_CD,COM.POLYGON ,COM.COMMUNE_CD,COM.LONGITUDE_LATITUDE,com.POPULATION,com.AREA,COM.COMMUNE_NAME_EN,DIST.DISTRICT_NAME_EN + ',' + PRO.PROVINCE_NAME_EN
                                            , COM.CENTRAL_POINT  ");

            if (this.P5sDdlPROVINCE_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" PRO.PROVINCE_CD in ({0})", this.P5sDdlPROVINCE_CD.SelectedValue));
            else
                if (this.P5sDdlAREA_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" ARE.AREA_CD = {0} ", this.P5sDdlAREA_CD.SelectedValue));
            else
                    if (this.P5sDdlREGION_CD.SelectedValue != "-1")
                sql = sql.Replace("(1=1)", String.Format(" REG.REGION_CD =  {0}", this.P5sDdlREGION_CD.SelectedValue));
            return L5sSql.Query(sql);
        }


        #endregion
        #region Json Object

        private String P5sConvertDtableLongTudeToJson(DataTable dt)
        {
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";


            List<CLONGITUDE_LATITUDE> longTudes = new List<CLONGITUDE_LATITUDE>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                String cd = dt.Rows[i]["CD"].ToString();
                String name = dt.Rows[i]["NAME"].ToString();
                String dsp = dt.Rows[i]["SALES"].ToString();
                String distributorCD = dt.Rows[i]["DISTRIBUTOR_CD"].ToString();
                String polygon = dt.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                String population = dt.Rows[i]["POPULATION"].ToString();
                String wholesales = dt.Rows[i]["WHOLE_SALES"].ToString();
                String customer = dt.Rows[i]["CUSTOMER"].ToString();
                String area = dt.Rows[i]["AREA"].ToString();
                String ams = dt.Rows[i]["AMS"].ToString();
                String description = dt.Rows[i]["DESCRIPTION"].ToString();
                String distributorDesc = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();
                if (polygon != "")
                {
                    CLONGITUDE_LATITUDE c = new CLONGITUDE_LATITUDE(name, distributorCD, cd, polygon, wholesales, customer, population, area, ams, dsp, description, distributorDesc);
                    bool flag = longTudes.Find(delegate (CLONGITUDE_LATITUDE x) { return x.CD == cd; }) != null;
                    if (!flag)
                        longTudes.Add(c);
                }
            }
            return oSerializer.Serialize(longTudes);
        }


        private String P5sConvertDtableCentralPointToJson(DataTable dt)
        {
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";


            List<CLONGITUDE_LATITUDE> longTudes = new List<CLONGITUDE_LATITUDE>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                String cd = dt.Rows[i]["CD"].ToString();
                String name = dt.Rows[i]["NAME"].ToString();
                String dsp = dt.Rows[i]["SALES"].ToString();
                String distributorCD = dt.Rows[i]["DISTRIBUTOR_CD"].ToString();
                String polygon = dt.Rows[i]["CENTRAL_POINT"].ToString();
                String population = dt.Rows[i]["POPULATION"].ToString();
                String wholesales = dt.Rows[i]["WHOLE_SALES"].ToString();
                String customer = dt.Rows[i]["CUSTOMER"].ToString();
                String area = dt.Rows[i]["AREA"].ToString();
                String ams = dt.Rows[i]["AMS"].ToString();
                String description = dt.Rows[i]["DESCRIPTION"].ToString();
                String distributorDesc = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();
                if (polygon != "")
                {
                    CLONGITUDE_LATITUDE c = new CLONGITUDE_LATITUDE(name, distributorCD, cd, polygon, wholesales, customer, population, area, ams, dsp, description, distributorDesc);
                    bool flag = longTudes.Find(delegate (CLONGITUDE_LATITUDE x) { return x.CD == cd; }) != null;
                    if (!flag)
                        longTudes.Add(c);
                }
            }
            return oSerializer.Serialize(longTudes);
        }

        private String P5sConvertDtableToJson(DataTable dt)
        {
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";


            List<CPolygon> polygons = new List<CPolygon>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String polygon = dt.Rows[i]["POLYGON"].ToString();
                if (polygon != "")
                {
                    CPolygon c = new CPolygon(polygon);
                    polygons.Add(c);
                }
            }
            return oSerializer.Serialize(polygons);
        }

        private String P5sConvertDtablePOIToJson(DataTable dt)
        {
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            List<CPOI> pois = new List<CPOI>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                String POIS_NAME = dt.Rows[i]["POIS_NAME"].ToString();
                String POIS_ADDRESS = dt.Rows[i]["POIS_ADDRESS"].ToString();
                String POIS_LONGITUDE_LATITUDE = dt.Rows[i]["POIS_LONGITUDE_LATITUDE"].ToString();
                String POIS_PHONE = dt.Rows[i]["POIS_PHONE"].ToString();
                String POIS_DESCRIPTION = dt.Rows[i]["POIS_DESCRIPTION"].ToString();


                if (POIS_LONGITUDE_LATITUDE != "")
                {
                    CPOI c = new CPOI(POIS_NAME, POIS_ADDRESS, POIS_LONGITUDE_LATITUDE, POIS_PHONE, POIS_DESCRIPTION);
                    pois.Add(c);
                }
            }
            return oSerializer.Serialize(pois);
        }


        private String P5sConvertDataTableCoveragedJson(DataTable dtable)
        {
            DataTable dt = dtable;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            List<CCommune> communes = new List<CCommune>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            for (int i = 0; i < dt.Rows.Count; i++)
            {


                String communeName = dt.Rows[i]["COMMUNE_NAME_EN"].ToString();
                String communeLongTude = dt.Rows[i]["COM_LONGITUDE_LATITUDE"].ToString();
                String communeAddress = dt.Rows[i]["FULL_ADDRESS"].ToString();
                String communePopulation = dt.Rows[i]["POPULATION"].ToString();
                String distributorCD = dt.Rows[i]["DISTRIBUTOR_CD"].ToString();
                String distributorName = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();
                String distributorDistance = dt.Rows[i]["DISTRIBUTOR_DISTANCE"].ToString();
                String townCoverName = dt.Rows[i]["TOWN_COVERED_NAME"].ToString();
                String townCoverDistance = dt.Rows[i]["TOWN_COVERED_DISTANCE"].ToString();
                String communeCoverName = dt.Rows[i]["COMMUNE_COVERED_NAME"].ToString();
                String communeCoverDistance = dt.Rows[i]["COMMUNE_COVERED_DISTANCE"].ToString();
                String totalCustomer = dt.Rows[i]["TOTAL_CUSTOMER"].ToString();
                String ams = dt.Rows[i]["AMS"].ToString();
                String totalWholeSales = dt.Rows[i]["WHOLE_SALES"].ToString();
                String communeTitle = dt.Rows[i]["TITLE_DIRECTION"].ToString();
                String distributorLongTue = dt.Rows[i]["LONGITUDE_LATITUDE_DISTRIBUTOR"].ToString();
                String communeType = dt.Rows[i]["COMMUNE_TYPE"].ToString();
                String districtCD = dt.Rows[i]["DISTRICT_CD"].ToString();

                if (communeLongTude != "")
                {

                    CCommune c = new CCommune(communeName, communeLongTude, communeAddress, communePopulation, distributorName, distributorDistance, townCoverName, townCoverDistance, communeCoverName, communeCoverDistance
                                                    , totalCustomer, ams, totalWholeSales, communeTitle, distributorLongTue, distributorCD, communeType, districtCD);

                    communes.Add(c);
                }
            }
            return oSerializer.Serialize(communes);
        }
        private String P5sConvertDataTableDistributorJson(DataTable dtable)
        {
            DataTable dt = dtable;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            List<CDistributor> distributors = new List<CDistributor>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String Id = dt.Rows[i]["DISTRIBUTOR_CD"].ToString();
                String Name = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();
                String LONGITUDE_LATITUDE = dt.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                String FULL_ADDRESS = dt.Rows[i]["FULL_ADDRESS"].ToString();
                String POPULATION = dt.Rows[i]["POPULATION_COVERED"].ToString();

                String RADIUS = dt.Rows[i]["RADIUS"].ToString();
                String AREA_COVERED = dt.Rows[i]["AREA_COVERED"].ToString();
                String CUSTOMERS = dt.Rows[i]["CUSTOMER"].ToString();
                String DSP = dt.Rows[i]["DSP"].ToString();
                String WHOLE_SALES = dt.Rows[i]["WHOLE_SALES"].ToString();
                String AMS = dt.Rows[i]["AMS"].ToString();
                String distributorType = dt.Rows[i]["DISTRIBUTOR_TYPE_CD"].ToString();
                String parentName = dt.Rows[i]["DISTRIBUTOR_PARENT"].ToString();

                String distributorTypeImage = dt.Rows[i]["DISTRIBUTOR_TYPE_IMAGE"].ToString();

                if (LONGITUDE_LATITUDE != "")
                {

                    CDistributor d = new CDistributor(Id, Name, LONGITUDE_LATITUDE, FULL_ADDRESS, POPULATION,
                                    RADIUS, AREA_COVERED, CUSTOMERS, WHOLE_SALES,
                                    DSP, AMS, distributorType, parentName, distributorTypeImage);
                    distributors.Add(d);
                }
            }
            return oSerializer.Serialize(distributors);
        }
        private String P5sConvertDataTableUnCoveragedJson(DataTable dtable)
        {
            DataTable dt = dtable;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            List<CCommune> communes = new List<CCommune>();
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                         new System.Web.Script.Serialization.JavaScriptSerializer();
            oSerializer.MaxJsonLength = Int32.MaxValue;

            for (int i = 0; i < dt.Rows.Count; i++)
            {


                String Name = dt.Rows[i]["COMMUNE_NAME_EN"].ToString();
                String LONGITUDE_LATITUDE = dt.Rows[i]["COM_LONGITUDE_LATITUDE"].ToString();
                String FULL_ADDRESS = dt.Rows[i]["FULL_ADDRESS"].ToString();
                String POPULATION = dt.Rows[i]["POPULATION"].ToString();
                String DISTRIBUTOR_NAME = dt.Rows[i]["DISTRIBUTOR_NAME"].ToString();
                String DISTRIBUTOR_DISTANCE = dt.Rows[i]["DISTRIBUTOR_DISTANCE"].ToString();
                String TOWN_COVERED_NAME = dt.Rows[i]["TOWN_COVERED_NAME"].ToString();
                String TOWN_COVERED_DISTANCE = dt.Rows[i]["TOWN_COVERED_DISTANCE"].ToString();
                String COMMUNE_COVERED_NAME = dt.Rows[i]["COMMUNE_COVERED_NAME"].ToString();
                String COMMUNE_COVERED_DISTANCE = dt.Rows[i]["COMMUNE_COVERED_DISTANCE"].ToString();
                String TITLE_DIRECTION = dt.Rows[i]["TITLE_DIRECTION"].ToString();
                String districtCD = dt.Rows[i]["DISTRICT_CD"].ToString();

                if (LONGITUDE_LATITUDE != "")
                {

                    CCommune c = new CCommune(Name, LONGITUDE_LATITUDE, FULL_ADDRESS, POPULATION, DISTRIBUTOR_NAME, DISTRIBUTOR_DISTANCE, TOWN_COVERED_NAME, TOWN_COVERED_DISTANCE, COMMUNE_COVERED_NAME, COMMUNE_COVERED_DISTANCE
                                               , TITLE_DIRECTION, districtCD);

                    communes.Add(c);
                }
            }
            return oSerializer.Serialize(communes);
        }
        private long P5sCountRow(DataTable dtable, String ColumnName, String v)
        {
            long result = 0;
            for (int i = 0; i < dtable.Rows.Count; i++)
            {
                if (dtable.Rows[i][ColumnName].ToString() == v)
                    result++;
            }
            return result;
        }

        #endregion

        #region POIS


        private Boolean P5sCheckBoxChecked()
        {
            //P5sChkBank
            //P5sChkATM
            //P5sChkStateHighway
            //P5sChkSchool
            //P5sChkPostOffice
            //P5sChkNationlHighway
            //P5sChkBSBF
            //P5sChkHospital
            //P5sChkMarket 
            CheckBox[] arr = new CheckBox[] { this.P5sChkBank, this.P5sChkATM, this.P5sChkStateHighway, this.P5sChkSchool, this.P5sChkPostOffice, this.P5sChkNationlHighway, this.P5sChkBSBF, this.P5sChkHospital, this.P5sChkMarket };
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Checked)
                    return true;
            }
            return false;
        }



        private void P5sPOIs(String communeCds)
        {
            //P5sChkBank
            //P5sChkATM
            //P5sChkStateHighway
            //P5sChkSchool
            //P5sChkPostOffice
            //P5sChkNationlHighway
            //P5sChkBSBF
            //P5sChkHospital
            if (!this.P5sCheckBoxChecked())
            {
                this.P5sPnlParentPOIS.Visible = false;
                return;
            }

            this.P5sPnlParentPOIS.Visible = true;


            if (communeCds.Trim() == "")
                communeCds = "-1";

            this.P5sPnlParentPOIS.Visible = true;

            this.P5sPnlBank.Visible = false;
            this.P5sPnlATM.Visible = false;
            this.P5sPnlStateHighway.Visible = false;
            this.P5sPnlSchool.Visible = false;
            this.P5sPnlNationlHighway.Visible = false;
            this.P5sPnlBSBF.Visible = false;
            this.P5sPnlHospital.Visible = false;
            this.P5sPnlPostOffice.Visible = false;
            this.P5sPnlMarket.Visible = false;

            //P5sLblTotalPOIs

            //

            //P5sPnlBank
            //P5sLblTotalBank
            Int32 TotalPOIs = 0;
            if (this.P5sChkBank.Checked)
            {
                String sqlBanking = String.Format(@"select POIS_NAME ,POIS_ADDRESS ,POIS_LONGITUDE_LATITUDE,POIS_PHONE ,POIS_DESCRIPTION  from M_POIS where COMMUNE_CD in ({0}) and POIS_TYPE = 1", communeCds);
                DataTable dtBanking = L5sSql.Query(sqlBanking);
                String objectBanking = this.P5sConvertDtablePOIToJson(dtBanking);
                this.P5sPnlBank.Visible = true;
                this.P5sLblTotalBank.Text = dtBanking.Rows.Count + "";
                TotalPOIs += dtBanking.Rows.Count;

                if (dtBanking.Rows.Count > 0)
                    L5sJS.L5sRun("AddBanking('" + objectBanking + "')");

            }

            //P5sPnlATM
            //P5sLblTotalATM
            if (this.P5sChkATM.Checked)
            {

                String sqlATM = String.Format(@"select POIS_NAME ,POIS_ADDRESS ,POIS_LONGITUDE_LATITUDE,POIS_PHONE ,POIS_DESCRIPTION  from M_POIS where COMMUNE_CD in ({0}) and POIS_TYPE = 2", communeCds);
                DataTable dtATM = L5sSql.Query(sqlATM);
                String objectATM = this.P5sConvertDtablePOIToJson(dtATM);
                this.P5sPnlATM.Visible = true;
                this.P5sLblTotalATM.Text = dtATM.Rows.Count + "";
                TotalPOIs += dtATM.Rows.Count;

                if (dtATM.Rows.Count > 0)
                    L5sJS.L5sRun("AddATM('" + objectATM + "')");

            }


            //P5sPnlHospital
            //P5sLblHospital
            if (this.P5sChkHospital.Checked)
            {

                String sqlHospital = String.Format(@"select POIS_NAME ,POIS_ADDRESS ,POIS_LONGITUDE_LATITUDE,POIS_PHONE ,POIS_DESCRIPTION  from M_POIS where COMMUNE_CD in ({0}) and POIS_TYPE = 3", communeCds);
                DataTable dtHospital = L5sSql.Query(sqlHospital);
                String objectHospital = this.P5sConvertDtablePOIToJson(dtHospital);
                this.P5sPnlHospital.Visible = true;
                this.P5sLblHospital.Text = dtHospital.Rows.Count + "";
                TotalPOIs += dtHospital.Rows.Count;

                if (dtHospital.Rows.Count > 0)
                    L5sJS.L5sRun("AddHospital('" + objectHospital + "')");
            }


            //P5sPnlPostOffice
            //P5sLblPostOffice
            if (this.P5sChkPostOffice.Checked)
            {
                String sqlPostOffice = String.Format(@"select POIS_NAME ,POIS_ADDRESS ,POIS_LONGITUDE_LATITUDE,POIS_PHONE ,POIS_DESCRIPTION  from M_POIS where COMMUNE_CD in ({0}) and POIS_TYPE = 4", communeCds);
                DataTable dtPostOffice = L5sSql.Query(sqlPostOffice);
                String objectPostOffice = this.P5sConvertDtablePOIToJson(dtPostOffice);
                this.P5sPnlPostOffice.Visible = true;
                this.P5sLblPostOffice.Text = dtPostOffice.Rows.Count + "";
                TotalPOIs += dtPostOffice.Rows.Count;

                if (dtPostOffice.Rows.Count > 0)
                    L5sJS.L5sRun("AddPostOffice('" + objectPostOffice + "')");
            }

            //P5sPnlSchool
            //P5sLblTotalSchool
            if (this.P5sChkSchool.Checked)
            {
                String sqlSchool = String.Format(@"select POIS_NAME ,POIS_ADDRESS ,POIS_LONGITUDE_LATITUDE,POIS_PHONE ,POIS_DESCRIPTION  from M_POIS where COMMUNE_CD in ({0}) and POIS_TYPE = 5", communeCds);
                DataTable dtSchool = L5sSql.Query(sqlSchool);
                String objectSchool = this.P5sConvertDtablePOIToJson(dtSchool);
                this.P5sPnlSchool.Visible = true;
                this.P5sLblTotalSchool.Text = dtSchool.Rows.Count + "";
                TotalPOIs += dtSchool.Rows.Count;

                if (dtSchool.Rows.Count > 0)
                    L5sJS.L5sRun("AddSchool('" + objectSchool + "')");

            }

            //Market
            if (this.P5sChkMarket.Checked)
            {
                String sqlMarket = String.Format(@"select POIS_NAME ,POIS_ADDRESS ,POIS_LONGITUDE_LATITUDE,POIS_PHONE ,POIS_DESCRIPTION  from M_POIS where COMMUNE_CD in ({0}) and POIS_TYPE = 9", communeCds);
                DataTable dtMarket = L5sSql.Query(sqlMarket);
                String objectMarket = this.P5sConvertDtablePOIToJson(dtMarket);
                this.P5sPnlMarket.Visible = true;
                this.P5sLblTotalMarket.Text = dtMarket.Rows.Count + "";
                TotalPOIs += dtMarket.Rows.Count;

                if (dtMarket.Rows.Count > 0)
                    L5sJS.L5sRun("AddMarket('" + objectMarket + "')");

            }

            //P5sPnlStateHighway
            //P5sLblTotalStateHighway
            //if (this.P5sChkStateHighway.Checked)
            //{
            //    this.P5sPnlStateHighway.Visible = true;
            //    this.P5sLblTotalStateHighway.Text = "0";
            //}


            //P5sPnlNationlHighway
            //P5sLblNationlHighway
            //if (this.P5sChkNationlHighway.Checked)
            //{
            //    this.P5sPnlNationlHighway.Visible = true;
            //    this.P5sLblNationlHighway.Text = "0";
            //}

            //P5sPnlBSBF
            //P5sLblBSBF
            if (this.P5sChkBSBF.Checked)
            {
                this.P5sPnlBSBF.Visible = true;
                this.P5sLblBSBF.Text = "0";
            }


            this.P5sLblTotalPOIs.Text = String.Format("Total POIs [{0}]:", TotalPOIs);
        }
        #endregion

        #region Polygon

        public DataTable P5sGetPolygonCommuneByCD(String CDs)
        {
            if (CDs.Trim().Length <= 0)
                return null;

            return L5sSql.Query(String.Format(@"select POLYGON from M_COMMUNE where COMMUNE_CD in ({0}) ", CDs));

        }

        public DataTable P5sGetPolygonDistrictByCD(String CDs)
        {
            if (CDs.Trim().Length <= 0)
                return null;

            return L5sSql.Query(String.Format(@"select POLYGON from M_DISTRICT where DISTRICT_CD in ({0}) ", CDs));

        }

        private void P5sProcessPolygon(DataTable ws, DataTable wws, DataTable un, String strCommunesNoSatisfy)
        {
            #region WS
            String strCommuneCDs = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(ws, "COMMUNE_CD");
            DataTable dtable = this.P5sGetPolygonCommuneByCD(strCommuneCDs);
            String objecJson = this.P5sConvertDtableToJson(dtable);
            L5sJS.L5sRun("AddPWS('" + objecJson + "')");

            #endregion

            #region WWS
            strCommuneCDs = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(wws, "COMMUNE_CD");
            dtable = this.P5sGetPolygonCommuneByCD(strCommuneCDs);
            objecJson = this.P5sConvertDtableToJson(dtable);
            L5sJS.L5sRun("AddPWWS('" + objecJson + "')");
            #endregion

            #region Uncover
            strCommuneCDs = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(un, "COMMUNE_CD");
            dtable = this.P5sGetPolygonCommuneByCD(strCommuneCDs);
            objecJson = this.P5sConvertDtableToJson(dtable);
            L5sJS.L5sRun("AddPUnCover('" + objecJson + "')");
            #endregion


            #region Not Satisfy condition
            dtable = this.P5sGetPolygonCommuneByCD(strCommunesNoSatisfy);
            objecJson = this.P5sConvertDtableToJson(dtable);
            L5sJS.L5sRun("AddNoSatisfy('" + objecJson + "')");
            #endregion


        }

        #endregion


        private String P5sGetProvinceCdByArea(String regionCDs, ref String areaCDs)
        {
            String provinceCDByArea = this.P5sDdlPROVINCE_CD.SelectedValue;
            String sql = "";
            if (regionCDs == "-1")
            {
                provinceCDByArea = P5sCmmFns.P5sConvertDataTableToListStr("select PROVINCE_CD from M_AREA_PROVINCE Where Active = 1");
                areaCDs = P5sCmmFns.P5sConvertDataTableToListStr("select AREA_CD from M_AREA_PROVINCE where  AREA_CD is not null And ACTIVE = 1");
            }
            else
                if (areaCDs == "-1")
            {
                sql = String.Format(@"select distinct  PROVINCE_CD from M_AREA_PROVINCE AREP INNER JOIN M_AREA ARE on AREP.AREA_CD = ARE.AREA_CD AND AREP.ACTIVE = 1
                                          INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
                            where REG.REGION_CD = {0}", regionCDs);
                provinceCDByArea = P5sCmmFns.P5sConvertDataTableToListStr(sql);
                areaCDs = P5sCmmFns.P5sConvertDataTableToListStr(String.Format(@"select distinct AREA_CD from M_AREA
                                                                        where REGION_CD  = {0} and AREA_CD is not null", regionCDs));
            }
            else
                    if (provinceCDByArea == "")
            {
                sql = String.Format(@"select distinct PROVINCE_CD from M_AREA_PROVINCE AREP INNER JOIN M_AREA ARE on AREP.AREA_CD = ARE.AREA_CD AND AREP.ACTIVE = 1
                                              INNER JOIN M_REGION REG on ARE.REGION_CD = REG.REGION_CD
                                where ARE.AREA_CD = {0}", areaCDs);
                provinceCDByArea = P5sCmmFns.P5sConvertDataTableToListStr(sql);
            }

            return provinceCDByArea;

        }


        private void P5sProcessPolyline(String provinceCDByArea)
        {
            //String sql = String.Format(@"select POLYGON from M_PROVINCE where PROVINCE_CD in ({0})", provinceCDByArea);
            //DataTable dt = L5sSql.Query(sql);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{                
            //    String polygon = dt.Rows[i]["POLYGON"].ToString();
            //    L5sJS.L5sRun("AddPolyline('" + polygon + "')");
            //}         
        }
        protected void P5sLbtnSearch_Click(object sender, EventArgs e)
        {
            Int64 pMin = 0;
            Int64 pMax = 0;
            if (!this.P5sCheckPopulation(out pMin, out pMax))
                return;

            #region WS + WWS + UnCover


            #region WS
            //coverageWS là những commune có tồn tại KH có CUSTOMER_CHAIN_CODE = WSN 
            DataTable dtableWS = this.P5sGetWS(pMin, pMax);
            String objectWS = this.P5sConvertDataTableCoveragedJson(dtableWS);
            String distributorCDs = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dtableWS, "DISTRIBUTOR_CD");
            String strCommunes = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dtableWS, "COMMUNE_CD");



            Int64 TotalWS = dtableWS.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }).Rows.Count;
            Int64 TotalWardWS = this.P5sCountRow(dtableWS.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }), "COMMUNE_TYPE", "1");
            Int64 TotalCommuneWS = this.P5sCountRow(dtableWS.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }), "COMMUNE_TYPE", "3");
            Int64 TotalTownWS = TotalWS - TotalCommuneWS - TotalWardWS;


            #endregion

            #region WWS
            //coverageWS là những commune kh tồn tại KH có CUSTOMER_CHAIN_CODE = WSN 
            DataTable dtableWWS = this.P5sGetWWS(pMin, pMax);
            distributorCDs += "," + P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dtableWWS, "DISTRIBUTOR_CD");
            strCommunes += "," + P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dtableWWS, "COMMUNE_CD");


            String objectWWS = this.P5sConvertDataTableCoveragedJson(dtableWWS);


            //xử lý TH 1 comment được map là WS và WWS (ưu tiên WS)
            DataTable dtWWSNotInWS = new DataTable();
            dtWWSNotInWS = dtableWWS.Copy();

            for (int i = dtWWSNotInWS.Rows.Count - 1; i >= 0; i--)
            {
                if (dtableWS.Select("COMMUNE_CD" + " = " + dtWWSNotInWS.Rows[i]["COMMUNE_CD"].ToString()).Length > 0)
                {
                    dtWWSNotInWS.Rows.Remove(dtWWSNotInWS.Rows[i]);
                }
            }



            Int64 TotalWWS = dtWWSNotInWS.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }).Rows.Count;
            Int64 TotalWardWWS = this.P5sCountRow(dtWWSNotInWS.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }), "COMMUNE_TYPE", "1");
            Int64 TotalCommuneWWS = this.P5sCountRow(dtWWSNotInWS.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }), "COMMUNE_TYPE", "3");
            Int64 TotalTownWWS = TotalWWS - TotalCommuneWWS - TotalWardWWS;
            #endregion



            #region UnCover
            DataTable dtableUnCover = this.P5sGetUnCoveraged(pMin, pMax);
            String objectUnCover = this.P5sConvertDataTableUnCoveragedJson(dtableUnCover);
            strCommunes += "," + P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dtableUnCover, "COMMUNE_CD");

            Int64 TotalUnCover = dtableUnCover.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }).Rows.Count;
            Int64 TotalWardUnCover = this.P5sCountRow(dtableUnCover.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }), "COMMUNE_TYPE", "1");
            Int64 TotalCommuneUnCover = this.P5sCountRow(dtableUnCover.DefaultView.ToTable(true, new String[] { "COMMUNE_CD", "COMMUNE_TYPE" }), "COMMUNE_TYPE", "3");
            Int64 TotalTownUnCover = TotalUnCover - TotalCommuneUnCover - TotalWardUnCover;

            #endregion


            //Total All

            this.P5slblTotalWard.Text = TotalWardWS + TotalWardWWS + TotalWardUnCover + "";
            this.P5slblTotalTowns.Text = TotalTownWS + TotalTownWWS + TotalTownUnCover + "";
            this.P5slblTotalCommunes.Text = TotalCommuneWS + TotalCommuneWWS + TotalCommuneUnCover + "";


            //Total Covered
            this.P5slblCoveredWardWS.Text = TotalWardWS + TotalWardWWS + "";
            this.P5slblCoveredTownsWS.Text = TotalTownWS + TotalTownWWS + "";
            this.P5slblCoveredCommunesWS.Text = TotalCommuneWS + TotalCommuneWWS + "";


            //Covered with WS
            this.P5slblCoveredWardWS.Text = TotalWardWS.ToString();
            this.P5slblCoveredTownsWS.Text = TotalTownWS.ToString();
            this.P5slblCoveredCommunesWS.Text = TotalCommuneWS.ToString();

            //Covered without WS
            this.P5slblCoveredWardWithoutWS.Text = TotalWardWWS.ToString();
            this.P5slblCoveredTownsWithoutWS.Text = TotalTownWWS.ToString();
            this.P5slblCoveredCommunesWithoutWS.Text = TotalCommuneWWS.ToString();

            //Uncovered
            this.P5sLbtnUncoveredWards.Text = TotalWardUnCover.ToString();
            this.P5sLbtnUncoveredTowns.Text = TotalTownUnCover.ToString();
            this.P5sLbtnUncoveredCommunes.Text = TotalCommuneUnCover.ToString();


            #endregion

            #region Distributor

            DataTable dtableDirect = this.P5sGetDistributorDirect(distributorCDs);
            DataTable dtableInDirect = this.P5sGetDistributorInDirect(distributorCDs);


            //Display InDirect NPP No Customer
            if (this.P5sDdlDistributorType.SelectedValue == "0" || this.P5sDdlDistributorType.SelectedValue == "2" || this.P5sDdlDistributorType.SelectedValue == "3")
            {
                DataTable dtableInDirectNoCustomer = this.P5sGetDistributorInDirectNoCustomer(distributorCDs);
                dtableInDirect.Merge(dtableInDirectNoCustomer);
            }


            //process user select khu vực uncovered
            if (dtableDirect.Rows.Count + dtableInDirect.Rows.Count <= 0)
            {
                String districts = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(dtableUnCover, "DISTRICT_CD");
                districts = districts == "-1" ? P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(this.P5sGetCommuneNotExsits(strCommunes), "DISTRICT_CD") : districts;


                dtableDirect = this.P5sGetDistributorDirectByDistrict(districts);
                dtableInDirect = this.P5sGetDistributorInDirectByDistrict(districts);

            }


            String objectDistributorDirect = this.P5sConvertDataTableDistributorJson(dtableDirect);
            String objectDistributorInDirect = this.P5sConvertDataTableDistributorJson(dtableInDirect);



            //Int64 TotalDistributor = dtableDirect.Rows.Count + dtableInDirect.Rows.Count;

            DataTable dtDistributorType = L5sSql.Query(@"SELECT DISTRIBUTOR_TYPE_CD,DISTRIBUTOR_TYPE_CODE,DISTRIBUTOR_TYPE_IMAGE
                                                          FROM M_DISTRIBUTOR_TYPE");
            this.P5sLblDistributorDirect.Text = String.Format("{1}  [{0}]", dtableDirect.Rows.Count, dtDistributorType.Rows[0]["DISTRIBUTOR_TYPE_CODE"].ToString());

            DataView dViewSDW = new DataView(dtableInDirect);
            dViewSDW.RowFilter = "DISTRIBUTOR_TYPE_CD = 2";


            DataView dViewSDN = new DataView(dtableInDirect);
            dViewSDN.RowFilter = "DISTRIBUTOR_TYPE_CD = 3";

            //this.P5sLblDistributorInDirectSDW.Text = String.Format("{1}  [{0}]", dViewSDW.ToTable().Rows.Count, dtDistributorType.Rows[1]["DISTRIBUTOR_TYPE_CODE"].ToString());
            this.P5sLblDistributorInDirectSDN.Text = String.Format("{1}  [{0}]", dViewSDN.ToTable().Rows.Count, dtDistributorType.Rows[2]["DISTRIBUTOR_TYPE_CODE"].ToString());

            //set image
            this.P5sImgDistributorDirect.ImageUrl = dtDistributorType.Rows[0]["DISTRIBUTOR_TYPE_IMAGE"].ToString();
            //this.P5sImgDistributorInDirectSDW.ImageUrl = dtDistributorType.Rows[1]["DISTRIBUTOR_TYPE_IMAGE"].ToString();
            this.P5sImgDistributorInDirectSDN.ImageUrl = dtDistributorType.Rows[2]["DISTRIBUTOR_TYPE_IMAGE"].ToString();


            #endregion


            //   Get commune không thỏa đk


            L5sJS.L5sRun("loadMap()");
            //Call Polyline
            //  this.P5sProcessPolyline(provinceCDByArea);
            //Call Polygon


            if (this.P5sDdlREGION_CD.SelectedIndex != 0)
            {
                String strCommunesNoSatisfy = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(this.P5sGetCommuneNotExsits(strCommunes), "COMMUNE_CD");
                this.P5sProcessPolygon(dtableWS, dtableWWS, dtableUnCover, strCommunesNoSatisfy);
                L5sJS.L5sRun("AddDistributorDirect('" + objectDistributorDirect + "')");
                L5sJS.L5sRun("AddDistributorInDirect('" + objectDistributorInDirect + "')");

                //  Call POIS                
                this.P5sPOIs(strCommunes);

            }





            L5sJS.L5sRun("AddUnCoverage('" + objectUnCover + "')");
            L5sJS.L5sRun("AddCoverageWithoutWS('" + objectWWS + "')");
            L5sJS.L5sRun("AddCoverageWS('" + objectWS + "')");
            L5sJS.L5sRun("FitOverlays()");
            this.P5sPnlSearchCoverage.Visible = true;


        }


        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {

            this.P5sActRegion.L5sSetDefaultValues(this.P5sTxtRegionCD.Text);
            this.P5sActArea.L5sSetDefaultValues(this.P5sTxtAreaCD.Text);
            this.P5sActDistributor.L5sSetDefaultValues(this.P5sTxtDistributor.Text);


            //  this.P5sPnlMain.Height = Unit.Pixel(800);
        }


        protected void P5sLbtnAccept_Click(object sender, EventArgs e)
        {
            this.P5sLbtnNetWorkDistributorSearch_Click(this.P5sLbtnNetWorkDistributorSearch, e);
            L5sMsg.Show("Chuyển vùng thành công !.");
        }

        protected void P5sRemapRouteList_Click(object sender, EventArgs e)
        {
            Response.Redirect("./MAPPING_HIERARCHY.aspx");
        }

        protected void P5sLbtnLoadDistributor_Click(object sender, EventArgs e)
        {
            L5sJS.L5sRun("loadMap()"); // load map first
            String txtDistributorCDs = this.P5sTxtDistributor.Text;
            if (txtDistributorCDs.Trim().Length <= 0)
            {
                L5sMsg.Show("Nhà phân phối chưa được chọn .");
                return;
            }

            List<String> latLngList = new List<string>();

            String[] cColors = new String[] { "#0000ee", "#458b00", "#cd8c95", "#008b8b", "#eead0e", "#006400", "#8b4500", "#00bfff", "#cd5555", "#7cfc00", "#eedc82", "#20b2aa", "#8470ff", "#ffc1c1", "#436eee", "#d02090", "#8b8b00", "#9acd32", "#87ceff", "#eed2ee", "#4a708b", "#8b0000", "#8b2252", "#8b4789", "#698b22", "#ffec8b", "#f08080", "#1e90ff", "#838b8b", "#adff2f", "#7fffd4", "#8b8378", "#eed5b7", "#ff7f24", "#ffff00", "#0000ee", "#458b00", "#cd8c95", "#008b8b", "#eead0e", "#006400", "#8b4500", "#00bfff", "#cd5555", "#7cfc00", "#eedc82", "#20b2aa", "#8470ff", "#ffc1c1", "#436eee", "#d02090", "#8b8b00", "#9acd32", "#87ceff", "#eed2ee", "#4a708b", "#8b0000", "#8b2252", "#8b4789", "#698b22", "#ffec8b", "#f08080", "#1e90ff", "#838b8b", "#adff2f", "#7fffd4", "#8b8378", "#eed5b7" };
            DataTable dtDistributor = new DataTable();
            Dictionary<String, String> dic;
            String distributorCDs = "";
            Int16 colorIndex = 0;


            String strDictrictCds = "";
            dtDistributor = this.P5sGetDistributorNetworkByDistributor(txtDistributorCDs);
            dic = new Dictionary<String, String>();

            for (int i = 0; i < dtDistributor.Rows.Count; i++)
            {
                String lnglatDis = dtDistributor.Rows[i]["LONGITUDE_LATITUDE"].ToString();

                latLngList.Add(lnglatDis);
                String distributorCD = dtDistributor.Rows[i]["DISTRIBUTOR_CD"].ToString();

                if (!dic.ContainsKey(distributorCD))
                {
                    distributorCDs += distributorCD + ",";


                    //Get Color to set for distributor
                    if (colorIndex < cColors.Length)
                        dic.Add(distributorCD, cColors[colorIndex++]);
                    else
                    {
                        String hexColour = this.P5sRandomColorHexa(new List<String>(dic.Values));
                        dic.Add(distributorCD, hexColour);
                    }

                }
                String polygon = dtDistributor.Rows[i]["POLYGON"].ToString();
                String districtCD = dtDistributor.Rows[i]["CD"].ToString();
                strDictrictCds = strDictrictCds == "" ? districtCD : strDictrictCds + "," + districtCD;
                String color = "";
                dic.TryGetValue(distributorCD, out color);

                //call function AddDistributorNetwork to paint polygon
                L5sJS.L5sRun("AddDistributorNetwork('" + polygon + "','District','" + dtDistributor.Rows[i]["DISTRIBUTOR_CD"].ToString() + "','" + districtCD + "','" + color + "')");
            }

            DataTable dtableDirect = this.P5sGetDistributorDirect(txtDistributorCDs);
            DataTable dtableInDirect = this.P5sGetDistributorInDirect(txtDistributorCDs);

            String objectDistributorDirect = this.P5sConvertDataTableDistributorJson(dtableDirect);
            String objectDistributorInDirect = this.P5sConvertDataTableDistributorJson(dtableInDirect);

            L5sJS.L5sRun("AddDistributorDirect('" + objectDistributorDirect + "')");
            L5sJS.L5sRun("AddDistributorInDirect('" + objectDistributorInDirect + "')");


            L5sJS.L5sRun("FitOverlays2('" + latLngList[0] + "')");

        }

        protected void P5sSettingAndReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Report/CustomerList.aspx");
        }

        protected void P5sCoverageBySales_Click(object sender, EventArgs e)
        {
            Response.Redirect("./COVERAGE_NEW_V2.aspx");
        }


        protected String P5sRandomColorHexa(List<String> v)
        {
            while (true)
            {
                Random random = new Random();
                int red = random.Next(0, 255);
                int green = random.Next(0, 255);
                int blue = random.Next(0, 255);
                String hexColour = String.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
                if (!v.Contains(hexColour))
                {
                    return hexColour;
                }
            }

            return "";
        }



        protected String P5sGetDistributorNetWork(String provinceCDByArea)
        {


            String provinceCDs = this.P5sDdlPROVINCE_CD.SelectedValue;

            String sql = String.Format(@"SELECT dis.DISTRIBUTOR_CD FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                            INNER JOIN M_DISTRICT district ON cmm.DISTRICT_CD = district.DISTRICT_CD
	                            INNER JOIN M_PROVINCE pro ON district.PROVINCE_CD = pro.PROVINCE_CD
                            WHERE dis.ACTIVE  = 1 AND (1=1) and pro.PROVINCE_CD IN ({0})", provinceCDByArea);


            if (provinceCDs != "")
                sql = sql.Replace("(1=1)", String.Format(" pro.PROVINCE_CD IN ({0}) ", provinceCDs));

            DataTable dt = L5sSql.Query(sql);

            if (dt == null && dt.Rows.Count <= 0)
                return "-1";

            return P5sCmmFns.P5sConvertDataTableToListStr(dt, "DISTRIBUTOR_CD");
        }

        protected void P5sLbtnNetWorkDistributorSearch_Click(object sender, EventArgs e)
        {
            String regionCDs = this.P5sDdlREGION_CD.SelectedValue;
            String areaCDs = this.P5sDdlAREA_CD.SelectedValue;
            String provinceCDByArea = this.P5sGetProvinceCdByArea(regionCDs, ref areaCDs);


            L5sJS.L5sRun("loadMap()"); // load map first
            String[] cColors = new String[] { "#0000ee", "#458b00", "#cd8c95", "#008b8b", "#eead0e", "#006400", "#8b4500", "#00bfff", "#cd5555", "#7cfc00", "#eedc82", "#20b2aa", "#8470ff", "#ffc1c1", "#436eee", "#d02090", "#8b8b00", "#9acd32", "#87ceff", "#eed2ee", "#4a708b", "#8b0000", "#8b2252", "#8b4789", "#698b22", "#ffec8b", "#f08080", "#1e90ff", "#838b8b", "#adff2f", "#7fffd4", "#8b8378", "#eed5b7", "#ff7f24", "#ffff00", "#0000ee", "#458b00", "#cd8c95", "#008b8b", "#eead0e", "#006400", "#8b4500", "#00bfff", "#cd5555", "#7cfc00", "#eedc82", "#20b2aa", "#8470ff", "#ffc1c1", "#436eee", "#d02090", "#8b8b00", "#9acd32", "#87ceff", "#eed2ee", "#4a708b", "#8b0000", "#8b2252", "#8b4789", "#698b22", "#ffec8b", "#f08080", "#1e90ff", "#838b8b", "#adff2f", "#7fffd4", "#8b8378", "#eed5b7" };

            DataTable dtDistributor = new DataTable();
            Dictionary<String, String> dic;
            String distributorCDs = "";
            Int16 colorIndex = 0;

            List<String> latLngList = new List<string>();
            switch (P5sRdBtnlist.SelectedValue)
            {
                case "Province":
                    dtDistributor = this.P5sGetDistributorNetworkByProvince(provinceCDByArea);
                    String[] provinceCDs = new String[dtDistributor.Rows.Count];
                    Int32 count = -1;
                    dic = new Dictionary<String, String>();
                    for (int i = 0; i < dtDistributor.Rows.Count; i++)
                    {
                        String latLng = dtDistributor.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                        latLngList.Add(latLng);
                        distributorCDs += dtDistributor.Rows[i]["DISTRIBUTOR_CD"].ToString() + ",";
                        String provinceCD = dtDistributor.Rows[i]["CD"].ToString();
                        String polygon = dtDistributor.Rows[i]["POLYGON"].ToString();
                        if (Array.IndexOf(provinceCDs, provinceCD) == -1)
                        {
                            count++;
                            provinceCDs[count] = provinceCD;

                            if (!dic.ContainsKey(provinceCD))
                            {
                                //Get Color to set for distributor
                                if (colorIndex < cColors.Length)
                                    dic.Add(provinceCD, cColors[colorIndex++]);
                                else
                                {
                                    String hexColour = this.P5sRandomColorHexa(new List<String>(dic.Values));
                                    dic.Add(provinceCD, hexColour);
                                }
                            }

                        }

                        String color = "";
                        dic.TryGetValue(provinceCD, out color);

                        //call function AddDistributorNetwork to paint polygon
                        L5sJS.L5sRun("AddDistributorNetwork('" + polygon + "','Province','" + dtDistributor.Rows[i]["DISTRIBUTOR_CD"].ToString() + "','" + provinceCD + "','" + color + "')");
                    }
                    break;
                case "District":
                    String strDictrictCds = "";
                    dtDistributor = this.P5sGetDistributorNetworkByDistrict(provinceCDByArea);
                    dic = new Dictionary<String, String>();

                    for (int i = 0; i < dtDistributor.Rows.Count; i++)
                    {
                        String latLng = dtDistributor.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                        latLngList.Add(latLng);
                        String distributorCD = dtDistributor.Rows[i]["DISTRIBUTOR_CD"].ToString();

                        if (!dic.ContainsKey(distributorCD))
                        {
                            distributorCDs += distributorCD + ",";


                            //Get Color to set for distributor
                            if (colorIndex < cColors.Length)
                                dic.Add(distributorCD, cColors[colorIndex++]);
                            else
                            {
                                String hexColour = this.P5sRandomColorHexa(new List<String>(dic.Values));
                                dic.Add(distributorCD, hexColour);
                            }

                        }
                        String polygon = dtDistributor.Rows[i]["POLYGON"].ToString();
                        String districtCD = dtDistributor.Rows[i]["CD"].ToString();
                        strDictrictCds = strDictrictCds == "" ? districtCD : strDictrictCds + "," + districtCD;
                        String color = "";
                        dic.TryGetValue(distributorCD, out color);

                        //call function AddDistributorNetwork to paint polygon
                        L5sJS.L5sRun("AddDistributorNetwork('" + polygon + "','District','" + dtDistributor.Rows[i]["DISTRIBUTOR_CD"].ToString() + "','" + districtCD + "','" + color + "')");

                    }

                    // District not uncover
                    strDictrictCds = strDictrictCds == "" ? "-1" : strDictrictCds;
                    String strDistrictCreatePolygon = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(this.P5sGetDistrictNotExsits(strDictrictCds), "DISTRICT_CD");
                    DataTable dtable = this.P5sGetPolygonDistrictByCD(strDistrictCreatePolygon);
                    String objecJson = this.P5sConvertDtableToJson(dtable);
                    L5sJS.L5sRun("AddPolygonDistributorNetworkUncover('" + objecJson + "')");

                    break;
                case "Commune":

                    String strCommuneCds = "";
                    dtDistributor = this.P5sGetDistributorNetworkByCommune(provinceCDByArea);
                    dic = new Dictionary<String, String>();
                    for (int i = 0; i < dtDistributor.Rows.Count; i++)
                    {
                        String latLng = dtDistributor.Rows[i]["LONGITUDE_LATITUDE"].ToString();
                        latLngList.Add(latLng);
                        String distributorCD = dtDistributor.Rows[i]["DISTRIBUTOR_CD"].ToString();

                        if (!dic.ContainsKey(distributorCD))
                        {
                            distributorCDs += distributorCD + ",";


                            //Get Color to set for distributor
                            if (colorIndex < cColors.Length)
                                dic.Add(distributorCD, cColors[colorIndex++]);
                            else
                            {
                                String hexColour = this.P5sRandomColorHexa(new List<String>(dic.Values));
                                dic.Add(distributorCD, hexColour);
                            }

                        }
                        String communeCD = dtDistributor.Rows[i]["CD"].ToString();
                        String polygon = dtDistributor.Rows[i]["POLYGON"].ToString();
                        String color = "";
                        dic.TryGetValue(distributorCD, out color);

                        strCommuneCds = strCommuneCds == "" ? communeCD : strCommuneCds + "," + communeCD;
                        //call function AddDistributorNetwork to paint polygon
                        L5sJS.L5sRun("AddDistributorNetwork('" + polygon + "','Commune','" + dtDistributor.Rows[i]["DISTRIBUTOR_CD"].ToString() + "','" + communeCD + "','" + color + "')");


                    }
                    //Commune not uncover
                    strCommuneCds = strCommuneCds == "" ? "-1" : strCommuneCds;
                    String strCommuneCreatePolygon = P5sCmm.P5sCmmFns.P5sConvertDataTableToListStr(this.P5sGetCommuneNotExsits(strCommuneCds), "COMMUNE_CD");
                    dtable = this.P5sGetPolygonCommuneByCD(strCommuneCreatePolygon);
                    objecJson = this.P5sConvertDtableToJson(dtable);
                    L5sJS.L5sRun("AddPolygonDistributorNetworkUncover('" + objecJson + "')");
                    break;
            }





            if (distributorCDs.Length > 0)
            {
                distributorCDs = distributorCDs.Remove(distributorCDs.Length - 1);
                distributorCDs = distributorCDs + "," + this.P5sGetDistributorNetWork(provinceCDByArea); //get thêm thông tin NPP dựa vào region

                DataTable dtableDirect = this.P5sGetDistributorDirect(distributorCDs);
                String objectDirect = this.P5sConvertDataTableDistributorJson(dtableDirect);

                DataTable dtableInDirect = this.P5sGetDistributorInDirect(distributorCDs);
                String objectInDirect = this.P5sConvertDataTableDistributorJson(dtableInDirect);

                String objectLongTude = this.P5sConvertDtableLongTudeToJson(dtDistributor);
                String centralPoint = this.P5sConvertDtableCentralPointToJson(dtDistributor);


                L5sJS.L5sRun("AddCentralPoint('" + centralPoint + "')");
                L5sJS.L5sRun("AddDistributorNetworkPoint('" + objectLongTude + "')");


                L5sJS.L5sRun("AddDistributorNetworkDirect('" + objectDirect + "')");
                L5sJS.L5sRun("AddDistributorNetworkInDirect('" + objectInDirect + "')");
                //  L5sJS.L5sRun("FitOverlays()
                // JavaScriptSerializer serializer = new JavaScriptSerializer();
                //L5sJS.L5sRun("FitOverlays2('" + serializer.ConvertToType<Object>(latLngList) + "')");   
                L5sJS.L5sRun("FitOverlays2('" + latLngList[0] + "')");
                this.P5sLbtnShowHideDistributorNetworkPoint.Enabled = true;
                this.P5sLbtnShowHideCentralPoint.Enabled = true;
            }

        }

        protected void P5sDdlPROVINCE_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.P5sInit();
            L5sJS.L5sRun("loadMap()");
            this.P5sPnlSearchCoverage.Visible = false;
        }

        protected void P5sRdBtnlist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}