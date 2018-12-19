using AlphaEssWeb.Api_V2.Model.Validation;

namespace AlphaEssWeb.Api_V2.Model.RequestCommands
{
	public class PaginatedRequestCommand : IRequestCommand
	{
		[Minimum(1)]
		public int PageIndex { get; set; }
		[Minimum(1)]
		public int PageSize { get; set; }
	}
}
