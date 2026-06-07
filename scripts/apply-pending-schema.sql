/* WebShopABMATIC — apply pending schema changes (idempotent)
   Database: WebShopABMATIC
   Run: sqlcmd -S MULLER -E -d WebShopABMATIC -i scripts\apply-pending-schema.sql
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

USE [WebShopABMATIC];
GO

PRINT N'Applying domain schema patches...';

-- [Projects].[OrderAdvancePayments] — Mollie payment columns
IF COL_LENGTH(N'Projects.OrderAdvancePayments', N'MollieCheckoutUrl') IS NULL
    ALTER TABLE [Projects].[OrderAdvancePayments] ADD [MollieCheckoutUrl] nvarchar(500) NULL;
IF COL_LENGTH(N'Projects.OrderAdvancePayments', N'MolliePaidAt') IS NULL
    ALTER TABLE [Projects].[OrderAdvancePayments] ADD [MolliePaidAt] datetime2 NULL;
IF COL_LENGTH(N'Projects.OrderAdvancePayments', N'MolliePaymentId') IS NULL
    ALTER TABLE [Projects].[OrderAdvancePayments] ADD [MolliePaymentId] nvarchar(50) NULL;
IF COL_LENGTH(N'Projects.OrderAdvancePayments', N'MolliePaymentStatus') IS NULL
    ALTER TABLE [Projects].[OrderAdvancePayments] ADD [MolliePaymentStatus] nvarchar(30) NULL;
GO

-- [Customers].[Customers] — link to ASP.NET Identity user
IF COL_LENGTH(N'Customers.Customers', N'IdentityUserId') IS NULL
    ALTER TABLE [Customers].[Customers] ADD [IdentityUserId] nvarchar(450) NULL;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = N'IX_Customers_IdentityUserId'
      AND object_id = OBJECT_ID(N'[Customers].[Customers]')
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_IdentityUserId]
        ON [Customers].[Customers] ([IdentityUserId])
        WHERE [IdentityUserId] IS NOT NULL;
END
GO

PRINT N'Applying application / Identity schema patches...';

-- [AspNetUsers].[CustomerId] — reverse link to domain customer
IF OBJECT_ID(N'[dbo].[AspNetUsers]', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.AspNetUsers', N'CustomerId') IS NULL
    ALTER TABLE [dbo].[AspNetUsers] ADD [CustomerId] int NULL;
GO

-- [AuditLogs]
IF OBJECT_ID(N'[dbo].[AuditLogs]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[AuditLogs]
    (
        [Id] bigint IDENTITY(1,1) NOT NULL,
        [Timestamp] datetime2 NOT NULL,
        [Action] nvarchar(50) NOT NULL,
        [EntityName] nvarchar(100) NOT NULL,
        [EntityId] nvarchar(50) NULL,
        [IdentityUserId] nvarchar(450) NULL,
        [LegacyStaffUserId] int NULL,
        [UserDisplayName] nvarchar(256) NOT NULL,
        [Severity] nvarchar(20) NOT NULL,
        [Success] bit NOT NULL,
        [ErrorMessage] nvarchar(max) NULL,
        [IpAddress] nvarchar(45) NULL,
        [UserAgent] nvarchar(512) NULL,
        [OldValues] nvarchar(max) NULL,
        [NewValues] nvarchar(max) NULL,
        [DurationMs] int NULL,
        [AdditionalInfo] nvarchar(max) NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id])
    );

    CREATE INDEX [IX_AuditLogs_Action_Success] ON [dbo].[AuditLogs] ([Action], [Success]);
    CREATE INDEX [IX_AuditLogs_EntityName_EntityId] ON [dbo].[AuditLogs] ([EntityName], [EntityId]);
    CREATE INDEX [IX_AuditLogs_IdentityUserId_Timestamp] ON [dbo].[AuditLogs] ([IdentityUserId], [Timestamp]);
    CREATE INDEX [IX_AuditLogs_Timestamp] ON [dbo].[AuditLogs] ([Timestamp]);
END
GO

-- [StockLowAlerts]
IF OBJECT_ID(N'[dbo].[StockLowAlerts]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[StockLowAlerts]
    (
        [Id] bigint IDENTITY(1,1) NOT NULL,
        [ProductStockLocationId] int NOT NULL,
        [ProductId] int NOT NULL,
        [ProductName] nvarchar(256) NOT NULL,
        [StockLocationId] int NOT NULL,
        [StockLocationName] nvarchar(256) NOT NULL,
        [Quantity] decimal(18,4) NOT NULL,
        [MinQuantity] decimal(18,4) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsRead] bit NOT NULL,
        [ReadAt] datetime2 NULL,
        CONSTRAINT [PK_StockLowAlerts] PRIMARY KEY CLUSTERED ([Id])
    );

    CREATE INDEX [IX_StockLowAlerts_CreatedAt] ON [dbo].[StockLowAlerts] ([CreatedAt]);
    CREATE INDEX [IX_StockLowAlerts_IsRead_CreatedAt] ON [dbo].[StockLowAlerts] ([IsRead], [CreatedAt]);
    CREATE INDEX [IX_StockLowAlerts_ProductStockLocationId] ON [dbo].[StockLowAlerts] ([ProductStockLocationId]);
END
GO

-- Keep EF migration history in sync when tables/columns were applied via SQL
IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20260606141224_OrderAdvancePaymentMollieColumns')
        INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260606141224_OrderAdvancePaymentMollieColumns', N'8.0.0');

    IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20260606144352_CustomerIdentityUserId')
        INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260606144352_CustomerIdentityUserId', N'8.0.0');

    IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20260606144406_ApplicationUserCustomerId')
        INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260606144406_ApplicationUserCustomerId', N'8.0.0');

    IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20260607132504_AuditLogs')
        INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260607132504_AuditLogs', N'8.0.0');

    IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20260607144427_StockLowAlerts')
        INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260607144427_StockLowAlerts', N'8.0.0');
END
GO

PRINT N'Pending schema applied successfully.';
GO
