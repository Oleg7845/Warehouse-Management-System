using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;

namespace WarehouseManagementSystem.Application.Abstractions.Services;

public interface ISupportTicketService
{
    Task<SupportTicket> CreateSupportTicket(string? Username, string title, string? description, TicketPriority priority);

    Task<int> GetCountAsync();

    Task<SupportTicket> GetByPublicIdAsync(Guid publicId);

    Task<List<SupportTicket>> GetSupportTicketsAsync(SupportTicketSearchQuery query);

    Task<int> GetSupportTicketsCountAsync(SupportTicketSearchQuery query);

    Task UpdateSupportTicketDescription(Guid publicId, string newDescription);

    Task UpdateStatusToInProgress(Guid publicId);

    Task UpdateStatusToWaitingForUser(Guid publicId);

    Task UpdateStatusToResolved(Guid publicId);

    Task UpdateStatusToClosed(Guid publicId);

    Task UpdateSupportTicket(SupportTicket supportTicket);
}
