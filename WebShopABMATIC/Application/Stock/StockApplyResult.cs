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
    public int? OutMovementId { get; init; }
    public int? InMovementId { get; init; }
    public decimal? NewBalance { get; init; }
    public decimal? FromNewBalance { get; init; }
    public decimal? ToNewBalance { get; init; }
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

    public static StockApplyResult TransferApplied(
        int outMovementId,
        int inMovementId,
        decimal fromNewBalance,
        decimal toNewBalance) =>
        new()
        {
            Status = StockApplyStatus.Applied,
            MovementsCreated = 2,
            OutMovementId = outMovementId,
            InMovementId = inMovementId,
            FromNewBalance = fromNewBalance,
            ToNewBalance = toNewBalance
        };

    public static StockApplyResult AlreadyApplied() =>
        new() { Status = StockApplyStatus.AlreadyApplied };

    public static StockApplyResult Skipped(string reason) =>
        new() { Status = StockApplyStatus.Skipped, Errors = [reason] };

    public static StockApplyResult Failed(IReadOnlyList<string> errors) =>
        new() { Status = StockApplyStatus.Failed, Errors = errors };
}
