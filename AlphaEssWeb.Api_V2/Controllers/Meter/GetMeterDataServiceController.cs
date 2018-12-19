using AlphaEssWeb.Api_V2.Domain.Services;
using AlphaEssWeb.Api_V2.Model;
using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace AlphaEssWeb.Api_V2.Controllers.Meter
{
	public class GetMeterDataServiceController : ApiController
	{
		private readonly IDevInfoService _devInfoService;
		private readonly IDevDataService _devDataService;

		public GetMeterDataServiceController(IDevInfoService devInfoService, IDevDataService devDataService)
		{
			_devInfoService = devInfoService;
			_devDataService = devDataService;
		}

		/// <summary>
		/// 获取设备信息
		/// </summary>
		/// <param name="egrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetDeviceInfo(ExternalGetDeviceInfoRequestModel egrm)
		{
			var ss = _devInfoService.GetDeviceInfo(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.DeviceID);
			var respContent = ss.ToExternalDeviceInfoResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}


		/// <summary>
		/// 获取电流数据
		/// </summary>
		/// <param name="egcvrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetDeviceCV(ExternalGetDeviceCVRequestModel egcvrm)
		{
			var ss = _devDataService.GetDeviceCV(egcvrm.Api_Account, egcvrm.TimeStamp, egcvrm.Sign, egcvrm.DeviceID, egcvrm.LocalDate);
			var respContent = ss.ToExternalDeviceCVResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

	}
}
