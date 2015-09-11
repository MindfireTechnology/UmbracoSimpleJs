using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocxRecieptBuilder.Models
{
	public class Reciept
	{
		public string Date { get; set; }
		public string InvoiceNumber { get; set; }
		public int Amount { get; set; }
		public string EntrantID { get; set; }
		public string BilledName { get; set; }
		public string CompanyName { get; set; }
		public string PaymentMethod { get; set; }
		public string LastFourOfCard { get; set; }
		public string TransactionNumber { get; set; }
		public string EntrantName { get; set; }
		public string Address { get; set; }
		public string StateProvince { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string CompetitionName { get; set; }
		public string Quantity { get; set; }
		public string EntryName { get; set; }
		public Category Cat1 { get; set; }
		public Category Cat2 { get; set; }
		public Category Cat3 { get; set; }
		public Category Cat4 { get; set; }
		public Category Cat5 { get; set; }


	}
}
