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

namespace MMV
{
    public partial class P5s2 : System.Web.UI.MasterPage
    {
        private bool _refreshState;
        private bool _isRefresh;
        public string[][,] L5sMenus;
        public string[] L5sMainMenus;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
        }
    }
}
