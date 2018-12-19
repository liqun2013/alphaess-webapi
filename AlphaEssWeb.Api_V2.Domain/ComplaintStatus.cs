namespace AlphaEssWeb.Api_V2.Domain
{
	public enum ComplaintStatus
	{
		//客诉状态: Open(刚提交的), Accepted(已接受), Processing(处理中的), ToBeVerified(待验证), Verification(验证中的), VerificationCompleted(验证完成), ReOpen(重新打开的), Completed(已完成的), Evaluated(已评价的)						
		Open,
		Accepted,
		Processing,
		ToBeVerified,
		Verification,
		VerificationCompleted,
		ReOpen,
		Completed,
		Evaluated
	}

	public enum ComplaintsType
	{
		title_inverter,
		title_battery,
		lab_meter,
		Backup_Box,
		EMS,
		lab_monitoring,
		APP,
		lab_other
	}
}
