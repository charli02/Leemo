CREATE TABLE [dbo].[SubscriptionPlanResource] (
    [SubscriptionPlanId] UNIQUEIDENTIFIER NOT NULL,
    [ResourceId]         UNIQUEIDENTIFIER NOT NULL,
    [ResourceValue]      VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_PackageResource] PRIMARY KEY CLUSTERED ([SubscriptionPlanId] ASC, [ResourceId] ASC),
    CONSTRAINT [FK_PackageResource_Package] FOREIGN KEY ([SubscriptionPlanId]) REFERENCES [dbo].[SubscriptionPlan] ([Id]),
    CONSTRAINT [FK_PackageResource_Resource] FOREIGN KEY ([ResourceId]) REFERENCES [dbo].[Resource] ([Id])
);

