CREATE TABLE [dbo].[Auth_FeatureCodeMapping] (
    [FeatureId] UNIQUEIDENTIFIER NOT NULL,
    [CodeId]    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Auth_FeatureCodeMapping] PRIMARY KEY CLUSTERED ([FeatureId] ASC, [CodeId] ASC),
    CONSTRAINT [FK_Auth_FeatureCodeMapping_Auth_Feature] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Feature] ([Id]),
    CONSTRAINT [FK_Auth_FeatureCodeMapping_GeneralCode] FOREIGN KEY ([CodeId]) REFERENCES [dbo].[GeneralCode] ([Id])
);

