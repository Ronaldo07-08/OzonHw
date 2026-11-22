namespace Domain.Models
{
    public record OrderedItem(long ClientId, long OrderId, string Name, int Quantity, decimal Price);
}
