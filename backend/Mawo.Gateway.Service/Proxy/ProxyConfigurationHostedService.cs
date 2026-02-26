using Mawo.Configuration.Api.Client;
using Yarp.ReverseProxy.Configuration;

namespace Mawo.Gateway.Service.Proxy;

public sealed partial class ProxyConfigurationHostedService : BackgroundService
{
	private readonly IApiProxyServiceClient _apiProxyServiceClient;
	private readonly ILogger<ProxyConfigurationHostedService> _logger;
	private readonly InMemoryConfigProvider _configProvider;

	public ProxyConfigurationHostedService(
		IApiProxyServiceClient apiProxyServiceClient,
		ILogger<ProxyConfigurationHostedService> logger,
		InMemoryConfigProvider configProvider)
	{
		_apiProxyServiceClient = apiProxyServiceClient;
		_logger = logger;
		_configProvider = configProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			LogCall(_logger);

			var config = await _apiProxyServiceClient.GetProxyConfigurationAsync(stoppingToken);
			if (config is not null)
			{
				IReadOnlyList<RouteConfig> routes = [.. config.Routes!
					.Select(x => new RouteConfig
					{
						ClusterId = x.ClusterId!,
						RouteId = x.RouteId!,
						Match = new RouteMatch
						{
							Path = x.Match.Path,
						},
						AuthorizationPolicy = x.AuthorizationPolicy,
						Metadata = x.Metadata,

					})];
				IReadOnlyList<ClusterConfig> clusters = [.. config.Clusters!
					.Select(x => new ClusterConfig
					{
						ClusterId = x.ClusterId!,
						LoadBalancingPolicy= "FeatureLoadBalancingPolicy",
						Destinations = x.Destinations
						 .ToDictionary(x=>x.Key, x=> new DestinationConfig
						 {
							  Address =x.Value.Address
						 })
					})];

				_configProvider.Update(routes, clusters);
			}

			await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
		}
	}

	[LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Attemp to load proxy configuration")]
	private static partial void LogCall(ILogger<ProxyConfigurationHostedService> logger);
}
