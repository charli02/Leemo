  

-- =============================================  
-- Author:  Adarsh Prasad  
-- Create date: 01-02-2021  
-- Description: Procedure to get all the role feature listing with permissions  
-- =============================================  
create PROCEDURE [dbo].[sp_GetAuth_FeatureListWithGeneralCodeByUserId_New] --'f2550a14-f411-4768-b974-08d89cd2a915', '9a5ca3a9-18f6-4940-8d4e-08d8975037cb'  
(  
 @UserId uniqueidentifier null,  
 @RoleId uniqueidentifier null,  
 @ProductId uniqueidentifier null  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 -- Delete existing record  
 DELETE FROM Auth_RoleFeatureMappingTemp WHERE SessionId=@UserId --AND RoleId = @RoleId  
  
   
 -- Available features for this role product wise  
 SELECT FeatureId, CodeId, FeatureName, CodeValue INTO #TEMP1  
 FROM Auth_RoleFeatureMapping ARFM  
 LEFT JOIN GeneralCode GC ON ARFM.CodeId = GC.Id   
 LEFT OUTER JOIN Feature AF ON AF.Id = FeatureId  
 WHERE RoleId=@RoleId  
 AND AF.ProductFeatureGroupId IN (SELECT ProductFeatureGroupId FROM ProductFeatureGroup WHERE ProductId = @ProductId)  
  
 -- Insert into temp table   
 INSERT INTO Auth_RoleFeatureMappingTemp   
 (FeatureId,CodeId,RoleId,CreatedDate,SessionId,CreatedBy,IsDeleted)  
  
 SELECT Feature.Id, GeneralCode.Id, @RoleId, GETUTCDATE(), @UserId, @UserId  
 ,CASE WHEN TP1.CodeId is NULL THEN 1 ELSE 0 END AS IsDeleted  
 FROM Feature      
 INNER JOIN ProductFeatureGroup PFG ON PFG.Id = Feature.ProductFeatureGroupId
 LEFT JOIN AdminMenu ON [Feature].AdminMenuId = AdminMenu.Id  
 LEFT JOIN Auth_FeatureCodeMapping ON Feature.Id = Auth_FeatureCodeMapping.FeatureId   
 LEFT JOIN GeneralCode ON Auth_FeatureCodeMapping.CodeId = GeneralCode.Id   
 LEFT JOIN GeneralCodeGroup ON GeneralCode.GeneralCodeGroupId = GeneralCodeGroup.Id   
 LEFT JOIN #TEMP1 TP1 ON TP1.CodeId = Auth_FeatureCodeMapping.CodeId AND TP1.FeatureId = Auth_FeatureCodeMapping.FeatureId  
 WHERE GeneralCode.Id IS NOT NULL AND PFG.ProductId = @ProductId
  
 DROP TABLE #TEMP1  
  
 -- Get feature list permission wise for perticular RoleId  
    SELECT    
  AdminMenu.MenuName as AdminMenuName  
  , Feature.FeatureName  
  , GeneralCode.CodeName  
  , GeneralCode.CodeValue  
  , GeneralCodeGroup.GroupName as CodeGroupName  
  , [User].UserName  
  , [USER].Id as UserId  
  , [Auth_Role].Name as RoleName  
  , Feature.Id AuthFeatureId  
  , GeneralCode.Id GeneralCodeId  
  , Auth_RoleFeatureMappingTemp.IsDeleted  
FROM     
  Feature   
  INNER JOIN Auth_FeatureCodeMapping ON Feature.Id = Auth_FeatureCodeMapping.FeatureId   
  INNER JOIN Auth_RoleFeatureMappingTemp ON Auth_FeatureCodeMapping.FeatureId = Auth_RoleFeatureMappingTemp.FeatureId   
    AND Auth_FeatureCodeMapping.CodeId = Auth_RoleFeatureMappingTemp.CodeId   
  INNER JOIN GeneralCode ON Auth_FeatureCodeMapping.CodeId = GeneralCode.Id   
  INNER JOIN GeneralCodeGroup ON GeneralCode.GeneralCodeGroupId = GeneralCodeGroup.Id   
  INNER JOIN [Auth_Role] ON Auth_RoleFeatureMappingTemp.RoleId = [Auth_Role].Id   
  LEFT OUTER JOIN [Auth_RoleUserMapping] ON [Auth_Role].Id = [Auth_RoleUserMapping].RoleId   
  LEFT OUTER JOIN [User] ON [Auth_RoleUserMapping].UserId = [User].Id  
  LEFT OUTER JOIN AdminMenu ON [Feature].AdminMenuId = AdminMenu.Id  
  WHERE (@RoleId is null or ([Auth_Role].Id = @RoleId))  
  AND (@UserId is null or ([Auth_RoleFeatureMappingTemp].SessionId = @UserId))   
  AND Feature.IsActive=1  
  --AND (@UserId is null or ([User].Id = @UserId)) AND   
  
END