using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.Application.Abstractions.Factories.Users;

public interface IUserFactory
{
    User CreateAdmin(string username, string passwordHash);
    User CreateUser(string username, string passwordHash);
}
