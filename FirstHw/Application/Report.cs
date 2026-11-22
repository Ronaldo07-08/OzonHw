using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace Application
{
    public class Report
    {
        private readonly IRepository _repository;

        public Report(IRepository repository)
        {
            _repository = repository;
        }

        public List<ClientReport> GetReport(List<long> clientIds)
        {
            var allOrders = _repository.GetAllOrders();
            var clientToOrdersSum = new Dictionary<long, decimal>();
            // Для каждого клиента: название товара и его суммарное количество.
            var clientToItems = new Dictionary<long, Dictionary<string, int>>();
            var clientToReport = new Dictionary<long, ClientReport>();

            foreach (var clientId in clientIds)
            {
                if (!clientToOrdersSum.ContainsKey(clientId))
                {
                    clientToOrdersSum.Add(clientId, 0);
                    clientToItems.Add(clientId, new Dictionary<string, int>());
                }
            }

            foreach (var orderedItem in allOrders)
            {
                if (!clientToOrdersSum.ContainsKey(orderedItem.ClientId))
                {
                    continue;
                }

                clientToOrdersSum[orderedItem.ClientId] += orderedItem.Price * orderedItem.Quantity;

                if (!clientToItems[orderedItem.ClientId].ContainsKey(orderedItem.Name))
                {
                    clientToItems[orderedItem.ClientId][orderedItem.Name] = orderedItem.Quantity;
                }
                else
                {
                    clientToItems[orderedItem.ClientId][orderedItem.Name] += orderedItem.Quantity;
                }
            }

            var notFoundIds = new List<long>();

            foreach (var client in clientToItems)
            {
                if (client.Value.Count == 0)
                {
                    notFoundIds.Add(client.Key);
                }

                if (notFoundIds.Count == 0)
                {
                    var favoriteItem = client.Value.MaxBy(item => item.Value);
                    clientToReport[client.Key] = new ClientReport(
                        client.Key,
                        clientToOrdersSum[client.Key],
                        favoriteItem.Key);
                }
            }

            if (notFoundIds.Count != 0)
            {
                throw new ClientNotFoundException(notFoundIds);
            }

            var reports = new List<ClientReport>();

            foreach (var clientId in clientIds)
            {
                reports.Add(clientToReport[clientId]);
            }

            return reports;
        }
    }
}