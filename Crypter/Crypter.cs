using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CustomAspNetUser.Crypter;

public class Crypter
{
    private const int SaltPosition = 0;
    private const int CipherPosition = 1;

    /// <summary>
    /// Encrypt Plain Password.
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Crypt(string plainText)
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var cipherTextBytes = GetCipherTextBytes(plainText, salt);
        var cipherText = EmbedSaltInCipherText(cipherTextBytes, salt);
        return cipherText;
    }

    /// <summary>
    /// Verify Plain password.
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static bool Verify(string plainText, string cipherText)
    {
        var salt = ExtractSaltFromCipherText(cipherText);
        var expectedHashedBytes = ExtractHashBytesFromCipherText(cipherText);

        var actualHashedBytes = GetCipherTextBytes(plainText, salt);
        return ByteArraysEqual(expectedHashedBytes, actualHashedBytes);
    }

    private static byte[] GetCipherTextBytes(string plainText, byte[] salt)
    {
        return KeyDerivation.Pbkdf2(
            plainText,
            salt,
            KeyDerivationPrf.HMACSHA1,
            10000,
            256 / 8);
    }

    private static string[] GetSplittedCipherText(string hash)
    {
        return hash.Split(new[] { ':' });
    }

    protected static byte[] ExtractHashBytesFromCipherText(string hash)
    {
        var splittedHash = GetSplittedCipherText(hash);
        return Convert.FromBase64String(splittedHash[CipherPosition]);
    }

    private static byte[] ExtractSaltFromCipherText(string hash)
    {
        var splittedHash = GetSplittedCipherText(hash);
        return Convert.FromBase64String(splittedHash[SaltPosition]);
    }

    public static string EmbedSaltInCipherText(byte[] cipherTextBytes, byte[] salt)
    {
        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(cipherTextBytes);
    }

    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (a == null && b == null) return true;

        if (a == null || b == null || a.Length != b.Length) return false;

        var areSame = true;
        for (var i = 0; i < a.Length; i++) areSame &= a[i] == b[i];

        return areSame;
    }
}