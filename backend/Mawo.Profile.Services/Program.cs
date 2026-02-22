var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.MapGet("/api/profile", () => Results.Ok(new { message = "profile content" }));
app.MapGet("/api/profile/test", () => Results.Ok("Api Profile test"));

app.MapGet("/health", () => Results.Ok("ApiService OK"));
app.Run();
