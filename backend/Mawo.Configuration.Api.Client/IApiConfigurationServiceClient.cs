using Mawo.Configuration.Api.Client.Contracts;

namespace Mawo.Configuration.Api.Client
{
	public interface IApiConfigurationServiceClient
	{
		ValueTask<AppIdResponse?> GetAppUrl(string appId, CancellationToken cancellationToken);
	}
}