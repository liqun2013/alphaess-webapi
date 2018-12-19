using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public class SysMsgService : ISysMsgService
	{
		private readonly IEntityRepository<SYS_MSG, Guid> _msgRepository;
		private readonly IEntityRepository<SYS_MSGUSER, Guid> _msguserRepository;
		private readonly ICryptoService _cryptoService;
		private readonly IParameterValidateService _parameterValidateService;
		private readonly IEntityRepository<SYS_API, Guid> _apiRepository;
		private readonly IEntityRepository<SYS_USER, Guid> _userRepository;

		public SysMsgService(IEntityRepository<SYS_MSG, Guid> msgRepository, IEntityRepository<SYS_MSGUSER, Guid> msguserRepository, ICryptoService cryptoService, IParameterValidateService parameterValidateService,
			IEntityRepository<SYS_API, Guid> apiRepository, IEntityRepository<SYS_USER, Guid> userRepository)
		{
			_userRepository = userRepository;
			_apiRepository = apiRepository;
			_msgRepository = msgRepository;
			_msguserRepository = msguserRepository;
			_cryptoService = cryptoService;
			_parameterValidateService = parameterValidateService;
		}

		private bool CheckSignForGetMsg(string api_account, long timeStamp, string sign, string token, int onlyUnread, int pi, int ps, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("pageindex", pi.ToString());
			slParams.Add("pagesize", ps.ToString());
			slParams.Add("onlyUnread", onlyUnread.ToString());
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		public List<SYS_MSGUSER> QueryMsg(string userId, Guid? msgId, int pageIndex, int pageSize, out int total)
		{
			total = 0;
			List<SYS_MSGUSER> result = null;
			Expression<Func<SYS_MSGUSER, bool>> userIdFunc = f => true;
			Expression<Func<SYS_MSGUSER, bool>> msgIdFunc = f => true;

			if (!string.IsNullOrWhiteSpace(userId))
			{
				var g = new Guid(userId);
				userIdFunc = f => f.USER_ID == g;
			}
			if (msgId.HasValue)
				msgIdFunc = f => f.MSG_ID == msgId;

			var query = from mu in _msguserRepository.GetAll().Where(x => x.DELETE_FLAG == 0).Where(userIdFunc).Where(msgIdFunc)
									join msg in _msgRepository.GetAll() on mu.MSG_ID equals msg.Key
									join su in _userRepository.GetAll() on mu.SENDUSER_ID equals su.Key into mu_su
									from musu in mu_su.DefaultIfEmpty()
									join ru in _userRepository.GetAll() on mu.USER_ID equals ru.Key into mu_ru
									from muru in mu_ru.DefaultIfEmpty()
									orderby mu.CREATE_DATETIME descending
									select new
									{
										mu.CREATE_ACCOUNT, mu.CREATE_DATETIME, mu.DELETE_FLAG, mu.LASTUPDATE_ACCOUNT, mu.LASTUPDATE_DATETIME, mu.Key, mu.MSG_ID, mu.SENDUSER_ID, mu.USERDEL_FLAG, mu.USERREAD_FLAG, mu.USER_ID,
										msg.MSGCONT, MsgId = msg.Key, msg.MsgLevel, msg.MSGTITLE, msg.MsgType,
										SendUserId = musu.Key, SendUserName = musu.USERNAME,
										RecieveUserId = muru.Key, RecieveUserName = muru.USERNAME
									};
			total = query.Count();

			if (total > 0)
			{
				result = new List<SYS_MSGUSER>();
				foreach (var i in query.Skip((pageIndex - 1) * pageSize).Take(pageSize))
				{
					var item = new SYS_MSGUSER
					{
						CREATE_ACCOUNT = i.CREATE_ACCOUNT, Key = i.Key, CREATE_DATETIME = i.CREATE_DATETIME, DELETE_FLAG = i.DELETE_FLAG,
						LASTUPDATE_ACCOUNT = i.LASTUPDATE_ACCOUNT, LASTUPDATE_DATETIME = i.LASTUPDATE_DATETIME, MSG_ID = i.MSG_ID, SENDUSER_ID = i.SENDUSER_ID, USERDEL_FLAG = i.USERDEL_FLAG,
						USERREAD_FLAG = i.USERREAD_FLAG, USER_ID = i.USER_ID
					};
					item.SYS_MSG = new SYS_MSG { MsgType = i.MsgType, MSGTITLE = i.MSGTITLE, MsgLevel = i.MsgLevel, Key = i.MsgId, MSGCONT = i.MSGCONT };
					item.SYS_USER = new SYS_USER { Key = i.SendUserId, USERNAME = i.SendUserName };
					item.SYS_USER1 = new SYS_USER { Key = i.RecieveUserId, USERNAME = i.RecieveUserName };
					result.Add(item);
				}
			}

			return result;
		}

		public List<SYS_MSGUSER> GetUnreadMsgs(string userid, int pageIndex, int pageSize, out int total)
		{
			if (string.IsNullOrWhiteSpace(userid))
				throw new ArgumentNullException("userid");

			total = 0;
			List<SYS_MSGUSER> result = null;

			Guid uid = new Guid(userid);

			var query = from mu in _msguserRepository.GetAll()
									join msg in _msgRepository.GetAll() on mu.MSG_ID equals msg.Key into mu_msg
									from mumsg in mu_msg.DefaultIfEmpty()
									join ru in _userRepository.GetAll() on mu.USER_ID equals ru.Key into mu_ru
									from muru in mu_ru.DefaultIfEmpty()
									join su in _userRepository.GetAll() on mu.SENDUSER_ID equals su.Key into mu_su
									from musu in mu_su.DefaultIfEmpty()
									where mu.DELETE_FLAG == 0 && mu.USERREAD_FLAG != 1 && muru.Key == uid
									orderby mu.CREATE_DATETIME descending
									select new
									{
										mu.CREATE_ACCOUNT, mu.CREATE_DATETIME, mu.DELETE_FLAG, mu.LASTUPDATE_ACCOUNT, mu.LASTUPDATE_DATETIME, mu.Key, mu.MSG_ID, mu.SENDUSER_ID, mu.USERDEL_FLAG, mu.USERREAD_FLAG, mu.USER_ID,
										mumsg.MSGCONT, MsgId = mumsg.Key, mumsg.MsgLevel, mumsg.MSGTITLE, mumsg.MsgType,
										SendUserId = musu.Key, SendUserName = musu.USERNAME,
										RecieveUserId = muru.Key, RecieveUserName = muru.USERNAME
									};

			total = query.Count();

			if (total > 0)
			{
				result = new List<SYS_MSGUSER>();
				foreach (var i in query.Skip((pageIndex - 1) * pageSize).Take(pageSize))
				{
					var item = new SYS_MSGUSER
					{
						CREATE_ACCOUNT = i.CREATE_ACCOUNT, Key = i.Key, CREATE_DATETIME = i.CREATE_DATETIME, DELETE_FLAG = i.DELETE_FLAG,
						LASTUPDATE_ACCOUNT = i.LASTUPDATE_ACCOUNT, LASTUPDATE_DATETIME = i.LASTUPDATE_DATETIME, MSG_ID = i.MSG_ID, SENDUSER_ID = i.SENDUSER_ID, USERDEL_FLAG = i.USERDEL_FLAG,
						USERREAD_FLAG = i.USERREAD_FLAG, USER_ID = i.USER_ID
					};
					item.SYS_MSG = new SYS_MSG { MsgType = i.MsgType, MSGTITLE = i.MSGTITLE, MsgLevel = i.MsgLevel, Key = i.MsgId, MSGCONT = i.MSGCONT };
					item.SYS_USER = new SYS_USER { Key = i.SendUserId, USERNAME = i.SendUserName };
					item.SYS_USER1 = new SYS_USER { Key = i.RecieveUserId, USERNAME = i.RecieveUserName };
					result.Add(item);
				}
			}

			return result;
		}

		public OperationResult<PaginatedList<SYS_MSGUSER>> GetMsg(string api_account, long timeStamp, string sign, string token, int onlyUnread, int pi, int ps)
		{
			if (string.IsNullOrWhiteSpace(token))
				return new OperationResult<PaginatedList<SYS_MSGUSER>>(OperationCode.Error_Param_Empty);

			if (!_parameterValidateService.CheckTimestamp(timeStamp))
				return new OperationResult<PaginatedList<SYS_MSGUSER>>(OperationCode.Error_TimeStamp);

			if (!TokenHelper.CheckToken(token))
				return new OperationResult<PaginatedList<SYS_MSGUSER>>(OperationCode.Error_TokenExpiration);

			var apiAccount = _apiRepository.GetSingleByAccount(api_account);
			if (apiAccount == null)
				return new OperationResult<PaginatedList<SYS_MSGUSER>>(OperationCode.Error_ApiAccountNotExist);

			if (!CheckSignForGetMsg(api_account, timeStamp, sign, token, onlyUnread, pi, ps, apiAccount.Api_SecretKey))
				return new OperationResult<PaginatedList<SYS_MSGUSER>>(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult<PaginatedList<SYS_MSGUSER>>(OperationCode.Error_UserNotExist);

			List<SYS_MSGUSER> data = null;
			int t = 0;
			if (onlyUnread == 1)
				data = GetUnreadMsgs(user.Key.ToString(), pi, ps, out t);
			else
				data = QueryMsg(user.Key.ToString(), null, pi, ps, out t);

			PaginatedList<SYS_MSGUSER> result = new PaginatedList<SYS_MSGUSER>(pi, ps, t, data);
			return new OperationResult<PaginatedList<SYS_MSGUSER>>(OperationCode.Success, result);
		}

		private bool CheckSignForUpdateMsgFlag(string api_account, long timeStamp, string sign, string token, int flag, string msgId, string secretKey)
		{
			SortedList slParams = new SortedList();
			StringBuilder sbParams = new StringBuilder();
			slParams.Add("api_account", api_account);
			slParams.Add("timestamp", timeStamp.ToString());
			slParams.Add("flag", flag.ToString());
			slParams.Add("msgId", msgId);
			slParams.Add("Token", token);
			slParams.Add("secretkey", secretKey);

			for (var i = 0; i < slParams.Count; i++)
			{
				sbParams.AppendFormat("{0}={1}", slParams.GetKey(i), slParams.GetByIndex(i));
			}

			return sign.Equals(_cryptoService.GenerateMD5Hash(sbParams.ToString()));
		}

		public OperationResult UpdateMsgFlag(string api_account, long timeStamp, string sign, string token, int flag, string msgId)
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

			if (!CheckSignForUpdateMsgFlag(api_account, timeStamp, sign, token,flag, msgId, apiAccount.Api_SecretKey))
				return new OperationResult(OperationCode.Error_Sign);

			var theToken = TokenHelper.GetToken(token);
			var theCompanyId = new Guid(apiAccount.CompanyId);
			var user = _userRepository.GetSingleByKey(theToken.UserId);

			if (user == null)
				return new OperationResult(OperationCode.Error_UserNotExist);

			var mu = _msguserRepository.GetAll().FirstOrDefault(x => x.USER_ID == user.Key && x.MSG_ID == new Guid(msgId) && x.DELETE_FLAG == 0);
			if (mu == null)
				return new OperationResult(OperationCode.Error_Unknown);

			if (flag == 1)
				mu.USERREAD_FLAG = 1;
			else if (flag == 2)
				mu.DELETE_FLAG = 1;
			mu.LASTUPDATE_DATETIME = DateTime.Now;
			mu.LASTUPDATE_ACCOUNT = user.USERNAME;

			_msguserRepository.Edit(mu);
			_msguserRepository.Save();

			return new OperationResult(OperationCode.Success);
		}
	}
}
