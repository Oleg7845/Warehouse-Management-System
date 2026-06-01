namespace WarehouseManagementSystem.Domain.Exceptions;

public class SupportTicketNotFoundException : DomainException
{
    public SupportTicketNotFoundException(Guid publicId)
        : base(userMessage: $"Support ticket with ID '{publicId}' was not found") { }
}
