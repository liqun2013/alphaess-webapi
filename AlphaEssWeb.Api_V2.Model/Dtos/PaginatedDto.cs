using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public class PaginatedDto<TDto> : IPaginatedDto<TDto> where TDto : class
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public int TotalPageCount { get; set; }
		public bool HasNextPage { get; set; }
		public bool HasPreviousPage { get; set; }
		public IEnumerable<TDto> Items { get; set; }
	}
}
