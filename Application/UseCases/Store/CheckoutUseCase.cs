using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Payments;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Checkout;
using WebShopABMATIC.Application.Store.Orders;
namespace WebShopABMATIC.Application.UseCases.Store;

public sealed class CheckoutUseCase : ICheckoutPort
{
    private const decimal DeliveryFee = 9.00m;
    private const decimal VatPercentage = 21m;

    private readonly IStoreCustomerRepository _customers;
    private readonly IStoreOrderRepository _orders;
    private readonly IProductPricingPort _pricing;
    private readonly IMolliePaymentPort _mollie;
    private readonly IMollieWebhookPort _webhook;
    private readonly IStockMovementService _stock;
    private readonly ICurrentUserContext _currentUser;
    private readonly IAuditService _audit;

    public CheckoutUseCase(
        IStoreCustomerRepository customers,
        IStoreOrderRepository orders,
        IProductPricingPort pricing,
        IMolliePaymentPort mollie,
        IMollieWebhookPort webhook,
        IStockMovementService stock,
        ICurrentUserContext currentUser,
        IAuditService audit)
    {
        _customers = customers;
        _orders = orders;
        _pricing = pricing;
        _mollie = mollie;
        _webhook = webhook;
        _stock = stock;
        _currentUser = currentUser;
        _audit = audit;
    }

    public async Task<CheckoutOptionsDto?> GetOptionsAsync(StoreUserLookup user, CancellationToken cancellationToken = default)
    {
        var ctx = await _customers.GetForStoreUserAsync(user, cancellationToken);
        return ctx is null ? null : await _orders.GetCheckoutOptionsAsync(ctx.CustomerId, cancellationToken);
    }

    public async Task<CheckoutQuoteDto> BuildQuoteAsync(CheckoutQuoteRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Lines.Count == 0)
        {
            return EmptyQuote(["Your cart is empty."]);
        }

        var lookup = new StoreUserLookup { IdentityUserId = request.IdentityUserId, Email = request.UserEmail };
        var ctx = await _customers.GetForStoreUserAsync(lookup, cancellationToken);
        if (ctx is null)
        {
            return EmptyQuote(["Customer account not linked to this login."]);
        }

        var productIds = request.Lines.Select(l => l.ProductId).Distinct().ToList();
        var prices = await _pricing.GetCatalogPricesAsync(productIds, ctx.CustomerId, cancellationToken);
        var stock = await _orders.GetAvailableStockAsync(productIds, cancellationToken);
        var names = await _orders.GetProductNamesAsync(productIds, cancellationToken);

        var errors = new List<string>();
        var quoteLines = new List<CheckoutLineQuoteDto>();
        foreach (var line in request.Lines)
        {
            if (!prices.TryGetValue(line.ProductId, out var unitPrice))
            {
                errors.Add($"Product {line.ProductId} has no active price.");
                continue;
            }

            stock.TryGetValue(line.ProductId, out var available);
            if (line.Quantity > available)
            {
                errors.Add($"{names.GetValueOrDefault(line.ProductId, $"Product {line.ProductId}")}: only {available} in stock.");
            }

            quoteLines.Add(new CheckoutLineQuoteDto
            {
                ProductId = line.ProductId,
                Name = names.GetValueOrDefault(line.ProductId, $"Product {line.ProductId}"),
                UnitPrice = unitPrice,
                Quantity = line.Quantity,
                LineTotal = unitPrice * line.Quantity,
                AvailableStock = available
            });
        }

        var subtotal = quoteLines.Sum(l => l.LineTotal);
        var delivery = quoteLines.Count > 0 ? DeliveryFee : 0m;
        var vatAmount = Math.Round((subtotal + delivery) * VatPercentage / 100m, 2);

