namespace WebShopABMATIC.Application.Audit;

public static class AuditActions
{
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Login = "Login";
    public const string LoginFailed = "LoginFailed";
    public const string Logout = "Logout";
    public const string ReportExport = "ReportExport";
    public const string CheckoutStarted = "CheckoutStarted";
    public const string PaymentPaid = "PaymentPaid";
    public const string PaymentExpired = "PaymentExpired";
    public const string OrderCancelled = "OrderCancelled";
    public const string PasswordReset = "PasswordReset";
    public const string StockAdjust = "StockAdjust";

    public static readonly IReadOnlyList<string> All =
    [
        Create, Update, Delete, Login, LoginFailed, Logout, ReportExport,
        CheckoutStarted, PaymentPaid, PaymentExpired, OrderCancelled, PasswordReset, StockAdjust
    ];
}

public static class AuditSeverity
{
    public const string Information = "Information";
    public const string Warning = "Warning";
    public const string Error = "Error";
    public const string Critical = "Critical";

    public static readonly IReadOnlyList<string> All =
    [
        Information, Warning, Error, Critical
    ];
}
