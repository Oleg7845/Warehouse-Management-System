using WarehouseManagementSystem.Application.Abstractions.Factories.Users;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Factories.Users;

public class UserFactory : IUserFactory
{
    public User CreateAdmin(string username, string passwordHash)
    {
        return Build(
            username: username,
            passwordHash: passwordHash,
            role: UserRole.Admin);
    }

    public User CreateUser(string username, string passwordHash)
    {
        return Build(
            username: username,
            passwordHash: passwordHash,
            role: UserRole.User);
    }

    private User Build(string username, string passwordHash, UserRole role)
    {
        return new User(
            id: null,
            username: username,
            passwordHash: passwordHash,
            role: role,
            isActive: true,
            isLocked: false,
            mustChangePassword: true,
            failedLoginAttempts: 0,
            lastLoginAt: null,
            createdAt: DateTimeOffset.UtcNow,
            rowVersion: null
        );
    }
}
