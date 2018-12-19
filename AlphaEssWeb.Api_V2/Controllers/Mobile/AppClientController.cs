using AlphaEssWeb.Api_V2.Domain.Services;
using AlphaEssWeb.Api_V2.Model;
using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AlphaEssWeb.Api_V2.Controllers.Mobile
{
	public class AppClientController : ApiController
	{
		private readonly IAppClientService _appClientService;

		public AppClientController(IAppClientService appClientService)
		{
			_appClientService = appClientService;
		}

		/// <summary>
		/// 获取app最新版本信息
		/// </summary>
		/// <param name="etlacvrm"></param>
		/// <returns></returns>
		public HttpResponseMessage TheLastAppClientVersion(ExternalTheLastAppClientVersionRequestModel etlacvrm)
		{
			var appVersion = _appClientService.GetLastVersion(etlacvrm.Api_Account, etlacvrm.TimeStamp, etlacvrm.Sign, etlacvrm.AppType);
			return Request.CreateResponse(HttpStatusCode.OK, appVersion.ToExternalTheLastAppClientVersionResponseModel());
		}
	}
}
