using WarehouseManagementSystem.Application.Abstractions.Persistence.Repositories;
using WarehouseManagementSystem.Infrastructure.Persistence.Abstractions;

namespace WarehouseManagementSystem.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbContextFactory _dbContextFactory;

    public ProductRepository(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
}
