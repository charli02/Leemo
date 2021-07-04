CREATE TABLE [dbo].[ResourceGroup] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_ResourceGroup_Id] DEFAULT (newid()) NOT NULL,
    [GroupName] VARCHAR (80)     NOT NULL,
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [IsActive]  BIT              NOT NULL,
    [SortOrder] INT              NOT NULL,
    CONSTRAINT [PK_ResourceGroup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResourceGroup_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

