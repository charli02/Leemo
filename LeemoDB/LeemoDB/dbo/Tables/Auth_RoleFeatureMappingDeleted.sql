CREATE TABLE [dbo].[Auth_RoleFeatureMappingDeleted] (
    [FeatureId]   UNIQUEIDENTIFIER NOT NULL,
    [CodeId]      UNIQUEIDENTIFIER NOT NULL,
    [RoleId]      UNIQUEIDENTIFIER NOT NULL,
    [DeletedDate] DATETIME         NOT NULL,
    [CreatedDate] DATETIME         NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [DeletedBy]   UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Auth_RoleFeatureMappingDeleted] PRIMARY KEY CLUSTERED ([FeatureId] ASC, [CodeId] ASC, [RoleId] ASC, [DeletedDate] ASC)
);

