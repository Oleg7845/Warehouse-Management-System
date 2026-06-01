namespace WarehouseManagementSystem.Application.Abstractions.Security;

public interface IPasswordGenerator
{
    string GenerateText(int length, bool useUpper, bool useLower, bool useDigits, bool useSymbols);

    char[] GenerateChars(int length, bool useUpper, bool useLower, bool useDigits, bool useSymbols);

    void Clear(char[] data);
}
