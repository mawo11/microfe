using Mawo.Configuration.Api.Client.Contracts;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/gateway-config", () =>
{
	var content = File.ReadAllText("gw_docker.json");

	var config = System.Text.Json.JsonSerializer.Deserialize<ReverseProxyConfig>(content);

	return TypedResults.Ok(config);
});

app.MapGet("/config/app/{appId}/link", ([FromRoute] string appId) =>
{
	return TypedResults.Ok(new AppIdResponse
	{
		Url = appId switch
		{
			"timeline" => "/timeline",
			"dashboard" => "/dashboard",
			"profile" => "/profile",
			_ => string.Empty
		},
	});

});

app.MapGet("/config/apps", () =>
{
	MenuItem[] items = [
		new MenuItem{
			 Name = "Dashboard",
			 Url="/dashboard/"
		},
		new MenuItem{
			 Name = "Timeline",
			 Url="/timeline/"
		},
		new MenuItem{
			 Name = "Profile",
			 Url="/profile/"
		},
		];

	return Results.Ok(new MenuResponse
	{
		Items = items
	});
});

app.Run();

