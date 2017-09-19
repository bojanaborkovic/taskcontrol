USE [TaskControl]
GO

/****** Object:  Table [dbo].[TaskAsigneeHistory]    Script Date: 7/17/2017 3:38:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TaskAsigneeHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AsigneeBefore] [bigint]  NULL,
	[AsigneeAfter] [bigint] NOT NULL,
	[ChangeDate] [datetime] NOT NULL,
	[TaskId] [bigint]  NOT NULL,
 CONSTRAINT [PK_TaskAsigneeHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TaskAsigneeHistory]  WITH CHECK ADD  CONSTRAINT [FK_TaskAsigneeHistory_Task] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([Id])
GO

ALTER TABLE [dbo].[TaskAsigneeHistory] CHECK CONSTRAINT [FK_TaskAsigneeHistory_Task]
GO


