CREATE TABLE [dbo].[Nlog] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Application] NVARCHAR (50)  NOT NULL,
    [Logged]      DATETIME       NOT NULL,
    [Level]       NVARCHAR (50)  NOT NULL,
    [Message]     NVARCHAR (MAX) NOT NULL,
    [Logger]      NVARCHAR (250) NULL,
    [Callsite]    NVARCHAR (MAX) NULL,
    [Exception]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.NlogDBLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

