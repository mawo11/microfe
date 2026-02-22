using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Mawo.Gateway.Service.Proxy;

public sealed class FeaturePolicyProvider : DefaultAuthorizationPolicyProvider
{
	public FeaturePolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

	public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		if (policyName.StartsWith("Feature:", StringComparison.OrdinalIgnoreCase))
		{
			var feature = policyName["Feature:".Length..];

			var policy = new AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser()
				.AddRequirements(new FeatureRequirement(feature))
				.Build();

			return Task.FromResult<AuthorizationPolicy?>(policy);
		}

		return base.GetPolicyAsync(policyName);
	}
}
