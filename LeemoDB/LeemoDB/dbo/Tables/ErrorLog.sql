CREATE TABLE [dbo].[ErrorLog] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [TimeStamp]        DATETIME         NOT NULL,
    [RequestId]        VARCHAR (MAX)    NULL,
    [Message]          VARCHAR (MAX)    NULL,
    [Type]             VARCHAR (200)    NULL,
    [Source]           VARCHAR (500)    NULL,
    [StackTrace]       VARCHAR (MAX)    NULL,
    [RequestPath]      VARCHAR (MAX)    NULL,
    [User]             VARCHAR (500)    NULL,
    [ActionDescriptor] VARCHAR (500)    NULL,
    [IpAddress]        VARCHAR (50)     NULL,
    [LogType]          VARCHAR (20)     NULL,
    [ProjectSource]    VARCHAR (10)     NULL,
    CONSTRAINT [PK__ErrorLog__3214EC07A54520CA] PRIMARY KEY CLUSTERED ([Id] ASC)
);

