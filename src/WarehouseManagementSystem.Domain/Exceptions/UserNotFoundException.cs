namespace WarehouseManagementSystem.Domain.Exceptions;

public class UserNotFoundException : DomainException
{
    public UserNotFoundException(string username)
        : base(userMessage: $"User with username '{username}' was not found") { }
}
