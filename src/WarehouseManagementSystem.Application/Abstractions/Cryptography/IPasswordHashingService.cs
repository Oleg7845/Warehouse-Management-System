namespace WarehouseManagementSystem.Application.Abstractions.Cryptography;

public  interface IPasswordHashingService
{
    string HashPassword(ReadOnlySpan<char> password);
    bool VerifyPassword(ReadOnlySpan<char> password, string passwordHash);
}
