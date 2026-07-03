using System.ComponentModel.DataAnnotations;

namespace WebShopABMATIC.Web.Forms;

public sealed class AdminProfileFormModel
{
    [Required]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}
//sealed protect
public sealed class AdminPasswordFormModel
{
    [Required]
    public string CurrentPassword { get; set; } = "";

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string NewPassword { get; set; } = "";

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = "";
}
