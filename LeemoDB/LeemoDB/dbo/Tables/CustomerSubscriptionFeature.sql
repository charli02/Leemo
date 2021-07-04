CREATE TABLE [dbo].[CustomerSubscriptionFeature] (
    [CustomerSubscriptionId] UNIQUEIDENTIFIER NOT NULL,
    [SubscriptionPlanId]     UNIQUEIDENTIFIER NOT NULL,
    [FeatureId]              UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_CustomerSubscriptionFeature] PRIMARY KEY CLUSTERED ([CustomerSubscriptionId] ASC, [SubscriptionPlanId] ASC, [FeatureId] ASC),
    CONSTRAINT [FK_CustomerSubscriptionFeature_CustomerSubscription] FOREIGN KEY ([CustomerSubscriptionId]) REFERENCES [dbo].[CustomerSubscription] ([Id]),
    CONSTRAINT [FK_CustomerSubscriptionFeature_SubscriptionPlanFeature] FOREIGN KEY ([SubscriptionPlanId], [FeatureId]) REFERENCES [dbo].[SubscriptionPlanFeature] ([SubscriptionPlanId], [FeatureId])
);

