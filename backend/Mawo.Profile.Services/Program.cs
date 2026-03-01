using Mawo.Profile.Services;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
builder.Configuration.AddEnvironmentVariables();

app.MapGet("/api/profile", () => Results.Ok(new { message = "profile content" }));
app.MapGet("/api/profile/test", () => Results.Ok("Api Profile test"));

app.MapGet("/health", () => Results.Ok("ApiService OK"));

app.MapGet("api/profile/diagnostic", (HttpContext context) =>
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

app.Run();
