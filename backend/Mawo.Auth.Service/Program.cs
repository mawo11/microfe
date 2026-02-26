using Mawo.Auth.Service.Data;
using Mawo.Configuration.Api.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddSingleton(new JwtOptions(jwtKey, jwtIssuer, jwtAudience));
builder.Services.AddSingleton<IRefreshTokenStore, InMemoryRefreshTokenStore>();

builder.Services.AddCors(opt =>
{
	opt.AddPolicy("Frontends", p =>
		p.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? Array.Empty<string>())
		 .AllowAnyHeader()
		 .AllowAnyMethod()
		 .AllowCredentials());
});
builder.Services.AddHttpClient<IApiConfigurationServiceClient, ApiConfigurationServiceClient>(client =>
{
	client.BaseAddress = new Uri(builder.Configuration["Endpoints:ApiConfigurationUrl"]!);
});

var app = builder.Build();
app.UseCors("Frontends");

var cookieOptions = new CookieOptions
{
	HttpOnly = true,
	Secure = false, // set true on HTTPS
	SameSite = SameSiteMode.Lax,
	//Domain = ".test.pl",
	//Domain = "localhost"
	Path = "/"
};



app.MapPost("/api/auth/login", async (
	[FromBody] LoginRequest request,
	[FromServices] JwtOptions jwt,
	[FromServices] IRefreshTokenStore store,
	[FromServices] IApiConfigurationServiceClient apiConfigurationServiceClient,
	HttpContext ctx) =>
{
	var user = UserStorage.Users.FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);

	if (user is null)
	{
		return Results.Unauthorized();
	}

	var access = JwtFactory.CreateAccessToken(jwt, user.Id, request.Email, [user.Role!], minutes: 10, user.Feature);
	var refresh = TokenFactory.GenerateRefreshToken();

	await store.StoreAsync(user.Id, refresh);

	ctx.Response.Cookies.Append("AuthToken", access, cookieOptions);
	ctx.Response.Cookies.Append("RefreshToken", refresh, cookieOptions);

	var result = await apiConfigurationServiceClient.GetAppUrl("dashboard", CancellationToken.None);

	return Results.Ok(new UserLoginResponse
	{
		Success = true,
		UrlToRedirect = result?.Url
	});
});

app.MapPost("/api/auth/refresh", async (IRefreshTokenStore store, JwtOptions jwt, HttpContext ctx) =>
{
	var refresh = ctx.Request.Cookies["refresh_token"];
	if (string.IsNullOrWhiteSpace(refresh))
	{
		return Results.Unauthorized();
	}

	var userId = await store.ValidateAndRotateAsync(refresh);
	if (userId == 0)
	{
		return Results.Unauthorized();
	}

	var user = UserStorage.Users.FirstOrDefault(x => x.Id == userId);
	if (user is null)
	{
		return Results.Unauthorized();
	}


	var access = JwtFactory.CreateAccessToken(jwt, userId, user.Email!, [user.Role!], minutes: 10, user.Feature);
	var newRefresh = TokenFactory.GenerateRefreshToken();
	await store.StoreAsync(userId, newRefresh);

	ctx.Response.Cookies.Append("AuthToken", access, cookieOptions);
	ctx.Response.Cookies.Append("RefreshToken", newRefresh, cookieOptions);

	return Results.Ok(new { ok = true });
});

app.MapPost("/api/auth/logout", async ([FromServices] IRefreshTokenStore store, HttpContext ctx) =>
{
	var refresh = ctx.Request.Cookies["refresh_token"];
	if (!string.IsNullOrWhiteSpace(refresh))
	{
		await store.RevokeAsync(refresh);
	}

	ctx.Response.Cookies.Delete("AuthToken", cookieOptions);
	ctx.Response.Cookies.Delete("RefreshToken", cookieOptions);

	return Results.Ok(new UserLoginResponse
	{
		Success = true,
		UrlToRedirect = "/sso"
	});
});


app.Map("/api/auth/session/me", async (
	[FromServices] IRefreshTokenStore store,
	[FromServices] IApiConfigurationServiceClient apiConfigurationServiceClient,
	HttpContext ctx) =>
{
	var refresh = ctx.Request.Cookies["RefreshToken"];
	if (string.IsNullOrWhiteSpace(refresh))
	{
		return Results.Unauthorized();
	}

	var userId = await store.ValidateAndRotateAsync(refresh);
	if (userId == 0)
	{
		return Results.Unauthorized();
	}

	var user = UserStorage.Users.FirstOrDefault(x => x.Id == userId);
	if (user is null)
	{
		return Results.Unauthorized();
	}

	var result = await apiConfigurationServiceClient.GetAppUrl("dashboard", CancellationToken.None);

	return Results.Ok(new MyDataResponse
	{

		Email = user.Email,
		Name = user.Name
	});
});

app.MapGet("/api/auth/check", async (
	[FromServices] IRefreshTokenStore store,
	[FromServices] IApiConfigurationServiceClient apiConfigurationServiceClient,
	[FromHeader(Name = "X-User-Id")] int userId,
	HttpContext ctx) =>
{
	if (userId == 0)
	{
		return Results.Unauthorized();
	}

	var user = UserStorage.Users.FirstOrDefault(x => x.Id == userId);
	if (user is null)
	{
		return Results.Unauthorized();
	}

	var result = await apiConfigurationServiceClient.GetAppUrl("dashboard", CancellationToken.None);

	return Results.Ok(new UserLoginResponse
	{
		Success = true,
		UrlToRedirect = result?.Url
	});
});

app.Map("/api/auth/test", () => Results.Ok("Api Auth test"));

app.MapGet("/health", () => Results.Ok("AuthService OK"));
app.Run();
