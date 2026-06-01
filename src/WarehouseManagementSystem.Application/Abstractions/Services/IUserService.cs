using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;

namespace WarehouseManagementSystem.Application.Abstractions.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(string username, string password, UserRole role, UserCreationContext context = UserCreationContext.Default);

    Task<int> GetUsersCountAsync();

    Task<User> GetUserByUsernameAsync(string username);

    Task<List<User>> GetUsersAsync(UserSearchQuery query);

    Task<int> GetUsersCountAsync(UserSearchQuery query);

    Task<User> GetVerifiedUserAsync(string username, string password);

    Task SetUserSession(User user);

    Task<User> ForceChangePasswordWithLoginAsync(string username, string newPassword);

    Task LogoutAsync();

    Task ActivateUserAsync(string username);

    Task DeactivateUserAsync(string username);

    Task LockUserAsync(string username);

    Task UnlockUserAsync(string username);

    Task ResetFailedLoginAttemptsAsync(string username);

    Task ChangePasswordAsync(string username, string currentPassword, string newPassword);

    Task ResetPasswordAsync(string username, string tempPassword);

    string FormatUserCredentials(string username, string password);
}
