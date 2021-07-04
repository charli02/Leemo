CREATE TABLE [dbo].[GeneralCode] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_GeneralCode_Id] DEFAULT (newid()) NOT NULL,
    [GeneralCodeGroupId] UNIQUEIDENTIFIER NOT NULL,
    [CodeName]           NVARCHAR (150)   NOT NULL,
    [CodeValue]          NVARCHAR (150)   NOT NULL,
    [IsActive]           BIT              NOT NULL,
    CONSTRAINT [PK_GeneralCode] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GeneralCode_GeneralCodeGroup] FOREIGN KEY ([GeneralCodeGroupId]) REFERENCES [dbo].[GeneralCodeGroup] ([Id])
);

