using WarehouseManagementSystem.Application.Abstractions.Factories.AuditLogs;
using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;

namespace WarehouseManagementSystem.Application.Services;

public class SupportTicketService : ISupportTicketService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSessionService _sessionService;
    private readonly ISupportTicketFactory _supportTicketFactory;
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly ISupportTicketAuditLogFactory _supportTicketAuditLogFactory;
    private readonly IAuditLogService _auditLogService;

    public SupportTicketService(
        IUserRepository userRepository,
        IUserSessionService sessionService,
        ISupportTicketFactory supportTicketFactory,
        ISupportTicketRepository supportTicketRepository,
        ISupportTicketAuditLogFactory supportTicketAuditLogFactory,
        IAuditLogService auditLogService)
    {
        _userRepository = userRepository;
        _sessionService = sessionService;
        _supportTicketFactory = supportTicketFactory;
        _supportTicketRepository = supportTicketRepository;
        _supportTicketAuditLogFactory = supportTicketAuditLogFactory;
        _auditLogService = auditLogService;
    }

    public async Task<SupportTicket> CreateSupportTicket(string? username, string title, string? description, TicketPriority priority)
    {
        User? user = null;

        if (!string.IsNullOrEmpty(username))
        {
            user = await _userRepository.GetByUsernameAsync(username);
        }
        else
        {
            user = _sessionService.CurrentUser!;
        }

        SupportTicket newTicket = _supportTicketFactory.CreateSupportTicket(
            user: user,
            title: title,
            description: null,
            priority: priority);

        if (description != null)
        {
            newTicket.AppendComment(
                author: user.Username,
                text: description);
        }

        SupportTicket newSupportTicket = await _supportTicketRepository.AddAsync(newTicket);

        await WriteUserLog(user, newSupportTicket, AuditLogAction.SupportTicketOpened);

        return newTicket;
    }

    public async Task<int> GetCountAsync()
    {
        return await _supportTicketRepository.GetCountAsync();
    }

    public async Task<SupportTicket> GetByPublicIdAsync(Guid publicId)
    {
        return await _supportTicketRepository.GetByPublicIdAsync(publicId);
    }

    public async Task<List<SupportTicket>> GetSupportTicketsAsync(SupportTicketSearchQuery query)
    {
        return await _supportTicketRepository.GetUsersAsync(query);
    }

    public async Task<int> GetSupportTicketsCountAsync(SupportTicketSearchQuery query)
    {
        return await _supportTicketRepository.GetUsersCountAsync(query);
    }

    public async Task UpdateSupportTicketDescription(Guid publicId, string newDescription)
    {
        SupportTicket supportTicket = await GetByPublicIdAsync(publicId);

        supportTicket.UpdateDescription(newDescription);

        await _supportTicketRepository.UpdateAsync(supportTicket);

        if (_sessionService.CurrentUser!.Role == UserRole.User)
        {
            await WriteUserLog(_sessionService.CurrentUser!, supportTicket, AuditLogAction.SupportTicketOpened);
        }
        else
        {
            await WriteAdminLog(supportTicket, AuditLogAction.SupportTicketOpened);
        }
    }

    public async Task UpdateStatusToInProgress(Guid publicId)
    {
        SupportTicket supportTicket = await GetByPublicIdAsync(publicId);

        supportTicket.UpdateStatusToInProgress();

        await UpdateSupportTicket(supportTicket);
    }

    public async Task UpdateStatusToWaitingForUser(Guid publicId)
    {
        SupportTicket supportTicket = await GetByPublicIdAsync(publicId);

        supportTicket.UpdateStatusToWaitingForUser();

        await UpdateSupportTicket(supportTicket);
    }

    public async Task UpdateStatusToResolved(Guid publicId)
    {
        SupportTicket supportTicket = await GetByPublicIdAsync(publicId);

        supportTicket.UpdateStatusToResolved();

        await UpdateSupportTicket(supportTicket);
    }

    public async Task UpdateStatusToClosed(Guid publicId)
    {
        SupportTicket supportTicket = await GetByPublicIdAsync(publicId);

        supportTicket.UpdateStatusToClosed();

        await UpdateSupportTicket(supportTicket);
    }

    public async Task UpdateSupportTicket(SupportTicket supportTicket)
    {
        await _supportTicketRepository.UpdateAsync(supportTicket);

        if (_sessionService.CurrentUser!.Role == UserRole.User)
        {
            await WriteUserLog(_sessionService.CurrentUser!, supportTicket, AuditLogAction.SupportTicketOpened);
        }
        else
        {
            await WriteAdminLog(supportTicket, AuditLogAction.SupportTicketOpened);
        }
    }

    private async Task WriteUserLog(User actor, SupportTicket subject, AuditLogAction action)
    {
        AuditLog newlog = _supportTicketAuditLogFactory.BuildUserLog(
            actor: actor,
            subject: subject,
            action: action);

        await _auditLogService.WriteAsync(newlog);
    }

    private async Task WriteAdminLog(SupportTicket subject, AuditLogAction action)
    {
        AuditLog newlog = _supportTicketAuditLogFactory.BuildAdminLog(
            _sessionService.CurrentUser!,
            subject: subject,
            action: action);

        await _auditLogService.WriteAsync(newlog);
    }
}
