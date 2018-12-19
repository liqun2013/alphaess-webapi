namespace AlphaEss.Api_V2.Infrastructure
{
	/// <summary>
	/// 返回码
	/// </summary>
	public enum OperationCode
	{
		/// <summary>
		/// 未知错误
		/// </summary>
		Error_Unknown = -1,
		/// <summary>
		/// 成功
		/// </summary>
		Success = 0,
		/// <summary>
		/// 参数错误
		/// </summary>
		Error_Param = 1,
		/// <summary>
		/// 参数为空
		/// </summary>
		Error_Param_Empty = 2,
		/// <summary>
		/// 时间戳错误
		/// </summary>
		Error_TimeStamp = 3,
		/// <summary>
		/// 签名错误
		/// </summary>
		Error_Sign = 4,
		/// <summary>
		/// 用户已存在
		/// </summary>
		Error_UserExist = 5,
		/// <summary>
		/// license已存在
		/// </summary>
		Error_LicenseExist = 6,
		/// <summary>
		/// sn已存在
		/// </summary>
		Error_SNExist = 7,
		/// <summary>
		/// api账号不存在
		/// </summary>
		Error_ApiAccountNotExist = 8,
		/// <summary>
		/// 用户不存在
		/// </summary>
		Error_UserNotExist = 9,
		/// <summary>
		/// email地址与用户不匹配
		/// </summary>
		Error_EmailNotMapToTheUser = 10,
		/// <summary>
		/// 密码不匹配
		/// </summary>
		Error_PasswordNotMatch = 11,
		/// <summary>
		/// sn不存在
		/// </summary>
		Error_SNNotExist = 12,
		/// <summary>
		/// 命令发送失败
		/// </summary>
		Error_SendCommandFailed = 13,
		/// <summary>
		/// 登录失效   
		/// </summary>
		Error_LoginFailed = 14,
		/// <summary>
		/// 用户无此操作权限
		/// </summary>    
		Error_NoPermissionsToQuery = 15,
		/// <summary>
		/// 用户更换设备，请重新登录
		/// </summary>    
		Error_UserChangesDevice = 16,
		/// <summary>
		/// 原密码不对
		/// </summary>
		Error_OldPasswordNotMatch = 17,
		/// <summary>
		/// 微网Id不存在
		/// </summary>
		Error_MicrogridIdNotExist = 18,
		/// <summary>
		/// 无微网信息
		/// </summary>
		Error_NoMicrogridInfo = 19,
		/// <summary>
		/// 无微网调度策略
		/// </summary>
		Error_NoSchedulingStrategy = 20,
		/// <summary>
		/// 微网调度策略更新失败
		/// </summary>
		Error_UpdateSchedulingStrategyFailed = 21,
		/// <summary>
		/// 无汇总信息
		/// </summary>
		Error_NoMicrogridSummary = 22,
		/// <summary>
		/// 令牌超时
		/// </summary>
		Error_TokenExpiration = 23,
		/// <summary>
		/// 系统不存在此license
		/// </summary>
		Error_LicenseNotExist = 24,
		/// <summary>
		/// license已被注册
		/// </summary>
		Error_LicenseBeRegistered = 25,
		/// <summary>
		/// 用户协议没有
		/// </summary>
		Error_NoAgreement = 26,
		/// <summary>
		/// SN已被注册
		/// </summary>
		Error_SnBeRegistered = 27,
		/// <summary>
		/// 系统设置失败
		/// </summary>
		Error_UpdateSystemFailed = 28,
		/// <summary>
		/// 没有对应sn的系统
		/// </summary>
		Error_SystemNotExist = 29,
		/// <summary>
		/// 系统已绑定用户
		/// </summary>
		Error_AccountBound = 30,
		/// <summary>
		/// 无数据
		/// </summary>
		Error_NoData = 31,
		Error_AddNewComplaintsFailed = 32,
		/// <summary>
		/// License与当前安装商不一致！	
		/// </summary>
		Error_LicenseInconsistent = 33,
		/// <summary>
		/// checkcode错误
		/// </summary>
		Error_WrongCheckcode = 34,
		/// <summary>
		/// 文件太大
		/// </summary>
		Error_FileIsTooLarge = 35,
		/// <summary>
		/// 更新系统固件失败
		/// </summary>
		Error_UpdateSystemFirmwareFailed = 36,
		/// <summary>
		/// 已安装
		/// </summary>
		Error_InstallExist = 37,
		/// <summary>
		/// 已经评价过
		/// </summary>
		Error_ComplaintsEvaluated = 38
	}
	public class OperationResult
	{
		public OperationResult(OperationCode code)
		{
			ReturnCode = (int)code;
		}
		public int ReturnCode { get; private set; }
	}
	public class OperationResult<TEntity> : OperationResult
	{
		public OperationResult(OperationCode code) : base(code)
		{ }
		public OperationResult(OperationCode code, TEntity e) : base(code)
		{
			Entity = e;
		}
		public TEntity Entity { get; set; }
	}
}
