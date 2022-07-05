using L5sDmComm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MMV.Dashboard
{
    public class GetDataChart
    {
        public static string P5sConvertDataTabletoJson(DataTable dt)
        {
            string JSONString = JsonConvert.SerializeObject(dt);

            return JSONString;
        }
        internal static String P5sConvertDataTableToListStr(DataTable dtable, String columnName)
        {
            DataTable dt = dtable;
            if (dt != null && dt.Rows.Count <= 0)
                return "-1";

            String result = "";
            List<String> arr = new List<String>();
            if (dt.Rows[0][columnName].ToString() != "" && Array.IndexOf(arr.ToArray(), dt.Rows[0][columnName].ToString()) == -1)
            {
                result += "" + dt.Rows[0][columnName].ToString() + "',";
                arr.Add(dt.Rows[0][columnName].ToString());
            }
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][columnName].ToString() != "" && Array.IndexOf(arr.ToArray(), dt.Rows[i][columnName].ToString()) == -1)
                {
                    result += dt.Rows[i][columnName].ToString() + ",";
                    arr.Add(dt.Rows[i][columnName].ToString());
                }
            }
            return result.Remove(result.Length - 1);
        }
    } 
}