using WarehouseManagementSystem.Application.Abstractions.Utilities;

namespace WarehouseManagementSystem.Infrastructure.Utilities;

public class SkuGenerator : ISkuGenerator
{
    public string Generate()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return "PRD-" + new string(
            Enumerable.Range(0, 6)
                .Select(_ => chars[Random.Shared.Next(chars.Length)])
                .ToArray());
    }
}
