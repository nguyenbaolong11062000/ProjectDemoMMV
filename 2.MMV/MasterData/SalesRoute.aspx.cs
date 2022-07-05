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


namespace MMV.MasterData
{
    public partial class SalesRoute : System.Web.UI.Page
    {
        public L5sForm P5sMainFrm;
        public L5sPaging P5sMainPaging;
        public L5sTextboxFilter P5sTxtFtr;

        public L5sAutocomplete P5sActROUTE;
        public L5sAutocomplete P5sActSALES;

        String[] P5sControlNameInitAutoComplete = new String[] { "P5sFilterTxt", "P5sSearch","P5sLbtnAddRoute" ,"P5sDdlMainPaging", "P5sEdit", 
            "P5sImgCanncelConfirmPopup", "P5sLbtnConfirmMoveRoute"};

        String P5sSqlSelect = @"SELECT  sr.SALES_ROUTE_CD,sr.ROUTE_CD,r.ROUTE_CODE,r.ROUTE_NAME,
                                        sr.ACTIVE,sr.SEQUENCE,r.ROUTE_FREQ,
                                        r.ROUTE_CODE +'-'+r.ROUTE_NAME AS ROUTE_DESC,
		                                sr.SALES_CD,  s.SALES_CODE +'-'+   s.SALES_NAME AS SALES_DESC,
		                                d.DISTRIBUTOR_CODE +'-'+   d.DISTRIBUTOR_NAME AS DISTRIBUTOR_DESC                                		
                                FROM M_SALES s INNER JOIN  O_SALES_ROUTE sr  on s.SALES_CD = sr.SALES_CD 
                                     INNER JOIN M_ROUTE r on sr.ROUTE_CD = r.ROUTE_CD
                                     INNER JOIN M_DISTRIBUTOR d on s.DISTRIBUTOR_CD = d.DISTRIBUTOR_CD WHERE (1=1) and sr.DEACTIVE_DATE is null ";

