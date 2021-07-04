CREATE TABLE [dbo].[User] (
    [Id]                     UNIQUEIDENTIFIER NOT NULL,
    [UserName]               VARCHAR (200)    NOT NULL,
    [PasswordSalt]           VARCHAR (MAX)    NOT NULL,
    [PasswordHash]           VARCHAR (MAX)    NOT NULL,
    [CreatedOn]              DATETIME         DEFAULT (getdate()) NULL,
    [ModifiedOn]             DATETIME         NULL,
    [ForcePasswordReset]     BIT              DEFAULT ((1)) NULL,
    [IsActive]               BIT              DEFAULT ((0)) NOT NULL,
    [TempPasswordHash]       VARCHAR (MAX)    NULL,
    [TempPasswordSalt]       VARCHAR (MAX)    NULL,
    [TempPasswordExpiryDate] DATETIME         NULL,
    [IsFirstLogin]           BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);

