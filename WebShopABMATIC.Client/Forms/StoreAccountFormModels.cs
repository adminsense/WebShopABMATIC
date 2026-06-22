using System.ComponentModel.DataAnnotations;
using WebShopABMATIC.Application.Store.Profile;

namespace WebShopABMATIC.Web.Forms;

public sealed class StoreAccountProfileFormModel
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = "";

    [Required, StringLength(100)]
    public string LastName { get; set; } = "";

    [Required, Phone]
    public string Phone { get; set; } = "";

    [Required, StringLength(200)]
    public string Street { get; set; } = "";

    [Required, StringLength(20)]
    public string HouseNumber { get; set; } = "";

    [StringLength(20)]
    public string? Box { get; set; }

    [Required, StringLength(20)]
    public string PostalCode { get; set; } = "";

    [Required, StringLength(100)]
    public string CityName { get; set; } = "";

    public static StoreAccountProfileFormModel FromDto(StoreProfileDto dto) => new()
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Phone = dto.Phone,
        Street = dto.Street,
        HouseNumber = dto.HouseNumber,
        Box = string.IsNullOrWhiteSpace(dto.Box) ? null : dto.Box,
        PostalCode = dto.PostalCode,
        CityName = dto.CityName
    };

    public StoreProfileUpdateDto ToUpdateDto() => new()
    {
        FirstName = FirstName,
        LastName = LastName,
        Phone = Phone,
        Street = Street,
        HouseNumber = HouseNumber,
        Box = Box,
        PostalCode = PostalCode,
        CityName = CityName
    };
}

public sealed class StoreAccountPasswordFormModel
{
    [Required]
    public string CurrentPassword { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 8)]
    public string NewPassword { get; set; } = "";

    [Required, Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = "";
}
