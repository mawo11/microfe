namespace Mawo.Auth.Service.Client
{
	public interface IAuthServiceClient
	{
		ValueTask<RefreshTokenResponse> Refresh(string refreshToken, CancellationToken cancellationToken);
	}
}