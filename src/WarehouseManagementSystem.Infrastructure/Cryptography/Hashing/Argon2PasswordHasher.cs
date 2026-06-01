using WarehouseManagementSystem.Application.Abstractions.Cryptography;

namespace WarehouseManagementSystem.Infrastructure.Cryptography.Hashing;

public class Argon2PasswordHasher : IPasswordHashingService
{
    public string HashPassword(ReadOnlySpan<char> password)
        => Argon2Id.HashPassword(password);

    public bool VerifyPassword(ReadOnlySpan<char> password, string passwordHash)
        => Argon2Id.VerifyPassword(password, passwordHash);
}
