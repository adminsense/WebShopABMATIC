using System.ComponentModel.DataAnnotations;

namespace WebShopABMATIC.Web.Forms;

public sealed class IdentityRegisterFormModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = "";
}

public sealed class IdentityForgotPasswordFormModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";
}

public sealed class IdentityResetPasswordFormModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = "";

    [Required]
    public string Code { get; set; } = "";
}

public sealed class IdentityResendEmailConfirmationFormModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";
}

public sealed class IdentityExternalLoginConfirmationFormModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";
}

public sealed class IdentityLoginWith2FaFormModel
{
    [Required, StringLength(7, MinimumLength = 6)]
    public string? TwoFactorCode { get; set; }

    public bool RememberMachine { get; set; }
}

public sealed class IdentityLoginWithRecoveryCodeFormModel
{
    [Required]
    public string RecoveryCode { get; set; } = "";
}

public sealed class IdentityManageProfileFormModel
{
    [Phone]
    public string? PhoneNumber { get; set; }
}

public sealed class IdentityManageChangePasswordFormModel
{
    [Required, DataType(DataType.Password)]
    public string OldPassword { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = "";

    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = "";
}

public sealed class IdentityManageSetPasswordFormModel
{
    [Required, StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    public string? ConfirmPassword { get; set; }
}

public sealed class IdentityManageEmailFormModel
{
    [Required, EmailAddress]
    public string? NewEmail { get; set; }
}

public sealed class IdentityManageDeletePersonalDataFormModel
{
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";
}

public sealed class IdentityManageEnableAuthenticatorFormModel
{
    [Required, StringLength(7, MinimumLength = 6)]
    public string Code { get; set; } = "";
}
