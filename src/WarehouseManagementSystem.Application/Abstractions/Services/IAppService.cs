namespace WarehouseManagementSystem.Application.Abstractions.Services;

public interface IAppService
{
    string AppName { get; }
    string AppVersion { get; }
    string DbConnectionString { get; }
    int MaxFailedLoginAttemptsCount { get; }
}
