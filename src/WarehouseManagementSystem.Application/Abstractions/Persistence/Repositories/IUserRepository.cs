using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;

namespace WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;

public interface IUserRepository
{
    Task<bool> HasAnyUsersAsync();

    Task<int> GetUsersCountAsync();

    Task<bool> ExistsByUsernameAsync(string username);

    Task<User> AddAsync(User user);

    Task<User> GetByUsernameAsync(string username);

    Task<List<User>> GetUsersAsync(UserSearchQuery query);

    Task<int> GetUsersCountAsync(UserSearchQuery query);

    Task UpdateAsync(User user);
}
