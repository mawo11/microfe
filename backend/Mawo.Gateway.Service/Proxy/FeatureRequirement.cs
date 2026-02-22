using Microsoft.AspNetCore.Authorization;

namespace Mawo.Gateway.Service.Proxy;

public sealed class FeatureRequirement : IAuthorizationRequirement
{
	public FeatureRequirement(string featureName) => FeatureName = featureName;

	public string FeatureName { get; }
}
