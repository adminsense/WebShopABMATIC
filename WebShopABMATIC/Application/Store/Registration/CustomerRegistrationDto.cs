namespace WebShopABMATIC.Application.Store.Registration;

public sealed class CustomerRegistrationRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Phone { get; init; }
    public required string Street { get; init; }
    public required string HouseNumber { get; init; }
    public string? Box { get; init; }
    public required string PostalCode { get; init; }
    public required string CityName { get; init; }
}

public sealed class CustomerRegistrationResult
{
    public required bool Succeeded { get; init; }
    public int? CustomerId { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];
}
