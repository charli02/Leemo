CREATE TABLE [dbo].[CustomerSubscription] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_CustomerSubscription_Id] DEFAULT (newid()) NOT NULL,
    [SubscriptionPlanId] UNIQUEIDENTIFIER NOT NULL,
    [CompanyId]          UNIQUEIDENTIFIER NOT NULL,
    [DomainName]         VARCHAR (150)    NULL,
    [StartDate]          DATETIME         NOT NULL,
    [EndDate]            DATETIME         NOT NULL,
    [IsActive]           BIT              NOT NULL,
    [YearlyPrice]        DECIMAL (10, 2)  NOT NULL,
    [MonthlyPrice]       DECIMAL (10, 2)  NOT NULL,
    CONSTRAINT [PK_CustomerSubscription] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CompanyPackageSubscription_Package] FOREIGN KEY ([SubscriptionPlanId]) REFERENCES [dbo].[SubscriptionPlan] ([Id]),
    CONSTRAINT [FK_CompanyProductPluginMapping_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);

