using Domain.Exceptions;

namespace Presentation.Input
{
    public static class Parser
    {
        public static List<long> ParseClientIds(string[] arguments)
        {
            var clientIds = new List<long>();

            foreach (var argument in arguments)
            {
                if (!long.TryParse(argument, out var clientId))
                {
                    throw new WrongInputException($"Некорректное значение ClientId - {argument}");
                }

                clientIds.Add(clientId);
            }

            return clientIds;
        }
    }
}