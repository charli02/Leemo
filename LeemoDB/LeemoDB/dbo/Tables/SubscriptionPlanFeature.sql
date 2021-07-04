CREATE TABLE [dbo].[SubscriptionPlanFeature] (
    [SubscriptionPlanId] UNIQUEIDENTIFIER NOT NULL,
    [FeatureId]          UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PackagePlugin] PRIMARY KEY CLUSTERED ([SubscriptionPlanId] ASC, [FeatureId] ASC),
    CONSTRAINT [FK_PackageFeature_Feature] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Feature] ([Id]),
    CONSTRAINT [FK_PackagePlugin_Package] FOREIGN KEY ([SubscriptionPlanId]) REFERENCES [dbo].[SubscriptionPlan] ([Id])
);

