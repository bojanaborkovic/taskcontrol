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
-- Create date: 31/10/2017
-- Description:	
-- =============================================
CREATE PROCEDURE SearchUsers 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT users.Id as 'UserId', UserName as 'Username', Email as 'Email', FirstName as 'FirstName', LastName as 'LastName', roles.Name as 'RoleName' FROM [dbo].[AspNetUsers] users
	LEFT JOIN [dbo].[AspNetUserRoles] userRoles
	ON userRoles.UserId = users.Id
	LEFT JOIN [dbo].[AspNetRoles] roles
	ON roles.Id = userRoles.RoleId
	
    
END
GO
