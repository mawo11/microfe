using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Mawo.Gateway.Service.RequestTransforms;

public class JwTokenTransformer : ITransformProvider
{
	public void Apply(TransformBuilderContext context)
	{
		context.AddRequestTransform(ctx =>
		{
			// JWT np. w cookie "AuthToken"
			var token = ctx.HttpContext.Request.Cookies["AuthToken"];

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


			var userId = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
			if (!string.IsNullOrEmpty(userId))
			{
				AddHeader(ctx, "X-User-Id", userId);
			}

			var email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			if (!string.IsNullOrEmpty(email))
			{
				AddHeader(ctx, "X-User-Email", email);
			}

			var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
			if (!string.IsNullOrEmpty(role))
			{
				AddHeader(ctx, "X-Role", role);
			}


			return ValueTask.CompletedTask;
		});
	}

	public void ValidateCluster(TransformClusterValidationContext context)
	{
	}

	public void ValidateRoute(TransformRouteValidationContext context)
	{
	}

	private static void AddHeader(RequestTransformContext ctx, string name, string value)
	{
		ctx.ProxyRequest.Headers.Remove(name);
		ctx.ProxyRequest.Headers.TryAddWithoutValidation(name, value);
	}
}
