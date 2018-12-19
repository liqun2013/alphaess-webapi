using System;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace AlphaEssWeb.Api_V2.Config
{
	public class TraceConfig
	{
		public static void Register(HttpConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			var traceWriter = new NLogTraceWriter();
			configuration.Services.Replace(typeof(ITraceWriter), traceWriter);
		}
	}
}
