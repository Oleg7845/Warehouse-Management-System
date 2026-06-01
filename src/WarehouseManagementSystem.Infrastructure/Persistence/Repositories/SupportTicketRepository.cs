using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.Infrastructure.Persistence.Abstractions;
using WarehouseManagementSystem.Infrastructure.Persistence.Database;
using WarehouseManagementSystem.Infrastructure.Persistence.Database.Entities;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Repositories;

public class SupportTicketRepository : ISupportTicketRepository
{
    private readonly IDbContextFactory _dbContextFactory;

    public SupportTicketRepository(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<SupportTicket> AddAsync(SupportTicket supportTicket)
    {
        using var db = _dbContextFactory.CreateDbContext();

        SupportTicketEntity entity = new SupportTicketEntity
        {
            Id = supportTicket.Id,
            PublicId = supportTicket.PublicId,
            CreatedByUserId = supportTicket.CreatedByUserId,
            CreatedByUsername = supportTicket.CreatedByUsername,
            Title = supportTicket.Title,
            Description = supportTicket.Description,
            Status = supportTicket.Status,
            Priority = supportTicket.Priority,
            CreatedAt = supportTicket.CreatedAt,
            UpdatedAt = supportTicket.UpdatedAt,
            ClosedAt = supportTicket.ClosedAt
        };

        db.SupportTickets.Add(entity);
        await db.SaveChangesAsync();

        return ToDomain(entity);
    }
    
    public async Task<int> GetCountAsync()
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.SupportTickets.CountAsync();
    }

    public async Task<SupportTicket> GetByPublicIdAsync(Guid publicId)
    {
        using var db = _dbContextFactory.CreateDbContext();

        SupportTicketEntity? entity = await db.SupportTickets.FirstOrDefaultAsync(x => x.PublicId == publicId);

        if (entity == null)
            throw new SupportTicketNotFoundException(publicId);

        return ToDomain(entity);
    }

    public async Task<List<SupportTicket>> GetUsersAsync(SupportTicketSearchQuery query)
    {
        using var db = _dbContextFactory.CreateDbContext();

        IQueryable<SupportTicketEntity> supportTicketQuery = BuildUserSQuery(db, query);

        var entities = await supportTicketQuery
            .OrderBy(x => x.Id)
            .Skip(query.Offset)
            .Take(query.Limit)
            .ToListAsync();

        return entities.Select(ToDomain).ToList();
    }

    public async Task<int> GetUsersCountAsync(SupportTicketSearchQuery query)
    {
        using var db = _dbContextFactory.CreateDbContext();

        IQueryable<SupportTicketEntity> supportTicketQuery = BuildUserSQuery(db, query);

        return await supportTicketQuery.CountAsync();
    }

    public async Task UpdateAsync(SupportTicket supportTicket)
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();

            SupportTicketEntity? entity = await db.SupportTickets.FirstOrDefaultAsync(x => x.PublicId == supportTicket.PublicId);

            if (entity == null)
                throw new SupportTicketNotFoundException(supportTicket.PublicId);

            entity.Description = supportTicket.Description;
            entity.Status = supportTicket.Status;
            entity.UpdatedAt = supportTicket.UpdatedAt;
            entity.ClosedAt = supportTicket.ClosedAt;

            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyConflictException();
        }
    }

    private IQueryable<SupportTicketEntity> BuildUserSQuery(AppDbContext db, SupportTicketSearchQuery query)
    {
        IQueryable<SupportTicketEntity> supportTicketusersQuery = db.SupportTickets.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.CreatedByUsername))
        {
            supportTicketusersQuery = supportTicketusersQuery.Where(x => x.CreatedByUsername == query.CreatedByUsername);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            var matchingStatus = Enum.GetValues<TicketStatus>()
                .Where(r => r.ToString().Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var matchingPriority = Enum.GetValues<TicketPriority>()
                .Where(r => r.ToString().Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            string pattern = $"%{search}%";

            supportTicketusersQuery = supportTicketusersQuery.Where(x =>
                EF.Functions.ILike(x.PublicId.ToString(), pattern) ||
                (string.IsNullOrEmpty(query.CreatedByUsername) && EF.Functions.ILike(x.CreatedByUsername, pattern)) ||
                EF.Functions.ILike(x.Title, pattern) ||
                EF.Functions.ILike(x.Description ?? "", pattern) ||
                (matchingStatus.Any() && matchingStatus.Contains(x.Status)) ||
                (matchingPriority.Any() && matchingPriority.Contains(x.Priority)));
        }

        if (query.Status is not null)
        {
            supportTicketusersQuery = supportTicketusersQuery.Where(
                x => x.Status == query.Status);
        }

        if (query.Priority is not null)
        {
            supportTicketusersQuery = supportTicketusersQuery.Where(
                x => x.Priority == query.Priority);
        }

        return supportTicketusersQuery;
    }

    private static SupportTicket ToDomain(SupportTicketEntity entity) => new(
        id: entity.Id,
        publicId: entity.PublicId,
        createdByUserId: entity.CreatedByUserId,
        createdByUsername: entity.CreatedByUsername,
        title: entity.Title,
        description: entity.Description,
        status: entity.Status,
        priority: entity.Priority,
        createdAt: entity.CreatedAt,
        updatedAt: entity.UpdatedAt,
        closedAt: entity.ClosedAt,
        rowVersion: entity.RowVersion
    );
}
