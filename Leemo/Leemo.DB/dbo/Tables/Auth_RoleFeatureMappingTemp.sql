CREATE TABLE [dbo].[Auth_RoleFeatureMappingTemp] (
    [FeatureId]   UNIQUEIDENTIFIER NOT NULL,
    [CodeId]      UNIQUEIDENTIFIER NOT NULL,
    [RoleId]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATETIME         NOT NULL,
    [SessionId]   UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              NOT NULL,
    CONSTRAINT [PK_Auth_RoleFeatureMappingTemp] PRIMARY KEY CLUSTERED ([FeatureId] ASC, [CodeId] ASC, [RoleId] ASC, [SessionId] ASC)
);

