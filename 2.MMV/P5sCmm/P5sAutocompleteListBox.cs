using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;

namespace MMV
{
    public class P5sAutocompleteForListBox
    {

        private ListBox parentCtrl;
        private ListBox childCtrl;
        private DataTable dtSource;
        public  String P5sGetSelectedValues = "";

        public P5sAutocompleteForListBox(ListBox parentCtrl, DataTable dtSource)
        {
            this.parentCtrl = parentCtrl;
            this.P5sGetSelectedValues = this.getValues();
            this.dtSource = this.standDataTable(dtSource);

            this.parentCtrl.DataSource = this.dtSource;
            this.parentCtrl.DataValueField = this.dtSource.Columns[0].ColumnName;
            this.parentCtrl.DataTextField = this.dtSource.Columns[1].ColumnName; 
            this.parentCtrl.DataBind();
            this.parentCtrl.Attributes.Add("onChange", "eventListBoxChange('" + this.parentCtrl.ClientID + "','');");
            this.setAttributeParent();
            this.createHiddenField();

        }



        private DataTable standDataTable(DataTable dtSource)
        {
            DataView dtView = new DataView(dtSource);
            dtView.RowFilter = "ACTIVE = 1";
            DataTable result = new DataTable();
            result = dtView.ToTable();
            result.Columns[0].ColumnName = "CD";
            result.Columns[1].ColumnName = "NAME";
            result.Columns[2].ColumnName = "PARENT";
            result.Columns[3].ColumnName = "ACTIVE";
            return result;
        }

        private String GetJSONString(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
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

        private void createHiddenField()
        {
            HtmlInputHidden Hidden = new HtmlInputHidden();
            Hidden.ID = parentCtrl.ID + "_hdf";
            Hidden.Value = this.GetJSONString(this.dtSource);
            this.parentCtrl.Parent.Controls.Add(Hidden);
        }


        public P5sAutocompleteForListBox(ListBox parentCtrl, DataTable dtSource, ListBox childCtrl)
        {
            this.parentCtrl = parentCtrl;
            this.childCtrl = childCtrl;

            this.P5sGetSelectedValues = this.getValues();

            this.dtSource = this.standDataTable(dtSource);


            this.parentCtrl.DataSource = this.dtSource;
            this.parentCtrl.DataValueField = this.dtSource.Columns[0].ColumnName;
            this.parentCtrl.DataTextField = this.dtSource.Columns[1].ColumnName;
            this.parentCtrl.DataBind();

            this.setAttributeParent();

            this.parentCtrl.Attributes.Add("onChange", "eventListBoxChange('" + this.parentCtrl.ClientID + "','" + this.childCtrl.ClientID + "');");
            ScriptManager.RegisterClientScriptBlock(parentCtrl, this.GetType(), "ct100JavaScriptLoadDefaultValue" + System.Guid.NewGuid() + parentCtrl.ClientID, "$(document).ready(function(){ eventListBoxLoadFirstTime ('" + this.parentCtrl.ClientID + "','" + this.childCtrl.ClientID + "');});", true);
            this.createHiddenField();

        }

        public void P5sSetSelectedAllsValues(String parentSelectedCDs)
        {

            Page page = HttpContext.Current.Handler as Page;

            if (page != null && page.IsPostBack) //nếu không phải lần đầu load thì không set default value
                return;

            if (this.parentCtrl.Items.Count <= 0)
                return;

            if (parentSelectedCDs == "")
            {
                this.P5sSetSelectedAllsValues();
                return;
            }

            String[] parentCDs = parentSelectedCDs.Split(new char[] {','});
            if (this.parentCtrl.SelectionMode == ListSelectionMode.Single) 
            {
                if(parentCDs.Length > 1)
                {
                    L5sDmComm.L5sMsg.Show("Cannot have multiple items selected when the SelectionMode is Single.");
                    return;
                }

                //duyệt dựa trên datasource để lấy parentcd
                for (int i = 0; i < this.dtSource.Rows.Count; i++)
                {
                    if (this.dtSource.Rows[i]["PARENT"].ToString() == parentCDs[0])
                    {
                        this.parentCtrl.Items[i].Selected = true;
                        break;
                    }
                }
                
            }
            else
            { 
                //duyệt dựa trên datasource để lấy parentcd
                for (int i = 0; i < this.dtSource.Rows.Count; i++)
                {
                    if (Array.IndexOf(parentCDs, this.dtSource.Rows[i]["PARENT"].ToString()) >= 0)
                        this.parentCtrl.Items[i].Selected = true;
                }                
            }
        
           this.P5sGetSelectedValues = this.getValues(); //lấy lại giá trị selected
           
        }


        public void P5sSetSelectedAllsValues()
        {
            Page page = HttpContext.Current.Handler as Page;

            if (page != null && page.IsPostBack) //nếu không phải lần đầu load thì không set default value
                return;

            if (this.parentCtrl.Items.Count <= 0)
                return;

            if (this.parentCtrl.SelectionMode == ListSelectionMode.Single)
            {
                this.parentCtrl.Items[0].Selected = true; //select first item
            }
            else
            {
                for (int i = 0; i < this.parentCtrl.Items.Count; i++)
                {
                    this.parentCtrl.Items[i].Selected = true;
                }
            }

            this.P5sGetSelectedValues = this.getValues(); //lấy lại giá trị selected

        }


   

        public void P5sSetDefaultValues(String values)
        {

          

            if (this.parentCtrl.Items.Count <= 0)
                return;

            if (values == "")
                return;


            for (int i = 0; i < this.parentCtrl.Items.Count; i++)
            {
                this.parentCtrl.Items[i].Selected = false;
            }


            if (this.parentCtrl != null)
            {
                if (values != "")
                {
                    String[] arr = values.Split(new char[] { ',' });
                    for (int i = 0; i < arr.Length; i++)
                    {
                        ListItem item = this.parentCtrl.Items.FindByValue(arr[i]);
                        if (item != null)
                            item.Selected = true;
                    }
                }
                else
                    if (this.dtSource.Rows.Count == 1)
                    {
                        ListItem item = this.parentCtrl.Items.FindByValue(this.dtSource.Rows[0][0].ToString());
                        if (item != null)
                            item.Selected = true;
                    }
            }
        }

        private String getValues()
        {
            String result = "";
            if (this.parentCtrl != null)
            {
                for (int i = 0; i < this.parentCtrl.Items.Count; i++)
                {
                    if (this.parentCtrl.Items[i].Selected)
                        result += this.parentCtrl.Items[i].Value + ",";
                }
            }
            if (result == "")
                return result;

            return result.Remove(result.Length -1);
        }

        private void setAttributeParent()
        {
            for (int i = 0; i < this.parentCtrl.Items.Count; i++)
            {
               this.parentCtrl.Items[i].Attributes.Add("ParentId", this.dtSource.Rows[i][this.dtSource.Columns[2]].ToString());
            }
        }

       
         
    }
}
