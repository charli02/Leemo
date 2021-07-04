CREATE TABLE [dbo].[CompanyProductMapping] (
    [CompanyId]  UNIQUEIDENTIFIER NOT NULL,
    [ProductId]  UNIQUEIDENTIFIER NOT NULL,
    [DomainName] VARCHAR (150)    NULL,
    CONSTRAINT [PK_CompanyProductMapping] PRIMARY KEY CLUSTERED ([CompanyId] ASC, [ProductId] ASC),
    CONSTRAINT [FK_CompanyProductMapping_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_CompanyProductMapping_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

