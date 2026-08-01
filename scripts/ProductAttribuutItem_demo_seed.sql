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
--
-- Store CD4: facets ONLY on a TRUE leaf (ProductStructuur with no children).
-- ProdId 11742 may sit on a PARENT (e.g. Id 17). This script walks to a
-- descendant leaf that has WebShop products and seeds Waarde THERE.
-- Parent nodes show child tiles only — never the filter sidebar.
--
-- After run, open the printed URL: /?categoryId=<LeafId>
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

DECLARE @StartId INT =
(
    SELECT ProductStructuurId
    FROM Products.Product
    WHERE ProdId = 11742
);

IF @StartId IS NULL
BEGIN
    RAISERROR(N'ProdId 11742 not found or has no ProductStructuurId.', 16, 1);
    RETURN;
END;

-- True leaf under @StartId (or @StartId itself if already a leaf), preferring most WebShop products
DECLARE @LeafId INT;

;WITH Tree AS
(
    SELECT s.Id
    FROM Products.ProductStructuur s
    WHERE s.Id = @StartId

    UNION ALL

    SELECT c.Id
    FROM Products.ProductStructuur c
    INNER JOIN Tree t ON c.ParentId = t.Id
),
Leaves AS
(
    SELECT t.Id
    FROM Tree t
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM Products.ProductStructuur ch
        WHERE ch.ParentId = t.Id
    )
)
SELECT TOP (1) @LeafId = l.Id
FROM Leaves l
INNER JOIN Products.Product p ON p.ProductStructuurId = l.Id
WHERE ISNULL(p.WebShop, 0) = 1   -- C# ShowOnWebshop → column WebShop
GROUP BY l.Id
ORDER BY COUNT(*) DESC, l.Id;

IF @LeafId IS NULL
BEGIN
    RAISERROR(N'No true leaf with WebShop products under ProductStructuurId of ProdId 11742.', 16, 1);
    RETURN;
END;

DECLARE @LeafNameNl NVARCHAR(250) =
(
    SELECT TOP (1) NaamNl FROM Products.ProductStructuur WHERE Id = @LeafId
);

PRINT N'Demo leaf Id = ' + CAST(@LeafId AS NVARCHAR(20)) + N' (' + ISNULL(@LeafNameNl, N'?') + N')';
PRINT N'Open store: /?categoryId=' + CAST(@LeafId AS NVARCHAR(20));

-- Up to 6 webshop products on the TRUE leaf only
;WITH Candidates AS
(
    SELECT p.ProdId, 1 AS SortKey
    FROM Products.Product p
    WHERE p.ProductStructuurId = @LeafId
      AND ISNULL(p.WebShop, 0) = 1
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
Assignments AS
(
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

-- URL reminder (result set 1)
SELECT
    @LeafId AS OpenCategoryId,
    @LeafNameNl AS LeafNameNl,
    N'/?categoryId=' + CAST(@LeafId AS NVARCHAR(20)) AS StoreUrlPath;

-- Facet preview on the leaf (result set 2)
SELECT
    a.Naam AS AttributeTitle,
    i.Waarde,
    COUNT(*) AS ProductCount
FROM Products.ProductAttribuutItem i
INNER JOIN Products.ProductAttribuut a ON a.AttribuutId = i.ProductAttribuutId
INNER JOIN Products.Product p ON p.ProdId = i.ProductProdId
WHERE p.ProductStructuurId = @LeafId
  AND ISNULL(p.WebShop, 0) = 1
  AND a.Naam IN (N'Power Supply', N'Gate Type', N'Safety Features')
GROUP BY a.Naam, i.Waarde
ORDER BY a.Naam, i.Waarde;

-- Per-product rows (result set 3)
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
