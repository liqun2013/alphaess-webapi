namespace AlphaEssWeb.Api_V2.Domain
{
	public sealed class FirmwareVersionData
	{
		public string BMSVersion { get; set; }
		public string EMSVersion { get; set; }
		public string InvVersion { get; set; }
		public string LatestBMSVersion { get; set; }
		public string LatestEMSVersion { get; set; }
		public string LatestInvVersion { get; set; }
	}
}
