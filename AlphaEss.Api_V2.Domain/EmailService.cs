using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AlphaEss.Api_V2.Infrastructure
{
	public class EmailService : IEmailService
	{
		public void SendEmail(string to, string title, string body)
		{
			if (!string.IsNullOrWhiteSpace(to))
			{
				string fromEmail = ConfigurationManager.AppSettings["Mail_From_adress"];
				string fromEmailAccount = ConfigurationManager.AppSettings["Mail_UserName"];
				string fromEmailPwd = ConfigurationManager.AppSettings["Mail_Password"];

				string mailHost = ConfigurationManager.AppSettings["Mail_Host"];
				var mailHostPort = Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Host_Port"]);

				MailAddress from = new MailAddress(fromEmail, fromEmailAccount);
				using (MailMessage mail = new MailMessage()
				{
					Subject = title,
					From = from,
					Body = body,
					IsBodyHtml = false,
					BodyEncoding = System.Text.Encoding.UTF8,
					DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess
				})
				{
					var client = new SmtpClient()
					{
						Host = mailHost,
						Port = mailHostPort,
						UseDefaultCredentials = true,
						Credentials = new System.Net.NetworkCredential(fromEmail, fromEmailPwd),
						DeliveryMethod = SmtpDeliveryMethod.Network,
						EnableSsl = false
					};

					mail.To.Add(to);

					try
					{
						client.Send(mail);
					}
					catch (SmtpException)
					{
					}
				}
			}
		}
	}
}
