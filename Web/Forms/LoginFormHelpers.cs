namespace WebShopABMATIC.Web.Forms;

public static class LoginFormHelpers
{
    public static string ResolveAdminReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return "/admin";
        }

        if (returnUrl.StartsWith('/') && !returnUrl.StartsWith("//", StringComparison.Ordinal))
        {
            return returnUrl;
        }

        return "/admin";
    }

    public static string ResolveStoreReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return "/";
        }

        if (returnUrl.StartsWith('/') && !returnUrl.StartsWith("//", StringComparison.Ordinal))
        {
            return returnUrl;
        }

        return "/";
    }
}
