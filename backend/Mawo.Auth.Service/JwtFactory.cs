using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

internal static class JwtFactory
{
	public static string CreateAccessToken(JwtOptions opt, int userId, string email, string[] roles, int minutes)
	{
		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, userId.ToString()),
			new(ClaimTypes.Email, email),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};
		claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Key));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: opt.Issuer,
			audience: opt.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(minutes),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public static ClaimsPrincipal? ValidateToken(JwtOptions opt, string token)
	{
		var handler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(opt.Key);

		try
		{
			return handler.ValidateToken(token, new TokenValidationParameters
			{
				ValidIssuer = opt.Issuer,
				ValidAudience = opt.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.FromSeconds(10)
			}, out _);
		}
		catch { return null; }
	}
}
