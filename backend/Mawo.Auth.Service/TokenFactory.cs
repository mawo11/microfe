using System.Security.Cryptography;

internal static class TokenFactory
{
	public static string GenerateRefreshToken()
	{
		var bytes = RandomNumberGenerator.GetBytes(64);
		return Convert.ToBase64String(bytes);
	}
}
