CREATE TABLE [dbo].[Auth_RoleUserMapping] (
    [RoleId]    UNIQUEIDENTIFIER NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn] DATETIME         CONSTRAINT [DF_Auth_Role_Createddate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Auth_RoleUserMapping] PRIMARY KEY CLUSTERED ([RoleId] ASC, [UserId] ASC),
    CONSTRAINT [FK_RoleUserMapping_Profile] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Auth_Role] ([Id]),
    CONSTRAINT [FK_RoleUserMapping_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

