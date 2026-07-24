using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Tests.Unit.Admin;

internal static class AdminTestHelpers
{
    public static PagedResult<T> EmptyPage<T>() => new()
    {
        Items = [],
        TotalCount = 0,
        Page = 1,
        PageSize = 20
    };
}
