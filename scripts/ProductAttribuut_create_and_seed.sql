-- =============================================================================
-- Catalog filters: ProductAttribuut + ProductAttribuutItem
-- Apply manually on Azure SQL abmatic_test (Marco/DBA). Not an EF migration.
-- See docs/PLAN_CATALOG_FILTERS.md
-- =============================================================================

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

IF OBJECT_ID(N'[Products].[ProductAttribuut]', N'U') IS NULL
BEGIN
    CREATE TABLE [Products].[ProductAttribuut]
    (
        [Id]        INT            NOT NULL IDENTITY(1, 1) CONSTRAINT [PK_ProductAttribuut] PRIMARY KEY,
        [NaamEn]    NVARCHAR(250)  NOT NULL,
        [NaamNl]    NVARCHAR(250)  NOT NULL,
        [NaamFr]    NVARCHAR(250)  NOT NULL,
        [Volgorde]  INT            NOT NULL CONSTRAINT [DF_ProductAttribuut_Volgorde] DEFAULT (0)
    );
END;

IF OBJECT_ID(N'[Products].[ProductAttribuutItem]', N'U') IS NULL
BEGIN
    CREATE TABLE [Products].[ProductAttribuutItem]
    (
        [Id]                  INT            NOT NULL IDENTITY(1, 1) CONSTRAINT [PK_ProductAttribuutItem] PRIMARY KEY,
        [ProductAttribuutId]  INT            NOT NULL,
        [ProductProdId]       INT            NOT NULL,
        [Waarde]              NVARCHAR(250)  NOT NULL,
        CONSTRAINT [FK_ProductAttribuutItem_ProductAttribuut]
            FOREIGN KEY ([ProductAttribuutId]) REFERENCES [Products].[ProductAttribuut] ([Id]),
        CONSTRAINT [FK_ProductAttribuutItem_Product]
            FOREIGN KEY ([ProductProdId]) REFERENCES [Products].[Product] ([ProdId]),
        CONSTRAINT [UQ_ProductAttribuutItem_Product_Attr]
            UNIQUE ([ProductProdId], [ProductAttribuutId])
    );

    CREATE INDEX [IX_ProductAttribuutItem_ProductProdId]
        ON [Products].[ProductAttribuutItem] ([ProductProdId]);

    CREATE INDEX [IX_ProductAttribuutItem_ProductAttribuutId]
        ON [Products].[ProductAttribuutItem] ([ProductAttribuutId]);
END;

-- Seed 18 attribute definitions (idempotent by NaamEn)
;WITH Seed ([NaamEn], [Volgorde]) AS
(
    SELECT N'Power Supply', 1 UNION ALL
    SELECT N'Application Type', 2 UNION ALL
    SELECT N'Gate Type', 3 UNION ALL
    SELECT N'Maximum Gate Weight', 4 UNION ALL
    SELECT N'Maximum Gate Length', 5 UNION ALL
    SELECT N'Duty Cycle', 6 UNION ALL
    SELECT N'Motor Type', 7 UNION ALL
    SELECT N'Control Technology', 8 UNION ALL
    SELECT N'Access Control Method', 9 UNION ALL
    SELECT N'Communication Protocol', 10 UNION ALL
    SELECT N'Safety Features', 11 UNION ALL
    SELECT N'IP Protection Rating', 12 UNION ALL
    SELECT N'Frequency', 13 UNION ALL
    SELECT N'Battery Backup', 14 UNION ALL
    SELECT N'Smart Home Compatibility', 15 UNION ALL
    SELECT N'Installation Type', 16 UNION ALL
    SELECT N'Environment', 17 UNION ALL
    SELECT N'Certifications', 18
)
INSERT INTO [Products].[ProductAttribuut] ([NaamEn], [NaamNl], [NaamFr], [Volgorde])
SELECT s.[NaamEn], s.[NaamEn], s.[NaamEn], s.[Volgorde]
FROM Seed s
WHERE NOT EXISTS (
    SELECT 1
    FROM [Products].[ProductAttribuut] a
    WHERE a.[NaamEn] = s.[NaamEn]
);

COMMIT TRANSACTION;
GO
