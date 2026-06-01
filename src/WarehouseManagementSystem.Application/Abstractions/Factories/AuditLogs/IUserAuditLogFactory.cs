using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;

public interface IUserAuditLogFactory
{
    AuditLog BuildSystemLog(User subject, AuditLogAction action);

    AuditLog BuildAdminLog(User actor, User subject, AuditLogAction action);

    AuditLog BuildUserLog(User actor, AuditLogAction action);
}
