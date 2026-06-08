namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IAuditSuppressionContext
{
    IDisposable SuppressEntityTypes(params string[] entityTypeNames);

    bool IsSuppressed(string entityTypeName);
}

public interface ILowStockEmailNotifier
{
    Task NotifyAsync(
        int productId,
        string productName,
        string stockLocationName,
        decimal quantity,
        decimal minQuantity,
        CancellationToken cancellationToken = default);
}
