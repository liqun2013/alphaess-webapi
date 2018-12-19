using AlphaEssWeb.Api_V2.Domain.Services;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using WebApiDoodle.Web.MessageHandlers;

namespace AlphaEssWeb.Api_V2
{
	internal static class HttpRequestMessageExtensions
	{
		private static TService GetService<TService>(this HttpRequestMessage request)
		{
			IDependencyScope dependencyScope = request.GetDependencyScope();
			TService service = (TService)dependencyScope.GetService(typeof(TService));

			return service;
		}
	}
}
