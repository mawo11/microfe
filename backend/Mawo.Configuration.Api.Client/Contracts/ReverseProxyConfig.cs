namespace Mawo.Configuration.Api.Client.Contracts;

public sealed class ReverseProxyConfig
{
	public ClusterConfig[]? Clusters { get; set; }

	public RouteConfig[]? Routes { get; set; }
}
