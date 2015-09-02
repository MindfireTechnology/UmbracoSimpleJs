using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbracoSimpleJsProcessor
{
	public class PaymentInfo
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostCodeOrZip { get; set; }
		public string Country { get; set; }
		public string Email { get; set; }

		public string NameOnCard { get; set; }
		public string CardNumber { get; set; }
		public string CVV { get; set; }
		public int ExpirationMonth { get; set; }
		public int ExpirationYear { get; set; }

		public decimal ChargeAmount { get; set; }
		public string Currency { get; set; }
		public string Description { get; set; }
		public string OrderRefNumber { get; set; }
	}
}
