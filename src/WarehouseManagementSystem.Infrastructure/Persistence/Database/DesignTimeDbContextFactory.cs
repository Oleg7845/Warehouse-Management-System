using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Design;
using WarehouseManagementSystem.Infrastructure.Persistence.Database.Migrations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Update EF Core
        // dotnet tool update --global dotnet-ef

        // Add migration
        /*
            dotnet ef migrations add InitialPostgres `
                --project WarehouseManagementSystem.Infrastructure `
                --startup - project WarehouseManagementSystem.UI `
                -o Persistence / Database / Migrations
        */

        return new AppDbContext("Host=localhost;Port=5432;Database=wms_db;Username=wms_admin;Password=erX0QjECFh0SSI8PkO7ZVAIry1cdEGXE");
    }
}
