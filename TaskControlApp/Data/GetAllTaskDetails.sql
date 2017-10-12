SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 16/7/2017
-- Description:	get all tasks with details included (author, issue type, reporter...)
-- =============================================
CREATE PROCEDURE GetAllTasksDetails
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
			  project.Name as 'Project'
			  FROM [dbo].Tasks task
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
