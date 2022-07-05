using L5sDmComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MMV.Dashboard
{
    /// <summary>
    /// Summary description for ChartService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ChartService : System.Web.Services.WebService
    {
        [WebMethod]
        public string getRanking(string Condition, string groupCode, string month, string saleName)
        {
            string sql = "";
            string top10 = "";
            if (!string.IsNullOrEmpty(saleName))
            {
                top10 = "top 10";
            }
            
            if(Condition == "")
            {
                sql =String.Format(@"

                        select {0} t.*
                        from (
                        select [YEAR],[MONTH],RANKING,DSR_CODE,DSR_NAME,[1.ECC] as ECC ,[2.Visit] AS Visit,[3.Actual] as [Actual]

                        from (
                        select RANKING,DSR_CODE,DSR_NAME,HEADER,VALUE,[YEAR],[MONTH]
                        FROM [R_DSR_RANKING]
                        where [YEAR] = YEAR(GETDATE()) AND [MONTH] = MONTH(GETDATE())
                        ) T
                        pivot (
                        sum(VALUE)
                        FOR HEADER IN ([1.ECC],[2.Visit],[3.Actual])
                        ) AS B
                        ) t
                       -- order by RANKING
                        union all
                        select  t.*
                        from (
                        select [YEAR],[MONTH],RANKING,DSR_CODE,DSR_NAME,[1.ECC] as ECC ,[2.Visit] AS Visit,[3.Actual] as [Actual]

                        from (
                        select RANKING,DSR_CODE,DSR_NAME,HEADER,VALUE,[YEAR],[MONTH]
                        FROM [R_DSR_RANKING]
                        where [YEAR] = YEAR(GETDATE()) AND [MONTH] = MONTH(GETDATE())
                        and DSR_CODE='{1}'
                        ) T
                        pivot (
                        sum(VALUE)
                        FOR HEADER IN ([1.ECC],[2.Visit],[3.Actual])
                        ) AS B
                        ) t
                        order by RANKING

                        ", top10, saleName);
            }
            else if (month != "" && groupCode =="")
            {
                sql = String.Format(@"

                            select {0} t.*
                            from (
                            select [YEAR],[MONTH],RANKING,DSR_CODE,DSR_NAME,[1.ECC] as ECC ,[2.Visit] AS Visit,[3.Actual] as [Actual]

                            from (
                            select RANKING,DSR_CODE,DSR_NAME,HEADER,VALUE,[YEAR],[MONTH] 
                            FROM [R_DSR_RANKING]
                            where (1=1) {1}
                            ) T
                            pivot (
                            sum(VALUE)
                            FOR HEADER IN ([1.ECC],[2.Visit],[3.Actual])
                            ) AS B
                            ) t
                           -- order by RANKING
                            union all
                            select  t.*
                            from (
                            select [YEAR],[MONTH],RANKING,DSR_CODE,DSR_NAME,[1.ECC] as ECC ,[2.Visit] AS Visit,[3.Actual] as [Actual]

                            from (
                            select RANKING,DSR_CODE,DSR_NAME,HEADER,VALUE,[YEAR],[MONTH] 
                            FROM [R_DSR_RANKING]
                            where (1=1) {1} and DSR_CODE='{2}'
                            ) T
                            pivot (
                            sum(VALUE)
                            FOR HEADER IN ([1.ECC],[2.Visit],[3.Actual])
                            ) AS B
                            ) t
                            order by RANKING
                        ", top10, month, saleName);
            }
            else
            {
                sql = String.Format(@"

                            select {0} t.*
                            from (
                            select [YEAR],[MONTH],[GROUP_CODE],RANKING,DSR_CODE,DSR_NAME,[1.ECC] as ECC ,[2.Visit] AS Visit,[3.Actual] as [Actual]

                            from (
                            select RANKING,DSR_CODE,DSR_NAME,HEADER,VALUE,[GROUP_CODE],[YEAR],[MONTH] 
                            FROM [R_DSR_RANKING]
                            where (1=1) {1}
                            ) T
                            pivot (
                            sum(VALUE)
                            FOR HEADER IN ([1.ECC],[2.Visit],[3.Actual])
                            ) AS B
                            ) t
                          --  order by RANKING
                            UNION ALL
                            select  t.*
                            from (
                            select [YEAR],[MONTH],[GROUP_CODE],RANKING,DSR_CODE,DSR_NAME,[1.ECC] as ECC ,[2.Visit] AS Visit,[3.Actual] as [Actual]

                            from (
                            select RANKING,DSR_CODE,DSR_NAME,HEADER,VALUE,[GROUP_CODE],[YEAR],[MONTH] 
                            FROM [R_DSR_RANKING]
                            where (1=1) {1} and DSR_CODE='{2}'
                            ) T
                            pivot (
                            sum(VALUE)
                            FOR HEADER IN ([1.ECC],[2.Visit],[3.Actual])
                            ) AS B
                            ) t
                            order by RANKING

                        ", top10, Condition, saleName);
            }
             
            DataTable dtSql = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dtSql);
            return json;
        }
        [WebMethod(EnableSession = true)]
        public string OnClickChar(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            String sql = "";
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();

            if ((table == "D_YTD_SALES_DATA_PRI" || table == "D_YTD_SALES_DATA_SEC") && nameCountry == "VIET NAM")
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1} 
                                            from {3} _data
                                            left JOIN 
                                            (	SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on _data.AREA = are.AREA_CODE
                                            where (1=1) 
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table);
            }
            else
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1} 
                                            from {3} _data
                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                            where (1=1) 
                                            group by {0}
                                        ) t1 order by {2} 
                                ", Cols, colSum, orderBy, table);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }



        [WebMethod(EnableSession = true)]
        public string getGrowth(string filter, string colFilter, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();
            String sql = "";
            if ((table == "D_YTD_SALES_DATA_PRI" || table == "D_YTD_SALES_DATA_SEC") && nameCountry == "VIET NAM")
            {
                sql = String.Format(@"SELECT concat((select case when GROWTH is null then 0 else GROWTH end
                                        from
                                        (
                                        select round(((((SELECT sum({0})
				                                    FROM {1} D 
                                       left join (SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on are.AREA_CODE = d.AREA
				                                    where YEAR = year(DATEADD(MONTH,-1,getdate())) and (1=1)) *100)/(SELECT sum({0})
										                                    FROM {1} D 
                                        left join (SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on are.AREA_CODE = d.AREA
										                                    where YEAR = (year(DATEADD(MONTH,-1,getdate())) -1) and (1=1))) - 100),0) as GROWTH
	                                    ) t1),'%') as GROWTH
                                        ", colSum, table);
            }
            else
            {
                sql = String.Format(@"SELECT concat((select case when GROWTH is null then 0 else GROWTH end
                                        from
                                        (
                                        select round(((((SELECT sum({0})
				                                    FROM {1} d
                                                    left join M_DISTRIBUTOR dis on dis.DISTRIBUTOR_CODE = d.DIST
				                                    where 
				                                     YEAR = year(DATEADD(MONTH,-1,getdate())) and (1=1)) *100)/(SELECT sum({0})
										                                    FROM {1} d
                                                        left join M_DISTRIBUTOR dis on dis.DISTRIBUTOR_CODE = d.DIST
				                                        where 
										                                    YEAR = (year(DATEADD(MONTH,-1,getdate())) -1) and (1=1))) - 100),0) as GROWTH
	                                    ) t1),'%') as GROWTH
                                        ", colSum, table);
            }


            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getYTD(string filter, string colFilter, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            String sql = "";
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();
            if ((table == "D_YTD_SALES_DATA_PRI" || table == "D_YTD_SALES_DATA_SEC") && nameCountry == "VIET NAM")
            {
                sql = String.Format(@"SELECT replace(convert(varchar,cast(round(SUM({0}),0) as money), 1),'.00','') as YTD
                                          FROM {1} d
                                       left join (SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on are.AREA_CODE = d.AREA
                                          where (1=1) and YEAR = YEAR(getdate())
                                        ", colSum, table);
               
            }
            else
            {
                sql = String.Format(@"SELECT replace(convert(varchar,cast(round(SUM({0}),0) as money), 1),'.00','') as YTD
                                          FROM {1} d
                                        join M_DISTRIBUTOR dis on dis.DISTRIBUTOR_CODE = d.DIST
				                        where dis.ACTIVE = 1
                                          and (1=1) and YEAR = YEAR(getdate())
                                        ", colSum, table);

            }


            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            if ((table == "D_YTD_SALES_DATA_PRI" || table == "D_YTD_SALES_DATA_SEC") && nameCountry == "TH")
            {
                var a = Session["Where"].ToString();
                if (Session["Where"].ToString().IndexOf("YEAR in") > 0)
                {
                    var yearVal = filter;
                    var year = a.Substring(a.IndexOf("YEAR in (N'") + 11, 4);
                    //sql = sql.Replace("YEAR in (N'" + year + "')", "(9=9)");
                    sql = sql.Replace("and YEAR = YEAR(getdate())", "and (11=11)");
                }
            }
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getDataOrtherPercent(string filter, string colFilter, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            String sql = "";
            if (colSum == "TIME_GONE")
            {
                sql = String.Format(@"SELECT top 1
                                                concat(ROUND (({0})*100,2),'%') as ORTHERPERCENT
                                                FROM {1} _data
                                                JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                                where (1=1)
                                        ", colSum, table);
            }
            else
            {
                sql = String.Format(@"SELECT 
                                              concat(ROUND (AVG({0})*100,0),'%') as ORTHERPERCENT
                                            FROM {1} _data
                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                            where (1=1)
                                        ", colSum, table);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getDataPercent(string filter, string colFilter, string colSum, string colSum2, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            //Phu
            //String sql = String.Format(@"SELECT concat(round(((SELECT SUM({0})*100 FROM {2} where (1=1) ) /
            //                         (SELECT SUM({1}) FROM {2} where (1=1))),0),'%') as PERCENTTAG
            //                            ", colSum, colSum2, table);
            //Hung
            String sql = String.Format(@"declare @sumActual float = ( SELECT SUM({0})*100 FROM {2} _data JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE where (1=1))
													declare @sumTARGET float = ( SELECT SUM({1}) FROM {2} _data JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE where (1=1))
													SELECT CASE WHEN @sumActual = 0 OR @sumTARGET = 0 THEN  CONCAT(0,' %') ELSE
													concat(round( ( @sumActual / @sumTARGET),0),'%') end as PERCENTTAG
                                        ", colSum, colSum2, table);

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getDataMTD(string filter, string colFilter, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();
            string language = "";
            if (nameCountry == "TH")
            {
                language = "฿";
            }
            else
            {
                language = "Mđ";
            }

            String sql = String.Format(@" SELECT 
	                                            concat(replace(convert(varchar,cast(round(SUM({0}),0) as money), 1),'.00',''),N' {2}') as MTD
                                            FROM {1} _data
                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                              where (1=1) 
                                        ", colSum, table, language);

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string OnClickCharV2(string filter, string colFilter, string Cols, string colSum, string orderBy, string table, string ACTUAL, string TARGET)
        {
            L5sInitial.LoadForLoginPage();
            String sql = "";
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            if (ACTUAL == "ACTUAL" && TARGET == "TARGET")
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                           select {0}
                                            ,round(sum({1}),0) as {1}
											,case when sum({4})= 0 or sum({5}) = 0 then 0 else cast( sum({4}) /sum({5})*100 as numeric(18,0)) end as [PERCENT]
                                            from {3} _data
                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                            where (1=1)
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table, ACTUAL, TARGET);
            }
            else
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1}
                                            from {3}
                                            where (1=1)
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getGrowthV2(string filter, string colFilter, string Cols, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            //String sql = String.Format(@"SELECT concat((select case when GROWTH is null then 0 else GROWTH end
            //                            from
            //                            (
            //                            select round(((((SELECT sum({0})
            //                            FROM {1}
            //                            where YEAR = Year(getdate()) and (1=1)) *100)/(SELECT sum({0})
            //                                  FROM {1}
            //                                  where YEAR = (Year(getdate()) -1) and (1=1))) - 100),0) as GROWTH
            //                         ) t1),'%') as GROWTH
            //                            ", colSum, table);
            String sql = String.Format(@"SELECT Y.{0},Y.[GROWTH SALES] AS [GROWTH SALES: ],
			case when Y.[GROWTH SALES] = 0 or LastY.[GROWTH SALES] =0 then 0 else 
			CAST( ( (Y.[GROWTH SALES]*1.0*100)/LastY.[GROWTH SALES]) -100 as numeric(18,0)) end as [PERCENT]
	                                                FROM 
	                                                (SELECT DIST,{0},SUM({1}) AS [GROWTH SALES]
	                                                FROM {2} _data
                                                    JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
	                                                where (1=1)  and YEAR = year(DATEADD(MONTH,-1,getdate()))
	                                                	
	                                                GROUP BY DIST,{0}
	                                                ) Y
	                                                JOIN
	                                                (SELECT DIST,{0},SUM({1}) AS [GROWTH SALES]
							                                                FROM {2} _data
                                                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
							                                                where (1=1) and YEAR = (year(DATEADD(MONTH,-1,getdate())) -1)
							                                                GROUP BY DIST,{0}
							                                                 ) LastY

	                                                ON Y.DIST = LastY.DIST ", Cols, colSum, table);
            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {

                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();

                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);

                    }
                    else
                    {


                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);

                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }

                }
            }
            var a = Session["Where"].ToString();
            double Num;
            bool isNum = double.TryParse(filter, out Num);
            if (Session["Where"].ToString().IndexOf("YEAR in") > 0)
            {
                var index = a.IndexOf("YEAR in (N'");
                var year = a.Substring(a.IndexOf("YEAR in (N'") + 11, 4);
                var yearValBefore = Int32.Parse(year) - 1;
                var month = DateTime.Now.Month;
                var year_current = DateTime.Now.Year;
                sql = sql.Replace("YEAR in (N'" + year + "')", "(9=9)");
                sql = sql.Replace("and YEAR = year(DATEADD(MONTH,-1,getdate()))", "and (11=11)");
                sql = sql.Replace("and YEAR = (year(DATEADD(MONTH,-1,getdate())) -1)", "and (22=22)");
                if (month == 1 && year_current == (Int32.Parse(year)))
                {
                    sql = sql.Replace("(11=11)", "YEAR in (N'" + (Int32.Parse(year) - 1) + "')");
                    sql = sql.Replace("(22=22)", "YEAR in (N'" + (yearValBefore - 1) + "')");
                }
                else
                {
                    sql = sql.Replace("(11=11)", "YEAR in (N'" + Int32.Parse(year) + "')");
                    sql = sql.Replace("(22=22)", "YEAR in (N'" + yearValBefore + "')");
                }


            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }
        
        [WebMethod(EnableSession = true)]
        public string getMonth(string table)
        {
            String sql = String.Format(@"select distinct MONTH,CASE
						                    WHEN MONTH LIKE '%Jan%' or MONTH = '1' or  MONTH = '01' THEN '01'	 
						                    WHEN MONTH LIKE '%Feb%' or MONTH = '2' or  MONTH = '02' THEN '02'	
						                    WHEN MONTH LIKE '%Mar%' or MONTH = '3' or  MONTH = '03' THEN '03'	
						                    WHEN MONTH LIKE '%Apr%' or MONTH = '4' or  MONTH = '04' THEN '04'
						                    WHEN MONTH LIKE '%May%' or MONTH = '5' or  MONTH = '05' THEN '05'	
						                    WHEN MONTH LIKE '%Jun%' or MONTH = '6' or  MONTH = '06' THEN '06'	
						                    WHEN MONTH LIKE '%Jul%' or MONTH = '7' or  MONTH = '07' THEN '07'	
						                    WHEN MONTH LIKE '%Aug%' or MONTH = '8' or  MONTH = '08' THEN '08'	
						                    WHEN MONTH LIKE '%Sep%' or MONTH = '9' or  MONTH = '09' THEN '09'	
						                    WHEN MONTH LIKE '%Oct%' or MONTH = '10' or  MONTH = '10' THEN '10'
						                    WHEN MONTH LIKE '%Nov%' or MONTH = '11' or  MONTH = '11' THEN '11'
						                    WHEN MONTH LIKE '%Dec%' or MONTH = '12' or  MONTH = '12' THEN '12'
					                    END as Va_Month 
                                        FROM {0}
                                        ORDER BY Va_Month ASC", table
                                        );
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        #region Hàm dùng cho chart quarter trong SELLOUT SUMMARY
        [WebMethod(EnableSession = true)]
        public string OnClickCharV3(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            String sql = "";
            // load map first
            if (Cols == "MONTH")
            {
                sql = String.Format(@"  SELECT {0},ISNULL([{4}],0) AS [Y {4}],ISNULL([{5}],0) AS [Y {5}] FROM(
                                    SELECT {0},MONTH_NUMBER,YEAR,SUM({1}) AS {1}
                                    FROM  {3} _target
                                    JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                    WHERE (1=1)
                                    GROUP BY {0},MONTH_NUMBER,YEAR
                                    ) T1
                                    PIVOT 
                                    (	
	                                    SUM({1}) FOR [YEAR] IN ([{4}],[{5}])
                                    )  AS T2
                                    ORDER BY {2}
                                ", Cols, colSum, orderBy, table, DateTime.Now.Year - 1, DateTime.Now.Year);
            }
            else if (Cols == "YEAR")
            {
                sql = String.Format(@" SELECT 'YEAR' AS [YEAR] ,ISNULL([{3}],0) AS [Y {3}],ISNULL([{4}],0) AS [Y {4}]
                                        FROM (
                                            SELECT {0}, SUM({1}) AS ACTUAL
                                            FROM {2} _target
                                            JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                            WHERE (1=1)
                                            GROUP BY YEAR) T1 PIVOT (
	                                        SUM(ACTUAL) FOR [YEAR] in ([{3}],[{4}])
                                        )   AS T2
                                ", Cols, colSum, table, DateTime.Now.Year - 1, DateTime.Now.Year);
            }
            else
            {
                sql = String.Format(@"  SELECT {0},ISNULL([{4}],0) AS [Y {4}],ISNULL([{5}],0) AS [Y {5}] FROM(
                                    SELECT {0},YEAR,SUM({1}) AS {1}
                                    FROM  {3} _target
                                    JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                    WHERE (1=1)
                                    GROUP BY {0},YEAR
                                    ) T1
                                    PIVOT 
                                    (	
	                                    SUM({1}) FOR [YEAR] IN ([{4}],[{5}])
                                    )  AS T2
                                    ORDER BY {2}
                                ", Cols, colSum, orderBy, table, DateTime.Now.Year - 1, DateTime.Now.Year);
            }


            if (!string.IsNullOrEmpty(filter) )
            {
                if (Session["Where1"].ToString() == "(1=1)")
                {
                    Session["Where1"] = Session["Where1"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where1"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where1"] = Session["Where1"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where1"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where1"] = Session["Where1"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where1"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getDataQuarter(string filter, string colFilter, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            string sqlPercent = string.Format(@"  DECLARE @PAST float
                                    DECLARE @PRESENT float
                                    DECLARE @TMP varchar(50)
                                    SET @TMP='{0}'

                                    SET @PAST = ISNULL((SELECT SUM({2}) 
                                    FROM {1} _target
                                    JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                    WHERE {3}=@TMP AND YEAR=(YEAR(GETDATE())-1)
                                    GROUP BY YEAR,QUARTER),0)

                                    SET @PRESENT = 	  ISNULL((SELECT SUM({2}) 
                                    FROM {1} _target
                                    JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                    WHERE {3}=@TMP AND YEAR=(YEAR(GETDATE()))
                                    GROUP BY YEAR,QUARTER),0)

                                    IF(@PAST=0 AND @PRESENT=0)
                                    BEGIN
	                                    SELECT 0 AS [PERCENT]
                                    END
                                    ELSE IF(@PAST=0 AND @PRESENT <> 0)
                                    BEGIN
	                                    SELECT 100 AS [PERCENT]
                                    END
                                    ELSE
                                    BEGIN
	                                    SELECT ROUND(((@PRESENT-@PAST)/@PAST)*100,2)  AS [PERCENT]
                                    END
                                    ", filter, table, colSum, colFilter);
            DataTable dt = L5sSql.Query(sqlPercent);
            return dt.Rows[0]["PERCENT"].ToString() + "%";
        }
        #endregion
        #region Hàm dùng cho chart act and target
        [WebMethod(EnableSession = true)]
        public string OnClickCharV4(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            String sql = "";
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }

            if (Cols == "MTD")
            {
                sql = String.Format(@"  DECLARE @GETDATE datetime
                                        SET @GETDATE=GETDATE()

                                        DECLARE @ACTUAL float
                                        SET @ACTUAL=
                                        (SELECT ROUND(SUM(ACTUAL),0) AS [TotalCoverage]
                                        FROM D_MTD_ACT_TARGET _target
                                        JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                        WHERE [YEAR]=YEAR(@GETDATE) AND MONTH_NUMBER=MONTH(@GETDATE) AND (2=2))

                                        DECLARE @TARGET float
                                        SET @TARGET=
                                        (SELECT ROUND(SUM(TARGET),0) AS [TotalCoverage]
                                        FROM D_MTD_ACT_TARGET _target
                                        JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                        WHERE [YEAR]=YEAR(@GETDATE) AND MONTH_NUMBER=MONTH(@GETDATE) AND (2=2) )

                                        SELECT 'MTD Actual' AS [Coverage], @ACTUAL as [TotalCoverage]
                                        UNION ALL
                                        SELECT '' AS [Coverage], CASE WHEN (@TARGET-@ACTUAL) <0 THEN 0 ELSE (@TARGET-@ACTUAL) END as [TotalCoverage]
                                ");
            }
            else
            {
                sql = String.Format(@"  DECLARE @GETDATE datetime
                                    SET @GETDATE=GETDATE()

                                    SELECT {0},ROUND(SUM({1}),0) AS [MTD Actual],ROUND(SUM(TARGET),0) AS [Sales Target]
                                    FROM {3} _target
                                    JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                    WHERE [YEAR]=YEAR(@GETDATE) AND MONTH_NUMBER=MONTH(@GETDATE) AND (2=2)
                                    GROUP BY {0},{2}
                                    ORDER BY {2} ASC
                                ", Cols, colSum, orderBy, table);
            }


            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where2"].ToString() == "(2=2)")
                {
                    Session["Where2"] = Session["Where2"].ToString().Replace("(2=2)", String.Format("(2=2) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                    Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where2"] = Session["Where2"].ToString().Replace("(2=2)", String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                        Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(2=2)", "");
                            Session["Where2"] = Session["Where2"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getTarget(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            String   sql = String.Format(@"  DECLARE @GETDATE datetime
                                        SET @GETDATE=GETDATE()

                                        DECLARE @TARGET float
                                        SET @TARGET=
                                        (SELECT ROUND(SUM(TARGET),0) AS [TotalCoverage]
                                        FROM D_MTD_ACT_TARGET _target
                                        JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                        WHERE [YEAR]=YEAR(@GETDATE) AND MONTH_NUMBER=MONTH(@GETDATE) AND (2=2) )

                                        SELECT 'Target' AS [Coverage], @TARGET as [TotalCoverage]
                                ");
         


            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where2"].ToString() == "(2=2)")
                {
                    Session["Where2"] = Session["Where2"].ToString().Replace("(2=2)", String.Format("(2=2) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                    Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where2"] = Session["Where2"].ToString().Replace("(2=2)", String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                        Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(2=2)", "");
                            Session["Where2"] = Session["Where2"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }
        #endregion
        //MTD_DISTRIBUTION
        //MTD_DISTRIBUTION
        #region Hung
        [WebMethod(EnableSession = true)]
        public string OnClickCharCPTH(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            colFilter = colFilter + "MSS";
            String sql = "";
            if (colSum == "Point_RL")
            {
                sql = String.Format(@"	select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),1) as [MSS Total]  
                                            from {3} d
											join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                            where RE_CODE='503'and (3=3) 
                                           AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE()) 
                                            group by {0}
                                        ) t1 order by [MSS Total]     desc
                                ", Cols, colSum, orderBy, table);
            } 
            else
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),1) as [MSS Total]   
                                            from {3} d
                                            join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                            where RE_CODE='503'and (3=3) 
                                             AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE())
                                            group by {0}
                                        ) t1 order by [MSS Total]  
                                ", Cols, colSum, orderBy, table);
            }
            

            #region hung
            //String sql = String.Format(@"
            //                        declare @sum FLOAT = (select sum({1})
            //                  from
            //                  (
            //                  select {0}
            //                    ,round(sum({1}),0) as {1}
            //                  from {3}
            //                  where (1=1)
            //                  group by {0}
            //                  ) t1 )

            //                        select *
            //                         from (
            //                  select {0},{1},   cast(  ({1}/@sum)*100  as numeric(18,2)   )  AS [PERCENT] 
            //                   from
            //                    (
            //                    select {0}
            //                      ,round(sum({1}),0) as {1}
            //                    from {3}
            //                    where (1=1)
            //                    group by {0}
            //                    ) t1 
            //                            )t2 order by {2} ", Cols, colSum, orderBy, table);
            #endregion
            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where3"].ToString() == "(3=3)")
                {
                    Session["Where3"] = Session["Where3"].ToString().Replace("(3=3)", String.Format("(3=3) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                    Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where3"] = Session["Where3"].ToString().Replace("(3=3)", String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                        Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(3=3)", "");
                            Session["Where3"] = Session["Where3"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                    }
                }
            }
            sql = sql.Replace("DISTRIBUTOR_NAMEMSS", "DISTRIBUTOR_NAME");
            sql = sql.Replace("REGIONMSS", "REGION");
            sql = sql.Replace("SALES_NAMEMSS", "SALES_NAME");
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string OnClickCharRECPTH(string filter, string colFilter, string Cols, string colSum, string orderBy, string table, string re_code)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            colFilter = colFilter + "MSS";
            String sql = "";
            if (Cols == "SALES_NAME")
            {
                sql = String.Format(@"	select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum(Point_RL),1) as [MSS Total]  ,SALES_CODE
                                            from {3} d
											join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                            where RE_CODE='{4}'and (3=3) 
                                            AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE())
                                            group by {0},SALES_CODE
                                        ) t1 order by SALES_CODE   ASC
                                ", Cols, colSum, orderBy, table, re_code);
            }
            else if (colSum == "Point_RL")
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),1) as [MSS Total]    
                                            from {3} d
                                            join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                            where RE_CODE = '{4}' AND (3=3)
                                                AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE())
                                            group by RE_CODE,{0}
                                        ) t1 order by [MSS Total]   desc
                                ", Cols, colSum, orderBy, table, re_code);
            }
            else
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),1) as {1} 
                                            from {3} d
                                            join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                            where RE_CODE = '{4}' AND (3=3)
                                            AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE())
                                            group by RE_CODE,{0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table, re_code);
            }
          

            #region hung
            //String sql = String.Format(@"
            //                        declare @sum FLOAT = (select sum({1})
            //                  from
            //                  (
            //                  select {0}
            //                    ,round(sum({1}),0) as {1}
            //                  from {3}
            //                  where (1=1)
            //                  group by {0}
            //                  ) t1 )

            //                        select *
            //                         from (
            //                  select {0},{1},   cast(  ({1}/@sum)*100  as numeric(18,2)   )  AS [PERCENT] 
            //                   from
            //                    (
            //                    select {0}
            //                      ,round(sum({1}),0) as {1}
            //                    from {3}
            //                    where (1=1)
            //                    group by {0}
            //                    ) t1 
            //                            )t2 order by {2} ", Cols, colSum, orderBy, table);
            #endregion
            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where3"].ToString() == "(3=3)")
                {
                    Session["Where3"] = Session["Where3"].ToString().Replace("(3=3)", String.Format("(3=3) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                    Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where3"] = Session["Where3"].ToString().Replace("(3=3)", String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                        Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(3=3)", "");
                            Session["Where3"] = Session["Where3"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                    }
                }
            }
            sql = sql.Replace("DISTRIBUTOR_NAMEMSS", "DISTRIBUTOR_NAME");
            sql = sql.Replace("REGIONMSS", "REGION");
            sql = sql.Replace("SALES_NAMEMSS", "SALES_NAME");
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }
        [WebMethod(EnableSession = true)]
        public string getTotalPointRL(string filter, string colFilter, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            colFilter = colFilter + "MSS";
            String sql = String.Format(@"select *
                                        from
                                        (
                                            select round(sum({0}),1) as {0} 
                                            from {2} d
                                            join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                            where RE_CODE='503' AND (3=3)
                                             AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE())
                                            
                                        ) t1 order by {1}
                                ", colSum, orderBy, table);

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where3"].ToString() == "(3=3)")
                {
                    Session["Where3"] = Session["Where3"].ToString().Replace("(3=3)", String.Format("(3=3) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                    Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where3"] = Session["Where3"].ToString().Replace("(3=3)", String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                        Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(3=3)", "");
                            Session["Where3"] = Session["Where3"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                    }
                }
            }
            sql = sql.Replace("DISTRIBUTOR_NAMEMSS", "DISTRIBUTOR_NAME");
            sql = sql.Replace("REGIONMSS", "REGION");
            sql = sql.Replace("SALES_NAMEMSS", "SALES_NAME");
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getTimeGone(string filter, string colFilter, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            colFilter = colFilter + "MSS";
            String sql = String.Format(@"select *
                                        from
                                        (
                                            select top 1 {0} as {0} 
                                            from {2} d
                                            join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                            AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE())
                                            where (3=3)
                                            
                                        ) t1 order by {1}
                                ", colSum, orderBy, table);

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where3"].ToString() == "(3=3)")
                {
                    Session["Where3"] = Session["Where3"].ToString().Replace("(3=3)", String.Format("(3=3) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                    Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where3"] = Session["Where3"].ToString().Replace("(3=3)", String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                        Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(3=3)", "");
                            Session["Where3"] = Session["Where3"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(3=3) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(3=3)", Session["Where3"].ToString());
                    }
                }
            }
            sql = sql.Replace("DISTRIBUTOR_NAMEMSS", "DISTRIBUTOR_NAME");
            sql = sql.Replace("REGIONMSS", "REGION");
            sql = sql.Replace("SALES_NAMEMSS", "SALES_NAME");
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        // CBS
        [WebMethod(EnableSession = true)]
        public string getCoverageBuyingStoreVisit(string filter, string Cols, string colFilter, string colSum, string colSum2, string colSum3, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            colFilter = colFilter + "CBS";
            String sql = "";
            if (Cols == "SALES_NAME")
            {
                sql = String.Format(@"select  t1.*
                                        from
                                        (
                                           SELECT {0},SUM({1}) as {1},SUM({2}) as {2},SUM({3}) as [VISIT],SALES_CODE
                                            FROM {5} t1
                                            join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where active = 1) vdis on vdis.DISTRIBUTOR_CODE = t1.DISTRIBUTOR_CODE
                                            where (4=4)
                                            AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE())
                                            GROUP BY {0},SALES_CODE
                                        ) t1 
										
										order by SALES_CODE 
                                ", Cols, colSum, colSum2, colSum3, orderBy, table);
            }
            else
            {
                sql = String.Format(@"select  t1.*
                                        from
                                        (
                                           SELECT {0},SUM({1}) as {1},SUM({2}) as {2},SUM({3}) as [VISIT]
                                            FROM {5} t1
                                            join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where active = 1) vdis on vdis.DISTRIBUTOR_CODE = t1.DISTRIBUTOR_CODE
                                            where (4=4)
                                            AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE())
                                            GROUP BY {0}
                                        ) t1 
										
										order by {4}
                                ", Cols, colSum, colSum2, colSum3, orderBy, table);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where4"].ToString() == "(4=4)")
                {
                    Session["Where4"] = Session["Where4"].ToString().Replace("(4=4)", String.Format("(4=4) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                    Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where4"] = Session["Where4"].ToString().Replace("(4=4)", String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                        Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(4=4)", "");
                            Session["Where4"] = Session["Where4"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                    }
                }
            }
            sql = sql.Replace("DISTRIBUTOR_NAMECBS", "DISTRIBUTOR_NAME");
            sql = sql.Replace("REGIONCBS", "REGION");
            sql = sql.Replace("SALES_NAMECBS", "SALES_NAME");
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }
        [WebMethod(EnableSession = true)]
        public string getECC_StoreVisited_CoverageBuyingStoreVisit(string filter, string colFilter, string colSum, string colSum2, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            String sql = String.Format(@"	declare @StoreVisit numeric(18,1) = (select sum({0}) 
                                                from {3} d
                                                join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where active = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                                where (4=4) 
                                                 AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE()))
										declare @RL numeric(18,1) = (select sum({1}) 
                                            from {3} d
                                             join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where active = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                            where (4=4) 
                                                AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE()))
                                        select *
                                         from (
										select case when @StoreVisit = 0 or @RL = 0 then 0 else cast( (@StoreVisit/@RL * 100) as numeric(18,1)) end as Store_visited
                                        ) t
                                ", colSum, colSum2, orderBy, table);

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where4"].ToString() == "(4=4)")
                {
                    Session["Where4"] = Session["Where4"].ToString().Replace("(4=4)", String.Format("(4=4) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                    Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where4"] = Session["Where4"].ToString().Replace("(4=4)", String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                        Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(4=4)", "");
                            Session["Where4"] = Session["Where4"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                    }
                }
            }
            sql = sql.Replace("DISTRIBUTOR_NAMECBS", "DISTRIBUTOR_NAME");
            sql = sql.Replace("REGIONCBS", "REGION");
            sql = sql.Replace("SALES_NAMECBS", "SALES_NAME");
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getDonutChart(string filter, string colFilter, string colSum, string colSum2, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            String sql = String.Format(@"declare @StoreVisit_Buying numeric(18,1) = (select sum({0}) 
                                        from {3} d
                                        join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                        where (4=4) AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE()) )
                                        declare @RL numeric(18,1) = (select sum({1}) 
                                        from {3} d
                                        join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                                        where (4=4) AND  [MONTH] = MONTH(GETDATE()) AND [YEAR] = YEAR(GETDATE()) )
                                        select '{0}' as 'Coverage',@StoreVisit_Buying  as 'TotalCoverage' ,@RL as'RL'
                                        union all
                                        select 'Coverage' ,@RL - @StoreVisit_Buying,@RL
                                ", colSum, colSum2, orderBy, table);

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where4"].ToString() == "(4=4)")
                {
                    Session["Where4"] = Session["Where4"].ToString().Replace("(4=4)", String.Format("(4=4) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                    Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where4"] = Session["Where4"].ToString().Replace("(4=4)", String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                        Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(4=4)", "");
                            Session["Where4"] = Session["Where4"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(4=4) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(4=4)", Session["Where4"].ToString());
                    }
                }
            }
            sql = sql.Replace("DISTRIBUTOR_NAMECBS", "DISTRIBUTOR_NAME");
            sql = sql.Replace("REGIONCBS", "REGION");
            sql = sql.Replace("SALES_NAMECBS", "SALES_NAME");

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }
        //PAR in percentage
        [WebMethod(EnableSession = true)]
        public string getPARinPercentage(string filter, string colFilter, string Cols, string table, string table2)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }

          

            String sql = String.Format(@"
                                	select *
									from (
									select act.{0},
				                    case when sum(act.ACTUAL) = 0 or sum(act.TARGET) = 0 then 0 else 
				                    cast( sum(act.ACTUAL)/sum(act.TARGET)* 100 as numeric(18,1) )end as Sec
				                    FROM ( select REGION, 
				                    (select DISTRIBUTOR_CODE from [M_DISTRIBUTOR] where ACTIVE = 1 and DISTRIBUTOR_NAME=DISTRIBUTOR) as DISTRIBUTOR_CODE,
				                    DISTRIBUTOR AS DISTRIBUTOR_NAME,
				                    (SELECT SALES_CODE FROM M_SALES WHERE SALES_NAME=act.DSR) as SALE_CODE,DSR,
				                    [TARGET],[ACTUAL],[YEAR],MONTH_NUMBER
				                    from {2} act --D_MTD_ACT_TARGET
                                    join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = act.DISTRIBUTOR_CODE
				                    WHERE ACT.YEAR = year(GETDATE()) and act.MONTH_NUMBER = month(GETDATE())
				
				                    ) act
				                    where (1=1)
				                    GROUP BY  act.{0}
									)t
									order by t.Sec desc          
                            ", Cols, table, table2);

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }
        [WebMethod(EnableSession = true)]
        public string getDSRPARinPercentage(string filter, string colFilter, string Cols, string table, string table2)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }

            String sql = String.Format(@"   select *
                                          from (
                                          SELECT DSR,
                                          case when ACTUAL = 0 or TARGET = 0 then 0 else 
                                           CAST( (ACTUAL/TARGET)* 100 AS numeric(18,1)) end as Sec
                                          FROM( 
											  SELECT REGION,DISTRIBUTOR as DISTRIBUTOR_NAME,DSR,TARGET,ACTUAL,YEAR,MONTH_NUMBER
											  FROM [D_MTD_ACT_TARGET] ACT 
                                              join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = act.DISTRIBUTOR_CODE) ACT
                                          WHERE ACT.YEAR = year(GETDATE()) and act.MONTH_NUMBER = month(GETDATE())
                                          AND (1=1)
                                          ) t
                                          order by Sec desc
                                ", Cols, table, table2);

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }
        [WebMethod(EnableSession = true)]
        public string getParPARinPercentage(string filter, string colFilter, string colSum, string colSum2, string month, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            string sql = "";
            if (colFilter == "Time_gone")
            {
                sql = @"SELECT *
                            FROM(
                            SELECT TOP 1 cast( TIME_GONE*100 as numeric(18,2) ) as TIME_GONE
                      FROM [D_MTD_SECONDATY_SALES_UPDATE] d
                      join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DIST
                      ORDER BY CREATED_DATE DESC) T";
            }
            else
            {
                
                sql = String.Format(@"  
                        select *
                        from (
                        select case when sum(TARGET) = 0 or sum(actual) = 0 then 0 else
                        cast( (sum(ACTUAL)/sum(TARGET) * 100) as numeric(18,2)) END AS Par
                        FROM 
                        (
                        SELECT {0} AS TARGET,{1} AS ACTUAL

                        FROM {3} d
                        join (select DISTRIBUTOR_CODE from M_DISTRIBUTOR where ACTIVE = 1) vdis on vdis.DISTRIBUTOR_CODE  = d.DISTRIBUTOR_CODE
                          WHERE {2} = month(GETDATE()) and [YEAR] = YEAR(GETDATE()) ) T
                         )t
                                ", colSum, colSum2, month, table);
            }



            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }
        #endregion

        //**********YTD_SALES_DATA_PRIMARY**********
        // 15-09-2020
        [WebMethod(EnableSession = true)]
        public string OnClickChar_PRI_TH(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            String sql = "";
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();

            if ((table == "D_YTD_SALES_DATA_PRI_NEW" || table == "D_YTD_SALES_DATA_SEC") && nameCountry == "TH")
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1} 
                                            from {3} _data
                                            left JOIN 
                                            (	SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on _data.AREA = are.AREA_CODE
                                            where (1=1) 
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table);
            }
            else
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1} 
                                            from {3} _data
                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                            where (1=1) 
                                            group by {0}
                                        ) t1 order by {2} 
                                ", Cols, colSum, orderBy, table);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getGrowth_PRI_TH(string filter, string colFilter, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();
            String sql = "";
            if ((table == "D_YTD_SALES_DATA_PRI_NEW" || table == "D_YTD_SALES_DATA_SEC") && nameCountry == "TH")
            {
                sql = String.Format(@"SELECT concat((select case when GROWTH is null then 0 else GROWTH end
                                        from
                                        (
                                        select round(((((SELECT sum({0})
				                                    FROM {1} D 
                                       left join (SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on are.AREA_CODE = d.AREA
				                                    where YEAR = year(DATEADD(MONTH,-1,getdate())) and (1=1)) *100)/(SELECT sum({0})
										                                    FROM {1} D 
                                        left join (SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on are.AREA_CODE = d.AREA
										                                    where YEAR = (year(DATEADD(MONTH,-1,getdate())) -1) and (1=1))) - 100),0) as GROWTH
	                                    ) t1),'%') as GROWTH
                                        ", colSum, table);
            }
            else
            {
                sql = String.Format(@"SELECT concat((select case when GROWTH is null then 0 else GROWTH end
                                        from
                                        (
                                        select round(((((SELECT sum({0})
				                                    FROM {1} d
                                                    left join M_DISTRIBUTOR dis on dis.DISTRIBUTOR_CODE = d.DIST
				                                    where 
				                                     YEAR = year(DATEADD(MONTH,-1,getdate())) and (1=1)) *100)/(SELECT sum({0})
										                                    FROM {1} d
                                                        left join M_DISTRIBUTOR dis on dis.DISTRIBUTOR_CODE = d.DIST
				                                        where 
										                                    YEAR = (year(DATEADD(MONTH,-1,getdate())) -1) and (1=1))) - 100),0) as GROWTH
	                                    ) t1),'%') as GROWTH
                                        ", colSum, table);
            }


            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getYTD_PRI_TH(string filter, string colFilter, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            String sql = "";
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();
            if ((table == "D_YTD_SALES_DATA_PRI_NEW" || table == "D_YTD_SALES_DATA_SEC") && nameCountry == "TH")
            {
                sql = String.Format(@"SELECT replace(convert(varchar,cast(round(SUM({0}),0) as money), 1),'.00','') as YTD
                                          FROM {1} d
                                       left join (SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on are.AREA_CODE = d.AREA
                                          where (1=1) and YEAR = YEAR(getdate())
                                        ", colSum, table);
            }
            else
            {
                sql = String.Format(@"SELECT replace(convert(varchar,cast(round(SUM({0}),0) as money), 1),'.00','') as YTD
                                          FROM {1} d
                                        join M_DISTRIBUTOR dis on dis.DISTRIBUTOR_CODE = d.DIST
				                        where dis.ACTIVE = 1
                                          and (1=1) and YEAR = YEAR(getdate())
                                        ", colSum, table);

            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            if ((table == "D_YTD_SALES_DATA_PRI_NEW" || table == "D_YTD_SALES_DATA_SEC") && nameCountry == "TH")
            {
                var a = Session["Where"].ToString();
                if (Session["Where"].ToString().IndexOf("YEAR in") > 0)
                {
                    var yearVal = filter;
                    var year = a.Substring(a.IndexOf("YEAR in (N'") + 11, 4);
                    //sql = sql.Replace("YEAR in (N'" + year + "')", "(9=9)");
                    sql = sql.Replace("and YEAR = YEAR(getdate())", "and (11=11)");
                }
            }
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string getGrowthV2_PRI_TH(string filter, string colFilter, string Cols, string colSum, string table)
        {
            L5sInitial.LoadForLoginPage();
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            // load map first
            //String sql = String.Format(@"SELECT concat((select case when GROWTH is null then 0 else GROWTH end
            //                            from
            //                            (
            //                            select round(((((SELECT sum({0})
            //                            FROM {1}
            //                            where YEAR = Year(getdate()) and (1=1)) *100)/(SELECT sum({0})
            //                                  FROM {1}
            //                                  where YEAR = (Year(getdate()) -1) and (1=1))) - 100),0) as GROWTH
            //                         ) t1),'%') as GROWTH
            //                            ", colSum, table);
            String sql = String.Format(@"SELECT Y.{0},Y.[GROWTH SALES] AS [GROWTH SALES: ],
			case when Y.[GROWTH SALES] = 0 or LastY.[GROWTH SALES] =0 then 0 else 
			CAST( ( (Y.[GROWTH SALES]*1.0*100)/LastY.[GROWTH SALES]) -100 as numeric(18,0)) end as [PERCENT]
	                                                FROM 
	                                                (SELECT DIST,{0},SUM({1}) AS [GROWTH SALES]
	                                                FROM {2} _data
                                                    JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
	                                                where (1=1)  and YEAR = year(DATEADD(MONTH,-1,getdate()))
	                                                	
	                                                GROUP BY DIST,{0}
	                                                ) Y
	                                                JOIN
	                                                (SELECT DIST,{0},SUM({1}) AS [GROWTH SALES]
							                                                FROM {2} _data
                                                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
							                                                where (1=1) and YEAR = (year(DATEADD(MONTH,-1,getdate()))-1)
							                                                GROUP BY DIST,{0}
							                                                 ) LastY

	                                                ON Y.DIST = LastY.DIST ", Cols, colSum, table);
            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {

                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();

                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);

                    }
                    else
                    {


                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);

                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }

                }
            }
            var a = Session["Where"].ToString();
            double Num;
            bool isNum = double.TryParse(filter, out Num);
            if (Session["Where"].ToString().IndexOf("YEAR in") > 0)
            {
                var index = a.IndexOf("YEAR in (N'");
                var year = a.Substring(a.IndexOf("YEAR in (N'") + 11, 4);
                var yearValBefore = Int32.Parse(year) - 1;
                var month = DateTime.Now.Month;
                var year_current = DateTime.Now.Year;
                sql = sql.Replace("YEAR in (N'" + year + "')", "(9=9)");
                sql = sql.Replace("and YEAR = year(DATEADD(MONTH,-1,getdate()))", "and (11=11)");
                sql = sql.Replace("and YEAR = (year(DATEADD(MONTH,-1,getdate())) -1)", "and (22=22)");
                if (month == 1 && year_current == (Int32.Parse(year)))
                {
                    sql = sql.Replace("(11=11)", "YEAR in (N'" + (Int32.Parse(year) - 1) + "')");
                    sql = sql.Replace("(22=22)", "YEAR in (N'" + (yearValBefore - 1) + "')");
                }
                else
                {
                    sql = sql.Replace("(11=11)", "YEAR in (N'" + Int32.Parse(year) + "')");
                    sql = sql.Replace("(22=22)", "YEAR in (N'" + yearValBefore + "')");
                }


            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string OnClickCharV2_PRI_TH(string filter, string colFilter, string Cols, string colSum, string orderBy, string table, string ACTUAL, string TARGET)
        {
            L5sInitial.LoadForLoginPage();
            String sql = "";
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            if (ACTUAL == "ACTUAL" && TARGET == "TRGET")
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                           select {0}
                                            ,round(sum({1}),0) as {1}
											,case when sum({4})= 0 or sum({5}) = 0 then 0 else cast( sum({4}) /sum({5})*100 as numeric(18,0)) end as [PERCENT]
                                            from {3} _data
                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                            where (1=1)
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table, ACTUAL, TARGET);
            }
            else
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1}
                                            from {3}
                                            where (1=1)
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string OnClickCharV3_PRI_TH(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            String sql = "";
            // load map first
            if (Cols == "MONTH")
            {
                sql = String.Format(@"  SELECT {0},ISNULL([{4}],0) AS [Y {4}],ISNULL([{5}],0) AS [Y {5}] FROM(
                                    SELECT {0},MONTH_NUMBER,YEAR,SUM({1}) AS {1}
                                    FROM  {3} _target
                                    JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                    WHERE (1=1)
                                    GROUP BY {0},MONTH_NUMBER,YEAR
                                    ) T1
                                    PIVOT 
                                    (	
	                                    SUM({1}) FOR [YEAR] IN ([{4}],[{5}])
                                    )  AS T2
                                    ORDER BY {2}
                                ", Cols, colSum, orderBy, table, DateTime.Now.Year - 1, DateTime.Now.Year);
            }
            else if (Cols == "YEAR")
            {
                sql = String.Format(@" SELECT 'YEAR' AS [YEAR] ,ISNULL([{3}],0) AS [Y {3}],ISNULL([{4}],0) AS [Y {4}]
                                        FROM (
                                            SELECT {0}, SUM({1}) AS ACTUAL
                                            FROM {2} _target
                                            JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                            WHERE (1=1)
                                            GROUP BY YEAR) T1 PIVOT (
	                                        SUM(ACTUAL) FOR [YEAR] in ([{3}],[{4}])
                                        )   AS T2
                                ", Cols, colSum, table, DateTime.Now.Year - 1, DateTime.Now.Year);
            }
            else
            {
                sql = String.Format(@"  SELECT {0},ISNULL([{4}],0) AS [Y {4}],ISNULL([{5}],0) AS [Y {5}] FROM(
                                    SELECT {0},YEAR,SUM({1}) AS {1}
                                    FROM  {3} _target
                                    JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                    WHERE (1=1)
                                    GROUP BY {0},YEAR
                                    ) T1
                                    PIVOT 
                                    (	
	                                    SUM({1}) FOR [YEAR] IN ([{4}],[{5}])
                                    )  AS T2
                                    ORDER BY {2}
                                ", Cols, colSum, orderBy, table, DateTime.Now.Year - 1, DateTime.Now.Year);
            }


            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where1"].ToString() == "(1=1)")
                {
                    Session["Where1"] = Session["Where1"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where1"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where1"] = Session["Where1"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where1"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where1"] = Session["Where1"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where1"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string OnClickCharV4_PRI_TH(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            String sql = "";
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }

            if (Cols == "MTD")
            {
                sql = String.Format(@"  DECLARE @GETDATE datetime
                                        SET @GETDATE=GETDATE()

                                        DECLARE @ACTUAL float
                                        SET @ACTUAL=
                                        (SELECT ROUND(SUM(ACTUAL),0) AS [TotalCoverage]
                                        FROM D_MTD_ACT_TARGET _target
                                        JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                        WHERE [YEAR]=YEAR(@GETDATE) AND MONTH_NUMBER=MONTH(@GETDATE) AND (2=2))

                                        DECLARE @TARGET float
                                        SET @TARGET=
                                        (SELECT ROUND(SUM(TARGET),0) AS [TotalCoverage]
                                        FROM D_MTD_ACT_TARGET _target
                                        JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                        WHERE [YEAR]=YEAR(@GETDATE) AND MONTH_NUMBER=MONTH(@GETDATE) AND (2=2) )

                                        SELECT 'MTD Actual' AS [Coverage], @ACTUAL as [TotalCoverage]
                                        UNION ALL
                                        SELECT '' AS [Coverage], CASE WHEN (@TARGET-@ACTUAL) <0 THEN 0 ELSE (@TARGET-@ACTUAL) END as [TotalCoverage]
                                ");
            }
            else
            {
                sql = String.Format(@"  DECLARE @GETDATE datetime
                                    SET @GETDATE=GETDATE()

                                    SELECT {0},ROUND(SUM({1}),0) AS [MTD Actual],ROUND(SUM(TARGET),0) AS [Sales Target]
                                    FROM {3} _target
                                    JOIN M_DISTRIBUTOR _dist ON _target.DISTRIBUTOR_CODE=_dist.DISTRIBUTOR_CODE
                                    WHERE [YEAR]=YEAR(@GETDATE) AND MONTH_NUMBER=MONTH(@GETDATE) AND (2=2)
                                    GROUP BY {0},{2}
                                    ORDER BY {2} ASC
                                ", Cols, colSum, orderBy, table);
            }


            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where2"].ToString() == "(2=2)")
                {
                    Session["Where2"] = Session["Where2"].ToString().Replace("(2=2)", String.Format("(2=2) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                    Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where2"] = Session["Where2"].ToString().Replace("(2=2)", String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                        Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(2=2)", "");
                            Session["Where2"] = Session["Where2"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(2=2) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(2=2)", Session["Where2"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        //**********END YTD_SALES_DATA_PRIMARY**********

        //**********MTD_SALES_DATA_PRIMARY**********
        // 30/09/2020

        [WebMethod(EnableSession = true)]
        public string OnClickChar_PRI_MON_TH(string filter, string colFilter, string Cols, string colSum, string orderBy, string table)
        {
            L5sInitial.LoadForLoginPage();
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            String sql = "";
            string sqlCountry = @"SELECT 
                             [COUNTRY_NAME]
                          FROM [M_COUNTRY]";
            DataTable dtCountry = L5sSql.Query(sqlCountry);
            string nameCountry = dtCountry.Rows[0]["COUNTRY_NAME"].ToString();

            if ((table == "D_MTD_PRIMARY_UPDATE" || table == "D_MTD_SECONDATY_SALES_UPDATE") && nameCountry == "TH")
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1} 
                                            from {3} _data
                                            left JOIN 
                                            (	SELECT DISTINCT are.AREA_CODE
                                            FROM (					
                                            [M_DISTRIBUTOR] dis 
                                            join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
                                            join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
                                            join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
											join M_AREA_PROVINCE P on p.PROVINCE_CD = pro.PROVINCE_CD and P.ACTIVE = 1
                                            join M_AREA are on are.AREA_CD = P.AREA_CD and are.ACTIVE = 1
                                            join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                            ) where dis.ACTIVE = 1 ) are on _data.AREA = are.AREA_CODE
                                            where (1=1) 
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table);
            }
            else
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1} 
                                            from {3} _data
                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                            where (1=1) 
                                            group by {0}
                                        ) t1 order by {2} 
                                ", Cols, colSum, orderBy, table);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }
            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string OnClickCharV2_PRI_MON_TH(string filter, string colFilter, string Cols, string colSum, string orderBy, string table, string ACTUAL, string TARGET)
        {
            L5sInitial.LoadForLoginPage();
            String sql = "";
            // load map first
            if (colFilter == "MONTH" || colFilter == "AREA")
            {
                filter = filter.Replace(",", "\',\'");
            }
            if (ACTUAL == "ACTUAL" && TARGET == "TRGET")
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                           select {0}
                                            ,round(sum({1}),0) as {1}
											,case when sum({4})= 0 or sum({5}) = 0 then 0 else cast( sum({4}) /sum({5})*100 as numeric(18,0)) end as [PERCENT]
                                            from {3} _data
                                            JOIN M_DISTRIBUTOR _dist ON _data.DIST=_dist.DISTRIBUTOR_CODE
                                            where (1=1)
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table, ACTUAL, TARGET);
            }
            else
            {
                sql = String.Format(@"select *
                                        from
                                        (
                                            select {0}
                                                 ,round(sum({1}),0) as {1}
                                            from {3}
                                            where (1=1)
                                            group by {0}
                                        ) t1 order by {2}
                                ", Cols, colSum, orderBy, table);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (Session["Where"].ToString() == "(1=1)")
                {
                    Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter));
                    sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}') ", filter, colFilter);
                }
                else
                {
                    Session[colFilter] = Session[colFilter] == null ? "" : Session[colFilter].ToString();
                    if (string.IsNullOrEmpty(Session[colFilter].ToString()))
                    {
                        Session["Where"] = Session["Where"].ToString().Replace("(1=1)", String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter));
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                        Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                    }
                    else
                    {
                        if (Session[colFilter].ToString() != String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter))
                        {
                            string ss = Session[colFilter].ToString().Replace("(1=1)", "");
                            Session["Where"] = Session["Where"].ToString().Replace(ss, String.Format(" and {1} in (N'{0}')", filter, colFilter));
                            Session[colFilter] = String.Format("(1=1) and {1} in (N'{0}')", filter, colFilter);
                        }
                        sql = sql.Replace("(1=1)", Session["Where"].ToString());
                    }
                }
            }

            DataTable dt = L5sSql.Query(sql);
            string json = GetDataChart.P5sConvertDataTabletoJson(dt);
            return json;
        }

        //**********END MTD_SALES_DATA_PRIMARY**********
    }
}
