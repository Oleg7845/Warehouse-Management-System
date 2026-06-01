namespace WarehouseManagementSystem.Domain.Exceptions;

public class DatabaseConnectionException : DomainException
{
    public DatabaseConnectionException(string? message = null, Exception? innerException = null)
        : base(userMessage: "Fail to connect to database" ?? message, innerException) { }
}
