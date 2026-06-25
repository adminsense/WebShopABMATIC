# WebShop — Migração de layout da loja

![Status](https://img.shields.io/badge/Status-Fases%20B–D%20implementadas-28a745?style=flat-square) ![Referência](https://img.shields.io/badge/Referência-adminsenceweb-512BD4?style=flat-square) ![Idioma](https://img.shields.io/badge/Loja-English-0dcaf0?style=flat-square)

> [!IMPORTANT]
> **Resumo:** Migrar o **layout da loja** para ficar igual à referência que o cliente aprovou: **[https://adminsenceweb.azurewebsites.net/](https://adminsenceweb.azurewebsites.net/)**. Mantém-se a arquitectura hexagonal (`Application` → ports → `Infrastructure`); mudam UI/CSS da webstore e contratos de catálogo.

**Próximo passo sugerido:** Fase E + F; em paralelo evoluir análise §3.5 (formulários) e §2.2.2 (login unificado com admin).

---

## 1. Referência única

| Item | Valor |
|------|--------|
| **URL a seguir** | https://adminsenceweb.azurewebsites.net/ |
| **App** | `Adminsence.Shop` — Blazor Server |
| **Loja actual (código)** | Sidebar 300px + header §2.1.1 + `Catalog` sem hero (New products + grelha) |
| **Loja alvo** | Layout da referência + sidebar esquerda com árvore `[Products].[ProductStructuur]` + área de produtos em **inglês** |

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

### 2.2 Comparação com código actual

| Ficheiro | Estado actual | Gap |
|----------|---------------|-----|
| `StoreLayout.razor` | Header + `@Body` sem sidebar | Precisa grid **sidebar + main** |
| `StoreHeader.razor` | Logo, search, nav pills azuis | Reescrever §2.1.1: Back·Home·Cart·Contact·Search·Login |
| `Catalog.razor` | Hero + chips + grelha 12 produtos | Remover hero/chips; **New products** + grelha |
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
Nós intermédios têm produtos directos (ex.: id 80 *Schuifdeuren* — 17 produtos).

### 2.4 Decisões adoptadas (Fase A)

| # | Questão | Decisão para implementação |
|---|---------|----------------------------|
| 1 | Fonte de categorias | **Só `[Products].[ProductStructuur]`** — confirmado por BD (0 linhas em `ProductStructuurWebshop`) |
| 2 | Clique em categoria intermédia | **Incluir produtos do nó + descendentes** (pedido do cliente + dados com `Level` 1 e 2) |
| 3 | Quantidade “New products” | **12** na homepage (igual ao `HomePageProductLimit` actual) — *confirmar com cliente se preferir 8* |
| 4 | Paleta visual | Migrar de azul claro para **laranja/branco** da referência DevExpress |
| 5 | Hero homepage | **Remover** na migração — referência não tem hero; foco em sidebar + produtos |
| 6 | Carousel vs grelha | **Grelha responsiva** na v1 (mais simples); carousel opcional fase posterior |

### 2.5 Checklist Fase A

- ✅ Documentar layout da referência (sidebar, header, grelha, cores, tipografia)
- ✅ Documentar **header navigation** (§2.1.1 — Back, Home, Cart, Contact, Search, Login)
- ✅ Documentar **login / sign-up** (shell, card, formulário, rotas, copy EN)
- ✅ Comparar com `Client/Components/Pages/Store/` e `store.css`
- ✅ Confirmar fonte categorias: só `ProductStructuur` (BD: webshop struct vazia)
- ✅ Confirmar filtro intermédio: nó + descendentes
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
| Navegação | Menu **lateral esquerdo** com árvore expandível | `StoreCategorySidebar.razor` |
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
| Homepage | ✅ New products (12) + grelha filtrada — sem hero/chips |
| Filtro | ✅ Por nó + descendentes (`CollectDescendantIds`) |
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
- ✅ `GetCatalogAsync`: filtro por descendentes do nó seleccionado
- ✅ `GetNewProductsAsync`: `IsNew == true` + regras de visibilidade
- ✅ Remover fallback `WebshopStructures` em `GetCategoriesAsync`
- ✅ Labels em `NameEn` (`CatalogCategoryTree.PickDisplayName`)
- ⬜ Testes dedicados: árvore, filtro intermédio, exclusões `ProdNonActive` / `Webshop=false`

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
- ✅ `Catalog.razor` — sem hero/chips; New products; grelha + `?categoryId` / `?q`
- ✅ `ProductCard.razor` — **novo** — card reutilizável + badge New
- ✅ Textos principais da loja em inglês
- ✅ Redirect `/login` → `/sign-in` (`Program.cs`)
- ⬜ `ProductDetail.razor` — breadcrumb (opcional fase 2)

**Fase D.2 — Formulários na UI loja (planeado ⬜)** — ver §3.5

- ⬜ `StoreProductOptionsForm.razor` — opções por produto no detalhe
- ⬜ `ProductDetail.razor` — integrar formulário + validação antes do carrinho
- ⬜ Cabeçalho de categoria na main (`Catalog.razor` ou `StoreCategoryHeader.razor`) com intro EN
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
- ✅ Filtro por categoria inclui subcategorias
- ✅ `ProdNonActive` / `Webshop=false` excluídos
- ✅ `IsNieuw=true` na secção New products (12)
- ✅ Loja em inglês
- ✅ Hexagonal preservada (páginas → ports)
- ✅ Imagens e preços como hoje

---

## 10. Decisões em aberto

| # | Questão | Estado |
|---|---------|--------|
| 1 | Só `ProductStructuur`? | ✅ Sim (BD vazia em webshop struct) |
| 2 | Categoria intermédia inclui filhos? | ✅ Sim |
| 3 | New products: 12 ou 8? | ⏳ 12 adoptado — confirmar com cliente |
| 4 | Paginação catálogo grande? | Fase posterior |
| 5 | Tabs login+registo no mesmo ecrã? | ⏳ Não na v1 — páginas separadas, estilo igual |
| 6 | Link staff no sign-in da loja? | ✅ Remover — admin fora do layout loja |
| 7 | Conteúdo `/contact` | ✅ Estático EN v1 — placeholder; ligar `BaseCompany` depois |
| 8 | Logo na navbar | ✅ Fora da barra — alinhar screenshot cliente |
| 9 | Formulários produto/categoria na loja? | ⏳ Sim — §3.5; Fase C.2 + D.2 |
| 10 | Login único loja + admin? | ⏳ Sim — §2.2.2; Fase C.3 + D.3; reutilizar admin pronto |
| 11 | Formulário por categoria na referência | ⏳ Confirmar com cliente (screenshot / URL live) |

---

## 11. Ordem sugerida

1. ~~**A** — análise~~ ✅  
2. ~~**B + C** — backend (árvore + `IsNew` + filtros)~~ ✅  
3. ~~**D** — UI espelhando a referência~~ ✅ (exc. `MyAccount` restyle, breadcrumb produto)  
4. **E + F** — testes dedicados catálogo, `SPEC_WEB_STORE.md`, validação e deploy com cliente  
5. **C.2 + D.2** — formulários produto/categoria (opções, intro categoria, carrinho)  
6. **C.3 + D.3** — login unificado navbar → admin + loja  

---

## Documentação técnica

- 🛒 [SPEC_WEB_STORE.md](SPEC_WEB_STORE.md) — comportamento loja  
- 🖼️ [AZUREBLOB.md](AZUREBLOB.md) — imagens  

---

**© 2026 AdminSense. All rights reserved.**
