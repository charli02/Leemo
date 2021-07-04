CREATE TABLE [dbo].[GroupGroupsMapping] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [GroupId]       UNIQUEIDENTIFIER NULL,
    [MappedGroupId] UNIQUEIDENTIFIER NULL,
    [CreatedOn]     DATETIME         DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GroupGroupsMapping_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]),
    CONSTRAINT [FK_GroupGroupsMapping_MappedGroup] FOREIGN KEY ([MappedGroupId]) REFERENCES [dbo].[Group] ([Id])
);

