using Mawo.Gateway.Service.MIddlewares;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Mawo.Gateway.Service.RequestTransforms;

public sealed class CorrelationIdTransformer : ITransformProvider
{
	public void Apply(TransformBuilderContext context)
	{
		// dodajemy transform do KAŻDEGO route’a
		context.AddRequestTransform(async transformContext =>
		{
			if (transformContext.HttpContext.Items.TryGetValue(
					CorrelationIdMiddleware.HeaderName, out var value))
			{
				transformContext.ProxyRequest.Headers.Remove(
					CorrelationIdMiddleware.HeaderName);

				transformContext.ProxyRequest.Headers.TryAddWithoutValidation(
					CorrelationIdMiddleware.HeaderName,
					value!.ToString());
			}
		});
	}

	public void ValidateCluster(TransformClusterValidationContext context)
	{
	}

	public void ValidateRoute(TransformRouteValidationContext context)
	{
	}
}
