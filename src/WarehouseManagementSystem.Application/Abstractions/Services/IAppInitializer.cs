namespace WarehouseManagementSystem.Application.Abstractions.Services;

public interface IAppInitializer
{
    Task<bool> HasAnyUsersAsync();
}
