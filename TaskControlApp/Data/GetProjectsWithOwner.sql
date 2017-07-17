SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bojana Borkovic
-- Create date: 7/6/2017
-- Description:	get all project with author included
-- =============================================
CREATE PROCEDURE GetAllProjectsWithOwner
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
