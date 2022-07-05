USE [MMC_CPTH]
GO

/****** Object:  StoredProcedure [dbo].[IMPORT_DAILY_MTD_PRIMARY_TH]    Script Date: 09/12/2020 9:09:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[IMPORT_DAILY_MTD_PRIMARY_TH]
-- Khởi tạo tham số viewstate CD từ bảng TMP để truyền vào 2 lệnh insert và update bên dưới 
@ViewState nvarchar(max)
AS
BEGIN
		declare @NumberOfCurrent int = DATEPART(DD,getdate()) -1
		declare @NumberOfSales int
		declare @NumberOfSunday int
		declare @DayCurrent int = DATEPART(DD,getdate())
		declare @TimeGone nvarchar(50)
		---Tổng số ngày trong tháng trừ chủ nhật
		declare @NumberOfdays float = convert(int,day(eomonth(getdate()))-4)
		declare @StartDate DATE = getdate()
		-- Lấy ngày đầu tiên của tháng
		declare @FirstDayMonth DATE = CONVERT(nvarchar,DATEADD(dd,-(DAY(@StartDate)-1),@StartDate),101)
		-- Lấy đúng tên tháng hiện tại
		declare @NameMonth char(10)
		select @NameMonth = LEFT(DATENAME(MONTH,INV_DATE),3) from TMP_MTD_PRIMARY_UPDATE_TH

		IF OBJECT_ID('tempdb..#dim') IS NOT NULL BEGIN DROP TABLE #dim END
		CREATE TABLE #dim
		(	[date]       DATE PRIMARY KEY,
			[DayOfWeek]  AS DATEPART(WEEK,  [date] )
		);
		-- Lấy từ ngày đầu tiên của tháng đên (ngày hiện tại trừ đi 1)
		-- VD: @StartDate là 7/10/2020 thì time gone sẽ là ngày 1 -> 6 vì ngày 7/10 DSR chưa bán xong hoặc chưa kết thúc
		INSERT #dim([date]) SELECT d FROM
		(
		  SELECT d = DATEADD(DAY, rn - 1, @FirstDayMonth)
		  FROM 
		  (
			SELECT TOP (DATEDIFF(DAY, @FirstDayMonth, @StartDate)) 
			  rn = ROW_NUMBER() OVER (ORDER BY s1.[object_id])
			FROM sys.all_objects AS s1
			CROSS JOIN sys.all_objects AS s2 ORDER BY s1.[object_id]
		  ) AS x
		) AS y;

		select @NumberOfSunday = count(*) from #dim where [date] between getdate()- @DayCurrent and getdate() and DayOfWeek = 1
		--Số ngày sales làm việc trừ đi chủ nhật
		set @NumberOfSales = @NumberOfCurrent - @NumberOfSunday
		--Tính TimeGone (số ngày đi làm của sales chia cho tổng số ngày trong tháng trừ đi chủ nhật)
		set @TimeGone = cast( (@NumberOfSales/ @NumberOfdays) as decimal(18,5))

		-- Sum giá trị ACTUAL và TRGET trước khi select bảng cuối để đảm bảo giá trị sum theo DITRIBUTOR_CODE, Ngày Xuất không sai số

		select STK_CD, sum(CONVERT(float,INVOICE_VALUE)) as ACTUAL, LEFT(INV_DATE,6) as INV_DATE, 
			SUBSTRING(INV_DATE,1,4) as YEAR, TMP_MTD_PRIMARY_UPDATE_CD into #tmpMTD
			from TMP_MTD_PRIMARY_UPDATE_TH 
			group by STK_CD, INV_DATE, TMP_MTD_PRIMARY_UPDATE_CD

		select DISTRIBUTOR_CODE, sum(CONVERT(float,TOTAL_SALES_TARGET)) as TRGET, SALES_TARGET_YYYYMM into #tmpTarget 
			from D_SALES_TARGET_PRI
			where SALES_TARGET_YYYYMM in (select distinct LEFT(INV_DATE,6) from TMP_MTD_PRIMARY_UPDATE_TH)
			group by DISTRIBUTOR_CODE,SALES_TARGET_YYYYMM
			 
		-- Left join các bảng để bắt được trường hợp DISTRIBUTOR_CODE chưa tồn tại trên hệ thống

		select  ISNULL(region.REGION_CODE,'') as REGION
				,ISNULL(area.AREA_CODE,'') as AREA
				,ISNULL(pro.PROVINCE_CODE,'') as PROVINCE
				,a.STK_CD as DIST
				,dis.DISTRIBUTOR_NAME as NAME
				,bb.SUPERVISOR_NAME as CDS
				,a.ACTUAL as ACTUAL
				,tmpTar.TRGET as TRGET
				,@TimeGone AS TIME_GONE
				,@NameMonth as MONTH
				,SUBSTRING(a.INV_DATE,1,4) as YEAR
				,a.TMP_MTD_PRIMARY_UPDATE_CD
				into #tmpFinal
		from #tmpMTD a
		left join #tmpTarget tmpTar on tmpTar.DISTRIBUTOR_CODE = a.STK_CD and tmpTar.SALES_TARGET_YYYYMM = LEFT(a.INV_DATE,6) 
		left join [M_DISTRIBUTOR.] dis on a.STK_CD = dis.DISTRIBUTOR_CODE and dis.ACTIVE = 1
		left join M_COMMUNE com on dis.COMMUNE_CD = com.COMMUNE_CD and com.ACTIVE = 1
		left join M_DISTRICT dist on dist.DISTRICT_CD = com.DISTRICT_CD and dist.ACTIVE = 1
		left join M_PROVINCE pro  on pro.PROVINCE_CD = dist.PROVINCE_CD and pro.ACTIVE = 1
		left join M_AREA_PROVINCE map on  map.PROVINCE_CD = pro.PROVINCE_CD and map.ACTIVE = 1
		left join M_AREA area on area.AREA_CD = map.AREA_CD and area.ACTIVE = 1
		left join M_REGION region on region.REGION_CD = area.REGION_CD and region.ACTIVE = 1
		left join O_SUPERVISOR_DISTRIBUTOR aa on aa.DISTRIBUTOR_CD = dis.DISTRIBUTOR_CD and aa.ACTIVE = 1
		left join M_SUPERVISOR bb on bb.SUPERVISOR_CD = aa.SUPERVISOR_CD and bb.ACTIVE = 1
		group by region.REGION_CODE,area.AREA_CODE,pro.PROVINCE_CODE,a.STK_CD,
					dis.DISTRIBUTOR_NAME,bb.SUPERVISOR_NAME,a.INV_DATE,a.ACTUAL,tmpTar.TRGET,a.TMP_MTD_PRIMARY_UPDATE_CD
		
		-- Nếu DISTRIBUTOR CODE chưa tồn tại trên hệ thống thì insert vào bảng chính

		Insert into D_MTD_PRIMARY_UPDATE
		(REGION,AREA,PROVINCE,DIST,NAME,CDS,TRGET,ACTUAL,ACHIEVED,TIME_GONE,MONTH,YEAR,CREATED_DATE)

		select REGION,AREA,PROVINCE,DIST,NAME,CDS,TRGET,ACTUAL
				,cast(((ACTUAL/TRGET) * 100) as numeric(18,2)) AS ACHIEVED
				,TIME_GONE,MONTH,YEAR,GETDATE() AS CREATED_DATE from #tmpFinal a
		where not EXISTS(
			select DIST,MONTH,YEAR from D_MTD_PRIMARY_UPDATE b
			where b.DIST = a.DIST and b.MONTH = a.MONTH and b.YEAR = a.YEAR
		) and a.TMP_MTD_PRIMARY_UPDATE_CD = @ViewState

		-- Nếu DISTRIBUTOR CODE có tồn tại thì update lại ACTUAL dựa trên DIS CODE, MONTH và YEAR

		UPDATE D_MTD_PRIMARY_UPDATE 
		SET ACTUAL = tmtdp.ACTUAL
		FROM D_MTD_PRIMARY_UPDATE mtdp
		LEFT JOIN #tmpFinal tmtdp on tmtdp.DIST = mtdp.DIST
		and tmtdp.MONTH = mtdp.MONTH
		and tmtdp.YEAR = mtdp.YEAR
		Where tmtdp.TMP_MTD_PRIMARY_UPDATE_CD = @ViewState

		select 1
END

GO


