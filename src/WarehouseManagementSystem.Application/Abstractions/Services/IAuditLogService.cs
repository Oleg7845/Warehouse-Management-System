using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;

namespace WarehouseManagementSystem.Application.Abstractions.Services;

public interface IAuditLogService
{
    Task WriteAsync(AuditLog auditLog);

    Task<int> GetAuditLogsCountAsync();

    Task<AuditLog> GetByPublicIdAsync(Guid publicId);

    Task<int> GetAuditLogsCountAsync(AuditLogSearchQuery query);

    Task<List<AuditLog>> GetAuditLogsAsync(AuditLogSearchQuery query);
}
