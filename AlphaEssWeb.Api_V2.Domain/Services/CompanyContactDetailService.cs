using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaEss.Api_V2.Infrastructure;
using System.Collections;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class CompanyContactDetailService : ICompanyContactDetailService
	{
		private readonly IEntityRepository<CompanyContactDetail, long> _companyContactDetailRepository;
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly ICryptoService _cryptoService;

		public CompanyContactDetailService(IEntityRepository<CompanyContactDetail, long> companyContactDetailRepository, IEntityRepository<SYS_API, Guid> apiRepository, IParameterValidateService parameterValidateService, ICryptoService cryptoService)
		{
			_cryptoService = cryptoService;
			_parameterValidateService = parameterValidateService;
			_apiRepository = apiRepository;
			_companyContactDetailRepository = companyContactDetailRepository;
		}
		private bool checkSignForGetCompanyContacts(string api_account, long timeStamp, string sign, string secretKey, int flag)
		{
			SortedList slParams = new SortedList();
			slParams.Add("api_account", api_account);
			slParams.Add("flag", flag.ToString());
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("secretkey", secretKey);

			StringBuilder sbParams = new StringBuilder();
			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}
		public OperationResult<IEnumerable<CompanyContactDetail>> GetCompanyContacts(string api_account, long timeStamp, string sign, int flag)
		{
			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<IEnumerable<CompanyContactDetail>>(OperationCode.Error_TimeStamp);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<IEnumerable<CompanyContactDetail>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetCompanyContacts(api_account, timeStamp, sign, apiAccount.Api_SecretKey, flag))
				return new OperationResult<IEnumerable<CompanyContactDetail>>(OperationCode.Error_Sign);

			var result = new List<CompanyContactDetail>();
			if(flag == 0)
				result = _companyContactDetailRepository.GetAll().OrderBy(x => x.DisplayOrder).ToList();
			else
				result.Add(_companyContactDetailRepository.GetAll().OrderBy(x => x.DisplayOrder).FirstOrDefault());

			return new OperationResult<IEnumerable<CompanyContactDetail>>(OperationCode.Success, result);
		}
	}
}
