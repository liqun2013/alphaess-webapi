using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class ComplaintsExtensions
	{
		public static ComplaintsDto ToComplaintsDto(this Complaints c)
		{
			return new ComplaintsDto
			{
				Area = c.Area, Attachment = c.Attachment, Attachment2 = c.Attachment2, Attachment3 = c.Attachment3,
				ComplaintsType = c.StrComplaintsType, ContactNumber = c.ContactNumber, CreateTime = c.CreateTime, Creator = c.Creator,
				CurrentProcessor = c.CurrentProcessor, Description = c.Description, Email = c.Email, Id = c.Key, IsDelete = c.IsDelete,
				ReceiveTime = c.ReceiveTime, Recipient = c.Recipient, SysSn = c.SysSn, Title = c.Title, UpdateTime = c.UpdateTime, OnsiteHandler = c.OnsiteHandler, ProcessingPriority = c.ProcessingPriority ?? 0, Status = c.StrStatus
			};
		}
	}
}
