using WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Factories.AuditLogs;

public class UserAuditLogFactory
    : AuditLogFactoryBase,
    IUserAuditLogFactory
{
    public AuditLog BuildSystemLog(User subject, AuditLogAction action)
    {
        return BuildUserBaseLog(
            actorType: AuditLogActorType.System,
            action: action,
            subject: subject,
            details: new { Username = subject.Username, Role = subject.Role.ToString() }
        );
    }

    public AuditLog BuildAdminLog(User actor, User subject, AuditLogAction action)
    {
        return BuildUserBaseLog(
            actorId: actor.Id,
            actorName: actor.Username,
            actorType: AuditLogActorType.Admin,
            action: action,
            subject: subject,
            details: new { Username = subject.Username, Role = subject.Role.ToString() }
        );
    }

    public AuditLog BuildUserLog(User actor, AuditLogAction action)
    {
        return BuildUserBaseLog(
            actorId: actor.Id,
            actorName: actor.Username,
            actorType: AuditLogActorType.User,
            action: action,
            subject: actor,
            details: new { Username = actor.Username, Role = actor.Role.ToString() }
        );
    }

    private AuditLog BuildUserBaseLog(
        AuditLogAction action,
        User subject,
        object details,
        AuditLogActorType actorType,
        int? actorId = null,
        string? actorName = null)
    {
        if (subject.Id is null)
            throw new InvalidOperationException("User.Id is not assigned");

        return Build(
            actorId: actorId,
            actorName: actorName,
            actorType: actorType,
            action: action,
            subjectId: subject.Id.Value,
            subjectType: AuditLogSubjectType.User,
            details: details
        );
    }
}
