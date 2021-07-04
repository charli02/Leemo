CREATE TABLE [dbo].[Audit] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [TableName]   VARCHAR (150)    NOT NULL,
    [PK]          VARCHAR (500)    NOT NULL,
    [FieldName]   VARCHAR (150)    NOT NULL,
    [OldValue]    NVARCHAR (250)   NULL,
    [NewValue]    NVARCHAR (250)   NULL,
    [UpdateDate]  DATETIME         NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NULL,
    [RequestData] NVARCHAR (1000)  NULL,
    CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

