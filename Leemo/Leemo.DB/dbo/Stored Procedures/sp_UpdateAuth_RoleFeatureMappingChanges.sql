-- =============================================
-- Author:		Adarsh Prasad
-- Create date: 02-02-2021 
-- Description:	Save changes from temp to main table for feature permissions
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateAuth_RoleFeatureMappingChanges] --'7228c323-06ed-45aa-afb4-08d8b77cbd5c', '01e2f4fa-6c8f-4cfd-a525-08d896b9b5a9'
(
	@UserId uniqueidentifier,
	@RoleId uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON;
	--DECLARE @UserId uniqueidentifier='7228c323-06ed-45aa-afb4-08d8b77cbd5c',
	--@RoleId uniqueidentifier='01e2f4fa-6c8f-4cfd-a525-08d896b9b5a9'
	BEGIN TRANSACTION  

	DECLARE @inserr int  
	DECLARE @delerr int  
	DECLARE @maxerr int
	DECLARE @retval int

	-- Deleting all existing mappings
	DELETE FROM Auth_RoleFeatureMapping
	WHERE RoleId = @RoleId

	-- Save error number returned from Delete statement  
	SET @delerr = @@error  
	IF @delerr > @maxerr  
	SET @maxerr = @delerr  

	-- Inserting all Auth_RoleFeatureMapping for perticular role with all features
	INSERT INTO Auth_RoleFeatureMapping 
	(FeatureId,CodeId,RoleId,CreatedBy,CreatedDate)
	SELECT FeatureId, CodeId, RoleId, @UserId, GETUTCDATE() 
	FROM Auth_RoleFeatureMappingTemp
	WHERE SessionId = @UserId AND RoleId = @RoleId AND (IsDeleted = 0 OR IsDeleted IS NULL)

	-- Save error number returned from Insert statement  
	SET @inserr = @@error  
	IF @inserr > @maxerr  
	SET @maxerr = @inserr

	-- Update modified by
	UPDATE Auth_Role SET ModifiedBy = @UserId, ModifiedOn = GETUTCDATE()
	WHERE Id = @RoleId

	-- Save error number returned from update statement  
	SET @inserr = @@error  
	IF @inserr > @maxerr  
	SET @maxerr = @inserr

	-- Inserting deleted features 
	--INSERT INTO Auth_RoleFeatureMappingDeleted
	--(FeatureId,CodeId,RoleId,DeletedDate,CreatedDate,CreatedBy,DeletedBy)
	--SELECT FeatureId, CodeId, RoleId, GETUTCDATE(),CreatedDate,CreatedBy, @UserId
	--FROM Auth_RoleFeatureMappingTemp
	--WHERE SessionId = @UserId AND RoleId = @RoleId AND (IsDeleted = 1)

	-- Save error number returned from Insert statement  
	--SET @inserr = @@error  
	--IF @inserr > @maxerr  
	--SET @maxerr = @inserr

	-- If an error occurred, roll back  
	IF @maxerr <> 0  
	BEGIN  
		ROLLBACK  
		SET @retval = 0
	END  
	ELSE  
	BEGIN  
		COMMIT  
		SET @retval = 1
	END

	SELECT @retval
END