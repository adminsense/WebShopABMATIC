# Plano: logs admin (modelo IMMO + menu Logs + CRUD + CSV)

**Overview:** Gravar logs no CRUD de cada form admin → `[Logging].[Error]`. Consulta no menu separado **Logs**, UI modelada em [`docs/AUDIT.md`](AUDIT.md) + screenshot IMMO (filtros Apply/Clear, **Action em badges coloridos**). Sem schema Immo; sem painel dentro dos forms.

**Referência UI:** `docs/AUDIT.md` §2 (grid/filters/badges) + screenshot Audit Logs IMMO. Aproveitar ideia geral e badges; **não** copiar entidades/actions Immo (2FA, WKFLOW*, FileManager, Rent…).

## Todos

- [ ] Gravação CRUD admin (interceptor + `IAuditService` onde faltar)
- [ ] Menu Logs: hub + sidebar; Back → `/admin/hub/logs`
- [ ] UI tipo IMMO: filtros Date/Action/Module/User/Status + Apply/Clear; grelha com badges Action coloridos + Status + View
- [ ] Corrigir mapeamento Action no repo (hoje `ClassName`=entidade; badge precisa da Action real no texto `Exception`)
- [ ] Completar CSS/`AuditActionBadge` para todas as `AuditActions` WebShop
- [ ] Export CSV (`IGridExportService`)
- [ ] SPEC_ADMIN + SPEC_INFRASTRUCTURE §3.5 + AMENDMENTS

## O que pediste

1. **Gravar** no fluxo CRUD de cada formulário admin → `[Logging].[Error]`
2. **Mostrar** só na tela **Logs** do admin
3. **Actions** = labels coloridas como no IMMO (`CREATE` verde, `UPDATE` azul, `DELETE` vermelho, `LOGIN` azul, `LOGOUT` cinza, etc.)

## Modelo UI (IMMO → WebShopABMATIC)

Aproveitar de `docs/AUDIT.md` / screenshot:

- Header: título Audit Logs + Refresh
- Filtros: Date From / Date To, Action (dropdown), User, Module (equiv. “Person”/área — `ModuleName`), Status (All / Success / Failed)
- Severity: só se der para derivar de Outcome/texto sem coluna nova; senão omitir ou mapear Information vs Failed→Error
- Botões: **Apply Filters** (primary + funnel) + **Clear** (danger) — padrão `PATTERNS_UI_QUICK_START`
- Opcional: botão **Badges legend** (modal com `AuditActionBadge` por action WebShop) — como IMMO
- Colunas: Timestamp | Severity (se houver) | **Action (badge colorido)** | Module | User | Status (✓/✗) | View (olho / Detail)
- Detail: modal na mesma página
- Paginação; ordenação mais recente primeiro
- Export CSV das linhas filtradas

**Não trazer do IMMO:** IP Address (não está em `[Logging].[Error]`), Person/tenant Immo, 2FA, WKFLOW*, FileManager, Rent*, OldValues/NewValues JSON ricos se não existirem na tabela, tabela `AuditLogs` própria.

## Badges Action (WebShop)

Componente existente: [`AuditActionBadge.razor`](../WebShopABMATIC.Client/Components/Admin/AuditActionBadge.razor).

Cores (alinhar a IMMO + CSS admin):

- CREATE → verde (`audit-badge-create` / success)
- UPDATE → azul (`audit-badge-update` / primary)
- DELETE → vermelho (`audit-badge-delete` / danger)
- LOGIN → azul
- LOGINFAILED → vermelho/warning
- LOGOUT → cinza (secondary)
- REPORTEXPORT, CHECKOUT, PAYMENT*, STOCK*, PASSWORDRESET, ORDERCANCELLED → classes já previstas / completar gaps

CSS em `admin.css` (Client + host): garantir classes `audit-badge-*` visíveis como labels (pill/badge).

## Gravação

- Interceptor staff `SaveChanges` → `AttachCrudAppError`
- Gaps: `IAuditService.LogAsync` no use case
- Soft-delete = Update
- Auth/checkout/registration já via `IAuditService` (plano anterior)

## Bug a corrigir na listagem

Hoje o repo mapeia `Action = ClassName` (nome da entidade). Badges precisam da **action** (`Create`/`Update`/…), tipicamente prefixo de `Exception` (`Create Product id=…`). Ajustar `LegacyAuditLogRepository` + DTO: Action = acção parseada; Entity/Module = `ModuleName` / entidade.

## Menu

- Sidebar **Logs** → `/admin/hub/logs`
- Card → `/admin/audit-logs`
- Sem card em Settings

## Fora de escopo

- Viewer de logs dentro de cada formulário
- Colunas/tabelas novas no ERP
- Actions/features só do Immo (2FA, workflow, file manager, rent index)
- Substituir `DossierLog`

## Verificação manual

1. Menu Logs separado
2. CRUD form → linha na lista com badge Create/Update colorido
3. Login → badge LOGIN azul; Logout cinza
4. Filtros Apply/Clear + CSV
5. Detail modal fecha sem navegar
