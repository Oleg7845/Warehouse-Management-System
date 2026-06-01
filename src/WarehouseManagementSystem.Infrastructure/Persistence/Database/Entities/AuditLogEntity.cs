using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Database.Entities;

public class AuditLogEntity
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public int? ActorId { get; set; }
    public string? ActorName { get; set; }
    public AuditLogActorType ActorType { get; set; }
    public AuditLogAction Action { get; set; }
    public int SubjectId { get; set; }
    public AuditLogSubjectType SubjectType { get; set; }
    public string DetailsJson { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}
