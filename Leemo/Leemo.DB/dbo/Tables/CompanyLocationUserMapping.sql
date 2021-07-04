CREATE TABLE [dbo].[CompanyLocationUserMapping] (
    [CompanyLocationId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]            UNIQUEIDENTIFIER NOT NULL,
    [IsBaseLocation]    BIT              DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CompanyLocationUserMapping] PRIMARY KEY CLUSTERED ([CompanyLocationId] ASC, [UserId] ASC),
    CONSTRAINT [FK_CompanyLocationUserMapping_CompanyLocation_CompanyLocationId] FOREIGN KEY ([CompanyLocationId]) REFERENCES [dbo].[CompanyLocation] ([Id]),
    CONSTRAINT [FK_CompanyLocationUserMapping_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

