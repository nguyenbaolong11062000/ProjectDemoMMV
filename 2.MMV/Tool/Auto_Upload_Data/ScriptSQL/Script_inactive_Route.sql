USE [MMV]
GO

/****** Object:  StoredProcedure [dbo].[INACTIVE_ROUTE]    Script Date: 7/23/2019 5:21:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[INACTIVE_ROUTE]
    @RESULT varchar(50)
AS
BEGIN
    IF @RESULT = 'SELECT'
		select *
		from MMV.dbo.M_ROUTE
		where ROUTE_CODE not in
		(
		select ROUTE_CODE
		from TEMP_UPLOAD_CUSTOMER
		)
		and ROUTE_CODE not in
		(
		select rout.ROUTE_CODE 
		  from MMV.dbo.M_CUSTOMER cust
		  join O_CUSTOMER_ROUTE ocr on cust.CUSTOMER_CD = ocr.CUSTOMER_CD 
		  join O_SALES_ROUTE osr on osr.ROUTE_CD = ocr.ROUTE_CD
		  join M_ROUTE rout on rout.ROUTE_CD = osr.ROUTE_CD
		  join M_SALES sls on sls.SALES_CD = osr.SALES_CD
		  join [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
		  where dis.DISTRIBUTOR_CODE in ('109001','109002','900001') 
		)
		and ACTIVE = 1 and DISTRIBUTOR_CD not in ('40966','41002','41003') and ROUTE_NAME not like '%create%'

	ELSE IF @RESULT = 'UPDATE'
		update MMV.dbo.M_ROUTE
		set ACTIVE = 0
		where ROUTE_CODE not in
		(
		select ROUTE_CODE
		from TEMP_UPLOAD_CUSTOMER
		)
		and ROUTE_CODE not in
		(
		select rout.ROUTE_CODE 
		  from MMV.dbo.M_CUSTOMER cust
		  join O_CUSTOMER_ROUTE ocr on cust.CUSTOMER_CD = ocr.CUSTOMER_CD 
		  join O_SALES_ROUTE osr on osr.ROUTE_CD = ocr.ROUTE_CD
		  join M_ROUTE rout on rout.ROUTE_CD = osr.ROUTE_CD
		  join M_SALES sls on sls.SALES_CD = osr.SALES_CD
		  join [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
		  where dis.DISTRIBUTOR_CODE in ('109001','109002','900001') 
		)
		and ACTIVE = 1 and DISTRIBUTOR_CD not in ('40966','41002','41003') and ROUTE_NAME not like '%create%'

END


GO


