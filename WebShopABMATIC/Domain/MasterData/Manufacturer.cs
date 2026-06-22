namespace WebShopABMATIC.Domain.MasterData;

public sealed class Manufacturer
{
    public int ManufacturerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int? CityId { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }

    public static Manufacturer Create(string name, int? cityId, string? email, string? phone, string? address) =>
        new() { Name = name, CityId = cityId, Email = email, Phone = phone, Address = address };

    public static Manufacturer Rehydrate(int id, string name, int? cityId, string? email, string? phone, string? address) =>
        new() { ManufacturerId = id, Name = name, CityId = cityId, Email = email, Phone = phone, Address = address };

    public void Update(string name, int? cityId, string? email, string? phone, string? address)
    {
        Name = name;
        CityId = cityId;
        Email = email;
        Phone = phone;
        Address = address;
    }
}
