using WebShopABMATIC.Application.Ports;

namespace WebShopABMATIC.Web.Endpoints;

public static class MollieWebhookEndpoint
{
    public static RouteGroupBuilder MapMollieWebhook(this WebApplication app)
    {
        var group = app.MapGroup("/api/webhooks/mollie");

        group.MapPost("/payments", async (HttpContext context, IMollieWebhookPort webhookPort) =>
        {
            var form = await context.Request.ReadFormAsync();
            var paymentId = form["id"].ToString();
            if (string.IsNullOrWhiteSpace(paymentId))
            {
                return Results.BadRequest();
            }

            await webhookPort.ProcessPaymentAsync(paymentId, context.RequestAborted);
            return Results.Ok();
        }).DisableAntiforgery();

        return group;
    }
}
