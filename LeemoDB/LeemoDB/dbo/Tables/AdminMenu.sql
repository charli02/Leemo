CREATE TABLE [dbo].[AdminMenu] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_AdminMenu_Id] DEFAULT (newid()) NOT NULL,
    [MenuName]        VARCHAR (100)    NULL,
    [SortOrder]       INT              NULL,
    [MenuAccessLevel] VARCHAR (50)     NULL,
    [IsActive]        BIT              NULL,
    CONSTRAINT [PK_AdminMenu] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A for Admin, SA for SuperAdmin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AdminMenu', @level2type = N'COLUMN', @level2name = N'MenuAccessLevel';

