IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Script]') AND type in (N'U'))
BEGIN
CREATE TABLE [Script](
	[Environment] [varchar](200) NOT NULL,
	[ScriptFolder] [varchar](260) NOT NULL,
	[RelativePath] [varchar](260) NOT NULL,
	[Hash] [binary](16) NOT NULL,
	[Status] [varchar](25) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[DateStarted] [datetime] NOT NULL,
	[DateCompleted] [datetime] NOT NULL,
 CONSTRAINT [PK_Script] PRIMARY KEY CLUSTERED 
(
	[Environment] ASC,
	[ScriptFolder] ASC,
	[RelativePath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

