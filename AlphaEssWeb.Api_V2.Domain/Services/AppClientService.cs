using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Text;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class AppClientService : IAppClientService
	{
		private readonly IEntityRepository<APP_Version, Guid> _appVersionRepository;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;
		private readonly ICryptoService _cryptoService;

		public AppClientService(IEntityRepository<APP_Version, Guid> appVersionRepository, IEntityRepository<SYS_API, Guid> apiRepository, IParameterValidateService parameterValidateService, ICryptoService cryptoService)
		{
			_appVersionRepository = appVersionRepository;
			_parameterValidateService = parameterValidateService;
			_apiRepository = apiRepository;
			_cryptoService = cryptoService;
		}

		/// <summary>
		/// 为获取最新版app检查签名
		/// </summary>
		/// <param name="api_account"></param>
		/// <param name="timeStamp"></param>
		/// <param name="sign"></param>
		/// <param name="appType"></param>
		/// <returns></returns>
		private bool checkSignForGetLastVersion(string api_account, long timeStamp, string sign, string appType, string secretKey)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("appType", appType);
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		/// <summary>
		/// 获取最新版本信息
		/// </summary>
		/// <param name="appType"></param>
		/// <returns>APP_Version实例</returns>
		public APP_Version GetLastVersion(string appType)
		{
			return _appVersionRepository.GetLastAppVersionByAppType(appType);
		}

		/// <summary>
		/// 获取最新版本信息
		/// </summary>
		/// <param name="api_account"></param>
		/// <param name="timeStamp"></param>
		/// <param name="sign"></param>
		/// <param name="appType"></param>
		/// <returns>OperationResult实例</returns>
		public OperationResult<APP_Version> GetLastVersion(string api_account, long timeStamp, string sign, string appType)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<APP_Version>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<APP_Version>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetLastVersion(api_account, timeStamp, sign, appType, apiAccount.Api_SecretKey))
				return new OperationResult<APP_Version>(OperationCode.Error_Sign);

			var version = GetLastVersion(appType);
			var result = new OperationResult<APP_Version>(OperationCode.Success);
			result.Entity = version;

			return result;
		}
	}
}
