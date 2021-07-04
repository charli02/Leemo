-- =============================================
-- Author:		Harpreet Singh Matharu
-- Create date: 26-Nov-2020
-- Description:	Script for creating initial tables 
-- related to User Management module
-- =============================================

--====================================
------------Master Tables-------------
--====================================
CREATE TABLE [AddressType] (
    [Id]		UNIQUEIDENTIFIER	PRIMARY KEY,
    [Name]		VARCHAR(20)			NOT NULL,
    [IsActive]	BIT
);
INSERT INTO AddressType VALUES('65BEA0DD-5FF0-4E15-8F93-D39FEA74B9F3', 'Primary', 1)


CREATE TABLE [ProfilePermission] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [Name]			VARCHAR(20)		NOT NULL,
    [IsActive]		BIT,
	[AllowInsert]	BIT				NOT NULL	DEFAULT		0,
	[AllowUpdate]	BIT				NOT NULL	DEFAULT		0,
	[AllowDelete]	BIT				NOT NULL	DEFAULT		0,
	[AllowExport]	BIT				NOT NULL	DEFAULT		0,
	[AllowEmail]	BIT				NOT NULL	DEFAULT		0,
	[AllowDownload]	BIT				NOT NULL	DEFAULT		0,
	[CreatedOn]		DATETIME		DEFAULT		GETDATE(),
	[ModifiedOn]	DATETIME		NULL
);
--=====================================



--====================================
-----------Company tables------------
--====================================
CREATE TABLE [Company] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [Name]			VARCHAR(50)		NOT NULL,
    [EmployeeCount]	INT				NULL,
	[Phone]			VARCHAR(20)		NULL,
	[Mobile]		VARCHAR(20)		NOT NULL,
	[Fax]			VARCHAR(20)		NULL,
	[Website]		VARCHAR(200)	NULL,
	[Description]	VARCHAR(500)	NULL,
	[Currency]		VARCHAR(10)		NULL,
	[TimeZone]		VARCHAR(10)		NULL,
	[CreatedOn]		DATETIME		DEFAULT		GETDATE(),
	[ModifiedOn]	DATETIME		NULL
);


CREATE TABLE [CompanyAddress] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
	[CompanyId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_CompanyAddress_Company	FOREIGN KEY (CompanyId) REFERENCES Company(Id),
	[TypeId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_CompanyAddress_Address	FOREIGN KEY ([Type])	REFERENCES [AddressType](Id),
	[Street]		VARCHAR(200)	NOT NULL,
	[City]			VARCHAR(20)		NOT NULL,
	[State]			VARCHAR(20)		NOT NULL,
	[ZipCode]		VARCHAR(20)		NOT NULL,
	[Country]		VARCHAR(20)		NOT NULL,
	[CreatedOn]		DATETIME		DEFAULT		GETDATE(),
	[ModifiedOn]	DATETIME		NULL
);
--=====================================


--====================================
-----------Role Tables------------
--====================================

CREATE TABLE [Role] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [Name]			VARCHAR(20)		NOT NULL,
    [Description]	VARCHAR(200)	NULL,
    [IsActive]		BIT				NOT NULL	DEFAULT 0,
	[CreatedOn]		DATETIME		DEFAULT		GETDATE(),
	[ModifiedOn]	DATETIME		NULL
);

CREATE TABLE [RoleHierarchy] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [RoleId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_RoleHierarchy_Role		FOREIGN KEY (RoleId)		REFERENCES [Role](Id),
    [ParentRoleId]	UNIQUEIDENTIFIER	CONSTRAINT	FK_RoleHierarchy_ParentRole	FOREIGN KEY (ParentRoleId)	REFERENCES [Role](Id),
    [Order]			INT					NULL
);

--=====================================


--ORDER SCRIPT starts
--=========================
CREATE TABLE [User] (
    [Id]					UNIQUEIDENTIFIER	PRIMARY KEY,
    [UserName]				VARCHAR(50)			NOT NULL,
    [Email]					VARCHAR(200)		NOT NULL 	UNIQUE,
    [PasswordSalt]			VARCHAR(MAX)		NOT NULL,
    [PasswordHash]			VARCHAR(MAX)		NOT NULL,
	[IsActive] 				BIT 				NOT NULL 	DEFAULT 0,
	[ForcePasswordReset] 	BIT 				DEFAULT 	1
	[CreatedOn]				DATETIME			DEFAULT		GETDATE(),
	[ModifiedOn]			DATETIME			NULL
);

