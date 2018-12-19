using AlphaEssWeb.Api_V2.Filters;
using AlphaEssWeb.Api_V2.Model.RequestCommands;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Validation;
using WebApiDoodle.Web.Filters;

namespace AlphaEssWeb.Api_V2.Config
{
	public class WebAPIConfig
	{
		public static void Configure(HttpConfiguration config)
		{
			//Formatters
			var jqueryFormatter = config.Formatters.FirstOrDefault(x => x.GetType() == typeof(JQueryMvcFormUrlEncodedFormatter));
			config.Formatters.Remove(config.Formatters.XmlFormatter);
			config.Formatters.Remove(config.Formatters.FormUrlEncodedFormatter);
			config.Formatters.Remove(jqueryFormatter);
			foreach (var formatter in config.Formatters)
			{
				formatter.RequiredMemberSelector = new SuppressedRequiredMemberSelector();
			}
			config.Formatters.JsonFormatter.SupportedEncodings.Add(System.Text.Encoding.GetEncoding("utf-8"));
			//config.Formatters.JsonFormatter.SupportedEncodings.Add(System.Text.Encoding.GetEncoding("gb2312"));
			// Message Handlers
			//config.MessageHandlers.Add(new RequireHttpsMessageHandler());
			//config.MessageHandlers.Add(new AlphaEssWebAuthHandler());
			//config.MessageHandlers.Add(new AlphaEssMessageHandler());

			//Default Services
			config.Services.Replace(typeof(IContentNegotiator),new DefaultContentNegotiator(excludeMatchOnTypeOnly: true));
			config.Services.RemoveAll(typeof(ModelValidatorProvider),validator => !(validator is System.Web.Http.Validation.Providers.DataAnnotationsModelValidatorProvider));

			config.ParameterBindingRules.Insert(0,descriptor => typeof(IRequestCommand).IsAssignableFrom(descriptor.ParameterType) ? new FromUriAttribute().GetBinding(descriptor) : null);

			// Filters
			config.Filters.Add(new InvalidModelStateFilterAttribute());
			config.Filters.Add(new HandleErrorAttribute());
		}
	}
}
