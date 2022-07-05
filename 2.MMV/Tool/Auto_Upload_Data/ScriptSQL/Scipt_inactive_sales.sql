USE [MMV]
GO

/****** Object:  StoredProcedure [dbo].[INACTIVE_SALES]    Script Date: 7/23/2019 5:21:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[INACTIVE_SALES]
    @RESULT varchar(50)
AS
BEGIN
    IF @RESULT = 'SELECT'
		select *
		from MMV.dbo.M_SALES

		where SALES_CODE not in
		(
		select SALES_CODE
		from TEMP_UPLOAD_CUSTOMER
		)
		and SALES_CODE not in
		(
		select sls.SALES_CODE 
		  from MMV.dbo.M_CUSTOMER cust
		  join O_CUSTOMER_ROUTE ocr on cust.CUSTOMER_CD = ocr.CUSTOMER_CD 
		  join O_SALES_ROUTE osr on osr.ROUTE_CD = ocr.ROUTE_CD
		  join M_ROUTE rout on rout.ROUTE_CD = osr.ROUTE_CD
		  join M_SALES sls on sls.SALES_CD = osr.SALES_CD
		  join [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
		  where dis.DISTRIBUTOR_CODE in ('109001','109002','900001') 
		)
		and ACTIVE = 1 and DISTRIBUTOR_CD not in ('40966','41002','41003') 

	ELSE IF @RESULT = 'UPDATE'

			update MMV.dbo.M_SALES
			set ACTIVE = 0

			where SALES_CODE not in
			(
			select SALES_CODE
			from TEMP_UPLOAD_CUSTOMER
			)
			and SALES_CODE not in
			(
			select sls.SALES_CODE 
			  from MMV.dbo.M_CUSTOMER cust
			  join O_CUSTOMER_ROUTE ocr on cust.CUSTOMER_CD = ocr.CUSTOMER_CD 
			  join O_SALES_ROUTE osr on osr.ROUTE_CD = ocr.ROUTE_CD
			  join M_ROUTE rout on rout.ROUTE_CD = osr.ROUTE_CD
			  join M_SALES sls on sls.SALES_CD = osr.SALES_CD
			  join [M_DISTRIBUTOR.] dis on dis.DISTRIBUTOR_CD = sls.DISTRIBUTOR_CD
			  where dis.DISTRIBUTOR_CODE in ('109001','109002','900001') 
			)
			and ACTIVE = 1 and DISTRIBUTOR_CD not in ('40966','41002','41003') 


END


GO


