USE [TaskControl]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 7/17/2017 3:31:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[Id] [bigint] NOT NULL,
	[Text] [nvarchar](1000) NULL,
	[DateCreated] [datetime] NULL,
	[Author] [bigint] NOT NULL,
	[TaskId] [bigint] NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IssueType]    Script Date: 7/17/2017 3:31:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_IssueType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Priority]    Script Date: 7/17/2017 3:31:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Priority](
	[Id] [bigint] NOT NULL,
	[Name] [nchar](10) NULL,
 CONSTRAINT [PK_Priority] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 7/17/2017 3:31:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[OwnerId] [bigint] NULL,
	[Description] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 7/17/2017 3:31:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Task]    Script Date: 7/17/2017 3:31:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [bigint] NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[IssueType] [int] NOT NULL,
	[Asignee] [bigint] NULL,
	[DateCreated] [datetime] NULL,
	[Status] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Reporter] [int] NULL,
	[Priority] [bigint] NULL,
	[ProjectId] [bigint] NOT NULL,
	[DueDate] [datetime] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Comment] ([Id], [Text], [DateCreated], [Author], [TaskId]) VALUES (2, N'ovo je komentar ', CAST(N'2017-06-17 00:00:00.000' AS DateTime), 1, 2)
INSERT [dbo].[Comment] ([Id], [Text], [DateCreated], [Author], [TaskId]) VALUES (3, N'neki  comment', CAST(N'2017-07-17 00:00:00.000' AS DateTime), 1, 3)
INSERT [dbo].[Comment] ([Id], [Text], [DateCreated], [Author], [TaskId]) VALUES (4, N'aahahaha', CAST(N'2017-07-17 00:00:00.000' AS DateTime), 2, 4)
INSERT [dbo].[Comment] ([Id], [Text], [DateCreated], [Author], [TaskId]) VALUES (6, N'sadsada', CAST(N'2017-07-17 00:00:00.000' AS DateTime), 2, 5)
INSERT [dbo].[Comment] ([Id], [Text], [DateCreated], [Author], [TaskId]) VALUES (7, N'sadasda', CAST(N'2017-07-17 00:00:00.000' AS DateTime), 3, 6)
INSERT [dbo].[Comment] ([Id], [Text], [DateCreated], [Author], [TaskId]) VALUES (8, N'sadsasad', CAST(N'2017-07-17 00:00:00.000' AS DateTime), 4, 7)
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (1, N'Bug')
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (2, N'Feature')
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (3, N'Improvement')
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (4, N'Story')
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (5, N'Task')
INSERT [dbo].[IssueType] ([Id], [Name]) VALUES (6, N'Subtask')
INSERT [dbo].[Priority] ([Id], [Name]) VALUES (1, N'Low       ')
INSERT [dbo].[Priority] ([Id], [Name]) VALUES (2, N'Medium    ')
INSERT [dbo].[Priority] ([Id], [Name]) VALUES (3, N'High      ')
INSERT [dbo].[Priority] ([Id], [Name]) VALUES (4, N'Urgent    ')
INSERT [dbo].[Project] ([Id], [Name], [OwnerId], [Description]) VALUES (1, N'Test', 2, NULL)
INSERT [dbo].[Project] ([Id], [Name], [OwnerId], [Description]) VALUES (2, N'Bocca', 2, NULL)
INSERT [dbo].[Project] ([Id], [Name], [OwnerId], [Description]) VALUES (3, N'Novi Projekat', 1, NULL)
INSERT [dbo].[Project] ([Id], [Name], [OwnerId], [Description]) VALUES (4, N'SuperCoolProject', 2, NULL)
INSERT [dbo].[Project] ([Id], [Name], [OwnerId], [Description]) VALUES (5, N'Marsovci', 3, N'Neki projekat')
INSERT [dbo].[Project] ([Id], [Name], [OwnerId], [Description]) VALUES (6, N'TamoNekiProjekat', 4, N'hahaha')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (1, N'New')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (2, N'InProgress')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (3, N'Delayed')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (4, N'Done')
INSERT [dbo].[Task] ([Id], [Title], [IssueType], [Asignee], [DateCreated], [Status], [Description], [Reporter], [Priority], [ProjectId], [DueDate]) VALUES (2, N'Implement welcome page', 5, 2, CAST(N'2017-07-07 00:00:00.000' AS DateTime), 1, N'Create and implement welcome page on the website.', 1, 1, 5, CAST(N'2017-10-10 00:00:00.000' AS DateTime))
INSERT [dbo].[Task] ([Id], [Title], [IssueType], [Asignee], [DateCreated], [Status], [Description], [Reporter], [Priority], [ProjectId], [DueDate]) VALUES (3, N'Implement basic authentication', 5, 3, CAST(N'2017-07-17 00:00:00.000' AS DateTime), 2, N'implement oauth', 1, 1, 1, CAST(N'2017-10-10 00:00:00.000' AS DateTime))
INSERT [dbo].[Task] ([Id], [Title], [IssueType], [Asignee], [DateCreated], [Status], [Description], [Reporter], [Priority], [ProjectId], [DueDate]) VALUES (4, N'Add user mgmt', 5, 1, CAST(N'2017-07-17 00:00:00.000' AS DateTime), 1, N'add page were users will be managed', 2, 2, 3, CAST(N'2017-10-10 00:00:00.000' AS DateTime))
INSERT [dbo].[Task] ([Id], [Title], [IssueType], [Asignee], [DateCreated], [Status], [Description], [Reporter], [Priority], [ProjectId], [DueDate]) VALUES (5, N'Design contact page', 4, 2, CAST(N'2017-07-17 00:00:00.000' AS DateTime), 3, N'implement and design contact page', 3, 3, 4, CAST(N'2017-10-10 00:00:00.000' AS DateTime))
INSERT [dbo].[Task] ([Id], [Title], [IssueType], [Asignee], [DateCreated], [Status], [Description], [Reporter], [Priority], [ProjectId], [DueDate]) VALUES (6, N'Create about us page', 2, 1, CAST(N'2017-07-17 00:00:00.000' AS DateTime), 2, N'implement and complete about us page', 2, 4, 2, CAST(N'2017-11-12 00:00:00.000' AS DateTime))
INSERT [dbo].[Task] ([Id], [Title], [IssueType], [Asignee], [DateCreated], [Status], [Description], [Reporter], [Priority], [ProjectId], [DueDate]) VALUES (7, N'Design welcome email', 4, 4, CAST(N'2017-07-17 00:00:00.000' AS DateTime), 4, N'design and implement welcome email', 4, 4, 6, CAST(N'2017-11-12 00:00:00.000' AS DateTime))
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Task] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([Id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Task]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_AspNetUsers] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_AspNetUsers]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Issue_Type] FOREIGN KEY([IssueType])
REFERENCES [dbo].[IssueType] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Issue_Type]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Priority] FOREIGN KEY([Priority])
REFERENCES [dbo].[Priority] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Priority]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Project]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Status] FOREIGN KEY([Status])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Status]
GO
