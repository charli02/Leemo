CREATE TABLE [dbo].[Designation] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_Designation_Id] DEFAULT (newid()) NOT NULL,
    [Name]              VARCHAR (250)    NOT NULL,
    [Description]       VARCHAR (200)    NULL,
    [IsActive]          BIT              CONSTRAINT [DF_Designation_IsActive] DEFAULT ((0)) NOT NULL,
    [CreatedOn]         DATETIME         CONSTRAINT [DF_Designation_CreatedOn] DEFAULT (getdate()) NULL,
    [ModifiedOn]        DATETIME         NULL,
    [CompanyLocationId] UNIQUEIDENTIFIER NOT NULL,
    [IsRoot]            BIT              CONSTRAINT [DF_Designation_IsRoot] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Designation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Designation_CompanyLocation] FOREIGN KEY ([CompanyLocationId]) REFERENCES [dbo].[CompanyLocation] ([Id])
);

