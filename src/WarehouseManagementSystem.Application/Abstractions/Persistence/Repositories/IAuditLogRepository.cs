using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;

namespace WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog);

    Task<int> GetAuditLogsCountAsync();

    Task<AuditLog> GetByPublicIdAsync(Guid publicId);

    Task<int> GetAuditLogsCountAsync(AuditLogSearchQuery query);

    Task<List<AuditLog>> GetAuditLogsAsync(AuditLogSearchQuery query);

}
