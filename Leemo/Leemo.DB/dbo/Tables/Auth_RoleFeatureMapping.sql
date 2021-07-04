CREATE TABLE [dbo].[Auth_RoleFeatureMapping] (
    [FeatureId]   UNIQUEIDENTIFIER NOT NULL,
    [CodeId]      UNIQUEIDENTIFIER NOT NULL,
    [RoleId]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [CreatedDate] DATETIME         NULL,
    CONSTRAINT [PK_Auth_RoleFeatureMapping] PRIMARY KEY CLUSTERED ([FeatureId] ASC, [CodeId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_Auth_ProfileFeatureMapping_Auth_FeatureCodeMapping] FOREIGN KEY ([FeatureId], [CodeId]) REFERENCES [dbo].[Auth_FeatureCodeMapping] ([FeatureId], [CodeId]),
    CONSTRAINT [FK_Auth_ProfileFeatureMapping_Profile] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Auth_Role] ([Id]),
    CONSTRAINT [FK_Auth_RoleFeatureMapping_Auth_FeatureCodeMapping] FOREIGN KEY ([FeatureId], [CodeId]) REFERENCES [dbo].[Auth_FeatureCodeMapping] ([FeatureId], [CodeId]),
    CONSTRAINT [FK_Auth_RoleFeatureMapping_Profile] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Auth_Role] ([Id])
);

