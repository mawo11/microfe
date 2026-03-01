namespace Mawo.Auth.Service.Client;

public class RefreshTokenResponse
{
	public bool Success { get; set; }

	public string[]? Tokens { get; set; }
}
