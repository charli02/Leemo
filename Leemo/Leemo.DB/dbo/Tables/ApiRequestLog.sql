CREATE TABLE [dbo].[ApiRequestLog] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [RequestByUser]     VARCHAR (250)    NOT NULL,
    [IPAddress]         VARCHAR (200)    NULL,
    [RequestDateTime]   DATETIME         NOT NULL,
    [ApiPath]           VARCHAR (MAX)    NOT NULL,
    [RequestParameters] VARCHAR (MAX)    NULL,
    [ResponseSuccess]   BIT              NOT NULL,
    [ErrorDescription]  VARCHAR (MAX)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

