using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Text;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Helpers;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class ComplaintsService : IComplaintsService
	{
		private readonly IComplaintsRepository _repository;
		private readonly IEntityRepository<SYS_Country_Region, Guid> _countryRegionRepository;
		private readonly IEntityRepository<BASE_COUNTRY, int> _countryRepository;
		private readonly IEntityRepository<SYS_USER, Guid> _userRepository;
		private readonly IEntityRepository<CustomerInfo, long> _customerInfoRepository;
		private readonly IEntityRepository<Sys_Language, Guid> _languageRepository;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly ICryptoService _cryptoService;
		private readonly ISystemRepository _systemRepository;
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;
		private readonly IEntityRepository<SYS_LOG, Guid> _syslogRepository;

		private string GetCountryCode(string idOrName)
		{
			var countryName = idOrName;
			int c = 0;
			if (int.TryParse(idOrName, out c))
			{
				var theCountry = _countryRepository.GetAll().FirstOrDefault(x => x.Key == c);
				countryName = theCountry != null ? theCountry.AREA_NAME : string.Empty;
			}

			if (!string.IsNullOrWhiteSpace(countryName))
			{
				var countryRegions = _countryRegionRepository.GetAll().ToList();
				if (countryRegions != null && countryRegions.Any(x => x.AreaEnglishName.Equals(countryName, StringComparison.OrdinalIgnoreCase) || x.AreaFirstName.Equals(countryName, StringComparison.OrdinalIgnoreCase)))
				{
					var theCountryRegion = countryRegions.FirstOrDefault(x => x.AreaEnglishName.Equals(countryName, StringComparison.OrdinalIgnoreCase) || x.AreaFirstName.Equals(countryName, StringComparison.OrdinalIgnoreCase));
					return theCountryRegion.AreaEnglishName;
				}
			}

			return string.Empty;
		}

		private bool checkSignForAddNewComplaints(string api_account, long timeStamp, string sign, string token, string title, string description, string complaintsType, string email, string contactNumber, string sn, string attachment1, string attachment2, string attachment3, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("sysSn", sn);
			slParams.Add("title", title);
			slParams.Add("description", description);
			slParams.Add("attachment1", string.IsNullOrWhiteSpace(attachment1) ? string.Empty : attachment1);
			slParams.Add("attachment2", string.IsNullOrWhiteSpace(attachment2) ? string.Empty : attachment2);
			slParams.Add("attachment3", string.IsNullOrWhiteSpace(attachment3) ? string.Empty : attachment3);
			slParams.Add("complaintsType", complaintsType);
			slParams.Add("email", string.IsNullOrWhiteSpace(email) ? string.Empty : email);
			slParams.Add("contactNumber", string.IsNullOrWhiteSpace(contactNumber) ? string.Empty : contactNumber);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForGetComplaintsList(string api_account, long timeStamp, string sign, string token, int pi, int ps, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("pageindex", pi.ToString());
			slParams.Add("pagesize", ps.ToString());
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		private bool checkSignForEvaluateComplaints(string api_account, long timeStamp, string sign, string token, long cpid, int satsf, int satsf1, int satsf2, string content, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("complaintsId", cpid.ToString());
			slParams.Add("satisfaction", satsf.ToString());
			slParams.Add("satisfaction1", satsf1.ToString());
			slParams.Add("satisfaction2", satsf2.ToString());
			slParams.Add("Token", token);
			slParams.Add("content", content);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			//_syslogRepository.Add(new SYS_LOG { Key = Guid.NewGuid(), MESSAGE = sbParams.ToString(), LOGGER = "webapi", CREATE_DATETIME = DateTime.Now });
			//_syslogRepository.Save();
			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		public ComplaintsService(IComplaintsRepository repository, IEntityRepository<SYS_USER, Guid> userRepository, IEntityRepository<CustomerInfo, long> customerInfoRepository,
			IEntityRepository<BASE_COUNTRY, int> countryRepository, IEntityRepository<SYS_Country_Region, Guid> countryRegionRepository, 
			IEntityRepository<SYS_API, Guid> apiRepository, IEntityRepository<Sys_Language, Guid> languageRepository, ISystemRepository systemRepository, IEntityRepository<SYS_LOG, Guid> syslogRepository,
			IParameterValidateService parameterValidateService, ICryptoService cryptoService)
		{
			_syslogRepository = syslogRepository;
			_repository = repository; //new EntityRepository<Complaints, long>(new Domain.AlphaComplaintsProcessingDbContext());
			_cryptoService = cryptoService;
			_countryRegionRepository = countryRegionRepository;
			_parameterValidateService = parameterValidateService;
			_apiRepository = apiRepository;
			_customerInfoRepository = customerInfoRepository;
			_languageRepository = languageRepository;
			_userRepository = userRepository;
			_countryRepository = countryRepository;
			_systemRepository = systemRepository;
		}
		public Complaints AddNewComplaint(Complaints complaint)
		{
			complaint.CreateTime = DateTime.Now;
			complaint.UpdateTime = DateTime.Now;
			_repository.Add(complaint);
			_repository.Save();
			return complaint;
		}

		public OperationResult AddNewComplaints(string api_account, long timeStamp, string sign, string token, string title, string description, string complaintsTyps, string email, string contactNumber, string sn, string attachment1, string attachment2, string attachment3)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForAddNewComplaints(api_account, timeStamp, sign, token, title, description, complaintsTyps, email, contactNumber, sn, attachment1, attachment2, attachment3, apiAccount.Api_SecretKey))
				return new OperationResult(OperationCode.Error_Sign);

			var theNewComplaint = new Complaints { Title = title, ComplaintsType = complaintsTyps, Email = email, ContactNumber = contactNumber, SysSn = sn, Attachment = HttpUtility.UrlDecode(attachment1), Attachment3 = HttpUtility.UrlDecode(attachment3), Attachment2 = HttpUtility.UrlDecode(attachment2) };
			theNewComplaint.Description = !string.IsNullOrWhiteSpace(description) ? HttpUtility.UrlDecode(description).Replace("\n", HttpUtility.HtmlEncode("<br/>")) : string.Empty;
			var s = _systemRepository.GetSystemBySn(sn);
			if (s != null)
			{
				if (!string.IsNullOrWhiteSpace(s.CountryCode))
				{
					theNewComplaint.Area = GetCountryCode(s.CountryCode);
					theNewComplaint.SystemLicense = s.LicNo;
					theNewComplaint.SystemMinv = s.Minv;
					theNewComplaint.SystemPostcode = s.PostCode;
				}
			}

			var tk = TokenHelper.GetToken(new Guid(token));
			var u = _userRepository.GetSingleByKey(tk.UserId);
			if (tk.UserTypes.Contains("installer"))
			{
				theNewComplaint.SystemLicense = u.LICNO;
			}
			theNewComplaint.Creator = u.USERNAME;

			theNewComplaint.ComplaintsProcessing.Add(new ComplaintsProcessing { CurrentStatus = ComplaintStatus.Open.ToString(), ProcessFlowNumber = 0, ProcessingTime = DateTime.Now, Processor = u.USERNAME });
			if (AddNewComplaint(theNewComplaint) == null)
				return new OperationResult(OperationCode.Error_AddNewComplaintsFailed);

			SYS_USER theCustomer = null;
			if (tk.UserTypes.Contains("customer"))
				theCustomer = u;
			else if (s != null && s.UserId.HasValue)
				theCustomer = _userRepository.GetSingleByKey(s.UserId.Value);

			if (theCustomer != null)
			{
				var customerInfo = new CustomerInfo
				{
					ComplaintId = theNewComplaint.Key, SysSn = theNewComplaint.SysSn, CustomerContactNumber = theCustomer.CELLPHONE,
					CustomerCountry = GetCountryCode(theCustomer.COUNTRYCODE), CustomerEmail = theCustomer.EMAIL, ContactName = theCustomer.LINKMAN,
					CustomerAddress = theCustomer.ADDRESS, CustomerPostcode = theCustomer.POSTCODE, CustomerName = theCustomer.USERNAME, CreateTime = DateTime.Now
				};
				_customerInfoRepository.Add(customerInfo);
				_customerInfoRepository.Save();
			}

			return new OperationResult(OperationCode.Success);
		}

		public List<Complaints> QueryForCreator(long? id, string creator, int pageIndex, int pageSize, out int total)
		{
			if (string.IsNullOrWhiteSpace(creator))
				throw new ArgumentException("Invalid parameter", "creator");

			Expression<Func<Complaints, bool>> idFunc = f => true;

			if (id.HasValue && id > 0)
				idFunc = x => x.Key == id.Value;

			var query = _repository.GetAll().Where(x => x.Creator == creator && x.IsDelete == 0).Where(idFunc);
			total = query.Count();

			if (total > 0)
				return query.OrderByDescending(x => x.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
			else
				return null;
		}

		public OperationResult<PaginatedList<Complaints>> GetComplaintsList(string api_account, long timeStamp, string sign, string token, int pi, int ps)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForGetComplaintsList(api_account, timeStamp, sign, token, pi, ps, apiAccount.Api_SecretKey))
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_UserNotExist);

			IList<Complaints> complaints = null;
			int t = 0;
			if (theToken.UserTypes.Contains("customer"))
				complaints = QueryForCreator(null, user.USERNAME, pi, ps, out t);
			else if (theToken.UserTypes.Contains("installer"))
				complaints = _repository.QueryComplaintsForInstaller(null, user.USERNAME, user.LICNO, pi, ps, out t);

			if (t > 0)
			{
				foreach (var itm in complaints)
				{
					itm.StrStatus = GetComplaintsStatus(itm.Status);
					itm.StrComplaintsType = GetComplaintType(itm.ComplaintsType);
				}
			}

			PaginatedList<Complaints> result = new PaginatedList<Complaints>(pi, ps, t, complaints);

			return new OperationResult<PaginatedList<Complaints>>(OperationCode.Success, result);
		}

		public OperationResult EvaluateComplaints(string api_account, long timeStamp, string sign, string token, long cpid, int satsf, int satsf1, int satsf2, string content)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_ApiAccountNotExist);

			if (!checkSignForEvaluateComplaints(api_account, timeStamp, sign, token, cpid, satsf, satsf1, satsf2, content, apiAccount.Api_SecretKey))
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<PaginatedList<Complaints>>(OperationCode.Error_UserNotExist);

			try
			{
				var cp = GetComplaintsById(cpid, theToken, user);
				if (cp.Status != (int)ComplaintStatus.Completed)
					return new OperationResult(OperationCode.Error_ComplaintsEvaluated);

				var customerReview = new CustomerReviews { ComplaintId = cpid, CreateTime = DateTime.Now, Content = content, Satisfaction = satsf, Satisfaction1 = satsf1, Satisfaction2 = satsf2 };
				var complaintsProcessing = new ComplaintsProcessing { ComplaintId = cpid, OriginalStatus = ((ComplaintStatus)cp.Status).ToString(), ProcessingTime = DateTime.Now, Processor = user.USERNAME };
				cp.Status = (int)ComplaintStatus.Evaluated;

				complaintsProcessing.CurrentStatus = ComplaintStatus.Evaluated.ToString();
				complaintsProcessing.ProcessFlowNumber = cp.ComplaintsProcessing != null && cp.ComplaintsProcessing.Any() ?
																							cp.ComplaintsProcessing.Max(x => x.ProcessFlowNumber) + 1 : 1;

				if (cp.ComplaintsProcessing == null)
					cp.ComplaintsProcessing = new List<ComplaintsProcessing>();
				cp.ComplaintsProcessing.Add(complaintsProcessing);

				if (cp.CustomerReviews == null)
					cp.CustomerReviews = new List<CustomerReviews>();
				cp.CustomerReviews.Add(customerReview);

				_repository.Edit(cp);
				_repository.Save();
			}
			catch (Exception)
			{
				return new OperationResult(OperationCode.Error_Unknown);
			}

			return new OperationResult(OperationCode.Success);
		}

		public List<Sys_Language> Languages
		{
			get
			{
				var languages = WebCache.Get("syslanguages") as List<Sys_Language>;
				if (languages == null)
					languages = _languageRepository.GetByLanguage("en-US").ToList();

				return languages;
			}
		}

		private Complaints GetComplaintsById(long id, Model.Token token, SYS_USER u)
		{
			int t = 0;
			IEnumerable<Complaints> data = null;
			Complaints result = null;

			try
			{
				if (token.UserTypes.Contains("customer"))
				{ data = QueryForCreator(null, u.USERNAME, 1, int.MaxValue, out t); }
				else if (token.UserTypes.Contains("installer"))
				{ data = _repository.QueryComplaintsForInstaller(id, u.USERNAME, u.LICNO, 1, 1, out t); }
				
				if (t > 0)
				{
					result = _repository.AllIncluding(x => x.ComplaintsProcessing, x => x.CustomerReviews).FirstOrDefault(x=>x.Key == id);
				}
			}
			catch (Exception exc)
			{
				throw;
			}

			return result;
		}
		private string GetComplaintsStatus(int st)
		{
			var processingStatus = new List<int>()
					{
						(int)ComplaintStatus.Processing,
						(int)ComplaintStatus.ToBeVerified,
						(int)ComplaintStatus.Verification,
						(int)ComplaintStatus.VerificationCompleted,
						(int)ComplaintStatus.ReOpen
					};

			return processingStatus.Contains(st) ? ComplaintStatus.Processing.ToString() : ((ComplaintStatus)st).ToString();
			//var l = Languages.FirstOrDefault(x => x.LanguageKey == k);
			//return l != null ? l.LanguageValue : string.Empty;
		}

		private string GetComplaintType(string ct)
		{
			var result = ct;

			var k = ComplaintsType.lab_other;
			if (Enum.TryParse(ct, out k))
			{
				var l = Languages.FirstOrDefault(x => x.LanguageKey == k.ToString());
				if (l != null)
					result = l.LanguageValue;
			}
			return result;
		}
	}
}
