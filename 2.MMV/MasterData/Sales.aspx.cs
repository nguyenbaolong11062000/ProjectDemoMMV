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
using P5sCmm;
using L5sDmComm;
using P5sDmComm;


namespace MMV.MasterData
{
    public partial class Sales : System.Web.UI.Page
    {
        public L5sForm P5sMainFrm;
        public L5sPaging P5sMainPaging;
        public L5sTextboxFilter P5sTxtFtr;


        public L5sAutocomplete P5sAcDISTRIBUTOR;
        public L5sAutocomplete P5sAcSalesType;


        String[] P5sControlNameInitAutoComplete = new String[] { "P5sEdit", "P5sLbtnNew", "P5sLbtnUpdate", "P5sLbtnInsert", "P5sLbtnChangeUseDMS", "P5sLbtnChangeOutRangeOrder" };
        String[] P5sCtrlInitNotMainGridView = new String[] { "P5sLbtnChangeUseDMS", "P5sLbtnChangeOutRangeOrder", "P5sLbtnChange5PTracking", "P5sLbtnChangeSurvey", "P5sLbtnChangeEditInfor" };

        String P5sSqlSelect = @"SELECT s.SALES_CD,s.SALES_CODE,s.SALES_NAME,s.DISTRIBUTOR_CD, d.DISTRIBUTOR_CODE + '-' + d.DISTRIBUTOR_NAME AS DISTRIBUTOR_DESC,
                                 s.ACTIVE  ,s.SALES_TYPE_CD,s.USE_DMS,s.ALLOW_OUT_RANGE_ORDER,s.USE_5PTRACKING,s.USE_SURVEY,s.EDIT_INFO                        
                                FROM M_SALES s INNER JOIN M_DISTRIBUTOR d on s.DISTRIBUTOR_CD = d.DISTRIBUTOR_CD";

        String P5sSqlInsert = "INSERT INTO M_SALES(SALES_CODE, SALES_NAME,DISTRIBUTOR_CD,ACTIVE,SALES_TYPE_CD) values(@0,@1,@2,@3,@4)";
        String P5sSqlUpdate = "UPDATE M_SALES SET SALES_NAME=@1 , DISTRIBUTOR_CD =@2, ACTIVE =@3, SALES_TYPE_CD = @4 where SALES_CD=@0 ";
        String P5sSqlDelete = "";
        String P5sSqlOrderBy = " SALES_CODE, SALES_NAME ";
        DataTable dtableDistributor;



        protected void Page_Load(object sender, EventArgs e)
        {
            L5sInitial.Load(ViewState);
            this.P5sInit();
        }

        private void P5sInit()
        {
            if (Array.IndexOf(P5sCtrlInitNotMainGridView, P5sCmm.P5sCmmFns.P5sGetPostBackControlId(this)) == -1)
                this.P5sMainGridViewInit();
            if (Array.IndexOf(P5sControlNameInitAutoComplete, P5sCmmFns.P5sGetPostBackControlId(this)) != -1)
                this.P5sAutoCompleteInit();
        }

        private void P5sAutoCompleteInit()
        {
            dtableDistributor = P5sCmmFns.P5sGetAllDistributor("AREA_CD");
            DataTable dtSalesType = P5sCmmFns.P5sGetSalesType("");

            //Load Supervisor list to texbox P5sTxtSUPERVISOR_CD      
            this.P5sAcDISTRIBUTOR = this.P5sAcDISTRIBUTOR == null ? new L5sAutocomplete(dtableDistributor, this.P5sTxtDISTRIBUTOR_CD.ClientID, 1, true) : this.P5sAcDISTRIBUTOR;

            this.P5sAcSalesType = this.P5sAcSalesType == null ? new L5sAutocomplete(dtSalesType, this.P5sTxtSALES_TYPE_CD.ClientID, 1, true) : this.P5sAcSalesType;

        }




