CREATE TABLE [dbo].[ErrorLog] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [TimeStamp]        DATETIME         NOT NULL,
    [RequestId]        VARCHAR (MAX)    NULL,
    [Message]          VARCHAR (200)    NULL,
    [Type]             VARCHAR (200)    NULL,
    [Source]           VARCHAR (500)    NULL,
    [StackTrace]       VARCHAR (MAX)    NULL,
    [RequestPath]      VARCHAR (MAX)    NULL,
    [User]             VARCHAR (500)    NULL,
    [ActionDescriptor] VARCHAR (500)    NULL,
    [IpAddress]        VARCHAR (50)     NULL,
    [LogType]          VARCHAR (20)     NULL,
    [ProjectSource]    VARCHAR (10)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

