﻿CREATE TABLE [dbo].[Addresses] (
    [Id]            UNIQUEIDENTIFIER CONSTRAINT [DF_Addresses_Id] DEFAULT (newid()) NOT NULL,
    [ReferenceId]   UNIQUEIDENTIFIER NULL,
    [AddressTypeId] UNIQUEIDENTIFIER NULL,
    [AddressLine1]  VARCHAR (200)    NULL,
    [AddressLine2]  VARCHAR (100)    NULL,
    [Street]        VARCHAR (200)    NOT NULL,
    [City]          VARCHAR (150)    NOT NULL,
    [State]         VARCHAR (150)    NOT NULL,
    [ZipCode]       VARCHAR (20)     NOT NULL,
    [Country]       VARCHAR (150)    NOT NULL,
    [Phone]         VARCHAR (50)     NULL,
    [CreatedOn]     DATETIME         NULL,
    [ModifiedOn]    DATETIME         NULL,
    [CountryCode]   NVARCHAR (20)    NULL,
    [Fax]           NVARCHAR (20)    NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Addresses_AddressType] FOREIGN KEY ([AddressTypeId]) REFERENCES [dbo].[AddressType] ([Id])
);

