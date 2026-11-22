using Domain.Models;

namespace Domain.Interfaces
{
    public interface IRepository
    {
        public List<OrderedItem> GetAllOrders();
    }
}