        return new CheckoutQuoteDto
        {
            Lines = quoteLines,
            Subtotal = subtotal,
            DeliveryFee = delivery,
            VatAmount = vatAmount,
            Total = subtotal + delivery + vatAmount,
            Errors = errors
        };
    }

    public async Task<CheckoutResult> PlaceOrderAsync(CheckoutRequest request, StoreUserLookup user, CancellationToken cancellationToken = default)
    {
        var quote = await BuildQuoteAsync(new CheckoutQuoteRequest
        {
            Lines = request.Lines,
            UserEmail = user.Email ?? "",
            IdentityUserId = user.IdentityUserId
        }, cancellationToken);

        if (quote.Errors.Count > 0)
        {
            return new CheckoutResult { Success = false, Errors = quote.Errors };
        }

        var ctx = await _customers.GetForStoreUserAsync(user, cancellationToken);
        if (ctx is null)
        {
            return new CheckoutResult { Success = false, Errors = ["Customer account not linked to this login."] };
        }

        var options = await _orders.GetCheckoutOptionsAsync(ctx.CustomerId, cancellationToken);
        if (options is null)
        {
            return new CheckoutResult { Success = false, Errors = ["Checkout options unavailable."] };
        }

        if (!options.DeliveryAddresses.Any(a => a.Id == request.DeliveryAddressId))
        {
            return new CheckoutResult { Success = false, Errors = ["Invalid delivery address."] };
        }

        var paymentMethod = options.PaymentMethods.FirstOrDefault(p => p.Id == request.PaymentMethodId);
        if (paymentMethod is null)
        {
            return new CheckoutResult { Success = false, Errors = ["Invalid payment method."] };
        }

        var lineCreates = quote.Lines.Select(l => new StoreOrderLineCreate
        {
            ProductId = l.ProductId,
            ProductName = l.Name,
            Quantity = l.Quantity,
            UnitPrice = l.UnitPrice,
            LineTotalExclVat = l.LineTotal,
            VatAmount = Math.Round(l.LineTotal * VatPercentage / 100m, 2),
            LineTotalInclVat = l.LineTotal + Math.Round(l.LineTotal * VatPercentage / 100m, 2)
        }).ToList();

        var currentUser = await _currentUser.GetCurrentUserAsync(cancellationToken);
        var createdByUserId = currentUser.ResolveLegacyUserId(ctx.AccountManagerUserId);

        if (paymentMethod.IsPrePay)
        {
            var created = await _orders.CreateWebshopOrderAsync(new StoreOrderCreateCommand
            {
                CustomerId = ctx.CustomerId,
                ProjectId = ctx.ProjectId,
                CustomerTypeId = ctx.CustomerTypeId,
                DeliveryTypeId = ctx.DeliveryTypeId,
                BetaaltermijnId = ctx.BetaaltermijnId,
                DeliveryAddressId = request.DeliveryAddressId,
                PaymentMethodId = request.PaymentMethodId,
                CreatedByUserId = createdByUserId,
                IsPrePay = true,
                DeliveryFee = quote.DeliveryFee,
                VatPercentage = VatPercentage,
                Lines = lineCreates,
                MolliePaymentStatus = "open"
            }, cancellationToken);

            try
            {
                var redirectUrl = $"{request.RedirectBaseUrl.TrimEnd('/')}/orders/{created.OrderId}/payment-return";
                var webhookUrl = $"{request.WebhookBaseUrl.TrimEnd('/')}/api/webhooks/mollie/payments";
                var payment = await _mollie.CreatePaymentAsync(new CreateMolliePaymentCommand
                {
                    Amount = created.TotalInclVat,
                    Currency = "EUR",
                    Description = $"Webshop order #{created.OrderNumber ?? created.OrderId}",
                    RedirectUrl = redirectUrl,
                    WebhookUrl = webhookUrl,
                    MetadataJson = $"{{\"orderId\":{created.OrderId}}}"
                }, cancellationToken);

                await _orders.UpdateAdvancePaymentMollieAsync(created.OrderId, payment.PaymentId, payment.Status, payment.CheckoutUrl, cancellationToken);

                await LogCheckoutStartedAsync(created.OrderId, created.OrderNumber, created.TotalInclVat, paymentMethod.IsPrePay, ctx.CustomerId, cancellationToken);

                return new CheckoutResult
                {
                    Success = true,
                    OrderId = created.OrderId,
                    RedirectUrl = payment.CheckoutUrl
                };
            }
            catch (Exception ex)
            {
                return new CheckoutResult
                {
                    Success = false,
                    OrderId = created.OrderId,
                    Errors = [$"Payment could not be started: {ex.Message}"]
                };
            }
        }

        var postPayCreated = await _orders.CreateWebshopOrderAsync(new StoreOrderCreateCommand
        {
            CustomerId = ctx.CustomerId,
            ProjectId = ctx.ProjectId,
            CustomerTypeId = ctx.CustomerTypeId,
            DeliveryTypeId = ctx.DeliveryTypeId,
            BetaaltermijnId = ctx.BetaaltermijnId,
            DeliveryAddressId = request.DeliveryAddressId,
            PaymentMethodId = request.PaymentMethodId,
            CreatedByUserId = createdByUserId,
            IsPrePay = false,
            DeliveryFee = quote.DeliveryFee,
            VatPercentage = VatPercentage,
            Lines = lineCreates
        }, cancellationToken);

        await LogCheckoutStartedAsync(postPayCreated.OrderId, postPayCreated.OrderNumber, postPayCreated.TotalInclVat, isPrePay: false, ctx.CustomerId, cancellationToken);

        await _stock.ApplySaleFromOrderAsync(postPayCreated.OrderId, cancellationToken);

        return new CheckoutResult
        {
            Success = true,
            OrderId = postPayCreated.OrderId,
            ConfirmationUrl = $"{request.RedirectBaseUrl.TrimEnd('/')}/orders/{postPayCreated.OrderId}/confirmation"
        };
    }

    public async Task<CheckoutOrderSummaryDto?> GetOrderSummaryAsync(int orderId, StoreUserLookup user, CancellationToken cancellationToken = default)
    {
        var ctx = await _customers.GetForStoreUserAsync(user, cancellationToken);
        if (ctx is null)
        {
            return null;
        }

        var summary = await _orders.GetOrderSummaryForCustomerAsync(orderId, ctx.CustomerId, cancellationToken);
        if (summary is { IsPrePay: true, IsPaid: false, MolliePaymentId: not null and not "" })
        {
            await _webhook.ProcessPaymentAsync(summary.MolliePaymentId, cancellationToken);
            summary = await _orders.GetOrderSummaryForCustomerAsync(orderId, ctx.CustomerId, cancellationToken);
        }

        if (summary is { IsPaid: true })
        {
            await _stock.ApplySaleFromOrderAsync(orderId, cancellationToken);
        }

        return summary;
    }

    public async Task<IReadOnlyList<StoreOrderListItemDto>> GetCustomerOrdersAsync(
        StoreUserLookup user,
        CancellationToken cancellationToken = default)
    {
        var ctx = await _customers.GetForStoreUserAsync(user, cancellationToken);
        if (ctx is null)
        {
            return [];
        }

        return await _orders.GetOrdersForCustomerAsync(ctx.CustomerId, cancellationToken);
    }

    private static CheckoutQuoteDto EmptyQuote(IReadOnlyList<string> errors) =>
        new() { Errors = errors };

    private Task LogCheckoutStartedAsync(
        int orderId,
        int? orderNumber,
        decimal totalInclVat,
        bool isPrePay,
        int customerId,
        CancellationToken cancellationToken) =>
        _audit.LogAsync(new AuditLogWriteRequest
        {
            Action = AuditActions.CheckoutStarted,
            EntityName = "Order",
            EntityId = orderId.ToString(),
            NewValues = System.Text.Json.JsonSerializer.Serialize(new
            {
                orderId,
                orderNumber,
                totalInclVat,
                isPrePay,
                customerId
            })
        }, cancellationToken);
}
