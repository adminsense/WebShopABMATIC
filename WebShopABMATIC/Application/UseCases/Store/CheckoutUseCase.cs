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

        var current = await _currentUser.GetCurrentUserAsync(cancellationToken);
        var lookup = new StoreUserLookup
        {
            Email = request.UserEmail,
            CustomerId = current.CustomerId
        };
        var ctx = await _customers.GetForStoreUserAsync(lookup, cancellationToken);
        if (ctx is null)
        {
            return EmptyQuote(["Customer account not linked to this login."]);
        }

        var productIds = request.Lines.Select(l => l.ProductId).Distinct().ToList();
        var prices = await _pricing.GetCatalogPricesAsync(productIds, ctx.CustomerId, cancellationToken);
        var stock = await _orders.GetAvailableStockAsync(productIds, cancellationToken);
        var names = await _orders.GetProductNamesAsync(productIds, cancellationToken);
        var options = await _orders.GetCheckoutOptionsAsync(ctx.CustomerId, cancellationToken);
        var freightOptions = options?.FreightOptions ?? [];

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
        var deliveryProductId = request.DeliveryProductId is > 0 ? request.DeliveryProductId : null;
        decimal delivery = 0m;
        var deliveryLabel = "No delivery charge";
        int? resolvedDeliveryProductId = null;

        if (deliveryProductId is int freightProductId)
        {
            var freight = freightOptions.FirstOrDefault(f => f.ProductId == freightProductId);
            if (freight is null)
            {
                errors.Add("Selected delivery option is not valid for your account.");
            }
            else
            {
                // Missing ERP price → €0 (admin can fix ProductPrices later).
                delivery = Math.Max(0m, freight.UnitPrice);
                deliveryLabel = freight.Name;
                resolvedDeliveryProductId = freight.ProductId;
            }
        }

        var vatAmount = Math.Round((subtotal + delivery) * VatPercentage / 100m, 2);

        return new CheckoutQuoteDto
        {
            Lines = quoteLines,
            Subtotal = subtotal,
            DeliveryFee = delivery,
            DeliveryLabel = deliveryLabel,
            DeliveryProductId = resolvedDeliveryProductId,
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
            DeliveryProductId = request.DeliveryProductId
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

        var deliveryAddressId = request.DeliveryAddressId;
        if (!options.DeliveryAddresses.Any(a => a.Id == deliveryAddressId))
        {
            // Blazor <select> can leave Id=0 when the user never changed the dropdown.
            if (deliveryAddressId <= 0 && options.DeliveryAddresses.Count > 0)
            {
                deliveryAddressId = options.DeliveryAddresses[0].Id;
            }
            else
            {
                return new CheckoutResult { Success = false, Errors = ["Invalid delivery address."] };
            }
        }

        var paymentMethodId = request.PaymentMethodId;
        var paymentMethod = options.PaymentMethods.FirstOrDefault(p => p.Id == paymentMethodId && p.IsSelectable);
        if (paymentMethod is null)
        {
            paymentMethod = options.PaymentMethods.FirstOrDefault(p => p.IsSelectable);
            if (paymentMethod is null)
            {
                return new CheckoutResult
                {
                    Success = false,
                    Errors = ["Online payment (Mollie) is not configured. Ask an administrator to enable a PrePay / Bancontact method."]
                };
            }

            paymentMethodId = paymentMethod.Id;
        }

        // Quote lines are produced 1:1 (and in order) from request.Lines whenever there
        // are no errors, so we can align them by index to recover the selected options.
        var lineCreates = quote.Lines.Select((l, index) => new StoreOrderLineCreate
        {
            ProductId = l.ProductId,
            ProductName = l.Name,
            Quantity = l.Quantity,
            UnitPrice = l.UnitPrice,
            LineTotalExclVat = l.LineTotal,
            VatAmount = Math.Round(l.LineTotal * VatPercentage / 100m, 2),
            LineTotalInclVat = l.LineTotal + Math.Round(l.LineTotal * VatPercentage / 100m, 2),
            Options = MapLineOptions(request.Lines, index, l.ProductId)
        }).ToList();

        var currentUser = await _currentUser.GetCurrentUserAsync(cancellationToken);
        var createdByUserId = currentUser.ResolveLegacyUserId(ctx.AccountManagerUserId);

        var orderCommand = new StoreOrderCreateCommand
        {
            CustomerId = ctx.CustomerId,
            ProjectId = ctx.ProjectId,
            CustomerTypeId = ctx.CustomerTypeId,
            DeliveryTypeId = ctx.DeliveryTypeId,
            BetaaltermijnId = ctx.BetaaltermijnId,
            DeliveryAddressId = deliveryAddressId,
            PaymentMethodId = paymentMethodId,
            CreatedByUserId = createdByUserId,
            IsPrePay = paymentMethod.IsPrePay,
            DeliveryFee = quote.DeliveryFee,
            DeliveryProductId = quote.DeliveryProductId,
            DeliveryProductName = quote.DeliveryLabel,
            VatPercentage = VatPercentage,
            Lines = lineCreates
        };

        if (paymentMethod.IsPrePay)
        {
            StoreOrderCreated prePayCreated;
            try
            {
                prePayCreated = await _orders.CreateWebshopOrderAsync(orderCommand, cancellationToken);
            }
            catch (Exception ex)
            {
                return new CheckoutResult
                {
                    Success = false,
                    Errors = [$"Could not create order: {ex.GetBaseException().Message}"]
                };
            }

            var reserveResult = await _stock.ApplyReservationFromOrderAsync(prePayCreated.OrderId, cancellationToken);
            if (!reserveResult.IsSuccess)
            {
                return new CheckoutResult
                {
                    Success = false,
                    Errors = reserveResult.Errors.Count > 0
                        ? reserveResult.Errors
                        : ["Could not reserve stock for this order."]
                };
            }

            await LogCheckoutStartedAsync(
                prePayCreated.OrderId,
                prePayCreated.OrderNumber,
                prePayCreated.TotalInclVat,
                isPrePay: true,
                ctx.CustomerId,
                cancellationToken);

            var paymentReturnUrl =
                $"{request.RedirectBaseUrl.TrimEnd('/')}/orders/{prePayCreated.OrderId}/payment-return";
            var webhookUrl =
                $"{request.WebhookBaseUrl.TrimEnd('/')}/api/webhooks/mollie/payments";

            try
            {
                var payment = await _mollie.CreatePaymentAsync(new CreateMolliePaymentCommand
                {
                    Amount = prePayCreated.TotalInclVat,
                    Currency = "EUR",
                    Description = $"WebShop order {prePayCreated.OrderNumber ?? prePayCreated.OrderId}",
                    RedirectUrl = paymentReturnUrl,
                    WebhookUrl = webhookUrl,
                    MetadataJson = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        orderId = prePayCreated.OrderId,
                        advancePaymentId = prePayCreated.AdvancePaymentId
                    })
                }, cancellationToken);

                await _orders.UpdateAdvancePaymentMollieAsync(
                    prePayCreated.OrderId,
                    payment.PaymentId,
                    payment.Status,
                    payment.CheckoutUrl,
                    cancellationToken);

                return new CheckoutResult
                {
                    Success = true,
                    OrderId = prePayCreated.OrderId,
                    RedirectUrl = payment.CheckoutUrl
                };
            }
            catch (Exception ex)
            {
                return new CheckoutResult
                {
                    Success = false,
                    Errors = [$"Online payment could not be started: {ex.Message}"]
                };
            }
        }

        StoreOrderCreated postPayCreated;
        try
        {
            postPayCreated = await _orders.CreateWebshopOrderAsync(orderCommand, cancellationToken);
        }
        catch (Exception ex)
        {
            return new CheckoutResult
            {
                Success = false,
                Errors = [$"Could not create order: {ex.GetBaseException().Message}"]
            };
        }

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

        if (summary is { IsPrePay: true, IsPaid: false } &&
            IsTerminalUnpaidStatus(summary.PaymentStatus))
        {
            await _stock.ReleaseReservationAsync(orderId, cancellationToken);
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

    private static bool IsTerminalUnpaidStatus(string? status) =>
        status is not null &&
        (status.Equals("expired", StringComparison.OrdinalIgnoreCase) ||
         status.Equals("canceled", StringComparison.OrdinalIgnoreCase) ||
         status.Equals("failed", StringComparison.OrdinalIgnoreCase));

    private static IReadOnlyList<StoreOrderLineOptionCreate> MapLineOptions(
        IReadOnlyList<CheckoutLineRequest> requestLines,
        int index,
        int productId)
    {
        // Prefer positional alignment (quote lines mirror request lines 1:1 when valid);
        // fall back to the first request line for this product if indexes ever drift.
        var source = index >= 0 && index < requestLines.Count && requestLines[index].ProductId == productId
            ? requestLines[index]
            : requestLines.FirstOrDefault(l => l.ProductId == productId);

        if (source is null || source.Options.Count == 0)
        {
            return [];
        }

        return source.Options
            .Select(o => new StoreOrderLineOptionCreate
            {
                ProductOptionId = o.ProductOptionId,
                ProductOptionValueId = o.ProductOptionValueId,
                OptionName = o.OptionName,
                ValueText = o.ValueText
            })
            .ToList();
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
