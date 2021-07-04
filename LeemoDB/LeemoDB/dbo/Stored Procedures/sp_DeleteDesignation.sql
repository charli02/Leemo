-- =============================================
-- Author:		Adarsh Prasad
-- Create date: 18-03-2021
-- Description:	Check designation, delete and return
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteDesignation]
(
	@DesignationId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;

	-- declaring temp variables
	DECLARE @retVal INT = 1, @msg nvarchar(200)='Record you are trying to delete is associated with ';

	--check if exists in Designation Hierarchy 
	IF EXISTS(SELECT 1 FROM DesignationHierarchy WHERE ParentDesignationId=@DesignationId)
	BEGIN
		SET @retVal = -1;
		SET @msg += 'other designations,';
	END

	--check if exists in User Profile
	IF EXISTS(SELECT 1 FROM UserProfile WHERE DesignationId=@DesignationId)
	BEGIN
		SET @retVal=-1;
		SET @msg+=' users,'
	END

	--check if exists in Group Designation Mapping
	IF EXISTS(SELECT 1 FROM GroupDesignationMapping WHERE DesignationId=@DesignationId)
	BEGIN
		SET @retVal=-1;
		SET @msg+=' groups,'
	END

	IF @retVal=1
	BEGIN
		DELETE FROM DesignationHierarchy WHERE DesignationId=@DesignationId
		DELETE FROM Designation WHERE Id=@DesignationId
		SET @msg = ''
	END

	SELECT @retVal ReturnValue
	SELECT @msg ErrorMessage

END