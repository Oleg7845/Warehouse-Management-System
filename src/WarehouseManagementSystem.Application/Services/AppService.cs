using WarehouseManagementSystem.Application.Abstractions.Services;

namespace WarehouseManagementSystem.Application.Services;

public class AppService : IAppService
{
    public string AppName => "Warehouse Management System (WMS)";

    public string AppVersion => "1.0.0";

    public string DbConnectionString => "Host=localhost;Port=5432;Database=wms_db;Username=wms_admin;Password=erX0QjECFh0SSI8PkO7ZVAIry1cdEGXE";

    public int MaxFailedLoginAttemptsCount => 3;
}
