CREATE TABLE [dbo].[UserProfile] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [UserId]            UNIQUEIDENTIFIER NULL,
    [FirstName]         VARCHAR (50)     NOT NULL,
    [LastName]          VARCHAR (50)     NOT NULL,
    [DateOfBirth]       DATE             NOT NULL,
    [DesignationId]     UNIQUEIDENTIFIER NULL,
    [Alias]             VARCHAR (50)     NOT NULL,
    [Phone]             VARCHAR (20)     NULL,
    [Mobile]            VARCHAR (20)     NOT NULL,
    [Fax]               VARCHAR (20)     NULL,
    [Website]           VARCHAR (200)    NULL,
    [Language]          VARCHAR (100)    NULL,
    [CountryLocale]     VARCHAR (20)     NULL,
    [DateFormat]        VARCHAR (20)     NULL,
    [TimeFormat]        VARCHAR (20)     NULL,
    [TimeZone]          VARCHAR (20)     NULL,
    [CompanyId]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]         DATETIME         CONSTRAINT [DF__UserProfi__Creat__208CD6FA] DEFAULT (getdate()) NULL,
    [ModifiedOn]        DATETIME         NULL,
    [ReportingToUserId] UNIQUEIDENTIFIER NULL,
    [Description]       VARCHAR (500)    NULL,
    [ImageName]         VARCHAR (250)    NULL,
    [CountryCode]       VARCHAR (50)     NULL,
    CONSTRAINT [PK__UserProf__3214EC07913A5601] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserProfile_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_UserProfile_Designation] FOREIGN KEY ([DesignationId]) REFERENCES [dbo].[Designation] ([Id]),
    CONSTRAINT [FK_UserProfile_ReportingTo] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_UserProfile_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);



