CREATE TABLE [dbo].[SubscriptionPlan] (
    [Id]            UNIQUEIDENTIFIER CONSTRAINT [DF_ProductPackage_Id] DEFAULT (newid()) NOT NULL,
    [PackageName]   VARCHAR (150)    NOT NULL,
    [YearlyPrice]   DECIMAL (10, 2)  NULL,
    [MonthlyPrice]  DECIMAL (10, 2)  NULL,
    [ProductId]     UNIQUEIDENTIFIER NULL,
    [PackageTypeId] UNIQUEIDENTIFIER NULL,
    [IsPrivate]     BIT              NULL,
    [IsActive]      BIT              NULL,
    CONSTRAINT [PK_ProductPackage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Package_PackageType] FOREIGN KEY ([PackageTypeId]) REFERENCES [dbo].[PackageType] ([Id]),
    CONSTRAINT [FK_Package_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