        private void P5sMainGridViewInit()
        {

            this.P5sMainFrm = new L5sForm(this.P5sMainGridView, false, "SALES_CD", this.P5sDetailPanel, "P5sShowFormOnStatus",
                                this.P5sSqlInsert,
                                this.P5sSqlUpdate,
                                this.P5sSqlDelete,
                                new String[] { "P5sTxtSALES_CODE.Text", 
                                               "P5sTxtSALES_NAME.Text", 
                                               "P5sTxtDISTRIBUTOR_CD.Text", 
                                               "P5sACTIVE.Checked","P5sTxtSALES_TYPE_CD.Text" },
                                new Int32[] {   11, 12,13 ,14,15 });

            this.P5sMainPaging = new L5sPaging(this.P5sMainGridView, P5sEnum.PagingSize, this.P5sDdlMainPaging);


            this.P5sTxtFtr = new L5sTextboxFilter(this.P5sFilterPanel, this.P5sMainGridView,
                                        new String[] { "s.SALES_CODE", "s.SALES_NAME", "d.DISTRIBUTOR_CODE + '-' + d.DISTRIBUTOR_NAME" }, this.P5sFilterTxt);

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

        public void P5sShowFormOnStatus(Int32 P5sFormStatus, Boolean P5sIsLoaded)
        {
            this.P5sLbtnInsert.Visible = this.P5sLbtnUpdate.Visible = this.P5sACTIVE.Checked = false;
            this.P5sTxtSALES_CODE.Text = this.P5sTxtSALES_NAME.Text = this.P5sTxtDISTRIBUTOR_CD.Text = this.P5sTxtSALES_TYPE_CD.Text = "";
            this.P5sTxtSALES_CODE.Enabled = true;
            switch (P5sFormStatus)
            {
                case 0://read only 


                    if (this.P5sAcDISTRIBUTOR != null)
                        this.P5sAcDISTRIBUTOR.L5sDisable(true);

                    if (this.P5sAcSalesType != null)
                        this.P5sAcSalesType.L5sDisable(true);

                    this.P5sLbtnNew.Visible = this.P5sTxtSALES_NAME.Enabled = this.P5sTxtDISTRIBUTOR_CD.Enabled = false;

                    P5sTxtSALES_CODE.Enabled = false;
                    break;
                case 1: //new    
                    this.P5sACTIVE.Checked = true;

                    if (dtableDistributor.Rows.Count == 1)
                        this.P5sTxtDISTRIBUTOR_CD.Text = dtableDistributor.Rows[0][0].ToString();

                    this.P5sLbtnInsert.Visible = true;
                    break;
                case 2: //Edit   

                    if (P5sAcDISTRIBUTOR != null)
                        this.P5sAcDISTRIBUTOR.L5sDisable(true);

                    this.P5sLbtnUpdate.Visible = true;
                    P5sTxtSALES_CODE.Enabled = false;
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

        protected void P5sLbtnNew_Click(object sender, EventArgs e)
        {
            this.P5sMainFrm.L5sAddNewRow();
        }

        protected Boolean P5sIsVaild()
        {

            String salesCode = this.P5sTxtSALES_CODE.Text.Trim();
            String salesName = this.P5sTxtSALES_NAME.Text.Trim();

            String distributorCD = this.P5sTxtDISTRIBUTOR_CD.Text.Trim();

            String salesType = this.P5sTxtSALES_TYPE_CD.Text.Trim();

            Boolean active = P5sACTIVE.Checked;

            if (salesCode == "")
            {
                L5sMsg.Show("All fields marked with an asterisk (*) are required!");
                return false;
            }

            if (salesName == "")
            {
                L5sMsg.Show("All fields marked with an asterisk (*) are required!");
                return false;
            }



            if (distributorCD == "")
            {
                L5sMsg.Show("All fields marked with an asterisk (*) are required!");
                return false;
            }

            if (salesType == "")
            {
                L5sMsg.Show("All fields marked with an asterisk (*) are required!");
                return false;
            }


            return true;
        }


        protected void P5sLbtnInsert_Click(object sender, EventArgs e)
        {
            if (!P5sIsVaild())
                return;


            String salesName = this.P5sTxtSALES_NAME.Text.Trim();
            String distributorCD = this.P5sTxtDISTRIBUTOR_CD.Text.Trim();
            String salesCode = P5sTxtSALES_CODE.Text.Trim();
            Boolean active = P5sACTIVE.Checked;

            String salesType = this.P5sTxtSALES_TYPE_CD.Text.Trim();


            if (L5sSql.Query("SELECT SALES_CD FROM M_SALES WHERE SALES_CODE=@0", salesCode).Rows.Count != 0)
            {
                L5sMsg.Show("DSR Code existed!");
                return;
            }

            //Insert new row
            this.P5sMainFrm.L5sInsertNewRow(salesCode, salesName, distributorCD, active, salesType);
            this.P5sMainFrm.L5sLoadPage(this.P5sMainPaging.L5sParseSql(this.P5sSqlSelect, this.P5sSqlOrderBy));
            this.P5sMainFrm.L5sCloseForm();
        }


        protected void P5sLbtnUpdate_Click(object sender, EventArgs e)
        {
            if (!P5sIsVaild())
                return;

            if (ViewState["SALES_CD"] == null)
                return;

            String salesCD = ViewState["SALES_CD"].ToString();
            String salesName = this.P5sTxtSALES_NAME.Text.Trim();
            String distributorCD = this.P5sTxtDISTRIBUTOR_CD.Text.Trim();
            Boolean active = P5sACTIVE.Checked;
            String salesType = this.P5sTxtSALES_TYPE_CD.Text.Trim();




            //Insert new row
            this.P5sMainFrm.L5sUpdateRow(salesName, distributorCD, active, salesType);
            this.P5sMainFrm.L5sLoadPage(this.P5sMainPaging.L5sParseSql(this.P5sSqlSelect, this.P5sSqlOrderBy));

        }

        protected void P5sLbtnCancel_Click(object sender, EventArgs e)
        {
            this.P5sMainFrm.L5sCloseForm();
        }

        protected void P5sMainGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow ||
            e.Row.RowType == DataControlRowType.Header ||
            e.Row.RowType == DataControlRowType.Footer)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        if (i == 0)
                            e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 10px; padding-left: 10px;");
                        else
                        e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 10px;");
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                        e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;padding-right: 10px;padding-left:10px");

                }

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                e.Row.Cells[0].Width = Unit.Percentage(1); //No.
              //  e.Row.Cells[1].Width = Unit.Percentage(1); //Customer Code
                e.Row.Cells[4].Width = Unit.Percentage(1);//Active
                e.Row.Cells[5].Width = Unit.Percentage(1);
                e.Row.Cells[6].Width = Unit.Percentage(1);
                e.Row.Cells[7].Width = Unit.Percentage(1);
                e.Row.Cells[8].Width = Unit.Percentage(1);
                e.Row.Cells[9].Width = Unit.Percentage(1);


                // e.Row.Cells[4].Visible = false;
                //e.Row.Cells[5].Visible = false;
                ////e.Row.Cells[6].Visible = false;
                //e.Row.Cells[7].Visible = false;
                //e.Row.Cells[8].Visible = false;
                //e.Row.Cells[9].Visible = false;
                //e.Row.Cells[10].Width = Unit.Percentage(1); //Add route
                //e.Row.Cells[11].Visible = false;
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (i >= 11)
                        e.Row.Cells[i].Visible = false;

                }

            }
        }

        protected void P5sMainGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["SALES_CD"] = Convert.ToInt32(((GridView)sender).DataKeys[e.NewEditIndex].Value);
            this.P5sMainFrm.L5sEditRow(e.NewEditIndex);
            this.MainGridView(ViewState["SALES_CD"].ToString());
        }

        protected void P5sMainGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 index = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("AddRoute"))
            {
                String salemanCD = this.P5sMainGridView.DataKeys[index].Value.ToString();
                L5sTransferrer.L5sGoto(new Object[] { salemanCD }, "./SalesRoute.aspx");
            }

        }
        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {
            this.P5sLblEmptyRow.Visible = !this.P5sPanelMainGV.Visible;
            if (Array.IndexOf(P5sControlNameInitAutoComplete, P5sCmmFns.P5sGetPostBackControlId(this)) != -1)
            {

                this.P5sAcDISTRIBUTOR.L5sSetDefaultValues(this.P5sTxtDISTRIBUTOR_CD.Text);
                this.P5sAcSalesType.L5sSetDefaultValues(this.P5sTxtSALES_TYPE_CD.Text);
            }
            else
                this.P5sDetailPanel.Visible = false;
        }

        protected void P5sLbtnChangeUseDMS_Click(object sender, EventArgs e)
        {
           
            
                Boolean isSelected = false;
                foreach (GridViewRow row in this.P5sMainGridView.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("SalesSelector");
                    if (cb != null && cb.Checked)
                    {
                        String salesCD = this.P5sMainGridView.DataKeys[row.RowIndex].Value.ToString();
                        String sql = @"UPDATE M_SALES SET USE_DMS = ~USE_DMS WHERE SALES_CD = @0";
                        L5sSql.Execute(sql, salesCD);
                        isSelected = true;
                    }
                }
                if (!isSelected)
                {
                    L5sMsg.Show("No Sales selected.");
                    return;
                }
                else
                {
                    L5sMsg.Show("Have changed sucessfully.");
                    this.P5sMainGridViewInit();
                    this.P5sAutoCompleteInit();
                }
            
        }

        protected void P5sLbtnChangeOutRangeOrder_Click(object sender, EventArgs e)
        {


            Boolean isSelected = false;
            foreach (GridViewRow row in this.P5sMainGridView.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("SalesSelector");
                if (cb != null && cb.Checked)
                {
                    String salesCD = this.P5sMainGridView.DataKeys[row.RowIndex].Value.ToString();
                    String sql = @"UPDATE M_SALES SET ALLOW_OUT_RANGE_ORDER = ~ALLOW_OUT_RANGE_ORDER WHERE SALES_CD = @0";
                    L5sSql.Execute(sql, salesCD);
                    isSelected = true;
                }
            }
            if (!isSelected)
            {
                L5sMsg.Show("No Sales selected.");
                return;
            }
            else
            {
                L5sMsg.Show("Have changed sucessfully.");
                this.P5sMainGridViewInit();
                this.P5sAutoCompleteInit();
            }

        }

        protected void P5sLbtnChange5PTracking_Click(object sender, EventArgs e)
        {


            Boolean isSelected = false;
            foreach (GridViewRow row in this.P5sMainGridView.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("SalesSelector");
                if (cb != null && cb.Checked)
                {
                    String salesCD = this.P5sMainGridView.DataKeys[row.RowIndex].Value.ToString();
                    String sql = @"UPDATE M_SALES SET USE_5PTRACKING = ~USE_5PTRACKING WHERE SALES_CD = @0";
                    L5sSql.Execute(sql, salesCD);
                    isSelected = true;
                }
            }
            if (!isSelected)
            {
                L5sMsg.Show("No Sales selected.");
                return;
            }
            else
            {
                L5sMsg.Show("Have changed sucessfully.");
                this.P5sMainGridViewInit();
                this.P5sAutoCompleteInit();
            }

        }

        protected void P5sLbtnChangeSurvey_Click(object sender, EventArgs e)
        {


            Boolean isSelected = false;
            foreach (GridViewRow row in this.P5sMainGridView.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("SalesSelector");
                if (cb != null && cb.Checked)
                {
                    String salesCD = this.P5sMainGridView.DataKeys[row.RowIndex].Value.ToString();
                    String sql = @"UPDATE M_SALES SET USE_SURVEY = ~USE_SURVEY WHERE SALES_CD = @0";
                    L5sSql.Execute(sql, salesCD);
                    isSelected = true;
                }
            }
            if (!isSelected)
            {
                L5sMsg.Show("No Sales selected.");
                return;
            }
            else
            {
                L5sMsg.Show("Have changed sucessfully.");
                this.P5sMainGridViewInit();
                this.P5sAutoCompleteInit();
            }

        }

        protected void P5sLbtnChangeEditInfor_Click(object sender, EventArgs e)
        {
            Boolean isSelected = false;
            foreach (GridViewRow row in this.P5sMainGridView.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("SalesSelector");
                if (cb != null && cb.Checked)
                {
                    String salesCD = this.P5sMainGridView.DataKeys[row.RowIndex].Value.ToString();
                    String sql = @"UPDATE M_SALES SET EDIT_INFO = ~EDIT_INFO WHERE SALES_CD = @0";
                    L5sSql.Execute(sql, salesCD);
                    isSelected = true;
                }
            }
            if (!isSelected)
            {
                L5sMsg.Show("No Sales selected.");
                return;
            }
            else
            {
                L5sMsg.Show("Have changed sucessfully.");
                this.P5sMainGridViewInit();
                this.P5sAutoCompleteInit();
            }
        }
        protected void MainGridView(string viewStatee)
        {
            this.P5sDetailPanel.Visible = true;
            this.P5sLbtnInsert.Visible = false;
            this.P5sLbtnUpdate.Visible = true;
            String sql = @"SELECT s.SALES_CD,s.SALES_CODE,s.SALES_NAME,s.DISTRIBUTOR_CD, d.DISTRIBUTOR_CODE + '-' + d.DISTRIBUTOR_NAME AS DISTRIBUTOR_DESC,
                                 s.ACTIVE  ,s.SALES_TYPE_CD,s.USE_DMS,s.ALLOW_OUT_RANGE_ORDER,s.USE_5PTRACKING,s.USE_SURVEY,s.EDIT_INFO                        
                                FROM M_SALES s INNER JOIN M_DISTRIBUTOR d on s.DISTRIBUTOR_CD = d.DISTRIBUTOR_CD 

                                where s.SALES_CD = @0";
            DataTable dt = L5sSql.Query(sql, viewStatee);

            this.P5sTxtSALES_CODE.Text = dt.Rows[0]["SALES_CODE"].ToString();
            this.P5sTxtSALES_NAME.Text = dt.Rows[0]["SALES_NAME"].ToString();
            this.P5sTxtDISTRIBUTOR_CD.Text = dt.Rows[0]["DISTRIBUTOR_CD"].ToString();
            this.P5sTxtSALES_TYPE_CD.Text = dt.Rows[0]["SALES_TYPE_CD"].ToString();
            this.P5sACTIVE.Checked = Convert.ToBoolean(dt.Rows[0]["ACTIVE"].ToString());
            

        }
    }
}
