using System.ComponentModel.DataAnnotations;

namespace WebShopABMATIC.Web.Forms;

public sealed class StoreSignUpFormModel : IValidatableObject
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 1)]
    public string FirstName { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 1)]
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

    [Required, StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required, DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = "";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Password))
        {
            yield break;
        }

        if (!Password.Any(char.IsUpper))
        {
            yield return new ValidationResult("Password must contain an uppercase letter.", [nameof(Password)]);
        }

        if (!Password.Any(char.IsLower))
        {
            yield return new ValidationResult("Password must contain a lowercase letter.", [nameof(Password)]);
        }

        if (!Password.Any(char.IsDigit))
        {
            yield return new ValidationResult("Password must contain a digit.", [nameof(Password)]);
        }
    }
}
