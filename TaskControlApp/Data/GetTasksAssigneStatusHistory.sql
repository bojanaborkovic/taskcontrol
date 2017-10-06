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
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE GetTasksAssigneStatusHistory 
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
END
GO
