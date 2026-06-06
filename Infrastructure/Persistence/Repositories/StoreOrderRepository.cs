using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Store.Checkout;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StoreOrderRepository : IStoreOrderRepository
{
    private const decimal DefaultDeliveryFee = 9.00m;
    private const decimal DefaultVatPercentage = 21m;

    private readonly WebShopABMATICDbContext _db;

    public StoreOrderRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<CheckoutOptionsDto?> GetCheckoutOptionsAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var addresses = await (
            from a in _db.CustomerDeliveryAddresses.AsNoTracking()
            join c in _db.Cities.AsNoTracking() on a.CityId equals c.CityId into cityJoin
            from city in cityJoin.DefaultIfEmpty()
            where a.CustomerId == customerId
            orderby a.Name
            select new CheckoutDeliveryAddressDto
            {
                Id = a.Id,
                Label = a.Name + " — " + a.Straat + " " + a.Number + (city != null ? ", " + city.CityName : "")
            }).ToListAsync(cancellationToken);

        var paymentMethods = await _db.PaymentMethods.AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new CheckoutPaymentMethodDto
            {
                Id = p.Id,
                Name = p.NameEn,
                IsPrePay = p.IsPrePay,
                IsPostPay = p.IsPostPay
            })
            .ToListAsync(cancellationToken);

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

    public async Task<IReadOnlyDictionary<int, int>> GetAvailableStockAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default)
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

    public async Task<StoreOrderCreated> CreateWebshopOrderAsync(StoreOrderCreateCommand command, CancellationToken cancellationToken = default)
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
            CreatedByUserId = 1,
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

        var sortOrder = 1;
        foreach (var line in command.Lines)
        {
            _db.OrderLines.Add(CreateOrderLine(order.Id, line, sortOrder++, command.VatPercentage));
        }

        if (deliveryExcl > 0)
        {
            var deliveryVat = Math.Round(deliveryExcl * command.VatPercentage / 100m, 2);
            _db.OrderLines.Add(CreateDeliveryLine(order.Id, sortOrder, deliveryExcl, deliveryVat, command.VatPercentage));
        }

        int? advancePaymentId = null;
        if (command.IsPrePay)
        {
            var advance = new OrderAdvancePayment
            {
                OrderId = order.Id,
                Name = "Online payment",
                Percent = 100,
                IsFinalInvoice = false,
                SortOrder = 1,
                Amount = totalInclVat,
                AdvancePaymentVisibility = "Default",
                MolliePaymentId = command.MolliePaymentId,
                MolliePaymentStatus = command.MolliePaymentStatus ?? "open",
                MollieCheckoutUrl = command.MollieCheckoutUrl
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
        return await _db.OrderAdvancePayments.AsNoTracking()
            .Where(a => a.MolliePaymentId == molliePaymentId)
            .Select(a => new StoreAdvancePaymentInfo
            {
                Id = a.Id,
                OrderId = a.OrderId,
                MolliePaymentStatus = a.MolliePaymentStatus,
                MolliePaidAt = a.MolliePaidAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task MarkAdvancePaymentPaidAsync(int advancePaymentId, string mollieStatus, DateTime paidAt, CancellationToken cancellationToken = default)
    {
        var entity = await _db.OrderAdvancePayments.FirstAsync(a => a.Id == advancePaymentId, cancellationToken);
        entity.MolliePaymentStatus = mollieStatus;
        entity.MolliePaidAt = paidAt;
        await _db.SaveChangesAsync(cancellationToken);
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

        var lines = await (
            from l in _db.OrderLines.AsNoTracking()
            join prod in _db.Products.AsNoTracking() on l.ProductId equals prod.ProductId into prodJoin
            from prod in prodJoin.DefaultIfEmpty()
            where l.OrderId == orderId && l.ProductId != null
            orderby l.SortOrder
            select new CheckoutLineQuoteDto
            {
                ProductId = l.ProductId!.Value,
                Name = prod != null ? prod.NameEn : l.DocumentDisplayName,
                UnitPrice = l.UnitPrice,
                Quantity = (int)l.Quantity,
                LineTotal = l.TotalExclVat,
                AvailableStock = 0
            }).ToListAsync(cancellationToken);

        var advance = await _db.OrderAdvancePayments.AsNoTracking()
            .Where(a => a.OrderId == orderId)
            .OrderBy(a => a.SortOrder)
            .FirstOrDefaultAsync(cancellationToken);

        var totalIncl = await _db.OrderLines.AsNoTracking()
            .Where(l => l.OrderId == orderId)
            .SumAsync(l => l.TotalInclVat, cancellationToken);

        var isPrePay = advance is not null;
        var isPaid = advance?.MolliePaidAt != null ||
                     string.Equals(advance?.MolliePaymentStatus, "paid", StringComparison.OrdinalIgnoreCase);

        return new CheckoutOrderSummaryDto
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            CreatedAt = order.CreatedAt,
            TotalInclVat = totalIncl,
            PaymentStatus = advance?.MolliePaymentStatus ?? (isPrePay ? "open" : "invoice"),
            IsPrePay = isPrePay,
            IsPaid = isPaid || !isPrePay,
            MolliePaymentId = advance?.MolliePaymentId,
            Lines = lines
        };
    }

    private static OrderLine CreateOrderLine(int orderId, StoreOrderLineCreate line, int sortOrder, decimal vatPct)
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
            ProductType = 0,
            BtwBedrag = line.VatAmount,
            SortOrder = sortOrder,
            DocumentDisplayNameFr = line.ProductName,
            AantalSubProduct = 0,
            AantalTeBestellen = 0,
            ReportingGroupId = 1,
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
            VatTypeId = 1,
            PieceUnitPrice = line.UnitPrice,
            ReportRecupel = false,
            ReportBebat = false,
            AdvancePaymentVisibility = "Default",
            Goederen = line.LineTotalExclVat,
            Diensten = 0
        };
    }

    private static OrderLine CreateDeliveryLine(int orderId, int sortOrder, decimal exclVat, decimal vatAmount, decimal vatPct)
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
        }, sortOrder, vatPct);

        line.ProductId = null;
        line.DocumentDisplayName = "Standard delivery";
        line.DocumentDisplayNameFr = "Standard delivery";
        line.IsLeveringsTypeProduct = true;
        return line;
    }

    public async Task<IReadOnlyDictionary<int, string>> GetProductNamesAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default)
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

        advance.MolliePaymentId = paymentId;
        advance.MolliePaymentStatus = status;
        advance.MollieCheckoutUrl = checkoutUrl;
        await _db.SaveChangesAsync(cancellationToken);
    }
}
