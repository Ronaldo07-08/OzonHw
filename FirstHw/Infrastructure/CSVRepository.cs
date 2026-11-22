using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using System.Globalization;

namespace Infrastructure
{
    public class CSVRepository : IRepository
    {
        private const string FileName = "orders_log.csv";
        private const char Separator = ',';
        private const int ColumnsCount = 5;
        private const int FirstDataLineIndex = 1;

        public List<OrderedItem> GetAllOrders()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, FileName);
            var lines = File.ReadAllLines(filePath);
            var orders = new List<OrderedItem>();

            for (var lineIndex = FirstDataLineIndex; lineIndex < lines.Length; lineIndex++)
            {
                var columns = lines[lineIndex].Split(Separator);
                orders.Add(ParseOrder(columns, lineIndex + 1));
            }

            return orders;
        }

        private static OrderedItem ParseOrder(string[] columns, int lineNumber)
        {
            if (columns.Length != ColumnsCount ||
                !long.TryParse(columns[0], out var clientId) ||
                !long.TryParse(columns[1], out var orderId) ||
                !int.TryParse(columns[3], out var quantity) ||
                !decimal.TryParse(columns[4], CultureInfo.InvariantCulture, out var price))
            {
                throw new BadRepositoryException($"Ошибка чтения данных из файла в строке {lineNumber}.");
            }

            return new OrderedItem(clientId, orderId, columns[2], quantity, price);
        }
    }
}