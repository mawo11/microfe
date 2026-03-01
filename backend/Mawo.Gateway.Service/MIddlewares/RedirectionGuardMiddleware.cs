namespace Mawo.Gateway.Service.MIddlewares;

public sealed partial class RedirectionGuardMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		await next(context);

		if (context.Response.Headers.TryGetValue("Location", out var location))
		{
			var uri = new Uri(location.ToString());
			// zostaw tylko ścieżkę
			context.Response.Headers["Location"] = uri.PathAndQuery;
		}
	}
}