CREATE TABLE [Profile] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [Name]			VARCHAR(20)			NOT NULL,
    [Description]	VARCHAR(200)		NULL,
	[CreatedOn]		DATETIME			DEFAULT		GETDATE(),
	[CreatedBy]		UNIQUEIDENTIFIER	CONSTRAINT	FK_Profile_User				FOREIGN KEY (CreatedBy)		REFERENCES [User](Id),
	[ModifiedOn]	DATETIME			NULL,
	[ModifiedBy]	UNIQUEIDENTIFIER	CONSTRAINT	FK_Profile_UserModifiedBy	FOREIGN KEY (ModifiedBy)	REFERENCES [User](Id),
	[IsDeleted]		BIT  				DEFAULT  	0
);


CREATE TABLE [ProfilePermissionMapping] (
    [Id]					UNIQUEIDENTIFIER	PRIMARY KEY,
	[ProfileId]				UNIQUEIDENTIFIER	CONSTRAINT	FK_ProfilePermissionMapping_Profile				FOREIGN KEY (ProfileId)				REFERENCES	[Profile](Id),
	[ProfilePermissionId]	UNIQUEIDENTIFIER	CONSTRAINT	FK_ProfilePermissionMapping_ProfilePermission	FOREIGN KEY (ProfilePermissionId)	REFERENCES	[ProfilePermission](Id),
	[AllowInsert]	BIT				NULL		DEFAULT		0,
	[AllowUpdate]	BIT				NULL		DEFAULT		0,
	[AllowDelete]	BIT				NULL		DEFAULT		0,
	[AllowExport]	BIT				NULL		DEFAULT		0,
	[AllowEmail]	BIT				NULL		DEFAULT		0,
	[AllowDownload]	BIT				NULL		DEFAULT		0,
	[CreatedOn]		DATETIME		DEFAULT		GETDATE(),
	[ModifiedOn]	DATETIME		NULL
);

