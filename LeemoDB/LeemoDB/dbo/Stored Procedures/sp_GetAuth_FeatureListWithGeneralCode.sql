CREATE procedure [dbo].[sp_GetAuth_FeatureListWithGeneralCode]

as
begin
	SELECT  DISTINCT
		AdminMenu.MenuName AdminMenuName
		, Feature.FeatureName		
		, GeneralCode.CodeName
		, GeneralCode.CodeValue
		--, GeneralCodeGroup.GroupName as CodeGroupName
		,Feature.Id AuthFeatureId
		,GeneralCode.Id GeneralCodeId
	FROM   
		Feature 			
		LEFT JOIN AdminMenu ON [Feature].AdminMenuId = AdminMenu.Id
		LEFT JOIN Auth_FeatureCodeMapping ON Feature.Id = Auth_FeatureCodeMapping.FeatureId 
		LEFT JOIN GeneralCode ON Auth_FeatureCodeMapping.CodeId = GeneralCode.Id 
		LEFT JOIN GeneralCodeGroup ON GeneralCode.GeneralCodeGroupId = GeneralCodeGroup.Id 
end