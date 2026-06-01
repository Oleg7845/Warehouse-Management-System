using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Domain.Queries;

public sealed class SupportTicketSearchQuery
{
    public string? CreatedByUsername { get; init; }

    public string? Search { get; init; }  // PublicId | CreatedByUsername | Title | Description

    public TicketStatus? Status { get; init; }

    public TicketPriority? Priority { get; init; }

    public int Offset { get; init; } = 0;

    public int Limit { get; init; } = 50;
}
