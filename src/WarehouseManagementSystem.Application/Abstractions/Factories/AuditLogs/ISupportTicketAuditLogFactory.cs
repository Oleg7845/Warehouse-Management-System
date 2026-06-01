using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;

public interface ISupportTicketAuditLogFactory
{
    AuditLog BuildAdminLog(User actor, SupportTicket subject, AuditLogAction action);

    AuditLog BuildUserLog(User actor, SupportTicket subject, AuditLogAction action);
}
