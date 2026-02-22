using System.Security.Claims;
using Yarp.ReverseProxy.LoadBalancing;
using Yarp.ReverseProxy.Model;

namespace Mawo.Gateway.Service.Proxy;

public sealed class FeatureLoadBalancingPolicy : ILoadBalancingPolicy
{
	public string Name => "JwtFeature";

	public DestinationState? PickDestination(HttpContext context, ClusterState cluster, IReadOnlyList<DestinationState> availableDestinations)
	{
		// availableDestinations zawiera np. v1 i v2, które są zdrowe

		var user = context.User;
		if (user?.Identity?.IsAuthenticated != true)
		{
			// jak brak auth, default
			return availableDestinations.FirstOrDefault();
		}

		bool useV2 = HasFeature(user, "UseV2");

		// Szukamy konkretnych destination po nazwie
		var targetId = useV2 ? "v2" : "v1";

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
