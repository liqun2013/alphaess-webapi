using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Domain.Services;
using AlphaEssWeb.Api_V2.Model;
using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using AlphaEssWeb.Api_V2.Model.ExternalResponseModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AlphaEssWeb.Api_V2.Controllers
{
	public class V2Controller : ApiController
	{
		private readonly IMembershipService _membershipService;
		private readonly ISystemService _systemService;
		private readonly IAppClientService _appClientSvc;
		private readonly IComplaintsService _complaintSvc;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly IApiService _apiService;
		private readonly ISysMsgService _msgService;
		private readonly ICompanyContactDetailService _companyContactDetailService;

		public V2Controller(IMembershipService membershipService, ISystemService systemService, IAppClientService appClientSvc, IComplaintsService complaintSvc,
			ISysMsgService msgService, IParameterValidateService parameterValidateService, IApiService apiService, ICompanyContactDetailService companyContactDetailService)
		{
			_companyContactDetailService = companyContactDetailService;
			_msgService = msgService;
			_complaintSvc = complaintSvc;
			_membershipService = membershipService;
			_systemService = systemService;
			_appClientSvc = appClientSvc;
			_parameterValidateService = parameterValidateService;
			_apiService = apiService;
		}

		/// <summary>
		/// 用户登入
		/// </summary>
		/// <param name="elrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage Login(Externalv2LoginRequestModel elrm)
		{
			var or = _membershipService.Login(elrm.Api_Account, elrm.TimeStamp, elrm.Sign, elrm.Username, elrm.Password);

			var respContent = or.ToExternalLoginResponseModel();
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
			var or = _systemService.GetSystemByUser(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Token, egrm.PageIndex, egrm.PageSize);

			var respContent = or.ToExternalQuerySystemByUserResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 获取设备功率数据
		/// </summary>
		/// <param name="egpdrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetPowerData(ExternalGetPowerDataRequestModel egpdrm)
		{
			var pd = _systemService.GetPowerData(egpdrm.Api_Account, egpdrm.TimeStamp, egpdrm.Sign, egpdrm.Sn, egpdrm.Username, egpdrm.Date, egpdrm.Token);
			var respContent = pd.ToExternalQueryPowerDataResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 获取设备能量数据
		/// </summary>
		/// <param name="egedrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetEnergeData(ExternalGetEnergeDataRequestModel egedrm)
		{
			var ed = _systemService.GetEnergeData(egedrm.Api_Account, egedrm.TimeStamp, egedrm.Sign, egedrm.Sn, egedrm.Username, egedrm.Date_Start, egedrm.Date_End, egedrm.StatisticsBy, egedrm.Token);
			var respContent = ed.ToExternalGetEnergeDataResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		[HttpPost]
		public HttpResponseMessage GetProfitData(ExternalGetProfitDataRequestModel req)
		{
			var pd = _systemService.GetProfitData(req.Api_Account, req.TimeStamp, req.Sign, req.Sn, req.Username, req.Date_Start, req.Date_End, req.StatisticsBy, req.Token);
			var respContent = pd.ToExternalGetProfitDataResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		[HttpPost]
		public HttpResponseMessage GetSystemStatus(ExternalGetSystemStatusRequestModel req)
		{
			var sts = _systemService.GetSystemStatus(req.Api_Account, req.TimeStamp, req.Sign, req.Sn, req.Token);
			return Request.CreateResponse(HttpStatusCode.OK, sts.ToExternalGetSystemStatusResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage GetCompanyContacts(ExternalGetCompanyContactsRequestModel req)
		{
			var or = _companyContactDetailService.GetCompanyContacts(req.Api_Account, req.TimeStamp, req.Sign, req.Flag);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalGetCompanyContactsResponseModel());
		}

		/// <summary>
		/// 获取系统能量信息 
		/// </summary>
		/// <param name="egrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetEnergySummary(Externalv2GetEnergySummaryRequestModel egrm)
		{
			var or = _systemService.GetEnergyReportData(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Sn, egrm.TheDate, egrm.Token);

			var respContent = or.ToExternalGetEnergySummaryResponseModel();
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
			var or = _systemService.GetRunningNewData(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Sn, egrm.Token);

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
			var or = _systemService.GetHistoryRunningData(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Sn, egrm.Token, egrm.Starttime, egrm.endtime);

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
			var or = _systemService.GetLastPowerData(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Sn, egrm.Token);

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
			var or = _systemService.AddRemoteDispatch(egrm.Api_Account, egrm.TimeStamp, egrm.Sign, egrm.Token, egrm.SN, egrm.ActivePower, egrm.ReactivePower, egrm.SOC, egrm.Status, egrm.ControlMode);

			var respContent = or.ToExternaRemoteDispatchResponseModel();
			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 获取最新版本
		/// </summary>
		/// <param name="etlacvrm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetTheLastAppClientVersion(ExternalTheLastAppClientVersionRequestModel etlacvrm)
		{
			var appVersion = _appClientSvc.GetLastVersion(etlacvrm.Api_Account, etlacvrm.TimeStamp, etlacvrm.Sign, etlacvrm.AppType);
			return Request.CreateResponse(HttpStatusCode.OK, appVersion.ToExternalTheLastAppClientVersionResponseModel());
		}

		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="errm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage Register(ExternalRegisterRequestModel errm)
		{
			var or = _membershipService.Register(errm.Api_Account, errm.TimeStamp, errm.Sign, errm.Sn, errm.License_No, errm.Username, errm.Password, errm.Email, errm.Allow_AutoUpdate, errm.PostCode, errm.Country);
			var respContent = or.ToExternalRegisterResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 用户协议
		/// </summary>
		/// <param name="eguarm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetUserAgreement(ExternalGetUserAgreementRequestModel eguarm)
		{
			var or = _membershipService.GetUserAgreementByLanguageCode(eguarm.Api_Account, eguarm.TimeStamp, eguarm.Sign, eguarm.Language_Code);
			var respContent = or.ToExternalGetUserAgreementByLanguageCodeResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 获取用户信息
		/// </summary>
		/// <param name="eurm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage GetUserInfo(ExternalUserInfoRequestModel eurm)
		{
			var or = _membershipService.GetUser(eurm.Api_Account, eurm.TimeStamp, eurm.Sign, eurm.Username, eurm.Token);
			var respContent = or.ToExternalUserinfoResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 修改用户信息
		/// </summary>
		/// <param name="ecurm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage UpdateUserInfo(ExternalChangeUserInfoRequestModel ecurm)
		{
			var or = _membershipService.UpdateUserinfo(ecurm.Api_Account, ecurm.TimeStamp, ecurm.Sign, ecurm.Username, ecurm.Language_Code, ecurm.Country,
				ecurm.State, ecurm.City, ecurm.Zipcode, ecurm.Contact_User, ecurm.Email, ecurm.Allow_AutoUpdate, ecurm.CellPhone, ecurm.Address, ecurm.Token);

			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalChangeUserInfoResponseModel());
		}

		/// <summary>
		/// 找回密码
		/// </summary>
		/// <param name="erprm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage RetrievePwd(ExternalRetrievePwdRequestModel erprm)
		{
			var or = _membershipService.RetrievePwd(erprm.Api_Account, erprm.TimeStamp, erprm.Sign, erprm.Username, erprm.Email);

			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalRetrievePwdResponseModel());
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="ecprm"></param>
		/// <returns></returns>
		[HttpPost]
		public HttpResponseMessage ChangePassword(ExternalChangePwdRequestModel ecprm)
		{
			var or = _membershipService.ChangePassword(ecprm.Api_Account, ecprm.TimeStamp, ecprm.Sign, ecprm.Username, ecprm.OldPwd, ecprm.NewPwd, ecprm.Token);

			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalChangePasswordResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage GetSystemDetail(ExternalGetSystemDetailRequestModel req)
		{
			var s = _systemService.GetSystemDetail(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.Sn);

			return Request.CreateResponse(HttpStatusCode.OK, s.ToExternalGetSystemDetailResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage UpdateSystemConfig(ExternalSetSystemRequestModel req)
		{
			var s = new VT_SYSTEM();
			s.CloneFromVTSYSTEMDto(req);
			var or = _systemService.UpdateVtSystem(req.Api_Account, req.TimeStamp, req.Sign, req.Token, s);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalSetSystemResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage AdditionalSystem(ExternalBindNewSystemRequestModel req)
		{
			var or = _systemService.BindNewSystem(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.NewSn, req.License_no, req.Username, req.CheckCode);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalBindNewSystemResponseModel());
		}

		public HttpResponseMessage InstallNewSystem(ExternalInstallNewSystemRequestModel req)
		{
			var or = _systemService.InstallNewSystem(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.NewSN, req.License_no, req.CheckCode, req.InstallationDate, req.CustomerName, req.ContactNumber, req.ContactAddress);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalInstallNewSystemResponseModel());
		}

		public HttpResponseMessage AddNewComplaints(ExternalAddNewComplaintRequestModel req)
		{
			var or = _complaintSvc.AddNewComplaints(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.Title, req.Description, req.ComplaintsType, req.Email, req.ContactNumber, req.SysSn, req.Attachment1, req.Attachment2, req.Attachment3);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalAddNewComplaintsResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage GetComplaintsList(ExternalGetComplaintsListRequestModel req)
		{
			var or = _complaintSvc.GetComplaintsList(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.PageIndex, req.PageSize);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalGetComplaintsListResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage EvaluateComplaints(ExternalEvaluateComplaintsRequestModel req)
		{
			var or = _complaintSvc.EvaluateComplaints(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.ComplaintsId, req.Satisfaction, req.Satisfaction1, req.Satisfaction2, req.Content);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalEvaluateComplaintsResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage GetMsgList(ExternalGetMsgReauestModel req)
		{
			var or = _msgService.GetMsg(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.OnlyUnread, req.PageIndex, req.PageSize);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalGetMsgResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage UpdateMsgFlag(ExternalUpdateMsgFlagRequestModel req)
		{
			var or = _msgService.UpdateMsgFlag(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.Flag, req.MsgId);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalUpdateMsgFlagResponseModel());
		}

		[HttpPost]
		public HttpResponseMessage GetFirmwareUpdate(ExternalGetFirmwareUpdateRequestModel req)
		{
			var or = _systemService.GetFirmwareUpdate(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.Sn);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalGetFirmwareUpdateResponsModel());
		}

		[HttpPost]
		public HttpResponseMessage GetSystemSummaryStatisticsData(ExternalGetSystemSummaryStatisticsDataRequestModel req)
		{
			var or = _systemService.GetSystemSummaryStatisticsData(req.Api_Account, req.TimeStamp, req.Sign, req.Token);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalGetSystemSummaryStatisticsDataResponsModel());
		}
		public HttpResponseMessage UpdateSystemFirmware(ExternalUpdateSystemFirmwareRequestModel req)
		{
			var or = _systemService.UpdateSystemFirmware(req.Api_Account, req.TimeStamp, req.Sign, req.Token, req.Sn, req.Category);
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalUpdateSystemFirmwareResponseModel());
		}

		#region 文件上传
		public async Task<HttpResponseMessage> FileUpload(/*ExternalFileUploadRequestModel req*/)
		{
			var token = HttpContext.Current.Request.Params["Token"];
			var timestamp = Convert.ToInt64(HttpContext.Current.Request.Params["timestamp"]);
			var sign = HttpContext.Current.Request.Params["sign"];
			var api_account = HttpContext.Current.Request.Params["api_account"];

			if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.Params["Token"]) || 
				string.IsNullOrWhiteSpace(HttpContext.Current.Request.Params["timestamp"]) ||
				string.IsNullOrWhiteSpace(HttpContext.Current.Request.Params["sign"]) || 
				string.IsNullOrWhiteSpace(HttpContext.Current.Request.Params["api_account"]))
				return Request.CreateResponse(HttpStatusCode.OK, new ExternalFileUploadResponseModel { ReturnCode = (int)OperationCode.Error_Param_Empty });

			if (string.IsNullOrWhiteSpace(token))
				return Request.CreateResponse(HttpStatusCode.OK, new ExternalFileUploadResponseModel { ReturnCode = (int)OperationCode.Error_Param_Empty });

			if (!_parameterValidateService.CheckTimestamp(timestamp))
				return Request.CreateResponse(HttpStatusCode.OK, new ExternalFileUploadResponseModel { ReturnCode = (int)OperationCode.Error_TimeStamp });

			if (!TokenHelper.CheckToken(token))
				return Request.CreateResponse(HttpStatusCode.OK, new ExternalFileUploadResponseModel { ReturnCode = (int)OperationCode.Error_TokenExpiration });

			var apiAccount = _apiService.GetByAccount(api_account);
			if (apiAccount == null)
				return Request.CreateResponse(HttpStatusCode.OK, new ExternalFileUploadResponseModel { ReturnCode = (int)OperationCode.Error_ApiAccountNotExist });

			// Check whether the POST operation is MultiPart?
			if (!Request.Content.IsMimeMultipartContent())
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

			// Prepare CustomMultipartFormDataStreamProvider in which our multipart form
			// data will be loaded.
			string fileSaveLocation = System.Configuration.ConfigurationManager.AppSettings["filesavelocation"] + DateTime.Now.ToString("yyyyMM");
			string fileSaveLogicalLocation = "~/App_Data/" + DateTime.Now.ToString("yyyyMM");
			Directory.CreateDirectory(fileSaveLocation);
			AlphaEssMultipartFormDataStreamProvider provider = new AlphaEssMultipartFormDataStreamProvider(fileSaveLocation);
			//List<string> files = new List<string>();

			if (Request.Content.Headers.ContentLength > 10485760)
				return Request.CreateResponse(HttpStatusCode.OK, new ExternalFileUploadResponseModel() { ReturnCode = (int)OperationCode.Error_FileIsTooLarge });

			try
			{
				// Read all contents of multipart message into CustomMultipartFormDataStreamProvider.
				await Request.Content.ReadAsMultipartAsync(provider);

				//foreach (MultipartFileData file in provider.FileData)
				//{
				var result = new ExternalFileUploadResponseModel() { ReturnCode = (int)OperationCode.Success };
				result.FilePath = Path.Combine(fileSaveLogicalLocation, Path.GetFileName(provider.FileData[0].LocalFileName));
				//}

				return Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (Exception exc)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new ExternalFileUploadResponseModel { ReturnCode = (int)OperationCode.Error_Unknown });
			}
		}

		#endregion
	}
}
