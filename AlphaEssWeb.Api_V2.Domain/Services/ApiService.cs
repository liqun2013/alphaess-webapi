using AlphaEss.Api_V2.Infrastructure;
using System;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class ApiService : IApiService
	{
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;

		public ApiService(IEntityRepository<SYS_API, Guid> apiRepository)
		{
			_apiRepository = apiRepository;
		}
		public SYS_API GetByAccount(string account)
		{
			return _apiRepository.GetSingleByAccount(account);
		}
	}
}
