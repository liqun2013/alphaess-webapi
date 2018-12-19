using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public class SysMsgDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public string MsgId { get; set; }
		public string Msgtitle { get; set; }
		public string Msgcont { get; set; }
		public int MsgType { get; set; }
		public string Sender { get; set; }
		public int ReadFlag { get; set; }
		public string CreateDatetime { get; set; }
	}
}
