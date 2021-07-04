

Create PROCEDURE [dbo].[sp_DeleteRole]  
(  
 @RoleId uniqueidentifier  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 -- declaring temp variables  
 DECLARE @retVal INT = 1, @msg nvarchar(200)='Record you are trying to delete is associated with ';  
  
 --check if exists in Auth Role Mapping  
  if Exists(select 1 from [User] where Id  in (select UserId from Auth_RoleUserMapping where RoleId =@RoleId)and IsActive = 1)
 BEGIN  
  SET @retVal=-1;  
  SET @msg+=' Users,'  
 END  
  
 IF @retVal=1  
 BEGIN  

 UPDATE Auth_Role
SET IsDeleted = 1
WHERE Id=@RoleId

  SET @msg = ''  
 END  
  
 SELECT @retVal ReturnValue  
 SELECT @msg ErrorMessage  
  
END