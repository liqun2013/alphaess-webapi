using AlphaEss.Api_V2.Infrastructure;
using System;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class ParameterValidateService : IParameterValidateService
	{
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;

		public ParameterValidateService(IEntityRepository<SYS_API, Guid> apiRepository)
		{
			_apiRepository = apiRepository;
		}

		/// <summary>
		/// api账号是否存在
		/// </summary>
		/// <param name="account">api账号</param>
		/// <param name="companyId">公司编号</param>
		/// <returns>存在:true/不存在:false</returns>
		public bool ApiAccountExist(string account, out Guid companyId)
		{
			bool result = false;
			companyId = Guid.Empty;
			if (!string.IsNullOrWhiteSpace(account))
			{
				var theApiAccount = _apiRepository.GetSingleByAccount(account);
				if (theApiAccount != null && !string.IsNullOrWhiteSpace(theApiAccount.CompanyId))
				{
					companyId = new Guid(theApiAccount.CompanyId);
					result = true;
				}
			}

			return result;
		}

		/// <summary>
		/// api账号是否存在
		/// </summary>
		/// <param name="account">api账号</param>
		/// <returns>存在:true/不存在:false</returns>
		public bool ApiAccountExist(string account)
		{
			if (string.IsNullOrWhiteSpace(account))
				return false;

			return _apiRepository.GetSingleByAccount(account) != null;
		}

		/// <summary>
		/// 检测时间戳
		/// </summary>
		/// <param name="timestamp">时间戳</param>
		/// <returns>有效:true/无效:false</returns>
		public bool CheckTimestamp(long timestamp)
		{
			var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
			return (ts - timestamp) < 180;
    }
	}
}