        String P5sSqlInsert = "";
        String P5sSqlUpdate = "";
        String P5sSqlDelete = "";
        String P5sSqlOrderBy = "r.ROUTE_CODE, s.SALES_CODE ";


        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);

      
            if (!IsPostBack)
            {
                Object[] parameter = L5sTransferrer.L5sGetOldPageParameters();
                if (parameter != null)
                    ViewState["SALES_CD"] = parameter[0].ToString();
                else
                {
                    Response.Redirect("./Sales.aspx");
                }

                String sqlSelect = "SELECT s.SALES_CODE,s.SALES_NAME,d.DISTRIBUTOR_CODE +'-'+d.DISTRIBUTOR_NAME AS DISTRIBUTOR_DESC FROM M_SALES s INNER JOIN M_DISTRIBUTOR d on s.DISTRIBUTOR_CD = d.DISTRIBUTOR_CD WHERE s.SALES_CD =@0";
                DataTable dtableSelect = L5sSql.Query(sqlSelect, ViewState["SALES_CD"].ToString());
                if (dtableSelect == null)
                    return;

                this.P5sLblSALES_CODE.Text = dtableSelect.Rows[0]["SALES_CODE"].ToString();
                this.P5sLblSALES_NAME.Text = dtableSelect.Rows[0]["SALES_NAME"].ToString();
                this.P5sLblDISTRIBUTOR_DESC.Text = dtableSelect.Rows[0]["DISTRIBUTOR_DESC"].ToString();
                this.P5sInit();
            }
            else
            {
                if (Array.IndexOf(P5sControlNameInitAutoComplete, P5sCmmFns.P5sGetPostBackControlId(this)) != -1)
                    P5sInit();

            }



        }


        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {
            this.P5sLblEmptyRow.Visible = !this.P5sPanelMainGV.Visible;

        }

        private void P5sInit()
        {
            this.P5sMainGridViewInit();
            this.P5sAutoCompleteInit();
        }

        private void P5sAutoCompleteInit()
        {
            //Load region list to texbox P5sTxtREGION_CD  
            String sqlSelectDistributor = "SELECT s.DISTRIBUTOR_CD FROM M_SALES s WHERE s.SALES_CD = @0";

            DataTable dtableDistributor = L5sSql.Query(sqlSelectDistributor, ViewState["SALES_CD"].ToString());
            if (dtableDistributor == null)
                return;

            this.P5sActROUTE = this.P5sActROUTE == null ? new L5sAutocomplete(P5sCmmFns.P5sDtableRouteByDistributor("", dtableDistributor.Rows[0]["DISTRIBUTOR_CD"].ToString()), this.P5sTxtROUTE_CD.ClientID, 0, true) : this.P5sActROUTE;
                           
        }

        private void P5sMainGridViewInit()
        {
            this.P5sSqlSelect = this.P5sSqlSelect + String.Format("and s.SALES_CD ={0}", ViewState["SALES_CD"].ToString());
            this.P5sMainFrm = new L5sForm(this.P5sMainGridView, false, "SALES_ROUTE_CD", this.P5sPanelDetailMain, "P5sShowFormOnStatus",
                               this.P5sSqlInsert,
                               this.P5sSqlUpdate,
                               this.P5sSqlDelete,
                               new String[] { },
                               new Int32[] { });

            this.P5sMainPaging = new L5sPaging(this.P5sMainGridView, P5sEnum.PagingSize, this.P5sDdlMainPaging);


            this.P5sTxtFtr = new L5sTextboxFilter(this.P5sFilterPanel, this.P5sMainGridView,
                                        new String[] { "r.ROUTE_CODE","r.ROUTE_FREQ", "r.ROUTE_NAME" ,"d.DISTRIBUTOR_CODE + '-' + d.DISTRIBUTOR_NAME",
                                                        }, this.P5sFilterTxt);

            if (this.P5sTxtFtr.L5sIsClicked() && this.P5sFilterTxt.Text != "")
                this.P5sTxtFtr.L5sLoadFilter(this.P5sMainPaging, this.P5sSqlSelect, this.P5sSqlOrderBy, "");
            else
                this.P5sMainFrm.L5sLoadPage(this.P5sMainPaging.L5sParseSql(this.P5sSqlSelect, this.P5sSqlOrderBy));

            this.P5sMainPaging.L5sLoadPaging();

            //Hide P5sUpanelMainGV if this empty
            this.P5sPanelMainGV.Visible = this.P5sMainGridView.Rows.Count > 0 ? true : false;

        }


        public void P5sShowFormOnStatus(Int32 P5sFormStatus, Boolean P5sIsLoaded)
        {
            switch (P5sFormStatus)
            {
                case 0://read only  
                    if (P5sActROUTE != null)
                        P5sActROUTE.L5sDisable(true);
                    if (P5sActSALES != null)
                        P5sActSALES.L5sDisable(true);
                    P5sLbtnInsert.Visible = W5sLbtnRemoveRoute.Visible = P5sLbtnMoveRoute.Visible = false;
                    break;
                case 1: //new        
                    break;
                case 2: //Edit    
                    break;
                default:
                    break;
            }
        }

        protected void P5sSearch_Click(object sender, EventArgs e)
        {
            this.P5sFilterTxt.Text = this.P5sFilterTxt.Text.Trim();
            this.P5sTxtFtr.L5sFilterButton_Click(this.P5sMainPaging, this.P5sSqlSelect, this.P5sSqlOrderBy, "");
            this.P5sMainPaging.L5sLoadPaging();

        }

        protected void P5sMainGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow ||
            e.Row.RowType == DataControlRowType.Header ||
            e.Row.RowType == DataControlRowType.Footer)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                        e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 10px;");
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                        e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 10px;padding-left:10px");

                }

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;


                e.Row.Cells[0].Width = Unit.Percentage(1); //Code

                e.Row.Cells[5].Width = Unit.Percentage(1); //Active                
                e.Row.Cells[7].Width = Unit.Percentage(1); //Active

                e.Row.Cells[6].Visible = false; //So TT

                for (int i = 8; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }

                //Image button
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[14].Width = Unit.Percentage(1);
            }

        }

        protected void P5sLbtnInsert_Click(object sender, EventArgs e)
        {
            if (ViewState["SALES_CD"] == null)
                return;

            String routeCDs = this.P5sTxtROUTE_CD.Text.Trim();
            if (routeCDs == "")
            {
                this.P5sInit();
                return;
            }
   
            // deactive 
            String sqlDeactiveRoute = String.Format(@"update O_SALES_ROUTE set active=0, [DEACTIVE_DATE]='{1}' 
                                                    where ROUTE_CD in ({0}) AND [ACTIVE] =1
                                                    ", routeCDs, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            L5sSql.Query(sqlDeactiveRoute);

            String sqlInsertSales_Route = String.Format("INSERT INTO O_SALES_ROUTE(SALES_CD,ROUTE_CD,ACTIVE)  SELECT {0},r.ROUTE_CD,1 FROM M_ROUTE r WHERE r.ROUTE_CD IN ({1}) ", ViewState["SALES_CD"].ToString(), routeCDs);
            L5sSql.Query(sqlInsertSales_Route);

            this.P5sInit();
        }

        protected void W5sLbtnRemoveRoute_Click(object sender, EventArgs e)
        {
            if (ViewState["SALES_CD"] == null)
                return;

            foreach (GridViewRow gvr in this.P5sMainGridView.Rows)
            {
                CheckBox cb = (CheckBox)gvr.FindControl("RouteSelector");
                Int64 key = Convert.ToInt64(this.P5sMainGridView.DataKeys[gvr.RowIndex].Value.ToString());
                if (cb.Checked)
                {
                   
                    String sqlUpdate = @"UPDATE  O_SALES_ROUTE SET DEACTIVE_DATE = GETDATE(), ACTIVE = 0 WHERE SALES_ROUTE_CD = @0";
                    L5sSql.Query(sqlUpdate, key);
                }
            }
            this.P5sInit();

        }

        protected void P5sLbtnCancel_Click(object sender, EventArgs e)
        {

            Response.Redirect("./Sales.aspx");
        }



        protected void P5sLbtnMoveRoute_Click(object sender, EventArgs e)
        {
            if (ViewState["SALES_CD"] == null)
                return;

            int count = 0;
            String keys = "";
            foreach (GridViewRow gvr in this.P5sMainGridView.Rows)
            {
                CheckBox cb = (CheckBox)gvr.FindControl("RouteSelector");
                int key = Convert.ToInt32(this.P5sMainGridView.DataKeys[gvr.RowIndex].Value.ToString());

                if (cb.Checked)
                {
                    count++;
                    keys += key + ",";
                }
            }

            if (count != 0)
            {
                keys = keys.Remove(keys.Length - 1, 1);
                ViewState["SALES_ROUTE_CD_LIST"] = keys;
                this.ModalPopupExtender.Show();

             
                this.P5sActSALES = this.P5sActSALES == null ? new L5sAutocomplete(
                    P5sCmmFns.P5sGetSalesBySales("", ViewState["SALES_CD"].ToString())                    
                    , this.P5sTxtSELECT_NEW_SALE.ClientID, 1, true) : this.P5sActSALES;
            }
            else
            {
                L5sMsg.Show("No route selected.");
            }
            //this.P5sInit();
            this.P5sAutoCompleteInit();
        }

        protected void P5sLbtnConfirmMoveRoute_Click(object sender, EventArgs e)
        {
            if (ViewState["SALES_CD"] == null)
                return;
            if (ViewState["SALES_ROUTE_CD_LIST"] == null)
                return;
            if (this.P5sTxtSELECT_NEW_SALE.Text.Trim() == "")
            {
                this.ModalPopupExtender.Hide();
                return;
            }

            // chuyen nhan vien ban hang cho khach hang



            string sqlChangeRoute = String.Format(@"update O_SALES_ROUTE set active=0, [DEACTIVE_DATE]='{2}' where [SALES_ROUTE_CD] in ({0});


UPDATE O_SALES_ROUTE
SET
	
	[ACTIVE] = 0,
		DEACTIVE_DATE = GETDATE()
WHERE [ACTIVE]=1 AND ROUTE_CD IN  (
					SELECT ROUTE_CD
					FROM O_SALES_ROUTE osr
					WHERE osr.SALES_ROUTE_CD IN ({0}))

--insert new 

 insert into O_SALES_ROUTE ([SALES_CD], [ROUTE_CD], [ACTIVE], [SEQUENCE])
 select {1}, [ROUTE_CD], 1, [SEQUENCE] from O_SALES_ROUTE where [SALES_ROUTE_CD] in ({0})",
                ViewState["SALES_ROUTE_CD_LIST"].ToString(), P5sTxtSELECT_NEW_SALE.Text, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

            L5sSql.Execute(sqlChangeRoute);


            L5sMsg.Show("Transfer sucessfull!");
            // this.P5sPnlTabControl.Visible = false;

            this.P5sInit();
        }
        


    }
}
