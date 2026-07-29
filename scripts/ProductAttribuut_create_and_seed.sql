-- ============================================================================
-- Catalog filter attributes — Dutch physical schema (DBA / Marco applies on abmatic_test)
-- App maps English C# ↔ these names via WebShopABMATICModelBuilder (DE-PARA).
-- Do NOT run Migrate() / EnsureCreated() from the webshop app.
--
-- FK: ProductAttribuutItem.ProductProdId → Products.Product.ProdId
-- FK: ProductAttribuutItem.ProductAttribuutId → ProductAttribuut.AttribuutId
--
-- If older columns (NaamEn/NaamNl/NaamFr/Volgorde) still exist, migrate/replace
-- those tables before applying this script (out of band DBA work).
-- ============================================================================

IF OBJECT_ID(N'Products.ProductAttribuut', N'U') IS NULL
BEGIN
    CREATE TABLE Products.ProductAttribuut (
        AttribuutId   INT IDENTITY(1,1) NOT NULL,
        Naam          NVARCHAR(100) NOT NULL,
        Gegevenstype  NVARCHAR(20) NULL,   -- Number / Text / Boolean
        Eenheid       NVARCHAR(20) NULL,   -- V, W, A, kg, mm...
        CONSTRAINT PK_ProductAttribuut PRIMARY KEY (AttribuutId)
    );
END
GO

IF OBJECT_ID(N'Products.ProductAttribuutItem', N'U') IS NULL
BEGIN
    CREATE TABLE Products.ProductAttribuutItem (
        Id                 INT IDENTITY(1,1) NOT NULL,
        ProductAttribuutId INT NOT NULL,
        ProductProdId      INT NOT NULL,   -- FK → Products.Product.ProdId
        Waarde             NVARCHAR(100) NOT NULL,
        CONSTRAINT PK_ProductAttribuutItem PRIMARY KEY (Id),
        CONSTRAINT FK_ProductAttribuutItem_Product
            FOREIGN KEY (ProductProdId) REFERENCES Products.Product (ProdId),
        CONSTRAINT FK_ProductAttribuutItem_Attribuut
            FOREIGN KEY (ProductAttribuutId) REFERENCES Products.ProductAttribuut (AttribuutId)
    );

    CREATE INDEX IX_ProductAttribuutItem_ProductProdId
        ON Products.ProductAttribuutItem (ProductProdId);

    CREATE INDEX IX_ProductAttribuutItem_ProductAttribuutId
        ON Products.ProductAttribuutItem (ProductAttribuutId);
END
GO

-- ============================================================================
-- Seed dictionary (18) — Naam only; Gegevenstype/Eenheid left NULL for staff/client later.
-- Idempotent: insert only when Naam is missing.
-- ============================================================================

;WITH Seed(Naam) AS (
    SELECT v.Naam
    FROM (VALUES
        (N'Power Supply'),
        (N'Application Type'),
        (N'Gate Type'),
        (N'Maximum Gate Weight'),
        (N'Maximum Gate Length'),
        (N'Duty Cycle'),
        (N'Motor Type'),
        (N'Control Technology'),
        (N'Access Control Method'),
        (N'Communication Protocol'),
        (N'Safety Features'),
        (N'IP Protection Rating'),
        (N'Frequency'),
        (N'Battery Backup'),
        (N'Smart Home Compatibility'),
        (N'Installation Type'),
        (N'Environment'),
        (N'Certifications')
    ) AS v(Naam)
)
INSERT INTO Products.ProductAttribuut (Naam, Gegevenstype, Eenheid)
SELECT s.Naam, NULL, NULL
FROM Seed s
WHERE NOT EXISTS (
    SELECT 1
    FROM Products.ProductAttribuut a
    WHERE a.Naam = s.Naam
);
GO
