# Provisório — filtros de catálogo (ProductAttribuut)

**Documento de trabalho (PT).** A especificação canónica em inglês está em [PLAN_CATALOG_FILTERS.md](./PLAN_CATALOG_FILTERS.md).

## O que o cliente pediu

- Tabela(s) novas no ERP para atributos de produto.
- Admin: cadastrar atributos e, em cada produto, escolher atributo + preencher valor.
- Loja: numa categoria folha, mostrar atributos **distintos** dos produtos dessa categoria como **checkboxes** e filtrar.
- Atributos só no produto (não na categoria).
- **Layout da loja não muda** (grid + sidebar de filtros atual).

## Schema (holandês no SQL)

| Tabela | C# |
|--------|-----|
| `[Products].[ProductAttribuut]` | `ProductAttribute` |
| `[Products].[ProductAttribuutItem]` | `ProductAttributeValue` (`Waarde`, `ProductProdId`, …) |

Fonte de produtos: `[Products].[Product]` (`ProdId`, `ProdName` / `ProdNameEN` / `ProdNameFr`).

Lista seed (18): Power Supply, Application Type, Gate Type, Maximum Gate Weight, Maximum Gate Length, Duty Cycle, Motor Type, Control Technology, Access Control Method, Communication Protocol, Safety Features, IP Protection Rating, Frequency, Battery Backup, Smart Home Compatibility, Installation Type, Environment, Certifications.

## Admin

1. `/admin/attributes` — dicionário das 18 definições.  
2. `/admin/product-attributes` — **tela dedicada**: busca produto (NL/EN/FR) → seleciona → cria filtros (`Waarde`) para a webstore.

SQL para DBA: `scripts/ProductAttribuut_create_and_seed.sql`.

## O que deixa de valer

Piloto S.7 (whitelist 54, Merk/Voorraad/Prijs, facets via `ProductProperty`) — **removido do código**.

## Estado

- Docs + SQL script + EF + admin + store: **feito**.  
- Falta Marco/DBA aplicar o script em `abmatic_test`.
