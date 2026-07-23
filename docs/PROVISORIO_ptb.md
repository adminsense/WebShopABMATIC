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

Lista seed (18): Power Supply, Application Type, Gate Type, Maximum Gate Weight, Maximum Gate Length, Duty Cycle, Motor Type, Control Technology, Access Control Method, Communication Protocol, Safety Features, IP Protection Rating, Frequency, Battery Backup, Smart Home Compatibility, Installation Type, Environment, Certifications.

## O que deixa de valer

Piloto S.7 (whitelist 54, Merk/Voorraad/Prijs, facets via `ProductProperty`) — **obsoleto**. Código antigo será removido na implementação; docs já descrevem só o modelo novo.

## Estado

- Docs (PLAN + SPECs alinhados): feito nesta passagem.  
- SQL / código / admin / store: ainda não.
