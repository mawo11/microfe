using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Mawo.Gateway.Service.RequestTransforms;

public sealed class AuthTransformProvider : ITransformProvider
{
	public void Apply(TransformBuilderContext context)
	{
		if (context.Route?.Metadata is null)
		{
			return;
		}

		if (!context.Route.Metadata.TryGetValue("mode", out var mode))
		{
			return;
		}

		if (mode == "cookies")
		{
			ApplyCookieMode(context);
		}
		else if (mode == "jwt")
		{
			ApplyJwtMode(context);
		}
	}

	private static void ApplyCookieMode(TransformBuilderContext context)
	{
		context.AddRequestTransform(ctx =>
		{
			return ValueTask.CompletedTask;
		});
	}

	private static void ApplyJwtMode(TransformBuilderContext context)
	{
		context.AddRequestTransform(ctx =>
		{
			var httpContext = ctx.HttpContext;

			var token = httpContext.Request.Cookies["AuthToken"];
			if (string.IsNullOrWhiteSpace(token))
			{
				return ValueTask.CompletedTask;
			}

			var handler = new JwtSecurityTokenHandler();
			if (!handler.CanReadToken(token))
			{
				return ValueTask.CompletedTask;
			}

			var jwt = handler.ReadJwtToken(token);

			AddHeader(ctx, "Authorization", $"Bearer {token}");

			var userId = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
			if (!string.IsNullOrEmpty(userId))
			{
				AddHeader(ctx, "X-User-Id", userId);
			}

			ctx.ProxyRequest.Headers.Remove("X-User-Role");

			var roles = jwt.Claims.Where(x => x.Type == ClaimTypes.Role)
						.Select(x => x.Value)
						.ToArray();
			foreach (var role in roles)
			{
				ctx.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Role", role);
			}

			return ValueTask.CompletedTask;
		});
	}

	private static void AddHeader(RequestTransformContext ctx, string name, string value)
	{
		ctx.ProxyRequest.Headers.Remove(name);
		ctx.ProxyRequest.Headers.TryAddWithoutValidation(name, value);
	}

	public void ValidateRoute(TransformRouteValidationContext context) { }

	public void ValidateCluster(TransformClusterValidationContext context) { }
}