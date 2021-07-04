CREATE TABLE [dbo].[GeneralCodeGroup] (
    [Id]        UNIQUEIDENTIFIER CONSTRAINT [DF_GeneralCodeGroup_Id] DEFAULT (newid()) NOT NULL,
    [GroupName] VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_GeneralCodeGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);

