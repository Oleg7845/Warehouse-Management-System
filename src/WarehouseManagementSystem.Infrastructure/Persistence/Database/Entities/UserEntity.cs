using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Database.Entities;

public class UserEntity
{
    public int? Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public bool MustChangePassword { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public byte[] RowVersion { get; set; } = null!;
}
