using AlphaEss.Api_V2.Infrastructure;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IAppClientService
	{
		/// <summary>
		/// 获取最新版本信息
		/// </summary>
		/// <param name="appType"></param>
		/// <returns></returns>
		APP_Version GetLastVersion(string appType);
		/// <summary>
		/// 获取最新版本信息
		/// </summary>
		/// <param name="api_account"></param>
		/// <param name="timeStamp"></param>
		/// <param name="sign"></param>
		/// <param name="appType"></param>
		/// <returns></returns>
		OperationResult<APP_Version> GetLastVersion(string api_account, long timeStamp, string sign, string appType);
	}
}
