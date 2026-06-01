namespace WarehouseManagementSystem.Domain.Exceptions;

public class InvalidPasswordException : DomainException
{
    public InvalidPasswordException()
        : base(userMessage: "The password provided is incorrect") { }
}
