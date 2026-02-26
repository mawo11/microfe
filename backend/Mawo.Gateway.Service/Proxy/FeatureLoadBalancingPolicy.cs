using System.Security.Claims;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Mawo.Gateway.Service.Proxy;

public sealed class FeatureLoadBalancingPolicy : ILoadBalancingPolicy
{
	public string Name => "FeatureLoadBalancingPolicy";

	public DestinationState? PickDestination(HttpContext context, ClusterState cluster, IReadOnlyList<DestinationState> availableDestinations)
	{
		var user = context.User;
		if (user?.Identity?.IsAuthenticated != true)
		{
			return availableDestinations.FirstOrDefault();
		}

		bool useV2 = HasFeature(user, "TimelineV2");

		var targetId = useV2 ? "TimelineV2" : "default";

		var chosen = availableDestinations.FirstOrDefault(d =>
			string.Equals(d.DestinationId, targetId, StringComparison.OrdinalIgnoreCase));

		return chosen ?? availableDestinations.FirstOrDefault();
	}

	private static bool HasFeature(ClaimsPrincipal user, string featureName)
	{
		// przykład: wiele claimów "feature"
		return user.Claims.Where(c => c.Type == "feature")
			.Any(c => string.Equals(c.Value, featureName, StringComparison.OrdinalIgnoreCase));
	}
}
