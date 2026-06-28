using Microsoft.AspNetCore.Components.Web;

namespace WebShopABMATIC.Web.Components;

public static class AppRenderModes
{
    public static InteractiveServerRenderMode InteractiveServer { get; } = new(prerender: false);
}
