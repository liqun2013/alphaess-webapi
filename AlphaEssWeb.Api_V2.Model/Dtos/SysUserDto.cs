using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class SysUserDto: IDto<Guid>
	{
		public Guid Id { get; set; }
		public string UserType { get; set; }
		public string LicenseNo { get; set; }
		public string UserName { get; set; }
		public string UserPwd { get; set; }
		public string Email { get; set; }
		public string CountryCode { get; set; }
		public string StateCode { get; set; }
		public string CityCode { get; set; }
		public string Address { get; set; }
		public string PostCode { get; set; }
		/// <summary>
		/// 对应LINKMAN字段
		/// </summary>
		public string ContactUser { get; set; }
		public string CellPhone { get; set; }
		public string Fax { get; set; }
		public int DeleteFlag { get; set; }
		public string CreateAccount { get; set; }
		public string UpdateAccount { get; set; }
		public DateTime? CreateDatetime { get; set; }
		public DateTime? UpdateDatetime { get; set; }
		/// <summary>
		/// 分享的SN
		/// 对应Attr1字段
		/// </summary>
		public string SharedSn { get; set; }
		/// <summary>
		/// 有效天数
		/// 对应Attr2字段
		/// </summary>
		public string EffectiveDays { get; set; }
		/// <summary>
		/// 分享时的时间
		/// 对应Attr3字段
		/// </summary>
		public string SharedTime { get; set; }
		public int ServiceAreaFlag { get; set; }
		public string LanguageCode { get; set; }
		public int LicenseValidFlag { get; set; }

		public IEnumerable<SysRoleDto> Roles { get; set; }
		public IEnumerable<VtSystemDto> VtSystems { get; set; }
		public string AllowAutoUpdate { get; set; }
	}
}
