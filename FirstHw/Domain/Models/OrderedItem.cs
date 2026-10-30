using System.Diagnostics.CodeAnalysis;

namespace Domain.Models
{
    public record OrderedItem
    {
        public long ClientId { get; init; }
        public long OrderId { get; init; }
        public string Name { get; init; }
        public int Quantity { get; init; }
        public float Price { get; init; }

        public OrderedItem(long clientId, long orderId, string name, int quantity, float price)
        {
            ClientId = clientId;
            OrderId = orderId;
            Name = name;
            Quantity = quantity;
            Price = price;
        }

    }

}
