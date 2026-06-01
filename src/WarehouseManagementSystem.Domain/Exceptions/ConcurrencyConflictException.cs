namespace WarehouseManagementSystem.Domain.Exceptions;

public class ConcurrencyConflictException : DomainException
{
    public ConcurrencyConflictException()
        : base(userMessage: "The record was modified by another user") { }
}
