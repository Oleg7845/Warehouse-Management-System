using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;

public interface ISupportTicketFactory
{
    SupportTicket CreateSupportTicket(
        User user,
        string title,
        string? description,
        TicketPriority priority);
}
