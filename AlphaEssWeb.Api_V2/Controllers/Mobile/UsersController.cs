using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain.Services;
using AlphaEssWeb.Api_V2.Model;
using AlphaEssWeb.Api_V2.Model.ExternalRequestModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AlphaEssWeb.Api_V2.Controllers.Mobile
{
	public class UsersController : ApiController
	{
		private readonly IMembershipService _membershipService;
		private readonly ICryptoService _cryptoService;

		public UsersController(IMembershipService membershipService, ICryptoService cryptoService)
		{
			_membershipService = membershipService;
			_cryptoService = cryptoService;
		}

		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="errm"></param>
		/// <returns></returns>
		public HttpResponseMessage Register(ExternalRegisterRequestModel errm)
		{
			var or = _membershipService.Register(errm.Api_Account, errm.TimeStamp, errm.Sign, errm.Sn, errm.License_No, errm.Username, errm.Password, errm.Email, errm.Allow_AutoUpdate);
			var respContent = or.ToExternalRegisterResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 用户协议
		/// </summary>
		/// <param name="eguarm"></param>
		/// <returns></returns>
		public HttpResponseMessage UserAgreement(ExternalGetUserAgreementRequestModel eguarm)
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
		public HttpResponseMessage UserInfo(ExternalUserInfoRequestModel eurm)
		{
			var or = _membershipService.GetUser(eurm.Api_Account, eurm.TimeStamp, eurm.Sign, eurm.Username);
			var respContent = or.ToExternalUserinfoResponseModel();

			return Request.CreateResponse(HttpStatusCode.OK, respContent);
		}

		/// <summary>
		/// 修改用户信息
		/// </summary>
		/// <param name="ecurm"></param>
		/// <returns></returns>
		public HttpResponseMessage ChangeUserInfo(ExternalChangeUserInfoRequestModel ecurm)
		{
			var or = _membershipService.UpdateUserinfo(ecurm.Api_Account, ecurm.TimeStamp, ecurm.Sign, ecurm.Username, ecurm.Language_Code, ecurm.Country,
				ecurm.State, ecurm.City, ecurm.Zipcode, ecurm.Contact_User, ecurm.Email, ecurm.Allow_AutoUpdate, ecurm.CellPhone);

			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalChangeUserInfoResponseModel());
		}

		/// <summary>
		/// 找回密码
		/// </summary>
		/// <param name="erprm"></param>
		/// <returns></returns>
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
		public HttpResponseMessage ChangePassword(ExternalChangePwdRequestModel ecprm)
		{
			var or = _membershipService.ChangePassword(ecprm.Api_Account, ecprm.TimeStamp, ecprm.Sign, ecprm.Username, ecprm.OldPwd, ecprm.NewPwd);

			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalChangePasswordResponseModel());
		}

		/// <summary>
		/// 登入
		/// </summary>
		/// <param name="elrm"></param>
		/// <returns></returns>
		public HttpResponseMessage Login([FromBody]ExternalLoginRequestModel elrm)
		{
			var or = _membershipService.Login(elrm.Api_Account, elrm.TimeStamp, elrm.Sign, elrm.Username, elrm.Password);
			
			return Request.CreateResponse(HttpStatusCode.OK, or.ToExternalLoginResponseModel());
		}

		//[HttpGet]
		//public string PasswordReset()
		//{
		//	var users = _membershipService.GetAllUsers().ToList();
		//	foreach (var u in users)
		//	{
		//		if (!string.IsNullOrWhiteSpace(u.USERPWD))
		//		{
		//			var decryptPwd = _cryptoService.Decrypt(u.USERPWD, u.USERNAME);
		//			if (!string.IsNullOrWhiteSpace(decryptPwd))
		//			{
		//				var newEncrypt = _cryptoService.EncryptNew(decryptPwd, u.USERNAME);
		//				//u.USERPWD = newEncrypt;
		//				_membershipService.ChangePassword(u.USERNAME, newEncrypt);
		//			}
		//		}
		//	}

		//	return "success";
		//}

		//[HttpGet]
		//public string PasswordResetNew(string s)
		//{
		//	var users = _membershipService.GetAllUsers().ToList();
		//	foreach (var u in users)
		//	{
		//		if (!string.IsNullOrWhiteSpace(u.USERPWD))
		//		{
		//			var decryptPwd = _cryptoService.DecryptNew(u.USERPWD, u.USERNAME);
		//			if (!string.IsNullOrWhiteSpace(decryptPwd))
		//			{
		//				var newEncrypt = _cryptoService.EncryptNew(decryptPwd, u.USERNAME);
		//				//u.USERPWD = newEncrypt;
		//				_membershipService.ChangePassword(u.USERNAME, newEncrypt);
		//			}
		//		}
		//	}

		//	return "success again";
		//}
	}
}
