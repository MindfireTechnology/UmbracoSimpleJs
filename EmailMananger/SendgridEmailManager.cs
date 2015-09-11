using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Threading.Tasks;

namespace EmailManager
{
    public class SendgridEmailManager
    {
		
		public void SendEmail(string FromEmail, string ToEmail, string Subject, string body, Stream attachment = null)
		{

			EmailSendService eservice = new EmailSendService();
			eservice.SendEmail(FromEmail, ToEmail, Subject, body, attachment);
		}
    }
}
