namespace Mawo.Configuration.Api.Client.Contracts;

public class ClusterConfig
{
	public string? ClusterId { get; set; }

	public Dictionary<string, DestinationConfig> Destinations { get; set; } = new();
}
