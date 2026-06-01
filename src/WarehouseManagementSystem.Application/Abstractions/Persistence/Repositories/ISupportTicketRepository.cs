using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;

namespace WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;

public interface ISupportTicketRepository
{
    Task<SupportTicket> AddAsync(SupportTicket supportTicket);

    Task<int> GetCountAsync();

    Task<SupportTicket> GetByPublicIdAsync(Guid publicId);

    Task<List<SupportTicket>> GetUsersAsync(SupportTicketSearchQuery query);

    Task<int> GetUsersCountAsync(SupportTicketSearchQuery query);

    Task UpdateAsync(SupportTicket supportTicket);


}
