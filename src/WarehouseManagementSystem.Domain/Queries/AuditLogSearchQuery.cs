using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Domain.Queries;

public sealed class AuditLogSearchQuery
{
    public string? Search { get; init; }  // PublicId | ActorName

    public AuditLogActorType? ActorType { get; init; }

    public AuditLogAction? Action { get; init; }

    public AuditLogSubjectType? SubjectType { get; init; }

    public int Offset { get; init; } = 0;

    public int Limit { get; init; } = 50;
}
