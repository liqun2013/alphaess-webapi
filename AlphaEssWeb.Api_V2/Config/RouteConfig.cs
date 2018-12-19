using System.Web.Http;

namespace AlphaEssWeb.Api_V2.Config
{
	public class RouteConfig
	{
		public static void RegisterRoutes(HttpConfiguration config)
		{
			var routes = config.Routes;

			routes.MapHttpRoute("rasroute", "ras/{controller}/{action}/", new { controller = "v2" });
			routes.MapHttpRoute("extranetroute", "extranet/{controller}/{action}/", new { });

			routes.MapHttpRoute("meterroute", "meter/{controller}/{action}/", new { });

			routes.MapHttpRoute("mobileroute", "mobile/{controller}/{action}/", new { });
			//routes.MapHttpRoute("rasroute", "ras/{controller}/{action}/", new { });


			routes.MapHttpRoute("DefaultHttpRoute", "api/{controller}/{id}");
		}
	}
}
