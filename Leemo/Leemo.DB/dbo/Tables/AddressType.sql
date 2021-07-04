CREATE TABLE [dbo].[AddressType] (
    [Id]       UNIQUEIDENTIFIER CONSTRAINT [DF_AddressType_Id] DEFAULT (newid()) NOT NULL,
    [Name]     VARCHAR (20)     NOT NULL,
    [IsActive] BIT              NULL,
    CONSTRAINT [PK_AddressType] PRIMARY KEY CLUSTERED ([Id] ASC)
);



