
CREATE PROCEDURE [dbo].[sp_DeleteAuth_RoleUsersMappingByUserId]
(
	@CompanyLocationId uniqueidentifier,
	@UserId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	-- Get all roles of a perticular location user wise
	SELECT Id AS RoleId INTO #tempRoles FROM Auth_Role
	WHERE CompanyLocationId = @CompanyLocationId

	-- Remove all roles of a user location wise
	DELETE FROM Auth_RoleUserMapping 
	WHERE RoleId IN (SELECT RoleId FROM #tempRoles) 
	AND UserId = @UserId

	DROP TABLE #tempRoles

END