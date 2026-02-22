namespace Mawo.Configuration.Api.Client.Contracts;

public sealed class RouteConfig
{
	public string? ClusterId { get; set; } = string.Empty;

	public RouteMatchConfig Match { get; set; } = new();

	public string? AuthorizationPolicy { get; set; }

	public string? RouteId { get; set; }

	public IReadOnlyDictionary<string, string>? Metadata { get; set; }
}

