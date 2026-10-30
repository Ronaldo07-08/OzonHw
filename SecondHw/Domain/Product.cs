namespace Domain
{
    public record Product
    {
        public long Id { get; init; }
        public string Name { get; init; }

        public decimal Price { get; private set; }

        public double Weight { get; private set; }

        public ProductType Type { get; init; }

        public DateTime CreatedAt { get; init; }

        public int WarehouseNum { get; private set; }


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
    }
}
