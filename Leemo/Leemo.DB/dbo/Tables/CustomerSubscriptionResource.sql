CREATE TABLE [dbo].[CustomerSubscriptionResource] (
    [CustomerSubscriptionId] UNIQUEIDENTIFIER NOT NULL,
    [SubscriptionPlanId]     UNIQUEIDENTIFIER NOT NULL,
    [ResourceId]             UNIQUEIDENTIFIER NOT NULL,
    [ResourceValue]          VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_CustomerSubscriptionResource] PRIMARY KEY CLUSTERED ([CustomerSubscriptionId] ASC, [SubscriptionPlanId] ASC, [ResourceId] ASC),
    CONSTRAINT [FK_CustomerSubscriptionResource_CustomerSubscription] FOREIGN KEY ([CustomerSubscriptionId]) REFERENCES [dbo].[CustomerSubscription] ([Id]),
    CONSTRAINT [FK_CustomerSubscriptionResource_SubscriptionPlanResource] FOREIGN KEY ([SubscriptionPlanId], [ResourceId]) REFERENCES [dbo].[SubscriptionPlanResource] ([SubscriptionPlanId], [ResourceId])
);

