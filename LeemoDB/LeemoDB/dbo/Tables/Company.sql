CREATE TABLE [dbo].[Company] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Name]          VARCHAR (250)    NOT NULL,
    [EmployeeCount] INT              NULL,
    [Email]         VARCHAR (150)    NULL,
    [Phone]         VARCHAR (20)     NULL,
    [Mobile]        VARCHAR (20)     NOT NULL,
    [Fax]           VARCHAR (20)     NULL,
    [Website]       VARCHAR (200)    NULL,
    [Description]   VARCHAR (500)    NULL,
    [Currency]      VARCHAR (10)     NULL,
    [TimeZone]      VARCHAR (10)     NULL,
    [CreatedOn]     DATETIME         CONSTRAINT [DF_Company_Createddate] DEFAULT (getdate()) NULL,
    [ModifiedOn]    DATETIME         NULL,
    [Language]      VARCHAR (200)    NULL,
    [ImageName]     VARCHAR (250)    NULL,
    [CountryCode]   VARCHAR (50)     NULL,
    [TimeFormat]    VARCHAR (20)     NULL,
    [DateFormat]    VARCHAR (20)     NULL,
    [DatabaseName]  VARCHAR (250)    NULL,
    [ApiAddress]    VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([Id] ASC)
);

