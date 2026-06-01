namespace WarehouseManagementSystem.Domain.Exceptions;

public class SupportTicketUpdatingException : DomainException
{
    public SupportTicketUpdatingException(string message)
        : base(userMessage: message) { }
}
