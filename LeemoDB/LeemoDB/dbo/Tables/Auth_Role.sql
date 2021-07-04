CREATE TABLE [dbo].[Auth_Role] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [Name]              VARCHAR (50)     NOT NULL,
    [Description]       VARCHAR (200)    NULL,
    [CreatedOn]         DATETIME         CONSTRAINT [DF__Auth_Role__Creat__03F0984C] DEFAULT (getdate()) NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOn]        DATETIME         NULL,
    [ModifiedBy]        UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              CONSTRAINT [DF__Auth_Role__IsDel__04E4BC85] DEFAULT ((0)) NOT NULL,
    [CompanyLocationId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK__Auth_Rol__3214EC0725869FD0] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Auth_Role_CompanyLocation] FOREIGN KEY ([CompanyLocationId]) REFERENCES [dbo].[CompanyLocation] ([Id])
);

