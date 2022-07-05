USE [MMV]
GO

/****** Object:  Table [dbo].[TMP_D_MTD_PRIMARY]    Script Date: 4/11/2019 7:36:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TMP_D_MTD_PRIMARY](
	[TMP_CD] [bigint] IDENTITY(1,1) NOT NULL,
	[REGION] [nvarchar](255) NULL,
	[AREA] [nvarchar](50) NULL,
	[PROVINCE] [nvarchar](50) NULL,
	[DIST] [nvarchar](50) NULL,
	[NAME] [nvarchar](512) NULL,
	[CDS] [nvarchar](255) NULL,
	[TRGET] [float] NULL,
	[ACTUAL] [float] NULL,
	[ACHIEVED] [float] NULL,
	[TIME_GONE] [float] NULL,
	[ACTIVE] [bit] NULL,
	[CREATED_DATE] [datetime] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TMP_D_MTD_PRIMARY] ADD  CONSTRAINT [DF_TMP_D_MTD_PRIMARY_ACTIVE]  DEFAULT ((1)) FOR [ACTIVE]
GO

ALTER TABLE [dbo].[TMP_D_MTD_PRIMARY] ADD  CONSTRAINT [DF_TMP_D_MTD_PRIMARY_CREATED_DATE]  DEFAULT (getdate()) FOR [CREATED_DATE]
GO


