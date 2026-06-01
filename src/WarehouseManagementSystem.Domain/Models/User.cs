using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Domain.Models;

public class User
{
    public int? Id { get; init; }
    public string Username { get; init; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsLocked { get; private set; }
    public bool MustChangePassword { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTimeOffset? LastLoginAt { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public byte[]? RowVersion { get; private set; }

    public User(
        int? id,
        string username,
        string passwordHash,
        UserRole role,
        bool isActive,
        bool isLocked,
        bool mustChangePassword,
        int failedLoginAttempts,
        DateTimeOffset? lastLoginAt,
        DateTimeOffset createdAt,
        byte[]? rowVersion)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty");

        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = isActive;
        IsLocked = isLocked;
        MustChangePassword = mustChangePassword;
        FailedLoginAttempts = failedLoginAttempts;
        LastLoginAt = lastLoginAt;
        CreatedAt = createdAt;
        RowVersion = rowVersion;
    }

    public void ChangeRole(UserRole newRole)
    {
        Role = newRole;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        MustChangePassword = false;
    }

    public void ResetMustChangePassword()
    {
        MustChangePassword = false;
    }

    public void ResetPassword(string tempPasswordHash)
    {
        PasswordHash = tempPasswordHash;
        MustChangePassword = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void IncrementFailedLoginAttempts()
    {
        FailedLoginAttempts++;
    }

    public void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
    }

    public void UpdateLastLoginTime()
    {
        LastLoginAt = DateTimeOffset.UtcNow;
    }

    public void Lock()
    {
        IsLocked = true;
    }

    public void Unlock()
    {
        IsLocked = false;
    }
}
