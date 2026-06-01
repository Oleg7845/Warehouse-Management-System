using WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Factories.SupportTickets;

public class SupportTicketFactory : ISupportTicketFactory
{
    public SupportTicket CreateSupportTicket(
        User user,
        string title,
        string? description,
        TicketPriority priority)
    {
        return new SupportTicket(
        id: null,
        publicId: Guid.NewGuid(),
        createdByUserId: user.Id!.Value,
        createdByUsername: user.Username,
        title: title,
        description: description,
        status: TicketStatus.Open,
        priority: priority,
        createdAt: DateTimeOffset.UtcNow,
        updatedAt: null,
        closedAt: null,
        rowVersion: null);
    }
}
