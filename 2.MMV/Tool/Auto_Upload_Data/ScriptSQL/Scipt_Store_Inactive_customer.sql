USE [MMV]
GO

/****** Object:  StoredProcedure [dbo].[INACTIVE_CUSTOMER]    Script Date: 7/23/2019 3:50:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INACTIVE_CUSTOMER]
    @RESULT varchar(50),
	@maxautonumber int
AS
BEGIN
    IF @RESULT = 'SELECT'
	begin
					declare @temp table
			(
				[CUSTOMER_CD] [bigint] NULL,
				[CUSTOMER_CODE] [nvarchar](50) NULL,
				[CUSTOMER_NAME] [nvarchar](512) NULL,
				[CUSTOMER_ADDRESS] [nvarchar](max) NULL,
				[CUSTOMER_TYPE_CODE] [nvarchar](50) NULL,
				[CUSTOMER_CHAIN_CODE] [nvarchar](50) NULL,
				[CUSTOMER_STATUS] [nvarchar](50) NULL,
				[CUSTOMER_DISPLAY] [nvarchar](50) NULL,
				[DISTRIBUTOR_CD] [bigint] NULL,
				[PROVINCE_CD] [bigint] NULL,
				[DISTRICT_CD] [bigint] NULL,
				[COMMUNE_CD] [bigint] NULL,
				[ACTIVE] [bit] NULL,
				[CREATED_DATE] [datetime] NULL,
				[AMS] [money] NULL,
				[LONGITUDE_LATITUDE] [nvarchar](128) NULL,
				[LONGITUDE_LATITUDE_ACCURACY] [float] NULL,
				[IS_PDA] [bit] NULL,
				[UPDATED_DATE] [datetime] NULL,
				[CUSTOMER_LOCATION_ADDRESS] [nvarchar](512) NULL,
				[CREATED_USER] [nvarchar](20) NULL,
				[SYS_CREATED_DATE] [datetime] NULL,
				[PHONE_NUMBER] [nvarchar](50) NULL,
				[IDMS_CUSTOMER_CODE] [nvarchar](50) NULL,
				[GLOBAL_RE] [varchar](50) NULL
			)
			insert @temp ([CUSTOMER_CD]
				  ,[CUSTOMER_CODE]
				  ,[CUSTOMER_NAME]
				  ,[CUSTOMER_ADDRESS]
				  ,[CUSTOMER_TYPE_CODE]
				  ,[CUSTOMER_CHAIN_CODE]
				  ,[CUSTOMER_STATUS]
				  ,[CUSTOMER_DISPLAY]
				  ,[DISTRIBUTOR_CD]
				  ,[PROVINCE_CD]
				  ,[DISTRICT_CD]
				  ,[COMMUNE_CD]
				  ,[ACTIVE]
				  ,[CREATED_DATE]
				  ,[AMS]
				  ,[LONGITUDE_LATITUDE]
				  ,[LONGITUDE_LATITUDE_ACCURACY]
				  ,[IS_PDA]
				  ,[UPDATED_DATE]
				  ,[CUSTOMER_LOCATION_ADDRESS]
				  ,[CREATED_USER]
				  ,[SYS_CREATED_DATE]
				  ,[PHONE_NUMBER]
				  ,[IDMS_CUSTOMER_CODE]
				  ,[GLOBAL_RE])
			select *
			from MMV.dbo.M_CUSTOMER
			where CUSTOMER_CODE not in
			(
			select CUSTOMER_CODE
			from TMP_IMPORT_CUSTOMER
			)
			and CUSTOMER_CODE not in
			(
			select cust.CUSTOMER_CODE 
				from MMV.dbo.M_CUSTOMER cust
				join O_CUSTOMER_ROUTE ocr on cust.CUSTOMER_CD = ocr.CUSTOMER_CD 
				join O_SALES_ROUTE osr on osr.ROUTE_CD = ocr.ROUTE_CD
				join M_ROUTE rout on rout.ROUTE_CD = osr.ROUTE_CD
				join M_SALES sls on sls.SALES_CD = osr.SALES_CD
				join [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
				where dis.DISTRIBUTOR_CODE in ('109001','109002','900001') 
			)
			and ACTIVE = 1 and CUSTOMER_CODE not like N'%TEST%'

			select * from @temp where convert(int,right(CUSTOMER_CODE, len(CUSTOMER_CODE)- 3)) < @maxautonumber
		end
	ELSE IF @RESULT = 'UPDATE'
	begin
		declare @temp1 table
			(
				[CUSTOMER_CD] [bigint] NULL,
				[CUSTOMER_CODE] [nvarchar](50) NULL,
				[CUSTOMER_NAME] [nvarchar](512) NULL,
				[CUSTOMER_ADDRESS] [nvarchar](max) NULL,
				[CUSTOMER_TYPE_CODE] [nvarchar](50) NULL,
				[CUSTOMER_CHAIN_CODE] [nvarchar](50) NULL,
				[CUSTOMER_STATUS] [nvarchar](50) NULL,
				[CUSTOMER_DISPLAY] [nvarchar](50) NULL,
				[DISTRIBUTOR_CD] [bigint] NULL,
				[PROVINCE_CD] [bigint] NULL,
				[DISTRICT_CD] [bigint] NULL,
				[COMMUNE_CD] [bigint] NULL,
				[ACTIVE] [bit] NULL,
				[CREATED_DATE] [datetime] NULL,
				[AMS] [money] NULL,
				[LONGITUDE_LATITUDE] [nvarchar](128) NULL,
				[LONGITUDE_LATITUDE_ACCURACY] [float] NULL,
				[IS_PDA] [bit] NULL,
				[UPDATED_DATE] [datetime] NULL,
				[CUSTOMER_LOCATION_ADDRESS] [nvarchar](512) NULL,
				[CREATED_USER] [nvarchar](20) NULL,
				[SYS_CREATED_DATE] [datetime] NULL,
				[PHONE_NUMBER] [nvarchar](50) NULL,
				[IDMS_CUSTOMER_CODE] [nvarchar](50) NULL,
				[GLOBAL_RE] [varchar](50) NULL
			)
			insert @temp1 ([CUSTOMER_CD]
				  ,[CUSTOMER_CODE]
				  ,[CUSTOMER_NAME]
				  ,[CUSTOMER_ADDRESS]
				  ,[CUSTOMER_TYPE_CODE]
				  ,[CUSTOMER_CHAIN_CODE]
				  ,[CUSTOMER_STATUS]
				  ,[CUSTOMER_DISPLAY]
				  ,[DISTRIBUTOR_CD]
				  ,[PROVINCE_CD]
				  ,[DISTRICT_CD]
				  ,[COMMUNE_CD]
				  ,[ACTIVE]
				  ,[CREATED_DATE]
				  ,[AMS]
				  ,[LONGITUDE_LATITUDE]
				  ,[LONGITUDE_LATITUDE_ACCURACY]
				  ,[IS_PDA]
				  ,[UPDATED_DATE]
				  ,[CUSTOMER_LOCATION_ADDRESS]
				  ,[CREATED_USER]
				  ,[SYS_CREATED_DATE]
				  ,[PHONE_NUMBER]
				  ,[IDMS_CUSTOMER_CODE]
				  ,[GLOBAL_RE])
			select *
			from MMV.dbo.M_CUSTOMER
			where CUSTOMER_CODE not in
			(
			select CUSTOMER_CODE
			from TMP_IMPORT_CUSTOMER
			)
			and CUSTOMER_CODE not in
			(
			select cust.CUSTOMER_CODE 
				from MMV.dbo.M_CUSTOMER cust
				join O_CUSTOMER_ROUTE ocr on cust.CUSTOMER_CD = ocr.CUSTOMER_CD 
				join O_SALES_ROUTE osr on osr.ROUTE_CD = ocr.ROUTE_CD
				join M_ROUTE rout on rout.ROUTE_CD = osr.ROUTE_CD
				join M_SALES sls on sls.SALES_CD = osr.SALES_CD
				join [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
				where dis.DISTRIBUTOR_CODE in ('109001','109002','900001') 
			)
			and ACTIVE = 1 and CUSTOMER_CODE not like N'%TEST%'

			update cus
			set cus.ACTIVE = 0
			from @temp1 tmp
			join M_CUSTOMER cus on cus.CUSTOMER_CD = tmp.CUSTOMER_CD
			where convert(int,right(tmp.CUSTOMER_CODE, len(tmp.CUSTOMER_CODE)- 3)) < @maxautonumber
end
END
GO


