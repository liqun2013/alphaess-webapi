using AlphaEssWeb.Api_V2.Domain.Services;
using AlphaEssWeb.Api_V2.Model;
using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AlphaEssWeb.Api_V2.Controllers.Mobile
{
	public class SystemsController:ApiController
	{
		private readonly ISystemService _systemService;

		public SystemsController(ISystemService systemService)
		{
			_systemService = systemService;
		}

		/// <summary>
		/// 根据用户获取系统信息
		/// </summary>
		/// <param name="egsburm"></param>
		/// <returns></returns>
		public HttpResponseMessage QuerySystemByUser(ExternalQuerySystemByUserRequestModel egsburm)
		{
			var sys = _systemService.GetSystemByUser(egsburm.Api_Account, egsburm.TimeStamp, egsburm.Sign, egsburm.PageIndex, egsburm.PageSize, egsburm.Username);
			var respContent = sys.ToExternalQuerySystemByUserResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 获取系统状态信息
		/// </summary>
		/// <param name="egssrm"></param>
		/// <returns></returns>
		public HttpResponseMessage QuerySystemStatus(ExternalQuerySystemStatusRequestModel egssrm)
		{
			var ss = _systemService.GetSystemStatus(egssrm.Api_Account, egssrm.TimeStamp, egssrm.Sign, egssrm.Sn);
			var respContent = ss.ToExternalQuerySystemStatusResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 获取设备功率数据
		/// </summary>
		/// <param name="egpdrm"></param>
		/// <returns></returns>
		public HttpResponseMessage QueryPowerData(ExternalQueryPowerDataRequestModel egpdrm)
		{
			var pd = _systemService.GetPowerData(egpdrm.Api_Account, egpdrm.TimeStamp, egpdrm.Sign, egpdrm.Sn, egpdrm.Username, egpdrm.Date);
			var respContent = pd.ToExternalQueryPowerDataResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 获取设备能量数据
		/// </summary>
		/// <param name="egedrm"></param>
		/// <returns></returns>
		public HttpResponseMessage QueryEnergeData(ExternalQueryEnergeDataRequestModel egedrm)
		{
			var ed = _systemService.GetEnergeData(egedrm.Api_Account, egedrm.TimeStamp, egedrm.Sign, egedrm.Sn, egedrm.Username, egedrm.Date_Start, egedrm.Date_End);
			var respContent = ed.ToExternalQueryEnergeDataResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}
	}
}
