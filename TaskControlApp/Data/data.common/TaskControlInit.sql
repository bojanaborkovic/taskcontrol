USE [TaskControl]
GO
/****** Object:  User [NT AUTHORITY\NETWORK SERVICE]    Script Date: 24.1.2018. 10:16:06 ******/
CREATE USER [NT AUTHORITY\NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DateCreated] [datetime] NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[FirstName] [nvarchar](500) NULL,
	[LastName] [nvarchar](500) NULL,
	[CultureCode] [varchar](5) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](1000) NULL,
	[DateCreated] [datetime] NULL,
	[Author] [bigint] NOT NULL,
	[TaskId] [bigint] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IssueType]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_IssueTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Note]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Note](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](1000) NULL,
	[DateCreated] [datetime] NULL,
	[Author] [bigint] NULL,
	[ProjectId] [bigint] NOT NULL,
 CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Priority]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Priority](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Priorities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[OwnerId] [bigint] NULL,
	[Description] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RoleClaimsOnProject]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleClaimsOnProject](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleId] [bigint] NOT NULL,
	[ProjectId] [bigint] NOT NULL,
	[HaveAcess] [bit] NULL,
 CONSTRAINT [PK_RoleClaimsOnProject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 24.1.2018. 10:16:06 ******/
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
/****** Object:  Table [dbo].[Task]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[IssueType] [int] NOT NULL,
	[Asignee] [bigint] NULL,
	[DateCreated] [datetime] NULL,
	[Status] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Reporter] [int] NULL,
	[Priority] [int] NOT NULL,
	[ProjectId] [bigint] NOT NULL,
	[DueDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaskAsigneeHistory]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskAsigneeHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AsigneeBefore] [bigint] NULL,
	[AsigneeAfter] [bigint] NOT NULL,
	[ChangeDate] [datetime] NOT NULL,
	[TaskId] [bigint] NOT NULL,
	[ChangeBy] [bigint] NULL,
 CONSTRAINT [PK_TaskAsigneeHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaskStatusHistory]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskStatusHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StatusBefore] [int] NULL,
	[StatusAfter] [int] NOT NULL,
	[ChangeDate] [datetime] NULL,
	[TaskId] [bigint] NOT NULL,
	[ChangeBy] [bigint] NULL,
 CONSTRAINT [PK_TaskStatusHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo_AspNetRoleClaims_dbo_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_dbo_AspNetRoleClaims_dbo_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Task] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([Id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Task]
GO
ALTER TABLE [dbo].[Note]  WITH CHECK ADD  CONSTRAINT [FK_Note_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Note] CHECK CONSTRAINT [FK_Note_Project]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_AspNetUsers] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_AspNetUsers]
GO
ALTER TABLE [dbo].[RoleClaimsOnProject]  WITH CHECK ADD  CONSTRAINT [FK_RoleClaimsOnProject_AspNetRoles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[RoleClaimsOnProject] CHECK CONSTRAINT [FK_RoleClaimsOnProject_AspNetRoles]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Issue_Type] FOREIGN KEY([IssueType])
REFERENCES [dbo].[IssueType] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Issue_Type]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_AspNetUsers] FOREIGN KEY([Asignee])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_AspNetUsers]
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
ALTER TABLE [dbo].[TaskAsigneeHistory]  WITH CHECK ADD  CONSTRAINT [FK_TaskAsigneeHistory_Task] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([Id])
GO
ALTER TABLE [dbo].[TaskAsigneeHistory] CHECK CONSTRAINT [FK_TaskAsigneeHistory_Task]
GO
ALTER TABLE [dbo].[TaskStatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_TaskStatusHistory_Task] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([Id])
GO
ALTER TABLE [dbo].[TaskStatusHistory] CHECK CONSTRAINT [FK_TaskStatusHistory_Task]
GO
/****** Object:  StoredProcedure [dbo].[GetAllProjectsWithOwner]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 7/6/2017
-- Description:	get all project with author included
-- =============================================
CREATE PROCEDURE [dbo].[GetAllProjectsWithOwner]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT project.[Id] as 'ProjectId', project.Name as 'Name', project.[Description] as 'Description', users.UserName as 'Owner', users.Id as 'OwnerId'
	FROM Project project INNER JOIN AspNetUsers users
	ON project.OwnerId = users.Id
END

GO
/****** Object:  StoredProcedure [dbo].[GetAllTasksDetails]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 16/7/2017
-- Description:	get all tasks with details included (author, issue type, reporter...)
-- =============================================
CREATE PROCEDURE [dbo].[GetAllTasksDetails]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	  SELECT  task.Id as 'TaskId',
			  task.Title,
			  assignee.UserName as 'Asignee',
			  task.Description,
			  task.DueDate,
			  task.DateCreated,
			  IssueType.Name as 'IssueType',
			  priority.Name as 'Priority',
			  taskStatus.Name as 'TaskStatus',
			  reporter.UserName as 'Reporter',
			  project.Name as 'Project',
			  project.Id as 'ProjectId'
			  FROM [dbo].Task task
			  INNER JOIN [dbo].Project project
			  on task.ProjectId = project.Id
			  INNER JOIN [dbo].Status taskStatus
			  on task.Status = taskStatus.Id
			  INNER JOIN [dbo].AspNetUsers reporter
			  on task.Reporter = reporter.Id
			  INNER JOIN [dbo].Priority priority
			  on task.Priority = priority.Id
			  --INNER JOIN [dbo].Comment comment
			  --on comment.TaskId = task.Id
			  INNER JOIN [dbo].IssueType issueType
			  on task.IssueType = issueType.Id
			  INNER JOIN [dbo].AspNetUsers assignee
			  on task.Asignee = assignee.Id
END


GO
/****** Object:  StoredProcedure [dbo].[GetProjectStatistics]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 09/19/2017
-- Description:	Get all tasks for project, sort by status
-- =============================================
CREATE PROCEDURE [dbo].[GetProjectStatistics] 
	-- Add the parameters for the stored procedure here
	@projectId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [dbo].[Task] WHERE [ProjectId] = @projectId ORDER By [Status]
END

GO
/****** Object:  StoredProcedure [dbo].[GetTaskById]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 7/19/2017
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetTaskById] 
	-- Add the parameters for the stored procedure here
	@taskId bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT task.Id AS 'TaskId', task.DateCreated, task.Description, task.Title, task.IssueType, task.Priority,
	task.Status, task.ProjectId, reporter.UserName as 'Reporter', assigne.UserName as 'Asignee', reporter.Id as 'ReporterId', 
	assigne.Id as 'AsigneeId', task.DueDate 
	FROM [dbo].[Task] task
	INNER JOIN [dbo].[AspNetUsers] assigne
	on task.Asignee = assigne.Id
	INNER JOIN [dbo].[AspNetUsers] reporter
	on task.Reporter = reporter.Id
	WHERE task.Id = @taskId
END

GO
/****** Object:  StoredProcedure [dbo].[GetTasksAssigneStatusHistory]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetTasksAssigneStatusHistory] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT asigneeHistory.TaskId,
		   asigneeHistory.AsigneeBefore, 
		   asigneeHistory.AsigneeAfter,
		   asigneeHistory.ChangeBy AS AssigneChangedBy,
		   asigneeHistory.ChangeDate AS AssigneChangedOn,
	       statusHistory.StatusBefore, 
		   statusHistory.StatusAfter, 
		   statusHistory.ChangeBy AS StatusChangeBy, 
		   statusHistory.ChangeDate AS StatusChangedOn FROM [dbo].TaskAsigneeHistory asigneeHistory
	INNER JOIN [dbo].TaskStatusHistory statusHistory
	on asigneeHistory.TaskId = statusHistory.TaskId
	ORDER By AssigneChangedOn DESC
END

GO
/****** Object:  StoredProcedure [dbo].[GetTasksAudit]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetTasksAudit]
  @FromDate datetime2(7),
  @ToDate datetime2(7),
  @pageNumber int,
  @pageSize int,
  @orderBy varchar(50)
AS

SET NOCOUNT ON

DECLARE @sql nvarchar(max);
DECLARE @andstring nvarchar(5);

SET @sql = 'WITH SelectSqlWithRowNums AS (SELECT';

DECLARE @minNumber int;
DECLARE @maxNumber int;
IF (@pageNumber IS NOT NULL AND @pageSize IS NOT NULL)
BEGIN
  SET @minNumber = (@pageNumber - 1) * @pageSize;
  SET @maxNumber = @pageNumber * @pageSize;
  SET @sql = @sql + ' TOP(@maxNumber) ROW_NUMBER () OVER (ORDER BY ' + @orderBy +') AS [RowNumber], ';
END
ELSE
  SET @sql = @sql + ' ROW_NUMBER () OVER (ORDER BY ' + @orderBy + ') AS [RowNumber], ';

SET @sql = @sql + ' COUNT(*) OVER () AS [RecordCount],* FROM (SELECT asigneeHistory.TaskId, asigneeHistory.AsigneeBefore, asigneeHistory.AsigneeAfter,
asigneeHistory.ChangeBy AS AssigneChangedBy, asigneeHistory.ChangeDate AS AssigneChangedOn,
 statusHistory.StatusBefore, statusHistory.StatusAfter, statusHistory.ChangeBy
AS StatusChangeBy, statusHistory.ChangeDate AS StatusChangedOn FROM [dbo].TaskAsigneeHistory asigneeHistory
INNER JOIN [dbo].TaskStatusHistory statusHistory
on asigneeHistory.TaskId = statusHistory.TaskId';

IF (@FromDate IS NOT NULL OR @ToDate IS NOT NULL)
  SET @sql = @sql + ' WHERE ';

SET @andstring = '';
IF (@FromDate IS NOT NULL)
BEGIN
  SET @sql = @sql + @andstring + 'AssigneChangedOn >= @FromDate AND StatusChangedOn >= @FromDate';
  SET @andstring = ' AND ';
END

IF (@ToDate IS NOT NULL)
BEGIN
  SET @sql = @sql + @andstring + 'AssigneChangedOn <= @ToDate AND StatusChangedOn <= @ToDate';
  SET @andstring = ' AND ';
END

SET @sql = @sql + ') AS SelectSql) SELECT * FROM SelectSqlWithRowNums';

IF (@minNumber IS NOT NULL)
  SET @sql = @sql + ' WHERE [RowNumber] > @minNumber AND [RowNumber] <= @maxNumber';


EXEC sp_executesql @sql, N'@FromDate datetime2(7), @ToDate datetime2(7), @minNumber int, @maxNumber int',
                   @FromDate, @ToDate,  @minNumber, @maxNumber;


GO
/****** Object:  StoredProcedure [dbo].[SearchUsers]    Script Date: 24.1.2018. 10:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 31/10/2017
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SearchUsers] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT users.Id as 'UserId', UserName as 'Username', Email as 'Email', FirstName as 'FirstName', LastName as 'LastName', roles.Name as 'RoleName' FROM [dbo].[AspNetUsers] users
	LEFT JOIN [dbo].[AspNetUserRoles] userRoles
	ON userRoles.UserId = users.Id
	LEFT JOIN [dbo].[AspNetRoles] roles
	ON roles.Id=userRoles.RoleId
	
    
END

GO
