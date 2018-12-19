namespace AlphaEss.Api_V2.Infrastructure
{
	public interface IAlphaRemotingService
	{
		bool SendCommand(string sn, string cmd, object[] args);
		bool SendBatchCommand(string id, string cmd, object[] args);
	}
}
