using Mawo.Configuration.Api.Client.Contracts;

namespace Mawo.Configuration.Api.Client;

public interface IApiProxyServiceClient
{
	ValueTask<ReverseProxyConfig> GetProxyConfigurationAsync(CancellationToken cancellationToken);
}