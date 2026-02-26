using Mawo.Configuration.Api.Client;
using Mawo.Gateway.Service;
using Mawo.Gateway.Service.Proxy;
using Mawo.Gateway.Service.RequestTransforms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Transforms.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
	opt.AddPolicy("Frontends", p =>
		p.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? Array.Empty<string>())
		 .AllowAnyHeader()
		 .AllowAnyMethod()
		 .AllowCredentials());
});

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.Events = new JwtBearerEvents
		{
			OnMessageReceived = ctx =>
			{
				ctx.Token = ctx.Request.Cookies["AuthToken"];
				return Task.CompletedTask;
			}
		};

		opt.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true
		};

	});

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("RequireJwt", policy => policy.RequireAuthenticatedUser());

builder.Services.AddReverseProxy().LoadFromMemory([], []);

builder.Services.AddSingleton<ITransformProvider, CorrelationIdTransformer>();
builder.Services.AddSingleton<ITransformProvider, JwTokenTransformer>();
builder.Services.AddSingleton<ILoadBalancingPolicy, FeatureLoadBalancingPolicy>();
builder.Services.AddHttpClient<IApiProxyServiceClient, ApiProxyServiceClient>(client =>
{
	client.BaseAddress = new Uri(builder.Configuration["Endpoints:ApiConfigurationUrl"]!);
});
builder.Services.AddHostedService<ProxyConfigurationHostedService>();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();
app.MapFallback(context =>
{
	context.Response.Redirect("/sso");
	return Task.CompletedTask;
});

app.Map("/api/hello", () => "Mawo Gateway Service is running.");

app.Run();
// dobra dzisiaj zrobimy to i owo 