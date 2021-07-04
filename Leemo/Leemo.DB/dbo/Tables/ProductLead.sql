﻿CREATE TABLE [dbo].[ProductLead] (
    [Id]               UNIQUEIDENTIFIER CONSTRAINT [DF_ProductLead_Id] DEFAULT (newid()) NOT NULL,
    [ProductId]        UNIQUEIDENTIFIER NOT NULL,
    [ProductPackageId] UNIQUEIDENTIFIER NOT NULL,
    [Email]            VARCHAR (150)    NOT NULL,
    [Phone]            VARCHAR (50)     NOT NULL,
    [CompanyName]      VARCHAR (250)    NOT NULL,
    [IsVerified]       BIT              NOT NULL,
    [VerificationDate] DATETIME         NULL,
    [CreatedDate]      DATETIME         NOT NULL,
    [IpAddress]        VARCHAR (50)     NULL,
    [MacAddress]       VARCHAR (100)    NULL,
    [DomainName]       NVARCHAR (50)    NULL,
    [AddressLine1]     NVARCHAR (150)   NULL,
    [Street]           NVARCHAR (150)   NULL,
    [City]             NVARCHAR (20)    NULL,
    [State]            NVARCHAR (20)    NULL,
    [ZipCode]          NVARCHAR (20)    NULL,
    [Country]          NVARCHAR (20)    NULL,
    [AddressLine2]     NVARCHAR (150)   NULL,
    [Fax]              NVARCHAR (20)    NULL,
    [PasswordSalt]     NVARCHAR (MAX)   NULL,
    [PasswordHash]     NVARCHAR (MAX)   NULL,
    [FullName]         VARCHAR (50)     NULL,
    [CountryCode]      VARCHAR (50)     NULL,
    CONSTRAINT [PK_ProductLead] PRIMARY KEY CLUSTERED ([Id] ASC)
);



