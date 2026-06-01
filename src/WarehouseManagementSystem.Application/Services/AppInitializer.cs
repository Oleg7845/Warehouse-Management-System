using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Application.Abstractions.Services;

namespace WarehouseManagementSystem.Application.Services;

public class AppInitializer : IAppInitializer
{
    private readonly IUserRepository _repository;

    public AppInitializer(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HasAnyUsersAsync()
    {
        return await _repository.HasAnyUsersAsync();
    }
}
