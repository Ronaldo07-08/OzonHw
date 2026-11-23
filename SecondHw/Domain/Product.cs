namespace Domain;

public record Product
{
    public long Id { get; init; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public double Weight { get; private set; }
    public ProductType Type { get; private set; }
    public DateTime CreatedAt { get; init; }
    public int WarehouseNum { get; private set; }

    private Product()
    {
    }

    public Product(
        long id,
        string name,
        decimal price,
        double weight,
        ProductType type,
        DateTime createdAt,
        int warehouseNum)
    {
        Id = id;
        Name = name;
        Price = price;
        Weight = weight;
        Type = type;
        CreatedAt = createdAt;
        WarehouseNum = warehouseNum;
    }

    public void Update(string name, decimal price, double weight, ProductType type, int warehouseNum)
    {
        Name = name;
        Price = price;
        Weight = weight;
        Type = type;
        WarehouseNum = warehouseNum;
    }
}