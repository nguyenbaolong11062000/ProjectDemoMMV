USE [MMC_CPTH]
GO

/****** Object:  Table [dbo].[TMP_MTD_PRIMARY_UPDATE_TH]    Script Date: 10/10/2020 11:08:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TMP_MTD_PRIMARY_UPDATE_TH](
	[TEST_AUTO_CD] [bigint] IDENTITY(1,1) NOT NULL,
	[STK_CD] [nvarchar](50) NULL,
	[INV_NO] [nvarchar](50) NULL,
	[INV_DATE] [nvarchar](50) NULL,
	[SKU_CD] [nvarchar](50) NULL,
	[SKU_DESC] [nvarchar](512) NULL,
	[LINE_NO] [nvarchar](50) NULL,
	[DASH_CODE] [nvarchar](50) NULL,
	[INV_RATE] [nvarchar](50) NULL,
	[NET_INV_VAL] [nvarchar](50) NULL,
	[TAX] [nvarchar](50) NULL,
	[PURCH_VAL] [nvarchar](50) NULL,
	[INV_QTY] [nvarchar](50) NULL,
	[UOM_REC] [nvarchar](50) NULL,
	[INVOICE_VALUE] [nvarchar](50) NULL,
	[NET_INV_VAL_1] [nvarchar](50) NULL,
	[INVOICE_TYPE] [nvarchar](50) NULL,
	[UOM_REC_NO_CPK] [nvarchar](50) NULL,
	[CASE_FACTOR_PC] [nvarchar](50) NULL,
	[MRP_DOZ] [nvarchar](50) NULL,
	[NET_WEIGHT] [nvarchar](50) NULL,
	[FIELD21] [nvarchar](50) NULL,
	[FIELD22] [nvarchar](50) NULL,
	[FIELD23] [nvarchar](50) NULL,
	[CATEGORY] [nvarchar](50) NULL,
	[CATEGORY_DES] [nvarchar](50) NULL,
	[SUB_CATE] [nvarchar](50) NULL,
	[SUB_CATE_DES] [nvarchar](50) NULL,
	[SIZE] [nvarchar](50) NULL,
	[SUB_BRAND] [nvarchar](50) NULL,
	[SUB_BRAND_DES] [nvarchar](255) NULL,
	[VARIANT] [nvarchar](50) NULL,
	[VARIANT_DES] [nvarchar](255) NULL,
	[FIELD33] [nvarchar](50) NULL,
	[FIELD34] [nvarchar](50) NULL,
	[FIELD35] [nvarchar](50) NULL,
	[FIELD36] [nvarchar](50) NULL,
	[TMP_MTD_PRIMARY_UPDATE_CD] [nvarchar](255) NULL
) ON [PRIMARY]

GO


