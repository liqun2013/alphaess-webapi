using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain.Services;
using AlphaEssWeb.Api_V2.Model;
using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AlphaEssWeb.Api_V2.Controllers.Extranet
{
	/// <summary>
	/// 微网控制接口
	/// </summary>
	public class MicrogridController : ApiController
	{
		private readonly IMicrogridService _microgridService;
		private readonly IMembershipService _membershipService;
		private readonly ICryptoService _cryptoService;

		public MicrogridController(IMicrogridService microgridService, IMembershipService membershipService, ICryptoService cryptoService)
		{
			_microgridService = microgridService;
			_membershipService = membershipService;
			_cryptoService = cryptoService;
		}

		/// <summary>
		/// 登入
		/// </summary>
		/// <param name="elrm"></param>
		/// <returns></returns>
		public HttpResponseMessage Login(ExternalLoginRequestModel elrm)
		{
			var or = _membershipService.Login(elrm.Api_Account, elrm.TimeStamp, elrm.Sign, elrm.Username, elrm.Password);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalLoginResponseModel());
		}

		/// <summary>
		/// 获取微网信息
		/// </summary>
		/// <param name="requestModel"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetMicrogridInfo(ExternalGetMicrogridInfoRequestModel requestModel)
		{
			var r = _microgridService.GetMicrogridInfoByUserId(requestModel.Api_Account, requestModel.TimeStamp, requestModel.Sign, requestModel.Username);
			return Request.CreateResponse(HttpStatusCode.OK, r.ToExternalGetMicrogridInfoResponseModel());
		}

		/// <summary>
		/// 获取微网调度策略
		/// </summary>
		/// <param name="requestModel"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetMicrogridSchedulingStrategy(ExternalGetMicrogridSchedulingStrategyRequestModel requestModel)
		{
			var r = _microgridService.GetSchedulingStrategyByMicrogridId(requestModel.Api_Account, requestModel.TimeStamp, requestModel.Sign, requestModel.MicrogridId);
			return Request.CreateResponse(HttpStatusCode.OK, r.ToExternalGetMicrogridSchedulingStrategyResponseModel());
		}

		/// <summary>
		/// 修改微网调度策略
		/// </summary>
		/// <param name="requestModel"></param>
		/// <returns></returns>
		public HttpResponseMessage UpdateSchedulingStrategy(ExternalUpdateSchedulingStrategyRequestModel requestModel)
		{
			var r = _microgridService.UpdateSchedulingStrategy(requestModel.Api_Account, requestModel.TimeStamp, requestModel.Sign, requestModel.MicrogridId, requestModel.PGridMax, requestModel.POutputMax, requestModel.DieselStartSOC, requestModel.DieselPOutputMax,
				requestModel.DieselStopSOC, requestModel.DieselStopPower, requestModel.SOC1, requestModel.SOC2, requestModel.SOC3, requestModel.SOC4, requestModel.SOC5, requestModel.SOC6, requestModel.SOC7, requestModel.Power1, requestModel.Power2, requestModel.Power3,
				requestModel.Power4, requestModel.Power5, requestModel.ChargingStart1, requestModel.ChargingEnd1, requestModel.ChargingStart2, requestModel.ChargingEnd2, requestModel.DischargeStart1, requestModel.DischargeEnd1, requestModel.DischargeStart2, requestModel.DischargeEnd2,
				requestModel.ChargingSOCPoint1, requestModel.ChargingSOCPoint2, requestModel.DischargeSOCPoint1, requestModel.DischargeSOCPoint2);
			return Request.CreateResponse(HttpStatusCode.OK, r.ToExternalUpdateSchedulingStrategyResponseModel());
		}

		/// <summary>
		/// 微网控制指令
		/// </summary>
		/// <param name="requestModel"></param>
		/// <returns></returns>
		public HttpResponseMessage MicrogridControlCommand(ExternalMicrogridControlCommandRequestModel requestModel)
		{
			var r = _microgridService.ChangeMicrogridStateCommand(requestModel.Api_Account, requestModel.TimeStamp, requestModel.Sign, requestModel.MicrogridId, requestModel.Command);
			return Request.CreateResponse(HttpStatusCode.OK, r.ToExternalMicrogridControlCommandResponseModel());
		}

		/// <summary>
		/// 获取微网汇总信息
		/// </summary>
		/// <param name="requestModel"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetMicrogridSummary(ExternalGetMicrogridSummaryRequestModel requestModel)
		{
			var r = _microgridService.GetMicrogridSummary(requestModel.Api_Account, requestModel.TimeStamp, requestModel.Sign, requestModel.MicrogridId);
			return Request.CreateResponse(HttpStatusCode.OK, r.ToExternalGetMicrogridSummaryResponseModel());
		}

		/// <summary>
		/// 发送电力调度信息
		/// </summary>
		/// <param name="requestModel"></param>
		/// <returns></returns>
		public HttpResponseMessage ElectricDispatchingControlRecord(ExternalElectricDispatchingControlRecordRequestModel requestModel)
		{
			var r = _microgridService.SendPowerDispatchingCommand(requestModel.Api_Account, requestModel.TimeStamp, requestModel.Sign, requestModel.MicrogridId, requestModel.CmdIndex, requestModel.ControlPower);
			return Request.CreateResponse(HttpStatusCode.OK, r.ToExternalElectricDispatchingControlRecordResponseModel());
		}
	}
}
