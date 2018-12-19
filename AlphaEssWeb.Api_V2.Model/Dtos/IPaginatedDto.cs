using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public interface IPaginatedDto<out TDto> where TDto : class
	{
		int PageIndex { get; set; }
		int PageSize { get; set; }
		int TotalCount { get; set; }
		int TotalPageCount { get; set; }
		bool HasNextPage { get; set; }
		bool HasPreviousPage { get; set; }
		IEnumerable<TDto> Items { get; }
	}
}
