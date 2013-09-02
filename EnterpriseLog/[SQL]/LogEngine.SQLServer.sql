USE [LogEngine]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 01/19/2012 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[TagID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_LogTag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleSet]    Script Date: 01/19/2012 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleSet](
	[RuleSetID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ApplyOnReceive] [bit] NOT NULL,
	[ApplyOnCleanUp] [bit] NOT NULL,
 CONSTRAINT [PK_RuleSet] PRIMARY KEY CLUSTERED 
(
	[RuleSetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Level]    Script Date: 01/19/2012 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Level](
	[LevelID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Icon] [nvarchar](50) NULL,
 CONSTRAINT [PK_LogLevel] PRIMARY KEY CLUSTERED 
(
	[LevelID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogEntry]    Script Date: 01/19/2012 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogEntry](
	[LogEntryID] [bigint] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Content] [text] NULL,
	[LevelID] [int] NOT NULL,
	[Trash] [bit] NOT NULL,
 CONSTRAINT [PK_LogEntry] PRIMARY KEY CLUSTERED 
(
	[LogEntryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagValue]    Script Date: 01/19/2012 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TagValue](
	[TagValueID] [bigint] IDENTITY(1,1) NOT NULL,
	[TagID] [bigint] NOT NULL,
	[Value] [varchar](max) NOT NULL,
 CONSTRAINT [PK_TagValue] PRIMARY KEY CLUSTERED 
(
	[TagValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 01/19/2012 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rule](
	[RuleID] [int] IDENTITY(1,1) NOT NULL,
	[RuleSetID] [int] NOT NULL,
	[Expression] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[RuleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogEntryTagValue]    Script Date: 01/19/2012 16:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogEntryTagValue](
	[LogEntryID] [bigint] NOT NULL,
	[TagValueID] [bigint] NOT NULL,
 CONSTRAINT [PK_LogEntryTagValue] PRIMARY KEY CLUSTERED 
(
	[LogEntryID] ASC,
	[TagValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[TagsAndValues]    Script Date: 01/19/2012 16:51:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[TagsAndValues]
AS
SELECT     TOP (100) PERCENT dbo.Tag.Name, dbo.TagValue.Value, COUNT(*) AS LogCount
FROM         dbo.Tag INNER JOIN
                      dbo.TagValue ON dbo.Tag.TagID = dbo.TagValue.TagID INNER JOIN
                      dbo.LogEntryTagValue ON dbo.TagValue.TagValueID = dbo.LogEntryTagValue.TagValueID
GROUP BY dbo.Tag.Name, dbo.TagValue.Value
ORDER BY dbo.Tag.Name, dbo.TagValue.Value

GO
/****** Object:  View [dbo].[FullLogInfomation]    Script Date: 01/19/2012 16:51:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FullLogInformation]
AS
SELECT     dbo.LogEntry.LogEntryID, dbo.LogEntry.Date, dbo.LogEntry.[Content], dbo.[Level].Name AS LevelName, dbo.Tag.Name AS TagName, 
                      dbo.TagValue.Value AS TagValue
FROM         dbo.TagValue INNER JOIN
                      dbo.LogEntryTagValue ON dbo.TagValue.TagValueID = dbo.LogEntryTagValue.TagValueID INNER JOIN
                      dbo.Tag ON dbo.TagValue.TagID = dbo.Tag.TagID RIGHT OUTER JOIN
                      dbo.[Level] INNER JOIN
                      dbo.LogEntry ON dbo.[Level].LevelID = dbo.LogEntry.LevelID ON dbo.LogEntryTagValue.LogEntryID = dbo.LogEntry.LogEntryID
GO

GO
/****** Object:  ForeignKey [FK_LogEntry_LogLevel]    Script Date: 01/19/2012 16:51:50 ******/
ALTER TABLE [dbo].[LogEntry]  WITH CHECK ADD  CONSTRAINT [FK_LogEntry_LogLevel] FOREIGN KEY([LevelID])
REFERENCES [dbo].[Level] ([LevelID])
GO
ALTER TABLE [dbo].[LogEntry] CHECK CONSTRAINT [FK_LogEntry_LogLevel]
GO
/****** Object:  ForeignKey [FK_LogEntryTagValue_LogEntry]    Script Date: 01/19/2012 16:51:50 ******/
ALTER TABLE [dbo].[LogEntryTagValue]  WITH CHECK ADD  CONSTRAINT [FK_LogEntryTagValue_LogEntry] FOREIGN KEY([LogEntryID])
REFERENCES [dbo].[LogEntry] ([LogEntryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogEntryTagValue] CHECK CONSTRAINT [FK_LogEntryTagValue_LogEntry]
GO
/****** Object:  ForeignKey [FK_LogEntryTagValue_TagValue]    Script Date: 01/19/2012 16:51:50 ******/
ALTER TABLE [dbo].[LogEntryTagValue]  WITH CHECK ADD  CONSTRAINT [FK_LogEntryTagValue_TagValue] FOREIGN KEY([TagValueID])
REFERENCES [dbo].[TagValue] ([TagValueID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogEntryTagValue] CHECK CONSTRAINT [FK_LogEntryTagValue_TagValue]
GO
/****** Object:  ForeignKey [FK_Rule_RuleSet]    Script Date: 01/19/2012 16:51:50 ******/
ALTER TABLE [dbo].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_RuleSet] FOREIGN KEY([RuleSetID])
REFERENCES [dbo].[RuleSet] ([RuleSetID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rule] CHECK CONSTRAINT [FK_Rule_RuleSet]
GO
/****** Object:  ForeignKey [FK_TagValue_Tag]    Script Date: 01/19/2012 16:51:50 ******/
ALTER TABLE [dbo].[TagValue]  WITH CHECK ADD  CONSTRAINT [FK_TagValue_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([TagID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TagValue] CHECK CONSTRAINT [FK_TagValue_Tag]
GO
INSERT INTO [Level]( LevelID, Name)
VALUES
(1, 'Debug'),
(2, 'Trace'),
(3, 'Warn'),
(4, 'Error'),
(5, 'Fatal'),
(6, 'Audit')
GO
/* ###################################################################################################################
Indice de Performance
################################################################################################################### */
CREATE NONCLUSTERED INDEX [PerformanceIndex01]
ON [dbo].[TagValue] ([TagID])
INCLUDE ([TagValueID],[Value])
GO
CREATE NONCLUSTERED INDEX [PerformanceIndex02]
ON [dbo].[LogEntryTagValue] ([TagValueID])
GO
drop index [TagValue].[PerformanceIndex01]
GO
ALTER TABLE TagValue ALTER COLUMN [Value] varchar(MAX) NULL
GO
Drop table [Rule];
GO
Drop table [RuleSet];
GO
/* ###################################################################################################################
Indice de App
################################################################################################################### */
ALTER TABLE dbo.LogEntry ADD Indexed bit NOT NULL CONSTRAINT DF_LogEntry_Indexed DEFAULT 0