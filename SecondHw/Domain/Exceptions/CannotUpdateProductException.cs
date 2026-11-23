namespace Domain.Exceptions;

public class CannotUpdateProductException : Exception
{
    public CannotUpdateProductException(string message) : base(message)
    {
    }
}