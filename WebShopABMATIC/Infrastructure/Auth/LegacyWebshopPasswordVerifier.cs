using System.Security.Cryptography;
using System.Text;

namespace WebShopABMATIC.Infrastructure.Auth;

/// <summary>
/// Verifies webshop passwords stored in <c>Klanten.Klant.PasswordWebshop</c> + <c>SaltWebshop</c> (legacy ABMATIC).
/// </summary>
public static class LegacyWebshopPasswordVerifier
{
    public static bool Verify(string password, string? storedHash, string? storedSalt)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(storedSalt))
        {
            return string.Equals(password, storedHash, StringComparison.Ordinal);
        }

        try
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var passwordBytes = Encoding.Unicode.GetBytes(password);

            var combined = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combined, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combined, saltBytes.Length, passwordBytes.Length);
            var hash = Convert.ToBase64String(SHA1.HashData(combined));
            if (FixedTimeEquals(hash, storedHash))
            {
                return true;
            }

            var utf8Combined = Encoding.UTF8.GetBytes(storedSalt + password);
            var utf8Hash = Convert.ToBase64String(SHA1.HashData(utf8Combined));
            return FixedTimeEquals(utf8Hash, storedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static (string Hash, string Salt) CreateHash(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var passwordBytes = Encoding.Unicode.GetBytes(password);
        var combined = new byte[saltBytes.Length + passwordBytes.Length];
        Buffer.BlockCopy(saltBytes, 0, combined, 0, saltBytes.Length);
        Buffer.BlockCopy(passwordBytes, 0, combined, saltBytes.Length, passwordBytes.Length);
        return (Convert.ToBase64String(SHA1.HashData(combined)), Convert.ToBase64String(saltBytes));
    }

    private static bool FixedTimeEquals(string a, string b) =>
        CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(a),
            Encoding.UTF8.GetBytes(b));
}
