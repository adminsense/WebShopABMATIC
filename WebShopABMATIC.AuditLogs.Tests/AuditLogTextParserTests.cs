using WebShopABMATIC.Application.Audit;

namespace WebShopABMATIC.AuditLogs.Tests;

public sealed class AuditLogTextParserTests
{
    [Theory]
    [InlineData("Create Product id=42", "Product", AuditActions.Create)]
    [InlineData("Update Customer id=7", "Customer", AuditActions.Update)]
    [InlineData("Delete StaffUser id=3", "StaffUser", AuditActions.Delete)]
    [InlineData("Login StaffUser", "StaffUser", AuditActions.Login)]
    [InlineData("Logout Customer", "Customer", AuditActions.Logout)]
    [InlineData("Failed LoginFailed Customer: bad password", "Customer", AuditActions.LoginFailed)]
    [InlineData("CheckoutStarted Order id=99", "Order", AuditActions.CheckoutStarted)]
    [InlineData("PaymentPaid Order id=99", "Order", AuditActions.PaymentPaid)]
    [InlineData("ReportExport customers", "customers", AuditActions.ReportExport)]
    public void ParseAction_reads_known_prefix(string exception, string className, string expected) =>
        AuditLogTextParser.ParseAction(exception, className).Should().Be(expected);

    [Fact]
    public void ParseAction_strips_Failed_prefix() =>
        AuditLogTextParser.ParseAction("Failed Update Product id=1", "Product")
            .Should().Be(AuditActions.Update);

    [Fact]
    public void ParseAction_falls_back_to_className_when_action_token() =>
        AuditLogTextParser.ParseAction("something else", AuditActions.StockAdjust)
            .Should().Be(AuditActions.StockAdjust);

    [Fact]
    public void ParseAction_returns_Unknown_when_empty() =>
        AuditLogTextParser.ParseAction(null, null).Should().Be("Unknown");

    [Fact]
    public void ParseAction_prefers_longer_token_LoginFailed_over_Login() =>
        AuditLogTextParser.ParseAction("LoginFailed Customer id=1", "Customer")
            .Should().Be(AuditActions.LoginFailed);

    [Theory]
    [InlineData("Create Product id=42", "42")]
    [InlineData("Update Customer id=11742", "11742")]
    [InlineData("Login StaffUser", null)]
    [InlineData("", null)]
    [InlineData(null, null)]
    public void ParseEntityId_reads_id_marker(string? exception, string? expected) =>
        AuditLogTextParser.ParseEntityId(exception).Should().Be(expected);

    [Theory]
    [InlineData("Create Product id=1", true)]
    [InlineData("Failed Login Customer: x", false)]
    [InlineData("LoginFailed Customer", false)]
    [InlineData(null, true)]
    public void IsSuccess_detects_failed_word(string? exception, bool expected) =>
        AuditLogTextParser.IsSuccess(exception).Should().Be(expected);
}
