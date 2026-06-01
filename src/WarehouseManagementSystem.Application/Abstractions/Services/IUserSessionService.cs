using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Abstractions.Services;

public interface IUserSessionService
{
    User? CurrentUser { get; }
    bool IsAuthenticated { get; }

    void SetUser(User user);
    void ClearUser();
}
