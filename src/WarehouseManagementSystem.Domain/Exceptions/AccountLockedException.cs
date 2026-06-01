namespace WarehouseManagementSystem.Domain.Exceptions;

public class AccountLockedException : DomainException
{
    public AccountLockedException()
        : base(userMessage: "Your account has been locked. Contact the Support") { }
}
