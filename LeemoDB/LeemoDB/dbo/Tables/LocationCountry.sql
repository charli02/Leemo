CREATE TABLE [dbo].[LocationCountry] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [CountryName] VARCHAR (150)    NOT NULL,
    CONSTRAINT [PK_LocationCountry] PRIMARY KEY CLUSTERED ([Id] ASC)
);

