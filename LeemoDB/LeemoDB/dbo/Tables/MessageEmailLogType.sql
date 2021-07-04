CREATE TABLE [dbo].[MessageEmailLogType] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Name]          VARCHAR (50)     NOT NULL,
    [EmailSentDays] INT              NOT NULL,
    [Template]      VARCHAR (150)    NULL,
    CONSTRAINT [PK_MessageEmailLogType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

