namespace WebShopABMATIC.Application.Store.Profile;

public sealed class StoreProfileDto
{
    public int CustomerId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string HouseNumber { get; init; } = string.Empty;
    public string Box { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
    public string CityName { get; init; } = string.Empty;
}

public sealed class StoreProfileUpdateDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Phone { get; init; }
    public required string Street { get; init; }
    public required string HouseNumber { get; init; }
    public string? Box { get; init; }
    public required string PostalCode { get; init; }
    public required string CityName { get; init; }
}

public sealed class StoreProfileSaveResult
{
    public required bool Succeeded { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];
}
