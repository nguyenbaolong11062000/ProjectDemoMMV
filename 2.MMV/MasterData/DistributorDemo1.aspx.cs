using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using L5sDmComm;
using P5sCmm;

namespace MMV.MasterData
{
    public partial class DistributorDemo1 : System.Web.UI.Page
    {
        public L5sForm P5sMainFrm;
        public L5sPaging P5sMainPaging;
        public L5sTextboxFilter P5sTxtFtr;

        public L5sAutocomplete P5sActDISTRIBUTOR;
        public L5sAutocomplete P5sActPROVINCE;
        public L5sAutocomplete P5sActDISTRICT;
        public L5sAutocomplete P5sActCOMMUNE;

        public L5sAutocomplete P5sActRSM;
        public L5sAutocomplete P5sActASM;
        public L5sAutocomplete P5sActCDs;


        String P5sSqlSelect = @"SELECT dis.DISTRIBUTOR_CD
                                      ,dis.DISTRIBUTOR_CODE
                                      ,dis.DISTRIBUTOR_NAME
                                      ,dis.ACTIVE
                                      ,are.REGION_CD
                                      ,are.AREA_CD
                                      ,dis.LONGITUDE_LATITUDE
                                      ,dis.COMMUNE_CD
                                      ,dis.DISTRIBUTOR_ADDRESS,                           
	                                  dist.DISTRICT_CD,
	                                  dist.PROVINCE_CD, dis.DISTRIBUTOR_TYPE_CD, dis.DISTRIBUTOR_PARENT_CD,                            
                                      type.DISTRIBUTOR_TYPE_CODE
                        FROM 
	                        [M_DISTRIBUTOR.] dis 
	                        INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                        INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD  
                            INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                        INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1 
                            INNER JOIN M_AREA are ON arePro.AREA_CD = are.AREA_CD                     
                            INNER JOIN M_DISTRIBUTOR_TYPE type ON dis.DISTRIBUTOR_TYPE_CD = type.DISTRIBUTOR_TYPE_CD
                            ";
        String P5sSqlInsert = @"INSERT INTO [dbo].[M_DISTRIBUTOR.]
                               ([DISTRIBUTOR_CODE]
                               ,[DISTRIBUTOR_NAME]
                               ,[ACTIVE]                               
                               ,[LONGITUDE_LATITUDE]
                               ,[DISTRIBUTOR_TYPE]
                               ,[COMMUNE_CD]
                               ,[DISTRIBUTOR_ADDRESS] 
                               ,[SUPERVISOR_CD], DISTRIBUTOR_TYPE_CD , DISTRIBUTOR_PARENT_CD)
                                VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8,@9)
                                ";
        String P5sSqlUpdate = @"
                             UPDATE [dbo].[M_DISTRIBUTOR.]
                               SET [DISTRIBUTOR_CODE] = @1
                                  ,[DISTRIBUTOR_NAME] =  @2
                                  ,[ACTIVE] = @3
                                  ,[LONGITUDE_LATITUDE]  = @4
                                  ,[COMMUNE_CD] = @5
                                  ,[DISTRIBUTOR_ADDRESS] = @6
                                  ,[SUPERVISOR_CD] = @7
                                  ,DISTRIBUTOR_TYPE_CD = @8
                                  ,DISTRIBUTOR_PARENT_CD = @9
                             WHERE DISTRIBUTOR_CD = @0            
                              ";
        String P5sSqlDelete = @"
                             DELETE FROM [dbo].[M_DISTRIBUTOR.]
                             WHERE DISTRIBUTOR_CD = @0            
                              ";
        String P5sSqlOrderBy = " DISTRIBUTOR_CODE, DISTRIBUTOR_NAME ";

        String[] P5sControlNameInitAutoComplete = new String[] { "P5sConfirm", "P5sDdlDistributorType", "P5sEdit", "P5sLbtnNew", "P5sLbtnUpdate", "P5sLbtnInsert", "P5sDdlREGION_CD", "P5sDdlAREA_CD" };

