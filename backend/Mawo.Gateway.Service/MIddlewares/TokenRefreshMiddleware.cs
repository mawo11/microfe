using Mawo.Auth.Service.Client;
using System.IdentityModel.Tokens.Jwt;

namespace Mawo.Gateway.Service.MIddlewares;

public sealed partial class TokenRefreshMiddleware : IMiddleware
{
	private readonly ILogger<TokenRefreshMiddleware> _logger;
	private readonly IAuthServiceClient _authServiceClient;

	public TokenRefreshMiddleware(
		ILogger<TokenRefreshMiddleware> logger,
		IAuthServiceClient authServiceClient)
	{
		_logger = logger;
		_authServiceClient = authServiceClient;
	}

	private async Task TryRefreshToken(HttpContext context, string refreshToken)
	{
		try
		{
			var response = await _authServiceClient.Refresh(refreshToken, context.RequestAborted);
			if (response.Success && response.Tokens is not null)
			{
				foreach (var cookie in response.Tokens)
				{
					context.Response.Headers.Append("Set-Cookie", cookie);
				}
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Token refresh failed");
		}
	}

	private static bool TokenExpiresSoon(string token)
	{
		try
		{
			var handler = new JwtSecurityTokenHandler();
			var jwt = handler.ReadJwtToken(token);
			return jwt.ValidTo < DateTime.UtcNow.AddMinutes(5);
		}
		catch
		{
			return false;
		}
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		var refreshToken = context.Request.Cookies["RefreshToken"];

		if (!string.IsNullOrEmpty(refreshToken) && TokenExpiresSoon(refreshToken))
		{
			_logger.LogInformation("Access token is expiring soon, attempting to refresh");
			await TryRefreshToken(context, refreshToken);
		}

		await next(context);
	}
}
