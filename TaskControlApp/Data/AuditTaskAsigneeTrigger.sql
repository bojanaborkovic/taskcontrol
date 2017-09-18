SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 9/15/2017
-- Description:	Audit of author who changed Task Asignee
-- =============================================
CREATE TRIGGER [dbo].AuditTaskAsigneeTrigger 
   ON  [dbo].[dbo].[Task] 
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @time [datetime2](7) = SYSUTCDATETIME()
	DECLARE @ctx [varbinary](128) = CONTEXT_INFO()

	   -- Insert statements for trigger here
	  IF (SELECT COUNT(*) FROM inserted) > 0
		BEGIN
 
			INSERT INTO [dbo].[TaskAsigneeHistory]
			([AsigneeBefore],
			 [AsigneeAfter],
			 [ChangeDate],
			 [TaskId])
			SELECT i.Asignee, i.Asignee, @time, i.Id from inserted i

		END
END
GO