CREATE TABLE [ProfileUserMapping] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
	[ProfileId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_ProfileUserMapping_Profile	FOREIGN KEY (ProfileId)	REFERENCES	[Profile](Id),
	[UserId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_ProfileUserMapping_User		FOREIGN KEY (UserId)	REFERENCES [User](Id),
	[CreatedOn]		DATETIME			DEFAULT		GETDATE()
);


CREATE TABLE [UserProfile] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [UserId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_UserProfile_User		FOREIGN KEY (UserId) REFERENCES [User](Id),
    [FirstName]		VARCHAR(50)			NOT NULL,
    [LastName]		VARCHAR(50)			NOT NULL,
	[DateOfBirth]	DATE				NOT NULL,
	[RoleId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_UserProfile_Role		FOREIGN KEY (RoleId)		REFERENCES [Role](Id),
	ReportingToUserId UNIQUEIDENTIFIER	CONSTRAINT	FK_UserProfile_ReportingTo	FOREIGN KEY (UserId)			REFERENCES [User](Id),
	add [Description]	VARCHAR(500)			NULL,
	[ProfileId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_UserProfile_Profile	FOREIGN KEY (ProfileId)		REFERENCES [Profile](Id),
	[Alias]			VARCHAR(50)			NOT NULL,
	[Phone]			VARCHAR(20)			NULL,
	[Mobile]		VARCHAR(20)			NOT NULL,
	[Fax]			VARCHAR(20)			NULL,
	[Website]		VARCHAR(200)		NULL,
	[Language]		VARCHAR(100)			NULL,
	[CountryLocale]	VARCHAR(20)			NULL,
	[DateFormat]	VARCHAR(20)			NULL,
	[TimeFormat]	VARCHAR(20)			NULL,

	[TimeZone]		VARCHAR(20)			NULL,
	[CompanyId]		UNIQUEIDENTIFIER	CONSTRAINT		FK_UserProfile_Company	FOREIGN KEY (CompanyId) REFERENCES Company(Id),
	[CreatedOn]		DATETIME			DEFAULT		GETDATE(),
	[ModifiedOn]	DATETIME			NULL
);

CREATE TABLE [UserAddress] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [UserId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_UserAddress_User		FOREIGN KEY (UserId)			REFERENCES [User](Id),
	[AddressTypeId]	UNIQUEIDENTIFIER	CONSTRAINT	FK_UserAddress_Address	FOREIGN KEY ([AddressTypeId])	REFERENCES [AddressType](Id),
	[Street]		VARCHAR(200)		NOT NULL,
	[City]			VARCHAR(20)			NOT NULL,
	[State]			VARCHAR(20)			NOT NULL,
	[ZipCode]		VARCHAR(20)			NOT NULL,
	[Country]		VARCHAR(20)			NOT NULL,
	[CreatedOn]		DATETIME			DEFAULT		GETDATE(),
	[ModifiedOn]	DATETIME			NULL
);



--====================================
-----------Group Tables------------
--====================================
CREATE TABLE [Group] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [Name]			VARCHAR(50)		NOT NULL,
    [Description]	VARCHAR(200)	NULL,
	[CreatedOn]		DATETIME		DEFAULT		GETDATE(),
	[ModifiedOn]	DATETIME		NULL
);


CREATE TABLE [GroupUsers] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [UserId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_GroupUsers_User		FOREIGN KEY (UserId)	REFERENCES [User](Id),
	[GroupId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_GroupUsers_Group		FOREIGN KEY (GroupId)	REFERENCES [Group](Id),
	[CreatedOn]		DATETIME			DEFAULT		GETDATE()
);


CREATE TABLE [GroupRoles] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [RoleId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_GroupRoles_Role	FOREIGN KEY (RoleId)	REFERENCES [Role](Id),
	[GroupId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_GroupRoles_Group	FOREIGN KEY (GroupId)	REFERENCES [Group](Id),
	[CreatedOn]		DATETIME			DEFAULT		GETDATE()
);

CREATE TABLE [GroupGroupsMapping] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
	[GroupId]		UNIQUEIDENTIFIER	CONSTRAINT	FK_GroupGroupsMapping_Group			FOREIGN KEY (GroupId)		REFERENCES [Group](Id),
	[MappedGroupId]	UNIQUEIDENTIFIER	CONSTRAINT	FK_GroupGroupsMapping_MappedGroup	FOREIGN KEY (MappedGroupId)	REFERENCES [Group](Id),
	[CreatedOn]		DATETIME			DEFAULT		GETDATE()
);

--=====================================



--====================================
-----------Group Tables------------
--====================================

CREATE TABLE [ErrorLog] (
    [Id]			UNIQUEIDENTIFIER	PRIMARY KEY,
    [Path]			VARCHAR(max)		NOT NULL,
    [Description]	VARCHAR(max)		NULL,
	[CreatedOn]		DATETIME			DEFAULT		GETDATE()
);

--=====================================



CREATE TABLE [Log] (
    [Id]					UNIQUEIDENTIFIER	PRIMARY KEY,
    [TimeStamp]				DATETIME			NOT NULL,			
    [RequestId]				VARCHAR(MAX)		NULL,
    [Message]				VARCHAR(200)		NULL,
    [Type]					VARCHAR(200)		NULL,
    [Source]				VARCHAR(500)		NULL,
    [StackTrace]			VARCHAR(MAX)		NULL,
    [RequestPath]			VARCHAR(MAX)		NULL,
    [User]					VARCHAR(500)		NULL,
    [ActionDescriptor]		VARCHAR(500)		NULL,
    [IpAddress]				VARCHAR(50)			NULL
);


--=====================================

CREATE TABLE [ApiRequestLog] (
    [Id]					UNIQUEIDENTIFIER	PRIMARY KEY,
    [RequestbyUser]			VARCHAR(250)		NOT NULL,			
    [IPAddress]				VARCHAR(200)		NULL,
    [RequestDateTime]		DATETIME			NOT NULL,
    [ApiPath]				VARCHAR(MAX)		NOT NULL,
    [RequestParameters]		VARCHAR(MAX)		NULL,
    [ResponseSuccess]		BIT					NOT NULL,
    [ErrorDescription]		VARCHAR(MAX)		NULL
);