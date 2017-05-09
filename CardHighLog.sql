USE [WaterMonitorSystemDB2]
GO

/****** Object:  Table [dbo].[CardClearLog]    Script Date: 05/09/2017 16:51:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CardHighLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SerialNumber] [nvarchar](50) NOT NULL,
	[LogUserId] [bigint] NOT NULL,
	[LogUserName] [nvarchar](50) NOT NULL,
	[LogAddress] [nvarchar](200) NOT NULL,
	[LogTime] [datetime] NOT NULL,
	[LogType] [nvarchar](50) NOT NULL,
	[LogContent] [nvarchar](max) NULL,
 CONSTRAINT [PK_CardHighLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IC卡序列号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHighLog', @level2type=N'COLUMN',@level2name=N'SerialNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHighLog', @level2type=N'COLUMN',@level2name=N'LogUserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人名字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHighLog', @level2type=N'COLUMN',@level2name=N'LogUserName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHighLog', @level2type=N'COLUMN',@level2name=N'LogAddress'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHighLog', @level2type=N'COLUMN',@level2name=N'LogTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHighLog', @level2type=N'COLUMN',@level2name=N'LogType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHighLog', @level2type=N'COLUMN',@level2name=N'LogContent'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'清除卡操作日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHighLog'
GO

