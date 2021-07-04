CREATE TYPE [dbo].[MessageEmailLogTableType] AS TABLE (
    [Id]             UNIQUEIDENTIFIER NULL,
    [EmailLogTypeId] UNIQUEIDENTIFIER NULL,
    [Email]          VARCHAR (250)    NULL,
    [Message]        VARCHAR (250)    NULL,
    [CompanyId]      UNIQUEIDENTIFIER NULL);

