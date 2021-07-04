CREATE TABLE [dbo].[HostingInfo] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_HostingInfo_Id] DEFAULT (newid()) NOT NULL,
    [Host]            VARCHAR (150)    NOT NULL,
    [DockerContainer] VARCHAR (150)    NOT NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_HostingInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

