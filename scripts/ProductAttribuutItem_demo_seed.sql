-- ============================================================================
-- Demo seed: ProductAttribuutItem (Waarde) for store facet presentation
-- DBA / Marco applies on abmatic_test. Idempotent. No schema changes.
--
-- Tables (ERD / PLAN_CATALOG_FILTERS.md):
--   ProductAttribuut.Naam     = facet group title (dictionary already seeded)
--   ProductAttribuutItem.Waarde = checkbox values (this script)
--   Gegevenstype / Eenheid    = left NULL (client fills later)
--
-- Prerequisite: scripts/ProductAttribuut_create_and_seed.sql (18 Naam including
--   Power Supply, Gate Type, Safety Features).
-- Anchor product: ProdId 11742 → same ProductStructuurId leaf.
-- ============================================================================

SET NOCOUNT ON;

DECLARE @PowerSupplyId INT = (SELECT TOP (1) AttribuutId FROM Products.ProductAttribuut WHERE Naam = N'Power Supply');
DECLARE @GateTypeId    INT = (SELECT TOP (1) AttribuutId FROM Products.ProductAttribuut WHERE Naam = N'Gate Type');
DECLARE @SafetyId      INT = (SELECT TOP (1) AttribuutId FROM Products.ProductAttribuut WHERE Naam = N'Safety Features');

IF @PowerSupplyId IS NULL OR @GateTypeId IS NULL OR @SafetyId IS NULL
BEGIN
    RAISERROR(N'Missing dictionary rows (Power Supply / Gate Type / Safety Features). Apply ProductAttribuut_create_and_seed.sql first.', 16, 1);
    RETURN;
END;

DECLARE @LeafId INT =
(
    SELECT ProductStructuurId
    FROM Products.Product
    WHERE ProdId = 11742
);

IF @LeafId IS NULL
BEGIN
    RAISERROR(N'ProdId 11742 not found or has no ProductStructuurId.', 16, 1);
    RETURN;
END;

-- Up to 6 webshop products in the leaf; always try to include 11742 first
;WITH Candidates AS
(
    -- Always include 11742 when it exists on this leaf (even if WebShop is off — still useful for admin checks)
    SELECT p.ProdId, 0 AS SortKey
    FROM Products.Product p
    WHERE p.ProdId = 11742
      AND p.ProductStructuurId = @LeafId

    UNION ALL

    SELECT p.ProdId, 1 AS SortKey
    FROM Products.Product p
    WHERE p.ProductStructuurId = @LeafId
      AND ISNULL(p.WebShop, 0) = 1   -- C# ShowOnWebshop → column WebShop
      AND p.ProdId <> 11742
),
Numbered AS
(
    SELECT
        c.ProdId,
        ROW_NUMBER() OVER (ORDER BY c.SortKey, c.ProdId) AS Rn
    FROM Candidates c
),
Leaf AS
(
    SELECT TOP (6) ProdId, Rn
    FROM Numbered
    ORDER BY Rn
),
-- One Waarde per attribute per product (admin model)
Assignments AS
(
    -- Power Supply: 24 VDC (1), 12-24 VACDC (2), 230 VAC (1+)
    SELECT
        @PowerSupplyId AS ProductAttribuutId,
        l.ProdId AS ProductProdId,
        CASE
            WHEN l.Rn = 1 THEN N'24 VDC'
            WHEN l.Rn IN (2, 3) THEN N'12-24 VACDC'
            ELSE N'230 VAC'
        END AS Waarde
    FROM Leaf l

    UNION ALL

    -- Gate Type: Sliding / Swing / Barrier
    SELECT
        @GateTypeId,
        l.ProdId,
        CASE ((l.Rn - 1) % 3)
            WHEN 0 THEN N'Sliding'
            WHEN 1 THEN N'Swing'
            ELSE N'Barrier'
        END
    FROM Leaf l

    UNION ALL

    -- Safety Features: Photocells / Safety edge / Flashing light
    SELECT
        @SafetyId,
        l.ProdId,
        CASE ((l.Rn - 1) % 3)
            WHEN 0 THEN N'Photocells'
            WHEN 1 THEN N'Safety edge'
            ELSE N'Flashing light'
        END
    FROM Leaf l
)
INSERT INTO Products.ProductAttribuutItem (ProductAttribuutId, ProductProdId, Waarde)
SELECT a.ProductAttribuutId, a.ProductProdId, a.Waarde
FROM Assignments a
WHERE EXISTS (SELECT 1 FROM Products.Product p WHERE p.ProdId = a.ProductProdId)
  AND NOT EXISTS
  (
      SELECT 1
      FROM Products.ProductAttribuutItem i
      WHERE i.ProductAttribuutId = a.ProductAttribuutId
        AND i.ProductProdId = a.ProductProdId
  );

-- Result set for Marco (facet preview)
SELECT
    a.Naam AS AttributeTitle,
    i.Waarde,
    COUNT(*) AS ProductCount
FROM Products.ProductAttribuutItem i
INNER JOIN Products.ProductAttribuut a ON a.AttribuutId = i.ProductAttribuutId
INNER JOIN Products.Product p ON p.ProdId = i.ProductProdId
WHERE p.ProductStructuurId = @LeafId
  AND ISNULL(p.WebShop, 0) = 1   -- C# ShowOnWebshop → column WebShop
  AND a.Naam IN (N'Power Supply', N'Gate Type', N'Safety Features')
GROUP BY a.Naam, i.Waarde
ORDER BY a.Naam, i.Waarde;

SELECT
    i.ProductProdId,
    a.Naam,
    i.Waarde
FROM Products.ProductAttribuutItem i
INNER JOIN Products.ProductAttribuut a ON a.AttribuutId = i.ProductAttribuutId
INNER JOIN Products.Product p ON p.ProdId = i.ProductProdId
WHERE p.ProductStructuurId = @LeafId
  AND a.Naam IN (N'Power Supply', N'Gate Type', N'Safety Features')
ORDER BY i.ProductProdId, a.Naam;
GO
