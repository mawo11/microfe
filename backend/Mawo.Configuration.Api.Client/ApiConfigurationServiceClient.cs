using Mawo.Configuration.Api.Client.Contracts;
using System.Net.Http.Json;

namespace Mawo.Configuration.Api.Client;

public sealed class ApiConfigurationServiceClient : IApiConfigurationServiceClient
{
	private readonly HttpClient _httpClient;


	public ApiConfigurationServiceClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}


	public async ValueTask<AppIdResponse?> GetAppUrl(string appId, CancellationToken cancellationToken)
	{
		var result = await _httpClient.GetFromJsonAsync<AppIdResponse>($"/config/app/{appId}/link", cancellationToken);

		return result;
	}
}
