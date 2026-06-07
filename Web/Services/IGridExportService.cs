namespace WebShopABMATIC.Web.Services;

public interface IGridExportService
{
    Task ExportAsync(string format, GridExportRequest request, CancellationToken cancellationToken = default);
}
