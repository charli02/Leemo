CREATE TABLE [dbo].[LocationCity] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [CityName] VARCHAR (150)    NOT NULL,
    [StateId]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_LocationCity] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LocationCity_LocationState] FOREIGN KEY ([StateId]) REFERENCES [dbo].[LocationState] ([Id])
);

