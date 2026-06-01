using System.Text.Json;
using WarehouseManagementSystem.Application.Abstractions.Factories;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Factories;

public abstract class AuditLogFactoryBase
{
    protected AuditLog Build(
        int? actorId,
        string? actorName,
        AuditLogActorType actorType,
        AuditLogAction action,
        int subjectId,
        AuditLogSubjectType subjectType,
        object details)
    {
        return new AuditLog(
            publicId: Guid.NewGuid(),
            actorId: actorId,
            actorName: actorName,
            actorType: actorType,
            action: action,
            subjectId: subjectId,
            subjectType: subjectType,
            detailsJson: JsonSerializer.Serialize(details),
            createdAt: DateTimeOffset.UtcNow
        );
    }
}
