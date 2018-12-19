using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class SysUserAgreementDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public string AgreementLanguage { get; set; }
		public string AgreementContent { get; set; }
		public DateTime? CreateDatetime { get; set; }
		public DateTime? UpdateDatetime { get; set; }
	}
}
