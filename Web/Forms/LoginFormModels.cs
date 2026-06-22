using System.ComponentModel.DataAnnotations;

namespace WebShopABMATIC.Web.Forms;

public sealed class AdminLoginFormModel
{
    [Required]
    public string Login { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public sealed class StoreLoginFormModel
{
    [Required]
    public string Login { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
