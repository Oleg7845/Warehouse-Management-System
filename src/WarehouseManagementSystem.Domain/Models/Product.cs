namespace WarehouseManagementSystem.Domain.Models;

public class Product
{
    public string Name { get; private set; }
    public string SKU { get; init; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int QuantityInStock { get; private set; }
    public int CategoryId { get; private set; }
    public int SupplierId { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public byte[]? RowVersion { get; private set; }

    public Product(
        string name,
        string sku,
        string? description,
        decimal price,
        int quantityInStock,
        int categoryId,
        int supplierId,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        byte[]? powVersion)
    {
        Name = name;
        SKU = sku;
        Description = description;
        Price = price;
        QuantityInStock = quantityInStock;
        CategoryId = categoryId;
        SupplierId = supplierId;
        IsDeleted = isDeleted;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        RowVersion = powVersion;
    }
}
