using AlphaEssWeb.Api_V2.Config;
using System;
using System.Web.Http;

namespace AlphaEssWeb.Api_V2.WebHost
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			//GlobalConfiguration.Configure(WebApiConfig.Register);
			var config = GlobalConfiguration.Configuration;
			RouteConfig.RegisterRoutes(config);
			WebAPIConfig.Configure(config);
			AutofacWebAPI.Initialize(config);
			TraceConfig.Register(config);
		}

	}
}
