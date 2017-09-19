USE [TaskControl]
GO

/****** Object:  Table [dbo].[TaskStatusHistory]    Script Date: 7/17/2017 3:38:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TaskStatusHistory](
	[Id] [bigint]  IDENTITY(1,1) NOT NULL,
	[StatusBefore] [int]  NULL,
	[StatusAfter] [int] NOT NULL,
	[ChangeDate] [datetime] NULL,
	[TaskId] [bigint] NOT NULL,
 CONSTRAINT [PK_TaskStatusHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TaskStatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_TaskStatusHistory_Task] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([Id])
GO

ALTER TABLE [dbo].[TaskStatusHistory] CHECK CONSTRAINT [FK_TaskStatusHistory_Task]
GO


