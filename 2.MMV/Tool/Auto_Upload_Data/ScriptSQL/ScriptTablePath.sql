USE [MMV]
GO

/****** Object:  Table [dbo].[S_PATH_UPLOAD]    Script Date: 4/13/2019 9:12:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[S_PATH_UPLOAD](
	[CD_PATH] [bigint] IDENTITY(1,1) NOT NULL,
	[PATH_NAME] [nvarchar](255) NULL,
	[PATH_FILE] [nvarchar](max) NULL,
	[PATH_BACKUP_FILE] [nvarchar](100) NULL,
	[CREATED_DATE] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[S_PATH_UPLOAD] ADD  CONSTRAINT [DF_PATH_UPLOAD_DAILYSALES_PERFORMANCE_CREATED_DATE]  DEFAULT (getdate()) FOR [CREATED_DATE]
GO


