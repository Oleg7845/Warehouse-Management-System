using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Domain.Exceptions;
using System.Data;
using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.Infrastructure.Persistence.Abstractions;
using WarehouseManagementSystem.Infrastructure.Persistence.Database;
using WarehouseManagementSystem.Infrastructure.Persistence.Database.Entities;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory _dbContextFactory;

    public UserRepository(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<bool> HasAnyUsersAsync()
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.Users.AnyAsync();
    }

    public async Task<int> GetUsersCountAsync()
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.Users.CountAsync();
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        using var db = _dbContextFactory.CreateDbContext();
        return await db.Users.AnyAsync(x => x.Username == username);
    }

    public async Task<User> AddAsync(User user)
    {
        using var db = _dbContextFactory.CreateDbContext();

        UserEntity entity = new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            Role = user.Role,
            IsActive = user.IsActive,
            IsLocked = user.IsLocked,
            MustChangePassword = user.MustChangePassword,
            FailedLoginAttempts = user.FailedLoginAttempts,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };

        db.Users.Add(entity);
        await db.SaveChangesAsync();

        return ToDomain(entity);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        using var db = _dbContextFactory.CreateDbContext();

        UserEntity? entity = await db.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (entity == null)
            throw new UserNotFoundException(username);

        return ToDomain(entity);
    }

    public async Task<List<User>> GetUsersAsync(UserSearchQuery query)
    {
        using var db = _dbContextFactory.CreateDbContext();

        IQueryable<UserEntity> usersQuery = BuildUserSQuery(db, query);

        var entities = await usersQuery
            .OrderBy(x => x.Id)
            .Skip(query.Offset)
            .Take(query.Limit)
            .ToListAsync();

        return entities.Select(ToDomain).ToList();
    }

    public async Task<int> GetUsersCountAsync(UserSearchQuery query)
    {
        using var db = _dbContextFactory.CreateDbContext();

        IQueryable<UserEntity> usersQuery = BuildUserSQuery(db, query);

        return await usersQuery.CountAsync();
    }

    public async Task UpdateAsync(User user)
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();

            UserEntity? entity = await db.Users.FirstOrDefaultAsync(x => x.Username == user.Username);

            if (entity == null)
                throw new UserNotFoundException(user.Username);

            entity.PasswordHash = user.PasswordHash;
            entity.Role = user.Role;
            entity.IsActive = user.IsActive;
            entity.IsLocked = user.IsLocked;
            entity.MustChangePassword = user.MustChangePassword;
            entity.FailedLoginAttempts = user.FailedLoginAttempts;
            entity.LastLoginAt = user.LastLoginAt?.ToUniversalTime();

            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyConflictException();
        }
    }

    private IQueryable<UserEntity> BuildUserSQuery(AppDbContext db, UserSearchQuery query)
    {
        IQueryable<UserEntity> usersQuery = db.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            var matchingRoles = Enum.GetValues<UserRole>()
                .Where(r => r.ToString().Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            usersQuery = usersQuery.Where(x =>
                EF.Functions.ILike(x.Username, $"%{search}%") ||
                (matchingRoles.Any() && matchingRoles.Contains(x.Role)));
        }

        if (query.Role is not null)
        {
            usersQuery = usersQuery.Where(
                x => x.Role == query.Role);
        }

        return usersQuery;
    }

    private static User ToDomain(UserEntity entity) => new (
        id: entity.Id,
        username: entity.Username,
        passwordHash: entity.PasswordHash,
        role: entity.Role,
        isActive: entity.IsActive,
        isLocked: entity.IsLocked,
        mustChangePassword: entity.MustChangePassword,
        failedLoginAttempts: entity.FailedLoginAttempts,
        lastLoginAt: entity.LastLoginAt?.ToUniversalTime(),
        createdAt: entity.CreatedAt.ToUniversalTime(),
        rowVersion: entity.RowVersion
    );
}