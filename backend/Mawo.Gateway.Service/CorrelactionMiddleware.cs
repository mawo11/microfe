namespace Mawo.Gateway.Service;

public sealed class CorrelationIdMiddleware
{
	public const string HeaderName = "X-Correlation-Id";

	private readonly RequestDelegate _next;

	public CorrelationIdMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		if (!context.Request.Headers.TryGetValue(HeaderName, out var correlationId) ||
			string.IsNullOrWhiteSpace(correlationId))
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

		await _next(context);
	}
}
