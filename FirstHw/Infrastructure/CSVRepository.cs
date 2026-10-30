using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using System.Globalization;
using System.IO;    

namespace Infrastructure
{
    public class CSVRepository : IRepository
    {
        private static char separator = ',';

        public List<OrderedItem> GetAllOrders()
        {
            var appDirectory = AppContext.BaseDirectory;
            var filePath = Path.Combine(appDirectory, "orders_log.csv");

            var lines = File.ReadAllLines(filePath);
            var allItems = new List<OrderedItem>();

            for (var ind = 1; ind < lines.Length; ind++) // пропускаем заголовок
            {
                var line = lines[ind];

                var data = line.Split(separator);

                if (data.Length > 0)
                {
                    var orderedItem = ParseLineData(data, ind);
                    allItems.Add(orderedItem);
                }
            }

            return allItems;
            
        }

        private static OrderedItem ParseLineData(string[] data, int stringIndexInFile)
        {
            long clientId;
            long orderId;
            string itemName;
            int itemQuantity;
            float itemPrice;

            if (data.Length != 5 ||
                !long.TryParse(data[0], out clientId) ||
                !long.TryParse(data[1], out orderId) ||
                !int.TryParse(data[3], out itemQuantity) ||
                !float.TryParse(data[4], CultureInfo.InvariantCulture, out itemPrice))
            {
                throw new BadRepositoryException(
                    $"Ошибка сохранения данных в репозитории в строке - {stringIndexInFile}");
            }
            else
            {
                itemName = data[2];
                return new OrderedItem(clientId, orderId, itemName, itemQuantity, itemPrice);
            }

        }
    }
}
