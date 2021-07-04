CREATE TABLE [dbo].[Product] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_Product_Id] DEFAULT (newid()) NOT NULL,
    [ProductName]     VARCHAR (50)     NOT NULL,
    [IsLocationBased] BIT              NULL,
    [SortOrder]       INT              NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id] ASC)
);

