using Microsoft.AspNetCore.Components.Web;

namespace WebShopABMATIC.Web.Components;

public static class AppRenderModes
{
    // Prerender so the store HTML (nav, catalog links) works before the circuit connects.
    // Interactive handlers attach after SignalR is up.
    public static InteractiveServerRenderMode InteractiveServer { get; } = new(prerender: true);
}
