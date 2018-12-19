namespace AlphaEss.Api_V2.Infrastructure
{
	public interface IEmailService
	{
		void SendEmail(string to, string title, string body);
	}
}
