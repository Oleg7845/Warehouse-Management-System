using WarehouseManagementSystem.Infrastructure.Persistence.Database;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Abstractions;

/// <summary>
/// Defines a factory for creating instances of <see cref="AppDbContext"/>.
/// Used to manage database context lifecycle and ensure thread safety in asynchronous operations.
/// </summary>
/// <remarks>
/// In a desktop application environment, this factory prevents issues related to 
/// long-running contexts, such as stale data and multi-threading conflicts.
/// </remarks>
public interface IDbContextFactory
{
    void Initialize();

    /// <summary>
    /// Creates and configures a new instance of the application database context.
    /// </summary>
    /// <returns>A fresh instance of <see cref="AppDbContext"/> configured for the local environment.</returns>
    AppDbContext CreateDbContext();

}
