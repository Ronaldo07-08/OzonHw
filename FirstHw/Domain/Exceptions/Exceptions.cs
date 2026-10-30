using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class ClientNotFoundException : Exception
    {

        public List<long> ClientsIds { get; init; }

        public ClientNotFoundException(List<long> notFoundClients) : base($"Клиентов в с id {string.Join(", ", notFoundClients)} нет в базе")
        {
            ClientsIds = notFoundClients;
        }
    }

    public class WrongInputException : Exception
    {
        public WrongInputException(string message) : base(message)
        {
        }
    }

    public class BadRepositoryException : Exception
    {
        public BadRepositoryException(string message) : base(message)
        {
        }
    }
}
