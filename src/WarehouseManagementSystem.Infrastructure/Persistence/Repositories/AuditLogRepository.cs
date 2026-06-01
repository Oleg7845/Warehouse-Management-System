using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.Infrastructure.Persistence.Abstractions;
using WarehouseManagementSystem.Infrastructure.Persistence.Database;
using WarehouseManagementSystem.Infrastructure.Persistence.Database.Entities;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly IDbContextFactory _dbContextFactory;

    public AuditLogRepository(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        using var db = _dbContextFactory.CreateDbContext();

        AuditLogEntity entity = new AuditLogEntity
        {
            PublicId = auditLog.PublicId,
            ActorId = auditLog.ActorId,
            ActorName = auditLog.ActorName,
            ActorType = auditLog.ActorType,
            Action = auditLog.Action,
            SubjectId = auditLog.SubjectId,
            SubjectType = auditLog.SubjectType,
            DetailsJson = auditLog.DetailsJson,
            CreatedAt = auditLog.CreatedAt
        };

        db.AuditLogs.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task<int> GetAuditLogsCountAsync()
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.AuditLogs.CountAsync();
    }

    public async Task<AuditLog> GetByPublicIdAsync(Guid publicId)
    {
        using var db = _dbContextFactory.CreateDbContext();

        AuditLogEntity? entity = await db.AuditLogs.FirstOrDefaultAsync(x => x.PublicId == publicId);

        if (entity == null)
            throw new AuditLogNotFoundException(publicId);

        return ToDomain(entity);
    }
    
    public async Task<int> GetAuditLogsCountAsync(AuditLogSearchQuery query)
    {
        using var db = _dbContextFactory.CreateDbContext();

        IQueryable<AuditLogEntity> auditLogsQuery = BuildAuditLogsQuery(db, query);

        return await auditLogsQuery.CountAsync();
    }

    public async Task<List<AuditLog>> GetAuditLogsAsync(AuditLogSearchQuery query)
    {
        using var db = _dbContextFactory.CreateDbContext();

        IQueryable<AuditLogEntity> auditLogsQuery = BuildAuditLogsQuery(db, query);

        var entities = await auditLogsQuery
            .OrderBy(x => x.Id)
            .Skip(query.Offset)
            .Take(query.Limit)
            .ToListAsync();

        return entities.Select(ToDomain).ToList();
    }

    private IQueryable<AuditLogEntity> BuildAuditLogsQuery(AppDbContext db, AuditLogSearchQuery query)
    {
        IQueryable<AuditLogEntity> auditLogsQuery = db.AuditLogs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            string search = query.Search.Trim();

            string pattern = $"%{search}%";

            auditLogsQuery = auditLogsQuery.Where(x =>
                EF.Functions.ILike(x.PublicId.ToString(), pattern) ||
                EF.Functions.ILike(x.ActorName ?? "", pattern) ||
                EF.Functions.ILike(Convert.ToString(x.DetailsJson), pattern));
        }

        if (query.ActorType is not null)
        {
            auditLogsQuery = auditLogsQuery.Where(
                x => x.ActorType == query.ActorType);
        }

        if (query.Action is not null)
        {
            auditLogsQuery = auditLogsQuery.Where(
                x => x.Action == query.Action);
        }

        if (query.SubjectType is not null)
        {
            auditLogsQuery = auditLogsQuery.Where(
                x => x.SubjectType == query.SubjectType);
        }

        return auditLogsQuery;
    }

    private static AuditLog ToDomain(AuditLogEntity entity) => new(
        publicId: entity.PublicId,
        actorId: entity.ActorId,
        actorName: entity.ActorName,
        actorType: entity.ActorType,
        action: entity.Action,
        subjectId: entity.SubjectId,
        subjectType: entity.SubjectType,
        detailsJson: entity.DetailsJson,
        createdAt: entity.CreatedAt
    );
}
