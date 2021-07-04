  
-- =============================================  
-- Author:  Vikash Kumar  
-- Create date: 30-04-2021   
-- Description: Seup Company for user with username and the CompanyName  
-- =============================================  
  
CREATE PROCEDURE [dbo].[sp_CompanySetup]  
(  
 @CompanyName varchar(max),  
 @UserName varchar(max),  
 @PasswordHash varchar(max),  
 @PasswordSalt varchar(max),  
 @FirstName varchar(max),  
 @LastName varchar(max)  
)   
AS  
BEGIN   
 SET NOCOUNT ON;  
  
 --temporary variable declaration  
 DECLARE @CompanyId uniqueidentifier = NEWID()  
 DECLARE @LocationId uniqueidentifier = NEWID()  
 DECLARE @AddressId uniqueidentifier = NEWID()  
 DECLARE @AddressTypeId uniqueidentifier = Cast('4E6FE552-2C8A-4AC2-A784-8A26583125EE' as uniqueidentifier)  
 DECLARE @RoleId uniqueidentifier = NEWID()  
 DECLARE @DesignationId uniqueidentifier = NEWID()  
 DECLARE @UserId uniqueidentifier = NEWID()  
 DECLARE @ProductId uniqueidentifier = Cast('75B0307D-8DBD-4A2F-B00D-500F4B5137F5' as uniqueidentifier)  
 DECLARE @SubscriptionPlanId uniqueidentifier = Cast('BAEDE308-6B5F-41AF-AC53-50BBD6B503D8' as uniqueidentifier)
 DECLARE @DatabaseName varchar(250) = @CompanyName + '_Db'  
  
 BEGIN TRY  
  BEGIN TRANSACTION;    
  
   DECLARE @retval bit  
  
   -- Inserting Data into Company with the given CompanyName  
   INSERT INTO Company  
   (Id,Name,EmployeeCount,Email,Phone,Mobile,Fax,Website,Description,Currency,TimeZone,CreatedOn, ModifiedOn,Language,ImageName,CountryCode,TimeFormat,DateFormat,DatabaseName)  
   VALUES (@CompanyId,@CompanyName,1,@UserName,'123456','91-0000000',null,'test.com',null,'INR',null,GETDATE(),null,null,null,null,null,null,@DatabaseName)  
  
   --Inserting Data into CompanyLocation  
   INSERT INTO CompanyLocation  
   (Id,CompanyId,LocationName,IsHeadOffice,IsActive,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy)  
   VALUES (@LocationId,@CompanyId,'Test',1,1,GETDATE(),null,null,null)  
     
   --Inserting Data into Addresses  
   INSERT INTO Addresses  
   (Id,ReferenceId,AddressTypeId,AddressLine1,AddressLine2,Street,City,State,ZipCode,Country,Phone,CreatedOn,ModifiedOn,CountryCode,Fax)  
   VALUES (@AddressId,@LocationId,@AddressTypeId,'-','-','-','Chandigarh','-','123456','India','91-0000000',GETDATE(),null,'in','12345')  
  
   --Inserting Data into Designation  
   INSERT INTO Designation  
   (Id,Name,Description,IsActive,CreatedOn,ModifiedOn,CompanyLocationId,IsRoot)  
   VALUES (@DesignationId,@CompanyName,'...',1,GETDATE(),null,@LocationId,1)  
     
   --Inserting Data into [User]  
   INSERT INTO [User]  
   (Id,UserName,PasswordSalt,PasswordHash,CreatedOn,ModifiedOn,ForcePasswordReset,IsActive,TempPasswordHash,TempPasswordSalt,TempPasswordExpiryDate,IsFirstLogin)  
   VALUES (@UserId,@UserName,@PasswordSalt,@PasswordHash,GETDATE(),null,0,1,null,null,null,0)  
     
   --Inserting Data into UserProfile  
   INSERT INTO UserProfile  
   (Id,UserId,FirstName,LastName,DateOfBirth,DesignationId,Alias,Phone,Mobile,Fax,Website,Language,CountryLocale,DateFormat,TimeFormat,TimeZone,CompanyId,CreatedOn,ModifiedOn,ReportingToUserId,Description,ImageName,CountryCode)  
   VALUES (NEWID(),@UserId,@FirstName,@LastName,'1944-03-08',@DesignationId,'New',null,'91-1234567',null,null,null,null,null,null,null,@CompanyId,GETDATE(),null,null,'...',null,'in')  
  
   --Inserting Data into Auth_Role  
   INSERT INTO Auth_Role  
   (Id,Name,Description,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsDeleted,CompanyLocationId)  
   VALUES (@RoleId,'Owner','...',GETDATE(),@UserId,null,null,0,@LocationId)  
  
   --Inserting Data into Auth_RoleUserMapping  
   INSERT INTO Auth_RoleUserMapping  
   (RoleId,UserId,CreatedOn)  
   VALUES (@RoleId,@UserId,GETDATE())  
  
   --Inserting Data into CompanyLocationUserMapping  
   INSERT INTO CompanyLocationUserMapping  
   (CompanyLocationId,UserId,IsBaseLocation)  
   VALUES (@LocationId,@UserId,1)  
  
 
   --Inserting Data in SubscriptionPlan -----***Not Required------ 
   --INSERT INTO SubscriptionPlan  
   --(Id,PackageName,YearlyPrice,MonthlyPrice,ProductId,PackageTypeId,IsPrivate,IsActive)  
   --VALUES (@SubscriptionPlanId,'Master','500.00','60.00',@ProductId,'7356AD95-42BA-4FC0-A4FA-517F2796407E',0,1)  
  
   --Inserting Data into CustomerSubscription  ---Uproster Subscription
   INSERT INTO CustomerSubscription  
   (Id,SubscriptionPlanId,CompanyId,DomainName,StartDate,EndDate,IsActive,YearlyPrice,MonthlyPrice)  
   VALUES (NEWID(),@SubscriptionPlanId,@CompanyId,'MytestDomain',GETDATE(),'2021-12-31 00:00:00.000',1,'600.00','60.00')
   
      --Inserting Data into CustomerSubscription  ---Leemo Subscription
   INSERT INTO CustomerSubscription  
   (Id,SubscriptionPlanId,CompanyId,DomainName,StartDate,EndDate,IsActive,YearlyPrice,MonthlyPrice)  
   VALUES (NEWID(),'308FBFC0-E1E5-407C-BE1E-00F7CFBEAC99',@CompanyId,'MytestDomain',GETDATE(),'2021-12-31 00:00:00.000',1,'600.00','60.00') 
  
   --Insert Data into Auth_RoleFeatureMapping using stored procedure  
   EXEC sp_InsertNewLocationAuth_RoleFeatureMapping @UserId,@RoleId  
  
   --select @UserName as Email , 'Prastish124#' as YourPassword  
  
  -- if not error, commit the transcation  
  COMMIT TRANSACTION;  
  SET @retval = 1;  
 END TRY    
 BEGIN CATCH  
  -- if error, roll back any chanegs done by any of the sql statements  
   ROLLBACK TRANSACTION;  
   SET @retval = 0;  
 END CATCH;  
  
 SELECT @retval    
END