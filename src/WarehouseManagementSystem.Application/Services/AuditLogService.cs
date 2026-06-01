using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;

namespace WarehouseManagementSystem.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditLogService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task WriteAsync(AuditLog auditLog)
    {
        await _auditLogRepository.AddAsync(auditLog);
    }

    public async Task<int> GetAuditLogsCountAsync()
    {
        return await _auditLogRepository.GetAuditLogsCountAsync();
    }

    public async Task<AuditLog> GetByPublicIdAsync(Guid publicId)
    {
        return await _auditLogRepository.GetByPublicIdAsync(publicId);
    }

    public async Task<int> GetAuditLogsCountAsync(AuditLogSearchQuery query)
    {
        return await _auditLogRepository.GetAuditLogsCountAsync(query);
    }

    public async Task<List<AuditLog>> GetAuditLogsAsync(AuditLogSearchQuery query)
    {
        return await _auditLogRepository.GetAuditLogsAsync(query);
    }
}
