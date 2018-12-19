using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Domain.Services;
using AlphaEssWeb.Api_V2.Model;
using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace AlphaEssWeb.Api_V2.Controllers.ras
{
	public class v2Controller : ApiController
	{
		private readonly IAlphaESSV2Service _alphaESSV2Service;
		private readonly IReportInfoService _reportInfoService;
		private readonly IRunningDataService _runningDataService;
		private readonly IPowerDataService _powerDataService;
		private readonly IRemoteDispatchService _remoteDispatchService;

		#region 取客户端真实IP

		///  <summary>  
		///  取得客户端真实IP。如果有代理则取第一个非内网地址  
		///  </summary>  
		public static string GetIPAddress
		{
			get
			{
				var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (!string.IsNullOrEmpty(result))
				{
					//可能有代理  
					if (result.IndexOf(".") == -1)        //没有“.”肯定是非IPv4格式  
						result = null;
					else
					{
						if (result.IndexOf(",") != -1)
						{
							//有“,”，估计多个代理。取第一个不是内网的IP。  
							result = result.Replace("  ", "").Replace("'", "");
							string[] temparyip = result.Split(",;".ToCharArray());
							for (int i = 0; i < temparyip.Length; i++)
							{
								if (IsIPAddress(temparyip[i])
												&& temparyip[i].Substring(0, 3) != "10."
												&& temparyip[i].Substring(0, 7) != "192.168"
												&& temparyip[i].Substring(0, 7) != "172.16.")
								{
									return temparyip[i];        //找到不是内网的地址  
								}
							}
						}
						else if (IsIPAddress(result))  //代理即是IP格式  
							return result;
						else
							result = null;        //代理中的内容  非IP，取IP  
					}

				}

				string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

				if (string.IsNullOrEmpty(result))
					result = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

				if (string.IsNullOrEmpty(result))
					result = HttpContext.Current.Request.UserHostAddress;

				return result;
			}
		}

		private static bool IsIPAddress(string str1)
		{
			if (string.IsNullOrEmpty(str1) || str1.Length < 7 || str1.Length > 15) return false;

			const string regFormat = @"^d{1,3}[.]d{1,3}[.]d{1,3}[.]d{1,3}$";

			var regex = new System.Text.RegularExpressions.Regex(regFormat, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			return regex.IsMatch(str1);
		}

		#endregion

		public v2Controller(IAlphaESSV2Service alphaESSV2Service, IReportInfoService reportInfoService, IRunningDataService runningDataService, IPowerDataService powerDataService, IRemoteDispatchService remoteDispatchService)
		{
			_alphaESSV2Service = alphaESSV2Service;
			_reportInfoService = reportInfoService;
			_runningDataService = runningDataService;
			_powerDataService = powerDataService;
			_remoteDispatchService = remoteDispatchService;
		}

		/// <summary>
		/// 用户登入
		/// </summary>
		/// <param name="elrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage Login(Externalv2LoginRequestModel elrm)
		{
			var or = _alphaESSV2Service.LoginForUser(elrm.Api_Account, elrm.TimeStamp, elrm.Sign, elrm.Username, elrm.Password, GetIPAddress);

			var respContent = or.ToExternalLoginUserResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 获取系统列表 
		/// </summary>
		/// <param name="egrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetSystemList(Externalv2GetSystemListRequestModel egrm)
		{
			var or = _alphaESSV2Service.GetSystemListForUser(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Token, egrm.PageIndex, egrm.PageSize, GetIPAddress);

			var respContent = or.ToExternalSystemListResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}



		/// <summary>
		/// 获取系统能量信息 
		/// </summary>
		/// <param name="egrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetEnergySummary(Externalv2GetEnergySummaryRequestModel egrm)
		{
			var or = _reportInfoService.GetEnergySummary(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Sn, egrm.TheDate, egrm.Token, GetIPAddress);

			var respContent = or.ToExternaReportEnergyResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}


		/// <summary>
		/// 获取最新的系统常规运行数据 
		/// </summary>
		/// <param name="egrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetRunningData(Externalv2GetRunningDataRequestModel egrm)
		{
			var or = _runningDataService.GetRunningNewData(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Sn, egrm.Token, GetIPAddress);

			var respContent = or.ToExternaRunningDataResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}


		/// <summary>
		/// 获取历史的系统常规运行数据  
		/// </summary>
		/// <param name="egrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetHistoryRunningData(Externalv2GetHistoryRunningDataRequestModel egrm)
		{
			var or = _runningDataService.GetHistoryRunningData(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Sn, egrm.Token, egrm.Starttime, egrm.endtime, GetIPAddress);

			var respContent = or.ToExternaRunningDataResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}


		/// <summary>
		/// 获取实时功率数据  
		/// </summary>
		/// <param name="egrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetLastPowerData(Externalv2GetLastPowerDataRequestModel egrm)
		{
			var or = _powerDataService.GetLastPowerData(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Sn, egrm.Token, GetIPAddress);

			var respContent = or.ToExternaLastPowerDataResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}


		/// <summary>
		/// 发送系统控制指令
		/// </summary>
		/// <param name="egrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage RemoteDispatch(Externalv2RemoteDispatchRequestModel egrm)
		{
			var or = _remoteDispatchService.AddRemoteDispatch(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Token, egrm.SN, egrm.ActivePower, egrm.ReactivePower, egrm.SOC, egrm.Status, egrm.ControlMode, GetIPAddress);

			var respContent = or.ToExternaRemoteDispatchResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}
	}
}
