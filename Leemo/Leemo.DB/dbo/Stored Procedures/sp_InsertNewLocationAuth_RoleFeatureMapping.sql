  Create Procedure sp_InsertNewLocationAuth_RoleFeatureMapping(
  @Createdby uniqueidentifier,
  @RoleId uniqueidentifier
  ) As
  
  Begin 

  Insert into [dbo].[Auth_RoleFeatureMapping]
  select aa.FeatureId as FeatureId ,
		 aa.CodeId as CodeId , @RoleId as RoleId , 
		 @Createdby as Createdby , GETDATE() as CreatedDate		 
		 from Auth_FeatureCodeMapping as aa
  
  End