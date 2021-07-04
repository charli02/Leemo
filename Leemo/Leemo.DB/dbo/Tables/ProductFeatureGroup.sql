CREATE TABLE [dbo].[ProductFeatureGroup] (
    [Id]               UNIQUEIDENTIFIER CONSTRAINT [DF_ProductPlugin_Id] DEFAULT (newid()) NOT NULL,
    [FeatureGroupName] VARCHAR (50)     NOT NULL,
    [ProductId]        UNIQUEIDENTIFIER NOT NULL,
    [IsActive]         BIT              NOT NULL,
    CONSTRAINT [PK_ProductPlugin] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductPlugin_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

