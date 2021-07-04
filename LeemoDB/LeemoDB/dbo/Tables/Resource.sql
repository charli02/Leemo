CREATE TABLE [dbo].[Resource] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_Resource_Id] DEFAULT (newid()) NOT NULL,
    [ResourceName]    VARCHAR (80)     NOT NULL,
    [ListOfValuesXML] VARCHAR (250)    NULL,
    [ResourceGroupId] UNIQUEIDENTIFIER NOT NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Resource_ResourceGroup] FOREIGN KEY ([ResourceGroupId]) REFERENCES [dbo].[ResourceGroup] ([Id])
);

