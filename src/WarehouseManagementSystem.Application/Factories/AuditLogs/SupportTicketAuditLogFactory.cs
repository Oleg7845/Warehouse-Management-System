using WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Factories.AuditLogs;

public class SupportTicketAuditLogFactory
    : AuditLogFactoryBase,
    ISupportTicketAuditLogFactory
{

    public AuditLog BuildAdminLog(User actor, SupportTicket subject, AuditLogAction action)
    {
        return BuildSupportTicketBaseLog(
            actor: actor,
            action: action,
            subject: subject,
            actorType: AuditLogActorType.Admin);
    }

    public AuditLog BuildUserLog(User actor, SupportTicket subject, AuditLogAction action)
    {
        return BuildSupportTicketBaseLog(
            actor: actor,
            action: action,
            subject: subject,
            actorType: AuditLogActorType.User);
    }

    private AuditLog BuildSupportTicketBaseLog(
        User actor,
        AuditLogAction action,
        SupportTicket subject,
        AuditLogActorType actorType)
    {
        if (subject.Id is null)
            throw new InvalidOperationException("SupportTicket.Id is not assigned");

        return Build(
            actorId: actor.Id,
            actorName: actor.Username,
            actorType: actorType,
            action: action,
            subjectId: subject.Id.Value,
            subjectType: AuditLogSubjectType.SupportTicket,
            details: new { CreatorRole = actor.Role.ToString(), PublicId = subject.PublicId, Priority = subject.Priority, Status = subject.Status }
        );
    }
}
