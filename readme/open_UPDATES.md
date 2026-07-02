# WebShop — Migração de layout da loja

![Status](https://img.shields.io/badge/Status-Fases%20B–D%20implementadas-28a745?style=flat-square) ![Referência](https://img.shields.io/badge/Referência-adminsenceweb-512BD4?style=flat-square) ![Idioma](https://img.shields.io/badge/Loja-English-0dcaf0?style=flat-square)

> [!IMPORTANT]
> **Resumo:** Migrar o **layout da loja** para ficar igual à referência que o cliente aprovou: **[https://adminsenceweb.azurewebsites.net/](https://adminsenceweb.azurewebsites.net/)**. Mantém-se a arquitectura hexagonal (`Application` → ports → `Infrastructure`); mudam UI/CSS da webstore e contratos de catálogo.

**Próximo passo sugerido:** **C.5 + D.5** (navegação por níveis — prioridade pelo screenshot); depois C.4 + D.4 (homepage); E + F; §3.5 e §2.2.2 em paralelo.

---

## 1. Referência única

| Item | Valor |
|------|--------|
| **URL a seguir** | https://adminsenceweb.azurewebsites.net/ |
| **App** | `Adminsence.Shop` — Blazor Server |
| **Loja actual (código)** | Sidebar + header OK; homepage com **New products** + grelha genérica (≠ referência) |
| **Loja alvo** | Homepage = **grelha ícones categorias** + secção **Deals** + sidebar árvore `ProductStructuur` — **inglês** |

Toda a análise visual e decisões de UI partem **só** desta URL.

---

## 2. Fase A — Análise concluída ✅

### 2.1 Layout da referência (extraído de CSS/JS públicos)

A app de referência usa **Blazor Server + Bootstrap 4 + DevExpress (tema office-white) + BlazorStrap**. Elementos relevantes para a nossa loja:

| Elemento | Referência (`adminsenceweb`) | Nossa loja actual |
|----------|------------------------------|-------------------|
| **Shell** | `height: 100%`, `overflow: hidden` — app a preencher viewport | Scroll livre, sem shell fixo |
| **Header** | Navbar branca, `3.5rem`, sombra `0 2px 6px rgba(0,0,0,.12)` | `StoreHeader` sticky, azul claro, search centrada |
| **Sidebar** | **300px** fixa, sombra lateral, `nav-pills`, fonte **600** | ❌ Não existe — chips horizontais |
| **Conteúdo** | Padding `1.1rem 2rem`, scroll interno, `max-width: 1100px` | `.container` 1200px, hero + secções |
| **Cor primária** | **Laranja** `#fe7109` / `#ff6c00` (DevExpress) | Azul `#3b82f6` |
| **Tipografia** | Segoe UI / Helvetica Neue, `0.88rem` base | DM Sans + Instrument Serif |
| **Produtos** | Carousel Bootstrap (5 itens por slide, `Navbar.js`) | Grelha CSS `minmax(240px, 1fr)` |
| **Ícones** | Font Awesome 6 | Bootstrap Icons + SVG search |
| **Mobile** | Sidebar colapsa &lt; 1200px, overlay full-width | Chips wrap, header flex |

**Nota:** A URL live devolve o shell Blazor mas o circuito pode falhar no browser anónimo; a análise acima veio de `Adminsence.Shop.styles.css`, `css/site.css` e `js/Navbar.js` servidos pela mesma app.

### 2.1.1 Header navigation — barra superior (screenshot cliente)

A referência mostra uma **barra horizontal única** (fundo branco, ícone + texto por item). Na app holandesa do cliente os rótulos são *Home*, *Winkelkar*, *Contact*, *Zoeken*, *Login*; na nossa implementação usamos **inglês** com a **mesma ordem e estrutura**.

**Ordem alvo (esquerda → direita):**

| # | Referência (NL) | Alvo (EN) | Ícone (FA6 sugerido) | Acção / rota |
|---|-----------------|-----------|----------------------|--------------|
| 1 | ← (voltar) | **Back** | `fa-arrow-left` | `NavigationManager` history back; se não houver histórico → `/` |
| 2 | Home | **Home** | `fa-house` | `/` (catálogo) |
| 3 | Winkelkar | **Cart** | `fa-cart-shopping` | `/cart` — opcional badge com contagem |
| 4 | Contact | **Contact** | `fa-compass` (ou `fa-envelope`) | `/contact` — página estática v1 (ver H7) |
| 5 | — | **Search** (campo) | — | Input texto; filtra catálogo / dispara pesquisa |
| 6 | Zoeken | **Search** (botão) | `fa-magnifying-glass` | Submete pesquisa (mesmo comportamento que Enter no campo) |
| 7 | Login | **Login** | `fa-right-to-bracket` | `/sign-in` (`data-enhance-nav="false"`) |

**Estado autenticado (comportamento nosso — não visível no screenshot anónimo):**

| Estado | Item 7 no header | Itens extra |
|--------|------------------|-------------|
| Guest | **Login** → `/sign-in` | — |
| Customer signed in | Nome abreviado ou **Account** → `/my-account` | **Sign out** (texto ou ícone); **My orders** pode ficar em `/orders` via conta, não na barra |
| Staff (cookie legacy) | Igual customer se entrar pela loja | Link **Admin** **fora** desta barra — URL directa `/admin/login` apenas |

**Comparação com `StoreHeader.razor` actual:**

| Aspecto | Referência (screenshot) | Nossa loja actual | Gap |
|---------|-------------------------|-------------------|-----|
| Logo “WebShop” | ❌ Não aparece na barra | ✅ Logo centrado/esquerda | **Remover logo** da navbar ou mover para sidebar/footer |
| Links principais | Back · Home · Cart · Contact | Catalog · My orders · My account · Cart | **Substituir** por ordem da referência |
| Pesquisa | Campo + botão “Search” à direita | Barra centrada, só Enter | **Separar** input + botão Search |
| Login | “Login” com ícone, extremo direito | “Sign in” + link Admin | **Login** EN; **remover Admin** da barra pública |
| Estilo links | Ícone + texto, sem pills coloridos | `.nav-btn` pills azuis | Links simples, hover sublinhado ou cor laranja |
| Tipografia header | Sistema (Segoe UI), ~0.88rem | DM Sans | Alinhar §2.1 |
| Altura / sombra | 3.5rem, sombra suave | Sticky, tema azul | Navbar branca §2.1 |

**Wireframe alvo (inglês):**

```text
┌──────────────────────────────────────────────────────────────────────────────┐
│  ← Back   🏠 Home   🛒 Cart   🧭 Contact          [ Search... ]  🔍 Search  Login │
└──────────────────────────────────────────────────────────────────────────────┘
```

**Decisões de UI — header (Fase A):**

| # | Questão | Decisão |
|---|---------|---------|
| H1 | Ordem dos itens | **Back → Home → Cart → Contact → Search field → Search button → Login** |
| H2 | Texto login | **Login** (não “Sign in”) na navbar — página continua `/sign-in` |
| H3 | Logo WebShop | **Fora da navbar** na v1 (sidebar ou conteúdo); não competir com a referência |
| H4 | Catalog / My orders na barra | **Remover** — Home = catálogo; orders via conta pós-login |
| H5 | Admin na barra | **Remover** da loja pública |
| H6 | Ícones | **Font Awesome 6** (CDN como referência) ou Bootstrap Icons equivalentes |
| H7 | Página Contact | **Nova** `Contact.razor` em `/contact` — conteúdo estático EN (empresa, email, telefone de `Settings.BaseCompany` ou placeholder até port existir) |
| H8 | Back | `NavigationManager` back; fallback `/` |
| H9 | Cart badge | Manter contagem no ícone Cart se couber visualmente |
| H10 | Mobile &lt; 1200px | Colapsar para menu hamburger **ou** wrap com prioridade: Home, Cart, Login visíveis; Search em linha 2 — *detalhar em implementação* |

**Textos alvo (inglês) — navbar:**

| Control | Texto |
|---------|--------|
| Back | Back |
| Home | Home |
| Cart | Cart |
| Contact | Contact |
| Search placeholder | Search products… |
| Search button | Search |
| Login (guest) | Login |
| Sign out (auth) | Sign out |

### 2.1.2 Homepage — estrutura da tela inicial (screenshot cliente)

> [!NOTE]
> **Fonte:** screenshot homepage da referência (NL: *Categorien*, *Deals*). **Estado:** análise para implementação futura — o `Catalog.razor` actual **não** replica esta estrutura.

A página inicial da referência não é uma grelha única de produtos. É um **painel de três zonas** com conteúdo principal em **duas secções empilhadas**.

#### Arquitectura visual (3 zonas)

```text
┌─────────────────────────────────────────────────────────────────────────────┐
│ HEADER: Home · Search · Zoeken · Login                                        │
├──────────────┬──────────────────────────────────────────────────────────────┤
│   SIDEBAR    │  MAIN (scroll vertical, fundo branco)                       │
│   ~300px     │                                                              │
│              │  ┌─ Categories ─────────────────────────────────────────┐   │
│  ▶ Cat A     │  │  [□ icon] [□ icon] [□ icon] ...  (grelha quadrada)   │   │
│  ▶ Cat B     │  │  [□ icon] [□ icon] ...        (~16 cartões raiz)    │   │
│    Cat B1    │  └──────────────────────────────────────────────────────┘   │
│  ▶ Cat C     │                                                              │
│  ...         │  ┌─ Deals ──────────────────────────────────────────────┐   │
│  (lista      │  │ [foto][foto][foto][foto][foto][foto][foto][foto]    │   │
│   densa,     │  │  TÍTULO  título  título ...   (fila horizontal ~8)  │   │
│   chevron    │  │  🛒      🛒      🛒   ...  (quick-add laranja)        │   │
│   expand)    │  │  descrição curta por produto                         │   │
│              │  └──────────────────────────────────────────────────────┘   │
└──────────────┴──────────────────────────────────────────────────────────────┘
```

#### Zona 1 — Header (já analisado §2.1.1)

Na imagem: **Home**, campo de pesquisa, **Zoeken** (Search), **Login** — sem logo, sem hero.

#### Zona 2 — Sidebar esquerda (navegação completa)

| Aspecto | Referência (screenshot) | Nossa loja (`StoreCategorySidebar`) | Gap |
|---------|-------------------------|-------------------------------------|-----|
| Posição | Fixa à esquerda, altura total abaixo do header | ✅ `store-sidebar` 300px | — |
| Conteúdo | Lista **longa** de categorias + subcategorias | Árvore `ProductStructuur` expandível | ✅ alinhado |
| Expandir | Chevron `>` por item com filhos | ✅ `CategoryNode` toggle | — |
| Densidade | Texto pequeno, muitas entradas visíveis | Fonte ~0.95rem, peso 600 | 🟡 Ajustar tamanho/espacamento |
| Acção | Clique navega para categoria / filtro | `/?categoryId=` | ✅ |
| “All products” | Não visível no screenshot (pode estar no topo) | Link explícito | 🟡 Confirmar se mantemos |

A sidebar é a **navegação profunda**; a homepage mostra só um **resumo visual** das categorias principais.

#### Zona 3a — Secção **Categories** (categorias principais na main)

| Aspecto | Referência | Nossa loja | Gap |
|---------|------------|------------|-----|
| Título secção | **Categorien** | ❌ Não existe | **Categories** (EN) |
| Layout | Grelha de **cartões quadrados** com borda cinza fina | ❌ | `category-tile-grid` |
| Quantidade | ~**16** cartões (2 linhas: ~11 + ~5 centrados) | — | Raízes `ProductStructuur` com produtos webshop |
| Conteúdo cartão | **Ícone P&B** + etiqueta por baixo | — | Ícone de `ProductStructure.Icon` (blob) ou fallback |
| Acção | Clique → catálogo dessa categoria | Sidebar faz isto; main não | `/?categoryId={rootId}` |
| vs sidebar | Atalho visual das **raízes** | Só sidebar lista categorias | **Falta grelha na homepage** |

**Componente alvo:** `StoreCategoryTile.razor` — quadrado, ícone centrado, `NameEn`, hover borda/sombra laranja.

#### Zona 3b — Secção **Deals** (produtos em promoção)

| Aspecto | Referência | Nossa loja | Gap |
|---------|------------|------------|-----|
| Título secção | **Deals** | **New products** (≠ mesmo conceito) | Renomear / separar secções |
| Layout | **Fila horizontal** ~8 cartões (não grelha 4 colunas) | Grelha `product-grid` | `deals-row` ou carousel |
| Imagem | Foto real do produto (placas, caixas) | ✅ `ImageUrl` blob | — |
| Título produto | **MAIÚSCULAS**, negrito (ex. BRAINY24, HEADY, THINKY) | Title case normal | Estilo `.deal-card-title` |
| Quick-add | Ícone **carrinho laranja** no cartão | Só no detalhe / sem ícone no card | `Add to cart` rápido no card |
| Descrição | 2–3 linhas texto técnico sob o título | Descrição no card genérico | Manter no deal card |
| Preço | Não destacado na imagem (pode ser “on request”) | Preço na grelha actual | 🟡 Confirmar com cliente |

**Nota:** *Deals* na referência **não é necessariamente** `IsNieuw` — pode ser promoções, destaques manuais ou outro critério ERP. Ver decisão **HP4**.

#### Comparação homepage — referência vs `Catalog.razor` actual

| Bloco | Referência | Implementado hoje |
|-------|------------|-------------------|
| Hero | ❌ | ❌ (removido ✅) |
| Chips horizontais | ❌ | ❌ (removido ✅) |
| Grelha ícones categorias | ✅ **Categories** | ❌ |
| Secção promoção | ✅ **Deals** (~8 produtos, fila) | 🟡 **New products** (12, grelha) — **substituir/ajustar** |
| Grelha produtos genérica na `/` | ❌ (só deals + tiles) | ✅ 12 produtos — **remover ou mover** para `?categoryId=` |
| Sidebar árvore | ✅ | ✅ |

#### Decisões de UI — homepage (Fase A+)

| # | Questão | Decisão / estudo |
|---|---------|------------------|
| HP1 | Estrutura `/` | **Categories (tiles)** → **Deals (row)**; sem grelha genérica na raiz |
| HP2 | Títulos EN | **Categories** + **Deals** (não “Categorien” / “New products”) |
| HP3 | Ícones categorias | `ProductStructure.Icon` (byte[] ERP); fallback ícone genérico por categoria |
| HP4 | Fonte dos Deals | ⏳ Confirmar cliente: flag promoção ERP, lista fixa, `IsNieuw`, ou tabela dedicada |
| HP5 | Quantidade Deals | **~8** na homepage (screenshot); configurável |
| HP6 | Quantidade tiles | **Raízes** com `Webshop`+produtos (28 max.; screenshot ~16) — só com ícone ou todas |
| HP7 | Quick-add no deal | Carrinho laranja no card → `StoreCartService.Add` sem ir ao detalhe (se preço OK) |
| HP8 | Após clicar tile | `/?categoryId=` + opcional cabeçalho categoria §3.5 |
| HP9 | Deals responsivo | Desktop: fila horizontal scroll; mobile: carousel ou stack — alinhar `Navbar.js` ref. |
| HP10 | New products | ⏳ Fundir com Deals, secção separada abaixo, ou remover — **confirmar cliente** |

#### Implementação prevista (fases)

| Camada | Itens |
|--------|--------|
| **C.4** | `GetRootCategoriesForHomeAsync()` com ícone URL; `GetDealsAsync(take)` com critério HP4 |
| **D.4** | `StoreCategoryTileGrid.razor`, `StoreDealCard.razor`, refactor `Catalog.razor` (2 secções) |
| **CSS** | `.category-tile-grid`, `.deal-card`, `.deals-row`, ícone quick-add laranja |

**Textos alvo (inglês) — homepage:**

| Control | Texto |
|---------|--------|
| Secção categorias | Categories |
| Secção promoção | Deals |
| Tile (aria) | `{CategoryName}` — browse products |

#### Dúvidas e checklists — homepage (§2.1.2)

**HP3 — Ícones das categorias**
- ❓ **Dúvida:** `ProductStructure.Icon` (byte[] no ERP) tem formato consistente? Servimos como data-URL, endpoint dedicado, ou ignoramos nós sem ícone?
- Checklist:
  - [ ] Amostrar quantos nós raiz têm `Icon` não nulo em `abmatic_test`
  - ✅ Definir fallback visual (ícone genérico por `Level` ou placeholder SVG)
  - ✅ Endpoint ou helper media para ícones de estrutura (separado de `AzureFile` produto)

**HP4 — Fonte dos Deals**
- ❓ **Dúvida:** O que define um produto em **Deals** na referência — promoção ERP, lista manual, `IsNieuw`, ou outro campo?
- Checklist:
  - ✅ Confirmado com cliente: `ProdNonActive == true` significa produto desativado e não deve aparecer na loja
  - [ ] Mapear campo legacy se existir (ex. flag promoção em `Product`)
  - [ ] Implementar `GetDealsAsync(take)` com critério acordado
  - ✅ Verificado no código: deals usam `QueryVisibleProducts()` com `ShowOnWebshop == true` e `!IsInactive` (`ProdNonActive`)

**HP6 — Quantidade de tiles na homepage**
- ❓ **Dúvida:** Mostramos as **28 raízes** todas ou só as ~16 com ícone / com produtos?
- Checklist:
  - [ ] Contar raízes com `ProductCount > 0` na BD
  - ✅ Decisão cliente: mostrar categorias mesmo sem ícone (menu + cards/submenus), com fallback visual quando não houver ícone
  - ✅ Layout CSS para 2 linhas centradas (como screenshot)

**HP10 — New products vs Deals**
- ❓ **Dúvida:** Substituímos **New products** por **Deals**, mantemos ambos, ou `IsNieuw` alimenta Deals?
- Checklist:
  - [ ] Decisão cliente documentada em §10
  - ✅ Remover secção New products do `Catalog.razor` após decisão
  - [ ] Actualizar `GetNewProductsAsync` se ficar obsoleto

**Grelha genérica na `/`**
- ❓ **Dúvida:** Na homepage sem `categoryId`, mostramos **zero** produtos na grelha (só tiles + deals)?
- Checklist:
  - ✅ Remover `GetCatalogAsync(12)` da `/` sem query
  - ✅ Produtos só em `?categoryId=` (modo filho ou folha — ver §2.1.3)
  - ✅ Pesquisa `?q=` continua a listar produtos

### 2.1.3 Navegação por níveis — categoria → subcategorias → produtos (screenshot cliente)

> [!NOTE]
> **Fonte:** screenshot ao clicar categoria *Bedieningen, signaallampen, accessoires, toegangscontrole* (NL). **Estado:** ✅ **implementado** (2026-06-26). `/?categoryId=` mostra **tiles dos filhos directos** quando o nó tem filhos; na **folha** mostra produtos com `ProductStructuurId` igual ao nó (regra CD4 — sem agregar descendentes).

#### Comportamento observado (referência)

Ao clicar num item do **menu lateral** ou num **cartão de categoria** (homepage ou nível anterior):

1. **Sidebar** — categoria activa com **fundo laranja** e texto branco; filhos **indentados** visíveis abaixo; restantes raízes listadas mais abaixo; chevrons nos nós com mais níveis.
2. **Main** — **título H1** = nome da categoria seleccionada (EN: mesmo texto que sidebar activa).
3. **Main** — **grelha de cartões** dos **filhos directos** (ex. ~19 cartões: *Handzenders*, *Batterijen*, *Accessoires*, …) — ícone/imagem + etiqueta, borda cinza.
4. **Não** mostra ainda a grelha de produtos neste ecrã — mostra o **nível intermédio** da árvore.

```text
Clique: sidebar "Cat A" OU tile "Cat A" na homepage
         │
         ▼
┌──────────────┬──────────────────────────────────────────────┐
│ SIDEBAR      │ MAIN                                          │
│ [Cat A] ◀──  │  H1: Cat A (full name)                        │
│  orange bg   │  ┌────┐ ┌────┐ ┌────┐ ┌────┐ ...               │
│   › Child 1  │  │icon│ │icon│ │icon│     (filhos directos)   │
│   › Child 2  │  │ C1 │ │ C2 │ │ C3 │                         │
│   › Child 3  │  └────┘ └────┘ └────┘                         │
│ ...          │  (sem grelha de produtos ainda)               │
│ Other roots  │                                               │
└──────────────┴──────────────────────────────────────────────┘

Clique: Child 2 (sem filhos OU folha na BD)
         │
         ▼
┌──────────────┬──────────────────────────────────────────────┐
│ SIDEBAR      │ MAIN                                          │
│ [Cat A]      │  H1: Child 2                                  │
│   [Child 2]  │  [product] [product] [product] ...            │
│     orange   │  (grelha produtos directos do nó — CD4)       │
└──────────────┴──────────────────────────────────────────────┘
```

#### Comparação — referência vs implementação actual

| Aspecto | Referência | Nós hoje (`Catalog.razor` + sidebar) | Gap |
|---------|------------|----------------------------------------|-----|
| Clique sidebar / tile | Próximo nível = **cartões filhos** | ✅ `/?categoryId=` mostra **subcategorias** quando o nó tem filhos; produtos só na folha | ✅ Alinhado com regra CD4 |
| Item activo sidebar | Fundo **laranja** + texto branco | ✅ Classe `.active` com `background: #fe7109` e texto branco | ✅ Contraste ajustado |
| Expandir filhos | Auto-expandir ramo activo | ✅ `ExpandAncestors` expande o ramo activo | ✅ `scrollIntoView` do item activo implementado |
| Título main | H1 = nome categoria | ✅ `StoreCategoryHeader` exibe H1 com nome da categoria na main | ✅ Concluído (intro/form segue em §3.5) |
| Conteúdo main (nó com filhos) | Grelha **subcategorias** | ✅ Grelha de `Children` (`categories-grid`) | ✅ Alinhado |
| Conteúdo main (folha) | Grelha **produtos** | ✅ `GetCatalogAsync` — só produtos directos (`ProductStructureId == categoryId`) | ✅ |
| Deals na `/` | Só na homepage | ✅ Homepage com secções **Categories** + **Deals** | ✅ Alinhado |
| URL | Provável `/category/{id}` ou query | ✅ Mantido `/?categoryId=` (decisão v1) | ✅ Rota dedicada fica para fase futura |

**Dados BD (Fase A):** árvore `ProductStructuur` com **máx. 2 níveis** abaixo da raiz — navegação típica: **Raiz → filhos (tiles) → produtos** (ou raiz com filhos que também têm filhos → 3 cliques até produtos).

#### Regra de navegação fechada (árvore `ProductStructuur`)

| Situação do nó `categoryId` | O que mostrar na **main** | Sidebar |
|----------------------------|---------------------------|---------|
| Tem **filhos** com produtos webshop na subárvore | Grelha **cartões dos filhos directos** (`Children` do DTO), sem mostrar produtos ainda | Activar nó; expandir filhos |
| **Sem filhos** (folha) | Grelha **produtos** desse nó — **apenas** `ProductStructuurId` = nó (sem descendentes) | Activar folha |
| Raiz na **homepage** `/` | Secções **Categories** (tiles raiz) + **Deals** | Nenhum activo ou “All products” |
| Pesquisa `?q=` | Grelha produtos (resultados) | Sem mudança de activo |

> ✅ **Regra CD4 fechada:** mostrar **cards/submenus de categorias** até à folha; a grelha de produtos aparece **apenas** quando não houver filhos.

#### Decisões — navegação por níveis (Fase A+)

| # | Questão | Decisão / estudo |
|---|---------|------------------|
| CD1 | Modo da página | `BrowseMode`: `Home` \| `Subcategories` \| `Products` \| `Search` |
| CD2 | Trigger subcategorias | Se `Children.Count > 0` no nó → modo **Subcategories** |
| CD3 | Trigger produtos | Se folha (`Children` vazio) → modo **Products** |
| CD4 | Produtos no nó intermédio | ✅ Só tiles/submenus até folha; produtos apenas na folha |
| CD5 | Sidebar activo | Classe `.store-nav-link--active-branch` (laranja sólido #fe7109, texto branco) |
| CD6 | Tile = sidebar | Mesmo `categoryId` — tile homepage, tile main, e link sidebar usam mesma rota |
| CD7 | Rota | Manter `/?categoryId=` **ou** `/category/{id}` — preferir query na v1 (menos mudanças) |
| CD8 | Breadcrumb | Opcional fase 2: Raiz › … › actual |
| CD9 | Texto intro categoria | H1 + parágrafo intro (`IntroPriceListTextId`) abaixo do título — §3.5 |
| CD10 | Formulário por categoria | Se existir na referência, abaixo do H1 antes dos tiles — §3.5 |

#### Dúvidas e checklists — navegação por níveis (§2.1.3)

**CD2 / CD3 — Quando mostrar tiles vs produtos**
- ❓ **Dúvida:** Filho sem subfilhos mas com 0 produtos — mostramos tile vazio ou escondemos?
- Checklist:
  - ✅ Filtrar `Children` onde `ProductCount > 0` (aplicado no browse do `Catalog.razor`)
  - ✅ Confirmado com cliente: categorias vazias aparecem com o texto “No products”
  - ✅ Teste funcional: nó só com filhos vazios → mensagem “No products” na main

**CD4 — Produtos no mesmo nó que tem filhos**
- ✅ **Regra fechada:** mostrar só tiles/submenus quando houver filhos; produtos apenas na folha.
- Checklist:
  - ✅ Query em `abmatic_test` (2026-06-26): **4** nós intermédios com **16** produtos webshop directos deixam de aparecer na grelha (ex.: id 17 *Signaallampen* — 7; id 17183 — 6; id 36 — 2; id 27 — 1). Acesso via filhos na árvore.
  - ✅ Decisão registada nesta secção (§2.1.3)
  - ✅ `GetCatalogAsync` ajustado: `HasStructuralChildren` → `[]`; folha filtra `ProductStructureId == categoryId` (`StoreCatalogService.GetCatalogCoreAsync`)

**CD5 — Estilo activo sidebar (laranja)**
- ❓ **Dúvida:** Activar só o nó clicado ou também o ancestral com fundo diferente?
- Checklist:
  - ✅ CSS `.store-nav-link.active` → `background: #fe7109; color: #fff`
  - ✅ Filhos indentados com fundo branco / hover laranja claro
  - [ ] Comparar lado a lado com screenshot cliente

**CD6 — Sincronização tile ↔ sidebar**
- ❓ **Dúvida:** Ao clicar tile na main, sidebar faz scroll até o item activo?
- Checklist:
  - ✅ `categoryId` na URL actualiza sidebar e main no mesmo render
  - ✅ `EnsureExpandedPath` inclui ramo activo
  - ✅ `scrollIntoView` no item activo (JS interop)

**CD7 — Rota e histórico Back**
- ❓ **Dúvida:** Botão **Back** do header volta nível acima na árvore ou só `history.back()`?
- Checklist:
  - ✅ Definido: Back com `categoryId` = `ParentId` do nó actual
  - ✅ Raiz activa → Back vai para `/`
  - ✅ Implementado em `StoreHeader` (fallback para `history.back()` fora do browse de categorias)

**CD9 — Textos intro da categoria**
- ❓ **Dúvida:** `IntroPriceListTextId` aponta para que tabela de textos no Azure legacy?
- Checklist:
  - [ ] Mapear entidade/texto no model builder
  - [ ] `GetCategoryDetailAsync` devolve HTML ou plain text
  - [ ] Render seguro na main (sem XSS)

#### Implementação prevista (fases C.5 + D.5)

| Camada | Item | Ficheiro / notas |
|--------|------|------------------|
| **Application** | `StoreCategoryBrowseDto` — `Mode`, `CategoryName`, `IntroText?`, `Children`, `Products?` | Novo DTO |
| **Application** | `GetCategoryBrowseAsync(categoryId?)` | Estender `IStoreCatalogPort` |
| **Infrastructure** | Resolver modo CD2/CD3; filhos com contagens; produtos só em folha | ✅ `StoreCatalogService.GetCatalogCoreAsync` + `CatalogCategoryTree.HasStructuralChildren` |
| **Client** | `Catalog.razor` — ramo por `BrowseMode` (home / subcats / products / search) | ✅ Ramificação CD2/CD3; refactor formal `BrowseMode` opcional |
| **Client** | `StoreCategoryHeader.razor` — H1 + intro | Novo |
| **Client** | Reutilizar `StoreCategoryTileGrid` para filhos | D.4 + D.5 |
| **Client** | `StoreCategorySidebar` — estilo activo laranja CD5 | CSS + estado |
| **CSS** | `.store-nav-link--active`, `.category-browse-grid` | `store.css` |

**Checklist implementação global (C.5 + D.5):**

- [ ] Port `GetCategoryBrowseAsync` + testes unitários (opcional — lógica já em `Catalog.razor` + `GetCatalogAsync`)
- ✅ Homepage `/` não chama modo Products por defeito
- ✅ `?categoryId=raiz` → tiles filhos (se tiver filhos)
- ✅ `?categoryId=folha` → grelha produtos (directos)
- ✅ `?categoryId=intermédio` → tiles filhos; **sem** produtos directos do nó (CD4)
- ✅ Sidebar sincronizada com URL em todos os modos
- ✅ Clique tile homepage = clique sidebar (mesma URL)
- [ ] Regressão: pesquisa `?q=`, deals na home, imagens blob

### 2.2 Comparação com código actual

| Ficheiro | Estado actual | Gap |
|----------|---------------|-----|
| `StoreLayout.razor` | Header + `@Body` sem sidebar | Precisa grid **sidebar + main** |
| `StoreHeader.razor` | Logo, search, nav pills azuis | Reescrever §2.1.1: Back·Home·Cart·Contact·Search·Login |
| `Catalog.razor` | Homepage com Categories + Deals; `?categoryId=` com drill-down (tiles até folha; produtos directos na folha — CD4) | Testes E + regressão pesquisa/deals |
| `store.css` | Tema azul claro, sem `.sidebar` | Novo tema laranja/branco + componentes sidebar + `.store-auth-*` |
| `StoreCatalogService` | Lista plana de raízes; fallback `WebshopStructures` | Árvore completa só `ProductStructuur` |
| `SignIn.razor` / `SignUp.razor` | Card azul, serif, `StoreLayout` | `StoreAuthLayout`, card branco/laranja, copy EN (§2.2.1) |

Páginas store existentes (mantêm fluxo, só herdam novo layout): `ProductDetail`, `Cart`, `Orders`, `SignIn`, `SignUp`, `MyAccount`, `OrderDetail`, `OrderConfirmation`, `OrderPaymentReturn`.

### 2.2.1 Login / conta — referência vs nossa loja

A referência expõe rotas `/login` e `/sign-in` (ambas servem o shell Blazor). Não há CSS scoped dedicado a `Login.razor` em `Adminsence.Shop.styles.css` — o login usa o **mesmo shell da loja** (header branco 3.5rem) e estilos **Bootstrap 4 + DevExpress** (botão primário laranja, `form-control` 0.88rem).

| Elemento | Referência (`adminsenceweb`) | Nossa loja actual (`SignIn.razor`) |
|----------|------------------------------|-------------------------------------|
| **Layout** | `MainLayout` + `Header` — **sem sidebar** na área de auth; conteúdo centrado na main | `StoreLayout` → header + `@Body` (também sem sidebar hoje) |
| **Rota** | `/login` (e variantes) | `/sign-in` (manter; opcional alias `/login` → redirect) |
| **Card** | Card branco, sombra suave DevExpress/Bootstrap | `.auth-card` — border-radius 16px, Instrument Serif no título |
| **Título** | Tipografia sistema (Segoe UI), peso 600–700 | `Welcome` — serif decorativa (fora da referência) |
| **Campos** | Login + password + remember | ✅ Login or email, Password, Remember me |
| **Botão** | `.btn-primary` laranja `#fe7109`, largura total | Azul `#3b82f6`, largura total |
| **Registo** | Provável página ou tab separada (shell igual) | `/sign-up` — página separada, mesmo `.auth-wrap` |
| **Header quando anónimo** | **Login** com ícone no extremo direito (§2.1.1) | `StoreHeader` → “Sign in” + Admin — **alinhar** |
| **SSR / POST** | Form HTTP (padrão Blazor Server) | ✅ `App.razor` static em `/sign-in`; POST `/account/store-login` |
| **Staff login** | Fora do âmbito da loja de referência | Link “Staff login (admin)” no rodapé — **ocultar ou mover** para alinhar só com a loja cliente |
| **Idioma** | UI da loja em inglês (fase actual) | ✅ Inglês nos textos; manter |

**Comportamento de auth (não muda na migração visual):**

| Fluxo | Endpoint / página | Notas |
|-------|-------------------|--------|
| Sign in | `SignIn.razor` → POST `/account/store-login` | Legacy `WebshopLogin` + password |
| Sign up | `SignUp.razor` → `ICustomerRegistrationPort` | Auto sign-in após registo |
| My account | `MyAccount.razor` | `[Authorize CustomerOnly]` — formulário perfil + password |
| Redirect se já autenticado | `SignIn.razor` `OnInitialized` | Customer → `returnUrl` ou `/` |
| Checkout / orders | `/sign-in?returnUrl=…` | Manter query `returnUrl` |

**Decisões de UI — login (Fase A):**

| # | Questão | Decisão |
|---|---------|---------|
| L1 | Sidebar no login? | **Não** — só header branco + conteúdo centrado (como referência) |
| L2 | Layout dedicado auth | Novo `StoreAuthLayout.razor`: header + `.store-auth-main` (sem sidebar, sem hero) |
| L3 | Páginas que usam auth layout | `/sign-in`, `/sign-up`; `MyAccount` usa shell loja **com** header, card largo na main |
| L4 | Paleta formulários | Laranja `#fe7109`, inputs Bootstrap, fonte Segoe UI — alinhar `store.css` |
| L5 | Tabs Sign in / Register num ecrã | **Não na v1** — manter duas rotas; estilo visual partilhado (mock interno tinha tabs; referência não confirma) |
| L6 | Alias `/login` | **Sim** — redirect 302 para `/sign-in` (compatibilidade com URL do cliente) |
| L7 | Link admin no rodapé do sign-in | **Remover** da UI loja pública; staff continua via `/admin/login` |
| L8 | Copy em inglês (sign-in) | Ver tabela abaixo |

**Textos alvo (inglês) — sign-in / sign-up:**

| Control | Texto alvo |
|---------|------------|
| Page title | Sign in |
| Heading | Sign in |
| Subtitle | Sign in with your webshop account to place orders and view order history. |
| Login field | Login or email |
| Password | Password |
| Remember | Remember me |
| Submit | Sign in |
| Footer | Create account · Back to catalog |
| Sign-up heading | Create account |
| Sign-up footer | Already have an account? Sign in |

### 2.2.2 Login unificado loja ↔ admin (evolução — análise)

> [!NOTE]
> **Estado:** só documentado — **não implementado**. A v1 mantém `/sign-in` (cliente) e `/admin/login` (staff) separados.

Na referência do cliente, **Login** na navbar é o ponto único de entrada. No nosso ecossistema já existe o **admin Blazor** (`/admin/*`, `LegacySignInService`, cookie `.WebShopABMATIC.Auth`, roles Admin/Manager/Customer). A evolução pretendida é **um único fluxo de login** a partir do botão **Login** da loja, ligado ao app admin já pronto.

| Aspecto | Hoje (v1) | Alvo (evolução) |
|---------|-----------|-----------------|
| Entrada UI | **Login** → `/sign-in` (só cliente) | **Login** → página/rota **única** de autenticação |
| Backend staff | `POST /account/admin-login` | Mesmo `LegacySignInService` — valida `Instellingen.User` |
| Backend cliente | `POST /account/store-login` | Mesmo serviço — valida `Klanten.Klant` / `WebshopLogin` |
| Pós-login staff | Manual `/admin/login` ou URL directa | **Redirect automático** → `/admin` (ou `returnUrl` admin) |
| Pós-login cliente | `/` ou `returnUrl` loja | Mantém redirect loja (`/cart`, `/orders`, …) |
| Cookie / circuit | `LegacyCookieAuthentication` + `LegacyAuthenticationStateProvider` | **Reutilizar** — uma sessão para loja e admin |
| Admin já pronto | `AdminLayout`, hubs, policies `AdminOrManager` | Após login staff, utilizador **entra directamente** no painel sem segundo formulário |
| Sign-up cliente | `/sign-up` separado | Mantém-se à parte (só role Customer) |

**Fluxo alvo (mermaid):**

```mermaid
flowchart LR
  A[Navbar Login] --> B[SignIn unificado]
  B --> C{LegacySignInService}
  C -->|StaffUser| D[/admin]
  C -->|Customer| E[Loja / returnUrl]
  C -->|Falha| B
```

**Decisões em estudo (login unificado):**

| # | Questão | Direcção provável |
|---|---------|-------------------|
| U1 | Rota única | `/login` ou `/sign-in` com **um** formulário; deprecar login duplicado na UI |
| U2 | Endpoint POST | Unificar em `POST /account/login` **ou** manter dois endpoints mas **uma** página que escolhe internamente |
| U3 | Ordem de resolução | Tentar staff **ou** customer (definir: email partilhado, prioridade staff vs cliente) |
| U4 | Navbar após staff login | Opcional link **Admin** só quando role Admin/Manager — ou redirect silencioso na próxima acção |
| U5 | L7 (rodapé sem staff link) | Mantém-se na v1; unificação **substitui** a necessidade de `/admin/login` público na loja |

**Ficheiros prováveis (fase posterior):** `SignIn.razor`, `LoginEndpoints` / `MapLoginEndpoints`, `StoreHeader.razor`, `LegacySignInService`, `Program.cs` (redirect `/login`), `AdminLogin.razor` (redundante ou redirect).

#### Dúvidas e checklists — login unificado (§2.2.2)

**U3 — Mesmo email em staff e cliente**
- ❓ **Dúvida:** Se `admin@…` existir em `StaffUsers` e `Klant`, qual login ganha?
- Checklist:
  - [ ] Listar colisões reais em `abmatic_test`
  - [ ] Documentar prioridade (staff primeiro vs cliente primeiro)
  - [ ] Teste de integração para ambos os cenários

**U2 — Um endpoint ou dois**
- ❓ **Dúvida:** Unificar POST num só endpoint simplifica Azure SSR ou preferimos página única com dois forms ocultos?
- Checklist:
  - [ ] Rever `MapLoginEndpoints` actual
  - [ ] Prototipar `POST /account/login` com ramo staff/customer
  - [ ] Regressão sign-in estático (`App.razor` static rendering)

**U4 — Staff na navbar após login**
- ❓ **Dúvida:** Mostrar link **Admin** na loja quando Admin/Manager, ou só redirect na primeira sessão?
- Checklist:
  - [ ] Decisão UX com cliente
  - [ ] `StoreHeader` condicional por role
  - [ ] Política: customer nunca vê link admin

### 2.3 Validação de dados (`abmatic_test`)

Consulta em 2026-06-25 à BD de teste:

| Métrica | Valor | Implicação |
|---------|-------|------------|
| Categorias raiz (`ParentId` null/0) | **28** | Sidebar com 28 entradas de topo |
| Nós totais `ProductStructuur` | **204** | Árvore com 2 níveis (`Level` máx. 2) |
| Produtos visíveis (`Webshop=1`, `ProdNonActive=0`) | **581** | Catálogo real substancial |
| Produtos `IsNieuw=1` (visíveis) | **67** | Secção New products tem stock |
| Produtos com `ProductStructuurId` | **581** (100%) | Todos ligados à árvore |
| Nós `ProductStructuurWebshop` | **0** | **Usar só `ProductStructuur`** — tabela webshop vazia |

Exemplos de raízes (inglês): *Automatische garagedeur en poortopeners*, *Draaipoortopeners*, *Schuifpoortopeners*, *Montage*, …  
Nós intermédios podem ter produtos directos na BD (ex.: id 80 *Schuifdeuren* — 17 produtos). Com **CD4**, esses produtos **não** aparecem na grelha do nó intermédio — só nas folhas ou via subcategorias. Impacto medido: 4 nós / 16 produtos em `abmatic_test`.

### 2.4 Decisões adoptadas (Fase A)

| # | Questão | Decisão para implementação |
|---|---------|----------------------------|
| 1 | Fonte de categorias | **Só `[Products].[ProductStructuur]`** — confirmado por BD (0 linhas em `ProductStructuurWebshop`) |
| 2 | Clique em categoria intermédia | **Tiles dos filhos directos** — sem grelha de produtos no nó (CD4). Produtos na **folha** só com `ProductStructuurId` = nó |
| 3 | Quantidade “New products” | **12** na homepage (igual ao `HomePageProductLimit` actual) — *confirmar com cliente se preferir 8* |
| 4 | Paleta visual | Migrar de azul claro para **laranja/branco** da referência DevExpress |
| 5 | Hero homepage | **Remover** na migração — referência não tem hero; foco em sidebar + produtos |
| 6 | Carousel vs grelha | **Grelha responsiva** na v1 (mais simples); carousel opcional fase posterior |

### 2.5 Checklist Fase A

- ✅ Documentar **homepage** (§2.1.2 — Categories tiles + Deals + sidebar)
- ✅ Documentar **header navigation** (§2.1.1 — Back, Home, Cart, Contact, Search, Login)
- ✅ Documentar **login / sign-up** (shell, card, formulário, rotas, copy EN)
- ✅ Comparar com `Client/Components/Pages/Store/` e `store.css`
- ✅ Confirmar fonte categorias: só `ProductStructuur` (BD: webshop struct vazia)
- ✅ Confirmar filtro folha: produtos **directos** do nó (`ProductStructureId == categoryId`); intermédio → tiles (CD4)
- ✅ Definir secção New: 12 produtos (pendente confirmação cliente)
- ✅ Validar dados: `ParentId`, `IsNieuw`, `Webshop`, `ProdNonActive` em `abmatic_test`

---

## 3. Regras de negócio (cliente)

### 3.1 Categorias — `[Products].[ProductStructuur]`

| Regra | Detalhe | Campo legacy / EF |
|-------|---------|-------------------|
| Fonte da árvore | Tabela ERP de estrutura de produtos | `ProductStructuur` → `ProductStructure` |
| Raiz | `ParentId` **null** ou **0** | `ProductStructure.ParentTaskId` |
| Ligação ao produto | Cada produto aponta para um nó da árvore | `Product.ProductStructuurId` → `Product.ProductStructureId` |
| Navegação | Menu lateral + **drill-down**: filhos em tiles → produtos na folha (directos) | ✅ `StoreCategorySidebar` + `Catalog.razor` + `/?categoryId=` — §2.1.3 |
| Idioma (fase actual) | **Inglês** — `NaamEn` / `NameEn` | `CatalogCategoryTree.PickDisplayName` |

### 3.2 Visibilidade de produtos

| Regra | Detalhe | Campo legacy / EF |
|-------|---------|-------------------|
| Ocultar inactivos | `ProdNonActive = true` → **não mostrar** | `Product.IsInactive` |
| Só webshop | `Webshop = true` → **mostrar** | `Product.ShowOnWebshop` |
| Filtro | `ShowOnWebshop == true` **e** `IsInactive == false` | Validar em todos os endpoints da loja |

### 3.3 Destaque “NEW PRODUCT”

| Regra | Detalhe | Campo legacy / EF |
|-------|---------|-------------------|
| Novidade | `IsNieuw = true` → secção **New products** na homepage | `Product.IsNew` |
| Quantidade | **12** produtos (decisão Fase A) | `GetNewProductsAsync(12)` |

### 3.4 Preço e imagens (manter)

| Regra | Estado |
|-------|--------|
| Sem preço | **“Price on request”** | ✅ |
| Imagens | `AzureFile` + blob `files` | ✅ [AZUREBLOB.md](AZUREBLOB.md) |

### 3.5 Formulários por produto e categoria (evolução — análise)

> [!NOTE]
> **Estado:** só documentado — **não implementado**. Fases C e D v1 entregaram catálogo + árvore + detalhe básico; **falta juntar os formulários** que a referência do cliente associa a cada produto e categoria.

Na app de referência e no ERP AB-MATIC, produtos e categorias não são só listagem — trazem **campos configuráveis**, textos e opções que o comprador preenche antes de adicionar ao carrinho. O admin **já gere** parte destes dados (`ProductOption`, `ProductStructure`, textos de lista de preços); a loja ainda **não expõe** esses formulários no layout novo.

| Âmbito | Dados / origem (admin + ERP) | Loja hoje | Alvo Fase C (ports/DTOs) | Alvo Fase D (UI) |
|--------|------------------------------|-----------|--------------------------|------------------|
| **Produto — opções** | `ProductOption` (+ valores, obrigatório/opcional) — ver [SPEC_WEB_STORE.md](SPEC_WEB_STORE.md) §3 | `ProductDetail.razor`: qty + Add to cart apenas | Port read-only: `GetProductOptionsForStoreAsync(productId)`; DTOs de opção/valor; validação no carrinho/checkout | `StoreProductOptionsForm.razor` no detalhe; bloquear cart se `IsRequired` em falta |
| **Produto — descrição rica** | `WebshopDescriptionNl` / `NameEn`, imagens blob | Texto plano + 1 imagem | Incluir no `StoreProductDto` campos extra se necessário (galeria, fichas) | Layout detalhe alinhado à referência (galeria, tabs) |
| **Categoria — contexto** | `ProductStructuur`: `NameEn`, `IntroPriceListTextId`, `OutroPriceListTextId`, ícone | Sidebar: só nome + contagem | `GetCategoryDetailAsync(categoryId)` com textos intro/outro resolvidos | Cabeçalho na main ao filtrar categoria: título + texto introdutório |
| **Categoria — formulário** | Possível formulário de filtro/config por nó (confirmar com cliente na referência live) | ❌ | Contrato mínimo após validação UX cliente | Componente na área principal quando `?categoryId=` activo |
| **Carrinho / encomenda** | Linhas com opções seleccionadas | `StoreCartService` sem opções | Persistir opções na linha do carrinho (DTO + port) | Resumo no cart e checkout |

**Princípio:** formulários são **mantidos no admin** (cadastro); a loja **consome** via ports (`IStoreCatalogPort` ou ports dedicados `IStoreProductConfiguratorPort`) — hexagonal preservada.

**Dependências admin já existentes (reutilizar):**

| Admin | Port / repositório | Uso na loja |
|-------|-------------------|-------------|
| `/admin/product-options` | `IProductOptionAdminPort` | Ler opções publicadas para o `ProductId` |
| Estrutura produtos | `ProductStructure` | Textos e metadados por categoria |
| Produto | `Product` + preços + stock | Já usado em `StoreCatalogService` |

**Critérios de aceitação (quando implementar):**

- Detalhe do produto mostra **todas** as opções obrigatórias antes de “Add to cart”.
- Página/filtro de categoria mostra **cabeçalho** com nome EN e texto intro quando existir na BD.
- Opções seleccionadas seguem para o carrinho e encomenda (validação alinhada ao admin).
- Formulários visuais seguem tema laranja/branco §2.1 (mesmos `.form-group`, `.btn-primary`).

### 3.6 Login ↔ admin (resumo)

Ver **§2.2.2** — o botão **Login** da navbar deve evoluir para autenticação **única** que liga automaticamente utilizadores staff ao **app admin** já implementado, sem segundo login.

---

## 4. Arquitectura (o que muda / o que não)

```text
WebShopABMATIC.Client          → UI Blazor loja (layout, componentes, CSS)
WebShopABMATIC/Application     → Ports, DTOs (extensões mínimas)
WebShopABMATIC/Infrastructure  → StoreCatalogService, media, preços
```

| Camada | Alteração |
|--------|-----------|
| **Client / Store** | ✅ Layout, sidebar, homepage, CSS |
| **Application** | ✅ Novos DTOs + métodos em `IStoreCatalogPort` |
| **Infrastructure** | ✅ Árvore categorias, `IsNew`, filtros |
| **Resto da solution** | ❌ Fora de âmbito desta migração |

---

## 5. Gap — actual vs referência cliente

| Área | Estado (2026-06-25) |
|------|---------------------|
| Layout geral | ✅ Sidebar 300px + header branco + área scroll |
| Categorias | ✅ Árvore `ProductStructuur` na sidebar (`StoreCategorySidebar`) |
| Cor / fonte | ✅ Laranja `#fe7109`, Segoe UI |
| Homepage | ✅ Categories tiles + Deals; sem grelha genérica na `/` | §2.1.2 |
| **Navegação por níveis** | ✅ `?categoryId=` — tiles até folha; produtos directos na folha (CD4); sidebar activa laranja | §2.1.3 |
| Filtro catálogo (folha) | ✅ Só produtos com `ProductStructureId == categoryId`; intermédio → `[]` em `GetCatalogAsync` |
| Contagens tiles (`ProductCount`) | ✅ Subárvore via `CollectDescendantIds` (badges nos cartões) |
| `WebshopStructures` | ✅ Fallback removido em `GetCategoriesAsync` |
| **Login** | ✅ `StoreAuthLayout`, copy EN, sem link staff; botão laranja |
| **Sign-up** | ✅ Mesmo shell auth que sign-in |
| **Rota `/login`** | ✅ Redirect → `/sign-in` (`Program.cs`) |
| **Header nav** | ✅ Back · Home · Cart · Contact · Search · Login |
| **Contact** | ✅ `/contact` — conteúdo estático EN (placeholder email/tel) |
| **My account** | 🟡 Usa `StoreLayout` com sidebar — sem restyle dedicado |
| **Product detail** | 🟡 Sem breadcrumb (fase 2) |
| **Formulários produto/categoria** | ❌ Opções e textos de categoria não expostos | Ver §3.5 |
| **Login unificado admin** | ❌ `/sign-in` e `/admin/login` separados | Ver §2.2.2 |
| **Testes catálogo** | 🟡 164 testes passam; sem testes unitários novos da árvore |

---

## 6. Checklist de implementação

### Fase A — Análise da referência ✅

*Concluída — ver secção 2.*

### Fase B — Application ✅

- ✅ `StoreCategoryTreeNodeDto` (Id, ParentId, Name, Level, ProductCount?, Children)
- ✅ Estender `IStoreCatalogPort`:
  - `GetCategoryTreeAsync()`
  - `GetNewProductsAsync(take)`
- ✅ `StoreProductDto` + `IsNew` (badge no card via `ProductCard.razor`)
- ✅ Contrato: `categoryId` = id de qualquer nó `ProductStructuur`

### Fase C — Infrastructure ✅

- ✅ Árvore: raízes `ParentTaskId is null or 0`
- ✅ Contagens por nó (produtos `Webshop` + `!ProdNonActive`)
- ✅ `GetCatalogAsync`: folha → `ProductStructureId == categoryId`; intermédio com filhos → `[]` (CD4)
- ✅ `GetNewProductsAsync`: `IsNew == true` + regras de visibilidade
- ✅ Remover fallback `WebshopStructures` em `GetCategoriesAsync`
- ✅ Labels em `NameEn` (`CatalogCategoryTree.PickDisplayName`)
- ⬜ Testes dedicados: árvore, filtro intermédio, exclusões `ProdNonActive` / `Webshop=false`

**Fase C.4 — Homepage referência (planeado ⬜)** — ver §2.1.2

- ⬜ `GetRootCategoryTilesAsync()` — raízes `ProductStructuur` + `NameEn` + URL ícone (`Icon` blob ou SAS)
- ⬜ `GetDealsAsync(take)` — produtos destaque/promo (critério HP4 a confirmar)
- ⬜ DTOs: `StoreCategoryTileDto`, `StoreDealProductDto` (quick-add: id, nome, imagem, descrição curta, preço)
- ⬜ Testes: contagens tiles, deals só `Webshop` + `!ProdNonActive`

**Fase C.5 — Navegação por níveis ✅** — ver §2.1.3

- ⬜ `StoreCategoryBrowseDto` + `CategoryBrowseMode` (Home / Subcategories / Products / Search) — opcional; lógica já em `Catalog.razor`
- ✅ Modo CD2/CD3 em `Catalog.razor` + `GetCatalogAsync` (filhos directos **ou** produtos na folha)
- ⬜ `GetCategoryDetailAsync` — nome EN, intro (`IntroPriceListTextId`) para H1
- ✅ Regra CD4 implementada e testada em `abmatic_test` (4 nós / 16 produtos directos ocultos no intermédio)
- ⬜ Testes automatizados: raiz → tiles; folha → produtos; contagem `ProductCount` por filho

**Fase C.2 — Formulários produto/categoria (planeado ⬜)** — ver §3.5

- ⬜ DTOs store: opções de produto (`StoreProductOptionDto`, valores, `IsRequired`)
- ⬜ `GetProductOptionsForStoreAsync(productId)` (read-only; dados de `ProductOption`)
- ⬜ `GetCategoryDetailAsync(categoryId)` — nome EN, textos intro/outro (`IntroPriceListTextId` / `OutroPriceListTextId`)
- ⬜ Extender carrinho/encomenda: linhas com opções seleccionadas (validação Application)
- ⬜ Testes: opções obrigatórias, categoria com texto intro

**Fase C.3 — Login unificado (planeado ⬜)** — ver §2.2.2

- ⬜ Endpoint ou orquestração única: staff → `/admin`, customer → loja (`LegacySignInService`)
- ⬜ Política de resolução quando o mesmo email existe em staff e cliente (decisão U3)
- ⬜ Testes: redirect Admin/Manager vs Customer após POST login

### Fase D — UI loja (espelhar referência) ✅

- ✅ `StoreLayout.razor` — grid sidebar 300px + main
- ✅ `StoreAuthLayout.razor` — header + main centrada, sem sidebar (login/registo)
- ✅ `StoreCategorySidebar.razor` + `CategoryNode.razor` — árvore expandível, “All products”
- ✅ `StoreHeader.razor` — §2.1.1: Back·Home·Cart·Contact·Search+button·Login
- ✅ `Contact.razor` — `/contact`, conteúdo EN estático
- ✅ `store.css` — tema laranja, `.store-header-nav`, sidebar, shell overflow
- ✅ `SignIn.razor` — copy EN, sem link staff; `@layout StoreAuthLayout`
- ✅ `SignUp.razor` — `@layout StoreAuthLayout`
- 🟡 `MyAccount.razor` — ainda `.auth-card` antigo dentro do layout com sidebar
- ✅ `Catalog.razor` — homepage com **Categories + Deals** e sem grelha genérica na `/`
- ✅ `ProductCard.razor` — card reutilizável + badge New
- ✅ Textos principais da loja em inglês
- ✅ Redirect `/login` → `/sign-in` (`Program.cs`)
- ⬜ `ProductDetail.razor` — breadcrumb (opcional fase 2)

**Fase D.4 — Homepage referência (em progresso 🟡)** — ver §2.1.2

- ✅ `StoreCategoryTile.razor` — grelha de cartões com ícone/fallback e estado “No products”
- ✅ `StoreDealCard.razor` — foto, título uppercase, descrição e quick-add laranja
- ✅ `Catalog.razor` — secções **Categories** + **Deals**; removida grelha genérica na `/`
- ✅ `store.css` — estilos de `.categories-grid`, `.deals-row`, `.deal-card` e cartões de categoria
- ⬜ Responsivo: deals em scroll horizontal ou carousel &lt; 1200px

**Fase D.5 — Navegação por níveis na UI ✅** — ver §2.1.3

- ✅ `Catalog.razor` — ramificação implementada (não mostrar produtos quando há filhos)
- ✅ `Catalog.razor` — filhos com `ProductCount` e estado “No products” nos tiles vazios
- ✅ `StoreCategoryTile.razor` — categoria vazia aparece com estado visual “No products” e clique bloqueado
- ✅ `StoreCategoryHeader.razor` — H1 com nome da categoria na main
- ⬜ Reutilizar `StoreCategoryTileGrid` para filhos do nó activo (refactor opcional)
- ✅ Sidebar: activo laranja sólido (CD5), ramo activo expandido e `scrollIntoView` do item activo
- ✅ Back no header: subir para `ParentId` (CD7)
- ⬜ CSS `.store-nav-link--active-branch`, `.category-browse-grid` (nomenclatura final)

**Fase D.2 — Formulários na UI loja (planeado ⬜)** — ver §3.5

- ⬜ `StoreProductOptionsForm.razor` — opções por produto no detalhe
- ⬜ `ProductDetail.razor` — integrar formulário + validação antes do carrinho
- 🟡 Cabeçalho de categoria na main entregue (H1 com nome); intro EN/form ainda pendentes
- ⬜ `Cart.razor` / checkout — mostrar opções seleccionadas por linha
- ⬜ Estilos formulário alinhados ao tema referência (laranja/Bootstrap)

**Fase D.3 — Login unificado na UI (planeado ⬜)** — ver §2.2.2

- ⬜ **Login** na navbar → formulário único ligado ao admin + loja
- ⬜ Pós-login staff: entrada directa em `/admin` (app admin existente)
- ⬜ Remover ou redireccionar `/admin/login` para o fluxo único
- ⬜ Mensagens de erro genéricas (sem enumerar staff vs cliente)

### Fase E — Qualidade ⬜

- ⬜ Testes Razor com mock `IStoreCatalogPort`
- ⬜ Regressão: preços, imagens blob, carrinho, sign-in
- ⬜ Performance: evitar N+1 (árvore + produtos)
- ⬜ Actualizar [SPEC_WEB_STORE.md](SPEC_WEB_STORE.md)

### Fase F — Validação ⬜

- ⬜ Teste local
- ⬜ Deploy `abmaticwebshop.azurewebsites.net`
- ⬜ Validação com cliente lado a lado com a referência

---

## 7. Ficheiros a tocar

| Ficheiro | Estado |
|----------|--------|
| `Application/Ports/IStoreCatalogPort.cs` | ✅ Estendido |
| `Application/Store/StoreCategoryTreeNodeDto.cs` | ✅ |
| `Application/Store/StoreProductDto.cs` | ✅ `IsNew` |
| `Infrastructure/Store/StoreCatalogService.cs` | ✅ Árvore, New, filtros, sem webshop fallback |
| `Infrastructure/Store/CatalogCategoryTree.cs` | ✅ `CollectDescendantIds` |
| `Client/Components/Layout/StoreLayout.razor` | ✅ |
| `Client/Components/Layout/StoreAuthLayout.razor` | ✅ Novo |
| `Client/Components/Store/StoreCategorySidebar.razor` | ✅ Novo |
| `Client/Components/Store/CategoryNode.razor` | ✅ Novo |
| `Client/Components/Store/ProductCard.razor` | ✅ Novo |
| `Client/Components/Store/StoreHeader.razor` | ✅ §2.1.1 |
| `Client/Components/Pages/Store/Contact.razor` | ✅ Novo |
| `Client/Components/Pages/Store/SignIn.razor` | ✅ |
| `Client/Components/Pages/Store/SignUp.razor` | ✅ |
| `Client/Components/Pages/Store/MyAccount.razor` | 🟡 Pendente restyle |
| `Client/Components/Pages/Store/Catalog.razor` | ✅ |
| `Client/wwwroot/css/store.css` | ✅ Tema referência |
| `WebShopABMATIC/Program.cs` | ✅ `/login` redirect |
| `WebShopABMATIC.Tests/...` | 🟡 Testes árvore catálogo por adicionar |

---

## 8. Mapeamento legacy

```text
[Products].[ProductStructuur]  → ProductStructure (ParentId → ParentTaskId)
[Products].[Product]
  ProductStructuurId           → ProductStructureId
  ProdNonActive = 1            → ocultar
  Webshop = 1                  → mostrar
  IsNieuw = 1                  → New products
[Bestanden].[AzureFile]        → imagens (AZUREBLOB.md)
```

---

## 9. Critérios de aceitação

- ✅ Layout visual alinhado com https://adminsenceweb.azurewebsites.net/
- ✅ Header: Back · Home · Cart · Contact · Search · Login (inglês, ícone + texto)
- ✅ Login/sign-up com header branco, card centrado, botão laranja, textos em inglês (sem sidebar)
- ✅ Sidebar com árvore `ProductStructuur` (raízes `ParentId` null/0)
- ✅ Homepage: grelha **Categories** (ícones) + fila **Deals** (§2.1.2)
- ✅ Navegação: clicar categoria → **tiles subcategorias** → folha → produtos directos (§2.1.3, CD4)
- ✅ Filtro folha: produtos com `ProductStructuurId` = nó activo (sem agregar descendentes)
- ✅ `ProdNonActive` / `Webshop=false` excluídos
- 🟡 Destaques promo: hoje **New products**; alvo secção **Deals** (~8)
- ✅ Loja em inglês
- ✅ Hexagonal preservada (páginas → ports)
- ✅ Imagens e preços como hoje

---

## 10. Decisões em aberto

| # | Questão | Estado |
|---|---------|--------|
| 1 | Só `ProductStructuur`? | ✅ Sim (BD vazia em webshop struct) |
| 2 | Categoria intermédia vs folha | ✅ Intermédio → tiles filhos; folha → produtos directos (CD4) |
| 3 | New products: 12 ou 8? | ⏳ 12 adoptado — confirmar com cliente |
| 4 | Paginação catálogo grande? | Fase posterior |
| 5 | Tabs login+registo no mesmo ecrã? | ⏳ Não na v1 — páginas separadas, estilo igual |
| 6 | Link staff no sign-in da loja? | ✅ Remover — admin fora do layout loja |
| 7 | Conteúdo `/contact` | ✅ Estático EN v1 — placeholder; ligar `BaseCompany` depois |
| 8 | Logo na navbar | ✅ Fora da barra — alinhar screenshot cliente |
| 9 | Formulários produto/categoria na loja? | ⏳ Sim — §3.5; Fase C.2 + D.2 |
| 10 | Login único loja + admin? | ⏳ Sim — §2.2.2; Fase C.3 + D.3; reutilizar admin pronto |
| 11 | Formulário por categoria na referência | ⏳ Confirmar com cliente (screenshot / URL live) |
| 12 | Secção Deals vs New products | ⏳ Deals ~8 promo; HP4 — critério BD a confirmar |
| 13 | Grelha ícones Categories na `/` | ⏳ Sim — raízes com ícone `ProductStructure.Icon` |
| 14 | Grelha produtos na homepage | ⏳ Remover da `/`; produtos só por categoria ou pesquisa |
| 15 | Tiles vs produtos ao clicar categoria | ✅ Filhos primeiro (CD2); produtos só na folha (CD3/CD4) |
| 16 | Nó intermédio com produtos directos | ✅ CD4 — tiles apenas; 4 nós / 16 produtos em `abmatic_test` |
| 17 | Sidebar activo laranja sólido | ✅ CD5 — `#fe7109` |
| 18 | Back sobe nível na árvore | ✅ CD7 — `ParentId` |

---

## 11. Ordem sugerida

1. ~~**A** — análise~~ ✅  
2. ~~**B + C** — backend (árvore + `IsNew` + filtros)~~ ✅  
3. ~~**D** — UI base (header, sidebar, tema)~~ ✅ — homepage completa em **C.4 + D.4**  
4. **E + F** — testes dedicados catálogo, `SPEC_WEB_STORE.md`, validação e deploy com cliente  
5. **C.2 + D.2** — formulários produto/categoria (opções, intro categoria, carrinho)  
6. **C.3 + D.3** — login unificado navbar → admin + loja  
7. ~~**C.4 + D.4** — homepage: **Categories** (tiles) + **Deals**~~ ✅  
8. ~~**C.5 + D.5** — navegação por níveis (tiles subcategorias → produtos na folha, CD4)~~ ✅  

---

## Documentação técnica

- 🛒 [SPEC_WEB_STORE.md](SPEC_WEB_STORE.md) — comportamento loja  
- 🖼️ [AZUREBLOB.md](AZUREBLOB.md) — imagens  

---

**© 2026 AdminSense. All rights reserved.**

---

## 12. Cart, checkout & Mollie (Jun/2026)

### Checkout + Mollie (dev)

- Removed the block that prevented PrePay; Mollie flow restored in `CheckoutUseCase`
- `Mollie:UseMock=true` (default) → `MollieMockPaymentAdapter` — simulates payment without an API key
- Webhook registered: `POST /api/webhooks/mollie/payments`
- **D.7** — reserves stock (`ReservedQuantity`) when a PrePay order is created; decrements stock after payment

### UI `/cart`

- Guest sees line items; checkout requires **Sign in**
- Authenticated customer: delivery address + payment method (default **Mollie / iDEAL**)
- Button **Place order & pay**

### Implemented (dev mock)

| Item | Status |
|------|--------|
| Cart persists in **session** (`StoreCartService`) across pages | ✅ |
| `/cart` shows lines for guests; checkout requires login | ✅ |
| **Place order & pay** — PrePay (Mollie) with `Mollie:UseMock=true` | ✅ |
| Stock reservation (`ReservedQuantity`) on PrePay order create | ✅ D.7 |
| Redirect → `/orders/{id}/payment-return` → confirmation (mock = paid) | ✅ |
| PostPay (invoice) → direct confirmation + stock decrement | ✅ |
| Webhook `POST /api/webhooks/mollie/payments` registered | ✅ |

### Local flow (mock)

1. Add product → `/cart` (items visible).
2. **Sign in** as webshop customer (`Klanten.Klant` / `WebshopLogin`).
3. Choose address + **iDEAL / card (Mollie)** → **Place order & pay**.
4. Redirect to **demo Mollie checkout** (`/checkout/mollie-mock`) — card/iDEAL pre-filled → click **Pay** → payment-return → **paid** → stock decremented → `/orders/{id}/confirmation`.

### ⬜ Pending — real Mollie (B.9 prod go-live)

Only when you have credentials and a public URL. See [open_MOLLIE_PAYMENTS_open.md](open_MOLLIE_PAYMENTS_open.md).

- [ ] `Mollie:ApiKey` test (`test_…`) in user secrets or App Settings
- [ ] `Mollie:UseMock` = `false`
- [ ] Public HTTPS URL (ngrok / Azure) for webhook
- [ ] E2E: hosted Mollie checkout → webhook → paid + stock
- [ ] **Live** keys + production webhook (go-live)

---

## 13. Category navigation — CD4 (Jun/2026)

### Rule

| Node type | Main content | `GetCatalogAsync` |
|-----------|--------------|-------------------|
| Has structural children | Child category **tiles** only | Returns `[]` |
| Leaf (no children) | Product grid | `ProductStructureId == categoryId` |

`ProductCount` on tree DTOs still uses subtree totals (`CollectDescendantIds`) for tile badges.

### Code

| File | Change |
|------|--------|
| `CatalogCategoryTree.cs` | `HasStructuralChildren()` |
| `StoreCatalogService.GetCatalogCoreAsync` | CD4 branch: intermediate → `[]`; leaf → direct filter |
| `Catalog.razor` | `Children.Count > 0` → browse mode; else `GetCatalogAsync` |
| `IStoreCatalogPort.cs` | XML doc on leaf-only catalog behaviour |

### `abmatic_test` impact (2026-06-26)

| Metric | Value |
|--------|-------|
| Intermediate nodes with direct webshop products | **4** |
| Products hidden on intermediate grids | **16** |

Examples: id **17** *Signaallampen* (7), **17183** *Lakkerij…* (6), **36** *Parkeersystemen* (2), **27** *IR fotocellen* (1). These remain reachable via child categories in the tree.

