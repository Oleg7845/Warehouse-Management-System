using System.Text.Json;
using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Domain.Models;

public class AuditLog
{
    public Guid PublicId { get; init; }
    public int? ActorId { get; init; }
    public string? ActorName { get; init; }
    public AuditLogActorType ActorType { get; init; }
    public AuditLogAction Action { get; init; }
    public int SubjectId { get; init; }
    public AuditLogSubjectType SubjectType { get; init; }
    public string DetailsJson { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    public AuditLog(
        Guid publicId,
        int? actorId,
        string? actorName,
        AuditLogActorType actorType,
        AuditLogAction action,
        int subjectId,
        AuditLogSubjectType subjectType,
        string detailsJson,
        DateTimeOffset createdAt)
    {
        PublicId = publicId;
        ActorId = actorId;
        ActorName = actorName;
        ActorType = actorType;
        Action = action;
        SubjectId = subjectId;
        SubjectType = subjectType;
        DetailsJson = detailsJson;
        CreatedAt = createdAt;
    }

    public T? GetDetails<T>()
    {
        return JsonSerializer.Deserialize<T>(DetailsJson);
    }
}
