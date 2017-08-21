-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 7/19/2017
-- Description:	
-- =============================================
CREATE PROCEDURE GetTaskById 
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






