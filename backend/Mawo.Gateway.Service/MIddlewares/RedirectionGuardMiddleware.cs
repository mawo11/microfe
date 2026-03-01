namespace Mawo.Gateway.Service.MIddlewares;

public sealed partial class RedirectionGuardMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		context.Response.OnStarting(() =>
		{
			if (context.Response.Headers.TryGetValue("Location", out var location))
			{
				try
				{
					var uri = new Uri(location.ToString());
					context.Response.Headers["Location"] = uri.PathAndQuery;
				}
				catch
				{
					// ignoruj jeśli Location nie jest pełnym URI
				}
			}

			return Task.CompletedTask;
		});

		await next(context);

	}
}
