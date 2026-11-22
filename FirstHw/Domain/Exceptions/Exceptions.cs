namespace Domain.Exceptions
{
    public class ClientNotFoundException : Exception
    {
        public List<long> ClientIds { get; }

        public ClientNotFoundException(List<long> clientIds)
            : base($"Клиенты с id [{string.Join(", ", clientIds)}] не найдены.")
        {
            ClientIds = clientIds;
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