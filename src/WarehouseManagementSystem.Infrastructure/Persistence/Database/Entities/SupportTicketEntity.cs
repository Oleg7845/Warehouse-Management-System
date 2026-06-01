using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Database.Entities;

public class SupportTicketEntity
{
    public int? Id { get; set; }
    public Guid PublicId { get; set; }
    public int CreatedByUserId { get; set; }
    public string CreatedByUsername { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }
    public byte[] RowVersion { get; set; } = null!;
}
