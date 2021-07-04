CREATE TABLE [dbo].[Group] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [Name]              VARCHAR (50)     NOT NULL,
    [Description]       VARCHAR (200)    NULL,
    [CreatedOn]         DATETIME         CONSTRAINT [DF_Group_CreatedOn] DEFAULT (getdate()) NULL,
    [ModifiedOn]        DATETIME         NULL,
    [ImageName]         VARCHAR (250)    NULL,
    [IsActive]          BIT              CONSTRAINT [DF_Group_IsActive] DEFAULT ((1)) NOT NULL,
    [CompanyLocationId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Group_CompanyLocation] FOREIGN KEY ([CompanyLocationId]) REFERENCES [dbo].[CompanyLocation] ([Id])
);

