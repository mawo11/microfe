namespace Mawo.Gateway.Service.MIddlewares;

public sealed class CorrelationIdMiddleware : IMiddleware
{
	public const string HeaderName = "X-Correlation-Id";

	public async Task Invoke(HttpContext context)
	{

	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if (!context.Request.Headers.TryGetValue(HeaderName, out var correlationId) || string.IsNullOrWhiteSpace(correlationId))
		{
			correlationId = Guid.NewGuid().ToString("N");
			context.Request.Headers[HeaderName] = correlationId;
		}

		// zapisz w HttpContext (przydatne do logów)
		context.Items[HeaderName] = correlationId.ToString();

		// dodaj do response
		context.Response.OnStarting(() =>
		{
			context.Response.Headers[HeaderName] = correlationId.ToString();
			return Task.CompletedTask;
		});

		await next(context);
	}
}
