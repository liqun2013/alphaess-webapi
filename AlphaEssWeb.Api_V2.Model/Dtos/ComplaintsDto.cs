using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class ComplaintsDto : IDto<long>
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ComplaintsType { get; set; }
		public DateTime CreateTime { get; set; }
		public DateTime? UpdateTime { get; set; }
		public string Creator { get; set; }
		//public ComplaintStatus Status { get; set; }
		public string Recipient { get; set; }
		public DateTime? ReceiveTime { get; set; }
		public string CurrentProcessor { get; set; }
		public string Area { get; set; }
		public string Attachment { get; set; }
		public string Attachment2 { get; set; }
		public string Attachment3 { get; set; }
		public int IsDelete { get; set; }
		public string SysSn { get; set; }
		public string Email { get; set; }
		public string ContactNumber { get; set; }

		public string OnsiteHandler { get; set; }
		public int ProcessingPriority { get; set; }
		public string Status { get; set; }
		public string AttachmentUrl
		{
			get
			{
				var result = string.Empty;
				if (!string.IsNullOrWhiteSpace(this.Attachment))
				{
					result = string.Format(FileBaseUrl, this.Id.ToString(), "1", "1");
				}
				return (result);
			}
		}
		public string Attachment2Url
		{
			get
			{
				var result = string.Empty;
				if (!string.IsNullOrWhiteSpace(this.Attachment2))
				{
					result = string.Format(FileBaseUrl, this.Id.ToString(), "2", "1");
				}
				return (result);
			}
		}
		public string Attachment3Url
		{
			get
			{
				var result = string.Empty;
				if (!string.IsNullOrWhiteSpace(this.Attachment3))
				{
					result = string.Format(FileBaseUrl, this.Id.ToString(), "3", "1");
				}
				return (result);
			}
		}

		private readonly string FileBaseUrl = @"https://service.alphaess.com/FileDownlod/GetFile?id={0}&no={1}&type={2}";
		public ComplaintsDto() { }
	}
}
