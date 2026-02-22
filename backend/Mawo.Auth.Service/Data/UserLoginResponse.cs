namespace Mawo.Auth.Service.Data;

public class UserLoginResponse
{
	public bool Success { get; set; }

	public string? UrlToRedirect { get; set; }
}
