namespace WarehouseManagementSystem.Domain.Exceptions;

public class DomainException : Exception
{
    public string UserMessage { get; }

    public DomainException(string? userMessage, Exception? innerException = null)
        : base(userMessage ?? "Application exception", innerException)
    {
        UserMessage = userMessage ?? "Unexpected error";
    }
}
