namespace Mawo.Auth.Service.Client;

public sealed class AuthServiceClient : IAuthServiceClient
{
	private static readonly RefreshTokenResponse Fail = new() { Success = false };

	private readonly HttpClient _httpClient;

	public AuthServiceClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}


	public async ValueTask<RefreshTokenResponse> Refresh(string refreshToken, CancellationToken cancellationToken)
	{
		var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/refresh");
		request.Headers.Add("RefreshToken", refreshToken);

		var response = await _httpClient.SendAsync(request);

		if (response.IsSuccessStatusCode && response.Headers.TryGetValues("Set-Cookie", out var cookies))
		{
			return new RefreshTokenResponse
			{
				Success = true,
				Tokens = [.. cookies]
			};
		}

		return Fail;
	}
}
