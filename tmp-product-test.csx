using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Admin;
using WebShopABMATIC.Infrastructure.Media;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

var config = new ConfigurationBuilder()
    .AddJsonFile("Web/appsettings.Development.json")
    .Build();
var conn = config.GetConnectionString("connWebShopABMATIC");
var opts = new DbContextOptionsBuilder<WebShopABMATICDbContext>()
    .UseSqlServer(conn)
    .Options;
await using var db = new WebShopABMATICDbContext(opts);
var env = new FakeEnv();
var media = new LocalProductMediaService(db, env);
var svc = new ProductAdminService(db, media);
var result = await svc.GetProductsAsync(new WebShopABMATIC.Application.Admin.Products.ProductListFilter());
Console.WriteLine($"Count={result.TotalCount} Items={result.Items.Count}");
foreach (var p in result.Items.Take(3)) Console.WriteLine($"{p.ProductId} {p.NameEn}");

sealed class FakeEnv : IWebHostEnvironment
{
    public string WebRootPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "Web", "wwwroot");
    public string EnvironmentName { get; set; } = "Development";
    public string ApplicationName { get; set; } = "Test";
    public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
    public IFileProvider ContentRootFileProvider { get; set; } = null!;
    public IFileProvider WebRootFileProvider { get; set; } = null!;
}
