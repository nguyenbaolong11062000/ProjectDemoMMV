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
using P5sCmm;
using P5sDmComm;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using System.Web.Script.Services;

namespace MMV.fsmdls.mmcv4
{
    public partial class TradeProgramMarkDSR : System.Web.UI.Page
    {
        public L5sForm P5sMainFrm;
        public L5sPaging P5sMainPaging;
        public L5sTextboxFilter P5sTxtFtr;
        public L5sAutocomplete P5sActType;


        public L5sAutocomplete P5sActDistributor, P5sActSales;
        public L5sAutocomplete P5sActRegion, P5sActArea, P5sActTradeProgram;


        String P5sSqlSelect = @"SELECT * FROM (SELECT tradeProgram.TRADE_PROGRAM_CODE,tradeProgram.TRADE_PROGRAM_NAME,
	                                           tradeProgram.TRADE_PROGRAM_CODE  + '-' + tradeProgram.TRADE_PROGRAM_NAME AS TRADE_PROGRAM_DESC,
	                                           cust.CUSTOMER_CD, cust.CUSTOMER_CODE, cust.CUSTOMER_NAME,cust.CUSTOMER_CHAIN_CODE,   
	                                           cust.CUSTOMER_ADDRESS,
                                               distInfo.AREA_CODE ,
											   distInfo.AREA_ORDER ,
											   distInfo.DISTRIBUTOR_CODE ,
											   distInfo.DISTRIBUTOR_NAME ,
											   distInfo.REGION_CODE ,
											   distInfo.REGION_ORDER ,
	                                           distInfo.DISTRIBUTOR_CODE + '-' + distInfo.DISTRIBUTOR_NAME AS DISTRIBUTOR_DESC,
	                                           sls.SALES_CODE  +'-' + sls.SALES_NAME AS SALES_DESC,
	                                           cust.CUSTOMER_CODE +'-' + cust.CUSTOMER_NAME AS CUSTOMER_DESC,
	                                           LEVEL_1_REVIEW_DESC = ISNULL( (SELECT TOP 1 LEVEL_APPROVE_NAME FROM O_LEVEL_APPROVE OLA WITH(NOLOCK, READUNCOMMITTED)
                                                                    WHERE TRADE_PROGRAM_CD=tradeProgram.TRADE_PROGRAM_CD AND EXISTS (SELECT  OCV.COMBOBOX_VALUE_CD FROM O_CONDITION_VALUE OCV  WITH(NOLOCK, READUNCOMMITTED)
                                                                    INNER JOIN O_CONDITION OC WITH(NOLOCK, READUNCOMMITTED) ON OCV.CONDITION_CD=OC.CONDITION_CD
                                                                    WHERE OC.LEVEL_APPROVE_CD=OLA.LEVEL_APPROVE_CD AND OCV.TRADE_PROGRAM_CUSTOMER_CD=tradeProgramCustomer.TRADE_PROGRAM_CUSTOMER_CD)
                                                                    ORDER BY ORDERBY DESC,LEVEL_APPROVE_CD DESC),''),
	                                           tradeProgramCustomer.NUMBER_PHOTO,
                                               tradeProgramCustomer.TRADE_PROGRAM_CUSTOMER_CD,
                                               tradeProgramCustomer.LEVEL_1_REVIEW,
                                               tradeProgramCustomer.LEVEL_2_REVIEW,
                                               tradeProgram.START_DATE
 , ROUND((ISNULL(PRE1.SALES_AMOUNT,0)+ISNULL(PRE2.SALES_AMOUNT,0)+ISNULL(PRE3.SALES_AMOUNT,0))/3,0) AMS
                                        	    
                                        FROM O_TRADE_PROGRAM tradeProgram WITH(NOLOCK, READUNCOMMITTED)
INNER JOIN O_TRADE_PROGRAM_CUSTOMER tradeProgramCustomer WITH(NOLOCK, READUNCOMMITTED) ON 
	                                         tradeProgram.TRADE_PROGRAM_CD = tradeProgramCustomer.TRADE_PROGRAM_CD
	                                         INNER JOIN M_CUSTOMER cust WITH(NOLOCK, READUNCOMMITTED) ON tradeProgramCustomer.CUSTOMER_CD = cust.CUSTOMER_CD
	                                         INNER JOIN dbo.[ufnGetCustomerOfSales]() AS  T ON cust.CUSTOMER_CD = T.CUSTOMER_CD
                                             INNER JOIN M_SALES sls WITH(NOLOCK, READUNCOMMITTED) ON T.SALES_CD = sls.SALES_CD
                                             INNER JOIN M_DISTRIBUTOR dist WITH(NOLOCK, READUNCOMMITTED) ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD 
	                                         INNER JOIN dbo.ufnGetDistributorInformation() distInfo ON dist.DISTRIBUTOR_CD = distInfo.DISTRIBUTOR_CD
                                             LEFT JOIN ME_SALES_AMOUNT PRE1  WITH(NOLOCK, READUNCOMMITTED) ON CUST.CUSTOMER_CD=PRE1.CUSTOMER_CD AND PRE1.YRMTH= CONVERT(nvarchar(6), DATEADD(MONTH,-1, tradeProgram.START_DATE),112) 
											 LEFT JOIN ME_SALES_AMOUNT PRE2 WITH(NOLOCK, READUNCOMMITTED) ON CUST.CUSTOMER_CD=PRE2.CUSTOMER_CD AND PRE2.YRMTH=CONVERT(nvarchar(6), DATEADD(MONTH,-2, tradeProgram.START_DATE), 112) 
											 LEFT JOIN ME_SALES_AMOUNT PRE3 WITH(NOLOCK, READUNCOMMITTED) ON CUST.CUSTOMER_CD=PRE3.CUSTOMER_CD AND PRE3.YRMTH=CONVERT(nvarchar(6), DATEADD(MONTH,-3, tradeProgram.START_DATE), 112)
                                    WHERE tradeProgram.TYPE = 1 AND (1=1) AND (2=2)) T WHERE (3=3)

                               ";

        String P5sSqlSelectBase = @"SELECT * FROM (SELECT tradeProgram.TRADE_PROGRAM_CODE,tradeProgram.TRADE_PROGRAM_NAME,
	                                           tradeProgram.TRADE_PROGRAM_CODE  + '-' + tradeProgram.TRADE_PROGRAM_NAME AS TRADE_PROGRAM_DESC,
	                                           cust.CUSTOMER_CD, cust.CUSTOMER_CODE, cust.CUSTOMER_NAME,cust.CUSTOMER_CHAIN_CODE,   
	                                           cust.CUSTOMER_ADDRESS,
											   distInfo.AREA_CODE ,
											   distInfo.AREA_ORDER ,
											   distInfo.DISTRIBUTOR_CODE ,
											   distInfo.DISTRIBUTOR_NAME ,
											   distInfo.REGION_CODE ,
											   distInfo.REGION_ORDER ,
	                                           distInfo.DISTRIBUTOR_CODE + '-' + distInfo.DISTRIBUTOR_NAME AS DISTRIBUTOR_DESC,
	                                           sls.SALES_CODE  +'-' + sls.SALES_NAME AS SALES_DESC,
	                                           cust.CUSTOMER_CODE +'-' + cust.CUSTOMER_NAME AS CUSTOMER_DESC,	
                                                                               
