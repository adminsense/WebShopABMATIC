namespace WebShopABMATIC.Application.Admin.Hubs;

public sealed class AdminHubDefinitionDto
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Subtitle { get; init; }
    public required string IconClass { get; init; }
    public required IReadOnlyList<AdminHubCardDto> Cards { get; init; }
}

public sealed class AdminHubCardDto
{
    public required string Entity { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string ListRoute { get; init; }
    public string? FormRoute { get; init; }
    public required string IconClass { get; init; }
    public required string ColorHex { get; init; }
}
