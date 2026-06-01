using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Infrastructure.Persistence.Abstractions;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Database;

/// <summary>
/// Provides a factory for creating instances of <see cref="AppDbContext"/>.
/// Handles the initialization and lifecycle management of the SQLite database.
/// </summary>
/// <remarks>
/// Using a factory pattern ensures that short-lived context instances are used for each operation, 
/// which is a best practice for desktop applications to avoid memory leaks and stale data.
/// </remarks>
public class DbContextFactory : IDbContextFactory
{
    private readonly string _connectionString;

    public DbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Ensures that the database file exists and the schema is correctly created.
    /// Should be called during the application startup sequence.
    /// </summary>
    /// <remarks>
    /// Uses <c>Database.EnsureCreated()</c> to automatically generate tables based on 
    /// the EF Core model if they do not already exist.
    /// </remarks>
    public void Initialize()
    {
        using (var context = CreateDbContext())
        {
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw new DatabaseConnectionException(innerException: ex);
            }
        }
    }

    /// <summary>
    /// Creates a new, ready-to-use instance of <see cref="AppDbContext"/>.
    /// </summary>
    /// <returns>A new database context instance.</returns>
    public AppDbContext CreateDbContext()
    {
        return new AppDbContext(_connectionString);
    }
}