	                                          LEVEL_1_REVIEW_DESC = ISNULL((select TOP 1 LEVEL_APPROVE_NAME FROM O_LEVEL_APPROVE OLA WITH(NOLOCK, READUNCOMMITTED)
                                                                    WHERE TRADE_PROGRAM_CD=tradeProgram.TRADE_PROGRAM_CD AND exists (select  OCV.COMBOBOX_VALUE_CD FROM O_CONDITION_VALUE OCV WITH(NOLOCK, READUNCOMMITTED) 
                                                                    INNER JOIN O_CONDITION OC WITH(NOLOCK, READUNCOMMITTED) ON OCV.CONDITION_CD=OC.CONDITION_CD
                                                                    WHERE OC.LEVEL_APPROVE_CD=OLA.LEVEL_APPROVE_CD AND OCV.TRADE_PROGRAM_CUSTOMER_CD=tradeProgramCustomer.TRADE_PROGRAM_CUSTOMER_CD)
                                                                    ORDER BY ORDERBY DESC,LEVEL_APPROVE_CD DESC),''),

	                                           tradeProgramCustomer.NUMBER_PHOTO,
                                               tradeProgramCustomer.TRADE_PROGRAM_CUSTOMER_CD,
                                               tradeProgramCustomer.LEVEL_1_REVIEW,
                                               tradeProgramCustomer.LEVEL_2_REVIEW,
                                               tradeProgram.START_DATE
 , ROUND((ISNULL(PRE1.SALES_AMOUNT,0)+ISNULL(PRE2.SALES_AMOUNT,0)+ISNULL(PRE3.SALES_AMOUNT,0))/3,0) AMS
                                        	    
                                        FROM O_TRADE_PROGRAM tradeProgram WITH(NOLOCK, READUNCOMMITTED) INNER JOIN O_TRADE_PROGRAM_CUSTOMER tradeProgramCustomer WITH(NOLOCK, READUNCOMMITTED) ON 
	                                         tradeProgram.TRADE_PROGRAM_CD = tradeProgramCustomer.TRADE_PROGRAM_CD
	                                         INNER JOIN M_CUSTOMER cust WITH(NOLOCK, READUNCOMMITTED) ON tradeProgramCustomer.CUSTOMER_CD = cust.CUSTOMER_CD
	                                         INNER JOIN dbo.[ufnGetCustomerOfSales]() AS  T ON cust.CUSTOMER_CD = T.CUSTOMER_CD
                                             INNER JOIN M_SALES sls WITH(NOLOCK, READUNCOMMITTED) ON T.SALES_CD = sls.SALES_CD
                                             INNER JOIN M_DISTRIBUTOR dist WITH(NOLOCK, READUNCOMMITTED) ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD 
	                                         INNER JOIN dbo.ufnGetDistributorInformation() distInfo ON dist.DISTRIBUTOR_CD = distInfo.DISTRIBUTOR_CD
                                             
