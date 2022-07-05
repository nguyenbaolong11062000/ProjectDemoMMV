USE [MMV]
GO

/****** Object:  Table [dbo].[TMP_IMPORT_MTD_SECONDARY]    Script Date: 4/11/2019 7:36:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TMP_IMPORT_MTD_SECONDARY](
	[TMP_CD] [bigint] IDENTITY(1,1) NOT NULL,
	[REGION] [nvarchar](255) NULL,
	[AREA] [nvarchar](50) NULL,
	[PROVINCE] [nvarchar](50) NULL,
	[DIST] [nvarchar](50) NULL,
	[NAME] [nvarchar](512) NULL,
	[CDS] [nvarchar](255) NULL,
	[DSR_CODE] [nvarchar](50) NULL,
	[DSR_NAME] [nvarchar](512) NULL,
	[AM] [nvarchar](255) NULL,
	[RM] [nvarchar](255) NULL,
	[VISITED] [float] NULL,
	[TARGET] [float] NULL,
	[ACTUAL] [float] NULL,
	[ACHIEVED] [float] NULL,
	[TIME_GONE] [float] NULL,
	[RL] [int] NULL,
	[BUYING_STORES] [int] NULL,
	[ECC] [float] NULL,
	[ACTIVE] [bit] NULL,
	[CREATED_DATE] [datetime] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TMP_IMPORT_MTD_SECONDARY] ADD  DEFAULT ((1)) FOR [ACTIVE]
GO

ALTER TABLE [dbo].[TMP_IMPORT_MTD_SECONDARY] ADD  CONSTRAINT [DF_R_TMP_IMPORT_MTD_SECONDARY_CREATED_DATE]  DEFAULT (getdate()) FOR [CREATED_DATE]
GO


