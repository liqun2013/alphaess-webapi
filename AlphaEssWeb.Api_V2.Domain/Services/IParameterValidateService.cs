namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IParameterValidateService
	{
		/// <summary>
		/// api账号是否存在
		/// </summary>
		/// <param name="account">api账号</param>
		/// <returns>存在:true/不存在:false</returns>
		bool ApiAccountExist(string account, out System.Guid companyId);
		bool ApiAccountExist(string account);
		/// <summary>
		/// 检测时间戳
		/// </summary>
		/// <param name="timestamp">时间戳</param>
		/// <returns>有效:true/无效:false</returns>
		bool CheckTimestamp(long timestamp);
  }
}
