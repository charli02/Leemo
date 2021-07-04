CREATE TABLE [dbo].[PackageType] (
    [Id]       UNIQUEIDENTIFIER CONSTRAINT [DF_PackageType_Id] DEFAULT (newid()) NOT NULL,
    [TypeName] VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_PackageType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

