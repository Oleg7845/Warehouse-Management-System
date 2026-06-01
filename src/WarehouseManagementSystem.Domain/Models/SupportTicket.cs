using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Domain.Models;

public class SupportTicket
{
    public int? Id { get; init; }
    public Guid PublicId { get; init; }
    public int CreatedByUserId { get; init; }
    public string CreatedByUsername { get; init; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public TicketStatus Status { get; private set; }
    public TicketPriority Priority { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    public byte[]? RowVersion { get; init; }

    public SupportTicket(
        int? id,
        Guid publicId,
        int createdByUserId,
        string createdByUsername,
        string title,
        string? description,
        TicketStatus status,
        TicketPriority priority,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt,
        DateTimeOffset? closedAt,
        byte[]? rowVersion)
    {
        Id = id;
        PublicId = publicId;
        CreatedByUserId = createdByUserId;
        CreatedByUsername = createdByUsername;
        Title = title;
        Description = description;
        Status = status;  //Set Open in factory
        Priority = priority;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        ClosedAt = closedAt;
        RowVersion = rowVersion;
    }

    public void UpdateDescription(string description)
    {
        EnsureIsNotClosed();
        Description = description;
    }

    public void AppendComment(string author, string text)
    {
        EnsureIsNotClosed();

        var timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm");

        if (string.IsNullOrEmpty(Description))
        {
            Description += $"--- Description ---\n{text}";
        }
        else
        {
            Description += $"\n\n--- Comment by {author} ({timestamp}) ---\n{text}";
        }
    }

    public void UpdateStatusToInProgress()
    {
        Status = TicketStatus.InProgress;
    }

    public void UpdateStatusToWaitingForUser()
    {
        EnsureIsNotClosed();
        Status = TicketStatus.WaitingForUser;
    }

    public void UpdateStatusToResolved()
    {
        EnsureIsNotClosed();
        Status = TicketStatus.Resolved;
    }

    public void UpdateStatusToClosed()
    {
        EnsureIsNotClosed();
        Status = TicketStatus.Closed;
    }

    private void EnsureIsNotClosed()
    {
        if (Status == TicketStatus.Closed)
            throw new SupportTicketUpdatingException("Cannot change the closed ticket");
    }
}
