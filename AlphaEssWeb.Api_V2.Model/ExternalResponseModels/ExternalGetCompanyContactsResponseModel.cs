using AlphaEssWeb.Api_V2.Model.Dtos;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalGetCompanyContactsResponseModel: ExternalBaseResponseModel
	{
		public IEnumerable<CompanyContactDetailDto> Result { get; set; }
	}
}
