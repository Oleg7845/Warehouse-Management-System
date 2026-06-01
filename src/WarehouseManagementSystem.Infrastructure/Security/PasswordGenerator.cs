using System.Security.Cryptography;
using WarehouseManagementSystem.Application.Abstractions.Security;

namespace WarehouseManagementSystem.Infrastructure.Security;

public sealed class PasswordGenerator : IPasswordGenerator
{
    private static readonly char[] Lower = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
    private static readonly char[] Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private static readonly char[] Digits = "0123456789".ToCharArray();
    private static readonly char[] Symbols = "!@#$%^&*()-_=+[]{}|;:,.<>?".ToCharArray();

    private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    public int GetPasswordLength(char[] password) => password?.Length ?? 0;

    public bool HasLowercase(char[] password) => ContainsAny(password, Lower);

    public bool HasUppercase(char[] password) => ContainsAny(password, Upper);

    public bool HasDigits(char[] password) => ContainsAny(password, Digits);

    public bool HasSymbols(char[] password) => ContainsAny(password, Symbols);

    public char[] GenerateChars(
        int length,
        bool useUpper,
        bool useLower,
        bool useDigits,
        bool useSymbols)
    {
        if (length <= 0)
            throw new ArgumentException("Length must be > 0");

        int poolSize = 0;

        if (useLower) poolSize += Lower.Length;
        if (useUpper) poolSize += Upper.Length;
        if (useDigits) poolSize += Digits.Length;
        if (useSymbols) poolSize += Symbols.Length;

        if (poolSize == 0)
            throw new ArgumentException("No character sets selected");

        char[] pool = new char[poolSize];
        int pos = 0;

        if (useLower)
        {
            Array.Copy(Lower, 0, pool, pos, Lower.Length);
            pos += Lower.Length;
        }

        if (useUpper)
        {
            Array.Copy(Upper, 0, pool, pos, Upper.Length);
            pos += Upper.Length;
        }

        if (useDigits)
        {
            Array.Copy(Digits, 0, pool, pos, Digits.Length);
            pos += Digits.Length;
        }

        if (useSymbols)
        {
            Array.Copy(Symbols, 0, pool, pos, Symbols.Length);
        }

        char[] result = new char[length];
        int index = 0;

        if (useLower) result[index++] = Lower[NextInt(Lower.Length)];
        if (useUpper) result[index++] = Upper[NextInt(Upper.Length)];
        if (useDigits) result[index++] = Digits[NextInt(Digits.Length)];
        if (useSymbols) result[index++] = Symbols[NextInt(Symbols.Length)];

        while (index < length)
        {
            result[index++] = pool[NextInt(pool.Length)];
        }

        Shuffle(result);
        Array.Clear(pool);

        return result;
    }

    public string GenerateText(
        int length,
        bool useUpper,
        bool useLower,
        bool useDigits,
        bool useSymbols)
    {
        char[] chars = GenerateChars(length, useUpper, useLower, useDigits, useSymbols);
        string result = new(chars);
        Array.Clear(chars);
        return result;
    }

    private bool ContainsAny(char[] password, char[] charset)
    {
        if (password == null || password.Length == 0)
            return false;

        foreach (char p in password)
        {
            foreach (char c in charset)
            {
                if (p == c)
                    return true;
            }
        }

        return false;
    }

    private void Shuffle(char[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = NextInt(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    public void Clear(char[] data)
    {
        if (data != null)
            Array.Clear(data);
    }

    private int NextInt(int maxExclusive)
    {
        if (maxExclusive <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxExclusive));

        byte[] buffer = new byte[4];
        _rng.GetBytes(buffer);

        int value = BitConverter.ToInt32(buffer, 0) & int.MaxValue;

        return value % maxExclusive;
    }
}