                                             LEFT JOIN ME_SALES_AMOUNT PRE1  WITH(NOLOCK, READUNCOMMITTED) ON CUST.CUSTOMER_CD=PRE1.CUSTOMER_CD AND PRE1.YRMTH= CONVERT(nvarchar(6), DATEADD(MONTH,-1, tradeProgram.START_DATE),112) 
											 LEFT JOIN ME_SALES_AMOUNT PRE2 WITH(NOLOCK, READUNCOMMITTED) ON CUST.CUSTOMER_CD=PRE2.CUSTOMER_CD AND PRE2.YRMTH=CONVERT(nvarchar(6), DATEADD(MONTH,-2, tradeProgram.START_DATE), 112) 
											 LEFT JOIN ME_SALES_AMOUNT PRE3 WITH(NOLOCK, READUNCOMMITTED) ON CUST.CUSTOMER_CD=PRE3.CUSTOMER_CD AND PRE3.YRMTH=CONVERT(nvarchar(6), DATEADD(MONTH,-3, tradeProgram.START_DATE), 112)
                                    WHERE tradeProgram.TYPE = 1 AND (1=1) AND (2=2)   )T WHERE (3=3)";

        String P5sSqlInsert = @"";
        String P5sSqlUpdate = @"";
        String P5sSqlDelete = "";
        String P5sSqlOrderBy = " T.START_DATE DESC, T.TRADE_PROGRAM_CODE ,T.REGION_ORDER, T.AREA_ORDER, T.DISTRIBUTOR_CODE, T.CUSTOMER_CODE  ";



        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["F5sUsrCD"] = 100;
            L5sInitial.Load(ViewState);
            String controlPostback = P5sCmm.P5sCmmFns.P5sGetPostBackControlId(this);
            if (!IsPostBack)
            {
                this.P5sTxtFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MM-yyyy");
                this.P5sTxtTo.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("dd-MM-yyyy");
                DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                P5sGetDataRegion();
                P5sGetDataArea(this.P5sDDLRegion.SelectedValue);
                P5sGetDataDistributor(this.P5sDDLArea.SelectedValue);
                P5sGetDataSales(this.P5sDDLDistributor.SelectedValue);
                P5sGetTradeProgram(from, to, 1);
            }
            else
            {
                //nếu gridview có dữ liệu thì khi use nhấn vào nút ẩn hiển filter sẽ phải khởi tạo lại gridview 
                //nếu không khỏi tạo thì khi nhấn vào xem chi tiết khách hàng sẽ không hiển thị gallery hình ảnh
                if (this.P5sMainGridView.Rows.Count > 0 && "P5sLtbnHideShowFilter" == controlPostback)
                {
                    this.P5sMainGridViewInit();
                }
                else //nếu control post back không nằm trong mảng thì mới khỏi tạo lại gridview
                    if (Array.IndexOf(new String[] { "P5sLbtnSearch", "P5sTxtFrom", "P5sTxtTo", "P5sDDLRegion", "P5sDDLArea", "P5sDDLDistributor" }, controlPostback) == -1)
                {
                    this.P5sMainGridViewInit();
                }
            }

            this.P5sTxtAMSFrom.Attributes.Add("onkeypress", "return isNumberKey(this);");
            this.P5sTxtAMSFrom.Attributes.Add("onkeyup", "javascript:this.value=Comma(this.value);");

            this.P5sTxtAMSTo.Attributes.Add("onkeypress", "return isNumberKey(this);");
            this.P5sTxtAMSTo.Attributes.Add("onkeyup", "javascript:this.value=Comma(this.value);");


            if (IsPostBack && Array.IndexOf(new String[] { "P5sLtbnHideShowFilter" }, controlPostback) > -1)
            {
                if (ViewState["HideShowFilter"] != null && ViewState["HideShowFilter"].ToString().Equals("1"))
                {
                    DataTable dtTemp = new DataTable();
                    dtTemp.Columns.Add("CD");
                    dtTemp.Columns.Add("Name");
                    dtTemp.Columns.Add("Filter");
                    dtTemp.Columns.Add("Active");

                    DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                    P5sGetDataRegion();
                    P5sGetDataArea(this.P5sDDLRegion.SelectedValue);
                    P5sGetDataDistributor(this.P5sDDLArea.SelectedValue);
                    P5sGetTradeProgram(from, to, 1);

                    //this.P5sActRegion = this.P5sActRegion == null ? new L5sAutocomplete(dtTemp, this.P5sTxtRegionCD.ClientID, 1, true, this.P5sTxtAreaCD.ClientID) : this.P5sActRegion;
                    //this.P5sActArea = this.P5sActArea == null ? new L5sAutocomplete(dtTemp, this.P5sTxtAreaCD.ClientID, 1, true, this.P5sTxtDistributorCD.ClientID) : this.P5sActArea;
                    //this.P5sActArea.L5sChangeFilteringId(this.P5sTxtRegionCD.ClientID);
                    //this.P5sActDistributor = this.P5sActDistributor == null ? new L5sAutocomplete(dtTemp, this.P5sTxtDistributorCD.ClientID, 0, true, this.P5sTxtSalesCD.ClientID) : this.P5sActDistributor;
                    //this.P5sActDistributor.L5sChangeFilteringId(this.P5sTxtAreaCD.ClientID);

                    //this.P5sActSales = this.P5sActSales == null ? new L5sAutocomplete(dtTemp, this.P5sTxtSalesCD.ClientID, 0, true) : this.P5sActSales;
                    //this.P5sActSales.L5sChangeFilteringId(this.P5sTxtDistributorCD.ClientID);
                    //this.P5sActTradeProgram = this.P5sActTradeProgram == null ? new L5sAutocomplete(dtTemp, this.P5sTxtTradeProgramCD.ClientID, 1, true) : this.P5sActTradeProgram;
                }
                else
                    this.P5sInitAct();
            }
            else
                this.P5sInitAct();
        }

        private void P5sInitAct()
        {
            

            if (!IsPostBack)
            {
                P5sGetDataRegion();
                P5sGetDataArea(this.P5sDDLRegion.SelectedValue);
                P5sGetDataDistributor(this.P5sDDLArea.SelectedValue);
                //this.P5sActTradeProgram = this.P5sActTradeProgram == null ? new L5sAutocomplete(
                //            P5sCmmFns.P5sGetTradeProgram(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                //                                         new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), 1), this.P5sTxtTradeProgramCD.ClientID, 1, true) : this.P5sActTradeProgram;
                if (string.IsNullOrEmpty(this.P5sTxtTradeProgram_CD.SelectedValue))
                {
                    P5sGetTradeProgram(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), 1);
                }
            }
        }



        private void P5sMainGridViewInit()
        {
            if (ViewState["SqlSearch"] != null)
                this.P5sSqlSelect = ViewState["SqlSearch"].ToString();


            this.P5sMainFrm = new L5sForm(this.P5sMainGridView, false, "TRADE_PROGRAM_CUSTOMER_CD", this.P5sPnlMainDetail, "P5sShowFormOnStatus",
                                          this.P5sSqlInsert,
                                          this.P5sSqlUpdate,
                                          this.P5sSqlDelete,
                                         new String[] {
                                                         "P5sLblRegionCode.Text",
                                                         "P5sLblAreaCode.Text",
                                                         "P5sLblDistributorDesc.Text",
                                                         "P5sLblSalesDesc.Text",
                                                         "P5sLblCustomerDesc.Text",
                                                         "P5sLblCustomerAddress.Text",
                                                         "P5sLblTradeProgramDesc.Text"},
                                          new Int32[] { 12, 13, 14, 15, 16, 17, 18 });

            this.P5sMainPaging = new L5sPaging(this.P5sMainGridView, P5sCmm.P5sEnum.PagingSize, this.P5sDdlMainPaging);
            this.P5sMainFrm.L5sLoadPage(this.P5sMainPaging.L5sParseSql(this.P5sSqlSelect, this.P5sSqlOrderBy));
            this.P5sMainPaging.L5sLoadPaging();


        }

        protected void P5sMainGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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

                //hàm đăng ký Postback trigger cho gridview (important for load Image)
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton edit = e.Row.FindControl("P5sEdit") as LinkButton;
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(edit);
                }


                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (i != 4) // 1 là thứ tự column cần thả nổi (độ dài tối đa), Trong 1 GridView thì sẽ có 1 cột thả nổi, những cột còn lại sẽ có with 1%
                        e.Row.Cells[i].Width = Unit.Percentage(1);

                }

                //Code ẩn colum trong gridView
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (i >= 12)
                        e.Row.Cells[i].Visible = false;

                }
            }
        }


        protected void P5sMainGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grdView = (GridView)sender;
            ViewState["TRADE_PROGRAM_CUSTOMER_CD"] = Convert.ToInt32(((GridView)sender).DataKeys[e.NewEditIndex].Value);
            //Session["TRADE_PROGRAM_CUSTOMER_CD"] = ViewState["TRADE_PROGRAM_CUSTOMER_CD"].ToString();
            //string a = Convert.ToInt32(((GridView)sender).DataKeys[e.NewEditIndex].Value);
            this.P5sMainFrm.L5sEditRow(e.NewEditIndex);
            this.P5sLoadImage();
            //            String strGetLevelApprove = @"SELECT OLA.LEVEL_APPROVE_CD,OLA.LEVEL_APPROVE_NAME,OLA.TRADE_PROGRAM_CD, OLA.PARENT_LEVEL_CD,OTPC.TRADE_PROGRAM_CUSTOMER_CD,
            //                                        CASE 
            //	                                        WHEN  OLA.PST_CD = MU.POSITION_CD AND 
            //	                                        NOT EXISTS(SELECT * FROM O_CONDITION_VALUE A  WITH(NOLOCK, READUNCOMMITTED)
            //				                                        INNER JOIN O_CONDITION OC WITH(NOLOCK, READUNCOMMITTED) ON A.CONDITION_CD=OC.CONDITION_CD 
            //				                                        WHERE A.TRADE_PROGRAM_CUSTOMER_CD=19331 AND OC.LEVEL_APPROVE_CD=OLA.LEVEL_APPROVE_CD) 
            //				                                        AND
            //				                                        (OLA.PARENT_LEVEL_CD = 0 OR  EXISTS(SELECT * FROM O_CONDITION_VALUE tmp1 WITH(NOLOCK, READUNCOMMITTED)
            //													                                        INNER JOIN O_CONDITION tmp2 WITH(NOLOCK, READUNCOMMITTED) ON tmp1.CONDITION_CD=tmp2.CONDITION_CD
            //													                                        WHERE tmp2.LEVEL_APPROVE_CD=OLA.PARENT_LEVEL_CD AND TMP1.TRADE_PROGRAM_CUSTOMER_CD=OTPC.TRADE_PROGRAM_CUSTOMER_CD))
            //                                        THEN 1 ELSE 0 
            //                                        END AS 'CHECK'
            //                                        FROM O_TRADE_PROGRAM_CUSTOMER OTPC WITH(NOLOCK, READUNCOMMITTED)
            //                                        INNER JOIN O_TRADE_PROGRAM OTP WITH(NOLOCK, READUNCOMMITTED) ON OTPC.TRADE_PROGRAM_CD=OTP.TRADE_PROGRAM_CD
            //                                        INNER JOIN O_LEVEL_APPROVE OLA WITH(NOLOCK, READUNCOMMITTED) ON OTP.TRADE_PROGRAM_CD=OLA.TRADE_PROGRAM_CD AND OLA.ACTIVE=1 
            //                                        INNER JOIN M_USER MU WITH(NOLOCK, READUNCOMMITTED) ON 1=1 AND USER_CD=112
            //                                        WHERE TRADE_PROGRAM_CUSTOMER_CD=19331
            //                                        ORDER BY OLA.ORDERBY ASC";
            String strGetLevelApprove = @"SELECT OLA.LEVEL_APPROVE_CD,OLA.LEVEL_APPROVE_NAME,OLA.TRADE_PROGRAM_CD, OLA.PARENT_LEVEL_CD,OTPC.TRADE_PROGRAM_CUSTOMER_CD,
                                        CASE 
	                                        WHEN  OLA.PST_CD = MU.POSITION_CD AND 
	                                        NOT EXISTS(SELECT * FROM O_CONDITION_VALUE A  WITH(NOLOCK, READUNCOMMITTED)
				                                        INNER JOIN O_CONDITION OC WITH(NOLOCK, READUNCOMMITTED) ON A.CONDITION_CD=OC.CONDITION_CD 
				                                        WHERE A.TRADE_PROGRAM_CUSTOMER_CD={0} AND OC.LEVEL_APPROVE_CD=OLA.LEVEL_APPROVE_CD) 
														THEN 1 ELSE 0 
                                        END AS 'CHECK'
                                        FROM O_TRADE_PROGRAM_CUSTOMER OTPC WITH(NOLOCK, READUNCOMMITTED)
                                        INNER JOIN O_TRADE_PROGRAM OTP WITH(NOLOCK, READUNCOMMITTED) ON OTPC.TRADE_PROGRAM_CD=OTP.TRADE_PROGRAM_CD
                                        INNER JOIN O_LEVEL_APPROVE OLA WITH(NOLOCK, READUNCOMMITTED) ON OTP.TRADE_PROGRAM_CD=OLA.TRADE_PROGRAM_CD AND OLA.ACTIVE=1 
                                        INNER JOIN M_USER MU WITH(NOLOCK, READUNCOMMITTED) ON 1=1 AND USER_CD={1}
                                        WHERE TRADE_PROGRAM_CUSTOMER_CD={0}
                                        ORDER BY OLA.ORDERBY ASC
                                        ";

            String strGetCondition = @"SELECT OC.LEVEL_APPROVE_CD,OC.CONDITION_CD,OC.CONDITION_TYPE_CD,OC.CONDITION_NAME,ISNULL(OCV.VALUE,'') AS VALUE, ISNULL(OCV.COMBOBOX_VALUE_CD,0) AS COMBOBOX_VALUE_CD, ISNULL(OCV.CONDITION_VALUE_CD,0) AS CONDITION_VALUE_CD
                                        FROM O_TRADE_PROGRAM_CUSTOMER OTPC WITH(NOLOCK, READUNCOMMITTED)
                                        INNER JOIN O_TRADE_PROGRAM OTP WITH(NOLOCK, READUNCOMMITTED) ON OTPC.TRADE_PROGRAM_CD=OTP.TRADE_PROGRAM_CD
                                        INNER JOIN O_LEVEL_APPROVE OLA WITH(NOLOCK, READUNCOMMITTED) ON OTP.TRADE_PROGRAM_CD=OLA.TRADE_PROGRAM_CD  AND OLA.ACTIVE=1
                                        INNER JOIN O_CONDITION OC WITH(NOLOCK, READUNCOMMITTED) ON OLA.LEVEL_APPROVE_CD=OC.LEVEL_APPROVE_CD AND OC.ACTIVE=1
										LEFT JOIN O_CONDITION_VALUE OCV WITH(NOLOCK, READUNCOMMITTED) ON OC.CONDITION_CD=OCV.CONDITION_CD AND OCV.ACTIVE=1 AND OCV.TRADE_PROGRAM_CUSTOMER_CD=@0
                                        WHERE OTPC.TRADE_PROGRAM_CUSTOMER_CD=@0
                                        ORDER BY OLA.ORDERBY ASC,OC.ORDERBY ASC";

            String strGetComboValue = @"SELECT OCV.CONDITION_CD,OCV.COMBOBOX_VALUE_CD,COMBOBOX_VALUE
                                        FROM O_TRADE_PROGRAM_CUSTOMER OTPC WITH(NOLOCK, READUNCOMMITTED)
                                        INNER JOIN O_TRADE_PROGRAM OTP WITH(NOLOCK, READUNCOMMITTED) ON OTPC.TRADE_PROGRAM_CD=OTP.TRADE_PROGRAM_CD
                                        INNER JOIN O_LEVEL_APPROVE OLA WITH(NOLOCK, READUNCOMMITTED) ON OTP.TRADE_PROGRAM_CD=OLA.TRADE_PROGRAM_CD  AND OLA.ACTIVE=1
                                        INNER JOIN O_CONDITION OC WITH(NOLOCK, READUNCOMMITTED) ON OLA.LEVEL_APPROVE_CD=OC.LEVEL_APPROVE_CD AND OC.ACTIVE=1 AND OC.CONDITION_TYPE_CD=2
                                        INNER JOIN O_COMBOBOX_VALUE OCV WITH(NOLOCK, READUNCOMMITTED) ON OC.CONDITION_CD=OCV.CONDITION_CD AND OCV.ACTIVE=1
                                        WHERE TRADE_PROGRAM_CUSTOMER_CD=@0 
                                        ORDER BY OLA.ORDERBY ASC,OC.ORDERBY ASC,OCV.ORDERBY ASC";

            //DataTable tbLevelApprove = L5sSql.Query(strGetLevelApprove, ViewState["TRADE_PROGRAM_CUSTOMER_CD"].ToString(), Session["F5sUsrCD"].ToString());
            String string_format = string.Format(strGetLevelApprove, ViewState["TRADE_PROGRAM_CUSTOMER_CD"].ToString(), Session["F5sUsrCD"].ToString());
            // DataTable tbLevelApprove = L5sSql.Query(strGetLevelApprove, ViewState["TRADE_PROGRAM_CUSTOMER_CD"].ToString(), 1);
            DataTable tbLevelApprove = L5sSql.Query(string_format);

            DataTable tbCondition = L5sSql.Query(strGetCondition, ViewState["TRADE_PROGRAM_CUSTOMER_CD"].ToString());

            DataTable tbComboValue = L5sSql.Query(strGetComboValue, ViewState["TRADE_PROGRAM_CUSTOMER_CD"].ToString());

            this.P5sHdfData.Value = ConvertDataTableToJSon(tbLevelApprove) + "❂" + ConvertDataTableToJSon(tbCondition) + "❂" + ConvertDataTableToJSon(tbComboValue);
            L5sJS.L5sRun("LoadLevelApprove();");
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


        public void P5sLoadImage()
        {

            String sql = String.Format(@"SELECT 
                                                TRADE_PROGRAM_PHOTO_CD,PHOTO_NAME AS ImageName, '/FileUpload/TradeProgram/' + PHOTO_NAME AS ImagePath, 
                                                CONVERT(VARCHAR,PHOTO_CREATED_DATE, 103)  + ' ' +  CONVERT(VARCHAR,PHOTO_CREATED_DATE, 108) AS ImageTakeDay,
                                                ISNULL(PHOTO_NOTES_1,'') AS PHOTO_NOTES_1,
                                                ISNULL(PHOTO_NOTES_2,'') AS PHOTO_NOTES_2,
                                                ISNULL(PHOTO_NOTES_3,'') AS PHOTO_NOTES_3,
                                                ISNULL(PHOTO_NOTES_4,'') AS PHOTO_NOTES_4,
                                                ISNULL(PHOTO_NOTES_5,'') AS PHOTO_NOTES_5                                                                            

                                       FROM O_TRADE_PROGRAM_PHOTO WITH(NOLOCK, READUNCOMMITTED)
                                       WHERE TRADE_PROGRAM_CUSTOMER_CD = {0} AND ACTIVE = 1
                        ", ViewState["TRADE_PROGRAM_CUSTOMER_CD"].ToString());


            DataTable dt = L5sSql.Query(sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                this.P5sHdfDataImage.Value = "";
                return;
            }

            String javascriptValue = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                javascriptValue += System.Environment.NewLine + " $P5s('#Pgwimg_" + (i + 1) + "').ezPlus();";
            }


            String script = @"$P5s(document).ready(function($){ 
                                                       " + javascriptValue + "    });";

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), System.Guid.NewGuid().ToString(), script, true);

            this.P5sHdfDataImage.Value = P5sCmmFns.P5sConvertDataTableToJSONString(dt);



            sql = @"SELECT * 
                           FROM O_TRADE_PROGRAM_CUSTOMER WITH(NOLOCK, READUNCOMMITTED) WHERE TRADE_PROGRAM_CUSTOMER_CD = @0";
            dt = L5sSql.Query(sql, ViewState["TRADE_PROGRAM_CUSTOMER_CD"].ToString());



        }


        protected void P5sLbtnSearch_Click(object sender, EventArgs e)
        {
            this.P5sSqlSelect = this.P5sSqlSelectBase; // set default sql SELECT

            //String[] dSR = new String[P5sTxtDistributor_CD.Items.Count];
            //int f = 0;
            //for (int i = 0; i < P5sTxtDistributor_CD.Items.Count; i++)
            //{

            //    if (P5sTxtDistributor_CD.Items[i].Selected == true)
            //    {
            //        dSR[f] = P5sTxtDistributor_CD.Items[i].Value;
            //        f++;
            //    }
            //}
            String dsR = "";
            //for (int i = 0; i < f; i++)
            //{
            //    dsR += dSR[i];
            //    if (i < f - 1)
            //        dsR += ",";
            //}
            dsR = this.P5sDDLDistributor.SelectedValue;
            if (string.IsNullOrEmpty(P5sTxtTradeProgram_CD.SelectedValue) || string.IsNullOrEmpty(dsR))
            {
                L5sMsg.Show("All fields marked with an asterisk (*) are required!");
                return;
            }

            String[] sales = new String[P5sDDLDistributor.Items.Count];
            int fl = 0;
            for (int i = 0; i < P5sTxtSales_CD.Items.Count; i++)
            {

                if (P5sTxtSales_CD.Items[i].Selected == true)
                {
                    sales[fl] = P5sTxtSales_CD.Items[i].Value;
                    fl++;
                }
            }
            String Sales = "";
            for (int i = 0; i < fl; i++)
            {
                Sales += sales[i];
                if (i < fl - 1)
                    Sales += ",";
            }

            if (Sales != "")
            {
                this.P5sSqlSelect = this.P5sSqlSelect.Replace("(1=1)", String.Format(" sls.SALES_CD IN ({0}) ", Sales));
            }
            else
            {
                if (dsR != "")
                {
                    this.P5sSqlSelect = this.P5sSqlSelect.Replace("(1=1)", String.Format(@" sls.DISTRIBUTOR_CD IN ({0})", dsR));
                }
                else
                   if (this.P5sDDLArea.SelectedValue.Trim() != "")
                {
                    this.P5sDDLArea.SelectedValue = this.P5sDDLArea.SelectedValue.Trim();
                    this.P5sSqlSelect = this.P5sSqlSelect.Replace("(1=1)", String.Format(" distInfo.AREA_CD IN ({0}) ", this.P5sDDLArea.SelectedValue.Trim()));
                }
                else
                       if (this.P5sDDLRegion.SelectedValue.Trim() != "")
                {
                    this.P5sDDLRegion.SelectedValue = this.P5sDDLRegion.SelectedValue.Trim();
                    this.P5sSqlSelect = this.P5sSqlSelect.Replace("(1=1)", String.Format(" distInfo.REGION_CD IN ({0}) ", this.P5sDDLRegion.SelectedValue.Trim()));
                }
            }
               


            if (this.P5sTxtTradeProgram_CD.SelectedValue.Trim() != "")
            {
                this.P5sTxtTradeProgram_CD.SelectedValue = this.P5sTxtTradeProgram_CD.SelectedValue.Trim();
                this.P5sSqlSelect = this.P5sSqlSelect.Replace("(2=2)", String.Format(" tradeProgram.TRADE_PROGRAM_CD IN ({0}) ", this.P5sTxtTradeProgram_CD.SelectedValue.Trim()));
            }


            String AMSFrom = this.P5sTxtAMSFrom.Text.Trim().Replace(",", "");
            String AMSTo = this.P5sTxtAMSTo.Text.Trim().Replace(",", "");
            if (AMSFrom.Length > 0 && AMSTo.Length > 0)
                this.P5sSqlSelect = this.P5sSqlSelect.Replace("(3=3)", String.Format(" T.AMS BETWEEN {0} AND {1} ", AMSFrom, AMSTo));
            else
               if (AMSFrom.Length > 0)
                this.P5sSqlSelect = this.P5sSqlSelect.Replace("(3=3)", String.Format(" T.AMS >= {0} ", AMSFrom));
            else
                    if (AMSTo.Length > 0)
                this.P5sSqlSelect = this.P5sSqlSelect.Replace("(3=3)", String.Format(" T.AMS <= {0} ", AMSTo));



            ViewState["SqlSearch"] = this.P5sSqlSelect;

            this.P5sPnlMainDetail.Visible = false;


            //clear all value
            this.P5sHdfDataImage.Value = "";
            this.P5sMainGridViewInit();


        }


        protected void P5sLbtnRemovePictures_Click(object sender, EventArgs e)
        {
            String tradeProgramPhotoCD = this.P5sHdfImageCDSelected.Value.Trim();
            if (tradeProgramPhotoCD.Length <= 0)
                return;

            String sql = @"
                            DECLARE @TRADE_PROGRAM_CUSTOMER_CD  BIGINT
                            SELECT @TRADE_PROGRAM_CUSTOMER_CD = TRADE_PROGRAM_CUSTOMER_CD 
                            FROM O_TRADE_PROGRAM_PHOTO  
                            WHERE TRADE_PROGRAM_PHOTO_CD = @0

                            UPDATE O_TRADE_PROGRAM_PHOTO SET ACTIVE = 0 
                            WHERE TRADE_PROGRAM_PHOTO_CD = @0

                            UPDATE [O_TRADE_PROGRAM_CUSTOMER] SET [NUMBER_PHOTO] = [NUMBER_PHOTO] - 1
                            WHERE TRADE_PROGRAM_CUSTOMER_CD = @TRADE_PROGRAM_CUSTOMER_CD      
                            

                          ";
            L5sSql.Execute(sql, tradeProgramPhotoCD);
            L5sMsg.Show("Remove photo sucessfull!");
            this.P5sLoadImage(); //load lại hình ảnh sau khi xóa
        }


        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {


            this.P5sPnlInfo.Visible = this.P5sPnlMainDetail.Visible;

            try
            {
                
                //this.P5sActRegion.L5sSetDefaultValues(this.P5sTxtRegionCD.Text);
                //this.P5sActArea.L5sSetDefaultValues(this.P5sTxtAreaCD.Text);
                //this.P5sActDistributor.L5sSetDefaultValues(this.P5sTxtDistributorCD.Text);
                //this.P5sActSales.L5sSetDefaultValues(this.P5sTxtSalesCD.Text);
                //this.P5sActTradeProgram.L5sSetDefaultValues(this.P5sTxtTradeProgramCD.Text);

            }
            catch (Exception)
            {

            }

            if (ViewState["HideShowFilter"] != null && ViewState["HideShowFilter"].ToString().Equals("0"))
            {
                String script = @"$P5s(document).ready(function($){ document.getElementById('TdPnlFilter').style.display = 'none'; });";
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), System.Guid.NewGuid().ToString(), script, true);
            }
            else
            {
                String script = @"$P5s(document).ready(function($){ document.getElementById('TdPnlFilter').style.display = 'block'; });";
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), System.Guid.NewGuid().ToString(), script, true);
            }



        }


        protected void P5sLbtnCancel_Click(object sender, EventArgs e)
        {
            this.P5sMainFrm.L5sCloseForm();
        }

        protected void P5sTxtFrom_TextChanged(object sender, EventArgs e)
        {
            this.P5sTxtTradeProgramCD.Text = "";
        }

        protected void P5sTxtTo_TextChanged(object sender, EventArgs e)
        {
            this.P5sTxtTradeProgramCD.Text = "";
        }
        //  [WebMethod()]
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public static String ResultTradeProgram(String result)
        {
            try
            {
                if (HttpContext.Current.Session["F5sUsrCD"] == null ||
                    String.IsNullOrEmpty(HttpContext.Current.Session["F5sUsrCD"].ToString()))
                {
                    return "Err00100";
                }
                String userCD = HttpContext.Current.Session["F5sUsrCD"].ToString();
                JObject json = JObject.Parse(result);
                String levelApprove = json["Level"].ToString();
                String TradeProCustCD = json["TRADE_PROGRAM_CUSTOMER_CD"].ToString();
                //String tradeProgramCustomerCD = HttpContext.Current.Session["TRADE_PROGRAM_CUSTOMER_CD"].ToString();
                // KIEM TRA QUYEN APPROVE
                String StrCheckPST = @"SELECT OLA.PST_CD FROM O_TRADE_PROGRAM_CUSTOMER OTPC
                                        INNER JOIN O_TRADE_PROGRAM OTP ON OTPC.TRADE_PROGRAM_CD=OTP.TRADE_PROGRAM_CD
                                        INNER JOIN O_LEVEL_APPROVE OLA ON OTP.TRADE_PROGRAM_CD=OLA.TRADE_PROGRAM_CD
                                        WHERE OLA.LEVEL_APPROVE_CD=@0 AND OTPC.TRADE_PROGRAM_CUSTOMER_CD=@1";
                String StrGetPSTUser = @"SELECT POSITION_CD FROM M_USER WHERE USER_CD=@0";

                if (!L5sSql.Query(StrCheckPST, levelApprove, TradeProCustCD).Rows[0][0].ToString().Trim().Equals(L5sSql.Query(StrGetPSTUser, userCD).Rows[0][0].ToString().Trim()))
                {
                    return "Err00101";
                }
                // KIEM TRA XEM LEVEL APPROVE
                String StrCheckapprove = @"SELECT LEVEL_APPROVE_CD FROM O_LEVEL_APPROVE A
                                            WHERE LEVEL_APPROVE_CD=@0 AND (PARENT_LEVEL_CD=0 OR PARENT_LEVEL_CD IS NULL OR EXISTS (SELECT VALUE FROM O_CONDITION_VALUE V
                                            INNER JOIN O_CONDITION C ON V.CONDITION_CD=C.CONDITION_CD AND C.LEVEL_APPROVE_CD=A.PARENT_LEVEL_CD AND V.TRADE_PROGRAM_CUSTOMER_CD=@1
                                            ))";

                DataTable tbtmp = L5sSql.Query(StrCheckapprove, levelApprove, TradeProCustCD);
                if (tbtmp == null || tbtmp.Rows.Count == 0)
                {
                    return "Err00102";
                }
                String getInfo = @"SELECT OC.LEVEL_APPROVE_CD,OC.CONDITION_CD,OC.CONDITION_TYPE_CD, OC.CONDITION_NAME,ISNULL(OCV.VALUE,'') AS VALUE, ISNULL(OCV.COMBOBOX_VALUE_CD,0) AS COMBOBOX_VALUE_CD, ISNULL(OCV.CONDITION_VALUE_CD,0) AS CONDITION_VALUE_CD
                            FROM O_TRADE_PROGRAM_CUSTOMER OTPC
                            INNER JOIN O_TRADE_PROGRAM OTP ON OTPC.TRADE_PROGRAM_CD=OTP.TRADE_PROGRAM_CD
                            INNER JOIN O_LEVEL_APPROVE OLA ON OTP.TRADE_PROGRAM_CD=OLA.TRADE_PROGRAM_CD  AND OLA.ACTIVE=1
                            INNER JOIN O_CONDITION OC ON OLA.LEVEL_APPROVE_CD=OC.LEVEL_APPROVE_CD AND OC.ACTIVE=1
                            LEFT JOIN O_CONDITION_VALUE OCV ON OC.CONDITION_CD=OCV.CONDITION_CD AND OCV.ACTIVE=1 AND OCV.TRADE_PROGRAM_CUSTOMER_CD=@0
                            WHERE OTPC.TRADE_PROGRAM_CUSTOMER_CD=@0 AND OC.LEVEL_APPROVE_CD=@1
                            ORDER BY OLA.ORDERBY ASC,OC.ORDERBY ASC";
                String insert = @" INSERT INTO O_CONDITION_VALUE(CONDITION_CD,VALUE,CONDITION_TYPE_CD,COMBOBOX_VALUE_CD,USER_CD,TRADE_PROGRAM_CUSTOMER_CD) 
                                        VALUES({0},N'{1}',{2},{3},{4},{5})";
                DataTable tb = L5sSql.Query(getInfo, TradeProCustCD, levelApprove);
                String StrSqlRun = "", value = "";
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    if (int.Parse(tb.Rows[i]["CONDITION_VALUE_CD"].ToString()) > 0)
                    {
                        return "Err00103";
                    }
                    try
                    {
                        value = json[tb.Rows[i]["CONDITION_CD"].ToString()].ToString();
                        if (String.IsNullOrEmpty(value))
                        {
                            return "Err00104";
                        }
                        if (tb.Rows[i]["CONDITION_TYPE_CD"].ToString().Equals("1"))
                        {
                            StrSqlRun = StrSqlRun + String.Format(insert, tb.Rows[i]["CONDITION_CD"].ToString(), value, tb.Rows[i]["CONDITION_TYPE_CD"].ToString(), 0, userCD, TradeProCustCD);
                        }
                        else
                            if (tb.Rows[i]["CONDITION_TYPE_CD"].ToString().Equals("2"))
                        {
                            StrSqlRun = StrSqlRun + String.Format(insert, tb.Rows[i]["CONDITION_CD"].ToString(), L5sSql.Query("SELECT COMBOBOX_VALUE FROM O_COMBOBOX_VALUE WHERE COMBOBOX_VALUE_CD =@0", value).Rows[0][0].ToString(), tb.Rows[i]["CONDITION_TYPE_CD"].ToString(), value, userCD, TradeProCustCD);

                        }
                    }
                    catch { return "Err00105"; }
                }
                if (!string.IsNullOrEmpty(StrSqlRun))
                {
                    StrSqlRun = @"DECLARE @tradeprogramcustomer int
                        DECLARE @levelapprove int
                        SET @tradeprogramcustomer=@0
                        SET @levelapprove=@1
                        IF NOT EXISTS(
                        SELECT OCV.CONDITION_VALUE_CD FROM O_CONDITION_VALUE OCV
                        INNER JOIN O_CONDITION OC ON OCV.CONDITION_CD=OC.CONDITION_CD
                        WHERE OCV.TRADE_PROGRAM_CUSTOMER_CD=@tradeprogramcustomer AND  OC.LEVEL_APPROVE_CD=@levelapprove
                        )
                        BEGIN " + StrSqlRun + " END";
                    L5sSql.Query(StrSqlRun, TradeProCustCD, levelApprove);
                    return "T";
                }
                else
                {
                    return "Err00106";
                }

            }
            catch { return "Err00107"; }
        }

        public string ConvertDataTableToJSon(DataTable dt)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        protected void P5sDDLRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            String index = this.P5sDDLRegion.SelectedValue;
            P5sGetDataArea(index);

            string area = this.P5sDDLArea.SelectedValue;
            P5sGetDataDistributor(area);

            string dsR = this.P5sDDLDistributor.SelectedValue;
            P5sGetDataSales(dsR);
            //String[] dSR = new String[P5sTxtDistributor_CD.Items.Count];
            //int f = 0;
            //for (int i = 0; i < P5sTxtDistributor_CD.Items.Count; i++)
            //{

            //    if (P5sTxtDistributor_CD.Items[i].Selected == true)
            //    {
            //        dSR[f] = P5sTxtDistributor_CD.Items[i].Value;
            //        f++;
            //    }
            //}
            //String dsR = "";
            //for (int i = 0; i < f; i++)
            //{
            //    dsR += dSR[i];
            //    if (i < f - 1)
            //        dsR += ",";
            //}
            //P5sGetDataSales(dsR);
        }

        protected void P5sDDLArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            string area = P5sDDLArea.SelectedValue;
            P5sGetDataDistributor(area);

            string dsR = P5sDDLDistributor.SelectedValue;
            P5sGetDataSales(dsR);
            //String[] dSR = new String[P5sTxtDistributor_CD.Items.Count];
            //int f = 0;
            //for (int i = 0; i < P5sTxtDistributor_CD.Items.Count; i++)
            //{

            //    if (P5sTxtDistributor_CD.Items[i].Selected == true)
            //    {
            //        dSR[f] = P5sTxtDistributor_CD.Items[i].Value;
            //        f++;
            //    }
            //}
            //String dsR = "";
            //for (int i = 0; i < f; i++)
            //{
            //    dsR += dSR[i];
            //    if (i < f - 1)
            //        dsR += ",";
            //}
            //P5sGetDataSales(dsR);
        }

        protected void P5sDDLDistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dsR = P5sDDLDistributor.SelectedValue;
            P5sGetDataSales(dsR);
        }

        //protected void Distributor_ServerChange(object sender, EventArgs e)
        //{
        //    String[] dSR = new String[P5sTxtDistributor_CD.Items.Count];
        //    int f = 0;
        //    for (int i = 0; i < P5sTxtDistributor_CD.Items.Count; i++)
        //    {

        //        if (P5sTxtDistributor_CD.Items[i].Selected == true)
        //        {
        //            dSR[f] = P5sTxtDistributor_CD.Items[i].Value;
        //            f++;
        //        }
        //    }
        //    String dsR = "";
        //    for (int i = 0; i < f; i++)
        //    {
        //        dsR += dSR[i];
        //        if (i < f - 1)
        //            dsR += ",";
        //    }
        //    P5sGetDataSales(dsR);
        //}

        protected void P5sGetDataArea(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@" 
                                            SELECT  AREA_CD,AREA_CODE, '', ACTIVE 
                                            FROM  M_AREA 
                                            WHERE  AREA_CD IN (  SELECT are.AREA_CD
                                                                        FROM  M_AREA are  INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                                                        WHERE are.ACTIVE = 1  AND EXISTS ( SELECT * FROM
										                                                                            M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										                                            INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										                                            INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
								                                            WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1 and
								                                            are.REGION_CD = {0}
                                                                                
								                                            ) 
					                                            )

                                            ORDER BY AREA_ORDER", p);
            DataTable dt = L5sSql.Query(P5sSqlSelect);
            P5sDDLArea.DataSource = dt;
            P5sDDLArea.DataTextField = "AREA_CODE";
            P5sDDLArea.DataValueField = "AREA_CD";
            P5sDDLArea.DataBind();
        }

        protected void P5sGetDataRegion()
        {
            String P5sSqlSelect = String.Format(@"
                                        SELECT DISTINCT reg.REGION_CD,reg.REGION_CODE, '', reg.ACTIVE  
                                         FROM M_REGION reg INNER JOIN M_AREA are ON reg.REGION_CD  = are.REGION_CD AND are.ACTIVE = 1
	                                 INNER JOIN M_AREA_PROVINCE arePro ON are.AREA_CD = arePro.AREA_CD AND arePro.ACTIVE = 1
                                WHERE reg.ACTIVE = 1 AND EXISTS ( SELECT * FROM
										                                  M_PROVINCE pro	 INNER JOIN M_DISTRICT distr ON pro.PROVINCE_CD = distr.PROVINCE_CD
										  INNER JOIN M_COMMUNE cmm ON distr.DISTRICT_CD = cmm.DISTRICT_CD
										  INNER JOIN M_DISTRIBUTOR dis ON cmm.COMMUNE_CD = dis.COMMUNE_CD
									WHERE pro.PROVINCE_CD = arePro.PROVINCE_CD AND dis.ACTIVE = 1
								 )  ORDER BY REGION_CD");
            DataTable dtRegion = L5sSql.Query(P5sSqlSelect);
            P5sDDLRegion.DataSource = dtRegion;
            P5sDDLRegion.DataTextField = "REGION_CODE";
            P5sDDLRegion.DataValueField = "REGION_CD";
            P5sDDLRegion.DataBind();
        }

        protected void P5sTxtFrom_TextChanged1(object sender, EventArgs e)
        {
            DateTime from = DateTime.ParseExact(Request[P5sTxtFrom.UniqueID], "dd-MM-yyyy", null);
            DateTime to = DateTime.ParseExact(Request[P5sTxtTo.UniqueID], "dd-MM-yyyy", null);
            P5sGetTradeProgram(from, to, 1);
        }

        protected void P5sTxtTo_TextChanged1(object sender, EventArgs e)
        {
            DateTime from = DateTime.ParseExact(Request[P5sTxtFrom.UniqueID], "dd-MM-yyyy", null);
            DateTime to = DateTime.ParseExact(Request[P5sTxtTo.UniqueID], "dd-MM-yyyy", null);
            P5sGetTradeProgram(from, to, 1);
        }

        protected void P5sGetDataDistributor(string p)
        {
            p = p == "" ? @"' '" : p;


            String P5sSqlSelect = String.Format(@"SELECT DISTINCT dis.DISTRIBUTOR_CD,dis.DISTRIBUTOR_CODE + '-'+ dis.DISTRIBUTOR_NAME as DISTRI_NAME, '', dis.ACTIVE 
                                                FROM M_DISTRIBUTOR dis INNER JOIN M_COMMUNE cmm ON dis.COMMUNE_CD = cmm.COMMUNE_CD
                                                INNER JOIN M_DISTRICT dist ON cmm.DISTRICT_CD = dist.DISTRICT_CD
                                                INNER JOIN M_PROVINCE pro ON dist.PROVINCE_CD = pro.PROVINCE_CD
                                                INNER JOIN M_AREA_PROVINCE arePro ON pro.PROVINCE_CD = arePro.PROVINCE_CD AND arePro.ACTIVE = 1
                                                AND dis.DISTRIBUTOR_TYPE_CD = 1
                                                JOIN M_AREA are on are.AREA_CD = arePro.AREA_CD
                                                where are.AREA_CD = {0} and dis.ACTIVE = 1 ", p);
            DataTable dtDistri = L5sSql.Query(P5sSqlSelect);
            P5sDDLDistributor.DataSource = dtDistri;
            P5sDDLDistributor.DataTextField = "DISTRI_NAME";
            P5sDDLDistributor.DataValueField = "DISTRIBUTOR_CD";
            P5sDDLDistributor.DataBind();
        }

        protected void P5sGetDataSales(string p)
        {
            p = p == "" ? @"' '" : p;
            String P5sSqlSelect = String.Format(@"SELECT sls.SALES_CD,sls.SALES_CODE + '-'+ sls.SALES_NAME as NAME_SALE, '', sls.ACTIVE 
                                                FROM  M_SALES sls INNER JOIN M_DISTRIBUTOR dist ON sls.DISTRIBUTOR_CD = dist.DISTRIBUTOR_CD
                                                where dist.DISTRIBUTOR_CD in ({0}) and sls.ACTIVE = 1
                                                ORDER BY SALES_CODE
                                                 ", p);
            DataTable dtSale = L5sSql.Query(P5sSqlSelect);
            P5sTxtSales_CD.DataSource = dtSale;
            P5sTxtSales_CD.DataTextField = "NAME_SALE";
            P5sTxtSales_CD.DataValueField = "SALES_CD";
            P5sTxtSales_CD.DataBind();
        }

        protected void P5sGetTradeProgram(DateTime from, DateTime to, int type)
        {
            String sql = String.Format(@"SELECT TRADE_PROGRAM_CD,TRADE_PROGRAM_CODE + '-'+ TRADE_PROGRAM_NAME as NAME_PROGRAM,'',1 
                                        FROM  O_TRADE_PROGRAM 
                                        WHERE  CONVERT(Date, START_DATE , 103) >= '{0}'
                                               AND CONVERT(Date, END_DATE , 103) <= '{1}' AND TYPE = {2}
                                        ORDER BY TRADE_PROGRAM_CODE , START_DATE DESC ",
                                       from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), type);
            DataTable dt = L5sSql.Query(sql);
            P5sTxtTradeProgram_CD.DataSource = dt;
            P5sTxtTradeProgram_CD.DataTextField = "NAME_PROGRAM";
            P5sTxtTradeProgram_CD.DataValueField = "TRADE_PROGRAM_CD";
            P5sTxtTradeProgram_CD.DataBind();
        }
    }
}
