namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class CompanyContactDetailDto : IDto<long>
	{
		public long Id { get; set; }
		public string CompanyName { get; set; }
		public string ContactNumber1 { get; set; }
		public string ContactNumber2 { get; set; }
		public string Email { get; set; }
		public string ContactAddress { get; set; }
		public string WebSite { get; set; }
		public string Remark { get; set; }
		public int DisplayOrder { get; set; }
	}
}
