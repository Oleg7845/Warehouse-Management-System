using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Infrastructure.Persistence.Database.Entities;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Database;

/// <summary>
/// The primary database context for the Onyx Archiver application.
/// Manages persistent storage for user profiles and peer information using Entity Framework Core and SQLite.
/// </summary>
/// <remarks>
/// This context is responsible for configuring the database connection, 
/// defining schema constraints, and managing transactions for domain entities.
/// </remarks>
public class AppDbContext : DbContext
{
    private readonly string _connectionString;

    /// <summary> Gets the set of local user profiles (identities) stored in the database. </summary>
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<AuditLogEntity> AuditLogs => Set<AuditLogEntity>();
    public DbSet<SupportTicketEntity> SupportTickets => Set<SupportTicketEntity>();
    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    /// <summary>
    /// Initializes a new instance of the context with a specific path to the SQLite database file.
    /// </summary>
    /// <param name="db">The absolute or relative path to the .db file.</param>
    public AppDbContext(string connectionString) => _connectionString = connectionString;

    /// <summary>
    /// Configures the context to use the SQLite provider with shared cache enabled for better performance.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }

    /// <summary>
    /// Configures the database schema, including keys, constraints, and indexes using Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            entity.HasIndex(e => e.Username)
                .IsUnique();

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<AuditLogEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PublicId)
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.ActorName)
                .HasMaxLength(50);

            entity.Property(e => e.ActorType)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            entity.Property(e => e.SubjectType)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            entity.Property(e => e.DetailsJson)
                .IsRequired()
                .HasColumnType("jsonb");

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.HasIndex(e => e.CreatedAt);

            entity.HasIndex(e => e.ActorId);

            entity.HasIndex(e => new { e.SubjectType, e.SubjectId });

            entity.HasIndex(e => e.PublicId)
                .IsUnique();
        });

        modelBuilder.Entity<SupportTicketEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PublicId)
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            entity.Property(e => e.Priority)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt);

            entity.Property(e => e.ClosedAt);

            entity.Property(e => e.CreatedByUserId)
                .IsRequired();

            entity.HasIndex(e => e.Status);

            entity.HasIndex(e => e.Priority);

            entity.HasIndex(e => e.CreatedByUserId);

            entity.HasIndex(e => e.CreatedAt);

            entity.HasIndex(e => new { e.Status, e.Priority });

            entity.HasIndex(e => new { e.CreatedByUserId, e.Status });

            entity.HasIndex(e => e.PublicId)
                .IsUnique();

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.SKU)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("numeric(18,2)");

            entity.Property(e => e.QuantityInStock)
                .IsRequired();

            entity.Property(e => e.CategoryId)
                .IsRequired();

            entity.Property(e => e.SupplierId)
                .IsRequired();

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired();

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasIndex(e => e.SKU)
                .IsUnique();

            entity.HasIndex(e => e.Name);

            entity.HasIndex(e => e.CategoryId);

            entity.HasIndex(e => e.SupplierId);

            entity.HasIndex(e => e.IsDeleted);
        });
    }
}
