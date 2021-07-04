CREATE TABLE [dbo].[Feature] (
    [Id]                    UNIQUEIDENTIFIER CONSTRAINT [DF_Auth_Feature_Id] DEFAULT (newid()) NOT NULL,
    [FeatureName]           NVARCHAR (150)   NOT NULL,
    [IsActive]              BIT              NOT NULL,
    [AdminMenuId]           UNIQUEIDENTIFIER NULL,
    [ProductFeatureGroupId] UNIQUEIDENTIFIER NULL,
    [IsLocationBased]       BIT              NULL,
    CONSTRAINT [PK_Auth_Feature] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Auth_Feature_AdminMenu] FOREIGN KEY ([AdminMenuId]) REFERENCES [dbo].[AdminMenu] ([Id]),
    CONSTRAINT [FK_Feature_ProductFeatureGroup] FOREIGN KEY ([ProductFeatureGroupId]) REFERENCES [dbo].[ProductFeatureGroup] ([Id])
);

