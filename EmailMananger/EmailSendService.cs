using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
namespace EmailManager
{
	public class EmailSendService
	{
		public void SendEmail(string from, string recipient, string subject, string body, Stream attachment = null)
		{

			var client = new SmtpClient();

			client.Credentials = new NetworkCredential("auroraawards", "Mindfire2015!");
			client.Host = "smtp.sendgrid.net";
			client.Port = 587;

			var message = new MailMessage(from, recipient);
			//var message = new SendGridMessage();
			
			message.Subject = subject;
			message.Body = body;


			if (attachment != null)
				message.Attachments.Add(new System.Net.Mail.Attachment(attachment, "reciept.pdf"));

			client.Send(message);

		}
	}
}