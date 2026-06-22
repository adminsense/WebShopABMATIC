namespace WebShopABMATIC.Application.Stock;

public enum StockApplyStatus
{
    Applied,
    AlreadyApplied,
    Skipped,
    Failed
}

public sealed class StockApplyResult
{
    public StockApplyStatus Status { get; init; }
    public int MovementsCreated { get; init; }
    public int? MovementId { get; init; }
    public decimal? NewBalance { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];

    public bool IsSuccess => Status is StockApplyStatus.Applied or StockApplyStatus.AlreadyApplied or StockApplyStatus.Skipped;

    public static StockApplyResult Applied(int movementsCreated, int? movementId = null, decimal? newBalance = null) =>
        new()
        {
            Status = StockApplyStatus.Applied,
            MovementsCreated = movementsCreated,
            MovementId = movementId,
            NewBalance = newBalance
        };

    public static StockApplyResult AlreadyApplied() =>
        new() { Status = StockApplyStatus.AlreadyApplied };

    public static StockApplyResult Skipped(string reason) =>
        new() { Status = StockApplyStatus.Skipped, Errors = [reason] };

    public static StockApplyResult Failed(IReadOnlyList<string> errors) =>
        new() { Status = StockApplyStatus.Failed, Errors = errors };
}
