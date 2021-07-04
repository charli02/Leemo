CREATE TABLE [dbo].[MessageEmailLog] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [EmailLogTypeId] UNIQUEIDENTIFIER NULL,
    [Email]          VARCHAR (250)    NULL,
    [Message]        VARCHAR (MAX)    NULL,
    [CompnayId]      UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_MessageEmailLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MessageEmailLog_MessageEmailLogType] FOREIGN KEY ([EmailLogTypeId]) REFERENCES [dbo].[MessageEmailLogType] ([Id])
);

