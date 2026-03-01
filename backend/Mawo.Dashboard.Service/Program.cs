using Mawo.Dashboard.Service;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
builder.Configuration.AddEnvironmentVariables();

app.MapGet("/api/dashboard", () => Results.Ok(new { message = "portal content" }));

app.Map("/api/dashboard/test", () => Results.Ok("Api Portal test"));

app.MapGet("/api/dashboard/diagnostic", (HttpContext context) =>
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
