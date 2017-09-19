SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 9/15/2017
-- Description:	Audit of author who changed Task Asignee
-- =============================================
ALTER TRIGGER [dbo].[AuditTaskAsigneeTrigger] 
   ON [dbo].[Task] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @time [datetime2](7) = SYSUTCDATETIME()


	   -- Insert statements for trigger here
	    IF (SELECT COUNT(*) FROM inserted) > 0
		  BEGIN
			IF (SELECT COUNT(*) FROM deleted) > 0
			BEGIN
 
			INSERT INTO [dbo].[TaskAsigneeHistory]
			([AsigneeBefore],
			 [AsigneeAfter],
			 [ChangeDate],
			 [TaskId],
			 [ChangeBy])

			SELECT d.Asignee AS [AsigneeBefore], i.Asignee AS [AsigneeAfter], @time AS [ChangeDate], i.Id as [TaskId], i.CreatedBy as [ChangeBy]
			FROM inserted i
			FULL OUTER JOIN deleted d ON i.Id = d.Id

			END

		END


		IF (SELECT COUNT(*) FROM inserted) > 0
		BEGIN
			IF (SELECT COUNT(*) FROM deleted) = 0
			BEGIN
			INSERT INTO [dbo].[TaskAsigneeHistory]
			([AsigneeBefore],
			 [AsigneeAfter],
			 [ChangeDate],
			 [TaskId],
			 [ChangeBy])

			 SELECT NULL, i.Asignee AS [AsigneeAfter], @time AS [ChangeDate], i.Id as [TaskId], i.CreatedBy as [ChangeBy]
			FROM inserted i
			END
		END
		
END

GO