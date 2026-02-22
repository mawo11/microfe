var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/api/portal", () => Results.Ok(new { message = "portal content" }));

app.Map("/api/portal/test", () => Results.Ok("Api Portal test"));

app.MapGet("/health", () => Results.Ok("ApiService OK"));
app.Run();
