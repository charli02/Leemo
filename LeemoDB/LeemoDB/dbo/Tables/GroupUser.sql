CREATE TABLE [dbo].[GroupUser] (
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [GroupId]   UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn] DATETIME         CONSTRAINT [DF_GroupUser_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_GroupUser] PRIMARY KEY CLUSTERED ([UserId] ASC, [GroupId] ASC),
    CONSTRAINT [FK_GroupUsers_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]),
    CONSTRAINT [FK_GroupUsers_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

