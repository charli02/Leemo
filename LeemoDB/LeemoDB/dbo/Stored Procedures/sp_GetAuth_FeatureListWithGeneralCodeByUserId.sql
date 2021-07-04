CREATE procedure [dbo].[sp_GetAuth_FeatureListWithGeneralCodeByUserId]
(
	@UserId uniqueidentifier null,
	@RoleId uniqueidentifier null,
	@CompanyLocationId uniqueidentifier null
)
as
begin


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
FROM   
		Feature 
		INNER JOIN Auth_FeatureCodeMapping ON Feature.Id = Auth_FeatureCodeMapping.FeatureId 
		INNER JOIN Auth_RoleFeatureMapping ON Auth_FeatureCodeMapping.FeatureId = Auth_RoleFeatureMapping.FeatureId 
				AND Auth_FeatureCodeMapping.CodeId = Auth_RoleFeatureMapping.CodeId 
		INNER JOIN GeneralCode ON Auth_FeatureCodeMapping.CodeId = GeneralCode.Id 
		INNER JOIN GeneralCodeGroup ON GeneralCode.GeneralCodeGroupId = GeneralCodeGroup.Id 
		INNER JOIN [Auth_Role] ON Auth_RoleFeatureMapping.RoleId = [Auth_Role].Id 
		INNER JOIN [Auth_RoleUserMapping] ON [Auth_Role].Id = [Auth_RoleUserMapping].RoleId 
		LEFT OUTER JOIN [User] ON [Auth_RoleUserMapping].UserId = [User].Id
		LEFT OUTER JOIN AdminMenu ON [Feature].AdminMenuId = AdminMenu.Id
WHERE  (@UserId is null or ([User].Id = @UserId))
	   And	(@RoleId is null or ([Auth_Role].Id = @RoleId))
	   and Auth_Role.CompanyLocationId = @CompanyLocationId
end