using Mawo.Configuration.Api.Client.Contracts;
using System.Net.Http.Json;

namespace Mawo.Configuration.Api.Client;

public sealed class ApiProxyServiceClient : IApiProxyServiceClient
{
	private static readonly ReverseProxyConfig Fail = new()
	{
		Routes = [],
		Clusters = []
	};

	private readonly HttpClient _httpClient;


	public ApiProxyServiceClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}


	public async ValueTask<ReverseProxyConfig> GetProxyConfigurationAsync(CancellationToken cancellationToken)
	{
		var result = await _httpClient.GetFromJsonAsync<ReverseProxyConfig>("/gateway-config", cancellationToken);

		return result ?? Fail;
	}
}
