using Domain;
using Domain.Interfaces;
using Domain.Models;
using Domain.Exceptions;
namespace Application
{
    public class Report
    {
        private readonly IRepository _repo;

        public Report(IRepository repo)
        {
            _repo = repo;
        }

        public List<ClientReport> GetReport(List<long> clientsIds)
        {
            var allOrders = _repo.GetAllOrders();
            //сопоставляем id клиента к результирующему отзыву о нем
            var clientToReport = new Dictionary<long, ClientReport>();
            // id клиента - все его айтемы + для каждого айтема его количество
            var clientToItems = new Dictionary<long, Dictionary<string, int>>();

            foreach (var clientId in clientsIds)
            {
                if (!clientToReport.ContainsKey(clientId))
                {
                    clientToReport.Add(clientId, new ClientReport(clientId));
                    clientToItems.Add(clientId, new Dictionary<string, int>());
                }
            }

            foreach (var orderedItem in allOrders)
            {
                if (!clientToReport.ContainsKey(orderedItem.ClientId))
                    continue;

                clientToReport[orderedItem.ClientId].OrdersSum += orderedItem.Price * orderedItem.Quantity;

                if (!clientToItems[orderedItem.ClientId].ContainsKey(orderedItem.Name))
                {
                    clientToItems[orderedItem.ClientId][orderedItem.Name] = orderedItem.Quantity;
                }
                else
                {
                    clientToItems[orderedItem.ClientId][orderedItem.Name] += orderedItem.Quantity;
                }

            }

            var notFoundIds = new List<long>(); // лист для ненайденных в базе id, на случай если пользователь ввел несколько несуществующих

            foreach (var client in clientToItems)
            {
                if (client.Value.Count == 0)
                    notFoundIds.Add(client.Key);

                if (notFoundIds.Count == 0)
                {
                    var favoriteItem = clientToItems[client.Key].MaxBy(item => item.Value);
                    clientToReport[client.Key].FavoriteItem = favoriteItem.Key;
                }
            }
            if (notFoundIds.Count != 0)
                throw new ClientNotFoundException(notFoundIds);

            var res = new List<ClientReport>();

            foreach (var clientId in clientsIds)
                res.Add(clientToReport[clientId]);

            return res;
        }
    }
}
