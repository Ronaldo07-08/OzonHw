using Domain.Models;
using System.Globalization;
using System.Text;

namespace Presentation.Output
{
    public static class OutputFormatter
    {
        public static string ReportFormat(ClientReport client)
        {
            return $"client_id - {client.ClientId};" +
                    $" orders_sum - {client.OrdersSum.ToString("F2", CultureInfo.InvariantCulture)};" +
                    $" favorite_item_name - {client.FavoriteItem}";
        }
    }
}
