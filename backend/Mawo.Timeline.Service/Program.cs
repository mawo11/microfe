using Mawo.Timeline.Service;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
builder.Configuration.AddEnvironmentVariables();

app.MapGet("/api/timeline", () => Results.Ok(new { message = "portal content" }));

app.Map("/api/timeline/test", () => Results.Ok("Api Portal test"));

app.MapGet("/api/timeline/diagnostic", (HttpContext context) =>
{
	var headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

	DiagItem[] items = context.Request.Headers
								.Select(h => new DiagItem
								{
									Key = h.Key,
									Value = h.Value.ToString()
								})
								.ToArray()!;

	return items;
});


app.MapGet("/health", () => Results.Ok("ApiService OK"));
app.Run();
