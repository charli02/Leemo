CREATE TABLE [dbo].[GroupDesignationMapping] (
    [DesignationId] UNIQUEIDENTIFIER NOT NULL,
    [GroupId]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]     DATETIME         CONSTRAINT [DF_GroupRole_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_GroupDesignationMapping] PRIMARY KEY CLUSTERED ([DesignationId] ASC, [GroupId] ASC),
    CONSTRAINT [FK_GroupDesignationMapping_Designation] FOREIGN KEY ([DesignationId]) REFERENCES [dbo].[Designation] ([Id]),
    CONSTRAINT [FK_GroupRoles_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id])
);

