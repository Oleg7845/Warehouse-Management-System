namespace WarehouseManagementSystem.Domain.Exceptions;

public class AuditLogNotFoundException : DomainException
{
    public AuditLogNotFoundException(Guid publicId)
        : base(userMessage: $"Audit log with public ID '{publicId.ToString()}' was not found") { }
}
