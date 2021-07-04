CREATE TABLE [dbo].[LocationState] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [StateName] VARCHAR (150)    NOT NULL,
    [CountryId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_LocationState] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LocationState_LocationCountry] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[LocationCountry] ([Id])
);

