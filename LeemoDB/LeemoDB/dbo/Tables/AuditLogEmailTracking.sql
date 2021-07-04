CREATE TABLE [dbo].[AuditLogEmailTracking] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [AuditLogID] INT NOT NULL,
    [EmailDays]  INT NULL,
    CONSTRAINT [PK_AuditLogEmailTracking] PRIMARY KEY CLUSTERED ([Id] ASC)
);