        protected void Page_Load(object sender, EventArgs e)
        {
            //Phương thức load dữ liệu từ CSDL lên đổ vào GridView
            L5sInitial.Load(ViewState);

            if (!IsPostBack)
            {
                this.RequiredFieldValidator1.ErrorMessage = L5sMaster.L5sLangs["The Distributor Code field is required!"];
                this.RequiredFieldValidator2.ErrorMessage = L5sMaster.L5sLangs["The Distributor Name field is required!"];
                this.RequiredFieldValidator8.ErrorMessage = L5sMaster.L5sLangs["The Ward field is required!"];
                this.RequiredFieldValidator9.ErrorMessage = L5sMaster.L5sLangs["The Geocode field is required!"];
                this.RequiredFieldValidator10.ErrorMessage = L5sMaster.L5sLangs["The Address field is required!"];
                this.hiddenRequiredTH();
                this.P5sACTIVE.Text = L5sMaster.L5sLangs["Active"];
                this.P5sLoadDropDownListREGION();
                this.P5sLoadDropDownListAREA(this.P5sDdlREGION_CD.SelectedValue);
                this.P5sLoaDdlDistributorType();
            }
            if (P5sCmm.P5sCmmFns.P5sGetPostBackControlId(this) == "P5sConfirm")
            {
                this.P5sLbtnConfirm();
            }

            this.P5sInit();
        }
        protected void hiddenRequiredTH()
        {
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();
            if (nameCountry == "TH")
            {
                this.RequiredFieldValidator8.Enabled = false;
                this.lbWard.Visible = false;
            }


        }
        private void P5sAutoCompleteInit()
        {
            //hàm dùng để load dữ liệu lên ở các DropDownList
            if (P5sCmm.P5sCmmFns.P5sGetPostBackControlId(this) != "P5sEdit")
            {
                String areaCD = this.P5sDdlAREA_CD.SelectedValue;

                this.P5sActDISTRIBUTOR = this.P5sActDISTRIBUTOR == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistributorDirect(), this.P5sTxtDISTRIBUTOR_CD.ClientID, 1, true) : this.P5sActDISTRIBUTOR;


                //Load province 
                this.P5sActPROVINCE = this.P5sActPROVINCE == null ? new L5sAutocomplete(P5sCmmFns.P5sGetProvinceByArea(areaCD), this.P5sTxtPROVINCE_CD.ClientID, 1, true, this.P5sTxtDISTRICT_CD.ClientID) : this.P5sActPROVINCE;


                //Load district 
                String sqlProvince = String.Format(@"SELECT p.PROVINCE_CD 
                                                    FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD
                                                    WHERE a.AREA_CD IN ({0})", areaCD);

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
        private void P5sInit()
        {
            this.P5sMainGridViewInit();
            if (Array.IndexOf(P5sControlNameInitAutoComplete, P5sCmm.P5sCmmFns.P5sGetPostBackControlId(this)) != -1)
            {
                this.P5sAutoCompleteInit();
            }
        }
        public void P5sShowFormOnStatus(Int32 P5sFormStatus, Boolean P5sIsLoaded)
        {
            //PHương thức bắt buộc đối với tất cả các trang dùng để điều chỉnh các btn, ...

            //Khỏi tạo lại các giá trị mặt định ban đầu cho các control
            this.P5sLbtnInsert.Visible = this.P5sLbtnUpdate.Visible = this.P5sACTIVE.Checked = false;
            this.P5sTxtDISTRIBUTOR_CODE.Text = "";
            this.P5sTxtDISTRIBUTOR_NAME.Text = "";
            this.P5sDdlREGION_CD.SelectedValue = "-1";

            this.P5sDdlAREA_CD.SelectedValue = "-1";
            this.P5sTxtPROVINCE_CD.Text = "";
            this.P5sTxtDISTRICT_CD.Text = "";
            this.P5sTxtCOMMUNE_CD.Text = "";
            this.P5sTxtDISTRIBUTOR_ADDRESS.Text = "";
            this.P5sTxtLongTude.Text = "";
            this.P5sDdlAREA_CD.Enabled = true;
            this.P5sDdlREGION_CD.Enabled = true;
            this.P5sPnlUnderDistributor.Visible = false;//false



            switch (P5sFormStatus)
            {
                case 0://read only      
                    //Ẩn các button, disable các control không cho user chỉnh sửa 
                    this.P5sTxtDISTRIBUTOR_CODE.Enabled = false;
                    this.P5sTxtDISTRIBUTOR_NAME.Enabled = false;
                    this.P5sDdlREGION_CD.Enabled = false;
                    this.P5sDdlAREA_CD.Enabled = false;
                    this.P5sTxtPROVINCE_CD.Enabled = false;
                    this.P5sTxtDISTRICT_CD.Enabled = false;
                    this.P5sTxtCOMMUNE_CD.Enabled = false;
                    this.P5sTxtDISTRIBUTOR_ADDRESS.Enabled = false;
                    this.P5sTxtLongTude.Enabled = false;

                    if (this.P5sActPROVINCE != null)
                        this.P5sActPROVINCE.L5sDisable(true);

                    if (this.P5sActDISTRICT != null)
                        this.P5sActDISTRICT.L5sDisable(true);

                    if (this.P5sActCOMMUNE != null)
                        this.P5sActCOMMUNE.L5sDisable(true);
                    break;
                case 1: //new        
                    //Khi user thêm mới thì sẽ tiến hành ẩn button cập nhật                    
                    this.P5sLbtnInsert.Visible = true;
                    this.P5sLbtnDelete.Visible = false;
                    this.P5sACTIVE.Checked = true;
                    this.P5sDdlDistributorType.SelectedIndex = 0;
                    break;
                case 2: //Edit   
                    //Khi user cập  nhật thì sẽ tiến hành ẩn button thêm mới  
                    this.P5sLbtnUpdate.Visible = true;
                    break;
                default:
                    break;
            }
        }
        private void P5sMainGridViewInit()
        {
            //Code khai báo và khỏi tạo gridView         

            //Khai báo đối tương form
            this.P5sMainFrm = new L5sForm(this.P5sMainGridView, false, "DISTRIBUTOR_CD", this.P5sDetailPanel, "P5sShowFormOnStatus",
                                this.P5sSqlInsert,
                                this.P5sSqlUpdate,
                                this.P5sSqlDelete,
                                new String[] { "P5sTxtDISTRIBUTOR_CODE.Text",
                                                "P5sTxtDISTRIBUTOR_NAME.Text",
                                                "P5sDdlREGION_CD.SelectedValue",
                                                "P5sDdlREGION_CD.SelectedValue",
                                                "P5sHdfAREA_CD.Value",
                                                "P5sHdfAREA_CD.Value",
                                                "P5sTxtPROVINCE_CD.Text",
                                                "P5sTxtPROVINCE_CD.Text",
                                                "P5sTxtDISTRICT_CD.Text",
                                                "P5sTxtCOMMUNE_CD.Text",
                                                "P5sTxtDISTRIBUTOR_ADDRESS.Text",
                                                "P5sTxtLongTude.Text",
                                                "P5sACTIVE.Checked" ,"P5sDdlDistributorType.SelectedValue","P5sTxtDISTRIBUTOR_CD.Text" },
                                new Int32[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 });


            //Khai báo đối tương paging
            this.P5sMainPaging = new L5sPaging(this.P5sMainGridView, P5sCmm.P5sEnum.PagingSize, this.P5sDdlMainPaging);

            //KHai báo Control Filter, chỉ định các Column (code sql) sẽ được filter
            this.P5sTxtFtr = new L5sTextboxFilter(this.P5sFilterPanel, this.P5sMainGridView,
                                        new String[] { "DISTRIBUTOR_CODE", "DISTRIBUTOR_NAME", "DISTRIBUTOR_ADDRESS" }, this.P5sFilterTxt);


            if (this.P5sTxtFtr.L5sIsClicked() && this.P5sFilterTxt.Text != "")
                this.P5sTxtFtr.L5sLoadFilter(this.P5sMainPaging, this.P5sSqlSelect, this.P5sSqlOrderBy, "");
            else
            {
                if (!IsPostBack)
                    this.P5sTxtFtr.L5sFilterButton_Click(this.P5sMainPaging, this.P5sSqlSelect, this.P5sSqlOrderBy, "");
                this.P5sMainFrm.L5sLoadPage(this.P5sMainPaging.L5sParseSql(this.P5sSqlSelect, this.P5sSqlOrderBy));
            }

            this.P5sMainPaging.L5sLoadPaging();

            //Hide P5sUpanelMainGV if this empty
            this.P5sPanelMainGV.Visible = this.P5sMainGridView.Rows.Count > 0 ? true : false;


        }
        protected void P5sSearch_Click(object sender, EventArgs e)
        {
            this.P5sFilterTxt.Text = this.P5sFilterTxt.Text.Trim();
            this.P5sTxtFtr.L5sFilterButton_Click(this.P5sMainPaging, this.P5sSqlSelect, this.P5sSqlOrderBy, "");
            this.P5sMainPaging.L5sLoadPaging();
        }
        protected void P5sLbtnNew_Click(object sender, EventArgs e)
        {
            this.P5sMainFrm.L5sAddNewRow();
        }
        private void P5sLoaDdlDistributorType()
        {
            String sql = @"
                            SELECT DISTRIBUTOR_TYPE_CD,DISTRIBUTOR_TYPE_CODE
                            FROM M_DISTRIBUTOR_TYPE WHERE ACTIVE = 1";

            DataTable dt = L5sSql.Query(sql);
            this.P5sDdlDistributorType.DataSource = dt;
            this.P5sDdlDistributorType.DataValueField = "DISTRIBUTOR_TYPE_CD";
            this.P5sDdlDistributorType.DataTextField = "DISTRIBUTOR_TYPE_CODE";
            this.P5sDdlDistributorType.SelectedIndex = 0;
            this.P5sDdlDistributorType.DataBind();

        }
        private void P5sLoadDropDownListREGION()
        {
            //Cmt table M_DISTRIBUTOR phân quyền Distributor thì ko cần join Distributor.
            String sql = @"SELECT -1 AS REGION_CD ,'--Select Region--' AS REGION_CODE  
                                UNION ALL
                               SELECT DISTINCT reg.REGION_CD, reg.REGION_CODE 
                                FROM M_REGION reg INNER JOIN M_AREA are ON reg.REGION_CD  = are.REGION_CD AND are.ACTIVE = 1
	                                 INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                WHERE reg.ACTIVE = 1 AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										 -- INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD
								 )  ORDER BY REGION_CD ";
            DataTable dt = L5sSql.Query(sql);

            this.P5sDdlREGION_CD.DataSource = dt;
            this.P5sDdlREGION_CD.DataValueField = "REGION_CD";
            this.P5sDdlREGION_CD.DataTextField = "REGION_CODE";
            this.P5sDdlREGION_CD.DataBind();
            if (dt.Rows.Count == 2)
            {
                this.P5sDdlREGION_CD.SelectedIndex = 1;
            }
        }
        protected void P5sDdlAREA_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.P5sTxtPROVINCE_CD.Text = this.P5sTxtDISTRICT_CD.Text = this.P5sTxtCOMMUNE_CD.Text = "";
            this.P5sInit();
        }
        private void P5sLoadDropDownListAREA(String RegionCD)
        {
            try
            {
                RegionCD = RegionCD == "" ? "-1" : RegionCD;
                String sql = @"SELECT -1 AS AREA_CD ,'--Select Area--' AS AREA_CODE , 0 AS AREA_ORDER 
                          UNION ALL 
                        
                          SELECT DISTINCT are.AREA_CD, are.AREA_CODE   ,are.AREA_ORDER
                                FROM  M_AREA are  INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                WHERE are.ACTIVE = 1 AND are.REGION_CD = @0 AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										 -- INNER JOIN [M_DISTRIBUTOR.] dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD -- AND dis.ACTIVE = 1
								 )  
								 ORDER BY AREA_CD   ";
                DataTable dt = L5sSql.Query(sql, RegionCD);
                this.P5sDdlAREA_CD.DataSource = dt;
                this.P5sDdlAREA_CD.DataValueField = "AREA_CD";
                this.P5sDdlAREA_CD.DataTextField = "AREA_CODE";
                this.P5sDdlAREA_CD.DataBind();
                this.P5sDdlAREA_CD.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }
        }
        private static DataTable P5sGetProvinceByArea(String areaCDs)
        {

            String P5sSqlSelect = String.Format(@"SELECT DISTINCT p.PROVINCE_CD, p.PROVINCE_CODE + '-' + p.PROVINCE_NAME_EN ,'', p.ACTIVE 
                                                FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD
                                                WHERE a.AREA_CD IN ({0})  AND a.ACTIVE = 1
                                                  AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										                      INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										                      INNER JOIN [M_DISTRIBUTOR.] dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									                    WHERE pro.PROVINCE_CD = a.PROVINCE_CD --AND dis.ACTIVE = 1 
                                                                )", areaCDs);
            DataTable dtable = L5sSql.Query(P5sSqlSelect);
            return dtable;
        }
        protected void P5sMainGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = L5sMaster.L5sLangs["Distributor Code"];
                e.Row.Cells[1].Text = L5sMaster.L5sLangs["Distributor Name"];
                e.Row.Cells[2].Text = L5sMaster.L5sLangs["Type"];
                e.Row.Cells[3].Text = L5sMaster.L5sLangs["Address"];
                e.Row.Cells[4].Text = L5sMaster.L5sLangs["Active"];
            }
            if (e.Row.RowType == DataControlRowType.DataRow ||
            e.Row.RowType == DataControlRowType.Header ||
            e.Row.RowType == DataControlRowType.Footer)
            {

                //Canh vị trí
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                        e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 10px;");
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                        e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 10px;padding-left:10px");

                }

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;



                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (i != 1) // 1 là thứ tự column cần thả nổi (độ dài tối đa), Trong 1 GridView thì sẽ có 1 cột thả nổi, những cột còn lại sẽ có with 1%
                        e.Row.Cells[i].Width = Unit.Percentage(1);

                }

                //Code ẩn colum trong gridView
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (i >= 6)
                        e.Row.Cells[i].Visible = false;
                }
            }
        }
        private void P5sLbtnConfirm()
        {
            String distributorCD = this.P5sTxtDISTRIBUTOR_CD.Text;
            if (distributorCD.Length <= 0)
            {
                L5sMsg.Show(L5sMaster.L5sLangs["All fields marked with an asterisk (*) are required!"]);
                return;
            }

            String sql = @"SELECT dis.DISTRIBUTOR_CD
                                      ,dis.DISTRIBUTOR_CODE
                                      ,dis.DISTRIBUTOR_NAME
                                      ,dis.ACTIVE
                                      ,are.REGION_CD
                                      ,are.AREA_CD
                                      ,dis.LONGITUDE_LATITUDE
                                      ,dis.COMMUNE_CD
                                      ,dis.DISTRIBUTOR_ADDRESS,
	                                  dist.DISTRICT_CD,
	                                  dist.PROVINCE_CD, dis.DISTRIBUTOR_TYPE_CD, dis.DISTRIBUTOR_PARENT_CD
                        FROM 
	                        [M_DISTRIBUTOR.] dis
	                        INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                        INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD  
                            INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                        INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1 
                            INNER JOIN M_AREA are ON arePro.AREA_CD = are.AREA_CD
                         WHERE dis.DISTRIBUTOR_CD = @0
                            ";
            DataTable dt = L5sSql.Query(sql, distributorCD);
            this.P5sDdlREGION_CD.SelectedValue = dt.Rows[0]["REGION_CD"].ToString();
            this.P5sLoadDropDownListAREA(dt.Rows[0]["REGION_CD"].ToString());
            this.P5sDdlAREA_CD.SelectedValue = dt.Rows[0]["AREA_CD"].ToString();

            this.P5sTxtPROVINCE_CD.Text = dt.Rows[0]["PROVINCE_CD"].ToString();
            this.P5sDdlAREA_CD.Enabled = false;
            this.P5sDdlREGION_CD.Enabled = false;
            this.P5sAutoCompleteInit();

            if (this.P5sActRSM != null)
                this.P5sActRSM.L5sDisable(true);


            if (this.P5sActASM != null)
                this.P5sActASM.L5sDisable(true);


            if (this.P5sActCDs != null)
                this.P5sActCDs.L5sDisable(true);

        }
        protected void P5sDdlDistributorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.P5sDdlREGION_CD.SelectedValue = "-1";
            this.P5sDdlAREA_CD.SelectedValue = "-1";
            this.P5sTxtPROVINCE_CD.Text = "";
            this.P5sTxtDISTRICT_CD.Text = "";
            this.P5sTxtCOMMUNE_CD.Text = "";
            this.P5sDdlAREA_CD.Enabled = true;
            this.P5sDdlREGION_CD.Enabled = true;
            this.P5sTxtDISTRIBUTOR_CD.Text = "";


            switch (((DropDownList)sender).SelectedValue)
            {
                case "1":
                    this.P5sPnlUnderDistributor.Visible = false;//false
                    break;
                case "2":
                    this.P5sPnlUnderDistributor.Visible = true;
                    this.P5sDdlAREA_CD.Enabled = false;
                    this.P5sDdlREGION_CD.Enabled = false;
                    break;

                case "3":
                    this.P5sPnlUnderDistributor.Visible = true;
                    this.P5sDdlAREA_CD.Enabled = false;
                    this.P5sDdlREGION_CD.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        protected void P5sDdlREGION_CD_SelectedIndexChanged(object sender, EventArgs e)
        {
            String regionCD = this.P5sDdlREGION_CD.SelectedValue;
            this.P5sDdlAREA_CD.Items.Clear();
            this.P5sLoadDropDownListAREA(regionCD);
            this.P5sTxtPROVINCE_CD.Text = this.P5sTxtDISTRICT_CD.Text = this.P5sTxtCOMMUNE_CD.Text = "";
            this.P5sInit();
        }
        protected Boolean P5sIsVaild()
        {
            String distributorCode = this.P5sTxtDISTRIBUTOR_CODE.Text.Trim();
            String distributorName = this.P5sTxtDISTRIBUTOR_NAME.Text.Trim();
            String communeCD = this.P5sTxtCOMMUNE_CD.Text.Trim();
            String distributorAddress = this.P5sTxtDISTRIBUTOR_ADDRESS.Text.Trim();
            String distributorLongTude = this.P5sTxtLongTude.Text.Trim();

            if (distributorCode == "")
            {
                L5sMsg.Show(L5sMaster.L5sLangs["All fields marked with an asterisk (*) are required!"]);
                return false;
            }

            if (distributorName == "")
            {
                L5sMsg.Show(L5sMaster.L5sLangs["All fields marked with an asterisk (*) are required!"]);
                return false;
            }

            if (communeCD == "")
            {
                L5sMsg.Show(L5sMaster.L5sLangs["All fields marked with an asterisk (*) are required!"]);
                return false;
            }

            if (distributorAddress == "")
            {
                L5sMsg.Show(L5sMaster.L5sLangs["All fields marked with an asterisk (*) are required!"]);
                return false;
            }
            return true;
        }
        protected void P5sLbtnInsert_Click(object sender, EventArgs e)
        {
            if (!P5sIsVaild())
                return;

            String distributorCode = this.P5sTxtDISTRIBUTOR_CODE.Text.Trim();
            String distributorName = this.P5sTxtDISTRIBUTOR_NAME.Text.Trim();
            String communeCD = this.P5sTxtCOMMUNE_CD.Text.Trim();
            String distributorAddress = this.P5sTxtDISTRIBUTOR_ADDRESS.Text.Trim();
            String distributorLongTude = this.P5sTxtLongTude.Text.Trim();
            String distributorType = this.P5sDdlDistributorType.SelectedValue;
            String parent = this.P5sTxtDISTRIBUTOR_CD.Text;
            bool active = this.P5sACTIVE.Checked;

            String strSql = "SELECT * FROM [M_DISTRIBUTOR.] WHERE DISTRIBUTOR_CODE =@0";
            if (L5sSql.Query(strSql, distributorCode).Rows.Count != 0)
            {
                L5sMsg.Show(L5sMaster.L5sLangs["Distributor Code existed!"]);
                return;
            }

            //Gọi phương thức thêm mới dữ liệu của framework
            //Insert new row
            this.P5sMainFrm.L5sInsertNewRow(distributorCode, distributorName, active, distributorLongTude, "ND",
                                          communeCD, distributorAddress, -1, distributorType, parent);

            //DISTRIBUTOR TYPE = 1: HUD DIST
            //DISTRIBUTOR TYPE = 2: SDW
            //DISTRIBUTOR TYPE = 3: SDN

            //nếu loại NPP là 2 hoặc 3 thì sẽ tiến hành thêm tuyến và nvbh tự động dùng để xử dụng bên MMV
            if (distributorType == "2" || distributorType == "3")
            {
                String distributorCD = L5sSql.Query("SELECT DISTRIBUTOR_CD FROM [M_DISTRIBUTOR.] WHERE DISTRIBUTOR_CODE = @0", distributorCode).Rows[0]["DISTRIBUTOR_CD"].ToString();

                String sql = @"
                            DECLARE @ROUTE_CD AS BIGINT
                            DECLARE @SALES_CD AS BIGINT
                            SET @ROUTE_CD = 0
                            SET @SALES_CD = 0

                            IF NOT EXISTS ( SELECT * FROM M_ROUTE WHERE DISTRIBUTOR_CD = @0 )
                            BEGIN
                            
                                        INSERT INTO [dbo].[M_ROUTE]
                                               ([ROUTE_CODE]
                                               ,[ROUTE_NAME]
                                               ,[DISTRIBUTOR_CD]
                                               ,[ACTIVE])
                                       SELECT 'R' + CAST ( @0 AS NVARCHAR) AS ROUTE_CODE, 'AutoCreate',@0,1


                                       SET @ROUTE_CD  = ( SELECT SCOPE_IDENTITY() )
                            END            

                            IF NOT EXISTS ( SELECT * FROM M_SALES WHERE DISTRIBUTOR_CD = @0 )
                            BEGIN
                                INSERT INTO [dbo].[M_SALES]
                                           ([SALES_CODE]
                                           ,[SALES_NAME]
                                           ,[DISTRIBUTOR_CD],[ACTIVE])
                                     SELECT 'S' + CAST ( @0 AS NVARCHAR) AS SALES_CODE, 'AutoCreate',@0,1
                                    SET @SALES_CD  = ( SELECT SCOPE_IDENTITY() )
                            END
                            
                             IF @SALES_CD != 0 AND @ROUTE_CD != 0
                             BEGIN
                                    INSERT INTO [dbo].[O_SALES_ROUTE]
                                       ([SALES_CD]
                                       ,[ROUTE_CD]
                                       ,[ACTIVE] )
                                     VALUES  (@SALES_CD , @ROUTE_CD, 1)
                             END
              
                          ";

                L5sSql.Execute(sql, distributorCD);
            }

            //Load lại thông tin GridView khi thêm mới thành công
            this.P5sMainFrm.L5sLoadPage(this.P5sMainPaging.L5sParseSql(P5sSqlSelect, this.P5sSqlOrderBy));

            this.P5sDdlDistributorType.SelectedIndex = 0;
        }
        protected void P5sMainGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["DISTRIBUTOR_CD"] = Convert.ToInt32(((GridView)sender).DataKeys[e.NewEditIndex].Value);

            this.P5sLbtnUpdate.Visible = true;
            this.P5sLbtnDelete.Visible = true;
            this.P5sDetailPanel.Visible = true;
            this.P5sLbtnInsert.Visible = false;
            this.MainGridView(ViewState["DISTRIBUTOR_CD"].ToString());
            // this.P5sMainFrm.L5sEditRow(e.NewEditIndex);



            String regionCD = ViewState["REGION_CD"].ToString();
            this.P5sLoadDropDownListAREA(regionCD);


            this.P5sActDISTRIBUTOR = this.P5sActDISTRIBUTOR == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistributorDirect(), this.P5sTxtDISTRIBUTOR_CD.ClientID, 1, true) : this.P5sActDISTRIBUTOR;


            String areaCD = ViewState["Area_CD"].ToString();
            //Load province 
            this.P5sActPROVINCE = this.P5sActPROVINCE == null ? new L5sAutocomplete(P5sGetProvinceByArea(areaCD), this.P5sTxtPROVINCE_CD.ClientID, 1, true, this.P5sTxtDISTRICT_CD.ClientID) : this.P5sActPROVINCE;


            //Load district 
            String sqlProvince = String.Format(@"SELECT p.PROVINCE_CD 
                                                    FROM M_AREA_PROVINCE a INNER JOIN M_PROVINCE p ON a.PROVINCE_CD = p.PROVINCE_CD 
                                                    WHERE a.AREA_CD IN ({0})", areaCD);

            String provinceCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlProvince);
            this.P5sActDISTRICT = this.P5sActDISTRICT == null ? new L5sAutocomplete(P5sCmmFns.P5sGetDistrict("PROVINCE_CD", provinceCDs), this.P5sTxtDISTRICT_CD.ClientID, 1, true, this.P5sTxtCOMMUNE_CD.ClientID) : this.P5sActDISTRICT;
            this.P5sActDISTRICT.L5sChangeFilteringId(this.P5sTxtPROVINCE_CD.ClientID);


            //Load COMMUNE 
            String sqlDistrict = String.Format(@"SELECT DISTRICT_CD FROM  M_DISTRICT WHERE PROVINCE_CD IN ({0}) ", provinceCDs);
            String districtCDs = P5sCmmFns.P5sConvertDataTableToListStr(sqlDistrict);
            this.P5sActCOMMUNE = this.P5sActCOMMUNE == null ? new L5sAutocomplete(P5sCmmFns.P5sGetCommune("DISTRICT_CD", districtCDs), this.P5sTxtCOMMUNE_CD.ClientID, 1, true) : this.P5sActCOMMUNE;
            this.P5sActCOMMUNE.L5sChangeFilteringId(this.P5sTxtDISTRICT_CD.ClientID);

            if (this.P5sDdlDistributorType.SelectedValue == "2" || this.P5sDdlDistributorType.SelectedValue == "3")
            {
                this.P5sLbtnConfirm();
                this.P5sPnlUnderDistributor.Visible = true;
            }
            this.P5sDdlAREA_CD.Text = ViewState["Area_CD"].ToString();
        }
        protected void P5sLbtnUpdate_Click(object sender, EventArgs e)
        {
            if (!P5sIsVaild())
                return;
            if (ViewState["DISTRIBUTOR_CD"] == null)
                return;



            String distributorCode = this.P5sTxtDISTRIBUTOR_CODE.Text.Trim();
            String distributorName = this.P5sTxtDISTRIBUTOR_NAME.Text.Trim();
            String communeCD = this.P5sTxtCOMMUNE_CD.Text.Trim();
            String distributorAddress = this.P5sTxtDISTRIBUTOR_ADDRESS.Text.Trim();
            String distributorLongTude = this.P5sTxtLongTude.Text.Trim();
            bool active = this.P5sACTIVE.Checked;
            String distributorType = this.P5sDdlDistributorType.SelectedValue;
            String distributorCD1 = ViewState["DISTRIBUTOR_CD"].ToString();
            String parent = P5sTxtDISTRIBUTOR_CD.Text.Trim();

            String strSql = "SELECT * FROM [M_DISTRIBUTOR.] WHERE DISTRIBUTOR_CODE =@0 AND DISTRIBUTOR_CD != @1";
            if (L5sSql.Query(strSql, distributorCode, ViewState["DISTRIBUTOR_CD"]).Rows.Count != 0)
            {
                L5sMsg.Show(L5sMaster.L5sLangs["Distributor Code existed!"]);
                return;
            }
            //Gọi phương thức cập nhật dữ liệu của framework
            long a = L5sSql.Execute(this.P5sSqlUpdate, distributorCD1, distributorCode, distributorName, active,
                                        distributorLongTude, communeCD,
                                        distributorAddress, -1, distributorType, parent);
            if (a > 0)
            {
                L5sMsg.Show("From the form: Data updated successfully!");
                this.P5sDetailPanel.Visible = false;
            }
            else
            {
                L5sMsg.Show("From the form: Data updated fail!");
            }
            //Load lại thông tin GridView khi cập nhật thành công
            this.P5sMainFrm.L5sLoadPage(this.P5sMainPaging.L5sParseSql(this.P5sSqlSelect, this.P5sSqlOrderBy));




            if (distributorType == "2" || distributorType == "3")
            {
                String distributorCD = L5sSql.Query("SELECT DISTRIBUTOR_CD FROM [M_DISTRIBUTOR.] WHERE DISTRIBUTOR_CODE = @0", distributorCode).Rows[0]["DISTRIBUTOR_CD"].ToString();

                String sql = @"
                            DECLARE @ROUTE_CD AS BIGINT
                            DECLARE @SALES_CD AS BIGINT
                            SET @ROUTE_CD = 0
                            SET @SALES_CD = 0

                            IF NOT EXISTS ( SELECT * FROM M_ROUTE WHERE DISTRIBUTOR_CD = @0 )
                            BEGIN
                            
                                        INSERT INTO [dbo].[M_ROUTE]
                                               ([ROUTE_CODE]
                                               ,[ROUTE_NAME]
                                               ,[DISTRIBUTOR_CD]
                                               ,[ACTIVE],[ROUTE_FREQ])
                                       SELECT 'R' + CAST ( @0 AS NVARCHAR) AS ROUTE_CODE, 'AutoCreate',@0,1,'F4'


                                       SET @ROUTE_CD  = ( SELECT SCOPE_IDENTITY() )
                            END            

                            IF NOT EXISTS ( SELECT * FROM M_SALES WHERE DISTRIBUTOR_CD = @0 )
                            BEGIN
                                INSERT INTO [dbo].[M_SALES]
                                           ([SALES_CODE]
                                           ,[SALES_NAME]
                                           ,[DISTRIBUTOR_CD],[ACTIVE])
                                     SELECT 'S' + CAST ( @0 AS NVARCHAR) AS SALES_CODE, 'AutoCreate',@0,1
                                    SET @SALES_CD  = ( SELECT SCOPE_IDENTITY() )
                            END
                            
                             IF @SALES_CD != 0 AND @ROUTE_CD != 0
                             BEGIN
                                    INSERT INTO [dbo].[O_SALES_ROUTE]
                                       ([SALES_CD]
                                       ,[ROUTE_CD]
                                       ,[ACTIVE] )
                                     VALUES  (@SALES_CD , @ROUTE_CD, 1)
                             END
              
                          ";

                L5sSql.Execute(sql, distributorCD);
            }
        }
        protected void P5sLbtnDelete_Click(object sender, EventArgs e)
        {
            if (!P5sIsVaild())
                return;
            if (ViewState["DISTRIBUTOR_CD"] == null)
                return;

            String distributorCD1 = ViewState["DISTRIBUTOR_CD"].ToString();
            //Gọi phương thức cập nhật dữ liệu của framework
            long a = L5sSql.Execute(this.P5sSqlDelete, distributorCD1);
            if (a > 0)
            {
                L5sMsg.Show("From the form: Data deleted successfully!");
                this.P5sDetailPanel.Visible = false;
            }
            else
            {
                L5sMsg.Show("From the form: Data deleted fail!");
            }
            //Load lại thông tin GridView khi xóa thành công
            this.P5sMainFrm.L5sLoadPage(this.P5sMainPaging.L5sParseSql(this.P5sSqlSelect, this.P5sSqlOrderBy));
        }
        protected void P5sLbtnCancel_Click(object sender, EventArgs e)
        {
            this.P5sMainFrm.L5sCloseForm();
        }
        protected void MainGridView(string viewStatee)
        {



            String sql = @"SELECT dis.DISTRIBUTOR_CD
                                      ,dis.DISTRIBUTOR_CODE
                                      ,dis.DISTRIBUTOR_NAME
                                      ,dis.ACTIVE
                                      ,are.REGION_CD
                                      ,are.AREA_CD
                                      ,dis.LONGITUDE_LATITUDE
                                      ,dis.COMMUNE_CD
                                      ,dis.DISTRIBUTOR_ADDRESS,                           
	                                  dist.DISTRICT_CD,
	                                  dist.PROVINCE_CD, dis.DISTRIBUTOR_TYPE_CD, dis.DISTRIBUTOR_PARENT_CD,                            
                                      type.DISTRIBUTOR_TYPE_CODE
                        FROM 
	                        [M_DISTRIBUTOR.] dis 
	                        INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
	                        INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD  
                            INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
	                        INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1 
                            INNER JOIN M_AREA are ON arePro.AREA_CD = are.AREA_CD                     
                            INNER JOIN M_DISTRIBUTOR_TYPE type ON dis.DISTRIBUTOR_TYPE_CD = type.DISTRIBUTOR_TYPE_CD
                            where dis.DISTRIBUTOR_CD = @0
                            ";
            //this.P5sTxtDISTRIBUTOR_CODE.Text = "";
            //this.P5sTxtDISTRIBUTOR_NAME.Text = "";
            //this.P5sDdlREGION_CD.Text = "-1";

            //this.P5sDdlAREA_CD.Text = "-1";
            //this.P5sTxtPROVINCE_CD.Text = "";
            //this.P5sTxtDISTRICT_CD.Text = "";
            //this.P5sTxtCOMMUNE_CD.Text = "";
            //this.P5sTxtDISTRIBUTOR_ADDRESS.Text = "";
            //this.P5sTxtLongTude.Text = "";

            DataTable dt = L5sSql.Query(sql, viewStatee);

            this.P5sTxtDISTRIBUTOR_CODE.Text = dt.Rows[0]["DISTRIBUTOR_CODE"].ToString();
            this.P5sTxtDISTRIBUTOR_NAME.Text = dt.Rows[0]["DISTRIBUTOR_NAME"].ToString();
            this.P5sDdlREGION_CD.Text = dt.Rows[0]["REGION_CD"].ToString();
            ViewState["REGION_CD"] = dt.Rows[0]["REGION_CD"].ToString();
            // this.P5sDdlAREA_CD.Text = dt.Rows[0]["AREA_CD"].ToString();
            ViewState["Area_CD"] = dt.Rows[0]["AREA_CD"].ToString();
            this.P5sTxtPROVINCE_CD.Text = dt.Rows[0]["PROVINCE_CD"].ToString();
            this.P5sTxtDISTRICT_CD.Text = dt.Rows[0]["DISTRICT_CD"].ToString();
            this.P5sTxtCOMMUNE_CD.Text = dt.Rows[0]["COMMUNE_CD"].ToString();
            this.P5sTxtDISTRIBUTOR_ADDRESS.Text = dt.Rows[0]["DISTRIBUTOR_ADDRESS"].ToString();
            this.P5sTxtLongTude.Text = dt.Rows[0]["LONGITUDE_LATITUDE"].ToString();
            this.P5sACTIVE.Checked = dt.Rows[0]["ACTIVE"].ToString() == "True" ? true : false;
        }
    }
}