namespace WarehouseManagementSystem.Domain.Exceptions;

public class UserAlreadyExistsException : DomainException
{
    public UserAlreadyExistsException(string username)
        : base(userMessage: $"User with username '{username}' already exists") { }
}
