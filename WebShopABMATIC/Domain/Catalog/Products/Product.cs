namespace WebShopABMATIC.Domain.Catalog.Products;

public sealed class Product
{
    public int ProductId { get; private set; }
    public string NameEn { get; private set; } = string.Empty;
    public string? OrderPartNumber { get; private set; }
    public int SupplierId { get; private set; }
    public int ManufacturerId { get; private set; }
    public bool ShowOnWebshop { get; private set; }
    public string? WebshopDescriptionNl { get; private set; }
    public string? EanCode { get; private set; }
    public bool IsInactive { get; private set; }

    public static Product Create(
        string nameEn,
        string? orderPartNumber,
        int supplierId,
        int manufacturerId,
        bool showOnWebshop,
        string? webshopDescriptionNl,
        string? eanCode)
    {
        return new Product
        {
            ProductId = 0,
            NameEn = nameEn,
            OrderPartNumber = orderPartNumber,
            SupplierId = supplierId,
            ManufacturerId = manufacturerId,
            ShowOnWebshop = showOnWebshop,
            WebshopDescriptionNl = webshopDescriptionNl,
            EanCode = eanCode,
            IsInactive = false
        };
    }

    public static Product Rehydrate(
        int productId,
        string nameEn,
        string? orderPartNumber,
        int supplierId,
        int manufacturerId,
        bool showOnWebshop,
        string? webshopDescriptionNl,
        string? eanCode,
        bool isInactive)
    {
        return new Product
        {
            ProductId = productId,
            NameEn = nameEn,
            OrderPartNumber = orderPartNumber,
            SupplierId = supplierId,
            ManufacturerId = manufacturerId,
            ShowOnWebshop = showOnWebshop,
            WebshopDescriptionNl = webshopDescriptionNl,
            EanCode = eanCode,
            IsInactive = isInactive
        };
    }

    public void Update(
        string nameEn,
        string? orderPartNumber,
        int supplierId,
        int manufacturerId,
        bool showOnWebshop,
        string? webshopDescriptionNl,
        string? eanCode)
    {
        NameEn = nameEn;
        OrderPartNumber = orderPartNumber;
        SupplierId = supplierId;
        ManufacturerId = manufacturerId;
        ShowOnWebshop = showOnWebshop;
        WebshopDescriptionNl = webshopDescriptionNl;
        EanCode = eanCode;
    }

    public void Deactivate()
    {
        IsInactive = true;
        ShowOnWebshop = false;
    }
}
