using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Services;

public class UserSessionService : IUserSessionService
{
    public User? CurrentUser { get; private set; }

    public bool IsAuthenticated => CurrentUser != null;

    public void SetUser(User user)
    {
        CurrentUser = user;
    }

    public void ClearUser()
    {
        CurrentUser = null;
    }
}
