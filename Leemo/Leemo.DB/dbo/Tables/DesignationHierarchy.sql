CREATE TABLE [dbo].[DesignationHierarchy] (
    [DesignationId]       UNIQUEIDENTIFIER NOT NULL,
    [ParentDesignationId] UNIQUEIDENTIFIER NOT NULL,
    [SortOrder]           INT              NOT NULL,
    CONSTRAINT [PK_DesignationHierarchy] PRIMARY KEY CLUSTERED ([DesignationId] ASC, [ParentDesignationId] ASC),
    CONSTRAINT [FK_DesignationHierarchy_Designation] FOREIGN KEY ([DesignationId]) REFERENCES [dbo].[Designation] ([Id]),
    CONSTRAINT [FK_DesignationHierarchy_DesignationParentKey] FOREIGN KEY ([ParentDesignationId]) REFERENCES [dbo].[Designation] ([Id])
);

