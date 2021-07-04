CREATE TABLE [dbo].[MessageRequestCallback] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [FullName]       VARCHAR (150)    NOT NULL,
    [CountryCode]    VARCHAR (10)     NOT NULL,
    [PhoneNumber]    VARCHAR (50)     NOT NULL,
    [BestTimeToCall] VARCHAR (50)     NOT NULL,
    [Message]        NVARCHAR (500)   NULL,
    [SubmittedOn]    DATETIME         NOT NULL,
    [RequestDataXML] VARCHAR (250)    NULL,
    [TimeZone]       VARCHAR (250)    NULL,
    CONSTRAINT [PK_MessageRequestCallback] PRIMARY KEY CLUSTERED ([Id] ASC)
);

