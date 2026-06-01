using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.Domain.Queries;

public sealed class UserSearchQuery
{
    public string? Search { get; init; }  // Username

    public UserRole? Role { get; init; }

    public int Offset { get; init; }

    public int Limit { get; init; }
}
