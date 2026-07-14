using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Store.Checkout;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Application.Store.Orders;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Store;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StoreOrderRepository : IStoreOrderRepository
{
    private const decimal DefaultDeliveryFee = 9.00m;
    private const decimal DefaultVatPercentage = 21m;

    private readonly WebShopABMATICDbContext _db;
    private readonly StoreDbGate _dbGate;

    public StoreOrderRepository(WebShopABMATICDbContext db, StoreDbGate dbGate)
    {
        _db = db;
        _dbGate = dbGate;
    }

    public Task<CheckoutOptionsDto?> GetCheckoutOptionsAsync(int customerId, CancellationToken cancellationToken = default) =>
        _dbGate.RunAsync(() => GetCheckoutOptionsCoreAsync(customerId, cancellationToken), cancellationToken);

    private async Task<CheckoutOptionsDto?> GetCheckoutOptionsCoreAsync(int customerId, CancellationToken cancellationToken)
    {
        var addressRows = await (
            from a in _db.CustomerDeliveryAddresses.AsNoTracking()
            join c in _db.Cities.AsNoTracking() on a.CityId equals c.CityId into cityJoin
            from city in cityJoin.DefaultIfEmpty()
            where a.CustomerId == customerId
            orderby a.Name
            select new
            {
                a.Id,
                a.Name,
                a.Straat,
                a.Number,
                CityName = city != null ? city.CityName : null
            }).ToListAsync(cancellationToken);

        // Build labels in-memory: SQL concatenation fails when address/city columns use different collations.
        var addresses = addressRows
            .Select(a => new CheckoutDeliveryAddressDto
            {
                Id = a.Id,
                Label = $"{a.Name} — {a.Straat} {a.Number}"
                    + (string.IsNullOrWhiteSpace(a.CityName) ? "" : $", {a.CityName}")
            })
            .ToList();

        var paymentRows = await _db.PaymentMethods.AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new { p.Id, p.NameEn, p.NameNl, p.NameFr, p.IsPrePay, p.IsPostPay })
            .ToListAsync(cancellationToken);

        var paymentMethods = paymentRows
            .Select(p =>
            {
                var selectable = IsStorefrontMollieMethod(p.NameEn, p.NameNl, p.NameFr, p.IsPrePay, p.IsPostPay);
                return new CheckoutPaymentMethodDto
                {
                    Id = p.Id,
                    Name = selectable
                        ? "iDEAL / card (Mollie)"
                        : PreferPaymentName(p.NameEn, p.NameNl, p.NameFr),
                    IsPrePay = selectable,
                    IsPostPay = !selectable,
                    IsSelectable = selectable
                };
            })
            .ToList();

        // Ensure at least one Mollie-labeled option when any PrePay row exists but name heuristic missed it.
        if (paymentMethods.All(p => !p.IsSelectable))
        {
            var fallback = paymentRows.FirstOrDefault(p => p.IsPrePay);
            if (fallback is not null)
            {
                paymentMethods = paymentMethods.Select(p =>
                {
                    if (p.Id != fallback.Id)
                    {
                        return p;
                    }

                    return new CheckoutPaymentMethodDto
                    {
                        Id = p.Id,
                        Name = "iDEAL / card (Mollie)",
                        IsPrePay = true,
                        IsPostPay = false,
                        IsSelectable = true
                    };
                }).ToList();
            }
        }

        if (paymentMethods.Count == 0)
        {
            return null;
        }

        return new CheckoutOptionsDto
        {
            CustomerId = customerId,
            DeliveryAddresses = addresses,
            PaymentMethods = paymentMethods,
            DeliveryFee = DefaultDeliveryFee,
            VatPercentage = DefaultVatPercentage
        };
    }

    public Task<IReadOnlyDictionary<int, int>> GetAvailableStockAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default) =>
        _dbGate.RunAsync(() => GetAvailableStockCoreAsync(productIds, cancellationToken), cancellationToken);

    private async Task<IReadOnlyDictionary<int, int>> GetAvailableStockCoreAsync(IEnumerable<int> productIds, CancellationToken cancellationToken)
    {
        var ids = productIds.Distinct().ToList();
        if (ids.Count == 0)
        {
            return new Dictionary<int, int>();
        }

        var rows = await _db.ProductStockLocations.AsNoTracking()
            .Where(x => x.IsDefault && ids.Contains(x.ProductId))
            .Select(x => new { x.ProductId, x.Quantity, x.ReservedQuantity })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(
            x => x.ProductId,
            x => (int)Math.Max(0, x.Quantity - x.ReservedQuantity));
    }

    public Task<StoreOrderCreated> CreateWebshopOrderAsync(StoreOrderCreateCommand command, CancellationToken cancellationToken = default) =>
        _dbGate.RunAsync(() => CreateWebshopOrderCoreAsync(command, cancellationToken), cancellationToken);

    private async Task<StoreOrderCreated> CreateWebshopOrderCoreAsync(StoreOrderCreateCommand command, CancellationToken cancellationToken)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);

        var subtotal = command.Lines.Sum(l => l.LineTotalExclVat);
        var deliveryExcl = command.DeliveryFee;
        var vatBase = subtotal + deliveryExcl;
        var vatAmount = Math.Round(vatBase * command.VatPercentage / 100m, 2);
        var totalInclVat = vatBase + vatAmount;

        var orderNumber = 20260000 + (await _db.Orders.CountAsync(cancellationToken) + 1);

        var order = new Order
        {
            IsAccepted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = command.CreatedByUserId,
            ProjectId = command.ProjectId,
            GeneralDiscount = 0,
            DeliveryTypeId = command.DeliveryTypeId,
            PriceListTypeId = 1,
            CommissionAmount = 0,
            VatTypeId = 1,
            OrderProcessingTypeId = 1,
            CustomerTypeId = command.CustomerTypeId,
            AllowPartialDelivery = true,
            BetaaltermijnId = command.BetaaltermijnId,
            QuoteValidDays = 30,
            IsUrgent = false,
            BaseCompanyVatNumberId = 1,
            OrderNumber = orderNumber,
            LeveradresId = command.DeliveryAddressId
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(cancellationToken);

        var productIds = command.Lines.Select(l => l.ProductId).Distinct().ToList();
        var productMeta = await _db.Products.AsNoTracking()
            .Where(p => productIds.Contains(p.ProductId))
            .Select(p => new { p.ProductId, p.ProductTypeId, p.ReportingGroupId })
            .ToDictionaryAsync(p => p.ProductId, cancellationToken);

        // Resolve FK defaults from ERP lookup tables — never invent Ids (0 is invalid).
        var fallbackProductTypeId = productMeta.Values
            .Select(p => p.ProductTypeId)
            .FirstOrDefault(id => id is > 0)
            ?? await _db.ProductTypes.AsNoTracking().Select(t => (int?)t.Id).FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("No ProductType rows found in Products.ProductType.");

        var fallbackReportingGroupId = productMeta.Values
            .Select(p => p.ReportingGroupId)
            .FirstOrDefault(id => id is > 0)
            ?? await _db.ReportingGroups.AsNoTracking().Select(g => (int?)g.Id).FirstOrDefaultAsync(cancellationToken)
            ?? 1;

        var fallbackVatTypeId = await _db.VatTypes.AsNoTracking().Select(v => (int?)v.Id).FirstOrDefaultAsync(cancellationToken) ?? 1;

        var sortOrder = 1;
        var linesWithOptions = new List<(OrderLine Parent, StoreOrderLineCreate Source, int ProductType, int ReportingGroupId)>();
        foreach (var line in command.Lines)
        {
            productMeta.TryGetValue(line.ProductId, out var meta);
            var productType = meta?.ProductTypeId is > 0 ? meta.ProductTypeId.Value : fallbackProductTypeId;
            var reportingGroupId = meta?.ReportingGroupId is > 0 ? meta.ReportingGroupId.Value : fallbackReportingGroupId;
            var entity = CreateOrderLine(order.Id, line, sortOrder++, command.VatPercentage, productType, reportingGroupId, fallbackVatTypeId);
            _db.OrderLines.Add(entity);
            if (line.Options.Count > 0)
            {
                linesWithOptions.Add((entity, line, productType, reportingGroupId));
            }
        }

        // Persist the product lines first so their Ids exist for the option child lines
        // (legacy convention: options are IsOption rows linked via ProductOptionHoofdDetaillId).
        if (linesWithOptions.Count > 0)
        {
            await _db.SaveChangesAsync(cancellationToken);
            foreach (var (parent, source, productType, reportingGroupId) in linesWithOptions)
            {
                foreach (var option in source.Options)
                {
                    _db.OrderLines.Add(CreateOptionLine(
                        order.Id, parent.Id, source, option, sortOrder++, command.VatPercentage,
                        productType, reportingGroupId, fallbackVatTypeId));
                }
            }
        }

        if (deliveryExcl > 0)
        {
            var deliveryVat = Math.Round(deliveryExcl * command.VatPercentage / 100m, 2);
            _db.OrderLines.Add(CreateDeliveryLine(
                order.Id, sortOrder, deliveryExcl, deliveryVat, command.VatPercentage,
                fallbackProductTypeId, fallbackReportingGroupId, fallbackVatTypeId));
        }

        int? advancePaymentId = null;
        if (command.IsPrePay)
        {
            var advance = new OrderAdvancePayment
            {
                OrderId = order.Id,
                Name = StoreAdvancePaymentEncoding.BuildName(command.MolliePaymentId),
                Percent = 100,
                IsFinalInvoice = false,
                SortOrder = 1,
                Amount = totalInclVat,
                // Existing ERP column Voorschotzichtbaarheid holds payment lifecycle status.
                AdvancePaymentVisibility = command.MolliePaymentStatus ?? "open"
            };
            _db.OrderAdvancePayments.Add(advance);
            await _db.SaveChangesAsync(cancellationToken);
            advancePaymentId = advance.Id;
        }

        await _db.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return new StoreOrderCreated
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            AdvancePaymentId = advancePaymentId,
            TotalInclVat = totalInclVat
        };
    }

    public async Task<StoreAdvancePaymentInfo?> GetAdvancePaymentByMollieIdAsync(string molliePaymentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(molliePaymentId))
        {
            return null;
        }

        var advance = await _db.OrderAdvancePayments.AsNoTracking()
            .Where(a => a.Name != null && a.Name.Contains(molliePaymentId))
            .OrderByDescending(a => a.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (advance is null)
        {
            return null;
        }

        return new StoreAdvancePaymentInfo
        {
            Id = advance.Id,
            OrderId = advance.OrderId,
            MolliePaymentStatus = advance.AdvancePaymentVisibility,
            MolliePaidAt = advance.InvoicedAt
        };
    }

    public async Task MarkAdvancePaymentPaidAsync(int advancePaymentId, string mollieStatus, DateTime paidAt, CancellationToken cancellationToken = default)
    {
        var entity = await _db.OrderAdvancePayments.FirstAsync(a => a.Id == advancePaymentId, cancellationToken);
        entity.AdvancePaymentVisibility = mollieStatus;
        entity.InvoicedAt = paidAt;
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAdvancePaymentStatusAsync(int advancePaymentId, string mollieStatus, CancellationToken cancellationToken = default)
    {
        var entity = await _db.OrderAdvancePayments.FirstAsync(a => a.Id == advancePaymentId, cancellationToken);
        entity.AdvancePaymentVisibility = mollieStatus;
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExpiredReservationInfo>> GetExpiredPrePayOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow - olderThan;
        var terminalStatuses = new[] { "paid", "expired", "canceled", "failed" };

        var rows = await (
            from a in _db.OrderAdvancePayments.AsNoTracking()
            join o in _db.Orders.AsNoTracking() on a.OrderId equals o.Id
            where a.InvoicedAt == null
                  && a.Name != null
                  && a.Name.StartsWith(StoreAdvancePaymentEncoding.OnlinePaymentLabel)
                  && o.CreatedAt < cutoff
            select new { a.Id, a.OrderId, a.Name, a.AdvancePaymentVisibility, o.CreatedAt }
        ).ToListAsync(cancellationToken);

        return rows
            .Select(r => new
            {
                r.Id,
                r.OrderId,
                r.CreatedAt,
                MolliePaymentId = StoreAdvancePaymentEncoding.ExtractMolliePaymentId(r.Name),
                Status = r.AdvancePaymentVisibility
            })
            .Where(r => !string.IsNullOrEmpty(r.MolliePaymentId)
                        && (r.Status == null || !terminalStatuses.Contains(r.Status)))
            .Select(r => new ExpiredReservationInfo
            {
                OrderId = r.OrderId,
                AdvancePaymentId = r.Id,
                MolliePaymentId = r.MolliePaymentId,
                MolliePaymentStatus = r.Status,
                OrderCreatedAt = r.CreatedAt
            })
            .ToList();
    }

    public async Task<CheckoutOrderSummaryDto?> GetOrderSummaryForCustomerAsync(int orderId, int customerId, CancellationToken cancellationToken = default)
    {
        var order = await (
            from o in _db.Orders.AsNoTracking()
            join p in _db.Projects.AsNoTracking() on o.ProjectId equals p.ProjectId
            where o.Id == orderId && p.CustomerId == customerId
            select new { o.Id, o.OrderNumber, o.CreatedAt }
        ).FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return null;
        }

        // Do not coalesce NameEn / DocumentDisplayName in SQL — different collations
        // (Latin1_General_CI_AS vs SQL_Latin1_General_CP1_CI_AS) blow up the CASE.
        var lineRows = await (
            from l in _db.OrderLines.AsNoTracking()
            join prod in _db.Products.AsNoTracking() on l.ProductId equals prod.ProductId into prodJoin
            from prod in prodJoin.DefaultIfEmpty()
            where l.OrderId == orderId && l.ProductId != null
            orderby l.SortOrder
            select new
            {
                ProductId = l.ProductId!.Value,
                ProductName = prod.NameEn,
                LineName = l.DocumentDisplayName,
                l.UnitPrice,
                Quantity = (int)l.Quantity,
                LineTotal = l.TotalExclVat
            }).ToListAsync(cancellationToken);

        var lines = lineRows
            .Select(l => new CheckoutLineQuoteDto
            {
                ProductId = l.ProductId,
                Name = !string.IsNullOrWhiteSpace(l.ProductName) ? l.ProductName! : (l.LineName ?? $"Product #{l.ProductId}"),
                UnitPrice = l.UnitPrice,
                Quantity = l.Quantity,
                LineTotal = l.LineTotal,
                AvailableStock = 0
            })
            .ToList();

        var advance = await _db.OrderAdvancePayments.AsNoTracking()
            .Where(a => a.OrderId == orderId)
            .OrderBy(a => a.SortOrder)
            .FirstOrDefaultAsync(cancellationToken);

        var totalIncl = await _db.OrderLines.AsNoTracking()
            .Where(l => l.OrderId == orderId)
            .SumAsync(l => l.TotalInclVat, cancellationToken);

        var isPrePay = advance is not null;
        var molliePaymentId = StoreAdvancePaymentEncoding.ExtractMolliePaymentId(advance?.Name);
        var paymentStatus = advance?.AdvancePaymentVisibility ?? (isPrePay ? "open" : "invoice");
        var isPaid = advance?.InvoicedAt != null ||
                     string.Equals(paymentStatus, "paid", StringComparison.OrdinalIgnoreCase);

        return new CheckoutOrderSummaryDto
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            CreatedAt = order.CreatedAt,
            TotalInclVat = totalIncl,
            PaymentStatus = paymentStatus,
            IsPrePay = isPrePay,
            IsPaid = isPaid || !isPrePay,
            MolliePaymentId = molliePaymentId,
            Lines = lines
        };
    }

    private static OrderLine CreateOrderLine(
        int orderId,
        StoreOrderLineCreate line,
        int sortOrder,
        decimal vatPct,
        int productType,
        int reportingGroupId,
        int vatTypeId)
    {
        return new OrderLine
        {
            OrderId = orderId,
            ProductId = line.ProductId,
            Quantity = line.Quantity,
            UnitPrice = line.UnitPrice,
            AssemblyPrice = 0,
            InstallationPrice = 0,
            TotalExclVat = line.LineTotalExclVat,
            TotalInclVat = line.LineTotalInclVat,
            IsOption = false,
            Discount = 0,
            Btw = vatPct,
            UpliftType = "Standard",
            DocumentDisplayName = line.ProductName,
            Lengte = 0,
            Hoogte = 0,
            Breedte = 0,
            Assemblage = false,
            Montage = false,
            QuoteNotesHeader = "",
            MontageStukprijs = 0,
            AssemblageStukprijs = 0,
            ProductionGroupId = "WEB",
            PurchaseOrderNotes = "",
            ProductType = productType,
            BtwBedrag = line.VatAmount,
            SortOrder = sortOrder,
            DocumentDisplayNameFr = line.ProductName,
            AantalSubProduct = 0,
            AantalTeBestellen = 0,
            ReportingGroupId = reportingGroupId,
            BrutoAankoopprijs = 0,
            NettoAankoopPrijs = 0,
            WinstPercentage = 0,
            OrigineleKorting = 0,
            UpliftTypeOrigineel = "Standard",
            BasePrice = line.UnitPrice,
            BebatAantal = 0,
            BebatStukPrijs = 0,
            BebatTotaal = 0,
            RecupelAantal = 0,
            RecupelStukPrijs = 0,
            RecupelTotaal = 0,
            IsGarantie = false,
            TotaalExclVoorCommissie = line.LineTotalExclVat,
            PopUpRow = false,
            Selected = false,
            UnitParameter = 1,
            BasisPrijsTellen = true,
            ShowOnQuote = true,
            ShowOnOrderConfirmation = true,
            ShowOnInvoice = true,
            ShowOnPackingSlip = true,
            ShowOnDeliveryNote = true,
            ShowOnProductionOrder = true,
            ToonOpLakBon = false,
            ShowOnInstallationOrder = false,
            ToonOmschrijvingOpFactuur = true,
            ToonOmschrijvingOpPakbon = true,
            ToonOmschrijvingOpLeverbon = true,
            ToonOmschrijvingOpProductiebon = true,
            ToonOmschrijvingOpLakBon = false,
            ToonOmschrijvingOpOfferte = true,
            ToonOpVrachtbrief = false,
            ToonOmschrijvingOpVrachtbrief = false,
            ToonOmschrijvingOpOrderbevestiging = true,
            StartupCost = 0,
            OpstartKostTotaal = 0,
            BasisPrijsTotaal = line.LineTotalExclVat,
            VatTypeId = vatTypeId,
            PieceUnitPrice = line.UnitPrice,
            ReportRecupel = false,
            ReportBebat = false,
            AdvancePaymentVisibility = "Default",
            Goederen = line.LineTotalExclVat,
            Diensten = 0
        };
    }

    private static OrderLine CreateDeliveryLine(
        int orderId,
        int sortOrder,
        decimal exclVat,
        decimal vatAmount,
        decimal vatPct,
        int productType,
        int reportingGroupId,
        int vatTypeId)
    {
        var line = CreateOrderLine(orderId, new StoreOrderLineCreate
        {
            ProductId = 1,
            ProductName = "Standard delivery",
            Quantity = 1,
            UnitPrice = exclVat,
            LineTotalExclVat = exclVat,
            VatAmount = vatAmount,
            LineTotalInclVat = exclVat + vatAmount
        }, sortOrder, vatPct, productType, reportingGroupId, vatTypeId);

        line.ProductId = null;
        line.DocumentDisplayName = "Standard delivery";
        line.DocumentDisplayNameFr = "Standard delivery";
        line.IsLeveringsTypeProduct = true;
        return line;
    }

    private static OrderLine CreateOptionLine(
        int orderId,
        int parentLineId,
        StoreOrderLineCreate parent,
        StoreOrderLineOptionCreate option,
        int sortOrder,
        decimal vatPct,
        int productType,
        int reportingGroupId,
        int vatTypeId)
    {
        var display = string.IsNullOrWhiteSpace(option.ValueText)
            ? option.OptionName
            : $"{option.OptionName}: {option.ValueText}";

        var line = CreateOrderLine(orderId, new StoreOrderLineCreate
        {
            ProductId = parent.ProductId,
            ProductName = display,
            Quantity = parent.Quantity,
            UnitPrice = 0,
            LineTotalExclVat = 0,
            VatAmount = 0,
            LineTotalInclVat = 0
        }, sortOrder, vatPct, productType, reportingGroupId, vatTypeId);

        line.ProductId = null;
        line.IsOption = true;
        line.IsTextOnly = true;
        line.BasisPrijsTellen = false;
        line.ProductOptionId = option.ProductOptionId;
        line.ProductOptionHoofdDetaillId = parentLineId;
        line.DocumentDisplayName = display;
        line.DocumentDisplayNameFr = display;
        line.PopUpDataXml = BuildOptionXml(option);
        line.BasePrice = 0;
        line.PieceUnitPrice = 0;
        line.BasisPrijsTotaal = 0;
        line.TotaalExclVoorCommissie = 0;
        line.Goederen = 0;
        return line;
    }

    private static string BuildOptionXml(StoreOrderLineOptionCreate option)
    {
        var name = System.Security.SecurityElement.Escape(option.OptionName) ?? string.Empty;
        var value = System.Security.SecurityElement.Escape(option.ValueText) ?? string.Empty;
        var valueIdAttr = option.ProductOptionValueId is int valueId ? $" valueId=\"{valueId}\"" : string.Empty;
        return $"<ProductOption id=\"{option.ProductOptionId}\"{valueIdAttr} name=\"{name}\">{value}</ProductOption>";
    }

    public Task<IReadOnlyDictionary<int, string>> GetProductNamesAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default) =>
        _dbGate.RunAsync(() => GetProductNamesCoreAsync(productIds, cancellationToken), cancellationToken);

    private async Task<IReadOnlyDictionary<int, string>> GetProductNamesCoreAsync(IEnumerable<int> productIds, CancellationToken cancellationToken)
    {
        var ids = productIds.Distinct().ToList();
        if (ids.Count == 0)
        {
            return new Dictionary<int, string>();
        }

        return await _db.Products.AsNoTracking()
            .Where(p => ids.Contains(p.ProductId))
            .ToDictionaryAsync(p => p.ProductId, p => p.NameEn, cancellationToken);
    }

    public async Task UpdateAdvancePaymentMollieAsync(int orderId, string paymentId, string status, string checkoutUrl, CancellationToken cancellationToken = default)
    {
        var advance = await _db.OrderAdvancePayments.FirstOrDefaultAsync(a => a.OrderId == orderId, cancellationToken);
        if (advance is null)
        {
            return;
        }

        // Persist Mollie id + status in existing Naam / Voorschotzichtbaarheid only.
        // Checkout URL is not stored (ephemeral redirect).
        advance.Name = StoreAdvancePaymentEncoding.BuildName(paymentId);
        advance.AdvancePaymentVisibility = status;
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StoreOrderListItemDto>> GetOrdersForCustomerAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        var orders = await (
            from o in _db.Orders.AsNoTracking()
            join p in _db.Projects.AsNoTracking() on o.ProjectId equals p.ProjectId
            where p.CustomerId == customerId
            orderby o.CreatedAt descending
            select new { o.Id, o.OrderNumber, o.CreatedAt }
        ).Take(50).ToListAsync(cancellationToken);

        if (orders.Count == 0)
        {
            return [];
        }

        var orderIds = orders.Select(o => o.Id).ToList();

        var advances = (await _db.OrderAdvancePayments.AsNoTracking()
            .Where(a => orderIds.Contains(a.OrderId))
            .OrderBy(a => a.SortOrder)
            .ToListAsync(cancellationToken))
            .GroupBy(a => a.OrderId)
            .ToDictionary(g => g.Key, g => g.First());

        var totals = await _db.OrderLines.AsNoTracking()
            .Where(l => orderIds.Contains(l.OrderId))
            .GroupBy(l => l.OrderId)
            .Select(g => new { OrderId = g.Key, TotalInclVat = g.Sum(l => l.TotalInclVat) })
            .ToDictionaryAsync(x => x.OrderId, x => x.TotalInclVat, cancellationToken);

        var lineSummaries = await (
            from l in _db.OrderLines.AsNoTracking()
            join prod in _db.Products.AsNoTracking() on l.ProductId equals prod.ProductId into prodJoin
            from prod in prodJoin.DefaultIfEmpty()
            where orderIds.Contains(l.OrderId) && l.ProductId != null
            orderby l.OrderId, l.SortOrder
            select new
            {
                l.OrderId,
                ProductName = prod.NameEn,
                LineName = l.DocumentDisplayName,
                l.Quantity
            }).ToListAsync(cancellationToken);

        var summaryByOrder = lineSummaries
            .GroupBy(x => x.OrderId)
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var items = g.Take(3).Select(x =>
                    {
                        var name = !string.IsNullOrWhiteSpace(x.ProductName)
                            ? x.ProductName!
                            : (x.LineName ?? "Item");
                        return $"{name} ×{(int)x.Quantity}";
                    });
                    return string.Join(", ", items)
                           + (g.Count() > 3 ? $" +{g.Count() - 3} more" : "");
                });

        return orders.Select(o =>
        {
            advances.TryGetValue(o.Id, out var advance);
            var isPrePay = advance is not null;
            var paymentStatus = advance?.AdvancePaymentVisibility ?? (isPrePay ? "open" : "invoice");
            var isPaid = advance?.InvoicedAt != null ||
                         string.Equals(paymentStatus, "paid", StringComparison.OrdinalIgnoreCase) ||
                         !isPrePay;
            var (label, css) = StoreOrderPaymentDisplay.Describe(isPrePay, isPaid, paymentStatus);

            return new StoreOrderListItemDto
            {
                OrderId = o.Id,
                OrderNumber = o.OrderNumber,
                CreatedAt = o.CreatedAt,
                TotalInclVat = totals.GetValueOrDefault(o.Id),
                PaymentStatus = paymentStatus,
                IsPrePay = isPrePay,
                IsPaid = isPaid,
                ItemsSummary = summaryByOrder.GetValueOrDefault(o.Id, "—"),
                StatusLabel = label,
                StatusClass = css
            };
        }).ToList();
    }

    /// <summary>
    /// Webshop only enables Mollie/online methods. Cash, wire and invoice stay visible but disabled.
    /// </summary>
    private static bool IsStorefrontMollieMethod(
        string? nameEn,
        string? nameNl,
        string? nameFr,
        bool isPrePay,
        bool isPostPay)
    {
        var blob = $"{nameEn}|{nameNl}|{nameFr}".ToLowerInvariant();

        if (blob.Contains("contant")
            || blob.Contains("cash")
            || blob.Contains("wire")
            || blob.Contains("factuur")
            || blob.Contains("invoice")
            || (blob.Contains("overschrijving") && !blob.Contains("mollie")))
        {
            return false;
        }

        if (blob.Contains("mollie")
            || blob.Contains("ideal")
            || blob.Contains("i deal")
            || blob.Contains("bancontact")
            || blob.Contains("bankcontact")
            || blob.Contains("creditcard")
            || blob.Contains("credit card"))
        {
            return true;
        }

        return isPrePay && !isPostPay;
    }

    private static string PreferPaymentName(string? nameEn, string? nameNl, string? nameFr)
    {
        if (!string.IsNullOrWhiteSpace(nameEn))
        {
            return nameEn;
        }

        if (!string.IsNullOrWhiteSpace(nameNl))
        {
            return nameNl;
        }

        return string.IsNullOrWhiteSpace(nameFr) ? "Payment method" : nameFr;
    }
}
