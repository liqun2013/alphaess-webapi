using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	public static class SYS_USERAGREEMENTExtensions
	{
		public static SysUserAgreementDto ToSysUserAgreementDto(this SYS_USERAGREEMENT userAgreement)
		{
			return new SysUserAgreementDto
			{
				AgreementContent = userAgreement.Agreement_Content,
				AgreementLanguage = userAgreement.Agreement_Language,
				CreateDatetime = userAgreement.CreateTime,
				Id = userAgreement.Key,
				UpdateDatetime = userAgreement.UpdateTime
			};
		}
	}
}
