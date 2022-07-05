using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using P5sDmComm;
using L5sDmComm;
using System.Data;
using System.Collections;

namespace MMV.CPTH.DashboardTH.NewDashboard
{
    public partial class RunScriptDataSQL : System.Web.UI.Page
    {

        ArrayList Resualt = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {

            L5sInitial.Load(ViewState);
        }

        protected void P5stxtChoose_Click(object sender, EventArgs e)
        {
            String ipAddress = P5sCmm.P5sCmmFns.getIpAddress(this.Page); //lấy thông tin IP gọi trang chốt số

            //nếu tham số IP được phép chốt số chưa thiết lập thì exit
            DataTable dtIP = L5sSql.Query("SELECT * FROM S_PARAMS WHERE NAME = 'LOCAL_IP_ADDRESS' ");
            if (dtIP == null || dtIP.Rows.Count == 0)
                return;
            //kiểm tra IP gọi tới có hợp lệ hay không
            if (ipAddress.Contains(dtIP.Rows[0]["VALUE"].ToString()))
            {


                #region NgoanSQL
                string kq = P5stxtScript.Text;


                string sql = (@"
                           --------D_MTD_ACT_TARGET------------
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                                    WHERE TABLE_NAME = 'D_MTD_ACT_TARGET')
                        BEGIN
	                        PRINT('')
                        END
                        ELSE
                        BEGIN

                            CREATE TABLE [dbo].[D_MTD_ACT_TARGET](
	                            [MTD_ACT_TARGET_CD] [bigint] IDENTITY(1,1) NOT NULL,
	                            [REGION] [nvarchar](250) NULL,
	                            [DISTRIBUTOR] [nvarchar](250) NULL,
	                            [DSR] [nvarchar](250) NULL,
	                            [TARGET] [float] NULL,
	                            [ACTUAL] [float] NULL,
	                            [YEAR] [varchar](10) NULL,
	                            [QUARTER] [varchar](10) NULL,
	                            [MONTH] [varchar](10) NULL,
	                            [MONTH_NUMBER] [int] NULL,
	                            [CREATED_DATE] [datetime] NULL,
	                            [ACTIVE] [bit] NULL,
                             CONSTRAINT [PK_D_MTD_ACT_TARGET] PRIMARY KEY CLUSTERED 
                            (
	                            [MTD_ACT_TARGET_CD] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]

                        

                            SET ANSI_PADDING OFF
                     

                            ALTER TABLE [dbo].[D_MTD_ACT_TARGET] ADD  CONSTRAINT [DF_D_MTD_ACT_TARGET_CREATED_DATE]  DEFAULT (getdate()) FOR [CREATED_DATE]
                        

                            ALTER TABLE [dbo].[D_MTD_ACT_TARGET] ADD  CONSTRAINT [DF_D_MTD_ACT_TARGET_ACTIVE]  DEFAULT ((1)) FOR [ACTIVE]
                     
                        END
                         --------[D_COVERAGE_BUYING_STORE_VISIT]------------
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                                    WHERE TABLE_NAME = 'D_COVERAGE_BUYING_STORE_VISIT')
                        BEGIN
	                        PRINT('')
                        END
                        ELSE
                        BEGIN    

                            CREATE TABLE [dbo].[D_COVERAGE_BUYING_STORE_VISIT](
	                            [CD_COVERAGE_BUYING_STORE_VISIT] [bigint] IDENTITY(1,1) NOT NULL,
	                            [REGION] [nvarchar](100) NULL,
	                            [DISTRIBUTOR_CODE] [nvarchar](100) NULL,
	                            [DISTRIBUTOR_NAME] [nvarchar](100) NULL,
	                            [SALES_CODE] [nvarchar](100) NULL,
	                            [SALES_NAME] [nvarchar](100) NULL,
	                            [YEAR] [nvarchar](50) NULL,
	                            [MONTH] [nvarchar](50) NULL,
	                            [BUYING] [numeric](18, 1) NULL,
	                            [COVERAGE] [numeric](18, 1) NULL,
	                            [STORE_VISIT] [numeric](18, 1) NULL,
	                            [CREATED_DATE] [datetime] NULL,
                             CONSTRAINT [PK_D_COVERAGE_BUYING_STORE_VISIT] PRIMARY KEY CLUSTERED 
                            (
	                            [CD_COVERAGE_BUYING_STORE_VISIT] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]

                     

                            ALTER TABLE [dbo].[D_COVERAGE_BUYING_STORE_VISIT] ADD  CONSTRAINT [DF_D_COVERAGE_BUYING_STORE_VISIT_CREATED_DATE]  DEFAULT (getdate()) FOR [CREATED_DATE]
                       
                        END     
                        --------D_MSS_DISTRIBUTION-----
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                                    WHERE TABLE_NAME = 'D_MSS_DISTRIBUTION')
                        BEGIN
	                        PRINT('')
                        END
                        ELSE
                        BEGIN
                            CREATE TABLE [dbo].[D_MSS_DISTRIBUTION](
	                            [CD_MSS_DIST] [bigint] IDENTITY(1,1) NOT NULL,
	                            [REGION] [nvarchar](100) NULL,
	                            [DISTRIBUTOR_CODE] [nvarchar](100) NULL,
	                            [DISTRIBUTOR_NAME] [nvarchar](100) NULL,
	                            [SALES_CODE] [nvarchar](100) NULL,
	                            [SALES_NAME] [nvarchar](100) NULL,
	                            [RE_CODE] [nvarchar](100) NULL,
	                            [RE_DESCRIPTION] [nvarchar](100) NULL,
	                            [MONTH] [nvarchar](50) NULL,
	                            [YEAR] [nvarchar](50) NULL,
	                            [Point_RL] [numeric](18, 3) NULL,
	                            [TIME_GONE] [nvarchar](50) NULL,
	                            [CREATED_DATE] [datetime] NULL,
                             CONSTRAINT [PK_D_MSS_DISTRIBUTION] PRIMARY KEY CLUSTERED 
                            (
	                            [CD_MSS_DIST] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]

                      

                            ALTER TABLE [dbo].[D_MSS_DISTRIBUTION] ADD  CONSTRAINT [DF_D_MSS_DISTRIBUTION_CREATED_DATE]  DEFAULT (getdate()) FOR [CREATED_DATE]
                          
                        END");
                string sql1 = (@"
                            select *
		                    FROM INFORMATION_SCHEMA.ROUTINES
		                    WHERE ROUTINE_TYPE='PROCEDURE' and SPECIFIC_NAME = 'Consolidate_D_Coverage_Buying_StoreVisit'
                        ");
                string sql2 = (@"select*
                            FROM INFORMATION_SCHEMA.ROUTINES
                            WHERE ROUTINE_TYPE = 'PROCEDURE' and SPECIFIC_NAME = 'Consolidate_D_MSS_DISTRIBUTION'");

                string sql3 = (@"select*
                            FROM INFORMATION_SCHEMA.ROUTINES
                            WHERE ROUTINE_TYPE = 'PROCEDURE' and SPECIFIC_NAME = 'Consolidate_MTD_TO_D_MTD_ACT_TARGET'");

                DataTable dt = new DataTable();
                if (kq.Equals("1"))
                {
                    try
                    {

                        dt = L5sSql.Query(sql);
                        _TxtLog.Text = ("Create table of dashboard successfull!");


                    }
                    catch (Exception ex)
                    {
                        _TxtLog.Text = "Create table of dashboard fail!";

                    }
                }
                if (kq.Equals("2"))
                {
                    try
                    {
                        dt = L5sSql.Query(sql1);
                        if (dt.Rows.Count.ToString() == "0")
                        {
                            L5sSql.Execute(@"
                        CREATE proc[dbo].[Consolidate_D_Coverage_Buying_StoreVisit]

                        as
                        begin 
                                                            DECLARE @SUBFORMATDATE INT

                                                            DECLARE @SUBFTODATE INT
                                                            DECLARE @FORMDATE DATE
                                                            DECLARE @TODATE DATE
                                                            SET @FORMDATE = GETDATE()
                                                            SET @TODATE = GETDATE()

                                                            SET @SUBFORMATDATE = CAST(SUBSTRING(convert(varchar, @FORMDATE, 112), 0, LEN(Convert(varchar, @FORMDATE, 112)) - 1) AS INT)

                                                            SET @SUBFTODATE = CAST(SUBSTRING(convert(varchar, @TODATE, 112), 0, LEN(Convert(varchar, @TODATE, 112)) - 1) AS INT)



                                                             IF OBJECT_ID('tempdb..#TB_SummaryReport') IS NOT NULL
                                                            BEGIN
                                                                DROP TABLE #TB_SummaryReport
                                                            END


                                                            Create Table #TB_SummaryReport
	                                                            (

                                                                    [STOCKIST_CODE] nvarchar(50),

                                                                    [STOCKIST_NAME] nvarchar(500),

                                                                    [SAP_SALES_MAN_CODE] nvarchar(50),

                                                                    [SALES_MAN_NAME] nvarchar(500),

                                                                    [DMS_DATE] Datetime default getdate(),

                                                                    [DELAY_DATE] int,

                                                                    [C_HEADER] nvarchar(100),

                                                                    [C_VALUES] float,

                                                                    [MSS_RE] nvarchar(50) default NULL,

                                                                )


                                                            IF OBJECT_ID('tempdb..#TB_Performance') IS NOT NULL
                                                            BEGIN
                                                                DROP TABLE #TB_Performance
                                                            END

                                                            Create Table #TB_Performance
	                                                            (

                                                                    [STOCKIST_CODE] nvarchar(50),

                                                                    [STOCKIST_NAME] nvarchar(500),
		                                                            [SAP_SALES_MAN_CODE] nvarchar(50),
		                                                            [SALES_MAN_NAME] nvarchar(500),
		                                                            [DMS_DATE] Datetime default getdate(),
		                                                            [DELAY_DATE] int,
		                                                            [C_HEADER] nvarchar(100),
		                                                            [C_VALUES] float,
											                        [MSS_RE] nvarchar(50) default NULL,
	                                                            )


									                        IF OBJECT_ID('tempdb..#TemplateBuying') IS NOT NULL
                                                            BEGIN
                                                                DROP TABLE #TemplateBuying
                                                            END

                                                            Create Table #TemplateBuying
	                                                            (
                                                                    [STOCKIST_CODE] nvarchar(50),
		                                                            [STOCKIST_NAME] nvarchar(500),
		                                                            [SAP_SALES_MAN_CODE] nvarchar(50),
		                                                            [SALES_MAN_NAME] nvarchar(500),
		                                                            [DMS_DATE] Date ,
		                                                            [DELAY_DATE] int,
		                                                            [C_HEADER] nvarchar(100),
		                                                            [C_VALUES] float,
											                        [MSS_RE] nvarchar(50) default NULL,
	                                                            )
						                        declare @sqlTITO2 NVARCHAR(MAX)

                                                declare @sqlTITO NVARCHAR(MAX)

                                                declare @sqlTimeGone NVARCHAR(MAX)

                                                declare @yyyy nvarchar(5)

                                                declare @NumberOfCurrent int
                                                declare @NumberOfSales int
                                                declare @NumberOfSunday int
                                                declare @DayCurrent int
                                                declare @TimeGone nvarchar(50)

                                                declare @NumberOfdays float
                                                set @yyyy = LEFT(CONVERT(nvarchar, getdate(),112),4)
						                        DECLARE @StartDate DATE = '20180101', @NumberOfYears INT = 10;
						                        -- prevent set or regional settings from interfering with 
						                        -- interpretation of dates / literals

                                                SET DATEFIRST 7;
						                        SET DATEFORMAT mdy;
						                        SET LANGUAGE US_ENGLISH;

						                        DECLARE @CutoffDate DATE = DATEADD(YEAR, @NumberOfYears, @StartDate);

						                        -- this is just a holding table for intermediate calculations:

                                                IF OBJECT_ID('tempdb..#dim') IS NOT NULL
                                                                        BEGIN

                                                                            DROP TABLE #dim
												                        END
                                                CREATE TABLE #dim
						                        (
                                                [date]       DATE PRIMARY KEY,
                                                [day]        AS DATEPART(DAY,      [date]),
                                                [month]      AS DATEPART(MONTH,    [date]),
                                                FirstOfMonth AS CONVERT(DATE, DATEADD(MONTH, DATEDIFF(MONTH, 0, [date]), 0)),
						                        [MonthName] AS DATENAME(MONTH, [date]),
						                        [week] AS DATEPART(WEEK, [date]),
						                        [ISOweek] AS DATEPART(ISO_WEEK, [date]),
						                        [DayOfWeek] AS DATEPART(WEEKDAY, [date]),
						                        [quarter] AS DATEPART(QUARTER, [date]),
						                        [year] AS DATEPART(YEAR, [date]),
						                        FirstOfYear AS CONVERT(DATE, DATEADD(YEAR, DATEDIFF(YEAR,  0, [date]), 0)),
						                        Style112 AS CONVERT(CHAR(8),   [date], 112),
						                        Style101 AS CONVERT(CHAR(10),  [date], 101)
						                        );
						                        -- use the catalog views to generate as many rows as we need


                                                INSERT #dim([date]) 
						                        SELECT d
                                                FROM
                                                (

                                                SELECT d = DATEADD(DAY, rn - 1, @StartDate)

                                                FROM

                                                (

                                                SELECT TOP (DATEDIFF(DAY, @StartDate, @CutoffDate))

                                                rn = ROW_NUMBER() OVER(ORDER BY s1.[object_id])

                                                FROM sys.all_objects AS s1

                                                CROSS JOIN sys.all_objects AS s2
						                        -- on my system this would support > 5 million days

                                                ORDER BY s1.[object_id]

                                                ) AS x
						                        ) AS y;

                                select @NumberOfCurrent = DATEPART(DD, getdate()) - 1

                                                select @DayCurrent = DATEPART(DD, getdate())

                                                select @NumberOfSunday = count(*)
                                                --select*
                                                from #dim
						                        where[date] between getdate()-@DayCurrent and getdate() and DayOfWeek = 1
                                               --so ngay sales lam viec tru di chu nhat
                                               select @NumberOfSales = @NumberOfCurrent - @NumberOfSunday
						                        ---tong so ngay trong thang tru chu nhat

                                                select @NumberOfdays = convert(int, day(eomonth(getdate())) - 4)
                                                --tinh time gone(so ngay di lam cua sale chia cho tong so ngay trong thang tru di chu nhat)

                                                select @TimeGone = cast((@NumberOfSales / @NumberOfdays) as decimal(18, 5))



                                                declare @TITO nvarchar(max)

                                                declare @a date
                                                set @a=GETDATE()
						                        -----------------
						                        DECLARE @SQL_QUREY nvarchar(max)

                                                DECLARE @RESUAL nvarchar(max)

                                                SET @RESUAL = ''

                                                DECLARE CursorRE Cursor for
						                        select 'select * from [MMV_CONSOLIDATE_SALES].[dbo].'+t1.TABLE_NAME_1 +' '+'union all' as SQL_QUREY
                                                from(select distinct mtb.TABLE_NAME_1
                                                from #dim d
						                        join[MMV_CONSOLIDATE_SALES].[dbo].[M_TABLE] mtb on d.week = mtb.WEEK

                                                where month(date) = month(@a) and year(date) = year(@a) and mtb.TYPE_CD = 3 and mtb.YEAR = @yyyy) t1

                                               OPEN CursorRE
                                               FETCH NEXT FROM CursorRE
                                               into @SQL_QUREY

                                               WHILE @@FETCH_STATUS = 0   --vòng l?p WHILE khi ??c Cursor thành công

                                                BEGIN

                                                SET @RESUAL = @RESUAL +' '+ @SQL_QUREY

                                                FETCH NEXT FROM CursorRE -- ??c dòng ti? p

                                                into @SQL_QUREY

                                                eND
                                                CLOSE CursorRE              -- ?óng Cursor

                                                DEALLOCATE CursorRE         -- Gi? i phóng tài nguyên
                                                 declare @result nvarchar(max)

                                                SET @result = SUBSTRING(@RESUAL, -8, LEN(@RESUAL))
                                                --SELECT @result


                                                IF OBJECT_ID('tempdb..#MMV_CONSOLIDATE_SALES') is not null
							                        drop table #MMV_CONSOLIDATE_SALES
						                        create table #MMV_CONSOLIDATE_SALES(
						                         [ROUTE_CD] [bigint] NULL,
						                        [SALES_CD] [bigint] NULL,
						                        [DISTRIBUTOR_CD] [bigint] NULL,
						                        [CUSTOMER_CD] [bigint] NULL,
						                        [CUSTOMER_CODE] [nvarchar] (50) NULL,
						                        [TIME_IN_LATITUDE_LONGITUDE] [nvarchar] (50) NULL,
						                        [TIME_IN_LATITUDE_LONGITUDE_ACCURACY] [float] NULL,
						                        [TIME_OUT_LATITUDE_LONGITUDE] [nvarchar] (50) NULL,
						                        [TIME_OUT_LATITUDE_LONGITUDE_ACCURACY] [float] NULL,
						                        [TIME_IN_CREATED_DATE] [datetime] NULL,
						                        [TIME_OUT_CREATED_DATE] [datetime] NULL,
						                        [CREATED_DATE] [datetime] NULL,
						                        [TYPE_CD] [bigint] NULL,
						                        [TIME_IN_LOCATION_ADDRESS] [nvarchar] (512) NULL,
						                        [TIME_OUT_LOCATION_ADDRESS] [nvarchar] (512) NULL,
						                        [MAX_DATETIME_TRACKING] [nvarchar] (50) NULL,
						                        [LOCATION_IS_NULL] [bit] NULL,
						                        [TIME_IN_OUT_CD] [bigint] NULL
						                        )
					                        -- select* from[MMV_CONSOLIDATE_SALES].[dbo].O_TIME_IN_OUT_2020_05
                        insert into #MMV_CONSOLIDATE_SALES
						                        EXECUTE sp_executesql @result


                                                IF OBJECT_ID('tempdb..#TMP_COUNT_STORE_VISIT') is not null
								                        drop table #TMP_COUNT_STORE_VISIT


                                                SELECT sale.SALES_CODE, dis.DISTRIBUTOR_CODE, COUNT(CUSTOMER_CODE) AS COUNT_CUSTOMER into #TMP_COUNT_STORE_VISIT
						                        FROM #MMV_CONSOLIDATE_SALES d 
						                        join M_SALES sale on sale.SALES_CD = d.SALES_CD and sale.ACTIVE = 1

                                                join[M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = d.DISTRIBUTOR_CD and dis.ACTIVE = 1

                                                GROUP BY sale.SALES_CODE, dis.DISTRIBUTOR_CODE


                                                            IF OBJECT_ID('tempdb..#TMP_SH') IS NOT NULL
                                                                DROP TABLE #TMP_SH
										                        SELECT SH.DISTRIBUTOR_CODE,SH.DISTRIBUTOR_CODE_CD,SH.SAP_SALESMAN_CODE
											                        ,SH.SAP_SALESMAN_CODE_CD,SI.KZWI1,SH.BILL_DATE_TIME,RE,SH.INVOICE_NO
											                        ,SH.SAP_CUST_CODE,SH.SAP_CUST_CODE_CD,SI.PRODUCT_CODE,SH.SUB_RE,SH.SUB_RE_CD ,SI.SALES_IN_A_UNIT,NET_VALUES INTO #TMP_SH
										                        FROM TH_SALESH_DATA SH
                                                                JOIN TH_SALESI_DATA SI ON SH.INVOICE_NO = SI.INVOICE_NO AND SH.ACTIVE = 1

                                                                WHERE SI.ACTIVE = 1

                                                                and CAST(SUBSTRING(convert(varchar, BILL_DATE_TIME, 112),0,LEN(Convert(varchar, BILL_DATE_TIME, 112))-1 ) AS INT)=@SUBFTODATE 
										                        --and sh.DISTRIBUTOR_CODE_CD in ({ 2}) 
										                        --and si.NET_VALUES > 0 and sh.NET_VALUE > 0


                                                            IF OBJECT_ID('tempdb..#TMP_SH_Buying') IS NOT NULL
                                                                DROP TABLE #TMP_SH_Buying
										                        select * into #TMP_SH_Buying
										                        from #TMP_SH 
										                        where NET_VALUES > 0 

                                                           IF OBJECT_ID('tempdb..#TMP_SH_Buying_Sum') IS NOT NULL
                                                                DROP TABLE #TMP_SH_Buying_Sum
										                        select * into #TMP_SH_Buying_Sum
										                        from(

                                                                select DISTRIBUTOR_CODE, DISTRIBUTOR_CODE_CD, SAP_SALESMAN_CODE, RE

                                                                , SAP_SALESMAN_CODE_CD, SAP_CUST_CODE, sum(NET_VALUES) as NET_VALUES
                                                                from #TMP_SH 
										                        group by  DISTRIBUTOR_CODE, DISTRIBUTOR_CODE_CD, SAP_SALESMAN_CODE, RE

                                                                , SAP_SALESMAN_CODE_CD, SAP_CUST_CODE

                                                                ) t
                                                                where NET_VALUES > 0
                                     


									                        if OBJECT_ID('tempdb..#TMP_MSS') IS NOT NULL
                                                                DROP TABLE #TMP_MSS

									                             select GRP.PRODUCT_CD,GRP.MSS_GROUP_CD
						                                                            ,RE.MSS_RE_CD
						                                                            ,MRE.RE_CODE ,MRE.RE_DESCRIPTION,MGRP.MSS_GROUP_DESCRIPTION
						                                                            ,GRP.COMBO ,GRP.PERCENT_DISTRIBUTION ,GRP.SPECIAL,MRE.PARENT_RE_CD
						                                                            ,MGRP.MIN_PIECE INTO #TMP_MSS
					                                                            from O_MSS_PRODUCT_GROUP GRP
                                                                                join O_MSS_GROUP_RE RE on RE.MSS_GROUP_CD = GRP.MSS_GROUP_CD and RE.ACTIVE = 1

                                                                               join M_MSS_RE MRE on MRE.MSS_RE_CD = RE.MSS_RE_CD and MRE.ACTIVE = 1

                                                                               join M_MSS_GROUP MGRP on MGRP.MSS_GROUP_CD = GRP.MSS_GROUP_CD and MGRP.ACTIVE = 1

                                                                               join O_PRODUCT pro on pro.PRODUCT_CD = grp.PRODUCT_CD and pro.ACTIVE = 1

                                                                               where GRP.ACTIVE = 1 and (@TODATE between MGRP.START_DATE and MGRP.END_DATE)

						

								                        if OBJECT_ID('tempdb..#TMP_No_Disnct') IS NOT NULL
                                                                DROP TABLE #TMP_No_Disnct

											                        select  distinct dis.DISTRIBUTOR_CODE

                                                                        , dis.DISTRIBUTOR_NAME, prod_RE.MSS_GROUP_DESCRIPTION, SUB_RE

                                                                        , sale.SALES_CODE, sale.SALES_NAME, sale.SALES_CD, SH.DISTRIBUTOR_CODE_CD, SH.SAP_SALESMAN_CODE_CD, SH.RE, SH.PRODUCT_CODE, SAP_CUST_CODE

                                                                        , getdate() -1   as DMS_DATE

                                                                        , 0 as DELAY_DATE

                                                                        ,'04. #Buying'  as C_HEADER

                                                                        , prod_RE.PARENT_RE_CD
												                        --, prod_RE.COMBO , SaleI.KZWI1, prod_RE.PERCENT_DISTRIBUTION

                                                                        , case

                                                                                when prod_RE.COMBO = 1 then  ((SH.NET_VALUES* prod_RE.PERCENT_DISTRIBUTION)/100) 
												                          else SH.NET_VALUES end as NET_VALUES into #TMP_No_Disnct
										                        from(SELECT* FROM #TMP_SH_Buying) SH
										                         join O_PRODUCT prod on prod.PRODUCT_CODE = SH.PRODUCT_CODE and prod.ACTIVE = 1

                                                                 join (
												                        -- lấy ra sản phẩm thuộc MSS GROUP đang hoạt động

                                                                        SELECT* FROM #TMP_MSS
												                        --where SPECIAL = 0  and COMBO = 0-- SP thuộc special ko cần count


                                                                    ) prod_RE on prod_RE.PRODUCT_CD = prod.PRODUCT_CD and prod_RE.MSS_RE_CD = SH.SUB_RE_CD
                                                                 join[M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = SH.DISTRIBUTOR_CODE_CD and dis.ACTIVE = 1
										                         join M_SALES sale on sale.SALES_CD = SH.SAP_SALESMAN_CODE_CD and sale.ACTIVE = 1
										                        where SH.SALES_IN_A_UNIT >= prod_RE.MIN_PIECE -- ĐK min piece
										



									
									                        if OBJECT_ID('tempdb..#TMP_Disnct') IS NOT NULL
                                                                DROP TABLE #TMP_Disnct
									
									                        select distinct dis.DISTRIBUTOR_CODE

                                                                            , dis.DISTRIBUTOR_NAME

                                                                             , sale.SALES_CODE, sale.SALES_NAME, SH.DISTRIBUTOR_CODE_CD


                                                                                , prod_RE.PARENT_RE_CD, sale.SALES_CD into #TMP_Disnct
			                                                            from (SELECT* FROM #TMP_SH) SH
													                         join O_PRODUCT prod on prod.PRODUCT_CODE = SH.PRODUCT_CODE and prod.ACTIVE = 1

                                                                             join (
															                        -- lấy ra sản phẩm thuộc MSS GROUP đang hoạt động

                                                                                    SELECT* FROM #TMP_MSS
															                        --and GRP.SPECIAL = 0-- SP thuộc special ko cần count


                                                                                ) prod_RE on prod_RE.PRODUCT_CD = prod.PRODUCT_CD and prod_RE.MSS_RE_CD = SH.SUB_RE_CD
                                                                             join[M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = SH.DISTRIBUTOR_CODE_CD and dis.ACTIVE = 1
													                         join M_SALES sale on sale.SALES_CD = SH.SAP_SALESMAN_CODE_CD and sale.ACTIVE = 1
			                                                            where SH.SALES_IN_A_UNIT >= prod_RE.MIN_PIECE -- ĐK min piece
                                                   
										
										                        -------------RouteList - 01. RL--------------------

                                                                    insert #TB_Performance -- đẩy vào bảng tạm
											                        select dis.DISTRIBUTOR_CODE, dis.DISTRIBUTOR_NAME,
                                                                    sale.SALES_CODE, SALE.SALES_NAME

                                                                    , getdate() -1   as DMS_DATE

                                                                    , 0 as DELAY_DATE

                                                                    ,'01. RL' as C_HEADER

                                                                    , COUNT(*) as C_VALUES

                                                                    , NULL
                                                                    from M_SALES sale

                                                                    join O_SALES_ROUTE osr on osr.SALES_CD = sale.SALES_CD and osr.ACTIVE = 1

                                                                    join O_CUSTOMER_ROUTE ocr on ocr.ROUTE_CD = osr.ROUTE_CD and ocr.ACTIVE = 1

                                                                    join M_CUSTOMER cust on cust.CUSTOMER_CD = ocr.CUSTOMER_CD and cust.ACTIVE = 1

                                                                    join[M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sale.DISTRIBUTOR_CD and dis.ACTIVE = 1

                                                                    where sale.ACTIVE= 1
                                                                    --and dis.DISTRIBUTOR_CD in ({ 2})
											                        group by dis.DISTRIBUTOR_CODE,dis.DISTRIBUTOR_NAME,
											                        sale.SALES_CODE,SALE.SALES_NAME
										                        ----insert buying


                                                                 insert #TemplateBuying -- đẩy vào bảng tạm 
	                                                             select

                                                                        TBuying.DISTRIBUTOR_CODE,DIS.DISTRIBUTOR_NAME
			                                                            ,sale.SALES_CODE,sale.SALES_NAME
			                                                            , getdate() -1   as DMS_DATE	
			                                                            , 0 as DELAY_DATE
			                                                            ,'02. Buying'  as C_HEADER
			                                                            ,COUNT(TBuying.SAP_CUST_CODE) as C_VALUES
												                        ,NULL


                                                                    from M_SALES sale

                                                                    join[M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sale.DISTRIBUTOR_CD and dis.ACTIVE = 1

                                                                    join --LEFT JOIN

                                                                       (
                                                                       select DISTRIBUTOR_CODE, DISTRIBUTOR_CODE_CD, SAP_SALESMAN_CODE, SAP_SALESMAN_CODE_CD, SAP_CUST_CODE
                                                                      from #TMP_SH_Buying_Sum
		
			                                                            ) TBuying on TBuying.SAP_SALESMAN_CODE_CD = sale.SALES_CD and dis.DISTRIBUTOR_CD = TBuying.DISTRIBUTOR_CODE_CD


                                                                   group by TBuying.DISTRIBUTOR_CODE, DIS.DISTRIBUTOR_NAME

                                                                           , sale.SALES_CODE, sale.SALES_NAME

									
									                        --------------Insert vao bang #TB_Performance-----
									
									                        --INSERT %ECC
                                                           insert #TB_Performance


                                                           select tb.STOCKIST_CODE, tb.STOCKIST_NAME, tb.SAP_SALES_MAN_CODE, tb.SALES_MAN_NAME, tb.DMS_DATE, tb.DELAY_DATE,'03. %ECC' as C_HEADER

                                                           ,case when buying.C_VALUES = 0 or tb.C_VALUES = 0 then  0  else  (cast (((buying.C_VALUES/tb.C_VALUES) * 100 )as numeric(18,2)) ) end as C_VALUES,BUYING.MSS_RE
                                                          from #TemplateBuying  buying  
									                        right join(select* from #TB_Performance where C_HEADER='01. RL') tb on buying.SAP_SALES_MAN_CODE = tb.SAP_SALES_MAN_CODE


									
									                        --insert #TB_SummaryReport


									                        IF OBJECT_ID ('tempdb..#D_MSS_DISTRIBUTION') IS NOT NULL
                                                                DROP TABLE #D_MSS_DISTRIBUTION

									                        select  reg.REGION_CODE,per.[STOCKIST_CODE],per.[STOCKIST_NAME]
									                        ,per.[SAP_SALES_MAN_CODE],per.[SALES_MAN_NAME],PER.MSS_RE as RE_CODE
									                        ,(select RE_DESCRIPTION from M_MSS_RE WHERE ACTIVE = 1 AND RE_CODE = PER.MSS_RE ) AS RE_DESCRIPTION
                                                            , month(getdate()) as [MONTH],year(getdate()) as [YEAR],round(per.[C_VALUES],1) as [C_VALUES]
                                INTO #D_MSS_DISTRIBUTION
									                        from #TB_Performance per 
									                        JOIN[M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CODE = per.STOCKIST_CODE and dis.ACTIVE = 1

                                                           join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1

                                                           join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1

                                                           join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1

                                                           join M_AREA are on are.AREA_CD = pro.AREA_CD and are.ACTIVE = 1

                                                           join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1

                                                           WHERE C_HEADER = '03. %ECC'

                                                          ORDER BY SAP_SALES_MAN_CODE ASC


                                                       truncate table D_COVERAGE_BUYING_STORE_VISIT
								
				                        -----------------update D_COVERAGE_BUYING_STORE_VISIT----------------

                                                       update das set das.COVERAGE = d.RL
                                                                       , das.BUYING = d.Buying
                                                                       , das.STORE_VISIT = d.STORE_VISIT


                                                       from D_COVERAGE_BUYING_STORE_VISIT das
                                                       join


                                                       (select d.REGION_CODE, d.STOCKIST_CODE, d.STOCKIST_NAME, d.SAP_SALES_MAN_CODE, d.SALES_MAN_NAME, D.[YEAR], D.[MONTH], d.RL as RL, buying.C_VALUES as Buying, d.STORE_VISIT
                                                       from (
                                                           SELECT distinct d.REGION_CODE, d.STOCKIST_CODE, d.STOCKIST_NAME, d.SAP_SALES_MAN_CODE, d.SALES_MAN_NAME, D.[YEAR], D.[MONTH], p.C_VALUES as RL, v.COUNT_CUSTOMER as STORE_VISIT

                                                           FROM #D_MSS_DISTRIBUTION D
									                        left join #TMP_COUNT_STORE_VISIT v on d.SAP_SALES_MAN_CODE = v.SALES_CODE and d.STOCKIST_CODE = v.DISTRIBUTOR_CODE
									                        right join (select* from #TB_Performance where C_HEADER='01. RL') p on p.SAP_SALES_MAN_CODE = d.SAP_SALES_MAN_CODE and p.STOCKIST_CODE = d.STOCKIST_CODE
									

                                                           ) d

                                                           left JOIN #TemplateBuying buying on buying.SAP_SALES_MAN_CODE = d.SAP_SALES_MAN_CODE and buying.STOCKIST_CODE = d.STOCKIST_CODE
									
									                        ) d on d.STOCKIST_CODE = das.DISTRIBUTOR_CODE and d.SAP_SALES_MAN_CODE = das.SALES_CODE
                                                            where d.MONTH = das.MONTH and d.YEAR = das.YEAR
				                        -----------------INSERT D_COVERAGE_BUYING_STORE_VISIT--------------

                                                        insert into D_COVERAGE_BUYING_STORE_VISIT([REGION]

                                                                , [DISTRIBUTOR_CODE]

                                                                , [DISTRIBUTOR_NAME]

                                                                , [SALES_CODE]

                                                                , [SALES_NAME]

                                                                , [YEAR]

                                                                , [MONTH]

                                                                , COVERAGE

                                                                , BUYING

                                                                , STORE_VISIT

                                                                )

                                                        select d.REGION_CODE, d.STOCKIST_CODE, d.STOCKIST_NAME, d.SAP_SALES_MAN_CODE, d.SALES_MAN_NAME, D.[YEAR], D.[MONTH], d.RL as RL, buying.C_VALUES as Buying, d.STORE_VISIT
                                                        from (
                                                            SELECT distinct d.REGION_CODE, d.STOCKIST_CODE, d.STOCKIST_NAME, d.SAP_SALES_MAN_CODE, d.SALES_MAN_NAME, D.[YEAR], D.[MONTH], p.C_VALUES as RL, v.COUNT_CUSTOMER as STORE_VISIT

                                                            FROM #D_MSS_DISTRIBUTION D
									                        left join #TMP_COUNT_STORE_VISIT v on d.SAP_SALES_MAN_CODE = v.SALES_CODE and d.STOCKIST_CODE = v.DISTRIBUTOR_CODE
									                        right join (select* from #TB_Performance where C_HEADER='01. RL') p on p.SAP_SALES_MAN_CODE = d.SAP_SALES_MAN_CODE and p.STOCKIST_CODE = d.STOCKIST_CODE
									

                                                            ) d

                                                            left JOIN #TemplateBuying buying on buying.SAP_SALES_MAN_CODE = d.SAP_SALES_MAN_CODE and buying.STOCKIST_CODE = d.STOCKIST_CODE
								                        WHERE NOT EXISTS(SELECT*
                                                            from D_COVERAGE_BUYING_STORE_VISIT das
                                                            join


                                                            (select d.REGION_CODE, d.STOCKIST_CODE, d.STOCKIST_NAME, d.SAP_SALES_MAN_CODE, d.SALES_MAN_NAME, D.[YEAR], D.[MONTH], d.RL as RL, buying.C_VALUES as Buying, d.STORE_VISIT
                                                            from (
                                                                SELECT distinct d.REGION_CODE, d.STOCKIST_CODE, d.STOCKIST_NAME, d.SAP_SALES_MAN_CODE, d.SALES_MAN_NAME, D.[YEAR], D.[MONTH], p.C_VALUES as RL, v.COUNT_CUSTOMER as STORE_VISIT
                                                                FROM #D_MSS_DISTRIBUTION D
										                        left join #TMP_COUNT_STORE_VISIT v on d.SAP_SALES_MAN_CODE = v.SALES_CODE and d.STOCKIST_CODE = v.DISTRIBUTOR_CODE
										                        join (select* from #TB_Performance where C_HEADER='01. RL') p on p.SAP_SALES_MAN_CODE = d.SAP_SALES_MAN_CODE and p.STOCKIST_CODE = d.STOCKIST_CODE
									

                                                                ) d

                                                                JOIN #TemplateBuying buying on buying.SAP_SALES_MAN_CODE = d.SAP_SALES_MAN_CODE and buying.STOCKIST_CODE = d.STOCKIST_CODE
									

                                                                ) d on d.STOCKIST_CODE = das.DISTRIBUTOR_CODE and d.SAP_SALES_MAN_CODE = das.SALES_CODE
									                        )

									                        update D_COVERAGE_BUYING_STORE_VISIT set STORE_VISIT = 0

                                                            where STORE_VISIT is null

                                                            update D_COVERAGE_BUYING_STORE_VISIT set BUYING = 0

                                                            where BUYING is null

                                                            update D_COVERAGE_BUYING_STORE_VISIT set COVERAGE = 0

                                                            where COVERAGE is null


                            select 1
                        ----------DONE
                        end


                      


                        ");
                        }

                        _TxtLog.Text = "Created Consolidate_D_Coverage_Buying_StoreVisit successfull!";


                    }
                    catch (Exception ex)
                    {
                        _TxtLog.Text = "Created Consolidate_D_Coverage_Buying_StoreVisit fail!";
                    }



                }
                if (kq.Equals("3"))
                {
                    try
                    {
                        dt = L5sSql.Query(sql2);
                        if (dt.Rows.Count == 0)
                        {
                            L5sSql.Execute(@"

                        CREATE proc [dbo].[Consolidate_D_MSS_DISTRIBUTION]

                        as
                        begin
					
						                        --TIME GONE			
						                        declare @sqlTITO2 NVARCHAR(MAX)
						                        declare @sqlTITO NVARCHAR(MAX)
						                        declare @sqlTimeGone NVARCHAR(MAX)
						                        declare @yyyy nvarchar(5)
						                        declare @NumberOfCurrent int
						                        declare @NumberOfSales int
						                        declare @NumberOfSunday int
						                        declare @DayCurrent int
						                        declare @TimeGone nvarchar(50)
						                        declare @NumberOfdays float
						                        set @yyyy = LEFT(CONVERT(nvarchar,getdate(),112),4)
						                        DECLARE @StartDate DATE = '20180101', @NumberOfYears INT = 10;
						                        -- prevent set or regional settings from interfering with 
						                        -- interpretation of dates / literals

						                        SET DATEFIRST 7;
						                        SET DATEFORMAT mdy;
						                        SET LANGUAGE US_ENGLISH;

						                        DECLARE @CutoffDate DATE = DATEADD(YEAR, @NumberOfYears, @StartDate);

						                        -- this is just a holding table for intermediate calculations:
						                        IF OBJECT_ID('tempdb..#dim') IS NOT NULL 
												                        BEGIN 
													                        DROP TABLE #dim
												                        END
						                        CREATE TABLE #dim
						                        (
						                        [date]       DATE PRIMARY KEY, 
						                        [day]        AS DATEPART(DAY,      [date]),
						                        [month]      AS DATEPART(MONTH,    [date]),
						                        FirstOfMonth AS CONVERT(DATE, DATEADD(MONTH, DATEDIFF(MONTH, 0, [date]), 0)),
						                        [MonthName]  AS DATENAME(MONTH,    [date]),
						                        [week]       AS DATEPART(WEEK,     [date]),
						                        [ISOweek]    AS DATEPART(ISO_WEEK, [date]),
						                        [DayOfWeek]  AS DATEPART(WEEKDAY,  [date]),
						                        [quarter]    AS DATEPART(QUARTER,  [date]),
						                        [year]       AS DATEPART(YEAR,     [date]),
						                        FirstOfYear  AS CONVERT(DATE, DATEADD(YEAR,  DATEDIFF(YEAR,  0, [date]), 0)),
						                        Style112     AS CONVERT(CHAR(8),   [date], 112),
						                        Style101     AS CONVERT(CHAR(10),  [date], 101)
						                        );
						                        -- use the catalog views to generate as many rows as we need

						                        INSERT #dim([date]) 
						                        SELECT d
						                        FROM
						                        (
	
						                        SELECT d = DATEADD(DAY, rn - 1, @StartDate)
						                        FROM 
						                        (

						                        SELECT TOP (DATEDIFF(DAY, @StartDate, @CutoffDate)) 
						                        rn = ROW_NUMBER() OVER (ORDER BY s1.[object_id])
						                        FROM sys.all_objects AS s1
						                        CROSS JOIN sys.all_objects AS s2
						                        -- on my system this would support > 5 million days
						                        ORDER BY s1.[object_id]
						                        ) AS x
						                        ) AS y;

						                        select @NumberOfCurrent =  DATEPART(DD,getdate()) -1
						                        select @DayCurrent =  DATEPART(DD,getdate())
						                        select @NumberOfSunday = count(*)
						                        --select *
						                        from #dim
						                        where [date] between getdate()-@DayCurrent and getdate() and DayOfWeek = 1
						                        --so ngay sales lam viec tru di chu nhat
						                        select @NumberOfSales = @NumberOfCurrent - @NumberOfSunday
						                        ---tong so ngay trong thang tru chu nhat
						                        select @NumberOfdays = convert(int,day(eomonth(getdate()))-4)
						                        --tinh time gone (so ngay di lam cua sale chia cho tong so ngay trong thang tru di chu nhat)
						                        select @TimeGone = cast( (@NumberOfSales/ @NumberOfdays) as decimal(18,5))
						                        --END TIME GONE

                                   	                        DECLARE @SUBFORMATDATE INT
                                    DECLARE @SUBFTODATE INT
                                    DECLARE @FORMDATE date
                                    DECLARE @groupType int
                                    DECLARE @TODATE date
                                    DECLARE @SPECIAL int
                                    DECLARE @RE_CODE nvarchar(20)
                                    DECLARE @RE_DESC  nvarchar(100)
                                    DECLARE @RE_CD nvarchar(20)
                                    SET @groupType =1
                                    SET @FORMDATE = getdate()
                                    SET @TODATE = getdate()
                                    SET @SUBFORMATDATE = CAST( SUBSTRING(convert(varchar, @FORMDATE, 112),0,LEN(Convert(varchar, @FORMDATE, 112))-1 ) AS INT)
                                    SET @SUBFTODATE = CAST( SUBSTRING(convert(varchar, @TODATE, 112),0,LEN(Convert(varchar, @TODATE, 112))-1 ) AS INT)

                                    IF OBJECT_ID('tempdb..#MSS_Distributor_Report') IS NOT NULL 
                                        BEGIN 
                                            DROP TABLE #MSS_Distributor_Report
                                        END


                                        Create table #MSS_Distributor_Report
                                        (
                                        _DISTRIBUTOR_CODE  nvarchar(50),
                                        _DISTRIBUTOR_NAME nvarchar(250),
                                        _SALES_CODE nvarchar(50),
                                        _SALES_NAME nvarchar(250),
                                        _C_HEADER nvarchar(50),
                                        _C_VALUES FLOAT,
                                        _RE_CODE nvarchar(50),
                                        )

	                                    IF OBJECT_ID('tempdb..#TemplateRouteList') IS NOT NULL 
                                        BEGIN 
                                            DROP TABLE #TemplateRouteList
                                        END
							

                                        Create table #TemplateRouteList
                                        (
                                        _DISTRIBUTOR_CODE  nvarchar(50),
                                        _DISTRIBUTOR_NAME nvarchar(250),
                                        _SALES_CODE nvarchar(50),
                                        _SALES_NAME nvarchar(250),
                                        _C_HEADER nvarchar(50),
                                        _C_VALUES FLOAT,
                                        _RE_CODE nvarchar(50),
                                        )
	           
                                        IF OBJECT_ID('tempdb..#TemplateGroupValue') IS NOT NULL 
                                        BEGIN 
                                            DROP TABLE #TemplateGroupValue
                                        END


                                        Create table #TemplateGroupValue
                                        (
                                        _DISTRIBUTOR_CODE  nvarchar(50),
                                        _DISTRIBUTOR_NAME nvarchar(250),
                                        _SALES_CODE nvarchar(50),
                                        _SALES_NAME nvarchar(250),
                                        _C_HEADER nvarchar(50),
                                        _C_VALUES FLOAT,
                                        _RE_CODE nvarchar(50)
                                        )
							
	            

	                                    IF OBJECT_ID('tempdb..#TMP_MSSValue') IS NOT NULL 
                                            BEGIN 
                                                DROP TABLE #TMP_MSSValue
                                            END

	                                    select MGRP.MSS_GROUP_CODE, MGRP.MSS_GROUP_DESCRIPTION
							                                    ,GRE.MSS_RE_CD,MRE.RE_CODE as SUB_RE ,MRE.RE_DESCRIPTION as SUB_RE_DESCRIPTION
							                                    ,OPG.PRODUCT_CD,OPRO.PRODUCT_CODE
							                                    ,OPG.SPECIAL, OPG.COMBO,OPG.PERCENT_DISTRIBUTION
							                                    ,MGRP.MIN_PIECE into #TMP_MSSValue
						                                    from M_MSS_GROUP_TYPE GTYPE
						                                    join M_MSS_GROUP MGRP on MGRP.MSS_GROUP_TYPE_CD = GTYPE.MSS_GROUP_TYPE_CD and MGRP.ACTIVE =1
						                                    join O_MSS_GROUP_RE GRE on GRE.MSS_GROUP_CD = MGRP.MSS_GROUP_CD and GRE.ACTIVE =1
						                                    join M_MSS_RE MRE on MRE.MSS_RE_CD = GRE.MSS_RE_CD and MRE.ACTIVE =1
						                                    join O_MSS_PRODUCT_GROUP OPG on OPG.MSS_GROUP_CD =  MGRP.MSS_GROUP_CD and OPG.ACTIVE =1
						                                    join O_PRODUCT OPRO on OPRO.PRODUCT_CD = OPG.PRODUCT_CD and OPRO.ACTIVE =1
						                                    where GTYPE.MSS_GROUP_TYPE_CD = @groupType -- Chọn Group Type
							                                    and  @TODATE between MGRP.START_DATE and MGRP.END_DATE -- thời gian group 
							                                    --and MRE.MSS_RE_CD IN ({1})


	                                    IF OBJECT_ID('tempdb..#TMP_MSS') IS NOT NULL 
                                            BEGIN 
                                                DROP TABLE #TMP_MSS
                                            END
	                                    select GRP.PRODUCT_CD,GRP.MSS_GROUP_CD
                                                ,RE.MSS_RE_CD
                                                ,MRE.RE_CODE ,MRE.RE_DESCRIPTION,PARENT_RE_CD
                                                ,GRP.COMBO ,GRP.PERCENT_DISTRIBUTION ,GRP.SPECIAL,PRODUCT_CODE,MSS_GROUP_DESCRIPTION,MSS_GROUP_CODE
                                                ,MGRP.MIN_PIECE into #TMP_MSS
                                                from O_MSS_PRODUCT_GROUP GRP 
                                                join O_MSS_GROUP_RE  RE on RE.MSS_GROUP_CD = GRP.MSS_GROUP_CD and RE.ACTIVE =1
                                                join M_MSS_RE MRE on MRE.MSS_RE_CD = RE.MSS_RE_CD and MRE.ACTIVE =1
                                                join M_MSS_GROUP MGRP on MGRP.MSS_GROUP_CD = GRP.MSS_GROUP_CD and MGRP.ACTIVE =1
			                                    join O_PRODUCT OPRO on OPRO.PRODUCT_CD = GRP.PRODUCT_CD and OPRO.ACTIVE =1
                                                --where  MRE.PARENT_RE_CD in (Select MSS_RE_CD From M_MSS_RE where PARENT_RE_CD is null) -- thay đổi RE------DOI QUA MSS--
                                                where  GRP.ACTIVE =1 and (@TODATE between MGRP.START_DATE and MGRP.END_DATE)
					
	                                    ---tmp Distributor

	                                    IF OBJECT_ID('tempdb..#Distributor') IS NOT NULL
		                                    DROP TABLE #Distributor

	                                    select DISTRIBUTOR_CD,DISTRIBUTOR_CODE,DISTRIBUTOR_NAME,ACTIVE INTO #Distributor
	                                    from [M_DISTRIBUTOR.]
	                                    WHERE ACTIVE = 1 
	                                    --and DISTRIBUTOR_CD in({4})
							
	                                    ------tmp SH_SI

	                                    IF OBJECT_ID('tempdb..#TMP_SH') IS NOT NULL
		                                    DROP TABLE #TMP_SH
		                                    SELECT SH.DISTRIBUTOR_CODE,SH.DISTRIBUTOR_CODE_CD,SH.SAP_SALESMAN_CODE
			                                    ,SH.SAP_SALESMAN_CODE_CD,SI.KZWI1,SH.BILL_DATE_TIME,sh.RE
			                                    ,SH.SAP_CUST_CODE,SH.SAP_CUST_CODE_CD,SI.PRODUCT_CODE,SH.SUB_RE,SH.SUB_RE_CD ,SI.SALES_IN_A_UNIT,sh.LIST_PRICE,NET_VALUES INTO #TMP_SH
		                                    FROM TH_SALESH_DATA SH 
		                                    JOIN TH_SALESI_DATA SI ON SH.INVOICE_NO = SI.INVOICE_NO AND SH.ACTIVE = 1
		                                    join (select * from #Distributor ) dis on dis.DISTRIBUTOR_CD = sh.DISTRIBUTOR_CODE_CD
		                                    WHERE SI.ACTIVE = 1 and CAST( SUBSTRING(convert(varchar, BILL_DATE_TIME, 112),0,LEN(Convert(varchar, BILL_DATE_TIME, 112))-1 ) AS INT) between @SUBFORMATDATE and @SUBFTODATE 
		                                   -- and SUB_RE_CD in ({1})
                                        --and   LIST_PRICE > 0 


         
		                                    -------------
	                                        IF OBJECT_ID('tempdb..#TMP_SH_NET_VALUES') IS NOT NULL
										                        DROP TABLE #TMP_SH_NET_VALUES
										                        select * into #TMP_SH_NET_VALUES
										                        from #TMP_SH 
										                        where NET_VALUES > 0 
                                              IF OBJECT_ID('tempdb..#TMP_SH_Buying_Sum') IS NOT NULL
						                        DROP TABLE #TMP_SH_Buying_Sum
						                        select * into #TMP_SH_Buying_Sum
						                        from (

						                        select  DISTRIBUTOR_CODE,DISTRIBUTOR_CODE_CD,SAP_SALESMAN_CODE,RE
						                        ,SAP_SALESMAN_CODE_CD,SAP_CUST_CODE,sum(NET_VALUES) as NET_VALUES 
						                        from #TMP_SH 
						                        group by  DISTRIBUTOR_CODE,DISTRIBUTOR_CODE_CD,SAP_SALESMAN_CODE,RE
						                        ,SAP_SALESMAN_CODE_CD,SAP_CUST_CODE
						                        ) t
						                        where NET_VALUES > 0
	           
			
	                                    ---Rout List 
		                                    insert into  #TemplateRouteList
			                                    select distinct dis.DISTRIBUTOR_CODE,dis.DISTRIBUTOR_NAME,
			                                    sale.SALES_CODE,SALE.SALES_NAME
			                                    ,'Route List' as C_HEADER
			                                    ,COUNT(*) as C_VALUES	
				                                    , 1 as _ORDER_COLUMN
		                                    from M_SALES sale
		                                    join O_SALES_ROUTE osr on osr.SALES_CD = sale.SALES_CD and osr.ACTIVE =1
		                                    join O_CUSTOMER_ROUTE ocr on ocr.ROUTE_CD = osr.ROUTE_CD and ocr.ACTIVE =1
		                                    join M_CUSTOMER cust on cust.CUSTOMER_CD = ocr.CUSTOMER_CD and cust.ACTIVE =1
		                                    join #Distributor dis on dis.DISTRIBUTOR_CD = sale.DISTRIBUTOR_CD and dis.ACTIVE =1
		                                    JOIN  M_MSS_RE MRE on MRE.RE_CODE = CUST.CUSTOMER_CHAIN_CODE AND MRE.ACTIVE = 1
		                                    where sale.ACTIVE= 1  
					                        --and mre.MSS_RE_CD in ({1}) -- dung du lieu voi HH khi ko co dieu kien mss_Re_Cd
                                            -- and dis.DISTRIBUTOR_CD in ({4})
		                                    group by dis.DISTRIBUTOR_CODE,dis.DISTRIBUTOR_NAME,
				                                    sale.SALES_CODE,SALE.SALES_NAME

	            

                                            -------GROUP Values_point
                
                                           insert  #TemplateGroupValue

					                        select 
			                                t.DISTRIBUTOR_CODE,t.DISTRIBUTOR_NAME
			                                ,t.SALES_CODE,t.SALES_NAME
			                                ,t.MSS_GROUP_DESCRIPTION  as C_HEADER
			                                ,COUNT(*) as C_VALUES
				                            , RE

			                            from (

					                        select MSS.RE_CODE ,SH.SAP_SALESMAN_CODE,sale.SALES_CODE,sale.SALES_NAME
					                        ,mss.PARENT_RE_CD,dis.DISTRIBUTOR_CODE,dis.DISTRIBUTOR_NAME,
							                        SH.SAP_CUST_CODE,MSS_GROUP_DESCRIPTION,COUNT(prod.PRODUCT_CODE) as counta,
							                        SUM(Case when MSS.COMBO = 1 then (SH.NET_VALUES * MSS.PERCENT_DISTRIBUTION)/100 ELSE SH.NET_VALUES end) as Vales
							                        ,SH.RE
					                        from #TMP_MSS MSS
					                        join O_PRODUCT prod on prod.PRODUCT_CD = MSS.PRODUCT_CD and prod.ACTIVE = 1
					                        join (
						                        SELECT SAP_SALESMAN_CODE,SAP_CUST_CODE,SUB_RE,PRODUCT_CODE,DISTRIBUTOR_CODE_CD,SAP_SALESMAN_CODE_CD,
						                        SUM(SALES_IN_A_UNIT) AS SALES_IN_A_UNIT 
						                        ,SUM(NET_VALUES) AS NET_VALUES,_TSH.RE
						                        FROM #TMP_SH _tsh
						                        GROUP BY   SAP_SALESMAN_CODE,SAP_CUST_CODE,SUB_RE,PRODUCT_CODE,DISTRIBUTOR_CODE_CD,SAP_SALESMAN_CODE_CD,_TSH.RE
					                        ) SH on SH.PRODUCT_CODE = prod.PRODUCT_CODE and SH.SUB_RE =MSS.RE_CODE
					                        join [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sh.DISTRIBUTOR_CODE_CD and dis.ACTIVE = 1
					                        join M_SALES sale on sale.SALES_CD = sh.SAP_SALESMAN_CODE_CD and sale.ACTIVE = 1
					                        where SH.SALES_IN_A_UNIT>=MSS.MIN_PIECE	
					                        group by MSS.RE_CODE ,SH.SAP_SALESMAN_CODE,SH.SAP_CUST_CODE,PARENT_RE_CD,dis.DISTRIBUTOR_CODE,dis.DISTRIBUTOR_NAME,sale.SALES_CODE,sale.SALES_NAME,MSS_GROUP_DESCRIPTION,SH.RE
					                        HAVING SUM(Case when MSS.COMBO = 1 then (SH.NET_VALUES * MSS.PERCENT_DISTRIBUTION)/100 ELSE SH.NET_VALUES end)>0
				                        ) t
		                                    group by t.DISTRIBUTOR_CODE,t.DISTRIBUTOR_NAME
				                                    ,t.SALES_CODE,t.SALES_NAME,t.PARENT_RE_CD,t.MSS_GROUP_DESCRIPTION,RE
					
				                                    --END  Count Customer by Sale --
		  
					

		                                    --select end----
		                           --         select *
		                           --         from #MSS_Distributor_Report
		                           --        -- WHERE _C_HEADER='Avg Point/RL'
					                        ----AND _SALES_CODE = 'TH37S00014'
                             --               ORDER BY _C_HEADER,_SALES_CODE 
									
									                        --insert #TB_SummaryReport

									                        IF OBJECT_ID ('tempdb..#D_MSS_DISTRIBUTION') IS NOT NULL
										                        DROP TABLE #D_MSS_DISTRIBUTION

									                        select  reg.REGION_CODE,per.[_DISTRIBUTOR_CODE],per.[_DISTRIBUTOR_NAME]
									                        ,per.[_SALES_CODE],per.[_SALES_NAME],PER._RE_CODE as RE_CODE
									                        ,(select RE_DESCRIPTION from M_MSS_RE WHERE ACTIVE = 1 AND RE_CODE = PER._RE_CODE ) AS RE_DESCRIPTION,MONTH(GETDATE()) as [MONTH],YEAR(GETDATE()) AS [YEAR],round(per.[_C_VALUES],3) as [C_VALUES] INTO #D_MSS_DISTRIBUTION
									                        from (select _DISTRIBUTOR_CODE,_DISTRIBUTOR_NAME,_SALES_CODE
									                        ,_SALES_NAME,'MSS Total' as _C_HEADER, sum(_C_VALUES) AS _C_VALUES,v._RE_CODE
									                        from #TemplateGroupValue v
									                        GROUP BY _DISTRIBUTOR_CODE,_DISTRIBUTOR_NAME,_SALES_CODE,_SALES_NAME,v._RE_CODE
									                        ) per 
									                        JOIN [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CODE = per._DISTRIBUTOR_CODE and dis.ACTIVE = 1
									                        join M_COMMUNE com on com.COMMUNE_CD = dis.COMMUNE_CD and com.ACTIVE = 1
									                        join M_DISTRICT disc on disc.DISTRICT_CD = com.DISTRICT_CD and disc.ACTIVE = 1
									                        join M_PROVINCE pro on pro.PROVINCE_CD = disc.PROVINCE_CD and pro.ACTIVE= 1
									                        join M_AREA are on are.AREA_CD = pro.AREA_CD and are.ACTIVE = 1
									                        join M_REGION reg on reg.REGION_CD = are.REGION_CD and reg.ACTIVE = 1
                                  

								                           --DROP TABLE BEFORE WHEN INSERT
								                        --TRUNCATE TABLE D_MSS_DISTRIBUTION
								

								                        update mss set mss.POINT_RL = d.C_VALUES,mss.TIME_GONE = cast( cast(@TimeGone as numeric(18,4))* 100 as numeric(18,2))
								                        from  #D_MSS_DISTRIBUTION d
								                        join D_MSS_DISTRIBUTION mss on d._SALES_CODE = mss.SALES_CODE and d._DISTRIBUTOR_CODE = mss.DISTRIBUTOR_CODE
								                        where mss.[MONTH] = d.[MONTH] and mss.[YEAR] = d.[YEAR]

								                           --INSERT D_MSS_DISTRIBUTION
								                        insert into D_MSS_DISTRIBUTION([REGION]
										                        ,[DISTRIBUTOR_CODE]
										                        ,[DISTRIBUTOR_NAME]
										                        ,[SALES_CODE]
										                        ,[SALES_NAME]
										                        ,[RE_CODE]
										                        ,[RE_DESCRIPTION]
										                        ,[MONTH]
										                        ,[YEAR]
										                        ,[Point_RL]
										                        ,[TIME_GONE]
										                        )
									                        SELECT d.REGION_CODE,d._DISTRIBUTOR_CODE,d._DISTRIBUTOR_NAME,d._SALES_CODE,d._SALES_NAME,d.RE_CODE,d.RE_DESCRIPTION,[MONTH],[YEAR],cast(D.C_VALUES as numeric(18,3)) as C_VALUES,cast( cast(@TimeGone as numeric(18,4))* 100 as numeric(18,2))
									                        FROM #D_MSS_DISTRIBUTION D
									                        where not exists(
												                        select *
												                        from  #D_MSS_DISTRIBUTION D
												                        join D_MSS_DISTRIBUTION mss on d._SALES_CODE = mss.SALES_CODE and d._DISTRIBUTOR_CODE = mss.DISTRIBUTOR_CODE
												                        where mss.[MONTH] = d.[MONTH] and mss.[YEAR] = d.[YEAR]
											                        )

					                        select 1					
                        ----------DONE
                        end 




                        ");
                        }
                        _TxtLog.Text = "Created Consolidate_D_MSS_DISTRIBUTION successfull!";
                    }
                    catch
                    {
                        _TxtLog.Text = "Created Consolidate_D_MSS_DISTRIBUTION fail!";
                    }
                }
                if (kq.Equals("4"))
                {
                    try
                    {
                        dt = L5sSql.Query(sql3);
                        if (dt.Rows.Count.ToString() == "0")
                        {
                            L5sSql.Execute(@"
                            create proc [dbo].[Consolidate_MTD_TO_D_MTD_ACT_TARGET]

                            as
                            begin
					
	
                                                               /****** Script for SelectTopNRows command from SSMS  ******/

                            --create template table
                            IF OBJECT_ID ('tempdb..#TMP_MTD') is not null
	                            drop table #TMP_MTD

                            SELECT 
                                  [REGION]
                                  ,[NAME]
                                  ,[DSR_NAME]
                                  ,[TARGET]
                                  ,[ACTUAL]
	                              ,year(CREATED_DATE) as [YEAR]
	                              ,case
		                             when month(CREATED_DATE) between 1 and 3 then 'Q1'
		                             when month(CREATED_DATE) between 4 and 6 then 'Q2'
		                             when month(CREATED_DATE) between 7 and 9 then 'Q3'
		                             when month(CREATED_DATE) between 10 and 12 then 'Q4'
		                             end as [QUARTER]
	                             , CASE 
		                            WHEN month(CREATED_DATE) = 1 then 'Jan'
		                            WHEN month(CREATED_DATE) = 2 then 'Feb'
		                            WHEN month(CREATED_DATE) = 3 then 'Mar'
		                            WHEN month(CREATED_DATE) = 4 then 'Apr'
		                            WHEN month(CREATED_DATE) = 5 then 'May'
		                            WHEN month(CREATED_DATE) = 6 then 'Jun'
		                            WHEN month(CREATED_DATE) = 7 then 'Jul'
		                            WHEN month(CREATED_DATE) = 8 then 'Aug'
		                            WHEN month(CREATED_DATE) = 9 then 'Sep'
		                            WHEN month(CREATED_DATE) = 10 then 'Oct'
		                            WHEN month(CREATED_DATE) = 11 then 'Nov'
		                            WHEN month(CREATED_DATE) = 12 then 'Dec'
		                            END AS [MONTH]
	                               ,month(CREATED_DATE) AS [MONTH_NUMBER] into #TMP_MTD

                              FROM [dbo].[D_MTD_SECONDATY_SALES_UPDATE]

                              --update if exists
                               update act set act.ACTUAL = mtd.ACTUAL
                               ,act.TARGET = mtd.TARGET
                              from #TMP_MTD mtd 
                              join [D_MTD_ACT_TARGET] act on act.DISTRIBUTOR = mtd.NAME and act.REGION = mtd.REGION  AND ACT.DSR = mtd.DSR_NAME
                              where mtd.MONTH_NUMBER = act.MONTH_NUMBER and mtd.YEAR = act.YEAR	

                              --insert if not exists
                            insert into [dbo].[D_MTD_ACT_TARGET]([REGION]
                                  ,[DISTRIBUTOR]
                                  ,[DSR]
                                  ,[TARGET]
                                  ,[ACTUAL]
                                  ,[YEAR]
                                  ,[QUARTER]
                                  ,[MONTH]
                                  ,[MONTH_NUMBER])
                              SELECT *
                              FROM #TMP_MTD
                              where not exists (
                              select *
                              from #TMP_MTD mtd 
                              join [D_MTD_ACT_TARGET] act on act.DISTRIBUTOR = mtd.NAME and act.REGION = mtd.REGION
                              where mtd.MONTH_NUMBER = act.MONTH_NUMBER and mtd.YEAR = act.YEAR	
                              )


  
                              ------

  
				
	                            select 1
                            ----------DONE
                            end");
                        }

                        _TxtLog.Text = "Created Consolidate_MTD_TO_D_MTD_ACT_TARGET successfull!";

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _TxtLog.Text = "Created Consolidate_MTD_TO_D_MTD_ACT_TARGET fail!";
                    }
                }

                string sql5 = @"--------O_SALES_MSS_SUMMARY------------
                                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                                            WHERE TABLE_NAME = 'O_SALES_MSS_SUMMARY')
                                BEGIN
	                                PRINT('')
                                END
                                ELSE
                                BEGIN
	                                CREATE TABLE [dbo].[O_SALES_MSS_SUMMARY](
		                                [SALES_MSS_SUMMARY_CD] [bigint] IDENTITY(1,1) NOT NULL,
		                                [SALES_CD] [bigint] NULL,
		                                [SALES_CODE] [nvarchar](15) NULL,
		                                [YYYYMM] [nchar](10) NULL,
		                                [ROUTE_LIST] [int] NULL,
		                                [BUYING] [int] NULL,
		                                [CREATED_DATE] [datetime] NULL CONSTRAINT [DF_O_SALES_MSS_SUMMARY_CREATED_DATE]  DEFAULT (getdate()),
		                                [UPDATE_DATE] [datetime] NULL,
		                                [ACTIVE] [bit] NULL CONSTRAINT [DF_O_SALES_MSS_SUMMARY_ACTIVE]  DEFAULT ((1))
	                                ) ON [PRIMARY]
                                END

                                --------O_CUSTOMER_MSS_VALUE------------
                                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                                            WHERE TABLE_NAME = 'O_CUSTOMER_MSS_VALUE')
                                BEGIN
	                                PRINT('')
                                END
                                ELSE
                                BEGIN
	                                CREATE TABLE [dbo].[O_CUSTOMER_MSS_VALUE](
		                                [CUSTOMER_MSS_VALUE_CD] [bigint] IDENTITY(1,1) NOT NULL,
		                                [SALE_CD] [bigint] NULL,
		                                [SALE_CODE] [nvarchar](15) NULL,
		                                [CUSTOMER_CD] [bigint] NULL,
		                                [CUSTOMER_CODE] [nvarchar](15) NULL,
		                                [RE] [nvarchar](15) NULL,
		                                [SUB_RE] [nvarchar](15) NULL,
		                                [YYYYMM] [nchar](10) NULL,
		                                [MSS_GROUP_CD] [bigint] NULL,
		                                [MSS_GROUP_CODE] [nvarchar](15) NULL,
		                                [MSS_GROUP_TYPE_CD] [bigint] NULL,
		                                [MSS_POINT] [int] NULL,
		                                [MSS_VALUE] [float] NULL,
		                                [CREATED_DATE] [datetime] NULL CONSTRAINT [DF_O_CUSTOMER_MSS_VALUE_CREATED_DATE]  DEFAULT (getdate()),
		                                [UPDATE_DATE] [datetime] NULL,
		                                [ACTIVE] [bit] NULL CONSTRAINT [DF_O_CUSTOMER_MSS_VALUE_ACTIVE]  DEFAULT ((1))
	                                ) ON [PRIMARY]
                                END

                                --------O_CUSTOMER_ORTHER_VALUE------------
                                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                                            WHERE TABLE_NAME = 'O_CUSTOMER_ORTHER_VALUE')
                                BEGIN
	                                PRINT('')
                                END
                                ELSE
                                BEGIN
	                                CREATE TABLE [dbo].[O_CUSTOMER_ORTHER_VALUE](
		                                [CUSTOMER_ORTHER_VALUE_CD] [bigint] IDENTITY(1,1) NOT NULL,
		                                [SALE_CD] [bigint] NULL,
		                                [SALE_CODE] [nvarchar](15) NULL,
		                                [CUSTOMER_CD] [bigint] NULL,
		                                [CUSTOMER_CODE] [nvarchar](15) NULL,
		                                [RE] [nvarchar](15) NULL,
		                                [SUB_RE] [nvarchar](15) NULL,
		                                [YYYYMM] [nchar](10) NULL,
		                                [ORTHER_VALUE] [float] NULL,
		                                [CREATED_DATE] [datetime] NULL CONSTRAINT [DF_O_CUSTOMER_ORTHER_VALUE_CREATED_DATE]  DEFAULT (getdate()),
		                                [UPDATE_DATE] [datetime] NULL,
		                                [ACTIVE] [bit] NULL CONSTRAINT [DF_O_CUSTOMER_ORTHER_VALUE_ACTIVE]  DEFAULT ((1))
	                                ) ON [PRIMARY]
                                END";

                if (kq.Equals("5"))
                {
                    try
                    {
                        dt = L5sSql.Query(sql5);
                        _TxtLog.Text = ("Create temp table reports MSS successfull!");
                    }
                    catch (Exception ex)
                    {
                        _TxtLog.Text = "Create temp table reports MSS fail!";

                    }
                }
                else
                {
                    _TxtLog.Text = "\nIP invalid! " + ipAddress;
                }

                string sql6 = @"SELECT *
                            FROM INFORMATION_SCHEMA.ROUTINES
                            WHERE ROUTINE_TYPE = 'PROCEDURE' and SPECIFIC_NAME = 'CONSOLIDATE_REPORTS_MSS'";

                if (kq.Equals("6"))
                {
                    try
                    {
                        dt = L5sSql.Query(sql6);
                        if (dt.Rows.Count.ToString() == "0")
                        {
                            L5sSql.Execute(@"
                           CREATE PROC [dbo].[CONSOLIDATE_REPORTS_MSS] @DATE DATETIME
                           AS
                           BEGIN
	                             --GET DATE
	                            DECLARE @GETDATE datetime
	                            SET @GETDATE=DATEADD(DAY,-1,CONVERT(DATE,@DATE))
	                            print 'Consolidate date: '+ CONVERT(nchar,@GETDATE)
	                            --GET YYYYMM
	                            DECLARE @YYYYMM nchar(10)
	                            SET @YYYYMM=LEFT(CONVERT(nchar,@GETDATE,112),6)

	                            IF OBJECT_ID('tempdb..#TMP_SH') IS NOT NULL
	                            DROP TABLE #TMP_SH
	                            SELECT SH.DISTRIBUTOR_CODE,SH.DISTRIBUTOR_CODE_CD,SH.SAP_SALESMAN_CODE
		                            ,SH.SAP_SALESMAN_CODE_CD,SI.KZWI1,SH.BILL_DATE_TIME,LEFT(CONVERT(nchar(10),BILL_DATE_TIME,112),6) AS YYYYMM,RE ,SH.INVOICE_NO
		                            ,SH.SAP_CUST_CODE,SH.SAP_CUST_CODE_CD,SI.PRODUCT_CODE,SH.SUB_RE,SH.SUB_RE_CD ,SI.SALES_IN_A_UNIT,NET_VALUES 
		                            INTO #TMP_SH
	                            FROM TH_SALESH_DATA SH 
	                            JOIN TH_SALESI_DATA SI ON SH.INVOICE_NO = SI.INVOICE_NO AND SH.ACTIVE = 1
	                            WHERE SI.ACTIVE = 1 
	                            and MONTH(BILL_DATE_TIME) =MONTH(@GETDATE) 
	                            and YEAR(BILL_DATE_TIME) = YEAR(@GETDATE)
	                            ----------------------------------O_SALES_MSS_SUMMARY-------------------------------------
	                            --BUYING
	                            IF OBJECT_ID('tempdb..#TMP_SH_Buying_Sum') IS NOT NULL
	                            DROP TABLE #TMP_SH_Buying_Sum
	                            select * into #TMP_SH_Buying_Sum
	                            from (
		                            select  DISTRIBUTOR_CODE,DISTRIBUTOR_CODE_CD,SAP_SALESMAN_CODE,RE
		                            ,SAP_SALESMAN_CODE_CD,SAP_CUST_CODE,sum(NET_VALUES) as NET_VALUES 
		                            from #TMP_SH 
		                            group by  DISTRIBUTOR_CODE,DISTRIBUTOR_CODE_CD,SAP_SALESMAN_CODE,RE
		                            ,SAP_SALESMAN_CODE_CD,SAP_CUST_CODE
	                            ) t
	                            where NET_VALUES > 0

	                            IF OBJECT_ID('tempdb..#TMP_BUYING') IS NOT NULL
	                            DROP TABLE #TMP_BUYING
	                            select sale.SALES_CD
		                            ,COUNT(TBuying.SAP_CUST_CODE) as BUYING	INTO #TMP_BUYING	
	                            from M_SALES sale  
	                            join [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sale.DISTRIBUTOR_CD
		                            join --LEFT JOIN
		                            ( 
			                            select DISTRIBUTOR_CODE,DISTRIBUTOR_CODE_CD,SAP_SALESMAN_CODE,SAP_SALESMAN_CODE_CD,SAP_CUST_CODE
			                            from #TMP_SH_Buying_Sum
		                            ) TBuying on TBuying.SAP_SALESMAN_CODE_CD = sale.SALES_CD and dis.DISTRIBUTOR_CD = TBuying.DISTRIBUTOR_CODE_CD
	                            group by sale.SALES_CD

	                            IF OBJECT_ID('tempdb..#SUMMARY') IS NOT NULL DROP TABLE #SUMMARY
                                  
	                            SELECT _route_list.*,_buying.BUYING INTO #SUMMARY FROM (
			                            SELECT DISTINCT sale.SALES_CD,sale.SALES_CODE,@YYYYMM AS YYYYMM
				                            ,COUNT(*) as ROUTE_LIST
			                            FROM M_SALES sale
			                            JOIN O_SALES_ROUTE osr ON osr.SALES_CD = sale.SALES_CD and osr.ACTIVE =1
			                            JOIN O_CUSTOMER_ROUTE ocr ON ocr.ROUTE_CD = osr.ROUTE_CD AND ocr.ACTIVE =1
			                            JOIN M_CUSTOMER cust ON cust.CUSTOMER_CD = ocr.CUSTOMER_CD AND cust.ACTIVE =1
			                            JOIN [M_DISTRIBUTOR.] dis ON dis.DISTRIBUTOR_CD = sale.DISTRIBUTOR_CD AND dis.ACTIVE =1
			                            JOIN  M_MSS_RE MRE ON MRE.RE_CODE = CUST.CUSTOMER_CHAIN_CODE AND MRE.ACTIVE = 1
			                            WHERE sale.ACTIVE=1
			                            GROUP BY sale.SALES_CD,sale.SALES_CODE
		                            ) _route_list
		                            LEFT JOIN #TMP_BUYING _buying ON _route_list.SALES_CD=_buying.SALES_CD
		                            ORDER BY YYYYMM,SALES_CODE



	                            --UPDATE  O_SALES_MSS_SUMMARY
	                            UPDATE 	O_SALES_MSS_SUMMARY   SET ROUTE_LIST=_summary.ROUTE_LIST,BUYING=_summary.BUYING,UPDATE_DATE=GETDATE()
	                            FROM O_SALES_MSS_SUMMARY  _mss
	                            JOIN #SUMMARY _summary ON _mss.SALES_CD=_summary.SALES_CD and _mss.YYYYMM=_summary.YYYYMM

	                            --INSERT O_SALES_MSS_SUMMARY
	                            INSERT INTO O_SALES_MSS_SUMMARY (SALES_CD,SALES_CODE,YYYYMM,ROUTE_LIST,BUYING)
	                            SELECT * 
	                            FROM #SUMMARY _summary
	                            WHERE NOT EXISTS (
		                            SELECT * FROM  O_SALES_MSS_SUMMARY _tmp WHERE _tmp.SALES_CD=_summary.SALES_CD and _tmp.YYYYMM=_summary.YYYYMM
	                            )


	                            ----------------------------------O_CUSTOMER_MSS_VALUE-------------------------------------
	                            if OBJECT_ID('tempdb..#TMP_MSS') IS NOT NULL
	                            DROP TABLE #TMP_MSS

	                            select GRP.PRODUCT_CD,PRODUCT_CODE,GRP.MSS_GROUP_CD,MSS_GROUP_CODE
		                            ,RE.MSS_RE_CD
		                            ,MRE.RE_CODE ,MRE.RE_DESCRIPTION,MGRP.MSS_GROUP_DESCRIPTION
		                            ,GRP.COMBO ,GRP.PERCENT_DISTRIBUTION ,GRP.SPECIAL,MRE.PARENT_RE_CD,MGRP.MSS_GROUP_TYPE_CD
		                            ,MGRP.MIN_PIECE INTO #TMP_MSS
	                            from O_MSS_PRODUCT_GROUP GRP 
	                            join O_MSS_GROUP_RE  RE on RE.MSS_GROUP_CD = GRP.MSS_GROUP_CD and RE.ACTIVE =1
	                            join M_MSS_RE MRE on MRE.MSS_RE_CD = RE.MSS_RE_CD and MRE.ACTIVE =1
	                            join M_MSS_GROUP MGRP on MGRP.MSS_GROUP_CD = GRP.MSS_GROUP_CD and MGRP.ACTIVE =1
	                            join O_PRODUCT pro on pro.PRODUCT_CD = grp.PRODUCT_CD and pro.ACTIVE = 1
	                            where GRP.ACTIVE =1 and (@GETDATE between MGRP.START_DATE and MGRP.END_DATE)


	                            if OBJECT_ID('tempdb..#SH_MSS') IS NOT NULL
	                            DROP TABLE #SH_MSS
	                            SELECT	SAP_SALESMAN_CODE_CD,SAP_SALESMAN_CODE,SAP_CUST_CODE_CD,SAP_CUST_CODE,RE,SUB_RE,
			                            LEFT(CONVERT(nchar(10),BILL_DATE_TIME,112),6) AS YYYYMM,MSS_GROUP_CD,MSS_GROUP_CODE,
			                            _sh.PRODUCT_CODE,SALES_IN_A_UNIT,NET_VALUES,COMBO,MIN_PIECE,PERCENT_DISTRIBUTION,MSS_GROUP_TYPE_CD	INTO #SH_MSS
	                            FROM #TMP_SH _sh
	                            JOIN #TMP_MSS _mss ON _sh.PRODUCT_CODE=_mss.PRODUCT_CODE and _sh.SUB_RE_CD=_mss.MSS_RE_CD
	                            WHERE MONTH(BILL_DATE_TIME)=MONTH(@GETDATE) AND YEAR(BILL_DATE_TIME)=YEAR(BILL_DATE_TIME)

	                            --Tính MSS POINT và MSS_VALUE cho group
	                            IF OBJECT_ID('tempdb..#CUSTOMER_VALUE') IS NOT NULL
	                            DROP TABLE #CUSTOMER_VALUE
	                            SELECT SAP_SALESMAN_CODE_CD,SAP_SALESMAN_CODE,SAP_CUST_CODE_CD,SAP_CUST_CODE,RE,SUB_RE,YYYYMM,MSS_GROUP_CD,MSS_GROUP_CODE,MSS_GROUP_TYPE_CD,CASE WHEN SUM(CASE WHEN SALES_IN_A_UNIT>= MIN_PIECE THEN 1 ELSE 0 END)=0 THEN 0 ELSE 1  END AS MSS_POINT,SUM(NET_VALUES) AS MSS_VALUES INTO #CUSTOMER_VALUE
	                            FROM (
		                            SELECT  SAP_SALESMAN_CODE_CD,SAP_SALESMAN_CODE,SAP_CUST_CODE_CD,SAP_CUST_CODE,RE,SUB_RE,YYYYMM,MSS_GROUP_CD,MSS_GROUP_CODE,PRODUCT_CODE,MIN_PIECE,MSS_GROUP_TYPE_CD,SUM(SALES_IN_A_UNIT) AS  SALES_IN_A_UNIT ,SUM(CASE WHEN COMBO=1 THEN (NET_VALUES*PERCENT_DISTRIBUTION)/100 ELSE NET_VALUES END) AS NET_VALUES
		                            FROM #SH_MSS
		                            GROUP BY  SAP_SALESMAN_CODE_CD,SAP_SALESMAN_CODE,SAP_CUST_CODE_CD,SAP_CUST_CODE,RE,SUB_RE,YYYYMM,MSS_GROUP_CD,MSS_GROUP_CODE,PRODUCT_CODE,MIN_PIECE,MSS_GROUP_TYPE_CD
	                            ) _cust_value
	                            GROUP BY SAP_SALESMAN_CODE_CD,SAP_SALESMAN_CODE,SAP_CUST_CODE_CD,SAP_CUST_CODE,RE,SUB_RE,YYYYMM,MSS_GROUP_CD,MSS_GROUP_CODE,MSS_GROUP_TYPE_CD


	                            --UPDATE O_CUSTOMER_MSS_VALUE
	                            UPDATE 	O_CUSTOMER_MSS_VALUE SET MSS_POINT= _cust_value.MSS_POINT, MSS_VALUE=_cust_value.MSS_VALUES	,UPDATE_DATE=GETDATE() ,MSS_GROUP_TYPE_CD=_cust_value.MSS_GROUP_TYPE_CD
	                            FROM O_CUSTOMER_MSS_VALUE _tmp
	                            JOIN #CUSTOMER_VALUE _cust_value ON _tmp.CUSTOMER_CD=_cust_value.SAP_CUST_CODE_CD and _tmp.SALE_CD=_cust_value.SAP_SALESMAN_CODE_CD and _tmp.YYYYMM=_cust_value.YYYYMM and _tmp.MSS_GROUP_CD=_cust_value.MSS_GROUP_CD
								  
	                            --INSERT O_CUSTOMER_MSS_VALUE
	                            INSERT INTO  O_CUSTOMER_MSS_VALUE([SALE_CD],[SALE_CODE],[CUSTOMER_CD],[CUSTOMER_CODE],[RE],[SUB_RE],[YYYYMM],[MSS_GROUP_CD],[MSS_GROUP_CODE],MSS_GROUP_TYPE_CD,[MSS_POINT],[MSS_VALUE])
	                            SELECT * FROM #CUSTOMER_VALUE  _cust_value
	                            WHERE NOT EXISTS (
		                            SELECT * FROM  O_CUSTOMER_MSS_VALUE _tmp WHERE _tmp.CUSTOMER_CD=_cust_value.SAP_CUST_CODE_CD and _tmp.SALE_CD=_cust_value.SAP_SALESMAN_CODE_CD and _tmp.YYYYMM=_cust_value.YYYYMM and _tmp.MSS_GROUP_CD=_cust_value.MSS_GROUP_CD
	                            )



	                            ----------------------------------O_CUSTOMER_ORTHER_VALUE-------------------------------------

	                            IF OBJECT_ID('tempdb..#SHI_MSS') IS NOT NULL DROP TABLE #SHI_MSS
	                            SELECT _sh.SAP_CUST_CODE,_sh.SAP_SALESMAN_CODE,_sh.INVOICE_NO,LEFT(CONVERT(nchar(10),BILL_DATE_TIME,112),6) AS YYYYMM,_sh.SUB_RE_CD,_mss.MSS_GROUP_CD,_mss.MSS_GROUP_DESCRIPTION,_mss.RE_CODE,_mss.RE_DESCRIPTION,_mss.PRODUCT_CD,_mss.PRODUCT_CODE,_mss.COMBO,_mss.PERCENT_DISTRIBUTION,NET_VALUES,SALES_IN_A_UNIT,_mss.MIN_PIECE INTO #SHI_MSS
	                            FROM #TMP_SH _sh
	                            JOIN #TMP_MSS _mss on _mss.PRODUCT_CODE=_sh.PRODUCT_CODE and _mss.MSS_RE_CD=_sh.SUB_RE_CD


	                            IF OBJECT_ID('tempdb..#ORTHER_VALUE') IS NOT NULL DROP TABLE #ORTHER_VALUE
	                            SELECT SAP_SALESMAN_CODE_CD,SAP_SALESMAN_CODE,SAP_CUST_CODE_CD,SAP_CUST_CODE,RE,SUB_RE,YYYYMM,SUM(NET_VALUES) AS ORTHER_VALUE INTO #ORTHER_VALUE
	                            FROM #TMP_SH _sh
	                            WHERE NOT EXISTS (
		                            SELECT * 
		                            FROM #SHI_MSS _shmss
		                            WHERE _sh.INVOICE_NO=_shmss.INVOICE_NO and _sh.PRODUCT_CODE=_shmss.PRODUCT_CODE
	                            )
	                            GROUP BY  SAP_CUST_CODE_CD,SAP_CUST_CODE,SAP_SALESMAN_CODE_CD,SAP_SALESMAN_CODE,RE,SUB_RE,YYYYMM

	                            --UPDATE O_CUSTOMER_ORTHER_VALUE
	                            UPDATE 	O_CUSTOMER_ORTHER_VALUE SET ORTHER_VALUE=_tmp.ORTHER_VALUE, UPDATE_DATE=GETDATE()
	                            FROM O_CUSTOMER_ORTHER_VALUE _orther
	                            JOIN #ORTHER_VALUE _tmp ON _orther.CUSTOMER_CD=_tmp.SAP_CUST_CODE_CD  and _orther.SALE_CD=_tmp.SAP_SALESMAN_CODE_CD and _orther.YYYYMM=_tmp.YYYYMM

	                            --INSERT O_CUSTOMER_ORTHER_VALUE
	                            INSERT INTO  O_CUSTOMER_ORTHER_VALUE([SALE_CD],[SALE_CODE],[CUSTOMER_CD],[CUSTOMER_CODE],[RE],[SUB_RE],[YYYYMM],[ORTHER_VALUE])
	                            SELECT * 
	                            FROM #ORTHER_VALUE  _tmp
	                            WHERE NOT EXISTS (
		                            SELECT * FROM  O_CUSTOMER_ORTHER_VALUE _orther WHERE _orther.CUSTOMER_CD=_tmp.SAP_CUST_CODE_CD  and _orther.SALE_CD=_tmp.SAP_SALESMAN_CODE_CD and _orther.YYYYMM=_tmp.YYYYMM
	                            )

                            END
  
                            ");
                        }

                        _TxtLog.Text = "Created CONSOLIDATE_REPORTS_MSS successfull!";

                    }
                    catch (Exception ex)
                    {
                        _TxtLog.Text = "Created CONSOLIDATE_REPORTS_MSS fail!";
                    }
                }
            }
        }
    }

}