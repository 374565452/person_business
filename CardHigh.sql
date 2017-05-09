USE [WaterMonitorSystemDB2]
GO

/****** Object:  Table [dbo].[CardClear]    Script Date: 05/09/2017 16:52:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CardHigh](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SerialNumber] [nvarchar](50) NOT NULL,
	[OpenUserId] [bigint] NOT NULL,
	[OpenUserName] [nvarchar](50) NOT NULL,
	[OpenAddress] [nvarchar](200) NOT NULL,
	[OpenTime] [datetime] NOT NULL,
 CONSTRAINT [PK_CardHigh] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IC卡序列号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHigh', @level2type=N'COLUMN',@level2name=N'SerialNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开卡操作人id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHigh', @level2type=N'COLUMN',@level2name=N'OpenUserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开卡操作人名字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHigh', @level2type=N'COLUMN',@level2name=N'OpenUserName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开卡地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHigh', @level2type=N'COLUMN',@level2name=N'OpenAddress'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开卡时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHigh', @level2type=N'COLUMN',@level2name=N'OpenTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'清除卡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CardHigh'
GO

