using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class CompanyContactDetailExtensions
	{
		public static CompanyContactDetailDto ToCompanyContactDetailDto(this CompanyContactDetail ccd)
		{
			return new CompanyContactDetailDto { CompanyName = ccd.CompanyName, ContactAddress = ccd.ContactAddress, ContactNumber1 = ccd.ContactNumber1, ContactNumber2 = ccd.ContactNumber2, Email = ccd.Email, Id = ccd.Key, Remark = ccd.Remark, WebSite = ccd.WebSite, DisplayOrder = ccd.DisplayOrder ?? 0 };
		}
		public static IEnumerable<CompanyContactDetailDto> ToCollectionCompanyContactDetailDto(this IEnumerable<CompanyContactDetail> ccd)
		{
			return ccd.Select(x => x.ToCompanyContactDetailDto());
		}
	}
}