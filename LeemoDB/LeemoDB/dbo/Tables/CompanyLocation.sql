CREATE TABLE [dbo].[CompanyLocation] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [CompanyId]    UNIQUEIDENTIFIER NOT NULL,
    [LocationName] NVARCHAR (MAX)   NOT NULL,
    [IsHeadOffice] BIT              NULL,
    [IsActive]     BIT              NULL,
    [CreatedOn]    DATETIME         CONSTRAINT [DF_CompanyLocation_CreatedOn] DEFAULT (getdate()) NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedOn]   DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CompanyLocation_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);

